
namespace DiversityWorkbench.Api
{
    partial class FormApi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApi));
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageContent = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelContent = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridViewJsonCache = new System.Windows.Forms.DataGridView();
            this.labelJsonCacheTable = new System.Windows.Forms.Label();
            this.labelJson = new System.Windows.Forms.Label();
            this.labelSourceTable = new System.Windows.Forms.Label();
            this.progressBarUpdate = new System.Windows.Forms.ProgressBar();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.textBoxJson = new System.Windows.Forms.TextBox();
            this.buttonUpdateSingle = new System.Windows.Forms.Button();
            this.comboBoxProject = new System.Windows.Forms.ComboBox();
            this.labelProject = new System.Windows.Forms.Label();
            this.buttonTest = new System.Windows.Forms.Button();
            this.comboBoxTest = new System.Windows.Forms.ComboBox();
            this.buttonShowInBrowser = new System.Windows.Forms.Button();
            this.textBoxPublic = new System.Windows.Forms.TextBox();
            this.buttonShowPublic = new System.Windows.Forms.Button();
            this.labelPublic = new System.Windows.Forms.Label();
            this.imageListTab = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tabControlMain.SuspendLayout();
            this.tabPageContent.SuspendLayout();
            this.tableLayoutPanelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJsonCache)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageContent);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.ImageList = this.imageListTab;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(592, 542);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageContent
            // 
            this.tabPageContent.Controls.Add(this.tableLayoutPanelContent);
            this.tabPageContent.ImageIndex = 0;
            this.tabPageContent.Location = new System.Drawing.Point(4, 23);
            this.tabPageContent.Name = "tabPageContent";
            this.tabPageContent.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageContent.Size = new System.Drawing.Size(584, 515);
            this.tabPageContent.TabIndex = 0;
            this.tabPageContent.Text = "Content";
            this.tabPageContent.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelContent
            // 
            this.tableLayoutPanelContent.ColumnCount = 6;
            this.tableLayoutPanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelContent.Controls.Add(this.dataGridViewJsonCache, 0, 2);
            this.tableLayoutPanelContent.Controls.Add(this.labelJsonCacheTable, 0, 1);
            this.tableLayoutPanelContent.Controls.Add(this.labelJson, 2, 1);
            this.tableLayoutPanelContent.Controls.Add(this.labelSourceTable, 0, 0);
            this.tableLayoutPanelContent.Controls.Add(this.progressBarUpdate, 0, 8);
            this.tableLayoutPanelContent.Controls.Add(this.buttonUpdate, 0, 6);
            this.tableLayoutPanelContent.Controls.Add(this.textBoxJson, 2, 2);
            this.tableLayoutPanelContent.Controls.Add(this.buttonUpdateSingle, 2, 6);
            this.tableLayoutPanelContent.Controls.Add(this.comboBoxProject, 1, 7);
            this.tableLayoutPanelContent.Controls.Add(this.labelProject, 1, 6);
            this.tableLayoutPanelContent.Controls.Add(this.buttonTest, 4, 0);
            this.tableLayoutPanelContent.Controls.Add(this.comboBoxTest, 3, 0);
            this.tableLayoutPanelContent.Controls.Add(this.buttonShowInBrowser, 5, 0);
            this.tableLayoutPanelContent.Controls.Add(this.textBoxPublic, 2, 5);
            this.tableLayoutPanelContent.Controls.Add(this.buttonShowPublic, 5, 4);
            this.tableLayoutPanelContent.Controls.Add(this.labelPublic, 2, 4);
            this.tableLayoutPanelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelContent.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelContent.Name = "tableLayoutPanelContent";
            this.tableLayoutPanelContent.RowCount = 9;
            this.tableLayoutPanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanelContent.Size = new System.Drawing.Size(578, 509);
            this.tableLayoutPanelContent.TabIndex = 1;
            // 
            // dataGridViewJsonCache
            // 
            this.dataGridViewJsonCache.AllowUserToAddRows = false;
            this.dataGridViewJsonCache.AllowUserToDeleteRows = false;
            this.dataGridViewJsonCache.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanelContent.SetColumnSpan(this.dataGridViewJsonCache, 2);
            this.dataGridViewJsonCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewJsonCache.Location = new System.Drawing.Point(3, 32);
            this.dataGridViewJsonCache.MultiSelect = false;
            this.dataGridViewJsonCache.Name = "dataGridViewJsonCache";
            this.dataGridViewJsonCache.ReadOnly = true;
            this.dataGridViewJsonCache.RowHeadersVisible = false;
            this.tableLayoutPanelContent.SetRowSpan(this.dataGridViewJsonCache, 4);
            this.dataGridViewJsonCache.Size = new System.Drawing.Size(268, 428);
            this.dataGridViewJsonCache.TabIndex = 0;
            this.dataGridViewJsonCache.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewJsonCache_CellContentClick);
            // 
            // labelJsonCacheTable
            // 
            this.labelJsonCacheTable.AutoSize = true;
            this.tableLayoutPanelContent.SetColumnSpan(this.labelJsonCacheTable, 2);
            this.labelJsonCacheTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelJsonCacheTable.Location = new System.Drawing.Point(3, 13);
            this.labelJsonCacheTable.Name = "labelJsonCacheTable";
            this.labelJsonCacheTable.Size = new System.Drawing.Size(268, 16);
            this.labelJsonCacheTable.TabIndex = 2;
            this.labelJsonCacheTable.Text = "Content of table JsonCache";
            this.labelJsonCacheTable.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelJson
            // 
            this.labelJson.AutoSize = true;
            this.labelJson.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelJson.Location = new System.Drawing.Point(277, 13);
            this.labelJson.Name = "labelJson";
            this.labelJson.Size = new System.Drawing.Size(49, 16);
            this.labelJson.TabIndex = 3;
            this.labelJson.Text = "JSON";
            this.labelJson.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelSourceTable
            // 
            this.labelSourceTable.AutoSize = true;
            this.tableLayoutPanelContent.SetColumnSpan(this.labelSourceTable, 3);
            this.labelSourceTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSourceTable.Location = new System.Drawing.Point(3, 0);
            this.labelSourceTable.Name = "labelSourceTable";
            this.labelSourceTable.Size = new System.Drawing.Size(323, 13);
            this.labelSourceTable.TabIndex = 4;
            this.labelSourceTable.Text = "label1";
            // 
            // progressBarUpdate
            // 
            this.tableLayoutPanelContent.SetColumnSpan(this.progressBarUpdate, 6);
            this.progressBarUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBarUpdate.Location = new System.Drawing.Point(3, 499);
            this.progressBarUpdate.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.progressBarUpdate.Name = "progressBarUpdate";
            this.progressBarUpdate.Size = new System.Drawing.Size(572, 7);
            this.progressBarUpdate.TabIndex = 0;
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonUpdate.Image = global::DiversityWorkbench.Properties.Resources.UpdateDatabase;
            this.buttonUpdate.Location = new System.Drawing.Point(3, 466);
            this.buttonUpdate.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.buttonUpdate.Name = "buttonUpdate";
            this.tableLayoutPanelContent.SetRowSpan(this.buttonUpdate, 2);
            this.buttonUpdate.Size = new System.Drawing.Size(131, 33);
            this.buttonUpdate.TabIndex = 1;
            this.buttonUpdate.Text = "Update database";
            this.buttonUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip.SetToolTip(this.buttonUpdate, "Update all data of the selected project");
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // textBoxJson
            // 
            this.tableLayoutPanelContent.SetColumnSpan(this.textBoxJson, 4);
            this.textBoxJson.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxJson.Location = new System.Drawing.Point(277, 32);
            this.textBoxJson.Multiline = true;
            this.textBoxJson.Name = "textBoxJson";
            this.textBoxJson.ReadOnly = true;
            this.textBoxJson.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxJson.Size = new System.Drawing.Size(298, 252);
            this.textBoxJson.TabIndex = 5;
            // 
            // buttonUpdateSingle
            // 
            this.buttonUpdateSingle.AutoSize = true;
            this.tableLayoutPanelContent.SetColumnSpan(this.buttonUpdateSingle, 4);
            this.buttonUpdateSingle.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonUpdateSingle.Image = global::DiversityWorkbench.Properties.Resources.Update;
            this.buttonUpdateSingle.Location = new System.Drawing.Point(433, 463);
            this.buttonUpdateSingle.Margin = new System.Windows.Forms.Padding(3, 0, 3, 6);
            this.buttonUpdateSingle.Name = "buttonUpdateSingle";
            this.tableLayoutPanelContent.SetRowSpan(this.buttonUpdateSingle, 2);
            this.buttonUpdateSingle.Size = new System.Drawing.Size(142, 30);
            this.buttonUpdateSingle.TabIndex = 6;
            this.buttonUpdateSingle.Text = "Update current dataset";
            this.buttonUpdateSingle.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.buttonUpdateSingle.UseVisualStyleBackColor = true;
            this.buttonUpdateSingle.Click += new System.EventHandler(this.buttonUpdateSingle_Click);
            // 
            // comboBoxProject
            // 
            this.comboBoxProject.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.comboBoxProject.FormattingEnabled = true;
            this.comboBoxProject.Location = new System.Drawing.Point(140, 477);
            this.comboBoxProject.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.comboBoxProject.Name = "comboBoxProject";
            this.comboBoxProject.Size = new System.Drawing.Size(131, 21);
            this.comboBoxProject.TabIndex = 7;
            this.toolTip.SetToolTip(this.comboBoxProject, "Select the project for which the json cache data should be updated");
            this.comboBoxProject.Visible = false;
            this.comboBoxProject.DropDown += new System.EventHandler(this.comboBoxProject_DropDown);
            // 
            // labelProject
            // 
            this.labelProject.AutoSize = true;
            this.labelProject.Location = new System.Drawing.Point(140, 463);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(40, 13);
            this.labelProject.TabIndex = 8;
            this.labelProject.Text = "Project";
            this.labelProject.UseWaitCursor = true;
            // 
            // buttonTest
            // 
            this.buttonTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTest.Location = new System.Drawing.Point(497, 3);
            this.buttonTest.Name = "buttonTest";
            this.tableLayoutPanelContent.SetRowSpan(this.buttonTest, 2);
            this.buttonTest.Size = new System.Drawing.Size(49, 23);
            this.buttonTest.TabIndex = 9;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // comboBoxTest
            // 
            this.comboBoxTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxTest.FormattingEnabled = true;
            this.comboBoxTest.Location = new System.Drawing.Point(332, 3);
            this.comboBoxTest.Name = "comboBoxTest";
            this.tableLayoutPanelContent.SetRowSpan(this.comboBoxTest, 2);
            this.comboBoxTest.Size = new System.Drawing.Size(159, 21);
            this.comboBoxTest.TabIndex = 10;
            this.comboBoxTest.Visible = false;
            // 
            // buttonShowInBrowser
            // 
            this.buttonShowInBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShowInBrowser.FlatAppearance.BorderSize = 0;
            this.buttonShowInBrowser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonShowInBrowser.Image = global::DiversityWorkbench.Properties.Resources.Lupe;
            this.buttonShowInBrowser.Location = new System.Drawing.Point(549, 0);
            this.buttonShowInBrowser.Margin = new System.Windows.Forms.Padding(0);
            this.buttonShowInBrowser.Name = "buttonShowInBrowser";
            this.tableLayoutPanelContent.SetRowSpan(this.buttonShowInBrowser, 2);
            this.buttonShowInBrowser.Size = new System.Drawing.Size(29, 29);
            this.buttonShowInBrowser.TabIndex = 11;
            this.toolTip.SetToolTip(this.buttonShowInBrowser, "Show Json Data in browser");
            this.buttonShowInBrowser.UseVisualStyleBackColor = true;
            this.buttonShowInBrowser.Click += new System.EventHandler(this.buttonShowInBrowser_Click);
            // 
            // textBoxPublic
            // 
            this.tableLayoutPanelContent.SetColumnSpan(this.textBoxPublic, 4);
            this.textBoxPublic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPublic.Location = new System.Drawing.Point(277, 310);
            this.textBoxPublic.Multiline = true;
            this.textBoxPublic.Name = "textBoxPublic";
            this.textBoxPublic.ReadOnly = true;
            this.textBoxPublic.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxPublic.Size = new System.Drawing.Size(298, 150);
            this.textBoxPublic.TabIndex = 12;
            this.textBoxPublic.Visible = false;
            // 
            // buttonShowPublic
            // 
            this.buttonShowPublic.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonShowPublic.FlatAppearance.BorderSize = 0;
            this.buttonShowPublic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonShowPublic.Image = global::DiversityWorkbench.Properties.Resources.Lupe;
            this.buttonShowPublic.Location = new System.Drawing.Point(549, 287);
            this.buttonShowPublic.Margin = new System.Windows.Forms.Padding(0);
            this.buttonShowPublic.Name = "buttonShowPublic";
            this.buttonShowPublic.Size = new System.Drawing.Size(29, 20);
            this.buttonShowPublic.TabIndex = 13;
            this.toolTip.SetToolTip(this.buttonShowPublic, "Show public data in browser");
            this.buttonShowPublic.UseVisualStyleBackColor = true;
            this.buttonShowPublic.Visible = false;
            this.buttonShowPublic.Click += new System.EventHandler(this.buttonShowPublic_Click);
            // 
            // labelPublic
            // 
            this.labelPublic.AutoSize = true;
            this.labelPublic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPublic.Location = new System.Drawing.Point(277, 287);
            this.labelPublic.Name = "labelPublic";
            this.labelPublic.Size = new System.Drawing.Size(49, 20);
            this.labelPublic.TabIndex = 14;
            this.labelPublic.Text = "Public";
            this.labelPublic.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.labelPublic.Visible = false;
            // 
            // imageListTab
            // 
            this.imageListTab.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTab.ImageStream")));
            this.imageListTab.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTab.Images.SetKeyName(0, "Lupe.ico");
            this.imageListTab.Images.SetKeyName(1, "Update.ico");
            // 
            // FormApi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 542);
            this.Controls.Add(this.tabControlMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormApi";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "JSON Interface";
            this.tabControlMain.ResumeLayout(false);
            this.tabPageContent.ResumeLayout(false);
            this.tableLayoutPanelContent.ResumeLayout(false);
            this.tableLayoutPanelContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJsonCache)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageContent;
        private System.Windows.Forms.ImageList imageListTab;
        private System.Windows.Forms.ProgressBar progressBarUpdate;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelContent;
        private System.Windows.Forms.DataGridView dataGridViewJsonCache;
        private System.Windows.Forms.Label labelJsonCacheTable;
        private System.Windows.Forms.Label labelSourceTable;
        private System.Windows.Forms.TextBox textBoxJson;
        private System.Windows.Forms.Label labelJson;
        private System.Windows.Forms.Button buttonUpdateSingle;
        private System.Windows.Forms.ComboBox comboBoxProject;
        private System.Windows.Forms.Label labelProject;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.ComboBox comboBoxTest;
        private System.Windows.Forms.Button buttonShowInBrowser;
        private System.Windows.Forms.TextBox textBoxPublic;
        private System.Windows.Forms.Button buttonShowPublic;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelPublic;
    }
}