using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSamplingPlotPage;

namespace DiversityCollection.Tasks.Taxa
{
    class TaxonResource
    {
        //public TaxonResource(ResourceType type) 
        //{
        //    _Type = type;
        //}

        //private System.Collections.Generic.Dictionary<string, string> _PreviewImages;
        //private System.Collections.Generic.Dictionary<string, string> PreviewImages
        //{
        //    get
        //    {
        //        if (_PreviewImages == null)
        //        {
        //            _PreviewImages = new System.Collections.Generic.Dictionary<string, string>();
        //            foreach (System.Data.DataRow R in Database.ResourceTable(Tasks.ResourceType.Preview).Rows)
        //            {
        //                _PreviewImages.Add(R[0].ToString(), R[1].ToString());
        //            }
        //        }
        //        return _PreviewImages;
        //    }
        //}

        private static System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, Tasks.Preview>> _PreviewImages;
        private static System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, Tasks.Preview>> PreviewImages
        {
            get
            {
                if (_PreviewImages == null)
                {
                    _PreviewImages = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, Tasks.Preview>>();
                    foreach (System.Data.DataRow R in Database.ResourceTable(Tasks.ResourceType.Preview).Rows)
                    {
                        int NameID = int.Parse(R[0].ToString());
                        int StageID = int.Parse(R[1].ToString());
                        string URI = R[2].ToString();
                        PreviewImageAdd(NameID, StageID, URI);
                    }
                }
                return _PreviewImages;
            }
        }

