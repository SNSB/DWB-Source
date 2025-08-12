namespace DiversityCollection.Forms
{
    partial class FormAccessionNumber
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAccessionNumber));
            tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            textBoxStart = new System.Windows.Forms.TextBox();
            labelStart = new System.Windows.Forms.Label();
            buttonSearch = new System.Windows.Forms.Button();
            labelResult = new System.Windows.Forms.Label();
            textBoxResult = new System.Windows.Forms.TextBox();
            labelHeader = new System.Windows.Forms.Label();
            checkBoxIncludeSpecimen = new System.Windows.Forms.CheckBox();
            checkBoxIncludeParts = new System.Windows.Forms.CheckBox();
            userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            helpProvider = new System.Windows.Forms.HelpProvider();
            tableLayoutPanelMain.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            resources.ApplyResources(tableLayoutPanelMain, "tableLayoutPanelMain");
            tableLayoutPanelMain.Controls.Add(textBoxStart, 2, 1);
            tableLayoutPanelMain.Controls.Add(labelStart, 0, 1);
            tableLayoutPanelMain.Controls.Add(buttonSearch, 1, 2);
            tableLayoutPanelMain.Controls.Add(labelResult, 1, 3);
            tableLayoutPanelMain.Controls.Add(textBoxResult, 2, 3);
            tableLayoutPanelMain.Controls.Add(labelHeader, 0, 0);
            tableLayoutPanelMain.Controls.Add(checkBoxIncludeSpecimen, 0, 2);
            tableLayoutPanelMain.Controls.Add(checkBoxIncludeParts, 0, 3);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            // 
            // textBoxStart
            // 
            textBoxStart.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            textBoxStart.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            resources.ApplyResources(textBoxStart, "textBoxStart");
            textBoxStart.Name = "textBoxStart";
            // 
            // labelStart
            // 
            resources.ApplyResources(labelStart, "labelStart");
            tableLayoutPanelMain.SetColumnSpan(labelStart, 2);
            labelStart.Name = "labelStart";
            // 
            // buttonSearch
            // 
            resources.ApplyResources(buttonSearch, "buttonSearch");
            buttonSearch.Name = "buttonSearch";
            buttonSearch.UseVisualStyleBackColor = true;
            buttonSearch.Click += buttonSearch_Click;
            // 
            // labelResult
            // 
            resources.ApplyResources(labelResult, "labelResult");
            labelResult.Name = "labelResult";
            // 
            // textBoxResult
            // 
            resources.ApplyResources(textBoxResult, "textBoxResult");
            textBoxResult.Name = "textBoxResult";
            textBoxResult.ReadOnly = true;
            // 
            // labelHeader
            // 
            resources.ApplyResources(labelHeader, "labelHeader");
            tableLayoutPanelMain.SetColumnSpan(labelHeader, 3);
            labelHeader.Name = "labelHeader";
            // 
            // checkBoxIncludeSpecimen
            // 
            resources.ApplyResources(checkBoxIncludeSpecimen, "checkBoxIncludeSpecimen");
            checkBoxIncludeSpecimen.Name = "checkBoxIncludeSpecimen";
            checkBoxIncludeSpecimen.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeParts
            // 
            resources.ApplyResources(checkBoxIncludeParts, "checkBoxIncludeParts");
            checkBoxIncludeParts.Name = "checkBoxIncludeParts";
            checkBoxIncludeParts.UseVisualStyleBackColor = true;
            // 
            // userControlDialogPanel
            // 
            resources.ApplyResources(userControlDialogPanel, "userControlDialogPanel");
            userControlDialogPanel.Name = "userControlDialogPanel";
            // 
            // FormAccessionNumber
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tableLayoutPanelMain);
            Controls.Add(userControlDialogPanel);
            helpProvider.SetHelpKeyword(this, resources.GetString("$this.HelpKeyword"));
            KeyPreview = true;
            Name = "FormAccessionNumber";
            helpProvider.SetShowHelp(this, (bool)resources.GetObject("$this.ShowHelp"));
            ShowInTaskbar = false;
            KeyDown += Form_KeyDown;
            tableLayoutPanelMain.ResumeLayout(false);
            tableLayoutPanelMain.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TextBox textBoxStart;
        private System.Windows.Forms.Label labelStart;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.Label labelHeader;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.CheckBox checkBoxIncludeSpecimen;
        private System.Windows.Forms.CheckBox checkBoxIncludeParts;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}