using Android.App;
using Android.Content;
using Microsoft.Extensions.DependencyInjection;
using RememberMePlusApp.Application.Alarms;

namespace RememberMePlusApp.Platforms.Android.Alarms;

[BroadcastReceiver(Enabled = true, Exported = false)]
[IntentFilter([AndroidAlarmConstants.ActionReminderAlarm])]
public sealed class ReminderAlarmReceiver : BroadcastReceiver
{
    public override void OnReceive(Context? context, Intent? intent)
    {
        if (context is null || intent is null)
        {
            return;
        }

        var taskId = intent.GetLongExtra(AndroidAlarmConstants.ExtraTaskId, 0);
        if (taskId <= 0)
        {
            return;
        }

        var activityIntent = new Intent(context, typeof(MainActivity));
        activityIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.SingleTop | ActivityFlags.ClearTop);
        activityIntent.PutExtra(AndroidAlarmConstants.ExtraTaskId, taskId);
        activityIntent.PutExtra(AndroidAlarmConstants.ExtraHandledByReceiver, true);
        context.StartActivity(activityIntent);

        var pendingResult = GoAsync();
        _ = Task.Run(async () =>
        {
            try
            {
                var handler = MauiServiceProvider.Current.GetService<IAlarmTriggerHandler>();
                if (handler is not null)
                {
                    await handler.HandleAsync(taskId);
                }
            }
            finally
            {
                pendingResult.Finish();
            }
        });
    }
}
