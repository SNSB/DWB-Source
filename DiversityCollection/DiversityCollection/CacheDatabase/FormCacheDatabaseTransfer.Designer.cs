namespace DiversityCollection.CacheDatabase
{
    partial class FormCacheDatabaseTransfer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCacheDatabaseTransfer));
            panelTransferSteps = new System.Windows.Forms.Panel();
            tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            buttonStartTransfer = new System.Windows.Forms.Button();
            buttonFeedback = new System.Windows.Forms.Button();
            labelHaeder = new System.Windows.Forms.Label();
            buttonSelectAll = new System.Windows.Forms.Button();
            buttonSelectNone = new System.Windows.Forms.Button();
            pictureBoxTarget = new System.Windows.Forms.PictureBox();
            pictureBoxSource = new System.Windows.Forms.PictureBox();
            labelHeaderSource = new System.Windows.Forms.Label();
            labelCount = new System.Windows.Forms.Label();
            labelCountTarget = new System.Windows.Forms.Label();
            labelCountSource = new System.Windows.Forms.Label();
            labelTarget = new System.Windows.Forms.Label();
            buttonTimeout = new System.Windows.Forms.Button();
            buttonLogging = new System.Windows.Forms.Button();
            buttonSettings = new System.Windows.Forms.Button();
            buttonStopOnError = new System.Windows.Forms.Button();
            pictureBoxBulkTransfer = new System.Windows.Forms.PictureBox();
            helpProvider = new System.Windows.Forms.HelpProvider();
            toolTip = new System.Windows.Forms.ToolTip(components);
            imageList = new System.Windows.Forms.ImageList(components);
            tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxTarget).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBulkTransfer).BeginInit();
            SuspendLayout();
            // 
            // panelTransferSteps
            // 
            panelTransferSteps.AutoScroll = true;
            panelTransferSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            panelTransferSteps.Location = new System.Drawing.Point(0, 57);
            panelTransferSteps.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panelTransferSteps.Name = "panelTransferSteps";
            panelTransferSteps.Size = new System.Drawing.Size(903, 676);
            panelTransferSteps.TabIndex = 0;
            toolTip.SetToolTip(panelTransferSteps, "Transfer via bcp (Bulk transfer)");
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 10;
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableLayoutPanelMain.Controls.Add(buttonStartTransfer, 1, 0);
            tableLayoutPanelMain.Controls.Add(buttonFeedback, 9, 0);
            tableLayoutPanelMain.Controls.Add(labelHaeder, 3, 0);
            tableLayoutPanelMain.Controls.Add(buttonSelectAll, 0, 1);
            tableLayoutPanelMain.Controls.Add(buttonSelectNone, 0, 0);
            tableLayoutPanelMain.Controls.Add(pictureBoxTarget, 7, 0);
            tableLayoutPanelMain.Controls.Add(pictureBoxSource, 5, 0);
            tableLayoutPanelMain.Controls.Add(labelHeaderSource, 2, 1);
            tableLayoutPanelMain.Controls.Add(labelCount, 4, 1);
            tableLayoutPanelMain.Controls.Add(labelCountTarget, 6, 1);
            tableLayoutPanelMain.Controls.Add(labelCountSource, 5, 1);
            tableLayoutPanelMain.Controls.Add(labelTarget, 3, 1);
            tableLayoutPanelMain.Controls.Add(buttonTimeout, 4, 0);
            tableLayoutPanelMain.Controls.Add(buttonLogging, 9, 1);
            tableLayoutPanelMain.Controls.Add(buttonSettings, 8, 0);
            tableLayoutPanelMain.Controls.Add(buttonStopOnError, 8, 1);
            tableLayoutPanelMain.Controls.Add(pictureBoxBulkTransfer, 6, 0);
            tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Top;
            helpProvider.SetHelpKeyword(tableLayoutPanelMain, "cachedatabase_transfer_dc");
            tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 2;
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            helpProvider.SetShowHelp(tableLayoutPanelMain, true);
            tableLayoutPanelMain.Size = new System.Drawing.Size(903, 57);
            tableLayoutPanelMain.TabIndex = 1;
            // 
            // buttonStartTransfer
            // 
            tableLayoutPanelMain.SetColumnSpan(buttonStartTransfer, 2);
            buttonStartTransfer.Image = Resource.ArrowNext1;
            buttonStartTransfer.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            buttonStartTransfer.Location = new System.Drawing.Point(28, 3);
            buttonStartTransfer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            buttonStartTransfer.Name = "buttonStartTransfer";
            buttonStartTransfer.Size = new System.Drawing.Size(100, 24);
            buttonStartTransfer.TabIndex = 0;
            buttonStartTransfer.Text = "Start transfer";
            buttonStartTransfer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            toolTip.SetToolTip(buttonStartTransfer, "Start the transfer of the data");
            buttonStartTransfer.UseVisualStyleBackColor = true;
            buttonStartTransfer.Click += buttonStartTransfer_Click;
            // 
            // buttonFeedback
            // 
            buttonFeedback.Dock = System.Windows.Forms.DockStyle.Right;
            buttonFeedback.Image = Resource.Feedback;
            buttonFeedback.Location = new System.Drawing.Point(875, 1);
            buttonFeedback.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            buttonFeedback.Name = "buttonFeedback";
            buttonFeedback.Size = new System.Drawing.Size(28, 27);
            buttonFeedback.TabIndex = 1;
            toolTip.SetToolTip(buttonFeedback, "Send a feedback to the administrator");
            buttonFeedback.UseVisualStyleBackColor = true;
            buttonFeedback.Click += buttonFeedback_Click;
            // 
            // labelHaeder
            // 
            labelHaeder.AutoSize = true;
            labelHaeder.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHaeder.Location = new System.Drawing.Point(136, 0);
            labelHaeder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelHaeder.Name = "labelHaeder";
            labelHaeder.Size = new System.Drawing.Size(441, 28);
            labelHaeder.TabIndex = 2;
            labelHaeder.Text = "Transfer";
            labelHaeder.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // buttonSelectAll
            // 
            buttonSelectAll.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonSelectAll.Image = Resource.CheckYes;
            buttonSelectAll.Location = new System.Drawing.Point(0, 28);
            buttonSelectAll.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            buttonSelectAll.Name = "buttonSelectAll";
            buttonSelectAll.Size = new System.Drawing.Size(24, 26);
            buttonSelectAll.TabIndex = 3;
            toolTip.SetToolTip(buttonSelectAll, "Select all transfer steps");
            buttonSelectAll.UseVisualStyleBackColor = true;
            buttonSelectAll.Click += buttonSelectAll_Click;
            // 
            // buttonSelectNone
            // 
            buttonSelectNone.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonSelectNone.Image = Resource.CheckNo;
            buttonSelectNone.Location = new System.Drawing.Point(0, 3);
            buttonSelectNone.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            buttonSelectNone.Name = "buttonSelectNone";
            buttonSelectNone.Size = new System.Drawing.Size(24, 25);
            buttonSelectNone.TabIndex = 4;
            toolTip.SetToolTip(buttonSelectNone, "Select none of the transfer steps");
            buttonSelectNone.UseVisualStyleBackColor = true;
            buttonSelectNone.Click += buttonSelectNone_Click;
            // 
            // pictureBoxTarget
            // 
            pictureBoxTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxTarget.Image = Resource.Postgres;
            pictureBoxTarget.Location = new System.Drawing.Point(801, 9);
            pictureBoxTarget.Margin = new System.Windows.Forms.Padding(2, 9, 22, 0);
            pictureBoxTarget.Name = "pictureBoxTarget";
            pictureBoxTarget.Size = new System.Drawing.Size(22, 19);
            pictureBoxTarget.TabIndex = 5;
            pictureBoxTarget.TabStop = false;
            // 
            // pictureBoxSource
            // 
            pictureBoxSource.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxSource.Image = Resource.CacheDB;
            pictureBoxSource.Location = new System.Drawing.Point(729, 9);
            pictureBoxSource.Margin = new System.Windows.Forms.Padding(31, 9, 31, 0);
            pictureBoxSource.Name = "pictureBoxSource";
            pictureBoxSource.Size = new System.Drawing.Size(20, 19);
            pictureBoxSource.TabIndex = 6;
            pictureBoxSource.TabStop = false;
            // 
            // labelHeaderSource
            // 
            labelHeaderSource.AutoSize = true;
            labelHeaderSource.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHeaderSource.Location = new System.Drawing.Point(46, 28);
            labelHeaderSource.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            labelHeaderSource.Name = "labelHeaderSource";
            labelHeaderSource.Size = new System.Drawing.Size(82, 29);
            labelHeaderSource.TabIndex = 7;
            labelHeaderSource.Text = "Source tables";
            labelHeaderSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelCount
            // 
            labelCount.AutoSize = true;
            labelCount.Dock = System.Windows.Forms.DockStyle.Fill;
            labelCount.Location = new System.Drawing.Point(585, 28);
            labelCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelCount.Name = "labelCount";
            labelCount.Size = new System.Drawing.Size(109, 29);
            labelCount.TabIndex = 8;
            labelCount.Text = "Current count in";
            labelCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCountTarget
            // 
            labelCountTarget.AutoSize = true;
            tableLayoutPanelMain.SetColumnSpan(labelCountTarget, 2);
            labelCountTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            labelCountTarget.Location = new System.Drawing.Point(780, 28);
            labelCountTarget.Margin = new System.Windows.Forms.Padding(0);
            labelCountTarget.Name = "labelCountTarget";
            labelCountTarget.Size = new System.Drawing.Size(65, 29);
            labelCountTarget.TabIndex = 9;
            labelCountTarget.Text = "CacheDB";
            labelCountTarget.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelCountSource
            // 
            labelCountSource.AutoSize = true;
            labelCountSource.Dock = System.Windows.Forms.DockStyle.Fill;
            labelCountSource.Location = new System.Drawing.Point(698, 28);
            labelCountSource.Margin = new System.Windows.Forms.Padding(0);
            labelCountSource.Name = "labelCountSource";
            labelCountSource.Size = new System.Drawing.Size(82, 29);
            labelCountSource.TabIndex = 10;
            labelCountSource.Text = "CacheDB";
            labelCountSource.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTarget
            // 
            labelTarget.AutoSize = true;
            labelTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            labelTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            labelTarget.Location = new System.Drawing.Point(136, 28);
            labelTarget.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelTarget.Name = "labelTarget";
            labelTarget.Size = new System.Drawing.Size(441, 29);
            labelTarget.TabIndex = 11;
            labelTarget.Text = "Target";
            labelTarget.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // buttonTimeout
            // 
            buttonTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonTimeout.Image = Resource.Time;
            buttonTimeout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonTimeout.Location = new System.Drawing.Point(581, 0);
            buttonTimeout.Margin = new System.Windows.Forms.Padding(0);
            buttonTimeout.Name = "buttonTimeout";
            buttonTimeout.Size = new System.Drawing.Size(117, 28);
            buttonTimeout.TabIndex = 12;
            buttonTimeout.Text = "      Infinite";
            buttonTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            toolTip.SetToolTip(buttonTimeout, "Set timeout for database interactions");
            buttonTimeout.UseVisualStyleBackColor = true;
            buttonTimeout.Click += buttonTimeout_Click;
            // 
            // buttonLogging
            // 
            buttonLogging.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonLogging.FlatAppearance.BorderSize = 0;
            buttonLogging.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonLogging.Image = Resource.List;
            buttonLogging.Location = new System.Drawing.Point(873, 28);
            buttonLogging.Margin = new System.Windows.Forms.Padding(0);
            buttonLogging.Name = "buttonLogging";
            buttonLogging.Size = new System.Drawing.Size(30, 29);
            buttonLogging.TabIndex = 13;
            buttonLogging.UseVisualStyleBackColor = true;
            buttonLogging.Click += buttonLogging_Click;
            // 
            // buttonSettings
            // 
            buttonSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonSettings.FlatAppearance.BorderSize = 0;
            buttonSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonSettings.Image = Resource.Settings;
            buttonSettings.Location = new System.Drawing.Point(849, 3);
            buttonSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonSettings.Name = "buttonSettings";
            buttonSettings.Size = new System.Drawing.Size(20, 22);
            buttonSettings.TabIndex = 14;
            buttonSettings.UseVisualStyleBackColor = true;
            buttonSettings.Click += buttonSettings_Click;
            // 
            // buttonStopOnError
            // 
            buttonStopOnError.FlatAppearance.BorderSize = 0;
            buttonStopOnError.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonStopOnError.Image = Resource.ArrowStop;
            buttonStopOnError.Location = new System.Drawing.Point(849, 31);
            buttonStopOnError.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonStopOnError.Name = "buttonStopOnError";
            buttonStopOnError.Size = new System.Drawing.Size(20, 22);
            buttonStopOnError.TabIndex = 15;
            toolTip.SetToolTip(buttonStopOnError, "Do not stop on error");
            buttonStopOnError.UseVisualStyleBackColor = true;
            buttonStopOnError.Click += buttonStopOnError_Click;
            // 
            // pictureBoxBulkTransfer
            // 
            pictureBoxBulkTransfer.Image = Resource.TransferToFile;
            pictureBoxBulkTransfer.Location = new System.Drawing.Point(780, 9);
            pictureBoxBulkTransfer.Margin = new System.Windows.Forms.Padding(0, 9, 0, 0);
            pictureBoxBulkTransfer.Name = "pictureBoxBulkTransfer";
            pictureBoxBulkTransfer.Size = new System.Drawing.Size(19, 18);
            pictureBoxBulkTransfer.TabIndex = 16;
            pictureBoxBulkTransfer.TabStop = false;
            pictureBoxBulkTransfer.Visible = false;
            // 
            // imageList
            // 
            imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList.ImageStream");
            imageList.TransparentColor = System.Drawing.Color.Transparent;
            imageList.Images.SetKeyName(0, "Database.ico");
            imageList.Images.SetKeyName(1, "CacheDB.ico");
            imageList.Images.SetKeyName(2, "Postgres.ico");
            imageList.Images.SetKeyName(3, "Package.ico");
            // 
            // FormCacheDatabaseTransfer
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(903, 733);
            Controls.Add(panelTransferSteps);
            Controls.Add(tableLayoutPanelMain);
            helpProvider.SetHelpKeyword(this, "cachedatabase_transfer_dc");
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormCacheDatabaseTransfer";
            helpProvider.SetShowHelp(this, true);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "FormCacheDatabaseTransfer";
            KeyDown += Form_KeyDown;
            tableLayoutPanelMain.ResumeLayout(false);
            tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxTarget).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxBulkTransfer).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTransferSteps;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Button buttonStartTransfer;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.Label labelHaeder;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Button buttonSelectNone;
        private System.Windows.Forms.PictureBox pictureBoxTarget;
        private System.Windows.Forms.PictureBox pictureBoxSource;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Label labelHeaderSource;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.Label labelCountTarget;
        private System.Windows.Forms.Label labelCountSource;
        private System.Windows.Forms.Label labelTarget;
        private System.Windows.Forms.Button buttonTimeout;
        private System.Windows.Forms.Button buttonLogging;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Button buttonStopOnError;
        private System.Windows.Forms.PictureBox pictureBoxBulkTransfer;
    }
}