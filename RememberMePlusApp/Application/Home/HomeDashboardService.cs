using System.Globalization;
using RememberMePlusApp.Application.Time;
using RememberMePlusApp.Infrastructure.Data;

namespace RememberMePlusApp.Application.Home;

public sealed class HomeDashboardService : IHomeDashboardService
{
    private const int CriticalSoonMinutes = 15;
    private const int DefaultRelativeOffsetMinutes = 120;
    private const int DefaultSnoozeMinutes = 5;

    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly ITaskRepository _taskRepository;
    private readonly IAppRepository _appRepository;
    private readonly IAppClock _appClock;

    public HomeDashboardService(
        IUnitOfWorkFactory unitOfWorkFactory,
        ITaskRepository taskRepository,
        IAppRepository appRepository,
        IAppClock appClock)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _taskRepository = taskRepository;
        _appRepository = appRepository;
        _appClock = appClock;
    }

    public async Task<HomeDashboardDto> GetAsync(IReadOnlySet<long> temporarilyIgnoredTaskIds, CancellationToken cancellationToken = default)
    {
        var now = _appClock.Now;

        await using var unitOfWork = await _unitOfWorkFactory.CreateAsync(useTransaction: false, cancellationToken);
        var appRecord = await _appRepository.GetFirstAsync(unitOfWork, cancellationToken);
        var relativeOffsetMinutes = ResolvePositiveInt(appRecord?.RelativeOffsetMinutes, DefaultRelativeOffsetMinutes);
        var snoozeMinutes = ResolvePositiveInt(appRecord?.SnoozeMinutes, DefaultSnoozeMinutes);
        var windowEnd = now.AddMinutes(relativeOffsetMinutes);

        var activeTasks = await _taskRepository.GetActiveDueBeforeAsync(windowEnd, unitOfWork, cancellationToken);
        var visibleTasks = activeTasks
            .Where(task => !temporarilyIgnoredTaskIds.Contains(task.IdTask))
            .Select(task => CreateDashboardTask(task, now))
            .ToList();

        var overdueTasks = visibleTasks
            .Where(task => task.DueAt < now)
            .OrderBy(task => task.DueAt)
            .ThenBy(task => task.Title)
            .ToList();

        var dueSoonTasks = visibleTasks
            .Where(task => task.DueAt >= now && task.DueAt <= windowEnd)
            .OrderBy(task => task.DueAt)
            .ThenBy(task => task.Title)
            .ToList();

        return new HomeDashboardDto(now, relativeOffsetMinutes, snoozeMinutes, overdueTasks, dueSoonTasks);
    }

    private static HomeDashboardTaskDto CreateDashboardTask(TaskItemRecord task, DateTime now)
    {
        var dueAt = ParseDueAt(task.DateDueAt, now);
        var delta = dueAt - now;
        var isOverdue = delta < TimeSpan.Zero;
        var isDueInLessThan15Minutes = !isOverdue && delta <= TimeSpan.FromMinutes(CriticalSoonMinutes);

        return new HomeDashboardTaskDto(
            task.IdTask,
            task.Title,
            dueAt,
            isOverdue,
            isDueInLessThan15Minutes,
            delta);
    }

    private static DateTime ParseDueAt(string value, DateTime fallback)
    {
        if (DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out var currentCultureDate))
        {
            return currentCultureDate;
        }

        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var invariantDate))
        {
            return invariantDate;
        }

        return fallback;
    }

    private static int ResolvePositiveInt(long? value, int fallback)
    {
        return value is > 0 and <= int.MaxValue ? (int)value.Value : fallback;
    }
}
