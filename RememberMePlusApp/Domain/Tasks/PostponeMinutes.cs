namespace RememberMePlusApp.Domain.Tasks;

public sealed record PostponeMinutes
{
    private PostponeMinutes(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static PostponeMinutes Create(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), value, "Postpone minutes must be greater than zero.");
        }

        return new PostponeMinutes(value);
    }
}
