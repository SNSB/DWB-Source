namespace DiversitySamplingPlots
{
    partial class FormNewObject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNewObject));
            this.userControlDialogPanel1 = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.tableLayoutPanelSelector = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonFromList = new System.Windows.Forms.RadioButton();
            this.radioButtonNew = new System.Windows.Forms.RadioButton();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.listBoxObjects = new System.Windows.Forms.ListBox();
            this.tableLayoutPanelObjectDetails = new System.Windows.Forms.TableLayoutPanel();
            this.labelObjectDescription = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryCollectionSpecimen = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.textBoxObjectDisplayText = new System.Windows.Forms.TextBox();
            this.textBoxObjectsDescription = new System.Windows.Forms.TextBox();
            this.labelObjectDisplayText = new System.Windows.Forms.Label();
            this.tableLayoutPanelSelector.SuspendLayout();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tableLayoutPanelObjectDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // userControlDialogPanel1
            // 
            this.userControlDialogPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel1.Location = new System.Drawing.Point(0, 199);
            this.userControlDialogPanel1.Name = "userControlDialogPanel1";
            this.userControlDialogPanel1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel1.Size = new System.Drawing.Size(386, 27);
            this.userControlDialogPanel1.TabIndex = 0;
            // 
            // tableLayoutPanelSelector
            // 
            this.tableLayoutPanelSelector.ColumnCount = 2;
            this.tableLayoutPanelSelector.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSelector.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSelector.Controls.Add(this.radioButtonFromList, 0, 0);
            this.tableLayoutPanelSelector.Controls.Add(this.radioButtonNew, 1, 0);
            this.tableLayoutPanelSelector.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelSelector.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSelector.Name = "tableLayoutPanelSelector";
            this.tableLayoutPanelSelector.RowCount = 1;
            this.tableLayoutPanelSelector.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSelector.Size = new System.Drawing.Size(386, 28);
            this.tableLayoutPanelSelector.TabIndex = 1;
            // 
            // radioButtonFromList
            // 
            this.radioButtonFromList.AutoSize = true;
            this.radioButtonFromList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonFromList.Location = new System.Drawing.Point(3, 3);
            this.radioButtonFromList.Name = "radioButtonFromList";
            this.radioButtonFromList.Size = new System.Drawing.Size(187, 22);
            this.radioButtonFromList.TabIndex = 0;
            this.radioButtonFromList.TabStop = true;
            this.radioButtonFromList.Text = "Choose from existing objects";
            this.radioButtonFromList.UseVisualStyleBackColor = true;
            this.radioButtonFromList.CheckedChanged += new System.EventHandler(this.radioButtonFromList_CheckedChanged);
            // 
            // radioButtonNew
            // 
            this.radioButtonNew.AutoSize = true;
            this.radioButtonNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonNew.Location = new System.Drawing.Point(196, 3);
            this.radioButtonNew.Name = "radioButtonNew";
            this.radioButtonNew.Size = new System.Drawing.Size(187, 22);
            this.radioButtonNew.TabIndex = 1;
            this.radioButtonNew.TabStop = true;
            this.radioButtonNew.Text = "Create a new object";
            this.radioButtonNew.UseVisualStyleBackColor = true;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 28);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.listBoxObjects);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tableLayoutPanelObjectDetails);
            this.splitContainerMain.Size = new System.Drawing.Size(386, 171);
            this.splitContainerMain.SplitterDistance = 192;
            this.splitContainerMain.TabIndex = 2;
            // 
            // listBoxObjects
            // 
            this.listBoxObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxObjects.FormattingEnabled = true;
            this.listBoxObjects.IntegralHeight = false;
            this.listBoxObjects.Location = new System.Drawing.Point(0, 0);
            this.listBoxObjects.Name = "listBoxObjects";
            this.listBoxObjects.Size = new System.Drawing.Size(192, 171);
            this.listBoxObjects.TabIndex = 0;
            // 
            // tableLayoutPanelObjectDetails
            // 
            this.tableLayoutPanelObjectDetails.ColumnCount = 1;
            this.tableLayoutPanelObjectDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelObjectDetails.Controls.Add(this.labelObjectDescription, 0, 2);
            this.tableLayoutPanelObjectDetails.Controls.Add(this.userControlModuleRelatedEntryCollectionSpecimen, 0, 4);
            this.tableLayoutPanelObjectDetails.Controls.Add(this.textBoxObjectDisplayText, 0, 1);
            this.tableLayoutPanelObjectDetails.Controls.Add(this.textBoxObjectsDescription, 0, 3);
            this.tableLayoutPanelObjectDetails.Controls.Add(this.labelObjectDisplayText, 0, 0);
            this.tableLayoutPanelObjectDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelObjectDetails.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelObjectDetails.Name = "tableLayoutPanelObjectDetails";
            this.tableLayoutPanelObjectDetails.RowCount = 5;
            this.tableLayoutPanelObjectDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelObjectDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelObjectDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelObjectDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelObjectDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelObjectDetails.Size = new System.Drawing.Size(190, 171);
            this.tableLayoutPanelObjectDetails.TabIndex = 1;
            // 
            // labelObjectDescription
            // 
            this.labelObjectDescription.AutoSize = true;
            this.labelObjectDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelObjectDescription.Location = new System.Drawing.Point(0, 36);
            this.labelObjectDescription.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelObjectDescription.Name = "labelObjectDescription";
            this.labelObjectDescription.Size = new System.Drawing.Size(187, 13);
            this.labelObjectDescription.TabIndex = 0;
            this.labelObjectDescription.Text = "Description";
            // 
            // userControlModuleRelatedEntryCollectionSpecimen
            // 
            this.userControlModuleRelatedEntryCollectionSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryCollectionSpecimen.Location = new System.Drawing.Point(0, 149);
            this.userControlModuleRelatedEntryCollectionSpecimen.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.userControlModuleRelatedEntryCollectionSpecimen.Module = null;
            this.userControlModuleRelatedEntryCollectionSpecimen.Name = "userControlModuleRelatedEntryCollectionSpecimen";
            this.userControlModuleRelatedEntryCollectionSpecimen.Size = new System.Drawing.Size(187, 22);
            this.userControlModuleRelatedEntryCollectionSpecimen.TabIndex = 1;
            // 
            // textBoxObjectDisplayText
            // 
            this.textBoxObjectDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxObjectDisplayText.Location = new System.Drawing.Point(0, 13);
            this.textBoxObjectDisplayText.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxObjectDisplayText.Name = "textBoxObjectDisplayText";
            this.textBoxObjectDisplayText.Size = new System.Drawing.Size(187, 20);
            this.textBoxObjectDisplayText.TabIndex = 2;
            // 
            // textBoxObjectsDescription
            // 
            this.textBoxObjectsDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxObjectsDescription.Location = new System.Drawing.Point(0, 49);
            this.textBoxObjectsDescription.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxObjectsDescription.Multiline = true;
            this.textBoxObjectsDescription.Name = "textBoxObjectsDescription";
            this.textBoxObjectsDescription.Size = new System.Drawing.Size(187, 94);
            this.textBoxObjectsDescription.TabIndex = 3;
            // 
            // labelObjectDisplayText
            // 
            this.labelObjectDisplayText.AutoSize = true;
            this.labelObjectDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelObjectDisplayText.Location = new System.Drawing.Point(0, 0);
            this.labelObjectDisplayText.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelObjectDisplayText.Name = "labelObjectDisplayText";
            this.labelObjectDisplayText.Size = new System.Drawing.Size(187, 13);
            this.labelObjectDisplayText.TabIndex = 4;
            this.labelObjectDisplayText.Text = "Display text";
            // 
            // FormNewObject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 226);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.tableLayoutPanelSelector);
            this.Controls.Add(this.userControlDialogPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormNewObject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Insert a new object";
            this.tableLayoutPanelSelector.ResumeLayout(false);
            this.tableLayoutPanelSelector.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.tableLayoutPanelObjectDetails.ResumeLayout(false);
            this.tableLayoutPanelObjectDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSelector;
        private System.Windows.Forms.RadioButton radioButtonFromList;
        private System.Windows.Forms.RadioButton radioButtonNew;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.ListBox listBoxObjects;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelObjectDetails;
        private System.Windows.Forms.Label labelObjectDescription;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryCollectionSpecimen;
        private System.Windows.Forms.TextBox textBoxObjectDisplayText;
        private System.Windows.Forms.TextBox textBoxObjectsDescription;
        private System.Windows.Forms.Label labelObjectDisplayText;
    }
}