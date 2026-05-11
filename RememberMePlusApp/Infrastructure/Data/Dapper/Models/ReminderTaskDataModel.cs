namespace RememberMePlusApp.Infrastructure.Data;

public sealed class ReminderTaskDataModel
{
    public long IdTask { get; init; }

    public string Title { get; init; } = string.Empty;

    public string DateDueAt { get; init; } = string.Empty;

    public string? DateTimeNotifyAt { get; init; }

    public long? SnoozeMinutes { get; init; }

    public long IsActive { get; init; } = 1;

    public long IsInsistent { get; init; }
}
