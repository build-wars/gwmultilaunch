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
using IWshRuntimeLibrary;

namespace GWMultiLaunch
{
    class ShortcutCreator
    {
        private const string GWML_PREFIX = "Guild War ML-";

        public static bool CreateDesktopShortcut(string path, string gwPath, string gwArg)
        {
            bool success = false;

            string arg = "\"" + gwPath + "\"" + " " + "\"" + gwArg + "\"";

            string shortcutPath = GetUnusedShortcutPath();
            if (shortcutPath == string.Empty) return false;
            
            try
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                shortcut.TargetPath = path;
                shortcut.Arguments = arg;
                shortcut.WorkingDirectory = Directory.GetParent(path).FullName;
                shortcut.IconLocation = gwPath + ", 0";
                shortcut.Save();
                success = true;
            }
            catch (Exception e) 
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }

            return success;
        }

        public static string GetShortcutTarget(string shortcutFile)
        {
            IWshShell shell = new WshShell();

            IWshShortcut tmpShortcut = (IWshShortcut)shell.CreateShortcut(shortcutFile);

            return tmpShortcut.TargetPath;
        }

        public static string GetShortcutArguments(string shortcutFile)
        {
            IWshShell shell = new WshShell();

            IWshShortcut tmpShortcut = (IWshShortcut)shell.CreateShortcut(shortcutFile);

            return tmpShortcut.Arguments;
        }

        private static string GetUnusedShortcutPath()
        {
            string name = string.Empty;
            string pathToCheck = string.Empty;
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            //there should not be over 100.. of these icons...
            for (int i = 1; i < 100; i++)
            {
                name = GWML_PREFIX + i.ToString();
                pathToCheck = desktopPath + "\\" + name + ".lnk";

                if (!System.IO.File.Exists(pathToCheck))
                {
                    return pathToCheck;
                }
            }

            return string.Empty;
        }
    }
}
