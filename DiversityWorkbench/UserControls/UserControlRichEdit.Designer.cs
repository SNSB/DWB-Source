
namespace DiversityWorkbench.UserControls
{
    partial class UserControlRichEdit
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
        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelType = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelLen = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelLength = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelGap = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelGapSym = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelDisplay = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelPosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelInsert = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxSymbol = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButtonInsert = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonBold = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonItalic = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonUnderline = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonWeb = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButtonDatabase = new System.Windows.Forms.ToolStripDropDownButton();
            this.dAgentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dCollectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dDescriptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dExsciccataeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dGazetteersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dProjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dReferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dSamplingPlotsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dScientificTermsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dTaxonNamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonReload = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonScanRtf = new System.Windows.Forms.ToolStripButton();
            this.richTextBoxCvt = new System.Windows.Forms.RichTextBox();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanelMain.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.Controls.Add(this.statusStrip, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.richTextBox, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.toolStrip, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.richTextBoxCvt, 0, 3);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 5;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(443, 300);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // statusStrip
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.statusStrip, 2);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelType,
            this.toolStripStatusLabelLen,
            this.toolStripStatusLabelLength,
            this.toolStripStatusLabelGap,
            this.toolStripStatusLabelGapSym,
            this.toolStripStatusLabelDisplay,
            this.toolStripStatusLabelPos,
            this.toolStripStatusLabelPosition});
            this.statusStrip.Location = new System.Drawing.Point(3, 275);
            this.statusStrip.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.ShowItemToolTips = true;
            this.statusStrip.Size = new System.Drawing.Size(437, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabelType
            // 
            this.toolStripStatusLabelType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabelType.Name = "toolStripStatusLabelType";
            this.toolStripStatusLabelType.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabelType.Text = "Nucleotide";
            this.toolStripStatusLabelType.ToolTipText = "Molecular sequence type";
            // 
            // toolStripStatusLabelLen
            // 
            this.toolStripStatusLabelLen.Name = "toolStripStatusLabelLen";
            this.toolStripStatusLabelLen.Size = new System.Drawing.Size(29, 17);
            this.toolStripStatusLabelLen.Text = "Len:";
            this.toolStripStatusLabelLen.ToolTipText = "Symbol length";
            // 
            // toolStripStatusLabelLength
            // 
            this.toolStripStatusLabelLength.Name = "toolStripStatusLabelLength";
            this.toolStripStatusLabelLength.Size = new System.Drawing.Size(13, 17);
            this.toolStripStatusLabelLength.Text = "1";
            this.toolStripStatusLabelLength.ToolTipText = "Symbol length";
            // 
            // toolStripStatusLabelGap
            // 
            this.toolStripStatusLabelGap.Name = "toolStripStatusLabelGap";
            this.toolStripStatusLabelGap.Size = new System.Drawing.Size(31, 17);
            this.toolStripStatusLabelGap.Text = "Gap:";
            this.toolStripStatusLabelGap.ToolTipText = "Gap symbol";
            // 
            // toolStripStatusLabelGapSym
            // 
            this.toolStripStatusLabelGapSym.Name = "toolStripStatusLabelGapSym";
            this.toolStripStatusLabelGapSym.Size = new System.Drawing.Size(12, 17);
            this.toolStripStatusLabelGapSym.Text = "-";
            this.toolStripStatusLabelGapSym.ToolTipText = "Gap symbol";
            // 
            // toolStripStatusLabelDisplay
            // 
            this.toolStripStatusLabelDisplay.Name = "toolStripStatusLabelDisplay";
            this.toolStripStatusLabelDisplay.Size = new System.Drawing.Size(227, 17);
            this.toolStripStatusLabelDisplay.Spring = true;
            // 
            // toolStripStatusLabelPos
            // 
            this.toolStripStatusLabelPos.Name = "toolStripStatusLabelPos";
            this.toolStripStatusLabelPos.Size = new System.Drawing.Size(29, 17);
            this.toolStripStatusLabelPos.Text = "Pos:";
            this.toolStripStatusLabelPos.ToolTipText = "Actual position within text";
            // 
            // toolStripStatusLabelPosition
            // 
            this.toolStripStatusLabelPosition.Name = "toolStripStatusLabelPosition";
            this.toolStripStatusLabelPosition.Size = new System.Drawing.Size(13, 17);
            this.toolStripStatusLabelPosition.Text = "1";
            this.toolStripStatusLabelPosition.ToolTipText = "Actual position within text";
            // 
            // richTextBox
            // 
            this.richTextBox.AcceptsTab = true;
            this.richTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelMain.SetColumnSpan(this.richTextBox, 2);
            this.richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox.Location = new System.Drawing.Point(3, 28);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(437, 212);
            this.richTextBox.TabIndex = 1;
            this.richTextBox.Text = "";
            this.richTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox_LinkClicked);
            this.richTextBox.SelectionChanged += new System.EventHandler(this.richTextBox_SelectionChanged);
            this.richTextBox.TextChanged += new System.EventHandler(this.richTextBox_TextChanged);
            this.richTextBox.DoubleClick += new System.EventHandler(this.richTextBox_DoubleClick);
            this.richTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.richTextBox_KeyPress);
            // 
            // toolStrip
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.toolStrip, 2);
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelInsert,
            this.toolStripComboBoxSymbol,
            this.toolStripButtonInsert,
            this.toolStripButtonBold,
            this.toolStripButtonItalic,
            this.toolStripButtonUnderline,
            this.toolStripSeparator1,
            this.toolStripButtonWeb,
            this.toolStripDropDownButtonDatabase,
            this.toolStripSeparator2,
            this.toolStripButtonReload,
            this.toolStripButtonScanRtf});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(443, 25);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.TabStop = true;
            this.toolStrip.Text = "toolStrip";
            // 
            // toolStripLabelInsert
            // 
            this.toolStripLabelInsert.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripLabelInsert.Name = "toolStripLabelInsert";
            this.toolStripLabelInsert.Size = new System.Drawing.Size(81, 22);
            this.toolStripLabelInsert.Text = "Insert symbol:";
            // 
            // toolStripComboBoxSymbol
            // 
            this.toolStripComboBoxSymbol.Name = "toolStripComboBoxSymbol";
            this.toolStripComboBoxSymbol.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBoxSymbol.ToolTipText = "Select the symbol for insert";
            this.toolStripComboBoxSymbol.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxSymbol_SelectedIndexChanged);
            this.toolStripComboBoxSymbol.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripComboBoxSymbol_KeyPress);
            // 
            // toolStripButtonInsert
            // 
            this.toolStripButtonInsert.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonInsert.Image = global::DiversityWorkbench.Properties.Resources.Append;
            this.toolStripButtonInsert.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonInsert.Name = "toolStripButtonInsert";
            this.toolStripButtonInsert.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonInsert.Text = "Insert selected symbol";
            this.toolStripButtonInsert.Click += new System.EventHandler(this.toolStripButtonInsert_Click);
            // 
            // toolStripButtonBold
            // 
            this.toolStripButtonBold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonBold.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripButtonBold.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonBold.Name = "toolStripButtonBold";
            this.toolStripButtonBold.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonBold.Text = "B";
            this.toolStripButtonBold.ToolTipText = "Bold";
            this.toolStripButtonBold.Click += new System.EventHandler(this.toolStripButtonBold_Click);
            // 
            // toolStripButtonItalic
            // 
            this.toolStripButtonItalic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonItalic.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Italic);
            this.toolStripButtonItalic.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonItalic.Name = "toolStripButtonItalic";
            this.toolStripButtonItalic.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonItalic.Text = "I";
            this.toolStripButtonItalic.ToolTipText = "Italic";
            this.toolStripButtonItalic.Click += new System.EventHandler(this.toolStripButtonItalic_Click);
            // 
            // toolStripButtonUnderline
            // 
            this.toolStripButtonUnderline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonUnderline.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Underline);
            this.toolStripButtonUnderline.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUnderline.Name = "toolStripButtonUnderline";
            this.toolStripButtonUnderline.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonUnderline.Text = "U";
            this.toolStripButtonUnderline.ToolTipText = "Underline";
            this.toolStripButtonUnderline.Click += new System.EventHandler(this.toolStripButtonUnderline_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonWeb
            // 
            this.toolStripButtonWeb.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonWeb.Image = global::DiversityWorkbench.Properties.Resources.Browse;
            this.toolStripButtonWeb.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonWeb.Name = "toolStripButtonWeb";
            this.toolStripButtonWeb.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonWeb.Text = "Insert an internet link";
            this.toolStripButtonWeb.Click += new System.EventHandler(this.toolStripButtonWeb_Click);
            // 
            // toolStripDropDownButtonDatabase
            // 
            this.toolStripDropDownButtonDatabase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonDatabase.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dAgentsToolStripMenuItem,
            this.dCollectionToolStripMenuItem,
            this.dDescriptionsToolStripMenuItem,
            this.dExsciccataeToolStripMenuItem,
            this.dGazetteersToolStripMenuItem,
            this.dProjectsToolStripMenuItem,
            this.dReferencesToolStripMenuItem,
            this.dSamplingPlotsToolStripMenuItem,
            this.dScientificTermsToolStripMenuItem,
            this.dTaxonNamesToolStripMenuItem});
            this.toolStripDropDownButtonDatabase.Image = global::DiversityWorkbench.Properties.Resources.DiversityWorkbench_3;
            this.toolStripDropDownButtonDatabase.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonDatabase.Name = "toolStripDropDownButtonDatabase";
            this.toolStripDropDownButtonDatabase.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButtonDatabase.Text = "Insert a database link";
            this.toolStripDropDownButtonDatabase.ToolTipText = "Insert a database link";
            // 
            // dAgentsToolStripMenuItem
            // 
            this.dAgentsToolStripMenuItem.Name = "dAgentsToolStripMenuItem";
            this.dAgentsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dAgentsToolStripMenuItem.Text = "Agents ...";
            this.dAgentsToolStripMenuItem.Click += new System.EventHandler(this.dAgentsToolStripMenuItem_Click);
            // 
            // dCollectionToolStripMenuItem
            // 
            this.dCollectionToolStripMenuItem.Name = "dCollectionToolStripMenuItem";
            this.dCollectionToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dCollectionToolStripMenuItem.Text = "Collection ...";
            this.dCollectionToolStripMenuItem.Click += new System.EventHandler(this.dCollectionToolStripMenuItem_Click);
            // 
            // dDescriptionsToolStripMenuItem
            // 
            this.dDescriptionsToolStripMenuItem.Name = "dDescriptionsToolStripMenuItem";
            this.dDescriptionsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dDescriptionsToolStripMenuItem.Text = "Descriptions ...";
            this.dDescriptionsToolStripMenuItem.Click += new System.EventHandler(this.dDescriptionsToolStripMenuItem_Click);
            // 
            // dExsciccataeToolStripMenuItem
            // 
            this.dExsciccataeToolStripMenuItem.Name = "dExsciccataeToolStripMenuItem";
            this.dExsciccataeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dExsciccataeToolStripMenuItem.Text = "Exsiccatae ...";
            this.dExsciccataeToolStripMenuItem.Click += new System.EventHandler(this.dExsciccataeToolStripMenuItem_Click);
            // 
            // dGazetteersToolStripMenuItem
            // 
            this.dGazetteersToolStripMenuItem.Name = "dGazetteersToolStripMenuItem";
            this.dGazetteersToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dGazetteersToolStripMenuItem.Text = "Gazetteers ...";
            this.dGazetteersToolStripMenuItem.Click += new System.EventHandler(this.dGazetteersToolStripMenuItem_Click);
            // 
            // dProjectsToolStripMenuItem
            // 
            this.dProjectsToolStripMenuItem.Name = "dProjectsToolStripMenuItem";
            this.dProjectsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dProjectsToolStripMenuItem.Text = "Projects ...";
            this.dProjectsToolStripMenuItem.Click += new System.EventHandler(this.dProjectsToolStripMenuItem_Click);
            // 
            // dReferencesToolStripMenuItem
            // 
            this.dReferencesToolStripMenuItem.Name = "dReferencesToolStripMenuItem";
            this.dReferencesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dReferencesToolStripMenuItem.Text = "References ...";
            this.dReferencesToolStripMenuItem.Click += new System.EventHandler(this.dReferencesToolStripMenuItem_Click);
            // 
            // dSamplingPlotsToolStripMenuItem
            // 
            this.dSamplingPlotsToolStripMenuItem.Name = "dSamplingPlotsToolStripMenuItem";
            this.dSamplingPlotsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dSamplingPlotsToolStripMenuItem.Text = "Sampling plots ...";
            this.dSamplingPlotsToolStripMenuItem.Click += new System.EventHandler(this.dSamplingPlotsToolStripMenuItem_Click);
            // 
            // dScientificTermsToolStripMenuItem
            // 
            this.dScientificTermsToolStripMenuItem.Name = "dScientificTermsToolStripMenuItem";
            this.dScientificTermsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dScientificTermsToolStripMenuItem.Text = "Scientific terms ...";
            this.dScientificTermsToolStripMenuItem.Click += new System.EventHandler(this.dScientificTermsToolStripMenuItem_Click);
            // 
            // dTaxonNamesToolStripMenuItem
            // 
            this.dTaxonNamesToolStripMenuItem.Name = "dTaxonNamesToolStripMenuItem";
            this.dTaxonNamesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dTaxonNamesToolStripMenuItem.Text = "Taxon names ...";
            this.dTaxonNamesToolStripMenuItem.Click += new System.EventHandler(this.dTaxonNamesToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonReload
            // 
            this.toolStripButtonReload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonReload.Image = global::DiversityWorkbench.Properties.Resources.Reload;
            this.toolStripButtonReload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReload.Name = "toolStripButtonReload";
            this.toolStripButtonReload.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonReload.Text = "Reload original text";
            this.toolStripButtonReload.ToolTipText = "Reload original text";
            this.toolStripButtonReload.Click += new System.EventHandler(this.toolStripButtonReload_Click);
            // 
            // toolStripButtonScanRtf
            // 
            this.toolStripButtonScanRtf.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonScanRtf.Image = global::DiversityWorkbench.Properties.Resources.ScanRTF;
            this.toolStripButtonScanRtf.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonScanRtf.Name = "toolStripButtonScanRtf";
            this.toolStripButtonScanRtf.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonScanRtf.Text = "Convert RTF format tags";
            this.toolStripButtonScanRtf.ToolTipText = "Convert RTF format tags";
            this.toolStripButtonScanRtf.Click += new System.EventHandler(this.toolStripButtonScanRtf_Click);
            // 
            // richTextBoxCvt
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.richTextBoxCvt, 2);
            this.richTextBoxCvt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxCvt.Location = new System.Drawing.Point(3, 246);
            this.richTextBoxCvt.Name = "richTextBoxCvt";
            this.richTextBoxCvt.Size = new System.Drawing.Size(437, 26);
            this.richTextBoxCvt.TabIndex = 3;
            this.richTextBoxCvt.Text = "";
            this.richTextBoxCvt.Visible = false;
            // 
            // UserControlRichEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "UserControlRichEdit";
            this.Size = new System.Drawing.Size(443, 300);
            this.BackColorChanged += new System.EventHandler(this.UserControlRichEdit_BackColorChanged);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelType;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelLen;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelLength;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelDisplay;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelPos;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelPosition;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelGap;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelGapSym;
        private System.Windows.Forms.ToolStripLabel toolStripLabelInsert;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxSymbol;
        private System.Windows.Forms.ToolStripButton toolStripButtonInsert;
        private System.Windows.Forms.RichTextBox richTextBoxCvt;
        private System.Windows.Forms.ToolStripButton toolStripButtonBold;
        private System.Windows.Forms.ToolStripButton toolStripButtonItalic;
        private System.Windows.Forms.ToolStripButton toolStripButtonUnderline;
        private System.Windows.Forms.ToolStripButton toolStripButtonScanRtf;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonReload;
        private System.Windows.Forms.ToolStripButton toolStripButtonWeb;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonDatabase;
        private System.Windows.Forms.ToolStripMenuItem dAgentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dCollectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dDescriptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dExsciccataeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dGazetteersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dProjectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dReferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dSamplingPlotsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dScientificTermsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dTaxonNamesToolStripMenuItem;
    }
}
