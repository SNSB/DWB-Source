namespace DiversityWorkbench.Forms
{
    partial class FormGetNumber
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
            this.maskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.labelHeader = new System.Windows.Forms.Label();
            this.userControlDialogPanel1 = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.SuspendLayout();
            // 
            // maskedTextBox
            // 
            this.maskedTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBox.Location = new System.Drawing.Point(0, 42);
            this.maskedTextBox.Mask = "000000.0000";
            this.maskedTextBox.Name = "maskedTextBox";
            this.maskedTextBox.Size = new System.Drawing.Size(196, 20);
            this.maskedTextBox.TabIndex = 8;
            this.maskedTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelHeader
            // 
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelHeader.Location = new System.Drawing.Point(0, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.labelHeader.Size = new System.Drawing.Size(196, 42);
            this.labelHeader.TabIndex = 7;
            this.labelHeader.Text = "please enter a number";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userControlDialogPanel1
            // 
            this.userControlDialogPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel1.Location = new System.Drawing.Point(0, 67);
            this.userControlDialogPanel1.Name = "userControlDialogPanel1";
            this.userControlDialogPanel1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel1.Size = new System.Drawing.Size(196, 27);
            this.userControlDialogPanel1.TabIndex = 6;
            // 
            // FormGetNumber
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(196, 94);
            this.Controls.Add(this.maskedTextBox);
            this.Controls.Add(this.labelHeader);
            this.Controls.Add(this.userControlDialogPanel1);
            this.Name = "FormGetNumber";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Value?";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox maskedTextBox;
        private System.Windows.Forms.Label labelHeader;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel1;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}