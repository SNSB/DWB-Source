namespace DiversityWorkbench.Import
{
    partial class FormFileDataGrid
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFileDataGrid));
            this.panelGrid = new System.Windows.Forms.Panel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.userControlDialogPanel1 = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.SuspendLayout();
            // 
            // panelGrid
            // 
            this.panelGrid.AutoScroll = true;
            this.panelGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGrid.Location = new System.Drawing.Point(0, 23);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Size = new System.Drawing.Size(1184, 212);
            this.panelGrid.TabIndex = 6;
            this.panelGrid.DoubleClick += new System.EventHandler(this.panelGrid_DoubleClick);
            // 
            // labelHeader
            // 
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelHeader.Location = new System.Drawing.Point(0, 0);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(3);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(1184, 23);
            this.labelHeader.TabIndex = 5;
            this.labelHeader.Text = "Select column";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // userControlDialogPanel1
            // 
            this.userControlDialogPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel1.Location = new System.Drawing.Point(0, 235);
            this.userControlDialogPanel1.Name = "userControlDialogPanel1";
            this.userControlDialogPanel1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel1.Size = new System.Drawing.Size(1184, 27);
            this.userControlDialogPanel1.TabIndex = 4;
            // 
            // FormFileDataGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 262);
            this.Controls.Add(this.panelGrid);
            this.Controls.Add(this.labelHeader);
            this.Controls.Add(this.userControlDialogPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormFileDataGrid";
            this.Text = "Select a column";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelGrid;
        private System.Windows.Forms.Label labelHeader;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel1;
    }
}