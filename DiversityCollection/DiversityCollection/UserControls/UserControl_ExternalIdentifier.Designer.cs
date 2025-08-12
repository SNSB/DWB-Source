namespace DiversityCollection.UserControls
{
    partial class UserControl_ExternalIdentifier
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_ExternalIdentifier));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.labelType = new System.Windows.Forms.Label();
            this.textBoxID = new System.Windows.Forms.TextBox();
            this.buttonSetURL = new System.Windows.Forms.Button();
            this.textBoxURL = new System.Windows.Forms.TextBox();
            this.labelNotes = new System.Windows.Forms.Label();
            this.textBoxNotes = new System.Windows.Forms.TextBox();
            this.labelURL = new System.Windows.Forms.Label();
            this.pictureBoxID = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxID)).BeginInit();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Controls.Add(this.comboBoxType, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelType, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxID, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonSetURL, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.textBoxURL, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.labelNotes, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.textBoxNotes, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.labelURL, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxID, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 6;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(606, 290);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // comboBoxType
            // 
            this.tableLayoutPanel.SetColumnSpan(this.comboBoxType, 2);
            this.comboBoxType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(41, 3);
            this.comboBoxType.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(562, 21);
            this.comboBoxType.TabIndex = 0;
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelType.Location = new System.Drawing.Point(3, 0);
            this.labelType.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(38, 27);
            this.labelType.TabIndex = 1;
            this.labelType.Text = "Type:";
            this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxID
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textBoxID, 2);
            this.textBoxID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxID.Location = new System.Drawing.Point(41, 27);
            this.textBoxID.Margin = new System.Windows.Forms.Padding(0, 0, 3, 2);
            this.textBoxID.Name = "textBoxID";
            this.textBoxID.Size = new System.Drawing.Size(562, 20);
            this.textBoxID.TabIndex = 2;
            // 
            // buttonSetURL
            // 
            this.buttonSetURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSetURL.Image = global::DiversityCollection.Resource.Browse;
            this.buttonSetURL.Location = new System.Drawing.Point(579, 49);
            this.buttonSetURL.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.buttonSetURL.Name = "buttonSetURL";
            this.buttonSetURL.Size = new System.Drawing.Size(24, 24);
            this.buttonSetURL.TabIndex = 4;
            this.buttonSetURL.UseVisualStyleBackColor = true;
            this.buttonSetURL.Click += new System.EventHandler(this.buttonSetURL_Click);
            // 
            // textBoxURL
            // 
            this.textBoxURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxURL.Location = new System.Drawing.Point(41, 50);
            this.textBoxURL.Margin = new System.Windows.Forms.Padding(0, 1, 3, 3);
            this.textBoxURL.Name = "textBoxURL";
            this.textBoxURL.Size = new System.Drawing.Size(535, 20);
            this.textBoxURL.TabIndex = 5;
            // 
            // labelNotes
            // 
            this.labelNotes.AutoSize = true;
            this.labelNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNotes.Location = new System.Drawing.Point(3, 76);
            this.labelNotes.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelNotes.Name = "labelNotes";
            this.labelNotes.Size = new System.Drawing.Size(38, 13);
            this.labelNotes.TabIndex = 6;
            this.labelNotes.Text = "Notes:";
            this.labelNotes.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxNotes
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textBoxNotes, 2);
            this.textBoxNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNotes.Location = new System.Drawing.Point(41, 73);
            this.textBoxNotes.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxNotes.Multiline = true;
            this.textBoxNotes.Name = "textBoxNotes";
            this.tableLayoutPanel.SetRowSpan(this.textBoxNotes, 2);
            this.textBoxNotes.Size = new System.Drawing.Size(562, 214);
            this.textBoxNotes.TabIndex = 7;
            // 
            // labelURL
            // 
            this.labelURL.AutoSize = true;
            this.labelURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelURL.Location = new System.Drawing.Point(3, 49);
            this.labelURL.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelURL.Name = "labelURL";
            this.labelURL.Size = new System.Drawing.Size(38, 24);
            this.labelURL.TabIndex = 8;
            this.labelURL.Text = "URL:";
            this.labelURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBoxID
            // 
            this.pictureBoxID.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBoxID.Image = global::DiversityCollection.Resource.Identifier;
            this.pictureBoxID.InitialImage = global::DiversityCollection.Resource.Identifier;
            this.pictureBoxID.Location = new System.Drawing.Point(23, 29);
            this.pictureBoxID.Margin = new System.Windows.Forms.Padding(3, 2, 2, 3);
            this.pictureBoxID.Name = "pictureBoxID";
            this.pictureBoxID.Size = new System.Drawing.Size(16, 17);
            this.pictureBoxID.TabIndex = 9;
            this.pictureBoxID.TabStop = false;
            // 
            // UserControl_ExternalIdentifier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControl_ExternalIdentifier";
            this.Size = new System.Drawing.Size(606, 290);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.TextBox textBoxID;
        private System.Windows.Forms.Button buttonSetURL;
        private System.Windows.Forms.TextBox textBoxURL;
        private System.Windows.Forms.Label labelNotes;
        private System.Windows.Forms.TextBox textBoxNotes;
        private System.Windows.Forms.Label labelURL;
        private System.Windows.Forms.PictureBox pictureBoxID;
    }
}
