namespace DiversityWorkbench.Forms
{
    partial class FormReplicationTools
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReplicationTools));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageSyncRowGUID = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelSyncRowGUID = new System.Windows.Forms.TableLayoutPanel();
            this.labelSyncRowGUIDtable = new System.Windows.Forms.Label();
            this.comboBoxSyncRowGUIDtable = new System.Windows.Forms.ComboBox();
            this.dataGridViewSyncRowGUID = new System.Windows.Forms.DataGridView();
            this.buttonSyncRowGUIDupdate = new System.Windows.Forms.Button();
            this.pictureBoxSubscriber = new System.Windows.Forms.PictureBox();
            this.labelSubscriber = new System.Windows.Forms.Label();
            this.labelPublisher = new System.Windows.Forms.Label();
            this.pictureBoxPublisher = new System.Windows.Forms.PictureBox();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.labelSub = new System.Windows.Forms.Label();
            this.labelPub = new System.Windows.Forms.Label();
            this.tableLayoutPanelMain.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageSyncRowGUID.SuspendLayout();
            this.tableLayoutPanelSyncRowGUID.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSyncRowGUID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSubscriber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPublisher)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 5;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.Controls.Add(this.tabControlMain, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.pictureBoxSubscriber, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelSubscriber, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelPublisher, 3, 1);
            this.tableLayoutPanelMain.Controls.Add(this.pictureBoxPublisher, 4, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelSub, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.labelPub, 3, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(781, 539);
            this.tableLayoutPanelMain.TabIndex = 2;
            // 
            // tabControlMain
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.tabControlMain, 5);
            this.tabControlMain.Controls.Add(this.tabPageSyncRowGUID);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(3, 45);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(775, 491);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageSyncRowGUID
            // 
            this.tabPageSyncRowGUID.Controls.Add(this.tableLayoutPanelSyncRowGUID);
            this.tabPageSyncRowGUID.Location = new System.Drawing.Point(4, 22);
            this.tabPageSyncRowGUID.Name = "tabPageSyncRowGUID";
            this.tabPageSyncRowGUID.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSyncRowGUID.Size = new System.Drawing.Size(767, 465);
            this.tabPageSyncRowGUID.TabIndex = 0;
            this.tabPageSyncRowGUID.Text = "Synchronize RowGUID";
            this.tabPageSyncRowGUID.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelSyncRowGUID
            // 
            this.tableLayoutPanelSyncRowGUID.ColumnCount = 2;
            this.tableLayoutPanelSyncRowGUID.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSyncRowGUID.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSyncRowGUID.Controls.Add(this.labelSyncRowGUIDtable, 0, 0);
            this.tableLayoutPanelSyncRowGUID.Controls.Add(this.comboBoxSyncRowGUIDtable, 1, 0);
            this.tableLayoutPanelSyncRowGUID.Controls.Add(this.dataGridViewSyncRowGUID, 0, 1);
            this.tableLayoutPanelSyncRowGUID.Controls.Add(this.buttonSyncRowGUIDupdate, 0, 2);
            this.tableLayoutPanelSyncRowGUID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSyncRowGUID.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelSyncRowGUID.Name = "tableLayoutPanelSyncRowGUID";
            this.tableLayoutPanelSyncRowGUID.RowCount = 3;
            this.tableLayoutPanelSyncRowGUID.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSyncRowGUID.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSyncRowGUID.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSyncRowGUID.Size = new System.Drawing.Size(761, 459);
            this.tableLayoutPanelSyncRowGUID.TabIndex = 0;
            // 
            // labelSyncRowGUIDtable
            // 
            this.labelSyncRowGUIDtable.AutoSize = true;
            this.labelSyncRowGUIDtable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSyncRowGUIDtable.Location = new System.Drawing.Point(3, 0);
            this.labelSyncRowGUIDtable.Name = "labelSyncRowGUIDtable";
            this.labelSyncRowGUIDtable.Size = new System.Drawing.Size(362, 27);
            this.labelSyncRowGUIDtable.TabIndex = 0;
            this.labelSyncRowGUIDtable.Text = "Select the table for which the RowGUID should be taken from the pubilsher";
            this.labelSyncRowGUIDtable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxSyncRowGUIDtable
            // 
            this.comboBoxSyncRowGUIDtable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxSyncRowGUIDtable.FormattingEnabled = true;
            this.comboBoxSyncRowGUIDtable.Location = new System.Drawing.Point(371, 3);
            this.comboBoxSyncRowGUIDtable.Name = "comboBoxSyncRowGUIDtable";
            this.comboBoxSyncRowGUIDtable.Size = new System.Drawing.Size(387, 21);
            this.comboBoxSyncRowGUIDtable.TabIndex = 1;
            this.comboBoxSyncRowGUIDtable.SelectionChangeCommitted += new System.EventHandler(this.comboBoxSyncRowGUIDtable_SelectionChangeCommitted);
            // 
            // dataGridViewSyncRowGUID
            // 
            this.dataGridViewSyncRowGUID.AllowUserToAddRows = false;
            this.dataGridViewSyncRowGUID.AllowUserToDeleteRows = false;
            this.dataGridViewSyncRowGUID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanelSyncRowGUID.SetColumnSpan(this.dataGridViewSyncRowGUID, 2);
            this.dataGridViewSyncRowGUID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSyncRowGUID.Location = new System.Drawing.Point(3, 30);
            this.dataGridViewSyncRowGUID.Name = "dataGridViewSyncRowGUID";
            this.dataGridViewSyncRowGUID.ReadOnly = true;
            this.dataGridViewSyncRowGUID.Size = new System.Drawing.Size(755, 396);
            this.dataGridViewSyncRowGUID.TabIndex = 2;
            // 
            // buttonSyncRowGUIDupdate
            // 
            this.buttonSyncRowGUIDupdate.Image = global::DiversityWorkbench.Properties.Resources.UpdateDatabase;
            this.buttonSyncRowGUIDupdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSyncRowGUIDupdate.Location = new System.Drawing.Point(3, 432);
            this.buttonSyncRowGUIDupdate.Name = "buttonSyncRowGUIDupdate";
            this.buttonSyncRowGUIDupdate.Size = new System.Drawing.Size(91, 24);
            this.buttonSyncRowGUIDupdate.TabIndex = 3;
            this.buttonSyncRowGUIDupdate.Text = "Start update";
            this.buttonSyncRowGUIDupdate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSyncRowGUIDupdate.UseVisualStyleBackColor = true;
            this.buttonSyncRowGUIDupdate.Click += new System.EventHandler(this.buttonSyncRowGUIDupdate_Click);
            // 
            // pictureBoxSubscriber
            // 
            this.pictureBoxSubscriber.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBoxSubscriber.Image = global::DiversityWorkbench.Properties.Resources.Laptop;
            this.pictureBoxSubscriber.Location = new System.Drawing.Point(302, 23);
            this.pictureBoxSubscriber.Name = "pictureBoxSubscriber";
            this.pictureBoxSubscriber.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxSubscriber.TabIndex = 1;
            this.pictureBoxSubscriber.TabStop = false;
            // 
            // labelSubscriber
            // 
            this.labelSubscriber.AutoSize = true;
            this.labelSubscriber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSubscriber.Location = new System.Drawing.Point(324, 20);
            this.labelSubscriber.Name = "labelSubscriber";
            this.labelSubscriber.Size = new System.Drawing.Size(57, 22);
            this.labelSubscriber.TabIndex = 2;
            this.labelSubscriber.Text = "label1";
            this.labelSubscriber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelPublisher
            // 
            this.labelPublisher.AutoSize = true;
            this.labelPublisher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPublisher.Location = new System.Drawing.Point(407, 20);
            this.labelPublisher.Name = "labelPublisher";
            this.labelPublisher.Size = new System.Drawing.Size(50, 22);
            this.labelPublisher.TabIndex = 3;
            this.labelPublisher.Text = "label1";
            this.labelPublisher.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBoxPublisher
            // 
            this.pictureBoxPublisher.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxPublisher.Image = global::DiversityWorkbench.ResourceWorkbench.Database;
            this.pictureBoxPublisher.Location = new System.Drawing.Point(463, 23);
            this.pictureBoxPublisher.Name = "pictureBoxPublisher";
            this.pictureBoxPublisher.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxPublisher.TabIndex = 4;
            this.pictureBoxPublisher.TabStop = false;
            // 
            // labelSub
            // 
            this.labelSub.AutoSize = true;
            this.labelSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSub.Location = new System.Drawing.Point(324, 0);
            this.labelSub.Name = "labelSub";
            this.labelSub.Size = new System.Drawing.Size(57, 20);
            this.labelSub.TabIndex = 5;
            this.labelSub.Text = "Subscriber";
            this.labelSub.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelPub
            // 
            this.labelPub.AutoSize = true;
            this.labelPub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPub.Location = new System.Drawing.Point(407, 0);
            this.labelPub.Name = "labelPub";
            this.labelPub.Size = new System.Drawing.Size(50, 20);
            this.labelPub.TabIndex = 6;
            this.labelPub.Text = "Publisher";
            this.labelPub.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // FormReplicationTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 539);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormReplicationTools";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tools for replication";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageSyncRowGUID.ResumeLayout(false);
            this.tableLayoutPanelSyncRowGUID.ResumeLayout(false);
            this.tableLayoutPanelSyncRowGUID.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSyncRowGUID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSubscriber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPublisher)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageSyncRowGUID;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSyncRowGUID;
        private System.Windows.Forms.Label labelSyncRowGUIDtable;
        private System.Windows.Forms.ComboBox comboBoxSyncRowGUIDtable;
        private System.Windows.Forms.DataGridView dataGridViewSyncRowGUID;
        private System.Windows.Forms.Button buttonSyncRowGUIDupdate;
        private System.Windows.Forms.PictureBox pictureBoxSubscriber;
        private System.Windows.Forms.Label labelSubscriber;
        private System.Windows.Forms.Label labelPublisher;
        private System.Windows.Forms.PictureBox pictureBoxPublisher;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Label labelSub;
        private System.Windows.Forms.Label labelPub;
    }
}