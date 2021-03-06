﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;

namespace CODuo
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Windows.UI.Xaml.Application
    {
        private IDisposable _app;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            ConfigureFilters(global::Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory);

            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.Resuming += OnResuming;

            Platform.Services.Service.PerformRegistration();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
			if (System.Diagnostics.Debugger.IsAttached)
			{
				// this.DebugSettings.EnableFrameRateCounter = true;
			}
#endif

            if (_app == null)
            {
                _app = Platform.Services.Service.Provider.GetService<Application.State.IMachine>().Start();
            }

            Windows.UI.Xaml.Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
#if NETFX_CORE
            var deferral = e.SuspendingOperation.GetDeferral();
#endif
            var eventBus = Platform.Services.Service.Provider.GetService<Event.IBus>();

            var task = eventBus.GetEvent<Event.Application.Suspended>().Take(1).ToTask();

            eventBus.Publish(new Event.Application.Suspending());
#if NETFX_CORE
            await task;

            deferral.Complete();
#else
            // No deferral in Uno so wait for event Application.Suspended event synchronously
            task.GetAwaiter().GetResult();
#endif
        }

        private void OnResuming(object sender, object e)
        {
            Platform.Services.Service.Provider
                .GetService<Event.IBus>()
                .Publish(new Event.Application.Resuming());
        }

        /// <summary>
        /// Configures global logging
        /// </summary>
        /// <param name="factory"></param>
        static void ConfigureFilters(ILoggerFactory factory)
        {
            factory
                .WithFilter(new FilterLoggerSettings
                    {
                        { "Uno", LogLevel.Warning },
                        { "Windows", LogLevel.Warning },

						// Debug JS interop
						// { "Uno.Foundation.WebAssemblyRuntime", LogLevel.Debug },

						// Generic Xaml events
						// { "Windows.UI.Xaml", LogLevel.Debug },
						// { "Windows.UI.Xaml.VisualStateGroup", LogLevel.Debug },
						// { "Windows.UI.Xaml.StateTriggerBase", LogLevel.Debug },
						// { "Windows.UI.Xaml.UIElement", LogLevel.Debug },

						// Layouter specific messages
						// { "Windows.UI.Xaml.Controls", LogLevel.Debug },
						// { "Windows.UI.Xaml.Controls.Layouter", LogLevel.Debug },
						// { "Windows.UI.Xaml.Controls.Panel", LogLevel.Debug },
						// { "Windows.Storage", LogLevel.Debug },

						// Binding related messages
						// { "Windows.UI.Xaml.Data", LogLevel.Debug },

						// DependencyObject memory references tracking
						// { "ReferenceHolder", LogLevel.Debug },
					}
                );
        }
    }
}
