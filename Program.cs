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

            if (args.Length >= 2)
            {
                // Shortcut launching mode

                //get current gw path from registry
                string currentPath = Form1.GetCurrentGuildWarsPath();

                //set new gw path
                Form1.SetRegistry(args[0]);

                //clear mutex
                Form1.ClearMutex();

                Form1.LaunchGame(args[0], args[1]);

                //delay for defined valued in ini file
                System.Threading.Thread.Sleep(fileCloset.RegistryCooldown);

                //set back to saved path
                Form1.SetRegistry(currentPath);

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