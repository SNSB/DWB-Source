using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DiversityWorkbench.Forms;
using DWBServices.WebServices;
using Microsoft.Extensions.DependencyInjection;
using static DWBServices.WebServices.DwbServiceEnums;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Web;
using DWBServices;
using DWBServices.WebServices.TaxonomicServices;
using DWBServices.WebServices.TaxonomicServices.CatalogueOfLife;

namespace DiversityWorkbench.UserControls
{
    /// <summary>
    /// Additional bindings of columns
    /// </summary>
    public struct RemoteValueBinding
    {
        /// <summary>
        /// MW 4.5.2015: The update mode for a column
        /// NotSet: default, no changes on code necessary
        /// OnlyEmpty: The new values will only be inserted if the current data are empty
        /// AskForUpdate: In this case the user will be asked if a filled value should be changed
        /// </summary>
        public enum UpdateMode { NotSet, OnlyEmpty, AskForUpdate }

        public System.Windows.Forms.BindingSource BindingSource;
        public string Column;
        public string RemoteParameter;
        public UpdateMode ModeOfUpdate;
    }

    public partial class UserControlModuleRelatedEntry : UserControl
    {
        #region Parameter

        private bool _localValuesPossible;

        private string _module;
        private string _SQL;
        private string _DisplayColumn;
        private string _ValueColumn;

        private bool _ShowInfo = false;
        public enum FixSourceMode { None, Database, CacheDB, Webservice, Undefined }
        private FixSourceMode _fixSourceMode = FixSourceMode.Undefined;

        public string ValueColumn
        {
            get { return _ValueColumn; }
            //set { _ValueColumn = value; }
        }
        private string _TableName;

        public string TableName
        {
            get { return _TableName; }
            //set { _TableName = value; }
        }
        private System.Data.DataTable _dtTable;
        private DiversityWorkbench.IWorkbenchUnit _IWorkbenchUnit;
        //private bool _DatabaseIsAvailable = false;
        private Microsoft.Data.SqlClient.SqlDataAdapter _DataAdapter;
        private System.Windows.Forms.BindingSource _BindingSource;
        private bool? _CanDeleteConnectionToModule;
        private bool _LinkDeleteConnectionToModuleToTableGrant = false;

        public bool LinkDeleteConnectionToModuleToTableGrant
        {
            get { return _LinkDeleteConnectionToModuleToTableGrant; }
            set { _LinkDeleteConnectionToModuleToTableGrant = value; }
        }

        private System.Collections.Generic.Dictionary<string, string> _Values;

        private string _ListInDatabase = "";
        private bool _IsListInDatabase = false;
        private bool _AnyServiceAvailable = false;
        private enum ConnectionState { NotConnected, Connected };
        private enum ValueState { LocalValue, RemoteValue, NoValue, unknown };
        private enum BindingState { Binding_Unit, NoBinding_NoUnit, Binding_NoUnit, NoBinding_Unit };
        private enum UriAvailableState { Available, NoAccess };

        DiversityWorkbench.RemoteValues _RemoteValues;
        System.Collections.Generic.List<DiversityWorkbench.UserControls.RemoteValueBinding> _RemoteValueBindings;

        private bool _SupressEmptyRemoteValues = false;

        public bool SupressEmptyRemoteValues
        {
            get { return _SupressEmptyRemoteValues; }
            set { _SupressEmptyRemoteValues = value; }
        }

        private string _Domain = "";

        public string Domain
        {
            get { return _Domain; }
            set { _Domain = value; }
        }

        private string _DependsOnUri = "";

        public string DependsOnUri
        {
            get { return _DependsOnUri; }
            set { _DependsOnUri = value; }
        }

        private bool _ShowHtmlUnitValues = false;
        public bool ShowHtmlUnitValues
        {
            get { return _ShowHtmlUnitValues; }
            set { _ShowHtmlUnitValues = value; }
        }

        public bool HtmlUnitValues
        {
            get
            {
                if (!_ShowHtmlUnitValues)
                    return false;
                if (_IWorkbenchUnit != null && _IWorkbenchUnit is WorkbenchUnit)
                {
                    //if ((_IWorkbenchUnit as WorkbenchUnit).FeatureList.Contains(WorkbenchUnit.ClientFeature.HtmlUnitValues))
                    return true;
                }
                return false;
            }
        }

        #endregion

        #region Construction

