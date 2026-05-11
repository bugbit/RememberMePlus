using RememberMePlusApp.Application.Alarms;

namespace RememberMePlusApp;

public partial class AlarmAlertPage : ContentPage
{
    private readonly ScheduledReminderTask _task;
    private readonly int _snoozeMinutes;
    private readonly IAlarmActionService _alarmActionService;

    public AlarmAlertPage(
        ScheduledReminderTask task,
        int snoozeMinutes,
        IAlarmActionService alarmActionService)
    {
        _task = task;
        _snoozeMinutes = snoozeMinutes;
        _alarmActionService = alarmActionService;

        InitializeComponent();

        TaskTitleLabel.Text = task.Title;
        SnoozeInfoLabel.Text = $"Puedes posponerla {_snoozeMinutes} min.";
        SnoozeButton.Text = $"Posponer {_snoozeMinutes} min";
    }

    private async void OnCompleteClicked(object? sender, EventArgs e)
    {
        await _alarmActionService.CompleteAsync(_task.Id);
        await CloseAsync();
    }

    private async void OnSnoozeClicked(object? sender, EventArgs e)
    {
        await _alarmActionService.SnoozeAsync(_task.Id, _snoozeMinutes);
        await CloseAsync();
    }

    private async Task CloseAsync()
    {
        if (Navigation.ModalStack.Contains(this))
        {
            await Navigation.PopModalAsync();
        }
    }
}
