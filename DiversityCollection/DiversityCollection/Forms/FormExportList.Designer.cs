namespace DiversityCollection.Forms
{
    partial class FormExportList
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExportList));
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxFilePath = new System.Windows.Forms.TextBox();
            this.checkBoxForReimport = new System.Windows.Forms.CheckBox();
            this.buttonStartExport = new System.Windows.Forms.Button();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.checkBoxIncludeSQL = new System.Windows.Forms.CheckBox();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.labelOrderBy = new System.Windows.Forms.Label();
            this.comboBoxOrderBy = new System.Windows.Forms.ComboBox();
            this.labelPreview1 = new System.Windows.Forms.Label();
            this.labelPreview2 = new System.Windows.Forms.Label();
            this.numericUpDownPreview = new System.Windows.Forms.NumericUpDown();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.buttonTimeout = new System.Windows.Forms.Button();
            this.labelCurrentAction = new System.Windows.Forms.Label();
            this.progressBarCurrentAction = new System.Windows.Forms.ProgressBar();
            this.radioButtonOuterJoin = new System.Windows.Forms.RadioButton();
            this.radioButtonInnerJoin = new System.Windows.Forms.RadioButton();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.toolTip = new System.Windows.Forms.ToolTip();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
            this.tableLayoutPanel.Controls.Add(this.textBoxFilePath, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.checkBoxForReimport, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonStartExport, 8, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonOpenFile, 10, 0);
            this.tableLayoutPanel.Controls.Add(this.checkBoxIncludeSQL, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonSettings, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxResult, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.labelOrderBy, 6, 1);
            this.tableLayoutPanel.Controls.Add(this.comboBoxOrderBy, 7, 1);
            this.tableLayoutPanel.Controls.Add(this.labelPreview1, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.labelPreview2, 5, 1);
            this.tableLayoutPanel.Controls.Add(this.numericUpDownPreview, 4, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonFeedback, 10, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonTimeout, 9, 1);
            this.tableLayoutPanel.Controls.Add(this.labelCurrentAction, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.progressBarCurrentAction, 2, 4);
            this.tableLayoutPanel.Controls.Add(this.radioButtonOuterJoin, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.radioButtonInnerJoin, 2, 2);
            this.helpProvider.SetHelpKeyword(this.tableLayoutPanel, resources.GetString("tableLayoutPanel.HelpKeyword"));
            this.helpProvider.SetHelpNavigator(this.tableLayoutPanel, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("tableLayoutPanel.HelpNavigator"))));
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.helpProvider.SetShowHelp(this.tableLayoutPanel, ((bool)(resources.GetObject("tableLayoutPanel.ShowHelp"))));
            // 
            // textBoxFilePath
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textBoxFilePath, 10);
            resources.ApplyResources(this.textBoxFilePath, "textBoxFilePath");
            this.textBoxFilePath.Name = "textBoxFilePath";
            this.helpProvider.SetShowHelp(this.textBoxFilePath, ((bool)(resources.GetObject("textBoxFilePath.ShowHelp"))));
            // 
            // checkBoxForReimport
            // 
            resources.ApplyResources(this.checkBoxForReimport, "checkBoxForReimport");
            this.checkBoxForReimport.Name = "checkBoxForReimport";
            this.helpProvider.SetShowHelp(this.checkBoxForReimport, ((bool)(resources.GetObject("checkBoxForReimport.ShowHelp"))));
            this.checkBoxForReimport.UseVisualStyleBackColor = true;
            // 
            // buttonStartExport
            // 
            resources.ApplyResources(this.buttonStartExport, "buttonStartExport");
            this.buttonStartExport.Name = "buttonStartExport";
            this.helpProvider.SetShowHelp(this.buttonStartExport, ((bool)(resources.GetObject("buttonStartExport.ShowHelp"))));
            this.buttonStartExport.UseVisualStyleBackColor = true;
            this.buttonStartExport.Click += new System.EventHandler(this.buttonStartExport_Click);
            // 
            // buttonOpenFile
            // 
            resources.ApplyResources(this.buttonOpenFile, "buttonOpenFile");
            this.buttonOpenFile.Image = global::DiversityCollection.Resource.Open;
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.helpProvider.SetShowHelp(this.buttonOpenFile, ((bool)(resources.GetObject("buttonOpenFile.ShowHelp"))));
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // checkBoxIncludeSQL
            // 
            resources.ApplyResources(this.checkBoxIncludeSQL, "checkBoxIncludeSQL");
            this.checkBoxIncludeSQL.Name = "checkBoxIncludeSQL";
            this.helpProvider.SetShowHelp(this.checkBoxIncludeSQL, ((bool)(resources.GetObject("checkBoxIncludeSQL.ShowHelp"))));
            this.checkBoxIncludeSQL.UseVisualStyleBackColor = true;
            // 
            // buttonSettings
            // 
            resources.ApplyResources(this.buttonSettings, "buttonSettings");
            this.buttonSettings.Name = "buttonSettings";
            this.helpProvider.SetShowHelp(this.buttonSettings, ((bool)(resources.GetObject("buttonSettings.ShowHelp"))));
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // textBoxResult
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textBoxResult, 11);
            resources.ApplyResources(this.textBoxResult, "textBoxResult");
            this.textBoxResult.Name = "textBoxResult";
            this.helpProvider.SetShowHelp(this.textBoxResult, ((bool)(resources.GetObject("textBoxResult.ShowHelp"))));
            // 
            // labelOrderBy
            // 
            resources.ApplyResources(this.labelOrderBy, "labelOrderBy");
            this.labelOrderBy.Name = "labelOrderBy";
            this.helpProvider.SetShowHelp(this.labelOrderBy, ((bool)(resources.GetObject("labelOrderBy.ShowHelp"))));
            // 
            // comboBoxOrderBy
            // 
            resources.ApplyResources(this.comboBoxOrderBy, "comboBoxOrderBy");
            this.comboBoxOrderBy.FormattingEnabled = true;
            this.comboBoxOrderBy.Name = "comboBoxOrderBy";
            this.helpProvider.SetShowHelp(this.comboBoxOrderBy, ((bool)(resources.GetObject("comboBoxOrderBy.ShowHelp"))));
            // 
            // labelPreview1
            // 
            resources.ApplyResources(this.labelPreview1, "labelPreview1");
            this.labelPreview1.Name = "labelPreview1";
            this.helpProvider.SetShowHelp(this.labelPreview1, ((bool)(resources.GetObject("labelPreview1.ShowHelp"))));
            // 
            // labelPreview2
            // 
            resources.ApplyResources(this.labelPreview2, "labelPreview2");
            this.labelPreview2.Name = "labelPreview2";
            this.helpProvider.SetShowHelp(this.labelPreview2, ((bool)(resources.GetObject("labelPreview2.ShowHelp"))));
            // 
            // numericUpDownPreview
            // 
            resources.ApplyResources(this.numericUpDownPreview, "numericUpDownPreview");
            this.numericUpDownPreview.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownPreview.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownPreview.Name = "numericUpDownPreview";
            this.helpProvider.SetShowHelp(this.numericUpDownPreview, ((bool)(resources.GetObject("numericUpDownPreview.ShowHelp"))));
            this.toolTip.SetToolTip(this.numericUpDownPreview, resources.GetString("numericUpDownPreview.ToolTip"));
            this.numericUpDownPreview.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Image = global::DiversityCollection.Resource.Feedback;
            resources.ApplyResources(this.buttonFeedback, "buttonFeedback");
            this.buttonFeedback.Name = "buttonFeedback";
            this.helpProvider.SetShowHelp(this.buttonFeedback, ((bool)(resources.GetObject("buttonFeedback.ShowHelp"))));
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // buttonTimeout
            // 
            this.buttonTimeout.Image = global::DiversityCollection.Resource.Time;
            resources.ApplyResources(this.buttonTimeout, "buttonTimeout");
            this.buttonTimeout.Name = "buttonTimeout";
            this.toolTip.SetToolTip(this.buttonTimeout, resources.GetString("buttonTimeout.ToolTip"));
            this.buttonTimeout.UseVisualStyleBackColor = true;
            this.buttonTimeout.Click += new System.EventHandler(this.buttonTimeout_Click);
            // 
            // labelCurrentAction
            // 
            resources.ApplyResources(this.labelCurrentAction, "labelCurrentAction");
            this.tableLayoutPanel.SetColumnSpan(this.labelCurrentAction, 2);
            this.labelCurrentAction.Name = "labelCurrentAction";
            // 
            // progressBarCurrentAction
            // 
            this.tableLayoutPanel.SetColumnSpan(this.progressBarCurrentAction, 9);
            resources.ApplyResources(this.progressBarCurrentAction, "progressBarCurrentAction");
            this.progressBarCurrentAction.Name = "progressBarCurrentAction";
            // 
            // radioButtonOuterJoin
            // 
            resources.ApplyResources(this.radioButtonOuterJoin, "radioButtonOuterJoin");
            this.radioButtonOuterJoin.Checked = true;
            this.radioButtonOuterJoin.Name = "radioButtonOuterJoin";
            this.radioButtonOuterJoin.TabStop = true;
            this.toolTip.SetToolTip(this.radioButtonOuterJoin, resources.GetString("radioButtonOuterJoin.ToolTip"));
            this.radioButtonOuterJoin.UseVisualStyleBackColor = true;
            // 
            // radioButtonInnerJoin
            // 
            resources.ApplyResources(this.radioButtonInnerJoin, "radioButtonInnerJoin");
            this.radioButtonInnerJoin.Name = "radioButtonInnerJoin";
            this.toolTip.SetToolTip(this.radioButtonInnerJoin, resources.GetString("radioButtonInnerJoin.ToolTip"));
            this.radioButtonInnerJoin.UseVisualStyleBackColor = true;
            // 
            // FormExportList
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "FormExportList";
            this.helpProvider.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.TextBox textBoxFilePath;
        private System.Windows.Forms.CheckBox checkBoxForReimport;
        private System.Windows.Forms.Button buttonStartExport;
        private System.Windows.Forms.CheckBox checkBoxIncludeSQL;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Label labelOrderBy;
        private System.Windows.Forms.ComboBox comboBoxOrderBy;
        private System.Windows.Forms.Label labelPreview1;
        private System.Windows.Forms.Label labelPreview2;
        private System.Windows.Forms.NumericUpDown numericUpDownPreview;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.Button buttonTimeout;
        private System.Windows.Forms.Label labelCurrentAction;
        private System.Windows.Forms.ProgressBar progressBarCurrentAction;
        private System.Windows.Forms.RadioButton radioButtonOuterJoin;
        private System.Windows.Forms.RadioButton radioButtonInnerJoin;
    }
}