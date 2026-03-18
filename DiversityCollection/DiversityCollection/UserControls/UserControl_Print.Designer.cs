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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_Print));
            tableLayoutPanelLabel = new System.Windows.Forms.TableLayoutPanel();
            toolStripLabel = new System.Windows.Forms.ToolStrip();
            toolStripButtonNewSchemaFile = new System.Windows.Forms.ToolStripButton();
            toolStripButtonOpenSchemaFile = new System.Windows.Forms.ToolStripButton();
            toolStripDropDownButtonGitHub = new System.Windows.Forms.ToolStripDropDownButton();
            sNSBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            zFMKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparatorLabel = new System.Windows.Forms.ToolStripSeparator();
            toolStripButtonLabelPreview = new System.Windows.Forms.ToolStripButton();
            toolStripButtonLabelMulti = new System.Windows.Forms.ToolStripButton();
            toolStripTextBoxPrintDuplicates = new System.Windows.Forms.ToolStripTextBox();
            toolStripSeparatorPrint = new System.Windows.Forms.ToolStripSeparator();
            toolStripButtonLabelPrintBrowserOptinos = new System.Windows.Forms.ToolStripButton();
            toolStripButtonPageSetup = new System.Windows.Forms.ToolStripButton();
            toolStripButtonPrint = new System.Windows.Forms.ToolStripButton();
            toolStripButtonLabelExport = new System.Windows.Forms.ToolStripButton();
            textBoxSchemaFile = new System.Windows.Forms.TextBox();
            labelSchemaFile = new System.Windows.Forms.Label();
            textBoxReportTitle = new System.Windows.Forms.TextBox();
            labelReportTitle = new System.Windows.Forms.Label();
            checkBoxUseStockForLabelDuplicates = new System.Windows.Forms.CheckBox();
            checkBoxPrintRestrictToMaterial = new System.Windows.Forms.CheckBox();
            checkBoxPrintRestrictToCollection = new System.Windows.Forms.CheckBox();
            pictureBoxPrintRestrictToMaterial = new System.Windows.Forms.PictureBox();
            labelLabelConversion = new System.Windows.Forms.Label();
            comboBoxLabelConversion = new System.Windows.Forms.ComboBox();
            buttonLabelQRcode = new System.Windows.Forms.Button();
            contextMenuStripQR = new System.Windows.Forms.ContextMenuStrip(components);
            setServiceTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            setSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            comboBoxLabelQRcode = new System.Windows.Forms.ComboBox();
            pictureBoxLabelQRcodeSource = new System.Windows.Forms.PictureBox();
            comboBoxLabelQRcodeType = new System.Windows.Forms.ComboBox();
            checkBoxLabelQRcode = new System.Windows.Forms.CheckBox();
            panelWebbrowserLabel = new System.Windows.Forms.Panel();
            webBrowserLabel = new System.Windows.Forms.WebBrowser();
            openFileDialogLabelSchema = new System.Windows.Forms.OpenFileDialog();
            saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            tableLayoutPanelLabel.SuspendLayout();
            toolStripLabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPrintRestrictToMaterial).BeginInit();
            contextMenuStripQR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLabelQRcodeSource).BeginInit();
            panelWebbrowserLabel.SuspendLayout();
            SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            imageListDataWithholding.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListDataWithholding.ImageStream");
            imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // tableLayoutPanelLabel
            // 
            tableLayoutPanelLabel.ColumnCount = 9;
            tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            tableLayoutPanelLabel.Controls.Add(toolStripLabel, 7, 0);
            tableLayoutPanelLabel.Controls.Add(textBoxSchemaFile, 1, 0);
            tableLayoutPanelLabel.Controls.Add(labelSchemaFile, 0, 0);
            tableLayoutPanelLabel.Controls.Add(textBoxReportTitle, 1, 1);
            tableLayoutPanelLabel.Controls.Add(labelReportTitle, 0, 1);
            tableLayoutPanelLabel.Controls.Add(checkBoxUseStockForLabelDuplicates, 7, 1);
            tableLayoutPanelLabel.Controls.Add(checkBoxPrintRestrictToMaterial, 7, 2);
            tableLayoutPanelLabel.Controls.Add(checkBoxPrintRestrictToCollection, 5, 1);
            tableLayoutPanelLabel.Controls.Add(pictureBoxPrintRestrictToMaterial, 8, 2);
            tableLayoutPanelLabel.Controls.Add(labelLabelConversion, 0, 2);
            tableLayoutPanelLabel.Controls.Add(comboBoxLabelConversion, 1, 2);
            tableLayoutPanelLabel.Controls.Add(buttonLabelQRcode, 2, 2);
            tableLayoutPanelLabel.Controls.Add(comboBoxLabelQRcode, 4, 2);
            tableLayoutPanelLabel.Controls.Add(pictureBoxLabelQRcodeSource, 5, 2);
            tableLayoutPanelLabel.Controls.Add(comboBoxLabelQRcodeType, 6, 2);
            tableLayoutPanelLabel.Controls.Add(checkBoxLabelQRcode, 3, 2);
            tableLayoutPanelLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            tableLayoutPanelLabel.Location = new System.Drawing.Point(0, 338);
            tableLayoutPanelLabel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelLabel.Name = "tableLayoutPanelLabel";
            tableLayoutPanelLabel.RowCount = 3;
            tableLayoutPanelLabel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            tableLayoutPanelLabel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelLabel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelLabel.Size = new System.Drawing.Size(1040, 82);
            tableLayoutPanelLabel.TabIndex = 1;
            // 
            // toolStripLabel
            // 
            tableLayoutPanelLabel.SetColumnSpan(toolStripLabel, 2);
            toolStripLabel.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStripLabel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonNewSchemaFile, toolStripButtonOpenSchemaFile, toolStripDropDownButtonGitHub, toolStripSeparatorLabel, toolStripButtonLabelPreview, toolStripButtonLabelMulti, toolStripTextBoxPrintDuplicates, toolStripSeparatorPrint, toolStripButtonLabelPrintBrowserOptinos, toolStripButtonPageSetup, toolStripButtonPrint, toolStripButtonLabelExport });
            toolStripLabel.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            toolStripLabel.Location = new System.Drawing.Point(834, 0);
            toolStripLabel.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            toolStripLabel.Name = "toolStripLabel";
            toolStripLabel.Size = new System.Drawing.Size(202, 23);
            toolStripLabel.TabIndex = 0;
            // 
            // toolStripButtonNewSchemaFile
            // 
            toolStripButtonNewSchemaFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonNewSchemaFile.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonNewSchemaFile.Image");
            toolStripButtonNewSchemaFile.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButtonNewSchemaFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonNewSchemaFile.Name = "toolStripButtonNewSchemaFile";
            toolStripButtonNewSchemaFile.Size = new System.Drawing.Size(23, 20);
            toolStripButtonNewSchemaFile.Text = "Create a new schema file";
            toolStripButtonNewSchemaFile.Visible = false;
            // 
            // toolStripButtonOpenSchemaFile
            // 
            toolStripButtonOpenSchemaFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonOpenSchemaFile.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonOpenSchemaFile.Image");
            toolStripButtonOpenSchemaFile.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButtonOpenSchemaFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonOpenSchemaFile.Name = "toolStripButtonOpenSchemaFile";
            toolStripButtonOpenSchemaFile.Size = new System.Drawing.Size(23, 20);
            toolStripButtonOpenSchemaFile.Text = "Select schema file (XSD)";
            toolStripButtonOpenSchemaFile.Click += toolStripButtonOpenSchemaFile_Click;
            // 
            // toolStripDropDownButtonGitHub
            // 
            toolStripDropDownButtonGitHub.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripDropDownButtonGitHub.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { sNSBToolStripMenuItem, zFMKToolStripMenuItem });
            toolStripDropDownButtonGitHub.Image = Resource.Github;
            toolStripDropDownButtonGitHub.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripDropDownButtonGitHub.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButtonGitHub.Name = "toolStripDropDownButtonGitHub";
            toolStripDropDownButtonGitHub.Size = new System.Drawing.Size(29, 20);
            toolStripDropDownButtonGitHub.Text = "Search for schema files on GitHub";
            // 
            // sNSBToolStripMenuItem
            // 
            sNSBToolStripMenuItem.Image = Resource.SNSB;
            sNSBToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            sNSBToolStripMenuItem.Name = "sNSBToolStripMenuItem";
            sNSBToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            sNSBToolStripMenuItem.Text = "SNSB";
            sNSBToolStripMenuItem.ToolTipText = "Schema files provided by the Staatliche Naturwissenschaftliche Sammlungen Bayerns";
            sNSBToolStripMenuItem.Click += sNSBToolStripMenuItem_Click;
            // 
            // zFMKToolStripMenuItem
            // 
            zFMKToolStripMenuItem.Image = Resource.ZFMK;
            zFMKToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            zFMKToolStripMenuItem.Name = "zFMKToolStripMenuItem";
            zFMKToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            zFMKToolStripMenuItem.Text = "ZFMK";
            zFMKToolStripMenuItem.ToolTipText = "Schema files provided by the Zoologische Forschungsmuseum Alexander Koenig";
            zFMKToolStripMenuItem.Click += zFMKToolStripMenuItem_Click;
            // 
            // toolStripSeparatorLabel
            // 
            toolStripSeparatorLabel.Name = "toolStripSeparatorLabel";
            toolStripSeparatorLabel.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonLabelPreview
            // 
            toolStripButtonLabelPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonLabelPreview.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonLabelPreview.Image");
            toolStripButtonLabelPreview.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButtonLabelPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonLabelPreview.Name = "toolStripButtonLabelPreview";
            toolStripButtonLabelPreview.Size = new System.Drawing.Size(23, 20);
            toolStripButtonLabelPreview.Text = "Label preview";
            toolStripButtonLabelPreview.Click += toolStripButtonLabelPreview_Click;
            // 
            // toolStripButtonLabelMulti
            // 
            toolStripButtonLabelMulti.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonLabelMulti.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonLabelMulti.Image");
            toolStripButtonLabelMulti.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButtonLabelMulti.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonLabelMulti.Name = "toolStripButtonLabelMulti";
            toolStripButtonLabelMulti.Size = new System.Drawing.Size(23, 20);
            toolStripButtonLabelMulti.Text = "Preview for all labels in query";
            toolStripButtonLabelMulti.Click += toolStripButtonLabelMulti_Click;
            // 
            // toolStripTextBoxPrintDuplicates
            // 
            toolStripTextBoxPrintDuplicates.Name = "toolStripTextBoxPrintDuplicates";
            toolStripTextBoxPrintDuplicates.Size = new System.Drawing.Size(20, 23);
            toolStripTextBoxPrintDuplicates.Text = "1";
            toolStripTextBoxPrintDuplicates.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            toolStripTextBoxPrintDuplicates.ToolTipText = "Number of duplicates that should be printed (1 - 99)";
            toolStripTextBoxPrintDuplicates.TextChanged += toolStripTextBoxPrintDuplicates_TextChanged;
            // 
            // toolStripSeparatorPrint
            // 
            toolStripSeparatorPrint.Name = "toolStripSeparatorPrint";
            toolStripSeparatorPrint.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButtonLabelPrintBrowserOptinos
            // 
            toolStripButtonLabelPrintBrowserOptinos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonLabelPrintBrowserOptinos.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonLabelPrintBrowserOptinos.Image");
            toolStripButtonLabelPrintBrowserOptinos.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButtonLabelPrintBrowserOptinos.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonLabelPrintBrowserOptinos.Name = "toolStripButtonLabelPrintBrowserOptinos";
            toolStripButtonLabelPrintBrowserOptinos.Size = new System.Drawing.Size(23, 20);
            toolStripButtonLabelPrintBrowserOptinos.Text = "set browser options";
            toolStripButtonLabelPrintBrowserOptinos.Visible = false;
            // 
            // toolStripButtonPageSetup
            // 
            toolStripButtonPageSetup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonPageSetup.Image = (System.Drawing.Image)resources.GetObject("toolStripButtonPageSetup.Image");
            toolStripButtonPageSetup.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButtonPageSetup.ImageTransparentColor = System.Drawing.Color.Purple;
            toolStripButtonPageSetup.Name = "toolStripButtonPageSetup";
            toolStripButtonPageSetup.Size = new System.Drawing.Size(23, 20);
            toolStripButtonPageSetup.Text = "Page setup";
            toolStripButtonPageSetup.Click += toolStripButtonPageSetup_Click;
            // 
            // toolStripButtonPrint
            // 
            toolStripButtonPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonPrint.Image = Resource.Print1;
            toolStripButtonPrint.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButtonPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonPrint.Name = "toolStripButtonPrint";
            toolStripButtonPrint.Size = new System.Drawing.Size(23, 20);
            toolStripButtonPrint.Text = "Print label";
            toolStripButtonPrint.Click += toolStripButtonPrint_Click;
            // 
            // toolStripButtonLabelExport
            // 
            toolStripButtonLabelExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonLabelExport.Image = Resource.Save;
            toolStripButtonLabelExport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButtonLabelExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonLabelExport.Name = "toolStripButtonLabelExport";
            toolStripButtonLabelExport.Size = new System.Drawing.Size(23, 18);
            toolStripButtonLabelExport.Text = "save print files";
            toolStripButtonLabelExport.Click += toolStripButtonLabelExport_Click;
            // 
            // textBoxSchemaFile
            // 
            tableLayoutPanelLabel.SetColumnSpan(textBoxSchemaFile, 6);
            textBoxSchemaFile.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxSchemaFile.Location = new System.Drawing.Point(75, 5);
            textBoxSchemaFile.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            textBoxSchemaFile.Name = "textBoxSchemaFile";
            textBoxSchemaFile.Size = new System.Drawing.Size(759, 23);
            textBoxSchemaFile.TabIndex = 1;
            // 
            // labelSchemaFile
            // 
            labelSchemaFile.AutoSize = true;
            labelSchemaFile.Dock = System.Windows.Forms.DockStyle.Fill;
            labelSchemaFile.Location = new System.Drawing.Point(4, 0);
            labelSchemaFile.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelSchemaFile.Name = "labelSchemaFile";
            labelSchemaFile.Size = new System.Drawing.Size(71, 29);
            labelSchemaFile.TabIndex = 2;
            labelSchemaFile.Text = "Schema file:";
            labelSchemaFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxReportTitle
            // 
            tableLayoutPanelLabel.SetColumnSpan(textBoxReportTitle, 4);
            textBoxReportTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxReportTitle.Location = new System.Drawing.Point(75, 29);
            textBoxReportTitle.Margin = new System.Windows.Forms.Padding(0);
            textBoxReportTitle.Name = "textBoxReportTitle";
            textBoxReportTitle.Size = new System.Drawing.Size(599, 23);
            textBoxReportTitle.TabIndex = 3;
            // 
            // labelReportTitle
            // 
            labelReportTitle.AutoSize = true;
            labelReportTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            labelReportTitle.Location = new System.Drawing.Point(4, 29);
            labelReportTitle.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelReportTitle.Name = "labelReportTitle";
            labelReportTitle.Size = new System.Drawing.Size(71, 23);
            labelReportTitle.TabIndex = 4;
            labelReportTitle.Text = "Title:";
            labelReportTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBoxUseStockForLabelDuplicates
            // 
            checkBoxUseStockForLabelDuplicates.AutoSize = true;
            checkBoxUseStockForLabelDuplicates.Dock = System.Windows.Forms.DockStyle.Right;
            checkBoxUseStockForLabelDuplicates.Location = new System.Drawing.Point(870, 32);
            checkBoxUseStockForLabelDuplicates.Margin = new System.Windows.Forms.Padding(4, 3, 1, 1);
            checkBoxUseStockForLabelDuplicates.Name = "checkBoxUseStockForLabelDuplicates";
            checkBoxUseStockForLabelDuplicates.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            checkBoxUseStockForLabelDuplicates.Size = new System.Drawing.Size(142, 19);
            checkBoxUseStockForLabelDuplicates.TabIndex = 7;
            checkBoxUseStockForLabelDuplicates.Text = ".Regard stock for dupl";
            checkBoxUseStockForLabelDuplicates.UseVisualStyleBackColor = true;
            // 
            // checkBoxPrintRestrictToMaterial
            // 
            checkBoxPrintRestrictToMaterial.AutoSize = true;
            checkBoxPrintRestrictToMaterial.Dock = System.Windows.Forms.DockStyle.Right;
            checkBoxPrintRestrictToMaterial.Location = new System.Drawing.Point(887, 52);
            checkBoxPrintRestrictToMaterial.Margin = new System.Windows.Forms.Padding(4, 0, 1, 3);
            checkBoxPrintRestrictToMaterial.Name = "checkBoxPrintRestrictToMaterial";
            checkBoxPrintRestrictToMaterial.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            checkBoxPrintRestrictToMaterial.Size = new System.Drawing.Size(125, 30);
            checkBoxPrintRestrictToMaterial.TabIndex = 9;
            checkBoxPrintRestrictToMaterial.Text = "Restrict to material";
            checkBoxPrintRestrictToMaterial.UseVisualStyleBackColor = true;
            // 
            // checkBoxPrintRestrictToCollection
            // 
            checkBoxPrintRestrictToCollection.AutoSize = true;
            tableLayoutPanelLabel.SetColumnSpan(checkBoxPrintRestrictToCollection, 2);
            checkBoxPrintRestrictToCollection.Dock = System.Windows.Forms.DockStyle.Right;
            checkBoxPrintRestrictToCollection.Location = new System.Drawing.Point(700, 29);
            checkBoxPrintRestrictToCollection.Margin = new System.Windows.Forms.Padding(4, 0, 0, 3);
            checkBoxPrintRestrictToCollection.Name = "checkBoxPrintRestrictToCollection";
            checkBoxPrintRestrictToCollection.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            checkBoxPrintRestrictToCollection.Size = new System.Drawing.Size(134, 20);
            checkBoxPrintRestrictToCollection.TabIndex = 10;
            checkBoxPrintRestrictToCollection.Text = "Restrict to collection";
            checkBoxPrintRestrictToCollection.UseVisualStyleBackColor = true;
            // 
            // pictureBoxPrintRestrictToMaterial
            // 
            pictureBoxPrintRestrictToMaterial.Dock = System.Windows.Forms.DockStyle.Left;
            pictureBoxPrintRestrictToMaterial.Location = new System.Drawing.Point(1013, 52);
            pictureBoxPrintRestrictToMaterial.Margin = new System.Windows.Forms.Padding(0, 0, 4, 3);
            pictureBoxPrintRestrictToMaterial.Name = "pictureBoxPrintRestrictToMaterial";
            pictureBoxPrintRestrictToMaterial.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
            pictureBoxPrintRestrictToMaterial.Size = new System.Drawing.Size(19, 30);
            pictureBoxPrintRestrictToMaterial.TabIndex = 11;
            pictureBoxPrintRestrictToMaterial.TabStop = false;
            // 
            // labelLabelConversion
            // 
            labelLabelConversion.AutoSize = true;
            labelLabelConversion.Dock = System.Windows.Forms.DockStyle.Fill;
            labelLabelConversion.Location = new System.Drawing.Point(4, 55);
            labelLabelConversion.Margin = new System.Windows.Forms.Padding(4, 3, 0, 0);
            labelLabelConversion.Name = "labelLabelConversion";
            labelLabelConversion.Size = new System.Drawing.Size(71, 30);
            labelLabelConversion.TabIndex = 12;
            labelLabelConversion.Text = "Conversion:";
            labelLabelConversion.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBoxLabelConversion
            // 
            comboBoxLabelConversion.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxLabelConversion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxLabelConversion.FormattingEnabled = true;
            comboBoxLabelConversion.Location = new System.Drawing.Point(75, 52);
            comboBoxLabelConversion.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            comboBoxLabelConversion.Name = "comboBoxLabelConversion";
            comboBoxLabelConversion.Size = new System.Drawing.Size(387, 23);
            comboBoxLabelConversion.TabIndex = 13;
            // 
            // buttonLabelQRcode
            // 
            buttonLabelQRcode.ContextMenuStrip = contextMenuStripQR;
            buttonLabelQRcode.Dock = System.Windows.Forms.DockStyle.Top;
            buttonLabelQRcode.FlatAppearance.BorderSize = 0;
            buttonLabelQRcode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonLabelQRcode.Image = Resource.QRcode;
            buttonLabelQRcode.Location = new System.Drawing.Point(462, 52);
            buttonLabelQRcode.Margin = new System.Windows.Forms.Padding(0);
            buttonLabelQRcode.Name = "buttonLabelQRcode";
            buttonLabelQRcode.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            buttonLabelQRcode.Size = new System.Drawing.Size(23, 21);
            buttonLabelQRcode.TabIndex = 14;
            buttonLabelQRcode.UseVisualStyleBackColor = true;
            buttonLabelQRcode.Click += buttonLabelQRcode_Click;
            // 
            // contextMenuStripQR
            // 
            contextMenuStripQR.ImageScalingSize = new System.Drawing.Size(20, 20);
            contextMenuStripQR.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { setServiceTemplateToolStripMenuItem, setSizeToolStripMenuItem });
            contextMenuStripQR.Name = "contextMenuStripQR";
            contextMenuStripQR.Size = new System.Drawing.Size(180, 48);
            // 
            // setServiceTemplateToolStripMenuItem
            // 
            setServiceTemplateToolStripMenuItem.Name = "setServiceTemplateToolStripMenuItem";
            setServiceTemplateToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            setServiceTemplateToolStripMenuItem.Text = "Set service template";
            setServiceTemplateToolStripMenuItem.Click += setServiceTemplateToolStripMenuItem_Click;
            // 
            // setSizeToolStripMenuItem
            // 
            setSizeToolStripMenuItem.Name = "setSizeToolStripMenuItem";
            setSizeToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            setSizeToolStripMenuItem.Text = "Set size";
            setSizeToolStripMenuItem.Click += setSizeToolStripMenuItem_Click;
            // 
            // comboBoxLabelQRcode
            // 
            comboBoxLabelQRcode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxLabelQRcode.Enabled = false;
            comboBoxLabelQRcode.FormattingEnabled = true;
            comboBoxLabelQRcode.Location = new System.Drawing.Point(552, 52);
            comboBoxLabelQRcode.Margin = new System.Windows.Forms.Padding(0);
            comboBoxLabelQRcode.Name = "comboBoxLabelQRcode";
            comboBoxLabelQRcode.Size = new System.Drawing.Size(122, 23);
            comboBoxLabelQRcode.TabIndex = 15;
            comboBoxLabelQRcode.DropDown += comboBoxLabelQRcode_DropDown;
            comboBoxLabelQRcode.SelectionChangeCommitted += comboBoxLabelQRcode_SelectionChangeCommitted;
            // 
            // pictureBoxLabelQRcodeSource
            // 
            pictureBoxLabelQRcodeSource.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxLabelQRcodeSource.Image = Resource.QRcodeGray;
            pictureBoxLabelQRcodeSource.Location = new System.Drawing.Point(676, 55);
            pictureBoxLabelQRcodeSource.Margin = new System.Windows.Forms.Padding(2, 3, 0, 0);
            pictureBoxLabelQRcodeSource.Name = "pictureBoxLabelQRcodeSource";
            pictureBoxLabelQRcodeSource.Size = new System.Drawing.Size(21, 30);
            pictureBoxLabelQRcodeSource.TabIndex = 16;
            pictureBoxLabelQRcodeSource.TabStop = false;
            // 
            // comboBoxLabelQRcodeType
            // 
            comboBoxLabelQRcodeType.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxLabelQRcodeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxLabelQRcodeType.Enabled = false;
            comboBoxLabelQRcodeType.FormattingEnabled = true;
            comboBoxLabelQRcodeType.Location = new System.Drawing.Point(697, 52);
            comboBoxLabelQRcodeType.Margin = new System.Windows.Forms.Padding(0);
            comboBoxLabelQRcodeType.Name = "comboBoxLabelQRcodeType";
            comboBoxLabelQRcodeType.Size = new System.Drawing.Size(137, 23);
            comboBoxLabelQRcodeType.TabIndex = 17;
            // 
            // checkBoxLabelQRcode
            // 
            checkBoxLabelQRcode.AutoSize = true;
            checkBoxLabelQRcode.Dock = System.Windows.Forms.DockStyle.Top;
            checkBoxLabelQRcode.Location = new System.Drawing.Point(485, 55);
            checkBoxLabelQRcode.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            checkBoxLabelQRcode.Name = "checkBoxLabelQRcode";
            checkBoxLabelQRcode.Size = new System.Drawing.Size(67, 19);
            checkBoxLabelQRcode.TabIndex = 18;
            checkBoxLabelQRcode.Text = "incl. QR";
            checkBoxLabelQRcode.UseVisualStyleBackColor = true;
            checkBoxLabelQRcode.Click += checkBoxLabelQRcode_Click;
            // 
            // panelWebbrowserLabel
            // 
            panelWebbrowserLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panelWebbrowserLabel.Controls.Add(webBrowserLabel);
            panelWebbrowserLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            panelWebbrowserLabel.Location = new System.Drawing.Point(0, 0);
            panelWebbrowserLabel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panelWebbrowserLabel.Name = "panelWebbrowserLabel";
            panelWebbrowserLabel.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panelWebbrowserLabel.Size = new System.Drawing.Size(1040, 338);
            panelWebbrowserLabel.TabIndex = 3;
            // 
            // webBrowserLabel
            // 
            webBrowserLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            webBrowserLabel.Location = new System.Drawing.Point(4, 3);
            webBrowserLabel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            webBrowserLabel.MinimumSize = new System.Drawing.Size(23, 23);
            webBrowserLabel.Name = "webBrowserLabel";
            webBrowserLabel.Size = new System.Drawing.Size(1028, 328);
            webBrowserLabel.TabIndex = 0;
            // 
            // UserControl_Print
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(panelWebbrowserLabel);
            Controls.Add(tableLayoutPanelLabel);
            Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            Name = "UserControl_Print";
            Size = new System.Drawing.Size(1040, 420);
            tableLayoutPanelLabel.ResumeLayout(false);
            tableLayoutPanelLabel.PerformLayout();
            toolStripLabel.ResumeLayout(false);
            toolStripLabel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPrintRestrictToMaterial).EndInit();
            contextMenuStripQR.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxLabelQRcodeSource).EndInit();
            panelWebbrowserLabel.ResumeLayout(false);
            ResumeLayout(false);

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
