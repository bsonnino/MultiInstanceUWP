using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// This code is generated from the Multi-Instance with Redirection project template.
// In addition to the SupportsMultipleInstances declaration in the package.appxmanifest,
// it also includes a Program.cs which defines a simple Main method - this is required
// if you want to use multi-instance activation redirection.

namespace MultiInstance
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            var arguments = (args as CommandLineActivatedEventArgs)?.Operation.Arguments;
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(MainPage), 
                    $"Activated Kind {args.Kind}\nArguments: {arguments}");
            }
            Window.Current.Activate();
        }

        private async void CreateSecondaryTile()
        {
            var secondaryTile = new SecondaryTile("tileID",
                "MultiInstance",
                "args",
                new Uri("ms-appx:///Assets/Square150x150Logo.png"),
                TileSize.Default);
            if (!SecondaryTile.Exists("tileId"))
                await secondaryTile.RequestCreateAsync();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            CreateSecondaryTile();
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application.
                }
                Window.Current.Content = rootFrame;
            }
            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(MainPage), $"Lauched kind {e.Kind}\nArguments: {e.Arguments}");
                }
                Window.Current.Activate();
            }
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity.
            deferral.Complete();
        }
    }
}
