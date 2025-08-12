namespace DiversityWorkbench.Spreadsheet
{
    partial class FormFixedSources
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFixedSources));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.treeViewSettings = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.treeViewSettings, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(584, 561);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // treeViewSettings
            // 
            this.treeViewSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewSettings.ImageIndex = 0;
            this.treeViewSettings.ImageList = this.imageList;
            this.treeViewSettings.Location = new System.Drawing.Point(3, 3);
            this.treeViewSettings.Name = "treeViewSettings";
            this.treeViewSettings.SelectedImageIndex = 0;
            this.treeViewSettings.Size = new System.Drawing.Size(578, 555);
            this.treeViewSettings.TabIndex = 0;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Settings.ico");
            this.imageList.Images.SetKeyName(1, "DiversityWorkbench.ico");
            this.imageList.Images.SetKeyName(2, "Speadsheet.ico");
            this.imageList.Images.SetKeyName(3, "SelectRow.ico");
            this.imageList.Images.SetKeyName(4, "Pin_3.ico");
            this.imageList.Images.SetKeyName(5, "Database.ico");
            this.imageList.Images.SetKeyName(6, "Project.ico");
            this.imageList.Images.SetKeyName(7, "Webservice.ico");
            // 
            // FormFixedSources
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 561);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormFixedSources";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sources set for columns linked to remote modules";
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TreeView treeViewSettings;
        private System.Windows.Forms.ImageList imageList;
    }
}