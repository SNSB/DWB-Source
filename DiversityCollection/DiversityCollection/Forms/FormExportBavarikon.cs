using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{

    //public struct KulturObjekt
    //{
    //    public string bav07_Titel_Name_Objektbezeichnung;
    //    public string bav08_Alternativtitel;
    //    public System.Collections.Generic.Dictionary<string, string> bav12_Ereignis;
    //}

    public struct XmlNode
    {
        public string Name;
        public System.Object Content;
        public System.Collections.Generic.List<XmlAttribut> xmlAttributs;
    }

    public struct XmlAttribut
    {
        public string Name;
        public string Content;
    }

    public partial class FormExportBavarikon : Form
    {

        #region const Parameter

        System.Data.DataTable _dtCollectionProjects;
        private System.IO.FileInfo _XmlFile;
        private string _bav04_DatenlieferndeInstitution = "Staatliche Naturwissenschaftliche Sammlungen Bayerns (SNSB)";
        private string _bav05_BestandshaltendeInstitution = "SNSB - Botanische Staatssammlung München";
        private System.Collections.Generic.List<string> _bav10_Objektkategorie;
        private System.Collections.Generic.List<string> _bav11_Schlagwort_Thema;
        private string _bav13_Sprache = "Deutsch";
        System.Data.DataTable _dtKulturobjekte;

        #endregion

        #region Variable user input

        private System.Collections.Generic.List<string> bav10_Objektkategorie
        {
            get
            {
                if (_bav10_Objektkategorie == null)
                {
                    _bav10_Objektkategorie = new List<string>();
                    _bav10_Objektkategorie.Add("Malerei");
                    _bav10_Objektkategorie.Add("Nachlass");
                }
                return _bav10_Objektkategorie;
            }
        }

        private System.Collections.Generic.List<string> bav11_Schlagwort
        {
            get
            {
                if (_bav11_Schlagwort_Thema == null || _bav11_Schlagwort_Thema.Count == 0 || _bav11_Schlagwort_Thema.Count != this.listBoxSchlagwort.Items.Count)
                {
                    _bav11_Schlagwort_Thema = new List<string>();
                    foreach (System.Object o in this.listBoxSchlagwort.Items)
                        _bav11_Schlagwort_Thema.Add(o.ToString());
                }
                return _bav11_Schlagwort_Thema;
            }
        }



        private string _Hauptverantwortlichkeit;
        private string _Datenpaket;
        private string Datenpaket
        {
            get
            {
                _Datenpaket = this.textBoxDatenpaket.Text;
                return _Datenpaket;
            }
            set
            {
                _Datenpaket = value;
                this.textBoxDatenpaket.Text = _Datenpaket;
            }
        }

        private string Hauptverantwortlichkeit
        {
            get
            {
                _Hauptverantwortlichkeit = this.textBoxHauptverantwortlichkeit.Text;
                return _Hauptverantwortlichkeit;
            }
            set
            {
                _Hauptverantwortlichkeit = value;
                this.textBoxHauptverantwortlichkeit.Text = _Hauptverantwortlichkeit;
            }
        }

        private int ProjectID
        {
            get
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxProject.SelectedItem;
                int ID = 0;
                int.TryParse(R["ProjectID"].ToString(), out ID);
                return ID;
            }
        }

        private string ProjektNummer { get { return this.textBoxProjektNummer.Text; } }

        private string Objekttyp { get => this.textBoxObjekt.Text; }

        private string GND_ID { get => this.textBoxGND_ID.Text; }

        private string Material { get => this.textBoxMaterial.Text; }

        //private System.Collections.Generic.List<string> Schlagworte
        //{
        //    get
        //    {
        //        System.Collections.Generic.List<string> L = new List<string>();
        //        foreach (System.Object o in this.listBoxSchlagwort.Items)
        //            L.Add(o.ToString());
        //        return L;
        //    }
        //}

        #endregion

        /*
         * <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
        <bavarikonDatenlieferung datenpaket_name="Sammlung Pilzaquarelle von Konrad Schieferdecker (1902-1965)" export_zeitstempel="2023-02-18T18:27:56" schema_version="1.0.0" xmlns="https://www.bavarikon.de">
        <Kulturobjekt>
        <bav01_bavarikonProjektnummer>0219</bav01_bavarikonProjektnummer>
        <bav02_LieferID typ="Datensatznummer">M-0036907</bav02_LieferID>
        <bav03_AnzeigeID typ="Datensatznummer">M-0036907</bav03_AnzeigeID>
        <bav04_DatenlieferndeInstitution gnd-id="2001684-0">Staatliche Naturwissenschaftliche Sammlungen Bayerns</bav04_DatenlieferndeInstitution>
        <bav05_BestandshaltendeInstitution gnd-id="2005856-1">SNSB - Botanische Staatssammlung München</bav05_BestandshaltendeInstitution>
        <bav07_Titel_Name_Objektbezeichnung>Uromyces pisi (DC.) G. H. Otth - Aquarellzeichnung</bav07_Titel_Name_Objektbezeichnung>
        <bav08_Alternativtitel></bav08_Alternativtitel>
        <bav10_Objektkategorie>Malerei</bav10_Objektkategorie>
        <bav10_Objektkategorie>Nachlass</bav10_Objektkategorie>
        <bav11_Schlagwort_Thema typ="Sachbegriff">Aquarell</bav11_Schlagwort_Thema>
        <bav11_Schlagwort_Thema typ="Sachbegriff">Biologische Sammlung</bav11_Schlagwort_Thema>
        <bav11_Schlagwort_Thema typ="Sachbegriff">Pilz</bav11_Schlagwort_Thema>
        <bav11_Schlagwort_Thema typ="Sachbegriff">Mykologie</bav11_Schlagwort_Thema>
        <bav12_Ereignis typ="Entstehung">
            <Hauptverantwortlichkeit typ="Person" gnd-id="0123456789">Konrad Schieferdecker</Hauptverantwortlichkeit>
        </bav12_Ereignis>
        <bav12_Ereignis typ="Entstehung">
            <Ort>Hildesheim, Kreis Hildesheim, Niedersachsen, Germany. Hildesheim, Garten.</Ort>
        </bav12_Ereignis>
        <bav12_Ereignis typ="Entstehung">
            <Zeit>26.07.1945</Zeit>
        </bav12_Ereignis>
        <bav13_Sprache>Deutsch</bav13_Sprache>
        <bav14_Material>Aquarelltechnik auf Papier</bav14_Material>
        <bav15_Umfang_Abmessungen_Laufzeit>30 x 40 cm (Breite x Höhe))</bav15_Umfang_Abmessungen_Laufzeit>
        <bav16_Bemerkung>Handschiftliche Notiz auf dem Aquarell: Teleutosporenlager auf der Blattunterseite</bav16_Bemerkung>
        <bav17_RechtedeklarationMetadaten>CC0</bav17_RechtedeklarationMetadaten>
        <bav18_RechtedeklarationBeschreibungstext>CC BY 4.0</bav17_RechtedeklarationMetadaten>
        <bav19_RechtedeklarationDigitalisat>CC BY-SA 4.0</bav19_RechtedeklarationDigitalisat>
        <bav21_Dateinamen_Bilddateien>M-0036907.tif</bav21_Dateinamen_Bilddateien>
        </Kulturobjekt>
        </bavarikonDatenlieferung>


        <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
        <bavarikonDatenlieferung datenpaket_name="Sammlung Pilzaquarelle von Dr. Fritz Wohlfarth (1906-2005)" export_zeitstempel="2023-02-18T17:49:13" schema_version="1.0.0" xmlns="https://www.bavarikon.de">
            <Kulturobjekt>
                <bav01_bavarikonProjektnummer>0219</bav01_bavarikonProjektnummer>
                <bav02_LieferID typ="Datensatznummer">M-0031349</bav02_LieferID>
                <bav03_AnzeigeID typ="Datensatznummer">M-0031349</bav03_AnzeigeID>
                <bav04_DatenlieferndeInstitution gnd-id="2001684-0">Staatliche Naturwissenschaftliche Sammlungen Bayerns</bav04_DatenlieferndeInstitution>
                <bav05_BestandshaltendeInstitution gnd-id="2005856-1">SNSB - Botanische Staatssammlung München</bav05_BestandshaltendeInstitution>
                <bav07_Titel_Name_Objektbezeichnung>Phellodon niger (Fr. : Fr.) P. Karst. - Aquarellzeichnung</bav07_Titel_Name_Objektbezeichnung>
                <bav08_Alternativtitel>Schwarzer Korkstacheling</bav08_Alternativtitel>
                <bav10_Objektkategorie>Malerei</bav10_Objektkategorie>
                <bav10_Objektkategorie>Nachlass</bav10_Objektkategorie>
                <bav11_Schlagwort_Thema typ="Sachbegriff">Aquarell</bav11_Schlagwort_Thema>
                <bav11_Schlagwort_Thema typ="Sachbegriff">Biologische Sammlung</bav11_Schlagwort_Thema>
                <bav11_Schlagwort_Thema typ="Sachbegriff">Pilz</bav11_Schlagwort_Thema>
                <bav11_Schlagwort_Thema typ="Sachbegriff">Mykologie</bav11_Schlagwort_Thema>
                <bav12_Ereignis typ="Entstehung">
                    <Hauptverantwortlichkeit typ="Person" gnd-id="1129113183">Dr. Fritz Wohlfarth</Hauptverantwortlichkeit>
                </bav12_Ereignis>
                <bav12_Ereignis typ="Entstehung">
                    <Ort>Polsdorf, Stadt-/Gemeindeteil v. Allersberg, Kreis Roth, Bayern, Germany. In sandigem Kiefernwald sehr stark mit Moos verwachsen. Polsdorf bei Hilpoltstein</Ort>
                </bav12_Ereignis>
                <bav12_Ereignis typ="Entstehung">
                    <Zeit>28.11.1968</Zeit>
                </bav12_Ereignis>
                <bav13_Sprache>Deutsch</bav13_Sprache>
                <bav14_Material>Aquarelltechnik auf Papier</bav14_Material>
                <bav15_Umfang_Abmessungen_Laufzeit>30 x 40 cm (Breite x Höhe))</bav15_Umfang_Abmessungen_Laufzeit>
                <bav16_Bemerkung>Handschiftliche Notiz auf dem Aquarell: [painted] Braunweiler, 28.11.1968   Fleisch schwarz, ledrig zäh</bav16_Bemerkung>
                <bav17_RechtedeklarationMetadaten>CC0</bav17_RechtedeklarationMetadaten>
                <bav18_RechtedeklarationBeschreibungstext>CC BY 4.0</bav17_RechtedeklarationMetadaten>
                <bav19_RechtedeklarationDigitalisat>CC BY-SA 4.0</bav19_RechtedeklarationDigitalisat>
                <bav21_Dateinamen_Bilddateien>M-0031349.tif</bav21_Dateinamen_Bilddateien>
            </Kulturobjekt>
        </bavarikonDatenlieferung>


            */

        #region Construction
        public FormExportBavarikon()
        {
            InitializeComponent();
            this.initForm();
        }

        #endregion
                                                    
        #region Form

        private void initForm()
        {
            if (this._dtCollectionProjects == null) this._dtCollectionProjects = new DataTable();
            else this._dtCollectionProjects.Clear();
            string SQL = "SELECT [ProjectID], [Project] " +
                "FROM [dbo].[ProjectList] ORDER BY Project";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(this._dtCollectionProjects);

            this.comboBoxProject.DataSource = this._dtCollectionProjects;
            this.comboBoxProject.DisplayMember = "Project";
            this.comboBoxProject.ValueMember = "ProjectID";

            this.initObjektKategorie();
        }

        private void toolStripButtonKategorieAdd_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonKategorieDelete_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonSchlagwortAdd_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Schlagwort", "Bitte Schlagwort eingeben", "");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK && f.String.Length > 0)
            {
                this.listBoxSchlagwort.Items.Add(f.String);
                this._bav11_Schlagwort_Thema = null;
            }
        }

        private void toolStripButtonSchlagwortDelete_Click(object sender, EventArgs e)
        {
            if (this.listBoxSchlagwort.SelectedItem != null)
            {
                this.listBoxSchlagwort.Items.Remove(this.listBoxSchlagwort.SelectedItem);
                this._bav11_Schlagwort_Thema = null;
            }
        }


        private void initObjektKategorie()
        {
            foreach (string o in this.bav10_Objektkategorie)
                this.listBoxKategorie.Items.Add(o);
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this._ExportExceptions = "";
            if (this.checkBoxSingleFile.Checked)
                this.Export();
            else
            {
                this.progressBar.Value = 0;
                this.progressBar.Maximum = this.DataTableKulturObjekte().Rows.Count;
                foreach (System.Data.DataRow R in this.DataTableKulturObjekte(false).Rows)
                    this.Export(R);
            }
            if (_XmlFile != null)
            {
                this.buttonOpenDirectory.Tag = this._XmlFile;
                System.Uri u = new Uri(this._XmlFile.FullName);
                this.userControlWebView.Url = null;
                this.userControlWebView.Navigate(u);
                this.dataGridView.DataSource = this.DataTableKulturObjekte(false);
                this.dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            if(_ExportExceptions.Length > 0)
            {
                System.Windows.Forms.MessageBox.Show(_ExportExceptions, "Aufgetretene Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonOpenDirectory_Click(object sender, EventArgs e)
        {
            if (this.buttonOpenDirectory.Tag != null)
            {
                System.IO.FileInfo fileInfo = (System.IO.FileInfo)this.buttonOpenDirectory.Tag;
                if (fileInfo is null || fileInfo.DirectoryName is null)
                {
                    System.Windows.Forms.MessageBox.Show("Opening the directory is not possible");
                    return;
                }

                this.openFileDialog.InitialDirectory = fileInfo.DirectoryName;
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(this.openFileDialog.FileName);
                    if (file.Exists)
                    {
                        // System.Diagnostics.Process.Start(this.openFileDialog.FileName);
                        System.Diagnostics.Process.Start(new ProcessStartInfo
                        {
                            FileName = this.openFileDialog.FileName,
                            UseShellExecute = true
                        });
                    }
                }
            }
        }

        #endregion

        #region Export

        private string _ExportExceptions = "";

        private void Export(System.Data.DataRow Row = null)
        {
            string xmlFile = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.FolderType.Export);
            if (Row != null)
            {
                string xmlDir = xmlFile + "bavaricon\\" + this.comboBoxProject.Text + "_" + System.DateTime.Now.ToString("yyyyMMdd");
                System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(xmlDir);
                if (!directory.Exists)
                    directory.Create();
                xmlFile = directory.FullName + "\\" + Row["AccessionNumber"].ToString();
            }
            else
            {
                xmlFile += this.comboBoxProject.Text + "_" + System.DateTime.Now.ToString("yyyyMMdd");
            }
            xmlFile += ".xml";
            this._XmlFile = new System.IO.FileInfo(xmlFile);
            if (Row != null && this._XmlFile.Exists)
                this._XmlFile.Delete();
            System.Xml.XmlWriter W;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            W = System.Xml.XmlWriter.Create(this._XmlFile.FullName, settings);
            try
            {
                //W.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                W.WriteStartDocument(true);
                //        <?xml version="1.0" encoding="UTF-8" standalone="yes"?>

                W.WriteStartElement("bavarikonDatenlieferung");
                // datenpaket_name="Sammlung Pilzaquarelle von Dr. Fritz Wohlfarth (1906-2005)" export_zeitstempel="2023-02-18T17:49:13" schema_version="1.0.0" xmlns="https://www.bavarikon.de"
                W.WriteAttributeString("datenpaket_name", this.Datenpaket);
                W.WriteAttributeString("export_zeitstempel", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                W.WriteAttributeString("schema_version", "1.0.0");

                //xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="https://www.bavarikon.de" xsi:schemaLocation="https://www.bavarikon.de bavarikon-Lieferformat.xsd

                //W.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                //W.WriteAttributeString("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                //W.WriteAttributeString("xmlns", "https://www.bavarikon.de");
                //W.WriteAttributeString("xsi:schemaLocation", "https://www.bavarikon.de bavarikon-Lieferformat.xsd");

                //W.WriteAttributeString("xsi:schemaLocation", "https://www.bavarikon.de bavarikon-Lieferformat.xsd");
                //W.WriteAttributeString("xmlns", "xsi", "schemaLocation", "https://www.bavarikon.de bavarikon-Lieferformat.xsd");
                //W.WriteAttributeString("xsi", "schemaLocation", "https://www.bavarikon.de");

                //W.WriteAttributeString("xsi:schemaLocation", "https://www.bavarikon.de");
                if (Row == null)
                {
                    this.progressBar.Value = 0;
                    this.progressBar.Maximum = this.DataTableKulturObjekte().Rows.Count;
                    foreach (System.Data.DataRow R in this.DataTableKulturObjekte(false).Rows)
                    {
                        W.WriteStartElement("Kulturobjekt");
                        this.ExportKulturObjekt(ref W, R);
                        W.WriteEndElement();        //Kulturobjekt
                        if (progressBar.Value < progressBar.Maximum) progressBar.Value++;
                    }
                }
                else
                {
                    W.WriteStartElement("Kulturobjekt");
                    this.ExportKulturObjekt(ref W, Row);
                    W.WriteEndElement();        //Kulturobjekt
                    if (progressBar.Value < progressBar.Maximum) progressBar.Value++;
                }
                //W.WriteEndElement();        //bavarikonDatenlieferung
                W.WriteFullEndElement();    //bavarikonDatenlieferung
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

        private System.Data.DataTable DataTableKulturObjekte(bool Refresh = true)
        {
            if (Refresh)
                this._dtKulturobjekte = new DataTable();
            else
            {
                if (this._dtKulturobjekte != null && this._dtKulturobjekte.Rows.Count > 0)
                    return this._dtKulturobjekte;
            }
            string SQL = "SELECT S.CollectionSpecimenID AS ID, S.AccessionNumber, S.OriginalNotes, " +
                "case when E.CollectionDate is null then case when E.CollectionYear is null then '' else cast(E.CollectionYear as varchar) end else convert(varchar(10), [CollectionDate], 104)  end AS Datum, " + // cast(E.CollectionYear as varchar) end else cast(E.CollectionDay as varchar) + '.' + cast(E.CollectionMonth as varchar) + '.' + cast(E.CollectionYear as varchar) end AS Datum, " +
                "L.Location1 AS NamedArea, L.Location2 AS LinkDG, E.CountryCache, " +
                "I.TaxonomicName + CASE WHEN I.IdentificationQualifier IS NULL OR I.IdentificationQualifier = '' THEN '' ELSE ' ' + I.IdentificationQualifier END AS TaxonomicName, Max(V.VernacularTerm) AS VernacularTerm, " +
                "[dbo].[StableIdentifier] (" + this.ProjectID.ToString() + ", S.CollectionSpecimenID, MIN(U.IdentificationUnitID), MIN(SP.SpecimenPartID)) AS Permalink " +
                "FROM CollectionSpecimen AS S INNER JOIN " +
                "CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID INNER JOIN " +
                "CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID INNER JOIN " +
                "IdentificationUnit AS U ON S.CollectionSpecimenID = U.CollectionSpecimenID AND U.TaxonomicGroup = 'fungus' INNER JOIN " +
                "Identification AS I ON U.CollectionSpecimenID = I.CollectionSpecimenID AND U.IdentificationUnitID = I.IdentificationUnitID AND U.LastIdentificationCache = I.TaxonomicName LEFT OUTER JOIN " +
                "Identification AS V ON U.CollectionSpecimenID = V.CollectionSpecimenID AND U.IdentificationUnitID = V.IdentificationUnitID LEFT OUTER JOIN " +
                "CollectionEventLocalisation AS L ON E.CollectionEventID = L.CollectionEventID AND L.LocalisationSystemID = 7  LEFT OUTER JOIN " +
                "CollectionSpecimenPart AS SP ON S.CollectionSpecimenID = SP.CollectionSpecimenID " + 
                "WHERE (P.ProjectID = " + this.ProjectID.ToString() + ") AND (S.DataWithholdingReason IS NULL OR S.DataWithholdingReason = '') " +
                "GROUP BY E.CollectionDate, E.CollectionDay, E.CollectionMonth, E.CollectionYear, S.CollectionSpecimenID, S.AccessionNumber, L.Location1, I.TaxonomicName, I.IdentificationQualifier, S.OriginalNotes, L.Location2, E.CountryCache " + //
                "ORDER BY S.AccessionNumber";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._dtKulturobjekte);
            return _dtKulturobjekte;
        }

        private string TranslateCountry(string NamedArea, string LinkToGazetteer, string Country, ref string Error)
        {
            string Location = NamedArea;
            try
            {
                if (_Countries == null) _Countries = new Dictionary<string, string>();
                if (Country.Length > 0 && !_Countries.ContainsKey(Country))
                {
                    System.Collections.Generic.Dictionary<string, DiversityWorkbench.WorkbenchUnit> dict = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList();
                    if (dict.ContainsKey("DiversityGazetteer"))
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in dict["DiversityGazetteer"].ServerConnectionList())
                        {
                            if (LinkToGazetteer.StartsWith(KV.Value.BaseURL))
                            {
                                string NameID = DiversityWorkbench.WorkbenchUnit.getIDFromURI(LinkToGazetteer);
                                int ID;
                                if (NameID.Length > 0 && int.TryParse(NameID, out ID))
                                {
                                    string Prefix = "dbo.";
                                    if (KV.Value.LinkedServer.Length > 0)
                                        Prefix = "[" + KV.Value.LinkedServer + "]." + KV.Value.DatabaseName + ".dbo.";
                                    this.GetCountries(Prefix, Country);
                                    if (!_Countries.ContainsKey(Country))
                                    {
                                        string Translation = GetCountry(ID, Prefix);
                                        if (Translation.Length > 0 && Translation != Country)
                                        {
                                            if (!_Countries.ContainsKey(Country))
                                                _Countries.Add(Country, Translation);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (_Countries.ContainsKey(Country))
                {
                    if (Location.IndexOf(Country) > -1)
                        Location = Location.Replace(Country, _Countries[Country]);
                }
                else if (Country.Length > 0)
                {
                    Error = "Keine Übersetzung von " + Country + "\r\n";
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return Location;
        }

        private void GetCountries(string Prefix, string Country)
        {
            string SQL = "select N.Name, D.Name from " + Prefix + "[GeoName] N " +
                "inner join " + Prefix + "[ViewGeoPlace] P on  N.PlaceID = P.PlaceID and P.PlaceType = 'nation' and N.Name = '" + Country + "' " +
                "inner join " + Prefix + "[GeoName] D on P.PlaceID = D.PlaceID and D.LanguageCode = 'de' and N.NameID <> D.NameID and len(n.name) > 3 and ISNUMERIC(substring(N.Name, len(N.Name), 1)) = 0";
            System.Data.DataTable dtCountry = new DataTable(); // = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL, "Country");
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtCountry);
            foreach (System.Data.DataRow R in dtCountry.Rows)
            {
                //string Country = R[0].ToString();
                string Translation = R[1].ToString();
                if (!_Countries.ContainsKey(Country) && Country.Length > 0 && Translation.Length > 0 && Country != Translation)
                    _Countries.Add(Country, Translation);
            }
        }

        private string GetCountry(int NameID, string Prefix)
        {
            string Country = "";
            if (NameID > 0)
            {
                string SQL = "SELECT TOP 1 NC.Name AS Country " +
                    "FROM " + Prefix + "GeoName AS N INNER JOIN " +
                    Prefix + "ViewGeoPlace AS P ON N.PlaceID = P.PlaceID INNER JOIN " +
                    Prefix + "GeoName AS NC ON P.CountryPlaceID_Cache = NC.PlaceID " +
                    "WHERE N.NameID = " + NameID.ToString() + " AND NC.LanguageCode = 'de'";
                Country = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Country.Length > 0)
                    return Country;

                SQL = "SELECT TOP 1 N.PlaceID " +
                    "FROM " + Prefix + "GeoName AS N " +
                    "WHERE N.NameID = " + NameID.ToString();
                int PlaceID = 0;
                if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out PlaceID))
                {
                    Country = GetCountry(Prefix, PlaceID);
                }
            }
            return Country;
        }

        private string GetCountry(string Prefix, int PlaceID)
        {
            string Country = "";
            if (PlaceID > 0)
            {
                string SQL = "SELECT TOP 1 N.Name AS Country " +
                    "FROM " + Prefix + "GeoName AS N INNER JOIN " +
                    Prefix + "ViewGeoPlace AS P ON N.PlaceID = P.PlaceID " +
                    "WHERE N.PlaceID = " + PlaceID + " AND N.LanguageCode = 'de' AND P.PlaceType = 'nation'";
                Country = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Country.Length > 0)
                    return Country;

                SQL = "SELECT NC.Name AS Country, PC.PlaceType, P.SuperiorPlaceID, NC.LanguageCode " +
                    "FROM " + Prefix + "GeoName AS N INNER JOIN " +
                    Prefix + "ViewGeoPlace AS P ON N.PlaceID = P.PlaceID INNER JOIN " +
                    Prefix + "GeoName AS NC ON P.SuperiorPlaceID = NC.PlaceID INNER JOIN " +
                    Prefix + "ViewGeoPlace AS PC ON P.SuperiorPlaceID = PC.PlaceID " +
                    "WHERE N.PlaceID = " + PlaceID;
                System.Data.DataTable dtCountry = new DataTable(); // = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL, "Country");
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtCountry);
                if (dtCountry.Rows.Count > 0)
                {
                    foreach(System.Data.DataRow R in dtCountry.Rows)
                    {
                        if(R["PlaceType"].ToString().ToLower() == "nation")
                        {
                            if (R["LanguageCode"].ToString().ToLower() == "de")
                            {
                                Country = R["Country"].ToString();
                                return Country;
                            }
                            SQL = "SELECT TOP 1 N.Name " +
                                "FROM " + Prefix + "GeoName AS N " +
                                "WHERE N.PlaceID = " + R["SuperiorPlaceID"].ToString() + " AND N.LanguageCode = 'de'";
                            Country = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                            if (Country.Length > 0)
                                return Country;
                        }
                    }
                    int SuperiorPlaceID;
                    if (int.TryParse(dtCountry.Rows[0]["SuperiorPlaceID"].ToString(), out SuperiorPlaceID))
                    {
                        Country = GetCountry(Prefix, SuperiorPlaceID);
                    }
                }
            }
            return Country;
        }

        private System.Collections.Generic.Dictionary<string, string> _Countries;


        private void ExportKulturObjekt(ref System.Xml.XmlWriter W, System.Data.DataRow R)
        {
            System.Collections.Generic.SortedDictionary<string, System.Object> Objekt = this.KulturObject(R);
            foreach(System.Collections.Generic.KeyValuePair<string, System.Object> KV in Objekt)
            {
                if (KV.Value.GetType() == typeof(string))
                {
                    W.WriteElementString(KV.Key, KV.Value.ToString());
                }
                else if (KV.Value.GetType() == typeof(XmlNode))
                {
                    XmlNode xmlNode = (XmlNode)KV.Value;
                    this.WriteXmlNode(ref W, xmlNode);
                }
                else { }
            }
        }

        private void WriteXmlNode(ref System.Xml.XmlWriter W, XmlNode xmlNode)
        {
            if (xmlNode.xmlAttributs != null && xmlNode.xmlAttributs.Count > 0)
            {
                W.WriteStartElement(xmlNode.Name);
                foreach (XmlAttribut xmlAttribut in xmlNode.xmlAttributs)
                {
                    W.WriteAttributeString(xmlAttribut.Name, xmlAttribut.Content);
                }
                if (xmlNode.Content.GetType() == typeof(string))
                    W.WriteString(xmlNode.Content.ToString());
                else
                {
                    if (xmlNode.Content.GetType() == typeof(XmlNode))
                    {
                        XmlNode node = (XmlNode)xmlNode.Content;
                        this.WriteXmlNode(ref W, node);
                    }
                }
                W.WriteEndElement();        //KV.Key
            }
            else
            {
                W.WriteElementString(xmlNode.Name, xmlNode.Content.ToString());
            }
        }

        private enum XmlType { Attribute, Content }
        private  System.Collections.Generic.SortedDictionary<string, System.Object> KulturObject(System.Data.DataRow R)
        {
            System.Collections.Generic.SortedDictionary<string, System.Object> dict = new SortedDictionary<string, object>();

            dict.Add("bav01_bavarikonProjektnummer", this.ProjektNummer);

            System.Collections.Generic.List<XmlAttribut> Datensatznummer = new List<XmlAttribut>();
            XmlAttribut xmlAttributDatensatznummer = new XmlAttribut();
            xmlAttributDatensatznummer.Content = "Datensatznummer";
            xmlAttributDatensatznummer.Name = "typ";
            Datensatznummer.Add(xmlAttributDatensatznummer);
            XmlNode xmlNodeNummer = new XmlNode();
            xmlNodeNummer.Name = "bav02_LieferID";
            xmlNodeNummer.Content = R["AccessionNumber"].ToString();
            xmlNodeNummer.xmlAttributs = Datensatznummer;

            dict.Add(xmlNodeNummer.Name, xmlNodeNummer);

            XmlNode xmlNodeAnzeige = new XmlNode();
            xmlNodeAnzeige.Name = "bav03_AnzeigeID";
            xmlNodeAnzeige.Content = R["AccessionNumber"].ToString();
            xmlNodeAnzeige.xmlAttributs = Datensatznummer;

            dict.Add(xmlNodeAnzeige.Name, xmlNodeAnzeige);


            System.Collections.Generic.List<XmlAttribut> xmlAttribute_bav04_DatenlieferndeInstitution = new List<XmlAttribut>();
            XmlAttribut xmlAttribute_bav04_DatenlieferndeInstitution_gnd_id = new XmlAttribut();
            xmlAttribute_bav04_DatenlieferndeInstitution_gnd_id.Content = "2001684-0";
            xmlAttribute_bav04_DatenlieferndeInstitution_gnd_id.Name = "gnd-id";
            xmlAttribute_bav04_DatenlieferndeInstitution.Add(xmlAttribute_bav04_DatenlieferndeInstitution_gnd_id);

            XmlNode xmlNode_bav04_DatenlieferndeInstitution = new XmlNode();
            xmlNode_bav04_DatenlieferndeInstitution.Name = "bav04_DatenlieferndeInstitution";
            xmlNode_bav04_DatenlieferndeInstitution.Content = _bav04_DatenlieferndeInstitution;
            xmlNode_bav04_DatenlieferndeInstitution.xmlAttributs = xmlAttribute_bav04_DatenlieferndeInstitution;

            dict.Add(xmlNode_bav04_DatenlieferndeInstitution.Name, xmlNode_bav04_DatenlieferndeInstitution);


            System.Collections.Generic.List<XmlAttribut> xmlAttribute_bav05_BestandshaltendeInstitution = new List<XmlAttribut>();
            XmlAttribut xmlAttribute_bav05_BestandshaltendeInstitution_gnd_id = new XmlAttribut();
            xmlAttribute_bav05_BestandshaltendeInstitution_gnd_id.Content = "2005856-1";
            xmlAttribute_bav05_BestandshaltendeInstitution_gnd_id.Name = "gnd-id";
            xmlAttribute_bav05_BestandshaltendeInstitution.Add(xmlAttribute_bav05_BestandshaltendeInstitution_gnd_id);

            XmlNode xmlNode_bav05_BestandshaltendeInstitution = new XmlNode();
            xmlNode_bav05_BestandshaltendeInstitution.Name = "bav05_BestandshaltendeInstitution";
            xmlNode_bav05_BestandshaltendeInstitution.Content = _bav05_BestandshaltendeInstitution; // "SNSB - Botanische Staatssammlung München";
            xmlNode_bav05_BestandshaltendeInstitution.xmlAttributs = xmlAttribute_bav05_BestandshaltendeInstitution;

            dict.Add(xmlNode_bav05_BestandshaltendeInstitution.Name, xmlNode_bav05_BestandshaltendeInstitution);

            string Titel = R["TaxonomicName"].ToString();
            if (!R["VernacularTerm"].Equals(System.DBNull.Value) && R["VernacularTerm"].ToString().Length > 0)
                Titel += " (" + R["VernacularTerm"].ToString() + ")";
            Titel += " - " + Objekttyp;
            dict.Add("bav07_Titel_Name_Objektbezeichnung", Titel);

            if (!R["VernacularTerm"].Equals(System.DBNull.Value) && R["VernacularTerm"].ToString().Length > 0)
                dict.Add("bav08_Alternativtitel", R["VernacularTerm"].ToString());

            foreach(string O in bav10_Objektkategorie)
            {
                XmlNode xmlNode = new XmlNode();
                xmlNode.Name = "bav10_Objektkategorie";
                xmlNode.Content = O;
                dict.Add("bav10_Objektkategorie_" + O, xmlNode);
            }

            System.Collections.Generic.List<XmlAttribut> xmlAttribute_bav11_Schlagwort_Thema = new List<XmlAttribut>();
            XmlAttribut xmlAttribute_bav11_Schlagwort_Thema_typ = new XmlAttribut();
            xmlAttribute_bav11_Schlagwort_Thema_typ.Content = "Sachbegriff";
            xmlAttribute_bav11_Schlagwort_Thema_typ.Name = "typ";
            xmlAttribute_bav11_Schlagwort_Thema.Add(xmlAttribute_bav11_Schlagwort_Thema_typ);

            foreach (string O in bav11_Schlagwort)
            {
                XmlNode xmlNode = new XmlNode();
                xmlNode.Name = "bav11_Schlagwort_Thema";
                xmlNode.Content = O;
                xmlNode.xmlAttributs = xmlAttribute_bav11_Schlagwort_Thema;
                dict.Add("bav11_Schlagwort_Thema" + O, xmlNode);
            }


            System.Collections.Generic.List<XmlAttribut> xmlAttribute_Hauptverantwortlichkeit = new List<XmlAttribut>();
            XmlAttribut xmlAttribute_Hauptverantwortlichkeit_typ = new XmlAttribut();
            xmlAttribute_Hauptverantwortlichkeit_typ.Content = "Person";
            xmlAttribute_Hauptverantwortlichkeit_typ.Name = "typ";
            xmlAttribute_Hauptverantwortlichkeit.Add(xmlAttribute_Hauptverantwortlichkeit_typ);
            XmlAttribut xmlAttribute_Hauptverantwortlichkeit_gnd_id = new XmlAttribut();
            xmlAttribute_Hauptverantwortlichkeit_gnd_id.Content = this.GND_ID;
            xmlAttribute_Hauptverantwortlichkeit_gnd_id.Name = "gnd-id";
            xmlAttribute_Hauptverantwortlichkeit.Add(xmlAttribute_Hauptverantwortlichkeit_gnd_id);

            XmlNode xmlNode_Hauptverantwortlichkeit = new XmlNode();
            xmlNode_Hauptverantwortlichkeit.Name = "Hauptverantwortlichkeit";
            xmlNode_Hauptverantwortlichkeit.Content = Hauptverantwortlichkeit;
            xmlNode_Hauptverantwortlichkeit.xmlAttributs = xmlAttribute_Hauptverantwortlichkeit;


            System.Collections.Generic.List<XmlAttribut> xmlAttribute_Entstehung = new List<XmlAttribut>();
            XmlAttribut xmlAttribute_Entstehung_typ = new XmlAttribut();
            xmlAttribute_Entstehung_typ.Content = "Entstehung";
            xmlAttribute_Entstehung_typ.Name = "typ";
            xmlAttribute_Entstehung.Add(xmlAttribute_Entstehung_typ);

            XmlNode xmlNode_bav12_Ereignis_Hauptverantwortlichkeit = new XmlNode();
            xmlNode_bav12_Ereignis_Hauptverantwortlichkeit.Name = "bav12_Ereignis";
            xmlNode_bav12_Ereignis_Hauptverantwortlichkeit.Content = xmlNode_Hauptverantwortlichkeit;
            xmlNode_bav12_Ereignis_Hauptverantwortlichkeit.xmlAttributs = xmlAttribute_Entstehung;

            dict.Add("bav12_Ereignis_Hauptverantwortlichkeit", xmlNode_bav12_Ereignis_Hauptverantwortlichkeit);

            XmlNode xmlNode_Ort = new XmlNode();
            xmlNode_Ort.Name = "Ort";
            string Error = "";
            xmlNode_Ort.Content = this.TranslateCountry(R["NamedArea"].ToString(), R["LinkDG"].ToString(), R["CountryCache"].ToString(), ref Error);
            if (Error.Length > 0) _ExportExceptions += R["AccessionNumber"].ToString() + ": " + Error + "\r\n";

            System.Collections.Generic.List<XmlAttribut> xmlAttribute_Entstehung_Ort = new List<XmlAttribut>();
            XmlAttribut xmlAttribute_Entstehung_Ort_typ = new XmlAttribut();
            xmlAttribute_Entstehung_Ort_typ.Content = "Entstehung";
            xmlAttribute_Entstehung_Ort_typ.Name = "typ";
            xmlAttribute_Entstehung_Ort.Add(xmlAttribute_Entstehung_Ort_typ);

            XmlNode xmlNode_bav12_Ereignis_Ort = new XmlNode();
            xmlNode_bav12_Ereignis_Ort.Name = "bav12_Ereignis";
            xmlNode_bav12_Ereignis_Ort.Content = xmlNode_Ort;
            xmlNode_bav12_Ereignis_Ort.xmlAttributs = xmlAttribute_Entstehung_Ort;

            dict.Add("bav12_Ereignis_Ort", xmlNode_bav12_Ereignis_Ort);

            // Zeit
            XmlNode xmlNode_Zeit = new XmlNode();
            xmlNode_Zeit.Name = "Zeit";
            xmlNode_Zeit.Content = this.GetDatum(R);

            System.Collections.Generic.List<XmlAttribut> xmlAttribute_Entstehung_Zeit = new List<XmlAttribut>();
            XmlAttribut xmlAttribute_Entstehung_Zeit_typ = new XmlAttribut();
            xmlAttribute_Entstehung_Zeit_typ.Content = "Entstehung";
            xmlAttribute_Entstehung_Zeit_typ.Name = "typ";
            xmlAttribute_Entstehung_Zeit.Add(xmlAttribute_Entstehung_Zeit_typ);

            XmlNode xmlNode_bav12_Ereignis_Zeit = new XmlNode();
            xmlNode_bav12_Ereignis_Zeit.Name = "bav12_Ereignis";
            xmlNode_bav12_Ereignis_Zeit.Content = xmlNode_Zeit;
            xmlNode_bav12_Ereignis_Zeit.xmlAttributs = xmlAttribute_Entstehung_Zeit;

            dict.Add("bav12_Ereignis_Zeit", xmlNode_bav12_Ereignis_Zeit);

            dict.Add("bav13_Sprache", this._bav13_Sprache);
            dict.Add("bav14_Material", this.Material); // "Aquarelltechnik auf Papier");
            dict.Add("bav17_RechtedeklarationMetadaten", "CC0");
            //dict.Add("bav18_RechtedeklarationBeschreibungstext", "CC BY 4.0");
            dict.Add("bav19_RechtedeklarationDigitalisat", "CC BY-SA 4.0");

            dict.Add("bav20_Permalink_DigitalesObjekt", R["Permalink"].ToString());

            dict.Add("bav21_Dateinamen_Bilddateien", R["AccessionNumber"].ToString() + ".tif");

            /*
                                               
                <bav12_Ereignis typ="Entstehung">
                    <Hauptverantwortlichkeit typ="Person" gnd-id="1129113183">Dr. Fritz Wohlfarth</Hauptverantwortlichkeit>
                </bav12_Ereignis>
                <bav12_Ereignis typ="Entstehung">
                    <Ort>Polsdorf, Stadt-/Gemeindeteil v. Allersberg, Kreis Roth, Bayern, Germany. In sandigem Kiefernwald sehr stark mit Moos verwachsen. Polsdorf bei Hilpoltstein</Ort>
                </bav12_Ereignis>
                <bav12_Ereignis typ="Entstehung">
                    <Zeit>28.11.1968</Zeit>
                </bav12_Ereignis>
                <bav13_Sprache>Deutsch</bav13_Sprache>
                <bav14_Material>Aquarelltechnik auf Papier</bav14_Material>
                <bav15_Umfang_Abmessungen_Laufzeit>30 x 40 cm (Breite x Höhe))</bav15_Umfang_Abmessungen_Laufzeit>
                <bav16_Bemerkung>Handschiftliche Notiz auf dem Aquarell: [painted] Braunweiler, 28.11.1968   Fleisch schwarz, ledrig zäh</bav16_Bemerkung>
                <bav17_RechtedeklarationMetadaten>CC0</bav17_RechtedeklarationMetadaten>
                <bav18_RechtedeklarationBeschreibungstext>CC BY 4.0</bav17_RechtedeklarationMetadaten>
                <bav19_RechtedeklarationDigitalisat>CC BY-SA 4.0</bav19_RechtedeklarationDigitalisat>
                <bav21_Dateinamen_Bilddateien>M-0031349.tif</bav21_Dateinamen_Bilddateien>
            */
            return dict;
        }

        private enum DatumsTyp { fehlt, Jahr, Datum}

        private enum DatumsQuelle { Fund, Notes, Alle }

        private DatumsQuelle datumsQuelle
        {
            get
            {
                if (this.radioButtonDatumFund.Checked)
                    return DatumsQuelle.Fund;
                if (this.radioButtonDatumNotes.Checked)
                    return DatumsQuelle.Notes;
                return DatumsQuelle.Alle;
            }
        }

        private string GetDatum(System.Data.DataRow R)
        {
            DatumsTyp DatumFund = DatumsTyp.fehlt;
            DatumsTyp DatumNotes = DatumsTyp.fehlt;
            string Datum = R["Datum"].ToString();
            if (datumsQuelle == DatumsQuelle.Fund)
                return Datum;
            else if (datumsQuelle == DatumsQuelle.Notes)
                Datum = "";
            try
            {
                System.DateTime FundDatum;
                int? FundJahr = null;
                System.DateTime NotesDatum = new DateTime(1500, 1, 1);
                int? NotesJahr = null;
                System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CreateSpecificCulture("de-DE");
                System.Globalization.DateTimeStyles styles = System.Globalization.DateTimeStyles.None;
                if (!System.DateTime.TryParse(Datum, culture, styles, out FundDatum))
                {
                    //DatumFund = DatumsTyp.fehlt;
                    if (Datum.Length > 0)
                    {
                        FundJahr = int.Parse(Datum);
                        FundDatum = new DateTime((int)FundJahr, 1, 1);
                        DatumFund = DatumsTyp.Jahr;
                    }
                    //else
                    //{
                    //    FundDatum = System.DateTime.Now;
                    //    DatumFund = DatumsTyp.fehlt;
                    //}
                }
                else
                    DatumFund = DatumsTyp.Datum;

                if (!R["OriginalNotes"].Equals(System.DBNull.Value) && R["OriginalNotes"].ToString().Length > 0)
                {
                    string[] OriginalNotes = R["OriginalNotes"].ToString().Split(new char[] { ' ' });
                    for (int i = 0; i < OriginalNotes.Length; i++)
                    {
                        int Year;
                        if (int.TryParse(OriginalNotes[i], out Year) && Year > 1700 && Year <= System.DateTime.Now.Year)
                        {
                            NotesJahr = Year;
                            DatumNotes = DatumsTyp.Jahr;
                            int Month;
                            int Day;
                            if (i > 1 && int.TryParse(OriginalNotes[i-1].Replace(".", ""), out Month) && int.TryParse(OriginalNotes[i - 2].Replace(".", ""), out Day))
                            {
                                NotesDatum = new DateTime(Year, Month, Day);
                                DatumNotes = DatumsTyp.Datum;
                            }
                            //Datum = Year.ToString();
                        }
                        else
                        {
                            string[] OriNotesDatum = OriginalNotes[i].Split(new char[] { '.', '/', '-' });
                            int Jahr; int Monat; int Tag;
                            if (OriNotesDatum.Length == 3 && 
                                int.TryParse(OriNotesDatum[0], out Tag) && Tag > 0 && Tag < 32 && 
                                int.TryParse(OriNotesDatum[1], out Monat) && Monat > 0 && Monat < 13 && 
                                int.TryParse(OriNotesDatum[2], out Jahr) && Jahr > 1500 && Jahr <= System.DateTime.Now.Year  && 
                                System.DateTime.TryParse(OriginalNotes[i], culture, styles, out NotesDatum))
                            {
                                DatumNotes = DatumsTyp.Datum;
                            }
                        }
                    }
                }
                switch(DatumFund)
                {
                    case DatumsTyp.fehlt:
                        switch(DatumNotes)
                        {
                            case DatumsTyp.fehlt:
                                break;
                            case DatumsTyp.Jahr:
                                if (NotesJahr != null)
                                    Datum = NotesJahr.ToString();
                                break;
                            case DatumsTyp.Datum:
                                Datum = NotesDatum.ToString("dd.MM.yyyy");
                                break;
                        }
                        break;
                    case DatumsTyp.Jahr:
                        switch (DatumNotes)
                        {
                            case DatumsTyp.fehlt:
                                break;
                            case DatumsTyp.Jahr:
                                if (FundJahr != null && NotesJahr != null && (int)NotesJahr > (int)FundJahr)
                                    Datum = NotesJahr.ToString();
                                break;
                            case DatumsTyp.Datum:
                                if (FundJahr != null && NotesDatum.Year >= (int)FundJahr)
                                    Datum = NotesDatum.ToString("dd.MM.yyyy");
                                break;
                        }
                        break;
                    case DatumsTyp.Datum:
                        switch (DatumNotes)
                        {
                            case DatumsTyp.fehlt:
                                break;
                            case DatumsTyp.Jahr:
                                if (NotesJahr != null && (int)NotesJahr > FundDatum.Year)
                                    Datum = NotesJahr.ToString();
                                break;
                            case DatumsTyp.Datum:
                                if (NotesDatum > FundDatum)
                                    Datum = NotesDatum.ToString("dd.MM.yyyy");
                                break;
                        }
                        break;
                }
                //if (NotesDatum > FundDatum) // Datum in beiden Feldern und Notes datum später
                //{
                //    Datum = NotesDatum.ToString("dd.MM.yyyy");
                //}
                //else if (FundJahr != null && NotesJahr != null && (int)NotesJahr > (int)FundJahr)
                //    Datum = NotesJahr.ToString();
                //else if ((NurJahr || Datum.Length == 0 || !MitFunddatum) && NotesDatum.Year > 1500)
                //    Datum = NotesDatum.ToString("dd.MM.yyyy");
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return Datum;
        }

        #endregion

    }
}
