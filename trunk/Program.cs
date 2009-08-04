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
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GWMultiLaunch
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            FileManager fileCloset = new FileManager();

            if (args.Length >= 1)
            {
                // Shortcut launching modes

                string firstArgument = args[0];

                //auto mode?
                bool autoMode = firstArgument.Equals(Form1.GW_AUTO_SWITCH, StringComparison.OrdinalIgnoreCase);

                if (autoMode)
                {
                    //launch by trying paths in the ini file
                    LaunchCycler(fileCloset);
                }
                else
                {
                    string pathArgs = string.Empty;

                    if (args.Length >= 2)
                    {
                        pathArgs = args[1];
                    }

                    LaunchByArguments(firstArgument, pathArgs);
                }
                
                Environment.Exit(0);
            }
            else
            {
                // Launch GUI
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1(fileCloset));
            }
        }

        static void LaunchByArguments(string pathToLaunch, string pathArgs)
        {
            //validate path
            if (!File.Exists(pathToLaunch))
            {
                MessageBox.Show("The path: " + pathToLaunch + " does not exist!",
                    Form1.ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //set new gw path
                Form1.SetRegistry(pathToLaunch);

                //attempt to launch
                Form1.LaunchGame(pathToLaunch, pathArgs, false);
            }
        }

        static void LaunchCycler(FileManager fileCloset)
        {
            bool copyLaunched = false;

            foreach (KeyValuePair<string, string> i in fileCloset.Profiles)
            {
                String currentPath = i.Key;
                if (Form1.IsCopyRunning(currentPath) == false)
                {
                    LaunchByArguments(currentPath, i.Value);
                    copyLaunched = true;
                    break;
                }
            }

            if (copyLaunched == false)
            {
                MessageBox.Show("No more copies left to launch. Add more copies to GWMultilaunch.", 
                    "Unable to launch more copies.", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}