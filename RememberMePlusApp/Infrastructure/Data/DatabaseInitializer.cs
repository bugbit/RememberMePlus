using Dapper;

namespace RememberMePlusApp.Infrastructure.Data;

public sealed class DatabaseInitializer(IDatabaseSchemaRepository databaseSchemaRepository) : IDatabaseInitializer
{
    private readonly IDatabaseSchemaRepository _databaseSchemaRepository = databaseSchemaRepository;

    /// <summary>
    /// Inicializa el acceso a datos leyendo la versión de la base de datos.
    /// </summary>
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await _databaseSchemaRepository.CreateOrUpdateDatabaseAsync(cancellationToken);
    }
}
