namespace DiversityWorkbench.Import
{
    partial class UserControlFile
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlFile));
            textBoxFile = new System.Windows.Forms.TextBox();
            textBoxSchema = new System.Windows.Forms.TextBox();
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            labelSchema = new System.Windows.Forms.Label();
            labelFile = new System.Windows.Forms.Label();
            labelEncoding = new System.Windows.Forms.Label();
            comboBoxEncoding = new System.Windows.Forms.ComboBox();
            labelStartLine = new System.Windows.Forms.Label();
            numericUpDownStartLine = new System.Windows.Forms.NumericUpDown();
            labelEndLine = new System.Windows.Forms.Label();
            numericUpDownEndLine = new System.Windows.Forms.NumericUpDown();
            buttonOpenFile = new System.Windows.Forms.Button();
            contextMenuStripOpenFile = new System.Windows.Forms.ContextMenuStrip(components);
            adaptDefaultDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            checkBoxFirstLine = new System.Windows.Forms.CheckBox();
            webBrowserSchema = new System.Windows.Forms.WebBrowser();
            labelLanguage = new System.Windows.Forms.Label();
            toolStripLanguage = new System.Windows.Forms.ToolStrip();
            toolStripDropDownButtonLanguage = new System.Windows.Forms.ToolStripDropDownButton();
            toolStripMenuItemGerman = new System.Windows.Forms.ToolStripMenuItem();
            uSAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemEnglish = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemFrensh = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemItaly = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemSpanish = new System.Windows.Forms.ToolStripMenuItem();
            invariantCultureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            checkBoxUseDefaultDuplicateCheck = new System.Windows.Forms.CheckBox();
            labelSeparator = new System.Windows.Forms.Label();
            comboBoxSeparator = new System.Windows.Forms.ComboBox();
            tableLayoutPanelSchema = new System.Windows.Forms.TableLayoutPanel();
            toolStripSchema = new System.Windows.Forms.ToolStrip();
            toolStripButtonRemoveSchema = new System.Windows.Forms.ToolStripButton();
            toolStripButtonOpenSchema = new System.Windows.Forms.ToolStripButton();
            toolStripButtonShowSchema = new System.Windows.Forms.ToolStripButton();
            toolStripDropDownButtonGitHub = new System.Windows.Forms.ToolStripDropDownButton();
            sNSBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            zFMKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            checkBoxIncludePreAndPostfix = new System.Windows.Forms.CheckBox();
            checkBoxRecordSQL = new System.Windows.Forms.CheckBox();
            buttonOpenRecordSQL = new System.Windows.Forms.Button();
            checkBoxTranslateReturn = new System.Windows.Forms.CheckBox();
            openFileDialog = new System.Windows.Forms.OpenFileDialog();
            imageListLanguage = new System.Windows.Forms.ImageList(components);
            toolTip = new System.Windows.Forms.ToolTip(components);
            tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownStartLine).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownEndLine).BeginInit();
            contextMenuStripOpenFile.SuspendLayout();
            toolStripLanguage.SuspendLayout();
            tableLayoutPanelSchema.SuspendLayout();
            toolStripSchema.SuspendLayout();
            SuspendLayout();
            // 
            // textBoxFile
            // 
            tableLayoutPanel.SetColumnSpan(textBoxFile, 7);
            textBoxFile.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxFile.Location = new System.Drawing.Point(64, 3);
            textBoxFile.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            textBoxFile.Name = "textBoxFile";
            textBoxFile.ReadOnly = true;
            textBoxFile.Size = new System.Drawing.Size(678, 23);
            textBoxFile.TabIndex = 1;
            // 
            // textBoxSchema
            // 
            textBoxSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxSchema.Location = new System.Drawing.Point(0, 3);
            textBoxSchema.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            textBoxSchema.Name = "textBoxSchema";
            textBoxSchema.ReadOnly = true;
            textBoxSchema.Size = new System.Drawing.Size(600, 23);
            textBoxSchema.TabIndex = 2;
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 9;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.Controls.Add(labelSchema, 0, 5);
            tableLayoutPanel.Controls.Add(textBoxFile, 1, 0);
            tableLayoutPanel.Controls.Add(labelFile, 0, 0);
            tableLayoutPanel.Controls.Add(labelEncoding, 0, 1);
            tableLayoutPanel.Controls.Add(comboBoxEncoding, 1, 1);
            tableLayoutPanel.Controls.Add(labelStartLine, 4, 1);
            tableLayoutPanel.Controls.Add(numericUpDownStartLine, 5, 1);
            tableLayoutPanel.Controls.Add(labelEndLine, 6, 1);
            tableLayoutPanel.Controls.Add(numericUpDownEndLine, 7, 1);
            tableLayoutPanel.Controls.Add(buttonOpenFile, 8, 0);
            tableLayoutPanel.Controls.Add(checkBoxFirstLine, 1, 2);
            tableLayoutPanel.Controls.Add(webBrowserSchema, 1, 6);
            tableLayoutPanel.Controls.Add(labelLanguage, 4, 2);
            tableLayoutPanel.Controls.Add(toolStripLanguage, 6, 2);
            tableLayoutPanel.Controls.Add(checkBoxUseDefaultDuplicateCheck, 1, 3);
            tableLayoutPanel.Controls.Add(labelSeparator, 2, 2);
            tableLayoutPanel.Controls.Add(comboBoxSeparator, 3, 2);
            tableLayoutPanel.Controls.Add(tableLayoutPanelSchema, 1, 5);
            tableLayoutPanel.Controls.Add(checkBoxIncludePreAndPostfix, 1, 4);
            tableLayoutPanel.Controls.Add(checkBoxRecordSQL, 4, 4);
            tableLayoutPanel.Controls.Add(buttonOpenRecordSQL, 8, 4);
            tableLayoutPanel.Controls.Add(checkBoxTranslateReturn, 5, 3);
            tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 7;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.Size = new System.Drawing.Size(769, 361);
            tableLayoutPanel.TabIndex = 1;
            // 
            // labelSchema
            // 
            labelSchema.AutoSize = true;
            labelSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            labelSchema.Location = new System.Drawing.Point(4, 133);
            labelSchema.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelSchema.Name = "labelSchema";
            labelSchema.Size = new System.Drawing.Size(60, 29);
            labelSchema.TabIndex = 5;
            labelSchema.Text = "Schema:";
            labelSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelFile
            // 
            labelFile.AutoSize = true;
            labelFile.Dock = System.Windows.Forms.DockStyle.Fill;
            labelFile.Location = new System.Drawing.Point(4, 0);
            labelFile.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelFile.Name = "labelFile";
            labelFile.Size = new System.Drawing.Size(60, 30);
            labelFile.TabIndex = 4;
            labelFile.Text = "File:";
            labelFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelEncoding
            // 
            labelEncoding.AutoSize = true;
            labelEncoding.Dock = System.Windows.Forms.DockStyle.Fill;
            labelEncoding.Location = new System.Drawing.Point(4, 30);
            labelEncoding.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelEncoding.Name = "labelEncoding";
            labelEncoding.Size = new System.Drawing.Size(60, 29);
            labelEncoding.TabIndex = 7;
            labelEncoding.Text = "Encoding:";
            labelEncoding.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxEncoding
            // 
            tableLayoutPanel.SetColumnSpan(comboBoxEncoding, 3);
            comboBoxEncoding.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxEncoding.FormattingEnabled = true;
            comboBoxEncoding.Location = new System.Drawing.Point(64, 33);
            comboBoxEncoding.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            comboBoxEncoding.Name = "comboBoxEncoding";
            comboBoxEncoding.Size = new System.Drawing.Size(414, 23);
            comboBoxEncoding.TabIndex = 8;
            toolTip.SetToolTip(comboBoxEncoding, "The encoding of the source file");
            comboBoxEncoding.SelectedIndexChanged += comboBoxEncoding_SelectedIndexChanged;
            // 
            // labelStartLine
            // 
            labelStartLine.AutoSize = true;
            labelStartLine.Dock = System.Windows.Forms.DockStyle.Fill;
            labelStartLine.Location = new System.Drawing.Point(486, 30);
            labelStartLine.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelStartLine.Name = "labelStartLine";
            labelStartLine.Size = new System.Drawing.Size(56, 29);
            labelStartLine.TabIndex = 9;
            labelStartLine.Text = "Start line:";
            labelStartLine.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            toolTip.SetToolTip(labelStartLine, "Double click here to set to minimum");
            labelStartLine.DoubleClick += labelStartLine_DoubleClick;
            // 
            // numericUpDownStartLine
            // 
            numericUpDownStartLine.Dock = System.Windows.Forms.DockStyle.Fill;
            numericUpDownStartLine.Location = new System.Drawing.Point(542, 33);
            numericUpDownStartLine.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            numericUpDownStartLine.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numericUpDownStartLine.Name = "numericUpDownStartLine";
            numericUpDownStartLine.Size = new System.Drawing.Size(70, 23);
            numericUpDownStartLine.TabIndex = 10;
            toolTip.SetToolTip(numericUpDownStartLine, "The line where the import should start");
            numericUpDownStartLine.ValueChanged += numericUpDownStartLine_ValueChanged;
            // 
            // labelEndLine
            // 
            labelEndLine.AutoSize = true;
            labelEndLine.Dock = System.Windows.Forms.DockStyle.Fill;
            labelEndLine.Location = new System.Drawing.Point(620, 30);
            labelEndLine.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelEndLine.Name = "labelEndLine";
            labelEndLine.Size = new System.Drawing.Size(52, 29);
            labelEndLine.TabIndex = 11;
            labelEndLine.Text = "End line:";
            labelEndLine.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            toolTip.SetToolTip(labelEndLine, "Double click here to set to maximum");
            labelEndLine.DoubleClick += labelEndLine_DoubleClick;
            // 
            // numericUpDownEndLine
            // 
            tableLayoutPanel.SetColumnSpan(numericUpDownEndLine, 2);
            numericUpDownEndLine.Dock = System.Windows.Forms.DockStyle.Fill;
            numericUpDownEndLine.Location = new System.Drawing.Point(672, 33);
            numericUpDownEndLine.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            numericUpDownEndLine.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numericUpDownEndLine.Name = "numericUpDownEndLine";
            numericUpDownEndLine.Size = new System.Drawing.Size(93, 23);
            numericUpDownEndLine.TabIndex = 12;
            toolTip.SetToolTip(numericUpDownEndLine, "The last line that should be imported");
            numericUpDownEndLine.ValueChanged += numericUpDownEndLine_ValueChanged;
            // 
            // buttonOpenFile
            // 
            buttonOpenFile.ContextMenuStrip = contextMenuStripOpenFile;
            buttonOpenFile.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonOpenFile.FlatAppearance.BorderSize = 0;
            buttonOpenFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonOpenFile.Image = ResourceWorkbench.Open;
            buttonOpenFile.Location = new System.Drawing.Point(742, 3);
            buttonOpenFile.Margin = new System.Windows.Forms.Padding(0, 3, 4, 0);
            buttonOpenFile.Name = "buttonOpenFile";
            buttonOpenFile.Size = new System.Drawing.Size(23, 27);
            buttonOpenFile.TabIndex = 0;
            toolTip.SetToolTip(buttonOpenFile, "Open the source file with the data that should be imported");
            buttonOpenFile.UseVisualStyleBackColor = true;
            buttonOpenFile.Click += buttonOpenFile_Click;
            // 
            // contextMenuStripOpenFile
            // 
            contextMenuStripOpenFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { adaptDefaultDirectoryToolStripMenuItem });
            contextMenuStripOpenFile.Name = "contextMenuStripOpenFile";
            contextMenuStripOpenFile.Size = new System.Drawing.Size(197, 26);
            // 
            // adaptDefaultDirectoryToolStripMenuItem
            // 
            adaptDefaultDirectoryToolStripMenuItem.Checked = true;
            adaptDefaultDirectoryToolStripMenuItem.CheckOnClick = true;
            adaptDefaultDirectoryToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            adaptDefaultDirectoryToolStripMenuItem.Name = "adaptDefaultDirectoryToolStripMenuItem";
            adaptDefaultDirectoryToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            adaptDefaultDirectoryToolStripMenuItem.Text = "Adapt default directory";
            adaptDefaultDirectoryToolStripMenuItem.Click += adaptDefaultDirectoryToolStripMenuItem_Click;
            // 
            // checkBoxFirstLine
            // 
            checkBoxFirstLine.AutoSize = true;
            checkBoxFirstLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            checkBoxFirstLine.Location = new System.Drawing.Point(64, 65);
            checkBoxFirstLine.Margin = new System.Windows.Forms.Padding(0);
            checkBoxFirstLine.Name = "checkBoxFirstLine";
            checkBoxFirstLine.Size = new System.Drawing.Size(284, 19);
            checkBoxFirstLine.TabIndex = 13;
            checkBoxFirstLine.Text = "First line contains column definition";
            checkBoxFirstLine.UseVisualStyleBackColor = true;
            checkBoxFirstLine.Click += checkBoxFirstLine_Click;
            // 
            // webBrowserSchema
            // 
            tableLayoutPanel.SetColumnSpan(webBrowserSchema, 8);
            webBrowserSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            webBrowserSchema.Location = new System.Drawing.Point(64, 165);
            webBrowserSchema.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            webBrowserSchema.MinimumSize = new System.Drawing.Size(23, 23);
            webBrowserSchema.Name = "webBrowserSchema";
            webBrowserSchema.Size = new System.Drawing.Size(701, 193);
            webBrowserSchema.TabIndex = 14;
            // 
            // labelLanguage
            // 
            labelLanguage.AutoSize = true;
            tableLayoutPanel.SetColumnSpan(labelLanguage, 2);
            labelLanguage.Dock = System.Windows.Forms.DockStyle.Top;
            labelLanguage.Location = new System.Drawing.Point(486, 66);
            labelLanguage.Margin = new System.Windows.Forms.Padding(4, 7, 4, 0);
            labelLanguage.Name = "labelLanguage";
            labelLanguage.Size = new System.Drawing.Size(126, 15);
            labelLanguage.TabIndex = 15;
            labelLanguage.Text = "Language / Country:";
            labelLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripLanguage
            // 
            tableLayoutPanel.SetColumnSpan(toolStripLanguage, 3);
            toolStripLanguage.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripLanguage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripDropDownButtonLanguage });
            toolStripLanguage.Location = new System.Drawing.Point(616, 59);
            toolStripLanguage.Name = "toolStripLanguage";
            toolStripLanguage.Size = new System.Drawing.Size(153, 25);
            toolStripLanguage.TabIndex = 16;
            toolStripLanguage.Text = "toolStrip1";
            // 
            // toolStripDropDownButtonLanguage
            // 
            toolStripDropDownButtonLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItemGerman, uSAToolStripMenuItem, toolStripMenuItemEnglish, toolStripMenuItemFrensh, toolStripMenuItemItaly, toolStripMenuItemSpanish, invariantCultureToolStripMenuItem });
            toolStripDropDownButtonLanguage.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButtonLanguage.Name = "toolStripDropDownButtonLanguage";
            toolStripDropDownButtonLanguage.Size = new System.Drawing.Size(25, 22);
            toolStripDropDownButtonLanguage.Text = "?";
            toolStripDropDownButtonLanguage.Paint += toolStripDropDownButtonLanguage_Paint;
            // 
            // toolStripMenuItemGerman
            // 
            toolStripMenuItemGerman.Image = Properties.Resources.Deutsch;
            toolStripMenuItemGerman.Name = "toolStripMenuItemGerman";
            toolStripMenuItemGerman.Size = new System.Drawing.Size(160, 22);
            toolStripMenuItemGerman.Text = "German";
            toolStripMenuItemGerman.Click += toolStripMenuItemGerman_Click;
            // 
            // uSAToolStripMenuItem
            // 
            uSAToolStripMenuItem.Image = Properties.Resources.USA;
            uSAToolStripMenuItem.Name = "uSAToolStripMenuItem";
            uSAToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            uSAToolStripMenuItem.Text = "USA";
            uSAToolStripMenuItem.Click += uSAToolStripMenuItem_Click;
            // 
            // toolStripMenuItemEnglish
            // 
            toolStripMenuItemEnglish.Image = Properties.Resources.English;
            toolStripMenuItemEnglish.Name = "toolStripMenuItemEnglish";
            toolStripMenuItemEnglish.Size = new System.Drawing.Size(160, 22);
            toolStripMenuItemEnglish.Text = "English";
            toolStripMenuItemEnglish.Click += toolStripMenuItemEnglish_Click;
            // 
            // toolStripMenuItemFrensh
            // 
            toolStripMenuItemFrensh.Image = Properties.Resources.Frankreich;
            toolStripMenuItemFrensh.Name = "toolStripMenuItemFrensh";
            toolStripMenuItemFrensh.Size = new System.Drawing.Size(160, 22);
            toolStripMenuItemFrensh.Text = "Frensh";
            toolStripMenuItemFrensh.Click += toolStripMenuItemFrensh_Click;
            // 
            // toolStripMenuItemItaly
            // 
            toolStripMenuItemItaly.Image = Properties.Resources.Italien;
            toolStripMenuItemItaly.Name = "toolStripMenuItemItaly";
            toolStripMenuItemItaly.Size = new System.Drawing.Size(160, 22);
            toolStripMenuItemItaly.Text = "Italian";
            toolStripMenuItemItaly.Click += toolStripMenuItemItaly_Click;
            // 
            // toolStripMenuItemSpanish
            // 
            toolStripMenuItemSpanish.Image = Properties.Resources.Spanien;
            toolStripMenuItemSpanish.Name = "toolStripMenuItemSpanish";
            toolStripMenuItemSpanish.Size = new System.Drawing.Size(160, 22);
            toolStripMenuItemSpanish.Text = "Spanish";
            toolStripMenuItemSpanish.Click += toolStripMenuItemSpanish_Click;
            // 
            // invariantCultureToolStripMenuItem
            // 
            invariantCultureToolStripMenuItem.Image = Properties.Resources.NoFlag;
            invariantCultureToolStripMenuItem.Name = "invariantCultureToolStripMenuItem";
            invariantCultureToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            invariantCultureToolStripMenuItem.Text = "Invariant culture";
            invariantCultureToolStripMenuItem.Click += invariantCultureToolStripMenuItem_Click;
            // 
            // checkBoxUseDefaultDuplicateCheck
            // 
            checkBoxUseDefaultDuplicateCheck.AutoSize = true;
            checkBoxUseDefaultDuplicateCheck.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxUseDefaultDuplicateCheck.Location = new System.Drawing.Point(64, 84);
            checkBoxUseDefaultDuplicateCheck.Margin = new System.Windows.Forms.Padding(0);
            checkBoxUseDefaultDuplicateCheck.Name = "checkBoxUseDefaultDuplicateCheck";
            checkBoxUseDefaultDuplicateCheck.Size = new System.Drawing.Size(284, 19);
            checkBoxUseDefaultDuplicateCheck.TabIndex = 19;
            checkBoxUseDefaultDuplicateCheck.Text = "Use default duplicate check";
            checkBoxUseDefaultDuplicateCheck.UseVisualStyleBackColor = true;
            checkBoxUseDefaultDuplicateCheck.Visible = false;
            checkBoxUseDefaultDuplicateCheck.Click += checkBoxUseDefaultDuplicateCheck_Click;
            // 
            // labelSeparator
            // 
            labelSeparator.AutoSize = true;
            labelSeparator.Dock = System.Windows.Forms.DockStyle.Top;
            labelSeparator.Location = new System.Drawing.Point(352, 66);
            labelSeparator.Margin = new System.Windows.Forms.Padding(4, 7, 0, 0);
            labelSeparator.Name = "labelSeparator";
            tableLayoutPanel.SetRowSpan(labelSeparator, 2);
            labelSeparator.Size = new System.Drawing.Size(60, 15);
            labelSeparator.TabIndex = 21;
            labelSeparator.Text = "Separator:";
            labelSeparator.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxSeparator
            // 
            comboBoxSeparator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxSeparator.FormattingEnabled = true;
            comboBoxSeparator.Location = new System.Drawing.Point(412, 59);
            comboBoxSeparator.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            comboBoxSeparator.Name = "comboBoxSeparator";
            tableLayoutPanel.SetRowSpan(comboBoxSeparator, 2);
            comboBoxSeparator.Size = new System.Drawing.Size(66, 23);
            comboBoxSeparator.TabIndex = 22;
            comboBoxSeparator.SelectionChangeCommitted += comboBoxSeparator_SelectionChangeCommitted;
            // 
            // tableLayoutPanelSchema
            // 
            tableLayoutPanelSchema.ColumnCount = 3;
            tableLayoutPanel.SetColumnSpan(tableLayoutPanelSchema, 8);
            tableLayoutPanelSchema.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSchema.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelSchema.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSchema.Controls.Add(textBoxSchema, 1, 0);
            tableLayoutPanelSchema.Controls.Add(toolStripSchema, 2, 0);
            tableLayoutPanelSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelSchema.Location = new System.Drawing.Point(64, 133);
            tableLayoutPanelSchema.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanelSchema.Name = "tableLayoutPanelSchema";
            tableLayoutPanelSchema.RowCount = 1;
            tableLayoutPanelSchema.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelSchema.Size = new System.Drawing.Size(705, 29);
            tableLayoutPanelSchema.TabIndex = 23;
            // 
            // toolStripSchema
            // 
            toolStripSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripSchema.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripSchema.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonRemoveSchema, toolStripButtonOpenSchema, toolStripButtonShowSchema, toolStripDropDownButtonGitHub });
            toolStripSchema.Location = new System.Drawing.Point(604, 0);
            toolStripSchema.Name = "toolStripSchema";
            toolStripSchema.Size = new System.Drawing.Size(101, 29);
            toolStripSchema.TabIndex = 20;
            toolStripSchema.Text = "toolStrip1";
            // 
            // toolStripButtonRemoveSchema
            // 
            toolStripButtonRemoveSchema.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonRemoveSchema.Image = Properties.Resources.Delete;
            toolStripButtonRemoveSchema.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonRemoveSchema.Name = "toolStripButtonRemoveSchema";
            toolStripButtonRemoveSchema.Size = new System.Drawing.Size(23, 26);
            toolStripButtonRemoveSchema.Text = "Remove the selected schema and reset all settings";
            toolStripButtonRemoveSchema.Click += toolStripButtonRemoveSchema_Click;
            // 
            // toolStripButtonOpenSchema
            // 
            toolStripButtonOpenSchema.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonOpenSchema.Image = ResourceWorkbench.Open;
            toolStripButtonOpenSchema.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonOpenSchema.Name = "toolStripButtonOpenSchema";
            toolStripButtonOpenSchema.Size = new System.Drawing.Size(23, 26);
            toolStripButtonOpenSchema.Text = "Open a predefined schema";
            toolStripButtonOpenSchema.Click += toolStripButtonOpenSchema_Click;
            // 
            // toolStripButtonShowSchema
            // 
            toolStripButtonShowSchema.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonShowSchema.Image = Properties.Resources.Lupe;
            toolStripButtonShowSchema.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonShowSchema.Name = "toolStripButtonShowSchema";
            toolStripButtonShowSchema.Size = new System.Drawing.Size(23, 26);
            toolStripButtonShowSchema.Text = "Open the schema in a separate window";
            toolStripButtonShowSchema.Click += toolStripButtonShowSchema_Click;
            // 
            // toolStripDropDownButtonGitHub
            // 
            toolStripDropDownButtonGitHub.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripDropDownButtonGitHub.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { sNSBToolStripMenuItem, zFMKToolStripMenuItem });
            toolStripDropDownButtonGitHub.Image = Properties.Resources.Github;
            toolStripDropDownButtonGitHub.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButtonGitHub.Name = "toolStripDropDownButtonGitHub";
            toolStripDropDownButtonGitHub.Size = new System.Drawing.Size(29, 26);
            toolStripDropDownButtonGitHub.Text = "Search for schema files on GitHub";
            // 
            // sNSBToolStripMenuItem
            // 
            sNSBToolStripMenuItem.Image = Properties.Resources.SNSB;
            sNSBToolStripMenuItem.Name = "sNSBToolStripMenuItem";
            sNSBToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            sNSBToolStripMenuItem.Text = "SNSB";
            sNSBToolStripMenuItem.ToolTipText = "Schema files provided by the Staatliche Naturwissenschaftliche Sammlungen Bayerns";
            sNSBToolStripMenuItem.Click += sNSBToolStripMenuItem_Click;
            // 
            // zFMKToolStripMenuItem
            // 
            zFMKToolStripMenuItem.Image = Properties.Resources.ZFMK;
            zFMKToolStripMenuItem.Name = "zFMKToolStripMenuItem";
            zFMKToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            zFMKToolStripMenuItem.Text = "ZFMK";
            zFMKToolStripMenuItem.ToolTipText = "Schema files provided by the Zoologische Forschungsmuseum Alexander Koenig";
            zFMKToolStripMenuItem.Click += zFMKToolStripMenuItem_Click;
            // 
            // checkBoxIncludePreAndPostfix
            // 
            checkBoxIncludePreAndPostfix.AutoSize = true;
            tableLayoutPanel.SetColumnSpan(checkBoxIncludePreAndPostfix, 3);
            checkBoxIncludePreAndPostfix.Dock = System.Windows.Forms.DockStyle.Top;
            checkBoxIncludePreAndPostfix.Location = new System.Drawing.Point(64, 103);
            checkBoxIncludePreAndPostfix.Margin = new System.Windows.Forms.Padding(0);
            checkBoxIncludePreAndPostfix.Name = "checkBoxIncludePreAndPostfix";
            checkBoxIncludePreAndPostfix.Size = new System.Drawing.Size(418, 19);
            checkBoxIncludePreAndPostfix.TabIndex = 24;
            checkBoxIncludePreAndPostfix.Text = "Use transformed value for comparision of attachment";
            toolTip.SetToolTip(checkBoxIncludePreAndPostfix, "By default the not transformed value is not used for comparision with content in the database. Select this option to use the transformed value");
            checkBoxIncludePreAndPostfix.UseVisualStyleBackColor = true;
            checkBoxIncludePreAndPostfix.Visible = false;
            checkBoxIncludePreAndPostfix.Click += checkBoxIncludePreAndPostfix_Click;
            // 
            // checkBoxRecordSQL
            // 
            checkBoxRecordSQL.AutoSize = true;
            tableLayoutPanel.SetColumnSpan(checkBoxRecordSQL, 4);
            checkBoxRecordSQL.Dock = System.Windows.Forms.DockStyle.Right;
            checkBoxRecordSQL.Image = Properties.Resources.DatabaseOverview;
            checkBoxRecordSQL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            checkBoxRecordSQL.Location = new System.Drawing.Point(563, 103);
            checkBoxRecordSQL.Margin = new System.Windows.Forms.Padding(0);
            checkBoxRecordSQL.Name = "checkBoxRecordSQL";
            checkBoxRecordSQL.Size = new System.Drawing.Size(179, 30);
            checkBoxRecordSQL.TabIndex = 25;
            checkBoxRecordSQL.Text = "Record all SQL statements";
            checkBoxRecordSQL.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            checkBoxRecordSQL.UseVisualStyleBackColor = true;
            checkBoxRecordSQL.CheckedChanged += checkBoxRecordSQL_CheckedChanged;
            // 
            // buttonOpenRecordSQL
            // 
            buttonOpenRecordSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonOpenRecordSQL.FlatAppearance.BorderSize = 0;
            buttonOpenRecordSQL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonOpenRecordSQL.Image = ResourceWorkbench.Open;
            buttonOpenRecordSQL.Location = new System.Drawing.Point(742, 103);
            buttonOpenRecordSQL.Margin = new System.Windows.Forms.Padding(0);
            buttonOpenRecordSQL.Name = "buttonOpenRecordSQL";
            buttonOpenRecordSQL.Size = new System.Drawing.Size(27, 30);
            buttonOpenRecordSQL.TabIndex = 26;
            toolTip.SetToolTip(buttonOpenRecordSQL, "Open SQL statements");
            buttonOpenRecordSQL.UseVisualStyleBackColor = true;
            buttonOpenRecordSQL.Click += buttonOpenRecordSQL_Click;
            // 
            // checkBoxTranslateReturn
            // 
            checkBoxTranslateReturn.AutoSize = true;
            tableLayoutPanel.SetColumnSpan(checkBoxTranslateReturn, 4);
            checkBoxTranslateReturn.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxTranslateReturn.Location = new System.Drawing.Point(546, 84);
            checkBoxTranslateReturn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            checkBoxTranslateReturn.Name = "checkBoxTranslateReturn";
            checkBoxTranslateReturn.Size = new System.Drawing.Size(219, 19);
            checkBoxTranslateReturn.TabIndex = 27;
            checkBoxTranslateReturn.Text = "Translate \\r\\n to line break";
            checkBoxTranslateReturn.UseVisualStyleBackColor = true;
            checkBoxTranslateReturn.CheckedChanged += checkBoxTranslateReturn_CheckedChanged;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog";
            // 
            // imageListLanguage
            // 
            imageListLanguage.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListLanguage.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListLanguage.ImageStream");
            imageListLanguage.TransparentColor = System.Drawing.Color.Transparent;
            imageListLanguage.Images.SetKeyName(0, "Deutsch.ico");
            imageListLanguage.Images.SetKeyName(1, "USA.ico");
            imageListLanguage.Images.SetKeyName(2, "English.ico");
            imageListLanguage.Images.SetKeyName(3, "Frankreich.ico");
            imageListLanguage.Images.SetKeyName(4, "Italien.ico");
            imageListLanguage.Images.SetKeyName(5, "Spanien.ico");
            // 
            // UserControlFile
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "UserControlFile";
            Size = new System.Drawing.Size(769, 361);
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownStartLine).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownEndLine).EndInit();
            contextMenuStripOpenFile.ResumeLayout(false);
            toolStripLanguage.ResumeLayout(false);
            toolStripLanguage.PerformLayout();
            tableLayoutPanelSchema.ResumeLayout(false);
            tableLayoutPanelSchema.PerformLayout();
            toolStripSchema.ResumeLayout(false);
            toolStripSchema.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        public System.Windows.Forms.TextBox textBoxFile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        public System.Windows.Forms.TextBox textBoxSchema;
        private System.Windows.Forms.Label labelFile;
        private System.Windows.Forms.Label labelSchema;
        private System.Windows.Forms.Label labelEncoding;
        public System.Windows.Forms.ComboBox comboBoxEncoding;
        private System.Windows.Forms.Label labelStartLine;
        public System.Windows.Forms.NumericUpDown numericUpDownStartLine;
        private System.Windows.Forms.Label labelEndLine;
        public System.Windows.Forms.NumericUpDown numericUpDownEndLine;
        public System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.CheckBox checkBoxFirstLine;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.WebBrowser webBrowserSchema;
        private System.Windows.Forms.ImageList imageListLanguage;
        private System.Windows.Forms.Label labelLanguage;
        private System.Windows.Forms.ToolStrip toolStripLanguage;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonLanguage;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGerman;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEnglish;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSpanish;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFrensh;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemItaly;
        private System.Windows.Forms.ToolStripMenuItem uSAToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox checkBoxUseDefaultDuplicateCheck;
        private System.Windows.Forms.ToolStripMenuItem invariantCultureToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStripSchema;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemoveSchema;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenSchema;
        private System.Windows.Forms.ToolStripButton toolStripButtonShowSchema;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonGitHub;
        private System.Windows.Forms.ToolStripMenuItem sNSBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zFMKToolStripMenuItem;
        private System.Windows.Forms.Label labelSeparator;
        private System.Windows.Forms.ComboBox comboBoxSeparator;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSchema;
        private System.Windows.Forms.CheckBox checkBoxIncludePreAndPostfix;
        private System.Windows.Forms.CheckBox checkBoxRecordSQL;
        private System.Windows.Forms.Button buttonOpenRecordSQL;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripOpenFile;
        private System.Windows.Forms.ToolStripMenuItem adaptDefaultDirectoryToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxTranslateReturn;
    }
}
