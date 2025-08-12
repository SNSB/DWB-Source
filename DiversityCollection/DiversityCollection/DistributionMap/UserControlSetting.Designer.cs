namespace DiversityCollection.DistributionMap
{
    partial class UserControlSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlSetting));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSetRestriction = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSetColor = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabelSize = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBoxSize = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabelTransparency = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBoxTransparency = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabelSymbol = new System.Windows.Forms.ToolStripLabel();
            this.toolStripDropDownButtonSymbol = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripDropDownButtonColor = new System.Windows.Forms.ToolStripDropDownButton();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.imageListSymbols = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.toolStripMain, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.textBoxTitle, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonRemove, 1, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(299, 50);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // toolStripMain
            // 
            this.tableLayoutPanel.SetColumnSpan(this.toolStripMain, 2);
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSetRestriction,
            this.toolStripButtonSetColor,
            this.toolStripLabelSize,
            this.toolStripTextBoxSize,
            this.toolStripLabelTransparency,
            this.toolStripTextBoxTransparency,
            this.toolStripLabelSymbol,
            this.toolStripDropDownButtonSymbol,
            this.toolStripDropDownButtonColor});
            this.toolStripMain.Location = new System.Drawing.Point(0, 24);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(299, 25);
            this.toolStripMain.TabIndex = 4;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // toolStripButtonSetRestriction
            // 
            this.toolStripButtonSetRestriction.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSetRestriction.Image = global::DiversityCollection.Resource.Lupe;
            this.toolStripButtonSetRestriction.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSetRestriction.Name = "toolStripButtonSetRestriction";
            this.toolStripButtonSetRestriction.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSetRestriction.Text = "Set restriction";
            this.toolStripButtonSetRestriction.Click += new System.EventHandler(this.toolStripButtonSetRestriction_Click);
            // 
            // toolStripButtonSetColor
            // 
            this.toolStripButtonSetColor.BackColor = System.Drawing.Color.Black;
            this.toolStripButtonSetColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSetColor.Image = global::DiversityCollection.Resource.Color;
            this.toolStripButtonSetColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSetColor.Name = "toolStripButtonSetColor";
            this.toolStripButtonSetColor.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSetColor.Text = "Set color";
            this.toolStripButtonSetColor.Visible = false;
            // 
            // toolStripLabelSize
            // 
            this.toolStripLabelSize.Name = "toolStripLabelSize";
            this.toolStripLabelSize.Size = new System.Drawing.Size(30, 22);
            this.toolStripLabelSize.Text = "Size:";
            // 
            // toolStripTextBoxSize
            // 
            this.toolStripTextBoxSize.Name = "toolStripTextBoxSize";
            this.toolStripTextBoxSize.Size = new System.Drawing.Size(20, 25);
            this.toolStripTextBoxSize.Text = "10";
            this.toolStripTextBoxSize.Leave += new System.EventHandler(this.toolStripTextBoxSize_Leave);
            // 
            // toolStripLabelTransparency
            // 
            this.toolStripLabelTransparency.Name = "toolStripLabelTransparency";
            this.toolStripLabelTransparency.Size = new System.Drawing.Size(48, 22);
            this.toolStripLabelTransparency.Text = "Transp.:";
            // 
            // toolStripTextBoxTransparency
            // 
            this.toolStripTextBoxTransparency.Name = "toolStripTextBoxTransparency";
            this.toolStripTextBoxTransparency.Size = new System.Drawing.Size(20, 25);
            this.toolStripTextBoxTransparency.Text = "0";
            this.toolStripTextBoxTransparency.Leave += new System.EventHandler(this.toolStripTextBoxTransparency_Leave);
            // 
            // toolStripLabelSymbol
            // 
            this.toolStripLabelSymbol.Name = "toolStripLabelSymbol";
            this.toolStripLabelSymbol.Size = new System.Drawing.Size(50, 22);
            this.toolStripLabelSymbol.Text = "Symbol:";
            // 
            // toolStripDropDownButtonSymbol
            // 
            this.toolStripDropDownButtonSymbol.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonSymbol.Image = global::DiversityCollection.Resource.KreisVoll;
            this.toolStripDropDownButtonSymbol.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonSymbol.Name = "toolStripDropDownButtonSymbol";
            this.toolStripDropDownButtonSymbol.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButtonSymbol.Text = "toolStripDropDownButton1";
            // 
            // toolStripDropDownButtonColor
            // 
            this.toolStripDropDownButtonColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonColor.Image = global::DiversityCollection.Resource.Color;
            this.toolStripDropDownButtonColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonColor.Name = "toolStripDropDownButtonColor";
            this.toolStripDropDownButtonColor.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButtonColor.Text = "toolStripDropDownButton1";
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxTitle.Location = new System.Drawing.Point(0, 4);
            this.textBoxTitle.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(279, 20);
            this.textBoxTitle.TabIndex = 5;
            this.textBoxTitle.Leave += new System.EventHandler(this.textBoxTitle_Leave);
            // 
            // buttonRemove
            // 
            this.buttonRemove.FlatAppearance.BorderSize = 0;
            this.buttonRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRemove.Image = global::DiversityCollection.Resource.Delete;
            this.buttonRemove.Location = new System.Drawing.Point(279, 4);
            this.buttonRemove.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(20, 20);
            this.buttonRemove.TabIndex = 6;
            this.toolTip.SetToolTip(this.buttonRemove, "Remove this setting");
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // imageListSymbols
            // 
            this.imageListSymbols.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSymbols.ImageStream")));
            this.imageListSymbols.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSymbols.Images.SetKeyName(0, "Diamond.ico");
            this.imageListSymbols.Images.SetKeyName(1, "DiamondVoll.ico");
            this.imageListSymbols.Images.SetKeyName(2, "Dreieck.ico");
            this.imageListSymbols.Images.SetKeyName(3, "Dreieck2.ico");
            this.imageListSymbols.Images.SetKeyName(4, "Dreieck2Voll.ico");
            this.imageListSymbols.Images.SetKeyName(5, "DreieckVoll.ico");
            this.imageListSymbols.Images.SetKeyName(6, "Fragezeichen.ico");
            this.imageListSymbols.Images.SetKeyName(7, "Kreis.ico");
            this.imageListSymbols.Images.SetKeyName(8, "KreisVoll.ico");
            this.imageListSymbols.Images.SetKeyName(9, "Kreuz.ico");
            this.imageListSymbols.Images.SetKeyName(10, "Minus.ico");
            this.imageListSymbols.Images.SetKeyName(11, "Quadrat.ico");
            this.imageListSymbols.Images.SetKeyName(12, "QuadratVoll.ico");
            this.imageListSymbols.Images.SetKeyName(13, "X.ico");
            // 
            // UserControlSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlSetting";
            this.Size = new System.Drawing.Size(299, 50);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton toolStripButtonSetRestriction;
        private System.Windows.Forms.ToolStripButton toolStripButtonSetColor;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.ToolStripLabel toolStripLabelSize;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxSize;
        private System.Windows.Forms.ToolStripLabel toolStripLabelTransparency;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxTransparency;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.ImageList imageListSymbols;
        private System.Windows.Forms.ToolStripLabel toolStripLabelSymbol;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonSymbol;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonColor;
    }
}
