namespace DiversityCollection.CacheDatabase
{
    partial class UserControlExchangeCongruence
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlExchangeCongruence));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxCongruenceStatus = new System.Windows.Forms.PictureBox();
            this.labelMessage = new System.Windows.Forms.Label();
            this.groupBoxPackages = new System.Windows.Forms.GroupBox();
            this.groupBoxContent = new System.Windows.Forms.GroupBox();
            this.buttonInfo = new System.Windows.Forms.Button();
            this.imageListStatus = new System.Windows.Forms.ImageList(this.components);
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelPackages = new System.Windows.Forms.Panel();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCongruenceStatus)).BeginInit();
            this.groupBoxPackages.SuspendLayout();
            this.groupBoxContent.SuspendLayout();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.pictureBoxCongruenceStatus, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelMessage, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.groupBoxPackages, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.groupBoxContent, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonInfo, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(537, 52);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // pictureBoxCongruenceStatus
            // 
            this.pictureBoxCongruenceStatus.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxCongruenceStatus.Name = "pictureBoxCongruenceStatus";
            this.pictureBoxCongruenceStatus.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxCongruenceStatus.TabIndex = 0;
            this.pictureBoxCongruenceStatus.TabStop = false;
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelMessage, 2);
            this.labelMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMessage.Location = new System.Drawing.Point(3, 23);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(36, 29);
            this.labelMessage.TabIndex = 1;
            this.labelMessage.Text = "OK";
            // 
            // groupBoxPackages
            // 
            this.groupBoxPackages.Controls.Add(this.panelPackages);
            this.groupBoxPackages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxPackages.Location = new System.Drawing.Point(337, 0);
            this.groupBoxPackages.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxPackages.Name = "groupBoxPackages";
            this.tableLayoutPanel.SetRowSpan(this.groupBoxPackages, 2);
            this.groupBoxPackages.Size = new System.Drawing.Size(200, 52);
            this.groupBoxPackages.TabIndex = 3;
            this.groupBoxPackages.TabStop = false;
            this.groupBoxPackages.Text = "Packages";
            this.groupBoxPackages.Visible = false;
            // 
            // groupBoxContent
            // 
            this.groupBoxContent.Controls.Add(this.panelContent);
            this.groupBoxContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxContent.Location = new System.Drawing.Point(45, 0);
            this.groupBoxContent.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.groupBoxContent.Name = "groupBoxContent";
            this.tableLayoutPanel.SetRowSpan(this.groupBoxContent, 2);
            this.groupBoxContent.Size = new System.Drawing.Size(289, 52);
            this.groupBoxContent.TabIndex = 4;
            this.groupBoxContent.TabStop = false;
            this.groupBoxContent.Text = "Content";
            this.groupBoxContent.Visible = false;
            // 
            // buttonInfo
            // 
            this.buttonInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonInfo.FlatAppearance.BorderSize = 0;
            this.buttonInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonInfo.Image = global::DiversityCollection.Resource.Manual;
            this.buttonInfo.Location = new System.Drawing.Point(22, 0);
            this.buttonInfo.Margin = new System.Windows.Forms.Padding(0);
            this.buttonInfo.Name = "buttonInfo";
            this.buttonInfo.Size = new System.Drawing.Size(20, 23);
            this.buttonInfo.TabIndex = 5;
            this.buttonInfo.UseVisualStyleBackColor = true;
            // 
            // imageListStatus
            // 
            this.imageListStatus.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListStatus.ImageStream")));
            this.imageListStatus.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListStatus.Images.SetKeyName(0, "Add.ico");
            this.imageListStatus.Images.SetKeyName(1, "OK.ico");
            this.imageListStatus.Images.SetKeyName(2, "Conflict3.ico");
            this.imageListStatus.Images.SetKeyName(3, "Update.ico");
            this.imageListStatus.Images.SetKeyName(4, "Error.ico");
            this.imageListStatus.Images.SetKeyName(5, "Package.ico");
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.tableLayoutPanel);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(543, 71);
            this.groupBox.TabIndex = 1;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Source";
            // 
            // panelContent
            // 
            this.panelContent.AutoScroll = true;
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(3, 16);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(283, 33);
            this.panelContent.TabIndex = 0;
            // 
            // panelPackages
            // 
            this.panelPackages.AutoScroll = true;
            this.panelPackages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPackages.Location = new System.Drawing.Point(3, 16);
            this.panelPackages.Name = "panelPackages";
            this.panelPackages.Size = new System.Drawing.Size(194, 33);
            this.panelPackages.TabIndex = 0;
            // 
            // UserControlExchangeCongruence
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox);
            this.Name = "UserControlExchangeCongruence";
            this.Size = new System.Drawing.Size(543, 71);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCongruenceStatus)).EndInit();
            this.groupBoxPackages.ResumeLayout(false);
            this.groupBoxContent.ResumeLayout(false);
            this.groupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.PictureBox pictureBoxCongruenceStatus;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.ImageList imageListStatus;
        private System.Windows.Forms.GroupBox groupBoxPackages;
        private System.Windows.Forms.GroupBox groupBoxContent;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Button buttonInfo;
        private System.Windows.Forms.Panel panelPackages;
        private System.Windows.Forms.Panel panelContent;
    }
}
