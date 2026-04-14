using Dapper;

namespace RememberMePlusApp.Infrastructure.Data;

public sealed class DatabaseInitializer(IAppRepository appRepository) : IDatabaseInitializer
{
    private readonly IAppRepository _appRepository = appRepository;
    private int? _databaseVersion;

    /// <summary>
    /// Inicializa el acceso a datos leyendo la versión de la base de datos.
    /// </summary>
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        AppRecord? app;

        try
        {
            app = await _appRepository.GetFirstAsync(cancellationToken);
        }
        catch
        {
            app = null;
        }

        _databaseVersion = app?.Version;
    }
}
