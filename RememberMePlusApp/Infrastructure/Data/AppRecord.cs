namespace RememberMePlusApp.Infrastructure.Data;

public sealed record AppRecord(
    long IdApp,
    long Version,
    long RelativeOffsetMinutes,
    long SnoozeMinutes);
