namespace DiversityWorkbench.Spreadsheet
{
    partial class UserControlTableSetting
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
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.labelDisplayText = new System.Windows.Forms.Label();
            this.textBoxDisplayText = new System.Windows.Forms.TextBox();
            this.groupBoxColumns = new System.Windows.Forms.GroupBox();
            this.panelColumns = new System.Windows.Forms.Panel();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.labelTableName = new System.Windows.Forms.Label();
            this.labelOperator = new System.Windows.Forms.Label();
            this.comboBoxOperator = new System.Windows.Forms.ComboBox();
            this.labelOperatorExplanation = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanelMain.SuspendLayout();
            this.groupBoxColumns.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 6;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.Controls.Add(this.buttonAdd, 5, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelDisplayText, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxDisplayText, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.groupBoxColumns, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.buttonRemove, 4, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelTableName, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelOperator, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.comboBoxOperator, 2, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelOperatorExplanation, 3, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelDescription, 0, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 4;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(661, 217);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonAdd.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.buttonAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonAdd.Location = new System.Drawing.Point(541, 52);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(117, 23);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "Add another table";
            this.buttonAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // labelDisplayText
            // 
            this.labelDisplayText.AutoSize = true;
            this.labelDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDisplayText.Location = new System.Drawing.Point(3, 23);
            this.labelDisplayText.Name = "labelDisplayText";
            this.labelDisplayText.Size = new System.Drawing.Size(105, 26);
            this.labelDisplayText.TabIndex = 4;
            this.labelDisplayText.Text = "Display text for table:";
            this.labelDisplayText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxDisplayText
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxDisplayText, 5);
            this.textBoxDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDisplayText.Location = new System.Drawing.Point(114, 26);
            this.textBoxDisplayText.Name = "textBoxDisplayText";
            this.textBoxDisplayText.Size = new System.Drawing.Size(544, 20);
            this.textBoxDisplayText.TabIndex = 5;
            this.textBoxDisplayText.Leave += new System.EventHandler(this.textBoxDisplayText_Leave);
            // 
            // groupBoxColumns
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.groupBoxColumns, 6);
            this.groupBoxColumns.Controls.Add(this.panelColumns);
            this.groupBoxColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxColumns.Location = new System.Drawing.Point(3, 81);
            this.groupBoxColumns.Name = "groupBoxColumns";
            this.groupBoxColumns.Size = new System.Drawing.Size(655, 133);
            this.groupBoxColumns.TabIndex = 6;
            this.groupBoxColumns.TabStop = false;
            this.groupBoxColumns.Text = "Columns";
            // 
            // panelColumns
            // 
            this.panelColumns.AutoScroll = true;
            this.panelColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelColumns.Location = new System.Drawing.Point(3, 16);
            this.panelColumns.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.panelColumns.Name = "panelColumns";
            this.panelColumns.Size = new System.Drawing.Size(649, 114);
            this.panelColumns.TabIndex = 1;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonRemove.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonRemove.Location = new System.Drawing.Point(383, 52);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(71, 23);
            this.buttonRemove.TabIndex = 7;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonRemove, "Remove this table");
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // labelTableName
            // 
            this.labelTableName.AutoSize = true;
            this.labelTableName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTableName.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelTableName.Location = new System.Drawing.Point(3, 49);
            this.labelTableName.Name = "labelTableName";
            this.labelTableName.Size = new System.Drawing.Size(105, 29);
            this.labelTableName.TabIndex = 8;
            this.labelTableName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelOperator
            // 
            this.labelOperator.AutoSize = true;
            this.labelOperator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOperator.Location = new System.Drawing.Point(114, 49);
            this.labelOperator.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelOperator.Name = "labelOperator";
            this.labelOperator.Size = new System.Drawing.Size(32, 29);
            this.labelOperator.TabIndex = 9;
            this.labelOperator.Text = "Filter:";
            this.labelOperator.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxOperator
            // 
            this.comboBoxOperator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxOperator.FormattingEnabled = true;
            this.comboBoxOperator.Location = new System.Drawing.Point(146, 52);
            this.comboBoxOperator.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.comboBoxOperator.Name = "comboBoxOperator";
            this.comboBoxOperator.Size = new System.Drawing.Size(30, 21);
            this.comboBoxOperator.TabIndex = 10;
            this.toolTip.SetToolTip(this.comboBoxOperator, "the operator for filtering the data");
            this.comboBoxOperator.DropDown += new System.EventHandler(this.comboBoxOperator_DropDown);
            this.comboBoxOperator.SelectionChangeCommitted += new System.EventHandler(this.comboBoxOperator_SelectionChangeCommitted);
            // 
            // labelOperatorExplanation
            // 
            this.labelOperatorExplanation.AutoSize = true;
            this.labelOperatorExplanation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOperatorExplanation.Location = new System.Drawing.Point(176, 49);
            this.labelOperatorExplanation.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelOperatorExplanation.Name = "labelOperatorExplanation";
            this.labelOperatorExplanation.Size = new System.Drawing.Size(201, 29);
            this.labelOperatorExplanation.TabIndex = 11;
            this.labelOperatorExplanation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDescription
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.labelDescription, 6);
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDescription.Location = new System.Drawing.Point(3, 0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(655, 23);
            this.labelDescription.TabIndex = 12;
            this.labelDescription.Text = "label1";
            this.labelDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelDescription.Visible = false;
            // 
            // UserControlTableSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "UserControlTableSetting";
            this.Size = new System.Drawing.Size(661, 217);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.groupBoxColumns.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Panel panelColumns;
        private System.Windows.Forms.Label labelDisplayText;
        private System.Windows.Forms.TextBox textBoxDisplayText;
        private System.Windows.Forms.GroupBox groupBoxColumns;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelTableName;
        private System.Windows.Forms.Label labelOperator;
        private System.Windows.Forms.ComboBox comboBoxOperator;
        private System.Windows.Forms.Label labelOperatorExplanation;
        private System.Windows.Forms.Label labelDescription;
    }
}
