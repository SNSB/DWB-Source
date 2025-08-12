namespace DiversityWorkbench.UserControls
{
    partial class UserControlDiagram
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
            this.toolStripDiagram = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripComboBoxPalette = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparatorColor = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3D = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3D = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonLeft = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRight = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonUp = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRect = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonUpper = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorPosition = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonPerspectiveDec = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPerspectiveInc = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorPerspective = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonHeightInc = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonHegihtDec = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSaveData = new System.Windows.Forms.ToolStripButton();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.toolStripDiagram.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripDiagram
            // 
            this.toolStripDiagram.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripDiagram.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSave,
            this.toolStripComboBoxPalette,
            this.toolStripSeparatorColor,
            this.toolStripButton3D,
            this.toolStripSeparator3D,
            this.toolStripButtonLeft,
            this.toolStripButtonRight,
            this.toolStripButtonUp,
            this.toolStripButtonDown,
            this.toolStripButtonRect,
            this.toolStripButtonUpper,
            this.toolStripSeparatorPosition,
            this.toolStripButtonPerspectiveDec,
            this.toolStripButtonPerspectiveInc,
            this.toolStripSeparatorPerspective,
            this.toolStripButtonHeightInc,
            this.toolStripButtonHegihtDec,
            this.toolStripButtonSaveData});
            this.toolStripDiagram.Location = new System.Drawing.Point(0, 0);
            this.toolStripDiagram.Name = "toolStripDiagram";
            this.toolStripDiagram.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripDiagram.Size = new System.Drawing.Size(470, 25);
            this.toolStripDiagram.TabIndex = 11;
            this.toolStripDiagram.Text = "Diagram tools";
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Image = global::DiversityWorkbench.Properties.Resources.Save;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSave.Text = "Save diagram as image";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripComboBoxPalette
            // 
            this.toolStripComboBoxPalette.Name = "toolStripComboBoxPalette";
            this.toolStripComboBoxPalette.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBoxPalette.Text = "Colors";
            this.toolStripComboBoxPalette.ToolTipText = "Select the color palette of the diagram";
            this.toolStripComboBoxPalette.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxPalette_SelectedIndexChanged);
            // 
            // toolStripSeparatorColor
            // 
            this.toolStripSeparatorColor.Name = "toolStripSeparatorColor";
            this.toolStripSeparatorColor.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton3D
            // 
            this.toolStripButton3D.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.toolStripButton3D.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3D.Image = global::DiversityWorkbench.Properties.Resources.Style3D;
            this.toolStripButton3D.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3D.Name = "toolStripButton3D";
            this.toolStripButton3D.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3D.Text = "Switch between 2D and 3D view";
            this.toolStripButton3D.Visible = false;
            this.toolStripButton3D.Click += new System.EventHandler(this.toolStripButton3D_Click);
            // 
            // toolStripSeparator3D
            // 
            this.toolStripSeparator3D.Name = "toolStripSeparator3D";
            this.toolStripSeparator3D.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator3D.Visible = false;
            // 
            // toolStripButtonLeft
            // 
            this.toolStripButtonLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLeft.Image = global::DiversityWorkbench.Properties.Resources.ChartRotateLeft;
            this.toolStripButtonLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLeft.Name = "toolStripButtonLeft";
            this.toolStripButtonLeft.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonLeft.Text = "Rotate left";
            this.toolStripButtonLeft.Click += new System.EventHandler(this.toolStripButtonLeft_Click);
            // 
            // toolStripButtonRight
            // 
            this.toolStripButtonRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRight.Image = global::DiversityWorkbench.Properties.Resources.ChartRotateRight;
            this.toolStripButtonRight.Name = "toolStripButtonRight";
            this.toolStripButtonRight.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRight.Text = "Rotate right";
            this.toolStripButtonRight.Click += new System.EventHandler(this.toolStripButtonRight_Click);
            // 
            // toolStripButtonUp
            // 
            this.toolStripButtonUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUp.Image = global::DiversityWorkbench.Properties.Resources.TiltUp;
            this.toolStripButtonUp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUp.Name = "toolStripButtonUp";
            this.toolStripButtonUp.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonUp.Text = "Rotate up";
            this.toolStripButtonUp.Click += new System.EventHandler(this.toolStripButtonUp_Click);
            // 
            // toolStripButtonDown
            // 
            this.toolStripButtonDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDown.Image = global::DiversityWorkbench.Properties.Resources.TiltDown;
            this.toolStripButtonDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDown.Name = "toolStripButtonDown";
            this.toolStripButtonDown.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDown.Text = "Rotate down";
            this.toolStripButtonDown.Click += new System.EventHandler(this.toolStripButtonDown_Click);
            // 
            // toolStripButtonRect
            // 
            this.toolStripButtonRect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRect.Image = global::DiversityWorkbench.Properties.Resources.PerspRect;
            this.toolStripButtonRect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRect.Name = "toolStripButtonRect";
            this.toolStripButtonRect.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRect.Text = "Front view";
            this.toolStripButtonRect.Click += new System.EventHandler(this.toolStripButtonRect_Click);
            // 
            // toolStripButtonUpper
            // 
            this.toolStripButtonUpper.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUpper.Image = global::DiversityWorkbench.Properties.Resources.PerspUpper;
            this.toolStripButtonUpper.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUpper.Name = "toolStripButtonUpper";
            this.toolStripButtonUpper.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonUpper.Text = "Upper view";
            this.toolStripButtonUpper.Click += new System.EventHandler(this.toolStripButtonUpper_Click);
            // 
            // toolStripSeparatorPosition
            // 
            this.toolStripSeparatorPosition.Name = "toolStripSeparatorPosition";
            this.toolStripSeparatorPosition.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonPerspectiveDec
            // 
            this.toolStripButtonPerspectiveDec.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPerspectiveDec.Image = global::DiversityWorkbench.Properties.Resources.PerspectiveDec;
            this.toolStripButtonPerspectiveDec.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPerspectiveDec.Name = "toolStripButtonPerspectiveDec";
            this.toolStripButtonPerspectiveDec.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPerspectiveDec.Text = "Decrease perspective";
            this.toolStripButtonPerspectiveDec.Click += new System.EventHandler(this.toolStripButtonPerspectiveDec_Click);
            // 
            // toolStripButtonPerspectiveInc
            // 
            this.toolStripButtonPerspectiveInc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPerspectiveInc.Image = global::DiversityWorkbench.Properties.Resources.PerspectiveInc;
            this.toolStripButtonPerspectiveInc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPerspectiveInc.Name = "toolStripButtonPerspectiveInc";
            this.toolStripButtonPerspectiveInc.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonPerspectiveInc.Text = "Increase perspective";
            this.toolStripButtonPerspectiveInc.Click += new System.EventHandler(this.toolStripButtonPerspectiveInc_Click);
            // 
            // toolStripSeparatorPerspective
            // 
            this.toolStripSeparatorPerspective.Name = "toolStripSeparatorPerspective";
            this.toolStripSeparatorPerspective.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonHeightInc
            // 
            this.toolStripButtonHeightInc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHeightInc.Image = global::DiversityWorkbench.Properties.Resources.HeightInc;
            this.toolStripButtonHeightInc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHeightInc.Name = "toolStripButtonHeightInc";
            this.toolStripButtonHeightInc.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonHeightInc.Text = "Increase height";
            this.toolStripButtonHeightInc.Click += new System.EventHandler(this.toolStripButtonHeightInc_Click);
            // 
            // toolStripButtonHegihtDec
            // 
            this.toolStripButtonHegihtDec.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHegihtDec.Image = global::DiversityWorkbench.Properties.Resources.HeightDec;
            this.toolStripButtonHegihtDec.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHegihtDec.Name = "toolStripButtonHegihtDec";
            this.toolStripButtonHegihtDec.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonHegihtDec.Text = "Decrease height";
            this.toolStripButtonHegihtDec.Click += new System.EventHandler(this.toolStripButtonHeightDec_Click);
            // 
            // toolStripButtonSaveData
            // 
            this.toolStripButtonSaveData.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonSaveData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSaveData.Image = global::DiversityWorkbench.Properties.Resources.SaveRows;
            this.toolStripButtonSaveData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveData.Name = "toolStripButtonSaveData";
            this.toolStripButtonSaveData.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSaveData.Text = "Save data rows to file";
            this.toolStripButtonSaveData.Visible = false;
            this.toolStripButtonSaveData.Click += new System.EventHandler(this.toolStripButtonSaveData_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "png";
            this.saveFileDialog.Filter = "PNG image|*.png|GIF image|*.gif|JPEG image|*.jpg";
            // 
            // UserControlDiagram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripDiagram);
            this.Name = "UserControlDiagram";
            this.Size = new System.Drawing.Size(470, 460);
            this.toolStripDiagram.ResumeLayout(false);
            this.toolStripDiagram.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStrip toolStripDiagram;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxPalette;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorPosition;
        private System.Windows.Forms.ToolStripButton toolStripButtonLeft;
        private System.Windows.Forms.ToolStripButton toolStripButtonRight;
        private System.Windows.Forms.ToolStripButton toolStripButtonUp;
        private System.Windows.Forms.ToolStripButton toolStripButtonDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorPerspective;
        private System.Windows.Forms.ToolStripButton toolStripButtonPerspectiveDec;
        private System.Windows.Forms.ToolStripButton toolStripButtonPerspectiveInc;
        private System.Windows.Forms.ToolStripButton toolStripButtonHeightInc;
        private System.Windows.Forms.ToolStripButton toolStripButtonHegihtDec;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorColor;
        private System.Windows.Forms.ToolStripButton toolStripButtonRect;
        private System.Windows.Forms.ToolStripButton toolStripButtonUpper;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveData;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3D;
        private System.Windows.Forms.ToolStripButton toolStripButton3D;
    }
}
