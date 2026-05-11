using RememberMePlusApp.Infrastructure.Data;

namespace RememberMePlusApp.Application.Alarms;

public sealed class AlarmStartupService(
    IUnitOfWorkFactory unitOfWorkFactory,
    ITaskRepository taskRepository,
    IAlarmScheduler alarmScheduler)
    : IAlarmStartupService
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory = unitOfWorkFactory;
    private readonly ITaskRepository _taskRepository = taskRepository;
    private readonly IAlarmScheduler _alarmScheduler = alarmScheduler;

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now.TrimToMinute();

        await using (var unitOfWork = await _unitOfWorkFactory.CreateAsync(cancellationToken: cancellationToken))
        {
            await _taskRepository.NormalizeOverdueNotificationsAsync(now, unitOfWork, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
        }

        await ScheduleNextAsync(cancellationToken);
    }

    private async Task ScheduleNextAsync(CancellationToken cancellationToken)
    {
        await using var unitOfWork = await _unitOfWorkFactory.CreateAsync(useTransaction: false, cancellationToken);
        var nextTask = await _taskRepository.GetNextToNotifyAsync(unitOfWork, cancellationToken);

        if (nextTask is null)
        {
            return;
        }

        await _alarmScheduler.ScheduleAsync(nextTask, cancellationToken);
    }
}
