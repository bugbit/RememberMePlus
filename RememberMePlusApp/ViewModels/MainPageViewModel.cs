using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RememberMePlusApp.Infrastructure.Data;

namespace RememberMePlusApp.ViewModels;

public sealed class MainPageViewModel : INotifyPropertyChanged
{
    private static readonly CultureInfo DisplayCulture = CultureInfo.GetCultureInfo("es-ES");

    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly ITaskRepository _taskRepository;
    private readonly IAppRepository _appRepository;

    private bool _isLoading;
    private int _relativeOffsetMinutes = 120;
    private int _snoozeMinutes = 10;
    private string _lastUpdatedText = string.Empty;
    private double _pulseOpacity = 1;

    public MainPageViewModel(
        IUnitOfWorkFactory unitOfWorkFactory,
        ITaskRepository taskRepository,
        IAppRepository appRepository)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _taskRepository = taskRepository;
        _appRepository = appRepository;

        CompleteTaskCommand = new Command<HomeTaskItemViewModel>(OnCompleteTask);
        SnoozeTaskCommand = new Command<HomeTaskItemViewModel>(OnSnoozeTask);
        IgnoreTemporarilyCommand = new Command<HomeTaskItemViewModel>(OnIgnoreTemporarily);
        RefreshCommand = new Command(async () => await LoadAsync());

        OverdueTasks.CollectionChanged += (_, _) => NotifyCountersChanged();
        DueSoonTasks.CollectionChanged += (_, _) => NotifyCountersChanged();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<HomeTaskItemViewModel> OverdueTasks { get; } = [];

    public ObservableCollection<HomeTaskItemViewModel> DueSoonTasks { get; } = [];

    public ICommand CompleteTaskCommand { get; }

    public ICommand SnoozeTaskCommand { get; }

    public ICommand IgnoreTemporarilyCommand { get; }

    public ICommand RefreshCommand { get; }

    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            if (_isLoading == value)
            {
                return;
            }

