using Dapper;

namespace RememberMePlusApp.Infrastructure.Data;

public sealed class TaskRepository : ITaskRepository
{
    public async Task<IReadOnlyList<TaskItemRecord>> GetPendingTodayOrOverdueAsync(IUnitOfWork unitOfWork, CancellationToken cancellationToken = default)
    {
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

        var rows = await unitOfWork.Connection.QueryAsync<TaskItemRecord>(
            new CommandDefinition(query, transaction: unitOfWork.Transaction, cancellationToken: cancellationToken));

        return rows.AsList();
    }

    public async Task CompleteAsync(long taskId, IUnitOfWork unitOfWork, CancellationToken cancellationToken = default)
    {
        const string command = """
            UPDATE Task
            SET is_active = 0,
                date_due_at_last = date_due_at
            WHERE id_task = @TaskId;
            """;

        await unitOfWork.Connection.ExecuteAsync(new CommandDefinition(
            command,
            parameters: new { TaskId = taskId },
            transaction: unitOfWork.Transaction,
            cancellationToken: cancellationToken));
    }
}
