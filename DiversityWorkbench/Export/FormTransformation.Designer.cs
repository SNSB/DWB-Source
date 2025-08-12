namespace DiversityWorkbench.Export
{
    partial class FormTransformation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTransformation));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.tabControlTransformations = new System.Windows.Forms.TabControl();
            this.imageListTabControl = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanelTest = new System.Windows.Forms.TableLayoutPanel();
            this.buttonTest = new System.Windows.Forms.Button();
            this.dataGridViewTest = new System.Windows.Forms.DataGridView();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelAdd = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButtonSplit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonTranslation = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRegularExpression = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonReplacement = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFilter = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCalculation = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonColor = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFormat = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFeedback = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBoxPostfix = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabelPostfix = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBoxPrefix = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabelPrefix = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tableLayoutPanelTest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTest)).BeginInit();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 25);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.tabControlTransformations);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tableLayoutPanelTest);
            this.splitContainerMain.Size = new System.Drawing.Size(567, 415);
            this.splitContainerMain.SplitterDistance = 163;
            this.splitContainerMain.TabIndex = 3;
            // 
            // tabControlTransformations
            // 
            this.tabControlTransformations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlTransformations.ImageList = this.imageListTabControl;
            this.tabControlTransformations.Location = new System.Drawing.Point(0, 0);
            this.tabControlTransformations.Name = "tabControlTransformations";
            this.tabControlTransformations.SelectedIndex = 0;
            this.tabControlTransformations.Size = new System.Drawing.Size(567, 163);
            this.tabControlTransformations.TabIndex = 0;
            // 
            // imageListTabControl
            // 
            this.imageListTabControl.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTabControl.ImageStream")));
            this.imageListTabControl.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTabControl.Images.SetKeyName(0, "Schere.gif");
            this.imageListTabControl.Images.SetKeyName(1, "Translation.ico");
            this.imageListTabControl.Images.SetKeyName(2, "Replace.ico");
            this.imageListTabControl.Images.SetKeyName(3, "Filter.ico");
            this.imageListTabControl.Images.SetKeyName(4, "Color.ico");
            // 
            // tableLayoutPanelTest
            // 
            this.tableLayoutPanelTest.ColumnCount = 1;
            this.tableLayoutPanelTest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTest.Controls.Add(this.buttonTest, 0, 0);
            this.tableLayoutPanelTest.Controls.Add(this.dataGridViewTest, 0, 1);
            this.tableLayoutPanelTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTest.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelTest.Name = "tableLayoutPanelTest";
            this.tableLayoutPanelTest.RowCount = 2;
            this.tableLayoutPanelTest.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTest.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTest.Size = new System.Drawing.Size(567, 248);
            this.tableLayoutPanelTest.TabIndex = 0;
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(3, 3);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(138, 23);
            this.buttonTest.TabIndex = 0;
            this.buttonTest.Text = "Test the transformation";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // dataGridViewTest
            // 
            this.dataGridViewTest.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTest.Location = new System.Drawing.Point(3, 32);
            this.dataGridViewTest.Name = "dataGridViewTest";
            this.dataGridViewTest.Size = new System.Drawing.Size(561, 213);
            this.dataGridViewTest.TabIndex = 1;
            this.dataGridViewTest.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewTest_DataError);
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelAdd,
            this.toolStripButtonSplit,
            this.toolStripButtonTranslation,
            this.toolStripButtonRegularExpression,
            this.toolStripButtonReplacement,
            this.toolStripButtonFilter,
            this.toolStripButtonCalculation,
            this.toolStripButtonColor,
            this.toolStripButtonFormat,
            this.toolStripButtonFeedback,
            this.toolStripTextBoxPostfix,
            this.toolStripLabelPostfix,
            this.toolStripTextBoxPrefix,
            this.toolStripLabelPrefix,
            this.toolStripButtonDelete});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(567, 25);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripLabelAdd
            // 
            this.toolStripLabelAdd.Name = "toolStripLabelAdd";
            this.toolStripLabelAdd.Size = new System.Drawing.Size(123, 22);
            this.toolStripLabelAdd.Text = "Add a transformation:";
            // 
            // toolStripButtonSplit
            // 
            this.toolStripButtonSplit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSplit.Image = global::DiversityWorkbench.Properties.Resources.Schere;
            this.toolStripButtonSplit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSplit.Name = "toolStripButtonSplit";
            this.toolStripButtonSplit.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSplit.Text = "Split the value";
            this.toolStripButtonSplit.Click += new System.EventHandler(this.toolStripButtonSplit_Click);
            // 
            // toolStripButtonTranslation
            // 
            this.toolStripButtonTranslation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTranslation.Image = global::DiversityWorkbench.Properties.Resources.Translation;
            this.toolStripButtonTranslation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTranslation.Name = "toolStripButtonTranslation";
            this.toolStripButtonTranslation.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonTranslation.Text = "Add a translation dictionary for the value";
            this.toolStripButtonTranslation.Click += new System.EventHandler(this.toolStripButtonTranslation_Click);
            // 
            // toolStripButtonRegularExpression
            // 
            this.toolStripButtonRegularExpression.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonRegularExpression.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripButtonRegularExpression.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripButtonRegularExpression.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRegularExpression.Image")));
            this.toolStripButtonRegularExpression.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRegularExpression.Name = "toolStripButtonRegularExpression";
            this.toolStripButtonRegularExpression.Size = new System.Drawing.Size(46, 22);
            this.toolStripButtonRegularExpression.Text = "RegEx";
            this.toolStripButtonRegularExpression.ToolTipText = "Add a transformation using a regular expression";
            this.toolStripButtonRegularExpression.Click += new System.EventHandler(this.toolStripButtonRegularExpression_Click);
            // 
            // toolStripButtonReplacement
            // 
            this.toolStripButtonReplacement.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonReplacement.Image = global::DiversityWorkbench.Properties.Resources.Replace;
            this.toolStripButtonReplacement.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReplacement.Name = "toolStripButtonReplacement";
            this.toolStripButtonReplacement.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonReplacement.Text = "Replace parts of the value";
            this.toolStripButtonReplacement.Click += new System.EventHandler(this.toolStripButtonReplacement_Click);
            // 
            // toolStripButtonFilter
            // 
            this.toolStripButtonFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFilter.Image = global::DiversityWorkbench.Properties.Resources.Filter;
            this.toolStripButtonFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFilter.Name = "toolStripButtonFilter";
            this.toolStripButtonFilter.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonFilter.Text = "Insert a condition for the value";
            this.toolStripButtonFilter.Click += new System.EventHandler(this.toolStripButtonFilter_Click);
            // 
            // toolStripButtonCalculation
            // 
            this.toolStripButtonCalculation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonCalculation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripButtonCalculation.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripButtonCalculation.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCalculation.Image")));
            this.toolStripButtonCalculation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCalculation.Name = "toolStripButtonCalculation";
            this.toolStripButtonCalculation.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonCalculation.Text = "∑";
            this.toolStripButtonCalculation.ToolTipText = "Calculation";
            this.toolStripButtonCalculation.Click += new System.EventHandler(this.toolStripButtonCalculation_Click);
            // 
            // toolStripButtonColor
            // 
            this.toolStripButtonColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonColor.Image = global::DiversityWorkbench.Properties.Resources.Color;
            this.toolStripButtonColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonColor.Name = "toolStripButtonColor";
            this.toolStripButtonColor.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonColor.Text = "Convert color values";
            this.toolStripButtonColor.Visible = false;
            this.toolStripButtonColor.Click += new System.EventHandler(this.toolStripButtonColor_Click);
            // 
            // toolStripButtonFormat
            // 
            this.toolStripButtonFormat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonFormat.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButtonFormat.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFormat.Image")));
            this.toolStripButtonFormat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFormat.Name = "toolStripButtonFormat";
            this.toolStripButtonFormat.Size = new System.Drawing.Size(32, 22);
            this.toolStripButtonFormat.Text = "[ F ]";
            this.toolStripButtonFormat.ToolTipText = "Fromat";
            this.toolStripButtonFormat.Visible = false;
            // 
            // toolStripButtonFeedback
            // 
            this.toolStripButtonFeedback.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonFeedback.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFeedback.Image = global::DiversityWorkbench.Properties.Resources.Feedback;
            this.toolStripButtonFeedback.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFeedback.Name = "toolStripButtonFeedback";
            this.toolStripButtonFeedback.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonFeedback.Text = "Send a feedback";
            this.toolStripButtonFeedback.Click += new System.EventHandler(this.toolStripButtonFeedback_Click);
            // 
            // toolStripTextBoxPostfix
            // 
            this.toolStripTextBoxPostfix.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripTextBoxPostfix.Name = "toolStripTextBoxPostfix";
            this.toolStripTextBoxPostfix.ReadOnly = true;
            this.toolStripTextBoxPostfix.Size = new System.Drawing.Size(20, 25);
            // 
            // toolStripLabelPostfix
            // 
            this.toolStripLabelPostfix.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabelPostfix.Name = "toolStripLabelPostfix";
            this.toolStripLabelPostfix.Size = new System.Drawing.Size(46, 22);
            this.toolStripLabelPostfix.Text = "Postfix:";
            // 
            // toolStripTextBoxPrefix
            // 
            this.toolStripTextBoxPrefix.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripTextBoxPrefix.Name = "toolStripTextBoxPrefix";
            this.toolStripTextBoxPrefix.ReadOnly = true;
            this.toolStripTextBoxPrefix.Size = new System.Drawing.Size(20, 25);
            // 
            // toolStripLabelPrefix
            // 
            this.toolStripLabelPrefix.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabelPrefix.Name = "toolStripLabelPrefix";
            this.toolStripLabelPrefix.Size = new System.Drawing.Size(40, 22);
            this.toolStripLabelPrefix.Text = "Prefix:";
            this.toolStripLabelPrefix.ToolTipText = "Prefix as defined for this column";
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDelete.Text = "Delete the current transformation";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // FormTransformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 440);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.toolStrip);
            this.helpProvider.SetHelpKeyword(this, "Export wizard transformation");
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormTransformation";
            this.helpProvider.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transformation";
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.tableLayoutPanelTest.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTest)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TabControl tabControlTransformations;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTest;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.DataGridView dataGridViewTest;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabelAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonSplit;
        private System.Windows.Forms.ToolStripButton toolStripButtonTranslation;
        private System.Windows.Forms.ToolStripButton toolStripButtonRegularExpression;
        private System.Windows.Forms.ToolStripButton toolStripButtonReplacement;
        private System.Windows.Forms.ToolStripButton toolStripButtonFilter;
        private System.Windows.Forms.ToolStripButton toolStripButtonCalculation;
        private System.Windows.Forms.ToolStripButton toolStripButtonFormat;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxPostfix;
        private System.Windows.Forms.ToolStripLabel toolStripLabelPostfix;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxPrefix;
        private System.Windows.Forms.ToolStripLabel toolStripLabelPrefix;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ImageList imageListTabControl;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.ToolStripButton toolStripButtonFeedback;
        private System.Windows.Forms.ToolStripButton toolStripButtonColor;
    }
}