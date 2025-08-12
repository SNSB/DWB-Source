using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;
using DWBServices.WebServices;
using DWBServices.WebServices.TaxonomicServices.CatalogueOfLife;
using DWBServices;
using Microsoft.Extensions.DependencyInjection;
using DiversityWorkbench.DwbManual;

namespace DiversityWorkbench.Spreadsheet
{
    public partial class FormSpreadsheet : Form
    {

        #region Parameter

        private Sheet _Sheet;
        private Setting _Setting;

        private bool _IsStarter;

        private bool _ResetSettings = false;

        private bool _SuppressRescanOfDataGrid = false;

        private DataColumn _ImageColumn;
        public void setImageColumn(DataColumn IC) 
        { 
            this._ImageColumn = IC; 
            this.toolTip.SetToolTip(this.pictureBoxImage, "Image as stored in Table " + IC.Table.Name + " in Column " + IC.Column.Name + " (if present)");
        }

        private System.Drawing.Font _FontBold = new Font(FontFamily.GenericSansSerif, 8.25f, FontStyle.Bold);

        #endregion

        #region Construction

        public FormSpreadsheet(Sheet Sheet, string HelpNameSpace)
        {
            try
            {
                InitializeComponent();
//#if !DEBUG
//                ///TODO: Nach Behebung des Fehlers wieder entfernen
//                this.buttonStart.Visible = false;
//#endif
                this.useThisRowAsTemplateForNewRowsToolStripMenuItem.Enabled = false;
                this.copyValuesToClipboardToolStripMenuItem.Enabled = false;
                this.transferValuesToClipboardToolStripMenuItem.Enabled = false;
                this.insertValesFromClipboardToolStripMenuItem.Enabled = false;
                this.toolStripMenuItemRemove.Enabled = false;

                if (DiversityWorkbench.Settings.ConnectionString.Length == 0)
                    this.ConnectToDatabase();
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                    this.buttonConnect.Image = DiversityWorkbench.ResourceWorkbench.Database;
                this._Sheet = Sheet;
                this.Text = this._Sheet.DisplayText();
                this.labelSetting.Text = this._Sheet.TargetDisplayText();//.Target();
                this.toolTip.SetToolTip(this.labelSetting, this._Sheet.TargetDisplayText());
                try
                {
                    bool ColorFound = false;
                    foreach (System.Collections.Generic.KeyValuePair<string, DataTable> DT in this._Sheet.DataTables())
                    {
                        if (DT.Value.Type() == DataTable.TableType.Target)
                        {
                            this.labelSetting.BackColor = DT.Value.ColorBack();
                            ColorFound = true;
                            break;
                        }
                    }
                    if (!ColorFound)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DataTable> DT in this._Sheet.DataTables())
                        {
                            if (DT.Value.Type() == DataTable.TableType.Root)
                            {
                                this.labelSetting.BackColor = DT.Value.ColorBack();
                                ColorFound = true;
                                break;
                            }
                        }
                    }
                }
                catch { }

                this.toolTip.SetToolTip(buttonTimeout, DiversityWorkbench.Spreadsheet.FormSpreadsheetText.Current_timeout + ": " + DiversityWorkbench.Settings.TimeoutDatabase.ToString() + " sec.");
                this.EditingEnableEditing();//false);

                // Map controls only for certain targets
                if (this._Sheet.Target() == "TK25")
                {
                    this.toolStripMap.Visible = true;
                }
                else
                {
                    this.toolStripMap.Visible = false;
                    if (this._Sheet.Target() == "Tasks")
                    {
                        this.buttonShowGeometry.Visible = true;
                        this.buttonSetGeometryIcons.Visible = true; ;
                    }
                }

                // Reading the settings
                this._Setting = new Setting(ref this._Sheet);
                if (!this._Setting.ReadSettings())
                {
                    if (this._Sheet.MaxResult() == 0)
                        this._Sheet.setMaxResult(50);
                }

                this.ReadUserSettings();

                // apply the settings
                if (this._Sheet.Target() == "Tasks")
                {
                    this._Sheet.ResetProjects();
                    this.comboBoxProject.Visible = false;
                    this.labelProject.Visible = false;
                    this.checkBoxProject.Visible = false;
                }
                else
                this.setProjectSource();
                int Max = this._Sheet.MaxResult();
                if (Max <= this.numericUpDownMaxResults.Maximum && Max > this.numericUpDownMaxResults.Minimum)
                    this.numericUpDownMaxResults.Value = Max;
                    
                int iMaxResult = this._Sheet.MaxResult();
                this._Sheet.setMaxResult(0);
                bool OK = this.RequeryAllGrids();
                if (OK)
                {
                    this._Sheet.setMaxResult(iMaxResult);
                    this.FilterAlarm_Start();
                    this.helpProvider.HelpNamespace = HelpNameSpace;
                    this.setContextMenu();
                    this.buttonFilter.Focus();
                    this._Sheet.setMaxResult((int)this.numericUpDownMaxResults.Value);
                }
                else
                {
                    this.buttonResetToFactorySettings.Visible = true;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        #endregion

        #region Project

        private bool _ProjectFound = false;
        private void setProjectSource(bool ReadOnly = false)
        {
            try
            {
                this._Sheet.ResetProjects();
                if (ReadOnly)
                    this.comboBoxProject.DataSource = this._Sheet.DtProjectsReadOnly();
                else
                    this.comboBoxProject.DataSource = this._Sheet.DtProjects();
                this.comboBoxProject.DisplayMember = "Project";
                this.comboBoxProject.ValueMember = "ProjectID";
                _ProjectFound = false;
                if (ReadOnly)
                {
                    for (int i = 0; i < this._Sheet.DtProjectsReadOnly().Rows.Count; i++)
                    {
                        if (this._Sheet.DtProjectsReadOnly().ToString() == this._Sheet.DtProjectsReadOnly().Rows[i]["ProjectID"].ToString())
                        {
                            this.comboBoxProject.SelectedIndex = i;
                            _ProjectFound = true;
                            break;
                        }
                    }
                    if (!_ProjectFound && this._Sheet.DtProjectsReadOnly().Rows.Count > 0)
                    {
                        this.comboBoxProject.SelectedIndex = 0;
                        this.ProjectAlarm_Start();
                    }
                }
                else
                {
                    for (int i = 0; i < this._Sheet.DtProjects().Rows.Count; i++)
                    {
                        if (this._Sheet.ProjectID().ToString() == this._Sheet.DtProjects().Rows[i]["ProjectID"].ToString())
                        {
                            this.comboBoxProject.SelectedIndex = i;
                            _ProjectFound = true;
                            break;
                        }
                    }
                    if (!_ProjectFound && this._Sheet.DtProjects().Rows.Count > 0)
                    {
                        this.comboBoxProject.SelectedIndex = 0;
                        this.ProjectAlarm_Start();
                    }
                }
                if (this._Sheet.DtProjectsReadOnly().Rows.Count > 0 && this._Sheet.DtProjects().Rows.Count > 0)
                    this.checkBoxProject.Visible = true;
                else
                    this.checkBoxProject.Visible = false;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        private void checkBoxProject_Click(object sender, EventArgs e)
        {
            this._Sheet.ProjectReadOnly = !this._Sheet.ProjectReadOnly;
            this._Sheet.ReadOnly = this._Sheet.ProjectReadOnly;
            this.SetControlsAccordingToProjectReadOnlyState();
            this.setControlsAccordingToReadOnly();
            this._Sheet.DT().Clear();
            this.ProjectAlarm_Start();
        }

        private void SetControlsAccordingToProjectReadOnlyState()
        {
            this.setProjectSource(this._Sheet.ProjectReadOnly);
            if (this._Sheet.HasProjects() && this._Sheet.HasReadOnlyProjects())
            {
                this.checkBoxProject.Visible = true;
                if (this._Sheet.ProjectReadOnly)
                {
                    this.checkBoxProject.Image = DiversityWorkbench.Properties.Resources.Project;
                    this.toolTip.SetToolTip(this.checkBoxProject, "Read only projects. Click here to change to write accessible projects");
                }
                else
                {
                    this.checkBoxProject.Image = DiversityWorkbench.Properties.Resources.ProjectOpen;
                    this.toolTip.SetToolTip(this.checkBoxProject, "Write accessible projects. Click here to change to read only projects");
                }
            }
            else
            {
                this.checkBoxProject.Visible = false;
            }
            if (this._Sheet.ProjectReadOnly)
            {
                this.buttonReadOnly.Enabled = false;
            }
            else if (this._Sheet.HasProjects())
            {
                this.buttonReadOnly.Enabled = true;
            }
        }

        #endregion

        #region Form

        private void initForm()
        {
        }

        private void FormSpreadsheet_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!this._ResetSettings)
                this.SaveSetting();
            if (this.IsForGettingFromWhereClause)
            {
                this.Hide();
                e.Cancel = true;
                return;
            }
        }

        public void setSize(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
        }

        public void setLocation(int X, int Y)
        {
            this.Location = new Point(X, Y);
        }
        
        #endregion

        #region Feedback

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }
        
        #endregion

        #region Settings

