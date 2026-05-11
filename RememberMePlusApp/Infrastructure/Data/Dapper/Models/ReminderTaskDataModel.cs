namespace RememberMePlusApp.Infrastructure.Data.Dapper.Models;

internal sealed class ReminderTaskDataModel
{
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string DueAt { get; set; } = string.Empty;

    public int Priority { get; set; }

    public bool IsActive { get; set; }

    public string? CompletedAt { get; set; }

    public string? ReactivatedFrom { get; set; }

    public int? CustomPostponeMinutes { get; set; }
}
