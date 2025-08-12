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
    public partial class UserControl_Method : UserControl__Data
    {
        #region Parameter

        private System.Windows.Forms.BindingSource _ParentSource;
        private System.Windows.Forms.BindingSource _MethodListSource;
        private System.Windows.Forms.BindingSource _MethodParameterListSource;
        public enum Target { Event, Analysis, Processing }
        private Target _Target = Target.Analysis;
        System.Collections.Generic.Dictionary<string, string> _ParentKey = null;
        private System.Collections.Generic.Dictionary<string, string> ParentKey
        {
            get
            {
                if (this._ParentKey == null)
                {
                    this._ParentKey = new Dictionary<string, string>();
                    switch (this._Target)
                    {
                        case Target.Analysis:
                            this._ParentKey.Add("AnalysisID", "");
                            this._ParentKey.Add("AnalysisNumber", "");
                            this._ParentKey.Add("CollectionSpecimenID", "");
                            this._ParentKey.Add("IdentificationUnitID", "");
                            break;
                        case Target.Event:
                            this._ParentKey.Add("CollectionEventID", "");
                            break;
                        case Target.Processing:
                            this._ParentKey.Add("ProcessingID", "");
                            this._ParentKey.Add("SpecimenProcessingID", "");
                            break;
                    }
                }
                return this._ParentKey;
            }
        }
        
        #endregion

        #region Construction

        public UserControl_Method(
            Target Target,
            iMainForm MainForm,
            System.Windows.Forms.BindingSource ParentSource,
            System.Windows.Forms.BindingSource MethodListSource,
            System.Windows.Forms.BindingSource MethodParameterListSource,
            System.Windows.Forms.BindingSource MethodParameterSource,
            string HelpNamespace)
            : base(MainForm, MethodParameterSource, HelpNamespace)
        {
            InitializeComponent();
            this._Target = Target;
            this._ParentSource = ParentSource;
            this._MethodListSource = MethodListSource;
            this._MethodParameterListSource = MethodParameterListSource;
            this.initControl();
        }

        public UserControl_Method(
            Target Target,
            iMainForm MainForm,
            System.Windows.Forms.BindingSource ParentSource,
            System.Windows.Forms.BindingSource MethodListSource,
            System.Windows.Forms.BindingSource MethodParameterSource,
            string HelpNamespace)
            : base(MainForm, MethodParameterSource, HelpNamespace)
        {
            InitializeComponent();
            this._Target = Target;
            this._ParentSource = ParentSource;
            this._MethodListSource = MethodListSource;
            this.initControl();
        }
        
        #endregion

        private void initControl()
        {
            try
            {
                this.comboBoxMethodParameterValue.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "Value", true));
                // 
                // MethodParameterBindingSource
                // 
                this._Source.DataMember = this._Source.DataMember; // "IdentificationUnitAnalysisMethodParameter";
                this._Source.DataSource = this._iMainForm.DataSetCollectionSpecimen();
                this.textBoxMethodParameterValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Value", true));

                // 
                // MethodListBindingSource
                // 
                this._MethodListSource.DataMember = this._MethodListSource.DataMember;// "IdentificationUnitAnalysisMethodList";
                this._MethodListSource.DataSource = this._iMainForm.DataSetCollectionSpecimen();

                // 
                // listBoxMethod
                // 
                this.listBoxMethod.DataSource = this._MethodListSource;
                this.listBoxMethod.DisplayMember = "DisplayText";
                this.listBoxMethod_SelectedIndexChanged(null, null);

                // 
                // listBoxMethodParameter
                // 
                if (this._MethodParameterListSource != null)
                {
                    this.listBoxMethodParameter.DataSource = this._MethodParameterListSource;
                    this.listBoxMethodParameter.DisplayMember = "DisplayText";
                }
                else
                {
                    this.listBoxMethodParameter.DataSource = this.dvMethodParameter;
                    this.listBoxMethodParameter.DisplayMember = "DisplayText";
                    this.listBoxMethodParameter.ValueMember = "ParameterID";
                }

                DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

                DiversityWorkbench.Entity.setEntity(this, this.toolTip);

                this.CheckIfClientIsUpToDate();
                this.FormFunctions.addEditOnDoubleClickToTextboxes();
                this.FormFunctions.setDescriptions(this);
            }
            catch (System.Exception ex)
            {
            }
        }

        private void toolStripButtonMethodAdd_Click(object sender, EventArgs e)
        {
            if (this._ParentSource.Current == null)
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
                return;
            }
            System.Collections.Generic.List<string> ParentKeyColumns = new List<string>();
            //string ParentIDColumn = "";
            switch (this._Target)
            {
                case Target.Analysis:
                    ParentKeyColumns.Add("CollectionSpecimenID");
                    ParentKeyColumns.Add("IdentificationUnitID");
                    ParentKeyColumns.Add("AnalysisID");
                    ParentKeyColumns.Add("AnalysisNumber");
                    break;
                case Target.Processing:
                    ParentKeyColumns.Add("CollectionSpecimenID");
                    ParentKeyColumns.Add("SpecimenProcessingID");
                    ParentKeyColumns.Add("ProcessingID");
                    break;
                case Target.Event:
                    ParentKeyColumns.Add("CollectionEventID");
                    break;
            }
            int ParentID;
            //int AnalysisID;
            System.Data.DataRowView RA = (System.Data.DataRowView)this._ParentSource.Current;

            System.Collections.Generic.Dictionary<string, string> ParentKeyValues = new Dictionary<string, string>();

            foreach (string C in ParentKeyColumns)
            {
                if (RA[C].Equals(System.DBNull.Value))
                    return;
                else
                    ParentKeyValues.Add(C, RA[C].ToString());
            }
            string ParentRowFilter = "";
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in ParentKeyValues)
            {
                if (ParentRowFilter.Length > 0)
                    ParentRowFilter += " AND ";
                ParentRowFilter += KV.Key + " = '" + KV.Value + "'";
            }
            //if (!int.TryParse(RA["AnalysisID"].ToString(), out AnalysisID))
            //    return;
            //if (!int.TryParse(RA[ParentIDColumn].ToString(), out ParentID))
            //    return;
            //string AnalysisNumber;
            //AnalysisNumber = RA["AnalysisNumber"].ToString();

            string MethodMarker = this.MethodMarker(ParentRowFilter);// "0";
            if (MethodMarker.Length == 0)
                return;
            //System.Data.DataRow[] RR = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethod.Select(ParentIDColumn + " = " + ParentID.ToString() + " AND AnalysisID = " + ParentID.ToString() + " AND AnalysisNumber = '" + AnalysisNumber + "'", "MethodMarker DESC");
            //if (RR.Length > 0)
            //    MethodMarker = RR[0]["MethodMarker"].ToString();
            //int iMethodMarker;
            //if (int.TryParse(MethodMarker, out iMethodMarker))
            //    MethodMarker = (iMethodMarker + 1).ToString();
            //System.Data.DataRow[] RRtest = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethod.Select(ParentIDColumn + " = " + ParentID.ToString() + " AND AnalysisID = " + ParentID.ToString() + " AND AnalysisNumber = '" + AnalysisNumber + "' AND MethodMarker = '" + MethodMarker + "'", "");
            //int i = RRtest.Length;
            //while (i > 0)
            //{
            //    DiversityWorkbench.Forms.FormGetString fMarker = new DiversityWorkbench.Forms.FormGetString("Method marker", "Please enter a new method marker. The marker " + MethodMarker + " is allready present", "");
            //    fMarker.ShowDialog();
            //    if (fMarker.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            //        return;
            //    else if (fMarker.DialogResult == System.Windows.Forms.DialogResult.OK)
            //    {
            //        MethodMarker = fMarker.String;
            //        System.Data.DataRow[] RRt = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethod.Select(ParentIDColumn + " = " + ParentID.ToString() + " AND AnalysisID = " + ParentID.ToString() + " AND AnalysisNumber = '" + AnalysisNumber + "' AND MethodMarker = '" + MethodMarker + "'", "");
            //        i = RRt.Length;
            //    }
            //}
            string ID = "";
            switch (this._Target)
            {
                case Target.Analysis:
                    ID = ParentKeyValues["AnalysisID"];
                    break;
                case Target.Processing:
                    ID = ParentKeyValues["ProcessingID"];
                    break;
                case Target.Event:
                    ID = ParentKeyValues["CollectionEventID"];
                    break;
            }
            System.Data.DataTable dtMethods = this.dtMethodsAvailable(ID);// = new DataTable();
            //string SQL = "SELECT A.MethodID, M.DisplayText " +
            //    " FROM MethodForAnalysis A, Method M " +
            //    " WHERE A.MethodID = M.MethodID AND A.AnalysisID = " + AnalysisID.ToString();
            //SQL += " ORDER BY DisplayText";
            //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //ad.Fill(dtMethods);
            if (dtMethods.Rows.Count == 0)
            {
                string Message = "No methods available";
                System.Windows.Forms.MessageBox.Show(Message);
                return;
            }
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtMethods, "DisplayText", "MethodID", "New method", "Please select the new method for the analysis");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    switch (this._Target)
                    {
                        case Target.Analysis:
                            DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitAnalysisMethodRow RM = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethod.NewIdentificationUnitAnalysisMethodRow();
                            RM.AnalysisID = int.Parse(ParentKeyValues["AnalysisID"]);
                            RM.MethodID = int.Parse(f.SelectedValue);
                            RM.AnalysisNumber = ParentKeyValues["AnalysisNumber"];
                            RM.CollectionSpecimenID = this._iMainForm.ID_Specimen();
                            RM.IdentificationUnitID = int.Parse(ParentKeyValues["IdentificationUnitID"]);
                            RM.MethodMarker = MethodMarker;
                            this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethod.Rows.Add(RM);
                            DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitAnalysisMethodListRow RML = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodList.NewIdentificationUnitAnalysisMethodListRow();
                            RML.AnalysisID = int.Parse(ParentKeyValues["AnalysisID"]);
                            RML.MethodID = int.Parse(f.SelectedValue);
                            RML.AnalysisNumber = ParentKeyValues["AnalysisNumber"];
                            RML.IdentificationUnitID = int.Parse(ParentKeyValues["IdentificationUnitID"]);
                            RML.CollectionSpecimenID = this._iMainForm.ID_Specimen();
                            RML.DisplayText = f.SelectedString + " " + MethodMarker;
                            RML.MethodMarker = MethodMarker;
                            this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodList.Rows.Add(RML);
                            System.Data.DataTable dtAV = new DataTable();
                            string SQL = "SELECT " + this._iMainForm.ID_Specimen().ToString() + ", " +
                                ParentKeyValues["IdentificationUnitID"] + ", " +
                                ParentKeyValues["AnalysisID"] + ", '" + ParentKeyValues["AnalysisNumber"] + "', " +
                                "MethodID, ParameterID, DisplayText, DefaultValue " +
                                "FROM Parameter " +
                                "WHERE MethodID = " + RM.MethodID.ToString();
                            Microsoft.Data.SqlClient.SqlDataAdapter adAV = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                            adAV.Fill(dtAV);
                            foreach (System.Data.DataRow RP in dtAV.Rows)
                            {
                                DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitAnalysisMethodParameterRow RPV = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodParameter.NewIdentificationUnitAnalysisMethodParameterRow();
                                DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitAnalysisMethodParameterListRow RPVL = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodParameterList.NewIdentificationUnitAnalysisMethodParameterListRow();
                                RPV.CollectionSpecimenID = this._iMainForm.ID_Specimen();
                                RPVL.CollectionSpecimenID = this._iMainForm.ID_Specimen();
                                RPV.IdentificationUnitID = int.Parse(ParentKeyValues["IdentificationUnitID"]);
                                RPVL.IdentificationUnitID = int.Parse(ParentKeyValues["IdentificationUnitID"]);
                                RPV.AnalysisID = int.Parse(ParentKeyValues["AnalysisID"]);
                                RPVL.AnalysisID = int.Parse(ParentKeyValues["AnalysisID"]);
                                RPV.AnalysisNumber = ParentKeyValues["AnalysisNumber"];
                                RPVL.AnalysisNumber = ParentKeyValues["AnalysisNumber"];
                                RPV.MethodID = int.Parse(f.SelectedValue);
                                RPVL.MethodID = int.Parse(f.SelectedValue);
                                RPV.MethodMarker = MethodMarker;
                                RPVL.MethodMarker = MethodMarker;
                                RPV.ParameterID = int.Parse(RP["ParameterID"].ToString());
                                RPVL.ParameterID = int.Parse(RP["ParameterID"].ToString());
                                RPV.Value = RP["DefaultValue"].ToString();
                                RPVL.Value = RP["DefaultValue"].ToString();
                                RPVL.DisplayText = RP["DisplayText"].ToString();
                                this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodParameter.Rows.Add(RPV);
                                this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodParameterList.Rows.Add(RPVL);
                            }
                            break;
                        case Target.Processing:
                            DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenProcessingMethodRow PM = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethod.NewCollectionSpecimenProcessingMethodRow();
                            PM.ProcessingID = int.Parse(ParentKeyValues["ProcessingID"]);
                            PM.MethodID = int.Parse(f.SelectedValue);
                            PM.CollectionSpecimenID = this._iMainForm.ID_Specimen();
                            PM.SpecimenProcessingID = int.Parse(ParentKeyValues["SpecimenProcessingID"]);
                            PM.MethodMarker = MethodMarker;
                            this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethod.Rows.Add(PM);
                            DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenProcessingMethodListRow PML = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodList.NewCollectionSpecimenProcessingMethodListRow();
                            PML.ProcessingID = int.Parse(ParentKeyValues["ProcessingID"]);
                            PML.MethodID = int.Parse(f.SelectedValue);
                            PML.SpecimenProcessingID = int.Parse(ParentKeyValues["SpecimenProcessingID"]);
                            PML.CollectionSpecimenID = this._iMainForm.ID_Specimen();
                            PML.DisplayText = f.SelectedString + " " + MethodMarker;
                            PML.MethodMarker = MethodMarker;
                            this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodList.Rows.Add(PML);
                            System.Data.DataTable dtPV = new DataTable();
                            SQL = "SELECT " + this._iMainForm.ID_Specimen().ToString() + ", " +
                                ParentKeyValues["SpecimenProcessingID"] + ", " +
                                ParentKeyValues["ProcessingID"] + ", MethodID, ParameterID, DisplayText, DefaultValue " +
                                "FROM Parameter " +
                                "WHERE MethodID = " + PM.MethodID.ToString();
                            Microsoft.Data.SqlClient.SqlDataAdapter adPV = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                            adPV.Fill(dtPV);
                            foreach (System.Data.DataRow RP in dtPV.Rows)
                            {
                                DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenProcessingMethodParameterRow RPV = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodParameter.NewCollectionSpecimenProcessingMethodParameterRow();// .IdentificationUnitAnalysisMethodParameterRow RPV = this.dataSetCollectionSpecimen.IdentificationUnitAnalysisMethodParameter.NewIdentificationUnitAnalysisMethodParameterRow();
                                DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenProcessingMethodParameterListRow RPVL = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodParameterList.NewCollectionSpecimenProcessingMethodParameterListRow();// .IdentificationUnitAnalysisMethodParameterListRow RPVL = this.dataSetCollectionSpecimen.IdentificationUnitAnalysisMethodParameterList.NewIdentificationUnitAnalysisMethodParameterListRow();
                                RPV.CollectionSpecimenID = this._iMainForm.ID_Specimen();
                                RPVL.CollectionSpecimenID = this._iMainForm.ID_Specimen();
                                RPV.SpecimenProcessingID = int.Parse(ParentKeyValues["SpecimenProcessingID"]);
                                RPVL.SpecimenProcessingID = int.Parse(ParentKeyValues["SpecimenProcessingID"]);
                                RPV.ProcessingID = int.Parse(ParentKeyValues["ProcessingID"]);
                                RPVL.ProcessingID = int.Parse(ParentKeyValues["ProcessingID"]);
                                RPV.MethodID = int.Parse(f.SelectedValue);
                                RPVL.MethodID = int.Parse(f.SelectedValue);
                                RPV.MethodMarker = MethodMarker;
                                RPVL.MethodMarker = MethodMarker;
                                RPV.ParameterID = int.Parse(RP["ParameterID"].ToString());
                                RPVL.ParameterID = int.Parse(RP["ParameterID"].ToString());
                                RPV.Value = RP["DefaultValue"].ToString();
                                RPVL.Value = RP["DefaultValue"].ToString();
                                RPVL.DisplayText = RP["DisplayText"].ToString();
                                this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodParameter.Rows.Add(RPV);
                                this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodParameterList.Rows.Add(RPVL);
                            }

                            break;
                        case Target.Event:
                            DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionEventMethodRow EM = this._iMainForm.DataSetCollectionSpecimen().CollectionEventMethod.NewCollectionEventMethodRow();
                            EM.CollectionEventID = int.Parse(ParentKeyValues["CollectionEventID"]);
                            EM.MethodID = int.Parse(f.SelectedValue);
                            EM.MethodMarker = MethodMarker;
                            this._iMainForm.DataSetCollectionSpecimen().CollectionEventMethod.Rows.Add(EM);

                            DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionEventMethodListRow EML = this._iMainForm.DataSetCollectionSpecimen().CollectionEventMethodList.NewCollectionEventMethodListRow();
                            EML.CollectionEventID = int.Parse(ParentKeyValues["CollectionEventID"]);
                            EML.MethodID = int.Parse(f.SelectedValue);
                            EML.DisplayText = f.SelectedString + " " + MethodMarker;
                            EML.MethodMarker = MethodMarker;
                            this._iMainForm.DataSetCollectionSpecimen().CollectionEventMethodList.Rows.Add(EML);
                            System.Data.DataTable dtEPV = new DataTable();
                            SQL = "SELECT " + ParentKeyValues["CollectionEventID"] + ", " +
                               "MethodID, ParameterID, DisplayText, DefaultValue " +
                               "FROM Parameter " +
                               "WHERE MethodID = " + EM.MethodID.ToString();
                            Microsoft.Data.SqlClient.SqlDataAdapter adEPV = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                            adEPV.Fill(dtEPV);
                            foreach (System.Data.DataRow RP in dtEPV.Rows)
                            {
                                DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionEventParameterValueRow EMPV = this._iMainForm.DataSetCollectionSpecimen().CollectionEventParameterValue.NewCollectionEventParameterValueRow();
                                DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionEventParameterValueListRow EMPVL = this._iMainForm.DataSetCollectionSpecimen().CollectionEventParameterValueList.NewCollectionEventParameterValueListRow();
                                EMPV.CollectionEventID = int.Parse(ParentKeyValues["CollectionEventID"]);
                                EMPVL.CollectionEventID = int.Parse(ParentKeyValues["CollectionEventID"]);
                                EMPV.MethodID = int.Parse(f.SelectedValue);
                                EMPVL.MethodID = int.Parse(f.SelectedValue);
                                EMPV.MethodMarker = MethodMarker;
                                EMPVL.MethodMarker = MethodMarker;
                                EMPV.ParameterID = int.Parse(RP["ParameterID"].ToString());
                                EMPVL.ParameterID = int.Parse(RP["ParameterID"].ToString());
                                EMPV.Value = RP["DefaultValue"].ToString();
                                EMPVL.Value = RP["DefaultValue"].ToString();
                                EMPVL.DisplayText = RP["DisplayText"].ToString();
                                this._iMainForm.DataSetCollectionSpecimen().CollectionEventParameterValue.Rows.Add(EMPV);
                                this._iMainForm.DataSetCollectionSpecimen().CollectionEventParameterValueList.Rows.Add(EMPVL);
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private System.Data.DataTable dtMethodsAvailable(string ID)
        {
            string SQL = "SELECT M.MethodID, M.DisplayText FROM ";
            switch (this._Target)
            {
                case Target.Analysis:
                    SQL += "MethodForAnalysis A,";
                    break;
                case Target.Processing:
                    SQL += "MethodForProcessing P,";
                    break;
                case Target.Event:
                    //SQL += "MethodForAnalysis";
                    break;
            }
            SQL += " Method M ";
                switch(this._Target)
                {
                    case Target.Analysis:
                        SQL += " WHERE A.MethodID = M.MethodID AND A.AnalysisID = " + ID;
                        break;
                    case Target.Processing:
                        SQL += " WHERE P.MethodID = P.MethodID AND P.ProcessingID = " + ID;
                        break;
                    case Target.Event:
                        SQL += " WHERE M.ForCollectionEvent = 1";
                        break;
                }
            SQL += " ORDER BY DisplayText";
            System.Data.DataTable dtMethods = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtMethods);
            return dtMethods;
        }

        private string MethodMarker(string ParentRowFilter)
        {
            string MethodMarker = "0";
            System.Data.DataRow[] RR = null;
            System.Data.DataRow[] RRtest = null;
            switch(this._Target)
            {
                case Target.Analysis:
                    RR = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethod.Select(ParentRowFilter, "MethodMarker DESC");
                    RRtest = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethod.Select(ParentRowFilter + " AND MethodMarker = '" + MethodMarker + "'", "");
                    break;
                case Target.Processing:
                    RR = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethod.Select(ParentRowFilter, "MethodMarker DESC");
                    RRtest = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethod.Select(ParentRowFilter + " AND MethodMarker = '" + MethodMarker + "'", "");
                    break;
                case Target.Event:
                    RR = this._iMainForm.DataSetCollectionSpecimen().CollectionEventMethod.Select(ParentRowFilter, "MethodMarker DESC");
                    RRtest = this._iMainForm.DataSetCollectionSpecimen().CollectionEventMethod.Select(ParentRowFilter + " AND MethodMarker = '" + MethodMarker + "'", "");
                    break;
            }
             
            if (RR.Length > 0)
                MethodMarker = RR[0]["MethodMarker"].ToString();
            int iMethodMarker;
            if (int.TryParse(MethodMarker, out iMethodMarker))
                MethodMarker = (iMethodMarker + 1).ToString();
            int i = RRtest.Length;
            while (i > 0)
            {
                DiversityWorkbench.Forms.FormGetString fMarker = new DiversityWorkbench.Forms.FormGetString("Method marker", "Please enter a new method marker. The marker " + MethodMarker + " is allready present", "");
                fMarker.ShowDialog();
                if (fMarker.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                    return "";
                else if (fMarker.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    MethodMarker = fMarker.String;
                    System.Data.DataRow[] RRt = null;
                    switch (this._Target)
                    {
                        case Target.Analysis:
                            RRt = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethod.Select(ParentRowFilter + " AND MethodMarker = '" + MethodMarker + "'", "");
                            break;
                        case Target.Processing:
                            RRt = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethod.Select(ParentRowFilter + " AND MethodMarker = '" + MethodMarker + "'", "");
                            break;
                        case Target.Event:
                            RRt = this._iMainForm.DataSetCollectionSpecimen().CollectionEventMethod.Select(ParentRowFilter + " AND MethodMarker = '" + MethodMarker + "'", "");
                            break;
                    }
                    i = RRt.Length;
                }
            }
            return MethodMarker;
        }

        //private void toolStripButtonProcessingMethodAdd_Click(object sender, EventArgs e)
        //{
        //    if (this.collectionSpecimenProcessingBindingSource.Current == null) //.identificationUnitAnalysisBindingSource.Current == null)
        //    {
        //        System.Windows.Forms.MessageBox.Show("No processing selected");
        //        return;
        //    }
        //    int SpecimenProcessingID;
        //    int ProcessingID;
        //    System.Data.DataRowView RA = (System.Data.DataRowView)this.collectionSpecimenProcessingBindingSource.Current;
        //    if (!int.TryParse(RA["ProcessingID"].ToString(), out ProcessingID))
        //        return;
        //    if (!int.TryParse(RA["SpecimenProcessingID"].ToString(), out SpecimenProcessingID))
        //        return;

        //    string SQL = "SELECT A.MethodID, M.DisplayText " +
        //        " FROM MethodForProcessing A, Method M " +
        //        " WHERE A.MethodID = M.MethodID AND A.ProcessingID = " + ProcessingID.ToString();
        //    SQL += " ORDER BY DisplayText";
        //    System.Data.DataTable dtMethods = new DataTable();
        //    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //    ad.Fill(dtMethods);
        //    if (dtMethods.Rows.Count == 0)
        //    {
        //        string Message = "No methods available";
        //        System.Windows.Forms.MessageBox.Show(Message);
        //        return;
        //    }

        //    string MethodMarker = "0";
        //    System.Data.DataRow[] RR = this.dataSetCollectionSpecimen.CollectionSpecimenProcessingMethod.Select("SpecimenProcessingID = " + SpecimenProcessingID.ToString(), "MethodMarker DESC");
        //    if (RR.Length > 0)
        //        MethodMarker = RR[0]["MethodMarker"].ToString();
        //    int iMethodMarker;
        //    if (int.TryParse(MethodMarker, out iMethodMarker))
        //        MethodMarker = (iMethodMarker + 1).ToString();
        //    System.Data.DataRow[] RRtest = this.dataSetCollectionSpecimen.CollectionSpecimenProcessingMethod.Select("SpecimenProcessingID = " + SpecimenProcessingID.ToString() + " AND MethodMarker = '" + MethodMarker + "'", "");
        //    int i = RRtest.Length;
        //    while (i > 0)
        //    {
        //        DiversityWorkbench.Forms.FormGetString fMarker = new DiversityWorkbench.Forms.FormGetString("Method marker", "Please enter a new method marker. The marker " + MethodMarker + " is allready present", "");
        //        fMarker.ShowDialog();
        //        if (fMarker.DialogResult == System.Windows.Forms.DialogResult.Cancel)
        //            return;
        //        else if (fMarker.DialogResult == System.Windows.Forms.DialogResult.OK)
        //        {
        //            MethodMarker = fMarker.String;
        //            System.Data.DataRow[] RRt = this.dataSetCollectionSpecimen.CollectionSpecimenProcessingMethod.Select("SpecimenProcessingID = " + SpecimenProcessingID.ToString() + " AND MethodMarker = '" + MethodMarker + "'", "");
        //            i = RRt.Length;
        //        }
        //    }

        //    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtMethods, "DisplayText", "MethodID", "New method", "Please select the new method for the processing");
        //    f.ShowDialog();
        //    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
        //    {
        //        try
        //        {
        //            DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenProcessingMethodRow RM = this.dataSetCollectionSpecimen.CollectionSpecimenProcessingMethod.NewCollectionSpecimenProcessingMethodRow();//.NewIdentificationUnitAnalysisMethodRow();
        //            RM.ProcessingID = ProcessingID;
        //            RM.MethodID = int.Parse(f.SelectedValue);
        //            RM.CollectionSpecimenID = this.ID;
        //            RM.SpecimenProcessingID = SpecimenProcessingID;
        //            RM.MethodMarker = MethodMarker;
        //            this.dataSetCollectionSpecimen.CollectionSpecimenProcessingMethod.Rows.Add(RM);//.IdentificationUnitAnalysisMethod.Rows.Add(RM);
        //            DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenProcessingMethodListRow RML = this.dataSetCollectionSpecimen.CollectionSpecimenProcessingMethodList.NewCollectionSpecimenProcessingMethodListRow();//  .IdentificationUnitAnalysisMethodListRow RML = this.dataSetCollectionSpecimen.IdentificationUnitAnalysisMethodList.NewIdentificationUnitAnalysisMethodListRow();
        //            RML.ProcessingID = ProcessingID;
        //            RML.MethodID = int.Parse(f.SelectedValue);
        //            RML.SpecimenProcessingID = SpecimenProcessingID;
        //            RML.CollectionSpecimenID = this.ID;
        //            RML.DisplayText = f.SelectedString + " " + MethodMarker;
        //            RML.MethodMarker = MethodMarker;
        //            this.dataSetCollectionSpecimen.CollectionSpecimenProcessingMethodList.Rows.Add(RML);
        //            System.Data.DataTable dtPV = new DataTable();
        //            SQL = "SELECT " + this.ID.ToString() + ", " + SpecimenProcessingID.ToString() + ", " + ProcessingID.ToString() + ", MethodID, ParameterID, DisplayText, DefaultValue " +
        //                "FROM Parameter " +
        //                "WHERE MethodID = " + RM.MethodID.ToString();
        //            Microsoft.Data.SqlClient.SqlDataAdapter adPV = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //            adPV.Fill(dtPV);
        //            foreach (System.Data.DataRow RP in dtPV.Rows)
        //            {
        //                DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenProcessingMethodParameterRow RPV = this.dataSetCollectionSpecimen.CollectionSpecimenProcessingMethodParameter.NewCollectionSpecimenProcessingMethodParameterRow();// .IdentificationUnitAnalysisMethodParameterRow RPV = this.dataSetCollectionSpecimen.IdentificationUnitAnalysisMethodParameter.NewIdentificationUnitAnalysisMethodParameterRow();
        //                DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenProcessingMethodParameterListRow RPVL = this.dataSetCollectionSpecimen.CollectionSpecimenProcessingMethodParameterList.NewCollectionSpecimenProcessingMethodParameterListRow();// .IdentificationUnitAnalysisMethodParameterListRow RPVL = this.dataSetCollectionSpecimen.IdentificationUnitAnalysisMethodParameterList.NewIdentificationUnitAnalysisMethodParameterListRow();
        //                RPV.CollectionSpecimenID = this.ID;
        //                RPVL.CollectionSpecimenID = this.ID;
        //                RPV.SpecimenProcessingID = SpecimenProcessingID;
        //                RPVL.SpecimenProcessingID = SpecimenProcessingID;
        //                RPV.ProcessingID = ProcessingID;
        //                RPVL.ProcessingID = ProcessingID;
        //                RPV.MethodID = int.Parse(f.SelectedValue);
        //                RPVL.MethodID = int.Parse(f.SelectedValue);
        //                RPV.MethodMarker = MethodMarker;
        //                RPVL.MethodMarker = MethodMarker;
        //                RPV.ParameterID = int.Parse(RP["ParameterID"].ToString());
        //                RPVL.ParameterID = int.Parse(RP["ParameterID"].ToString());
        //                RPV.Value = RP["DefaultValue"].ToString();
        //                RPVL.Value = RP["DefaultValue"].ToString();
        //                RPVL.DisplayText = RP["DisplayText"].ToString();
        //                this.dataSetCollectionSpecimen.CollectionSpecimenProcessingMethodParameter.Rows.Add(RPV);// .IdentificationUnitAnalysisMethodParameter.Rows.Add(RPV);
        //                this.dataSetCollectionSpecimen.CollectionSpecimenProcessingMethodParameterList.Rows.Add(RPVL);// .IdentificationUnitAnalysisMethodParameterList.Rows.Add(RPVL);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        }
        //    }
        //}

        //private void toolStripButtonProcessingMethodDelete_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProcessingMethods.SelectedItem;// .listBoxAnalysisMethod.SelectedItem;
        //        string Restriction = "MethodID = " + R["MethodID"].ToString() +
        //            " AND CollectionSpecimenID = " + R["CollectionSpecimenID"].ToString() +
        //            " AND SpecimenProcessingID = " + R["SpecimenProcessingID"].ToString() +
        //            " AND ProcessingID = " + R["ProcessingID"].ToString() +
        //            " AND MethodMarker = '" + R["MethodMarker"].ToString() + "'";
        //        System.Data.DataRow[] rrP = this.dataSetCollectionSpecimen.CollectionSpecimenProcessingMethodParameter.Select(Restriction);// .IdentificationUnitAnalysisMethodParameter.Select(Restriction);
        //        System.Data.DataRow[] rrPL = this.dataSetCollectionSpecimen.CollectionSpecimenProcessingMethodParameterList.Select(Restriction);// .IdentificationUnitAnalysisMethodParameterList.Select(Restriction);
        //        for (int i = 0; i < rrP.Length; i++)
        //            rrP[i].Delete();
        //        for (int i = 0; i < rrPL.Length; i++)
        //            rrPL[i].Delete();
        //        System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionSpecimenProcessingMethod.Select(Restriction);// .IdentificationUnitAnalysisMethod.Select(Restriction);
        //        System.Data.DataRow[] rrL = this.dataSetCollectionSpecimen.CollectionSpecimenProcessingMethodList.Select(Restriction);// .IdentificationUnitAnalysisMethodList.Select(Restriction);
        //        rr[0].Delete();
        //        rrL[0].Delete();
        //        this.updateSpecimen();
        //        this.setSpecimen(this.SpecimenID);
        //    }
        //    catch (System.Exception ex) { }
        //}
        

        
        private string ParentKeyRestriction(System.Data.DataRowView R)
        {
            string Restriction = "";
            System.Collections.Generic.Dictionary<string, string> Key = new Dictionary<string, string>();
            bool NewKey = false;
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ParentKey)
            {
                if (!R.Row.Table.Columns.Contains(KV.Key) || R[KV.Key].Equals(System.DBNull.Value))
                {
                    return "";
                }
                else
                {
                    Key.Add(KV.Key, R[KV.Key].ToString());
                    if (Restriction.Length > 0)
                        Restriction += " AND ";
                    Restriction += KV.Key + " = '" + R[KV.Key].ToString() + "'";
                    if (R[KV.Key].ToString() != KV.Value)
                        NewKey = true;
                }
            }
            if (NewKey)
                this._ParentKey = Key;
            return Restriction;
        }

        private void listBoxMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.listBoxMethodParameter.DataSource = this.dvMethodParameter;
            this.listBoxMethodParameter_SelectedIndexChanged(null, null);
            try
            {
                if (this.listBoxMethod.SelectedItem != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxMethod.SelectedItem;
                    this.toolStripTextBoxMethodMarker.Text = R["MethodMarker"].ToString();
                }
                else this.toolStripTextBoxMethodMarker.Text = "";
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonMethodDelete_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxMethod.SelectedItem;
                string Restriction = this.ParentKeyRestriction(R);
                if (Restriction.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("deleting failed");
                }
                else
                {
                    Restriction += " AND MethodID = " + R["MethodID"].ToString() +
                    " AND MethodMarker = '" + R["MethodMarker"].ToString() + "'";
                    switch(this._Target)
                    {
                        case Target.Analysis:
                            System.Data.DataRow[] rrAP = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodParameter.Select(Restriction);
                            System.Data.DataRow[] rrAPL = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodParameterList.Select(Restriction);
                            for (int i = 0; i < rrAP.Length; i++)
                                rrAP[i].Delete();
                            for (int i = 0; i < rrAPL.Length; i++)
                                rrAPL[i].Delete();
                            System.Data.DataRow[] Arr = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethod.Select(Restriction);
                            System.Data.DataRow[] ArrL = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodList.Select(Restriction);
                            Arr[0].Delete();
                            ArrL[0].Delete();
                            break;
                        case Target.Event:
                            System.Data.DataRow[] rrEP = this._iMainForm.DataSetCollectionSpecimen().CollectionEventParameterValue.Select(Restriction);
                            System.Data.DataRow[] rrEPL = this._iMainForm.DataSetCollectionSpecimen().CollectionEventParameterValueList.Select(Restriction);
                            for (int i = 0; i < rrEP.Length; i++)
                                rrEP[i].Delete();
                            for (int i = 0; i < rrEPL.Length; i++)
                                rrEPL[i].Delete();
                            System.Data.DataRow[] Err = this._iMainForm.DataSetCollectionSpecimen().CollectionEventMethod.Select(Restriction);
                            System.Data.DataRow[] ErrL = this._iMainForm.DataSetCollectionSpecimen().CollectionEventMethodList.Select(Restriction);
                            Err[0].Delete();
                            ErrL[0].Delete();
                            break;
                        case Target.Processing:
                            System.Data.DataRow[] rrPP = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodParameter.Select(Restriction);
                            System.Data.DataRow[] rrPPL = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodParameterList.Select(Restriction);
                            for (int i = 0; i < rrPP.Length; i++)
                                rrPP[i].Delete();
                            for (int i = 0; i < rrPPL.Length; i++)
                                rrPPL[i].Delete();
                            System.Data.DataRow[] Prr = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethod.Select(Restriction);
                            System.Data.DataRow[] PrrL = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodList.Select(Restriction);
                            Prr[0].Delete();
                            PrrL[0].Delete();
                            break;
                }
                    this._iMainForm.saveSpecimen(); //this.updateSpecimen();
                    this._iMainForm.setSpecimen();
                }
            }
            catch (System.Exception ex) { }

        }

        private void buttonMethodParameterRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxMethodParameter.SelectedItem != null)
                {
                    System.Data.DataRowView Rlist = (System.Data.DataRowView)this.listBoxMethodParameter.SelectedItem;
                    string Message = "Do you want to remove parameter\r\n" + Rlist["DisplayText"].ToString();
                    if (System.Windows.Forms.MessageBox.Show(Message, "Remove parameter?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        int i = this.listBoxMethod.SelectedIndex;
                        System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                        R.Delete();
                        this._iMainForm.saveSpecimen();
                        this.listBoxMethod.SelectedIndex = i;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void listBoxMethodParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxMethodParameter.DataSource != null &&
                this.listBoxMethodParameter.DataSource.GetType() == typeof(System.Data.DataView) &&
                this.listBoxMethodParameter.SelectedIndex == -1)
            {
                if (this.dvMethodParameter.Count > 0)
                {
                }
            }
            if (this.listBoxMethodParameter.SelectedItem == null)
            {
                this.comboBoxMethodParameterValue.Visible = false;
                this.textBoxMethodParameterValue.Visible = false;
                return;
            }
            System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxMethodParameter.SelectedItem;
            switch (this._Target)
            {
                case Target.Analysis:
                    for (int i = 0; i < this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodParameter.Rows.Count; i++)
                    {
                        if (this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodParameter.Rows[i]["ParameterID"].ToString() == RV["ParameterID"].ToString() &&
                            this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodParameter.Rows[i]["MethodID"].ToString() == RV["MethodID"].ToString() &&
                            this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodParameter.Rows[i]["IdentificationUnitID"].ToString() == RV["IdentificationUnitID"].ToString() &&
                            this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodParameter.Rows[i]["AnalysisID"].ToString() == RV["AnalysisID"].ToString() &&
                            this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodParameter.Rows[i]["AnalysisNumber"].ToString() == RV["AnalysisNumber"].ToString() &&
                            this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodParameter.Rows[i]["MethodMarker"].ToString() == RV["MethodMarker"].ToString())
                        {
                            this._Source.Position = i;
                            break;
                        }
                    }
                    break;
                case Target.Event:
                    for (int i = 0; i < this._iMainForm.DataSetCollectionSpecimen().CollectionEventParameterValue.Count; i++)
                    {
                        if (this._iMainForm.DataSetCollectionSpecimen().CollectionEventParameterValue.Rows[i]["ParameterID"].ToString() == RV["ParameterID"].ToString() &&
                            this._iMainForm.DataSetCollectionSpecimen().CollectionEventParameterValue.Rows[i]["MethodID"].ToString() == RV["MethodID"].ToString() &&
                            this._iMainForm.DataSetCollectionSpecimen().CollectionEventParameterValue.Rows[i]["CollectionEventID"].ToString() == RV["CollectionEventID"].ToString() &&
                            this._iMainForm.DataSetCollectionSpecimen().CollectionEventParameterValue.Rows[i]["MethodMarker"].ToString() == RV["MethodMarker"].ToString())
                        {
                            this._Source.Position = i;
                            break;
                        }
                    }
                    break;
                case Target.Processing:
                    for (int i = 0; i < this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodParameter.Rows.Count; i++)
                    {
                        if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodParameter.Rows[i]["ParameterID"].ToString() == RV["ParameterID"].ToString() &&
                            this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodParameter.Rows[i]["MethodID"].ToString() == RV["MethodID"].ToString() &&
                            this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodParameter.Rows[i]["SpecimenProcessingID"].ToString() == RV["SpecimenProcessingID"].ToString() &&
                            this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodParameter.Rows[i]["CollectionSpecimenID"].ToString() == RV["CollectionSpecimenID"].ToString() &&
                            this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodParameter.Rows[i]["MethodMarker"].ToString() == RV["MethodMarker"].ToString())
                        {
                            this._Source.Position = i;
                            break;
                        }
                    }
                    break;
            }

            if (this._Source.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                string SQL = "SELECT Value, DisplayText " +
                    "FROM ParameterValue_Enum " +
                    "WHERE (MethodID = " + R["MethodID"].ToString() + ") AND (ParameterID = " + R["ParameterID"].ToString() + ") ORDER BY DisplayText";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    this.textBoxMethodParameterValue.Visible = false;
                    this.textBoxMethodParameterValue.Dock = DockStyle.None;
                    this.comboBoxMethodParameterValue.Visible = true;
                    this.comboBoxMethodParameterValue.Dock = DockStyle.Fill;
                    this.comboBoxMethodParameterValue.DataSource = dt;
                    this.comboBoxMethodParameterValue.DisplayMember = "DisplayText";
                    this.comboBoxMethodParameterValue.ValueMember = "Value";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (R["Value"].ToString() == dt.Rows[i]["Value"].ToString())
                        {
                            this.comboBoxMethodParameterValue.SelectedIndex = i;
                            break;
                        }
                    }
                }
                else
                {
                    this.comboBoxMethodParameterValue.Visible = false;
                    this.comboBoxMethodParameterValue.Dock = DockStyle.None;
                    this.textBoxMethodParameterValue.Visible = true;
                    this.textBoxMethodParameterValue.Dock = DockStyle.Fill;

                }
            }
            else
            {
                this.textBoxMethodParameterValue.Visible = false;
                this.comboBoxMethodParameterValue.Visible = false;
            }
        }

        private void buttonMethodParameterAddMissing_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxMethod.SelectedItem != null)//this._MethodListSource.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxMethod.SelectedItem;
                    string SQL = "SELECT MethodID, ParameterID, DisplayText, DefaultValue " +
                        "FROM Parameter AS P " +
                        "WHERE (MethodID = " + R["MethodID"].ToString() + ") AND (ParameterID NOT IN ";
                    switch (this._Target)
                    {
                        case Target.Analysis:
                            SQL += "(SELECT ParameterID " +
                            "FROM IdentificationUnitAnalysisMethodParameter AS U " +
                            "WHERE (IdentificationUnitID = " + R["IdentificationUnitID"].ToString() + ") " +
                            "AND (AnalysisID = " + R["AnalysisID"].ToString() + ") " +
                            "AND (AnalysisNumber = '" + R["AnalysisNumber"].ToString() + "') " +
                            "AND (MethodID = " + R["MethodID"].ToString() + ") " +
                            "AND (MethodMarker = '" + R["MethodMarker"].ToString() + "')))";
                            break;
                        case Target.Processing:
                            SQL += "(SELECT ParameterID " +
                            "FROM CollectionSpecimenProcessingMethodParameter AS U " +
                            "WHERE (SpecimenProcessingID = " + R["SpecimenProcessingID"].ToString() + ") " +
                            "AND (ProcessingID = " + R["ProcessingID"].ToString() + ") " +
                            "AND (MethodID = " + R["MethodID"].ToString() + ") " +
                            "AND (MethodMarker = '" + R["MethodMarker"].ToString() + "')))";
                            break;
                        case Target.Event:
                            SQL += "(SELECT ParameterID " +
                            "FROM CollectionEventParameterValue AS U " +
                            "WHERE (CollectionEventID = " + R["CollectionEventID"].ToString() + ") " +
                            "AND (MethodID = " + R["MethodID"].ToString() + ") " +
                            "AND (MethodMarker = '" + R["MethodMarker"].ToString() + "')))";
                            break;
                    }
                    System.Data.DataTable dt = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        System.Collections.Generic.Dictionary<string, bool> Dict = new Dictionary<string, bool>();
                        foreach (System.Data.DataRow RP in dt.Rows)
                            Dict.Add(RP["DisplayText"].ToString(), false);
                        DiversityWorkbench.Forms.FormGetMultiFromList f = new DiversityWorkbench.Forms.FormGetMultiFromList("Missing parameter", "Please select the parameters that should be added", Dict);
                        f.ShowDialog();
                        if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            switch (this._Target)
                            {
                                case Target.Analysis:
                                    int UnitID = int.Parse(R["IdentificationUnitID"].ToString());
                                    int AnalysisID = int.Parse(R["AnalysisID"].ToString());
                                    string AnalysisNumber = R["AnalysisNumber"].ToString();
                                    int MethodID = int.Parse(R["MethodID"].ToString());
                                    string MethodMarker = R["MethodMarker"].ToString();
                                    foreach (System.Collections.Generic.KeyValuePair<string, bool> KV in f.Items)
                                    {
                                        if (KV.Value)
                                        {
                                            System.Data.DataRow[] rr = dt.Select("DisplayText = '" + KV.Key + "'");
                                            if (rr.Length > 0)
                                            {
                                                int ParameterID = int.Parse(rr[0]["ParameterID"].ToString());
                                                string DefaultValue = rr[0]["DefaultValue"].ToString();
                                                DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitAnalysisMethodParameterRow RPV = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodParameter.NewIdentificationUnitAnalysisMethodParameterRow();
                                                DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitAnalysisMethodParameterListRow RPVL = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodParameterList.NewIdentificationUnitAnalysisMethodParameterListRow();
                                                RPV.CollectionSpecimenID = this._iMainForm.ID_Specimen();
                                                RPVL.CollectionSpecimenID = this._iMainForm.ID_Specimen();
                                                RPV.IdentificationUnitID = UnitID;
                                                RPVL.IdentificationUnitID = UnitID;
                                                RPV.AnalysisID = AnalysisID;
                                                RPVL.AnalysisID = AnalysisID;
                                                RPV.AnalysisNumber = AnalysisNumber;
                                                RPVL.AnalysisNumber = AnalysisNumber;
                                                RPV.MethodID = MethodID;
                                                RPVL.MethodID = MethodID;
                                                RPV.MethodMarker = MethodMarker;
                                                RPVL.MethodMarker = MethodMarker;
                                                RPV.ParameterID = int.Parse(rr[0]["ParameterID"].ToString());
                                                RPVL.ParameterID = int.Parse(rr[0]["ParameterID"].ToString());
                                                RPV.Value = rr[0]["DefaultValue"].ToString();
                                                RPVL.Value = rr[0]["DefaultValue"].ToString();
                                                RPVL.DisplayText = rr[0]["DisplayText"].ToString();
                                                this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodParameter.Rows.Add(RPV);
                                                this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodParameterList.Rows.Add(RPVL);
                                            }
                                        }
                                    }
                                    break;
                                case Target.Processing:
                                    //int PartID = int.Parse(R["PartID"].ToString());
                                    int ProcessingID = int.Parse(R["ProcessingID"].ToString());
                                    string SpecimenProcessingID = R["SpecimenProcessingID"].ToString();
                                    int ProcMethodID = int.Parse(R["MethodID"].ToString());
                                    string ProcMethodMarker = R["MethodMarker"].ToString();
                                    System.Collections.Generic.Dictionary<string, bool> Checked = f.Items;
                                    System.Collections.Generic.List<string> Selected = new List<string>();
                                    foreach (System.Collections.Generic.KeyValuePair<string, bool> KV in Checked)
                                    {
                                        Selected.Add(KV.Key);
                                    }
                                    //foreach (System.Collections.Generic.KeyValuePair<string, bool> KV in Checked)
                                    foreach(string S in Selected)
                                    {
                                        //if (KV.Value)
                                        {
                                            System.Data.DataRow[] rr = dt.Select("DisplayText = '" + S + "'");
                                            if (rr.Length > 0)
                                            {
                                                int ParameterID = int.Parse(rr[0]["ParameterID"].ToString());
                                                string DefaultValue = rr[0]["DefaultValue"].ToString();
                                                DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenProcessingMethodParameterRow RPV = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodParameter.NewCollectionSpecimenProcessingMethodParameterRow();
                                                DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenProcessingMethodParameterListRow RPVL = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodParameterList.NewCollectionSpecimenProcessingMethodParameterListRow();
                                                RPV.CollectionSpecimenID = this._iMainForm.ID_Specimen();
                                                RPVL.CollectionSpecimenID = this._iMainForm.ID_Specimen();
                                                RPV.SpecimenProcessingID = int.Parse(SpecimenProcessingID);
                                                RPVL.SpecimenProcessingID = int.Parse(SpecimenProcessingID);
                                                RPV.ProcessingID = ProcessingID;
                                                RPVL.ProcessingID = ProcessingID;
                                                RPV.MethodID = ProcMethodID;
                                                RPVL.MethodID = ProcMethodID;
                                                RPV.MethodMarker = ProcMethodMarker;
                                                RPVL.MethodMarker = ProcMethodMarker;
                                                RPV.ParameterID = int.Parse(rr[0]["ParameterID"].ToString());
                                                RPVL.ParameterID = int.Parse(rr[0]["ParameterID"].ToString());
                                                RPV.Value = rr[0]["DefaultValue"].ToString();
                                                RPVL.Value = rr[0]["DefaultValue"].ToString();
                                                RPVL.DisplayText = rr[0]["DisplayText"].ToString();
                                                this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodParameter.Rows.Add(RPV);
                                                this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodParameterList.Rows.Add(RPVL);
                                            }
                                        }
                                    }
                                    break;
                                case Target.Event:
                                    int EventID = int.Parse(R["CollectionEventID"].ToString());
                                    int EventMethodID = int.Parse(R["MethodID"].ToString());
                                    string EventMethodMarker = R["MethodMarker"].ToString();
                                    foreach (string M in f.SelectedItems())
                                    {
                                        if (M != null)
                                        {
                                            System.Data.DataRow[] rr = dt.Select("DisplayText = '" + M + "'");
                                            if (rr.Length == 1)
                                            {
                                                DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionEventParameterValueRow RPV = this._iMainForm.DataSetCollectionSpecimen().CollectionEventParameterValue.NewCollectionEventParameterValueRow();
                                                DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionEventParameterValueListRow RPVL = this._iMainForm.DataSetCollectionSpecimen().CollectionEventParameterValueList.NewCollectionEventParameterValueListRow();
                                                RPV.CollectionEventID = EventID;
                                                RPVL.CollectionEventID = EventID;
                                                RPV.MethodID = EventMethodID;
                                                RPVL.MethodID = EventMethodID;
                                                RPV.MethodMarker = EventMethodMarker;
                                                RPVL.MethodMarker = EventMethodMarker;
                                                RPV.ParameterID = int.Parse(rr[0]["ParameterID"].ToString());
                                                RPVL.ParameterID = int.Parse(rr[0]["ParameterID"].ToString());
                                                RPV.Value = rr[0]["DefaultValue"].ToString();
                                                RPVL.Value = rr[0]["DefaultValue"].ToString();
                                                RPVL.DisplayText = rr[0]["DisplayText"].ToString();
                                                this._iMainForm.DataSetCollectionSpecimen().CollectionEventParameterValue.Rows.Add(RPV);
                                                this._iMainForm.DataSetCollectionSpecimen().CollectionEventParameterValueList.Rows.Add(RPVL);
                                            }
                                        }
                                    }
                                    break;
                            }

                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("No parameters missing");
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private System.Data.DataView _dvMethod;
        private System.Data.DataView dvMethod
        {
            get
            {
                if (this._dvMethod == null)
                {
                    switch (this._Target)
                    {
                        case Target.Analysis:
                            this._dvMethod = new DataView(this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodList);
                            break;
                        case Target.Event:
                            this._dvMethod = new DataView(this._iMainForm.DataSetCollectionSpecimen().CollectionEventMethodList);
                            break;
                        case Target.Processing:
                            this._dvMethod = new DataView(this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodList);
                            break;
                    }
                }
                System.Data.DataRowView RU = (System.Data.DataRowView)this._ParentSource.Current;
                string RowFilter = this.ParentKeyRestriction(RU);
                if (RowFilter.Length > 0)
                {
                    this._dvMethod.RowFilter = RowFilter;
                    this._dvMethod.Sort = "DisplayText";
                }
                return this._dvMethod;
            }
        }

        private System.Data.DataView _dvMethodParameter;
        private System.Data.DataView dvMethodParameter
        {
            get
            {
                if (this._dvMethodParameter == null)
                {
                    switch (this._Target)
                    {
                        case Target.Analysis:
                            this._dvMethodParameter = new DataView(this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodParameterList);
                            break;
                        case Target.Event:
                            this._dvMethodParameter = new DataView(this._iMainForm.DataSetCollectionSpecimen().CollectionEventParameterValueList);
                            break;
                        case Target.Processing:
                            this._dvMethodParameter = new DataView(this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodParameterList);
                            break;
                    }
                }
                if (this.listBoxMethod.SelectedIndex > -1)
                {
                    try
                    {
                        System.Data.DataRowView RM = (System.Data.DataRowView)this.listBoxMethod.SelectedItem;// this.dataSetCollectionSpecimen.IdentificationUnitAnalysisMethodList.Rows[this.listBoxAnalysisMethod.SelectedIndex];
                        string Restriction = this.ParentKeyRestriction(RM);
                        if (Restriction.Length == 0)
                        {
                        }
                        else
                        {
                            Restriction += " AND MethodID = " + RM["MethodID"].ToString() +
                                " AND MethodMarker = '" + RM["MethodMarker"].ToString() + "'";
                            this._dvMethodParameter.RowFilter = Restriction;
                            this._dvMethodParameter.Sort = "DisplayText";
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
                else
                    this._dvMethodParameter.RowFilter = "1 = 0";
                return this._dvMethodParameter;
            }
        }

        #region Public methods

        public void setMethodControls()
        {
            System.Data.DataRowView RV = null;
            if (this._Source.Current != null)
                RV = (System.Data.DataRowView)this._Source.Current;
            System.Data.DataRowView RVparent = null;
            if (this._ParentSource.Current != null)
                RVparent = (System.Data.DataRowView)this._ParentSource.Current;
            try
            {
                string WhereClause = "";
                string SQL = "";
                if (RVparent != null)
                {
                    switch (this._Target)
                    {
                        case Target.Analysis:
                                WhereClause = "WHERE (A.IdentificationUnitID = " + RVparent["IdentificationUnitID"].ToString() + ") " +
                                    "AND (A.AnalysisID = " + RVparent["AnalysisID"].ToString() + ") " +
                                    "AND (A.AnalysisNumber = '" + RVparent["AnalysisNumber"].ToString() + "')";
                                this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodList.Clear();
                                SQL = "SELECT A.CollectionSpecimenID, A.IdentificationUnitID, M.MethodID, A.AnalysisID, A.AnalysisNumber, M.DisplayText + ' ' + A.MethodMarker AS DisplayText, A.MethodMarker " +
                                    "FROM IdentificationUnitAnalysisMethod AS A INNER JOIN " +
                                    "Method AS M ON A.MethodID = M.MethodID " + WhereClause;
                                Microsoft.Data.SqlClient.SqlDataAdapter adA = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                                adA.SelectCommand.CommandText = SQL;
                                adA.Fill(this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodList);
                                SQL = "SELECT A.CollectionSpecimenID, A.IdentificationUnitID, A.AnalysisID, A.AnalysisNumber, A.MethodID, A.ParameterID, A.Value, P.DisplayText, A.MethodMarker " +
                                    "FROM IdentificationUnitAnalysisMethodParameter AS A INNER JOIN " +
                                    "Parameter AS P ON A.ParameterID = P.ParameterID AND A.MethodID = P.MethodID " + WhereClause;
                                adA.SelectCommand.CommandText = SQL;
                                adA.Fill(this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodParameterList);
                            break;
                        case Target.Processing:
                                WhereClause = "WHERE (A.SpecimenProcessingID = " + RVparent["SpecimenProcessingID"].ToString() + ") " +
                                    "AND (A.ProcessingID = " + RVparent["ProcessingID"].ToString() + ") ";
                                this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodList.Clear();
                                SQL = "SELECT A.CollectionSpecimenID, A.SpecimenProcessingID, M.MethodID, A.ProcessingID, M.DisplayText + ' ' + A.MethodMarker AS DisplayText, A.MethodMarker " +
                                    "FROM CollectionSpecimenProcessingMethod AS A INNER JOIN " +
                                    "Method AS M ON A.MethodID = M.MethodID " + WhereClause;
                                Microsoft.Data.SqlClient.SqlDataAdapter adP = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                                adP.SelectCommand.CommandText = SQL;
                                adP.Fill(this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodList);
                                SQL = "SELECT A.CollectionSpecimenID, A.SpecimenProcessingID, A.ProcessingID, A.MethodID, A.ParameterID, A.Value, P.DisplayText, A.MethodMarker " +
                                    "FROM CollectionSpecimenProcessingMethodParameter AS A INNER JOIN " +
                                    "Parameter AS P ON A.ParameterID = P.ParameterID AND A.MethodID = P.MethodID " + WhereClause;
                                adP.SelectCommand.CommandText = SQL;
                                adP.Fill(this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenProcessingMethodParameterList);
                            break;
                        case Target.Event:
                            WhereClause = "WHERE (A.CollectionEventID = " + RVparent["CollectionEventID"].ToString() + ") ";
                            this._iMainForm.DataSetCollectionSpecimen().CollectionEventMethodList.Clear();
                            SQL = "SELECT A.CollectionEventID, M.MethodID, M.DisplayText + ' ' + A.MethodMarker AS DisplayText, A.MethodMarker " +
                                "FROM CollectionEventMethod AS A INNER JOIN " +
                                "Method AS M ON A.MethodID = M.MethodID " + WhereClause;
                            Microsoft.Data.SqlClient.SqlDataAdapter adE = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                            adE.SelectCommand.CommandText = SQL;
                            adE.Fill(this._iMainForm.DataSetCollectionSpecimen().CollectionEventMethodList);
                            SQL = "SELECT A.CollectionEventID, A.MethodID, A.ParameterID, A.Value, P.DisplayText, A.MethodMarker " +
                                "FROM CollectionEventParameterValue AS A INNER JOIN " +
                                "Parameter AS P ON A.ParameterID = P.ParameterID AND A.MethodID = P.MethodID " + WhereClause;
                            adE.SelectCommand.CommandText = SQL;
                            adE.Fill(this._iMainForm.DataSetCollectionSpecimen().CollectionEventParameterValueList);
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            this.listBoxMethodParameter.DataSource = this.dvMethodParameter;
            this.listBoxMethodParameter.DisplayMember = "DisplayText";
            this.listBoxMethodParameter.ValueMember = "ParameterID";

        }
        
        #endregion

        //private void setAnalysisMethodControls(int UnitID, int AnalysisID)
        //{
        //    // METHOD
        //    try
        //    {
        //        this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodList.Clear();
        //        string SQL = "SELECT A.CollectionSpecimenID, A.IdentificationUnitID, M.MethodID, A.AnalysisID, A.AnalysisNumber, M.DisplayText, A.MethodMarker " +
        //            "FROM IdentificationUnitAnalysisMethod AS A INNER JOIN " +
        //            "Method AS M ON A.MethodID = M.MethodID " +
        //            "WHERE (A.IdentificationUnitID = " + UnitID.ToString() + ") AND (A.AnalysisID = " + AnalysisID.ToString() + ")";
        //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //        ad.SelectCommand.CommandText = SQL;
        //        ad.Fill(this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodList);
        //    }
        //    catch (System.Exception ex) { }

        //    // METHOD PARAMETER
        //    try
        //    {
        //        string SQL = "SELECT A.CollectionSpecimenID, A.IdentificationUnitID, A.AnalysisID, A.AnalysisNumberker, A.MethodID, A.ParameterID, A.Value, P.DisplayText, A.MethodMar " +
        //            "FROM IdentificationUnitAnalysisMethodParameter AS A INNER JOIN " +
        //            "Parameter AS P ON A.ParameterID = P.ParameterID AND A.MethodID = P.MethodID " +
        //            "WHERE (A.IdentificationUnitID = " + UnitID.ToString() + ") AND (A.AnalysisID = " + AnalysisID.ToString() + ")";
        //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //        ad.SelectCommand.CommandText = SQL;
        //        ad.Fill(this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitAnalysisMethodParameterList);
        //    }
        //    catch (System.Exception ex) { }

        //    this.listBoxMethodParameter.DataSource = this.dvMethodParameter;
        //    this.listBoxMethodParameter.DisplayMember = "DisplayText";
        //    this.listBoxMethodParameter.ValueMember = "ParameterID";

        //}

    }
}