            _isLoading = value;
            OnPropertyChanged();
        }
    }

    public int OverdueCount => OverdueTasks.Count;

    public int DueSoonCount => DueSoonTasks.Count;

    public bool HasOverdueTasks => OverdueTasks.Count > 0;

    public bool HasDueSoonTasks => DueSoonTasks.Count > 0;

    public bool HasNoAttentionTasks => OverdueTasks.Count == 0 && DueSoonTasks.Count == 0 && !IsLoading;

    public int RelativeOffsetMinutes
    {
        get => _relativeOffsetMinutes;
        private set
        {
            if (_relativeOffsetMinutes == value)
            {
                return;
            }

            _relativeOffsetMinutes = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(AttentionWindowText));
        }
    }

    public string AttentionWindowText => $"Ventana de próximo vencimiento: {RelativeOffsetMinutes} min";

    public string LastUpdatedText
    {
        get => _lastUpdatedText;
        private set
        {
            if (_lastUpdatedText == value)
            {
                return;
            }

            _lastUpdatedText = value;
            OnPropertyChanged();
        }
    }

    public double PulseOpacity
    {
        get => _pulseOpacity;
        private set
        {
            if (Math.Abs(_pulseOpacity - value) < 0.01)
            {
                return;
            }

            _pulseOpacity = value;
            OnPropertyChanged();
        }
    }

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        IsLoading = true;

        try
        {
            var now = DateTime.Now;
            await using var unitOfWork = await _unitOfWorkFactory.CreateAsync(useTransaction: false, cancellationToken);
            var app = await _appRepository.GetFirstAsync(unitOfWork, cancellationToken);

            RelativeOffsetMinutes = app?.RelativeOffsetMinutes > 0 ? Convert.ToInt32(app.RelativeOffsetMinutes) : 120;
            _snoozeMinutes = app?.SnoozeMinutes > 0 ? Convert.ToInt32(app.SnoozeMinutes) : 10;

            var attentionLimit = now.AddMinutes(RelativeOffsetMinutes);
            var tasks = await _taskRepository.GetHomeAttentionTasksAsync(attentionLimit, unitOfWork, cancellationToken);

            OverdueTasks.Clear();
            DueSoonTasks.Clear();

            foreach (var task in tasks)
            {
                var dueAt = ParseDueAt(task.DateDueAt);
                if (dueAt is null)
                {
                    continue;
                }

                var item = HomeTaskItemViewModel.Create(task.IdTask, task.Title, dueAt.Value, now);
                if (item.IsOverdue)
                {
                    OverdueTasks.Add(item);
                    continue;
                }

                DueSoonTasks.Add(item);
            }

            LastUpdatedText = $"Actualizado: {now.ToString("HH:mm", DisplayCulture)}";
        }
        finally
        {
            IsLoading = false;
            NotifyCountersChanged();
        }
    }

    public void ToggleAttentionPulse()
    {
        PulseOpacity = PulseOpacity >= 1 ? 0.45 : 1;
    }

    private async void OnCompleteTask(HomeTaskItemViewModel? task)
    {
        if (task is null)
        {
            return;
        }

        await using var unitOfWork = await _unitOfWorkFactory.CreateAsync();
        await _taskRepository.CompleteAsync(task.Id, unitOfWork);
        await unitOfWork.CommitAsync();

        RemoveTask(task);
    }

    private async void OnSnoozeTask(HomeTaskItemViewModel? task)
    {
        if (task is null)
        {
            return;
        }

        var snoozedDueAt = DateTime.Now.AddMinutes(_snoozeMinutes);
        await using var unitOfWork = await _unitOfWorkFactory.CreateAsync();
        await _taskRepository.SnoozeAsync(task.Id, snoozedDueAt, unitOfWork);
        await unitOfWork.CommitAsync();

        await LoadAsync();
    }

    private void OnIgnoreTemporarily(HomeTaskItemViewModel? task)
    {
        if (task is null)
        {
            return;
        }

        RemoveTask(task);
    }

    private void RemoveTask(HomeTaskItemViewModel task)
    {
        if (!OverdueTasks.Remove(task))
        {
            DueSoonTasks.Remove(task);
        }

        NotifyCountersChanged();
    }

    private static DateTime? ParseDueAt(string dueAt)
    {
        string[] formats = ["yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd"];
        if (DateTime.TryParseExact(dueAt, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var parsed))
        {
            return parsed;
        }

        return DateTime.TryParse(dueAt, DisplayCulture, DateTimeStyles.AssumeLocal, out parsed) ? parsed : null;
    }

    private void NotifyCountersChanged()
    {
        OnPropertyChanged(nameof(OverdueCount));
        OnPropertyChanged(nameof(DueSoonCount));
        OnPropertyChanged(nameof(HasOverdueTasks));
        OnPropertyChanged(nameof(HasDueSoonTasks));
        OnPropertyChanged(nameof(HasNoAttentionTasks));
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public sealed class HomeTaskItemViewModel : INotifyPropertyChanged
{
    private static readonly CultureInfo DisplayCulture = CultureInfo.GetCultureInfo("es-ES");

    private HomeTaskItemViewModel(
        long id,
        string title,
        DateTime dueAt,
        bool isOverdue,
        bool isCriticalSoon,
        string dueAtText,
        string timeStatusText,
        string headlineText)
    {
        Id = id;
        Title = title;
        DueAt = dueAt;
        IsOverdue = isOverdue;
        IsCriticalSoon = isCriticalSoon;
        DueAtText = dueAtText;
        TimeStatusText = timeStatusText;
        HeadlineText = headlineText;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public long Id { get; }

    public string Title { get; }

    public DateTime DueAt { get; }

    public bool IsOverdue { get; }

    public bool IsCriticalSoon { get; }

    public bool RequiresPulse => IsOverdue || IsCriticalSoon;

    public string DueAtText { get; }

    public string TimeStatusText { get; }

    public string HeadlineText { get; }

    public static HomeTaskItemViewModel Create(long id, string title, DateTime dueAt, DateTime now)
    {
        var remaining = dueAt - now;
        var isOverdue = remaining <= TimeSpan.Zero;
        var isCriticalSoon = !isOverdue && remaining <= TimeSpan.FromMinutes(15);
        var dueAtText = dueAt.ToString("dd/MM/yyyy HH:mm", DisplayCulture);
        var timeStatusText = isOverdue ? $"Vencida hace {FormatDuration(now - dueAt)}" : $"Faltan {FormatDuration(remaining)}";
        var headlineText = isOverdue ? "Requiere acción ahora" : isCriticalSoon ? "Vence en menos de 15 min" : "Próximo vencimiento";

        return new HomeTaskItemViewModel(id, title, dueAt, isOverdue, isCriticalSoon, dueAtText, timeStatusText, headlineText);
    }

    private static string FormatDuration(TimeSpan duration)
    {
        var totalMinutes = Math.Max(1, Convert.ToInt32(Math.Ceiling(duration.TotalMinutes)));
        var days = totalMinutes / 1440;
        var hours = totalMinutes % 1440 / 60;
        var minutes = totalMinutes % 60;

        if (days > 0)
        {
            return hours > 0 ? $"{days} d {hours} h" : $"{days} d";
        }

        if (hours > 0)
        {
            return minutes > 0 ? $"{hours} h {minutes} min" : $"{hours} h";
        }

        return $"{minutes} min";
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
