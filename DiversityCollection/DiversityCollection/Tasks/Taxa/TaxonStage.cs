using DiversityWorkbench;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DiversityCollection.Tasks.IPM;

namespace DiversityCollection.Tasks.Taxa
{
    public class TaxonStage : iItem
    {

        #region Parameter

        private Tasks.Taxa.Taxon _Taxon;

        private string _Identifier;
        //private int _NameID;
        //private string _BaseURL;
        //private string _ScientificName;
        //private string _VernacularName;
        //private bool _IsAcceptedName = false;
        //private int? _AcceptedNameID = null;
        //private int? _ParentNameID = null;
        private string _Group;
        //private string _Rank;
        private IPM.TaxonSource _taxonSource;
        //private System.Collections.Generic.Dictionary<int, Resource> _Images;
        //private System.Collections.Generic.Dictionary<int, Resource> _Infos;
        //Resource _PreviewImage;
        Preview _Preview;
        private string _Stage;
        private int _StageID;
        //private System.Collections.Generic.Dictionary<int, string> _Stages;
        //private System.Collections.Generic.Dictionary<int, Preview> _PreviewImages;
        //private static System.Collections.Generic.Dictionary<int, string> _Stages;
        //private string _PreviewImage;
        private int _Sorting;

        #endregion

        #region Interface

        public string Identifier { get => _Identifier; }

        public int Sorting { get => _Sorting; }

        public string Stage { get => _Stage; }
        public int StageID { get => _StageID; }
        //public int AcceptedNameID { set => _AcceptedNameID = value; }

        public int NameID { get => _Taxon.NameID; }
        //public string BaseURL { get => _BaseURL; }
        public string ScientificName { get => _Taxon.ScientificName; }
        public string VernacularName { get => _Taxon.VernacularName; }
        //public bool AcceptedName { get => _AcceptedNameID == _NameID; }
        public string Group { get => _Group; }
        //public IPM.TaxonSource taxonSource { get => _taxonSource; }
        public Preview PreviewImage { get => _Preview; }
        public System.Collections.Generic.Dictionary<int, Resource> Images 
        {
            get
            {
                return _Taxon.Images;
            }
        }
        public System.Collections.Generic.Dictionary<int, Resource> Infos 
        { 
            get
            {
                return _Taxon.Infos;
            }
        }

