using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        await using var unitOfWork = await _unitOfWorkFactory.CreateAsync();
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
    private bool _isCompleted;

    public event PropertyChangedEventHandler? PropertyChanged;

    public long Id { get; } = id;

    public string Title { get; } = title;

    public string DueAt { get; } = dueAt;

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

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
