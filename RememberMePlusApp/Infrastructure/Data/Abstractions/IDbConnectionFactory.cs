using System.Data.Common;

namespace RememberMePlusApp.Infrastructure.Data.Abstractions;

public interface IDbConnectionFactory
{
    DbConnection CreateConnection();
}
