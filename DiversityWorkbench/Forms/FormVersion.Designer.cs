namespace DiversityWorkbench.Forms
{
    partial class FormVersion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormVersion));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxOldVersionClientAsInClient = new System.Windows.Forms.TextBox();
            this.textBoxOldVersionDbAsInClient = new System.Windows.Forms.TextBox();
            this.labelDbAsInClient = new System.Windows.Forms.Label();
            this.labelCurrentVersion = new System.Windows.Forms.Label();
            this.labelClient = new System.Windows.Forms.Label();
            this.labelDatabase = new System.Windows.Forms.Label();
            this.textBoxCurrentVersionClient = new System.Windows.Forms.TextBox();
            this.textBoxCurrentVersionDatabase = new System.Windows.Forms.TextBox();
            this.buttonSetVersionClient = new System.Windows.Forms.Button();
            this.buttonSetVersionDatabase = new System.Windows.Forms.Button();
            this.labelNewVersion = new System.Windows.Forms.Label();
            this.maskedTextBoxVersionDB = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxVersionClient = new System.Windows.Forms.MaskedTextBox();
            this.labelAsInClient = new System.Windows.Forms.Label();
            this.labelClientAsInClient = new System.Windows.Forms.Label();
            this.labelEditClientVersion = new System.Windows.Forms.Label();
            this.labelEditDbVersion = new System.Windows.Forms.Label();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 4;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.textBoxOldVersionClientAsInClient, 1, 4);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxOldVersionDbAsInClient, 1, 5);
            this.tableLayoutPanelMain.Controls.Add(this.labelDbAsInClient, 0, 5);
            this.tableLayoutPanelMain.Controls.Add(this.labelCurrentVersion, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.labelClient, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelDatabase, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxCurrentVersionClient, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxCurrentVersionDatabase, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonSetVersionClient, 3, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonSetVersionDatabase, 3, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelNewVersion, 2, 0);
            this.tableLayoutPanelMain.Controls.Add(this.maskedTextBoxVersionDB, 2, 2);
            this.tableLayoutPanelMain.Controls.Add(this.maskedTextBoxVersionClient, 2, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelAsInClient, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelClientAsInClient, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.labelEditClientVersion, 2, 4);
            this.tableLayoutPanelMain.Controls.Add(this.labelEditDbVersion, 2, 5);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 6;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(418, 171);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // textBoxOldVersionClientAsInClient
            // 
            this.textBoxOldVersionClientAsInClient.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxOldVersionClientAsInClient.Location = new System.Drawing.Point(65, 121);
            this.textBoxOldVersionClientAsInClient.Name = "textBoxOldVersionClientAsInClient";
            this.textBoxOldVersionClientAsInClient.ReadOnly = true;
            this.textBoxOldVersionClientAsInClient.Size = new System.Drawing.Size(119, 20);
            this.textBoxOldVersionClientAsInClient.TabIndex = 17;
            this.textBoxOldVersionClientAsInClient.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxOldVersionDbAsInClient
            // 
            this.textBoxOldVersionDbAsInClient.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxOldVersionDbAsInClient.Location = new System.Drawing.Point(65, 147);
            this.textBoxOldVersionDbAsInClient.Name = "textBoxOldVersionDbAsInClient";
            this.textBoxOldVersionDbAsInClient.ReadOnly = true;
            this.textBoxOldVersionDbAsInClient.Size = new System.Drawing.Size(119, 20);
            this.textBoxOldVersionDbAsInClient.TabIndex = 16;
            this.textBoxOldVersionDbAsInClient.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelDbAsInClient
            // 
            this.labelDbAsInClient.AutoSize = true;
            this.labelDbAsInClient.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDbAsInClient.Location = new System.Drawing.Point(3, 150);
            this.labelDbAsInClient.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelDbAsInClient.Name = "labelDbAsInClient";
            this.labelDbAsInClient.Size = new System.Drawing.Size(56, 13);
            this.labelDbAsInClient.TabIndex = 15;
            this.labelDbAsInClient.Text = "Database:";
            this.labelDbAsInClient.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCurrentVersion
            // 
            this.labelCurrentVersion.AutoSize = true;
            this.labelCurrentVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCurrentVersion.Location = new System.Drawing.Point(65, 8);
            this.labelCurrentVersion.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.labelCurrentVersion.Name = "labelCurrentVersion";
            this.labelCurrentVersion.Size = new System.Drawing.Size(119, 13);
            this.labelCurrentVersion.TabIndex = 0;
            this.labelCurrentVersion.Text = "Current version";
            // 
            // labelClient
            // 
            this.labelClient.AutoSize = true;
            this.labelClient.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelClient.Location = new System.Drawing.Point(3, 27);
            this.labelClient.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelClient.Name = "labelClient";
            this.labelClient.Size = new System.Drawing.Size(56, 13);
            this.labelClient.TabIndex = 1;
            this.labelClient.Text = "Client:";
            this.labelClient.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelDatabase
            // 
            this.labelDatabase.AutoSize = true;
            this.labelDatabase.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDatabase.Location = new System.Drawing.Point(3, 69);
            this.labelDatabase.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelDatabase.Name = "labelDatabase";
            this.labelDatabase.Size = new System.Drawing.Size(56, 13);
            this.labelDatabase.TabIndex = 2;
            this.labelDatabase.Text = "Database:";
            this.labelDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCurrentVersionClient
            // 
            this.textBoxCurrentVersionClient.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxCurrentVersionClient.Location = new System.Drawing.Point(65, 24);
            this.textBoxCurrentVersionClient.Name = "textBoxCurrentVersionClient";
            this.textBoxCurrentVersionClient.ReadOnly = true;
            this.textBoxCurrentVersionClient.Size = new System.Drawing.Size(119, 20);
            this.textBoxCurrentVersionClient.TabIndex = 3;
            this.textBoxCurrentVersionClient.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxCurrentVersionDatabase
            // 
            this.textBoxCurrentVersionDatabase.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxCurrentVersionDatabase.Location = new System.Drawing.Point(65, 66);
            this.textBoxCurrentVersionDatabase.Name = "textBoxCurrentVersionDatabase";
            this.textBoxCurrentVersionDatabase.ReadOnly = true;
            this.textBoxCurrentVersionDatabase.Size = new System.Drawing.Size(119, 20);
            this.textBoxCurrentVersionDatabase.TabIndex = 4;
            this.textBoxCurrentVersionDatabase.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonSetVersionClient
            // 
            this.buttonSetVersionClient.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonSetVersionClient.Location = new System.Drawing.Point(315, 24);
            this.buttonSetVersionClient.Name = "buttonSetVersionClient";
            this.buttonSetVersionClient.Size = new System.Drawing.Size(100, 22);
            this.buttonSetVersionClient.TabIndex = 7;
            this.buttonSetVersionClient.Text = "Set client version";
            this.buttonSetVersionClient.UseVisualStyleBackColor = true;
            this.buttonSetVersionClient.Click += new System.EventHandler(this.buttonSetVersionClient_Click);
            // 
            // buttonSetVersionDatabase
            // 
            this.buttonSetVersionDatabase.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonSetVersionDatabase.Location = new System.Drawing.Point(315, 66);
            this.buttonSetVersionDatabase.Name = "buttonSetVersionDatabase";
            this.buttonSetVersionDatabase.Size = new System.Drawing.Size(100, 23);
            this.buttonSetVersionDatabase.TabIndex = 8;
            this.buttonSetVersionDatabase.Text = "Set DB version";
            this.buttonSetVersionDatabase.UseVisualStyleBackColor = true;
            this.buttonSetVersionDatabase.Click += new System.EventHandler(this.buttonSetVersionDatabase_Click);
            // 
            // labelNewVersion
            // 
            this.labelNewVersion.AutoSize = true;
            this.labelNewVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNewVersion.Location = new System.Drawing.Point(190, 8);
            this.labelNewVersion.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.labelNewVersion.Name = "labelNewVersion";
            this.labelNewVersion.Size = new System.Drawing.Size(119, 13);
            this.labelNewVersion.TabIndex = 9;
            this.labelNewVersion.Text = "New version";
            // 
            // maskedTextBoxVersionDB
            // 
            this.maskedTextBoxVersionDB.Dock = System.Windows.Forms.DockStyle.Top;
            this.maskedTextBoxVersionDB.Location = new System.Drawing.Point(190, 66);
            this.maskedTextBoxVersionDB.Mask = "00/00/00";
            this.maskedTextBoxVersionDB.Name = "maskedTextBoxVersionDB";
            this.maskedTextBoxVersionDB.Size = new System.Drawing.Size(119, 20);
            this.maskedTextBoxVersionDB.TabIndex = 10;
            this.maskedTextBoxVersionDB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // maskedTextBoxVersionClient
            // 
            this.maskedTextBoxVersionClient.Dock = System.Windows.Forms.DockStyle.Top;
            this.maskedTextBoxVersionClient.Location = new System.Drawing.Point(190, 24);
            this.maskedTextBoxVersionClient.Mask = "00/00/00/00";
            this.maskedTextBoxVersionClient.Name = "maskedTextBoxVersionClient";
            this.maskedTextBoxVersionClient.Size = new System.Drawing.Size(119, 20);
            this.maskedTextBoxVersionClient.TabIndex = 11;
            this.maskedTextBoxVersionClient.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelAsInClient
            // 
            this.labelAsInClient.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelAsInClient, 4);
            this.labelAsInClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAsInClient.Location = new System.Drawing.Point(3, 105);
            this.labelAsInClient.Name = "labelAsInClient";
            this.labelAsInClient.Size = new System.Drawing.Size(412, 13);
            this.labelAsInClient.TabIndex = 13;
            this.labelAsInClient.Text = "Versions as stored in client";
            this.labelAsInClient.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelClientAsInClient
            // 
            this.labelClientAsInClient.AutoSize = true;
            this.labelClientAsInClient.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelClientAsInClient.Location = new System.Drawing.Point(3, 124);
            this.labelClientAsInClient.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelClientAsInClient.Name = "labelClientAsInClient";
            this.labelClientAsInClient.Size = new System.Drawing.Size(56, 13);
            this.labelClientAsInClient.TabIndex = 14;
            this.labelClientAsInClient.Text = "Client:";
            this.labelClientAsInClient.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelEditClientVersion
            // 
            this.labelEditClientVersion.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelEditClientVersion, 2);
            this.labelEditClientVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEditClientVersion.Location = new System.Drawing.Point(190, 118);
            this.labelEditClientVersion.Name = "labelEditClientVersion";
            this.labelEditClientVersion.Size = new System.Drawing.Size(225, 26);
            this.labelEditClientVersion.TabIndex = 18;
            this.labelEditClientVersion.Text = "Please edit the AssemblyVersion in the source code file Properties - AssemblyInfo" +
    ".cs";
            this.labelEditClientVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelEditDbVersion
            // 
            this.labelEditDbVersion.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelEditDbVersion, 2);
            this.labelEditDbVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEditDbVersion.Location = new System.Drawing.Point(190, 144);
            this.labelEditDbVersion.Name = "labelEditDbVersion";
            this.labelEditDbVersion.Size = new System.Drawing.Size(225, 27);
            this.labelEditDbVersion.TabIndex = 19;
            this.labelEditDbVersion.Text = "Please edit the version in the source code";
            this.labelEditDbVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormVersion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 171);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormVersion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setting the version of client and database";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelCurrentVersion;
        private System.Windows.Forms.Label labelClient;
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.TextBox textBoxCurrentVersionClient;
        private System.Windows.Forms.TextBox textBoxCurrentVersionDatabase;
        private System.Windows.Forms.Button buttonSetVersionClient;
        private System.Windows.Forms.Button buttonSetVersionDatabase;
        private System.Windows.Forms.Label labelNewVersion;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxVersionDB;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxVersionClient;
        private System.Windows.Forms.Label labelAsInClient;
        private System.Windows.Forms.TextBox textBoxOldVersionClientAsInClient;
        private System.Windows.Forms.TextBox textBoxOldVersionDbAsInClient;
        private System.Windows.Forms.Label labelDbAsInClient;
        private System.Windows.Forms.Label labelClientAsInClient;
        private System.Windows.Forms.Label labelEditClientVersion;
        private System.Windows.Forms.Label labelEditDbVersion;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}