namespace DiversityCollection.CacheDatabase
{
    partial class UserControlTransferStep
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlTransferStep));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelStep = new System.Windows.Forms.Label();
            this.buttonViewResult = new System.Windows.Forms.Button();
            this.labelCurrentCount = new System.Windows.Forms.Label();
            this.buttonInfo = new System.Windows.Forms.Button();
            this.pictureBoxStep = new System.Windows.Forms.PictureBox();
            this.imageListInfo = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStep)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.labelStep, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonViewResult, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.labelCurrentCount, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonInfo, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxStep, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(576, 30);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelStep
            // 
            this.labelStep.AutoSize = true;
            this.labelStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStep.Location = new System.Drawing.Point(20, 0);
            this.labelStep.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelStep.Name = "labelStep";
            this.labelStep.Size = new System.Drawing.Size(419, 30);
            this.labelStep.TabIndex = 0;
            this.labelStep.Text = "label1";
            this.labelStep.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonViewResult
            // 
            this.buttonViewResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonViewResult.Image = global::DiversityCollection.Resource.Lupe;
            this.buttonViewResult.Location = new System.Drawing.Point(549, 3);
            this.buttonViewResult.Name = "buttonViewResult";
            this.buttonViewResult.Size = new System.Drawing.Size(24, 24);
            this.buttonViewResult.TabIndex = 2;
            this.buttonViewResult.UseVisualStyleBackColor = true;
            this.buttonViewResult.Click += new System.EventHandler(this.buttonViewResult_Click);
            // 
            // labelCurrentCount
            // 
            this.labelCurrentCount.AutoSize = true;
            this.labelCurrentCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCurrentCount.Location = new System.Drawing.Point(445, 0);
            this.labelCurrentCount.Name = "labelCurrentCount";
            this.labelCurrentCount.Size = new System.Drawing.Size(74, 30);
            this.labelCurrentCount.TabIndex = 4;
            this.labelCurrentCount.Text = "Current count:";
            this.labelCurrentCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonInfo
            // 
            this.buttonInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonInfo.Image = global::DiversityCollection.Resource.wait_animation;
            this.buttonInfo.Location = new System.Drawing.Point(522, 3);
            this.buttonInfo.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.buttonInfo.Name = "buttonInfo";
            this.buttonInfo.Size = new System.Drawing.Size(24, 24);
            this.buttonInfo.TabIndex = 5;
            this.buttonInfo.UseVisualStyleBackColor = true;
            this.buttonInfo.Visible = false;
            this.buttonInfo.Click += new System.EventHandler(this.buttonInfo_Click);
            // 
            // pictureBoxStep
            // 
            this.pictureBoxStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxStep.Location = new System.Drawing.Point(2, 7);
            this.pictureBoxStep.Margin = new System.Windows.Forms.Padding(2, 7, 2, 7);
            this.pictureBoxStep.Name = "pictureBoxStep";
            this.pictureBoxStep.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxStep.TabIndex = 6;
            this.pictureBoxStep.TabStop = false;
            // 
            // imageListInfo
            // 
            this.imageListInfo.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListInfo.ImageStream")));
            this.imageListInfo.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListInfo.Images.SetKeyName(0, "OK.ico");
            this.imageListInfo.Images.SetKeyName(1, "info.ico");
            this.imageListInfo.Images.SetKeyName(2, "Error.ico");
            // 
            // UserControlTransferStep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlTransferStep";
            this.Size = new System.Drawing.Size(576, 30);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStep)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelStep;
        private System.Windows.Forms.Button buttonViewResult;
        private System.Windows.Forms.Label labelCurrentCount;
        private System.Windows.Forms.Button buttonInfo;
        private System.Windows.Forms.ImageList imageListInfo;
        private System.Windows.Forms.PictureBox pictureBoxStep;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
