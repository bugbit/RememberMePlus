namespace RememberMePlusApp.Domain.Tasks;

public readonly record struct ReminderTaskId(long Value)
{
    public static ReminderTaskId From(long value)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Reminder task id must be greater than zero.");
        }

        return new ReminderTaskId(value);
    }
}
