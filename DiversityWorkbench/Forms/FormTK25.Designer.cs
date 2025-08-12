namespace DiversityWorkbench.Forms
{
    partial class FormTK25
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTK25));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelOptions = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.labelQuadrant = new System.Windows.Forms.Label();
            this.comboBoxQuadrant = new System.Windows.Forms.ComboBox();
            this.buttonRequery = new System.Windows.Forms.Button();
            this.labelDatabase = new System.Windows.Forms.Label();
            this.comboBoxDatabase = new System.Windows.Forms.ComboBox();
            this.labelCurrentPosition = new System.Windows.Forms.Label();
            this.panelWebbrowser = new System.Windows.Forms.Panel();
            this.userControlGoogleMaps = new DiversityWorkbench.UserControls.UserControlGoogleMaps();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tableLayoutPanelOptions.SuspendLayout();
            this.panelWebbrowser.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.IsSplitterFixed = true;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.tableLayoutPanelOptions);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.panelWebbrowser);
            this.splitContainerMain.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainerMain.Size = new System.Drawing.Size(879, 655);
            this.splitContainerMain.SplitterDistance = 78;
            this.splitContainerMain.TabIndex = 1;
            // 
            // tableLayoutPanelOptions
            // 
            this.tableLayoutPanelOptions.ColumnCount = 4;
            this.tableLayoutPanelOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelOptions.Controls.Add(this.labelHeader, 0, 1);
            this.tableLayoutPanelOptions.Controls.Add(this.labelQuadrant, 3, 1);
            this.tableLayoutPanelOptions.Controls.Add(this.comboBoxQuadrant, 3, 2);
            this.tableLayoutPanelOptions.Controls.Add(this.buttonRequery, 1, 2);
            this.tableLayoutPanelOptions.Controls.Add(this.labelDatabase, 0, 0);
            this.tableLayoutPanelOptions.Controls.Add(this.comboBoxDatabase, 1, 0);
            this.tableLayoutPanelOptions.Controls.Add(this.labelCurrentPosition, 2, 2);
            this.tableLayoutPanelOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelOptions.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelOptions.Name = "tableLayoutPanelOptions";
            this.tableLayoutPanelOptions.RowCount = 3;
            this.tableLayoutPanelOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelOptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelOptions.Size = new System.Drawing.Size(879, 78);
            this.tableLayoutPanelOptions.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanelOptions.SetColumnSpan(this.labelHeader, 3);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 27);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(806, 24);
            this.labelHeader.TabIndex = 4;
            this.labelHeader.Text = "To set the TK25 (MTB) move the map using your mouse and set the center to the new" +
    " position with the button below.";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelQuadrant
            // 
            this.labelQuadrant.AutoSize = true;
            this.labelQuadrant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelQuadrant.Location = new System.Drawing.Point(815, 27);
            this.labelQuadrant.Name = "labelQuadrant";
            this.labelQuadrant.Size = new System.Drawing.Size(61, 24);
            this.labelQuadrant.TabIndex = 1;
            this.labelQuadrant.Text = "Quadrant";
            this.labelQuadrant.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // comboBoxQuadrant
            // 
            this.comboBoxQuadrant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxQuadrant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxQuadrant.FormattingEnabled = true;
            this.comboBoxQuadrant.Items.AddRange(new object[] {
            "1",
            "1/4",
            "1/16",
            "1/64",
            "1/256"});
            this.comboBoxQuadrant.Location = new System.Drawing.Point(815, 54);
            this.comboBoxQuadrant.Name = "comboBoxQuadrant";
            this.comboBoxQuadrant.Size = new System.Drawing.Size(61, 21);
            this.comboBoxQuadrant.TabIndex = 2;
            this.comboBoxQuadrant.SelectedIndexChanged += new System.EventHandler(this.comboBoxQuadrant_SelectedIndexChanged);
            this.comboBoxQuadrant.SelectionChangeCommitted += new System.EventHandler(this.comboBoxQuadrant_SelectionChangeCommitted);
            // 
            // buttonRequery
            // 
            this.buttonRequery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRequery.Location = new System.Drawing.Point(306, 51);
            this.buttonRequery.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRequery.Name = "buttonRequery";
            this.buttonRequery.Size = new System.Drawing.Size(200, 27);
            this.buttonRequery.TabIndex = 3;
            this.buttonRequery.Text = "Set TK25 to current position";
            this.buttonRequery.UseVisualStyleBackColor = true;
            this.buttonRequery.Click += new System.EventHandler(this.buttonRequery_Click);
            // 
            // labelDatabase
            // 
            this.labelDatabase.AutoSize = true;
            this.labelDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDatabase.Location = new System.Drawing.Point(3, 0);
            this.labelDatabase.Name = "labelDatabase";
            this.labelDatabase.Size = new System.Drawing.Size(300, 27);
            this.labelDatabase.TabIndex = 5;
            this.labelDatabase.Text = "Database:";
            this.labelDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxDatabase
            // 
            this.tableLayoutPanelOptions.SetColumnSpan(this.comboBoxDatabase, 2);
            this.comboBoxDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxDatabase.FormattingEnabled = true;
            this.comboBoxDatabase.Location = new System.Drawing.Point(309, 3);
            this.comboBoxDatabase.Name = "comboBoxDatabase";
            this.comboBoxDatabase.Size = new System.Drawing.Size(500, 21);
            this.comboBoxDatabase.TabIndex = 6;
            this.comboBoxDatabase.SelectionChangeCommitted += new System.EventHandler(this.comboBoxDatabase_SelectionChangeCommitted);
            // 
            // labelCurrentPosition
            // 
            this.labelCurrentPosition.AutoSize = true;
            this.labelCurrentPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCurrentPosition.Location = new System.Drawing.Point(509, 51);
            this.labelCurrentPosition.Name = "labelCurrentPosition";
            this.labelCurrentPosition.Size = new System.Drawing.Size(300, 27);
            this.labelCurrentPosition.TabIndex = 7;
            this.labelCurrentPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelWebbrowser
            // 
            this.panelWebbrowser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelWebbrowser.Controls.Add(this.userControlGoogleMaps);
            this.panelWebbrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWebbrowser.Location = new System.Drawing.Point(3, 3);
            this.panelWebbrowser.Name = "panelWebbrowser";
            this.panelWebbrowser.Size = new System.Drawing.Size(873, 567);
            this.panelWebbrowser.TabIndex = 0;
            // 
            // userControlGoogleMaps
            // 
            this.userControlGoogleMaps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlGoogleMaps.Location = new System.Drawing.Point(0, 0);
            this.userControlGoogleMaps.Name = "userControlGoogleMaps";
            this.userControlGoogleMaps.Size = new System.Drawing.Size(871, 565);
            this.userControlGoogleMaps.TabIndex = 0;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 655);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(879, 27);
            this.userControlDialogPanel.TabIndex = 0;
            // 
            // FormTK25
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 682);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormTK25";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " TK25";
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.tableLayoutPanelOptions.ResumeLayout(false);
            this.tableLayoutPanelOptions.PerformLayout();
            this.panelWebbrowser.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Panel panelWebbrowser;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelOptions;
        private DiversityWorkbench.UserControls.UserControlGoogleMaps userControlGoogleMaps;
        private System.Windows.Forms.Label labelQuadrant;
        private System.Windows.Forms.ComboBox comboBoxQuadrant;
        private System.Windows.Forms.Button buttonRequery;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.ComboBox comboBoxDatabase;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Label labelCurrentPosition;
    }
}