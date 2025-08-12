namespace DiversityWorkbench.Forms
{
    partial class FormPrivacyConsent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrivacyConsent));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label = new System.Windows.Forms.Label();
            this.buttonConsent = new System.Windows.Forms.Button();
            this.buttonReject = new System.Windows.Forms.Button();
            this.linkLabelUrlInfo = new System.Windows.Forms.LinkLabel();
            this.labelUrlInfo = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.label, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonConsent, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.buttonReject, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.linkLabelUrlInfo, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.labelUrlInfo, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(376, 157);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.label, 2);
            this.label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label.Location = new System.Drawing.Point(3, 0);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(370, 45);
            this.label.TabIndex = 0;
            this.label.Text = "The DiversityWorkbench stores and processes personal data. This data is used amon" +
    "gst others for security, history and feedback functionality.";
            this.label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonConsent
            // 
            this.buttonConsent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonConsent.ForeColor = System.Drawing.Color.Green;
            this.buttonConsent.Image = global::DiversityWorkbench.Properties.Resources.OK;
            this.buttonConsent.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonConsent.Location = new System.Drawing.Point(3, 90);
            this.buttonConsent.Name = "buttonConsent";
            this.buttonConsent.Size = new System.Drawing.Size(182, 64);
            this.buttonConsent.TabIndex = 1;
            this.buttonConsent.Text = "I consent to the storage and processing of my personal data in the DiversityWorkb" +
    "ench";
            this.buttonConsent.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonConsent.UseVisualStyleBackColor = true;
            this.buttonConsent.Click += new System.EventHandler(this.buttonConsent_Click);
            // 
            // buttonReject
            // 
            this.buttonReject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonReject.ForeColor = System.Drawing.Color.Red;
            this.buttonReject.Image = global::DiversityWorkbench.Properties.Resources.LoginMissing;
            this.buttonReject.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonReject.Location = new System.Drawing.Point(191, 90);
            this.buttonReject.Name = "buttonReject";
            this.buttonReject.Size = new System.Drawing.Size(182, 64);
            this.buttonReject.TabIndex = 2;
            this.buttonReject.Text = "I reject the storage and processing of my personal data in the DiversityWorkbench" +
    "";
            this.buttonReject.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonReject.UseVisualStyleBackColor = true;
            this.buttonReject.Click += new System.EventHandler(this.buttonReject_Click);
            // 
            // linkLabelUrlInfo
            // 
            this.linkLabelUrlInfo.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.linkLabelUrlInfo, 2);
            this.linkLabelUrlInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkLabelUrlInfo.Location = new System.Drawing.Point(3, 58);
            this.linkLabelUrlInfo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.linkLabelUrlInfo.Name = "linkLabelUrlInfo";
            this.linkLabelUrlInfo.Size = new System.Drawing.Size(370, 26);
            this.linkLabelUrlInfo.TabIndex = 3;
            this.linkLabelUrlInfo.TabStop = true;
            this.linkLabelUrlInfo.Text = "http://diversityworkbench.net/Portal/Default_Agreement_on_Processing_of_Personal_" +
    "Data_in_DWB_Software";
            this.linkLabelUrlInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabelUrlInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelUrlInfo_LinkClicked);
            // 
            // labelUrlInfo
            // 
            this.labelUrlInfo.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelUrlInfo, 2);
            this.labelUrlInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUrlInfo.Location = new System.Drawing.Point(3, 45);
            this.labelUrlInfo.Name = "labelUrlInfo";
            this.labelUrlInfo.Size = new System.Drawing.Size(370, 13);
            this.labelUrlInfo.TabIndex = 4;
            this.labelUrlInfo.Text = "For further information see";
            this.labelUrlInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormPrivacyConsent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 157);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPrivacyConsent";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Privacy Consent";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Button buttonConsent;
        private System.Windows.Forms.Button buttonReject;
        private System.Windows.Forms.LinkLabel linkLabelUrlInfo;
        private System.Windows.Forms.Label labelUrlInfo;
    }
}