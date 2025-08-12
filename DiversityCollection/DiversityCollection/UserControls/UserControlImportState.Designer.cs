namespace DiversityCollection.UserControls
{
    partial class UserControlImportState
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlImportState));
            this.pictureBoxStep = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxStatus = new System.Windows.Forms.PictureBox();
            this.labelStep = new System.Windows.Forms.Label();
            this.pictureBoxCurrent = new System.Windows.Forms.PictureBox();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.pictureBoxWait = new System.Windows.Forms.PictureBox();
            this.imageListStatus = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStep)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCurrent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWait)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxStep
            // 
            this.pictureBoxStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxStep.Location = new System.Drawing.Point(16, 0);
            this.pictureBoxStep.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxStep.Name = "pictureBoxStep";
            this.pictureBoxStep.Size = new System.Drawing.Size(18, 16);
            this.pictureBoxStep.TabIndex = 0;
            this.pictureBoxStep.TabStop = false;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel.Controls.Add(this.pictureBoxStep, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxStatus, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.labelStep, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxCurrent, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxMessage, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxWait, 4, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(322, 73);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // pictureBoxStatus
            // 
            this.pictureBoxStatus.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBoxStatus.Location = new System.Drawing.Point(286, 0);
            this.pictureBoxStatus.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxStatus.Name = "pictureBoxStatus";
            this.pictureBoxStatus.Size = new System.Drawing.Size(18, 16);
            this.pictureBoxStatus.TabIndex = 1;
            this.pictureBoxStatus.TabStop = false;
            // 
            // labelStep
            // 
            this.labelStep.AutoSize = true;
            this.labelStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStep.Location = new System.Drawing.Point(34, 0);
            this.labelStep.Margin = new System.Windows.Forms.Padding(0);
            this.labelStep.Name = "labelStep";
            this.labelStep.Size = new System.Drawing.Size(252, 16);
            this.labelStep.TabIndex = 2;
            this.labelStep.Text = "Step";
            // 
            // pictureBoxCurrent
            // 
            this.pictureBoxCurrent.Image = global::DiversityCollection.Resource.ArrowNext1;
            this.pictureBoxCurrent.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxCurrent.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxCurrent.Name = "pictureBoxCurrent";
            this.pictureBoxCurrent.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxCurrent.TabIndex = 4;
            this.pictureBoxCurrent.TabStop = false;
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.BackColor = System.Drawing.Color.Yellow;
            this.tableLayoutPanel.SetColumnSpan(this.textBoxMessage, 4);
            this.textBoxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMessage.Location = new System.Drawing.Point(19, 19);
            this.textBoxMessage.Multiline = true;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.ReadOnly = true;
            this.textBoxMessage.Size = new System.Drawing.Size(300, 51);
            this.textBoxMessage.TabIndex = 5;
            // 
            // pictureBoxWait
            // 
            this.pictureBoxWait.Image = global::DiversityCollection.Resource.wait_animation;
            this.pictureBoxWait.Location = new System.Drawing.Point(304, 0);
            this.pictureBoxWait.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxWait.Name = "pictureBoxWait";
            this.pictureBoxWait.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxWait.TabIndex = 6;
            this.pictureBoxWait.TabStop = false;
            this.pictureBoxWait.Visible = false;
            // 
            // imageListStatus
            // 
            this.imageListStatus.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListStatus.ImageStream")));
            this.imageListStatus.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListStatus.Images.SetKeyName(0, "OK.gif");
            this.imageListStatus.Images.SetKeyName(1, "Warning.ico");
            this.imageListStatus.Images.SetKeyName(2, "error.gif");
            // 
            // UserControlImportState
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlImportState";
            this.Size = new System.Drawing.Size(322, 73);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStep)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCurrent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWait)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxStep;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.PictureBox pictureBoxStatus;
        private System.Windows.Forms.ImageList imageListStatus;
        private System.Windows.Forms.Label labelStep;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox pictureBoxCurrent;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.PictureBox pictureBoxWait;
    }
}
