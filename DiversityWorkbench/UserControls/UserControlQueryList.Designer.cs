namespace DiversityWorkbench.UserControls
{
    partial class UserControlQueryList
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlQueryList));
            splitContainerMain = new System.Windows.Forms.SplitContainer();
            groupBoxQueryResults = new System.Windows.Forms.GroupBox();
            listBoxQueryResult = new System.Windows.Forms.ListBox();
            tableLayoutPanelQueryButtons = new System.Windows.Forms.TableLayoutPanel();
            buttonQueryKeep = new System.Windows.Forms.Button();
            buttonQueryPrevious = new System.Windows.Forms.Button();
            buttonQueryRemove = new System.Windows.Forms.Button();
            buttonQueryAdd = new System.Windows.Forms.Button();
            buttonQuery = new System.Windows.Forms.Button();
            contextMenuStripShowSQL = new System.Windows.Forms.ContextMenuStrip(components);
            showSQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            buttonSetQueryConditionsUpDown = new System.Windows.Forms.Button();
            labelQueryColumn = new System.Windows.Forms.Label();
            buttonShowQueryConditions = new System.Windows.Forms.Button();
            buttonQueryNext = new System.Windows.Forms.Button();
            buttonQueryClear = new System.Windows.Forms.Button();
            buttonQueryLoad = new System.Windows.Forms.Button();
            buttonQuerySave = new System.Windows.Forms.Button();
            buttonSelectAllItems = new System.Windows.Forms.Button();
            buttonFreeText = new System.Windows.Forms.Button();
            tableLayoutPanelOrderBy = new System.Windows.Forms.TableLayoutPanel();
            buttonOptimize = new System.Windows.Forms.Button();
            contextMenuStripOptimizing = new System.Windows.Forms.ContextMenuStrip(components);
            toolStripMenuItemOptimizingAsDefault = new System.Windows.Forms.ToolStripMenuItem();
            buttonSwitchDescAsc = new System.Windows.Forms.Button();
            buttonQueryRemember = new System.Windows.Forms.Button();
            contextMenuStripRemeber = new System.Windows.Forms.ContextMenuStrip(components);
            saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemSetAsDefault = new System.Windows.Forms.ToolStripMenuItem();
            comboBoxQueryColumn = new System.Windows.Forms.ComboBox();
            buttonOrderByColumnAdd = new System.Windows.Forms.Button();
            maskedTextBoxOrderByColumnWidth = new System.Windows.Forms.MaskedTextBox();
            pictureBoxOrderByColumnWidth = new System.Windows.Forms.PictureBox();
            panelOrderByColumns = new System.Windows.Forms.Panel();
            groupBoxQueryConditions = new System.Windows.Forms.GroupBox();
            panelQueryConditions = new System.Windows.Forms.Panel();
            textBoxFreeText = new System.Windows.Forms.TextBox();
            textBoxSQL = new System.Windows.Forms.TextBox();
            toolStripQueryList = new System.Windows.Forms.ToolStrip();
            toolStripButtonConnection = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            toolStripButtonBacklinkUpdate = new System.Windows.Forms.ToolStripButton();
            toolStripButtonUndo = new System.Windows.Forms.ToolStripButton();
            toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
            toolStripButtonCopy = new System.Windows.Forms.ToolStripButton();
            toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButtonOptions = new System.Windows.Forms.ToolStripButton();
            toolStripButtonSwitchOrientation = new System.Windows.Forms.ToolStripButton();
            toolStripButtonApplicationID = new System.Windows.Forms.ToolStripButton();
            toolTipQueryList = new System.Windows.Forms.ToolTip(components);
            imageListConnectionState = new System.Windows.Forms.ImageList(components);
            imageListOrientation = new System.Windows.Forms.ImageList(components);
            imageListAscDesc = new System.Windows.Forms.ImageList(components);
            contextMenuStripOrderBy = new System.Windows.Forms.ContextMenuStrip(components);
            addPrefixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            removePrefixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            contextMenuStripBacklink = new System.Windows.Forms.ContextMenuStrip(components);
            insertCurrentIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            insertAllIDsInBacklinkListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            showCurrentListOfIDsForBacklinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            resetBacklinkListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            groupBoxQueryResults.SuspendLayout();
            tableLayoutPanelQueryButtons.SuspendLayout();
            contextMenuStripShowSQL.SuspendLayout();
            tableLayoutPanelOrderBy.SuspendLayout();
            contextMenuStripOptimizing.SuspendLayout();
            contextMenuStripRemeber.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxOrderByColumnWidth).BeginInit();
            groupBoxQueryConditions.SuspendLayout();
            toolStripQueryList.SuspendLayout();
            contextMenuStripOrderBy.SuspendLayout();
            contextMenuStripBacklink.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainerMain
            // 
            resources.ApplyResources(splitContainerMain, "splitContainerMain");
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(groupBoxQueryResults);
            resources.ApplyResources(splitContainerMain.Panel1, "splitContainerMain.Panel1");
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(groupBoxQueryConditions);
            resources.ApplyResources(splitContainerMain.Panel2, "splitContainerMain.Panel2");
            // 
            // groupBoxQueryResults
            // 
            groupBoxQueryResults.Controls.Add(listBoxQueryResult);
            groupBoxQueryResults.Controls.Add(tableLayoutPanelQueryButtons);
            resources.ApplyResources(groupBoxQueryResults, "groupBoxQueryResults");
            groupBoxQueryResults.Name = "groupBoxQueryResults";
            groupBoxQueryResults.TabStop = false;
            // 
            // listBoxQueryResult
            // 
            listBoxQueryResult.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(listBoxQueryResult, "listBoxQueryResult");
            listBoxQueryResult.FormattingEnabled = true;
            listBoxQueryResult.Name = "listBoxQueryResult";
            listBoxQueryResult.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            // 
            // tableLayoutPanelQueryButtons
            // 
            resources.ApplyResources(tableLayoutPanelQueryButtons, "tableLayoutPanelQueryButtons");
            tableLayoutPanelQueryButtons.Controls.Add(buttonQueryKeep, 12, 2);
            tableLayoutPanelQueryButtons.Controls.Add(buttonQueryPrevious, 4, 2);
            tableLayoutPanelQueryButtons.Controls.Add(buttonQueryRemove, 12, 0);
            tableLayoutPanelQueryButtons.Controls.Add(buttonQueryAdd, 7, 2);
            tableLayoutPanelQueryButtons.Controls.Add(buttonQuery, 6, 2);
            tableLayoutPanelQueryButtons.Controls.Add(buttonSetQueryConditionsUpDown, 0, 2);
            tableLayoutPanelQueryButtons.Controls.Add(labelQueryColumn, 0, 0);
            tableLayoutPanelQueryButtons.Controls.Add(buttonShowQueryConditions, 1, 2);
            tableLayoutPanelQueryButtons.Controls.Add(buttonQueryNext, 5, 2);
            tableLayoutPanelQueryButtons.Controls.Add(buttonQueryClear, 8, 2);
            tableLayoutPanelQueryButtons.Controls.Add(buttonQueryLoad, 9, 2);
            tableLayoutPanelQueryButtons.Controls.Add(buttonQuerySave, 10, 2);
            tableLayoutPanelQueryButtons.Controls.Add(buttonSelectAllItems, 11, 2);
            tableLayoutPanelQueryButtons.Controls.Add(buttonFreeText, 3, 2);
            tableLayoutPanelQueryButtons.Controls.Add(tableLayoutPanelOrderBy, 3, 0);
            tableLayoutPanelQueryButtons.Controls.Add(panelOrderByColumns, 0, 1);
            tableLayoutPanelQueryButtons.Name = "tableLayoutPanelQueryButtons";
            // 
            // buttonQueryKeep
            // 
            resources.ApplyResources(buttonQueryKeep, "buttonQueryKeep");
            buttonQueryKeep.Image = ResourceWorkbench.MarkColumnRange;
            buttonQueryKeep.Name = "buttonQueryKeep";
            toolTipQueryList.SetToolTip(buttonQueryKeep, resources.GetString("buttonQueryKeep.ToolTip"));
            buttonQueryKeep.UseVisualStyleBackColor = true;
            buttonQueryKeep.Click += buttonQueryKeep_Click;
            // 
            // buttonQueryPrevious
            // 
            resources.ApplyResources(buttonQueryPrevious, "buttonQueryPrevious");
            buttonQueryPrevious.Image = ResourceWorkbench.ArrowBackward;
            buttonQueryPrevious.Name = "buttonQueryPrevious";
            buttonQueryPrevious.UseVisualStyleBackColor = true;
            buttonQueryPrevious.Click += buttonQueryPrevious_Click;
            // 
            // buttonQueryRemove
            // 
            resources.ApplyResources(buttonQueryRemove, "buttonQueryRemove");
            buttonQueryRemove.Image = ResourceWorkbench.Ignore;
            buttonQueryRemove.Name = "buttonQueryRemove";
            toolTipQueryList.SetToolTip(buttonQueryRemove, resources.GetString("buttonQueryRemove.ToolTip"));
            buttonQueryRemove.UseVisualStyleBackColor = true;
            buttonQueryRemove.Click += buttonQueryRemove_Click;
            // 
            // buttonQueryAdd
            // 
            resources.ApplyResources(buttonQueryAdd, "buttonQueryAdd");
            buttonQueryAdd.Image = ResourceWorkbench.QueryAdd;
            buttonQueryAdd.Name = "buttonQueryAdd";
            toolTipQueryList.SetToolTip(buttonQueryAdd, resources.GetString("buttonQueryAdd.ToolTip"));
            buttonQueryAdd.UseVisualStyleBackColor = true;
            buttonQueryAdd.Click += buttonQueryAdd_Click;
            // 
            // buttonQuery
            // 
            buttonQuery.ContextMenuStrip = contextMenuStripShowSQL;
            resources.ApplyResources(buttonQuery, "buttonQuery");
            buttonQuery.Image = ResourceWorkbench.Query;
            buttonQuery.Name = "buttonQuery";
            toolTipQueryList.SetToolTip(buttonQuery, resources.GetString("buttonQuery.ToolTip"));
            buttonQuery.UseVisualStyleBackColor = true;
            buttonQuery.Click += buttonQuery_Click;
            // 
            // contextMenuStripShowSQL
            // 
            contextMenuStripShowSQL.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { showSQLToolStripMenuItem });
            contextMenuStripShowSQL.Name = "contextMenuStripShowSQL";
            resources.ApplyResources(contextMenuStripShowSQL, "contextMenuStripShowSQL");
            // 
            // showSQLToolStripMenuItem
            // 
            showSQLToolStripMenuItem.Name = "showSQLToolStripMenuItem";
            resources.ApplyResources(showSQLToolStripMenuItem, "showSQLToolStripMenuItem");
            showSQLToolStripMenuItem.Click += showSQLToolStripMenuItem_Click;
            // 
            // buttonSetQueryConditionsUpDown
            // 
            resources.ApplyResources(buttonSetQueryConditionsUpDown, "buttonSetQueryConditionsUpDown");
            buttonSetQueryConditionsUpDown.Image = ResourceWorkbench.ArrowDown;
            buttonSetQueryConditionsUpDown.Name = "buttonSetQueryConditionsUpDown";
            buttonSetQueryConditionsUpDown.Tag = "True";
            toolTipQueryList.SetToolTip(buttonSetQueryConditionsUpDown, resources.GetString("buttonSetQueryConditionsUpDown.ToolTip"));
            buttonSetQueryConditionsUpDown.UseVisualStyleBackColor = true;
            buttonSetQueryConditionsUpDown.Click += buttonSetQueryConditionsUpDown_Click;
            // 
            // labelQueryColumn
            // 
            resources.ApplyResources(labelQueryColumn, "labelQueryColumn");
            tableLayoutPanelQueryButtons.SetColumnSpan(labelQueryColumn, 3);
            labelQueryColumn.Name = "labelQueryColumn";
            // 
            // buttonShowQueryConditions
            // 
            tableLayoutPanelQueryButtons.SetColumnSpan(buttonShowQueryConditions, 2);
            resources.ApplyResources(buttonShowQueryConditions, "buttonShowQueryConditions");
            buttonShowQueryConditions.Image = Properties.Resources.ShowQueryConditions;
            buttonShowQueryConditions.Name = "buttonShowQueryConditions";
            toolTipQueryList.SetToolTip(buttonShowQueryConditions, resources.GetString("buttonShowQueryConditions.ToolTip"));
            buttonShowQueryConditions.UseVisualStyleBackColor = true;
            buttonShowQueryConditions.Click += buttonShowQueryConditions_Click;
            // 
            // buttonQueryNext
            // 
            resources.ApplyResources(buttonQueryNext, "buttonQueryNext");
            buttonQueryNext.Image = ResourceWorkbench.ArrowForward;
            buttonQueryNext.Name = "buttonQueryNext";
            buttonQueryNext.UseVisualStyleBackColor = true;
            buttonQueryNext.Click += buttonQueryNext_Click;
            // 
            // buttonQueryClear
            // 
            resources.ApplyResources(buttonQueryClear, "buttonQueryClear");
            buttonQueryClear.Image = Properties.Resources.FilterClear;
            buttonQueryClear.Name = "buttonQueryClear";
            toolTipQueryList.SetToolTip(buttonQueryClear, resources.GetString("buttonQueryClear.ToolTip"));
            buttonQueryClear.UseVisualStyleBackColor = true;
            buttonQueryClear.Click += buttonQueryClear_Click;
            // 
            // buttonQueryLoad
            // 
            resources.ApplyResources(buttonQueryLoad, "buttonQueryLoad");
            buttonQueryLoad.Image = Properties.Resources.FilterLoad;
            buttonQueryLoad.Name = "buttonQueryLoad";
            toolTipQueryList.SetToolTip(buttonQueryLoad, resources.GetString("buttonQueryLoad.ToolTip"));
            buttonQueryLoad.UseVisualStyleBackColor = true;
            buttonQueryLoad.Click += buttonQueryLoad_Click;
            // 
            // buttonQuerySave
            // 
            resources.ApplyResources(buttonQuerySave, "buttonQuerySave");
            buttonQuerySave.Image = Properties.Resources.SaveFilter3;
            buttonQuerySave.Name = "buttonQuerySave";
            toolTipQueryList.SetToolTip(buttonQuerySave, resources.GetString("buttonQuerySave.ToolTip"));
            buttonQuerySave.UseVisualStyleBackColor = true;
            buttonQuerySave.Click += buttonQuerySave_Click;
            // 
            // buttonSelectAllItems
            // 
            resources.ApplyResources(buttonSelectAllItems, "buttonSelectAllItems");
            buttonSelectAllItems.Image = Properties.Resources.MarkColumn;
            buttonSelectAllItems.Name = "buttonSelectAllItems";
            toolTipQueryList.SetToolTip(buttonSelectAllItems, resources.GetString("buttonSelectAllItems.ToolTip"));
            buttonSelectAllItems.UseVisualStyleBackColor = true;
            buttonSelectAllItems.Click += buttonSelectAllItems_Click;
            // 
            // buttonFreeText
            // 
            resources.ApplyResources(buttonFreeText, "buttonFreeText");
            buttonFreeText.Image = Properties.Resources.Lupe;
            buttonFreeText.Name = "buttonFreeText";
            toolTipQueryList.SetToolTip(buttonFreeText, resources.GetString("buttonFreeText.ToolTip"));
            buttonFreeText.UseVisualStyleBackColor = true;
            buttonFreeText.Click += buttonFreeText_Click;
            // 
            // tableLayoutPanelOrderBy
            // 
            resources.ApplyResources(tableLayoutPanelOrderBy, "tableLayoutPanelOrderBy");
            tableLayoutPanelQueryButtons.SetColumnSpan(tableLayoutPanelOrderBy, 9);
            tableLayoutPanelOrderBy.Controls.Add(buttonOptimize, 6, 0);
            tableLayoutPanelOrderBy.Controls.Add(buttonSwitchDescAsc, 5, 0);
            tableLayoutPanelOrderBy.Controls.Add(buttonQueryRemember, 4, 0);
            tableLayoutPanelOrderBy.Controls.Add(comboBoxQueryColumn, 0, 1);
            tableLayoutPanelOrderBy.Controls.Add(buttonOrderByColumnAdd, 1, 1);
            tableLayoutPanelOrderBy.Controls.Add(maskedTextBoxOrderByColumnWidth, 2, 1);
            tableLayoutPanelOrderBy.Controls.Add(pictureBoxOrderByColumnWidth, 3, 1);
            tableLayoutPanelOrderBy.Name = "tableLayoutPanelOrderBy";
            // 
            // buttonOptimize
            // 
            buttonOptimize.ContextMenuStrip = contextMenuStripOptimizing;
            resources.ApplyResources(buttonOptimize, "buttonOptimize");
            buttonOptimize.Image = Properties.Resources.SpeedUp;
            buttonOptimize.Name = "buttonOptimize";
            tableLayoutPanelOrderBy.SetRowSpan(buttonOptimize, 3);
            toolTipQueryList.SetToolTip(buttonOptimize, resources.GetString("buttonOptimize.ToolTip"));
            buttonOptimize.UseVisualStyleBackColor = true;
            buttonOptimize.Click += buttonOptimize_Click;
            // 
            // contextMenuStripOptimizing
            // 
            contextMenuStripOptimizing.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItemOptimizingAsDefault });
            contextMenuStripOptimizing.Name = "contextMenuStripOptimizing";
            resources.ApplyResources(contextMenuStripOptimizing, "contextMenuStripOptimizing");
            // 
            // toolStripMenuItemOptimizingAsDefault
            // 
            toolStripMenuItemOptimizingAsDefault.Image = Properties.Resources.Settings;
            toolStripMenuItemOptimizingAsDefault.Name = "toolStripMenuItemOptimizingAsDefault";
            resources.ApplyResources(toolStripMenuItemOptimizingAsDefault, "toolStripMenuItemOptimizingAsDefault");
            toolStripMenuItemOptimizingAsDefault.Click += toolStripMenuItemOptimizingAsDefault_Click;
            // 
            // buttonSwitchDescAsc
            // 
            resources.ApplyResources(buttonSwitchDescAsc, "buttonSwitchDescAsc");
            buttonSwitchDescAsc.Image = Properties.Resources.Sort;
            buttonSwitchDescAsc.Name = "buttonSwitchDescAsc";
            tableLayoutPanelOrderBy.SetRowSpan(buttonSwitchDescAsc, 3);
            buttonSwitchDescAsc.Tag = "ASC";
            buttonSwitchDescAsc.UseVisualStyleBackColor = true;
            buttonSwitchDescAsc.Click += buttonSwitchDescAsc_Click;
            // 
            // buttonQueryRemember
            // 
            buttonQueryRemember.ContextMenuStrip = contextMenuStripRemeber;
            resources.ApplyResources(buttonQueryRemember, "buttonQueryRemember");
            buttonQueryRemember.Image = Properties.Resources.Pin_3;
            buttonQueryRemember.Name = "buttonQueryRemember";
            tableLayoutPanelOrderBy.SetRowSpan(buttonQueryRemember, 3);
            toolTipQueryList.SetToolTip(buttonQueryRemember, resources.GetString("buttonQueryRemember.ToolTip"));
            buttonQueryRemember.UseVisualStyleBackColor = true;
            buttonQueryRemember.Click += buttonQueryRemember_Click;
            // 
            // contextMenuStripRemeber
            // 
            contextMenuStripRemeber.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { saveToolStripMenuItem, toolStripMenuItemSetAsDefault });
            contextMenuStripRemeber.Name = "contextMenuStripRemeber";
            resources.ApplyResources(contextMenuStripRemeber, "contextMenuStripRemeber");
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Image = Properties.Resources.Save;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            resources.ApplyResources(saveToolStripMenuItem, "saveToolStripMenuItem");
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // toolStripMenuItemSetAsDefault
            // 
            toolStripMenuItemSetAsDefault.Image = Properties.Resources.Settings;
            toolStripMenuItemSetAsDefault.Name = "toolStripMenuItemSetAsDefault";
            resources.ApplyResources(toolStripMenuItemSetAsDefault, "toolStripMenuItemSetAsDefault");
            toolStripMenuItemSetAsDefault.Click += toolStripMenuItemSetAsDefault_Click;
            // 
            // comboBoxQueryColumn
            // 
            resources.ApplyResources(comboBoxQueryColumn, "comboBoxQueryColumn");
            comboBoxQueryColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxQueryColumn.DropDownWidth = 200;
            comboBoxQueryColumn.FormattingEnabled = true;
            comboBoxQueryColumn.Name = "comboBoxQueryColumn";
            toolTipQueryList.SetToolTip(comboBoxQueryColumn, resources.GetString("comboBoxQueryColumn.ToolTip"));
            comboBoxQueryColumn.SelectedIndexChanged += comboBoxQueryColumn_SelectedIndexChanged;
            // 
            // buttonOrderByColumnAdd
            // 
            resources.ApplyResources(buttonOrderByColumnAdd, "buttonOrderByColumnAdd");
            buttonOrderByColumnAdd.FlatAppearance.BorderSize = 0;
            buttonOrderByColumnAdd.Image = Properties.Resources.Add;
            buttonOrderByColumnAdd.Name = "buttonOrderByColumnAdd";
            toolTipQueryList.SetToolTip(buttonOrderByColumnAdd, resources.GetString("buttonOrderByColumnAdd.ToolTip"));
            buttonOrderByColumnAdd.UseVisualStyleBackColor = true;
            buttonOrderByColumnAdd.Click += buttonOrderByColumnAdd_Click;
            // 
            // maskedTextBoxOrderByColumnWidth
            // 
            maskedTextBoxOrderByColumnWidth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(maskedTextBoxOrderByColumnWidth, "maskedTextBoxOrderByColumnWidth");
            maskedTextBoxOrderByColumnWidth.Name = "maskedTextBoxOrderByColumnWidth";
            toolTipQueryList.SetToolTip(maskedTextBoxOrderByColumnWidth, resources.GetString("maskedTextBoxOrderByColumnWidth.ToolTip"));
            maskedTextBoxOrderByColumnWidth.TextChanged += maskedTextBoxOrderByColumnWidth_TextChanged;
            // 
            // pictureBoxOrderByColumnWidth
            // 
            resources.ApplyResources(pictureBoxOrderByColumnWidth, "pictureBoxOrderByColumnWidth");
            pictureBoxOrderByColumnWidth.Image = Properties.Resources.ArrowWidthSmall;
            pictureBoxOrderByColumnWidth.Name = "pictureBoxOrderByColumnWidth";
            pictureBoxOrderByColumnWidth.TabStop = false;
            toolTipQueryList.SetToolTip(pictureBoxOrderByColumnWidth, resources.GetString("pictureBoxOrderByColumnWidth.ToolTip"));
            pictureBoxOrderByColumnWidth.Click += pictureBoxOrderByColumnWidth_Click;
            // 
            // panelOrderByColumns
            // 
            tableLayoutPanelQueryButtons.SetColumnSpan(panelOrderByColumns, 12);
            resources.ApplyResources(panelOrderByColumns, "panelOrderByColumns");
            panelOrderByColumns.Name = "panelOrderByColumns";
            // 
            // groupBoxQueryConditions
            // 
            groupBoxQueryConditions.Controls.Add(panelQueryConditions);
            groupBoxQueryConditions.Controls.Add(textBoxFreeText);
            groupBoxQueryConditions.Controls.Add(textBoxSQL);
            resources.ApplyResources(groupBoxQueryConditions, "groupBoxQueryConditions");
            groupBoxQueryConditions.Name = "groupBoxQueryConditions";
            groupBoxQueryConditions.TabStop = false;
            // 
            // panelQueryConditions
            // 
            resources.ApplyResources(panelQueryConditions, "panelQueryConditions");
            panelQueryConditions.Name = "panelQueryConditions";
            // 
            // textBoxFreeText
            // 
            resources.ApplyResources(textBoxFreeText, "textBoxFreeText");
            textBoxFreeText.Name = "textBoxFreeText";
            // 
            // textBoxSQL
            // 
            textBoxSQL.BackColor = System.Drawing.SystemColors.Control;
            textBoxSQL.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(textBoxSQL, "textBoxSQL");
            textBoxSQL.Name = "textBoxSQL";
            // 
            // toolStripQueryList
            // 
            toolStripQueryList.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripQueryList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonConnection, toolStripSeparator1, toolStripButtonSave, toolStripButtonBacklinkUpdate, toolStripButtonUndo, toolStripButtonNew, toolStripButtonCopy, toolStripButtonDelete, toolStripSeparator2, toolStripButtonOptions, toolStripButtonSwitchOrientation, toolStripButtonApplicationID });
            resources.ApplyResources(toolStripQueryList, "toolStripQueryList");
            toolStripQueryList.Name = "toolStripQueryList";
            // 
            // toolStripButtonConnection
            // 
            toolStripButtonConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonConnection.Image = ResourceWorkbench.Database;
            resources.ApplyResources(toolStripButtonConnection, "toolStripButtonConnection");
            toolStripButtonConnection.Name = "toolStripButtonConnection";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripButtonSave
            // 
            toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(toolStripButtonSave, "toolStripButtonSave");
            toolStripButtonSave.Image = ResourceWorkbench.Save;
            toolStripButtonSave.Name = "toolStripButtonSave";
            // 
            // toolStripButtonBacklinkUpdate
            // 
            toolStripButtonBacklinkUpdate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(toolStripButtonBacklinkUpdate, "toolStripButtonBacklinkUpdate");
            toolStripButtonBacklinkUpdate.Image = Properties.Resources.DatabaseUpdate;
            toolStripButtonBacklinkUpdate.Name = "toolStripButtonBacklinkUpdate";
            toolStripButtonBacklinkUpdate.Click += toolStripButtonBacklinkUpdate_Click;
            // 
            // toolStripButtonUndo
            // 
            toolStripButtonUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(toolStripButtonUndo, "toolStripButtonUndo");
            toolStripButtonUndo.Image = ResourceWorkbench.Undo;
            toolStripButtonUndo.Name = "toolStripButtonUndo";
            // 
            // toolStripButtonNew
            // 
            toolStripButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(toolStripButtonNew, "toolStripButtonNew");
            toolStripButtonNew.Image = ResourceWorkbench.New;
            toolStripButtonNew.Name = "toolStripButtonNew";
            // 
            // toolStripButtonCopy
            // 
            toolStripButtonCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(toolStripButtonCopy, "toolStripButtonCopy");
            toolStripButtonCopy.Image = ResourceWorkbench.Copy;
            toolStripButtonCopy.Name = "toolStripButtonCopy";
            // 
            // toolStripButtonDelete
            // 
            toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(toolStripButtonDelete, "toolStripButtonDelete");
            toolStripButtonDelete.Image = ResourceWorkbench.Delete;
            toolStripButtonDelete.Name = "toolStripButtonDelete";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(toolStripSeparator2, "toolStripSeparator2");
            // 
            // toolStripButtonOptions
            // 
            toolStripButtonOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(toolStripButtonOptions, "toolStripButtonOptions");
            toolStripButtonOptions.Image = ResourceWorkbench.Option;
            toolStripButtonOptions.Name = "toolStripButtonOptions";
            toolStripButtonOptions.Click += toolStripButtonOptions_Click;
            // 
            // toolStripButtonSwitchOrientation
            // 
            toolStripButtonSwitchOrientation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonSwitchOrientation.Image = ResourceWorkbench.QueryHorizontal1;
            resources.ApplyResources(toolStripButtonSwitchOrientation, "toolStripButtonSwitchOrientation");
            toolStripButtonSwitchOrientation.Name = "toolStripButtonSwitchOrientation";
            // 
            // toolStripButtonApplicationID
            // 
            toolStripButtonApplicationID.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            toolStripButtonApplicationID.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonApplicationID.Image = Properties.Resources.ToServer;
            resources.ApplyResources(toolStripButtonApplicationID, "toolStripButtonApplicationID");
            toolStripButtonApplicationID.Name = "toolStripButtonApplicationID";
            toolStripButtonApplicationID.Click += toolStripButtonApplicationID_Click;
            // 
            // imageListConnectionState
            // 
            imageListConnectionState.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListConnectionState.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListConnectionState.ImageStream");
            imageListConnectionState.TransparentColor = System.Drawing.Color.Transparent;
            imageListConnectionState.Images.SetKeyName(0, "Database.ico");
            imageListConnectionState.Images.SetKeyName(1, "NoDatabase.ico");
            // 
            // imageListOrientation
            // 
            imageListOrientation.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListOrientation.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListOrientation.ImageStream");
            imageListOrientation.TransparentColor = System.Drawing.Color.Transparent;
            imageListOrientation.Images.SetKeyName(0, "QueryHorizontal.ico");
            imageListOrientation.Images.SetKeyName(1, "QueryVertical.ico");
            // 
            // imageListAscDesc
            // 
            imageListAscDesc.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListAscDesc.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListAscDesc.ImageStream");
            imageListAscDesc.TransparentColor = System.Drawing.Color.Transparent;
            imageListAscDesc.Images.SetKeyName(0, "ArrowDownSmall.ico");
            imageListAscDesc.Images.SetKeyName(1, "ArrowUpSmall.ico");
            imageListAscDesc.Images.SetKeyName(2, "Sort_Desc.ico");
            imageListAscDesc.Images.SetKeyName(3, "Sort.ico");
            // 
            // contextMenuStripOrderBy
            // 
            contextMenuStripOrderBy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { addPrefixToolStripMenuItem, removePrefixToolStripMenuItem });
            contextMenuStripOrderBy.Name = "contextMenuStripOrderBy";
            resources.ApplyResources(contextMenuStripOrderBy, "contextMenuStripOrderBy");
            // 
            // addPrefixToolStripMenuItem
            // 
            addPrefixToolStripMenuItem.Image = Properties.Resources.Add;
            addPrefixToolStripMenuItem.Name = "addPrefixToolStripMenuItem";
            resources.ApplyResources(addPrefixToolStripMenuItem, "addPrefixToolStripMenuItem");
            addPrefixToolStripMenuItem.Click += AddPrefixToolStripMenuItem_Click;
            // 
            // removePrefixToolStripMenuItem
            // 
            removePrefixToolStripMenuItem.Image = Properties.Resources.Minus;
            removePrefixToolStripMenuItem.Name = "removePrefixToolStripMenuItem";
            resources.ApplyResources(removePrefixToolStripMenuItem, "removePrefixToolStripMenuItem");
            removePrefixToolStripMenuItem.Click += RemovePrefixToolStripMenuItem_Click;
            // 
            // contextMenuStripBacklink
            // 
            contextMenuStripBacklink.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { insertCurrentIDToolStripMenuItem, insertAllIDsInBacklinkListToolStripMenuItem, showCurrentListOfIDsForBacklinkToolStripMenuItem, resetBacklinkListToolStripMenuItem });
            contextMenuStripBacklink.Name = "contextMenuStripBacklink";
            resources.ApplyResources(contextMenuStripBacklink, "contextMenuStripBacklink");
            // 
            // insertCurrentIDToolStripMenuItem
            // 
            insertCurrentIDToolStripMenuItem.Image = Properties.Resources.ListAdd;
            insertCurrentIDToolStripMenuItem.Name = "insertCurrentIDToolStripMenuItem";
            resources.ApplyResources(insertCurrentIDToolStripMenuItem, "insertCurrentIDToolStripMenuItem");
            insertCurrentIDToolStripMenuItem.Click += insertCurrentIDToolStripMenuItem_Click;
            // 
            // insertAllIDsInBacklinkListToolStripMenuItem
            // 
            insertAllIDsInBacklinkListToolStripMenuItem.Image = Properties.Resources.ListAddAdd;
            insertAllIDsInBacklinkListToolStripMenuItem.Name = "insertAllIDsInBacklinkListToolStripMenuItem";
            resources.ApplyResources(insertAllIDsInBacklinkListToolStripMenuItem, "insertAllIDsInBacklinkListToolStripMenuItem");
            insertAllIDsInBacklinkListToolStripMenuItem.Click += insertAllIDsInBacklinkListToolStripMenuItem_Click;
            // 
            // showCurrentListOfIDsForBacklinkToolStripMenuItem
            // 
            showCurrentListOfIDsForBacklinkToolStripMenuItem.Image = Properties.Resources.Lupe;
            showCurrentListOfIDsForBacklinkToolStripMenuItem.Name = "showCurrentListOfIDsForBacklinkToolStripMenuItem";
            resources.ApplyResources(showCurrentListOfIDsForBacklinkToolStripMenuItem, "showCurrentListOfIDsForBacklinkToolStripMenuItem");
            showCurrentListOfIDsForBacklinkToolStripMenuItem.Click += showCurrentListOfIDsForBacklinkToolStripMenuItem_Click;
            // 
            // resetBacklinkListToolStripMenuItem
            // 
            resetBacklinkListToolStripMenuItem.Image = Properties.Resources.ListNot;
            resetBacklinkListToolStripMenuItem.Name = "resetBacklinkListToolStripMenuItem";
            resources.ApplyResources(resetBacklinkListToolStripMenuItem, "resetBacklinkListToolStripMenuItem");
            resetBacklinkListToolStripMenuItem.Click += resetBacklinkListToolStripMenuItem_Click;
            // 
            // UserControlQueryList
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitContainerMain);
            Controls.Add(toolStripQueryList);
            Name = "UserControlQueryList";
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            groupBoxQueryResults.ResumeLayout(false);
            tableLayoutPanelQueryButtons.ResumeLayout(false);
            tableLayoutPanelQueryButtons.PerformLayout();
            contextMenuStripShowSQL.ResumeLayout(false);
            tableLayoutPanelOrderBy.ResumeLayout(false);
            tableLayoutPanelOrderBy.PerformLayout();
            contextMenuStripOptimizing.ResumeLayout(false);
            contextMenuStripRemeber.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxOrderByColumnWidth).EndInit();
            groupBoxQueryConditions.ResumeLayout(false);
            groupBoxQueryConditions.PerformLayout();
            toolStripQueryList.ResumeLayout(false);
            toolStripQueryList.PerformLayout();
            contextMenuStripOrderBy.ResumeLayout(false);
            contextMenuStripBacklink.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxQueryResults;
        private System.Windows.Forms.ComboBox comboBoxQueryColumn;
        private System.Windows.Forms.Label labelQueryColumn;
        private System.Windows.Forms.GroupBox groupBoxQueryConditions;
        private System.Windows.Forms.TextBox textBoxSQL;
        private System.Windows.Forms.ToolTip toolTipQueryList;
        private System.Windows.Forms.Button buttonSetQueryConditionsUpDown;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.Button buttonQueryAdd;
        public System.Windows.Forms.Button buttonQuery;
        public System.Windows.Forms.ListBox listBoxQueryResult;
        public System.Windows.Forms.ToolStripButton toolStripButtonConnection;
        public System.Windows.Forms.ToolStripButton toolStripButtonCopy;
        public System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        public System.Windows.Forms.ToolStripButton toolStripButtonNew;
        public System.Windows.Forms.ToolStripButton toolStripButtonSave;
        public System.Windows.Forms.ToolStripButton toolStripButtonUndo;
        private System.Windows.Forms.ImageList imageListConnectionState;
        private System.Windows.Forms.Button buttonQueryRemove;
        public System.Windows.Forms.ToolStrip toolStripQueryList;
        public System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.ImageList imageListOrientation;
        private System.Windows.Forms.Panel panelQueryConditions;
        public System.Windows.Forms.ToolStripButton toolStripButtonSwitchOrientation;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelQueryButtons;
        private System.Windows.Forms.Button buttonShowQueryConditions;
        private System.Windows.Forms.Button buttonQueryNext;
        private System.Windows.Forms.Button buttonQueryClear;
        private System.Windows.Forms.Button buttonQueryPrevious;
        private System.Windows.Forms.Button buttonQueryLoad;
        private System.Windows.Forms.Button buttonQuerySave;
        private System.Windows.Forms.Button buttonSelectAllItems;
        private System.Windows.Forms.Button buttonSwitchDescAsc;
        private System.Windows.Forms.ImageList imageListAscDesc;
        private System.Windows.Forms.Button buttonFreeText;
        private System.Windows.Forms.TextBox textBoxFreeText;
        private System.Windows.Forms.Button buttonOptimize;
        public System.Windows.Forms.ToolStripButton toolStripButtonOptions;
        private System.Windows.Forms.Button buttonQueryRemember;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripRemeber;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripShowSQL;
        private System.Windows.Forms.ToolStripMenuItem showSQLToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripOrderBy;
        private System.Windows.Forms.ToolStripMenuItem addPrefixToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removePrefixToolStripMenuItem;
        private System.Windows.Forms.Button buttonQueryKeep;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelOrderBy;
        private System.Windows.Forms.Button buttonOrderByColumnAdd;
        private System.Windows.Forms.Panel panelOrderByColumns;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxOrderByColumnWidth;
        private System.Windows.Forms.PictureBox pictureBoxOrderByColumnWidth;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSetAsDefault;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripOptimizing;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOptimizingAsDefault;
        public System.Windows.Forms.ToolStripButton toolStripButtonBacklinkUpdate;
        private System.Windows.Forms.ToolStripButton toolStripButtonApplicationID;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripBacklink;
        private System.Windows.Forms.ToolStripMenuItem insertCurrentIDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetBacklinkListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showCurrentListOfIDsForBacklinkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertAllIDsInBacklinkListToolStripMenuItem;
    }
}
