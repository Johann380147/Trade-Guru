using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CefSharp;
using CefSharp.Wpf;
namespace TradeGuru
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            Cef.EnableHighDPISupport();
            var settings = new CefSettings() { CachePath = System.IO.Directory.GetCurrentDirectory() };
            //Perform dependency check to make sure all relevant resources are in our output directory.
            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

        }

        private void App_Exit(object sender, ExitEventArgs e)
        {

        }
    }
}