        public string DisplayText(bool IncludeStage = true)
        {
            string Name = "";
            try
            {
                Name = _Taxon.DisplayText;
                //if (VernacularName == null && ScientificName == null)
                //    this.initTaxonStage();
                //if (VernacularName != null)
                //    Name = VernacularName;
                //if (Name.Length == 0 && ScientificName != null && ScientificName.Length > 0) // && Name.IndexOf("(") == -1)
                //    Name = ScientificName;
#if DEBUG
                if (Name.Trim().Length == 0)
                {

                }
#endif
                if (IncludeStage && this.Stage != null && this.Stage.Length > 0)
                {
                    if (!Name.EndsWith("\r\n"))
                        Name += "\r\n";
                    Name += this.Stage;
                }
#if DEBUG
                if (Name.Trim().Length == 0)
                {

                }
#endif
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return Name;
        }

        public string NameURI()
        {
            return Database.TaxonDBBaseURL + NameID.ToString();
        }

        #endregion

        #region Construction

        public TaxonStage(string Identifier, IPM.TaxonSource taxonSource)
        {
            _Identifier = Identifier;
            _taxonSource = taxonSource;
            this.initTaxonStage();
        }

        #endregion

        #region init

        private void initTaxonStage()
        {
            try
            {
                string nameID = _Identifier.Substring(Database.TaxonDBBaseURL.Length);
                if (nameID.IndexOf("|") > -1)
                {
                    string stageID = nameID.Substring(nameID.IndexOf("|") + 1);
                    int.TryParse(stageID, out _StageID);
                    nameID = nameID.Substring(0, nameID.IndexOf("|"));
                }
                int NameID;
                if (int.TryParse(nameID, out NameID))
                {
                    this._Taxon = new Taxon(NameID, this._taxonSource);

                    this._Group = Groups.GroupForTaxon(this._taxonSource, NameID); //.GetTaxonGroup(_taxonSource, NameID);//.getGroup(_taxonSource, NameID);

                    this._Sorting = Tasks.Taxa.Sorting.SortingForTaxon(_taxonSource, NameID, StageID);
                }

                //if (int.TryParse(NameID, out _NameID))
                //{
                //    Tasks.Taxa.Taxon taxon = new Taxon(_NameID);
                //    this._ScientificName = taxon.ScientificName;
                //    this._ParentNameID = taxon._ParentNameID;
                //    //this._PreviewImage = taxon._PreviewImage;
                //    this._Rank = taxon._Rank;
                //    string CommonName = Database.CommonName(_NameID);
                //    if (CommonName.Length == 0)
                //        CommonName = this._ScientificName;
                //    this._VernacularName = CommonName;

                //    this._Group = Groups.getGroup(_taxonSource, _NameID);

                //    //this._Group = Groups.GetTaxonGroup(_taxonSource, _NameID);

                //    //this.initScientificName();
                //    //this.initCommonNames();
                //    this.initInfos();
                //    this.initImages();
                //}
                this.initStage();
                this.initPreviewImage();
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        //private bool initScientificName()
        //{
        //    bool OK = false;
        //    try
        //    {
        //        string SQL = "SELECT T.TaxonNameCache AS Taxon " +
        //            "FROM " + Database.TaxonDBPrefix + "TaxonName AS T " +
        //            " WHERE T.NameID = " + NameID.ToString();
        //        _ScientificName = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
        //        OK = _ScientificName.Length > 0;
        //    }
        //    catch (System.Exception ex) { OK = false; DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //    return OK;
        //}

        private void initStage()
        {
            if (_Identifier != null && _Identifier.IndexOf("|") > -1)
            {
                string NameID = _Identifier.Substring(Database.TaxonDBBaseURL.Length);
                int AnalysisID;
                if (int.TryParse(NameID.Substring(NameID.IndexOf("|") + 1), out AnalysisID))
                {
                    _StageID = AnalysisID;
                    _Stage = Taxa.Stages.Stage(AnalysisID);
                }
            }
            else _Stage = "";
        }

        private void initPreviewImage()
        {
            try
            {

                string URI = Tasks.Taxa.TaxonResource.PreviewImage(this._Taxon.NameID, this._StageID);
                _Preview = new Preview();
                _Preview.NameID = _Taxon.NameID;
                _Preview.StageID = _StageID;
                _Preview.URI = URI;
                string Message = "";
                _Preview.Icon = DiversityWorkbench.Forms.FormFunctions.BitmapFromWeb(_Preview.URI.ToString(), ref Message);

                //_PreviewImage = new Resource();
                //_

                //string SQL = "SELECT TOP 1 Im.NameID, Im.URI AS Image, Im.Notes, Im.CopyrightStatement, Im.DisplayOrder " +
                //    "FROM " + Database.TaxonDBPrefix + "ViewTaxonNameResource AS Im " +
                //    "WHERE Im.ResourceType = 'preview' AND (Im.NameID = " + NameID.ToString() + ") ";
                //if (this._Stage.Length > 0)
                //{
                //    SQL += " AND Im.Title = '" + this._Stage + "'";
                //}
                //    SQL += " ORDER BY Im.DisplayOrder";
                //System.Data.DataTable dt = new System.Data.DataTable();
                //DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                //if (dt.Rows.Count > 0)
                //{
                //    foreach (System.Data.DataRow R in dt.Rows)
                //    {
                //        _PreviewImage = new Resource();
                //        _PreviewImage.Type = ResourceType.Preview;
                //        _PreviewImage.Title = this.DisplayText();
                //        if (!R["Image"].Equals(System.DBNull.Value) && R["Image"].ToString().Length > 0)
                //        {
                //            _PreviewImage.Uri = new Uri(R["Image"].ToString());
                //            string Message = "";
                //            _PreviewImage.Icon = DiversityWorkbench.Forms.FormFunctions.BitmapFromWeb(_PreviewImage.Uri.ToString(), ref Message);
                //        }
                //        if (!R["Notes"].Equals(System.DBNull.Value) && R["Notes"].ToString().Length > 0)
                //            _PreviewImage.Notes = R["Notes"].ToString();
                //        _PreviewImage.CopyRight = R["CopyrightStatement"].ToString();
                //        break;
                //    }
                //}
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        #endregion

        #region TaxonInfos

        public bool IsAcceptedName
        {
            get { return true; }
        }

        //private System.Drawing.Image _PreviewImage;
        //public System.Drawing.Image PreviewImage
        //{
        //    get
        //    {
        //        if (this._PreviewImage == null)
        //        {

        //        }
        //        return null;
        //    }
        //}

        //private static System.Collections.Generic.Dictionary<int, Tasks.Taxa.TaxonStage> _TaxaInDatabase;

        //private static System.Collections.Generic.Dictionary<int, Tasks.Taxa.TaxonStage> TaxaInDatabase
        //{
        //    get
        //    {
        //        if (_TaxaInDatabase ==null )
        //        {
        //            _TaxaInDatabase = new Dictionary<int, TaxonStage>();
        //            foreach(System.Data.DataRow R in Database.DtTaxa.Rows)
        //            {
        //                int ID = 0;
        //                if(int.TryParse(R["NameID"].ToString(), out ID))
        //                {
        //                    Taxa.TaxonStage taxon = new TaxonStage(ID);
        //                    taxon._ScientificName = R["TaxonNameCache"].ToString();
        //                    taxon._Rank = R["TaxonomicRank"].ToString();
        //                    string Name = R["GenusOrSupragenericName"].ToString();
        //                    string Species = R["SpeciesEpithet"].ToString();
        //                    if (Species.Length > 0) Name += " " + Species;
        //                    taxon._ScientificName = Name;
        //                    if (int.TryParse(R["NameParentID"].ToString(), out ID) && ID > 0)
        //                        taxon._ParentNameID = ID;
        //                    //taxon._IsAcceptedName = true;
        //                    //int.TryParse(R["IsAccepted"].ToString(), out ID) && ID == 1;
        //                    //if (int.TryParse(R["SynNameID"].ToString(), out ID))
        //                    //    taxon._AcceptedNameID = ID;
        //                    //R["BasionymAuthors"].ToString();
        //                    //R["BasionymAuthorsYear"].ToString();
        //                    //R["CombiningAuthors"].ToString();
        //                    //R["YearOfPubl"].ToString();
        //                    //R["NomenclaturalCode"].ToString();
        //                }
        //            }
        //        }
        //        return _TaxaInDatabase;
        //    }
        //}

        //public static Tasks.Taxa.TaxonStage TaxonInDatabase(int NameID)
        //{
        //    if (TaxaInDatabase.ContainsKey(NameID))
        //        return TaxaInDatabase[NameID];
        //    return null;
        //}

        #endregion

    }
}
