namespace RememberMePlusApp.Infrastructure.Data;

public interface ITaskRepository
{
    Task<IReadOnlyList<TaskItemRecord>> GetPendingTodayOrOverdueAsync(IUnitOfWork unitOfWork, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TaskItemRecord>> GetActiveDueBeforeAsync(DateTime dueBeforeOrEqual, IUnitOfWork unitOfWork, CancellationToken cancellationToken = default);

    Task CompleteAsync(long taskId, IUnitOfWork unitOfWork, CancellationToken cancellationToken = default);

    Task SnoozeAsync(long taskId, DateTime newDueAt, IUnitOfWork unitOfWork, CancellationToken cancellationToken = default);

    Task AddNonRecurringAsync(string title, DateTime dueDate, IUnitOfWork unitOfWork, CancellationToken cancellationToken = default);
}
