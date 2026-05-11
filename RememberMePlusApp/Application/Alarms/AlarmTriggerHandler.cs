using RememberMePlusApp.Infrastructure.Data;

namespace RememberMePlusApp.Application.Alarms;

public sealed class AlarmTriggerHandler(
    IUnitOfWorkFactory unitOfWorkFactory,
    ITaskRepository taskRepository,
    IAppRepository appRepository,
    IAlarmNotificationService notificationService,
    IAlarmAlertCoordinator alertCoordinator,
    IAlarmStartupService alarmStartupService)
    : IAlarmTriggerHandler
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory = unitOfWorkFactory;
    private readonly ITaskRepository _taskRepository = taskRepository;
    private readonly IAppRepository _appRepository = appRepository;
    private readonly IAlarmNotificationService _notificationService = notificationService;
    private readonly IAlarmAlertCoordinator _alertCoordinator = alertCoordinator;
    private readonly IAlarmStartupService _alarmStartupService = alarmStartupService;

    public async Task HandleAsync(long taskId, CancellationToken cancellationToken = default)
    {
        await using var unitOfWork = await _unitOfWorkFactory.CreateAsync(useTransaction: false, cancellationToken);
        var task = await _taskRepository.GetByIdForAlarmAsync(taskId, unitOfWork, cancellationToken);

        if (task is null)
        {
            await _alarmStartupService.StartAsync(cancellationToken);
            return;
        }

        var app = await _appRepository.GetFirstAsync(unitOfWork, cancellationToken);
        var snoozeMinutes = task.SnoozeMinutes.GetValueOrDefault(
            app?.SnoozeMinutes > 0 ? Convert.ToInt32(app.SnoozeMinutes) : 10);

        await _notificationService.NotifyAsync(task, cancellationToken);
        await _alertCoordinator.ShowAsync(task, snoozeMinutes, cancellationToken);
    }
}
