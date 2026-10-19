namespace DiversityAgents.Forms
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
            userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            comboBoxProjects = new System.Windows.Forms.ComboBox();
            helpProvider = new System.Windows.Forms.HelpProvider();
            SuspendLayout();
            // 
            // userControlDialogPanel
            // 
            userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            userControlDialogPanel.Location = new System.Drawing.Point(5, 30);
            userControlDialogPanel.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlDialogPanel.Name = "userControlDialogPanel";
            userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            userControlDialogPanel.Size = new System.Drawing.Size(284, 31);
            userControlDialogPanel.TabIndex = 0;
            // 
            // comboBoxProjects
            // 
            comboBoxProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxProjects.FormattingEnabled = true;
            comboBoxProjects.Location = new System.Drawing.Point(5, 5);
            comboBoxProjects.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxProjects.MaxDropDownItems = 20;
            comboBoxProjects.Name = "comboBoxProjects";
            comboBoxProjects.Size = new System.Drawing.Size(284, 23);
            comboBoxProjects.TabIndex = 1;
            // 
            // FormGetProject
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(294, 66);
            Controls.Add(comboBoxProjects);
            Controls.Add(userControlDialogPanel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            helpProvider.SetHelpKeyword(this, "projects_da");
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormGetProject";
            Padding = new System.Windows.Forms.Padding(5);
            helpProvider.SetShowHelp(this, true);
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = " Select a project";
            KeyDown += Form_KeyDown;
            ResumeLayout(false);
        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.ComboBox comboBoxProjects;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}