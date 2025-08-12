namespace DiversityWorkbench.Forms
{
    partial class FormHtmlColors
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHtmlColors));
            this.userControlHtmlColors = new DiversityWorkbench.UserControls.UserControlHtmlColors();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.SuspendLayout();
            // 
            // userControlHtmlColors
            // 
            this.userControlHtmlColors.ActiveCC = "#FF00FF";
            this.userControlHtmlColors.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.userControlHtmlColors.BackCC = "#FFFFFF";
            this.userControlHtmlColors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userControlHtmlColors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlHtmlColors.LinkCC = "#0000FF";
            this.userControlHtmlColors.Location = new System.Drawing.Point(0, 0);
            this.userControlHtmlColors.Name = "userControlHtmlColors";
            this.userControlHtmlColors.Size = new System.Drawing.Size(284, 77);
            this.userControlHtmlColors.TabIndex = 0;
            this.userControlHtmlColors.TextCC = "#000000";
            this.userControlHtmlColors.TitleCC = "#000080";
            this.userControlHtmlColors.VisitedCC = "#080008";
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 77);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(284, 27);
            this.userControlDialogPanel.TabIndex = 1;
            // 
            // FormHtmlColors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 104);
            this.Controls.Add(this.userControlHtmlColors);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormHtmlColors";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select HTML Colors";
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.UserControlHtmlColors userControlHtmlColors;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
    }
}