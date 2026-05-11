namespace RememberMePlusApp.Domain.Tasks;

public readonly record struct PostponeMinutes
{
    public PostponeMinutes(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Postpone minutes must be greater than zero.");
        }

        Value = value;
    }

    public int Value { get; }
}
