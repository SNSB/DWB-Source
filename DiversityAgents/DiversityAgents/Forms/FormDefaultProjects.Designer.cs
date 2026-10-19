namespace DiversityAgents.Forms
{
    partial class FormDefaultProjects
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDefaultProjects));
            dataSetProject = new DataSetProject();
            dataGridView = new System.Windows.Forms.DataGridView();
            loginNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            combinedNameCacheDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            userURIDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            projectIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            projectProxyBindingSource = new System.Windows.Forms.BindingSource(components);
            userProxyBindingSource = new System.Windows.Forms.BindingSource(components);
            userProxyTableAdapter = new DataSetProjectTableAdapters.UserProxyTableAdapter();
            projectProxyTableAdapter = new DataSetProjectTableAdapters.ProjectProxyTableAdapter();
            helpProvider = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)dataSetProject).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)projectProxyBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)userProxyBindingSource).BeginInit();
            SuspendLayout();
            // 
            // dataSetProject
            // 
            dataSetProject.DataSetName = "DataSetProject";
            dataSetProject.Namespace = "http://tempuri.org/DataSetProject.xsd";
            dataSetProject.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dataGridView
            // 
            dataGridView.AutoGenerateColumns = false;
            dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { loginNameDataGridViewTextBoxColumn, combinedNameCacheDataGridViewTextBoxColumn, userURIDataGridViewTextBoxColumn, projectIDDataGridViewTextBoxColumn });
            dataGridView.DataSource = userProxyBindingSource;
            dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView.Location = new System.Drawing.Point(0, 0);
            dataGridView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dataGridView.Name = "dataGridView";
            dataGridView.Size = new System.Drawing.Size(488, 262);
            dataGridView.TabIndex = 0;
            // 
            // loginNameDataGridViewTextBoxColumn
            // 
            loginNameDataGridViewTextBoxColumn.DataPropertyName = "LoginName";
            loginNameDataGridViewTextBoxColumn.HeaderText = "Login name";
            loginNameDataGridViewTextBoxColumn.Name = "loginNameDataGridViewTextBoxColumn";
            loginNameDataGridViewTextBoxColumn.ReadOnly = true;
            loginNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // combinedNameCacheDataGridViewTextBoxColumn
            // 
            combinedNameCacheDataGridViewTextBoxColumn.DataPropertyName = "CombinedNameCache";
            combinedNameCacheDataGridViewTextBoxColumn.HeaderText = "User";
            combinedNameCacheDataGridViewTextBoxColumn.Name = "combinedNameCacheDataGridViewTextBoxColumn";
            combinedNameCacheDataGridViewTextBoxColumn.ReadOnly = true;
            combinedNameCacheDataGridViewTextBoxColumn.Width = 150;
            // 
            // userURIDataGridViewTextBoxColumn
            // 
            userURIDataGridViewTextBoxColumn.DataPropertyName = "UserURI";
            userURIDataGridViewTextBoxColumn.HeaderText = "User URI";
            userURIDataGridViewTextBoxColumn.Name = "userURIDataGridViewTextBoxColumn";
            userURIDataGridViewTextBoxColumn.ReadOnly = true;
            userURIDataGridViewTextBoxColumn.Visible = false;
            // 
            // projectIDDataGridViewTextBoxColumn
            // 
            projectIDDataGridViewTextBoxColumn.DataPropertyName = "ProjectID";
            projectIDDataGridViewTextBoxColumn.DataSource = projectProxyBindingSource;
            projectIDDataGridViewTextBoxColumn.DisplayMember = "Project";
            projectIDDataGridViewTextBoxColumn.HeaderText = "Project";
            projectIDDataGridViewTextBoxColumn.Name = "projectIDDataGridViewTextBoxColumn";
            projectIDDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            projectIDDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            projectIDDataGridViewTextBoxColumn.ValueMember = "ProjectID";
            projectIDDataGridViewTextBoxColumn.Width = 200;
            // 
            // projectProxyBindingSource
            // 
            projectProxyBindingSource.DataMember = "ProjectProxy";
            projectProxyBindingSource.DataSource = dataSetProject;
            // 
            // userProxyBindingSource
            // 
            userProxyBindingSource.DataMember = "UserProxy";
            userProxyBindingSource.DataSource = dataSetProject;
            // 
            // userProxyTableAdapter
            // 
            userProxyTableAdapter.ClearBeforeFill = true;
            // 
            // projectProxyTableAdapter
            // 
            projectProxyTableAdapter.ClearBeforeFill = true;
            // 
            // FormDefaultProjects
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(488, 262);
            Controls.Add(dataGridView);
            helpProvider.SetHelpKeyword(this, "agentdisplaytypes_da");
            helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormDefaultProjects";
            helpProvider.SetShowHelp(this, true);
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Set the default projects for the user";
            FormClosing += FormDefaultProjects_FormClosing;
            Load += FormDefaultProjects_Load;
            KeyDown += Form_KeyDown;
            ((System.ComponentModel.ISupportInitialize)dataSetProject).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)projectProxyBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)userProxyBindingSource).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataSetProject dataSetProject;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.BindingSource userProxyBindingSource;
        private DiversityAgents.DataSetProjectTableAdapters.UserProxyTableAdapter userProxyTableAdapter;
        private System.Windows.Forms.BindingSource projectProxyBindingSource;
        private DiversityAgents.DataSetProjectTableAdapters.ProjectProxyTableAdapter projectProxyTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn loginNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn combinedNameCacheDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userURIDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn projectIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}