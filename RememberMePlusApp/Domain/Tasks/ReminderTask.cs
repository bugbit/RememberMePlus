namespace RememberMePlusApp.Domain.Tasks;

public sealed class ReminderTask
{
    private ReminderTask(
        ReminderTaskId id,
        ReminderTitle title,
        DateTimeOffset dueAt,
        ReminderPriority priority,
        PostponeMinutes? customPostponeMinutes,
        bool isActive,
        DateTimeOffset? completedAt,
        DateTimeOffset? reactivatedFrom)
    {
        Id = id;
        Title = title;
        DueAt = dueAt;
        Priority = priority;
        CustomPostponeMinutes = customPostponeMinutes;
        IsActive = isActive;
        CompletedAt = completedAt;
        ReactivatedFrom = reactivatedFrom;
    }

    public ReminderTaskId Id { get; }

    public ReminderTitle Title { get; private set; }

    public DateTimeOffset DueAt { get; private set; }

    public ReminderPriority Priority { get; private set; }

    public bool IsActive { get; private set; }

    public DateTimeOffset? CompletedAt { get; private set; }

    public DateTimeOffset? ReactivatedFrom { get; private set; }

    public PostponeMinutes? CustomPostponeMinutes { get; private set; }

    public bool IsCompleted => CompletedAt.HasValue;

    public static ReminderTask Create(
        ReminderTitle title,
        DateTimeOffset dueAt,
        ReminderPriority priority = ReminderPriority.Normal,
        PostponeMinutes? customPostponeMinutes = null)
    {
        return new ReminderTask(
            ReminderTaskId.New(),
            title,
            dueAt,
            priority,
            customPostponeMinutes,
            isActive: true,
            completedAt: null,
            reactivatedFrom: null);
    }

    public static ReminderTask Restore(
        ReminderTaskId id,
        ReminderTitle title,
        DateTimeOffset dueAt,
        ReminderPriority priority,
        bool isActive,
        DateTimeOffset? completedAt,
        DateTimeOffset? reactivatedFrom,
        PostponeMinutes? customPostponeMinutes)
    {
        return new ReminderTask(
            id,
            title,
            dueAt,
            priority,
            customPostponeMinutes,
            isActive,
            completedAt,
            reactivatedFrom);
    }

    public void Rename(ReminderTitle title)
    {
        Title = title;
    }

    public void Reschedule(DateTimeOffset dueAt)
    {
        DueAt = dueAt;
        CompletedAt = null;
    }

    public void ChangePriority(ReminderPriority priority)
    {
        Priority = priority;
    }

    public void ConfigureCustomPostpone(PostponeMinutes? customPostponeMinutes)
    {
        CustomPostponeMinutes = customPostponeMinutes;
    }

    public void Complete(DateTimeOffset completedAt)
    {
        CompletedAt = completedAt;
    }

    public void Postpone(PostponeMinutes minutes, DateTimeOffset postponedAt)
    {
        if (!IsActive)
        {
            throw new InvalidOperationException("Inactive reminders cannot be postponed.");
        }

        if (IsCompleted)
        {
            throw new InvalidOperationException("Completed reminders cannot be postponed.");
        }

        DueAt = postponedAt.AddMinutes(minutes.Value);
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void ReactivateFrom(DateTimeOffset effectiveFrom)
    {
        IsActive = true;
        CompletedAt = null;
        ReactivatedFrom = effectiveFrom;

        if (DueAt < effectiveFrom)
        {
            DueAt = effectiveFrom;
        }
    }

    public bool IsDue(DateTimeOffset now)
    {
        return IsActive && !IsCompleted && DueAt <= now;
    }
}
