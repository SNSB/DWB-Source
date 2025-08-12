namespace DiversityWorkbench.Forms
{
    partial class FormUserAdministration
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUserAdministration));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelProjectsNotAvailable = new System.Windows.Forms.Label();
            this.listBoxProjectsNotAvailable = new System.Windows.Forms.ListBox();
            this.labelDatabaseAccounts = new System.Windows.Forms.Label();
            this.listBoxDatabaseAccounts = new System.Windows.Forms.ListBox();
            this.groupBoxUser = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelPermissions = new System.Windows.Forms.TableLayoutPanel();
            this.listBoxUserRoles = new System.Windows.Forms.ListBox();
            this.labelUserRoles = new System.Windows.Forms.Label();
            this.labelProjectsAvailable = new System.Windows.Forms.Label();
            this.listBoxProjectsAvailable = new System.Windows.Forms.ListBox();
            this.listBoxRoles = new System.Windows.Forms.ListBox();
            this.labelRoles = new System.Windows.Forms.Label();
            this.labelUser = new System.Windows.Forms.Label();
            this.listBoxUser = new System.Windows.Forms.ListBox();
            this.buttonUserAdd = new System.Windows.Forms.Button();
            this.buttonUserRemove = new System.Windows.Forms.Button();
            this.buttonProjectAdd = new System.Windows.Forms.Button();
            this.buttonProjectRemove = new System.Windows.Forms.Button();
            this.buttonRoleAdd = new System.Windows.Forms.Button();
            this.buttonRoleRemove = new System.Windows.Forms.Button();
            this.textBoxCombinedNameCache = new System.Windows.Forms.TextBox();
            this.dataGridViewRolePermissions = new System.Windows.Forms.DataGridView();
            this.listBoxRoleMembers = new System.Windows.Forms.ListBox();
            this.labelRoleMembers = new System.Windows.Forms.Label();
            this.labelRolePermissions = new System.Windows.Forms.Label();
            this.buttonSynchronizeUser = new System.Windows.Forms.Button();
            this.buttonCreateUser = new System.Windows.Forms.Button();
            this.buttonCreateProject = new System.Windows.Forms.Button();
            this.buttonSynchronizeProjects = new System.Windows.Forms.Button();
            this.buttonSaveUserCombinedNameCache = new System.Windows.Forms.Button();
            this.labelUserURI = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryAgentURI = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.labelOrderProject = new System.Windows.Forms.Label();
            this.radioButtonOrderProjectByName = new System.Windows.Forms.RadioButton();
            this.radioButtonOrderProjectByID = new System.Windows.Forms.RadioButton();
            this.labelIncludedRoles = new System.Windows.Forms.Label();
            this.listBoxIncludedRoles = new System.Windows.Forms.ListBox();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanelMain.SuspendLayout();
            this.groupBoxUser.SuspendLayout();
            this.tableLayoutPanelPermissions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRolePermissions)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 10;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanelMain.Controls.Add(this.labelProjectsNotAvailable, 7, 1);
            this.tableLayoutPanelMain.Controls.Add(this.listBoxProjectsNotAvailable, 7, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelDatabaseAccounts, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.listBoxDatabaseAccounts, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.groupBoxUser, 4, 0);
            this.tableLayoutPanelMain.Controls.Add(this.listBoxRoles, 4, 8);
            this.tableLayoutPanelMain.Controls.Add(this.labelRoles, 4, 7);
            this.tableLayoutPanelMain.Controls.Add(this.labelUser, 2, 1);
            this.tableLayoutPanelMain.Controls.Add(this.listBoxUser, 2, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonUserAdd, 1, 3);
            this.tableLayoutPanelMain.Controls.Add(this.buttonUserRemove, 1, 4);
            this.tableLayoutPanelMain.Controls.Add(this.buttonProjectAdd, 6, 3);
            this.tableLayoutPanelMain.Controls.Add(this.buttonProjectRemove, 6, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonRoleAdd, 4, 6);
            this.tableLayoutPanelMain.Controls.Add(this.buttonRoleRemove, 5, 6);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxCombinedNameCache, 2, 0);
            this.tableLayoutPanelMain.Controls.Add(this.dataGridViewRolePermissions, 0, 8);
            this.tableLayoutPanelMain.Controls.Add(this.listBoxRoleMembers, 7, 8);
            this.tableLayoutPanelMain.Controls.Add(this.labelRoleMembers, 7, 7);
            this.tableLayoutPanelMain.Controls.Add(this.labelRolePermissions, 0, 7);
            this.tableLayoutPanelMain.Controls.Add(this.buttonSynchronizeUser, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonCreateUser, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonCreateProject, 6, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonSynchronizeProjects, 7, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonSaveUserCombinedNameCache, 3, 0);
            this.tableLayoutPanelMain.Controls.Add(this.labelUserURI, 0, 6);
            this.tableLayoutPanelMain.Controls.Add(this.userControlModuleRelatedEntryAgentURI, 2, 6);
            this.tableLayoutPanelMain.Controls.Add(this.labelOrderProject, 7, 5);
            this.tableLayoutPanelMain.Controls.Add(this.radioButtonOrderProjectByName, 8, 5);
            this.tableLayoutPanelMain.Controls.Add(this.radioButtonOrderProjectByID, 9, 5);
            this.tableLayoutPanelMain.Controls.Add(this.labelIncludedRoles, 7, 9);
            this.tableLayoutPanelMain.Controls.Add(this.listBoxIncludedRoles, 7, 10);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(8, 8);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 11;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(890, 592);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // labelProjectsNotAvailable
            // 
            this.labelProjectsNotAvailable.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelProjectsNotAvailable, 3);
            this.labelProjectsNotAvailable.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelProjectsNotAvailable.Location = new System.Drawing.Point(691, 37);
            this.labelProjectsNotAvailable.Name = "labelProjectsNotAvailable";
            this.labelProjectsNotAvailable.Size = new System.Drawing.Size(196, 13);
            this.labelProjectsNotAvailable.TabIndex = 16;
            this.labelProjectsNotAvailable.Text = "Project that are not available for a user";
            // 
            // listBoxProjectsNotAvailable
            // 
            this.listBoxProjectsNotAvailable.BackColor = System.Drawing.Color.Pink;
            this.tableLayoutPanelMain.SetColumnSpan(this.listBoxProjectsNotAvailable, 3);
            this.listBoxProjectsNotAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjectsNotAvailable.FormattingEnabled = true;
            this.listBoxProjectsNotAvailable.IntegralHeight = false;
            this.listBoxProjectsNotAvailable.Location = new System.Drawing.Point(691, 53);
            this.listBoxProjectsNotAvailable.Name = "listBoxProjectsNotAvailable";
            this.tableLayoutPanelMain.SetRowSpan(this.listBoxProjectsNotAvailable, 3);
            this.listBoxProjectsNotAvailable.Size = new System.Drawing.Size(196, 227);
            this.listBoxProjectsNotAvailable.TabIndex = 6;
            // 
            // labelDatabaseAccounts
            // 
            this.labelDatabaseAccounts.AutoSize = true;
            this.labelDatabaseAccounts.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelDatabaseAccounts.Location = new System.Drawing.Point(3, 24);
            this.labelDatabaseAccounts.Name = "labelDatabaseAccounts";
            this.labelDatabaseAccounts.Size = new System.Drawing.Size(196, 26);
            this.labelDatabaseAccounts.TabIndex = 13;
            this.labelDatabaseAccounts.Text = "User accounts available in the database";
            // 
            // listBoxDatabaseAccounts
            // 
            this.listBoxDatabaseAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxDatabaseAccounts.FormattingEnabled = true;
            this.listBoxDatabaseAccounts.IntegralHeight = false;
            this.listBoxDatabaseAccounts.Location = new System.Drawing.Point(3, 53);
            this.listBoxDatabaseAccounts.Name = "listBoxDatabaseAccounts";
            this.tableLayoutPanelMain.SetRowSpan(this.listBoxDatabaseAccounts, 4);
            this.listBoxDatabaseAccounts.Size = new System.Drawing.Size(196, 250);
            this.listBoxDatabaseAccounts.TabIndex = 3;
            // 
            // groupBoxUser
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.groupBoxUser, 2);
            this.groupBoxUser.Controls.Add(this.tableLayoutPanelPermissions);
            this.groupBoxUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxUser.Location = new System.Drawing.Point(460, 3);
            this.groupBoxUser.Name = "groupBoxUser";
            this.tableLayoutPanelMain.SetRowSpan(this.groupBoxUser, 6);
            this.groupBoxUser.Size = new System.Drawing.Size(196, 300);
            this.groupBoxUser.TabIndex = 19;
            this.groupBoxUser.TabStop = false;
            this.groupBoxUser.Text = "Permissions of user";
            // 
            // tableLayoutPanelPermissions
            // 
            this.tableLayoutPanelPermissions.ColumnCount = 1;
            this.tableLayoutPanelPermissions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPermissions.Controls.Add(this.listBoxUserRoles, 0, 3);
            this.tableLayoutPanelPermissions.Controls.Add(this.labelUserRoles, 0, 2);
            this.tableLayoutPanelPermissions.Controls.Add(this.labelProjectsAvailable, 0, 0);
            this.tableLayoutPanelPermissions.Controls.Add(this.listBoxProjectsAvailable, 0, 1);
            this.tableLayoutPanelPermissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelPermissions.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelPermissions.Name = "tableLayoutPanelPermissions";
            this.tableLayoutPanelPermissions.RowCount = 4;
            this.tableLayoutPanelPermissions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelPermissions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPermissions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelPermissions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelPermissions.Size = new System.Drawing.Size(190, 281);
            this.tableLayoutPanelPermissions.TabIndex = 0;
            // 
            // listBoxUserRoles
            // 
            this.listBoxUserRoles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.listBoxUserRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxUserRoles.FormattingEnabled = true;
            this.listBoxUserRoles.IntegralHeight = false;
            this.listBoxUserRoles.Location = new System.Drawing.Point(3, 167);
            this.listBoxUserRoles.Name = "listBoxUserRoles";
            this.listBoxUserRoles.Size = new System.Drawing.Size(184, 111);
            this.listBoxUserRoles.TabIndex = 4;
            // 
            // labelUserRoles
            // 
            this.labelUserRoles.AutoSize = true;
            this.labelUserRoles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelUserRoles.Location = new System.Drawing.Point(3, 151);
            this.labelUserRoles.Name = "labelUserRoles";
            this.labelUserRoles.Size = new System.Drawing.Size(184, 13);
            this.labelUserRoles.TabIndex = 18;
            this.labelUserRoles.Text = "Roles of the user";
            // 
            // labelProjectsAvailable
            // 
            this.labelProjectsAvailable.AutoSize = true;
            this.labelProjectsAvailable.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelProjectsAvailable.Location = new System.Drawing.Point(3, 11);
            this.labelProjectsAvailable.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.labelProjectsAvailable.Name = "labelProjectsAvailable";
            this.labelProjectsAvailable.Size = new System.Drawing.Size(184, 13);
            this.labelProjectsAvailable.TabIndex = 15;
            this.labelProjectsAvailable.Text = "Projects that are available for a user";
            // 
            // listBoxProjectsAvailable
            // 
            this.listBoxProjectsAvailable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.listBoxProjectsAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProjectsAvailable.FormattingEnabled = true;
            this.listBoxProjectsAvailable.IntegralHeight = false;
            this.listBoxProjectsAvailable.Location = new System.Drawing.Point(3, 27);
            this.listBoxProjectsAvailable.Name = "listBoxProjectsAvailable";
            this.listBoxProjectsAvailable.Size = new System.Drawing.Size(184, 110);
            this.listBoxProjectsAvailable.TabIndex = 5;
            // 
            // listBoxRoles
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.listBoxRoles, 2);
            this.listBoxRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxRoles.FormattingEnabled = true;
            this.listBoxRoles.IntegralHeight = false;
            this.listBoxRoles.Location = new System.Drawing.Point(460, 346);
            this.listBoxRoles.Name = "listBoxRoles";
            this.tableLayoutPanelMain.SetRowSpan(this.listBoxRoles, 3);
            this.listBoxRoles.Size = new System.Drawing.Size(196, 243);
            this.listBoxRoles.TabIndex = 12;
            this.listBoxRoles.SelectedIndexChanged += new System.EventHandler(this.listBoxRoles_SelectedIndexChanged);
            // 
            // labelRoles
            // 
            this.labelRoles.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelRoles, 2);
            this.labelRoles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelRoles.Location = new System.Drawing.Point(460, 330);
            this.labelRoles.Name = "labelRoles";
            this.labelRoles.Size = new System.Drawing.Size(196, 13);
            this.labelRoles.TabIndex = 17;
            this.labelRoles.Text = "Roles available in the database";
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelUser, 2);
            this.labelUser.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelUser.Location = new System.Drawing.Point(234, 37);
            this.labelUser.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(220, 13);
            this.labelUser.TabIndex = 14;
            this.labelUser.Text = "User accounts with access to projects";
            // 
            // listBoxUser
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.listBoxUser, 2);
            this.listBoxUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxUser.FormattingEnabled = true;
            this.listBoxUser.IntegralHeight = false;
            this.listBoxUser.Location = new System.Drawing.Point(234, 53);
            this.listBoxUser.Name = "listBoxUser";
            this.tableLayoutPanelMain.SetRowSpan(this.listBoxUser, 4);
            this.listBoxUser.Size = new System.Drawing.Size(220, 250);
            this.listBoxUser.TabIndex = 11;
            this.listBoxUser.SelectedIndexChanged += new System.EventHandler(this.listBoxUser_SelectedIndexChanged);
            // 
            // buttonUserAdd
            // 
            this.buttonUserAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonUserAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUserAdd.Location = new System.Drawing.Point(205, 140);
            this.buttonUserAdd.Name = "buttonUserAdd";
            this.buttonUserAdd.Size = new System.Drawing.Size(23, 23);
            this.buttonUserAdd.TabIndex = 20;
            this.buttonUserAdd.Text = ">";
            this.toolTip.SetToolTip(this.buttonUserAdd, "Take the selected user into  the list of users with access to projects");
            this.buttonUserAdd.UseVisualStyleBackColor = true;
            this.buttonUserAdd.Click += new System.EventHandler(this.buttonUserAdd_Click);
            // 
            // buttonUserRemove
            // 
            this.buttonUserRemove.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonUserRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUserRemove.Location = new System.Drawing.Point(205, 169);
            this.buttonUserRemove.Name = "buttonUserRemove";
            this.buttonUserRemove.Size = new System.Drawing.Size(23, 23);
            this.buttonUserRemove.TabIndex = 21;
            this.buttonUserRemove.Text = "<";
            this.toolTip.SetToolTip(this.buttonUserRemove, "Remove the selected user from the list of users with access to projects");
            this.buttonUserRemove.UseVisualStyleBackColor = true;
            this.buttonUserRemove.Click += new System.EventHandler(this.buttonUserRemove_Click);
            // 
            // buttonProjectAdd
            // 
            this.buttonProjectAdd.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonProjectAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonProjectAdd.Location = new System.Drawing.Point(662, 99);
            this.buttonProjectAdd.Name = "buttonProjectAdd";
            this.buttonProjectAdd.Size = new System.Drawing.Size(23, 23);
            this.buttonProjectAdd.TabIndex = 22;
            this.buttonProjectAdd.Text = "<";
            this.toolTip.SetToolTip(this.buttonProjectAdd, "All the selected project to the projects available to the user");
            this.buttonProjectAdd.UseVisualStyleBackColor = true;
            this.buttonProjectAdd.Click += new System.EventHandler(this.buttonProjectAdd_Click);
            // 
            // buttonProjectRemove
            // 
            this.buttonProjectRemove.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonProjectRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonProjectRemove.Location = new System.Drawing.Point(662, 70);
            this.buttonProjectRemove.Name = "buttonProjectRemove";
            this.buttonProjectRemove.Size = new System.Drawing.Size(23, 23);
            this.buttonProjectRemove.TabIndex = 23;
            this.buttonProjectRemove.Text = ">";
            this.toolTip.SetToolTip(this.buttonProjectRemove, "Remove selected project from the availability list of the user");
            this.buttonProjectRemove.UseVisualStyleBackColor = true;
            this.buttonProjectRemove.Click += new System.EventHandler(this.buttonProjectRemove_Click);
            // 
            // buttonRoleAdd
            // 
            this.buttonRoleAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonRoleAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRoleAdd.Image = global::DiversityWorkbench.ResourceWorkbench.ArrowUp;
            this.buttonRoleAdd.Location = new System.Drawing.Point(533, 309);
            this.buttonRoleAdd.Name = "buttonRoleAdd";
            this.buttonRoleAdd.Size = new System.Drawing.Size(22, 18);
            this.buttonRoleAdd.TabIndex = 24;
            this.toolTip.SetToolTip(this.buttonRoleAdd, "Add the selected role to the roles of the current user");
            this.buttonRoleAdd.UseVisualStyleBackColor = true;
            this.buttonRoleAdd.Click += new System.EventHandler(this.buttonRoleAdd_Click);
            // 
            // buttonRoleRemove
            // 
            this.buttonRoleRemove.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonRoleRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRoleRemove.Image = global::DiversityWorkbench.ResourceWorkbench.ArrowDown;
            this.buttonRoleRemove.Location = new System.Drawing.Point(561, 309);
            this.buttonRoleRemove.Name = "buttonRoleRemove";
            this.buttonRoleRemove.Size = new System.Drawing.Size(22, 18);
            this.buttonRoleRemove.TabIndex = 25;
            this.toolTip.SetToolTip(this.buttonRoleRemove, "Remove the selected role from the roles of the current user");
            this.buttonRoleRemove.UseVisualStyleBackColor = true;
            this.buttonRoleRemove.Click += new System.EventHandler(this.buttonRoleRemove_Click);
            // 
            // textBoxCombinedNameCache
            // 
            this.textBoxCombinedNameCache.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxCombinedNameCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCombinedNameCache.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCombinedNameCache.Location = new System.Drawing.Point(234, 0);
            this.textBoxCombinedNameCache.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.textBoxCombinedNameCache.Name = "textBoxCombinedNameCache";
            this.textBoxCombinedNameCache.Size = new System.Drawing.Size(199, 20);
            this.textBoxCombinedNameCache.TabIndex = 27;
            // 
            // dataGridViewRolePermissions
            // 
            this.dataGridViewRolePermissions.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridViewRolePermissions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewRolePermissions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanelMain.SetColumnSpan(this.dataGridViewRolePermissions, 3);
            this.dataGridViewRolePermissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewRolePermissions.Location = new System.Drawing.Point(3, 346);
            this.dataGridViewRolePermissions.Name = "dataGridViewRolePermissions";
            this.tableLayoutPanelMain.SetRowSpan(this.dataGridViewRolePermissions, 3);
            this.dataGridViewRolePermissions.Size = new System.Drawing.Size(427, 243);
            this.dataGridViewRolePermissions.TabIndex = 28;
            // 
            // listBoxRoleMembers
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.listBoxRoleMembers, 3);
            this.listBoxRoleMembers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxRoleMembers.FormattingEnabled = true;
            this.listBoxRoleMembers.IntegralHeight = false;
            this.listBoxRoleMembers.Location = new System.Drawing.Point(691, 346);
            this.listBoxRoleMembers.Name = "listBoxRoleMembers";
            this.listBoxRoleMembers.Size = new System.Drawing.Size(196, 134);
            this.listBoxRoleMembers.TabIndex = 29;
            // 
            // labelRoleMembers
            // 
            this.labelRoleMembers.AutoSize = true;
            this.labelRoleMembers.Location = new System.Drawing.Point(691, 330);
            this.labelRoleMembers.Name = "labelRoleMembers";
            this.labelRoleMembers.Size = new System.Drawing.Size(74, 13);
            this.labelRoleMembers.TabIndex = 30;
            this.labelRoleMembers.Text = "Role members";
            // 
            // labelRolePermissions
            // 
            this.labelRolePermissions.AutoSize = true;
            this.labelRolePermissions.Location = new System.Drawing.Point(3, 330);
            this.labelRolePermissions.Name = "labelRolePermissions";
            this.labelRolePermissions.Size = new System.Drawing.Size(86, 13);
            this.labelRolePermissions.TabIndex = 31;
            this.labelRolePermissions.Text = "Role permissions";
            // 
            // buttonSynchronizeUser
            // 
            this.buttonSynchronizeUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSynchronizeUser.Image = global::DiversityWorkbench.Properties.Resources.DiversityWorkbench_3;
            this.buttonSynchronizeUser.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.buttonSynchronizeUser.Location = new System.Drawing.Point(3, 0);
            this.buttonSynchronizeUser.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.buttonSynchronizeUser.Name = "buttonSynchronizeUser";
            this.buttonSynchronizeUser.Size = new System.Drawing.Size(196, 23);
            this.buttonSynchronizeUser.TabIndex = 32;
            this.buttonSynchronizeUser.Text = "Synchronise with DiversityUsers";
            this.buttonSynchronizeUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSynchronizeUser.UseVisualStyleBackColor = true;
            this.buttonSynchronizeUser.Click += new System.EventHandler(this.buttonSynchronizeUser_Click);
            // 
            // buttonCreateUser
            // 
            this.buttonCreateUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonCreateUser.Image = global::DiversityWorkbench.Properties.Resources.Agent;
            this.buttonCreateUser.Location = new System.Drawing.Point(205, 0);
            this.buttonCreateUser.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonCreateUser.Name = "buttonCreateUser";
            this.buttonCreateUser.Size = new System.Drawing.Size(23, 23);
            this.buttonCreateUser.TabIndex = 33;
            this.toolTip.SetToolTip(this.buttonCreateUser, "Create a new user");
            this.buttonCreateUser.UseVisualStyleBackColor = true;
            this.buttonCreateUser.Click += new System.EventHandler(this.buttonCreateUser_Click);
            // 
            // buttonCreateProject
            // 
            this.buttonCreateProject.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonCreateProject.Image = global::DiversityWorkbench.Properties.Resources.Project;
            this.buttonCreateProject.Location = new System.Drawing.Point(662, 0);
            this.buttonCreateProject.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonCreateProject.Name = "buttonCreateProject";
            this.tableLayoutPanelMain.SetRowSpan(this.buttonCreateProject, 2);
            this.buttonCreateProject.Size = new System.Drawing.Size(23, 23);
            this.buttonCreateProject.TabIndex = 34;
            this.toolTip.SetToolTip(this.buttonCreateProject, "Create a new project");
            this.buttonCreateProject.UseVisualStyleBackColor = true;
            this.buttonCreateProject.Click += new System.EventHandler(this.buttonCreateProject_Click);
            // 
            // buttonSynchronizeProjects
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonSynchronizeProjects, 3);
            this.buttonSynchronizeProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSynchronizeProjects.Image = global::DiversityWorkbench.Properties.Resources.DiversityWorkbench_3;
            this.buttonSynchronizeProjects.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.buttonSynchronizeProjects.Location = new System.Drawing.Point(691, 0);
            this.buttonSynchronizeProjects.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonSynchronizeProjects.Name = "buttonSynchronizeProjects";
            this.buttonSynchronizeProjects.Size = new System.Drawing.Size(196, 24);
            this.buttonSynchronizeProjects.TabIndex = 35;
            this.buttonSynchronizeProjects.Text = "Synchronize with DiversityProjects";
            this.buttonSynchronizeProjects.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSynchronizeProjects.UseVisualStyleBackColor = true;
            this.buttonSynchronizeProjects.Click += new System.EventHandler(this.buttonSynchronizeProjects_Click);
            // 
            // buttonSaveUserCombinedNameCache
            // 
            this.buttonSaveUserCombinedNameCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSaveUserCombinedNameCache.Image = global::DiversityWorkbench.ResourceWorkbench.Save;
            this.buttonSaveUserCombinedNameCache.Location = new System.Drawing.Point(433, 0);
            this.buttonSaveUserCombinedNameCache.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.buttonSaveUserCombinedNameCache.Name = "buttonSaveUserCombinedNameCache";
            this.buttonSaveUserCombinedNameCache.Padding = new System.Windows.Forms.Padding(0, 0, 2, 2);
            this.buttonSaveUserCombinedNameCache.Size = new System.Drawing.Size(24, 23);
            this.buttonSaveUserCombinedNameCache.TabIndex = 36;
            this.toolTip.SetToolTip(this.buttonSaveUserCombinedNameCache, "Save the explizit name of the user");
            this.buttonSaveUserCombinedNameCache.UseVisualStyleBackColor = true;
            this.buttonSaveUserCombinedNameCache.Click += new System.EventHandler(this.buttonSaveUserCombinedNameCache_Click);
            // 
            // labelUserURI
            // 
            this.labelUserURI.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelUserURI, 2);
            this.labelUserURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUserURI.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.labelUserURI.Location = new System.Drawing.Point(3, 306);
            this.labelUserURI.Name = "labelUserURI";
            this.labelUserURI.Size = new System.Drawing.Size(225, 24);
            this.labelUserURI.TabIndex = 37;
            this.labelUserURI.Text = "URI of user:";
            this.labelUserURI.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // userControlModuleRelatedEntryAgentURI
            // 
            this.userControlModuleRelatedEntryAgentURI.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.userControlModuleRelatedEntryAgentURI, 2);
            this.userControlModuleRelatedEntryAgentURI.DependsOnUri = "";
            this.userControlModuleRelatedEntryAgentURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryAgentURI.Domain = "";
            this.userControlModuleRelatedEntryAgentURI.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryAgentURI.Location = new System.Drawing.Point(231, 306);
            this.userControlModuleRelatedEntryAgentURI.Margin = new System.Windows.Forms.Padding(0);
            this.userControlModuleRelatedEntryAgentURI.Module = null;
            this.userControlModuleRelatedEntryAgentURI.Name = "userControlModuleRelatedEntryAgentURI";
            this.userControlModuleRelatedEntryAgentURI.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryAgentURI.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryAgentURI.ShowInfo = false;
            this.userControlModuleRelatedEntryAgentURI.Size = new System.Drawing.Size(226, 24);
            this.userControlModuleRelatedEntryAgentURI.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryAgentURI.TabIndex = 38;
            // 
            // labelOrderProject
            // 
            this.labelOrderProject.AutoSize = true;
            this.labelOrderProject.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelOrderProject.Location = new System.Drawing.Point(696, 283);
            this.labelOrderProject.Name = "labelOrderProject";
            this.labelOrderProject.Size = new System.Drawing.Size(85, 23);
            this.labelOrderProject.TabIndex = 39;
            this.labelOrderProject.Text = "Order project by:";
            this.labelOrderProject.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radioButtonOrderProjectByName
            // 
            this.radioButtonOrderProjectByName.AutoSize = true;
            this.radioButtonOrderProjectByName.Checked = true;
            this.radioButtonOrderProjectByName.Dock = System.Windows.Forms.DockStyle.Right;
            this.radioButtonOrderProjectByName.Location = new System.Drawing.Point(792, 286);
            this.radioButtonOrderProjectByName.Name = "radioButtonOrderProjectByName";
            this.radioButtonOrderProjectByName.Size = new System.Drawing.Size(53, 17);
            this.radioButtonOrderProjectByName.TabIndex = 40;
            this.radioButtonOrderProjectByName.TabStop = true;
            this.radioButtonOrderProjectByName.Text = "Name";
            this.radioButtonOrderProjectByName.UseVisualStyleBackColor = true;
            // 
            // radioButtonOrderProjectByID
            // 
            this.radioButtonOrderProjectByID.AutoSize = true;
            this.radioButtonOrderProjectByID.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonOrderProjectByID.Location = new System.Drawing.Point(851, 286);
            this.radioButtonOrderProjectByID.Name = "radioButtonOrderProjectByID";
            this.radioButtonOrderProjectByID.Size = new System.Drawing.Size(36, 17);
            this.radioButtonOrderProjectByID.TabIndex = 41;
            this.radioButtonOrderProjectByID.Text = "ID";
            this.radioButtonOrderProjectByID.UseVisualStyleBackColor = true;
            // 
            // labelIncludedRoles
            // 
            this.labelIncludedRoles.AutoSize = true;
            this.labelIncludedRoles.Location = new System.Drawing.Point(691, 483);
            this.labelIncludedRoles.Name = "labelIncludedRoles";
            this.labelIncludedRoles.Size = new System.Drawing.Size(73, 13);
            this.labelIncludedRoles.TabIndex = 42;
            this.labelIncludedRoles.Text = "Included roles";
            // 
            // listBoxIncludedRoles
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.listBoxIncludedRoles, 3);
            this.listBoxIncludedRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxIncludedRoles.FormattingEnabled = true;
            this.listBoxIncludedRoles.IntegralHeight = false;
            this.listBoxIncludedRoles.Location = new System.Drawing.Point(691, 499);
            this.listBoxIncludedRoles.Name = "listBoxIncludedRoles";
            this.listBoxIncludedRoles.Size = new System.Drawing.Size(196, 90);
            this.listBoxIncludedRoles.TabIndex = 43;
            // 
            // FormUserAdministration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(906, 608);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormUserAdministration";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User administration";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.groupBoxUser.ResumeLayout(false);
            this.tableLayoutPanelPermissions.ResumeLayout(false);
            this.tableLayoutPanelPermissions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRolePermissions)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.ListBox listBoxDatabaseAccounts;
        private System.Windows.Forms.ListBox listBoxUserRoles;
        private System.Windows.Forms.ListBox listBoxProjectsAvailable;
        private System.Windows.Forms.ListBox listBoxProjectsNotAvailable;
        private System.Windows.Forms.ListBox listBoxUser;
        private System.Windows.Forms.ListBox listBoxRoles;
        private System.Windows.Forms.Label labelDatabaseAccounts;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelProjectsAvailable;
        private System.Windows.Forms.Label labelProjectsNotAvailable;
        private System.Windows.Forms.Label labelRoles;
        private System.Windows.Forms.Label labelUserRoles;
        private System.Windows.Forms.GroupBox groupBoxUser;
        private System.Windows.Forms.Button buttonUserAdd;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPermissions;
        private System.Windows.Forms.Button buttonUserRemove;
        private System.Windows.Forms.Button buttonProjectAdd;
        private System.Windows.Forms.Button buttonProjectRemove;
        private System.Windows.Forms.Button buttonRoleAdd;
        private System.Windows.Forms.Button buttonRoleRemove;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.TextBox textBoxCombinedNameCache;
        private System.Windows.Forms.DataGridView dataGridViewRolePermissions;
        private System.Windows.Forms.ListBox listBoxRoleMembers;
        private System.Windows.Forms.Label labelRoleMembers;
        private System.Windows.Forms.Label labelRolePermissions;
        private System.Windows.Forms.Button buttonSynchronizeUser;
        private System.Windows.Forms.Button buttonCreateUser;
        private System.Windows.Forms.Button buttonCreateProject;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonSynchronizeProjects;
        private System.Windows.Forms.Button buttonSaveUserCombinedNameCache;
        private System.Windows.Forms.Label labelUserURI;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryAgentURI;
        private System.Windows.Forms.Label labelOrderProject;
        private System.Windows.Forms.RadioButton radioButtonOrderProjectByName;
        private System.Windows.Forms.RadioButton radioButtonOrderProjectByID;
        private System.Windows.Forms.Label labelIncludedRoles;
        private System.Windows.Forms.ListBox listBoxIncludedRoles;
    }
}