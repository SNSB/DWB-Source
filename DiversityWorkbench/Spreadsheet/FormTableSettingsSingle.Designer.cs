namespace DiversityWorkbench.Spreadsheet
{
    partial class FormTableSettingsSingle
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTableSettingsSingle));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.pictureBoxTableImage = new System.Windows.Forms.PictureBox();
            this.labelHeader = new System.Windows.Forms.Label();
            this.panelTable = new System.Windows.Forms.Panel();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelSelection = new System.Windows.Forms.TableLayoutPanel();
            this.labelTableSelectionHeader = new System.Windows.Forms.Label();
            this.panelSelection = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTableImage)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tableLayoutPanelSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 3;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.userControlDialogPanel, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonFeedback, 2, 0);
            this.tableLayoutPanelMain.Controls.Add(this.pictureBoxTableImage, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.panelTable, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(456, 453);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // userControlDialogPanel
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.userControlDialogPanel, 3);
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDialogPanel.Location = new System.Drawing.Point(3, 423);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(450, 27);
            this.userControlDialogPanel.TabIndex = 0;
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFeedback.FlatAppearance.BorderSize = 0;
            this.buttonFeedback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFeedback.Image = global::DiversityWorkbench.Properties.Resources.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(433, 3);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(20, 20);
            this.buttonFeedback.TabIndex = 1;
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // pictureBoxTableImage
            // 
            this.pictureBoxTableImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxTableImage.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxTableImage.Name = "pictureBoxTableImage";
            this.pictureBoxTableImage.Size = new System.Drawing.Size(16, 20);
            this.pictureBoxTableImage.TabIndex = 2;
            this.pictureBoxTableImage.TabStop = false;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(25, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(402, 26);
            this.labelHeader.TabIndex = 3;
            this.labelHeader.Text = "Please select the columns that should be displayed in the spreadsheet";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTable
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.panelTable, 3);
            this.panelTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTable.Location = new System.Drawing.Point(3, 29);
            this.panelTable.Name = "panelTable";
            this.panelTable.Size = new System.Drawing.Size(450, 388);
            this.panelTable.TabIndex = 4;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.tableLayoutPanelSelection);
            this.splitContainerMain.Panel1.Padding = new System.Windows.Forms.Padding(10);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tableLayoutPanelMain);
            this.splitContainerMain.Size = new System.Drawing.Size(849, 453);
            this.splitContainerMain.SplitterDistance = 389;
            this.splitContainerMain.TabIndex = 1;
            // 
            // tableLayoutPanelSelection
            // 
            this.tableLayoutPanelSelection.ColumnCount = 3;
            this.tableLayoutPanelSelection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSelection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSelection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSelection.Controls.Add(this.labelTableSelectionHeader, 1, 0);
            this.tableLayoutPanelSelection.Controls.Add(this.panelSelection, 1, 1);
            this.tableLayoutPanelSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSelection.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanelSelection.Name = "tableLayoutPanelSelection";
            this.tableLayoutPanelSelection.RowCount = 2;
            this.tableLayoutPanelSelection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSelection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSelection.Size = new System.Drawing.Size(369, 433);
            this.tableLayoutPanelSelection.TabIndex = 1;
            // 
            // labelTableSelectionHeader
            // 
            this.labelTableSelectionHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTableSelectionHeader.Location = new System.Drawing.Point(64, 0);
            this.labelTableSelectionHeader.Name = "labelTableSelectionHeader";
            this.labelTableSelectionHeader.Size = new System.Drawing.Size(240, 30);
            this.labelTableSelectionHeader.TabIndex = 0;
            this.labelTableSelectionHeader.Text = "Please select the table that should be added";
            this.labelTableSelectionHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelSelection
            // 
            this.panelSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSelection.Location = new System.Drawing.Point(64, 33);
            this.panelSelection.Name = "panelSelection";
            this.panelSelection.Size = new System.Drawing.Size(240, 397);
            this.panelSelection.TabIndex = 1;
            // 
            // FormTableSettingsSingle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 453);
            this.Controls.Add(this.splitContainerMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormTableSettingsSingle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormTableSettingsSingle";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTableSettings_FormClosing);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTableImage)).EndInit();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.tableLayoutPanelSelection.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.PictureBox pictureBoxTableImage;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Panel panelTable;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelTableSelectionHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSelection;
        private System.Windows.Forms.Panel panelSelection;
    }
}