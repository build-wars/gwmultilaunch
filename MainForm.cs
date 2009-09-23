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

//comment out for 64 bit builds
#define BUILD32

using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GWMultiLaunch
{
    public partial class MainForm : Form
    {
        private string mSelectedPath = string.Empty;

        public MainForm()
        {
            InitializeComponent();
            InitUI();
            PlatformAlert();
        }

        private void InitUI()
        {
            //Populate listbox with copies
            foreach (KeyValuePair<string, string> i in Program.settings.Profiles)
            {
                profilesListBox.Items.Add(i.Key);
            }

            //Select copies
            foreach (int i in Program.settings.SelectedIndices)
            {
                profilesListBox.SelectedIndices.Add(i);
            }

            //Avoids springing up alert box with setting it to checked state
            if (Program.settings.ForceUnlock == false)
            {
                forceUnlockCheckBox.CheckBoxControl.Checked = false;
            }

            UpdateArgumentsTextBox();
            UpdateButtonStates();
        }

        private void PlatformAlert()
        {
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

        #region Button Click Events

        private void addButton_Click(object sender, EventArgs e)
        {
            AddCopy();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            RemoveCopies();
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            MakeCopy();
        }

        private void launchButton_Click(object sender, EventArgs e)
        {
            UpdateSelectedPath();
            LaunchCopies();
        }

        private void shortcutButton_Click(object sender, EventArgs e)
        {
            CreateShortcut(GetSelectedInstalls());
        }

        private void masterShortcutButton_Click(object sender, EventArgs e)
        {
            CreateMasterShortcut();
        }

        private void setPathButton_Click(object sender, EventArgs e)
        {
            SetGWRegPath(GetSelectedInstall());
        }

        private void killMutexButton_Click(object sender, EventArgs e)
        {
            HandleManager.ClearMutex();
        }

        private void startTexModButton_Click(object sender, EventArgs e)
        {
            SetGWRegPath(GetSelectedInstall());
            HandleManager.ClearMutex();
            StartTexMod();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region Other Events

        private void profilesListBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] droppedFilenames = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            AddCopies(droppedFilenames);
        }

        private void profilesListBox_DragEnter(object sender, DragEventArgs e)
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
                LaunchGame(clickedIndex);
            }
        }

        private void profilesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSelectedPath();
            UpdateArgumentsTextBox();
            UpdateButtonStates();
        }

        private void argumentsTextBox_Leave(object sender, EventArgs e)
        {
            Program.settings.UpdateProfile(mSelectedPath, argumentsTextBox.Text);
        }

        private void forceUnlockCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            VerifyForceUnlock();
        }

        #endregion

        #region Helpers

        private int AddCopies(string[] pathsToAdd)
        {
            int copiesAdded = 0;

            foreach (string pathToAdd in pathsToAdd)
            {
                if (AddCopy(pathToAdd))
                    copiesAdded++;
            }

            return copiesAdded;
        }

        private bool AddCopy()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Guild Wars (*.exe, *.lnk)|*.exe;*.lnk";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return AddCopy(dlg.FileName);
            }

            return false;
        }

        private bool AddCopy(string pathToAdd)
        {
            string gwPath = pathToAdd;
            string arguments = string.Empty;

            string fileExt = Path.GetExtension(pathToAdd);
            if (fileExt.Equals(".lnk", StringComparison.OrdinalIgnoreCase))
            {
                gwPath = ShortcutCreator.GetShortcutTarget(pathToAdd);
                arguments = ShortcutCreator.GetShortcutArguments(pathToAdd).Trim();
            }

            if (arguments == string.Empty)
            {
                return AddCopy(gwPath, Program.DEFAULT_ARGUMENT);
            }
            
            return AddCopy(gwPath, arguments);
        }

        private bool AddCopy(string gwPath, string arguments)
        {
            bool success = false;

            if (!profilesListBox.Items.Contains(gwPath))
            {
                //add to file
                Program.settings.AddProfile(gwPath, arguments);

                //add to list
                profilesListBox.Items.Add(gwPath);

                //deselect all
                profilesListBox.SelectedIndex = -1;

                success = true;
            }

            return success;
        }

        private void RemoveCopies()
        {
            if (ConfirmRemoval() == false)
            {
                return;
            }

            string[] selectedInstalls = GetSelectedInstalls();

            foreach (string selectedInstall in selectedInstalls)
            {
                //remove from file
                Program.settings.RemoveProfile(selectedInstall);

                //remove from list
                mSelectedPath = string.Empty;
                profilesListBox.Items.Remove(selectedInstall);
            }
        }

        private bool ConfirmRemoval()
        {
            DialogResult removeCopies =
                MessageBox.Show("Are you sure you want to remove selected copies?", "Remove Copies?",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (removeCopies == DialogResult.Yes)
            {
                return true;
            }

            return false;
        }

        private void MakeCopy()
        {
            string newCopy = MakeCopy(GetSelectedInstall());

            if (File.Exists(newCopy))
            {
                AddCopy(newCopy);
            }
        }

        private void VerifyForceUnlock()
        {
            if (forceUnlockCheckBox.CheckBoxControl.Checked)
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
                    forceUnlockCheckBox.CheckBoxControl.Checked = false;
                }
                else
                {
                    Program.settings.ForceUnlock = true;
                }
            }
            else
            {
                Program.settings.ForceUnlock = false;
            }
        }

        private void LaunchCopies()
        {
            foreach (string copy in GetSelectedInstalls())
            {
                LaunchGame(copy);
            }
        }

        private void LaunchGame(int index)
        {
            try
            {
                string gwPath = profilesListBox.Items[index].ToString();
                LaunchGame(gwPath);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Program.ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LaunchGame(string gwPath)
        {
            //check if the install exists
            if (!File.Exists(gwPath))
            {
                MessageBox.Show("The path: " + gwPath + " does not exist!");

            }
            else
            {
                bool forced = forceUnlockCheckBox.CheckBoxControl.Checked;
                if (forced)
                {
                    HandleManager.ClearDatLock(Directory.GetParent(gwPath).FullName);
                }

                //attempt to launch
                if (LaunchGame(gwPath, Program.settings.GetArguments(gwPath), forced))
                {
                    //give time for gw to read path before it gets changed again.
                    System.Threading.Thread.Sleep(Program.settings.RegistryCooldown);
                }
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

            return string.Empty;
        }

        private string[] GetSelectedInstalls()
        {
            string[] selectedItems = new string[profilesListBox.SelectedItems.Count];
            profilesListBox.SelectedItems.CopyTo(selectedItems, 0);
            return selectedItems;
        }

        private void UpdateSelectedPath()
        {
            //update arguments of last selected copy with arguments in textbox
            Program.settings.UpdateProfile(mSelectedPath, argumentsTextBox.Text);

            if (profilesListBox.SelectedIndices.Count == 1)
            {
                //one item selected
                mSelectedPath = profilesListBox.SelectedItem.ToString();
            }
            else
            {
                //multiple items or nothing selected
                mSelectedPath = string.Empty;
            }

            //update selected indices
            int[] indicesArr = new int[profilesListBox.SelectedIndices.Count];
            profilesListBox.SelectedIndices.CopyTo(indicesArr, 0);
            Program.settings.SelectedIndices = indicesArr;
        }

        private void UpdateArgumentsTextBox()
        {
            if (profilesListBox.SelectedIndices.Count == 1)
            {
                //one item selected
                argumentsTextBox.Enabled = true;
                argumentsTextBox.Text = Program.settings.GetArguments(mSelectedPath);
            }
            else
            {
                //multiple items or nothing selected
                argumentsTextBox.Enabled = false;
                argumentsTextBox.Text = string.Empty;
            }
        }

        private void UpdateButtonStates()
        {
            if (profilesListBox.SelectedIndices.Count > 1)
            {
                removeButton.Enabled = true;
                copyButton.Enabled = false;
                launchButton.Enabled = true;
                shortcutButton.Enabled = true;
                setPathButton.Enabled = false;
                startTexModButton.Enabled = false;
            }
            else if (profilesListBox.SelectedIndices.Count == 1)
            {
                removeButton.Enabled = true;
                copyButton.Enabled = true;
                launchButton.Enabled = true;
                shortcutButton.Enabled = true;
                setPathButton.Enabled = true;
                startTexModButton.Enabled = true;
            }
            else
            {
                removeButton.Enabled = false;
                copyButton.Enabled = false;
                launchButton.Enabled = false;
                shortcutButton.Enabled = false;
                setPathButton.Enabled = false;
                startTexModButton.Enabled = false;
            }
        }
        
        #endregion

    }
}