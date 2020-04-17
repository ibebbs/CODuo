using System;
using System.Collections.Generic;
using System.Linq;
using Uno.UI;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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

	public enum DualPaneViewMode
	{
		SinglePane = 0,
		Wide = 1,
		Tall = 2,
	};

	public enum DualPaneViewPriority
	{
		Pane1 = 0,
		Pane2 = 1,
	};

	public enum DualPaneViewWideModeConfiguration
	{
		SinglePane = 0,
		LeftRight = 1,
		RightLeft = 2,
	};

	public enum DualPaneViewTallModeConfiguration
	{
		SinglePane = 0,
		TopBottom = 1,
		BottomTop = 2,
	};

	internal enum DeviceOrientation
	{
		Rotation0,
		Rotation90,
		Rotation180,
		Rotation270
	}

	internal class DeviceInfo
	{
		public DeviceInfo(IEnumerable<Rect> screens, DeviceOrientation orientation)
		{
			Screens = (screens ?? Enumerable.Empty<Rect>()).ToList();
			Orientation = orientation;
		}

		public IReadOnlyList<Rect> Screens { get; }

		public DeviceOrientation Orientation { get; }
	}

#if __ANDROID__
	internal static class Helpers
	{

		private static double PixelsToDip(double px) => px / (ContextHelper.Current as Android.App.Activity)?.Resources?.DisplayMetrics?.Density ?? 1;

		private static double DipScaling => 1 / (ContextHelper.Current as Android.App.Activity)?.Resources?.DisplayMetrics?.Density ?? 1;

		public static Rect PhysicalToLogicalPixels(this Rect rect)
		{
			var scaling = DipScaling;

			return new Rect(rect.Left * scaling, rect.Top * scaling, rect.Width * scaling, rect.Height * scaling);
		}
		public static Rect LogicalToPhysicalPixels(this Rect rect)
		{
			var scaling = 1 / DipScaling;

			return new Rect(rect.Left * scaling, rect.Top * scaling, rect.Width * scaling, rect.Height * scaling);
		}

		public static Rect? IntersectWith(this Rect rect1, Rect rect2)
		{
			if (rect1.Equals(rect2))
			{
				return rect1;
			}

			var left = Math.Max(rect1.Left, rect2.Left);
			var right = Math.Min(rect1.Right, rect2.Right);
			var top = Math.Max(rect1.Top, rect2.Top);
			var bottom = Math.Min(rect1.Bottom, rect2.Bottom);

			if (right >= left && bottom >= top)
			{
				return new Rect(left, top, right - left, bottom - top);
			}
			else
			{
				return null;
			}
		}
	}
