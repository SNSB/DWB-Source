namespace DiversityWorkbench.Forms
{
    partial class FormGeography
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGeography));
            this.elementHostGeography = new System.Windows.Forms.Integration.ElementHost();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.SuspendLayout();
            // 
            // elementHostGeography
            // 
            this.elementHostGeography.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHostGeography.Location = new System.Drawing.Point(0, 0);
            this.elementHostGeography.Name = "elementHostGeography";
            this.elementHostGeography.Size = new System.Drawing.Size(846, 573);
            this.elementHostGeography.TabIndex = 1;
            this.elementHostGeography.Text = "elementHost1";
            this.elementHostGeography.Child = null;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 573);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(846, 27);
            this.userControlDialogPanel.TabIndex = 0;
            // 
            // FormGeography
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 600);
            this.Controls.Add(this.elementHostGeography);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormGeography";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Get geography";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormGeography_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.Integration.ElementHost elementHostGeography;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}