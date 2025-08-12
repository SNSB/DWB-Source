namespace DiversityWorkbench.PostgreSQL
{
    partial class FormCopyDatabase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCopyDatabase));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.checkBoxIncludeData = new System.Windows.Forms.CheckBox();
            this.textBoxNameOfDatabaseCopy = new System.Windows.Forms.TextBox();
            this.labelNameOfDatabaseCopy = new System.Windows.Forms.Label();
            this.buttonCreateCopy = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.labelPostgresApplicationDirectory = new System.Windows.Forms.Label();
            this.textBoxPostgresApplicationDirectory = new System.Windows.Forms.TextBox();
            this.buttonPostgresApplicationDirectory = new System.Windows.Forms.Button();
            this.labelDatabaseOri = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.checkBoxIncludeData, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.textBoxNameOfDatabaseCopy, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.labelNameOfDatabaseCopy, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.buttonCreateCopy, 1, 5);
            this.tableLayoutPanel.Controls.Add(this.labelPostgresApplicationDirectory, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.textBoxPostgresApplicationDirectory, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.buttonPostgresApplicationDirectory, 2, 4);
            this.tableLayoutPanel.Controls.Add(this.labelDatabaseOri, 1, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 6;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(393, 156);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelHeader, 2);
            this.labelHeader.Location = new System.Drawing.Point(3, 6);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(186, 13);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "Create a copy of the current database";
            // 
            // checkBoxIncludeData
            // 
            this.checkBoxIncludeData.AutoSize = true;
            this.checkBoxIncludeData.Location = new System.Drawing.Point(101, 64);
            this.checkBoxIncludeData.Name = "checkBoxIncludeData";
            this.checkBoxIncludeData.Size = new System.Drawing.Size(85, 17);
            this.checkBoxIncludeData.TabIndex = 0;
            this.checkBoxIncludeData.Text = "Include data";
            this.checkBoxIncludeData.UseVisualStyleBackColor = true;
            this.checkBoxIncludeData.CheckedChanged += new System.EventHandler(this.checkBoxIncludeData_CheckedChanged);
            // 
            // textBoxNameOfDatabaseCopy
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textBoxNameOfDatabaseCopy, 2);
            this.textBoxNameOfDatabaseCopy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNameOfDatabaseCopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxNameOfDatabaseCopy.Location = new System.Drawing.Point(101, 38);
            this.textBoxNameOfDatabaseCopy.Name = "textBoxNameOfDatabaseCopy";
            this.textBoxNameOfDatabaseCopy.Size = new System.Drawing.Size(289, 20);
            this.textBoxNameOfDatabaseCopy.TabIndex = 2;
            this.textBoxNameOfDatabaseCopy.Text = "Database";
            // 
            // labelNameOfDatabaseCopy
            // 
            this.labelNameOfDatabaseCopy.AutoSize = true;
            this.labelNameOfDatabaseCopy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNameOfDatabaseCopy.Location = new System.Drawing.Point(3, 35);
            this.labelNameOfDatabaseCopy.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelNameOfDatabaseCopy.Name = "labelNameOfDatabaseCopy";
            this.labelNameOfDatabaseCopy.Size = new System.Drawing.Size(95, 26);
            this.labelNameOfDatabaseCopy.TabIndex = 3;
            this.labelNameOfDatabaseCopy.Text = "Name of the Copy:";
            this.labelNameOfDatabaseCopy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonCreateCopy
            // 
            this.tableLayoutPanel.SetColumnSpan(this.buttonCreateCopy, 2);
            this.buttonCreateCopy.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonCreateCopy.Image = global::DiversityWorkbench.Properties.Resources.PostgresCopy;
            this.buttonCreateCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonCreateCopy.Location = new System.Drawing.Point(299, 130);
            this.buttonCreateCopy.Name = "buttonCreateCopy";
            this.buttonCreateCopy.Size = new System.Drawing.Size(91, 23);
            this.buttonCreateCopy.TabIndex = 4;
            this.buttonCreateCopy.Text = "Create copy";
            this.buttonCreateCopy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonCreateCopy.UseVisualStyleBackColor = true;
            this.buttonCreateCopy.Click += new System.EventHandler(this.buttonCreateCopy_Click);
            // 
            // labelPostgresApplicationDirectory
            // 
            this.labelPostgresApplicationDirectory.AutoSize = true;
            this.labelPostgresApplicationDirectory.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPostgresApplicationDirectory.Location = new System.Drawing.Point(3, 90);
            this.labelPostgresApplicationDirectory.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelPostgresApplicationDirectory.Name = "labelPostgresApplicationDirectory";
            this.labelPostgresApplicationDirectory.Size = new System.Drawing.Size(95, 13);
            this.labelPostgresApplicationDirectory.TabIndex = 5;
            this.labelPostgresApplicationDirectory.Text = "Postgres apps.:";
            this.labelPostgresApplicationDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPostgresApplicationDirectory
            // 
            this.textBoxPostgresApplicationDirectory.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxPostgresApplicationDirectory.Location = new System.Drawing.Point(101, 87);
            this.textBoxPostgresApplicationDirectory.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.textBoxPostgresApplicationDirectory.Name = "textBoxPostgresApplicationDirectory";
            this.textBoxPostgresApplicationDirectory.Size = new System.Drawing.Size(265, 20);
            this.textBoxPostgresApplicationDirectory.TabIndex = 6;
            this.textBoxPostgresApplicationDirectory.TextChanged += new System.EventHandler(this.textBoxPostgresApplicationDirectory_TextChanged);
            // 
            // buttonPostgresApplicationDirectory
            // 
            this.buttonPostgresApplicationDirectory.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonPostgresApplicationDirectory.Image = global::DiversityWorkbench.Properties.Resources.OpenFolder;
            this.buttonPostgresApplicationDirectory.Location = new System.Drawing.Point(366, 86);
            this.buttonPostgresApplicationDirectory.Margin = new System.Windows.Forms.Padding(0, 2, 3, 3);
            this.buttonPostgresApplicationDirectory.Name = "buttonPostgresApplicationDirectory";
            this.buttonPostgresApplicationDirectory.Size = new System.Drawing.Size(24, 23);
            this.buttonPostgresApplicationDirectory.TabIndex = 7;
            this.buttonPostgresApplicationDirectory.UseVisualStyleBackColor = true;
            this.buttonPostgresApplicationDirectory.Click += new System.EventHandler(this.buttonPostgresApplicationDirectory_Click);
            // 
            // labelDatabaseOri
            // 
            this.labelDatabaseOri.AutoSize = true;
            this.labelDatabaseOri.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDatabaseOri.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDatabaseOri.Location = new System.Drawing.Point(101, 22);
            this.labelDatabaseOri.Name = "labelDatabaseOri";
            this.labelDatabaseOri.Size = new System.Drawing.Size(262, 13);
            this.labelDatabaseOri.TabIndex = 8;
            this.labelDatabaseOri.Text = "Database";
            // 
            // FormCopyDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 156);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCopyDatabase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Copy Database";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.CheckBox checkBoxIncludeData;
        private System.Windows.Forms.TextBox textBoxNameOfDatabaseCopy;
        private System.Windows.Forms.Label labelNameOfDatabaseCopy;
        private System.Windows.Forms.Button buttonCreateCopy;
        private System.Windows.Forms.Label labelPostgresApplicationDirectory;
        private System.Windows.Forms.TextBox textBoxPostgresApplicationDirectory;
        private System.Windows.Forms.Button buttonPostgresApplicationDirectory;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Label labelDatabaseOri;
    }
}