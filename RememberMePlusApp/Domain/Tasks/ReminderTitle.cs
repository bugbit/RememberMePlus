namespace RememberMePlusApp.Domain.Tasks;

public sealed record ReminderTitle
{
    private const int MaxLength = 120;

    private ReminderTitle(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static ReminderTitle Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Reminder title cannot be empty.", nameof(value));
        }

        var normalizedValue = value.Trim();

        if (normalizedValue.Length > MaxLength)
        {
            throw new ArgumentException($"Reminder title cannot exceed {MaxLength} characters.", nameof(value));
        }

        return new ReminderTitle(normalizedValue);
    }

    public override string ToString() => Value;
}
