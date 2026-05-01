using Dapper;

namespace RememberMePlusApp.Infrastructure.Data;

public sealed class DatabaseInitializer(
    IUnitOfWorkFactory unitOfWorkFactory,
    IDatabaseSchemaRepository databaseSchemaRepository) : IDatabaseInitializer
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory = unitOfWorkFactory;
    private readonly IDatabaseSchemaRepository _databaseSchemaRepository = databaseSchemaRepository;

    /// <summary>
    /// Inicializa el acceso a datos creando o actualizando el esquema de la base de datos.
    /// </summary>
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await using var uow = await _unitOfWorkFactory.CreateAsync(cancellationToken: cancellationToken);

        await _databaseSchemaRepository.CreateOrUpdateDatabaseAsync(uow, cancellationToken);

        await uow.CommitAsync(cancellationToken);
    }
}
