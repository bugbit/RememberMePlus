using Dapper;
using System.Data.Common;

namespace RememberMePlusApp.Infrastructure.Data;

/// <summary>
/// Unidad de trabajo que gestiona una conexión y transacción SQLite.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork, ISqlExecutor
{
    private readonly DbConnection _connection;
    private readonly DbTransaction _transaction;

    private bool _completed;

    private UnitOfWork(DbConnection connection, DbTransaction transaction)
    {
        _connection = connection;
        _transaction = transaction;
    }

    /// <summary>
    /// Abre la conexión e inicia la transacción.
    /// </summary>
    public static async Task<UnitOfWork> CreateAsync(
        IDbConnectionFactory dbConnectionFactory,
        CancellationToken cancellationToken = default)
    {
        var connection = dbConnectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        var transaction = await connection.BeginTransactionAsync(cancellationToken);

        return new UnitOfWork(connection, transaction);
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        return await _connection.QueryAsync<T>(
            new CommandDefinition(sql, parameters, _transaction, cancellationToken: cancellationToken));
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        return await _connection.QueryFirstOrDefaultAsync<T>(
            new CommandDefinition(sql, parameters, _transaction, cancellationToken: cancellationToken));
    }

    public async Task<int> ExecuteAsync(
        string sql,
        object? parameters = null,
        CancellationToken cancellationToken = default)
    {
        return await _connection.ExecuteAsync(
            new CommandDefinition(sql, parameters, _transaction, cancellationToken: cancellationToken));
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _transaction.CommitAsync(cancellationToken);
        _completed = true;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        await _transaction.RollbackAsync(cancellationToken);
        _completed = true;
    }

    public async ValueTask DisposeAsync()
    {
        if (!_completed)
        {
            await _transaction.RollbackAsync();
        }

        await _transaction.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
