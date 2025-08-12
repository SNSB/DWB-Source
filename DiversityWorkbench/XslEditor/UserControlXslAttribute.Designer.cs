namespace DiversityWorkbench.XslEditor
{
    partial class UserControlXslAttribute
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
            this.buttonDelete = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonAddCondition = new System.Windows.Forms.Button();
            this.buttonRemoveCondition = new System.Windows.Forms.Button();
            this.comboBoxValue = new System.Windows.Forms.ComboBox();
            this.label = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panelAttributeConditions = new System.Windows.Forms.Panel();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.labelName = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonDelete
            // 
            this.buttonDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDelete.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonDelete.Location = new System.Drawing.Point(817, 0);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(22, 22);
            this.buttonDelete.TabIndex = 0;
            this.toolTip.SetToolTip(this.buttonDelete, "Delete this attribute");
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonAddCondition
            // 
            this.buttonAddCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAddCondition.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.buttonAddCondition.Location = new System.Drawing.Point(795, 0);
            this.buttonAddCondition.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAddCondition.Name = "buttonAddCondition";
            this.buttonAddCondition.Size = new System.Drawing.Size(22, 22);
            this.buttonAddCondition.TabIndex = 7;
            this.toolTip.SetToolTip(this.buttonAddCondition, "Add condition");
            this.buttonAddCondition.UseVisualStyleBackColor = true;
            this.buttonAddCondition.Click += new System.EventHandler(this.buttonAddCondition_Click);
            // 
            // buttonRemoveCondition
            // 
            this.buttonRemoveCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRemoveCondition.Image = global::DiversityWorkbench.Properties.Resources.Minus;
            this.buttonRemoveCondition.Location = new System.Drawing.Point(773, 0);
            this.buttonRemoveCondition.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRemoveCondition.Name = "buttonRemoveCondition";
            this.buttonRemoveCondition.Size = new System.Drawing.Size(22, 22);
            this.buttonRemoveCondition.TabIndex = 10;
            this.toolTip.SetToolTip(this.buttonRemoveCondition, "Remove the last condition");
            this.buttonRemoveCondition.UseVisualStyleBackColor = true;
            this.buttonRemoveCondition.Click += new System.EventHandler(this.buttonRemoveCondition_Click);
            // 
            // comboBoxValue
            // 
            this.comboBoxValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxValue.FormattingEnabled = true;
            this.comboBoxValue.Location = new System.Drawing.Point(79, 0);
            this.comboBoxValue.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxValue.Name = "comboBoxValue";
            this.comboBoxValue.Size = new System.Drawing.Size(306, 21);
            this.comboBoxValue.TabIndex = 2;
            this.comboBoxValue.DropDown += new System.EventHandler(this.comboBoxValue_DropDown);
            this.comboBoxValue.Leave += new System.EventHandler(this.comboBoxValue_Leave);
            // 
            // label
            // 
            this.label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label.Location = new System.Drawing.Point(66, 0);
            this.label.Margin = new System.Windows.Forms.Padding(0);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(13, 22);
            this.label.TabIndex = 3;
            this.label.Text = "=";
            this.label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 9;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.buttonDelete, 8, 0);
            this.tableLayoutPanel.Controls.Add(this.panelAttributeConditions, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.label, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxValue, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonEdit, 5, 0);
            this.tableLayoutPanel.Controls.Add(this.labelName, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonAddCondition, 7, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonRemoveCondition, 6, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(839, 22);
            this.tableLayoutPanel.TabIndex = 4;
            // 
            // panelAttributeConditions
            // 
            this.panelAttributeConditions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAttributeConditions.Location = new System.Drawing.Point(385, 0);
            this.panelAttributeConditions.Margin = new System.Windows.Forms.Padding(0);
            this.panelAttributeConditions.Name = "panelAttributeConditions";
            this.tableLayoutPanel.SetRowSpan(this.panelAttributeConditions, 2);
            this.panelAttributeConditions.Size = new System.Drawing.Size(364, 22);
            this.panelAttributeConditions.TabIndex = 9;
            // 
            // buttonEdit
            // 
            this.buttonEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEdit.Image = global::DiversityWorkbench.ResourceWorkbench.Edit;
            this.buttonEdit.Location = new System.Drawing.Point(753, 0);
            this.buttonEdit.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(20, 22);
            this.buttonEdit.TabIndex = 4;
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Visible = false;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelName.Location = new System.Drawing.Point(3, 0);
            this.labelName.MaximumSize = new System.Drawing.Size(200, 0);
            this.labelName.MinimumSize = new System.Drawing.Size(60, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(60, 22);
            this.labelName.TabIndex = 6;
            this.labelName.Text = "Attribute";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UserControlXslAttribute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlXslAttribute";
            this.Size = new System.Drawing.Size(839, 22);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ComboBox comboBoxValue;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Button buttonAddCondition;
        private System.Windows.Forms.Panel panelAttributeConditions;
        private System.Windows.Forms.Button buttonRemoveCondition;
    }
}
