namespace DiversityCollection.CacheDatabase
{
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            tableLayoutPanelChunks = new System.Windows.Forms.TableLayoutPanel();
            labelPostgresChunkLimit = new System.Windows.Forms.Label();
            labelCacheChunkLimit = new System.Windows.Forms.Label();
            labelPostgresChunkSize = new System.Windows.Forms.Label();
            labelCacheChunkSize = new System.Windows.Forms.Label();
            textBoxCacheChunkLimit = new System.Windows.Forms.TextBox();
            textBoxPostgresChunkLimit = new System.Windows.Forms.TextBox();
            textBoxCacheChunkSize = new System.Windows.Forms.TextBox();
            textBoxPostgresChunkSize = new System.Windows.Forms.TextBox();
            pictureBoxCacheDB = new System.Windows.Forms.PictureBox();
            pictureBoxPostgres = new System.Windows.Forms.PictureBox();
            labelHeader = new System.Windows.Forms.Label();
            buttonDefaults = new System.Windows.Forms.Button();
            pictureBoxSourceDB = new System.Windows.Forms.PictureBox();
            pictureBoxSourceToCache = new System.Windows.Forms.PictureBox();
            pictureBoxSourceCache = new System.Windows.Forms.PictureBox();
            pictureBoxCacheToPostgres = new System.Windows.Forms.PictureBox();
            checkBoxUseChunksForPostgres = new System.Windows.Forms.CheckBox();
            labelTimeout = new System.Windows.Forms.Label();
            textBoxTimeout = new System.Windows.Forms.TextBox();
            pictureBoxTimeout = new System.Windows.Forms.PictureBox();
            textBoxTimeoutPostgres = new System.Windows.Forms.TextBox();
            textBoxPostgresBashFile = new System.Windows.Forms.TextBox();
            buttonPostgresBashFile = new System.Windows.Forms.Button();
            textBoxPostgresTransferDirectory = new System.Windows.Forms.TextBox();
            buttonPostgresTransferDirectory = new System.Windows.Forms.Button();
            checkBoxLogEvents = new System.Windows.Forms.CheckBox();
            pictureBoxLogEvents = new System.Windows.Forms.PictureBox();
            labelLogEvents = new System.Windows.Forms.Label();
            checkBoxStopOnError = new System.Windows.Forms.CheckBox();
            pictureBoxStopOnError = new System.Windows.Forms.PictureBox();
            labelPostgresBulkTransfer = new System.Windows.Forms.Label();
            toolTip = new System.Windows.Forms.ToolTip(components);
            buttonPostgresMountPoint = new System.Windows.Forms.Button();
            userControlDialogPanel1 = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            splitContainerMain = new System.Windows.Forms.SplitContainer();
            tableLayoutPanelSettings = new System.Windows.Forms.TableLayoutPanel();
            pictureBox2 = new System.Windows.Forms.PictureBox();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            textBoxPostgresMountPoint = new System.Windows.Forms.TextBox();
            helpProvider = new System.Windows.Forms.HelpProvider();
            tableLayoutPanelChunks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxCacheDB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPostgres).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSourceDB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSourceToCache).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSourceCache).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxCacheToPostgres).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxTimeout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogEvents).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxStopOnError).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            tableLayoutPanelSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanelChunks
            // 
            tableLayoutPanelChunks.ColumnCount = 6;
            tableLayoutPanelChunks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelChunks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            tableLayoutPanelChunks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            tableLayoutPanelChunks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelChunks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelChunks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelChunks.Controls.Add(labelPostgresChunkLimit, 3, 6);
            tableLayoutPanelChunks.Controls.Add(labelCacheChunkLimit, 3, 1);
            tableLayoutPanelChunks.Controls.Add(labelPostgresChunkSize, 3, 7);
            tableLayoutPanelChunks.Controls.Add(labelCacheChunkSize, 3, 2);
            tableLayoutPanelChunks.Controls.Add(textBoxCacheChunkLimit, 5, 1);
            tableLayoutPanelChunks.Controls.Add(textBoxPostgresChunkLimit, 5, 6);
            tableLayoutPanelChunks.Controls.Add(textBoxCacheChunkSize, 5, 2);
            tableLayoutPanelChunks.Controls.Add(textBoxPostgresChunkSize, 5, 7);
            tableLayoutPanelChunks.Controls.Add(pictureBoxCacheDB, 2, 1);
            tableLayoutPanelChunks.Controls.Add(pictureBoxPostgres, 2, 6);
            tableLayoutPanelChunks.Controls.Add(labelHeader, 0, 0);
            tableLayoutPanelChunks.Controls.Add(buttonDefaults, 2, 10);
            tableLayoutPanelChunks.Controls.Add(pictureBoxSourceDB, 0, 1);
            tableLayoutPanelChunks.Controls.Add(pictureBoxSourceToCache, 1, 1);
            tableLayoutPanelChunks.Controls.Add(pictureBoxSourceCache, 0, 6);
            tableLayoutPanelChunks.Controls.Add(pictureBoxCacheToPostgres, 1, 6);
            tableLayoutPanelChunks.Controls.Add(checkBoxUseChunksForPostgres, 0, 5);
            tableLayoutPanelChunks.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelChunks.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelChunks.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelChunks.Name = "tableLayoutPanelChunks";
            tableLayoutPanelChunks.RowCount = 11;
            tableLayoutPanelChunks.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelChunks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanelChunks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanelChunks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanelChunks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            tableLayoutPanelChunks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelChunks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanelChunks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanelChunks.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelChunks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            tableLayoutPanelChunks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            tableLayoutPanelChunks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelChunks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelChunks.Size = new System.Drawing.Size(96, 100);
            tableLayoutPanelChunks.TabIndex = 0;
            // 
            // labelPostgresChunkLimit
            // 
            labelPostgresChunkLimit.AutoSize = true;
            tableLayoutPanelChunks.SetColumnSpan(labelPostgresChunkLimit, 2);
            labelPostgresChunkLimit.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPostgresChunkLimit.Location = new System.Drawing.Point(68, 93);
            labelPostgresChunkLimit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelPostgresChunkLimit.Name = "labelPostgresChunkLimit";
            labelPostgresChunkLimit.Size = new System.Drawing.Size(169, 1);
            labelPostgresChunkLimit.TabIndex = 2;
            labelPostgresChunkLimit.Text = "Limit for the transfer in chunks";
            labelPostgresChunkLimit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCacheChunkLimit
            // 
            labelCacheChunkLimit.AutoSize = true;
            tableLayoutPanelChunks.SetColumnSpan(labelCacheChunkLimit, 2);
            labelCacheChunkLimit.Dock = System.Windows.Forms.DockStyle.Fill;
            labelCacheChunkLimit.Location = new System.Drawing.Point(68, 134);
            labelCacheChunkLimit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelCacheChunkLimit.Name = "labelCacheChunkLimit";
            labelCacheChunkLimit.Size = new System.Drawing.Size(169, 1);
            labelCacheChunkLimit.TabIndex = 6;
            labelCacheChunkLimit.Text = "Limit for the transfer in chunks";
            labelCacheChunkLimit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelPostgresChunkSize
            // 
            labelPostgresChunkSize.AutoSize = true;
            tableLayoutPanelChunks.SetColumnSpan(labelPostgresChunkSize, 2);
            labelPostgresChunkSize.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPostgresChunkSize.Location = new System.Drawing.Point(68, 67);
            labelPostgresChunkSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelPostgresChunkSize.Name = "labelPostgresChunkSize";
            labelPostgresChunkSize.Size = new System.Drawing.Size(169, 1);
            labelPostgresChunkSize.TabIndex = 4;
            labelPostgresChunkSize.Text = "Maximal size of the chunks";
            labelPostgresChunkSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCacheChunkSize
            // 
            labelCacheChunkSize.AutoSize = true;
            tableLayoutPanelChunks.SetColumnSpan(labelCacheChunkSize, 2);
            labelCacheChunkSize.Dock = System.Windows.Forms.DockStyle.Fill;
            labelCacheChunkSize.Location = new System.Drawing.Point(68, 108);
            labelCacheChunkSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelCacheChunkSize.Name = "labelCacheChunkSize";
            labelCacheChunkSize.Size = new System.Drawing.Size(169, 1);
            labelCacheChunkSize.TabIndex = 8;
            labelCacheChunkSize.Text = "Maximal size of the chunks";
            labelCacheChunkSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCacheChunkLimit
            // 
            textBoxCacheChunkLimit.BackColor = System.Drawing.Color.Thistle;
            textBoxCacheChunkLimit.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxCacheChunkLimit.Location = new System.Drawing.Point(245, 137);
            textBoxCacheChunkLimit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxCacheChunkLimit.Name = "textBoxCacheChunkLimit";
            textBoxCacheChunkLimit.Size = new System.Drawing.Size(1, 23);
            textBoxCacheChunkLimit.TabIndex = 7;
            textBoxCacheChunkLimit.TextChanged += textBoxCacheChunkLimit_TextChanged;
            // 
            // textBoxPostgresChunkLimit
            // 
            textBoxPostgresChunkLimit.BackColor = System.Drawing.Color.LightSteelBlue;
            textBoxPostgresChunkLimit.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxPostgresChunkLimit.Location = new System.Drawing.Point(245, 96);
            textBoxPostgresChunkLimit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxPostgresChunkLimit.Name = "textBoxPostgresChunkLimit";
            textBoxPostgresChunkLimit.Size = new System.Drawing.Size(1, 23);
            textBoxPostgresChunkLimit.TabIndex = 3;
            textBoxPostgresChunkLimit.TextChanged += textBoxPostgresChunkLimit_TextChanged;
            // 
            // textBoxCacheChunkSize
            // 
            textBoxCacheChunkSize.BackColor = System.Drawing.Color.Thistle;
            textBoxCacheChunkSize.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxCacheChunkSize.Location = new System.Drawing.Point(245, 111);
            textBoxCacheChunkSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxCacheChunkSize.Name = "textBoxCacheChunkSize";
            textBoxCacheChunkSize.Size = new System.Drawing.Size(1, 23);
            textBoxCacheChunkSize.TabIndex = 9;
            textBoxCacheChunkSize.TextChanged += textBoxCacheChunkSize_TextChanged;
            // 
            // textBoxPostgresChunkSize
            // 
            textBoxPostgresChunkSize.BackColor = System.Drawing.Color.LightSteelBlue;
            textBoxPostgresChunkSize.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxPostgresChunkSize.Location = new System.Drawing.Point(245, 70);
            textBoxPostgresChunkSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxPostgresChunkSize.Name = "textBoxPostgresChunkSize";
            textBoxPostgresChunkSize.Size = new System.Drawing.Size(1, 23);
            textBoxPostgresChunkSize.TabIndex = 5;
            textBoxPostgresChunkSize.TextChanged += textBoxPostgresChunkSize_TextChanged;
            // 
            // pictureBoxCacheDB
            // 
            pictureBoxCacheDB.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxCacheDB.Image = Resource.CacheDB;
            pictureBoxCacheDB.Location = new System.Drawing.Point(41, 137);
            pictureBoxCacheDB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBoxCacheDB.Name = "pictureBoxCacheDB";
            pictureBoxCacheDB.Size = new System.Drawing.Size(19, 1);
            pictureBoxCacheDB.TabIndex = 11;
            pictureBoxCacheDB.TabStop = false;
            // 
            // pictureBoxPostgres
            // 
            pictureBoxPostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxPostgres.Image = Resource.Postgres;
            pictureBoxPostgres.Location = new System.Drawing.Point(41, 96);
            pictureBoxPostgres.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBoxPostgres.Name = "pictureBoxPostgres";
            pictureBoxPostgres.Size = new System.Drawing.Size(19, 1);
            pictureBoxPostgres.TabIndex = 12;
            pictureBoxPostgres.TabStop = false;
            // 
            // labelHeader
            // 
            labelHeader.AutoSize = true;
            tableLayoutPanelChunks.SetColumnSpan(labelHeader, 6);
            labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHeader.Location = new System.Drawing.Point(7, 7);
            labelHeader.Margin = new System.Windows.Forms.Padding(7);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new System.Drawing.Size(82, 120);
            labelHeader.TabIndex = 16;
            labelHeader.Text = "Edit the settings for the transfer into the cache database and into the Postgres database";
            labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonDefaults
            // 
            tableLayoutPanelChunks.SetColumnSpan(buttonDefaults, 4);
            buttonDefaults.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonDefaults.Location = new System.Drawing.Point(41, 74);
            buttonDefaults.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonDefaults.Name = "buttonDefaults";
            buttonDefaults.Size = new System.Drawing.Size(51, 27);
            buttonDefaults.TabIndex = 17;
            buttonDefaults.Text = "Set default values";
            buttonDefaults.UseVisualStyleBackColor = true;
            buttonDefaults.Click += buttonDefaults_Click;
            // 
            // pictureBoxSourceDB
            // 
            pictureBoxSourceDB.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxSourceDB.Image = Resource.Database;
            pictureBoxSourceDB.Location = new System.Drawing.Point(4, 137);
            pictureBoxSourceDB.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            pictureBoxSourceDB.Name = "pictureBoxSourceDB";
            pictureBoxSourceDB.Size = new System.Drawing.Size(19, 1);
            pictureBoxSourceDB.TabIndex = 18;
            pictureBoxSourceDB.TabStop = false;
            // 
            // pictureBoxSourceToCache
            // 
            pictureBoxSourceToCache.Image = Resource.ArrowNext1;
            pictureBoxSourceToCache.Location = new System.Drawing.Point(23, 137);
            pictureBoxSourceToCache.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            pictureBoxSourceToCache.Name = "pictureBoxSourceToCache";
            pictureBoxSourceToCache.Size = new System.Drawing.Size(14, 1);
            pictureBoxSourceToCache.TabIndex = 19;
            pictureBoxSourceToCache.TabStop = false;
            // 
            // pictureBoxSourceCache
            // 
            pictureBoxSourceCache.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxSourceCache.Image = Resource.CacheDB;
            pictureBoxSourceCache.Location = new System.Drawing.Point(4, 96);
            pictureBoxSourceCache.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            pictureBoxSourceCache.Name = "pictureBoxSourceCache";
            pictureBoxSourceCache.Size = new System.Drawing.Size(19, 1);
            pictureBoxSourceCache.TabIndex = 20;
            pictureBoxSourceCache.TabStop = false;
            // 
            // pictureBoxCacheToPostgres
            // 
            pictureBoxCacheToPostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxCacheToPostgres.Image = Resource.ArrowNext1;
            pictureBoxCacheToPostgres.Location = new System.Drawing.Point(23, 96);
            pictureBoxCacheToPostgres.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            pictureBoxCacheToPostgres.Name = "pictureBoxCacheToPostgres";
            pictureBoxCacheToPostgres.Size = new System.Drawing.Size(14, 1);
            pictureBoxCacheToPostgres.TabIndex = 21;
            pictureBoxCacheToPostgres.TabStop = false;
            // 
            // checkBoxUseChunksForPostgres
            // 
            checkBoxUseChunksForPostgres.AutoSize = true;
            tableLayoutPanelChunks.SetColumnSpan(checkBoxUseChunksForPostgres, 6);
            checkBoxUseChunksForPostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxUseChunksForPostgres.Location = new System.Drawing.Point(4, 70);
            checkBoxUseChunksForPostgres.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            checkBoxUseChunksForPostgres.Name = "checkBoxUseChunksForPostgres";
            checkBoxUseChunksForPostgres.Size = new System.Drawing.Size(88, 23);
            checkBoxUseChunksForPostgres.TabIndex = 25;
            checkBoxUseChunksForPostgres.Text = "Use chunks for transfer to postgres";
            checkBoxUseChunksForPostgres.UseVisualStyleBackColor = true;
            checkBoxUseChunksForPostgres.Click += checkBoxUseChunksForPostgres_Click;
            // 
            // labelTimeout
            // 
            labelTimeout.AutoSize = true;
            tableLayoutPanelSettings.SetColumnSpan(labelTimeout, 5);
            labelTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            labelTimeout.Location = new System.Drawing.Point(32, 0);
            labelTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelTimeout.Name = "labelTimeout";
            labelTimeout.Size = new System.Drawing.Size(538, 30);
            labelTimeout.TabIndex = 0;
            labelTimeout.Text = "Timeout in minutes (0 = infinite)";
            labelTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxTimeout
            // 
            textBoxTimeout.BackColor = System.Drawing.Color.Thistle;
            tableLayoutPanelSettings.SetColumnSpan(textBoxTimeout, 3);
            textBoxTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxTimeout.Location = new System.Drawing.Point(32, 33);
            textBoxTimeout.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxTimeout.Name = "textBoxTimeout";
            textBoxTimeout.Size = new System.Drawing.Size(263, 23);
            textBoxTimeout.TabIndex = 1;
            textBoxTimeout.TextChanged += textBoxTimeout_TextChanged;
            // 
            // pictureBoxTimeout
            // 
            pictureBoxTimeout.Dock = System.Windows.Forms.DockStyle.Bottom;
            pictureBoxTimeout.Image = Resource.Time;
            pictureBoxTimeout.Location = new System.Drawing.Point(4, 9);
            pictureBoxTimeout.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBoxTimeout.Name = "pictureBoxTimeout";
            pictureBoxTimeout.Size = new System.Drawing.Size(20, 18);
            pictureBoxTimeout.TabIndex = 10;
            pictureBoxTimeout.TabStop = false;
            // 
            // textBoxTimeoutPostgres
            // 
            textBoxTimeoutPostgres.BackColor = System.Drawing.Color.LightSteelBlue;
            textBoxTimeoutPostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxTimeoutPostgres.Location = new System.Drawing.Point(327, 33);
            textBoxTimeoutPostgres.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxTimeoutPostgres.Name = "textBoxTimeoutPostgres";
            textBoxTimeoutPostgres.Size = new System.Drawing.Size(243, 23);
            textBoxTimeoutPostgres.TabIndex = 15;
            textBoxTimeoutPostgres.TextChanged += textBoxTimeoutPostgres_TextChanged;
            // 
            // textBoxPostgresBashFile
            // 
            textBoxPostgresBashFile.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxPostgresBashFile.Enabled = false;
            textBoxPostgresBashFile.Location = new System.Drawing.Point(323, 184);
            textBoxPostgresBashFile.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            textBoxPostgresBashFile.Name = "textBoxPostgresBashFile";
            textBoxPostgresBashFile.ReadOnly = true;
            textBoxPostgresBashFile.Size = new System.Drawing.Size(247, 23);
            textBoxPostgresBashFile.TabIndex = 32;
            // 
            // buttonPostgresBashFile
            // 
            buttonPostgresBashFile.Enabled = false;
            buttonPostgresBashFile.FlatAppearance.BorderSize = 0;
            buttonPostgresBashFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonPostgresBashFile.Image = Resource.TransferToFile;
            buttonPostgresBashFile.Location = new System.Drawing.Point(299, 184);
            buttonPostgresBashFile.Margin = new System.Windows.Forms.Padding(0);
            buttonPostgresBashFile.Name = "buttonPostgresBashFile";
            buttonPostgresBashFile.Size = new System.Drawing.Size(23, 24);
            buttonPostgresBashFile.TabIndex = 31;
            toolTip.SetToolTip(buttonPostgresBashFile, "Set the bash file for conversion of the exported files");
            buttonPostgresBashFile.UseVisualStyleBackColor = true;
            buttonPostgresBashFile.Click += buttonPostgresBashFile_Click;
            // 
            // textBoxPostgresTransferDirectory
            // 
            textBoxPostgresTransferDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxPostgresTransferDirectory.Enabled = false;
            textBoxPostgresTransferDirectory.Location = new System.Drawing.Point(32, 184);
            textBoxPostgresTransferDirectory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            textBoxPostgresTransferDirectory.Name = "textBoxPostgresTransferDirectory";
            textBoxPostgresTransferDirectory.ReadOnly = true;
            textBoxPostgresTransferDirectory.Size = new System.Drawing.Size(141, 23);
            textBoxPostgresTransferDirectory.TabIndex = 30;
            // 
            // buttonPostgresTransferDirectory
            // 
            buttonPostgresTransferDirectory.Enabled = false;
            buttonPostgresTransferDirectory.FlatAppearance.BorderSize = 0;
            buttonPostgresTransferDirectory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonPostgresTransferDirectory.Image = Resource.Open;
            buttonPostgresTransferDirectory.Location = new System.Drawing.Point(0, 184);
            buttonPostgresTransferDirectory.Margin = new System.Windows.Forms.Padding(0);
            buttonPostgresTransferDirectory.Name = "buttonPostgresTransferDirectory";
            buttonPostgresTransferDirectory.Size = new System.Drawing.Size(23, 24);
            buttonPostgresTransferDirectory.TabIndex = 29;
            toolTip.SetToolTip(buttonPostgresTransferDirectory, "Set the transfer directory on the Postgres server");
            buttonPostgresTransferDirectory.UseVisualStyleBackColor = true;
            buttonPostgresTransferDirectory.Click += buttonPostgresTransferDirectory_Click;
            // 
            // checkBoxLogEvents
            // 
            checkBoxLogEvents.AutoSize = true;
            tableLayoutPanelSettings.SetColumnSpan(checkBoxLogEvents, 5);
            checkBoxLogEvents.Location = new System.Drawing.Point(32, 103);
            checkBoxLogEvents.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            checkBoxLogEvents.Name = "checkBoxLogEvents";
            checkBoxLogEvents.Size = new System.Drawing.Size(359, 19);
            checkBoxLogEvents.TabIndex = 22;
            checkBoxLogEvents.Text = "Log the events of the transfer in the error log and create reports";
            checkBoxLogEvents.UseVisualStyleBackColor = true;
            checkBoxLogEvents.Click += checkBoxLogEvents_Click;
            // 
            // pictureBoxLogEvents
            // 
            pictureBoxLogEvents.ErrorImage = Resource.ListNot;
            pictureBoxLogEvents.Image = Resource.ListNot;
            pictureBoxLogEvents.Location = new System.Drawing.Point(4, 103);
            pictureBoxLogEvents.Margin = new System.Windows.Forms.Padding(4, 3, 0, 0);
            pictureBoxLogEvents.Name = "pictureBoxLogEvents";
            pictureBoxLogEvents.Size = new System.Drawing.Size(20, 23);
            pictureBoxLogEvents.TabIndex = 23;
            pictureBoxLogEvents.TabStop = false;
            // 
            // labelLogEvents
            // 
            labelLogEvents.AutoSize = true;
            tableLayoutPanelSettings.SetColumnSpan(labelLogEvents, 6);
            labelLogEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            labelLogEvents.Location = new System.Drawing.Point(4, 60);
            labelLogEvents.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelLogEvents.Name = "labelLogEvents";
            labelLogEvents.Size = new System.Drawing.Size(566, 40);
            labelLogEvents.TabIndex = 24;
            labelLogEvents.Text = "Settings, numbers and errors are documented within the database. In addition you can log every step in the error log and create report files";
            labelLogEvents.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // checkBoxStopOnError
            // 
            checkBoxStopOnError.AutoSize = true;
            tableLayoutPanelSettings.SetColumnSpan(checkBoxStopOnError, 5);
            checkBoxStopOnError.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            checkBoxStopOnError.Location = new System.Drawing.Point(32, 129);
            checkBoxStopOnError.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxStopOnError.Name = "checkBoxStopOnError";
            checkBoxStopOnError.Size = new System.Drawing.Size(259, 19);
            checkBoxStopOnError.TabIndex = 26;
            checkBoxStopOnError.Text = "Stop execution of transfer in case of an error";
            checkBoxStopOnError.UseVisualStyleBackColor = true;
            checkBoxStopOnError.Click += checkBoxStopOnError_Click;
            // 
            // pictureBoxStopOnError
            // 
            pictureBoxStopOnError.Image = Resource.ArrowNextNext;
            pictureBoxStopOnError.InitialImage = Resource.ArrowNextNext;
            pictureBoxStopOnError.Location = new System.Drawing.Point(4, 129);
            pictureBoxStopOnError.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            pictureBoxStopOnError.Name = "pictureBoxStopOnError";
            pictureBoxStopOnError.Size = new System.Drawing.Size(20, 22);
            pictureBoxStopOnError.TabIndex = 27;
            pictureBoxStopOnError.TabStop = false;
            // 
            // labelPostgresBulkTransfer
            // 
            labelPostgresBulkTransfer.AutoSize = true;
            tableLayoutPanelSettings.SetColumnSpan(labelPostgresBulkTransfer, 6);
            labelPostgresBulkTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPostgresBulkTransfer.Location = new System.Drawing.Point(4, 154);
            labelPostgresBulkTransfer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelPostgresBulkTransfer.Name = "labelPostgresBulkTransfer";
            labelPostgresBulkTransfer.Size = new System.Drawing.Size(566, 30);
            labelPostgresBulkTransfer.TabIndex = 28;
            labelPostgresBulkTransfer.Text = "Settings for Postgres transfer via bcp (Bulk transfer)";
            labelPostgresBulkTransfer.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // buttonPostgresMountPoint
            // 
            buttonPostgresMountPoint.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonPostgresMountPoint.FlatAppearance.BorderSize = 0;
            buttonPostgresMountPoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonPostgresMountPoint.Image = Resource.MountPoint;
            buttonPostgresMountPoint.Location = new System.Drawing.Point(177, 184);
            buttonPostgresMountPoint.Margin = new System.Windows.Forms.Padding(0);
            buttonPostgresMountPoint.Name = "buttonPostgresMountPoint";
            buttonPostgresMountPoint.Size = new System.Drawing.Size(23, 28);
            buttonPostgresMountPoint.TabIndex = 35;
            toolTip.SetToolTip(buttonPostgresMountPoint, "Mount point name of the transfer folder");
            buttonPostgresMountPoint.UseVisualStyleBackColor = true;
            buttonPostgresMountPoint.Click += buttonPostgresMountPoint_Click;
            // 
            // userControlDialogPanel1
            // 
            userControlDialogPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            userControlDialogPanel1.Location = new System.Drawing.Point(0, 212);
            userControlDialogPanel1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlDialogPanel1.Name = "userControlDialogPanel1";
            userControlDialogPanel1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            userControlDialogPanel1.Size = new System.Drawing.Size(574, 31);
            userControlDialogPanel1.TabIndex = 1;
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
            splitContainerMain.Panel1.Controls.Add(tableLayoutPanelSettings);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(tableLayoutPanelChunks);
            splitContainerMain.Panel2Collapsed = true;
            splitContainerMain.Size = new System.Drawing.Size(574, 212);
            splitContainerMain.SplitterDistance = 322;
            splitContainerMain.SplitterWidth = 5;
            splitContainerMain.TabIndex = 2;
            // 
            // tableLayoutPanelSettings
            // 
            tableLayoutPanelSettings.ColumnCount = 6;
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelSettings.Controls.Add(buttonPostgresMountPoint, 2, 6);
            tableLayoutPanelSettings.Controls.Add(labelTimeout, 1, 0);
            tableLayoutPanelSettings.Controls.Add(textBoxPostgresBashFile, 5, 6);
            tableLayoutPanelSettings.Controls.Add(buttonPostgresTransferDirectory, 0, 6);
            tableLayoutPanelSettings.Controls.Add(buttonPostgresBashFile, 4, 6);
            tableLayoutPanelSettings.Controls.Add(textBoxPostgresTransferDirectory, 1, 6);
            tableLayoutPanelSettings.Controls.Add(labelPostgresBulkTransfer, 0, 5);
            tableLayoutPanelSettings.Controls.Add(pictureBoxStopOnError, 0, 4);
            tableLayoutPanelSettings.Controls.Add(pictureBoxLogEvents, 0, 3);
            tableLayoutPanelSettings.Controls.Add(textBoxTimeoutPostgres, 5, 1);
            tableLayoutPanelSettings.Controls.Add(labelLogEvents, 0, 2);
            tableLayoutPanelSettings.Controls.Add(checkBoxLogEvents, 1, 3);
            tableLayoutPanelSettings.Controls.Add(checkBoxStopOnError, 1, 4);
            tableLayoutPanelSettings.Controls.Add(pictureBoxTimeout, 0, 0);
            tableLayoutPanelSettings.Controls.Add(pictureBox2, 4, 1);
            tableLayoutPanelSettings.Controls.Add(pictureBox1, 0, 1);
            tableLayoutPanelSettings.Controls.Add(textBoxTimeout, 1, 1);
            tableLayoutPanelSettings.Controls.Add(textBoxPostgresMountPoint, 3, 6);
            tableLayoutPanelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(tableLayoutPanelSettings, "cachedatabase_transfersettings_dc");
            tableLayoutPanelSettings.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelSettings.Name = "tableLayoutPanelSettings";
            tableLayoutPanelSettings.RowCount = 7;
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            helpProvider.SetShowHelp(tableLayoutPanelSettings, true);
            tableLayoutPanelSettings.Size = new System.Drawing.Size(574, 212);
            tableLayoutPanelSettings.TabIndex = 0;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Resource.CacheDB;
            pictureBox2.Location = new System.Drawing.Point(303, 33);
            pictureBox2.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new System.Drawing.Size(20, 24);
            pictureBox2.TabIndex = 34;
            pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Resource.Database;
            pictureBox1.Location = new System.Drawing.Point(4, 33);
            pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(20, 24);
            pictureBox1.TabIndex = 33;
            pictureBox1.TabStop = false;
            // 
            // textBoxPostgresMountPoint
            // 
            textBoxPostgresMountPoint.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxPostgresMountPoint.Location = new System.Drawing.Point(200, 184);
            textBoxPostgresMountPoint.Margin = new System.Windows.Forms.Padding(0);
            textBoxPostgresMountPoint.Name = "textBoxPostgresMountPoint";
            textBoxPostgresMountPoint.ReadOnly = true;
            textBoxPostgresMountPoint.Size = new System.Drawing.Size(99, 23);
            textBoxPostgresMountPoint.TabIndex = 36;
            // 
            // FormSettings
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(574, 243);
            Controls.Add(splitContainerMain);
            Controls.Add(userControlDialogPanel1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            helpProvider.SetHelpKeyword(this, "cachedatabase_transfersettings_dc");
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormSettings";
            helpProvider.SetShowHelp(this, true);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Settings";
            FormClosing += FormSettings_FormClosing;
            KeyDown += Form_KeyDown;
            tableLayoutPanelChunks.ResumeLayout(false);
            tableLayoutPanelChunks.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxCacheDB).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPostgres).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSourceDB).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSourceToCache).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSourceCache).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxCacheToPostgres).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxTimeout).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogEvents).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxStopOnError).EndInit();
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            tableLayoutPanelSettings.ResumeLayout(false);
            tableLayoutPanelSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelChunks;
        private System.Windows.Forms.Label labelTimeout;
        private System.Windows.Forms.TextBox textBoxTimeout;
        private System.Windows.Forms.PictureBox pictureBoxTimeout;
        private System.Windows.Forms.Label labelPostgresChunkLimit;
        private System.Windows.Forms.Label labelCacheChunkLimit;
        private System.Windows.Forms.Label labelPostgresChunkSize;
        private System.Windows.Forms.Label labelCacheChunkSize;
        private System.Windows.Forms.TextBox textBoxCacheChunkLimit;
        private System.Windows.Forms.TextBox textBoxPostgresChunkLimit;
        private System.Windows.Forms.TextBox textBoxCacheChunkSize;
        private System.Windows.Forms.TextBox textBoxPostgresChunkSize;
        private System.Windows.Forms.PictureBox pictureBoxCacheDB;
        private System.Windows.Forms.PictureBox pictureBoxPostgres;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel1;
        private System.Windows.Forms.TextBox textBoxTimeoutPostgres;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Button buttonDefaults;
        private System.Windows.Forms.PictureBox pictureBoxSourceDB;
        private System.Windows.Forms.PictureBox pictureBoxSourceToCache;
        private System.Windows.Forms.PictureBox pictureBoxSourceCache;
        private System.Windows.Forms.PictureBox pictureBoxCacheToPostgres;
        private System.Windows.Forms.CheckBox checkBoxLogEvents;
        private System.Windows.Forms.PictureBox pictureBoxLogEvents;
        private System.Windows.Forms.Label labelLogEvents;
        private System.Windows.Forms.CheckBox checkBoxUseChunksForPostgres;
        private System.Windows.Forms.CheckBox checkBoxStopOnError;
        private System.Windows.Forms.PictureBox pictureBoxStopOnError;
        private System.Windows.Forms.Label labelPostgresBulkTransfer;
        private System.Windows.Forms.Button buttonPostgresTransferDirectory;
        private System.Windows.Forms.TextBox textBoxPostgresTransferDirectory;
        private System.Windows.Forms.Button buttonPostgresBashFile;
        private System.Windows.Forms.TextBox textBoxPostgresBashFile;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSettings;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonPostgresMountPoint;
        private System.Windows.Forms.TextBox textBoxPostgresMountPoint;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}