using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;
using RememberMePlusApp.Infrastructure.Data.Options;

namespace RememberMePlusApp.Infrastructure.Data;

public sealed class SqliteDbConnectionFactory(ILogger<SqliteDbConnectionFactory> logger) : IDbConnectionFactory
{
    private readonly ILogger<SqliteDbConnectionFactory> _logger = logger;

    public string GetDatabasePath()
    {
        return Path.Combine(FileSystem.Current.AppDataDirectory, SqliteDataOptions.DatabaseFileName);
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
