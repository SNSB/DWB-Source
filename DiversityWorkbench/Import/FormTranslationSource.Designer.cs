namespace DiversityWorkbench.Import
{
    partial class FormTranslationSource
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTranslationSource));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.checkBoxFirstLine = new System.Windows.Forms.CheckBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.comboBoxEncoding = new System.Windows.Forms.ComboBox();
            this.labelEncoding = new System.Windows.Forms.Label();
            this.labelFile = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelDataTable = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxFromColumn = new System.Windows.Forms.ComboBox();
            this.comboBoxIntoColumn = new System.Windows.Forms.ComboBox();
            this.labelSourceTable = new System.Windows.Forms.Label();
            this.labelFromColumn = new System.Windows.Forms.Label();
            this.labelIntoColumn = new System.Windows.Forms.Label();
            this.dataGridViewSourceTable = new System.Windows.Forms.DataGridView();
            this.labelSourceTableHeader = new System.Windows.Forms.Label();
            this.comboBoxSourceTable = new System.Windows.Forms.ComboBox();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tableLayoutPanelDataTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSourceTable)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.checkBoxFirstLine, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.dataGridView, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.buttonOpenFile, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.comboBoxEncoding, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.labelEncoding, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.labelFile, 0, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(323, 300);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.tableLayoutPanel.SetRowSpan(this.labelHeader, 2);
            this.labelHeader.Size = new System.Drawing.Size(122, 39);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "Please open the file containing the translations";
            // 
            // checkBoxFirstLine
            // 
            this.checkBoxFirstLine.AutoSize = true;
            this.checkBoxFirstLine.Checked = true;
            this.checkBoxFirstLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFirstLine.Location = new System.Drawing.Point(131, 3);
            this.checkBoxFirstLine.Name = "checkBoxFirstLine";
            this.checkBoxFirstLine.Size = new System.Drawing.Size(189, 17);
            this.checkBoxFirstLine.TabIndex = 1;
            this.checkBoxFirstLine.Text = "First line contains column definition";
            this.checkBoxFirstLine.UseVisualStyleBackColor = true;
            this.checkBoxFirstLine.Click += new System.EventHandler(this.checkBoxFirstLine_Click);
            // 
            // dataGridView
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel.SetColumnSpan(this.dataGridView, 2);
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(3, 84);
            this.dataGridView.Name = "dataGridView";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView.Size = new System.Drawing.Size(317, 216);
            this.dataGridView.TabIndex = 2;
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonOpenFile.Image = global::DiversityWorkbench.Properties.Resources.OpenFolder;
            this.buttonOpenFile.Location = new System.Drawing.Point(3, 42);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(24, 23);
            this.buttonOpenFile.TabIndex = 3;
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // comboBoxEncoding
            // 
            this.comboBoxEncoding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxEncoding.FormattingEnabled = true;
            this.comboBoxEncoding.Location = new System.Drawing.Point(131, 42);
            this.comboBoxEncoding.Name = "comboBoxEncoding";
            this.comboBoxEncoding.Size = new System.Drawing.Size(189, 21);
            this.comboBoxEncoding.TabIndex = 4;
            this.comboBoxEncoding.SelectionChangeCommitted += new System.EventHandler(this.comboBoxEncoding_SelectionChangeCommitted);
            // 
            // labelEncoding
            // 
            this.labelEncoding.AutoSize = true;
            this.labelEncoding.Location = new System.Drawing.Point(131, 23);
            this.labelEncoding.Name = "labelEncoding";
            this.labelEncoding.Size = new System.Drawing.Size(52, 13);
            this.labelEncoding.TabIndex = 5;
            this.labelEncoding.Text = "Encoding";
            // 
            // labelFile
            // 
            this.labelFile.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelFile, 2);
            this.labelFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFile.Location = new System.Drawing.Point(3, 68);
            this.labelFile.Name = "labelFile";
            this.labelFile.Size = new System.Drawing.Size(317, 13);
            this.labelFile.TabIndex = 6;
            this.labelFile.Tag = " ";
            this.labelFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tableLayoutPanel);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tableLayoutPanelDataTable);
            this.splitContainer.Panel2Collapsed = true;
            this.splitContainer.Size = new System.Drawing.Size(323, 300);
            this.splitContainer.SplitterDistance = 107;
            this.splitContainer.TabIndex = 2;
            // 
            // tableLayoutPanelDataTable
            // 
            this.tableLayoutPanelDataTable.ColumnCount = 2;
            this.tableLayoutPanelDataTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDataTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDataTable.Controls.Add(this.comboBoxFromColumn, 0, 4);
            this.tableLayoutPanelDataTable.Controls.Add(this.comboBoxIntoColumn, 1, 4);
            this.tableLayoutPanelDataTable.Controls.Add(this.labelSourceTable, 0, 1);
            this.tableLayoutPanelDataTable.Controls.Add(this.labelFromColumn, 0, 3);
            this.tableLayoutPanelDataTable.Controls.Add(this.labelIntoColumn, 1, 3);
            this.tableLayoutPanelDataTable.Controls.Add(this.dataGridViewSourceTable, 0, 5);
            this.tableLayoutPanelDataTable.Controls.Add(this.labelSourceTableHeader, 0, 0);
            this.tableLayoutPanelDataTable.Controls.Add(this.comboBoxSourceTable, 0, 2);
            this.tableLayoutPanelDataTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDataTable.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDataTable.Name = "tableLayoutPanelDataTable";
            this.tableLayoutPanelDataTable.RowCount = 6;
            this.tableLayoutPanelDataTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDataTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDataTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDataTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDataTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDataTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDataTable.Size = new System.Drawing.Size(96, 100);
            this.tableLayoutPanelDataTable.TabIndex = 0;
            // 
            // comboBoxFromColumn
            // 
            this.comboBoxFromColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxFromColumn.FormattingEnabled = true;
            this.comboBoxFromColumn.Location = new System.Drawing.Point(3, 114);
            this.comboBoxFromColumn.Name = "comboBoxFromColumn";
            this.comboBoxFromColumn.Size = new System.Drawing.Size(42, 21);
            this.comboBoxFromColumn.TabIndex = 1;
            this.comboBoxFromColumn.SelectionChangeCommitted += new System.EventHandler(this.comboBoxFromColumn_SelectionChangeCommitted);
            // 
            // comboBoxIntoColumn
            // 
            this.comboBoxIntoColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxIntoColumn.FormattingEnabled = true;
            this.comboBoxIntoColumn.Location = new System.Drawing.Point(51, 114);
            this.comboBoxIntoColumn.Name = "comboBoxIntoColumn";
            this.comboBoxIntoColumn.Size = new System.Drawing.Size(42, 21);
            this.comboBoxIntoColumn.TabIndex = 2;
            this.comboBoxIntoColumn.SelectionChangeCommitted += new System.EventHandler(this.comboBoxIntoColumn_SelectionChangeCommitted);
            // 
            // labelSourceTable
            // 
            this.labelSourceTable.AutoSize = true;
            this.labelSourceTable.Location = new System.Drawing.Point(3, 45);
            this.labelSourceTable.Name = "labelSourceTable";
            this.labelSourceTable.Size = new System.Drawing.Size(41, 26);
            this.labelSourceTable.TabIndex = 3;
            this.labelSourceTable.Text = "Sourcetable:";
            // 
            // labelFromColumn
            // 
            this.labelFromColumn.AutoSize = true;
            this.labelFromColumn.Location = new System.Drawing.Point(3, 98);
            this.labelFromColumn.Name = "labelFromColumn";
            this.labelFromColumn.Size = new System.Drawing.Size(33, 13);
            this.labelFromColumn.TabIndex = 4;
            this.labelFromColumn.Text = "From:";
            // 
            // labelIntoColumn
            // 
            this.labelIntoColumn.AutoSize = true;
            this.labelIntoColumn.Location = new System.Drawing.Point(51, 98);
            this.labelIntoColumn.Name = "labelIntoColumn";
            this.labelIntoColumn.Size = new System.Drawing.Size(28, 13);
            this.labelIntoColumn.TabIndex = 5;
            this.labelIntoColumn.Text = "Into:";
            // 
            // dataGridViewSourceTable
            // 
            this.dataGridViewSourceTable.AllowUserToAddRows = false;
            this.dataGridViewSourceTable.AllowUserToDeleteRows = false;
            this.dataGridViewSourceTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewSourceTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewSourceTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanelDataTable.SetColumnSpan(this.dataGridViewSourceTable, 2);
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewSourceTable.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewSourceTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSourceTable.Location = new System.Drawing.Point(3, 141);
            this.dataGridViewSourceTable.Name = "dataGridViewSourceTable";
            this.dataGridViewSourceTable.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewSourceTable.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewSourceTable.RowHeadersVisible = false;
            this.dataGridViewSourceTable.Size = new System.Drawing.Size(90, 1);
            this.dataGridViewSourceTable.TabIndex = 6;
            // 
            // labelSourceTableHeader
            // 
            this.labelSourceTableHeader.AutoSize = true;
            this.tableLayoutPanelDataTable.SetColumnSpan(this.labelSourceTableHeader, 2);
            this.labelSourceTableHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSourceTableHeader.Location = new System.Drawing.Point(3, 3);
            this.labelSourceTableHeader.Margin = new System.Windows.Forms.Padding(3);
            this.labelSourceTableHeader.Name = "labelSourceTableHeader";
            this.labelSourceTableHeader.Size = new System.Drawing.Size(90, 39);
            this.labelSourceTableHeader.TabIndex = 7;
            this.labelSourceTableHeader.Text = "Select a table for translation from the list";
            // 
            // comboBoxSourceTable
            // 
            this.tableLayoutPanelDataTable.SetColumnSpan(this.comboBoxSourceTable, 2);
            this.comboBoxSourceTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxSourceTable.FormattingEnabled = true;
            this.comboBoxSourceTable.Location = new System.Drawing.Point(3, 74);
            this.comboBoxSourceTable.Name = "comboBoxSourceTable";
            this.comboBoxSourceTable.Size = new System.Drawing.Size(90, 21);
            this.comboBoxSourceTable.TabIndex = 0;
            this.comboBoxSourceTable.DropDown += new System.EventHandler(this.comboBoxSourceTable_DropDown);
            this.comboBoxSourceTable.SelectionChangeCommitted += new System.EventHandler(this.comboBoxSourceTable_SelectionChangeCommitted);
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 300);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(323, 27);
            this.userControlDialogPanel.TabIndex = 1;
            // 
            // FormTranslationSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 327);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormTranslationSource";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Translation source";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tableLayoutPanelDataTable.ResumeLayout(false);
            this.tableLayoutPanelDataTable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSourceTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.CheckBox checkBoxFirstLine;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.ComboBox comboBoxEncoding;
        private System.Windows.Forms.Label labelEncoding;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label labelFile;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDataTable;
        private System.Windows.Forms.ComboBox comboBoxSourceTable;
        private System.Windows.Forms.ComboBox comboBoxFromColumn;
        private System.Windows.Forms.ComboBox comboBoxIntoColumn;
        private System.Windows.Forms.Label labelSourceTable;
        private System.Windows.Forms.Label labelFromColumn;
        private System.Windows.Forms.Label labelIntoColumn;
        private System.Windows.Forms.DataGridView dataGridViewSourceTable;
        private System.Windows.Forms.Label labelSourceTableHeader;
    }
}