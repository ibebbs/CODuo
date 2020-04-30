using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using System;

namespace CODuo.Droid
{
    [Activity(
            MainLauncher = true,
            ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize,
            WindowSoftInputMode = SoftInput.AdjustPan | SoftInput.StateHidden,
            Icon = "@drawable/icon"
        )]
    public class MainActivity : Windows.UI.Xaml.ApplicationActivity
    {
        public static Activity Activity { get; private set; }

        public MainActivity() : base() 
        {
            Activity = this;
        }

        public MainActivity(IntPtr ptr, JniHandleOwnership owner) : base(ptr, owner)
        {
            Activity = this;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Essentials.Platform.Init(this, bundle);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