        /// <summary>
        /// Reading the settings from the table UserProxy esp. the links to the modules
        /// </summary>
        private void ReadUserSettings()
        {
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                {
                    if (DC.Value.LinkedModule != RemoteLink.LinkedModule.None)
                    {
                        if (DC.Value.IsLinkColumn != null && (bool)DC.Value.IsLinkColumn)
                        {
                            UserSettings U = new UserSettings();
                            System.Collections.Generic.List<string> L = DC.Value.FixedSourceGetSetting();
                            string DB = U.GetSetting(L, "Database");
                            if (DB.Length > 0)
                            {
                                string Project = U.GetSetting(L, "Project");
                                string ProjectID = U.GetSetting(L, "ProjectID");
                                DiversityWorkbench.ServerConnection SC = new ServerConnection(DiversityWorkbench.Settings.ConnectionString);
                                SC.DatabaseName = DB;
                                SC.Project = Project;
                                string Module = U.GetSetting(L, "Module");
                                if (Module.Length > 0)
                                    SC.ModuleName = Module;
                                else
                                    SC.ModuleName = DC.Value.LinkedModule.ToString();
                                int iProjectID;
                                if (int.TryParse(ProjectID, out iProjectID))
                                    SC.ProjectID = iProjectID;
                                string baseUrl = this.BaseURL(DiversityWorkbench.Settings.ConnectionString);
                                SC.BaseURL = baseUrl;
                                this._Sheet.setFixSourceConnection(DC.Value.DataTable().Alias(), DC.Value.Name, SC);
                            }
                            else
                            {
                                string Webservice = U.GetSetting(L, "Webservice");
                                if (Webservice.Length > 0)
                                {
                                    // No action for webservices
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string BaseURL(string ConnectionString)
        {
            string baseUrl = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("Select dbo.BaseURL()", ConnectionString);
            if (baseUrl.Length == 0)
                baseUrl = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("Select top 1 BaseURL ViewBaseURL", ConnectionString);
            return baseUrl;

        }

        private void ReadSettings()
        {
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            try
            {
                //FormTableSettings f = new FormTableSettings(ref this._Sheet, this.helpProvider.HelpNamespace);
                //f.ShowDialog();
                //if (Sheet.RebuildNeeded)
                //{
                //    this.RequeryAllGrids();
                //    Sheet.RebuildNeeded = false;
                //}
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonSettingsAddTable_Click(object sender, EventArgs e)
        {
            try
            {
                FormTableSettingsSingle fT = new FormTableSettingsSingle(ref this._Sheet, "", this.helpProvider.HelpNamespace, true, false);
                fT.ShowDialog();
                if (Sheet.RebuildNeeded)
                {
                    this.RequeryAllGrids();
                    Sheet.RebuildNeeded = false;
                }
                fT.Dispose();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private bool SaveSetting()
        {
            bool OK = true;
            if (this._Setting != null)
                this._Setting.WriteSettings();
            try
            {
                if (this._Sheet.FixSourceConnections() != null)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, ServerConnection> SC in this._Sheet.FixSourceConnections())
                    {
                        string[] TableColumn = SC.Key.Split(new char[] { '.' });
                        if (TableColumn.Length == 2 && this._Sheet.DataTables().ContainsKey(TableColumn[0]))
                        {
                            if (this._Sheet.DataTables()[TableColumn[0]].DataColumns().ContainsKey(TableColumn[1]))
                            {
                                if (this._Sheet.DataTables()[TableColumn[0]].DataColumns()[TableColumn[1]].LinkedModule != RemoteLink.LinkedModule.None)
                                {
                                    if (this._Sheet.DataTables()[TableColumn[0]].DataColumns()[TableColumn[1]].IsLinkColumn != null
                                        && (bool)this._Sheet.DataTables()[TableColumn[0]].DataColumns()[TableColumn[1]].IsLinkColumn)
                                    {
                                        System.Collections.Generic.List<string> Setting = this._Sheet.DataTables()[TableColumn[0]].DataColumns()[TableColumn[1]].FixedSourceGetSetting();
                                        if (Setting.Count > 0)
                                        {
                                            DiversityWorkbench.UserSettings U = new UserSettings();
                                            if (SC.Value != null)
                                            {
                                                if (SC.Value.ModuleName == "DiversityCollectionCache")
                                                {
                                                    U.DeleteSettingAttribute(Setting, "Webservice");
                                                    if (SC.Value.Project != null)
                                                    {
                                                        U.SaveSetting(Setting, "Project", SC.Value.Project);
                                                        U.SaveSetting(Setting, "ProjectID", SC.Value.ProjectID.ToString());
                                                    }
                                                    U.SaveSetting(Setting, "Database", SC.Value.DisplayText);
                                                    U.SaveSetting(Setting, "Module", SC.Value.ModuleName);
                                                }
                                                else if (SC.Value.ProjectID != null)
                                                {
                                                    U.DeleteSettingAttribute(Setting, "Webservice");
                                                    U.SaveSetting(Setting, "Project", SC.Value.Project);
                                                    U.SaveSetting(Setting, "ProjectID", SC.Value.ProjectID.ToString());
                                                    U.SaveSetting(Setting, "Database", SC.Value.DisplayText);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        private void buttonSettingsSave_Click(object sender, EventArgs e)
        {
            string _Setting = this._Sheet.Target();
            if (this.labelSetting.Text.Length > 0) 
                _Setting = this.labelSetting.Text;
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString(DiversityWorkbench.Spreadsheet.FormSpreadsheetText.Save_settings, DiversityWorkbench.Spreadsheet.FormSpreadsheetText.Please_enter_the_name_of_the_settings, _Setting);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                this._Setting.WriteSettings(f.String);
                this.labelSetting.Text = f.String;
                this.toolTip.SetToolTip(this.labelSetting, f.String);
            }
            f.Dispose();
        }

        private void buttonSettingsLoad_Click(object sender, EventArgs e)
        {
            if (this._Setting.TargetSettings().Count > 0)
            {
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(this._Setting.TargetSettings(), DiversityWorkbench.Spreadsheet.FormSpreadsheetText.Load_settings, DiversityWorkbench.Spreadsheet.FormSpreadsheetText.Please_select_the_settings, true);
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    this._Setting.ReadSettings(f.SelectedValue);
                    this.numericUpDownMaxResults.Value = this._Sheet.MaxResult();
                    this.RequeryAllGrids();
                    this.labelSetting.Text = f.SelectedString;
                    this.toolTip.SetToolTip(this.labelSetting, f.SelectedString);
                    this.setProjectSource();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
                f.Dispose();
            }
            else
                System.Windows.Forms.MessageBox.Show(DiversityWorkbench.Spreadsheet.FormSpreadsheetText.No_settings_available);
        }

        private void buttonSettingsReset_Click(object sender, EventArgs e)
        {
            this.ResetSettings();
        }

        private void buttonResetToFactorySettings_Click(object sender, EventArgs e)
        {
            this.ResetSettings();
        }

        private void ResetSettings()
        {
            if (this._Setting.ResetSettings())
            {
                System.Windows.Forms.MessageBox.Show("Settings were reset.\r\nWindow will be closed.\r\nPlease reopen sheet to start with factory settings");
                this._ResetSettings = true;
                this.Close();
            }
            else
                System.Windows.Forms.MessageBox.Show("Reset failed");
        }

        #region Context and language - obsolet, not used any more

        private void buttonLanguage_Click(object sender, EventArgs e)
        {
            //DiversityWorkbench.Spreadsheet.FormChooseLanguage f = new FormChooseLanguage(DiversityWorkbench.Settings.Language);
            //f.ShowDialog();
            //this.setLanguage(f.LanguageCode());
        }

        private void labelContext_Click(object sender, EventArgs e)
        {
            //if (DiversityWorkbench.Entity.DtContext.Rows.Count == 0)
            //    return;

            //DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(DiversityWorkbench.Entity.DtContext, "DisplayText", "Code", "Context", "Please select a context");
            //f.ShowDialog();
            //if (f.DialogResult == DialogResult.OK)
            //{
            //    if (DiversityWorkbench.Settings.Context != f.SelectedValue)
            //    {
            //        DiversityWorkbench.Settings.Context = f.SelectedValue;
            //        DiversityWorkbench.Entity.setEntity(this, this.toolTip);
            //        this.labelContext.Text = f.SelectedString.ToUpper();
            //    }
            //}
        }

        private void setContextMenu()
        {
//            try
//            {
//                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
//                {
//                    bool OK = DiversityWorkbench.Entity.EntityTablesExist;

//                    // TODO - wieder einblenden wenn Verwendung fertig
//                    OK = false;
//#if !DEBUG
//#endif
//                    this.buttonLanguage.Visible = OK;
//                    this.labelContext.Visible = OK;
//                    if (OK)
//                    {
//                        string CurrentContext = DiversityWorkbench.Settings.Context;
//                        System.Data.DataRow[] RR = DiversityWorkbench.Entity.DtContext.Select("Code = '" + DiversityWorkbench.Settings.Context + "'", "");
//                        if (RR.Length == 1)
//                            CurrentContext = RR[0]["DisplayText"].ToString();
//                        this.labelContext.Text = CurrentContext.ToUpper();
//                        this.buttonLanguage.Image = DiversityWorkbench.Entity.LanguageImage;
//                    }
//                }
//            }
//            catch (System.Exception ex)
//            {
//                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
//            }
        }

        private void setLanguage(string LanguageCode)
        {
            //try
            //{
            //    if (LanguageCode != DiversityWorkbench.Settings.Language)
            //    {
            //        DiversityWorkbench.Settings.Language = LanguageCode;
            //        this.buttonLanguage.Image = DiversityWorkbench.Entity.LanguageImage;
            //        DiversityWorkbench.Entity.setEntity(this, this.toolTip);
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //}
            //try
            //{
            //    if (DiversityWorkbench.Settings.Language != System.Globalization.CultureInfo.CurrentUICulture.Name)
            //    {
            //        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(DiversityWorkbench.Settings.Language);
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //}
        }
        
        #endregion

        #endregion

        #region Starting form

        public void StartingEnabled(bool IsEnabled)
        {
            this.buttonStart.Visible = IsEnabled;
        }
        
        private void buttonStart_Click(object sender, EventArgs e)
        {
            this.IsStarter = !this._IsStarter;
        }

        /// <summary>
        /// if the form is used to start the access to the database without opening the main form
        /// </summary>
        public bool IsStarter
        {
            get { return _IsStarter; }
            set
            {
                _IsStarter = value;
                if (value)
                    this.buttonStart.BackColor = System.Drawing.Color.Red;
                else this.buttonStart.BackColor = System.Drawing.SystemColors.Control;
            }
        }
        
        #endregion

        #region IsForGettingWhereClause
        
        private bool _IsForGettingFromWhereClause;

        /// <summary>
        /// if the form is used to get the where clause
        /// </summary>
        public bool IsForGettingFromWhereClause
        {
            get { return _IsForGettingFromWhereClause; }
            set
            {
                _IsForGettingFromWhereClause = value;
            }
        }

        public string FromWhereClause(string ResultTableAlias)
        {
            return " FROM " + this._Sheet.FromClause(ResultTableAlias) + " WHERE " + this._Sheet.WhereClause(); ;// this._Sheet.W
        }

        public string FromWhereClause(System.Collections.Generic.List<string> IncludedTableAliases)
        {
            return " FROM " + this._Sheet.FromClause(IncludedTableAliases) + " WHERE " + this._Sheet.WhereClause(); ;// this._Sheet.W
        }

        #endregion        

        #region Data grids - formatting and restart

        private bool RequeryAllGrids()
        {
            bool OK = true;
            this.Cursor = Cursors.WaitCursor;
            this.SuspendLayout();
            try
            {
                this.ResetGrids();

                // setting the filter
                if (this.initFilterTable())
                {
                    this.SetFilterSettings();

                    //this.getDataStructureToFilterAndAdding();
                    this.initAddingTable();
                    this.SetDefaultsForAdding();

                    // get the data
                    OK = this.getData();
                    this.initColumnWidthsAndVisibility();
                }
                else
                    OK = false;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.ResumeLayout();
            this.Cursor = Cursors.Default;
            return OK;
        }

        private readonly System.Drawing.Color _ColorNull = System.Drawing.Color.LightGray;

        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<int, System.Drawing.Color>> _TableBackColors;

        private void ResetGrids()
        {
            this.dataGridViewFilter.DataSource = null;
            this.dataGridView.DataSource = null;
            this.dataGridViewAdding.DataSource = null;

            foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridViewAdding.Columns)
                C.Dispose();
            this.dataGridViewAdding.Columns.Clear();

            foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                C.Dispose();
            this.dataGridView.Columns.Clear();

            foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridViewFilter.Columns)
                C.Dispose();
            this.dataGridViewFilter.Columns.Clear();

            this._Sheet.ResetSelectedColumns();
        }

        private void initColumnWidthsAndVisibility()
        {
            this.SuspendLayout();
            try
            {
                this.panelTableMarker.SuspendLayout();
                for (int i = 0; i < this._Sheet.DT().Columns.Count; i++)
                {
                    if (this._Sheet.SelectedColumns().ContainsKey(i))
                    {
                        if (this.dataGridViewFilter.Columns.Count > i)
                        {
                            if (!this._Sheet.SelectedColumns()[i].IsVisible)
                            {
                                this.dataGridView.Columns[i].Visible = false;
                                this.dataGridViewFilter.Columns[i].Visible = false;
                                if (this._AddingPossible)
                                    this.dataGridViewAdding.Columns[i].Visible = false;
                            }
                            else if (this._Sheet.SelectedColumns()[i].IsHidden)
                            {
                                this.dataGridView.Columns[i].Visible = false;
                                this.dataGridViewFilter.Columns[i].Visible = false;
                                if (this._AddingPossible)
                                    this.dataGridViewAdding.Columns[i].Visible = false;
                            }
                            else
                            {
                                if (this._Sheet.SelectedColumns()[i].Type() != DataColumn.ColumnType.Data)
                                {
                                    if (this._Sheet.SelectedColumns()[i].Type() == DataColumn.ColumnType.Operation)
                                        this.dataGridViewFilter.Columns[i].Width = DataColumn.OperationWidth();
                                    if (this._Sheet.SelectedColumns()[i].Type() == DataColumn.ColumnType.Spacer)
                                        this.dataGridViewFilter.Columns[i].Width = DataColumn.SpacerWidth();
                                }
                                else
                                {
                                    int Width = this._Sheet.SelectedColumns()[i].Width;
                                    this.dataGridViewFilter.Columns[i].Width = this._Sheet.SelectedColumns()[i].Width;
                                }
                            }
                        }
                        else
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("initColumnWidthsAndVisibility()", "this.dataGridViewFilter.Columns.Count > " + i.ToString(), "The dataGridViewFilter contains only " + this.dataGridViewFilter.Columns.Count.ToString() + " columns");
                    }
                    else
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("initColumnWidthsAndVisibility()", "this._Sheet.SelectedColumns().ContainsKey(i)", "The SelectedColumns() did not contain a column as position " + i.ToString());
                }
                if (this.dataGridView.Width > this.Width)
                {
                    Screen myScreen = Screen.FromControl(this);
                    if (myScreen.WorkingArea.Width > this.Width)
                    {
                        if (this.dataGridView.Width > myScreen.WorkingArea.Width - 20)
                            this.Width = myScreen.WorkingArea.Width - 20;
                        else
                            this.Width = this.dataGridView.Width + 20;
                    }
                }
                this.panelTableMarker.ResumeLayout();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.ResumeLayout();
        }

        private void setDataDefaultToolTips()
        {
            if (this._Sheet.ReadOnly)
                return;

            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.Type() == DataColumn.ColumnType.Operation)
                {
                    for(int i = 0; i < this._Sheet.DT().Rows.Count; i++)
                    this.dataGridView.Rows[i].Cells[DC.Key].ToolTipText = "Delete data from " + this._Sheet.SelectedColumns()[DC.Key].DataTable().DisplayText;
                }
            }
        }

        private void buttonAdaptColumnWidth_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Spreadsheet.FormSetColumnWidth f = new FormSetColumnWidth(this._Sheet);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                this._Sheet.setMaxColumnWidth(f.MaxColumnWidth());
                this.setColumnWidths(f.ModeForColumnWidth());
            }
            f.Dispose();
        }

        private void setColumnWidths(DiversityWorkbench.Spreadsheet.FormSetColumnWidth.ColumnWidthMode Mode)
        {
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                {
                    int WidthHeader;
                    if (this.dataGridViewFilter.Columns[DC.Key].HeaderText.Length > 0 && (DC.Value.Type() == DataColumn.ColumnType.Data || DC.Value.Type() == DataColumn.ColumnType.Link))
                        WidthHeader = this.ColumnHeaderWidth(this.dataGridViewFilter.Columns[DC.Key]);// + 6; // + 1 = round to ceiling
                    else
                        WidthHeader = this.StringLength(DC.Value.DisplayText);

                    int WidthContent;
                    if (this.dataGridViewFilter.Columns[DC.Key].HeaderText.Length > 0 && (DC.Value.Type() == DataColumn.ColumnType.Data || DC.Value.Type() == DataColumn.ColumnType.Link))
                        WidthContent = this.GetMaxContentLength(this.dataGridViewFilter.Columns[DC.Key]); // + 1 = round to ceiling
                    else
                        WidthContent = this.GetMaxContentLength(DC.Key);

                    int Width = this.dataGridViewFilter.Columns[DC.Key].Width;

                    switch (Mode)
                    {
                        case FormSetColumnWidth.ColumnWidthMode.Header:
                            Width = WidthHeader;
                            //if(this.dataGridViewFilter.Columns[DC.Key].HeaderText.Length > 0 && (DC.Value.Type() == DataColumn.ColumnType.Data || DC.Value.Type() == DataColumn.ColumnType.Link))
                            //    Width = this.ColumnHeaderWidth(this.dataGridViewFilter.Columns[DC.Key]) + 6; // + 1 = round to ceiling
                            //else
                            //    Width = this.StringLength(DC.Value.DisplayText);
                            break;
                        case FormSetColumnWidth.ColumnWidthMode.Content:
                            Width = WidthContent;
                            //if (this.dataGridViewFilter.Columns[DC.Key].HeaderText.Length > 0 && (DC.Value.Type() == DataColumn.ColumnType.Data || DC.Value.Type() == DataColumn.ColumnType.Link))
                            //    Width = this.GetMaxContentLength(this.dataGridViewFilter.Columns[DC.Key]); // + 1 = round to ceiling
                            //else
                            //    Width = this.GetMaxContentLength(DC.Key);
                            break;
                        case FormSetColumnWidth.ColumnWidthMode.HeaderAndContent:
                            if (WidthContent > WidthHeader)
                                Width = WidthContent;
                            else
                                Width = WidthHeader;
                            //if (this.dataGridViewFilter.Columns[DC.Key].HeaderText.Length > 0 && (DC.Value.Type() == DataColumn.ColumnType.Data || DC.Value.Type() == DataColumn.ColumnType.Link))
                            //    Width = this.GetMaxContentLength(this.dataGridViewFilter.Columns[DC.Key]); // + 1 = round to ceiling
                            //else
                            //    Width = this.GetMaxContentLength(DC.Key);
                            //if (this.StringLength(DC.Value.DisplayText) > Width)
                            //    Width = this.StringLength(DC.Value.DisplayText);
                            break;
                    }
                    if (Width > this._Sheet.MaxColumnWidth())
                        Width = this._Sheet.MaxColumnWidth();
                    this.setColumnWidth(DC.Key, Width);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private int StringLength(string Text)
        {
            SizeF size;
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(new Bitmap(1, 1)))
            {
                size = graphics.MeasureString(Text, new Font("Segoe UI", 9, FontStyle.Regular, GraphicsUnit.Point));
            }
            return (int)size.Width + 3; // short text seem to need more space
        }

        private int ColumnHeaderWidth(System.Windows.Forms.DataGridViewColumn C)
        {
            SizeF size;
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(new Bitmap(1, 1)))
            {
                size = graphics.MeasureString(C.HeaderText, C.DefaultCellStyle.Font);
            }
            int Width = (int)size.Width;
            return Width + this.ColumnWidthAdjustmentFactor(Width); 
        }

        private int GetMaxContentLength(int Column)
        {
            int Width = DiversityWorkbench.Spreadsheet.DataColumn.OperationWidth();
            foreach (System.Data.DataRow R in this._Sheet.DT().Rows)
            {
                if (!R[Column].Equals(System.DBNull.Value) && R[Column].ToString().Length > 0)
                {
                    int Test = this.StringLength(R[Column].ToString());
                    if (Test > Width)
                        Width = Test;
                }
            }
            return Width;
        }

        private int GetMaxContentLength(System.Windows.Forms.DataGridViewColumn C)
        {
            int Width = DiversityWorkbench.Spreadsheet.DataColumn.OperationWidth();
            foreach (System.Data.DataRow R in this._Sheet.DT().Rows)
            {
                SizeF size;
                using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(new Bitmap(1, 1)))
                {
                    if (!R[C.Index].Equals(System.DBNull.Value) && R[C.Index].ToString().Length > 0)
                    {
                        size = graphics.MeasureString(R[C.Index].ToString(), C.DefaultCellStyle.Font);
                        int currentSize = (int)size.Width;
                        if (currentSize > Width)
                            Width = currentSize;
                    }
                }
            }
            return Width + this.ColumnWidthAdjustmentFactor(Width); 
        }

        /// <summary>
        /// Adaption of the column width as the length of the text and the font seem not to be enough to calculate the correct value
        /// </summary>
        /// <param name="Width">Breite der Spalten</param>
        /// <returns>Korrekturfaktor für Spaltenbreite</returns>
        private int ColumnWidthAdjustmentFactor(int Width)
        {
            int KorrekturFaktor = 10 - (int)Width / 9;
            KorrekturFaktor = (int)(KorrekturFaktor * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
            return KorrekturFaktor;
        }

        private void setColumnWidth(int Position, int Width)
        {
            try
            {
                if (this._Sheet.SelectedColumns()[Position].Type() == DataColumn.ColumnType.Operation)
                {
                    Width = DataColumn.OperationWidth();
                }
                else if (this._Sheet.SelectedColumns()[Position].Type() == DataColumn.ColumnType.Spacer)
                {
                    Width = DataColumn.SpacerWidth();
                }
                this.dataGridViewFilter.Columns[Position].Width = Width;
                this.dataGridView.Columns[Position].Width = Width;
                if (this._AddingPossible)
                    this.dataGridViewAdding.Columns[Position].Width = Width;
                if (this._Sheet.SelectedColumns()[Position].IsVisible && this._Sheet.SelectedColumns()[Position].Type() == DataColumn.ColumnType.Data)
                    this._Sheet.SelectedColumns()[Position].Width = Width;

                this.setTableMarker();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        #endregion

        #region Colors
        
        private System.Collections.Generic.Dictionary<string, System.Drawing.Color> _PaleColors;

        private System.Drawing.Color GetPaleColor(string TableName)
        {
            if (this._PaleColors == null)
                this._PaleColors = new Dictionary<string, Color>();
            if (!this._PaleColors.ContainsKey(TableName))
            {
                try
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DataTable> DT in this._Sheet.DataTables())
                    {
                        if (DT.Value.Name == TableName && !this._PaleColors.ContainsKey(TableName))
                            this._PaleColors.Add(TableName, this.paleColor(DT.Value.ColorBack(), 0.5f));
                    }
                }
                catch (System.Exception ex)
                {
                    //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            if (this._PaleColors.ContainsKey(TableName))
                return this._PaleColors[TableName];
            else
                return System.Drawing.Color.White;
        }

        private System.Drawing.Color paleColor(System.Drawing.Color color, float correctionFactor)
        {
            return DiversityWorkbench.Forms.FormFunctions.paleColor(color, correctionFactor);
            //float red = (255 - color.R) * correctionFactor + color.R;
            //float green = (255 - color.G) * correctionFactor + color.G;
            //float blue = (255 - color.B) * correctionFactor + color.B;
            //Color lighterColor = Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
            //return lighterColor;
        }

        public static System.Drawing.Color ColorLookup()
        {
            return DataTable.ColorLookup();
        }

        public static System.Drawing.Color ColorLink()
        {
            return System.Drawing.Color.Blue;
        }

        public static System.Drawing.Color ColorInsertOnly()
        {
            return System.Drawing.Color.Green;
        }

        public static System.Drawing.Color ColorViewOnly()
        {
            return System.Drawing.Color.DarkGreen;
        }

        #endregion

        #region TableMarker
        
        private void setTableMarker()
        {
            try
            {
                System.Collections.Generic.Dictionary<string, int> _TableMarkerWidth = new Dictionary<string, int>();
                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> KV in this._Sheet.SelectedColumns())
                {
                    if (!KV.Value.IsVisible)// || KV.Value.Type() == DataColumn.ColumnType.Operation)
                        continue;
                    if (KV.Value.IsHidden)// || KV.Value.Type() == DataColumn.ColumnType.Operation)
                        continue;
                    if (KV.Value.Type() == DataColumn.ColumnType.Spacer)
                    {
                        if (_TableMarkerWidth.ContainsKey(KV.Value.DataTable().Alias()))
                            _TableMarkerWidth[KV.Value.DataTable().Alias()] += DataColumn.SpacerWidth();
                        else
                            _TableMarkerWidth.Add(KV.Value.DataTable().Alias(), DataColumn.SpacerWidth());
                    }
                    else if (_TableMarkerWidth.ContainsKey(KV.Value.DataTable().Alias()))
                        _TableMarkerWidth[KV.Value.DataTable().Alias()] += KV.Value.Width;
                    else
                        _TableMarkerWidth.Add(KV.Value.DataTable().Alias(), KV.Value.Width);// + 10);
                }
                foreach (System.Windows.Forms.Control C in this.panelTableMarker.Controls)
                {
                    foreach (System.Windows.Forms.Control CC in C.Controls)
                    {
                        CC.Dispose();
                    }
                    C.Controls.Clear();
                    C.Dispose();
                }
                this.panelTableMarker.Controls.Clear();
                int iMinWidth = 7 
                    + this.buttonReadOnly.Width 
                    + Spreadsheet.DataColumn.SpacerWidth() 
                    + this.panelTableMarker.Margin.Left
                    + this.panelTableMarker.Margin.Right
                    + 7;
                this.panelTableMarker.SuspendLayout();
                foreach (System.Collections.Generic.KeyValuePair<string, int> KV in _TableMarkerWidth)
                {
                    System.Windows.Forms.Panel P = new Panel();
                    System.Windows.Forms.Button B = new Button();

                    if (this._Sheet.DataTables()[KV.Key].Type() == DataTable.TableType.Lookup)
                        B.BackColor = this._Sheet.DataTables()[KV.Key].paleColor();
                    else
                        B.BackColor = this._Sheet.DataTables()[KV.Key].ColorBack();

                    //B.BackColor = this._Sheet.DataTables()[KV.Key].ColorBack();
                    B.ForeColor = this._Sheet.DataTables()[KV.Key].ColorFont();
                    if (this._ImageColumn != null && this._Sheet.DataTables()[KV.Key].Alias() == this._ImageColumn.DataTable().Alias())
                    {
                        System.Drawing.Font F = new System.Drawing.Font(B.Font, FontStyle.Italic);
                        B.ForeColor = System.Drawing.Color.Blue;
                        B.Font = F;
                    }
                    B.Text = this._Sheet.DataTables()[KV.Key].DisplayText;
                    B.TextAlign = ContentAlignment.MiddleLeft;
                    this.toolTip.SetToolTip(B, this._Sheet.DataTables()[KV.Key].DisplayText);
                    B.FlatStyle = FlatStyle.Flat;
                    B.FlatAppearance.BorderSize = 0;
                    if(this._Sheet.DataTables()[KV.Key].Description() != this._Sheet.DataTables()[KV.Key].DisplayText 
                        && this._Sheet.DataTables()[KV.Key].Description() != this._Sheet.DataTables()[KV.Key].Name)
                        toolTip.SetToolTip(B, this._Sheet.DataTables()[KV.Key].Description());
                    else
                        toolTip.SetToolTip(B, "Edit columns of this table");
                    B.Tag = KV.Key;
                    P.Controls.Add(B);
                    P.BorderStyle = BorderStyle.FixedSingle;
                    if (this._Sheet.DataTables()[KV.Key].Type() == DataTable.TableType.Parallel
                        || this._Sheet.DataTables()[KV.Key].Type() == DataTable.TableType.Lookup
                        || this._Sheet.DataTables()[KV.Key].Type() == DataTable.TableType.Single
                        || this._Sheet.DataTables()[KV.Key].Type() == DataTable.TableType.InsertOnly)
                    {
                        bool HasDependentTables = false;
                        foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                        {
                            if (DC.Value.DataTable().ParentTable() != null && DC.Value.DataTable().ParentTable().Alias() == KV.Key)
                            {
                                HasDependentTables = true;
                                break;
                            }
                        }
                        if (!HasDependentTables && !this._Sheet.DataTables()[KV.Key].IsRequired)
                        {
                            System.Windows.Forms.Button Bdel = new Button();

                            if (this._Sheet.DataTables()[KV.Key].Type() == DataTable.TableType.Lookup)
                                Bdel.BackColor = this._Sheet.DataTables()[KV.Key].paleColor();
                            else
                                Bdel.BackColor = this._Sheet.DataTables()[KV.Key].ColorBack();

                            //Bdel.BackColor = this._Sheet.DataTables()[KV.Key].ColorBack();
                            Bdel.Image = DiversityWorkbench.Properties.Resources.Delete;
                            Bdel.ImageAlign = ContentAlignment.MiddleCenter;
                            Bdel.FlatStyle = FlatStyle.Flat;
                            Bdel.FlatAppearance.BorderSize = 0;
                            Bdel.Dock = DockStyle.Right;
                            Bdel.Width = 18;
                            P.Controls.Add(Bdel);
                            Bdel.BringToFront();
                            Bdel.Tag = KV.Key;
                            this.toolTip.SetToolTip(Bdel, "Remove this table");
                            Bdel.Click += new System.EventHandler(this.buttonTableRemove_Click);
                        }
                    }
                    B.Dock = DockStyle.Fill;
                    this.panelTableMarker.Controls.Add(P);//B);
                    P.Dock = DockStyle.Left;
                    P.Width = KV.Value;
                    // Cutting the text if button is too small
                    if (B.Text.Length * 4 > B.Width)
                    {
                        SizeF Size = B.CreateGraphics().MeasureString(B.Text, B.Font);
                        if (B.Width < Size.Width)
                        {
                            int Stop = (int)(B.Text.Length * (B.Width / Size.Width));
                            B.Text = B.Text.Substring(0, Stop);
                        }
                    }
                    P.BringToFront();
                    if (this._Sheet.DataTables()[KV.Key].TableImage() != null)
                    {
                        B.Image = this._Sheet.DataTables()[KV.Key].TableImage();
                        B.ImageAlign = ContentAlignment.MiddleLeft;
                        B.TextAlign = ContentAlignment.MiddleLeft;
                        B.Text = "      " + this._Sheet.DataTables()[KV.Key].DisplayText;
                    }
                    else
                    {
                        B.TextAlign = ContentAlignment.MiddleLeft;
                    }
                    B.Click += new System.EventHandler(this.buttonTableMarker_Click);
                    iMinWidth += P.Width;
                }
                if (this.tableLayoutPanelGrids.Width < iMinWidth)
                    this.tableLayoutPanelGrids.Width = iMinWidth;
                if (!this.panelTableMarkerScroll.HorizontalScroll.Visible)
                {
                    System.Drawing.Point P = new Point(0, 0);
                    this.panelTableMarker.Location = P;
                }
                this.panelTableMarker.ResumeLayout();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonTableMarker_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                System.Windows.Forms.Button B = (System.Windows.Forms.Button)sender;
                FormTableSettingsSingle f = new FormTableSettingsSingle(ref this._Sheet, B.Tag.ToString(), this.helpProvider.HelpNamespace, false, true);
                this.Cursor = Cursors.Default;
                f.ShowDialog();
                if (Sheet.RebuildNeeded && f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    this.RequeryAllGrids();
                }
                f.Dispose();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void buttonTableRemove_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Button B = (System.Windows.Forms.Button)sender;
                string TableAlias = B.Tag.ToString();
                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                {
                    if (DC.Value.DataTable().Alias() == TableAlias)
                        DC.Value.IsVisible = false;
                }
                this._Sheet.ResetSelectedColumns();
                this.RequeryAllGrids();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Obsolet

        private void buttonFind_Click(object sender, EventArgs e)
        {
            getData();
        }
        
        #endregion

        #region Data

        #region dataGridView Events

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (this._Sheet.ReadOnly)
                return;

            try
            {
                if (e.ColumnIndex > -1)
                {
                    if (this._Sheet.SelectedColumns()[e.ColumnIndex].LookupSource != null)
                    {
                        if (this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Length > 0)
                        {
                            this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = this._Sheet.LookupDictionary(e.ColumnIndex)[this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()];
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void dataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                // set cell for setting the fixed source as a marker for the start of the action
                //if (e.Button == System.Windows.Forms.MouseButtons.Left) // not context menu
                //    this.setSelectedDataCell(e.ColumnIndex, e.RowIndex);
            }
            catch (System.Exception ex)
            {
            }

        }

        private void dataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (this._MarkColumnRange)
                    return;
                if (e.Button == System.Windows.Forms.MouseButtons.Left) // not context menu
                {
                    // Markus 21.7.23: do not change ReadOnly columns
                    if (this._Sheet.SelectedColumns()[e.ColumnIndex].IsReadOnly())
                        return;
                    // Setting the cell and all dependent values
                    if (this.FixedSourceSetCellSelected(e.ColumnIndex, e.RowIndex))// ._SelectedDataCell != null) // a start is available
                    {
                        // for linked columns either show values from remote source or set value via romte source
                        if (this._Sheet.SelectedColumns()[e.ColumnIndex].IsLinkedColumn())
                        {
                            // direktes Setzen nur fuer einzelne Zellen
                            string Error = "";
                            if (this.dataGridView.SelectedCells.Count == 1)
                            {
                                System.Collections.Generic.List<string> MissingColumns = new List<string>();
                                if (this.dataGridView.SelectedCells[0].Value != null && this.dataGridView.SelectedCells[0].Value.ToString().Length > 0)
                                {
                                    // cell with a available link
                                    this._Sheet.setLinkedColumnValues(this.dataGridView.SelectedCells[0], this.FixedSourceIWorkbenchUnit(), Sheet.Grid.Data, ref Error);
                                    // if columns linked to the linkcolumn are missing, block the editing
                                    if (!this.FixedSourceLinkedColumnsIncluded(e.ColumnIndex, ref MissingColumns))
                                        this.EditingEnableEditing(-1);
                                }
                                else // no value, so set value
                                {
                                    if (!this.FixedSourceLinkedColumnsIncluded(e.ColumnIndex, ref MissingColumns))
                                    {
                                        string DisplayColumn = this._Sheet.SelectedColumns()[e.ColumnIndex].RemoteLinkDisplayColumn;
                                        string Message = "To use the setting via a link to a remote source\r\n" +
                                            "please include the following columns:\r\n";
                                        foreach (string C in MissingColumns)
                                            Message += "\r\n" + C;
                                        System.Windows.Forms.MessageBox.Show(Message, "Missing columns", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        this._FixedSourceIWorkbenchUnit = null;
                                        this._FixedSourceServerConnection = null;
                                        this._FixedSourceWebservice = DwbServiceEnums.DwbService.None;
                                        this._FixedSourceSQL = null;
                                        this.EditingEnableEditing(-1);
                                        return;
                                    }

                                    if (this._FixedSourceIWorkbenchUnit == null)
                                        this._FixedSourceIWorkbenchUnit = this.FixedSourceIWorkbenchUnit();
                                    if (this._FixedSourceSetting == null)
                                    {
                                        string Value = "";
                                        if (this._Sheet.SelectedColumns()[e.ColumnIndex].FixedSourceNeedsValueFromData())
                                        {
                                            Value = this.FixedSourceGetValueFromData(this._Sheet.SelectedColumns()[e.ColumnIndex].FixedSourceGetSetting(), e.ColumnIndex, e.RowIndex);
                                        }
                                        this._FixedSourceSetting = this._Sheet.SelectedColumns()[e.ColumnIndex].FixedSourceGetSetting(Value);
                                    }
                                    if (this._Sheet.SelectedColumns()[e.ColumnIndex].FixedSourceNeedsValueFromData())
                                    {
                                        if (this._FixedSourceSetting.Last().IndexOf(".") > -1)
                                        {
                                            string Value = this.FixedSourceGetValueFromData(this._FixedSourceSetting, e.ColumnIndex, e.RowIndex);
                                            this._FixedSourceSetting[this._FixedSourceSetting.Count - 1] = Value;
                                        }
                                        else
                                        {
                                            // getting the value from the data (i.e. the spreadsheet, e.g. reading the content of the column TaxonomicGroup)
                                            string Alias = this._Sheet.SelectedColumns()[e.ColumnIndex].FixedSourceValueFromDataTableAlias;
                                            string Column = this._Sheet.SelectedColumns()[e.ColumnIndex].FixedSourceValueFromDataColumnName;
                                            if (Alias.Length > 0 && Column.Length > 0)
                                            {
                                                string Value = this.FixedSourceGetValueFromData(Alias, Column, e.RowIndex);
                                                // if the value has changed in comparision to the last query
                                                if (Value != this._FixedSourceSetting[this._FixedSourceSetting.Count - 1])
                                                {
                                                    this._FixedSourceSetting[this._FixedSourceSetting.Count - 1] = Value;
                                                    this._FixedSourceServerConnection = null;
                                                }
                                            }
                                        }
                                    }
                                    this._Sheet.setLinkedColumnValues(this._FixedSourceSetting, this._FixedSourceServerConnection, this._FixedSourceWebservice, this._FixedSourceIWorkbenchUnit, this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex], Sheet.Grid.Data, ref Error);
                                    // this._Sheet.setLinkedColumnValues(this._FixedSourceSetting, this._FixedSourceServerConnection, this._FixedSourceIWorkbenchUnit, this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex], Sheet.Grid.Data, ref Error);
                                    this._FixedSourceServerConnection = this._Sheet.FixedSourceServerConnection();
                                    this._FixedSourceWebservice = this._Sheet.FixedSourceWebservice();
                                    this.FixedSourceSetControls();
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this._Sheet.ReadOnly)
                return;
            this.dataGridView.SuspendLayout();
            try
            {
                if (e.ColumnIndex == -1)
                {
                }
                else if (this._Sheet.SelectedColumns()[e.ColumnIndex].IsVisible)// .DisplayedColumns().ContainsKey(e.ColumnIndex))
                {
                    string Alias = this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Alias();
                    if (this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Type() != DataTable.TableType.Root &&
                        //this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Type() != DataTable.TableType.RootHidden &&
                        this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Type() != DataTable.TableType.Target)
                        Alias = this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().ParentTable().Alias();
                    // Operation columns
                    if (this._Sheet.SelectedColumns()[e.ColumnIndex].Type() == DataColumn.ColumnType.Operation)
                    {
                        if (this._Sheet.getTableReadOnlyRows(this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Alias()).Contains(e.RowIndex))
                        {
                            if (this._Sheet.getTableWhiteRows(Alias).Contains(e.RowIndex))
                            {
                                e.CellStyle.ForeColor = this.GetPaleColor(this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Name);
                                e.CellStyle.BackColor = this.GetPaleColor(this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Name);

                                e.CellStyle.SelectionForeColor = this.GetPaleColor(this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Name);
                                e.CellStyle.SelectionBackColor = this.GetPaleColor(this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Name);
                            }
                            else
                            {
                                e.CellStyle.ForeColor = this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().ColorBack();
                                e.CellStyle.BackColor = this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().ColorBack();

                                e.CellStyle.SelectionForeColor = this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().ColorBack();
                                e.CellStyle.SelectionBackColor = this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().ColorBack();
                            }
                        }
                        else
                        {
                            try
                            {
                                if (this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value.Equals(System.DBNull.Value))
                                {
                                    e.CellStyle.BackColor = this._ColorNull;
                                    e.CellStyle.SelectionBackColor = this._ColorNull;
                                    e.CellStyle.ForeColor = System.Drawing.Color.White;
                                    e.CellStyle.SelectionForeColor = System.Drawing.Color.White;
                                }
                                else
                                {
                                    if (this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Type() == DataTable.TableType.Lookup)
                                    {
                                        e.CellStyle.BackColor = this._ColorNull;
                                        e.CellStyle.SelectionBackColor = this._ColorNull;
                                    }
                                    else
                                    {
                                        e.CellStyle.BackColor = System.Drawing.Color.Red;
                                        e.CellStyle.SelectionBackColor = System.Drawing.Color.Red;
                                    }
                                    e.CellStyle.ForeColor = System.Drawing.Color.White;
                                    e.CellStyle.SelectionForeColor = System.Drawing.Color.White;
                                    this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = "Delete data from " + this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().DisplayText; ;
                                }
                            }
                            catch (System.Exception ex)
                            {
                                e.CellStyle.BackColor = System.Drawing.Color.Red;
                                e.CellStyle.SelectionBackColor = System.Drawing.Color.Red;
                            }
                        }
                    }
                    // Spacer columns
                    else if (this._Sheet.SelectedColumns()[e.ColumnIndex].Type() == DataColumn.ColumnType.Spacer)
                    {
                        e.CellStyle.BackColor = System.Drawing.SystemColors.ControlDark;
                        e.CellStyle.SelectionBackColor = System.Drawing.SystemColors.ControlDark;
                    }
                    // data columns
                    else if (this._Sheet.SelectedColumns()[e.ColumnIndex].Type() == DataColumn.ColumnType.Link &&
                        !this._Sheet.getTableReadOnlyRows(this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Alias()).Contains(e.RowIndex))
                    {
                        e.CellStyle.ForeColor = System.Drawing.Color.White;
                        e.CellStyle.BackColor = ColorLink();// System.Drawing.Color.Blue;
                        e.CellStyle.SelectionForeColor = System.Drawing.Color.White;
                        e.CellStyle.SelectionBackColor = ColorLink();// System.Drawing.Color.Blue;
                    }
                    else
                    {
                        if (this._Sheet.getTableReadOnlyRows(Alias).Contains(e.RowIndex))
                        {
                            this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
                            System.Collections.Generic.List<int> L = this._Sheet.getTableWhiteRows(Alias);
                            if (this._Sheet.getTableWhiteRows(Alias).Contains(e.RowIndex))
                            {
                                e.CellStyle.ForeColor = this.GetPaleColor(this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Name);
                                e.CellStyle.BackColor = this.GetPaleColor(this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Name);

                                e.CellStyle.SelectionForeColor = this.GetPaleColor(this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Name);
                                e.CellStyle.SelectionBackColor = this.GetPaleColor(this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Name);
                            }
                            else
                            {
                                e.CellStyle.ForeColor = this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().ColorBack();
                                e.CellStyle.BackColor = this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().ColorBack();

                                e.CellStyle.SelectionForeColor = this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().ColorBack();
                                e.CellStyle.SelectionBackColor = this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().ColorBack();
                            }
                        }
                        else if (this._Sheet.getTableNullRows(this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Alias()).Contains(e.RowIndex))
                        {
                            e.CellStyle.BackColor = this._ColorNull;
                        }
                        else
                        {
                            if (this._Sheet.SelectedColumns()[e.ColumnIndex].TypeOfLink == DataColumn.LinkType.OptionalLinkToDiversityWorkbenchModule)
                            {
                                if (this._Sheet.SelectedColumns()[e.ColumnIndex].IsLinkColumn == null)
                                {
                                    string DecisionColumn = this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().DataColumns()[this._Sheet.SelectedColumns()[e.ColumnIndex].RemoteLinkDecisionColmn].DisplayText;
                                    string DecisionValue = this._Sheet.DT().Rows[e.RowIndex][DecisionColumn].ToString();
                                    this._Sheet.SelectedColumns()[e.ColumnIndex].SetIsLinkColumn(false);
                                    foreach (DiversityWorkbench.Spreadsheet.RemoteLink RL in this._Sheet.SelectedColumns()[e.ColumnIndex].RemoteLinks)
                                    {
                                        if (RL.DecisionColumnValues.Contains(DecisionValue))
                                        {
                                            this._Sheet.SelectedColumns()[e.ColumnIndex].SetIsLinkColumn(true);
                                            break;
                                        }
                                    }
                                }
                            }
                            if (this._Sheet.SelectedColumns()[e.ColumnIndex].IsLinkedColumn())
                            {
                                System.Drawing.Font F = new System.Drawing.Font(e.CellStyle.Font, FontStyle.Underline);
                                e.CellStyle.ForeColor = ColorLink();// System.Drawing.Color.Blue;
                                e.CellStyle.Font = F;
                            }
                            else if (this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Type() == DataTable.TableType.Lookup)
                                e.CellStyle.ForeColor = ColorLookup();
                            else if (this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().ColorFont() != System.Drawing.Color.Black)
                            {
                                e.CellStyle.ForeColor = this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().ColorFont();
                            }

                            if (this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().ColorBack() != System.Drawing.Color.White)
                            {
                                if (!this._Sheet.getTableWhiteRows(Alias).Contains(e.RowIndex))
                                    e.CellStyle.BackColor = this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().ColorBack();
                                else
                                {
                                    e.CellStyle.BackColor = this.GetPaleColor(this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Name);
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.dataGridView.ResumeLayout();
        }

        private void dataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    this.HandleDataGridData(this.dataGridView.SelectedCells[0].ColumnIndex, this.dataGridView.SelectedCells[0].RowIndex);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dataGridView.SelectedCells.Count > 1)
            {
                if (this._FixedSourceSelectedCell != null)
                {
                    foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.SelectedCells)
                    {
                        if (C.ColumnIndex != this._FixedSourceSelectedCell.ColumnIndex)
                        {
                            this.EditingEnableEditing(-1);
                            break;
                        }
                    }
                }
            }
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.useThisRowAsTemplateForNewRowsToolStripMenuItem.Enabled = false;
            this.copyValuesToClipboardToolStripMenuItem.Enabled = false;
            this.transferValuesToClipboardToolStripMenuItem.Enabled = false;
            this.insertValesFromClipboardToolStripMenuItem.Enabled = false;
            this.toolStripMenuItemRemove.Enabled = false;

            // kommt in MouseUp
            // Prepare index for setting the fixed source
            // this.setEditingControlsAndFixedSourceCell(e.ColumnIndex, e.RowIndex);
            //this._FixSourceColumnIndex = e.ColumnIndex; 

            // not action if grid is set on read only
            if (this._Sheet.ReadOnly)
            {
                return;
            }

            // setting the state for Update
            this.SettingUpdateStateForData(e.ColumnIndex, e.RowIndex);
            //if (e.ColumnIndex > -1 && 
            //    (
            //    this._Sheet.SelectedColumns()[e.ColumnIndex].Type() == DataColumn.ColumnType.Link || 
            //    this._Sheet.SelectedColumns()[e.ColumnIndex].Type() == DataColumn.ColumnType.Data
            //    )
            //    )
            //{
            //    // setting the Readonly state
            //    if (this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().AllowUpdate())
            //    {
            //        if (this._Sheet.SelectedColumns()[e.ColumnIndex].IsReadOnly() || this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Type() == DataTable.TableType.Lookup)
            //            this.dataGridView.Columns[e.ColumnIndex].ReadOnly = true;
            //        else if (this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().PrimaryKeyColumnList.Contains(this._Sheet.SelectedColumns()[e.ColumnIndex].Name))
            //        {
            //            if (this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != System.DBNull.Value ||
            //                this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Length > 0)
            //                this.dataGridView.Columns[e.ColumnIndex].ReadOnly = true;
            //        }
            //        else if (e.ColumnIndex > -1 && !this._Sheet.getTableReadOnlyRows(this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Alias()).Contains(e.RowIndex))
            //        {
            //            if (this._Sheet.SelectedColumns()[e.ColumnIndex].LinkedToColumn != null && this._Sheet.SelectedColumns()[e.ColumnIndex].LinkedToColumn != null)
            //            {
            //                if (this._Sheet.DT().Rows[e.RowIndex][this._Sheet.SelectedColumns()[e.ColumnIndex].LinkedToColumn.DisplayText].Equals(System.DBNull.Value))
            //                    this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = false;
            //                else
            //                    this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
            //            }
            //            // Action for not read only cell
            //            //this.dataGridViewDataCell_Click(dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex], Sheet.Grid.Data);
            //        }
            //        else if (e.ColumnIndex > -1)
            //        {
            //            this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
            //        }
            //    }
            //    else
            //    {
            //        this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
            //    }
            //    this.EnableEditing(e.ColumnIndex);
            //}

            if (this.MarkColumnRange)
            {
                this.MarkRange(e.ColumnIndex);

                //System.Collections.Generic.List<int> CC = new List<int>();
                //foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.SelectedCells)
                //{
                //    if (this._MarkedColumnIndex != null && C.ColumnIndex != this._MarkedColumnIndex)
                //    {
                //        System.Windows.Forms.MessageBox.Show("Only one column can be selected");
                //        C.Selected = false;
                //        this.MarkColumnRange = false;
                //        break;
                //    }
                //    if (!CC.Contains(C.ColumnIndex))
                //        CC.Add(C.ColumnIndex);
                //    if (CC.Count > 1)
                //    {
                //        System.Windows.Forms.MessageBox.Show("Only one column can be selected");
                //        C.Selected = false;
                //        this.MarkColumnRange = false;
                //        break;
                //    }
                //}
                //if (CC.Count > 1)
                //{
                //    foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.SelectedCells)
                //    {
                //        if (C.ColumnIndex != CC[0])
                //            C.Selected = false;
                //    }
                //}
                //else
                //{
                //    if (e.ColumnIndex > -1 && this._Sheet.SelectedColumns()[e.ColumnIndex].Type() != DataColumn.ColumnType.Spacer &&
                //        e.ColumnIndex > -1 && this._Sheet.SelectedColumns()[e.ColumnIndex].Type() != DataColumn.ColumnType.Operation &&
                //        this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().AllowDelete())
                //    {
                //        if (this.dataGridView.SelectedCells.Count > 1)
                //        {
                //            this.copyValuesToClipboardToolStripMenuItem.Enabled = true;
                //            this.transferValuesToClipboardToolStripMenuItem.Enabled = true;
                //            this.toolStripMenuItemRemove.Enabled = true;
                //        }
                //        else
                //        {
                //            this.copyValuesToClipboardToolStripMenuItem.Enabled = false;
                //            this.transferValuesToClipboardToolStripMenuItem.Enabled = false;
                //            if (this.dataGridView.SelectedCells.Count > 0)
                //                this.toolStripMenuItemRemove.Enabled = true;
                //            else
                //                this.toolStripMenuItemRemove.Enabled = false;
                //        }
                //    }
                //}

                return;
            }

            if (e.ColumnIndex > -1 && this._Sheet.SelectedColumns()[e.ColumnIndex].Type() == DataColumn.ColumnType.Operation)
            {
                if (!this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().AllowDelete())
                {
                    System.Windows.Forms.MessageBox.Show("You have no permission to delete data from table " + this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Name);
                    return;
                }
                if (this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Type() == DataTable.TableType.Lookup)
                    return;
                if (!this._Sheet.getTableReadOnlyRows(this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Alias()).Contains(e.RowIndex) &&
                    !this._Sheet.getTableNullRows(this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Alias()).Contains(e.RowIndex)) // this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor != this._ColorNull)
                {
                    this.DeleteData(e.ColumnIndex, e.RowIndex);

                    //try
                    //{
                    //    System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> DependetData = new Dictionary<string, List<int>>();
                    //    string Message = "Do you really want to delete these data from table " + this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().DisplayText;
                    //    if (System.Windows.Forms.MessageBox.Show(Message, "Delete data", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    //        return;

                    //    System.Collections.Generic.List<int> Rows = new List<int>();
                    //    Rows.Add(e.RowIndex);
                    //    int StartIndex = e.RowIndex;
                    //    StartIndex++;
                    //    while (this._Sheet.getTableReadOnlyRows(this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Alias()).Contains(StartIndex))
                    //    {
                    //        Rows.Add(StartIndex);
                    //        StartIndex++;
                    //    }

                    //    string Alias = this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Alias();

                    //    // getting dependent tables
                    //    System.Collections.Generic.Dictionary<string, string> pk = this.PrimaryKey(e.RowIndex, Alias);
                    //    System.Collections.Generic.Dictionary<string, System.Data.DataTable> Tables = new Dictionary<string, System.Data.DataTable>();
                    //    this.FindDependentData(Alias, pk, ref Tables);
                    //    if (Tables.Count > 0)
                    //    {
                    //        FormDependentData fdd = new FormDependentData("Dependent data will be deleted as well. OK?", Tables);
                    //        fdd.ShowDialog();
                    //        if (fdd.DialogResult != System.Windows.Forms.DialogResult.OK)
                    //        {
                    //            return;
                    //        }
                    //        // Changing sequence of tables for delete (most depending data should be removed first)
                    //        System.Collections.Generic.SortedList<int, string> DependingTables = new SortedList<int, string>();
                    //        foreach (System.Collections.Generic.KeyValuePair<string, System.Data.DataTable> DT in Tables)
                    //        {
                    //            DependingTables.Add(Tables.Count - DependingTables.Count, DT.Key);
                    //        }
                    //        foreach (System.Collections.Generic.KeyValuePair<int, string> DT in DependingTables)
                    //        {
                    //            string SQL = "DELETE T FROM [" + DT.Value + "] AS T WHERE 1 = 1 ";
                    //            foreach (System.Collections.Generic.KeyValuePair<string, string> K in pk)
                    //                SQL += " AND [" + K.Key + "] = '" + K.Value + "' ";
                    //            Message = "";
                    //            if (!DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message))
                    //            {
                    //                System.Windows.Forms.MessageBox.Show(Message);
                    //                return;
                    //            }
                    //        }
                    //    }
                    //    // Getting the PK of the parent
                    //    System.Collections.Generic.Dictionary<string, string> PkParent = new Dictionary<string, string>();
                    //    if (this._Sheet.DataTables()[this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Alias()].ParentTable() != null)
                    //    {
                    //        DiversityWorkbench.Spreadsheet.DataTable PT = this._Sheet.DataTables()[this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Alias()].ParentTable();
                    //        foreach (string PK in PT.PrimaryKeyColumnList)
                    //        {
                    //            if (PT.Type() == DataTable.TableType.Project)
                    //            {
                    //                if (PK == "ProjectID")
                    //                    PkParent.Add(PK, this._Sheet.ProjectID().ToString());
                    //                else
                    //                    PkParent.Add(PK, this._Sheet.DT().Rows[e.RowIndex][PT.DataColumns()[PK].Name].ToString());
                    //            }
                    //            else
                    //            {
                    //                string TableAlias = this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Alias();
                    //                string Column = this._Sheet.DataTables()[TableAlias].DataColumns()[PK].DisplayText;
                    //                try
                    //                {
                    //                    PkParent.Add(PK, this._Sheet.DT().Rows[e.RowIndex][Column].ToString());
                    //                }
                    //                catch (System.Exception ex)
                    //                {
                    //                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    //                }
                    //            }
                    //        }
                    //    }
                    //    if (this._Sheet.DataTables()[this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Alias()].DeleteData(e.RowIndex, ref Message))
                    //    {
                    //        System.Windows.Forms.MessageBox.Show("Data deleted");
                    //        this.getData();
                    //        // try to find the parent
                    //        if (PkParent.Count > 0)
                    //        {
                    //            for (int i = 0; i < this._Sheet.DT().Rows.Count; i++)
                    //            {
                    //                bool RowFound = true;
                    //                foreach (System.Data.DataColumn C in this._Sheet.DT().Columns)
                    //                {
                    //                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in PkParent)
                    //                    {
                    //                        if (KV.Key == C.ColumnName && KV.Value != this._Sheet.DT().Rows[i][C].ToString())
                    //                        {
                    //                            RowFound = false;
                    //                            break;
                    //                        }
                    //                    }
                    //                }
                    //                if (RowFound)
                    //                {
                    //                    this.dataGridView.Rows[i].Selected = true;
                    //                    break;
                    //                }
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (Message.Length > 0) Message = "Deleting failed:\r\n" + Message;
                    //        else Message = "Deleting failed";
                    //        System.Windows.Forms.MessageBox.Show(Message);
                    //    }
                    //}
                    //catch (System.Exception ex)
                    //{
                    //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    //}
                }
            }
            else if (e.ColumnIndex > -1 && this._Sheet.SelectedColumns()[e.ColumnIndex].Type() == DataColumn.ColumnType.Spacer)
            {
                // No handling of spacer
            }
            else // Data columns
            {
                this.useThisRowAsTemplateForNewRowsToolStripMenuItem.Enabled = false;
                if (this.dataGridView.SelectedCells.Count > 1)
                {
                    this.copyValuesToClipboardToolStripMenuItem.Enabled = true;
                    this.transferValuesToClipboardToolStripMenuItem.Enabled = true;
                    this.toolStripMenuItemRemove.Enabled = true;
                }
                else
                {
                    this.copyValuesToClipboardToolStripMenuItem.Enabled = false;
                    this.transferValuesToClipboardToolStripMenuItem.Enabled = false;
                    if (this.dataGridView.SelectedCells.Count > 0)
                        this.toolStripMenuItemRemove.Enabled = true;
                    else
                        this.toolStripMenuItemRemove.Enabled = false;
                }
                this.insertValesFromClipboardToolStripMenuItem.Enabled = true;
                //if (this.dataGridView.SelectedCells.Count == 1)
                //    this.toolStripMenuItemRemove.Enabled = true;
                //else
                //    this.toolStripMenuItemRemove.Enabled = false;

                // Fixing the source
                // nach MouseDown verschoben
                // this.setFixedSource(e.ColumnIndex, e.RowIndex);

                // setting the image
                if (this._ImageColumn != null && this._Sheet.DT().Columns.Contains(this._ImageColumn.DisplayText))
                {
                    try
                    {
                        if (this._Sheet.DT().Columns.Contains(this._ImageColumn.DisplayText))
                        {
                            System.Data.DataRow R = this._Sheet.DT().Rows[e.RowIndex];
                            string URI = R[this._ImageColumn.DisplayText].ToString(); // this._Sheet.DT().Rows[e.RowIndex][this._ImageColumn.DisplayText].ToString();
                            this.setImage(URI);
                        }
                        else
                        {
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }

                if (e.ColumnIndex > -1)
                {
                    //// setting the Readonly state - An Anfang verschoben
                    //if (this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().AllowUpdate())
                    //{
                    //    if (this._Sheet.SelectedColumns()[e.ColumnIndex].IsReadOnly() || this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Type() == DataTable.TableType.Lookup)
                    //        this.dataGridView.Columns[e.ColumnIndex].ReadOnly = true;
                    //    else if (this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().PrimaryKeyColumnList.Contains(this._Sheet.SelectedColumns()[e.ColumnIndex].Name))
                    //    {
                    //        if (this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != System.DBNull.Value ||
                    //            this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Length > 0)
                    //            this.dataGridView.Columns[e.ColumnIndex].ReadOnly = true;
                    //    }
                    //    else if (e.ColumnIndex > -1 && !this._Sheet.getTableReadOnlyRows(this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Alias()).Contains(e.RowIndex))
                    //    {
                    //        if (this._Sheet.SelectedColumns()[e.ColumnIndex].LinkedToColumn != null && this._Sheet.SelectedColumns()[e.ColumnIndex].LinkedToColumn != null)
                    //        {
                    //            if (this._Sheet.DT().Rows[e.RowIndex][this._Sheet.SelectedColumns()[e.ColumnIndex].LinkedToColumn.DisplayText].Equals(System.DBNull.Value))
                    //                this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = false;
                    //            else
                    //                this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
                    //        }
                    //        // Action for not read only cell
                    //        //this.dataGridViewDataCell_Click(dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex], Sheet.Grid.Data);
                    //    }
                    //    else if (e.ColumnIndex > -1)
                    //    {
                    //        this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
                    //    }
                    //}
                    //else
                    //{
                    //    this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
                    //}

                    // performing potential actions
                    if (this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().AllowUpdate())
                    {
                        bool DoAct = false;
                        if (this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().PrimaryKeyColumnList.Contains(this._Sheet.SelectedColumns()[e.ColumnIndex].Name)
                            && this._Sheet.DT().Rows[e.RowIndex][e.ColumnIndex].Equals(System.DBNull.Value)
                            && this._Sheet.SelectedColumns()[e.ColumnIndex].LookupSource != null)
                            DoAct = true;
                        if (e.ColumnIndex > -1 && !this._Sheet.getTableReadOnlyRows(this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Alias()).Contains(e.RowIndex))
                            DoAct = true;
                        if (this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Type() == DataTable.TableType.Lookup)
                            DoAct = false;
                        if (DoAct)
                            this.dataGridViewDataCell_Click(dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex], Sheet.Grid.Data);
                    }
                    bool AllowEdit = this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().AllowUpdate();
                    bool AllowChange = AllowEdit;
                    if (AllowEdit)
                    {
                        if (this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Type() == DataTable.TableType.Lookup
                            || this._Sheet.SelectedColumns()[e.ColumnIndex].IsIdentity
                            || this._Sheet.SelectedColumns()[e.ColumnIndex].IsReadOnly()
                            || this._Sheet.SelectedColumns()[e.ColumnIndex].Column.DataType == "geography"
                            || this._Sheet.SelectedColumns()[e.ColumnIndex].Column.DataType == "datetime")
                        {
                            AllowEdit = false;
                            AllowChange = AllowEdit;
                        }
                        if (((this._Sheet.SelectedColumns()[e.ColumnIndex].IsLinkedColumn())
                            || this._Sheet.SelectedColumns()[e.ColumnIndex].LookupSource != null)
                            && (this._EditMode == EditMode.Append
                            || this._EditMode == EditMode.Calculate
                            || this._EditMode == EditMode.Prepend))
                        {
                            AllowEdit = false;
                            AllowChange = true;
                        }
                    }
                    // An Anfang verschoben
                    //this.EnableEditing();//AllowEdit, AllowChange);
                }
            }
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this._Sheet.ReadOnly)
                return;

            this.MarkColumnRangeReset();

            if (!this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().AllowUpdate())
            {
                System.Windows.Forms.MessageBox.Show("You have no permission to change data in table " + this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Name);
                this._Sheet.DT().Rows[e.RowIndex].CancelEdit();
                return;
            }

            // No insert for InsertOnly tables in nothing is inserted
            if (this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Type() == DataTable.TableType.InsertOnly
                && this.dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Length == 0)
            {
                return;
            }

            System.Data.DataRow R = this._Sheet.DT().Rows[e.RowIndex];
            string Message = "";
            DataTable.SqlCommandTypeExecuted SqlType = DataTable.SqlCommandTypeExecuted.None;
            this.toolTip.SetToolTip(this.buttonSaveData, "");

            if (!this._Sheet.SaveData(e.RowIndex, this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Alias(), ref Message, ref SqlType))
            {
                if (Message.Length > 0)
                {
                    System.Windows.Forms.MessageBox.Show("Update failed:\r\n" + Message, "Failed to update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.toolTip.SetToolTip(this.buttonSaveData, "Update failed: " + Message);
                    this.buttonSaveData.BackColor = System.Drawing.Color.Red;
                }
                else
                    System.Windows.Forms.MessageBox.Show("Update failed");
                R.CancelEdit();
            }
            else if (Message.Length > 0)
            {
                System.Windows.Forms.MessageBox.Show("Update failed:\r\n" + Message);
                this.buttonSaveData.BackColor = System.Drawing.Color.Red;
                this.toolTip.SetToolTip(this.buttonSaveData, "Update failed:\r\n" + Message);
            }
            else
            {
                if (SqlType == DataTable.SqlCommandTypeExecuted.Insert && !this._SuppressRescanOfDataGrid)
                    this.getData();
                else
                {
                    try
                    {
                        DiversityWorkbench.Spreadsheet.DataColumn DCchanged = this._Sheet.SelectedColumns()[e.ColumnIndex];
                        foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                        {
                            if (DC.Value.IsVisible && !DC.Value.IsHidden && DC.Value.DataTable().Alias() == DCchanged.DataTable().Alias())
                            {
                                if (DC.Value.SqlForColumn != null && DC.Value.SqlForColumn.Length > 0)
                                {
                                    if (DC.Value.SqlForColumn.IndexOf(DCchanged.Column.Name) > -1)
                                    {
                                        this.dataGridView.Rows[e.RowIndex].Cells[DC.Key].Style.ForeColor = System.Drawing.Color.Red;
                                    }
                                }
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
        }

        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (this._Sheet.ReadOnly)
                return;

            DiversityWorkbench.Forms.FormFunctions.DrawRowNumber(this.dataGridView, this.dataGridView.Font, e, false);
            this.adaptControlsToRowHeaderWidth(this.dataGridView.RowHeadersWidth);
        }

        private void dataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.toolStripMenuItemRemove.Enabled = false;
            this.copyValuesToClipboardToolStripMenuItem.Enabled = false;
            this.transferValuesToClipboardToolStripMenuItem.Enabled = false;
            this.insertValesFromClipboardToolStripMenuItem.Enabled = false;
            this.useThisRowAsTemplateForNewRowsToolStripMenuItem.Enabled = true;
        }
        
        #endregion

        private void adaptControlsToRowHeaderWidth(int Width)
        {
            this.buttonFilter.Width = Width-1;
            this.buttonAddingClear.Width = Width-1;
        }

        private void buttonTimeout_Click(object sender, EventArgs e)
        {
            int Timeout = DiversityWorkbench.Settings.TimeoutDatabase;
            DiversityWorkbench.Forms.FormGetInteger f = new DiversityWorkbench.Forms.FormGetInteger(Timeout, "Timeout", "Please enter the timeout for database queries in seconds");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.Integer != null)
            {
                DiversityWorkbench.Settings.TimeoutDatabase = (int)f.Integer;
                this.toolTip.SetToolTip(buttonTimeout, "Current timeout: " + DiversityWorkbench.Settings.TimeoutDatabase.ToString() + " sec.");
            }
            f.Dispose();
            this._Sheet.ResetConnection();
        }

        #region Getting the data

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            if (!this._ProjectFound && this._Sheet.Target() != "Tasks")
            {
                System.Windows.Forms.MessageBox.Show("Please select a project");
                return;
            }

            getData();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (!this._ProjectFound)
            {
                System.Windows.Forms.MessageBox.Show("Please select a project");
                return;
            }

            this._Sheet.IncreaseOffset();
            this.getData();
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            if (!this._ProjectFound)
            {
                System.Windows.Forms.MessageBox.Show("Please select a project");
                return;
            }

            this._Sheet.ReduceOffset();
            this.getData();
        }

        private void setResultRange()
        {
            try
            {
                this.labelResultRange.Text = this._Sheet.Offset().ToString() + " - " + (this._Sheet.Offset() + this.dataGridView.Rows.Count /*this._Sheet.MaxResult()*/).ToString();
                this.labelResultMax.Text = "of " + this._Sheet.TotalCount().ToString();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private bool getData()
        {
            if (this._SuppressRescanOfDataGrid)
                return true;

            this.dataGridView.SuspendLayout();
            //this.dataGridView.DataSource = null;
            try
            {
                int ProjectID;
                if (this.comboBoxProject.SelectedValue == null ||
                    !int.TryParse(this.comboBoxProject.SelectedValue.ToString(), out ProjectID))
                {
                    // if no project is selected, take the first assuming that the user meant to take this project
                    if (!this._Sheet.HasProjects() && this._Sheet.Target() != "Tasks")
                    {
                        this.setProjectSource(true);
                        if (!this._Sheet.HasReadOnlyProjects())
                        {
                            System.Windows.Forms.MessageBox.Show("You have no access to any project");
                            this.Close();
                            return false;
                        }
                        else
                        {
                            this._Sheet.ProjectReadOnly = true;
                            this.SetControlsAccordingToProjectReadOnlyState();
                        }
                    }
                }
                int? SeletedRow = null;
                int? SelectedColumn = null;
                if (this.dataGridView.SelectedCells != null && this.dataGridView.SelectedCells.Count > 0)
                {
                    SeletedRow = this.dataGridView.SelectedCells[0].RowIndex;
                    SelectedColumn = this.dataGridView.SelectedCells[0].ColumnIndex;
                }
                this.dataGridView.DataSource = this._Sheet.getData();//(int)this.numericUpDownMaxResults.Value, ProjectID);
                this._Sheet.ResetLookupLists();
                this._LookupListCopies = null;
                this.setResultRange();
                this.setDataDefaultToolTips();
                this.setReadOnlyColumns();

                if (SeletedRow != null && SelectedColumn != null)
                {
                    if (this._Sheet.SelectedColumns()[(int)SelectedColumn].Type() == DataColumn.ColumnType.Operation)
                    {
                        SelectedColumn++;
                        while (!this._Sheet.SelectedColumns()[(int)SelectedColumn].IsVisible)
                            SelectedColumn++;
                    }
                    if (this.dataGridView.Rows.Count > (int)SeletedRow && this.dataGridView.Columns.Count > (int)SelectedColumn)
                        this.dataGridView.CurrentCell = this.dataGridView.Rows[(int)SeletedRow].Cells[(int)SelectedColumn];
                }
                this.FilterAlarm_Stop();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return false;
            }
            this.dataGridView.ResumeLayout();
            return true;
        }

        private void setReadOnlyColumns()
        {
            try
            {
                if (this._Sheet.ReadOnly)
                {
                    this.dataGridView.ReadOnly = true;
                }
                else
                {
                    this.dataGridView.ReadOnly = false;
                    try
                    {
                        if (this.dataGridViewFilter.Columns.Count > 0)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                            {
                                if (DC.Value.Type() == DataColumn.ColumnType.Operation ||
                                    DC.Value.Type() == DataColumn.ColumnType.Spacer ||
                                    DC.Value.LookupSource != null ||
                                    DC.Value.DataTable().Type() == DataTable.TableType.Lookup)
                                {
                                    if (this.dataGridView.Columns.Count > DC.Key)
                                        this.dataGridView.Columns[DC.Key].ReadOnly = true;
                                    else
                                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("setReadOnlyColumns()", DC.Value.DisplayText, "Try to set ReadOnly for column " + DC.Value.DisplayText + " at position " + DC.Key.ToString() + " in table " + DC.Value.DataTable().DisplayText + " while the dataGridView only contains " + this.dataGridView.Columns.Count.ToString() + " columns");
                                    if (this.dataGridViewAdding.Columns.Count > DC.Key)
                                        this.dataGridViewAdding.Columns[DC.Key].ReadOnly = true;
                                    else if (this._AddingPossible)
                                    {
                                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("setReadOnlyColumns()", DC.Value.DisplayText, "Try to set ReadOnly for column " + DC.Value.DisplayText + " at position " + DC.Key.ToString() + " in table " + DC.Value.DataTable().DisplayText + " while the dataGridViewAdding only contains " + this.dataGridView.Columns.Count.ToString() + " columns");
                                    }
                                }
                                else
                                {
                                    if (DC.Value.DataTable().AllowUpdate())
                                    {
                                        if (this.dataGridView.Columns.Count > DC.Key)
                                            this.dataGridView.Columns[DC.Key].ReadOnly = false;
                                        else
                                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("setReadOnlyColumns()", DC.Value.DisplayText, "Try to set ReadOnly for column " + DC.Value.DisplayText + " at position " + DC.Key.ToString() + " in table " + DC.Value.DataTable().DisplayText + " while the dataGridView only contains " + this.dataGridView.Columns.Count.ToString() + " columns");
                                    }
                                    else
                                    {
                                        if (this.dataGridView.Columns.Count > DC.Key)
                                            this.dataGridView.Columns[DC.Key].ReadOnly = true;
                                        else
                                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("setReadOnlyColumns()", DC.Value.DisplayText, "Try to set ReadOnly for column " + DC.Value.DisplayText + " at position " + DC.Key.ToString() + " in table " + DC.Value.DataTable().DisplayText + " while the dataGridView only contains " + this.dataGridView.Columns.Count.ToString() + " columns");
                                    }
                                }
                                if (DC.Value.Type() == DataColumn.ColumnType.Spacer)
                                {
                                    if (this.dataGridView.Columns.Count > DC.Key)
                                    {
                                        this.dataGridView.Columns[DC.Key].MinimumWidth = 2;
                                        if (this.dataGridViewAdding.Columns.Count > DC.Key)
                                            this.dataGridViewAdding.Columns[DC.Key].MinimumWidth = 2;
                                        if (this.dataGridViewFilter.Columns.Count > DC.Key)
                                            this.dataGridViewFilter.Columns[DC.Key].MinimumWidth = 2;
                                    }
                                    else
                                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("setReadOnlyColumns()", DC.Value.DisplayText, "Try to set the minimum width for column " + DC.Value.DisplayText + " at position " + DC.Key.ToString() + " in table " + DC.Value.DataTable().DisplayText + " while the dataGridView only contains " + this.dataGridView.Columns.Count.ToString() + " columns");
                                }
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Handling data: Update, Delete etc.

        private void FindDependentData(string Alias, System.Collections.Generic.Dictionary<string, string> PK, ref System.Collections.Generic.Dictionary<string, System.Data.DataTable> DependingTables)
        {
            foreach(System.Collections.Generic.KeyValuePair<string, DataTable> DT in this._Sheet.DataTables())
            {
                // getting the tables defined by relations within the spreadsheet
                if (DT.Value.ParentTable() != null && DT.Value.ParentTable().Alias() == Alias)
                {
                    string SQL = "SELECT * FROM [" + DT.Value.Name + "] WHERE 1 = 1";
                    foreach (System.Collections.Generic.KeyValuePair<string, string> K in PK)
                        SQL += " AND [" + K.Key + "] = '" + K.Value + "' ";
                    System.Data.DataTable dt = new System.Data.DataTable(DT.Value.Name);
                    string Message = "";
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
                    if (dt.Rows.Count > 0)
                    {
                        if (DependingTables.ContainsKey(DT.Value.Name))
                        {
                            continue;
                        }
                        else
                        {
                            DependingTables.Add(DT.Value.Name, dt);
                            this.FindDependentData(DT.Value.Alias(), PK, ref DependingTables);
                        }
                    }
                }
                // getting tables defined by relations within the database
                if (DT.Key == Alias)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, Data.Table.TableRelation> KV in DT.Value.RelatedTables())
                    {
                        if (KV.Value == Data.Table.TableRelation.Child)
                        {
                            string SQL = "SELECT * FROM [" + KV.Key + "] WHERE 1 = 1";
                            foreach (System.Collections.Generic.KeyValuePair<string, string> K in PK)
                                SQL += " AND [" + K.Key + "] = '" + K.Value + "' ";
                            System.Data.DataTable dt = new System.Data.DataTable(DT.Value.Name);
                            string Message = "";
                            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
                            if (dt.Rows.Count > 0)
                            {
                                if (DependingTables.ContainsKey(KV.Key))
                                {
                                    continue;
                                }
                                else
                                {
                                    DependingTables.Add(KV.Key, dt);
                                    string AliasDependent = "";
                                    foreach (System.Collections.Generic.KeyValuePair<string, DataTable> DTchild in this._Sheet.DataTables())
                                    {
                                        if (DTchild.Value.Name == KV.Key)
                                        {
                                            AliasDependent = DTchild.Key;
                                            this.FindDependentData(AliasDependent, PK, ref DependingTables);
                                            break;
                                        }
                                    }
                                    //this.FindDependentData(AliasDependent, PK, ref DependingTables);
                                }
                            }
                        }
                    }
                }
            }

        }

        private System.Collections.Generic.Dictionary<string, string> PrimaryKey(int iRow, string TableAlias)
        {
            System.Collections.Generic.Dictionary<string, string> PK = new Dictionary<string, string>();
            DiversityWorkbench.Spreadsheet.DataTable T = this._Sheet.DataTables()[TableAlias];
            foreach (string K in T.PrimaryKeyColumnList)
            {
                string Column = this._Sheet.DataTables()[TableAlias].DataColumns()[K].DisplayText;
                try
                {
                    if (this._Sheet.DT().Rows[iRow][Column].Equals(System.DBNull.Value)) // PK values can not be empty
                    {
                        PK.Clear();
                        break;
                    }
                    PK.Add(K, this._Sheet.DT().Rows[iRow][Column].ToString());
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    PK.Clear();
                    break;
                }
            }
            return PK;
        }

        private void DependetDataForDeleting(System.Collections.Generic.List<int> Rows, string TableAlias, System.Collections.Generic.Dictionary<string, string> PK, ref System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> DependetData)
        {
            //string Alias = this._Sheet.SelectedColumns()[Column].DataTable().Alias();
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().Type() == DataTable.TableType.Lookup)
                    continue;
                if (DC.Value.DataTable().ParentTable() != null &&
                    DC.Value.DataTable().ParentTable().Alias() == TableAlias &&
                    DC.Value.Type() == DataColumn.ColumnType.Data)
                {
                    string Alias = DC.Value.DataTable().Alias();
                    foreach (int iR in Rows)
                    {
                        if (this._Sheet.getTableReadOnlyRows(Alias).Contains(iR))
                            continue;
                        if (this._Sheet.DT().Rows[iR][DC.Key].Equals(System.DBNull.Value))
                            continue;
                        if (!DependetData.ContainsKey(Alias))
                        {
                            // getting data within the spread sheet
                            System.Collections.Generic.List<int> iRR = new List<int>();
                            iRR.Add(iR);

                            // getting data not included in the spread sheet
                            System.Collections.Generic.Dictionary<string, string> pkLocal = this.PrimaryKey(iR, Alias);
                            string SQL = "SELECT COUNT(*) FROM [" + this._Sheet.DataTables()[Alias].Name + "] WHERE 1 = 1 ";
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in PK)
                            {
                                SQL += " AND [" + KV.Key + "] = '" + KV.Value + "'";
                            }
                            if (pkLocal.Count > 0)
                            {
                                SQL += " AND (";
                                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in pkLocal)
                                {
                                    if (!PK.ContainsKey(KV.Key))
                                    {
                                        if (!SQL.EndsWith("("))
                                            SQL += " OR ";
                                        SQL += " [" + KV.Key + "] <> '" + KV.Value + "' ";
                                    }
                                }
                                SQL += ")";
                            }
                            string Count = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                            int iCount;
                            if (int.TryParse(Count, out iCount) && iCount > 0)
                            {
                                iRR.Add(iCount * -1);
                            }

                            DependetData.Add(Alias, iRR);

                            //if (
                        }
                        else
                        {
                            if (!DependetData[Alias].Contains(iR))
                                DependetData[Alias].Add(iR);
                        }
                    }
                    foreach (int iRow in Rows)
                    {
                        System.Collections.Generic.Dictionary<string, string> pk = this.PrimaryKey(iRow, Alias);
                        if (pk.Count > 0)
                            this.DependetDataForDeleting(Rows, Alias, pk, ref DependetData);
                    }
                }
            }
            return;
        }

        private void HandleDataGridData(int ColumnIndex, int RowIndex)
        {
            if (ColumnIndex > -1 && this._Sheet.SelectedColumns()[ColumnIndex].Type() == DataColumn.ColumnType.Operation)
            {
                if (!this._Sheet.SelectedColumns()[ColumnIndex].DataTable().AllowDelete())
                {
                    System.Windows.Forms.MessageBox.Show("You have no permission to delete data from table " + this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Name);
                    return;
                }
                if (!this._Sheet.getTableReadOnlyRows(this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias()).Contains(RowIndex) &&
                    !this._Sheet.getTableNullRows(this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias()).Contains(RowIndex)) // this.dataGridView.Rows[RowIndex].Cells[ColumnIndex].Style.ForeColor != this._ColorNull)
                {
                    // getting dependent tables
                    System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> DependetData = new Dictionary<string, List<int>>();
                    string Message = "Do you really want to delete these data from table " + this._Sheet.SelectedColumns()[ColumnIndex].DataTable().DisplayText;
                    System.Collections.Generic.List<int> Rows = new List<int>();
                    Rows.Add(RowIndex);
                    int StartIndex = RowIndex;
                    StartIndex++;
                    while (this._Sheet.getTableReadOnlyRows(this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias()).Contains(StartIndex))
                    {
                        Rows.Add(StartIndex);
                        StartIndex++;
                    }
                    foreach (int iRow in Rows)
                    {
                        System.Collections.Generic.Dictionary<string, string> PK = this.PrimaryKey(iRow, this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias());
                        this.DependetDataForDeleting(Rows, this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias(), PK, ref DependetData);
                    }
                    //this.DependetDataForDeleting(Rows, this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias(), ref DependetData);
                    if (DependetData.Count > 0)
                    {
                        Message += "\r\n\tThe following data will be deleted as well";
                        foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<int>> KV in DependetData)
                        {
                            Message += "\r\n\t\t" + this._Sheet.DataTables()[KV.Key].DisplayText + ":\t" + KV.Value.Count.ToString();
                            if (KV.Value.Count > 1) Message += " datasets";
                            else Message += " dataset";
                        }
                    }
                    if (System.Windows.Forms.MessageBox.Show(Message, "Delete data", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        bool OK = true;
                        if (DependetData.Count > 0)
                        {
                            System.Collections.Generic.List<string> L = new List<string>();
                            foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<int>> KV in DependetData)
                            {
                                L.Add(KV.Key);
                            }
                            L.Reverse();
                            Message = "";
                            foreach (string T in L)
                            {
                                foreach (int i in DependetData[T])
                                {
                                    this._Sheet.DataTables()[T].DeleteData(i, ref Message);
                                    if (Message.Length > 0)
                                    {
                                        System.Windows.Forms.MessageBox.Show(Message);
                                        OK = false;
                                    }
                                    if (!OK)
                                        break;
                                }
                                if (!OK)
                                    break;
                            }
                        }
                        if (OK)
                        {
                            // Getting the PK of the parent
                            System.Collections.Generic.Dictionary<string, string> PkParent = new Dictionary<string, string>();
                            if (this._Sheet.DataTables()[this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias()].ParentTable() != null)
                            {
                                foreach (string PK in this._Sheet.DataTables()[this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias()].ParentTable().PrimaryKeyColumnList)
                                {
                                    if (this._Sheet.DataTables()[this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias()].ParentTable().Type() == DataTable.TableType.Project)
                                    {
                                        if (PK == "ProjectID")
                                            PkParent.Add(PK, this._Sheet.ProjectID().ToString());
                                        else
                                            PkParent.Add(PK, this._Sheet.DT().Rows[RowIndex][this._Sheet.DataTables()[this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias()].ParentTable().DataColumns()[PK].Name].ToString());
                                    }
                                    else
                                        PkParent.Add(PK, this._Sheet.DT().Rows[RowIndex][this._Sheet.DataTables()[this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias()].ParentTable().DataColumns()[PK].DisplayText].ToString());
                                }
                            }
                            if (this._Sheet.DataTables()[this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias()].DeleteData(RowIndex, ref Message))
                            {
                                System.Windows.Forms.MessageBox.Show("Data deleted");
                                this.getData();
                                // try to find the parent
                                if (PkParent.Count > 0)
                                {
                                    for (int i = 0; i < this._Sheet.DT().Rows.Count; i++)
                                    {
                                        bool RowFound = true;
                                        foreach (System.Data.DataColumn C in this._Sheet.DT().Columns)
                                        {
                                            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in PkParent)
                                            {
                                                if (KV.Key == C.ColumnName && KV.Value != this._Sheet.DT().Rows[i][C].ToString())
                                                {
                                                    RowFound = false;
                                                    break;
                                                }
                                            }
                                        }
                                        if (RowFound)
                                        {
                                            this.dataGridView.Rows[i].Selected = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Message.Length > 0) Message = "Deleting failed:\r\n" + Message;
                                else Message = "Deleting failed";
                                System.Windows.Forms.MessageBox.Show(Message);
                            }
                        }
                    }
                }
            }
            else if (ColumnIndex > -1 && this._Sheet.SelectedColumns()[ColumnIndex].Type() == DataColumn.ColumnType.Spacer)
            { }
            else // Data columns
            {
                // setting the context menu
                this.useThisRowAsTemplateForNewRowsToolStripMenuItem.Enabled = false;
                if (this.dataGridView.SelectedCells.Count > 1)
                {
                    this.copyValuesToClipboardToolStripMenuItem.Enabled = true;
                    this.transferValuesToClipboardToolStripMenuItem.Enabled = true;
                }
                else
                {
                    this.copyValuesToClipboardToolStripMenuItem.Enabled = false;
                    this.transferValuesToClipboardToolStripMenuItem.Enabled = false;
                }
                this.insertValesFromClipboardToolStripMenuItem.Enabled = true;
                if (this.dataGridView.SelectedCells.Count > 1)
                    this.toolStripMenuItemRemove.Enabled = true;
                else
                    this.toolStripMenuItemRemove.Enabled = false;

                // setting the image
                if (this._ImageColumn != null && this._Sheet.DT().Columns.Contains(this._ImageColumn.DisplayText))
                {
                    try
                    {
                        if (this._Sheet.DT().Columns.Contains(this._ImageColumn.DisplayText))
                        {
                            System.Data.DataRow R = this._Sheet.DT().Rows[RowIndex];
                            string URI = R[this._ImageColumn.DisplayText].ToString(); // this._Sheet.DT().Rows[RowIndex][this._ImageColumn.DisplayText].ToString();
                            this.setImage(URI);
                        }
                        else
                        {
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }

                // setting the Readonly state
                if (ColumnIndex > -1)
                {
                    if (this._Sheet.SelectedColumns()[ColumnIndex].DataTable().AllowUpdate())
                    {
                        if (this._Sheet.SelectedColumns()[ColumnIndex].IsReadOnly())
                            this.dataGridView.Columns[ColumnIndex].ReadOnly = true;
                        else if (this._Sheet.SelectedColumns()[ColumnIndex].DataTable().PrimaryKeyColumnList.Contains(this._Sheet.SelectedColumns()[ColumnIndex].Name))
                        {
                            if (this.dataGridView.Rows[RowIndex].Cells[ColumnIndex].Value != System.DBNull.Value ||
                                this.dataGridView.Rows[RowIndex].Cells[ColumnIndex].Value.ToString().Length > 0)
                                this.dataGridView.Columns[ColumnIndex].ReadOnly = true;
                            //{

                            //}
                            if (this._Sheet.DT().Rows[RowIndex][ColumnIndex].Equals(System.DBNull.Value) &&
                                this._Sheet.SelectedColumns()[ColumnIndex].LookupSource != null)
                            {
                                this.dataGridViewDataCell_Click(dataGridView.Rows[RowIndex].Cells[ColumnIndex], Sheet.Grid.Data);
                            }
                        }
                        else if (ColumnIndex > -1 && !this._Sheet.getTableReadOnlyRows(this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias()).Contains(RowIndex))
                        {
                            if (this._Sheet.SelectedColumns()[ColumnIndex].LinkedToColumn != null && this._Sheet.SelectedColumns()[ColumnIndex].LinkedToColumn != null)
                            {
                                if (this._Sheet.DT().Rows[RowIndex][this._Sheet.SelectedColumns()[ColumnIndex].LinkedToColumn.DisplayText].Equals(System.DBNull.Value))
                                    this.dataGridView.Rows[RowIndex].Cells[ColumnIndex].ReadOnly = false;
                                else
                                    this.dataGridView.Rows[RowIndex].Cells[ColumnIndex].ReadOnly = true;
                            }
                            this.dataGridViewDataCell_Click(dataGridView.Rows[RowIndex].Cells[ColumnIndex], Sheet.Grid.Data);
                        }
                        else if (ColumnIndex > -1)
                        {
                            this.dataGridView.Rows[RowIndex].Cells[ColumnIndex].ReadOnly = true;
                        }
                    }
                    else
                    {
                        this.dataGridView.Rows[RowIndex].Cells[ColumnIndex].ReadOnly = true;
                    }
                    this.EditingEnableEditing(ColumnIndex);//this._Sheet.SelectedColumns()[ColumnIndex].DataTable().AllowUpdate());
                }
            }
        }

        private void SettingUpdateStateForData(int ColumnIndex, int RowIndex)
        {
            if (ColumnIndex > -1 && RowIndex > -1 &&
                (
                this._Sheet.SelectedColumns()[ColumnIndex].Type() == DataColumn.ColumnType.Link ||
                this._Sheet.SelectedColumns()[ColumnIndex].Type() == DataColumn.ColumnType.Data
                )
                )
            {
                // setting the Readonly state
                if (this._Sheet.SelectedColumns()[ColumnIndex].DataTable().AllowUpdate())
                {
                    if (this._Sheet.SelectedColumns()[ColumnIndex].IsReadOnly() || this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Type() == DataTable.TableType.Lookup)
                        this.dataGridView.Columns[ColumnIndex].ReadOnly = true;
                    else if (this._Sheet.SelectedColumns()[ColumnIndex].DataTable().PrimaryKeyColumnList.Contains(this._Sheet.SelectedColumns()[ColumnIndex].Name))
                    {
                        if (this.dataGridView.Rows[RowIndex].Cells[ColumnIndex].Value != System.DBNull.Value ||
                            this.dataGridView.Rows[RowIndex].Cells[ColumnIndex].Value.ToString().Length > 0)
                            this.dataGridView.Columns[ColumnIndex].ReadOnly = true;
                    }
                    else if (ColumnIndex > -1 && !this._Sheet.getTableReadOnlyRows(this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias()).Contains(RowIndex))
                    {
                        if (this._Sheet.SelectedColumns()[ColumnIndex].LinkedToColumn != null && this._Sheet.SelectedColumns()[ColumnIndex].LinkedToColumn != null)
                        {
                            if (this._Sheet.DT().Rows[RowIndex][this._Sheet.SelectedColumns()[ColumnIndex].LinkedToColumn.DisplayText].Equals(System.DBNull.Value))
                                this.dataGridView.Rows[RowIndex].Cells[ColumnIndex].ReadOnly = false;
                            else
                                this.dataGridView.Rows[RowIndex].Cells[ColumnIndex].ReadOnly = true;
                        }
                    }
                    else if (ColumnIndex > -1)
                    {
                        this.dataGridView.Rows[RowIndex].Cells[ColumnIndex].ReadOnly = true;
                    }
                }
                else
                {
                    this.dataGridView.Rows[RowIndex].Cells[ColumnIndex].ReadOnly = true;
                }
                //this.EnableEditing(ColumnIndex);
            }
        }

        private void MarkRange(int ColumnIndex)
        {
            if (this._FixedSourceSelectedCell != null
                && this._FixedSourceSelectedCell.ColumnIndex != ColumnIndex)
            {
                System.Windows.Forms.MessageBox.Show("Only one column can be selected");
                this.MarkColumnRange = false;
                return;
            }
            System.Collections.Generic.List<int> CC = new List<int>();
            foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.SelectedCells)
            {
                if (this._MarkedColumnIndex != null && C.ColumnIndex != this._MarkedColumnIndex)
                {
                    System.Windows.Forms.MessageBox.Show("Only one column can be selected");
                    C.Selected = false;
                    this.MarkColumnRange = false;
                    break;
                }
                if (!CC.Contains(C.ColumnIndex))
                    CC.Add(C.ColumnIndex);
                if (CC.Count > 1)
                {
                    System.Windows.Forms.MessageBox.Show("Only one column can be selected");
                    C.Selected = false;
                    this.MarkColumnRange = false;
                    break;
                }
            }
            if (CC.Count > 1)
            {
                foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.SelectedCells)
                {
                    if (C.ColumnIndex != CC[0])
                        C.Selected = false;
                }
            }
            else
            {
                if (ColumnIndex > -1 && this._Sheet.SelectedColumns()[ColumnIndex].Type() != DataColumn.ColumnType.Spacer &&
                    ColumnIndex > -1 && this._Sheet.SelectedColumns()[ColumnIndex].Type() != DataColumn.ColumnType.Operation &&
                    this._Sheet.SelectedColumns()[ColumnIndex].DataTable().AllowDelete())
                {
                    if (this.dataGridView.SelectedCells.Count > 1)
                    {
                        this.copyValuesToClipboardToolStripMenuItem.Enabled = true;
                        this.transferValuesToClipboardToolStripMenuItem.Enabled = true;
                        this.toolStripMenuItemRemove.Enabled = true;
                    }
                    else
                    {
                        this.copyValuesToClipboardToolStripMenuItem.Enabled = false;
                        this.transferValuesToClipboardToolStripMenuItem.Enabled = false;
                        if (this.dataGridView.SelectedCells.Count > 0)
                            this.toolStripMenuItemRemove.Enabled = true;
                        else
                            this.toolStripMenuItemRemove.Enabled = false;
                    }
                }
            }
        }

        private void DeleteData(int ColumnIndex, int RowIndex)
        {
            try
            {
                System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> DependetData = new Dictionary<string, List<int>>();
                string Message = "Do you really want to delete these data from table " + this._Sheet.SelectedColumns()[ColumnIndex].DataTable().DisplayText;
                if (System.Windows.Forms.MessageBox.Show(Message, "Delete data", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return;

                System.Collections.Generic.List<int> Rows = new List<int>();
                Rows.Add(RowIndex);
                int StartIndex = RowIndex;
                StartIndex++;
                while (this._Sheet.getTableReadOnlyRows(this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias()).Contains(StartIndex))
                {
                    Rows.Add(StartIndex);
                    StartIndex++;
                }

                string Alias = this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias();

                // getting dependent tables
                System.Collections.Generic.Dictionary<string, string> pk = this.PrimaryKey(RowIndex, Alias);
                System.Collections.Generic.Dictionary<string, System.Data.DataTable> Tables = new Dictionary<string, System.Data.DataTable>();
                this.FindDependentData(Alias, pk, ref Tables);
                if (Tables.Count > 0)
                {
                    FormDependentData fdd = new FormDependentData("Dependent data will be deleted as well. OK?", Tables);
                    fdd.ShowDialog();
                    if (fdd.DialogResult != System.Windows.Forms.DialogResult.OK)
                    {
                        return;
                    }
                    fdd.Dispose();
                    // Changing sequence of tables for delete (most depending data should be removed first)
                    System.Collections.Generic.SortedList<int, string> DependingTables = new SortedList<int, string>();
                    foreach (System.Collections.Generic.KeyValuePair<string, System.Data.DataTable> DT in Tables)
                    {
                        DependingTables.Add(Tables.Count - DependingTables.Count, DT.Key);
                    }
                    foreach (System.Collections.Generic.KeyValuePair<int, string> DT in DependingTables)
                    {
                        string SQL = "DELETE T FROM [" + DT.Value + "] AS T WHERE 1 = 1 ";
                        foreach (System.Collections.Generic.KeyValuePair<string, string> K in pk)
                            SQL += " AND [" + K.Key + "] = '" + K.Value + "' ";
                        Message = "";
                        if (!DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Message))
                        {
                            System.Windows.Forms.MessageBox.Show(Message);
                            return;
                        }
                    }
                }
                // Getting the PK of the parent
                System.Collections.Generic.Dictionary<string, string> PkParent = new Dictionary<string, string>();
                if (this._Sheet.DataTables()[this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias()].ParentTable() != null)
                {
                    DiversityWorkbench.Spreadsheet.DataTable PT = this._Sheet.DataTables()[this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias()].ParentTable();
                    foreach (string PK in PT.PrimaryKeyColumnList)
                    {
                        if (PT.Type() == DataTable.TableType.Project)
                        {
                            if (PK == "ProjectID")
                                PkParent.Add(PK, this._Sheet.ProjectID().ToString());
                            else
                                PkParent.Add(PK, this._Sheet.DT().Rows[RowIndex][PT.DataColumns()[PK].Name].ToString());
                        }
                        else
                        {
                            string TableAlias = this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias();
                            string Column = this._Sheet.DataTables()[TableAlias].DataColumns()[PK].DisplayText;
                            try
                            {
                                PkParent.Add(PK, this._Sheet.DT().Rows[RowIndex][Column].ToString());
                            }
                            catch (System.Exception ex)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            }
                        }
                    }
                }
                if (this._Sheet.DataTables()[this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias()].DeleteData(RowIndex, ref Message))
                {
                    System.Windows.Forms.MessageBox.Show("Data deleted");
                    this.getData();
                    // try to find the parent
                    if (PkParent.Count > 0)
                    {
                        for (int i = 0; i < this._Sheet.DT().Rows.Count; i++)
                        {
                            bool RowFound = true;
                            foreach (System.Data.DataColumn C in this._Sheet.DT().Columns)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in PkParent)
                                {
                                    if (KV.Key == C.ColumnName && KV.Value != this._Sheet.DT().Rows[i][C].ToString())
                                    {
                                        RowFound = false;
                                        break;
                                    }
                                }
                            }
                            if (RowFound)
                            {
                                this.dataGridView.Rows[i].Selected = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (Message.Length > 0) Message = "Deleting failed:\r\n" + Message;
                    else Message = "Deleting failed";
                    System.Windows.Forms.MessageBox.Show(Message);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        private void buttonSaveData_Click(object sender, EventArgs e)
        {
            this.getData();
            this.buttonSaveData.BackColor = System.Drawing.SystemColors.Control;
        }

        private void buttonGetSQL_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("SQL", this._Sheet.SQL(Sheet.QueryType.SQL), true);
            f.ShowDialog();
            f.Dispose();
        }

        #endregion

        #region Scrolling

        private void dataGridView_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                int VerticalOffset = 0;
                int HeightAvailable = this.dataGridView.Height;
                int HeightNeeded = this.dataGridView.Rows.Count * this.dataGridView.ColumnHeadersHeight;
                if (HeightNeeded > HeightAvailable) //in this case the scroll to the right end will leed to a mismatch
                {
                    int WidthAvailable = this.dataGridView.Width;
                    int WidthNeeded = this.dataGridView.RowHeadersWidth;
                    foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                    {
                        if (C.Visible)
                            WidthNeeded += C.Width;
                    }
                    if (this.dataGridView.HorizontalScrollingOffset > 0
                        && WidthNeeded > WidthAvailable
                        && this.dataGridView.HorizontalScrollingOffset > WidthNeeded - WidthAvailable) // reaching the right end
                    {
                        VerticalOffset = this.dataGridView.HorizontalScrollingOffset - (WidthNeeded - WidthAvailable);
                        this.dataGridView.HorizontalScrollingOffset -= VerticalOffset;
                    }
                }
                if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
                {
                    this.dataGridViewAdding.HorizontalScrollingOffset = e.NewValue;
                    this.dataGridViewFilter.HorizontalScrollingOffset = e.NewValue;

                    System.Drawing.Point P = new Point(VerticalOffset - e.NewValue, 0);
                    this.panelTableMarker.Location = P;
                    this.panelTableMarker.Width = this.panelTableMarker.Width + e.NewValue;
                    this.panelTableMarker.Dock = DockStyle.None;
                }
            }
            catch (System.Exception ex)
            {
            }
        }
        
        private void FormSpreadsheet_SizeChanged(object sender, EventArgs e)
        {
            if (this._Sheet != null)
                this.setTableMarker();
        }

        #endregion

        #region Editing and fixed source

        #region The selected data cell and setting this cell

        /// <summary>
        /// The last activated cell by a user action that may be involved in fixing the source
        /// is addressed by Pin and DataGridView -Click and -MouseDown 
        /// it is set by -MouseUp
        /// </summary>
        private System.Windows.Forms.DataGridViewCell _FixedSourceSelectedCell = null;

        /// <summary>
        /// The indesx of the selected row. May be null if several rows are selected
        /// </summary>
        //private int? _SelectedRowIndex = null;

        /// <summary>
        /// Setting the cell as a marker for the position of any action related to fixed sources and editing controls
        /// resets the FixSourceIWorkbenchUnit if there are any changes in the column
        /// </summary>
        /// <param name="ColumnIndex"></param>
        /// <param name="RowIndex"></param>
        //private void setSelectedDataCell(int ColumnIndex, int RowIndex)
        //{
        //    bool OK = false;
        //    try
        //    {
        //        if (ColumnIndex > -1
        //            && RowIndex > -1
        //            && this._Sheet.SelectedColumns().ContainsKey(ColumnIndex)
        //            && this._Sheet.SelectedColumns()[ColumnIndex].IsVisible
        //            && !this._Sheet.SelectedColumns()[ColumnIndex].IsHidden)
        //        {
        //            if (this.dataGridView.Rows.Count >= RowIndex)
        //            {
        //                if (this._SelectedDataCell != null && this._FixSourceIWorkbenchUnit != null)
        //                {
        //                    if (this._SelectedDataCell.ColumnIndex != ColumnIndex)
        //                        this._FixSourceIWorkbenchUnit = null;
        //                }
        //                // if the column has been changed
        //                if (this._SelectedDataCell != null 
        //                    && this._SelectedDataCell.ColumnIndex != ColumnIndex)
        //                {
        //                    this.EditingResetComboboxSources();
        //                    this.FixingParametersReset();
        //                }
        //                // Setting the cell
        //                this._SelectedDataCell = this.dataGridView.Rows[RowIndex].Cells[ColumnIndex];
        //                OK = true;
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        OK = false;
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    if (!OK)
        //    {
        //        this._SelectedDataCell = null;
        //        //this._SelectedRowIndex = null;
        //        this._FixSourceIWorkbenchUnit = null;
        //    }
        //}

        /// <summary>
        /// If a valid cell within the spreadsheet has been selected
        /// in case of a change of the column or if no valid cell all parameters will be reset
        /// otherwise set the value for _SelectedDataCell
        /// and set the connections parameters for the retrieval of the fixed source
        /// </summary>
        /// <param name="ColumnIndex">Column index of the cell</param>
        /// <param name="RowIndex">Row index of the cell</param>
        /// <returns>Cell is valid</returns>
        private bool FixedSourceSetCellSelected(int ColumnIndex, int RowIndex)
        {
            // check if any valid cell has been selected
            bool AnyValidCellSelected = this.AnyValidCellSelected(ColumnIndex, RowIndex); 

            // if a valid cell has been selected
            if (AnyValidCellSelected)
            {
                try
                {
                    bool FixingParametersNeedReset = this.FixedSourceParametersNeedReset(ColumnIndex, RowIndex);
                    if (FixingParametersNeedReset)
                    {
                        this.EditingResetComboboxSources();
                        this.FixedSourceParametersReset();

                        // Setting the cell
                        this._FixedSourceSelectedCell = this.dataGridView.Rows[RowIndex].Cells[ColumnIndex];

                        this.EditingSetComboboxStyle();
                        this.EditingEnableEditing(ColumnIndex);

                        // if the column is a link column
                        if (this._Sheet.SelectedColumns()[ColumnIndex].IsLinkedColumn())
                        {
                            string Value = "";
                            if (this._Sheet.SelectedColumns()[ColumnIndex].FixedSourceNeedsValueFromData())
                                Value = this.FixedSourceGetValueFromData(this._Sheet.SelectedColumns()[ColumnIndex].FixedSourceGetSetting(), ColumnIndex, RowIndex);
                            this.FixedSourcesSetSettings(this._Sheet.SelectedColumns()[ColumnIndex].FixedSourceGetSetting(Value));
                        }
                        else
                        {
                            this.FixedSourceSetControls();
                        }
                    }
                    else
                    {
                        // Setting the cell
                        this._FixedSourceSelectedCell = this.dataGridView.Rows[RowIndex].Cells[ColumnIndex];
                    }


                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else // no valid cell selected
            {
                this._FixedSourceSelectedCell = null;
                this.EditingResetComboboxSources();
                this.FixedSourceParametersReset();
            }

            return AnyValidCellSelected;
        }

        /// <summary>
        /// Check if the parameters of the fixed source need a reset e.g. if the column has changed
        /// </summary>
        /// <param name="ColumnIndex"></param>
        /// <param name="RowIndex"></param>
        /// <returns></returns>
        private bool FixedSourceParametersNeedReset(int ColumnIndex, int RowIndex)
        {
            bool FixingParametersNeedReset = false;
            try
            {
                if (this._FixedSourceSelectedCell == null)
                {
                    // If for any reason this value is null, reset the parameters
                    FixingParametersNeedReset = true;
                }
                else
                {
                    // resetting connection values if the colum has changed
                    if (this._FixedSourceSelectedCell.ColumnIndex != ColumnIndex)
                    {
                        FixingParametersNeedReset = true;
                    }
                    else
                    {
                        // if the column is a link column
                        if (this._Sheet.SelectedColumns()[ColumnIndex].IsLinkedColumn())
                        {
                            // if the setting depends on values from the data
                            if (this._Sheet.SelectedColumns()[ColumnIndex].FixedSourceNeedsValueFromData())
                            {
                                // if the row changed
                                if (this._FixedSourceSelectedCell != null && this._FixedSourceSelectedCell.RowIndex != RowIndex)
                                {
                                    // previous value
                                    string ValuePrevious = this.FixedSourceGetValueFromData(this._Sheet.SelectedColumns()[ColumnIndex].FixedSourceGetSetting(), ColumnIndex, _FixedSourceSelectedCell.RowIndex);// this._Sheet.SelectedColumns()[ColumnIndex].FixedSourceNeedsValueFromData();
                                    // current value
                                    string ValueCurrent = this.FixedSourceGetValueFromData(this._Sheet.SelectedColumns()[ColumnIndex].FixedSourceGetSetting(), ColumnIndex, RowIndex);
                                    if (ValuePrevious != ValueCurrent)
                                        FixingParametersNeedReset = true;
                                }
                            }
                        }
                        else // No link column
                        {
                            //FixingParametersNeedReset = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                FixingParametersNeedReset = true;
            }
            return FixingParametersNeedReset;
        }

        /// <summary>
        /// Check if any valid cell has been selected
        /// </summary>
        /// <param name="ColumnIndex"></param>
        /// <param name="RowIndex"></param>
        /// <returns></returns>
        private bool AnyValidCellSelected(int ColumnIndex, int RowIndex)
        {
            // check if any valid cell has been selected
            bool AnyValidCellSelected = true;
            try
            {
                // no column selected
                if (ColumnIndex == -1)
                    AnyValidCellSelected = false;
                // No column in the sheet found
                if (!this._Sheet.SelectedColumns().ContainsKey(ColumnIndex))
                    AnyValidCellSelected = false;
                // column in the sheet not shown
                if (AnyValidCellSelected)
                {
                    if (!this._Sheet.SelectedColumns()[ColumnIndex].IsVisible
                    || this._Sheet.SelectedColumns()[ColumnIndex].IsHidden)
                        AnyValidCellSelected = false;
                }
                // row not in valid range
                if (this.dataGridView.Rows.Count < RowIndex || RowIndex < 0)
                    AnyValidCellSelected = false;

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                AnyValidCellSelected = false;
            }
            return AnyValidCellSelected;
        }

        /// <summary>
        /// Set the edit controls and the cell for which the fixing is performed
        /// compares values to FixedSourceCell or changes
        /// as last step, set the fixed source cell to the given values for later comparision
        /// </summary>
        /// <param name="ColumnIndex">The index of the column. If different from the previous value, Editing and fixing controls are disabled</param>
        /// <param name="RowIndex">Index of the row</param>
        //private void setEditingControlsAndFixedSourceCell(int ColumnIndex, int RowIndex)
        //{
        //    try
        //    {
        //        // setting of the value _SelectedDataCell does not happen here to enable comparision at later point in code

        //        // the column has been changed
        //        //if (this._SelectedDataCell != null && ColumnIndex != this._SelectedDataCell.ColumnIndex)
        //        //{
        //        //    this._SelectedDataCell = null;
        //        //    this.FixingAndEditingReset();
        //        //    this.EditingResetComboboxSources();
        //        //}
        //        //else
        //        {
        //            if (this.FixedSourceSetCellSelected(ColumnIndex, RowIndex))
        //            {
        //                this.EditingSetComboboxStyle();
        //                this.EditingEnableEditing(ColumnIndex);
        //                if (this._Sheet.SelectedColumns()[ColumnIndex].IsLinkedColumn())
        //                {
        //                    this.FixedSourceSetControls();
        //                }
        //                // non special cells
        //                else
        //                {
        //                    if (this._Sheet.SelectedColumns()[ColumnIndex].Column.ForeignRelationTable != null)
        //                    {
        //                    }
        //                    else
        //                    {
        //                        if (this._FixedSourceSelectedCell == null || this._FixedSourceSelectedCell.ColumnIndex != ColumnIndex)
        //                        {
        //                            // sollte nie erreicht werden
        //                        }
        //                    }
        //                }
        //                if (this._FixedSourceSelectedCell == null || this._FixedSourceSelectedCell.ColumnIndex != ColumnIndex)
        //                {
        //                    this.EditingEnableEditing(ColumnIndex);
        //                }

        //                this.FixedSourceSetFixedSourceSettings(ColumnIndex, RowIndex);

        //                // Setting the cell
        //                //this.setSelectedDataCell(ColumnIndex, RowIndex);
        //            }
        //            else
        //            {
        //                this._FixedSourceSelectedCell = null;
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        #endregion

        #region Editing

        #region Events of buttons and comboboxes

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            this._SuppressRescanOfDataGrid = true;
            int iColumn = this.dataGridView.SelectedCells[0].ColumnIndex;
            int iRowStart = this.dataGridView.SelectedCells[0].RowIndex;
            int iRowEnd = this.dataGridView.SelectedCells[0].RowIndex;
            string Error = "";
            try
            {
                if (this.dataGridView.SelectedCells.Count < 2
                    && this.dataGridView.SelectedCells[0].ColumnIndex == 0
                    && this.dataGridView.SelectedCells[0].RowIndex == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Nothing selected");
                    return;
                }
                switch (this._EditMode)
                {
                    case EditMode.Append:
                    case EditMode.Prepend:
                        if (this._Sheet.SelectedColumns()[this.dataGridView.SelectedCells[0].ColumnIndex].LookupSource != null)
                        {
                            System.Windows.Forms.MessageBox.Show("Only replacement allowed");
                            return;
                        }
                        if (this.comboBoxReplace.Text.Length == 0)
                        {
                            System.Windows.Forms.MessageBox.Show("No value entered");
                            return;
                        }
                        break;
                    case EditMode.Clear:
                        if (!this._Sheet.SelectedColumns()[this.dataGridView.SelectedCells[0].ColumnIndex].Column.IsNullable)
                        {
                            System.Windows.Forms.MessageBox.Show("No clearing allowed. This column can not be empty");
                            return;
                        }
                        break;
                    case EditMode.Replace:
                        if (this.comboBoxReplace.Text.Length == 0)
                        {
                            System.Windows.Forms.MessageBox.Show("No value to replace entered");
                            return;
                        }
                        break;
                    case EditMode.Calculate:
                        if (this.comboBoxReplace.Text.Length == 0)
                        {
                            System.Windows.Forms.MessageBox.Show("No value for calculation entered");
                            return;
                        }
                        break;
                    case EditMode.Overwrite:
                        if (this.comboBoxReplaceWith.Text.Length == 0 && this._Sheet.SelectedColumns()[this.dataGridView.SelectedCells[0].ColumnIndex].LookupSource != null)
                        {
                            System.Windows.Forms.MessageBox.Show("No value entered");
                            return;
                        }
                        break;
                }
                System.Collections.Generic.List<int> SelectedCells = new List<int>();
                foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.SelectedCells)
                {
                    SelectedCells.Add(C.RowIndex);
                }
                int iNoUpdate = 0;
                foreach (int i in SelectedCells)
                {
                    if (!this._Sheet.SelectedColumns()[iColumn].DataTable().AllowUpdate())
                    {
                        iNoUpdate++;
                        continue;
                    }
                    switch (this._EditMode)
                    {
                        case EditMode.Append:
                            if (this.dataGridView.Rows[i].Cells[iColumn].Value == null)
                                this.dataGridView.Rows[i].Cells[iColumn].Value = this.comboBoxReplace.Text;
                            else
                                this.dataGridView.Rows[i].Cells[iColumn].Value = this.dataGridView.Rows[i].Cells[iColumn].Value.ToString() + this.comboBoxReplace.Text;
                            break;
                        case EditMode.Clear:
                            this.dataGridView.Rows[i].Cells[iColumn].Value = System.DBNull.Value;
                            break;
                        case EditMode.Prepend:
                            if (this.dataGridView.Rows[i].Cells[iColumn].Value == null)
                                this.dataGridView.Rows[i].Cells[iColumn].Value = this.comboBoxReplace.Text;
                            else
                                this.dataGridView.Rows[i].Cells[iColumn].Value = this.comboBoxReplace.Text + this.dataGridView.Rows[i].Cells[iColumn].Value.ToString();
                            break;
                        case EditMode.Replace:
                            if (this.dataGridView.Rows[i].Cells[iColumn].Value == null)
                                continue;
                            if (this.dataGridView.Rows[i].Cells[iColumn].Value.ToString().IndexOf(this.comboBoxReplace.Text) == -1)
                                continue;
                            this.dataGridView.Rows[i].Cells[iColumn].Value = this.dataGridView.Rows[i].Cells[iColumn].Value.ToString().Replace(this.comboBoxReplace.Text, this.comboBoxReplaceWith.Text);
                            break;
                        case EditMode.Overwrite:
                            if (this._FixedSourceValues != null
                                && this._FixedSourceValues.Count > 0
                                && this._Sheet.SelectedColumns()[iColumn].LookupSource == null) // not for lookup lists
                            {
                                this._Sheet.setLinkedColumnValues(this.dataGridView.Rows[i].Cells[iColumn], this._FixedSourceValues, Sheet.Grid.Data, ref Error);
                            }
                            else
                            {
                                this.dataGridView.Rows[i].Cells[iColumn].Value = this.comboBoxReplaceWith.Text;
                            }
                            break;
                    }
                }
                if (SelectedCells.Count > 100)
                {
                    string Message = this._EditMode.ToString() + ": " + SelectedCells.Count + " Rows in " + this.dataGridView.Columns[iColumn].HeaderText;
                    if (iNoUpdate > 0)
                        Message += "\r\nNo update for " + iNoUpdate.ToString() + " Rows";
                    if (Error.Length > 0)
                        Message += "\r\nError: " + Error;
                    System.Windows.Forms.MessageBox.Show(Message);
                }

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this._SuppressRescanOfDataGrid = false;
            this.MarkColumnRangeReset();
            this.getData();
        }

        private void buttonEditMode_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(this.EditingPossibleEditModes(), "Edit mode", "Please select the edit mode", true);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                switch (f.SelectedString)
                {
                    case "Append":
                        this.EditingSetEditMode(EditMode.Append);
                        break;
                    case "Clear":
                        this.EditingSetEditMode(EditMode.Clear);
                        break;
                    case "Prepend":
                        this.EditingSetEditMode(EditMode.Prepend);
                        break;
                    case "Replace":
                        this.EditingSetEditMode(EditMode.Replace);
                        break;
                    case "Overwrite":
                        this.EditingSetEditMode(EditMode.Overwrite);
                        break;
                }
                this.EditingEnableEditing(this.dataGridView.SelectedCells[0].ColumnIndex);//true, true);
                this.MarkColumnRangeReset();
            }
            f.Dispose();
        }

        private async void comboBoxReplace_DropDown(object sender, EventArgs e)
        {
            await this.EditingSetComboboxSource(this.comboBoxReplace);
        }

        private async void comboBoxReplaceWith_DropDown(object sender, EventArgs e)
        {
            await this.EditingSetComboboxSource(this.comboBoxReplaceWith);
        }

        private void comboBoxReplaceWith_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.comboBoxReplaceWith.SelectedItem != null)
                {
                    if (this.comboBoxReplaceWith.SelectedItem.GetType() == typeof(System.Data.DataRowView))
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxReplaceWith.SelectedItem;
                        this.FixedSourceSetValues(R.Row);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void comboBoxReplaceWith_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            this.ShowFixSourceHelp();
        }

        private void comboBoxReplaceWith_KeyUp(object sender, KeyEventArgs e)
        {
            this._RequeryDatasource = true;
            //if (this.comboBoxReplaceWith.DataSource != null)
            //    this.comboBoxReplaceWith.DataSource = null;
        }

        private bool _RequeryDatasource = false;
        
        #endregion

        #region Edit mode

        private enum EditMode { Replace, Overwrite, Prepend, Append, Clear, Calculate }
        private EditMode _EditMode = EditMode.Overwrite;

        /// <summary>
        /// Setting the mode for the editing controls
        /// </summary>
        /// <param name="Mode">The selected mode</param>
        private void EditingSetEditMode(EditMode Mode)
        {
            this._EditMode = Mode;
            // default settings
            this.labelReplaceWith.Text = "";
            this.comboBoxReplace.Visible = true;
            this.comboBoxReplaceWith.Visible = true;
            this.labelReplaceWith.Visible = true;
            this.comboBoxReplace.Enabled = true;
            this.comboBoxReplaceWith.Enabled = false;

            // Markus 20.8.24: Icons for second button
            switch (Mode)
            {
                case EditMode.Append:
                    this.buttonEditMode.Image = this.imageListEditMode.Images[0];
                    this.buttonEdit.Image = this.imageListEditMode.Images[0];
                    this.buttonEdit.Text = EditMode.Append.ToString();
                    this.toolTip.SetToolTip(this.buttonEdit, "Append the selected or entered string to all cells selected in the spreadsheet");
                    break;
                case EditMode.Clear:
                    this.buttonEditMode.Image = this.imageListEditMode.Images[1];
                    this.buttonEdit.Image = this.imageListEditMode.Images[1];
                    this.buttonEdit.Text = EditMode.Clear.ToString();
                    this.toolTip.SetToolTip(this.buttonEdit, "Clear all cells selected in the spreadsheet");
                    this.comboBoxReplace.Enabled = false;
                    break;
                case EditMode.Prepend:
                    this.buttonEditMode.Image = this.imageListEditMode.Images[2];
                    this.buttonEdit.Image = this.imageListEditMode.Images[2];
                    this.buttonEdit.Text = EditMode.Prepend.ToString();
                    this.toolTip.SetToolTip(this.buttonEdit, "Prepend the selected or entered string to all cells selected in the spreadsheet");
                    break;
                case EditMode.Replace:
                    this.buttonEditMode.Image = this.imageListEditMode.Images[3];
                    this.buttonEdit.Image = this.imageListEditMode.Images[3];
                    this.buttonEdit.Text = EditMode.Replace.ToString();
                    this.toolTip.SetToolTip(this.buttonEdit, "Replace the selected or entered string to all cells selected in the spreadsheet");
                    this.comboBoxReplaceWith.Enabled = true;
                    this.labelReplaceWith.Text = "With";
                    break;
                case EditMode.Overwrite:
                    this.buttonEditMode.Image = this.imageListEditMode.Images[4];
                    this.buttonEdit.Image = this.imageListEditMode.Images[4];
                    this.buttonEdit.Text = EditMode.Overwrite.ToString();
                    this.toolTip.SetToolTip(this.buttonEdit, "Overwrite the content of all cells selected in the spreadsheet");
                    this.comboBoxReplace.Enabled = false;
                    this.comboBoxReplaceWith.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// After a change of e.g. the selected column, check if the selected mode can still be applied, otherwise change to OVERWRITE
        /// </summary>
        private void EditingCheckEditMode()
        {
            if (!this.EditingCanAppend())
            {
                if (this._EditMode == EditMode.Append || this._EditMode == EditMode.Prepend)
                    this.EditingSetEditMode(EditMode.Overwrite);
            }
            if (!this.EditingCanClear())
            {
                if (this._EditMode == EditMode.Clear)
                    this.EditingSetEditMode(EditMode.Overwrite);
            }
        }
        
        /// <summary>
        /// If the editing versions APPEND or PREPEND are allowed
        /// </summary>
        /// <returns>Allowed = true</returns>
        private bool EditingCanAppend()
        {
            bool CanAppend = true;
            if (this._FixedSourceSelectedCell != null)
            {
                // No Append for Lookup sources
                if (this._Sheet.SelectedColumns()[this._FixedSourceSelectedCell.ColumnIndex].LookupSource != null)
                    CanAppend = false;
                // No Append for linked columns
                if ((this._Sheet.SelectedColumns()[this._FixedSourceSelectedCell.ColumnIndex].IsLinkedColumn()))
                    CanAppend = false;
            }
            else
                CanAppend = false;
            return CanAppend;
        }

        /// <summary>
        /// If the editing version CLEAR is allowed
        /// </summary>
        /// <returns>Allowed = true</returns>
        private bool EditingCanClear()
        {
            bool CanClear = true;
            if (this._FixedSourceSelectedCell != null)
            {
                if (!this._Sheet.SelectedColumns()[this._FixedSourceSelectedCell.ColumnIndex].Column.IsNullable)
                    CanClear = false;
            }
            else
                CanClear = false;
            return CanClear;
        }

        /// <summary>
        /// The possible edit modes, dependent on the selected columns
        /// </summary>
        /// <returns>List of modes</returns>
        private System.Collections.Generic.List<string> EditingPossibleEditModes()
        {
            System.Collections.Generic.List<string> EditModes = new List<string>();
            if (this.EditingCanAppend())
            {
                EditModes.Add(EditMode.Append.ToString());
                EditModes.Add(EditMode.Prepend.ToString());
            }
            if (this.EditingCanClear())
                EditModes.Add(EditMode.Clear.ToString());
            EditModes.Add(EditMode.Replace.ToString());
            EditModes.Add(EditMode.Overwrite.ToString());
            return EditModes;
        }

        #endregion

        #region Sources for the comboboxes

        /// <summary>
        /// Reset the datasources for the editing comboboxes e.g after changing the column
        /// </summary>
        private void EditingResetComboboxSources()
        {
            this.comboBoxReplace.DataSource = null;
            this.comboBoxReplaceWith.DataSource = null;
        }

        /// <summary>
        /// Setting the source for a combobox, dependent on the column and possible fixing
        /// </summary>
        /// <param name="CB">The combobox for which the source should be set if missing</param>
        private async System.Threading.Tasks.Task EditingSetComboboxSource(System.Windows.Forms.ComboBox CB)
        {
            try
            {
                if (CB.DataSource == null || this._RequeryDatasource)
                {
                    if (this._FixedSourceSelectedCell != null) // Any cell selected
                    {
                        // Lookup source available
                        if (this._Sheet.SelectedColumns()[this._FixedSourceSelectedCell.ColumnIndex].LookupSource !=
                            null)
                        {
                            string Preselection = "";
                            if (this.dataGridView.Rows[this._FixedSourceSelectedCell.RowIndex]
                                    .Cells[this._FixedSourceSelectedCell.ColumnIndex].Value != null)
                                Preselection = this.dataGridView.Rows[this._FixedSourceSelectedCell.RowIndex]
                                    .Cells[this._FixedSourceSelectedCell.ColumnIndex].Value.ToString();
                            System.Data.DataTable DtValues =
                                this.LookupSourceForColumn(this._FixedSourceSelectedCell.ColumnIndex, Preselection);
                            CB.DataSource = DtValues;
                            CB.DisplayMember = DtValues.Columns[0].ColumnName;
                            CB.ValueMember = DtValues.Columns[0].ColumnName;
                        }
                        // Linked columns
                        else if (this._Sheet.SelectedColumns()[this._FixedSourceSelectedCell.ColumnIndex]
                                 .IsLinkedColumn())
                        {
                            // source is fixed
                            if (this.FixedSourceSourceIsFixed())
                            {
                                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                                try
                                {
                                    string SQL = "";
                                    if (this._FixedSourceServerConnection != null)
                                    {
                                        this._FixedSourceSQL = Sheet.SqlForLinkedSource(CB.Text,
                                            this._FixedSourceServerConnection, this.FixedSourceIWorkbenchUnit(),
                                            this._FixedSourceListInDatabase);

                                        // only retrieve data if not the whole database is used
                                        if (CB.Text.Length > 0)
                                        {
                                            SQL = Sheet.SqlForLinkedSource(CB.Text, this._FixedSourceServerConnection,
                                                this.FixedSourceIWorkbenchUnit(), this._FixedSourceListInDatabase);

                                            this._FixedSourceTable = new System.Data.DataTable();
                                            Microsoft.Data.SqlClient.SqlDataAdapter ad =
                                                new Microsoft.Data.SqlClient.SqlDataAdapter(SQL,
                                                    this._FixedSourceServerConnection.ConnectionString);
                                            ad.Fill(this._FixedSourceTable);
                                            CB.DataSource = this._FixedSourceTable;
                                            CB.DisplayMember = "DisplayText";
                                            CB.ValueMember = "URI";
                                        }
                                    }
                                    else if (this._FixedSourceWebservice != DwbServiceEnums.DwbService.None)
                                    {
                                        System.Data.DataTable dt = new System.Data.DataTable();

                                        if (string.IsNullOrEmpty(CB.Text))
                                        {
                                            MessageBox.Show("Please enter at least one character");
                                            return;
                                        }

                                        try
                                        {
                                            IDwbWebservice<DwbSearchResult, DwbSearchResultItem, DwbEntity> _api =
                                                DwbServiceProviderAccessor.GetDwbWebservice(_FixedSourceWebservice);
                                            var searchUrl = CreateSearchUrl(CB.Text, _api, _FixedSourceWebservice);
                                            if (_api == null || string.IsNullOrEmpty(searchUrl))
                                            {
                                                MessageBox.Show("No webservice defined");
                                                return;
                                            }

                                            var tt = await _api.CallWebServiceAsync<object>(searchUrl,
                                                DwbServiceEnums.HttpAction.GET);
                                            if (tt != null)
                                            {
                                                var clientSearchModel = _api.GetDwbApiSearchResultModel(tt);
                                                ReadDwbSearchModelInQueryTable(clientSearchModel, ref dt);
                                            }

                                            CB.DataSource = dt;
                                            CB.ValueMember = "_URI";
                                            CB.DisplayMember = "_DisplayText";
                                            //if (this._FixedSourceWebserviceOptions != null && this._FixedSourceWebserviceOptions.Count > 0)
                                            //    this._FixedSourceWebservice.getQueryResults(CB.Text, 50, ref dt, this._FixedSourceWebserviceOptions);
                                            //else
                                            //    this._FixedSourceWebservice.getQueryResults(CB.Text, 50, ref dt);
                                            //CB.DataSource = dt;
                                            //CB.DisplayMember = "_DisplayText";
                                            //CB.ValueMember = "_URI";
                                        }
                                        catch (InvalidOperationException ioe)
                                        {
                                            MessageBox.Show(
                                                "The record details cannot be displayed because the web service response is invalid.\r\n\r\n  " +
                                                "For more details on the error, see the error log file.\r\n\r\n",
                                                "Data Mapping Error", MessageBoxButtons.OK,
                                                MessageBoxIcon.Error);
                                            ExceptionHandling.WriteToErrorLogFile("FormSpreadSheet - EditingSetComboBoxSource, InvalidOprationException exception: " + ioe);
                                            return;
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(
                                                "The record details cannot be displayed because the web service response is invalid.\r\n\r\n  " +
                                                "For more details on the error, see the error log file.\r\n\r\n",
                                                "Data Mapping Error", MessageBoxButtons.OK,
                                                MessageBoxIcon.Error);
                                            ExceptionHandling.WriteToErrorLogFile("FormSpreadSheet - EditingSetComboBoxSourc, Exception exception: " + ex);
                                            return;
                                        }
                                    }
                                    else if (this._FixedSourceSQL != null && this._FixedSourceSQL.Length > 0)
                                    {
                                        SQL = this._FixedSourceSQL;
                                        if (CB.Text.Length > 0)
                                        {
                                            string DisplayColumn =
                                                this._Sheet.SelectedColumns()[this._FixedSourceSelectedCell.ColumnIndex]
                                                    .RemoteLinks[0].RemoteColumnBindings[0].Column.Name;

                                            SQL += " AND " + DisplayColumn + " LIKE '" + CB.Text + "%' ORDER BY " +
                                                   DisplayColumn;
                                            this.FixedSourceDataAdapter.SelectCommand.CommandText = SQL;
                                            this.FixedSourceDataAdapter.Fill(this._FixedSourceTable);
                                            CB.DataSource = this._FixedSourceTable;
                                            CB.DisplayMember = "DisplayText";
                                            CB.ValueMember = "URI";
                                        }
                                        else
                                            System.Windows.Forms.MessageBox.Show(
                                                "Please give at least the initial characters of the searched item");
                                    }
                                    else
                                    {
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("An error occured. Message: " + ex.Message);
                                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                }

                                this.Cursor = System.Windows.Forms.Cursors.Default;
                            }
                            else
                            {
                                this.EditingSetComboboxSourceFromGrid(CB);
                            }
                        }
                        else // take values from the grid
                        {
                            this.EditingSetComboboxSourceFromGrid(CB);
                        }
                    }
                }
            }
            catch (NotSupportedException notSupported)
            {
                MessageBox.Show("The webservice is not supported: " + notSupported.Message);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(notSupported);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string CreateSearchUrl(string userInput, IDwbWebservice<DwbSearchResult, DwbSearchResultItem, DwbEntity> _api, DwbServiceEnums.DwbService dwbService)
        {
            const int offset = 0; // default TODO Ariane if we want to add paging, then we can get/set the offset here
            const int MaxRecords = 50; // default TODO Ariane

            var queryRestriction = QueryRestriction(userInput);
            try
            {
                return _api?.DwbApiQueryUrlString(dwbService, queryRestriction, offset, MaxRecords) ?? string.Empty;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Argument Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }

        private string QueryRestriction(string userInputText)
        {
            // TODO Ariane do we need more?
            string Restriction = userInputText;

            Restriction = HttpUtility.UrlEncode(Restriction);
            return Restriction;
        }

        private void ReadDwbSearchModelInQueryTable(DwbSearchResult result, ref System.Data.DataTable dtQuery)
        {
            try
            {
                if (result is null)
                    return;

                dtQuery.Columns.Clear();
                var columns = new[]
                {
                    new { Name = "URI", Type = "System.String" },
                    new { Name = "DisplayText", Type = "System.String" }
                };
                foreach (var column in columns)
                {
                    dtQuery.Columns.Add(new System.Data.DataColumn(column.Name, Type.GetType(column.Type)));
                }

                foreach (var item in result.DwbApiSearchResponse)
                {
                    var row = dtQuery.NewRow();
                    row["URI"] = item._URL;
                    row["DisplayText"] = item._DisplayText;
                    dtQuery.Rows.Add(row);
                }

            }
            catch (Exception ex)
            {
#if DEBUG

                Console.WriteLine(ex.StackTrace);
#endif 
            }
        }

        /// <summary>
        /// Setting the source for a combobox with values from the grid
        /// </summary>
        /// <param name="CB">The combobox</param>
        private void EditingSetComboboxSourceFromGrid(System.Windows.Forms.ComboBox CB)
        {
            try
            {
                if (CB.DataSource != null)
                    CB.DataSource = null;
                CB.Items.Clear();
                System.Collections.Generic.SortedList<string, string> L = new System.Collections.Generic.SortedList<string, string>();
                foreach (System.Data.DataRow R in this._Sheet.DT().Rows)
                {
                    if (R[this._FixedSourceSelectedCell.ColumnIndex].ToString().Length > 0)
                    {
                        if (!L.ContainsKey(R[this._FixedSourceSelectedCell.ColumnIndex].ToString()))
                            L.Add(R[this._FixedSourceSelectedCell.ColumnIndex].ToString(), R[this._FixedSourceSelectedCell.ColumnIndex].ToString());
                    }
                }
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in L)
                    CB.Items.Add(KV.Key);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        //private void EditingSetComboboxSources(int ColumnIndex, ComboBoxStyle Style, System.Data.DataTable dtLookup)
        //{
        //    try
        //    {
        //        bool setSource = false;
        //        if (this._SelectedDataCell != null
        //            && ColumnIndex != this._SelectedDataCell.ColumnIndex)
        //            setSource = true;
        //        if (this.comboBoxReplace.DataSource == null)
        //            setSource = true;
        //        if (!setSource && this.comboBoxReplace.DataSource != null)
        //        {
        //            System.Data.DataTable dt = (System.Data.DataTable)this.comboBoxReplace.DataSource;
        //            if (dt.Rows.Count != dtLookup.Rows.Count)
        //                setSource = true;
        //        }
        //        if (setSource)
        //        {
        //            this.comboBoxReplace.DropDownStyle = Style;
        //            this.comboBoxReplaceWith.DropDownStyle = Style;
        //            this.comboBoxReplace.DataSource = dtLookup;
        //            this.comboBoxReplace.DisplayMember = dtLookup.Columns[0].ColumnName;
        //            this.comboBoxReplace.ValueMember = dtLookup.Columns[0].ColumnName;
        //            System.Data.DataTable dt = dtLookup.Copy();
        //            this.comboBoxReplaceWith.DataSource = dt;
        //            this.comboBoxReplaceWith.DisplayMember = dt.Columns[0].ColumnName;
        //            this.comboBoxReplaceWith.ValueMember = dt.Columns[0].ColumnName;
        //        }

        //        this.EditingCheckEditMode();

        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}
        
        #endregion

        #region Enabling the editing controls

        /// <summary>
        /// Setting the style for both editing comboboxes
        /// </summary>
        private void EditingSetComboboxStyle()
        {
            this.EditingSetComboboxStyle(this.comboBoxReplace);
            this.EditingSetComboboxStyle(this.comboBoxReplaceWith);
        }

        /// <summary>
        /// Setting the style for a combobox
        /// </summary>
        /// <param name="CB">The combobox for which the style should be set</param>
        private void EditingSetComboboxStyle(System.Windows.Forms.ComboBox CB)
        {
            CB.BackColor = System.Drawing.Color.White;
            if (this._FixedSourceSelectedCell != null) // Any cell selected
            {
                // Lookup source available
                if (this._Sheet.SelectedColumns()[this._FixedSourceSelectedCell.ColumnIndex].LookupSource != null)
                {
                    CB.DropDownStyle = ComboBoxStyle.DropDownList;
                }
                // Linked columns
                else if (this._Sheet.SelectedColumns()[this._FixedSourceSelectedCell.ColumnIndex].IsLinkedColumn())
                {
                    // source is fixed
                    if (this.FixedSourceSourceIsFixed())
                    {
                        CB.DropDownStyle = ComboBoxStyle.DropDown;
                        CB.BackColor = System.Drawing.Color.SkyBlue;
                    }
                    else
                    {
                        CB.DropDownStyle = ComboBoxStyle.DropDownList;
                    }
                }
                else // any other column
                {
                    CB.DropDownStyle = ComboBoxStyle.DropDown;
                }
            }
        }

        /// <summary>
        /// Enable editing controls in dependence of the selected column
        /// </summary>
        /// <param name="ColumnIndex">the index of the selected column, -1 = nothing selected, so editing will be disabled</param>
        private void EditingEnableEditing(int ColumnIndex)
        {
            bool AllowEdit = false;
            bool AllowAppend = false;

            try
            {
                // for not ReadOnly
                if (!this._Sheet.ReadOnly)
                {
                    // if anything is selected
                    if (ColumnIndex > -1
                        && this.dataGridView.SelectedCells != null
                        && this.dataGridView.SelectedCells.Count > 0
                        && this._Sheet.SelectedColumns()[ColumnIndex].DataTable().AllowUpdate())
                    {
                        // no colums that can not be changed etc.
                        if (this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Type() != DataTable.TableType.Lookup
                            && !this._Sheet.SelectedColumns()[ColumnIndex].IsIdentity
                            && !this._Sheet.SelectedColumns()[ColumnIndex].IsReadOnly()
                            && this._Sheet.SelectedColumns()[ColumnIndex].Column.DataType != "geography"
                            && this._Sheet.SelectedColumns()[ColumnIndex].Column.DataType != "datetime")
                        {
                            AllowEdit = true;// this._Sheet.SelectedColumns()[this.dataGridView.SelectedCells[0].ColumnIndex].DataTable().AllowUpdate();
                            AllowAppend = true;
                        }
                    }
                }

                this.buttonEdit.Enabled = AllowEdit;
                this.comboBoxReplace.Enabled = AllowEdit;
                this.comboBoxReplaceWith.Enabled = AllowEdit;
                this.buttonEditMode.Enabled = AllowEdit;
                this.buttonMarkWholeColumn.Enabled = AllowEdit;
                this.checkBoxMarkColumnRange.Enabled = AllowEdit;
                if (AllowEdit)
                {
                    switch (this._EditMode)
                    {
                        case EditMode.Append:
                        case EditMode.Calculate:
                        case EditMode.Prepend:
                            this.comboBoxReplace.Enabled = AllowAppend;
                            this.comboBoxReplaceWith.Enabled = false;
                            this.buttonEdit.Enabled = AllowAppend;
                            this.buttonMarkWholeColumn.Enabled = AllowAppend;
                            break;
                        case EditMode.Overwrite:
                            this.comboBoxReplace.Enabled = false;
                            this.comboBoxReplaceWith.Enabled = true;
                            break;
                        case EditMode.Clear:
                            this.comboBoxReplace.Enabled = false;
                            this.comboBoxReplaceWith.Enabled = false;
                            break;
                        case EditMode.Replace:
                            this.comboBoxReplace.Enabled = true;
                            this.comboBoxReplaceWith.Enabled = true;
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Enable editing controls in dependence of the selected column
        /// </summary>
        private void EditingEnableEditing()
        {
            int Index = -1;
            if (this._FixedSourceSelectedCell != null)// this.dataGridView.SelectedCells != null && this.dataGridView.SelectedCells.Count > 0)
                Index = this._FixedSourceSelectedCell.ColumnIndex;
            this.EditingEnableEditing(Index);
            return;
        }
        
        #endregion

        #endregion

        #region Mark column range

        private void buttonMarkWholeColumn_Click(object sender, EventArgs e)
        {
            this.MarkColumnRangeReset();
            try
            {
                int ColumnIndex = this.dataGridView.SelectedCells[0].ColumnIndex;
                foreach (System.Windows.Forms.DataGridViewCell Cell in dataGridView.SelectedCells)
                    Cell.Selected = false;
                System.Windows.Forms.DataGridViewColumn C = dataGridView.Columns[ColumnIndex];
                int iLines = dataGridView.Rows.Count;
                if (dataGridView.AllowUserToAddRows)
                    iLines--;
                for (int i = 0; i < iLines; i++)
                {
                    dataGridView.Rows[i].Cells[ColumnIndex].Selected = true;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private int? _MarkedColumnIndex = null;

        private void checkBoxMarkColumnRange_Click(object sender, EventArgs e)
        {
            try
            {
                this.MarkColumnRange = !this.MarkColumnRange;
                if (this.MarkColumnRange && this.dataGridView.SelectedCells.Count > 0)
                {
                    this._MarkedColumnIndex = this.dataGridView.SelectedCells[0].ColumnIndex;
                }
                else
                    this._MarkedColumnIndex = null;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private bool _MarkColumnRange = false;

        /// <summary>
        /// Marking the range of fields within a column, supress 
        /// </summary>
        public bool MarkColumnRange
        {
            get { return _MarkColumnRange; }
            set
            {
                _MarkColumnRange = value;
                if (this._MarkColumnRange)
                {
                    this.checkBoxMarkColumnRange.BackColor = System.Drawing.Color.White;
                    this.checkBoxMarkColumnRange.FlatAppearance.BorderSize = 1;
                    this.checkBoxMarkColumnRange.Checked = true;
                }
                else
                {
                    this.checkBoxMarkColumnRange.BackColor = System.Drawing.SystemColors.Control;
                    this.checkBoxMarkColumnRange.FlatAppearance.BorderSize = 0;
                    this.checkBoxMarkColumnRange.Checked = false;
                }
            }
        }

        private void MarkColumnRangeReset()
        {
            this.MarkColumnRange = false;
            //this.setMarkColorRangeControl();
        }

        #endregion

        #region Fixing the source for the ReplaceWith combobox

        #region Parameter

        // SQL set in configuration
        private string _FixedSourceSQL = "";

        // Module interface
        private DiversityWorkbench.IWorkbenchUnit _FixedSourceIWorkbenchUnit;

        // ServerConnection
        private DiversityWorkbench.ServerConnection _FixedSourceServerConnection;
        private string _FixedSourceListInDatabase = "";
            
        private bool _FixedSourceIsListInDatabase = false;

        private DWBServices.WebServices.DwbServiceEnums.DwbService _FixedSourceWebservice;

        // Settings for the current column
        private System.Collections.Generic.List<string> _FixedSourceSetting;

        // Parameters for editing comboboxes
        private Microsoft.Data.SqlClient.SqlDataAdapter _FixedSourceDataAdapter = null;
        private System.Data.DataTable _FixedSourceTable;
        private System.Collections.Generic.Dictionary<string, string> _FixedSourceValues;
        
        #endregion

        /// <summary>
        /// Check if one of the parameters of the FixSource (ServerConnection, Webservice, SQL) are set
        /// </summary>
        /// <returns></returns>
        private bool FixedSourceSourceIsFixed()
        {
            if (this._FixedSourceServerConnection != null)
            {
                return true;
            }
            else if (this._FixedSourceWebservice != DwbServiceEnums.DwbService.None)
            {
                return true;
            }
            else if (this._FixedSourceSQL != null && this._FixedSourceSQL.Length > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// If all columns linked to t fixed source are included in the selected columns of the spreadsheet
        /// </summary>
        /// <param name="ColumnIndex">The index of the linked column</param>
        /// <param name="MissingColumns">The list of missing columns</param>
        /// <returns>If all columns linked to the fixed source are included</returns>
        private bool FixedSourceLinkedColumnsIncluded(int ColumnIndex, ref System.Collections.Generic.List<string> MissingColumns)
        {
            bool IsIncluded = true;
            // Check if the column is a link column
            if (this._Sheet.SelectedColumns()[ColumnIndex].IsLinkedColumn())
            {
                System.Collections.Generic.Dictionary<string, string> MissingColumnsDisplayText = new Dictionary<string, string>();
                // Check if connected columns are available, 
                // e.g. for an ReferenceURI the ReferenceTitle must be included in the selected columns 
                // because otherwise the software can not enter the values retrieved from the remote source
                foreach (string L in this._Sheet.SelectedColumns()[ColumnIndex].LinkedColumns())
                {
                    bool IsVisible = false;
                    foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                    {
                        if ((DC.Value.Type() == DataColumn.ColumnType.Data || DC.Value.Type() == DataColumn.ColumnType.Link)
                            && (DC.Value.DisplayText == L || DC.Value.Name == L))
                        {
                            // Column detected
                            IsVisible = true;
                            break;
                        }
                    }
                    if (!IsVisible) // Column could not be found
                    {
                        IsIncluded = false;
                        foreach(System.Collections.Generic.KeyValuePair<string, DataTable> DT in this._Sheet.DataTables())
                        {
                            bool found = false;
                            foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in DT.Value.DataColumns())
                            {
                                if (DC.Value.DisplayText == L || DC.Value.Name == L)
                                {
                                    string Display = DC.Value.Name;
                                    if (DC.Value.DisplayText != DC.Value.Name)
                                        Display += " (= " + DC.Value.DisplayText + ") ";
                                    Display += " in " + DT.Value.Name;
                                    if (DT.Value.DisplayText != DT.Value.Name)
                                        Display += " (= " + DT.Value.DisplayText + ")";
                                    MissingColumnsDisplayText.Add(L, Display);
                                    found = true;
                                    break;
                                }
                            }
                            if (found)
                                break;
                        }
                        if (MissingColumnsDisplayText.ContainsKey(L))
                            MissingColumns.Add(MissingColumnsDisplayText[L]);
                        else
                            MissingColumns.Add(L);
                        //break;
                    }
                }
                if (IsIncluded 
                    && this._Sheet.SelectedColumns()[ColumnIndex].RemoteLinkDisplayColumn.Length > 0)
                {
                    string DisplayColumn = this._Sheet.SelectedColumns()[ColumnIndex].RemoteLinkDisplayColumn;
                    string Alias = this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias();
                    if (!this._Sheet.DataTables()[Alias].DataColumns()[DisplayColumn].IsVisible)
                    {
                        IsIncluded = false;
                        if (this._Sheet.DataTables()[Alias].DataColumns()[DisplayColumn].Name != this._Sheet.DataTables()[Alias].DataColumns()[DisplayColumn].DisplayText)
                            DisplayColumn += " (= " + this._Sheet.DataTables()[Alias].DataColumns()[DisplayColumn].DisplayText + ")";
                        MissingColumns.Add(DisplayColumn);
                    }
                }
            }
            return IsIncluded;
        }

        /// <summary>
        /// Reset all fixing parameters
        /// </summary>
        private void FixingAndEditingReset()
        {
            this._FixedSourceSelectedCell = null;
            this.FixedSourceParametersReset();
            this.FixedSourceSetControls();
            this.EditingEnableEditing(-1);
            this.EditingResetComboboxSources();
        }

        /// <summary>
        /// Set all parameter linked to the fixed source to null
        /// </summary>
        private void FixedSourceParametersReset()
        {
            this._FixedSourceIWorkbenchUnit = null;
            this._FixedSourceServerConnection = null;
            this._FixedSourceWebservice = DwbServiceEnums.DwbService.None;
            this._FixedSourceSetting = null;
            this._FixedSourceSQL = null;
        }

        private DiversityWorkbench.IWorkbenchUnit FixedSourceIWorkbenchUnit()
        {
            if (this._FixedSourceSelectedCell != null && this._FixedSourceIWorkbenchUnit == null)
            {
                DiversityWorkbench.Spreadsheet.DataColumn DC = this._Sheet.SelectedColumns()[this._FixedSourceSelectedCell.ColumnIndex];
                switch (DC.LinkedModule)
                {
                    case RemoteLink.LinkedModule.DiversityAgents:
                        DiversityWorkbench.Agent A = new Agent(DiversityWorkbench.Settings.ServerConnection);
                        this._FixedSourceIWorkbenchUnit = A;
                        break;
                    case RemoteLink.LinkedModule.DiversityGazetteer:
                        DiversityWorkbench.Gazetteer G = new Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                        this._FixedSourceIWorkbenchUnit = G;
                        break;
                    case RemoteLink.LinkedModule.DiversityProjects:
                        DiversityWorkbench.Project P = new Project(DiversityWorkbench.Settings.ServerConnection);
                        this._FixedSourceIWorkbenchUnit = P;
                        break;
                    case RemoteLink.LinkedModule.DiversityReferences:
                        DiversityWorkbench.Reference R = new Reference(DiversityWorkbench.Settings.ServerConnection);
                        this._FixedSourceIWorkbenchUnit = R;
                        break;
                    case RemoteLink.LinkedModule.DiversitySamplingPlots:
                        DiversityWorkbench.SamplingPlot S = new SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                        this._FixedSourceIWorkbenchUnit = S;
                        break;
                    case RemoteLink.LinkedModule.DiversityScientificTerms:
                        DiversityWorkbench.ScientificTerm T = new ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                        this._FixedSourceIWorkbenchUnit = T;
                        break;
                    case RemoteLink.LinkedModule.DiversityTaxonNames:
                        DiversityWorkbench.TaxonName N = new TaxonName(DiversityWorkbench.Settings.ServerConnection);
                        this._FixedSourceIWorkbenchUnit = N;
                        break;
                }
            }
            return this._FixedSourceIWorkbenchUnit;
        }

        private Microsoft.Data.SqlClient.SqlDataAdapter FixedSourceDataAdapter
        {
            get
            {
                if (this._FixedSourceDataAdapter == null)
                {
                    try
                    {
                        this._FixedSourceDataAdapter = new Microsoft.Data.SqlClient.SqlDataAdapter(this._FixedSourceSQL, DiversityWorkbench.Settings.ConnectionString);
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                return this._FixedSourceDataAdapter;
            }
        }

        /// <summary>
        /// Setting the server connetion and setting the webservice to null
        /// </summary>
        /// <param name="ServerConnection">The server connection for the fixed source</param>
        //private void FixedSourceSetServerConnection(DiversityWorkbench.ServerConnection ServerConnection)
        //{
        //    this._FixedSourceServerConnection = ServerConnection;
        //    this._FixedSourceWebservice = null;
        //    this._FixedSourceWebserviceOptions = null;
        //}

        //private void FixSource(DiversityWorkbench.ServerConnection ServerConnection, int ProjectID)
        //{
        //    this._FixSourceServerConnection = ServerConnection;
        //    this._FixSourceServerConnection.ProjectID = ProjectID;
        //    this._FixSourceWebservice = null;
        //    this._FixSourceWebserviceOptions = null;
        //}

        //private void FixSource(DiversityWorkbench.Webservice Webservice)
        //{
        //    this._FixSourceWebservice = Webservice;
        //    this._FixSourceServerConnection = null;
        //}

        private void FixedSourceSetParameters()//string ServerConnection)
        {
            try
            {
                this._FixedSourceIWorkbenchUnit = this.FixedSourceIWorkbenchUnit();
                bool SourceFound = DiversityWorkbench.WorkbenchUnit.FixedSourceSetParameters(
                    ref this._FixedSourceIWorkbenchUnit
                    , ref this._FixedSourceServerConnection
                    , ref this._FixedSourceWebservice
                    //, ref this._FixedSourceWebserviceOptions
                    , ref this.buttonFixSource
                    , ref this.comboBoxReplaceWith, this.toolTip);//false;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private enum ValueState { LocalValue, RemoteValue, NoValue };
        private enum ConnectionState { NotConnected, Connected };
        private enum BindingState { Binding_Unit, NoBinding_NoUnit, Binding_NoUnit, NoBinding_Unit };
        private enum UriAvailableState { Available, NoAccess };

        /// <summary>
        /// Setting the controls associated with fixed sources according to the current settings
        /// </summary>
        private void FixedSourceSetControls()
        {
            try
            {
                if (this._FixedSourceSelectedCell != null)
                {
                    if (this._Sheet.SelectedColumns()[this._FixedSourceSelectedCell.ColumnIndex].IsLinkedColumn())
                    {
                        if (!this.FixedSourceSourceIsFixed()) 
                        {
                            this.buttonFixSource.Image = DiversityWorkbench.Properties.Resources.Pin_3Gray;// this.imageList.Images[1];
                            this.toolTip.SetToolTip(this.buttonFixSource, "Set the source");
                            this.buttonFixSource.Tag = null;
                        }
                        else
                        {
                            this.buttonFixSource.Image = DiversityWorkbench.Properties.Resources.Pin_3;// this.imageList.Images[0];
                            string Source = "";
                            if (this._FixedSourceServerConnection != null)
                            {
                                Source = this._FixedSourceServerConnection.DisplayText;
                                if (this._FixedSourceServerConnection.Project != null && this._FixedSourceServerConnection.Project.Length > 0)
                                    Source += " (" + this._FixedSourceServerConnection.Project + ")";
                            }
                            else if (this._FixedSourceWebservice != DwbServiceEnums.DwbService.None)
                                Source = this._FixedSourceWebservice.ToString();
                            this.toolTip.SetToolTip(this.buttonFixSource, "Source: " + Source);
                            this.buttonFixSource.Tag = "Source: " + Source;
                            this.toolTip.SetToolTip(this.comboBoxReplaceWith, "Search for values in " + Source);
                        }
                        this.EditingSetComboboxStyle(this.comboBoxReplaceWith);
                    }
                    else
                    {
                        this.buttonFixSource.Tag = "X";
                        this.buttonFixSource.Image = DiversityWorkbench.Properties.Resources.Pin_3Empty;
                        this.toolTip.SetToolTip(this.buttonFixSource, "No source");
                        this.toolTip.SetToolTip(this.comboBoxReplaceWith, "No source");
                    }
                }
                else
                {
                    this.buttonFixSource.Tag = "X";
                    this.buttonFixSource.Image = DiversityWorkbench.Properties.Resources.Pin_3Empty;
                    this.toolTip.SetToolTip(this.buttonFixSource, "No source");
                    this.toolTip.SetToolTip(this.comboBoxReplaceWith, "No source");
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Setting the source for the combobox ReplaceWith
        /// </summary>
        //private void FixSourceSetComboBoxReplaceWithSource()
        //{
        //    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
        //    try
        //    {
        //        string SQL = "";
        //        if (this._FixSourceServerConnection != null)
        //        {
        //            this._FixSourceSQL = Sheet.SqlForLinkedSource(this.comboBoxReplaceWith.Text, this._FixSourceServerConnection, this.FixSourceIWorkbenchUnit(), this._FixSourceListInDatabase);

        //            // only retrieve data if not the whole database is used
        //            if (this.comboBoxReplaceWith.Text.Length > 0)
        //            {
        //                SQL = Sheet.SqlForLinkedSource(this.comboBoxReplaceWith.Text, this._FixSourceServerConnection, this.FixSourceIWorkbenchUnit(), this._FixSourceListInDatabase);

        //                this._FixSourceTable = new System.Data.DataTable();
        //                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._FixSourceServerConnection.ConnectionString);
        //                ad.Fill(this._FixSourceTable);
        //                this.comboBoxReplaceWith.DataSource = this._FixSourceTable;
        //                this.comboBoxReplaceWith.DisplayMember = "DisplayText";
        //                this.comboBoxReplaceWith.ValueMember = "URI";
        //            }
        //        }
        //        else if (this._FixSourceWebservice != null)
        //        {
        //            System.Data.DataTable dt = new System.Data.DataTable();
        //            if (this._FixSourceWebserviceOptions != null && this._FixSourceWebserviceOptions.Count > 0)
        //                this._FixSourceWebservice.getQueryResults(this.comboBoxReplaceWith.Text, 50, ref dt, this._FixSourceWebserviceOptions);
        //            else
        //                this._FixSourceWebservice.getQueryResults(this.comboBoxReplaceWith.Text, 50, ref dt);
        //            this.comboBoxReplaceWith.DataSource = dt;
        //            this.comboBoxReplaceWith.DisplayMember = "_DisplayText";
        //            this.comboBoxReplaceWith.ValueMember = "_URI";
        //        }
        //        else if (this._FixSourceSQL != null && this._FixSourceSQL.Length > 0)
        //        {
        //            SQL = this._FixSourceSQL;
        //            if (this.comboBoxReplaceWith.Text.Length > 0)
        //            {
        //                string DisplayColumn = this._Sheet.SelectedColumns()[this.dataGridView.SelectedCells[0].ColumnIndex].RemoteLinks[0].RemoteColumnBindings[0].Column.Name;

        //                SQL += " AND " + DisplayColumn + " LIKE '" + this.comboBoxReplaceWith.Text + "%' ORDER BY " + DisplayColumn;
        //                this.FixSourceDataAdapter.SelectCommand.CommandText = SQL;
        //                this.FixSourceDataAdapter.Fill(this._FixSourceTable);
        //                this.comboBoxReplaceWith.DataSource = this._FixSourceTable;
        //                this.comboBoxReplaceWith.DisplayMember = "DisplayText";
        //                this.comboBoxReplaceWith.ValueMember = "URI";
        //            }
        //            else
        //                System.Windows.Forms.MessageBox.Show("Please give at least the initial characters of the searched item");
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    this.Cursor = System.Windows.Forms.Cursors.Default;
        //}

        /// <summary>
        /// Setting the value for _FixedSourceSetting in dependence of the selected cell
        /// </summary>
        /// <param name="ColumnIndex">Index of the column</param>
        /// <param name="RowIndex">Index of row as sources may depend on values, e.g. Taxa depend on taxonomic group. May be null if not needed</param>
        private void FixedSourceSetFixedSourceSettings(int ColumnIndex, int RowIndex)
        {
            System.Collections.Generic.List<string> Setting = this._Sheet.SelectedColumns()[ColumnIndex].FixedSourceGetSetting();
            bool Fixed = false;

            if (ColumnIndex > -1 && this._Sheet.SelectedColumns().ContainsKey(ColumnIndex))
            {
                if (this._Sheet.SelectedColumns()[ColumnIndex].FixedSourceNeedsValueFromData() && Setting.Last().IndexOf(".") > -1)
                {
                    string Value = this.FixedSourceGetValueFromData(Setting, ColumnIndex, RowIndex);
                    this._FixedSourceSetting = this._Sheet.SelectedColumns()[ColumnIndex].FixedSourceGetSetting(Value);
                    Fixed = this._Sheet.SelectedColumns()[ColumnIndex].FixedSourceIsFixed(Value);
                }
                else
                {
                    this._FixedSourceSetting = this._Sheet.SelectedColumns()[ColumnIndex].FixedSourceGetSetting();
                    Fixed = this._Sheet.SelectedColumns()[ColumnIndex].FixedSourceIsFixed();
                }
                this.FixedSourcesSetSettings(this._FixedSourceSetting);//NewSetting);//.setFixSourceControls();
            }

            if (!Fixed)
            {
                this.FixedSourceRelease();
            }

            this._FixedSourceValues = null;
        }

        /// <summary>
        /// Getting values for the settings from the data
        /// </summary>
        /// <param name="Settings">The list of settings</param>
        /// <param name="ColumnIndex">Index of the column of the fixed source</param>
        /// <param name="RowIndex">Index of the row in the spreadsheet where the data should be taken from</param>
        /// <returns>The value found in the spreadsheet cell</returns>
        private string FixedSourceGetValueFromData(System.Collections.Generic.List<string> Settings, int ColumnIndex, int RowIndex)
        {
            string Value = "";
            if (Settings.Last().IndexOf(".") > -1) // the last value of the setting still contains the TableAlias and the ColumnName where the data should be taken from
            {
                string[] Values = Settings.Last().Split(new char[] { '.' });
                if (Values.Length == 2)
                {
                    string ValueFromDataAlias = Values[0]; //Settings.Last().Substring(0, Settings.Last().IndexOf("."));
                    string ValueFromDataColumn = Values[1]; // Settings.Last().Substring(Settings.Last().IndexOf(".") + 1);
                    this._Sheet.SelectedColumns()[ColumnIndex].FixedSourceValueFromDataTableAlias = ValueFromDataAlias;
                    this._Sheet.SelectedColumns()[ColumnIndex].FixedSourceValueFromDataColumnName = ValueFromDataColumn;
                    Value = this.FixedSourceGetValueFromData(ValueFromDataAlias, ValueFromDataColumn, RowIndex);
                }
            }
            else if (this._Sheet.SelectedColumns()[ColumnIndex].FixedSourceValueFromDataTableAlias.Length > 0
                && this._Sheet.SelectedColumns()[ColumnIndex].FixedSourceValueFromDataColumnName.Length > 0)
            {
                Value = this.FixedSourceGetValueFromData(
                    this._Sheet.SelectedColumns()[ColumnIndex].FixedSourceValueFromDataTableAlias, 
                    this._Sheet.SelectedColumns()[ColumnIndex].FixedSourceValueFromDataColumnName, 
                    RowIndex);
            }
            else
                Value = Settings.Last();
            return Value;
        }

        /// <summary>
        /// Getting a value needed for the determiation of a fixed source, e.g. if Identification Sources are linked to the taxonomic group, the taxonomic group must be taken from the data
        /// </summary>
        /// <param name="TableAlias">The alias of the table containing the value</param>
        /// <param name="ColumnName">The name of the columne containing the value</param>
        /// <param name="RowIndex">The index of the row within the spreadsheet</param>
        /// <returns>The value found in the spreadsheet cell</returns>
        private string FixedSourceGetValueFromData(string TableAlias, string ColumnName, int RowIndex)
        {
            string Value = "";
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (DC.Value.DataTable().Alias() == TableAlias && DC.Value.Name == ColumnName)
                {
                    Value = this.dataGridView.Rows[(int)RowIndex].Cells[DC.Key].Value.ToString();
                    break;
                }
            }
            return Value;
        }

        /// <summary>
        /// Setting the connection values for a fixed source according to the settings
        /// </summary>
        /// <param name="Settings">List of the settings down to the final (resolved) value</param>
        private void FixedSourcesSetSettings(System.Collections.Generic.List<string> Settings)
        {
            try
            {
                if (this._FixedSourceSetting == null)
                    this._FixedSourceSetting = Settings;
                DiversityWorkbench.UserSettings U = new DiversityWorkbench.UserSettings();
                string Source = "";
                string ProjectID = "";
                string Project = "";
                Source = U.GetSetting(Settings);
                if (Source.Length > 0)
                {
                    this.FixedSourceSetParameters();//Source);
                }
                else
                {
                    Source = U.GetSetting(Settings, "Database");
                    if (Source.Length == 0)
                    {
                        Source = U.GetSetting(Settings, "Webservice");
                        if (Source.Length == 0)
                        {
                            this.FixedSourceRelease();
                        }
                        else
                        {
                            this.FixedSourceSetConnection(Source, "", "", "", true);
                        }
                    }
                    else
                    {
                        ProjectID = U.GetSetting(Settings, "ProjectID");
                        Project = U.GetSetting(Settings, "Project");
                        string Module = U.GetSetting(Settings, "Module");
                        if (Module.Length > 0 && Module == "DiversityCollectionCache")
                            this.FixedSourceSetConnection(Source, ProjectID, Project, Module);
                        else
                            this.FixedSourceSetConnection(Source, ProjectID, Project);

                        // Aufruf unklar - Webservice steht oben

                        //System.Collections.Generic.Dictionary<string, string> Options = U.GetSettingOptions(Settings, this.FixedSourceWebService());
                        //foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Options)
                        //{
                        //    this.FixedSourceSetWebserviceOptions(KV.Key, KV.Value);
                        //}
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Setting the connection parameters for the fixed source
        /// </summary>
        /// <param name="ServerConnection"></param>
        /// <param name="ProjectID"></param>
        /// <param name="Project"></param>
        /// <param name="Module"></param>
        /// <param name="IsWebservice"></param>
        private void FixedSourceSetConnection(string ServerConnection, string ProjectID, string Project, string Module = "", bool IsWebservice = false)
        {
            if (this._FixedSourceSelectedCell == null)
                return;
            try
            {
                DataColumn DC = this._Sheet.SelectedColumns()[this._FixedSourceSelectedCell.ColumnIndex];
                RemoteLink RL = null;
                if (DC.RemoteLinks == null && DC.LinkedModule != null)
                {
                    RL = new RemoteLink(DC.LinkedModule);
                }
                else if (DC.RemoteLinks != null)
                {
                    RL = new RemoteLink(DC.RemoteLinks[0].LinkedToModule);
                }
                if (RL != null)
                {
                    if (IsWebservice)
                    {
                        switch (ServerConnection.ToLower())
                        {
                            //case "rll":
                            //    DiversityWorkbench.WebserviceRLL RLL = new WebserviceRLL();
                            //    this._FixedSourceWebservice = RLL;
                            //    break;
                            case "catalogueoflife":
                                this._FixedSourceWebservice = DwbServiceEnums.DwbService.CatalogueOfLife;
                                break;
                            case "indexfungorum":
                                this._FixedSourceWebservice = DwbServiceEnums.DwbService.IndexFungorum;
                                break;
                            case "pesi":
                                this._FixedSourceWebservice = DwbServiceEnums.DwbService.PESI;
                                break;
                            case "mycobank":
                                this._FixedSourceWebservice = DwbServiceEnums.DwbService.Mycobank;
                                break;
                            case "gbifspecies":
                                this._FixedSourceWebservice = DwbServiceEnums.DwbService.GBIFSpecies;
                                break;
                            case "worms":
                                this._FixedSourceWebservice = DwbServiceEnums.DwbService.WoRMS;
                                break;
                            case "gfbioterminology":
                                this._FixedSourceWebservice = DwbServiceEnums.DwbService.GFBioTerminologyService;
                                break;
                                //case "palaeodb":
                                //    DiversityWorkbench.PalaeoDB PDB = new PalaeoDB();
                                //    this._FixedSourceWebservice = PDB;
                                //    break
                                //                case "prokaryoticnomenclature":
                                //                    DiversityWorkbench.WebserviceGfbioProkaryoticNomenclature PNC = new WebserviceGfbioProkaryoticNomenclature();
                                //                    this._FixedSourceWebservice = PNC;
                                //                    break;
                                //               
                        }

                    }
                    else
                    {
                        switch (RL.LinkedToModule)
                        {
                            case RemoteLink.LinkedModule.DiversityTaxonNames:
                                if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].ServerConnectionList().ContainsKey(ServerConnection))
                                {
                                    this._FixedSourceServerConnection = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].ServerConnectionList()[ServerConnection];
                                    goto default;
                                }
                                else if (Module == "DiversityCollectionCache")
                                {
                                    this._FixedSourceServerConnection = new ServerConnection(DiversityWorkbench.Settings.ConnectionString);
                                    this._FixedSourceServerConnection.DatabaseName = ServerConnection;
                                    this._FixedSourceServerConnection.ModuleName = Module;
                                    goto default;
                                }
                                else
                                {
                                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].AccessibleDatabasesAndServicesOfModule())
                                    {
                                        if (KV.Key.ToLower() == ServerConnection.ToLower())
                                        {
                                            DiversityWorkbench.WorkbenchUnit.FixedSourceWebservice(KV.Key, ref this._FixedSourceServerConnection, ref this._FixedSourceWebservice);
                                        }
                                    }
                                }
                                DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                                if (Module == "DiversityCollectionCache")
                                    T.ServerConnection.ModuleName = Module;
                                this._FixedSourceIWorkbenchUnit = T;
                                break;
                            case RemoteLink.LinkedModule.DiversityAgents:
                                if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityAgents"].ServerConnectionList().ContainsKey(ServerConnection))
                                {
                                    this._FixedSourceServerConnection = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityAgents"].ServerConnectionList()[ServerConnection];
                                    goto default;
                                }
                                break;
                            case RemoteLink.LinkedModule.DiversityGazetteer:
                                if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnectionList().ContainsKey(ServerConnection))
                                {
                                    this._FixedSourceServerConnection = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnectionList()[ServerConnection];
                                    goto default;
                                }
                                break;
                            case RemoteLink.LinkedModule.DiversityReferences:
                                if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityReferences"].ServerConnectionList().ContainsKey(ServerConnection))
                                {
                                    this._FixedSourceServerConnection = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityReferences"].ServerConnectionList()[ServerConnection];
                                    goto default;
                                }
                                else
                                {
                                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in
                                             DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[
                                                 "DiversityReferences"].AccessibleDatabasesAndServicesOfModule())
                                    {
                                        if (KV.Key.ToLower() == ServerConnection.ToLower())
                                        {
                                            DiversityWorkbench.WorkbenchUnit.FixedSourceWebservice(KV.Key,
                                                ref this._FixedSourceServerConnection, ref this._FixedSourceWebservice);
                                        }
                                    }
                                }

                                // Ariane replaced with above, to test if we have implemented rll
                                //else if (ServerConnection == "RLL")
                                //{
                                //    DiversityWorkbench.WebserviceRLL W = new WebserviceRLL();
                                //    this._FixedSourceWebservice = W;
                                //}
                                break;
                            case RemoteLink.LinkedModule.DiversitySamplingPlots:
                                if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversitySamplingPlots"].ServerConnectionList().ContainsKey(ServerConnection))
                                {
                                    this._FixedSourceServerConnection = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversitySamplingPlots"].ServerConnectionList()[ServerConnection];
                                    goto default;
                                }
                                break;
                            case RemoteLink.LinkedModule.DiversityScientificTerms:
                                if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityScientificTerms"].ServerConnectionList().ContainsKey(ServerConnection))
                                {
                                    this._FixedSourceServerConnection = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityScientificTerms"].ServerConnectionList()[ServerConnection];
                                    goto default;
                                }
                                break;
                            default:
                                int iProjectID;
                                if (ProjectID.Length > 0 && int.TryParse(ProjectID, out iProjectID))
                                {
                                    this._FixedSourceServerConnection.ProjectID = iProjectID;
                                    this._FixedSourceServerConnection.Project = Project;
                                }
                                break;
                        }
                    }
                }
                this.FixedSourceSetControls();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private DiversityWorkbench.ServerConnection FixedSourceServerConnection() { return this._FixedSourceServerConnection; }
        private DwbServiceEnums.DwbService FixedSourceWebService() { return this._FixedSourceWebservice; }

        private void FixedSourceRelease()
        {
            this._FixedSourceServerConnection = null;
            this._FixedSourceWebservice = DwbServiceEnums.DwbService.None;
            //this._FixedSourceWebserviceOptions = null;
            this.FixedSourceSetControls();
        }

        private void buttonFixSource_Click(object sender, EventArgs e)
        {
            if (this.buttonFixSource.Tag != null &&
                this.buttonFixSource.Tag.ToString() == "X")
                return;
            try
            {
                this._FixedSourceIWorkbenchUnit = this.FixedSourceIWorkbenchUnit();
                if (DiversityWorkbench.WorkbenchUnit.FixedSourceSetParameters(
                    ref this._FixedSourceIWorkbenchUnit
                    , ref this._FixedSourceServerConnection
                    , ref this._FixedSourceWebservice
                    //, ref this._FixedSourceWebserviceOptions
                    , ref this.buttonFixSource
                    , ref this.comboBoxReplaceWith
                    , this.toolTip))
                {
                    this.FixedSourceSetControls();
                    if (this._FixedSourceServerConnection != null && this._FixedSourceSetting == null && this._FixedSourceSelectedCell != null)
                    {
                        this._FixedSourceSetting = new List<string>();
                        this._FixedSourceSetting.Add("ModuleSource");
                        this._FixedSourceSetting.Add(this._Sheet.SelectedColumns()[this._FixedSourceSelectedCell.ColumnIndex].DataTable().Name);
                        this._FixedSourceSetting.Add(this._Sheet.SelectedColumns()[this._FixedSourceSelectedCell.ColumnIndex].Name);
                    }
                    if (this._FixedSourceSetting != null)
                    {
                        if (this._FixedSourceServerConnection != null)
                        {
                            if (this._FixedSourceServerConnection.Project != null)
                                DiversityWorkbench.WorkbenchUnit.SaveSetting(this._FixedSourceSetting, this._FixedSourceServerConnection, this._FixedSourceWebservice);
                        }
                        else
                            DiversityWorkbench.WorkbenchUnit.SaveSetting(this._FixedSourceSetting, this._FixedSourceServerConnection, this._FixedSourceWebservice);
                        //DiversityWorkbench.WorkbenchUnit.SaveSetting(this._FixedSourceSetting, this._FixedSourceServerConnection, this._FixedSourceWebservice, this._FixedSourceWebserviceOptions);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void FixedSourceSetValues(System.Data.DataRow R)
        {
            try
            {
                this._FixedSourceValues = new Dictionary<string, string>();
                foreach (System.Data.DataColumn C in R.Table.Columns)
                {
                    if (!R[C.ColumnName].Equals(System.DBNull.Value) && R[C.ColumnName].ToString().Length > 0)
                    {
                        this._FixedSourceValues.Add(C.ColumnName, R[C.ColumnName].ToString());
                    }
                }
                if (this._FixedSourceValues.ContainsKey("ID"))
                {
                    int ID;
                    if (int.TryParse(this._FixedSourceValues["ID"], out ID))
                    {
                        DiversityWorkbench.ServerConnection SC = new ServerConnection(this._FixedSourceServerConnection.ConnectionString);
                        this.FixedSourceIWorkbenchUnit().setServerConnection(SC);
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.FixedSourceIWorkbenchUnit().UnitValues(ID))
                        {
                            if (!this._FixedSourceValues.ContainsKey(KV.Key) && !KV.Key.StartsWith("_"))
                            {
                                this._FixedSourceValues.Add(KV.Key, KV.Value);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #region Help request

        private void buttonFixSource_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            this.ShowFixSourceHelp();
        }

        private void ShowFixSourceHelp()
        {
            if (this.buttonFixSource.Tag == null)
                System.Windows.Forms.MessageBox.Show("No source set");
            else
                System.Windows.Forms.MessageBox.Show(this.buttonFixSource.Tag.ToString());
        }

        #endregion

        #endregion
        
        #endregion

        #region Auxillary

        private void dataGridViewDataCell_Click(System.Windows.Forms.DataGridViewCell Cell, Spreadsheet.Sheet.Grid SourceGrid)
        {
            try
            {
                if (SourceGrid == Sheet.Grid.Filter)
                {
                    //bool LinkedToFixedModule = false;
                    if (this._Sheet.SelectedColumns()[Cell.ColumnIndex].LinkedModule != RemoteLink.LinkedModule.None && Cell.Value.ToString().Length == 0)
                    {
                        if (this._Sheet.SelectedColumns()[Cell.ColumnIndex].IsLinkedColumn())
                        {
                            string Error = "";
                            System.Collections.Generic.List<string> Settings = null;
                            if (this._Sheet.SelectedColumns()[Cell.ColumnIndex].FixedSourceIsFixed())
                            {
                            }
                            this._Sheet.setLinkedColumnValues(Settings, null, DwbServiceEnums.DwbService.None, null, Cell, SourceGrid, ref Error);
                            this._Sheet.SelectedColumns()[Cell.ColumnIndex].FilterValue = Cell.Value.ToString();

                            // set FilterModuleLinkRoot for a single value retrieved from a remote query
                            if (Cell.Value.ToString().IndexOf(" ") == -1 
                                && Cell.Value.ToString().IndexOf("\r\n") == -1 
                                && Cell.Value.ToString().Length > 0)
                                this._Sheet.SelectedColumns()[Cell.ColumnIndex].FilterModuleLinkRoot = Cell.Value.ToString();

                            if (this._Sheet.SelectedColumns()[Cell.ColumnIndex].FilterModuleLinkRoot.Length > 0)
                            {
                                DiversityWorkbench.Spreadsheet.FormColumnFilter f = new FormColumnFilter(this._Sheet.SelectedColumns()[Cell.ColumnIndex]);
                                f.ShowDialog();
                                f.Dispose();
                            }
                        }
                    }
                    else
                    {
                        System.Collections.Generic.List<string> FilterValueList = new List<string>();
                        foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.Rows)
                        {
                            string Filter = "";
                            if (!R.Cells[Cell.ColumnIndex].Value.Equals(System.DBNull.Value))
                                Filter = R.Cells[Cell.ColumnIndex].Value.ToString();
                            if (Filter.Length > 0 
                                && !FilterValueList.Contains(Filter)
                                && !FilterValueList.Contains(Filter.ToLower())
                                && !FilterValueList.Contains(Filter.ToUpper()))
                                FilterValueList.Add(Filter);
                        }
                        if (FilterValueList.Count > 0)
                        {
                            FilterValueList.Sort();
                            DiversityWorkbench.Spreadsheet.FormColumnFilter f = new FormColumnFilter(this._Sheet.SelectedColumns()[Cell.ColumnIndex], FilterValueList);
                            try
                            {
                                f.ShowDialog();
                                f.Dispose();
                            }
                            catch (System.Exception ex)
                            {
                            }
                        }
                        else
                        {
                            DiversityWorkbench.Spreadsheet.FormColumnFilter f = new FormColumnFilter(this._Sheet.SelectedColumns()[Cell.ColumnIndex]);
                            f.ShowDialog();
                            f.Dispose();
                        }
                    }

                    string DisplayText = "";
                    if (this._Sheet.SelectedColumns()[Cell.ColumnIndex].OrderDirection != DataColumn.OrderByDirection.none)
                    {
                        DisplayText = this._Sheet.SelectedColumns()[Cell.ColumnIndex].OrderSequence.ToString();
                        switch (this._Sheet.SelectedColumns()[Cell.ColumnIndex].OrderDirection)
                        {
                            case DataColumn.OrderByDirection.ascending:
                                DisplayText += "↑  ";
                                break;
                            case DataColumn.OrderByDirection.descending:
                                DisplayText += "↓  ";
                                break;
                        }
                    }
                    if (this._Sheet.SelectedColumns()[Cell.ColumnIndex].FilterValue != null && this._Sheet.SelectedColumns()[Cell.ColumnIndex].FilterValue.Length > 0)
                    {
                        DisplayText += this._Sheet.SelectedColumns()[Cell.ColumnIndex].FilterOperator + " " + this._Sheet.SelectedColumns()[Cell.ColumnIndex].FilterValue;
                    }
                    else if (this._Sheet.SelectedColumns()[Cell.ColumnIndex].FilterOperator == "Ø")
                    {
                    }
                    else if (this._Sheet.SelectedColumns()[Cell.ColumnIndex].FilterOperator == "•")
                    {
                        DisplayText += this._Sheet.SelectedColumns()[Cell.ColumnIndex].FilterOperator + " (Not empty)";
                    }
                    if (DisplayText.Length > 0)
                        this.dataGridViewFilter.Rows[0].Cells[Cell.ColumnIndex].Value = DisplayText;
                    else
                        this.dataGridViewFilter.Rows[0].Cells[Cell.ColumnIndex].Value = "";
                }
                else
                {
                    // handling all columns with a lookup source behind
                    if (this._Sheet.SelectedColumns()[Cell.ColumnIndex].LookupSource != null)
                    {
                        string Preselection = "";
                        try
                        {
                            switch (SourceGrid)
                            {
                                case Sheet.Grid.Adding:
                                    Preselection = this.dataGridViewAdding.Rows[0].Cells[Cell.ColumnIndex].Value.ToString();
                                    break;
                                case Sheet.Grid.Data:
                                    Preselection = this.dataGridView.Rows[Cell.RowIndex].Cells[Cell.ColumnIndex].Value.ToString();
                                    break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                        }
                        System.Data.DataTable DtValues = this.LookupSourceForColumn(Cell.ColumnIndex, Preselection, "Display");

                        DiversityWorkbench.Forms.FormGetStringFromList f =
                            new DiversityWorkbench.Forms.FormGetStringFromList(
                                DtValues,
                                "Display",
                                "Value",
                                dataGridView.Columns[Cell.ColumnIndex].HeaderText,
                                "Select value for " + dataGridView.Columns[Cell.ColumnIndex].HeaderText,
                                Preselection,
                                false,
                                true);
                        f.ShowInTaskbar = true;
                        f.ShowDialog();
                        if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            Cell.Value = f.SelectedValue;
                        }
                        f.Dispose();
                    }

                    // internal relations
                    else if (this._Sheet.SelectedColumns()[Cell.ColumnIndex].Column.ForeignRelations.Count > 0 &&
                        this._Sheet.SelectedColumns()[Cell.ColumnIndex].InternalRelationDisplay != null &&
                        this._Sheet.SelectedColumns()[Cell.ColumnIndex].InternalRelationDisplay.Length > 0)
                    {
                        if (SourceGrid == Sheet.Grid.Data)
                        {
                            string PK_Exclude = this._Sheet.SelectedColumns()[Cell.ColumnIndex].Column.ForeignRelationColumn;
                            string PK_ExcludeColumn = this._Sheet.SelectedColumns()[Cell.ColumnIndex].DataTable().DataColumns()[PK_Exclude].DisplayText;
                            System.Data.DataRow R = this._Sheet.DT().Rows[Cell.RowIndex];
                            string PK_ExcludeValue = R[PK_ExcludeColumn].ToString();
                            string SQL = "SELECT " + PK_Exclude + " AS VALUE, " + this._Sheet.SelectedColumns()[Cell.ColumnIndex].InternalRelationDisplay + " AS Display " +
                                "FROM [" + this._Sheet.SelectedColumns()[Cell.ColumnIndex].DataTable().Name + "] ";
                            string WhereClause = "";
                            foreach (string PK in this._Sheet.SelectedColumns()[Cell.ColumnIndex].DataTable().PrimaryKeyColumnList)
                            {
                                if (PK == PK_Exclude)
                                {
                                    if (PK_ExcludeValue.Length > 0)
                                    {
                                        if (WhereClause.Length > 0)
                                            WhereClause += " AND ";
                                        WhereClause += PK_Exclude + " <> '" + PK_ExcludeValue + "'";
                                    }
                                }
                                else
                                {
                                    if (WhereClause.Length > 0)
                                        WhereClause += " AND ";
                                    WhereClause += PK + " = '" + R[PK].ToString() + "'";
                                }
                            }
                            SQL += " WHERE " + WhereClause;
                            System.Data.DataTable dt = new System.Data.DataTable();
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                            ad.Fill(dt);
                            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Display", "Value", this._Sheet.SelectedColumns()[Cell.ColumnIndex].DisplayText, "Select a value from the list", "", false);
                            f.ShowDialog();
                            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                            {

                            }
                            f.Dispose();
                        }
                    }
                    // geography columns
                    else if (this._Sheet.SelectedColumns()[Cell.ColumnIndex].Column.DataType == "geography")
                    {
                        switch (SourceGrid)
                        {
                            case Sheet.Grid.Data:
                                if (this._Sheet.DT().Rows[Cell.RowIndex][Cell.ColumnIndex].Equals(System.DBNull.Value))
                                {
                                    DiversityWorkbench.Forms.FormGeography f = new DiversityWorkbench.Forms.FormGeography();
                                    f.ShowDialog();
                                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                                    {
                                        this.dataGridView.Rows[Cell.RowIndex].Cells[Cell.ColumnIndex].Value = f.Geography;
                                        this._Sheet.DT().Rows[Cell.RowIndex][Cell.ColumnIndex] = f.Geography;
                                    }
                                    f.Dispose();
                                }
                                else
                                {
                                    System.Collections.Generic.List<WpfSamplingPlotPage.GeoObject> GL = new List<WpfSamplingPlotPage.GeoObject>();
                                    WpfSamplingPlotPage.GeoObject G = new WpfSamplingPlotPage.GeoObject();
                                    G.GeometryData = this.dataGridView.Rows[Cell.RowIndex].Cells[Cell.ColumnIndex].Value.ToString();
                                    G.DisplayText = this._Sheet.SelectedColumns()[Cell.ColumnIndex].DataTable().DisplayText;
                                    G.Identifier = this._Sheet.SelectedColumns()[Cell.ColumnIndex].DataTable().DisplayText;
                                    System.Windows.Media.Brush B = System.Windows.Media.Brushes.Red;
                                    G.FillBrush = B;
                                    G.FillTransparency = 50;
                                    G.StrokeBrush = B;
                                    G.StrokeTransparency = 255;
                                    G.StrokeThickness = 1;
                                    G.PointType = WpfSamplingPlotPage.PointSymbol.Pin;
                                    G.PointSymbolSize = 1;
                                    GL.Add(G);
                                    DiversityWorkbench.Forms.FormGeography f = new DiversityWorkbench.Forms.FormGeography(GL);
                                    try
                                    {
                                        f.ShowDialog();
                                        if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                                        {
                                            this.dataGridView.Rows[Cell.RowIndex].Cells[Cell.ColumnIndex].Value = f.Geography;
                                            this._Sheet.DT().Rows[Cell.RowIndex][Cell.ColumnIndex] = f.Geography;
                                        }
                                        f.Dispose();
                                    }
                                    catch (System.Exception ex)
                                    {
                                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                    }
                                }
                                break;
                        }
                    }
                    // datetime
                    else if (this._Sheet.SelectedColumns()[Cell.ColumnIndex].Column.DataType == "datetime")
                    {
                        DiversityWorkbench.Forms.FormGetDate f = new Forms.FormGetDate(true);
                        f.ShowDialog();
                        if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            string Test = f.DateTime.ToString("yyyy/MM/dd HH:mm:ss");
                            string SetDate = f.Date.Year.ToString() + "/" + f.Date.Month.ToString() + "/" + f.Date.Day.ToString() + " " + f.DateTime.Hour.ToString() + ":" + f.DateTime.Minute.ToString() + ":" + f.DateTime.Second.ToString();
                            this.dataGridView.Rows[Cell.RowIndex].Cells[Cell.ColumnIndex].Value = SetDate;
                            this._Sheet.DT().Rows[Cell.RowIndex][Cell.ColumnIndex] = f.DateTime;
                        }
                        f.Dispose();
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private System.Data.DataTable LookupSourceForColumn(int ColumnIndex, string Preselection, string SortColumn = "")
        {
            string Alias = this._Sheet.SelectedColumns()[ColumnIndex].DataTable().Alias();
            string Column = this._Sheet.SelectedColumns()[ColumnIndex].Column.Name;
            System.Data.DataTable DtValues = new System.Data.DataTable();
            if (this._Sheet.DataTables()[Alias].DataColumns()[Column].SqlLookupSource.IndexOf("#") > -1)
            {
                string SQL = this._Sheet.DataTables()[Alias].DataColumns()[Column].SqlLookupSource;
                string[] ss = SQL.Split(new char[] { '#' });
                string SqlNew = "";
                for (int i = 0; i < ss.Length; i++)
                {
                    if (this._Sheet.DataTables()[Alias].DataColumns().ContainsKey(ss[i]))
                    {
                        string Restriction = this._Sheet.DataTables()[Alias].DataColumns()[ss[i]].RestrictionValue;
                        SqlNew += this._Sheet.DataTables()[Alias].DataColumns()[ss[i]].RestrictionValue;
                    }
                    else
                        SqlNew += ss[i];
                }
                string Message = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SqlNew, ref DtValues, ref Message);
            }
            else
            {
                DtValues = this._Sheet.SelectedColumns()[ColumnIndex].LookupSource;
            }
            // Markus 20.8.24: Sorting the values
            string DisplayColumnName = SortColumn;
            System.Data.DataTable DtValuesSorted = new System.Data.DataTable();
            foreach(System.Data.DataColumn dc in DtValues.Columns)
            {
                System.Data.DataColumn dataColumn = new System.Data.DataColumn(dc.ColumnName, dc.DataType);
                DtValuesSorted.Columns.Add(dataColumn);
                if (DisplayColumnName.Length == 0)
                    DisplayColumnName = dc.ColumnName;
            }
            if (DisplayColumnName.Length > 0)
            {
                foreach (System.Data.DataRow R in DtValues.Select("", DisplayColumnName))
                {
                    System.Data.DataRow dataRow = DtValuesSorted.NewRow();
                    foreach (System.Data.DataColumn dc in DtValues.Columns)
                    {
                        dataRow[dc.ColumnName] = R[dc.ColumnName];
                    }
                    DtValuesSorted.Rows.Add(dataRow);
                }
                return DtValuesSorted;
            }
            return DtValues;
        }

        private System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<string>> _LookupListCopies;
        public System.Collections.Generic.List<string> LookupListCopy(int ColumnIndex)
        {
            if (this._LookupListCopies == null)
                this._LookupListCopies = new Dictionary<int, List<string>>();
            if (!this._LookupListCopies.ContainsKey(ColumnIndex))
            {
                System.Collections.Generic.List<string> L = new List<string>();
                foreach (string s in this._Sheet.LookupList(ColumnIndex))
                    L.Add(s); 
                this._LookupListCopies.Add(ColumnIndex, L);
            }
            return this._LookupListCopies[ColumnIndex];
        }


        #endregion 
       
        #region Filter

        private bool initFilterTable()
        {
            bool OK = true;
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                if (this._Sheet.getDataStructure(ref dt, Sheet.QueryType.Filter))
                {
                    this._Sheet.DtFilter = dt;
                    System.Data.DataRow Rfilter = this._Sheet.DtFilter.NewRow();
                    this._Sheet.DtFilter.Rows.Add(Rfilter);
                    this.dataGridViewFilter.DataSource = this._Sheet.DtFilter;
                    foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridViewFilter.Columns)
                        C.SortMode = DataGridViewColumnSortMode.NotSortable;
                    this.FormatDataGridViewFilter();
                }
                else 
                    OK = false;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        private void FormatDataGridViewFilter()
        {
            try
            {
                this.dataGridViewFilter.EnableHeadersVisualStyles = false;
                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                {
                    this.dataGridViewFilter.Columns[DC.Key].HeaderText = DC.Value.DisplayText.Replace(" ", "_");
                    this.dataGridViewFilter.Columns[DC.Key].DefaultCellStyle.Font = this._FontBold;
                    if (DC.Value.IsVisible)// .DisplayedColumns().ContainsKey(e.ColumnIndex))
                    {
                        if (DC.Value.Type() == DataColumn.ColumnType.Operation)
                        {
                            this.dataGridViewFilter.Columns[DC.Key].DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                            this.dataGridViewFilter.Columns[DC.Key].DefaultCellStyle.BackColor = System.Drawing.Color.Black;
                            this.dataGridViewFilter.Columns[DC.Key].DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                            this.dataGridViewFilter.Columns[DC.Key].DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Black;
                            this.dataGridViewFilter.Columns[DC.Key].HeaderText = "";
                            this.dataGridViewFilter.Columns[DC.Key].ReadOnly = true;
                        }
                        else if (DC.Value.Type() == DataColumn.ColumnType.Spacer)
                        {
                            this.dataGridViewFilter.Columns[DC.Key].DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlDark;
                            this.dataGridViewFilter.Columns[DC.Key].DefaultCellStyle.BackColor = System.Drawing.SystemColors.ControlDark;
                            this.dataGridViewFilter.Columns[DC.Key].DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.ControlDark;
                            this.dataGridViewFilter.Columns[DC.Key].DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.ControlDark;
                            this.dataGridViewFilter.Columns[DC.Key].HeaderText = "";
                            this.dataGridViewFilter.Columns[DC.Key].ReadOnly = true;
                            this.dataGridViewFilter.Columns[DC.Key].HeaderCell.Style.BackColor = System.Drawing.SystemColors.ControlDark;
                            this.dataGridViewFilter.Columns[DC.Key].HeaderCell.Style.ForeColor = System.Drawing.SystemColors.ControlDark;
                        }
                        else // Data
                        {
                            if (this._Sheet.SelectedColumns()[DC.Key].Column.DataType == "bit")
                                this.dataGridViewFilter.Columns[DC.Key].ReadOnly = true;
                            else
                            {
                                this.dataGridViewFilter.Columns[DC.Key].DefaultCellStyle.ForeColor = System.Drawing.SystemColors.GrayText;
                                if (this._Sheet.SelectedColumns()[DC.Key].DataTable().ColorBack() != System.Drawing.Color.White)
                                {
                                    this.dataGridViewFilter.Columns[DC.Key].DefaultCellStyle.BackColor = this.paleColor(this.GetPaleColor(this._Sheet.SelectedColumns()[DC.Key].DataTable().Name), 0.3f);
                                }
                                if (DC.Value.DataRetrievalType == DataColumn.RetrievalType.ViewOnly || DC.Value.Column.Table == null)
                                    this.dataGridViewFilter.Columns[DC.Key].HeaderCell.Style.ForeColor = ColorViewOnly();// System.Drawing.Color.Blue;
                                else if (!DC.Value.Column.IsNullable && DC.Value.Column.ColumnDefault == null)
                                {
                                    this.dataGridViewFilter.Columns[DC.Key].HeaderCell.Style.ForeColor = System.Drawing.Color.Red;
                                }
                                else if (DC.Value.DataTable().Type() == DataTable.TableType.Lookup)
                                    this.dataGridViewFilter.Columns[DC.Key].HeaderCell.Style.ForeColor = ColorLookup();
                                else if (DC.Value.LinkedModule != RemoteLink.LinkedModule.None)
                                    this.dataGridViewFilter.Columns[DC.Key].HeaderCell.Style.ForeColor = System.Drawing.Color.Blue;
                                else if (DC.Value.IsRequired)
                                    this.dataGridViewFilter.Columns[DC.Key].HeaderCell.Style.ForeColor = System.Drawing.Color.Purple;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void SetFilterSettings()
        {
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<int, DiversityWorkbench.Spreadsheet.DataColumn> DC in this._Sheet.SelectedColumns())
                {
                    if (DC.Value.Type() == DataColumn.ColumnType.Data)
                    {
                        string Filter = "";
                        if (DC.Value.DataTable().DataColumns()[DC.Value.Name].OrderDirection != DataColumn.OrderByDirection.none)
                            Filter = DC.Value.DataTable().DataColumns()[DC.Value.Name].OrderSequence.ToString();
                        switch (DC.Value.DataTable().DataColumns()[DC.Value.Name].OrderDirection)
                        {
                            case DataColumn.OrderByDirection.ascending:
                                Filter += "↑  ";
                                break;
                            case DataColumn.OrderByDirection.descending:
                                Filter += "↓  ";
                                break;
                            default:
                                break;
                        }
                        if (DC.Value.DataTable().DataColumns()[DC.Value.Name].FilterValue != null &&
                            DC.Value.DataTable().DataColumns()[DC.Value.Name].FilterValue.Length > 0 &&
                            this.dataGridViewFilter.Rows[0].Cells[DC.Key].Value.ToString() != DC.Value.DataTable().DataColumns()[DC.Value.Name].FilterValue)
                        {
                            Filter += DC.Value.DataTable().DataColumns()[DC.Value.Name].FilterOperator + " " + DC.Value.DataTable().DataColumns()[DC.Value.Name].FilterValue;
                        }
                        if (Filter.Length > 0)
                        {
                            this.dataGridViewFilter.Rows[0].Cells[DC.Key].Value = Filter;
                            this._Sheet.ResetFilter();
                        }
                    }
                    else if (DC.Value.Type() == DataColumn.ColumnType.Operation)
                    {
                        try
                        {
                            if (this.dataGridViewFilter.Rows[0].Cells[DC.Key].Value.ToString() != DC.Value.DataTable().FilterOperator)
                            {
                                this.dataGridViewFilter.Rows[0].Cells[DC.Key].Value = DC.Value.DataTable().FilterOperator;
                                this._Sheet.ResetFilter();
                            }
                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void dataGridViewFilter_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            this.setColumnWidth(e.Column.Index, this.dataGridViewFilter.Columns[e.Column.Index].Width);
        }

        private void initFilterValues()
        {
            int i = 0;
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                {
                    i = DC.Key;
                    if (this._Sheet.Filter().ContainsKey(DC.Key) && this.dataGridViewFilter.Columns.Count >= DC.Key)
                        this.dataGridViewFilter.Rows[0].Cells[DC.Key].Value = this._Sheet.Filter()[DC.Key];
                    if (DC.Value.Type() == DataColumn.ColumnType.Operation && this.dataGridViewFilter.Columns.Count >= DC.Key)
                    {
                        try
                        {
                            this.dataGridViewFilter.Rows[0].Cells[DC.Key].Value = this._Sheet.DataTables()[DC.Value.DataTable().Alias()].FilterOperator;
                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void GetFilter()
        {
            foreach(System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                if (this._Sheet.Filter().ContainsKey(DC.Key))
                    this.dataGridViewFilter.Rows[0].Cells[DC.Key].Value = this._Sheet.Filter()[DC.Key];
            }
        }

        #region Project alarm

        private void ProjectAlarm_Start()
        {
            this._Sheet.ResetTotalCount();
            this.ProjectAlarmTimer.Start();
        }

        private void ProjectAlarm_Stop()
        {
            this.ProjectAlarmTimer.Stop();
            this.labelProject.BackColor = System.Drawing.SystemColors.Control;
        }

        private System.Timers.Timer _ProjectAlarmTimer;

        private System.Timers.Timer ProjectAlarmTimer
        {
            get
            {
                if (this._ProjectAlarmTimer == null)
                {
                    this._ProjectAlarmTimer = new System.Timers.Timer(200);
                    this._ProjectAlarmTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedProjectEvent);
                    this._ProjectAlarmTimer.Interval = 200;
                    this._ProjectAlarmTimer.Enabled = true;
                }
                return _ProjectAlarmTimer;
            }
        }

        void OnTimedProjectEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                // Change label color
                if (this.labelProject.BackColor == System.Drawing.Color.Red)
                    this.labelProject.BackColor = System.Drawing.SystemColors.Control;
                else
                    this.labelProject.BackColor = System.Drawing.Color.Red;
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Filter alarm

        private void FilterAlarm_Start()
        {
            this._Sheet.ResetTotalCount();
            this.FilterAlarmTimer.Start();
        }

        private void FilterAlarm_Stop()
        {
            this.FilterAlarmTimer.Stop();
            this.buttonFilter.BackColor = System.Drawing.SystemColors.Control;
        }

        private System.Timers.Timer _FilterAlarmTimer;

        private System.Timers.Timer FilterAlarmTimer
        {
            get
            {
                if (this._FilterAlarmTimer == null)
                {
                    this._FilterAlarmTimer = new System.Timers.Timer(200);
                    this._FilterAlarmTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
                    this._FilterAlarmTimer.Interval = 200;
                    this._FilterAlarmTimer.Enabled = true;
                }
                return _FilterAlarmTimer;
            }
        }

        void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                // Change button color
                if (this.buttonFilter.BackColor == System.Drawing.Color.Red)
                    this.buttonFilter.BackColor = System.Drawing.SystemColors.Control;
                else
                    this.buttonFilter.BackColor = System.Drawing.Color.Red;
            }
            catch (System.Exception ex)
            {
            }
        }
        
        #endregion

        #region Changing the filter values

        private void comboBoxProject_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int iProject;
            if (int.TryParse(this.comboBoxProject.SelectedValue.ToString(), out iProject))
            {
                this._Sheet.setProjectID(iProject);
                this._ProjectFound = true;
                this.FilterAlarm_Start();
                this.buttonFilter.Focus();
                this.ProjectAlarm_Stop();
            }
            else
                this._ProjectFound = false;
        }

        private void dataGridViewFilter_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewFilter.SelectedCells != null &&
                this.dataGridViewFilter.SelectedCells.Count > 0 &&
                this.dataGridViewFilter.SelectedCells[0].Value != null)
            {
                try
                {
                    string Filter = this.dataGridViewFilter.Rows[0].Cells[e.ColumnIndex].Value.ToString();
                    this._Sheet.SetFilterValue(e.ColumnIndex, this.dataGridViewFilter.Rows[0].Cells[e.ColumnIndex].Value.ToString());
                    if (Filter.Length == 0)
                        this._Sheet.DtFilter.Rows[0][e.ColumnIndex] = System.DBNull.Value;
                    else
                        this._Sheet.DtFilter.Rows[0][e.ColumnIndex] = this.dataGridViewFilter.Rows[0].Cells[e.ColumnIndex].Value.ToString();
                    this.FilterAlarm_Start();
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void numericUpDownMaxResults_ValueChanged(object sender, EventArgs e)
        {
            if (this._Sheet.MaxResult() != (int)this.numericUpDownMaxResults.Value)
            {
                this._Sheet.setMaxResult((int)this.numericUpDownMaxResults.Value);
                this.toolTip.SetToolTip(this.buttonNext, "Get next " + this.numericUpDownMaxResults.Value.ToString() + " datasets");
                this.toolTip.SetToolTip(this.buttonPrevious, "Get previous " + this.numericUpDownMaxResults.Value.ToString() + " datasets");
                this.FilterAlarm_Start();
            }
        }

        private void dataGridViewFilter_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this._Sheet.SelectedColumns()[e.ColumnIndex].Column.DataType == "bit")
            {
                return;
            }
            if (this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Type() == DataTable.TableType.InsertOnly)
            {
                return;
            }

            if ((this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().FilterOperator == "Ø"
                || this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().FilterOperator == "◊")
                && (this._Sheet.SelectedColumns()[e.ColumnIndex].Type() == DataColumn.ColumnType.Data
                || this._Sheet.SelectedColumns()[e.ColumnIndex].Type() == DataColumn.ColumnType.Link))
            {
                System.Windows.Forms.MessageBox.Show("Not possible with NULL operations (Ø or ◊) on table");
                return;
            }

            string Filter = this.dataGridViewFilter.Rows[0].Cells[e.ColumnIndex].Value.ToString();
            if (this._Sheet.SelectedColumns()[e.ColumnIndex].Type() == DataColumn.ColumnType.Operation)
            {
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(this._Sheet.FilterOperatorTableDictionary(), "Operator", "Please select the operator", true);
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    this.dataGridViewFilter.Rows[0].Cells[e.ColumnIndex].Value = f.SelectedValue;
                    this.dataGridViewFilter.Rows[0].Cells[e.ColumnIndex].ToolTipText = f.SelectedString;
                    this._Sheet.DtFilter.Rows[0][e.ColumnIndex] = f.SelectedValue;
                    this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().FilterOperator = f.SelectedValue;
                    if (f.SelectedValue == "◊" || f.SelectedValue == "Ø")
                    {
                        bool ColumnFilterHasBeenReset = false;
                        foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                        {
                            if (DC.Value.DataTable().Alias() == this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Alias() 
                                && DC.Value.Type() != DataColumn.ColumnType.Operation
                                && DC.Value.Type() != DataColumn.ColumnType.Spacer)
                            {
                                //DC.Value.FilterOperator = "";
                                if ((DC.Value.FilterValue != null && DC.Value.FilterValue.Length > 0) || DC.Value.OrderDirection != DataColumn.OrderByDirection.none || DC.Value.OrderSequence != null)
                                    ColumnFilterHasBeenReset = true;
                                DC.Value.FilterValue = "";
                                DC.Value.OrderDirection = DataColumn.OrderByDirection.none;
                                DC.Value.OrderSequence = null;
                                this.dataGridViewFilter.Rows[0].Cells[DC.Key].Value = "";
                            }
                        }
                        if (ColumnFilterHasBeenReset)
                            System.Windows.Forms.MessageBox.Show("Column filters and sorting for table " + this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Name + " have been reset");
                    }
                }
                f.Dispose();
            }
            else if (e.RowIndex == 0 &&
                this._Sheet.SelectedColumns()[e.ColumnIndex].Type() == DataColumn.ColumnType.Data)
            {
                this.dataGridViewDataCell_Click(this.dataGridViewFilter.Rows[e.RowIndex].Cells[e.ColumnIndex], Sheet.Grid.Filter);
                this._Sheet.SetFilterValue(e.ColumnIndex, this._Sheet.SelectedColumns()[e.ColumnIndex].FilterValue); // this.dataGridViewFilter.Rows[0].Cells[e.ColumnIndex].Value.ToString());
            }
            if (e.RowIndex == 0
                && this.dataGridViewFilter.Rows[0].Cells[e.ColumnIndex].Value.ToString() != Filter)
            {
                this.FilterAlarm_Start();
            }
            // Reset fixing and editing
            this.FixingAndEditingReset();
        }

        private void dataGridViewFilter_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().FilterOperator == "|" &&
                this._Sheet.SelectedColumns()[e.ColumnIndex].Type() == DataColumn.ColumnType.Data)
            {
                DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Restriction: Use one line for every value", this.dataGridViewFilter.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    this.dataGridViewFilter.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = f.EditedText;
                    this.FilterAlarm_Start();
                }
                f.Dispose();
            }
        }
        
        #endregion

        private void dataGridViewFilter_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this._Sheet.SelectedColumns()[e.ColumnIndex].Type() == DataColumn.ColumnType.Operation)
                {
                    string Operator = this.dataGridViewFilter.Rows[0].Cells[e.ColumnIndex].Value.ToString();
                    this.dataGridViewFilter.Rows[0].Cells[e.ColumnIndex].ToolTipText = this._Sheet.FilterOperatorToolTip(Operator);
                }
                else if (this._Sheet.SelectedColumns()[e.ColumnIndex].Type() == DataColumn.ColumnType.Data
                    || this._Sheet.SelectedColumns()[e.ColumnIndex].Type() == DataColumn.ColumnType.Link)
                {
                    foreach (System.Windows.Forms.DataGridViewCell D in this.dataGridViewFilter.Rows[e.RowIndex].Cells)
                        D.Selected = false;
                    System.Windows.Forms.DataGridViewCell C = this.dataGridViewFilter.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    C.Selected = true;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void dataGridViewFilter_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        #endregion

        #region Adding

        private bool _AddingPossible = true;

        public void AddingPossible(bool IsPossible)
        {
            this._AddingPossible = IsPossible;
            this.tableLayoutPanelAdding.Visible = IsPossible;
        }

        public void EnableAdding(bool Enabled)
        {
            this.dataGridViewAdding.Enabled = Enabled;
        }

        private void initAddingTable()
        {
            if (!this._AddingPossible)
                return;
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                this._Sheet.getDataStructure(ref dt, Sheet.QueryType.Adding);
                this._Sheet.DtAdding = dt;
                System.Data.DataRow Radd = this._Sheet.DtAdding.NewRow();
                this._Sheet.DtAdding.Rows.Add(Radd);
                this.dataGridViewAdding.DataSource = this._Sheet.DtAdding;
                this.FormatDataGridViewAdding();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void initAddingDefaults()
        {
            if (!this._AddingPossible)
                return;

            this.SetDefaultsForAdding();
        }

        private void SetDefaultsForAdding()
        {
            if (!this._AddingPossible)
                return;
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<int, DiversityWorkbench.Spreadsheet.DataColumn> DC in this._Sheet.SelectedColumns())
                {
                    if (DC.Value.Type() == DataColumn.ColumnType.Data)
                    {
                        if (DC.Value.DefaultForAdding != null && DC.Value.DefaultForAdding.Length > 0)
                        {
                            this.dataGridViewAdding.Rows[0].Cells[DC.Key].Value = DC.Value.DefaultForAdding;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void FormatDataGridViewAdding()
        {
            if (!this._AddingPossible)
                return;

            try
            {
                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                {
                    this.dataGridViewAdding.Columns[DC.Key].DefaultCellStyle.Font = this._FontBold;
                    if (DC.Value.Type() == DataColumn.ColumnType.Operation)
                    {
                        if (this._Sheet.SelectedColumns()[DC.Key].DataTable().Type() == DataTable.TableType.Root ||
                            this._Sheet.SelectedColumns()[DC.Key].DataTable().Type() == DataTable.TableType.Target)
                        {
                            this.dataGridViewAdding.Columns[DC.Key].DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                            this.dataGridViewAdding.Columns[DC.Key].DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                            this.dataGridViewAdding.Columns[DC.Key].DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                            this.dataGridViewAdding.Columns[DC.Key].DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Green;
                            this.dataGridViewAdding.Rows[0].Cells[DC.Key].Value = "+";
                            this._Sheet.DtAdding.Rows[0][DC.Key] = "+";
                        }
                        else
                        {
                            this.dataGridViewAdding.Columns[DC.Key].DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlDark;
                            this.dataGridViewAdding.Columns[DC.Key].DefaultCellStyle.BackColor = System.Drawing.SystemColors.ControlDark;
                            this.dataGridViewAdding.Columns[DC.Key].DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.ControlDark;
                            this.dataGridViewAdding.Columns[DC.Key].DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.ControlDark;
                        }
                    }
                    else if (DC.Value.Type() == DataColumn.ColumnType.Spacer)
                    {
                        this.dataGridViewAdding.Columns[DC.Key].DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlDark;
                        this.dataGridViewAdding.Columns[DC.Key].DefaultCellStyle.BackColor = System.Drawing.SystemColors.ControlDark;
                        this.dataGridViewAdding.Columns[DC.Key].DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.ControlDark;
                        this.dataGridViewAdding.Columns[DC.Key].DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.ControlDark;
                    }
                    else
                    {
                        this.dataGridViewAdding.Columns[DC.Key].DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                        if (this._Sheet.SelectedColumns()[DC.Key].DataTable().ColorFont() != System.Drawing.Color.Black)
                            this.dataGridViewAdding.Columns[DC.Key].DefaultCellStyle.ForeColor = this._Sheet.SelectedColumns()[DC.Key].DataTable().ColorFont();
                        if (this._Sheet.SelectedColumns()[DC.Key].DataTable().ColorBack() != System.Drawing.Color.White)
                        {
                            this.dataGridViewAdding.Columns[DC.Key].DefaultCellStyle.BackColor = this.paleColor(this.GetPaleColor(this._Sheet.SelectedColumns()[DC.Key].DataTable().Name), 0.2f);//.ColorBack();
                            if (this._Sheet.SelectedColumns()[DC.Key].DataTable().Type() == DataTable.TableType.Lookup)
                                this.dataGridViewAdding.Columns[DC.Key].ReadOnly = true;
                            if (this._Sheet.SelectedColumns()[DC.Key].IsIdentity)
                                this.dataGridViewAdding.Columns[DC.Key].ReadOnly = true;
                            if (this._Sheet.SelectedColumns()[DC.Key].IsReadOnly())
                                this.dataGridViewAdding.Columns[DC.Key].ReadOnly = true;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void dataGridViewAdding_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (!this._AddingPossible)
                    return;

                if (this._Sheet.ReadOnly)
                    return;

                if (this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Type() == DataTable.TableType.Lookup)
                    return;

                if (this._Sheet.SelectedColumns()[e.ColumnIndex].Type() == DataColumn.ColumnType.Data
                    && this._Sheet.SelectedColumns()[e.ColumnIndex].IsIdentity)
                    return;

                if (this._Sheet.SelectedColumns()[e.ColumnIndex].Type() == DataColumn.ColumnType.Data
                    && this._Sheet.SelectedColumns()[e.ColumnIndex].IsReadOnly())
                    return;

                if (this._Sheet.SelectedColumns()[e.ColumnIndex].Type() == DataColumn.ColumnType.Operation &&
                    (this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Type() == DataTable.TableType.Root ||
                     this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Type() == DataTable.TableType.Target))
                {
                    if (!this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().AllowAdding())
                    {
                        System.Windows.Forms.MessageBox.Show("You have no permisssion to add data to table " + this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Name);
                        return;
                    }
                    if (this.dataGridView.SelectedRows != null)
                    {
                        int? selectedRowIndex = null;
                        if (this.dataGridView.SelectedRows.Count == 1)
                            selectedRowIndex = this.dataGridView.SelectedRows[0].Index;
                        else if (this.dataGridView.Rows.Count > 0 && this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Type() != DataTable.TableType.Root)
                        {
                            System.Windows.Forms.MessageBox.Show("Please mark the row the new data should be added to");
                            return;
                        }
                        string Message = "";
                        System.Data.DataRow AddedRow = this._Sheet.DT().NewRow();
                        // insert key values of parent tables
                        if (this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().ParentTable() != null &&
                            selectedRowIndex != null)
                        {
                            // getting the table alias of the inserted table
                            string TabInsAlias = this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Alias();
                            string TabParAlias = this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().ParentTable().Alias();
                            foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this._Sheet.DataTables()[TabInsAlias].DataColumns())
                            {
                                if (DC.Value.Column.ForeignRelations.ContainsKey(this._Sheet.DataTables()[TabParAlias].Name))
                                {
                                    string ConParAlias = this._Sheet.DataTables()[TabParAlias].DataColumns()[DC.Value.Name].DisplayText;
                                    if (this._Sheet.DT().Columns.Contains(ConParAlias))
                                    {
                                        AddedRow[ConParAlias] = this._Sheet.DT().Rows[(int)selectedRowIndex][ConParAlias];
                                        AddedRow[DC.Value.DisplayText] = this._Sheet.DT().Rows[(int)selectedRowIndex][ConParAlias];
                                    }
                                    else
                                    {
                                        bool ParentTableIsMissing = true;
                                        // The parent table may not be included in the list, e.g. table CollectionSpecimen is hidden
                                        // Check if parent table is included
                                        foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> dc in this._Sheet.SelectedColumns())
                                        {
                                            if (dc.Value.DataTable().Alias() == TabParAlias)
                                            {
                                                ParentTableIsMissing = false;
                                            }
                                        }
                                        if (ParentTableIsMissing)
                                        {
                                            Message = "Parent table " + this._Sheet.DataTables()[TabParAlias].Name + " (= " + this._Sheet.DataTables()[TabParAlias].DisplayText + ") is not included." +
                                                "\r\nPlease include this table to enable the adding of data.";
                                            System.Windows.Forms.MessageBox.Show(Message, "Missing table", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                        System.Collections.Generic.Dictionary<string, string> PKvalues = new Dictionary<string, string>();
                        if (this._Sheet.AddingData(this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().Alias(), AddedRow, ref Message, ref PKvalues))
                        {
                            this.getData();
                            if (this._Sheet.DT().Rows.Count > 0 && PKvalues.Count > 0)
                            {
                                bool InsertedRowFound = false;
                                for (int i = 0; i < this._Sheet.DT().Rows.Count; i++)
                                {
                                    InsertedRowFound = true;
                                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in PKvalues)
                                    {
                                        if (this._Sheet.DT().Rows[i][KV.Key].ToString().Length > 0
                                            && this._Sheet.DT().Rows[i][KV.Key].ToString() != KV.Value)
                                        {
                                            InsertedRowFound = false;
                                            break;
                                        }
                                    }
                                    if (InsertedRowFound)
                                    {
                                        this.dataGridView.Rows[i].Selected = true;
                                        break;
                                    }
                                }
                                if (!InsertedRowFound)
                                {
                                    Message = "Failed to mark added row.";
                                    if (this.dataGridView.Rows.Count >= (int)this.numericUpDownMaxResults.Value)
                                    {
                                        Message += "\r\nMay be not within selected range";
                                    }
                                    System.Windows.Forms.MessageBox.Show(Message);
                                }
                            }
                            else
                                System.Windows.Forms.MessageBox.Show("Failed to mark added row");
                        }
                        else
                            System.Windows.Forms.MessageBox.Show("Adding failed:\r\n" + Message);
                    }
                }
                else if (this._Sheet.SelectedColumns()[e.ColumnIndex].Type() == DataColumn.ColumnType.Data)
                {
                    this.dataGridViewDataCell_Click(this.dataGridViewAdding.Rows[e.RowIndex].Cells[e.ColumnIndex], Sheet.Grid.Adding);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.FixingAndEditingReset();
        }

        private void dataGridViewAdding_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!this._AddingPossible)
                return;

            this._Sheet.SelectedColumns()[e.ColumnIndex].DefaultForAdding = this._Sheet.DtAdding.Rows[0][e.ColumnIndex].ToString();
            foreach (string LC in this._Sheet.SelectedColumns()[e.ColumnIndex].LinkedColumns())
            {
                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                {
                    if (DC.Value.DisplayText == LC)
                    {
                        this._Sheet.SelectedColumns()[DC.Key].DefaultForAdding = this._Sheet.DtAdding.Rows[0][DC.Key].ToString();
                    }
                }
            }
        }

        private void buttonAddingClear_Click(object sender, EventArgs e)
        {
            if (!this._AddingPossible)
                return;

            try
            {
                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                {
                    if (DC.Value.Type() == DataColumn.ColumnType.Data)
                        this.dataGridViewAdding.Rows[0].Cells[DC.Key].Value = System.DBNull.Value;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void dataGridViewAdding_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!this._AddingPossible)
                return;

            try
            {
                if (this._Sheet.SelectedColumns()[e.ColumnIndex].Type() == DataColumn.ColumnType.Operation)
                {
                    if (this.dataGridViewAdding.Rows[0].Cells[e.ColumnIndex].Value.ToString() == "+")
                        this.dataGridViewAdding.Rows[0].Cells[e.ColumnIndex].ToolTipText = "Add data to table " + this._Sheet.SelectedColumns()[e.ColumnIndex].DataTable().DisplayText;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void dataGridViewAdding_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        #endregion

        #region Image

        private void setImage(string URI)
        {
            try
            {
                if (URI.Length == 0)
                {
                    this.pictureBoxImage.Image = null;
                    this.pictureBoxImage.Tag = null;
                }
                else
                {
                    DiversityWorkbench.Forms.FormFunctions.Medium _MediumType = DiversityWorkbench.Forms.FormFunctions.MediaType(URI);
                    if (_MediumType == DiversityWorkbench.Forms.FormFunctions.Medium.Image)
                    {
                        string Message = "";
                        this.pictureBoxImage.Image = DiversityWorkbench.Forms.FormFunctions.BitmapFromWeb(URI, ref Message);
                        if (Message.Length > 0)
                        {
                            System.Windows.Forms.MessageBox.Show(Message);
                        }
                        this.pictureBoxImage.Tag = URI;
                    }
                    else
                    {
                        this.pictureBoxImage.Image = null;
                        this.pictureBoxImage.Tag = null;
                    }
                }
            }
            catch (System.Exception ex)
            {
                this.pictureBoxImage.Image = null;
                this.pictureBoxImage.Tag = null;
            }
        }

        private void pictureBoxImage_Click(object sender, EventArgs e)
        {
            if (this.pictureBoxImage.Tag != null)
            {
                DiversityWorkbench.Forms.FormImage fI = new DiversityWorkbench.Forms.FormImage(pictureBoxImage.Tag.ToString());
                fI.ShowDialog();
                fI.Dispose();
            }
        }
        
        #endregion

        #region Map

        private enum MapType { WGS84, TK25, Map, Geometry, Undefined }
        private MapType _MapType = MapType.TK25;
        
        public void SetMapColumns(
            string GeographyColumn, string GeographyTableAlias,
            string GeographyIdentifierColum, string GeographyIdentifierTableAlias)
        {
            this._Sheet.GeographyColumn = GeographyColumn;
            this._Sheet.GeographyTableAlias = GeographyTableAlias;
            if (this._Sheet.GeographyKeyTableAlias == null || this._Sheet.GeographyKeyTableAlias.Length == 0)
            {
                this._Sheet.GeographyKeyTableAlias = GeographyIdentifierTableAlias;
                this._Sheet.GeographyKeyColumn = GeographyIdentifierColum;
            }
            if (this._Sheet.GeographyKeyColumn == null || this._Sheet.GeographyKeyColumn.Length == 0)
            {
                this._Sheet.GeographyKeyColumn = GeographyIdentifierColum;
                //this._Sheet.GeographyKeyTableAlias = GeographyIdentifierTableAlias;
            }
            this.toolStripMap.Visible = true;
            //this.buttonShowMap.Visible = true;
            //this.buttonSetMapIcons.Visible = true;
        }

        public void SetMapColumns(
            string GeographyColumn, string GeographyTableAlias,
            string GeographyIdentifierColum, string GeographyIdentifierTableAlias,
            string GeographyIconColumn, string GeographyIconTableAlias,
            string GeographyColorColumn, string GeographyColorTableAlias)
        {
            this._Sheet.GeographyColumn = GeographyColumn;
            this._Sheet.GeographyTableAlias = GeographyTableAlias;
            this._Sheet.GeographyKeyColumn = GeographyIdentifierColum;
            this._Sheet.GeographyKeyTableAlias = GeographyIdentifierTableAlias;
            this._Sheet.GeographySymbolColumn = GeographyIconColumn;
            this._Sheet.GeographySymbolTableAlias = GeographyIconTableAlias;
            this.toolStripMap.Visible = true;
            //this.buttonShowMap.Visible = true;
            //this.buttonSetMapIcons.Visible = true;
        }

        private System.Collections.Generic.List<WpfSamplingPlotPage.GeoObject> GeoObjects()
        {
            System.Collections.Generic.List<WpfSamplingPlotPage.GeoObject> Objects = new List<WpfSamplingPlotPage.GeoObject>();
            int iGeographyPosition = this.GeographyObjectPosition(this._Sheet.GeographyColumn, this._Sheet.GeographyTableAlias);
            int iGeographyObjectPosition = this.GeographyObjectPosition(this._Sheet.GeographyKeyColumn, this._Sheet.GeographyKeyTableAlias);
            int iGeographySymbolPosition = this.GeographyObjectPosition(this._Sheet.GeographySymbolColumn, this._Sheet.GeographySymbolTableAlias);
            int iGeographyColorPosition = this.GeographyObjectPosition(this._Sheet.GeographyColorColumn, this._Sheet.GeographyColorTableAlias);
            if (iGeographyObjectPosition > -1
                && iGeographyPosition > -1
                && iGeographySymbolPosition > -1)
            {
                foreach (System.Data.DataRow R in this._Sheet.DT().Rows)
                {
                    try
                    {
                        string Value = R[iGeographySymbolPosition].ToString();
                        if (!this._Sheet.MapSymbols().ContainsKey(Value))
                            continue;
                        if (R[iGeographyPosition].Equals(System.DBNull.Value) || R[iGeographyPosition].ToString().Length == 0)
                            continue;
                        WpfSamplingPlotPage.GeoObject O = new WpfSamplingPlotPage.GeoObject();
                        O.GeometryData = R[iGeographyPosition].ToString();
                        O.Identifier = R[iGeographyObjectPosition].ToString();
                        O.DisplayText = R[iGeographyObjectPosition].ToString();
                        string MapColorValue = "";
                        if(iGeographyColorPosition > -1)
                            MapColorValue = R[iGeographyColorPosition].ToString();
                        MapColor MC = this._Sheet.getMapColor(MapColorValue);
                        O.FillBrush = MC.Brush;
                        O.StrokeBrush = MC.Brush;
                        O.StrokeTransparency = this._Sheet.GeographyTransparency;
                        MapSymbol MS = this._Sheet.MapSymbols()[Value];
                        O.PointType = MS.Symbol;
                        if (MS.SymbolFilled)
                        {
                            O.FillTransparency = this._Sheet.GeographyTransparency;
                            O.StrokeThickness = 0;
                        }
                        else
                        {
                            O.FillTransparency = 0;
                            O.StrokeThickness = MS.SymbolSize;
                        }
                        //O.StrokeThickness = this._Sheet.GeographyStrokeThickness;
                        if (this._Sheet.GeographySymbolSizeCanBeLinkedToColumnValue())
                        {
                            int iGeographySymbolSizePosition = this.GeographyObjectPosition(this._Sheet.GeographySymbolSizeColumn, this._Sheet.GeographySymbolSizeTableAlias);
                            if (iGeographySymbolSizePosition > -1)
                            {
                                double SymbolSize;
                                if (double.TryParse(R[iGeographySymbolSizePosition].ToString(), out SymbolSize) && SymbolSize > 0)
                                    O.PointSymbolSize = SymbolSize * this._Sheet.GeographySymbolSize * DiversityWorkbench.Spreadsheet.MapSymbols.SymbolScalingFactors()[O.PointType];
                                else
                                    O.PointSymbolSize = this._Sheet.GeographySymbolSize;
                            }
                            else if (this._Sheet.GeographySymbolSize > 0)
                            {
                                O.PointSymbolSize = this._Sheet.GeographySymbolSize;
                            }
                            else
                                O.PointSymbolSize = 1;
                        }
                        else
                        {
                            O.PointSymbolSize = MS.SymbolSize;
                        }
                        Objects.Add(O);
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
            return Objects;
        }

        private System.Collections.Generic.List<WpfSamplingPlotPage.GeoObject> GeoObjectsWGS84()
        {
            System.Collections.Generic.List<WpfSamplingPlotPage.GeoObject> Objects = new List<WpfSamplingPlotPage.GeoObject>();
            //int iGeographyPosition = this.GeographyObjectPosition(this._Sheet.GeographyColumn, this._Sheet.GeographyTableAlias);
            int iGeographyCoordinatesPosition = this.GeographyObjectPosition(this._Sheet.GeographyWGS84Column, this._Sheet.GeographyWGS84TableAlias);
            int iGeographyObjectPosition = -1;// this.GeographyObjectPosition(this._Sheet.GeographyKeyColumn, this._Sheet.GeographyKeyTableAlias);
            string TargetTable = "";
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Spreadsheet.DataTable> KV in this._Sheet.DataTables())
            {
                if (KV.Value.Type() == DataTable.TableType.Target)
                {
                    iGeographyObjectPosition = this.GeographyObjectPosition(KV.Value.IdentityColumn, KV.Key);
                    TargetTable = KV.Value.DisplayText;
                    break;
                }
            }
            int iGeographySymbolPosition = this.GeographyObjectPosition(this._Sheet.GeographySymbolColumn, this._Sheet.GeographySymbolTableAlias);
            int iGeographyColorPosition = this.GeographyObjectPosition(this._Sheet.GeographyColorColumn, this._Sheet.GeographyColorTableAlias);
            if (iGeographyObjectPosition > -1
                && iGeographyCoordinatesPosition > -1
                && iGeographySymbolPosition > -1)
            {
                foreach (System.Data.DataRow R in this._Sheet.DT().Rows)
                {
                    try
                    {
                        string Value = R[iGeographySymbolPosition].ToString();
                        if (R[iGeographyCoordinatesPosition].Equals(System.DBNull.Value) || R[iGeographyCoordinatesPosition].ToString().Length == 0)
                            continue;
                        WpfSamplingPlotPage.GeoObject O = new WpfSamplingPlotPage.GeoObject();
                        O.GeometryData = R[iGeographyCoordinatesPosition].ToString();
                        O.Identifier = R[iGeographyObjectPosition].ToString();
                        string IDInfos = "";
                        if (TargetTable.Length > 0)
                            IDInfos = "ID " + TargetTable + ": " + R[iGeographyObjectPosition].ToString();
                        if (this._Sheet.ShowDetailsInMap)
                            O.DisplayText = this.GeoObjectInfos(R, IDInfos, 255);
                        else
                            O.DisplayText = R[iGeographyObjectPosition].ToString();
                        string MapColorValue = "";
                        if (iGeographyColorPosition > -1)
                            MapColorValue = R[iGeographyColorPosition].ToString();
                        MapColor MC = this._Sheet.getMapColor(MapColorValue);
                        O.FillBrush = MC.Brush;
                        O.StrokeBrush = MC.Brush;
                        if (!this._Sheet.MapSymbols().ContainsKey(Value) && this._Sheet.MapSymbols().ContainsKey(Value.ToUpper()))
                            Value = Value.ToUpper();
                        if (!this._Sheet.MapSymbols().ContainsKey(Value) && this._Sheet.MapSymbols().ContainsKey(Value.ToLower()))
                            Value = Value.ToLower();
                        if (!this._Sheet.MapSymbols().ContainsKey(Value))
                            continue;
                        MapSymbol MS = this._Sheet.MapSymbols()[Value];
                        if (MS.Symbol == WpfSamplingPlotPage.PointSymbol.None) // will not be shown in map
                        {
                            continue;
                        }
                        O.PointType = MS.Symbol;
                        if (MS.SymbolFilled)
                        {
                            O.FillTransparency = this._Sheet.GeographyTransparency;
                            O.StrokeTransparency = 0;
                            O.StrokeThickness = 0;
                        }
                        else
                        {
                            O.FillTransparency = 0;
                            O.StrokeTransparency = this._Sheet.GeographyTransparency;
                            O.StrokeThickness = MS.SymbolSize;
                        }
                        //O.StrokeThickness = this._Sheet.GeographyStrokeThickness;
                        if (this._Sheet.GeographySymbolSizeCanBeLinkedToColumnValue())
                        {
                            int iGeographySymbolSizePosition = this.GeographyObjectPosition(this._Sheet.GeographySymbolSizeColumn, this._Sheet.GeographySymbolSizeTableAlias);
                            if (iGeographySymbolSizePosition > -1)
                            {
                                double SymbolSize;
                                if (R[iGeographySymbolSizePosition].Equals(System.DBNull.Value) || R[iGeographySymbolSizePosition].ToString().Length == 0)
                                    O.PointSymbolSize = this._Sheet.GeographySymbolSize / 2;
                                else if (double.TryParse(R[iGeographySymbolSizePosition].ToString(), out SymbolSize) && SymbolSize > 0)
                                    O.PointSymbolSize = SymbolSize * this._Sheet.GeographySymbolSize * DiversityWorkbench.Spreadsheet.MapSymbols.SymbolScalingFactors()[O.PointType];
                                else
                                    O.PointSymbolSize = this._Sheet.GeographySymbolSize;
                            }
                            else if (this._Sheet.GeographySymbolSize > 0)
                            {
                                O.PointSymbolSize = this._Sheet.GeographySymbolSize;
                            }
                            else
                                O.PointSymbolSize = 1;
                        }
                        else
                        {
                            O.PointSymbolSize = MS.SymbolSize;
                        }
                        Objects.Add(O);
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
            return Objects;
        }

        private System.Collections.Generic.List<WpfSamplingPlotPage.GeoObject> GeoObjectsMap()
        {
            System.Collections.Generic.List<WpfSamplingPlotPage.GeoObject> Objects = new List<WpfSamplingPlotPage.GeoObject>();
            int iGeographyCoordinatesPosition = this.GeographyObjectPosition(this._Sheet.GeographyWGS84Column, this._Sheet.GeographyUnitGeoTableAlias);
            int iGeographyObjectPosition = -1;// this.GeographyObjectPosition(this._Sheet.GeographyKeyColumn, this._Sheet.GeographyKeyTableAlias);
            string TargetTable = "";
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Spreadsheet.DataTable> KV in this._Sheet.DataTables())
            {
                if (KV.Value.Type() == DataTable.TableType.Target)
                {
                    iGeographyObjectPosition = this.GeographyObjectPosition(KV.Value.IdentityColumn, KV.Key);
                    TargetTable = KV.Value.DisplayText;
                    break;
                }
            }
            int iGeographySymbolPosition = this.GeographyObjectPosition(this._Sheet.GeographySymbolColumn, this._Sheet.GeographySymbolTableAlias);
            int iGeographyColorPosition = this.GeographyObjectPosition(this._Sheet.GeographyColorColumn, this._Sheet.GeographyColorTableAlias);
            if (iGeographyObjectPosition > -1
                && iGeographyCoordinatesPosition > -1
                && iGeographySymbolPosition > -1)
            {
                foreach (System.Data.DataRow R in this._Sheet.DT().Rows)
                {
                    try
                    {
                        string Value = R[iGeographySymbolPosition].ToString();
                        if (R[iGeographyCoordinatesPosition].Equals(System.DBNull.Value) || R[iGeographyCoordinatesPosition].ToString().Length == 0)
                            continue;
                        WpfSamplingPlotPage.GeoObject O = new WpfSamplingPlotPage.GeoObject();
                        O.GeometryData = R[iGeographyCoordinatesPosition].ToString();
                        O.Identifier = R[iGeographyObjectPosition].ToString();
                        string IDInfos = "";
                        if (TargetTable.Length > 0)
                            IDInfos = "ID " + TargetTable + ": " + R[iGeographyObjectPosition].ToString();
                        if (this._Sheet.ShowDetailsInMap)
                            O.DisplayText = this.GeoObjectInfos(R, IDInfos, 255);
                        else
                            O.DisplayText = R[iGeographyObjectPosition].ToString();
                        System.Windows.Media.Brush Brush = System.Windows.Media.Brushes.Red;
                        string MapColorValue = "";
                        if (iGeographyColorPosition > -1)
                        {
                            MapColorValue = R[iGeographyColorPosition].ToString();
                            if (MapColorValue.Length > 0)
                            {
                                MapColor MC = this._Sheet.getMapColor(MapColorValue);
                                Brush = MC.Brush;
                            }
                        }
                        O.FillBrush = Brush;
                        O.StrokeBrush = Brush;
                        O.StrokeTransparency = this._Sheet.GeographyTransparency;
                        O.PointType = WpfSamplingPlotPage.PointSymbol.Pin;
                        O.FillTransparency = 0;
                        O.StrokeThickness = this._Sheet.GeographyStrokeThickness;
                        Objects.Add(O);
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
            return Objects;
        }

        private System.Collections.Generic.List<WpfSamplingPlotPage.GeoObject> GeoObjectsTK25()
        {
            return this.GeoObjectsFiltered(MapType.TK25);
        }

        private string GeoObjectInfos(System.Data.DataRow R, string CurrentInfo, int ColumnLength)
        {
            string Infos = "";
            if (CurrentInfo.Length > 0)
                Infos = CurrentInfo;
            if (this._Sheet.ShowDetailsInMap)
            {
                foreach (System.Collections.Generic.KeyValuePair<int, DiversityWorkbench.Spreadsheet.DataColumn> DC in this._Sheet.SelectedColumns())
                {
                    if (DC.Value.Type() == DataColumn.ColumnType.Data 
                        && DC.Value.IsVisible 
                        && !DC.Value.IsHidden
                        && !R[DC.Key].Equals(System.DBNull.Value)
                        && R[DC.Key].ToString().Length > 0)
                    {
                        if (Infos.Length > 0)
                            Infos += "\r\n";
                        string Info = "";
                        try
                        {
                            Info = R[DC.Key].ToString(); //
                            if (Info.Length > 0)
                                Info = Info.Substring(0, ColumnLength);
                            if (Info.Length > 100)
                                Info = Info.Substring(0, 100) + " ...";
                        }
                        catch (System.Exception ex)
                        {
                        }
                        Infos += DC.Value.DisplayText + ": " + Info;
                    }
                }
            }
            return Infos;
        }

        /// <summary>
        /// Getting a list of the geographial objects according to MapKeyObjects
        /// </summary>
        /// <returns></returns>
        private System.Collections.Generic.List<WpfSamplingPlotPage.GeoObject> GeoObjectsFiltered(MapType MapType = MapType.Undefined)
        {

            System.Collections.Generic.Dictionary<string, MapKeyObject> MapKeyObjects = new Dictionary<string, MapKeyObject>();

            int iGeographyPosition = this.GeographyObjectPosition(this._Sheet.GeographyColumn, this._Sheet.GeographyTableAlias);
            int iGeographyKeyPosition = this.GeographyObjectPosition(this._Sheet.GeographyKeyColumn, this._Sheet.GeographyKeyTableAlias);
            int iGeographySymbolPosition = -1;
            if (this._Sheet.GeographySymbolColumn.Length > 0 && this._Sheet.GeographySymbolTableAlias.Length > 0)
                iGeographySymbolPosition = this.GeographyObjectPosition(this._Sheet.GeographySymbolColumn, this._Sheet.GeographySymbolTableAlias);
            int iGeographyColorPosition = -1;
            if (this._Sheet.GeographyColorColumn != null 
                && this._Sheet.GeographyColorColumn.Length > 0 
                && this._Sheet.GeographyColorTableAlias != null 
                && this._Sheet.GeographyColorTableAlias.Length > 0)
                iGeographyColorPosition = this.GeographyObjectPosition(this._Sheet.GeographyColorColumn, this._Sheet.GeographyColorTableAlias);

            Map M = new Map(this._Sheet);
            return M.GeoObjectsFiltered(iGeographyPosition, iGeographyKeyPosition, iGeographySymbolPosition, iGeographyColorPosition, 
                //this._Sheet.MapSymbols(), 
                this._Sheet, this._Sheet.ShowDetailsInMap, Map.MapType.TK25);





            int iMissingGeography = 0;
            int iMissingSymbol = 0;

            // Testing every row
            foreach (System.Data.DataRow R in this._Sheet.DT().Rows)
            {
                // if no geography available
                if (iGeographyKeyPosition < 0 || iGeographyKeyPosition > R.Table.Columns.Count || R[iGeographyKeyPosition].Equals(System.DBNull.Value))
                    continue;
                try
                {
                    // the key for the object
                    string Key = R[iGeographyKeyPosition].ToString();
                    if (MapKeyObjects.ContainsKey(Key))
                    {
                        // Object already present - so it may have to be changed
                        if (MapType == FormSpreadsheet.MapType.TK25)
                            MapKeyObjects[Key].EvaluateContent(R);
                        else
                            MapKeyObjects[Key].UpdateContent(R);
                    }
                    else
                    {
                        // Object missing so far - so a new one will be created
                        string Value = "";
                        if (iGeographySymbolPosition > -1)
                            Value = R[iGeographySymbolPosition].ToString();
                        if (!this._Sheet.MapSymbols().ContainsKey(Value) && this._Sheet.MapSymbols().ContainsKey(Value.ToUpper()))
                            Value = Value.ToUpper();
                        if (!this._Sheet.MapSymbols().ContainsKey(Value) && this._Sheet.MapSymbols().ContainsKey(Value.ToLower()))
                            Value = Value.ToLower();
                        if (iGeographySymbolPosition > -1 && !this._Sheet.MapSymbols().ContainsKey(Value))
                            continue;
                        MapKeyObject MFO = new MapKeyObject(Key, this._Sheet, R, iGeographyPosition, iGeographySymbolPosition, iGeographyColorPosition);
                        MapKeyObjects.Add(Key, MFO);
                        if (MapType == FormSpreadsheet.MapType.Undefined)
                            MapKeyObjects[Key].UpdateContent(R);
                        else if (MapType == FormSpreadsheet.MapType.TK25)
                            MapKeyObjects[Key].EvaluateContent(R);
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            System.Collections.Generic.List<WpfSamplingPlotPage.GeoObject> Objects = new List<WpfSamplingPlotPage.GeoObject>();
            foreach (System.Collections.Generic.KeyValuePair<string, MapKeyObject> KV in MapKeyObjects)
            {
                try
                {
                    if (KV.Value.GeographyValue() == null)
                    {
                        iMissingGeography++;
                        continue;
                    }
                    if (KV.Value.SybolValue() == null)
                        continue;
                    if (KV.Value.SybolValue() != "")
                    {
                        string Val = KV.Value.SybolValue();
                    }
                    WpfSamplingPlotPage.GeoObject GO = new WpfSamplingPlotPage.GeoObject();
                    if (this._Sheet.ShowDetailsInMap)
                        GO.DisplayText = KV.Value.InfoDisplayText();
                    else
                        GO.DisplayText = KV.Value.Key();
                    GO.Identifier = KV.Value.Key();
                    GO.GeometryData = KV.Value.GeographyValue();
                    MapColor MC = this._Sheet.getMapColor(KV.Value.ColorValue());
                    GO.FillBrush = MC.Brush;
                    GO.StrokeBrush = MC.Brush;
                    GO.StrokeTransparency = this._Sheet.GeographyTransparency;
                    string SymbolValue = KV.Value.SybolValue();
                    if (!this._Sheet.MapSymbols().ContainsKey(SymbolValue))
                        SymbolValue = SymbolValue.ToUpper();
                    if (!this._Sheet.MapSymbols().ContainsKey(SymbolValue))
                        SymbolValue = SymbolValue.ToLower();
                    MapSymbol MS = null;
                    if (!this._Sheet.MapSymbols().ContainsKey(SymbolValue))
                    {
                        MS = new MapSymbol("", this._Sheet.GeographySymbolSize, "Circle filled");
                        MS.SymbolFilled = true;
                    }
                    else if (this._Sheet.MapSymbols().ContainsKey(SymbolValue))
                    {
                        MS = this._Sheet.MapSymbols()[SymbolValue];
                    }
                    if (MS.Symbol == WpfSamplingPlotPage.PointSymbol.None && MS.SymbolTitle == "Hide")
                        continue;
                    if (KV.Value.SybolValue().Length == 0 && iGeographySymbolPosition > -1)
                        MS = this._Sheet.MapSymbolForMissing;
                    GO.PointType = MS.Symbol;
                    if (MS.SymbolFilled)
                    {
                        GO.FillTransparency = this._Sheet.GeographyTransparency;
                        GO.StrokeThickness = 0;
                    }
                    else
                    {
                        GO.FillTransparency = 0;
                        GO.StrokeThickness = MS.SymbolSize;// this._Sheet.GeographyStrokeThickness;
                    }
                    GO.PointSymbolSize = MS.SymbolSize;
                    Objects.Add(GO);
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            return Objects;
        }

        private int GeographyObjectPosition(string Column, string TableAlias)
        {
            int i = -1;
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                {
                    if (DC.Value.DataTable().Alias() == TableAlias)
                    {
                        if (DC.Value.Column.Name == Column || DC.Value.DisplayText == Column)
                        {
                            i = DC.Key;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return i;
        }

        private WpfSamplingPlotPage.PointSymbol GeographySymbol(string Value)
        {
            if (this.GeographyPointSymbols().ContainsKey(Value))
                return this.GeographyPointSymbols()[Value];
            else
                return WpfSamplingPlotPage.PointSymbol.Circle;
        }

        private System.Collections.Generic.Dictionary<string, WpfSamplingPlotPage.PointSymbol> _GeographyPointSymbols;
        private System.Collections.Generic.Dictionary<string, WpfSamplingPlotPage.PointSymbol> GeographyPointSymbols()
        {
            if (this._GeographyPointSymbols == null)
            {
                this._GeographyPointSymbols = new Dictionary<string, WpfSamplingPlotPage.PointSymbol>();
            }
            return this._GeographyPointSymbols;
        }

        private void buttonShowMap_Click(object sender, EventArgs e)
        {
            try
            {
                System.Collections.Generic.List<WpfSamplingPlotPage.GeoObject> Objects;
                if (this._Sheet.GeographyUseKeyFilter && this._MapType == MapType.TK25)
                    Objects = this.GeoObjectsFiltered();
                else
                    Objects= this.GeoObjects();

                if (Objects.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("No objects for display in a map are defined so far");
                    return;
                }
                if (this._Sheet.GeographyColorColumn.Length == 0 || this._Sheet.GeographyColorTableAlias.Length == 0)
                {
                    if (System.Windows.Forms.MessageBox.Show("No colors have been defined so far. Do you want to define colors for the symbols?", "Missing colors", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        return;
                }
                if (this._Sheet.GeographySymbolColumn.Length == 0 || this._Sheet.GeographySymbolTableAlias.Length == 0)
                {
                    if (System.Windows.Forms.MessageBox.Show("No symbols have been defined so far. Do you want to define symbols shown in the map?", "Missing symbols", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        return;
                }

                DiversityWorkbench.Forms.FormGeography f = null;
                if (this._Sheet.GeographyMap.Length > 0)
                {
                    f = new DiversityWorkbench.Forms.FormGeography(Objects, this._Sheet.GeographyMap);
                }
                else
                {
                    f = new DiversityWorkbench.Forms.FormGeography(Objects);
                }
                f.ShowFrame(true);
                f.ShowInTaskbar = true;
                // not working this way - Filter will not be used
                //f.Show(); // no Dialog to enable positioning of legend
                
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    System.Collections.Generic.List<WpfSamplingPlotPage.GeoObject> SelectedObjects = new List<WpfSamplingPlotPage.GeoObject>();
                    f.ReceiveGeoObjectsAsList(out SelectedObjects);
                    if (SelectedObjects.Count > 0)
                    {
                        string Filter = "";
                        foreach (WpfSamplingPlotPage.GeoObject G in SelectedObjects)
                        {
                            if (this._Sheet.DataTables()[this._Sheet.GeographyKeyTableAlias].DataColumns()[this._Sheet.GeographyKeyColumn].Column.Table == null)
                            {
                                if (Filter.Length > 0)
                                    Filter += "\r\n";
                                Filter += "'" + G.Identifier + "'";
                            }
                            else
                            {
                                if (Filter.Length > 0)
                                    Filter += "\r\n";
                                if (this._Sheet.DataTables()[this._Sheet.GeographyKeyTableAlias].DataColumns()[this._Sheet.GeographyKeyColumn].Column.DataTypeBasicType != Data.Column.DataTypeBase.numeric)
                                    Filter += "'";
                                Filter += G.Identifier;
                                if (this._Sheet.DataTables()[this._Sheet.GeographyKeyTableAlias].DataColumns()[this._Sheet.GeographyKeyColumn].Column.DataTypeBasicType != Data.Column.DataTypeBase.numeric)
                                    Filter += "'";
                            }
                        }
                        this._Sheet.DataTables()[this._Sheet.GeographyKeyTableAlias].DataColumns()[this._Sheet.GeographyKeyColumn].FilterOperator = "|";
                        this._Sheet.DataTables()[this._Sheet.GeographyKeyTableAlias].DataColumns()[this._Sheet.GeographyKeyColumn].FilterValue = Filter;
                        this.initFilterValues();
                        this.getData();
                    }
                }
                f.Dispose();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonSetMapIcons_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Spreadsheet.FormMapSymbols f = new FormMapSymbols(this._Sheet, this._Setting);
                f.ShowDialog();
                f.Dispose();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private byte _GeographyTransparency = 0;

        private void toolStripButtonMapSymbols_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Spreadsheet.FormMapSymbols f = new FormMapSymbols(this._Sheet, this._Setting);
                f.setKeyword(this.getKeyword(KeywordTarget.MapSymbols));
                f.ShowDialog();
                f.Dispose();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        private void toolStripButtonLegend_Click(object sender, EventArgs e)
        {
            FormMapLegend f = new FormMapLegend(this._Sheet);
            f.setKeyword(this.getKeyword(KeywordTarget.MapLegend));
            f.Show();
        }

        private void wGS84ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this._Sheet.GeographySymbolColumn.Length == 0 ||
                this._Sheet.GeographySymbolTableAlias.Length == 0 ||
                this._Sheet.GeographyKeyColumn.Length == 0 ||
                this._Sheet.GeographyKeyTableAlias.Length == 0 ||
                this._Sheet.GeographyColorColumn.Length == 0 ||
                this._Sheet.GeographyColorTableAlias.Length == 0 ||
                this._Sheet.GeographyMap.Length == 0 ||
                this._Sheet.GeographyWGS84Column.Length == 0 ||
                this._Sheet.GeographyWGS84TableAlias.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please set all parameters for symbols, colors, coordinates etc. before creating a map");
                return;
            }

            this._MapType = MapType.WGS84;
            this.toolStripDropDownButtonMap.Image = wGS84ToolStripMenuItem.Image;
            this.openMap();
        }

        private void tK25ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (
                this._Sheet.GeographyKeyColumn.Length == 0 ||
                this._Sheet.GeographyKeyTableAlias.Length == 0 ||
                this._Sheet.GeographyMap.Length == 0 ||
                this._Sheet.EvaluationGazetteer().Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please set all parameters for before creating a map");
                return;
            }
            if (
                (this._Sheet.GeographyColorColumn.Length == 0 ||
                this._Sheet.GeographyColorTableAlias.Length == 0 )
                &&
                (this._Sheet.GeographySymbolColumn.Length == 0 ||
                this._Sheet.GeographySymbolTableAlias.Length == 0)
                )
            {
                System.Windows.Forms.MessageBox.Show("Please set parameters for symbols or colors etc. before creating a map");
                return;
            }
            this._MapType = MapType.TK25;
            this.toolStripDropDownButtonMap.Image = tK25ToolStripMenuItem.Image;
            this.openMap();
        }

        private void mapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this._Sheet.GeographySymbolColumn.Length == 0 ||
                this._Sheet.GeographySymbolTableAlias.Length == 0 ||
                this._Sheet.GeographyKeyColumn.Length == 0 ||
                this._Sheet.GeographyKeyTableAlias.Length == 0 ||
                this._Sheet.GeographyColorColumn.Length == 0 ||
                this._Sheet.GeographyColorTableAlias.Length == 0 ||
                this._Sheet.GeographyMap.Length == 0 ||
                this._Sheet.GeographyUnitGeoColumn.Length == 0 ||
                this._Sheet.GeographyUnitGeoTableAlias.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please set all parameters for symbols, colors, coordinates etc. before creating a map");
                return;
            }

            this._MapType = MapType.Map;
            this.toolStripDropDownButtonMap.Image = mapToolStripMenuItem.Image;
            this.openMap();
        }

        private void openMap()
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                System.Collections.Generic.List<WpfSamplingPlotPage.GeoObject> Objects;
                if (this._MapType == MapType.TK25)// this._Sheet.GeographyUseKeyFilter)
                    Objects = this.GeoObjectsFiltered(this._MapType);
                else if (this._MapType == MapType.WGS84)
                    Objects = this.GeoObjectsWGS84();
                else if (this._MapType == MapType.Map)
                    Objects = this.GeoObjectsMap();
                else
                    Objects = this.GeoObjects();

                if (Objects.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("No objects for display in a map are defined so far");
                    return;
                }

                DiversityWorkbench.Forms.FormGeography f = null;
                if (this._Sheet.GeographyMap.Length > 0 && this._MapType != MapType.Map)
                {
                    f = new DiversityWorkbench.Forms.FormGeography(Objects, this._Sheet.GeographyMap);
                }
                else if (this._MapType == MapType.Map)
                {
                    f = new DiversityWorkbench.Forms.FormGeography(Objects);//, true);
                }
                else
                {
                    f = new DiversityWorkbench.Forms.FormGeography(Objects);
                }
                f.ShowFrame(true);
                f.ShowInTaskbar = true;
                // not working this way - Filter will not be used
                //f.Show(); // no Dialog to enable positioning of legend
                f.Width = this.Width - 10;
                f.Height = this.Height - 10;
                f.StartPosition = FormStartPosition.CenterParent;
                this.Cursor = System.Windows.Forms.Cursors.Default;
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    System.Collections.Generic.List<WpfSamplingPlotPage.GeoObject> SelectedObjects = new List<WpfSamplingPlotPage.GeoObject>();
                    f.ReceiveGeoObjectsAsList(out SelectedObjects);
                    if (SelectedObjects.Count > 0)
                    {
                        string Filter = "";
                        string TargetTableAlias = "";
                        string TargetTableIdentityColumn = "";
                        string TargetTableIdentityColumnDisplayText = "";
                        foreach (WpfSamplingPlotPage.GeoObject G in SelectedObjects)
                        {
                            if (this._MapType == MapType.TK25)
                            {
                                if (this._Sheet.DataTables()[this._Sheet.GeographyKeyTableAlias].DataColumns()[this._Sheet.GeographyKeyColumn].Column.Table == null)
                                {
                                    if (Filter.Length > 0)
                                        Filter += "\r\n";
                                    Filter += "'" + G.Identifier + "'";
                                }
                                else
                                {
                                    if (Filter.Length > 0)
                                        Filter += "\r\n";
                                    if (this._Sheet.DataTables()[this._Sheet.GeographyKeyTableAlias].DataColumns()[this._Sheet.GeographyKeyColumn].Column.DataTypeBasicType != Data.Column.DataTypeBase.numeric)
                                        Filter += "'";
                                    Filter += G.Identifier;
                                    if (this._Sheet.DataTables()[this._Sheet.GeographyKeyTableAlias].DataColumns()[this._Sheet.GeographyKeyColumn].Column.DataTypeBasicType != Data.Column.DataTypeBase.numeric)
                                        Filter += "'";
                                }
                                this._Sheet.DataTables()[this._Sheet.GeographyKeyTableAlias].DataColumns()[this._Sheet.GeographyKeyColumn].FilterOperator = "|";
                                this._Sheet.DataTables()[this._Sheet.GeographyKeyTableAlias].DataColumns()[this._Sheet.GeographyKeyColumn].FilterValue = Filter;
                            }
                            else if (this._MapType == MapType.WGS84 || this._MapType == MapType.Map)
                            {
                                int iGeographyObjectPosition = -1;// this.GeographyObjectPosition(this._Sheet.GeographyKeyColumn, this._Sheet.GeographyKeyTableAlias);
                                try
                                {
                                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Spreadsheet.DataTable> KV in this._Sheet.DataTables())
                                    {
                                        if (KV.Value.Type() == DataTable.TableType.Target)
                                        {
                                            TargetTableAlias = KV.Key;
                                            TargetTableIdentityColumn = KV.Value.DataColumns()[KV.Value.IdentityColumn].Name;
                                            TargetTableIdentityColumnDisplayText = KV.Value.DataColumns()[KV.Value.IdentityColumn].DisplayText;
                                            iGeographyObjectPosition = this.GeographyObjectPosition(KV.Value.DataColumns()[KV.Value.IdentityColumn].DisplayText, KV.Key);
                                            break;
                                        }
                                    }
                                    if (!this._Sheet.DataTables()[TargetTableAlias].DataColumns().ContainsKey(TargetTableIdentityColumnDisplayText) &&
                                        this._Sheet.DataTables()[TargetTableAlias].DataColumns().ContainsKey(TargetTableIdentityColumn))
                                        TargetTableIdentityColumnDisplayText = TargetTableIdentityColumn;
                                    if (this._Sheet.DataTables()[TargetTableAlias].DataColumns()[TargetTableIdentityColumnDisplayText].Column.Table == null)
                                    {
                                        if (Filter.Length > 0)
                                            Filter += "\r\n";
                                        Filter += "'" + G.Identifier + "'";
                                    }
                                    else
                                    {
                                        if (Filter.Length > 0)
                                            Filter += "\r\n";
                                        if (this._Sheet.DataTables()[TargetTableAlias].DataColumns()[TargetTableIdentityColumnDisplayText].Column.DataTypeBasicType != Data.Column.DataTypeBase.numeric)
                                            Filter += "'";
                                        Filter += G.Identifier;
                                        if (this._Sheet.DataTables()[TargetTableAlias].DataColumns()[TargetTableIdentityColumnDisplayText].Column.DataTypeBasicType != Data.Column.DataTypeBase.numeric)
                                            Filter += "'";
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                }
                                this._Sheet.DataTables()[TargetTableAlias].DataColumns()[TargetTableIdentityColumnDisplayText].FilterOperator = "|";
                                this._Sheet.DataTables()[TargetTableAlias].DataColumns()[TargetTableIdentityColumnDisplayText].FilterValue = Filter;
                            }
                            if (Filter.Length > 0 && this._MapType == MapType.WGS84)
                            {
                                bool HeaderChanged = false;
                                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                                {
                                    if (DC.Value.DataTable().Alias() == TargetTableAlias
                                        && DC.Value.Name == TargetTableIdentityColumn)
                                    {
                                        if (DC.Value.IsHidden || !DC.Value.IsVisible)
                                        {
                                            DC.Value.IsVisible = true;
                                            DC.Value.IsHidden = false;
                                            HeaderChanged = true;
                                        }
                                        break;
                                    }
                                }
                                if (HeaderChanged)
                                {
                                    this.RequeryAllGrids();
                                    //this.setTableMarker();
                                }
                            }
                        }
                        this.initFilterValues();
                        this.getData();
                    }
                }
                f.Dispose();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

        }

        #endregion

        #region Connect

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            this.ConnectToDatabase();
        }


        private void ConnectToDatabase()
        {
            try
            {
                DiversityWorkbench.Forms.FormConnectToDatabase f = new DiversityWorkbench.Forms.FormConnectToDatabase();
                f.setHelpProviderNameSpace(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace(), "Login");
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    this._Sheet.ResetConnection();
                }
                f.Dispose();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;

            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                this.buttonConnect.Image = DiversityWorkbench.ResourceWorkbench.Database;
            }
            this.setProjectSource();
            this.getData();
            this.Text = this._Sheet.DisplayText();
        }

        #endregion

        #region Export

        private void xMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("available in upcoming version");
            return;


            this.saveFileDialog.RestoreDirectory = true;
            this.saveFileDialog.ShowDialog();
            this.saveFileDialog.Filter = "(*.xml)|*.xml";
            if (this.saveFileDialog.FileName.Length > 0)
            {
                string ExportFile = "";
                ExportFile = this.saveFileDialog.FileName;
                if (!ExportFile.EndsWith(".xml"))
                    ExportFile += ".xml";
                System.Xml.XmlWriter w = null;
                try
                {
                    System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                    settings.Indent = true;
                    settings.Encoding = System.Text.Encoding.UTF8;
                    settings.CloseOutput = true;
                    settings.CheckCharacters = true;
                    settings.NewLineChars = "\r\n";
                    System.Data.DataSet ds = new System.Data.DataSet();
                    System.Data.DataTable dt = this._Sheet.DT().Copy();
                    dt.TableName = this._Sheet.Target();
                    ds.Tables.Clear();
                    ds.Tables.Add(dt);
                    w = System.Xml.XmlWriter.Create(ExportFile, settings);
                    ds.Tables[0].WriteXml(w, XmlWriteMode.WriteSchema);
                    //this._Sheet.DT().WriteXml(w, XmlWriteMode.IgnoreSchema);
                    ds.Tables.Clear();
                    ds.Dispose();
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                finally
                {
                    w.Flush();
                    w.Close();
                }
            }
            this.saveFileDialog.Dispose();
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            bool IncludeHiddenColumns = false;
            if (System.Windows.Forms.MessageBox.Show("Some columns like the primary key are hidden from the interface. Do you want to include these hidden columns into the export", "Include hidden columns", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                IncludeHiddenColumns = true;
            this.saveFileDialog.RestoreDirectory = true;
            this.saveFileDialog.Filter = "(*.txt)|*.txt";
            this.saveFileDialog.ShowDialog();
            if (this.saveFileDialog.FileName.Length > 0)
            {
                string ExportFile = "";
                ExportFile = this.saveFileDialog.FileName;
                if (!ExportFile.EndsWith(".txt"))
                    ExportFile += ".txt";
                if (!this._Sheet.ExportToTextFile(ExportFile, IncludeHiddenColumns))
                    System.Windows.Forms.MessageBox.Show("Export failed");
                else
                    System.Windows.Forms.MessageBox.Show("Data exported to " + ExportFile);
            }
            this.saveFileDialog.Dispose();
        }

        #endregion

        #region Context menus

        // TableMarker
        private void removeTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string x = e.GetType().ToString();
        }

       
        // Data
        private void toolStripMenuItemRemove_Click(object sender, EventArgs e)
        {
            //if (this.dataGridView.SelectedCells.Count == 1 && this.dataGridView.SelectedCells[0].ColumnIndex > 0)
            //{
            //    this.dataGridView.SelectedCells[0].Value = null;
            //}
            try
            {
                if (this.dataGridView.SelectedCells[0].ColumnIndex > 0)
                {
                    this._SuppressRescanOfDataGrid = true;
                    int iNoUpdate = 0;
                    int iColumn = this.dataGridView.SelectedCells[0].ColumnIndex;
                    System.Collections.Generic.List<int> SelectedCells = new List<int>();
                    foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.SelectedCells)
                    {
                        SelectedCells.Add(C.RowIndex);
                    }
                    foreach (int i in SelectedCells)
                    {
                        if (!this._Sheet.SelectedColumns()[iColumn].DataTable().AllowUpdate())
                        {
                            iNoUpdate++;
                            continue;
                        }
                        this.dataGridView.Rows[i].Cells[iColumn].Value = System.DBNull.Value;
                    }
                    this._SuppressRescanOfDataGrid = false;
                    this.getData();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        // Transfer to clipboard
        private void transferValuesToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._SuppressRescanOfDataGrid = true;
            try
            {
                if (this.dataGridView.SelectedCells.Count < 2
                    && this.dataGridView.SelectedCells[0].ColumnIndex == 0
                    && this.dataGridView.SelectedCells[0].RowIndex == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Nothing selected");
                    return;
                }
                if (!this._Sheet.SelectedColumns()[this.dataGridView.SelectedCells[0].ColumnIndex].Column.IsNullable)
                {
                    System.Windows.Forms.MessageBox.Show("No clearing allowed. This column can not be empty");
                    return;
                }
                DiversityWorkbench.Forms.FormFunctions.WriteToClipboard(this.dataGridView);
                int iColumn = this.dataGridView.SelectedCells[0].ColumnIndex;
                System.Collections.Generic.List<int> SelectedCells = new List<int>();
                foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.SelectedCells)
                {
                    SelectedCells.Add(C.RowIndex);
                }
                int iNoUpdate = 0;
                foreach (int i in SelectedCells)
                {
                    if (!this._Sheet.SelectedColumns()[iColumn].DataTable().AllowUpdate())
                    {
                        iNoUpdate++;
                        continue;
                    }
                    this.dataGridView.Rows[i].Cells[iColumn].Value = System.DBNull.Value;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this._SuppressRescanOfDataGrid = false;
            this.getData();
        }

        private void copyValuesToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormFunctions.WriteToClipboard(this.dataGridView);
        }

        private void insertValesFromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Clipboard.ContainsData(System.Windows.Forms.DataFormats.CommaSeparatedValue)
                || System.Windows.Forms.Clipboard.ContainsData(System.Windows.Forms.DataFormats.Text))
            {
                // finding the top row
                int IndexTopRow = this.dataGridView.Rows.Count;
                if (this.dataGridView.SelectedCells.Count > 0)
                {
                    foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.SelectedCells)
                        if (IndexTopRow > C.RowIndex) IndexTopRow = C.RowIndex;
                }

                // finding the first column
                int IndexFirstColumn = this.dataGridView.Columns.Count;
                if (this.dataGridView.SelectedCells.Count > 0)
                {
                    foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.SelectedCells)
                        if (IndexFirstColumn > C.ColumnIndex) IndexFirstColumn = C.ColumnIndex;
                }

                // parsing the content of the clipboard
                System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues = DiversityWorkbench.Forms.FormFunctions.ClipBoardValues;// this.ClipBoardValues;
                System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums = DiversityWorkbench.Forms.FormFunctions.GridColums(this.dataGridView);
                if (!DiversityWorkbench.Forms.FormFunctions.CanCopyClipboardInDataGrid(IndexTopRow, ClipBoardValues, GridColums, this.dataGridView))
                    return;
                try
                {
                    for (int ii = 0; ii < GridColums.Count; ii++) // the columns
                    {
                        for (int i = 0; i < ClipBoardValues.Count; i++) // the rows
                        {
                            if (this.dataGridView.Rows[IndexTopRow + i].Cells[GridColums[ii].Index].ReadOnly)
                                continue;
                            if (DiversityWorkbench.Forms.FormFunctions.ValueIsValid(this.dataGridView, GridColums[ii].Index, ClipBoardValues[i][ii]))
                            {
                                this.dataGridView.Rows[IndexTopRow + i].Cells[GridColums[ii].Index].Value = ClipBoardValues[i][ii];
                                //this.checkForMissingAndDefaultValues(this.dataGridView.Rows[IndexTopRow + i].Cells[GridColums[ii].Index], true);
                            }
                            else
                            {
                                string Message = ClipBoardValues[i][ii] + " is not a valid value for "
                                    + this.dataGridView.Columns[GridColums[ii].Index].DataPropertyName
                                    + "\r\n\r\nDo you want to try to insert the other values?";
                                if (System.Windows.Forms.MessageBox.Show(Message, "Invalid value", MessageBoxButtons.YesNo) == DialogResult.No)
                                    break;
                            }
                            if (i + IndexTopRow + 3 > this.dataGridView.Rows.Count)
                                continue;
                        }
                    }
                }
                catch { }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Only text and spreadsheet values can be copied");
                return;
            }
        }

        // Copy template
        private void useThisRowAsTemplateForNewRowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dataGridView.SelectedRows.Count == 1)
            {
                foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
                {
                    if (DC.Value.Type() == DataColumn.ColumnType.Operation || DC.Value.Type() == DataColumn.ColumnType.Spacer)
                        continue;
                    if (DC.Value.Type() == DataColumn.ColumnType.Data)
                    {
                        if (DC.Value.Column.IsIdentity || !DC.Value.IsVisible)
                            continue;
                        if (DC.Value.Column.ForeignRelations != null &&
                            DC.Value.Column.ForeignRelations.Count > 0 &&
                            DC.Value.DataTable().ParentTable() != null &&
                            DC.Value.Column.ForeignRelations.ContainsKey(DC.Value.DataTable().ParentTable().Name))
                            continue;
                    }
                    this._Sheet.DtAdding.Rows[0][DC.Key] = this._Sheet.DT().Rows[this.dataGridView.SelectedRows[0].Index][DC.Key];
                    this._Sheet.SelectedColumns()[DC.Key].DefaultForAdding = this._Sheet.DtAdding.Rows[0][DC.Key].ToString();
                }
                this.dataGridViewAdding.Refresh();
            }
            else
                System.Windows.Forms.MessageBox.Show("Please mark the row that should be used as template");
        }

        // Adding
        private void removeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewAdding.SelectedCells.Count == 1 && this.dataGridViewAdding.SelectedCells[0].ColumnIndex > 0)
            {
                this.dataGridViewAdding.SelectedCells[0].Value = null;
                if (this.dataGridViewAdding.SelectedCells[0].Value != null)
                    this.dataGridViewAdding.SelectedCells[0].Value = System.DBNull.Value;
            }
        }

        // Filter
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewFilter.SelectedCells.Count == 1 && this.dataGridViewFilter.SelectedCells[0].ColumnIndex > 0)
            {
                if (this._Sheet.SelectedColumns()[this.dataGridViewFilter.SelectedCells[0].ColumnIndex].Type() == DataColumn.ColumnType.Operation)
                {
                    this.dataGridViewFilter.SelectedCells[0].Value = "";
                    this._Sheet.SelectedColumns()[this.dataGridViewFilter.SelectedCells[0].ColumnIndex].DataTable().FilterOperator = "";
                    // this._Sheet.SelectedColumns()[this.dataGridViewFilter.SelectedCells[0].ColumnIndex].DataTable().f
                }
                else
                {
                    this.ResetColumnFilter(this.dataGridViewFilter.SelectedCells[0].ColumnIndex);
                }
                this.FilterAlarm_Start();
            }
        }
        
        // Whole filter
        private void toolStripMenuItemClear_Click(object sender, EventArgs e)
        {
            foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
            {
                try
                {
                    if (DC.Value != null &&
                        DC.Value.Type() == DataColumn.ColumnType.Data &&
                        DC.Value.IsVisible)
                    {
                        this.ResetColumnFilter(DC.Key);
                    }
                    else if (DC.Value != null &&
                        DC.Value.Type() == DataColumn.ColumnType.Operation &&
                        DC.Value.IsVisible &&
                        this.dataGridViewFilter.Rows[0].Cells[DC.Key].Value != null &&
                        this.dataGridViewFilter.Rows[0].Cells[DC.Key].Value.ToString().Length > 0)
                    {
                        this.dataGridViewFilter.Rows[0].Cells[DC.Key].Value = "";
                        this.dataGridViewFilter.Rows[0].Cells[DC.Key].ToolTipText = "";
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            foreach (System.Collections.Generic.KeyValuePair<string, DataTable> DT in this._Sheet.DataTables())
            {
                if (DT.Value.FilterOperator.Length > 0)
                {
                    DT.Value.FilterOperator = "";
                }
                foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in DT.Value.DataColumns())
                {
                    if (DC.Value != null &&
                        DC.Value.Type() == DataColumn.ColumnType.Data &&
                        DC.Value.FilterValue != null &&
                        DC.Value.FilterValue.Length > 0)
                    {
                        DC.Value.FilterValue = "";
                        DC.Value.FilterOperator = "";
                        //if (DC.Value.Column.IsNullable)
                        //this.ResetColumnFilter(DC.Key);
                    }
                }
            }
            this.FilterAlarm_Start();
            this._Sheet.ResetFilter();
        }

        private void ResetColumnFilter(int ColumnIndex)
        {
            try
            {
                this._Sheet.SelectedColumns()[ColumnIndex].FilterValue = "";// null;
                //this._Sheet.SelectedColumns()[ColumnIndex].FilterOperator = "=";
                this._Sheet.SelectedColumns()[ColumnIndex].FilterModuleLinkRoot = "";
                this._Sheet.SelectedColumns()[ColumnIndex].OrderDirection = DataColumn.OrderByDirection.none;// = "";// null;
                this._Sheet.SelectedColumns()[ColumnIndex].OrderSequence = null;// = "";// null;
                if (this._Sheet.SelectedColumns()[ColumnIndex].Column.IsNullable)
                {
                    this.dataGridViewFilter.Rows[0].Cells[ColumnIndex].Value = null;
                    this._Sheet.DtFilter.Rows[0][this._Sheet.SelectedColumns()[ColumnIndex].DisplayText] = System.DBNull.Value;
                }
                else
                {
                    if (this._Sheet.DtFilter.Columns[this._Sheet.SelectedColumns()[ColumnIndex].DisplayText].AllowDBNull)
                        this.dataGridViewFilter.Rows[0].Cells[ColumnIndex].Value = "";
                    this._Sheet.DtFilter.Rows[0][this._Sheet.SelectedColumns()[ColumnIndex].DisplayText] = System.DBNull.Value;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        // Show legend for map
        private void showLegendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMapLegend f = new FormMapLegend(this._Sheet);
            f.Show();
        }

        // Show settings for all linked columns
        private void showAllSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool HasBeenReset = false;
            if (this._FixedSourceSetting != null)
            {
                FormUserSettings f = new FormUserSettings(this._FixedSourceSetting);
                f.ShowDialog();
                HasBeenReset = f.HasBeenReset();
                f.Dispose();
            }
            else
            {
                FormUserSettings f = new FormUserSettings();
                f.ShowDialog();
                HasBeenReset = f.HasBeenReset();
                f.Dispose();
            }
            if (HasBeenReset)
            {
                this.FixingAndEditingReset();
            }
        }

        #endregion

        #region Order by

        //private void initOrderBy()
        //{
        //    if (this._Sheet.getOrderDirection() == Sheet.OrderDirection.Asc)
        //    {
        //        this.buttonOrderByDirection.Image = DiversityWorkbench.Properties.Resources.ArrowDown;
        //        this.toolTip.SetToolTip(this.buttonOrderByDirection, "Order descending");
        //    }
        //    else
        //    {
        //        this.buttonOrderByDirection.Image = DiversityWorkbench.Properties.Resources.ArrowUp;
        //        this.toolTip.SetToolTip(this.buttonOrderByDirection, "Order ascending");
        //    }
        //    this.comboBoxOrderBy.Text = this._Sheet.getOrderColumn();
        //}

        //private void comboBoxOrderBy_DropDown(object sender, EventArgs e)
        //{
        //    System.Collections.Generic.List<string> OrderColumns = new List<string>();
        //    OrderColumns.Add("");
        //    string StartTableAlias = "";
        //    foreach (System.Collections.Generic.KeyValuePair<int, DataColumn> DC in this._Sheet.SelectedColumns())
        //    {
        //        // only visible data columns
        //        if (!DC.Value.IsVisible || DC.Value.Type() != DataColumn.ColumnType.Data)
        //            continue;

        //        if (StartTableAlias.Length == 0 && DC.Value.IsVisible)
        //            StartTableAlias = DC.Value.DataTable().Alias();
        //        if (StartTableAlias.Length > 0
        //            && StartTableAlias != DC.Value.DataTable().Alias())
        //        {
        //            // walk through the visible columns until the next change
        //            if (DC.Value.DataTable().Type() == DataTable.TableType.Root
        //            || DC.Value.DataTable().Type() == DataTable.TableType.Target)
        //            {
        //                break;
        //            }
        //            if (DC.Value.DataTable().ParentTable() != null
        //                && DC.Value.DataTable().ParentTable().Alias() != StartTableAlias)
        //            {
        //                if (DC.Value.DataTable().ParentTable().Type() == DataTable.TableType.Single &&
        //                    DC.Value.DataTable().ParentTable().ParentTable() != null &&
        //                    DC.Value.DataTable().ParentTable().ParentTable().Alias() == StartTableAlias)
        //                {
        //                }
        //                else
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //        OrderColumns.Add(DC.Value.DisplayText);
        //    }
        //    this.comboBoxOrderBy.DataSource = OrderColumns;
        //}

        //private void comboBoxOrderBy_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    if (this.comboBoxOrderBy.SelectedItem.ToString() != this._Sheet.getOrderColumn())
        //    {
        //        this.FilterAlarm_Start();
        //        this._Sheet.setOrderColumn(this.comboBoxOrderBy.SelectedItem.ToString());
        //    }
        //    if (this._Sheet.getOrderColumn().Length > 0)
        //    {
        //        this.buttonNext.Enabled = false;
        //        this.buttonPrevious.Enabled = false;
        //        this._Sheet.ResetOffset();
        //    }
        //    else
        //    {
        //        this.buttonNext.Enabled = true;
        //        this.buttonPrevious.Enabled = true;
        //    }
        //}
        
        //private void buttonOrderByDirection_Click(object sender, EventArgs e)
        //{
        //    if (this._Sheet.getOrderDirection() == Sheet.OrderDirection.Asc)
        //    {
        //        this._Sheet.setOrderDirection(Sheet.OrderDirection.Desc);
        //    }
        //    else
        //    {
        //        this._Sheet.setOrderDirection(Sheet.OrderDirection.Asc);
        //    }
        //    this.initOrderBy();
        //    this.FilterAlarm_Start();
        //}

        #endregion

        #region Read only

        private void buttonReadOnly_Click(object sender, EventArgs e)
        {
            this._Sheet.ReadOnly = !this._Sheet.ReadOnly;
            this.setControlsAccordingToReadOnly();
        }

        private void setControlsAccordingToReadOnly()
        {
            if (!this._Sheet.ReadOnly)
                this.buttonReadOnly.Image = DiversityWorkbench.Properties.Resources.Speadsheet;
            else
                this.buttonReadOnly.Image = DiversityWorkbench.Properties.Resources.Spreadsheet;
            this.initColumnWidthsAndVisibility();
            this.adaptControlsToRowHeaderWidth(this.dataGridView.RowHeadersWidth);
            this.EditingEnableEditing();//!this._Sheet.ReadOnly);
            this.EnableAdding(!this._Sheet.ReadOnly);
            this.FilterAlarm_Start();
        }
        
        #endregion

        #region Main form for details
        
        private void buttonMainForm_Click(object sender, EventArgs e)
        {
            try
            {
                string Column = MasterColumn();
                int ID = int.Parse(this._Sheet.DT().Rows[this.dataGridView.SelectedCells[0].RowIndex][Column].ToString());
                this._Sheet.iMainForm.ShowInMainForm(ID, true, false);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string MasterColumn()
        {
            string MasterTableAlias = this._Sheet.MasterQueryColumn.DataTable().Alias();
            string Column = this._Sheet.DataTables()[MasterTableAlias].DataColumns()[this._Sheet.MasterQueryColumn.Name].DisplayText;
            if (!this._Sheet.DT().Columns.Contains(Column)) // Master column may not be visible
            {
                Column = this.TargetMasterColumn();
            }
            return Column;
        }

        private string TargetMasterColumn()
        {
            string Column = "";
            // finding the column related to the master column within the target table
            foreach (System.Collections.Generic.KeyValuePair<string, DataTable> DT in this._Sheet.DataTables())
            {
                if (DT.Value.Type() == DataTable.TableType.Target)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in DT.Value.DataColumns())
                    {
                        string MasterTable = this._Sheet.MasterQueryColumn.DataTable().Name;
                        string MasterColumn = this._Sheet.MasterQueryColumn.Name;
                        if (DT.Value.ForeignRelationColumns(MasterTable) != null && DT.Value.ForeignRelationColumns(MasterTable).Any())
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, string> FR in DT.Value.ForeignRelationColumns(MasterTable))
                            {
                                if (FR.Value == MasterColumn)
                                {
                                    Column = DC.Value.DisplayText;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            } 
            return Column;
        }

        #endregion

        #region Geometry

        private void buttonSetGeometryIcons_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
        }

        private void buttonShowGeometry_Click(object sender, EventArgs e)
        {

            this._MapType = MapType.Geometry;
            this.openGeometry();
        }

        private void openGeometry()
        {
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
        }

        private void setMapPath(string Path)
        {
            this._Sheet.GeometryPlan = Path;
        }


        #endregion

        #region Autosuggest

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                TextBox textBox = e.Control as TextBox;
                this.SetAutoSuggest(textBox, this.dataGridView, AutoCompleteMode.SuggestAppend);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void dataGridViewFilter_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                TextBox textBox = e.Control as TextBox;
                this.SetAutoSuggest(textBox, this.dataGridViewFilter);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void dataGridViewAdding_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                TextBox textBox = e.Control as TextBox;
                this.SetAutoSuggest(textBox, this.dataGridViewAdding);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void SetAutoSuggest(TextBox textBox, System.Windows.Forms.DataGridView dataGridView, AutoCompleteMode autoCompleteMode = AutoCompleteMode.Suggest)
        {
            try
            {
                if (textBox != null)
                {
                    string ColumnName = this._Sheet.SelectedColumns()[dataGridView.SelectedCells[0].ColumnIndex].Column.Name; // this.dataGridView.Columns[dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
                    string TableName = this._Sheet.SelectedColumns()[dataGridView.SelectedCells[0].ColumnIndex].DataTable().Name;
                    System.Windows.Forms.AutoCompleteStringCollection autoCompleteStringCollection = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionOnDemand(TableName, ColumnName);
                    textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    textBox.AutoCompleteMode = autoCompleteMode;
                    textBox.AutoCompleteCustomSource = autoCompleteStringCollection;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Manual

        /// <summary>
        /// #35
        /// setting the keyword for the help provider
        /// </summary>
        public void setKeyword(string Keyword)
        {
            this.helpProvider.SetHelpKeyword(this, Keyword);
            this.setKeyword(KeywordTarget.Spreadsheet, Keyword);
        }

        public void setKeyword(string Keyword, KeywordTarget keywordTarget)
        {
            this.setKeyword(keywordTarget, Keyword);

            switch (keywordTarget)
            {
                case KeywordTarget.Map:
                    System.Collections.Generic.List<System.Windows.Forms.Control> Controls = new List<System.Windows.Forms.Control>();
                    //Controls.Add(this.buttonSetGeometryIcons);
                    //Controls.Add(this.buttonShowGeometry);
                    Controls.Add(this.buttonSetMapIcons);
                    Controls.Add(this.buttonShowMap);
                    Controls.Add(this.toolStripMap);
                    foreach (System.Windows.Forms.Control C in Controls)
                    {
                        this.helpProvider.SetHelpKeyword(C, Keyword);
                    }
                    break;
                case KeywordTarget.MapLegend:
                    break;
                case KeywordTarget.MapColors:
                case KeywordTarget.MapSymbols:
                    break;
            }
        }

        public enum KeywordTarget
        {
            Map,
            MapLegend,
            MapColors,
            MapSymbols,
            Spreadsheet
        }

        private System.Collections.Generic.Dictionary<KeywordTarget, string> _KeywordTargets = new Dictionary<KeywordTarget, string>();
        private void setKeyword(KeywordTarget Target, string Keyword)
        {
            if (this._KeywordTargets == null)
                this._KeywordTargets = new Dictionary<KeywordTarget, string>();
            if (this._KeywordTargets.ContainsKey(Target))
                this._KeywordTargets[Target] = Keyword;
            else
                this._KeywordTargets.Add(Target, Keyword);
        }

        private string getKeyword(KeywordTarget Target)
        {
            if (this._KeywordTargets == null)
                this._KeywordTargets = new Dictionary<KeywordTarget, string>();
            if (this._KeywordTargets.ContainsKey(Target))
                return this._KeywordTargets[Target];
            else
                return "";
        }


        /// <summary>
        /// Adding event deletates to form and controls
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task InitManual()
        {
            try
            {

                DiversityWorkbench.DwbManual.Hugo manual = new Hugo(this.helpProvider, this);
                if (manual != null)
                {
                    await manual.addKeyDownF1ToForm();
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// ensure that init is only done once
        /// </summary>
        private bool _InitManualDone = false;


        /// <summary>
        /// KeyDown of the form adding event deletates to form and controls within the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!_InitManualDone)
                {
                    await this.InitManual();
                    _InitManualDone = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion

    }
}