        public static string PreviewImage(int NameID, int StageID = 0)
        {
            string URI = "";
            try
            {
                if (PreviewImages.ContainsKey(NameID) && PreviewImages[NameID].ContainsKey(StageID))
                    URI = PreviewImages[NameID][StageID].URI;
                else
                {
                    System.Collections.Generic.List<int> MissingImageEntry = new List<int>();
                    MissingImageEntry.Add(NameID);
                    bool ImageFound = false;
                    int CurrentID = NameID;
                    while (!ImageFound)
                    {
                        if (!Tasks.Taxa.Hierarchy.TaxonHierarchy.ContainsKey(CurrentID))
                            break;
                        int ParentID = Tasks.Taxa.Hierarchy.TaxonHierarchy[CurrentID];
                        MissingImageEntry.Add(ParentID);
                        if (PreviewImages.ContainsKey(ParentID) && PreviewImages[ParentID].ContainsKey(StageID))
                        {
                            URI = PreviewImages[ParentID][StageID].URI;
                            foreach (int nameID in MissingImageEntry)
                            {
                                PreviewImageAdd(nameID, StageID, URI);
                            }
                            ImageFound = true;
                            break;
                        }
                        CurrentID = ParentID;
                    }
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return URI;
        }

        private static void PreviewImageAdd(int NameID, int StageID, string URI)
        {
            try
            {
                Tasks.Preview preview = new Preview();
                preview.NameID = NameID;
                preview.StageID = StageID;
                preview.URI = URI;
                if (!_PreviewImages.ContainsKey(NameID))
                {
                    System.Collections.Generic.Dictionary<int, Tasks.Preview> dict = new Dictionary<int, Preview>();
                    dict.Add(StageID, preview);
                    _PreviewImages.Add(NameID, dict);
                }
                else
                {
                    if (!_PreviewImages[NameID].ContainsKey(StageID))
                        _PreviewImages[NameID].Add(StageID, preview);
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }


        private void setPreviewImagesForDependingHierarchy(string Identifier, IPM.TaxonSource taxonSource)
        {

        }

        //public string PreviewImage(string TaxonIdentifier)
        //{
        //    string Preview = "";
        //    if (PreviewImages.ContainsKey(TaxonIdentifier))
        //    {
        //        Preview = PreviewImages[TaxonIdentifier];
        //    }
        //    return Preview;
        //}

        public static System.Collections.Generic.Dictionary<int, Resource> TaxonResources(IPM.TaxonSource taxonSource, ResourceType resourceType, int NameID)
        {
            System.Collections.Generic.Dictionary<int, Resource> dict = new Dictionary<int, Resource>();
            if (List.TaxonDict(taxonSource).ContainsKey(NameID))
            {
                int r = 0;
                Taxon taxon = List.TaxonDict(taxonSource)[NameID];

            }
            //System.Data.DataRow[] rr = Database.ResourceTable(resourceType).Select("NameID = " + NameID.ToString());
            //int i = 0;
            //foreach (System.Data.DataRow R in rr)
            //{
            //    i++;
            //    Resource resource = new Resource();
            //    resource.Type = resourceType;
            //    if (R.Table.Columns.Contains("Title") && !R["Title"].Equals(System.DBNull.Value) && R["Title"].ToString().Length > 0)
            //        resource.Title = R["Title"].ToString();
            //    if (R.Table.Columns.Contains("Info") && !R["Info"].Equals(System.DBNull.Value) && R["Info"].ToString().Length > 0)
            //        resource.Uri = new Uri(R["Info"].ToString());
            //    if (R.Table.Columns.Contains("Notes") && !R["Notes"].Equals(System.DBNull.Value) && R["Notes"].ToString().Length > 0)
            //        resource.Notes = R["Notes"].ToString();
            //    resource.CopyRight = R["Creator"].ToString();
            //    dict.Add(i, resource);
            //}
            return dict;
        }




        public static System.Collections.Generic.Dictionary<int, Resource> TaxonResources(ResourceType resourceType, int NameID)
        {
            System.Collections.Generic.Dictionary<int, Resource> dict = new Dictionary<int, Resource>();
            System.Data.DataRow[] rr = Database.ResourceTable(resourceType).Select("NameID = " + NameID.ToString());
            int i = 0;
            foreach(System.Data.DataRow R in rr)
            {
                i++;
                Resource resource = new Resource();
                resource.Type = resourceType;
                if (R.Table.Columns.Contains("Title") && !R["Title"].Equals(System.DBNull.Value) && R["Title"].ToString().Length > 0)
                    resource.Title = R["Title"].ToString();
                if (R.Table.Columns.Contains("Info") && !R["Info"].Equals(System.DBNull.Value) && R["Info"].ToString().Length > 0)
                    resource.Uri = new Uri(R["Info"].ToString());
                if (R.Table.Columns.Contains("Notes") && !R["Notes"].Equals(System.DBNull.Value) && R["Notes"].ToString().Length > 0)
                    resource.Notes = R["Notes"].ToString();
                resource.CopyRight = R["Creator"].ToString();
                dict.Add(i, resource);
            }
            return dict;
        }

        public static System.Collections.Generic.Dictionary<int, Resource> TaxonResources(ResourceType resourceType)
        {
            if (_TaxonResources == null)
                _TaxonResources = new Dictionary<ResourceType, Dictionary<int, Resource>>();
            if (!_TaxonResources.ContainsKey(resourceType))
            {
                Dictionary<int, Resource> dict = new Dictionary<int, Resource>();
                try
                {
                    int i = 0;
                    System.Data.DataTable dt = Database.ResourceTable(resourceType);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            Resource resource = new Resource();
                            resource.Type = resourceType;
                            if (R.Table.Columns.Contains("Title") && !R["Title"].Equals(System.DBNull.Value) && R["Title"].ToString().Length > 0)
                                resource.Title = R["Title"].ToString();
                            if (R.Table.Columns.Contains("Info") && !R["Info"].Equals(System.DBNull.Value) && R["Info"].ToString().Length > 0)
                                resource.Uri = new Uri(R["Info"].ToString());
                            if (R.Table.Columns.Contains("Notes") && !R["Notes"].Equals(System.DBNull.Value) && R["Notes"].ToString().Length > 0)
                                resource.Notes = R["Notes"].ToString();
                            resource.CopyRight = R["Creator"].ToString();
                            dict.Add(i, resource);
                            i++;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                _TaxonResources.Add(resourceType, dict);
            }
            return _TaxonResources[resourceType];
        }

        private static System.Collections.Generic.Dictionary<ResourceType, System.Collections.Generic.Dictionary<int, Resource>> _TaxonResources;

    }
}
