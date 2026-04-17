using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;

namespace RememberMePlusApp.Infrastructure.Data;

public sealed class DbConnectionFactory : IDbConnectionFactory
{
    private const string DatabaseFileName = "remembermeplus.db3";

    private readonly ILogger<DbConnectionFactory> _logger;

    public DbConnectionFactory(ILogger<DbConnectionFactory> logger)
    {
        _logger = logger;
    }

    public string GetDatabasePath()
    {
        return Path.Combine(FileSystem.Current.AppDataDirectory, DatabaseFileName);
    }

    public DbConnection CreateConnection()
    {
        var dbPath = GetDatabasePath();

        _logger.LogDebug("Database path: {DatabasePath}", dbPath);

        var connectionStringBuilder = new SqliteConnectionStringBuilder
        {
            DataSource = dbPath
        };

        return new SqliteConnection(connectionStringBuilder.ToString());
    }
}
