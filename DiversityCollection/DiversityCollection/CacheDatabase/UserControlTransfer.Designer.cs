namespace DiversityCollection.CacheDatabase
{
    partial class UserControlTransfer
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlTransfer));
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            labelStep = new System.Windows.Forms.Label();
            buttonViewResult = new System.Windows.Forms.Button();
            buttonInfo = new System.Windows.Forms.Button();
            pictureBoxStep = new System.Windows.Forms.PictureBox();
            labelStart = new System.Windows.Forms.Label();
            labelStartTime = new System.Windows.Forms.Label();
            labelEnd = new System.Windows.Forms.Label();
            labelEndTime = new System.Windows.Forms.Label();
            labelCount = new System.Windows.Forms.Label();
            checkBoxTransfer = new System.Windows.Forms.CheckBox();
            labelCountSource = new System.Windows.Forms.Label();
            progressBar = new System.Windows.Forms.ProgressBar();
            labelInfo = new System.Windows.Forms.Label();
            buttonShowProcedure = new System.Windows.Forms.Button();
            imageListInfo = new System.Windows.Forms.ImageList(components);
            toolTip = new System.Windows.Forms.ToolTip(components);
            tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxStep).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 13;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.Controls.Add(labelStep, 2, 0);
            tableLayoutPanel.Controls.Add(buttonViewResult, 11, 0);
            tableLayoutPanel.Controls.Add(buttonInfo, 10, 0);
            tableLayoutPanel.Controls.Add(pictureBoxStep, 1, 0);
            tableLayoutPanel.Controls.Add(labelStart, 4, 0);
            tableLayoutPanel.Controls.Add(labelStartTime, 5, 0);
            tableLayoutPanel.Controls.Add(labelEnd, 6, 0);
            tableLayoutPanel.Controls.Add(labelEndTime, 7, 0);
            tableLayoutPanel.Controls.Add(labelCount, 9, 0);
            tableLayoutPanel.Controls.Add(checkBoxTransfer, 0, 0);
            tableLayoutPanel.Controls.Add(labelCountSource, 8, 0);
            tableLayoutPanel.Controls.Add(progressBar, 2, 1);
            tableLayoutPanel.Controls.Add(labelInfo, 3, 0);
            tableLayoutPanel.Controls.Add(buttonShowProcedure, 12, 0);
            tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 2;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            tableLayoutPanel.Size = new System.Drawing.Size(718, 35);
            tableLayoutPanel.TabIndex = 1;
            // 
            // labelStep
            // 
            labelStep.AutoSize = true;
            labelStep.Dock = System.Windows.Forms.DockStyle.Fill;
            labelStep.Location = new System.Drawing.Point(46, 0);
            labelStep.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            labelStep.Name = "labelStep";
            labelStep.Size = new System.Drawing.Size(150, 26);
            labelStep.TabIndex = 0;
            labelStep.Text = "label1";
            labelStep.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // buttonViewResult
            // 
            buttonViewResult.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonViewResult.Image = Resource.Lupe;
            buttonViewResult.Location = new System.Drawing.Point(655, 3);
            buttonViewResult.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            buttonViewResult.Name = "buttonViewResult";
            tableLayoutPanel.SetRowSpan(buttonViewResult, 2);
            buttonViewResult.Size = new System.Drawing.Size(37, 29);
            buttonViewResult.TabIndex = 2;
            toolTip.SetToolTip(buttonViewResult, "View first 100 data lines");
            buttonViewResult.UseVisualStyleBackColor = true;
            buttonViewResult.Click += buttonViewResult_Click;
            // 
            // buttonInfo
            // 
            buttonInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonInfo.Image = Resource.wait_animation;
            buttonInfo.Location = new System.Drawing.Point(627, 3);
            buttonInfo.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            buttonInfo.Name = "buttonInfo";
            tableLayoutPanel.SetRowSpan(buttonInfo, 2);
            buttonInfo.Size = new System.Drawing.Size(28, 29);
            buttonInfo.TabIndex = 5;
            buttonInfo.UseVisualStyleBackColor = true;
            buttonInfo.Visible = false;
            buttonInfo.Click += buttonInfo_Click;
            // 
            // pictureBoxStep
            // 
            pictureBoxStep.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxStep.Location = new System.Drawing.Point(25, 8);
            pictureBoxStep.Margin = new System.Windows.Forms.Padding(2, 8, 2, 8);
            pictureBoxStep.Name = "pictureBoxStep";
            tableLayoutPanel.SetRowSpan(pictureBoxStep, 2);
            pictureBoxStep.Size = new System.Drawing.Size(19, 19);
            pictureBoxStep.TabIndex = 6;
            pictureBoxStep.TabStop = false;
            // 
            // labelStart
            // 
            labelStart.AutoSize = true;
            labelStart.Dock = System.Windows.Forms.DockStyle.Fill;
            labelStart.ForeColor = System.Drawing.SystemColors.ControlDark;
            labelStart.Location = new System.Drawing.Point(344, 0);
            labelStart.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelStart.Name = "labelStart";
            labelStart.Size = new System.Drawing.Size(34, 26);
            labelStart.TabIndex = 7;
            labelStart.Text = "Start:";
            labelStart.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            labelStart.Visible = false;
            // 
            // labelStartTime
            // 
            labelStartTime.AutoSize = true;
            labelStartTime.Dock = System.Windows.Forms.DockStyle.Fill;
            labelStartTime.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            labelStartTime.Location = new System.Drawing.Point(378, 0);
            labelStartTime.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            labelStartTime.Name = "labelStartTime";
            labelStartTime.Size = new System.Drawing.Size(29, 26);
            labelStartTime.TabIndex = 8;
            labelStartTime.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelEnd
            // 
            labelEnd.AutoSize = true;
            labelEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            labelEnd.ForeColor = System.Drawing.SystemColors.ControlDark;
            labelEnd.Location = new System.Drawing.Point(415, 0);
            labelEnd.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelEnd.Name = "labelEnd";
            labelEnd.Size = new System.Drawing.Size(33, 26);
            labelEnd.TabIndex = 9;
            labelEnd.Text = "End:";
            labelEnd.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            labelEnd.Visible = false;
            // 
            // labelEndTime
            // 
            labelEndTime.AutoSize = true;
            labelEndTime.Dock = System.Windows.Forms.DockStyle.Fill;
            labelEndTime.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            labelEndTime.Location = new System.Drawing.Point(448, 0);
            labelEndTime.Margin = new System.Windows.Forms.Padding(0);
            labelEndTime.Name = "labelEndTime";
            labelEndTime.Size = new System.Drawing.Size(33, 26);
            labelEndTime.TabIndex = 10;
            labelEndTime.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelCount
            // 
            labelCount.AutoSize = true;
            labelCount.Dock = System.Windows.Forms.DockStyle.Fill;
            labelCount.Location = new System.Drawing.Point(563, 0);
            labelCount.Margin = new System.Windows.Forms.Padding(0);
            labelCount.MinimumSize = new System.Drawing.Size(47, 0);
            labelCount.Name = "labelCount";
            labelCount.Size = new System.Drawing.Size(64, 26);
            labelCount.TabIndex = 11;
            labelCount.Text = "12345678";
            labelCount.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // checkBoxTransfer
            // 
            checkBoxTransfer.AutoSize = true;
            checkBoxTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxTransfer.Location = new System.Drawing.Point(4, 3);
            checkBoxTransfer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxTransfer.Name = "checkBoxTransfer";
            tableLayoutPanel.SetRowSpan(checkBoxTransfer, 2);
            checkBoxTransfer.Size = new System.Drawing.Size(15, 29);
            checkBoxTransfer.TabIndex = 12;
            toolTip.SetToolTip(checkBoxTransfer, "Transfer these data");
            checkBoxTransfer.UseVisualStyleBackColor = true;
            checkBoxTransfer.Click += checkBoxTransfer_Click;
            // 
            // labelCountSource
            // 
            labelCountSource.AutoSize = true;
            labelCountSource.Dock = System.Windows.Forms.DockStyle.Fill;
            labelCountSource.ForeColor = System.Drawing.Color.Red;
            labelCountSource.Location = new System.Drawing.Point(481, 0);
            labelCountSource.Margin = new System.Windows.Forms.Padding(0);
            labelCountSource.Name = "labelCountSource";
            labelCountSource.Size = new System.Drawing.Size(82, 26);
            labelCountSource.TabIndex = 13;
            labelCountSource.Text = "12345678 <>";
            labelCountSource.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // progressBar
            // 
            tableLayoutPanel.SetColumnSpan(progressBar, 8);
            progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            progressBar.Location = new System.Drawing.Point(46, 26);
            progressBar.Margin = new System.Windows.Forms.Padding(0);
            progressBar.Name = "progressBar";
            progressBar.Size = new System.Drawing.Size(581, 9);
            progressBar.TabIndex = 14;
            progressBar.Visible = false;
            // 
            // labelInfo
            // 
            labelInfo.AutoSize = true;
            labelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            labelInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            labelInfo.ForeColor = System.Drawing.SystemColors.GrayText;
            labelInfo.Location = new System.Drawing.Point(204, 0);
            labelInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelInfo.MaximumSize = new System.Drawing.Size(117, 0);
            labelInfo.Name = "labelInfo";
            labelInfo.Size = new System.Drawing.Size(117, 26);
            labelInfo.TabIndex = 15;
            labelInfo.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // buttonShowProcedure
            // 
            buttonShowProcedure.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonShowProcedure.FlatAppearance.BorderSize = 0;
            buttonShowProcedure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonShowProcedure.Image = Resource.Manual;
            buttonShowProcedure.Location = new System.Drawing.Point(692, 3);
            buttonShowProcedure.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            buttonShowProcedure.Name = "buttonShowProcedure";
            tableLayoutPanel.SetRowSpan(buttonShowProcedure, 2);
            buttonShowProcedure.Size = new System.Drawing.Size(26, 29);
            buttonShowProcedure.TabIndex = 16;
            buttonShowProcedure.UseVisualStyleBackColor = true;
            buttonShowProcedure.Visible = false;
            buttonShowProcedure.Click += buttonShowProcedure_Click;
            // 
            // imageListInfo
            // 
            imageListInfo.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListInfo.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListInfo.ImageStream");
            imageListInfo.TransparentColor = System.Drawing.Color.Transparent;
            imageListInfo.Images.SetKeyName(0, "OK.ico");
            imageListInfo.Images.SetKeyName(1, "info.ico");
            imageListInfo.Images.SetKeyName(2, "Error.ico");
            // 
            // UserControlTransfer
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "UserControlTransfer";
            Size = new System.Drawing.Size(718, 35);
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxStep).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelStep;
        private System.Windows.Forms.Button buttonViewResult;
        private System.Windows.Forms.Button buttonInfo;
        private System.Windows.Forms.PictureBox pictureBoxStep;
        private System.Windows.Forms.ImageList imageListInfo;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelStart;
        private System.Windows.Forms.Label labelStartTime;
        private System.Windows.Forms.Label labelEnd;
        private System.Windows.Forms.Label labelEndTime;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.CheckBox checkBoxTransfer;
        private System.Windows.Forms.Label labelCountSource;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Button buttonShowProcedure;
    }
}
