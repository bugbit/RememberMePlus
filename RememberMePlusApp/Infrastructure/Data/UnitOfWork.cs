using System.Data.Common;

namespace RememberMePlusApp.Infrastructure.Data;

/// <summary>
/// Unidad de trabajo que gestiona una conexión y transacción SQLite.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    public DbConnection Connection { get; }
    public DbTransaction Transaction { get; private set; }

    private bool _completed;

    private UnitOfWork(DbConnection connection, DbTransaction transaction)
    {
        Connection = connection;
        Transaction = transaction;
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

    /// <summary>
    /// Confirma todos los cambios de la unidad de trabajo.
    /// </summary>
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await Transaction.CommitAsync(cancellationToken);
        _completed = true;
    }

    /// <summary>
    /// Revierte todos los cambios de la unidad de trabajo.
    /// </summary>
    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        await Transaction.RollbackAsync(cancellationToken);
        _completed = true;
    }

    public async ValueTask DisposeAsync()
    {
        if (!_completed)
            await Transaction.RollbackAsync();

        await Transaction.DisposeAsync();
        await Connection.DisposeAsync();
    }
}
