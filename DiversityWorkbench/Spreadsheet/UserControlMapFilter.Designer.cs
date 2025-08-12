namespace DiversityWorkbench.Spreadsheet
{
    partial class UserControlMapFilter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlMapFilter));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonForwardType = new System.Windows.Forms.Button();
            this.splitContainerDetails = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelGeography = new System.Windows.Forms.TableLayoutPanel();
            this.labelGeography = new System.Windows.Forms.Label();
            this.comboBoxGeographyConversionType = new System.Windows.Forms.ComboBox();
            this.labelGazetteer = new System.Windows.Forms.Label();
            this.comboBoxGazetteer = new System.Windows.Forms.ComboBox();
            this.splitContainerColorSymbol = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelColor = new System.Windows.Forms.TableLayoutPanel();
            this.labelColor = new System.Windows.Forms.Label();
            this.comboBoxColorFilterType = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanelSymbol = new System.Windows.Forms.TableLayoutPanel();
            this.labelSymbol = new System.Windows.Forms.Label();
            this.buttonSymbolAdd = new System.Windows.Forms.Button();
            this.buttonSymbolValuesClear = new System.Windows.Forms.Button();
            this.panelSymbols = new System.Windows.Forms.Panel();
            this.imageListForwardType = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel.SuspendLayout();
            this.splitContainerDetails.Panel1.SuspendLayout();
            this.splitContainerDetails.Panel2.SuspendLayout();
            this.splitContainerDetails.SuspendLayout();
            this.tableLayoutPanelGeography.SuspendLayout();
            this.splitContainerColorSymbol.Panel1.SuspendLayout();
            this.splitContainerColorSymbol.Panel2.SuspendLayout();
            this.splitContainerColorSymbol.SuspendLayout();
            this.tableLayoutPanelColor.SuspendLayout();
            this.tableLayoutPanelSymbol.SuspendLayout();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel.Controls.Add(this.buttonRemove, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonUp, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonDown, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonForwardType, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.splitContainerDetails, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(477, 47);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRemove.FlatAppearance.BorderSize = 0;
            this.buttonRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRemove.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonRemove.Location = new System.Drawing.Point(456, 3);
            this.buttonRemove.Name = "buttonRemove";
            this.tableLayoutPanel.SetRowSpan(this.buttonRemove, 2);
            this.buttonRemove.Size = new System.Drawing.Size(18, 41);
            this.buttonRemove.TabIndex = 0;
            this.toolTip.SetToolTip(this.buttonRemove, "Remove this filter from the list");
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonUp.FlatAppearance.BorderSize = 0;
            this.buttonUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUp.Image = global::DiversityWorkbench.Properties.Resources.ArrowUpSmall;
            this.buttonUp.Location = new System.Drawing.Point(3, 3);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(14, 8);
            this.buttonUp.TabIndex = 1;
            this.toolTip.SetToolTip(this.buttonUp, "Move this filter up");
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Visible = false;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            this.buttonDown.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonDown.FlatAppearance.BorderSize = 0;
            this.buttonDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDown.Image = global::DiversityWorkbench.Properties.Resources.ArrowDownSmall;
            this.buttonDown.Location = new System.Drawing.Point(3, 36);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(14, 8);
            this.buttonDown.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonDown, "Move this filter down");
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Visible = false;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // buttonForwardType
            // 
            this.buttonForwardType.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonForwardType.FlatAppearance.BorderSize = 0;
            this.buttonForwardType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonForwardType.Image = global::DiversityWorkbench.Properties.Resources.ArrowDown;
            this.buttonForwardType.Location = new System.Drawing.Point(436, 22);
            this.buttonForwardType.Name = "buttonForwardType";
            this.tableLayoutPanel.SetRowSpan(this.buttonForwardType, 2);
            this.buttonForwardType.Size = new System.Drawing.Size(14, 22);
            this.buttonForwardType.TabIndex = 3;
            this.toolTip.SetToolTip(this.buttonForwardType, "Decide if the following filters should be applied");
            this.buttonForwardType.UseVisualStyleBackColor = true;
            this.buttonForwardType.Click += new System.EventHandler(this.buttonForwardType_Click);
            // 
            // splitContainerDetails
            // 
            this.splitContainerDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerDetails.Location = new System.Drawing.Point(23, 3);
            this.splitContainerDetails.Name = "splitContainerDetails";
            // 
            // splitContainerDetails.Panel1
            // 
            this.splitContainerDetails.Panel1.Controls.Add(this.tableLayoutPanelGeography);
            // 
            // splitContainerDetails.Panel2
            // 
            this.splitContainerDetails.Panel2.Controls.Add(this.splitContainerColorSymbol);
            this.splitContainerDetails.Panel2Collapsed = true;
            this.tableLayoutPanel.SetRowSpan(this.splitContainerDetails, 2);
            this.splitContainerDetails.Size = new System.Drawing.Size(407, 41);
            this.splitContainerDetails.SplitterDistance = 135;
            this.splitContainerDetails.TabIndex = 6;
            // 
            // tableLayoutPanelGeography
            // 
            this.tableLayoutPanelGeography.ColumnCount = 2;
            this.tableLayoutPanelGeography.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeography.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelGeography.Controls.Add(this.labelGeography, 0, 0);
            this.tableLayoutPanelGeography.Controls.Add(this.comboBoxGeographyConversionType, 1, 0);
            this.tableLayoutPanelGeography.Controls.Add(this.labelGazetteer, 0, 1);
            this.tableLayoutPanelGeography.Controls.Add(this.comboBoxGazetteer, 1, 1);
            this.tableLayoutPanelGeography.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelGeography.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelGeography.Name = "tableLayoutPanelGeography";
            this.tableLayoutPanelGeography.RowCount = 2;
            this.tableLayoutPanelGeography.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelGeography.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelGeography.Size = new System.Drawing.Size(407, 41);
            this.tableLayoutPanelGeography.TabIndex = 0;
            // 
            // labelGeography
            // 
            this.labelGeography.AutoSize = true;
            this.labelGeography.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGeography.Location = new System.Drawing.Point(3, 0);
            this.labelGeography.Name = "labelGeography";
            this.labelGeography.Size = new System.Drawing.Size(101, 20);
            this.labelGeography.TabIndex = 0;
            this.labelGeography.Text = "Type of conversion:";
            this.labelGeography.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxGeographyConversionType
            // 
            this.comboBoxGeographyConversionType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxGeographyConversionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGeographyConversionType.FormattingEnabled = true;
            this.comboBoxGeographyConversionType.Location = new System.Drawing.Point(107, 0);
            this.comboBoxGeographyConversionType.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxGeographyConversionType.Name = "comboBoxGeographyConversionType";
            this.comboBoxGeographyConversionType.Size = new System.Drawing.Size(300, 21);
            this.comboBoxGeographyConversionType.TabIndex = 1;
            this.comboBoxGeographyConversionType.SelectionChangeCommitted += new System.EventHandler(this.comboBoxGeographyConversionType_SelectionChangeCommitted);
            // 
            // labelGazetteer
            // 
            this.labelGazetteer.AutoSize = true;
            this.labelGazetteer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGazetteer.Location = new System.Drawing.Point(3, 20);
            this.labelGazetteer.Name = "labelGazetteer";
            this.labelGazetteer.Size = new System.Drawing.Size(101, 21);
            this.labelGazetteer.TabIndex = 2;
            this.labelGazetteer.Text = "Gazetteer:";
            this.labelGazetteer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxGazetteer
            // 
            this.comboBoxGazetteer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxGazetteer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGazetteer.DropDownWidth = 500;
            this.comboBoxGazetteer.FormattingEnabled = true;
            this.comboBoxGazetteer.Location = new System.Drawing.Point(107, 20);
            this.comboBoxGazetteer.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxGazetteer.Name = "comboBoxGazetteer";
            this.comboBoxGazetteer.Size = new System.Drawing.Size(300, 21);
            this.comboBoxGazetteer.TabIndex = 3;
            this.comboBoxGazetteer.SelectedIndexChanged += new System.EventHandler(this.comboBoxGazetteer_SelectedIndexChanged);
            this.comboBoxGazetteer.SelectionChangeCommitted += new System.EventHandler(this.comboBoxGazetteer_SelectionChangeCommitted);
            // 
            // splitContainerColorSymbol
            // 
            this.splitContainerColorSymbol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerColorSymbol.Location = new System.Drawing.Point(0, 0);
            this.splitContainerColorSymbol.Name = "splitContainerColorSymbol";
            // 
            // splitContainerColorSymbol.Panel1
            // 
            this.splitContainerColorSymbol.Panel1.Controls.Add(this.tableLayoutPanelColor);
            this.splitContainerColorSymbol.Panel1Collapsed = true;
            // 
            // splitContainerColorSymbol.Panel2
            // 
            this.splitContainerColorSymbol.Panel2.Controls.Add(this.tableLayoutPanelSymbol);
            this.splitContainerColorSymbol.Size = new System.Drawing.Size(96, 100);
            this.splitContainerColorSymbol.SplitterDistance = 71;
            this.splitContainerColorSymbol.TabIndex = 0;
            // 
            // tableLayoutPanelColor
            // 
            this.tableLayoutPanelColor.ColumnCount = 2;
            this.tableLayoutPanelColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelColor.Controls.Add(this.labelColor, 0, 0);
            this.tableLayoutPanelColor.Controls.Add(this.comboBoxColorFilterType, 1, 0);
            this.tableLayoutPanelColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelColor.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelColor.Name = "tableLayoutPanelColor";
            this.tableLayoutPanelColor.RowCount = 1;
            this.tableLayoutPanelColor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelColor.Size = new System.Drawing.Size(71, 100);
            this.tableLayoutPanelColor.TabIndex = 0;
            // 
            // labelColor
            // 
            this.labelColor.AutoSize = true;
            this.labelColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelColor.Location = new System.Drawing.Point(3, 0);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(34, 100);
            this.labelColor.TabIndex = 0;
            this.labelColor.Text = "Type:";
            this.labelColor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxColorFilterType
            // 
            this.comboBoxColorFilterType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxColorFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxColorFilterType.FormattingEnabled = true;
            this.comboBoxColorFilterType.Location = new System.Drawing.Point(40, 0);
            this.comboBoxColorFilterType.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxColorFilterType.Name = "comboBoxColorFilterType";
            this.comboBoxColorFilterType.Size = new System.Drawing.Size(31, 21);
            this.comboBoxColorFilterType.TabIndex = 1;
            this.toolTip.SetToolTip(this.comboBoxColorFilterType, "Type of the color filter");
            this.comboBoxColorFilterType.SelectionChangeCommitted += new System.EventHandler(this.comboBoxColorFilterType_SelectionChangeCommitted);
            // 
            // tableLayoutPanelSymbol
            // 
            this.tableLayoutPanelSymbol.ColumnCount = 4;
            this.tableLayoutPanelSymbol.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSymbol.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelSymbol.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelSymbol.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSymbol.Controls.Add(this.labelSymbol, 0, 0);
            this.tableLayoutPanelSymbol.Controls.Add(this.buttonSymbolAdd, 1, 0);
            this.tableLayoutPanelSymbol.Controls.Add(this.buttonSymbolValuesClear, 2, 0);
            this.tableLayoutPanelSymbol.Controls.Add(this.panelSymbols, 3, 0);
            this.tableLayoutPanelSymbol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSymbol.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSymbol.Name = "tableLayoutPanelSymbol";
            this.tableLayoutPanelSymbol.RowCount = 1;
            this.tableLayoutPanelSymbol.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSymbol.Size = new System.Drawing.Size(96, 100);
            this.tableLayoutPanelSymbol.TabIndex = 0;
            // 
            // labelSymbol
            // 
            this.labelSymbol.AutoSize = true;
            this.labelSymbol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSymbol.Location = new System.Drawing.Point(0, 0);
            this.labelSymbol.Margin = new System.Windows.Forms.Padding(0);
            this.labelSymbol.Name = "labelSymbol";
            this.labelSymbol.Size = new System.Drawing.Size(42, 100);
            this.labelSymbol.TabIndex = 0;
            this.labelSymbol.Text = "Values:";
            this.labelSymbol.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonSymbolAdd
            // 
            this.buttonSymbolAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSymbolAdd.FlatAppearance.BorderSize = 0;
            this.buttonSymbolAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSymbolAdd.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.buttonSymbolAdd.Location = new System.Drawing.Point(42, 0);
            this.buttonSymbolAdd.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSymbolAdd.Name = "buttonSymbolAdd";
            this.buttonSymbolAdd.Size = new System.Drawing.Size(20, 100);
            this.buttonSymbolAdd.TabIndex = 1;
            this.toolTip.SetToolTip(this.buttonSymbolAdd, "Add a symbol value");
            this.buttonSymbolAdd.UseVisualStyleBackColor = true;
            this.buttonSymbolAdd.Click += new System.EventHandler(this.buttonSymbolAdd_Click);
            // 
            // buttonSymbolValuesClear
            // 
            this.buttonSymbolValuesClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSymbolValuesClear.FlatAppearance.BorderSize = 0;
            this.buttonSymbolValuesClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSymbolValuesClear.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonSymbolValuesClear.Location = new System.Drawing.Point(62, 0);
            this.buttonSymbolValuesClear.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSymbolValuesClear.Name = "buttonSymbolValuesClear";
            this.buttonSymbolValuesClear.Size = new System.Drawing.Size(20, 100);
            this.buttonSymbolValuesClear.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonSymbolValuesClear, "Clear the symbol value list");
            this.buttonSymbolValuesClear.UseVisualStyleBackColor = true;
            this.buttonSymbolValuesClear.Click += new System.EventHandler(this.buttonSymbolValuesClear_Click);
            // 
            // panelSymbols
            // 
            this.panelSymbols.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSymbols.Location = new System.Drawing.Point(82, 0);
            this.panelSymbols.Margin = new System.Windows.Forms.Padding(0);
            this.panelSymbols.Name = "panelSymbols";
            this.panelSymbols.Size = new System.Drawing.Size(14, 100);
            this.panelSymbols.TabIndex = 3;
            // 
            // imageListForwardType
            // 
            this.imageListForwardType.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListForwardType.ImageStream")));
            this.imageListForwardType.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListForwardType.Images.SetKeyName(0, "ArrowDown.ico");
            this.imageListForwardType.Images.SetKeyName(1, "ArrowButtom.ico");
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.tableLayoutPanel);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(483, 66);
            this.groupBox.TabIndex = 1;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "groupBox1";
            // 
            // UserControlMapFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox);
            this.Name = "UserControlMapFilter";
            this.Size = new System.Drawing.Size(483, 66);
            this.tableLayoutPanel.ResumeLayout(false);
            this.splitContainerDetails.Panel1.ResumeLayout(false);
            this.splitContainerDetails.Panel2.ResumeLayout(false);
            this.splitContainerDetails.ResumeLayout(false);
            this.tableLayoutPanelGeography.ResumeLayout(false);
            this.tableLayoutPanelGeography.PerformLayout();
            this.splitContainerColorSymbol.Panel1.ResumeLayout(false);
            this.splitContainerColorSymbol.Panel2.ResumeLayout(false);
            this.splitContainerColorSymbol.ResumeLayout(false);
            this.tableLayoutPanelColor.ResumeLayout(false);
            this.tableLayoutPanelColor.PerformLayout();
            this.tableLayoutPanelSymbol.ResumeLayout(false);
            this.tableLayoutPanelSymbol.PerformLayout();
            this.groupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonForwardType;
        private System.Windows.Forms.ImageList imageListForwardType;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.SplitContainer splitContainerDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelGeography;
        private System.Windows.Forms.Label labelGeography;
        private System.Windows.Forms.ComboBox comboBoxGeographyConversionType;
        private System.Windows.Forms.SplitContainer splitContainerColorSymbol;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelColor;
        private System.Windows.Forms.Label labelColor;
        private System.Windows.Forms.ComboBox comboBoxColorFilterType;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSymbol;
        private System.Windows.Forms.Label labelSymbol;
        private System.Windows.Forms.Button buttonSymbolAdd;
        private System.Windows.Forms.Button buttonSymbolValuesClear;
        private System.Windows.Forms.Panel panelSymbols;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Label labelGazetteer;
        private System.Windows.Forms.ComboBox comboBoxGazetteer;
    }
}
