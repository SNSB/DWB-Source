using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection
{
    public class TaxonomicName
    {
        #region Parameter

        private string _Name = "";
        private string _URI = "";
        private string _TaxonomicGroup = "";
        private bool _RankFound;
        private System.Collections.Generic.List<DiversityCollection.TaxonomicName> _HybridParts;
        public enum NamePart { Family, Genus, InfragenericEpithet, SpeciesEpithet, TaxonomicRank, InfraspecificEpithet, Authors, AuthorType, IdentificationQualifier, IdentificationQualifierType, NomenclaturalStatus, Undefined };
        private enum _IdentificationQualifierType { QualifierLeading, QualifierGenus, QualifierSpecies, QualifierInfraspecific, QualifierTerminatory };
        private enum _AuthorType { AuthorsGenus, AuthorsInfrageneric, AuthorsSpecies, AuthorsInfraspecific };
        private System.Collections.Generic.Dictionary<NamePart, string> _NameParts;
        private System.Collections.Generic.Dictionary<DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart, string> _TaxonNameParts;
        private System.Collections.Generic.List<string> _IdenfificationQualifierList;
        private System.Collections.Generic.List<string> _RankList;
        private System.Collections.Generic.List<string> _AuthorSeparatorList;
        private string _Message;
        
        #endregion

        #region Construction

        /// <summary>
        /// Analysing the parts of a taxonimic name
        /// </summary>
        /// <param name="Name">the taxonomic name that should be analysed</param>
        /// <param name="TaxonomicGroup">The taxonomic group, e.g. plant as defined in the table CollTaxonomicGroup_Enum</param>
        /// <param name="URI">The URI of the name if linked to a resource</param>
        public TaxonomicName(string Name, string TaxonomicGroup, string URI)
        {
            this._Name = Name;
            this._TaxonomicGroup = TaxonomicGroup;
            this._URI = URI;
            //this.AnalyseName();
            this.TaxonNameAnalysis();
        }
        
        #endregion

        public string ErrorMessage { get { if (this._Message == null) this._Message = ""; return this._Message; } }

        private void TaxonNameAnalysis()
        {
            string Taxon = this._Name.Trim();
            while (Taxon.IndexOf("  ") > -1)
                Taxon = Taxon.Replace("  ", " ");
            DiversityWorkbench.TaxonNameAnalysis T = new DiversityWorkbench.TaxonNameAnalysis(Taxon);
            this._TaxonNameParts = T.TaxonNamePartDictionary;
            //string SpeciesEpithet = T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.SpeciesEpithet];
            //string InfraspecificEpithet = T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.InfraspecificEpithet];
            // Analysing the parts
            this._NameParts = new Dictionary<NamePart, string>();
            if (T.TaxonNamePartDictionary.ContainsKey(DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.GenusOrSupragenericName))
                this._NameParts.Add(NamePart.Genus, T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.GenusOrSupragenericName]);
            if (T.TaxonNamePartDictionary.ContainsKey(DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.SpeciesEpithet))
                this._NameParts.Add(NamePart.SpeciesEpithet, T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.SpeciesEpithet]);
            if (T.TaxonNamePartDictionary.ContainsKey(DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.InfraspecificEpithet))
                this._NameParts.Add(NamePart.InfraspecificEpithet, T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.InfraspecificEpithet]);
            if (T.TaxonNamePartDictionary.ContainsKey(DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.InfragenericEpithet))
                this._NameParts.Add(NamePart.InfragenericEpithet, T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.InfragenericEpithet]);
            if (T.TaxonNamePartDictionary.ContainsKey(DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.BasionymAuthors))
            {
                string Aut = T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.BasionymAuthors];
                if (T.TaxonNamePartDictionary.ContainsKey(DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.CombiningAuthors))
                    Aut = "(" + Aut + ") " + T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.CombiningAuthors];
                this._NameParts.Add(NamePart.Authors,Aut);
            }
            if (T.TaxonNamePartDictionary.ContainsKey(DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.NomenclaturalStatus))
                this._NameParts.Add(NamePart.NomenclaturalStatus, T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.NomenclaturalStatus]);
            if (T.TaxonNamePartDictionary.ContainsKey(DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.TaxonomicRank))
            {
                this._NameParts.Add(NamePart.TaxonomicRank, T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.TaxonomicRank]);
                this._RankFound = true;
            }


            //if (Taxon.IndexOf(" x ") > -1
            //   ||
            //   Taxon.IndexOf(" X ") > -1
            //   ||
            //   Taxon.IndexOf(" × ") > -1)
            //{
            //    this._HybridParts = new List<TaxonomicName>();
            //    string[] stringSeparators = new string[] { " x ", " X ", " × " };
            //    string[] HybridParts = Taxon.Split(stringSeparators, StringSplitOptions.None);
            //    bool HybridPartsAreValid = true;
            //    for (int i = 1; i < HybridParts.Length; i++)
            //    {
            //        DiversityCollection.TaxonomicName TaxonomicName = new TaxonomicName(HybridParts[i], this._TaxonomicGroup, this._URI);
            //        if (TaxonomicName.ErrorMessage.Length == 0)
            //            this._HybridParts.Add(TaxonomicName);
            //        else HybridPartsAreValid = false;
            //    }
            //    if (HybridPartsAreValid)
            //        Taxon = HybridParts[0];
            //}

            if (T.TaxonNamePartDictionary.ContainsKey(DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.NomenclaturalStatus) &&
                Taxon.IndexOf(';') > -1)
            {
                if (!_NameParts.ContainsKey(NamePart.NomenclaturalStatus))
                    this._NameParts.Add(NamePart.NomenclaturalStatus, T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.NomenclaturalStatus]);
                Taxon = Taxon.Substring(0, Taxon.IndexOf(';'));
            }
            string[] TaxonParts = Taxon.Split(new char[] { ' ' });

            //this._RankFound = false;

            int PositionOfAnalysis = 0;
            //string Genus = "";
            string SpeciesEpithet = "";
            string InfraspecificEpithet = "";

            int Position = 0;

            // Analysing the parts
            try
            {
                if (TaxonParts.Length == 1) // only one part
                {
                    //if (TaxonParts[PositionOfAnalysis].Length > 0 && !this._RankFound)
                    //{
                    //    if (!_NameParts.ContainsKey(NamePart.Genus))
                    //        _NameParts.Add(NamePart.Genus, TaxonParts[PositionOfAnalysis]);
                    //    if (!_NameParts.ContainsKey(NamePart.TaxonomicRank))
                    //        _NameParts.Add(NamePart.TaxonomicRank, "gen.");
                    //    this._RankFound = true;
                    //}
                }
                else // more than one part
                {
                    if (TaxonParts[0] == "x" || TaxonParts[0] == "X" || TaxonParts[0] == "×")
                    {
                        _NameParts.Add(NamePart.IdentificationQualifierType, TaxonParts[0]);
                        PositionOfAnalysis++;
                    }
                    this.AnalyseTaxonomicNameQualifier(TaxonParts, ref PositionOfAnalysis, _IdentificationQualifierType.QualifierLeading);
                    // Genus
                    if (TaxonParts[PositionOfAnalysis].Length > 0
                        &&
                        ((Position == 0 && TaxonParts[PositionOfAnalysis].Substring(0, 1).ToUpper() == TaxonParts[PositionOfAnalysis].Substring(0, 1))
                        ||
                        TaxonParts[PositionOfAnalysis].Substring(0, 1).ToUpper() == TaxonParts[PositionOfAnalysis].Substring(0, 1)))
                    {
                        if (!_NameParts.ContainsKey(NamePart.Genus))
                            this._NameParts.Add(NamePart.Genus, TaxonParts[PositionOfAnalysis]);
                    }
                    else if (TaxonParts[PositionOfAnalysis].Length > 0
                        && this._NameParts.ContainsKey(NamePart.Genus)
                        &&
                        (Position > 0
                        ||
                        TaxonParts[PositionOfAnalysis].Substring(0, 1).ToLower() == TaxonParts[PositionOfAnalysis].Substring(0, 1)))
                        if (!_NameParts.ContainsKey(NamePart.SpeciesEpithet))
                            this._NameParts.Add(NamePart.SpeciesEpithet, TaxonParts[PositionOfAnalysis]);
                    else
                        this._Message += "Genus is missing\r\n";
                    PositionOfAnalysis++;
                    if (TaxonParts.Length > PositionOfAnalysis)
                    {
                        if (Position == 0
                            ||
                            TaxonParts[0].Substring(0, 1).ToUpper() == TaxonParts[0].Substring(0, 1))
                            this.AnalyseTaxonomicNameAuthors(TaxonParts, ref PositionOfAnalysis, _AuthorType.AuthorsGenus);
                        else if (Position > 0
                            &&
                            TaxonParts[0].Substring(0, 1).ToLower() == TaxonParts[0].Substring(0, 1))
                            this.AnalyseTaxonomicNameAuthors(TaxonParts, ref PositionOfAnalysis, _AuthorType.AuthorsSpecies);
                        if (TaxonParts.Length == PositionOfAnalysis
                            &&
                            Position == 0
                            &&
                            !this._RankFound)
                        {
                            if (!_NameParts.ContainsKey(NamePart.TaxonomicRank))
                                this._NameParts.Add(NamePart.TaxonomicRank, "gen.");
                            this._RankFound = true;
                        }
                        else if (TaxonParts.Length == PositionOfAnalysis
                            &&
                            Position > 0
                            &&
                            !this._RankFound)
                        {
                            if (!_NameParts.ContainsKey(NamePart.TaxonomicRank))
                                this._NameParts.Add(NamePart.TaxonomicRank, "sp.");
                            this._RankFound = true;
                        }
                        else if (TaxonParts.Length > PositionOfAnalysis)
                        {
                            // Qualifier after genus
                            this.AnalyseTaxonomicNameQualifier(TaxonParts, ref PositionOfAnalysis, _IdentificationQualifierType.QualifierSpecies);
                            this.AnalyseTaxonomicNameRank(TaxonParts, ref PositionOfAnalysis);
                            // Infrageneric Epithet
                            if (TaxonParts.Length > PositionOfAnalysis)
                            {
                                if (TaxonParts[PositionOfAnalysis].Substring(0, 1).ToUpper() == TaxonParts[PositionOfAnalysis].Substring(0, 1)
                                    &&
                                    this._RankFound
                                    &&
                                    TaxonParts[PositionOfAnalysis - 1] == "subgen.")
                                {
                                    if (!_NameParts.ContainsKey(NamePart.InfragenericEpithet))
                                        _NameParts.Add(NamePart.InfragenericEpithet, TaxonParts[PositionOfAnalysis]);
                                    PositionOfAnalysis++;
                                    this.AnalyseTaxonomicNameAuthors(TaxonParts, ref PositionOfAnalysis, _AuthorType.AuthorsInfrageneric);
                                }
                                if (TaxonParts.Length > PositionOfAnalysis)
                                {
                                    // Epithet
                                    SpeciesEpithet = TaxonParts[PositionOfAnalysis];
                                    if (SpeciesEpithet.Length > 0 && !_NameParts.ContainsKey(NamePart.SpeciesEpithet))
                                        this._NameParts.Add(NamePart.SpeciesEpithet, SpeciesEpithet);
                                    PositionOfAnalysis++;
                                    if (TaxonParts.Length > PositionOfAnalysis)
                                    {
                                        this.AnalyseTaxonomicNameAuthors(TaxonParts, ref PositionOfAnalysis, _AuthorType.AuthorsSpecies);
                                        this.AnalyseTaxonomicNameRank(TaxonParts, ref PositionOfAnalysis);
                                        this.AnalyseTaxonomicNameQualifier(TaxonParts, ref PositionOfAnalysis, _IdentificationQualifierType.QualifierInfraspecific);
                                        this.AnalyseTaxonomicNameAuthors(TaxonParts, ref PositionOfAnalysis, _AuthorType.AuthorsSpecies);
                                        if (TaxonParts.Length > PositionOfAnalysis)
                                        {
                                            this.AnalyseTaxonomicNameRank(TaxonParts, ref PositionOfAnalysis);
                                            if (TaxonParts.Length > PositionOfAnalysis)
                                            {
                                                if (T.TaxonNamePartDictionary.ContainsKey(DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.TaxonomicRank) &&
                                                    T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.TaxonomicRank] != "sp.")
                                                {
                                                }
                                                // InfraspecificEpithet
                                                InfraspecificEpithet = TaxonParts[PositionOfAnalysis];
                                                if (InfraspecificEpithet.Length > 0 && 
                                                    !_NameParts.ContainsKey(NamePart.InfraspecificEpithet) &&
                                                    T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.TaxonomicRank] != "sp.")
                                                    this._NameParts.Add(NamePart.InfraspecificEpithet, InfraspecificEpithet);
                                                PositionOfAnalysis++;
                                                if (TaxonParts.Length > PositionOfAnalysis)
                                                {
                                                    // After InfraspecificEpithet
                                                    if (T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.TaxonomicRank] != "sp.")
                                                    {
                                                        this.AnalyseTaxonomicNameAuthors(TaxonParts, ref PositionOfAnalysis, _AuthorType.AuthorsInfraspecific);
                                                    }
                                                    if (TaxonParts.Length > PositionOfAnalysis &&
                                                        T.TaxonNamePartDictionary[DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart.IsHybrid].ToLower() == "false")
                                                    {
                                                        // Rest
                                                        string Undefined = "";
                                                        bool UndefIsContained = true;
                                                        while (TaxonParts.Length > PositionOfAnalysis)
                                                        {
                                                            if (TaxonParts[PositionOfAnalysis] != "x" &&
                                                                TaxonParts[PositionOfAnalysis] != "X" &&
                                                                TaxonParts[PositionOfAnalysis] != "×")
                                                            Undefined += TaxonParts[PositionOfAnalysis] + " ";
                                                            if (!this._NameParts.ContainsValue(TaxonParts[PositionOfAnalysis]))
                                                                UndefIsContained = false;
                                                            PositionOfAnalysis++;
                                                        }
                                                        Undefined = Undefined.Trim();
                                                        if (this._NameParts.ContainsValue(Undefined))
                                                            UndefIsContained = true;
                                                        if (!UndefIsContained)
                                                            this._NameParts.Add(NamePart.Undefined, Undefined);
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
                        if (!_NameParts.ContainsKey(NamePart.TaxonomicRank))
                            this._NameParts.Add(NamePart.TaxonomicRank, "sp.");
                        this._RankFound = true;
                    }
                }
            }
            catch (System.Exception ex) { this._Message += ex.Message + "\r\n"; }
        }

        private void AnalyseName()
        {
            string Taxon = this._Name; //.Replace("  ", " ");
            while (Taxon.IndexOf("  ") > -1)
                Taxon = Taxon.Replace("  ", " ");
            DiversityWorkbench.TaxonNameAnalysis T = new DiversityWorkbench.TaxonNameAnalysis(Taxon);
            System.Collections.Generic.Dictionary<DiversityWorkbench.TaxonNameAnalysis.TaxonNamePart, string> TaxonNameAnalysisDictionary = T.TaxonNamePartDictionary;
            this._NameParts = new Dictionary<NamePart, string>();

            //if (Taxon.IndexOf(" x ") > -1
            //   ||
            //   Taxon.IndexOf(" X ") > -1
            //   ||
            //   Taxon.IndexOf(" × ") > -1)
            //{
            //    this._HybridParts = new List<TaxonomicName>();
            //    string[] stringSeparators = new string[] { " x ", " X ", " × " };
            //    string[] HybridParts = Taxon.Split(stringSeparators, StringSplitOptions.None);
            //    bool HybridPartsAreValid = true;
            //    for (int i = 1; i < HybridParts.Length; i++)
            //    {
            //        DiversityCollection.TaxonomicName TaxonomicName = new TaxonomicName(HybridParts[i], this._TaxonomicGroup, this._URI);
            //        if (TaxonomicName.ErrorMessage.Length == 0)
            //            this._HybridParts.Add(TaxonomicName);
            //        else HybridPartsAreValid = false;
            //    }
            //    if (HybridPartsAreValid)
            //        Taxon = HybridParts[0];
            //}
            if (Taxon.IndexOf(';') > -1)
            {
                if (!this._NameParts.ContainsKey(NamePart.NomenclaturalStatus))
                    this._NameParts.Add(NamePart.NomenclaturalStatus, Taxon.Substring(Taxon.IndexOf(';') + 1));
                Taxon = Taxon.Substring(0, Taxon.IndexOf(';'));
            }
            string[] TaxonParts = Taxon.Split(new char[] { ' ' });

            this._RankFound = false;

            int PositionOfAnalysis = 0;
            //string Genus = "";
            string SpeciesEpithet = "";
            string InfraspecificEpithet = "";

            int Position = 0;

            // Analysing the parts
            try
            {
                if (TaxonParts.Length == 1) // only one part
                {
                    if (TaxonParts[PositionOfAnalysis].Length > 0 && !this._RankFound)
                    {
                        _NameParts.Add(NamePart.Genus, TaxonParts[PositionOfAnalysis]);
                        _NameParts.Add(NamePart.TaxonomicRank, "gen.");
                        this._RankFound = true;
                    }
                }
                else // more than one part
                {
                    if (TaxonParts[0] == "x" || TaxonParts[0] == "X" || TaxonParts[0] == "×")
                    {
                        _NameParts.Add(NamePart.IdentificationQualifierType, TaxonParts[0]);
                        PositionOfAnalysis++;
                    }
                    this.AnalyseTaxonomicNameQualifier(TaxonParts, ref PositionOfAnalysis, _IdentificationQualifierType.QualifierLeading);
                    // Genus
                    if (TaxonParts[PositionOfAnalysis].Length > 0
                        &&
                        ((Position == 0 && TaxonParts[PositionOfAnalysis].Substring(0, 1).ToUpper() == TaxonParts[PositionOfAnalysis].Substring(0, 1))
                        ||
                        TaxonParts[PositionOfAnalysis].Substring(0, 1).ToUpper() == TaxonParts[PositionOfAnalysis].Substring(0, 1)))
                        this._NameParts.Add(NamePart.Genus, TaxonParts[PositionOfAnalysis]);
                    else if (TaxonParts[PositionOfAnalysis].Length > 0
                        && this._NameParts.ContainsKey(NamePart.Genus)
                        &&
                        (Position > 0
                        ||
                        TaxonParts[PositionOfAnalysis].Substring(0, 1).ToLower() == TaxonParts[PositionOfAnalysis].Substring(0, 1)))
                        this._NameParts.Add(NamePart.SpeciesEpithet, TaxonParts[PositionOfAnalysis]);
                    else
                        this._Message += "Genus is missing\r\n";
                    PositionOfAnalysis++;
                    if (TaxonParts.Length > PositionOfAnalysis)
                    {
                        if (Position == 0
                            ||
                            TaxonParts[0].Substring(0, 1).ToUpper() == TaxonParts[0].Substring(0, 1))
                            this.AnalyseTaxonomicNameAuthors(TaxonParts, ref PositionOfAnalysis, _AuthorType.AuthorsGenus);
                        else if (Position > 0
                            &&
                            TaxonParts[0].Substring(0, 1).ToLower() == TaxonParts[0].Substring(0, 1))
                            this.AnalyseTaxonomicNameAuthors(TaxonParts, ref PositionOfAnalysis, _AuthorType.AuthorsSpecies);
                        if (TaxonParts.Length == PositionOfAnalysis
                            &&
                            Position == 0
                            &&
                            !this._RankFound)
                        {
                            this._NameParts.Add(NamePart.TaxonomicRank, "gen.");
                            this._RankFound = true;
                        }
                        else if (TaxonParts.Length == PositionOfAnalysis
                            &&
                            Position > 0
                            &&
                            !this._RankFound)
                        {
                            this._NameParts.Add(NamePart.TaxonomicRank, "sp.");
                            this._RankFound = true;
                        }
                        else if (TaxonParts.Length > PositionOfAnalysis)
                        {
                            // Qualifier after genus
                            this.AnalyseTaxonomicNameQualifier(TaxonParts, ref PositionOfAnalysis, _IdentificationQualifierType.QualifierSpecies);
                            this.AnalyseTaxonomicNameRank(TaxonParts, ref PositionOfAnalysis);
                            // Infrageneric Epithet
                            if (TaxonParts.Length > PositionOfAnalysis)
                            {
                                if (TaxonParts[PositionOfAnalysis].Substring(0, 1).ToUpper() == TaxonParts[PositionOfAnalysis].Substring(0, 1)
                                    &&
                                    this._RankFound
                                    &&
                                    TaxonParts[PositionOfAnalysis - 1] == "subgen.")
                                {
                                    _NameParts.Add(NamePart.InfragenericEpithet, TaxonParts[PositionOfAnalysis]);
                                    PositionOfAnalysis++;
                                    this.AnalyseTaxonomicNameAuthors(TaxonParts, ref PositionOfAnalysis, _AuthorType.AuthorsInfrageneric);
                                }
                                if (TaxonParts.Length > PositionOfAnalysis)
                                {
                                    // Epithet
                                    SpeciesEpithet = TaxonParts[PositionOfAnalysis];
                                    if (SpeciesEpithet.Length > 0)
                                        this._NameParts.Add(NamePart.SpeciesEpithet, SpeciesEpithet);
                                    PositionOfAnalysis++;
                                    if (TaxonParts.Length > PositionOfAnalysis)
                                    {
                                        this.AnalyseTaxonomicNameAuthors(TaxonParts, ref PositionOfAnalysis, _AuthorType.AuthorsSpecies);
                                        this.AnalyseTaxonomicNameRank(TaxonParts, ref PositionOfAnalysis);
                                        this.AnalyseTaxonomicNameQualifier(TaxonParts, ref PositionOfAnalysis, _IdentificationQualifierType.QualifierInfraspecific);
                                        this.AnalyseTaxonomicNameAuthors(TaxonParts, ref PositionOfAnalysis, _AuthorType.AuthorsSpecies);
                                        if (TaxonParts.Length > PositionOfAnalysis)
                                        {
                                            this.AnalyseTaxonomicNameRank(TaxonParts, ref PositionOfAnalysis);
                                            if (TaxonParts.Length > PositionOfAnalysis)
                                            {
                                                // InfraspecificEpithet
                                                InfraspecificEpithet = TaxonParts[PositionOfAnalysis];
                                                if (InfraspecificEpithet.Length > 0)
                                                    this._NameParts.Add(NamePart.InfraspecificEpithet, InfraspecificEpithet);
                                                PositionOfAnalysis++;
                                                if (TaxonParts.Length > PositionOfAnalysis)
                                                {
                                                    // After InfraspecificEpithet
                                                    this.AnalyseTaxonomicNameAuthors(TaxonParts, ref PositionOfAnalysis, _AuthorType.AuthorsInfraspecific);
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
                                                        this._NameParts.Add(NamePart.Undefined, Undefined);
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
                        this._NameParts.Add(NamePart.TaxonomicRank, "sp.");
                        this._RankFound = true;
                    }
                }
            }
            catch (System.Exception ex) { this._Message += ex.Message + "\r\n"; }
        }

        private void AnalyseTaxonomicNameRank(string[] TaxonParts, ref int PositionOfAnalysis)
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
                    if (!this._NameParts.ContainsKey(NamePart.TaxonomicRank))
                        this._NameParts.Add(NamePart.TaxonomicRank, Rank);
                    this._RankFound = true;
                }
            }
        }

        private void AnalyseTaxonomicNameAuthors(string[] TaxonParts, ref int PositionOfAnalysis, _AuthorType AuthorType)
        {
            string Authors = "";
            try
            {
                if (TaxonParts.Length > PositionOfAnalysis)
                {
                    if ((TaxonParts[PositionOfAnalysis].Substring(0, 1).ToUpper() == TaxonParts[PositionOfAnalysis].Substring(0, 1) &&
                        !this.RankList.Contains(TaxonParts[PositionOfAnalysis].Substring(0, 1)))
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
                        {
                            if (!this._NameParts.ContainsKey(NamePart.AuthorType))
                                this._NameParts.Add(NamePart.AuthorType, AuthorType.ToString());
                            if (!this._NameParts.ContainsKey(NamePart.Authors))
                                this._NameParts.Add(NamePart.Authors, Authors.Trim());
                        }
                    }
                }
            }
            catch (System.Exception ex) { this._Message += ex.Message + "\r\n"; }
        }

        private void AnalyseTaxonomicNameQualifier(string[] TaxonParts, ref int PositionOfAnalysis, _IdentificationQualifierType QualifierType)
        {
            string Qualifier = "";
            try
            {
                if (TaxonParts.Length > PositionOfAnalysis)
                {
                    if (TaxonParts.Length > PositionOfAnalysis + 1
                        &&
                        this.IdenfificationQualifierList.Contains(TaxonParts[PositionOfAnalysis] + " " + TaxonParts[PositionOfAnalysis + 1]))
                    {
                        // IdentificationQualifier has 2 parts
                        Qualifier = TaxonParts[PositionOfAnalysis] + " " + TaxonParts[PositionOfAnalysis + 1];
                        PositionOfAnalysis++;
                        PositionOfAnalysis++;
                    }
                    else if (this.IdenfificationQualifierList.Contains(TaxonParts[PositionOfAnalysis]))
                    {
                        // IdentificationQualifier has 1 part
                        Qualifier = TaxonParts[PositionOfAnalysis];
                        PositionOfAnalysis++;
                    }

                    // Writing the found Qualifier
                    if (Qualifier.Length > 0)
                    {
                        if (TaxonParts.Length <= PositionOfAnalysis)
                        {
                            this._NameParts.Add(NamePart.IdentificationQualifierType, _IdentificationQualifierType.QualifierTerminatory.ToString());
                        }
                        else
                            this._NameParts.Add(NamePart.IdentificationQualifierType, Qualifier);
                    }
                }
            }
            catch (System.Exception ex) { this._Message += ex.Message + "\r\n"; }
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
                    catch (System.Exception ex) { this._Message += ex.Message + "\r\n"; }
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

        //public string Family
        //{ get { return ""; } }

        //public string Genus
        //{ get { return ""; } }

        //public string Species
        //{ get { return ""; } }

        //public string Authors
        //{ get { return ""; } }

        //public string Qualifier
        //{ get { return ""; } }

        //public bool IsHybrid
        //{ get { return false; } }

        public System.Collections.Generic.Dictionary<DiversityCollection.TaxonomicName.NamePart, string> TaxonomicNameParts
        { get { return this._NameParts; } }
    }
}
