using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using RememberMePlusApp.Infrastructure.Data.Abstractions;
using RememberMePlusApp.Infrastructure.Data.Options;

namespace RememberMePlusApp.Infrastructure.Data.Dapper;

public sealed class SqliteDbConnectionFactory : IDbConnectionFactory
{
    private readonly SqliteDataOptions _options;

    public SqliteDbConnectionFactory(IOptions<SqliteDataOptions> options)
    {
        _options = options.Value;
    }

    public DbConnection CreateConnection()
    {
        var connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = _options.DatabasePath
        }.ToString();

        return new SqliteConnection(connectionString);
    }
}
