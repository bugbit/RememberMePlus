namespace RememberMePlusApp.Application.Alarms;

public interface IAlarmScheduler
{
    Task ScheduleAsync(ScheduledReminderTask task, CancellationToken cancellationToken = default);
}
