namespace RememberMePlusApp.Infrastructure.Data;

public sealed class UnitOfWorkFactory(IDbConnectionFactory dbConnectionFactory) : IUnitOfWorkFactory
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    /// <summary>
    /// Crea una nueva unidad de trabajo con conexión abierta y transacción opcional.
    /// </summary>
    public async Task<IUnitOfWork> CreateAsync(bool useTransaction = true, CancellationToken cancellationToken = default)
        => await UnitOfWork.CreateAsync(_dbConnectionFactory, useTransaction, cancellationToken);
}
