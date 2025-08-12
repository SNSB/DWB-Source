namespace DiversityWorkbench.Spreadsheet
{
    partial class UserControlSetMapColor
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
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripComboBoxOperator = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripTextBoxFrom = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabelBetween = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBoxTo = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButtonColor = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripButtonSortingValue = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabelSortingValue = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBoxOperator,
            this.toolStripTextBoxFrom,
            this.toolStripLabelBetween,
            this.toolStripTextBoxTo,
            this.toolStripButtonDelete,
            this.toolStripDropDownButtonColor,
            this.toolStripLabelSortingValue,
            this.toolStripButtonSortingValue});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(474, 27);
            this.toolStrip.TabIndex = 7;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripComboBoxOperator
            // 
            this.toolStripComboBoxOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxOperator.DropDownWidth = 75;
            this.toolStripComboBoxOperator.MaxDropDownItems = 20;
            this.toolStripComboBoxOperator.Name = "toolStripComboBoxOperator";
            this.toolStripComboBoxOperator.Size = new System.Drawing.Size(75, 27);
            this.toolStripComboBoxOperator.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxOperator_SelectedIndexChanged);
            // 
            // toolStripTextBoxFrom
            // 
            this.toolStripTextBoxFrom.BackColor = System.Drawing.Color.Pink;
            this.toolStripTextBoxFrom.Name = "toolStripTextBoxFrom";
            this.toolStripTextBoxFrom.Size = new System.Drawing.Size(80, 27);
            this.toolStripTextBoxFrom.ToolTipText = "Value for the restriction";
            this.toolStripTextBoxFrom.TextChanged += new System.EventHandler(this.toolStripTextBoxFrom_TextChanged);
            // 
            // toolStripLabelBetween
            // 
            this.toolStripLabelBetween.Name = "toolStripLabelBetween";
            this.toolStripLabelBetween.Size = new System.Drawing.Size(12, 24);
            this.toolStripLabelBetween.Text = "-";
            // 
            // toolStripTextBoxTo
            // 
            this.toolStripTextBoxTo.BackColor = System.Drawing.Color.Pink;
            this.toolStripTextBoxTo.Name = "toolStripTextBoxTo";
            this.toolStripTextBoxTo.Size = new System.Drawing.Size(80, 27);
            this.toolStripTextBoxTo.ToolTipText = "Upper value of the range";
            this.toolStripTextBoxTo.TextChanged += new System.EventHandler(this.toolStripTextBoxTo_TextChanged);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(23, 24);
            this.toolStripButtonDelete.Text = "Remove this color and restriction";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripDropDownButtonColor
            // 
            this.toolStripDropDownButtonColor.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripDropDownButtonColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButtonColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonColor.Name = "toolStripDropDownButtonColor";
            this.toolStripDropDownButtonColor.Size = new System.Drawing.Size(49, 24);
            this.toolStripDropDownButtonColor.Text = "Color";
            // 
            // toolStripButtonSortingValue
            // 
            this.toolStripButtonSortingValue.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonSortingValue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSortingValue.Image = global::DiversityWorkbench.Properties.Resources.Edit;
            this.toolStripButtonSortingValue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSortingValue.Name = "toolStripButtonSortingValue";
            this.toolStripButtonSortingValue.Size = new System.Drawing.Size(23, 24);
            this.toolStripButtonSortingValue.Text = "Set sorting value";
            this.toolStripButtonSortingValue.ToolTipText = "Set sorting value";
            this.toolStripButtonSortingValue.Visible = false;
            this.toolStripButtonSortingValue.Click += new System.EventHandler(this.toolStripButtonSortingValue_Click);
            // 
            // toolStripLabelSortingValue
            // 
            this.toolStripLabelSortingValue.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabelSortingValue.Name = "toolStripLabelSortingValue";
            this.toolStripLabelSortingValue.Size = new System.Drawing.Size(13, 24);
            this.toolStripLabelSortingValue.Text = "1";
            // 
            // UserControlSetMapColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip);
            this.Name = "UserControlSetMapColor";
            this.Size = new System.Drawing.Size(474, 27);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxOperator;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonColor;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxFrom;
        private System.Windows.Forms.ToolStripLabel toolStripLabelBetween;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxTo;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripLabel toolStripLabelSortingValue;
        private System.Windows.Forms.ToolStripButton toolStripButtonSortingValue;
    }
}
