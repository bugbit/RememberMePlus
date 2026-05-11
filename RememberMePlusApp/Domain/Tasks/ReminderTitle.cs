namespace RememberMePlusApp.Domain.Tasks;

public readonly record struct ReminderTitle
{
    public ReminderTitle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Reminder title cannot be empty.", nameof(value));
        }

        Value = value.Trim();
    }

    public string Value { get; }

    public override string ToString() => Value;
}
