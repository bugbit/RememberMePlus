namespace RememberMePlusApp.Application.Home;

public interface IHomeDashboardService
{
    Task<HomeDashboardDto> GetAsync(IReadOnlySet<long> temporarilyIgnoredTaskIds, CancellationToken cancellationToken = default);
}
