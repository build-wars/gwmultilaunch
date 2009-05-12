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
using System.Diagnostics;
using Microsoft.Win32;

namespace GWMultiLaunch
{
    public partial class Form1 : Form
    {
        #region Constants

        private const string DEFAULT_ARGUMENT = "-windowed";
        private const string MUTEX_MATCH_STRING = "AN-Mutex-Window";
        private const string GW_REG_LOCATION = "SOFTWARE\\ArenaNet\\Guild Wars";

        #endregion

        #region Member Variables

        private FileManager mFileCloset;
        private string mSelectedPath;

        #endregion

        #region Functions

        public Form1(FileManager fileCloset)
        {
            InitializeComponent();

            mFileCloset = fileCloset;
            mSelectedPath = string.Empty;

            InitializeInstallList();
        }

        public static bool ClearMutex()
        {
            bool success = false;

            //get list of currently running system processes
            Process[] processList = Process.GetProcesses();

            foreach (Process j in processList)
            {
                //filter for guild wars ones
                if (j.ProcessName.Equals("gw", StringComparison.OrdinalIgnoreCase))
                {
                    if (HandleManager.KillHandle(j, MUTEX_MATCH_STRING))
                    {
                        success = true;
                    }
                }
            }

            return success;
        }

        public static string GetCurrentGuildWarsPath()
        {
            RegistryKey localMachineKey = Registry.LocalMachine;    //for xp
            RegistryKey currentUserKey = Registry.CurrentUser;      //for vista/7

            try
            {
                RegistryKey activeKey;

                activeKey = localMachineKey.OpenSubKey(GW_REG_LOCATION, false);

                //will be null for vista/windows 7
                if (activeKey == null)
                {
                    activeKey = currentUserKey.OpenSubKey(GW_REG_LOCATION, false);
                }

                return activeKey.GetValue("Path").ToString();
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        private string GetSelectedInstall()
        {
            object selectedItem = profilesListBox.SelectedItem;

            if (selectedItem != null)
            {
                string selectedInstall = profilesListBox.SelectedItem.ToString();

                if (selectedInstall.Length > 0)
                {
                    return selectedInstall;
                }
            }

            return String.Empty;
        }

        private string[] GetSelectedInstalls()
        {
            string[] selectedItems = new string[profilesListBox.SelectedItems.Count];
            profilesListBox.SelectedItems.CopyTo(selectedItems, 0);

            return selectedItems;
        }

        private void InitializeInstallList()
        { 
            //Populate listbox with installs
            foreach (KeyValuePair<string, string> i in mFileCloset.Profiles)
            {
                profilesListBox.Items.Add(i.Key);
            }

            //Select Proper Items
            foreach (int i in mFileCloset.SelectedIndices)
            {
                profilesListBox.SelectedIndices.Add(i);
            }
        }

        public static bool LaunchGame(string path, string args)
        {
            bool success = false;

            Process gw = new Process();
            gw.StartInfo.FileName = path;
            gw.StartInfo.Arguments = args;
            gw.StartInfo.WorkingDirectory = Directory.GetParent(path).FullName;
            gw.StartInfo.UseShellExecute = true;

            try
            {
                gw.Start();
                success = true;
            }
            catch (Exception)
            {
                return success;
            }

            return success;
        }

        private bool LaunchProgram(string path)
        {
            Process prog = new Process();
            prog.StartInfo.FileName = path;

            try
            {
                prog.Start();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        public static bool SetRegistry(string gwPath)
        {
            RegistryKey localMachineKey = Registry.LocalMachine;    //for xp
            RegistryKey currentUserKey = Registry.CurrentUser;      //for vista/7

            try
            {
                RegistryKey activeKey;

                activeKey = localMachineKey.OpenSubKey(GW_REG_LOCATION, true);

                //will be null for vista/windows 7
                if (activeKey == null)
                {
                    activeKey = currentUserKey.OpenSubKey(GW_REG_LOCATION, true);
                }

                activeKey.SetValue("Path", gwPath);
                activeKey.SetValue("Src", gwPath);

                activeKey.Close();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Event Handlers

        private void AddCopyButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Guild Wars Executable (*.exe)|*.exe";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (!profilesListBox.Items.Contains(dlg.FileName))
                {
                    //add to file
                    mFileCloset.AddProfile(dlg.FileName, DEFAULT_ARGUMENT);

                    //add to list
                    int index = profilesListBox.Items.Add(dlg.FileName);

                    //deselect all
                    profilesListBox.SelectedIndex = -1;
                }
            }
        }

        private void ArgumentsTextBox_Leave(object sender, EventArgs e)
        {
            if (mSelectedPath != string.Empty)
            {
                mFileCloset.UpdateProfile(mSelectedPath, argumentsTextBox.Text);
            }
        }

        private void LaunchButton_Click(object sender, EventArgs e)
        {
            string[] selectedInstalls = GetSelectedInstalls();

            if (selectedInstalls.Length == 0)
            {
                MessageBox.Show("No installation selected!");
            }
            else
            {
                string selectedInstall = string.Empty;

                for (int i = 0; i < selectedInstalls.Length; i++)
                {
                    selectedInstall = selectedInstalls[i];
                    if (!File.Exists(selectedInstall))
                    {
                        MessageBox.Show("The path: " + selectedInstall + " does not exist!");
                    }

                    //get current gw path from registry
                    string currentPath = Form1.GetCurrentGuildWarsPath();

                    //set new gw path
                    SetRegistry(selectedInstall);

                    ClearMutex();
                    LaunchGame(selectedInstall, mFileCloset.GetArgument(selectedInstall));

                    //delay for defined valued in ini file
                    //gives time for gw to catch the path for updating the right install
                    System.Threading.Thread.Sleep(mFileCloset.RegistryCooldown);

                    //set back to saved path
                    Form1.SetRegistry(currentPath);
                }
            }
        }

        private void MutexButton_Click(object sender, EventArgs e)
        {
            ClearMutex();
        }

        private void ProfilesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //update launch arguments box of previous item
            if (mSelectedPath != string.Empty)
            {
                mFileCloset.UpdateProfile(mSelectedPath, argumentsTextBox.Text);
            }

            argumentsTextBox.Text = string.Empty;

            if (profilesListBox.SelectedIndices.Count > 1)
            {
                //multiple items selected
                mSelectedPath = string.Empty;
                argumentsTextBox.Enabled = false;

                regButton.Enabled = false;
                removeCopyButton.Enabled = true;
                launchButton.Enabled = true;
                shortcutButton.Enabled = true;

                int[] indicesArr = new int[profilesListBox.SelectedIndices.Count];
                profilesListBox.SelectedIndices.CopyTo(indicesArr, 0);
                mFileCloset.SelectedIndices = indicesArr;
            }
            else if (profilesListBox.SelectedIndices.Count == 1)
            {
                //one item selected
                mSelectedPath = profilesListBox.SelectedItem.ToString();
                argumentsTextBox.Enabled = true;
                argumentsTextBox.Text = mFileCloset.GetArgument(mSelectedPath);

                regButton.Enabled = true;
                removeCopyButton.Enabled = true;
                launchButton.Enabled = true;
                shortcutButton.Enabled = true;

                mFileCloset.SelectedIndices = new int[] { profilesListBox.SelectedIndex };
            }
            else
            {
                //nothing selected
                mSelectedPath = string.Empty;
                argumentsTextBox.Enabled = false;

                regButton.Enabled = false;
                removeCopyButton.Enabled = false;
                launchButton.Enabled = false;
                shortcutButton.Enabled = false;

                mFileCloset.SelectedIndices = new int[0];
            }
        }

        private void RegButton_Click(object sender, EventArgs e)
        {
            string selectedInstall = GetSelectedInstall();

            if (selectedInstall == String.Empty)
            {
                MessageBox.Show("No installation selected!");
            }
            else
            {
                SetRegistry(selectedInstall);
            }
        }

        private void RemoveCopyButton_Click(object sender, EventArgs e)
        {
            string[] selectedInstalls = GetSelectedInstalls();

            if (selectedInstalls.Length == 0)
            {
                MessageBox.Show("No installation selected!");
            }
            else
            {
                foreach (string selectedInstall in selectedInstalls)
                {
                    //remove from file
                    mFileCloset.RemoveProfile(selectedInstall);

                    //remove from list
                    mSelectedPath = string.Empty;
                    profilesListBox.Items.Remove(selectedInstall);
                }
            }
        }

        private void ShortcutButton_Click(object sender, EventArgs e)
        {
            int nShortcutsCreated = 0;
            string gwmlPath = System.Windows.Forms.Application.ExecutablePath;
            string arg = string.Empty;

            foreach (string path in profilesListBox.SelectedItems)
            {
                arg = mFileCloset.GetArgument(path);

                if (ShortcutCreator.CreateDesktopShortcut(gwmlPath, path, arg))
                {
                    nShortcutsCreated++;
                }
            }

            MessageBox.Show(nShortcutsCreated.ToString() + " Desktop shortcuts were created!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TexmodButton_Click(object sender, EventArgs e)
        {
            string texmodPath = string.Empty;

            //check for path from ini
            string iniTextmodPath = mFileCloset.TexmodPath;

            if (File.Exists(iniTextmodPath))
            {
                texmodPath = iniTextmodPath;
            }

            //let's search for it
            if (texmodPath == string.Empty)
            {
                //use guild wars folder as starting point to start search
                string gwPath = GetCurrentGuildWarsPath();

                if (File.Exists(gwPath))
                {
                    string directory = Directory.GetParent(gwPath).FullName;

                    try
                    {
                        string[] texmodFiles = Directory.GetFiles(directory, "texmod.exe", SearchOption.AllDirectories);

                        if (texmodFiles.Length > 0)
                        {
                            texmodPath = texmodFiles[0];    //use first match
                        }
                    }
                    catch (Exception) 
                    {
                        texmodPath = string.Empty;
                    }
                }
            }

            //ask user for it
            if (texmodPath == string.Empty)
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Title = "Locate TexMod!";
                dlg.Filter = "TexMod Executable (*.exe)|*.exe";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    texmodPath = dlg.FileName;
                }
            }

            if (texmodPath.Length > 0)
            {
                //attempt launch
                bool launchSuccess = LaunchProgram(texmodPath);

                if (launchSuccess == true)
                {
                    //write path to ini
                    if (iniTextmodPath != texmodPath)
                    {
                        mFileCloset.TexmodPath = texmodPath;
                    }
                }
                else
                {
                    //remove path from ini
                    mFileCloset.TexmodPath = string.Empty;
                }
            }
        }

        #endregion
    }
}