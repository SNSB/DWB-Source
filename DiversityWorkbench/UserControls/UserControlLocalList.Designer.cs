namespace DiversityWorkbench.UserControls
{
    partial class UserControlLocalList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlLocalList));
            this.comboBoxLocalValues = new System.Windows.Forms.ComboBox();
            this.textBoxValue = new System.Windows.Forms.TextBox();
            this.buttonDeleteValue = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // comboBoxLocalValues
            // 
            resources.ApplyResources(this.comboBoxLocalValues, "comboBoxLocalValues");
            this.comboBoxLocalValues.DropDownWidth = 200;
            this.comboBoxLocalValues.FormattingEnabled = true;
            this.comboBoxLocalValues.Name = "comboBoxLocalValues";
            this.toolTip.SetToolTip(this.comboBoxLocalValues, resources.GetString("comboBoxLocalValues.ToolTip"));
            // 
            // textBoxValue
            // 
            this.textBoxValue.BackColor = System.Drawing.SystemColors.Info;
            this.textBoxValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.textBoxValue, "textBoxValue");
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.ReadOnly = true;
            // 
            // buttonDeleteValue
            // 
            resources.ApplyResources(this.buttonDeleteValue, "buttonDeleteValue");
            this.buttonDeleteValue.Image = global::DiversityWorkbench.ResourceWorkbench.Delete;
            this.buttonDeleteValue.Name = "buttonDeleteValue";
            this.toolTip.SetToolTip(this.buttonDeleteValue, resources.GetString("buttonDeleteValue.ToolTip"));
            this.buttonDeleteValue.UseVisualStyleBackColor = true;
            // 
            // UserControlLocalList
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxValue);
            this.Controls.Add(this.comboBoxLocalValues);
            this.Controls.Add(this.buttonDeleteValue);
            this.Name = "UserControlLocalList";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox comboBoxLocalValues;
        public System.Windows.Forms.TextBox textBoxValue;
        public System.Windows.Forms.Button buttonDeleteValue;
        private System.Windows.Forms.ToolTip toolTip;

    }
}
