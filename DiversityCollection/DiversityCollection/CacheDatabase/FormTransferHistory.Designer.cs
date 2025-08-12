namespace DiversityCollection.CacheDatabase
{
    partial class FormTransferHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTransferHistory));
            splitContainerMain = new System.Windows.Forms.SplitContainer();
            listBoxTransfer = new System.Windows.Forms.ListBox();
            transferHistoryBindingSource = new System.Windows.Forms.BindingSource(components);
            dataSetTransferHistoryBindingSource = new System.Windows.Forms.BindingSource(components);
            dataSetTransferHistory = new DataSetTransferHistory();
            tableLayoutPanelTransfer = new System.Windows.Forms.TableLayoutPanel();
            labelResponsible = new System.Windows.Forms.Label();
            textBoxResponsible = new System.Windows.Forms.TextBox();
            labelSettings = new System.Windows.Forms.Label();
            treeViewSettings = new System.Windows.Forms.TreeView();
            labelPackage = new System.Windows.Forms.Label();
            textBoxPackage = new System.Windows.Forms.TextBox();
            tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            labelSource = new System.Windows.Forms.Label();
            labelDatabase = new System.Windows.Forms.Label();
            textBoxSource = new System.Windows.Forms.TextBox();
            textBoxDatabase = new System.Windows.Forms.TextBox();
            buttonFeedback = new System.Windows.Forms.Button();
            transferHistoryTableAdapter = new DiversityCollection.CacheDatabase.DataSetTransferHistoryTableAdapters.TransferHistoryTableAdapter();
            toolTip = new System.Windows.Forms.ToolTip(components);
            helpProvider = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)transferHistoryBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataSetTransferHistoryBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataSetTransferHistory).BeginInit();
            tableLayoutPanelTransfer.SuspendLayout();
            tableLayoutPanelMain.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainerMain
            // 
            tableLayoutPanelMain.SetColumnSpan(splitContainerMain, 3);
            splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerMain.Location = new System.Drawing.Point(4, 65);
            splitContainerMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(listBoxTransfer);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(tableLayoutPanelTransfer);
            splitContainerMain.Size = new System.Drawing.Size(593, 337);
            splitContainerMain.SplitterDistance = 196;
            splitContainerMain.SplitterWidth = 5;
            splitContainerMain.TabIndex = 0;
            // 
            // listBoxTransfer
            // 
            listBoxTransfer.DataSource = transferHistoryBindingSource;
            listBoxTransfer.DisplayMember = "TransferDate";
            listBoxTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxTransfer.FormattingEnabled = true;
            listBoxTransfer.ItemHeight = 15;
            listBoxTransfer.Location = new System.Drawing.Point(0, 0);
            listBoxTransfer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxTransfer.Name = "listBoxTransfer";
            listBoxTransfer.Size = new System.Drawing.Size(196, 337);
            listBoxTransfer.TabIndex = 0;
            listBoxTransfer.ValueMember = "TransferDate";
            listBoxTransfer.SelectedIndexChanged += listBoxTransfer_SelectedIndexChanged;
            // 
            // transferHistoryBindingSource
            // 
            transferHistoryBindingSource.DataMember = "TransferHistory";
            transferHistoryBindingSource.DataSource = dataSetTransferHistoryBindingSource;
            // 
            // dataSetTransferHistoryBindingSource
            // 
            dataSetTransferHistoryBindingSource.DataSource = dataSetTransferHistory;
            dataSetTransferHistoryBindingSource.Position = 0;
            // 
            // dataSetTransferHistory
            // 
            dataSetTransferHistory.DataSetName = "DataSetTransferHistory";
            dataSetTransferHistory.Namespace = "http://tempuri.org/DataSetTransferHistory.xsd";
            dataSetTransferHistory.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tableLayoutPanelTransfer
            // 
            tableLayoutPanelTransfer.ColumnCount = 2;
            tableLayoutPanelTransfer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelTransfer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelTransfer.Controls.Add(labelResponsible, 0, 0);
            tableLayoutPanelTransfer.Controls.Add(textBoxResponsible, 1, 0);
            tableLayoutPanelTransfer.Controls.Add(labelSettings, 0, 2);
            tableLayoutPanelTransfer.Controls.Add(treeViewSettings, 1, 2);
            tableLayoutPanelTransfer.Controls.Add(labelPackage, 0, 1);
            tableLayoutPanelTransfer.Controls.Add(textBoxPackage, 1, 1);
            tableLayoutPanelTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelTransfer.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelTransfer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelTransfer.Name = "tableLayoutPanelTransfer";
            tableLayoutPanelTransfer.RowCount = 3;
            tableLayoutPanelTransfer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelTransfer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelTransfer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelTransfer.Size = new System.Drawing.Size(392, 337);
            tableLayoutPanelTransfer.TabIndex = 0;
            // 
            // labelResponsible
            // 
            labelResponsible.AutoSize = true;
            labelResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            labelResponsible.Location = new System.Drawing.Point(4, 0);
            labelResponsible.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelResponsible.Name = "labelResponsible";
            labelResponsible.Size = new System.Drawing.Size(70, 29);
            labelResponsible.TabIndex = 0;
            labelResponsible.Text = "Responsible";
            labelResponsible.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxResponsible
            // 
            textBoxResponsible.DataBindings.Add(new System.Windows.Forms.Binding("Text", transferHistoryBindingSource, "Responsible", true));
            textBoxResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxResponsible.Location = new System.Drawing.Point(82, 3);
            textBoxResponsible.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxResponsible.Name = "textBoxResponsible";
            textBoxResponsible.ReadOnly = true;
            textBoxResponsible.Size = new System.Drawing.Size(306, 23);
            textBoxResponsible.TabIndex = 1;
            // 
            // labelSettings
            // 
            labelSettings.AutoSize = true;
            labelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            labelSettings.Location = new System.Drawing.Point(4, 65);
            labelSettings.Margin = new System.Windows.Forms.Padding(4, 7, 4, 0);
            labelSettings.Name = "labelSettings";
            labelSettings.Size = new System.Drawing.Size(70, 272);
            labelSettings.TabIndex = 2;
            labelSettings.Text = "Settings";
            labelSettings.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // treeViewSettings
            // 
            treeViewSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            treeViewSettings.Location = new System.Drawing.Point(82, 61);
            treeViewSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            treeViewSettings.Name = "treeViewSettings";
            treeViewSettings.Size = new System.Drawing.Size(306, 273);
            treeViewSettings.TabIndex = 4;
            // 
            // labelPackage
            // 
            labelPackage.AutoSize = true;
            labelPackage.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPackage.Location = new System.Drawing.Point(4, 29);
            labelPackage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelPackage.Name = "labelPackage";
            labelPackage.Size = new System.Drawing.Size(70, 29);
            labelPackage.TabIndex = 5;
            labelPackage.Text = "Package";
            labelPackage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPackage
            // 
            textBoxPackage.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxPackage.Location = new System.Drawing.Point(82, 32);
            textBoxPackage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxPackage.Name = "textBoxPackage";
            textBoxPackage.ReadOnly = true;
            textBoxPackage.Size = new System.Drawing.Size(306, 23);
            textBoxPackage.TabIndex = 6;
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 3;
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelMain.Controls.Add(splitContainerMain, 0, 2);
            tableLayoutPanelMain.Controls.Add(labelSource, 0, 0);
            tableLayoutPanelMain.Controls.Add(labelDatabase, 0, 1);
            tableLayoutPanelMain.Controls.Add(textBoxSource, 1, 0);
            tableLayoutPanelMain.Controls.Add(textBoxDatabase, 1, 1);
            tableLayoutPanelMain.Controls.Add(buttonFeedback, 2, 0);
            tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(tableLayoutPanelMain, "cachedatabase_transfer_dc#history");
            tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 3;
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            helpProvider.SetShowHelp(tableLayoutPanelMain, true);
            tableLayoutPanelMain.Size = new System.Drawing.Size(601, 405);
            tableLayoutPanelMain.TabIndex = 1;
            // 
            // labelSource
            // 
            labelSource.AutoSize = true;
            labelSource.Dock = System.Windows.Forms.DockStyle.Fill;
            labelSource.Location = new System.Drawing.Point(4, 0);
            labelSource.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelSource.Name = "labelSource";
            labelSource.Size = new System.Drawing.Size(55, 33);
            labelSource.TabIndex = 1;
            labelSource.Text = "Source";
            labelSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelDatabase
            // 
            labelDatabase.AutoSize = true;
            labelDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            labelDatabase.Location = new System.Drawing.Point(4, 33);
            labelDatabase.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelDatabase.Name = "labelDatabase";
            labelDatabase.Size = new System.Drawing.Size(55, 29);
            labelDatabase.TabIndex = 2;
            labelDatabase.Text = "Database";
            labelDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            labelDatabase.Visible = false;
            // 
            // textBoxSource
            // 
            textBoxSource.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxSource.Location = new System.Drawing.Point(67, 3);
            textBoxSource.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxSource.Name = "textBoxSource";
            textBoxSource.ReadOnly = true;
            textBoxSource.Size = new System.Drawing.Size(507, 23);
            textBoxSource.TabIndex = 3;
            // 
            // textBoxDatabase
            // 
            tableLayoutPanelMain.SetColumnSpan(textBoxDatabase, 2);
            textBoxDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxDatabase.Location = new System.Drawing.Point(67, 36);
            textBoxDatabase.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxDatabase.Name = "textBoxDatabase";
            textBoxDatabase.ReadOnly = true;
            textBoxDatabase.Size = new System.Drawing.Size(530, 23);
            textBoxDatabase.TabIndex = 4;
            textBoxDatabase.Visible = false;
            // 
            // buttonFeedback
            // 
            buttonFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonFeedback.FlatAppearance.BorderSize = 0;
            buttonFeedback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonFeedback.Image = Resource.Feedback;
            buttonFeedback.Location = new System.Drawing.Point(578, 0);
            buttonFeedback.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            buttonFeedback.Name = "buttonFeedback";
            buttonFeedback.Size = new System.Drawing.Size(19, 33);
            buttonFeedback.TabIndex = 5;
            toolTip.SetToolTip(buttonFeedback, "Send a feedback to the software developer");
            buttonFeedback.UseVisualStyleBackColor = true;
            buttonFeedback.Click += buttonFeedback_Click;
            // 
            // transferHistoryTableAdapter
            // 
            transferHistoryTableAdapter.ClearBeforeFill = true;
            // 
            // FormTransferHistory
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(601, 405);
            Controls.Add(tableLayoutPanelMain);
            helpProvider.SetHelpKeyword(this, "cachedatabase_transfer_dc#history");
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormTransferHistory";
            helpProvider.SetShowHelp(this, true);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Transfer History";
            Load += FormTransferHistory_Load;
            KeyDown += Form_KeyDown;
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)transferHistoryBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataSetTransferHistoryBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataSetTransferHistory).EndInit();
            tableLayoutPanelTransfer.ResumeLayout(false);
            tableLayoutPanelTransfer.PerformLayout();
            tableLayoutPanelMain.ResumeLayout(false);
            tableLayoutPanelMain.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.ListBox listBoxTransfer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTransfer;
        private System.Windows.Forms.Label labelResponsible;
        private System.Windows.Forms.TextBox textBoxResponsible;
        private System.Windows.Forms.Label labelSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.TextBox textBoxSource;
        private System.Windows.Forms.TextBox textBoxDatabase;
        private System.Windows.Forms.BindingSource dataSetTransferHistoryBindingSource;
        private DataSetTransferHistory dataSetTransferHistory;
        private System.Windows.Forms.BindingSource transferHistoryBindingSource;
        private DataSetTransferHistoryTableAdapters.TransferHistoryTableAdapter transferHistoryTableAdapter;
        private System.Windows.Forms.TreeView treeViewSettings;
        private System.Windows.Forms.Label labelPackage;
        private System.Windows.Forms.TextBox textBoxPackage;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}