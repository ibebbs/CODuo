using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using System;

namespace CODuo.Droid
{
    [Activity(
            MainLauncher = true,
            ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize,
            WindowSoftInputMode = SoftInput.AdjustPan | SoftInput.StateHidden
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
    }
}

