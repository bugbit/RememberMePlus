using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.Maui.Storage;

namespace RememberMePlusApp.Infrastructure.Data;

public sealed class DbConnectionFactory : IDbConnectionFactory
{
    private const string DatabaseFileName = "remembermeplus.db3";

    public string GetDatabasePath()
    {
        return Path.Combine(FileSystem.Current.AppDataDirectory, DatabaseFileName);
    }

    public DbConnection CreateConnection()
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder
        {
            DataSource = GetDatabasePath()
        };

        return new SqliteConnection(connectionStringBuilder.ToString());
    }
}
