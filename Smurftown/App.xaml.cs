﻿using System.IO;
using System.Windows;
using Serilog;

namespace Smurftown
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (!Directory.Exists(Directories.UserPath)) Directory.CreateDirectory(Directories.UserPath);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(Directories.UserPath, "smurftown.log"))
                .CreateLogger();

            Log.Information("starting smurftown");
            base.OnStartup(e);
        }
    }
}