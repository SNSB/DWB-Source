namespace DiversityWorkbench.Forms
{
    partial class FormSynchronisation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSynchronisation));
            this.comboBoxUser = new System.Windows.Forms.ComboBox();
            this.labelUser = new System.Windows.Forms.Label();
            this.labelDB = new System.Windows.Forms.Label();
            this.comboBoxDatabase = new System.Windows.Forms.ComboBox();
            this.buttonCreateSynchroTables = new System.Windows.Forms.Button();
            this.labelHeader = new System.Windows.Forms.Label();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.SuspendLayout();
            // 
            // comboBoxUser
            // 
            this.comboBoxUser.FormattingEnabled = true;
            this.comboBoxUser.Location = new System.Drawing.Point(74, 45);
            this.comboBoxUser.Name = "comboBoxUser";
            this.comboBoxUser.Size = new System.Drawing.Size(366, 21);
            this.comboBoxUser.TabIndex = 0;
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(12, 48);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(32, 13);
            this.labelUser.TabIndex = 1;
            this.labelUser.Text = "User:";
            // 
            // labelDB
            // 
            this.labelDB.AutoSize = true;
            this.labelDB.Location = new System.Drawing.Point(12, 71);
            this.labelDB.Name = "labelDB";
            this.labelDB.Size = new System.Drawing.Size(56, 13);
            this.labelDB.TabIndex = 2;
            this.labelDB.Text = "Database:";
            // 
            // comboBoxDatabase
            // 
            this.comboBoxDatabase.FormattingEnabled = true;
            this.comboBoxDatabase.Location = new System.Drawing.Point(74, 68);
            this.comboBoxDatabase.Name = "comboBoxDatabase";
            this.comboBoxDatabase.Size = new System.Drawing.Size(366, 21);
            this.comboBoxDatabase.TabIndex = 3;
            this.comboBoxDatabase.SelectedIndexChanged += new System.EventHandler(this.comboBoxDatabase_SelectedIndexChanged);
            // 
            // buttonCreateSynchroTables
            // 
            this.buttonCreateSynchroTables.Location = new System.Drawing.Point(74, 95);
            this.buttonCreateSynchroTables.Name = "buttonCreateSynchroTables";
            this.buttonCreateSynchroTables.Size = new System.Drawing.Size(290, 23);
            this.buttonCreateSynchroTables.TabIndex = 4;
            this.buttonCreateSynchroTables.Text = "Create schema and tables";
            this.buttonCreateSynchroTables.UseVisualStyleBackColor = true;
            this.buttonCreateSynchroTables.Click += new System.EventHandler(this.buttonCreateSynchroTables_Click);
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Location = new System.Drawing.Point(12, 18);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(404, 13);
            this.labelHeader.TabIndex = 5;
            this.labelHeader.Text = "Create the schema and tables necessary for the synchronisation with DiversityMobi" +
    "le";
            // 
            // FormSynchronisation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 144);
            this.Controls.Add(this.labelHeader);
            this.Controls.Add(this.buttonCreateSynchroTables);
            this.Controls.Add(this.comboBoxDatabase);
            this.Controls.Add(this.labelDB);
            this.Controls.Add(this.labelUser);
            this.Controls.Add(this.comboBoxUser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSynchronisation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Synchronisation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxUser;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelDB;
        private System.Windows.Forms.ComboBox comboBoxDatabase;
        private System.Windows.Forms.Button buttonCreateSynchroTables;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}