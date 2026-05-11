namespace RememberMePlusApp.Domain.Tasks;

public sealed class ReminderTask
{
    private ReminderTask(
        ReminderTaskId id,
        ReminderTitle title,
        DateTimeOffset dueAt,
        ReminderPriority priority,
        bool isActive,
        DateTimeOffset? reactivatedAt,
        PostponeMinutes? taskPostponeMinutes)
    {
        Id = id;
        Title = title;
        DueAt = dueAt;
        Priority = priority;
        IsActive = isActive;
        ReactivatedAt = reactivatedAt;
        TaskPostponeMinutes = taskPostponeMinutes;
    }

    public ReminderTaskId Id { get; }

    public ReminderTitle Title { get; private set; }

    public DateTimeOffset DueAt { get; private set; }

    public ReminderPriority Priority { get; private set; }

    public bool IsActive { get; private set; }

    public DateTimeOffset? ReactivatedAt { get; private set; }

    public PostponeMinutes? TaskPostponeMinutes { get; private set; }

    public static ReminderTask Create(
        ReminderTitle title,
        DateTimeOffset dueAt,
        ReminderPriority priority,
        PostponeMinutes? taskPostponeMinutes = null)
    {
        return new ReminderTask(ReminderTaskId.New(), title, dueAt, priority, isActive: true, reactivatedAt: null, taskPostponeMinutes);
    }

    public static ReminderTask Restore(
        ReminderTaskId id,
        ReminderTitle title,
        DateTimeOffset dueAt,
        ReminderPriority priority,
        bool isActive,
        DateTimeOffset? reactivatedAt,
        PostponeMinutes? taskPostponeMinutes)
    {
        return new ReminderTask(id, title, dueAt, priority, isActive, reactivatedAt, taskPostponeMinutes);
    }

    public void Rename(ReminderTitle title)
    {
        Title = title;
    }

    public void Reschedule(DateTimeOffset dueAt)
    {
        DueAt = dueAt;
    }

    public void ChangePriority(ReminderPriority priority)
    {
        Priority = priority;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Reactivate(DateTimeOffset effectiveAt)
    {
        IsActive = true;
        ReactivatedAt = effectiveAt;
    }

    public void ChangeTaskPostponeMinutes(PostponeMinutes postponeMinutes)
    {
        TaskPostponeMinutes = postponeMinutes;
    }

    public void ClearTaskPostponeMinutes()
    {
        TaskPostponeMinutes = null;
    }

    public void Postpone(DateTimeOffset nextDueAt, PostponeMinutes postponeMinutes)
    {
        DueAt = nextDueAt;
        TaskPostponeMinutes = postponeMinutes;
    }
}
