using System;
using Windows.UI.Xaml;

namespace CODuo.Platform
{
    public interface IInformation
    {
        string DeviceManufacturer { get; }
        string DeviceModel { get; }
        string DeviceFamily { get; }

        string OperatingSystem { get; }
        Version OperatingSystemVersion { get; }

        Thickness RequiredMargin { get; }
    }
}
