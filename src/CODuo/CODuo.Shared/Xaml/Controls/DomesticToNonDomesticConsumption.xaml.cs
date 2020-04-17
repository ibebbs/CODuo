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

namespace CODuo.Xaml.Controls
{
    public sealed partial class DomesticToNonDomesticConsumption : UserControl
    {
        public static readonly DependencyProperty DomesticConsumptionLengthProperty = DependencyProperty.Register(nameof(DomesticConsumptionLength), typeof(GridLength), typeof(DomesticToNonDomesticConsumption), new PropertyMetadata(new GridLength(0.5, GridUnitType.Star)));
        public static readonly DependencyProperty NonDomesticConsumptionLengthProperty = DependencyProperty.Register(nameof(NonDomesticConsumptionLength), typeof(GridLength), typeof(DomesticToNonDomesticConsumption), new PropertyMetadata(new GridLength(0.5, GridUnitType.Star)));

        public static readonly DependencyProperty NonDomesticConsumptionPercentProperty = DependencyProperty.Register("NonDomesticConsumptionPercent", typeof(double), typeof(DomesticToNonDomesticConsumption), new PropertyMetadata(0.5));
        public static readonly DependencyProperty DomesticConsumptionPercentProperty = DependencyProperty.Register("DomesticConsumptionPercent", typeof(double), typeof(DomesticToNonDomesticConsumption), new PropertyMetadata(0.5, DependentPropertyChanged));

        public static readonly DependencyProperty TotalGenerationProperty = DependencyProperty.Register("TotalGeneration", typeof(double), typeof(DomesticToNonDomesticConsumption), new PropertyMetadata(0.0, DependentPropertyChanged));
        public static readonly DependencyProperty DomesticConsumptionProperty = DependencyProperty.Register("DomesticConsumption", typeof(double), typeof(DomesticToNonDomesticConsumption), new PropertyMetadata(0.0));
        public static readonly DependencyProperty NonDomesticConsumptionProperty = DependencyProperty.Register("NonDomesticConsumption", typeof(double), typeof(DomesticToNonDomesticConsumption), new PropertyMetadata(0.0));

        private static void DependentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DomesticToNonDomesticConsumption v)
            {
                v.Refresh();
            }
        }

        public DomesticToNonDomesticConsumption()
        {
            this.InitializeComponent();
        }

        private void Refresh()
        {
            double totalGeneration = (double)GetValue(TotalGenerationProperty);
            double domesticConsumptionPercent = (double)GetValue(DomesticConsumptionPercentProperty);
            double nonDomesticConsumptionPercent = 1.0 - domesticConsumptionPercent;

            SetValue(NonDomesticConsumptionPercentProperty, nonDomesticConsumptionPercent);
            SetValue(DomesticConsumptionLengthProperty, new GridLength(domesticConsumptionPercent, GridUnitType.Star));
            SetValue(NonDomesticConsumptionLengthProperty, new GridLength(nonDomesticConsumptionPercent, GridUnitType.Star));
            SetValue(DomesticConsumptionProperty, totalGeneration * domesticConsumptionPercent);
            SetValue(NonDomesticConsumptionProperty, totalGeneration * nonDomesticConsumptionPercent);
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

        public double TotalGeneration
        {
            get { return (double)GetValue(TotalGenerationProperty); }
            set { SetValue(TotalGenerationProperty, value); }
        }


        public double DomesticConsumption
        {
            get { return (double)GetValue(DomesticConsumptionProperty); }
        }


        public double NonDomesticConsumption
        {
            get { return (double)GetValue(NonDomesticConsumptionProperty); }
        }
    }
}
