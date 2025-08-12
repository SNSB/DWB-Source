
namespace DiversityWorkbench.Forms
{
    partial class FormRichEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRichEdit));
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.userControlRichEdit = new DiversityWorkbench.UserControls.UserControlRichEdit();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.SuspendLayout();
            // 
            // userControlRichEdit
            // 
            this.userControlRichEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichEdit.EditText = "";
            this.userControlRichEdit.Location = new System.Drawing.Point(0, 0);
            this.userControlRichEdit.Name = "userControlRichEdit";
            this.userControlRichEdit.ReadOnly = false;
            this.userControlRichEdit.ShowMenu = false;
            this.userControlRichEdit.ShowStatus = true;
            this.userControlRichEdit.Size = new System.Drawing.Size(634, 364);
            this.userControlRichEdit.TabIndex = 1;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 364);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(634, 27);
            this.userControlDialogPanel.TabIndex = 0;
            // 
            // FormRichEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 391);
            this.Controls.Add(this.userControlRichEdit);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormRichEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit rich text";
            this.Shown += new System.EventHandler(this.FormRichEdit_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private DiversityWorkbench.UserControls.UserControlRichEdit userControlRichEdit;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}