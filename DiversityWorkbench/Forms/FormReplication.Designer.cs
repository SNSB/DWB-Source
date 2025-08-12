namespace DiversityWorkbench.Forms
{
    partial class FormReplication
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReplication));
            this.tableLayoutPanelHeader = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxProject = new System.Windows.Forms.ComboBox();
            this.labelProject = new System.Windows.Forms.Label();
            this.pictureBoxServer = new System.Windows.Forms.PictureBox();
            this.panelSubscriber = new System.Windows.Forms.Panel();
            this.pictureBoxSubscriber = new System.Windows.Forms.PictureBox();
            this.labelSubscriberDatabase = new System.Windows.Forms.Label();
            this.panelPublisher = new System.Windows.Forms.Panel();
            this.pictureBoxPublisher = new System.Windows.Forms.PictureBox();
            this.labelServerDatabase = new System.Windows.Forms.Label();
            this.labelHeaderSubscriber = new System.Windows.Forms.Label();
            this.labelHeaderPublisher = new System.Windows.Forms.Label();
            this.buttonUpload = new System.Windows.Forms.Button();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.buttonClean = new System.Windows.Forms.Button();
            this.buttonStartMerge = new System.Windows.Forms.Button();
            this.buttonStartDownload = new System.Windows.Forms.Button();
            this.panelTableList = new System.Windows.Forms.Panel();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonNone = new System.Windows.Forms.RadioButton();
            this.radioButtonAll = new System.Windows.Forms.RadioButton();
            this.buttonRequery = new System.Windows.Forms.Button();
            this.panelTableGroups = new System.Windows.Forms.Panel();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxServer)).BeginInit();
            this.panelSubscriber.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSubscriber)).BeginInit();
            this.panelPublisher.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPublisher)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelHeader
            // 
            this.tableLayoutPanelHeader.ColumnCount = 7;
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.Controls.Add(this.comboBoxProject, 4, 2);
            this.tableLayoutPanelHeader.Controls.Add(this.labelProject, 0, 2);
            this.tableLayoutPanelHeader.Controls.Add(this.pictureBoxServer, 3, 1);
            this.tableLayoutPanelHeader.Controls.Add(this.panelSubscriber, 0, 1);
            this.tableLayoutPanelHeader.Controls.Add(this.panelPublisher, 4, 1);
            this.tableLayoutPanelHeader.Controls.Add(this.labelHeaderSubscriber, 2, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.labelHeaderPublisher, 4, 0);
            this.tableLayoutPanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelHeader.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelHeader.Name = "tableLayoutPanelHeader";
            this.tableLayoutPanelHeader.RowCount = 3;
            this.tableLayoutPanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelHeader.Size = new System.Drawing.Size(561, 77);
            this.tableLayoutPanelHeader.TabIndex = 2;
            // 
            // comboBoxProject
            // 
            this.tableLayoutPanelHeader.SetColumnSpan(this.comboBoxProject, 2);
            this.comboBoxProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxProject.FormattingEnabled = true;
            this.comboBoxProject.Location = new System.Drawing.Point(250, 53);
            this.comboBoxProject.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxProject.Name = "comboBoxProject";
            this.comboBoxProject.Size = new System.Drawing.Size(213, 21);
            this.comboBoxProject.TabIndex = 22;
            this.comboBoxProject.Visible = false;
            // 
            // labelProject
            // 
            this.labelProject.AutoSize = true;
            this.tableLayoutPanelHeader.SetColumnSpan(this.labelProject, 3);
            this.labelProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProject.Location = new System.Drawing.Point(3, 50);
            this.labelProject.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(231, 27);
            this.labelProject.TabIndex = 28;
            this.labelProject.Text = "Project:";
            this.labelProject.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBoxServer
            // 
            this.pictureBoxServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxServer.Image = global::DiversityWorkbench.ResourceWorkbench.Database;
            this.pictureBoxServer.Location = new System.Drawing.Point(234, 27);
            this.pictureBoxServer.Margin = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.pictureBoxServer.Name = "pictureBoxServer";
            this.pictureBoxServer.Size = new System.Drawing.Size(16, 23);
            this.pictureBoxServer.TabIndex = 32;
            this.pictureBoxServer.TabStop = false;
            // 
            // panelSubscriber
            // 
            this.tableLayoutPanelHeader.SetColumnSpan(this.panelSubscriber, 3);
            this.panelSubscriber.Controls.Add(this.pictureBoxSubscriber);
            this.panelSubscriber.Controls.Add(this.labelSubscriberDatabase);
            this.panelSubscriber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSubscriber.Location = new System.Drawing.Point(0, 20);
            this.panelSubscriber.Margin = new System.Windows.Forms.Padding(0);
            this.panelSubscriber.Name = "panelSubscriber";
            this.panelSubscriber.Padding = new System.Windows.Forms.Padding(0, 14, 0, 0);
            this.panelSubscriber.Size = new System.Drawing.Size(234, 30);
            this.panelSubscriber.TabIndex = 44;
            // 
            // pictureBoxSubscriber
            // 
            this.pictureBoxSubscriber.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBoxSubscriber.Image = global::DiversityWorkbench.ResourceWorkbench.Upload;
            this.pictureBoxSubscriber.Location = new System.Drawing.Point(151, 14);
            this.pictureBoxSubscriber.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxSubscriber.Name = "pictureBoxSubscriber";
            this.pictureBoxSubscriber.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxSubscriber.TabIndex = 44;
            this.pictureBoxSubscriber.TabStop = false;
            // 
            // labelSubscriberDatabase
            // 
            this.labelSubscriberDatabase.AutoSize = true;
            this.labelSubscriberDatabase.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelSubscriberDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSubscriberDatabase.Location = new System.Drawing.Point(167, 14);
            this.labelSubscriberDatabase.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelSubscriberDatabase.Name = "labelSubscriberDatabase";
            this.labelSubscriberDatabase.Size = new System.Drawing.Size(67, 13);
            this.labelSubscriberDatabase.TabIndex = 43;
            this.labelSubscriberDatabase.Text = "Subscriber";
            this.labelSubscriberDatabase.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // panelPublisher
            // 
            this.tableLayoutPanelHeader.SetColumnSpan(this.panelPublisher, 3);
            this.panelPublisher.Controls.Add(this.pictureBoxPublisher);
            this.panelPublisher.Controls.Add(this.labelServerDatabase);
            this.panelPublisher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPublisher.Location = new System.Drawing.Point(250, 20);
            this.panelPublisher.Margin = new System.Windows.Forms.Padding(0);
            this.panelPublisher.Name = "panelPublisher";
            this.panelPublisher.Size = new System.Drawing.Size(311, 30);
            this.panelPublisher.TabIndex = 45;
            // 
            // pictureBoxPublisher
            // 
            this.pictureBoxPublisher.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxPublisher.Image = global::DiversityWorkbench.ResourceWorkbench.Download;
            this.pictureBoxPublisher.Location = new System.Drawing.Point(59, 0);
            this.pictureBoxPublisher.Name = "pictureBoxPublisher";
            this.pictureBoxPublisher.Size = new System.Drawing.Size(16, 30);
            this.pictureBoxPublisher.TabIndex = 34;
            this.pictureBoxPublisher.TabStop = false;
            // 
            // labelServerDatabase
            // 
            this.labelServerDatabase.AutoSize = true;
            this.labelServerDatabase.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelServerDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelServerDatabase.Location = new System.Drawing.Point(0, 0);
            this.labelServerDatabase.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelServerDatabase.Name = "labelServerDatabase";
            this.labelServerDatabase.Size = new System.Drawing.Size(59, 13);
            this.labelServerDatabase.TabIndex = 33;
            this.labelServerDatabase.Text = "Publisher";
            // 
            // labelHeaderSubscriber
            // 
            this.labelHeaderSubscriber.AutoSize = true;
            this.labelHeaderSubscriber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeaderSubscriber.Location = new System.Drawing.Point(174, 0);
            this.labelHeaderSubscriber.Name = "labelHeaderSubscriber";
            this.labelHeaderSubscriber.Size = new System.Drawing.Size(57, 20);
            this.labelHeaderSubscriber.TabIndex = 46;
            this.labelHeaderSubscriber.Text = "Subscriber";
            this.labelHeaderSubscriber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelHeaderPublisher
            // 
            this.labelHeaderPublisher.AutoSize = true;
            this.labelHeaderPublisher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeaderPublisher.Location = new System.Drawing.Point(253, 0);
            this.labelHeaderPublisher.Name = "labelHeaderPublisher";
            this.labelHeaderPublisher.Size = new System.Drawing.Size(102, 20);
            this.labelHeaderPublisher.TabIndex = 47;
            this.labelHeaderPublisher.Text = "Publisher";
            this.labelHeaderPublisher.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonUpload
            // 
            this.buttonUpload.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonUpload.Image = global::DiversityWorkbench.ResourceWorkbench.Upload;
            this.buttonUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonUpload.Location = new System.Drawing.Point(464, 0);
            this.buttonUpload.Name = "buttonUpload";
            this.buttonUpload.Size = new System.Drawing.Size(91, 34);
            this.buttonUpload.TabIndex = 31;
            this.buttonUpload.Text = "Start upload";
            this.buttonUpload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonUpload.UseVisualStyleBackColor = true;
            // 
            // panelButtons
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.panelButtons, 3);
            this.panelButtons.Controls.Add(this.buttonClean);
            this.panelButtons.Controls.Add(this.buttonStartMerge);
            this.panelButtons.Controls.Add(this.buttonStartDownload);
            this.panelButtons.Controls.Add(this.buttonUpload);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelButtons.Location = new System.Drawing.Point(3, 454);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(555, 34);
            this.panelButtons.TabIndex = 3;
            // 
            // buttonClean
            // 
            this.buttonClean.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonClean.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClean.ForeColor = System.Drawing.Color.Red;
            this.buttonClean.Image = global::DiversityWorkbench.ResourceWorkbench.CleanDatabase;
            this.buttonClean.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonClean.Location = new System.Drawing.Point(149, 0);
            this.buttonClean.Name = "buttonClean";
            this.buttonClean.Size = new System.Drawing.Size(125, 34);
            this.buttonClean.TabIndex = 50;
            this.buttonClean.Text = "Clean database";
            this.buttonClean.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonClean.UseVisualStyleBackColor = true;
            this.buttonClean.Visible = false;
            // 
            // buttonStartMerge
            // 
            this.buttonStartMerge.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonStartMerge.Image = global::DiversityWorkbench.Properties.Resources.Merge;
            this.buttonStartMerge.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonStartMerge.Location = new System.Drawing.Point(274, 0);
            this.buttonStartMerge.Name = "buttonStartMerge";
            this.buttonStartMerge.Size = new System.Drawing.Size(85, 34);
            this.buttonStartMerge.TabIndex = 49;
            this.buttonStartMerge.Text = "Start merge";
            this.buttonStartMerge.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonStartMerge.UseVisualStyleBackColor = true;
            this.buttonStartMerge.Visible = false;
            // 
            // buttonStartDownload
            // 
            this.buttonStartDownload.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonStartDownload.Image = global::DiversityWorkbench.ResourceWorkbench.Download;
            this.buttonStartDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonStartDownload.Location = new System.Drawing.Point(359, 0);
            this.buttonStartDownload.Name = "buttonStartDownload";
            this.buttonStartDownload.Size = new System.Drawing.Size(105, 34);
            this.buttonStartDownload.TabIndex = 32;
            this.buttonStartDownload.Text = "Start download";
            this.buttonStartDownload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonStartDownload.UseVisualStyleBackColor = true;
            // 
            // panelTableList
            // 
            this.panelTableList.AutoScroll = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.panelTableList, 3);
            this.panelTableList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTableList.Location = new System.Drawing.Point(3, 33);
            this.panelTableList.Name = "panelTableList";
            this.panelTableList.Size = new System.Drawing.Size(555, 415);
            this.panelTableList.TabIndex = 19;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 3;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.radioButtonNone, 2, 0);
            this.tableLayoutPanelMain.Controls.Add(this.radioButtonAll, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonRequery, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.panelTableList, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.panelButtons, 0, 2);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 113);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(561, 491);
            this.tableLayoutPanelMain.TabIndex = 20;
            // 
            // radioButtonNone
            // 
            this.radioButtonNone.AutoSize = true;
            this.radioButtonNone.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonNone.Location = new System.Drawing.Point(122, 0);
            this.radioButtonNone.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.radioButtonNone.Name = "radioButtonNone";
            this.radioButtonNone.Size = new System.Drawing.Size(49, 30);
            this.radioButtonNone.TabIndex = 45;
            this.radioButtonNone.TabStop = true;
            this.radioButtonNone.Text = "none";
            this.radioButtonNone.UseVisualStyleBackColor = true;
            // 
            // radioButtonAll
            // 
            this.radioButtonAll.AutoSize = true;
            this.radioButtonAll.Dock = System.Windows.Forms.DockStyle.Right;
            this.radioButtonAll.Location = new System.Drawing.Point(49, 0);
            this.radioButtonAll.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.radioButtonAll.Name = "radioButtonAll";
            this.radioButtonAll.Size = new System.Drawing.Size(67, 30);
            this.radioButtonAll.TabIndex = 44;
            this.radioButtonAll.TabStop = true;
            this.radioButtonAll.Text = "All tables";
            this.radioButtonAll.UseVisualStyleBackColor = true;
            // 
            // buttonRequery
            // 
            this.buttonRequery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRequery.Image = global::DiversityWorkbench.ResourceWorkbench.Transfrom;
            this.buttonRequery.Location = new System.Drawing.Point(3, 3);
            this.buttonRequery.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.buttonRequery.Name = "buttonRequery";
            this.buttonRequery.Size = new System.Drawing.Size(43, 27);
            this.buttonRequery.TabIndex = 43;
            this.buttonRequery.UseVisualStyleBackColor = true;
            // 
            // panelTableGroups
            // 
            this.panelTableGroups.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTableGroups.Location = new System.Drawing.Point(0, 77);
            this.panelTableGroups.Name = "panelTableGroups";
            this.panelTableGroups.Size = new System.Drawing.Size(561, 36);
            this.panelTableGroups.TabIndex = 21;
            // 
            // FormReplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 604);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Controls.Add(this.panelTableGroups);
            this.Controls.Add(this.tableLayoutPanelHeader);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormReplication";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Replication";
            this.tableLayoutPanelHeader.ResumeLayout(false);
            this.tableLayoutPanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxServer)).EndInit();
            this.panelSubscriber.ResumeLayout(false);
            this.panelSubscriber.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSubscriber)).EndInit();
            this.panelPublisher.ResumeLayout(false);
            this.panelPublisher.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPublisher)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelHeader;
        private System.Windows.Forms.ComboBox comboBoxProject;
        private System.Windows.Forms.Label labelProject;
        private System.Windows.Forms.Button buttonUpload;
        private System.Windows.Forms.PictureBox pictureBoxServer;
        private System.Windows.Forms.Panel panelSubscriber;
        private System.Windows.Forms.PictureBox pictureBoxSubscriber;
        private System.Windows.Forms.Label labelSubscriberDatabase;
        private System.Windows.Forms.Panel panelPublisher;
        private System.Windows.Forms.PictureBox pictureBoxPublisher;
        private System.Windows.Forms.Label labelServerDatabase;
        private System.Windows.Forms.Label labelHeaderSubscriber;
        private System.Windows.Forms.Label labelHeaderPublisher;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button buttonClean;
        private System.Windows.Forms.Button buttonStartMerge;
        private System.Windows.Forms.Button buttonStartDownload;
        private System.Windows.Forms.Panel panelTableList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.RadioButton radioButtonNone;
        private System.Windows.Forms.RadioButton radioButtonAll;
        private System.Windows.Forms.Button buttonRequery;
        private System.Windows.Forms.Panel panelTableGroups;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}