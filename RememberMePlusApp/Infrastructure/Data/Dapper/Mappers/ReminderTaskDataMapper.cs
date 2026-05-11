using System.Globalization;
using RememberMePlusApp.Domain.Tasks;
using RememberMePlusApp.Infrastructure.Data.Dapper.Models;

namespace RememberMePlusApp.Infrastructure.Data.Dapper.Mappers;

public static class ReminderTaskDataMapper
{
    public static ReminderTaskDataModel ToDataModel(ReminderTask task)
    {
        return new ReminderTaskDataModel
        {
            Id = task.Id.Value,
            Title = task.Title.Value,
            DueAt = task.DueAt.ToString("O", CultureInfo.InvariantCulture),
            Priority = (int)task.Priority,
            IsActive = task.IsActive,
            ReactivatedAt = task.ReactivatedAt?.ToString("O", CultureInfo.InvariantCulture),
            TaskPostponeMinutes = task.TaskPostponeMinutes?.Value
        };
    }

    public static ReminderTask ToDomain(ReminderTaskDataModel dataModel)
    {
        return ReminderTask.Restore(
            ReminderTaskId.From(dataModel.Id),
            ReminderTitle.From(dataModel.Title),
            DateTimeOffset.Parse(dataModel.DueAt, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
            (ReminderPriority)dataModel.Priority,
            dataModel.IsActive,
            dataModel.ReactivatedAt is null
                ? null
                : DateTimeOffset.Parse(dataModel.ReactivatedAt, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
            dataModel.TaskPostponeMinutes is null ? null : PostponeMinutes.From(dataModel.TaskPostponeMinutes.Value));
    }
}
