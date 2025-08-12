namespace DiversityCollection.CacheDatabase
{
    partial class UserControlOverviewProject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlOverviewProject));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelProject = new System.Windows.Forms.Label();
            this.labelCount = new System.Windows.Forms.Label();
            this.labelDate = new System.Windows.Forms.Label();
            this.buttonTransfer = new System.Windows.Forms.Button();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.labelProject, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelCount, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelDate, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonTransfer, 2, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(250, 40);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelProject
            // 
            this.labelProject.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelProject, 2);
            this.labelProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProject.Location = new System.Drawing.Point(3, 0);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(192, 13);
            this.labelProject.TabIndex = 0;
            this.labelProject.Text = "label1";
            this.labelProject.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelCount
            // 
            this.labelCount.AutoSize = true;
            this.labelCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCount.Location = new System.Drawing.Point(3, 13);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(151, 27);
            this.labelCount.TabIndex = 1;
            this.labelCount.Text = "label1";
            // 
            // labelDate
            // 
            this.labelDate.AutoSize = true;
            this.labelDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDate.Location = new System.Drawing.Point(160, 13);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(35, 27);
            this.labelDate.TabIndex = 2;
            this.labelDate.Text = "label1";
            this.labelDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonTransfer
            // 
            this.buttonTransfer.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonTransfer.Image = global::DiversityCollection.Resource.ArrowNextNext;
            this.buttonTransfer.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonTransfer.Location = new System.Drawing.Point(198, 0);
            this.buttonTransfer.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransfer.Name = "buttonTransfer";
            this.tableLayoutPanel.SetRowSpan(this.buttonTransfer, 2);
            this.buttonTransfer.Size = new System.Drawing.Size(52, 35);
            this.buttonTransfer.TabIndex = 3;
            this.buttonTransfer.Text = "Refresh";
            this.buttonTransfer.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip.SetToolTip(this.buttonTransfer, "Transfer data of the project");
            this.buttonTransfer.UseVisualStyleBackColor = true;
            this.buttonTransfer.Click += new System.EventHandler(this.buttonTransfer_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "ArrowNext.ico");
            this.imageList.Images.SetKeyName(1, "ArrowNextNext.ico");
            // 
            // UserControlOverviewProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlOverviewProject";
            this.Size = new System.Drawing.Size(250, 40);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelProject;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.Label labelDate;
        private System.Windows.Forms.Button buttonTransfer;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
