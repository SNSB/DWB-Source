using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection
{
    class CollectionTask : HierarchicalEntity
    {
        #region Parameter

        public static string sqlFieldCollectionTaskImages = "CollectionTaskID, URI, ImageType, Notes, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear,  DataWithholdingReason";
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterCollectionTaskImages;
        private System.Windows.Forms.ListBox _ListBoxImages;

        public System.Windows.Forms.ListBox ListBoxImages
        {
            get { return _ListBoxImages; }
            set { _ListBoxImages = value; }
        }

        private System.Windows.Forms.ImageList _imageListCollectionImages;

        public System.Windows.Forms.ImageList ImageListCollectionImages
        {
            get { return _imageListCollectionImages; }
            set { _imageListCollectionImages = value; }
        }

        private DiversityWorkbench.UserControls.UserControlImage _userControlImageCollectionImage;

        public DiversityWorkbench.UserControls.UserControlImage UserControlImageCollectionImage
        {
            get { return _userControlImageCollectionImage; }
            set { _userControlImageCollectionImage = value; }
        }

        //private System.Windows.Forms.Button _buttonHeaderShowCollectionImage;

        //public System.Windows.Forms.Button ButtonHeaderShowCollectionImage
        //{
        //    get { return _buttonHeaderShowCollectionImage; }
        //    set { _buttonHeaderShowCollectionImage = value; }
        //}

        private System.Windows.Forms.ToolStripMenuItem _imageToolStripItem;
        public System.Windows.Forms.ToolStripMenuItem ImageToolStripItem
        {
            get { return _imageToolStripItem; }
            set { _imageToolStripItem = value; }
        }


        private System.Windows.Forms.SplitContainer _splitContainerDataAndImages;

        public System.Windows.Forms.SplitContainer SplitContainerDataAndImages
        {
            get { return _splitContainerDataAndImages; }
            set { _splitContainerDataAndImages = value; }
        }

        private System.Windows.Forms.SplitContainer _splitContainerImagesAndLabel;

        public System.Windows.Forms.SplitContainer SplitContainerImagesAndLabel
        {
            get { return _splitContainerImagesAndLabel; }
            set { _splitContainerImagesAndLabel = value; }
        }

        private System.Windows.Forms.Label _LabelHeader;

        public System.Windows.Forms.Label LabelHeader
        {
            get { return _LabelHeader; }
            set { _LabelHeader = value; }
        }

        private QRcodeSource _QRsource;


        #endregion

        #region Construction

        public CollectionTask(
            ref System.Data.DataSet Dataset,
            System.Data.DataTable DataTable,
            ref System.Windows.Forms.TreeView TreeView,
            System.Windows.Forms.Form Form,
            DiversityWorkbench.UserControls.UserControlQueryList UserControlQueryList,
            System.Windows.Forms.SplitContainer SplitContainerMain,
            System.Windows.Forms.SplitContainer SplitContainerData,
            System.Windows.Forms.HelpProvider HelpProvider,
            System.Windows.Forms.ToolTip ToolTip,
            ref System.Windows.Forms.BindingSource BindingSource)
            : base(ref Dataset, DataTable, ref TreeView, Form, UserControlQueryList, SplitContainerMain,
            SplitContainerData, null, null,
            HelpProvider, ToolTip, ref BindingSource, DiversityCollection.LookupTable.DtCollectionTaskHierarchy, DiversityCollection.LookupTable.DtCollectionTaskHierarchy)
        {
            this._sqlItemFieldList = " CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, DisplayOrder, DisplayText, CollectionSpecimenID, SpecimenPartID, ModuleUri, TaskStart, TaskEnd, Result, URI, NumberValue, BoolValue, Description, Notes ";
            this._MainTable = "CollectionTask";
        }

        #endregion

        #region Datahandling

        public override void fillDependentTables(int ID)
        {
            this._DataSet.Tables["CollectionTaskImage"].Clear();

            if (this._DataSet == null) this._DataSet = new System.Data.DataSet();

            // Images
            string SQL = "SELECT " + sqlFieldCollectionTaskImages +
                " FROM CollectionTaskImage " +
                "WHERE CollectionTaskID = " + ID.ToString();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            this.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterCollectionTaskImages, SQL, this._DataSet.Tables["CollectionTaskImage"]);

            int TaskID;
            SQL = "SELECT TaskID FROM CollectionTask WHERE CollectionTaskID = " + ID.ToString();
            if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out TaskID))
                _TaskID = TaskID;
            else
                _TaskID = null;
            // #mehrfachaufruf
            //this.setFormControls();
            try
            {
                this._ListBoxImages.SelectedIndex = -1;
                if (this._DataSet.Tables["CollectionTaskImage"].Rows.Count > 0 && this._ListBoxImages.Items.Count > 0)
                    this._ListBoxImages.SelectedIndex = 0;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private int? _TaskID;
        public int? TaskID { get { return _TaskID; } }

        public override bool deleteDependentData(int ID)
        {
            try
            {
                // Images
                string SQL = "DELETE I FROM CollectionTaskImage I " +
                    "WHERE I.CollectionTaskID = " + ID.ToString();
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);

                // Tasks
                SQL = "DELETE T FROM CollectionTaskImage T " +
                    "WHERE T.CollectionTaskID = " + ID.ToString();
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override void setFormControls()
        {
            this.ItemChanged();

            if (this._ListBoxImages != null
                && this._imageListCollectionImages != null
                && this._userControlImageCollectionImage != null)
            {
                this._ListBoxImages.Items.Clear();
                if (this._ListBoxImages.Visible)
                {
                    this.FormFunctions.FillImageList(this._ListBoxImages, this._imageListCollectionImages,
                        this._DataSet.Tables["CollectionTaskImage"], "URI", this._userControlImageCollectionImage);
                }
            }
            try
            {
                // setting the button showing images
                if (this._splitContainerDataAndImages.Panel1Collapsed) // neither is shown
                {
                    this._imageToolStripItem.Image = DiversityCollection.Resource.IconesGrey;
                    this._imageToolStripItem.BackColor = System.Drawing.SystemColors.Control;
                }
                else
                {
                    // The backcolor for the printer button is set directly
                    if (this._splitContainerImagesAndLabel.Panel1Collapsed) // The printer is shown
                    {
                        this._imageToolStripItem.Image = DiversityCollection.Resource.IconesGrey;
                    }
                    else // the images are shown
                    {
                        this._imageToolStripItem.Image = DiversityCollection.Resource.Icones;
                        this._imageToolStripItem.BackColor = System.Drawing.Color.Red;
                    }
                }
                if (this._splitContainerImagesAndLabel.Panel1Collapsed)
                {
                    if (this._DataSet.Tables["CollectionTaskImage"].Rows.Count > 0)
                        this._imageToolStripItem.BackColor = System.Drawing.Color.Yellow;
                    else
                        this._imageToolStripItem.BackColor = System.Drawing.SystemColors.Control;
                }
            }
            catch { }
            try
            {
                if (this._DataTable.Rows.Count > 0)
                {
                    string DisplayText = this._DataTable.Rows[0]["DisplayText"].ToString();
                    System.Data.DataRow[] RR = this._DataTable.Select("CollectionTaskID = " + this.ID.ToString());
                    if (RR.Length > 0)
                        DisplayText = RR[0]["DisplayText"].ToString();
                    this._LabelHeader.Text = DisplayText;
                }
                else
                {
                    this._LabelHeader.Text = "";
                }
            }
            catch { }
            this.ItemChanged();
        }

        public override void saveDependentTables()
        {
            this.FormFunctions.updateTable(this._DataSet, "CollectionTaskImage", this._SqlDataAdapterCollectionTaskImages, this._BindingSource);
        }

        #endregion

        #region Interface

        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions
        {
            get
            {
                System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();
                string Description = "";
                string SQL = "";

                #region Task

                Description = this.FormFunctions.ColumnDescription("CollectionTask", "DisplayText");
                DiversityWorkbench.QueryCondition qDisplayText = new DiversityWorkbench.QueryCondition(true, "CollectionTask", "CollectionTaskID", "DisplayText", "CollectionTask", "DisplayText", "DisplayText", Description);
                QueryConditions.Add(qDisplayText);

                Description = this.FormFunctions.ColumnDescription("CollectionTask", "Result");
                DiversityWorkbench.QueryCondition q0 = new DiversityWorkbench.QueryCondition(true, "CollectionTask", "CollectionTaskID", "Result", "CollectionTask", "Result", "Result", Description);
                QueryConditions.Add(q0);

                Description = this.FormFunctions.ColumnDescription("CollectionTask", "Description");
                DiversityWorkbench.QueryCondition q1 = new DiversityWorkbench.QueryCondition(true, "CollectionTask", "CollectionTaskID", "Description", "CollectionTask", "Description", "Description", Description);
                QueryConditions.Add(q1);

                Description = this.FormFunctions.ColumnDescription("CollectionTask", "Notes");
                DiversityWorkbench.QueryCondition q2 = new DiversityWorkbench.QueryCondition(true, "CollectionTask", "CollectionTaskID", "Notes", "CollectionTask", "Notes", "Notes", Description);
                QueryConditions.Add(q2);

                Description = this.FormFunctions.ColumnDescription("CollectionTask", "URI");
                DiversityWorkbench.QueryCondition q3 = new DiversityWorkbench.QueryCondition(true, "CollectionTask", "CollectionTaskID", "URI", "CollectionTask", "URI", "URI", Description);
                QueryConditions.Add(q3);

                Description = this.FormFunctions.ColumnDescription("CollectionTask", "CollectionTaskID");
                DiversityWorkbench.QueryCondition q4 = new DiversityWorkbench.QueryCondition(true, "CollectionTask", "CollectionTaskID", "CollectionTaskID", "CollectionTask", "ID", "ID", Description);
                QueryConditions.Add(q4);

                #endregion

                #region Image

                Description = "If any image is present";
                DiversityWorkbench.QueryCondition qImagePresent = new DiversityWorkbench.QueryCondition(false, "CollectionTaskImage", "CollectionTaskID", "Image", "Presence", "Image present", Description, DiversityWorkbench.QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qImagePresent);

                Description = this.FormFunctions.ColumnDescription("CollectionTaskImage", "URI");
                DiversityWorkbench.QueryCondition qURI = new DiversityWorkbench.QueryCondition(false, "CollectionTaskImage", "CollectionTaskID", "URI", "Image", "URI", "URI", Description);
                QueryConditions.Add(qURI);

                Description = this.FormFunctions.ColumnDescription("CollectionTaskImage", "ImageType");
                DiversityWorkbench.QueryCondition qImageType = new DiversityWorkbench.QueryCondition(false, "CollectionTaskImage", "CollectionTaskID", "ImageType", "Image", "Type", "Image type", Description);
                QueryConditions.Add(qImageType);

                Description = this.FormFunctions.ColumnDescription("CollectionTaskImage", "Notes");
                DiversityWorkbench.QueryCondition qImageNotes = new DiversityWorkbench.QueryCondition(false, "CollectionTaskImage", "CollectionTaskID", "Notes", "Image", "Notes", "Notes", Description);
                QueryConditions.Add(qImageNotes);

                #endregion

                return QueryConditions;
            }
        }

        public override DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns
        {
            get
            {
                DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[6];
                QueryDisplayColumns[0].DisplayText = "Task";
                QueryDisplayColumns[0].DisplayColumn = "Task";
                QueryDisplayColumns[0].OrderColumn = "Task";
                QueryDisplayColumns[0].IdentityColumn = "CollectionTaskID";
                QueryDisplayColumns[0].TableName = "CollectionTaskHierarchyAll()";

                QueryDisplayColumns[1].DisplayText = "Hierarchy";
                QueryDisplayColumns[1].DisplayColumn = "HierarchyDisplayText";
                QueryDisplayColumns[1].OrderColumn = "HierarchyDisplayText";
                QueryDisplayColumns[1].IdentityColumn = "CollectionTaskID";
                QueryDisplayColumns[1].TableName = "CollectionTaskHierarchyAll()";

                QueryDisplayColumns[2].DisplayText = "Description";
                QueryDisplayColumns[2].DisplayColumn = "Description";
                QueryDisplayColumns[2].OrderColumn = "Description";
                QueryDisplayColumns[2].IdentityColumn = "CollectionTaskID";
                QueryDisplayColumns[2].TableName = "CollectionTaskHierarchyAll()";

                QueryDisplayColumns[3].DisplayText = "Start";
                QueryDisplayColumns[3].DisplayColumn = "TaskStart";
                QueryDisplayColumns[3].OrderColumn = "TaskStart";
                QueryDisplayColumns[3].IdentityColumn = "CollectionTaskID";
                QueryDisplayColumns[3].TableName = "CollectionTaskHierarchyAll()";

                QueryDisplayColumns[4].DisplayText = "Result";
                QueryDisplayColumns[4].DisplayColumn = "Result";
                QueryDisplayColumns[4].OrderColumn = "Result";
                QueryDisplayColumns[4].IdentityColumn = "CollectionTaskID";
                QueryDisplayColumns[4].TableName = "CollectionTaskHierarchyAll()";

                QueryDisplayColumns[5].DisplayText = "Collection";
                QueryDisplayColumns[5].DisplayColumn = "CollectionHierarchyDisplayText";
                QueryDisplayColumns[5].OrderColumn = "CollectionHierarchyDisplayText";
                QueryDisplayColumns[5].IdentityColumn = "CollectionTaskID";
                QueryDisplayColumns[5].TableName = "CollectionTaskHierarchyAll()";

                return QueryDisplayColumns;
            }
        }

        public override Dictionary<string, string> AdditionalNotNullColumnsForNewItem(int? ParentID)
        {
            Dictionary<string, string> Dict = new Dictionary<string, string>();
            if (ParentID == null)
            {
                DiversityWorkbench.Forms.FormGetItemFromHierarchy fromHierarchy = new DiversityWorkbench.Forms.FormGetItemFromHierarchy(DiversityCollection.LookupTable.DtCollectionWithHierarchy, "CollectionID", "CollectionParentID", "DisplayText", "CollectionID", "Please select a collection", "Collection");
                fromHierarchy.ShowDialog();
                if (fromHierarchy.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    int CollectionID;
                    if (int.TryParse(fromHierarchy.SelectedValue, out CollectionID))
                    {
                        Dict.Add("CollectionID", CollectionID.ToString());
                    }
                }
            }
            System.Data.DataTable dtTasks = this.DtTasks(ParentID);
            //System.Data.DataTable dtTasks = new System.Data.DataTable();
            //if (ParentID != null)
            //{
            //    string SqlTask = "SELECT T.[TaskID], T.HierarchyDisplayTextInvers AS DisplayText " +
            //        "FROM [Task] P inner join dbo.TaskHierarchyAll() T on P.TaskID = T.TaskParentID INNER JOIN CollectionTask C ON C.TaskID = P.TaskID AND C.CollectionTaskID = " + ParentID.ToString() +
            //        " WHERE T.DescriptionType <> '' OR T.ModuleType <> '' OR T.NumberType <> '' OR T.NotesType <> '' OR T.ResultType <> '' OR T.BoolType <> '' OR T.DateType <> '' OR T.UriType <> '' OR T.ModuleTitle <> ''" +
            //        " union " +
            //        "SELECT B.[TaskID], B.[HierarchyDisplayTextInvers] " +
            //        "FROM [Task] P inner join Task T on P.TaskID = T.TaskParentID  INNER JOIN CollectionTask C ON C.TaskID = P.TaskID AND C.CollectionTaskID = " + ParentID.ToString() +
            //        " inner join dbo.TaskHierarchyAll() B on T.Type = B.Type and B.DisplayText = T.DisplayText and B.TaskParentID is null ";
            //   DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SqlTask, ref dtTasks);
            //}
            //else
            //    dtTasks = DiversityCollection.LookupTable.DtTask;
            if (dtTasks.Rows.Count == 1)
            {
                int TaskID;
                if (int.TryParse(dtTasks.Rows[0][0].ToString(), out TaskID))
                {
                    Dict.Add("TaskID", TaskID.ToString());
                    Dict.Add("DisplayText", dtTasks.Rows[0][1].ToString());
                }
            }
            else
            {
                DiversityWorkbench.Forms.FormGetStringFromList formTask = new DiversityWorkbench.Forms.FormGetStringFromList(dtTasks, "DisplayText", "TaskID", "Task", "Please select a task", "", false, true, true, DiversityCollection.Resource.Task);
                formTask.ShowDialog();
                if (formTask.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    int TaskID;
                    if (int.TryParse(formTask.SelectedValue, out TaskID))
                    {
                        Dict.Add("TaskID", TaskID.ToString());
                        Dict.Add("DisplayText", formTask.SelectedString);
                    }
                }
            }
            return Dict;
        }

        public int? PresetTaskID = null;

        public System.Data.DataTable DtTasks(int? ParentID = null)
        {
            System.Data.DataTable dtTasks = new System.Data.DataTable();
            try
            {
                if (PresetTaskID != null)
                {
                    string SqlTask = "SELECT T.[TaskID], T.HierarchyDisplayTextInvers AS DisplayText " +
                        "FROM dbo.TaskHierarchyAll() T WHERE T.TaskID = " + PresetTaskID.ToString();
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SqlTask, ref dtTasks);
                }
                else if (ParentID != null)
                {
                    string SqlTask = "SELECT T.[TaskID], T.HierarchyDisplayTextInvers AS DisplayText " +
                        "FROM [Task] P inner join dbo.TaskHierarchyAll() T on P.TaskID = T.TaskParentID INNER JOIN CollectionTask C ON C.TaskID = P.TaskID AND C.CollectionTaskID = " + ParentID.ToString() +
                        " WHERE T.DescriptionType <> '' OR T.ModuleType <> '' OR T.NumberType <> '' OR T.NotesType <> '' OR T.ResultType <> '' OR T.BoolType <> '' OR T.DateType <> '' OR T.UriType <> '' OR T.ModuleTitle <> ''" +
                        " union " +
                        "SELECT B.[TaskID], B.[HierarchyDisplayTextInvers] " +
                        "FROM [Task] P inner join Task T on P.TaskID = T.TaskParentID  INNER JOIN CollectionTask C ON C.TaskID = P.TaskID AND C.CollectionTaskID = " + ParentID.ToString() +
                        " inner join dbo.TaskHierarchyAll() B on T.Type = B.Type and B.DisplayText = T.DisplayText and B.TaskParentID is null ";
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SqlTask, ref dtTasks);
                }
                else
                    dtTasks = DiversityCollection.LookupTable.DtTask;

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return dtTasks;
        }

        public override string HierarchyNodeText(System.Data.DataRow R)
        {
            return (CollectionTaskDisplay.NodeText(R, this._IncludeIDinTreeview));

            #region local
            string CollectionName = "";
            string Task = "";
            string Type = "";
            DiversityCollection.Task.TaskModuleType TaskModuleType = DiversityCollection.Task.TaskModuleType.None;
            DiversityCollection.Task.TaskResultType TaskResultType = DiversityCollection.Task.TaskResultType.None;
            DiversityCollection.Task.TaskDateType TaskDateType = DiversityCollection.Task.TaskDateType.None;
            string DisplayText = "";
            string Result = "";
            string ID = "";
            string Spacer = " ";
            string Start = "";
            string End = "";
            string SQL = "";
            string Parent = "";

            // DisplayText
            DisplayText = R[this.ColumnDisplayText].ToString();

            // Task
            if (!R["TaskID"].Equals(System.DBNull.Value))
            {
                string SqlTask = "SELECT DisplayText FROM Task WHERE TaskID = " + R["TaskID"].ToString();
                Task = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlTask);
                SqlTask = "SELECT [Type] FROM Task WHERE TaskID = " + R["TaskID"].ToString();
                Type = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlTask);
                TaskModuleType = DiversityCollection.Task.TypeOfTaskModule(Type);
                string SqlParent = "SELECT P.DisplayText From Task P INNER JOIN Task T ON T.TaskParentID = P.TaskID AND NOT T.TaskParentID IS NULL AND T.TaskID = " + R["TaskID"].ToString();
                string Message = "";
                Parent = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlParent, ref Message);
            }

            // ID
            if (this._IncludeIDinTreeview)
                ID = "[" + R[this.ColumnID].ToString() + "]   ";

            // Collection
            if (R[this.ColumnParentID].Equals(System.DBNull.Value))
            {
                if (!R["CollectionID"].Equals(System.DBNull.Value))
                {
                    SQL = "SELECT CollectionName FROM Collection WHERE CollectionID = " + R["CollectionID"].ToString();
                    CollectionName = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL) + " - ";
                }
            }
            else
            {
                SQL = "SELECT CASE WHEN T.CollectionID = P.CollectionID THEN '' ELSE C.CollectionName END AS CollectionName " +
                    "FROM CollectionTask AS T INNER JOIN " +
                    "Collection AS C ON T.CollectionID = C.CollectionID INNER JOIN " +
                    "CollectionTask AS P ON T.CollectionTaskParentID = P.CollectionTaskID " +
                    "WHERE T.CollectionTaskID = " + R[this.ColumnID].ToString();
                CollectionName = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (CollectionName.Length > 0)
                    CollectionName += " - ";
            }

            // Start
            string DateType = DiversityCollection.LookupTable.TaskDateType(int.Parse(R["TaskID"].ToString()));
            if (DateType.Length > 0)
            {
                TaskDateType = DiversityCollection.Task.TypeOfTaskDate(DateType);
            }

            if (!R["TaskStart"].Equals(System.DBNull.Value) && R["TaskStart"].ToString().Length > 0)
            {
                System.DateTime DT;
                if(System.DateTime.TryParse(R["TaskStart"].ToString(), out DT))
                {
                    switch(TaskDateType)
                    {
                        case DiversityCollection.Task.TaskDateType.DateFromTo:
                        case DiversityCollection.Task.TaskDateType.Date:
                            Start = DT.ToString("yyyy-MM-dd");
                            break;
                        case DiversityCollection.Task.TaskDateType.TimeFromTo:
                        case DiversityCollection.Task.TaskDateType.Time:
                            Start = DT.ToString("HH:mm");
                            break;
                        default:
                            Start = DT.ToString("yyyy-MM-dd HH:mm");
                            break;
                    }
                }
            }

            // End
            if (!R["TaskEnd"].Equals(System.DBNull.Value) && R["TaskEnd"].ToString().Length > 0)
            {
                System.DateTime DT;
                if (System.DateTime.TryParse(R["TaskEnd"].ToString(), out DT))
                {
                    switch (TaskDateType)
                    {
                        case DiversityCollection.Task.TaskDateType.DateFromTo:
                        case DiversityCollection.Task.TaskDateType.Date:
                            End = DT.ToString("yyyy-MM-dd");
                            break;
                        case DiversityCollection.Task.TaskDateType.TimeFromTo:
                        case DiversityCollection.Task.TaskDateType.Time:
                            End = DT.ToString("HH:mm");
                            break;
                        default:
                            End = DT.ToString("yyyy-MM-dd HH:mm");
                            break;
                    }
                }
            }


            // Result
            if (!R["Result"].Equals(System.DBNull.Value) && R["Result"].ToString().Length > 0)
            {
                Result = R["Result"].ToString();
                if (Result.Length > 50)
                    Result = Result.Substring(0, 50) + "...";
            }

            string NodeText = ID + CollectionName; // + Task;
            if (DisplayText != Task)
            {
                if (DisplayText.Length > 0)
                    NodeText += DisplayText + " (" + Task + ")";
                else if (Parent.Length > 0)
                    NodeText += Task + " (" + Parent + ")";
                else
                    NodeText += Task;
            }
            else
            {
                NodeText += DisplayText;
            }

            if (Start.Length > 0)
            {
                NodeText += " " + Start;
                if (End.Length > 0)
                    NodeText += " - " + End;
            }
            else if (End.Length > 0)
                NodeText += " " + End;

            if (Result.Length > 0)
                NodeText += ": " + Result;

            // Spacer
            if (R[this.ColumnParentID].Equals(System.DBNull.Value))
            {
                for (int i = 0; i < (int)NodeText.Length / 2; i++)
                    Spacer += " ";
                NodeText += Spacer;
            }

            return NodeText;
            #endregion
        }

        public override void markHierarchyNodes()
        {
            //base.markHierarchyNodes();
            try
            {
                if (this._TreeView.ImageList == null)
                    this._TreeView.ImageList = DiversityCollection.Specimen.ImageList;//.Task.ImageList();

                foreach (System.Windows.Forms.TreeNode N in this._TreeView.Nodes)
                {
                    this.markHierarchyNode(N);
                    this.markHierachyChildNodes(N);
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void markHierachyChildNodes(System.Windows.Forms.TreeNode N)
        {
            this.markHierarchyNode(N);
            foreach (System.Windows.Forms.TreeNode NC in N.Nodes)
                this.markHierachyChildNodes(NC);
        }

        private void markHierarchyNode(System.Windows.Forms.TreeNode N)
        {
            if (N.Tag != null && (N.Tag.GetType().BaseType == typeof(System.Data.DataRow) || N.Tag.GetType() == typeof(System.Data.DataRow)))
            {
                System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                N.ImageIndex = DiversityCollection.Specimen.ImageIndex(R, false);
                N.SelectedImageIndex = N.ImageIndex;
            }
        }


        public override void ItemChanged()
        {
            if (this._formCollectionTask != null)
                this._formCollectionTask.SetFormAccordingToItem();
        }

        private DiversityCollection.Forms.FormCollectionTask _formCollectionTask;
        public void setFormCollectionTask(DiversityCollection.Forms.FormCollectionTask f)
        {
            this._formCollectionTask = f;
        }

        #endregion

        #region static

        //public static string NodeText(System.Data.DataRow R, bool IncludeIDinTreeview = false)
        //{
        //    string NodeText = "";
        //    try
        //    {
        //        string CollectionName = "";
        //        string Task = "";
        //        string Type = "";
        //        DiversityCollection.Task.TaskModuleType TaskModuleType = DiversityCollection.Task.TaskModuleType.None;
        //        DiversityCollection.Task.TaskDateType TaskDateType = DiversityCollection.Task.TaskDateType.None;
        //        string DisplayText = "";
        //        string Result = "";
        //        string ID = "";
        //        string Spacer = " ";
        //        string Start = "";
        //        string End = "";
        //        string SQL = "";
        //        string Parent = "";

        //        // DisplayText
        //        DisplayText = R["DisplayText"].ToString();

        //        // Task
        //        if (!R["TaskID"].Equals(System.DBNull.Value))
        //        {
        //            string SqlTask = "SELECT DisplayText FROM Task WHERE TaskID = " + R["TaskID"].ToString();
        //            Task = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlTask);
        //            SqlTask = "SELECT [Type] FROM Task WHERE TaskID = " + R["TaskID"].ToString();
        //            Type = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlTask);
        //            TaskModuleType = DiversityCollection.Task.TypeOfTaskModule(Type);
        //            string SqlParent = "SELECT P.DisplayText From Task P INNER JOIN Task T ON T.TaskParentID = P.TaskID AND NOT T.TaskParentID IS NULL AND T.TaskID = " + R["TaskID"].ToString();
        //            string Message = "";
        //            Parent = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlParent, ref Message);
        //        }

        //        // ID
        //        if (IncludeIDinTreeview)
        //            ID = "[" + R["CollectionTaskID"].ToString() + "]   ";

        //        // Collection
        //        if (R["CollectionTaskParentID"].Equals(System.DBNull.Value))
        //        {
        //            if (!R["CollectionID"].Equals(System.DBNull.Value))
        //            {
        //                SQL = "SELECT CollectionName FROM Collection WHERE CollectionID = " + R["CollectionID"].ToString();
        //                CollectionName = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL) + " - ";
        //            }
        //        }
        //        else
        //        {
        //            SQL = "SELECT CASE WHEN T.CollectionID = P.CollectionID THEN '' ELSE C.CollectionName END AS CollectionName " +
        //                "FROM CollectionTask AS T INNER JOIN " +
        //                "Collection AS C ON T.CollectionID = C.CollectionID INNER JOIN " +
        //                "CollectionTask AS P ON T.CollectionTaskParentID = P.CollectionTaskID " +
        //                "WHERE T.CollectionTaskID = " + R["CollectionTaskID"].ToString();
        //            CollectionName = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
        //            if (CollectionName.Length > 0)
        //                CollectionName += " - ";
        //        }

        //        // Start
        //        string DateType = DiversityCollection.LookupTable.TaskDateType(int.Parse(R["TaskID"].ToString()));
        //        if (DateType.Length > 0)
        //        {
        //            TaskDateType = DiversityCollection.Task.TypeOfTaskDate(DateType);
        //        }

        //        if (!R["TaskStart"].Equals(System.DBNull.Value) && R["TaskStart"].ToString().Length > 0)
        //        {
        //            System.DateTime DT;
        //            if (System.DateTime.TryParse(R["TaskStart"].ToString(), out DT))
        //            {
        //                switch (TaskDateType)
        //                {
        //                    case DiversityCollection.Task.TaskDateType.DateFromTo:
        //                    case DiversityCollection.Task.TaskDateType.Date:
        //                        Start = DT.ToString("yyyy-MM-dd");
        //                        break;
        //                    case DiversityCollection.Task.TaskDateType.TimeFromTo:
        //                    case DiversityCollection.Task.TaskDateType.Time:
        //                        Start = DT.ToString("HH:mm");
        //                        break;
        //                    default:
        //                        Start = DT.ToString("yyyy-MM-dd HH:mm");
        //                        break;
        //                }
        //            }
        //        }

        //        // End
        //        if (!R["TaskEnd"].Equals(System.DBNull.Value) && R["TaskEnd"].ToString().Length > 0)
        //        {
        //            System.DateTime DT;
        //            if (System.DateTime.TryParse(R["TaskEnd"].ToString(), out DT))
        //            {
        //                switch (TaskDateType)
        //                {
        //                    case DiversityCollection.Task.TaskDateType.DateFromTo:
        //                    case DiversityCollection.Task.TaskDateType.Date:
        //                        End = DT.ToString("yyyy-MM-dd");
        //                        break;
        //                    case DiversityCollection.Task.TaskDateType.TimeFromTo:
        //                    case DiversityCollection.Task.TaskDateType.Time:
        //                        End = DT.ToString("HH:mm");
        //                        break;
        //                    default:
        //                        End = DT.ToString("yyyy-MM-dd HH:mm");
        //                        break;
        //                }
        //            }
        //        }


        //        // Result
        //        if (!R["Result"].Equals(System.DBNull.Value) && R["Result"].ToString().Length > 0)
        //        {
        //            Result = R["Result"].ToString();
        //            if (Result.Length > 50)
        //                Result = Result.Substring(0, 50) + "...";
        //        }

        //        NodeText = ID + CollectionName; // + Task;
        //        if (DisplayText != Task)
        //        {
        //            if (DisplayText.Length > 0)
        //                NodeText += DisplayText + " (" + Task + ")";
        //            else if (Parent.Length > 0)
        //                NodeText += Task + " (" + Parent + ")";
        //            else
        //                NodeText += Task;
        //        }
        //        else
        //        {
        //            NodeText += DisplayText;
        //        }

        //        if (Start.Length > 0)
        //        {
        //            NodeText += " " + Start;
        //            if (End.Length > 0)
        //                NodeText += " - " + End;
        //        }
        //        else if (End.Length > 0)
        //            NodeText += " " + End;

        //        if (Result.Length > 0)
        //            NodeText += ": " + Result;

        //        // Spacer
        //        if (R["CollectionTaskParentID"].Equals(System.DBNull.Value))
        //        {
        //            for (int i = 0; i < (int)NodeText.Length / 2; i++)
        //                Spacer += " ";
        //            NodeText += Spacer;
        //        }
        //    }
        //    catch(System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return NodeText;
        //}

        #endregion

        #region XML Report

        public enum QRcodeSource { None, StableIdentifier, GUID }
        private System.IO.FileInfo _XmlFile;
        private System.IO.FileInfo _XslFile;
        public string CreateXmlReport(int CollectionTaskID, string SchemaFile, QRcodeSource QRcode = QRcodeSource.None)
        {
#if !DEBUG
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            return "";
#endif
            System.Xml.XmlWriter W = null;
            try
            {
                System.IO.FileInfo FTemplate = new System.IO.FileInfo(Folder.Report() + "\\Schemas\\Templates\\DefaultTemplate.xslt");
                bool TemplateAvailable = FTemplate.Exists;
                this._XmlFile = new System.IO.FileInfo(Folder.Report() + "Task.XML");
                if (SchemaFile.Length > 0)
                    _XslFile = new System.IO.FileInfo(SchemaFile);
                this._QRsource = QRcode;
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                settings.Encoding = System.Text.Encoding.UTF8;
                W = System.Xml.XmlWriter.Create(this._XmlFile.FullName, settings);
                W.WriteStartDocument();
                W.WriteStartElement("Report");
                W.WriteElementString("User", DiversityCollection.LookupTable.CurrentUser);
                string Date = System.DateTime.Now.Year.ToString() + "/" + System.DateTime.Now.Month.ToString() + "/" + System.DateTime.Now.Day.ToString();
                W.WriteElementString("Date", Date);
                this.writeXmlCollectionTask(ref W, CollectionTaskID);
                W.WriteFullEndElement();  // Report
                W.WriteEndDocument();
                W.Flush();
                W.Close();
                W = null;
                if (this._XslFile != null && this._XslFile.Exists)
                {
                    System.Xml.Xsl.XslCompiledTransform XSLT = new System.Xml.Xsl.XslCompiledTransform();
                    XSLT.Load(this._XslFile.FullName);

                    // Load the file to transform.
                    System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(this._XmlFile.FullName);

                    // The output file:
                    string OutputFile = this._XmlFile.FullName.Substring(0, this._XmlFile.FullName.Length
                        - this._XmlFile.Extension.Length) + ".htm";

                    // Create the writer.             
                    System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(OutputFile, XSLT.OutputSettings);

                    // Transform the file and send the output to the console.
                    XSLT.Transform(doc, writer);
                    writer.Close();
                    return OutputFile;
                }

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                if (W != null)
                {
                    W.Flush();
                    W.Close();
                }
            }

            return this._XmlFile.FullName;
        }


        private void writeXmlCollectionTask(ref System.Xml.XmlWriter W, int CollectionTaskID)
        {
            // getting the date type
            string SQL = "SELECT T.DateType " +
                "FROM CollectionTask AS CT " +
                "INNER JOIN Task AS T ON CT.TaskID = T.TaskID " +
                "WHERE CT.DisplayOrder > 0 AND CT.CollectionTaskID = " + CollectionTaskID.ToString();
            string DateType = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            Task.TaskDateType taskDateType = Task.TypeOfTaskDate(DateType);
            string SqlDate = "";
            switch(taskDateType)
            {
                case Task.TaskDateType.Date:
                    SqlDate = "CONVERT(varchar(10), CT.TaskStart, 120) AS TaskStart, ";
                    break;
                case Task.TaskDateType.DateAndTime:
                    SqlDate = "CONVERT(varchar(20), CT.TaskStart, 120) AS TaskStart ";
                    break;
                case Task.TaskDateType.DateAndTimeFromTo:
                    SqlDate = "CONVERT(varchar(20), CT.TaskStart, 120) AS TaskStart, CONVERT(varchar(20), CT.TaskEnd, 120) AS TaskEnd, ";
                    break;
                case Task.TaskDateType.DateFromTo:
                    SqlDate = "CONVERT(varchar(10), CT.TaskStart, 120) AS TaskStart, CONVERT(varchar(10), CT.TaskEnd, 120) AS TaskEnd, ";
                    break;
                case Task.TaskDateType.Time:
                    SqlDate = "SUBSTRING(CONVERT(varchar(20), CT.TaskStart, 120), 12, 8) AS TaskStart, ";
                    break;
                case Task.TaskDateType.TimeFromTo:
                    SqlDate = "SUBSTRING(CONVERT(varchar(20), CT.TaskStart, 120), 12, 8) AS TaskStart, SUBSTRING(CONVERT(varchar(20), CT.TaskEnd, 120), 12, 8) AS TaskEnd, ";
                    break;
            }
            // getting the data
            System.Data.DataTable dataTable = new System.Data.DataTable();
            SQL = "SELECT T.DisplayText AS Task, T.ModuleTitle, CT.DisplayText, CT.ModuleUri, " + SqlDate +
                "T.ResultType, CT.Result, T.UriType, CT.URI, T.NumberType, CT.NumberValue, T.BoolType, CT.BoolValue,T.DescriptionType,  CT.Description, T.NotesType, CT.Notes, " +
                "C.CollectionName, C.CollectionAcronym, P.CollectionName AS ParentCollection, P.CollectionAcronym AS ParentCollectionAcronym, " +
                "T.Type AS TaskType, T.Description AS TaskDescription, T.Notes AS TaskNotes, T.TaskURI " +
                "FROM CollectionTask AS CT " +
                "INNER JOIN Task AS T ON CT.TaskID = T.TaskID " +
                "INNER JOIN Collection AS C ON CT.CollectionID = C.CollectionID " +
                "LEFT OUTER JOIN Collection AS P ON P.CollectionID = C.CollectionParentID " +
                "WHERE CT.DisplayOrder > 0 AND CT.CollectionTaskID = " + CollectionTaskID.ToString() +
                " ORDER BY CT.DisplayOrder";
            //SQL = "SELECT T.ModuleTitle, T.ModuleType, CT.DisplayText, CT.ModuleUri, " +
            //    "T.DateType, " + SqlDate +
            //    "T.ResultType, CT.Result, T.UriType, CT.URI, T.NumberType, CT.NumberValue, T.BoolType, CT.BoolValue,T.DescriptionType,  CT.Description, T.NotesType, CT.Notes, " +
            //    "C.CollectionName, C.CollectionAcronym, C.DisplayText AS CollectionHierarchy, " +
            //    "T.DisplayText AS Task, T.Type AS TaskType, T.Description AS TaskDescription, T.Notes AS TaskNotes, T.TaskURI, " +
            //    "T.HierarchyDisplayText AS TaskHierarchyTopDown, T.HierarchyDisplayTextInvers AS TaskHierarchyBottomUp " +
            //    "FROM CollectionTask AS CT " +
            //    "INNER JOIN dbo.CollectionHierarchyAll() AS C ON CT.CollectionID = C.CollectionID " +
            //    "INNER JOIN dbo.TaskHierarchyAll() AS T ON CT.TaskID = T.TaskID " +
            //    "WHERE CT.DisplayOrder > 0 AND CT.CollectionTaskID = " + CollectionTaskID.ToString() +
            //    " ORDER BY CT.DisplayOrder";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTable);
            if (dataTable.Rows.Count > 0)
            {
                W.WriteStartElement("CollectionTask");
                foreach (System.Data.DataColumn C in dataTable.Rows[0].Table.Columns)
                {
                    if (!dataTable.Rows[0][C.ColumnName].Equals(System.DBNull.Value) && dataTable.Rows[0][C.ColumnName].ToString().Length > 0)
                    {
                        W.WriteElementString(C.ColumnName, dataTable.Rows[0][C.ColumnName].ToString());
                    }
                }
                this.writeXmlCollectionTaskImage(ref W, CollectionTaskID);

                SQL = "SELECT CollectionTaskID FROM CollectionTask WHERE CollectionTaskParentID = " + CollectionTaskID.ToString();
                System.Data.DataTable dataTableDependent = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTableDependent);
                if(dataTableDependent.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow R in dataTableDependent.Rows)
                        this.writeXmlCollectionTask(ref W, int.Parse(R[0].ToString()));
                }

                W.WriteEndElement();  //  CollectionTask
            }
        }

        private void writeXmlCollectionTaskImage(ref System.Xml.XmlWriter W, int CollectionTaskID)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
            string SQL = "SELECT URI, ImageType, Notes, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, " +
                    "LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear " +
                    "FROM CollectionTaskImage WHERE (DataWithholdingReason IS NULL OR DataWithholdingReason = '') " +
                    "AND CollectionTaskID = " + CollectionTaskID.ToString();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTable);
            if (dataTable.Rows.Count > 0)
            {
                W.WriteStartElement("Images");
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {

                    W.WriteStartElement("Image");
                    foreach (System.Data.DataColumn C in dataTable.Rows[i].Table.Columns)
                    {
                        if (!dataTable.Rows[i][C.ColumnName].Equals(System.DBNull.Value) && dataTable.Rows[i][C.ColumnName].ToString().Length > 0)
                        {
                            W.WriteElementString(C.ColumnName, dataTable.Rows[i][C.ColumnName].ToString());
                        }
                    }
                    W.WriteEndElement();  //  Image
                }
                W.WriteEndElement();  //  Images
            }
        }

        #endregion

    }
}
