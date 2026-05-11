namespace RememberMePlusApp.Domain.Tasks;

public sealed record ReminderTaskId
{
    private ReminderTaskId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static ReminderTaskId New() => new(Guid.NewGuid());

    public static ReminderTaskId Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Reminder task id cannot be empty.", nameof(value));
        }

        return new ReminderTaskId(value);
    }

    public override string ToString() => Value.ToString();
}
