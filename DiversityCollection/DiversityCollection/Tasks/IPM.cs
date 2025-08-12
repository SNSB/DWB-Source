using DiversityCollection.Tasks.Taxa;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.Tasks
{

    #region Structs
    public enum ResourceType {Preview, Image, Info }
    public enum State { Reading, Editing }

    public struct Resource
    {
        public int NameID;
        public System.Uri Uri;
        public int DisplayOrder;
        public ResourceType Type;
        public System.Drawing.Image Icon;
        public string Title;
        public string CopyRight;
        public string Notes;
    }

    public struct Preview
    {
        public int NameID;
        public int StageID;
        public string URI;
        public System.Drawing.Image Icon;
    }

    public struct RecordData
    {
        public double? Count;
        public string State;
        public string Notes;
    }

    //public struct Pest
    //{
    //    public int NameID;
    //    public string BaseURL;
    //    public string ScientificName;
    //    public string VernacularName;
    //    public bool AcceptedName;
    //    public string Group;
    //    public System.Collections.Generic.Dictionary<int, Resource> Icones;
    //    public System.Collections.Generic.Dictionary<int, Resource> Images;
    //    public System.Collections.Generic.Dictionary<int, Resource> Infos;
    //    public string DisplayText()
    //    {
    //        string Name = VernacularName;
    //        if (ScientificName.Length > 0 && Name.IndexOf("(") == -1)
    //            Name += "\r\n(" + ScientificName + ")";
    //        return  Name;
    //    }
    //    public string NameURI()
    //    {
    //        return BaseURL + NameID.ToString(); 
    //    }
    //}

    public struct Metric
    {
        public int CollectionTaskID;
        public string Unit;
        public string Description;
        public string DisplayText()
        {
            string Display = Description;
            if (Unit != null && Unit.Length > 0)
                Display = Unit + " " + Display;
            return Display;
        }
    }

    #endregion

    public partial class IPM : Component
    {

        #region Construction

        public IPM()
        {
            InitializeComponent();
        }

        public IPM(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region INIT - set all contents needed in the database

        // Testing if all needed content for IPM is available
        public static bool AnyTaxaSelected()
        {
            bool Anything = false;
            if (Settings.Default.PestNameUris == null || Settings.Default.PestNameUris.Count == 0)
            {
                DiversityCollection.Tasks.FormSettings form = new FormSettings("So far no pest taxa have been selected.\r\nPlease select any pests from the list");
                form.ShowDialog();
                Anything = Settings.Default.PestNameUris != null && Settings.Default.PestNameUris.Count > 0;
            }

            else Anything = true;
            return Anything;
        }

        public static bool AnyTrapsDefined()
        {
            bool Anything = DataTableTraps().Rows.Count > 0;
            if (!Anything)
            {
                // Get name of the current top collection
                string TopColl = DiversityCollection.LookupTable.CollectionName(Settings.Default.TopCollectionID);

                System.Windows.Forms.MessageBox.Show("So far no traps have been defined within the current collection\r\n" + TopColl + "\r\nPlease select a collection containing traps or define some traps within the collection", "No traps", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                DiversityCollection.Forms.FormCollection f = new Forms.FormCollection();// Settings.Default.TopCollectionID);
                f.ShowDialogPanel = true;
                f.CurrentCollectionID = Settings.Default.TopCollectionID;
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK &&
                    f.ID > -1)
                {
                    Settings.Default.TopCollectionID = f.ID;
                }
                _CollectionIDList = null;
                Anything = DataTableTraps().Rows.Count > 0;
            }
            return Anything;
        }

        public static System.Data.DataTable DataTableTraps()
        {
            string SQL = "SELECT A.DisplayText, A.[CollectionID] " +
                "FROM dbo.CollectionLocationAll() A WHERE A.CollectionID IN (" + CollectionIDList() + ") AND A.Type = 'Trap' " +
                "ORDER BY A.DisplayText";
            System.Data.DataTable dataTable = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTable);
            return dataTable;
        }




        #endregion

        #region Handling controls

        public static int initTaxaGridViewRows(IPM.RecordingTarget recordingTarget, System.Windows.Forms.DataGridView dataGridView, bool ForSettings = false)
        {
            dataGridView.SuspendLayout();
            int count = 0;
            try
            {

                dataGridView.RowHeadersVisible = false;
                dataGridView.AllowUserToAddRows = false;
                dataGridView.AllowUserToDeleteRows = false;

                dataGridView.Rows.Clear();
                int i = RecordDicts.ChecklistRecordings(recordingTarget).Count;
                if (!ForSettings)
                {
                    switch (recordingTarget)
                    {
                        case RecordingTarget.Beneficial:
                            i = Settings.Default.BeneficialNameUris.Count;
                            break;
                        case RecordingTarget.TrapBycatch:
                            i = Settings.Default.BycatchNameUris.Count;
                            break;
                        default:
                            i = Settings.Default.PestNameUris.Count;
                            break;
                    }
                }
                count = i;
                if (i > 0)
                {
                    dataGridView.Rows.Add(i);
                    System.Windows.Forms.DataGridViewCellStyle s = dataGridView.DefaultCellStyle;
                    s.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                    dataGridView.Columns[0].DefaultCellStyle = s;
                    //dataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
                    System.Collections.Generic.Dictionary<string, TaxonRecord> records = RecordDicts.ChecklistRecordings(recordingTarget); // Tasks.Taxa.List.TaxonStageDict(source);// new Dictionary<string, Taxon>();
                    var recordsSorted = records.OrderBy(r => r.Value.Sorting);
                    i = 0;
                    foreach (System.Collections.Generic.KeyValuePair<string, TaxonRecord> keyValue in recordsSorted)
                    {
                        if (!ForSettings)
                        {
                            switch (recordingTarget)
                            {
                                case RecordingTarget.Beneficial:
                                    if (!Settings.Default.BeneficialNameUris.Contains(keyValue.Key)) continue;
                                    break;
                                case RecordingTarget.TrapBycatch:
                                    if (!Settings.Default.BycatchNameUris.Contains(keyValue.Key)) continue;
                                    break;
                                default:
                                    if (!Settings.Default.PestNameUris.Contains(keyValue.Key)) continue;
                                    break;
                            }
                        }


                        if (dataGridView.Rows.Count < i)
                            break;
                        try
                        {
                            dataGridView.Rows[i].Height = 50;
                            dataGridView.Rows[i].Cells[0].Value = keyValue.Value.Group;
                            dataGridView.Rows[i].Cells[1].Value = keyValue.Value.DisplayText;
                            if (keyValue.Value.PreviewImage.Icon != null && dataGridView.Rows[i].Cells.Count > 1)
                            {
                                dataGridView.Rows[i].Cells[2].Value = keyValue.Value.PreviewImage.Icon;
                            }
                            dataGridView.Rows[i].Tag = keyValue.Key;

                            if (ForSettings && dataGridView.Rows[i].Cells.Count > 3)
                            {
                                switch (recordingTarget)
                                {
                                    case IPM.RecordingTarget.TrapBycatch:
                                        dataGridView.Rows[i].Cells[3].Value = Settings.Default.BycatchNameUris.Contains(keyValue.Key);
                                        break;
                                    case IPM.RecordingTarget.Beneficial:
                                        dataGridView.Rows[i].Cells[3].Value = Settings.Default.BeneficialNameUris.Contains(keyValue.Key);
                                        break;
                                    default:
                                        dataGridView.Rows[i].Cells[3].Value = Settings.Default.PestNameUris.Contains(keyValue.Key);
                                        break;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                        i++;
                    }

                    for (int c = 2; c < dataGridView.Columns.Count; c++)
                    {
                        dataGridView.Columns[c].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
                        dataGridView.AutoResizeColumn(c, System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells);
                    }
                }
                //dataGridView.AutoResizeColumns(System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            dataGridView.ResumeLayout();
            return count;
        }



        #endregion

        #region Interface
        public static int ListID(IPM.TaxonSource taxonSource)
        {
            int listID = 1190;
            switch (taxonSource)
            {
                case IPM.TaxonSource.Pest:
                    listID = Settings.Default.DiversityTaxonNamesPestListID;
                    break;
                case IPM.TaxonSource.Beneficial:
                    listID = Settings.Default.DiversityTaxonNamesBeneficialListID;
                    break;
                case IPM.TaxonSource.Bycatch:
                    listID = Settings.Default.DiversityTaxonNamesBycatchListID;
                    break;
            }
            return listID;
        }

        public static int ListID(IPM.RecordingTarget recordingTarget)
        {
            int listID = 1190;
            switch (recordingTarget)
            {
                case IPM.RecordingTarget.TrapPest:
                    listID = Settings.Default.DiversityTaxonNamesPestListID;
                    break;
                case IPM.RecordingTarget.Beneficial:
                    listID = Settings.Default.DiversityTaxonNamesBeneficialListID;
                    break;
                case IPM.RecordingTarget.TrapBycatch:
                    listID = Settings.Default.DiversityTaxonNamesBycatchListID;
                    break;
            }
            return listID;
        }

        #endregion

        #region Taxon groups from DTN

        public enum IpmGroup { Pest, Beneficial, Bycatch, Item }

        public static string TaxonDBPrefix { get { return Settings.Default.DiversityTaxonNamesDatabase + ".dbo."; } }
        public static string TaxonDBBaseURL
        {
            get
            {
                if (_TaxonDBBaseURL == null)
                {
                    string SQL = "SELECT BaseURL FROM " + TaxonDBPrefix + "ViewBaseURL";
                    _TaxonDBBaseURL = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                }
                return _TaxonDBBaseURL;
            }
        }
        private static string _TaxonDBBaseURL;


        //private static System.Collections.Generic.Dictionary<string, TaxonGroup> _PestGroups;
        //private static System.Collections.Generic.Dictionary<string, TaxonGroup> _BeneficialGroups;
        //private static System.Collections.Generic.Dictionary<string, TaxonGroup> _BycatchGroups;
        //private static System.Collections.Generic.Dictionary<string, TaxonGroup> _ItemGroups;

        //public static System.Collections.Generic.Dictionary<string, TaxonGroup> GetGroups()
        //{
        //    return GetGroups(TaxonSource.Pest);
        //}


        //private static System.Collections.Generic.Dictionary<string, TaxonGroup> GetGroups(TaxonSource source)
        //{
        //    System.Collections.Generic.Dictionary<string, TaxonGroup> dict = new Dictionary<string, TaxonGroup>();
        //    int ListID = 0;
        //    switch (source)
        //    {
        //        case TaxonSource.Pest:
        //            ListID = Settings.Default.DiversityTaxonNamesPestListID;
        //            break;
        //        case TaxonSource.Beneficial:
        //            ListID = Settings.Default.DiversityTaxonNamesBeneficialListID;
        //            break;
        //        case TaxonSource.Bycatch:
        //            ListID = Settings.Default.DiversityTaxonNamesBycatchListID;
        //            break;
        //    }
        //    System.Collections.Generic.Dictionary<int, string> Groups = GroupList(ListID);
        //    foreach(System.Collections.Generic.KeyValuePair<int, string> KV in Groups)
        //    {

        //    }
        //    return dict;
        //}

        //private static System.Collections.Generic.Dictionary<int, string> GroupList(int ListID)
        //{
        //    System.Collections.Generic.Dictionary<int, string> Groups = new Dictionary<int, string>();
        //    string SQL = "SELECT T.NameID, MIN(N.CommonName) " +
        //        " FROM TaxonName T " +
        //        " INNER JOIN TaxonCommonName N ON T.NameID = N.NameID AND N.LanguageCode = '" + DiversityWorkbench.Settings.Language.Substring(0, 2) + "' " +
        //        " INNER JOIN TaxonNameListAnalysis A ON T.NameID = A.NameID AND A.ProjectID = " + ListID.ToString() +
        //        " INNER JOIN TaxonNameListAnalysisCategory C ON A.AnalysisID = C.AnalysisID AND C.DisplayText = 'IPM' " +
        //        " GROUP BY T.NameID";
        //    return Groups;
        //}

        #endregion

        #region Taxa from DTN

        private string Prefix() { return Settings.Default.DiversityTaxonNamesDatabase + ".dbo."; }

        #region Pests

        private static System.Collections.Generic.Dictionary<string, Taxon> _Pests;

        private System.Collections.Generic.Dictionary<string, Taxon> _PestTaxa;
        //public System.Collections.Generic.Dictionary<string, Taxon> GetPestTaxa(string TopLine = " ")
        //{
        //    if (_PestTaxa == null)
        //    {

        //        _PestTaxa = this.GetTaxa(TopLine, TaxonSource.Pest); // new Dictionary<string, Pest>();
        //        _Pests = _PestTaxa;
        //    }
        //    return _PestTaxa;
        //}

        #endregion

        #region Beneficials

        private static System.Collections.Generic.Dictionary<string, Taxon> _Beneficials;

        private System.Collections.Generic.Dictionary<string, Taxon> _BeneficialTaxa;
        //public System.Collections.Generic.Dictionary<string, Taxon> GetBeneficialTaxa(string TopLine = " ")
        //{
        //    if (_BeneficialTaxa == null)
        //    {
        //        _BeneficialTaxa = this.GetTaxa(TopLine, TaxonSource.Beneficial); // new Dictionary<string, Pest>();
        //        _Beneficials = _BeneficialTaxa;
        //    }
        //    return _BeneficialTaxa;
        //}

        #endregion

        #region Bycatch

        private System.Collections.Generic.Dictionary<string, Tasks.Taxa.TaxonRecord> _BycatchRecords;

        //public System.Collections.Generic.Dictionary<string, Tasks.Taxa.Record> GetBycatchRecords()
        //{
        //    if (_BycatchRecords == null)
        //    {
        //        _BycatchRecords = RecordDicts.ChecklistRecords(TaxonSource.Bycatch);
        //    }
        //    return _BycatchRecords;
        //}


        //private static System.Collections.Generic.Dictionary<string, Taxon> _Bycatch;

        //private System.Collections.Generic.Dictionary<string, Taxon> _BycatchTaxa;
        //public System.Collections.Generic.Dictionary<string, Taxon> GetBycatchTaxa(string TopLine = " ")
        //{
        //    if (_BycatchTaxa == null)
        //    {
        //        _BycatchTaxa = this.GetTaxa(TopLine, TaxonSource.Bycatch); // new Dictionary<string, Pest>();
        //        _Bycatch = _BycatchTaxa;
        //    }
        //    return _BycatchTaxa;
        //}

        //public static System.Collections.Generic.Dictionary<string, Taxon> BycatchTaxa()
        //{
        //    if (_Bycatch == null)
        //        _Bycatch = new Dictionary<string, Taxon>();
        //    return _Bycatch;
        //}

        //public static System.Collections.Generic.Dictionary<string, bool> BycatchSelected()
        //{
        //    return TaxaSelected(TaxonSource.Bycatch);
        //}


        private static System.Collections.Generic.Dictionary<string, bool> TaxaSelected(IPM.TaxonSource taxonSource)
        {
            System.Collections.Generic.Dictionary<string, bool> Taxa = new Dictionary<string, bool>();
            string Restriction = "";
            if (_ForCollection)
                Restriction = "T.CollectionID IN (" + CollectionIDList(_ID) + ") ";
            else
                Restriction = "T.CollectionTaskID = " + _ID.ToString();
            if (_Start != null)
            {
                DateTime _start = (DateTime)_Start;
                Restriction += "AND (T.[TaskStart] >= CONVERT(DATETIME, '" + _start.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.[TaskStart] IS NULL) AND (T.TaskEnd >= CONVERT(DATETIME, '" + _start.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.TaskEnd IS NULL) ";
            }
            if (_End != null)
            {
                DateTime _end = (DateTime)_End;
                Restriction += "AND (T.[TaskStart] <= CONVERT(DATETIME, '" + _end.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.[TaskStart] IS NULL) AND (T.TaskEnd <= CONVERT(DATETIME, '" + _end.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.TaskEnd IS NULL) ";
            }
            string Uris = "";
            System.Collections.Specialized.StringCollection NameUris = IPM.NameUris(taxonSource); // new System.Collections.Specialized.StringCollection();
            //switch (taxonSource)
            //{
            //    case TaxonSource.Bycatch:
            //        NameUris = DiversityCollection.Tasks.Settings.Default.BycatchNameUris;
            //        break;
            //    default:
            //        NameUris = DiversityCollection.Tasks.Settings.Default.PestNameUris;
            //        break;
            //}
            if (NameUris.Count > 0)
            {
                foreach (string U in NameUris)
                {
                    if (Uris.Length > 0)
                        Uris += ", ";
                    Uris += "'" + U + "'";
                }
                Restriction += " AND T.ModuleUri IN (" + Uris + ") ";
            }
            string SQL = "SELECT DISTINCT T.DisplayText, T.ModuleUri " +
                " FROM CollectionTask AS T INNER JOIN Task Y ON T.TaskID = Y.TaskID  WHERE Y.Type = 'pest' AND " + Restriction +
                " AND T.DisplayText <> '' AND (T.DisplayOrder > 0) " +
                " ORDER BY T.DisplayText, T.ModuleUri";
            System.Data.DataTable dataTableTaxa = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTableTaxa);
            int ii = 0;
            for (int i = 0; i < dataTableTaxa.Rows.Count; i++)
            {
                if (_Pests.ContainsKey(dataTableTaxa.Rows[i][1].ToString()))
                {
                    Taxa.Add(_Pests[dataTableTaxa.Rows[i][1].ToString()].VernacularName, true);
                }
                ii++;
            }
            return Taxa;
        }

        #endregion

        private string _BaseURL;
        private string BaseURL()
        {
            if (_BaseURL == null)
            {
                try
                {
                    string SQL = "SELECT [BaseURL]  FROM " + Prefix() + "[ViewBaseURL]";
                    _BaseURL = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            return _BaseURL;
        }

        public enum RecordingTarget { Beneficial, TrapBycatch, TrapPest, CollectionPest, SpecimenPest, TransactionPest }

        public enum MonitoringTarget { Trap, Collection, Specimen, Transaction }

        public enum TaxonSource { Pest, Beneficial, Bycatch }
        //public static TaxonSource taxonSource(int ListID)
        //{
        //    if (ListID == DiversityCollection.Tasks.Settings.Default.DiversityTaxonNamesBeneficialListID) return TaxonSource.Beneficial;
        //    else if (ListID == DiversityCollection.Tasks.Settings.Default.DiversityTaxonNamesBycatchListID) return TaxonSource.Bycatch;
        //    else return TaxonSource.Pest;
        //}

        //public static int ChecklistID(TaxonSource taxonSource)
        //{
        //    switch (taxonSource)
        //    {
        //        case TaxonSource.Pest: return DiversityCollection.Tasks.Settings.Default.DiversityTaxonNamesPestListID;
        //        case TaxonSource.Bycatch: return DiversityCollection.Tasks.Settings.Default.DiversityTaxonNamesBycatchListID;
        //        case TaxonSource.Beneficial: return DiversityCollection.Tasks.Settings.Default.DiversityTaxonNamesBeneficialListID;
        //    }
        //    return 0;
        //}


        public static int ChecklistID(RecordingTarget recordingTarget)
        {
            switch (recordingTarget)
            {
                case RecordingTarget.TrapBycatch: return DiversityCollection.Tasks.Settings.Default.DiversityTaxonNamesBycatchListID;
                case RecordingTarget.Beneficial: return DiversityCollection.Tasks.Settings.Default.DiversityTaxonNamesBeneficialListID;
                default: return DiversityCollection.Tasks.Settings.Default.DiversityTaxonNamesPestListID;
            }
            return 0;
        }

        public static string RecordingTaskType(RecordingTarget recording)
        {
            switch (recording)
            {
                case RecordingTarget.TrapBycatch: return "Bycatch";
                case RecordingTarget.Beneficial: return "Beneficial organism";
                default: return "Pest";
            }
        }

        private static System.Collections.Generic.Dictionary<TaxonSource, int> _TaxonSources;
        public static System.Collections.Generic.Dictionary<TaxonSource, int> TaxonSources
        {
            get
            {
                if (_TaxonSources == null)
                {
                    _TaxonSources = new Dictionary<TaxonSource, int>();
                    foreach (int i in Tasks.Taxa.List.IpmListIDs)
                    {
                        if (!_TaxonSources.ContainsKey(Tasks.Taxa.List.TaxonSource(i)))
                            _TaxonSources.Add(Tasks.Taxa.List.TaxonSource(i), i);
                    }
                }
                return _TaxonSources;
            }
        }


        //private System.Collections.Generic.Dictionary<string, Taxa.TaxonStage> GetTaxonList(TaxonSource Source)
        //{
        //    System.Collections.Generic.Dictionary<string, Taxa.TaxonStage> Taxa = Tasks.Taxa.List.TaxonStageDict(Source);
        //    return Taxa;
        //}

        //private System.Collections.Generic.Dictionary<int, DiversityWorkbench.Api.Taxon.Taxon> GetChecklistTaxa(string TopLine = " ", TaxonSource Source = TaxonSource.Pest)
        //{
        //    int listID = ListID(Source);
        //    int ProjectID = Settings.Default.DiversityTaxonNamesProjectID;
        //    System.Collections.Generic.Dictionary<int, DiversityWorkbench.Api.Taxon.Taxon> Taxa = DiversityWorkbench.Api.Taxon.Taxa.ChecklistTaxa(Settings.Default.DiversityTaxonNamesDatabase, listID, Settings.Default.DiversityTaxonNamesProjectID);
        //    return Taxa;



        //    //try
        //    //{
        //    //    string SQL = "SELECT T.NameID";
        //    //    if (Source != TaxonSource.Pest) SQL += ", T.TaxonNameCache AS Taxon , TC.CommonName AS Art, GC.CommonName AS Gruppe ";
        //    //    SQL += " FROM " + Prefix() + "TaxonName AS T " +
        //    //    "INNER JOIN " + Prefix() + "TaxonHierarchy AS H ON T.NameID = H.NameID " +
        //    //    "INNER JOIN " + Prefix() + "TaxonName AS G ON H.NameParentID = G.NameID ";
        //    //    if (Source != TaxonSource.Pest) SQL += "INNER JOIN " + Prefix() + "TaxonCommonName AS TC ON T.NameID = TC.NameID " +
        //    //        "INNER JOIN " + Prefix() + "TaxonCommonName AS GC ON G.NameID = GC.NameID ";
        //    //    SQL += "INNER JOIN " + Prefix() + "TaxonNameList AS L ON T.NameID = L.NameID AND L.ProjectID = ";
        //    //    switch (Source)
        //    //    {
        //    //        case TaxonSource.Pest:
        //    //            SQL += Settings.Default.DiversityTaxonNamesPestListID.ToString();
        //    //            break;
        //    //        case TaxonSource.Beneficial:
        //    //            SQL += Settings.Default.DiversityTaxonNamesBeneficialListID.ToString();
        //    //            break;
        //    //        case TaxonSource.Bycatch:
        //    //            SQL += Settings.Default.DiversityTaxonNamesBycatchListID.ToString();
        //    //            break;
        //    //    }
        //    //    SQL += " WHERE (H.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ") ";
        //    //    if (TopLine.Length > 0)
        //    //        SQL += "UNION SELECT -1";
        //    //    if (Source != TaxonSource.Pest && TopLine.Length > 0) SQL += ", '', '', '" + TopLine + "' ";
        //    //    if (Source != TaxonSource.Pest) SQL += "ORDER BY Gruppe, Art";
        //    //    System.Data.DataTable dt = new System.Data.DataTable();
        //    //    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);

        //    //    // Accepted Names
        //    //    System.Data.DataTable dtAccepted = new System.Data.DataTable();
        //    //    SQL = "SELECT T.NameID " +
        //    //        "FROM " + Prefix() + "TaxonName AS T " +
        //    //        "INNER JOIN " + Prefix() + "TaxonAcceptedName AS A ON T.NameID = A.NameID " +
        //    //        "INNER JOIN " + Prefix() + "TaxonNameList AS L ON T.NameID = L.NameID AND L.ProjectID = ";
        //    //    switch (Source)
        //    //    {
        //    //        case TaxonSource.Pest:
        //    //            SQL += Settings.Default.DiversityTaxonNamesPestListID.ToString();
        //    //            break;
        //    //        case TaxonSource.Beneficial:
        //    //            SQL += Settings.Default.DiversityTaxonNamesBeneficialListID.ToString();
        //    //            break;
        //    //        case TaxonSource.Bycatch:
        //    //            SQL += Settings.Default.DiversityTaxonNamesBycatchListID.ToString();
        //    //            break;
        //    //    }
        //    //    SQL += " WHERE (A.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ") ";
        //    //    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtAccepted);

        //    //    if (dt.Rows.Count == 0 && DiversityWorkbench.Settings.TimeoutDatabase < 10)
        //    //    {
        //    //        System.Windows.Forms.MessageBox.Show("The list for the taxa could not be retrieved because the timeout is set to " + DiversityWorkbench.Settings.TimeoutDatabase.ToString() + " sec.\n\rConsider increasing the timeout");
        //    //        int Timeout = DiversityWorkbench.Settings.TimeoutDatabase;
        //    //        DiversityWorkbench.Forms.FormGetInteger f = new DiversityWorkbench.Forms.FormGetInteger(Timeout, "Timeout", "Please enter the timeout for database queries in seconds (0 = no limit)");
        //    //        f.setIcon(DiversityCollection.Resource.Time);
        //    //        f.ShowDialog();
        //    //        if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.Integer != null && f.Integer > -1)
        //    //        {
        //    //            DiversityWorkbench.Settings.TimeoutDatabase = (int)f.Integer;
        //    //            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
        //    //        }
        //    //    }
        //    //    if (dt.Rows.Count > 0)
        //    //    {
        //    //        foreach (System.Data.DataRow R in dt.Rows)
        //    //        {
        //    //            if (R.Table.Columns.Contains("NameID") && Source == TaxonSource.Pest) // || ( R.Table.Columns.Contains("Art") && R.Table.Columns.Contains("Taxon") && R.Table.Columns.Contains("Gruppe"))))
        //    //            {
        //    //                System.Collections.Generic.Dictionary<string, Taxon> pests = GetTaxon(int.Parse(R["NameID"].ToString()), Source);//, R["Taxon"].ToString(), R["Art"].ToString(), R["Gruppe"].ToString());
        //    //                //pest.AcceptedName = (dtAccepted.Select("NameID = " + R["NameID"].ToString()).Length > 0);
        //    //                foreach (KeyValuePair<string, Taxon> pest in pests)
        //    //                {
        //    //                    if (!Taxa.ContainsKey(pest.Key))
        //    //                        Taxa.Add(pest.Key, pest.Value);
        //    //                    else { }
        //    //                }
        //    //            }
        //    //            else if (R.Table.Columns.Contains("NameID") && Source != TaxonSource.Pest && R.Table.Columns.Contains("Art") && R.Table.Columns.Contains("Taxon") && R.Table.Columns.Contains("Gruppe"))
        //    //            {
        //    //                System.Collections.Generic.Dictionary<string, Taxon> pests = GetTaxon(int.Parse(R["NameID"].ToString()), Source);//, R["Taxon"].ToString(), R["Art"].ToString(), R["Gruppe"].ToString());
        //    //                //pest.AcceptedName = (dtAccepted.Select("NameID = " + R["NameID"].ToString()).Length > 0);
        //    //                foreach (KeyValuePair<string, Taxon> pest in pests)
        //    //                {
        //    //                    if (!Taxa.ContainsKey(pest.Key))
        //    //                        Taxa.Add(pest.Key, pest.Value);
        //    //                    else { }
        //    //                }
        //    //            }
        //    //            else
        //    //            {
        //    //                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("private System.Collections.Generic.Dictionary<string, Pest> GetTaxa(string TopLine = , TaxonSource Source = TaxonSource.Pest)", "Adding Taxa", "Taxon could not be added due to not matching table columns", true, true);
        //    //            }
        //    //        }
        //    //    }
        //    //}
        //    //catch (System.Exception ex)
        //    //{
        //    //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    //}
        //    //return Taxa;
        //}

        //private System.Collections.Generic.Dictionary<string, Taxon> GetTaxa(string TopLine = " ", TaxonSource Source = TaxonSource.Pest)
        //{
        //    System.Collections.Generic.Dictionary<string, Taxon> Taxa = new Dictionary<string, Taxon>();
        //    try
        //    {
        //        string SQL = "SELECT T.NameID";
        //        if (Source != TaxonSource.Pest) SQL += ", T.TaxonNameCache AS Taxon , TC.CommonName AS Art, GC.CommonName AS Gruppe ";
        //        SQL += " FROM " + Prefix() + "TaxonName AS T " +
        //        "INNER JOIN " + Prefix() + "TaxonHierarchy AS H ON T.NameID = H.NameID " +
        //        "INNER JOIN " + Prefix() + "TaxonName AS G ON H.NameParentID = G.NameID ";
        //        if (Source != TaxonSource.Pest) SQL += "INNER JOIN " + Prefix() + "TaxonCommonName AS TC ON T.NameID = TC.NameID " +
        //            "INNER JOIN " + Prefix() + "TaxonCommonName AS GC ON G.NameID = GC.NameID ";
        //        SQL += "INNER JOIN " + Prefix() + "TaxonNameList AS L ON T.NameID = L.NameID AND L.ProjectID = ";
        //        switch (Source)
        //        {
        //            case TaxonSource.Pest:
        //                SQL += Settings.Default.DiversityTaxonNamesPestListID.ToString();
        //                break;
        //            case TaxonSource.Beneficial:
        //                SQL += Settings.Default.DiversityTaxonNamesBeneficialListID.ToString();
        //                break;
        //            case TaxonSource.Bycatch:
        //                SQL += Settings.Default.DiversityTaxonNamesBycatchListID.ToString();
        //                break;
        //        }
        //        SQL += " WHERE (H.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ") ";
        //        if (TopLine.Length > 0)
        //            SQL += "UNION SELECT -1";
        //        if (Source != TaxonSource.Pest && TopLine.Length > 0) SQL += ", '', '', '" + TopLine + "' ";
        //        if (Source != TaxonSource.Pest) SQL += "ORDER BY Gruppe, Art";
        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);

        //        // Accepted Names
        //        System.Data.DataTable dtAccepted = new System.Data.DataTable();
        //        SQL = "SELECT T.NameID " +
        //            "FROM " + Prefix() + "TaxonName AS T " +
        //            "INNER JOIN " + Prefix() + "TaxonAcceptedName AS A ON T.NameID = A.NameID " +
        //            "INNER JOIN " + Prefix() + "TaxonNameList AS L ON T.NameID = L.NameID AND L.ProjectID = ";
        //        switch (Source)
        //        {
        //            case TaxonSource.Pest:
        //                SQL += Settings.Default.DiversityTaxonNamesPestListID.ToString();
        //                break;
        //            case TaxonSource.Beneficial:
        //                SQL += Settings.Default.DiversityTaxonNamesBeneficialListID.ToString();
        //                break;
        //            case TaxonSource.Bycatch:
        //                SQL += Settings.Default.DiversityTaxonNamesBycatchListID.ToString();
        //                break;
        //        }
        //        SQL += " WHERE (A.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ") ";
        //        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtAccepted);

        //        if (dt.Rows.Count == 0 && DiversityWorkbench.Settings.TimeoutDatabase < 10)
        //        {
        //            System.Windows.Forms.MessageBox.Show("The list for the taxa could not be retrieved because the timeout is set to " + DiversityWorkbench.Settings.TimeoutDatabase.ToString() + " sec.\n\rConsider increasing the timeout");
        //            int Timeout = DiversityWorkbench.Settings.TimeoutDatabase;
        //            DiversityWorkbench.Forms.FormGetInteger f = new DiversityWorkbench.Forms.FormGetInteger(Timeout, "Timeout", "Please enter the timeout for database queries in seconds (0 = no limit)");
        //            f.setIcon(DiversityCollection.Resource.Time);
        //            f.ShowDialog();
        //            if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.Integer != null && f.Integer > -1)
        //            {
        //                DiversityWorkbench.Settings.TimeoutDatabase = (int)f.Integer;
        //                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
        //            }
        //        }
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (System.Data.DataRow R in dt.Rows)
        //            {
        //                if (R.Table.Columns.Contains("NameID") && Source == TaxonSource.Pest) // || ( R.Table.Columns.Contains("Art") && R.Table.Columns.Contains("Taxon") && R.Table.Columns.Contains("Gruppe"))))
        //                {
        //                    System.Collections.Generic.Dictionary<string, Taxon> pests = GetTaxon(int.Parse(R["NameID"].ToString()), Source);//, R["Taxon"].ToString(), R["Art"].ToString(), R["Gruppe"].ToString());
        //                    //pest.AcceptedName = (dtAccepted.Select("NameID = " + R["NameID"].ToString()).Length > 0);
        //                    foreach (KeyValuePair<string, Taxon> pest in pests)
        //                    {
        //                        if (!Taxa.ContainsKey(pest.Key))
        //                            Taxa.Add(pest.Key, pest.Value);
        //                        else { }
        //                    }
        //                }
        //                else if (R.Table.Columns.Contains("NameID") && Source != TaxonSource.Pest && R.Table.Columns.Contains("Art") && R.Table.Columns.Contains("Taxon") && R.Table.Columns.Contains("Gruppe"))
        //                {
        //                    System.Collections.Generic.Dictionary<string, Taxon> pests = GetTaxon(int.Parse(R["NameID"].ToString()), Source);//, R["Taxon"].ToString(), R["Art"].ToString(), R["Gruppe"].ToString());
        //                    //pest.AcceptedName = (dtAccepted.Select("NameID = " + R["NameID"].ToString()).Length > 0);
        //                    foreach (KeyValuePair<string, Taxon> pest in pests)
        //                    {
        //                        if (!Taxa.ContainsKey(pest.Key))
        //                            Taxa.Add(pest.Key, pest.Value);
        //                        else { }
        //                    }
        //                }
        //                else
        //                {
        //                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("private System.Collections.Generic.Dictionary<string, Pest> GetTaxa(string TopLine = , TaxonSource Source = TaxonSource.Pest)", "Adding Taxa", "Taxon could not be added due to not matching table columns", true, true);
        //                }
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return Taxa;
        //}

        //private System.Collections.Generic.Dictionary<string, Taxon> GetTaxon(int NameID, TaxonSource Source) //, string Taxon) //, string Art, string Gruppe)
        //{
        //    Taxon pest = new Taxon(NameID, Source);
        //    return pest.Stages;

        //    //string Language = DiversityWorkbench.Settings.Language.Substring(0, 2);
        //    //string Country = DiversityWorkbench.Settings.Language.Substring(3, 2);

        //    //string SQL = "SELECT T.NameID" + //, T.TaxonNameCache AS Taxon , TC.CommonName AS Art, GC.CommonName AS Gruppe " +
        //    //    "FROM " + Prefix() + "TaxonName AS T " +
        //    //    "INNER JOIN " + Prefix() + "TaxonHierarchy AS H ON T.NameID = H.NameID " +
        //    //    "INNER JOIN " + Prefix() + "TaxonName AS G ON H.NameParentID = G.NameID " +
        //    //    "INNER JOIN " + Prefix() + "TaxonCommonName AS TC ON T.NameID = TC.NameID " +
        //    //    "INNER JOIN " + Prefix() + "TaxonCommonName AS GC ON G.NameID = GC.NameID " +
        //    //    "INNER JOIN " + Prefix() + "TaxonNameList AS L ON T.NameID = L.NameID AND L.ProjectID = ";
        //    //switch (Source)
        //    //{
        //    //    case TaxonSource.Pest:
        //    //        SQL += Settings.Default.DiversityTaxonNamesPestListID.ToString();
        //    //        break;
        //    //    case TaxonSource.Beneficial:
        //    //        SQL += Settings.Default.DiversityTaxonNamesBeneficialListID.ToString();
        //    //        break;
        //    //    case TaxonSource.Bycatch:
        //    //        SQL += Settings.Default.DiversityTaxonNamesBycatchListID.ToString();
        //    //        break;
        //    //}
        //    //SQL += " WHERE (H.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ") " +
        //    //    " AND T.NameID = " + NameID.ToString();
        //    //System.Data.DataTable dt = new System.Data.DataTable();
        //    //DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);

        //    //Pest pest = new Pest();
        //    //pest.NameID = NameID;
        //    ////pest.VernacularName = Art;
        //    ////pest.ScientificName = Taxon;
        //    ////pest.Group = Gruppe;

        //    //// Icones
        //    //pest.Icones = new Dictionary<int, Resource>();
        //    //try
        //    //{
        //    //    SQL = "SELECT S.NameID, C.DisplayText AS Stage, I.AnalysisValue AS Preview " +
        //    //        "FROM TaxonNameListAnalysisCategory AS C " +
        //    //        "INNER JOIN TaxonNameListAnalysis AS S ON C.AnalysisID = S.AnalysisID " +
        //    //        "LEFT OUTER JOIN TaxonNameListAnalysisCategory AS PI ON C.AnalysisID = PI.AnalysisParentID " +
        //    //        "LEFT OUTER JOIN TaxonNameListAnalysis AS I ON PI.AnalysisID = I.AnalysisID AND S.NameID = I.NameID " +
        //    //        "WHERE (C.SortingID = 1) AND (C.DataWithholdingReason IS NULL) AND S.NameID = " + NameID.ToString();

        //    //    //SQL = "SELECT Ic.NameID, Ic.URI AS Icon, Ic.Title, Ic.Notes, Ic.DisplayOrder " +
        //    //    //    "FROM " + Prefix() + "ViewTaxonNameResource AS Ic " +
        //    //    //    "WHERE Ic.ResourceType = 'preview' AND (Ic.NameID = " + NameID.ToString() + ") " +
        //    //    //    "ORDER BY Ic.DisplayOrder";

        //    //    System.Data.DataTable dtStages = new System.Data.DataTable();
        //    //    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtStages);
        //    //    if (dt.Rows.Count > 0)
        //    //    {
        //    //        int i = 0;
        //    //        foreach (System.Data.DataRow R in dt.Rows)
        //    //        {
        //    //            Resource resource = new Resource();
        //    //            resource.Type = ResourceType.Image;
        //    //            //resource.Title = Art;
        //    //            if (!R["Title"].Equals(System.DBNull.Value) && R["Title"].ToString().Length > 0)
        //    //                resource.Title = R["Title"].ToString();
        //    //            string Message = "";
        //    //            if (!R["Icon"].Equals(System.DBNull.Value) && R["Icon"].ToString().Length > 0)
        //    //                resource.Icon = DiversityWorkbench.Forms.FormFunctions.BitmapFromWeb(R["Icon"].ToString(), ref Message);
        //    //            else
        //    //                resource.Icon = this.NoImage();
        //    //            if (!R["Notes"].Equals(System.DBNull.Value) && R["Notes"].ToString().Length > 0)
        //    //                resource.Notes = R["Notes"].ToString();
        //    //            pest.Icones.Add(i, resource);
        //    //            pest.BaseURL = this.BaseURL();
        //    //            i++;
        //    //        }
        //    //    }
        //    //}
        //    //catch (System.Exception ex)
        //    //{
        //    //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    //}

        //    //// Images
        //    //pest.Images = new Dictionary<int, Resource>();
        //    //try
        //    //{
        //    //    SQL = "SELECT Im.NameID, Im.URI AS Image, Im.Notes, Im.CopyrightStatement, Im.DisplayOrder " +
        //    //        "FROM " + Prefix() + "ViewTaxonNameResource AS Im " +
        //    //        "WHERE Im.ResourceType = 'Image' AND (Im.NameID = " + NameID.ToString() + ") " +
        //    //        "ORDER BY Im.DisplayOrder";
        //    //    System.Data.DataTable dt = new System.Data.DataTable();
        //    //    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
        //    //    if (dt.Rows.Count > 0)
        //    //    {
        //    //        int i = 0;
        //    //        foreach (System.Data.DataRow R in dt.Rows)
        //    //        {
        //    //            Resource resource = new Resource();
        //    //            resource.Type = ResourceType.Image;
        //    //            resource.Title = Art;
        //    //            if (!R["Image"].Equals(System.DBNull.Value) && R["Image"].ToString().Length > 0)
        //    //                resource.Uri = new Uri(R["Image"].ToString());
        //    //            if (!R["Notes"].Equals(System.DBNull.Value) && R["Notes"].ToString().Length > 0)
        //    //                resource.Notes = R["Notes"].ToString();
        //    //            resource.CopyRight = R["CopyrightStatement"].ToString();
        //    //            pest.Images.Add(i, resource);
        //    //            i++;
        //    //        }
        //    //    }
        //    //}
        //    //catch (System.Exception ex)
        //    //{
        //    //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    //}

        //    //// Infos
        //    //pest.Infos = new Dictionary<int, Resource>();
        //    //try
        //    //{
        //    //    string SQL = "SELECT URI AS Info, Creator " +
        //    //    "FROM " + Prefix() + "ViewTaxonNameResource AS R " +
        //    //    "WHERE ResourceType = 'Information' AND (NameID = " + NameID.ToString() + ") " +
        //    //    "ORDER BY DisplayOrder";
        //    //    System.Data.DataTable dt = new System.Data.DataTable();
        //    //    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
        //    //    if (dt.Rows.Count > 0)
        //    //    {
        //    //        int i = 0;
        //    //        foreach (System.Data.DataRow R in dt.Rows)
        //    //        {
        //    //            Resource resource = new Resource();
        //    //            resource.Type = ResourceType.Info;
        //    //            resource.Title = Art;
        //    //            if (!R["Info"].Equals(System.DBNull.Value) && R["Info"].ToString().Length > 0)
        //    //                resource.Uri = new Uri(R["Info"].ToString());
        //    //            resource.CopyRight = R["Creator"].ToString();
        //    //            pest.Infos.Add(i, resource);
        //    //            i++;
        //    //        }
        //    //    }
        //    //}
        //    //catch (System.Exception ex)
        //    //{
        //    //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    //}

        //    //return pest;
        //}

        #endregion

        #region Edit pest numbers

        private static State _State = State.Reading;

        public static State CurrentState
        {
            get { return _State; }
            set { _State = value; }
        }

        //public static bool WriteRecordValue(Tasks.Taxa.Record Record, int CollectionID, string RecordUri, double? Count, string DateSQL, string Responsible, string ResponsibleURL, 
        //    ref string Error, ref bool TrapAdded, TaxonSource taxonSource, MonitoringTarget monitoringTarget, 
        //    string Notes = "", int? CollectionSpecimenID = null, int? SpecimenPartID = null)
        //{
        //    bool OK = monitoringTarget != MonitoringTarget.Trap; // Try to find trap only for trap target
        //    try
        //    {
        //        string SQL = "";
        //        int TrapCollectionTaskID = 0;
        //        bool InsertCanceled = false;
        //        // find trap for Collection
        //        if (monitoringTarget == MonitoringTarget.Trap)
        //        {
        //            TrapCollectionTaskID = getTrapID(CollectionID, DateSQL, Responsible, ResponsibleURL, ref Error, ref TrapAdded, ref InsertCanceled);
        //            if (TrapCollectionTaskID > -1)
        //                OK = true;
        //            else
        //            {
        //                if (InsertCanceled)
        //                    return false;
        //            }
        //        }
        //        else
        //        {
        //            if (Record == null && monitoringTarget == MonitoringTarget.Collection)
        //            {
        //                string Anzahl = "";
        //                if (Count == null) Anzahl = "NULL";
        //                else Anzahl = Count.ToString();
        //                int MonitoringID = IPM.GetMonitoringTaskID(ref Error);
        //                int CollectionRecordTaskID = 0;
        //                    CollectionRecordTaskID = getTrapID(CollectionID, DateSQL, Responsible, ResponsibleURL, ref Error, ref TrapAdded, ref InsertCanceled); 
        //                SQL = "INSERT INTO CollectionTask " +
        //                    "(CollectionID, TaskID, DisplayOrder, Description, " +
        //                    "DisplayText, ModuleUri, TaskStart, NumberValue, Notes, ResponsibleAgent, ResponsibleAgentURI";
        //                if (CollectionSpecimenID != null && SpecimenPartID != null)
        //                    SQL += ", CollectionSpecimenID, SpecimenPartID";
        //                SQL += ") " +
        //                    "VALUES(" + CollectionID.ToString() + ", " + CollectionRecordTaskID.ToString() + ", 1, '" + Record.VernacularName + "', " +
        //                    "'" + Record.ScientificName + "', '" + RecordUri + "', " + DateSQL + ", " + Anzahl + ", " + Notes + ", " + Responsible + ", " + ResponsibleURL;
        //                if (CollectionSpecimenID != null && SpecimenPartID != null)
        //                    SQL += ", " + CollectionSpecimenID.ToString() + ", " + SpecimenPartID.ToString();
        //                SQL += ")";
        //                OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
        //            }
        //        }

        //        // Trap found or inserted - set value
        //        if (OK)
        //        {
        //            // Notes
        //            if (Notes.Length > 0)
        //            {
        //                Notes = "'" + Notes.Replace("'", "''") + "'";
        //            }
        //            else Notes = "NULL";

        //            // Responsible
        //            if (Responsible.Length == 0)
        //            {
        //                Responsible = "NULL";
        //                ResponsibleURL = "NULL";
        //            }
        //            else
        //            {
        //                Responsible = "'" + Responsible + "'";
        //                if (ResponsibleURL.Length == 0)
        //                    ResponsibleURL = "NULL";
        //                else
        //                    ResponsibleURL = "'" + ResponsibleURL + "'";
        //            }

        //            int RecordTaskID = 0;
        //            if (monitoringTarget == MonitoringTarget.Trap)
        //                RecordTaskID = getRecordTaskID(CollectionID, TrapCollectionTaskID, ref Error);
        //            else
        //            {

        //            }
        //            if (Error.Length > 0)
        //            {
        //                Error = "Failed to find task corresponding to a pest in the selected trap:\r\n" + Error;
        //                return false;
        //            }
        //            else
        //            {
        //                // Check if there is an entry for the same taxon at the same date
        //                SQL = "SELECT MIN(C.CollectionTaskID) " +
        //                    "FROM CollectionTask AS C INNER JOIN " +
        //                    "Task AS T ON C.TaskID = T.TaskID AND C.TaskID = " + RecordTaskID.ToString() + " " +
        //                    "AND T.ModuleType = 'DiversityTaxonNames' AND T.Type = '" + taxonSource.ToString() + "' " +
        //                    "WHERE C.CollectionID = " + CollectionID.ToString() + " " +
        //                    "AND C.ModuleUri = '" + RecordUri + "' " +
        //                    "AND C.TaskStart = " + DateSQL;
        //                if (CollectionSpecimenID != null && SpecimenPartID != null)
        //                    SQL += "AND C.CollectionSpecimenID = " + CollectionSpecimenID.ToString() + " AND C.SpecimenPartID = " + SpecimenPartID.ToString();
        //                int PestCollectionTaskID;
        //                if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out PestCollectionTaskID))
        //                {
        //                    SQL = "UPDATE C SET NumberValue = " + Count.ToString() + ", Notes = " + Notes + ", ResponsibleAgent = " + Responsible + ", ResponsibleAgentURI = " + ResponsibleURL;
        //                    SQL += " FROM CollectionTask C WHERE C.CollectionTaskID = " + PestCollectionTaskID.ToString();
        //                    if (CollectionSpecimenID != null && SpecimenPartID != null)
        //                        SQL += "AND C.CollectionSpecimenID = " + CollectionSpecimenID.ToString() + " AND C.SpecimenPartID = " + SpecimenPartID.ToString();
        //                    OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
        //                }
        //                else
        //                {
        //                    SQL = "INSERT INTO CollectionTask " +
        //                        "(CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, Description, " +
        //                        "DisplayText, ModuleUri, TaskStart, NumberValue, Notes, ResponsibleAgent, ResponsibleAgentURI";
        //                    if (CollectionSpecimenID != null && SpecimenPartID != null)
        //                        SQL += ", CollectionSpecimenID, SpecimenPartID";
        //                    SQL += ") " +
        //                        "VALUES(" + TrapCollectionTaskID.ToString() + ", " + CollectionID.ToString() + ", " + RecordTaskID.ToString() + ", 1, '" + Record.VernacularName + "', " +
        //                        "'" + Record.ScientificName + "', '" + RecordUri + "', " + DateSQL + ", " + Count.ToString() + ", " + Notes + ", " + Responsible + ", " + ResponsibleURL;
        //                    if (CollectionSpecimenID != null && SpecimenPartID != null)
        //                        SQL += ", " + CollectionSpecimenID.ToString() + ", " + SpecimenPartID.ToString();
        //                    SQL += ")";
        //                    OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
        //                }
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return OK;
        //}

        public static bool WriteRecordingValue(IPM.RecordingTarget recordingTarget, Tasks.Taxa.TaxonRecord Record, int CollectionID, string RecordUri, double? Count,
            string DateSQL, string Responsible, string ResponsibleURL,
            ref string Error, ref bool TrapAdded,
            string State = "", string Notes = "", int? CollectionSpecimenID = null, int? SpecimenPartID = null, int? TransactionID = null)
        {
            bool OK = recordingTarget != RecordingTarget.TrapPest && recordingTarget != RecordingTarget.TrapBycatch; // monitoringTarget != MonitoringTarget.Trap; // Try to find trap only for trap target
            try
            {
                string SQL = "";
                int TrapCollectionTaskID = 0;
                bool InsertCanceled = false;
                // find trap for Collection
                if (recordingTarget == RecordingTarget.TrapPest || recordingTarget == RecordingTarget.TrapBycatch) // monitoringTarget == MonitoringTarget.Trap)
                {
                    TrapCollectionTaskID = getTrapID(CollectionID, DateSQL, Responsible, ResponsibleURL, ref Error, ref TrapAdded, ref InsertCanceled);
                    if (TrapCollectionTaskID > -1)
                        OK = true;
                    else
                    {
                        if (InsertCanceled)
                            return false;
                    }
                }
                else
                {
                    if (Record == null && recordingTarget == RecordingTarget.CollectionPest) // monitoringTarget == MonitoringTarget.Collection)
                    {
                        string Anzahl = "";
                        if (Count == null) Anzahl = "NULL";
                        else Anzahl = Count.ToString();
                        int MonitoringID = IPM.GetMonitoringTaskID(ref Error);
                        int CollectionRecordTaskID = 0;
                        CollectionRecordTaskID = getTrapID(CollectionID, DateSQL, Responsible, ResponsibleURL, ref Error, ref TrapAdded, ref InsertCanceled);
                        SQL = "INSERT INTO CollectionTask " +
                            "(CollectionID, TaskID, DisplayOrder, Description, " +
                            "DisplayText, ModuleUri, TaskStart, NumberValue, Notes, ResponsibleAgent, ResponsibleAgentURI";
                        if (State.Length > 0)
                            SQL += ", Result";
                        if (CollectionSpecimenID != null && SpecimenPartID != null)
                            SQL += ", CollectionSpecimenID, SpecimenPartID";
                        if (TransactionID != null)
                            SQL += ", TransactionID";
                        SQL += ") " +
                            "VALUES(" + CollectionID.ToString() + ", " + CollectionRecordTaskID.ToString() + ", 1, '" + Record.VernacularName.Replace("'", "''") + "', " +
                            "'" + Record.ScientificName.Replace("'", "''") + "', '" + RecordUri + "', " + DateSQL + ", " + Anzahl + ", " + Notes.Replace("'", "''") + ", " + Responsible.Replace("'", "''") + ", " + ResponsibleURL;
                        if (State.Length > 0)
                            SQL += ", '" + State.Replace("'", "''") + "'";
                        if (CollectionSpecimenID != null && SpecimenPartID != null)
                            SQL += ", " + CollectionSpecimenID.ToString() + ", " + SpecimenPartID.ToString();
                        if (TransactionID != null)
                            SQL += ", " + TransactionID.ToString();
                        SQL += ")";
                        OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
                    }
                }

                // Trap found or inserted - set value
                if (OK)
                {
                    // State
                    if (State.Length > 0)
                    {
                        State = "'" + State.Replace("'", "''") + "'";
                    }
                    else State = "NULL";

                    // Notes
                    if (Notes.Length > 0)
                    {
                        Notes = "'" + Notes.Replace("'", "''") + "'";
                    }
                    else Notes = "NULL";

                    // Responsible
                    if (Responsible.Length == 0)
                    {
                        Responsible = "NULL";
                        ResponsibleURL = "NULL";
                    }
                    else
                    {
                        Responsible = "'" + Responsible + "'";
                        if (ResponsibleURL.Length == 0)
                            ResponsibleURL = "NULL";
                        else
                            ResponsibleURL = "'" + ResponsibleURL + "'";
                    }

                    int RecordTaskID = 0;
                    if (recordingTarget == RecordingTarget.TrapPest || recordingTarget == RecordingTarget.TrapBycatch)// monitoringTarget == MonitoringTarget.Trap)
                        RecordTaskID = getRecordTaskID(recordingTarget, CollectionID, TrapCollectionTaskID, ref Error);
                    else
                    {

                    }
                    if (Error.Length > 0)
                    {
                        Error = "Failed to find task corresponding to a pest in the selected trap:\r\n" + Error;
                        return false;
                    }
                    else
                    {
                        // Check if there is an entry for the same taxon at the same date
                        SQL = "SELECT MIN(C.CollectionTaskID) " +
                            "FROM CollectionTask AS C INNER JOIN " +
                            "Task AS T ON C.TaskID = T.TaskID AND C.TaskID = " + RecordTaskID.ToString() + " " +
                            "AND T.ModuleType = 'DiversityTaxonNames' AND T.Type = '" + RecordingTaskType(recordingTarget) + "' " +
                            "WHERE C.CollectionID = " + CollectionID.ToString() + " " +
                            "AND C.ModuleUri = '" + RecordUri + "' " +
                            "AND C.TaskStart = " + DateSQL;
                        if (CollectionSpecimenID != null && SpecimenPartID != null)
                            SQL += "AND C.CollectionSpecimenID = " + CollectionSpecimenID.ToString() + " AND C.SpecimenPartID = " + SpecimenPartID.ToString();
                        if (TransactionID != null)
                            SQL += "AND C.TransactionID = " + TransactionID.ToString();
                        int PestCollectionTaskID;
                        if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out PestCollectionTaskID))
                        {
                            SQL = "UPDATE C SET NumberValue = " + Count.ToString() + ", Result = " + State + ", Notes = " + Notes + ", ResponsibleAgent = " + Responsible + ", ResponsibleAgentURI = " + ResponsibleURL;
                            SQL += " FROM CollectionTask C WHERE C.CollectionTaskID = " + PestCollectionTaskID.ToString();
                            if (CollectionSpecimenID != null && SpecimenPartID != null)
                                SQL += "AND C.CollectionSpecimenID = " + CollectionSpecimenID.ToString() + " AND C.SpecimenPartID = " + SpecimenPartID.ToString();
                            if (TransactionID != null)
                                SQL += "AND C.TransactionID = " + TransactionID.ToString();
                            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
                        }
                        else
                        {
                            SQL = "INSERT INTO CollectionTask " +
                                "(CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, Description, " +
                                "DisplayText, ModuleUri, TaskStart, NumberValue, Result, Notes, ResponsibleAgent, ResponsibleAgentURI";
                            if (CollectionSpecimenID != null && SpecimenPartID != null)
                                SQL += ", CollectionSpecimenID, SpecimenPartID";
                            if (TransactionID != null)
                                SQL += ", TransactionID";
                            SQL += ") " +
                                "VALUES(" + TrapCollectionTaskID.ToString() + ", " + CollectionID.ToString() + ", " + RecordTaskID.ToString() + ", 1, '" + Record.VernacularName + "', " +
                                "'" + Record.ScientificName + "', '" + RecordUri + "', " + DateSQL + ", " + Count.ToString() + ", " + State + ", " + Notes + ", " + Responsible + ", " + ResponsibleURL;
                            if (CollectionSpecimenID != null && SpecimenPartID != null)
                                SQL += ", " + CollectionSpecimenID.ToString() + ", " + SpecimenPartID.ToString();
                            if (TransactionID != null)
                                SQL += ", " + TransactionID.ToString();
                            SQL += ")";
                            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
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

        #region Write values
        public static bool WriteRecordingValue(IPM.RecordingTarget recordingTarget, Tasks.Taxa.TaxonRecord taxonRecord, ref string Error, ref bool TrapAdded, 
            int CollectionID, string RecordUri, double? Count,
            string DateSQL, string Responsible, string ResponsibleURL,
            string State = "", string Notes = "", int? CollectionSpecimenID = null, int? SpecimenPartID = null, int? TransactionID = null)
        {
            bool OK = false; // recordingTarget != RecordingTarget.TrapPest && recordingTarget != RecordingTarget.TrapBycatch; // monitoringTarget != MonitoringTarget.Trap; // Try to find trap only for trap target
            try
            {
                string SQL = "";
                int TrapCollectionTaskID = 0;
                bool InsertCanceled = false;
                // find trap if the target is a trap
                if (recordingTarget == RecordingTarget.TrapPest || recordingTarget == RecordingTarget.TrapBycatch) // monitoringTarget == MonitoringTarget.Trap)
                {
                    TrapCollectionTaskID = getTrapID(CollectionID, DateSQL, Responsible, ResponsibleURL, ref Error, ref TrapAdded, ref InsertCanceled);
                    if (TrapCollectionTaskID > -1)
                        OK = true;
                    else
                    {
                        if (InsertCanceled)
                            return false;
                    }
                }
                else
                {
                    if (taxonRecord == null && recordingTarget == RecordingTarget.CollectionPest) // monitoringTarget == MonitoringTarget.Collection)
                    {
                        string Anzahl = "";
                        if (Count == null) Anzahl = "NULL";
                        else Anzahl = Count.ToString();
                        int MonitoringID = IPM.GetMonitoringTaskID(ref Error);
                        int CollectionRecordTaskID = 0;
                        CollectionRecordTaskID = getTrapID(CollectionID, DateSQL, Responsible, ResponsibleURL, ref Error, ref TrapAdded, ref InsertCanceled);
                        SQL = CollectionTaskSqlInsert(CollectionID, CollectionRecordTaskID, null, RecordUri,
                            taxonRecord.VernacularName, taxonRecord.ScientificName, Count, DateSQL, Responsible, ResponsibleURL, State, Notes,
                            CollectionSpecimenID, SpecimenPartID, TransactionID);
                        OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
                    }
                    else OK = true;
                }

                // Trap found or inserted - set value
                if (OK)
                {
                    int RecordTaskID = 0;
                    if (recordingTarget == RecordingTarget.TrapPest || recordingTarget == RecordingTarget.TrapBycatch)// monitoringTarget == MonitoringTarget.Trap)
                        RecordTaskID = getRecordTaskID(recordingTarget, CollectionID, TrapCollectionTaskID, ref Error);
                    else
                    {
                        RecordTaskID = getRecordTaskID(recordingTarget, CollectionID, -1, ref Error);
                    }
                    if (Error.Length > 0)
                    {
                        Error = "Failed to find task corresponding to a pest in the selected trap:\r\n" + Error;
                        return false;
                    }
                    else
                    {
                        // Check if there is an entry for the same taxon at the same date
                        int? existingCollectionTaskID = IDofExistingCollectionTask(RecordTaskID, RecordingTaskType(recordingTarget), CollectionID, RecordUri, DateSQL, 
                            CollectionSpecimenID, SpecimenPartID, TransactionID);
                        if (existingCollectionTaskID != null)
                        {
                            SQL = CollectionTaskSqlUpdate(CollectionID, RecordTaskID, (int)existingCollectionTaskID, Count, DateSQL, Responsible, ResponsibleURL, State, Notes); //, CollectionSpecimenID, SpecimenPartID, TransactionID);
                            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
                        }
                        else
                        {
                            string DisplayText = taxonRecord.VernacularName;
                            if (taxonRecord.Stage.Length > 0) DisplayText = taxonRecord.Stage + " of " + DisplayText;
                            SQL = CollectionTaskSqlInsert(CollectionID, RecordTaskID, null, RecordUri, DisplayText, taxonRecord.ScientificName, Count, DateSQL, Responsible, ResponsibleURL, State, Notes, CollectionSpecimenID, SpecimenPartID, TransactionID);
                            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
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

        private static int? IDofExistingCollectionTask(int TaskID, string TaskType, int CollectionID, string TaxonRecordUri, string StartDateSQL, 
            int? CollectionSpecimenID, int? SpecimenPartID, int? TransactionID)
        {
            int? CollectionTaskID = null;
            string SQL = "SELECT MIN(C.CollectionTaskID) " +
                "FROM CollectionTask AS C INNER JOIN " +
                "Task AS T ON C.TaskID = T.TaskID AND C.TaskID = " + TaskID.ToString() + " " +
                "AND T.ModuleType = 'DiversityTaxonNames' AND T.Type = '" + TaskType + "' " +
                "WHERE C.CollectionID = " + CollectionID.ToString() + " " +
                "AND C.ModuleUri = '" + TaxonRecordUri + "' " +
                "AND C.TaskStart = " + StartDateSQL;
            if (CollectionSpecimenID != null && SpecimenPartID != null)
                SQL += "AND C.CollectionSpecimenID = " + CollectionSpecimenID.ToString() + " AND C.SpecimenPartID = " + SpecimenPartID.ToString();
            if (TransactionID != null)
                SQL += "AND C.TransactionID = " + TransactionID.ToString();
            int ID;
            if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out ID)) CollectionTaskID = ID;
                return CollectionTaskID;
        }

        private static string CollectionTaskSqlInsert(int CollectionID, int TaskID, int? CollectionTaskID, string RecordUri, 
            string DisplayText, string Description, double? Count, string DateSQL, string Responsible, string ResponsibleURL,
            string State = "", string Notes = "", int? CollectionSpecimenID = null, int? SpecimenPartID = null, int? TransactionID = null, int DisplayOrder = 1)
        {
            string SQL = "";
            try
            {
                SQL = "INSERT INTO CollectionTask " +
                    "(CollectionID, TaskID, DisplayOrder, Description, " +
                    "DisplayText, ModuleUri, TaskStart, NumberValue, " +
                    "Notes, ResponsibleAgent, ResponsibleAgentURI, Result, " +
                    "CollectionSpecimenID, SpecimenPartID, TransactionID) " +
                    "VALUES(" + CollectionID.ToString() + ", " + TaskID.ToString() + ", " + DisplayOrder.ToString() + ", " + SqlConformValue(Description) + ", " +
                    SqlConformValue(DisplayText) + ", " + SqlConformValue(RecordUri) + ", " + DateSQL + ", " + SqlConformValue(Count) + ", " + 
                    SqlConformValue(Notes) + ", " + SqlConformValue(Responsible) + ", " + SqlConformValue(ResponsibleURL) + ", " + SqlConformValue(State) +  ", " +
                    SqlConformValue(CollectionSpecimenID) + ", " + SqlConformValue(SpecimenPartID) + ", " + SqlConformValue(TransactionID) + ")";
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return SQL;
        }

        private static string CollectionTaskSqlUpdate(int CollectionID, int TaskID, int CollectionTaskID,
            double? Count, string DateSQL, string Responsible, string ResponsibleURL,
            string State = "", string Notes = "") //, int? CollectionSpecimenID = null, int? SpecimenPartID = null, int? TransactionID = null)
        {
            string SQL = "";
            try
            {
                SQL = "UPDATE C SET NumberValue = " + SqlConformValue(Count) + ", Result = " + SqlConformValue(State) + ", Notes = " + SqlConformValue(Notes) +
                    ", ResponsibleAgent = " + SqlConformValue(Responsible) + ", ResponsibleAgentURI = " + SqlConformValue(ResponsibleURL);
                SQL += " FROM CollectionTask C " +
                    "WHERE C.CollectionTaskID = " + CollectionTaskID.ToString();
                //if (CollectionSpecimenID != null && SpecimenPartID != null)
                //    SQL += "AND C.CollectionSpecimenID = " + SqlConformValue(CollectionSpecimenID) + " AND C.SpecimenPartID = " + SqlConformValue(SpecimenPartID);
                //if (TransactionID != null)
                //    SQL += "AND C.TransactionID = " + SqlConformValue(TransactionID);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return SQL;
        }


        private static string SqlConformValue(object Value)
        {
            string SQL = "";
            if (Value == null)
                SQL = "NULL";
            else 
            {
                string Type = Value.GetType().ToString();
                switch (Type)
                {
                    case "System.String":
                    case "string":
                        string val = (string)Value;
                        if (val == null || val.Length == 0) SQL = "NULL";
                        else
                        {
                            if (val.EndsWith("'") && val.StartsWith("'")) SQL = val;
                            else SQL = "'" + val.Replace("'", "''") + "'";
                        }
                        break;
                    default:
                        SQL = Value.ToString();
                        break;
                }
            }
            return SQL;
        }

        //private static bool WriteRecordingTrapValue(IPM.RecordingTarget recordingTarget, Tasks.Taxa.TaxonRecord taxonRecord, ref string Error, ref bool TrapAdded,
        //    int CollectionID, string RecordUri, double? Count,
        //    string DateSQL, string Responsible, string ResponsibleURL,
        //    string State = "", string Notes = "")
        //{
        //    bool OK = recordingTarget != RecordingTarget.TrapPest && recordingTarget != RecordingTarget.TrapBycatch; // monitoringTarget != MonitoringTarget.Trap; // Try to find trap only for trap target
        //    try
        //    {
        //        string SQL = "";
        //        int TrapCollectionTaskID = 0;
        //        bool InsertCanceled = false;
        //        // find trap for Collection
        //        if (recordingTarget == RecordingTarget.TrapPest || recordingTarget == RecordingTarget.TrapBycatch) // monitoringTarget == MonitoringTarget.Trap)
        //        {
        //            TrapCollectionTaskID = getTrapID(CollectionID, DateSQL, Responsible, ResponsibleURL, ref Error, ref TrapAdded, ref InsertCanceled);
        //            if (TrapCollectionTaskID > -1)
        //                OK = true;
        //            else
        //            {
        //                if (InsertCanceled)
        //                    return false;
        //            }
        //        }
        //        else
        //        {
        //            if (taxonRecord == null && recordingTarget == RecordingTarget.CollectionPest) // monitoringTarget == MonitoringTarget.Collection)
        //            {
        //                string Anzahl = "";
        //                if (Count == null) Anzahl = "NULL";
        //                else Anzahl = Count.ToString();
        //                int MonitoringID = IPM.GetMonitoringTaskID(ref Error);
        //                int CollectionRecordTaskID = 0;
        //                CollectionRecordTaskID = getTrapID(CollectionID, DateSQL, Responsible, ResponsibleURL, ref Error, ref TrapAdded, ref InsertCanceled);
        //                SQL = "INSERT INTO CollectionTask " +
        //                    "(CollectionID, TaskID, DisplayOrder, Description, " +
        //                    "DisplayText, ModuleUri, TaskStart, NumberValue, Notes, ResponsibleAgent, ResponsibleAgentURI";
        //                if (State.Length > 0)
        //                    SQL += ", Result";
        //                SQL += ") " +
        //                    "VALUES(" + CollectionID.ToString() + ", " + CollectionRecordTaskID.ToString() + ", 1, '" + taxonRecord.VernacularName.Replace("'", "''") + "', " +
        //                    "'" + taxonRecord.ScientificName.Replace("'", "''") + "', '" + RecordUri + "', " + DateSQL + ", " + Anzahl + ", " + Notes.Replace("'", "''") + ", " + Responsible.Replace("'", "''") + ", " + ResponsibleURL;
        //                if (State.Length > 0)
        //                    SQL += ", '" + State.Replace("'", "''") + "'";
        //                SQL += ")";
        //                OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
        //            }
        //        }

        //        // Trap found or inserted - set value
        //        if (OK)
        //        {
        //            // State
        //            if (State.Length > 0)
        //            {
        //                State = "'" + State.Replace("'", "''") + "'";
        //            }
        //            else State = "NULL";

        //            // Notes
        //            if (Notes.Length > 0)
        //            {
        //                Notes = "'" + Notes.Replace("'", "''") + "'";
        //            }
        //            else Notes = "NULL";

        //            // Responsible
        //            if (Responsible.Length == 0)
        //            {
        //                Responsible = "NULL";
        //                ResponsibleURL = "NULL";
        //            }
        //            else
        //            {
        //                Responsible = "'" + Responsible + "'";
        //                if (ResponsibleURL.Length == 0)
        //                    ResponsibleURL = "NULL";
        //                else
        //                    ResponsibleURL = "'" + ResponsibleURL + "'";
        //            }

        //            int RecordTaskID = 0;
        //            if (recordingTarget == RecordingTarget.TrapPest || recordingTarget == RecordingTarget.TrapBycatch)// monitoringTarget == MonitoringTarget.Trap)
        //                RecordTaskID = getRecordTaskID(CollectionID, TrapCollectionTaskID, ref Error);
        //            else
        //            {

        //            }
        //            if (Error.Length > 0)
        //            {
        //                Error = "Failed to find task corresponding to a pest in the selected trap:\r\n" + Error;
        //                return false;
        //            }
        //            else
        //            {
        //                // Check if there is an entry for the same taxon at the same date
        //                SQL = "SELECT MIN(C.CollectionTaskID) " +
        //                    "FROM CollectionTask AS C INNER JOIN " +
        //                    "Task AS T ON C.TaskID = T.TaskID AND C.TaskID = " + RecordTaskID.ToString() + " " +
        //                    "AND T.ModuleType = 'DiversityTaxonNames' AND T.Type = '" + RecordingTaskType(recordingTarget) + "' " +
        //                    "WHERE C.CollectionID = " + CollectionID.ToString() + " " +
        //                    "AND C.ModuleUri = '" + RecordUri + "' " +
        //                    "AND C.TaskStart = " + DateSQL;
        //                int PestCollectionTaskID;
        //                if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out PestCollectionTaskID))
        //                {
        //                    SQL = "UPDATE C SET NumberValue = " + Count.ToString() + ", Result = " + State + ", Notes = " + Notes + ", ResponsibleAgent = " + Responsible + ", ResponsibleAgentURI = " + ResponsibleURL;
        //                    SQL += " FROM CollectionTask C WHERE C.CollectionTaskID = " + PestCollectionTaskID.ToString();
        //                    OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
        //                }
        //                else
        //                {
        //                    SQL = "INSERT INTO CollectionTask " +
        //                        "(CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, Description, " +
        //                        "DisplayText, ModuleUri, TaskStart, NumberValue, Result, Notes, ResponsibleAgent, ResponsibleAgentURI";
        //                    SQL += ") " +
        //                        "VALUES(" + TrapCollectionTaskID.ToString() + ", " + CollectionID.ToString() + ", " + RecordTaskID.ToString() + ", 1, '" + taxonRecord.VernacularName + "', " +
        //                        "'" + taxonRecord.ScientificName + "', '" + RecordUri + "', " + DateSQL + ", " + Count.ToString() + ", " + State + ", " + Notes + ", " + Responsible + ", " + ResponsibleURL;
        //                    SQL += ")";
        //                    OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
        //                }
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return OK;
        //}

        //private static System.Collections.Generic.Dictionary<string, string> SQLvalues(int CollectionID, string RecordUri, double? Count, string State = "", string Notes = "",
        //    int? CollectionSpecimenID = null, int? SpecimenPartID = null, int? TransactionID = null)
        //{
        //    System.Collections.Generic.Dictionary<string, string> dict = new Dictionary<string, string>();

        //    return dict;
        //}



        //public static bool WriteRecordingValue(IPM.RecordingTarget recordingTarget, Tasks.Taxa.Record Record, int CollectionID, string RecordUri, double? Count, 
        //    string DateSQL, string Responsible, string ResponsibleURL,
        //    ref string Error, ref bool TrapAdded,
        //    string Notes = "", int? CollectionSpecimenID = null, int? SpecimenPartID = null, int? TransactionID = null)
        //{
        //    bool OK = recordingTarget != RecordingTarget.TrapPest && recordingTarget != RecordingTarget.TrapBycatch; // monitoringTarget != MonitoringTarget.Trap; // Try to find trap only for trap target
        //    try
        //    {
        //        string SQL = "";
        //        int TrapCollectionTaskID = 0;
        //        bool InsertCanceled = false;
        //        // find trap for Collection
        //        if (recordingTarget == RecordingTarget.TrapPest || recordingTarget == RecordingTarget.TrapBycatch) // monitoringTarget == MonitoringTarget.Trap)
        //        {
        //            TrapCollectionTaskID = getTrapID(CollectionID, DateSQL, Responsible, ResponsibleURL, ref Error, ref TrapAdded, ref InsertCanceled);
        //            if (TrapCollectionTaskID > -1)
        //                OK = true;
        //            else
        //            {
        //                if (InsertCanceled)
        //                    return false;
        //            }
        //        }
        //        else
        //        {
        //            if (Record == null && recordingTarget == RecordingTarget.CollectionPest) // monitoringTarget == MonitoringTarget.Collection)
        //            {
        //                string Anzahl = "";
        //                if (Count == null) Anzahl = "NULL";
        //                else Anzahl = Count.ToString();
        //                int MonitoringID = IPM.GetMonitoringTaskID(ref Error);
        //                int CollectionRecordTaskID = 0;
        //                CollectionRecordTaskID = getTrapID(CollectionID, DateSQL, Responsible, ResponsibleURL, ref Error, ref TrapAdded, ref InsertCanceled);
        //                SQL = "INSERT INTO CollectionTask " +
        //                    "(CollectionID, TaskID, DisplayOrder, Description, " +
        //                    "DisplayText, ModuleUri, TaskStart, NumberValue, Notes, ResponsibleAgent, ResponsibleAgentURI";
        //                if (CollectionSpecimenID != null && SpecimenPartID != null)
        //                    SQL += ", CollectionSpecimenID, SpecimenPartID";
        //                if (TransactionID != null)
        //                    SQL += ", TransactionID";
        //                SQL += ") " +
        //                    "VALUES(" + CollectionID.ToString() + ", " + CollectionRecordTaskID.ToString() + ", 1, '" + Record.VernacularName + "', " +
        //                    "'" + Record.ScientificName + "', '" + RecordUri + "', " + DateSQL + ", " + Anzahl + ", " + Notes + ", " + Responsible + ", " + ResponsibleURL;
        //                if (CollectionSpecimenID != null && SpecimenPartID != null)
        //                    SQL += ", " + CollectionSpecimenID.ToString() + ", " + SpecimenPartID.ToString();
        //                if (TransactionID != null)
        //                    SQL += ", " + TransactionID.ToString();
        //                SQL += ")";
        //                OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
        //            }
        //        }

        //        // Trap found or inserted - set value
        //        if (OK)
        //        {
        //            // Notes
        //            if (Notes.Length > 0)
        //            {
        //                Notes = "'" + Notes.Replace("'", "''") + "'";
        //            }
        //            else Notes = "NULL";

        //            // Responsible
        //            if (Responsible.Length == 0)
        //            {
        //                Responsible = "NULL";
        //                ResponsibleURL = "NULL";
        //            }
        //            else
        //            {
        //                Responsible = "'" + Responsible + "'";
        //                if (ResponsibleURL.Length == 0)
        //                    ResponsibleURL = "NULL";
        //                else
        //                    ResponsibleURL = "'" + ResponsibleURL + "'";
        //            }

        //            int RecordTaskID = 0;
        //            if (recordingTarget == RecordingTarget.TrapPest || recordingTarget == RecordingTarget.TrapBycatch)// monitoringTarget == MonitoringTarget.Trap)
        //                RecordTaskID = getRecordTaskID(CollectionID, TrapCollectionTaskID, ref Error);
        //            else
        //            {

        //            }
        //            if (Error.Length > 0)
        //            {
        //                Error = "Failed to find task corresponding to a pest in the selected trap:\r\n" + Error;
        //                return false;
        //            }
        //            else
        //            {
        //                // Check if there is an entry for the same taxon at the same date
        //                SQL = "SELECT MIN(C.CollectionTaskID) " +
        //                    "FROM CollectionTask AS C INNER JOIN " +
        //                    "Task AS T ON C.TaskID = T.TaskID AND C.TaskID = " + RecordTaskID.ToString() + " " +
        //                    "AND T.ModuleType = 'DiversityTaxonNames' AND T.Type = '" + RecordingTaskType(recordingTarget) + "' " +
        //                    "WHERE C.CollectionID = " + CollectionID.ToString() + " " +
        //                    "AND C.ModuleUri = '" + RecordUri + "' " +
        //                    "AND C.TaskStart = " + DateSQL;
        //                if (CollectionSpecimenID != null && SpecimenPartID != null)
        //                    SQL += "AND C.CollectionSpecimenID = " + CollectionSpecimenID.ToString() + " AND C.SpecimenPartID = " + SpecimenPartID.ToString();
        //                if (TransactionID != null)
        //                    SQL += "AND C.TransactionID = " + TransactionID.ToString();
        //                int PestCollectionTaskID;
        //                if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out PestCollectionTaskID))
        //                {
        //                    SQL = "UPDATE C SET NumberValue = " + Count.ToString() + ", Notes = " + Notes + ", ResponsibleAgent = " + Responsible + ", ResponsibleAgentURI = " + ResponsibleURL;
        //                    SQL += " FROM CollectionTask C WHERE C.CollectionTaskID = " + PestCollectionTaskID.ToString();
        //                    if (CollectionSpecimenID != null && SpecimenPartID != null)
        //                        SQL += "AND C.CollectionSpecimenID = " + CollectionSpecimenID.ToString() + " AND C.SpecimenPartID = " + SpecimenPartID.ToString();
        //                    if (TransactionID != null)
        //                        SQL += "AND C.TransactionID = " + TransactionID.ToString();
        //                    OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
        //                }
        //                else
        //                {
        //                    SQL = "INSERT INTO CollectionTask " +
        //                        "(CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, Description, " +
        //                        "DisplayText, ModuleUri, TaskStart, NumberValue, Notes, ResponsibleAgent, ResponsibleAgentURI";
        //                    if (CollectionSpecimenID != null && SpecimenPartID != null)
        //                        SQL += ", CollectionSpecimenID, SpecimenPartID";
        //                    if (TransactionID != null)
        //                        SQL += ", TransactionID";
        //                    SQL += ") " +
        //                        "VALUES(" + TrapCollectionTaskID.ToString() + ", " + CollectionID.ToString() + ", " + RecordTaskID.ToString() + ", 1, '" + Record.VernacularName + "', " +
        //                        "'" + Record.ScientificName + "', '" + RecordUri + "', " + DateSQL + ", " + Count.ToString() + ", " + Notes + ", " + Responsible + ", " + ResponsibleURL;
        //                    if (CollectionSpecimenID != null && SpecimenPartID != null)
        //                        SQL += ", " + CollectionSpecimenID.ToString() + ", " + SpecimenPartID.ToString();
        //                    if (TransactionID != null)
        //                        SQL += ", " + TransactionID.ToString();
        //                    SQL += ")";
        //                    OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
        //                }
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return OK;
        //}



        //public static bool WritePestValue(Tasks.Taxon Pest, int CollectionID, string PestUri, double Count, string DateSQL, string Responsible, string ResponsibleURL, ref string Error, ref bool TrapAdded, string Notes = "")
        //{
        //    bool OK = false;
        //    try
        //    {
        //        string SQL = "";

        //        // find trap for Collection
        //        bool InsertCanceled = false;
        //        int TrapCollectionTaskID = getTrapID(CollectionID, DateSQL, Responsible, ResponsibleURL, ref Error, ref TrapAdded, ref InsertCanceled);
        //        if (TrapCollectionTaskID > -1)
        //            OK = true;

        //        // Trap found or inserted - set value
        //        if (OK)
        //        {
        //            // Notes
        //            if (Notes.Length > 0)
        //            {
        //                Notes = "'" + Notes.Replace("'", "''") + "'";
        //            }
        //            else Notes = "NULL";

        //            // Responsible
        //            if (Responsible.Length == 0)
        //            {
        //                Responsible = "NULL";
        //                ResponsibleURL = "NULL";
        //            }
        //            else
        //            {
        //                Responsible = "'" + Responsible + "'";
        //                if (ResponsibleURL.Length == 0)
        //                    ResponsibleURL = "NULL";
        //                else
        //                    ResponsibleURL = "'" + ResponsibleURL + "'";
        //            }

        //            // TaskID for the Pests in the trap
        //            //SQL = "SELECT MIN(P.TaskID) " +
        //            //    "FROM CollectionTask AS C INNER JOIN " +
        //            //    "Task AS T ON C.TaskID = T.TaskID " +
        //            //    "INNER JOIN Task P ON P.TaskParentID = T.TaskID AND P.ModuleType = 'DiversityTaxonNames' AND P.Type = 'Pest' " +
        //            //    "WHERE C.CollectionTaskID = " + TrapCollectionTaskID.ToString() + " AND (C.CollectionID = " + CollectionID.ToString() + ")";
        //            int PestTaskID = getPestTaskID(CollectionID, TrapCollectionTaskID, ref Error);
        //            if (Error.Length > 0)
        //            {
        //                Error = "Failed to find task corresponding to a pest in the selected trap:\r\n" + Error;
        //                return false;
        //            }
        //            else
        //            {
        //                // Check if there is an entry for the same taxon at the same date
        //                SQL = "SELECT MIN(C.CollectionTaskID) " +
        //                    "FROM CollectionTask AS C INNER JOIN " +
        //                    "Task AS T ON C.TaskID = T.TaskID AND C.TaskID = " + PestTaskID.ToString() + " " +
        //                    "AND T.ModuleType = 'DiversityTaxonNames' AND T.Type = 'Pest' " +
        //                    "WHERE C.CollectionID = " + CollectionID.ToString() + " " +
        //                    "AND C.ModuleUri = '" + PestUri + "' " +
        //                    "AND C.TaskStart = " + DateSQL;
        //                int PestCollectionTaskID;
        //                if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out PestCollectionTaskID))
        //                {
        //                    SQL = "UPDATE C SET NumberValue = " + Count.ToString() + ", Notes = " + Notes + ", ResponsibleAgent = " + Responsible + ", ResponsibleAgentURI = " + ResponsibleURL;
        //                    SQL += " FROM CollectionTask C WHERE C.CollectionTaskID = " + PestCollectionTaskID.ToString();
        //                    OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
        //                }
        //                else
        //                {

        //                    SQL = "INSERT INTO CollectionTask " +
        //                        "(CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, Description, " +
        //                        "DisplayText, ModuleUri, TaskStart, NumberValue, Notes, ResponsibleAgent, ResponsibleAgentURI) " +
        //                        "VALUES(" + TrapCollectionTaskID.ToString() + ", " + CollectionID.ToString() + ", " + PestTaskID.ToString() + ", 1, '" + Pest.VernacularName + "', " +
        //                        "'" + Pest.ScientificName + "', '" + PestUri + "', " + DateSQL + ", " + Count.ToString() + ", " + Notes + ", " + Responsible + ", " + ResponsibleURL + ")";
        //                    OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
        //                }
        //                //OK = true;
        //            }
        //            //else
        //            //{
        //            //    OK = false;
        //            //    Error = "Could not find task for this entry";
        //            //}
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return OK;
        //}

        private static int getTrapID(int CollectionID, string DateSQL, string Responsible, string ResponsibleURL, ref string Error, ref bool TrapAdded, ref bool InsertCanceled)
        {
            int TrapID = -1;
            TrapAdded = false;
            try {
                string SQL = "SELECT C.CollectionTaskID, C.DisplayText " +
                    "FROM CollectionTask AS C INNER JOIN " +
                    "Task AS T ON C.TaskID = T.TaskID " +
                    "WHERE (T.Type = N'Trap') " +
                    "AND(C.TaskStart IS NULL OR C.TaskStart >= " + DateSQL + ") " +
                    "AND(C.TaskEnd IS NULL OR C.TaskEnd <= " + DateSQL + " ) " +
                    "AND (C.CollectionID = " + CollectionID.ToString() + ")";
                System.Data.DataTable dtTraps = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTraps, ref Error);
                //if (Error.Length > 0)
                //    return -1;

                if (dtTraps.Rows.Count == 1)
                {
                    if (!int.TryParse(dtTraps.Rows[0][0].ToString(), out TrapID))
                    {
                        TrapID = -1;
                    }
                }
                else if (dtTraps.Rows.Count > 1) // more then 1 trap
                {
                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtTraps, "DisplayText", "CollectionTaskID", "Please select a trap from the list", "Trap", "", false, true, true, DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Trap));
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        if (!int.TryParse(f.SelectedValue, out TrapID))
                            TrapID = -1;
                    }
                }
                else // no trap
                {
                    string Datum = DateSQL.Substring(DateSQL.IndexOf("'"));
                    Datum = Datum.Substring(0, Datum.IndexOf(" "));
                    string Message = "There is no active trap."; // "Seems there is an active trap that has been installed after " + Datum + ".\r\nDo you want to use this trap (= YES)\r\nor create a new one (= NO)\r\nor cancel the insert to fix the settings";
                    // Check if there is an active trap
                    SQL = "SELECT C.CollectionTaskID, C.DisplayText , CONVERT(varchar(10), C.TaskStart, 120) AS TaskStart " +
                        "FROM CollectionTask AS C " +
                        "INNER JOIN Task AS T ON C.TaskID = T.TaskID " +
                        "WHERE (T.Type = N'Trap') " +
                        "AND(C.TaskStart IS NULL OR C.TaskStart <= GETDATE()) " +
                        "AND(C.TaskEnd IS NULL ) " +
                        "AND (C.CollectionID = " + CollectionID.ToString() + ")";
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTraps, ref Error);
                    string DateEnd = "";
                    if (dtTraps.Rows.Count > 0)
                    {
                        Message += "\r\n\r\nThere is an active trap that has been installed at " + dtTraps.Rows[0]["TaskStart"].ToString() + "\r\nThe new inserted trap will be set as removed at " + dtTraps.Rows[0]["TaskStart"].ToString() + ".\r\n";
                        DateEnd = dtTraps.Rows[0]["TaskStart"].ToString();
                        //switch (System.Windows.Forms.MessageBox.Show(Message, "Active trap present", System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question))
                        //{
                        //    case System.Windows.Forms.DialogResult.Yes:
                        //        break;
                        //    case System.Windows.Forms.DialogResult.No:
                        //        break;
                        //    default:
                        //        InsertCanceled = true;
                        //        return -1;
                        //}
                    }
                    Message += "\r\nDo you want to add a new trap containing this pest";
                    if (System.Windows.Forms.MessageBox.Show(Message, "No trap", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        // find monitoring for Collection
                        int MonitoringID = GetMonitoringID(CollectionID, DateSQL, ref Error);
                        if (Error.Length > 0 || MonitoringID == -1)
                            TrapID = -1;
                        else
                        {
                            // find task for trap
                            int TrapTaskID = GetTaskIdWithinParent("Trap", "Monitoring", ref Error, MonitoringID); //                           GetTrapTaskID(MonitoringID, ref Error);
                            if (Error.Length > 0 || TrapTaskID == -1)
                                TrapID = -1;
                            else
                            {
                                string TrapTitle = "";
                                DiversityWorkbench.Forms.FormGetString fTrap = new DiversityWorkbench.Forms.FormGetString("New trap", "Please enter the name for the new trap", "", DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Trap));
                                fTrap.ShowDialog();
                                if (fTrap.DialogResult == System.Windows.Forms.DialogResult.OK && fTrap.String.Length > 0)
                                {
                                    TrapTitle = fTrap.String;
                                    SQL = "INSERT INTO CollectionTask (CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, DisplayText, TaskStart, ";
                                    if (DateEnd.Length > 0) SQL += "TaskEnd, ";
                                    SQL += "ResponsibleAgent, ResponsibleAgentURI) " +
                                        "VALUES(" + MonitoringID.ToString() + ", " + CollectionID.ToString() + ", " + TrapTaskID.ToString() + ", 1, '" + TrapTitle + "', " + DateSQL + ", ";
                                    if (DateEnd.Length > 0) SQL += "CONVERT(DATETIME, '" + DateEnd + " 00:00:00', 120), ";
                                    SQL += "'" + Responsible + "', '" + ResponsibleURL + "') " +
                                        "; SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];";
                                    int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Error), out TrapID);
                                    if (TrapID == 0)
                                    {
                                        SQL = "SELECT MAX(CollectionTaskID) FROM CollectionTask WHERE CollectionTaskParentID = " + MonitoringID.ToString() + " AND TaskID = " + TrapTaskID.ToString() + " AND DisplayOrder = 1";
                                        int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out TrapID);
                                    }
                                    TrapAdded = true;
                                }
                                else
                                {
                                    Error = "No trap available";
                                }
                            }
                        }
                        //SQL = "SELECT C.CollectionTaskID, C.DisplayText " +
                        //    "FROM CollectionTask AS C INNER JOIN " +
                        //    "Task AS T ON C.TaskID = T.TaskID INNER JOIN " +
                        //    "dbo.CollectionHierarchySuperior(" + CollectionID.ToString() + ") S ON S.CollectionID = C.CollectionID " +
                        //    "AND (T.Type = N'Monitoring') " +
                        //    "AND(C.TaskStart IS NULL OR C.TaskStart <= " + DateSQL + ") " +
                        //    "AND(C.TaskEnd IS NULL OR C.TaskEnd >= " + DateSQL + ") " +
                        //    "AND (C.DisplayText <> '')";
                        //System.Data.DataTable dtMonitor = new System.Data.DataTable();
                        //DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtMonitor, ref Error);
                        //if (Error.Length > 0)
                        //    return -1;
                        //if (dtMonitor.Rows.Count == 1)
                        //{
                        //    if (!int.TryParse(dtMonitor.Rows[0][0].ToString(), out MonitoringID))
                        //        TrapID = -1;
                        //}
                        //else if (dtMonitor.Rows.Count > 1)
                        //{
                        //    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtMonitor, "DisplayText", "CollectionTaskID", "Please select a monitoring from the list", "Monitoring", "", false, true, true, DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Graph));
                        //    f.ShowDialog();
                        //    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                        //    {
                        //        if (!int.TryParse(f.SelectedValue, out MonitoringID))
                        //            TrapID = -1;
                        //        else
                        //        {
                        //            int TrapTaskID = -1;
                        //            // try to find the trap task
                        //            SQL = "SELECT T.DisplayText, T.TaskID " +
                        //                "FROM[dbo].[CollectionTask] C " +
                        //                "INNER JOIN Task M ON  C.TaskID = M.TaskID AND M.Type = 'Monitoring' AND C.CollectionTaskID = " + MonitoringID.ToString() + " " +
                        //                "INNER JOIN Task T ON T.TaskParentID = M.TaskID AND T.Type = 'Trap'";
                        //            System.Data.DataTable dtTrapTask = new System.Data.DataTable();
                        //            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTrapTask, ref Error);
                        //            if (Error.Length > 0)
                        //                return -1;
                        //            if (dtTrapTask.Rows.Count == 1)
                        //            {
                        //                int.TryParse(dtTrapTask.Rows[0][0].ToString(), out TrapTaskID);
                        //            }
                        //            else if (dtTrapTask.Rows.Count > 1)
                        //            {
                        //                DiversityWorkbench.Forms.FormGetStringFromList fTT = new DiversityWorkbench.Forms.FormGetStringFromList(dtMonitor, "DisplayText", "TaskID", "Please select a trap from the list", "Trap", "", false, true, true, DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Trap));
                        //                fTT.ShowDialog();
                        //                if (fTT.DialogResult == System.Windows.Forms.DialogResult.OK)
                        //                {
                        //                    if (int.TryParse(fTT.SelectedValue, out TrapTaskID))
                        //                    {
                        //                        // done
                        //                    }
                        //                }
                        //            }
                        //            else
                        //            {
                        //                System.Windows.Forms.MessageBox.Show("So far no traps had been defined for monitoring of the selected collection. Please turn to your administrator to define a trap for the monitoring task");
                        //                TrapID = -1;
                        //            }
                        //            if (TrapTaskID > -1)
                        //            {
                        //                string TrapTitle = "";
                        //                DiversityWorkbench.Forms.FormGetString fTrap = new DiversityWorkbench.Forms.FormGetString("New trap", "Please enter the name for the new trap", "", DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Trap));
                        //                fTrap.ShowDialog();
                        //                if (fTrap.DialogResult == System.Windows.Forms.DialogResult.OK && fTrap.String.Length > 0)
                        //                {
                        //                    TrapTitle = fTrap.String;
                        //                    SQL = "INSERT INTO CollectionTask (CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, DisplayText, TaskStart, ResponsibleAgent, ResponsibleAgentURI) " +
                        //                        "VALUES(" + MonitoringID.ToString() + ", " + CollectionID.ToString() + ", " + TrapTaskID.ToString() + ", 1, '" + TrapTitle + "', " + DateSQL + ", '" + Responsible + "', '" + ResponsibleURL + "') " +
                        //                        "; SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];";
                        //                    int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out TrapID);
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    System.Windows.Forms.MessageBox.Show("So far no monitoring has been defined for the selected collection. Please turn to your administrator to define a monitoring task");
                        //    TrapID = -1;
                        //}
                    }
                    else
                    {
                        TrapID = -1;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return TrapID;
        }

        private static int getRecordTaskID(IPM.RecordingTarget recordingTarget, int CollectionID, int TrapCollectionTaskID, ref string Error)
        {
            int ID = 0;
            string TaskType = "'Pest'";
            if (recordingTarget == RecordingTarget.TrapBycatch)
                TaskType = "'Bycatch'";
            // TaskID for the Records in the trap
            string SQL = "SELECT MIN(P.TaskID) " +
                "FROM CollectionTask AS C INNER JOIN " +
                "Task AS T ON C.TaskID = T.TaskID " +
                "INNER JOIN Task P ON P.TaskParentID = T.TaskID AND P.ModuleType = 'DiversityTaxonNames' AND P.Type = " + TaskType + //AND P.ParentCode = 'IPM' " +
                " WHERE (C.CollectionID = " + CollectionID.ToString() + ") "; 
            if (TrapCollectionTaskID > -1)
                SQL+= " AND C.CollectionTaskID = " + TrapCollectionTaskID.ToString() + "";
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out ID))
            {
                // try to get a task within the monitoring instead of the trap
                SQL = "SELECT MIN(P.TaskID) " +
                    "FROM CollectionTask AS C " +
                    "INNER JOIN Task AS T ON C.TaskID = T.TaskID " +
                    "INNER JOIN Task M ON T.TaskParentID = M.TaskID AND M.Type = 'Monitoring' " +
                    "INNER JOIN Task I ON M.TaskParentID = I.TaskID AND I.Type = 'IPM' " +
                    "INNER JOIN Task P ON P.TaskParentID = M.TaskID AND P.ModuleType = 'DiversityTaxonNames' AND P.Type = " + TaskType +
                    " WHERE C.CollectionTaskID = " + TrapCollectionTaskID.ToString() + " AND (C.CollectionID = " + CollectionID.ToString() + ")";
                if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out ID)) 
                {
                    // nothing found - insert the missing Task
                    SQL = "INSERT INTO Task (TaskParentID, DisplayText, Type, ModuleTitle, ModuleType, SpecimenPartType, DateType, DateBeginType, NumberType, DescriptionType, NotesType, ResponsibleType) " +
                    " SELECT C.TaskID, " + TaskType  + ", " + TaskType  + ", " + TaskType  + ", 'DiversityTaxonNames', 'Part', 'Date', 'Date', 'Count', 'Vernacular name', 'Notes', 'Responsible' " +
                    "FROM CollectionTask AS C WHERE C.CollectionTaskID = " + TrapCollectionTaskID.ToString() + " AND C.CollectionID = " + CollectionID.ToString() + ";  SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];";
                    if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Error), out ID))
                    {
                        Error = "Failed to insert missing task: " + Error;
                    }
                }
            }
            return ID;
        }

        #endregion

        //private static int getPestTaskID(int CollectionID, int TrapCollectionTaskID, ref string Error)
        //{
        //    int ID = 0;
        //    // TaskID for the Pests in the trap
        //    string SQL = "SELECT MIN(P.TaskID) " +
        //        "FROM CollectionTask AS C INNER JOIN " +
        //        "Task AS T ON C.TaskID = T.TaskID " +
        //        "INNER JOIN Task P ON P.TaskParentID = T.TaskID AND P.ModuleType = 'DiversityTaxonNames' AND P.Type = 'Pest' " +
        //        "WHERE C.CollectionTaskID = " + TrapCollectionTaskID.ToString() + " AND (C.CollectionID = " + CollectionID.ToString() + ")";
        //    if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out ID))
        //    {
        //        // nothing found - insert the missing Task
        //        SQL = "INSERT INTO Task (TaskParentID, DisplayText, Type, ModuleTitle, ModuleType, SpecimenPartType, DateType, DateBeginType, NumberType, DescriptionType, NotesType, ResponsibleType) " +
        //            " SELECT C.TaskID, 'Pest', 'Pest', 'Pest', 'DiversityTaxonNames', 'Part', 'Date', 'Date', 'Count', 'Vernacular name', 'Notes', 'Responsible' " +
        //            "FROM CollectionTask AS C WHERE C.CollectionTaskID = " + TrapCollectionTaskID.ToString() + " AND C.CollectionID = " + CollectionID.ToString() + ";  SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];";
        //        if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Error), out ID))
        //        {
        //            Error = "Failed to insert missing task: " + Error;
        //        }
        //    }
        //    return ID;
        //}

        private static int GetMonitoringID(int CollectionID, string DateSQL, ref string Error)
        {
            // find MonitoringID for Collection
            int MonitoringID = -1;
            string SQL = "SELECT C.CollectionTaskID, case when C.DisplayText <> '' then C.DisplayText else T.DisplayText end AS DisplayText " +
                "FROM CollectionTask AS C INNER JOIN " +
                "Task AS T ON C.TaskID = T.TaskID INNER JOIN " +
                "dbo.CollectionHierarchySuperior(" + CollectionID.ToString() + ") S ON S.CollectionID = C.CollectionID " +
                "AND (T.Type = N'Monitoring') ";
            if (DateSQL.Length > 0)
                SQL += "AND(C.TaskStart IS NULL OR C.TaskStart <= " + DateSQL + ") " +
                "AND(C.TaskEnd IS NULL OR C.TaskEnd >= " + DateSQL + ") ";
            SQL += "AND (C.DisplayText <> '' OR T.DisplayText <> '')";
            System.Data.DataTable dtMonitor = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtMonitor, ref Error);
            if (Error.Length > 0)
                return -1;
            if (dtMonitor.Rows.Count == 1)
            {
                if (!int.TryParse(dtMonitor.Rows[0][0].ToString(), out MonitoringID))
                    Error = "Conversion for MonitoringID failed";
            }
            else if (dtMonitor.Rows.Count > 1)
            {
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtMonitor, "DisplayText", "CollectionTaskID", "Please select a monitoring from the list", "Monitoring", "", false, true, true, DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Graph));
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    if (!int.TryParse(f.SelectedValue, out MonitoringID))
                        Error = "Selection for MonitoringID failed";
                }
            }
            else
            {
                if (System.Windows.Forms.MessageBox.Show("So far no monitoring has been defined for the selected collection. Do you want to create a monitoring task?", "Create monitoring task", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    MonitoringID = CreateMonitoringCollectionTask(CollectionID, ref Error);
                }
            }
            return MonitoringID;
        }

        private static int CreateMonitoringCollectionTask(int CollectionID, ref string Error)
        {
            int MonitoringID = -1;
            int TaskID = GetMonitoringTaskID(ref Error);
            int SuperiorCollectionID = GetSuperiorCollectionID(CollectionID, ref Error);
            if (TaskID > -1 && SuperiorCollectionID > -1)
            {
                string SQL = "INSERT INTO CollectionTask (CollectionID, DisplayText, TaskID) VALUES (" + SuperiorCollectionID.ToString() + ", 'Monitoring', " + TaskID.ToString() + ")";
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error))
                {
                    SQL = "SELECT MAX(T.CollectionTaskID) " +
                        "FROM CollectionTask AS T WHERE (T.TaskID = " + TaskID.ToString() + ") AND CollectionID = " + SuperiorCollectionID.ToString();
                    if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out MonitoringID))
                        Error = "Conversion for CollectionTaskID failed";
                }
            }
            return MonitoringID;
        }

        private static int GetSuperiorCollectionID(int CollectionID, ref string Error)
        {
            int SuperiorCollectionID = -1;
            string SQL = "SELECT S.CollectionID " +
                "FROM dbo.CollectionHierarchySuperior(" + CollectionID.ToString() + ") S WHERE S.LocationParentID IS NULL ";
            if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out SuperiorCollectionID))
                Error = "Conversion for SuperiorCollectionID failed";
            return SuperiorCollectionID;
        }

        private static int GetMonitoringTaskID(ref string Error)
        {
            // find TaskID for Monitoring
            int TaskID = -1;
            string SQL = "SELECT MAX(T.TaskID) " +
                "FROM Task AS T WHERE (T.Type = N'Monitoring') ";
            if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out TaskID))
            {
                TaskID = -1;
            }
            if (TaskID == -1)
            {
                //if (System.Windows.Forms.MessageBox.Show("So far no monitoring task has been defined. Do you want to create a monitoring task?", "Create monitoring task", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    TaskID = CreateMonitoringTask(ref Error);
                }
            }
            return TaskID;
        }

        private static int CreateMonitoringTask(ref string Error)
        {
            int TaskID = -1;
            string SQL = "INSERT INTO Task (DisplayText, Type) VALUES('Monitoring', 'Monitoring')";
            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error))
            {
                SQL = "SELECT MAX(T.TaskID) " +
                    "FROM Task AS T WHERE (T.Type = N'Monitoring') ";
                if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out TaskID))
                {
                    Error = "Conversion for TaskID failed";
                    TaskID = -1;
                }
            }
            return TaskID;
        }

        //private static int GetTrapTaskID(int MonitoringID, ref string Error)
        //{
        //    return GetTaskIdWithinMonitoring("Trap", MonitoringID, ref Error);

        //    //int TrapTaskID = -1;
        //    //// try to find the trap task
        //    //string SQL = "SELECT T.DisplayText, T.TaskID " +
        //    //    "FROM[dbo].[CollectionTask] C " +
        //    //    "INNER JOIN Task M ON  C.TaskID = M.TaskID AND M.Type = 'Monitoring' AND C.CollectionTaskID = " + MonitoringID.ToString() + " " +
        //    //    "INNER JOIN Task T ON T.TaskParentID = M.TaskID AND T.Type = 'Trap'";
        //    //System.Data.DataTable dtTrapTask = new System.Data.DataTable();
        //    //string ErrorLocal = "";
        //    //DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTrapTask, ref ErrorLocal);
        //    //if (ErrorLocal.Length > 0)
        //    //    return -1;
        //    //if (dtTrapTask.Rows.Count == 1)
        //    //{
        //    //    int.TryParse(dtTrapTask.Rows[0][1].ToString(), out TrapTaskID);
        //    //}
        //    //else if (dtTrapTask.Rows.Count > 1)
        //    //{
        //    //    DiversityWorkbench.Forms.FormGetStringFromList fTT = new DiversityWorkbench.Forms.FormGetStringFromList(dtTrapTask, "DisplayText", "TaskID", "Please select a trap from the list", "Trap", "", false, true, true, DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Trap));
        //    //    fTT.ShowDialog();
        //    //    if (fTT.DialogResult == System.Windows.Forms.DialogResult.OK)
        //    //    {
        //    //        if (!int.TryParse(fTT.SelectedValue, out TrapTaskID))
        //    //        {
        //    //            Error = "Retrieval of Task for Trap failed";
        //    //        }
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    if (System.Windows.Forms.MessageBox.Show("So far no traps had been defined for monitoring of the selected collection. Do you want to create one?", "Create trap task", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
        //    //    {
        //    //        TrapTaskID = CreateTrapTask(ref ErrorLocal);
        //    //    }
        //    //    else
        //    //        Error = "Failed to retrieve task for Trap";
        //    //}
        //    //return TrapTaskID;
        //}

        //private static int CreateTrapTask(ref string Error)
        //{
        //    return CreateTaskWithinMonitoring("Trap", ref Error);

        //    //int TaskID = -1;
        //    //int TaskParentID  = GetMonitoringTaskID(ref Error);
        //    ////SQL = "SELECT TaskID FROM  (TaskParentID, DisplayText, Type) VALUES(" + MonitoringID.ToString() + ", 'Trap', 'Trap')";
        //    //string SQL = "INSERT INTO Task (TaskParentID, DisplayText, Type) VALUES(" + TaskParentID.ToString() + ", 'Trap', 'Trap')";
        //    //if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error))
        //    //{
        //    //    SQL = "SELECT MAX(T.TaskID) " +
        //    //        "FROM Task AS T WHERE (T.Type = N'Trap') AND TaskParentID = " + TaskParentID.ToString();
        //    //    if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out TaskID))
        //    //    {
        //    //        Error = "Conversion for TaskID failed";
        //    //        TaskID = -1;
        //    //    }
        //    //}
        //    //return TaskID;
        //}

        #region Reading
        public static System.Data.DataTable ReadTaxonValues(ref string Date, TaxonSource source)
        {
            System.Data.DataTable dtPests = new System.Data.DataTable();
            try
            {
                string CollectionIDList = Tasks.IPM.CollectionIDList();
                if (Date == null || Date.Length == 0)
                    Date = System.DateTime.Now.ToString("yyyy-MM-dd");
                // getting the pests
                string SQL = "SELECT CT.CollectionID, CT.ModuleUri, CT.NumberValue, CT.Notes, CT.Result " +
                    "FROM dbo.Collection C " +
                    "INNER JOIN CollectionTask CT ON C.CollectionID IN (" + CollectionIDList + ") AND C.CollectionID = CT.CollectionID AND(CAST(CT.TaskStart AS VARCHAR(10)) = CAST(CONVERT(DATETIME, '" + Date + " 00:00:00', 102) AS varchar(10))) " +
                    "INNER JOIN Task T ON T.TaskID = CT.TaskID AND T.ModuleType = 'DiversityTaxonNames' AND T.Type = 'Pest' " + // + source.ToString() + "' " +
                    "AND C.Type = 'Trap' ";
                string Uris = "";
                System.Collections.Specialized.StringCollection NameUris = IPM.NameUris(source);// new System.Collections.Specialized.StringCollection();
                if (NameUris != null && NameUris.Count > 0)
                {
                    foreach (string U in NameUris)
                    {
                        if (Uris.Length > 0)
                            Uris += ", ";
                        Uris += "'" + U + "'";
                    }
                    SQL += " AND CT.ModuleUri IN (" + Uris + ") ";
                }
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtPests);

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return dtPests;
        }

        public static System.Data.DataTable ReadRecordings(ref string Date, RecordingTarget recordingTarget)
        {
            System.Data.DataTable dtRecords = new System.Data.DataTable();
            string source = "";
            try
            {
                switch(recordingTarget)
                {
                    case RecordingTarget.TrapBycatch:
                        source = "Bycatch";
                        break;
                    default:
                        source = "Pest";
                        break;
                }
                string CollectionIDList = Tasks.IPM.CollectionIDList();
                if (Date == null || Date.Length == 0)
                    Date = System.DateTime.Now.ToString("yyyy-MM-dd");
                // getting the records
                string SQL = "SELECT CT.CollectionID, CT.ModuleUri, CT.NumberValue, CT.Notes, CT.Result ";
                switch (recordingTarget)
                {
                    case RecordingTarget.SpecimenPest:
                        SQL += ", CT.CollectionSpecimenID, CT.SpecimenPartID ";
                        break;
                    case RecordingTarget.TransactionPest:
                        SQL += ", CT.TransactionID ";
                        break;
                }
                SQL += "FROM dbo.Collection C " +
                    "INNER JOIN CollectionTask CT ON C.CollectionID IN (" + CollectionIDList + ") AND C.CollectionID = CT.CollectionID AND(CAST(CT.TaskStart AS VARCHAR(10)) = CAST(CONVERT(DATETIME, '" + Date + " 00:00:00', 120) AS varchar(10))) " +
                    "INNER JOIN Task T ON T.TaskID = CT.TaskID AND T.ModuleType = 'DiversityTaxonNames' AND T.Type = '" + source + "' ";
                switch (recordingTarget)
                {
                    case RecordingTarget.TrapPest:
                    case RecordingTarget.TrapBycatch:
                        SQL += "AND C.Type =  'trap' AND CT.CollectionSpecimenID IS NULL AND CT.SpecimenPartID IS NULL AND CT.TransactionID IS NULL ";
                        break;
                    case RecordingTarget.CollectionPest:
                        SQL += "AND C.Type <> 'trap' AND CT.CollectionSpecimenID IS NULL AND CT.SpecimenPartID IS NULL AND CT.TransactionID IS NULL ";
                        break;
                    case RecordingTarget.SpecimenPest:
                        SQL += "AND NOT CT.CollectionSpecimenID IS NULL AND NOT CT.SpecimenPartID IS NULL ";
                        break;
                    case RecordingTarget.TransactionPest:
                        SQL += "AND NOT CT.TransactionID IS NULL ";
                        break;
                }
                string Uris = "";
                System.Collections.Specialized.StringCollection NameUris = IPM.NameUris(recordingTarget);
                if (NameUris != null && NameUris.Count > 0)
                {
                    foreach (string U in NameUris)
                    {
                        if (Uris.Length > 0)
                            Uris += ", ";
                        Uris += "'" + U + "'";
                    }
                    SQL += " AND CT.ModuleUri IN (" + Uris + ") ";
                }
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtRecords);

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return dtRecords;
        }

        #endregion


        //public static System.Data.DataTable ReadRecordValues(ref string Date, TaxonSource source, MonitoringTarget target)
        //{
        //    System.Data.DataTable dtRecords = new System.Data.DataTable();
        //    try
        //    {
        //        string CollectionIDList = Tasks.IPM.CollectionIDList();
        //        if (Date == null || Date.Length == 0)
        //            Date = System.DateTime.Now.ToString("yyyy-MM-dd");
        //        // getting the records
        //        string SQL = "SELECT CT.CollectionID, CT.ModuleUri, CT.NumberValue, CT.Notes, CT.Result " +
        //            "FROM dbo.Collection C " +
        //            "INNER JOIN CollectionTask CT ON C.CollectionID IN (" + CollectionIDList + ") AND C.CollectionID = CT.CollectionID AND(CAST(CT.TaskStart AS VARCHAR(10)) = CAST(CONVERT(DATETIME, '" + Date + " 00:00:00', 102) AS varchar(10))) " +
        //            "INNER JOIN Task T ON T.TaskID = CT.TaskID AND T.ModuleType = 'DiversityTaxonNames' AND T.Type = '" + source.ToString() + "' ";
        //        switch(target)
        //        {
        //            case MonitoringTarget.Trap:
        //                SQL += "AND C.Type = 'trap' ";
        //                break;
        //            case MonitoringTarget.Collection:
        //                SQL += "AND C.Type <> 'trap' ";
        //                break;
        //            case MonitoringTarget.Specimen:
        //                SQL += "AND NOT C.CollectionSpecimenID IS NULL AND NOT C.SpecimenPartID IS NULL ";
        //                break;
        //            case MonitoringTarget.Transaction:
        //                SQL += "AND NOT C.TransactionID IS NULL ";
        //                break;
        //        }
        //        string Uris = "";
        //        System.Collections.Specialized.StringCollection NameUris = IPM.NameUris(source);
        //        if (NameUris != null && NameUris.Count > 0)
        //        {
        //            foreach (string U in NameUris)
        //            {
        //                if (Uris.Length > 0)
        //                    Uris += ", ";
        //                Uris += "'" + U + "'";
        //            }
        //            SQL += " AND CT.ModuleUri IN (" + Uris + ") ";
        //        }
        //        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtRecords);

        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return dtRecords;
        //}

        public static System.Collections.Specialized.StringCollection NameUris(RecordingTarget recordingTarget)
        {
            System.Collections.Specialized.StringCollection NameUris = new System.Collections.Specialized.StringCollection();
            switch (recordingTarget)
            {
                case RecordingTarget.TrapBycatch:
                    if (DiversityCollection.Tasks.Settings.Default.BycatchNameUris == null)
                        DiversityCollection.Tasks.Settings.Default.BycatchNameUris = new System.Collections.Specialized.StringCollection();
                    NameUris = DiversityCollection.Tasks.Settings.Default.BycatchNameUris;
                    break;
                default:
                    if (DiversityCollection.Tasks.Settings.Default.PestNameUris == null)
                        DiversityCollection.Tasks.Settings.Default.PestNameUris = new System.Collections.Specialized.StringCollection();
                    NameUris = DiversityCollection.Tasks.Settings.Default.PestNameUris;
                    break;
            }
            return NameUris;
        }


        public static System.Collections.Specialized.StringCollection NameUris(TaxonSource source)
        {
            System.Collections.Specialized.StringCollection NameUris = new System.Collections.Specialized.StringCollection();
            switch (source)
            {
                case TaxonSource.Bycatch:
                    if (DiversityCollection.Tasks.Settings.Default.BycatchNameUris == null)
                        DiversityCollection.Tasks.Settings.Default.BycatchNameUris = new System.Collections.Specialized.StringCollection();
                    NameUris = DiversityCollection.Tasks.Settings.Default.BycatchNameUris;
                    break;
                default:
                    if (DiversityCollection.Tasks.Settings.Default.PestNameUris == null)
                        DiversityCollection.Tasks.Settings.Default.PestNameUris = new System.Collections.Specialized.StringCollection();
                    NameUris = DiversityCollection.Tasks.Settings.Default.PestNameUris;
                    break;
            }
            return NameUris;
        }

        #endregion

        #region Treatment

        public enum Treatment { Beneficial, Cleaning, Damage, Freezing, Gas, Repair, Poison }

        public static bool AddTreatment(int CollectionID, Treatment treatment)
        {
            bool OK = false;
            return OK;
        }

        public static System.Data.DataTable Treatments(int CollectionID, Treatment treatment)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string SQL = "";
            switch (treatment)
            {
                case Tasks.IPM.Treatment.Beneficial:
                    SQL = "SELECT T.HierarchyDisplayText + ' - ' + R.Result AS " + treatment.ToString() + ", T.TaskID, R.Result " +
                        "FROM [dbo].[TaskHierarchyAll] () T INNER JOIN [dbo].[TaskResult] R ON T.TaskID = R.TaskID AND T.Type = 'Beneficial organism'";
                    break;
                case Tasks.IPM.Treatment.Cleaning:
                    SQL = "SELECT T.HierarchyDisplayText + ' - ' + R.Result AS " + treatment.ToString() + ", T.TaskID, R.Result " +
                        "FROM [dbo].[TaskHierarchyAll] () T INNER JOIN [dbo].[TaskResult] R ON T.TaskID = R.TaskID AND T.Type = 'Cleaning'";
                    break;
            }
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            if (dt.Rows.Count == 0 && System.Windows.Forms.MessageBox.Show("No " + treatment.ToString().ToLower() + " defined so far. Do you want to define one?", "Missing " + treatment.ToString().ToLower(), System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {

            }
            return dt;
        }

        public static System.Data.DataTable Treatments(Treatment treatment)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string SQL = "";
            switch (treatment)
            {
                case Tasks.IPM.Treatment.Beneficial:
                    SQL = "SELECT T.HierarchyDisplayText + ' - ' + R.Result AS " + treatment.ToString() + ", T.TaskID, R.Result " +
                        "FROM [dbo].[TaskHierarchyAll] () T INNER JOIN [dbo].[TaskResult] R ON T.TaskID = R.TaskID AND T.Type = 'Beneficial organism'";
                    break;
                case Tasks.IPM.Treatment.Cleaning:
                    SQL = "SELECT T.HierarchyDisplayText + ' - ' + R.Result AS " + treatment.ToString() + ", T.TaskID, R.Result " +
                        "FROM [dbo].[TaskHierarchyAll] () T INNER JOIN [dbo].[TaskResult] R ON T.TaskID = R.TaskID AND T.Type = 'Cleaning'";
                    break;
                default:
                    SQL = "SELECT T.HierarchyDisplayText + ' - ' + R.Result AS " + treatment.ToString() + ", T.TaskID, R.Result " +
                        "FROM [dbo].[TaskHierarchyAll] () T INNER JOIN [dbo].[TaskResult] R ON T.TaskID = R.TaskID AND T.Type = '" + treatment.ToString() + "'";
                    break;
            }
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            if (dt.Rows.Count == 0 && System.Windows.Forms.MessageBox.Show("No " + treatment.ToString().ToLower() + " defined so far. Do you want to define one?", "Missing " + treatment.ToString().ToLower(), System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                string Error = "";
                int TaskID = GetTreatmentTaskID(treatment, ref Error);
                if (TaskID > -1)
                {
                    SQL = "SELECT T.HierarchyDisplayText + ' - ' + R.Result AS " + treatment.ToString() + ", T.TaskID, R.Result " +
                        "FROM [dbo].[TaskHierarchyAll] () T INNER JOIN [dbo].[TaskResult] R ON T.TaskID = R.TaskID AND T.TaskID = " + TaskID.ToString();
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                }
            }
            return dt;
        }

        private static int CreateTreatmentCollectionTask(int CollectionID, Treatment treatment, ref string Error)
        {
            int TreatmentID = -1;
            int TaskID = GetTreatmentTaskID(treatment, ref Error);
            int SuperiorCollectionID = GetSuperiorCollectionID(CollectionID, ref Error);
            if (TaskID > -1 && SuperiorCollectionID > -1)
            {
                string SQL = "INSERT INTO CollectionTask (CollectionID, DisplayText, TaskID) VALUES (" + SuperiorCollectionID.ToString() + ", 'Treatment', " + TaskID.ToString() + ")";
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error))
                {
                    SQL = "SELECT MAX(T.CollectionTaskID) " +
                        "FROM CollectionTask AS T WHERE (T.TaskID = " + TaskID.ToString() + ") AND CollectionID = " + SuperiorCollectionID.ToString();
                    if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out TreatmentID))
                        Error = "Conversion for CollectionTaskID failed";
                }
            }
            return TreatmentID;
        }

        private static int GetTreatmentCollectionTaskID(int CollectionID, Treatment treatment, ref string Error)
        {
            // find TreatmentID for Collection
            int TreatmentID = -1;
            string SQL = "SELECT C.CollectionTaskID, case when C.DisplayText <> '' then C.DisplayText else T.DisplayText end AS DisplayText " +
                "FROM CollectionTask AS C INNER JOIN " +
                "Task AS T ON C.TaskID = T.TaskID INNER JOIN " +
                "dbo.CollectionHierarchySuperior(" + CollectionID.ToString() + ") S ON S.CollectionID = C.CollectionID " +
                "AND (T.Type = N'Treatment') " +
                "AND (C.DisplayText <> '' OR T.DisplayText <> '')";
            System.Data.DataTable dtTreatment = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTreatment, ref Error);
            if (Error.Length > 0)
                return -1;
            if (dtTreatment.Rows.Count == 1)
            {
                if (!int.TryParse(dtTreatment.Rows[0][0].ToString(), out TreatmentID))
                    Error = "Conversion for TreatmentID failed";
            }
            else if (dtTreatment.Rows.Count > 1)
            {
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtTreatment, "DisplayText", "CollectionTaskID", "Please select a treatment from the list", "Treatment", "", false, true, true, DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Graph));
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    if (!int.TryParse(f.SelectedValue, out TreatmentID))
                        Error = "Selection for TreatmentID failed";
                }
            }
            else
            {
                if (System.Windows.Forms.MessageBox.Show("So far no treatment has been defined for the selected collection. Do you want to create a treatment task?", "Create treatment task", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    TreatmentID = CreateTreatmentCollectionTask(CollectionID, treatment, ref Error);
                }
            }
            return TreatmentID;
        }

        private static int GetTreatmentTaskID(Treatment treatment, ref string Error)
        {
            // find TaskID for Treatment
            int TaskID = -1;
            string SQL = "SELECT MAX(T.TaskID) " +
                "FROM Task AS T WHERE (T.Type = N'Treatment') ";
            if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out TaskID))
            {
                TaskID = -1;
            }
            if (TaskID == -1)
            {
                TaskID = CreateTreatmentTask(treatment, ref Error);
            }
            return TaskID;
        }


        private static int CreateTreatmentTask(Treatment treatment, ref string Error)
        {
            int TaskID = -1;
            string SQL = "INSERT INTO Task (DisplayText, Type) VALUES('Treatment', 'Treatment')";
            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error))
            {
                SQL = "SELECT MAX(T.TaskID) " +
                    "FROM Task AS T WHERE (T.Type = N'Treatment') ";
                if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out TaskID))
                {
                    Error = "Conversion for TaskID failed";
                    TaskID = -1;
                }
                else
                {
                    switch (treatment)
                    {
                        case Treatment.Beneficial:
                            SQL = "INSERT INTO Task (TaskParentID, DisplayText, Type" +
                                ", ModuleTitle, ModuleType, ResultType" +
                                ", DateType, DateBeginType, DateEndType" +
                                ", NumberType, NotesType, ResponsibleType, MetricUnit) " +
                                "VALUES (" + TaskID.ToString() + ",'" + treatment.ToString() + "', 'Beneficial organism'" +
                                ", 'Taxon', 'DiversityTaxonNames', 'provider'" +
                                ", 'Date from to', 'From', 'Until'" +
                                ", 'Number', 'Notes', 'Responsible', 'Units')";
                            break;
                        case Treatment.Cleaning:
                            SQL = "INSERT INTO Task (TaskParentID, DisplayText, Type" +
                                ", ResultType" +
                                ", DateType, DateBeginType, DateEndType" +
                                ", NotesType, ResponsibleType) " +
                                "VALUES (" + TaskID.ToString() + ",'" + treatment.ToString() + "', 'Cleaning'" +
                                ", 'Type of cleaning'" +
                                ", 'Date from to', 'From', 'Until'" +
                                ", 'Notes', 'Responsible')";
                            break;
                        default:
                            SQL = "INSERT INTO Task (TaskParentID, DisplayText, Type" +
                                ", ResultType" +
                                ", DateType, DateBeginType, DateEndType" +
                                ", NotesType, ResponsibleType) " +
                                "VALUES (" + TaskID.ToString() + ",'" + treatment.ToString() + "', '" + treatment.ToString() + "'" +
                                ", 'Type of " + treatment.ToString() + "'" +
                                ", 'Date from to', 'From', 'Until'" +
                                ", 'Notes', 'Responsible')";
                            break;
                    }
                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error))
                    {
                        SQL = "SELECT MAX(T.TaskID) " +
                            "FROM Task AS T WHERE (T.Type = N'" + treatment.ToString() + "') AND TaskParentID = " + TaskID.ToString();
                        if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out TaskID))
                        {
                            switch (treatment)
                            {
                                case Treatment.Beneficial:
                                    if (_Beneficials.Count > 0)
                                    {
                                        System.Collections.Generic.Dictionary<string, string> BeneDict = new Dictionary<string, string>();
                                        foreach (System.Collections.Generic.KeyValuePair<string, Taxon> KV in _Beneficials)
                                            BeneDict.Add(KV.Key, KV.Value.VernacularName);
                                        DiversityWorkbench.Forms.FormGetStringFromList fbeneficial = new DiversityWorkbench.Forms.FormGetStringFromList(BeneDict, "Cleaning", "Please enter the type of the cleaning", true, DiversityCollection.Specimen.Image("animal"));
                                        fbeneficial.ShowDialog();
                                        if (fbeneficial.DialogResult == System.Windows.Forms.DialogResult.OK)
                                        {
                                            SQL = "INSERT INTO TaskResult (TaskID, Result, URI, Description) " +
                                                "VALUES (" + TaskID.ToString() + ",'" + treatment.ToString() + "', 'Beneficial organism'" +
                                                ", 'Taxon', 'DiversityTaxonNames', 'provider'" +
                                                ", 'Date from to', 'From', 'Until'" +
                                                ", 'Number', 'Notes', 'Responsible', 'Units')";
                                        }
                                    }
                                    break;
                                case Treatment.Cleaning:
                                    DiversityWorkbench.Forms.FormGetString fcleaning = new DiversityWorkbench.Forms.FormGetString("Cleaning", "Please enter the type of the cleaning", "", Specimen.Image("Cleaning"));
                                    fcleaning.ShowDialog();
                                    if (fcleaning.DialogResult == System.Windows.Forms.DialogResult.OK && fcleaning.String.Length > 0)
                                    {
                                        SQL = "INSERT INTO TaskResult (TaskID, Result) " +
                                            "VALUES (" + TaskID.ToString() + ",'" + fcleaning.String + "')";
                                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                                    }
                                    break;
                                default:
                                    DiversityWorkbench.Forms.FormGetString ftreatment = new DiversityWorkbench.Forms.FormGetString(treatment.ToString(), "Please enter the type of the " + treatment, "", Specimen.Image(treatment.ToString()));
                                    ftreatment.ShowDialog();
                                    if (ftreatment.DialogResult == System.Windows.Forms.DialogResult.OK && ftreatment.String.Length > 0)
                                    {
                                        SQL = "INSERT INTO TaskResult (TaskID, Result) " +
                                            "VALUES (" + TaskID.ToString() + ",'" + ftreatment.String + "')";
                                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            return TaskID;
        }

        /****** Skript für SelectTopNRows-Befehl aus SSMS 
        SELECT TOP(1000) [TaskID]
        ,[TaskParentID]
        ,[DisplayText]
        ,[Type]
        ,[ModuleTitle]
        ,[ModuleType]
        ,[ResultType]
        ,[DateType]
        ,[DateBeginType]
        ,[NumberType]
        ,[NotesType]
        ,[ResponsibleType]
        ,[MetricUnit]
        FROM[DiversityCollection_IPM].[dbo].[Task] t where t.TaskParentID = 17
        ******/
        /*
        TaskID	TaskParentID	DisplayText	Type	ModuleTitle	ModuleType	ResultType	DateType	DateBeginType	NumberType	NotesType	ResponsibleType	MetricUnit
        18	17	Lagerpirat	Beneficial organism	Taxon	DiversityTaxonNames	Anbieter	Date	Einsatz	Röhrchen	NULL	NULL	Röhrchen
        19	17	Reinigung	Cleaning	NULL	NULL	Art der Reinigung	Date	Datum	NULL	Anmerkung	Verantwortlich / Firma	NULL
        20	17	Kleidermottenschlupfwespe	Beneficial organism	Taxon	DiversityTaxonNames	Anbieter	Date	Einsatz	Röhrchen	NULL	NULL	Röhrchen



        SELECT TOP (1000) [TaskID]
        ,[Result]
        ,[URI]
        ,[Description]
        FROM [DiversityCollection_IPM].[dbo].[TaskResult]

        TaskID	Result	URI	Description
        18	Schädlingsbekämpfungshop.de	https://www.schädlingsbekämpfungshop.de/de/Lagerpirat--Speckkaefer--Reismehlkaefer.html	30 Tiere pro Röhrchen
        19	Grundreinigung	NULL	NULL
        */

        #endregion

        #region Image

        private System.Drawing.Bitmap _NoImage;
        public System.Drawing.Bitmap NoImage()
        {
            if (this._NoImage == null)
            {
                string Message = "";
                string pictureDefault = global::DiversityCollection.Properties.Settings.Default.SNSBPictureServer + "IPM/NULL.png";
                _NoImage = DiversityWorkbench.Forms.FormFunctions.BitmapFromWeb(pictureDefault, ref Message);
            }
            return _NoImage;
        }

        #endregion

        #region Dates

        private static DateTime? _Start = null;
        private static DateTime? _End = null;
        private static DateTime? _AdditionalDate = null;

        public static DateTime? Start { get => _Start; set => _Start = value; }
        public static DateTime? End { get => _End; set => _End = value; }
        public static DateTime? AdditionalDate { get => _AdditionalDate;
            set
            {
                // AdditionalDate must be between start and end
                if (value != null && ((_Start != null && (DateTime)value < (DateTime)_Start) || (_End != null && (DateTime)value > (DateTime)_End)))
                {
                    _AdditionalDate = null;
                    DateTime dateTime = (DateTime)value;
                    System.Windows.Forms.MessageBox.Show("The date " + dateTime.ToString("yyyy-MM-dd") + " is outside the current range");
                }
                else
                    _AdditionalDate = value;
            }
        }

        /// <summary>
        /// Getting all dates from table CollectionTask that are within the current list of collections and the optional date restriction
        /// This includes the traps, the collections and the specimen as all entries in table CollectionTask provide the CollectionID
        /// </summary>
        /// <returns>List of CollectionID</returns>
        public static System.Collections.Generic.List<string> Dates()
        {
            System.Collections.Generic.List<string> dates = new List<string>();
            string SQL = "SELECT DISTINCT convert(varchar(10), [TaskStart], 120) FROM [dbo].[CollectionTask] T INNER JOIN " +
                "Collection C ON C.CollectionID IN (" + CollectionIDList() + ") AND T.CollectionID = C.CollectionID AND NOT T.[TaskStart] IS NULL ";
            if (_Start != null)
            {
                DateTime start = (DateTime)_Start;
                SQL += "AND (T.[TaskStart] >= CONVERT(DATETIME, '" + start.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.[TaskStart] IS NULL) AND (T.TaskEnd >= CONVERT(DATETIME, '" + start.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.TaskEnd IS NULL) ";
            }
            if (_End != null)
            {
                DateTime end = (DateTime)_End;
                SQL += "AND (T.[TaskStart] <= CONVERT(DATETIME, '" + end.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.[TaskStart] IS NULL) AND (T.TaskEnd <= CONVERT(DATETIME, '" + end.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.TaskEnd IS NULL) ";
            }
            SQL += " UNION SELECT convert(varchar(10), getdate(), 120) ";
            if (_AdditionalDate != null)
            {
                DateTime date = (DateTime)_AdditionalDate;
                SQL += " UNION SELECT '" + date.ToString("yyyy-MM-dd") + "' ";
            }
            SQL += " UNION SELECT convert(varchar(10), getdate(), 120) ";
            SQL += "ORDER BY convert(varchar(10), [TaskStart], 120)";
            System.Data.DataTable dt = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            if (dt.Rows.Count > 0)
            {
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    dates.Add(R[0].ToString());
                }
            }
            return dates;
        }

        #endregion

        #region Chart

        public static void ChartInit(int ID, bool forCollection = true)
        {
            _ID = ID;
            _ForCollection = forCollection;
        }

        private static int _ID = -1;
        private static bool _ForCollection = true;

        public static System.Collections.Generic.Dictionary<Metric, bool> MetricsSelected()
        {
            System.Collections.Generic.Dictionary<Metric, bool> Metrics = new Dictionary<Metric, bool>();
            string Restriction = "";
            if (_ForCollection)
                Restriction = "T.CollectionID IN (" + CollectionIDList(_ID) + ") ";
            else
                Restriction = "T.CollectionTaskID = " + _ID.ToString();
            if (_Start != null)
            {
                DateTime _start = (DateTime)_Start;
                Restriction += "AND (T.[TaskStart] >= CONVERT(DATETIME, '" + _start.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.[TaskStart] IS NULL) AND (T.TaskEnd >= CONVERT(DATETIME, '" + _start.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.TaskEnd IS NULL) ";
            }
            if (_End != null)
            {
                DateTime _end = (DateTime)_End;
                Restriction += "AND (T.[TaskStart] <= CONVERT(DATETIME, '" + _end.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.[TaskStart] IS NULL) AND (T.TaskEnd <= CONVERT(DATETIME, '" + _end.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.TaskEnd IS NULL) ";
            }
            //string SQL = "SELECT DISTINCT T.CollectionTaskID, T.MetricDescription, T.MetricUnit " +
            //    " FROM CollectionTask AS T INNER JOIN CollectionTaskMetric M ON T.CollectionTaskID = M.CollectionTaskID " +
            //    " WHERE " + Restriction + 
            //    " /*AND T.MetricDescription <> ''*/ AND (T.DisplayOrder > 0) " +
            //    " ORDER BY T.MetricDescription";
            string SQL = "SELECT DISTINCT T.CollectionTaskID, CASE WHEN T.MetricDescription <> '' THEN T.MetricDescription ELSE A.DisplayText END AS Description, T.MetricUnit " +
                "FROM CollectionTask AS T INNER JOIN " +
                "CollectionTaskMetric AS M ON T.CollectionTaskID = M.CollectionTaskID INNER JOIN " +
                "Task AS A ON T.TaskID = A.TaskID " +
                "WHERE(T.DisplayOrder > 0) " +
                "AND " + Restriction +
                " ORDER BY Description";
            System.Data.DataTable dataTableMetrics = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTableMetrics);
            int ii = 0;
            for (int i = 0; i < dataTableMetrics.Rows.Count; i++)
            {
                Metric M = new Metric();
                M.CollectionTaskID = int.Parse(dataTableMetrics.Rows[i][0].ToString());
                M.Description = dataTableMetrics.Rows[i][1].ToString();
                M.Unit = dataTableMetrics.Rows[i][2].ToString();
                if (!Metrics.ContainsKey(M))
                    Metrics.Add(M, true);
                ii++;
            }
            return Metrics;
        }

        public static System.Collections.Generic.Dictionary<string, bool> PestsSelected()
        {
            System.Collections.Generic.Dictionary<string, bool> Pests = new Dictionary<string, bool>();
            string Restriction = "";
            if (_ForCollection)
                Restriction = "T.CollectionID IN (" + CollectionIDList(_ID) + ") ";
            else
                Restriction = "T.CollectionTaskID = " + _ID.ToString();
            if (_Start != null)
            {
                DateTime _start = (DateTime)_Start;
                Restriction += "AND (T.[TaskStart] >= CONVERT(DATETIME, '" + _start.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.[TaskStart] IS NULL) AND (T.TaskEnd >= CONVERT(DATETIME, '" + _start.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.TaskEnd IS NULL) ";
            }
            if (_End != null)
            {
                DateTime _end = (DateTime)_End;
                Restriction += "AND (T.[TaskStart] <= CONVERT(DATETIME, '" + _end.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.[TaskStart] IS NULL) AND (T.TaskEnd <= CONVERT(DATETIME, '" + _end.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.TaskEnd IS NULL) ";
            }
            string SQL = "SELECT DISTINCT T.DisplayText, T.ModuleUri " +
                " FROM CollectionTask AS T INNER JOIN Task Y ON T.TaskID = Y.TaskID  WHERE Y.Type = 'pest' AND " + Restriction +
                " AND T.DisplayText <> '' AND (T.DisplayOrder > 0) " +
                " ORDER BY T.DisplayText, T.ModuleUri";
            System.Data.DataTable dataTablePests = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTablePests);
            int ii = 0;
            if (_Pests == null) _Pests = new Dictionary<string, Taxon>();
            if (dataTablePests != null && dataTablePests.Rows.Count > 0)
            {
                for (int i = 0; i < dataTablePests.Rows.Count; i++)
                {
                    if (_Pests.ContainsKey(dataTablePests.Rows[i][1].ToString()))
                    {
                        //Pests.Add(dataTablePests.Rows[i][0].ToString(), true);
                        Pests.Add(_Pests[dataTablePests.Rows[i][1].ToString()].VernacularName, true);
                    }
                    ii++;
                }
            }
            return Pests;
        }


        public static void ChartGetData(
            ref System.Data.DataTable dataTable,
            ref System.Collections.Generic.Dictionary<string, string> Spalten,
            ref System.Collections.Generic.List<string> Metrics,
            ref System.Collections.Generic.List<string> Cleaning,
            ref System.Collections.Generic.List<string> Beneficials,
            System.Windows.Forms.DataVisualization.Charting.SeriesChartType PestChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble,
            bool IncludeCleaning = false,
            //System.DateTime? start = null, System.DateTime? end = null,
            System.Collections.Generic.List<string> TaxonUris = null, System.Collections.Generic.List<int> SensorIDs = null)
        {
            try
            {
                Spalten.Clear();

                string Restriction = "";
                if (!_ForCollection)
                    Restriction = " T.CollectionTaskID = " + _ID.ToString() + "  ";
                else
                    Restriction = " T.CollectionID IN (" + CollectionIDList(_ID) + ") ";
                if (_Start != null)
                {
                    DateTime _start = (DateTime)_Start;
                    Restriction += "AND (T.[TaskStart] >= CONVERT(DATETIME, '" + _start.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.[TaskStart] IS NULL) AND (T.TaskEnd >= CONVERT(DATETIME, '" + _start.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.TaskEnd IS NULL) ";
                }
                if (_End != null)
                {
                    DateTime _end = (DateTime)_End;
                    Restriction += "AND (T.[TaskStart] <= CONVERT(DATETIME, '" + _end.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.[TaskStart] IS NULL) AND (T.TaskEnd <= CONVERT(DATETIME, '" + _end.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.TaskEnd IS NULL) ";
                }

                string SQL = "SELECT T.MetricDescription " +
                    "FROM CollectionTask AS T INNER JOIN CollectionTaskMetric M ON T.CollectionTaskID = M.CollectionTaskID AND " + Restriction +
                    " AND T.MetricDescription <> '' AND (T.DisplayOrder > 0) " +
                    "GROUP BY T.MetricDescription, T.DisplayOrder ORDER BY T.MetricDescription";

                SQL = "SELECT S.CollectionName + ': ' + CASE WHEN T.MetricDescription <> '' THEN T.MetricDescription ELSE A.MetricType END + CASE  WHEN T.MetricUnit <> ''THEN ' ' + T.MetricUnit ELSE CASE WHEN A.MetricUnit <> '' THEN ' ' + A.MetricUnit END END, T.CollectionTaskID " +
                    "FROM Task A " +
                    "INNER JOIN CollectionTask AS T ON T.TaskID = A.TaskID " +
                    "INNER JOIN CollectionTaskMetric M ON T.CollectionTaskID = M.CollectionTaskID AND (T.DisplayOrder > 0) AND " + Restriction +
                    "INNER JOIN Collection S ON S.CollectionID = T.CollectionID " +
                    "GROUP BY T.MetricDescription, T.DisplayOrder, A.MetricType, T.CollectionTaskID, T.MetricUnit, A.MetricUnit, S.CollectionName " +
                    "ORDER BY S.CollectionName";
                System.Collections.Generic.Dictionary<string, int> MetricIDs = new Dictionary<string, int>();
                System.Data.DataTable dataTableMetrics = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTableMetrics);
                int ii = 0;
                for (int i = 0; i < dataTableMetrics.Rows.Count; i++)
                {
                    string series = dataTableMetrics.Rows[i][0].ToString();
                    if (series.Length > 0)
                    {
                        if (Spalten.ContainsKey(series))
                            series += " [" + dataTableMetrics.Rows[i][1].ToString() + "]";
                        Spalten.Add(series, "Wert_" + (i + 1).ToString());
                        Metrics.Add(series);
                        int ID;
                        if (int.TryParse(dataTableMetrics.Rows[i][1].ToString(), out ID))
                        {
                            MetricIDs.Add(series, ID);
                        }
                        ii++;
                    }
                }

                // getting the cleaning
                SQL = "SELECT T.Result " +
                    "FROM CollectionTask AS T INNER JOIN Task A ON T.TaskID = A.TaskID AND A.Type = 'Cleaning' AND " + Restriction +
                    " AND T.Result <> '' AND (T.DisplayOrder > 0) " +
                    "GROUP BY T.Result ORDER BY T.Result";
                System.Data.DataTable dataTableCleaning = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTableCleaning);
                for (int i = 0; i < dataTableCleaning.Rows.Count; i++)
                {
                    string clean = dataTableCleaning.Rows[i][0].ToString();
                    if (clean.Length > 0)
                    {
                        Spalten.Add(clean, "Wert_" + (ii + 1).ToString());
                        Cleaning.Add(clean);
                        ii++;
                    }
                }

                // getting the beneficials
                SQL = "SELECT A.DisplayText " +
                    "FROM CollectionTask AS T INNER JOIN Task A ON T.TaskID = A.TaskID AND A.Type = 'Beneficial organism' AND " + Restriction +
                    " AND A.DisplayText <> '' AND (T.DisplayOrder > 0) AND T.NumberValue > 0 " +
                    "GROUP BY A.DisplayText ORDER BY A.DisplayText";
                System.Data.DataTable dataTableBeneficials = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTableBeneficials);
                for (int i = 0; i < dataTableBeneficials.Rows.Count; i++)
                {
                    string beneficial = dataTableBeneficials.Rows[i][0].ToString();
                    if (beneficial.Length > 0)
                    {
                        Spalten.Add(beneficial, "Wert_" + (ii + 1).ToString());
                        Beneficials.Add(beneficial);
                        ii++;
                    }
                }


                // getting the series
                if (!_ForCollection)
                    Restriction = " (T.CollectionTaskID = " + _ID.ToString() + " OR T.CollectionTaskParentID = " + _ID.ToString() + ") ";
                else
                    Restriction = " T.CollectionID IN (" + CollectionIDList(_ID) + ") ";
                if (_Start != null)
                {
                    DateTime _start = (DateTime)_Start;
                    Restriction += "AND (T.[TaskStart] >= CONVERT(DATETIME, '" + _start.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.[TaskStart] IS NULL) AND (T.TaskEnd >= CONVERT(DATETIME, '" + _start.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.TaskEnd IS NULL) ";
                }
                if (_End != null)
                {
                    DateTime _end = (DateTime)_End;
                    Restriction += "AND (T.[TaskStart] <= CONVERT(DATETIME, '" + _end.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.[TaskStart] IS NULL) AND (T.TaskEnd <= CONVERT(DATETIME, '" + _end.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.TaskEnd IS NULL) ";
                }

                SQL = "SELECT T.Description " +
                    "FROM CollectionTask AS T INNER JOIN Task A ON T.TaskID = A.TaskID AND A.Type = 'Pest' AND " + Restriction +
                    " AND T.Description <> '' AND (T.DisplayOrder > 0) AND (T.NumberValue > 0)" +
                    " GROUP BY T.Description, T.DisplayOrder ORDER BY T.Description";

                System.Data.DataTable dataTableSeries = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTableSeries);
                for (int i = 0; i < dataTableSeries.Rows.Count; i++)
                {
                    string series = dataTableSeries.Rows[i][0].ToString();
                    if (series.Length > 0)
                    {
                        Spalten.Add(series, "Wert_" + (ii + 1).ToString());
                        ii++;
                    }
                }

                // Setting the restriction
                if (!_ForCollection)
                    Restriction = " (T.CollectionTaskID = " + _ID.ToString() + " OR T.CollectionTaskParentID = " + _ID.ToString() + ") ";
                else
                    Restriction = " C.CollectionID IN (" + CollectionIDList(_ID) + ") ";
                if (_Start != null)
                {
                    DateTime _start = (DateTime)_Start;
                    Restriction += "AND (T.[TaskStart] >= CONVERT(DATETIME, '" + _start.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.[TaskStart] IS NULL) AND (T.TaskEnd >= CONVERT(DATETIME, '" + _start.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.TaskEnd IS NULL) ";
                }
                if (_End != null)
                {
                    DateTime _end = (DateTime)_End;
                    Restriction += "AND (T.[TaskStart] <= CONVERT(DATETIME, '" + _end.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.[TaskStart] IS NULL) AND (T.TaskEnd <= CONVERT(DATETIME, '" + _end.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.TaskEnd IS NULL) ";
                }

                //// getting start and end
                string RestrictionMetricDate = "";
                if (_Start != null)
                {
                    DateTime _start = (DateTime)_Start;
                    RestrictionMetricDate += " AND (M.MetricDate >= CONVERT(DATETIME, '" + _start.ToString("yyyy-MM-dd") + " 00:00:00', 102)) ";
                }
                if (_End != null)
                {
                    DateTime _end = (DateTime)_End;
                    RestrictionMetricDate += " AND (M.MetricDate <= CONVERT(DATETIME, '" + _end.ToString("yyyy-MM-dd") + " 00:00:00', 102)) ";
                }

                SQL = "/*no value*/ declare @NoValue int; Set @NoValue = NULL; " +
                    //"/*bubble*/ declare @F int; Set @F = " + this.numericUpDownBubble.Value.ToString() + "; " +
                    "/*getting start and end*/ " +
                    "declare @start date; " +
                    "declare @end date; " +
                    "set @start = (SELECT cast(min(CONVERT(varchar(10), T.TaskStart, 120)) as date) AS TaskStart " +
                    "FROM [dbo].[Collection] C INNER JOIN dbo.CollectionTask T ON " + Restriction + " AND C.CollectionID = T.CollectionID AND C.Type = 'Trap' AND T.DisplayText <> '') ; " +
                    "set @end = (SELECT cast(max(CONVERT(varchar(10), T.TaskStart, 120)) as date) AS TaskStart " +
                    "FROM [dbo].[Collection] C INNER JOIN dbo.CollectionTask T ON " + Restriction + " AND C.CollectionID = T.CollectionID AND C.Type = 'Trap' AND T.DisplayText <> ''); ";
                // metric start and end
                SQL += "/*metric start and end*/ " +
                    "declare @MetricStart date; " +
                    "declare @MetricEnd date; " +
                    "set @MetricStart = (SELECT cast(min(CONVERT(varchar(10), M.MetricDate, 120))  as date) AS TaskStart " +
                    "FROM [dbo].[Collection] C INNER JOIN dbo.CollectionTask T ON " + Restriction + " AND C.CollectionID = T.CollectionID AND C.Type = 'Sensor' INNER JOIN CollectionTaskMetric AS M ON M.CollectionTaskID = T.CollectionTaskID " + RestrictionMetricDate + ") ; " +
                    "set @MetricEnd = (SELECT cast(max(CONVERT(varchar(10), M.MetricDate, 120)) as date) AS TaskStart " +
                    "FROM [dbo].[Collection] C INNER JOIN dbo.CollectionTask T ON " + Restriction + " AND C.CollectionID = T.CollectionID AND C.Type = 'Sensor' INNER JOIN CollectionTaskMetric AS M ON M.CollectionTaskID = T.CollectionTaskID" + RestrictionMetricDate + "); ";
                // setting the wider range
                SQL += "/*setting the wider range*/ " +
                    "if (@start IS NULL OR (NOT @MetricStart IS NULL AND @start > @MetricStart)) " +
                    "begin set @start = @MetricStart end; " +
                    "if (@end IS NULL OR (NOT @MetricEnd IS NULL AND @end < @MetricEnd)) " +
                    "begin set @end = @MetricEnd end; ";
                // inserting start
                SQL += "/*declare table containing data*/ " +
                    "declare @Werte table(Zeitpunkt date ";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
                    SQL += ", " + KV.Value + " float";
                SQL += "); /*inserting start*/ insert into @Werte(Zeitpunkt";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
                    SQL += ", " + KV.Value;
                SQL += ") Values(@start";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
                    SQL += ", @NoValue";
                SQL += "); ";

                // filling the table with months
                SQL += "while (@end > (select max(Zeitpunkt) from @Werte)) " +
                    "begin " +
                    "insert into @Werte(Zeitpunkt";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
                    SQL += ", " + KV.Value;
                SQL += ") Values((select DateAdd(";
                //if (dataTableMetrics.Rows.Count == 0)
                //    SQL += "month";
                //else
                //    SQL += "day";
                SQL += "day";
                SQL += ", 1, max(Zeitpunkt)) from @Werte) ";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
                    SQL += ", @NoValue";
                SQL += ") " +
                    "end; ";

                // getting the maximum of all values
                SQL += "/*maximal value of all entries*/ declare @Max float; set @Max = 1; ";

                SQL += "/*getting number values*/";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
                {
                    if (Metrics.Contains(KV.Key))
                        continue;
                    if (Cleaning.Contains(KV.Key))
                        continue;
                    if (Beneficials.Contains(KV.Key))
                        continue;
                    // getting the values
                    string DisplayText = KV.Key;
                    string UpDown = "";
                    if (KV.Key.IndexOf("|") > 0)
                    {
                        DisplayText = KV.Key.Substring(0, KV.Key.IndexOf("|"));
                        UpDown = KV.Key.Substring(KV.Key.IndexOf("|") + 1);
                    }
                    SQL += "/* getting " + KV.Value + " */  declare @" + KV.Value + " table(Tag date, Wert float); " +
                        "insert into @" + KV.Value + " (Tag, Wert) " +
                        "SELECT cast(min(CONVERT(varchar(10), C.TaskStart, 120)) as date), ";
                    SQL += " sum(C.NumberValue) ";
                    SQL += "FROM CollectionTask AS C " +
                        "WHERE(C.Description = N'" + DisplayText + "') AND (NOT(C.TaskStart IS NULL)) AND (NOT(C.NumberValue IS NULL)) AND (C.NumberValue > 0) AND (C.DisplayOrder > 0) " +
                        "GROUP BY C.Description, CONVERT(varchar(10), C.TaskStart, 120); ";
                    // transfer values in main table
                    SQL += "update W set " + KV.Value + " = W1.Wert " +
                        "from @Werte W inner " +
                        "join @" + KV.Value + " W1 on cast(CONVERT(varchar(10), W.Zeitpunkt, 120) as date) = W1.Tag; ";
                    // getting max
                    SQL += "SET @Max = (SELECT CASE WHEN MAX(W.Wert) > @Max THEN MAX(W.Wert) ELSE @Max END from @" + KV.Value + " W); ";
                }
                SQL += "/*getting metrics*/ ";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
                {
                    if (!MetricIDs.ContainsKey(KV.Key))
                        continue;
                    if (!Metrics.Contains(KV.Key))
                        continue;
                    // getting the values
                    SQL += "/* getting " + KV.Value + " */  declare @" + KV.Value + " table(Tag date, Wert float); " +
                        "insert into @" + KV.Value + " (Tag, Wert) " +
                        "SELECT cast(min(CONVERT(varchar(10), M.MetricDate, 120)) as date), avg(M.MetricValue) " +
                        "FROM CollectionTask C INNER JOIN CollectionTaskMetric AS M ON C.CollectionTaskID = M.CollectionTaskID AND (C.CollectionTaskID = " + MetricIDs[KV.Key] + ") AND (NOT(M.MetricDate IS NULL)) AND (NOT(M.MetricValue IS NULL)) AND (C.DisplayOrder > 0) " +
                        "GROUP BY C.MetricDescription, CONVERT(varchar(10), M.MetricDate, 120); ";
                    // transfer values in main table
                    SQL += "update W set " + KV.Value + " = W1.Wert " +
                        "from @Werte W inner " +
                        "join @" + KV.Value + " W1 on W.Zeitpunkt = W1.Tag; ";
                    // getting max
                    SQL += "SET @Max = (SELECT CASE WHEN MAX(W.Wert) > @Max THEN MAX(W.Wert) ELSE @Max END from @" + KV.Value + " W); ";
                }
                SQL += "; /*getting cleaning*/ ";
                SQL += " SET @Max = (SELECT (CAST(@Max / 10 AS int) + 1) * 10) ; ";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
                {
                    if (!Cleaning.Contains(KV.Key))
                        continue;
                    // getting the values
                    SQL += "/* getting " + KV.Value + " */  declare @" + KV.Value + " table(Tag date, Wert float); " +
                        "insert into @" + KV.Value + " (Tag, Wert) " +
                        "SELECT cast(min(CONVERT(varchar(10), C.TaskStart, 120)) as date), @Max + 5 " +
                        "FROM CollectionTask C INNER JOIN Task T ON C.TaskID = T.TaskID AND T.Type = 'Cleaning' AND (C.DisplayOrder > 0) AND C.Result = '" + KV.Key + "' " +
                        "GROUP BY C.Result, CONVERT(varchar(10), C.TaskStart, 120); ";
                    // transfer values in main table
                    SQL += "update W set " + KV.Value + " = W1.Wert " +
                        "from @Werte W inner " +
                        "join @" + KV.Value + " W1 on W.Zeitpunkt = W1.Tag; ";
                }

                SQL += "; /*getting beneficials*/ ";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
                {
                    if (!Beneficials.Contains(KV.Key))
                        continue;
                    // getting the values
                    SQL += "/* getting " + KV.Value + " */  declare @" + KV.Value + " table(Tag date, Wert float); " +
                        "insert into @" + KV.Value + " (Tag, Wert) " +
                        "SELECT cast(min(CONVERT(varchar(10), C.TaskStart, 120)) as date), @Max + 10 " +
                        "FROM CollectionTask C INNER JOIN Task T ON C.TaskID = T.TaskID AND T.Type = 'Beneficial organism' AND (C.DisplayOrder > 0) AND T.DisplayText = '" + KV.Key + "' " +
                        "GROUP BY T.DisplayText, CONVERT(varchar(10), C.TaskStart, 120); ";
                    // transfer values in main table
                    SQL += "update W set " + KV.Value + " = W1.Wert " +
                        "from @Werte W inner " +
                        "join @" + KV.Value + " W1 on W.Zeitpunkt = W1.Tag; ";
                }

                SQL += "; select CONVERT(varchar(10), Zeitpunkt, 120) AS Zeitpunkt ";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Spalten)
                    SQL += ", " + KV.Value;
                SQL += " from @Werte";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTable);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private enum RestrictionType { Metric, Pest, MetricDate }//, Data }
        private static string ChartDataRestriction(RestrictionType Type)
        {
            string Restriction = "";
            DateTime _start = System.DateTime.Now;
            DateTime _end = System.DateTime.Now;
            if (_Start != null)
            {
                _start = (DateTime)_Start;
            }
            if (_End != null)
            {
                _end = (DateTime)_End;
            }


            switch (Type)
            {
                case RestrictionType.Metric:
                    if (!_ForCollection)
                        Restriction = " T.CollectionTaskID = " + _ID.ToString() + "  ";
                    else
                        Restriction = " T.CollectionID IN (" + CollectionIDList(_ID) + ") ";
                    if (_Start != null)
                    {
                        Restriction += "AND (T.[TaskStart] >= CONVERT(DATETIME, '" + _start.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.[TaskStart] IS NULL) AND (T.TaskEnd >= CONVERT(DATETIME, '" + _start.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.TaskEnd IS NULL) ";
                    }
                    if (_End != null)
                    {
                        Restriction += "AND (T.[TaskStart] <= CONVERT(DATETIME, '" + _end.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.[TaskStart] IS NULL) AND (T.TaskEnd <= CONVERT(DATETIME, '" + _end.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.TaskEnd IS NULL) ";
                    }
                    break;
                case RestrictionType.Pest:
                    if (!_ForCollection)
                        Restriction = " (T.CollectionTaskID = " + _ID.ToString() + " OR T.CollectionTaskParentID = " + _ID.ToString() + ") ";
                    else
                        Restriction = " T.CollectionID IN (" + CollectionIDList(_ID) + ") ";
                    if (_Start != null)
                    {
                        Restriction += "AND (T.[TaskStart] >= CONVERT(DATETIME, '" + _start.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.[TaskStart] IS NULL) AND (T.TaskEnd >= CONVERT(DATETIME, '" + _start.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.TaskEnd IS NULL) ";
                    }
                    if (_End != null)
                    {
                        Restriction += "AND (T.[TaskStart] <= CONVERT(DATETIME, '" + _end.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.[TaskStart] IS NULL) AND (T.TaskEnd <= CONVERT(DATETIME, '" + _end.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.TaskEnd IS NULL) ";
                    }

                    break;
                //case RestrictionType.Data:
                //    if (!_ForCollection)
                //        Restriction = " (T.CollectionTaskID = " + _ID.ToString() + " OR T.CollectionTaskParentID = " + _ID.ToString() + ") ";
                //    else
                //        Restriction = " C.CollectionID IN (" + CollectionIDList(_ID) + ") ";
                //    if (_Start != null)
                //    {
                //        Restriction += "AND (T.[TaskStart] >= CONVERT(DATETIME, '" + _start.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.[TaskStart] IS NULL) AND (T.TaskEnd >= CONVERT(DATETIME, '" + _start.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.TaskEnd IS NULL) ";
                //    }
                //    if (_End != null)
                //    {
                //        Restriction += "AND (T.[TaskStart] <= CONVERT(DATETIME, '" + _end.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.[TaskStart] IS NULL) AND (T.TaskEnd <= CONVERT(DATETIME, '" + _end.ToString("yyyy-MM-dd") + " 00:00:00', 102) OR T.TaskEnd IS NULL) ";
                //    }
                //    break;
                case RestrictionType.MetricDate:
                    if (_Start != null)
                    {
                        Restriction = " AND (M.MetricDate >= CONVERT(DATETIME, '" + _start.ToString("yyyy-MM-dd") + " 00:00:00', 102)) ";
                    }
                    if (_End != null)
                    {
                        Restriction = " AND (M.MetricDate <= CONVERT(DATETIME, '" + _end.ToString("yyyy-MM-dd") + " 00:00:00', 102)) ";
                    }

                    break;
            }
            return Restriction;
        }

        public static System.Collections.Generic.List<string> ChartTitles()
        {
            System.Collections.Generic.List<string> Titles = new List<string>();
            string SQL = "";
            if (_ForCollection)
                SQL = "SELECT C.DisplayText FROM [dbo].[CollectionHierarchyAll]() C WHERE C.CollectionID = " + _ID.ToString();
            else
                SQL = "SELECT C.DisplayText FROM [dbo].[CollectionHierarchyAll]() C INNER JOIN CollectionTask T ON T.CollectionID = C.CollectionID AND T.CollectionTaskID = " + _ID.ToString();
            System.Data.DataTable dt = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            if (!_ForCollection)
            {
                SQL = "SELECT C.TaskDisplayText AS DisplayText FROM [dbo].[CollectionTaskHierarchyAll]() C WHERE C.CollectionTaskID = " + _ID.ToString();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            }
            foreach (System.Data.DataRow R in dt.Rows)
            {
                Titles.Add(R[0].ToString());

            }
            return Titles;
        }


        private static string ChartTitle()
        {
            string Title = "";
            foreach (string T in ChartTitles())
            {
                if (Title.Length > 0)
                    Title += "_";
                Title += T.Replace(" ", "_").Replace("|", "_").Replace(".", "_");
            }
            while (Title.IndexOf("__") > -1)
                Title = Title.Replace("__", "_");
            return Title;
        }

        public bool ChartCreate(int ID, bool ForCollection = true, int Width = 800, int Height = 400)
        {
            bool HasData = false;
            try
            {
                IPM.ChartInit(ID, ForCollection);

                // Adding the titles
                this._Chart.Titles.Clear();
                System.Collections.Generic.List<string> TT = ChartTitles();
                foreach (string T in TT)
                {
                    this._Chart.Titles.Add(T);
                }

                System.Data.DataTable _dataTableChart = new System.Data.DataTable();
                Dictionary<string, string> _Spalten = new Dictionary<string, string>();
                System.Collections.Generic.List<string> Metrics = new List<string>();
                System.Collections.Generic.List<string> Cleaning = new List<string>();
                System.Collections.Generic.List<string> Beneficials = new List<string>();
                IPM.ChartGetData(ref _dataTableChart, ref _Spalten, ref Metrics, ref Cleaning, ref Beneficials);
                this._Chart.Width = Width;
                this._Chart.Height = Height;
                this._Chart.DataSource = _dataTableChart;
                this._Chart.Series.Clear();
                System.Collections.Generic.Dictionary<string, string> Series = new Dictionary<string, string>();
                System.Collections.Generic.Dictionary<string, string> CleaningPoints = new Dictionary<string, string>();
                System.Collections.Generic.Dictionary<string, string> BeneficialPoints = new Dictionary<string, string>();
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in _Spalten)
                {
                    if (Metrics.Contains(KV.Key))
                    {
                        Series.Add(KV.Key, KV.Value);
                    }
                    else if (Cleaning.Contains(KV.Key))
                        CleaningPoints.Add(KV.Key, KV.Value);
                    else if (Beneficials.Contains(KV.Key))
                        BeneficialPoints.Add(KV.Key, KV.Value);
                    else
                    {
                        if (KV.Key.IndexOf("|") == -1)
                        {
                            Series.Add(KV.Key, KV.Value + ", " + KV.Value);
                        }
                    }
                }
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Series)
                {
                    this._Chart.Series.Add(KV.Key);
                    this._Chart.Series[KV.Key].XValueMember = "Zeitpunkt";
                    if (Metrics.Contains(KV.Key))
                    {
                        this._Chart.Series[KV.Key].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                    }
                    else
                    {
                        this._Chart.Series[KV.Key].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
                    }
                    this._Chart.Series[KV.Key].YValueMembers = KV.Value;
                }
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in CleaningPoints)
                {
                    this._Chart.Series.Add(KV.Key);
                    this._Chart.Series[KV.Key].XValueMember = "Zeitpunkt";
                    if (Cleaning.Contains(KV.Key))
                    {
                        this._Chart.Series[KV.Key].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                        string Path = Folder.Report(Folder.ReportFolder.TaskImg) + "Cleaning.ico";
                        System.IO.FileInfo FI = new System.IO.FileInfo(Path);
                        if (FI.Exists)
                            this._Chart.Series[KV.Key].MarkerImage = Path;
                        else { }
                        if (CleaningPoints.Count > 1)
                        {
                            this._Chart.Series[KV.Key].Label = KV.Key.Substring(0, 2) + ".";
                            this._Chart.Series[KV.Key].LabelToolTip = KV.Key;
                        }
                    }
                    this._Chart.Series[KV.Key].YValueMembers = KV.Value;
                }
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in BeneficialPoints)
                {
                    this._Chart.Series.Add(KV.Key);
                    this._Chart.Series[KV.Key].XValueMember = "Zeitpunkt";
                    if (Beneficials.Contains(KV.Key))
                    {
                        this._Chart.Series[KV.Key].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                        string Path = Folder.Report(Folder.ReportFolder.TaskImg) + "Animal.ico";
                        System.IO.FileInfo FI = new System.IO.FileInfo(Path);
                        if (FI.Exists)
                            this._Chart.Series[KV.Key].MarkerImage = Path;
                        else { }
                        if (BeneficialPoints.Count > 1)
                        {
                            this._Chart.Series[KV.Key].Label = KV.Key.Substring(0, 2) + ".";
                            this._Chart.Series[KV.Key].LabelToolTip = KV.Key;
                        }
                    }
                    this._Chart.Series[KV.Key].YValueMembers = KV.Value;
                }
                if (_dataTableChart.Rows.Count > 0 && (_Spalten.Count > 0 || Series.Count > 0))
                    HasData = true;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                HasData = false;
            }
            return HasData;
        }

        public string ChartSaveImage(string Path = "")
        {
            try
            {
                if (Path.Length == 0)
                {
                    System.IO.FileInfo File = new System.IO.FileInfo(DiversityCollection.Folder.Report(Folder.ReportFolder.TaskImg) + ChartTitle() + "_Report_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".png");
                    Path = File.FullName;// DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.FolderType.Export) + ChartTitle() + "_Export_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".png";
                }
                this._Chart.SaveImage(Path, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); ;
            }
            return Path;
        }

        //public static string ChartImageSave(string Path = "")
        //{
        //    try
        //    {
        //        if (Path.Length == 0)
        //            Path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.FolderType.Export) + this.ChartTitle() + "_Export_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".png";
        //        Chart.SaveImage(Path, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); ;
        //    }
        //    return Path;
        //}

        //private static System.Windows.Forms.DataVisualization.Charting.Chart Chart()
        //{
        //    Tasks.IPM iPM = new IPM();
        //    return iPM._Chart;
        //}


        #endregion

        #region Collection ID

        public static System.Collections.Generic.List<int> CollectionIDs(int CollectionID)
        {
            System.Collections.Generic.List<int> IDs = new List<int>();
            try
            {
                string SQL = "SELECT CollectionID FROM [dbo].[CollectionLocationChildNodes](" + CollectionID.ToString() + ")";
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    int ID;
                    if (int.TryParse(R[0].ToString(), out ID))
                        IDs.Add(ID);
                }
                IDs.Add(CollectionID);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return IDs;
        }

        private static string _CollectionIDList;

        public static void CollectionIDListReset()
        {
            _CollectionIDList = null;
        }
        public static string CollectionIDList()
        {
            if (_CollectionIDList == null)
            {
                _CollectionIDList = Tasks.IPM.CollectionIDList(TopCollectionID());
            }
            return _CollectionIDList;
        }

        private static int TopCollectionID()
        {
            if (Settings.Default.TopCollectionID == -1)
            {
                int _TopCollectionID = 0;
                string SQL = "SELECT TOP 1 C.CollectionID FROM [dbo].[UserCollectionList] () C WHERE C.CollectionID > -1 ORDER BY C.LocationParentID";
                if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out _TopCollectionID))
                    Settings.Default.TopCollectionID = _TopCollectionID;
            }
            return Settings.Default.TopCollectionID;
        }

        public static string CollectionIDList(int CollectionID)
        {
            string SQL = "";
            foreach (int ID in CollectionIDs(CollectionID))
            {
                if (SQL.Length > 0)
                    SQL += ", ";
                SQL += ID.ToString();
            }
            return SQL;
        }

        private static System.Data.DataTable _DtCollection;
        public static System.Data.DataTable DtCollection(bool Reset = false)
        {
            if (_DtCollection == null)
                _DtCollection = new System.Data.DataTable("Collection");
            if (Reset)
                _DtCollection.Clear();
            string SQL = "SELECT DisplayText, CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
                "Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, Type, NULL AS LocationParentID " +
                "FROM DBO.CollectionLocationAll() AS C WHERE C.CollectionID = " + Settings.Default.TopCollectionID.ToString() +
                " AND C.CollectionID IN (SELECT CollectionID FROM dbo.ManagerCollectionList()) " +
                " UNION " +
                "SELECT DisplayText, CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
                "Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, Type, LocationParentID " +
                "FROM DBO.CollectionLocationAll() AS C WHERE C.CollectionID IN (" + CollectionIDList() + ") AND  C.CollectionID <> " + Settings.Default.TopCollectionID.ToString() +
                " AND C.CollectionID IN (SELECT CollectionID FROM dbo.ManagerCollectionList()) " +
                " ORDER BY CollectionName";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref _DtCollection);

            return _DtCollection;
        }

        public static System.Data.DataTable DataTableSpecimens(string DateSQL)
        {
            string SQL = "SELECT DISTINCT CASE WHEN P.AccessionNumber <> '' THEN P.AccessionNumber ELSE CASE WHEN S.AccessionNumber <> '' THEN S.AccessionNumber ELSE '[' + CAST( P.CollectionSpecimenID AS VARCHAR) + '-' + CAST( P.SpecimenPartID AS VARCHAR) + ']' END END " +
                " + ' ' + CASE WHEN P.PartSublabel <> '' THEN P.PartSublabel ELSE '' END + ' | ' + P.MaterialCategory " +
                " AS DisplayText, /*T.CollectionTaskID,*/ T.TaskStart, P.CollectionSpecimenID, P.SpecimenPartID, T.CollectionID " +
                " FROM CollectionTask AS T " +
                " INNER JOIN CollectionSpecimen AS S ON T.CollectionSpecimenID = S.CollectionSpecimenID ";
            if (DateSQL.Length > 0) SQL += " AND T.TaskStart = " + DateSQL;
            SQL += " INNER JOIN CollectionSpecimenPart AS P ON T.CollectionID = P.CollectionID AND T.CollectionSpecimenID = P.CollectionSpecimenID AND T.SpecimenPartID = P.SpecimenPartID AND S.CollectionSpecimenID = P.CollectionSpecimenID " +
                " AND P.CollectionID IN (" + CollectionIDList() + ") " +
                " WHERE T.ModuleUri <> '' " +
                " ORDER BY DisplayText";
            System.Data.DataTable dataTable = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTable);
            return dataTable;
        }

        public static System.Data.DataTable DataTableCollections(string DateSQL)
        {
            string SQL = "SELECT DISTINCT " + 
                "A.DisplayText, A.[CollectionID] " +
//                "A.[CollectionID], A.DisplayText " +
                " FROM dbo.CollectionHierarchyAll() A " +
                " INNER JOIN CollectionTask CT ON A.CollectionID = CT.CollectionID ";
            if (DateSQL.Length > 0) SQL += " AND CT.TaskStart = " + DateSQL;
            SQL += " INNER JOIN Task T ON T.TaskID = CT.TaskID AND T.Type = 'Monitoring' " +
                " INNER JOIN Task I ON T.TaskParentID = I.TaskID AND I.Type = 'IPM' " +
                " WHERE A.CollectionID IN (" + Tasks.IPM.CollectionIDList() + ") AND A.Type<> 'Trap'  " +
                " ORDER BY A.DisplayText";
            System.Data.DataTable dataTable = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTable);
            return dataTable;
        }

        public static System.Data.DataTable DataTableTransactions(string DateSQL)
        {
            string SQL = "SELECT DISTINCT I.TransactionTitle + ' | ' + I.TransactionType " +
                " AS DisplayText, /*T.CollectionTaskID,*/ T.TaskStart, I.TransactionID, T.CollectionID " +
                " FROM CollectionTask AS T " +
                " INNER JOIN [Transaction] AS I ON T.TransactionID = I.TransactionID ";
            if (DateSQL.Length > 0) SQL += " AND T.TaskStart = " + DateSQL;
            SQL += " AND I.AdministratingCollectionID IN (" + CollectionIDList() + ") " +
                " WHERE T.ModuleUri <> '' " +
                " ORDER BY DisplayText";
            System.Data.DataTable dataTable = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTable);
            return dataTable;
        }



        #endregion

        #region Cleaning



        #endregion

        #region Sensor

        public static bool AddSensor(int CollectionID, ref int CollectionTaskID, ref string Error)
        {
            bool OK = true;
            int ID = CreateCollectionTaskWithinParent("Sensor", "Monitoring", CollectionID, ref Error); // getSensorID(CollectionID, ref Error);
            if (ID < 0 || Error.Length > 0) OK = false;
            else CollectionTaskID = ID;
            return OK;
        }

        //private static int getSensorID(int CollectionID, ref string Error)
        //{
        //    return getCollectionTaskIDWithinParent("Sensor", "Monitoring", CollectionID, ref Error);

        //    int SensorCollectionTaskID = -1;
        //    try
        //    {
        //        string SQL = "SELECT DISTINCT T.TaskID, T.DisplayText + CASE WHEN T .Description <> '' THEN ' [' + T .Description + ']' ELSE '' END AS DisplayText " +
        //            "FROM Task AS T INNER JOIN " +
        //            "Task AS M ON T.TaskParentID = M.TaskID INNER JOIN " +
        //            "CollectionTask AS C ON M.TaskID = C.TaskID " +
        //            "WHERE (T.Type = N'Sensor') AND (M.Type = N'Monitoring') ";
        //        System.Data.DataTable dtSensors = new System.Data.DataTable();
        //        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtSensors, ref Error);
        //        if (dtSensors.Rows.Count == 1)
        //        {
        //            if (!int.TryParse(dtSensors.Rows[0][0].ToString(), out SensorCollectionTaskID))
        //            {
        //                SensorCollectionTaskID = -1;
        //            }
        //        }
        //        else if (dtSensors.Rows.Count > 1) // more then 1 sensor
        //        {
        //            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtSensors, "DisplayText", "CollectionTaskID", "Please select a sensor from the list", "Sensor", "", false, true, true, DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Sensor));
        //            f.ShowDialog();
        //            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
        //            {
        //                if (!int.TryParse(f.SelectedValue, out SensorCollectionTaskID))
        //                    SensorCollectionTaskID = -1;
        //            }
        //        }
        //        else // no sensor
        //        {
        //            if (System.Windows.Forms.MessageBox.Show("There is no sensor defined so far. Do you want to define a new sensor?", "No sensor", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
        //            {
        //                // find monitoring for Collection
        //                int MonitoringID = GetMonitoringID(CollectionID, "", ref Error);
        //                if (Error.Length > 0 || MonitoringID == -1)
        //                    SensorCollectionTaskID = -1;
        //                else
        //                {
        //                    // find task for sensor
        //                    int SensorTaskID = GetSensorTaskID(MonitoringID, ref Error);
        //                    if (Error.Length > 0 || SensorTaskID == -1)
        //                        SensorCollectionTaskID = -1;
        //                    else
        //                    {
        //                        DiversityWorkbench.Forms.FormGetString fSensor = new DiversityWorkbench.Forms.FormGetString("New sensor", "Please enter the name for the new sensor", "", DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Sensor));
        //                        fSensor.ShowDialog();
        //                        if (fSensor.DialogResult == System.Windows.Forms.DialogResult.OK && fSensor.String.Length > 0)
        //                        {
        //                            string SensorTitle = fSensor.String;
        //                            SQL = "INSERT INTO CollectionTask (CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, DisplayText) " +
        //                                "VALUES(" + MonitoringID.ToString() + ", " + CollectionID.ToString() + ", " + SensorTaskID.ToString() + ", 1, '" + SensorTitle + "') " +
        //                                "; SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];";
        //                            int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Error), out SensorCollectionTaskID);
        //                            if (SensorCollectionTaskID == 0)
        //                            {
        //                                SQL = "SELECT MAX(CollectionTaskID) FROM CollectionTask WHERE CollectionTaskParentID = " + MonitoringID.ToString() + " AND TaskID = " + SensorTaskID.ToString() + " AND DisplayOrder = 1";
        //                                int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out SensorCollectionTaskID);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            Error = "No sensor available";
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                SensorCollectionTaskID = -1;
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return SensorCollectionTaskID;
        //}

        //private static int GetSensorTaskID(int MonitoringID, ref string Error)
        //{
        //    return GetTaskIdWithinMonitoring("Sensor", MonitoringID, ref Error);

        //    //int SensorTaskID = -1;
        //    //// try to find the sensor task
        //    //string SQL = "SELECT T.DisplayText, T.TaskID " +
        //    //    "FROM[dbo].[CollectionTask] C " +
        //    //    "INNER JOIN Task M ON  C.TaskID = M.TaskID AND M.Type = 'Monitoring' AND C.CollectionTaskID = " + MonitoringID.ToString() + " " +
        //    //    "INNER JOIN Task T ON T.TaskParentID = M.TaskID AND T.Type = 'Sensor'";
        //    //System.Data.DataTable dtSensorTask = new System.Data.DataTable();
        //    //string ErrorLocal = "";
        //    //DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtSensorTask, ref ErrorLocal);
        //    //if (ErrorLocal.Length > 0)
        //    //    return -1;
        //    //if (dtSensorTask.Rows.Count == 1)
        //    //{
        //    //    int.TryParse(dtSensorTask.Rows[0][1].ToString(), out SensorTaskID);
        //    //}
        //    //else if (dtSensorTask.Rows.Count > 1)
        //    //{
        //    //    DiversityWorkbench.Forms.FormGetStringFromList fTT = new DiversityWorkbench.Forms.FormGetStringFromList(dtSensorTask, "DisplayText", "TaskID", "Please select a sensor from the list", "Sensor", "", false, true, true, DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Sensor));
        //    //    fTT.ShowDialog();
        //    //    if (fTT.DialogResult == System.Windows.Forms.DialogResult.OK)
        //    //    {
        //    //        if (!int.TryParse(fTT.SelectedValue, out SensorTaskID))
        //    //        {
        //    //            Error = "Retrieval of Task for Sensor failed";
        //    //        }
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    if (System.Windows.Forms.MessageBox.Show("So far no sensors had been defined for monitoring of the selected collection. Do you want to create one?", "Create sensor task", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
        //    //    {
        //    //        SensorTaskID = CreateSensorTask(ref ErrorLocal);
        //    //    }
        //    //    else
        //    //        Error = "Failed to retrieve task for Sensor";
        //    //}
        //    //return SensorTaskID;
        //}

        //private static int CreateSensorTask(ref string Error)
        //{
        //    return CreateTaskWithinParent("Sensor", "Monitoring", ref Error);

        //    return CreateTaskWithinMonitoring("Sensor", ref Error);
        //}

#region Metric

        public static bool AddMetrics(int CollectionID, int SensorCollectionTaskID, System.Collections.Generic.SortedDictionary<string, PrometheusMetric> Metrics, ref System.Collections.Generic.List<int> CollectionTaskIDs, ref string Error)
        {
            bool OK = true;
            CollectionTaskIDs = getMetricIDs(CollectionID, SensorCollectionTaskID, Metrics, ref Error);
            if (CollectionTaskIDs.Count == 0 || Error.Length > 0) OK = false;
            return OK;
        }

        private static System.Collections.Generic.List<int> getMetricIDs(int CollectionID, int SensorCollectionTaskID, System.Collections.Generic.SortedDictionary<string, PrometheusMetric> Metrics, ref string Error)
        {
            System.Collections.Generic.List<int> MetricIDs = new List<int>();
            try
            {
                string SQL = "SELECT DISTINCT T.TaskID, T.ModuleTitle " +
                    "FROM Task AS T INNER JOIN " +
                    "Task AS S ON T.TaskParentID = S.TaskID INNER JOIN " +
                    "CollectionTask AS C ON S.TaskID = C.TaskID INNER JOIN " +
                    "Task AS M ON S.TaskParentID = M.TaskID AND M.Type = N'Monitoring' " +
                    "WHERE (S.Type = N'Sensor') AND (C.CollectionTaskID = " + SensorCollectionTaskID.ToString() + ") ";
                System.Data.DataTable dtMetrics = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtMetrics, ref Error);
                foreach (System.Collections.Generic.KeyValuePair<string, PrometheusMetric> M in Metrics)
                {
                    System.Data.DataRow[] rr = dtMetrics.Select("ModuleTitle = '" + M.Value.Metric + "' AND ModuleType = 'Prometheus'");
                    if(rr.Length > 0)
                    {

                    }
                    else
                    {

                    }


                    if (dtMetrics.Rows.Count > 0) // sensor contains metrics
                    {
                        foreach (System.Data.DataRow R in dtMetrics.Rows)
                        {
                            if (R["ModuleTitle"].ToString() == M.Value.Metric)
                            {
                                SQL = "INSERT INTO CollectionTask " +
                                    "(TaskID, CollectionTaskParentID, CollectionID" +
                                    ", DisplayText, MetricSource " +
                                    ", MetricDescription, MetricUnit) " +
                                    "VALUES (" + R[0].ToString() + ", " + SensorCollectionTaskID.ToString() + ", " + CollectionID.ToString() +
                                    ", '" + M.Value.MetricDisplayText + "', '" + M.Value.PrometheusIdentifier + "'" +
                                    ", '" + M.Value.MetricIdentifier + "', '" + M.Value.MetricUnitsDisplayText + "'); " +
                                    "SELECT MAX(CollectionTaskID) FROM CollectionTask " +
                                    "WHERE TaskID = " + R[0].ToString() + " AND CollectionTaskParentID = " + SensorCollectionTaskID.ToString() + " AND CollectionID =  " + CollectionID.ToString();
                                int ID = 0;
                                if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Error), out ID))
                                {
                                    if (Error.Length == 0)
                                    {
                                        MetricIDs.Add(ID);
                                    }
                                }
                            }
                        }
                    }
                    else // no metrics for sensor defined
                    {
                        if (System.Windows.Forms.MessageBox.Show("There is no metric defined so far. Do you want to define a new metric?", "No metric", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            // find monitoring for Collection
                            int MonitoringID = GetMonitoringID(CollectionID, "", ref Error);
                            if (Error.Length > 0 || MonitoringID == -1)
                                return MetricIDs;
                            else
                            {
                                // find task for sensor
                                int SensorTaskID = GetTaskIdWithinParent("Sensor", "Monitoring", ref Error, MonitoringID); //GetSensorTaskID(MonitoringID, ref Error);
                                if (Error.Length > 0 || SensorTaskID == -1)
                                    return MetricIDs;
                                else
                                {
                                    foreach (System.Collections.Generic.KeyValuePair<string, PrometheusMetric> PM in Metrics)
                                    {

                                    }
                                }
                            }
                        }
                        else
                        {
                            return MetricIDs;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return MetricIDs;
        }

#endregion

#endregion

        #region Common

        #region Task

        private enum InstallationLevel { IPM, Monitoring, Trap, Pest, Bycatch, All }

        private static int GetTaskID(string Type, int CollectionID, string DateSQL, ref string Error)
        {
            // find TaskID for Collection
            int TaskID = -1;
            string SQL = "SELECT C.CollectionTaskID, case when C.DisplayText <> '' then C.DisplayText else T.DisplayText end AS DisplayText " +
                "FROM CollectionTask AS C INNER JOIN " +
                "Task AS T ON C.TaskID = T.TaskID INNER JOIN " +
                "dbo.CollectionHierarchySuperior(" + CollectionID.ToString() + ") S ON S.CollectionID = C.CollectionID " +
                "AND (T.Type = N'" + Type + "') ";
            if (DateSQL.Length > 0)
                SQL += "AND(C.TaskStart IS NULL OR C.TaskStart <= " + DateSQL + ") " +
                "AND(C.TaskEnd IS NULL OR C.TaskEnd >= " + DateSQL + ") ";
            SQL += "AND (C.DisplayText <> '' OR T.DisplayText <> '')";
            System.Data.DataTable dtCollectionTask = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtCollectionTask, ref Error);
            if (Error.Length > 0)
                return -1;
            if (dtCollectionTask.Rows.Count == 1)
            {
                if (!int.TryParse(dtCollectionTask.Rows[0][0].ToString(), out TaskID))
                    Error = "Conversion for TaskID failed";
            }
            else if (dtCollectionTask.Rows.Count > 1)
            {
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtCollectionTask, "DisplayText", "CollectionTaskID", "Please select a " + Type.ToLower() + " from the list", Type, "", false, true, true, TaskImage(Type));
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    if (!int.TryParse(f.SelectedValue, out TaskID))
                        Error = "Selection for TaskID failed";
                }
            }
            else
            {
                if (System.Windows.Forms.MessageBox.Show("So far no " + Type.ToLower() + " has been defined for the selected collection. Do you want to create a " + Type.ToLower() + " task?", "Create " + Type.ToLower() + " task", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    TaskID = CreateMonitoringCollectionTask(CollectionID, ref Error);
                }
            }
            return TaskID;
        }

        private static int getTaskIDWithinParent(string Type, string ParentType, int CollectionID, ref string Error)
        {
            int TaskID = -1;
            try
            {
                string SQL = "SELECT DISTINCT T.TaskID, T.DisplayText + CASE WHEN T.Description <> '' THEN ' [' + T.Description + ']' ELSE '' END AS DisplayText " +
                    "FROM Task AS T INNER JOIN " +
                    "Task AS M ON T.TaskParentID = M.TaskID INNER JOIN " +
                    "CollectionTask AS C ON M.TaskID = C.TaskID " +
                    "WHERE (T.Type = N'" + Type + "') AND (M.Type = N'" + ParentType + "') ";
                System.Data.DataTable dtTasks = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTasks, ref Error);
                if (dtTasks.Rows.Count == 1)
                {
                    if (!int.TryParse(dtTasks.Rows[0][0].ToString(), out TaskID))
                    {
                        TaskID = -1;
                    }
                }
                else if (dtTasks.Rows.Count > 1) // more then 1 sensor
                {
                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtTasks, "DisplayText", "TaskID", "Please select a " + Type.ToLower() + " from the list", Type, "", false, true, true, TaskImage(Type));
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        if (!int.TryParse(f.SelectedValue, out TaskID))
                            TaskID = -1;
                    }
                }
                else // no sensor
                {
                    if (System.Windows.Forms.MessageBox.Show("There is no " + Type.ToLower() + " defined so far. Do you want to define a new " + Type.ToLower() + "?", "No " + Type.ToLower(), System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        // find parent task for Collection
                        int ParentTaskID = GetTaskID(ParentType, CollectionID, "", ref Error);// GetTaskIdWithinParent(Type, ParentType, -1, ref Error); //etMonitoringID(CollectionID, "", ref Error);
                        if (Error.Length > 0 || ParentTaskID == -1)
                            TaskID = -1;
                        else
                        {
                            // find taskID for type
                            TaskID = GetTaskIdWithinParent(Type, ParentType, ref Error);
                            if (Error.Length > 0 || TaskID == -1)
                                TaskID = -1;
                            else
                            {
                                DiversityWorkbench.Forms.FormGetString fGetTask = new DiversityWorkbench.Forms.FormGetString("New " + Type.ToLower(), "Please enter the name for the new " + Type.ToLower(), "", TaskImage(Type));
                                fGetTask.ShowDialog();
                                if (fGetTask.DialogResult == System.Windows.Forms.DialogResult.OK && fGetTask.String.Length > 0)
                                {
                                    string SensorTitle = fGetTask.String;
                                    SQL = "INSERT INTO CollectionTask (CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, DisplayText) " +
                                        "VALUES(" + ParentTaskID.ToString() + ", " + CollectionID.ToString() + ", " + TaskID.ToString() + ", 1, '" + SensorTitle + "') " +
                                        "; SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];";
                                    int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Error), out TaskID);
                                    if (TaskID == 0)
                                    {
                                        SQL = "SELECT MAX(CollectionTaskID) FROM CollectionTask WHERE CollectionTaskParentID = " + ParentTaskID.ToString() + " AND TaskID = " + TaskID.ToString() + " AND DisplayOrder = 1";
                                        int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out TaskID);
                                    }
                                }
                                else
                                {
                                    Error = "No " + Type.ToLower() + " available";
                                }
                            }
                        }
                    }
                    else
                    {
                        TaskID = -1;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return TaskID;
        }

        #region init

        public static bool InitIPM()
        {
            bool OK = false;
            try 
            {
                OK = IPMisInstalled();
                if (!OK)
                {
                    if (!IPMisInstalled(InstallationLevel.IPM)) InitIPM(InstallationLevel.IPM);
                    if (!IPMisInstalled(InstallationLevel.Monitoring)) InitIPM(InstallationLevel.Monitoring);
                    if (!IPMisInstalled(InstallationLevel.Trap)) InitIPM(InstallationLevel.Trap);
                    if (!IPMisInstalled(InstallationLevel.Pest)) InitIPM(InstallationLevel.Pest);
                    if (!IPMisInstalled(InstallationLevel.Bycatch)) InitIPM(InstallationLevel.Bycatch);
                    OK = IPMisInstalled();
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return OK; 
        }

        private static bool IPMisInstalled(InstallationLevel installationLevel = InstallationLevel.All)
        {
            bool OK = false;
            try
            {
                string SQL = "SELECT COUNT(*) " +
                    "FROM Task I ";
                if (installationLevel > InstallationLevel.IPM || installationLevel == InstallationLevel.All)
                    SQL += "INNER JOIN Task M ON M.TaskParentID = I.TaskID AND  M.Type = 'Monitoring' AND M.DisplayText = 'Monitoring' ";
                if (installationLevel == InstallationLevel.Trap || installationLevel == InstallationLevel.All)
                    SQL += "INNER JOIN Task T ON T.TaskParentID = M.TaskID AND T.Type = 'Trap' AND T.DisplayText = 'Trap' ";
                if (installationLevel == InstallationLevel.Pest || installationLevel == InstallationLevel.All)
                    SQL += "INNER JOIN Task P ON P.TaskParentID = M.TaskID AND P.Type = 'Pest' AND P.DisplayText = 'Pest' ";
                if (installationLevel == InstallationLevel.Bycatch || installationLevel == InstallationLevel.All)
                    SQL += "INNER JOIN Task B ON B.TaskParentID = M.TaskID AND B.Type = 'Bycatch' AND B.DisplayText = 'Bycatch' ";
                SQL += "WHERE I.Type = 'IPM' AND I.DisplayText = 'IPM' ";
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                int i;
                OK = int.TryParse(Result, out i) && i > 0;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return OK;
        }

        private static void InitIPM(InstallationLevel installationLevel)
        {
            try
            {
                string SQL = "INSERT INTO Task " +
                    "(TaskParentID, DisplayText, Type, ModuleTitle, ModuleType, SpecimenPartType, DateType, DateBeginType, NumberType, DescriptionType, NotesType, ResponsibleType) " +
                    "SELECT ";
                // TaskParentID
                if (installationLevel > InstallationLevel.IPM) 
                {
                    switch (installationLevel)
                    {
                        case InstallationLevel.Monitoring:
                            SQL += "I.TaskID "; 
                            break;
                        default:
                            SQL += "M.TaskID ";
                            break;
                    }
                }
                else SQL += "NULL ";

                // DisplayText
                SQL += ", '" + installationLevel.ToString() + "' ";

                // Type
                SQL += ", '" + installationLevel.ToString() + "' ";

                // ModuleTitle
                if (installationLevel == InstallationLevel.Pest || installationLevel == InstallationLevel.Bycatch)
                    SQL += ", '" + installationLevel.ToString() + "' ";
                else SQL += ", NULL ";

                // ModuleType
                if (installationLevel == InstallationLevel.Pest || installationLevel == InstallationLevel.Bycatch)
                    SQL += ", 'DiversityTaxonNames '";
                else SQL += ", NULL ";

                // SpecimenPartType
                if (installationLevel == InstallationLevel.Pest || installationLevel == InstallationLevel.Bycatch)
                    SQL += ", 'Part' ";
                else SQL += ", NULL ";

                // DateType
                if (installationLevel == InstallationLevel.Pest || installationLevel == InstallationLevel.Bycatch)
                    SQL += ", 'Date' ";
                else SQL += ", NULL ";

                // DateBeginType
                if (installationLevel == InstallationLevel.Pest || installationLevel == InstallationLevel.Bycatch)
                    SQL += ", 'Date' ";
                else SQL += ", NULL ";

                // NumberType
                if (installationLevel == InstallationLevel.Pest || installationLevel == InstallationLevel.Bycatch)
                    SQL += ", 'Count' ";
                else SQL += ", NULL ";

                // DescriptionType
                if (installationLevel == InstallationLevel.Pest || installationLevel == InstallationLevel.Bycatch)
                    SQL += ", 'Common name' ";
                else SQL += ", NULL";

                // NotesType
                if (installationLevel == InstallationLevel.Pest || installationLevel == InstallationLevel.Bycatch)
                    SQL += ", 'Notes' ";
                else SQL += ", NULL ";

                // ResponsibleType
                if (installationLevel == InstallationLevel.Pest || installationLevel == InstallationLevel.Bycatch)
                    SQL += ", 'Responsible' ";
                else SQL += ", NULL ";

                if (installationLevel > InstallationLevel.IPM)
                    SQL += "FROM Task AS I ";
                if (installationLevel > InstallationLevel.Monitoring) 
                    SQL += " INNER JOIN Task M ON M.TaskParentID = I.TaskID AND  M.Type = 'Monitoring' AND M.DisplayText = 'Monitoring' ";
                if (installationLevel > InstallationLevel.IPM)
                    SQL += "WHERE (I.Type = 'IPM') AND (I.DisplayText = 'IPM')";
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion

        private static int GetTaskIdWithinParent(string Type, string ParentType, ref string Error, int? CollectionTaskParentID = null)
        {
            int TaskID = -1;
            try
            {
                // try to find the task
                string SQL = "SELECT T.DisplayText, T.TaskID " +
                    "FROM[dbo].[CollectionTask] C " +
                    "INNER JOIN Task M ON  C.TaskID = M.TaskID AND M.Type = '" + ParentType + "' ";
                if (CollectionTaskParentID != null)
                    SQL += "AND C.CollectionTaskID = " + CollectionTaskParentID.ToString() + " ";
                SQL += "INNER JOIN Task T ON T.TaskParentID = M.TaskID AND T.Type = '" + Type + "'";
                System.Data.DataTable dtTask = new System.Data.DataTable();
                string ErrorLocal = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTask, ref ErrorLocal);
                if (ErrorLocal.Length > 0)
                    return -1;
                if (dtTask.Rows.Count == 1)
                {
                    int.TryParse(dtTask.Rows[0][1].ToString(), out TaskID);
                }
                else if (dtTask.Rows.Count > 1)
                {
                    DiversityWorkbench.Forms.FormGetStringFromList fTT = new DiversityWorkbench.Forms.FormGetStringFromList(dtTask, "DisplayText", "TaskID", "Please select a " + Type.ToLower() + " from the list", Type, "", false, true, true, TaskImage(Type));
                    fTT.ShowDialog();
                    if (fTT.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        if (!int.TryParse(fTT.SelectedValue, out TaskID))
                        {
                            Error = "Retrieval of Task for " + Type + " failed";
                        }
                    }
                }
                else
                {
                    if (System.Windows.Forms.MessageBox.Show("So far no " + Type + " had been defined for monitoring of the selected collection. Do you want to create one?", "Create " + Type.ToLower() + " task", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        TaskID = CreateTaskWithinMonitoring(Type, ref ErrorLocal);
                    }
                    else
                        Error = "Failed to retrieve task for " + Type + "";
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return TaskID;
        }

        private static int CreateTaskWithinParent(string Type, string ParentType, ref string Error)
        {
            int TaskID = -1;
            int TaskParentID = GetTaskIdWithinParent(Type, ParentType, ref Error);
            string SQL = "INSERT INTO Task (TaskParentID, DisplayText, Type) VALUES(" + TaskParentID.ToString() + ", '" + Type + "', '" + Type + "')";
            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error))
            {
                SQL = "SELECT MAX(T.TaskID) " +
                    "FROM Task AS T WHERE (T.Type = N'" + Type + "') AND TaskParentID = " + TaskParentID.ToString();
                if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out TaskID))
                {
                    Error = "Conversion of TaskID for " + Type + " failed";
                    TaskID = -1;
                }
            }
            return TaskID;
        }

        #endregion

        #region CollectionTask

        /// <summary>
        /// Getting the CollectionTaskID for task within a collection - in case non is found, create all needed entries
        /// </summary>
        /// <param name="Type">The type of the task</param>
        /// <param name="ParentType">The type of the parent task</param>
        /// <param name="CollectionID">The ID of the collection where the task should be inserted</param>
        /// <param name="Error">A possible Error</param>
        /// <returns>The ID of the new CollectionTask</returns>
        private static int CreateCollectionTaskWithinParent(string Type, string ParentType, int CollectionID, ref string Error)
        {
            // getting the TaskID for the new item
            int TaskID = GetTaskIdWithinParent(Type, ParentType, ref Error);


            int CollectionTaskID = -1;
            try
            {
                // Listing all tasks that fit the type + parent type restriction
                string SQL = "SELECT DISTINCT T.TaskID, T.DisplayText + CASE WHEN T .Description <> '' THEN ' [' + T .Description + ']' ELSE '' END AS DisplayText " +
                    "FROM Task AS T INNER JOIN " +
                    "Task AS M ON T.TaskParentID = M.TaskID INNER JOIN " +
                    "CollectionTask AS C ON M.TaskID = C.TaskID " +
                    "WHERE (T.Type = N'" + Type + "') AND (M.Type = N'" + ParentType + "') ";
                System.Data.DataTable dtTasks = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTasks, ref Error);
                if (dtTasks.Rows.Count == 1)
                {
                    if (!int.TryParse(dtTasks.Rows[0][0].ToString(), out TaskID))
                    {
                        CollectionTaskID = -1;
                    }
                }
                else if (dtTasks.Rows.Count > 1) // more then 1 sensor
                {
                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtTasks, "DisplayText", "CollectionTaskID", "Please select a " + Type.ToLower() + " from the list", Type, "", false, true, true, TaskImage(Type));
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        if (!int.TryParse(f.SelectedValue, out CollectionTaskID))
                            CollectionTaskID = -1;
                    }
                }
                else // no sensor
                {
                    if (System.Windows.Forms.MessageBox.Show("There is no " + Type.ToLower() + " defined so far. Do you want to define a new " + Type.ToLower() + "?", "No " + Type.ToLower(), System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        // find parent task for Collection
                        int ParentTaskID = GetTaskID(ParentType, CollectionID, "", ref Error);// GetTaskIdWithinParent(Type, ParentType, -1, ref Error); //etMonitoringID(CollectionID, "", ref Error);
                        if (Error.Length > 0 || ParentTaskID == -1)
                            CollectionTaskID = -1;
                        else
                        {
                            // find taskID for type
                            TaskID = GetTaskIdWithinParent(Type, ParentType, ref Error);
                            if (Error.Length > 0 || TaskID == -1)
                                CollectionTaskID = -1;
                            else
                            {
                                DiversityWorkbench.Forms.FormGetString fGetTask = new DiversityWorkbench.Forms.FormGetString("New " + Type.ToLower(), "Please enter the name for the new " + Type.ToLower(), "", TaskImage(Type));
                                fGetTask.ShowDialog();
                                if (fGetTask.DialogResult == System.Windows.Forms.DialogResult.OK && fGetTask.String.Length > 0)
                                {
                                    string SensorTitle = fGetTask.String;
                                    SQL = "INSERT INTO CollectionTask (CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, DisplayText) " +
                                        "VALUES(" + ParentTaskID.ToString() + ", " + CollectionID.ToString() + ", " + TaskID.ToString() + ", 1, '" + SensorTitle + "') " +
                                        "; SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];";
                                    int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Error), out CollectionTaskID);
                                    if (CollectionTaskID == 0)
                                    {
                                        SQL = "SELECT MAX(CollectionTaskID) FROM CollectionTask WHERE CollectionTaskParentID = " + ParentTaskID.ToString() + " AND TaskID = " + TaskID.ToString() + " AND DisplayOrder = 1";
                                        int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out CollectionTaskID);
                                    }
                                }
                                else
                                {
                                    Error = "No " + Type.ToLower() + " available";
                                }
                            }
                        }
                    }
                    else
                    {
                        CollectionTaskID = -1;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return CollectionTaskID;
        }

#endregion

        #region image

        private static System.Drawing.Image TaskImage(string Type)
        {
            System.Drawing.Image image = DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Task);
            switch (Type.ToLower())
            {
                case "trap":
                    image = DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Trap);
                    break;
                case "sensor":
                    image = DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Sensor);
                    break;
                case "monitoring":
                    image = DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Graph);
                    break;
            }
            return image;
        }

