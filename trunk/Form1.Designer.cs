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

namespace GWMultiLaunch
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.profilesListBox = new System.Windows.Forms.ListBox();
            this.mutexButton = new System.Windows.Forms.Button();
            this.argumentsTextBox = new System.Windows.Forms.TextBox();
            this.regButton = new System.Windows.Forms.Button();
            this.addCopyButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.forceLaunchCheckBox = new System.Windows.Forms.CheckBox();
            this.texmodButton = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.removeCopyButton = new System.Windows.Forms.Button();
            this.shortcutButton = new System.Windows.Forms.Button();
            this.makeCopyButton = new System.Windows.Forms.Button();
            this.launchButton = new System.Windows.Forms.Button();
            this.automodeButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // profilesListBox
            // 
            this.profilesListBox.AllowDrop = true;
            this.profilesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.profilesListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.profilesListBox.FormattingEnabled = true;
            this.profilesListBox.ItemHeight = 16;
            this.profilesListBox.Location = new System.Drawing.Point(12, 12);
            this.profilesListBox.Name = "profilesListBox";
            this.profilesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.profilesListBox.Size = new System.Drawing.Size(382, 180);
            this.profilesListBox.TabIndex = 0;
            this.profilesListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.profilesListBox_MouseDoubleClick);
            this.profilesListBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.ProfilesListBox_DragEnter);
            this.profilesListBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.ProfilesListBox_DragDrop);
            this.profilesListBox.SelectedIndexChanged += new System.EventHandler(this.ProfilesListBox_SelectedIndexChanged);
            // 
            // mutexButton
            // 
            this.mutexButton.Location = new System.Drawing.Point(116, 19);
            this.mutexButton.Name = "mutexButton";
            this.mutexButton.Size = new System.Drawing.Size(104, 40);
            this.mutexButton.TabIndex = 1;
            this.mutexButton.Text = "Clear Mutex";
            this.mutexButton.UseVisualStyleBackColor = true;
            this.mutexButton.Click += new System.EventHandler(this.MutexButton_Click);
            // 
            // argumentsTextBox
            // 
            this.argumentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.argumentsTextBox.Location = new System.Drawing.Point(114, 204);
            this.argumentsTextBox.Name = "argumentsTextBox";
            this.argumentsTextBox.Size = new System.Drawing.Size(280, 20);
            this.argumentsTextBox.TabIndex = 3;
            this.argumentsTextBox.Leave += new System.EventHandler(this.ArgumentsTextBox_Leave);
            // 
            // regButton
            // 
            this.regButton.Enabled = false;
            this.regButton.Location = new System.Drawing.Point(6, 19);
            this.regButton.Name = "regButton";
            this.regButton.Size = new System.Drawing.Size(104, 40);
            this.regButton.TabIndex = 4;
            this.regButton.Text = "Set Registry Path";
            this.regButton.UseVisualStyleBackColor = true;
            this.regButton.Click += new System.EventHandler(this.RegButton_Click);
            // 
            // addCopyButton
            // 
            this.addCopyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addCopyButton.BackColor = System.Drawing.SystemColors.Control;
            this.addCopyButton.Location = new System.Drawing.Point(411, 12);
            this.addCopyButton.Name = "addCopyButton";
            this.addCopyButton.Size = new System.Drawing.Size(124, 40);
            this.addCopyButton.TabIndex = 6;
            this.addCopyButton.Text = "Add";
            this.toolTip1.SetToolTip(this.addCopyButton, "Add a copy of Guild Wars to the list.");
            this.addCopyButton.UseVisualStyleBackColor = false;
            this.addCopyButton.Click += new System.EventHandler(this.AddCopyButton_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 207);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Launch Arguments";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.forceLaunchCheckBox);
            this.groupBox1.Controls.Add(this.texmodButton);
            this.groupBox1.Controls.Add(this.regButton);
            this.groupBox1.Controls.Add(this.mutexButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 236);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(382, 90);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Expert Controls";
            // 
            // forceLaunchCheckBox
            // 
            this.forceLaunchCheckBox.AutoSize = true;
            this.forceLaunchCheckBox.Checked = true;
            this.forceLaunchCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.forceLaunchCheckBox.Location = new System.Drawing.Point(6, 65);
            this.forceLaunchCheckBox.Name = "forceLaunchCheckBox";
            this.forceLaunchCheckBox.Size = new System.Drawing.Size(372, 17);
            this.forceLaunchCheckBox.TabIndex = 6;
            this.forceLaunchCheckBox.Text = "Force gw.dat unlock. Enable multi-launching of same copy. (experimental)";
            this.forceLaunchCheckBox.UseVisualStyleBackColor = true;
            this.forceLaunchCheckBox.CheckedChanged += new System.EventHandler(this.ForceLaunchCheckBox_CheckedChanged);
            // 
            // texmodButton
            // 
            this.texmodButton.Location = new System.Drawing.Point(226, 19);
            this.texmodButton.Name = "texmodButton";
            this.texmodButton.Size = new System.Drawing.Size(104, 40);
            this.texmodButton.TabIndex = 5;
            this.texmodButton.Text = "Open Texmod";
            this.texmodButton.UseVisualStyleBackColor = true;
            this.texmodButton.Click += new System.EventHandler(this.TexmodButton_Click);
            // 
            // removeCopyButton
            // 
            this.removeCopyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeCopyButton.BackColor = System.Drawing.SystemColors.Control;
            this.removeCopyButton.Enabled = false;
            this.removeCopyButton.Location = new System.Drawing.Point(411, 58);
            this.removeCopyButton.Name = "removeCopyButton";
            this.removeCopyButton.Size = new System.Drawing.Size(124, 40);
            this.removeCopyButton.TabIndex = 11;
            this.removeCopyButton.Text = "Remove";
            this.toolTip1.SetToolTip(this.removeCopyButton, "Remove selected copy from list.");
            this.removeCopyButton.UseVisualStyleBackColor = false;
            this.removeCopyButton.Click += new System.EventHandler(this.RemoveCopyButton_Click);
            // 
            // shortcutButton
            // 
            this.shortcutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.shortcutButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.shortcutButton.Enabled = false;
            this.shortcutButton.Location = new System.Drawing.Point(411, 240);
            this.shortcutButton.Name = "shortcutButton";
            this.shortcutButton.Size = new System.Drawing.Size(124, 40);
            this.shortcutButton.TabIndex = 12;
            this.shortcutButton.Text = "Make Shortcut";
            this.toolTip1.SetToolTip(this.shortcutButton, "Make multi-launch enabled desktop shortcut.");
            this.shortcutButton.UseVisualStyleBackColor = false;
            this.shortcutButton.Click += new System.EventHandler(this.ShortcutButton_Click);
            // 
            // makeCopyButton
            // 
            this.makeCopyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.makeCopyButton.BackColor = System.Drawing.SystemColors.Control;
            this.makeCopyButton.Enabled = false;
            this.makeCopyButton.Location = new System.Drawing.Point(411, 104);
            this.makeCopyButton.Name = "makeCopyButton";
            this.makeCopyButton.Size = new System.Drawing.Size(124, 40);
            this.makeCopyButton.TabIndex = 14;
            this.makeCopyButton.Text = "Make Copy";
            this.toolTip1.SetToolTip(this.makeCopyButton, "Make a new copy.");
            this.makeCopyButton.UseVisualStyleBackColor = false;
            this.makeCopyButton.Click += new System.EventHandler(this.MakeCopyButton_Click);
            // 
            // launchButton
            // 
            this.launchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.launchButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.launchButton.Enabled = false;
            this.launchButton.Location = new System.Drawing.Point(411, 150);
            this.launchButton.Name = "launchButton";
            this.launchButton.Size = new System.Drawing.Size(124, 80);
            this.launchButton.TabIndex = 13;
            this.launchButton.Text = "Launch";
            this.launchButton.UseVisualStyleBackColor = false;
            this.launchButton.Click += new System.EventHandler(this.LaunchButton_Click);
            // 
            // automodeButton
            // 
            this.automodeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.automodeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.automodeButton.Location = new System.Drawing.Point(411, 286);
            this.automodeButton.Name = "automodeButton";
            this.automodeButton.Size = new System.Drawing.Size(124, 40);
            this.automodeButton.TabIndex = 15;
            this.automodeButton.Text = "Make Master Shortcut";
            this.toolTip1.SetToolTip(this.automodeButton, "Make multi-launch enabled desktop shortcut.");
            this.automodeButton.UseVisualStyleBackColor = false;
            this.automodeButton.Click += new System.EventHandler(this.AutomodeButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 338);
            this.Controls.Add(this.automodeButton);
            this.Controls.Add(this.makeCopyButton);
            this.Controls.Add(this.launchButton);
            this.Controls.Add(this.shortcutButton);
            this.Controls.Add(this.removeCopyButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.addCopyButton);
            this.Controls.Add(this.argumentsTextBox);
            this.Controls.Add(this.profilesListBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(555, 365);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Guild Wars Multi-Launch (v0.6 beta)";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox profilesListBox;
        private System.Windows.Forms.Button mutexButton;
        private System.Windows.Forms.TextBox argumentsTextBox;
        private System.Windows.Forms.Button regButton;
        private System.Windows.Forms.Button addCopyButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button removeCopyButton;
        private System.Windows.Forms.Button shortcutButton;
        private System.Windows.Forms.Button launchButton;
        private System.Windows.Forms.Button texmodButton;
        private System.Windows.Forms.Button makeCopyButton;
        private System.Windows.Forms.CheckBox forceLaunchCheckBox;
        private System.Windows.Forms.Button automodeButton;
    }
}

