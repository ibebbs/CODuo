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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CODuo.Home.InfoPanels
{
    public sealed partial class Region : UserControl
    {
        private const string LastUpdateFormat = "hh:mmtt on dddd, dd MMMM yyyy";
        private const string LastUpdateTimeFormat = "hh:mmtt";
        private const string LastUpdateDateFormat = "dddd, dd MMMM yyyy";

        public static readonly DependencyProperty CurrentProperty = DependencyProperty.Register("Current", typeof(Common.Region), typeof(Region), new PropertyMetadata(null, DataPropertyChanged));
        public static readonly DependencyProperty ContainerProperty = DependencyProperty.Register("Container", typeof(Common.Container), typeof(Region), new PropertyMetadata(null, DataPropertyChanged));

        public static readonly DependencyProperty LastUpdateProperty = DependencyProperty.Register(nameof(LastUpdate), typeof(string), typeof(Region), new PropertyMetadata(DateTime.Now.ToString(LastUpdateFormat)));

        public static readonly DependencyProperty PopulationProperty = DependencyProperty.Register("Population", typeof(double), typeof(Region), new PropertyMetadata(65.0));
        public static readonly DependencyProperty BusinessesProperty = DependencyProperty.Register("Businesses", typeof(double), typeof(Region), new PropertyMetadata(4.3));
        public static readonly DependencyProperty BusinessStringProperty = DependencyProperty.Register("BusinessString", typeof(string), typeof(Region), new PropertyMetadata("4.3m"));

        public static readonly DependencyProperty DomesticConsumptionProperty = DependencyProperty.Register("DomesticConsumption", typeof(double), typeof(Region), new PropertyMetadata(0.3));
        public static readonly DependencyProperty NonDomesticConsumptionProperty = DependencyProperty.Register("NonDomesticConsumption", typeof(double), typeof(Region), new PropertyMetadata(0.7));
        
        private static void DataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Region region)
            {
                region.OnDataChanged();
            }
        }

        public Region()
        {
            this.InitializeComponent();
        }

        private void OnDataChanged()
        {
            if (Container != null && Current != null)
            {
                var lastUpdate = (Container?.LastUpdated ?? DateTime.UtcNow).ToLocalTime();

                LastUpdate = $"{lastUpdate.ToString(LastUpdateTimeFormat).ToLower()} on {lastUpdate.ToString(LastUpdateDateFormat)}";

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

        public string LastUpdate
        {
            get { return (string)GetValue(LastUpdateProperty); }
            set { SetValue(LastUpdateProperty, value); }
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

        //public Brush[] PopulationBrushes
        //{
        //    get { return (Brush[])GetValue(PopulationBrushesProperty); }
        //    set { SetValue(PopulationBrushesProperty, value); }
        //}

        //public Brush[] BusinessBrushes
        //{
        //    get { return (Brush[])GetValue(BusinessBrushesProperty); }
        //    set { SetValue(BusinessBrushesProperty, value); }
        //}
    }
}
