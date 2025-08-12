namespace DiversityWorkbench.Forms
{
    partial class FormUpdateClient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUpdateClient));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelSetClientVersion = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSetClientVersion = new System.Windows.Forms.Button();
            this.labelSetVersion = new System.Windows.Forms.Label();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.labelHeader = new System.Windows.Forms.Label();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanelMain.SuspendLayout();
            this.tableLayoutPanelSetClientVersion.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelSetClientVersion, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.splitContainer, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(488, 327);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // tableLayoutPanelSetClientVersion
            // 
            this.tableLayoutPanelSetClientVersion.ColumnCount = 2;
            this.tableLayoutPanelSetClientVersion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSetClientVersion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSetClientVersion.Controls.Add(this.buttonSetClientVersion, 1, 0);
            this.tableLayoutPanelSetClientVersion.Controls.Add(this.labelSetVersion, 0, 0);
            this.tableLayoutPanelSetClientVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelSetClientVersion.Location = new System.Drawing.Point(3, 269);
            this.tableLayoutPanelSetClientVersion.Name = "tableLayoutPanelSetClientVersion";
            this.tableLayoutPanelSetClientVersion.RowCount = 1;
            this.tableLayoutPanelSetClientVersion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSetClientVersion.Size = new System.Drawing.Size(482, 55);
            this.tableLayoutPanelSetClientVersion.TabIndex = 0;
            // 
            // buttonSetClientVersion
            // 
            this.buttonSetClientVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSetClientVersion.Image = global::DiversityWorkbench.Properties.Resources.UpdateDatabase;
            this.buttonSetClientVersion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSetClientVersion.Location = new System.Drawing.Point(349, 3);
            this.buttonSetClientVersion.Name = "buttonSetClientVersion";
            this.buttonSetClientVersion.Size = new System.Drawing.Size(130, 49);
            this.buttonSetClientVersion.TabIndex = 0;
            this.buttonSetClientVersion.Text = "Set client version in database";
            this.buttonSetClientVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSetClientVersion.UseVisualStyleBackColor = true;
            this.buttonSetClientVersion.Click += new System.EventHandler(this.buttonSetClientVersion_Click);
            // 
            // labelSetVersion
            // 
            this.labelSetVersion.AutoSize = true;
            this.labelSetVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSetVersion.Location = new System.Drawing.Point(3, 0);
            this.labelSetVersion.Name = "labelSetVersion";
            this.labelSetVersion.Size = new System.Drawing.Size(340, 55);
            this.labelSetVersion.TabIndex = 1;
            this.labelSetVersion.Text = "After the upload of the latest version, set the version in the database:";
            this.labelSetVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(3, 16);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.webBrowser);
            this.splitContainer.Panel2Collapsed = true;
            this.splitContainer.Size = new System.Drawing.Size(482, 247);
            this.splitContainer.SplitterDistance = 222;
            this.splitContainer.TabIndex = 0;
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(482, 247);
            this.webBrowser.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(35, 13);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "label1";
            // 
            // FormUpdateClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 327);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormUpdateClient";
            this.Text = "FormUpdateClient";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.tableLayoutPanelSetClientVersion.ResumeLayout(false);
            this.tableLayoutPanelSetClientVersion.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSetClientVersion;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Button buttonSetClientVersion;
        private System.Windows.Forms.Label labelSetVersion;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}