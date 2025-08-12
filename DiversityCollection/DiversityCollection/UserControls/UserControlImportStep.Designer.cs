namespace DiversityCollection.UserControls
{
    partial class UserControlImportStep
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlImportStep));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxStep = new System.Windows.Forms.PictureBox();
            this.pictureBoxStatus = new System.Windows.Forms.PictureBox();
            this.labelStep = new System.Windows.Forms.Label();
            this.pictureBoxCurrent = new System.Windows.Forms.PictureBox();
            this.labelError = new System.Windows.Forms.Label();
            this.pictureBoxLevel = new System.Windows.Forms.PictureBox();
            this.imageListStatus = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonMustImport = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCurrent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 7;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel.Controls.Add(this.pictureBoxStep, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxStatus, 6, 0);
            this.tableLayoutPanel.Controls.Add(this.labelStep, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxCurrent, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelError, 5, 0);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxLevel, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonMustImport, 4, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(382, 18);
            this.tableLayoutPanel.TabIndex = 2;
            this.tableLayoutPanel.Click += new System.EventHandler(this.tableLayoutPanel_Click);
            // 
            // pictureBoxStep
            // 
            this.pictureBoxStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxStep.Location = new System.Drawing.Point(32, 1);
            this.pictureBoxStep.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.pictureBoxStep.Name = "pictureBoxStep";
            this.pictureBoxStep.Size = new System.Drawing.Size(16, 17);
            this.pictureBoxStep.TabIndex = 0;
            this.pictureBoxStep.TabStop = false;
            this.pictureBoxStep.Click += new System.EventHandler(this.pictureBoxStep_Click);
            // 
            // pictureBoxStatus
            // 
            this.pictureBoxStatus.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBoxStatus.Location = new System.Drawing.Point(364, 1);
            this.pictureBoxStatus.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.pictureBoxStatus.Name = "pictureBoxStatus";
            this.pictureBoxStatus.Size = new System.Drawing.Size(18, 17);
            this.pictureBoxStatus.TabIndex = 1;
            this.pictureBoxStatus.TabStop = false;
            // 
            // labelStep
            // 
            this.labelStep.AutoSize = true;
            this.labelStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStep.Location = new System.Drawing.Point(48, 2);
            this.labelStep.Margin = new System.Windows.Forms.Padding(0, 2, 0, 1);
            this.labelStep.Name = "labelStep";
            this.labelStep.Size = new System.Drawing.Size(29, 15);
            this.labelStep.TabIndex = 2;
            this.labelStep.Text = "Step";
            this.labelStep.Click += new System.EventHandler(this.labelStep_Click);
            // 
            // pictureBoxCurrent
            // 
            this.pictureBoxCurrent.Image = global::DiversityCollection.Resource.ArrowNext1;
            this.pictureBoxCurrent.Location = new System.Drawing.Point(0, 1);
            this.pictureBoxCurrent.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.pictureBoxCurrent.Name = "pictureBoxCurrent";
            this.pictureBoxCurrent.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxCurrent.TabIndex = 4;
            this.pictureBoxCurrent.TabStop = false;
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.BackColor = System.Drawing.Color.Pink;
            this.labelError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelError.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelError.Location = new System.Drawing.Point(94, 2);
            this.labelError.Margin = new System.Windows.Forms.Padding(0, 2, 3, 1);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(267, 15);
            this.labelError.TabIndex = 5;
            this.labelError.Text = "Error:";
            this.labelError.Visible = false;
            this.labelError.Click += new System.EventHandler(this.labelError_Click);
            // 
            // pictureBoxLevel
            // 
            this.pictureBoxLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxLevel.Location = new System.Drawing.Point(16, 0);
            this.pictureBoxLevel.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxLevel.Name = "pictureBoxLevel";
            this.pictureBoxLevel.Size = new System.Drawing.Size(16, 18);
            this.pictureBoxLevel.TabIndex = 6;
            this.pictureBoxLevel.TabStop = false;
            // 
            // imageListStatus
            // 
            this.imageListStatus.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListStatus.ImageStream")));
            this.imageListStatus.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListStatus.Images.SetKeyName(0, "OK.gif");
            this.imageListStatus.Images.SetKeyName(1, "Warning.ico");
            this.imageListStatus.Images.SetKeyName(2, "error.gif");
            // 
            // buttonMustImport
            // 
            this.buttonMustImport.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonMustImport.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonMustImport.Image = global::DiversityCollection.Resource.Import;
            this.buttonMustImport.Location = new System.Drawing.Point(77, 0);
            this.buttonMustImport.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMustImport.Name = "buttonMustImport";
            this.buttonMustImport.Size = new System.Drawing.Size(17, 17);
            this.buttonMustImport.TabIndex = 7;
            this.toolTip.SetToolTip(this.buttonMustImport, "If the data of this step should allways be imported, independet of data found in " +
        "the file");
            this.buttonMustImport.UseVisualStyleBackColor = true;
            this.buttonMustImport.Visible = false;
            this.buttonMustImport.Click += new System.EventHandler(this.buttonMustImport_Click);
            // 
            // UserControlImportStep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlImportStep";
            this.Size = new System.Drawing.Size(382, 18);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCurrent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLevel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.PictureBox pictureBoxStep;
        private System.Windows.Forms.PictureBox pictureBoxStatus;
        private System.Windows.Forms.Label labelStep;
        private System.Windows.Forms.PictureBox pictureBoxCurrent;
        private System.Windows.Forms.Label labelError;
        private System.Windows.Forms.ImageList imageListStatus;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox pictureBoxLevel;
        private System.Windows.Forms.Button buttonMustImport;
    }
}
