using System.Globalization;
using Dapper;
using RememberMePlusApp.Application.Tasks;
using RememberMePlusApp.Domain.Tasks;
using RememberMePlusApp.Infrastructure.Data.Abstractions;
using RememberMePlusApp.Infrastructure.Data.Dapper.Mappers;
using RememberMePlusApp.Infrastructure.Data.Dapper.Models;

namespace RememberMePlusApp.Infrastructure.Data.Dapper.Repositories;

public sealed class DapperReminderTaskRepository : IReminderTaskRepository
{
    private const string SelectByIdSql = """
        SELECT Id, Title, DueAt, Priority, IsActive, CompletedAt, ReactivatedFrom, CustomPostponeMinutes
        FROM ReminderTasks
        WHERE Id = @Id;
        """;

    private const string SelectActiveDueSql = """
        SELECT Id, Title, DueAt, Priority, IsActive, CompletedAt, ReactivatedFrom, CustomPostponeMinutes
        FROM ReminderTasks
        WHERE IsActive = 1
          AND CompletedAt IS NULL
          AND DueAt <= @Now
        ORDER BY Priority DESC, DueAt ASC;
        """;

    private const string UpsertSql = """
        INSERT INTO ReminderTasks (
            Id,
            Title,
            DueAt,
            Priority,
            IsActive,
            CompletedAt,
            ReactivatedFrom,
            CustomPostponeMinutes
        ) VALUES (
            @Id,
            @Title,
            @DueAt,
            @Priority,
            @IsActive,
            @CompletedAt,
            @ReactivatedFrom,
            @CustomPostponeMinutes
        )
        ON CONFLICT(Id) DO UPDATE SET
            Title = excluded.Title,
            DueAt = excluded.DueAt,
            Priority = excluded.Priority,
            IsActive = excluded.IsActive,
            CompletedAt = excluded.CompletedAt,
            ReactivatedFrom = excluded.ReactivatedFrom,
            CustomPostponeMinutes = excluded.CustomPostponeMinutes;
        """;

    private const string DeleteSql = """
        DELETE FROM ReminderTasks
        WHERE Id = @Id;
        """;

    private readonly IDbConnectionFactory _connectionFactory;
    private readonly IDatabaseInitializer _databaseInitializer;

    public DapperReminderTaskRepository(
        IDbConnectionFactory connectionFactory,
        IDatabaseInitializer databaseInitializer)
    {
        _connectionFactory = connectionFactory;
        _databaseInitializer = databaseInitializer;
    }

    public async Task<ReminderTask?> GetByIdAsync(ReminderTaskId id, CancellationToken cancellationToken)
    {
        await _databaseInitializer.EnsureCreatedAsync(cancellationToken);

        await using var connection = _connectionFactory.CreateConnection();
        var dataModel = await connection.QuerySingleOrDefaultAsync<ReminderTaskDataModel>(
            new CommandDefinition(SelectByIdSql, new { Id = id.Value.ToString() }, cancellationToken: cancellationToken));

        return dataModel is null ? null : ReminderTaskDataMapper.ToDomain(dataModel);
    }

    public async Task<IReadOnlyList<ReminderTask>> ListActiveDueAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        await _databaseInitializer.EnsureCreatedAsync(cancellationToken);

        await using var connection = _connectionFactory.CreateConnection();
        var dataModels = await connection.QueryAsync<ReminderTaskDataModel>(
            new CommandDefinition(SelectActiveDueSql, new { Now = FormatDateTime(now) }, cancellationToken: cancellationToken));

        return dataModels.Select(ReminderTaskDataMapper.ToDomain).ToArray();
    }

    public async Task SaveAsync(ReminderTask reminderTask, CancellationToken cancellationToken)
    {
        await _databaseInitializer.EnsureCreatedAsync(cancellationToken);

        await using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(new CommandDefinition(
            UpsertSql,
            ReminderTaskDataMapper.ToDataModel(reminderTask),
            cancellationToken: cancellationToken));
    }

    public async Task DeleteAsync(ReminderTaskId id, CancellationToken cancellationToken)
    {
        await _databaseInitializer.EnsureCreatedAsync(cancellationToken);

        await using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(new CommandDefinition(DeleteSql, new { Id = id.Value.ToString() }, cancellationToken: cancellationToken));
    }

    private static string FormatDateTime(DateTimeOffset value)
    {
        return value.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture);
    }
}
