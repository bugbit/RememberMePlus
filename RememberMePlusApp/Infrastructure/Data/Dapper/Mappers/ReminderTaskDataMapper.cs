using System.Globalization;
using RememberMePlusApp.Domain.Tasks;
using RememberMePlusApp.Infrastructure.Data.Dapper.Models;

namespace RememberMePlusApp.Infrastructure.Data.Dapper.Mappers;

internal static class ReminderTaskDataMapper
{
    public static ReminderTaskDataModel ToDataModel(ReminderTask reminderTask)
    {
        return new ReminderTaskDataModel
        {
            Id = reminderTask.Id.Value.ToString(),
            Title = reminderTask.Title.Value,
            DueAt = FormatDateTime(reminderTask.DueAt),
            Priority = (int)reminderTask.Priority,
            IsActive = reminderTask.IsActive,
            CompletedAt = FormatNullableDateTime(reminderTask.CompletedAt),
            ReactivatedFrom = FormatNullableDateTime(reminderTask.ReactivatedFrom),
            CustomPostponeMinutes = reminderTask.CustomPostponeMinutes?.Value
        };
    }

    public static ReminderTask ToDomain(ReminderTaskDataModel dataModel)
    {
        return ReminderTask.Restore(
            ReminderTaskId.Create(Guid.Parse(dataModel.Id)),
            ReminderTitle.Create(dataModel.Title),
            ParseDateTime(dataModel.DueAt),
            (ReminderPriority)dataModel.Priority,
            dataModel.IsActive,
            ParseNullableDateTime(dataModel.CompletedAt),
            ParseNullableDateTime(dataModel.ReactivatedFrom),
            dataModel.CustomPostponeMinutes.HasValue
                ? PostponeMinutes.Create(dataModel.CustomPostponeMinutes.Value)
                : null);
    }

    private static string FormatDateTime(DateTimeOffset value)
    {
        return value.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture);
    }

    private static string? FormatNullableDateTime(DateTimeOffset? value)
    {
        return value.HasValue ? FormatDateTime(value.Value) : null;
    }

    private static DateTimeOffset ParseDateTime(string value)
    {
        return DateTimeOffset.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
    }

    private static DateTimeOffset? ParseNullableDateTime(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : ParseDateTime(value);
    }
}
