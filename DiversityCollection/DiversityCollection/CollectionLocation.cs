using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityCollection
{
    public struct LocationNode
    {
        public int ID;
        public int? ParentID;
        public string DisplayText;
        public string Type;
        public bool HasAccess { get; set; } = true; // Indicates if the user has access to this location
        public LocationNode(int value, int? parent, string Display, string Type = "Collection", bool HasAccess = true)
        {
            ID = value;
            ParentID = parent;
            DisplayText = Display;
            this.Type = Type;
            this.HasAccess = HasAccess;
        }
    }

    public class CollectionLocation
    {
        private int _ID;
        private System.Windows.Forms.TreeView _TreeView;
        private HierarchicalEntity.HierarchyDisplayState _HierarchyDisplayState = HierarchicalEntity.HierarchyDisplayState.Show;

        public CollectionLocation(int id, System.Windows.Forms.TreeView treeView)
        {
            ID = id;
            _TreeView = treeView;
            _TreeView.Nodes.Clear();
            _TreeView.ImageList = DiversityCollection.Specimen.ImageList;
        }

        public int ID
        {
            get { return _ID; }
            set { _LocationNodes = null; _ID = value; }
        }

        /// <summary>
        /// Sets the parent of a collection location.
        /// </summary>
        /// <param name="ID">ID of the current collection</param>
        /// <param name="parentId">ID of the parent collection</param>
        /// <returns></returns>
        public bool SetParent(int ID, int? parentId)
        {
            bool OK = true;
            try
            {
                if (parentId.HasValue && parentId.Value == ID)
                {
                    throw new InvalidOperationException("Cannot set a collection as its own parent.");
                }
                string SQL = "UPDATE Collection SET LocationParentID = ";
                if (parentId.HasValue)
                    SQL += parentId.Value.ToString();
                else
                    SQL += "NULL";
                SQL += " WHERE CollectionID = " + ID.ToString();
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                _LocationNodes = null; // Reset the cached location nodes
                this._HierarchyID = -1;
                this.BuildHierarchy(ID);
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); OK = false; }
            return OK;
        }

        /// <summary>
        /// Sets the hierarchy display state for the collection locations, e.g. hide, parents etc. to adapt the tree in the interface.
        /// </summary>
        /// <param name="state">The state of the tree</param>
        public void setHierarchyDisplayState(HierarchicalEntity.HierarchyDisplayState state)
        {
            _HierarchyDisplayState = state;
            _HierarchyID = -1;
            _LocationNodes = null;
            this.BuildHierarchy(_ID);
        }

        private System.Collections.Generic.List<LocationNode> _LocationNodes;

        public System.Collections.Generic.List<LocationNode> LocationNodes
        {
            get 
            {
                if (_LocationNodes == null)
                    initLocationNodes();
                return _LocationNodes; 
            }
        }

        /// <summary>
        /// Bilds the location nodes according to the display state.
        /// </summary>
        private bool initLocationNodes(int? ID = null)
        {
            bool OK = true;
            if (_LocationNodes != null)
                return OK;
            try
            {
                // #205
                int id = ID.HasValue ? ID.Value : this._ID;

                _LocationNodes = new System.Collections.Generic.List<LocationNode>();
                string SQL = "";
                switch (_HierarchyDisplayState)
                {
                    case HierarchicalEntity.HierarchyDisplayState.Show:
                        SQL = @"SELECT L.CollectionID, L.LocationParentID, L.CollectionName, L.Type FROM dbo.[CollectionLocation](" + id.ToString() + ") AS L";
                        break;
                    case HierarchicalEntity.HierarchyDisplayState.Parents:
                        SQL = @"SELECT L.CollectionID, L.LocationParentID, L.CollectionName, L.Type FROM [dbo].[CollectionLocationSuperior] (" + id.ToString() + ") AS L ";
                        break;
                    case HierarchicalEntity.HierarchyDisplayState.Children:
                        SQL = @"SELECT L.CollectionID, L.LocationParentID, L.CollectionName, L.Type FROM [dbo].[CollectionLocationSuperior] (" + id.ToString() + ") AS L " +
                            "UNION " +
                            "SELECT L.CollectionID, L.LocationParentID, L.CollectionName, L.Type FROM [dbo].[CollectionLocationChildNodes] (" + id.ToString() + ") AS L";
                        break;
                }
                if (SQL.Length > 0)
                {
                    System.Data.DataTable dt = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        id = Convert.ToInt32(row["CollectionID"]);
                        int? parentId = row["LocationParentID"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["LocationParentID"]);
                        string displayText = row["CollectionName"].ToString();
                        string type = row["Type"].ToString();
                        bool hasAccess = DiversityCollection.Collection.ManagerHasAccessToCollection(id); 
                        _LocationNodes.Add(new LocationNode(id, parentId, displayText, type, hasAccess));
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); OK = false;
            }
            return OK;
        }

        private int _HierarchyID = -1;
        /// <summary>
        /// Builds the hierarchy of collection locations and populates the TreeView control.
        /// </summary>
        /// <param name="ID">The ID of the collection</param>
        /// <param name="ForceRedraw">If the hierarchy should be refreshed in any case even if for the same ID, e.g. when saving</param>
        public void BuildHierarchy(int ID, bool ForceRedraw = false)
        {
            // Build hierarchy only if shown
            if (!DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation)
                return;
            if ((ID < 0 || ID == _HierarchyID) && !ForceRedraw) 
                return;
            _HierarchyID = ID;
            if (ForceRedraw)
            {
                _LocationNodes = null;
                // #205
                this.initLocationNodes(ID);
            }
            if (this._TreeView.Nodes.Count > 0)
                this._TreeView.Nodes.Clear();

            if (LocationNodes.Count == 0)
                return;
            var rootNodes = _LocationNodes.Where(n => n.ParentID == null).ToList();
            foreach (var rootNode in rootNodes)
            {
                var rootTreeNode = new System.Windows.Forms.TreeNode(rootNode.DisplayText) { Tag = rootNode }; //{ Tag = rootNode.ID };
                rootTreeNode.ImageIndex = DiversityCollection.Specimen.CollectionTypeImage(rootNode.Type, !rootNode.HasAccess); // Use a method to get the image index based on type
                rootTreeNode.SelectedImageIndex = DiversityCollection.Specimen.CollectionTypeImage(rootNode.Type, !rootNode.HasAccess);
                this._TreeView.Nodes.Add(rootTreeNode);
                AddChildNodes(rootTreeNode, rootNode.ID);
            }
            this._TreeView.ExpandAll();
        }

        /// <summary>
        /// Adds child nodes to the specified parent node in the TreeView.
        /// </summary>  
        private void AddChildNodes(System.Windows.Forms.TreeNode parentNode, int parentId)
        {
            var childNodes = _LocationNodes.Where(n => n.ParentID == parentId).ToList();
            foreach (var childNode in childNodes)
            {
                var childTreeNode = new System.Windows.Forms.TreeNode(childNode.DisplayText) { Tag = childNode }; //{ Tag = childNode.ID };
                childTreeNode.ImageIndex = DiversityCollection.Specimen.CollectionTypeImage(childNode.Type, !childNode.HasAccess); // Use a method to get the image index based on type
                childTreeNode.SelectedImageIndex = DiversityCollection.Specimen.CollectionTypeImage(childNode.Type, !childNode.HasAccess);
                parentNode.Nodes.Add(childTreeNode);
                AddChildNodes(childTreeNode, childNode.ID);
            }
        }

        public void collapseTreeView()
        {
            if (_TreeView.Nodes.Count > 0)
            {
                _TreeView.CollapseAll();
            }
        }

        public void expandTreeView()
        {
            if (_TreeView.Nodes.Count > 0)
            {
                _TreeView.ExpandAll();
            }
        }

    }
}
