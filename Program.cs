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
                // Shortcut launching mode
                string pathToLaunch = args[0];
                string pathArgs = string.Empty;

                if (args.Length >= 2)
                {
                    pathArgs = args[1];
                }

                //validate path
                if (!File.Exists(pathToLaunch))
                {
                    MessageBox.Show("The path: " + pathToLaunch + " does not exist! Check the shortcut arguments.", 
                        Form1.ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //set new gw path
                    Form1.SetRegistry(pathToLaunch);

                    //attempt to launch
                    Form1.LaunchGame(pathToLaunch, pathArgs);
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
    }
}