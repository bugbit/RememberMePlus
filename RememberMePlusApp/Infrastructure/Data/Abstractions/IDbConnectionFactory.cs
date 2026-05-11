using System.Data;

namespace RememberMePlusApp.Infrastructure.Data.Abstractions;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
