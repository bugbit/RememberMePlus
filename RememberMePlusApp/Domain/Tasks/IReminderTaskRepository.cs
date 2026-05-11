namespace RememberMePlusApp.Domain.Tasks;

public interface IReminderTaskRepository
{
    Task<ReminderTask?> GetByIdAsync(ReminderTaskId id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ReminderTask>> GetActiveDueUntilAsync(DateTimeOffset dueUntil, CancellationToken cancellationToken = default);

    Task SaveAsync(ReminderTask task, CancellationToken cancellationToken = default);
}
