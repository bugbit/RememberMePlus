using System.Data.Common;

namespace RememberMePlusApp.Infrastructure.Data;

public interface IDbConnectionFactory
{
    string GetDatabasePath();
    DbConnection CreateConnection();
}
