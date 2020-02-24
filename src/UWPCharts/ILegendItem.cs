﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Media;
using Windows.UI.Xaml.Media;

namespace Ailon.QuickCharts
{
    /// <summary>
    /// Interface which should be implemented by classes used as legend items.
    /// </summary>
    public interface ILegendItem
    {
        /// <summary>
        /// Gets or sets title shown in Legend.
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// Gets or sets brush for the Legend key.
        /// </summary>
        Brush Brush { get; set; }
    }
}
