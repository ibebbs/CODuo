using Windows.Foundation;
using Windows.UI.Xaml;

namespace CODuo.Xaml
{
    public class WindowHelper
    {
        public static readonly WindowHelper Instance = new WindowHelper();

        public Rect Bounds => Window.Current.Bounds;
    }
}
