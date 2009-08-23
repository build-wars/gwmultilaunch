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

//comment out for 64 bit builds
#define BUILD32

using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;

namespace GWMultiLaunch
{
    /// DEPRECATED ///
    /// SEE MainForm.cs ///
    public partial class Form1 : Form
    {
        #region Member Variables

        private SettingsManager mSettings;
        private string mSelectedPath;

        #endregion

        #region Functions

        public Form1()
        {
            InitializeComponent();

            mSettings = Program.settings;
            mSelectedPath = string.Empty;

            InitializeInstallList();

            if (!mSettings.ForceUnlock)
            {
                forceLaunchCheckBox.Checked = false;
            }

            #if (BUILD32)
            if (Environment.GetEnvironmentVariable("ProgramFiles(x86)") != null)
                {
                    MessageBox.Show(@"Warning: You are running the 32-bit build" + 
                    " of Guild Wars Multi-Launch under a 64-bit operating system. Functionality " +
                    "will be affected! Use the 64-bit version of GWML.", 
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                this.Text += " (32-bit)";
            #else
                this.Text += " (64-bit)";
            #endif
        }

        private bool AddCopy(string pathToAdd)
        {
            return AddCopy(pathToAdd, Program.DEFAULT_ARGUMENT);
        }

        private bool AddCopy(string pathToAdd, string pathArg)
        {
            bool success = false;

            if (!profilesListBox.Items.Contains(pathToAdd))
            {
                //add to file
                mSettings.AddProfile(pathToAdd, pathArg);

                //add to list
                int index = profilesListBox.Items.Add(pathToAdd);

                //deselect all
                profilesListBox.SelectedIndex = -1;

                success = true;
            }

            return success;
        }

        public static bool ClearFileLock(string basePath)
        {
            bool success = false;

            //take off the drive portion due to limitation in how killhandle works for file name
            string root = Directory.GetDirectoryRoot(basePath).Substring(0, 2);
            basePath = basePath.Replace(root, string.Empty);

            string fileToUnlock = basePath + "\\" + Program.GW_DAT;

            //get list of currently running system processes
            Process[] processList = Process.GetProcesses();

            foreach (Process i in processList)
            {
                //filter for guild wars ones
                if (i.ProcessName.Equals(Program.GW_PROCESS_NAME, StringComparison.OrdinalIgnoreCase))
                {
                    if (HandleManager.KillHandle(i, fileToUnlock, true))
                    {
                        success = true;
                    }
                }
            }

            return success;
        }

        public static bool ClearMutex()
        {
            bool success = false;

            //get list of currently running system processes
            Process[] processList = Process.GetProcesses();

            foreach (Process i in processList)
            {
                //filter for guild wars ones
                if (i.ProcessName.Equals(Program.GW_PROCESS_NAME, StringComparison.OrdinalIgnoreCase))
                {
                    if (HandleManager.KillHandle(i, Program.MUTEX_MATCH_STRING, false))
                    {
                        success = true;
                    }
                }
            }

            return success;
        }

        public static string GetCurrentGuildWarsPath()
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
            foreach (KeyValuePair<string, string> i in mSettings.Profiles)
            {
                profilesListBox.Items.Add(i.Key);
            }

            //Select Proper Items
            foreach (int i in mSettings.SelectedIndices)
            {
                profilesListBox.SelectedIndices.Add(i);
            }
        }

        public static bool IsCopyRunning(string path)
        {
            //get list of currently running system processes
            Process[] processList = Process.GetProcesses();

            foreach (Process i in processList)
            {
                //does process name match?
                if (i.ProcessName.Equals(Program.GW_PROCESS_NAME, StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        string processPath = i.MainModule.FileName;

                        //does filename match?
                        if (processPath.Equals(path, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        //Exception is caught if gw.exe is in the process of closing itself
                    }
                }
            }

            return false;
        }

        private void LaunchGame(string path)
        {
            //check if the install exists
            if (!File.Exists(path))
            {
                MessageBox.Show("The path: " + path + " does not exist!");

            }
            else
            {
                //set new gw path
                SetRegistry(path);

                bool forced = false;
                if (forceLaunchCheckBox.Checked)
                {
                    ClearFileLock(Directory.GetParent(path).FullName);
                    forced = true;
                }

                //attempt to launch
                if (LaunchGame(path, mSettings.GetArguments(path), forced))
                {
                    //give time for gw to read value before it gets changed again.
                    System.Threading.Thread.Sleep(mSettings.RegistryCooldown);
                }
            }
        }

        public static bool LaunchGame(string path, string args, bool forced)
        {
            bool success = false;

            if (!forced)
            {
                //check to see if this copy is already started
                if (IsCopyRunning(path))
                {
                    MessageBox.Show(path + " is already running, please launch a different copy.",
                        Program.ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return success;
                }
            }

            Process gw = new Process();
            gw.StartInfo.FileName = path;
            gw.StartInfo.Arguments = args;
            gw.StartInfo.WorkingDirectory = Directory.GetParent(path).FullName;
            gw.StartInfo.UseShellExecute = true;

            try
            {
                //clear mutex to allow for another gw launch
                ClearMutex();

                //attempt to start gw process
                gw.Start();

                success = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error launching: " + path + "!\n" + e.Message,
                    Program.ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return success;
        }

        private bool LaunchProgram(string path)
        {
            bool success = false;

            Process prog = new Process();
            prog.StartInfo.FileName = path;

            try
            {
                prog.Start();
                success = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return success;
        }

        private bool MakeGWCopy(string sourceFolder, string destFolder)
        {
            bool success = false;

            try
            {
                DirectoryInfo source = new DirectoryInfo(sourceFolder);
                DirectoryInfo destination = new DirectoryInfo(destFolder);

                if (source.FullName != destination.FullName)
                {
                    List<string> sourceFileList = new List<string>();
                    List<string> destFileList = new List<string>();

                    //include files in base directory
                    sourceFileList.AddRange(Directory.GetFiles(sourceFolder, "*.*", SearchOption.TopDirectoryOnly));

                    //include files in templates folder if available (Vista/7 have it in user documents, no need to copy)
                    string templateDir = sourceFolder + Program.GW_TEMPLATES;
                    if (Directory.Exists(templateDir))
                    {
                        sourceFileList.AddRange(Directory.GetFiles(templateDir, "*.*", SearchOption.AllDirectories));
                    }

                    //translate to destination paths
                    destFileList = GetDestFileList(sourceFileList, sourceFolder, destFolder);

                    //lets do the copying
                    success = FileCopier.CopyFiles(sourceFileList, destFileList);
                }
                else
                {
                    MessageBox.Show("Error, cannot copy to the same directory.", Program.ERROR_CAPTION, 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    success = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error occurred while copying Guild Wars from "
                    + sourceFolder + " to " + destFolder + "!\n" + e.Message, Program.ERROR_CAPTION,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                success = false;
            }

            return success;
        }

        public static bool SetRegistry(string gwPath)
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
                
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private List<string> GetDestFileList(List<string> sourceFileList, string sourceFolder, string destFolder)
        {
            List<string> destFileList = new List<string>();

            foreach (string filename in sourceFileList)
            {
                destFileList.Add(filename.Replace(sourceFolder, destFolder));
            }

            return destFileList;
        }

        #endregion

        #region Event Handlers

        private void AddCopyButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Guild Wars Executable (*.exe)|*.exe";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                AddCopy(dlg.FileName);
            }
        }

        private void ArgumentsTextBox_Leave(object sender, EventArgs e)
        {
            if (mSelectedPath != string.Empty)
            {
                mSettings.UpdateProfile(mSelectedPath, argumentsTextBox.Text);
            }
        }

        private void ForceLaunchCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (forceLaunchCheckBox.Checked)
            {
                DialogResult t = MessageBox.Show("This option is very experimental! Forcing an unlock may corrupt gw.dat. \n" +
                    "Guild Wars may crash. Only do a forced launch after first copy has fully loaded an area (item art including). " +
                    "Any graphics or text that have not been loaded into memory will not show up once gw.dat is detached. " +
                    "Use this option if you want to mule between characters in your guild hall and do not " +
                    "have the disk space to make an extra copy of Guild Wars.\n\n" +
                    "Are you sure you want to enable this option?",
                    "Experimental Option!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                if (t == DialogResult.No)
                {
                    forceLaunchCheckBox.Checked = false;
                }
                else
                {
                    mSettings.ForceUnlock = true;
                }
            }
            else
            {
                mSettings.ForceUnlock = false;
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

                    LaunchGame(selectedInstall);
                }
            }
        }

        private void MakeCopyButton_Click(object sender, EventArgs e)
        {
            string selectedInstall = GetSelectedInstall();

            if (selectedInstall == String.Empty)
            {
                MessageBox.Show("No installation selected!");
            }
            else if (!File.Exists(selectedInstall))
            {
                MessageBox.Show("Can not make a copy. The select install: " + selectedInstall + " does not exist!",
                    Program.ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                FolderBrowserDialog selectFolderDlg = new FolderBrowserDialog();
                selectFolderDlg.ShowNewFolderButton = true;
                selectFolderDlg.Description = "Select an empty folder to copy to. (Hint: Click \"Make New Folder\" button.)";
                selectFolderDlg.RootFolder = Environment.SpecialFolder.MyComputer;
                selectFolderDlg.SelectedPath = Directory.GetParent(selectedInstall).Parent.FullName;

                DialogResult result = selectFolderDlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    DialogResult confirm = MessageBox.Show("Are you sure you want to make a copy of Guild Wars at: " +
                        selectFolderDlg.SelectedPath + "?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (confirm == DialogResult.Yes)
                    {
                        // lets attempt to make the copy
                        bool copySuccess = MakeGWCopy(Directory.GetParent(selectedInstall).FullName, selectFolderDlg.SelectedPath);

                        if (copySuccess)
                        {
                            // lets add the new copy to the profile list
                            string gwPath = selectFolderDlg.SelectedPath + "\\" + Program.GW_FILENAME;
                            if (File.Exists(gwPath))
                            {
                                AddCopy(gwPath);
                            }
                        }
                    }
                }
            }
        }

        private void MutexButton_Click(object sender, EventArgs e)
        {
            ClearMutex();
        }

        private void ProfilesListBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] droppedFilenames = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            foreach (string filename in droppedFilenames)
            {
                string filePathToAdd = filename;
                string arguments = string.Empty;

                string fileExt = Path.GetExtension(filePathToAdd);
                if (fileExt.Equals(".lnk", StringComparison.OrdinalIgnoreCase))
                {
                    filePathToAdd = ShortcutCreator.GetShortcutTarget(filename);
                    arguments = ShortcutCreator.GetShortcutArguments(filename).Trim();
                }

                if (arguments == string.Empty)
                {
                    AddCopy(filePathToAdd);
                }
                else
                {
                    AddCopy(filePathToAdd, arguments);
                }
            }
        }

        private void ProfilesListBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void profilesListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int clickedIndex = profilesListBox.IndexFromPoint(e.X, e.Y);
            if (clickedIndex != ListBox.NoMatches)
            {
                try
                {
                    string selectedInstall = profilesListBox.Items[clickedIndex].ToString();

                    LaunchGame(selectedInstall);
                }
                catch (Exception t)
                {
                    MessageBox.Show(t.Message, Program.ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ProfilesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //update launch arguments box of previous item
            if (mSelectedPath != string.Empty)
            {
                mSettings.UpdateProfile(mSelectedPath, argumentsTextBox.Text);
            }

            argumentsTextBox.Text = string.Empty;

            if (profilesListBox.SelectedIndices.Count > 1)
            {
                //multiple items selected
                mSelectedPath = string.Empty;
                argumentsTextBox.Enabled = false;

                regButton.Enabled = false;
                removeCopyButton.Enabled = true;
                makeCopyButton.Enabled = false;
                launchButton.Enabled = true;
                shortcutButton.Enabled = true;

                int[] indicesArr = new int[profilesListBox.SelectedIndices.Count];
                profilesListBox.SelectedIndices.CopyTo(indicesArr, 0);
                mSettings.SelectedIndices = indicesArr;
            }
            else if (profilesListBox.SelectedIndices.Count == 1)
            {
                //one item selected
                mSelectedPath = profilesListBox.SelectedItem.ToString();
                argumentsTextBox.Enabled = true;
                argumentsTextBox.Text = mSettings.GetArguments(mSelectedPath);

                regButton.Enabled = true;
                removeCopyButton.Enabled = true;
                makeCopyButton.Enabled = true;
                launchButton.Enabled = true;
                shortcutButton.Enabled = true;

                mSettings.SelectedIndices = new int[] { profilesListBox.SelectedIndex };
            }
            else
            {
                //nothing selected
                mSelectedPath = string.Empty;
                argumentsTextBox.Enabled = false;

                regButton.Enabled = false;
                removeCopyButton.Enabled = false;
                makeCopyButton.Enabled = false;
                launchButton.Enabled = false;
                shortcutButton.Enabled = false;

                mSettings.SelectedIndices = new int[0];
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
                    mSettings.RemoveProfile(selectedInstall);

                    //remove from list
                    mSelectedPath = string.Empty;
                    profilesListBox.Items.Remove(selectedInstall);
                }
            }
        }

        private void ShortcutButton_Click(object sender, EventArgs e)
        {
            int nShortcutsCreated = 0;
            string gwArgs = string.Empty;
            
            foreach (string gwPath in profilesListBox.SelectedItems)
            {
                gwArgs = mSettings.GetArguments(gwPath);

                if (ShortcutCreator.CreateSingleLaunchShortcut(gwPath, gwArgs))
                {
                    nShortcutsCreated++;
                }
            }

            MessageBox.Show(nShortcutsCreated.ToString() + " Desktop shortcuts were created!", "Info", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AutomodeButton_Click(object sender, EventArgs e)
        {
            if (ShortcutCreator.CreateMasterShortcut())
            {
                MessageBox.Show("Master shortcut created!", 
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void TexmodButton_Click(object sender, EventArgs e)
        {
            string texmodPath = string.Empty;

            //check for path from ini
            string iniTextmodPath = mSettings.TexModPath;

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
                        mSettings.TexModPath = texmodPath;
                    }
                }
                else
                {
                    //remove path from ini
                    mSettings.TexModPath = string.Empty;
                }
            }
        }

        #endregion

    }
}