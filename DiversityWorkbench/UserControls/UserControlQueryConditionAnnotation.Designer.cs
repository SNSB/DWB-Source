namespace DiversityWorkbench.UserControls
{
    partial class UserControlQueryConditionAnnotation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlQueryConditionAnnotation));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCondition = new System.Windows.Forms.Button();
            this.comboBoxReferencedTable = new System.Windows.Forms.ComboBox();
            this.buttonQueryConditionOperator = new System.Windows.Forms.Button();
            this.comboBoxQueryConditionOperator = new System.Windows.Forms.ComboBox();
            this.comboBoxAnnotationType = new System.Windows.Forms.ComboBox();
            this.pictureBoxAnnotationType = new System.Windows.Forms.PictureBox();
            this.labelReferencedTable = new System.Windows.Forms.Label();
            this.panelCondition = new System.Windows.Forms.Panel();
            this.textBoxQueryCondition = new System.Windows.Forms.TextBox();
            this.labelNull = new System.Windows.Forms.Label();
            this.toolTipQueryCondition = new System.Windows.Forms.ToolTip(this.components);
            this.imageListAnnotationType = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAnnotationType)).BeginInit();
            this.panelCondition.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 6;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.buttonCondition, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxReferencedTable, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonQueryConditionOperator, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxQueryConditionOperator, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxAnnotationType, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxAnnotationType, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.labelReferencedTable, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.panelCondition, 5, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 2);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(303, 41);
            this.tableLayoutPanel.TabIndex = 11;
            // 
            // buttonCondition
            // 
            this.tableLayoutPanel.SetColumnSpan(this.buttonCondition, 3);
            this.buttonCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCondition.FlatAppearance.BorderSize = 0;
            this.buttonCondition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCondition.Location = new System.Drawing.Point(0, 0);
            this.buttonCondition.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCondition.Name = "buttonCondition";
            this.buttonCondition.Size = new System.Drawing.Size(71, 20);
            this.buttonCondition.TabIndex = 9;
            this.buttonCondition.TabStop = false;
            this.buttonCondition.Text = "Condition";
            this.buttonCondition.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.buttonCondition.UseVisualStyleBackColor = true;
            // 
            // comboBoxReferencedTable
            // 
            this.comboBoxReferencedTable.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel.SetColumnSpan(this.comboBoxReferencedTable, 3);
            this.comboBoxReferencedTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxReferencedTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxReferencedTable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxReferencedTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxReferencedTable.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.comboBoxReferencedTable.FormattingEnabled = true;
            this.comboBoxReferencedTable.ItemHeight = 12;
            this.comboBoxReferencedTable.Location = new System.Drawing.Point(71, 20);
            this.comboBoxReferencedTable.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxReferencedTable.Name = "comboBoxReferencedTable";
            this.comboBoxReferencedTable.Size = new System.Drawing.Size(232, 20);
            this.comboBoxReferencedTable.TabIndex = 13;
            // 
            // buttonQueryConditionOperator
            // 
            this.buttonQueryConditionOperator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonQueryConditionOperator.FlatAppearance.BorderSize = 0;
            this.buttonQueryConditionOperator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonQueryConditionOperator.Location = new System.Drawing.Point(88, 0);
            this.buttonQueryConditionOperator.Margin = new System.Windows.Forms.Padding(0);
            this.buttonQueryConditionOperator.Name = "buttonQueryConditionOperator";
            this.buttonQueryConditionOperator.Size = new System.Drawing.Size(17, 20);
            this.buttonQueryConditionOperator.TabIndex = 11;
            this.buttonQueryConditionOperator.TabStop = false;
            this.buttonQueryConditionOperator.Text = "~";
            this.buttonQueryConditionOperator.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonQueryConditionOperator.UseVisualStyleBackColor = true;
            this.buttonQueryConditionOperator.TextChanged += new System.EventHandler(this.buttonQueryConditionOperator_TextChanged);
            // 
            // comboBoxQueryConditionOperator
            // 
            this.comboBoxQueryConditionOperator.BackColor = System.Drawing.SystemColors.Control;
            this.comboBoxQueryConditionOperator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxQueryConditionOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxQueryConditionOperator.DropDownWidth = 20;
            this.comboBoxQueryConditionOperator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxQueryConditionOperator.FormattingEnabled = true;
            this.comboBoxQueryConditionOperator.Items.AddRange(new object[] {
            "~",
            "=",
            "•"});
            this.comboBoxQueryConditionOperator.Location = new System.Drawing.Point(71, 0);
            this.comboBoxQueryConditionOperator.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxQueryConditionOperator.Name = "comboBoxQueryConditionOperator";
            this.comboBoxQueryConditionOperator.Size = new System.Drawing.Size(17, 21);
            this.comboBoxQueryConditionOperator.TabIndex = 10;
            this.comboBoxQueryConditionOperator.TabStop = false;
            this.comboBoxQueryConditionOperator.SelectedIndexChanged += new System.EventHandler(this.comboBoxQueryConditionOperator_SelectedIndexChanged);
            // 
            // comboBoxAnnotationType
            // 
            this.comboBoxAnnotationType.BackColor = System.Drawing.SystemColors.Control;
            this.comboBoxAnnotationType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxAnnotationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAnnotationType.DropDownWidth = 100;
            this.comboBoxAnnotationType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxAnnotationType.FormattingEnabled = true;
            this.comboBoxAnnotationType.Items.AddRange(new object[] {
            "",
            "Annotation",
            "Problem",
            "Reference"});
            this.comboBoxAnnotationType.Location = new System.Drawing.Point(0, 20);
            this.comboBoxAnnotationType.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxAnnotationType.Name = "comboBoxAnnotationType";
            this.comboBoxAnnotationType.Size = new System.Drawing.Size(16, 21);
            this.comboBoxAnnotationType.TabIndex = 23;
            this.comboBoxAnnotationType.SelectionChangeCommitted += new System.EventHandler(this.comboBoxAnnotationType_SelectionChangeCommitted);
            // 
            // pictureBoxAnnotationType
            // 
            this.pictureBoxAnnotationType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxAnnotationType.Location = new System.Drawing.Point(17, 22);
            this.pictureBoxAnnotationType.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.pictureBoxAnnotationType.Name = "pictureBoxAnnotationType";
            this.pictureBoxAnnotationType.Size = new System.Drawing.Size(16, 17);
            this.pictureBoxAnnotationType.TabIndex = 24;
            this.pictureBoxAnnotationType.TabStop = false;
            // 
            // labelReferencedTable
            // 
            this.labelReferencedTable.AutoSize = true;
            this.labelReferencedTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReferencedTable.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelReferencedTable.Location = new System.Drawing.Point(34, 20);
            this.labelReferencedTable.Margin = new System.Windows.Forms.Padding(0);
            this.labelReferencedTable.Name = "labelReferencedTable";
            this.labelReferencedTable.Size = new System.Drawing.Size(37, 21);
            this.labelReferencedTable.TabIndex = 25;
            this.labelReferencedTable.Text = "Table:";
            this.labelReferencedTable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelCondition
            // 
            this.panelCondition.Controls.Add(this.textBoxQueryCondition);
            this.panelCondition.Controls.Add(this.labelNull);
            this.panelCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCondition.Location = new System.Drawing.Point(105, 0);
            this.panelCondition.Margin = new System.Windows.Forms.Padding(0);
            this.panelCondition.Name = "panelCondition";
            this.panelCondition.Size = new System.Drawing.Size(198, 20);
            this.panelCondition.TabIndex = 26;
            // 
            // textBoxQueryCondition
            // 
            this.textBoxQueryCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxQueryCondition.Location = new System.Drawing.Point(0, 0);
            this.textBoxQueryCondition.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxQueryCondition.Name = "textBoxQueryCondition";
            this.textBoxQueryCondition.Size = new System.Drawing.Size(157, 20);
            this.textBoxQueryCondition.TabIndex = 14;
            this.textBoxQueryCondition.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelNull
            // 
            this.labelNull.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelNull.Location = new System.Drawing.Point(157, 0);
            this.labelNull.Name = "labelNull";
            this.labelNull.Size = new System.Drawing.Size(41, 20);
            this.labelNull.TabIndex = 19;
            this.labelNull.Text = "present";
            this.labelNull.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelNull.Visible = false;
            // 
            // imageListAnnotationType
            // 
            this.imageListAnnotationType.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListAnnotationType.ImageStream")));
            this.imageListAnnotationType.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListAnnotationType.Images.SetKeyName(0, "Note.ico");
            this.imageListAnnotationType.Images.SetKeyName(1, "References.ico");
            this.imageListAnnotationType.Images.SetKeyName(2, "Problem.ico");
            // 
            // UserControlQueryConditionAnnotation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlQueryConditionAnnotation";
            this.Padding = new System.Windows.Forms.Padding(0, 2, 0, 1);
            this.Size = new System.Drawing.Size(303, 44);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAnnotationType)).EndInit();
            this.panelCondition.ResumeLayout(false);
            this.panelCondition.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TextBox textBoxQueryCondition;
        private System.Windows.Forms.Button buttonCondition;
        private System.Windows.Forms.ComboBox comboBoxReferencedTable;
        private System.Windows.Forms.Button buttonQueryConditionOperator;
        private System.Windows.Forms.ComboBox comboBoxQueryConditionOperator;
        private System.Windows.Forms.Label labelNull;
        private System.Windows.Forms.ToolTip toolTipQueryCondition;
        private System.Windows.Forms.ComboBox comboBoxAnnotationType;
        private System.Windows.Forms.PictureBox pictureBoxAnnotationType;
        private System.Windows.Forms.ImageList imageListAnnotationType;
        private System.Windows.Forms.Label labelReferencedTable;
        private System.Windows.Forms.Panel panelCondition;
    }
}
