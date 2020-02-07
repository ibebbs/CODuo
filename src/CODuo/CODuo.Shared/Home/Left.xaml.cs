using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CODuo.Home
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Left : Page
    {
        public Left()
        {
            this.InitializeComponent();
        }

        public double GetHeight()
        {
            var size = Xaml.LayoutExtensions.FindSize(this);

            if (size.HasValue)
            {
                return Math.Min(size.Value.Item1, size.Value.Item2);
            }
            else
            {
                return 500;
            }
        }
    }
}
