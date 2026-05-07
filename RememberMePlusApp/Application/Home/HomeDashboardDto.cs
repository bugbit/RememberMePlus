namespace RememberMePlusApp.Application.Home;

public sealed record HomeDashboardDto(
    DateTime Now,
    int RelativeOffsetMinutes,
    int SnoozeMinutes,
    IReadOnlyList<HomeDashboardTaskDto> OverdueTasks,
    IReadOnlyList<HomeDashboardTaskDto> DueSoonTasks);

public sealed record HomeDashboardTaskDto(
    long Id,
    string Title,
    DateTime DueAt,
    bool IsOverdue,
    bool IsDueInLessThan15Minutes,
    TimeSpan Delta);
