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
}
