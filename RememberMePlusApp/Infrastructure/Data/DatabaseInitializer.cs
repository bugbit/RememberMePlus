namespace RememberMePlusApp.Infrastructure.Data;

public sealed class DatabaseInitializer(IDbConnectionFactory dbConnectionFactory)
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;
}
