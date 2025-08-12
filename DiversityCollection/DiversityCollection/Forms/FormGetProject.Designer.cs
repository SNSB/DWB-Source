namespace DiversityCollection.Forms
{
    partial class FormGetProject
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGetProject));
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.comboBoxProjects = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // userControlDialogPanel
            // 
            resources.ApplyResources(this.userControlDialogPanel, "userControlDialogPanel");
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            // 
            // comboBoxProjects
            // 
            resources.ApplyResources(this.comboBoxProjects, "comboBoxProjects");
            this.comboBoxProjects.FormattingEnabled = true;
            this.comboBoxProjects.Name = "comboBoxProjects";
            // 
            // FormGetProject
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxProjects);
            this.Controls.Add(this.userControlDialogPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormGetProject";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.ComboBox comboBoxProjects;
    }
}