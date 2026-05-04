using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Windows.Input;
using RememberMePlusApp.Infrastructure.Data;

namespace RememberMePlusApp.ViewModels;

public sealed class MainPageViewModel : INotifyPropertyChanged
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly ITaskRepository _taskRepository;

    private bool _isLoading;

    public MainPageViewModel(IUnitOfWorkFactory unitOfWorkFactory, ITaskRepository taskRepository)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _taskRepository = taskRepository;
        CompleteTaskCommand = new Command<PendingTaskItemViewModel>(OnCompleteTask);
        PendingTasks.CollectionChanged += (_, _) => OnPropertyChanged(nameof(HasPendingTasks));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<PendingTaskItemViewModel> PendingTasks { get; } = [];

    public ICommand CompleteTaskCommand { get; }

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
            OnPropertyChanged(nameof(HasPendingTasks));
        }
    }

    public bool HasPendingTasks => PendingTasks.Count > 0;

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        IsLoading = true;

        await using var unitOfWork = await _unitOfWorkFactory.CreateAsync(useTransaction: false, cancellationToken);
        var pendingTasks = await _taskRepository.GetPendingTodayOrOverdueAsync(unitOfWork, cancellationToken);

        PendingTasks.Clear();
        foreach (var pendingTask in pendingTasks)
        {
            PendingTasks.Add(new PendingTaskItemViewModel(
                pendingTask.IdTask,
                pendingTask.Title,
                pendingTask.DateDueAt));
        }

        IsLoading = false;
    }

    private async void OnCompleteTask(PendingTaskItemViewModel? task)
    {
        if (task is null || !task.IsCompleted)
        {
            return;
        }

        await using var unitOfWork = await _unitOfWorkFactory.CreateAsync(useTransaction: false);

        // TODO: De momento solo marca la tarea como completada. Falta implementar la lógica de cuando se completa una tarea recurrente
        // se tiene que calcular la fecha de vencimiento según recurrencia.
        await _taskRepository.CompleteAsync(task.Id, unitOfWork);
        await unitOfWork.CommitAsync();

        PendingTasks.Remove(task);
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public sealed class PendingTaskItemViewModel(long id, string title, string dueAt) : INotifyPropertyChanged
{
    private static readonly TimeSpan NearDueThreshold = TimeSpan.FromHours(2);
    private bool _isCompleted;

    public event PropertyChangedEventHandler? PropertyChanged;

    public long Id { get; } = id;

    public string Title { get; } = title;

    public string DueAt { get; } = dueAt;

    public bool IsOverdue => TryParseDueAt(out var dueAtLocal) && dueAtLocal <= DateTime.Now;

    public bool IsNearDue => TryParseDueAt(out var dueAtLocal)
        && dueAtLocal > DateTime.Now
        && dueAtLocal <= DateTime.Now.Add(NearDueThreshold);

    public Color TaskTextColor
    {
        get
        {
            if (IsOverdue)
            {
                return Color.FromArgb("#B91C1C");
            }

            if (IsNearDue)
            {
                return Color.FromArgb("#C2410C");
            }

            return Colors.Black;
        }
    }

    public bool IsCompleted
    {
        get => _isCompleted;
        set
        {
            if (_isCompleted == value)
            {
                return;
            }

            _isCompleted = value;
            OnPropertyChanged();
        }
    }


    private bool TryParseDueAt(out DateTime dueAtLocal)
    {
        return DateTime.TryParseExact(
            DueAt,
            "yyyy-MM-dd HH:mm:ss",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeLocal,
            out dueAtLocal);
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
