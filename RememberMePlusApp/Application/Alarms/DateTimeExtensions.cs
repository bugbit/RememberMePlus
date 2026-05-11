namespace RememberMePlusApp.Application.Alarms;

internal static class DateTimeExtensions
{
    public static DateTime TrimToMinute(this DateTime value)
    {
        return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, 0, value.Kind);
    }
}
