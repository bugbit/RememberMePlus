namespace RememberMePlusApp.Application.Alarms;

public interface IAlarmAlertCoordinator
{
    Task ShowAsync(ScheduledReminderTask task, int snoozeMinutes, CancellationToken cancellationToken = default);
}
