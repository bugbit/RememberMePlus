namespace RememberMePlusApp.Infrastructure.Data;

/// <summary>
/// Representa una unidad de trabajo que encapsula una transacción activa.
/// </summary>
public interface IUnitOfWork : IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
