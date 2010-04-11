namespace GWMultiLaunch
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.basicControlsToolStrip = new System.Windows.Forms.ToolStrip();
            this.launchButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.addButton = new System.Windows.Forms.ToolStripButton();
            this.removeButton = new System.Windows.Forms.ToolStripButton();
            this.copyButton = new System.Windows.Forms.ToolStripButton();
            this.masterShortcutButton = new System.Windows.Forms.ToolStripButton();
            this.shortcutButton = new System.Windows.Forms.ToolStripButton();
            this.expertControlsToolStrip = new System.Windows.Forms.ToolStrip();
            this.setPathButton = new System.Windows.Forms.ToolStripButton();
            this.killMutexButton = new System.Windows.Forms.ToolStripButton();
            this.startTexModButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitButton = new System.Windows.Forms.ToolStripButton();
            this.profilesListBox = new System.Windows.Forms.ListBox();
            this.editArgButton = new System.Windows.Forms.Button();
            this.auxToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.argumentsTextBox = new GWMultiLaunch.SelfLabeledTextBox();
            this.forceUnlockCheckBox = new GWMultiLaunch.ToolStripCheckBox();
            this.basicControlsToolStrip.SuspendLayout();
            this.expertControlsToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // basicControlsToolStrip
            // 
            this.basicControlsToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.basicControlsToolStrip.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.basicControlsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.launchButton,
            this.toolStripSeparator2,
            this.addButton,
            this.removeButton,
            this.copyButton,
            this.masterShortcutButton,
            this.shortcutButton});
            this.basicControlsToolStrip.Location = new System.Drawing.Point(0, 0);
            this.basicControlsToolStrip.Name = "basicControlsToolStrip";
            this.basicControlsToolStrip.Padding = new System.Windows.Forms.Padding(2, 0, 5, 0);
            this.basicControlsToolStrip.Size = new System.Drawing.Size(392, 55);
            this.basicControlsToolStrip.TabIndex = 0;
            this.basicControlsToolStrip.Text = "Basic Controls";
            // 
            // launchButton
            // 
            this.launchButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.launchButton.Image = ((System.Drawing.Image)(resources.GetObject("launchButton.Image")));
            this.launchButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.launchButton.Name = "launchButton";
            this.launchButton.Padding = new System.Windows.Forms.Padding(20, 0, 10, 0);
            this.launchButton.Size = new System.Drawing.Size(82, 52);
            this.launchButton.Text = "Launch";
            this.launchButton.Click += new System.EventHandler(this.launchButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 55);
            // 
            // addButton
            // 
            this.addButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addButton.Image = ((System.Drawing.Image)(resources.GetObject("addButton.Image")));
            this.addButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(52, 52);
            this.addButton.Text = "Add";
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removeButton.Image = ((System.Drawing.Image)(resources.GetObject("removeButton.Image")));
            this.removeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(52, 52);
            this.removeButton.Text = "Remove";
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // copyButton
            // 
            this.copyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copyButton.Image = ((System.Drawing.Image)(resources.GetObject("copyButton.Image")));
            this.copyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(52, 52);
            this.copyButton.Text = "Make Copy";
            this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
            // 
            // masterShortcutButton
            // 
            this.masterShortcutButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.masterShortcutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.masterShortcutButton.Image = ((System.Drawing.Image)(resources.GetObject("masterShortcutButton.Image")));
            this.masterShortcutButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.masterShortcutButton.Name = "masterShortcutButton";
            this.masterShortcutButton.Size = new System.Drawing.Size(52, 52);
            this.masterShortcutButton.Text = "Create Master Shortcut";
            this.masterShortcutButton.Click += new System.EventHandler(this.masterShortcutButton_Click);
            // 
            // shortcutButton
            // 
            this.shortcutButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.shortcutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.shortcutButton.Image = ((System.Drawing.Image)(resources.GetObject("shortcutButton.Image")));
            this.shortcutButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.shortcutButton.Name = "shortcutButton";
            this.shortcutButton.Size = new System.Drawing.Size(52, 52);
            this.shortcutButton.Text = "Create Shortcut";
            this.shortcutButton.Click += new System.EventHandler(this.shortcutButton_Click);
            // 
            // expertControlsToolStrip
            // 
            this.expertControlsToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.expertControlsToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.expertControlsToolStrip.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.expertControlsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setPathButton,
            this.killMutexButton,
            this.startTexModButton,
            this.toolStripSeparator1,
            this.exitButton,
            this.forceUnlockCheckBox});
            this.expertControlsToolStrip.Location = new System.Drawing.Point(0, 230);
            this.expertControlsToolStrip.Name = "expertControlsToolStrip";
            this.expertControlsToolStrip.Padding = new System.Windows.Forms.Padding(2, 0, 5, 0);
            this.expertControlsToolStrip.Size = new System.Drawing.Size(392, 55);
            this.expertControlsToolStrip.TabIndex = 3;
            this.expertControlsToolStrip.Text = "Expert Controls";
            // 
            // setPathButton
            // 
            this.setPathButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.setPathButton.Image = ((System.Drawing.Image)(resources.GetObject("setPathButton.Image")));
            this.setPathButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.setPathButton.Name = "setPathButton";
            this.setPathButton.Size = new System.Drawing.Size(52, 52);
            this.setPathButton.Text = "Set Path";
            this.setPathButton.Click += new System.EventHandler(this.setPathButton_Click);
            // 
            // killMutexButton
            // 
            this.killMutexButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.killMutexButton.Image = ((System.Drawing.Image)(resources.GetObject("killMutexButton.Image")));
            this.killMutexButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.killMutexButton.Name = "killMutexButton";
            this.killMutexButton.Size = new System.Drawing.Size(52, 52);
            this.killMutexButton.Text = "Kill Mutex";
            this.killMutexButton.Click += new System.EventHandler(this.killMutexButton_Click);
            // 
            // startTexModButton
            // 
            this.startTexModButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.startTexModButton.Image = ((System.Drawing.Image)(resources.GetObject("startTexModButton.Image")));
            this.startTexModButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.startTexModButton.Name = "startTexModButton";
            this.startTexModButton.Size = new System.Drawing.Size(52, 52);
            this.startTexModButton.Text = "Start TexMod";
            this.startTexModButton.Click += new System.EventHandler(this.startTexModButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 55);
            // 
            // exitButton
            // 
            this.exitButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.exitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.exitButton.Image = ((System.Drawing.Image)(resources.GetObject("exitButton.Image")));
            this.exitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(52, 52);
            this.exitButton.Text = "Exit";
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // profilesListBox
            // 
            this.profilesListBox.AllowDrop = true;
            this.profilesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.profilesListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.profilesListBox.FormattingEnabled = true;
            this.profilesListBox.IntegralHeight = false;
            this.profilesListBox.ItemHeight = 20;
            this.profilesListBox.Location = new System.Drawing.Point(0, 58);
            this.profilesListBox.Name = "profilesListBox";
            this.profilesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.profilesListBox.Size = new System.Drawing.Size(392, 144);
            this.profilesListBox.TabIndex = 1;
            this.profilesListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.profilesListBox_MouseDoubleClick);
            this.profilesListBox.SelectedIndexChanged += new System.EventHandler(this.profilesListBox_SelectedIndexChanged);
            this.profilesListBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.profilesListBox_DragDrop);
            this.profilesListBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.profilesListBox_DragEnter);
            // 
            // editArgButton
            // 
            this.editArgButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.editArgButton.FlatAppearance.BorderSize = 0;
            this.editArgButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.editArgButton.Image = ((System.Drawing.Image)(resources.GetObject("editArgButton.Image")));
            this.editArgButton.Location = new System.Drawing.Point(366, 203);
            this.editArgButton.Name = "editArgButton";
            this.editArgButton.Size = new System.Drawing.Size(25, 25);
            this.editArgButton.TabIndex = 4;
            this.auxToolTip.SetToolTip(this.editArgButton, "Open Arguments Wizard");
            this.editArgButton.UseVisualStyleBackColor = true;
            this.editArgButton.Click += new System.EventHandler(this.editArgButton_Click);
            // 
            // argumentsTextBox
            // 
            this.argumentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.argumentsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.argumentsTextBox.LabelColor = System.Drawing.SystemColors.GrayText;
            this.argumentsTextBox.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.argumentsTextBox.LabelText = "Launch Arguments";
            this.argumentsTextBox.Location = new System.Drawing.Point(0, 203);
            this.argumentsTextBox.Name = "argumentsTextBox";
            this.argumentsTextBox.Size = new System.Drawing.Size(365, 26);
            this.argumentsTextBox.TabIndex = 2;
            this.argumentsTextBox.TextChanged += new System.EventHandler(this.argumentsTextBox_TextChanged);
            // 
            // forceUnlockCheckBox
            // 
            this.forceUnlockCheckBox.BackColor = System.Drawing.Color.Transparent;
            // 
            // forceUnlockCheckBox
            // 
            this.forceUnlockCheckBox.CheckBoxControl.AccessibleName = "forceUnlockCheckBox";
            this.forceUnlockCheckBox.CheckBoxControl.BackColor = System.Drawing.Color.Transparent;
            this.forceUnlockCheckBox.CheckBoxControl.Checked = true;
            this.forceUnlockCheckBox.CheckBoxControl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.forceUnlockCheckBox.CheckBoxControl.Location = new System.Drawing.Point(184, 1);
            this.forceUnlockCheckBox.CheckBoxControl.Name = "toolStripCheckBox1";
            this.forceUnlockCheckBox.CheckBoxControl.Size = new System.Drawing.Size(123, 52);
            this.forceUnlockCheckBox.CheckBoxControl.TabIndex = 1;
            this.forceUnlockCheckBox.CheckBoxControl.Text = "Force gw.dat unlock";
            this.forceUnlockCheckBox.CheckBoxControl.UseVisualStyleBackColor = false;
            this.forceUnlockCheckBox.Name = "forceUnlockCheckBox";
            this.forceUnlockCheckBox.Size = new System.Drawing.Size(123, 52);
            this.forceUnlockCheckBox.Text = "Force gw.dat unlock";
            this.forceUnlockCheckBox.ToolTipText = "Enables multi-launching of same copy. (experimental)";
            this.forceUnlockCheckBox.CheckedChanged += new System.EventHandler(this.forceUnlockCheckBox_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 285);
            this.Controls.Add(this.editArgButton);
            this.Controls.Add(this.argumentsTextBox);
            this.Controls.Add(this.profilesListBox);
            this.Controls.Add(this.expertControlsToolStrip);
            this.Controls.Add(this.basicControlsToolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(375, 192);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Guild Wars Multi-Launch (v0.6)";
            this.basicControlsToolStrip.ResumeLayout(false);
            this.basicControlsToolStrip.PerformLayout();
            this.expertControlsToolStrip.ResumeLayout(false);
            this.expertControlsToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip basicControlsToolStrip;
        private System.Windows.Forms.ToolStrip expertControlsToolStrip;
        private System.Windows.Forms.ListBox profilesListBox;
        private System.Windows.Forms.ToolStripButton addButton;
        private System.Windows.Forms.ToolStripButton setPathButton;
        private System.Windows.Forms.ToolStripButton removeButton;
        private System.Windows.Forms.ToolStripButton copyButton;
        private System.Windows.Forms.ToolStripButton launchButton;
        private System.Windows.Forms.ToolStripButton shortcutButton;
        private System.Windows.Forms.ToolStripButton masterShortcutButton;
        private System.Windows.Forms.ToolStripButton killMutexButton;
        private System.Windows.Forms.ToolStripButton startTexModButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton exitButton;
        private ToolStripCheckBox forceUnlockCheckBox;
        private SelfLabeledTextBox argumentsTextBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Button editArgButton;
        private System.Windows.Forms.ToolTip auxToolTip;

    }
}