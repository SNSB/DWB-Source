namespace DiversityWorkbench.Spreadsheet
{
    partial class FormFixedSourceQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFixedSourceQuery));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.buttonSetSource = new System.Windows.Forms.Button();
            this.labelSource = new System.Windows.Forms.Label();
            this.comboBoxQuery = new System.Windows.Forms.ComboBox();
            this.buttonInfoSettings = new System.Windows.Forms.Button();
            this.labelSettings = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonRemoveSource = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.userControlDialogPanel, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.buttonSetSource, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.labelSource, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.comboBoxQuery, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.buttonInfoSettings, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.labelSettings, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonRemoveSource, 0, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(483, 117);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // userControlDialogPanel
            // 
            this.tableLayoutPanel.SetColumnSpan(this.userControlDialogPanel, 3);
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDialogPanel.Location = new System.Drawing.Point(3, 85);
            this.userControlDialogPanel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(477, 29);
            this.userControlDialogPanel.TabIndex = 1;
            // 
            // buttonSetSource
            // 
            this.buttonSetSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSetSource.FlatAppearance.BorderSize = 0;
            this.buttonSetSource.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetSource.Image = global::DiversityWorkbench.Properties.Resources.Pin_3;
            this.buttonSetSource.Location = new System.Drawing.Point(464, 39);
            this.buttonSetSource.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.buttonSetSource.Name = "buttonSetSource";
            this.buttonSetSource.Size = new System.Drawing.Size(16, 19);
            this.buttonSetSource.TabIndex = 0;
            this.toolTip.SetToolTip(this.buttonSetSource, "Change the source");
            this.buttonSetSource.UseVisualStyleBackColor = true;
            this.buttonSetSource.Click += new System.EventHandler(this.buttonSetSource_Click);
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.labelSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSource.Location = new System.Drawing.Point(19, 20);
            this.labelSource.Name = "labelSource";
            this.tableLayoutPanel.SetRowSpan(this.labelSource, 2);
            this.labelSource.Size = new System.Drawing.Size(442, 38);
            this.labelSource.TabIndex = 1;
            this.labelSource.Text = "Source";
            this.labelSource.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxQuery
            // 
            this.tableLayoutPanel.SetColumnSpan(this.comboBoxQuery, 3);
            this.comboBoxQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxQuery.FormattingEnabled = true;
            this.comboBoxQuery.Location = new System.Drawing.Point(3, 61);
            this.comboBoxQuery.Name = "comboBoxQuery";
            this.comboBoxQuery.Size = new System.Drawing.Size(477, 21);
            this.comboBoxQuery.TabIndex = 2;
            this.comboBoxQuery.DropDown += new System.EventHandler(this.comboBoxQuery_DropDown);
            this.comboBoxQuery.SelectionChangeCommitted += new System.EventHandler(this.comboBoxQuery_SelectionChangeCommitted);
            // 
            // buttonInfoSettings
            // 
            this.buttonInfoSettings.FlatAppearance.BorderSize = 0;
            this.buttonInfoSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonInfoSettings.Image = global::DiversityWorkbench.Properties.Resources.Manual;
            this.buttonInfoSettings.Location = new System.Drawing.Point(464, 20);
            this.buttonInfoSettings.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.buttonInfoSettings.Name = "buttonInfoSettings";
            this.buttonInfoSettings.Size = new System.Drawing.Size(16, 19);
            this.buttonInfoSettings.TabIndex = 3;
            this.toolTip.SetToolTip(this.buttonInfoSettings, "Show the current settings for remote modules");
            this.buttonInfoSettings.UseVisualStyleBackColor = true;
            this.buttonInfoSettings.Click += new System.EventHandler(this.buttonInfoSettings_Click);
            // 
            // labelSettings
            // 
            this.labelSettings.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelSettings, 2);
            this.labelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSettings.ForeColor = System.Drawing.Color.SteelBlue;
            this.labelSettings.Location = new System.Drawing.Point(19, 0);
            this.labelSettings.Name = "labelSettings";
            this.labelSettings.Size = new System.Drawing.Size(461, 20);
            this.labelSettings.TabIndex = 4;
            this.labelSettings.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonRemoveSource
            // 
            this.buttonRemoveSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRemoveSource.FlatAppearance.BorderSize = 0;
            this.buttonRemoveSource.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRemoveSource.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonRemoveSource.Location = new System.Drawing.Point(0, 39);
            this.buttonRemoveSource.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRemoveSource.Name = "buttonRemoveSource";
            this.buttonRemoveSource.Size = new System.Drawing.Size(16, 19);
            this.buttonRemoveSource.TabIndex = 5;
            this.toolTip.SetToolTip(this.buttonRemoveSource, "Remove the source");
            this.buttonRemoveSource.UseVisualStyleBackColor = true;
            this.buttonRemoveSource.Visible = false;
            this.buttonRemoveSource.Click += new System.EventHandler(this.buttonRemoveSource_Click);
            // 
            // FormFixedSourceQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 117);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormFixedSourceQuery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormFixedSourceQuery";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonSetSource;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.ComboBox comboBoxQuery;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonInfoSettings;
        private System.Windows.Forms.Label labelSettings;
        private System.Windows.Forms.Button buttonRemoveSource;
    }
}