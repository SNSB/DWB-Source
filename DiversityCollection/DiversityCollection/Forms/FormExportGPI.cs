using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormExportGPI : Form
    {

        #region Parameter

        private string _ErrorMessage = "";
        private string _Message = "";
        private int _ErrorCount = 0;
        private System.Collections.Generic.List<int> _SpecimenIDs;
        private System.IO.FileInfo _XmlFile;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterSpecimen;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterSpecimenImage;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterAgent;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterProject;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterRelation;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterRelationInvers;

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterPart;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterProcessing;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterTransaction;

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterUnit;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterUnitInPart;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterIdentification;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterAnalysis;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterGeoAnalysis;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterGeoAnalysisGeography;

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEvent;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventGeography;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventImage;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterLocalisation;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterLocalisationSystem;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterProperty;

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesSuperiorList;

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeries;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesImage;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesEvent;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesSpecimen;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesSpecimenExtern;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesUnit;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesUnitExtern;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventSeriesLocalisation;

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterCollection;

        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        private DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                return this._FormFunctions;
            }
        }

        private System.Collections.Generic.List<string> _TypeStati;

        #endregion

        #region Construction

        public FormExportGPI(System.Collections.Generic.List<int> SpecimenIDs)
        {
            InitializeComponent();
            this._SpecimenIDs = SpecimenIDs;
            // online manual
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
            this.initForm();
        }

        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void initForm()
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                string SQL = "";
                this.progressBar.Maximum = this._SpecimenIDs.Count;
                this.progressBar.Minimum = 0;
                this.progressBar.Value = 0;
                string SpecimenIDs = "";
                string FirstSpecimenIDs = "";
                int ii = 0;
                foreach (int i in this._SpecimenIDs)
                {
                    if (SQL.Length > 0) SQL += ",";
                    SQL += i.ToString();
                    ii++;
                    if (ii < 100) FirstSpecimenIDs = SQL;
                }
                SpecimenIDs = SQL;
                SQL = "SELECT P.CollectionID " +
                    "FROM CollectionSpecimenPart AS p " +
                    "WHERE p.CollectionSpecimenID IN (" + SpecimenIDs + ")";
                System.Data.DataTable dtCollections = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                try
                {
                    ad.Fill(dtCollections);
                }
                catch { }
                if (dtCollections.Rows.Count == 0)
                {
                    SQL = "SELECT  TOP 1 p.CollectionID " +
                        "FROM CollectionSpecimenPart AS p " +
                        "WHERE p.CollectionSpecimenID IN (" + FirstSpecimenIDs + ")";
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(dtCollections);
                }
                SQL = "";
                System.Collections.Generic.List<string> CollectionIDs = new List<string>();
                foreach (System.Data.DataRow R in dtCollections.Rows)
                {
                    if (!CollectionIDs.Contains(R[0].ToString()))
                        CollectionIDs.Add(R[0].ToString());
                    //SQL += " " + R[0].ToString();
                }
                foreach (string s in CollectionIDs)
                {
                    SQL += " " + s;
                }
                SQL = "SELECT CollectionAcronym, CAST([Description] AS nvarchar(800)) AS [Description], CollectionID " +
                    "FROM dbo.CollectionHierarchyMulti('" + SQL.Trim() + "') AS H " +
                    "WHERE (NOT (CollectionAcronym IS NULL)) AND (NOT ([Description] IS NULL))";
                System.Data.DataTable dt = new DataTable();
                ad.SelectCommand.CommandText = SQL;
                ad.Fill(dt);
                this.comboBoxInstitution.DataSource = dt;
                this.comboBoxInstitution.DisplayMember = "Description";
                this.comboBoxInstitution.ValueMember = "CollectionAcronym";
                this.comboBoxInstitution.SelectedIndex = 0;

                this.comboBoxGazetteer.Items.Clear();
                foreach (DiversityWorkbench.DatabaseService DS in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].DatabaseServices())
                {
                    this.comboBoxGazetteer.Items.Add(DS.DisplayText);
                }

            }
            catch (System.Exception ex) { }
            finally{this.Cursor = System.Windows.Forms.Cursors.Default;}
        }

        #endregion

        #region Control events

        private void buttonCreateExport_Click(object sender, EventArgs e)
        {
            this.CreateExport();
            System.Uri URI = new Uri(this.textBoxExportFile.Text);
            this.webBrowser.Url = URI;
        }

        private void comboBoxInstitution_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int CollectionID = -1;
                if (this.comboBoxInstitution.SelectedItem.GetType() == typeof(System.Data.DataRowView))
                {
                    System.Data.DataRowView RV = (System.Data.DataRowView)this.comboBoxInstitution.SelectedItem;
                    this.textBoxInstitutionCode.Text = RV["CollectionAcronym"].ToString();
                    this.textBoxInstitutionName.Text = RV["Description"].ToString();
                    CollectionID = int.Parse(RV["CollectionID"].ToString());
                    string SQL = "SELECT AdministrativeContactAgentURI FROM Collection WHERE CollectionID = " + CollectionID.ToString();
                    string AgentURI = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (AgentURI.Length > 0)
                    {
                        DiversityWorkbench.ServerConnection S = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(AgentURI);
                        DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(S);
                        System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
                        int ID = int.Parse(DiversityWorkbench.WorkbenchUnit.getIDFromURI(AgentURI).ToString());
                        Values = A.UnitValues(ID);
                        if (Values.ContainsKey("City"))
                            this.textBoxCity.Text = Values["City"];
                        else this.textBoxCity.Text = "";
                    }
                    else this.textBoxCity.Text = "";
                }
                else
                {
                    this.textBoxInstitutionCode.Text = this.comboBoxInstitution.SelectedItem.ToString();
                    if (this.textBoxInstitutionCode.Text == "System.Data.DataRowView")
                    {
                        System.Data.DataRowView RV = (System.Data.DataRowView)this.comboBoxInstitution.SelectedItem;
                        this.textBoxInstitutionCode.Text = RV[0].ToString();
                    }
                    this.textBoxInstitutionName.Text = this.comboBoxInstitution.Text;
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show( ex.Message);
            }
        }

        private void checkBoxExportTypeNotes_CheckedChanged(object sender, EventArgs e)
        {
            this.checkBoxTypeNoteRestrictions.Enabled = checkBoxExportTypeNotes.Checked;
            this.textBoxTypeNoteRestrictions.Enabled = checkBoxExportTypeNotes.Checked;
        }

        private void buttonSaveAs_Click(object sender, EventArgs e)
        {
            try
            {
                string ErrorLogFile = Folder.Export() + "GPIexport_" + System.DateTime.Now.ToShortDateString().Replace("/", "_") + "_Error.txt";
                System.IO.StreamWriter swError;
                swError = new System.IO.StreamWriter(ErrorLogFile, false, System.Text.Encoding.UTF8);
                string[] stringSeparators = new string[] { "\r\n" };
                string[] Errors = this._ErrorMessage.Split(stringSeparators, StringSplitOptions.None);
                foreach (string s in Errors)
                    swError.WriteLine(s);
                swError.Flush();
                swError.Close();
                System.Windows.Forms.MessageBox.Show("Error list saved as " + ErrorLogFile);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region XML generation

        private void CreateExport()
        {
            System.Xml.XmlWriter W;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            this._ErrorCount = 0;
            settings.Encoding = System.Text.Encoding.UTF8;
            string FileDate = System.DateTime.Now.Year.ToString();
            if (System.DateTime.Now.Month < 10) FileDate += "0";
            FileDate += System.DateTime.Now.Month.ToString();
            if (System.DateTime.Now.Day < 10) FileDate += "0";
            FileDate += System.DateTime.Now.Day.ToString() + "_";
            if (System.DateTime.Now.Hour < 10) FileDate += "0";
            FileDate += System.DateTime.Now.Hour.ToString();
            if (System.DateTime.Now.Minute < 10) FileDate += "0";
            FileDate += System.DateTime.Now.Minute.ToString();
            if (System.DateTime.Now.Second < 10) FileDate += "0";
            FileDate += System.DateTime.Now.Second.ToString();
            this.textBoxExportFile.Text = Folder.Export() + "GPIexport_" + FileDate + ".XML";
            this._XmlFile = new System.IO.FileInfo(this.textBoxExportFile.Text);
            W = System.Xml.XmlWriter.Create(this._XmlFile.FullName, settings);
            try
            {
                W.WriteStartDocument();
                //<DataSet xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="http://plants.jstor.org/XSD/AfricanTypesv2.xsd">
                W.WriteStartElement("DataSet");
                W.WriteAttributeString("xsi", "noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "http://plants.jstor.org/XSD/AfricanTypesv2.xsd");
                W.WriteElementString("InstitutionCode", this.ReplaceSpecialSignsAndRemoveSpaces("InstitutionCode", this.textBoxInstitutionCode.Text));
                string Institution = this.textBoxInstitutionName.Text;
                if (this.textBoxCity.Text.Length > 0) 
                    Institution += ", " + this.textBoxCity.Text;
                W.WriteElementString("InstitutionName", this.ReplaceSpecialSignsAndRemoveSpaces("InstitutionName", Institution));
                string Date = System.DateTime.Now.Year.ToString() + "-";
                if (System.DateTime.Now.Month < 10) Date += "0";
                Date += System.DateTime.Now.Month.ToString() + "-";
                if (System.DateTime.Now.Day < 10) Date += "0";
                Date += System.DateTime.Now.Day.ToString();
                W.WriteElementString("DateSupplied", Date);
                this.writeUser(W);
                this.progressBar.Value = 0;
                this._ErrorMessage = "";
                this._Message = "";
                foreach (int i in this._SpecimenIDs)
                {
                    this.WriteUnit(i, W);
                    if (this.progressBar.Value < this.progressBar.Maximum)
                        this.progressBar.Value++;
                }
                W.WriteFullEndElement();  // Dataset
                W.WriteEndDocument();
                W.Flush();
                W.Close();
                if (this._ErrorMessage.Length > 0)
                {
                    this._ErrorMessage = "List of " + this._ErrorCount.ToString() + " errors that occured during the export:\r\n\r\n" + _ErrorMessage; ;
                    this.textBoxErrors.Text = this._ErrorMessage;
                    this.textBoxErrors.Visible = true;
                    this.buttonSaveAs.Visible = true;
                }
                else
                {
                    this.textBoxErrors.Visible = false;
                    this.buttonSaveAs.Visible = false;
                }
                if (this._Message.Length > 0)
                {
                    this._Message = "List of messages that occured during the export:\r\n\r\n" + _Message; ;
                    this.textBoxMessages.Text = this._Message;
                    this.textBoxMessages.Visible = true;
                    this.labelMessages.Visible = true;
                }
                else
                {
                    this.textBoxMessages.Visible = false;
                    this.labelMessages.Visible = false;
                }
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

        private void WriteUnit(int SpecimenID, System.Xml.XmlWriter W)
        {
            try
            {
                this.fillSpecimen(SpecimenID);

                W.WriteStartElement("Unit");
                W.WriteElementString("UnitID", this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["AccessionNumber"].ToString().Replace("-", ""));

                string SQL = "SELECT CONVERT( nvarchar(10), LogUpdatedWhen,126) FROM CollectionSpecimen C WHERE C.CollectionSpecimenID = " + SpecimenID.ToString();
                string Date = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL).ToString();
                W.WriteElementString("DateLastModified", Date);

                this.writeIdentification(SpecimenID, W);

                this.writeCollectors(W);

                this.writeCollectionDate(W);

                if (!this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CountryCache"].Equals(System.DBNull.Value) &&
                    this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CountryCache"].ToString().Length > 0)
                    W.WriteElementString("CountryName", this.ReplaceSpecialSignsAndRemoveSpaces("CountryName", this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CountryCache"].ToString()));

                string ISO2Letter = "ZZ";
                string Country = this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CountryCache"].ToString();
                Country = DiversityWorkbench.Forms.FormFunctions.SqlRemoveHyphens(Country);
                ISO2Letter = this.IsoLetterCode(Country);
                W.WriteElementString("ISO2Letter", ISO2Letter);

                string Locality = "";
                if (!this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["LocalityDescription"].Equals(System.DBNull.Value) &&
                    this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["LocalityDescription"].ToString().Length > 0)
                    Locality = this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["LocalityDescription"].ToString().Trim();
                if (!this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["HabitatDescription"].Equals(System.DBNull.Value) &&
                    this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["HabitatDescription"].ToString().Length > 0)
                {
                    if (Locality.Length > 0)
                    {
                        if (!Locality.EndsWith(".") && !Locality.EndsWith(";") && !Locality.EndsWith(",")) Locality += ".";
                        Locality += " ";
                    }
                    Locality += this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["HabitatDescription"].ToString().Trim();
                }
                if (Locality.Length > 0)
                    W.WriteElementString("Locality", this.ReplaceSpecialSignsAndRemoveSpaces("Locality", Locality));

                string Altitude = "";
                System.Data.DataRow[] rrAlt = this.dataSetCollectionSpecimen.CollectionEventLocalisation.Select("LocalisationSystemID = 4");
                if (rrAlt.Length > 0)
                {
                    try
                    {
                        if (!rrAlt[0]["LocationNotes"].Equals(System.DBNull.Value) &&
                            rrAlt[0]["LocationNotes"].ToString().Trim().Length > 0)
                        {
                            Altitude = rrAlt[0]["LocationNotes"].ToString().Trim();

                            if (Altitude.StartsWith("[Ori.val.: ") && Altitude.EndsWith("]"))
                            {
                                Altitude = Altitude.Replace("[Ori.val.: ", "").Replace("]", "").Replace(" - ", "-").Trim();
                                W.WriteElementString("Altitude", this.ReplaceSpecialSignsAndRemoveSpaces("Altitude", Altitude));
                            }
                            else
                            {
                                this.RegisterError("Altitude notes not exported: " + Altitude);
                            }
                        }
                        else
                        {
                            if (!rrAlt[0]["Location1"].Equals(System.DBNull.Value) &&
                                rrAlt[0]["Location1"].ToString().Trim().Length > 0)
                            {
                                Altitude = rrAlt[0]["Location1"].ToString().Trim();
                            }
                            if (!rrAlt[0]["Location2"].Equals(System.DBNull.Value) &&
                                rrAlt[0]["Location2"].ToString().Trim().Length > 0)
                            {
                                if (Altitude.Length > 0) Altitude += "-";
                                Altitude += rrAlt[0]["Location2"].ToString().Trim();
                            }
                            double m;
                            if (Altitude.Length > 0 && double.TryParse(Altitude, out m))
                                Altitude += " meters";
                            else if (Altitude.Length > 0 &&
                                double.TryParse(rrAlt[0]["Location1"].ToString().Trim(), out m) &&
                                double.TryParse(rrAlt[0]["Location2"].ToString().Trim(), out m))
                                Altitude += " meters";
                            else
                            {

                                if (Altitude.EndsWith(" m"))
                                    Altitude = Altitude.Substring(0, Altitude.Length - 1) + "meters";
                            }
                            if (!Altitude.EndsWith(" meters"))
                            {
                                string[] Alt = Altitude.Split(new char[] { ' ', '-' });
                                double dTest;
                                if (double.TryParse(Alt[Alt.Length - 1], out dTest))
                                    Altitude += " meters";
                                else
                                    this.RegisterError("Altitude not numeric");
                            }
                            W.WriteElementString("Altitude", this.ReplaceSpecialSignsAndRemoveSpaces("Altitude", Altitude));
                        }
                    }
                    catch (System.Exception ex) { }
                }

                string Notes = this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["OriginalNotes"].ToString();
                if (!this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["AdditionalNotes"].Equals(System.DBNull.Value) &&
                    this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["AdditionalNotes"].ToString().Length > 0 &&
                    this.checkBoxIncludeAdditionalNotes.Checked)
                {
                    if (Notes.Length > 0)
                        Notes += "; ";
                    Notes += this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["AdditionalNotes"].ToString();
                }
                if (Notes.Length > 0)
                    W.WriteElementString("Notes", this.ReplaceSpecialSignsAndRemoveSpaces("Notes", Notes));

                W.WriteEndElement(); //Unit
            }
            catch (System.Exception ex)
            {
            }
        }

        private void writeUser(System.Xml.XmlWriter W)
        {
            string SQL = "select USER_NAME()";
            string User = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            SQL = "SELECT     AgentURI " +
                "FROM         DiversityCollection.dbo.UserProxy AS u " +
                "WHERE     (LoginName = '" + User + "')";
            string AgentURI = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            DiversityWorkbench.ServerConnection S = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(AgentURI);
            DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(S);
            string PersonName = A.AgentName(AgentURI, "Tt. Gg Ii");
            if (PersonName.Length == 0)
                PersonName = DiversityCollection.LookupTable.CurrentUser;
            if (PersonName.Length == 0)
                PersonName = System.Environment.UserName;
            W.WriteElementString("PersonName", this.ReplaceSpecialSignsAndRemoveSpaces("PersonName", PersonName));
        }

        private void writeIdentification(int SpecimenID, System.Xml.XmlWriter W)
        {
            try
            {
                System.Data.DataRow[] RROrder = this.dataSetCollectionSpecimen.IdentificationUnit.Select("", "DisplayOrder");
                int MinDisplayOrder = int.Parse(RROrder[0]["DisplayOrder"].ToString());
                System.Data.DataRow[] RRUnit = this.dataSetCollectionSpecimen.IdentificationUnit.Select("DisplayOrder = " + MinDisplayOrder.ToString());
                int UnitID = int.Parse(RRUnit[0]["IdentificationUnitID"].ToString());

                string SQL = "SELECT MAX(p.StorageLocation) AS StoredUnder " +
                    "FROM IdentificationUnit AS u INNER JOIN " +
                    "CollectionSpecimenPart AS p ON u.CollectionSpecimenID = p.CollectionSpecimenID " +
                    "WHERE u.CollectionSpecimenID = " + SpecimenID.ToString();
                if (this.checkBoxLastNameAsStorage.Checked)
                    SQL = "SELECT LastIdentificationCache FROM IdentificationUnit WHERE IdentificationUnitID = " + UnitID.ToString();
                string StoredUnder = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);

                string WhereClause = "CollectionSpecimenID = " + SpecimenID.ToString() +
                    " AND IdentificationUnitID = " + UnitID.ToString();
                if (this.radioButtonRestrictToTypeAndLast.Checked)
                    WhereClause += " AND (TaxonomicName = '" + RRUnit[0]["LastIdentificationCache"].ToString() + "'" +
                    " OR TypeStatus <> '')";
                if (this.checkBoxRestrictToLinkedNames.Checked)
                    WhereClause += " AND NOT NameURI IS NULL";
                System.Data.DataRow[] rrIdent = this.dataSetCollectionSpecimen.Identification.Select(WhereClause, "IdentificationSequence DESC");
                int StoredUnderNames = 0;
                if (rrIdent.Length > 0)
                {
                    System.Collections.Generic.Dictionary<string, System.Data.DataRow> DictNames = new Dictionary<string, DataRow>();
                    foreach (System.Data.DataRow Rident in rrIdent)
                    {
                        if (!DictNames.ContainsKey(Rident["TaxonomicName"].ToString()))
                            DictNames.Add(Rident["TaxonomicName"].ToString(), Rident);
                        else
                        {
                            if (!Rident["TypeStatus"].Equals(System.DBNull.Value))
                            {
                                DictNames.Remove(Rident["TaxonomicName"].ToString());
                                DictNames.Add(Rident["TaxonomicName"].ToString(), Rident);
                            }
                        }
                    }

                    #region Version mit DiversityWorkbench.TaxonNameAnalysis

                    //foreach (System.Collections.Generic.KeyValuePair<string, System.Data.DataRow> KV in DictNames)
                    //{
                    //    W.WriteStartElement("Identification");
                    //    DiversityWorkbench.TaxonNameAnalysis T = new DiversityWorkbench.TaxonNameAnalysis(KV.Key);
                    //    //DiversityCollection.TaxonomicName T = new TaxonomicName(KV.Key, "", "");
                    //    if (T.Message.Length > 0)
                    //    {
                    //        this.RegisterError("Taxon:\t" + KV.Key + "\tcould not be processed.");
                    //        continue;
                    //    }
                    //    string StoredUnderNameElement = "false";
                    //    if (KV.Key == StoredUnder)
                    //    {
                    //        StoredUnderNameElement = "true";
                    //        StoredUnderNames++;
                    //    }
                    //    W.WriteAttributeString("StoredUnderName", StoredUnderNameElement);

                    //    W.WriteElementString("Family", this.RemoveSpaces(RRUnit[0]["FamilyCache"].ToString().ToUpper()));
                    //    if (RRUnit[0]["FamilyCache"].Equals(System.DBNull.Value) || RRUnit[0]["FamilyCache"].ToString().Length == 0)
                    //        this.RegisterError("Family is missing.");

                    //    if (T.TaxonNamePartDictionary.ContainsKey(DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.GenusOrSupragenericName))
                    //        W.WriteElementString("Genus", this.RemoveSpaces(T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.GenusOrSupragenericName]));
                    //    else
                    //    {
                    //        W.WriteElementString("Genus", "");
                    //        this.RegisterError("Genus is missing.");
                    //    }

                    //    if (T.TaxonNamePartDictionary.ContainsKey(DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.SpeciesEpithet))
                    //        W.WriteElementString("Species", this.RemoveSpaces(T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.SpeciesEpithet]));
                    //    else
                    //    {
                    //        W.WriteElementString("Species", "");
                    //        this.RegisterError("Species is missing.");
                    //    }

                    //    if (T.TaxonNamePartDictionary.ContainsKey(DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.BasionymAuthors) &&
                    //        !T.TaxonNamePartDictionary.ContainsKey(DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.InfraspecificEpithet))
                    //    {
                    //        W.WriteElementString("Author", this.Authors(T));
                    //    }
                    //    else W.WriteElementString("Author", "Not on Sheet");

                    //    if (T.TaxonNamePartDictionary.ContainsKey(DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.TaxonomicRank) &&
                    //        T.TaxonNamePartDictionary.ContainsKey(DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.InfraspecificEpithet))
                    //        W.WriteElementString("Infra-specificRank", T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.TaxonomicRank]);

                    //    if (T.TaxonNamePartDictionary.ContainsKey(DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.InfraspecificEpithet))
                    //        W.WriteElementString("Infra-specificEpithet", this.RemoveSpaces(T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.InfraspecificEpithet]));

                    //    if (T.TaxonNamePartDictionary.ContainsKey(DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.InfraspecificEpithet))
                    //    {
                    //        W.WriteElementString("Infra-specificAuthor", this.Authors(T));
                    //    }

                    //    if (KV.Value["ResponsibleName"].Equals(System.DBNull.Value) ||
                    //        KV.Value["ResponsibleName"].ToString().Length == 0)
                    //        W.WriteElementString("Identifier", "Not on Sheet");
                    //    else
                    //        W.WriteElementString("Identifier", this.RemoveSpaces(KV.Value["ResponsibleName"].ToString()));

                    //    W.WriteStartElement("IdentificationDate");
                    //    if (this.dataSetCollectionSpecimen.Identification.Rows.Count == 0 ||
                    //        (KV.Value["IdentificationDay"].Equals(System.DBNull.Value) &&
                    //        KV.Value["IdentificationMonth"].Equals(System.DBNull.Value) &&
                    //        KV.Value["IdentificationMonth"].Equals(System.DBNull.Value)))
                    //        W.WriteElementString("OtherText", "Not on Sheet");
                    //    else
                    //    {
                    //        if (!KV.Value["IdentificationDay"].Equals(System.DBNull.Value) &&
                    //            !KV.Value["IdentificationMonth"].Equals(System.DBNull.Value) &&
                    //            !KV.Value["IdentificationYear"].Equals(System.DBNull.Value))
                    //        {
                    //            string Day = KV.Value["IdentificationDay"].ToString();
                    //            if (Day.Length < 2) Day = "0" + Day;
                    //            W.WriteElementString("StartDay", Day);
                    //        }
                    //        if (!KV.Value["IdentificationMonth"].Equals(System.DBNull.Value) &&
                    //            !KV.Value["IdentificationYear"].Equals(System.DBNull.Value))
                    //        {
                    //            string Month = KV.Value["IdentificationMonth"].ToString();
                    //            if (Month.Length < 2) Month = "0" + Month;
                    //            W.WriteElementString("StartMonth", Month);
                    //        }
                    //        if (!KV.Value["IdentificationYear"].Equals(System.DBNull.Value))
                    //            W.WriteElementString("StartYear", KV.Value["IdentificationYear"].ToString());
                    //    }
                    //    W.WriteEndElement(); //IdentificationDate

                    //    if (!KV.Value["TypeStatus"].Equals(System.DBNull.Value))
                    //    {
                    //        string TypeStatus = KV.Value["TypeStatus"].ToString()[0].ToString().ToUpper() + KV.Value["TypeStatus"].ToString().Substring(1);
                    //        if (!this.TypeStati.Contains(TypeStatus))
                    //            TypeStatus = "Type";
                    //        if (TypeStatus.ToLower() == "not a type") TypeStatus = "-";
                    //        if (this.checkBoxExportTypeNotes.Checked &&
                    //            !KV.Value["TypeNotes"].Equals(System.DBNull.Value) &&
                    //            KV.Value["TypeNotes"].ToString().Trim().Length > 0)
                    //        {
                    //            string TypeNotes = KV.Value["TypeNotes"].ToString().Trim();
                    //            if (this.checkBoxTypeNoteRestrictions.Checked)
                    //            {
                    //                string[] TypeNoteRestrictions = this.textBoxTypeNoteRestrictions.Text.Split(new char[] { '|' });
                    //                foreach (string s in TypeNoteRestrictions)
                    //                {
                    //                    if (TypeNotes == s)
                    //                    {
                    //                        TypeStatus += " " + TypeNotes;
                    //                        break;
                    //                    }
                    //                }
                    //            }
                    //            else
                    //            {
                    //                TypeStatus += TypeNotes;
                    //            }
                    //        }
                    //        W.WriteElementString("TypeStatus", TypeStatus);
                    //    }

                    //    if (!KV.Value["IdentificationQualifier"].Equals(System.DBNull.Value) &&
                    //        KV.Value["IdentificationQualifier"].ToString().Length > 0)
                    //    {
                    //        string IdentificationQualifier = KV.Value["IdentificationQualifier"].ToString();
                    //        if (IdentificationQualifier.IndexOf("gen.") > -1)
                    //            W.WriteElementString("GenusQualifier", IdentificationQualifier.Replace("gen.", "").Trim());
                    //        else
                    //            W.WriteElementString("SpeciesQualifier", IdentificationQualifier.Replace("sp.", "").Trim());
                    //    }

                    //    //if (T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.InfraspecificEpithet))
                    //    //    W.WriteElementString("PlantNameCode", T.TaxonomicNameParts[TaxonomicName.NamePart.InfraspecificEpithet]);

                    //    W.WriteEndElement(); //Identification
                    //}
                    
                    #endregion

                    foreach (System.Collections.Generic.KeyValuePair<string, System.Data.DataRow> KV in DictNames)
                    {
                        W.WriteStartElement("Identification");

                        string NameURI = "";
                        if (!KV.Value["NameURI"].Equals(System.DBNull.Value))
                            NameURI= KV.Value["NameURI"].ToString();
                        DiversityWorkbench.ServerConnection S = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(NameURI);

                        if (!KV.Value["NameURI"].Equals(System.DBNull.Value) && S != null)
                        {
                            DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(S);
                            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
                            int ID = int.Parse(DiversityWorkbench.WorkbenchUnit.getIDFromURI(NameURI).ToString());
                            Values = T.UnitValues(ID);
                            string StoredUnderNameElement = "false";
                            string x = Values["Taxonomic name"].ToString().Trim();
                            string y = StoredUnder.Trim();
                            if (Values["Taxonomic name"].ToString().Trim() == StoredUnder.Trim())
                            {
                                StoredUnderNameElement = "true";
                                StoredUnderNames++;
                            }
                            W.WriteAttributeString("StoredUnderName", StoredUnderNameElement);

                            string Family = "";
                            if (!RRUnit[0]["FamilyCache"].Equals(System.DBNull.Value) || RRUnit[0]["FamilyCache"].ToString().Length > 0)
                                Family = this.RemoveSpaces(RRUnit[0]["FamilyCache"].ToString().ToUpper());
                            if (Family.Length == 0 && Values.ContainsKey("Family") && Values["Family"] != "")
                                Family = Values["Family"];
                            if (Family.Length == 0 && this.checkBoxMissingFamily.Checked && this.textBoxMissingFamily.Text.Length > 0)
                                Family = textBoxMissingFamily.Text;
                            if (Family.Length == 0)
                            {
                                this.RegisterError("Family is missing.");
                                Family = "not assigned";
                            }
                            W.WriteElementString("Family", this.ReplaceSpecialSignsAndRemoveSpaces("Family", Family));
                            

                            if (Values.ContainsKey("Genus") && Values["Genus"] != "")
                                W.WriteElementString("Genus", this.ReplaceSpecialSignsAndRemoveSpaces("Genus", Values["Genus"]));
                            else
                            {
                                W.WriteElementString("Genus", "");
                                this.RegisterError("Genus is missing.");
                            }

                            if (Values.ContainsKey("Species epithet") && Values["Species epithet"] != "")
                                W.WriteElementString("Species", this.ReplaceSpecialSignsAndRemoveSpaces("Species", Values["Species epithet"].Replace("ssp.", "subsp.")));
                            else
                            {
                                W.WriteElementString("Species", "");
                                this.RegisterError("Species is missing.");
                            }

                            if (Values.ContainsKey("Authors") &&
                                Values["Infraspecific epithet"] == "")
                            {
                                string Author = Values["Authors"].Trim();
                                if (Values["Nomenclatural status"] != "")
                                    Author += "; " + Values["Nomenclatural status"].Trim();
                                if (Author.Length > 0)
                                    W.WriteElementString("Author", this.ReplaceSpecialSignsAndRemoveSpaces("Author", Author));
                                else W.WriteElementString("Author", "Not on Sheet");
                            }
                            else W.WriteElementString("Author", "Not on Sheet");

                            if (Values["Infraspecific epithet"] != "")
                            {
                                string Rank = Values["Rank"];
                                if (Rank == "ssp.") Rank = "subsp.";
                                W.WriteElementString("Infra-specificRank", Rank);
                                W.WriteElementString("Infra-specificEpithet", this.ReplaceSpecialSignsAndRemoveSpaces("Infra-specificEpithet", Values["Infraspecific epithet"]));
                                if (Values.ContainsKey("Authors"))
                                {
                                    string Author = Values["Authors"].Trim();
                                    if (Values["Nomenclatural status"] != "")
                                        Author += "; " + Values["Nomenclatural status"].Trim();
                                    if (Author.Length > 0)
                                        W.WriteElementString("Infra-specificAuthor", this.ReplaceSpecialSignsAndRemoveSpaces("Infra-specificAuthor", Author));
                                    else W.WriteElementString("Infra-specificAuthor", "Not on Sheet");
                                }
                                else W.WriteElementString("Infra-specificAuthor", "Not on Sheet");
                            }
                        }
                        else
                        {
                            if (!KV.Value["NameURI"].Equals(System.DBNull.Value))
                            {
                                this.RegisterError("Identification linked to " + KV.Value["NameURI"].ToString() + ". Connection could not be established");
                            }
                            else
                                this.RegisterError("Identification not linked to thesaurus");
                            DiversityCollection.TaxonomicName T = new TaxonomicName(KV.Key, "", "");
                            if (T.ErrorMessage.Length > 0)
                            {
                                this.RegisterError("Taxon:\t" + KV.Key + "\tcould not be processed.");
                                continue;
                            }
                            string StoredUnderNameElement = "false";
                            if (KV.Key == StoredUnder)
                            {
                                StoredUnderNameElement = "true";
                                StoredUnderNames++;
                            }
                            W.WriteAttributeString("StoredUnderName", StoredUnderNameElement);

                            string Family = "";
                            if (!RRUnit[0]["FamilyCache"].Equals(System.DBNull.Value) || RRUnit[0]["FamilyCache"].ToString().Length > 0)
                                Family = RRUnit[0]["FamilyCache"].ToString().ToUpper();
                            if (Family.Length == 0)
                            {
                                Family = "not assigned";
                                this.RegisterError("Family is missing.");
                            }
                            W.WriteElementString("Family", this.ReplaceSpecialSignsAndRemoveSpaces("Family", Family));

                            if (T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.Genus))
                                W.WriteElementString("Genus", this.ReplaceSpecialSignsAndRemoveSpaces("Genus", T.TaxonomicNameParts[TaxonomicName.NamePart.Genus]));
                            else
                            {
                                W.WriteElementString("Genus", "");
                                this.RegisterError("Genus is missing.");
                            }

                            if (T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.SpeciesEpithet))
                                W.WriteElementString("Species", this.ReplaceSpecialSignsAndRemoveSpaces("Species", T.TaxonomicNameParts[TaxonomicName.NamePart.SpeciesEpithet]));
                            else
                            {
                                W.WriteElementString("Species", "");
                                this.RegisterError("Species is missing.");
                            }

                            if (T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.Authors) &&
                                !T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.InfraspecificEpithet))
                            {
                                string Author = T.TaxonomicNameParts[TaxonomicName.NamePart.Authors].Trim();
                                if (T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.NomenclaturalStatus))
                                    Author += "; " + T.TaxonomicNameParts[TaxonomicName.NamePart.NomenclaturalStatus].Trim();
                                W.WriteElementString("Author", this.ReplaceSpecialSignsAndRemoveSpaces("Author", Author));
                            }
                            else W.WriteElementString("Author", "Not on Sheet");

                            if (T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.TaxonomicRank) &&
                                T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.InfraspecificEpithet))
                                W.WriteElementString("Infra-specificRank", this.ReplaceSpecialSignsAndRemoveSpaces("Infra-specificRank", T.TaxonomicNameParts[TaxonomicName.NamePart.TaxonomicRank].Replace("ssp.", "subsp.")));

                            if (T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.InfraspecificEpithet))
                                W.WriteElementString("Infra-specificEpithet", this.ReplaceSpecialSignsAndRemoveSpaces("Infra-specificEpithet", T.TaxonomicNameParts[TaxonomicName.NamePart.InfraspecificEpithet]));

                            if (T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.InfraspecificEpithet))
                            {
                                if (T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.Authors))
                                {
                                    string Author = T.TaxonomicNameParts[TaxonomicName.NamePart.Authors].Trim();
                                    if (T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.NomenclaturalStatus))
                                        Author += "; " + T.TaxonomicNameParts[TaxonomicName.NamePart.NomenclaturalStatus].Trim();
                                    W.WriteElementString("Infra-specificAuthor", this.ReplaceSpecialSignsAndRemoveSpaces("Infra-specificAuthor", Author));
                                }
                            }
                        }

                        if (KV.Value["ResponsibleName"].Equals(System.DBNull.Value) ||
                            KV.Value["ResponsibleName"].ToString().Length == 0)
                            W.WriteElementString("Identifier", "Not on Sheet");
                        else
                            W.WriteElementString("Identifier", this.ReplaceSpecialSignsAndRemoveSpaces("Identifier", KV.Value["ResponsibleName"].ToString()));

                        W.WriteStartElement("IdentificationDate");
                        if (this.dataSetCollectionSpecimen.Identification.Rows.Count == 0 ||
                            (KV.Value["IdentificationDay"].Equals(System.DBNull.Value) &&
                            KV.Value["IdentificationMonth"].Equals(System.DBNull.Value) &&
                            KV.Value["IdentificationMonth"].Equals(System.DBNull.Value)))
                            W.WriteElementString("OtherText", "Not on Sheet");
                        else
                        {
                            if (!KV.Value["IdentificationDay"].Equals(System.DBNull.Value) &&
                                !KV.Value["IdentificationMonth"].Equals(System.DBNull.Value) &&
                                !KV.Value["IdentificationYear"].Equals(System.DBNull.Value))
                            {
                                string Day = KV.Value["IdentificationDay"].ToString();
                                if (Day.Length < 2) Day = "0" + Day;
                                W.WriteElementString("StartDay", Day);
                            }
                            if (!KV.Value["IdentificationMonth"].Equals(System.DBNull.Value) &&
                                !KV.Value["IdentificationYear"].Equals(System.DBNull.Value))
                            {
                                string Month = KV.Value["IdentificationMonth"].ToString();
                                if (Month.Length < 2) Month = "0" + Month;
                                W.WriteElementString("StartMonth", Month);
                            }
                            if (!KV.Value["IdentificationYear"].Equals(System.DBNull.Value))
                                W.WriteElementString("StartYear", KV.Value["IdentificationYear"].ToString());
                        }
                        W.WriteEndElement(); //IdentificationDate

                        if (!KV.Value["TypeStatus"].Equals(System.DBNull.Value))
                        {
                            string TypeStatus = KV.Value["TypeStatus"].ToString()[0].ToString().ToUpper() + KV.Value["TypeStatus"].ToString().Substring(1);
                            if (!this.TypeStati.Contains(TypeStatus.ToLower()))
                                TypeStatus = "Type";
                            if (TypeStatus.ToLower() == "not a type") TypeStatus = "-";
                            if (this.checkBoxExportTypeNotes.Checked &&
                                !KV.Value["TypeNotes"].Equals(System.DBNull.Value) &&
                                KV.Value["TypeNotes"].ToString().Trim().Length > 0)
                            {
                                string TypeNotes = KV.Value["TypeNotes"].ToString().Trim();
                                if (this.checkBoxTypeNoteRestrictions.Checked)
                                {
                                    string[] TypeNoteRestrictions = this.textBoxTypeNoteRestrictions.Text.Split(new char[] { '|' });
                                    foreach (string s in TypeNoteRestrictions)
                                    {
                                        if (TypeNotes == s)
                                        {
                                            TypeStatus += " " + TypeNotes;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    TypeStatus += TypeNotes;
                                }
                            }
                            W.WriteElementString("TypeStatus", this.ReplaceSpecialSignsAndRemoveSpaces("TypeStatus", TypeStatus));
                        }

                        if (!KV.Value["IdentificationQualifier"].Equals(System.DBNull.Value) &&
                            KV.Value["IdentificationQualifier"].ToString().Length > 0)
                        {
                            string IdentificationQualifier = KV.Value["IdentificationQualifier"].ToString();
                            if (IdentificationQualifier.IndexOf("gen.") > -1)
                                W.WriteElementString("GenusQualifier", this.ReplaceSpecialSignsAndRemoveSpaces("GenusQualifier", IdentificationQualifier.Replace("gen.", "").Trim()));
                            else
                                W.WriteElementString("SpeciesQualifier", this.ReplaceSpecialSignsAndRemoveSpaces("SpeciesQualifier", IdentificationQualifier.Replace("sp.", "").Trim()));
                        }

                        W.WriteEndElement(); //Identification
                    }

                    #region Alter Code
                    //foreach (System.Data.DataRow Rident in rrIdent)
                    //{
                    //    W.WriteStartElement("Identification");
                    //    DiversityCollection.TaxonomicName T = new TaxonomicName(Rident["TaxonomicName"].ToString(), "", "");
                    //    if (T.ErrorMessage.Length > 0)
                    //    {
                    //        this.RegisterError("Taxon:\t" + Rident["TaxonomicName"].ToString() + "\tcould not be processed.");
                    //        continue;
                    //    }
                    //    string StoredUnderNameElement = "false";
                    //    if (Rident["TaxonomicName"].ToString() == StoredUnder)
                    //    {
                    //        StoredUnderNameElement = "true";
                    //        StoredUnderNames++;
                    //    }
                    //    W.WriteAttributeString("StoredUnderName", StoredUnderNameElement);

                    //    W.WriteElementString("Family", this.RemoveSpaces(RRUnit[0]["FamilyCache"].ToString().ToUpper()));
                    //    if (RRUnit[0]["FamilyCache"].Equals(System.DBNull.Value) || RRUnit[0]["FamilyCache"].ToString().Length == 0)
                    //        this.RegisterError("Family is missing.");

                    //    if (T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.Genus))
                    //        W.WriteElementString("Genus", this.RemoveSpaces(T.TaxonomicNameParts[TaxonomicName.NamePart.Genus]));
                    //    else
                    //    {
                    //        W.WriteElementString("Genus", "");
                    //        this.RegisterError("Genus is missing.");
                    //    }

                    //    if (T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.SpeciesEpithet))
                    //        W.WriteElementString("Species", this.RemoveSpaces(T.TaxonomicNameParts[TaxonomicName.NamePart.SpeciesEpithet]));
                    //    else
                    //    {
                    //        W.WriteElementString("Species", "");
                    //        this.RegisterError("Species is missing.");
                    //    }

                    //    if (T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.Authors) &&
                    //        !T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.InfraspecificEpithet))
                    //    {
                    //        string Author = T.TaxonomicNameParts[TaxonomicName.NamePart.Authors].Trim();
                    //        if (T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.NomenclaturalStatus))
                    //            Author += "; " + T.TaxonomicNameParts[TaxonomicName.NamePart.NomenclaturalStatus].Trim();
                    //        W.WriteElementString("Author", this.RemoveSpaces(Author));
                    //    }
                    //    else W.WriteElementString("Author", "Not on Sheet");

                    //    if (T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.TaxonomicRank) &&
                    //        T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.InfraspecificEpithet))
                    //        W.WriteElementString("Infra-specificRank", T.TaxonomicNameParts[TaxonomicName.NamePart.TaxonomicRank]);

                    //    if (T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.InfraspecificEpithet))
                    //        W.WriteElementString("Infra-specificEpithet", this.RemoveSpaces(T.TaxonomicNameParts[TaxonomicName.NamePart.InfraspecificEpithet]));

                    //    if (T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.InfraspecificEpithet))
                    //    {
                    //        string Author = T.TaxonomicNameParts[TaxonomicName.NamePart.Authors].Trim();
                    //        if (T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.NomenclaturalStatus))
                    //            Author += "; " + T.TaxonomicNameParts[TaxonomicName.NamePart.NomenclaturalStatus].Trim();
                    //        W.WriteElementString("Infra-specificAuthor", this.RemoveSpaces(Author));
                    //    }

                    //    if (Rident["ResponsibleName"].Equals(System.DBNull.Value) ||
                    //        Rident["ResponsibleName"].ToString().Length == 0)
                    //        W.WriteElementString("Identifier", "Not on Sheet");
                    //    else
                    //        W.WriteElementString("Identifier", this.RemoveSpaces(Rident["ResponsibleName"].ToString()));

                    //    W.WriteStartElement("IdentificationDate");
                    //    if (this.dataSetCollectionSpecimen.Identification.Rows.Count == 0 ||
                    //        (Rident["IdentificationDay"].Equals(System.DBNull.Value) &&
                    //        Rident["IdentificationMonth"].Equals(System.DBNull.Value) &&
                    //        Rident["IdentificationMonth"].Equals(System.DBNull.Value)))
                    //        W.WriteElementString("OtherText", "Not on Sheet");
                    //    else
                    //    {
                    //        if (!Rident["IdentificationDay"].Equals(System.DBNull.Value) &&
                    //            !Rident["IdentificationMonth"].Equals(System.DBNull.Value) &&
                    //            !Rident["IdentificationYear"].Equals(System.DBNull.Value))
                    //        {
                    //            string Day = Rident["IdentificationDay"].ToString();
                    //            if (Day.Length < 2) Day = "0" + Day;
                    //            W.WriteElementString("StartDay", Day);
                    //        }
                    //        if (!Rident["IdentificationMonth"].Equals(System.DBNull.Value) &&
                    //            !Rident["IdentificationYear"].Equals(System.DBNull.Value))
                    //        {
                    //            string Month = Rident["IdentificationMonth"].ToString();
                    //            if (Month.Length < 2) Month = "0" + Month;
                    //            W.WriteElementString("StartMonth", Month);
                    //        }
                    //        if (!Rident["IdentificationYear"].Equals(System.DBNull.Value))
                    //            W.WriteElementString("StartYear", Rident["IdentificationYear"].ToString());
                    //    }
                    //    W.WriteEndElement(); //IdentificationDate

                    //    if (!Rident["TypeStatus"].Equals(System.DBNull.Value))
                    //    {
                    //        string TypeStatus = Rident["TypeStatus"].ToString()[0].ToString().ToUpper() + Rident["TypeStatus"].ToString().Substring(1);
                    //        if (!this.TypeStati.Contains(TypeStatus))
                    //            TypeStatus = "Type";
                    //        if (TypeStatus.ToLower() == "not a type") TypeStatus = "-";
                    //        if (this.checkBoxExportTypeNotes.Checked &&
                    //            !Rident["TypeNotes"].Equals(System.DBNull.Value) &&
                    //            Rident["TypeNotes"].ToString().Trim().Length > 0)
                    //        {
                    //            string TypeNotes = Rident["TypeNotes"].ToString().Trim();
                    //            if (this.checkBoxTypeNoteRestrictions.Checked)
                    //            {
                    //                string[] TypeNoteRestrictions = this.textBoxTypeNoteRestrictions.Text.Split(new char[] { '|' });
                    //                foreach (string s in TypeNoteRestrictions)
                    //                {
                    //                    if (TypeNotes == s)
                    //                    {
                    //                        TypeStatus += " " + TypeNotes;
                    //                        break;
                    //                    }
                    //                }
                    //            }
                    //            else
                    //            {
                    //                TypeStatus += TypeNotes;
                    //            }
                    //        }
                    //        W.WriteElementString("TypeStatus", TypeStatus);
                    //    }

                    //    if (!Rident["IdentificationQualifier"].Equals(System.DBNull.Value) &&
                    //        Rident["IdentificationQualifier"].ToString().Length > 0)
                    //    {
                    //        string IdentificationQualifier = Rident["IdentificationQualifier"].ToString();
                    //        if (IdentificationQualifier.IndexOf("gen.") > -1)
                    //            W.WriteElementString("GenusQualifier", IdentificationQualifier.Replace("gen.", "").Trim());
                    //        else
                    //            W.WriteElementString("SpeciesQualifier", IdentificationQualifier.Replace("sp.", "").Trim());
                    //    }

                    //if (T.TaxonomicNameParts.ContainsKey(TaxonomicName.NamePart.InfraspecificEpithet))
                    //    W.WriteElementString("PlantNameCode", T.TaxonomicNameParts[TaxonomicName.NamePart.InfraspecificEpithet]);

                    //    W.WriteEndElement(); //Identification
                    //}
	                #endregion                
                }
                if (StoredUnderNames != 1)
                    this.RegisterError(StoredUnderNames.ToString() + " 'stored under' names. Should be 1"); 
            }
            catch (Exception ex)
            {
            }
        }

        private void writeIdentificationFromValues(System.Collections.Generic.Dictionary<string, string> Values, string StoredUnder, ref int StoredUnderNames, string Family, System.Xml.XmlWriter W)
        {
            try
            {
                string StoredUnderNameElement = "false";
                if (Values["Taxonomic name"].ToString() == StoredUnder)
                {
                    StoredUnderNameElement = "true";
                    StoredUnderNames++;
                }
                W.WriteAttributeString("StoredUnderName", StoredUnderNameElement);

                if (Family.Length == 0 && Values.ContainsKey("Family") && Values["Family"] != "")
                    Family = Values["Family"];
                if (Family.Length == 0)
                {
                    this.RegisterError("Family is missing.");
                    Family = "not assigned";
                }
                W.WriteElementString("Family", this.ReplaceSpecialSignsAndRemoveSpaces("Family", Family));


                if (Values.ContainsKey("Genus") && Values["Genus"] != "")
                    W.WriteElementString("Genus", this.ReplaceSpecialSignsAndRemoveSpaces("Genus", Values["Genus"]));
                else
                {
                    W.WriteElementString("Genus", "");
                    this.RegisterError("Genus is missing.");
                }

                if (Values.ContainsKey("Species epithet") && Values["Species epithet"] != "")
                    W.WriteElementString("Species", this.ReplaceSpecialSignsAndRemoveSpaces("Species", Values["Species epithet"].Replace("ssp.", "subsp.")));
                else
                {
                    W.WriteElementString("Species", "");
                    this.RegisterError("Species is missing.");
                }

                if (Values.ContainsKey("Authors") &&
                    Values["Infraspecific epithet"] == "")
                {
                    string Author = Values["Authors"].Trim();
                    if (Values["Nomenclatural status"] != "")
                        Author += "; " + Values["Nomenclatural status"].Trim();
                    if (Author.Length > 0)
                        W.WriteElementString("Author", this.ReplaceSpecialSignsAndRemoveSpaces("Author", Author));
                    else W.WriteElementString("Author", "Not on Sheet");
                }
                else W.WriteElementString("Author", "Not on Sheet");

                if (Values["Infraspecific epithet"] != "")
                {
                    string Rank = Values["Rank"];
                    if (Rank == "ssp.") Rank = "subsp.";
                    W.WriteElementString("Infra-specificRank", Rank);
                    W.WriteElementString("Infra-specificEpithet", this.ReplaceSpecialSignsAndRemoveSpaces("Infra-specificEpithet", Values["Infraspecific epithet"]));
                    if (Values.ContainsKey("Authors"))
                    {
                        string Author = Values["Authors"].Trim();
                        if (Values["Nomenclatural status"] != "")
                            Author += "; " + Values["Nomenclatural status"].Trim();
                        if (Author.Length > 0)
                            W.WriteElementString("Infra-specificAuthor", this.ReplaceSpecialSignsAndRemoveSpaces("Infra-specificAuthor", Author));
                        else W.WriteElementString("Infra-specificAuthor", "Not on Sheet");
                    }
                    else W.WriteElementString("Infra-specificAuthor", "Not on Sheet");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private string Authors(DiversityWorkbench.TaxonNameAnalysis T)
        {
            string Author = T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.BasionymAuthors].Trim();
            if (T.TaxonNamePartDictionary.ContainsKey(DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.CombiningAuthors))
                Author = "(" + Author + ") " + T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.CombiningAuthors];
            if (T.TaxonNamePartDictionary.ContainsKey(DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.NomenclaturalStatus))
            {
                Author += "; " + T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.NomenclaturalStatus].Trim();
            }
            this.RemoveSpaces(Author);
            return Author;
        }

        private void writeCollectors(System.Xml.XmlWriter W)
        {
            try
            {
                string NoCollectorString = "Not on Sheet";
                if (this.dataSetCollectionSpecimen.CollectionAgent.Rows.Count == 0)
                {
                    W.WriteElementString("Collectors", NoCollectorString);
                    W.WriteElementString("CollectorNumber", "s.n.");
                }
                else
                {
                    string[] MissingCollectorStrings = this.textBoxMissingCollector.Text.Split(new char[] { '|' });
                    System.Collections.Generic.List<string> MissingCollectorList = new List<string>();
                    if (MissingCollectorStrings.Length > 0)
                        foreach (string s in MissingCollectorStrings) MissingCollectorList.Add(s);
                    if (!MissingCollectorList.Contains("")) MissingCollectorList.Add("");
                    string Collectors = "";
                    string NumbersForAll = "";
                    string NumberForFirst = "";
                    bool HasMoreThanOneNumber = false;
                    bool HasNumbers = false;
                    for (int i = 0; i < this.dataSetCollectionSpecimen.CollectionAgent.Rows.Count; i++)
                    {
                        if (Collectors.Length > 0) Collectors += "; ";
                        Collectors += this.dataSetCollectionSpecimen.CollectionAgent.Rows[i]["CollectorsName"].ToString();

                        if (!this.dataSetCollectionSpecimen.CollectionAgent.Rows[i]["CollectorsNumber"].Equals(System.DBNull.Value) &&
                            this.dataSetCollectionSpecimen.CollectionAgent.Rows[i]["CollectorsNumber"].ToString().Length > 0)
                        {
                            if (NumbersForAll.Length > 0) NumbersForAll += ", ";
                            NumbersForAll += this.dataSetCollectionSpecimen.CollectionAgent.Rows[i]["CollectorsNumber"].ToString(); ;

                            if (HasNumbers) HasMoreThanOneNumber = true;
                            HasNumbers = true;
                        }
                        else
                        {
                            if (NumbersForAll.Length > 0) NumbersForAll += ", ";
                            NumbersForAll += "s.n.";
                        }
                        if (i == 0) NumberForFirst = NumbersForAll;
                    }
                    if (MissingCollectorList.Contains(Collectors))
                        W.WriteElementString("Collectors", NoCollectorString);
                    else if (Collectors.Length > 0)
                        W.WriteElementString("Collectors", this.ReplaceSpecialSignsAndRemoveSpaces("Collectors", Collectors));
                    else
                        W.WriteElementString("Collectors", NoCollectorString);

                    if (!HasNumbers)
                        W.WriteElementString("CollectorNumber", "s.n.");
                    else if (HasMoreThanOneNumber)
                        W.WriteElementString("CollectorNumber", this.ReplaceSpecialSignsAndRemoveSpaces("CollectorNumber", NumbersForAll));
                    else if (NumbersForAll.Length > 4 && HasNumbers && NumbersForAll.StartsWith("s.n."))
                        W.WriteElementString("CollectorNumber", this.ReplaceSpecialSignsAndRemoveSpaces("CollectorNumber", NumbersForAll));
                    else
                        W.WriteElementString("CollectorNumber", this.ReplaceSpecialSignsAndRemoveSpaces("CollectorNumber", NumberForFirst));
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void writeCollectionDate(System.Xml.XmlWriter W)
        {
            try
            {
                W.WriteStartElement("CollectionDate");
                if (this.dataSetCollectionSpecimen.CollectionEvent.Rows.Count == 0 ||
                    (this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionDay"].Equals(System.DBNull.Value) &&
                    this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionMonth"].Equals(System.DBNull.Value) &&
                    this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionYear"].Equals(System.DBNull.Value)))
                    W.WriteElementString("OtherText", "Not on Sheet");
                else
                {
                    bool HasDate = false;
                    if (!this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionDay"].Equals(System.DBNull.Value) &&
                        !this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionMonth"].Equals(System.DBNull.Value) &&
                        !this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionYear"].Equals(System.DBNull.Value))
                    {
                        string StartDay = this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionDay"].ToString();
                        if (StartDay.Length < 2) StartDay = "0" + StartDay;
                        W.WriteElementString("StartDay", StartDay);
                        HasDate = true;
                    }
                    if (!this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionMonth"].Equals(System.DBNull.Value) &&
                        !this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionYear"].Equals(System.DBNull.Value))
                    {
                        string StartMonth = this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionMonth"].ToString();
                        if (StartMonth.Length < 2) StartMonth = "0" + StartMonth;
                        W.WriteElementString("StartMonth", StartMonth);
                        HasDate = true;
                    }
                    if (!this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionYear"].Equals(System.DBNull.Value))
                    {
                        W.WriteElementString("StartYear", this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionYear"].ToString());
                        HasDate = true;
                    }

                    if (!this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionDateSupplement"].Equals(System.DBNull.Value) &&
                        this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionDateSupplement"].ToString().Length > 0)
                    {
                        string EndDate = this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionDateSupplement"].ToString();
                        string EndDay = "";
                        string EndMonth = "";
                        System.DateTime DT;
                        System.Globalization.CultureInfo culture;
                        System.Globalization.DateTimeStyles styles;
                        culture = new System.Globalization.CultureInfo("en-US");
                        styles = System.Globalization.DateTimeStyles.AdjustToUniversal | System.Globalization.DateTimeStyles.AssumeLocal;
                        if (System.DateTime.TryParse(EndDate, culture, styles, out DT))
                        {
                            EndDay = DT.Day.ToString();
                            if (EndDay.Length < 2) EndDay = "0" + EndDay;
                            W.WriteElementString("EndDay", EndDay);
                            EndMonth = DT.Month.ToString();
                            if (EndMonth.Length < 2) EndMonth = "0" + EndMonth;
                            W.WriteElementString("EndMonth", EndMonth);
                            W.WriteElementString("EndYear", DT.Year.ToString());
                            HasDate = true;
                        }
                        else
                        {
                            string[] EndDateParts = EndDate.Split(new char[] { '-' });
                            int Year = 0;
                            int Month = 0;
                            int Day = 0;
                            if (EndDateParts.Length == 3 &&
                                (EndDateParts[0].Length == 0 ||
                                int.TryParse(EndDateParts[0], out Year)) &&
                                (EndDateParts[1].Length == 0 ||
                                int.TryParse(EndDateParts[1], out Month)) &&
                                (EndDateParts[2].Length == 0 ||
                                int.TryParse(EndDateParts[2], out Day)))
                            {
                                bool IsValidDate = true;
                                if (Year == 0 || (Year > 0 && Year <= System.DateTime.Now.Year) &&
                                    Month == 0 || (Month > 0 && Month <= 12) &&
                                    Day == 0 || (Day > 0 && Day <= 31))
                                {
                                    if (Day > 0 && Month > 0 && Year > 0)
                                    {
                                        if (Day < 10) 
                                            EndDay = "0" + Day.ToString();
                                        else
                                            EndDay = Day.ToString();
                                        W.WriteElementString("EndDay", EndDay);
                                        HasDate = true;
                                    }
                                    if (Month > 0 && Year > 0)
                                    {
                                        if (Month < 10)
                                            EndMonth = "0" + Month.ToString();
                                        else
                                            EndMonth = Month.ToString();
                                        W.WriteElementString("EndMonth", EndMonth);
                                        HasDate = true;
                                    }
                                    if (Year > 0)
                                    {
                                        W.WriteElementString("EndYear", Year.ToString());
                                        HasDate = true;
                                    }
                                }
                            }
                        }
                    }
                    if (!HasDate)
                    {
                        W.WriteElementString("OtherText", "Not on Sheet");
                    }
                }
                W.WriteEndElement(); //CollectionDate
            }
            catch (Exception ex)
            {
            }
        }

        #region Auxillary

        private void RegisterError(string Error)
        {
            if (this._ErrorMessage.Length > 0)
                this._ErrorMessage += "\r\n";
            this._ErrorMessage += "Accession number: " + this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["AccessionNumber"].ToString() +
                "\t" + Error;
            this._ErrorCount++;
        }

        private void RegisterMessage(string Message)
        {
            if (this._Message.Length > 0)
                this._Message += "\r\n";
            this._Message += "Accession number: " + this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["AccessionNumber"].ToString() +
                "\t" + Message;
        }

        private string RemoveSpaces(string Text)
        {
            try
            {
                while (Text.IndexOf("  ") > -1)
                    Text = Text.Replace("  ", " ");
                Text = Text.Trim();
                return Text;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return Text;
        }

        private string ReplaceSpecialSignsAndRemoveSpaces(string Field, string Text)
        {
            try
            {
                while (Text.IndexOf("  ") > -1)
                    Text = Text.Replace("  ", " ");
                Text = Text.Trim();
                if (Text.IndexOf("&") > -1)
                {
                    Text = Text.Replace("&", "&amp;");
                    this.RegisterMessage("& in " + Field + " replaced by &amp;");
                }
                if (Text.IndexOf("<") > -1)
                {
                    Text = Text.Replace("<", "&lt;");
                    this.RegisterMessage("< in " + Field + " replaced by &lt;");
                }
                if (Text.IndexOf(">") > -1)
                {
                    Text = Text.Replace(">", "&gt;");
                    this.RegisterMessage("> in " + Field + " replaced by &gt;");
                }
                if (Text.IndexOf("\"") > -1)
                {
                    Text = Text.Replace("\"", "&quot;");
                    this.RegisterMessage("\" in " + Field + " replaced by &quot;");
                }
                if (Text.IndexOf("'") > -1)
                {
                    Text = Text.Replace("'", "&apos;");
                    this.RegisterMessage("' in " + Field + " replaced by &apos;");
                }
                if (Text.IndexOf("´") > -1)
                {
                    Text = Text.Replace("´", "&apos;");
                    this.RegisterMessage("´ in " + Field + " replaced by &apos;");
                }
                if (Text.IndexOf("`") > -1)
                {
                    Text = Text.Replace("`", "&apos;");
                    this.RegisterMessage("` in " + Field + " replaced by &apos;");
                }

            }
            catch {}
            return Text;
        }

        public System.Collections.Generic.List<string> TypeStati
        {
            get
            {
                if (this._TypeStati == null)
                {
                    this._TypeStati = new List<string>();
                    this._TypeStati.Add("type");
                    this._TypeStati.Add("holotype");
                    this._TypeStati.Add("isoepitype");
                    this._TypeStati.Add("isolectotype");
                    this._TypeStati.Add("isoneotype");
                    this._TypeStati.Add("isoparatype");
                    this._TypeStati.Add("isosyntype");
                    this._TypeStati.Add("epitype");
                    this._TypeStati.Add("lectotype");
                    this._TypeStati.Add("neotype");
                    this._TypeStati.Add("paratype");
                    this._TypeStati.Add("syntype");
                    this._TypeStati.Add("isotype");
                    this._TypeStati.Add("original material");
                    this._TypeStati.Add("not a type");
                }
                return _TypeStati;
            }
        }
        
        #endregion

        #endregion

        #region Datahandling

        private void fillSpecimen(int SpecimenID)
        {
            try
            {
                this.dataSetCollectionSpecimen.Clear();

                string WhereClause = " WHERE CollectionSpecimenID = " + SpecimenID.ToString() +
                    " AND CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenID_UserAvailable)";

                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSpecimen, DiversityCollection.CollectionSpecimen.SqlSpecimen + WhereClause, this.dataSetCollectionSpecimen.CollectionSpecimen);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterAgent, DiversityCollection.CollectionSpecimen.SqlAgent + WhereClause + " ORDER BY CollectorsSequence", this.dataSetCollectionSpecimen.CollectionAgent);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterProject, DiversityCollection.CollectionSpecimen.SqlProject + WhereClause + " ORDER BY ProjectID", this.dataSetCollectionSpecimen.CollectionProject);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSpecimenImage, DiversityCollection.CollectionSpecimen.SqlSpecimenImage + WhereClause + "ORDER BY LogCreatedWhen DESC", this.dataSetCollectionSpecimen.CollectionSpecimenImage);

                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterRelation, DiversityCollection.CollectionSpecimen.SqlRelation + WhereClause + " ORDER BY RelatedSpecimenDisplayText", this.dataSetCollectionSpecimen.CollectionSpecimenRelation);
                string SQL = "SELECT R.CollectionSpecimenID, R.RelatedSpecimenURI, S.AccessionNumber AS RelatedSpecimenDisplayText, R.RelationType, R.RelatedSpecimenCollectionID, " +
                    "R.RelatedSpecimenDescription, R.Notes, R.IsInternalRelationCache  " +
                    "FROM CollectionSpecimenRelation R, CollectionSpecimen S  " +
                    "WHERE (R.CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenID_UserAvailable))  " +
                    "AND (S.CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenID_UserAvailable))  " +
                    "AND (R.IsInternalRelationCache = 1)  " +
                    "AND rtrim(substring(R.RelatedSpecimenURI, len(dbo.BaseURL()) + 1, 255)) = '" + SpecimenID.ToString() + "'  " +
                    "AND S.CollectionSpecimenID = R.CollectionSpecimenID " +
                    "ORDER BY RelatedSpecimenDisplayText ";
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterRelationInvers, SQL, this.dataSetCollectionSpecimen.CollectionSpecimenRelationInvers);

                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterStorage, DiversityCollection.CollectionSpecimen.SqlStorage + WhereClause, this.dataSetCollectionSpecimen.CollectionStorage);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterPart, DiversityCollection.CollectionSpecimen.SqlPart + WhereClause, this.dataSetCollectionSpecimen.CollectionSpecimenPart);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterProcessing, DiversityCollection.CollectionSpecimen.SqlProcessing + WhereClause + " ORDER BY ProcessingDate, ProcessingID", this.dataSetCollectionSpecimen.CollectionSpecimenProcessing);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterTransaction, DiversityCollection.CollectionSpecimen.SqlTransaction + WhereClause, this.dataSetCollectionSpecimen.CollectionSpecimenTransaction);

                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterUnit, DiversityCollection.CollectionSpecimen.SqlUnit + WhereClause, this.dataSetCollectionSpecimen.IdentificationUnit);

                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterUnitInPart, DiversityCollection.CollectionSpecimen.SqlUnitInPart + WhereClause, this.dataSetCollectionSpecimen.IdentificationUnitInPart);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterIdentification, DiversityCollection.CollectionSpecimen.SqlIdentification + WhereClause + " ORDER BY IdentificationSequence", this.dataSetCollectionSpecimen.Identification);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterAnalysis, DiversityCollection.CollectionSpecimen.SqlAnalysis + WhereClause, this.dataSetCollectionSpecimen.IdentificationUnitAnalysis);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterGeoAnalysis, DiversityCollection.CollectionSpecimen.SqlGeoAnalysis + WhereClause + " ORDER BY AnalysisDate", this.dataSetCollectionSpecimen.IdentificationUnitGeoAnalysis);

                try
                {
                    SQL = "SELECT CollectionSpecimenID, IdentificationUnitID, AnalysisDate, Geography.ToString() AS GeographyAsString, Geometry.ToString() AS GeometryAsString " +
                        "FROM IdentificationUnitGeoAnalysis " + WhereClause + " ORDER BY AnalysisDate";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(this.dataSetCollectionSpecimen.IdentificationUnitGeoAnalysisGeo);
                }
                catch { }

                // Event
                if (!this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].Equals(System.DBNull.Value))
                {
                    try
                    {
                        int EventID = int.Parse(this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].ToString());
                        WhereClause = " WHERE CollectionEventID = " + EventID.ToString();

                        this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterEvent, DiversityCollection.CollectionSpecimen.SqlEvent + WhereClause, this.dataSetCollectionSpecimen.CollectionEvent);
                        this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterLocalisation, DiversityCollection.CollectionSpecimen.SqlEventLocalisation + WhereClause, this.dataSetCollectionSpecimen.CollectionEventLocalisation);
                        this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterProperty, DiversityCollection.CollectionSpecimen.SqlEventProperty + WhereClause, this.dataSetCollectionSpecimen.CollectionEventProperty);
                        try
                        {
                            SQL = "SELECT CollectionEventID, LocalisationSystemID, Geography.ToString() AS GeographyAsString, Geography.EnvelopeCenter().ToString() AS GeographyEnvelopeCenter " +
                                "FROM CollectionEventLocalisation " + WhereClause + " AND NOT Geography IS NULL AND LocalisationSystemID = 8 ";
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                            ad.SelectCommand.CommandText = SQL;
                            ad.Fill(this.dataSetCollectionSpecimen.CollectionEventLocalisationGeo);
                        }
                        catch { }
                    }
                    catch { }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error setting the specimen", System.Windows.Forms.MessageBoxButtons.OK);
            }
        }

        #endregion

        #region Gazetteer

        string _GazetteerBaseURL;
        
        private string GazetteerBaseURL
        {
            get
            {
                if (this._GazetteerBaseURL == null)
                {
                    DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].DataBaseURIs().TryGetValue(this.comboBoxGazetteer.Text, out this._GazetteerBaseURL);
                }
                return _GazetteerBaseURL;
            }
        }

        private string IsoLetterCode(string Country)
        {
            if (Country == null || Country.Length == 0)
                return "ZZ";
            string ISO2Letter = "";
            try
            {
                if (this.comboBoxGazetteer.SelectedItem == null)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a gazetteer for the retrieval of the country ISO codes");
                    return "ZZ";
                }
                string DB = this.comboBoxGazetteer.SelectedItem.ToString();// = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityScientificTerms"].DatabaseList();
                string ConnectionString = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnectionList()[DB].ConnectionString;
                string Database = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnectionList()[DB].DatabaseName;
                if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnectionList()[DB].LinkedServer.Length > 0)
                    Database = "[" + DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnectionList()[DB].LinkedServer + "]." + Database;
                string SQL = "SELECT TOP (1) I.Name " +
                    "FROM " + Database + ".dbo.GeoName AS N INNER JOIN " +
                    " " + Database + ".dbo.GeoName AS I ON N.PlaceID = I.PlaceID " +
                    "WHERE (N.Name = '" + Country.Trim() + "') AND (I.LanguageCode = 'ISO 2-letter') ";

                string Message = "";
                ISO2Letter = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
                if (Message.Length > 0)
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(Message);

                //string SQL = "SELECT  MIN(I.Name) " +
                //    "FROM " + Database + ".dbo.GeoName AS N INNER JOIN " +
                //    Database + ".dbo.ViewGeoPlace AS P ON N.PlaceID = P.PlaceID INNER JOIN " +
                //    Database + ".dbo.GeoName AS I ON P.PlaceID = I.PlaceID " +
                //    "WHERE  /*   (P.PlaceType = 'nation') AND*/ (I.LanguageCode = 'ISO 2-letter') AND (N.Name = '" + Country + "')";

                //Microsoft.Data.SqlClient.SqlConnection conGaz = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
                //Microsoft.Data.SqlClient.SqlCommand cGaz = new Microsoft.Data.SqlClient.SqlCommand(SQL, conGaz);
                //conGaz.Open();
                //ISO2Letter = cGaz.ExecuteScalar().ToString();
                //conGaz.Close();
                //conGaz.Dispose();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            if (ISO2Letter == "NULL" || ISO2Letter.Length == 0)
                ISO2Letter = "ZZ";

            return ISO2Letter;
        }

        #endregion

        #region Manual

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
