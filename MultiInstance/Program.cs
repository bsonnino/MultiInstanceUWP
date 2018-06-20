using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.StartScreen;

namespace MultiInstance
{
    public static class Program
    {
        // This project includes DISABLE_XAML_GENERATED_MAIN in the build properties,
        // which prevents the build system from generating the default Main method:
        //static void Main(string[] args)
        //{
        //    global::Windows.UI.Xaml.Application.Start((p) => new App());
        //}

        // This example code shows how you could implement the required Main method to
        // support multi-instance redirection. The minimum requirement is to call
        // Application.Start with a new App object. Beyond that, you may delete the
        // rest of the example code and replace it with your custom code if you wish.

        static void Main(string[] args)
        {
            Debugger.Break();
            // First, we'll get our activation event args, which are typically richer
            // than the incoming command-line args. We can use these in our app-defined
            // logic for generating the key for this instance.
            IActivatedEventArgs activatedArgs = AppInstance.GetActivatedEventArgs();
            var launchArgs = activatedArgs as LaunchActivatedEventArgs;
            var arguments = launchArgs?.Arguments;
            var commandArgs = activatedArgs as CommandLineActivatedEventArgs;
            arguments = arguments ?? commandArgs?.Operation.Arguments;
            if (arguments != null)
            {
                args = SplitArgs(arguments);
                if (arguments == commandArgs?.Operation.Arguments && args.Length >= 2)
                    args = args.Skip(1).ToArray();
            }
            // In some scenarios, the platform might indicate a recommended instance.
            // If so, we can redirect this activation to that instance instead, if we wish.
            if (AppInstance.RecommendedInstance != null)
            {
                AppInstance.RecommendedInstance.RedirectActivationTo();
            }
            else
            {
                // Define a key for this instance, based on some app-specific logic.
                // If the key is always unique, then the app will never redirect.
                // If the key is always non-unique, then the app will always redirect
                // to the first instance. In practice, the app should produce a key
                // that is sometimes unique and sometimes not, depending on its own needs.
                string key = "MainInstance";
                if (args.Length > 0)
                    key = args[0];
                //var instanceNo = GetInstanceNo();
                //string key = instanceNo.ToString();
                var instance = AppInstance.FindOrRegisterInstanceForKey(key);
                if (instance.IsCurrentInstance)
                {
                    // If we successfully registered this instance, we can now just
                    // go ahead and do normal XAML initialization.
                    global::Windows.UI.Xaml.Application.Start(p => new App());
                }
                else
                {
                    // Some other instance has registered for this key, so we'll 
                    // redirect this activation to that instance instead.
                    instance.RedirectActivationTo();
                }
            }
        }

        private static string[] SplitArgs(string args)
        {
            return Regex.Matches(args, @"(?<=\"")[^\""]*(?=\"")|[^\"" ]+")
                .Select(m => m.Value).ToArray();
        }

        private static int GetInstanceNo()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            var instanceNo = 0;
            object data = localSettings.Values["instanceNo"];
            if (data != null)
            {
                instanceNo = ((int)localSettings.Values["instanceNo"] + 1) % 3;
            }

            localSettings.Values["instanceNo"] = instanceNo;
            return instanceNo;
        }
    }
}