#endregion

        private static int CreateTaskWithinMonitoring(string Type, ref string Error)
        {
            return CreateTaskWithinParent(Type, "Monitoring", ref Error);

            //int TaskID = -1;
            //int TaskParentID = GetMonitoringTaskID(ref Error);
            //string SQL = "INSERT INTO Task (TaskParentID, DisplayText, Type) VALUES(" + TaskParentID.ToString() + ", '" + Type + "', '" + Type + "')";
            //if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error))
            //{
            //    SQL = "SELECT MAX(T.TaskID) " +
            //        "FROM Task AS T WHERE (T.Type = N'" + Type + "') AND TaskParentID = " + TaskParentID.ToString();
            //    if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out TaskID))
            //    {
            //        Error = "Conversion of TaskID for " + Type + " failed";
            //        TaskID = -1;
            //    }
            //}
            //return TaskID;
        }

        private static int CreateCollectionTask(string Type, int CollectionID, ref string Error)
        {
            int CollectionTaskID = -1;
            int TaskID = GetTaskID(Type, CollectionID, "", ref Error);
            int SuperiorCollectionID = GetSuperiorCollectionID(CollectionID, ref Error);
            if (TaskID > -1 && SuperiorCollectionID > -1)
            {
                string SQL = "INSERT INTO CollectionTask (CollectionID, DisplayText, TaskID) VALUES (" + SuperiorCollectionID.ToString() + ", '" + Type + "', " + TaskID.ToString() + ")";
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error))
                {
                    SQL = "SELECT MAX(T.CollectionTaskID) " +
                        "FROM CollectionTask AS T WHERE (T.TaskID = " + TaskID.ToString() + ") AND CollectionID = " + SuperiorCollectionID.ToString();
                    if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out CollectionTaskID))
                        Error = "Conversion for CollectionTaskID failed";
                }
            }
            return CollectionTaskID;
        }

        public static bool AddSpecimen(int CollectionID, string DateSQL)
        {
            bool OK = false;
            try
            {
                //if (int.TryParse(r["CollectionID"].ToString(), out CollectionID))
                {
                    string SQL = "SELECT  " +
                        "CASE WHEN P.AccessionNumber <> '' THEN P.AccessionNumber ELSE CASE WHEN S.AccessionNumber <> '' THEN S.AccessionNumber ELSE '' END END AS [Accession number], " +
                        "CASE WHEN P.AccessionNumber <> '' THEN P.AccessionNumber ELSE CASE WHEN S.AccessionNumber <> '' THEN S.AccessionNumber ELSE '[' + CAST( P.CollectionSpecimenID AS VARCHAR) + '-' + CAST( P.SpecimenPartID AS VARCHAR) + ']' END END " +
                        " + ' ' + CASE WHEN P.PartSublabel <> '' THEN P.PartSublabel ELSE '' END " +
                        " + ' (' + P.MaterialCategory + ')' AS Specimen, " +
                        "S.CollectionSpecimenID, P.SpecimenPartID " +
                        "FROM CollectionSpecimen AS S INNER JOIN " +
                        "CollectionSpecimenPart AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID AND (S.AccessionNumber <> '' OR P.AccessionNumber <> '') " +
                        "WHERE P.CollectionID = " + CollectionID.ToString();
                    System.Data.DataTable dt = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                    if (dt.Rows.Count > 0)
                    {
                        System.Collections.Generic.List<int> HiddenColumns = new List<int>();
                        HiddenColumns.Add(2);
                        HiddenColumns.Add(3);
                        DiversityWorkbench.Forms.FormGetRowFromTable f = new DiversityWorkbench.Forms.FormGetRowFromTable("Specimen", "Please select the specimen where the pest was found", dt, true, HiddenColumns);
                        f.setFilterColumn(0);
                        f.setOperator(1);
                        f.ShowDialog();
                        if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.SeletedRow() != null)
                        {
                            System.Data.DataRow R = f.SeletedRow();
                            SQL = "INSERT INTO CollectionTask " +
                                "(CollectionID, TaskID, DisplayText, " +
                                "CollectionSpecimenID, SpecimenPartID, TaskStart, " +
                                "ResponsibleAgent, ResponsibleAgentURI) " +
                                "SELECT " + CollectionID.ToString() + ", P.TaskID, '" + R["Specimen"].ToString() + "', "
                                + R["CollectionSpecimenID"].ToString() + ", " + R["SpecimenPartID"].ToString() + ", " + DateSQL + ", " +
                                "U.CombinedNameCache, U.AgentURI " +
                                "FROM UserProxy AS U CROSS JOIN " +
                                "Task AS I INNER JOIN " +
                                "Task AS M ON M.TaskParentID = I.TaskID AND M.Type = 'Monitoring' AND M.DisplayText = 'Monitoring' INNER JOIN " +
                                "Task P ON P.TaskParentID = M.TaskID AND P.Type = 'Pest' AND P.DisplayText = 'Pest'  " +
                                "WHERE (I.Type = 'IPM') AND (I.DisplayText = 'IPM') AND (U.ID = dbo.UserID()) ";
                            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                                OK = true;
                        }
                        else System.Windows.Forms.MessageBox.Show("Nothing selected");
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("So far no specimen are registered for this collection");
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); OK = false; }
            return OK;
        }

        public static bool AddCollection(int CollectionID, string CollectionName, string DateSQL)
        {
            bool OK = false;
            try
            {
                // Check if there is a content with the same parameters
                string SQL = "SELECT TOP 1 CollectionTaskID " +
                    " FROM CollectionTask AS C " +
                    " INNER JOIN Task AS M ON M.TaskID = C.TaskID AND M.Type = 'Monitoring' AND C.TaskStart = " + DateSQL + " AND C.CollectionID = " + CollectionID.ToString() +
                    " INNER JOIN Task AS I ON I.TaskID = M.TaskParentID AND (I.Type = 'IPM') AND (I.DisplayText = 'IPM')";
                int CollectionTaskID;
                if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true), out CollectionTaskID))
                {
                    SQL = "INSERT INTO CollectionTask " +
                        "(CollectionID, TaskID, DisplayText, TaskStart, " +
                        "ResponsibleAgent, ResponsibleAgentURI) " +
                        "SELECT " + CollectionID.ToString() + ", M.TaskID, '" + CollectionName + "', " + DateSQL + ", " +
                        "U.CombinedNameCache, U.AgentURI " +
                        "FROM UserProxy AS U CROSS JOIN " +
                        "Task AS I INNER JOIN " +
                        "Task AS M ON M.TaskParentID = I.TaskID AND M.Type = 'Monitoring' AND M.DisplayText = 'Monitoring' " +
                        "WHERE (I.Type = 'IPM') AND (I.DisplayText = 'IPM') AND (U.ID = dbo.UserID()) ";
                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                        OK = true;
                }
                else
                    OK = true;

            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); OK = false; }
            return OK;
        }

        public static bool AddTransaction(int CollectionID, string DateSQL)
        {
            bool OK = false;
            try
            {
                //if (int.TryParse(r["CollectionID"].ToString(), out CollectionID))
                {
                    string SQL = "SELECT T.TransactionTitle + ' (' + T.TransactionType + ')' AS DisplayText, " +
                        "T.TransactionID " +
                        "FROM [Transaction] AS T " +
                        "WHERE T.AdministratingCollectionID = " + CollectionID.ToString() +
                        " AND T.TransactionType IN ('borrow', 'exchange', 'gift', 'inventory', 'purchase', 'removal', 'return', 'transaction group')";
                    System.Data.DataTable dt = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                    if (dt.Rows.Count > 0)
                    {
                        System.Collections.Generic.List<int> HiddenColumns = new List<int>();
                        HiddenColumns.Add(1);
                        DiversityWorkbench.Forms.FormGetRowFromTable f = new DiversityWorkbench.Forms.FormGetRowFromTable("Convolute", "Please select the convolute or corresponding where the pests were found", dt, true, HiddenColumns);
                        f.setFilterColumn(0);
                        f.setOperator(1);
                        f.ShowDialog();
                        if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.SeletedRow() != null)
                        {
                            System.Data.DataRow R = f.SeletedRow();
                            SQL = "INSERT INTO CollectionTask " +
                                "(CollectionID, TaskID, DisplayText, " +
                                "TransactionID, TaskStart, " +
                                "ResponsibleAgent, ResponsibleAgentURI) " +
                                "SELECT " + CollectionID.ToString() + ", P.TaskID, '" + R["DisplayText"].ToString() + "', "
                                + R["TransactionID"].ToString() + ", " + DateSQL + ", " +
                                "U.CombinedNameCache, U.AgentURI " +
                                "FROM UserProxy AS U CROSS JOIN " +
                                "Task AS I INNER JOIN " +
                                "Task AS M ON M.TaskParentID = I.TaskID AND M.Type = 'Monitoring' AND M.DisplayText = 'Monitoring' INNER JOIN " +
                                "Task P ON P.TaskParentID = M.TaskID AND P.Type = 'Pest' AND P.DisplayText = 'Pest'  " +
                                "WHERE (I.Type = 'IPM') AND (I.DisplayText = 'IPM') AND (U.ID = dbo.UserID()) ";
                            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                                OK = true;
                        }
                        else System.Windows.Forms.MessageBox.Show("Nothing selected");
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("So far no transactions are registered for this collection");
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); OK = false; }
            return OK;
        }

        private static System.Collections.Generic.List<string> _CollectionTypeContainingPests;

        public static System.Collections.Generic.List<string> CollectionTypeContainingPests
        {
            get
            {
                if (_CollectionTypeContainingPests == null)
                {
                    _CollectionTypeContainingPests = new List<string>();
                    string SQL = "SELECT [Code] " +
                        "FROM [dbo].[CollCollectionType_Enum] C " +
                        "WHERE C.Code NOT IN ('area', 'freezer', 'fridge', 'hardware', 'radioactive', 'sensor', 'steel locker')";
                    System.Data.DataTable dt = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                    foreach (System.Data.DataRow R in dt.Rows)
                        _CollectionTypeContainingPests.Add(R[0].ToString());
                }
                return _CollectionTypeContainingPests;
            }
        }

        //private static int GetTaskIdWithinMonitoring(string Type, int MonitoringID, ref string Error)
        //{
        //    return GetTaskIdWithinParent(Type, "Monitoring", MonitoringID, ref Error);

        //    //int TaskID = -1;
        //    //try
        //    //{
        //    //    // try to find the task
        //    //    string SQL = "SELECT T.DisplayText, T.TaskID " +
        //    //        "FROM[dbo].[CollectionTask] C " +
        //    //        "INNER JOIN Task M ON  C.TaskID = M.TaskID AND M.Type = 'Monitoring' AND C.CollectionTaskID = " + MonitoringID.ToString() + " " +
        //    //        "INNER JOIN Task T ON T.TaskParentID = M.TaskID AND T.Type = '" + Type + "'";
        //    //    System.Data.DataTable dtTask = new System.Data.DataTable();
        //    //    string ErrorLocal = "";
        //    //    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTask, ref ErrorLocal);
        //    //    if (ErrorLocal.Length > 0)
        //    //        return -1;
        //    //    if (dtTask.Rows.Count == 1)
        //    //    {
        //    //        int.TryParse(dtTask.Rows[0][1].ToString(), out TaskID);
        //    //    }
        //    //    else if (dtTask.Rows.Count > 1)
        //    //    {
        //    //        System.Drawing.Image image = DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Task);
        //    //        switch (Type.ToLower())
        //    //        {
        //    //            case "trap":
        //    //                image = DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Trap);
        //    //                break;
        //    //            case "sensor":
        //    //                image = DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Sensor);
        //    //                break;
        //    //        }
        //    //        DiversityWorkbench.Forms.FormGetStringFromList fTT = new DiversityWorkbench.Forms.FormGetStringFromList(dtTask, "DisplayText", "TaskID", "Please select a " + Type.ToLower() + " from the list", Type, "", false, true, true, image);
        //    //        fTT.ShowDialog();
        //    //        if (fTT.DialogResult == System.Windows.Forms.DialogResult.OK)
        //    //        {
        //    //            if (!int.TryParse(fTT.SelectedValue, out TaskID))
        //    //            {
        //    //                Error = "Retrieval of Task for " + Type + " failed";
        //    //            }
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        if (System.Windows.Forms.MessageBox.Show("So far no " + Type + " had been defined for monitoring of the selected collection. Do you want to create one?", "Create " + Type.ToLower() + " task", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
        //    //        {
        //    //            TaskID = CreateTaskWithinMonitoring(Type, ref ErrorLocal);
        //    //        }
        //    //        else
        //    //            Error = "Failed to retrieve task for " + Type + "";
        //    //    }
        //    //}
        //    //catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //    //return TaskID;
        //}

        #endregion

    }
}
