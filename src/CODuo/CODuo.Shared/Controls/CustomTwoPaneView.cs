using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace CODuo.Controls
{
	public enum ViewMode
	{
		Pane1Only,
		Pane2Only,
		LeftRight,
		RightLeft,
		TopBottom,
		BottomTop,
		None
	}

	public enum TwoPaneViewMode
	{
		SinglePane = 0,
		Wide = 1,
		Tall = 2,
	};

	public enum TwoPaneViewPriority
	{
		Pane1 = 0,
		Pane2 = 1,
	};

	public enum TwoPaneViewWideModeConfiguration
	{
		SinglePane = 0,
		LeftRight = 1,
		RightLeft = 2,
	};

	public enum TwoPaneViewTallModeConfiguration
	{
		SinglePane = 0,
		TopBottom = 1,
		BottomTop = 2,
	};

	internal struct DisplayRegionHelperInfo
	{
		public TwoPaneViewMode Mode { get; set; }
		public Rect[] Regions { get; set; }
	}

	internal class LifetimeHandler
	{
		private static DisplayRegionHelper _displayRegionHelper;

		internal static DisplayRegionHelper GetDisplayRegionHelperInstance()
			=> _displayRegionHelper = _displayRegionHelper ?? new DisplayRegionHelper();
	}

	internal class DisplayRegionHelper
	{
		private bool m_simulateDisplayRegions = false;
		private TwoPaneViewMode m_simulateMode = TwoPaneViewMode.SinglePane;

		static readonly Rect m_simulateWide0 = new Rect(0, 0, 300, 400);
		static readonly Rect m_simulateWide1 = new Rect(312, 0, 300, 400);
		static readonly Rect m_simulateTall0 = new Rect(0, 0, 400, 300);
		static readonly Rect m_simulateTall1 = new Rect(0, 312, 400, 300);

		internal static DisplayRegionHelperInfo GetRegionInfo()
		{
			var instance = LifetimeHandler.GetDisplayRegionHelperInstance();

			var info = new DisplayRegionHelperInfo();
			info.Mode = TwoPaneViewMode.SinglePane;

			if (instance.m_simulateDisplayRegions)
			{
				// Create fake rectangles for test app
				if (instance.m_simulateMode == TwoPaneViewMode.Wide)
				{
					info.Regions[0] = m_simulateWide0;
					info.Regions[1] = m_simulateWide1;
					info.Mode = TwoPaneViewMode.Wide;
				}
				else if (instance.m_simulateMode == TwoPaneViewMode.Tall)
				{
					info.Regions[0] = m_simulateTall0;
					info.Regions[1] = m_simulateTall1;
					info.Mode = TwoPaneViewMode.Tall;
				}
				else
				{
					info.Regions[0] = m_simulateWide0;
				}
			}
			else
			{
				// ApplicationView.GetForCurrentView throws on failure; in that case we just won't do anything.
				ApplicationView view = null;
				try
				{
					view = ApplicationView.GetForCurrentView();
				}
				catch { }

				if (view != null)
				{
					var rects = new List<Rect>(); // view.GetSpanningRects();

					if (rects.Count == 2)
					{
						info.Regions = new Rect[rects.Count];
						//info.Regions[0] = new Rect(rects[0].Location.PhysicalToLogicalPixels(), rects[0].Size.PhysicalToLogicalPixels());
						//info.Regions[1] = new Rect(rects[1].Location.PhysicalToLogicalPixels(), rects[1].Size.PhysicalToLogicalPixels());

						// Determine orientation. If neither of these are true, default to doing nothing.
						if (info.Regions[0].X < info.Regions[1].X && info.Regions[0].Y == info.Regions[1].Y)
						{
							// Double portrait
							info.Mode = TwoPaneViewMode.Wide;
						}
						else if (info.Regions[0].X == info.Regions[1].X && info.Regions[0].Y < info.Regions[1].Y)
						{
							// Double landscape
							info.Mode = TwoPaneViewMode.Tall;
						}
					}
				}
			}

			return info;
		}

		/* static */
		internal static UIElement WindowElement()
		{
			var instance = LifetimeHandler.GetDisplayRegionHelperInstance();

			if (instance.m_simulateDisplayRegions)
			{
				// Instead of returning the actual window, find the SimulatedWindow element
				UIElement window = null;

				if (Window.Current.Content is FrameworkElement fe)
				{
					// UNO TODO
					// window = SharedHelpers.FindInVisualTreeByName(fe, "SimulatedWindow");
				}

				return window;
			}
			else
			{
				return Window.Current.Content;
			}
		}

		/* static */
		internal static Rect WindowRect()
		{
			var instance = LifetimeHandler.GetDisplayRegionHelperInstance();

			if (instance.m_simulateDisplayRegions)
			{
				// Return the bounds of the simulated window
				FrameworkElement window = WindowElement() as FrameworkElement;
				Rect rc = new Rect(
					0, 0,
					(float)window.ActualWidth,
					(float)window.ActualHeight);
				return rc;
			}
			else
			{
				return Window.Current.Bounds;
			}
		}

		/* static */
		internal static void SimulateDisplayRegions(bool value)
		{
			var instance = LifetimeHandler.GetDisplayRegionHelperInstance();
			instance.m_simulateDisplayRegions = value;
		}

		/* static */
		internal static bool SimulateDisplayRegions()
		{
			var instance = LifetimeHandler.GetDisplayRegionHelperInstance();
			return instance.m_simulateDisplayRegions;
		}

		/* static */
		internal static void SimulateMode(TwoPaneViewMode value)
		{
			var instance = LifetimeHandler.GetDisplayRegionHelperInstance();
			instance.m_simulateMode = value;
		}

		/* static */
		internal static TwoPaneViewMode SimulateMode()
		{
			var instance = LifetimeHandler.GetDisplayRegionHelperInstance();
			return instance.m_simulateMode;
		}
	}

	public sealed partial class CustomTwoPaneView : Control
	{
		public static readonly DependencyProperty Pane1Property = DependencyProperty.Register(nameof(Pane1), typeof(UIElement), typeof(CustomTwoPaneView), new PropertyMetadata(null));
		public static readonly DependencyProperty Pane2Property = DependencyProperty.Register(nameof(Pane2), typeof(UIElement), typeof(CustomTwoPaneView), new PropertyMetadata(null));
		public static readonly DependencyProperty Pane1LengthProperty = DependencyProperty.Register(nameof(Pane1Length), typeof(GridLength), typeof(CustomTwoPaneView), new PropertyMetadata(GridLength.Auto));
		public static readonly DependencyProperty Pane2LengthProperty = DependencyProperty.Register(nameof(Pane2Length), typeof(GridLength), typeof(CustomTwoPaneView), new PropertyMetadata(new GridLength(1, GridUnitType.Star)));
		public static readonly DependencyProperty PanePriorityProperty = DependencyProperty.Register("PanePriority", typeof(TwoPaneViewPriority), typeof(CustomTwoPaneView), new PropertyMetadata(TwoPaneViewPriority.Pane1));
		public static readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(TwoPaneViewMode), typeof(CustomTwoPaneView), new PropertyMetadata(TwoPaneViewMode.SinglePane));
		public static readonly DependencyProperty WideModeConfigurationProperty = DependencyProperty.Register("WideModeConfiguration", typeof(TwoPaneViewWideModeConfiguration), typeof(CustomTwoPaneView), new PropertyMetadata(TwoPaneViewWideModeConfiguration.LeftRight));
		public static readonly DependencyProperty TallModeConfigurationProperty = DependencyProperty.Register("TallModeConfiguration", typeof(TwoPaneViewTallModeConfiguration), typeof(CustomTwoPaneView), new PropertyMetadata(TwoPaneViewTallModeConfiguration.TopBottom));
		public static readonly DependencyProperty MinWideModeWidthProperty = DependencyProperty.Register("MinWideModeWidth", typeof(double), typeof(CustomTwoPaneView), new PropertyMetadata(641));
		public static readonly DependencyProperty MinTallModeHeightProperty = DependencyProperty.Register("MinTallModeHeight", typeof(double), typeof(CustomTwoPaneView), new PropertyMetadata(641));

		public event TypedEventHandler<CustomTwoPaneView, object> ModeChanged;

		const string c_columnLeftName = "PART_ColumnLeft";
		const string c_columnMiddleName = "PART_ColumnMiddle";
		const string c_columnRightName = "PART_ColumnRight";
		const string c_rowTopName = "PART_RowTop";
		const string c_rowMiddleName = "PART_RowMiddle";
		const string c_rowBottomName = "PART_RowBottom";

		ViewMode m_currentMode = ViewMode.None;

		bool m_loaded = false;

		ColumnDefinition m_columnLeft;
		ColumnDefinition m_columnMiddle;
		ColumnDefinition m_columnRight;
		RowDefinition m_rowTop;
		RowDefinition m_rowMiddle;
		RowDefinition m_rowBottom;


		public CustomTwoPaneView()
		{
			DefaultStyleKey = typeof(CustomTwoPaneView);

			SizeChanged += OnSizeChanged;
			Window.Current.SizeChanged += OnWindowSizeChanged;

			//this.RegisterDisposablePropertyChangedCallback((e, s, a) => OnPropertyChanged(a));
		}

		protected override void OnApplyTemplate()
		{
			m_loaded = true;

			// UNO TODO
			//SetScrollViewerProperties(c_pane1ScrollViewerName, m_pane1LoadedRevoker);
			//SetScrollViewerProperties(c_pane2ScrollViewerName, m_pane2LoadedRevoker);

			if (GetTemplateChild(c_columnLeftName) is ColumnDefinition column)
			{
				m_columnLeft = column;
			}
			if (GetTemplateChild(c_columnMiddleName) is ColumnDefinition middleColumn)
			{
				m_columnMiddle = middleColumn;
			}
			if (GetTemplateChild(c_columnRightName) is ColumnDefinition columnRight)
			{
				m_columnRight = columnRight;
			}
			if (GetTemplateChild(c_rowTopName) is RowDefinition rowTop)
			{
				m_rowTop = rowTop;
			}
			if (GetTemplateChild(c_rowMiddleName) is RowDefinition rowMiddle)
			{
				m_rowMiddle = rowMiddle;
			}
			if (GetTemplateChild(c_rowBottomName) is RowDefinition rowBottom)
			{
				m_rowBottom = rowBottom;
			}
		}

		void OnWindowSizeChanged(object sender, WindowSizeChangedEventArgs args)
		{
			UpdateMode();
		}

		void OnSizeChanged(object sender, SizeChangedEventArgs args)
		{
			UpdateMode();
		}

		void UpdateMode()
		{
			// Don't bother running this logic until after we hit OnApplyTemplate.
			if (!m_loaded) return;

			double controlWidth = ActualWidth;
			double controlHeight = ActualHeight;

			ViewMode newMode = (PanePriority == TwoPaneViewPriority.Pane1) ? ViewMode.Pane1Only : ViewMode.Pane2Only;

			// Calculate new mode
			DisplayRegionHelperInfo info = DisplayRegionHelper.GetRegionInfo();
			Rect rcControl = GetControlRect();
			bool isInMultipleRegions = false; // IsInMultipleRegions(info, rcControl);

			if (isInMultipleRegions)
			{
				if (info.Mode == TwoPaneViewMode.Wide)
				{
					// Regions are laid out horizontally
					if (WideModeConfiguration != TwoPaneViewWideModeConfiguration.SinglePane)
					{
						newMode = (WideModeConfiguration == TwoPaneViewWideModeConfiguration.LeftRight) ? ViewMode.LeftRight : ViewMode.RightLeft;
					}
				}
				else if (info.Mode == TwoPaneViewMode.Tall)
				{
					// Regions are laid out vertically
					if (TallModeConfiguration != TwoPaneViewTallModeConfiguration.SinglePane)
					{
						newMode = (TallModeConfiguration == TwoPaneViewTallModeConfiguration.TopBottom) ? ViewMode.TopBottom : ViewMode.BottomTop;
					}
				}
			}
			else
			{
				// One region
				if (controlWidth > MinWideModeWidth && WideModeConfiguration != TwoPaneViewWideModeConfiguration.SinglePane)
				{
					// Split horizontally
					newMode = (WideModeConfiguration == TwoPaneViewWideModeConfiguration.LeftRight) ? ViewMode.LeftRight : ViewMode.RightLeft;
				}
				else if (controlHeight > MinTallModeHeight && TallModeConfiguration != TwoPaneViewTallModeConfiguration.SinglePane)
				{
					// Split vertically
					newMode = (TallModeConfiguration == TwoPaneViewTallModeConfiguration.TopBottom) ? ViewMode.TopBottom : ViewMode.BottomTop;
				}
			}

			// Update row/column sizes (this may need to happen even if the mode doesn't change)
			UpdateRowsColumns(newMode, info, rcControl);

			// Update mode if necessary
			if (newMode != m_currentMode)
			{
				m_currentMode = newMode;

				TwoPaneViewMode newViewMode = TwoPaneViewMode.SinglePane;

				switch (m_currentMode)
				{
					case ViewMode.Pane1Only: VisualStateManager.GoToState(this, "ViewMode_OneOnly", true); break;
					case ViewMode.Pane2Only: VisualStateManager.GoToState(this, "ViewMode_TwoOnly", true); break;
					case ViewMode.LeftRight: VisualStateManager.GoToState(this, "ViewMode_LeftRight", true); newViewMode = TwoPaneViewMode.Wide; break;
					case ViewMode.RightLeft: VisualStateManager.GoToState(this, "ViewMode_RightLeft", true); newViewMode = TwoPaneViewMode.Wide; break;
					case ViewMode.TopBottom: VisualStateManager.GoToState(this, "ViewMode_TopBottom", true); newViewMode = TwoPaneViewMode.Tall; break;
					case ViewMode.BottomTop: VisualStateManager.GoToState(this, "ViewMode_BottomTop", true); newViewMode = TwoPaneViewMode.Tall; break;
				}

				if (newViewMode != Mode)
				{
					SetValue(ModeProperty, newViewMode);
					ModeChanged?.Invoke(this, this);
				}
			}
		}

		void UpdateRowsColumns(ViewMode newMode, DisplayRegionHelperInfo info, Rect rcControl)
		{
			if (m_columnLeft != null && m_columnMiddle != null && m_columnRight != null && m_rowTop != null && m_rowMiddle != null && m_rowBottom != null)
			{
				// Reset split lengths
				m_columnMiddle.Width = new GridLength(0, GridUnitType.Pixel);
				m_rowMiddle.Height = new GridLength(0, GridUnitType.Pixel);

				// Set columns lengths
				if (newMode == ViewMode.LeftRight || newMode == ViewMode.RightLeft)
				{
					m_columnLeft.Width = (newMode == ViewMode.LeftRight) ? Pane1Length : Pane2Length;
					m_columnRight.Width = (newMode == ViewMode.LeftRight) ? Pane2Length : Pane1Length;
				}
				else
				{
					m_columnLeft.Width = new GridLength(1, GridUnitType.Star);
					m_columnRight.Width = new GridLength(0, GridUnitType.Pixel);
				}

				// Set row lengths
				if (newMode == ViewMode.TopBottom || newMode == ViewMode.BottomTop)
				{
					m_rowTop.Height = (newMode == ViewMode.TopBottom) ? Pane1Length : Pane2Length;
					m_rowBottom.Height = (newMode == ViewMode.TopBottom) ? Pane2Length : Pane1Length;
				}
				else
				{
					m_rowTop.Height = new GridLength(1, GridUnitType.Star);
					m_rowBottom.Height = new GridLength(0, GridUnitType.Pixel);
				}

				// Handle regions
				if (IsInMultipleRegions(info, rcControl) && newMode != ViewMode.Pane1Only && newMode != ViewMode.Pane2Only)
				{
					Rect rc1 = info.Regions[0];
					Rect rc2 = info.Regions[1];
					Rect rcWindow = DisplayRegionHelper.WindowRect();

					if (info.Mode == TwoPaneViewMode.Wide)
					{
						m_columnMiddle.Width = new GridLength(rc2.X - rc1.Width, GridUnitType.Pixel);

						m_columnLeft.Width = new GridLength(rc1.Width - rcControl.X, GridUnitType.Pixel);

						// UNO TODO: Max is needed when regions don't match the Window size orientation
						m_columnRight.Width = new GridLength(Math.Max(0, rc2.Width - ((rcWindow.Width - rcControl.Width) - rcControl.X)), GridUnitType.Pixel);
					}
					else
					{
						m_rowMiddle.Height = new GridLength(rc2.Y - rc1.Height, GridUnitType.Pixel);

						m_rowTop.Height = new GridLength(rc1.Height - rcControl.Y, GridUnitType.Pixel);

						// UNO TODO: Max is needed when regions don't match the Window size orientation
						m_rowBottom.Height = new GridLength(Math.Max(0, rc2.Height - ((rcWindow.Height - rcControl.Height) - rcControl.Y)), GridUnitType.Pixel);
					}
				}
			}
		}

		Rect GetControlRect()
		{
			// Find out where this control is in the window
			GeneralTransform transform = TransformToVisual(DisplayRegionHelper.WindowElement());
			return transform.TransformBounds(new Rect(0, 0, (float)ActualWidth, (float)ActualHeight));
		}

		bool IsInMultipleRegions(DisplayRegionHelperInfo info, Rect rcControl)
		{
			bool isInMultipleRegions = false;

			if (info.Mode != TwoPaneViewMode.SinglePane)
			{
				Rect rc1 = info.Regions[0];
				Rect rc2 = info.Regions[1];
				Rect rcWindow = DisplayRegionHelper.WindowRect();

				if (info.Mode == TwoPaneViewMode.Wide)
				{
					// Check that the control is over the split
					if (rcControl.X < rc1.Width && rcControl.X + rcControl.Width > rc2.X)
					{
						isInMultipleRegions = true;
					}
				}
				else if (info.Mode == TwoPaneViewMode.Tall)
				{
					// Check that the control is over the split
					if (rcControl.Y < rc1.Height && rcControl.Y + rcControl.Height > rc2.Y)
					{
						isInMultipleRegions = true;
					}
				}
			}

			return isInMultipleRegions;
		}

		void OnPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			var property = args.Property;

			// Clamp property values -- early return if the values were clamped as we'll come back with the new value.
			if (property == MinWideModeWidthProperty || property == MinTallModeHeightProperty)
			{
				var value = (double)args.NewValue;
				var clampedValue = Math.Max(0.0, value);
				if (clampedValue != value)
				{
					SetValue(property, clampedValue);
					return;
				}
			}

			if (property == PanePriorityProperty
				|| property == Pane1LengthProperty
				|| property == Pane2LengthProperty
				|| property == WideModeConfigurationProperty
				|| property == TallModeConfigurationProperty
				|| property == MinWideModeWidthProperty
				|| property == MinTallModeHeightProperty)
			{
				UpdateMode();
			}
		}


		public UIElement Pane1
		{
			get { return (UIElement)GetValue(Pane1Property); }
			set { SetValue(Pane1Property, value); }
		}

		public UIElement Pane2
		{
			get { return (UIElement)GetValue(Pane2Property); }
			set { SetValue(Pane2Property, value); }
		}

		public GridLength Pane1Length
		{
			get { return (GridLength)GetValue(Pane1LengthProperty); }
			set { SetValue(Pane1LengthProperty, value); }
		}

		public GridLength Pane2Length
		{
			get { return (GridLength)GetValue(Pane2LengthProperty); }
			set { SetValue(Pane2LengthProperty, value); }
		}

		public TwoPaneViewPriority PanePriority
		{
			get { return (TwoPaneViewPriority)GetValue(PanePriorityProperty); }
			set { SetValue(PanePriorityProperty, value); }
		}

		public TwoPaneViewMode Mode
		{
			get { return (TwoPaneViewMode)GetValue(ModeProperty); }
			set { SetValue(ModeProperty, value); }
		}

		public TwoPaneViewWideModeConfiguration WideModeConfiguration
		{
			get { return (TwoPaneViewWideModeConfiguration)GetValue(WideModeConfigurationProperty); }
			set { SetValue(WideModeConfigurationProperty, value); }
		}

		public TwoPaneViewTallModeConfiguration TallModeConfiguration
		{
			get { return (TwoPaneViewTallModeConfiguration)GetValue(TallModeConfigurationProperty); }
			set { SetValue(TallModeConfigurationProperty, value); }
		}

		public double MinWideModeWidth
		{
			get { return (double)GetValue(MinWideModeWidthProperty); }
			set { SetValue(MinWideModeWidthProperty, value); }
		}

		public double MinTallModeHeight
		{
			get { return (double)GetValue(MinTallModeHeightProperty); }
			set { SetValue(MinTallModeHeightProperty, value); }
		}
	}
}
