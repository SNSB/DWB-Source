using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DiversityCollection.UserControls
{

    //public struct UserControlSetting
    //{
    //    public string LinkColumn;
    //    public string DisplayColumn;
    //    public string Value;
    //    public UserControlSetting(string LinkColumn, string DisplayColumn)
    //    {
    //        this.LinkColumn = LinkColumn;
    //        this.DisplayColumn = DisplayColumn;
    //        this.Value = "";
    //    }

    //    //public UserControlSetting(string Type, string LinkColumn, string DisplayColumn)
    //    //{
    //    //    this.Type = Type;
    //    //    this.Table = Table;
    //    //    this.LinkColumn = LinkColumn;
    //    //    this.DisplayColumn = DisplayColumn;
    //    //    this.Value = "";
    //    //}
    //    //public UserControlSetting(string Type, string LinkColumn, string DisplayColumn, string Value)
    //    //{
    //    //    this.Type = Type;
    //    //    this.Table = Table;
    //    //    this.LinkColumn = LinkColumn;
    //    //    this.DisplayColumn = DisplayColumn;
    //    //    this.Value = Value;
    //    //}
    //}

    public partial class UserControl__Data : UserControl
    {

        #region Properties and parameter

        protected System.Windows.Forms.BindingSource _Source;
        protected System.Windows.Forms.BindingSource _ParentSource;
        protected string _HelpNamespace;
        protected iMainForm _iMainForm;
        protected DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        protected DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                return this._FormFunctions;
            }
        }

        protected string _TableName;
        public string TableName
        {
            get
            {
                if (this._TableName == null)
                {
                    this._TableName = this._Source.DataMember;
                }
                return _TableName;
            }
            //set { _Table = value; }
        }

        #endregion

        #region set position

        public virtual void SetPosition(int Position = 0)
        {
            this._Source.CurrencyManager.Position = Position;
            this.setAvailability();
        }

        public virtual void SetPositionByID(int ID, int Position = 0)
        {
            this._Source.CurrencyManager.Position = Position;
            this.setAvailability();
        }

        #endregion

        #region Comboboxes

        protected System.Collections.Generic.Dictionary<System.Windows.Forms.ComboBox, string> _EnumComboBoxes;
        protected void InitEnums()
        {
            try
            {
                if (this._EnumComboBoxes != null)
                {
                    foreach (System.Collections.Generic.KeyValuePair<System.Windows.Forms.ComboBox, string> KV in _EnumComboBoxes)
                    {
                        this.initEnumCombobox(KV.Key, KV.Value);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        protected void initEnumCombobox(System.Windows.Forms.ComboBox CB, string EnumTableName)
        {
            try
            {
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString))
                    {
                        con.Open();
                        DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(CB, EnumTableName, con, true, true, true);
                        DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(CB);
                        con.Close();
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        protected System.Collections.Generic.List<System.Windows.Forms.ComboBox> _LookupComboBoxes;
        public virtual void InitLookupSources() { this.InitEnums(); }

        #endregion

        #region Construction

        public UserControl__Data()
        {
            InitializeComponent();
        }

        public UserControl__Data(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            string HelpNamespace)
        {
            InitializeComponent();
            this._iMainForm = MainForm;
            this._Source = Source;
            this._HelpNamespace = HelpNamespace;
            this.toolTip.AutoPopDelay = DiversityWorkbench.Settings.ToolTipAutoPopDelay;
        }
        
        #endregion

        #region Color

        public virtual void SetBackgroundColor(System.Drawing.Color Color)
        {
            foreach (System.Windows.Forms.Control C in this.Controls)
            {
                if (C.GetType() == typeof(System.Windows.Forms.GroupBox))
                    C.BackColor = Color;
                else if (C.GetType() == typeof(System.Windows.Forms.SplitContainer))
                {
                    System.Windows.Forms.SplitContainer SC = (System.Windows.Forms.SplitContainer)C;
                    SC.Panel1.BackColor = Color;
                    SC.Panel2.BackColor = Color;
                }
            }
        }

        #endregion

        #region Availability

        #region Permissions

        private System.Collections.Generic.Dictionary<DiversityWorkbench.Forms.FormFunctions.Permission, bool> _TablePermissions;

        private System.Collections.Generic.Dictionary<DiversityWorkbench.Forms.FormFunctions.Permission, bool> _OnlySelectPermissions;
        private System.Collections.Generic.Dictionary<DiversityWorkbench.Forms.FormFunctions.Permission, bool> OnlySelectPermissions
        {

            get
            {
                if (this._OnlySelectPermissions == null)
                {
                    this._OnlySelectPermissions = new Dictionary<DiversityWorkbench.Forms.FormFunctions.Permission, bool>();
                    _OnlySelectPermissions.Add(DiversityWorkbench.Forms.FormFunctions.Permission.DELETE, false);
                    _OnlySelectPermissions.Add(DiversityWorkbench.Forms.FormFunctions.Permission.INSERT, false);
                    _OnlySelectPermissions.Add(DiversityWorkbench.Forms.FormFunctions.Permission.SELECT, true);
                    _OnlySelectPermissions.Add(DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE, false);
                }
                return _OnlySelectPermissions;
            }
        }

        /// <summary>
        /// The current permissions of the table, dependent on the dataset including the availablity according to a project
        /// </summary>
        /// <returns>Dictionary for the permissions</returns>
        protected System.Collections.Generic.Dictionary<DiversityWorkbench.Forms.FormFunctions.Permission, bool> TablePermissions()
        {
            if (!this._iMainForm.DataAvailable())
            {
                return OnlySelectPermissions;
            }
            else
            {
                if (this._TablePermissions == null)
                    this._TablePermissions = DiversityWorkbench.Forms.FormFunctions.TablePermissions(this.TableName);
                return this._TablePermissions;
            }
        }

        #endregion

        public virtual void setAvailability()
        {
            if (this._iMainForm.ReadOnly())
            {
                this.setReadOnly();
            }
            else
            {
                foreach (System.Windows.Forms.Control C in this.Controls)
                    this.FormFunctions.setDataControlEnabled(C, this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE]);
            }
        }

        //public virtual void setAvailability(bool IsAvailable)
        //{
        //    foreach (System.Windows.Forms.Control C in this.Controls)
        //        C.Enabled = IsAvailable;
        //}

        #endregion

        #region Fixing the source

        protected void setUserControlModuleRelatedEntrySources(
            System.Collections.Generic.List<string> Settings, 
            ref DiversityWorkbench.UserControls.UserControlModuleRelatedEntry UC) //,            int? Width = null, int? Height = null)
        {
            try
            {
                string Source = "";
                string ProjectID = "";
                string Project = "";
                DiversityWorkbench.UserSettings U = new DiversityWorkbench.UserSettings();
                Source = U.GetSetting(Settings);
                if (Source.Length == 0)
                    Source = U.GetSetting(Settings, "Webservice");
                if (Source.Length > 0)
                {
                    UC.FixSource(Source);
                }
                else
                {
                    Source = U.GetSetting(Settings, "Database");
                    if (Source.Length == 0)
                    {
                        UC.ReleaseSource();
                    }
                    else
                    {
                        if (Source.IndexOf("Cache") > -1)
                        {
                            ProjectID = U.GetSetting(Settings, "ProjectID");
                            Project = U.GetSetting(Settings, "Project");
                            string Module = U.GetSetting(Settings, "Module");
                            UC.FixSource(Source, ProjectID, Project);
                        }
                        else
                        {
                            ProjectID = U.GetSetting(Settings, "ProjectID");
                            Project = U.GetSetting(Settings, "Project");
                            string SectionID = U.GetSetting(Settings, "SectionID");
                            string Section = U.GetSetting(Settings, "Section");
                            int iSectionID = -1;
                            int? iiSectionID = null;
                            if (int.TryParse(SectionID, out iSectionID))
                                iiSectionID = iSectionID;
                            iiSectionID = iSectionID;
                            UC.FixSource(Source, ProjectID, Project, iiSectionID, Section, this._iMainForm.FormWidth(), this._iMainForm.FormHeight());
                        }
                    }
                }
                UC.FixingOfSourceEnabled = true;
                UC.buttonFixSource.Click -= this.FixSource_Click; // Markus 28.10.22 - in case this is added several times
                UC.buttonFixSource.Click += new System.EventHandler(this.FixSource_Click);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        protected void FixSource_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Button B = (System.Windows.Forms.Button)sender;
                System.Windows.Forms.Panel P = (System.Windows.Forms.Panel)B.Parent;
                DiversityWorkbench.UserControls.UserControlModuleRelatedEntry UCMRE = (DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)P.Parent;
                DiversityWorkbench.UserSettings U = new DiversityWorkbench.UserSettings();
                System.Collections.Generic.List<string> Setting = new List<string>();
                Setting.Add(DiversityWorkbench.UserSettings.SettingGroups.ModuleSource.ToString());
                Setting.Add(UCMRE.TableName);
                switch (UCMRE.TableName)
                {
                    case "Identification":
                        switch (UCMRE.ValueColumn)
                        {
                            case "TermURI":
                            case "NameURI":
                                Setting.Add("TaxonomicGroup");
                                System.Data.DataRowView R = (System.Data.DataRowView)this._ParentSource.Current;
                                Setting.Add(R["TaxonomicGroup"].ToString());
                                break;
                            default:
                                Setting.Add(UCMRE.ValueColumn);
                                break;
                        }
                        break;
                    case "CollectionSpecimenPartDescription":
                        switch (UCMRE.ValueColumn)
                        {
                            case "DescriptionTermURI":
                                Setting.Add("MaterialCategory");
                                System.Data.DataRowView R = (System.Data.DataRowView)this._ParentSource.Current;
                                Setting.Add(R["MaterialCategory"].ToString());
                                break;
                            default:
                                Setting.Add(UCMRE.ValueColumn);
                                break;
                        }
                        break;
                    case "CollectionEventLocalisation":
                        switch (UCMRE.ValueColumn)
                        {
                            case "Location2":
                                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                                Setting.Add("LocalisationSystemID_" + R["LocalisationSystemID"].ToString());
                                break;
                            default:
                                Setting.Add(UCMRE.ValueColumn);
                                break;
                        }
                        break;
                    case "CollectionEventProperty":
                        switch (UCMRE.ValueColumn)
                        {
                            case "PropertyURI":
                                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                                Setting.Add("PropertyID_" + R["PropertyID"].ToString());
                                break;
                            default:
                                Setting.Add(UCMRE.ValueColumn);
                                break;
                        }
                        break;
                    //case "CollectionEvent":
                    //    switch (UCMRE.ValueColumn)
                    //    {
                    //        case "ReferenceURI":
                    //            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                    //            Setting.Add("PropertyID_" + R["PropertyID"].ToString());
                    //            break;
                    //        default:
                    //            Setting.Add(UCMRE.ValueColumn);
                    //            break;
                    //    }
                    //    break;
                    default:
                        Setting.Add(UCMRE.ValueColumn);
                        break;
                }
                if (!DiversityWorkbench.WorkbenchUnit.SaveSetting(Setting, UCMRE.SourceServerConnection(), UCMRE.SourceWebservice))
                    System.Windows.Forms.MessageBox.Show("Saving of setting failed");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        protected void inituserControlModuleRelatedEntry(ref DiversityWorkbench.UserControls.UserControlModuleRelatedEntry UC, DiversityWorkbench.IWorkbenchUnit iWorkbenchUnit, string TableName, string DisplayColumn, string ValueColumn, System.Windows.Forms.BindingSource BindingSource)
        {
            try
            {
                UC.FixingOfSourceEnabled = true;
                UC.buttonFixSource.Click += new System.EventHandler(this.FixSource_Click);

                UC.bindToData(TableName, DisplayColumn, ValueColumn, this._Source);
                UC.HelpProvider.HelpNamespace = this._HelpNamespace;
                UC.IWorkbenchUnit = iWorkbenchUnit;
                if (iWorkbenchUnit.getServerConnection().ModuleName.ToLower() == "diversityscientificterms")
                    UC.IsListInDatabase = true;

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        protected void inituserControlModuleRelatedEntry(ref DiversityWorkbench.UserControls.UserControlModuleRelatedEntry UC, System.Windows.Forms.BindingSource BindingSource, DiversityWorkbench.IWorkbenchUnit iWorkbenchUnit, string TableName, string DisplayColumn, string ValueColumn)
        {
            try
            {
                UC.FixingOfSourceEnabled = true;
                UC.buttonFixSource.Click += new System.EventHandler(this.FixSource_Click);

                UC.bindToData(TableName, DisplayColumn, ValueColumn, BindingSource);
                UC.HelpProvider.HelpNamespace = this._HelpNamespace;
                UC.IWorkbenchUnit = iWorkbenchUnit;
                if (iWorkbenchUnit.getServerConnection().ModuleName.ToLower() == "diversityscientificterms")
                    UC.IsListInDatabase = true;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        protected void setUserControlSourceFixing(ref DiversityWorkbench.UserControls.UserControlModuleRelatedEntry UC, string ValueColumn)
        {
            System.Collections.Generic.List<string> Settings = new List<string>();
            Settings.Add("ModuleSource");
            Settings.Add(TableName);
            Settings.Add(ValueColumn);
            this.setUserControlModuleRelatedEntrySources(Settings, ref UC);
            //UC.FixingOfSourceEnabled = true;
        }

        //protected void setUserControlSourceFixing(ref DiversityWorkbench.UserControls.UserControlModuleRelatedEntry UC, string ValueGroup, string Value)
        //{
        //    System.Collections.Generic.List<string> Settings = new List<string>();
        //    Settings.Add("ModuleSource");
        //    Settings.Add(TableName);
        //    Settings.Add(ValueGroup);
        //    Settings.Add(Value);
        //    this.setUserControlModuleRelatedEntrySources(Settings, ref UC);
        //}


        //protected void inituserControlModuleRelatedEntry(ref DiversityWorkbench.UserControls.UserControlModuleRelatedEntry UC, DiversityWorkbench.IWorkbenchUnit iWorkbenchUnit, UserControlSetting Setting, System.Windows.Forms.BindingSource BindingSource)
        //{
        //    try
        //    {
        //        UC.bindToData(TableName, Setting.DisplayColumn, Setting.LinkColumn, this._Source);
        //        UC.HelpProvider.HelpNamespace = this._HelpNamespace;
        //        UC.IWorkbenchUnit = iWorkbenchUnit;
        //        if (iWorkbenchUnit.getServerConnection().ModuleName.ToLower() == "diversityscientificterms")
        //            UC.IsListInDatabase = true;

        //        System.Collections.Generic.List<string> Settings = new List<string>();
        //        Settings.Clear();
        //        Settings.Add("ModuleSource");
        //        Settings.Add(TableName);
        //        Settings.Add(Setting.LinkColumn);
        //        this.setUserControlModuleRelatedEntrySources(Settings, ref UC);
        //        UC.FixingOfSourceEnabled = true;
        //        UC.buttonFixSource.Click += new System.EventHandler(this.FixSource_Click);

        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}



        //protected void inituserControlModuleRelatedEntryWithoutSourceFixing(ref DiversityWorkbench.UserControls.UserControlModuleRelatedEntry UC, DiversityWorkbench.IWorkbenchUnit iWorkbenchUnit, string TableName, string DisplayColumn, string ValueColumn, System.Windows.Forms.BindingSource BindingSource)
        //{
        //    try
        //    {
        //        UC.bindToData(TableName, DisplayColumn, ValueColumn, this._Source);
        //        UC.HelpProvider.HelpNamespace = this._HelpNamespace;
        //        UC.IWorkbenchUnit = iWorkbenchUnit;
        //        if (iWorkbenchUnit.getServerConnection().ModuleName.ToLower() == "diversityscientificterms")
        //            UC.IsListInDatabase = true;

        //        //System.Collections.Generic.List<string> Settings = new List<string>();
        //        //Settings.Clear();
        //        //Settings.Add("ModuleSource");
        //        //Settings.Add(TableName);
        //        //Settings.Add(ValueColumn);
        //        //this.setUserControlModuleRelatedEntrySources(Settings, ref UC);
        //        //UC.FixingOfSourceEnabled = true;
        //        //UC.buttonFixSource.Click += new System.EventHandler(this.FixSource_Click);    

        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        #endregion

        #region Disabling if client out of date
        protected void CheckIfClientIsUpToDate()
        {
            if (!this._iMainForm.ClientUpToDate())
            {
                foreach (System.Windows.Forms.Control C in this.Controls)
                {
                    this.DisableControls(C);
                }
            }
        }

        protected virtual void DisableControls(System.Windows.Forms.Control C)
        {
            try
            {
                if (C.Controls.Count > 0)
                {
                    foreach (System.Windows.Forms.Control cc in C.Controls)
                    {
                        cc.Enabled = false;
                        //if (cc.GetType() == typeof(System.Windows.Forms.TextBox))
                        //{
                        //    System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)cc;
                        //    T.ReadOnly = true;
                        //    T.Enabled = false;
                        //}
                        //else if (cc.GetType() == typeof(System.Windows.Forms.ComboBox) ||
                        //    cc.GetType() == typeof(System.Windows.Forms.ListBox) ||
                        //    cc.GetType() == typeof(System.Windows.Forms.CheckBox))
                        //{
                        //    cc.Enabled = false;
                        //}
                        this.DisableControls(cc);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public virtual void setReadOnly(System.Windows.Forms.Control C = null)
        {
            try
            {
                if (C == null && this.Controls.Count > 0)
                {
                    foreach (System.Windows.Forms.Control CC in this.Controls)
                        this.setReadOnly(CC);
                }
                else
                {
                    if (C.Controls.Count > 0)
                    {
                        foreach (System.Windows.Forms.Control cc in C.Controls)
                        {
                            if (cc.GetType() == typeof(System.Windows.Forms.TextBox))
                            {
                                System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)cc;
                                T.ReadOnly = true;
                            }
                            else if (cc.GetType() == typeof(System.Windows.Forms.MaskedTextBox))
                            {
                                System.Windows.Forms.MaskedTextBox MTB = (System.Windows.Forms.MaskedTextBox)cc;
                                MTB.ReadOnly = true;
                            }
                            else if (cc.GetType() == typeof(System.Windows.Forms.DateTimePicker))
                            {
                                System.Windows.Forms.DateTimePicker MTB = (System.Windows.Forms.DateTimePicker)cc;
                                MTB.Enabled = false;
                            }
                            else if (cc.GetType() == typeof(System.Windows.Forms.ToolStrip))
                            {
                                System.Windows.Forms.ToolStrip MTB = (System.Windows.Forms.ToolStrip)cc;
                                MTB.Enabled = false;
                            }
                            else if (cc.GetType() == typeof(System.Windows.Forms.ComboBox) ||
                                cc.GetType() == typeof(System.Windows.Forms.ListBox) ||
                                cc.GetType() == typeof(System.Windows.Forms.CheckBox) ||
                                cc.GetType() == typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry))
                            {
                                cc.Enabled = false;
                            }
                            else
                            {
                                //cc.Enabled = false;
                            }
                            this.setReadOnly(cc);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Data withholding

        protected virtual void SetDataWithholdingControl(System.Windows.Forms.Control C, System.Windows.Forms.PictureBox PB)
        {
            if (C.Text.Trim().Length == 0 && C.Text.Length > 0)
                C.Text = "";
            if (C.Text.Length > 0)
            {
                C.BackColor = System.Drawing.Color.Pink;
                PB.Image = this.imageListDataWithholding.Images[0];
                this.toolTip.SetToolTip(PB, "Data withheld");
            }
            else
            {
                C.BackColor = System.Drawing.Color.White;
                PB.Image = this.imageListDataWithholding.Images[1];
                this.toolTip.SetToolTip(PB, "Data will be published, not withheld");
            }
        }

        #endregion

        #region ToolTip

        #endregion

    }
}
