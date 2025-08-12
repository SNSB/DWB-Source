namespace DiversityWorkbench.Forms
{
    partial class FormUserRoles
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUserRoles));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelUserRoles = new System.Windows.Forms.Label();
            this.listBoxUserRoles = new System.Windows.Forms.ListBox();
            this.labelDatabaseAccounts = new System.Windows.Forms.Label();
            this.listBoxDatabaseAccounts = new System.Windows.Forms.ListBox();
            this.listBoxRoles = new System.Windows.Forms.ListBox();
            this.labelRoles = new System.Windows.Forms.Label();
            this.buttonRoleAdd = new System.Windows.Forms.Button();
            this.buttonRoleRemove = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.userControlDialogPanel1 = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.labelUserRoles, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.listBoxUserRoles, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelDatabaseAccounts, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.listBoxDatabaseAccounts, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.listBoxRoles, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelRoles, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonRoleAdd, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonRoleRemove, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.menuStrip1, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(671, 302);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // labelUserRoles
            // 
            this.labelUserRoles.AutoSize = true;
            this.labelUserRoles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelUserRoles.Location = new System.Drawing.Point(455, 21);
            this.labelUserRoles.Name = "labelUserRoles";
            this.labelUserRoles.Size = new System.Drawing.Size(203, 13);
            this.labelUserRoles.TabIndex = 27;
            this.labelUserRoles.Text = "Roles of the user";
            this.labelUserRoles.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // listBoxUserRoles
            // 
            this.listBoxUserRoles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.listBoxUserRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxUserRoles.FormattingEnabled = true;
            this.listBoxUserRoles.IntegralHeight = false;
            this.listBoxUserRoles.Location = new System.Drawing.Point(455, 37);
            this.listBoxUserRoles.Name = "listBoxUserRoles";
            this.tableLayoutPanel1.SetRowSpan(this.listBoxUserRoles, 2);
            this.listBoxUserRoles.Size = new System.Drawing.Size(203, 232);
            this.listBoxUserRoles.TabIndex = 26;
            // 
            // labelDatabaseAccounts
            // 
            this.labelDatabaseAccounts.AutoSize = true;
            this.labelDatabaseAccounts.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelDatabaseAccounts.Location = new System.Drawing.Point(13, 21);
            this.labelDatabaseAccounts.Name = "labelDatabaseAccounts";
            this.labelDatabaseAccounts.Size = new System.Drawing.Size(201, 13);
            this.labelDatabaseAccounts.TabIndex = 13;
            this.labelDatabaseAccounts.Text = "User accounts available in the database";
            this.labelDatabaseAccounts.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // listBoxDatabaseAccounts
            // 
            this.listBoxDatabaseAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxDatabaseAccounts.FormattingEnabled = true;
            this.listBoxDatabaseAccounts.IntegralHeight = false;
            this.listBoxDatabaseAccounts.Location = new System.Drawing.Point(13, 37);
            this.listBoxDatabaseAccounts.Name = "listBoxDatabaseAccounts";
            this.tableLayoutPanel1.SetRowSpan(this.listBoxDatabaseAccounts, 2);
            this.listBoxDatabaseAccounts.Size = new System.Drawing.Size(201, 232);
            this.listBoxDatabaseAccounts.TabIndex = 3;
            this.listBoxDatabaseAccounts.SelectedIndexChanged += new System.EventHandler(this.listBoxDatabaseAccounts_SelectedIndexChanged);
            // 
            // listBoxRoles
            // 
            this.listBoxRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxRoles.FormattingEnabled = true;
            this.listBoxRoles.IntegralHeight = false;
            this.listBoxRoles.Location = new System.Drawing.Point(220, 37);
            this.listBoxRoles.Name = "listBoxRoles";
            this.tableLayoutPanel1.SetRowSpan(this.listBoxRoles, 2);
            this.listBoxRoles.Size = new System.Drawing.Size(201, 232);
            this.listBoxRoles.TabIndex = 12;
            // 
            // labelRoles
            // 
            this.labelRoles.AutoSize = true;
            this.labelRoles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelRoles.Location = new System.Drawing.Point(220, 21);
            this.labelRoles.Name = "labelRoles";
            this.labelRoles.Size = new System.Drawing.Size(201, 13);
            this.labelRoles.TabIndex = 17;
            this.labelRoles.Text = "Roles available in the database";
            this.labelRoles.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // buttonRoleAdd
            // 
            this.buttonRoleAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonRoleAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRoleAdd.Location = new System.Drawing.Point(427, 127);
            this.buttonRoleAdd.Name = "buttonRoleAdd";
            this.buttonRoleAdd.Size = new System.Drawing.Size(22, 23);
            this.buttonRoleAdd.TabIndex = 24;
            this.buttonRoleAdd.Text = ">";
            this.buttonRoleAdd.UseVisualStyleBackColor = true;
            this.buttonRoleAdd.Click += new System.EventHandler(this.buttonRoleAdd_Click);
            // 
            // buttonRoleRemove
            // 
            this.buttonRoleRemove.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonRoleRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRoleRemove.Location = new System.Drawing.Point(427, 156);
            this.buttonRoleRemove.Name = "buttonRoleRemove";
            this.buttonRoleRemove.Size = new System.Drawing.Size(22, 23);
            this.buttonRoleRemove.TabIndex = 25;
            this.buttonRoleRemove.Text = "<";
            this.buttonRoleRemove.UseVisualStyleBackColor = true;
            this.buttonRoleRemove.Click += new System.EventHandler(this.buttonRoleRemove_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(217, 272);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(207, 20);
            this.menuStrip1.TabIndex = 28;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // userControlDialogPanel1
            // 
            this.userControlDialogPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel1.Location = new System.Drawing.Point(0, 302);
            this.userControlDialogPanel1.Name = "userControlDialogPanel1";
            this.userControlDialogPanel1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel1.Size = new System.Drawing.Size(671, 27);
            this.userControlDialogPanel1.TabIndex = 3;
            // 
            // FormUserRoles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 329);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.userControlDialogPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormUserRoles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " User roles";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelDatabaseAccounts;
        private System.Windows.Forms.ListBox listBoxDatabaseAccounts;
        private System.Windows.Forms.ListBox listBoxRoles;
        private System.Windows.Forms.Label labelRoles;
        private System.Windows.Forms.Button buttonRoleAdd;
        private System.Windows.Forms.Button buttonRoleRemove;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel1;
        private System.Windows.Forms.ListBox listBoxUserRoles;
        private System.Windows.Forms.Label labelUserRoles;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}