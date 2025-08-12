using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench
{
    public class TaxonNameAnalysis
    {
        #region Parameter

        private string _Taxon;
        private string[] _NamePartStrings;
        private System.Collections.Generic.List<string> _NamePartList;
        public enum TaxonNamePart
        {
            TaxonomicName, CreationType, TaxonomicRank, GenusOrSupragenericName, InfragenericEpithet,
            SpeciesEpithet, InfraspecificEpithet, BasionymAuthors, CombiningAuthors, PublishingAuthors, SanctioningAuthor, NonNomenclaturalNameSuffix, IsRecombination,
            IsHybrid, HybridName1, HybridName2, HybridName3, HybridName4, YearOfPubl,
            NomenclaturalCode, NomenclaturalStatus
        }
        private System.Collections.Generic.List<string> _NomenClaturalStati;
        private System.Collections.Generic.List<string> _AuthorNameParts;
        private string _TaxonmicRank = "sp.";
        private System.Collections.Generic.Dictionary<string, string> _Ranks;
        private System.Collections.Generic.List<string> _AuthorSeparatorList;
        private string _Message;

        private System.Collections.Generic.Dictionary<TaxonNamePart, string> _TaxonNamePartDictionary;
        
        #endregion

        #region Construction

        public TaxonNameAnalysis(string Taxon)
        {
            this._Taxon = Taxon;
        }
        
        #endregion

        #region Private functionality
        
        private string AnalyseName()
        {
            try
            {
                this._Taxon = this.RemoveSpaces(this._Taxon);
                foreach (string Status in this.NomenClaturalStati)
                {
                    if (Status.Length > 0 && this._Taxon.EndsWith("; " + Status))
                    {
                        this._TaxonNamePartDictionary.Add(TaxonNamePart.NomenclaturalStatus, Status);
                        this._Taxon = this._Taxon.Substring(0, this._Taxon.IndexOf("; " + Status));
                        break;
                    }
                }
                this._NamePartStrings = this._Taxon.Split(new char[] { ' ' });
                this.RearangeNameParts(ref this._NamePartStrings);
                this._TaxonNamePartDictionary.Add(TaxonNamePart.GenusOrSupragenericName, this.Genus);
                this._TaxonNamePartDictionary.Add(TaxonNamePart.SpeciesEpithet, this.SpeciesEpithet);
                if (this._NamePartStrings.Length == 2
                    && _NamePartStrings[1] == "sp.")
                    this._TaxonNamePartDictionary.Add(TaxonNamePart.TaxonomicRank, "gen.");
                else if (_NamePartStrings.Length == 1)
                    this._TaxonNamePartDictionary.Add(TaxonNamePart.TaxonomicRank, "gen.");
                else
                    this._TaxonNamePartDictionary.Add(TaxonNamePart.TaxonomicRank, this.TaxonomicRank);
                this._TaxonNamePartDictionary.Add(TaxonNamePart.InfraspecificEpithet, this.IntraspecificEpithet);
                this._TaxonNamePartDictionary.Add(TaxonNamePart.IsHybrid, this.IsHybrid.ToString());
                this._TaxonNamePartDictionary.Add(TaxonNamePart.CombiningAuthors, this.CombiningAuthos);
                this._TaxonNamePartDictionary.Add(TaxonNamePart.BasionymAuthors, this.BasionymAuthors);
                this._TaxonNamePartDictionary.Add(TaxonNamePart.NonNomenclaturalNameSuffix, this.NonNomenclaturalNameSuffix);
                this._TaxonNamePartDictionary.Add(TaxonNamePart.NomenclaturalStatus, this.NomenclaturalStatus);
            }
            catch (System.Exception ex) { this._Message = ex.Message; }
            return "";
        }

        #region Rearange the contents of the list

        private void RearangeNameParts(ref string[] Parts)
        {
            this._NamePartList = new List<string>();

            // filling the list
            for (int i = 0; i < Parts.Length; i++)
                this._NamePartList.Add(Parts[i]);

            int ii = _NamePartList.Count;
            while (ii != this.RearangeAuthors())
                ii = this.RearangeAuthors();
            _NamePartList.RemoveAll(Empty);
        }

        private int RearangeAuthors()
        {
            try
            {
                for (int i = 1; i < _NamePartList.Count; i++)
                {
                    if (_NamePartList.Count > i + 2
                        && _NamePartStrings[i].ToUpper()[0] == _NamePartStrings[i][0]
                        && _NamePartStrings[i + 1].ToUpper()[0] == _NamePartStrings[i + 1][0]
                        && !_NamePartStrings[i].EndsWith(")")) // next author
                    {
                        _NamePartStrings[i] += " " + _NamePartStrings[i + 1];
                        _NamePartStrings[i + 1] = "";
                        if (_NamePartList.Count > i + 3
                            && _NamePartStrings[i] == "f."
                            && _NamePartStrings[i + 1].ToUpper()[0] == _NamePartStrings[i + 1][0])// filius between 2 Authors
                        {
                            _NamePartStrings[i] += _NamePartStrings[i + 1] + " " + _NamePartStrings[i + 2];
                            _NamePartStrings[i + 1] = "";
                            _NamePartStrings[i + 2] = "";
                        }
                    }
                    if (_NamePartList.Count == i + 2
                        && _NamePartStrings[i + 1] == "f.") // filius at the end
                    {
                        _NamePartStrings[i] += " " + _NamePartStrings[i + 1];
                        _NamePartStrings[i + 1] = "";
                    }
                }
                if (TaxonomicRank != "f."
                    && _NamePartStrings.Contains("f.")
                    && TaxonomicRank != "f. sp.") // not forma and filius and not f. sp.
                {
                    _NamePartStrings[_NamePartList.IndexOf("f.") - 1] += " f.";
                    _NamePartStrings[_NamePartList.IndexOf("f.")] = "";
                }
                _NamePartList.RemoveAll(Empty);
            }
            catch { }
            return _NamePartList.Count;
        }

        #endregion

        #region Predicates
        // Search predicate returns true if the string is "f.".
        private static bool Filius(String s)
        {
            if (s == "f.")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool Hybrid(String s)
        {
            if (s == "x")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool Empty(String s)
        {
            if (s == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Parts of the name

        private string Genus
        {
            get
            {
                if (this._NamePartList.Count > 0
                    && _NamePartList[0].ToUpper()[0] == _NamePartList[0][0])
                    return _NamePartList[0];
                else return "";
            }
        }

        private string SpeciesEpithet
        {
            get
            {
                string Epithet = "";
                if (TaxonomicRank != "gen.")
                {
                    for (int i = 1; i < _NamePartList.Count; i++)
                    {
                        if ((_NamePartList[i].ToUpper()[0] == _NamePartList[i][0]
                            && _NamePartList[i][0].ToString() != "×")
                            || this.Ranks.ContainsValue(_NamePartList[i])
                            || this.Ranks.ContainsKey(_NamePartList[i])
                            || _NamePartList[i].StartsWith("(")
                            || (i > 1 && this.AuthorNameParts.Contains(_NamePartList[i])))
                            break;
                        if (_NamePartList[i].ToLower()[0] == _NamePartList[i][0])
                            Epithet += _NamePartList[i] + " ";
                    }
                    Epithet = Epithet.Trim();
                }
                return Epithet;
            }
        }

        private string TaxonomicRank
        {
            get
            {
                _TaxonmicRank = "sp.";
                if (_NamePartList.Count > 1
                    && _NamePartList[1].ToUpper()[0] == _NamePartList[1][0]
                    && _NamePartList[1][0].ToString() != "×") // is genus
                    _TaxonmicRank = "gen.";
                else
                {
                    foreach (string s in this._NamePartList)
                    {
                        if (Ranks.ContainsKey(s))
                        {
                            _TaxonmicRank = s;
                            if (_TaxonmicRank != "f.") break;
                        }
                        if (Ranks.ContainsValue(s))
                        {
                            _TaxonmicRank = Ranks[s];
                            if (_TaxonmicRank != "f.") break;
                        }
                    }
                    // Forma Specialis = "f. sp."
                    for (int i = 2; i < this._NamePartList.Count - 1; i++)
                    {
                        if ("f. sp." == this._NamePartList[i] + " " + this._NamePartList[i + 1])
                        {
                            _TaxonmicRank = "f. sp.";
                            break;
                        }
                    }

                    if (_TaxonmicRank == "f.")
                    {
                        // forma or no forma
                        bool isForma = false;
                        System.Collections.Generic.List<int> fList = new List<int>();
                        for (int i = 2; i < _NamePartList.Count; i++)
                        {
                            if (_NamePartList[i].ToString() == "f.")
                                fList.Add(i);
                        }
                        foreach (int i in fList)
                        {
                            if (_NamePartList.Count > i + 1
                                && _NamePartList[i + 1] != "&"
                                && _NamePartList[i + 1] != "ex"
                                && _NamePartList[i + 1].ToLower()[0] == _NamePartList[i + 1][0]
                                && !_NamePartList[i + 1].StartsWith("& ")
                                && !_NamePartList[i + 1].StartsWith("ex ")) // the next string after f. should not start with &, ex or upper case
                            {
                                isForma = true;
                                break;
                            }
                        }
                        if (!isForma)
                            _TaxonmicRank = "sp.";
                    }
                }
                return this.Ranks[this._TaxonmicRank];
            }
        }

        private string IntraspecificEpithet
        {
            get
            {
                string IntraSpec = "";
                if (TaxonomicRank != "sp."
                    && TaxonomicRank != "gen.")
                {
                    for (int i = 2; i < _NamePartList.Count; i++)
                    {
                        if (this.Ranks.ContainsValue(_NamePartList[i])
                            || this.Ranks.ContainsKey(_NamePartList[i]))
                        {
                            int ii = i + 1;
                            while (_NamePartList.Count > ii
                                && _NamePartList[ii].ToLower()[0] == _NamePartList[ii][0])
                            {
                                if (_NamePartList[ii].StartsWith("(")
                                    || _NamePartList[ii].ToUpper()[0] == _NamePartList[ii][0]
                                    || _NamePartList[ii].StartsWith("(")) // Start of the authors with ( or upper case
                                    break;
                                if (!this.Ranks.ContainsValue(_NamePartList[ii])) // no ranks
                                    IntraSpec += _NamePartList[ii] + " ";
                                ii++;
                            }
                            // for autonyms with the intraspecific epithet after the authors
                            if (IntraSpec.Length == 0)
                            {
                                while (_NamePartList.Count > ii)
                                {
                                    if (_NamePartList[ii].ToLower()[0] == _NamePartList[ii][0]
                                        && !this.Ranks.ContainsValue(_NamePartList[ii])
                                        && !_NamePartList[ii].StartsWith("&")
                                        && !_NamePartList[ii].StartsWith("ex")
                                        && !_NamePartList[ii].StartsWith("(")
                                        && !_NamePartList[ii].EndsWith(")")) // Start of something that is not an authors
                                    {
                                        int iii = ii;
                                        while (_NamePartList.Count > iii)
                                        {
                                            if (!this.Ranks.ContainsValue(_NamePartList[iii])
                                                && _NamePartList[iii].ToLower()[0] == _NamePartList[iii][0]) // no ranks and lower case
                                                IntraSpec += _NamePartList[iii] + " ";
                                            iii++;
                                        }
                                        break;
                                    }
                                    ii++;
                                }
                            }

                            break;
                        }
                    }
                    IntraSpec = IntraSpec.Trim();
                }
                return IntraSpec.Replace("  ", " ");
            }
        }

        private bool IsHybrid
        {
            get
            {
                if (_NamePartList.Contains("x")) return true;
                if (_NamePartList.Contains("×")) return true;
                else return false;
            }
        }

        private string CombiningAuthos
        {
            get
            {
                string CombAut = "";
                int i = 1;
                for (i = 1; i < _NamePartList.Count; i++)
                {
                    if (_NamePartList[i].EndsWith(")")
                        && _NamePartList.Count > i + 1)
                    {
                        int ii = i + 1;
                        CombAut = this.Author(ii);
                        CombAut = CombAut.Trim();
                        break;
                    }
                }
                return CombAut.Replace("  ", " ");
            }
        }

        private string BasionymAuthors
        {
            get
            {
                string BasAut = "";
                if (this.CombiningAuthos.Length > 0)
                {
                    int i = 1;
                    for (i = 1; i < _NamePartList.Count; i++)
                    {
                        if (_NamePartList[i].StartsWith("(")
                            && _NamePartList.Count > i + 1)
                        {
                            int ii = i;
                            while (ii < _NamePartList.Count)
                            {
                                BasAut += _NamePartList[ii] + " ";
                                if (_NamePartList[ii].EndsWith(")"))
                                    break;
                                ii++;
                            }
                            BasAut = BasAut.Replace("(", "").Replace(")", "").Trim();
                            break;
                        }
                    }
                }
                else
                {
                    int i = 1;
                    for (i = 1; i < _NamePartList.Count; i++)
                    {
                        if ((_NamePartList[i].ToUpper()[0] == _NamePartList[i][0]
                            || this.AuthorNameParts.Contains(_NamePartList[i]))
                            && _NamePartList[i].ToString() != "×")
                        {
                            int ii = i;
                            BasAut = this.Author(ii);
                            break;
                        }
                    }
                    BasAut = BasAut.Trim();
                }
                return BasAut.Replace("  ", " ");
            }
        }

        private string Author(int Index)
        {
            string Author = "";
            int ii = Index;
            while (ii < _NamePartList.Count
               && _NamePartList[ii].Length > 0)
            {
                if (_NamePartList[ii].ToUpper()[0] != _NamePartList[ii][0]
                    && _NamePartList.Count > ii + 2
                    && _NamePartList[ii + 1].ToUpper()[0] != _NamePartList[ii + 1][0]
                    && (_NamePartList[ii].ToString() == "f."
                    && SpeciesEpithet != IntraspecificEpithet))
                    break;
                if (_NamePartList[ii - 1].ToString() == "f."
                    && _NamePartList[ii].ToString() == "f.")
                    break;
                if ((_NamePartList[ii].ToUpper()[0] != _NamePartList[ii][0]
                    && !this.AuthorNameParts.Contains(_NamePartList[ii]))
                    && _NamePartList[ii].ToString() != "f."
                    && _NamePartList[ii].ToString() != "ex")
                    break;
                if (SpeciesEpithet == IntraspecificEpithet
                    && TaxonomicRank == "f."
                    && _NamePartList[ii].ToString() == "f."
                    && _NamePartList.Count > ii + 1
                    && _NamePartList[ii + 1].ToLower()[0] == _NamePartList[ii + 1][0]
                    && _NamePartList[ii + 1] != "f.")
                    break;
                Author += _NamePartList[ii] + " ";
                ii++;
            }
            return Author;
        }

        public System.Collections.Generic.List<string> AuthorNameParts
        {
            get
            {
                if (this._AuthorNameParts == null)
                {
                    this._AuthorNameParts = new List<string>();
                    this._AuthorNameParts.Add("de");
                    this._AuthorNameParts.Add("la");
                    this._AuthorNameParts.Add("van");
                    this._AuthorNameParts.Add("der");
                    this._AuthorNameParts.Add("von");
                    this._AuthorNameParts.Add("den");
                    this._AuthorNameParts.Add("ex");
                }
                return _AuthorNameParts;
            }
        }

        private string NonNomenclaturalNameSuffix
        {
            get
            {
                System.Collections.Generic.List<string> SuffixStartList = new List<string>();
                SuffixStartList.Add("em.");
                SuffixStartList.Add("emend.");

                string Suffix = "";
                for (int i = 3; i < _NamePartList.Count; i++)
                {
                    if (SuffixStartList.Contains(_NamePartList[i]))
                    {
                        for (int ii = i; ii < _NamePartList.Count; ii++)
                        {
                            if (ii > i) Suffix += " ";
                            Suffix += _NamePartList[ii];
                        }
                        break;
                    }
                }
                return Suffix.Replace("  ", " ");

            }
        }

        //private int? GenusNameID(string Genus)
        //{
        //    int NameID = 0;
        //    string SQL = "SELECT MIN(T.NameID) AS SpeciesGenusNameID " +
        //        "FROM TaxonName AS T INNER JOIN " +
        //        "TaxonNameProject AS P ON T.NameID = P.NameID " +
        //        "WHERE (T.TaxonomicRank = N'gen.') AND (P.ProjectID = " + this.ProjectID.ToString() + ") AND (T.GenusOrSupragenericName = N'" + Genus + "')";
        //    if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out NameID))
        //        return NameID;
        //    else
        //        return null;
        //}

        private string NomenclaturalStatus
        {
            get
            {
                string Status = "";
                foreach (string s in this.NomenClaturalStati)
                {
                    string[] NS = s.Split(new char[] { ' ' });
                    if (NS.Length == 1 && _NamePartList[_NamePartList.Count - 1] == NS[0].ToString())
                    {
                        Status = s;
                        break;
                    }
                    else if (NS.Length == 2)
                    {
                        if (NS[0] == _NamePartList[_NamePartList.Count - 2] &&
                            NS[1] == _NamePartList[_NamePartList.Count - 1])
                        {
                            Status = s;
                            break;
                        }
                    }
                }
                return Status;
            }
        }

        //public System.Data.DataTable DtNomenclaturalStatus
        //{
        //    get
        //    {
        //        if (this._dtNomenclaturalStatus == null)
        //        {
        //            this._dtNomenclaturalStatus = new DataTable();
        //            string SQL = "SELECT Code FROM TaxonNameNomenclaturalStatus_Enum";
        //            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //            ad.Fill(this._dtNomenclaturalStatus);
        //        }
        //        return _dtNomenclaturalStatus;
        //    }
        //}

        private System.Collections.Generic.Dictionary<string, string> Ranks
        {
            get
            {
                if (this._Ranks == null)
                {
                    this._Ranks = new Dictionary<string, string>();
                    this._Ranks.Add("cv.", "cult.");
                    this._Ranks.Add("var.", "var.");
                    this._Ranks.Add("f.", "f.");
                    this._Ranks.Add("ssp.", "ssp.");
                    this._Ranks.Add("subsp.", "ssp.");
                    this._Ranks.Add("subvar.", "subvar.");
                    this._Ranks.Add("trib.", "trib.");
                    this._Ranks.Add("sp.", "sp.");
                    this._Ranks.Add("gen.", "gen.");
                    this._Ranks.Add("f. sp.", "f. sp.");//
                }
                return this._Ranks;
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

        #endregion

        
        #endregion

        #region Public properties

        public string Message
        {
            get 
            {
                if (this._Message == null)
                    return "";
                return _Message; 
            }
        }

        public System.Collections.Generic.Dictionary<TaxonNamePart, string> TaxonNamePartDictionary
        {
            get 
            {
                if (this._TaxonNamePartDictionary == null)
                {
                    this._TaxonNamePartDictionary = new Dictionary<TaxonNamePart, string>();
                    this.AnalyseName();
                    System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<TaxonNamePart, string>> ItemsToRemove = new List<KeyValuePair<TaxonNamePart, string>>();
                    foreach (System.Collections.Generic.KeyValuePair<TaxonNamePart, string> KV in this._TaxonNamePartDictionary)
                        if (KV.Value.Length == 0) ItemsToRemove.Add(KV);
                    foreach (System.Collections.Generic.KeyValuePair<TaxonNamePart, string> KV in ItemsToRemove)
                        this._TaxonNamePartDictionary.Remove(KV.Key);
                }
                return _TaxonNamePartDictionary; 
            }
        }
        
        public System.Collections.Generic.List<string> NomenClaturalStati
        {
            get 
            {
                if (this._NomenClaturalStati == null)
                {
                    this._NomenClaturalStati = new List<string>();
                    this._NomenClaturalStati.Add("ad interim");
                    this._NomenClaturalStati.Add("comb.");
                    this._NomenClaturalStati.Add("comb. ined.");
                    this._NomenClaturalStati.Add("comb. invalid.");
                    this._NomenClaturalStati.Add("comb. superfl.");
                    this._NomenClaturalStati.Add("ined.");
                    this._NomenClaturalStati.Add("nom. confus.");
                    this._NomenClaturalStati.Add("nom. cons.");
                    this._NomenClaturalStati.Add("nom. excl.");
                    this._NomenClaturalStati.Add("nom. herb.");
                    this._NomenClaturalStati.Add("nom. illeg.");
                    this._NomenClaturalStati.Add("nom. invalid.");
                    this._NomenClaturalStati.Add("nom. nud.");
                    this._NomenClaturalStati.Add("nom. provis.");
                    this._NomenClaturalStati.Add("nom. rej.");
                    this._NomenClaturalStati.Add("nom. superfl.");
                    this._NomenClaturalStati.Add("pro syn.");
                }
                return _NomenClaturalStati; 
            }
            set { _NomenClaturalStati = value; }
        }
        
        #endregion
    }
}
