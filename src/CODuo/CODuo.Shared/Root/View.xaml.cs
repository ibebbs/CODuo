using System;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CODuo.Root
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class View : Page, IView
    {
        public View()
        {
            this.InitializeComponent();
        }

        private void LayoutPane(Border pane, bool paneVisible, Vector4 paneLocation, object paneContent, double maxHeight)
        {
            pane.Visibility = Visibility.Collapsed;

            if (paneVisible)
            {
                Canvas.SetLeft(pane, paneLocation.X);
                Canvas.SetTop(pane, paneLocation.Y);

                pane.Width = paneLocation.W;
                pane.Height = Math.Min(paneLocation.Z, maxHeight);

                pane.Child = paneContent as UIElement;
                pane.Visibility = Visibility.Visible;
            }
        }

        public void PerformLayout(Layout layout)
        {
            // Hack to ensure the Canvas element is resized before first being shown
            LayoutRow1.Height = new GridLength(1, GridUnitType.Star);
            LayoutRow2.Height = new GridLength(0);

            var height = Xaml.LayoutExtensions.GetActualHeight(Canvas);

            LayoutPane(Pane1, layout.Pane1Visible, layout.Pane1Location, layout.Pane1Content, height);
            LayoutPane(Pane2, layout.Pane2Visible, layout.Pane2Location, layout.Pane2Content, height);
        }
    }
}
