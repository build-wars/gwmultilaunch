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
using IWshRuntimeLibrary;

namespace GWMultiLaunch
{
    public static class ShortcutCreator
    {
        /// <summary>
        /// Create shortcut for launching specified Guild Wars install.
        /// </summary>
        /// <param name="gwPath">Path to Guild Wars executable.</param>
        /// <param name="gwArgs">Arguments to pass to Guild Wars.</param>
        /// <returns></returns>
        public static bool CreateSingleLaunchShortcut(string gwPath, string gwArgs)
        {
            string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string pathLink = GetUnusedFilePath(desktopFolder, Program.SHORTCUT_PREFIX);
            string targetPath = System.Windows.Forms.Application.ExecutablePath;
            string arguments = "\"" + gwPath + "\"" + " " + gwArgs;
            string iconLocation = gwPath + ", 0";

            return CreateShortcut(pathLink, targetPath, arguments, iconLocation);
        }

        /// <summary>
        /// Create master shortcut for launching.
        /// </summary>
        /// <returns></returns>
        public static bool CreateMasterShortcut()
        {
            string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string pathLink = desktopFolder + "\\" + Program.AUTO_LAUNCH_SHORTCUT + ".lnk";
            string targetPath = System.Windows.Forms.Application.ExecutablePath;
            string arguments = Program.AUTO_LAUNCH_SWITCH;
            string iconLocation = targetPath + ", 0";

            return CreateShortcut(pathLink, targetPath, arguments, iconLocation);
        }

        /// <summary>
        /// Create shortcut.
        /// </summary>
        /// <param name="pathLink">Full path for new shortcut file.</param>
        /// <param name="targetPath">Shortcut target.</param>
        /// <param name="arguments">Arguments to pass to shortcut target.</param>
        /// <param name="iconLocation">Location of icon.</param>
        /// <returns></returns>
        private static bool CreateShortcut(string pathLink, string targetPath, string arguments, string iconLocation)
        {
            bool success = false;

            try
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(pathLink);
                shortcut.TargetPath = targetPath;
                shortcut.Arguments = arguments;
                shortcut.WorkingDirectory = Directory.GetParent(targetPath).FullName;
                shortcut.IconLocation = iconLocation;
                shortcut.Save();
                success = true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }

            return success;
        }

        /// <summary>
        /// Retrieves the target of specified shortcut.
        /// </summary>
        /// <param name="pathLink">Full path to shortcut file.</param>
        /// <returns></returns>
        public static string GetShortcutTarget(string pathLink)
        {
            IWshShell shell = new WshShell();

            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(pathLink);

            return shortcut.TargetPath;
        }

        /// <summary>
        /// Retrieves the arguments of specified shortcut.
        /// </summary>
        /// <param name="pathLink">Full path to shortcut file.</param>
        /// <returns></returns>
        public static string GetShortcutArguments(string pathLink)
        {
            IWshShell shell = new WshShell();

            IWshShortcut tmpShortcut = (IWshShortcut)shell.CreateShortcut(pathLink);

            return tmpShortcut.Arguments;
        }

        /// <summary>
        /// Retrieve full path to unused filename in speicfied folder.
        /// </summary>
        /// <param name="folder">Folder to check.</param>
        /// <param name="filenamePrefix">Prefix to use in constructing filename.</param>
        /// <returns></returns>
        private static string GetUnusedFilePath(string folder, string filenamePrefix)
        {
            string filename = string.Empty;
            string filePath = string.Empty;

            //there should not be over 100.. of these icons...
            for (int i = 1; i < 100; i++)
            {
                filename = filenamePrefix + i.ToString();
                filePath = folder + "\\" + filename + ".lnk";

                if (System.IO.File.Exists(filePath) == false)
                {
                    return filePath;
                }
            }

            return string.Empty;
        }
    }
}
