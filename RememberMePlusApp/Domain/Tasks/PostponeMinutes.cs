namespace RememberMePlusApp.Domain.Tasks;

public readonly record struct PostponeMinutes
{
    public const int MinValue = 1;
    public const int MaxValue = 1440;

    private PostponeMinutes(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static PostponeMinutes From(int value)
    {
        if (value is < MinValue or > MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"Postpone minutes must be between {MinValue} and {MaxValue}.");
        }

        return new PostponeMinutes(value);
    }

    public override string ToString() => Value.ToString();
}
