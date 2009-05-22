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

        public const string DEFAULT_ARGUMENT = "-windowed";
        public const string ERROR_CAPTION = "GWMultiLaunch Error";
        private const string MUTEX_MATCH_STRING = "AN-Mutex-Window";
        private const string GW_REG_LOCATION = "SOFTWARE\\ArenaNet\\Guild Wars";
        private const string GW_PROCESS_NAME = "Gw";
        private const string GW_FILENAME = "Gw.exe";
        private const string GW_DAT = "Gw.dat";
        private const string GW_TEMPLATES = "\\Templates";

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

            if (!mFileCloset.ForceUnlock)
            {
                forceLaunchCheckBox.Checked = false;
            }

            if (IntPtr.Size == 8)
            {
                MessageBox.Show(@"Warning: You are running the 32-bit build" + 
                " of Guild Wars Multi-Launch under a 64-bit operating system. Functionality" +
                "will be affected! Use the 64-bit version.", 
                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool AddCopy(string pathToAdd)
        {
            return AddCopy(pathToAdd, DEFAULT_ARGUMENT);
        }

        private bool AddCopy(string pathToAdd, string pathArg)
        {
            bool success = false;

            if (!profilesListBox.Items.Contains(pathToAdd))
            {
                //add to file
                mFileCloset.AddProfile(pathToAdd, pathArg);

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

            string fileToUnlock = basePath + "\\" + GW_DAT;

            //get list of currently running system processes
            Process[] processList = Process.GetProcesses();

            foreach (Process i in processList)
            {
                //filter for guild wars ones
                if (i.ProcessName.Equals(GW_PROCESS_NAME, StringComparison.OrdinalIgnoreCase))
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
                if (i.ProcessName.Equals(GW_PROCESS_NAME, StringComparison.OrdinalIgnoreCase))
                {
                    if (HandleManager.KillHandle(i, MUTEX_MATCH_STRING, false))
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

        private static bool IsCopyRunning(string path)
        {
            //get list of currently running system processes
            Process[] processList = Process.GetProcesses();

            foreach (Process i in processList)
            {
                //does process name match?
                if (i.ProcessName.Equals(GW_PROCESS_NAME, StringComparison.OrdinalIgnoreCase))
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
                if (LaunchGame(path, mFileCloset.GetArgument(path), forced))
                {
                    //give time for gw to read value before it gets changed again.
                    System.Threading.Thread.Sleep(mFileCloset.RegistryCooldown);
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
                        ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            catch (Exception)
            {
                MessageBox.Show("Error launching: " + path + "!",
                    ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    string templateDir = sourceFolder + GW_TEMPLATES;
                    if (Directory.Exists(templateDir))
                    {
                        sourceFileList.AddRange(Directory.GetFiles(templateDir, "*.*", SearchOption.AllDirectories));
                    }

                    //translate to destination paths
                    destFileList = TranslateToDest(sourceFileList, sourceFolder, destFolder);

                    //translate string array to single string
                    string from = FileCopier.TranslateStringList(sourceFileList);
                    string to = FileCopier.TranslateStringList(destFileList);

                    //lets do the copying
                    success = FileCopier.CopyFiles(from, to);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error occurred while copying Guild Wars from "
                    + sourceFolder + " to " + destFolder + "!\n" + e.Message, ERROR_CAPTION,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                success = false;
            }

            return success;
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

        private List<string> TranslateToDest(List<string> sourceFileList, string sourceFolder, string destFolder)
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
                string pathToAdd = dlg.FileName;

                AddCopy(pathToAdd);
            }
        }

        private void ArgumentsTextBox_Leave(object sender, EventArgs e)
        {
            if (mSelectedPath != string.Empty)
            {
                mFileCloset.UpdateProfile(mSelectedPath, argumentsTextBox.Text);
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
                    mFileCloset.ForceUnlock = true;
                }
            }
            else
            {
                mFileCloset.ForceUnlock = false;
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
                    ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            string gwPath = selectFolderDlg.SelectedPath + "\\" + GW_FILENAME;
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
                    MessageBox.Show(t.Message, ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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
                makeCopyButton.Enabled = false;
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
                makeCopyButton.Enabled = true;
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
                makeCopyButton.Enabled = false;
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