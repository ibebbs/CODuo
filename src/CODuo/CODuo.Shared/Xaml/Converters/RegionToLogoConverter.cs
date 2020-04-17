using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace CODuo.Xaml.Converters
{
    public class RegionToLogoConverter : IValueConverter
    {
        private static readonly IReadOnlyDictionary<int, string> ResourceForRegion = new Dictionary<int, string>
        {
            { 0, "NationalGrid.png" },
            { 1, "ScottishAndSouthernEnergy.png" },
            { 2, "SPEnergyNetworks.png" },
            { 3, "ElectrictyNorthwest.png" },
            { 4, "NorthernPowergrid.png" },
            { 5, "NorthernPowergrid.png" },
            { 6, "SPEnergyNetworks.png" },
            { 7, "WesternPowerDistribution.png" },
            { 8, "WesternPowerDistribution.png" },
            { 9, "WesternPowerDistribution.png" },
            { 10, "UKPowerNetworks.png" },
            { 11, "ScottishAndSouthernEnergy.png" },
            { 12, "ScottishAndSouthernEnergy.png" },
            { 13, "UKPowerNetworks.png" },
            { 14, "UKPowerNetworks.png" },
        };
        
        private BitmapImage ImageForRegion(int region)
        {
            var resourceForRegion = ResourceForRegion[region];

            BitmapImage bitmapImage = new BitmapImage();
            Uri uri = new Uri($"ms-appx:///Assets/Logos/{resourceForRegion}");
            bitmapImage.UriSource = uri;

            return bitmapImage;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value switch
            {
                Common.Region region when region.Id > -1 && region.Id < 15 => ImageForRegion(region.Id),
                Common.RegionGeneration region when region.RegionId > -1 && region.RegionId < 15 => ImageForRegion(region.RegionId),
                int i when i > -1 && i < 15 => ImageForRegion(i),
                _ => DependencyProperty.UnsetValue
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
