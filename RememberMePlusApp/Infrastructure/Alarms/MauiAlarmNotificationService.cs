using RememberMePlusApp.Application.Alarms;

namespace RememberMePlusApp.Infrastructure.Alarms;

public sealed class MauiAlarmNotificationService : IAlarmNotificationService
{
    public Task NotifyAsync(ScheduledReminderTask task, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
