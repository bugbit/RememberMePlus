namespace RememberMePlusApp.Infrastructure.Data.Abstractions;

public interface IDatabaseInitializer
{
    Task EnsureCreatedAsync(CancellationToken cancellationToken);
}
