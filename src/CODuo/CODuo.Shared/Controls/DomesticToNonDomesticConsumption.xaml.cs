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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CODuo.Controls
{
    public sealed partial class DomesticToNonDomesticConsumption : UserControl
    {

        public static readonly DependencyProperty DomesticConsumptionLengthProperty = DependencyProperty.Register(nameof(DomesticConsumptionLength), typeof(GridLength), typeof(DomesticToNonDomesticConsumption), new PropertyMetadata(new GridLength(0.5, GridUnitType.Star)));
        public static readonly DependencyProperty NonDomesticConsumptionLengthProperty = DependencyProperty.Register(nameof(NonDomesticConsumptionLength), typeof(GridLength), typeof(DomesticToNonDomesticConsumption), new PropertyMetadata(new GridLength(0.5, GridUnitType.Star)));

        public static readonly DependencyProperty NonDomesticConsumptionPercentProperty = DependencyProperty.Register("NonDomesticConsumptionPercent", typeof(double), typeof(DomesticToNonDomesticConsumption), new PropertyMetadata(0.5));
        public static readonly DependencyProperty DomesticConsumptionPercentProperty = DependencyProperty.Register("DomesticConsumptionPercent", typeof(double), typeof(DomesticToNonDomesticConsumption), new PropertyMetadata(0.5, DomesticConsumptionPercentPropertyChanged));

        private static void DomesticConsumptionPercentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DomesticToNonDomesticConsumption v && e.NewValue is double domesticConsumptionPercent)
            {
                double nonDomesticConsumptionPercent = 1.0 - domesticConsumptionPercent;
                v.SetValue(NonDomesticConsumptionPercentProperty, nonDomesticConsumptionPercent);
                v.SetValue(DomesticConsumptionLengthProperty, new GridLength(domesticConsumptionPercent, GridUnitType.Star));
                v.SetValue(NonDomesticConsumptionLengthProperty, new GridLength(nonDomesticConsumptionPercent, GridUnitType.Star));
            }
        }

        public DomesticToNonDomesticConsumption()
        {
            this.InitializeComponent();
        }

        public GridLength DomesticConsumptionLength
        {
            get { return (GridLength)GetValue(DomesticConsumptionLengthProperty); }
        }

        public GridLength NonDomesticConsumptionLength
        {
            get { return (GridLength)GetValue(NonDomesticConsumptionLengthProperty); }
        }

        public double NonDomesticConsumptionPercent
        {
            get { return (double)GetValue(NonDomesticConsumptionPercentProperty); }
        }

        public double DomesticConsumptionPercent
        {
            get { return (double)GetValue(DomesticConsumptionPercentProperty); }
            set { SetValue(DomesticConsumptionPercentProperty, value); }
        }
    }
}
