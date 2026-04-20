namespace RememberMePlusApp.Infrastructure.Data;

public sealed class TaskItemRecord
{
    public long IdTask { get; init; }

    public string Title { get; init; } = string.Empty;

    public string DateDueAt { get; init; } = string.Empty;
}
