using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RememberMePlusApp.Application.Home;
using RememberMePlusApp.Application.Time;
using RememberMePlusApp.Infrastructure.Data;

namespace RememberMePlusApp.ViewModels;

public sealed class MainPageViewModel : INotifyPropertyChanged
{
    private readonly IHomeDashboardService _homeDashboardService;
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly ITaskRepository _taskRepository;
    private readonly IAppClock _appClock;
    private readonly HashSet<long> _temporarilyIgnoredTaskIds = [];

    private bool _isLoading;
    private DateTime _currentTime = DateTime.Now;
    private int _relativeOffsetMinutes;
    private int _snoozeMinutes;

    public MainPageViewModel(
        IHomeDashboardService homeDashboardService,
        IUnitOfWorkFactory unitOfWorkFactory,
        ITaskRepository taskRepository,
        IAppClock appClock)
    {
        _homeDashboardService = homeDashboardService;
        _unitOfWorkFactory = unitOfWorkFactory;
        _taskRepository = taskRepository;
        _appClock = appClock;

        RefreshCommand = new Command(async () => await LoadAsync());
        CompleteTaskCommand = new Command<HomeTaskItemViewModel>(OnCompleteTask);
        SnoozeTaskCommand = new Command<HomeTaskItemViewModel>(OnSnoozeTask);
        IgnoreTaskCommand = new Command<HomeTaskItemViewModel>(OnIgnoreTask);

        OverdueTasks.CollectionChanged += (_, _) => NotifyDashboardCounts();
        DueSoonTasks.CollectionChanged += (_, _) => NotifyDashboardCounts();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<HomeTaskItemViewModel> OverdueTasks { get; } = [];

    public ObservableCollection<HomeTaskItemViewModel> DueSoonTasks { get; } = [];

    public ICommand RefreshCommand { get; }

    public ICommand CompleteTaskCommand { get; }

    public ICommand SnoozeTaskCommand { get; }

    public ICommand IgnoreTaskCommand { get; }

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
            OnPropertyChanged(nameof(HasNoUrgentTasks));
        }
    }

    public DateTime CurrentTime
    {
        get => _currentTime;
        private set
        {
            if (_currentTime == value)
            {
                return;
            }

            _currentTime = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CurrentTimeText));
        }
    }

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
            OnPropertyChanged(nameof(DueSoonSubtitle));
        }
    }

    public string CurrentTimeText => $"Ahora: {CurrentTime:dd/MM/yyyy HH:mm}";

    public string OverdueTitle => $"{OverdueTasks.Count} tareas vencidas";

    public string DueSoonTitle => $"{DueSoonTasks.Count} a punto de vencer";

    public string DueSoonSubtitle => $"Dentro de los próximos {RelativeOffsetMinutes} minutos";

    public bool HasOverdueTasks => OverdueTasks.Count > 0;

    public bool HasDueSoonTasks => DueSoonTasks.Count > 0;

    public bool HasNoUrgentTasks => !HasOverdueTasks && !HasDueSoonTasks && !IsLoading;

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        IsLoading = true;

        try
        {
            var dashboard = await _homeDashboardService.GetAsync(_temporarilyIgnoredTaskIds, cancellationToken);

            CurrentTime = dashboard.Now;
            RelativeOffsetMinutes = dashboard.RelativeOffsetMinutes;
            _snoozeMinutes = dashboard.SnoozeMinutes;

            ReplaceTasks(OverdueTasks, dashboard.OverdueTasks.Select(CreateHomeTaskItem).ToList());
            ReplaceTasks(DueSoonTasks, dashboard.DueSoonTasks.Select(CreateHomeTaskItem).ToList());
        }
        finally
        {
            IsLoading = false;
            NotifyDashboardCounts();
        }
    }

    private static HomeTaskItemViewModel CreateHomeTaskItem(HomeDashboardTaskDto task)
    {
        return new HomeTaskItemViewModel(
            task.Id,
            task.Title,
            task.DueAt,
            task.IsOverdue,
            task.IsDueInLessThan15Minutes,
            FormatDueAt(task.DueAt),
            FormatRelativeTime(task.Delta));
    }

    private async void OnCompleteTask(HomeTaskItemViewModel? task)
    {
        if (task is null)
        {
            return;
        }

        await using var unitOfWork = await _unitOfWorkFactory.CreateAsync(useTransaction: true);
        await _taskRepository.CompleteAsync(task.Id, unitOfWork);
        await unitOfWork.CommitAsync();

        RemoveTask(task.Id);
    }

    private async void OnSnoozeTask(HomeTaskItemViewModel? task)
    {
        if (task is null)
        {
            return;
        }

        var newDueAt = _appClock.Now.AddMinutes(_snoozeMinutes);
        await using var unitOfWork = await _unitOfWorkFactory.CreateAsync(useTransaction: true);
        await _taskRepository.SnoozeAsync(task.Id, newDueAt, unitOfWork);
        await unitOfWork.CommitAsync();

        await LoadAsync();
    }

    private void OnIgnoreTask(HomeTaskItemViewModel? task)
    {
        if (task is null)
        {
            return;
        }

        _temporarilyIgnoredTaskIds.Add(task.Id);
        RemoveTask(task.Id);
    }

    private void RemoveTask(long taskId)
    {
        RemoveTaskFrom(OverdueTasks, taskId);
        RemoveTaskFrom(DueSoonTasks, taskId);
        NotifyDashboardCounts();
    }

    private static void RemoveTaskFrom(ObservableCollection<HomeTaskItemViewModel> tasks, long taskId)
    {
        var task = tasks.FirstOrDefault(item => item.Id == taskId);
        if (task is not null)
        {
            tasks.Remove(task);
        }
    }

    private static void ReplaceTasks(
        ObservableCollection<HomeTaskItemViewModel> target,
        IReadOnlyList<HomeTaskItemViewModel> source)
    {
        target.Clear();
        foreach (var task in source)
        {
            target.Add(task);
        }
    }

    private static string FormatDueAt(DateTime dueAt)
    {
        return dueAt.ToString("dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture);
    }

    private static string FormatRelativeTime(TimeSpan delta)
    {
        var absoluteDelta = delta.Duration();
        var parts = new List<string>();

        if (absoluteDelta.Days > 0)
        {
            parts.Add(absoluteDelta.Days == 1 ? "1 día" : $"{absoluteDelta.Days} días");
        }

        if (absoluteDelta.Hours > 0)
        {
            parts.Add(absoluteDelta.Hours == 1 ? "1 hora" : $"{absoluteDelta.Hours} horas");
        }

        if (absoluteDelta.Days == 0 && absoluteDelta.Minutes > 0)
        {
            parts.Add(absoluteDelta.Minutes == 1 ? "1 minuto" : $"{absoluteDelta.Minutes} minutos");
        }

        var text = parts.Count == 0 ? "menos de 1 minuto" : string.Join(" y ", parts.Take(2));
        return delta < TimeSpan.Zero ? $"Vencida hace {text}" : $"Vence en {text}";
    }

    private void NotifyDashboardCounts()
    {
        OnPropertyChanged(nameof(OverdueTitle));
        OnPropertyChanged(nameof(DueSoonTitle));
        OnPropertyChanged(nameof(HasOverdueTasks));
        OnPropertyChanged(nameof(HasDueSoonTasks));
        OnPropertyChanged(nameof(HasNoUrgentTasks));
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public sealed class HomeTaskItemViewModel(
    long id,
    string title,
    DateTime dueAt,
    bool isOverdue,
    bool isDueInLessThan15Minutes,
    string dueAtText,
    string relativeTimeText)
{
    public long Id { get; } = id;

    public string Title { get; } = title;

    public DateTime DueAt { get; } = dueAt;

    public bool IsOverdue { get; } = isOverdue;

    public bool IsDueInLessThan15Minutes { get; } = isDueInLessThan15Minutes;

    public string DueAtText { get; } = dueAtText;

    public string RelativeTimeText { get; } = relativeTimeText;

    public string AttentionIcon => IsOverdue ? "⚠️" : IsDueInLessThan15Minutes ? "🔥" : "⏰";

}
