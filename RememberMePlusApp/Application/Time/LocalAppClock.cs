namespace RememberMePlusApp.Application.Time;

public sealed class LocalAppClock : IAppClock
{
    public DateTime Now => DateTime.Now;
}
