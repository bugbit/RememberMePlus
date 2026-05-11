using Microsoft.Extensions.Logging;
using RememberMePlusApp.Domain.App;
using System.Reflection;

namespace RememberMePlusApp.Infrastructure.Data.Dapper.Repositories;

public sealed class DapperDatabaseSchemaRepository(
    IAppRepository appRepository,
    ILogger<DapperDatabaseSchemaRepository> logger) : IDatabaseSchemaRepository
{
    private readonly IAppRepository _appRepository = appRepository;
    private readonly ILogger<DapperDatabaseSchemaRepository> _logger = logger;

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
        if (unitOfWork is not ISqlExecutor sqlExecutor)
        {
            return;
        }

        _logger.LogDebug("Executing database script: {ScriptFileName}", scriptFileName);

        var resourceName = $"RememberMePlusApp.Resources.Scripts.{scriptFileName}";
        var assembly = Assembly.GetExecutingAssembly();

        await using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded script not found: {resourceName}");

        using var reader = new StreamReader(stream);
        var sql = await reader.ReadToEndAsync(cancellationToken);

        await sqlExecutor.ExecuteAsync(sql, cancellationToken: cancellationToken);

        _logger.LogInformation("Database script executed successfully: {ScriptFileName}", scriptFileName);
    }
}
