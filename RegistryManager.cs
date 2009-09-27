//Guild Wars MultiLaunch - Safe and efficient way to launch multiple GWs.
//The Guild Wars executable is never modified, keeping you inline with the tos.
//
//Copyright (C) 2009  IMKey@IMKey@GuildWarsGuru

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
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace GWMultiLaunch
{
    public class RegistryManager
    {
        #region Native Method Signatures

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        private static extern int RegOpenKeyEx(
          UIntPtr hKey,
          string subKey,
          int ulOptions,
          SamDesiredFlags samDesired,
          out UIntPtr hkResult);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int RegCloseKey(
            UIntPtr hKey);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int RegSetValueEx(
            UIntPtr hKey,
            [MarshalAs(UnmanagedType.LPStr)] string lpValueName,
            int Reserved,
            Microsoft.Win32.RegistryValueKind dwType,
            [MarshalAs(UnmanagedType.LPStr)] string lpData,
            int cbData);

        #endregion

        #region Constants

        private static UIntPtr HKEY_CURRENT_USER = new UIntPtr(0x80000001u);
        private static UIntPtr HKEY_LOCAL_MACHINE = new UIntPtr(0x80000002u);

        [Flags]
        private enum SamDesiredFlags : int
        {
            KEY_QUERY_VALUE = 0x0001,
            KEY_SET_VALUE = 0x0002,
            KEY_CREATE_SUB_KEY = 0x0004,
            KEY_ENUMERATE_SUB_KEYS = 0x0008,
            KEY_NOTIFY = 0x0010,
            KEY_CREATE_LINK = 0x0020,
            KEY_WOW64_64KEY = 0x0100,
            KEY_WOW64_32KEY = 0x0200,
            KEY_WOW64_RES = 0x0300,
        }

        #endregion

        private static bool Write32bitKey(UIntPtr hKey, string subKey, string key, string value)
        {
            bool success = false;

            if (RegOpenKeyEx(hKey, subKey, 0, (SamDesiredFlags.KEY_SET_VALUE | SamDesiredFlags.KEY_WOW64_32KEY), 
                out hKey) == 0)
            {
                int valueSize = (value.Length + 1) * Marshal.SystemDefaultCharSize;
                if (RegSetValueEx(hKey, key, 0,
                    RegistryValueKind.String, value, valueSize) == 0)
                {
                    success = true;
                }
           
                RegCloseKey(hKey);
            }

            return success;
        }

        public static string GetGWRegPath()
        {
            //the path could be stored in one of two locations
            //so we should try both.
            RegistryKey currentUserKey = Registry.CurrentUser;      //for user installs
            RegistryKey localMachineKey = Registry.LocalMachine;    //for machine installs

            try
            {
                RegistryKey activeKey;

                activeKey = currentUserKey.OpenSubKey(Program.GW_REG_LOCATION, false);

                if (activeKey == null)
                {
                    activeKey = localMachineKey.OpenSubKey(Program.GW_REG_LOCATION, false);
                }

                if (activeKey == null)
                {
                    activeKey = currentUserKey.OpenSubKey(Program.GW_REG_LOCATION_AUX, false);
                }

                if (activeKey == null)
                {
                    activeKey = localMachineKey.OpenSubKey(Program.GW_REG_LOCATION_AUX, false);
                }
                
                return activeKey.GetValue("Path").ToString();
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        public static bool SetGWRegPath(string gwPath)
        {
            //the path could be stored in one of two locations
            //so we should try both.
            RegistryKey currentUserKey = Registry.CurrentUser;      //for user installs
            RegistryKey localMachineKey = Registry.LocalMachine;    //for machine installs

            try
            {
                RegistryKey activeKey;

                activeKey = currentUserKey.OpenSubKey(Program.GW_REG_LOCATION, true);
                if (activeKey != null)
                {
                    activeKey.SetValue("Path", gwPath);
                    activeKey.SetValue("Src", gwPath);
                    activeKey.Close();
                }

                activeKey = localMachineKey.OpenSubKey(Program.GW_REG_LOCATION, true);
                if (activeKey != null)
                {
                    activeKey.SetValue("Path", gwPath);
                    activeKey.SetValue("Src", gwPath);
                    activeKey.Close();
                }

                activeKey = currentUserKey.OpenSubKey(Program.GW_REG_LOCATION_AUX, true);
                if (activeKey != null)
                {
                    activeKey.SetValue("Path", gwPath);
                    activeKey.SetValue("Src", gwPath);
                    activeKey.Close();
                }

                activeKey = localMachineKey.OpenSubKey(Program.GW_REG_LOCATION_AUX, true);
                if (activeKey != null)
                {
                    activeKey.SetValue("Path", gwPath);
                    activeKey.SetValue("Src", gwPath);
                    activeKey.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "Please run launcher as administrator.",
                    Program.ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
    }
}
