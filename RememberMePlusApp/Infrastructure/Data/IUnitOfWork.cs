using System.Data.Common;

namespace RememberMePlusApp.Infrastructure.Data;

/// <summary>
/// Representa una unidad de trabajo que encapsula una conexión y transacción activas.
/// </summary>
public interface IUnitOfWork : IAsyncDisposable
{
    DbConnection Connection { get; }
    DbTransaction Transaction { get; }

    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
