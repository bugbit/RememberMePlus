using RememberMePlusApp.Application.Alarms;
using RememberMePlusApp.Domain.Tasks;

namespace RememberMePlusApp.Infrastructure.Data;

public sealed class DapperReminderTaskRepository : ITaskRepository
{
    public async Task<IReadOnlyList<ReminderTask>> GetPendingTodayOrOverdueAsync(IUnitOfWork unitOfWork, CancellationToken cancellationToken = default)
    {
        if (unitOfWork is not ISqlExecutor sqlExecutor)
        {
            return [];
        }

        const string query = """
            SELECT
                id_task AS IdTask,
                title AS Title,
                date_due_at AS DateDueAt,
                is_active AS IsActive,
                is_insistent AS IsInsistent
            FROM Task
            WHERE is_active = 1
              AND date(date_due_at) <= date('now', 'localtime')
            ORDER BY date_due_at ASC;
            """;

        var rows = await sqlExecutor.QueryAsync<ReminderTaskDataModel>(query, cancellationToken: cancellationToken);

        return rows.Select(ReminderTaskDataMapper.ToDomain).ToList();
    }

    public async Task<IReadOnlyList<ReminderTask>> GetHomeAttentionTasksAsync(DateTime dueUntil, IUnitOfWork unitOfWork, CancellationToken cancellationToken = default)
    {
        if (unitOfWork is not ISqlExecutor sqlExecutor)
        {
            return [];
        }

        const string query = """
            SELECT
                id_task AS IdTask,
                title AS Title,
                date_due_at AS DateDueAt,
                is_active AS IsActive,
                is_insistent AS IsInsistent
            FROM Task
            WHERE is_active = 1
              AND datetime(date_due_at) <= datetime(@DueUntil)
            ORDER BY date_due_at ASC;
            """;

        var rows = await sqlExecutor.QueryAsync<ReminderTaskDataModel>(
            query,
            new { DueUntil = dueUntil.ToString("yyyy-MM-dd HH:mm:ss") },
            cancellationToken);

        return rows.Select(ReminderTaskDataMapper.ToDomain).ToList();
    }

    public async Task CompleteAsync(long taskId, IUnitOfWork unitOfWork, CancellationToken cancellationToken = default)
    {
        if (unitOfWork is not ISqlExecutor sqlExecutor)
        {
            return;
        }

        const string command = """
            UPDATE Task
            SET date_due_at_last = date_due_at,
                date_due_at = date('now', 'localtime'),
                is_active = CASE
                    WHEN id_taskscheduler IS NULL AND id_task_event IS NULL THEN 0
                    ELSE is_active
                END
            WHERE id_task = @TaskId;
            """;

        await sqlExecutor.ExecuteAsync(command, new { TaskId = taskId }, cancellationToken);
    }

    public async Task SnoozeAsync(long taskId, DateTime dueAt, IUnitOfWork unitOfWork, CancellationToken cancellationToken = default)
    {
        if (unitOfWork is not ISqlExecutor sqlExecutor)
        {
            return;
        }

        const string command = """
            UPDATE Task
            SET date_due_at_last = date_due_at,
                date_due_at = @DateDueAt,
                datetime_notify_at = @DateDueAt
            WHERE id_task = @TaskId
              AND is_active = 1;
            """;

        await sqlExecutor.ExecuteAsync(command, new
        {
            TaskId = taskId,
            DateDueAt = dueAt.ToString("yyyy-MM-dd HH:mm:ss")
        }, cancellationToken);
    }

    public async Task AddNonRecurringAsync(string title, DateTime dueDate, IUnitOfWork unitOfWork, CancellationToken cancellationToken = default)
    {
        if (unitOfWork is not ISqlExecutor sqlExecutor)
        {
            return;
        }

        const string command = """
            INSERT INTO Task
            (title, description, id_task_group, is_insistent, datetime_create, date_due_at, date_due_at_last, datetime_notify_at, snooze_minutes, id_taskscheduler, id_task_event, is_active)
            VALUES
            (@Title, NULL, NULL, 0, datetime('now', 'localtime'), @DateDueAt, NULL, @DateDueAt, NULL, NULL, NULL, 1);
            """;

        await sqlExecutor.ExecuteAsync(command, new
        {
            Title = title,
            DateDueAt = dueDate.ToString("yyyy-MM-dd HH:mm:ss")
        }, cancellationToken);
    }

    public async Task NormalizeOverdueNotificationsAsync(DateTime notifyAt, IUnitOfWork unitOfWork, CancellationToken cancellationToken = default)
    {
        if (unitOfWork is not ISqlExecutor sqlExecutor)
        {
            return;
        }

        const string command = """
            UPDATE Task
            SET datetime_notify_at = @NotifyAt
            WHERE is_active = 1
              AND datetime_notify_at IS NOT NULL
              AND datetime(datetime_notify_at) < datetime(@NotifyAt);
            """;

        await sqlExecutor.ExecuteAsync(command, new
        {
            NotifyAt = notifyAt.ToString("yyyy-MM-dd HH:mm:ss")
        }, cancellationToken);
    }

    public async Task<ScheduledReminderTask?> GetNextToNotifyAsync(IUnitOfWork unitOfWork, CancellationToken cancellationToken = default)
    {
        if (unitOfWork is not ISqlExecutor sqlExecutor)
        {
            return null;
        }

        const string query = """
            SELECT
                id_task AS IdTask,
                title AS Title,
                date_due_at AS DateDueAt,
                datetime_notify_at AS DateTimeNotifyAt,
                snooze_minutes AS SnoozeMinutes,
                is_active AS IsActive,
                is_insistent AS IsInsistent
            FROM Task
            WHERE is_active = 1
              AND datetime_notify_at IS NOT NULL
            ORDER BY datetime(datetime_notify_at) ASC, id_task ASC
            LIMIT 1;
            """;

        var row = await sqlExecutor.QueryFirstOrDefaultAsync<ReminderTaskDataModel>(query, cancellationToken: cancellationToken);
        return row is null ? null : ReminderTaskDataMapper.ToScheduledReminder(row);
    }

    public async Task<ScheduledReminderTask?> GetByIdForAlarmAsync(long taskId, IUnitOfWork unitOfWork, CancellationToken cancellationToken = default)
    {
        if (unitOfWork is not ISqlExecutor sqlExecutor)
        {
            return null;
        }

        const string query = """
            SELECT
                id_task AS IdTask,
                title AS Title,
                date_due_at AS DateDueAt,
                datetime_notify_at AS DateTimeNotifyAt,
                snooze_minutes AS SnoozeMinutes,
                is_active AS IsActive,
                is_insistent AS IsInsistent
            FROM Task
            WHERE id_task = @TaskId
              AND is_active = 1
            LIMIT 1;
            """;

        var row = await sqlExecutor.QueryFirstOrDefaultAsync<ReminderTaskDataModel>(
            query,
            new { TaskId = taskId },
            cancellationToken);

        return row is null ? null : ReminderTaskDataMapper.ToScheduledReminder(row);
    }
}
