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

        private void LayoutPane(Border pane, bool paneVisible, Vector4 paneLocation, object paneContent, double dipScaling)
        {
            pane.Visibility = Visibility.Collapsed;

            if (paneVisible)
            {
                Canvas.SetLeft(pane, paneLocation.X);
                Canvas.SetTop(pane, paneLocation.Y);

                pane.Width = paneLocation.W;
                pane.Height = paneLocation.Z;

                Attributes.SetWidth(pane, pane.Width);
                Attributes.SetHeight(pane, pane.Height);

                pane.Child = paneContent as UIElement;
                pane.Visibility = Visibility.Visible;
            }
        }

        public void PerformLayout(Layout layout)
        {
            LayoutPane(Pane1, layout.Pane1Visible, layout.Pane1Location, layout.Pane1Content, layout.DipScaling);
            LayoutPane(Pane2, layout.Pane2Visible, layout.Pane2Location, layout.Pane2Content, layout.DipScaling);
        }
    }
}
