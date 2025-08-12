using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityCollection
{
    class XmlExportABCD
    {
        #region Parameter
        private string _MaterialCategory;
        private string _Collection = "";
        private string _CollectionOwner = "";
        private string _CollectionOwnerAcronym;
        private string _BaseURL = "";
        private int _CollectionID = -1;
        private int? _CollectionSpecimenID = null;
        private int? _CollectionEventID = null;
        private int? _IdentificationUnitID = null;
        private DiversityCollection.Datasets.DataSetCollectionEventSeries dataSetCollectionEventSeries;
        private DiversityCollection.Datasets.DataSetCollectionSpecimen dataSetCollectionSpecimen;

        private System.IO.FileInfo _XmlFile;
        private System.Xml.XmlWriter W;
        private System.Data.DataTable _dtCollection;
        private System.Data.DataTable _dtLocalisationSystem;
        private System.Data.DataTable _dtAnalysis;
        private System.Data.DataTable _dtUserProxy;

        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;

        #endregion

        #region Construction

        public XmlExportABCD(
            string XmlFile,
            int CollectionID,
            DiversityCollection.Datasets.DataSetCollectionSpecimen DsSpecimen
            , DiversityCollection.Datasets.DataSetCollectionEventSeries DsEventSeries)
        {
            this._CollectionID = CollectionID;
            this.setCollectionOwner();
            this.setCollection();
            this.dataSetCollectionEventSeries = DsEventSeries;
            this.dataSetCollectionSpecimen = DsSpecimen;
            string Path = "";
            if (XmlFile.Length > 0)
                Path = XmlFile;
            else
            {
                string Date = System.DateTime.Now.Year.ToString();
                if (System.DateTime.Now.Month < 10) Date += "0";
                Date += System.DateTime.Now.Month.ToString();
                if (System.DateTime.Now.Day < 10) Date += "0";
                Date += System.DateTime.Now.Day.ToString() + "_";
                if (System.DateTime.Now.Hour < 10) Date += "0";
                Date += System.DateTime.Now.Hour.ToString();
                if (System.DateTime.Now.Minute < 10) Date += "0";
                Date += System.DateTime.Now.Minute.ToString();
                if (System.DateTime.Now.Second < 10) Date += "0";
                Date += System.DateTime.Now.Second.ToString();
                if (DsSpecimen.CollectionSpecimen.Rows.Count == 1 && !DsSpecimen.CollectionSpecimen.Rows[0]["AccessionNumber"].Equals(System.DBNull.Value))
                {
                    Path = Folder.Export() + "ABCDexport_" + DsSpecimen.CollectionSpecimen.Rows[0]["AccessionNumber"].ToString() + "_" + Date + ".XML";
                }
                else
                {
                    Path = Folder.Export() + "ABCDexport_" + Date + ".XML";
                }
            }
            this._XmlFile = new System.IO.FileInfo(Path);
        }
        #endregion

        #region XML Export

        public string createXmlFromDatasets(
           string DatasetGUID,
           System.Collections.Generic.List<string> TechnicalContacts,
           System.Collections.Generic.List<string> ContentContacts,
           System.Collections.Generic.List<string> OtherProviders,
           System.Collections.Generic.Dictionary<string, string> Metadata)
        {
            System.Xml.XmlWriter W;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            W = System.Xml.XmlWriter.Create(this._XmlFile.FullName, settings);
            try
            {
                W.WriteStartDocument();
                W.WriteStartElement("Datasets");
                W.WriteStartElement("Dataset");
                if (DatasetGUID.Length > 0)
                    W.WriteElementString("DatasetGUID", DatasetGUID);
                this.writeTechnicalContacts(ref W, TechnicalContacts);
                this.writeContentContacts(ref W, ContentContacts);
                this.writeOtherProviders(ref W, OtherProviders);
                this.writeMetadata(ref W, Metadata);
                //this.writeOriginalSource(ref W);
                this.writeUnits(ref W);
                W.WriteEndElement();        //Datasets
                W.WriteFullEndElement();    //Dataset
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
            return this._XmlFile.FullName;
        }

        #region Technical informations
        private void writeTechnicalContacts(ref System.Xml.XmlWriter W, System.Collections.Generic.List<string> TechnicalContacts)
        {
            //DataSets/DataSet/TechnicalContacts/TechnicalContact
            W.WriteStartElement("TechnicalContacts");
            foreach (string s in TechnicalContacts)
                W.WriteElementString("TechnicalContact", s);
            W.WriteEndElement();        //TechnicalContacts
        }

        private void writeContentContacts(ref System.Xml.XmlWriter W, System.Collections.Generic.List<string> ContentContacts)
        {
            //DataSets/DataSet/ContentContacts/ContentContact
            W.WriteStartElement("ContentContacts");
            foreach (string s in ContentContacts)
                W.WriteElementString("ContentContact", s);
            W.WriteEndElement();        //ContentContacts
        }

        private void writeOtherProviders(ref System.Xml.XmlWriter W, System.Collections.Generic.List<string> OtherProviders)
        {
            //DataSets/DataSet/OtherProviders/OtherProvider
            if (OtherProviders.Count > 0)
            {
                W.WriteStartElement("OtherProviders");
                foreach (string s in OtherProviders)
                {
                    W.WriteElementString("OtherProvider", s);
                }
                W.WriteEndElement();
            }
        }

        private void writeMetadata(ref System.Xml.XmlWriter W, System.Collections.Generic.Dictionary<string, string> Metadata)
        {
            if (Metadata.Count > 0)
            {
                W.WriteStartElement("Metadata");
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Metadata)
                {
                    switch (KV.Key)
                    {
                        ///DataSets/DataSet/Metadata/Scope/TaxonomicTerms/TaxonomicTerm
                        case "Scope":
                            W.WriteStartElement("Scope");
                            W.WriteStartElement("TaxonomicTerms");
                            string[] Scopes = KV.Value.Split(new char[] { ' ' });
                            foreach (string s in Scopes)
                            {
                                //string Scope = s.Replace(",", "");
                                W.WriteElementString("TaxonomicTerm", s);
                            }
                            W.WriteEndElement();        //TaxonomicTerms
                            W.WriteEndElement();        //Scope
                            break;

                        ///DataSets/DataSet/Metadata/IconURI
                        ///DataSets/DataSet/Metadata/Version
                        case "IconURI":
                        case "Version":
                            W.WriteElementString(KV.Key, KV.Value);
                            break;

                    }
                }
                W.WriteEndElement();
            }
        }

        private void writeOriginalSource(ref System.Xml.XmlWriter W)
        {
            W.WriteStartElement("OriginalSource");
            W.WriteElementString("SourceInstitutionCode", this.CollectionOwner);
            W.WriteElementString("SourceInstitutionID", this.CollectionOwner);
            W.WriteEndElement();
        }

        private void writeDatasetDerivations(ref System.Xml.XmlWriter W, System.Collections.Generic.Dictionary<string, string> Metadata)
        {
        }
        
        #endregion

        private void writeUnits(ref System.Xml.XmlWriter W)
        {
            W.WriteStartElement("Units");
            foreach (System.Data.DataRow R in this.dataSetCollectionSpecimen.CollectionSpecimen.Rows)
            {
                // reset IDs
                this._CollectionSpecimenID = null;
                this._CollectionEventID = null;
                this._IdentificationUnitID = null;
                int SpecimenID = 0;
                int EventID = 0;
                if (int.TryParse(R["CollectionSpecimenID"].ToString(), out SpecimenID))
                {
                    this._CollectionSpecimenID = SpecimenID;
                    W.WriteStartElement("Unit");
                    ///DataSets/DataSet/Units/Unit/UnitGUID
                    W.WriteElementString("UnitGUID", "URN:catalog:" + this.CollectionOwnerAcronym + ":" + this.Collection + ":" + R["CollectionSpecimenID"].ToString());
                    ///DataSets/DataSet/Units/Unit/SourceInstitutionID
                    W.WriteElementString("SourceInstitutionID", this.CollectionOwner);
                    W.WriteElementString("UnitID", this._CollectionSpecimenID.ToString());
                    W.WriteElementString("UnitIDNumeric", this._CollectionSpecimenID.ToString());
                    W.WriteElementString("LastEditor", this.LastEditor(R));
                    W.WriteElementString("DateLastEdited", this.DateLastEdited(R));
                    if (int.TryParse(R["CollectionEventID"].ToString(), out EventID)) this._CollectionEventID = EventID;
                    this.writeGathering(ref W);
                    this.writeIdentifications(ref W);
                    W.WriteEndElement();
                }
            }
            W.WriteEndElement();
        }

        private void writeHerbariumUnit(ref System.Xml.XmlWriter W)
        {
            ///DataSets/DataSet/Units/Unit/HerbariumUnit/Exsiccatum
            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionEventLocalisation.Select("CollectionEventID = " + this._CollectionEventID.ToString() + " AND LocalisationSystemID = 7");
            if (rr.Length > 0)
            {
                W.WriteStartElement("NamedAreas");
                W.WriteElementString("NamedArea", rr[0]["Location1"].ToString());
                W.WriteEndElement();
            }
        }

        #region Event etc.
        private void writeGathering(ref System.Xml.XmlWriter W)
        {
            W.WriteStartElement("Gathering");
            if (this._CollectionEventID != null)
            {
                System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionEvent.Select("CollectionEventID = " + this._CollectionEventID.ToString());
                if (rr.Length > 0)
                {
                    if (!rr[0]["CollectionYear"].Equals(System.DBNull.Value))
                    {
                        string DateText = rr[0]["CollectionYear"].ToString();
                        if (!rr[0]["CollectionMonth"].Equals(System.DBNull.Value))
                            DateText = rr[0]["CollectionMonth"].ToString() + "." + DateText;
                        if (!rr[0]["CollectionDay"].Equals(System.DBNull.Value))
                            DateText = rr[0]["CollectionDay"].ToString() + "." + DateText;
                        W.WriteElementString("DateText", DateText);
                    }
                    if (!rr[0]["CollectingMethod"].Equals(System.DBNull.Value))
                    {
                        W.WriteElementString("Method", rr[0]["CollectingMethod"].ToString());
                    }
                    this.writeAltitude(ref W);
                    if (!rr[0]["CountryCache"].Equals(System.DBNull.Value))
                    {
                        W.WriteStartElement("Country");
                        W.WriteElementString("Name", rr[0]["CountryCache"].ToString());
                        W.WriteEndElement();
                    }
                    this.writeStratigraphy(ref W);
                }
            }
            this.writeAgents(ref W);
            W.WriteEndElement();
        }

        private void writeAltitude(ref System.Xml.XmlWriter W)
        {
            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionEventLocalisation.Select("CollectionEventID = " + this._CollectionEventID.ToString() + " AND NOT AverageAltitudeCache IS NULL");
            if (rr.Length > 0)
            {
                W.WriteStartElement("Altitude");
                W.WriteElementString("Measurement", rr[0]["AverageAltitudeCache"].ToString());
                W.WriteEndElement();
            }
        }

        private void writeNamedArea(ref System.Xml.XmlWriter W)
        {
            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionEventLocalisation.Select("CollectionEventID = " + this._CollectionEventID.ToString() + " AND LocalisationSystemID = 7");
            if (rr.Length > 0)
            {
                W.WriteStartElement("NamedAreas");
                W.WriteElementString("NamedArea", rr[0]["Location1"].ToString());
                W.WriteEndElement();
            }
        }

        private void writeStratigraphy(ref System.Xml.XmlWriter W)
        {
            System.Data.DataRow[] rrChrono = this.dataSetCollectionSpecimen.CollectionEventProperty.Select("CollectionEventID = " + this._CollectionEventID.ToString() + " AND PropertyID = 20");
            System.Data.DataRow[] rrLitho = this.dataSetCollectionSpecimen.CollectionEventProperty.Select("CollectionEventID = " + this._CollectionEventID.ToString() + " AND PropertyID = 30");
            if (rrChrono.Length > 0 || rrLitho.Length > 0)
            {
                W.WriteStartElement("Stratigraphy");
                if (rrChrono.Length > 0)
                {
                    W.WriteStartElement("ChronostratigrahicTerms");
                    foreach (System.Data.DataRow r in rrChrono)
                    {
                        W.WriteElementString("ChronostratigrahicTerm", r["DisplayText"].ToString());
                    }
                    W.WriteEndElement();
                }
                if (rrLitho.Length > 0)
                {
                    W.WriteStartElement("LithostratigrahicTerms");
                    foreach (System.Data.DataRow r in rrLitho)
                    {
                        W.WriteElementString("LithostratigrahicTerm", r["DisplayText"].ToString());
                    }
                    W.WriteEndElement();
                }
                W.WriteEndElement();
            }
        }
        
        #endregion

        private void writeAgents(ref System.Xml.XmlWriter W)
        {
            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionAgent.Select("CollectionSpecimenID = " + this._CollectionSpecimenID.ToString(), "CollectorsSequence");
            if (rr.Length > 0)
            {
                W.WriteStartElement("Agents");
                for (int i = 0; i < rr.Length; i++)
                {
                    W.WriteStartElement("GatheringAgent");
                    W.WriteElementString("AgentText", rr[i]["CollectorsName"].ToString());
                    int Sequence = i + 1;
                    W.WriteElementString("sequence", Sequence.ToString());
                    W.WriteEndElement();
                }
                W.WriteEndElement();
            }
        }

        private void writeIdentifications(ref System.Xml.XmlWriter W)
        {

            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.IdentificationUnit.Select("CollectionSpecimenID = " + this._CollectionSpecimenID.ToString(), "DisplayOrder");
            if (rr.Length > 0)
            {
                System.Data.DataRow R = rr[0];
                int IdentificationUnitID = int.Parse(R["IdentificationUnitID"].ToString());
                System.Data.DataRow[] RR = this.dataSetCollectionSpecimen.Identification.Select("IdentificationUnitID = " + IdentificationUnitID.ToString(), "IdentificationSequence");
                W.WriteStartElement("Identifications");
                for (int i = 0; i < RR.Length; i++)
                {
                    if (!RR[i]["TaxonomicName"].Equals(System.DBNull.Value) && RR[i]["TaxonomicName"].ToString().Length > 0)
                    {
                        W.WriteStartElement("Identification");
                        W.WriteStartElement("Result");
                        W.WriteStartElement("TaxonIdentified");
                        if ((!R["FamilyCache"].Equals(System.DBNull.Value)
                            && R["FamilyCache"].ToString().Length > 0)
                            || (!R["OrderCache"].Equals(System.DBNull.Value)
                            && R["OrderCache"].ToString().Length > 0))
                        {
                            W.WriteStartElement("HigherTaxa");
                            if (!R["OrderCache"].Equals(System.DBNull.Value)
                                && R["OrderCache"].ToString().Length > 0)
                            {
                                W.WriteStartElement("HigherTaxon");
                                W.WriteElementString("HigherTaxonName", R["OrderCache"].ToString());
                                W.WriteElementString("HigherTaxonRank", "Order");
                                W.WriteEndElement();
                            }
                            if (!R["FamilyCache"].Equals(System.DBNull.Value)
                                && R["FamilyCache"].ToString().Length > 0)
                            {
                                W.WriteStartElement("HigherTaxon");
                                W.WriteElementString("HigherTaxonName", R["FamilyCache"].ToString());
                                W.WriteElementString("HigherTaxonRank", "Family");
                                W.WriteEndElement();
                            }
                            W.WriteEndElement();
                        }
                        W.WriteStartElement("ScientificName");
                        W.WriteElementString("FullScientificNameString", RR[i]["TaxonomicName"].ToString());
                        W.WriteEndElement();
                        if (!RR[i]["ResponsibleName"].Equals(System.DBNull.Value) && RR[i]["ResponsibleName"].ToString().Length > 0) 
                        {
                            W.WriteStartElement("Identifiers");
                            W.WriteElementString("IdentifiersText", RR[i]["ResponsibleName"].ToString());
                            W.WriteEndElement();
                        }
                        W.WriteEndElement();
                        W.WriteEndElement();
                        W.WriteEndElement();
                    }
                }
                W.WriteEndElement();
            }
        }

        #endregion

        #region Lookup tables
        private System.Data.DataTable DtCollection
        {
            get
            {
                if (this._dtCollection == null)
                {
                    string SQL = "SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, " +
                        "AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
                        "Location, DisplayOrder " +
                        "FROM Collection " +
                        "ORDER BY DisplayOrder";
                    return this.LookupTable(SQL, this._dtCollection);
                }
                return this._dtCollection;
            }
        }

        private System.Data.DataTable DtLocalisationSystem
        {
            get
            {
                if (this._dtLocalisationSystem == null)
                {
                    string SQL = "SELECT LocalisationSystemID, LocalisationSystemParentID, LocalisationSystemName, MeasurementUnit, DefaultAccuracyOfLocalisation, DiversityModule,  " +
                      "ParsingMethod, DisplayText, DisplayEnable, DisplayOrder, Description, DisplayTextLocation1, DescriptionLocation1, DisplayTextLocation2,  " +
                      "DescriptionLocation2 " +
                      "FROM LocalisationSystem " +
                      "WHERE (DisplayEnable = 1) " +
                      "ORDER BY DisplayOrder";
                    return this.LookupTable(SQL, this._dtLocalisationSystem);
                }
                return this._dtLocalisationSystem;
            }
        }

        private System.Data.DataTable DtAnalysis
        {
            get
            {
                if (this._dtAnalysis == null)
                {
                    string SQL = "SELECT AnalysisID, AnalysisParentID, DisplayText " +
                        "FROM Analysis ";
                    return this.LookupTable(SQL, this._dtAnalysis);
                }
                return this._dtAnalysis;
            }
        }

        private System.Data.DataTable DtUserProxy
        {
            get
            {
                if (this._dtUserProxy == null)
                {
                    string SQL = "SELECT [LoginName],[CombinedNameCache] FROM [UserProxy] ";
                    return this.LookupTable(SQL, this._dtUserProxy);
                }
                return this._dtUserProxy;
            }
        }


        private System.Data.DataTable LookupTable(string SQL, System.Data.DataTable dt)
        {
            try
            {
                if (dt == null)
                {
                    dt = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return dt;
        }

        #endregion

        #region Auxilliary
        private string BaseURL
        {
            get
            {
                if (this._BaseURL.Length == 0)
                {
                    string SQL = "SELECT dbo.BaseURL()";
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    try
                    {
                        con.Open();
                        this._BaseURL = C.ExecuteScalar()?.ToString();
                    }
                    catch { }
                    finally
                    {
                        con.Close();
                    }
                }
                return this._BaseURL;
            }
        }

        private string CollectionOwner
        {
            get
            {
                return this._CollectionOwner;
            }
        }

        private string CollectionOwnerAcronym
        {
            get
            {
                return this._CollectionOwnerAcronym;
            }
        }

        private void setCollectionOwner()
        {
            string SQL = "SELECT * FROM dbo.CollectionHierarchy(" + this._CollectionID.ToString() + ")";
            System.Data.DataTable dt = new System.Data.DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            System.Data.DataRow[] rr = dt.Select("DisplayOrder = 0");
            if (rr.Length > 0)
            {
                this._CollectionOwner = rr[0]["CollectionOwner"].ToString();
                this._CollectionOwnerAcronym = rr[0]["CollectionAcronym"].ToString();
            }
            else if (dt.Rows.Count > 0)
            {
                this._CollectionOwner = dt.Rows[0]["CollectionOwner"].ToString();
                this._CollectionOwnerAcronym = dt.Rows[0]["CollectionAcronym"].ToString();
            }
        }

        private string Collection { get { return this._Collection; } }

        private void setCollection()
        {
            if (this._CollectionID > -1)
            {
                string SQL = "SELECT CollectionName FROM Collection WHERE CollectionID = " + this._CollectionID.ToString();
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    this._Collection = C.ExecuteScalar()?.ToString();
                }
                catch { }
                finally
                {
                    con.Close();
                }
            }
        }

        private string LastEditor(System.Data.DataRow R)
        {
            string Editor = "";
            string ID = R[0].ToString();
            string SQL = "SELECT CASE WHEN UserProxy.CombinedNameCache IS NULL " +
                "THEN CollectionSpecimen.LogUpdatedBy ELSE UserProxy.CombinedNameCache END AS Editor " +
                "FROM CollectionSpecimen LEFT OUTER JOIN " +
                "UserProxy ON CollectionSpecimen.LogUpdatedBy = UserProxy.LoginName " +
                "WHERE CollectionSpecimen.CollectionSpecimenID = " + ID;
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                Editor = C.ExecuteScalar()?.ToString();
                if (Editor.IndexOf("\\") > -1)
                    Editor = Editor.Substring(Editor.IndexOf("\\")+1);
            }
            catch { }
            finally
            {
                con.Close();
            }
            return Editor;
        }

        private string DateLastEdited(System.Data.DataRow R)
        {
            string Date = "";
            string ID = R[0].ToString();
            string SQL = "SELECT CollectionSpecimen.LogUpdatedWhen " +
                "FROM CollectionSpecimen " +
                "WHERE CollectionSpecimen.CollectionSpecimenID = " + ID;
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                Date = C.ExecuteScalar()?.ToString();
            }
            catch { }
            finally
            {
                con.Close();
            }
            return Date;
        }

        #endregion

    }
}
