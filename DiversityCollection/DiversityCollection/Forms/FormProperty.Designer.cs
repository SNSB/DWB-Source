namespace DiversityCollection.Forms
{
    partial class FormProperty
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProperty));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.listBoxProperty = new System.Windows.Forms.ListBox();
            this.propertyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetProperty = new DiversityCollection.Datasets.DataSetProperty();
            this.toolStripProperty = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonImport = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFeedback = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanelProperty = new System.Windows.Forms.TableLayoutPanel();
            this.labelDescription = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryProperty = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.labelDisplayText = new System.Windows.Forms.Label();
            this.textBoxDisplayText = new System.Windows.Forms.TextBox();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelDefaultAccuracyOfProperty = new System.Windows.Forms.Label();
            this.textBoxDefaultAccuracyOfProperty = new System.Windows.Forms.TextBox();
            this.labelDefaultMeasurementUnit = new System.Windows.Forms.Label();
            this.textBoxDefaultMeasurementUnit = new System.Windows.Forms.TextBox();
            this.labelParsingMethodName = new System.Windows.Forms.Label();
            this.comboBoxParsingMethodName = new System.Windows.Forms.ComboBox();
            this.pictureBoxParsingMethodName = new System.Windows.Forms.PictureBox();
            this.labelID = new System.Windows.Forms.Label();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.propertyTableAdapter = new DiversityCollection.Datasets.DataSetPropertyTableAdapters.PropertyTableAdapter();
            this.imageListParsingMethodName = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertyBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetProperty)).BeginInit();
            this.toolStripProperty.SuspendLayout();
            this.tableLayoutPanelProperty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxParsingMethodName)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.listBoxProperty);
            this.splitContainerMain.Panel1.Controls.Add(this.toolStripProperty);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tableLayoutPanelProperty);
            this.splitContainerMain.Size = new System.Drawing.Size(557, 360);
            this.splitContainerMain.SplitterDistance = 185;
            this.splitContainerMain.TabIndex = 0;
            // 
            // listBoxProperty
            // 
            this.listBoxProperty.DataSource = this.propertyBindingSource;
            this.listBoxProperty.DisplayMember = "PropertyName";
            this.listBoxProperty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxProperty.FormattingEnabled = true;
            this.listBoxProperty.Location = new System.Drawing.Point(0, 0);
            this.listBoxProperty.Name = "listBoxProperty";
            this.listBoxProperty.Size = new System.Drawing.Size(185, 335);
            this.listBoxProperty.TabIndex = 1;
            this.listBoxProperty.ValueMember = "PropertyID";
            // 
            // propertyBindingSource
            // 
            this.propertyBindingSource.DataMember = "Property";
            this.propertyBindingSource.DataSource = this.dataSetProperty;
            // 
            // dataSetProperty
            // 
            this.dataSetProperty.DataSetName = "DataSetProperty";
            this.dataSetProperty.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // toolStripProperty
            // 
            this.toolStripProperty.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripProperty.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripProperty.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonImport,
            this.toolStripButtonNew,
            this.toolStripButtonDelete,
            this.toolStripButtonFeedback});
            this.toolStripProperty.Location = new System.Drawing.Point(0, 335);
            this.toolStripProperty.Name = "toolStripProperty";
            this.toolStripProperty.Size = new System.Drawing.Size(185, 25);
            this.toolStripProperty.TabIndex = 0;
            this.toolStripProperty.Text = "toolStrip1";
            // 
            // toolStripButtonImport
            // 
            this.toolStripButtonImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonImport.Image = global::DiversityCollection.Resource.Import;
            this.toolStripButtonImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImport.Name = "toolStripButtonImport";
            this.toolStripButtonImport.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonImport.Text = "Import property from module DiversityScientificTerms";
            this.toolStripButtonImport.Click += new System.EventHandler(this.toolStripButtonImport_Click);
            // 
            // toolStripButtonNew
            // 
            this.toolStripButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNew.Image = global::DiversityCollection.Resource.New;
            this.toolStripButtonNew.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNew.Name = "toolStripButtonNew";
            this.toolStripButtonNew.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNew.Text = "Insert a new property";
            this.toolStripButtonNew.Click += new System.EventHandler(this.toolStripButtonNew_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDelete.Text = "Delete the selected property";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripButtonFeedback
            // 
            this.toolStripButtonFeedback.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonFeedback.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFeedback.Image = global::DiversityCollection.Resource.Feedback;
            this.toolStripButtonFeedback.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFeedback.Name = "toolStripButtonFeedback";
            this.toolStripButtonFeedback.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonFeedback.Text = "Send a feedback to the developer";
            this.toolStripButtonFeedback.Click += new System.EventHandler(this.toolStripButtonFeedback_Click);
            // 
            // tableLayoutPanelProperty
            // 
            this.tableLayoutPanelProperty.ColumnCount = 3;
            this.tableLayoutPanelProperty.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelProperty.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelProperty.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelProperty.Controls.Add(this.labelDescription, 0, 5);
            this.tableLayoutPanelProperty.Controls.Add(this.userControlModuleRelatedEntryProperty, 0, 0);
            this.tableLayoutPanelProperty.Controls.Add(this.labelDisplayText, 0, 1);
            this.tableLayoutPanelProperty.Controls.Add(this.textBoxDisplayText, 1, 1);
            this.tableLayoutPanelProperty.Controls.Add(this.textBoxDescription, 1, 5);
            this.tableLayoutPanelProperty.Controls.Add(this.labelDefaultAccuracyOfProperty, 0, 3);
            this.tableLayoutPanelProperty.Controls.Add(this.textBoxDefaultAccuracyOfProperty, 1, 3);
            this.tableLayoutPanelProperty.Controls.Add(this.labelDefaultMeasurementUnit, 0, 4);
            this.tableLayoutPanelProperty.Controls.Add(this.textBoxDefaultMeasurementUnit, 1, 4);
            this.tableLayoutPanelProperty.Controls.Add(this.labelParsingMethodName, 0, 2);
            this.tableLayoutPanelProperty.Controls.Add(this.comboBoxParsingMethodName, 1, 2);
            this.tableLayoutPanelProperty.Controls.Add(this.pictureBoxParsingMethodName, 2, 2);
            this.tableLayoutPanelProperty.Controls.Add(this.labelID, 2, 0);
            this.tableLayoutPanelProperty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelProperty.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelProperty.Name = "tableLayoutPanelProperty";
            this.tableLayoutPanelProperty.RowCount = 6;
            this.tableLayoutPanelProperty.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProperty.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProperty.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProperty.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProperty.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProperty.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelProperty.Size = new System.Drawing.Size(368, 360);
            this.tableLayoutPanelProperty.TabIndex = 0;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDescription.Location = new System.Drawing.Point(3, 139);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(130, 221);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "Description:";
            this.labelDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // userControlModuleRelatedEntryProperty
            // 
            this.userControlModuleRelatedEntryProperty.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelProperty.SetColumnSpan(this.userControlModuleRelatedEntryProperty, 2);
            this.userControlModuleRelatedEntryProperty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryProperty.Domain = "";
            this.userControlModuleRelatedEntryProperty.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryProperty.Location = new System.Drawing.Point(3, 3);
            this.userControlModuleRelatedEntryProperty.Module = null;
            this.userControlModuleRelatedEntryProperty.Name = "userControlModuleRelatedEntryProperty";
            this.userControlModuleRelatedEntryProperty.Size = new System.Drawing.Size(342, 22);
            this.userControlModuleRelatedEntryProperty.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryProperty.TabIndex = 0;
            // 
            // labelDisplayText
            // 
            this.labelDisplayText.AutoSize = true;
            this.labelDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDisplayText.Location = new System.Drawing.Point(3, 28);
            this.labelDisplayText.Name = "labelDisplayText";
            this.labelDisplayText.Size = new System.Drawing.Size(130, 26);
            this.labelDisplayText.TabIndex = 2;
            this.labelDisplayText.Text = "Displaytext:";
            this.labelDisplayText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxDisplayText
            // 
            this.tableLayoutPanelProperty.SetColumnSpan(this.textBoxDisplayText, 2);
            this.textBoxDisplayText.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.propertyBindingSource, "DisplayText", true));
            this.textBoxDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDisplayText.Location = new System.Drawing.Point(139, 31);
            this.textBoxDisplayText.Name = "textBoxDisplayText";
            this.textBoxDisplayText.Size = new System.Drawing.Size(226, 20);
            this.textBoxDisplayText.TabIndex = 1;
            // 
            // textBoxDescription
            // 
            this.tableLayoutPanelProperty.SetColumnSpan(this.textBoxDescription, 2);
            this.textBoxDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.propertyBindingSource, "Description", true));
            this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescription.Location = new System.Drawing.Point(139, 136);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(226, 221);
            this.textBoxDescription.TabIndex = 3;
            // 
            // labelDefaultAccuracyOfProperty
            // 
            this.labelDefaultAccuracyOfProperty.AutoSize = true;
            this.labelDefaultAccuracyOfProperty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDefaultAccuracyOfProperty.Location = new System.Drawing.Point(3, 81);
            this.labelDefaultAccuracyOfProperty.Name = "labelDefaultAccuracyOfProperty";
            this.labelDefaultAccuracyOfProperty.Size = new System.Drawing.Size(130, 26);
            this.labelDefaultAccuracyOfProperty.TabIndex = 5;
            this.labelDefaultAccuracyOfProperty.Text = "Default accuracy:";
            this.labelDefaultAccuracyOfProperty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxDefaultAccuracyOfProperty
            // 
            this.tableLayoutPanelProperty.SetColumnSpan(this.textBoxDefaultAccuracyOfProperty, 2);
            this.textBoxDefaultAccuracyOfProperty.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.propertyBindingSource, "DefaultAccuracyOfProperty", true));
            this.textBoxDefaultAccuracyOfProperty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDefaultAccuracyOfProperty.Location = new System.Drawing.Point(139, 84);
            this.textBoxDefaultAccuracyOfProperty.Name = "textBoxDefaultAccuracyOfProperty";
            this.textBoxDefaultAccuracyOfProperty.Size = new System.Drawing.Size(226, 20);
            this.textBoxDefaultAccuracyOfProperty.TabIndex = 6;
            // 
            // labelDefaultMeasurementUnit
            // 
            this.labelDefaultMeasurementUnit.AutoSize = true;
            this.labelDefaultMeasurementUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDefaultMeasurementUnit.Location = new System.Drawing.Point(3, 107);
            this.labelDefaultMeasurementUnit.Name = "labelDefaultMeasurementUnit";
            this.labelDefaultMeasurementUnit.Size = new System.Drawing.Size(130, 26);
            this.labelDefaultMeasurementUnit.TabIndex = 7;
            this.labelDefaultMeasurementUnit.Text = "Default measurement unit:";
            this.labelDefaultMeasurementUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxDefaultMeasurementUnit
            // 
            this.tableLayoutPanelProperty.SetColumnSpan(this.textBoxDefaultMeasurementUnit, 2);
            this.textBoxDefaultMeasurementUnit.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.propertyBindingSource, "DefaultMeasurementUnit", true));
            this.textBoxDefaultMeasurementUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDefaultMeasurementUnit.Location = new System.Drawing.Point(139, 110);
            this.textBoxDefaultMeasurementUnit.Name = "textBoxDefaultMeasurementUnit";
            this.textBoxDefaultMeasurementUnit.Size = new System.Drawing.Size(226, 20);
            this.textBoxDefaultMeasurementUnit.TabIndex = 8;
            // 
            // labelParsingMethodName
            // 
            this.labelParsingMethodName.AutoSize = true;
            this.labelParsingMethodName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelParsingMethodName.Location = new System.Drawing.Point(3, 54);
            this.labelParsingMethodName.Name = "labelParsingMethodName";
            this.labelParsingMethodName.Size = new System.Drawing.Size(130, 27);
            this.labelParsingMethodName.TabIndex = 9;
            this.labelParsingMethodName.Text = "Parsing method:";
            this.labelParsingMethodName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxParsingMethodName
            // 
            this.comboBoxParsingMethodName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.propertyBindingSource, "ParsingMethodName", true));
            this.comboBoxParsingMethodName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxParsingMethodName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxParsingMethodName.FormattingEnabled = true;
            this.comboBoxParsingMethodName.Items.AddRange(new object[] {
            "Stratigraphy",
            "Vegetation"});
            this.comboBoxParsingMethodName.Location = new System.Drawing.Point(139, 57);
            this.comboBoxParsingMethodName.Name = "comboBoxParsingMethodName";
            this.comboBoxParsingMethodName.Size = new System.Drawing.Size(206, 21);
            this.comboBoxParsingMethodName.TabIndex = 10;
            this.comboBoxParsingMethodName.SelectedIndexChanged += new System.EventHandler(this.comboBoxParsingMethodName_SelectedIndexChanged);
            // 
            // pictureBoxParsingMethodName
            // 
            this.pictureBoxParsingMethodName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxParsingMethodName.Location = new System.Drawing.Point(349, 59);
            this.pictureBoxParsingMethodName.Margin = new System.Windows.Forms.Padding(1, 5, 3, 6);
            this.pictureBoxParsingMethodName.Name = "pictureBoxParsingMethodName";
            this.pictureBoxParsingMethodName.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxParsingMethodName.TabIndex = 11;
            this.pictureBoxParsingMethodName.TabStop = false;
            // 
            // labelID
            // 
            this.labelID.AutoSize = true;
            this.labelID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.propertyBindingSource, "PropertyID", true));
            this.labelID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelID.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelID.Location = new System.Drawing.Point(348, 0);
            this.labelID.Margin = new System.Windows.Forms.Padding(0);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(20, 28);
            this.labelID.TabIndex = 12;
            this.labelID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.labelID, "ID of the property");
            // 
            // propertyTableAdapter
            // 
            this.propertyTableAdapter.ClearBeforeFill = true;
            // 
            // imageListParsingMethodName
            // 
            this.imageListParsingMethodName.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListParsingMethodName.ImageStream")));
            this.imageListParsingMethodName.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListParsingMethodName.Images.SetKeyName(0, "EventProperty.ico");
            this.imageListParsingMethodName.Images.SetKeyName(1, "Geology.ico");
            // 
            // FormProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 360);
            this.Controls.Add(this.splitContainerMain);
            this.helpProvider.SetHelpKeyword(this, "Site property");
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormProperty";
            this.helpProvider.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Properties of the collection site";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProperty_FormClosing);
            this.Load += new System.EventHandler(this.FormProperty_Load);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel1.PerformLayout();
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.propertyBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetProperty)).EndInit();
            this.toolStripProperty.ResumeLayout(false);
            this.toolStripProperty.PerformLayout();
            this.tableLayoutPanelProperty.ResumeLayout(false);
            this.tableLayoutPanelProperty.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxParsingMethodName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.ListBox listBoxProperty;
        private System.Windows.Forms.ToolStrip toolStripProperty;
        private System.Windows.Forms.ToolStripButton toolStripButtonNew;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProperty;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryProperty;
        private Datasets.DataSetProperty dataSetProperty;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.TextBox textBoxDisplayText;
        private System.Windows.Forms.BindingSource propertyBindingSource;
        private Datasets.DataSetPropertyTableAdapters.PropertyTableAdapter propertyTableAdapter;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelDisplayText;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label labelDefaultAccuracyOfProperty;
        private System.Windows.Forms.TextBox textBoxDefaultAccuracyOfProperty;
        private System.Windows.Forms.Label labelDefaultMeasurementUnit;
        private System.Windows.Forms.TextBox textBoxDefaultMeasurementUnit;
        private System.Windows.Forms.Label labelParsingMethodName;
        private System.Windows.Forms.ComboBox comboBoxParsingMethodName;
        private System.Windows.Forms.PictureBox pictureBoxParsingMethodName;
        private System.Windows.Forms.ImageList imageListParsingMethodName;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonImport;
        private System.Windows.Forms.ToolStripButton toolStripButtonFeedback;
        private System.Windows.Forms.Label labelID;
        private System.Windows.Forms.ToolTip toolTip;
    }
}