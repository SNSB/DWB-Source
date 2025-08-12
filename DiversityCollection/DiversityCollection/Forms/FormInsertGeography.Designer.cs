namespace DiversityCollection.Forms
{
    partial class FormInsertGeography
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInsertGeography));
            this.userControlDialogPanel1 = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.comboBoxLocalisationSystem = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // userControlDialogPanel1
            // 
            this.userControlDialogPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel1.Location = new System.Drawing.Point(4, 43);
            this.userControlDialogPanel1.Name = "userControlDialogPanel1";
            this.userControlDialogPanel1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel1.Size = new System.Drawing.Size(284, 27);
            this.userControlDialogPanel1.TabIndex = 0;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTitle.Location = new System.Drawing.Point(4, 4);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(142, 13);
            this.labelTitle.TabIndex = 1;
            this.labelTitle.Text = "Choose a localisation system";
            // 
            // comboBoxLocalisationSystem
            // 
            this.comboBoxLocalisationSystem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxLocalisationSystem.FormattingEnabled = true;
            this.comboBoxLocalisationSystem.Location = new System.Drawing.Point(4, 17);
            this.comboBoxLocalisationSystem.Name = "comboBoxLocalisationSystem";
            this.comboBoxLocalisationSystem.Size = new System.Drawing.Size(284, 21);
            this.comboBoxLocalisationSystem.TabIndex = 2;
            // 
            // FormInsertGeography
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 74);
            this.Controls.Add(this.comboBoxLocalisationSystem);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.userControlDialogPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormInsertGeography";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Insert a new localisation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ComboBox comboBoxLocalisationSystem;
    }
}