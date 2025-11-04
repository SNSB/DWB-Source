using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.UserControls
{
    public partial class UserControl_Specimen : UserControl__Data
    {
        private bool _ForExsiccata = false;

        #region Construction

        public UserControl_Specimen(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            string HelpNamespace,
            bool ForExsiccata = false)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this._ForExsiccata = ForExsiccata;
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        #endregion

        #region Public

        public override void setAvailability()
        {
            base.setAvailability();
            if (!this._iMainForm.ReadOnly())
            {
                // #35
                //this.buttonFindNextAccessionNumber.Enabled = this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE];
                this.FormFunctions.setDataControlEnabled(this.groupBoxAccession, this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE]);
                this.FormFunctions.setDataControlEnabled(this.tableLayoutPanelExsiccataSeries, this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE]);
            }
            // #35
            this.buttonFindNextAccessionNumber.Enabled = this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE];

        }

        public override void SetPosition(int Position)
        {
            base.SetPosition(Position);
            this.CheckDuplicateAccessionNumber();
            //this.setAvailability();
            this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryDepositor, "DepositorsAgentURI");
            this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryExsiccate, "ExsiccataURI");

            this.initAccessionNumberEdit();
        }

        public void ShowExsiccata(bool ShowSpecimen, bool ShowExssicata)
        {
            this.splitContainerMain.Panel1Collapsed = !ShowSpecimen;
            this.splitContainerMain.Panel2Collapsed = !ShowExssicata;
        }

        public System.Windows.Forms.TableLayoutPanel TableLayoutPanelAccession
        {
            get { return this.tableLayoutPanelAccession; }
        }

        public System.Windows.Forms.TableLayoutPanel TableLayoutPanelExsiccataSeries
        {
            get { return this.tableLayoutPanelExsiccataSeries; }
        }

        //public override int DisplayHeight()
        //{
        //    if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowExsiccata)
        //        this._DisplayHeight = 138;
        //    else
        //        this._DisplayHeight = 116;
        //    return base.DisplayHeight();
        //}

        #endregion

        #region Private

        private void initControl()
        {
            try
            {
                this.comboBoxExternalSource.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "ExternalDatasourceID", true));
                this.comboBoxCollectionSpecimenDataWithholdingReason.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "DataWithholdingReason", true));
                this.comboBoxAccessionDateCategory.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "AccessionDateCategory", true));
                this.comboBoxRevisionState.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "LabelTranscriptionState", true));

                DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxCollectionSpecimenDataWithholdingReason);
                DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxExternalSource);

                DiversityCollection.LookupTable.ResetExternalDatasource();
                this.comboBoxExternalSource.DataSource = DiversityCollection.LookupTable.DtExternalDatasource;
                this.comboBoxExternalSource.DisplayMember = "ExternalDatasourceName";
                this.comboBoxExternalSource.ValueMember = "ExternalDatasourceID";
                DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxExternalSource);

                this._EnumComboBoxes = new Dictionary<ComboBox, string>();
                this._EnumComboBoxes.Add(this.comboBoxAccessionDateCategory, "CollDateCategory_Enum");
                this._EnumComboBoxes.Add(this.comboBoxRevisionState, "CollLabelTranscriptionState_Enum");

                this.InitLookupSources();

                this.textBoxExternalSourceID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "ExternalIdentifier", true));
                this.textBoxDepositorsNr.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "DepositorsAccessionNumber", true));
                this.textBoxAccessionNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "AccessionNumber", true));

                this.userControlHierarchySelectorAccessionDateCategory.initHierarchyForEnum("CollDateCategory_Enum", "AccessionDateCategory", this.comboBoxAccessionDateCategory, this._Source);

                // accession date
                this.userControlDatePanelAccessionDate.setDataBindings(this._Source, "AccessionDay", "AccessionMonth", "AccessionYear", "AccessionDateSupplement");

                this.userControlHierarchySelectorAccessionDateCategory.initHierarchyForEnum("CollDateCategory_Enum", "AccessionDateCategory", this.comboBoxAccessionDateCategory, this._Source);

                if (!this._ForExsiccata)
                {
                    DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                    this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryDepositor, A, "CollectionSpecimen", "DepositorsName", "DepositorsAgentURI", this._Source);
                }
                DiversityWorkbench.Exsiccate E = new DiversityWorkbench.Exsiccate(DiversityWorkbench.Settings.ServerConnection);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryExsiccate, E, "CollectionSpecimen", "ExsiccataAbbreviation", "ExsiccataURI", this._Source);

                this.userControlModuleRelatedEntryExsiccate.FixingOfSourceEnabled = true;

                DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

                DiversityWorkbench.Entity.setEntity(this, this.toolTip);

                this.CheckIfClientIsUpToDate();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public override void InitLookupSources() 
        { 
            this.InitEnums();

            DiversityCollection.LookupTable.ResetExternalDatasource();
            this.comboBoxExternalSource.DataSource = DiversityCollection.LookupTable.DtExternalDatasource;
            this.comboBoxExternalSource.DisplayMember = "ExternalDatasourceName";
            this.comboBoxExternalSource.ValueMember = "ExternalDatasourceID";

        }

        private void initAccessionNumberEdit()
        {
            this.textBoxAccessionNumber.ReadOnly = false;
            this.buttonAccessionNumberEdit.Enabled = false;
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                if (!this._iMainForm.ReadOnly() && this.textBoxAccessionNumber.Text.Length > 0 && !R["AccessionNumber"].Equals(System.DBNull.Value) && R["AccessionNumber"].ToString().Length > 0)
                {
                    string AccNr = R["AccessionNumber"].ToString();
                    if (AccessionNumberInCacheDB(AccNr))
                    {
                        this.textBoxAccessionNumber.ReadOnly = true;
                        this.buttonAccessionNumberEdit.Enabled = true;
                        this.toolTip.SetToolTip(this.buttonAccessionNumberEdit, "The accession number is published via the CacheDB and should not be changed");
                    }
                }
                // #85
                if (this._iMainForm.Availability() == CollectionSpecimen.AvailabilityState.ReadOnly)
                    this.textBoxAccessionNumber.ReadOnly = true;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private bool AccessionNumberInCacheDB(string AccessionNumber)
        {
            try
            {
                string SQL = "SELECT PP.Project " +
                    "FROM ProjectProxy AS PP INNER JOIN " +
                    "CollectionProject AS CP ON PP.ProjectID = CP.ProjectID INNER JOIN " +
                    "CollectionSpecimen AS S ON CP.CollectionSpecimenID = S.CollectionSpecimenID " +
                    "WHERE(S.AccessionNumber = N'" + AccessionNumber.Replace("'", "''") + "') ";
                System.Data.DataTable dt = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length == 0)
                    {
                        return false;
                    }
                    SQL = "SELECT count(*) FROM Project_" + R[0].ToString() + ".CacheCollectionSpecimen WHERE (AccessionNumber = N'" + AccessionNumber.Replace("'", "''") + "')";
                    string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                    int i = 0;
                    if (int.TryParse(Result, out i) && i > 0)
                        return true;
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return false;
        }

        private void textBoxAccessionNumber_TextChanged(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            if (!this._iMainForm.ReadOnly() && this.textBoxAccessionNumber.Text.Length > 0 && (R["AccessionNumber"].Equals(System.DBNull.Value) || R["AccessionNumber"].ToString().Length == 0))
                this.buttonFindNextAccessionNumber.Enabled = true;
            else
                this.buttonFindNextAccessionNumber.Enabled = false;
        }

        private void comboBoxCollectionSpecimenDataWithholdingReason_DropDown(object sender, EventArgs e)
        {
            string SQL = "SELECT DISTINCT DataWithholdingReason FROM CollectionSpecimen ORDER BY DataWithholdingReason";
            System.Data.DataTable dt = new DataTable();
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                this.comboBoxCollectionSpecimenDataWithholdingReason.DataSource = dt;
                this.comboBoxCollectionSpecimenDataWithholdingReason.DisplayMember = "DataWithholdingReason";
                this.comboBoxCollectionSpecimenDataWithholdingReason.ValueMember = "DataWithholdingReason";
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonFindNextAccessionNumber_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityCollection.Forms.FormAccessionNumber f = new Forms.FormAccessionNumber(this.textBoxAccessionNumber.Text, true, false);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    if (f.AccessionNumber.Length > 0)
                    {
                        this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0]["AccessionNumber"] = f.AccessionNumber;
                        this.textBoxAccessionNumber.Text = f.AccessionNumber;
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonAccessionNumberEdit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
#if DEBUG
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                string AccNrOri = R["AccessionNumber"].ToString();
                string AccNrNew = "";
                DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("AccessionNumber", "New accession number", AccNrOri, DiversityCollection.Resource.CollectionSpecimen);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK && f.String.Length > 0 && f.String != this.textBoxAccessionNumber.Text && f.String != AccNrOri)
                {
                    AccNrNew = f.String;
                    string Type = "additional accession number";
                    // Check existance of type additional accession number
                    string SQL = "SELECT COUNT(*) FROM ExternalIdentifierType WHERE(Type = N'" + Type + "')";
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Result == "0")
                    {
                        SQL = "SELECT Type FROM ExternalIdentifierType";
                        System.Data.DataTable dt = new DataTable();
                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                        if (dt.Rows.Count > 0)
                        {
                            DiversityWorkbench.Forms.FormGetStringFromList fType = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Please select a type for the original accession number", true);
                            fType.ShowDialog();
                            if (fType.DialogResult == DialogResult.OK && fType.SelectedString.Length > 0)
                                Type = fType.SelectedString;
                            else return;
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("No type for an identifier available");
                            return;
                        }
                    }
                    SQL = "INSERT INTO ExternalIdentifier " +
                        "(ReferencedTable, ReferencedID, Type, Identifier) " +
                        "VALUES('CollectionSpecimen', " + this._iMainForm.ID_Specimen().ToString() + ", '" + Type + "', '" + AccNrOri + "')";
                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                    {
                        this.textBoxAccessionNumber.Text = AccNrNew;
                        R["AccessionNumber"] = AccNrNew;
                        this._iMainForm.setSpecimen();
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
#endif
        }

        private void textBoxAccessionNumber_Leave(object sender, EventArgs e)
        {
            this.CheckDuplicateAccessionNumber();
        }

        private void CheckDuplicateAccessionNumber()
        {
            string CollectionSpecimenID = "x";
            if (this._iMainForm.ProjectID() != 0
                && this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows.Count > 0
                && !this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0]["AccessionNumber"].Equals(System.DBNull.Value))
            {
                // Check within the project
                try
                {
                    string SQL = "SELECT MIN(P.CollectionSpecimenID) AS ID " +
                        " FROM  CollectionProject AS P INNER JOIN " +
                        " CollectionSpecimen AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID " +
                        " WHERE S.AccessionNumber = N'" + this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0]["AccessionNumber"].ToString().Replace("'", "''") + "'" +
                        " AND P.CollectionSpecimenID <> " + this._iMainForm.ID_Specimen().ToString() +
                        " AND LEN(S.AccessionNumber) > 0 ";
                    if (this._iMainForm.ProjectID() != null)
                        SQL += " AND P.ProjectID = " + this._iMainForm.ProjectID().ToString();
                    CollectionSpecimenID = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                }
                catch { }
            }
            int ID;
            if (int.TryParse(CollectionSpecimenID, out ID))
            {
                this.buttonGoToDuplicateAccessionNumber.Tag = ID;
                this.buttonGoToDuplicateAccessionNumber.Visible = true;
                this.textBoxAccessionNumber.BackColor = System.Drawing.Color.Pink;
                this.buttonGoToDuplicateAccessionNumber.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                // Check within the whole database
                try
                {
                    string SQL = "SELECT MIN(S.CollectionSpecimenID) AS ID " +
                        " FROM CollectionSpecimen AS S " +
                        " WHERE S.AccessionNumber = N'" + this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0]["AccessionNumber"].ToString().Replace("'", "''") + "'" +
                        " AND S.CollectionSpecimenID <> " + this._iMainForm.ID_Specimen().ToString() +
                        " AND LEN(S.AccessionNumber) > 0 ";
                    CollectionSpecimenID = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                }
                catch { }
                if (int.TryParse(CollectionSpecimenID, out ID))
                {
                    this.buttonGoToDuplicateAccessionNumber.Tag = ID;
                    this.buttonGoToDuplicateAccessionNumber.Visible = true;
                    this.buttonGoToDuplicateAccessionNumber.ForeColor = System.Drawing.Color.Red;
                    this.textBoxAccessionNumber.BackColor = System.Drawing.Color.Pink;
                }
                else
                {
                    this.buttonGoToDuplicateAccessionNumber.Visible = false;
                    this.buttonGoToDuplicateAccessionNumber.Tag = null;

                    // Check UPDATE
                    bool OK = this.FormFunctions.getObjectPermissions("CollectionSpecimen", "UPDATE");
                    if (OK)
                        this.textBoxAccessionNumber.BackColor = System.Drawing.SystemColors.Window;
                    else
                        this.textBoxAccessionNumber.BackColor = System.Drawing.SystemColors.Control;
                }
            }
        }

        private void buttonGoToDuplicateAccessionNumber_Click(object sender, EventArgs e)
        {
            if (this.buttonGoToDuplicateAccessionNumber.Tag != null)
            {
                int ID;
                if (int.TryParse(this.buttonGoToDuplicateAccessionNumber.Tag.ToString(), out ID))
                {
                    string SQL = "SELECT TOP 1 CollectionSpecimenID " +
                        "FROM CollectionSpecimenID_Available " +
                        "WHERE CollectionSpecimenID = " + ID.ToString();
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Result.Length > 0)
                        this._iMainForm.setSpecimen();
                    else
                    {
                        SQL = "SELECT TOP (1) P.Project " +
                            "FROM CollectionProject AS c INNER JOIN " +
                            "ProjectProxy AS P ON c.ProjectID = P.ProjectID " +
                            "WHERE c.CollectionSpecimenID = " + ID.ToString();
                        string Message = DiversityCollection.Forms.FormCollectionSpecimenText.You_have_no_access_to_this_dataset +
                            "\r\nYou need access to project\r\n" + DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        System.Windows.Forms.MessageBox.Show(Message, "No access", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void buttonTemplateSpecimenSet_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.TemplateForData T = new DiversityWorkbench.TemplateForData("CollectionSpecimen", TemplateSpecimenSuppressedColumns, TemplateSpecimenSourceTables);
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            T.CopyTemplateToRow(R.Row);
        }

        private void buttonTemplateSpecimenEdit_Click(object sender, EventArgs e)
        {
            System.Data.DataRow R = ((System.Data.DataRowView)this._Source.Current).Row;
            DiversityWorkbench.Forms.FormTemplateEditor f = new DiversityWorkbench.Forms.FormTemplateEditor("CollectionSpecimen", R, TemplateSpecimenSuppressedColumns, TemplateSpecimenSourceTables);
            f.setHelp("Template");
            f.ShowDialog();
        }

        private System.Collections.Generic.List<string> TemplateSpecimenSuppressedColumns
        {
            get
            {
                System.Collections.Generic.List<string> Suppress = new List<string>();
                Suppress.Add("CollectionSpecimenID");
                Suppress.Add("CollectionEventID");
                Suppress.Add("Version");
                Suppress.Add("AccessionDate");
                Suppress.Add("Version");
                Suppress.Add("CollectionID");
                Suppress.Add("LogCreatedWhen");
                Suppress.Add("LogCreatedBy");
                Suppress.Add("LogUpdatedWhen");
                Suppress.Add("LogUpdatedBy");
                Suppress.Add("RowGUID");
                return Suppress;
            }
        }

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.TemplateForDataSourceTable> TemplateSpecimenSourceTables
        {
            get
            {
                DiversityWorkbench.TemplateForDataSourceTable ST = new DiversityWorkbench.TemplateForDataSourceTable(LookupTable.DtExternalDatasourceWithNull(), "ExternalDatasourceName", "ExternalDatasourceID");
                System.Collections.Generic.Dictionary<string, DiversityWorkbench.TemplateForDataSourceTable> SourceTables = new Dictionary<string, DiversityWorkbench.TemplateForDataSourceTable>();
                SourceTables.Add("ExternalDatasourceID", ST);
                return SourceTables;
            }
        }

        #endregion

    }
}
