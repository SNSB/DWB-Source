namespace DiversityWorkbench.UserControls
{
    partial class UserControlURI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlURI));
            this.textBoxURI = new System.Windows.Forms.TextBox();
            this.buttonOpenURI = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxURI
            // 
            resources.ApplyResources(this.textBoxURI, "textBoxURI");
            this.textBoxURI.Name = "textBoxURI";
            this.toolTip.SetToolTip(this.textBoxURI, resources.GetString("textBoxURI.ToolTip"));
            this.textBoxURI.DoubleClick += new System.EventHandler(this.buttonOpenURI_Click);
            // 
            // buttonOpenURI
            // 
            resources.ApplyResources(this.buttonOpenURI, "buttonOpenURI");
            this.buttonOpenURI.FlatAppearance.BorderSize = 0;
            this.buttonOpenURI.Image = global::DiversityWorkbench.ResourceWorkbench.Browse;
            this.buttonOpenURI.Name = "buttonOpenURI";
            this.toolTip.SetToolTip(this.buttonOpenURI, resources.GetString("buttonOpenURI.ToolTip"));
            this.buttonOpenURI.UseVisualStyleBackColor = true;
            this.buttonOpenURI.Click += new System.EventHandler(this.buttonOpenURI_Click);
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
            this.tableLayoutPanel.Controls.Add(this.buttonOpenURI, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxURI, 0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.toolTip.SetToolTip(this.tableLayoutPanel, resources.GetString("tableLayoutPanel.ToolTip"));
            // 
            // UserControlURI
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlURI";
            this.toolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOpenURI;
        public System.Windows.Forms.TextBox textBoxURI;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    }
}
