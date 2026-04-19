namespace RememberMePlusApp.Infrastructure.Data;

public sealed class UnitOfWorkFactory(IDbConnectionFactory dbConnectionFactory) : IUnitOfWorkFactory
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    /// <summary>
    /// Crea una nueva unidad de trabajo con conexión y transacción abiertas.
    /// </summary>
    public async Task<IUnitOfWork> CreateAsync(CancellationToken cancellationToken = default)
        => await UnitOfWork.CreateAsync(_dbConnectionFactory, cancellationToken);
}
