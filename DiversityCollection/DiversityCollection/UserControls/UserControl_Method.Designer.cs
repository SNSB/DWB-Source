namespace DiversityCollection.UserControls
{
    partial class UserControl_Method
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_Method));
            this.groupBoxMethod = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelMethod = new System.Windows.Forms.TableLayoutPanel();
            this.listBoxMethod = new System.Windows.Forms.ListBox();
            this.toolStripMethod = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonMethodAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMethodDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabelMethodMarker = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBoxMethodMarker = new System.Windows.Forms.ToolStripTextBox();
            this.listBoxMethodParameter = new System.Windows.Forms.ListBox();
            this.panelMethodParameterLabel = new System.Windows.Forms.Panel();
            this.buttonMethodParameterRemove = new System.Windows.Forms.Button();
            this.buttonMethodParameterAddMissing = new System.Windows.Forms.Button();
            this.pictureBoxMethodParameterValue = new System.Windows.Forms.PictureBox();
            this.labelMethodParameterValue = new System.Windows.Forms.Label();
            this.panelMethodParameter = new System.Windows.Forms.Panel();
            this.comboBoxMethodParameterValue = new System.Windows.Forms.ComboBox();
            this.textBoxMethodParameterValue = new System.Windows.Forms.TextBox();
            this.pictureBoxMethod = new System.Windows.Forms.PictureBox();
            this.groupBoxMethod.SuspendLayout();
            this.tableLayoutPanelMethod.SuspendLayout();
            this.toolStripMethod.SuspendLayout();
            this.panelMethodParameterLabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMethodParameterValue)).BeginInit();
            this.panelMethodParameter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMethod)).BeginInit();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxMethod
            // 
            this.groupBoxMethod.Controls.Add(this.tableLayoutPanelMethod);
            this.groupBoxMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxMethod.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxMethod.Location = new System.Drawing.Point(0, 0);
            this.groupBoxMethod.Name = "groupBoxMethod";
            this.groupBoxMethod.Size = new System.Drawing.Size(529, 150);
            this.groupBoxMethod.TabIndex = 0;
            this.groupBoxMethod.TabStop = false;
            this.groupBoxMethod.Text = "Methods";
            // 
            // tableLayoutPanelMethod
            // 
            this.tableLayoutPanelMethod.ColumnCount = 2;
            this.tableLayoutPanelMethod.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMethod.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMethod.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMethod.Controls.Add(this.listBoxMethod, 0, 0);
            this.tableLayoutPanelMethod.Controls.Add(this.toolStripMethod, 0, 3);
            this.tableLayoutPanelMethod.Controls.Add(this.listBoxMethodParameter, 1, 0);
            this.tableLayoutPanelMethod.Controls.Add(this.panelMethodParameterLabel, 1, 1);
            this.tableLayoutPanelMethod.Controls.Add(this.panelMethodParameter, 1, 2);
            this.tableLayoutPanelMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMethod.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelMethod.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelMethod.Name = "tableLayoutPanelMethod";
            this.tableLayoutPanelMethod.RowCount = 4;
            this.tableLayoutPanelMethod.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMethod.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMethod.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelMethod.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMethod.Size = new System.Drawing.Size(523, 131);
            this.tableLayoutPanelMethod.TabIndex = 2;
            // 
            // listBoxMethod
            // 
            this.listBoxMethod.DisplayMember = "DisplayText";
            this.listBoxMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxMethod.FormattingEnabled = true;
            this.listBoxMethod.IntegralHeight = false;
            this.listBoxMethod.Location = new System.Drawing.Point(0, 0);
            this.listBoxMethod.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxMethod.Name = "listBoxMethod";
            this.tableLayoutPanelMethod.SetRowSpan(this.listBoxMethod, 3);
            this.listBoxMethod.Size = new System.Drawing.Size(272, 106);
            this.listBoxMethod.TabIndex = 0;
            this.listBoxMethod.SelectedIndexChanged += new System.EventHandler(this.listBoxMethod_SelectedIndexChanged);
            // 
            // toolStripMethod
            // 
            this.toolStripMethod.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripMethod.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMethod.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonMethodAdd,
            this.toolStripButtonMethodDelete,
            this.toolStripLabelMethodMarker,
            this.toolStripTextBoxMethodMarker});
            this.toolStripMethod.Location = new System.Drawing.Point(0, 106);
            this.toolStripMethod.Name = "toolStripMethod";
            this.toolStripMethod.Size = new System.Drawing.Size(272, 25);
            this.toolStripMethod.TabIndex = 1;
            this.toolStripMethod.Text = "toolStrip2";
            // 
            // toolStripButtonMethodAdd
            // 
            this.toolStripButtonMethodAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMethodAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonMethodAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMethodAdd.Name = "toolStripButtonMethodAdd";
            this.toolStripButtonMethodAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonMethodAdd.Text = "Add a new method";
            this.toolStripButtonMethodAdd.Click += new System.EventHandler(this.toolStripButtonMethodAdd_Click);
            // 
            // toolStripButtonMethodDelete
            // 
            this.toolStripButtonMethodDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMethodDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonMethodDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMethodDelete.Name = "toolStripButtonMethodDelete";
            this.toolStripButtonMethodDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonMethodDelete.Text = "Remove the selected method";
            this.toolStripButtonMethodDelete.Click += new System.EventHandler(this.toolStripButtonMethodDelete_Click);
            // 
            // toolStripLabelMethodMarker
            // 
            this.toolStripLabelMethodMarker.ForeColor = System.Drawing.Color.Black;
            this.toolStripLabelMethodMarker.Name = "toolStripLabelMethodMarker";
            this.toolStripLabelMethodMarker.Size = new System.Drawing.Size(40, 22);
            this.toolStripLabelMethodMarker.Text = "Mark.:";
            // 
            // toolStripTextBoxMethodMarker
            // 
            this.toolStripTextBoxMethodMarker.Name = "toolStripTextBoxMethodMarker";
            this.toolStripTextBoxMethodMarker.ReadOnly = true;
            this.toolStripTextBoxMethodMarker.Size = new System.Drawing.Size(45, 25);
            // 
            // listBoxMethodParameter
            // 
            this.listBoxMethodParameter.DisplayMember = "DisplayText";
            this.listBoxMethodParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxMethodParameter.FormattingEnabled = true;
            this.listBoxMethodParameter.IntegralHeight = false;
            this.listBoxMethodParameter.Location = new System.Drawing.Point(275, 0);
            this.listBoxMethodParameter.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.listBoxMethodParameter.Name = "listBoxMethodParameter";
            this.listBoxMethodParameter.Size = new System.Drawing.Size(248, 50);
            this.listBoxMethodParameter.TabIndex = 2;
            this.listBoxMethodParameter.SelectedIndexChanged += new System.EventHandler(this.listBoxMethodParameter_SelectedIndexChanged);
            // 
            // panelMethodParameterLabel
            // 
            this.panelMethodParameterLabel.Controls.Add(this.buttonMethodParameterRemove);
            this.panelMethodParameterLabel.Controls.Add(this.buttonMethodParameterAddMissing);
            this.panelMethodParameterLabel.Controls.Add(this.pictureBoxMethodParameterValue);
            this.panelMethodParameterLabel.Controls.Add(this.labelMethodParameterValue);
            this.panelMethodParameterLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMethodParameterLabel.Location = new System.Drawing.Point(272, 50);
            this.panelMethodParameterLabel.Margin = new System.Windows.Forms.Padding(0);
            this.panelMethodParameterLabel.Name = "panelMethodParameterLabel";
            this.panelMethodParameterLabel.Size = new System.Drawing.Size(251, 16);
            this.panelMethodParameterLabel.TabIndex = 6;
            // 
            // buttonMethodParameterRemove
            // 
            this.buttonMethodParameterRemove.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonMethodParameterRemove.FlatAppearance.BorderSize = 0;
            this.buttonMethodParameterRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMethodParameterRemove.Image = global::DiversityCollection.Resource.Delete;
            this.buttonMethodParameterRemove.Location = new System.Drawing.Point(211, 0);
            this.buttonMethodParameterRemove.Name = "buttonMethodParameterRemove";
            this.buttonMethodParameterRemove.Size = new System.Drawing.Size(20, 16);
            this.buttonMethodParameterRemove.TabIndex = 6;
            this.toolTip.SetToolTip(this.buttonMethodParameterRemove, "Remove selected parameter");
            this.buttonMethodParameterRemove.UseVisualStyleBackColor = true;
            this.buttonMethodParameterRemove.Click += new System.EventHandler(this.buttonMethodParameterRemove_Click);
            // 
            // buttonMethodParameterAddMissing
            // 
            this.buttonMethodParameterAddMissing.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonMethodParameterAddMissing.FlatAppearance.BorderSize = 0;
            this.buttonMethodParameterAddMissing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMethodParameterAddMissing.Image = global::DiversityCollection.Resource.Add1;
            this.buttonMethodParameterAddMissing.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonMethodParameterAddMissing.Location = new System.Drawing.Point(231, 0);
            this.buttonMethodParameterAddMissing.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.buttonMethodParameterAddMissing.Name = "buttonMethodParameterAddMissing";
            this.buttonMethodParameterAddMissing.Size = new System.Drawing.Size(20, 16);
            this.buttonMethodParameterAddMissing.TabIndex = 5;
            this.buttonMethodParameterAddMissing.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonMethodParameterAddMissing, "add missing parameter");
            this.buttonMethodParameterAddMissing.UseVisualStyleBackColor = true;
            this.buttonMethodParameterAddMissing.Click += new System.EventHandler(this.buttonMethodParameterAddMissing_Click);
            // 
            // pictureBoxMethodParameterValue
            // 
            this.pictureBoxMethodParameterValue.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxMethodParameterValue.Image = global::DiversityCollection.Resource.Parameter;
            this.pictureBoxMethodParameterValue.Location = new System.Drawing.Point(84, 0);
            this.pictureBoxMethodParameterValue.Name = "pictureBoxMethodParameterValue";
            this.pictureBoxMethodParameterValue.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxMethodParameterValue.TabIndex = 4;
            this.pictureBoxMethodParameterValue.TabStop = false;
            // 
            // labelMethodParameterValue
            // 
            this.labelMethodParameterValue.AccessibleName = "IdentificationUnitAnalysisMethodParameter.Value";
            this.labelMethodParameterValue.AutoSize = true;
            this.labelMethodParameterValue.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelMethodParameterValue.Location = new System.Drawing.Point(0, 0);
            this.labelMethodParameterValue.Name = "labelMethodParameterValue";
            this.labelMethodParameterValue.Size = new System.Drawing.Size(84, 13);
            this.labelMethodParameterValue.TabIndex = 3;
            this.labelMethodParameterValue.Text = "Parameter value";
            this.labelMethodParameterValue.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // panelMethodParameter
            // 
            this.panelMethodParameter.Controls.Add(this.comboBoxMethodParameterValue);
            this.panelMethodParameter.Controls.Add(this.textBoxMethodParameterValue);
            this.panelMethodParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMethodParameter.Location = new System.Drawing.Point(275, 69);
            this.panelMethodParameter.Name = "panelMethodParameter";
            this.tableLayoutPanelMethod.SetRowSpan(this.panelMethodParameter, 2);
            this.panelMethodParameter.Size = new System.Drawing.Size(245, 59);
            this.panelMethodParameter.TabIndex = 7;
            // 
            // comboBoxMethodParameterValue
            // 
            this.comboBoxMethodParameterValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMethodParameterValue.FormattingEnabled = true;
            this.comboBoxMethodParameterValue.Location = new System.Drawing.Point(81, 37);
            this.comboBoxMethodParameterValue.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxMethodParameterValue.Name = "comboBoxMethodParameterValue";
            this.comboBoxMethodParameterValue.Size = new System.Drawing.Size(85, 21);
            this.comboBoxMethodParameterValue.TabIndex = 5;
            this.comboBoxMethodParameterValue.Visible = false;
            // 
            // textBoxMethodParameterValue
            // 
            this.textBoxMethodParameterValue.Location = new System.Drawing.Point(3, 18);
            this.textBoxMethodParameterValue.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.textBoxMethodParameterValue.Multiline = true;
            this.textBoxMethodParameterValue.Name = "textBoxMethodParameterValue";
            this.textBoxMethodParameterValue.Size = new System.Drawing.Size(49, 40);
            this.textBoxMethodParameterValue.TabIndex = 4;
            this.textBoxMethodParameterValue.Visible = false;
            // 
            // pictureBoxMethod
            // 
            this.pictureBoxMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxMethod.Image = global::DiversityCollection.Resource.Tools;
            this.pictureBoxMethod.Location = new System.Drawing.Point(513, 0);
            this.pictureBoxMethod.Name = "pictureBoxMethod";
            this.pictureBoxMethod.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxMethod.TabIndex = 1;
            this.pictureBoxMethod.TabStop = false;
            // 
            // UserControl_Method
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBoxMethod);
            this.Controls.Add(this.groupBoxMethod);
            this.Name = "UserControl_Method";
            this.Size = new System.Drawing.Size(529, 150);
            this.groupBoxMethod.ResumeLayout(false);
            this.tableLayoutPanelMethod.ResumeLayout(false);
            this.tableLayoutPanelMethod.PerformLayout();
            this.toolStripMethod.ResumeLayout(false);
            this.toolStripMethod.PerformLayout();
            this.panelMethodParameterLabel.ResumeLayout(false);
            this.panelMethodParameterLabel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMethodParameterValue)).EndInit();
            this.panelMethodParameter.ResumeLayout(false);
            this.panelMethodParameter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMethod)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMethod;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMethod;
        private System.Windows.Forms.ListBox listBoxMethod;
        private System.Windows.Forms.ToolStrip toolStripMethod;
        private System.Windows.Forms.ToolStripButton toolStripButtonMethodAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonMethodDelete;
        private System.Windows.Forms.ToolStripLabel toolStripLabelMethodMarker;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxMethodMarker;
        private System.Windows.Forms.ListBox listBoxMethodParameter;
        private System.Windows.Forms.Panel panelMethodParameterLabel;
        private System.Windows.Forms.Button buttonMethodParameterRemove;
        private System.Windows.Forms.Button buttonMethodParameterAddMissing;
        private System.Windows.Forms.PictureBox pictureBoxMethodParameterValue;
        private System.Windows.Forms.Label labelMethodParameterValue;
        private System.Windows.Forms.Panel panelMethodParameter;
        private System.Windows.Forms.ComboBox comboBoxMethodParameterValue;
        private System.Windows.Forms.TextBox textBoxMethodParameterValue;
        private System.Windows.Forms.PictureBox pictureBoxMethod;
    }
}
