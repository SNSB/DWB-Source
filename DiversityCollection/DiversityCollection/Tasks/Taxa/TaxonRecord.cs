using DiversityWorkbench.Api.Taxon;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.Tasks.Taxa
{
    public class TaxonRecord
    {

        #region Parameter

        private DiversityWorkbench.Api.Taxon.Taxon _taxon;
        private string _Identifier;
        private string _Group = "";
        private IPM.RecordingTarget _recordingTarget;
        private Preview _Preview;
        private string _Stage;
        private int _StageID;
        private string _StageOrRemains;
        private int _Sorting;
        private string _VernacularName = "";
        private string _DisplayText = "";
        private System.Collections.Generic.Dictionary<int, Resource> _Images;
        private System.Collections.Generic.Dictionary<int, Resource> _Infos;
        private System.Collections.Generic.List<string> _States;

        #endregion

        #region Interface

        public System.Collections.Generic.Dictionary<int, Resource> Infos
        {
            get
            {
                if( _Infos == null)
                {
                    _Infos = new Dictionary<int, Resource>();
                    if (_taxon.Resource != null)
                    {
                        foreach (DiversityWorkbench.Api.Taxon.TaxonResource r in _taxon.Resource)
                        {
                            if (r.Type != null && r.Type.ToLower() == "information")
                            {
                                Resource resource = new Resource();
                                resource.Title = r.Title;
                                resource.NameID = _taxon.ID;
                                resource.Uri = new Uri(r.URI);
                                resource.Type = ResourceType.Info;
                                _Infos.Add(_Infos.Count + 1, resource);
                            }
                        }
                    }
                }
                return _Infos;
            }
        }

        public System.Collections.Generic.Dictionary<int, Resource> Images
        {
            get
            {
                if (_Images == null)
                {
                    _Images = new Dictionary<int, Resource>();
                    if (_taxon.Resource != null)
                    {
                        foreach (DiversityWorkbench.Api.Taxon.TaxonResource r in _taxon.Resource)
                        {
                            if (r.Type != null && r.Type.ToLower() == "image")
                            {
                                Resource resource = new Resource();
                                resource.Title = r.Title;
                                resource.NameID = _taxon.ID;
                                resource.Uri = new Uri(r.URI);
                                resource.Type = ResourceType.Image;
                                _Images.Add(_Images.Count + 1, resource);
                            }
                        }
                    }
                }
                return _Images;
            }
        }

        public string Identifier { get => _Identifier; }

        public int Sorting { get => _Sorting; }

        public string Stage { get => _Stage; }
        public int StageID { get => _StageID; }

        public string StageOrRemains { get => _StageOrRemains; }
        //public int AcceptedNameID { set => _AcceptedNameID = value; }

        public int NameID { get => _taxon.ID; }
        //public string BaseURL { get => _BaseURL; }
        public string ScientificName { get => _taxon.FullName; }
        public string VernacularName 
        { 
            get
            {
                return _VernacularName;
            } 
        }
        //public bool AcceptedName { get => _AcceptedNameID == _NameID; }
        public string Group { get => _Group; }
        //public IPM.TaxonSource taxonSource { get => _taxonSource; }
        public Preview PreviewImage { get => _Preview; }
        public bool PreviewExists { get => PreviewImage.Icon != null; }
        //public System.Collections.Generic.Dictionary<int, Resource> Images
        //{
        //    //get
        //    //{
        //    //    return _Taxon.Images;
        //    //}
        //}
        //public System.Collections.Generic.Dictionary<int, Resource> Infos
        //{
        //    //get
        //    //{
        //    //    return _Taxon.Infos;
        //    //}
        //}

        public string DisplayText
        {
            get
            {
                if (_DisplayText == "")
                {
                    _DisplayText = this.VernacularName;
                    if(Settings.Default.ShowScientificName)
                    {
                        _DisplayText += "\r\n(" + ScientificName + ")";
                    }
                    _DisplayText += "\r\n" + Stage;
                }
                return _DisplayText;
            }
        }

        public System.Collections.Generic.List<string> States
        {
            get
            {
                if (_States == null)
                {
                    _States = new List<string>();
                }
                return _States;
            }
        }

        public string NameURI()
        {
            return _taxon.URL;
            //return Database.TaxonDBBaseURL + NameID.ToString();
        }

        #endregion

        #region Construction

        //public Record(string Identifier, DiversityWorkbench.Api.Taxon.Taxon taxon, IPM.TaxonSource taxonSource)
        //{
        //    _taxon = taxon;
        //    _Identifier = Identifier;
        //    _taxonSource = taxonSource;
        //    this.initRecord();
        //}

        public TaxonRecord(string Identifier, DiversityWorkbench.Api.Taxon.Taxon taxon, IPM.RecordingTarget recordingTarget)
        {
            _taxon = taxon;
            _Identifier = Identifier;
            _recordingTarget = recordingTarget;
            this.initRecord();
        }

        #endregion

        #region init

        private void initRecord()
        {
            try
            {
                this.initCommonName();
                this.getGroupAndSorting();
                this.initStage();
                this.initPreview();
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void initCommonName()
        {
            try
            {
                string Language = DiversityWorkbench.Settings.Language.Substring(0, 2).ToLower();
                if (this._taxon.CommonNames != null)
                {
                    foreach (DiversityWorkbench.Api.Taxon.TaxonCommonNames c in this._taxon.CommonNames)
                    {
                        if (c.LanguageCode.ToLower() == Language && c.CommonName != null && c.CommonName.Length > 0)
                        {
                            this._VernacularName = c.CommonName;
                            break;
                        }
                    }
                }
                if (this._VernacularName.Length == 0) this._VernacularName = this._taxon.Name;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

        }

        private void getGroupAndSorting()
        {
            try
            {
                System.Collections.Generic.List<int> hierarchy = new List<int>();
                foreach (DiversityWorkbench.Api.Taxon.TaxonHierarchy h in this._taxon.Hierarchy)
                {
                    hierarchy.Add(h.ID);
                }
#if old
                foreach (int ID in hierarchy)
                {
                    int MinID = DiversityWorkbench.Api.Taxon.Taxa.ChecklistTaxa(IPM.ChecklistID(_taxonSource)).First().Key;
                    int MaxID = DiversityWorkbench.Api.Taxon.Taxa.ChecklistTaxa(IPM.ChecklistID(_taxonSource)).Last().Key;
                    int MaxIDsort = MaxID - MinID;
                    if (DiversityWorkbench.Api.Taxon.Taxa.ChecklistTaxa(IPM.ChecklistID(_taxonSource)).ContainsKey(ID))
                    {
                        DiversityWorkbench.Api.Taxon.Taxon taxon = DiversityWorkbench.Api.Taxon.Taxa.ChecklistTaxa(IPM.ChecklistID(_taxonSource))[ID];
                        System.Collections.Generic.List<DiversityWorkbench.Api.Taxon.TaxonChecklist> checklists = taxon.Checklist;
                        foreach (DiversityWorkbench.Api.Taxon.TaxonChecklist c in checklists)
                        {
                            if (c.ChecklistID == IPM.ChecklistID(_taxonSource))
                            {
                                if (c.Analysis != null && c.Analysis.Count > 0)
                                {
                                    foreach (DiversityWorkbench.Api.Taxon.ChecklistAnalysis a in c.Analysis)
                                    {
                                        string[] aa = a.Analysis.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries);
                                        int Sort;
                                        if (aa[0].ToLower() == "ipm" && aa.Length > 1 && aa[1].ToLower() == "group" && a.Value != null && int.TryParse(a.Value, out Sort))
                                        {
                                            _Group = DiversityWorkbench.Api.Taxon.Taxa.ChecklistTaxa(IPM.ChecklistID(_taxonSource))[ID].Name;
                                            _Sorting = Sort * 100000 + (_taxon.ID - MinID) * 100 + a.AnalysisID;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (_Group.Length > 0) break;
                        }
                        if (_Group.Length > 0) break;
                    }
                    else { }
                }
#else
                foreach (int ID in hierarchy)
                {
                    if (DiversityWorkbench.Api.Taxon.Taxa.ChecklistTaxa(IPM.ChecklistID(_recordingTarget)).Count > 0)
                    {
                        int MinID = DiversityWorkbench.Api.Taxon.Taxa.ChecklistTaxa(IPM.ChecklistID(_recordingTarget)).First().Key;
                        int MaxID = DiversityWorkbench.Api.Taxon.Taxa.ChecklistTaxa(IPM.ChecklistID(_recordingTarget)).Last().Key;
                        int MaxIDsort = MaxID - MinID;
                        if (DiversityWorkbench.Api.Taxon.Taxa.ChecklistTaxa(IPM.ChecklistID(_recordingTarget)).ContainsKey(ID))
                        {
                            DiversityWorkbench.Api.Taxon.Taxon taxon = DiversityWorkbench.Api.Taxon.Taxa.ChecklistTaxa(IPM.ChecklistID(_recordingTarget))[ID];
                            System.Collections.Generic.List<DiversityWorkbench.Api.Taxon.TaxonChecklist> checklists = taxon.Checklist;
                            foreach (DiversityWorkbench.Api.Taxon.TaxonChecklist c in checklists)
                            {
                                if (c.ChecklistID == IPM.ChecklistID(_recordingTarget))
                                {
                                    if (c.Analysis != null && c.Analysis.Count > 0)
                                    {
                                        foreach (DiversityWorkbench.Api.Taxon.ChecklistAnalysis a in c.Analysis)
                                        {
                                            string[] aa = a.Analysis.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries);
                                            int Sort;
                                            if (aa[0].ToLower() == "ipm" && aa.Length > 1 && aa[1].ToLower() == "group" && a.Value != null && int.TryParse(a.Value, out Sort))
                                            {
                                                _Group = DiversityWorkbench.Api.Taxon.Taxa.ChecklistTaxa(IPM.ChecklistID(_recordingTarget))[ID].Name;
                                                _Sorting = Sort * 100000 + (_taxon.ID - MinID) * 100 + a.AnalysisID;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (_Group.Length > 0) break;
                            }
                            if (_Group.Length > 0) break;
                        }
                        else { }
                    }
                }
#endif
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

        }

        private void initStage()
        {
            try
            {
                if (_Identifier != null && _Identifier.IndexOf("|") > -1)
                {
                    string NameID = _Identifier.Substring(Database.TaxonDBBaseURL.Length);
                    int AnalysisID;
                    if (int.TryParse(NameID.Substring(NameID.IndexOf("|") + 1), out AnalysisID))
                    {
                        System.Collections.Generic.List<DiversityWorkbench.Api.Taxon.TaxonChecklist> checklists = _taxon.Checklist;
                        foreach (DiversityWorkbench.Api.Taxon.TaxonChecklist c in checklists)
                        {
                            if (c.ChecklistID == IPM.ChecklistID(_recordingTarget))
                            {
                                if (c.Analysis != null && c.Analysis.Count > 0)
                                {
                                    foreach (DiversityWorkbench.Api.Taxon.ChecklistAnalysis a in c.Analysis)
                                    {
                                        if (a.AnalysisID == AnalysisID)
                                        {
                                            string[] aa = a.Analysis.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries);
                                            if (aa[0].ToLower() == "ipm" && aa.Length > 1 && (aa[1].ToLower() == "stage" || aa[1].ToLower() == "remains") && aa.Length > 2)
                                            {
                                                _Stage = aa[2].ToLower();
                                                _StageOrRemains = aa[1].ToLower();
                                                if (_Stage.ToLower() == "adult" || _Stage.ToLower() == "larva")
                                                {
                                                    this.States.Add("alive");
                                                    this.States.Add("dead");
                                                    this.States.Add("unsure");
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (_Stage != null && _Stage.Length > 0) break;
                        }
                    }
                }
                else _Stage = "";
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

        }

        private void initPreview()
        {
            try
            {
                if (_Identifier != null && _Identifier.IndexOf("|") > -1)
                {
                    string NameID = _Identifier.Substring(Database.TaxonDBBaseURL.Length);
                    int AnalysisID;
                    if (int.TryParse(NameID.Substring(NameID.IndexOf("|") + 1), out AnalysisID))
                    {
                        System.Collections.Generic.List<int> hierarchy = new List<int>();
                        foreach (DiversityWorkbench.Api.Taxon.TaxonHierarchy h in this._taxon.Hierarchy)
                        {
                            hierarchy.Add(h.ID);
                        }
                        foreach (int ID in hierarchy)
                        {
                            if (DiversityWorkbench.Api.Taxon.Taxa.ChecklistTaxa(IPM.ChecklistID(_recordingTarget)).ContainsKey(ID))
                            {
                                DiversityWorkbench.Api.Taxon.Taxon taxon = DiversityWorkbench.Api.Taxon.Taxa.ChecklistTaxa(IPM.ChecklistID(_recordingTarget))[ID];
                                System.Collections.Generic.List<DiversityWorkbench.Api.Taxon.TaxonChecklist> checklists = taxon.Checklist;
                                foreach (DiversityWorkbench.Api.Taxon.TaxonChecklist c in checklists)
                                {
                                    if (c.Analysis != null && c.Analysis.Count > 0)
                                    {
                                        foreach (DiversityWorkbench.Api.Taxon.ChecklistAnalysis a in c.Analysis)
                                        {
                                            if (a.AnalysisID == AnalysisID)
                                            {
                                                if (a.PreviewImage != null && a.PreviewImage.Length > 0)
                                                {
                                                    _Preview = new Preview();
                                                    _Preview.NameID = _taxon.ID;
                                                    _Preview.StageID = _StageID;
                                                    _Preview.URI = _taxon.URL;
                                                    string Message = "";
                                                    _Preview.Icon = DiversityWorkbench.Forms.FormFunctions.BitmapFromWeb(a.PreviewImage, ref Message);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (_Preview.Icon != null) break;
                                }
                            }
                            if (_Preview.Icon != null) break;
                        }
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        public void SetParamters()
        {
            if (PreviewExists == false) initPreview();
            if ((_Stage.ToLower() == "adult" || _Stage.ToLower() == "larva") && (_States == null || _States.Count == 0)) this.initStage();
            if (this._Group == null || this._Group.Length == 0) this.getGroupAndSorting();
        }

#endregion

    }
}
