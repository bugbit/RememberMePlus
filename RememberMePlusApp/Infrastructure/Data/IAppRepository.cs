namespace RememberMePlusApp.Infrastructure.Data;

public interface IAppRepository
{
    /// <summary>
    /// Obtiene el primer registro de la tabla App.
    /// </summary>
    Task<AppRecord?> GetFirstAsync(CancellationToken cancellationToken = default);
}
