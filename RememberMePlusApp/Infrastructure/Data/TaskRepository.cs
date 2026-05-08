namespace RememberMePlusApp.Infrastructure.Data;

public sealed class TaskRepository : ITaskRepository
{
    public async Task<IReadOnlyList<TaskItemRecord>> GetPendingTodayOrOverdueAsync(IUnitOfWork unitOfWork, CancellationToken cancellationToken = default)
    {
        if (unitOfWork is not ISqlExecutor sqlExecutor)
        {
            return [];
        }

        const string query = """
            SELECT
                id_task AS IdTask,
                title AS Title,
                date_due_at AS DateDueAt
            FROM Task
            WHERE is_active = 1
              AND date(date_due_at) <= date('now', 'localtime')
            ORDER BY date_due_at ASC;
            """;

        var rows = await sqlExecutor.QueryAsync<TaskItemRecord>(query, cancellationToken: cancellationToken);

        return rows.ToList();
    }

    public async Task<IReadOnlyList<TaskItemRecord>> GetHomeAttentionTasksAsync(DateTime dueUntil, IUnitOfWork unitOfWork, CancellationToken cancellationToken = default)
    {
        if (unitOfWork is not ISqlExecutor sqlExecutor)
        {
            return [];
        }

        const string query = """
            SELECT
                id_task AS IdTask,
                title AS Title,
                date_due_at AS DateDueAt
            FROM Task
            WHERE is_active = 1
              AND datetime(date_due_at) <= datetime(@DueUntil)
            ORDER BY date_due_at ASC;
            """;

        var rows = await sqlExecutor.QueryAsync<TaskItemRecord>(
            query,
            new { DueUntil = dueUntil.ToString("yyyy-MM-dd HH:mm:ss") },
            cancellationToken);

        return rows.ToList();
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
}
