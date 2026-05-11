namespace RememberMePlusApp.Application.Alarms;

public interface IAlarmNotificationService
{
    Task NotifyAsync(ScheduledReminderTask task, CancellationToken cancellationToken = default);
}
