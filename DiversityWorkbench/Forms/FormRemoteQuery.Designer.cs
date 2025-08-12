using DiversityWorkbench.UserControls;

namespace DiversityWorkbench.Forms
{
    partial class FormRemoteQuery
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
            components = new System.ComponentModel.Container();
            Microsoft.Web.WebView2.WinForms.CoreWebView2CreationProperties coreWebView2CreationProperties1 = new Microsoft.Web.WebView2.WinForms.CoreWebView2CreationProperties();
            Microsoft.Web.WebView2.WinForms.CoreWebView2CreationProperties coreWebView2CreationProperties2 = new Microsoft.Web.WebView2.WinForms.CoreWebView2CreationProperties();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRemoteQuery));
            splitContainerMain = new System.Windows.Forms.SplitContainer();
            splitContainerQueries = new System.Windows.Forms.SplitContainer();
            splitContainerQueryLists = new System.Windows.Forms.SplitContainer();
            userControlQueryList = new UserControlQueryList();
            tableLayoutPanelCacheDB = new System.Windows.Forms.TableLayoutPanel();
            labelCacheDBSource = new System.Windows.Forms.Label();
            comboBoxCacheDBSource = new System.Windows.Forms.ComboBox();
            userControlQueryListCacheDB = new UserControlQueryList();
            splitContainerForeignSources = new System.Windows.Forms.SplitContainer();
            userControlWebservice = new UserControlWebservice();
            splitContainerCoL = new System.Windows.Forms.SplitContainer();
            panelDatabase = new System.Windows.Forms.Panel();
            comboBoxDatabase = new System.Windows.Forms.ComboBox();
            panelDatabaseMenuStrip = new System.Windows.Forms.Panel();
            menuStripDatabase = new System.Windows.Forms.MenuStrip();
            toolStripMenuItemGetAll = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemLists = new System.Windows.Forms.ToolStripMenuItem();
            buttonConnectToDatabase = new System.Windows.Forms.Button();
            labelDatabase = new System.Windows.Forms.Label();
            buttonFeedback = new System.Windows.Forms.Button();
            splitContainerUnitCollection = new System.Windows.Forms.SplitContainer();
            splitContainerUnit = new System.Windows.Forms.SplitContainer();
            panelUnit = new System.Windows.Forms.Panel();
            tableLayoutPanelUnit = new System.Windows.Forms.TableLayoutPanel();
            labelUnit = new System.Windows.Forms.Label();
            buttonShowJsonTreeView = new System.Windows.Forms.Button();
            userControlJsonTreeView = new UserControlJsonTreeView();
            treeViewUnit = new System.Windows.Forms.TreeView();
            listViewUnitCollection = new System.Windows.Forms.ListView();
            toolStripUnitCollection = new System.Windows.Forms.ToolStrip();
            toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            toolStripButtonAddUnit = new System.Windows.Forms.ToolStripButton();
            toolStripButtonRemoveUnit = new System.Windows.Forms.ToolStripButton();
            toolStripButtonShowUnit = new System.Windows.Forms.ToolStripButton();
            panelWebControls = new System.Windows.Forms.Panel();
            checkBoxDisplayWebsite = new System.Windows.Forms.CheckBox();
            buttonTimeoutWebrequest = new System.Windows.Forms.Button();
            panelOpenModule = new System.Windows.Forms.Panel();
            labelOpenModule = new System.Windows.Forms.Label();
            buttonOpenModule = new System.Windows.Forms.Button();
            splitContainerURI = new System.Windows.Forms.SplitContainer();
            panelURI = new System.Windows.Forms.Panel();
            splitContainerUriResources = new System.Windows.Forms.SplitContainer();
            userControlWebViewURI = new UserControlWebView();
            tableLayoutPanelResource = new System.Windows.Forms.TableLayoutPanel();
            toolStripResource = new System.Windows.Forms.ToolStrip();
            toolStripLabelResourceDescription = new System.Windows.Forms.ToolStripLabel();
            toolStripButtonResourceNext = new System.Windows.Forms.ToolStripButton();
            toolStripButtonResourceInSeparateForm = new System.Windows.Forms.ToolStripButton();
            linkLabelResource = new System.Windows.Forms.LinkLabel();
            userControlWebViewResource = new UserControlWebView();
            helpProvider = new System.Windows.Forms.HelpProvider();
            toolTip = new System.Windows.Forms.ToolTip(components);
            userControlDialogPanel = new UserControlDialogPanel();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerQueries).BeginInit();
            splitContainerQueries.Panel1.SuspendLayout();
            splitContainerQueries.Panel2.SuspendLayout();
            splitContainerQueries.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerQueryLists).BeginInit();
            splitContainerQueryLists.Panel1.SuspendLayout();
            splitContainerQueryLists.Panel2.SuspendLayout();
            splitContainerQueryLists.SuspendLayout();
            tableLayoutPanelCacheDB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerForeignSources).BeginInit();
            splitContainerForeignSources.Panel1.SuspendLayout();
            splitContainerForeignSources.Panel2.SuspendLayout();
            splitContainerForeignSources.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerCoL).BeginInit();
            splitContainerCoL.SuspendLayout();
            panelDatabase.SuspendLayout();
            panelDatabaseMenuStrip.SuspendLayout();
            menuStripDatabase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerUnitCollection).BeginInit();
            splitContainerUnitCollection.Panel1.SuspendLayout();
            splitContainerUnitCollection.Panel2.SuspendLayout();
            splitContainerUnitCollection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerUnit).BeginInit();
            splitContainerUnit.Panel1.SuspendLayout();
            splitContainerUnit.Panel2.SuspendLayout();
            splitContainerUnit.SuspendLayout();
            tableLayoutPanelUnit.SuspendLayout();
            toolStripUnitCollection.SuspendLayout();
            panelWebControls.SuspendLayout();
            panelOpenModule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerURI).BeginInit();
            splitContainerURI.Panel1.SuspendLayout();
            splitContainerURI.Panel2.SuspendLayout();
            splitContainerURI.SuspendLayout();
            panelURI.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerUriResources).BeginInit();
            splitContainerUriResources.Panel1.SuspendLayout();
            splitContainerUriResources.Panel2.SuspendLayout();
            splitContainerUriResources.SuspendLayout();
            tableLayoutPanelResource.SuspendLayout();
            toolStripResource.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerMain.Location = new System.Drawing.Point(0, 0);
            splitContainerMain.Margin = new System.Windows.Forms.Padding(4);
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(splitContainerQueries);
            splitContainerMain.Panel1.Controls.Add(panelDatabase);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(splitContainerUnitCollection);
            splitContainerMain.Panel2.Controls.Add(panelWebControls);
            splitContainerMain.Panel2.Controls.Add(panelOpenModule);
            splitContainerMain.Size = new System.Drawing.Size(949, 554);
            splitContainerMain.SplitterDistance = 562;
            splitContainerMain.TabIndex = 4;
            // 
            // splitContainerQueries
            // 
            splitContainerQueries.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerQueries.Location = new System.Drawing.Point(0, 28);
            splitContainerQueries.Margin = new System.Windows.Forms.Padding(4);
            splitContainerQueries.Name = "splitContainerQueries";
            // 
            // splitContainerQueries.Panel1
            // 
            splitContainerQueries.Panel1.Controls.Add(splitContainerQueryLists);
            // 
            // splitContainerQueries.Panel2
            // 
            splitContainerQueries.Panel2.Controls.Add(splitContainerForeignSources);
            splitContainerQueries.Size = new System.Drawing.Size(562, 526);
            splitContainerQueries.SplitterDistance = 233;
            splitContainerQueries.TabIndex = 3;
            // 
            // splitContainerQueryLists
            // 
            splitContainerQueryLists.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerQueryLists.Location = new System.Drawing.Point(0, 0);
            splitContainerQueryLists.Margin = new System.Windows.Forms.Padding(4);
            splitContainerQueryLists.Name = "splitContainerQueryLists";
            // 
            // splitContainerQueryLists.Panel1
            // 
            splitContainerQueryLists.Panel1.Controls.Add(userControlQueryList);
            // 
            // splitContainerQueryLists.Panel2
            // 
            splitContainerQueryLists.Panel2.Controls.Add(tableLayoutPanelCacheDB);
            splitContainerQueryLists.Size = new System.Drawing.Size(233, 526);
            splitContainerQueryLists.SplitterDistance = 77;
            splitContainerQueryLists.TabIndex = 1;
            // 
            // userControlQueryList
            // 
            userControlQueryList.BacklinkUpdateEnabled = false;
            userControlQueryList.Connection = null;
            userControlQueryList.DisplayTextSelectedItem = "";
            userControlQueryList.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpNavigator(userControlQueryList, System.Windows.Forms.HelpNavigator.KeywordIndex);
            userControlQueryList.IDisNumeric = true;
            userControlQueryList.ImageList = null;
            userControlQueryList.IsPredefinedQuery = false;
            userControlQueryList.Location = new System.Drawing.Point(0, 0);
            userControlQueryList.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            userControlQueryList.MaximalNumberOfResults = 100;
            userControlQueryList.Name = "userControlQueryList";
            userControlQueryList.Optimizing_UsedForQueryList = false;
            userControlQueryList.ProjectID = -1;
            userControlQueryList.QueryConditionVisiblity = "";
            userControlQueryList.QueryDisplayColumns = null;
            userControlQueryList.QueryMainTableLocal = null;
            userControlQueryList.QueryRestriction = "";
            userControlQueryList.RememberQuerySettingsIdentifier = "QueryList";
            userControlQueryList.SelectedProjectID = null;
            helpProvider.SetShowHelp(userControlQueryList, true);
            userControlQueryList.Size = new System.Drawing.Size(77, 526);
            userControlQueryList.TabIndex = 0;
            userControlQueryList.TableColors = null;
            userControlQueryList.TableImageIndex = null;
            userControlQueryList.WhereClause = null;
            // 
            // tableLayoutPanelCacheDB
            // 
            tableLayoutPanelCacheDB.ColumnCount = 2;
            tableLayoutPanelCacheDB.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelCacheDB.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelCacheDB.Controls.Add(labelCacheDBSource, 0, 0);
            tableLayoutPanelCacheDB.Controls.Add(comboBoxCacheDBSource, 1, 0);
            tableLayoutPanelCacheDB.Controls.Add(userControlQueryListCacheDB, 0, 1);
            tableLayoutPanelCacheDB.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelCacheDB.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelCacheDB.Margin = new System.Windows.Forms.Padding(4);
            tableLayoutPanelCacheDB.Name = "tableLayoutPanelCacheDB";
            tableLayoutPanelCacheDB.RowCount = 2;
            tableLayoutPanelCacheDB.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelCacheDB.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelCacheDB.Size = new System.Drawing.Size(152, 526);
            tableLayoutPanelCacheDB.TabIndex = 0;
            // 
            // labelCacheDBSource
            // 
            labelCacheDBSource.AutoSize = true;
            labelCacheDBSource.Dock = System.Windows.Forms.DockStyle.Fill;
            labelCacheDBSource.Location = new System.Drawing.Point(4, 0);
            labelCacheDBSource.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelCacheDBSource.Name = "labelCacheDBSource";
            labelCacheDBSource.Size = new System.Drawing.Size(46, 31);
            labelCacheDBSource.TabIndex = 0;
            labelCacheDBSource.Text = "Source:";
            labelCacheDBSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxCacheDBSource
            // 
            comboBoxCacheDBSource.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxCacheDBSource.FormattingEnabled = true;
            comboBoxCacheDBSource.Location = new System.Drawing.Point(50, 4);
            comboBoxCacheDBSource.Margin = new System.Windows.Forms.Padding(0, 4, 4, 4);
            comboBoxCacheDBSource.Name = "comboBoxCacheDBSource";
            comboBoxCacheDBSource.Size = new System.Drawing.Size(98, 23);
            comboBoxCacheDBSource.TabIndex = 1;
            comboBoxCacheDBSource.SelectedIndexChanged += comboBoxCacheDBSource_SelectedIndexChanged;
            comboBoxCacheDBSource.SelectionChangeCommitted += comboBoxCacheDBSource_SelectionChangeCommitted;
            // 
            // userControlQueryListCacheDB
            // 
            userControlQueryListCacheDB.BacklinkUpdateEnabled = false;
            tableLayoutPanelCacheDB.SetColumnSpan(userControlQueryListCacheDB, 2);
            userControlQueryListCacheDB.Connection = null;
            userControlQueryListCacheDB.DisplayTextSelectedItem = "";
            userControlQueryListCacheDB.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlQueryListCacheDB.IDisNumeric = true;
            userControlQueryListCacheDB.ImageList = null;
            userControlQueryListCacheDB.IsPredefinedQuery = false;
            userControlQueryListCacheDB.Location = new System.Drawing.Point(6, 35);
            userControlQueryListCacheDB.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            userControlQueryListCacheDB.MaximalNumberOfResults = 100;
            userControlQueryListCacheDB.Name = "userControlQueryListCacheDB";
            userControlQueryListCacheDB.Optimizing_UsedForQueryList = false;
            userControlQueryListCacheDB.ProjectID = -1;
            userControlQueryListCacheDB.QueryConditionVisiblity = "";
            userControlQueryListCacheDB.QueryDisplayColumns = null;
            userControlQueryListCacheDB.QueryMainTableLocal = null;
            userControlQueryListCacheDB.QueryRestriction = "";
            userControlQueryListCacheDB.RememberQuerySettingsIdentifier = "QueryList";
            userControlQueryListCacheDB.SelectedProjectID = null;
            userControlQueryListCacheDB.Size = new System.Drawing.Size(140, 487);
            userControlQueryListCacheDB.TabIndex = 2;
            userControlQueryListCacheDB.TableColors = null;
            userControlQueryListCacheDB.TableImageIndex = null;
            userControlQueryListCacheDB.WhereClause = null;
            // 
            // splitContainerForeignSources
            // 
            splitContainerForeignSources.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerForeignSources.Location = new System.Drawing.Point(0, 0);
            splitContainerForeignSources.Margin = new System.Windows.Forms.Padding(4);
            splitContainerForeignSources.Name = "splitContainerForeignSources";
            // 
            // splitContainerForeignSources.Panel1
            // 
            splitContainerForeignSources.Panel1.Controls.Add(userControlWebservice);
            // 
            // splitContainerForeignSources.Panel2
            // 
            splitContainerForeignSources.Panel2.Controls.Add(splitContainerCoL);
            splitContainerForeignSources.Panel2Collapsed = true;
            splitContainerForeignSources.Size = new System.Drawing.Size(325, 526);
            splitContainerForeignSources.SplitterDistance = 158;
            splitContainerForeignSources.TabIndex = 2;
            // 
            // userControlWebservice
            // 
            userControlWebservice.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(userControlWebservice, "Webservice");
            helpProvider.SetHelpNavigator(userControlWebservice, System.Windows.Forms.HelpNavigator.KeywordIndex);
            userControlWebservice.Location = new System.Drawing.Point(0, 0);
            userControlWebservice.Margin = new System.Windows.Forms.Padding(6);
            userControlWebservice.Name = "userControlWebservice";
            helpProvider.SetShowHelp(userControlWebservice, true);
            userControlWebservice.Size = new System.Drawing.Size(325, 526);
            userControlWebservice.TabIndex = 1;
            // 
            // splitContainerCoL
            // 
            splitContainerCoL.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerCoL.Location = new System.Drawing.Point(0, 0);
            splitContainerCoL.Margin = new System.Windows.Forms.Padding(4);
            splitContainerCoL.Name = "splitContainerCoL";
            splitContainerCoL.Orientation = System.Windows.Forms.Orientation.Horizontal;
            splitContainerCoL.Panel1Collapsed = true;
            splitContainerCoL.Size = new System.Drawing.Size(96, 100);
            splitContainerCoL.TabIndex = 0;
            // 
            // panelDatabase
            // 
            panelDatabase.Controls.Add(comboBoxDatabase);
            panelDatabase.Controls.Add(panelDatabaseMenuStrip);
            panelDatabase.Controls.Add(buttonConnectToDatabase);
            panelDatabase.Controls.Add(labelDatabase);
            panelDatabase.Controls.Add(buttonFeedback);
            panelDatabase.Dock = System.Windows.Forms.DockStyle.Top;
            panelDatabase.Location = new System.Drawing.Point(0, 0);
            panelDatabase.Margin = new System.Windows.Forms.Padding(4);
            panelDatabase.Name = "panelDatabase";
            panelDatabase.Size = new System.Drawing.Size(562, 28);
            panelDatabase.TabIndex = 1;
            // 
            // comboBoxDatabase
            // 
            comboBoxDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxDatabase.FormattingEnabled = true;
            comboBoxDatabase.Location = new System.Drawing.Point(146, 0);
            comboBoxDatabase.Margin = new System.Windows.Forms.Padding(4);
            comboBoxDatabase.Name = "comboBoxDatabase";
            comboBoxDatabase.Size = new System.Drawing.Size(360, 23);
            comboBoxDatabase.TabIndex = 1;
            comboBoxDatabase.SelectedIndexChanged += comboBoxDatabase_SelectedIndexChanged;
            // 
            // panelDatabaseMenuStrip
            // 
            panelDatabaseMenuStrip.Controls.Add(menuStripDatabase);
            panelDatabaseMenuStrip.Dock = System.Windows.Forms.DockStyle.Left;
            panelDatabaseMenuStrip.Location = new System.Drawing.Point(69, 0);
            panelDatabaseMenuStrip.Margin = new System.Windows.Forms.Padding(4);
            panelDatabaseMenuStrip.Name = "panelDatabaseMenuStrip";
            panelDatabaseMenuStrip.Size = new System.Drawing.Size(77, 28);
            panelDatabaseMenuStrip.TabIndex = 4;
            panelDatabaseMenuStrip.Visible = false;
            // 
            // menuStripDatabase
            // 
            menuStripDatabase.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStripDatabase.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItemGetAll, toolStripMenuItemLists });
            menuStripDatabase.Location = new System.Drawing.Point(0, 0);
            menuStripDatabase.Name = "menuStripDatabase";
            menuStripDatabase.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            menuStripDatabase.Size = new System.Drawing.Size(77, 28);
            menuStripDatabase.TabIndex = 0;
            menuStripDatabase.Text = "menuStrip1";
            // 
            // toolStripMenuItemGetAll
            // 
            toolStripMenuItemGetAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripMenuItemGetAll.Image = Properties.Resources.DatabaseAdd;
            toolStripMenuItemGetAll.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripMenuItemGetAll.Name = "toolStripMenuItemGetAll";
            toolStripMenuItemGetAll.Size = new System.Drawing.Size(28, 24);
            toolStripMenuItemGetAll.Text = "Get all";
            toolStripMenuItemGetAll.Click += toolStripMenuItemGetAll_Click;
            // 
            // toolStripMenuItemLists
            // 
            toolStripMenuItemLists.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripMenuItemLists.Image = ResourceWorkbench.DatabaseList;
            toolStripMenuItemLists.Name = "toolStripMenuItemLists";
            toolStripMenuItemLists.Size = new System.Drawing.Size(32, 24);
            toolStripMenuItemLists.Text = "Lists";
            // 
            // buttonConnectToDatabase
            // 
            buttonConnectToDatabase.Dock = System.Windows.Forms.DockStyle.Right;
            buttonConnectToDatabase.Image = ResourceWorkbench.Database;
            buttonConnectToDatabase.Location = new System.Drawing.Point(506, 0);
            buttonConnectToDatabase.Margin = new System.Windows.Forms.Padding(4);
            buttonConnectToDatabase.Name = "buttonConnectToDatabase";
            buttonConnectToDatabase.Size = new System.Drawing.Size(28, 28);
            buttonConnectToDatabase.TabIndex = 2;
            toolTip.SetToolTip(buttonConnectToDatabase, "Enter the login data for the selected source");
            buttonConnectToDatabase.UseVisualStyleBackColor = true;
            buttonConnectToDatabase.Visible = false;
            buttonConnectToDatabase.Click += buttonConnectToDatabase_Click;
            // 
            // labelDatabase
            // 
            labelDatabase.Dock = System.Windows.Forms.DockStyle.Left;
            labelDatabase.Location = new System.Drawing.Point(0, 0);
            labelDatabase.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            labelDatabase.Name = "labelDatabase";
            labelDatabase.Size = new System.Drawing.Size(69, 28);
            labelDatabase.TabIndex = 0;
            labelDatabase.Text = "Database:";
            labelDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonFeedback
            // 
            buttonFeedback.Dock = System.Windows.Forms.DockStyle.Right;
            buttonFeedback.Image = Properties.Resources.Feedback;
            buttonFeedback.Location = new System.Drawing.Point(534, 0);
            buttonFeedback.Margin = new System.Windows.Forms.Padding(4);
            buttonFeedback.Name = "buttonFeedback";
            buttonFeedback.Size = new System.Drawing.Size(28, 28);
            buttonFeedback.TabIndex = 3;
            toolTip.SetToolTip(buttonFeedback, "Send a feedback concering the current window or action");
            buttonFeedback.UseVisualStyleBackColor = true;
            buttonFeedback.Click += buttonFeedback_Click;
            // 
            // splitContainerUnitCollection
            // 
            splitContainerUnitCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerUnitCollection.Location = new System.Drawing.Point(0, 28);
            splitContainerUnitCollection.Margin = new System.Windows.Forms.Padding(4);
            splitContainerUnitCollection.Name = "splitContainerUnitCollection";
            splitContainerUnitCollection.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerUnitCollection.Panel1
            // 
            splitContainerUnitCollection.Panel1.Controls.Add(splitContainerUnit);
            // 
            // splitContainerUnitCollection.Panel2
            // 
            splitContainerUnitCollection.Panel2.Controls.Add(listViewUnitCollection);
            splitContainerUnitCollection.Panel2.Controls.Add(toolStripUnitCollection);
            splitContainerUnitCollection.Size = new System.Drawing.Size(383, 498);
            splitContainerUnitCollection.SplitterDistance = 344;
            splitContainerUnitCollection.TabIndex = 8;
            // 
            // splitContainerUnit
            // 
            splitContainerUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerUnit.Location = new System.Drawing.Point(0, 0);
            splitContainerUnit.Margin = new System.Windows.Forms.Padding(4);
            splitContainerUnit.Name = "splitContainerUnit";
            splitContainerUnit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerUnit.Panel1
            // 
            splitContainerUnit.Panel1.Controls.Add(panelUnit);
            splitContainerUnit.Panel1.Controls.Add(tableLayoutPanelUnit);
            // 
            // splitContainerUnit.Panel2
            // 
            splitContainerUnit.Panel2.Controls.Add(buttonShowJsonTreeView);
            splitContainerUnit.Panel2.Controls.Add(userControlJsonTreeView);
            splitContainerUnit.Panel2.Controls.Add(treeViewUnit);
            splitContainerUnit.Size = new System.Drawing.Size(383, 344);
            splitContainerUnit.SplitterDistance = 151;
            splitContainerUnit.TabIndex = 7;
            // 
            // panelUnit
            // 
            panelUnit.AutoScroll = true;
            panelUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            panelUnit.Location = new System.Drawing.Point(0, 23);
            panelUnit.Margin = new System.Windows.Forms.Padding(4);
            panelUnit.Name = "panelUnit";
            panelUnit.Size = new System.Drawing.Size(383, 128);
            panelUnit.TabIndex = 6;
            // 
            // tableLayoutPanelUnit
            // 
            tableLayoutPanelUnit.ColumnCount = 1;
            tableLayoutPanelUnit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelUnit.Controls.Add(labelUnit, 0, 0);
            tableLayoutPanelUnit.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPanelUnit.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelUnit.Margin = new System.Windows.Forms.Padding(4);
            tableLayoutPanelUnit.Name = "tableLayoutPanelUnit";
            tableLayoutPanelUnit.RowCount = 1;
            tableLayoutPanelUnit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelUnit.Size = new System.Drawing.Size(383, 23);
            tableLayoutPanelUnit.TabIndex = 5;
            tableLayoutPanelUnit.Visible = false;
            // 
            // labelUnit
            // 
            labelUnit.AutoSize = true;
            labelUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            labelUnit.Location = new System.Drawing.Point(4, 0);
            labelUnit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelUnit.Name = "labelUnit";
            labelUnit.Size = new System.Drawing.Size(375, 23);
            labelUnit.TabIndex = 0;
            labelUnit.Text = "Unit";
            labelUnit.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // buttonShowJsonTreeView
            // 
            buttonShowJsonTreeView.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonShowJsonTreeView.Location = new System.Drawing.Point(0, 166);
            buttonShowJsonTreeView.Name = "buttonShowJsonTreeView";
            buttonShowJsonTreeView.Size = new System.Drawing.Size(383, 23);
            buttonShowJsonTreeView.TabIndex = 8;
            buttonShowJsonTreeView.Text = "Show whole Information as JSON tree";
            buttonShowJsonTreeView.UseVisualStyleBackColor = true;
            buttonShowJsonTreeView.Visible = false;
            buttonShowJsonTreeView.Click += buttonShowJsonTreeView_Click;
            // 
            // userControlJsonTreeView
            // 
            userControlJsonTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlJsonTreeView.Location = new System.Drawing.Point(0, 0);
            userControlJsonTreeView.Name = "userControlJsonTreeView";
            userControlJsonTreeView.Size = new System.Drawing.Size(383, 189);
            userControlJsonTreeView.TabIndex = 7;
            userControlJsonTreeView.Visible = false;
            // 
            // treeViewUnit
            // 
            treeViewUnit.BackColor = System.Drawing.SystemColors.Control;
            treeViewUnit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            treeViewUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            treeViewUnit.Location = new System.Drawing.Point(0, 0);
            treeViewUnit.Margin = new System.Windows.Forms.Padding(4);
            treeViewUnit.Name = "treeViewUnit";
            treeViewUnit.Size = new System.Drawing.Size(383, 189);
            treeViewUnit.TabIndex = 0;
            treeViewUnit.AfterSelect += treeViewUnit_AfterSelect;
            // 
            // listViewUnitCollection
            // 
            listViewUnitCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewUnitCollection.Location = new System.Drawing.Point(0, 27);
            listViewUnitCollection.Margin = new System.Windows.Forms.Padding(4);
            listViewUnitCollection.MultiSelect = false;
            listViewUnitCollection.Name = "listViewUnitCollection";
            listViewUnitCollection.ShowItemToolTips = true;
            listViewUnitCollection.Size = new System.Drawing.Size(383, 123);
            listViewUnitCollection.TabIndex = 2;
            listViewUnitCollection.UseCompatibleStateImageBehavior = false;
            listViewUnitCollection.View = System.Windows.Forms.View.List;
            listViewUnitCollection.SelectedIndexChanged += listViewUnitCollection_SelectedIndexChanged;
            listViewUnitCollection.DoubleClick += listViewUnitCollection_DoubleClick;
            // 
            // toolStripUnitCollection
            // 
            toolStripUnitCollection.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripUnitCollection.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStripUnitCollection.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripLabel1, toolStripButtonAddUnit, toolStripButtonRemoveUnit, toolStripButtonShowUnit });
            toolStripUnitCollection.Location = new System.Drawing.Point(0, 0);
            toolStripUnitCollection.Name = "toolStripUnitCollection";
            toolStripUnitCollection.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            toolStripUnitCollection.Size = new System.Drawing.Size(383, 27);
            toolStripUnitCollection.TabIndex = 1;
            toolStripUnitCollection.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new System.Drawing.Size(50, 24);
            toolStripLabel1.Text = "Unit list:";
            // 
            // toolStripButtonAddUnit
            // 
            toolStripButtonAddUnit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonAddUnit.Image = Properties.Resources.Add;
            toolStripButtonAddUnit.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonAddUnit.Name = "toolStripButtonAddUnit";
            toolStripButtonAddUnit.Size = new System.Drawing.Size(24, 24);
            toolStripButtonAddUnit.Text = "Add unit to list";
            toolStripButtonAddUnit.Click += toolStripButtonAddUnit_Click;
            // 
            // toolStripButtonRemoveUnit
            // 
            toolStripButtonRemoveUnit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonRemoveUnit.Image = Properties.Resources.Minus;
            toolStripButtonRemoveUnit.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonRemoveUnit.Name = "toolStripButtonRemoveUnit";
            toolStripButtonRemoveUnit.Size = new System.Drawing.Size(24, 24);
            toolStripButtonRemoveUnit.Text = "Remove selected unit from list";
            toolStripButtonRemoveUnit.Visible = false;
            toolStripButtonRemoveUnit.Click += toolStripButtonRemoveUnit_Click;
            // 
            // toolStripButtonShowUnit
            // 
            toolStripButtonShowUnit.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripButtonShowUnit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonShowUnit.Image = Properties.Resources.NewForm;
            toolStripButtonShowUnit.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonShowUnit.Name = "toolStripButtonShowUnit";
            toolStripButtonShowUnit.Size = new System.Drawing.Size(24, 24);
            toolStripButtonShowUnit.Text = "Show selected unit in separate window";
            toolStripButtonShowUnit.Visible = false;
            toolStripButtonShowUnit.Click += listViewUnitCollection_DoubleClick;
            // 
            // panelWebControls
            // 
            panelWebControls.Controls.Add(checkBoxDisplayWebsite);
            panelWebControls.Controls.Add(buttonTimeoutWebrequest);
            panelWebControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelWebControls.Location = new System.Drawing.Point(0, 526);
            panelWebControls.Margin = new System.Windows.Forms.Padding(4);
            panelWebControls.Name = "panelWebControls";
            panelWebControls.Size = new System.Drawing.Size(383, 28);
            panelWebControls.TabIndex = 1;
            // 
            // checkBoxDisplayWebsite
            // 
            checkBoxDisplayWebsite.AutoSize = true;
            checkBoxDisplayWebsite.Checked = true;
            checkBoxDisplayWebsite.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxDisplayWebsite.Dock = System.Windows.Forms.DockStyle.Left;
            checkBoxDisplayWebsite.Location = new System.Drawing.Point(0, 0);
            checkBoxDisplayWebsite.Margin = new System.Windows.Forms.Padding(4);
            checkBoxDisplayWebsite.Name = "checkBoxDisplayWebsite";
            checkBoxDisplayWebsite.Size = new System.Drawing.Size(107, 28);
            checkBoxDisplayWebsite.TabIndex = 5;
            checkBoxDisplayWebsite.Text = "Display website";
            checkBoxDisplayWebsite.UseVisualStyleBackColor = true;
            checkBoxDisplayWebsite.CheckedChanged += checkBoxDisplayWebsite_CheckedChanged;
            // 
            // buttonTimeoutWebrequest
            // 
            buttonTimeoutWebrequest.Dock = System.Windows.Forms.DockStyle.Right;
            buttonTimeoutWebrequest.Image = Properties.Resources.Time;
            buttonTimeoutWebrequest.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            buttonTimeoutWebrequest.Location = new System.Drawing.Point(303, 0);
            buttonTimeoutWebrequest.Margin = new System.Windows.Forms.Padding(4);
            buttonTimeoutWebrequest.Name = "buttonTimeoutWebrequest";
            buttonTimeoutWebrequest.Size = new System.Drawing.Size(80, 28);
            buttonTimeoutWebrequest.TabIndex = 6;
            buttonTimeoutWebrequest.Text = "Timeout";
            buttonTimeoutWebrequest.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            toolTip.SetToolTip(buttonTimeoutWebrequest, "Set the timeout for the response of websites");
            buttonTimeoutWebrequest.UseVisualStyleBackColor = true;
            buttonTimeoutWebrequest.Visible = false;
            buttonTimeoutWebrequest.Click += buttonTimeoutWebrequest_Click;
            // 
            // panelOpenModule
            // 
            panelOpenModule.Controls.Add(labelOpenModule);
            panelOpenModule.Controls.Add(buttonOpenModule);
            panelOpenModule.Dock = System.Windows.Forms.DockStyle.Top;
            panelOpenModule.Location = new System.Drawing.Point(0, 0);
            panelOpenModule.Margin = new System.Windows.Forms.Padding(4);
            panelOpenModule.Name = "panelOpenModule";
            panelOpenModule.Size = new System.Drawing.Size(383, 28);
            panelOpenModule.TabIndex = 4;
            // 
            // labelOpenModule
            // 
            labelOpenModule.Dock = System.Windows.Forms.DockStyle.Fill;
            labelOpenModule.Location = new System.Drawing.Point(0, 0);
            labelOpenModule.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelOpenModule.Name = "labelOpenModule";
            labelOpenModule.Size = new System.Drawing.Size(352, 28);
            labelOpenModule.TabIndex = 4;
            labelOpenModule.Text = "Open the Module";
            labelOpenModule.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonOpenModule
            // 
            buttonOpenModule.BackColor = System.Drawing.SystemColors.Control;
            buttonOpenModule.Dock = System.Windows.Forms.DockStyle.Right;
            buttonOpenModule.Image = Properties.Resources.DiversityWorkbench16;
            buttonOpenModule.Location = new System.Drawing.Point(352, 0);
            buttonOpenModule.Margin = new System.Windows.Forms.Padding(4);
            buttonOpenModule.Name = "buttonOpenModule";
            buttonOpenModule.Size = new System.Drawing.Size(31, 28);
            buttonOpenModule.TabIndex = 3;
            buttonOpenModule.UseVisualStyleBackColor = false;
            buttonOpenModule.Click += buttonOpenModule_Click;
            // 
            // splitContainerURI
            // 
            splitContainerURI.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerURI.Location = new System.Drawing.Point(0, 0);
            splitContainerURI.Margin = new System.Windows.Forms.Padding(4);
            splitContainerURI.Name = "splitContainerURI";
            splitContainerURI.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerURI.Panel1
            // 
            splitContainerURI.Panel1.Controls.Add(splitContainerMain);
            // 
            // splitContainerURI.Panel2
            // 
            splitContainerURI.Panel2.Controls.Add(panelURI);
            splitContainerURI.Size = new System.Drawing.Size(949, 804);
            splitContainerURI.SplitterDistance = 554;
            splitContainerURI.TabIndex = 5;
            // 
            // panelURI
            // 
            panelURI.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panelURI.Controls.Add(splitContainerUriResources);
            panelURI.Dock = System.Windows.Forms.DockStyle.Fill;
            panelURI.Location = new System.Drawing.Point(0, 0);
            panelURI.Margin = new System.Windows.Forms.Padding(4);
            panelURI.Name = "panelURI";
            panelURI.Size = new System.Drawing.Size(949, 246);
            panelURI.TabIndex = 0;
            // 
            // splitContainerUriResources
            // 
            splitContainerUriResources.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerUriResources.Location = new System.Drawing.Point(0, 0);
            splitContainerUriResources.Margin = new System.Windows.Forms.Padding(4);
            splitContainerUriResources.Name = "splitContainerUriResources";
            // 
            // splitContainerUriResources.Panel1
            // 
            splitContainerUriResources.Panel1.Controls.Add(userControlWebViewURI);
            // 
            // splitContainerUriResources.Panel2
            // 
            splitContainerUriResources.Panel2.Controls.Add(tableLayoutPanelResource);
            splitContainerUriResources.Size = new System.Drawing.Size(945, 242);
            splitContainerUriResources.SplitterDistance = 314;
            splitContainerUriResources.TabIndex = 1;
            // 
            // userControlWebViewURI
            // 
            userControlWebViewURI.AllowScripting = true;
            coreWebView2CreationProperties1.AdditionalBrowserArguments = null;
            coreWebView2CreationProperties1.BrowserExecutableFolder = null;
            coreWebView2CreationProperties1.IsInPrivateModeEnabled = null;
            coreWebView2CreationProperties1.Language = null;
            coreWebView2CreationProperties1.ProfileName = null;
            coreWebView2CreationProperties1.UserDataFolder = "C:\\Users\\mweiss\\AppData\\Local\\DiversityWorkbench\\NETwebView";
            userControlWebViewURI.CreationProperties = coreWebView2CreationProperties1;
            userControlWebViewURI.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlWebViewURI.Location = new System.Drawing.Point(0, 0);
            userControlWebViewURI.Margin = new System.Windows.Forms.Padding(0);
            userControlWebViewURI.Name = "userControlWebViewURI";
            userControlWebViewURI.ScriptErrorsSuppressed = false;
            userControlWebViewURI.Size = new System.Drawing.Size(314, 242);
            userControlWebViewURI.TabIndex = 0;
            userControlWebViewURI.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            userControlWebViewURI.SizeChanged += userControlWebViewURI_SizeChanged;
            // 
            // tableLayoutPanelResource
            // 
            tableLayoutPanelResource.ColumnCount = 1;
            tableLayoutPanelResource.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelResource.Controls.Add(toolStripResource, 0, 1);
            tableLayoutPanelResource.Controls.Add(linkLabelResource, 0, 2);
            tableLayoutPanelResource.Controls.Add(userControlWebViewResource, 0, 0);
            tableLayoutPanelResource.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelResource.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelResource.Margin = new System.Windows.Forms.Padding(4);
            tableLayoutPanelResource.Name = "tableLayoutPanelResource";
            tableLayoutPanelResource.RowCount = 3;
            tableLayoutPanelResource.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelResource.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelResource.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelResource.Size = new System.Drawing.Size(627, 242);
            tableLayoutPanelResource.TabIndex = 0;
            // 
            // toolStripResource
            // 
            toolStripResource.Dock = System.Windows.Forms.DockStyle.Bottom;
            toolStripResource.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripResource.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStripResource.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripLabelResourceDescription, toolStripButtonResourceNext, toolStripButtonResourceInSeparateForm });
            toolStripResource.Location = new System.Drawing.Point(0, 202);
            toolStripResource.Name = "toolStripResource";
            toolStripResource.Size = new System.Drawing.Size(627, 27);
            toolStripResource.TabIndex = 0;
            toolStripResource.Text = "toolStrip1";
            // 
            // toolStripLabelResourceDescription
            // 
            toolStripLabelResourceDescription.Name = "toolStripLabelResourceDescription";
            toolStripLabelResourceDescription.Size = new System.Drawing.Size(117, 24);
            toolStripLabelResourceDescription.Text = "Resource description";
            // 
            // toolStripButtonResourceNext
            // 
            toolStripButtonResourceNext.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripButtonResourceNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonResourceNext.Image = Properties.Resources.ArrowNext;
            toolStripButtonResourceNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonResourceNext.Name = "toolStripButtonResourceNext";
            toolStripButtonResourceNext.Size = new System.Drawing.Size(24, 24);
            toolStripButtonResourceNext.Text = "Show next resource";
            toolStripButtonResourceNext.Click += toolStripButtonResourceNext_Click;
            // 
            // toolStripButtonResourceInSeparateForm
            // 
            toolStripButtonResourceInSeparateForm.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripButtonResourceInSeparateForm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonResourceInSeparateForm.Image = Properties.Resources.NewForm;
            toolStripButtonResourceInSeparateForm.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonResourceInSeparateForm.Name = "toolStripButtonResourceInSeparateForm";
            toolStripButtonResourceInSeparateForm.Size = new System.Drawing.Size(24, 24);
            toolStripButtonResourceInSeparateForm.Text = "Show resource in separate window";
            toolStripButtonResourceInSeparateForm.Click += toolStripButtonResourceInSeparateForm_Click;
            // 
            // linkLabelResource
            // 
            linkLabelResource.AutoSize = true;
            linkLabelResource.Dock = System.Windows.Forms.DockStyle.Fill;
            linkLabelResource.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            linkLabelResource.Location = new System.Drawing.Point(4, 229);
            linkLabelResource.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            linkLabelResource.Name = "linkLabelResource";
            linkLabelResource.Size = new System.Drawing.Size(619, 13);
            linkLabelResource.TabIndex = 2;
            linkLabelResource.TabStop = true;
            linkLabelResource.Text = "http://...";
            toolTip.SetToolTip(linkLabelResource, "Link of the resource");
            linkLabelResource.LinkClicked += linkLabelResource_LinkClicked;
            // 
            // userControlWebViewResource
            // 
            userControlWebViewResource.AllowScripting = false;
            coreWebView2CreationProperties2.AdditionalBrowserArguments = null;
            coreWebView2CreationProperties2.BrowserExecutableFolder = null;
            coreWebView2CreationProperties2.IsInPrivateModeEnabled = null;
            coreWebView2CreationProperties2.Language = null;
            coreWebView2CreationProperties2.ProfileName = null;
            coreWebView2CreationProperties2.UserDataFolder = "C:\\Users\\mweiss\\AppData\\Local\\DiversityWorkbench\\NETwebView";
            userControlWebViewResource.CreationProperties = coreWebView2CreationProperties2;
            userControlWebViewResource.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlWebViewResource.Location = new System.Drawing.Point(0, 0);
            userControlWebViewResource.Margin = new System.Windows.Forms.Padding(0);
            userControlWebViewResource.Name = "userControlWebViewResource";
            userControlWebViewResource.ScriptErrorsSuppressed = false;
            userControlWebViewResource.Size = new System.Drawing.Size(627, 202);
            userControlWebViewResource.TabIndex = 3;
            userControlWebViewResource.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            // 
            // userControlDialogPanel
            // 
            userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            userControlDialogPanel.Location = new System.Drawing.Point(0, 804);
            userControlDialogPanel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            userControlDialogPanel.Name = "userControlDialogPanel";
            userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            userControlDialogPanel.Size = new System.Drawing.Size(949, 32);
            userControlDialogPanel.TabIndex = 1;
            // 
            // FormRemoteQuery
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(949, 836);
            Controls.Add(splitContainerURI);
            Controls.Add(userControlDialogPanel);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4);
            Name = "FormRemoteQuery";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = " Remote query";
            FormClosing += FormRemoteQuery_FormClosing;
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            splitContainerQueries.Panel1.ResumeLayout(false);
            splitContainerQueries.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerQueries).EndInit();
            splitContainerQueries.ResumeLayout(false);
            splitContainerQueryLists.Panel1.ResumeLayout(false);
            splitContainerQueryLists.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerQueryLists).EndInit();
            splitContainerQueryLists.ResumeLayout(false);
            tableLayoutPanelCacheDB.ResumeLayout(false);
            tableLayoutPanelCacheDB.PerformLayout();
            splitContainerForeignSources.Panel1.ResumeLayout(false);
            splitContainerForeignSources.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerForeignSources).EndInit();
            splitContainerForeignSources.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerCoL).EndInit();
            splitContainerCoL.ResumeLayout(false);
            panelDatabase.ResumeLayout(false);
            panelDatabaseMenuStrip.ResumeLayout(false);
            panelDatabaseMenuStrip.PerformLayout();
            menuStripDatabase.ResumeLayout(false);
            menuStripDatabase.PerformLayout();
            splitContainerUnitCollection.Panel1.ResumeLayout(false);
            splitContainerUnitCollection.Panel2.ResumeLayout(false);
            splitContainerUnitCollection.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerUnitCollection).EndInit();
            splitContainerUnitCollection.ResumeLayout(false);
            splitContainerUnit.Panel1.ResumeLayout(false);
            splitContainerUnit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerUnit).EndInit();
            splitContainerUnit.ResumeLayout(false);
            tableLayoutPanelUnit.ResumeLayout(false);
            tableLayoutPanelUnit.PerformLayout();
            toolStripUnitCollection.ResumeLayout(false);
            toolStripUnitCollection.PerformLayout();
            panelWebControls.ResumeLayout(false);
            panelWebControls.PerformLayout();
            panelOpenModule.ResumeLayout(false);
            splitContainerURI.Panel1.ResumeLayout(false);
            splitContainerURI.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerURI).EndInit();
            splitContainerURI.ResumeLayout(false);
            panelURI.ResumeLayout(false);
            splitContainerUriResources.Panel1.ResumeLayout(false);
            splitContainerUriResources.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerUriResources).EndInit();
            splitContainerUriResources.ResumeLayout(false);
            tableLayoutPanelResource.ResumeLayout(false);
            tableLayoutPanelResource.PerformLayout();
            toolStripResource.ResumeLayout(false);
            toolStripResource.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private UserControls.UserControlQueryList userControlQueryList;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Button buttonOpenModule;
        private System.Windows.Forms.Panel panelOpenModule;
        private System.Windows.Forms.Label labelOpenModule;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelUnit;
        private System.Windows.Forms.Label labelUnit;
        private System.Windows.Forms.Panel panelDatabase;
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.ComboBox comboBoxDatabase;
        private System.Windows.Forms.Panel panelUnit;
        private DiversityWorkbench.UserControls.UserControlWebservice userControlWebservice;
        private System.Windows.Forms.SplitContainer splitContainerQueries;
        private System.Windows.Forms.SplitContainer splitContainerURI;
        private System.Windows.Forms.Panel panelURI;
        private System.Windows.Forms.SplitContainer splitContainerForeignSources;
        private System.Windows.Forms.SplitContainer splitContainerCoL;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.SplitContainer splitContainerUnit;
        private System.Windows.Forms.TreeView treeViewUnit;
        private System.Windows.Forms.CheckBox checkBoxDisplayWebsite;
        private System.Windows.Forms.Button buttonConnectToDatabase;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel panelWebControls;
        private System.Windows.Forms.Button buttonTimeoutWebrequest;
        private System.Windows.Forms.SplitContainer splitContainerUriResources;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelResource;
        private System.Windows.Forms.ToolStrip toolStripResource;
        private System.Windows.Forms.ToolStripLabel toolStripLabelResourceDescription;
        private System.Windows.Forms.ToolStripButton toolStripButtonResourceNext;
        private System.Windows.Forms.ToolStripButton toolStripButtonResourceInSeparateForm;
        private System.Windows.Forms.LinkLabel linkLabelResource;
        private System.Windows.Forms.SplitContainer splitContainerUnitCollection;
        private System.Windows.Forms.ToolStrip toolStripUnitCollection;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddUnit;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemoveUnit;
        private System.Windows.Forms.ListView listViewUnitCollection;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton toolStripButtonShowUnit;
        private System.Windows.Forms.SplitContainer splitContainerQueryLists;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCacheDB;
        private System.Windows.Forms.Label labelCacheDBSource;
        private System.Windows.Forms.ComboBox comboBoxCacheDBSource;
        private UserControls.UserControlQueryList userControlQueryListCacheDB;
        private System.Windows.Forms.Panel panelDatabaseMenuStrip;
        private System.Windows.Forms.MenuStrip menuStripDatabase;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemLists;
        private UserControls.UserControlWebView userControlWebViewURI;
        private UserControls.UserControlWebView userControlWebViewResource;
        private UserControlJsonTreeView userControlJsonTreeView;
        private System.Windows.Forms.Button buttonShowJsonTreeView;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGetAll;
    }
}