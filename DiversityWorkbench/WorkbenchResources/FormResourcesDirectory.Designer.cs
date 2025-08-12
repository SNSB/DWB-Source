namespace DiversityWorkbench.WorkbenchResources
{
    partial class FormResourcesDirectory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormResourcesDirectory));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.radioButtonHome = new System.Windows.Forms.RadioButton();
            this.radioButtonMyDocuments = new System.Windows.Forms.RadioButton();
            this.radioButtonUserDefined = new System.Windows.Forms.RadioButton();
            this.buttonSetFolder = new System.Windows.Forms.Button();
            this.textBoxFolder = new System.Windows.Forms.TextBox();
            this.buttonOpenDirectory = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.labelCopyOption = new System.Windows.Forms.Label();
            this.comboBoxCopyOption = new System.Windows.Forms.ComboBox();
            this.buttonCopyOption = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.radioButtonHome, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.radioButtonMyDocuments, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.radioButtonUserDefined, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.buttonSetFolder, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.textBoxFolder, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.buttonOpenDirectory, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.labelCopyOption, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.comboBoxCopyOption, 1, 6);
            this.tableLayoutPanel.Controls.Add(this.buttonCopyOption, 0, 6);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 8;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(333, 189);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelHeader, 2);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeader.Location = new System.Drawing.Point(3, 6);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(303, 17);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "Choose the location for the resources";
            // 
            // radioButtonHome
            // 
            this.tableLayoutPanel.SetColumnSpan(this.radioButtonHome, 3);
            this.radioButtonHome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonHome.Image = global::DiversityWorkbench.Properties.Resources.Home;
            this.radioButtonHome.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.radioButtonHome.Location = new System.Drawing.Point(3, 32);
            this.radioButtonHome.Name = "radioButtonHome";
            this.radioButtonHome.Size = new System.Drawing.Size(327, 24);
            this.radioButtonHome.TabIndex = 1;
            this.radioButtonHome.TabStop = true;
            this.radioButtonHome.Text = "      Home directory of the user";
            this.radioButtonHome.UseVisualStyleBackColor = true;
            this.radioButtonHome.Click += new System.EventHandler(this.radioButtonHome_Click);
            // 
            // radioButtonMyDocuments
            // 
            this.tableLayoutPanel.SetColumnSpan(this.radioButtonMyDocuments, 3);
            this.radioButtonMyDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonMyDocuments.Image = global::DiversityWorkbench.Properties.Resources.MyDocuments;
            this.radioButtonMyDocuments.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.radioButtonMyDocuments.Location = new System.Drawing.Point(3, 62);
            this.radioButtonMyDocuments.Name = "radioButtonMyDocuments";
            this.radioButtonMyDocuments.Size = new System.Drawing.Size(327, 24);
            this.radioButtonMyDocuments.TabIndex = 2;
            this.radioButtonMyDocuments.TabStop = true;
            this.radioButtonMyDocuments.Text = "      My documents directory of the user";
            this.radioButtonMyDocuments.UseVisualStyleBackColor = true;
            this.radioButtonMyDocuments.Click += new System.EventHandler(this.radioButtonMyDocuments_Click);
            // 
            // radioButtonUserDefined
            // 
            this.tableLayoutPanel.SetColumnSpan(this.radioButtonUserDefined, 3);
            this.radioButtonUserDefined.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonUserDefined.Image = global::DiversityWorkbench.Properties.Resources.OpenFolder;
            this.radioButtonUserDefined.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.radioButtonUserDefined.Location = new System.Drawing.Point(3, 92);
            this.radioButtonUserDefined.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.radioButtonUserDefined.Name = "radioButtonUserDefined";
            this.radioButtonUserDefined.Size = new System.Drawing.Size(327, 24);
            this.radioButtonUserDefined.TabIndex = 3;
            this.radioButtonUserDefined.TabStop = true;
            this.radioButtonUserDefined.Text = "      User defined directory";
            this.radioButtonUserDefined.UseVisualStyleBackColor = true;
            this.radioButtonUserDefined.Click += new System.EventHandler(this.radioButtonUserDefined_Click);
            // 
            // buttonSetFolder
            // 
            this.buttonSetFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonSetFolder.FlatAppearance.BorderSize = 0;
            this.buttonSetFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetFolder.Image = global::DiversityWorkbench.Properties.Resources.OpenFolder;
            this.buttonSetFolder.Location = new System.Drawing.Point(3, 116);
            this.buttonSetFolder.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.buttonSetFolder.Name = "buttonSetFolder";
            this.buttonSetFolder.Size = new System.Drawing.Size(23, 23);
            this.buttonSetFolder.TabIndex = 4;
            this.toolTip.SetToolTip(this.buttonSetFolder, "Click to set the directory");
            this.buttonSetFolder.UseVisualStyleBackColor = true;
            this.buttonSetFolder.Click += new System.EventHandler(this.buttonSetFolder_Click);
            // 
            // textBoxFolder
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textBoxFolder, 2);
            this.textBoxFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxFolder.Location = new System.Drawing.Point(26, 119);
            this.textBoxFolder.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxFolder.Name = "textBoxFolder";
            this.textBoxFolder.ReadOnly = true;
            this.textBoxFolder.Size = new System.Drawing.Size(304, 20);
            this.textBoxFolder.TabIndex = 5;
            this.toolTip.SetToolTip(this.textBoxFolder, "Directory as defined by the user");
            this.textBoxFolder.TextChanged += new System.EventHandler(this.textBoxFolder_TextChanged);
            // 
            // buttonOpenDirectory
            // 
            this.buttonOpenDirectory.FlatAppearance.BorderSize = 0;
            this.buttonOpenDirectory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOpenDirectory.Image = global::DiversityWorkbench.Properties.Resources.Files;
            this.buttonOpenDirectory.Location = new System.Drawing.Point(312, 3);
            this.buttonOpenDirectory.Name = "buttonOpenDirectory";
            this.buttonOpenDirectory.Size = new System.Drawing.Size(18, 23);
            this.buttonOpenDirectory.TabIndex = 6;
            this.toolTip.SetToolTip(this.buttonOpenDirectory, "Open resources directory in explorer");
            this.buttonOpenDirectory.UseVisualStyleBackColor = true;
            this.buttonOpenDirectory.Click += new System.EventHandler(this.ButtonOpenDirectory_Click);
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 189);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(333, 27);
            this.userControlDialogPanel.TabIndex = 0;
            // 
            // labelCopyOption
            // 
            this.labelCopyOption.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelCopyOption, 2);
            this.labelCopyOption.Location = new System.Drawing.Point(3, 142);
            this.labelCopyOption.Name = "labelCopyOption";
            this.labelCopyOption.Size = new System.Drawing.Size(210, 13);
            this.labelCopyOption.TabIndex = 7;
            this.labelCopyOption.Text = "How to copy files from app to user directory";
            // 
            // comboBoxCopyOption
            // 
            this.tableLayoutPanel.SetColumnSpan(this.comboBoxCopyOption, 2);
            this.comboBoxCopyOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxCopyOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCopyOption.FormattingEnabled = true;
            this.comboBoxCopyOption.Location = new System.Drawing.Point(29, 158);
            this.comboBoxCopyOption.Name = "comboBoxCopyOption";
            this.comboBoxCopyOption.Size = new System.Drawing.Size(301, 21);
            this.comboBoxCopyOption.TabIndex = 8;
            this.comboBoxCopyOption.SelectionChangeCommitted += new System.EventHandler(this.comboBoxCopyOption_SelectionChangeCommitted);
            // 
            // buttonCopyOption
            // 
            this.buttonCopyOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCopyOption.FlatAppearance.BorderSize = 0;
            this.buttonCopyOption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCopyOption.Image = global::DiversityWorkbench.Properties.Resources.Copy2;
            this.buttonCopyOption.Location = new System.Drawing.Point(3, 158);
            this.buttonCopyOption.Name = "buttonCopyOption";
            this.buttonCopyOption.Size = new System.Drawing.Size(20, 23);
            this.buttonCopyOption.TabIndex = 9;
            this.buttonCopyOption.UseVisualStyleBackColor = true;
            // 
            // FormResourcesDirectory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 216);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormResourcesDirectory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Directory for the resources";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.RadioButton radioButtonHome;
        private System.Windows.Forms.RadioButton radioButtonMyDocuments;
        private System.Windows.Forms.RadioButton radioButtonUserDefined;
        private System.Windows.Forms.Button buttonSetFolder;
        private System.Windows.Forms.TextBox textBoxFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonOpenDirectory;
        private System.Windows.Forms.Label labelCopyOption;
        private System.Windows.Forms.ComboBox comboBoxCopyOption;
        private System.Windows.Forms.Button buttonCopyOption;
    }
}