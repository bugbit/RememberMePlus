namespace RememberMePlusApp.Domain.Tasks;

public sealed record ReminderTitle
{
    public const int MaxLength = 120;

    private ReminderTitle(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static ReminderTitle From(string value)
    {
        var normalizedValue = value?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(normalizedValue))
        {
            throw new ArgumentException("Reminder title is required.", nameof(value));
        }

        if (normalizedValue.Length > MaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"Reminder title cannot exceed {MaxLength} characters.");
        }

        return new ReminderTitle(normalizedValue);
    }

    public override string ToString() => Value;
}
