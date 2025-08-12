using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.Tasks.Taxa
{
    public class Hierarchy
    {
        private static System.Collections.Generic.Dictionary<int, int> _TaxonHierarchy;
        public static System.Collections.Generic.Dictionary<int, int> TaxonHierarchy
        {
            get
            {
                if (_TaxonHierarchy == null)
                {
                    _TaxonHierarchy = new Dictionary<int, int>();
                    foreach (System.Data.DataRow R in Database.DtTaxa.Rows)
                    {
                        int NameID;
                        int ParentID = -1;
                        if (int.TryParse(R["NameID"].ToString(), out NameID))
                        {
                            int.TryParse(R["NameParentID"].ToString(), out ParentID);
                            if (!_TaxonHierarchy.ContainsKey(NameID))
                            {
                                _TaxonHierarchy.Add(NameID, ParentID);
                            }
                        }
                    }
                }
                return _TaxonHierarchy;
            }
        }

        public static System.Collections.Generic.List<int> Children(int ParentNameID)
        {
            System.Collections.Generic.List<int> NameIDs = new List<int>();
            foreach (System.Collections.Generic.KeyValuePair<int, int> KV in TaxonHierarchy)
            {
                if (KV.Value == ParentNameID)
                {
                    if (!NameIDs.Contains(KV.Key))
                    {
                        NameIDs.Add(KV.Key);
                    }
                }
            }
            return NameIDs;
        }

        private static System.Collections.Generic.List<int> _HierarchyRootNameIDs;
        public static System.Collections.Generic.List<int> HierarchyRootNameIDs
        {
            get
            {
                if (_HierarchyRootNameIDs == null)
                {
                    _HierarchyRootNameIDs = new List<int>();
                    foreach(System.Data.DataRow R in Database.DtHierarchyRoot().Rows)
                    {
                        if (!_HierarchyRootNameIDs.Contains(int.Parse(R[0].ToString())))
                            _HierarchyRootNameIDs.Add(int.Parse(R[0].ToString()));
                    }
                }
                return _HierarchyRootNameIDs;
            }
        }



        public static System.Collections.Generic.List<int> Children(int ParentNameID, System.Collections.Generic.List<int> NameIDs)
        {
            if (NameIDs == null) NameIDs = new List<int>();
            foreach (System.Collections.Generic.KeyValuePair<int, int> KV in TaxonHierarchy)
            {
                if (KV.Value == ParentNameID)
                {
                    if (!NameIDs.Contains(KV.Key))
                    {
                        NameIDs.Add(KV.Key);
                        foreach (int ID in Children(KV.Key, NameIDs))
                        {
                            if (!NameIDs.Contains(ID))
                                NameIDs.Add(ID);
                        }
                    }
                }
            }
            return NameIDs;
        }

        public static System.Collections.Generic.Dictionary<int, int> ChildrenDict(int ParentNameID, System.Collections.Generic.Dictionary<int, int> NameIDs)
        {
            if (NameIDs == null) NameIDs = new Dictionary<int, int>();
            foreach (System.Collections.Generic.KeyValuePair<int, int> KV in TaxonHierarchy)
            {
                if (KV.Value == ParentNameID)
                {
                    if (!NameIDs.ContainsKey(KV.Key))
                    {
                        NameIDs.Add(KV.Key, KV.Value);
                        foreach (System.Collections.Generic.KeyValuePair<int, int> kv in ChildrenDict(KV.Key, NameIDs))
                        {
                            if (!NameIDs.ContainsKey(kv.Key))
                                NameIDs.Add(kv.Key, kv.Value);
                        }
                    }
                }
            }
            return NameIDs;
        }



        public static System.Collections.Generic.List<int> Children(int ParentNameID, System.Collections.Generic.List<int> NameIDs = null, Break @break = Break.None, int? StageID = null, int? ListID = null)
        {
            if (NameIDs == null) NameIDs = new List<int>();
            foreach(System.Collections.Generic.KeyValuePair<int, int> KV in TaxonHierarchy)
            {
                switch(@break)
                {
                    case Break.Group:
                        if (ListID != null)
                        {

                        }
                        break;
                    case Break.PreviewImage:
                        break;
                }
                if (KV.Value == ParentNameID)
                {
                    if (!NameIDs.Contains(KV.Key))
                    {
                        NameIDs.Add(KV.Key);
                        foreach (int ID in Children(KV.Key, NameIDs))
                        {
                            if (!NameIDs.Contains(ID))
                                NameIDs.Add(ID);
                        }
                    }
                }
            }
            return NameIDs;
        }

        public enum Break {None, PreviewImage, Group, Resource }


    }
}
