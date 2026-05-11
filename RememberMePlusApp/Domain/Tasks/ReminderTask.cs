namespace RememberMePlusApp.Domain.Tasks;

public sealed class ReminderTask
{
    private ReminderTask(
        ReminderTaskId id,
        ReminderTitle title,
        DateTime dueAt,
        bool isActive,
        ReminderPriority priority)
    {
        Id = id;
        Title = title;
        DueAt = dueAt;
        IsActive = isActive;
        Priority = priority;
    }

    public ReminderTaskId Id { get; }

    public ReminderTitle Title { get; }

    public DateTime DueAt { get; private set; }

    public bool IsActive { get; private set; }

    public ReminderPriority Priority { get; }

    public static ReminderTask Restore(
        ReminderTaskId id,
        ReminderTitle title,
        DateTime dueAt,
        bool isActive,
        ReminderPriority priority)
    {
        return new ReminderTask(id, title, dueAt, isActive, priority);
    }

    public void Snooze(DateTime dueAt)
    {
        DueAt = dueAt;
        IsActive = true;
    }

    public void CompleteNonRecurring()
    {
        IsActive = false;
    }
}
