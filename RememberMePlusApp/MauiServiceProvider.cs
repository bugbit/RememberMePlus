namespace RememberMePlusApp;

public static class MauiServiceProvider
{
    public static IServiceProvider Current { get; private set; } = default!;

    public static void Initialize(IServiceProvider serviceProvider)
    {
        Current = serviceProvider;
    }
}
