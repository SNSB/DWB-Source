namespace DiversityCollection.UserControls
{
    partial class UserControl_Label
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_Label));
            this.groupBoxLabel = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelLabelDetails = new System.Windows.Forms.TableLayoutPanel();
            this.labelLabelNotes = new System.Windows.Forms.Label();
            this.textBoxLabelNotes = new System.Windows.Forms.TextBox();
            this.labelLabelType = new System.Windows.Forms.Label();
            this.comboBoxLabelType = new System.Windows.Forms.ComboBox();
            this.labelLabelTranscriptionState = new System.Windows.Forms.Label();
            this.comboBoxLabelTranscriptionState = new System.Windows.Forms.ComboBox();
            this.labelLabelTitle = new System.Windows.Forms.Label();
            this.buttonLabelTitle = new System.Windows.Forms.Button();
            this.panelLabelTitle = new System.Windows.Forms.Panel();
            this.textBoxLabelTitle = new System.Windows.Forms.TextBox();
            this.comboBoxLabelTitle = new System.Windows.Forms.ComboBox();
            this.groupBoxLabel.SuspendLayout();
            this.tableLayoutPanelLabelDetails.SuspendLayout();
            this.panelLabelTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxLabel
            // 
            this.groupBoxLabel.AccessibleName = "CollectionSpecimen.Label";
            this.groupBoxLabel.Controls.Add(this.tableLayoutPanelLabelDetails);
            this.groupBoxLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxLabel.ForeColor = System.Drawing.Color.Black;
            this.groupBoxLabel.Location = new System.Drawing.Point(0, 0);
            this.groupBoxLabel.Name = "groupBoxLabel";
            this.groupBoxLabel.Padding = new System.Windows.Forms.Padding(1);
            this.groupBoxLabel.Size = new System.Drawing.Size(467, 213);
            this.groupBoxLabel.TabIndex = 15;
            this.groupBoxLabel.TabStop = false;
            this.groupBoxLabel.Text = "Label";
            // 
            // tableLayoutPanelLabelDetails
            // 
            this.tableLayoutPanelLabelDetails.ColumnCount = 5;
            this.tableLayoutPanelLabelDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLabelDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLabelDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelLabelDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLabelDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelLabelDetails.Controls.Add(this.labelLabelNotes, 0, 2);
            this.tableLayoutPanelLabelDetails.Controls.Add(this.textBoxLabelNotes, 2, 2);
            this.tableLayoutPanelLabelDetails.Controls.Add(this.labelLabelType, 3, 1);
            this.tableLayoutPanelLabelDetails.Controls.Add(this.comboBoxLabelType, 4, 1);
            this.tableLayoutPanelLabelDetails.Controls.Add(this.labelLabelTranscriptionState, 0, 1);
            this.tableLayoutPanelLabelDetails.Controls.Add(this.comboBoxLabelTranscriptionState, 2, 1);
            this.tableLayoutPanelLabelDetails.Controls.Add(this.labelLabelTitle, 0, 0);
            this.tableLayoutPanelLabelDetails.Controls.Add(this.buttonLabelTitle, 1, 0);
            this.tableLayoutPanelLabelDetails.Controls.Add(this.panelLabelTitle, 2, 0);
            this.tableLayoutPanelLabelDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelLabelDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelLabelDetails.Location = new System.Drawing.Point(1, 14);
            this.tableLayoutPanelLabelDetails.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelLabelDetails.Name = "tableLayoutPanelLabelDetails";
            this.tableLayoutPanelLabelDetails.RowCount = 3;
            this.tableLayoutPanelLabelDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLabelDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLabelDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLabelDetails.Size = new System.Drawing.Size(465, 198);
            this.tableLayoutPanelLabelDetails.TabIndex = 22;
            // 
            // labelLabelNotes
            // 
            this.labelLabelNotes.AccessibleName = "CollectionSpecimen.LabelTranscriptionNotes";
            this.tableLayoutPanelLabelDetails.SetColumnSpan(this.labelLabelNotes, 2);
            this.labelLabelNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLabelNotes.Location = new System.Drawing.Point(3, 130);
            this.labelLabelNotes.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelLabelNotes.Name = "labelLabelNotes";
            this.labelLabelNotes.Size = new System.Drawing.Size(61, 68);
            this.labelLabelNotes.TabIndex = 12;
            this.labelLabelNotes.Text = "Notes:";
            this.labelLabelNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxLabelNotes
            // 
            this.tableLayoutPanelLabelDetails.SetColumnSpan(this.textBoxLabelNotes, 3);
            this.textBoxLabelNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLabelNotes.Location = new System.Drawing.Point(64, 127);
            this.textBoxLabelNotes.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxLabelNotes.Multiline = true;
            this.textBoxLabelNotes.Name = "textBoxLabelNotes";
            this.textBoxLabelNotes.Size = new System.Drawing.Size(398, 68);
            this.textBoxLabelNotes.TabIndex = 13;
            // 
            // labelLabelType
            // 
            this.labelLabelType.AccessibleName = "CollectionSpecimen.LabelType";
            this.labelLabelType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLabelType.Location = new System.Drawing.Point(246, 100);
            this.labelLabelType.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelLabelType.Name = "labelLabelType";
            this.labelLabelType.Size = new System.Drawing.Size(40, 27);
            this.labelLabelType.TabIndex = 14;
            this.labelLabelType.Text = "Type:";
            this.labelLabelType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxLabelType
            // 
            this.comboBoxLabelType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxLabelType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLabelType.Location = new System.Drawing.Point(286, 103);
            this.comboBoxLabelType.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxLabelType.MaxDropDownItems = 20;
            this.comboBoxLabelType.Name = "comboBoxLabelType";
            this.comboBoxLabelType.Size = new System.Drawing.Size(176, 21);
            this.comboBoxLabelType.TabIndex = 8;
            // 
            // labelLabelTranscriptionState
            // 
            this.labelLabelTranscriptionState.AccessibleName = "CollectionSpecimen.LabelTranscriptionState";
            this.tableLayoutPanelLabelDetails.SetColumnSpan(this.labelLabelTranscriptionState, 2);
            this.labelLabelTranscriptionState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLabelTranscriptionState.Location = new System.Drawing.Point(0, 100);
            this.labelLabelTranscriptionState.Margin = new System.Windows.Forms.Padding(0);
            this.labelLabelTranscriptionState.Name = "labelLabelTranscriptionState";
            this.labelLabelTranscriptionState.Size = new System.Drawing.Size(64, 27);
            this.labelLabelTranscriptionState.TabIndex = 16;
            this.labelLabelTranscriptionState.Text = "Transcript.:";
            this.labelLabelTranscriptionState.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxLabelTranscriptionState
            // 
            this.comboBoxLabelTranscriptionState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxLabelTranscriptionState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLabelTranscriptionState.Location = new System.Drawing.Point(64, 103);
            this.comboBoxLabelTranscriptionState.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxLabelTranscriptionState.MaxDropDownItems = 20;
            this.comboBoxLabelTranscriptionState.Name = "comboBoxLabelTranscriptionState";
            this.comboBoxLabelTranscriptionState.Size = new System.Drawing.Size(176, 21);
            this.comboBoxLabelTranscriptionState.TabIndex = 15;
            // 
            // labelLabelTitle
            // 
            this.labelLabelTitle.AccessibleName = "CollectionSpecimen.LabelTitle";
            this.labelLabelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelLabelTitle.Location = new System.Drawing.Point(0, 6);
            this.labelLabelTitle.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.labelLabelTitle.Name = "labelLabelTitle";
            this.labelLabelTitle.Size = new System.Drawing.Size(46, 13);
            this.labelLabelTitle.TabIndex = 11;
            this.labelLabelTitle.Text = "Title";
            this.labelLabelTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonLabelTitle
            // 
            this.buttonLabelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonLabelTitle.FlatAppearance.BorderSize = 0;
            this.buttonLabelTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLabelTitle.Image = global::DiversityCollection.Resource.List;
            this.buttonLabelTitle.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonLabelTitle.Location = new System.Drawing.Point(46, 0);
            this.buttonLabelTitle.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.buttonLabelTitle.Name = "buttonLabelTitle";
            this.buttonLabelTitle.Size = new System.Drawing.Size(16, 20);
            this.buttonLabelTitle.TabIndex = 18;
            this.toolTip.SetToolTip(this.buttonLabelTitle, "Switch to text");
            this.buttonLabelTitle.UseVisualStyleBackColor = true;
            this.buttonLabelTitle.Click += new System.EventHandler(this.buttonLabelTitle_Click);
            // 
            // panelLabelTitle
            // 
            this.tableLayoutPanelLabelDetails.SetColumnSpan(this.panelLabelTitle, 3);
            this.panelLabelTitle.Controls.Add(this.textBoxLabelTitle);
            this.panelLabelTitle.Controls.Add(this.comboBoxLabelTitle);
            this.panelLabelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLabelTitle.Location = new System.Drawing.Point(64, 0);
            this.panelLabelTitle.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.panelLabelTitle.Name = "panelLabelTitle";
            this.panelLabelTitle.Size = new System.Drawing.Size(398, 100);
            this.panelLabelTitle.TabIndex = 19;
            // 
            // textBoxLabelTitle
            // 
            this.textBoxLabelTitle.Location = new System.Drawing.Point(18, 57);
            this.textBoxLabelTitle.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.textBoxLabelTitle.Multiline = true;
            this.textBoxLabelTitle.Name = "textBoxLabelTitle";
            this.textBoxLabelTitle.Size = new System.Drawing.Size(123, 23);
            this.textBoxLabelTitle.TabIndex = 17;
            this.textBoxLabelTitle.Visible = false;
            // 
            // comboBoxLabelTitle
            // 
            this.comboBoxLabelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBoxLabelTitle.FormattingEnabled = true;
            this.comboBoxLabelTitle.Location = new System.Drawing.Point(0, 0);
            this.comboBoxLabelTitle.Margin = new System.Windows.Forms.Padding(0, 1, 3, 0);
            this.comboBoxLabelTitle.Name = "comboBoxLabelTitle";
            this.comboBoxLabelTitle.Size = new System.Drawing.Size(398, 21);
            this.comboBoxLabelTitle.TabIndex = 12;
            this.comboBoxLabelTitle.DropDown += new System.EventHandler(this.comboBoxLabelTitle_DropDown);
            // 
            // UserControl_Label
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxLabel);
            this.Name = "UserControl_Label";
            this.Size = new System.Drawing.Size(467, 213);
            this.Resize += new System.EventHandler(this.UserControl_Label_Resize);
            this.groupBoxLabel.ResumeLayout(false);
            this.tableLayoutPanelLabelDetails.ResumeLayout(false);
            this.tableLayoutPanelLabelDetails.PerformLayout();
            this.panelLabelTitle.ResumeLayout(false);
            this.panelLabelTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLabelDetails;
        private System.Windows.Forms.Label labelLabelNotes;
        private System.Windows.Forms.TextBox textBoxLabelNotes;
        private System.Windows.Forms.Label labelLabelType;
        private System.Windows.Forms.ComboBox comboBoxLabelType;
        private System.Windows.Forms.Label labelLabelTranscriptionState;
        private System.Windows.Forms.ComboBox comboBoxLabelTranscriptionState;
        private System.Windows.Forms.Label labelLabelTitle;
        private System.Windows.Forms.ComboBox comboBoxLabelTitle;
        private System.Windows.Forms.TextBox textBoxLabelTitle;
        private System.Windows.Forms.Button buttonLabelTitle;
        private System.Windows.Forms.Panel panelLabelTitle;
    }
}
