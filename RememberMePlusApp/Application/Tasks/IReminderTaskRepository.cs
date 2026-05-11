using RememberMePlusApp.Domain.Tasks;

namespace RememberMePlusApp.Application.Tasks;

public interface IReminderTaskRepository
{
    Task<ReminderTask?> GetByIdAsync(ReminderTaskId id, CancellationToken cancellationToken);

    Task<IReadOnlyList<ReminderTask>> ListActiveDueAsync(DateTimeOffset now, CancellationToken cancellationToken);

    Task SaveAsync(ReminderTask reminderTask, CancellationToken cancellationToken);

    Task DeleteAsync(ReminderTaskId id, CancellationToken cancellationToken);
}
