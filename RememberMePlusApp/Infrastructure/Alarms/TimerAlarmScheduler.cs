using Microsoft.Extensions.DependencyInjection;
using RememberMePlusApp.Application.Alarms;

namespace RememberMePlusApp.Infrastructure.Alarms;

public sealed class TimerAlarmScheduler(IServiceProvider serviceProvider) : IAlarmScheduler, IDisposable
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private Timer? _timer;

    public Task ScheduleAsync(ScheduledReminderTask task, CancellationToken cancellationToken = default)
    {
        var dueTime = task.NotifyAt.TrimToMinute() - DateTime.Now.TrimToMinute();
        if (dueTime < TimeSpan.Zero)
        {
            dueTime = TimeSpan.Zero;
        }

        _timer?.Dispose();
        _timer = new Timer(
            static state => _ = ((TimerAlarmState)state!).HandleAsync(),
            new TimerAlarmState(_serviceProvider, task.Id),
            dueTime,
            Timeout.InfiniteTimeSpan);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    private sealed class TimerAlarmState(IServiceProvider serviceProvider, long taskId)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly long _taskId = taskId;

        public Task HandleAsync()
        {
            return _serviceProvider.GetRequiredService<IAlarmTriggerHandler>().HandleAsync(_taskId);
        }
    }
}
