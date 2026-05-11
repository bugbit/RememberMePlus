namespace RememberMePlusApp.Infrastructure.Data;

public interface IDatabaseSchemaRepository
{
    /// <summary>
    /// Crea o actualiza el esquema de la base de datos según la versión detectada.
    /// </summary>
    Task CreateOrUpdateDatabaseAsync(IUnitOfWork unitOfWork, CancellationToken cancellationToken = default);
}