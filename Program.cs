//Guild Wars MultiLaunch - Safe and efficient way to launch multiple GWs.
//The Guild Wars executable is never modified, keeping you inline with the tos.
//
//Copyright (C) 2009  IMKey@GuildWarsGuru

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GWMultiLaunch
{
    static class Program
    {
        public static SettingsManager settings = new SettingsManager();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                HandleArguments(args);
            }
            else
            {
                LaunchGUI();
            }
        }

        /// <summary>
        /// Altered functionality depending on arguments.
        /// </summary>
        /// <param name="programArgs">GWMultiLaunch arguments.</param>
        static void HandleArguments(string[] programArgs)
        {
            string firstArgument = programArgs[0];

            bool autoLaunch = firstArgument.Equals(Form1.AUTO_LAUNCH_SWITCH, StringComparison.OrdinalIgnoreCase);

            if (autoLaunch)
            {
                //launch by trying gw paths in the ini file
                LaunchByList();
            }
            else
            {
                string gwArgs = GetGWLaunchArguments(programArgs);
                LaunchByArguments(firstArgument, gwArgs);
            }
        }

        /// <summary>
        /// Launch the GUI
        /// </summary>
        static void LaunchGUI()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        /// <summary>
        /// Retrieves Guild Wars launch arguments.
        /// </summary>
        /// <param name="programArgs">GWMultiLaunch arguments.</param>
        /// <returns>GW launch arguments. Empty string if none found.</returns>
        static string GetGWLaunchArguments(string[] programArgs)
        {
            string gwLaunchArgs = string.Empty;

            if (programArgs.Length >= 2)
            {
                gwLaunchArgs = programArgs[1];
            }

            return gwLaunchArgs;
        }

        /// <summary>
        /// Sets registry and attempts to launch Guild Wars with specified launch arguments.
        /// </summary>
        /// <param name="pathToLaunch">Path to Guild Wars executable.</param>
        /// <param name="launchArgs">Guild Wars launch arguments.</param>
        static void LaunchByArguments(string pathToLaunch, string launchArgs)
        {
            //check if path exists
            if (File.Exists(pathToLaunch) == false)
            {
                MessageBox.Show("The path: " + pathToLaunch + " does not exist!",
                    Form1.ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //set path in registry
                Form1.SetRegistry(pathToLaunch);

                //attempt to launch
                Form1.LaunchGame(pathToLaunch, launchArgs, false);
            }
        }

        /// <summary>
        /// Iterates through the Guild Wars copies list and attempts to launch new copy.
        /// </summary>
        static void LaunchByList()
        {
            bool launchAttempted = false;

            foreach (KeyValuePair<string, string> i in Program.settings.Profiles)
            {
                String currentPath = i.Key;

                if (Form1.IsCopyRunning(currentPath) == false)
                {
                    LaunchByArguments(currentPath, i.Value);
                    launchAttempted = true;
                    break;
                }
            }

            if (launchAttempted == false)
            {
                MessageBox.Show("No more copies left to launch. Add more copies to GWMultilaunch.", 
                    "Unable to launch more copies.", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}