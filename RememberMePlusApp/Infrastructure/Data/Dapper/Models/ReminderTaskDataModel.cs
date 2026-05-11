namespace RememberMePlusApp.Infrastructure.Data.Dapper.Models;

public sealed class ReminderTaskDataModel
{
    public Guid Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string DueAt { get; init; } = string.Empty;

    public int Priority { get; init; }

    public bool IsActive { get; init; }

    public string? ReactivatedAt { get; init; }

    public int? TaskPostponeMinutes { get; init; }
}