#endif

	internal class DeviceHelper
	{
		public static readonly DeviceHelper Instance = new DeviceHelper();

#if __ANDROID__
		
		private static readonly (Android.Views.SurfaceOrientation orientation, List<Rect> result) EmptyMode =
			((Android.Views.SurfaceOrientation)(-1), null);
		
		private static readonly IReadOnlyList<Rect> EmptyList = new List<Rect>(0);

		private readonly Lazy<bool> _isDualScreenDevice = new Lazy<bool>(GetIsDualScreenDevice);

		private (Android.Views.SurfaceOrientation orientation, List<Rect> result) _previousMode = EmptyMode;

		private static bool GetIsDualScreenDevice()
		{
			if (ContextHelper.Current is Android.App.Activity currentActivity)
			{
				return Microsoft.Device.Display.ScreenHelper.IsDualScreenDevice(currentActivity);
			}
			else
			{
				return false;
			}
		}

		private Android.Graphics.Rect GetHinge(Android.App.Activity activity, Android.Views.SurfaceOrientation rotation)
		{
			// Hinge's coordinates of its 4 edges in different mode
			// Double Landscape Rect(0, 1350 - 1800, 1434)
			// Double Portrait  Rect(1350, 0 - 1434, 1800)
			var displayMask = Microsoft.Device.Display.DisplayMask.FromResourcesRectApproximation(activity);
			var boundings = displayMask.GetBoundingRectsForRotation(rotation);

			if (boundings.Count <= 0)
				return new Rect();

			return boundings[0];
		}

		private Android.Graphics.Rect GetWindowRect(Android.App.Activity activity)
		{
			var windowRect = new Android.Graphics.Rect();
			activity.WindowManager.DefaultDisplay.GetRectSize(windowRect);
			return windowRect;
		}

		public bool IsDualMode(Android.App.Activity activity, Android.Views.SurfaceOrientation orientation)
		{
			var hinge = GetHinge(activity, orientation);
			var windowRect = GetWindowRect(activity);

			// Make sure hinge isn't null and window rect
			// Also make sure hinge has width OR height (not just Rect.Zero)
			// Finally make sure the window rect has width AND height
			if (hinge != null && windowRect != null
				&& (hinge.Width() > 0 || hinge.Height() > 0)
				&& windowRect.Width() > 0 && windowRect.Height() > 0)
			{
				// If the hinge intersects the window, dual mode
				return hinge.Intersect(windowRect);
			}

			return false;
		}

		private IReadOnlyList<Rect> GetDualScreenRects(Android.App.Activity currentActivity)
		{
			var orientation = Microsoft.Device.Display.ScreenHelper.GetRotation(currentActivity);

			var wuxWindowBounds = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds;
			var wuOrientation = Windows.Graphics.Display.DisplayInformation.GetForCurrentView().CurrentOrientation;

			if (IsDualMode(currentActivity, orientation))
			{
				//if (_previousMode.orientation == orientation && _previousMode.result != null)
				//{
				//	return _previousMode.result;
				//}

				_previousMode.orientation = orientation;
				_previousMode.result = null;

				var occludedRect = Microsoft.Device.Display.DisplayMask
					.FromResourcesRectApproximation(currentActivity)
					.GetBoundingRectsForRotation(orientation)
					.Select(r => (Rect?)r)
					.FirstOrDefault();

				if (occludedRect.HasValue)
				{
					var bounds = wuxWindowBounds.LogicalToPhysicalPixels();
					var intersection = bounds.IntersectWith(occludedRect.Value);

					//if (wuOrientation == Windows.Graphics.Display.DisplayOrientations.Portrait || wuOrientation == Windows.Graphics.Display.DisplayOrientations.PortraitFlipped)
					//{
					//	// Compensate for the status bar size (the occluded area is rooted on the screen size, whereas
					//	// wuxWindowBoundsis rooted on the visible size of the window, unless the status bar is translucent.
					//	if ((int)bounds.X == 0 && (int)bounds.Y == 0)
					//	{
					//		var statusBarRect = Windows.UI.ViewManagement.StatusBar.GetForCurrentView().OccludedRect.LogicalToPhysicalPixels();
					//		occludedRect = new Rect(occludedRect.Value.X, occludedRect.Value.Y - statusBarRect.Height, occludedRect.Value.Width, occludedRect.Value.Height);
					//	}
					//}

					if (intersection != null) // Occluded region overlaps the app
					{
						if ((int)occludedRect.Value.X == (int)bounds.X)
						{
							// Vertical stacking
							// +---------+
							// |         |
							// |         |
							// +---------+
							// +---------+
							// |         |
							// |         |
							// +---------+

							var spanningRects = new List<Rect> {
										// top region
										new Rect(bounds.X, bounds.Y, bounds.Width, occludedRect.Value.Top).PhysicalToLogicalPixels(),
										// bottom region
										new Rect(bounds.X,
											occludedRect.Value.Bottom,
											bounds.Width,
											bounds.Height - occludedRect.Value.Bottom).PhysicalToLogicalPixels(),
									};

							_previousMode.result = spanningRects;
						}
						else if ((int)occludedRect.Value.Y == (int)bounds.Y)
						{
							// Horizontal side-by-side
							// +-----+ +-----+
							// |     | |     |
							// |     | |     |
							// |     | |     |
							// |     | |     |
							// |     | |     |
							// +-----+ +-----+

							var spanningRects = new List<Rect> {
										// left region
										new Rect(bounds.X, bounds.Y, occludedRect.Value.X, bounds.Height).PhysicalToLogicalPixels(),
										// right region
										new Rect(occludedRect.Value.Right,
											bounds.Y,
											bounds.Width - occludedRect.Value.Right,
											bounds.Height).PhysicalToLogicalPixels(),
									};

							_previousMode.result = spanningRects;
						}
						else
						{
							// DualMode: Unknown screen layout
							_previousMode.result = new[] { wuxWindowBounds }.ToList();
						}
					}
					else
					{
						// DualMode: Without intersection, single screen"
						_previousMode.result = new[] { wuxWindowBounds }.ToList();
					}
				}
				else
				{
					// DualMode: Without occlusion
					_previousMode.result = new[] { wuxWindowBounds }.ToList();
				}
			}
			else
			{
				_previousMode.result = new[] { wuxWindowBounds }.ToList();
			}

			return _previousMode.result ?? EmptyList;
		}

		private IReadOnlyList<Rect> GetSpanningRects()
		{
			if (ContextHelper.Current is Android.App.Activity currentActivity)
			{
				if (_isDualScreenDevice.Value)
				{
					return GetDualScreenRects(currentActivity);
				}
				else
				{
					return new[] { Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds }.ToList();
				}
			}
			else
			{
				return EmptyList;
			}
		}
#else
		private IReadOnlyList<Rect> GetSpanningRects()
		{
			return new List<Rect>(new[] { Window.Current.Bounds });
		}
#endif

		public DeviceInfo GetDeviceInfo()
		{
			return new DeviceInfo(GetSpanningRects(), DeviceOrientation.Rotation0);
		}
	}


	[TemplatePart(Name = c_columnLeftName, Type = typeof(ColumnDefinition))]
	[TemplatePart(Name = c_columnMiddleName, Type = typeof(ColumnDefinition))]
	[TemplatePart(Name = c_columnRightName, Type = typeof(ColumnDefinition))]
	[TemplatePart(Name = c_rowTopName, Type = typeof(ColumnDefinition))]
	[TemplatePart(Name = c_rowMiddleName, Type = typeof(RowDefinition))]
	[TemplatePart(Name = c_rowBottomName, Type = typeof(RowDefinition))]
	[TemplateVisualState(GroupName = c_viewModelStates, Name = c_viewModeOneOnlyState)]
	[TemplateVisualState(GroupName = c_viewModelStates, Name = c_viewModeTwoOnlyState)]
	[TemplateVisualState(GroupName = c_viewModelStates, Name = c_viewModeLeftRightState)]
	[TemplateVisualState(GroupName = c_viewModelStates, Name = c_viewModeRightLeftState)]
	[TemplateVisualState(GroupName = c_viewModelStates, Name = c_viewModeTopBottomState)]
	[TemplateVisualState(GroupName = c_viewModelStates, Name = c_viewModeBottomTopState)]
	public sealed partial class DualPaneView : Control
	{
		private const string c_columnLeftName = "PART_ColumnLeft";
		private const string c_columnMiddleName = "PART_ColumnMiddle";
		private const string c_columnRightName = "PART_ColumnRight";
		private const string c_rowTopName = "PART_RowTop";
		private const string c_rowMiddleName = "PART_RowMiddle";
		private const string c_rowBottomName = "PART_RowBottom";

		private const string c_viewModelStates = "ModeStates";
		private const string c_viewModeOneOnlyState = "ViewMode_OneOnly";
		private const string c_viewModeTwoOnlyState = "ViewMode_TwoOnly";
		private const string c_viewModeLeftRightState = "ViewMode_LeftRight";
		private const string c_viewModeRightLeftState = "ViewMode_RightLeft";
		private const string c_viewModeTopBottomState = "ViewMode_TopBottom";
		private const string c_viewModeBottomTopState = "ViewMode_BottomTop";

		private static readonly GridLength OneStar = new GridLength(1, GridUnitType.Star);
		private static readonly GridLength ZeroStar = new GridLength(0, GridUnitType.Star);
		private static readonly GridLength ZeroPixel = new GridLength(0, GridUnitType.Pixel);

		public static readonly DependencyProperty Pane1Property = DependencyProperty.Register(nameof(Pane1), typeof(UIElement), typeof(DualPaneView), new PropertyMetadata(null, DataPropertyChanged));
		public static readonly DependencyProperty Pane2Property = DependencyProperty.Register(nameof(Pane2), typeof(UIElement), typeof(DualPaneView), new PropertyMetadata(null, DataPropertyChanged));
		public static readonly DependencyProperty Pane1LengthProperty = DependencyProperty.Register(nameof(Pane1Length), typeof(GridLength), typeof(DualPaneView), new PropertyMetadata(GridLength.Auto, DataPropertyChanged));
		public static readonly DependencyProperty Pane2LengthProperty = DependencyProperty.Register(nameof(Pane2Length), typeof(GridLength), typeof(DualPaneView), new PropertyMetadata(new GridLength(1, GridUnitType.Star), DataPropertyChanged));
		public static readonly DependencyProperty PanePriorityProperty = DependencyProperty.Register(nameof(PanePriority), typeof(DualPaneViewPriority), typeof(DualPaneView), new PropertyMetadata(DualPaneViewPriority.Pane1, DataPropertyChanged));
		public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(DualPaneViewMode), typeof(DualPaneView), new PropertyMetadata(DualPaneViewMode.SinglePane, DataPropertyChanged));
		public static readonly DependencyProperty WideModeConfigurationProperty = DependencyProperty.Register(nameof(WideModeConfiguration), typeof(DualPaneViewWideModeConfiguration), typeof(DualPaneView), new PropertyMetadata(DualPaneViewWideModeConfiguration.LeftRight, DataPropertyChanged));
		public static readonly DependencyProperty TallModeConfigurationProperty = DependencyProperty.Register(nameof(TallModeConfiguration), typeof(DualPaneViewTallModeConfiguration), typeof(DualPaneView), new PropertyMetadata(DualPaneViewTallModeConfiguration.TopBottom, DataPropertyChanged));
		public static readonly DependencyProperty MinWideModeWidthProperty = DependencyProperty.Register(nameof(MinWideModeWidth), typeof(double), typeof(DualPaneView), new PropertyMetadata(641.0, DataPropertyChanged));
		public static readonly DependencyProperty MinTallModeHeightProperty = DependencyProperty.Register(nameof(MinTallModeHeight), typeof(double), typeof(DualPaneView), new PropertyMetadata(641.0, DataPropertyChanged));

		private static void DataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is DualPaneView dpv)
			{
				dpv.OnPropertyChanged(e);
			}
		}

		private ViewMode m_currentMode = ViewMode.None;

		private bool m_loaded = false;

		private ColumnDefinition m_columnLeft;
		private ColumnDefinition m_columnMiddle;
		private ColumnDefinition m_columnRight;
		private RowDefinition m_rowTop;
		private RowDefinition m_rowMiddle;
		private RowDefinition m_rowBottom;

		public event TypedEventHandler<DualPaneView, object> ModeChanged;

		public DualPaneView()
		{
			DefaultStyleKey = typeof(DualPaneView);

			SizeChanged += OnSizeChanged;
			Windows.UI.Xaml.Window.Current.SizeChanged += OnWindowSizeChanged;
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

		private void OnWindowSizeChanged(object sender, WindowSizeChangedEventArgs args)
		{
			UpdateMode();
		}

		private void OnSizeChanged(object sender, SizeChangedEventArgs args)
		{
			UpdateMode();
		}

		private static DualPaneViewMode DetermineDualPaneViewMode(DeviceInfo deviceInfo, double wideModeMinWidth, double tallModeMinHeight)
		{
			return deviceInfo.Screens.Count switch
			{
				2 when deviceInfo.Screens[0].X < deviceInfo.Screens[1].X && deviceInfo.Screens[0].Y == deviceInfo.Screens[1].Y => DualPaneViewMode.Wide,
				2 when deviceInfo.Screens[0].X == deviceInfo.Screens[1].X && deviceInfo.Screens[0].Y < deviceInfo.Screens[1].Y => DualPaneViewMode.Tall,
				1 when deviceInfo.Screens[0].Width >= wideModeMinWidth => DualPaneViewMode.Wide,
				1 when deviceInfo.Screens[0].Height >= tallModeMinHeight => DualPaneViewMode.Tall,
				_ => DualPaneViewMode.SinglePane
			};
		}

		private static ViewMode DetermineViewMode(DualPaneViewMode viewMode, DualPaneViewWideModeConfiguration wideModelConfiguration, DualPaneViewTallModeConfiguration tallModelConfiguration, DualPaneViewPriority panePriority)
		{
			return viewMode switch
			{
				DualPaneViewMode.Wide when wideModelConfiguration == DualPaneViewWideModeConfiguration.LeftRight => ViewMode.LeftRight,
				DualPaneViewMode.Wide when wideModelConfiguration == DualPaneViewWideModeConfiguration.RightLeft => ViewMode.RightLeft,
				DualPaneViewMode.Wide when panePriority == DualPaneViewPriority.Pane2 => ViewMode.Pane2Only,
				DualPaneViewMode.Wide => ViewMode.Pane1Only,
				DualPaneViewMode.Tall when tallModelConfiguration == DualPaneViewTallModeConfiguration.TopBottom => ViewMode.TopBottom,
				DualPaneViewMode.Tall when tallModelConfiguration == DualPaneViewTallModeConfiguration.BottomTop => ViewMode.BottomTop,
				DualPaneViewMode.Tall when panePriority == DualPaneViewPriority.Pane2 => ViewMode.Pane2Only,
				DualPaneViewMode.Tall => ViewMode.Pane1Only,
				DualPaneViewMode.SinglePane when panePriority == DualPaneViewPriority.Pane2 => ViewMode.Pane2Only,
				_ => ViewMode.Pane1Only
			};
		}

		private static string DetermineVisualState(ViewMode viewMode)
		{
			return viewMode switch
			{
				ViewMode.Pane2Only => c_viewModeTwoOnlyState,
				ViewMode.LeftRight => c_viewModeLeftRightState,
				ViewMode.RightLeft => c_viewModeRightLeftState,
				ViewMode.TopBottom => c_viewModeTopBottomState,
				ViewMode.BottomTop => c_viewModeBottomTopState,
				_ => c_viewModeOneOnlyState
			};
		}

		private void UpdateMode()
		{
			// Don't bother running this logic until after we hit OnApplyTemplate.
			if (!m_loaded) return;

			double controlWidth = ActualWidth;
			double controlHeight = ActualHeight;

			var deviceInfo = DeviceHelper.Instance.GetDeviceInfo();
			var dualPaneViewMode = DetermineDualPaneViewMode(deviceInfo, MinWideModeWidth, MinTallModeHeight);
			var viewMode = DetermineViewMode(dualPaneViewMode, WideModeConfiguration, TallModeConfiguration, PanePriority);

			UpdateRowsColumns(viewMode, deviceInfo);

			if (viewMode != m_currentMode)
			{
				m_currentMode = viewMode;

				string visualState = DetermineVisualState(viewMode);
				VisualStateManager.GoToState(this, visualState, true);

				if (dualPaneViewMode != Mode)
				{
					SetValue(ModeProperty, dualPaneViewMode);
					ModeChanged?.Invoke(this, this);
				}
			}
		}

		private void UpdateRowsColumns(ViewMode viewMode, DeviceInfo deviceInfo)
		{
			if (m_columnLeft != null && m_columnMiddle != null && m_columnRight != null && m_rowTop != null && m_rowMiddle != null && m_rowBottom != null)
			{
				switch (viewMode)
				{
					case ViewMode.LeftRight when deviceInfo.Screens.Count > 1:
					case ViewMode.RightLeft when deviceInfo.Screens.Count > 1:
						m_columnLeft.Width = new GridLength(deviceInfo.Screens[0].Width, GridUnitType.Pixel);
						m_columnRight.Width = new GridLength(deviceInfo.Screens[1].Width, GridUnitType.Pixel);
						m_columnMiddle.Width = new GridLength(deviceInfo.Screens[1].Left - deviceInfo.Screens[0].Right, GridUnitType.Pixel);
						m_rowTop.Height = OneStar;
						m_rowBottom.Height = ZeroStar;
						m_rowMiddle.Height = ZeroPixel;
						break;
					case ViewMode.LeftRight when deviceInfo.Screens.Count == 1:
						m_columnLeft.Width = Pane1Length;
						m_columnRight.Width = Pane2Length;
						m_columnMiddle.Width = ZeroPixel;
						m_rowTop.Height = OneStar;
						m_rowBottom.Height = ZeroStar;
						m_rowMiddle.Height = ZeroPixel;
						break;
					case ViewMode.RightLeft when deviceInfo.Screens.Count == 1:
						m_columnLeft.Width = Pane2Length;
						m_columnRight.Width = Pane1Length;
						m_columnMiddle.Width = ZeroPixel;
						m_rowTop.Height = OneStar;
						m_rowBottom.Height = ZeroStar;
						m_rowMiddle.Height = ZeroPixel;
						break;
					case ViewMode.TopBottom when deviceInfo.Screens.Count > 1:
					case ViewMode.BottomTop when deviceInfo.Screens.Count > 1:
						m_columnLeft.Width = OneStar;
						m_columnRight.Width = ZeroStar;
						m_columnMiddle.Width = ZeroPixel;
						m_rowTop.Height = new GridLength(deviceInfo.Screens[0].Height, GridUnitType.Pixel);
						m_rowBottom.Height = new GridLength(deviceInfo.Screens[1].Height, GridUnitType.Pixel);
						m_rowMiddle.Height = new GridLength(deviceInfo.Screens[1].Top - deviceInfo.Screens[0].Bottom, GridUnitType.Pixel);
						break;
					case ViewMode.TopBottom when deviceInfo.Screens.Count == 1:
						m_columnLeft.Width = OneStar;
						m_columnRight.Width = ZeroStar;
						m_columnMiddle.Width = ZeroPixel;
						m_rowTop.Height = Pane1Length;
						m_rowBottom.Height = Pane2Length;
						m_rowMiddle.Height = ZeroPixel;
						break;
					case ViewMode.BottomTop when deviceInfo.Screens.Count == 1:
						m_columnLeft.Width = OneStar;
						m_columnRight.Width = ZeroStar;
						m_columnMiddle.Width = ZeroPixel;
						m_rowTop.Height = Pane2Length;
						m_rowBottom.Height = Pane1Length;
						m_rowMiddle.Height = ZeroPixel;
						break;
					case ViewMode.Pane1Only:
					case ViewMode.Pane2Only:
						m_columnLeft.Width = OneStar;
						m_columnRight.Width = ZeroStar;
						m_columnMiddle.Width = ZeroPixel;
						m_rowTop.Height = OneStar;
						m_rowMiddle.Height = ZeroStar;
						m_rowBottom.Height = ZeroPixel;
						break;
				}
			}
		}

		private void OnPropertyChanged(DependencyPropertyChangedEventArgs args)
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

		public DualPaneViewPriority PanePriority
		{
			get { return (DualPaneViewPriority)GetValue(PanePriorityProperty); }
			set { SetValue(PanePriorityProperty, value); }
		}

		public DualPaneViewMode Mode
		{
			get { return (DualPaneViewMode)GetValue(ModeProperty); }
			set { SetValue(ModeProperty, value); }
		}

		public DualPaneViewWideModeConfiguration WideModeConfiguration
		{
			get { return (DualPaneViewWideModeConfiguration)GetValue(WideModeConfigurationProperty); }
			set { SetValue(WideModeConfigurationProperty, value); }
		}

		public DualPaneViewTallModeConfiguration TallModeConfiguration
		{
			get { return (DualPaneViewTallModeConfiguration)GetValue(TallModeConfigurationProperty); }
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
