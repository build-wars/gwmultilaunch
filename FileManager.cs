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
    public class FileManager
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
        private const int DEFAULT_REG_DELAY = 1000;

        private const string PROFILES_SECTION = "profiles";
        private const string SELECTED_KEY_NAME = "selected";
        private const string PROFILE_KEY_NAME = "copy";
        private const char NUM_SPLIT_CHAR = ',';
        private const char PATHARG_SPLIT_CHAR = '|';

        private LaunchProfiles mProfiles;
        private string mINIFilePath;
        private string mTexmodPath;
        private int mRegistryCooldown;

        #endregion

        # region Properties

        public string TexmodPath
        {
            get { return mTexmodPath; }
            set { mTexmodPath = value; }
        }

        public int[] SelectedIndices
        {
            get { return mProfiles.SelectedIndices; }
            set { mProfiles.SelectedIndices = value; }
        }

        public Dictionary<string, string> Profiles
        {
            get { return mProfiles.Profiles; }
        }

        public int RegistryCooldown
        {
            get { return mRegistryCooldown; }
        }

        #endregion

        #region Functions

        public FileManager()
        {
            mINIFilePath = Directory.GetCurrentDirectory() + "\\" + INI_FILENAME;
 
            Load();
        }

        ~FileManager()
        {
            Save();
        }

        public void AddProfile(string pathToAdd, string argument)
        {
            UpdateProfile(pathToAdd, argument);
        }

        public void UpdateProfile(string pathToUpdate, string argument)
        {
            try
            {
                mProfiles.Profiles[pathToUpdate] = argument;
            }
            catch (Exception)
            {
                //hmmm...
            }
        }

        public void RemoveProfile(string pathToRemove)
        {
            mProfiles.Profiles.Remove(pathToRemove);
        }

        public string GetArgument(string path)
        {
            string argument;
            mProfiles.Profiles.TryGetValue(path, out argument);

            return argument;
        }

        private void Load()
        {
            //Load TexmodPath from ini
            mTexmodPath = GetIniValue(OPTIONS_SECTION, TEXMOD_PATH_KEY, mINIFilePath);

            //Get cooldown value
            mRegistryCooldown = GetRegDelay();

            //Load launch profiles from ini
            mProfiles = LoadProfiles();
        }

        private void Save()
        {
            //Write TexmodPath to ini
            WriteINIValue(OPTIONS_SECTION, TEXMOD_PATH_KEY, mTexmodPath, mINIFilePath);

            //Write cooldown value
            WriteINIValue(OPTIONS_SECTION, REG_DELAY_NAME, mRegistryCooldown.ToString(), mINIFilePath);

            //Write launch profiles to ini
            WriteProfiles();
        }

        private int GetRegDelay()
        {
            int registryCooldown = DEFAULT_REG_DELAY;
            
            string rawValue = GetIniValue(OPTIONS_SECTION, REG_DELAY_NAME, mINIFilePath);
            if (rawValue != string.Empty)
            {
                try
                {
                    registryCooldown = int.Parse(rawValue);
                }
                catch (Exception)
                {
                    registryCooldown = DEFAULT_REG_DELAY;
                }
            }

            return registryCooldown;
        }   

        private LaunchProfiles LoadProfiles()
        {
            LaunchProfiles profiles = new LaunchProfiles();

            // Get selected indexes
            string selectionString = GetIniValue(PROFILES_SECTION, SELECTED_KEY_NAME, mINIFilePath);
            string[] selectionStringArray = selectionString.Split(NUM_SPLIT_CHAR);

            profiles.SelectedIndices = new int[selectionStringArray.Length];

            for (int i = 0; i < selectionStringArray.Length; i++)
            {
                try
                {
                    profiles.SelectedIndices[i] = int.Parse(selectionStringArray[i]);
                }
                catch (Exception)
                {
                    profiles.SelectedIndices = new int[0];
                    break;
                }
            }

            // Get launch profiles
            profiles.Profiles = new Dictionary<string, string>();

            int j = 0;
            string key;
            string value;
            bool valueRead = true;

            do
            {
                key = PROFILE_KEY_NAME + j.ToString();
                value = GetIniValue(PROFILES_SECTION, key, mINIFilePath);

                if (value == string.Empty)
                {
                    valueRead = false;      //nothing found, lets stop reading

                    if (j == 0)
                    {
                        //try to at least add the path gw is installed to
                        string gwPath = Form1.GetCurrentGuildWarsPath();
                        string gwArg = Form1.DEFAULT_ARGUMENT;

                        if (gwPath != string.Empty)
                        {
                            profiles.Profiles.Add(gwPath, gwArg);
                        }
                    }
                }
                else
                {
                    string[] values = value.Split(PATHARG_SPLIT_CHAR);

                    if (values.Length < 2)
                    {
                        System.Windows.Forms.MessageBox.Show("Ini file error. The value for the copy \"" + value + "\" is malformed.");
                    }
                    else
                    {
                        profiles.Profiles.Add(values[0], values[1]);
                    }

                    j++;
                }
            } while (valueRead);


            return profiles;
        }

        private void WriteProfiles()
        {
            //Lets clear the section first
            ClearINISection(PROFILES_SECTION, mINIFilePath);

            string selectionString = string.Empty;
            foreach (int i in SelectedIndices)
            {
                if (selectionString != string.Empty)
                {
                    selectionString = selectionString + NUM_SPLIT_CHAR.ToString() + i.ToString();
                }
                else
                {
                    //first number, do not add comma before it
                    selectionString = i.ToString();
                }
            }
            

            //Write the data
            WriteINIValue(PROFILES_SECTION, SELECTED_KEY_NAME, selectionString, mINIFilePath);

            int j = 0;
            string key;
            string value;
            
            foreach (KeyValuePair<string, string> kvp in mProfiles.Profiles)
            {
                key = PROFILE_KEY_NAME + j.ToString();
                value = kvp.Key + PATHARG_SPLIT_CHAR + kvp.Value;

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
