namespace RememberMePlusApp.Application.Alarms;

public sealed record ScheduledReminderTask(
    long Id,
    string Title,
    DateTime NotifyAt,
    int? SnoozeMinutes);
