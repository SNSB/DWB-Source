namespace DiversityCollection.Tasks
{
    partial class FormIPMobile
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIPMobile));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            this.ColumnGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTaxon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnImage1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColumnResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pictureBoxScanner = new System.Windows.Forms.PictureBox();
            this.textBoxScanner = new System.Windows.Forms.TextBox();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.buttonImage = new System.Windows.Forms.Button();
            this.labelCollectionTask = new System.Windows.Forms.Label();
            this.labelDate = new System.Windows.Forms.Label();
            this.buttonSelectedObject = new System.Windows.Forms.Button();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxScanner)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 4;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.dataGridViewResults, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.pictureBoxScanner, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxScanner, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonSettings, 3, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonImage, 3, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelCollectionTask, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelDate, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonSelectedObject, 3, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 4;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(333, 450);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.AllowUserToAddRows = false;
            this.dataGridViewResults.AllowUserToDeleteRows = false;
            this.dataGridViewResults.BackgroundColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlDark;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewResults.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnGroup,
            this.ColumnTaxon,
            this.ColumnImage1,
            this.ColumnResult});
            this.tableLayoutPanelMain.SetColumnSpan(this.dataGridViewResults, 4);
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewResults.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewResults.GridColor = System.Drawing.Color.Black;
            this.dataGridViewResults.Location = new System.Drawing.Point(3, 72);
            this.dataGridViewResults.Name = "dataGridViewResults";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewResults.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewResults.RowHeadersVisible = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            this.dataGridViewResults.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewResults.Size = new System.Drawing.Size(327, 375);
            this.dataGridViewResults.TabIndex = 2;
            // 
            // ColumnGroup
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            this.ColumnGroup.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColumnGroup.HeaderText = "Group";
            this.ColumnGroup.Name = "ColumnGroup";
            this.ColumnGroup.ReadOnly = true;
            this.ColumnGroup.Width = 50;
            // 
            // ColumnTaxon
            // 
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Blue;
            this.ColumnTaxon.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColumnTaxon.FillWeight = 110F;
            this.ColumnTaxon.HeaderText = "Taxon";
            this.ColumnTaxon.Name = "ColumnTaxon";
            this.ColumnTaxon.ReadOnly = true;
            this.ColumnTaxon.Width = 110;
            // 
            // ColumnImage1
            // 
            this.ColumnImage1.HeaderText = "";
            this.ColumnImage1.Name = "ColumnImage1";
            this.ColumnImage1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnImage1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ColumnResult
            // 
            this.ColumnResult.FillWeight = 46F;
            this.ColumnResult.HeaderText = "Count";
            this.ColumnResult.Name = "ColumnResult";
            this.ColumnResult.Width = 46;
            // 
            // pictureBoxScanner
            // 
            this.pictureBoxScanner.BackColor = System.Drawing.Color.Pink;
            this.pictureBoxScanner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxScanner.Image = global::DiversityCollection.Resource.ScannerBarcode;
            this.pictureBoxScanner.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxScanner.Margin = new System.Windows.Forms.Padding(3, 3, 0, 6);
            this.pictureBoxScanner.Name = "pictureBoxScanner";
            this.pictureBoxScanner.Padding = new System.Windows.Forms.Padding(1, 1, 0, 0);
            this.pictureBoxScanner.Size = new System.Drawing.Size(18, 20);
            this.pictureBoxScanner.TabIndex = 0;
            this.pictureBoxScanner.TabStop = false;
            // 
            // textBoxScanner
            // 
            this.textBoxScanner.BackColor = System.Drawing.Color.Pink;
            this.textBoxScanner.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxScanner, 2);
            this.textBoxScanner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxScanner.Location = new System.Drawing.Point(21, 3);
            this.textBoxScanner.Margin = new System.Windows.Forms.Padding(0, 3, 3, 6);
            this.textBoxScanner.Multiline = true;
            this.textBoxScanner.Name = "textBoxScanner";
            this.textBoxScanner.Size = new System.Drawing.Size(278, 20);
            this.textBoxScanner.TabIndex = 1;
            this.textBoxScanner.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxScanner.TextChanged += new System.EventHandler(this.textBoxScanner_TextChanged);
            // 
            // buttonSettings
            // 
            this.buttonSettings.FlatAppearance.BorderSize = 0;
            this.buttonSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSettings.Image = global::DiversityCollection.Resource.Settings;
            this.buttonSettings.Location = new System.Drawing.Point(305, 30);
            this.buttonSettings.Margin = new System.Windows.Forms.Padding(3, 1, 3, 0);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(22, 19);
            this.buttonSettings.TabIndex = 3;
            this.buttonSettings.UseVisualStyleBackColor = true;
            // 
            // buttonImage
            // 
            this.buttonImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonImage.FlatAppearance.BorderSize = 0;
            this.buttonImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonImage.Image = global::DiversityCollection.Resource.Camera;
            this.buttonImage.Location = new System.Drawing.Point(305, 52);
            this.buttonImage.Name = "buttonImage";
            this.buttonImage.Size = new System.Drawing.Size(25, 14);
            this.buttonImage.TabIndex = 4;
            this.buttonImage.UseVisualStyleBackColor = true;
            // 
            // labelCollectionTask
            // 
            this.labelCollectionTask.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelCollectionTask, 3);
            this.labelCollectionTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionTask.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCollectionTask.ForeColor = System.Drawing.Color.White;
            this.labelCollectionTask.Location = new System.Drawing.Point(3, 29);
            this.labelCollectionTask.Name = "labelCollectionTask";
            this.labelCollectionTask.Size = new System.Drawing.Size(296, 20);
            this.labelCollectionTask.TabIndex = 5;
            this.labelCollectionTask.Text = "label1";
            this.labelCollectionTask.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDate
            // 
            this.labelDate.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelDate, 3);
            this.labelDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDate.ForeColor = System.Drawing.Color.White;
            this.labelDate.Location = new System.Drawing.Point(3, 49);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(296, 20);
            this.labelDate.TabIndex = 6;
            this.labelDate.Text = "label1";
            this.labelDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSelectedObject
            // 
            this.buttonSelectedObject.FlatAppearance.BorderSize = 0;
            this.buttonSelectedObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSelectedObject.Image = global::DiversityCollection.Resource.Falle;
            this.buttonSelectedObject.Location = new System.Drawing.Point(305, 3);
            this.buttonSelectedObject.Name = "buttonSelectedObject";
            this.buttonSelectedObject.Size = new System.Drawing.Size(25, 23);
            this.buttonSelectedObject.TabIndex = 7;
            this.buttonSelectedObject.Text = "button1";
            this.buttonSelectedObject.UseVisualStyleBackColor = true;
            // 
            // FormIPMobile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(333, 450);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormIPMobile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "IPMobile";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxScanner)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.PictureBox pictureBoxScanner;
        private System.Windows.Forms.TextBox textBoxScanner;
        private System.Windows.Forms.DataGridView dataGridViewResults;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Button buttonImage;
        private System.Windows.Forms.Label labelCollectionTask;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTaxon;
        private System.Windows.Forms.DataGridViewImageColumn ColumnImage1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnResult;
        private System.Windows.Forms.Label labelDate;
        private System.Windows.Forms.Button buttonSelectedObject;
    }
}