using Android.App;
using Android.Content;
using RememberMePlusApp.Application.Alarms;

namespace RememberMePlusApp.Platforms.Android.Alarms;

public sealed class AndroidAlarmScheduler : IAlarmScheduler
{
    public Task ScheduleAsync(ScheduledReminderTask task, CancellationToken cancellationToken = default)
    {
        var context = global::Android.App.Application.Context;
        var alarmManager = (AlarmManager?)context.GetSystemService(Context.AlarmService);
        if (alarmManager is null)
        {
            return Task.CompletedTask;
        }

        var intent = new Intent(context, typeof(ReminderAlarmReceiver));
        intent.SetAction(AndroidAlarmConstants.ActionReminderAlarm);
        intent.PutExtra(AndroidAlarmConstants.ExtraTaskId, task.Id);
        intent.PutExtra(AndroidAlarmConstants.ExtraTaskTitle, task.Title);

        var pendingIntent = PendingIntent.GetBroadcast(
            context,
            AndroidAlarmConstants.RequestCode,
            intent,
            PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

        var notifyAt = task.NotifyAt.TrimToMinute();
        var triggerAtMillis = new DateTimeOffset(notifyAt).ToUnixTimeMilliseconds();

        if (global::Android.OS.Build.VERSION.SdkInt >= global::Android.OS.BuildVersionCodes.M)
        {
            alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, triggerAtMillis, pendingIntent);
            return Task.CompletedTask;
        }

        alarmManager.SetExact(AlarmType.RtcWakeup, triggerAtMillis, pendingIntent);
        return Task.CompletedTask;
    }
}
