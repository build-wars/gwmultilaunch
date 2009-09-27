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
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace GWMultiLaunch
{
    public class SettingsManager
    {
        #region Native Method Signatures

        [DllImport("kernel32.dll")]
        private static extern uint GetPrivateProfileString(string lpAppName,
           string lpKeyName, string lpDefault, StringBuilder lpReturnedString,
           uint nSize, string lpFileName);

        [DllImport("kernel32.dll")]
        private static extern bool WritePrivateProfileString(string lpAppName,
           string lpKeyName, string lpString, string lpFileName);

        [DllImport("kernel32.dll")]
        private static extern bool WritePrivateProfileSection(string lpAppName,
           string lpString, string lpFileName);
        
        #endregion

        #region Structures

        private struct LaunchProfiles
        {
            public int[] SelectedIndices;
            public Dictionary<string, string> Profiles;
        }

        #endregion

        #region Constants

        private const string INI_FILENAME = "GWMultiLaunch.ini";

        private const string OPTIONS_SECTION = "options";
        private const string TEXMOD_PATH_KEY = "texmodpath";
        private const string REG_DELAY_NAME = "regdelay";
        private const int DEFAULT_REG_DELAY = 3000;
        private const string DAT_UNLOCK_KEY = "forcedatunlock";

        private const string PROFILES_SECTION = "profiles";
        private const string SELECTED_KEY_NAME = "selected";
        private const string PROFILE_KEY_NAME = "copy";
        private const char NUM_SPLIT_CHAR = ',';
        private const char PATH_ARG_SPLIT_CHAR = '|';

        #endregion

        #region Member Variables

        private LaunchProfiles mProfiles;
        private string mINIFilePath;
        private string mTexModPath;
        private int mRegistryCooldown;
        private bool mForceUnlock;

        #endregion

        # region Properties

        public bool ForceUnlock
        {
            get { return mForceUnlock; }
            set { mForceUnlock = value; }
        }

        public Dictionary<string, string> Profiles
        {
            get { return mProfiles.Profiles; }
        }

        public int RegistryCooldown
        {
            get { return mRegistryCooldown; }
        }

        public int[] SelectedIndices
        {
            get { return mProfiles.SelectedIndices; }
            set { mProfiles.SelectedIndices = value; }
        }

        public string TexModPath
        {
            get { return mTexModPath; }
            set { mTexModPath = value; }
        }

        #endregion

        #region Functions

        public SettingsManager()
        {
            mINIFilePath = Directory.GetCurrentDirectory() + "\\" + INI_FILENAME;
 
            Load();
        }

        ~SettingsManager()
        {
            Save();
        }

        public void AddProfile(string pathToAdd, string argument)
        {
            mProfiles.Profiles[pathToAdd] = argument;
        }

        public void UpdateProfile(string pathToUpdate, string argument)
        {
            if (mProfiles.Profiles.ContainsKey(pathToUpdate) == false)
            {
                return;
            }
            mProfiles.Profiles[pathToUpdate] = argument;
        }

        public void RemoveProfile(string pathToRemove)
        {
            mProfiles.Profiles.Remove(pathToRemove);
        }

        public string GetArguments(string path)
        {
            string argument;

            mProfiles.Profiles.TryGetValue(path, out argument);

            return argument;
        }

        private void Load()
        {
            //Load TexMod path
            mTexModPath = GetIniValue(OPTIONS_SECTION, TEXMOD_PATH_KEY, mINIFilePath);

            //Load registry delay value
            LoadRegDelay();

            //Load ForceDatUnlock value
            LoadForceUnlockValue();

            //Load launch profiles
            LoadProfiles();
        }

        private void LoadRegDelay()
        {
            string sRegistryCoolDown = GetIniValue(OPTIONS_SECTION, REG_DELAY_NAME, mINIFilePath);

            if (Int32.TryParse(sRegistryCoolDown, out mRegistryCooldown) == false)
            {
                mRegistryCooldown = DEFAULT_REG_DELAY;
            }
        }

        private void LoadForceUnlockValue()
        {
            string sForceUnlock = GetIniValue(OPTIONS_SECTION, DAT_UNLOCK_KEY, mINIFilePath);
            Boolean.TryParse(sForceUnlock, out mForceUnlock);
        }

        private void LoadProfiles()
        {
            LoadSelectedIndexes();
            LoadProfileList();
        }

        private void LoadSelectedIndexes()
        {
            string selectionString = GetIniValue(PROFILES_SECTION, SELECTED_KEY_NAME, mINIFilePath);
            string[] selectionStringArray = selectionString.Split(NUM_SPLIT_CHAR);

            mProfiles.SelectedIndices = new int[selectionStringArray.Length];

            for (int i = 0; i < selectionStringArray.Length; i++)
            {
                try
                {
                    mProfiles.SelectedIndices[i] = int.Parse(selectionStringArray[i]);
                }
                catch (Exception)
                {
                    mProfiles.SelectedIndices = new int[0];
                    break;
                }
            }
        }

        private void LoadProfileList()
        {
            mProfiles.Profiles = new Dictionary<string, string>();

            LoadProfileListFromINI();

            if (mProfiles.Profiles.Count == 0)
            {
                //add default install
                LoadInitialCopy();
            }
        }
        
        private void LoadProfileListFromINI()
        {
            string key;
            string profileString;

            for (int i = 0; ; i++)
            {
                key = PROFILE_KEY_NAME + i.ToString();
                profileString = GetIniValue(PROFILES_SECTION, key, mINIFilePath);

                string[] values = profileString.Split(PATH_ARG_SPLIT_CHAR);

                if (values.Length >= 2)
                {
                    mProfiles.Profiles.Add(values[0], values[1]);
                }
                else
                {
                    break;
                }
            }
        }

        private void LoadInitialCopy()
        {
            string gwPath = RegistryManager.GetGWRegPath();
            string gwArg = Program.DEFAULT_ARGUMENT;

            if (gwPath != string.Empty)
            {
                mProfiles.Profiles.Add(gwPath, gwArg);
            }
        }

        private void Save()
        {
            //Write TexMod path
            WriteINIValue(OPTIONS_SECTION, TEXMOD_PATH_KEY, mTexModPath, mINIFilePath);

            //Write cooldown value
            WriteINIValue(OPTIONS_SECTION, REG_DELAY_NAME, mRegistryCooldown.ToString(), mINIFilePath);

            //Write force unlock option
            WriteINIValue(OPTIONS_SECTION, DAT_UNLOCK_KEY, mForceUnlock.ToString(), mINIFilePath);

            //Save launch profiles
            SaveProfiles();
        }

        private void SaveProfiles()
        {
            ClearINISection(PROFILES_SECTION, mINIFilePath);
            SaveSelectedIndexes();
            SaveProfileList();
        }

        private void SaveSelectedIndexes()
        {
            StringBuilder selectionString = new StringBuilder();

            foreach (int i in SelectedIndices)
            {
                if (selectionString.Length > 0)
                {
                    selectionString.Append(NUM_SPLIT_CHAR);
                }

                selectionString.Append(i);
            }

            WriteINIValue(PROFILES_SECTION, SELECTED_KEY_NAME, selectionString.ToString(), mINIFilePath);
        }

        private void SaveProfileList()
        {
            int j = 0;
            string key;
            string value;

            foreach (KeyValuePair<string, string> kvp in mProfiles.Profiles)
            {
                key = PROFILE_KEY_NAME + j.ToString();
                value = kvp.Key + PATH_ARG_SPLIT_CHAR + kvp.Value;

                WriteINIValue(PROFILES_SECTION, key, value, mINIFilePath);

                j++;
            }
        }

        private static string GetIniValue(string section, string key, string filename)
        {
            StringBuilder sb = new StringBuilder(256);
            GetPrivateProfileString(section, key, string.Empty, sb, (uint)sb.Capacity, filename);
            return sb.ToString();
        }

        private static bool WriteINIValue(string section, string key, string value, string filename)
        {
            bool result = WritePrivateProfileString(section, key, value, filename);
            return result;
        }

        private static bool ClearINISection(string section, string filename)
        {
            bool result = WritePrivateProfileSection(section, string.Empty, filename);
            return result;
        }

        #endregion
    }
}
