namespace DiversityCollection.Forms
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSynchronisation));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelSource = new System.Windows.Forms.Label();
            this.comboBoxSource = new System.Windows.Forms.ComboBox();
            this.labelDestination = new System.Windows.Forms.Label();
            this.comboBoxDestination = new System.Windows.Forms.ComboBox();
            this.comboBoxTransfer = new System.Windows.Forms.ComboBox();
            this.labelTransferedData = new System.Windows.Forms.Label();
            this.buttonDestination = new System.Windows.Forms.Button();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.labelDestinationConnection = new System.Windows.Forms.Label();
            this.labelSourceConnection = new System.Windows.Forms.Label();
            this.textBoxSourceConnection = new System.Windows.Forms.TextBox();
            this.textBoxDestinationConnection = new System.Windows.Forms.TextBox();
            this.buttonTransfer = new System.Windows.Forms.Button();
            this.buttonCleanDestination = new System.Windows.Forms.Button();
            this.buttonSource = new System.Windows.Forms.Button();
            this.listBoxSpecimen = new System.Windows.Forms.ListBox();
            this.labelWhatIsToBeDone = new System.Windows.Forms.Label();
            this.panelTableList = new System.Windows.Forms.Panel();
            this.buttonReverseDirection = new System.Windows.Forms.Button();
            this.checkBoxRestrictToRelatedData = new System.Windows.Forms.CheckBox();
            this.checkBoxRestrictToProject = new System.Windows.Forms.CheckBox();
            this.comboBoxProject = new System.Windows.Forms.ComboBox();
            this.buttonSelectAllTables = new System.Windows.Forms.Button();
            this.buttonSelectNoTable = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.openFileDialogCompactDB = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 8;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelMain.Controls.Add(this.labelSource, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.comboBoxSource, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.labelDestination, 6, 0);
            this.tableLayoutPanelMain.Controls.Add(this.comboBoxDestination, 7, 0);
            this.tableLayoutPanelMain.Controls.Add(this.comboBoxTransfer, 4, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelTransferedData, 3, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonDestination, 6, 4);
            this.tableLayoutPanelMain.Controls.Add(this.labelDestinationConnection, 7, 4);
            this.tableLayoutPanelMain.Controls.Add(this.labelSourceConnection, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxSourceConnection, 0, 5);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxDestinationConnection, 7, 5);
            this.tableLayoutPanelMain.Controls.Add(this.buttonTransfer, 3, 4);
            this.tableLayoutPanelMain.Controls.Add(this.buttonCleanDestination, 7, 6);
            this.tableLayoutPanelMain.Controls.Add(this.buttonSource, 2, 4);
            this.tableLayoutPanelMain.Controls.Add(this.listBoxSpecimen, 1, 6);
            this.tableLayoutPanelMain.Controls.Add(this.labelWhatIsToBeDone, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.panelTableList, 2, 6);
            this.tableLayoutPanelMain.Controls.Add(this.buttonReverseDirection, 4, 0);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxRestrictToRelatedData, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxRestrictToProject, 2, 2);
            this.tableLayoutPanelMain.Controls.Add(this.comboBoxProject, 4, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonSelectAllTables, 2, 7);
            this.tableLayoutPanelMain.Controls.Add(this.buttonSelectNoTable, 4, 7);
            this.tableLayoutPanelMain.Controls.Add(this.buttonReset, 6, 7);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 8;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(829, 480);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.labelSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSource.Location = new System.Drawing.Point(3, 0);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(44, 29);
            this.labelSource.TabIndex = 0;
            this.labelSource.Text = "Source:";
            this.labelSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxSource
            // 
            this.comboBoxSource.BackColor = System.Drawing.Color.Pink;
            this.tableLayoutPanelMain.SetColumnSpan(this.comboBoxSource, 2);
            this.comboBoxSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxSource.FormattingEnabled = true;
            this.comboBoxSource.Location = new System.Drawing.Point(53, 3);
            this.comboBoxSource.Name = "comboBoxSource";
            this.comboBoxSource.Size = new System.Drawing.Size(197, 21);
            this.comboBoxSource.TabIndex = 1;
            this.comboBoxSource.Text = "select source type";
            this.comboBoxSource.SelectedIndexChanged += new System.EventHandler(this.comboBoxSource_SelectedIndexChanged);
            // 
            // labelDestination
            // 
            this.labelDestination.AutoSize = true;
            this.labelDestination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDestination.Location = new System.Drawing.Point(563, 0);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(75, 29);
            this.labelDestination.TabIndex = 2;
            this.labelDestination.Text = "Destination:";
            this.labelDestination.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxDestination
            // 
            this.comboBoxDestination.BackColor = System.Drawing.Color.Pink;
            this.comboBoxDestination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxDestination.FormattingEnabled = true;
            this.comboBoxDestination.Location = new System.Drawing.Point(644, 3);
            this.comboBoxDestination.Name = "comboBoxDestination";
            this.comboBoxDestination.Size = new System.Drawing.Size(182, 21);
            this.comboBoxDestination.TabIndex = 3;
            this.comboBoxDestination.Text = "Select destination type";
            this.comboBoxDestination.SelectedIndexChanged += new System.EventHandler(this.comboBoxDestination_SelectedIndexChanged);
            // 
            // comboBoxTransfer
            // 
            this.comboBoxTransfer.BackColor = System.Drawing.Color.Pink;
            this.comboBoxTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxTransfer.FormattingEnabled = true;
            this.comboBoxTransfer.Location = new System.Drawing.Point(317, 32);
            this.comboBoxTransfer.Name = "comboBoxTransfer";
            this.comboBoxTransfer.Size = new System.Drawing.Size(179, 21);
            this.comboBoxTransfer.TabIndex = 4;
            this.comboBoxTransfer.Text = "Select transfered data";
            this.comboBoxTransfer.SelectedIndexChanged += new System.EventHandler(this.comboBoxTransfer_SelectedIndexChanged);
            // 
            // labelTransferedData
            // 
            this.labelTransferedData.AutoSize = true;
            this.labelTransferedData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTransferedData.Location = new System.Drawing.Point(256, 29);
            this.labelTransferedData.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelTransferedData.Name = "labelTransferedData";
            this.labelTransferedData.Size = new System.Drawing.Size(58, 27);
            this.labelTransferedData.TabIndex = 5;
            this.labelTransferedData.Text = "Transfer:";
            this.labelTransferedData.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonDestination
            // 
            this.buttonDestination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDestination.ImageList = this.imageList;
            this.buttonDestination.Location = new System.Drawing.Point(563, 126);
            this.buttonDestination.Name = "buttonDestination";
            this.tableLayoutPanelMain.SetRowSpan(this.buttonDestination, 2);
            this.buttonDestination.Size = new System.Drawing.Size(75, 74);
            this.buttonDestination.TabIndex = 8;
            this.buttonDestination.UseVisualStyleBackColor = true;
            this.buttonDestination.Visible = false;
            this.buttonDestination.Click += new System.EventHandler(this.buttonDestination_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Server.ico");
            this.imageList.Images.SetKeyName(1, "Labtop.ico");
            this.imageList.Images.SetKeyName(2, "MobileDevice.ico");
            this.imageList.Images.SetKeyName(3, "Arrow.ico");
            this.imageList.Images.SetKeyName(4, "ReplicationList.ico");
            // 
            // labelDestinationConnection
            // 
            this.labelDestinationConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDestinationConnection.Location = new System.Drawing.Point(644, 123);
            this.labelDestinationConnection.Name = "labelDestinationConnection";
            this.labelDestinationConnection.Size = new System.Drawing.Size(182, 13);
            this.labelDestinationConnection.TabIndex = 9;
            this.labelDestinationConnection.Text = "Destination connection";
            this.labelDestinationConnection.Visible = false;
            // 
            // labelSourceConnection
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.labelSourceConnection, 2);
            this.labelSourceConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSourceConnection.Location = new System.Drawing.Point(3, 123);
            this.labelSourceConnection.Name = "labelSourceConnection";
            this.labelSourceConnection.Size = new System.Drawing.Size(167, 13);
            this.labelSourceConnection.TabIndex = 7;
            this.labelSourceConnection.Text = "Source connection";
            this.labelSourceConnection.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelSourceConnection.Visible = false;
            // 
            // textBoxSourceConnection
            // 
            this.textBoxSourceConnection.BackColor = System.Drawing.Color.Pink;
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxSourceConnection, 2);
            this.textBoxSourceConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSourceConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSourceConnection.ForeColor = System.Drawing.Color.Blue;
            this.textBoxSourceConnection.Location = new System.Drawing.Point(3, 139);
            this.textBoxSourceConnection.Multiline = true;
            this.textBoxSourceConnection.Name = "textBoxSourceConnection";
            this.textBoxSourceConnection.ReadOnly = true;
            this.textBoxSourceConnection.Size = new System.Drawing.Size(167, 61);
            this.textBoxSourceConnection.TabIndex = 10;
            this.textBoxSourceConnection.Text = "Please connect to source";
            this.textBoxSourceConnection.Visible = false;
            // 
            // textBoxDestinationConnection
            // 
            this.textBoxDestinationConnection.BackColor = System.Drawing.Color.Pink;
            this.textBoxDestinationConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDestinationConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDestinationConnection.ForeColor = System.Drawing.Color.Blue;
            this.textBoxDestinationConnection.Location = new System.Drawing.Point(644, 139);
            this.textBoxDestinationConnection.Multiline = true;
            this.textBoxDestinationConnection.Name = "textBoxDestinationConnection";
            this.textBoxDestinationConnection.ReadOnly = true;
            this.textBoxDestinationConnection.Size = new System.Drawing.Size(182, 61);
            this.textBoxDestinationConnection.TabIndex = 11;
            this.textBoxDestinationConnection.Text = "Please connect to destination";
            this.textBoxDestinationConnection.Visible = false;
            // 
            // buttonTransfer
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonTransfer, 3);
            this.buttonTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTransfer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonTransfer.ImageIndex = 4;
            this.buttonTransfer.ImageList = this.imageList;
            this.buttonTransfer.Location = new System.Drawing.Point(256, 126);
            this.buttonTransfer.Name = "buttonTransfer";
            this.tableLayoutPanelMain.SetRowSpan(this.buttonTransfer, 2);
            this.buttonTransfer.Size = new System.Drawing.Size(301, 74);
            this.buttonTransfer.TabIndex = 13;
            this.buttonTransfer.Tag = "List";
            this.buttonTransfer.Text = "List Tables";
            this.buttonTransfer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonTransfer.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.buttonTransfer.UseVisualStyleBackColor = true;
            this.buttonTransfer.Visible = false;
            this.buttonTransfer.Click += new System.EventHandler(this.buttonTransfer_Click);
            // 
            // buttonCleanDestination
            // 
            this.buttonCleanDestination.Image = global::DiversityCollection.Resource.Delete;
            this.buttonCleanDestination.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonCleanDestination.Location = new System.Drawing.Point(644, 206);
            this.buttonCleanDestination.Name = "buttonCleanDestination";
            this.buttonCleanDestination.Size = new System.Drawing.Size(117, 37);
            this.buttonCleanDestination.TabIndex = 14;
            this.buttonCleanDestination.Text = "Clean destination";
            this.buttonCleanDestination.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonCleanDestination.UseVisualStyleBackColor = true;
            this.buttonCleanDestination.Visible = false;
            this.buttonCleanDestination.Click += new System.EventHandler(this.buttonCleanDestination_Click);
            // 
            // buttonSource
            // 
            this.buttonSource.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonSource.ImageList = this.imageList;
            this.buttonSource.Location = new System.Drawing.Point(176, 126);
            this.buttonSource.Name = "buttonSource";
            this.tableLayoutPanelMain.SetRowSpan(this.buttonSource, 2);
            this.buttonSource.Size = new System.Drawing.Size(74, 74);
            this.buttonSource.TabIndex = 15;
            this.buttonSource.UseVisualStyleBackColor = true;
            this.buttonSource.Visible = false;
            this.buttonSource.Click += new System.EventHandler(this.buttonSource_Click);
            // 
            // listBoxSpecimen
            // 
            this.listBoxSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxSpecimen.FormattingEnabled = true;
            this.listBoxSpecimen.IntegralHeight = false;
            this.listBoxSpecimen.Location = new System.Drawing.Point(53, 206);
            this.listBoxSpecimen.Name = "listBoxSpecimen";
            this.tableLayoutPanelMain.SetRowSpan(this.listBoxSpecimen, 2);
            this.listBoxSpecimen.Size = new System.Drawing.Size(117, 271);
            this.listBoxSpecimen.TabIndex = 16;
            this.listBoxSpecimen.Visible = false;
            // 
            // labelWhatIsToBeDone
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.labelWhatIsToBeDone, 8);
            this.labelWhatIsToBeDone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWhatIsToBeDone.Location = new System.Drawing.Point(3, 83);
            this.labelWhatIsToBeDone.Name = "labelWhatIsToBeDone";
            this.labelWhatIsToBeDone.Size = new System.Drawing.Size(823, 40);
            this.labelWhatIsToBeDone.TabIndex = 17;
            this.labelWhatIsToBeDone.Text = "Please select the datasource, the type of the transfered data and the destination" +
    "";
            this.labelWhatIsToBeDone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTableList
            // 
            this.panelTableList.AutoScroll = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.panelTableList, 5);
            this.panelTableList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTableList.Location = new System.Drawing.Point(176, 206);
            this.panelTableList.Name = "panelTableList";
            this.panelTableList.Size = new System.Drawing.Size(462, 241);
            this.panelTableList.TabIndex = 18;
            // 
            // buttonReverseDirection
            // 
            this.buttonReverseDirection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonReverseDirection.Location = new System.Drawing.Point(317, 3);
            this.buttonReverseDirection.Name = "buttonReverseDirection";
            this.buttonReverseDirection.Size = new System.Drawing.Size(179, 23);
            this.buttonReverseDirection.TabIndex = 19;
            this.buttonReverseDirection.Text = "<- Reverse direction ->";
            this.buttonReverseDirection.UseVisualStyleBackColor = true;
            this.buttonReverseDirection.Click += new System.EventHandler(this.buttonReverseDirection_Click);
            // 
            // checkBoxRestrictToRelatedData
            // 
            this.checkBoxRestrictToRelatedData.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.checkBoxRestrictToRelatedData, 2);
            this.checkBoxRestrictToRelatedData.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxRestrictToRelatedData.Location = new System.Drawing.Point(37, 59);
            this.checkBoxRestrictToRelatedData.Name = "checkBoxRestrictToRelatedData";
            this.checkBoxRestrictToRelatedData.Size = new System.Drawing.Size(133, 21);
            this.checkBoxRestrictToRelatedData.TabIndex = 20;
            this.checkBoxRestrictToRelatedData.Text = "Restrict to related data";
            this.checkBoxRestrictToRelatedData.UseVisualStyleBackColor = true;
            this.checkBoxRestrictToRelatedData.Visible = false;
            // 
            // checkBoxRestrictToProject
            // 
            this.checkBoxRestrictToProject.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.checkBoxRestrictToProject, 2);
            this.checkBoxRestrictToProject.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxRestrictToProject.Location = new System.Drawing.Point(205, 59);
            this.checkBoxRestrictToProject.Name = "checkBoxRestrictToProject";
            this.checkBoxRestrictToProject.Size = new System.Drawing.Size(106, 21);
            this.checkBoxRestrictToProject.TabIndex = 21;
            this.checkBoxRestrictToProject.Text = "Restict to project";
            this.checkBoxRestrictToProject.UseVisualStyleBackColor = true;
            this.checkBoxRestrictToProject.Visible = false;
            // 
            // comboBoxProject
            // 
            this.comboBoxProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxProject.FormattingEnabled = true;
            this.comboBoxProject.Location = new System.Drawing.Point(317, 59);
            this.comboBoxProject.Name = "comboBoxProject";
            this.comboBoxProject.Size = new System.Drawing.Size(179, 21);
            this.comboBoxProject.TabIndex = 22;
            this.comboBoxProject.Visible = false;
            // 
            // buttonSelectAllTables
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonSelectAllTables, 2);
            this.buttonSelectAllTables.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonSelectAllTables.Location = new System.Drawing.Point(176, 453);
            this.buttonSelectAllTables.Name = "buttonSelectAllTables";
            this.buttonSelectAllTables.Size = new System.Drawing.Size(100, 24);
            this.buttonSelectAllTables.TabIndex = 23;
            this.buttonSelectAllTables.Text = "Select all tables";
            this.buttonSelectAllTables.UseVisualStyleBackColor = true;
            this.buttonSelectAllTables.Click += new System.EventHandler(this.buttonSelectAllTables_Click);
            // 
            // buttonSelectNoTable
            // 
            this.buttonSelectNoTable.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonSelectNoTable.Location = new System.Drawing.Point(317, 453);
            this.buttonSelectNoTable.Name = "buttonSelectNoTable";
            this.buttonSelectNoTable.Size = new System.Drawing.Size(100, 24);
            this.buttonSelectNoTable.TabIndex = 24;
            this.buttonSelectNoTable.Text = "Select no table";
            this.buttonSelectNoTable.UseVisualStyleBackColor = true;
            this.buttonSelectNoTable.Click += new System.EventHandler(this.buttonSelectNoTable_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonReset.Location = new System.Drawing.Point(563, 453);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 24);
            this.buttonReset.TabIndex = 25;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // openFileDialogCompactDB
            // 
            this.openFileDialogCompactDB.FileName = "openFileDialogCompactDB";
            this.openFileDialogCompactDB.Filter = "\"SDF|*.sdf\"";
            // 
            // FormSynchronisation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 480);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSynchronisation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Synchronisation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSynchronisation_FormClosing);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.ComboBox comboBoxSource;
        private System.Windows.Forms.Label labelDestination;
        private System.Windows.Forms.ComboBox comboBoxDestination;
        private System.Windows.Forms.ComboBox comboBoxTransfer;
        private System.Windows.Forms.Label labelTransferedData;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelSourceConnection;
        private System.Windows.Forms.Button buttonDestination;
        private System.Windows.Forms.Label labelDestinationConnection;
        private System.Windows.Forms.TextBox textBoxSourceConnection;
        private System.Windows.Forms.TextBox textBoxDestinationConnection;
        private System.Windows.Forms.Button buttonTransfer;
        private System.Windows.Forms.Button buttonCleanDestination;
        private System.Windows.Forms.Button buttonSource;
        private System.Windows.Forms.OpenFileDialog openFileDialogCompactDB;
        private System.Windows.Forms.ListBox listBoxSpecimen;
        private System.Windows.Forms.Label labelWhatIsToBeDone;
        private System.Windows.Forms.Panel panelTableList;
        private System.Windows.Forms.Button buttonReverseDirection;
        private System.Windows.Forms.CheckBox checkBoxRestrictToRelatedData;
        private System.Windows.Forms.CheckBox checkBoxRestrictToProject;
        private System.Windows.Forms.ComboBox comboBoxProject;
        private System.Windows.Forms.Button buttonSelectAllTables;
        private System.Windows.Forms.Button buttonSelectNoTable;
        private System.Windows.Forms.Button buttonReset;
    }
}