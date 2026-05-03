using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RememberMePlusApp.Infrastructure.Data;

namespace RememberMePlusApp.ViewModels;

public sealed class AddTaskPageViewModel(IUnitOfWorkFactory unitOfWorkFactory, ITaskRepository taskRepository) : INotifyPropertyChanged
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory = unitOfWorkFactory;
    private readonly ITaskRepository _taskRepository = taskRepository;

    private string _title = string.Empty;
    private DateTime _dueDate = DateTime.Today;
    private bool _isSaving;
    private Command? _saveCommand;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ICommand SaveCommand => _saveCommand ??= new Command(async () => await SaveAsync(), CanSave);

    public string Title
    {
        get => _title;
        set
        {
            if (_title == value)
            {
                return;
            }

            _title = value;
            OnPropertyChanged();
            ((Command)SaveCommand).ChangeCanExecute();
        }
    }

    public DateTime DueDate
    {
        get => _dueDate;
        set
        {
            if (_dueDate == value)
            {
                return;
            }

            _dueDate = value;
            OnPropertyChanged();
        }
    }

    public bool IsSaving
    {
        get => _isSaving;
        private set
        {
            if (_isSaving == value)
            {
                return;
            }

            _isSaving = value;
            OnPropertyChanged();
            ((Command)SaveCommand).ChangeCanExecute();
        }
    }

    private bool CanSave()
    {
        return !IsSaving && !string.IsNullOrWhiteSpace(Title);
    }

    private async Task SaveAsync()
    {
        if (!CanSave())
        {
            return;
        }

        IsSaving = true;

        await using var unitOfWork = await _unitOfWorkFactory.CreateAsync();
        await _taskRepository.AddNonRecurringAsync(Title.Trim(), DueDate, unitOfWork);
        await unitOfWork.CommitAsync();

        Title = string.Empty;
        DueDate = DateTime.Today;
        IsSaving = false;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
