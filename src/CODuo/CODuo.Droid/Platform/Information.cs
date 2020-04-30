using Android.Views;
using System;
using System.Linq;
using Windows.Foundation;
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
        }

        public string DeviceManufacturer => DeviceInfo.Manufacturer;
        public string DeviceModel => DeviceInfo.Model;
        public string DeviceFamily => DeviceInfo.Idiom.ToString();

        public string OperatingSystem => DeviceInfo.Platform.ToString();
        public Version OperatingSystemVersion { get; }
        public Thickness RequiredMargin => Thickness.Empty;
    }
}