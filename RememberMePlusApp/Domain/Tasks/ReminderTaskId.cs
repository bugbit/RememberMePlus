namespace RememberMePlusApp.Domain.Tasks;

public readonly record struct ReminderTaskId(Guid Value)
{
    public static ReminderTaskId New() => new(Guid.NewGuid());

    public static ReminderTaskId From(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Reminder task id cannot be empty.", nameof(value));
        }

        return new ReminderTaskId(value);
    }

    public override string ToString() => Value.ToString();
}
