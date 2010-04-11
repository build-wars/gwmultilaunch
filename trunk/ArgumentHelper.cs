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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace GWMultiLaunch
{
    public partial class ArgumentsWizard : Form
    {
        private string mArguments;
        private Dictionary<string, bool> mSwitches;
        private Dictionary<string, string> mTooltips;

        public string Arguments
        {
            get
            {
                return mArguments;
            }
            set
            {
                mArguments = value;
            }
        }

        public ArgumentsWizard()
        {
            InitializeComponent();

            linkLabel1.Links.Add(0, linkLabel1.Text.Length, 
                "http://wiki.guildwars.com/wiki/Command_line_arguments");

            mSwitches = new Dictionary<string, bool>();
            mSwitches.Add("-bmp", false);
            mSwitches.Add("-character", true);
            mSwitches.Add("-diag", false);
            mSwitches.Add("-dsound", false);
            mSwitches.Add("-dx8", false);
            mSwitches.Add("-email", true);
            mSwitches.Add("-fps", true);
            mSwitches.Add("-image", false);
            mSwitches.Add("-mce", false);
            mSwitches.Add("-mute", false);
            mSwitches.Add("-noshaders", false);
            mSwitches.Add("-nosound", false);
            mSwitches.Add("-noui", false);
            mSwitches.Add("-password", true);
            mSwitches.Add("-perf", false);
            mSwitches.Add("-repair", false);
            mSwitches.Add("-sndasio", false);
            mSwitches.Add("-sndwinmm", false);
            mSwitches.Add("-uninstall", false);
            mSwitches.Add("-update", false);
            mSwitches.Add("-windowed", false);

            mTooltips = new Dictionary<string, string>();
            mTooltips.Add("-bmp", "Save screenshots as .BMP files.");
            mTooltips.Add("-character", "Specify character name for login.");
            mTooltips.Add("-diag", "Create diagnostics file.");
            mTooltips.Add("-dsound", "Force old DirectSound mixer.");
            mTooltips.Add("-dx8", "Force DirectX 8 compatibility.");
            mTooltips.Add("-email", "Specify email for login.");
            mTooltips.Add("-fps", "Specify max frame rate.");
            mTooltips.Add("-image", "Force download of all updates and missing files.");
            mTooltips.Add("-mce", "Enable Windows Media Center compatibility.");
            mTooltips.Add("-mute", "Disable audio output.");
            mTooltips.Add("-noshaders", "Disable usage of shaders for graphics.");
            mTooltips.Add("-nosound", "Disable audio system.");
            mTooltips.Add("-noui", "Hide user interface.");
            mTooltips.Add("-password", "Specify password for login.");
            mTooltips.Add("-perf", "Show performance stats.");
            mTooltips.Add("-repair", "Fix GW.dat file.");
            mTooltips.Add("-sndasio", "Use ASIO driver in software mode.");
            mTooltips.Add("-sndwinmm", "Use WM Audio driver in software mode.");
            mTooltips.Add("-uninstall", "Uninstall Guild Wars.");
            mTooltips.Add("-update", "Prompt for install disc to update GW.dat");
            mTooltips.Add("-windowed", "Run game in windowed mode.");
        }

        private void ArgumentHelper_Load(object sender, EventArgs e)
        {
            FillArguments(mArguments);
            FillDefaultArguments();
            FillToolTips();
        }

        private void FillArguments(string arguments)
        {
            Regex rgx = new Regex(@"(?<match>[^""\s]+)|\""(?<match>[^""]*)""");
            MatchCollection matches = rgx.Matches(arguments);
            int iRow = -1;
            string sSwitch = string.Empty;
            bool enableOptionColumn = false;

            foreach (Match m in matches)
            {
                if (m.Value[0] == '-')
                {
                    object[] rowData = new object[] { true, m.Value };
                    iRow = gwSwitchesGridView.Rows.Add(rowData);
                    sSwitch = m.Value;

                    //disable options column?
                    if (mSwitches.TryGetValue(sSwitch, out enableOptionColumn))
                    {
                        if (enableOptionColumn == false)
                        {
                            gwSwitchesGridView.Rows[iRow].Cells["optionColumn"].ReadOnly = true;
                            gwSwitchesGridView.Rows[iRow].Cells["optionColumn"].Style.BackColor =
                                System.Drawing.SystemColors.ControlDark;
                        }
                    }
                    else
                    {
                        enableOptionColumn = true;
                    }
                    
                }
                else if (enableOptionColumn && iRow >= 0)
                {
                    if (gwSwitchesGridView.Rows[iRow].Cells["optionColumn"].Value != null)
                    {
                        gwSwitchesGridView.Rows[iRow].Cells["optionColumn"].Value += " " + m.Value;
                    }
                    else
                    {
                        gwSwitchesGridView.Rows[iRow].Cells["optionColumn"].Value = m.Value;
                    }
                    
                }
            }
        }

        private void FillDefaultArguments()
        {
            foreach (KeyValuePair<string, bool> i in mSwitches)
            {
                if (FindSwitch(i.Key) == false)
                {
                    object[] rowData = new object[] { false, i.Key };
                    int iRow = gwSwitchesGridView.Rows.Add(rowData);

                    //disable 3rd column
                    if (i.Value == false)
                    {
                        gwSwitchesGridView.Rows[iRow].Cells["optionColumn"].ReadOnly = true;
                        gwSwitchesGridView.Rows[iRow].Cells["optionColumn"].Style.BackColor = 
                            System.Drawing.SystemColors.ControlDark;
                    }
                }
            }
        }

        private void FillToolTips()
        {
            foreach (DataGridViewRow r in gwSwitchesGridView.Rows)
            {
                string gridVal = r.Cells["switchColumn"].Value.ToString().Trim();
                string sTooltip = string.Empty;
                
                //disable options column?
                if (mTooltips.TryGetValue(gridVal, out sTooltip))
                {
                    r.Cells["switchColumn"].ToolTipText = sTooltip;

                    if (r.Cells["optionColumn"].ReadOnly == false)
                    {
                        r.Cells["optionColumn"].ToolTipText = sTooltip;
                    }
                }
            }
        }

        private bool FindSwitch(string sValue)
        {
            bool foundSwitch = false;

            foreach (DataGridViewRow r in gwSwitchesGridView.Rows)
            {
                string gridVal = r.Cells["switchColumn"].Value.ToString().Trim();
                if (gridVal == sValue)
                {
                    foundSwitch = true;
                    break;
                }
            }

            return foundSwitch;
        }

        private string ListToArgString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataGridViewRow r in gwSwitchesGridView.Rows)
            {
                bool rEnabled = (bool)r.Cells["enableColumn"].Value;

                if (rEnabled)
                {
                    string rSwitch = r.Cells["switchColumn"].Value.ToString().Trim();
                    sb.Append(rSwitch);
                    sb.Append(' ');

                    string rOption = (string)r.Cells["optionColumn"].Value;
                    if (rOption != null && rOption != string.Empty)
                    {
                        if (rOption[0] != '"' && rOption.Contains(" "))
                        {
                            sb.Append('"');
                        }
                        
                        sb.Append(rOption);

                        if (rOption[rOption.Length - 1] != '"' && rOption.Contains(" "))
                        {
                            sb.Append('"');
                        }

                        sb.Append(' ');
                    }
                }
            }

            //we don't want last space
            return sb.ToString(0, Math.Max(0, sb.Length - 1));
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            mArguments = ListToArgString();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void gwSwitchesGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 2)
            {
                string cellValue = (string)gwSwitchesGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (cellValue != null && cellValue != string.Empty)
                {
                    gwSwitchesGridView.Rows[e.RowIndex].Cells["enableColumn"].Value = true;
                }
                else
                {
                    gwSwitchesGridView.Rows[e.RowIndex].Cells["enableColumn"].Value = false;
                }
            }
        }

        private void gwSwitchesGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                gwSwitchesGridView.Rows[e.RowIndex].Cells["enableColumn"].Value =
                    !(bool)gwSwitchesGridView.Rows[e.RowIndex].Cells["enableColumn"].Value;
            }
        }

        private void gwSwitchesGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (gwSwitchesGridView.IsCurrentCellInEditMode == false &&
                e.KeyCode == Keys.Delete)
            {
                foreach (DataGridViewCell c in gwSwitchesGridView.SelectedCells)
                {
                    if (c.ColumnIndex == 2)
                    {
                        c.Value = string.Empty;
                    }
                }
            }
        }
 
    }
}