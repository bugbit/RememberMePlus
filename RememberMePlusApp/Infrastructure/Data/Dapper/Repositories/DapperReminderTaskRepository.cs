using System.Globalization;
using Dapper;
using RememberMePlusApp.Domain.Tasks;
using RememberMePlusApp.Infrastructure.Data.Abstractions;
using RememberMePlusApp.Infrastructure.Data.Dapper.Mappers;
using RememberMePlusApp.Infrastructure.Data.Dapper.Models;

namespace RememberMePlusApp.Infrastructure.Data.Dapper.Repositories;

public sealed class DapperReminderTaskRepository(IDbConnectionFactory connectionFactory) : IReminderTaskRepository
{
    private readonly IDbConnectionFactory _connectionFactory = connectionFactory;

    public async Task<ReminderTask?> GetByIdAsync(ReminderTaskId id, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT Id, Title, DueAt, Priority, IsActive, ReactivatedAt, TaskPostponeMinutes
            FROM ReminderTasks
            WHERE Id = @Id
            LIMIT 1;
            """;

        using var connection = _connectionFactory.CreateConnection();
        var command = new CommandDefinition(sql, new { Id = id.Value }, cancellationToken: cancellationToken);
        var dataModel = await connection.QuerySingleOrDefaultAsync<ReminderTaskDataModel>(command);

        return dataModel is null ? null : ReminderTaskDataMapper.ToDomain(dataModel);
    }

    public async Task<IReadOnlyCollection<ReminderTask>> GetActiveDueUntilAsync(DateTimeOffset dueUntil, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT Id, Title, DueAt, Priority, IsActive, ReactivatedAt, TaskPostponeMinutes
            FROM ReminderTasks
            WHERE IsActive = 1 AND DueAt <= @DueUntil
            ORDER BY DueAt ASC, Priority DESC;
            """;

        using var connection = _connectionFactory.CreateConnection();
        var command = new CommandDefinition(sql, new { DueUntil = dueUntil.ToString("O", CultureInfo.InvariantCulture) }, cancellationToken: cancellationToken);
        var dataModels = await connection.QueryAsync<ReminderTaskDataModel>(command);

        return dataModels.Select(ReminderTaskDataMapper.ToDomain).ToArray();
    }

    public async Task SaveAsync(ReminderTask task, CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO ReminderTasks (Id, Title, DueAt, Priority, IsActive, ReactivatedAt, TaskPostponeMinutes)
            VALUES (@Id, @Title, @DueAt, @Priority, @IsActive, @ReactivatedAt, @TaskPostponeMinutes)
            ON CONFLICT(Id) DO UPDATE SET
                Title = excluded.Title,
                DueAt = excluded.DueAt,
                Priority = excluded.Priority,
                IsActive = excluded.IsActive,
                ReactivatedAt = excluded.ReactivatedAt,
                TaskPostponeMinutes = excluded.TaskPostponeMinutes;
            """;

        using var connection = _connectionFactory.CreateConnection();
        var dataModel = ReminderTaskDataMapper.ToDataModel(task);
        var command = new CommandDefinition(sql, dataModel, cancellationToken: cancellationToken);

        await connection.ExecuteAsync(command);
    }
}
