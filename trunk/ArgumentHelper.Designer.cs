namespace GWMultiLaunch
{
    partial class ArgumentsWizard
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.gwSwitchesGridView = new System.Windows.Forms.DataGridView();
            this.enableColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.switchColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.optionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.gwSwitchesGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(224, 506);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(305, 506);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // gwSwitchesGridView
            // 
            this.gwSwitchesGridView.AllowUserToAddRows = false;
            this.gwSwitchesGridView.AllowUserToDeleteRows = false;
            this.gwSwitchesGridView.AllowUserToResizeColumns = false;
            this.gwSwitchesGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gwSwitchesGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gwSwitchesGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gwSwitchesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gwSwitchesGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.enableColumn,
            this.switchColumn,
            this.optionColumn});
            this.gwSwitchesGridView.Location = new System.Drawing.Point(12, 12);
            this.gwSwitchesGridView.Name = "gwSwitchesGridView";
            this.gwSwitchesGridView.RowHeadersVisible = false;
            this.gwSwitchesGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gwSwitchesGridView.Size = new System.Drawing.Size(368, 483);
            this.gwSwitchesGridView.TabIndex = 2;
            this.gwSwitchesGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gwSwitchesGridView_CellValueChanged);
            this.gwSwitchesGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gwSwitchesGridView_CellDoubleClick);
            this.gwSwitchesGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gwSwitchesGridView_KeyDown);
            // 
            // enableColumn
            // 
            this.enableColumn.HeaderText = "Enable";
            this.enableColumn.Name = "enableColumn";
            this.enableColumn.Width = 45;
            // 
            // switchColumn
            // 
            this.switchColumn.HeaderText = "Switch";
            this.switchColumn.Name = "switchColumn";
            this.switchColumn.ReadOnly = true;
            this.switchColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // optionColumn
            // 
            this.optionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.optionColumn.HeaderText = "Option";
            this.optionColumn.Name = "optionColumn";
            this.optionColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(12, 511);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(142, 13);
            this.linkLabel1.TabIndex = 3;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "More info at Guild Wars Wiki";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // ArgumentsWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 536);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.gwSwitchesGridView);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.Name = "ArgumentsWizard";
            this.Text = "Arguments Wizard";
            this.Load += new System.EventHandler(this.ArgumentHelper_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gwSwitchesGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.DataGridView gwSwitchesGridView;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enableColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn switchColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn optionColumn;
    }
}