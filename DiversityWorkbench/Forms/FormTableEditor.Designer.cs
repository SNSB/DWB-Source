namespace DiversityWorkbench.Forms
{
    partial class FormTableEditor
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTableEditor));
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            dataGridView = new System.Windows.Forms.DataGridView();
            contextMenuStripTable = new System.Windows.Forms.ContextMenuStrip(components);
            copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            clearSelectedCellsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            buttonReset = new System.Windows.Forms.Button();
            buttonSave = new System.Windows.Forms.Button();
            buttonMarkColumn = new System.Windows.Forms.Button();
            buttonSetColumnWidth = new System.Windows.Forms.Button();
            labelInfo = new System.Windows.Forms.Label();
            progressBar = new System.Windows.Forms.ProgressBar();
            groupBoxFilter = new System.Windows.Forms.GroupBox();
            textBoxFilter = new System.Windows.Forms.TextBox();
            comboBoxFilter = new System.Windows.Forms.ComboBox();
            buttonFilter = new System.Windows.Forms.Button();
            checkBoxFilterCaseSensitiv = new System.Windows.Forms.CheckBox();
            buttonClose = new System.Windows.Forms.Button();
            groupBoxEdit = new System.Windows.Forms.GroupBox();
            tableLayoutPanelEdit = new System.Windows.Forms.TableLayoutPanel();
            comboBoxAction = new System.Windows.Forms.ComboBox();
            labelEditReplace = new System.Windows.Forms.Label();
            buttonEdit = new System.Windows.Forms.Button();
            panelEditInsert = new System.Windows.Forms.Panel();
            comboBoxEditInsert = new System.Windows.Forms.ComboBox();
            textBoxEditInsert = new System.Windows.Forms.TextBox();
            contextMenuStripEditInsert = new System.Windows.Forms.ContextMenuStrip(components);
            tabulatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            returnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            resetInsertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            panelEditReplace = new System.Windows.Forms.Panel();
            comboBoxEditReplace = new System.Windows.Forms.ComboBox();
            textBoxEditReplace = new System.Windows.Forms.TextBox();
            contextMenuStripEditReplace = new System.Windows.Forms.ContextMenuStrip(components);
            tabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            returnReplaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            groupBoxExport = new System.Windows.Forms.GroupBox();
            buttonExportSqlite = new System.Windows.Forms.Button();
            buttonOpenExportFile = new System.Windows.Forms.Button();
            buttonExport = new System.Windows.Forms.Button();
            buttonOpenLog = new System.Windows.Forms.Button();
            buttonDelete = new System.Windows.Forms.Button();
            buttonFeedback = new System.Windows.Forms.Button();
            checkBoxUseComboboxValues = new System.Windows.Forms.CheckBox();
            buttonShowRowNumber = new System.Windows.Forms.Button();
            buttonSetColumnWidthContent = new System.Windows.Forms.Button();
            toolTip = new System.Windows.Forms.ToolTip(components);
            imageListEditAction = new System.Windows.Forms.ImageList(components);
            helpProvider = new System.Windows.Forms.HelpProvider();
            tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            contextMenuStripTable.SuspendLayout();
            groupBoxFilter.SuspendLayout();
            groupBoxEdit.SuspendLayout();
            tableLayoutPanelEdit.SuspendLayout();
            panelEditInsert.SuspendLayout();
            contextMenuStripEditInsert.SuspendLayout();
            panelEditReplace.SuspendLayout();
            contextMenuStripEditReplace.SuspendLayout();
            groupBoxExport.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 17;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 89F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanel.Controls.Add(dataGridView, 0, 2);
            tableLayoutPanel.Controls.Add(buttonReset, 13, 1);
            tableLayoutPanel.Controls.Add(buttonSave, 4, 1);
            tableLayoutPanel.Controls.Add(buttonMarkColumn, 8, 1);
            tableLayoutPanel.Controls.Add(buttonSetColumnWidth, 6, 1);
            tableLayoutPanel.Controls.Add(labelInfo, 1, 3);
            tableLayoutPanel.Controls.Add(progressBar, 4, 3);
            tableLayoutPanel.Controls.Add(groupBoxFilter, 12, 0);
            tableLayoutPanel.Controls.Add(buttonClose, 5, 1);
            tableLayoutPanel.Controls.Add(groupBoxEdit, 1, 0);
            tableLayoutPanel.Controls.Add(groupBoxExport, 15, 0);
            tableLayoutPanel.Controls.Add(buttonOpenLog, 9, 1);
            tableLayoutPanel.Controls.Add(buttonDelete, 2, 1);
            tableLayoutPanel.Controls.Add(buttonFeedback, 16, 1);
            tableLayoutPanel.Controls.Add(checkBoxUseComboboxValues, 10, 1);
            tableLayoutPanel.Controls.Add(buttonShowRowNumber, 0, 1);
            tableLayoutPanel.Controls.Add(buttonSetColumnWidthContent, 7, 1);
            tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0, 3, 7, 3);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 4;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            tableLayoutPanel.Size = new System.Drawing.Size(1236, 727);
            tableLayoutPanel.TabIndex = 0;
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableLayoutPanel.SetColumnSpan(dataGridView, 17);
            dataGridView.ContextMenuStrip = contextMenuStripTable;
            dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView.Location = new System.Drawing.Point(4, 51);
            dataGridView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dataGridView.Name = "dataGridView";
            dataGridView.Size = new System.Drawing.Size(1228, 655);
            dataGridView.TabIndex = 0;
            dataGridView.CellClick += dataGridView_CellClick;
            dataGridView.CellContentClick += dataGridView_CellContentClick;
            dataGridView.DataError += dataGridView_DataError;
            dataGridView.EditingControlShowing += dataGridView_EditingControlShowing;
            dataGridView.SelectionChanged += dataGridView_SelectionChanged;
            // 
            // contextMenuStripTable
            // 
            contextMenuStripTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { copyToolStripMenuItem, pasteToolStripMenuItem, clearSelectedCellsToolStripMenuItem });
            contextMenuStripTable.Name = "contextMenuStripTable";
            contextMenuStripTable.Size = new System.Drawing.Size(174, 70);
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Image = ResourceWorkbench.Copy;
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += copyToolStripMenuItem_Click;
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.Image = ResourceWorkbench.Paste;
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            pasteToolStripMenuItem.Text = "Paste";
            pasteToolStripMenuItem.Click += pasteToolStripMenuItem_Click;
            // 
            // clearSelectedCellsToolStripMenuItem
            // 
            clearSelectedCellsToolStripMenuItem.Image = Properties.Resources.Delete;
            clearSelectedCellsToolStripMenuItem.Name = "clearSelectedCellsToolStripMenuItem";
            clearSelectedCellsToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            clearSelectedCellsToolStripMenuItem.Text = "Clear selected cells";
            clearSelectedCellsToolStripMenuItem.Click += clearSelectedCellsToolStripMenuItem_Click;
            // 
            // buttonReset
            // 
            buttonReset.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonReset.FlatAppearance.BorderSize = 0;
            buttonReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonReset.Image = Properties.Resources.Transfrom;
            buttonReset.Location = new System.Drawing.Point(1084, 16);
            buttonReset.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            buttonReset.Name = "buttonReset";
            buttonReset.Size = new System.Drawing.Size(19, 29);
            buttonReset.TabIndex = 20;
            toolTip.SetToolTip(buttonReset, "Reset filter without saving");
            buttonReset.UseVisualStyleBackColor = true;
            buttonReset.Click += buttonReset_Click;
            // 
            // buttonSave
            // 
            buttonSave.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonSave.FlatAppearance.BorderSize = 0;
            buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonSave.Image = Properties.Resources.Save;
            buttonSave.Location = new System.Drawing.Point(581, 16);
            buttonSave.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new System.Drawing.Size(21, 29);
            buttonSave.TabIndex = 21;
            toolTip.SetToolTip(buttonSave, "Save changes");
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // buttonMarkColumn
            // 
            buttonMarkColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonMarkColumn.FlatAppearance.BorderSize = 0;
            buttonMarkColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonMarkColumn.Image = Properties.Resources.MarkColumn;
            buttonMarkColumn.Location = new System.Drawing.Point(672, 16);
            buttonMarkColumn.Margin = new System.Windows.Forms.Padding(1, 3, 0, 3);
            buttonMarkColumn.Name = "buttonMarkColumn";
            buttonMarkColumn.Size = new System.Drawing.Size(21, 29);
            buttonMarkColumn.TabIndex = 22;
            toolTip.SetToolTip(buttonMarkColumn, "Mark whole column");
            buttonMarkColumn.UseVisualStyleBackColor = true;
            buttonMarkColumn.Click += buttonMarkColumn_Click;
            // 
            // buttonSetColumnWidth
            // 
            buttonSetColumnWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonSetColumnWidth.FlatAppearance.BorderSize = 0;
            buttonSetColumnWidth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonSetColumnWidth.Image = Properties.Resources.OptColumnWidth;
            buttonSetColumnWidth.Location = new System.Drawing.Point(627, 16);
            buttonSetColumnWidth.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            buttonSetColumnWidth.Name = "buttonSetColumnWidth";
            buttonSetColumnWidth.Size = new System.Drawing.Size(21, 29);
            buttonSetColumnWidth.TabIndex = 23;
            toolTip.SetToolTip(buttonSetColumnWidth, "Set column width according to content");
            buttonSetColumnWidth.UseVisualStyleBackColor = true;
            buttonSetColumnWidth.Click += buttonSetColumnWidth_Click;
            // 
            // labelInfo
            // 
            labelInfo.AutoSize = true;
            tableLayoutPanel.SetColumnSpan(labelInfo, 3);
            labelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            labelInfo.Location = new System.Drawing.Point(27, 709);
            labelInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelInfo.Name = "labelInfo";
            labelInfo.Size = new System.Drawing.Size(546, 18);
            labelInfo.TabIndex = 25;
            labelInfo.Text = "label1";
            labelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar
            // 
            tableLayoutPanel.SetColumnSpan(progressBar, 13);
            progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            progressBar.Location = new System.Drawing.Point(581, 712);
            progressBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            progressBar.Name = "progressBar";
            progressBar.Size = new System.Drawing.Size(651, 12);
            progressBar.TabIndex = 26;
            // 
            // groupBoxFilter
            // 
            groupBoxFilter.Controls.Add(textBoxFilter);
            groupBoxFilter.Controls.Add(comboBoxFilter);
            groupBoxFilter.Controls.Add(buttonFilter);
            groupBoxFilter.Controls.Add(checkBoxFilterCaseSensitiv);
            groupBoxFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxFilter.Location = new System.Drawing.Point(748, 0);
            groupBoxFilter.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            groupBoxFilter.Name = "groupBoxFilter";
            groupBoxFilter.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel.SetRowSpan(groupBoxFilter, 2);
            groupBoxFilter.Size = new System.Drawing.Size(336, 48);
            groupBoxFilter.TabIndex = 28;
            groupBoxFilter.TabStop = false;
            groupBoxFilter.Text = "Filter: ";
            // 
            // textBoxFilter
            // 
            textBoxFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxFilter.Location = new System.Drawing.Point(100, 19);
            textBoxFilter.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            textBoxFilter.Name = "textBoxFilter";
            textBoxFilter.Size = new System.Drawing.Size(218, 23);
            textBoxFilter.TabIndex = 4;
            toolTip.SetToolTip(textBoxFilter, "The text for filtering the data");
            // 
            // comboBoxFilter
            // 
            comboBoxFilter.Dock = System.Windows.Forms.DockStyle.Left;
            comboBoxFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxFilter.FormattingEnabled = true;
            comboBoxFilter.Items.AddRange(new object[] { "=", "~", "≠" });
            comboBoxFilter.Location = new System.Drawing.Point(66, 19);
            comboBoxFilter.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            comboBoxFilter.Name = "comboBoxFilter";
            comboBoxFilter.Size = new System.Drawing.Size(34, 23);
            comboBoxFilter.TabIndex = 5;
            toolTip.SetToolTip(comboBoxFilter, "The mode of comparision");
            // 
            // buttonFilter
            // 
            buttonFilter.Dock = System.Windows.Forms.DockStyle.Right;
            buttonFilter.FlatAppearance.BorderSize = 0;
            buttonFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonFilter.Image = Properties.Resources.Filter;
            buttonFilter.Location = new System.Drawing.Point(318, 19);
            buttonFilter.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            buttonFilter.Name = "buttonFilter";
            buttonFilter.Size = new System.Drawing.Size(14, 26);
            buttonFilter.TabIndex = 2;
            toolTip.SetToolTip(buttonFilter, "Filter data in the table");
            buttonFilter.UseVisualStyleBackColor = true;
            buttonFilter.Click += buttonFilter_Click;
            // 
            // checkBoxFilterCaseSensitiv
            // 
            checkBoxFilterCaseSensitiv.AutoSize = true;
            checkBoxFilterCaseSensitiv.Dock = System.Windows.Forms.DockStyle.Left;
            checkBoxFilterCaseSensitiv.Location = new System.Drawing.Point(4, 19);
            checkBoxFilterCaseSensitiv.Margin = new System.Windows.Forms.Padding(4, 7, 4, 0);
            checkBoxFilterCaseSensitiv.Name = "checkBoxFilterCaseSensitiv";
            checkBoxFilterCaseSensitiv.Size = new System.Drawing.Size(62, 26);
            checkBoxFilterCaseSensitiv.TabIndex = 16;
            checkBoxFilterCaseSensitiv.Text = "a <> A";
            toolTip.SetToolTip(checkBoxFilterCaseSensitiv, "Use case sensitivity for filter");
            checkBoxFilterCaseSensitiv.UseVisualStyleBackColor = true;
            // 
            // buttonClose
            // 
            buttonClose.Dock = System.Windows.Forms.DockStyle.Left;
            buttonClose.FlatAppearance.BorderSize = 0;
            buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonClose.Image = Properties.Resources.Exit;
            buttonClose.Location = new System.Drawing.Point(602, 16);
            buttonClose.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new System.Drawing.Size(21, 29);
            buttonClose.TabIndex = 29;
            toolTip.SetToolTip(buttonClose, "Close the window without saving the changes");
            buttonClose.UseVisualStyleBackColor = true;
            buttonClose.Click += buttonClose_Click;
            // 
            // groupBoxEdit
            // 
            groupBoxEdit.Controls.Add(tableLayoutPanelEdit);
            groupBoxEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxEdit.Location = new System.Drawing.Point(27, 0);
            groupBoxEdit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            groupBoxEdit.Name = "groupBoxEdit";
            groupBoxEdit.Padding = new System.Windows.Forms.Padding(4, 2, 4, 3);
            tableLayoutPanel.SetRowSpan(groupBoxEdit, 2);
            groupBoxEdit.Size = new System.Drawing.Size(507, 48);
            groupBoxEdit.TabIndex = 30;
            groupBoxEdit.TabStop = false;
            groupBoxEdit.Text = "Edit";
            groupBoxEdit.Visible = false;
            // 
            // tableLayoutPanelEdit
            // 
            tableLayoutPanelEdit.ColumnCount = 5;
            tableLayoutPanelEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelEdit.Controls.Add(comboBoxAction, 0, 0);
            tableLayoutPanelEdit.Controls.Add(labelEditReplace, 3, 0);
            tableLayoutPanelEdit.Controls.Add(buttonEdit, 1, 0);
            tableLayoutPanelEdit.Controls.Add(panelEditInsert, 2, 0);
            tableLayoutPanelEdit.Controls.Add(panelEditReplace, 4, 0);
            tableLayoutPanelEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelEdit.Location = new System.Drawing.Point(4, 18);
            tableLayoutPanelEdit.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanelEdit.Name = "tableLayoutPanelEdit";
            tableLayoutPanelEdit.RowCount = 1;
            tableLayoutPanelEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelEdit.Size = new System.Drawing.Size(499, 27);
            tableLayoutPanelEdit.TabIndex = 20;
            // 
            // comboBoxAction
            // 
            comboBoxAction.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxAction.FormattingEnabled = true;
            comboBoxAction.Items.AddRange(new object[] { "Insert", "Append", "Replace", "Clear", "Transfer" });
            comboBoxAction.Location = new System.Drawing.Point(0, 1);
            comboBoxAction.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            comboBoxAction.Name = "comboBoxAction";
            comboBoxAction.Size = new System.Drawing.Size(87, 23);
            comboBoxAction.TabIndex = 10;
            toolTip.SetToolTip(comboBoxAction, "Select the type of editing");
            comboBoxAction.SelectedIndexChanged += comboBoxAction_SelectedIndexChanged;
            // 
            // labelEditReplace
            // 
            labelEditReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            labelEditReplace.Location = new System.Drawing.Point(340, 0);
            labelEditReplace.Margin = new System.Windows.Forms.Padding(0);
            labelEditReplace.Name = "labelEditReplace";
            labelEditReplace.Size = new System.Drawing.Size(19, 27);
            labelEditReplace.TabIndex = 14;
            labelEditReplace.Text = "->";
            labelEditReplace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            labelEditReplace.Visible = false;
            // 
            // buttonEdit
            // 
            buttonEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonEdit.Location = new System.Drawing.Point(87, 0);
            buttonEdit.Margin = new System.Windows.Forms.Padding(0);
            buttonEdit.Name = "buttonEdit";
            buttonEdit.Size = new System.Drawing.Size(28, 27);
            buttonEdit.TabIndex = 15;
            toolTip.SetToolTip(buttonEdit, "Perform the selected changes");
            buttonEdit.UseVisualStyleBackColor = true;
            buttonEdit.Visible = false;
            buttonEdit.Click += buttonEdit_Click;
            // 
            // panelEditInsert
            // 
            panelEditInsert.Controls.Add(comboBoxEditInsert);
            panelEditInsert.Controls.Add(textBoxEditInsert);
            panelEditInsert.Dock = System.Windows.Forms.DockStyle.Fill;
            panelEditInsert.Location = new System.Drawing.Point(115, 1);
            panelEditInsert.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            panelEditInsert.Name = "panelEditInsert";
            panelEditInsert.Size = new System.Drawing.Size(225, 26);
            panelEditInsert.TabIndex = 20;
            // 
            // comboBoxEditInsert
            // 
            comboBoxEditInsert.Dock = System.Windows.Forms.DockStyle.Right;
            comboBoxEditInsert.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxEditInsert.FormattingEnabled = true;
            comboBoxEditInsert.Location = new System.Drawing.Point(198, 0);
            comboBoxEditInsert.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            comboBoxEditInsert.Name = "comboBoxEditInsert";
            comboBoxEditInsert.Size = new System.Drawing.Size(27, 23);
            comboBoxEditInsert.TabIndex = 18;
            toolTip.SetToolTip(comboBoxEditInsert, "The value that should be inserted");
            comboBoxEditInsert.Visible = false;
            comboBoxEditInsert.DropDown += comboBoxEditInsert_DropDown;
            comboBoxEditInsert.SelectionChangeCommitted += comboBoxEditInsert_SelectionChangeCommitted;
            // 
            // textBoxEditInsert
            // 
            textBoxEditInsert.ContextMenuStrip = contextMenuStripEditInsert;
            textBoxEditInsert.Dock = System.Windows.Forms.DockStyle.Left;
            textBoxEditInsert.Location = new System.Drawing.Point(0, 0);
            textBoxEditInsert.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            textBoxEditInsert.Name = "textBoxEditInsert";
            textBoxEditInsert.Size = new System.Drawing.Size(23, 23);
            textBoxEditInsert.TabIndex = 13;
            toolTip.SetToolTip(textBoxEditInsert, "The text that should be inserted");
            textBoxEditInsert.Visible = false;
            // 
            // contextMenuStripEditInsert
            // 
            contextMenuStripEditInsert.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tabulatorToolStripMenuItem, returnToolStripMenuItem, resetInsertToolStripMenuItem });
            contextMenuStripEditInsert.Name = "contextMenuStripEditSpecialSigns";
            contextMenuStripEditInsert.Size = new System.Drawing.Size(110, 70);
            // 
            // tabulatorToolStripMenuItem
            // 
            tabulatorToolStripMenuItem.Name = "tabulatorToolStripMenuItem";
            tabulatorToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            tabulatorToolStripMenuItem.Text = "Tab";
            tabulatorToolStripMenuItem.ToolTipText = "Insert tabulator sign (will disable manual editing)";
            tabulatorToolStripMenuItem.Click += tabulatorToolStripMenuItem_Click;
            // 
            // returnToolStripMenuItem
            // 
            returnToolStripMenuItem.Name = "returnToolStripMenuItem";
            returnToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            returnToolStripMenuItem.Text = "Return";
            returnToolStripMenuItem.ToolTipText = "Insert return sign (will disable manual editing)";
            returnToolStripMenuItem.Click += returnToolStripMenuItem_Click;
            // 
            // resetInsertToolStripMenuItem
            // 
            resetInsertToolStripMenuItem.ForeColor = System.Drawing.Color.Red;
            resetInsertToolStripMenuItem.Image = Properties.Resources.Delete;
            resetInsertToolStripMenuItem.Name = "resetInsertToolStripMenuItem";
            resetInsertToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            resetInsertToolStripMenuItem.Text = "reset";
            resetInsertToolStripMenuItem.ToolTipText = "Remove entry and enable manual editing";
            resetInsertToolStripMenuItem.Click += resetInsertToolStripMenuItem_Click;
            // 
            // panelEditReplace
            // 
            panelEditReplace.Controls.Add(comboBoxEditReplace);
            panelEditReplace.Controls.Add(textBoxEditReplace);
            panelEditReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            panelEditReplace.Location = new System.Drawing.Point(359, 1);
            panelEditReplace.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            panelEditReplace.Name = "panelEditReplace";
            panelEditReplace.Size = new System.Drawing.Size(140, 26);
            panelEditReplace.TabIndex = 21;
            // 
            // comboBoxEditReplace
            // 
            comboBoxEditReplace.Dock = System.Windows.Forms.DockStyle.Right;
            comboBoxEditReplace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxEditReplace.FormattingEnabled = true;
            comboBoxEditReplace.Location = new System.Drawing.Point(101, 0);
            comboBoxEditReplace.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            comboBoxEditReplace.Name = "comboBoxEditReplace";
            comboBoxEditReplace.Size = new System.Drawing.Size(19, 23);
            comboBoxEditReplace.TabIndex = 17;
            toolTip.SetToolTip(comboBoxEditReplace, "The value that should be replaced");
            comboBoxEditReplace.Visible = false;
            comboBoxEditReplace.SelectionChangeCommitted += comboBoxEditReplace_SelectionChangeCommitted;
            // 
            // textBoxEditReplace
            // 
            textBoxEditReplace.ContextMenuStrip = contextMenuStripEditReplace;
            textBoxEditReplace.Dock = System.Windows.Forms.DockStyle.Right;
            textBoxEditReplace.Location = new System.Drawing.Point(120, 0);
            textBoxEditReplace.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            textBoxEditReplace.Name = "textBoxEditReplace";
            textBoxEditReplace.Size = new System.Drawing.Size(20, 23);
            textBoxEditReplace.TabIndex = 12;
            toolTip.SetToolTip(textBoxEditReplace, "The text that should be replaced");
            textBoxEditReplace.Visible = false;
            // 
            // contextMenuStripEditReplace
            // 
            contextMenuStripEditReplace.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tabToolStripMenuItem, returnReplaceToolStripMenuItem, resetToolStripMenuItem });
            contextMenuStripEditReplace.Name = "contextMenuStripEditReplace";
            contextMenuStripEditReplace.Size = new System.Drawing.Size(110, 70);
            // 
            // tabToolStripMenuItem
            // 
            tabToolStripMenuItem.Name = "tabToolStripMenuItem";
            tabToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            tabToolStripMenuItem.Text = "Tab";
            tabToolStripMenuItem.ToolTipText = "Insert tabulator sign (will disable manual editing)";
            tabToolStripMenuItem.Click += tabToolStripMenuItem_Click;
            // 
            // returnReplaceToolStripMenuItem
            // 
            returnReplaceToolStripMenuItem.Name = "returnReplaceToolStripMenuItem";
            returnReplaceToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            returnReplaceToolStripMenuItem.Text = "Return";
            returnReplaceToolStripMenuItem.ToolTipText = "Insert return sign (will disable manual editing)";
            returnReplaceToolStripMenuItem.Click += returnReplaceToolStripMenuItem_Click;
            // 
            // resetToolStripMenuItem
            // 
            resetToolStripMenuItem.ForeColor = System.Drawing.Color.Red;
            resetToolStripMenuItem.Image = Properties.Resources.Delete;
            resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            resetToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            resetToolStripMenuItem.Text = "reset";
            resetToolStripMenuItem.ToolTipText = "Remove entry and enable manual editing";
            resetToolStripMenuItem.Click += resetToolStripMenuItem_Click;
            // 
            // groupBoxExport
            // 
            groupBoxExport.Controls.Add(buttonExportSqlite);
            groupBoxExport.Controls.Add(buttonOpenExportFile);
            groupBoxExport.Controls.Add(buttonExport);
            groupBoxExport.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxExport.Location = new System.Drawing.Point(1125, 0);
            groupBoxExport.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            groupBoxExport.Name = "groupBoxExport";
            groupBoxExport.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel.SetRowSpan(groupBoxExport, 2);
            groupBoxExport.Size = new System.Drawing.Size(81, 48);
            groupBoxExport.TabIndex = 31;
            groupBoxExport.TabStop = false;
            groupBoxExport.Text = "Export";
            // 
            // buttonExportSqlite
            // 
            buttonExportSqlite.Dock = System.Windows.Forms.DockStyle.Right;
            buttonExportSqlite.FlatAppearance.BorderSize = 0;
            buttonExportSqlite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonExportSqlite.Image = Properties.Resources.SQLite;
            buttonExportSqlite.Location = new System.Drawing.Point(56, 19);
            buttonExportSqlite.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonExportSqlite.Name = "buttonExportSqlite";
            buttonExportSqlite.Size = new System.Drawing.Size(21, 26);
            buttonExportSqlite.TabIndex = 28;
            toolTip.SetToolTip(buttonExportSqlite, "Export the data of the table into a SQLite database");
            buttonExportSqlite.UseVisualStyleBackColor = true;
            buttonExportSqlite.Click += buttonExportSqlite_Click;
            // 
            // buttonOpenExportFile
            // 
            buttonOpenExportFile.Dock = System.Windows.Forms.DockStyle.Left;
            buttonOpenExportFile.FlatAppearance.BorderSize = 0;
            buttonOpenExportFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonOpenExportFile.Image = Properties.Resources.OpenFolder;
            buttonOpenExportFile.Location = new System.Drawing.Point(27, 19);
            buttonOpenExportFile.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            buttonOpenExportFile.Name = "buttonOpenExportFile";
            buttonOpenExportFile.Size = new System.Drawing.Size(23, 26);
            buttonOpenExportFile.TabIndex = 27;
            toolTip.SetToolTip(buttonOpenExportFile, "Open exported file");
            buttonOpenExportFile.UseVisualStyleBackColor = true;
            buttonOpenExportFile.Click += buttonOpenExportFile_Click;
            // 
            // buttonExport
            // 
            buttonExport.Dock = System.Windows.Forms.DockStyle.Left;
            buttonExport.FlatAppearance.BorderSize = 0;
            buttonExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonExport.Image = Properties.Resources.Export;
            buttonExport.Location = new System.Drawing.Point(4, 19);
            buttonExport.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            buttonExport.Name = "buttonExport";
            buttonExport.Size = new System.Drawing.Size(23, 26);
            buttonExport.TabIndex = 24;
            toolTip.SetToolTip(buttonExport, "Export the data as tab separated text file");
            buttonExport.UseVisualStyleBackColor = true;
            buttonExport.Click += buttonExport_Click;
            // 
            // buttonOpenLog
            // 
            buttonOpenLog.Dock = System.Windows.Forms.DockStyle.Left;
            buttonOpenLog.FlatAppearance.BorderSize = 0;
            buttonOpenLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonOpenLog.Image = Properties.Resources.History;
            buttonOpenLog.Location = new System.Drawing.Point(695, 16);
            buttonOpenLog.Margin = new System.Windows.Forms.Padding(2, 3, 0, 3);
            buttonOpenLog.Name = "buttonOpenLog";
            buttonOpenLog.Size = new System.Drawing.Size(21, 29);
            buttonOpenLog.TabIndex = 32;
            toolTip.SetToolTip(buttonOpenLog, "Inspect data in logging table (not filtered)");
            buttonOpenLog.UseVisualStyleBackColor = true;
            buttonOpenLog.Click += buttonOpenLog_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.Dock = System.Windows.Forms.DockStyle.Left;
            buttonDelete.FlatAppearance.BorderSize = 0;
            buttonDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonDelete.Image = Properties.Resources.Delete;
            buttonDelete.Location = new System.Drawing.Point(538, 16);
            buttonDelete.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new System.Drawing.Size(23, 29);
            buttonDelete.TabIndex = 33;
            toolTip.SetToolTip(buttonDelete, "Delete selected data");
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += buttonDelete_Click;
            // 
            // buttonFeedback
            // 
            buttonFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonFeedback.FlatAppearance.BorderSize = 0;
            buttonFeedback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonFeedback.Image = Properties.Resources.Feedback;
            buttonFeedback.Location = new System.Drawing.Point(1210, 16);
            buttonFeedback.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            buttonFeedback.Name = "buttonFeedback";
            buttonFeedback.Size = new System.Drawing.Size(22, 29);
            buttonFeedback.TabIndex = 34;
            toolTip.SetToolTip(buttonFeedback, "Send a feedback to the administrator");
            buttonFeedback.UseVisualStyleBackColor = true;
            buttonFeedback.Click += buttonFeedback_Click;
            // 
            // checkBoxUseComboboxValues
            // 
            checkBoxUseComboboxValues.Appearance = System.Windows.Forms.Appearance.Button;
            checkBoxUseComboboxValues.AutoSize = true;
            checkBoxUseComboboxValues.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxUseComboboxValues.FlatAppearance.BorderSize = 0;
            checkBoxUseComboboxValues.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxUseComboboxValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            checkBoxUseComboboxValues.Image = Properties.Resources.IDGrey;
            checkBoxUseComboboxValues.Location = new System.Drawing.Point(716, 16);
            checkBoxUseComboboxValues.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            checkBoxUseComboboxValues.Name = "checkBoxUseComboboxValues";
            checkBoxUseComboboxValues.Size = new System.Drawing.Size(23, 29);
            checkBoxUseComboboxValues.TabIndex = 35;
            toolTip.SetToolTip(checkBoxUseComboboxValues, "Show IDs of predefined values from lookup tables");
            checkBoxUseComboboxValues.UseVisualStyleBackColor = true;
            checkBoxUseComboboxValues.Click += checkBoxUseComboboxValues_Click;
            // 
            // buttonShowRowNumber
            // 
            buttonShowRowNumber.Dock = System.Windows.Forms.DockStyle.Left;
            buttonShowRowNumber.FlatAppearance.BorderSize = 0;
            buttonShowRowNumber.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonShowRowNumber.Image = Properties.Resources.InLines;
            buttonShowRowNumber.Location = new System.Drawing.Point(4, 16);
            buttonShowRowNumber.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            buttonShowRowNumber.Name = "buttonShowRowNumber";
            buttonShowRowNumber.Size = new System.Drawing.Size(19, 29);
            buttonShowRowNumber.TabIndex = 36;
            toolTip.SetToolTip(buttonShowRowNumber, "Show row numbers");
            buttonShowRowNumber.UseVisualStyleBackColor = true;
            buttonShowRowNumber.Click += buttonShowRowNumber_Click;
            // 
            // buttonSetColumnWidthContent
            // 
            buttonSetColumnWidthContent.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonSetColumnWidthContent.FlatAppearance.BorderSize = 0;
            buttonSetColumnWidthContent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonSetColumnWidthContent.Image = Properties.Resources.OptColumnWidthContent;
            buttonSetColumnWidthContent.Location = new System.Drawing.Point(650, 13);
            buttonSetColumnWidthContent.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            buttonSetColumnWidthContent.Name = "buttonSetColumnWidthContent";
            buttonSetColumnWidthContent.Size = new System.Drawing.Size(21, 35);
            buttonSetColumnWidthContent.TabIndex = 37;
            toolTip.SetToolTip(buttonSetColumnWidthContent, "Set column width according to content excluding headers");
            buttonSetColumnWidthContent.UseVisualStyleBackColor = true;
            buttonSetColumnWidthContent.Click += buttonSetColumnWidthContent_Click;
            // 
            // imageListEditAction
            // 
            imageListEditAction.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListEditAction.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListEditAction.ImageStream");
            imageListEditAction.TransparentColor = System.Drawing.Color.Transparent;
            imageListEditAction.Images.SetKeyName(0, "Append.ico");
            imageListEditAction.Images.SetKeyName(1, "Insert.ico");
            imageListEditAction.Images.SetKeyName(2, "Replace.ico");
            imageListEditAction.Images.SetKeyName(3, "Radierer.ico");
            imageListEditAction.Images.SetKeyName(4, "Transfer.ico");
            imageListEditAction.Images.SetKeyName(5, "ArrowTrim.ico");
            // 
            // FormTableEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1236, 727);
            Controls.Add(tableLayoutPanel);
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormTableEditor";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "FormTableEditor";
            FormClosing += FormTableEditor_FormClosing;
            KeyDown += Form_KeyDown;
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            contextMenuStripTable.ResumeLayout(false);
            groupBoxFilter.ResumeLayout(false);
            groupBoxFilter.PerformLayout();
            groupBoxEdit.ResumeLayout(false);
            tableLayoutPanelEdit.ResumeLayout(false);
            panelEditInsert.ResumeLayout(false);
            panelEditInsert.PerformLayout();
            contextMenuStripEditInsert.ResumeLayout(false);
            panelEditReplace.ResumeLayout(false);
            panelEditReplace.PerformLayout();
            contextMenuStripEditReplace.ResumeLayout(false);
            groupBoxExport.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TextBox textBoxEditReplace;
        private System.Windows.Forms.TextBox textBoxEditInsert;
        private System.Windows.Forms.Label labelEditReplace;
        private System.Windows.Forms.ComboBox comboBoxAction;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.ImageList imageListEditAction;
        private System.Windows.Forms.ComboBox comboBoxEditReplace;
        private System.Windows.Forms.ComboBox comboBoxEditInsert;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonMarkColumn;
        private System.Windows.Forms.Button buttonSetColumnWidth;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button buttonOpenExportFile;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.GroupBox groupBoxEdit;
        private System.Windows.Forms.GroupBox groupBoxFilter;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.ComboBox comboBoxFilter;
        private System.Windows.Forms.Button buttonFilter;
        private System.Windows.Forms.CheckBox checkBoxFilterCaseSensitiv;
        private System.Windows.Forms.GroupBox groupBoxExport;
        private System.Windows.Forms.Button buttonOpenLog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEdit;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.Button buttonExportSqlite;
        private System.Windows.Forms.CheckBox checkBoxUseComboboxValues;
        private System.Windows.Forms.Button buttonShowRowNumber;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTable;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearSelectedCellsToolStripMenuItem;
        private System.Windows.Forms.Button buttonSetColumnWidthContent;
        private System.Windows.Forms.Panel panelEditInsert;
        private System.Windows.Forms.Panel panelEditReplace;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripEditInsert;
        private System.Windows.Forms.ToolStripMenuItem tabulatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem returnToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripEditReplace;
        private System.Windows.Forms.ToolStripMenuItem tabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem returnReplaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetInsertToolStripMenuItem;
    }
}