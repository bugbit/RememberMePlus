namespace RememberMePlusApp.Infrastructure.Data.Abstractions;

public interface IDatabaseInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
}
