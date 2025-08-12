namespace DiversityCollection.Forms
{
    partial class FormIdentificationUnitGridNewEntry
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIdentificationUnitGridNewEntry));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.buttonNewSpecimen = new System.Windows.Forms.Button();
            this.buttonSameSpecimen = new System.Windows.Forms.Button();
            this.treeViewSameEvent = new System.Windows.Forms.TreeView();
            this.treeViewNewEvent = new System.Windows.Forms.TreeView();
            this.labelHeaderEvent = new System.Windows.Forms.Label();
            this.groupBoxNewData = new System.Windows.Forms.GroupBox();
            this.treeViewCopy = new System.Windows.Forms.TreeView();
            this.groupBoxOldData = new System.Windows.Forms.GroupBox();
            this.treeViewOriginal = new System.Windows.Forms.TreeView();
            this.textBoxAccessionNumber = new System.Windows.Forms.TextBox();
            this.labelAccessionNumber = new System.Windows.Forms.Label();
            this.buttonFindNextAccessionNumber = new System.Windows.Forms.Button();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanelMain.SuspendLayout();
            this.groupBoxNewData.SuspendLayout();
            this.groupBoxOldData.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.AccessibleDescription = null;
            this.tableLayoutPanelMain.AccessibleName = null;
            resources.ApplyResources(this.tableLayoutPanelMain, "tableLayoutPanelMain");
            this.tableLayoutPanelMain.BackgroundImage = null;
            this.tableLayoutPanelMain.Controls.Add(this.buttonNewSpecimen, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.buttonSameSpecimen, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.treeViewSameEvent, 2, 2);
            this.tableLayoutPanelMain.Controls.Add(this.treeViewNewEvent, 2, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelHeaderEvent, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.groupBoxNewData, 3, 1);
            this.tableLayoutPanelMain.Controls.Add(this.groupBoxOldData, 3, 0);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxAccessionNumber, 4, 4);
            this.tableLayoutPanelMain.Controls.Add(this.labelAccessionNumber, 2, 4);
            this.tableLayoutPanelMain.Controls.Add(this.buttonFindNextAccessionNumber, 5, 4);
            this.tableLayoutPanelMain.Font = null;
            this.helpProvider.SetHelpKeyword(this.tableLayoutPanelMain, null);
            this.helpProvider.SetHelpNavigator(this.tableLayoutPanelMain, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("tableLayoutPanelMain.HelpNavigator"))));
            this.helpProvider.SetHelpString(this.tableLayoutPanelMain, null);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelMain, ((bool)(resources.GetObject("tableLayoutPanelMain.ShowHelp"))));
            this.toolTip.SetToolTip(this.tableLayoutPanelMain, resources.GetString("tableLayoutPanelMain.ToolTip"));
            // 
            // buttonNewSpecimen
            // 
            this.buttonNewSpecimen.AccessibleDescription = null;
            this.buttonNewSpecimen.AccessibleName = null;
            resources.ApplyResources(this.buttonNewSpecimen, "buttonNewSpecimen");
            this.buttonNewSpecimen.BackgroundImage = null;
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonNewSpecimen, 2);
            this.buttonNewSpecimen.Font = null;
            this.helpProvider.SetHelpKeyword(this.buttonNewSpecimen, null);
            this.helpProvider.SetHelpNavigator(this.buttonNewSpecimen, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("buttonNewSpecimen.HelpNavigator"))));
            this.helpProvider.SetHelpString(this.buttonNewSpecimen, null);
            this.buttonNewSpecimen.Name = "buttonNewSpecimen";
            this.tableLayoutPanelMain.SetRowSpan(this.buttonNewSpecimen, 2);
            this.helpProvider.SetShowHelp(this.buttonNewSpecimen, ((bool)(resources.GetObject("buttonNewSpecimen.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonNewSpecimen, resources.GetString("buttonNewSpecimen.ToolTip"));
            this.buttonNewSpecimen.UseVisualStyleBackColor = true;
            this.buttonNewSpecimen.Click += new System.EventHandler(this.buttonNewSpecimen_Click);
            // 
            // buttonSameSpecimen
            // 
            this.buttonSameSpecimen.AccessibleDescription = null;
            this.buttonSameSpecimen.AccessibleName = null;
            resources.ApplyResources(this.buttonSameSpecimen, "buttonSameSpecimen");
            this.buttonSameSpecimen.BackgroundImage = null;
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonSameSpecimen, 2);
            this.buttonSameSpecimen.Font = null;
            this.helpProvider.SetHelpKeyword(this.buttonSameSpecimen, null);
            this.helpProvider.SetHelpNavigator(this.buttonSameSpecimen, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("buttonSameSpecimen.HelpNavigator"))));
            this.helpProvider.SetHelpString(this.buttonSameSpecimen, null);
            this.buttonSameSpecimen.Name = "buttonSameSpecimen";
            this.helpProvider.SetShowHelp(this.buttonSameSpecimen, ((bool)(resources.GetObject("buttonSameSpecimen.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonSameSpecimen, resources.GetString("buttonSameSpecimen.ToolTip"));
            this.buttonSameSpecimen.UseVisualStyleBackColor = true;
            this.buttonSameSpecimen.Click += new System.EventHandler(this.buttonSameSpecimen_Click);
            // 
            // treeViewSameEvent
            // 
            this.treeViewSameEvent.AccessibleDescription = null;
            this.treeViewSameEvent.AccessibleName = null;
            resources.ApplyResources(this.treeViewSameEvent, "treeViewSameEvent");
            this.treeViewSameEvent.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewSameEvent.BackgroundImage = null;
            this.treeViewSameEvent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelMain.SetColumnSpan(this.treeViewSameEvent, 5);
            this.treeViewSameEvent.Font = null;
            this.helpProvider.SetHelpKeyword(this.treeViewSameEvent, null);
            this.helpProvider.SetHelpNavigator(this.treeViewSameEvent, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("treeViewSameEvent.HelpNavigator"))));
            this.helpProvider.SetHelpString(this.treeViewSameEvent, null);
            this.treeViewSameEvent.Name = "treeViewSameEvent";
            this.treeViewSameEvent.Scrollable = false;
            this.helpProvider.SetShowHelp(this.treeViewSameEvent, ((bool)(resources.GetObject("treeViewSameEvent.ShowHelp"))));
            this.toolTip.SetToolTip(this.treeViewSameEvent, resources.GetString("treeViewSameEvent.ToolTip"));
            // 
            // treeViewNewEvent
            // 
            this.treeViewNewEvent.AccessibleDescription = null;
            this.treeViewNewEvent.AccessibleName = null;
            resources.ApplyResources(this.treeViewNewEvent, "treeViewNewEvent");
            this.treeViewNewEvent.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewNewEvent.BackgroundImage = null;
            this.treeViewNewEvent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelMain.SetColumnSpan(this.treeViewNewEvent, 5);
            this.treeViewNewEvent.Font = null;
            this.helpProvider.SetHelpKeyword(this.treeViewNewEvent, null);
            this.helpProvider.SetHelpNavigator(this.treeViewNewEvent, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("treeViewNewEvent.HelpNavigator"))));
            this.helpProvider.SetHelpString(this.treeViewNewEvent, null);
            this.treeViewNewEvent.Name = "treeViewNewEvent";
            this.treeViewNewEvent.Scrollable = false;
            this.helpProvider.SetShowHelp(this.treeViewNewEvent, ((bool)(resources.GetObject("treeViewNewEvent.ShowHelp"))));
            this.toolTip.SetToolTip(this.treeViewNewEvent, resources.GetString("treeViewNewEvent.ToolTip"));
            // 
            // labelHeaderEvent
            // 
            this.labelHeaderEvent.AccessibleDescription = null;
            this.labelHeaderEvent.AccessibleName = null;
            resources.ApplyResources(this.labelHeaderEvent, "labelHeaderEvent");
            this.tableLayoutPanelMain.SetColumnSpan(this.labelHeaderEvent, 2);
            this.labelHeaderEvent.Font = null;
            this.helpProvider.SetHelpKeyword(this.labelHeaderEvent, null);
            this.helpProvider.SetHelpNavigator(this.labelHeaderEvent, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("labelHeaderEvent.HelpNavigator"))));
            this.helpProvider.SetHelpString(this.labelHeaderEvent, null);
            this.labelHeaderEvent.Name = "labelHeaderEvent";
            this.tableLayoutPanelMain.SetRowSpan(this.labelHeaderEvent, 2);
            this.helpProvider.SetShowHelp(this.labelHeaderEvent, ((bool)(resources.GetObject("labelHeaderEvent.ShowHelp"))));
            this.toolTip.SetToolTip(this.labelHeaderEvent, resources.GetString("labelHeaderEvent.ToolTip"));
            // 
            // groupBoxNewData
            // 
            this.groupBoxNewData.AccessibleDescription = null;
            this.groupBoxNewData.AccessibleName = null;
            resources.ApplyResources(this.groupBoxNewData, "groupBoxNewData");
            this.groupBoxNewData.BackgroundImage = null;
            this.tableLayoutPanelMain.SetColumnSpan(this.groupBoxNewData, 3);
            this.groupBoxNewData.Controls.Add(this.treeViewCopy);
            this.groupBoxNewData.Font = null;
            this.helpProvider.SetHelpKeyword(this.groupBoxNewData, null);
            this.helpProvider.SetHelpNavigator(this.groupBoxNewData, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("groupBoxNewData.HelpNavigator"))));
            this.helpProvider.SetHelpString(this.groupBoxNewData, null);
            this.groupBoxNewData.Name = "groupBoxNewData";
            this.helpProvider.SetShowHelp(this.groupBoxNewData, ((bool)(resources.GetObject("groupBoxNewData.ShowHelp"))));
            this.groupBoxNewData.TabStop = false;
            this.toolTip.SetToolTip(this.groupBoxNewData, resources.GetString("groupBoxNewData.ToolTip"));
            // 
            // treeViewCopy
            // 
            this.treeViewCopy.AccessibleDescription = null;
            this.treeViewCopy.AccessibleName = null;
            resources.ApplyResources(this.treeViewCopy, "treeViewCopy");
            this.treeViewCopy.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewCopy.BackgroundImage = null;
            this.treeViewCopy.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewCopy.Font = null;
            this.helpProvider.SetHelpKeyword(this.treeViewCopy, null);
            this.helpProvider.SetHelpNavigator(this.treeViewCopy, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("treeViewCopy.HelpNavigator"))));
            this.helpProvider.SetHelpString(this.treeViewCopy, null);
            this.treeViewCopy.Name = "treeViewCopy";
            this.treeViewCopy.Scrollable = false;
            this.helpProvider.SetShowHelp(this.treeViewCopy, ((bool)(resources.GetObject("treeViewCopy.ShowHelp"))));
            this.toolTip.SetToolTip(this.treeViewCopy, resources.GetString("treeViewCopy.ToolTip"));
            // 
            // groupBoxOldData
            // 
            this.groupBoxOldData.AccessibleDescription = null;
            this.groupBoxOldData.AccessibleName = null;
            resources.ApplyResources(this.groupBoxOldData, "groupBoxOldData");
            this.groupBoxOldData.BackgroundImage = null;
            this.tableLayoutPanelMain.SetColumnSpan(this.groupBoxOldData, 3);
            this.groupBoxOldData.Controls.Add(this.treeViewOriginal);
            this.groupBoxOldData.Font = null;
            this.groupBoxOldData.ForeColor = System.Drawing.Color.Gray;
            this.helpProvider.SetHelpKeyword(this.groupBoxOldData, null);
            this.helpProvider.SetHelpNavigator(this.groupBoxOldData, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("groupBoxOldData.HelpNavigator"))));
            this.helpProvider.SetHelpString(this.groupBoxOldData, null);
            this.groupBoxOldData.Name = "groupBoxOldData";
            this.helpProvider.SetShowHelp(this.groupBoxOldData, ((bool)(resources.GetObject("groupBoxOldData.ShowHelp"))));
            this.groupBoxOldData.TabStop = false;
            this.toolTip.SetToolTip(this.groupBoxOldData, resources.GetString("groupBoxOldData.ToolTip"));
            // 
            // treeViewOriginal
            // 
            this.treeViewOriginal.AccessibleDescription = null;
            this.treeViewOriginal.AccessibleName = null;
            resources.ApplyResources(this.treeViewOriginal, "treeViewOriginal");
            this.treeViewOriginal.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewOriginal.BackgroundImage = null;
            this.treeViewOriginal.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewOriginal.Font = null;
            this.helpProvider.SetHelpKeyword(this.treeViewOriginal, null);
            this.helpProvider.SetHelpNavigator(this.treeViewOriginal, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("treeViewOriginal.HelpNavigator"))));
            this.helpProvider.SetHelpString(this.treeViewOriginal, null);
            this.treeViewOriginal.Name = "treeViewOriginal";
            this.treeViewOriginal.Scrollable = false;
            this.helpProvider.SetShowHelp(this.treeViewOriginal, ((bool)(resources.GetObject("treeViewOriginal.ShowHelp"))));
            this.toolTip.SetToolTip(this.treeViewOriginal, resources.GetString("treeViewOriginal.ToolTip"));
            // 
            // textBoxAccessionNumber
            // 
            this.textBoxAccessionNumber.AccessibleDescription = null;
            this.textBoxAccessionNumber.AccessibleName = null;
            resources.ApplyResources(this.textBoxAccessionNumber, "textBoxAccessionNumber");
            this.textBoxAccessionNumber.BackgroundImage = null;
            this.textBoxAccessionNumber.Font = null;
            this.helpProvider.SetHelpKeyword(this.textBoxAccessionNumber, null);
            this.helpProvider.SetHelpNavigator(this.textBoxAccessionNumber, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("textBoxAccessionNumber.HelpNavigator"))));
            this.helpProvider.SetHelpString(this.textBoxAccessionNumber, null);
            this.textBoxAccessionNumber.Name = "textBoxAccessionNumber";
            this.helpProvider.SetShowHelp(this.textBoxAccessionNumber, ((bool)(resources.GetObject("textBoxAccessionNumber.ShowHelp"))));
            this.toolTip.SetToolTip(this.textBoxAccessionNumber, resources.GetString("textBoxAccessionNumber.ToolTip"));
            this.textBoxAccessionNumber.TextChanged += new System.EventHandler(this.textBoxAccessionNumber_TextChanged);
            // 
            // labelAccessionNumber
            // 
            this.labelAccessionNumber.AccessibleDescription = null;
            this.labelAccessionNumber.AccessibleName = null;
            resources.ApplyResources(this.labelAccessionNumber, "labelAccessionNumber");
            this.tableLayoutPanelMain.SetColumnSpan(this.labelAccessionNumber, 2);
            this.labelAccessionNumber.Font = null;
            this.helpProvider.SetHelpKeyword(this.labelAccessionNumber, null);
            this.helpProvider.SetHelpNavigator(this.labelAccessionNumber, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("labelAccessionNumber.HelpNavigator"))));
            this.helpProvider.SetHelpString(this.labelAccessionNumber, null);
            this.labelAccessionNumber.Name = "labelAccessionNumber";
            this.helpProvider.SetShowHelp(this.labelAccessionNumber, ((bool)(resources.GetObject("labelAccessionNumber.ShowHelp"))));
            this.toolTip.SetToolTip(this.labelAccessionNumber, resources.GetString("labelAccessionNumber.ToolTip"));
            // 
            // buttonFindNextAccessionNumber
            // 
            this.buttonFindNextAccessionNumber.AccessibleDescription = null;
            this.buttonFindNextAccessionNumber.AccessibleName = null;
            resources.ApplyResources(this.buttonFindNextAccessionNumber, "buttonFindNextAccessionNumber");
            this.buttonFindNextAccessionNumber.BackgroundImage = null;
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonFindNextAccessionNumber, 2);
            this.buttonFindNextAccessionNumber.Font = null;
            this.helpProvider.SetHelpKeyword(this.buttonFindNextAccessionNumber, null);
            this.helpProvider.SetHelpNavigator(this.buttonFindNextAccessionNumber, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("buttonFindNextAccessionNumber.HelpNavigator"))));
            this.helpProvider.SetHelpString(this.buttonFindNextAccessionNumber, null);
            this.buttonFindNextAccessionNumber.Name = "buttonFindNextAccessionNumber";
            this.helpProvider.SetShowHelp(this.buttonFindNextAccessionNumber, ((bool)(resources.GetObject("buttonFindNextAccessionNumber.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonFindNextAccessionNumber, resources.GetString("buttonFindNextAccessionNumber.ToolTip"));
            this.buttonFindNextAccessionNumber.UseVisualStyleBackColor = true;
            this.buttonFindNextAccessionNumber.Click += new System.EventHandler(this.buttonFindNextAccessionNumber_Click);
            // 
            // helpProvider
            // 
            this.helpProvider.HelpNamespace = null;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.AccessibleDescription = null;
            this.userControlDialogPanel.AccessibleName = null;
            resources.ApplyResources(this.userControlDialogPanel, "userControlDialogPanel");
            this.userControlDialogPanel.BackgroundImage = null;
            this.userControlDialogPanel.Font = null;
            this.helpProvider.SetHelpKeyword(this.userControlDialogPanel, null);
            this.helpProvider.SetHelpNavigator(this.userControlDialogPanel, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("userControlDialogPanel.HelpNavigator"))));
            this.helpProvider.SetHelpString(this.userControlDialogPanel, null);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.helpProvider.SetShowHelp(this.userControlDialogPanel, ((bool)(resources.GetObject("userControlDialogPanel.ShowHelp"))));
            this.toolTip.SetToolTip(this.userControlDialogPanel, resources.GetString("userControlDialogPanel.ToolTip"));
            // 
            // FormIdentificationUnitGridNewEntry
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Controls.Add(this.userControlDialogPanel);
            this.Font = null;
            this.helpProvider.SetHelpKeyword(this, null);
            this.helpProvider.SetHelpNavigator(this, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("$this.HelpNavigator"))));
            this.helpProvider.SetHelpString(this, null);
            this.Name = "FormIdentificationUnitGridNewEntry";
            this.helpProvider.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.ShowInTaskbar = false;
            this.toolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.groupBoxNewData.ResumeLayout(false);
            this.groupBoxOldData.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Button buttonNewSpecimen;
        private System.Windows.Forms.Button buttonSameSpecimen;
        private System.Windows.Forms.TreeView treeViewSameEvent;
        private System.Windows.Forms.TreeView treeViewNewEvent;
        private System.Windows.Forms.Label labelHeaderEvent;
        private System.Windows.Forms.GroupBox groupBoxNewData;
        private System.Windows.Forms.TreeView treeViewCopy;
        private System.Windows.Forms.GroupBox groupBoxOldData;
        private System.Windows.Forms.TreeView treeViewOriginal;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TextBox textBoxAccessionNumber;
        private System.Windows.Forms.Label labelAccessionNumber;
        private System.Windows.Forms.Button buttonFindNextAccessionNumber;
    }
}