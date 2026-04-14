namespace RememberMePlusApp.Infrastructure.Data;

public sealed record AppRecord(
    int IdApp,
    int Version,
    int RelativeOffsetMinutes,
    int SnoozeMinutes);
