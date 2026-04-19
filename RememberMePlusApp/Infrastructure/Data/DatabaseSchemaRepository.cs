using Dapper;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace RememberMePlusApp.Infrastructure.Data;

public sealed class DatabaseSchemaRepository(
    IAppRepository appRepository,
    ILogger<DatabaseSchemaRepository> logger) : IDatabaseSchemaRepository
{
    private readonly IAppRepository _appRepository = appRepository;
    private readonly ILogger<DatabaseSchemaRepository> _logger = logger;

    /// <summary>
    /// Crea o actualiza el esquema de la base de datos según la versión detectada.
    /// Si no existe registro en App, ejecuta el script de creación inicial (database-v1.sql).
    /// </summary>
    public async Task CreateOrUpdateDatabaseAsync(IUnitOfWork unitOfWork, CancellationToken cancellationToken = default)
    {
        AppRecord? app;

        try
        {
            app = await _appRepository.GetFirstAsync(unitOfWork, cancellationToken);
        }
        catch
        {
            app = null;
        }

        if (app is null)
        {
            await ExecuteScriptAsync(unitOfWork, "database-v1.sql", cancellationToken);
        }
    }

    private async Task ExecuteScriptAsync(IUnitOfWork unitOfWork, string scriptFileName, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Executing database script: {ScriptFileName}", scriptFileName);

        var resourceName = $"RememberMePlusApp.Resources.Scripts.{scriptFileName}";
        var assembly = Assembly.GetExecutingAssembly();

        await using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded script not found: {resourceName}");

        using var reader = new StreamReader(stream);
        var sql = await reader.ReadToEndAsync(cancellationToken);

        await unitOfWork.Connection.ExecuteAsync(new CommandDefinition(
            sql,
            transaction: unitOfWork.Transaction,
            cancellationToken: cancellationToken));

        _logger.LogInformation("Database script executed successfully: {ScriptFileName}", scriptFileName);
    }
}