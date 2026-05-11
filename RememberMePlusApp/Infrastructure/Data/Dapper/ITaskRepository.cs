using RememberMePlusApp.Application.Alarms;
using RememberMePlusApp.Domain.Tasks;

namespace RememberMePlusApp.Infrastructure.Data;

public interface ITaskRepository
{
    Task<IReadOnlyList<ReminderTask>> GetPendingTodayOrOverdueAsync(IUnitOfWork unitOfWork, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReminderTask>> GetHomeAttentionTasksAsync(DateTime dueUntil, IUnitOfWork unitOfWork, CancellationToken cancellationToken = default);

    Task CompleteAsync(long taskId, IUnitOfWork unitOfWork, CancellationToken cancellationToken = default);

    Task SnoozeAsync(long taskId, DateTime dueAt, IUnitOfWork unitOfWork, CancellationToken cancellationToken = default);

    Task AddNonRecurringAsync(string title, DateTime dueDate, IUnitOfWork unitOfWork, CancellationToken cancellationToken = default);

    Task NormalizeOverdueNotificationsAsync(DateTime notifyAt, IUnitOfWork unitOfWork, CancellationToken cancellationToken = default);

    Task<ScheduledReminderTask?> GetNextToNotifyAsync(IUnitOfWork unitOfWork, CancellationToken cancellationToken = default);

    Task<ScheduledReminderTask?> GetByIdForAlarmAsync(long taskId, IUnitOfWork unitOfWork, CancellationToken cancellationToken = default);
}
