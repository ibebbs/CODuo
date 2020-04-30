using System;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Profile;
using Windows.UI.Xaml;

namespace CODuo.Platform
{
    public class Information : IInformation
    {
        private static readonly Thickness Empty = new Thickness(0, 0, 0, 0);

        private readonly EasClientDeviceInformation _deviceInfo;

        public Information()
        {
            _deviceInfo = new EasClientDeviceInformation();
            ulong version = ulong.Parse(AnalyticsInfo.VersionInfo.DeviceFamilyVersion);
            OperatingSystemVersion = new Version(
                (ushort)((version & 0xFFFF000000000000L) >> 48),
                (ushort)((version & 0x0000FFFF00000000L) >> 32),
                (ushort)((version & 0x00000000FFFF0000L) >> 16),
                (ushort)(version & 0x000000000000FFFFL));
        }

        public string DeviceManufacturer => _deviceInfo.SystemManufacturer;
        public string DeviceModel => _deviceInfo.SystemProductName;
        public string DeviceFamily => AnalyticsInfo.VersionInfo.DeviceFamily;

        public string OperatingSystem => _deviceInfo.OperatingSystem;
        public Version OperatingSystemVersion { get; }
        public Thickness RequiredMargin => Empty;
    }
}
