namespace DiversityAgents.Forms
{
    partial class FormExternalDatabase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExternalDatabase));
            splitContainerMain = new System.Windows.Forms.SplitContainer();
            listBoxSource = new System.Windows.Forms.ListBox();
            agentExternalDatabaseBindingSource = new System.Windows.Forms.BindingSource(components);
            dataSetExternalDatasource = new Datasets.DataSetExternalDatasource();
            toolStripSource = new System.Windows.Forms.ToolStrip();
            toolStripButtonSourceAdd = new System.Windows.Forms.ToolStripButton();
            toolStripButtonSourceDelete = new System.Windows.Forms.ToolStripButton();
            tableLayoutPanelSource = new System.Windows.Forms.TableLayoutPanel();
            labelExternalDatabaseName = new System.Windows.Forms.Label();
            textBoxExternalDatabaseName = new System.Windows.Forms.TextBox();
            labelExternalDatabaseVersion = new System.Windows.Forms.Label();
            textBoxExternalDatabaseVersion = new System.Windows.Forms.TextBox();
            labelExternalDatabaseAuthors = new System.Windows.Forms.Label();
            textBoxExternalDatabaseAuthors = new System.Windows.Forms.TextBox();
            labelExternalDatabaseURI = new System.Windows.Forms.Label();
            textBoxExternalDatabaseURI = new System.Windows.Forms.TextBox();
            labelExternalDatabaseInstitution = new System.Windows.Forms.Label();
            textBoxExternalDatabaseInstitution = new System.Windows.Forms.TextBox();
            labelInternalNotes = new System.Windows.Forms.Label();
            textBoxInternalNotes = new System.Windows.Forms.TextBox();
            labelExternalAttribute_AgentID = new System.Windows.Forms.Label();
            textBoxExternalAttribute_AgentID = new System.Windows.Forms.TextBox();
            labelRights = new System.Windows.Forms.Label();
            textBoxRights = new System.Windows.Forms.TextBox();
            agentExternalDatabaseTableAdapter = new Datasets.DataSetExternalDatasourceTableAdapters.AgentExternalDatabaseTableAdapter();
            helpProvider = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)agentExternalDatabaseBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataSetExternalDatasource).BeginInit();
            toolStripSource.SuspendLayout();
            tableLayoutPanelSource.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerMain.Location = new System.Drawing.Point(0, 0);
            splitContainerMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(listBoxSource);
            splitContainerMain.Panel1.Controls.Add(toolStripSource);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(tableLayoutPanelSource);
            splitContainerMain.Size = new System.Drawing.Size(525, 268);
            splitContainerMain.SplitterDistance = 175;
            splitContainerMain.SplitterWidth = 5;
            splitContainerMain.TabIndex = 0;
            // 
            // listBoxSource
            // 
            listBoxSource.DataSource = agentExternalDatabaseBindingSource;
            listBoxSource.DisplayMember = "ExternalDatabaseName";
            listBoxSource.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxSource.FormattingEnabled = true;
            listBoxSource.ItemHeight = 15;
            listBoxSource.Location = new System.Drawing.Point(0, 0);
            listBoxSource.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxSource.Name = "listBoxSource";
            listBoxSource.Size = new System.Drawing.Size(175, 243);
            listBoxSource.TabIndex = 0;
            listBoxSource.ValueMember = "ExternalDatabaseID";
            // 
            // agentExternalDatabaseBindingSource
            // 
            agentExternalDatabaseBindingSource.DataMember = "AgentExternalDatabase";
            agentExternalDatabaseBindingSource.DataSource = dataSetExternalDatasource;
            // 
            // dataSetExternalDatasource
            // 
            dataSetExternalDatasource.DataSetName = "DataSetExternalDatasource";
            dataSetExternalDatasource.Namespace = "http://tempuri.org/DataSetExternalDatasource.xsd";
            dataSetExternalDatasource.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // toolStripSource
            // 
            toolStripSource.Dock = System.Windows.Forms.DockStyle.Bottom;
            toolStripSource.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripSource.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonSourceAdd, toolStripButtonSourceDelete });
            toolStripSource.Location = new System.Drawing.Point(0, 243);
            toolStripSource.Name = "toolStripSource";
            toolStripSource.Size = new System.Drawing.Size(175, 25);
            toolStripSource.TabIndex = 1;
            toolStripSource.Text = "toolStrip1";
            // 
            // toolStripButtonSourceAdd
            // 
            toolStripButtonSourceAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSourceAdd.Image = Properties.Resources.New;
            toolStripButtonSourceAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSourceAdd.Name = "toolStripButtonSourceAdd";
            toolStripButtonSourceAdd.Size = new System.Drawing.Size(23, 22);
            toolStripButtonSourceAdd.Text = "toolStripButton1";
            toolStripButtonSourceAdd.Click += toolStripButtonSourceAdd_Click;
            // 
            // toolStripButtonSourceDelete
            // 
            toolStripButtonSourceDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSourceDelete.Image = Properties.Resources.Delete;
            toolStripButtonSourceDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSourceDelete.Name = "toolStripButtonSourceDelete";
            toolStripButtonSourceDelete.Size = new System.Drawing.Size(23, 22);
            toolStripButtonSourceDelete.Text = "toolStripButton1";
            toolStripButtonSourceDelete.Click += toolStripButtonSourceDelete_Click;
            // 
            // tableLayoutPanelSource
            // 
            tableLayoutPanelSource.ColumnCount = 2;
            tableLayoutPanelSource.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSource.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelSource.Controls.Add(labelExternalDatabaseName, 0, 0);
            tableLayoutPanelSource.Controls.Add(textBoxExternalDatabaseName, 1, 0);
            tableLayoutPanelSource.Controls.Add(labelExternalDatabaseVersion, 0, 1);
            tableLayoutPanelSource.Controls.Add(textBoxExternalDatabaseVersion, 1, 1);
            tableLayoutPanelSource.Controls.Add(labelExternalDatabaseAuthors, 0, 2);
            tableLayoutPanelSource.Controls.Add(textBoxExternalDatabaseAuthors, 1, 2);
            tableLayoutPanelSource.Controls.Add(labelExternalDatabaseURI, 0, 4);
            tableLayoutPanelSource.Controls.Add(textBoxExternalDatabaseURI, 1, 4);
            tableLayoutPanelSource.Controls.Add(labelExternalDatabaseInstitution, 0, 5);
            tableLayoutPanelSource.Controls.Add(textBoxExternalDatabaseInstitution, 1, 5);
            tableLayoutPanelSource.Controls.Add(labelInternalNotes, 0, 6);
            tableLayoutPanelSource.Controls.Add(textBoxInternalNotes, 1, 6);
            tableLayoutPanelSource.Controls.Add(labelExternalAttribute_AgentID, 0, 7);
            tableLayoutPanelSource.Controls.Add(textBoxExternalAttribute_AgentID, 1, 7);
            tableLayoutPanelSource.Controls.Add(labelRights, 0, 3);
            tableLayoutPanelSource.Controls.Add(textBoxRights, 1, 3);
            tableLayoutPanelSource.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelSource.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelSource.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelSource.Name = "tableLayoutPanelSource";
            tableLayoutPanelSource.RowCount = 8;
            tableLayoutPanelSource.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSource.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSource.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSource.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSource.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSource.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSource.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelSource.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSource.Size = new System.Drawing.Size(345, 268);
            tableLayoutPanelSource.TabIndex = 0;
            // 
            // labelExternalDatabaseName
            // 
            labelExternalDatabaseName.AutoSize = true;
            labelExternalDatabaseName.Dock = System.Windows.Forms.DockStyle.Fill;
            labelExternalDatabaseName.Location = new System.Drawing.Point(4, 0);
            labelExternalDatabaseName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelExternalDatabaseName.Name = "labelExternalDatabaseName";
            labelExternalDatabaseName.Size = new System.Drawing.Size(68, 29);
            labelExternalDatabaseName.TabIndex = 0;
            labelExternalDatabaseName.Text = "Name";
            labelExternalDatabaseName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxExternalDatabaseName
            // 
            textBoxExternalDatabaseName.DataBindings.Add(new System.Windows.Forms.Binding("Text", agentExternalDatabaseBindingSource, "ExternalDatabaseName", true));
            textBoxExternalDatabaseName.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxExternalDatabaseName.Location = new System.Drawing.Point(80, 3);
            textBoxExternalDatabaseName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxExternalDatabaseName.Name = "textBoxExternalDatabaseName";
            textBoxExternalDatabaseName.Size = new System.Drawing.Size(261, 23);
            textBoxExternalDatabaseName.TabIndex = 1;
            // 
            // labelExternalDatabaseVersion
            // 
            labelExternalDatabaseVersion.AutoSize = true;
            labelExternalDatabaseVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            labelExternalDatabaseVersion.Location = new System.Drawing.Point(4, 29);
            labelExternalDatabaseVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelExternalDatabaseVersion.Name = "labelExternalDatabaseVersion";
            labelExternalDatabaseVersion.Size = new System.Drawing.Size(68, 29);
            labelExternalDatabaseVersion.TabIndex = 2;
            labelExternalDatabaseVersion.Text = "Version";
            labelExternalDatabaseVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxExternalDatabaseVersion
            // 
            textBoxExternalDatabaseVersion.DataBindings.Add(new System.Windows.Forms.Binding("Text", agentExternalDatabaseBindingSource, "ExternalDatabaseVersion", true));
            textBoxExternalDatabaseVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxExternalDatabaseVersion.Location = new System.Drawing.Point(80, 32);
            textBoxExternalDatabaseVersion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxExternalDatabaseVersion.Name = "textBoxExternalDatabaseVersion";
            textBoxExternalDatabaseVersion.Size = new System.Drawing.Size(261, 23);
            textBoxExternalDatabaseVersion.TabIndex = 3;
            // 
            // labelExternalDatabaseAuthors
            // 
            labelExternalDatabaseAuthors.AutoSize = true;
            labelExternalDatabaseAuthors.Dock = System.Windows.Forms.DockStyle.Fill;
            labelExternalDatabaseAuthors.Location = new System.Drawing.Point(4, 58);
            labelExternalDatabaseAuthors.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelExternalDatabaseAuthors.Name = "labelExternalDatabaseAuthors";
            labelExternalDatabaseAuthors.Size = new System.Drawing.Size(68, 29);
            labelExternalDatabaseAuthors.TabIndex = 4;
            labelExternalDatabaseAuthors.Text = "Authors";
            labelExternalDatabaseAuthors.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxExternalDatabaseAuthors
            // 
            textBoxExternalDatabaseAuthors.DataBindings.Add(new System.Windows.Forms.Binding("Text", agentExternalDatabaseBindingSource, "ExternalDatabaseAuthors", true));
            textBoxExternalDatabaseAuthors.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxExternalDatabaseAuthors.Location = new System.Drawing.Point(80, 61);
            textBoxExternalDatabaseAuthors.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxExternalDatabaseAuthors.Name = "textBoxExternalDatabaseAuthors";
            textBoxExternalDatabaseAuthors.Size = new System.Drawing.Size(261, 23);
            textBoxExternalDatabaseAuthors.TabIndex = 5;
            // 
            // labelExternalDatabaseURI
            // 
            labelExternalDatabaseURI.AutoSize = true;
            labelExternalDatabaseURI.Dock = System.Windows.Forms.DockStyle.Fill;
            labelExternalDatabaseURI.Location = new System.Drawing.Point(4, 116);
            labelExternalDatabaseURI.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelExternalDatabaseURI.Name = "labelExternalDatabaseURI";
            labelExternalDatabaseURI.Size = new System.Drawing.Size(68, 29);
            labelExternalDatabaseURI.TabIndex = 6;
            labelExternalDatabaseURI.Text = "URI";
            labelExternalDatabaseURI.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxExternalDatabaseURI
            // 
            textBoxExternalDatabaseURI.DataBindings.Add(new System.Windows.Forms.Binding("Text", agentExternalDatabaseBindingSource, "ExternalDatabaseURI", true));
            textBoxExternalDatabaseURI.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxExternalDatabaseURI.Location = new System.Drawing.Point(80, 119);
            textBoxExternalDatabaseURI.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxExternalDatabaseURI.Name = "textBoxExternalDatabaseURI";
            textBoxExternalDatabaseURI.Size = new System.Drawing.Size(261, 23);
            textBoxExternalDatabaseURI.TabIndex = 7;
            // 
            // labelExternalDatabaseInstitution
            // 
            labelExternalDatabaseInstitution.AutoSize = true;
            labelExternalDatabaseInstitution.Dock = System.Windows.Forms.DockStyle.Fill;
            labelExternalDatabaseInstitution.Location = new System.Drawing.Point(4, 145);
            labelExternalDatabaseInstitution.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelExternalDatabaseInstitution.Name = "labelExternalDatabaseInstitution";
            labelExternalDatabaseInstitution.Size = new System.Drawing.Size(68, 29);
            labelExternalDatabaseInstitution.TabIndex = 8;
            labelExternalDatabaseInstitution.Text = "Institution";
            labelExternalDatabaseInstitution.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxExternalDatabaseInstitution
            // 
            textBoxExternalDatabaseInstitution.DataBindings.Add(new System.Windows.Forms.Binding("Text", agentExternalDatabaseBindingSource, "ExternalDatabaseInstitution", true));
            textBoxExternalDatabaseInstitution.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxExternalDatabaseInstitution.Location = new System.Drawing.Point(80, 148);
            textBoxExternalDatabaseInstitution.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxExternalDatabaseInstitution.Name = "textBoxExternalDatabaseInstitution";
            textBoxExternalDatabaseInstitution.Size = new System.Drawing.Size(261, 23);
            textBoxExternalDatabaseInstitution.TabIndex = 9;
            // 
            // labelInternalNotes
            // 
            labelInternalNotes.AutoSize = true;
            labelInternalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            labelInternalNotes.Location = new System.Drawing.Point(4, 181);
            labelInternalNotes.Margin = new System.Windows.Forms.Padding(4, 7, 4, 0);
            labelInternalNotes.Name = "labelInternalNotes";
            labelInternalNotes.Size = new System.Drawing.Size(68, 58);
            labelInternalNotes.TabIndex = 10;
            labelInternalNotes.Text = "Notes";
            labelInternalNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxInternalNotes
            // 
            textBoxInternalNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", agentExternalDatabaseBindingSource, "InternalNotes", true));
            textBoxInternalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxInternalNotes.Location = new System.Drawing.Point(80, 177);
            textBoxInternalNotes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxInternalNotes.Multiline = true;
            textBoxInternalNotes.Name = "textBoxInternalNotes";
            textBoxInternalNotes.Size = new System.Drawing.Size(261, 59);
            textBoxInternalNotes.TabIndex = 11;
            // 
            // labelExternalAttribute_AgentID
            // 
            labelExternalAttribute_AgentID.AutoSize = true;
            labelExternalAttribute_AgentID.Dock = System.Windows.Forms.DockStyle.Fill;
            labelExternalAttribute_AgentID.Location = new System.Drawing.Point(4, 239);
            labelExternalAttribute_AgentID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelExternalAttribute_AgentID.Name = "labelExternalAttribute_AgentID";
            labelExternalAttribute_AgentID.Size = new System.Drawing.Size(68, 29);
            labelExternalAttribute_AgentID.TabIndex = 12;
            labelExternalAttribute_AgentID.Text = "Attribute ID";
            labelExternalAttribute_AgentID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxExternalAttribute_AgentID
            // 
            textBoxExternalAttribute_AgentID.DataBindings.Add(new System.Windows.Forms.Binding("Text", agentExternalDatabaseBindingSource, "ExternalAttribute_AgentID", true));
            textBoxExternalAttribute_AgentID.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxExternalAttribute_AgentID.Location = new System.Drawing.Point(80, 242);
            textBoxExternalAttribute_AgentID.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxExternalAttribute_AgentID.Name = "textBoxExternalAttribute_AgentID";
            textBoxExternalAttribute_AgentID.Size = new System.Drawing.Size(261, 23);
            textBoxExternalAttribute_AgentID.TabIndex = 13;
            // 
            // labelRights
            // 
            labelRights.AutoSize = true;
            labelRights.Dock = System.Windows.Forms.DockStyle.Fill;
            labelRights.Location = new System.Drawing.Point(4, 87);
            labelRights.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelRights.Name = "labelRights";
            labelRights.Size = new System.Drawing.Size(68, 29);
            labelRights.TabIndex = 14;
            labelRights.Text = "Rights";
            labelRights.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxRights
            // 
            textBoxRights.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxRights.Location = new System.Drawing.Point(80, 90);
            textBoxRights.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxRights.Name = "textBoxRights";
            textBoxRights.Size = new System.Drawing.Size(261, 23);
            textBoxRights.TabIndex = 15;
            // 
            // agentExternalDatabaseTableAdapter
            // 
            agentExternalDatabaseTableAdapter.ClearBeforeFill = true;
            // 
            // FormExternalDatabase
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(525, 268);
            Controls.Add(splitContainerMain);
            helpProvider.SetHelpKeyword(this, "externaldata_da");
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormExternalDatabase";
            helpProvider.SetShowHelp(this, true);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "External database";
            FormClosing += FormExternalDatabase_FormClosing;
            Load += FormExternalDatabase_Load;
            HelpRequested += FormExternalDatabase_HelpRequested;
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel1.PerformLayout();
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)agentExternalDatabaseBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataSetExternalDatasource).EndInit();
            toolStripSource.ResumeLayout(false);
            toolStripSource.PerformLayout();
            tableLayoutPanelSource.ResumeLayout(false);
            tableLayoutPanelSource.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.ListBox listBoxSource;
        private System.Windows.Forms.ToolStrip toolStripSource;
        private System.Windows.Forms.ToolStripButton toolStripButtonSourceAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonSourceDelete;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSource;
        private Datasets.DataSetExternalDatasource dataSetExternalDatasource;
        private System.Windows.Forms.BindingSource agentExternalDatabaseBindingSource;
        private Datasets.DataSetExternalDatasourceTableAdapters.AgentExternalDatabaseTableAdapter agentExternalDatabaseTableAdapter;
        private System.Windows.Forms.Label labelExternalDatabaseName;
        private System.Windows.Forms.TextBox textBoxExternalDatabaseName;
        private System.Windows.Forms.Label labelExternalDatabaseVersion;
        private System.Windows.Forms.TextBox textBoxExternalDatabaseVersion;
        private System.Windows.Forms.Label labelExternalDatabaseAuthors;
        private System.Windows.Forms.TextBox textBoxExternalDatabaseAuthors;
        private System.Windows.Forms.Label labelExternalDatabaseURI;
        private System.Windows.Forms.TextBox textBoxExternalDatabaseURI;
        private System.Windows.Forms.Label labelExternalDatabaseInstitution;
        private System.Windows.Forms.TextBox textBoxExternalDatabaseInstitution;
        private System.Windows.Forms.Label labelInternalNotes;
        private System.Windows.Forms.TextBox textBoxInternalNotes;
        private System.Windows.Forms.Label labelExternalAttribute_AgentID;
        private System.Windows.Forms.TextBox textBoxExternalAttribute_AgentID;
        private System.Windows.Forms.Label labelRights;
        private System.Windows.Forms.TextBox textBoxRights;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}