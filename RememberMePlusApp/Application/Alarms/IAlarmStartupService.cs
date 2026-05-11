namespace RememberMePlusApp.Application.Alarms;

public interface IAlarmStartupService
{
    Task StartAsync(CancellationToken cancellationToken = default);
}
