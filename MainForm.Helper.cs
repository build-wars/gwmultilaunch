using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;

namespace GWMultiLaunch
{
    public partial class MainForm : Form
    {
        #region Make Copy

        private static string MakeCopy(string gwPath)
        {
            if (gwPath == String.Empty)
            {
                MessageBox.Show("No installation selected!");
                return string.Empty;
            }

            if (!File.Exists(gwPath))
            {
                MessageBox.Show("Can not make a copy. The select install: " + gwPath + " does not exist!",
                    Program.ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }

            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            folderDlg.Description = "Select an empty folder to copy to. (Hint: Click \"Make New Folder\" button.)";
            folderDlg.RootFolder = Environment.SpecialFolder.MyComputer;
            folderDlg.SelectedPath = Directory.GetParent(gwPath).Parent.FullName;

            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                DialogResult confirm = MessageBox.Show("Are you sure you want to make a copy of Guild Wars at: " +
                    folderDlg.SelectedPath + "?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    bool copySuccess = CopyGWFolder(Directory.GetParent(gwPath).FullName, folderDlg.SelectedPath);
                    if (copySuccess)
                    {
                        return (folderDlg.SelectedPath + "\\" + Program.GW_FILENAME);
                    }
                }
            }

            return string.Empty;
        }

        private static bool CopyGWFolder(string sourceFolder, string destFolder)
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

        private static List<string> GetDestFileList(List<string> sourceFileList, string sourceFolder, string destFolder)
        {
            List<string> destFileList = new List<string>();

            foreach (string filename in sourceFileList)
            {
                destFileList.Add(filename.Replace(sourceFolder, destFolder));
            }

            return destFileList;
        }

        #endregion

        #region Shortcuts

        private static int CreateShortcut(string[] gwPaths)
        {
            int nShortcutsCreated = 0;
            string gwArgs = string.Empty;

            foreach (string gwPath in gwPaths)
            {
                gwArgs = Program.settings.GetArguments(gwPath);

                if (ShortcutCreator.CreateSingleLaunchShortcut(gwPath, gwArgs))
                {
                    nShortcutsCreated++;
                }
            }

            MessageBox.Show(nShortcutsCreated.ToString() + " Desktop shortcuts were created!", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            return nShortcutsCreated;
        }

        private static bool CreateMasterShortcut()
        {
            if (ShortcutCreator.CreateMasterShortcut())
            {
                MessageBox.Show("Master shortcut created!",
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }

            return false;
        }

        #endregion

        #region TexMod

        private static void StartTexMod()
        {
            string texModPath = Program.settings.TexModPath;
            if (File.Exists(texModPath) == false)
            {
                texModPath = string.Empty;
            }

            if (texModPath == string.Empty)
            {
                texModPath = FindTexMod();
            }

            if (texModPath == string.Empty)
            {
                texModPath = PromptForTexMod();
            }

            if (texModPath.Length > 0)
            {
                if (StartProgram(texModPath))
                {
                    Program.settings.TexModPath = texModPath;
                }
                else
                {
                    Program.settings.TexModPath = string.Empty;
                }
            }
        }

        private static string FindTexMod()
        {
            //use Guild Wars folder as starting point to start search
            string gwPath = GetGWRegPath();
            string texModPath = string.Empty;

            if (File.Exists(gwPath))
            {
                string directory = Directory.GetParent(gwPath).FullName;

                try
                {
                    string[] texmodFiles = Directory.GetFiles(directory, 
                        Program.TM_FILENAME, SearchOption.AllDirectories);

                    if (texmodFiles.Length > 0)
                    {
                        texModPath = texmodFiles[0];    //use first match
                    }
                }
                catch (Exception) { }
            }

            return texModPath;
        }

        private static string PromptForTexMod()
        {
            string texModPath = string.Empty;

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Locate TexMod";
            dlg.Filter = "TexMod Executable (*.exe)|*.exe";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                texModPath = dlg.FileName;
            }

            return texModPath;
        }

        #endregion

        #region Utils

        public static bool LaunchGame(string gwPath, string args, bool forced)
        {
            bool success = false;

            if (!forced)
            {
                //check to see if this copy is already started
                if (IsCopyRunning(gwPath))
                {
                    MessageBox.Show(gwPath + " is already running, please launch a different copy.",
                        Program.ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return success;
                }
            }

            Process gw = new Process();
            gw.StartInfo.FileName = gwPath;
            gw.StartInfo.Arguments = args;
            gw.StartInfo.WorkingDirectory = Directory.GetParent(gwPath).FullName;
            gw.StartInfo.UseShellExecute = true;

            try
            {
                //set new gw path
                SetGWRegPath(gwPath);

                //clear mutex to allow for another gw launch
                HandleManager.ClearMutex();

                //attempt to start gw process
                gw.Start();

                success = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error launching: " + gwPath + "!\n" + e.Message,
                    Program.ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return success;
        }

        public static bool IsCopyRunning(string gwPath)
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
                        if (processPath.Equals(gwPath, StringComparison.OrdinalIgnoreCase))
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

        public static bool StartProgram(string path)
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

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
