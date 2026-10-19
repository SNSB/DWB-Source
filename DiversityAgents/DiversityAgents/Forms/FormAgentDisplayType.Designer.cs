namespace DiversityAgents.Forms
{
    partial class FormAgentDisplayType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAgentDisplayType));
            dataSetProject = new DataSetProject();
            dataGridView = new System.Windows.Forms.DataGridView();
            projectIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            projectDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            agentNameDisplayTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            agentNameDisplayTypeEnumBindingSource = new System.Windows.Forms.BindingSource(components);
            projectProxyBindingSource = new System.Windows.Forms.BindingSource(components);
            projectProxyTableAdapter = new DataSetProjectTableAdapters.ProjectProxyTableAdapter();
            agentNameDisplayType_EnumTableAdapter = new DataSetProjectTableAdapters.AgentNameDisplayType_EnumTableAdapter();
            helpProvider = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)dataSetProject).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)agentNameDisplayTypeEnumBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)projectProxyBindingSource).BeginInit();
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
            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { projectIDDataGridViewTextBoxColumn, projectDataGridViewTextBoxColumn, agentNameDisplayTypeDataGridViewTextBoxColumn });
            dataGridView.DataSource = projectProxyBindingSource;
            dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView.Location = new System.Drawing.Point(0, 0);
            dataGridView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dataGridView.Name = "dataGridView";
            dataGridView.Size = new System.Drawing.Size(544, 205);
            dataGridView.TabIndex = 0;
            // 
            // projectIDDataGridViewTextBoxColumn
            // 
            projectIDDataGridViewTextBoxColumn.DataPropertyName = "ProjectID";
            projectIDDataGridViewTextBoxColumn.HeaderText = "ProjectID";
            projectIDDataGridViewTextBoxColumn.Name = "projectIDDataGridViewTextBoxColumn";
            projectIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // projectDataGridViewTextBoxColumn
            // 
            projectDataGridViewTextBoxColumn.DataPropertyName = "Project";
            projectDataGridViewTextBoxColumn.HeaderText = "Project";
            projectDataGridViewTextBoxColumn.Name = "projectDataGridViewTextBoxColumn";
            projectDataGridViewTextBoxColumn.Width = 200;
            // 
            // agentNameDisplayTypeDataGridViewTextBoxColumn
            // 
            agentNameDisplayTypeDataGridViewTextBoxColumn.DataPropertyName = "AgentNameDisplayType";
            agentNameDisplayTypeDataGridViewTextBoxColumn.DataSource = agentNameDisplayTypeEnumBindingSource;
            agentNameDisplayTypeDataGridViewTextBoxColumn.DisplayMember = "DisplayText";
            agentNameDisplayTypeDataGridViewTextBoxColumn.HeaderText = "AgentNameDisplayType";
            agentNameDisplayTypeDataGridViewTextBoxColumn.Name = "agentNameDisplayTypeDataGridViewTextBoxColumn";
            agentNameDisplayTypeDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            agentNameDisplayTypeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            agentNameDisplayTypeDataGridViewTextBoxColumn.ValueMember = "Code";
            agentNameDisplayTypeDataGridViewTextBoxColumn.Width = 200;
            // 
            // agentNameDisplayTypeEnumBindingSource
            // 
            agentNameDisplayTypeEnumBindingSource.DataMember = "AgentNameDisplayType_Enum";
            agentNameDisplayTypeEnumBindingSource.DataSource = dataSetProject;
            // 
            // projectProxyBindingSource
            // 
            projectProxyBindingSource.DataMember = "ProjectProxy";
            projectProxyBindingSource.DataSource = dataSetProject;
            // 
            // projectProxyTableAdapter
            // 
            projectProxyTableAdapter.ClearBeforeFill = true;
            // 
            // agentNameDisplayType_EnumTableAdapter
            // 
            agentNameDisplayType_EnumTableAdapter.ClearBeforeFill = true;
            // 
            // FormAgentDisplayType
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(544, 205);
            Controls.Add(dataGridView);
            helpProvider.SetHelpKeyword(this, "agentdisplaytypes_da");
            helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormAgentDisplayType";
            helpProvider.SetShowHelp(this, true);
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Edit the display types for the agents within the projects";
            FormClosing += FormAgentDisplayType_FormClosing;
            Load += FormAgentDisplayType_Load;
            KeyDown += Form_KeyDown;
            ((System.ComponentModel.ISupportInitialize)dataSetProject).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)agentNameDisplayTypeEnumBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)projectProxyBindingSource).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataSetProject dataSetProject;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.BindingSource projectProxyBindingSource;
        private DiversityAgents.DataSetProjectTableAdapters.ProjectProxyTableAdapter projectProxyTableAdapter;
        private System.Windows.Forms.BindingSource agentNameDisplayTypeEnumBindingSource;
        private DiversityAgents.DataSetProjectTableAdapters.AgentNameDisplayType_EnumTableAdapter agentNameDisplayType_EnumTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn projectIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn projectDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn agentNameDisplayTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}