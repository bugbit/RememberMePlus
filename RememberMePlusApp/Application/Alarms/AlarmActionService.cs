using RememberMePlusApp.Infrastructure.Data;

namespace RememberMePlusApp.Application.Alarms;

public interface IAlarmActionService
{
    Task CompleteAsync(long taskId, CancellationToken cancellationToken = default);

    Task SnoozeAsync(long taskId, int minutes, CancellationToken cancellationToken = default);
}

public sealed class AlarmActionService(
    IUnitOfWorkFactory unitOfWorkFactory,
    ITaskRepository taskRepository,
    IAlarmStartupService alarmStartupService)
    : IAlarmActionService
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory = unitOfWorkFactory;
    private readonly ITaskRepository _taskRepository = taskRepository;
    private readonly IAlarmStartupService _alarmStartupService = alarmStartupService;

    public async Task CompleteAsync(long taskId, CancellationToken cancellationToken = default)
    {
        await using var unitOfWork = await _unitOfWorkFactory.CreateAsync(cancellationToken: cancellationToken);
        await _taskRepository.CompleteAsync(taskId, unitOfWork, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        await _alarmStartupService.StartAsync(cancellationToken);
    }

    public async Task SnoozeAsync(long taskId, int minutes, CancellationToken cancellationToken = default)
    {
        var notifyAt = DateTime.Now.TrimToMinute().AddMinutes(minutes);
        await using var unitOfWork = await _unitOfWorkFactory.CreateAsync(cancellationToken: cancellationToken);
        await _taskRepository.SnoozeAsync(taskId, notifyAt, unitOfWork, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        await _alarmStartupService.StartAsync(cancellationToken);
    }
}
