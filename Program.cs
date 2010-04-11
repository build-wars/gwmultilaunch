//Guild Wars MultiLaunch - Safe and efficient way to launch multiple GWs.
//The Guild Wars executable is never modified, keeping you inline with the tos.
//
//Copyright (C) 2010  IMKey@GuildWarsGuru

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
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GWMultiLaunch
{
    static class Program
    {
        public const string MUTEX_MATCH_STRING      = "AN-Mute";
        public const string DEFAULT_ARGUMENT        = "-windowed";
        public const string ERROR_CAPTION           = "GWMultiLaunch Error";

        public const string SHORTCUT_PREFIX         = "Guild Wars ML-";
        public const string AUTO_LAUNCH_SHORTCUT    = "Guild Wars ML-X";
        public const string AUTO_LAUNCH_SWITCH      = "-auto";
        
        public const string GW_PROCESS_NAME         = "Gw";
        public const string GW_FILENAME             = "Gw.exe";
        public const string GW_DAT                  = "Gw.dat";
        public const string GW_REG_LOCATION         = "SOFTWARE\\ArenaNet\\Guild Wars";
        public const string GW_REG_LOCATION_AUX     = "SOFTWARE\\Wow6432Node\\ArenaNet\\Guild Wars";
        public const string GW_TEMPLATES            = "\\Templates";
        public const string TM_FILENAME             = "texmod.exe";
        
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
                ShowGUI();
            }
        }

        /// <summary>
        /// Altered functionality depending on arguments.
        /// </summary>
        /// <param name="programArgs">GWMultiLaunch arguments.</param>
        static void HandleArguments(string[] programArgs)
        {
            string firstArgument = programArgs[0];

            bool autoLaunch = firstArgument.Equals(AUTO_LAUNCH_SWITCH, StringComparison.OrdinalIgnoreCase);

            if (autoLaunch)
            {
                //launch by trying differents copies from the ini file
                LaunchAvailableCopy();
            }
            else
            {
                string gwArgs = GetGWLaunchArguments(programArgs);
                LaunchByArguments(firstArgument, gwArgs);
            }
        }

        /// <summary>
        /// Show the GUI
        /// </summary>
        static void ShowGUI()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        /// <summary>
        /// Retrieves Guild Wars launch arguments.
        /// </summary>
        /// <param name="programArgs">GWMultiLaunch arguments.</param>
        /// <returns>GW launch arguments. Empty string array if none found.</returns>
        static string GetGWLaunchArguments(string[] programArgs)
        {
            string gwLaunchArgs = string.Empty;

            if (programArgs.Length > 1)
            {
                string[] argArray = new string[programArgs.Length-1];

                //copy everything but the first argument
                Array.Copy(programArgs, 1, argArray, 0, argArray.Length);

                gwLaunchArgs = ConvertArgumentArray(argArray);
            }

            return gwLaunchArgs;
        }

        static string ConvertArgumentArray(string[] argumentsArray)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string s in argumentsArray)
            {
                if (s.Contains(" "))
                {
                    sb.Append('"');
                    sb.Append(s);
                    sb.Append('"');
                }
                else
                {
                    sb.Append(s);
                }

                sb.Append(' ');
            }

            //we don't want last space
            return sb.ToString(0, Math.Max(0, sb.Length-1));
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
                    ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //attempt to launch
                MainForm.LaunchGame(pathToLaunch, launchArgs, false);
            }
        }

        /// <summary>
        /// Iterates through the Guild Wars copies list and attempts to launch new copy.
        /// </summary>
        static void LaunchAvailableCopy()
        {
            bool launchAttempted = false;

            foreach (SettingsManager.Profile p in Program.settings.Profiles)
            {
                String currentPath = p.Path;

                if (MainForm.IsCopyRunning(currentPath) == false)
                {
                    LaunchByArguments(currentPath, p.Arguments);
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