using System;
using System.Linq;
using UIKit;
using Uno.Extensions;
using Windows.UI.Xaml;
using Xamarin.Essentials;

namespace CODuo.Platform
{
    public class Information : IInformation
    {
        public Information()
        {
            var version = DeviceInfo.VersionString
                .Split('.')
                .Select(part => int.TryParse(part, out int value) ? value : 0)
                .Concat(new[] { 0, 0, 0, 0 })
                .Take(4)
                .ToArray();

            OperatingSystemVersion = new Version(version[0], version[1], version[2], version[3]);

            RequiredMargin = GetRequiredMargin(OperatingSystemVersion);
        }

        private Thickness FromSharedApplicationWindowSafeAreaInserts()
        {
            var insets = UIApplication.SharedApplication.Windows[0].SafeAreaInsets;

            return new Thickness(insets.Left, insets.Top, insets.Right, insets.Bottom);
        }

        private Thickness GetRequiredMargin(Version os)
        {
            return os.Major >= 11
                ? FromSharedApplicationWindowSafeAreaInserts()
                : Thickness.Empty;
        }

        public string DeviceManufacturer => DeviceInfo.Manufacturer;
        public string DeviceModel => UIDevice.CurrentDevice.Model;
        public string DeviceFamily => DeviceInfo.Idiom.ToString();

        public string OperatingSystem => DeviceInfo.Platform.ToString();
        public Version OperatingSystemVersion { get; }
        public Thickness RequiredMargin { get; }
    }
}