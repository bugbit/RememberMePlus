namespace RememberMePlusApp.Infrastructure.Data;

public interface ITaskRepository
{
    Task<IReadOnlyList<TaskItemRecord>> GetPendingAsync(IUnitOfWork unitOfWork, CancellationToken cancellationToken = default);

    Task CompleteAsync(long taskId, IUnitOfWork unitOfWork, CancellationToken cancellationToken = default);
}
