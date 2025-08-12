using DiversityCollection.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityCollection
{
    class XmlExport
    {
        #region Parameter

        private string _MaterialCategory;
        private string _CollectionOwner;
        private int? _CollectionID;
        private int _ProjectID;
        private bool _MultiLabelPrint;
        private bool _LabelForSpecimen;
        private bool _RestrictToCollection;
        private bool _RestrictToMaterial;
        private bool _RegardStockForDuplicates;
        private int _Duplicates;
        private bool _RankFound;
        private DiversityCollection.Datasets.DataSetCollectionEventSeries dataSetCollectionEventSeries;
        private DiversityCollection.Datasets.DataSetCollectionSpecimen dataSetCollectionSpecimen;
        private System.IO.FileInfo _XslFile;
        private System.IO.FileInfo _XmlFile;
        private System.Xml.XmlWriter W;
        private DiversityWorkbench.Agent _Agent;
        private DiversityWorkbench.Gazetteer _Gazetteer;
        private DiversityWorkbench.TaxonName _TaxonName;
        private DiversityWorkbench.ScientificTerm _Term;
        private System.Collections.Generic.List<string> _IdenfificationQualifierList;
        private System.Collections.Generic.List<string> _RankList;
        private System.Collections.Generic.List<string> _AuthorSeparatorList;
        private enum _IdentificationQualifierType { QualifierLeading, QualifierGenus, QualifierSpecies, QualifierInfraspecific, QualifierTerminatory};
        private enum _AuthorType {AuthorsGenus, AuthorsInfrageneric, AuthorsSpecies, AuthorsInfraspecific };
        private enum _NameParts { Rank, HybridSeparator, Genus, InfragenericEpithet, SpeciesEpithet, InfraspecificEpithet, Undefined }; 
        private DiversityCollection.Transaction.ConversionType _ConversionType;

        public enum QRcodeSource { None, StableIdentifier, AccessionNumber, DepositorsAccessionNumber, CollectorsEventNumber, PartAccessionNumber, StorageLocation, ExternalIdentifier, CollectionName, GUID }
        public enum QRcodeStableIdentifierType { AccessionNumber, SpecimenID, Units, UnitsInPart, Collection }
        private QRcodeSource _QRsource;
        private string _QRtype;

        private System.IO.DirectoryInfo _ImageDirectory;

        public enum CollectionContentSorting { StorageLocation, AccessionNumber, Taxon }

        #endregion

        #region Construction

        public XmlExport(string XslFile, 
            string XmlFile, 
            DiversityCollection.Datasets.DataSetCollectionSpecimen DsSpecimen,
            DiversityCollection.Datasets.DataSetCollectionEventSeries DsEventSeries)
        {
            this.dataSetCollectionEventSeries = DsEventSeries;
            this.dataSetCollectionSpecimen = DsSpecimen;
            if (XslFile.Length > 0 && System.IO.File.Exists(XslFile))
                this._XslFile = new System.IO.FileInfo(XslFile);
            if (XmlFile.Length > 0)
                this._XmlFile = new System.IO.FileInfo(XmlFile);
            else
                this._XmlFile = new System.IO.FileInfo(Folder.Export() + "XmlExport.XML");
        }

        public XmlExport(string XslFile,
            string XmlFile,
            DiversityCollection.Datasets.DataSetCollection DsCollection,
            int ProjectID)
        {
            this._DataSetCollection = DsCollection;
            this._ProjectID = ProjectID;
            if (XslFile.Length > 0 && System.IO.File.Exists(XslFile))
                this._XslFile = new System.IO.FileInfo(XslFile);
            if (XmlFile.Length > 0)
                this._XmlFile = new System.IO.FileInfo(XmlFile);
            else
                this._XmlFile = new System.IO.FileInfo(Folder.Export() + "XmlExport.XML");
        }

        public XmlExport(string XslFile,
            string XmlFile,
            DiversityCollection.Datasets.DataSetCollectionTask DsCollectionTask)
        {
            this._DataSetCollectionTask = DsCollectionTask;
            if (XslFile.Length > 0 && System.IO.File.Exists(XslFile))
                this._XslFile = new System.IO.FileInfo(XslFile);
            if (XmlFile.Length > 0)
                this._XmlFile = new System.IO.FileInfo(XmlFile);
            else
                this._XmlFile = new System.IO.FileInfo(Folder.Export() + "XmlExport.XML");
        }

        public XmlExport(string XslFile,
            string XmlFile)
        {
            if (XslFile.Length > 0 && System.IO.File.Exists(XslFile))
                this._XslFile = new System.IO.FileInfo(XslFile);
            if (XmlFile.Length > 0)
                this._XmlFile = new System.IO.FileInfo(XmlFile);
            else
                this._XmlFile = new System.IO.FileInfo(Folder.Export() + "XmlExport.XML");
        }

        #endregion

        #region XML Export

        public string createXmlFromDatasets(
            string Title,
            QRcodeSource SourceForQRcode,
            string TypeOfQRcode,
            System.Data.DataRow DataRow,
            int ProjectID,
            int Duplicates,
            bool MultiLabelPrint,
            bool LabelForSpecimen,
            bool RestrictToCollection,
            bool RestrictToMaterial,
            bool RegardStockForDuplicates,
            Transaction.ConversionType ConversionType)
        {
            bool OK = true;
            System.IO.FileInfo FTemplate = new System.IO.FileInfo(this._XmlFile.Directory.FullName + "\\Schemas\\Templates\\LabelTemplates.xslt");
            bool TemplateAvailable = FTemplate.Exists;
            try
            {
                this._MultiLabelPrint = MultiLabelPrint;
                this._LabelForSpecimen = LabelForSpecimen;
                this._ProjectID = ProjectID;
                string Table = DataRow.Table.TableName;
                this._MaterialCategory = "";
                this._CollectionOwner = "";
                this._Duplicates = Duplicates;
                this._RestrictToCollection = RestrictToCollection;
                this._RestrictToMaterial = RestrictToMaterial;
                this._RegardStockForDuplicates = RegardStockForDuplicates;
                this._ConversionType = ConversionType;
                this._QRsource = SourceForQRcode;
                this._QRtype = TypeOfQRcode;
                this.ResetImageDirectory();
                switch (Table)
                {
                    case "CollectionSpecimen":
                        if (!DataRow["CollectionID"].Equals(System.DBNull.Value))
                            this._CollectionID = int.Parse(DataRow["CollectionID"].ToString());
                        break;
                    case "CollectionSpecimenPart":
                        if (!DataRow["CollectionID"].Equals(System.DBNull.Value))
                            this._CollectionID = int.Parse(DataRow["CollectionID"].ToString());
                        if (!DataRow["MaterialCategory"].Equals(System.DBNull.Value))
                            this._MaterialCategory = DataRow["MaterialCategory"].ToString();
                        break;
                    default:
                        break;
                }
                if (this._CollectionID != null)
                    this._CollectionOwner = DiversityCollection.LookupTable.CollectionOwner((int)this._CollectionID);
                System.Xml.XmlWriter W;
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                settings.Encoding = System.Text.Encoding.UTF8;
                W = System.Xml.XmlWriter.Create(this._XmlFile.FullName, settings);
                W.WriteStartDocument();
                W.WriteStartElement("LabelPrint");
                W.WriteStartElement("Report");
                W.WriteElementString("Title", Title);
                string ProjectTitle = DiversityCollection.LookupTable.ProjectTitle(this._ProjectID);
                W.WriteElementString("ProjectTitle", ProjectTitle);
                string ProjectParent = DiversityCollection.LookupTable.ProjectParentTitle(this._ProjectID);
                if (ProjectParent != ProjectTitle)
                    W.WriteElementString("ProjectParentTitle", ProjectParent);
                W.WriteElementString("User", DiversityCollection.LookupTable.CurrentUser);
                string Date = System.DateTime.Now.Year.ToString() + "/" + System.DateTime.Now.Month.ToString() + "/" + System.DateTime.Now.Day.ToString();
                W.WriteElementString("Date", Date);
                W.WriteEndElement(); // Report
                switch (Table)
                {
                    case "CollectionSpecimen":
                        if (!this.writeXmlLabelForSpecimen(ref W))
                            OK = false;
                        break;
                    case "CollectionSpecimenPart":
                        this.writeXmlLabel(ref W);
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("Please select the part that should be printed");
                        break;
                }
                // check for missing template
                //this.writeXmlLabel(ref W);
                W.WriteFullEndElement();  // LabelPrint
                W.WriteEndDocument();
                W.Flush();
                W.Close();
                if (this._XslFile != null && this._XslFile.Exists)
                {
                    // #65
                    System.Xml.Xsl.XslCompiledTransform XSLT = new System.Xml.Xsl.XslCompiledTransform();
                    System.Xml.Xsl.XsltSettings XsltSettings = new System.Xml.Xsl.XsltSettings(true, true);
                    System.Xml.XmlResolver resolver = new System.Xml.XmlUrlResolver();
                    XSLT.Load(this._XslFile.FullName, XsltSettings, resolver);

                    // Load the file to transform.
                    System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(this._XmlFile.FullName);

                    // The output file:
                    string OutputFile = this._XmlFile.FullName.Substring(0, this._XmlFile.FullName.Length 
                        - this._XmlFile.Extension.Length) + ".htm";

                    // Create the writer.             
                    System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(OutputFile, XSLT.OutputSettings);

                    // Transform the file and send the output to the console.
                    XSLT.Transform(doc, writer);
                    writer.Close();
                    return OutputFile;
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                if (!TemplateAvailable)
                {
                    string Message = "The template xslt file\r\n.../LabelPrinting/Templates/LabelTemplates.xslt\r\nfor the creation of the labels is missing\r\n" + ex.Message;
                    System.Windows.Forms.MessageBox.Show(Message);
                }
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

        /// <summary>
        /// Writing the labels based on table CollectionSpecimen
        /// </summary>
        /// <param name="W">XmlWriter</param>
        private bool writeXmlLabelForSpecimen(ref System.Xml.XmlWriter W)
        {
            bool OK = true;
            int Counter = 1;
            W.WriteStartElement("LabelList");
            foreach (System.Data.DataRow R in this.dataSetCollectionSpecimen.CollectionSpecimen.Rows)
            {
                try
                {
                    int Dupl = 1;
                    if (this._RegardStockForDuplicates && !R["Stock"].Equals(System.DBNull.Value))
                    {
                        if (int.TryParse(R["Stock"].ToString(), out Dupl))
                            Dupl = Dupl * this._Duplicates;
                    }
                    else
                        Dupl = this._Duplicates;

                    for (int i = 0; i < Dupl; i++)
                    {
                        W.WriteStartElement("Label");
                        W.WriteElementString("Counter", Counter.ToString());

                        // SPECIMEN
                        W.WriteStartElement("CollectionSpecimen");
                        foreach (System.Data.DataColumn C in R.Table.Columns)
                        {
                            if (C.ColumnName == "AccessionNumber")
                            {
                                string AccNr = "";
                                if (R["AccessionNumber"].Equals(System.DBNull.Value)
                                    || R["AccessionNumber"].ToString().Length == 0)
                                    AccNr = R["AccessionNumber"].ToString();
                                else
                                    AccNr = R["AccessionNumber"].ToString();
                                W.WriteElementString(C.ColumnName, DiversityCollection.Transaction.AccessionNumberConverted(this._ConversionType, AccNr));
                            }
                            if (!R[C.ColumnName].Equals(System.DBNull.Value) &&
                                R[C.ColumnName].ToString().Length > 0 &&
                                C.ColumnName != "CollectionEventID" &&
                                C.ColumnName != "AccessionNumber" &&
                                C.ColumnName != "Version" &&
                                //C.ColumnName != "CollectionSpecimenID" &&
                                C.ColumnName != "DepositorsAgentURI" &&
                                C.ColumnName != "ExsiccataURI" &&
                                C.ColumnName != "ReferenceURI" &&
                                C.ColumnName != "DataWithholdingReason" &&
                                C.ColumnName != "CollectionID" &&
                                C.ColumnName != "Problems" &&
                                C.ColumnName != "LabelTranscriptionNotes" &&
                                C.ColumnName != "LabelTranscriptionState" &&
                                C.ColumnName != "LabelType")
                            {
                                W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                            }
                        }
                        foreach (System.Data.DataColumn C in R.Table.Columns)
                        {
                            if (!C.ColumnName.Equals(System.DBNull.Value) &&
                                 C.ColumnName.ToString().Length > 0 &&
                                 C.ColumnName != "CollectionSpecimenID" &&
                                 C.ColumnName != "AccessionNumber" &&
                                 C.ColumnName != "CollectionID" &&
                                 C.ColumnName != "SpecimenPartID" &&
                                 C.ColumnName != "DerivedFromSpecimenPartID")
                            {
                                if (!R[C.ColumnName].Equals(System.DBNull.Value) && R[C.ColumnName].ToString().Length > 0)
                                    W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                            }

                        }
                        W.WriteEndElement(); // SPECIMEN

                        // EVENT
                        if (!R["CollectionEventID"].Equals(System.DBNull.Value))
                        {
                            int EventID = int.Parse(R["CollectionEventID"].ToString());
                            this.writeXmlEvent(ref W, EventID);
                        }
                        int SpecimenID = int.Parse(R["CollectionSpecimenID"].ToString());
                        this.writeXmlSpecimenImages(ref W, SpecimenID);
                        this.writeXmlAgents(ref W, SpecimenID);
                        if (!this.writeXmlUnits(ref W, SpecimenID, null))
                            OK = false;
                        this.writeXmlPartsForSpecimen(ref W, SpecimenID);
                        this.writeXmlRelations(ref W, SpecimenID);
                        this.writeXmlRelationsInvers(ref W, SpecimenID);

                        this.writeXmlLogUserAndDate(ref W, SpecimenID);
                        Counter++;
                        W.WriteEndElement(); // Label
                    }

                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            W.WriteEndElement(); // LabelList
            return OK;
        }

        /// <summary>
        /// Writing the labels based on table CollectionSpecimenPart
        /// </summary>
        /// <param name="W">XmlWriter</param>
        private bool writeXmlLabel(ref System.Xml.XmlWriter W)
        {
            bool OK = true;
            int Counter = 1;
            W.WriteStartElement("LabelList");
            foreach (System.Data.DataRow R in this.dataSetCollectionSpecimen.CollectionSpecimenPart.Rows)
            {
                try
                {
                    if ((R["CollectionID"].ToString() == this._CollectionID.ToString() || !this._RestrictToCollection)
                        && (R["MaterialCategory"].ToString() == this._MaterialCategory.ToString() || !this._RestrictToMaterial))
                    {
                        if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
                        {
                            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionSpecimen.Select("CollectionSpecimenID = " + R["CollectionSpecimenID"].ToString());
                            if (rr.Length > 0)
                            {
                                int Dupl = 1;
                                if (this._RegardStockForDuplicates && !R["Stock"].Equals(System.DBNull.Value))
                                {
                                    if (int.TryParse(R["Stock"].ToString(), out Dupl))
                                        Dupl = Dupl * this._Duplicates;
                                }
                                else
                                    Dupl = this._Duplicates;
                                //if (Dupl == 0) Dupl = this._Duplicates;
                                for (int i = 0; i < Dupl; i++)
                                {
                                    W.WriteStartElement("Label");
                                    W.WriteElementString("Counter", Counter.ToString());

                                    // COLLECTION
                                    W.WriteStartElement("Collection");
                                    if (this._CollectionID != null && R["CollectionID"].ToString() == this._CollectionID.ToString())
                                    {
                                        W.WriteElementString("CollectionName", DiversityCollection.LookupTable.CollectionName((int)this._CollectionID));
                                        W.WriteElementString("CollectionOwner", DiversityCollection.LookupTable.CollectionOwner((int)this._CollectionID));
                                        string URI = DiversityCollection.LookupTable.CollectionOwnerAgentURI((int)this._CollectionID);
                                        if (URI.Length > 0)
                                        {
                                            W.WriteStartElement("CollectionOwnerAddress");
                                            this.writeXmlAddress(ref W, URI);
                                            W.WriteEndElement(); // CollectionOwnerAddress
                                        }
                                    }
                                    else
                                    {
                                        int CollID = 0;
                                        if (int.TryParse(R["CollectionID"].ToString(), out CollID))
                                        {
                                            W.WriteElementString("CollectionName", DiversityCollection.LookupTable.CollectionName(CollID));
                                            W.WriteElementString("CollectionOwner", DiversityCollection.LookupTable.CollectionOwner(CollID));
                                            string URI = DiversityCollection.LookupTable.CollectionOwnerAgentURI(CollID);
                                            if (URI.Length > 0)
                                            {
                                                W.WriteStartElement("CollectionOwnerAddress");
                                                this.writeXmlAddress(ref W, URI);
                                                W.WriteEndElement(); // CollectionOwnerAddress
                                            }
                                            //W.WriteEndElement();
                                        }
                                    }
                                    W.WriteEndElement();  // Collection

                                    // DUPLICATE
                                    if (!R["DerivedFromSpecimenPartID"].Equals(System.DBNull.Value))
                                    {
                                        System.Data.DataRow[] RRParent = this.dataSetCollectionSpecimen.CollectionSpecimenPart.Select("CollectionSpecimenID = " + R["CollectionSpecimenID"].ToString() + " AND SpecimenPartID = " + R["DerivedFromSpecimenPartID"].ToString());
                                        if (RRParent.Length > 0)
                                        {
                                            if (!RRParent[0]["CollectionID"].Equals(System.DBNull.Value)
                                                && !R["CollectionID"].Equals(System.DBNull.Value)
                                                && RRParent[0]["CollectionID"].ToString() != R["CollectionID"].ToString())
                                            {
                                                string CollOwnerParent = DiversityCollection.LookupTable.CollectionOwner(int.Parse(RRParent[0]["CollectionID"].ToString()));
                                                string CollOwnerChild = DiversityCollection.LookupTable.CollectionOwner(int.Parse(R["CollectionID"].ToString()));
                                                if (CollOwnerChild != CollOwnerParent)
                                                {
                                                    W.WriteStartElement("OriginOfDuplicate"); // DUPLICATE
                                                    W.WriteElementString("CollectionOwner", DiversityCollection.LookupTable.CollectionOwner(int.Parse(RRParent[0]["CollectionID"].ToString())));
                                                    if (RRParent[0]["AccessionNumber"].Equals(System.DBNull.Value) || RRParent[0]["AccessionNumber"].ToString().Length == 0)
                                                        W.WriteElementString("AccessionNumber", DiversityCollection.Transaction.AccessionNumberConverted(this._ConversionType, rr[0]["AccessionNumber"].ToString()));
                                                    else
                                                        W.WriteElementString("AccessionNumber", DiversityCollection.Transaction.AccessionNumberConverted(this._ConversionType, RRParent[0]["AccessionNumber"].ToString()));
                                                    W.WriteEndElement(); // DUPLICATE
                                                }
                                            }
                                        }
                                    }

                                    // SPECIMEN
                                    W.WriteStartElement("CollectionSpecimen");
                                    foreach (System.Data.DataColumn C in rr[0].Table.Columns)
                                    {
                                        if (C.ColumnName == "AccessionNumber")
                                        {
                                            string AccNr = "";
                                            if (R["AccessionNumber"].Equals(System.DBNull.Value)
                                                || R["AccessionNumber"].ToString().Length == 0)
                                                AccNr = rr[0]["AccessionNumber"].ToString();
                                            else
                                                AccNr = R["AccessionNumber"].ToString();
                                            W.WriteElementString(C.ColumnName, DiversityCollection.Transaction.AccessionNumberConverted(this._ConversionType, AccNr));
                                        }
                                        if (!rr[0][C.ColumnName].Equals(System.DBNull.Value) &&
                                            rr[0][C.ColumnName].ToString().Length > 0 &&
                                            C.ColumnName != "CollectionEventID" &&
                                            C.ColumnName != "Version" &&
                                            //C.ColumnName != "CollectionSpecimenID" &&
                                            C.ColumnName != "DepositorsAgentURI" &&
                                            C.ColumnName != "ExsiccataURI" &&
                                            C.ColumnName != "ReferenceURI" &&
                                            C.ColumnName != "DataWithholdingReason" &&
                                            C.ColumnName != "CollectionID" &&
                                            C.ColumnName != "Problems" &&
                                            C.ColumnName != "LabelTranscriptionNotes" &&
                                            C.ColumnName != "LabelTranscriptionState" &&
                                            C.ColumnName != "LabelType")
                                        {
                                            if (C.ColumnName == "AccessionNumber")
                                                W.WriteElementString("AccessionNumberSpecimen", rr[0][C.ColumnName].ToString());
                                            else
                                                W.WriteElementString(C.ColumnName, rr[0][C.ColumnName].ToString());
                                        }
                                    }
                                    foreach (System.Data.DataColumn C in R.Table.Columns)
                                    {
                                        if (!C.ColumnName.Equals(System.DBNull.Value) &&
                                             C.ColumnName.ToString().Length > 0 &&
                                             C.ColumnName != "CollectionSpecimenID" &&
                                             C.ColumnName != "CollectionID" &&
                                             C.ColumnName != "SpecimenPartID" &&
                                             C.ColumnName != "DerivedFromSpecimenPartID")
                                        {
                                            if (!R[C.ColumnName].Equals(System.DBNull.Value) && R[C.ColumnName].ToString().Length > 0)
                                            {
                                                if (C.ColumnName == "AccessionNumber")
                                                    W.WriteElementString("AccessionNumberPart", R[C.ColumnName].ToString());
                                                else
                                                    W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                                            }
                                        }

                                    }
                                    if (this._QRsource == QRcodeSource.GUID)
                                    {
                                        string SQL = "SELECT CAST(RowGUID as varchar(40)) FROM CollectionSpecimen WHERE CollectionSpecimenID = " + R["CollectionSpecimenID"].ToString();
                                        string QR = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                                        W.WriteElementString("RowGUID", QR);
                                    }
                                    this.writeXmlExternalIdentifier(ref W, "CollectionSpecimen", int.Parse(R["CollectionSpecimenID"].ToString()));
                                    W.WriteEndElement(); // SPECIMEN

                                    // EVENT
                                    if (!rr[0]["CollectionEventID"].Equals(System.DBNull.Value))
                                    {
                                        int EventID = int.Parse(rr[0]["CollectionEventID"].ToString());
                                        this.writeXmlEvent(ref W, EventID);
                                    }
                                    int SpecimenID = int.Parse(rr[0]["CollectionSpecimenID"].ToString());
                                    if (this._QRsource != QRcodeSource.None)
                                        this.writeXmlQRcode(ref W, SpecimenID, int.Parse(R["SpecimenPartID"].ToString()));
                                    this.writeXmlSpecimenImages(ref W, SpecimenID);
                                    this.writeXmlAgents(ref W, SpecimenID);
                                    if (!this.writeXmlUnits(ref W, SpecimenID, int.Parse(R["SpecimenPartID"].ToString())))
                                        OK = false;
                                    this.writeXmlRelations(ref W, SpecimenID);
                                    this.writeXmlRelationsInvers(ref W, SpecimenID);
                                    this.writeXmlProcessing(ref W, SpecimenID, int.Parse(R["SpecimenPartID"].ToString()));
                                    if (!R["CollectionID"].Equals(System.DBNull.Value))
                                    {
                                        int CollectionID = int.Parse(R["CollectionID"].ToString());
                                        this.writeXmlTransaction(ref W, SpecimenID, int.Parse(R["SpecimenPartID"].ToString()), CollectionID);
                                    }
                                    else
                                        this.writeXmlTransaction(ref W, SpecimenID, int.Parse(R["SpecimenPartID"].ToString()), null);
                                    this.writeXmlLogUserAndDate(ref W, SpecimenID);
                                    Counter++;
                                    W.WriteEndElement(); // Label
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
            W.WriteEndElement(); // LabelList
            return OK;
        }

        private void writeXmlExternalIdentifier(ref System.Xml.XmlWriter W, string Table, int ID)
        {
            try
            {
                string SQL = "SELECT Type, Identifier, URL, Notes " +
                    " FROM ExternalIdentifier AS E " +
                    " WHERE ReferencedID = " + ID.ToString() +
                    " AND ReferencedTable = '" + Table + "'";
                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    W.WriteStartElement("ExternalIdentifiers");
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        W.WriteStartElement("ExternalIdentifier");
                        foreach (System.Data.DataColumn C in R.Table.Columns)
                        {
                            W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                        }
                        W.WriteEndElement();
                    }
                    W.WriteEndElement();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void writeXmlSpecimen(ref System.Xml.XmlWriter W, int Duplicates)
        {
            if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
            {
                foreach (System.Data.DataRow R in this.dataSetCollectionSpecimen.CollectionSpecimen.Rows)
                {
                    try
                    {
                        for (int i = 0; i < Duplicates; i++)
                        {
                            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionSpecimenPart.Select(
                                "CollectionSpecimenID = " + R["CollectionSpecimenID"].ToString() +
                                " AND CollectionID = " + this._CollectionID.ToString() +
                                " AND MaterialCategory = '" + this._MaterialCategory + "'");
                            if (rr.Length == 0) continue;
                            W.WriteStartElement("CollectionSpecimen");
                            foreach (System.Data.DataColumn C in R.Table.Columns)
                            {
                                if (!R[C.ColumnName].Equals(System.DBNull.Value) &&
                                    R[C.ColumnName].ToString().Length > 0 &&
                                    C.ColumnName != "CollectionEventID" &&
                                    C.ColumnName != "AccessionNumber" &&
                                    C.ColumnName != "DepositorsAgentURI" &&
                                    C.ColumnName != "ExsiccataURI" &&
                                    C.ColumnName != "ReferenceURI" &&
                                    C.ColumnName != "DataWithholdingReason" &&
                                    C.ColumnName != "Problems" &&
                                    C.ColumnName != "LabelTranscriptionNotes" &&
                                    C.ColumnName != "LabelTranscriptionState" &&
                                    C.ColumnName != "LabelType")
                                {
                                    W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                                }
                            }

                            W.WriteEndElement();
                        }

                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
        }
        
        private void writeXmlLogUserAndDate(ref System.Xml.XmlWriter W, int CollectionSpecimenID)
        {
            string SQL = @"
                declare @User nvarchar(100);
                set @User = (select User_Name());
                declare @i int;
                set @i = (select count(*) from UserProxy where loginName = @User AND loginName <> 'dbo');
                if (@i > 0) set @User = (select min(CombinedNameCache) from UserProxy where loginName = @User);
                if @User = 'dbo' set @User = '';
                SELECT 
                    case 
                        when CollectionSpecimen.LogUpdatedBy = 'dbo' then @User  
                        else ISNULL(UserProxy.CombinedNameCache, ISNULL(CollectionSpecimen.LogUpdatedBy, '')) 
                    end AS UserName,
                    YEAR(CollectionSpecimen.LogUpdatedWhen) AS Jahr,
                    MONTH(CollectionSpecimen.LogUpdatedWhen) AS Monat,
                    DAY(CollectionSpecimen.LogUpdatedWhen) AS Tag
                FROM CollectionSpecimen
                LEFT OUTER JOIN UserProxy 
                    ON (ISNUMERIC(CollectionSpecimen.LogUpdatedBy) = 0 AND CollectionSpecimen.LogUpdatedBy = UserProxy.LoginName)
                    OR (ISNUMERIC(CollectionSpecimen.LogUpdatedBy) = 1 AND CAST(UserProxy.ID AS varchar) = CollectionSpecimen.LogUpdatedBy)
                WHERE CollectionSpecimen.CollectionSpecimenID = " + CollectionSpecimenID.ToString();

            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            System.Data.DataTable dt = new System.Data.DataTable();
            ad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                try
                {
                    W.WriteStartElement("UpdateUserAndDate");
                    W.WriteStartElement("User");
                    string User = dt.Rows[0]["UserName"].ToString();
                    string UserInvers = User;
                    W.WriteElementString("UserName", User);
                    if (User.IndexOf(",") > -1)
                    {
                        W.WriteElementString("UserSecondName", User.Substring(0, User.IndexOf(",")));
                        UserInvers = User.Substring(0, User.IndexOf(","));
                        User = User.Substring(User.IndexOf(",") + 1).Trim();
                        if (User.IndexOf("(") > -1)
                        {
                            W.WriteElementString("UserCity", User.Substring(User.IndexOf("(") + 1, User.IndexOf(")") - User.IndexOf("(") - 1));
                            User = User.Substring(0, User.IndexOf("(") - 1).Trim();
                        }
                        W.WriteElementString("UserFirstName", User);
                        UserInvers = User + " " + UserInvers;
                        W.WriteElementString("UserInvers", UserInvers);
                    }
                    W.WriteEndElement();  // User

                    W.WriteStartElement("Date");
                    W.WriteElementString("Year", dt.Rows[0]["Jahr"].ToString());
                    W.WriteElementString("Month", dt.Rows[0]["Monat"].ToString());
                    W.WriteElementString("Day", dt.Rows[0]["Tag"].ToString());
                    W.WriteEndElement(); // Date

                    W.WriteEndElement();  // UpdateUserAndDate

                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void writeXmlSpecimens(ref System.Xml.XmlWriter W, int Duplicates)
        {
            int Counter = 1;
            if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
            {
                foreach (System.Data.DataRow R in this.dataSetCollectionSpecimen.CollectionSpecimen.Rows)
                {
                    try
                    {
                        for (int i = 0; i < Duplicates; i++)
                        {
                            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionSpecimenPart.Select(
                                "CollectionSpecimenID = " + R["CollectionSpecimenID"].ToString() +
                                " AND CollectionID = " + this._CollectionID.ToString() +
                                " AND MaterialCategory = '" + this._MaterialCategory + "'");
                            if (rr.Length == 0) continue;
                            W.WriteStartElement("CollectionSpecimen");
                            W.WriteElementString("Counter", Counter.ToString());
                            foreach (System.Data.DataColumn C in R.Table.Columns)
                            {
                                if (!R[C.ColumnName].Equals(System.DBNull.Value) &&
                                    R[C.ColumnName].ToString().Length > 0 &&
                                    C.ColumnName != "CollectionEventID" &&
                                    C.ColumnName != "DepositorsAgentURI" &&
                                    C.ColumnName != "ExsiccataURI" &&
                                    C.ColumnName != "ReferenceURI" &&
                                    C.ColumnName != "DataWithholdingReason" &&
                                    C.ColumnName != "Problems" &&
                                    C.ColumnName != "LabelTranscriptionNotes" &&
                                    C.ColumnName != "LabelTranscriptionState" &&
                                    C.ColumnName != "LabelType")
                                {
                                    W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                                }
                            }

                            W.WriteElementString("User", DiversityCollection.LookupTable.CurrentUser);

                            if (!R["CollectionEventID"].Equals(System.DBNull.Value))
                            {
                                int EventID = int.Parse(R["CollectionEventID"].ToString());
                                this.writeXmlEvent(ref W, EventID);
                            }
                            int SpecimenID = int.Parse(R["CollectionSpecimenID"].ToString());
                            this.writeXmlPart(ref W, SpecimenID);
                            W.WriteEndElement();
                            Counter++;
                        }

                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
        }

        #region Event

        private void writeXmlEvent(ref System.Xml.XmlWriter W, int EventID)
        {
            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionEvent.Select("CollectionEventID = " + EventID.ToString());
            if (rr.Length > 0)
            {
                W.WriteStartElement("CollectionEvent");
                for (int i = 0; i < rr.Length; i++)
                {
                    foreach (System.Data.DataColumn C in this.dataSetCollectionSpecimen.CollectionEvent.Columns)
                    {
                        if (!rr[i][C.ColumnName].Equals(System.DBNull.Value) &&
                            rr[i][C.ColumnName].ToString().Length > 0 &&
                             C.ColumnName != "CollectionEventID" &&
                             C.ColumnName != "Version" &&
                             C.ColumnName != "ExpeditionID" &&
                             C.ColumnName != "SeriesID" &&
                             C.ColumnName != "ExsiccataURI" &&
                             C.ColumnName != "ReferenceURI" &&
                             C.ColumnName != "DataWithholdingReason")
                        {
                            if (C.ColumnName == "LocalityDescription")
                            {
                                string L = rr[i][C.ColumnName].ToString();
                                //if (!L.EndsWith(".")) L += ".";
                                W.WriteElementString(C.ColumnName, L);
                            }
                            else if (C.ColumnName == "HabitatDescription")
                            {
                                string L = rr[i][C.ColumnName].ToString();
                                //if (!L.EndsWith(".")) L += ".";
                                L = L.Substring(0, 1).ToUpper() + L.Substring(1);
                                W.WriteElementString(C.ColumnName, L);
                            }
                            else if (C.ColumnName == "CollectionDate")
                            {
                                System.DateTime Date = System.DateTime.Now;
                                if (System.DateTime.TryParse(rr[i][C.ColumnName].ToString(), out Date))
                                {
                                    string L = Date.ToShortDateString();
                                    W.WriteElementString(C.ColumnName, L);
                                }
                            }
                            else
                                W.WriteElementString(C.ColumnName, rr[i][C.ColumnName].ToString());
                        }
                    }
                }
                W.WriteEndElement(); // CollectionEvent
                this.writeXmlEventSeries(ref W, EventID);
                this.writeXmlEventLocalisation(ref W, EventID);
                this.writeXmlEventProperty(ref W, EventID);
            }
        }

        private void writeXmlEventSeries(ref System.Xml.XmlWriter W, int EventID)
        {
            System.Data.DataRow[] rrEv = this.dataSetCollectionSpecimen.CollectionEvent.Select("CollectionEventID = " + EventID.ToString());
            if (!rrEv[0]["SeriesID"].Equals(System.DBNull.Value))
            {
                int SeriesID = int.Parse(rrEv[0]["SeriesID"].ToString());
                System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesID = " + SeriesID.ToString());
                if (rr.Length > 0)
                {
                    System.Collections.Generic.List<System.Data.DataRow> EventSeries = this.EventSeriesList(SeriesID);
                    if (EventSeries.Count > 0)
                    {
                        W.WriteStartElement("EventSeries");
                        for (int i = EventSeries.Count - 1; i > -1; i--)
                        {
                            W.WriteStartElement("EventSeries");
                            foreach (System.Data.DataColumn C in EventSeries[i].Table.Columns)
                            {
                                if (!EventSeries[i][C.ColumnName].Equals(System.DBNull.Value) &&
                                    EventSeries[i][C.ColumnName].ToString().Length > 0 &&
                                    C.ColumnName != "SeriesID" &&
                                    C.ColumnName != "SeriesParentID" &&
                                    C.ColumnName != "DateStart")
                                {
                                    W.WriteElementString(C.ColumnName, EventSeries[i][C.ColumnName].ToString());
                                }
                            }
                            W.WriteEndElement();
                        }
                        W.WriteEndElement();
                    }
                }
            }
        }

        private void writeXmlEventLocalisation(ref System.Xml.XmlWriter W, int EventID)
        {
            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionEventLocalisation.Select("CollectionEventID = " + EventID.ToString());
            if (rr.Length > 0)
            {
                W.WriteStartElement("CollectionEventLocalisations");
                foreach (System.Data.DataRow R in rr)
                {
                    W.WriteStartElement("Localisation");
                    string LocalisationSystem = DiversityCollection.LookupTable.LocalisationSystemName(int.Parse(R["LocalisationSystemID"].ToString()));
                    W.WriteElementString("LocalisationSystemName", LocalisationSystem);
                    string ParsingMethod = DiversityCollection.LookupTable.ParsingMethodName(int.Parse(R["LocalisationSystemID"].ToString()));
                    W.WriteElementString("ParsingMethod", ParsingMethod);
                    string MearsurementUnit = DiversityCollection.LookupTable.LocalisationMeasurementUnit(int.Parse(R["LocalisationSystemID"].ToString()));
                    decimal d;
                    if (MearsurementUnit.Length > 0
                        && R["Location1"].ToString().Trim().IndexOf(" ") == -1
                        && R["Location2"].ToString().Trim().IndexOf(" ") == -1
                        && (decimal.TryParse(R["Location1"].ToString(), out d) || R["Location1"].ToString().Length == 0)
                        && (decimal.TryParse(R["Location2"].ToString(), out d) || R["Location2"].ToString().Length == 0))
                    {
                        W.WriteElementString("MeasurementUnit", MearsurementUnit);
                    }
                    else
                        W.WriteElementString("MeasurementUnit", "");
                    W.WriteElementString("Location1", R["Location1"].ToString());
                    W.WriteElementString("Location2", R["Location2"].ToString());
                    foreach (System.Data.DataColumn C in this.dataSetCollectionSpecimen.CollectionEventLocalisation.Columns)
                    {
                        if (!R[C.ColumnName].Equals(System.DBNull.Value) &&
                            R[C.ColumnName].ToString().Length > 0 &&
                            C.ColumnName != "CollectionEventID" &&
                            C.ColumnName != "LocalisationSystemID" &&
                            C.ColumnName != "ResponsibleName" &&
                            C.ColumnName != "ResponsibleAgentURI" &&
                            C.ColumnName != "Location1" &&
                            C.ColumnName != "Location2")
                        {
                            string Value = R[C.ColumnName].ToString();
                            if (C.ColumnName.StartsWith("Average"))
                            {
                                Value = Value.Replace(",", ".");
                                decimal dec;
                                if (decimal.TryParse(Value, out dec))
                                    Value = System.Math.Round(dec, 5).ToString();
                            }
                            W.WriteElementString(C.ColumnName, Value);
                        }
                    }
                    int LocalisationSystemID;
                    if (int.TryParse(R["LocalisationSystemID"].ToString(), out LocalisationSystemID))
                    {
                        this.writeXmlEventLocalisationGazetteer(ref W, EventID, LocalisationSystemID);
                        this.writeXmlEventLocalisationCoordinates(ref W, EventID, LocalisationSystemID);
                    }
                    W.WriteEndElement(); // Localisation
                }
                W.WriteEndElement(); // CollectionEventLocalisations
            }
        }

        private void writeXmlEventLocalisationCoordinates(ref System.Xml.XmlWriter W, int EventID, int LocalisationSystemID)
        {
            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionEventLocalisation.Select("CollectionEventID = " + EventID.ToString() + " AND LocalisationSystemID = " + LocalisationSystemID.ToString());
            if (rr.Length > 0)
            {
                // Check values are present and valid
                if (rr[0]["AverageLatitudeCache"].Equals(System.DBNull.Value) || rr[0]["AverageLongitudeCache"].Equals(System.DBNull.Value))
                    return;
                double Latitude;
                if (!double.TryParse(rr[0]["AverageLatitudeCache"].ToString(), out Latitude))
                    return;
                double Longitude;
                if (!double.TryParse(rr[0]["AverageLongitudeCache"].ToString(), out Longitude))
                    return;

                // getting converted values
                string sLat = "";
                string sLong = "";
                if (DiversityWorkbench.GeoFunctions.ConvertNumericToDegree(Latitude, Longitude, ref sLat, ref sLong, 0))
                {
                    W.WriteStartElement("CoordinatesDegMinSec"); // Gazetteer
                    int Prefix = 0;
                    int Deg = 0;
                    int Min = 0;
                    double Sec = 0;
                    if (DiversityWorkbench.GeoFunctions.ConvertNumericToDegree(Latitude, ref Prefix, ref Deg, ref Min, ref Sec))
                    {
                        if (Prefix > 0)
                            W.WriteAttributeString("LatPrefix", "N");
                        else W.WriteAttributeString("LatPrefix", "S");
                        W.WriteAttributeString("LatSec", Sec.ToString());
                        W.WriteAttributeString("LatMin", Min.ToString());
                        W.WriteAttributeString("LatDeg", Deg.ToString());
                    }
                    if (DiversityWorkbench.GeoFunctions.ConvertNumericToDegree(Longitude, ref Prefix, ref Deg, ref Min, ref Sec))
                    {
                        if (Prefix > 0)
                            W.WriteAttributeString("LongPrefix", "E");
                        else W.WriteAttributeString("LongPrefix", "W");
                        W.WriteAttributeString("LongSec", Sec.ToString());
                        W.WriteAttributeString("LongMin", Min.ToString());
                        W.WriteAttributeString("LongDeg", Deg.ToString());
                    }
                    W.WriteAttributeString("Longitude", sLong.Trim());
                    W.WriteAttributeString("Latitude", sLat.Trim());
                    W.WriteEndElement(); // Gazetteer
                }
            }
        }

        private void writeXmlEventLocalisationGazetteer(ref System.Xml.XmlWriter W, int EventID, int LocalisationSystemID)
        {
            try
            {
                System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionEventLocalisation.Select("CollectionEventID = " + EventID.ToString() + " AND LocalisationSystemID = " + LocalisationSystemID.ToString());
                if (rr.Length > 0)
                {
                    // Check if URI is present
                    if (rr[0]["Location2"].Equals(System.DBNull.Value))
                        return;
                    // Check if the LocalisationSystemID belongs to a Gazetteer
                    System.Data.DataRow[] rrL = DiversityCollection.LookupTable.DtLocalisationSystem.Select("LocalisationSystemID = " + LocalisationSystemID.ToString() + " AND ParsingMethodName = 'Gazetteer'", "");
                    if (rrL.Length == 0)
                        return;
                    string GazetteerURI = (rr[0]["Location2"].ToString());
                    System.Collections.Generic.Dictionary<string, string> DictGazetteer = this.Gazetteer.UnitValues(GazetteerURI);
                    if (DictGazetteer.Count > 0)
                    {
                        W.WriteStartElement("Gazetteer"); // Gazetteer
                        int iG = 0;
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DictGazetteer)
                        {
                            if (!DictGazetteer.ContainsKey("Name_" + iG.ToString()))
                            {
                                iG++;
                                continue;
                            }
                            if (DictGazetteer["Name_" + iG.ToString()].Length == 0 ||
                                DictGazetteer["PlaceType_" + iG.ToString()].Length == 0)
                                break;
                            W.WriteStartElement("Hierarchy");
                            W.WriteAttributeString("PlaceType", DictGazetteer["PlaceType_" + iG.ToString()]);
                            W.WriteAttributeString("Name", DictGazetteer["Name_" + iG.ToString()]);
                            W.WriteAttributeString("Language", DictGazetteer["LanguageCode_" + iG.ToString()]);
                            W.WriteEndElement(); // Hierarchy
                            iG++;
                        }
                        W.WriteEndElement(); // Gazetteer
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void writeXmlEventProperty(ref System.Xml.XmlWriter W, int EventID)
        {
            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionEventProperty.Select("CollectionEventID = " + EventID.ToString());
            if (rr.Length > 0)
            {
                W.WriteStartElement("CollectionEventProperties");
                for (int i = 0; i < rr.Length; i++)
                {
                    W.WriteStartElement("Property");
                    foreach (System.Data.DataColumn C in this.dataSetCollectionSpecimen.CollectionEventProperty.Columns)
                    {
                        if (!rr[i][C.ColumnName].Equals(System.DBNull.Value) &&
                            rr[i][C.ColumnName].ToString().Length > 0 &&
                            C.ColumnName != "CollectionEventID" &&
                            C.ColumnName != "PropertyID" &&
                            C.ColumnName != "ResponsibleName" &&
                            C.ColumnName != "ResponsibleAgentURI")
                        {
                            W.WriteElementString(C.ColumnName, rr[i][C.ColumnName].ToString());
                        }
                    }
                    W.WriteEndElement();  // Property
                }
                W.WriteEndElement();  // CollectionEventProperties
            }
        }
        
        #endregion

        #region Parts etc.

        private void writeXmlPartsForSpecimen(ref System.Xml.XmlWriter W, int SpecimenID)
        {
            string SelectString = "CollectionSpecimenID = " + SpecimenID.ToString();
            if (this._CollectionID != null && this._RestrictToCollection)
                SelectString += " AND CollectionID = " + this._CollectionID.ToString();
            if (this._MaterialCategory.Length > 0 && this._RestrictToMaterial)
                SelectString += " AND MaterialCategory = '" + this._MaterialCategory + "'";
            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionSpecimenPart.Select(SelectString);
            if (rr.Length > 0)
            {
                W.WriteStartElement("SpecimenParts");
                for (int i = 0; i < rr.Length; i++)
                {
                    W.WriteStartElement("SpecimenPart");
                    if (!rr[i]["CollectionID"].Equals(System.DBNull.Value))
                    {
                        int C;
                        int.TryParse(rr[i]["CollectionID"].ToString(), out C);
                        this._CollectionID = C;
                        W.WriteElementString("CollectionOwner", DiversityCollection.LookupTable.CollectionOwner((int)this._CollectionID));
                    }
                    foreach (System.Data.DataColumn C in rr[i].Table.Columns)
                    {
                        if (!C.ColumnName.Equals(System.DBNull.Value) &&
                             C.ColumnName.ToString().Length > 0 &&
                             C.ColumnName != "CollectionSpecimenID" &&
                             C.ColumnName != "CollectionID" &&
                             C.ColumnName != "SpecimenPartID" &&
                             C.ColumnName != "DerivedFromSpecimenPartID")
                        {
                            if (!rr[i][C.ColumnName].Equals(System.DBNull.Value) && rr[i][C.ColumnName].ToString().Length > 0)
                                W.WriteElementString(C.ColumnName, rr[i][C.ColumnName].ToString());
                        }
                        int? CollID = null;
                        if (C.ColumnName == "CollectionID" && !rr[i]["CollectionID"].Equals(System.DBNull.Value))
                        {
                            int CollectionID = int.Parse(rr[i]["CollectionID"].ToString());
                            CollID = CollectionID;
                            string CollectionName = DiversityCollection.LookupTable.CollectionName(CollectionID);
                            W.WriteElementString("CollectionName", CollectionName);
                        }
                        int PartID;
                        if (int.TryParse(rr[i]["SpecimenPartID"].ToString(), out PartID))
                        {
                            this.writeXmlProcessing(ref W, SpecimenID, PartID);
                            this.writeXmlTransaction(ref W, SpecimenID, PartID, CollID);
                        }
                    }
                    W.WriteEndElement();//SpecimenPart
                }
                W.WriteEndElement();//SpecimenParts
            }
        }

        private void writeXmlPart(ref System.Xml.XmlWriter W, int SpecimenID)
        {
            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionSpecimenPart.Select(
                "CollectionSpecimenID = " + SpecimenID.ToString()
                + " AND CollectionID = " + this._CollectionID.ToString()
                + " AND MaterialCategory = '" + this._MaterialCategory + "'");
            if (rr.Length > 0)
            {
                W.WriteStartElement("SpecimenParts");
                for (int i = 0; i < rr.Length; i++)
                {
                    W.WriteStartElement("SpecimenPart");
                    W.WriteElementString("CollectionOwner", DiversityCollection.LookupTable.CollectionOwner((int)this._CollectionID));
                    foreach (System.Data.DataColumn C in rr[i].Table.Columns)
                    {
                        if (!C.ColumnName.Equals(System.DBNull.Value) &&
                             C.ColumnName.ToString().Length > 0 &&
                             C.ColumnName != "CollectionSpecimenID" &&
                             C.ColumnName != "CollectionID" &&
                             C.ColumnName != "SpecimenPartID" &&
                             C.ColumnName != "DerivedFromSpecimenPartID")
                        {
                            if (!rr[i][C.ColumnName].Equals(System.DBNull.Value) && rr[i][C.ColumnName].ToString().Length > 0)
                                W.WriteElementString(C.ColumnName, rr[i][C.ColumnName].ToString());
                        }
                        //if (!rr[i][C.ColumnName].Equals(System.DBNull.Value) &&
                        //    C.ColumnName == "CollectionID")
                        //{
                        //    W.WriteElementString("Collection", DiversityCollection.LookupTable.CollectionName((int)this._CollectionID));
                        //}
                    }
                    this.writeXmlProcessing(ref W, SpecimenID, int.Parse(rr[i]["SpecimenPartID"].ToString()));
                    if (!rr[i]["CollectionID"].Equals(System.DBNull.Value))
                    {
                        int CollectionID = int.Parse(rr[i]["CollectionID"].ToString());
                        this.writeXmlTransaction(ref W, SpecimenID, int.Parse(rr[i]["SpecimenPartID"].ToString()), CollectionID);
                    }
                    else
                        this.writeXmlTransaction(ref W, SpecimenID, int.Parse(rr[i]["SpecimenPartID"].ToString()), null);
                    W.WriteEndElement();
                    //this.writeXmlUnits(ref W, SpecimenID, int.Parse(rr[i]["SpecimenPartID"].ToString()));
                }
                W.WriteEndElement();
            }
        }

        private void writeXmlProcessing(ref System.Xml.XmlWriter W, int SpecimenID, int PartID)
        {
            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionSpecimenProcessing.Select("CollectionSpecimenID = " + SpecimenID.ToString() + " AND SpecimenPartID = " + PartID.ToString());
            if (rr.Length > 0)
            {
                W.WriteStartElement("Processings");
                for (int i = 0; i < rr.Length; i++)
                {
                    W.WriteStartElement("Processing");
                    W.WriteElementString("ProcessingName", DiversityCollection.LookupTable.ProcessingName(int.Parse(rr[i]["ProcessingID"].ToString())));
                    foreach (System.Data.DataColumn C in rr[i].Table.Columns)
                    {
                        if (!C.ColumnName.Equals(System.DBNull.Value) &&
                             C.ColumnName.ToString().Length > 0 &&
                             C.ColumnName != "CollectionSpecimenID" &&
                             C.ColumnName != "SpecimenPartID" &&
                             C.ColumnName != "MaterialCategory" &&
                             C.ColumnName != "ProcessingID")
                        {
                            if (!rr[i][C.ColumnName].Equals(System.DBNull.Value) && rr[i][C.ColumnName].ToString().Length > 0)
                                W.WriteElementString(C.ColumnName, rr[i][C.ColumnName].ToString());
                        }
                    }
                    W.WriteEndElement();
                }
                W.WriteEndElement();
            }
        }

        private void writeXmlTransaction(ref System.Xml.XmlWriter W, int SpecimenID, int PartID, int? CollectionID)
        {
            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionSpecimenTransaction.Select("CollectionSpecimenID = " + SpecimenID.ToString() + " AND SpecimenPartID = " + PartID.ToString());
            if (rr.Length > 0)
            {
                W.WriteStartElement("Transactions");
                for (int i = 0; i < rr.Length; i++)
                {
                    W.WriteStartElement("Transaction");
                    W.WriteElementString("TransactionType", DiversityCollection.LookupTable.TransactionType(int.Parse(rr[i]["TransactionID"].ToString())));
                    W.WriteElementString("TransactionTitle", DiversityCollection.LookupTable.TransactionTitle (int.Parse(rr[i]["TransactionID"].ToString())));
                    if (CollectionID != null)
                        W.WriteElementString("TransactionNumber", DiversityCollection.LookupTable.TransactionNumber(int.Parse(rr[i]["TransactionID"].ToString()), (int)CollectionID));
                    W.WriteEndElement();
                }
                W.WriteEndElement();
            }
        }
        
        #endregion

        private void writeXmlSpecimenImages(ref System.Xml.XmlWriter W, int SpecimenID)
        {
            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionSpecimenImage.Select("CollectionSpecimenID = " + SpecimenID.ToString() + " AND ImageType = 'label'");
            if (rr.Length > 0)
            {
                W.WriteStartElement("ImagesOfLabels");
                for (int i = 0; i < rr.Length; i++)
                {
                    W.WriteStartElement("ImageOfLabel");
                    foreach (System.Data.DataColumn C in rr[i].Table.Columns)
                    {
                        if (C.ColumnName == "URI")
                        {
                            W.WriteElementString(C.ColumnName, rr[i][C.ColumnName].ToString());
                        }
                    }
                    W.WriteEndElement();  // ImageOfLabel
                }
                W.WriteEndElement();  //  ImagesOfLabels
            }
        }

        #region Agents

        private void writeXmlAgents(ref System.Xml.XmlWriter W, int SpecimenID)
        {
            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionAgent.Select("CollectionSpecimenID = " + SpecimenID.ToString(), "CollectorsSequence ASC");
            if (rr.Length > 0)
            {
                W.WriteStartElement("Collectors");
                for (int i = 0; i < rr.Length; i++)
                {
                    W.WriteStartElement("Collector");
                    foreach (System.Data.DataColumn C in this.dataSetCollectionSpecimen.CollectionAgent.Columns)
                    {
                        if (!rr[i][C.ColumnName].Equals(System.DBNull.Value) &&
                            rr[i][C.ColumnName].ToString().Length > 0 &&
                            C.ColumnName != "CollectionSpecimenID" &&
                            C.ColumnName != "CollectorsAgentURI" &&
                            C.ColumnName != "CollectorsSequence" &&
                            C.ColumnName != "DataWithholdingReason")
                        {
                            W.WriteElementString(C.ColumnName, rr[i][C.ColumnName].ToString());
                            if (C.ColumnName == "CollectorsName")
                                this.writeXmlAgent(ref W, rr[i][C.ColumnName].ToString());
                        }
                    }
                    W.WriteEndElement();  // Collector
                }
                W.WriteEndElement();  // Collectors
            }
        }

        private void writeXmlAgent(ref System.Xml.XmlWriter W, string AgentName)
        {
            if (AgentName.Length > 0)
            {
                string Title = "";
                if (AgentName.Contains("Dr."))
                {
                    Title = "Dr.";
                    AgentName = AgentName.Replace("Dr.", "").Trim();
                    if (AgentName.EndsWith(","))
                        AgentName = AgentName.Remove(AgentName.Length - 1);
                    if (AgentName.StartsWith(","))
                        AgentName = AgentName.Substring(1).Trim();
                }
                if (AgentName.Contains("Prof."))
                {
                    if (Title.Length > 0)
                        Title = "Prof. " + Title;
                    else
                        Title = "Prof.";
                    AgentName = AgentName.Replace("Prof.", "").Trim();
                    if (AgentName.EndsWith(","))
                        AgentName = AgentName.Remove(AgentName.Length - 1);
                    if (AgentName.StartsWith(","))
                        AgentName = AgentName.Substring(1).Trim();
                }
                string FirstName = "";
                string FirstNameAbbreviation = "";
                string SecondName = AgentName;
                if (AgentName.IndexOf(", ") > 0
                    && AgentName.IndexOf("?") == -1
                    && AgentName.IndexOf("(") == -1
                    && AgentName.IndexOf(", ") == AgentName.LastIndexOf(", "))
                {
                    SecondName = AgentName.Substring(0, AgentName.IndexOf(", "));
                    FirstName = AgentName.Substring(AgentName.IndexOf(", ") + 2).Trim();
                    FirstNameAbbreviation = this.FirstNameAbbreviation(FirstName);// FirstName.Substring(0, 1) + ".";
                }
                else if (AgentName.IndexOf(' ') > -1)
                {
                    SecondName = AgentName.Substring(AgentName.LastIndexOf(" ") + 1).Trim();
                    FirstName = AgentName.Substring(0, AgentName.LastIndexOf(" ")).Trim();
                    FirstNameAbbreviation = this.FirstNameAbbreviation(FirstName);// FirstName.Substring(0, 1) + ".";
                }
                W.WriteStartElement("Agent");
                W.WriteElementString("SecondName", SecondName);
                if (FirstName.Length > 0)
                {
                    W.WriteElementString("FirstName", FirstName);
                    W.WriteElementString("FirstNameAbbreviation", FirstNameAbbreviation);
                }
                if (Title.Length > 0)
                {
                    W.WriteElementString("Title", Title);
                }
                W.WriteEndElement();  // Agent
            }
        }

        private string FirstNameAbbreviation(string FirstName)
        {
            string Abbreviation = "";
            try
            {
                if (FirstName.IndexOf("-") > -1)
                {
                    string[] AAm = FirstName.Split(new char[] { '-' });
                    foreach (string A in AAm)
                    {
                        if (Abbreviation.Length > 0)
                            Abbreviation += "-";
                        if (A.IndexOf(".") > -1)
                            Abbreviation += A;
                        else
                            Abbreviation += A.Substring(0, 1) + ".";
                    }
                }
                if (Abbreviation.Length == 0)
                    Abbreviation = FirstName;
                string[] AA = Abbreviation.Split(new char[] { ' ' });
                Abbreviation = "";
                foreach (string A in AA)
                {
                    if (Abbreviation.Length > 0)
                        Abbreviation += " ";
                    if (A.IndexOf(".") > -1)
                        Abbreviation += A;
                    else if (A.Length > 0)
                        Abbreviation += A.Substring(0, 1) + ".";
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Abbreviation;
        }
        
        #endregion

        private bool writeXmlUnits(ref System.Xml.XmlWriter W, int SpecimenID, int? PartID)
        {
            bool OK = true;
            System.Data.DataRow[] rr;
            if (this._LabelForSpecimen)
            {
                rr = this.dataSetCollectionSpecimen.IdentificationUnit.Select("CollectionSpecimenID = " + SpecimenID.ToString() + " AND DisplayOrder > 0", "DisplayOrder ASC");
            }
            else
            {
                System.Data.DataTable dtUnits = this.dataSetCollectionSpecimen.IdentificationUnit.Copy();
                dtUnits.Clear();
                if (this.dataSetCollectionSpecimen.IdentificationUnit.Rows.Count == 1)
                {
                    rr = this.dataSetCollectionSpecimen.IdentificationUnit.Select("DisplayOrder > 0", "DisplayOrder ASC");
                }
                else
                {
                    foreach (DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitRow RU in this.dataSetCollectionSpecimen.IdentificationUnit.Rows)
                    {
                        foreach (DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartRow RP in this.dataSetCollectionSpecimen.IdentificationUnitInPart.Rows)
                        {
                            if (RU.CollectionSpecimenID == SpecimenID
                                && RP.SpecimenPartID == PartID
                                && RP.IdentificationUnitID == RU.IdentificationUnitID
                                && RP.CollectionSpecimenID == SpecimenID
                                && RP.DisplayOrder > 0)
                            {
                                try
                                {
                                    DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitRow Rnew
                                        = (DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitRow)dtUnits.NewRow();
                                    Rnew.CollectionSpecimenID = SpecimenID;
                                    Rnew.IdentificationUnitID = RU.IdentificationUnitID;
                                    if (!RU.LastIdentificationCache.Equals(System.DBNull.Value))
                                        Rnew.LastIdentificationCache = RU.LastIdentificationCache;
                                    try
                                    {
                                        if (!RU["FamilyCache"].Equals(System.DBNull.Value))
                                            Rnew.FamilyCache = RU.FamilyCache;
                                    }
                                    catch { }
                                    try
                                    {
                                        if (!RU["OrderCache"].Equals(System.DBNull.Value))
                                            Rnew.OrderCache = RU.OrderCache;
                                    }
                                    catch { }
                                    if (!RU.TaxonomicGroup.Equals(System.DBNull.Value))
                                        Rnew.TaxonomicGroup = RU.TaxonomicGroup;
                                    try
                                    {
                                        if (!RU["OnlyObserved"].Equals(System.DBNull.Value))
                                            Rnew.OnlyObserved = RU.OnlyObserved;
                                    }
                                    catch { }
                                    try
                                    {
                                        if (!RU["RelatedUnitID"].Equals(System.DBNull.Value))
                                            Rnew.RelatedUnitID = RU.RelatedUnitID;
                                    }
                                    catch { }
                                    try
                                    {
                                        if (!RU["RelationType"].Equals(System.DBNull.Value))
                                            Rnew.RelationType = RU.RelationType;
                                    }
                                    catch { }
                                    try
                                    {
                                        if (!RU["ColonisedSubstratePart"].Equals(System.DBNull.Value))
                                            Rnew.ColonisedSubstratePart = RU.ColonisedSubstratePart;
                                    }
                                    catch { }
                                    try
                                    {
                                        if (!RU["LifeStage"].Equals(System.DBNull.Value))
                                            Rnew.LifeStage = RU.LifeStage;
                                    }
                                    catch { }
                                    try
                                    {
                                        if (!RU["Gender"].Equals(System.DBNull.Value))
                                            Rnew.Gender = RU.Gender;
                                    }
                                    catch { }
                                    try
                                    {
                                        if (!RU["NumberOfUnits"].Equals(System.DBNull.Value))
                                            Rnew.NumberOfUnits = RU.NumberOfUnits;
                                    }
                                    catch { }
                                    try
                                    {
                                        if (!RU["ExsiccataNumber"].Equals(System.DBNull.Value))
                                            Rnew.ExsiccataNumber = RU.ExsiccataNumber;
                                    }
                                    catch { }
                                    try
                                    {
                                        if (!RU["ExsiccataIdentification"].Equals(System.DBNull.Value))
                                            Rnew.ExsiccataIdentification = RU.ExsiccataIdentification;
                                    }
                                    catch { }
                                    try
                                    {
                                        if (!RU["Circumstances"].Equals(System.DBNull.Value))
                                            Rnew.Circumstances = RU.Circumstances;
                                    }
                                    catch { }
                                    try
                                    {
                                        if (!RU["DisplayOrder"].Equals(System.DBNull.Value))
                                            Rnew.DisplayOrder = RP.DisplayOrder;
                                    }
                                    catch { }
                                    try
                                    {
                                        if (!RU["Notes"].Equals(System.DBNull.Value))
                                            Rnew.Notes = RU.Notes;
                                    }
                                    catch { }
                                    try
                                    {
                                        if (!RU["HierarchyCache"].Equals(System.DBNull.Value))
                                            Rnew.HierarchyCache = RU.HierarchyCache;
                                    }
                                    catch { }
                                    dtUnits.Rows.Add(Rnew);
                                }
                                catch { }
                            }
                        }
                    }
                    rr = dtUnits.Select("DisplayOrder > 0", "DisplayOrder ASC");
                }
            }
            if (rr.Length > 0)
            {
                W.WriteStartElement("Units");
                for (int i = 0; i < rr.Length; i++)
                {
                    string Unit = "Unit";
                    if (i == 0)
                        Unit = "MainUnit";
                    else if (!rr[0]["RelatedUnitID"].Equals(System.DBNull.Value))
                    {
                        if (rr[i][1].ToString() == rr[0]["RelatedUnitID"].ToString())
                            Unit = "SubstrateUnit";
                        else if (rr[i]["RelatedUnitID"].ToString() == rr[0]["RelatedUnitID"].ToString())
                            Unit = "AssociatedUnit";
                    }
                    if (!rr[i]["RelatedUnitID"].Equals(System.DBNull.Value))
                    {
                        if (rr[i]["RelatedUnitID"].ToString() == rr[0][1].ToString())
                        {
                            Unit = "GrowingOnUnit";
                        }
                    }
                    W.WriteStartElement(Unit);
                    foreach (System.Data.DataColumn C in this.dataSetCollectionSpecimen.IdentificationUnit.Columns)
                    {
                        if (!rr[i][C.ColumnName].Equals(System.DBNull.Value) &&
                            rr[i][C.ColumnName].ToString().Length > 0 &&
                            C.ColumnName != "CollectionSpecimenID" &&
                            C.ColumnName != "IdentificationUnitID" &&
                            C.ColumnName != "LastIdentificationCache" &&
                            C.ColumnName != "ExsiccataIdentification" &&
                            C.ColumnName != "RelatedUnitID" &&
                            C.ColumnName != "DisplayOrder" &&
                            C.ColumnName != "OnlyObserved")
                        {
                            W.WriteElementString(C.ColumnName, rr[i][C.ColumnName].ToString());
                        }
                        else if (!rr[i][C.ColumnName].Equals(System.DBNull.Value) &&
                            C.ColumnName == "RelatedUnitID"
                            && Unit == "Unit")
                        {
                            System.Data.DataRow[] rrS = this.dataSetCollectionSpecimen.IdentificationUnit.Select("IdentificationUnitID = " + rr[i]["RelatedUnitID"].ToString());
                            W.WriteElementString("Substrate", rrS[0]["LastIdentificationCache"].ToString());
                        }
                    }
                    int UnitID = int.Parse(rr[i]["IdentificationUnitID"].ToString());
                    if (!this.writeXmlIdentifications(ref W, SpecimenID, UnitID))
                        OK = false;
                    this.writeXmlAnalysis(ref W, SpecimenID, UnitID);
                    this.writeXmlExternalIdentifier(ref W, "IdentificationUnit", UnitID);
                    W.WriteEndElement();  // Unit
                }
                W.WriteEndElement(); // Units
            }
            return OK;
        }

        private void writeXmlAnalysis(ref System.Xml.XmlWriter W, int SpecimenID, int UnitID)
        {
            System.Data.DataRow[] rrI = this.dataSetCollectionSpecimen.IdentificationUnitAnalysis.Select("CollectionSpecimenID = " + SpecimenID.ToString() + " AND IdentificationUnitID = " + UnitID, "AnalysisNumber");
            if (rrI.Length > 0)
            {
                W.WriteStartElement("UnitAnalysis");
                for (int I = 0; I < rrI.Length; I++)
                {
                    W.WriteStartElement("Analysis");
                    System.Data.DataRow[] rrA = DiversityCollection.LookupTable.DtAnalysis.Select("AnalysisID = " + rrI[I]["AnalysisID"].ToString());
                    if (rrA.Length > 0)
                    {
                        W.WriteElementString("AnalysisName", rrA[0]["DisplayText"].ToString());
                        W.WriteElementString("MeasurementUnit", rrA[0]["MeasurementUnit"].ToString());
                    }
                    foreach (System.Data.DataColumn C in rrI[I].Table.Columns)
                    {
                        if (!rrI[I][C.ColumnName].Equals(System.DBNull.Value) &&
                            rrI[I][C.ColumnName].ToString().Length > 0 &&
                            C.ColumnName != "CollectionSpecimenID" &&
                            C.ColumnName != "IdentificationUnitID" &&
                            C.ColumnName != "AnalysisID" &&
                            C.ColumnName != "ExternalAnalysisURI")
                        {
                            W.WriteElementString(C.ColumnName, rrI[I][C.ColumnName].ToString());
                        }
                    }
                    W.WriteEndElement();
                }
                W.WriteEndElement();
            }
        }

        #region Identification and taxonomy

        private bool writeXmlIdentifications(ref System.Xml.XmlWriter W, int SpecimenID, int UnitID)
        {
            bool OK = true;
            System.Data.DataRow[] rrI = this.dataSetCollectionSpecimen.Identification.Select("CollectionSpecimenID = " + SpecimenID.ToString() + " AND IdentificationUnitID = " + UnitID + " AND DependsOnIdentificationSequence IS NULL", "IdentificationSequence DESC");
            if (rrI.Length > 0)
            {
                W.WriteStartElement("Identifications");
                for (int I = 0; I < rrI.Length; I++)
                {
                    W.WriteStartElement("Identification");
                    string IdentificationSequence = "";
                    foreach (System.Data.DataColumn C in rrI[I].Table.Columns)
                    {
                        if (!rrI[I][C.ColumnName].Equals(System.DBNull.Value) &&
                            rrI[I][C.ColumnName].ToString().Length > 0 &&
                            C.ColumnName != "CollectionSpecimenID" &&
                            C.ColumnName != "IdentificationUnitID" &&
                            C.ColumnName != "IdentificationSequence" &&
                            C.ColumnName != "NameURI" &&
                            C.ColumnName != "ReferenceURI" &&
                            C.ColumnName != "ResponsibleAgentURI" &&
                            C.ColumnName != "IdentificationDate")
                        {
                            W.WriteElementString(C.ColumnName, rrI[I][C.ColumnName].ToString());
                            if (C.ColumnName == "ResponsibleName")
                                this.writeXmlAgent(ref W, rrI[I][C.ColumnName].ToString());
                        }
                        else if (C.ColumnName == "IdentificationSequence")
                        {
                            IdentificationSequence = rrI[I][C.ColumnName].ToString();
                        }
                        string QualifierText = "";
                        string QualifierRank = "";
                        this.IdentificationQualifier(rrI[I], ref QualifierText, ref QualifierRank);
                        if (!rrI[I][C.ColumnName].Equals(System.DBNull.Value) &&
                            rrI[I][C.ColumnName].ToString() != "" &&
                            C.ColumnName == "TaxonomicName")
                        {
                            this.writeXmlTaxonomicName(ref W, rrI[I]["TaxonomicName"].ToString(), QualifierText, QualifierRank);
                        }
                        if (QualifierText.Length > 0 && !rrI[I][C.ColumnName].Equals(System.DBNull.Value) &&
                            rrI[I][C.ColumnName].ToString() != "" &&
                            C.ColumnName == "IdentificationQualifier")
                        {
                            W.WriteElementString("QualifierText", QualifierText);
                            W.WriteElementString("QualifierRank", QualifierRank);
                        }
                        if ((rrI[I][C.ColumnName].Equals(System.DBNull.Value) ||
                            rrI[I][C.ColumnName].ToString().Length == 0) &&
                            C.ColumnName == "IdentificationQualifier")
                        {
                            W.WriteElementString(C.ColumnName, "");
                            W.WriteElementString("QualifierText", "");
                        }
                        if (C.ColumnName == "IdentificationDate")
                        {
                            System.DateTime Date = System.DateTime.Now;
                            if (System.DateTime.TryParse(rrI[I][C.ColumnName].ToString(), out Date))
                            {
                                string L = Date.ToShortDateString();
                                W.WriteElementString(C.ColumnName, L);
                            }
                        }
                        if (!rrI[I][C.ColumnName].Equals(System.DBNull.Value) &&
                            C.ColumnName == "NameURI")
                        {
                            this.writeXmlTaxon(ref W, rrI[I][C.ColumnName].ToString());
                        }

                        // Terms
                        if (!rrI[I][C.ColumnName].Equals(System.DBNull.Value) &&
                            C.ColumnName == "TermURI")
                        {
                            if (!this.writeXmlTerm(ref W, rrI[I][C.ColumnName].ToString()))
                                OK = false;
                        }

                    }
                    int iSequ; 
                    if (IdentificationSequence.Length > 0 && int.TryParse(IdentificationSequence, out iSequ))
                    {
                        if (!this.writeXmlIdentificationsDependent(ref W, SpecimenID, UnitID, iSequ))
                            OK = false;
                    }
                    W.WriteEndElement();  // Identification
                }
                W.WriteEndElement(); // Identifications
            }
            return OK;
        }

        private bool writeXmlIdentificationsDependent(ref System.Xml.XmlWriter W, int SpecimenID, int UnitID, int DependsOnIdentificationSequence)
        {
            bool OK = true;
            System.Data.DataRow[] rrI = this.dataSetCollectionSpecimen.Identification.Select("CollectionSpecimenID = " + SpecimenID.ToString() + " AND IdentificationUnitID = " + UnitID + " AND DependsOnIdentificationSequence = " + DependsOnIdentificationSequence.ToString(), "IdentificationSequence DESC");
            if (rrI.Length > 0)
            {
                W.WriteStartElement("DependentIdentifications");
                for (int I = 0; I < rrI.Length; I++)
                {
                    W.WriteStartElement("Identification");
                    foreach (System.Data.DataColumn C in rrI[I].Table.Columns)
                    {
                        if (!rrI[I][C.ColumnName].Equals(System.DBNull.Value) &&
                            rrI[I][C.ColumnName].ToString().Length > 0 &&
                            C.ColumnName != "CollectionSpecimenID" &&
                            C.ColumnName != "IdentificationUnitID" &&
                            C.ColumnName != "IdentificationSequence" &&
                            C.ColumnName != "NameURI" &&
                            C.ColumnName != "ReferenceURI" &&
                            C.ColumnName != "ResponsibleAgentURI" &&
                            C.ColumnName != "IdentificationDate")
                        {
                            W.WriteElementString(C.ColumnName, rrI[I][C.ColumnName].ToString());
                            if (C.ColumnName == "ResponsibleName")
                                this.writeXmlAgent(ref W, rrI[I][C.ColumnName].ToString());
                        }
                        if (C.ColumnName == "IdentificationDate")
                        {
                            System.DateTime Date = System.DateTime.Now;
                            if (System.DateTime.TryParse(rrI[I][C.ColumnName].ToString(), out Date))
                            {
                                string L = Date.ToShortDateString();
                                W.WriteElementString(C.ColumnName, L);
                            }
                        }
                        if (!rrI[I][C.ColumnName].Equals(System.DBNull.Value) &&
                            C.ColumnName == "TermURI")
                        {
                            if (!this.writeXmlTerm(ref W, rrI[I][C.ColumnName].ToString()))
                                OK = false;
                        }
                    }
                    W.WriteEndElement();  // Identification
                }
                W.WriteEndElement(); // Identifications
            }
            return OK;
        }

        private void IdentificationQualifier(System.Data.DataRow R, ref string QualifierText, ref string QualifierRank)
        {
            foreach (System.Data.DataColumn C in R.Table.Columns)
            {
                if (!R[C.ColumnName].Equals(System.DBNull.Value) &&
                    R[C.ColumnName].ToString() != "" &&
                    C.ColumnName == "IdentificationQualifier")
                {
                    string Qualifier = R["IdentificationQualifier"].ToString();
                    switch (Qualifier)
                    {
                        case "aff. forma":
                        case "aff. gen.":
                        case "aff. sp.":
                        case "aff. ssp.":
                        case "aff. var.":
                            QualifierText = "aff.";
                            QualifierRank = Qualifier.Substring(5);
                            break;
                        case "cf. forma":
                        case "cf. gen.":
                        case "cf. sp.":
                        case "cf. ssp.":
                        case "cf. var.":
                            QualifierText = "cf.";
                            QualifierRank = Qualifier.Substring(4);
                            break;
                        case "?":
                        case "agg.":
                        case "s. l.":
                        case "s. str.":
                        case "sp.":
                        case "sp. nov.":
                        case "spp.":
                            QualifierText = Qualifier;
                            QualifierRank = "";
                            break;
                    }
                }
            }
        }

        private void writeXmlTaxonomicName(ref System.Xml.XmlWriter W, string Taxon)
        {
            // clear of surplus space
            string OriTaxon = Taxon.Trim();
            string[] TaxonParts;
            while (OriTaxon.IndexOf("  ") > -1) OriTaxon = OriTaxon.Replace("  ", " ");
            if (OriTaxon.Length == 0) return;
            W.WriteStartElement("Taxon");
            if (OriTaxon.IndexOf(" x ") > -1
                ||
                OriTaxon.IndexOf(" X ") > -1
                ||
                OriTaxon.IndexOf(" + ") > -1
                ||
                OriTaxon.IndexOf(" × ") > -1)
            {
                string[] HypridParts = OriTaxon.Split(new char[] { 'x', 'X', '×', '+' });
                for (int i = 0; i < HypridParts.Length; i++)
                {
                    TaxonParts = HypridParts[i].Trim().Split(new char[] { ' ' });
                    this.writeXmlTaxonomicNamePart(ref W, TaxonParts, "×", i);
                }
            }
            else
            {
                TaxonParts = OriTaxon.Split(new char[] { ' ' });
                this.writeXmlTaxonomicNamePart(ref W, TaxonParts, "", 0);
            }
            W.WriteEndElement(); // Taxon
        }

        private void writeXmlTaxonomicName(ref System.Xml.XmlWriter W, string Taxon, string Qualifier, string QualifierRank)
        {
            // clear of surplus space
            string OriTaxon = Taxon.Trim();
            string[] TaxonParts;
            while (OriTaxon.IndexOf("  ") > -1) OriTaxon = OriTaxon.Replace("  ", " ");
            if (OriTaxon.Length == 0) return;
            W.WriteStartElement("Taxon");


            if (OriTaxon.IndexOf(" x ") > -1
                ||
                OriTaxon.IndexOf(" X ") > -1
                ||
                OriTaxon.IndexOf(" + ") > -1
                ||
                OriTaxon.IndexOf(" × ") > -1)
            {
                System.Collections.Generic.List<string[]> Hybrids = new List<string[]>();
                string[] HybridParts = OriTaxon.Split(new char[] { 'x', 'X', '×', '+' });
                for (int i = 0; i < HybridParts.Length; i++)
                {
                    Hybrids.Add(HybridParts[i].Trim().Split(new char[] { ' ' }));
                }
            }
            else
            {
                TaxonParts = OriTaxon.Split(new char[] { ' ' });
                this.writeXmlTaxonomicNamePart(ref W, TaxonParts, "", 0);
            }
            if (Qualifier.Length > 0)
            {
                W.WriteElementString("Qualifier", Qualifier);
                W.WriteElementString("QualifierRank", QualifierRank);
            }

            W.WriteEndElement(); // Taxon
        }

        private void writeXmlHybridName(ref System.Xml.XmlWriter W, string Taxon, string Qualifier, string QualifierRank)
        {
            string Separator = "×";
            if (Taxon.IndexOf(" + ") > -1)
                Separator = "+";
            System.Collections.Generic.List<string[]> Hybrids = new List<string[]>();
            string[] HybridParts = Taxon.Split(new char[] { 'x', 'X', '×', '+' });
            for (int i = 0; i < HybridParts.Length; i++)
            {
                Hybrids.Add(HybridParts[i].Trim().Split(new char[] { ' ' }));
            }
        }

        private void writeXmlTaxonomicNamePart(ref System.Xml.XmlWriter W, string[] TaxonParts, string Separator, int Position)
        {
            this._RankFound = false;

            int PositionOfAnalysis = 0;
            //string Genus = "";
            string SpeciesEpithet = "";
            string InfraspecificEpithet = "";

            W.WriteStartElement("TaxonPart");
            if (Separator.Length > 0 && Position > 0)
                W.WriteElementString(_NameParts.HybridSeparator.ToString(), Separator);

            // Analysing the parts
            if (TaxonParts.Length == 1) // only one part
            {
                if (TaxonParts[PositionOfAnalysis].Length > 0 && !this._RankFound)
                {
                    W.WriteElementString(_NameParts.Genus.ToString(), TaxonParts[PositionOfAnalysis]);
                    W.WriteElementString(_NameParts.Rank.ToString(), "gen.");
                    this._RankFound = true;
                }
            }
            else // more than one part
            {
                if (TaxonParts[0] == "x" || TaxonParts[0] == "X" || TaxonParts[0] == "×" || TaxonParts[0] == "+")
                {
                    W.WriteElementString(XmlExport._IdentificationQualifierType.QualifierLeading.ToString(), TaxonParts[0]);
                    PositionOfAnalysis++;
                }
                this.writeXmlTaxonomicNameQualifier(ref W, TaxonParts, ref PositionOfAnalysis, XmlExport._IdentificationQualifierType.QualifierLeading);
                // Genus
                if (TaxonParts[PositionOfAnalysis].Length > 0
                    &&
                    (Position == 0
                    ||
                    TaxonParts[PositionOfAnalysis].Substring(0, 1).ToUpper() == TaxonParts[PositionOfAnalysis].Substring(0, 1)))
                    W.WriteElementString(_NameParts.Genus.ToString(), TaxonParts[PositionOfAnalysis]);
                else if (TaxonParts[PositionOfAnalysis].Length > 0
                    &&
                    (Position > 0
                    ||
                    TaxonParts[PositionOfAnalysis].Substring(0, 1).ToLower() == TaxonParts[PositionOfAnalysis].Substring(0, 1)))
                    W.WriteElementString(_NameParts.SpeciesEpithet.ToString(), TaxonParts[PositionOfAnalysis]);
                PositionOfAnalysis++;
                if (TaxonParts.Length > PositionOfAnalysis)
                {
                    if (Position == 0
                        ||
                        TaxonParts[0].Substring(0, 1).ToUpper() == TaxonParts[0].Substring(0, 1))
                        this.writeXmlTaxonomicNameAuthors(ref W, TaxonParts, ref PositionOfAnalysis, XmlExport._AuthorType.AuthorsGenus);
                    else if (Position > 0
                        &&
                        TaxonParts[0].Substring(0, 1).ToLower() == TaxonParts[0].Substring(0, 1))
                        this.writeXmlTaxonomicNameAuthors(ref W, TaxonParts, ref PositionOfAnalysis, XmlExport._AuthorType.AuthorsSpecies);
                    if (TaxonParts.Length == PositionOfAnalysis
                        &&
                        Position == 0
                        &&
                        !this._RankFound)
                    {
                        W.WriteElementString(_NameParts.Rank.ToString(), "gen.");
                        this._RankFound = true;
                    }
                    else if (TaxonParts.Length == PositionOfAnalysis
                        &&
                        Position > 0
                        &&
                        !this._RankFound)
                    {
                        W.WriteElementString(_NameParts.Rank.ToString(), "sp.");
                        this._RankFound = true;
                    }
                    else if (TaxonParts.Length > PositionOfAnalysis)
                    {
                        // Qualifier after genus
                        this.writeXmlTaxonomicNameQualifier(ref W, TaxonParts, ref PositionOfAnalysis, XmlExport._IdentificationQualifierType.QualifierSpecies);
                        this.writeXmlTaxonomicNameRank(ref W, TaxonParts, ref PositionOfAnalysis);
                        // Infrageneric Epithet
                        if (TaxonParts.Length > PositionOfAnalysis)
                        {
                            if (TaxonParts[PositionOfAnalysis].Substring(0, 1).ToUpper() == TaxonParts[PositionOfAnalysis].Substring(0, 1)
                                &&
                                this._RankFound
                                &&
                                TaxonParts[PositionOfAnalysis - 1] == "subgen.")
                            {
                                W.WriteElementString(_NameParts.InfragenericEpithet.ToString(), TaxonParts[PositionOfAnalysis]);
                                PositionOfAnalysis++;
                                this.writeXmlTaxonomicNameAuthors(ref W, TaxonParts, ref PositionOfAnalysis, XmlExport._AuthorType.AuthorsInfrageneric);
                            }
                            if (TaxonParts.Length > PositionOfAnalysis)
                            {
                                // Hybrid
                                if (TaxonParts[PositionOfAnalysis] == "x" || TaxonParts[PositionOfAnalysis] == "X" || TaxonParts[PositionOfAnalysis] == "×")
                                {
                                    W.WriteElementString(_NameParts.HybridSeparator.ToString(), TaxonParts[PositionOfAnalysis]);
                                    //W.WriteElementString(XmlExport._IdentificationQualifierType.QualifierSpecies.ToString(), TaxonParts[PositionOfAnalysis]);
                                    PositionOfAnalysis++;
                                }
                                // Epithet
                                SpeciesEpithet = TaxonParts[PositionOfAnalysis];
                                if (SpeciesEpithet.Length > 0)
                                    W.WriteElementString(_NameParts.SpeciesEpithet.ToString(), SpeciesEpithet);
                                PositionOfAnalysis++;
                                if (TaxonParts.Length > PositionOfAnalysis)
                                {
                                    this.writeXmlTaxonomicNameAuthors(ref W, TaxonParts, ref PositionOfAnalysis, XmlExport._AuthorType.AuthorsSpecies);
                                    this.writeXmlTaxonomicNameRank(ref W, TaxonParts, ref PositionOfAnalysis);
                                    this.writeXmlTaxonomicNameQualifier(ref W, TaxonParts, ref PositionOfAnalysis, XmlExport._IdentificationQualifierType.QualifierInfraspecific);
                                    this.writeXmlTaxonomicNameAuthors(ref W, TaxonParts, ref PositionOfAnalysis, XmlExport._AuthorType.AuthorsSpecies);
                                    if (TaxonParts.Length > PositionOfAnalysis)
                                    {
                                        this.writeXmlTaxonomicNameRank(ref W, TaxonParts, ref PositionOfAnalysis);
                                        if (TaxonParts.Length > PositionOfAnalysis)
                                        {
                                            // InfraspecificEpithet
                                            InfraspecificEpithet = TaxonParts[PositionOfAnalysis];
                                            if (InfraspecificEpithet.Length > 0)
                                                W.WriteElementString(_NameParts.InfraspecificEpithet.ToString(), InfraspecificEpithet);
                                            PositionOfAnalysis++;
                                            if (TaxonParts.Length > PositionOfAnalysis)
                                            {
                                                // After InfraspecificEpithet
                                                this.writeXmlTaxonomicNameAuthors(ref W, TaxonParts, ref PositionOfAnalysis, XmlExport._AuthorType.AuthorsInfraspecific);
                                                if (TaxonParts.Length > PositionOfAnalysis)
                                                {
                                                    // Rest
                                                    string Undefined = "";
                                                    while (TaxonParts.Length > PositionOfAnalysis)
                                                    {
                                                        Undefined += TaxonParts[PositionOfAnalysis] + " ";
                                                        PositionOfAnalysis++;
                                                    }
                                                    Undefined = Undefined.Trim();
                                                    W.WriteElementString(_NameParts.Undefined.ToString(), Undefined);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (!this._RankFound)
                {
                    W.WriteElementString(_NameParts.Rank.ToString(), "sp.");
                    this._RankFound = true;
                }
            }
            W.WriteEndElement(); // TaxonPart
        }

        private void writeXmlTaxonomicNameAuthors(ref System.Xml.XmlWriter W, string[] TaxonParts, ref int PositionOfAnalysis, XmlExport._AuthorType AuthorType)
        {
            string Authors = "";

            if (TaxonParts.Length > PositionOfAnalysis)
            {
                if (TaxonParts[PositionOfAnalysis].Substring(0, 1).ToUpper() == TaxonParts[PositionOfAnalysis].Substring(0, 1)
                     ||
                     TaxonParts[PositionOfAnalysis].StartsWith("("))
                {
                    while (TaxonParts.Length > PositionOfAnalysis)
                    {
                        Authors += TaxonParts[PositionOfAnalysis] + " ";
                        PositionOfAnalysis++;
                        if (TaxonParts.Length == PositionOfAnalysis) break;

                        // special treatment for "f." - can be rank or part of author
                        if (TaxonParts[PositionOfAnalysis] == "f.")
                        {
                            if (TaxonParts.Length == PositionOfAnalysis + 1)
                            {
                                Authors += TaxonParts[PositionOfAnalysis] + " ";
                                PositionOfAnalysis++;
                                break;
                            }
                            if (TaxonParts.Length > PositionOfAnalysis + 1
                                &&
                                this.AuthorSeparatorList.Contains(TaxonParts[PositionOfAnalysis + 1]))
                            {
                                Authors += TaxonParts[PositionOfAnalysis] + " ";
                                PositionOfAnalysis++;
                            }
                            if (TaxonParts.Length > PositionOfAnalysis + 1
                                &&
                                TaxonParts[PositionOfAnalysis + 1] == "f.")
                            {
                                Authors += TaxonParts[PositionOfAnalysis] + " ";
                                PositionOfAnalysis++;
                            }
                        }

                        int YearForTest = 0;
                        if (TaxonParts[PositionOfAnalysis].Substring(0, 1).ToLower() == TaxonParts[PositionOfAnalysis].Substring(0, 1)
                            &&
                            !this.AuthorSeparatorList.Contains(TaxonParts[PositionOfAnalysis])
                            &&
                            !TaxonParts[PositionOfAnalysis].StartsWith("(")
                            &&
                            !TaxonParts[PositionOfAnalysis].Contains(")")
                            &&
                            !int.TryParse(TaxonParts[PositionOfAnalysis], out YearForTest))
                        {
                            break;
                        }
                        if (TaxonParts.Length > PositionOfAnalysis + 1
                            &&
                            this.IdenfificationQualifierList.Contains(TaxonParts[PositionOfAnalysis] + " " + TaxonParts[PositionOfAnalysis + 1]))
                            break;
                    }

                    // Writing the found Authors
                    if (Authors.Length > 0)
                        W.WriteElementString(AuthorType.ToString(), Authors.Trim());
                }
            }
        }

        private void writeXmlTaxonomicNameQualifier(ref System.Xml.XmlWriter W, string[] TaxonParts, ref int PositionOfAnalysis, XmlExport._IdentificationQualifierType QualifierType)
        {
            string Qualifier = "";
            if (TaxonParts.Length > PositionOfAnalysis)
            {
                if (TaxonParts.Length > PositionOfAnalysis + 1
                    &&
                    this.IdenfificationQualifierList.Contains(TaxonParts[PositionOfAnalysis] + " " + TaxonParts[PositionOfAnalysis + 1]))
                {
                    // IdenfificationQualifier has 2 parts
                    Qualifier = TaxonParts[PositionOfAnalysis] + " " + TaxonParts[PositionOfAnalysis + 1];
                    PositionOfAnalysis++;
                    PositionOfAnalysis++;
                }
                else if (this.IdenfificationQualifierList.Contains(TaxonParts[PositionOfAnalysis]))
                {
                    // IdenfificationQualifier has 1 part
                    Qualifier = TaxonParts[PositionOfAnalysis];
                    PositionOfAnalysis++;
                }

                // Writing the found Qualifier
                if (Qualifier.Length > 0)
                {
                    if (TaxonParts.Length <= PositionOfAnalysis)
                    {
                        W.WriteElementString(_IdentificationQualifierType.QualifierTerminatory.ToString(), Qualifier);
                    }
                    else
                        W.WriteElementString(QualifierType.ToString(), Qualifier);
                }
            }
        }

        private void writeXmlTaxonomicNameRank(ref System.Xml.XmlWriter W, string[] TaxonParts, ref int PositionOfAnalysis)
        {
            string Rank = "";
            //if (this._RankFound) return;
            if (TaxonParts.Length > PositionOfAnalysis)
            {
                if (TaxonParts.Length > PositionOfAnalysis + 1
                    &&
                    this.RankList.Contains(TaxonParts[PositionOfAnalysis] + " " + TaxonParts[PositionOfAnalysis + 1]))
                {
                    // IdenfificationQualifier has 2 parts
                    Rank = TaxonParts[PositionOfAnalysis] + " " + TaxonParts[PositionOfAnalysis + 1];
                    PositionOfAnalysis++;
                    PositionOfAnalysis++;
                }
                else if (this.RankList.Contains(TaxonParts[PositionOfAnalysis]))
                {
                    // IdenfificationQualifier has 1 part
                    Rank = TaxonParts[PositionOfAnalysis];
                    PositionOfAnalysis++;
                }
                else
                {
                    for (int i = 0; i < TaxonParts.Length; i++)
                    {
                        if (this.RankList.Contains(TaxonParts[i]))
                            return;
                        if (TaxonParts.Length > i + 1 && this.RankList.Contains(TaxonParts[i] + " " + TaxonParts[i + 1]))
                            return;
                    }
                    Rank = "sp.";
                }

                // Writing the found Rank
                if (Rank.Length > 0 && !this._RankFound)
                {
                    W.WriteElementString(_NameParts.Rank.ToString(), Rank);
                    this._RankFound = true;
                }
            }
        }

        private void writeXmlTaxon(ref System.Xml.XmlWriter W, string TaxonURI)
        {
            try
            {
                System.Collections.Generic.Dictionary<string, string> DictTaxon = this.TaxonName.UnitValues(TaxonURI);
                if (DictTaxon.Count > 0)
                {
                    W.WriteStartElement("DiversityTaxonNames");
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DictTaxon)
                    {
                        if (KV.Value.Length > 0)
                            W.WriteElementString(KV.Key.Replace(" ", "_").Replace("/", "-"), KV.Value);
                    }
                    W.WriteEndElement();  // DiversityTaxonNames
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public DiversityWorkbench.TaxonName TaxonName
        {
            get
            {
                if (this._TaxonName == null)
                    this._TaxonName = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                return this._TaxonName;
            }
        }

        private bool writeXmlTerm(ref System.Xml.XmlWriter W, string TermURI)
        {
            bool OK = true;
            try
            {
                System.Collections.Generic.Dictionary<string, string> DictTerm = this.Term.UnitValues(TermURI);
                if (DictTerm.Count > 0)
                {
                    W.WriteStartElement("DiversityScientificTerms");
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DictTerm)
                    {
                        if (KV.Value.Length > 0)
                        {
                            try
                            {
                                string Key = KV.Key.Replace(" ", "_").Replace("/", "-");
                                W.WriteElementString(Key, KV.Value);
                            }
                            catch (System.Exception ex)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                OK = false;
                            }
                        }
                    }
                    W.WriteEndElement();  // DiversityScientificTerms
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        public DiversityWorkbench.ScientificTerm Term
        {
            get
            {
                if (this._Term == null)
                    this._Term = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                return this._Term;
            }
        }



        #endregion

        #region Relations

        private void writeXmlRelations(ref System.Xml.XmlWriter W, int SpecimenID)
        {
            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionSpecimenRelation.Select("CollectionSpecimenID = " + SpecimenID.ToString());
            if (rr.Length > 0)
            {
                W.WriteStartElement("Relations");
                for (int i = 0; i < rr.Length; i++)
                {
                    W.WriteStartElement("Relation");
                    foreach (System.Data.DataColumn C in rr[i].Table.Columns)
                    {
                        if (!C.ColumnName.Equals(System.DBNull.Value) &&
                            C.ColumnName.ToString().Length > 0 &&
                            C.ColumnName != "CollectionSpecimenID" &&
                            C.ColumnName != "RelatedSpecimenURI" &&
                            C.ColumnName != "IsInternalRelationCache" &&
                            C.ColumnName != "RelatedSpecimenCollectionID")
                        {
                            if (!rr[i][C.ColumnName].Equals(System.DBNull.Value) && rr[i][C.ColumnName].ToString().Length > 0)
                                W.WriteElementString(C.ColumnName, rr[i][C.ColumnName].ToString());
                        }
                    }
                    W.WriteEndElement();
                }
                W.WriteEndElement();
            }
        }

        private void writeXmlRelationsInvers(ref System.Xml.XmlWriter W, int SpecimenID)
        {
            System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionSpecimenRelationInvers.Select("");
            if (rr.Length > 0)
            {
                W.WriteStartElement("Relations");
                for (int i = 0; i < rr.Length; i++)
                {
                    W.WriteStartElement("Relation");
                    foreach (System.Data.DataColumn C in rr[i].Table.Columns)
                    {
                        if (!C.ColumnName.Equals(System.DBNull.Value) &&
                            C.ColumnName.ToString().Length > 0 &&
                            C.ColumnName != "CollectionSpecimenID" &&
                            C.ColumnName != "RelatedSpecimenURI" &&
                            C.ColumnName != "IsInternalRelationCache" &&
                            C.ColumnName != "RelatedSpecimenCollectionID")
                        {
                            if (!rr[i][C.ColumnName].Equals(System.DBNull.Value) && rr[i][C.ColumnName].ToString().Length > 0)
                                W.WriteElementString(C.ColumnName, rr[i][C.ColumnName].ToString());
                        }
                    }
                    W.WriteEndElement();
                }
                W.WriteEndElement();
            }
        }

        private void writeXmlRelationInfo(ref System.Xml.XmlWriter W, System.Data.DataRow R)
        {

            DiversityCollection.Datasets.DataSetCollectionSpecimen DS = new Datasets.DataSetCollectionSpecimen();
            string CollectionSpecimenID = DiversityWorkbench.CollectionSpecimen.getIDFromURI(R["RelatedSpecimenURI"].ToString());
            string BaseURL = DiversityWorkbench.CollectionSpecimen.getBaseURIfromURI(R["RelatedSpecimenURI"].ToString());
            string WhereClause = " WHERE CollectionSpecimenID = " + CollectionSpecimenID;
            string SQL = DiversityCollection.CollectionSpecimen.SqlSpecimen + WhereClause;
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(DS.CollectionSpecimen);
            if (DS.CollectionSpecimen.Rows.Count > 0)
            {

            }
            //System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionSpecimenRelation.Select("CollectionSpecimenID = " + CollectionSpecimenID.ToString());
            //if (rr.Length > 0)
            //{
            //    W.WriteStartElement("Relations");
            //    for (int i = 0; i < rr.Length; i++)
            //    {
            //        W.WriteStartElement("Relation");
            //        foreach (System.Data.DataColumn C in rr[i].Table.Columns)
            //        {
            //            if (!C.ColumnName.Equals(System.DBNull.Value) &&
            //                C.ColumnName.ToString().Length > 0 &&
            //                C.ColumnName != "CollectionSpecimenID" &&
            //                C.ColumnName != "RelatedSpecimenURI" &&
            //                C.ColumnName != "IsInternalRelationCache" &&
            //                C.ColumnName != "RelatedSpecimenCollectionID")
            //            {
            //                if (!rr[i][C.ColumnName].Equals(System.DBNull.Value) && rr[i][C.ColumnName].ToString().Length > 0)
            //                    W.WriteElementString(C.ColumnName, rr[i][C.ColumnName].ToString());
            //            }
            //        }
            //        W.WriteEndElement();
            //    }
            //    W.WriteEndElement();
            //}
        }
        
        #endregion

        #region Images

        private void ResetImageDirectory()
        {
            string Path = this.ImageDirectory();
            if (this._ImageDirectory.Exists)
                this._ImageDirectory.Delete(true);
        }

        private string ImageDirectory()
        {
            if (this._ImageDirectory == null)
                this._ImageDirectory = new System.IO.DirectoryInfo(Folder.LabelPrinting(Folder.LabelPrintingFolder.img));
            if (!this._ImageDirectory.Exists)
                this._ImageDirectory.Create();
            return this._ImageDirectory.FullName;
        }

        private void writeXmlQRcode(ref System.Xml.XmlWriter W, int SpecimenID, int PartID)
        {
            string QRcode = "";
            try
            {
                switch (this._QRsource)
                {
                    case QRcodeSource.None:
                        return;
                    case QRcodeSource.AccessionNumber:
                        System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionSpecimen.Select("CollectionSpecimenID = " + SpecimenID.ToString());
                        if (rr.Length == 1)
                        {
                            if (!rr[0]["AccessionNumber"].Equals(System.DBNull.Value))
                                QRcode = rr[0]["AccessionNumber"].ToString();
                        }
                        break;
                    case QRcodeSource.CollectorsEventNumber:
                        System.Data.DataRow[] rrS = this.dataSetCollectionSpecimen.CollectionSpecimen.Select("CollectionSpecimenID = " + SpecimenID.ToString());
                        if (rrS.Length == 1)
                        {
                            System.Data.DataRow[] rrE = this.dataSetCollectionSpecimen.CollectionEvent.Select("CollectionEventID = " + rrS[0]["CollectionEventID"].ToString());
                            if (rrE.Length == 1)
                            {
                                if (!rrE[0]["CollectorsEventNumber"].Equals(System.DBNull.Value))
                                    QRcode = rrE[0]["CollectorsEventNumber"].ToString();
                            }
                        }
                        break;
                    case QRcodeSource.DepositorsAccessionNumber:
                        System.Data.DataRow[] rrSdan = this.dataSetCollectionSpecimen.CollectionSpecimen.Select("CollectionSpecimenID = " + SpecimenID.ToString());
                        if (rrSdan.Length == 1)
                        {
                            if (!rrSdan[0]["DepositorsAccessionNumber"].Equals(System.DBNull.Value))
                                QRcode = rrSdan[0]["DepositorsAccessionNumber"].ToString();
                        }
                        break;
                    case QRcodeSource.ExternalIdentifier:
                       string Restriction = "ReferencedID = " + SpecimenID.ToString() + " AND ReferencedTable = 'CollectionSpecimen'";
                       if (_QRtype.Length > 0)
                            Restriction += " AND Type = '" + _QRtype + "'";
                       if (this.dataSetCollectionSpecimen.ExternalIdentifier.Rows.Count == 0 && this.dataSetCollectionSpecimen.CollectionSpecimen.Count > 1)
                        {
                            string SqlExID = "SELECT TOP 1 Identifier FROM ExternalIdentifier WHERE " + Restriction;
                            QRcode = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlExID, true);
                        }
                        else
                        {
                            System.Data.DataRow[] rrEx = this.dataSetCollectionSpecimen.ExternalIdentifier.Select(Restriction);
                            if (rrEx.Length > 0)
                            {
                                if (!rrEx[0]["Identifier"].Equals(System.DBNull.Value))
                                    QRcode = rrEx[0]["Identifier"].ToString();
                            }
                        }
                        break;
                    case QRcodeSource.PartAccessionNumber:
                        System.Data.DataRow[] rrSP = this.dataSetCollectionSpecimen.CollectionSpecimenPart.Select("CollectionSpecimenID = " + SpecimenID.ToString());
                        if (rrSP.Length > 0)
                        {
                            if (!rrSP[0]["AccessionNumber"].Equals(System.DBNull.Value))
                                QRcode = rrSP[0]["AccessionNumber"].ToString();
                        }
                        break;
                    case QRcodeSource.StableIdentifier:
                        string SQL = "SELECT dbo.StableIdentifier(" + _ProjectID.ToString() + ", " + SpecimenID.ToString() + ", ";
                        if (this._QRtype == DiversityCollection.XmlExport.QRcodeStableIdentifierType.SpecimenID.ToString()
                            || this._QRtype == DiversityCollection.XmlExport.QRcodeStableIdentifierType.AccessionNumber.ToString())
                        {
                            SQL += " NULL, NULL)";
                        }
                        else
                        {
                            System.Data.DataRow[] rrU = this.dataSetCollectionSpecimen.IdentificationUnit.Select("CollectionSpecimenID = " + SpecimenID.ToString() + " AND DisplayOrder > 0", "DisplayOrder");
                            if (rrU.Length > 0)
                            {
                                string UnitID = rrU[0]["IdentificationUnitID"].ToString();
                                SQL += UnitID;
                                if (this._QRtype == "UnitInPart")
                                    SQL += ", " + PartID.ToString() + ")";
                                else
                                    SQL += ", NULL)";
                            }
                            else
                                SQL += " NULL, NULL)";
                        }
                        QRcode = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);//"SELECT dbo.StableIdentifier(909, 651925,  NULL, NULL)"
                        if (this._QRtype == DiversityCollection.XmlExport.QRcodeStableIdentifierType.AccessionNumber.ToString())
                        {
                            System.Data.DataRow[] rrA = this.dataSetCollectionSpecimen.CollectionSpecimen.Select("CollectionSpecimenID = " + SpecimenID.ToString());
                            if (rrA.Length == 1)
                            {
                                if (!rrA[0]["AccessionNumber"].Equals(System.DBNull.Value))
                                    QRcode = QRcode.Substring(0, QRcode.LastIndexOf('/') + 1) + rrA[0]["AccessionNumber"].ToString();
                            }
                        }
                        break;
                    case QRcodeSource.StorageLocation:
                        System.Data.DataRow[] rrSPL = this.dataSetCollectionSpecimen.CollectionSpecimenPart.Select("CollectionSpecimenID = " + SpecimenID.ToString());
                        if (rrSPL.Length > 0)
                        {
                            if (!rrSPL[0]["StorageLocation"].Equals(System.DBNull.Value))
                                QRcode = rrSPL[0]["StorageLocation"].ToString();
                        }
                        break;
                    case QRcodeSource.GUID:
                        SQL = "SELECT cast(RowGUID as varchar(40)) FROM CollectionSpecimen AS S WHERE CollectionSpecimenID = " + SpecimenID.ToString();
                        QRcode = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        break;
                }
                if (QRcode.Length > 0)
                {
                    W.WriteStartElement("QRcode");
                    W.WriteElementString("QRcode", QRcode);
                    string ImagePath = DiversityWorkbench.QRCode.QRCodeImage(QRcode, DiversityWorkbench.Settings.QRcodeSize, this.ImageDirectory(), PartID.ToString());
                    W.WriteElementString("ImagePath", ImagePath);
                    W.WriteEndElement();  // QRcode
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        #endregion

        #region Agents
        
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
                    KV.Key == "Address" ||
                    KV.Key == "PersonName" ||
                    KV.Key == "AgentName" ||
                    KV.Key == "SuperiorAgents")
                {
                    if (KV.Key == "Country")
                        W.WriteElementString(KV.Key, KV.Value.ToUpper());
                    else if (KV.Key == "SuperiorAgents")
                    {
                        W.WriteStartElement("SuperiorAgents"); // SuperiorAgents
                        string SA = KV.Value;
                        int i = 0;
                        while (i > -1 && SA.Length > 0)
                        {
                            string S = "";
                            if (i == 0)
                                S = SA.Substring(i, SA.IndexOf("\r\n", i) - i);
                            else
                            {
                                int e = SA.IndexOf("\r\n", i + 1);
                                int l = e - i;
                                if (l > 0)
                                    S = SA.Substring(i + 2, l - 2);
                            }
                            if (S.Length > 0)
                            {
                                W.WriteStartElement("SuperiorAgent");
                                W.WriteElementString("AgentName", S);
                                W.WriteEndElement();
                            }
                            i = SA.IndexOf("\r\n", i + 1);
                        }
                        W.WriteEndElement(); // SuperiorAgents
                    }
                    else
                        W.WriteElementString(KV.Key, KV.Value);
                }
            }
        }

        private void writeXmlAddress(ref System.Xml.XmlWriter W, string AgentURI, bool IncludeAddress)
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
                    (KV.Key == "Address" && IncludeAddress) ||
                    KV.Key == "PersonName" ||
                    KV.Key == "AgentName" ||
                    KV.Key == "SuperiorAgents")
                {
                    if (KV.Key == "Country")
                        W.WriteElementString(KV.Key, KV.Value.ToUpper());
                    else if (KV.Key == "SuperiorAgents")
                    {
                        W.WriteStartElement("SuperiorAgents"); // SuperiorAgents
                        string SA = KV.Value;
                        int i = 0;
                        while (i > -1 && SA.Length > 0)
                        {
                            string S = "";
                            if (i == 0)
                                S = SA.Substring(i, SA.IndexOf("\r\n", i) - i);
                            else
                            {
                                int e = SA.IndexOf("\r\n", i + 1);
                                int l = e - i;
                                if (l > 0)
                                    S = SA.Substring(i + 2, l - 2);
                            }
                            if (S.Length > 0)
                            {
                                W.WriteStartElement("SuperiorAgent");
                                W.WriteElementString("AgentName", S);
                                W.WriteEndElement();
                            }
                            i = SA.IndexOf("\r\n", i + 1);
                        }
                        W.WriteEndElement(); // SuperiorAgents
                    }
                    else
                        W.WriteElementString(KV.Key, KV.Value);
                }
            }
        }

        public DiversityWorkbench.Agent Agent
        {
            get
            {
                if (this._Agent == null)
                    this._Agent = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                return _Agent;
            }
        }
        
        #endregion

        public DiversityWorkbench.Gazetteer Gazetteer
        {
            get
            {
                if (this._Gazetteer == null)
                    this._Gazetteer = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                return _Gazetteer;
            }
        }
        
        #endregion

        #region XML Export for Collection

        #region Parameters for collection printing
        
        //private System.IO.FileInfo _XslFileCollection;
        //private System.IO.FileInfo _XmlFileCollection;
        private DiversityCollection.Datasets.DataSetCollection _DataSetCollection;
        private System.IO.DirectoryInfo _ImageDirectoryCollection;
        public enum QRcodeSourceCollection { None, StableIdentifier, CollectionName }
        private QRcodeSourceCollection _QRsourceCollection;
        
        #endregion

        public string CreateXmlForCollection(
            DiversityCollection.Datasets.DataSetCollection DataSetCollection, 
            int ProjectID, 
            string Title, 
            QRcodeSourceCollection SourceForQRcode,
            string TypeOfQRcode,
            int Duplicates = 1,
            bool IncludeContent = false,
            CollectionContentSorting sorting = CollectionContentSorting.StorageLocation)
        {
            try
            {
                this._DataSetCollection = DataSetCollection;
                this._ProjectID = ProjectID;
                this._Duplicates = Duplicates;
                System.Xml.XmlWriter W;
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                settings.Encoding = System.Text.Encoding.UTF8;
                W = System.Xml.XmlWriter.Create(this._XmlFile.FullName, settings);
                W.WriteStartDocument();
                W.WriteStartElement("LabelPrint");
                W.WriteStartElement("Report");
                W.WriteElementString("Title", Title);
                string ProjectTitle = DiversityCollection.LookupTable.ProjectTitle(this._ProjectID);
                W.WriteElementString("ProjectTitle", ProjectTitle);
                string ProjectParent = DiversityCollection.LookupTable.ProjectParentTitle(this._ProjectID);
                if (ProjectParent != ProjectTitle)
                    W.WriteElementString("ProjectParentTitle", ProjectParent);
                W.WriteElementString("User", DiversityCollection.LookupTable.CurrentUser);
                string Date = System.DateTime.Now.Year.ToString() + "/" + System.DateTime.Now.Month.ToString() + "/" + System.DateTime.Now.Day.ToString();
                W.WriteElementString("Date", Date);
                W.WriteEndElement(); // Report

                foreach (System.Data.DataRow R in DataSetCollection.Collection.Rows)
                {
                    int CollectionID;
                    if (int.TryParse(R["CollectionID"].ToString(), out CollectionID))
                    {
                        for (int i = 0; i < this._Duplicates; i++)
                        {
                            W.WriteStartElement("Label");
                            this.writeXmlCollection(ref W, CollectionID, SourceForQRcode, TypeOfQRcode, IncludeContent, sorting);
                            W.WriteEndElement(); // Label
                        }
                    }
                }
                W.WriteEndDocument();
                W.Flush();
                W.Close();
                if (this._XslFile != null && this._XslFile.Exists)
                {
                    // #65
                    System.Xml.Xsl.XslCompiledTransform XSLT = new System.Xml.Xsl.XslCompiledTransform();
                    System.Xml.Xsl.XsltSettings XsltSettings = new System.Xml.Xsl.XsltSettings(true, true);
                    System.Xml.XmlResolver resolver = new System.Xml.XmlUrlResolver();
                    XSLT.Load(this._XslFile.FullName);

                    // Load the file to transform.
                    System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(this._XmlFile.FullName);

                    // The output file:
                    string OutputFile = this._XmlFile.FullName.Substring(0, this._XmlFile.FullName.Length
                        - this._XmlFile.Extension.Length) + ".htm";

                    // Create the writer.             
                    System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(OutputFile, XSLT.OutputSettings);

                    // Transform the file and send the output to the console.
                    XSLT.Transform(doc, writer);
                    writer.Close();
                    return OutputFile;
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
            return this._XmlFile.FullName;
        }

        private void writeXmlCollection(ref System.Xml.XmlWriter W, int CollectionID, QRcodeSourceCollection SourceForQRcode, string TypeOfQRcode, bool IncludeContent = false, CollectionContentSorting sorting = CollectionContentSorting.StorageLocation)
        {
            try
            {
                string SQL = "SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
                    "Location, LocationPlan, LocationPlanWidth, LocationHeight, LocationGeometry.ToString() AS LocationGeometry, CollectionOwner, DisplayOrder " +
                    "FROM dbo.CollectionHierarchySuperior(" + CollectionID.ToString() + ")";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                System.Data.DataTable Dt = new System.Data.DataTable();
                ad.Fill(Dt);
                System.Data.DataRow[] rr = Dt.Select("CollectionID = " + CollectionID.ToString());
                foreach (System.Data.DataColumn C in rr[0].Table.Columns)
                {
                    if (!C.ColumnName.Equals(System.DBNull.Value) &&
                        C.ColumnName.ToString().Length > 0 &&
                        !C.ColumnName.StartsWith("Log") &&
                        C.ColumnName != "RowGUID")
                    {
                        if (!rr[0][C.ColumnName].Equals(System.DBNull.Value) && rr[0][C.ColumnName].ToString().Length > 0)
                            W.WriteElementString(C.ColumnName, rr[0][C.ColumnName].ToString());
                    }
                }

                W.WriteStartElement("TopCollection");
                string AdministrativeContact = "";
                System.Data.DataRow[] rrTop = Dt.Select("CollectionParentID IS NULL");
                foreach (System.Data.DataColumn C in rrTop[0].Table.Columns)
                {
                    if (!C.ColumnName.Equals(System.DBNull.Value) &&
                            C.ColumnName.ToString().Length > 0 &&
                        !C.ColumnName.StartsWith("Log") &&
                        C.ColumnName != "RowGUID")
                    {
                        if (!rrTop[0][C.ColumnName].Equals(System.DBNull.Value) && rrTop[0][C.ColumnName].ToString().Length > 0)
                        {
                            W.WriteElementString(C.ColumnName, rrTop[0][C.ColumnName].ToString());
                            if (C.ColumnName == "AdministrativeContactAgentURI")
                            {
                                AdministrativeContact = rrTop[0][C.ColumnName].ToString();
                            }
                        }
                    }
                }
                if (AdministrativeContact.Length > 0)
                {
                    W.WriteStartElement("AdministrativeContact");
                    this.writeXmlAddress(ref W, AdministrativeContact, false);
                    W.WriteEndElement(); // AdministrativeContact
                }
                W.WriteEndElement(); // TopCollection
                if (SourceForQRcode != QRcodeSourceCollection.None)
                    this.writeXmlQRcodeCollection(ref W, CollectionID, SourceForQRcode, TypeOfQRcode);
                if (IncludeContent)
                    this.writeXmlCollectionContent(ref W, CollectionID, sorting);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private string ImageDirectoryCollection()
        {
            if (this._ImageDirectoryCollection == null)
                this._ImageDirectoryCollection = new System.IO.DirectoryInfo(Folder.LabelPrinting(Folder.LabelPrintingFolder.CollectionImg));
            if (!this._ImageDirectoryCollection.Exists)
                this._ImageDirectoryCollection.Create();
            return this._ImageDirectoryCollection.FullName;
        }

        private void ResetImageDirectoryCollection()
        {
            string Path = this.ImageDirectoryCollection();
            if (this._ImageDirectoryCollection.Exists)
                this._ImageDirectoryCollection.Delete(true);
        }

        private void writeXmlQRcodeCollection(ref System.Xml.XmlWriter W, int CollectionID, QRcodeSourceCollection SourceForQRcode,
            string TypeOfQRcode)
        {
            string QRcode = "";
            try
            {
                switch (SourceForQRcode)
                {
                    case QRcodeSourceCollection.None:
                        return;
                    case QRcodeSourceCollection.CollectionName:
                        System.Data.DataRow[] rr = this._DataSetCollection.Collection.Select("CollectionID = " + CollectionID.ToString());
                        if (rr.Length == 1)
                        {
                            if (!rr[0]["CollectionName"].Equals(System.DBNull.Value))
                                QRcode = rr[0]["CollectionName"].ToString();
                        }
                        break;
                    case QRcodeSourceCollection.StableIdentifier:
                        string SQL = "";
                        if (_ProjectID == -1)
                            SQL = "SELECT dbo.StableIdentifierBase() + 'Collection/' + CAST(" + CollectionID.ToString() + " AS varchar)";
                        else 
                            SQL = "SELECT StableIdentifierBase + 'Collection/' + CAST(" + CollectionID.ToString() + " AS varchar) FROM ProjectProxy WHERE ProjectID = " + _ProjectID.ToString();
                        QRcode = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);//"SELECT dbo.StableIdentifier(909, 651925,  NULL, NULL)"
                        break;
                }
                if (QRcode.Length > 0)
                {
                    W.WriteStartElement("QRcode");
                    W.WriteElementString("QRcode", QRcode);
                    string ImagePath = DiversityWorkbench.QRCode.QRCodeImage(QRcode, DiversityWorkbench.Settings.QRcodeSize, this.ImageDirectoryCollection(), CollectionID.ToString());
                    W.WriteElementString("ImagePath", ImagePath);
                    W.WriteEndElement();  // QRcode
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void writeXmlCollectionContent(ref System.Xml.XmlWriter W, int CollectionID, CollectionContentSorting sorting = CollectionContentSorting.StorageLocation)
        {
            try
            {
                W.WriteStartElement("Content");
                string SQL = "SELECT P.StorageLocation, SUBSTRING(P.StorageLocation, 1, 1) AS ContainerRow, SUBSTRING(P.StorageLocation, 2, 255) AS ContainerColumn, S.AccessionNumber, U.LastIdentificationCache, E.CollectorsEventNumber " +
                    "FROM CollectionSpecimenPart AS P INNER JOIN " +
                    "CollectionSpecimen AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID INNER JOIN " +
                    "IdentificationUnit AS U ON S.CollectionSpecimenID = U.CollectionSpecimenID LEFT OUTER JOIN " +
                    "CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID " +
                    "WHERE P.CollectionID = " + CollectionID.ToString();
                switch(sorting)
                {
                    case CollectionContentSorting.AccessionNumber:
                        SQL += " ORDER BY S.AccessionNumber";
                        break;
                    case CollectionContentSorting.Taxon:
                        SQL += " ORDER BY U.LastIdentificationCache";
                        break;
                    default:
                        SQL += " ORDER BY P.StorageLocation";
                        break;
                }
                System.Data.DataTable dtT = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtT);
                this.writeXmlCollectionContent(ref W, dtT);
                W.WriteEndElement();  // Content
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void writeXmlCollectionContent(ref System.Xml.XmlWriter W, System.Data.DataTable dataTable)
        {
            foreach(System.Data.DataRow R in dataTable.Rows)
            {
                W.WriteStartElement("Item");
                foreach(System.Data.DataColumn C in dataTable.Columns)
                {
                    W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                }
                W.WriteEndElement();  // Item
            }
        }

        #endregion

        #region XML Export for CollectionTask

        #region Parameters for collection task printing

        private DiversityCollection.Datasets.DataSetCollectionTask _DataSetCollectionTask;
        private System.IO.DirectoryInfo _ImageDirectoryCollectionTask;
        public enum QRcodeSourceCollectionTask { None, StableIdentifier, GUID }
        private QRcodeSourceCollectionTask _QRsourceCollectionTask;

        public enum CollectionTaskSourceForChart { None, Location, Room }
        private CollectionTaskSourceForChart _CollectionTaskTypeForChart;

        #endregion

        #region Exhibitions

        /// <summary>
        /// For Exhibitions
        /// </summary>
        /// <param name="CollectionIDs">List of collections for the report</param>
        /// <param name="Title">Optional title</param>
        /// <param name="SourceForQRcode">A source for the creation of QR-Codes</param>
        /// <param name="IncludeFloorPlan">If the floor plans should be included</param>
        /// <returns></returns>
        public string CreateXmlForCollectionTask(
            Exhibition exhibition,
            //System.Collections.Generic.List<int> CollectionIDs,
            int? CollectionID,
            string Title = "Exhibition",
            QRcodeSourceCollectionTask SourceForQRcode = QRcodeSourceCollectionTask.None,
            bool IncludeFloorPlan = false)
        {
            try
            {
                System.Collections.Generic.List<int> CollectionIDs = new List<int>();
                if (CollectionID == null)
                {
                    foreach(System.Data.DataRow R in exhibition.Collections().Rows)
                    {
                        int i;
                        if (int.TryParse(R["CollectionID"].ToString(), out i))
                            CollectionIDs.Add(i);
                    }
                }
                else
                {
                    CollectionIDs.Add((int)CollectionID);
                }
                System.Xml.XmlWriter W;
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                settings.Encoding = System.Text.Encoding.UTF8;
                W = System.Xml.XmlWriter.Create(this._XmlFile.FullName, settings);
                W.WriteStartDocument();
                W.WriteStartElement("CollectionTasks");
                W.WriteStartElement("Report");
                if (Title.Length > 0)
                    W.WriteElementString("Title", Title);
                W.WriteElementString("User", DiversityCollection.LookupTable.CurrentUser);
                string Date = System.DateTime.Now.Year.ToString() + "/" + System.DateTime.Now.Month.ToString() + "/" + System.DateTime.Now.Day.ToString();
                W.WriteElementString("Date", Date);
                W.WriteEndElement(); // Report

                foreach (int ID in CollectionIDs)
                {
                    this.writeXmlCollectionForTask(ref W, exhibition, ID, SourceForQRcode, IncludeFloorPlan);
                }
                W.WriteEndDocument();
                W.Flush();
                W.Close();
                if (this._XslFile != null && this._XslFile.Exists)
                {
                    // #65
                    System.Xml.Xsl.XslCompiledTransform XSLT = new System.Xml.Xsl.XslCompiledTransform();
                    System.Xml.Xsl.XsltSettings XsltSettings = new System.Xml.Xsl.XsltSettings(true, true);
                    System.Xml.XmlResolver resolver = new System.Xml.XmlUrlResolver();
                    XSLT.Load(this._XslFile.FullName);

                    // Load the file to transform.
                    System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(this._XmlFile.FullName);

                    // The output file:
                    string OutputFile = this._XmlFile.FullName.Substring(0, this._XmlFile.FullName.Length
                        - this._XmlFile.Extension.Length) + ".htm";

                    // Create the writer.             
                    System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(OutputFile, XSLT.OutputSettings);

                    // Transform the file and send the output to the console.
                    XSLT.Transform(doc, writer);
                    writer.Close();
                    return OutputFile;
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
            return this._XmlFile.FullName;
        }


        /// <summary>
        /// For exhibitions
        /// </summary>
        /// <param name="W"></param>
        /// <param name="CollectionID">The ID of the collection</param>
        /// <param name="SourceForQRcode"></param>
        /// <param name="IncludeFloorPlan"></param>
        private void writeXmlCollectionForTask(
            ref System.Xml.XmlWriter W, 
            Exhibition exhibition,
            int CollectionID, QRcodeSourceCollectionTask SourceForQRcode,
            bool IncludeFloorPlan = false)
        {
            // Getting the collection
            string FloorPlan = "NULL AS LocationPlan, NULL AS LocationPlanWidth, ";
            if (IncludeFloorPlan) FloorPlan = "LocationPlan, LocationPlanWidth, ";
            string SQL = "SELECT CollectionID, NULL AS CollectionParentID, CollectionName, CollectionAcronym, Description, " + FloorPlan + "DisplayOrder, Type " +
                "FROM dbo.Collection C WHERE C.CollectionID = " + CollectionID.ToString();
            System.Data.DataTable dt = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            W.WriteStartElement("Collections");
            foreach (System.Data.DataRow R in dt.Rows)
            {
                W.WriteStartElement("Collection");
                foreach (System.Data.DataColumn C in dt.Columns)
                {
                    if (!R[C.ColumnName].Equals(System.DBNull.Value) && R[C.ColumnName].ToString().Length > 0)
                    {
                        W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                    }
                }
                W.WriteStartElement("Parts");
                foreach(System.Data.DataRow P in exhibition.Parts(CollectionID).Rows)
                {
                    W.WriteStartElement("Part");
                    foreach (System.Data.DataColumn c in P.Table.Columns)
                    {
                        if (!P[c.ColumnName].Equals(System.DBNull.Value) && P[c.ColumnName].ToString().Length > 0)
                        {
                            W.WriteElementString(c.ColumnName, P[c.ColumnName].ToString());
                        }
                    }
                    W.WriteEndElement(); // Part
                }
                W.WriteEndElement(); // Parts
                //string Type = R["Type"].ToString().ToLower();
                //int ID;
                //if (int.TryParse(R["CollectionID"].ToString(), out ID))
                //{
                //    switch (Type)
                //    {
                //        case "trap":
                //        case "sensor":
                //            break;
                //        default:
                //            SQL = "SELECT CollectionTaskID FROM dbo.CollectionTask C WHERE C.CollectionTaskParentID IS NULL AND C.DisplayOrder > 0 AND C.CollectionID = " + ID.ToString();
                //            System.Data.DataTable dtTask = new System.Data.DataTable();
                //            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTask);
                //            if (dtTask.Rows.Count > 0)
                //            {
                //                W.WriteStartElement("CollectionTasks");
                //                foreach (System.Data.DataRow row in dtTask.Rows)
                //                {
                //                    int id;
                //                    if (int.TryParse(row[0].ToString(), out id))
                //                    {
                //                        this.writeXmlCollectionTask(ref W, id, SourceForQRcode, true);
                //                    }
                //                }
                //                W.WriteEndElement();  //  CollectionTasks
                //            }
                //            //this.writeXmlCollectionForTask(ref W, CollectionID, SourceForQRcode, ChartSource, IncludeFloorPlan, ID, ChartWidth, ChartHeight);
                //            break;
                //    }
                //}
                W.WriteEndElement();  //  Collection
            }
            W.WriteEndElement();  //  Collections
        }

        #endregion

        public string CreateXmlForCollectionTask(
            int CollectionTaskID,
            string Title,
            QRcodeSourceCollectionTask SourceForQRcode,
            int Duplicates = 1)
        {
            try
            {
                this._Duplicates = Duplicates;
                System.Xml.XmlWriter W;
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                settings.Encoding = System.Text.Encoding.UTF8;
                W = System.Xml.XmlWriter.Create(this._XmlFile.FullName, settings);
                W.WriteStartDocument();
                W.WriteStartElement("CollectionTasks");
                W.WriteStartElement("Report");
                if (Title.Length > 0)
                    W.WriteElementString("Title", Title);
                //string ProjectTitle = DiversityCollection.LookupTable.ProjectTitle(this._ProjectID);
                //W.WriteElementString("ProjectTitle", ProjectTitle);
                //string ProjectParent = DiversityCollection.LookupTable.ProjectParentTitle(this._ProjectID);
                //if (ProjectParent != ProjectTitle)
                //    W.WriteElementString("ProjectParentTitle", ProjectParent);
                W.WriteElementString("User", DiversityCollection.LookupTable.CurrentUser);
                string Date = System.DateTime.Now.Year.ToString() + "/" + System.DateTime.Now.Month.ToString() + "/" + System.DateTime.Now.Day.ToString();
                W.WriteElementString("Date", Date);
                W.WriteEndElement(); // Report

                for (int i = 0; i < this._Duplicates; i++)
                {
                    W.WriteStartElement("Task");
                    this.writeXmlCollectionTask(ref W, CollectionTaskID, SourceForQRcode);
                    W.WriteEndElement(); // Task
                }
                W.WriteEndDocument();
                W.Flush();
                W.Close();
                if (this._XslFile != null && this._XslFile.Exists)
                {
                    // #65
                    System.Xml.Xsl.XslCompiledTransform XSLT = new System.Xml.Xsl.XslCompiledTransform();
                    System.Xml.Xsl.XsltSettings XsltSettings = new System.Xml.Xsl.XsltSettings(true, true);
                    System.Xml.XmlResolver resolver = new System.Xml.XmlUrlResolver();
                    XSLT.Load(this._XslFile.FullName);

                    // Load the file to transform.
                    System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(this._XmlFile.FullName);

                    // The output file:
                    string OutputFile = this._XmlFile.FullName.Substring(0, this._XmlFile.FullName.Length
                        - this._XmlFile.Extension.Length) + ".htm";

                    // Create the writer.             
                    System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(OutputFile, XSLT.OutputSettings);

                    // Transform the file and send the output to the console.
                    XSLT.Transform(doc, writer);
                    writer.Close();
                    return OutputFile;
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
            return this._XmlFile.FullName;
        }


        public string CreateXmlForCollectionTask(
            int ID,
            bool ForCollection = false,
            string Title = "",
            QRcodeSourceCollectionTask SourceForQRcode = QRcodeSourceCollectionTask.None,
            int Duplicates = 1,
            CollectionTaskSourceForChart ChartSource = CollectionTaskSourceForChart.None,
            bool IncludeFloorPlan = false,
            int ChartHeight = 400,
            int ChartWidth = 800)
        {
            try
            {
                this._Duplicates = Duplicates;
                System.Xml.XmlWriter W;
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                settings.Encoding = System.Text.Encoding.UTF8;
                W = System.Xml.XmlWriter.Create(this._XmlFile.FullName, settings);
                W.WriteStartDocument();
                W.WriteStartElement("CollectionTasks");
                W.WriteStartElement("Report");
                if (Title.Length > 0)
                    W.WriteElementString("Title", Title);
                //string ProjectTitle = DiversityCollection.LookupTable.ProjectTitle(this._ProjectID);
                //W.WriteElementString("ProjectTitle", ProjectTitle);
                //string ProjectParent = DiversityCollection.LookupTable.ProjectParentTitle(this._ProjectID);
                //if (ProjectParent != ProjectTitle)
                //    W.WriteElementString("ProjectParentTitle", ProjectParent);
                W.WriteElementString("User", DiversityCollection.LookupTable.CurrentUser);
                string Date = System.DateTime.Now.Year.ToString() + "/" + System.DateTime.Now.Month.ToString() + "/" + System.DateTime.Now.Day.ToString();
                W.WriteElementString("Date", Date);
                W.WriteEndElement(); // Report

                for (int i = 0; i < this._Duplicates; i++)
                {
                    //if (ForCollection)
                    //    W.WriteStartElement("Collections");
                    //else
                    //    W.WriteStartElement("Task");                    
                    if (!ForCollection)
                        W.WriteStartElement("Task");

                    if (ForCollection)
                        this.writeXmlCollectionForTask(ref W, ID, SourceForQRcode, ChartSource, IncludeFloorPlan, null, ChartWidth, ChartHeight);
                    else
                        this.writeXmlCollectionTask(ref W, ID, SourceForQRcode);
                    if (!ForCollection)
                        W.WriteEndElement(); // Task
                }
                W.WriteEndDocument();
                W.Flush();
                W.Close();
                if (this._XslFile != null && this._XslFile.Exists)
                {
                    // #65
                    System.Xml.Xsl.XslCompiledTransform XSLT = new System.Xml.Xsl.XslCompiledTransform();
                    System.Xml.Xsl.XsltSettings XsltSettings = new System.Xml.Xsl.XsltSettings(true, true);
                    System.Xml.XmlResolver resolver = new System.Xml.XmlUrlResolver();
                    XSLT.Load(this._XslFile.FullName);

                    // Load the file to transform.
                    System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(this._XmlFile.FullName);

                    // The output file:
                    string OutputFile = this._XmlFile.FullName.Substring(0, this._XmlFile.FullName.Length
                        - this._XmlFile.Extension.Length) + ".htm";

                    // Create the writer.             
                    System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(OutputFile, XSLT.OutputSettings);

                    // Transform the file and send the output to the console.
                    XSLT.Transform(doc, writer);
                    writer.Close();
                    return OutputFile;
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
            return this._XmlFile.FullName;
        }
        //private void writeXmlCollectionTask(ref System.Xml.XmlWriter W, int CollectionTaskID, QRcodeSourceCollectionTask SourceForQRcode)
        //{
        //    string SQL = "SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, Location, CollectionOwner, DisplayOrder " +
        //        "FROM dbo.CollectionHierarchySuperior(" + CollectionTaskID.ToString() + ")";
        //    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //    System.Data.DataTable Dt = new System.Data.DataTable();
        //    ad.Fill(Dt);
        //    System.Data.DataRow[] rr = Dt.Select("CollectionID = " + CollectionTaskID.ToString());
        //    foreach (System.Data.DataColumn C in rr[0].Table.Columns)
        //    {
        //        if (!C.ColumnName.Equals(System.DBNull.Value) &&
        //            C.ColumnName.ToString().Length > 0 &&
        //            !C.ColumnName.StartsWith("Log") &&
        //            C.ColumnName != "RowGUID")
        //        {
        //            if (!rr[0][C.ColumnName].Equals(System.DBNull.Value) && rr[0][C.ColumnName].ToString().Length > 0)
        //                W.WriteElementString(C.ColumnName, rr[0][C.ColumnName].ToString());
        //        }
        //    }

        //    W.WriteStartElement("TopCollection");
        //    string AdministrativeContact = "";
        //    System.Data.DataRow[] rrTop = Dt.Select("CollectionTaskParentID IS NULL");
        //    foreach (System.Data.DataColumn C in rrTop[0].Table.Columns)
        //    {
        //        if (!C.ColumnName.Equals(System.DBNull.Value) &&
        //                C.ColumnName.ToString().Length > 0 &&
        //            !C.ColumnName.StartsWith("Log") &&
        //            C.ColumnName != "RowGUID")
        //        {
        //            if (!rrTop[0][C.ColumnName].Equals(System.DBNull.Value) && rrTop[0][C.ColumnName].ToString().Length > 0)
        //            {
        //                W.WriteElementString(C.ColumnName, rrTop[0][C.ColumnName].ToString());
        //                if (C.ColumnName == "AdministrativeContactAgentURI")
        //                {
        //                    AdministrativeContact = rrTop[0][C.ColumnName].ToString();
        //                }
        //            }
        //        }
        //    }
        //    if (AdministrativeContact.Length > 0)
        //    {
        //        W.WriteStartElement("AdministrativeContact");
        //        this.writeXmlAddress(ref W, AdministrativeContact, false);
        //        W.WriteEndElement(); // AdministrativeContact
        //    }
        //    W.WriteEndElement(); // TopCollection
        //    if (SourceForQRcode != QRcodeSourceCollectionTask.None)
        //        this.writeXmlQRcodeCollectionTask(ref W, CollectionTaskID, SourceForQRcode);
        //}

        private void writeXmlCollectionTask(ref System.Xml.XmlWriter W, int CollectionTaskID, QRcodeSourceCollectionTask SourceForQRcode, bool ForCollection = false)
        {
            // getting the date type
            string SQL = "SELECT T.DateType " +
                "FROM CollectionTask AS CT " +
                "INNER JOIN Task AS T ON CT.TaskID = T.TaskID " +
                "WHERE CT.DisplayOrder > 0 AND CT.CollectionTaskID = " + CollectionTaskID.ToString();
            string DateType = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            Task.TaskDateType taskDateType = Task.TypeOfTaskDate(DateType);
            string SqlDate = "";
            switch (taskDateType)
            {
                case Task.TaskDateType.Date:
                    SqlDate = "CONVERT(varchar(10), CT.TaskStart, 120) AS TaskStart, ";
                    break;
                case Task.TaskDateType.DateAndTime:
                    SqlDate = "CONVERT(varchar(20), CT.TaskStart, 120) AS TaskStart, ";
                    break;
                case Task.TaskDateType.DateAndTimeFromTo:
                    SqlDate = "CONVERT(varchar(20), CT.TaskStart, 120) AS TaskStart, CONVERT(varchar(20), CT.TaskEnd, 120) AS TaskEnd, ";
                    break;
                case Task.TaskDateType.DateFromTo:
                    SqlDate = "CONVERT(varchar(10), CT.TaskStart, 120) AS TaskStart, CONVERT(varchar(10), CT.TaskEnd, 120) AS TaskEnd, ";
                    break;
                case Task.TaskDateType.Time:
                    SqlDate = "SUBSTRING(CONVERT(varchar(20), CT.TaskStart, 120), 12, 8) AS TaskStart, ";
                    break;
                case Task.TaskDateType.TimeFromTo:
                    SqlDate = "SUBSTRING(CONVERT(varchar(20), CT.TaskStart, 120), 12, 8) AS TaskStart, SUBSTRING(CONVERT(varchar(20), CT.TaskEnd, 120), 12, 8) AS TaskEnd, ";
                    break;
            }
            // getting the data
            System.Data.DataTable dataTable = new System.Data.DataTable();
            if (ForCollection)
            {
                SQL = "SELECT T.DisplayText AS Task, T.ModuleTitle, CT.DisplayText, CT.SpecimenPartID, CT.ModuleUri, " + SqlDate +
                    "T.ResultType, CT.Result, T.UriType, CT.URI, T.NumberType, CT.NumberValue, T.BoolType, CT.BoolValue,T.DescriptionType,  CT.Description, T.NotesType, CT.Notes, " +
                    "T.Type AS TaskType, T.Description AS TaskDescription, T.Notes AS TaskNotes, T.TaskURI, CT.ResponsibleAgent " +
                    "FROM CollectionTask AS CT " +
                    "INNER JOIN Task AS T ON CT.TaskID = T.TaskID " +
                    "WHERE CT.DisplayOrder > 0 AND CT.CollectionTaskID = " + CollectionTaskID.ToString() +
                    " ORDER BY CT.DisplayOrder";
            }
            else
                SQL = "SELECT T.DisplayText AS Task, T.ModuleTitle, CT.DisplayText, CT.SpecimenPartID, CT.ModuleUri, " + SqlDate + 
                    "T.ResultType, CT.Result, T.UriType, CT.URI, T.NumberType, CT.NumberValue, T.BoolType, CT.BoolValue,T.DescriptionType,  CT.Description, T.NotesType, CT.Notes, " +
                    "C.CollectionID, C.CollectionName, C.CollectionAcronym, P.CollectionName AS ParentCollection, P.CollectionAcronym AS ParentCollectionAcronym, " +
                    "T.Type AS TaskType, T.Description AS TaskDescription, T.Notes AS TaskNotes, T.TaskURI, CT.ResponsibleAgent " +
                    "FROM CollectionTask AS CT " +
                    "INNER JOIN Task AS T ON CT.TaskID = T.TaskID " +
                    "INNER JOIN Collection AS C ON CT.CollectionID = C.CollectionID " +
                    "LEFT OUTER JOIN Collection AS P ON P.CollectionID = C.CollectionParentID " +
                    "WHERE CT.DisplayOrder > 0 AND CT.CollectionTaskID = " + CollectionTaskID.ToString() +
                    " ORDER BY CT.DisplayOrder";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTable);
            if (dataTable.Rows.Count > 0)
            {
                W.WriteStartElement("CollectionTask");
                foreach (System.Data.DataColumn C in dataTable.Rows[0].Table.Columns)
                {
                    if (!dataTable.Rows[0][C.ColumnName].Equals(System.DBNull.Value) && dataTable.Rows[0][C.ColumnName].ToString().Length > 0)
                    {
                        W.WriteElementString(C.ColumnName, dataTable.Rows[0][C.ColumnName].ToString());
                        switch(C.ColumnName)
                        {
                            case "CollectionID":
                                if (!ForCollection)
                                {
                                    W.WriteStartElement("Collection");
                                    this.writeXmlCollection(ref W, int.Parse(dataTable.Rows[0][C.ColumnName].ToString()), QRcodeSourceCollection.None, "");
                                    W.WriteEndElement();  // Collection
                                }
                                break;
                            case "SpecimenPartID":
                                break;
                        }
                    }
                }
                this.writeXmlCollectionTaskImage(ref W, CollectionTaskID);
                if (ForCollection)
                    W.WriteEndElement();  //  CollectionTask
                //if (!ForCollection)
                {
                    SQL = "SELECT C.CollectionTaskID FROM CollectionTask C ";
                    if (ForCollection)
                        SQL += " INNER JOIN Task T ON C.TaskID = T.TaskID AND T.Type NOT IN ('Trap', 'Pest') AND ";
                    else
                        SQL += " WHERE ";
                    SQL += " C.CollectionTaskParentID = " + CollectionTaskID.ToString() + " AND C.DisplayOrder > 0 ORDER BY C.DisplayOrder";
                    System.Data.DataTable dataTableDependent = new System.Data.DataTable();
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTableDependent);
                    if (dataTableDependent.Rows.Count > 0)
                    {
                        if (!ForCollection)
                            W.WriteStartElement("CollectionTasks");
                        foreach (System.Data.DataRow R in dataTableDependent.Rows)
                            this.writeXmlCollectionTask(ref W, int.Parse(R[0].ToString()), SourceForQRcode, ForCollection);
                        if (!ForCollection)
                            W.WriteEndElement();  //  CollectionTasks
                    }
                }
                if (!ForCollection)
                    W.WriteEndElement();  //  CollectionTask
                if (SourceForQRcode != QRcodeSourceCollectionTask.None)
                    this.writeXmlQRcodeCollectionTask(ref W, CollectionTaskID, SourceForQRcode);
            }
        }




        private void writeXmlCollectionForTask(ref System.Xml.XmlWriter W, int CollectionID, QRcodeSourceCollectionTask SourceForQRcode,
            CollectionTaskSourceForChart ChartSource = CollectionTaskSourceForChart.None,
            bool IncludeFloorPlan = false,
            int? CollectionParentID = null,
            int ChartWidth = 800,
            int ChartHeight = 400)
        {
            // Getting the top collection and children
            string FloorPlan = "NULL AS LocationPlan, NULL AS LocationPlanWidth, ";
            if (IncludeFloorPlan) FloorPlan = "LocationPlan, LocationPlanWidth, ";
            string SQL = "SELECT CollectionID, NULL AS CollectionParentID, CollectionName, CollectionAcronym, Description, " + FloorPlan + "DisplayOrder, Type " +
                "FROM dbo.Collection C WHERE ";
            if (CollectionParentID != null)
                SQL += " C.CollectionParentID = " + CollectionParentID.ToString();
            else
                SQL += " C.CollectionID = " + CollectionID.ToString();
            SQL += " AND (DisplayOrder IS NULL OR DisplayOrder > 0) ORDER BY DisplayOrder, CollectionName";
            //string SQL = "SELECT CollectionID, NULL AS CollectionParentID, CollectionName, CollectionAcronym, Description, " + FloorPlan + "DisplayOrder, Type " +
            //    "FROM dbo.Collection C WHERE C.CollectionID = " + CollectionID.ToString() + " " +
            //    "UNION " +
            //    "SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, Description, " + FloorPlan + "DisplayOrder, Type " +
            //    "FROM dbo.CollectionChildNodes(" + CollectionID.ToString() + ") AS C " +
            //    "ORDER BY CollectionParentID, DisplayOrder, CollectionName";
            //string SQL = "SELECT CollectionID, NULL AS CollectionParentID, DisplayOrder " +
            //    "FROM dbo.Collection C WHERE C.CollectionID = " + CollectionID.ToString() + " " +
            //    "UNION " +
            //    "SELECT CollectionID, CollectionParentID, DisplayOrder " +
            //    "FROM dbo.CollectionChildNodes(" + CollectionID.ToString() + ") AS C " +
            //    "ORDER BY CollectionParentID, DisplayOrder";
            System.Data.DataTable dt = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            W.WriteStartElement("Collections");
            foreach (System.Data.DataRow R in dt.Rows)
            {
                W.WriteStartElement("Collection");
                foreach (System.Data.DataColumn C in dt.Columns)
                {
                    if (!R[C.ColumnName].Equals(System.DBNull.Value) && R[C.ColumnName].ToString().Length > 0)
                    {
                        W.WriteElementString(C.ColumnName, R[C.ColumnName].ToString());
                    }
                }
                string Type = R["Type"].ToString().ToLower();
                int ID;
                if (int.TryParse(R["CollectionID"].ToString(), out ID))
                {
                    switch(Type)
                    {
                        case "trap":
                        case "sensor":
                            break;
                        case "room":
                        case "location":
                            if (Type == ChartSource.ToString().ToLower())
                            {
                                Tasks.IPM iPM = new Tasks.IPM();
                                if (iPM.ChartCreate(ID, true, ChartWidth, ChartHeight))
                                {
                                    string Path = iPM.ChartSaveImage();
                                    W.WriteElementString("Chart", Path);
                                }
                            }
                            else if (ChartSource != CollectionTaskSourceForChart.None)
                                goto default;
                            break;
                        default:
                            SQL = "SELECT CollectionTaskID FROM dbo.CollectionTask C WHERE C.CollectionTaskParentID IS NULL AND C.DisplayOrder > 0 AND C.CollectionID = " + ID.ToString();
                            System.Data.DataTable dtTask = new System.Data.DataTable();
                            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTask);
                            if (dtTask.Rows.Count > 0)
                            {
                                W.WriteStartElement("CollectionTasks");
                                foreach (System.Data.DataRow row in dtTask.Rows)
                                {
                                    int id;
                                    if (int.TryParse(row[0].ToString(), out id))
                                    {
                                        this.writeXmlCollectionTask(ref W, id, SourceForQRcode, true);
                                    }
                                }
                                W.WriteEndElement();  //  CollectionTasks
                            }
                            if (ChartSource != CollectionTaskSourceForChart.None && R["Type"].ToString() == ChartSource.ToString())
                            {

                            }
                            this.writeXmlCollectionForTask(ref W, CollectionID, SourceForQRcode, ChartSource, IncludeFloorPlan, ID, ChartWidth, ChartHeight);
                            break;
                    }
                }
                W.WriteEndElement();  //  Collection
            }
            W.WriteEndElement();  //  Collections
        }

        private void writeXmlCollectionTaskImage(ref System.Xml.XmlWriter W, int CollectionTaskID)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
            string SQL = "SELECT URI, ImageType, Notes, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, " +
                    "LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear " +
                    "FROM CollectionTaskImage WHERE (DataWithholdingReason IS NULL OR DataWithholdingReason = '') " +
                    "AND CollectionTaskID = " + CollectionTaskID.ToString();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTable);
            if (dataTable.Rows.Count > 0)
            {
                W.WriteStartElement("Images");
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {

                    W.WriteStartElement("Image");
                    foreach (System.Data.DataColumn C in dataTable.Rows[i].Table.Columns)
                    {
                        if (!dataTable.Rows[i][C.ColumnName].Equals(System.DBNull.Value) && dataTable.Rows[i][C.ColumnName].ToString().Length > 0)
                        {
                            W.WriteElementString(C.ColumnName, dataTable.Rows[i][C.ColumnName].ToString());
                        }
                    }
                    W.WriteEndElement();  //  Image
                }
                W.WriteEndElement();  //  Images
            }
        }


        //private void writeXmlCollectionFloorPlan(ref System.Xml.XmlWriter W, int CollectionID)
        //{
        //    System.Data.DataTable dataTable = new System.Data.DataTable();
        //    string SQL = "SELECT LocationPlan " +
        //            "FROM Collection WHERE CollectionID = " + CollectionID.ToString();
        //    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTable);
        //    if (dataTable.Rows.Count > 0)
        //    {
        //        W.WriteStartElement("Images");
        //        for (int i = 0; i < dataTable.Rows.Count; i++)
        //        {

        //            W.WriteStartElement("Image");
        //            foreach (System.Data.DataColumn C in dataTable.Rows[i].Table.Columns)
        //            {
        //                if (!dataTable.Rows[i][C.ColumnName].Equals(System.DBNull.Value) && dataTable.Rows[i][C.ColumnName].ToString().Length > 0)
        //                {
        //                    W.WriteElementString(C.ColumnName, dataTable.Rows[i][C.ColumnName].ToString());
        //                }
        //            }
        //            W.WriteEndElement();  //  Image
        //        }
        //        W.WriteEndElement();  //  Images
        //    }
        //}

        private void writeXmlCollectionChart(ref System.Xml.XmlWriter W, int CollectionID)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
            string SQL = "SELECT URI, ImageType, Notes, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, " +
                    "LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear " +
                    "FROM CollectionTaskImage WHERE (DataWithholdingReason IS NULL OR DataWithholdingReason = '') " +
                    "AND CollectionTaskID = " + CollectionID.ToString();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTable);
            if (dataTable.Rows.Count > 0)
            {
                W.WriteStartElement("Images");
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {

                    W.WriteStartElement("Image");
                    foreach (System.Data.DataColumn C in dataTable.Rows[i].Table.Columns)
                    {
                        if (!dataTable.Rows[i][C.ColumnName].Equals(System.DBNull.Value) && dataTable.Rows[i][C.ColumnName].ToString().Length > 0)
                        {
                            W.WriteElementString(C.ColumnName, dataTable.Rows[i][C.ColumnName].ToString());
                        }
                    }
                    W.WriteEndElement();  //  Image
                }
                W.WriteEndElement();  //  Images
            }
        }


        private string ImageDirectoryCollectionTask()
        {
            if (this._ImageDirectoryCollectionTask == null)
                this._ImageDirectoryCollectionTask = new System.IO.DirectoryInfo(Folder.Report(Folder.ReportFolder.TaskImg));
            if (!this._ImageDirectoryCollectionTask.Exists)
                this._ImageDirectoryCollectionTask.Create();
            return this._ImageDirectoryCollectionTask.FullName;
        }

        private void ResetImageDirectoryCollectionTask()
        {
            string Path = this.ImageDirectoryCollectionTask();
            if (this._ImageDirectoryCollectionTask.Exists)
                this._ImageDirectoryCollectionTask.Delete(true);
        }

        private void writeXmlQRcodeCollectionTask(ref System.Xml.XmlWriter W, int CollectionTaskID, QRcodeSourceCollectionTask SourceForQRcode)
        {
            string QRcode = "";
            try
            {
                switch (SourceForQRcode)
                {
                    case QRcodeSourceCollectionTask.None:
                        return;
                    case QRcodeSourceCollectionTask.StableIdentifier:
                        string SQL = "SELECT dbo.StableIdentifierBase() + 'CollectionTask/' + CAST(" + CollectionTaskID.ToString() + " AS varchar)";
                        QRcode = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);//"SELECT dbo.StableIdentifier(909, 651925,  NULL, NULL)"
                        break;
                    case QRcodeSourceCollectionTask.GUID:
                        SQL = "SELECT cast(RowGUID as varchar(40)) FROM CollectionTask AS S WHERE CollectionTaskID = " + CollectionTaskID.ToString();
                        QRcode = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        break;
                }
                if (QRcode.Length > 0)
                {
                    W.WriteStartElement("QRcode");
                    W.WriteElementString("QRcode", QRcode);
                    string ImagePath = DiversityWorkbench.QRCode.QRCodeImage(QRcode, DiversityWorkbench.Settings.QRcodeSize, this.ImageDirectoryCollectionTask(), CollectionTaskID.ToString());
                    W.WriteElementString("ImagePath", ImagePath);
                    W.WriteEndElement();  // QRcode
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Event series
        private System.Collections.Generic.List<System.Data.DataRow> EventSeriesList(int SeriesID)
        {
            System.Collections.Generic.List<System.Data.DataRow> Series = new List<System.Data.DataRow>();
            System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesID = " + SeriesID.ToString());
            if (rr.Length > 0)
            {
                Series.Add(rr[0]);
                Series = this.EventSeries(ref Series);
            }
            return Series;
        }

        private System.Collections.Generic.List<System.Data.DataRow> EventSeries(ref System.Collections.Generic.List<System.Data.DataRow> EventSeriesList)
        {
            System.Data.DataRow r = EventSeriesList[EventSeriesList.Count - 1];
            if (!r["SeriesParentID"].Equals(System.DBNull.Value))
            {
                System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesID = " + r["SeriesParentID"].ToString());
                if (rr.Length > 0)
                {
                    EventSeriesList.Add(rr[0]);
                    EventSeriesList = this.EventSeries(ref EventSeriesList);
                }
            }
            return EventSeriesList;
        }
        
        #endregion        

        #region Properties

        private string AccessionNumber(int SpecimenID, int? PartID)
        {
            string AccNr = "";
            if (PartID != null)
            {
                System.Data.DataRow[] RR = this.dataSetCollectionSpecimen.CollectionSpecimenPart.Select("CollectionSpecimenID = " + SpecimenID + " AND SpecimenPartID = " + PartID.ToString());
                if (RR.Length > 0 && !RR[0]["AccessionNumber"].Equals(System.DBNull.Value))
                    AccNr = RR[0]["AccessionNumber"].ToString();
            }
            if (AccNr.Length == 0)
            {
                System.Data.DataRow[] RR = this.dataSetCollectionSpecimen.CollectionSpecimen.Select("CollectionSpecimenID = " + SpecimenID);
                if (RR.Length > 0)
                    AccNr = RR[0]["AccessionNumber"].ToString();
            }
            return AccNr;
        }

        private System.Collections.Generic.List<string> IdenfificationQualifierList
        {
            get 
            {
                if (this._IdenfificationQualifierList == null)
                {
                    this._IdenfificationQualifierList = new List<string>();
                    try
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        string SQL = "SELECT RTRIM(Code) AS Code FROM CollIdentificationQualifier_Enum " +
                            "WHERE (DisplayEnable = 1) AND (LEN(Code) > 0)";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (System.Data.DataRow R in dt.Rows)
                                this._IdenfificationQualifierList.Add(R[0].ToString());
                        }
                        if (!this._IdenfificationQualifierList.Contains("aff."))
                            this._IdenfificationQualifierList.Add("aff.");
                        if (!this._IdenfificationQualifierList.Contains("cf."))
                            this._IdenfificationQualifierList.Add("cf.");
                        if (!this._IdenfificationQualifierList.Contains("nec."))
                            this._IdenfificationQualifierList.Add("nec.");
                        if (!this._IdenfificationQualifierList.Contains("nom. nov."))
                            this._IdenfificationQualifierList.Add("nom. nov.");
                        if (!this._IdenfificationQualifierList.Contains("nomen nudum"))
                            this._IdenfificationQualifierList.Add("nomen nudum");
                        if (!this._IdenfificationQualifierList.Contains("nom. illeg."))
                            this._IdenfificationQualifierList.Add("nom. illeg.");
                    }
                    catch { }
                }
                return _IdenfificationQualifierList; 
            }
        }

        private System.Collections.Generic.List<string> RankList
        {
            get
            {
                if (this._RankList == null)
                {
                    this._RankList = new List<string>();
                    try
                    {
                        this._RankList.Add("f. sp.");
                        this._RankList.Add("subsubfm.");
                        this._RankList.Add("subfm.");
                        this._RankList.Add("f.");
                        this._RankList.Add("subvar.");
                        this._RankList.Add("var.");
                        this._RankList.Add("pathovar.");
                        this._RankList.Add("biovar.");
                        this._RankList.Add("cult.");
                        this._RankList.Add("convar.");
                        this._RankList.Add("cultivar. group");
                        this._RankList.Add("graft-chimaera");
                        this._RankList.Add("infrasp.");
                        this._RankList.Add("subsp.");
                        this._RankList.Add("ssp.");
                        this._RankList.Add("sp.");
                        this._RankList.Add("sp. group");
                        this._RankList.Add("aggr.");
                        this._RankList.Add("subser.");
                        this._RankList.Add("ser.");
                        this._RankList.Add("subsect.");
                        this._RankList.Add("sect.");
                        this._RankList.Add("infragen.");
                        this._RankList.Add("subgen.");
                        this._RankList.Add("gen.");
                        this._RankList.Add("infratrib.");
                        this._RankList.Add("subtrib.");
                        this._RankList.Add("trib.");
                    }
                    catch { }
                }
                return _RankList;
            }
        }

        private System.Collections.Generic.List<string> AuthorSeparatorList
        {
            get
            {
                if (this._AuthorSeparatorList == null)
                {
                    this._AuthorSeparatorList = new List<string>();
                    try
                    {
                        this._AuthorSeparatorList.Add("&");
                        this._AuthorSeparatorList.Add("ex.");
                        this._AuthorSeparatorList.Add("ex");
                        this._AuthorSeparatorList.Add("de.");
                        this._AuthorSeparatorList.Add("al.");
                        this._AuthorSeparatorList.Add(":");
                        this._AuthorSeparatorList.Add("de");
                        this._AuthorSeparatorList.Add("van");
                        this._AuthorSeparatorList.Add("der");
                        this._AuthorSeparatorList.Add("den");
                        this._AuthorSeparatorList.Add("syn.");
                        this._AuthorSeparatorList.Add("illeg.");
                        this._AuthorSeparatorList.Add("non");
                        this._AuthorSeparatorList.Add("et");
                        this._AuthorSeparatorList.Add("ad.");
                        this._AuthorSeparatorList.Add("int.");
                        this._AuthorSeparatorList.Add("n.");
                        this._AuthorSeparatorList.Add("non");
                        this._AuthorSeparatorList.Add("non sensu");
                        this._AuthorSeparatorList.Add("s.");
                        this._AuthorSeparatorList.Add("non s.");
                        this._AuthorSeparatorList.Add("sensu");
                        this._AuthorSeparatorList.Add("s. auct.");
                        this._AuthorSeparatorList.Add("sensu non");
                        this._AuthorSeparatorList.Add("s. non");
                        //this._AuthorSeparatorList.Add("f.");
                    }
                    catch { }
                }
                return _AuthorSeparatorList;
            }
        }
        #endregion

        #region ALT

        #region Expedition
        private System.Collections.Generic.List<System.Data.DataRow> ExpeditionList(int ExpeditionID)
        {
            System.Collections.Generic.List<System.Data.DataRow> Expeditions = new List<System.Data.DataRow>();
            System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("ExpeditionID = " + ExpeditionID.ToString());
            if (rr.Length > 0)
            {
                Expeditions.Add(rr[0]);
                Expeditions = this.EventExpeditions(ref Expeditions);
            }
            return Expeditions;
        }

        private System.Collections.Generic.List<System.Data.DataRow> EventExpeditions(ref System.Collections.Generic.List<System.Data.DataRow> ExpeditionList)
        {
            System.Data.DataRow r = ExpeditionList[ExpeditionList.Count - 1];
            if (!r["ExpeditionParentID"].Equals(System.DBNull.Value))
            {
                System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("ExpeditionID = " + r["ExpeditionParentID"].ToString());
                if (rr.Length > 0)
                {
                    ExpeditionList.Add(rr[0]);
                    ExpeditionList = this.EventExpeditions(ref ExpeditionList);
                }
            }
            return ExpeditionList;
        }

        #endregion

        #endregion

        #region Lookup tables
        //private System.Data.DataTable DtCollection
        //{
        //    get
        //    {
        //        try
        //        {
        //            if (this._dtCollection == null)
        //            {
        //                this._dtCollection = new System.Data.DataTable("Collection");
        //                string SQL = "SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, " +
        //                    "AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
        //                    "Location, DisplayOrder " +
        //                    "FROM Collection " +
        //                    "ORDER BY DisplayOrder";
        //                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //                ad.Fill(_dtCollection);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        }
        //        return this._dtCollection;
        //    }
        //}

        //private System.Data.DataTable DtLocalisationSystem
        //{
        //    get
        //    {
        //        if (this._dtLocalisationSystem == null)
        //        {
        //            this._dtLocalisationSystem = new System.Data.DataTable("LocalisationSystem");
        //            DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterLocalisationSystem, this._dtLocalisationSystem, this.SqlLocalisationSystem, DiversityWorkbench.Settings.ConnectionString);
        //        }
        //        return _dtLocalisationSystem;
        //    }
        //}

        //private System.Data.DataTable DtAnalysis
        //{
        //    get
        //    {
        //        try
        //        {
        //            if (this._dtAnalysis == null)
        //            {
        //                this._dtAnalysis = new System.Data.DataTable("Analysis");
        //                string SQL = "SELECT AnalysisID, AnalysisParentID, DisplayText " +
        //                    "FROM Analysis ";
        //                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //                ad.Fill(_dtAnalysis);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        }
        //        return this._dtAnalysis;
        //    }
        //}

        //private System.Data.DataTable DtProcessing
        //{
        //    get
        //    {
        //        try
        //        {
        //            if (this._dtProcessing == null)
        //            {
        //                this._dtProcessing = new System.Data.DataTable("Processing");
        //                string SQL = "SELECT ProcessingID, ProcessingParentID, DisplayText " +
        //                    "FROM Processing ";
        //                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //                ad.Fill(_dtProcessing);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        }
        //        return this._dtProcessing;
        //    }
        //}

        #endregion

        #region XSLT

        public static void writeDefaultXslt(string Path)
        {
            System.Xml.XmlWriter W;
            try
            {
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                settings.Encoding = System.Text.Encoding.UTF8;
                W = System.Xml.XmlWriter.Create(Path, settings);
                W.WriteRaw(XsltHeader);
                W.WriteRaw(XsltFonts);
                W.WriteRaw(XsltOptions);
                W.WriteRaw(XsltSpecimen);
                W.WriteRaw(XsltCollectors);
                W.WriteRaw(XsltEvent);
                W.WriteRaw(XsltUnit);
                W.WriteRaw(XsltType);
                W.WriteRaw(XsltTaxon);
                W.WriteRaw(XsltReport);
                W.WriteRaw(XsltEnd);
                W.Close();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private static string XsltHeader
        {
            get
            {
                return "<?xml version=\"1.0\" encoding=\"utf-16\" ?> " +
                "<xsl:stylesheet version=\"1.0\"  " +
                  "xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\"> " +
                  "<xsl:output method=\"xml\" encoding=\"utf-16\"/> ";
            }
        }
        private static string XsltFonts
        {
            get
            {
                return "<!--Fonts--> " +
                  "<xsl:variable name=\"FontDefault\">  font-size: 10pt; font-family: Arial</xsl:variable> " +
                  "<xsl:variable name=\"FontSmall\">    font-size:  8pt; font-family: Arial</xsl:variable> " +
                  "<xsl:variable name=\"FontTitle\">    font-size: 10pt; font-family: Arial</xsl:variable> " +
                  "<xsl:variable name=\"FontTaxonName\">font-size: 12pt; font-family: Arial</xsl:variable> " +
                  "<xsl:variable name=\"FontType\">     font-size: 12pt; font-family: Arial; text-transform:uppercase</xsl:variable> " +
                  "<xsl:variable name=\"FontBarcode\">  font-size: 16pt; font-family: Code 39</xsl:variable> " +
                  "<xsl:variable name=\"FontCountry\">  font-size:  8pt; font-family: Arial; text-transform:uppercase</xsl:variable> ";
            }
        }
        private static string XsltOptions
        {
            get
            {
                return "<!--Printing options--> " +
                  "<xsl:variable name=\"ReportHeader\">Header of report</xsl:variable> " +
                  "<xsl:variable name=\"PrintReportHeader\">0</xsl:variable> " +
                  "<xsl:variable name=\"PrintReportTitle\">1</xsl:variable> ";
            }
        }
        private static string XsltSpecimen
        {
            get
            {
                return "<xsl:template match=\"/CollectionSpecimens\"> " +
                    "<html> " +
                      "<body> " +
                        "<xsl:value-of select=\"/CollectionSpecimens/CollectionSpecimen/Storages/Storage/CollectionOwner\"/> " +
                        "<xsl:if test=\"$PrintReportHeader = 1\"> " +
                          "<hr/> " +
                          "<span style=\"{$FontTitle}\"> " +
                            "<xsl:value-of select=\"$ReportHeader\"/> " +
                          "</span> " +
                        "</xsl:if> " +
                        "<xsl:apply-templates select=\"CollectionSpecimen\"/> " +
                      "</body> " +
                    "</html> " +
                  "</xsl:template> " +

                  "<xsl:template match=\"CollectionSpecimen\"> " +
                    "<hr/> " +
                    "<table cellspacing=\"1\" cellpadding=\"1\" width=\"400\" border=\"0\" style=\"{$FontDefault}\"> " +
                      "<tr height =\"0\"> " +
                        "<th width=\"200\" ></th> " +
                        "<th width=\"200\"></th> " +
                      "</tr> " +
                      "<tr> " +
                        "<td align =\"left\"> " +
                          "<xsl:value-of select=\"./Storages/Storage/Collection\"/> " +
                        "</td> " +
                        "<td align =\"right\"> " +
                          "Nr.  <xsl:value-of select=\"./AccessionNumber\"/> " +
                        "</td> " +
                      "</tr> " +
                      "<tr height =\"40\" style=\"{$FontTaxonName}\"> " +
                        "<td colspan =\"2\"> " +
                          "<b> " +
                            "<xsl:for-each select=\"./Units/MainUnit/Identifications/Identification\"> " +
                              "<xsl:if test=\"position()=1\"> " +
                                "<xsl:call-template name=\"TaxonomicName\"/> " +
                              "</xsl:if> " +
                            "</xsl:for-each> " +
                         " </b> " +
                        "</td> " +
                      "</tr> " +
                      "<xsl:call-template name=\"Type\"/> " +
                      "<tr style=\"{$FontSmall}\"> " +
                        "<td colspan =\"2\"> " +
                         " <xsl:call-template name=\"Event\"/> " +
                          "<xsl:call-template name=\"Altitude\"/> " +
                        "</td> " +
                      "</tr> " +
                      "<tr style=\"{$FontSmall}\"> " +
                        "<td colspan =\"2\"> " +
                          "<xsl:call-template name=\"UnitHierarchy\"/> " +
                        "</td> " +
                      "</tr> " +
                      "<tr style=\"{$FontSmall}\"> " +
                        "<td colspan =\"1\" align =\"left\" valign =\"top\"> " +
                          "<xsl:call-template name=\"CollectionDate\"/> " +
                        "</td> " +
                        "<td colspan =\"1\" align =\"right\"> " +
                          "<xsl:apply-templates select =\"Collectors\"/> " +
                        "</td> " +
                      "</tr> " +
                      "<xsl:if test=\"./Units/MainUnit/Identifications/Identification/ResponsibleName != ''\"> " +
                        "<tr style=\"{$FontSmall}\"> " +
                          "<td > " +
                          "</td> " +
                          "<td colspan =\"1\" align =\"right\"> " +
                            "det.: <xsl:value-of select=\"./Units/MainUnit/Identifications/Identification/ResponsibleName\"/> " +
                          "</td> " +
                        "</tr> " +
                      "</xsl:if> " +
                      "<tr height =\"60\" style=\"{$FontBarcode}\"> " +
                        "<td colspan =\"2\" align=\"center\" valign =\"bottom\"> " +
                          "*<xsl:value-of select=\"./AccessionNumber\"/>* " +
                        "</td> " +
                      "</tr> " +
                      "<tr height =\"20\" style=\"{$FontDefault}\"> " +
                        "<td colspan =\"2\" align=\"center\" valign =\"top\"> " +
                          "<xsl:value-of select=\"./AccessionNumber\"/> " +
                        "</td> " +
                      "</tr> " +
                    "</table> " +
                    "<hr/> " +
                  "</xsl:template> ";
            }
        }
        private static string XsltCollectors
        {
            get
            {
                return "<xsl:template match=\"Collectors\"> " +
                    "leg.: <xsl:apply-templates select =\"Collector\"/> " +
                  "</xsl:template> " +

                  "<xsl:template match=\"Collector\"> " +
                    "<xsl:value-of select=\"./CollectorsName\"/> " +
                    "<xsl:apply-templates select =\"CollectorsNumber\"/> " +
                    "<xsl:if test=\"position()!= last()\"> , " +

                    "</xsl:if> " +
                  "</xsl:template> " +

                  "<xsl:template match=\"CollectorsNumber\"> " +
                    "(<xsl:value-of select=\".\"/>) " +
                  "</xsl:template> ";
            }
        }
        private static string XsltEvent
        {
            get
            {
                return "<xsl:template name=\"Event\"> " +
                    "<span style= \"{$FontCountry}\"> " +
                      "<xsl:value-of select=\"./CollectionEvent/CountryCache\"/>. " +
                    "</span> " +
                    "<xsl:value-of select=\"./CollectionEvent/LocalityDescription\"/>. " +
                    "<xsl:value-of select=\"./CollectionEvent/HabitatDescription\"/>. " +
                  "</xsl:template> " +

                  "<xsl:template name=\"CollectionDate\"> " +
                    "<xsl:if test=\"./CollectionEvent/CollectionYear != '' or ./CollectionEvent/CollectionMonth != '' or ./CollectionEvent/CollectionDay != ''\"> " +
                      "<xsl:value-of select=\"./CollectionEvent/CollectionYear\"/>/ " +
                      "<xsl:value-of select=\"./CollectionEvent/CollectionMonth\"/>/ " +
                      "<xsl:value-of select=\"./CollectionEvent/CollectionDay\"/> " +
                    "</xsl:if> " +
                  "</xsl:template> " +

                  "<xsl:template name=\"IdentificationDate\"> " +
                    "<xsl:if test=\"./Units/Unit/Identifications/Identification/IdentificationYear != '' or ./Units/Unit/Identifications/Identification/IdentificationMonth != '' or ./Units/Unit/Identifications/Identification/IdentificationDay != ''\"> " +
                      "<xsl:value-of select=\"./Units/Unit/Identifications/Identification/IdentificationYear\"/>/ " +
                      "<xsl:value-of select=\"./Units/Unit/Identifications/Identification/IdentificationMonth\"/>/ " +
                      "<xsl:value-of select=\"./Units/Unit/Identifications/Identification/IdentificationDay\"/> " +
                    "</xsl:if> " +
                  "</xsl:template> " +

                  "<xsl:template name=\"Altitude\"> " +
                    "<xsl:if test=\"./Geography/Altitude/from != ''\"> " +
                      "Alt. <xsl:value-of select=\"./Geography/Altitude/from\"/> " +
                    "</xsl:if> " +
                    "<xsl:if test=\"./Geography/Altitude/to != ''\"> " +
                      "- <xsl:value-of select=\"./Geography/Altitude/to\"/> " +
                    "</xsl:if> " +
                  "</xsl:template> ";
            }
        }
        private static string XsltUnit
        {
            get
            {
                return "<xsl:template name=\"UnitHierarchy\"> " +
                    "<xsl:call-template name=\"SubstrateUnit\"/> " +
                    "<xsl:call-template name=\"GrowingOnUnits\"/> " +
                    "<xsl:call-template name=\"AssociatedUnits\"/> " +
                    "<xsl:call-template name=\"Units\"/> " +
                  "</xsl:template> " +

                  "<xsl:template name=\"SubstrateUnit\"> " +
                    "<xsl:for-each select=\"./Units/SubstrateUnit\"> " +
                      "<xsl:if test=\"position() = 1\"> " +
                        "<xsl:if test=\"./RelationType != ''\"> " +
                          "<xsl:value-of select=\"./RelationType\"/> " +
                          "<xsl:text> </xsl:text> " +
                        "</xsl:if> " +
                        "<xsl:if test=\"./RelationType = '' or count(./RelationType) = 0\"> " +
                          "<xsl:text> On </xsl:text> " +
                        "</xsl:if> " +
                        "<xsl:if test=\"./ColonisedSubstratePart != ''\"> " +
                          "<xsl:value-of select=\"./ColonisedSubstratePart\"/> of " +
                        "</xsl:if> " +
                        "<xsl:if test=\"./ColonisedSubstratePart = '' and ./RelationType = ''\"> " +
                          "<xsl:text> On </xsl:text> " +
                        "</xsl:if> " +
                        "<xsl:call-template name=\"Unit\"/> " +
                        "<xsl:text>. </xsl:text> " +
                      "</xsl:if> " +
                    "</xsl:for-each> " +
                  "</xsl:template> " +

                  "<xsl:template name=\"GrowingOnUnits\"> " +
                    "<xsl:for-each select=\"./Units/GrowingOnUnit\"> " +
                      "<xsl:if test=\"position() = 1\"> " +
                        "<xsl:text>With </xsl:text> " +
                      "</xsl:if> " +
                      "<xsl:if test=\"position() > 1\"> " +
                        "<xsl:text> and </xsl:text> " +
                      "</xsl:if> " +
                      "<xsl:call-template name=\"Unit\"/> " +
                      "<xsl:if test=\"./RelationType != ''\"> " +
                        "<xsl:value-of select=\"./RelationType\"/> " +
                        "<xsl:text> </xsl:text> " +
                      "</xsl:if> " +
                      "<xsl:if test=\"./ColonisedSubstratePart != ''\"> " +
                        "<xsl:value-of select=\"./ColonisedSubstratePart\"/> of " +
                      "</xsl:if> " +
                      "<xsl:if test=\"position() = last()\"> " +
                        "<xsl:text>. </xsl:text> " +
                      "</xsl:if> " +
                    "</xsl:for-each> " +
                  "</xsl:template> " +

                  "<xsl:template name=\"AssociatedUnits\"> " +
                    "<xsl:for-each select=\"./Units/AssociatedUnit\"> " +
                      "<xsl:if test=\"position() = 1\"> " +
                        "<xsl:text>Associated with </xsl:text> " +
                      "</xsl:if> " +
                      "<xsl:if test=\"position() > 1\"> " +
                        "<xsl:text> and </xsl:text> " +
                      "</xsl:if> " +
                      "<xsl:call-template name=\"Unit\"/> " +
                      "<xsl:if test=\"./RelationType != ''\"> " +
                        "<xsl:value-of select=\"./RelationType\"/> " +
                        "<xsl:text> </xsl:text> " +
                      "</xsl:if> " +
                      "<xsl:if test=\"./ColonisedSubstratePart != ''\"> " +
                        "<xsl:value-of select=\"./ColonisedSubstratePart\"/> " +
                      "</xsl:if> " +
                      "<xsl:if test=\"position() = last()\"> " +
                        "<xsl:text>. </xsl:text> " +
                      "</xsl:if> " +
                    "</xsl:for-each> " +
                  "</xsl:template> " +

                  "<xsl:template name=\"Units\"> " +
                    "<xsl:for-each select=\"./Units/Unit\"> " +
                      "<xsl:if test=\"position() = 1\"> Further <xsl:value-of select=\"./TaxonomicGroup\"/>s: " +
                      "</xsl:if> " +
                      "<xsl:if test=\"position() > 1\"> " +
                        "<xsl:text> and </xsl:text> " +
                      "</xsl:if> " +
                      "<xsl:call-template name=\"Unit\"/> " +
                      "<xsl:if test=\"./Substrate != ''\"> " +
                        "<xsl:if test=\"./RelationType != ''\"> " +
                          "<xsl:value-of select=\"./RelationType\"/> " +
                          "<xsl:text> </xsl:text> " +
                        "</xsl:if> " +
                        "<xsl:if test=\"./ColonisedSubstratePart != ''\"> " +
                          "<xsl:value-of select=\"./ColonisedSubstratePart\"/> of <xsl:value-of select=\"./Substrate\"/> " +
                        "</xsl:if> " +
                        "<xsl:if test=\"position() = last()\"> " +
                          "<xsl:text>. </xsl:text> " +
                        "</xsl:if> " +
                      "</xsl:if> " +
                    "</xsl:for-each> " +
                  "</xsl:template> " +

                  "<xsl:template name=\"Unit\"> " +
                    "<xsl:for-each select=\"./Identifications/Identification\"> " +
                      "<xsl:if test=\"position() = 1\"> " +
                        "<xsl:if test=\"count(./Taxon) > 0\"> " +
                          "<xsl:call-template name=\"TaxonomicName\"/> " +
                        "</xsl:if> " +
                        "<xsl:if test=\"count(./Taxon) = 0\"> " +
                          "<xsl:if test=\"./TaxonomicName != ''\"> " +
                            "<xsl:value-of select=\"./TaxonomicName\"></xsl:value-of> " +
                          "</xsl:if> " +
                          "<xsl:if test=\"./TaxonomicName = '' and ./VernacularTerm != ''\"> " +
                            "<xsl:value-of select=\"./VernacularTerm\"></xsl:value-of> " +
                          "</xsl:if> " +
                        "</xsl:if> " +
                      "</xsl:if> " +
                    "</xsl:for-each> " +
                  "</xsl:template> ";
            }
        }
        private static string XsltType
        {
            get
            {
                return "<xsl:template name=\"Type\"> " +
                    "<xsl:if test=\"./Units/MainUnit/Identifications/Identification/TypeStatus != ''\"> " +
                      "<tr height =\"40\" valign =\"middle\" style=\"{$FontType}\"> " +
                        "<td colspan =\"2\" align =\"center\"> " +
                          "<b> " +
                          "  <xsl:value-of select=\"./Units/MainUnit/Identifications/Identification/TypeStatus\"/> " +
                          "</b> " +
                       " </td> " +
                      "</tr> " +
                    "</xsl:if> " +
                 " </xsl:template> ";
            }
        }
        private static string XsltTaxon
        {
            get
            {
                return " <xsl:template name=\"TaxonomicName\"> " +
                   " <i> " +
                     " <xsl:value-of select=\"concat(./Taxon/Genus, ' ' , ./Taxon/SpeciesEpithet)\"/> " +
                    "</i> " +
                    "<xsl:if test=\"./Taxon/Rank!='sp.'\"> " +
                    "  <xsl:if test=\"./Taxon/SpeciesEpithet  " +
                      "  = ./Taxon/IntraSpecificEpithet\"> " +
                       " <xsl:value-of select=\"concat(' ' , ./Taxon/Authors)\"/> " +
                     " </xsl:if> " +
                     " <xsl:value-of select=\"concat(' ' ,./Taxon/Rank)\"/> " +
                     " <i> " +
                      "  <xsl:value-of select=\"concat(' ' , ./Taxon/IntraSpecificEpithet)\"/> " +
                     " </i> " +
                     " <xsl:if test=\"./Taxon/SpeciesEpithet  " +
                      "  != ./Taxon/IntraSpecificEpithet\"> " +
                       " <xsl:value-of select=\"concat(' ' , ./Taxon/Authors)\"/> " +
                     " </xsl:if> " +
                   " </xsl:if> " +
                   " <xsl:if test=\"./Taxon/Rank='sp.'\"> " +
                   "   <xsl:value-of select=\"concat(' ' , ./Taxon/Authors)\"/> " +
                    "</xsl:if> " +
                 " </xsl:template> " +

                  "<xsl:template name=\"TaxonomicName2Lines\"> " +
                   " <xsl:choose> " +
                    "  <xsl:when test=\"./Taxon/Rank!='sp.'\"> " +
                     "   <tr height =\"40\"> " +
                      "    <td colspan =\"2\" valign=\"bottom\"> " +
                       "     <span style=\"{$FontTaxonName}\"> " +
                        "      <xsl:value-of select=\"concat(./Taxon/Genus, ' ' , ./Taxon/SpeciesEpithet)\"/> " +
                         "   </span> " +
                          "</td> " +
                    "    </tr> " +
                     "   <tr height =\"40\"> " +
                      "    <td> " +
                       "   </td> " +
                        "  <td valign=\"top\"> " +
                          "  <span style=\"{$FontTaxonName}\"> " +
                         "     <xsl:value-of select=\"concat(./Taxon/Rank, ' ' , ./Taxon/IntraSpecificEpithet, ' ' , ./Taxon/Authors)\"/> " +
                        "    </span> " +
                       "   </td> " +
                      "  </tr> " +
                   "   </xsl:when> " +
                    "  <xsl:otherwise> " +
                     "   <tr height =\"60\"> " +
                       "   <td colspan =\"2\"> " +
                      "      <span style=\"font-size: 14pt; font-family: Verdana\"> " +
                     "         <xsl:value-of select=\"./TaxonomicName\"/> " +
                    "        </span> " +
                   "       </td> " +
                  "      </tr> " +
                 "     </xsl:otherwise> " +
                "    </xsl:choose> " +
               "   </xsl:template> ";
            }
        }
        private static string XsltReport
        {
            get
            {
                return "   <xsl:template name=\"ReportTitle\"> " +
                "    <xsl:if test=\"./Report/Title != ''\"> " +
                 "     <tr height =\"40\" valign =\"middle\" style=\"{$FontTitle}\"> " +
                  "      <td colspan =\"2\" align =\"center\"> " +
                   "       <b> " +
                    "        <xsl:value-of select=\"./Report/Title\"/> " +
                   "       </b> " +
                  "      </td> " +
                 "     </tr> " +
                "    </xsl:if> " +
               "   </xsl:template> ";
            }
        }
        private static string XsltEnd
        {
            get
            {
                return "    <xsl:template match=\"text\"></xsl:template> " +
             "   </xsl:stylesheet>";
            }
        }

        private static string DefaultXslt()
        {
            string Xslt = "<?xml version=\"1.0\" encoding=\"utf-16\" ?> " +
                "<xsl:stylesheet version=\"1.0\"  " +
                  "xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\"> " +
                  "<xsl:output method=\"xml\" encoding=\"utf-16\"/> " +

                  "<!--Fonts--> " +
                  "<xsl:variable name=\"FontDefault\">  font-size: 10pt; font-family: Arial</xsl:variable> " +
                  "<xsl:variable name=\"FontSmall\">    font-size:  8pt; font-family: Arial</xsl:variable> " +
                  "<xsl:variable name=\"FontTitle\">    font-size: 10pt; font-family: Arial</xsl:variable> " +
                  "<xsl:variable name=\"FontTaxonName\">font-size: 12pt; font-family: Arial</xsl:variable> " +
                  "<xsl:variable name=\"FontType\">     font-size: 12pt; font-family: Arial; text-transform:uppercase</xsl:variable> " +
                  "<xsl:variable name=\"FontBarcode\">  font-size: 16pt; font-family: Code 39</xsl:variable> " +
                  "<xsl:variable name=\"FontCountry\">  font-size:  8pt; font-family: Arial; text-transform:uppercase</xsl:variable> " +

                  "<!--Printing options--> " +
                  "<xsl:variable name=\"ReportHeader\">Header of report</xsl:variable> " +
                  "<xsl:variable name=\"PrintReportHeader\">0</xsl:variable> " +
                  "<xsl:variable name=\"PrintReportTitle\">1</xsl:variable> " +

                  "<xsl:template match=\"/CollectionSpecimens\"> " +
                    "<html> " +
                      "<body> " +
                        "<xsl:value-of select=\"/CollectionSpecimens/CollectionSpecimen/Storages/Storage/CollectionOwner\"/> " +
                        "<xsl:if test=\"$PrintReportHeader = 1\"> " +
                          "<hr/> " +
                          "<span style=\"{$FontTitle}\"> " +
                            "<xsl:value-of select=\"$ReportHeader\"/> " +
                          "</span> " +
                        "</xsl:if> " +
                        "<xsl:apply-templates select=\"CollectionSpecimen\"/> " +
                      "</body> " +
                    "</html> " +
                  "</xsl:template> " +

                  "<xsl:template match=\"CollectionSpecimen\"> " +
                    "<hr/> " +
                    "<table cellspacing=\"1\" cellpadding=\"1\" width=\"400\" border=\"0\" style=\"{$FontDefault}\"> " +
                      "<tr height =\"0\"> " +
                        "<th width=\"200\" ></th> " +
                        "<th width=\"200\"></th> " +
                      "</tr> " +
                      "<tr> " +
                        "<td align =\"left\"> " +
                          "<xsl:value-of select=\"./Storages/Storage/Collection\"/> " +
                        "</td> " +
                        "<td align =\"right\"> " +
                          "Nr.  <xsl:value-of select=\"./AccessionNumber\"/> " +
                        "</td> " +
                      "</tr> " +
                      "<tr height =\"40\" style=\"{$FontTaxonName}\"> " +
                        "<td colspan =\"2\"> " +
                          "<b> " +
                            "<xsl:for-each select=\"./Units/MainUnit/Identifications/Identification\"> " +
                              "<xsl:if test=\"position()=1\"> " +
                                "<xsl:call-template name=\"TaxonomicName\"/> " +
                              "</xsl:if> " +
                            "</xsl:for-each> " +
                         " </b> " +
                        "</td> " +
                      "</tr> " +
                      "<xsl:call-template name=\"Type\"/> " +
                      "<tr style=\"{$FontSmall}\"> " +
                        "<td colspan =\"2\"> " +
                         " <xsl:call-template name=\"Event\"/> " +
                          "<xsl:call-template name=\"Altitude\"/> " +
                        "</td> " +
                      "</tr> " +
                      "<tr style=\"{$FontSmall}\"> " +
                        "<td colspan =\"2\"> " +
                          "<xsl:call-template name=\"UnitHierarchy\"/> " +
                        "</td> " +
                      "</tr> " +
                      "<tr style=\"{$FontSmall}\"> " +
                        "<td colspan =\"1\" align =\"left\" valign =\"top\"> " +
                          "<xsl:call-template name=\"CollectionDate\"/> " +
                        "</td> " +
                        "<td colspan =\"1\" align =\"right\"> " +
                          "<xsl:apply-templates select =\"Collectors\"/> " +
                        "</td> " +
                      "</tr> " +
                      "<xsl:if test=\"./Units/MainUnit/Identifications/Identification/ResponsibleName != ''\"> " +
                        "<tr style=\"{$FontSmall}\"> " +
                          "<td > " +
                          "</td> " +
                          "<td colspan =\"1\" align =\"right\"> " +
                            "det.: <xsl:value-of select=\"./Units/MainUnit/Identifications/Identification/ResponsibleName\"/> " +
                          "</td> " +
                        "</tr> " +
                      "</xsl:if> " +
                      "<tr height =\"60\" style=\"{$FontBarcode}\"> " +
                        "<td colspan =\"2\" align=\"center\" valign =\"bottom\"> " +
                          "*<xsl:value-of select=\"./AccessionNumber\"/>* " +
                        "</td> " +
                      "</tr> " +
                      "<tr height =\"20\" style=\"{$FontDefault}\"> " +
                        "<td colspan =\"2\" align=\"center\" valign =\"top\"> " +
                          "<xsl:value-of select=\"./AccessionNumber\"/> " +
                        "</td> " +
                      "</tr> " +
                    "</table> " +
                    "<hr/> " +
                  "</xsl:template> " +

                  "<xsl:template match=\"Collectors\"> " +
                    "leg.: <xsl:apply-templates select =\"Collector\"/> " +
                  "</xsl:template> " +

                  "<xsl:template match=\"Collector\"> " +
                    "<xsl:value-of select=\"./CollectorsName\"/> " +
                    "<xsl:apply-templates select =\"CollectorsNumber\"/> " +
                    "<xsl:if test=\"position()!= last()\"> , " +

                    "</xsl:if> " +
                  "</xsl:template> " +

                  "<xsl:template match=\"CollectorsNumber\"> " +
                    "(<xsl:value-of select=\".\"/>) " +
                  "</xsl:template> " +

                  "<xsl:template name=\"Event\"> " +
                    "<span style= \"{$FontCountry}\"> " +
                      "<xsl:value-of select=\"./CollectionEvent/CountryCache\"/>. " +
                    "</span> " +
                    "<xsl:value-of select=\"./CollectionEvent/LocalityDescription\"/>. " +
                    "<xsl:value-of select=\"./CollectionEvent/HabitatDescription\"/>. " +
                  "</xsl:template> " +

                  "<xsl:template name=\"CollectionDate\"> " +
                    "<xsl:if test=\"./CollectionEvent/CollectionYear != '' Or ./CollectionEvent/CollectionMonth != '' or ./CollectionEvent/CollectionDay != ''\"> " +
                      "<xsl:value-of select=\"./CollectionEvent/CollectionYear\"/>/ " +
                      "<xsl:value-of select=\"./CollectionEvent/CollectionMonth\"/>/ " +
                      "<xsl:value-of select=\"./CollectionEvent/CollectionDay\"/> " +
                    "</xsl:if> " +
                  "</xsl:template> " +

                  "<xsl:template name=\"IdentificationDate\"> " +
                    "<xsl:if test=\"./Units/Unit/Identifications/Identification/IdentificationYear != '' or ./Units/Unit/Identifications/Identification/IdentificationMonth != '' or ./Units/Unit/Identifications/Identification/IdentificationDay != ''\"> " +
                      "<xsl:value-of select=\"./Units/Unit/Identifications/Identification/IdentificationYear\"/>/ " +
                      "<xsl:value-of select=\"./Units/Unit/Identifications/Identification/IdentificationMonth\"/>/ " +
                      "<xsl:value-of select=\"./Units/Unit/Identifications/Identification/IdentificationDay\"/> " +
                    "</xsl:if> " +
                  "</xsl:template> " +

                  "<xsl:template name=\"Altitude\"> " +
                    "<xsl:if test=\"./Geography/Altitude/from != ''\"> " +
                      "Alt. <xsl:value-of select=\"./Geography/Altitude/from\"/> " +
                    "</xsl:if> " +
                    "<xsl:if test=\"./Geography/Altitude/to != ''\"> " +
                      "- <xsl:value-of select=\"./Geography/Altitude/to\"/> " +
                    "</xsl:if> " +
                  "</xsl:template> " +

                  "<xsl:template name=\"UnitHierarchy\"> " +
                    "<xsl:call-template name=\"SubstrateUnit\"/> " +
                    "<xsl:call-template name=\"GrowingOnUnits\"/> " +
                    "<xsl:call-template name=\"AssociatedUnits\"/> " +
                    "<xsl:call-template name=\"Units\"/> " +
                  "</xsl:template> " +

                  "<xsl:template name=\"SubstrateUnit\"> " +
                    "<xsl:for-each select=\"./Units/SubstrateUnit\"> " +
                      "<xsl:if test=\"position() = 1\"> " +
                        "<xsl:if test=\"./RelationType != ''\"> " +
                          "<xsl:value-of select=\"./RelationType\"/> " +
                          "<xsl:text> </xsl:text> " +
                        "</xsl:if> " +
                        "<xsl:if test=\"./RelationType = '' or count(./RelationType) = 0\"> " +
                          "<xsl:text> On </xsl:text> " +
                        "</xsl:if> " +
                        "<xsl:if test=\"./ColonisedSubstratePart != ''\"> " +
                          "<xsl:value-of select=\"./ColonisedSubstratePart\"/> of " +
                        "</xsl:if> " +
                        "<xsl:if test=\"./ColonisedSubstratePart = '' and ./RelationType = ''\"> " +
                          "<xsl:text> On </xsl:text> " +
                        "</xsl:if> " +
                        "<xsl:call-template name=\"Unit\"/> " +
                        "<xsl:text>. </xsl:text> " +
                      "</xsl:if> " +
                    "</xsl:for-each> " +
                  "</xsl:template> " +

                  "<xsl:template name=\"GrowingOnUnits\"> " +
                    "<xsl:for-each select=\"./Units/GrowingOnUnit\"> " +
                      "<xsl:if test=\"position() = 1\"> " +
                        "<xsl:text>With </xsl:text> " +
                      "</xsl:if> " +
                      "<xsl:if test=\"position() > 1\"> " +
                        "<xsl:text> and </xsl:text> " +
                      "</xsl:if> " +
                      "<xsl:call-template name=\"Unit\"/> " +
                      "<xsl:if test=\"./RelationType != ''\"> " +
                        "<xsl:value-of select=\"./RelationType\"/> " +
                        "<xsl:text> </xsl:text> " +
                      "</xsl:if> " +
                      "<xsl:if test=\"./ColonisedSubstratePart != ''\"> " +
                        "<xsl:value-of select=\"./ColonisedSubstratePart\"/> of " +
                      "</xsl:if> " +
                      "<xsl:if test=\"position() = last()\"> " +
                        "<xsl:text>. </xsl:text> " +
                      "</xsl:if> " +
                    "</xsl:for-each> " +
                  "</xsl:template> " +

                  "<xsl:template name=\"AssociatedUnits\"> " +
                    "<xsl:for-each select=\"./Units/AssociatedUnit\"> " +
                      "<xsl:if test=\"position() = 1\"> " +
                        "<xsl:text>Associated with </xsl:text> " +
                      "</xsl:if> " +
                      "<xsl:if test=\"position() > 1\"> " +
                        "<xsl:text> and </xsl:text> " +
                      "</xsl:if> " +
                      "<xsl:call-template name=\"Unit\"/> " +
                      "<xsl:if test=\"./RelationType != ''\"> " +
                        "<xsl:value-of select=\"./RelationType\"/> " +
                        "<xsl:text> </xsl:text> " +
                      "</xsl:if> " +
                      "<xsl:if test=\"./ColonisedSubstratePart != ''\"> " +
                        "<xsl:value-of select=\"./ColonisedSubstratePart\"/> " +
                      "</xsl:if> " +
                      "<xsl:if test=\"position() = last()\"> " +
                        "<xsl:text>. </xsl:text> " +
                      "</xsl:if> " +
                    "</xsl:for-each> " +
                  "</xsl:template> " +

                  "<xsl:template name=\"Units\"> " +
                    "<xsl:for-each select=\"./Units/Unit\"> " +
                      "<xsl:if test=\"position() = 1\"> Further <xsl:value-of select=\"./TaxonomicGroup\"/>s: " +
                      "</xsl:if> " +
                      "<xsl:if test=\"position() > 1\"> " +
                        "<xsl:text> and </xsl:text> " +
                      "</xsl:if> " +
                      "<xsl:call-template name=\"Unit\"/> " +
                      "<xsl:if test=\"./Substrate != ''\"> " +
                        "<xsl:if test=\"./RelationType != ''\"> " +
                          "<xsl:value-of select=\"./RelationType\"/> " +
                          "<xsl:text> </xsl:text> " +
                        "</xsl:if> " +
                        "<xsl:if test=\"./ColonisedSubstratePart != ''\"> " +
                          "<xsl:value-of select=\"./ColonisedSubstratePart\"/> of <xsl:value-of select=\"./Substrate\"/> " +
                        "</xsl:if> " +
                        "<xsl:if test=\"position() = last()\"> " +
                          "<xsl:text>. </xsl:text> " +
                        "</xsl:if> " +
                      "</xsl:if> " +
                    "</xsl:for-each> " +
                  "</xsl:template> " +

                  "<xsl:template name=\"Unit\"> " +
                    "<xsl:for-each select=\"./Identifications/Identification\"> " +
                      "<xsl:if test=\"position() = 1\"> " +
                        "<xsl:if test=\"count(./Taxon) > 0\"> " +
                          "<xsl:call-template name=\"TaxonomicName\"/> " +
                        "</xsl:if> " +
                        "<xsl:if test=\"count(./Taxon) = 0\"> " +
                          "<xsl:if test=\"./TaxonomicName != ''\"> " +
                            "<xsl:value-of select=\"./TaxonomicName\"></xsl:value-of> " +
                          "</xsl:if> " +
                          "<xsl:if test=\"./TaxonomicName = '' and ./VernacularTerm != ''\"> " +
                            "<xsl:value-of select=\"./VernacularTerm\"></xsl:value-of> " +
                          "</xsl:if> " +
                        "</xsl:if> " +
                      "</xsl:if> " +
                    "</xsl:for-each> " +
                  "</xsl:template> " +

                  "<xsl:template name=\"Type\"> " +
                    "<xsl:if test=\"./Units/MainUnit/Identifications/Identification/TypeStatus != ''\"> " +
                      "<tr height =\"40\" valign =\"middle\" style=\"{$FontType}\"> " +
                        "<td colspan =\"2\" align =\"center\"> " +
                          "<b> " +
                          "  <xsl:value-of select=\"./Units/MainUnit/Identifications/Identification/TypeStatus\"/> " +
                          "</b> " +
                       " </td> " +
                      "</tr> " +
                    "</xsl:if> " +
                 " </xsl:template> " +

                 " <xsl:template name=\"TaxonomicName\"> " +
                   " <i> " +
                     " <xsl:value-of select=\"concat(./Taxon/Genus, ' ' , ./Taxon/SpeciesEpithet)\"/> " +
                    "</i> " +
                    "<xsl:if test=\"./Taxon/Rank!='sp.'\"> " +
                    "  <xsl:if test=\"./Taxon/SpeciesEpithet  " +
                      "  = ./Taxon/IntraSpecificEpithet\"> " +
                       " <xsl:value-of select=\"concat(' ' , ./Taxon/Authors)\"/> " +
                     " </xsl:if> " +
                     " <xsl:value-of select=\"concat(' ' ,./Taxon/Rank)\"/> " +
                     " <i> " +
                      "  <xsl:value-of select=\"concat(' ' , ./Taxon/IntraSpecificEpithet)\"/> " +
                     " </i> " +
                     " <xsl:if test=\"./Taxon/SpeciesEpithet  " +
                      "  != ./Taxon/IntraSpecificEpithet\"> " +
                       " <xsl:value-of select=\"concat(' ' , ./Taxon/Authors)\"/> " +
                     " </xsl:if> " +
                   " </xsl:if> " +
                   " <xsl:if test=\"./Taxon/Rank='sp.'\"> " +
                   "   <xsl:value-of select=\"concat(' ' , ./Taxon/Authors)\"/> " +
                    "</xsl:if> " +
                 " </xsl:template> " +

                  "<xsl:template name=\"TaxonomicName2Lines\"> " +
                   " <xsl:choose> " +
                    "  <xsl:when test=\"./Taxon/Rank!='sp.'\"> " +
                     "   <tr height =\"40\"> " +
                      "    <td colspan =\"2\" valign=\"bottom\"> " +
                       "     <span style=\"{$FontTaxonName}\"> " +
                        "      <xsl:value-of select=\"concat(./Taxon/Genus, ' ' , ./Taxon/SpeciesEpithet)\"/> " +
                         "   </span> " +
                          "</td> " +
                    "    </tr> " +
                     "   <tr height =\"40\"> " +
                      "    <td> " +
                       "   </td> " +
                        "  <td valign=\"top\"> " +
                          "  <span style=\"{$FontTaxonName}\"> " +
                         "     <xsl:value-of select=\"concat(./Taxon/Rank, ' ' , ./Taxon/IntraSpecificEpithet, ' ' , ./Taxon/Authors)\"/> " +
                        "    </span> " +
                       "   </td> " +
                      "  </tr> " +
                   "   </xsl:when> " +
                    "  <xsl:otherwise> " +
                     "   <tr height =\"60\"> " +
                       "   <td colspan =\"2\"> " +
                      "      <span style=\"font-size: 14pt; font-family: Verdana\"> " +
                     "         <xsl:value-of select=\"./TaxonomicName\"/> " +
                    "        </span> " +
                   "       </td> " +
                  "      </tr> " +
                 "     </xsl:otherwise> " +
                "    </xsl:choose> " +
               "   </xsl:template> " +

               "   <xsl:template name=\"ReportTitle\"> " +
                "    <xsl:if test=\"./Report/Title != ''\"> " +
                 "     <tr height =\"40\" valign =\"middle\" style=\"{$FontTitle}\"> " +
                  "      <td colspan =\"2\" align =\"center\"> " +
                   "       <b> " +
                    "        <xsl:value-of select=\"./Report/Title\"/> " +
                   "       </b> " +
                  "      </td> " +
                 "     </tr> " +
                "    </xsl:if> " +
               "   </xsl:template> " +

              "    <xsl:template match=\"text\"></xsl:template> " +

             "   </xsl:stylesheet>\"";
            return Xslt;
        }

        #endregion
    }
}
