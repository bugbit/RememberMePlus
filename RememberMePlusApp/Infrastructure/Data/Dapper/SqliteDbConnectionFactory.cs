using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using RememberMePlusApp.Infrastructure.Data.Abstractions;
using RememberMePlusApp.Infrastructure.Data.Options;

namespace RememberMePlusApp.Infrastructure.Data.Dapper;

public sealed class SqliteDbConnectionFactory(IOptions<SqliteDataOptions> options) : IDbConnectionFactory
{
    private readonly IOptions<SqliteDataOptions> _options = options;

    public IDbConnection CreateConnection()
    {
        var databasePath = _options.Value.DatabasePath;
        var databaseDirectory = Path.GetDirectoryName(databasePath);

        if (!string.IsNullOrWhiteSpace(databaseDirectory))
        {
            Directory.CreateDirectory(databaseDirectory);
        }

        return new SqliteConnection($"Data Source={databasePath}");
    }
}
