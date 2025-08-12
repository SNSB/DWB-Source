using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.Tasks.Taxa
{
    public class Taxon
    {
        #region Parameter

        private int _NameID;
        //private string _BaseURL;
        private string _ScientificName;
        private string _VernacularName;
        private string _Json = "";
        private string _TaxonName;
        //private bool _IsAcceptedName = false;
        //private int? _AcceptedNameID = null;
        private int? _ParentNameID = null;
        //private string _Group;
        private string _Rank;
        private int _RankOrder;
        private IPM.TaxonSource _taxonSource;
        private System.Collections.Generic.Dictionary<int, Resource> _Images;
        private System.Collections.Generic.Dictionary<int, Resource> _Infos;

        #endregion

        #region Interface


        public int NameID { get => _NameID; }
        //public string BaseURL { get => _BaseURL; }
        public string ScientificName { get => _ScientificName; }
        public string VernacularName { get => _VernacularName; }
        //public bool AcceptedName { get => _AcceptedNameID == _NameID; }
        //public string Group { get => _Group; }

        public System.Collections.Generic.Dictionary<int, Resource> Images
        {
            get
            {
                if (_Images == null) { _Images = new Dictionary<int, Resource>(); }
                return _Images;
            }
        }
        public System.Collections.Generic.Dictionary<int, Resource> Infos
        {
            get
            {
                if (_Infos == null)
                {
                    _Infos = new Dictionary<int, Resource>();
                }
                return _Infos;
            }
        }

        public string DisplayText
        {
            get
            {
                string Name = "";
                if (VernacularName != null)
                    Name = VernacularName;
                if (Name.Length == 0 && ScientificName != null && ScientificName.Length > 0) // && Name.IndexOf("(") == -1)
                    Name = ScientificName;
                return Name;
            }
        }

        public string NameURI()
        {
            return Database.TaxonDBBaseURL + NameID.ToString();
        }

        #endregion

        #region Construction

        //public Taxon(string Identifier, IPM.TaxonSource taxonSource)
        //{
        //    if (List.TaxonStageDict(taxonSource) != null &&
        //        List.TaxonStageDict(taxonSource).Count > 0 &&
        //        List.TaxonStageDict(taxonSource).ContainsKey(Identifier))
        //    {
        //        string nameID = Identifier.Substring(Database.TaxonDBBaseURL.Length);
        //        if (nameID.IndexOf("|") > -1)
        //        {
        //            nameID = nameID.Substring(0, nameID.IndexOf("|"));
        //            int NameID;
        //            if (int.TryParse(nameID, out NameID))
        //            {

        //            }
        //        }
        //    }
        //}


        public Taxon(int ID, IPM.TaxonSource taxonSource)
        {
            _NameID = ID;
            _taxonSource = taxonSource;
            if (List.TaxonDict(taxonSource) != null && 
                List.TaxonDict(taxonSource).Count > 0 &&
                List.TaxonDict(taxonSource).ContainsKey(ID))
            {
                this.initTaxon();
            }
            ////_taxonSource = taxonSource;
            //if (Database.DtTaxa.Select("NameID = " + ID.ToString()).Length > 0)
            //{
            //    System.Data.DataRow R = Database.DtTaxa.Select("NameID = " + ID.ToString())[0];
            //    //_ScientificName = R["TaxonNameCache"].ToString();
            //    _Rank = R["TaxonomicRank"].ToString();
            //    int RankOrder;
            //    if (int.TryParse(R["RankOrder"].ToString(), out RankOrder))
            //        _RankOrder = RankOrder;
            //    _TaxonName = R["TaxonName"].ToString();
            //    _ScientificName = R["GenusOrSupragenericName"].ToString();
            //    if (!R["SpeciesEpithet"].Equals(System.DBNull.Value) && R["SpeciesEpithet"].ToString().Length > 0)
            //        _ScientificName += " " + R["SpeciesEpithet"].ToString();
            //    int Parent;
            //    if (int.TryParse(R["NameParentID"].ToString(), out Parent))
            //        _ParentNameID = Parent;
            //    this.initTaxon();
            //}
            //else { }
        }

        //public Taxon(int ID)
        //{
        //    _NameID = ID;
        //    if (Database.DtTaxa.Select("NameID = " + ID.ToString()).Length > 0)
        //    {
        //        System.Data.DataRow R = Database.DtTaxa.Select("NameID = " + ID.ToString())[0];
        //        _Rank = R["TaxonomicRank"].ToString();
        //        _ScientificName = R["GenusOrSupragenericName"].ToString();
        //        if (!R["SpeciesEpithet"].Equals(System.DBNull.Value) && R["SpeciesEpithet"].ToString().Length > 0)
        //            _ScientificName += " " + R["SpeciesEpithet"].ToString();
        //        int Parent;
        //        if (int.TryParse(R["NameParentID"].ToString(), out Parent))
        //            _ParentNameID = Parent;
        //        this.initTaxon();
        //    }
        //    else { }
        //}

        //public Taxon(int ID, string Json)
        //{
        //    _NameID = ID;
        //    _Json = Json;
        //}

        #endregion

        #region init

        private void initTaxon()
        {
            try
            {
                string CommonName = Database.CommonName(_NameID);
                if (CommonName.Length == 0)
                    CommonName = this._ScientificName;
                this._VernacularName = CommonName;

                //this._Group = Groups.getGroup(_taxonSource, _NameID);

                this.initInfos();
                this.initImages();
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }




        private void initImages()
        {
            try
            {
                this.Images.Clear();
                this._Images = TaxonResource.TaxonResources(_taxonSource, ResourceType.Image, NameID);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        private void initInfos()
        {
            try
            {
                this.Infos.Clear();
                this._Infos = TaxonResource.TaxonResources(_taxonSource, ResourceType.Info, NameID);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        #endregion

        #region TaxonInfos

        //private static System.Collections.Generic.Dictionary<int, Tasks.Taxa.TaxonStage> _TaxaInDatabase;

        //private static System.Collections.Generic.Dictionary<int, Tasks.Taxa.TaxonStage> TaxaInDatabase
        //{
        //    get
        //    {
        //        if (_TaxaInDatabase == null)
        //        {
        //            _TaxaInDatabase = new Dictionary<int, TaxonStage>();
        //            foreach (System.Data.DataRow R in Database.DtTaxa.Rows)
        //            {
        //                int ID = 0;
        //                if (int.TryParse(R["NameID"].ToString(), out ID))
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
