using RememberMePlusApp.Application.Alarms;

namespace RememberMePlusApp;

public sealed class AlarmAlertCoordinator(IAlarmActionService alarmActionService) : IAlarmAlertCoordinator
{
    private readonly IAlarmActionService _alarmActionService = alarmActionService;

    public Task ShowAsync(ScheduledReminderTask task, int snoozeMinutes, CancellationToken cancellationToken = default)
    {
        return MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var page = Microsoft.Maui.Controls.Application.Current?.Windows.FirstOrDefault()?.Page;
            if (page is null)
            {
                return;
            }

            if (page.Navigation.ModalStack.OfType<AlarmAlertPage>().Any())
            {
                return;
            }

            await page.Navigation.PushModalAsync(new AlarmAlertPage(task, snoozeMinutes, _alarmActionService));
        });
    }
}
