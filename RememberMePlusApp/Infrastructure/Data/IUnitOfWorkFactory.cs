namespace RememberMePlusApp.Infrastructure.Data;

/// <summary>
/// Factoría para crear instancias de IUnitOfWork.
/// </summary>
public interface IUnitOfWorkFactory
{
    Task<IUnitOfWork> CreateAsync(CancellationToken cancellationToken = default);
}
