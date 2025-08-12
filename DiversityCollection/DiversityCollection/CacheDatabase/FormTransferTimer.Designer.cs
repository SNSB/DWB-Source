namespace DiversityCollection.CacheDatabase
{
    partial class FormTransferTimer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTransferTimer));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonTransferCacheDB = new System.Windows.Forms.Button();
            this.buttonTransferPostgres = new System.Windows.Forms.Button();
            this.buttonStopTimer = new System.Windows.Forms.Button();
            this.labelTimesteps = new System.Windows.Forms.Label();
            this.buttonTimerImage = new System.Windows.Forms.Button();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 7;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel.Controls.Add(this.buttonTransferCacheDB, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.buttonTransferPostgres, 2, 3);
            this.tableLayoutPanel.Controls.Add(this.buttonStopTimer, 3, 5);
            this.tableLayoutPanel.Controls.Add(this.labelTimesteps, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonTimerImage, 3, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 6;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(231, 130);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // buttonTransferCacheDB
            // 
            this.buttonTransferCacheDB.BackColor = System.Drawing.Color.Thistle;
            this.tableLayoutPanel.SetColumnSpan(this.buttonTransferCacheDB, 3);
            this.buttonTransferCacheDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTransferCacheDB.FlatAppearance.BorderSize = 0;
            this.buttonTransferCacheDB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferCacheDB.Image = global::DiversityCollection.Resource.CacheDB;
            this.buttonTransferCacheDB.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonTransferCacheDB.Location = new System.Drawing.Point(34, 36);
            this.buttonTransferCacheDB.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.buttonTransferCacheDB.Name = "buttonTransferCacheDB";
            this.buttonTransferCacheDB.Size = new System.Drawing.Size(162, 23);
            this.buttonTransferCacheDB.TabIndex = 3;
            this.buttonTransferCacheDB.Text = "Transfer into Cache DB";
            this.buttonTransferCacheDB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonTransferCacheDB.UseVisualStyleBackColor = false;
            // 
            // buttonTransferPostgres
            // 
            this.buttonTransferPostgres.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableLayoutPanel.SetColumnSpan(this.buttonTransferPostgres, 3);
            this.buttonTransferPostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTransferPostgres.FlatAppearance.BorderSize = 0;
            this.buttonTransferPostgres.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferPostgres.Image = global::DiversityCollection.Resource.Postgres;
            this.buttonTransferPostgres.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonTransferPostgres.Location = new System.Drawing.Point(34, 59);
            this.buttonTransferPostgres.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransferPostgres.Name = "buttonTransferPostgres";
            this.buttonTransferPostgres.Size = new System.Drawing.Size(162, 23);
            this.buttonTransferPostgres.TabIndex = 4;
            this.buttonTransferPostgres.Text = "Transfer into Postgres DB";
            this.buttonTransferPostgres.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonTransferPostgres.UseVisualStyleBackColor = false;
            // 
            // buttonStopTimer
            // 
            this.buttonStopTimer.Image = global::DiversityCollection.Resource.Stop3;
            this.buttonStopTimer.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonStopTimer.Location = new System.Drawing.Point(57, 88);
            this.buttonStopTimer.Name = "buttonStopTimer";
            this.buttonStopTimer.Size = new System.Drawing.Size(116, 39);
            this.buttonStopTimer.TabIndex = 0;
            this.buttonStopTimer.Text = "Stop Timer";
            this.buttonStopTimer.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonStopTimer.UseVisualStyleBackColor = true;
            this.buttonStopTimer.Click += new System.EventHandler(this.buttonStopTimer_Click);
            // 
            // labelTimesteps
            // 
            this.labelTimesteps.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelTimesteps, 5);
            this.labelTimesteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTimesteps.Location = new System.Drawing.Point(23, 20);
            this.labelTimesteps.Name = "labelTimesteps";
            this.labelTimesteps.Size = new System.Drawing.Size(184, 13);
            this.labelTimesteps.TabIndex = 2;
            this.labelTimesteps.Text = "At:";
            this.labelTimesteps.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonTimerImage
            // 
            this.buttonTimerImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTimerImage.FlatAppearance.BorderSize = 0;
            this.buttonTimerImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTimerImage.Image = global::DiversityCollection.Resource.Time;
            this.buttonTimerImage.Location = new System.Drawing.Point(54, 0);
            this.buttonTimerImage.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTimerImage.Name = "buttonTimerImage";
            this.buttonTimerImage.Size = new System.Drawing.Size(122, 20);
            this.buttonTimerImage.TabIndex = 5;
            this.buttonTimerImage.UseVisualStyleBackColor = true;
            // 
            // FormTransferTimer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 130);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormTransferTimer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Timer for datatransfer into cache database";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonStopTimer;
        private System.Windows.Forms.Button buttonTransferCacheDB;
        private System.Windows.Forms.Button buttonTransferPostgres;
        private System.Windows.Forms.Label labelTimesteps;
        private System.Windows.Forms.Button buttonTimerImage;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}