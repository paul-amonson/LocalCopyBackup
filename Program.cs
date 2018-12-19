// Copyright (c) 2017-2018, Paul Amonson
// SPDX-License-Identifier: MIT

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Backup;
using BackupPlan;

namespace LocalCopyBackup
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] rawArgs)
        {
            var args = new List<string>(rawArgs);
            if (args.Contains("--backup-now"))
            { // Run immediate backup...
                RunBackup();
            }
            else
            { // Run GUI...
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }

        private static void RunBackup()
        {
            var plans = LoadPlans();
            foreach (var plan in plans)
                plan.RunBackup();
        }

        private static string GetSettingsFolder()
        {
            var home = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory).Replace("Desktop", "");
            return home + ".local_backup";
        }

        private static BackupPlanList LoadPlans()
        {
            var settingsFolder = GetSettingsFolder();
            var plansFile = Path.Combine(settingsFolder, "plans_file");
            return BackupPlanList.Load(plansFile, Log);
        }

        private static void Log(string text)
        {
            var logFile = Path.Combine(GetSettingsFolder(), "UnattendedBackup.log");
            using (var file = new StreamWriter(logFile, true))
                file.WriteLine(text);
        }
    }
}
