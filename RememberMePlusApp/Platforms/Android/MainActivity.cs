using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Microsoft.Extensions.DependencyInjection;
using RememberMePlusApp.Application.Alarms;
using RememberMePlusApp.Platforms.Android.Alarms;

namespace RememberMePlusApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestNotificationPermission();
            HandleAlarmIntent(Intent);
        }

        protected override void OnNewIntent(Intent? intent)
        {
            base.OnNewIntent(intent);
            HandleAlarmIntent(intent);
        }

        private void RequestNotificationPermission()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu &&
                CheckSelfPermission(global::Android.Manifest.Permission.PostNotifications) != Permission.Granted)
            {
                RequestPermissions([global::Android.Manifest.Permission.PostNotifications], 10);
            }
        }

        private static void HandleAlarmIntent(Intent? intent)
        {
            if (intent?.GetBooleanExtra(AndroidAlarmConstants.ExtraHandledByReceiver, false) == true)
            {
                return;
            }

            var taskId = intent?.GetLongExtra(AndroidAlarmConstants.ExtraTaskId, 0) ?? 0;
            if (taskId <= 0)
            {
                return;
            }

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var handler = MauiServiceProvider.Current.GetService<IAlarmTriggerHandler>();
                if (handler is not null)
                {
                    await handler.HandleAsync(taskId);
                }
            });
        }
    }
}
