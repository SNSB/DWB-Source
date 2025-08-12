using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityCollection
{
    class Loan : HierarchicalEntity//, IHierarchicalEntity
    {

        #region Parameter
        public enum LoanCorrespondenceType {Sending, Confirmation, Reminder, PartialReturn, Return};
        public static string SqlFieldsLoan = " LoanID, LoanParentID, LoanTitle, LoanPartnerName, LoanPartnerAgentURI, " +
            "LoanBegin, LoanEnd, LoanComment, LoanNumber, Investigator, InitialNumberOfSpecimen, " +
            "InternalNotes, ResponsibleName, ResponsibleAgentURI ";
        public static string SqlFieldsLoanHistory = " LoanID, [Date], LoanText, LoanDocument, InternalNotes ";

        private DiversityCollection.Datasets.DataSetLoan _dsLoanForReturn;
        private System.IO.FileInfo _XmlFile;
        private System.Collections.Generic.Dictionary<string, string> _LoanParterAddressValues;
        private DiversityWorkbench.Agent _Agent;
        private System.Windows.Forms.TabControl _TabControl;
        private int _LoanID;
        private int _InitialNumberOfSpecimen = 0;

        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterLoanHistory;
        private DiversityCollection.Datasets.DataSetLoan _DataSetLoanForReturn;
        private System.Data.DataTable _SpecimenListForPartialReturn;

        #endregion

        #region Construction
        public Loan(
            ref System.Data.DataSet Dataset,
            System.Data.DataTable DataTable, 
            ref System.Windows.Forms.TreeView TreeView, 
            System.Windows.Forms.Form Form,
            DiversityWorkbench.UserControls.UserControlQueryList UserControlQueryList,
            System.Windows.Forms.SplitContainer SplitContainerMain,
            System.Windows.Forms.SplitContainer SplitContainerData,
            System.Windows.Forms.ToolStripButton ToolStripButtonSpecimenList,
            //System.Windows.Forms.ImageList ImageListSpecimenList,
            DiversityCollection.UserControls.UserControlSpecimenList UserControlSpecimenList,
            System.Windows.Forms.HelpProvider HelpProvider,
            System.Windows.Forms.ToolTip ToolTip, 
            ref System.Windows.Forms.BindingSource BindingSource,
            System.Windows.Forms.TabControl TabControl)
            : base(ref Dataset, DataTable, ref TreeView, Form, UserControlQueryList, SplitContainerMain,
            SplitContainerData, ToolStripButtonSpecimenList, /*ImageListSpecimenList,*/ UserControlSpecimenList,
            HelpProvider, ToolTip, ref BindingSource, null, null)
        {
            this._sqlItemFieldList = " LoanID, LoanParentID, LoanTitle, LoanPartnerName, LoanPartnerAgentURI, " +
                "LoanBegin, LoanEnd, LoanComment, LoanNumber, Investigator, InitialNumberOfSpecimen, InternalNotes, " +
                "ResponsibleName, ResponsibleAgentURI  ";
            this._SpecimenTable = "CollectionStorage";
            this._MainTable = "Loan";
            this._TabControl = TabControl;
        }
        
        #endregion

        #region Functions and properties
        protected override string SqlSpecimenCount(int ID)
        {
            return "SELECT COUNT(*) FROM CollectionStorage WHERE LoanID = " + ID.ToString();
        }

        #endregion

        #region Interface

        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions
        {
            get
            {
                System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

                string Description = this.FormFunctions.ColumnDescription("Loan", "LoanTitle");
                DiversityWorkbench.QueryCondition q0 = new DiversityWorkbench.QueryCondition(true, "Loan", "LoanID", "LoanTitle", "Loan", "Name", "Name", Description);
                QueryConditions.Add(q0);

                Description = this.FormFunctions.ColumnDescription("Loan", "LoanPartnerName");
                DiversityWorkbench.QueryCondition q1 = new DiversityWorkbench.QueryCondition(true, "Loan", "LoanID", "LoanPartnerName", "Loan", "Loan partner", "Loan partner", Description);
                QueryConditions.Add(q1);

                Description = this.FormFunctions.ColumnDescription("Loan", "LoanBegin");
                DiversityWorkbench.QueryCondition q2 = new DiversityWorkbench.QueryCondition(true, "Loan", "LoanID", "LoanBegin", "Loan", "Begin", "Begin", Description, true);
                QueryConditions.Add(q2);

                Description = this.FormFunctions.ColumnDescription("Loan", "LoanEnd");
                DiversityWorkbench.QueryCondition q3 = new DiversityWorkbench.QueryCondition(true, "Loan", "LoanID", "LoanEnd", "Loan", "End", "End", Description, true);
                QueryConditions.Add(q3);

                Description = this.FormFunctions.ColumnDescription("Loan", "LoanComment");
                DiversityWorkbench.QueryCondition q4 = new DiversityWorkbench.QueryCondition(true, "Loan", "LoanID", "LoanComment", "Loan", "Comment", "Comment", Description);
                QueryConditions.Add(q4);

                Description = this.FormFunctions.ColumnDescription("Loan", "LoanNumber");
                DiversityWorkbench.QueryCondition q5 = new DiversityWorkbench.QueryCondition(true, "Loan", "LoanID", "LoanNumber", "Loan", "Number", "Number", Description);
                QueryConditions.Add(q5);

                Description = this.FormFunctions.ColumnDescription("Loan", "InternalNotes");
                DiversityWorkbench.QueryCondition q6 = new DiversityWorkbench.QueryCondition(true, "Loan", "LoanID", "InternalNotes", "Loan", "Notes", "Notes", Description);
                QueryConditions.Add(q6);

                return QueryConditions;
            }
        }

        public override DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns
        {
            get
            {
                DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[2];
                QueryDisplayColumns[0].DisplayText = "Loan";
                QueryDisplayColumns[0].DisplayColumn = "LoanTitle";
                QueryDisplayColumns[0].OrderColumn = "LoanTitle";
                QueryDisplayColumns[0].IdentityColumn = "LoanID";
                QueryDisplayColumns[0].TableName = "Loan";
                QueryDisplayColumns[1].DisplayText = "Loan partner";
                QueryDisplayColumns[1].DisplayColumn = "LoanPartnerName";
                QueryDisplayColumns[1].OrderColumn = "LoanPartnerName";
                QueryDisplayColumns[1].IdentityColumn = "LoanID";
                QueryDisplayColumns[1].TableName = "Loan";
                return QueryDisplayColumns;
            }
        }

        public int InitialNumberOfSpecimen { set { this._InitialNumberOfSpecimen = value; } }

        //public void setInitalNumberOfSpecimen(int LoanID, int Count)
        //{
        //    System.Data.DataRow[] rr = this._DataSet.Tables["Loan"].Select("LoanID = " + LoanID.ToString());
        //    if (rr.Length > 0)
        //    {
        //        rr[0]["InitialNumberOfSpecimen"] = Count;
        //    }
        //    //this._DataSet.Tables["Loan"].Rows[0]["InitialNumberOfSpecimen"] = value;
        //    //this._DataTable.Rows[0]["InitialNumberOfSpecimen"] = value; 
        //}

        public System.Data.DataSet DataSet { get { return this._DataSet; } }

        public override void ItemChanged() 
        {
            this.initDatasetForPartialReturn((int)this.ID);
        }

        public DiversityCollection.Datasets.DataSetLoan DataSetLoanForReturn
        {
            get 
            {
                if (this._DataSetLoanForReturn == null) this.initDatasetForPartialReturn((int)this.ID);
                return _DataSetLoanForReturn; 
            }
            //set { _DataSetLoanForReturn = value; }
        }

        public System.Data.DataTable SpecimenListForPartialReturn
        {
            get 
            {
                if (this._SpecimenListForPartialReturn == null)
                {
                    this._SpecimenListForPartialReturn = new System.Data.DataTable();
                    string SQL = "SELECT CASE WHEN AccessionNumber IS NULL THEN '?' + replicate(' ', 10) " +
                        "+ '  |  ' ELSE AccessionNumber + '  |  ' END + CollectionStorage.MaterialCategory + '  |  ' + Collection.CollectionName + '  |  ' + CASE WHEN CollectionStorage.StorageLocation " +
                        "IS NULL THEN '?' ELSE CollectionStorage.StorageLocation END AS AccessionNumber,  " +
                        "CollectionStorage.CollectionSpecimenID, CollectionStorage.CollectionID,  " +
                        "CollectionStorage.StorageLocation, CollectionStorage.Stock, CollectionStorage.LoanID, CollectionStorage.Notes  " +
                        "FROM CollectionSpecimen INNER JOIN  " +
                        "CollectionStorage ON CollectionSpecimen.CollectionSpecimenID = CollectionStorage.CollectionSpecimenID INNER JOIN  " +
                        "Collection ON CollectionStorage.CollectionID = Collection.CollectionID  " +
                        "WHERE CollectionStorage.LoanID = " + this.ID.ToString();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    //ad.Fill(this._DataSetLoanForReturn.Tables["Loan"]);
                    //ad.SelectCommand.CommandText = SQL;
                    ad.Fill(this._SpecimenListForPartialReturn);
                }
                return _SpecimenListForPartialReturn; 
            }
            //set { _SpecimenListForPartialReturn = value; }
        }


        public void initDatasetForPartialReturn(int ID)
        {
            if (this._DataSetLoanForReturn == null) this._DataSetLoanForReturn = new Datasets.DataSetLoan();
            foreach (System.Data.DataTable T in this._DataSetLoanForReturn.Tables)
                T.Clear();
            string SQL = "SELECT LoanID, LoanParentID, LoanTitle, LoanPartnerName, LoanPartnerAgentURI, " +
                "LoanBegin, LoanEnd, LoanComment, LoanNumber, " +
                "InitialNumberOfSpecimen, Investigator, InternalNotes, ResponsibleName, ResponsibleAgentURI " +
                "FROM Loan " +
                "WHERE LoanID = " + ID.ToString();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(this._DataSetLoanForReturn.Tables["Loan"]);

            if (this._SpecimenListForPartialReturn != null)
            {
                SQL = "SELECT CASE WHEN AccessionNumber IS NULL THEN '?' + replicate(' ', 10) " +
                    "+ '  |  ' ELSE AccessionNumber + '  |  ' END + CollectionStorage.MaterialCategory + '  |  ' + Collection.CollectionName + '  |  ' + CASE WHEN CollectionStorage.StorageLocation " +
                    "IS NULL THEN '?' ELSE CollectionStorage.StorageLocation END AS AccessionNumber,  " +
                    "CollectionStorage.CollectionSpecimenID, CollectionStorage.CollectionID,  " +
                    "CollectionStorage.StorageLocation, CollectionStorage.Stock, CollectionStorage.LoanID, CollectionStorage.Notes  " +
                    "FROM CollectionSpecimen INNER JOIN  " +
                    "CollectionStorage ON CollectionSpecimen.CollectionSpecimenID = CollectionStorage.CollectionSpecimenID INNER JOIN  " +
                    "Collection ON CollectionStorage.CollectionID = Collection.CollectionID  " +
                    "WHERE CollectionStorage.LoanID = " + ID.ToString();
                ad.SelectCommand.CommandText = SQL;
                ad.Fill(this._SpecimenListForPartialReturn);
            }
            //int CollectionID;
            //string SQL = "SELECT MIN(CollectionStorage.CollectionID) " +
            //    "FROM CollectionStorage INNER JOIN " +
            //    "CollectionSpecimen ON CollectionStorage.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID " +
            //    "WHERE CollectionStorage.LoanID = " + ID.ToString();
            //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            //Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            //con.Open();
            //if (int.TryParse(Com.ExecuteScalar().ToString(), out CollectionID))
            //{
            //    ad.SelectCommand.CommandText = "select * from dbo.CollectionHierarchy (" + CollectionID + ")";
            //    ad.Fill(this._DataSetLoanForReturn.Tables["Collection"]);
            //}
            //con.Close();

            //ad.SelectCommand.CommandText = "SELECT DISTINCT IdentificationUnit.CollectionSpecimenID, " +
            //    "IdentificationUnit.IdentificationUnitID, IdentificationUnit.LastIdentificationCache, " +
            //    "IdentificationUnit.DisplayOrder " +
            //    "FROM IdentificationUnit INNER JOIN " +
            //    "CollectionStorage ON IdentificationUnit.CollectionSpecimenID = CollectionStorage.CollectionSpecimenID " +
            //    "WHERE CollectionStorage.LoanID = " + ID.ToString();
            //ad.Fill(this._DataSet.Tables["IdentificationUnit"]);

            //ad.SelectCommand.CommandText = "SELECT COUNT(*) AS NumberOfSpecimen, IdentificationUnit.TaxonomicGroup " +
            //    "FROM CollectionStorage INNER JOIN " +
            //    "IdentificationUnit ON CollectionStorage.CollectionSpecimenID = IdentificationUnit.CollectionSpecimenID " +
            //    "WHERE CollectionStorage.LoanID = " + ID.ToString() + " AND IdentificationUnit.DisplayOrder = 1 " +
            //    "GROUP BY IdentificationUnit.TaxonomicGroup";
            //ad.Fill(this._DataSet.Tables["TaxonomicGroups"]);

            //this.clearBrowser();
            //this.setTabControl();
        }


        
        #endregion

        #region Datahandling

        public override void fillDependentTables(int ID)
        {
            if (this._DataSet == null) this._DataSet = new System.Data.DataSet();
            foreach (System.Data.DataTable T in this._DataSet.Tables)
            {
                if (T.TableName != "Loan")
                    T.Clear();
            }
            string SQL = "SELECT LoanID, LoanParentID, LoanTitle, LoanPartnerName, LoanPartnerAgentURI, " +
                "LoanBegin, LoanEnd, LoanComment, LoanNumber, " +
                "InitialNumberOfSpecimen, Investigator, InternalNotes, ResponsibleName, ResponsibleAgentURI " +
                "FROM Loan " +
                "WHERE LoanID = " + ID.ToString();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(this._DataSet.Tables["Loan"]);

            SQL = "SELECT LoanID, Date, LoanText, LoanDocument, InternalNotes " +
                "FROM LoanHistory " +
                "WHERE LoanID = " + ID.ToString();
            //if (this._SqlDataAdapterLoanHistory == null)
            //{
            //    this._SqlDataAdapterLoanHistory = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //}
            this.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterLoanHistory, SQL, this._DataSet.Tables["LoanHistory"]);

            //ad.SelectCommand.CommandText = "SELECT CollectionSpecimen.CollectionSpecimenID, CollectionSpecimen.AccessionNumber " +
            //    "FROM  CollectionStorage INNER JOIN " +
            //    "CollectionSpecimen ON CollectionStorage.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID " +
            //    "WHERE CollectionStorage.LoanID = " + ID.ToString(); ;
            //ad.Fill(this._dsLoan.CollectionSpecimen);

            ad.SelectCommand.CommandText = "SELECT CollectionSpecimen.AccessionNumber, " +
                "CollectionStorage.CollectionSpecimenID, CollectionStorage.CollectionID, CollectionStorage.MaterialCategory, " +
                "CollectionStorage.StorageLocation, CollectionStorage.Stock, CollectionStorage.LoanID, CollectionStorage.Notes " +
                "FROM CollectionStorage INNER JOIN " +
                "CollectionSpecimen ON CollectionStorage.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID " +
                "WHERE (CollectionStorage.LoanID = " + ID.ToString() +
                ") ORDER BY CollectionSpecimen.AccessionNumber ";
            ad.Fill(this._DataSet.Tables["CollectionStorage"]);

            if (this._DataSet.Tables["CollectionStorage"].Rows.Count > 0)
            {
                System.Data.DataRow R = this._DataSet.Tables["CollectionStorage"].Rows[0];
                int CollectionID = int.Parse(R["CollectionID"].ToString());

                ad.SelectCommand.CommandText = "select * from dbo.CollectionHierarchy (" + CollectionID + ")";
                ad.Fill(this._DataSet.Tables["Collection"]);
            }

            ad.SelectCommand.CommandText = "SELECT DISTINCT IdentificationUnit.CollectionSpecimenID, " +
                "IdentificationUnit.IdentificationUnitID, IdentificationUnit.LastIdentificationCache, " +
                "IdentificationUnit.DisplayOrder " +
                "FROM IdentificationUnit INNER JOIN " +
                "CollectionStorage ON IdentificationUnit.CollectionSpecimenID = CollectionStorage.CollectionSpecimenID " +
                "WHERE CollectionStorage.LoanID = " + ID.ToString();
            ad.Fill(this._DataSet.Tables["IdentificationUnit"]);

            ad.SelectCommand.CommandText = "SELECT COUNT(*) AS NumberOfSpecimen, IdentificationUnit.TaxonomicGroup " +
                "FROM CollectionStorage INNER JOIN " +
                "IdentificationUnit ON CollectionStorage.CollectionSpecimenID = IdentificationUnit.CollectionSpecimenID " +
                "WHERE CollectionStorage.LoanID = " + ID.ToString() + " AND IdentificationUnit.DisplayOrder = 1 " +
                "GROUP BY IdentificationUnit.TaxonomicGroup";
            ad.Fill(this._DataSet.Tables["TaxonomicGroups"]);

            this.clearBrowser();
            this.setTabControl();
        }

        private void clearBrowser()
        {
            foreach (System.Windows.Forms.Control C in this._TabControl.Controls)
                this.clearBrowser(C);
        }

        private void clearBrowser(System.Windows.Forms.Control C)
        {
            if (C.GetType() == typeof(System.Windows.Forms.WebBrowser))
            {
                System.Windows.Forms.WebBrowser W = (System.Windows.Forms.WebBrowser)C;
                W.DocumentText = "";
            }
            if (C.Controls.Count > 0)
            {
                foreach (System.Windows.Forms.Control CC in C.Controls)
                    this.clearBrowser(CC);
            }
        }

        //public override void fillDependentTables(int ID)
        //{
        //    if (this._dsLoan == null) this._dsLoan = new DataSetLoan();
        //    this._dsLoan.Collection.Clear();
        //    //this._dsLoan.CollectionSpecimen.Clear();
        //    this._dsLoan.CollectionStorage.Clear();
        //    this._dsLoan.IdentificationUnit.Clear();
        //    this._dsLoan.LoanHistory.Clear();
        //    string SQL = "SELECT LoanID, LoanParentID, LoanTitle, LoanPartnerName, LoanPartnerAgentURI, " +
        //        "LoanBegin, LoanEnd, LoanComment, LoanNumber, " +
        //        "InitialNumberOfSpecimen, Investigator, InternalNotes, ResponsibleName, ResponsibleAgentURI " +
        //        "FROM Loan " +
        //        "WHERE LoanID = " + ID.ToString();
        //    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //    ad.Fill(this._dsLoan.Loan);

        //    SQL = "SELECT LoanID, Date, LoanText, LoanDocument, InternalNotes " +
        //        "FROM LoanHistory " +
        //        "WHERE LoanID = " + ID.ToString();
        //    //if (this._SqlDataAdapterLoanHistory == null)
        //    //{
        //    //    this._SqlDataAdapterLoanHistory = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //    //}
        //    this.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterLoanHistory, SQL, this._dsLoan.LoanHistory);

        //    //ad.SelectCommand.CommandText = "SELECT CollectionSpecimen.CollectionSpecimenID, CollectionSpecimen.AccessionNumber " +
        //    //    "FROM  CollectionStorage INNER JOIN " +
        //    //    "CollectionSpecimen ON CollectionStorage.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID " +
        //    //    "WHERE CollectionStorage.LoanID = " + ID.ToString(); ;
        //    //ad.Fill(this._dsLoan.CollectionSpecimen);

        //    ad.SelectCommand.CommandText = "SELECT CollectionSpecimen.AccessionNumber, " +
        //        "CollectionStorage.CollectionSpecimenID, CollectionStorage.CollectionID, CollectionStorage.MaterialCategory, " +
        //        "CollectionStorage.StorageLocation, CollectionStorage.Stock, CollectionStorage.LoanID, CollectionStorage.Notes " +
        //        "FROM CollectionStorage INNER JOIN " +
        //        "CollectionSpecimen ON CollectionStorage.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID " +
        //        "WHERE (CollectionStorage.LoanID = " + ID.ToString() +
        //        ") ORDER BY CollectionSpecimen.AccessionNumber "   ;
        //    ad.Fill(this._dsLoan.CollectionStorage);

        //    if (this._dsLoan.CollectionStorage.Rows.Count > 0)
        //    {
        //        System.Data.DataRow R = this._dsLoan.CollectionStorage.Rows[0];
        //        int CollectionID = int.Parse(R["CollectionID"].ToString());

        //        ad.SelectCommand.CommandText = "select * from dbo.CollectionHierarchy (" + CollectionID + ")";
        //        ad.Fill(this._dsLoan.Collection);
        //    }

        //    ad.SelectCommand.CommandText = "SELECT DISTINCT IdentificationUnit.CollectionSpecimenID, " +
        //        "IdentificationUnit.IdentificationUnitID, IdentificationUnit.LastIdentificationCache, " +
        //        "IdentificationUnit.DisplayOrder " +
        //        "FROM IdentificationUnit INNER JOIN " +
        //        "CollectionStorage ON IdentificationUnit.CollectionSpecimenID = CollectionStorage.CollectionSpecimenID " +
        //        "WHERE CollectionStorage.LoanID = " + ID.ToString();
        //    ad.Fill(this._dsLoan.IdentificationUnit);

        //    this.setTabControl();
        //}

        public override void saveDependentTables() 
        {
            this.FormFunctions.updateTable(this._DataSet, "LoanHistory", this._SqlDataAdapterLoanHistory, this._BindingSource);
            //this.FormFunctions.updateTable(this._dsLoan, "LoanHistory", this._SqlDataAdapterLoanHistory, this._BindingSource);
        }

        
        #endregion

        #region From functions
        public void setTabControl()
        {
            if (this._TabControl != null)
            {
                //if (this._dsLoan.LoanHistory.Rows.Count > 0)
                if (this._DataSet.Tables["LoanHistory"].Rows.Count > 0)
                {
                    foreach (System.Windows.Forms.Control C in this._TabControl.TabPages[1].Controls)
                        C.Enabled = false;
                    for (int i = 2; i < this._TabControl.TabPages.Count; i++)
                    {
                        foreach (System.Windows.Forms.Control C in this._TabControl.TabPages[i].Controls)
                            C.Enabled = true;
                    }
                }
                else
                {
                    foreach (System.Windows.Forms.Control C in this._TabControl.TabPages[1].Controls)
                        C.Enabled = true;
                    for (int i = 2; i < this._TabControl.TabPages.Count; i++)
                    {
                        foreach (System.Windows.Forms.Control C in this._TabControl.TabPages[i].Controls)
                            C.Enabled = false;
                    }
                }
            }
        }
        
        #endregion

        #region Properties

        public System.Collections.Generic.Dictionary<string, string> LoanParterAddressValues
        {
            get 
            {
                this._LoanParterAddressValues = this.Agent.UnitValues((int)this.ID);
                return _LoanParterAddressValues; 
            }
            //set { _LoanParterAddressValues = value; }
        }

        public DiversityWorkbench.Agent Agent
        {
            get 
            {
                if (this._Agent == null)
                    this._Agent = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                return _Agent; 
            }
            //set { _Agent = value; }
        }

        #endregion

        #region XML

        public void writeDefaultXslt(System.IO.FileInfo File)
        {
            System.Xml.XmlWriter W;
            try
            {
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                settings.Encoding = System.Text.Encoding.UTF8;
                W = System.Xml.XmlWriter.Create(File.FullName, settings);
                W.WriteRaw("<?xml version=\"1.0\" encoding=\"utf-16\" ?> " +
                    "<xsl:stylesheet version=\"1.0\"  " +
                    "xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\"> " +
                    "<xsl:output method=\"xml\" encoding=\"utf-16\"/> ");
                W.WriteRaw("<xsl:template match=\"text\"></xsl:template> " +
                    "   </xsl:stylesheet>)");
                W.Close();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public string XmlCorrespondence(
            System.IO.FileInfo XmlFile,
            System.IO.FileInfo XsltStilesheet,
            DiversityCollection.Loan.LoanCorrespondenceType Type,
            int? NumberOfReturnedSpecimen,
            int LoanID)
        {
            //this._dsLoan = dsLoan;
            this._LoanID = LoanID;
            string XML = "";

            System.Xml.XmlWriter W;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            //System.Text.StringBuilder S = new StringBuilder();
            //W = System.Xml.XmlWriter.Create(S, settings);
            W = System.Xml.XmlWriter.Create(XmlFile.FullName, settings);
            try
            {
                W.WriteStartDocument();
                if (XsltStilesheet != null)
                {
                    //W.WriteProcessingInstruction("xml-stylesheet", "href='#style_Loan' type='text/xsl'");
                    W.WriteProcessingInstruction("xml-stylesheet", "href='" + XsltStilesheet.FullName + "' type='text/xsl'");
                }
                W.WriteStartElement("Loan");
                this.writeXmlLoan(ref W, NumberOfReturnedSpecimen);
                //if (XsltStilesheet != null)
                //{
                //    //this.writeXslt(ref W, XsltStilesheet);
                //}
                W.WriteFullEndElement();
                W.WriteEndDocument();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                if (W != null)
                {
                    W.Flush();
                    W.Close();
                }
            }
            return XML;
        }

        public void  XmlCorrespondencePartialReturn(
            System.IO.FileInfo XmlFile,
            System.IO.FileInfo XsltStilesheet,
            DiversityCollection.Datasets.DataSetLoan DataSetLoan,
            System.DateTime Date,
            int LoanID)
        {
            this._LoanID = LoanID;
            this._dsLoanForReturn = DataSetLoan;
            System.Xml.XmlWriter W;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            W = System.Xml.XmlWriter.Create(XmlFile.FullName, settings);
            try
            {
                W.WriteStartDocument();
                if (XsltStilesheet != null)
                    W.WriteProcessingInstruction("xml-stylesheet", "href='" + XsltStilesheet.FullName + "' type='text/xsl'");
                W.WriteStartElement("Loan");
                int N = DataSetLoan.CollectionStorage.Rows.Count;
                this.writeXmlLoan(ref W, N);
                W.WriteFullEndElement();
                W.WriteEndDocument();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                if (W != null)
                {
                    W.Flush();
                    W.Close();
                }
            }
        }

        private void writeXmlLoanForReturn(ref System.Xml.XmlWriter W)
        {
            if (this._DataSet.Tables["Loan"].Rows.Count > 0)
            {
                System.Data.DataRow[] rr = this._DataSet.Tables["Loan"].Select("LoanID = " + this._LoanID.ToString());
                if (rr.Length > 0)
                {
                    W.WriteElementString("Date", this.getDateInEnglish(System.DateTime.Now));
                    System.DateTime Begin = System.DateTime.Now;
                    System.DateTime End = System.DateTime.Now;
                    bool DatePeriodOK = true;
                    System.Data.DataRow R = rr[0];
                    foreach (System.Data.DataColumn C in R.Table.Columns)
                    {
                        if (!R[C.ColumnName].Equals(System.DBNull.Value) &&
                            R[C.ColumnName].ToString().Length > 0 &&
                            C.ColumnName != "LoanID" &&
                            C.ColumnName != "LoanParentID" &&
                            C.ColumnName != "LoanPartnerAgentURI" &&
                            C.ColumnName != "InitialNumberOfSpecimen" &&
                            C.ColumnName != "ResponsibleAgentURI")
                        {
                            if (C.ColumnName == "LoanBegin" || C.ColumnName == "LoanEnd")
                            {
                                System.DateTime d;
                                if (System.DateTime.TryParse(R[C.ColumnName].ToString(), out d))
                                    W.WriteElementString(C.ColumnName, this.getDateInEnglish(d));
                                if (C.ColumnName == "LoanBegin")
                                {
                                    if (!System.DateTime.TryParse(R[C.ColumnName].ToString(), out Begin))
                                        DatePeriodOK = false;
                                }
                                if (C.ColumnName == "LoanEnd")
                                {
                                    if (!System.DateTime.TryParse(R[C.ColumnName].ToString(), out End))
                                        DatePeriodOK = false;
                                }
                            }
                            else
                                W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                        }
                    }
                    if (DatePeriodOK)
                    {
                        System.TimeSpan T = End - Begin;
                        int Months = T.Days / 30;
                        W.WriteElementString("DurationInMonths", Months.ToString());
                    }
                    if (this._InitialNumberOfSpecimen > 0)
                    {
                        W.WriteElementString("InitialNumberOfSpecimen", this._InitialNumberOfSpecimen.ToString());
                        int CurrentCount = 0;
                        string SQL = "SELECT COUNT(*) AS Count FROM CollectionStorage WHERE LoanID = " + this._LoanID.ToString();
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                        Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                        con.Open();
                        if (int.TryParse(Com.ExecuteScalar().ToString(), out CurrentCount))
                        {
                            W.WriteElementString("CurrentNumberOfSpecimen", CurrentCount.ToString());
                        }
                        con.Close();
                        int N = this._dsLoanForReturn.CollectionStorage.Rows.Count;
                        if (N > 0)
                            W.WriteElementString("NumberOfReturnedSpecimen", N.ToString());
                    }
                    if (!R["LoanPartnerAgentURI"].Equals(System.DBNull.Value))
                    {
                        W.WriteStartElement("LoanPartnerAddress");
                        this.writeXmlAddress(ref W, R["LoanPartnerAgentURI"].ToString());
                        W.WriteEndElement();
                    }
                    this.writeXmlTaxonomicGroups(ref W);
                    this.writeXmlCollection(ref W);
                    this.writeXmlStorage(ref W);
                }
            }
        }

        private void writeXslt(ref System.Xml.XmlWriter W, System.IO.FileInfo XsltStilesheet)
        {
            if (XsltStilesheet != null)
            {
                System.Xml.XmlReader R;
                R = System.Xml.XmlReader.Create(XsltStilesheet.FullName);
                //R.Read();
                //string Content = R.ReadInnerXml();// +"</" + NodeName + ">";
                //W.WriteRaw(Content);
                while (R.ReadState != System.Xml.ReadState.EndOfFile)
                {
                    R.Read();
                    if (R.NodeType != System.Xml.XmlNodeType.XmlDeclaration &&
                        //R.NodeType != System.Xml.XmlNodeType.ProcessingInstruction &&
                        R.NodeType != System.Xml.XmlNodeType.Whitespace)
                    {
                        if (R.NodeType == System.Xml.XmlNodeType.Element)
                        {
                            string Content = "";
                            //string NodeName = R.Name;
                            //string lineEnd = ">";
                            //string line = "<" + R.Name;
                            if (!R.IsEmptyElement)
                            {
                                Content = R.ReadInnerXml();// +"</" + NodeName + ">";
                                Content = "<xsl:stylesheet id='style_Loan' version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>" + Content + "</xsl:stylesheet>";
                            }
                            W.WriteRaw(Content);

                            //if (R.HasAttributes)
                            //{
                            //    int AC = R.AttributeCount;
                            //    for (int i = 0; i < AC; i++)
                            //    {
                            //        R.MoveToNextAttribute();
                            //        line += " " + R.Name + "='" + R.Value + "'";
                            //    }
                            //}
                            //line += lineEnd;
                            //W.WriteRaw(line);
                            //if (R.LocalName == "output") W.WriteRaw("<" + R.Name + "id=\"style_Loan\" version=\"1.0\" xmlns:xsl=\"" + R.NamespaceURI + "\">");
                            //if (R.LocalName == "stylesheet") W.WriteRaw("<" + R.Name + "id=\"style_Loan\" version=\"1.0\" xmlns:xsl=\"" + R.NamespaceURI + "\">");
                            //R.GetAttribute(
                            //W.WriteAttributes(R, true);
                        }
                    }
                    //break;
                }
                //W.WriteDocType("Loan", "xsl:stylesheet", "ID", "#IMPLIED");
            }
        }

        private void writeXmlLoan(ref System.Xml.XmlWriter W, int? NumberOfReturnedSpecimen)
        {
            //if (this._dsLoan.Loan.Rows.Count > 0)
            if (this._DataSet.Tables["Loan"].Rows.Count > 0)
            {
                //int LoanCount = 0;
                //System.Data.DataRow[] rr = this._dsLoan.Loan.Select("LoanID = " + this._LoanID.ToString());
                System.Data.DataRow[] rr = this._DataSet.Tables["Loan"].Select("LoanID = " + this._LoanID.ToString());
                if (rr.Length > 0)
                {
                    W.WriteElementString("Date", this.getDateInEnglish(System.DateTime.Now));
                    System.DateTime Begin = System.DateTime.Now;
                    System.DateTime End = System.DateTime.Now;
                    bool DatePeriodOK = true;
                    System.Data.DataRow R = rr[0];
                    foreach (System.Data.DataColumn C in R.Table.Columns)
                    {
                        if (!R[C.ColumnName].Equals(System.DBNull.Value) &&
                            R[C.ColumnName].ToString().Length > 0 &&
                            C.ColumnName != "LoanID" &&
                            C.ColumnName != "LoanParentID" &&
                            C.ColumnName != "LoanPartnerAgentURI" &&
                            C.ColumnName != "InitialNumberOfSpecimen" &&
                            C.ColumnName != "ResponsibleAgentURI")
                        {
                            if (C.ColumnName == "LoanBegin" || C.ColumnName == "LoanEnd")
                            {
                                System.DateTime d;
                                if ( System.DateTime.TryParse(R[C.ColumnName].ToString(), out d))
                                    W.WriteElementString(C.ColumnName, this.getDateInEnglish(d));
                                if (C.ColumnName == "LoanBegin")
                                {
                                    if (!System.DateTime.TryParse(R[C.ColumnName].ToString(), out Begin)) 
                                        DatePeriodOK = false;
                                }
                                if (C.ColumnName == "LoanEnd")
                                {
                                    if (!System.DateTime.TryParse(R[C.ColumnName].ToString(), out End))
                                        DatePeriodOK = false;
                                }
                            }
                            else
                            W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                        }
                    }
                    if (DatePeriodOK)
                    {
                        System.TimeSpan T = End - Begin;
                        int Months = T.Days / 30;
                        W.WriteElementString("DurationInMonths", Months.ToString());
                    }
                    if (this._InitialNumberOfSpecimen > 0)
                    {
                        W.WriteElementString("InitialNumberOfSpecimen", this._InitialNumberOfSpecimen.ToString());
                        int CurrentCount = 0;
                        string SQL = "SELECT COUNT(*) AS Count FROM CollectionStorage WHERE LoanID = " + this._LoanID.ToString();
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                        Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                        con.Open();
                        if (int.TryParse(Com.ExecuteScalar().ToString(), out CurrentCount))
                        {
                            W.WriteElementString("CurrentNumberOfSpecimen", CurrentCount.ToString());
                        }
                        con.Close();
                        if (NumberOfReturnedSpecimen != null)
                            W.WriteElementString("NumberOfReturnedSpecimen", NumberOfReturnedSpecimen.ToString());
                    }
                    if (!R["LoanPartnerAgentURI"].Equals(System.DBNull.Value))
                    {
                        W.WriteStartElement("LoanPartnerAddress");
                        this.writeXmlAddress(ref W, R["LoanPartnerAgentURI"].ToString());
                        //System.Collections.Generic.Dictionary<string, string> DictAddress = this.Agent.UnitValues(R["LoanPartnerAgentURI"].ToString());
                        //foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DictAddress)
                        //{
                        //    if (KV.Key == "ParentName" ||
                        //        KV.Key == "Country" ||
                        //        KV.Key == "City" ||
                        //        KV.Key == "PostalCode" ||
                        //        KV.Key == "Streetaddress" ||
                        //        KV.Key == "Address")
                        //    {
                        //        W.WriteElementString(KV.Key, KV.Value);
                        //    }
                        //}
                        W.WriteEndElement();
                    }
                    this.writeXmlTaxonomicGroups(ref W);
                    this.writeXmlCollection(ref W);
                    this.writeXmlStorage(ref W);
                }
            }
        }

        private string getDateInEnglish(System.DateTime Date)
        {
            string D = Date.Day.ToString() + " ";
            int Month = Date.Month;
            switch (Month)
            {
                case 1:
                    D += "Jan.";
                    break;
                case 2:
                    D += "Feb.";
                    break;
                case 3:
                    D += "Mar.";
                    break;
                case 4:
                    D += "Apr.";
                    break;
                case 5:
                    D += "May";
                    break;
                case 6:
                    D += "Jun.";
                    break;
                case 7:
                    D += "Jul.";
                    break;
                case 8:
                    D += "Aug.";
                    break;
                case 9:
                    D += "Sep.";
                    break;
                case 10:
                    D += "Oct.";
                    break;
                case 11:
                    D += "Nov.";
                    break;
                case 12:
                    D += "Dec.";
                    break;
            }
            D += " " + Date.Year;
            return D;
        }

        private void writeXmlAddress(ref System.Xml.XmlWriter W, string AgentURI)
        {
            System.Collections.Generic.Dictionary<string, string> DictAddress = this.Agent.UnitValues(AgentURI);
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DictAddress)
            {
                if (KV.Key == "ParentName" ||
                    KV.Key == "Country" ||
                    KV.Key == "City" ||
                    KV.Key == "PostalCode" ||
                    KV.Key == "Streetaddress" ||
                    KV.Key == "CellularPhone" ||
                    KV.Key == "Telefax" ||
                    KV.Key == "Telephone" ||
                    KV.Key == "Email" ||
                    KV.Key == "URI" ||
                    KV.Key == "Address")
                {
                    if (KV.Key == "Country")
                        W.WriteElementString(KV.Key, KV.Value.ToUpper());
                    else
                        W.WriteElementString(KV.Key, KV.Value);
                }
            }
        }

        private void writeXmlTaxonomicGroups(ref System.Xml.XmlWriter W)
        {
            W.WriteStartElement("TaxonomicGroups");
            foreach (System.Data.DataRow R in this._DataSet.Tables["TaxonomicGroups"].Rows)
            {
                W.WriteStartElement("TaxonomicGroup");
                W.WriteElementString("NumberOfSpecimen", R["NumberOfSpecimen"].ToString());
                W.WriteElementString("Group", R["TaxonomicGroup"].ToString());
                W.WriteEndElement();
            }
            W.WriteEndElement();
        }

        private void writeXmlCollection(ref System.Xml.XmlWriter W)
        {
            //System.Data.DataRow[] rr = this._dsLoan.Collection.Select("CollectionParentID IS NULL AND LEN(CollectionOwner) > 0", "DisplayOrder");
            System.Data.DataRow[] rr = this._DataSet.Tables["Collection"].Select("CollectionParentID IS NULL AND LEN(CollectionOwner) > 0");
            if (rr.Length > 0)
            {
                W.WriteStartElement("Collection");
                W.WriteElementString("CollectionName", rr[0]["CollectionName"].ToString());
                W.WriteElementString("CollectionOwner", rr[0]["CollectionOwner"].ToString());
                W.WriteElementString("CollectionAcronym", rr[0]["CollectionAcronym"].ToString());
                W.WriteElementString("AdministrativeContactName", rr[0]["AdministrativeContactName"].ToString());
                if (!rr[0]["AdministrativeContactName"].Equals(System.DBNull.Value))
                {
                    W.WriteStartElement("CollectionAddress");
                    this.writeXmlAddress(ref W, rr[0]["AdministrativeContactAgentURI"].ToString());
                    //System.Collections.Generic.Dictionary<string, string> DictAddress = this.Agent.UnitValues(rr[0]["AdministrativeContactAgentURI"].ToString());
                    //foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DictAddress)
                    //{
                    //    if (KV.Key == "ParentName" ||
                    //        KV.Key == "Country" ||
                    //        KV.Key == "City" ||
                    //        KV.Key == "PostalCode" ||
                    //        KV.Key == "Streetaddress" ||
                    //        KV.Key == "Address")
                    //    {
                    //        W.WriteElementString(KV.Key, KV.Value);
                    //    }
                    //}
                    W.WriteEndElement();
                }
                W.WriteEndElement();
            }
        }

        private void writeXmlStorage(ref System.Xml.XmlWriter W)
        {
            //if (this._dsLoan.CollectionStorage.Rows.Count > 0)
            if (this._DataSet.Tables["CollectionStorage"].Rows.Count > 0)
            {
                W.WriteStartElement("Storage");
                W.WriteStartElement("MaterialCategories");
                W.WriteStartElement("MaterialCategory");
                string MaterialInitial = "";
                string MaterialNew = "";
                //string Material = "";
                string AccessionNumberInitial = "";
                string AccessionNumberNew = "";
                string AccessionNumber = "";
                string IdentificationInitial = "";
                string IdentificationNew = "";
                string Identification = "";
                //bool WriteSpecimen = false;
                int Number = 0;
                int Counter = 0;
                //System.Data.DataRow[] rr = this._dsLoan.CollectionStorage.Select("LoanID = " + this._LoanID.ToString(), "MaterialCategory, AccessionNumber");
                System.Data.DataRow[] rr = this._DataSet.Tables["CollectionStorage"].Select("LoanID = " + this._LoanID.ToString(), "MaterialCategory, AccessionNumber");
                for (int i = 0; i < rr.Length; i++)
                {
                    AccessionNumberNew = rr[i]["AccessionNumber"].ToString();
                    IdentificationNew = this.LastIdentification(rr[i]["CollectionSpecimenID"].ToString());
                    MaterialNew = rr[i]["MaterialCategory"].ToString();

                    int? NewNumber = this.NumberFromAccessionNumber(rr[i]["AccessionNumber"].ToString());
                    if (NewNumber != null) NewNumber = System.Math.Abs((int)NewNumber);

                    if (i == 0) // Start of the Specimen
                    {
                        W.WriteElementString("Material", MaterialNew);
                        Counter += 1;
                        W.WriteElementString("Counter", Counter.ToString());
                        W.WriteStartElement("Specimens");

                        AccessionNumberInitial = AccessionNumberNew;
                        AccessionNumber = AccessionNumberNew;
                        if (AccessionNumberInitial.Length == 0) AccessionNumberInitial = "-";

                        IdentificationInitial = IdentificationNew;
                        Identification = IdentificationNew;

                        MaterialInitial = MaterialNew;
                        //Material = MaterialNew;
                        if (NewNumber != null)
                            Number = (int)NewNumber;
                    }
                    else // within the specimen list
                    {
                        if (
                            NewNumber == null || // the number could not be retrieved
                            NewNumber != Number + 1 || // the number is not in sequence
                            IdentificationInitial != IdentificationNew || // the identifiction differs
                            MaterialNew != MaterialInitial // a change of the material category
                            ) 
                        {
                            W.WriteStartElement("Specimen");
                            string AccNr = AccessionNumberInitial;
                            if (AccessionNumber != AccessionNumberInitial) AccNr += " - " + AccessionNumber;
                            W.WriteElementString("AccessionNumber", AccNr);
                            W.WriteElementString("Identification", IdentificationInitial);
                            Counter += 1;
                            W.WriteElementString("Counter", Counter.ToString());
                            AccessionNumberInitial = AccessionNumberNew;
                            IdentificationInitial = IdentificationNew;
                            W.WriteEndElement(); // Specimen
                        }
                        if (i + 1 == rr.Length)
                        {
                            W.WriteStartElement("Specimen");
                            string AccNr = AccessionNumberInitial;
                            if (AccessionNumberNew != AccessionNumberInitial) AccNr += " - " + AccessionNumberNew;
                            W.WriteElementString("AccessionNumber", AccNr);
                            W.WriteElementString("Identification", IdentificationInitial);
                            Counter += 1;
                            W.WriteElementString("Counter", Counter.ToString());
                            AccessionNumberInitial = AccessionNumberNew;
                            IdentificationInitial = IdentificationNew;
                            W.WriteEndElement(); // Specimen
                        }
                        if (NewNumber != null)
                            Number = (int)NewNumber;
                        else
                            Number = 0;
                        AccessionNumber = AccessionNumberNew;
                        //Material = MaterialNew;
                    }
                    if (MaterialInitial != MaterialNew) // Change of the material category
                    {
                        //if (MaterialInitial.Length > 0) W.WriteEndElement();
                        MaterialInitial = MaterialNew;
                        W.WriteEndElement(); // Specimens for previous material category
                        W.WriteEndElement(); // MaterialCategory
                        W.WriteStartElement("MaterialCategory");
                        W.WriteElementString("Material", MaterialInitial);
                        Counter += 1;
                        W.WriteElementString("Counter", Counter.ToString());
                        W.WriteStartElement("Specimens");
                    }
                }
                W.WriteEndElement(); // Specimens
                W.WriteEndElement(); // MaterialCategory
                W.WriteEndElement(); // MaterialCategories
                W.WriteEndElement(); // Storage
            }
        }

        private string LastIdentification(string CollectionSpecimenID)
        {
            string Identification = "";
            //System.Data.DataRow[] rrI = this._dsLoan.IdentificationUnit.Select("CollectionSpecimenID = " + CollectionSpecimenID, "DisplayOrder");
            System.Data.DataRow[] rrI = this._DataSet.Tables["IdentificationUnit"].Select("CollectionSpecimenID = " + CollectionSpecimenID, "DisplayOrder");
            if (rrI.Length > 0)
            {
                Identification = rrI[0]["LastIdentificationCache"].ToString();
            }
            return Identification;
        }

        /// <summary>
        /// Try to get a number out of an AccessionNumber
        /// </summary>
        /// <param name="AccessionNumber"></param>
        /// <returns></returns>
        private int? NumberFromAccessionNumber(string AccessionNumber)
        {
            int? Nr = null;
            string sNr = "";
            for (int i = AccessionNumber.Length - 1; i > 0; i--)
            {
                int TestNr;
                sNr = AccessionNumber.Substring(i);
                if (!int.TryParse(sNr, out TestNr))
                    break;
                else
                    Nr = TestNr;
            }
            return Nr;
        }

        #endregion
    }
}
