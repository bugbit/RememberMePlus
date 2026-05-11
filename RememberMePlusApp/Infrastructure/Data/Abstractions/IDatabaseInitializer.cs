namespace RememberMePlusApp.Infrastructure.Data;

public interface IDatabaseInitializer
{
    /// <summary>
    /// Inicializa el acceso a datos leyendo la versión de la base de datos.
    /// </summary>
    Task InitializeAsync(CancellationToken cancellationToken = default);
}
