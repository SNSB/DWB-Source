namespace DiversityCollection.UserControls
{
    partial class UserControl_Print
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_Print));
            this.tableLayoutPanelLabel = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripLabel = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNewSchemaFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonOpenSchemaFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButtonGitHub = new System.Windows.Forms.ToolStripDropDownButton();
            this.sNSBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zFMKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorLabel = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonLabelPreview = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLabelMulti = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBoxPrintDuplicates = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparatorPrint = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonLabelPrintBrowserOptinos = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPageSetup = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLabelExport = new System.Windows.Forms.ToolStripButton();
            this.textBoxSchemaFile = new System.Windows.Forms.TextBox();
            this.labelSchemaFile = new System.Windows.Forms.Label();
            this.textBoxReportTitle = new System.Windows.Forms.TextBox();
            this.labelReportTitle = new System.Windows.Forms.Label();
            this.checkBoxUseStockForLabelDuplicates = new System.Windows.Forms.CheckBox();
            this.checkBoxPrintRestrictToMaterial = new System.Windows.Forms.CheckBox();
            this.checkBoxPrintRestrictToCollection = new System.Windows.Forms.CheckBox();
            this.pictureBoxPrintRestrictToMaterial = new System.Windows.Forms.PictureBox();
            this.labelLabelConversion = new System.Windows.Forms.Label();
            this.comboBoxLabelConversion = new System.Windows.Forms.ComboBox();
            this.buttonLabelQRcode = new System.Windows.Forms.Button();
            this.contextMenuStripQR = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setServiceTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comboBoxLabelQRcode = new System.Windows.Forms.ComboBox();
            this.pictureBoxLabelQRcodeSource = new System.Windows.Forms.PictureBox();
            this.comboBoxLabelQRcodeType = new System.Windows.Forms.ComboBox();
            this.checkBoxLabelQRcode = new System.Windows.Forms.CheckBox();
            this.panelWebbrowserLabel = new System.Windows.Forms.Panel();
            this.webBrowserLabel = new System.Windows.Forms.WebBrowser();
            this.openFileDialogLabelSchema = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.setSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanelLabel.SuspendLayout();
            this.toolStripLabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPrintRestrictToMaterial)).BeginInit();
            this.contextMenuStripQR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLabelQRcodeSource)).BeginInit();
            this.panelWebbrowserLabel.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // tableLayoutPanelLabel
            // 
            this.tableLayoutPanelLabel.ColumnCount = 9;
            this.tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanelLabel.Controls.Add(this.toolStripLabel, 7, 0);
            this.tableLayoutPanelLabel.Controls.Add(this.textBoxSchemaFile, 1, 0);
            this.tableLayoutPanelLabel.Controls.Add(this.labelSchemaFile, 0, 0);
            this.tableLayoutPanelLabel.Controls.Add(this.textBoxReportTitle, 1, 1);
            this.tableLayoutPanelLabel.Controls.Add(this.labelReportTitle, 0, 1);
            this.tableLayoutPanelLabel.Controls.Add(this.checkBoxUseStockForLabelDuplicates, 7, 1);
            this.tableLayoutPanelLabel.Controls.Add(this.checkBoxPrintRestrictToMaterial, 7, 2);
            this.tableLayoutPanelLabel.Controls.Add(this.checkBoxPrintRestrictToCollection, 5, 1);
            this.tableLayoutPanelLabel.Controls.Add(this.pictureBoxPrintRestrictToMaterial, 8, 2);
            this.tableLayoutPanelLabel.Controls.Add(this.labelLabelConversion, 0, 2);
            this.tableLayoutPanelLabel.Controls.Add(this.comboBoxLabelConversion, 1, 2);
            this.tableLayoutPanelLabel.Controls.Add(this.buttonLabelQRcode, 2, 2);
            this.tableLayoutPanelLabel.Controls.Add(this.comboBoxLabelQRcode, 4, 2);
            this.tableLayoutPanelLabel.Controls.Add(this.pictureBoxLabelQRcodeSource, 5, 2);
            this.tableLayoutPanelLabel.Controls.Add(this.comboBoxLabelQRcodeType, 6, 2);
            this.tableLayoutPanelLabel.Controls.Add(this.checkBoxLabelQRcode, 3, 2);
            this.tableLayoutPanelLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanelLabel.Location = new System.Drawing.Point(0, 293);
            this.tableLayoutPanelLabel.Name = "tableLayoutPanelLabel";
            this.tableLayoutPanelLabel.RowCount = 3;
            this.tableLayoutPanelLabel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanelLabel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLabel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLabel.Size = new System.Drawing.Size(891, 71);
            this.tableLayoutPanelLabel.TabIndex = 1;
            // 
            // toolStripLabel
            // 
            this.tableLayoutPanelLabel.SetColumnSpan(this.toolStripLabel, 2);
            this.toolStripLabel.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripLabel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNewSchemaFile,
            this.toolStripButtonOpenSchemaFile,
            this.toolStripDropDownButtonGitHub,
            this.toolStripSeparatorLabel,
            this.toolStripButtonLabelPreview,
            this.toolStripButtonLabelMulti,
            this.toolStripTextBoxPrintDuplicates,
            this.toolStripSeparatorPrint,
            this.toolStripButtonLabelPrintBrowserOptinos,
            this.toolStripButtonPageSetup,
            this.toolStripButtonPrint,
            this.toolStripButtonLabelExport});
            this.toolStripLabel.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStripLabel.Location = new System.Drawing.Point(651, 0);
            this.toolStripLabel.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStripLabel.Name = "toolStripLabel";
            this.toolStripLabel.Size = new System.Drawing.Size(237, 25);
            this.toolStripLabel.TabIndex = 0;
            // 
            // toolStripButtonNewSchemaFile
            // 
            this.toolStripButtonNewSchemaFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNewSchemaFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNewSchemaFile.Image")));
            this.toolStripButtonNewSchemaFile.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonNewSchemaFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNewSchemaFile.Name = "toolStripButtonNewSchemaFile";
            this.toolStripButtonNewSchemaFile.Size = new System.Drawing.Size(29, 20);
            this.toolStripButtonNewSchemaFile.Text = "Create a new schema file";
            this.toolStripButtonNewSchemaFile.Visible = false;
            // 
            // toolStripButtonOpenSchemaFile
            // 
            this.toolStripButtonOpenSchemaFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOpenSchemaFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpenSchemaFile.Image")));
            this.toolStripButtonOpenSchemaFile.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonOpenSchemaFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpenSchemaFile.Name = "toolStripButtonOpenSchemaFile";
            this.toolStripButtonOpenSchemaFile.Size = new System.Drawing.Size(29, 20);
            this.toolStripButtonOpenSchemaFile.Text = "Select schema file (XSD)";
            this.toolStripButtonOpenSchemaFile.Click += new System.EventHandler(this.toolStripButtonOpenSchemaFile_Click);
            // 
            // toolStripDropDownButtonGitHub
            // 
            this.toolStripDropDownButtonGitHub.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonGitHub.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sNSBToolStripMenuItem,
            this.zFMKToolStripMenuItem});
            this.toolStripDropDownButtonGitHub.Image = global::DiversityCollection.Resource.Github;
            this.toolStripDropDownButtonGitHub.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripDropDownButtonGitHub.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonGitHub.Name = "toolStripDropDownButtonGitHub";
            this.toolStripDropDownButtonGitHub.Size = new System.Drawing.Size(30, 20);
            this.toolStripDropDownButtonGitHub.Text = "Search for schema files on GitHub";
            // 
            // sNSBToolStripMenuItem
            // 
            this.sNSBToolStripMenuItem.Image = global::DiversityCollection.Resource.SNSB;
            this.sNSBToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.sNSBToolStripMenuItem.Name = "sNSBToolStripMenuItem";
            this.sNSBToolStripMenuItem.Size = new System.Drawing.Size(130, 26);
            this.sNSBToolStripMenuItem.Text = "SNSB";
            this.sNSBToolStripMenuItem.ToolTipText = "Schema files provided by the Staatliche Naturwissenschaftliche Sammlungen Bayerns" +
    "";
            this.sNSBToolStripMenuItem.Click += new System.EventHandler(this.sNSBToolStripMenuItem_Click);
            // 
            // zFMKToolStripMenuItem
            // 
            this.zFMKToolStripMenuItem.Image = global::DiversityCollection.Resource.ZFMK;
            this.zFMKToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.zFMKToolStripMenuItem.Name = "zFMKToolStripMenuItem";
            this.zFMKToolStripMenuItem.Size = new System.Drawing.Size(130, 26);
            this.zFMKToolStripMenuItem.Text = "ZFMK";
            this.zFMKToolStripMenuItem.ToolTipText = "Schema files provided by the Zoologische Forschungsmuseum Alexander Koenig";
            this.zFMKToolStripMenuItem.Click += new System.EventHandler(this.zFMKToolStripMenuItem_Click);
            // 
            // toolStripSeparatorLabel
            // 
            this.toolStripSeparatorLabel.Name = "toolStripSeparatorLabel";
            this.toolStripSeparatorLabel.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonLabelPreview
            // 
            this.toolStripButtonLabelPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLabelPreview.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLabelPreview.Image")));
            this.toolStripButtonLabelPreview.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonLabelPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLabelPreview.Name = "toolStripButtonLabelPreview";
            this.toolStripButtonLabelPreview.Size = new System.Drawing.Size(29, 20);
            this.toolStripButtonLabelPreview.Text = "Label preview";
            this.toolStripButtonLabelPreview.Click += new System.EventHandler(this.toolStripButtonLabelPreview_Click);
            // 
            // toolStripButtonLabelMulti
            // 
            this.toolStripButtonLabelMulti.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLabelMulti.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLabelMulti.Image")));
            this.toolStripButtonLabelMulti.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonLabelMulti.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLabelMulti.Name = "toolStripButtonLabelMulti";
            this.toolStripButtonLabelMulti.Size = new System.Drawing.Size(29, 20);
            this.toolStripButtonLabelMulti.Text = "Preview for all labels in query";
            this.toolStripButtonLabelMulti.Click += new System.EventHandler(this.toolStripButtonLabelMulti_Click);
            // 
            // toolStripTextBoxPrintDuplicates
            // 
            this.toolStripTextBoxPrintDuplicates.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBoxPrintDuplicates.Name = "toolStripTextBoxPrintDuplicates";
            this.toolStripTextBoxPrintDuplicates.Size = new System.Drawing.Size(18, 27);
            this.toolStripTextBoxPrintDuplicates.Text = "1";
            this.toolStripTextBoxPrintDuplicates.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolStripTextBoxPrintDuplicates.ToolTipText = "Number of duplicates that should be printed (1 - 99)";
            this.toolStripTextBoxPrintDuplicates.TextChanged += new System.EventHandler(this.toolStripTextBoxPrintDuplicates_TextChanged);
            // 
            // toolStripSeparatorPrint
            // 
            this.toolStripSeparatorPrint.Name = "toolStripSeparatorPrint";
            this.toolStripSeparatorPrint.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonLabelPrintBrowserOptinos
            // 
            this.toolStripButtonLabelPrintBrowserOptinos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLabelPrintBrowserOptinos.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLabelPrintBrowserOptinos.Image")));
            this.toolStripButtonLabelPrintBrowserOptinos.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonLabelPrintBrowserOptinos.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLabelPrintBrowserOptinos.Name = "toolStripButtonLabelPrintBrowserOptinos";
            this.toolStripButtonLabelPrintBrowserOptinos.Size = new System.Drawing.Size(29, 20);
            this.toolStripButtonLabelPrintBrowserOptinos.Text = "set browser options";
            this.toolStripButtonLabelPrintBrowserOptinos.Visible = false;
            // 
            // toolStripButtonPageSetup
            // 
            this.toolStripButtonPageSetup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPageSetup.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPageSetup.Image")));
            this.toolStripButtonPageSetup.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonPageSetup.ImageTransparentColor = System.Drawing.Color.Purple;
            this.toolStripButtonPageSetup.Name = "toolStripButtonPageSetup";
            this.toolStripButtonPageSetup.Size = new System.Drawing.Size(29, 20);
            this.toolStripButtonPageSetup.Text = "Page setup";
            this.toolStripButtonPageSetup.Click += new System.EventHandler(this.toolStripButtonPageSetup_Click);
            // 
            // toolStripButtonPrint
            // 
            this.toolStripButtonPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPrint.Image = global::DiversityCollection.Resource.Print1;
            this.toolStripButtonPrint.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPrint.Name = "toolStripButtonPrint";
            this.toolStripButtonPrint.Size = new System.Drawing.Size(29, 20);
            this.toolStripButtonPrint.Text = "Print label";
            this.toolStripButtonPrint.Click += new System.EventHandler(this.toolStripButtonPrint_Click);
            // 
            // toolStripButtonLabelExport
            // 
            this.toolStripButtonLabelExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLabelExport.Image = global::DiversityCollection.Resource.Save;
            this.toolStripButtonLabelExport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonLabelExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLabelExport.Name = "toolStripButtonLabelExport";
            this.toolStripButtonLabelExport.Size = new System.Drawing.Size(29, 18);
            this.toolStripButtonLabelExport.Text = "save print files";
            this.toolStripButtonLabelExport.Click += new System.EventHandler(this.toolStripButtonLabelExport_Click);
            // 
            // textBoxSchemaFile
            // 
            this.tableLayoutPanelLabel.SetColumnSpan(this.textBoxSchemaFile, 6);
            this.textBoxSchemaFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSchemaFile.Location = new System.Drawing.Point(78, 4);
            this.textBoxSchemaFile.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.textBoxSchemaFile.Name = "textBoxSchemaFile";
            this.textBoxSchemaFile.Size = new System.Drawing.Size(573, 20);
            this.textBoxSchemaFile.TabIndex = 1;
            // 
            // labelSchemaFile
            // 
            this.labelSchemaFile.AutoSize = true;
            this.labelSchemaFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSchemaFile.Location = new System.Drawing.Point(3, 0);
            this.labelSchemaFile.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelSchemaFile.Name = "labelSchemaFile";
            this.labelSchemaFile.Size = new System.Drawing.Size(75, 25);
            this.labelSchemaFile.TabIndex = 2;
            this.labelSchemaFile.Text = "Schema file:";
            this.labelSchemaFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxReportTitle
            // 
            this.tableLayoutPanelLabel.SetColumnSpan(this.textBoxReportTitle, 4);
            this.textBoxReportTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxReportTitle.Location = new System.Drawing.Point(78, 25);
            this.textBoxReportTitle.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxReportTitle.Name = "textBoxReportTitle";
            this.textBoxReportTitle.Size = new System.Drawing.Size(435, 20);
            this.textBoxReportTitle.TabIndex = 3;
            // 
            // labelReportTitle
            // 
            this.labelReportTitle.AutoSize = true;
            this.labelReportTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReportTitle.Location = new System.Drawing.Point(3, 25);
            this.labelReportTitle.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelReportTitle.Name = "labelReportTitle";
            this.labelReportTitle.Size = new System.Drawing.Size(75, 23);
            this.labelReportTitle.TabIndex = 4;
            this.labelReportTitle.Text = "Title:";
            this.labelReportTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBoxUseStockForLabelDuplicates
            // 
            this.checkBoxUseStockForLabelDuplicates.AutoSize = true;
            this.checkBoxUseStockForLabelDuplicates.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxUseStockForLabelDuplicates.Location = new System.Drawing.Point(722, 28);
            this.checkBoxUseStockForLabelDuplicates.Margin = new System.Windows.Forms.Padding(3, 3, 1, 1);
            this.checkBoxUseStockForLabelDuplicates.Name = "checkBoxUseStockForLabelDuplicates";
            this.checkBoxUseStockForLabelDuplicates.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxUseStockForLabelDuplicates.Size = new System.Drawing.Size(145, 19);
            this.checkBoxUseStockForLabelDuplicates.TabIndex = 7;
            this.checkBoxUseStockForLabelDuplicates.Text = ".Regard stock for dupl";
            this.checkBoxUseStockForLabelDuplicates.UseVisualStyleBackColor = true;
            // 
            // checkBoxPrintRestrictToMaterial
            // 
            this.checkBoxPrintRestrictToMaterial.AutoSize = true;
            this.checkBoxPrintRestrictToMaterial.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxPrintRestrictToMaterial.Location = new System.Drawing.Point(739, 48);
            this.checkBoxPrintRestrictToMaterial.Margin = new System.Windows.Forms.Padding(3, 0, 1, 3);
            this.checkBoxPrintRestrictToMaterial.Name = "checkBoxPrintRestrictToMaterial";
            this.checkBoxPrintRestrictToMaterial.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxPrintRestrictToMaterial.Size = new System.Drawing.Size(128, 26);
            this.checkBoxPrintRestrictToMaterial.TabIndex = 9;
            this.checkBoxPrintRestrictToMaterial.Text = "Restrict to material";
            this.checkBoxPrintRestrictToMaterial.UseVisualStyleBackColor = true;
            // 
            // checkBoxPrintRestrictToCollection
            // 
            this.checkBoxPrintRestrictToCollection.AutoSize = true;
            this.tableLayoutPanelLabel.SetColumnSpan(this.checkBoxPrintRestrictToCollection, 2);
            this.checkBoxPrintRestrictToCollection.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxPrintRestrictToCollection.Location = new System.Drawing.Point(516, 25);
            this.checkBoxPrintRestrictToCollection.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.checkBoxPrintRestrictToCollection.Name = "checkBoxPrintRestrictToCollection";
            this.checkBoxPrintRestrictToCollection.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxPrintRestrictToCollection.Size = new System.Drawing.Size(135, 20);
            this.checkBoxPrintRestrictToCollection.TabIndex = 10;
            this.checkBoxPrintRestrictToCollection.Text = "Restrict to collection";
            this.checkBoxPrintRestrictToCollection.UseVisualStyleBackColor = true;
            // 
            // pictureBoxPrintRestrictToMaterial
            // 
            this.pictureBoxPrintRestrictToMaterial.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxPrintRestrictToMaterial.Location = new System.Drawing.Point(868, 48);
            this.pictureBoxPrintRestrictToMaterial.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.pictureBoxPrintRestrictToMaterial.Name = "pictureBoxPrintRestrictToMaterial";
            this.pictureBoxPrintRestrictToMaterial.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.pictureBoxPrintRestrictToMaterial.Size = new System.Drawing.Size(16, 26);
            this.pictureBoxPrintRestrictToMaterial.TabIndex = 11;
            this.pictureBoxPrintRestrictToMaterial.TabStop = false;
            // 
            // labelLabelConversion
            // 
            this.labelLabelConversion.AutoSize = true;
            this.labelLabelConversion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLabelConversion.Location = new System.Drawing.Point(3, 51);
            this.labelLabelConversion.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelLabelConversion.Name = "labelLabelConversion";
            this.labelLabelConversion.Size = new System.Drawing.Size(75, 26);
            this.labelLabelConversion.TabIndex = 12;
            this.labelLabelConversion.Text = "Conversion:";
            this.labelLabelConversion.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBoxLabelConversion
            // 
            this.comboBoxLabelConversion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxLabelConversion.FormattingEnabled = true;
            this.comboBoxLabelConversion.Location = new System.Drawing.Point(78, 48);
            this.comboBoxLabelConversion.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.comboBoxLabelConversion.Name = "comboBoxLabelConversion";
            this.comboBoxLabelConversion.Size = new System.Drawing.Size(241, 21);
            this.comboBoxLabelConversion.TabIndex = 13;
            // 
            // buttonLabelQRcode
            // 
            this.buttonLabelQRcode.ContextMenuStrip = this.contextMenuStripQR;
            this.buttonLabelQRcode.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonLabelQRcode.FlatAppearance.BorderSize = 0;
            this.buttonLabelQRcode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLabelQRcode.Image = global::DiversityCollection.Resource.QRcode;
            this.buttonLabelQRcode.Location = new System.Drawing.Point(319, 48);
            this.buttonLabelQRcode.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLabelQRcode.Name = "buttonLabelQRcode";
            this.buttonLabelQRcode.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.buttonLabelQRcode.Size = new System.Drawing.Size(20, 18);
            this.buttonLabelQRcode.TabIndex = 14;
            this.buttonLabelQRcode.UseVisualStyleBackColor = true;
            this.buttonLabelQRcode.Click += new System.EventHandler(this.buttonLabelQRcode_Click);
            // 
            // contextMenuStripQR
            // 
            this.contextMenuStripQR.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripQR.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setServiceTemplateToolStripMenuItem,
            this.setSizeToolStripMenuItem});
            this.contextMenuStripQR.Name = "contextMenuStripQR";
            this.contextMenuStripQR.Size = new System.Drawing.Size(213, 80);
            // 
            // setServiceTemplateToolStripMenuItem
            // 
            this.setServiceTemplateToolStripMenuItem.Name = "setServiceTemplateToolStripMenuItem";
            this.setServiceTemplateToolStripMenuItem.Size = new System.Drawing.Size(212, 24);
            this.setServiceTemplateToolStripMenuItem.Text = "Set service template";
            this.setServiceTemplateToolStripMenuItem.Click += new System.EventHandler(this.setServiceTemplateToolStripMenuItem_Click);
            // 
            // comboBoxLabelQRcode
            // 
            this.comboBoxLabelQRcode.Enabled = false;
            this.comboBoxLabelQRcode.FormattingEnabled = true;
            this.comboBoxLabelQRcode.Location = new System.Drawing.Point(408, 48);
            this.comboBoxLabelQRcode.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxLabelQRcode.Name = "comboBoxLabelQRcode";
            this.comboBoxLabelQRcode.Size = new System.Drawing.Size(105, 21);
            this.comboBoxLabelQRcode.TabIndex = 15;
            this.comboBoxLabelQRcode.DropDown += new System.EventHandler(this.comboBoxLabelQRcode_DropDown);
            this.comboBoxLabelQRcode.SelectionChangeCommitted += new System.EventHandler(this.comboBoxLabelQRcode_SelectionChangeCommitted);
            // 
            // pictureBoxLabelQRcodeSource
            // 
            this.pictureBoxLabelQRcodeSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxLabelQRcodeSource.Image = global::DiversityCollection.Resource.QRcodeGray;
            this.pictureBoxLabelQRcodeSource.Location = new System.Drawing.Point(515, 51);
            this.pictureBoxLabelQRcodeSource.Margin = new System.Windows.Forms.Padding(2, 3, 0, 0);
            this.pictureBoxLabelQRcodeSource.Name = "pictureBoxLabelQRcodeSource";
            this.pictureBoxLabelQRcodeSource.Size = new System.Drawing.Size(18, 26);
            this.pictureBoxLabelQRcodeSource.TabIndex = 16;
            this.pictureBoxLabelQRcodeSource.TabStop = false;
            // 
            // comboBoxLabelQRcodeType
            // 
            this.comboBoxLabelQRcodeType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxLabelQRcodeType.Enabled = false;
            this.comboBoxLabelQRcodeType.FormattingEnabled = true;
            this.comboBoxLabelQRcodeType.Location = new System.Drawing.Point(533, 48);
            this.comboBoxLabelQRcodeType.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxLabelQRcodeType.Name = "comboBoxLabelQRcodeType";
            this.comboBoxLabelQRcodeType.Size = new System.Drawing.Size(118, 21);
            this.comboBoxLabelQRcodeType.TabIndex = 17;
            // 
            // checkBoxLabelQRcode
            // 
            this.checkBoxLabelQRcode.AutoSize = true;
            this.checkBoxLabelQRcode.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxLabelQRcode.Location = new System.Drawing.Point(339, 51);
            this.checkBoxLabelQRcode.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.checkBoxLabelQRcode.Name = "checkBoxLabelQRcode";
            this.checkBoxLabelQRcode.Size = new System.Drawing.Size(69, 19);
            this.checkBoxLabelQRcode.TabIndex = 18;
            this.checkBoxLabelQRcode.Text = "incl. QR";
            this.checkBoxLabelQRcode.UseVisualStyleBackColor = true;
            this.checkBoxLabelQRcode.Click += new System.EventHandler(this.checkBoxLabelQRcode_Click);
            // 
            // panelWebbrowserLabel
            // 
            this.panelWebbrowserLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelWebbrowserLabel.Controls.Add(this.webBrowserLabel);
            this.panelWebbrowserLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWebbrowserLabel.Location = new System.Drawing.Point(0, 0);
            this.panelWebbrowserLabel.Name = "panelWebbrowserLabel";
            this.panelWebbrowserLabel.Padding = new System.Windows.Forms.Padding(3);
            this.panelWebbrowserLabel.Size = new System.Drawing.Size(891, 293);
            this.panelWebbrowserLabel.TabIndex = 3;
            // 
            // webBrowserLabel
            // 
            this.webBrowserLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserLabel.Location = new System.Drawing.Point(3, 3);
            this.webBrowserLabel.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserLabel.Name = "webBrowserLabel";
            this.webBrowserLabel.Size = new System.Drawing.Size(881, 283);
            this.webBrowserLabel.TabIndex = 0;
            // 
            // setSizeToolStripMenuItem
            // 
            this.setSizeToolStripMenuItem.Name = "setSizeToolStripMenuItem";
            this.setSizeToolStripMenuItem.Size = new System.Drawing.Size(212, 24);
            this.setSizeToolStripMenuItem.Text = "Set size";
            this.setSizeToolStripMenuItem.Click += new System.EventHandler(this.setSizeToolStripMenuItem_Click);
            // 
            // UserControl_Print
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelWebbrowserLabel);
            this.Controls.Add(this.tableLayoutPanelLabel);
            this.Name = "UserControl_Print";
            this.Size = new System.Drawing.Size(891, 364);
            this.tableLayoutPanelLabel.ResumeLayout(false);
            this.tableLayoutPanelLabel.PerformLayout();
            this.toolStripLabel.ResumeLayout(false);
            this.toolStripLabel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPrintRestrictToMaterial)).EndInit();
            this.contextMenuStripQR.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLabelQRcodeSource)).EndInit();
            this.panelWebbrowserLabel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLabel;
        private System.Windows.Forms.ToolStrip toolStripLabel;
        private System.Windows.Forms.ToolStripButton toolStripButtonNewSchemaFile;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenSchemaFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorLabel;
        private System.Windows.Forms.ToolStripButton toolStripButtonLabelPreview;
        private System.Windows.Forms.ToolStripButton toolStripButtonLabelMulti;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxPrintDuplicates;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorPrint;
        private System.Windows.Forms.ToolStripButton toolStripButtonLabelPrintBrowserOptinos;
        private System.Windows.Forms.ToolStripButton toolStripButtonPageSetup;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrint;
        private System.Windows.Forms.ToolStripButton toolStripButtonLabelExport;
        private System.Windows.Forms.TextBox textBoxSchemaFile;
        private System.Windows.Forms.Label labelSchemaFile;
        private System.Windows.Forms.TextBox textBoxReportTitle;
        private System.Windows.Forms.Label labelReportTitle;
        private System.Windows.Forms.CheckBox checkBoxUseStockForLabelDuplicates;
        private System.Windows.Forms.CheckBox checkBoxPrintRestrictToMaterial;
        private System.Windows.Forms.CheckBox checkBoxPrintRestrictToCollection;
        private System.Windows.Forms.PictureBox pictureBoxPrintRestrictToMaterial;
        private System.Windows.Forms.Label labelLabelConversion;
        private System.Windows.Forms.ComboBox comboBoxLabelConversion;
        private System.Windows.Forms.Button buttonLabelQRcode;
        private System.Windows.Forms.ComboBox comboBoxLabelQRcode;
        private System.Windows.Forms.PictureBox pictureBoxLabelQRcodeSource;
        private System.Windows.Forms.ComboBox comboBoxLabelQRcodeType;
        private System.Windows.Forms.CheckBox checkBoxLabelQRcode;
        private System.Windows.Forms.Panel panelWebbrowserLabel;
        private System.Windows.Forms.OpenFileDialog openFileDialogLabelSchema;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonGitHub;
        private System.Windows.Forms.ToolStripMenuItem sNSBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zFMKToolStripMenuItem;
        private System.Windows.Forms.WebBrowser webBrowserLabel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripQR;
        private System.Windows.Forms.ToolStripMenuItem setServiceTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setSizeToolStripMenuItem;
    }
}