        public UserControlModuleRelatedEntry()
        {
            InitializeComponent();
            try
            {
                this._dtTable = new DataTable();
                this.comboBoxLocalValues.DataSource = this._dtTable;
#if DEBUG
                if (this._BindingSource != null)
                {
                    this._BindingSource.CurrentChanged += new System.EventHandler(this.currentBinding_Changed);
                }
                //this.textBoxValue.Tag = this.comboBoxLocalValues;
#endif
                this.setControls();
                //this._BindingSource.CurrentChanged += new System.EventHandler(this.BindingSource_CurrentChanged);
                //this._BindingSource.CurrentItemChanged += new System.EventHandler(this.BindingSource_CurrentItemChanged);
                //this._BindingSource.PositionChanged
                //System.Windows.Forms.BindingManagerBase BMB = this._BindingSource.CurrencyManager;
                //BMB.PositionChanged += new System.EventHandler(this.BindingSource_PositionChanged);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Properties

        public System.Data.DataTable DtTable
        {
            get
            {
                if (this._dtTable == null)
                    this._dtTable = new DataTable();
                return _dtTable;
            }
            //set { _dtTable = value; }
        }

        private string SQL
        {
            //get { return _SQL; }
            set { _SQL = value; }
        }

        public DiversityWorkbench.ServerConnection ServerConnection
        {
            get
            {
                DiversityWorkbench.ServerConnection S = this._IWorkbenchUnit.getServerConnection();
                return S;
            }

            //set { this._ServerConnection = value; }
        }

        public string Module
        {
            get { return this._module; }
            set { this._module = value; }
        }

        public DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit
        {
            set
            {
                this._IWorkbenchUnit = value;
                this.setControls();
            }
        }

        public bool CanDeleteConnectionToModule
        {
            get
            {
                try
                {
                    if (!this._LinkDeleteConnectionToModuleToTableGrant)
                        return true;
                    if (this._CanDeleteConnectionToModule == null)
                    {
                        string TableName = "";
                        if (this._TableName.Length > 0)
                            TableName = this._TableName;
                        else if (this._BindingSource != null)
                            TableName = this._BindingSource.DataMember.ToString();

                        if (TableName.Length > 0 && DiversityWorkbench.Settings.ConnectionString.Length > 0)
                            _CanDeleteConnectionToModule = DiversityWorkbench.Forms.FormFunctions.Permissions(TableName, "DELETE");
                        else
                            _CanDeleteConnectionToModule = false;
                    }
                }
                catch { return false; }
                return (bool)_CanDeleteConnectionToModule;
            }
            set { /*_CanDeleteConnectionToModule = value;*/ }
        }

        public System.Collections.Generic.Dictionary<string, string> Values
        {
            get
            {
                if (this._Values == null)
                    this._Values = new System.Collections.Generic.Dictionary<string, string>();
                return _Values;
            }
            //set { _Values = value; }
        }

        public DiversityWorkbench.RemoteValues RemoteValues
        {
            get
            {
                if (this._RemoteValues == null)
                {
                    //if (this._module == "DiversityGazetteer")
                    //{
                    //    this._RemoteValues = new DiversityWorkbench.GazetteerValues();
                    //}
                }
                return _RemoteValues;
            }
            //set { _RemoteValues = value; }
        }

        public string ListInDatabase
        {
            //get { return _LocalList; }
            set { _ListInDatabase = value; }
        }

        public bool IsListInDatabase
        {
            //get { return _IsLocalList; }
            set { _IsListInDatabase = value; }
        }

        public System.Windows.Forms.HelpProvider HelpProvider { get { return this.helpProvider; } }

        public System.Data.DataRowView DataRowView
        {
            get
            {
                if (this._BindingSource != null && this._BindingSource.Current != null)
                    return (System.Data.DataRowView)this._BindingSource.Current;
                else return null;
            }
        }

        public void setForeColor(System.Drawing.Color Color)
        {
            this.textBoxValue.ForeColor = Color;
            this.comboBoxLocalValues.ForeColor = Color;
        }

        #endregion

        #region Info

        /// <summary>
        /// Decide if an optional info label is shown. E.g. an accepted name in DiversityTaxonNames
        /// </summary>
        public bool ShowInfo
        {
            get { return this._ShowInfo; }
            set
            {
                this._ShowInfo = value;
                if (this._ShowInfo)
                {
                    this.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(32);
                    this.tableLayoutPanel.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(30);
                }
                else
                {
                    this.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(24);
                    this.tableLayoutPanel.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(22);
                }
            }
        }

        public void SetInfoText(bool ShowText, string InfoText, string URI, System.Drawing.Color Color)
        {
            System.Windows.Forms.Padding padding = new Padding();
            if (ShowText)// && URI.Length > 0)
            {
                this.labelInfo.Visible = true;
                this.labelInfo.Text = "";
                this.labelInfo.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(3);
                this.labelInfo.BackColor = System.Drawing.SystemColors.ControlLightLight;
                padding = new Padding(0, DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(3), 0, 0);
                //this.comboBoxLocalValues.Margin =// funktioniert nur in tablelayoutpanel
            }
            else
            {
                this.labelInfo.Visible = false;
                padding = new Padding(0, 0, 0, 0);
                //this.comboBoxLocalValues.Margin = new Padding(0, 0, 0, 0); // funktioniert nur in tablelayoutpanel
            }
            this.buttonChart.Margin = padding;
            this.buttonConnectToDatabase.Margin = padding;
            this.buttonDeleteURI.Margin = padding;
            this.buttonFixSource.Margin = padding;
            this.buttonHtml.Margin = padding;
            this.buttonOpenModule.Margin = padding;
            this.buttonSave.Margin = padding;
            this.comboBoxLocalValues.Margin = padding;

            if (this._ShowInfo && ShowText && InfoText.Length > 0)
            {
                if (URI == this.labelURI.Text)
                {
                    this.textBoxValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    this.textBoxValue.ForeColor = Color;
                }
                else
                {
                    this.textBoxValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    this.textBoxValue.ForeColor = System.Drawing.SystemColors.WindowText;
                    this.labelInfo.BackColor = System.Drawing.SystemColors.Control;
                    this.labelInfo.ForeColor = Color;
                    this.labelInfo.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(13);
                    this.labelInfo.Text = InfoText;
                    this.labelInfo.Visible = true;
                    this.toolTip.SetToolTip(this.labelInfo, InfoText);
                }
            }
            else
            {
                this.textBoxValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.textBoxValue.ForeColor = System.Drawing.SystemColors.WindowText;
            }
        }

        #endregion

        #region public Interface functions

        public int HeightOfControl() { return this.Height; }

        public void setEntity()
        {
            string Entity = DiversityWorkbench.Entity.getEntityOfControlViaDatabinding(this.textBoxValue);
            if (Entity.Length > 0)
            {
                System.Collections.Generic.Dictionary<string, string> Dict = DiversityWorkbench.Entity.EntityInformation(Entity);
                if (Dict["DoesExist"] == "True" && Dict["DescriptionOK"] == "True")
                    this.toolTip.SetToolTip(this.textBoxValue, Dict["Description"]);
            }
        }

        #endregion

        #region Module connection

        private void buttonOpenModule_Click(object sender, EventArgs e)
        {
            SetCursor(System.Windows.Forms.Cursors.WaitCursor);
            try
            {
                if (!DiversityWorkbench.Settings.LoadConnections && !DiversityWorkbench.Settings.LoadConnectionsPassed())
                {
                    DiversityWorkbench.Settings.LoadConnectionsPassed(true);
                    int connectionCount = this._IWorkbenchUnit.ServerConnections().Count;
                }
                int? formHeight = GetCurrentFormHeight();
                if (this._BindingSource != null)
                {
                    HandleBindingSource(formHeight);
                }
                else
                {
                    HandleNoBindingSource(formHeight);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                MessageBox.Show("An error occurred while opening the module. Please check the error log for details.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetCursor(System.Windows.Forms.Cursors.Default);
            }
        }
        private void HandleBindingSource(int? formHeight)
        {
            System.Data.DataRowView currentRow = (System.Data.DataRowView)this._BindingSource.Current;
            string uri = GetUriFromRow(currentRow);
            if (string.IsNullOrEmpty(uri))
            {
                OpenRemoteQueryForm(formHeight, this.Domain, this._ListInDatabase, this._IsListInDatabase);
            }
            else
            {
                OpenRemoteQueryFormWithUri(uri, formHeight);
            }
        }
        private void HandleNoBindingSource(int? formHeight)
        {
            string uri = this.labelURI.Text.Trim();
            if (!string.IsNullOrEmpty(uri))
            {
                OpenRemoteQueryFormWithUri(uri, formHeight);
            }
            else
            {
                OpenRemoteQueryForm(formHeight, this.Domain, this._ListInDatabase, this._IsListInDatabase);
            }
        }
        private string GetUriFromRow(System.Data.DataRowView row)
        {
            if (row != null && !row[this._ValueColumn].Equals(System.DBNull.Value))
            {
                return row[this._ValueColumn].ToString();
            }
            return string.Empty;
        }
        private void OpenRemoteQueryForm(int? formHeight, string domain, string listInDatabase, bool isListInDatabase)
        {
            using (var form = CreateRemoteQueryForm(domain, listInDatabase, isListInDatabase))
            {
                if (formHeight.HasValue)
                {
                    form.Height = formHeight.Value;
                }
                form.ShowDialog();
                if (form.DialogResult == DialogResult.OK && form.DisplayText != null)
                {
                    this.SetRemoteValues(form.URI, form.DisplayText);
                }
            }
        }
        private void OpenRemoteQueryFormWithUri(string uri, int? formHeight)
        {
            using (var form = new DiversityWorkbench.Forms.FormRemoteQuery(uri, this._IWorkbenchUnit))
            {
                SetHelpNamespace(form);
                if (formHeight.HasValue)
                {
                    form.Height = formHeight.Value;
                }
                form.ShowDialog();
                if (form.DialogResult == DialogResult.OK)
                {
                    this.labelURI.Text = form.URI;
                    if (!string.IsNullOrEmpty(form.DisplayText) && !string.IsNullOrEmpty(form.URI))
                    {
                        this.textBoxValue.Text = form.DisplayText;
                    }
                    this.setControls();
                }
            }
        }
        private DiversityWorkbench.Forms.FormRemoteQuery CreateRemoteQueryForm(string domain, string listInDatabase, bool isListInDatabase)
        {
            if (isListInDatabase)
            {
                if (!string.IsNullOrEmpty(this.DependsOnUri))
                {
                    this.getDependentSource();
                }
                return !string.IsNullOrEmpty(domain)
                    ? new DiversityWorkbench.Forms.FormRemoteQuery(this._IWorkbenchUnit, listInDatabase, domain, true)
                    : new DiversityWorkbench.Forms.FormRemoteQuery(this._IWorkbenchUnit, listInDatabase);
            }
            else
            {
                return !string.IsNullOrEmpty(domain)
                    ? new DiversityWorkbench.Forms.FormRemoteQuery(this._IWorkbenchUnit, domain, true)
                    : new DiversityWorkbench.Forms.FormRemoteQuery(this._IWorkbenchUnit);
            }
        }
        private void SetHelpNamespace(DiversityWorkbench.Forms.FormRemoteQuery form)
        {
            try
            {
                form.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;
            }
            catch
            {
                // Log or handle silently
            }
        }
        private int? GetCurrentFormHeight()
        {
            var topLevelControl = this.TopLevelControl;
            return topLevelControl?.Height;
        }
        private void SetCursor(System.Windows.Forms.Cursor cursor)
        {
            this.Cursor = cursor;
        }


        //private void buttonOpenModule_Click(object sender, EventArgs e)
        //{
        //    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
        //    if (!DiversityWorkbench.Settings.LoadConnections && !DiversityWorkbench.Settings.LoadConnectionsPassed())
        //    {
        //        DiversityWorkbench.Settings.LoadConnectionsPassed(true);
        //        int i = this._IWorkbenchUnit.ServerConnections().Count;
        //    }
        //    DiversityWorkbench.Forms.FormRemoteQuery f = null;
        //    // Markus 20230308 - getting height of current form
        //    int? Height = null;
        //    {
        //        var topLevelControl = this.TopLevelControl;
        //        if (topLevelControl != null)
        //        {
        //            Height = topLevelControl.Height;
        //        }
        //    }

        //    try
        //    {
        //        if (this._BindingSource != null)
        //        {
        //            System.Data.DataRowView RU = (System.Data.DataRowView)this._BindingSource.Current;
        //            string URI = "";
        //            if (RU != null)
        //                if (!RU[this._ValueColumn].Equals(System.DBNull.Value)) URI = RU[this._ValueColumn].ToString();
        //            int ID;
        //            string sID = DiversityWorkbench.WorkbenchUnit.getIDFromURI(URI);
        //            if (int.TryParse(sID, out ID))
        //            {
        //                // Markus 28.3.23: No check for webservices
        //                DiversityWorkbench.WorkbenchUnit.ServiceType serviceType = DiversityWorkbench.WorkbenchUnit.getServiceType(URI);
        //                if (serviceType == WorkbenchUnit.ServiceType.WorkbenchModule && !this._IWorkbenchUnit.DoesExist(ID))
        //                {
        //                    if (System.Windows.Forms.MessageBox.Show("The provided link\r\n" + URI + "\r\ndoes not correspond to a dataset in the source\r\n\r\nDo you want to remove this link?", "Wrong link", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
        //                        URI = "";
        //                }
        //            }
        //            if (URI.Length == 0)
        //            {
        //                if (this._IsListInDatabase)
        //                {
        //                    if (this.DependsOnUri.Length > 0)
        //                    {
        //                        this.getDependentSource();
        //                    }
        //                    else
        //                    {
        //                        if (this.Domain.Length > 0)
        //                        {
        //                            f = new DiversityWorkbench.Forms.FormRemoteQuery(this._IWorkbenchUnit, this._ListInDatabase, this.Domain, true);
        //                        }
        //                        else
        //                        {
        //                            f = new DiversityWorkbench.Forms.FormRemoteQuery(this._IWorkbenchUnit, this._ListInDatabase);
        //                            //f.QueryRestriction = "";
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    if (this.Domain.Length > 0)
        //                    {
        //                        f = new DiversityWorkbench.Forms.FormRemoteQuery(this._IWorkbenchUnit, Domain, true);
        //                    }
        //                    else
        //                    {
        //                        f = new DiversityWorkbench.Forms.FormRemoteQuery(
        //                            this._IWorkbenchUnit);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (this.Domain.Length > 0)
        //                {
        //                    f = new DiversityWorkbench.Forms.FormRemoteQuery(URI, this._IWorkbenchUnit, this.Domain, true);
        //                }
        //                else
        //                {
        //                    f = new DiversityWorkbench.Forms.FormRemoteQuery(URI, this._IWorkbenchUnit);
        //                }
        //            }
        //            try { f.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace; }
        //            catch { }
        //            //f.TopMost = true;
        //            if (f == null)
        //                return;
        //            f.ShowInTaskbar = true;
        //            // Markus 20230308 - getting height of current form
        //            if (Height != null)
        //                f.Height = (int)Height;
        //            this.Cursor = System.Windows.Forms.Cursors.Default;
        //            f.ShowDialog();
        //            if (f.DialogResult == DialogResult.OK && f.DisplayText != null)
        //            {
        //                // Markus 31.1.2024 - Bugfix for CoL
        //                //string Uri = f.URI.ToString();
        //                this.SetRemoteValues(f.URI, f.DisplayText);
        //            }
        //        }
        //        else
        //        {
        //            // Toni 20140122: If data binding is not used and uri is specified, display referred data
        //            if (this.labelURI.Text.Trim() != "")
        //            {
        //                string URI = this.labelURI.Text;
        //                f = new DiversityWorkbench.Forms.FormRemoteQuery(URI, this._IWorkbenchUnit);
        //                try { f.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace; }
        //                catch { }
        //                this.Cursor = System.Windows.Forms.Cursors.Default;
        //                // Markus 20230308 - getting height of current form
        //                if (Height != null)
        //                    f.Height = (int)Height;
        //                f.ShowDialog();
        //            }
        //            else
        //            {
        //                if (this.Domain.Length > 0)
        //                {
        //                    f = new DiversityWorkbench.Forms.FormRemoteQuery(this._IWorkbenchUnit, this.Domain, true);
        //                    try { f.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace; }
        //                    catch { }
        //                }
        //                else
        //                {
        //                    f = new DiversityWorkbench.Forms.FormRemoteQuery(this._IWorkbenchUnit);
        //                    try { f.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace; }
        //                    catch { }
        //                }
        //                this.Cursor = System.Windows.Forms.Cursors.Default;
        //                // Markus 20230308 - getting height of current form
        //                if (Height != null)
        //                    f.Height = (int)Height;
        //                f.ShowDialog();
        //                if (f.DialogResult == DialogResult.OK)
        //                {
        //                    //System.Data.DataRowView R = (System.Data.DataRowView)this._BindingSource.Current;
        //                    this.labelURI.Text = f.URI;
        //                    if (f.DisplayText.Length > 0 && f.URI.Length > 0) // Toni 20180821: Take over new text only if values are provided
        //                        this.textBoxValue.Text = f.DisplayText;
        //                    this.setControls();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    finally
        //    {
        //        this.Cursor = System.Windows.Forms.Cursors.Default;
        //    }
        //}

        private void SetRemoteValues(string URI, string DisplayText, bool UpdateDisplayText = true)
        {
            try
            {
                System.Data.DataRowView R;
                //#267
                //if (this._BindingSource == null)
                //{
                //    this._BindingSource = new System.Windows.Forms.BindingSource();
                //    ((System.ComponentModel.ISupportInitialize)(this._BindingSource)).BeginInit();
                //    System.Data.DataTable dt = new DataTable("BindingSource");
                //    System.Data.DataColumn dcDisplayText = new DataColumn("DisplayText");
                //    dt.Columns.Add(dcDisplayText);
                //    System.Data.DataColumn dcUri = new DataColumn("URI");
                //    dt.Columns.Add(dcUri);
                //    System.Data.DataRow r = dt.NewRow();
                //    r[0] = DisplayText;
                //    r[1] = URI;
                //    dt.Rows.Add(r);
                //    //this._BindingSource.DataMember = "CollectionTask";
                //    this._BindingSource.DataSource = dt;
                //    ((System.ComponentModel.ISupportInitialize)(this._BindingSource)).EndInit();
                //}
                //if (this._ValueColumn == null || this._ValueColumn == "")
                //    this._ValueColumn = "URI";
                //if (this._DisplayColumn == null || this._DisplayColumn == "")
                //    this._DisplayColumn = "DisplayText";


                if (this._BindingSource != null && this._BindingSource.Current != null) // #267
                {
                    R = (System.Data.DataRowView)this._BindingSource.Current;
                    R.BeginEdit();
                    R[this._ValueColumn] = URI;
                    if (UpdateDisplayText)
                        R[this._DisplayColumn] = DisplayText.Trim();
                    R.EndEdit();
                }
                this.labelURI.Text = URI;
                if (this._RemoteValueBindings != null && this._RemoteValueBindings.Count > 0)
                {
                    System.Collections.Generic.Dictionary<string, string> UnitValues = new Dictionary<string, string>();

                    // Markus 10.10.22 getting the correct server connection
                    if (this.sourceServerConnection != null)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, ServerConnection> SC in this._IWorkbenchUnit.ServerConnections())
                        {
                            if (SC.Value.Key() == this.sourceServerConnection.Key())
                            {
                                this._IWorkbenchUnit.setServerConnection(this.SourceServerConnection());
                                break;
                            }
                        }
                    }

                    foreach (DiversityWorkbench.UserControls.RemoteValueBinding RVB in this._RemoteValueBindings)
                    {
                        // Markus 10.10.22: Reihenfolge umgedreht - erst Suche über ID - ansonsten wird e.g. bei mehrfachen Quellen die vorherige genommen, e.g. Verbindung zu Pflanzen und Pilzen in gleichem Datensatz
                        int ID;
                        if (int.TryParse(DiversityWorkbench.WorkbenchUnit.getIDFromURI(URI), out ID))
                        {
                            if (this.SourceServerConnection() != null)
                            {
                                this._IWorkbenchUnit.setServerConnection(this.SourceServerConnection());
                                UnitValues = this._IWorkbenchUnit.UnitValues(ID);
                            }
                        }
                        if (UnitValues.Count == 0)
                        {
                            UnitValues = this._IWorkbenchUnit.UnitValues();
                        }
                        foreach (System.Collections.Generic.KeyValuePair<string, string> P in UnitValues)
                        {
                            if (RVB.RemoteParameter == P.Key)
                            {
                                if (RVB.BindingSource != null)
                                {
                                    if (RVB.BindingSource.Current != null)
                                    {
                                        System.Data.DataRowView RV = (System.Data.DataRowView)RVB.BindingSource.Current;

                                        ///MW 4.5.2105: enable decision if an allready filled field should be changed
                                        if (RVB.ModeOfUpdate == RemoteValueBinding.UpdateMode.AskForUpdate)
                                        {
                                            if (!RV[RVB.Column].Equals(System.DBNull.Value) &&
                                                RV[RVB.Column].ToString().Length > 0 &&
                                                P.Value.Length > 0)
                                            {
                                                if (P.Value.Trim() != RV[RVB.Column].ToString().Trim()) ///MW 22.10.2019 - ask only if different
                                                {
                                                    string Message = "Should the current value\r\n\r\n" + RV[RVB.Column].ToString() +
                                                        "\r\n\r\nfor the field " + RVB.Column +
                                                        "\r\nshould be replaced by\r\n\r\n" + P.Value;
                                                    if (System.Windows.Forms.MessageBox.Show(Message, "Replace entry", MessageBoxButtons.YesNo) == DialogResult.No)
                                                        continue;
                                                }
                                            }
                                            else if ((RV[RVB.Column].Equals(System.DBNull.Value) ||
                                                RV[RVB.Column].ToString().Length == 0) &&
                                                P.Value.Length > 0)
                                            {
                                            }
                                            else
                                                continue;
                                        }
                                        else if (RVB.ModeOfUpdate == RemoteValueBinding.UpdateMode.OnlyEmpty)
                                        {
                                            if (!RV[RVB.Column].Equals(System.DBNull.Value) &&
                                                RV[RVB.Column].ToString().Length > 0)
                                                continue;
                                        }

                                        RV.BeginEdit();
                                        try
                                        {
                                            if (this._SupressEmptyRemoteValues && P.Value.Length == 0)
                                            { }
                                            else
                                            {
                                                // Markus 28.3.25: Prüfung ob Spalte in Tabelle vorhanden ist
                                                if (RV.Row.Table.Columns.Contains(RVB.Column))
                                                {
                                                    if (RV.Row.Table.Columns[RVB.Column].DataType.Name == "Double") //MW 22.10.2019 - Double has been inserted missing the '.'
                                                    {
                                                        Double DD;
                                                        System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Number;
                                                        System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CurrentCulture;
                                                        if (Double.TryParse(P.Value, style, culture, out DD))
                                                        {
                                                            RV[RVB.Column] = Double.Parse(DD.ToString(culture));
                                                        }
                                                    }
                                                    else
                                                        RV[RVB.Column] = P.Value;
                                                }
                                            }
                                        }
                                        catch (System.Exception ex) { }
                                        RV.EndEdit();
                                    }
                                }
                                else
                                { }
                            }
                        }
                    }
                }
                if (DisplayText.Length > 0 && URI.Length > 0 && UpdateDisplayText) // Toni 20180821: Take over new text only if values are provided
                    this.textBoxValue.Text = DisplayText;
                this.setControls();
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void buttonDeleteURI_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._BindingSource != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this._BindingSource.Current;
                    if (R.Row.Table.Columns[this._ValueColumn].AllowDBNull)
                        R[this._ValueColumn] = System.DBNull.Value;
                    else
                        R[this._ValueColumn] = "";
                    this.labelURI.Text = "";
                    this.setControls();
                }
                else
                {
                    this.labelURI.Text = "";
                    this.setControls();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonConnectToDatabase_Click(object sender, EventArgs e)
        {
            if (!DiversityWorkbench.Settings.LoadConnections && !DiversityWorkbench.Settings.LoadConnectionsPassed())
            {
                DiversityWorkbench.Settings.LoadConnectionsPassed(true);
                DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this._IWorkbenchUnit.ServiceName()].getServerConnection();
            }
            try
            {
                if (this._IWorkbenchUnit != null &&
                    this.textBoxValue.Text.Length > 0 &&
                    this.labelURI.Text.Length > 0 &&
                    !DiversityWorkbench.WorkbenchUnit.IsServiceAvailable(this._IWorkbenchUnit.ServiceName(), this.labelURI.Text))
                {
                    DiversityWorkbench.Forms.FormConnectToDatabase f = new Forms.FormConnectToDatabase(this._IWorkbenchUnit);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        DiversityWorkbench.ServerConnection S = f.ServerConnection;
                        S.ModuleName = this._IWorkbenchUnit.getServerConnection().ModuleName;
                        this._IWorkbenchUnit.setServerConnection(S);
                        string URI = this.labelURI.Text;
                        if (URI.Length == 0)
                        {
                            System.Data.DataRowView RV = (System.Data.DataRowView)this._BindingSource.Current;
                            URI = RV[this._ValueColumn].ToString();
                        }
                        this.setControls();
                    }
                }
                else if (this._IWorkbenchUnit != null &&
                    DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList().ContainsKey(this._IWorkbenchUnit.ServiceName()) &&
                    DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this._IWorkbenchUnit.ServiceName()].AccessibleDatabasesAndServicesOfModule().Count > 0)
                {
                    bool ValidConnection = false;
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this._IWorkbenchUnit.ServiceName()].ServerConnectionList())
                    {
                        if (KV.Value.ConnectionIsValid && KV.Value.ConnectionString.Length > 0)
                        {
                            this._IWorkbenchUnit.setServerConnection(KV.Value);
                            ValidConnection = true;
                            break;
                        }
                    }
                    if (ValidConnection)
                    {
                        this.setControls();
                        this.buttonOpenModule_Click(null, null);
                    }
                }
                else if (this._IWorkbenchUnit != null &&
                    !DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList().ContainsKey(this._IWorkbenchUnit.ServiceName()))
                {

                }
                else if (this._IWorkbenchUnit != null)
                {
                    DiversityWorkbench.Forms.FormConnectToDatabase f = new Forms.FormConnectToDatabase(this._IWorkbenchUnit);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        DiversityWorkbench.ServerConnection S = f.ServerConnection;
                        S.ModuleName = this._IWorkbenchUnit.getServerConnection().ModuleName;
                        this._IWorkbenchUnit.setServerConnection(S);
                        string URI = this.labelURI.Text;
                        if (URI.Length == 0)
                        {
                            System.Data.DataRowView RV = (System.Data.DataRowView)this._BindingSource.Current;
                            URI = RV[this._ValueColumn].ToString();
                        }
                        this.setControls();
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// For DiversityScientificTerms - Terms may depend on other terms, so the available data have to be restricted
        /// </summary>
        private void getDependentSource()
        {
            DiversityWorkbench.ScientificTerm ST = new ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
            bool OK = ST.DependentQueryAvailable(this.DependsOnUri);
            if (OK)
            {
                DiversityWorkbench.Forms.FormRemoteQueryDependent fD = new Forms.FormRemoteQueryDependent(this.DependsOnUri);
                fD.ShowDialog();
                if (fD.DialogResult == DialogResult.OK)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this._BindingSource.Current;
                    R.BeginEdit();
                    R[this._ValueColumn] = fD.URI;
                    string Display = fD.DisplayText.Trim();
                    try
                    {
                        System.Collections.Generic.Dictionary<string, string> VV = ST.UnitValues(fD.URI);
                        if (VV.ContainsKey("RankingTerm") &&
                            VV["RankingTerm"].Length > 0 &&
                            Display != VV["RankingTerm"])
                        {
                            Display = VV["RankingTerm"] + ": " + Display;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                    R[this._DisplayColumn] = Display;
                    R.EndEdit();
                    this.labelURI.Text = fD.URI;
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No dependent terms available");
                return;
            }

        }

        #endregion

        #region Control

        #region control events

        private void labelURI_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this._BindingSource != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this._BindingSource.Current;
                    string URI = R[this._ValueColumn].ToString();
                    string Display = R[this._DisplayColumn].ToString();
                    DiversityWorkbench.Forms.FormInfo f = new DiversityWorkbench.Forms.FormInfo("URI of " + Display, URI);
                    f.ShowDialog();
                    //System.Windows.Forms.MessageBox.Show(URI, "URI of " + Display, MessageBoxButtons.OK);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(this.labelURI.Text, "URI of " + this.textBoxValue.Text, MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void textBoxValue_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this._BindingSource != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this._BindingSource.Current;
                    if (R[this._ValueColumn].Equals(System.DBNull.Value))
                    {
                        object[] ar = new Object[9] { "", "", "", "", "", "", "", "", "" };
                        string Title = "Edit ";
                        if (this.textBoxValue.DataBindings != null)
                        {
                            try
                            {
                                this.textBoxValue.DataBindings.CopyTo(ar, 0);
                                if (ar[0].ToString() != "")
                                {
                                    System.Windows.Forms.Binding B = (System.Windows.Forms.Binding)ar[0];
                                    Title = Title + B.BindingMemberInfo.BindingField;
                                }
                            }
                            catch { }
                        }
                        DiversityWorkbench.Forms.FormEditText fet = new DiversityWorkbench.Forms.FormEditText(Title, this.textBoxValue.Text);
                        fet.ShowDialog();
                        if (fet.DialogResult == System.Windows.Forms.DialogResult.OK)
                            this.textBoxValue.Text = fet.EditedText;
                    }
                    else
                    {
                        string Value = R[this._DisplayColumn].ToString();
                        string URI = R[this._ValueColumn].ToString();
                        System.Windows.Forms.MessageBox.Show(Value, "Value of URI " + URI, MessageBoxButtons.OK);
                    }
                }
                else
                {
                    if (this.textBoxValue.Text.Length > 0 && this.labelURI.Text.Length > 0)
                    {
                        System.Windows.Forms.MessageBox.Show(this.textBoxValue.Text, "Value of URI " + this.labelURI.Text, MessageBoxButtons.OK);
                    }
                    else
                    {
                        if (this.labelURI.Text.Length > 0)
                            System.Windows.Forms.MessageBox.Show("URI: " + this.labelURI.Text, "URI", MessageBoxButtons.OK);
                        else
                        {
                            if (this.textBoxValue.Text.Length > 0)
                                System.Windows.Forms.MessageBox.Show(this.textBoxValue.Text, "Value", MessageBoxButtons.OK);
                            else
                                System.Windows.Forms.MessageBox.Show("Value und URI not set", "Not set", MessageBoxButtons.OK);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void textBoxValue_EnabledChanged(object sender, EventArgs e)
        {
            bool ValueSet = false;
            if (this._BindingSource != null && this._IWorkbenchUnit != null)
            {
                try
                {
                    if (this.textBoxValue.Visible == false)
                        this.textBoxValue.Visible = true;
                    if (this._BindingSource != null
                        && this._BindingSource.Position > -1)
                    {
                        //if (this._BindingSource.CurrencyManager.Count == 1)
                        bool OK = true;
                        System.Data.DataRowView R = null;
                        try
                        { R = (System.Data.DataRowView)this._BindingSource.Current; }
                        catch (Exception ex)
                        { OK = false; }
                        if (OK)
                        {
                            if (R != null && R.Row.RowState != DataRowState.Detached && R.Row.RowState != DataRowState.Added)
                            {
                                if (!R[this._ValueColumn].Equals(System.DBNull.Value) && R[this._ValueColumn].ToString().Length > 0)
                                    ValueSet = true;
                            }
                        }
                    }
                    if (this.textBoxValue.Enabled)
                    {
                        if (ValueSet)
                            this.textBoxValue.BackColor = System.Drawing.SystemColors.Info;
                        else
                            this.textBoxValue.BackColor = System.Drawing.SystemColors.Window;
                    }
                    else
                        this.textBoxValue.BackColor = System.Drawing.SystemColors.Control;
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else if (this._IWorkbenchUnit != null && this._BindingSource == null)
            {
                if (this.labelURI.Text.Length > 0) ValueSet = true;
                if (this.textBoxValue.Enabled)
                {
                    if (ValueSet)
                        this.textBoxValue.BackColor = System.Drawing.SystemColors.Info;
                    else
                        this.textBoxValue.BackColor = System.Drawing.SystemColors.Window;
                }
                else
                    this.textBoxValue.BackColor = System.Drawing.SystemColors.Control;
            }
            else
                this.textBoxValue.BackColor = System.Drawing.SystemColors.Control;
        }

        private void labelURI_TextChanged(object sender, EventArgs e)
        {
            //string x = this.labelURI.Text;
            this.setControls();
        }

        private void currentBinding_Changed(object sender, EventArgs e)
        {
            this.setControls();
        }

        #endregion

        #region Setting the controls

        private void setControls()
        {
            ValueState ValueState = ValueState.NoValue;
            ConnectionState ConnectionState = ConnectionState.NotConnected;
            BindingState BindingState = BindingState.NoBinding_NoUnit;
            UriAvailableState UriAvailableState = UriAvailableState.NoAccess;

            // getting the BindingState, ValueState and ConnectionState
            this.setControls_GetStates(ref ValueState, ref ConnectionState, ref BindingState, ref UriAvailableState);

            try
            {
                // Setting the visibility etc. of the controls
                this.setControls_ButtonConnectToDatabase(ValueState, ConnectionState, BindingState, UriAvailableState);
                this.setControls_ButtonDeleteURI(ValueState, BindingState);
                this.setControls_ButtonHtml(ValueState, ConnectionState, BindingState, UriAvailableState);
                this.setControls_ButtonOpenModule(ValueState, ConnectionState, BindingState, UriAvailableState);
                this.setControls_ComboBoxLocalValues(ValueState, ConnectionState, BindingState, UriAvailableState);
                this.setControls_LabelURI(ValueState, ConnectionState, BindingState, UriAvailableState);
                this.setControls_TextBoxValue(ValueState, ConnectionState, BindingState, UriAvailableState);

                if ((this.comboBoxLocalValues.Visible || this.labelURI.Text.Length == 0) && this._FixingOfSourceEnabled)
                {
                    this.buttonFixSource.Visible = true;
                    this.buttonChart.Visible = false;
                    if (this.sourceServerConnection != null)
                    {
                        if (this._ChartEnabled && this._Chart != null && DiversityWorkbench.Settings.UseQueryCharts)
                        {
                            this.buttonChart.Visible = true;
                        }
                    }
                    this.setFixSourceControls();
                }
                else
                {
                    this.buttonFixSource.Visible = false;
                    this.buttonChart.Visible = false;
                }
                this.textBoxValue.BringToFront();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setControls_GetStates(ref ValueState ValueState, ref ConnectionState ConnectionState, ref BindingState BindingState, ref UriAvailableState UriAvailableState)
        {
            System.Data.DataRowView RV;

            // getting the BindingState, ValueState and ConnectionState
            try
            {
                // with binding
                if (this._BindingSource != null && this._BindingSource.Current != null)
                {
                    RV = (System.Data.DataRowView)this._BindingSource.Current;
                    if (this._DisplayColumn != null
                        && RV.Row.Table.Columns.Contains(this._DisplayColumn)
                        && !RV[this._DisplayColumn].Equals(System.DBNull.Value)
                        && RV[this._DisplayColumn].ToString().Length > 0)
                    {
                        if (!RV[this._ValueColumn].Equals(System.DBNull.Value)
                            && RV[this._ValueColumn].ToString().Length > 0)
                        {
                            ValueState = ValueState.RemoteValue;
                            if (this._IWorkbenchUnit != null)
                            {
                                if (DiversityWorkbench.WorkbenchUnit.IsServiceAvailable(this._IWorkbenchUnit.ServiceName(), RV[this._ValueColumn].ToString()))
                                {
                                    ConnectionState = ConnectionState.Connected;
                                    UriAvailableState = UriAvailableState.Available;
                                }
                                else if (this._IsListInDatabase)
                                {
                                    string DB = RV[this._ValueColumn].ToString();
                                    try
                                    {
                                        DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(DB);
                                        if (this._DataAdapter != null && this._DataAdapter.SelectCommand.Connection.ConnectionString.Length > 0)
                                        {
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                    }
                                }
                            }
                            this.toolTip.SetToolTip(this.labelURI, RV[this._ValueColumn].ToString());
                        }
                        else
                        {
                            ValueState = ValueState.LocalValue;
                            if (this._IWorkbenchUnit != null)
                            {
                                if (DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable(this._IWorkbenchUnit.ServiceName()))
                                    ConnectionState = ConnectionState.Connected;
                            }
                            this.toolTip.SetToolTip(this.labelURI, "");
                        }
                    }
                    if (this._IWorkbenchUnit == null)
                        BindingState = BindingState.Binding_NoUnit;
                    else
                    {
                        BindingState = BindingState.Binding_Unit;
                        if (DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable(this._IWorkbenchUnit.ServiceName()))
                            ConnectionState = ConnectionState.Connected;
                    }
                }
                else // no binding
                {
                    if (this._IWorkbenchUnit == null)
                    {
                        BindingState = BindingState.NoBinding_NoUnit;
                    }
                    else
                    {
                        BindingState = BindingState.NoBinding_Unit;
                        if (DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable(this._IWorkbenchUnit.ServiceName()))
                            ConnectionState = ConnectionState.Connected;
                    }
                    if (this.labelURI.Text.Length > 0
                        && this.textBoxValue.Text.Length > 0
                        && this._IWorkbenchUnit != null)
                    {
                        ValueState = ValueState.RemoteValue;
                        if (DiversityWorkbench.WorkbenchUnit.IsServiceAvailable(this._IWorkbenchUnit.ServiceName(), this.labelURI.Text))
                            UriAvailableState = UriAvailableState.Available;
                    }
                    else if (this.labelURI.Text.Length > 0
                        && this._IWorkbenchUnit != null)
                    {
                        ValueState = ValueState.RemoteValue;
                        if (DiversityWorkbench.WorkbenchUnit.IsServiceAvailable(this._IWorkbenchUnit.ServiceName(), this.labelURI.Text))
                            UriAvailableState = UriAvailableState.Available;
                        else if (this.textBoxValue.Text.Length > 0)
                            ValueState = ValueState.LocalValue;
                        else
                            ValueState = ValueState.NoValue;
                    }
                    else
                    {
                        if (this.textBoxValue.Text.Length > 0)
                            ValueState = ValueState.LocalValue;
                        else
                            ValueState = ValueState.NoValue;
                    }
                }
                if (ConnectionState == ConnectionState.NotConnected && ValueState != ValueState.RemoteValue && this._IWorkbenchUnit != null)
                {
                    if (DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable(this._IWorkbenchUnit.ServiceName()))
                        ConnectionState = ConnectionState.Connected;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setControls_ButtonConnectToDatabase(ValueState ValueState, ConnectionState ConnectionState, BindingState BindingState, UriAvailableState UriAvailableState)
        {
            try
            {
                switch (ConnectionState)
                {
                    case ConnectionState.Connected:
                        if (UriAvailableState == UriAvailableState.NoAccess && ValueState == ValueState.RemoteValue)
                        {
                            this.buttonConnectToDatabase.Visible = BindingState != BindingState.Binding_NoUnit;
                        }
                        else
                        {
                            this.buttonConnectToDatabase.Visible = false;
                        }
                        break;
                    case ConnectionState.NotConnected:
                        this.buttonConnectToDatabase.Visible = BindingState != BindingState.Binding_NoUnit;
                        break;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setControls_ButtonOpenModule(ValueState ValueState, ConnectionState ConnectionState, BindingState BindingState, UriAvailableState UriAvailableState)
        {
            try
            {
                switch (ConnectionState)
                {
                    case ConnectionState.Connected:
                        if (UriAvailableState == UriAvailableState.NoAccess && ValueState == ValueState.RemoteValue)
                        {
                            this.buttonOpenModule.Visible = false;
                        }
                        else
                        {
                            this.buttonOpenModule.Visible = BindingState != BindingState.NoBinding_NoUnit && BindingState != BindingState.Binding_NoUnit;
                        }
                        break;
                    case ConnectionState.NotConnected:
                        this.buttonOpenModule.Visible = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setControls_ButtonHtml(ValueState ValueState, ConnectionState ConnectionState, BindingState BindingState, UriAvailableState UriAvailableState)
        {
            try
            {
                switch (ConnectionState)
                {
                    case ConnectionState.Connected:
                        if (UriAvailableState == UriAvailableState.NoAccess && ValueState == ValueState.RemoteValue)
                        {
                            this.buttonHtml.Visible = false;
                        }
                        else
                        {
                            if (BindingState == BindingState.NoBinding_Unit || BindingState == BindingState.NoBinding_NoUnit)
                                this.buttonHtml.Visible = false;
                            else
                                this.buttonHtml.Visible = HtmlUnitValues;
                        }
                        break;
                    case ConnectionState.NotConnected:
                        this.buttonHtml.Visible = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setControls_ButtonDeleteURI(ValueState ValueState, BindingState BindingState)
        {
            try
            {
                switch (ValueState)
                {
                    case ValueState.NoValue:
                    case ValueState.LocalValue:
                        this.buttonDeleteURI.Visible = false;
                        break;
                    case ValueState.RemoteValue:
                        {
                            switch (BindingState)
                            {
                                case BindingState.Binding_Unit:
                                    this.buttonDeleteURI.Visible = this.CanDeleteConnectionToModule;
                                    break;
                                case BindingState.Binding_NoUnit:
                                case BindingState.NoBinding_Unit:
                                    this.buttonDeleteURI.Visible = true;
                                    break;
                                case BindingState.NoBinding_NoUnit:
                                    this.buttonDeleteURI.Visible = false;
                                    break;
                            }
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setControls_ComboBoxLocalValues(ValueState ValueState, ConnectionState ConnectionState, BindingState BindingState, UriAvailableState UriAvailableState)
        {
            try
            {
                string Search = "";
                switch (BindingState)
                {
                    case BindingState.Binding_Unit:
                        switch (ValueState)
                        {
                            case ValueState.NoValue:
                            case ValueState.LocalValue:
                                switch (this.FixedSourceMode)
                                {
                                    case FixSourceMode.Database:
                                    case FixSourceMode.CacheDB:
                                        this.comboBoxLocalValues.Visible = false;
                                        break;
                                    case FixSourceMode.Webservice:
                                        this.comboBoxLocalValues.Visible = true;
                                        this.comboBoxLocalValues.BackColor = System.Drawing.Color.SkyBlue;
                                        var info = TaxonomicServiceInfoDictionary();
                                        if(info.TryGetValue(this.SourceWebservice, out var serviceInfo))
                                            Search = "Search for values in " + serviceInfo.Name;
                                        break;
                                    case FixSourceMode.None:
                                    default:
                                        if (this.textBoxValue.AutoCompleteCustomSource != null && this.textBoxValue.AutoCompleteCustomSource.Count > 0)
                                            this.comboBoxLocalValues.Visible = false;
                                        else
                                            this.comboBoxLocalValues.Visible = this.sourceServerConnection == null;
                                        this.comboBoxLocalValues.BackColor = System.Drawing.Color.White;
                                        Search = "Search for local values";
                                        break;
                                }
                                this.toolTip.SetToolTip(this.comboBoxLocalValues, Search);


                                //if (this._SQL == null)
                                //    this.comboBoxLocalValues.Visible = false;
                                //else
                                //{
                                //    if (this.textBoxValue.AutoCompleteCustomSource != null && this.textBoxValue.AutoCompleteCustomSource.Count > 0)
                                //        this.comboBoxLocalValues.Visible = false;
                                //    else
                                //        this.comboBoxLocalValues.Visible = this.sourceServerConnection == null;
                                //}
                                break;
                            case ValueState.RemoteValue:
                                {
                                    //this.comboBoxLocalValues.Visible = false;
                                    //string Search = "";
                                    switch (this.FixedSourceMode)
                                    {
                                        case FixSourceMode.Database:
                                        case FixSourceMode.CacheDB:
                                            this.comboBoxLocalValues.Visible = false;
                                            break;
                                        case FixSourceMode.Webservice:
                                            this.comboBoxLocalValues.Visible = true;
                                            this.comboBoxLocalValues.BackColor = System.Drawing.Color.SkyBlue;
                                            var info = TaxonomicServiceInfoDictionary();
                                            if (info.TryGetValue(this.SourceWebservice, out var serviceInfo))
                                                Search = "Search for values in " + serviceInfo.Name;
                                            break;
                                        case FixSourceMode.None:
                                        default:
                                            if (this.textBoxValue.AutoCompleteCustomSource != null && this.textBoxValue.AutoCompleteCustomSource.Count > 0)
                                                this.comboBoxLocalValues.Visible = false;
                                            else
                                                this.comboBoxLocalValues.Visible = this.sourceServerConnection == null && this.labelURI.Text.Length == 0;
                                            this.comboBoxLocalValues.BackColor = System.Drawing.Color.White;
                                            Search = "Search for local values";
                                            break;
                                    }
                                    this.toolTip.SetToolTip(this.comboBoxLocalValues, Search);
                                }
                                break;
                        }
                        break;
                    case BindingState.NoBinding_Unit:
                    case BindingState.Binding_NoUnit:
                        if (this.textBoxValue.AutoCompleteSource == AutoCompleteSource.CustomSource
                            && this.textBoxValue.AutoCompleteCustomSource != null
                            && this.textBoxValue.AutoCompleteCustomSource.Count > 0)
                            this.comboBoxLocalValues.Visible = false;
                        else
                            this.comboBoxLocalValues.Visible = true;
                        break;
                    case BindingState.NoBinding_NoUnit:
                        this.comboBoxLocalValues.Visible = false;
                        break;
                }

                //try
                //{
                //    string Search = "";
                //    switch (this.FixedSourceMode)
                //    {
                //        case FixSourceMode.Database:
                //        case FixSourceMode.CacheDB:
                //            this.comboBoxLocalValues.Visible = false;
                //            break;
                //        case FixSourceMode.Webservice:
                //            this.comboBoxLocalValues.Visible = true;
                //            this.comboBoxLocalValues.BackColor = System.Drawing.Color.SkyBlue;
                //            Search = "Search for values in " + this.sourceWebservice.ServiceName();
                //            break;
                //        case FixSourceMode.None:
                //        default:
                //            if (this.textBoxValue.AutoCompleteCustomSource != null && this.textBoxValue.AutoCompleteCustomSource.Count > 0)
                //                this.comboBoxLocalValues.Visible = false;
                //            else
                //                this.comboBoxLocalValues.Visible = this.sourceServerConnection == null;
                //            this.comboBoxLocalValues.BackColor = System.Drawing.Color.White;
                //            Search = "Search for local values";
                //            break;
                //    }
                //    this.toolTip.SetToolTip(this.comboBoxLocalValues, Search);
                //}
                //catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }



            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setControls_TextBoxValue(ValueState ValueState, ConnectionState ConnectionState, BindingState BindingState, UriAvailableState UriAvailableState)
        {
            try
            {
                textBoxValue.Visible = BindingState != BindingState.NoBinding_NoUnit;

                switch (ValueState)
                {
                    case ValueState.NoValue:
                    case ValueState.LocalValue:
                        this.textBoxValue.ReadOnly = false;
                        if (BindingState == BindingState.Binding_NoUnit)
                            this.textBoxValue.BackColor = System.Drawing.SystemColors.Window;
                        else if (this.textBoxValue.Enabled)
                        {
                            switch (this.FixedSourceMode)
                            {
                                case FixSourceMode.Database:
                                    this.textBoxValue.BackColor = System.Drawing.Color.PeachPuff;
                                    break;
                                case FixSourceMode.CacheDB:
                                    this.textBoxValue.BackColor = System.Drawing.Color.LavenderBlush;
                                    break;
                                case FixSourceMode.Webservice:
                                    this.textBoxValue.BackColor = System.Drawing.Color.AliceBlue;
                                    break;
                                case FixSourceMode.None:
                                default:
                                    this.textBoxValue.BackColor = System.Drawing.SystemColors.Window;
                                    break;
                            }
                        }
                        else this.textBoxValue.BackColor = System.Drawing.SystemColors.Control;
                        break;
                    case ValueState.RemoteValue:
                        this.textBoxValue.ReadOnly = true;
                        if (this.textBoxValue.Enabled) this.textBoxValue.BackColor = System.Drawing.SystemColors.Info;
                        else this.textBoxValue.BackColor = System.Drawing.SystemColors.Control;
                        this.textBoxValue.BringToFront();
                        break;
                }

                //switch (BindingState)
                //{
                //    case BindingState.Binding_Unit:
                //        textBoxValue.Visible = true;
                //        switch (ValueState)
                //        {
                //            case ValueState.NoValue:
                //            case ValueState.LocalValue:
                //                this.textBoxValue.ReadOnly = false;
                //                if (this.textBoxValue.Enabled) this.textBoxValue.BackColor = System.Drawing.SystemColors.Window;
                //                else this.textBoxValue.BackColor = System.Drawing.SystemColors.Control;
                //                break;
                //            case ValueState.RemoteValue:
                //                this.textBoxValue.ReadOnly = true;
                //                if (this.textBoxValue.Enabled) this.textBoxValue.BackColor = System.Drawing.SystemColors.Info;
                //                else this.textBoxValue.BackColor = System.Drawing.SystemColors.Control;
                //                break;
                //        }
                //        break;
                //    case BindingState.Binding_NoUnit:
                //        switch (ValueState)
                //        {
                //            case ValueState.NoValue:
                //            case ValueState.LocalValue:
                //                this.textBoxValue.ReadOnly = false;
                //                this.textBoxValue.BackColor = System.Drawing.SystemColors.Window;
                //                break;
                //            case ValueState.RemoteValue:
                //                this.textBoxValue.ReadOnly = true;
                //                this.textBoxValue.BackColor = System.Drawing.SystemColors.Info;
                //                break;
                //        }
                //        this.textBoxValue.Visible = true;
                //        break;
                //    case BindingState.NoBinding_Unit:
                //        this.textBoxValue.Visible = true;
                //        switch (ValueState)
                //        {
                //            case ValueState.NoValue:
                //            case ValueState.LocalValue:
                //                this.textBoxValue.ReadOnly = false;
                //                this.textBoxValue.BackColor = System.Drawing.SystemColors.Window;
                //                break;
                //            case ValueState.RemoteValue:
                //                this.textBoxValue.ReadOnly = true;
                //                this.textBoxValue.BackColor = System.Drawing.SystemColors.Info;
                //                break;
                //        }
                //        break;
                //    case BindingState.NoBinding_NoUnit:
                //        this.textBoxValue.Visible = false;
                //        break;
                //}
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setControls_LabelURI(ValueState ValueState, ConnectionState ConnectionState, BindingState BindingState, UriAvailableState UriAvailableState)
        {
            try
            {
                switch (ValueState)
                {
                    case ValueState.NoValue:
                    case ValueState.LocalValue:
                        this.labelURI.Visible = false;
                        switch (BindingState)
                        {
                            case BindingState.Binding_NoUnit:
                            case BindingState.NoBinding_Unit:
                                this.labelURI.BringToFront();
                                break;
                            case BindingState.Binding_Unit:
                            default:
                                this.toolTip.SetToolTip(this.labelURI, "");
                                break;
                        }
                        break;
                    case ValueState.RemoteValue:
                        this.labelURI.Visible = true;
                        this.labelURI.BringToFront();
                        break;
                }
                //switch (BindingState)
                //{
                //    case BindingState.Binding_NoUnit:
                //    case BindingState.NoBinding_Unit:
                //        switch (ValueState)
                //        {
                //            case ValueState.NoValue:
                //            case ValueState.LocalValue:
                //                this.labelURI.Visible = false;
                //                break;
                //            case ValueState.RemoteValue:
                //                this.labelURI.Visible = true;
                //                this.labelURI.BringToFront();
                //                break;
                //        }
                //        break;
                //    case BindingState.Binding_Unit:
                //    default:
                //        switch (ValueState)
                //        {
                //            case ValueState.NoValue:
                //            case ValueState.LocalValue:
                //                this.labelURI.Visible = false;
                //                this.toolTip.SetToolTip(this.labelURI, "");
                //                break;
                //            case ValueState.RemoteValue:
                //                this.labelURI.Visible = true;
                //                this.labelURI.BringToFront();
                //                break;
                //        }
                //        break;
                //    //default:
                //    //    switch (ValueState)
                //    //    {
                //    //        case ValueState.NoValue:
                //    //        case ValueState.LocalValue:
                //    //            this.labelURI.Visible = false;
                //    //            this.toolTip.SetToolTip(this.labelURI, "");
                //    //            break;
                //    //        case ValueState.RemoteValue:
                //    //            this.labelURI.Visible = true;
                //    //            break;
                //    //    }
                //    //    break;
                //}
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setControls_buttonChart(ValueState ValueState, ConnectionState ConnectionState, BindingState BindingState, UriAvailableState UriAvailableState)
        {
            try
            {
                this.buttonChart.Visible = this._FixingOfSourceEnabled
                    && this.sourceServerConnection != null
                    && this._ChartEnabled
                    && this._Chart != null
                    && ConnectionState == ConnectionState.Connected
                    && ValueState != ValueState.RemoteValue
                    && BindingState != BindingState.NoBinding_NoUnit;
                this.toolTip.SetToolTip(this.comboBoxLocalValues, "Search for local values");

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setControls_buttonFixSource(ValueState ValueState = ValueState.unknown)//, ConnectionState ConnectionState, BindingState BindingState, UriAvailableState UriAvailableState)
        {
            try
            {
                if (ValueState == ValueState.unknown)
                {
                    ConnectionState ConnectionState = ConnectionState.NotConnected;
                    BindingState BindingState = BindingState.NoBinding_NoUnit;
                    UriAvailableState UriAvailableState = UriAvailableState.NoAccess;

                    this.setControls_GetStates(ref ValueState, ref ConnectionState, ref BindingState, ref UriAvailableState);
                }
                if (this._FixingOfSourceEnabled && ValueState != ValueState.RemoteValue)///*(this.comboBoxLocalValues.Visible || this.buttonSave.Visible || this.labelURI.Text.Length == 0) &&*/ )
                {
                    this.buttonFixSource.Visible = true;
                    if (this.sourceServerConnection == null && SourceWebservice == DwbService.None)
                    {
                        this._fixSourceMode = FixSourceMode.None;
                        this.buttonFixSource.Image = this.imageList.Images[1];
                        this.toolTip.SetToolTip(this.buttonFixSource, "Set the source");
                    }
                    //this.setFixSourceControls();
                    try
                    {
                        string Source = "";
                        switch (this.FixedSourceMode)
                        {
                            case FixSourceMode.Database:
                                Source = this.sourceServerConnection.DisplayText;
                                if (this.sourceServerConnection.Project != null && this.sourceServerConnection.Project.Length > 0)
                                {
                                    Source += " (" + this.sourceServerConnection.Project;
                                    if (this.sourceServerConnection.Section != null && this.sourceServerConnection.Section.Length > 0)
                                        Source += " - " + this.sourceServerConnection.Section;
                                    Source += ")";
                                }
                                Source = "Source: " + Source;
                                this.buttonFixSource.Image = this.imageList.Images[2];
                                break;
                            case FixSourceMode.CacheDB:
                                Source = "Source: " + this.sourceServerConnection.DisplayText;
                                this.buttonFixSource.Image = this.imageList.Images[3];
                                break;
                            case FixSourceMode.Webservice:
                                Source = "Source: " + this.SourceWebservice.ToString();
                                this.buttonFixSource.Image = this.imageList.Images[0];
                                break;
                            case FixSourceMode.None:
                            default:
                                Source = "Set the source";
                                this.buttonFixSource.Image = this.imageList.Images[1];
                                break;
                        }
                        this.toolTip.SetToolTip(this.buttonFixSource, Source);
                    }
                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

                }
                else
                {
                    this.buttonFixSource.Visible = false;
                    this.buttonChart.Visible = false;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        #endregion

        #region Fixing the source

        private void FixedSourceModeReset() { this._fixSourceMode = FixSourceMode.Undefined; }
        private FixSourceMode FixedSourceMode
        {
            get
            {
                if (this._fixSourceMode == FixSourceMode.Undefined)
                {
                    if (this.sourceServerConnection == null && this.SourceWebservice == DwbService.None)
                    {
                        this._fixSourceMode = FixSourceMode.None;
                    }
                    else
                    {
                        if (this.sourceServerConnection == null && SourceWebservice != DwbService.None)
                            this._fixSourceMode = FixSourceMode.Webservice;
                        else if (this.sourceServerConnection != null && this.SourceWebservice == DwbService.None)
                        {
                            if (this.sourceServerConnection.CacheDB.Length > 0 && this.sourceServerConnection.CacheDBSourceView.Length > 0 && this.sourceServerConnection.CollectionCacheDbAccessible) // && this.CacheDbAccessible())// && Accessible
                                this._fixSourceMode = FixSourceMode.CacheDB;
                            else
                                this._fixSourceMode = FixSourceMode.Database;
                        }
                    }
                }
                return this._fixSourceMode;
            }
        }

        private void setFixSourceControls()
        {
            try
            {
                ValueState ValueState = ValueState.NoValue;
                ConnectionState ConnectionState = ConnectionState.NotConnected;
                BindingState BindingState = BindingState.NoBinding_NoUnit;
                UriAvailableState UriAvailableState = UriAvailableState.NoAccess;
                this.setControls_GetStates(ref ValueState, ref ConnectionState, ref BindingState, ref UriAvailableState);

                this.setControls_TextBoxValue(ValueState, ConnectionState, BindingState, UriAvailableState);
                this.setControls_buttonFixSource(ValueState);
                //this.setFixSourceControls_Button();
                this.setControls_ComboBoxLocalValues(ValueState, ConnectionState, BindingState, UriAvailableState);
                //this.setFixSourceControls_comboBoxLocalValues();
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// Setting the image and the tool tip for the button
        /// </summary>
        private void setFixSourceControls_Button()
        {
            try
            {
                string Source = "";
                switch (this.FixedSourceMode)
                {
                    case FixSourceMode.Database:
                        Source = this.sourceServerConnection.DisplayText;
                        if (this.sourceServerConnection.Project != null && this.sourceServerConnection.Project.Length > 0)
                        {
                            Source += " (" + this.sourceServerConnection.Project;
                            if (this.sourceServerConnection.Section != null && this.sourceServerConnection.Section.Length > 0)
                                Source += " - " + this.sourceServerConnection.Section;
                            Source += ")";
                        }
                        Source = "Source: " + Source;
                        this.buttonFixSource.Image = this.imageList.Images[2];
                        break;
                    case FixSourceMode.CacheDB:
                        Source = "Source: " + this.sourceServerConnection.DisplayText;
                        this.buttonFixSource.Image = this.imageList.Images[3];
                        break;
                    case FixSourceMode.Webservice:
                        var info = TaxonomicServiceInfoDictionary();
                        if (info.TryGetValue(this.SourceWebservice, out var serviceInfo))
                            Source = "Source: " + SourceWebservice.ToString();
                        this.buttonFixSource.Image = this.imageList.Images[0];
                        break;
                    case FixSourceMode.None:
                    default:
                        Source = "Set the source";
                        this.buttonFixSource.Image = this.imageList.Images[1];
                        break;
                }
                this.toolTip.SetToolTip(this.buttonFixSource, Source);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        // setting visibility, backcolor and tool tip for combobox
        private void setFixSourceControls_comboBoxLocalValues()
        {
            try
            {
                string Search = "";
                switch (this.FixedSourceMode)
                {
                    case FixSourceMode.Database:
                    case FixSourceMode.CacheDB:
                        this.comboBoxLocalValues.Visible = false;
                        break;
                    case FixSourceMode.Webservice:
                        this.comboBoxLocalValues.Visible = true;
                        this.comboBoxLocalValues.BackColor = System.Drawing.Color.SkyBlue;
                        Search = "Search for values in " + SourceWebservice.ToString();
                        break;
                    case FixSourceMode.None:
                    default:
                        if (this.textBoxValue.AutoCompleteCustomSource != null && this.textBoxValue.AutoCompleteCustomSource.Count > 0)
                            this.comboBoxLocalValues.Visible = false;
                        else
                            this.comboBoxLocalValues.Visible = this.sourceServerConnection == null;
                        this.comboBoxLocalValues.BackColor = System.Drawing.Color.White;
                        Search = "Search for local values";
                        break;
                }
                this.toolTip.SetToolTip(this.comboBoxLocalValues, Search);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion


        #endregion

        #region Local values and data binding

        public void bindToData(string TableName, string DisplayColumn, string ValueColumn, System.Windows.Forms.BindingSource BindingSource)
        {
            try
            {
                this.textBoxValue.DataBindings.Clear();
                if (this._BindingSource != null)
                {
                    this._BindingSource.CurrentChanged -= new EventHandler(this.currentBinding_Changed);
                }
                this._BindingSource = BindingSource;
                // 23.11.22 - search for bug
                this._BindingSource.CurrentChanged += new EventHandler(this.currentBinding_Changed);
                this.textBoxValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BindingSource, DisplayColumn, true));
                this.labelURI.DataBindings.Clear();
                this.labelURI.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BindingSource, ValueColumn, true));
                this._SQL = "SELECT DISTINCT " + DisplayColumn + " AS DisplayText, " +
                    "CASE WHEN RTRIM(" + ValueColumn + ") = '' THEN NULL ELSE " + ValueColumn + " END AS URI " +
                    "FROM " + TableName + "  WHERE (NOT (" + DisplayColumn + " IS NULL)) AND " + DisplayColumn + " <> N''";
                this._DisplayColumn = DisplayColumn;
                this._ValueColumn = ValueColumn;
                this._TableName = TableName;
                this.setControls();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, this._SQL);
            }
        }

        public void bindToData(string TableName, string DisplayColumn, string ValueColumn, string WhereClause, System.Windows.Forms.BindingSource BindingSource)
        {
            try
            {
                this.bindToData(TableName, DisplayColumn, ValueColumn, BindingSource);
                this._SQL += " AND " + WhereClause;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public bool LocalValuesPossible
        {
            set { _localValuesPossible = value; }
        }

        public void setRemoteValueBindings(System.Collections.Generic.List<DiversityWorkbench.UserControls.RemoteValueBinding> RemoteValueBindings)
        {
            if (RemoteValueBindings.Count > 0)
            {
                if (this._RemoteValueBindings == null) this._RemoteValueBindings = new List<RemoteValueBinding>();
                foreach (DiversityWorkbench.UserControls.RemoteValueBinding R in RemoteValueBindings)
                {
                    this._RemoteValueBindings.Add(R);
                }
            }
        }

        private string DisplayColumn
        {
            set { this._DisplayColumn = value; }
        }

        public void setTableSource(string TableName, string DisplayColumn, string ValueColumn, string Condition)
        {
            this._SQL = "SELECT DISTINCT " + DisplayColumn + " AS DisplayText, CASE WHEN RTRIM(" + ValueColumn + ") = '' THEN NULL ELSE " + ValueColumn + " END AS URI FROM " + TableName + "  WHERE (NOT (" + DisplayColumn + " IS NULL)) AND " + DisplayColumn + " <> N''";
            if (Condition.Length > 0) this._SQL += " AND " + Condition;
            this._DisplayColumn = DisplayColumn;
            this._ValueColumn = ValueColumn;
            this._TableName = TableName;
        }

        public void setTableSource(string TableName, string DisplayColumn, string ValueColumn, string Condition, string[] ValueColumns)
        {
            this._SQL = "SELECT DISTINCT " + DisplayColumn + " AS DisplayText, CASE WHEN RTRIM(" + ValueColumn + ") = '' THEN NULL ELSE " + ValueColumn + " END AS URI ";
            foreach (string s in ValueColumns)
            {
                this._SQL += ", " + s;
                this.Values.Add(s, "");
            }
            this._SQL += " FROM " + TableName + "  WHERE (NOT (" + DisplayColumn + " IS NULL)) AND " + DisplayColumn + " <> N''";
            if (Condition.Length > 0) this._SQL += " AND " + Condition;
            this._DisplayColumn = DisplayColumn;
            this._ValueColumn = ValueColumn;
            this._TableName = TableName;
        }

        private void comboBoxLocalValues_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (this.comboBoxLocalValues == null || this.comboBoxLocalValues.SelectedItem == null)
                {
                    return;
                }
                System.Data.DataRowView r = (System.Data.DataRowView)this.comboBoxLocalValues.SelectedItem;
                if (r != null)
                {
                    if (this._BindingSource != null)
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this._BindingSource.Current;
                        R.BeginEdit();
                        string Display = "";
                        if (r.Row.Table.Columns[0].ColumnName == "_DisplayText" || r.Row.Table.Columns[0].ColumnName == "DisplayText")
                            Display = r[0].ToString();
                        else if (r.Row.Table.Columns[1].ColumnName == "_DisplayText" || r.Row.Table.Columns[1].ColumnName == "DisplayText")
                            Display = r[1].ToString();
                        else
                            Display = r[0].ToString();
                        R[this._DisplayColumn] = Display;
                        this.textBoxValue.Text = Display;
                        if (!r[1].Equals(System.DBNull.Value))
                        {
                            string URI = "";
                            if (r.Row.Table.Columns[0].ColumnName == "_URI" ||
                                r.Row.Table.Columns[0].ColumnName == "URI")
                                URI = r[0].ToString();
                            else if (r.Row.Table.Columns[1].ColumnName == "_URI" ||
                                     r.Row.Table.Columns[1].ColumnName == "URI")
                                URI = r[1].ToString();
                            else
                                URI = r[0].ToString();
                            // Ariane exclude Webservice.. for .NET8
                            //if (!URI.StartsWith("http:"))
                            //{
                            //    URI = this.sourceWebservice.BaseURI() + URI;
                            //    URI += this.sourceWebservice.UriXmlNameSuffix();
                            //}
                            R[this._ValueColumn] = URI;
                            this.labelURI.Text = URI;


                            if (this.sourceServerConnection != null && this._RemoteValueBindings != null &&
                                this._RemoteValueBindings.Count > 0)
                            {
                                this._IWorkbenchUnit.setServerConnection(this.sourceServerConnection);
                                int ID;
                                if (int.TryParse(DiversityWorkbench.WorkbenchUnit.getIDFromURI(r[1].ToString()),
                                        out ID))
                                {
                                    foreach (DiversityWorkbench.UserControls.RemoteValueBinding RVB in this
                                                 ._RemoteValueBindings)
                                    {
                                        System.Collections.Generic.Dictionary<string, string> UnitValues =
                                            this._IWorkbenchUnit.UnitValues(ID);
                                        foreach (System.Collections.Generic.KeyValuePair<string, string> P in
                                                 UnitValues)
                                        {
                                            if (RVB.RemoteParameter == P.Key)
                                            {
                                                if (RVB.BindingSource != null)
                                                {
                                                    if (RVB.BindingSource.Current != null)
                                                    {
                                                        System.Data.DataRowView RV =
                                                            (System.Data.DataRowView)RVB.BindingSource.Current;

                                                        ///MW 4.5.2105: enable decision if an allready filled field should be changed
                                                        if (RVB.ModeOfUpdate ==
                                                            RemoteValueBinding.UpdateMode.AskForUpdate)
                                                        {
                                                            if (!RV[RVB.Column].Equals(System.DBNull.Value) &&
                                                                RV[RVB.Column].ToString().Length > 0 &&
                                                                P.Value.Length > 0)
                                                            {
                                                                string Message = "Should the current value\r\n\r\n" +
                                                                    RV[RVB.Column].ToString() +
                                                                    "\r\n\r\nfor the field " + RVB.Column +
                                                                    "\r\nshould be replaced by\r\n\r\n" + P.Value;
                                                                if (System.Windows.Forms.MessageBox.Show(Message,
                                                                        "Replace entry", MessageBoxButtons.YesNo) ==
                                                                    DialogResult.No)
                                                                    continue;
                                                            }
                                                            else if ((RV[RVB.Column].Equals(System.DBNull.Value) ||
                                                                      RV[RVB.Column].ToString().Length == 0) &&
                                                                     P.Value.Length > 0)
                                                            {
                                                            }
                                                            else
                                                                continue;
                                                        }
                                                        else if (RVB.ModeOfUpdate ==
                                                                 RemoteValueBinding.UpdateMode.OnlyEmpty)
                                                        {
                                                            if (!RV[RVB.Column].Equals(System.DBNull.Value) &&
                                                                RV[RVB.Column].ToString().Length > 0)
                                                                continue;
                                                        }

                                                        RV.BeginEdit();
                                                        try
                                                        {
                                                            if (this._SupressEmptyRemoteValues && P.Value.Length == 0)
                                                            {
                                                            }
                                                            else
                                                                RV[RVB.Column] = P.Value;
                                                        }
                                                        catch (System.Exception ex)
                                                        {
                                                        }

                                                        RV.EndEdit();
                                                    }
                                                }
                                                else
                                                {
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //// Ariane exclude Webservice.. for .NET8
                            ///  Ariane Todo Do we need this?
                            //else if (SourceWebservice != DwbService.None && this._RemoteValueBindings != null &&
                            //         this._RemoteValueBindings.Count > 0)
                            //{
                            //    //System.Collections.Generic.Dictionary<string, string> UnitValues =
                            //    //    this.sourceWebservice.UriValues(URI);
                            //    //if (UnitValues.Count > 0)
                            //    //{
                            //        System.Collections.Generic.List<string> RemoteBindings = new List<string>();
                            //        //foreach (DiversityWorkbench.UserControls.RemoteValueBinding RVB in this
                            //        //             ._RemoteValueBindings)
                            //        //    RemoteBindings.Add(RVB.RemoteParameter);
                            //        //System.Collections.Generic.Dictionary<string, string> Values =
                            //        //    this.sourceWebservice.UriValues(URI, RemoteBindings);
                            //        //if (Values.Count > 0)
                            //        //{
                            //        //    foreach (DiversityWorkbench.UserControls.RemoteValueBinding RVB in this
                            //        //                 ._RemoteValueBindings)
                            //        //    {
                            //        //        foreach (System.Collections.Generic.KeyValuePair<string, string> P in
                            //        //                 Values)
                            //        //        {
                            //        //            if (RVB.RemoteParameter.ToLower() == P.Key.ToLower())
                            //        //            {
                            //        //                if (RVB.BindingSource != null)
                            //        //                {
                            //        //                    if (RVB.BindingSource.Current != null)
                            //        //                    {
                            //        //                        System.Data.DataRowView RV =
                            //        //                            (System.Data.DataRowView)RVB.BindingSource.Current;

                            //        //                        ///MW 4.5.2105: enable decision if an allready filled field should be changed
                            //        //                        if (RVB.ModeOfUpdate == RemoteValueBinding.UpdateMode
                            //        //                                .AskForUpdate)
                            //        //                        {
                            //        //                            if (!RV[RVB.Column].Equals(System.DBNull.Value) &&
                            //        //                                RV[RVB.Column].ToString().Length > 0 &&
                            //        //                                P.Value.Length > 0)
                            //        //                            {
                            //        //                                string Message =
                            //        //                                    "Should the current value\r\n\r\n" +
                            //        //                                    RV[RVB.Column].ToString() +
                            //        //                                    "\r\n\r\nfor the field " + RVB.Column +
                            //        //                                    "\r\nshould be replaced by\r\n\r\n" + P.Value;
                            //        //                                if (System.Windows.Forms.MessageBox.Show(Message,
                            //        //                                        "Replace entry", MessageBoxButtons.YesNo) ==
                            //        //                                    DialogResult.No)
                            //        //                                    continue;
                            //        //                            }
                            //        //                            else if ((RV[RVB.Column].Equals(System.DBNull.Value) ||
                            //        //                                      RV[RVB.Column].ToString().Length == 0) &&
                            //        //                                     P.Value.Length > 0)
                            //        //                            {
                            //        //                            }
                            //        //                            else
                            //        //                                continue;
                            //        //                        }
                            //        //                        else if (RVB.ModeOfUpdate ==
                            //        //                                 RemoteValueBinding.UpdateMode.OnlyEmpty)
                            //        //                        {
                            //        //                            if (!RV[RVB.Column].Equals(System.DBNull.Value) &&
                            //        //                                RV[RVB.Column].ToString().Length > 0)
                            //        //                                continue;
                            //        //                        }

                            //        //                        RV.BeginEdit();
                            //        //                        try
                            //        //                        {
                            //        //                            if (this._SupressEmptyRemoteValues &&
                            //        //                                P.Value.Length == 0)
                            //        //                            {
                            //        //                            }
                            //        //                            else
                            //        //                                RV[RVB.Column] = P.Value;
                            //        //                        }
                            //        //                        catch (System.Exception ex)
                            //        //                        {
                            //        //                        }

                            //        //                        RV.EndEdit();
                            //        //                    }
                            //        //                }
                            //        //                else
                            //        //                {
                            //        //                }
                            //        //            }
                            //        //        }
                            //        //    }
                            //        //}
                            //        //else
                            //        //{
                            //            foreach (DiversityWorkbench.UserControls.RemoteValueBinding RVB in this
                            //                         ._RemoteValueBindings)
                            //            {
                            //                Dictionary<string, string> UnitValues = new Dictionary<string, string>();
                            //                UnitValues.Add("URI", URI);
                            //                foreach (System.Collections.Generic.KeyValuePair<string, string> P in
                            //                         UnitValues)
                            //                {
                            //                    if (RVB.RemoteParameter.ToLower() == P.Key.ToLower())
                            //                    {
                            //                        if (RVB.BindingSource != null)
                            //                        {
                            //                            if (RVB.BindingSource.Current != null)
                            //                            {
                            //                                System.Data.DataRowView RV =
                            //                                    (System.Data.DataRowView)RVB.BindingSource.Current;

                            //                                ///MW 4.5.2105: enable decision if an allready filled field should be changed
                            //                                if (RVB.ModeOfUpdate == RemoteValueBinding.UpdateMode
                            //                                        .AskForUpdate)
                            //                                {
                            //                                    if (!RV[RVB.Column].Equals(System.DBNull.Value) &&
                            //                                        RV[RVB.Column].ToString().Length > 0 &&
                            //                                        P.Value.Length > 0)
                            //                                    {
                            //                                        string Message =
                            //                                            "Should the current value\r\n\r\n" +
                            //                                            RV[RVB.Column].ToString() +
                            //                                            "\r\n\r\nfor the field " + RVB.Column +
                            //                                            "\r\nshould be replaced by\r\n\r\n" + P.Value;
                            //                                        if (System.Windows.Forms.MessageBox.Show(Message,
                            //                                                "Replace entry", MessageBoxButtons.YesNo) ==
                            //                                            DialogResult.No)
                            //                                            continue;
                            //                                    }
                            //                                    else if ((RV[RVB.Column].Equals(System.DBNull.Value) ||
                            //                                              RV[RVB.Column].ToString().Length == 0) &&
                            //                                             P.Value.Length > 0)
                            //                                    {
                            //                                    }
                            //                                    else
                            //                                        continue;
                            //                                }
                            //                                else if (RVB.ModeOfUpdate ==
                            //                                         RemoteValueBinding.UpdateMode.OnlyEmpty)
                            //                                {
                            //                                    if (!RV[RVB.Column].Equals(System.DBNull.Value) &&
                            //                                        RV[RVB.Column].ToString().Length > 0)
                            //                                        continue;
                            //                                }

                            //                                RV.BeginEdit();
                            //                                try
                            //                                {
                            //                                    if (this._SupressEmptyRemoteValues &&
                            //                                        P.Value.Length == 0)
                            //                                    {
                            //                                    }
                            //                                    else
                            //                                        RV[RVB.Column] = P.Value;
                            //                                }
                            //                                catch (System.Exception ex)
                            //                                {
                            //                                }

                            //                                RV.EndEdit();
                            //                            }
                            //                        }
                            //                        else
                            //                        {
                            //                        }
                            //                    }
                            //                }
                            //           // }
                            //        }
                            //    }
                            }

                        R.EndEdit();
                    }
                    else
                    {
                        this.textBoxValue.Text = r[0].ToString();
                        this.labelURI.Text = r[1].ToString();
                    }
                    this.setControls();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private Microsoft.Data.SqlClient.SqlDataAdapter DataAdapter
        {
            get
            {
                if (this._DataAdapter == null)
                {
                    try
                    {
                        this._DataAdapter = new Microsoft.Data.SqlClient.SqlDataAdapter(this._SQL, DiversityWorkbench.Settings.ConnectionString);
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                return this._DataAdapter;
            }
        }

        private async void comboBoxLocalValues_MouseClick(object sender, MouseEventArgs e)
        {
            // Disable the ComboBox to prevent user interaction during data loading
            comboBoxLocalValues.Enabled = false;
            await this.SetComboBoxLocalSource();
            comboBoxLocalValues.Enabled = true;
            // Open the dropdown only if there are items in the ComboBox
            if (comboBoxLocalValues.Items.Count > 0)
            {
                comboBoxLocalValues.DroppedDown = true;
            }
        }

        private async void comboBoxLocalValues_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                // Disable the ComboBox to prevent user interaction during data loading
                comboBoxLocalValues.Enabled = false;
                await SetComboBoxLocalSource();
                comboBoxLocalValues.Enabled = true;
                // Open the dropdown only if there are items in the ComboBox
                if (comboBoxLocalValues.Items.Count > 0)
                {
                    comboBoxLocalValues.Focus();
                    comboBoxLocalValues.DroppedDown = true;
                }
            }
        }

        private async System.Threading.Tasks.Task SetComboBoxLocalSource()
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string SQL = "";
            try
            {
                this._dtTable.Clear();
                int i = this.comboBoxLocalValues.Width + this.textBoxValue.Width;
                if (this.comboBoxLocalValues.DropDownWidth != i)
                    this.comboBoxLocalValues.DropDownWidth = i;
                if (this.textBoxValue.Text.Length > 0)
                {
                    if (this._FixingOfSourceEnabled && this.sourceServerConnection != null)
                    {
                        string Prefix = this.sourceServerConnection.Prefix();
                        SQL = "SELECT ";
                        switch (this.sourceServerConnection.ModuleName)
                        {
                            case "DiversityExsiccatae":
                                SQL +=
                                    "T.ExsAbbreviation + case when T.ExsNumberFirst <> '' or T.ExsNumberLast <> '' then ' (' else '' end + " +
                                    "case when T.ExsNumberFirst  is null or len(T.ExsNumberFirst) = 0 then '' else T.ExsNumberFirst end + " +
                                    "case when T.ExsNumberFirst <> '' and T.ExsNumberLast <> '' then ' - ' else '' end + " +
                                    "case when T.ExsNumberLast  is null or len(T.ExsNumberLast) = 0 then '' else T.ExsNumberLast end +" +
                                    "case when T.ExsNumberFirst <> '' or T.ExsNumberLast <> '' then ')' else '' end";
                                break;
                            default:
                                SQL += "T." + this._IWorkbenchUnit.QueryDisplayColumns()[0].DisplayColumn;
                                break;
                        }

                        SQL += " AS DisplayText, U.BaseURL + CAST(T." +
                               this._IWorkbenchUnit.QueryDisplayColumns()[0].IdentityColumn +
                               " AS VARCHAR(500)) AS URI, U.BaseURL, T." +
                               this._IWorkbenchUnit.QueryDisplayColumns()[0].IdentityColumn + " AS ID " +
                               " FROM " + Prefix + "VIEWBASEURL U, " + Prefix +
                               this._IWorkbenchUnit.QueryDisplayColumns()[0].TableName + " T ";
                        if (this.sourceServerConnection.ProjectID != null)
                        {
                            switch (this.sourceServerConnection.ModuleName)
                            {
                                case "DiversityAgents":
                                    SQL += " INNER JOIN " + Prefix +
                                           "AgentProject P ON P.AgentID = T.AgentID AND P.ProjectID = " +
                                           this.sourceServerConnection.ProjectID.ToString();
                                    break;
                                case "DiversityCollection":
                                    SQL += " INNER JOIN " + Prefix +
                                           "CollectionProject P ON P.CollectionSpecimenID = T.CollectionSpecimenID AND P.ProjectID = " +
                                           this.sourceServerConnection.ProjectID.ToString();
                                    break;
                                case "DiversityGazetteer":
                                    SQL += " INNER JOIN " + Prefix +
                                           "GeoProject P ON P.NameID = T.NameID AND P.ProjectID = " +
                                           this.sourceServerConnection.ProjectID.ToString();
                                    break;
                                case "DiversityReferences":
                                    SQL += " INNER JOIN " + Prefix +
                                           "ReferencesProject P ON P.RefID = T.RefID AND P.ProjectID = " +
                                           this.sourceServerConnection.ProjectID.ToString();
                                    break;
                                case "DiversitySamplingPlots":
                                    SQL += " INNER JOIN " + Prefix +
                                           "SamplingProject P ON P.PlotID = T.PlotID AND P.ProjectID = " +
                                           this.sourceServerConnection.ProjectID.ToString();
                                    break;
                                case "DiversityScientificTerms":
                                    SQL =
                                        "SELECT CASE WHEN TP.DisplayText IS NULL THEN '' ELSE TP.DisplayText + ' | ' END + T.DisplayText AS DisplayText, " +
                                        "U.BaseURL + CAST(T." +
                                        this._IWorkbenchUnit.QueryDisplayColumns()[0].IdentityColumn +
                                        " AS VARCHAR(500)) AS URI, U.BaseURL, T." +
                                        this._IWorkbenchUnit.QueryDisplayColumns()[0].IdentityColumn + " AS ID " +
                                        " FROM " + Prefix + "VIEWBASEURL U, " + Prefix +
                                        this._IWorkbenchUnit.QueryDisplayColumns()[0].TableName + " T " +
                                        " INNER JOIN " + Prefix +
                                        "Term AS ET ON T.TermID = ET.TermID AND T.TerminologyID = ET.TerminologyID  AND T.TerminologyID = " +
                                        this.sourceServerConnection.ProjectID.ToString() +
                                        " LEFT OUTER JOIN " + Prefix + "Term AS EP " +
                                        " INNER JOIN " + Prefix +
                                        "TermRepresentation AS TP ON EP.TerminologyID = TP.TerminologyID AND EP.TermID = TP.TermID AND EP.PreferredRepresentationID = TP.RepresentationID " +
                                        " ON ET.TerminologyID = EP.TerminologyID AND ET.BroaderTermID = EP.TermID AND ET.IsRankingTerm = 0 ";
                                    break;
                                case "DiversityTaxonNames":
                                    SQL += " INNER JOIN " + Prefix +
                                           "TaxonNameProject P ON P.NameID = T.NameID AND P.ProjectID = " +
                                           this.sourceServerConnection.ProjectID.ToString();
                                    break;
                            }
                            //SQL += " AND ";
                        }

                        if (this.sourceServerConnection.SectionID != null)
                        {
                            switch (this.sourceServerConnection.ModuleName)
                            {
                                case "DiversityScientificTerms":
                                    SQL += " INNER JOIN " + Prefix +
                                           "SectionTerm S ON TP.TerminologyID = S.TerminologyID AND S.TermID = TP.TermID AND S.SectionID = " +
                                           this.sourceServerConnection.SectionID.ToString();
                                    break;
                                case "DiversityTaxonNames":
                                    SQL += " INNER JOIN " + Prefix +
                                           "TaxonNameList L ON L.NameID = T.NameID AND L.ProjectID = " +
                                           this.sourceServerConnection.SectionID.ToString();
                                    break;
                            }
                            //SQL += " AND ";
                        }

                        //else if (!SQL.Trim().EndsWith(" AND"))
                        //    SQL += "  ";
                        SQL += " WHERE T." + this._IWorkbenchUnit.QueryDisplayColumns()[0].DisplayColumn +
                               " LIKE '" + this.textBoxValue.Text + "%'";
                        if (this._IsListInDatabase && this._ListInDatabase.Length > 0)
                        {
                            foreach (DiversityWorkbench.DatabaseService D in this._IWorkbenchUnit.DatabaseServices())
                            {
                                if (D.ListName == this._ListInDatabase)
                                {
                                    if (D.RestrictionForListInDatabase.Length > 0)
                                        SQL += " AND " + D.RestrictionForListInDatabase;
                                    break;
                                }
                            }
                        }

                        switch (this.sourceServerConnection.ModuleName)
                        {
                            case "DiversityScientificTerms":
                                string LinkedTable = "";
                                if (this.DataRowView != null)
                                {
                                    LinkedTable = this.DataRowView.Row.Table.TableName;
                                }

                                switch (LinkedTable)
                                {
                                    case "CollectionEventProperty":
                                    case "SamplingPlotProperty":
                                        SQL += " AND PropertyID = " + this.DataRowView["PropertyID"].ToString();
                                        break;
                                }

                                SQL += " ORDER BY TP.DisplayText, T.DisplayText";
                                break;
                            default:
                                SQL += " ORDER BY " + this._IWorkbenchUnit.QueryDisplayColumns()[0].DisplayColumn;
                                break;
                        }

                        Microsoft.Data.SqlClient.SqlDataAdapter ad =
                            new Microsoft.Data.SqlClient.SqlDataAdapter(SQL,
                                this.sourceServerConnection.ConnectionString);
                        ad.Fill(this._dtTable);
                        this.comboBoxLocalValues.DataSource = this._dtTable;
                        this.comboBoxLocalValues.DisplayMember = "DisplayText";
                        this.comboBoxLocalValues.ValueMember = "URI";
                    }
                    else if (this._FixingOfSourceEnabled && SourceWebservice != DwbService.None)
                    {
                        System.Data.DataTable dt = new DataTable();
                        if (string.IsNullOrEmpty(textBoxValue.Text))
                        {
                            MessageBox.Show("Please enter at least one character");
                            return;
                        }

                        IDwbWebservice<DwbSearchResult, DwbSearchResultItem, DwbEntity> _api =
                            DwbServiceProviderAccessor.GetDwbWebservice(SourceWebservice);
                        var searchUrl = CreateSearchUrl(this.textBoxValue.Text, _api, SourceWebservice);
                        if (_api == null || string.IsNullOrEmpty(searchUrl))
                        {
                            MessageBox.Show("No webservice defined");
                            return;
                        }

                        try
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            var tt = await _api.CallWebServiceAsync<object>(searchUrl,
                                DwbServiceEnums.HttpAction.GET);
                            if (tt != null)
                            {
                                var clientSearchModel = _api.GetDwbApiSearchResultModel(tt);
                                ReadDwbSearchModelInQueryTable(clientSearchModel, ref dt);
                            }
                        }
                        catch (Exception ioe)
                        {
                            MessageBox.Show(
                                "The record details cannot be displayed because the web service response is invalid.\r\n\r\n  " +
                                "For more details on the error, see the error log file.\r\n\r\n",
                                "Data Mapping Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            ExceptionHandling.WriteToErrorLogFile(
                                "UserControlModulRelatedEntry - SetComboBoxLocalSource - Exception exception: " +
                                ioe);
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                        }

                        this.comboBoxLocalValues.DataSource = dt;
                        this.comboBoxLocalValues.ValueMember = "URI";
                        this.comboBoxLocalValues.DisplayMember = "DisplayText";
                    }
                    else if (this._SQL != null && this._SQL.Length > 0)
                    {
                        SQL = this._SQL;
                        if (this.textBoxValue.Text.Length > 0)
                        {
                            string LinkedTable = "";
                            if (this.BindingContext != null && this.DataRowView != null)
                            {
                                LinkedTable = this.DataRowView.Row.Table.TableName;
                            }

                            switch (LinkedTable)
                            {
                                case "CollectionEventProperty":
                                case "SamplingPlotProperty":
                                    SQL += " AND PropertyID = " + this.DataRowView["PropertyID"].ToString();
                                    break;
                            }

                            SQL += " AND " + this._DisplayColumn + " LIKE '" + this.textBoxValue.Text + "%' ORDER BY " +
                                   this._DisplayColumn;
                            this.DataAdapter.SelectCommand.CommandText = SQL;
                            this.DataAdapter.Fill(this._dtTable);
                            this.comboBoxLocalValues.DataSource = this._dtTable;
                            this.comboBoxLocalValues.DisplayMember = "DisplayText";
                            this.comboBoxLocalValues.ValueMember = "URI";
                        }
                        else
                            System.Windows.Forms.MessageBox.Show(
                                "Please give at least the initial characters of the searched item");
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(
                        "Please give at least the initial characters of the searched item");
                    this.comboBoxLocalValues.DataSource = null;
                }
            }
            catch (NotSupportedException notSupported)
            {
                MessageBox.Show("The webservice is not supported. Message: " + notSupported.Message);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(notSupported);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred when trying to use the webservice. Message: " + ex.Message);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
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
            string Restriction = userInputText;

            // Ariane UrlEncoding has to be done within DwbApiQueryUrlString method of each implemented webservice when creating the search criteria
            // since we have Webservices which use a POST call and body content also for getting data
            //Restriction = HttpUtility.UrlEncode(Restriction);
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
                    dtQuery.Columns.Add(new DataColumn(column.Name, Type.GetType(column.Type)));
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
        #endregion

        #region Fixing the source for the combobox

        /// <summary>
        /// the source for the data provided in the combobox may be derived from local data (see region Local values and data binding)
        /// or from a module source. This source is selected from the module connections and directly accesses the remote datasource.
        /// This is (for now) only possible for DiversityWorkbench modules (may be provided for other sources in the future)
        /// Needed:
        /// Fixing and releasing
        /// Save the settings
        /// Change setting in dependence of the selected data, e.g. if the sources differ for taxonomic groups in DiversityCollection
        /// The table and columns are taken from the module resp. the first QueryDisplayColumn
        /// </summary>
        public bool FixingOfSourceEnabled
        {
            set
            {
                this._FixingOfSourceEnabled = value;
            }
        }

        private bool _FixingOfSourceEnabled = false;

        private DiversityWorkbench.ServerConnection _SourceServerConnection;
        private DiversityWorkbench.ServerConnection sourceServerConnection
        {
            get { return _SourceServerConnection; }
            set
            {
                _SourceServerConnection = value;
                if (value != null && _SourceServerConnection != null)
                {
                    SourceWebservice = DwbService.None;
                    this._SourceServerConnection.ModuleName = this.ServerConnection.ModuleName;
                }
                this.FixedSourceModeReset();
            }
        }

        private DWBServices.WebServices.DwbServiceEnums.DwbService _sourceWebservice;

        public DWBServices.WebServices.DwbServiceEnums.DwbService
            SourceWebservice
        {
            get => _sourceWebservice;
            private set
            {
                _sourceWebservice = value;
                if (value != DwbService.None) _SourceServerConnection = null;
                this.FixedSourceModeReset();
            }
            //else this._SourceWebserviceOptions = null;
            // this.FixedSourceModeReset();
        }


        public void FixSource(string ServerConnection)
        {
            try
            {

                bool SourceFound = false;
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> SC in this._IWorkbenchUnit.ServerConnections())
                {
                    if (SC.Key == ServerConnection)
                    {
                        this.sourceServerConnection = SC.Value;
                        SourceFound = true;
                        break;
                    }
                }
                if (!SourceFound)
                {
                    if (this._IWorkbenchUnit.AccessibleDatabasesAndServicesOfModule() != null)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._IWorkbenchUnit.AccessibleDatabasesAndServicesOfModule())
                        {
                            if (KV.Key.ToLower() == ServerConnection.ToLower() || KV.Key.ToLower() == ServerConnection.ToLower().Replace(" ", ""))
                            {
                                _ = DiversityWorkbench.WorkbenchUnit.FixedSourceWebservice(KV.Key,
                                    ref this._SourceServerConnection,
                                    ref this._sourceWebservice);
                                this.sourceServerConnection = null;
                                break;
                            }
                        }
                    }
                }
                //this.setControls();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void FixSource(string ServerConnection,
            string ProjectID, string Project,
            int? SectionID = null, string Section = "",
            int? Width = null, int? Height = null)
        {
            try
            {
                if (this._IWorkbenchUnit != null)
                {
                    if (this.sourceServerConnection != null && this.sourceServerConnection.CacheDB != null && this.sourceServerConnection.CacheDB != "")
                    {
                        this.sourceServerConnection.CacheDB = "";
                        this.sourceServerConnection.CacheDBSourceView = "";
                    }

                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> SC in this._IWorkbenchUnit.ServerConnections())
                    {
                        if (SC.Key == ServerConnection)
                        {
                            this.sourceServerConnection = SC.Value;
                            int iProjectID;
                            if (ProjectID.Length > 0 && int.TryParse(ProjectID, out iProjectID))
                            {
                                this.sourceServerConnection.ProjectID = iProjectID;
                                this.sourceServerConnection.Project = Project;
                                if (SectionID != null && Section.Length > 0)
                                {
                                    this.sourceServerConnection.SectionID = SectionID;
                                    this.sourceServerConnection.Section = Section;
                                }
                                else
                                {
                                    this.sourceServerConnection.Section = null;
                                    this.sourceServerConnection.SectionID = null;
                                }
                            }
                            break;
                        }
                    }
                    if (this.sourceServerConnection != null && Width != null && Height != null)
                    {
                        this.setChart(this.sourceServerConnection, (int)Width, (int)Height);
                    }
                }
                this.setControls();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void FixSource(string CacheDatabase,
            string ProjectID, string Project)
        {
            try
            {
                this.sourceServerConnection = DiversityWorkbench.Settings.ServerConnection;
                this.sourceServerConnection.CacheDB = CacheDatabase;
                this.sourceServerConnection.CacheDBSourceView = Project;
                this.setControls();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        public DiversityWorkbench.ServerConnection SourceServerConnection() { return this.sourceServerConnection; }

        public void ReleaseSource()
        {
            this.sourceServerConnection = null;
            this.SourceWebservice = DwbService.None;

            // Autocomplete
            this._autoCompleteStringCollection = null;
            this._AutocompleteLinks = null;

            this.setControls();
        }

        private void buttonFixSource_Click(object sender, EventArgs e)
        {
            this.SetFixSource();
            return;
        }

        private void SetFixSource()
        {
            try
            {
                string StartSetting = this.CurrentSetting();
                bool OK = false;
                bool ResetAutoComplete = false;
                System.Collections.Generic.List<string> Sources = this.Sources;
                if (this._IWorkbenchUnit.ServerConnections() != null)
                {
                    if (Sources.Count < 1 && !DiversityWorkbench.Settings.LoadConnections)
                    {
                        System.Windows.Forms.MessageBox.Show("So far no sources are available. Please connect to the available soures first", "No Sources", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(Sources, "Source", "Please select a source from the list", true);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        string Source = "";
                        if (f.SelectedString.Length == 0)
                        {
                            if (this._Autocomplete_Key.StartsWith("Local.") && System.Windows.Forms.MessageBox.Show("Reset autocomplete source for local values?", "Reset source?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                this._Autocomplete_Key = "";
                                this.TextBoxValue_SetAutocompleteOnDemandLocalSource(true);
                            }
                        }
                        else if (this._IWorkbenchUnit.ServerConnections().ContainsKey(f.SelectedString))
                        {
                            if (this.sourceServerConnection == null || (this.sourceServerConnection != null && this.sourceServerConnection.BaseURL != this._IWorkbenchUnit.ServerConnections()[f.SelectedString].BaseURL))
                                ResetAutoComplete = true;

                            this.sourceServerConnection = this._IWorkbenchUnit.ServerConnections()[f.SelectedString];

                            Source = this.sourceServerConnection.DisplayText;
                            SourceWebservice = DwbServiceEnums.DwbService.None;

                            if (this.sourceServerConnection.ModuleName != "DiversityExsiccatae") // DiversityExsiccatae has no project
                            {
                                // getting a project
                                DiversityWorkbench.Forms.FormGetProject fP = new Forms.FormGetProject(this.sourceServerConnection, "");
#if DEBUG
                                fP.DisplayOption = DiversityWorkbench.UserControls.UserControlDialogPanel.DisplayOption.Cancel_OK;
#endif
                                fP.ShowDialog();
                                if ((fP.DialogResult == DialogResult.OK || fP.DialogResult == DialogResult.Yes) && fP.ProjectID != null)
                                {
                                    if (!ResetAutoComplete && this.sourceServerConnection != null && this.sourceServerConnection.ProjectID != fP.ProjectID)
                                        ResetAutoComplete = true;
                                    this.sourceServerConnection.ProjectID = fP.ProjectID;
                                    this.sourceServerConnection.Project = fP.Project;
                                    OK = true;

                                    // getting an optional subgroup
                                    string HeaederText = "Do you want to restrict the ";
                                    Forms.FormGetProject.Subgroup subgroup = Forms.FormGetProject.Subgroup.None;
                                    string Restriction = "";
                                    switch (this.sourceServerConnection.ModuleName)
                                    {
                                        case "DiversityScientificTerms":
                                            subgroup = Forms.FormGetProject.Subgroup.Section;
                                            HeaederText += "terms to a section";
                                            Restriction = "TerminologyID = " + this.sourceServerConnection.ProjectID.ToString();
                                            break;
                                        case "DiversityTaxonNames":
                                            subgroup = Forms.FormGetProject.Subgroup.Checklist;
                                            HeaederText += "names to a checklist";
                                            break;
                                    }
                                    if (subgroup != Forms.FormGetProject.Subgroup.None)
                                    {
                                        DiversityWorkbench.Forms.FormGetProject fSub = new Forms.FormGetProject(this.sourceServerConnection, HeaederText, subgroup, Restriction, true);
                                        fSub.DisplayOption = DiversityWorkbench.UserControls.UserControlDialogPanel.DisplayOption.No_Yes;
                                        if (fSub.ProjectCount > 0)
                                        {
                                            fSub.ShowDialog();
                                            if (fSub.DialogResult == DialogResult.Yes && fSub.ProjectID != null)
                                            {
                                                this.sourceServerConnection.SectionID = fSub.ProjectID;
                                                this.sourceServerConnection.Section = fSub.Project;
                                            }
                                            else
                                            {
                                                this.sourceServerConnection.SectionID = null;
                                                this.sourceServerConnection.Section = null;
                                            }
                                        }
                                    }
                                }
                                else if (fP.DialogResult == DialogResult.No)
                                {
                                    OK = true;
                                }
                                else
                                    OK = false;
                                if (this.sourceServerConnection.Project != null && this.sourceServerConnection.Project.Length > 0)
                                {
                                    Source += " (" + this.sourceServerConnection.Project;
                                    if (this.sourceServerConnection.Section != null && this.sourceServerConnection.Section.Length > 0)
                                        Source += " - " + this.sourceServerConnection.Section;
                                    Source += ")";
                                }
                            }
                            else
                            {
                                OK = true;
                            }
                        }
                        else if (this._IWorkbenchUnit.AccessibleDatabasesAndServicesOfModule().ContainsKey(f.SelectedString) &&
                            this._IWorkbenchUnit.AdditionalServicesOfModule().ContainsKey(f.SelectedString))
                        {
                            Source = f.SelectedString;
                            if (!DiversityWorkbench.WorkbenchUnit.FixedSourceWebservice(
                               Source,
                               ref this._SourceServerConnection,
                               ref this._sourceWebservice))
                            {
                                if (Source == "CacheDB")
                                {
                                    if (!this.SetSourceCacheDB())
                                    {
                                        return;
                                    }
                                }
                            }
                            OK = true;
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("You may not have access to the selected source.\r\nPlease contact your Administrator");
                        }
                        if (OK)
                        {
                            this.FixedSourceModeReset();
                            this.setFixSourceControls();

                            // setting the new chart
                            if (this.sourceServerConnection != null && this.sourceServerConnection.ProjectID != null)
                            {
                                string URL = this.sourceServerConnection.BaseURL + this.sourceServerConnection.ProjectID.ToString();
                                switch (this.sourceServerConnection.ModuleName)
                                {
                                    case "DiversityTaxonNames":
                                        if (DiversityWorkbench.Settings.UseQueryCharts)
                                            this._Chart = DiversityWorkbench.TaxonName.GetChart(URL);
                                        break;
                                    case "DiversityScientificTerms":
                                        if (DiversityWorkbench.Settings.UseQueryCharts)
                                            this._Chart = DiversityWorkbench.Terminology.GetChart(URL, this.sourceServerConnection.SectionID);
                                        break;
                                }
                            }
                        }
                        else
                            this.ReleaseSource();
                    }
                    else
                    {
                        // Markus 1.11.22: No changes when using cancel. Now selection of the empty entry corresponds to releasing the source
                        return;
                        //this.ReleaseSource();
                    }
                }
                if (this.CurrentSetting() != StartSetting)
                    ResetAutoComplete = true;
                if (ResetAutoComplete)
                {
                    System.Windows.Forms.AutoCompleteMode Mode = AutoCompleteMode.Suggest;
                    if (SourceWebservice != DwbServiceEnums.DwbService.None && this.sourceServerConnection == null)
                        Mode = AutoCompleteMode.None;
                    this.AutoComplete_SetMode(Mode);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        private System.Collections.Generic.List<string> _sources;
        private System.Collections.Generic.List<string> Sources
        {
            get
            {
                if (_sources == null)
                {
                    _sources = new List<string>();
                    _sources.Add("");
                    try
                    {
                        if (this._IWorkbenchUnit.ServerConnections() != null)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> SC in this._IWorkbenchUnit.ServerConnections())
                            {
                                if (!_sources.Contains(SC.Key))
                                {
                                    _sources.Add(SC.Key);
                                }
                            }
                            if (this._IWorkbenchUnit.AccessibleDatabasesAndServicesOfModule() != null)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._IWorkbenchUnit.AccessibleDatabasesAndServicesOfModule())
                                {
                                    if (!_sources.Contains(KV.Key))
                                    {
                                        _sources.Add(KV.Key);
                                    }
                                }
                            }
                        }
                    }
                    catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                }
                return _sources;
            }
        }

        //private enum CacheDbSource {None, Agent, Gazetteer, Reference, Plot, Term, Taxa}
        //private CacheDbSource _CacheDbSource = CacheDbSource.None;
        private bool SetSourceCacheDB()
        {
            bool OK = true;
            try
            {
                SourceWebservice = DwbServiceEnums.DwbService.None;
                string DB = this.sourceServerConnection.CollectionCacheDB;// this.CacheDB;
                if (this.sourceServerConnection == null) this.sourceServerConnection = this._IWorkbenchUnit.getServerConnection();
                this.sourceServerConnection.CacheDB = DB;
                string[] ConStr = DiversityWorkbench.Settings.ConnectionString.Split(new char[] { ';' });
                string conStr = "";
                foreach (string S in ConStr)
                {
                    if (conStr.Length > 0) conStr += ";";
                    if (S.StartsWith("Initial"))
                    {
                        conStr += S.Substring(0, S.IndexOf("="));
                        conStr += "=" + DB;
                    }
                    else conStr += S;
                }
                if (this.sourceServerConnection == null)
                {
                    this.sourceServerConnection = DiversityWorkbench.Settings.ServerConnection;
                    this.sourceServerConnection.setConnection(conStr);
                    this.sourceServerConnection.DatabaseServer = DiversityWorkbench.Settings.DatabaseServer;
                }

                string Module = this._IWorkbenchUnit.ServiceName();
                string Source = "";
                switch (Module)
                {
                    case "DiversityAgents":
                        Source = "AgentSource";
                        break;
                    case "DiversityGazetteer":
                        Source = "GazetteerSource";
                        break;
                    case "DiversityReferences":
                        Source = "ReferenceTitleSource";
                        break;
                    case "DiversitySamplingPlots":
                        Source = "SamplingPlotSource";
                        break;
                    case "DiversityScientificTerms":
                        Source = "ScientificTermSource";
                        break;
                    case "DiversityTaxonNames":
                        Source = "TaxonSynonymySource";
                        break;
                }
                string SQL = "SELECT [SourceView] AS SourceView, REPLACE([SourceView], '_', ' ') AS Display FROM [" + DB + "].[dbo].[" + Source + "]";
                System.Data.DataTable dt = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Display", "SourceView", "Source view in CacheDB", "Please select a source in the cache database", "", false, true);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    if (f.SelectedValue.Length > 0 && f.SelectedString.Length > 0)
                    {
                        this.sourceServerConnection.CacheDBSourceView = f.SelectedValue;
                        this.sourceServerConnection.SectionID = null;
                        this.sourceServerConnection.Section = null;
                    }
                    else
                    {
                        this._autoCompleteMode = AutoCompleteMode.None;
                        this._autoCompleteStringCollection = null;
                        this.sourceServerConnection = null;
                        SourceWebservice = DwbServiceEnums.DwbService.None;
                    }
                }
                else OK = false;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); OK = false; }
            return OK;
        }

        private string CurrentSetting()
        {
            string CurrentSetting = "";
            try
            {
                if (this.sourceServerConnection != null) CurrentSetting = this.sourceServerConnection.Key();
                else if (SourceWebservice != DwbServiceEnums.DwbService.None)
                {
                    var tinfo = TaxonomicServiceInfoDictionary();
                    if (tinfo.TryGetValue(SourceWebservice, out var serviceTypeInfo))
                    {
                        CurrentSetting = serviceTypeInfo?.Url ?? string.Empty;
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return CurrentSetting;
        }

        #region CacheDB

        //private void CheckConnectionChange()
        //{
        //    if (this._ConnStrCurrent == null)
        //    {
        //        _CacheDB = null;
        //        _CacheDbAccessible = null;
        //        _ConnStrCurrent = DiversityWorkbench.Settings.ConnectionString;
        //    }
        //    else
        //    {
        //        if (_ConnStrCurrent != DiversityWorkbench.Settings.ConnectionString)
        //        {
        //            _CacheDB = null;
        //            _CacheDbAccessible = null;
        //            _ConnStrCurrent = DiversityWorkbench.Settings.ConnectionString;
        //        }
        //    }
        //}

        //private string _ConnStrCurrent = null;

        //private bool? _CacheDbAccessible = null;
        //private bool CacheDbAccessible()
        //{
        //    this.CheckConnectionChange();

        //    if (_CacheDbAccessible != null) return (bool)_CacheDbAccessible;
        //    try
        //    {
        //        if (this.sourceServerConnection.CacheDB.Length > 0 && this.sourceServerConnection.CacheDBSourceView.Length > 0)
        //        {
        //            if (this.CacheDB.Length == 0)
        //            {
        //                _CacheDbAccessible = false;
        //                return (bool)_CacheDbAccessible;
        //            }
        //            string[] ConStr = DiversityWorkbench.Settings.ConnectionString.Split(new char[] { ';' });
        //            string conStr = "";
        //            foreach (string S in ConStr)
        //            {
        //                if (conStr.Length > 0) conStr += ";";
        //                if (S.StartsWith("Initial"))
        //                {
        //                    conStr += S.Substring(0, S.IndexOf("="));
        //                    conStr += "=" + this._CacheDB;
        //                }
        //                else conStr += S;
        //            }
        //            using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(conStr))
        //            {
        //                string SQL = "SELECT COUNT(*) FROM " + this.sourceServerConnection.CacheDB.Replace("Diversity", "") + "_" + this.sourceServerConnection.CacheDBSourceView;
        //                con.Open();
        //                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //                try
        //                {
        //                    string Result = C.ExecuteScalar().ToString();
        //                    _CacheDbAccessible = true;
        //                }
        //                catch (System.Exception ex) { _CacheDbAccessible = false; }
        //                con.Close();
        //            }
        //            if (_CacheDbAccessible != null)
        //                return (bool)_CacheDbAccessible;
        //            else return false;
        //        }
        //        else
        //            return false;
        //    }
        //    catch { return false; }
        //}

        //private string _CacheDB = null;
        //private string CacheDB
        //{
        //    get
        //    {
        //        //this.CheckConnectionChange();

        //        if (_CacheDB != null) return _CacheDB;

        //        string SQL = "SELECT TOP (1) [DatabaseName] FROM [CacheDatabase 2]";
        //        _CacheDB = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
        //        return _CacheDB;
        //    }
        //}

        #endregion

        #endregion

        #region Autocomplete

        private void AutoComplete_SetMode(System.Windows.Forms.AutoCompleteMode Mode)
        {
            this._autoCompleteMode = Mode;
            this.textBoxValue.AutoCompleteMode = this._autoCompleteMode;
            this.comboBoxLocalValues.Visible = Mode == AutoCompleteMode.None;
        }

        private void ResetAutoComplete()
        {
            this._AutocompleteLinks = null;
            this._autoCompleteStringCollection = null;
        }

        public void setAutocompleteForTextbox(System.Windows.Forms.AutoCompleteMode Mode = System.Windows.Forms.AutoCompleteMode.Suggest)
        {
            this._autoCompleteMode = Mode;
            if (this.textBoxValue.AutoCompleteMode == AutoCompleteMode.None)
            {
                this.textBoxValue.KeyUp += textBoxValueOnDemandAutocomplete_KeyUp;
                this.textBoxValue.Leave += textBoxValueOnDemandAutocomplete_Leave;
                this.textBoxValue.AutoCompleteMode = this._autoCompleteMode;
            }
        }

        private int? _textBoxValuePosition;

        private System.Windows.Forms.AutoCompleteMode _autoCompleteMode = AutoCompleteMode.Suggest;

        private string _Autocomplete_Key = "";
        private void ExecuteAutocompleteLogic(object sender)
        {
            try
            {
                System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)sender;
                // Markus 2.5.23: Setting the cursor to the original position after getting the autocomplete source
                if (textBox.SelectionLength == 0)
                    _textBoxValuePosition = textBox.SelectionStart;
                else _textBoxValuePosition = null;

                if (this.textBoxValue.ReadOnly || this.textBoxValue.Text.Length == 0)
                    return;

                string Content = this.textBoxValue.Text;

                if (this.sourceServerConnection != null)
                {
                    if (this.textBoxValue.AutoCompleteSource != AutoCompleteSource.None && this.textBoxValue.AutoCompleteCustomSource != null)// && this._AutocompleteLinks != null && this._autoCompleteStringCollection != null)
                    {
                        if (_Autocomplete_Key == this.sourceServerConnection.Key())
                        {
                            if (this.textBoxValue.AutoCompleteCustomSource.Count > 0)
                                return;
                        }
                    }
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    this.textBoxValue.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                    this.textBoxValue.AutoCompleteMode = this._autoCompleteMode;
                    if (this.FixedSourceMode == FixSourceMode.Database)
                        this.textBoxValue.AutoCompleteCustomSource = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionOnDemand(this.sourceServerConnection);// this.AutoCompleteStringCollection();
                    else if (this.FixedSourceMode == FixSourceMode.CacheDB)
                    {
                        string Module = this.ServerConnection.ModuleName;
                        this.textBoxValue.AutoCompleteCustomSource = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionOnDemand_ForForCacheDB(this.sourceServerConnection.CacheDB, Module, this.sourceServerConnection.CacheDBSourceView);
                    }
                    this._Autocomplete_Key = this.sourceServerConnection.Key();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
                else
                {
                    this.TextBoxValue_SetAutocompleteOnDemandLocalSource();
                }
                this.textBoxValue.Select(this.textBoxValue.Text.Length, 0);
                if (_textBoxValuePosition != null)
                    textBox.SelectionStart = (int)_textBoxValuePosition;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void TextBoxValue_SetAutocompleteOnDemandLocalSource(bool Requery = false)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                object[] ar = new Object[9] { "", "", "", "", "", "", "", "", "" };
                string Table = "";
                string Column = "";
                string Database = "";
                this.textBoxValue.DataBindings.CopyTo(ar, 0);
                if (ar[0].ToString() != "")
                {
                    System.Windows.Forms.BindingMemberInfo bmi = this.textBoxValue.DataBindings[0].BindingMemberInfo;
                    Column = bmi.BindingField;
                    Table = bmi.BindingPath;
                    if (Table.Length == 0)
                    {
                        System.Windows.Forms.Binding B = (System.Windows.Forms.Binding)ar[0];
                        if (B.DataSource.GetType() == typeof(System.Windows.Forms.BindingSource))
                        {
                            System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)B.DataSource;
                            Table = BS.DataMember.ToString();
                            Column = B.BindingMemberInfo.BindingField;
                            Database = DiversityWorkbench.Settings.DatabaseName;
                        }
                        else if (B.DataSource.GetType().BaseType == typeof(System.Data.DataTable))
                        {
                            Table = B.DataSource.ToString();
                            Column = B.BindingMemberInfo.BindingField;
                            Database = DiversityWorkbench.Settings.DatabaseName;
                        }
                    }
                    if (this._Autocomplete_Key == "Local." + Table + "." + Column && this.textBoxValue.AutoCompleteMode == this._autoCompleteMode && this.textBoxValue.AutoCompleteCustomSource != null)
                        return;
                    if (!DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionExclusions.Contains(Table + "." + Column))
                    {
                        if (Requery)
                            DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionOnDemand_Remove(Table + "." + Column);
                        System.Windows.Forms.AutoCompleteStringCollection autoCompleteStringCollection = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionOnDemand(Table, Column);
                        this.textBoxValue.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                        this.textBoxValue.AutoCompleteMode = this._autoCompleteMode;
                        this.textBoxValue.AutoCompleteCustomSource = autoCompleteStringCollection;
                        this._Autocomplete_Key = "Local." + Table + "." + Column;
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private System.Windows.Forms.Timer _debounceTimer;
        private void textBoxValueOnDemandAutocomplete_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (_debounceTimer == null)
                {
                    _debounceTimer = new System.Windows.Forms.Timer();
                    _debounceTimer.Interval = 300; // Delay in milliseconds
                    _debounceTimer.Tick += (s, args) =>
                    {
                        _debounceTimer.Stop();
                        ExecuteAutocompleteLogic(sender); // Call the autocomplete logic
                    };
                }
                // Restart the debounce timer on every key press
                _debounceTimer.Stop();
                _debounceTimer.Start();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        private void textBoxValueOnDemandAutocomplete_Leave(object sender, EventArgs e)
        {
            try
            {
                // wird unmittelbar nach aufruf der Liste erreicht
                if (this.sourceServerConnection != null)
                {
#if DEBUG
                    string KeyServer = this.sourceServerConnection.Key();
                    string KeyCache = this.sourceServerConnection.KeyCacheDB();
                    string Text = this.textBoxValue.Text;

                    switch (this.FixedSourceMode)
                    {
                        case FixSourceMode.Database:
                            System.Collections.Generic.Dictionary<string, string> AutoCompleteServer = DiversityWorkbench.Forms.FormFunctions.AutoCompleteDict_ForServerConnection(KeyServer);
                            if (AutoCompleteServer.ContainsKey(Text))
                            {
                                this.SetRemoteValues(AutoCompleteServer[Text], Text);//, false);
                            }
                            else if (sourceServerConnection.ModuleName != DiversityWorkbench.Settings.ModuleName)
                            {
                                DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionOnDemand(this.sourceServerConnection);
                                if (DiversityWorkbench.Forms.FormFunctions.AutoCompleteDict_ForServerConnection(this.sourceServerConnection.Key()).ContainsKey(this.textBoxValue.Text))
                                    this.SetRemoteValues(DiversityWorkbench.Forms.FormFunctions.AutoCompleteDict_ForServerConnection(this.sourceServerConnection.Key())[this.textBoxValue.Text], this.textBoxValue.Text);
                                else
                                {
                                    // CacheDB should only be used if explicit selected
                                    //string Key = this.sourceServerConnection.KeyCacheDB();
                                    //if (DiversityWorkbench.Forms.FormFunctions.AutoCompleteDict_ForForCacheDB(Key).ContainsKey(this.textBoxValue.Text))
                                    //    this.SetRemoteValues(DiversityWorkbench.Forms.FormFunctions.AutoCompleteDict_ForForCacheDB(this.sourceServerConnection.KeyCacheDB())[this.textBoxValue.Text], this.textBoxValue.Text);
                                }
                                //this.SetRemoteValues(DiversityWorkbench.Forms.FormFunctions.AutoCompleteDict_ForServerConnection(this.sourceServerConnection.Key())[this.textBoxValue.Text], this.textBoxValue.Text);//, false);
                            }
                            break;
                        case FixSourceMode.CacheDB:
                            if (DiversityWorkbench.Forms.FormFunctions.AutoCompleteDict_ForForCacheDB(KeyCache).ContainsKey(Text))
                            {
                                this.SetRemoteValues(DiversityWorkbench.Forms.FormFunctions.AutoCompleteDict_ForForCacheDB(KeyCache)[Text], Text);
                            }
                            break;
                        default: break;
                    }
#else


                    if (this.FixedSourceMode == FixSourceMode.Database && DiversityWorkbench.Forms.FormFunctions.AutoCompleteDict_ForServerConnection(this.sourceServerConnection.Key()).ContainsKey(this.textBoxValue.Text))
                    {
                        this.SetRemoteValues(DiversityWorkbench.Forms.FormFunctions.AutoCompleteDict_ForServerConnection(this.sourceServerConnection.Key())[this.textBoxValue.Text], this.textBoxValue.Text);//, false);
                    }
                    else if (this.FixedSourceMode == FixSourceMode.Database && sourceServerConnection.ModuleName != DiversityWorkbench.Settings.ModuleName)
                    {
                        DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionOnDemand(this.sourceServerConnection);
                        if(DiversityWorkbench.Forms.FormFunctions.AutoCompleteDict_ForServerConnection(this.sourceServerConnection.Key()).ContainsKey(this.textBoxValue.Text))
                            this.SetRemoteValues(DiversityWorkbench.Forms.FormFunctions.AutoCompleteDict_ForServerConnection(this.sourceServerConnection.Key())[this.textBoxValue.Text], this.textBoxValue.Text);
                        else
                        {
                            // CacheDB should only be used if explicit selected
                            //string Key = this.sourceServerConnection.KeyCacheDB();
                            //if (DiversityWorkbench.Forms.FormFunctions.AutoCompleteDict_ForForCacheDB(Key).ContainsKey(this.textBoxValue.Text))
                            //    this.SetRemoteValues(DiversityWorkbench.Forms.FormFunctions.AutoCompleteDict_ForForCacheDB(this.sourceServerConnection.KeyCacheDB())[this.textBoxValue.Text], this.textBoxValue.Text);
                        }
                        //this.SetRemoteValues(DiversityWorkbench.Forms.FormFunctions.AutoCompleteDict_ForServerConnection(this.sourceServerConnection.Key())[this.textBoxValue.Text], this.textBoxValue.Text);//, false);
                    }
                    else if (this.FixedSourceMode == FixSourceMode.CacheDB)
                    {
                        string KeyCache = this.sourceServerConnection.KeyCacheDB();
                        if (DiversityWorkbench.Forms.FormFunctions.AutoCompleteDict_ForForCacheDB(KeyCache).ContainsKey(this.textBoxValue.Text))
                        {
                            this.SetRemoteValues(DiversityWorkbench.Forms.FormFunctions.AutoCompleteDict_ForForCacheDB(KeyCache)[this.textBoxValue.Text], this.textBoxValue.Text);
                        }
                    }
#endif
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }


        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (this.sourceServerConnection != null)
            {
                if (DiversityWorkbench.Forms.FormFunctions.AutoCompleteDict_ForServerConnection(this.sourceServerConnection.Key()).ContainsKey(this.textBoxValue.Text))
                {
                    this.SetRemoteValues(DiversityWorkbench.Forms.FormFunctions.AutoCompleteDict_ForServerConnection(this.sourceServerConnection.Key())[this.textBoxValue.Text], this.textBoxValue.Text);//, false);
                }
            }
            this.buttonSave.Visible = false;
        }


        private System.Windows.Forms.AutoCompleteStringCollection _autoCompleteStringCollection;
        private System.Collections.Generic.Dictionary<string, string> _AutocompleteLinks;

        private System.Windows.Forms.AutoCompleteStringCollection AutoCompleteStringCollection()
        {
            if (this._autoCompleteStringCollection != null)
                return this._autoCompleteStringCollection;
            this._autoCompleteStringCollection = new AutoCompleteStringCollection();
            this._AutocompleteLinks = new Dictionary<string, string>();
            if (this.SourceServerConnection() != null)
            {
                if (this.SourceServerConnection().ConnectionIsValid)
                {
                    System.Data.DataTable dt = new DataTable();
                    string SQL = "";
                    string Module = this.SourceServerConnection().ModuleName;
                    string Prefix = this.SourceServerConnection().Prefix();
                    switch (Module)
                    {
                        case "DiversityAgents":
                            SQL = "SELECT U.BaseURL + CAST(A.AgentID AS varchar) AS URI, A.AgentName AS DisplayText " +
                                "FROM " + Prefix + "ViewAgentNames AS A INNER JOIN " +
                                Prefix + "AgentProject AS P ON A.AgentID = P.AgentID CROSS JOIN " +
                                Prefix + "ViewBaseURL AS U " +
                                "WHERE(P.ProjectID = " + this.SourceServerConnection().ProjectID.ToString() + ")";
                            break;
                        case "DiversityCollection":
                            SQL = "SELECT U.BaseURL + CAST(C.CollectionSpecimenID AS varchar) AS URI, " +
                                "CASE WHEN C.[AccessionNumber] IS NULL OR  RTRIM(C.[AccessionNumber]) = '' THEN 'ID: ' + CAST(C.[CollectionSpecimenID] AS varchar) ELSE C.[AccessionNumber] END AS DisplayText " +
                                "FROM " + Prefix + "CollectionSpecimen AS C INNER JOIN " +
                                Prefix + "CollectionProject AS P ON C.CollectionSpecimenID = P.CollectionSpecimenID CROSS JOIN " +
                                Prefix + "ViewBaseURL AS U " +
                                "WHERE(P.ProjectID = " + this.SourceServerConnection().ProjectID.ToString() + ")";
                            break;
                        case "DiversityDescriptions":
                            break;
                        case "DiversityExsiccatae":
                            SQL = "SELECT U.BaseURL + CAST(T.[ExsiccataID] AS varchar) AS URI, T.ExsAbbreviation AS DisplayText " +
                                "FROM " + Prefix + "Exsiccata AS T CROSS JOIN " +
                                Prefix + "ViewBaseURL AS U ";
                            break;
                        case "DiversityGazetteer":
                            SQL = "SELECT U.BaseURL + CAST(G.PreferredNameID AS varchar) AS URI, REPLACE(C.HierarchyCountryToPlace, '|', ', ') AS DisplayText " +
                                "FROM " + Prefix + "GeoCache AS C INNER JOIN " +
                                Prefix + "ViewGeoPlace G ON C.PlaceID = G.PlaceID INNER JOIN " +
                                Prefix + "GeoProject AS P ON G.PreferredNameID = P.NameID CROSS JOIN " +
                                Prefix + "ViewBaseURL AS U " +
                                "WHERE(P.ProjectID = " + this.SourceServerConnection().ProjectID.ToString() + ") AND C.HierarchyCountryToPlace <> '' AND C.HierarchyCountryToPlace NOT LIKE '|%'";
                            break;
                        case "DiversityProjects":
                            SQL = "SELECT U.BaseURL + CAST(T.ProjectID AS varchar) AS URI, T.Project AS DisplayText " +
                                "FROM " + Prefix + "Project AS T CROSS JOIN " +
                                Prefix + "ViewBaseURL AS U ";
                            break;
                        case "DiversityReferences":
                            SQL = "SELECT U.BaseURL + CAST(T.RefID AS varchar) AS URI, T.RefDescription_Cache AS DisplayText " +
                                "FROM " + Prefix + "ReferenceTitle AS T INNER JOIN " +
                                Prefix + "ReferenceProject AS P ON T.RefID = P.RefID CROSS JOIN " +
                                Prefix + "ViewBaseURL AS U " +
                                "WHERE(P.ProjectID = " + this.SourceServerConnection().ProjectID.ToString() + ")";
                            break;
                        case "DiversitySamplingPlots":
                            SQL = "SELECT U.BaseURL + CAST(T.PlotID AS varchar) AS URI, T.PlotIdentifier AS DisplayText " +
                                "FROM " + Prefix + "ViewSamplingPlot AS T INNER JOIN " +
                                Prefix + "SamplingProject AS P ON T.PlotID = P.PlotID CROSS JOIN " +
                                Prefix + "ViewBaseURL AS U " +
                                "WHERE(P.ProjectID = " + this.SourceServerConnection().ProjectID.ToString() + ")";
                            break;
                        case "DiversityScientificTerms":
                            SQL = "SELECT U.BaseURL + CAST(T.TermRepresentationID AS varchar) AS URI, T.DisplayText " +
                                "FROM " + Prefix + "TermRepresentation AS T INNER JOIN " +
                                Prefix + "Terminology AS P ON T.TerminologyID = P.TerminologyID CROSS JOIN " +
                                Prefix + "ViewBaseURL AS U " +
                                "WHERE(P.TerminologyID = " + this.SourceServerConnection().ProjectID.ToString() + ")";
                            break;
                        case "DiversityTaxonNames":
                            SQL = "SELECT V.BaseURL + CAST(T.NameID AS varchar) AS URI, T.TaxonNameCache AS DisplayText " +
                                "FROM " + Prefix + "TaxonName AS T INNER JOIN " +
                                Prefix + "TaxonNameProject AS P ON T.NameID = P.NameID CROSS JOIN " +
                                Prefix + "ViewBaseURL AS V " +
                                "WHERE(P.ProjectID = " + this.SourceServerConnection().ProjectID.ToString() + ")";
                            break;
                        default:
                            break;
                    }
                    string conStr = this.SourceServerConnection().ConnectionString;
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, conStr);
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        if (!this._autoCompleteStringCollection.Contains(R["DisplayText"].ToString()))
                        {
                            this._autoCompleteStringCollection.Add(R["DisplayText"].ToString());
                            this._AutocompleteLinks.Add(R["DisplayText"].ToString(), R["URI"].ToString());
                        }
                    }
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }
            return this._autoCompleteStringCollection;
        }

        #endregion

        #region Chart

        private void buttonChart_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._Chart == null)
                    return;

                if (this.DependsOnUri.Length > 0)
                {
                    this.getDependentSource();
                }
                else
                {
                    DiversityWorkbench.Forms.FormRemoteQueryChart f = new Forms.FormRemoteQueryChart(this._Chart);//, this._BaseURL, this._ChartHeader);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK && f.SelectedURL().Length > 0)
                    {
                        int ID;
                        if (int.TryParse(DiversityWorkbench.WorkbenchUnit.getIDFromURI(f.SelectedURL()), out ID))
                        {
                            DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(f.SelectedURL());
                            this._IWorkbenchUnit.setServerConnection(SC);
                            System.Collections.Generic.Dictionary<string, string> DD = this._IWorkbenchUnit.UnitValues(ID);
                            this.setValues(DD);

                            if (false)
                            {
                                System.Data.DataRowView R = (System.Data.DataRowView)this._BindingSource.Current;
                                R.BeginEdit();
                                R[this._ValueColumn] = f.SelectedURL();
                                this.labelURI.Text = f.SelectedURL();
                                string SQL = "";
                                R[this._DisplayColumn] = DD["_DisplayText"];
                                R[this._ValueColumn] = DD["_URI"];
                                foreach (RemoteValueBinding rvb in this._RemoteValueBindings)
                                {
                                    if (DD.ContainsKey(rvb.RemoteParameter))
                                    {
                                        //rvb.
                                    }
                                }
                                switch (SC.ModuleName)
                                {
                                    case "DiversityScientificTerms":
                                        //DiversityWorkbench.ScientificTerm ST = new ScientificTerm(SC);
                                        //DD = ST.UnitValues(f.SelectedURL());
                                        if (DD.ContainsKey("Name") && DD["Name"].Length > 0 &&
                                            DD.ContainsKey("Hierarchy Top-Down") && DD["Hierarchy Top-Down"].Length > 0)
                                        {
                                            string Name = DD["Name"];
                                            string Hierarchy = DD["Hierarchy Top-Down"];

                                            string Table = R.Row.Table.TableName;
                                            switch (Table)
                                            {
                                                case "Identification":
                                                    R["VernacularTerm"] = Name;
                                                    break;
                                                case "CollectionSpecimenPartDescription":
                                                    R[this._DisplayColumn] = Name;
                                                    R["Notes"] = Hierarchy;
                                                    break;
                                                case "CollectionEventProperty":
                                                    R[this._DisplayColumn] = Name;
                                                    R["PropertyHierarchyCache"] = Hierarchy;
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            if (DD.ContainsKey("Name"))
                                            {
                                                R[this._DisplayColumn] = DD["Name"];
                                                if (R.Row.Table.Columns.Contains("PropertyHierarchyCache"))
                                                {
                                                    R["PropertyHierarchyCache"] = DD["Hierarchy"];
                                                    if (DD.ContainsKey("Name"))
                                                        R[this._DisplayColumn] = DD["Name"];
                                                }
                                                else if (R.Row.Table.Columns.Contains("VernacularTerm"))
                                                {
                                                    if (DD["Name"].Length > 0)
                                                    {
                                                        R["VernacularTerm"] = DD["Name"];
                                                    }
                                                    else if (DD.ContainsKey("Hierarchy") && DD["Hierarchy"].Length > 0)
                                                    {
                                                        R["VernacularTerm"] = DD["Hierarchy"];
                                                    }
                                                    else if (DD["_DisplayText"].Length > 0)
                                                    {
                                                        R["VernacularTerm"] = DD["_DisplayText"];
                                                    }
                                                }
                                            }
                                            else if (DD.ContainsKey("HierarchyCache") && DD["HierarchyCache"].Length > 0)
                                            {
                                                R[this._DisplayColumn] = DD["HierarchyCache"];
                                                if (R.Row.Table.Columns.Contains("PropertyHierarchyCache"))
                                                {
                                                    R["PropertyHierarchyCache"] = DD["HierarchyCache"];
                                                    if (DD.ContainsKey("DisplayText"))
                                                        R[this._DisplayColumn] = DD["DisplayText"];
                                                }
                                                else if (R.Row.Table.Columns.Contains("VernacularTerm"))
                                                {
                                                    if (DD["HierarchyCache"].Length > 0)
                                                    {
                                                        R["VernacularTerm"] = DD["HierarchyCache"];
                                                        //this.textBoxValue.Text = DD["HierarchyCache"];
                                                    }
                                                    else if (DD["_DisplayText"].Length > 0)
                                                    {
                                                        R["VernacularTerm"] = DD["_DisplayText"];
                                                        //this.textBoxValue.Text = DD["_DisplayText"];
                                                    }
                                                }
                                            }
                                            else if (DD.ContainsKey("Hierarchy") && DD["Hierarchy"].Length > 0)
                                                R[this._DisplayColumn] = DD["DisplayTextHierarchy"];
                                            else if (DD.ContainsKey("DisplayTextHierarchy") && DD["DisplayTextHierarchy"].Length > 0)
                                                R[this._DisplayColumn] = DD["DisplayTextHierarchy"];
                                            else if (DD.ContainsKey("_DisplayText") && DD["_DisplayText"].Length > 0)
                                                R[this._DisplayColumn] = DD["_DisplayText"];
                                            else if (DD.ContainsKey("DisplayText"))
                                                R[this._DisplayColumn] = DD["DisplayText"];
                                            break;
                                        }
                                        break;
                                    case "DiversityTaxonNames":
                                        DiversityWorkbench.TaxonName TN = new TaxonName(SC);
                                        DD = TN.UnitValues(f.SelectedURL());
                                        if (DD.ContainsKey("Name") && DD["Name"].Length > 0 &&
                                            DD.ContainsKey("Hierarchy Top-Down") && DD["Hierarchy Top-Down"].Length > 0)
                                        {
                                            string Name = DD["Name"];
                                            string Hierarchy = DD["Hierarchy Top-Down"];

                                            string Table = R.Row.Table.TableName;
                                            switch (Table)
                                            {
                                                case "Identification":
                                                    R["VernacularTerm"] = Name;
                                                    break;
                                                case "CollectionSpecimenPartDescription":
                                                    R[this._DisplayColumn] = Name;
                                                    R["Notes"] = Hierarchy;
                                                    break;
                                                case "CollectionEventProperty":
                                                    R[this._DisplayColumn] = Name;
                                                    R["PropertyHierarchyCache"] = Hierarchy;
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            if (DD.ContainsKey("Name"))
                                            {
                                                R[this._DisplayColumn] = DD["Name"];
                                                if (R.Row.Table.Columns.Contains("PropertyHierarchyCache"))
                                                {
                                                    R["PropertyHierarchyCache"] = DD["Hierarchy"];
                                                    if (DD.ContainsKey("Name"))
                                                        R[this._DisplayColumn] = DD["Name"];
                                                }
                                                else if (R.Row.Table.Columns.Contains("VernacularTerm"))
                                                {
                                                    if (DD["Name"].Length > 0)
                                                    {
                                                        R["VernacularTerm"] = DD["Name"];
                                                    }
                                                    else if (DD.ContainsKey("Hierarchy") && DD["Hierarchy"].Length > 0)
                                                    {
                                                        R["VernacularTerm"] = DD["Hierarchy"];
                                                    }
                                                    else if (DD["_DisplayText"].Length > 0)
                                                    {
                                                        R["VernacularTerm"] = DD["_DisplayText"];
                                                    }
                                                }
                                            }
                                            else if (DD.ContainsKey("HierarchyCache") && DD["HierarchyCache"].Length > 0)
                                            {
                                                R[this._DisplayColumn] = DD["HierarchyCache"];
                                                if (R.Row.Table.Columns.Contains("PropertyHierarchyCache"))
                                                {
                                                    R["PropertyHierarchyCache"] = DD["HierarchyCache"];
                                                    if (DD.ContainsKey("DisplayText"))
                                                        R[this._DisplayColumn] = DD["DisplayText"];
                                                }
                                                else if (R.Row.Table.Columns.Contains("VernacularTerm"))
                                                {
                                                    if (DD["HierarchyCache"].Length > 0)
                                                    {
                                                        R["VernacularTerm"] = DD["HierarchyCache"];
                                                        //this.textBoxValue.Text = DD["HierarchyCache"];
                                                    }
                                                    else if (DD["_DisplayText"].Length > 0)
                                                    {
                                                        R["VernacularTerm"] = DD["_DisplayText"];
                                                        //this.textBoxValue.Text = DD["_DisplayText"];
                                                    }
                                                }
                                            }
                                            else if (DD.ContainsKey("Hierarchy") && DD["Hierarchy"].Length > 0)
                                                R[this._DisplayColumn] = DD["DisplayTextHierarchy"];
                                            else if (DD.ContainsKey("DisplayTextHierarchy") && DD["DisplayTextHierarchy"].Length > 0)
                                                R[this._DisplayColumn] = DD["DisplayTextHierarchy"];
                                            else if (DD.ContainsKey("_DisplayText") && DD["_DisplayText"].Length > 0)
                                                R[this._DisplayColumn] = DD["_DisplayText"];
                                            else if (DD.ContainsKey("DisplayText"))
                                                R[this._DisplayColumn] = DD["DisplayText"];
                                            break;
                                        }
                                        break;
                                }
                                R.EndEdit();

                                if (this._RemoteValueBindings != null && this._RemoteValueBindings.Count > 0)
                                {
                                    foreach (DiversityWorkbench.UserControls.RemoteValueBinding RVB in this._RemoteValueBindings)
                                    {
                                        foreach (System.Collections.Generic.KeyValuePair<string, string> P in this._IWorkbenchUnit.UnitValues())
                                        {
                                            if (RVB.RemoteParameter == P.Key)
                                            {
                                                if (RVB.BindingSource != null)
                                                {
                                                    if (RVB.BindingSource.Current != null)
                                                    {
                                                        System.Data.DataRowView RV = (System.Data.DataRowView)RVB.BindingSource.Current;

                                                        ///MW 4.5.2105: enable decision if an allready filled field should be changed
                                                        if (RVB.ModeOfUpdate == RemoteValueBinding.UpdateMode.AskForUpdate)
                                                        {
                                                            if (!RV[RVB.Column].Equals(System.DBNull.Value) &&
                                                                RV[RVB.Column].ToString().Length > 0 &&
                                                                P.Value.Length > 0)
                                                            {
                                                                if (P.Value.Trim() != RV[RVB.Column].ToString().Trim()) ///MW 22.10.2019 - ask only if different
                                                                {
                                                                    string Message = "Should the current value\r\n\r\n" + RV[RVB.Column].ToString() +
                                                                        "\r\n\r\nfor the field " + RVB.Column +
                                                                        "\r\nshould be replaced by\r\n\r\n" + P.Value;
                                                                    if (System.Windows.Forms.MessageBox.Show(Message, "Replace entry", MessageBoxButtons.YesNo) == DialogResult.No)
                                                                        continue;
                                                                }
                                                            }
                                                            else if ((RV[RVB.Column].Equals(System.DBNull.Value) ||
                                                                RV[RVB.Column].ToString().Length == 0) &&
                                                                P.Value.Length > 0)
                                                            {
                                                            }
                                                            else
                                                                continue;
                                                        }
                                                        else if (RVB.ModeOfUpdate == RemoteValueBinding.UpdateMode.OnlyEmpty)
                                                        {
                                                            if (!RV[RVB.Column].Equals(System.DBNull.Value) &&
                                                                RV[RVB.Column].ToString().Length > 0)
                                                                continue;
                                                        }

                                                        RV.BeginEdit();
                                                        try
                                                        {
                                                            if (this._SupressEmptyRemoteValues && P.Value.Length == 0)
                                                            { }
                                                            else
                                                            {
                                                                if (RV.Row.Table.Columns[RVB.Column].DataType.Name == "Double") //MW 22.10.2019 - Double has been inserted missing the '.'
                                                                {
                                                                    Double dd;
                                                                    System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Number;
                                                                    System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CurrentCulture;
                                                                    if (Double.TryParse(P.Value, style, culture, out dd))
                                                                    {
                                                                        RV[RVB.Column] = Double.Parse(dd.ToString(culture));
                                                                    }
                                                                }
                                                                else
                                                                    RV[RVB.Column] = P.Value;
                                                            }
                                                        }
                                                        catch (System.Exception ex)
                                                        {
                                                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                                        }
                                                        RV.EndEdit();
                                                    }
                                                }
                                                else
                                                { }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        this.setControls();
                        //this.setChart(false);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private System.Collections.Generic.Dictionary<string, Chart> _Charts;

        private void setChart(DiversityWorkbench.ServerConnection SC, int Width, int Height)
        {
            try
            {
                if (!DiversityWorkbench.Settings.UseQueryCharts)
                {
                    this.buttonChart.Visible = DiversityWorkbench.Settings.UseQueryCharts;
                    return;
                }
                // init dictionary
                if (this._Charts == null)
                {
                    this._Charts = new Dictionary<string, Chart>();
                }

                // check if dict contains chart with same width
                if (this._Charts.ContainsKey(SC.DisplayTextExtended()) && this._Charts[SC.DisplayTextExtended()].WindowWidth() != Width)
                {
                    this._Charts.Remove(SC.DisplayTextExtended());
                }

                // if chart is missing, get one
                if (!this._Charts.ContainsKey(SC.DisplayTextExtended()))
                {
                    Chart C = null;
                    switch (SC.ModuleName)
                    {
                        case "DiversityScientificTerms":
                            C = DiversityWorkbench.Terminology.GetChart(SC.BaseURL + SC.ProjectID.ToString(), SC.SectionID, Height, Width);//, null, true);
                            break;
                        case "DiversityTaxonNames":
                            C = DiversityWorkbench.TaxonName.GetChart(SC.BaseURL + SC.ProjectID.ToString(), SC.SectionID, Height, Width);//, null, true);
                            break;
                    }
                    if (C != null)
                        this._Charts.Add(SC.DisplayTextExtended(), C);
                }

                // set the chart if available
                if (this._Charts.ContainsKey(SC.DisplayTextExtended()))
                {
                    this._Chart = this._Charts[SC.DisplayTextExtended()];
                    //this.buttonChart.Visible = true;
                }
                //else
                //{
                //    this.buttonChart.Visible = false;
                //}
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        //public void setChart(bool Show = false, bool ResetChart = false)
        //{
        //    if (ResetChart)
        //        this._Chart = null;
        //    if (!this._ChartEnabled)
        //    {
        //        this.buttonChart.Visible = false;
        //    }
        //    else
        //    {
        //        if (this._Chart != null)
        //            this.buttonChart.Visible = Show;
        //        else
        //            this.buttonChart.Visible = false;
        //    }
        //}

        public void EnableChart(bool IsEnabled)
        {
            this._ChartEnabled = IsEnabled;
        }

        //public void EnableChart(Chart Chart, string BaseURL, string Header)
        //{
        //    this._Chart = Chart;
        //    this._BaseURL = BaseURL;
        //    this._ChartHeader = Header;
        //    if (this.labelURI.Text.Length == 0)
        //    {
        //        this.buttonChart.Visible = true;
        //        this._ChartEnabled = true;
        //    }
        //}

        private Chart _Chart;
        //private string _BaseURL;
        //private string _ChartHeader;
        private bool _ChartEnabled = false;

        #endregion

        #region HTML
        private void buttonHtml_Click(object sender, EventArgs e)
        {
            if (_IWorkbenchUnit != null && _IWorkbenchUnit is WorkbenchUnit)
            {
                System.Windows.Forms.Cursor lastCusor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    string URI = "";
                    if (this._BindingSource != null)
                    {
                        System.Data.DataRowView RU = (System.Data.DataRowView)this._BindingSource.Current;
                        if (RU != null)
                        {
                            if (!RU[this._ValueColumn].Equals(System.DBNull.Value))
                                URI = RU[this._ValueColumn].ToString();
                        }
                    }
                    else if (this.labelURI.Text.Trim() != "")
                        URI = this.labelURI.Text;

                    if (URI.Length > 0)
                    {
                        // Get HTLM string and open browser
                        WorkbenchUnit wbu = _IWorkbenchUnit as WorkbenchUnit;
                        string html = wbu.HtmlUnitValues(wbu.UnitValues(URI));
                        //System.IO.FileInfo fi = new System.IO.FileInfo(WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\UnitValues.htm");
                        System.IO.FileInfo fi = new System.IO.FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\DiversityWorkbench\UnitValuesModule.htm");
                        using (System.IO.StreamWriter TxtWriter = new System.IO.StreamWriter(fi.FullName, false, Encoding.Unicode))
                        {
                            TxtWriter.Write(html);
                            TxtWriter.Flush();
                            TxtWriter.Close();
                        }
                        DiversityWorkbench.Forms.FormWebBrowser F = new DiversityWorkbench.Forms.FormWebBrowser(fi.FullName);
                        if (this.ParentForm != null)
                            F.ShowInTaskbar = this.ParentForm.ShowInTaskbar;
                        F.ShowExternal = true;
                        F.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                this.Cursor = lastCusor;
            }
        }
        #endregion

        #region setting the values

        private void setValues(System.Collections.Generic.Dictionary<string, string> Values, string DisplayText = "", string URL = "")
        {
            bool DataPresent = false;
            System.Data.DataRowView R = (System.Data.DataRowView)this._BindingSource.Current;
            R.BeginEdit();
            if (URL.Length > 0)
                R[this._ValueColumn] = URL;
            else if (Values.ContainsKey("_URI"))
                R[this._ValueColumn] = Values["_URI"];

            // restrict to data with URL
            if (R[this._ValueColumn].ToString().Length > 0)
            {
                DataPresent = true;
                if (DisplayText.Length > 0)
                    R[this._DisplayColumn] = DisplayText;
                else if (Values.ContainsKey("_DisplayText"))
                    R[this._DisplayColumn] = Values["_DisplayText"];
            }
            R.EndEdit();

            // do not enter data with missing URL
            if (!DataPresent)
                return;

            // setting the remote values
            if (this._RemoteValueBindings != null && this._RemoteValueBindings.Count > 0)
            {
                foreach (DiversityWorkbench.UserControls.RemoteValueBinding RVB in this._RemoteValueBindings)
                {
                    if (Values.ContainsKey(RVB.RemoteParameter) && RVB.BindingSource != null && RVB.BindingSource.Current != null)
                    {
                        // if empty values should  be ignored
                        if (this._SupressEmptyRemoteValues && (Values[RVB.RemoteParameter] == null || Values[RVB.RemoteParameter].Length == 0))
                            continue;

                        System.Data.DataRowView RV = (System.Data.DataRowView)RVB.BindingSource.Current;
                        switch (RVB.ModeOfUpdate)
                        {
                            case RemoteValueBinding.UpdateMode.AskForUpdate:
                                if (!RV[RVB.Column].Equals(System.DBNull.Value) &&
                                    RV[RVB.Column].ToString().Length > 0 &&
                                    Values[RVB.RemoteParameter].Length > 0)
                                {
                                    /// ask only if different
                                    if (Values[RVB.RemoteParameter].Trim() != RV[RVB.Column].ToString().Trim())
                                    {
                                        string Message = "Should the current value\r\n\r\n" + RV[RVB.Column].ToString() +
                                            "\r\n\r\nfor the field " + RVB.Column +
                                            "\r\nshould be replaced by\r\n\r\n" + Values[RVB.RemoteParameter];
                                        if (System.Windows.Forms.MessageBox.Show(Message, "Replace entry", MessageBoxButtons.YesNo) == DialogResult.No)
                                            continue;
                                    }
                                }
                                break;
                            case RemoteValueBinding.UpdateMode.OnlyEmpty:
                                if (!RV[RVB.Column].Equals(System.DBNull.Value) &&
                                    RV[RVB.Column].ToString().Length > 0)
                                    continue;
                                break;
                            default:
                                break;
                        }
                        RV.BeginEdit();
                        try
                        {
                            System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Number;
                            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CurrentCulture;
                            switch (RV.Row.Table.Columns[RVB.Column].DataType.Name.ToLower())
                            {
                                case "double":
                                    Double DD;
                                    if (Double.TryParse(Values[RVB.RemoteParameter], style, culture, out DD))
                                    {
                                        RV[RVB.Column] = Double.Parse(DD.ToString(culture));
                                    }
                                    break;
                                case "float":
                                    float FF;
                                    if (float.TryParse(Values[RVB.RemoteParameter], style, culture, out FF))
                                    {
                                        RV[RVB.Column] = Double.Parse(FF.ToString(culture));
                                    }
                                    break;
                                case "int":
                                    int II;
                                    if (int.TryParse(Values[RVB.RemoteParameter], style, culture, out II))
                                    {
                                        RV[RVB.Column] = Double.Parse(II.ToString(culture));
                                    }
                                    break;
                                default:
                                    RV[RVB.Column] = Values[RVB.RemoteParameter];
                                    break;
                            }
                        }
                        catch (System.Exception ex) { }
                        RV.EndEdit();
                    }
                    //foreach (System.Collections.Generic.KeyValuePair<string, string> P in this._IWorkbenchUnit.UnitValues())
                    //{
                    //    if (RVB.RemoteParameter == P.Key)
                    //    {
                    //        if (RVB.BindingSource != null)
                    //        {
                    //            if (RVB.BindingSource.Current != null)
                    //            {
                    //                System.Data.DataRowView RV = (System.Data.DataRowView)RVB.BindingSource.Current;

                    //                ///MW 4.5.2105: enable decision if an allready filled field should be changed
                    //                if (RVB.ModeOfUpdate == RemoteValueBinding.UpdateMode.AskForUpdate)
                    //                {
                    //                    if (!RV[RVB.Column].Equals(System.DBNull.Value) &&
                    //                        RV[RVB.Column].ToString().Length > 0 &&
                    //                        P.Value.Length > 0)
                    //                    {
                    //                        if (P.Value.Trim() != RV[RVB.Column].ToString().Trim()) ///MW 22.10.2019 - ask only if different
                    //                        {
                    //                            string Message = "Should the current value\r\n\r\n" + RV[RVB.Column].ToString() +
                    //                                "\r\n\r\nfor the field " + RVB.Column +
                    //                                "\r\nshould be replaced by\r\n\r\n" + P.Value;
                    //                            if (System.Windows.Forms.MessageBox.Show(Message, "Replace entry", MessageBoxButtons.YesNo) == DialogResult.No)
                    //                                continue;
                    //                        }
                    //                    }
                    //                    else if ((RV[RVB.Column].Equals(System.DBNull.Value) ||
                    //                        RV[RVB.Column].ToString().Length == 0) &&
                    //                        P.Value.Length > 0)
                    //                    {
                    //                    }
                    //                    else
                    //                        continue;
                    //                }
                    //                else if (RVB.ModeOfUpdate == RemoteValueBinding.UpdateMode.OnlyEmpty)
                    //                {
                    //                    if (!RV[RVB.Column].Equals(System.DBNull.Value) &&
                    //                        RV[RVB.Column].ToString().Length > 0)
                    //                        continue;
                    //                }

                    //                RV.BeginEdit();
                    //                try
                    //                {
                    //                    if (this._SupressEmptyRemoteValues && P.Value.Length == 0)
                    //                    { }
                    //                    else
                    //                    {
                    //                        if (RV.Row.Table.Columns[RVB.Column].DataType.Name == "Double") //MW 22.10.2019 - Double has been inserted missing the '.'
                    //                        {
                    //                            Double DD;
                    //                            System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Number;
                    //                            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CurrentCulture;
                    //                            if (Double.TryParse(P.Value, style, culture, out DD))
                    //                            {
                    //                                RV[RVB.Column] = Double.Parse(DD.ToString(culture));
                    //                            }
                    //                        }
                    //                        else
                    //                            RV[RVB.Column] = P.Value;
                    //                    }
                    //                }
                    //                catch (System.Exception ex) { }
                    //                RV.EndEdit();
                    //            }
                    //        }
                    //        else
                    //        { }
                    //    }
                    //}
                }
            }
            ////if (f.DisplayText.Length > 0 && f.URI.Length > 0) // Toni 20180821: Take over new text only if values are provided
            ////    this.textBoxValue.Text = f.DisplayText;
        }

        #endregion

    }
}
