using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.Tasks
{
    public struct TaxonGroup
    {
        public int NameID;
        public string GroupName;
        public System.Collections.Generic.List<int> InferiorNameIDs;
    }

    public class Taxon : iItem
    {
        #region Parameter

        private int _NameID;
        private string _BaseURL;
        private string _ScientificName;
        private string _VernacularName;
        private bool _AcceptedName;
        private string _Group;
        private IPM.TaxonSource _taxonSource;
        private System.Collections.Generic.Dictionary<int, Resource> _Icones;
        private System.Collections.Generic.Dictionary<int, Resource> _Images;
        private System.Collections.Generic.Dictionary<int, Resource> _Infos;
        private string _Stage;
        private System.Collections.Generic.Dictionary<string, Taxon> _Stages;
        private string _PreviewImage;

        private static System.Collections.Generic.Dictionary<int, TaxonGroup> _TaxonGroups;

        #endregion

        #region list

        private static System.Collections.Generic.Dictionary<int, Taxon> _Pests;
        public static System.Collections.Generic.Dictionary<int, Taxon> Pests
        {
            get
            {
                if (_Pests == null)
                {
                    _Pests = new Dictionary<int, Taxon>();
                    string tntServer = global::DiversityCollection.Properties.Settings.Default.TNTServer;
                    string tntDB = global::DiversityCollection.Properties.Settings.Default.TNTTaxaVariaDB;
                    string tnt = "[" + tntServer + "].[" + tntDB + "]";
                    string SQL = "select T.NameID, C.[CommonName] as Name, " +
                        "T.[GenusOrSupragenericName] + case when [SpeciesEpithet] is null then '' else ' ' + [SpeciesEpithet] end AS Taxon, " +
                        "case when V.DisplayText is null then A.DisplayText else V.DisplayText end AS ObservedPart, Ip.[AnalysisValue] AS PreviewImage " +
                        "from " + tnt + ".dbo.TaxonName T " +
                        "inner join " + tnt + ".dbo.TaxonNameListAnalysis L on T.NameID = L.NameID and L.ProjectID = 1190 " +
                        "inner join " + tnt + ".dbo.TaxonNameListAnalysisCategory A ON A.AnalysisID = L.AnalysisID " +
                        "inner join " + tnt + ".dbo.TaxonNameListAnalysisCategory P ON A.AnalysisParentID = P.AnalysisID and P.DisplayText in (N'Stage', N'Remains') " +
                        "left outer join " + tnt + ".dbo.TaxonNameListAnalysisCategory I ON I.AnalysisParentID = A.AnalysisID and I.DisplayText = 'preview image' " +
                        "left outer join " + tnt + ".dbo.TaxonNameListAnalysis Ip on I.AnalysisID = Ip.AnalysisID and T.NameID = Ip.NameID " +
                        "left outer join " + tnt + ".dbo.TaxonNameListAnalysisCategoryValue V ON A.AnalysisID = V.AnalysisID and V.[AnalysisValue] = '" + DiversityWorkbench.Settings.Language.Substring(0, 2).ToLower() + "' " +
                        "left outer join " + tnt + ".[dbo].[TaxonCommonName] C ON T.NameID = C.NameID and C.[CommonName] <> '' AND C.[LanguageCode] = '" + DiversityWorkbench.Settings.Language.Substring(0, 2).ToLower() + "' ";
                    System.Data.DataTable dt = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                    foreach(System.Data.DataRow R in dt.Rows)
                    {
                        int ID;
                        if (int.TryParse(R["NameID"].ToString(), out ID))
                        {
                            string CommonName = "";
                            if (!R["CommonName"].Equals(System.DBNull.Value) && R["CommonName"].ToString().Length > 0) CommonName = R["CommonName"].ToString();
                            string Taxon = R["Taxon"].ToString();
                            string Part = R["ObservedPart"].ToString();
                            string PreviewImage = "";
                            if (!R["PreviewImage"].Equals(System.DBNull.Value) && R["PreviewImage"].ToString().Length > 0) PreviewImage = R["PreviewImage"].ToString();
                            if (_Pests.ContainsKey(ID))
                            {

                            }
                            else
                            {
                                Taxon taxon = new Taxon(ID, IPM.TaxonSource.Pest);

                            }
                        }
                    }
                }
                return _Pests;
            }
        }

        #endregion

        #region Interface

        public int NameID { get => _NameID; }
        public string BaseURL { get => _BaseURL; }
        public string ScientificName { get => _ScientificName; }
        public string VernacularName { get => _VernacularName; }
        public bool AcceptedName { get => _AcceptedName; }
        public string Group { get => _Group; }
        public IPM.TaxonSource taxonSource { get => _taxonSource; }
        public System.Collections.Generic.Dictionary<int, Resource> Icones { get => _Icones; }
        public System.Collections.Generic.Dictionary<int, Resource> Images { get => _Images; }
        public System.Collections.Generic.Dictionary<int, Resource> Infos { get => _Infos; }

        public System.Collections.Generic.Dictionary<string, Taxon> Stages 
        {
            get 
            {
                if (_Stages == null)
                    _Stages = new Dictionary<string, Taxon>();
                return _Stages; 
            } 
        }
        public string PreviewImage { get => _PreviewImage; }

        public string DisplayText()
        {
            string Name = VernacularName;
            if (this.Stage != null)
                Name += "\r\n" + this.Stage;
            if (ScientificName != null && ScientificName.Length > 0) // && Name.IndexOf("(") == -1)
                Name += "\r\n(" + ScientificName + ")";
            return Name;
        }

        public string NameURI()
        {
            return BaseURL + NameID.ToString();
        }



        #endregion

        #region Construction


        public Taxon(int NameID, IPM.TaxonSource taxonSource)
        {
            this._NameID = NameID;
            this._taxonSource = taxonSource;
            this.initCommonNames();
            //this.initIcones();
            this.initImages();
            this.initInfos();
            this.initStages();
        }

        private Taxon(Taxon pest, string Stage)
        {
            this._NameID = pest.NameID;
            this._BaseURL = pest.BaseURL;
            this._ScientificName = pest.ScientificName;
            this._VernacularName = pest.VernacularName;
            this._AcceptedName = pest.AcceptedName;
            this._Group = pest.Group;
            this._Images = pest.Images;
            this._Infos = pest.Infos;
            this.initIcones();
            //this._Icones = pest.Icones;
        }

        #endregion

        #region init
        private string Prefix() { return Settings.Default.DiversityTaxonNamesDatabase + ".dbo."; }

        private void initCommonNames()
        {
            try
            {
                string Language = DiversityWorkbench.Settings.Language.Substring(0, 2).ToLower();

                string SQL = "SELECT T.TaxonNameCache AS Taxon, TC.CommonName AS Art, TC.LanguageCode AS LanguageArt " +
                    "FROM " + Prefix() + "TaxonName AS T " +
                    "LEFT OUTER JOIN " + Prefix() + "TaxonCommonName AS TC ON T.NameID = TC.NameID " +
                    "INNER JOIN " + Prefix() + "TaxonNameList AS L ON T.NameID = L.NameID AND L.ProjectID = ";
                switch (taxonSource)
                {
                    case IPM.TaxonSource.Pest:
                        SQL += Settings.Default.DiversityTaxonNamesPestListID.ToString();
                        break;
                    case IPM.TaxonSource.Beneficial:
                        SQL += Settings.Default.DiversityTaxonNamesBeneficialListID.ToString();
                        break;
                    case IPM.TaxonSource.Bycatch:
                        SQL += Settings.Default.DiversityTaxonNamesBycatchListID.ToString();
                        break;
                }
                SQL += " WHERE T.NameID = " + NameID.ToString();
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                if (dt.Rows.Count > 0)
                {
                    _ScientificName = dt.Rows[0]["Taxon"].ToString();
                    System.Data.DataRow[] rr = dt.Select(" LanguageArt = '" + Language + "'");
                    if (rr.Length > 0)
                        _VernacularName = rr[0]["Art"].ToString();
                    else
                    {
                        System.Data.DataRow[] rrL = dt.Select(" LanguageArt = '" + Language + "'");
                        if (rrL.Length > 0)
                            _VernacularName = rrL[0]["Art"].ToString();
                        else if (dt.Rows.Count > 0)
                            _VernacularName = dt.Rows[0]["Art"].ToString();
                    }
                    this.getTaxonGroup(NameID);
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void getTaxonGroup(int NameID, System.Collections.Generic.List<int> Inferiors = null)
        {
            try
            {
                string Language = DiversityWorkbench.Settings.Language.Substring(0, 2).ToLower();

                string SQL = "SELECT GC.CommonName AS Gruppe , GC.LanguageCode AS LanguageGruppe, H.NameParentID, A.AnalysisID " +
                    "FROM " + Prefix() + "TaxonName AS T " +
                    "INNER JOIN " + Prefix() + "TaxonHierarchy AS H ON T.NameID = H.NameID " +
                    "INNER JOIN " + Prefix() + "TaxonName AS G ON H.NameParentID = G.NameID " +
                    "LEFT OUTER JOIN " + Prefix() + "TaxonCommonName AS GC ON G.NameID = GC.NameID " +
                    "INNER JOIN " + Prefix() + "TaxonNameList AS L ON H.NameParentID = L.NameID AND L.ProjectID = ";
                switch (taxonSource)
                {
                    case IPM.TaxonSource.Pest:
                        SQL += Settings.Default.DiversityTaxonNamesPestListID.ToString();
                        break;
                    case IPM.TaxonSource.Beneficial:
                        SQL += Settings.Default.DiversityTaxonNamesBeneficialListID.ToString();
                        break;
                    case IPM.TaxonSource.Bycatch:
                        SQL += Settings.Default.DiversityTaxonNamesBycatchListID.ToString();
                        break;
                }
                SQL += " LEFT OUTER JOIN [TNT.DIVERSITYWORKBENCH.DE,5432].DiversityTaxonNames_TaxaVaria.dbo.TaxonNameListAnalysis AS A ON A.NameID = L.NameID AND A.ProjectID = L.ProjectID AND A.AnalysisID = 1 ";
                SQL += " WHERE (H.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ") " +
                    " AND T.NameID = " + NameID.ToString();
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                if (dt.Rows.Count > 0)
                {
                    int NameParentID = -1;
                    if (!dt.Rows[0]["NameParentID"].Equals(System.DBNull.Value))
                        int.TryParse(dt.Rows[0]["NameParentID"].ToString(), out NameParentID);

                    if (_TaxonGroups != null && NameParentID > -1)
                    {
                        if (_TaxonGroups.ContainsKey(NameParentID))
                            _Group = _TaxonGroups[NameParentID].GroupName;
                        else
                        {
                            foreach (System.Collections.Generic.KeyValuePair<int, TaxonGroup> KV in _TaxonGroups)
                            {
                                if (KV.Value.InferiorNameIDs.Contains(NameParentID))
                                {
                                    _Group = KV.Value.GroupName;
                                    if (!KV.Value.InferiorNameIDs.Contains(NameID))
                                        KV.Value.InferiorNameIDs.Add(NameID);
                                    break;
                                }
                            }
                        }
                    }

                    if (_Group == null)
                    {
                        if (!dt.Rows[0]["AnalysisID"].Equals(System.DBNull.Value))
                        {
                            _Group = dt.Rows[0]["Gruppe"].ToString();
                            if (_TaxonGroups == null)
                            {
                                _TaxonGroups = new Dictionary<int, TaxonGroup>();
                            }
                            if (!_TaxonGroups.ContainsKey(NameParentID))
                            {
                                if (Inferiors == null) Inferiors = new List<int>();
                                Inferiors.Add(NameID);
                                TaxonGroup taxonGroup = new TaxonGroup();
                                taxonGroup.GroupName = _Group;
                                taxonGroup.NameID = NameParentID;
                                taxonGroup.InferiorNameIDs = Inferiors;
                                _TaxonGroups.Add(NameParentID, taxonGroup);
                            }
                        }
                        else if (NameParentID > -1)
                        {
                            if (Inferiors == null) Inferiors = new List<int>();
                            Inferiors.Add(NameParentID);
                            getTaxonGroup(NameParentID, Inferiors);
                        }
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void initCommonNames_old()
        {
            try
            {
                string Language = DiversityWorkbench.Settings.Language.Substring(0, 2).ToLower();
                string Country = DiversityWorkbench.Settings.Language.Substring(3, 2).ToLower();

                string SQL = "SELECT T.TaxonNameCache AS Taxon, TC.CommonName AS Art, TC.LanguageCode AS LanguageArt, TC.CountryCode AS CountryArt, " +
                    "GC.CommonName AS Gruppe , GC.LanguageCode AS LanguageGruppe, GC.CountryCode AS CountryGruppe " +
                    "FROM " + Prefix() + "TaxonName AS T " +
                    "INNER JOIN " + Prefix() + "TaxonHierarchy AS H ON T.NameID = H.NameID " +
                    "INNER JOIN " + Prefix() + "TaxonName AS G ON H.NameParentID = G.NameID " +
                    "LEFT OUTER JOIN " + Prefix() + "TaxonCommonName AS TC ON T.NameID = TC.NameID " +
                    "LEFT OUTER JOIN " + Prefix() + "TaxonCommonName AS GC ON G.NameID = GC.NameID " +
                    "INNER JOIN " + Prefix() + "TaxonNameList AS L ON T.NameID = L.NameID AND L.ProjectID = ";
                switch (taxonSource)
                {
                    case IPM.TaxonSource.Pest:
                        SQL += Settings.Default.DiversityTaxonNamesPestListID.ToString();
                        break;
                    case IPM.TaxonSource.Beneficial:
                        SQL += Settings.Default.DiversityTaxonNamesBeneficialListID.ToString();
                        break;
                    case IPM.TaxonSource.Bycatch:
                        SQL += Settings.Default.DiversityTaxonNamesBycatchListID.ToString();
                        break;
                }
                SQL += " WHERE (H.ProjectID = " + Settings.Default.DiversityTaxonNamesProjectID.ToString() + ") " +
                    " AND T.NameID = " + NameID.ToString();
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                if (dt.Rows.Count > 0)
                    _ScientificName = dt.Rows[0]["Taxon"].ToString();
                System.Data.DataRow[] rr = dt.Select(" LanguageArt = '" + Language + "' AND CountryArt = '" + Country + "'");
                if (rr.Length > 0)
                    _VernacularName = rr[0]["Art"].ToString();
                else
                {
                    System.Data.DataRow[] rrL = dt.Select(" LanguageArt = '" + Language + "'");
                    if (rrL.Length > 0)
                        _VernacularName = rrL[0]["Art"].ToString();
                    else if (dt.Rows.Count > 0)
                        _VernacularName = dt.Rows[0]["Art"].ToString();
                }

                System.Data.DataRow[] rrG = dt.Select(" LanguageGruppe = '" + Language + "' AND CountryGruppe = '" + Country + "'");
                if (rrG.Length > 0)
                    _Group = rrG[0]["Gruppe"].ToString();
                else
                {
                    System.Data.DataRow[] rrL = dt.Select(" LanguageGruppe = '" + Language + "'");
                    if (rrL.Length > 0)
                        _Group = rrL[0]["Gruppe"].ToString();
                    else if (dt.Rows.Count > 0)
                        _Group = dt.Rows[0]["Gruppe"].ToString();
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// Getting the stages including their preview images
        /// </summary>
        private void initStages()
        {
            try
            {
                string SQL = "SELECT C.DisplayText AS Stage, I.AnalysisValue AS Preview " +
                    "FROM " + Prefix() + "TaxonNameListAnalysisCategory AS C " +
                    "INNER JOIN " + Prefix() + "TaxonNameListAnalysis AS S ON C.AnalysisID = S.AnalysisID " +
                    "LEFT OUTER JOIN " + Prefix() + "TaxonNameListAnalysisCategory AS PI ON C.AnalysisID = PI.AnalysisParentID " +
                    "LEFT OUTER JOIN " + Prefix() + "TaxonNameListAnalysis AS I ON PI.AnalysisID = I.AnalysisID AND S.NameID = I.NameID " +
                    "INNER JOIN " + Prefix() + "ViewTaxonNameResource AS R ON R.NameID = I.NameID AND R.URI = I.AnalysisValue " +
                    "WHERE (C.SortingID = 1) AND (C.DataWithholdingReason IS NULL) AND PI.DisplayText = 'preview image' AND S.NameID = " + this.NameID.ToString();
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    string Stage = R["Stage"].ToString();
                    Taxon pest = new Taxon(this, Stage);
                    if (!R["Preview"].Equals(System.DBNull.Value))
                        pest._PreviewImage = R["Preview"].ToString();
                    if (this._Stages == null) this._Stages = new Dictionary<string, Taxon>();
                    if (!this._Stages.ContainsKey(Stage))
                        this._Stages.Add(Stage, pest);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

        }

        private string Stage
        {
            get
            {
                string stage = _Stage;
                string Language = DiversityWorkbench.Settings.Language.Substring(0, 2).ToLower();
                switch(Language)
                {
                    case "de":
                    switch(_Stage)
                        {
                            case "larva":
                                stage = "Larve";
                                break;
                            case "egg":
                                stage = "Eier";
                                break;
                            case "cocoon":
                                stage = "Gespinst";
                                break;
                            case "exuviae":
                                stage = "Exuvie";
                                break;
                        }
                        break;
                }
                return stage;
            }
        }

        private void initImages()
        {
            try
            {
                string SQL = "SELECT Im.NameID, Im.URI AS Image, Im.Notes, Im.CopyrightStatement, Im.DisplayOrder " +
                    "FROM " + Prefix() + "ViewTaxonNameResource AS Im " +
                    "WHERE Im.ResourceType = 'Image' AND (Im.NameID = " + NameID.ToString() + ") " +
                    "ORDER BY Im.DisplayOrder";
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                if (dt.Rows.Count > 0)
                {
                    int i = 0;
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        Resource resource = new Resource();
                        resource.Type = ResourceType.Image;
                        resource.Title = this.DisplayText();
                        if (!R["Image"].Equals(System.DBNull.Value) && R["Image"].ToString().Length > 0)
                            resource.Uri = new Uri(R["Image"].ToString());
                        if (!R["Notes"].Equals(System.DBNull.Value) && R["Notes"].ToString().Length > 0)
                            resource.Notes = R["Notes"].ToString();
                        resource.CopyRight = R["CopyrightStatement"].ToString();
                        if (this._Images == null) this._Images = new Dictionary<int, Resource>();
                        this.Images.Add(i, resource);
                        i++;
                    }
                }
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
                string SQL = "SELECT URI AS Info, Creator " +
                "FROM " + Prefix() + "ViewTaxonNameResource AS R " +
                "WHERE ResourceType = 'Information' AND (NameID = " + NameID.ToString() + ") " +
                "ORDER BY DisplayOrder";
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                if (dt.Rows.Count > 0)
                {
                    int i = 0;
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        Resource resource = new Resource();
                        resource.Type = ResourceType.Info;
                        resource.Title = this.DisplayText();
                        if (!R["Info"].Equals(System.DBNull.Value) && R["Info"].ToString().Length > 0)
                            resource.Uri = new Uri(R["Info"].ToString());
                        resource.CopyRight = R["Creator"].ToString();
                        if (this._Infos == null) this._Infos = new Dictionary<int, Resource>();
                        this.Infos.Add(i, resource);
                        i++;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        private void initPreviewImage()
        {
            if (this._PreviewImage == null)
            {
                //string SQL = "SELECT Ic.NameID, Ic.URI AS Icon, Ic.Title, Ic.Notes, Ic.DisplayOrder " +
                //    "FROM " + Prefix() + "ViewTaxonNameResource AS Ic " +
                //    "WHERE Ic.ResourceType = 'preview' AND (Ic.NameID = " + NameID.ToString() + ") " +
                //    "ORDER BY Ic.DisplayOrder";
            }
        }

        private void initIcones()
        {
            try
            {
                if (this._Icones == null) this._Icones = new Dictionary<int, Resource>();
                Resource resource = new Resource();
                resource.Type = ResourceType.Image;
                resource.Title = this.DisplayText();

                if (this._PreviewImage != null && this._PreviewImage.Length > 0)
                {
                    string Message = "";
                    resource.Icon = DiversityWorkbench.Forms.FormFunctions.BitmapFromWeb(this._PreviewImage, ref Message);
                }
                else
                    resource.Icon = this.NoImage();
                _Icones.Add(0, resource);

                //string SQL = "SELECT Ic.NameID, Ic.URI AS Icon, Ic.Title, Ic.Notes, Ic.DisplayOrder " +
                //    "FROM " + Prefix() + "ViewTaxonNameResource AS Ic " +
                //    "WHERE Ic.ResourceType = 'preview' AND (Ic.NameID = " + NameID.ToString() + ") " +
                //    "ORDER BY Ic.DisplayOrder";

                //System.Data.DataTable dt = new System.Data.DataTable();
                //DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                //if (dt.Rows.Count > 0)
                //{
                //    int i = 0;
                //    foreach (System.Data.DataRow R in dt.Rows)
                //    {
                //        Resource resource = new Resource();
                //        resource.Type = ResourceType.Image;
                //        //resource.Title = Art;
                //        if (!R["Title"].Equals(System.DBNull.Value) && R["Title"].ToString().Length > 0)
                //            resource.Title = R["Title"].ToString();
                //        if (!R["Icon"].Equals(System.DBNull.Value) && R["Icon"].ToString().Length > 0)
                //            resource.Icon = DiversityWorkbench.Forms.FormFunctions.BitmapFromWeb(R["Icon"].ToString(), ref Message);
                //        else
                //            resource.Icon = this.NoImage();
                //        if (!R["Notes"].Equals(System.DBNull.Value) && R["Notes"].ToString().Length > 0)
                //            resource.Notes = R["Notes"].ToString();
                //        if (this._Icones == null) this._Icones = new Dictionary<int, Resource>();
                //        this._Icones.Add(i, resource);
                //        this._BaseURL = this.BaseURL;
                //        i++;
                //    }
                //}
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

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

        #region interface

        #endregion

    }

}
