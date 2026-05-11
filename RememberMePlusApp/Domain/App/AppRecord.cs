namespace RememberMePlusApp.Domain.App;

public sealed record AppRecord(
    long IdApp,
    long Version,
    long RelativeOffsetMinutes,
    long SnoozeMinutes);
