namespace RememberMePlusApp.Application.Alarms;

public interface IAlarmTriggerHandler
{
    Task HandleAsync(long taskId, CancellationToken cancellationToken = default);
}
