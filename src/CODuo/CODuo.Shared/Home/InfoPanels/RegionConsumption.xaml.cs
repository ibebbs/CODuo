﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CODuo.Home.InfoPanels
{
    public sealed partial class RegionConsumption : UserControl
    {
        public static readonly DependencyProperty CurrentProperty = DependencyProperty.Register("Current", typeof(Common.Region), typeof(RegionConsumption), new PropertyMetadata(null, DataPropertyChanged));
        public static readonly DependencyProperty ContainerProperty = DependencyProperty.Register("Container", typeof(Common.Container), typeof(RegionConsumption), new PropertyMetadata(null, DataPropertyChanged));

        public static readonly DependencyProperty PopulationProperty = DependencyProperty.Register("Population", typeof(double), typeof(RegionConsumption), new PropertyMetadata(65.0));
        public static readonly DependencyProperty BusinessesProperty = DependencyProperty.Register("Businesses", typeof(double), typeof(RegionConsumption), new PropertyMetadata(4.3));
        public static readonly DependencyProperty BusinessStringProperty = DependencyProperty.Register("BusinessString", typeof(string), typeof(RegionConsumption), new PropertyMetadata("4.3m"));

        public static readonly DependencyProperty DomesticConsumptionProperty = DependencyProperty.Register("DomesticConsumption", typeof(double), typeof(RegionConsumption), new PropertyMetadata(0.3));
        public static readonly DependencyProperty NonDomesticConsumptionProperty = DependencyProperty.Register("NonDomesticConsumption", typeof(double), typeof(RegionConsumption), new PropertyMetadata(0.7));
        
        private static void DataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RegionConsumption region)
            {
                region.DataChanged();
            }
        }

        public RegionConsumption()
        {
            this.InitializeComponent();
        }

        private void DataChanged()
        {
            if (Container != null && Current != null)
            {
                Population = Container.Static.UkPopulation * Current.PercentOfNationalPopulation / 1000000.0;
                Businesses = Container.Static.UkBusinesses * Current.PercentOfNationalBusinesses / 1000000.0;

                // TODO: Move this into a converter
                BusinessString = Businesses < 1
                    ? $"{Businesses * 1000:N0}k"
                    : $"{Businesses:N0}m";

                DomesticConsumption = Current.PercentOfConsumptionBeingDomestic;
                NonDomesticConsumption = 1 - Current.PercentOfConsumptionBeingDomestic;
            }
        }

        public Common.Container Container
        {
            get { return (Common.Container)GetValue(ContainerProperty); }
            set { SetValue(ContainerProperty, value); }
        }

        public Common.Region Current
        {
            get { return (Common.Region)GetValue(CurrentProperty); }
            set { SetValue(CurrentProperty, value); }
        }

        public double Population
        {
            get { return (double)GetValue(PopulationProperty); }
            set { SetValue(PopulationProperty, value); }
        }

        public double Businesses
        {
            get { return (double)GetValue(BusinessesProperty); }
            set { SetValue(BusinessesProperty, value); }
        }

        public string BusinessString
        {
            get { return (string)GetValue(BusinessStringProperty); }
            set { SetValue(BusinessStringProperty, value); }
        }

        public double DomesticConsumption
        {
            get { return (double)GetValue(DomesticConsumptionProperty); }
            set { SetValue(DomesticConsumptionProperty, value); }
        }

        public double NonDomesticConsumption
        {
            get { return (double)GetValue(NonDomesticConsumptionProperty); }
            set { SetValue(NonDomesticConsumptionProperty, value); }
        }
    }
}