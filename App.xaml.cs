using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using JClientBot.Commons;

namespace JClientBot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppLog.Write("JClientBot started.");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            AppLog.Write("JClientBot exited. Code={0}", e.ApplicationExitCode);
            base.OnExit(e);
        }
    }
}
