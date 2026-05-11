using Android.App;
using Android.Content;
using AndroidX.Core.App;
using RememberMePlusApp.Application.Alarms;

namespace RememberMePlusApp.Platforms.Android.Alarms;

public sealed class AndroidAlarmNotificationService : IAlarmNotificationService
{
    public Task NotifyAsync(ScheduledReminderTask task, CancellationToken cancellationToken = default)
    {
        var context = global::Android.App.Application.Context;
        var notificationManager = NotificationManagerCompat.From(context);
        EnsureChannel(context);

        var intent = new Intent(context, typeof(MainActivity));
        intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.SingleTop | ActivityFlags.ClearTop);
        intent.PutExtra(AndroidAlarmConstants.ExtraTaskId, task.Id);

        var pendingIntent = PendingIntent.GetActivity(
            context,
            Convert.ToInt32(task.Id),
            intent,
            PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

        var notification = new NotificationCompat.Builder(context, AndroidAlarmConstants.NotificationChannelId)
            .SetSmallIcon(Resource.Mipmap.appicon)
            .SetContentTitle("RememberMe+")
            .SetContentText($"Tarea vencida: {task.Title}")
            .SetPriority(NotificationCompat.PriorityHigh)
            .SetCategory(NotificationCompat.CategoryAlarm)
            .SetAutoCancel(true)
            .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
            .SetContentIntent(pendingIntent)
            .Build();

        notificationManager.Notify(Convert.ToInt32(task.Id), notification);
        return Task.CompletedTask;
    }

    private static void EnsureChannel(Context context)
    {
        if (global::Android.OS.Build.VERSION.SdkInt < global::Android.OS.BuildVersionCodes.O)
        {
            return;
        }

        var channel = new NotificationChannel(
            AndroidAlarmConstants.NotificationChannelId,
            "Recordatorios",
            NotificationImportance.High)
        {
            Description = "Alarmas de tareas vencidas"
        };

        channel.EnableVibration(true);
        channel.SetSound(global::Android.Media.RingtoneManager.GetDefaultUri(global::Android.Media.RingtoneType.Alarm), null);

        var notificationManager = (NotificationManager?)context.GetSystemService(Context.NotificationService);
        notificationManager?.CreateNotificationChannel(channel);
    }
}
