using System.Globalization;
using RememberMePlusApp.Domain.Tasks;

namespace RememberMePlusApp.Infrastructure.Data;

public static class ReminderTaskDataMapper
{
    private static readonly string[] DateFormats = ["yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd"];

    public static ReminderTask ToDomain(ReminderTaskDataModel model)
    {
        return ReminderTask.Restore(
            ReminderTaskId.From(model.IdTask),
            new ReminderTitle(model.Title),
            ParseDueAt(model.DateDueAt),
            model.IsActive == 1,
            model.IsInsistent == 1 ? ReminderPriority.Important : ReminderPriority.Normal);
    }

    private static DateTime ParseDueAt(string value)
    {
        if (DateTime.TryParseExact(value, DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var parsed))
        {
            return parsed;
        }

        return DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
    }
}
