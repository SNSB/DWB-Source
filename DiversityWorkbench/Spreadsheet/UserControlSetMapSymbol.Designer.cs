namespace DiversityWorkbench.Spreadsheet
{
    partial class UserControlSetMapSymbol
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlSetMapSymbol));
            this.toolStripIcon = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelValue = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBoxSize = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabelSize = new System.Windows.Forms.ToolStripLabel();
            this.toolStripDropDownButtonSymbol = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripLabelSymbol = new System.Windows.Forms.ToolStripLabel();
            this.imageListSymbol = new System.Windows.Forms.ImageList(this.components);
            this.toolStripButtonUp = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripIcon
            // 
            this.toolStripIcon.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelValue,
            this.toolStripButtonUp,
            this.toolStripButtonDown,
            this.toolStripTextBoxSize,
            this.toolStripLabelSize,
            this.toolStripDropDownButtonSymbol,
            this.toolStripLabelSymbol});
            this.toolStripIcon.Location = new System.Drawing.Point(0, 0);
            this.toolStripIcon.Name = "toolStripIcon";
            this.toolStripIcon.Size = new System.Drawing.Size(503, 25);
            this.toolStripIcon.TabIndex = 1;
            this.toolStripIcon.Text = "toolStrip1";
            // 
            // toolStripLabelValue
            // 
            this.toolStripLabelValue.Name = "toolStripLabelValue";
            this.toolStripLabelValue.Size = new System.Drawing.Size(35, 22);
            this.toolStripLabelValue.Text = "Value";
            // 
            // toolStripTextBoxSize
            // 
            this.toolStripTextBoxSize.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripTextBoxSize.Name = "toolStripTextBoxSize";
            this.toolStripTextBoxSize.Size = new System.Drawing.Size(40, 25);
            this.toolStripTextBoxSize.Text = "1.0";
            this.toolStripTextBoxSize.TextChanged += new System.EventHandler(this.toolStripTextBoxSize_TextChanged);
            // 
            // toolStripLabelSize
            // 
            this.toolStripLabelSize.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabelSize.Name = "toolStripLabelSize";
            this.toolStripLabelSize.Size = new System.Drawing.Size(30, 22);
            this.toolStripLabelSize.Text = "Size:";
            // 
            // toolStripDropDownButtonSymbol
            // 
            this.toolStripDropDownButtonSymbol.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripDropDownButtonSymbol.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonSymbol.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonSymbol.Name = "toolStripDropDownButtonSymbol";
            this.toolStripDropDownButtonSymbol.Size = new System.Drawing.Size(13, 22);
            this.toolStripDropDownButtonSymbol.Text = "Please select the icon for the value";
            // 
            // toolStripLabelSymbol
            // 
            this.toolStripLabelSymbol.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabelSymbol.Name = "toolStripLabelSymbol";
            this.toolStripLabelSymbol.Size = new System.Drawing.Size(50, 22);
            this.toolStripLabelSymbol.Text = "Symbol:";
            // 
            // imageListSymbol
            // 
            this.imageListSymbol.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSymbol.ImageStream")));
            this.imageListSymbol.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSymbol.Images.SetKeyName(0, "Circle.ico");
            this.imageListSymbol.Images.SetKeyName(1, "CircleFilled.ico");
            this.imageListSymbol.Images.SetKeyName(2, "Cross.ico");
            this.imageListSymbol.Images.SetKeyName(3, "Diamond.ico");
            this.imageListSymbol.Images.SetKeyName(4, "DiamondFilled.ico");
            this.imageListSymbol.Images.SetKeyName(5, "Pin.ico");
            this.imageListSymbol.Images.SetKeyName(6, "Square.ico");
            this.imageListSymbol.Images.SetKeyName(7, "SquareFilled.ico");
            this.imageListSymbol.Images.SetKeyName(8, "X.ico");
            // 
            // toolStripButtonUp
            // 
            this.toolStripButtonUp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUp.Image = global::DiversityWorkbench.Properties.Resources.ArrowUpSmall;
            this.toolStripButtonUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUp.Name = "toolStripButtonUp";
            this.toolStripButtonUp.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonUp.Text = "Up";
            this.toolStripButtonUp.ToolTipText = "Upwards in legend";
            this.toolStripButtonUp.Click += new System.EventHandler(this.toolStripButtonUp_Click);
            // 
            // toolStripButtonDown
            // 
            this.toolStripButtonDown.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDown.Image = global::DiversityWorkbench.Properties.Resources.ArrowDownSmall;
            this.toolStripButtonDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDown.Name = "toolStripButtonDown";
            this.toolStripButtonDown.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDown.Text = "Down";
            this.toolStripButtonDown.ToolTipText = "Downwards in legend";
            this.toolStripButtonDown.Click += new System.EventHandler(this.toolStripButtonDown_Click);
            // 
            // UserControlSetMapSymbol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripIcon);
            this.Name = "UserControlSetMapSymbol";
            this.Size = new System.Drawing.Size(503, 26);
            this.toolStripIcon.ResumeLayout(false);
            this.toolStripIcon.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripIcon;
        private System.Windows.Forms.ToolStripLabel toolStripLabelValue;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxSize;
        private System.Windows.Forms.ToolStripLabel toolStripLabelSize;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonSymbol;
        private System.Windows.Forms.ToolStripLabel toolStripLabelSymbol;
        private System.Windows.Forms.ImageList imageListSymbol;
        private System.Windows.Forms.ToolStripButton toolStripButtonUp;
        private System.Windows.Forms.ToolStripButton toolStripButtonDown;
    }
}
