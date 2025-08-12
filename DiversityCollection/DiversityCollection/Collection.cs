//#define CollectionLocactionIDAvailable
//#define LocationParentIDAvailable

using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace DiversityCollection
{
    class Collection : HierarchicalEntity
    {
        #region Parameter

        public static string SqlFieldsCollectionTask = " CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, DisplayText, TaskStart, TaskEnd, Result, URI, Description, Notes ";
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterCollectionTask;


        public static string SqlFieldsCollectionImage = " CollectionID, URI, ImageType, Notes, DataWithholdingReason, Description, Title, " +
                      "IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, RecordingDate ";
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterCollectionImages;
        private System.Windows.Forms.ListBox _ListBoxImages;

        public System.Windows.Forms.ListBox ListBoxImages
        {
            get 
            { 
                if (_ListBoxImages == null)
                {
                    if (this._formCollection != null)
                    {
                        //  _ListBoxImages = this._formCollection.li
                    }
                    else this._ListBoxImages = new System.Windows.Forms.ListBox();
                }
                return _ListBoxImages; 
            }
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

        private System.Windows.Forms.Button _buttonHeaderShowCollectionImage;

        public System.Windows.Forms.Button ButtonHeaderShowCollectionImage
        {
            get { return _buttonHeaderShowCollectionImage; }
            set { _buttonHeaderShowCollectionImage = value; }
        }

        private System.Windows.Forms.Button _buttonHeaderShowCollectionPlan;

        public System.Windows.Forms.Button ButtonHeaderShowCollectionPlan
        {
            get { return _buttonHeaderShowCollectionPlan; }
            set { _buttonHeaderShowCollectionPlan = value; }
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

        private System.Windows.Forms.SplitContainer _splitContainerImagesAndPlan;

        public System.Windows.Forms.SplitContainer SplitContainerImagesAndPlan
        {
            get { return _splitContainerImagesAndPlan; }
            set { _splitContainerImagesAndPlan = value; }
        }

        public System.Windows.Forms.Control ControlMainData
        {
            get { return _ControlMainData; }
            set { _ControlMainData = value; }
        }

        public System.Windows.Forms.Control ControlDependentData
        {
            get { return _ControlDependentData; }
            set { _ControlDependentData = value; }
        }

        private System.Windows.Forms.Label _LabelHeader;

        public System.Windows.Forms.Label LabelHeader
        {
            get 
            {
                if (_LabelHeader == null)
                {
                    if (this._formCollection != null)
                        _LabelHeader = this._formCollection.LabelHeader;
                    else
                        _LabelHeader = new System.Windows.Forms.Label();
                }
                return _LabelHeader; 
            }
            set { _LabelHeader = value; }
        }

        private DiversityCollection.Forms.FormCollection _formCollection;
        public void setFormCollection(DiversityCollection.Forms.FormCollection F) { this._formCollection = F; }

        // #205
        //private CollectionLocation _CollectionLocation;
        //public CollectionLocation CollectionLocation
        //{
        //    set { _CollectionLocation = value; }
        //}

        public override string ColumnParentID
        {
            get
            {
                if (HierarchyAccordingToLocation)
                    return "LocationParentID";
                //#if CollectionLocactionIDAvailable
                //#if CollectionLocactionIDAvailable
                //                if (this.HierarchyAccordingToLocation)
                //                    return "CollectionLocationID";
                //                else
                //#endif
                //#if DEBUG
                //#if LocationParentIDAvailable
                //                if (this.HierarchyAccordingToLocation)
                //                    return "LocationParentID";
                //                else
                //#endif
                //#endif
                return this.MainTable + "ParentID";
            }
        }

        public override string MainTableHierarchy
        {
            get
            {
                if (/*DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation &&*/ this.HierarchyAccordingToLocation)
                    return "CollectionLocation";
                return this._MainTable + "Hierarchy";
            }
        }

        public override string sqlItemFieldList 
        { 
            get 
            {
                return " CollectionID, CollectionParentID, CollectionName, CollectionAcronym, " +
                   "AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
                   "Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, Type, LocationParentID";
                //if (HierarchyAccordingToLocation)
                //{
                //    return " CollectionID, LocationParentID, CollectionName, CollectionAcronym, " +
                //        "AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
                //        "Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, Type";
                //}
                //else
                //{
                //    return " CollectionID, CollectionParentID, CollectionName, CollectionAcronym, " +
                //        "AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
                //        "Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, Type, LocationParentID";
                //}
            } 
        }


        #endregion

        #region Construction

        public Collection(
            ref System.Data.DataSet Dataset,
            System.Data.DataTable DataTable,
            ref System.Windows.Forms.TreeView TreeView,
            System.Windows.Forms.Form Form,
            DiversityWorkbench.UserControls.UserControlQueryList UserControlQueryList,
            System.Windows.Forms.SplitContainer SplitContainerMain,
            System.Windows.Forms.SplitContainer SplitContainerData,
            System.Windows.Forms.ToolStripButton ToolStripButtonSpecimenList,
            //System.Windows.Forms.ImageList ImageListSpecimenList,
            DiversityCollection.UserControls.UserControlSpecimenList UserControlSpecimenList,
            System.Windows.Forms.HelpProvider HelpProvider,
            System.Windows.Forms.ToolTip ToolTip,
            ref System.Windows.Forms.BindingSource BindingSource)
            : base(ref Dataset, DataTable, ref TreeView, Form, UserControlQueryList, SplitContainerMain,
            SplitContainerData, ToolStripButtonSpecimenList, /*ImageListSpecimenList,*/ UserControlSpecimenList,
            HelpProvider, ToolTip, ref BindingSource, DiversityCollection.LookupTable.DtCollection, DiversityCollection.LookupTable.DtCollectionWithHierarchy)
        {
            this._sqlItemFieldList = " CollectionID, CollectionParentID, CollectionName, CollectionAcronym, " +
                "AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
                "Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, Type, LocationParentID";
#if !DEBUG // Test f. #205
            if (HierarchyAccordingToLocation)
            {
                this._sqlItemFieldList = " CollectionID, LocationParentID, CollectionName, CollectionAcronym, " +
                    "AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
                    "Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, Type";
            }
#endif
            // ";
            //#if CollectionLocactionIDAvailable
            //#if CollectionLocactionIDAvailable
            //            this._sqlItemFieldList += ", CollectionLocationID";
            //#endif
            //#if LocationParentIDAvailable
            //            this._sqlItemFieldList += ", LocationParentID";
            //#endif
            this._SpecimenTable = "CollectionSpecimenPart";
            this._MainTable = "Collection";
            this.UseHierarchyNodes = true;
            string SQL = "SELECT USER_NAME()";
            this._OrderColumns.Add("CollectionName");
            string DatabaseUser = "";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                DatabaseUser = C.ExecuteScalar()?.ToString() ?? string.Empty;
                con.Close();
            }
            catch { }
            bool IsAdmin = false;
            if (DiversityWorkbench.Database.DatabaseRoles().Contains("Administrator"))
                IsAdmin = true;
            if (DatabaseUser != "dbo" && !IsAdmin)
            {
                DiversityWorkbench.UserControls.QueryRestrictionItem Q = new DiversityWorkbench.UserControls.QueryRestrictionItem();
                Q.ColumnName = "CollectionID";
                if (HierarchyAccordingToLocation)
                    Q.TableName = "CollectionLocationAll()";
                else
                    Q.TableName = "CollectionHierarchyAll()";
                Q.Restriction = " IN (SELECT CollectionID FROM dbo.ManagerCollectionList())";
                System.Collections.Generic.List<DiversityWorkbench.UserControls.QueryRestrictionItem> L = new List<DiversityWorkbench.UserControls.QueryRestrictionItem>();
                L.Add(Q);
                if (this._UserControlQueryList != null)
                    this._UserControlQueryList.setQueryRestrictionList(L);
            }
        }

        public Collection(
            ref System.Data.DataSet Dataset,
            System.Data.DataTable DataTable,
            ref System.Windows.Forms.TreeView TreeView,
            bool HierarchyAccordingToLocation,
            ref System.Windows.Forms.BindingSource BindingSource)
            : base(ref Dataset, DataTable, ref TreeView, null, null, null,
            null, null, /*ImageListSpecimenList,*/ null,
            null, null, ref BindingSource, DiversityCollection.LookupTable.DtCollection, DiversityCollection.LookupTable.DtCollectionWithHierarchy)
        {
            _HierarchyAccordingToLocation = HierarchyAccordingToLocation;
            this._sqlItemFieldList = " CollectionID, CollectionParentID, CollectionName, CollectionAcronym, " +
                "AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
                "Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, Type, LocationParentID";
            this._SpecimenTable = "CollectionSpecimenPart";
            //if (HierarchyAccordingToLocation)
            //{
            //    this._sqlItemFieldList = " CollectionID, LocationParentID, CollectionName, CollectionAcronym, " +
            //        "AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
            //        "Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, Type";
            //}
            //else
            //{
            //    this._sqlItemFieldList = " CollectionID, CollectionParentID, CollectionName, CollectionAcronym, " +
            //        "AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
            //        "Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, Type, LocationParentID";
            //    this._SpecimenTable = "CollectionSpecimenPart";
            //}
            // ";
            //#if CollectionLocactionIDAvailable
            //#if CollectionLocactionIDAvailable
            //            this._sqlItemFieldList += ", CollectionLocationID";
            //#endif
            //#if LocationParentIDAvailable
            //            this._sqlItemFieldList += ", LocationParentID";
            //#endif
            this._MainTable = "Collection";
            this.UseHierarchyNodes = true;
            string SQL = "SELECT USER_NAME()";
            this._OrderColumns.Add("CollectionName");
            string DatabaseUser = "";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                DatabaseUser = C.ExecuteScalar()?.ToString() ?? string.Empty;
                con.Close();
            }
            catch { }
            bool IsAdmin = false;
            if (DiversityWorkbench.Database.DatabaseRoles().Contains("Administrator"))
                IsAdmin = true;
            if (DatabaseUser != "dbo" && !IsAdmin)
            {
                DiversityWorkbench.UserControls.QueryRestrictionItem Q = new DiversityWorkbench.UserControls.QueryRestrictionItem();
                Q.ColumnName = "CollectionID";
                if (HierarchyAccordingToLocation)
                    Q.TableName = "CollectionLocationAll()";
                else
                    Q.TableName = "CollectionHierarchyAll()";
                Q.Restriction = " IN (SELECT CollectionID FROM dbo.ManagerCollectionList())";
                System.Collections.Generic.List<DiversityWorkbench.UserControls.QueryRestrictionItem> L = new List<DiversityWorkbench.UserControls.QueryRestrictionItem>();
                L.Add(Q);
                if (this._UserControlQueryList != null)
                    this._UserControlQueryList.setQueryRestrictionList(L);
            }
        }

#endregion

        #region Datahandling

        public override void fillDependentTables(int ID)
        {
            try
            {
                this._DataSet.Tables["CollectionImage"].Clear();

                if (this._DataSet == null) this._DataSet = new System.Data.DataSet();

                // Images
                string SQL = "SELECT CollectionID, URI, ImageType, Notes, DataWithholdingReason, Description, Title, " +
                          "IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, RecordingDate " +
                    "FROM CollectionImage " +
                    "WHERE CollectionID = " + ID.ToString();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                this.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterCollectionImages, SQL, this._DataSet.Tables["CollectionImage"]);

                // Tasks
                if (this._ShowTasks)
                {
                    try
                    {
                        SQL = "SELECT TOP 1 HierarchyDisplayText, CollectionTaskID FROM [dbo].[CollectionTaskHierarchyAll] () " + //SELECT CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, DisplayText, TaskStart, TaskEnd, Result, URI, Description, Notes FROM CollectionTask" +
                            "WHERE CollectionID = " + ID.ToString();
                        Microsoft.Data.SqlClient.SqlDataAdapter adTask = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        if (!this._DataSet.Tables.Contains("CollectionTask"))
                            this._DataSet.Tables.Add("CollectionTask");
                        this.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterCollectionTask, SQL, this._DataSet.Tables["CollectionTask"]);
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }

                this.setFormControls();
                try
                {
                    this.ListBoxImages.SelectedIndex = -1;
                    if (this._DataSet.Tables["CollectionImage"].Rows.Count > 0 && this._ListBoxImages.Items.Count > 0)
                        this._ListBoxImages.SelectedIndex = 0;
                }
                catch (System.Exception ex)
                {
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        
        /// <summary>
        /// Updates the parent-child relationship for a collection item in the database. Do not use for LocationHierarchy!
        /// </summary>
        /// <param name="itemId">
        /// The ID of the collection item to update. Can be <c>null</c> if no specific item is targeted.
        /// </param>
        /// <param name="parentId">
        /// The ID of the new parent collection. Can be <c>null</c> to remove the parent relationship.
        /// </param>
        /// <param name="setCurrent">
        /// A boolean value indicating whether to update the current binding source with the new parent ID.
        /// </param>
        /// <returns>
        /// <c>true</c> if the update operation succeeds; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method handles database transactions to ensure consistency when updating the parent-child relationship.
        /// If <paramref name="setCurrent"/> is <c>true</c>, the method updates the current binding source.
        /// Otherwise, it updates the database directly.
        /// </remarks>
        /// <exception cref="Exception">
        /// Logs and displays any exceptions that occur during the update process.
        /// </exception>
        public override bool updateParent(int? itemId, int? parentId, bool setCurrent)
        {
            try
            {
                // writing the data
                using (SqlConnection c = new SqlConnection(DiversityWorkbench.Settings.ConnectionString))
                {
                    c.Open();
                    using (SqlTransaction transaction = c.BeginTransaction())
                    {
                        try
                        {
                            if (setCurrent)
                            {
                                System.Data.DataRowView RV = (System.Data.DataRowView)this._BindingSource.Current;
                                RV[this.ColumnParentID] = parentId.HasValue ? parentId.Value : System.DBNull.Value;
                            }
                            else
                            {
                                // update parent entry in DB (called by transfer to collection)
                                if (itemId.HasValue && parentId.HasValue)
                                    updateParentInDB(c, transaction, itemId.Value, parentId.Value, false);
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception Ex)
                        {
                            transaction.Rollback();
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(Ex);
                            DiversityWorkbench.ExceptionHandling.ShowErrorMessage(
                                "The dataset could not be updated. An error occurred: \r\n" + Ex.Message);
                            return false;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(Ex);
                DiversityWorkbench.ExceptionHandling.ShowErrorMessage(
                    "The dataset could not be updated. An error occurred: \r\n" + Ex.Message);
                return false;
            }
        }

        /// <summary>
        /// #215 Updates the parent-child relationship for a collection item in the database. Use for LocationHierarchy
        /// </summary>
        /// <param name="itemId">
        /// The ID of the collection item to update. Can be <c>null</c> if no specific item is targeted.
        /// </param>
        /// <param name="parentId">
        /// The ID of the new parent collection. Can be <c>null</c> to remove the parent relationship.
        /// </param>
        /// <param name="setCurrent">
        /// A boolean value indicating whether to update the current binding source with the new parent ID.
        /// </param>
        /// <returns>
        /// <c>true</c> if the update operation succeeds; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method handles database transactions to ensure consistency when updating the parent-child relationship.
        /// If <paramref name="setCurrent"/> is <c>true</c>, the method updates the current binding source.
        /// Otherwise, it updates the database directly.
        /// </remarks>
        /// <exception cref="Exception">
        /// Logs and displays any exceptions that occur during the update process.
        /// </exception>
        public bool updateParentLocation(int? itemId, int? parentId, bool setCurrent)
        {
            try
            {
                // writing the data
                using (SqlConnection c = new SqlConnection(DiversityWorkbench.Settings.ConnectionString))
                {
                    c.Open();
                    using (SqlTransaction transaction = c.BeginTransaction())
                    {
                        try
                        {
                            if (setCurrent)
                            {
                                System.Data.DataRowView RV = (System.Data.DataRowView)this._BindingSource.Current;
                                RV["LocationParentID"] = parentId.HasValue ? parentId.Value : System.DBNull.Value;
                            }
                            else
                            {
                                // update parent entry in DB (called by transfer to collection)
                                if (itemId.HasValue && parentId.HasValue)
                                    updateParentInDB(c, transaction, itemId.Value, parentId.Value, true);
                            }

                            transaction.Commit();
                        }
                        catch (Exception Ex)
                        {
                            transaction.Rollback();
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(Ex);
                            DiversityWorkbench.ExceptionHandling.ShowErrorMessage(
                                "The dataset could not be updated. An error occurred: \r\n" + Ex.Message);
                            return false;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(Ex);
                DiversityWorkbench.ExceptionHandling.ShowErrorMessage(
                    "The dataset could not be updated. An error occurred: \r\n" + Ex.Message);
                return false;
            }
            // #215 - update hierarchy
            if (itemId != null)
                this._formCollection.CollectionLocation.BuildHierarchy((int)itemId, true);
            return true;
        }

        public static bool ManagerHasAccessToCollection(int CollectionID)
        {
            // Check if the user has access to the collection
            // #221
            string SQL = "SELECT COUNT(*) FROM dbo.ManagerCollectionList() WHERE CollectionID = " + CollectionID.ToString();
            try
            {
                int i = 0;
                if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out i))
                    return i > 0; // Access granted if count is greater than 0
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "ManagerHasAccessToCollection");
            }
            return false; // Assume access not granted for now
        }


        public override bool deleteData()
        {
            string Message = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(DiversityWorkbench.Settings.ConnectionString))
                {
                    connection.Open();
                    // Start a transaction
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Step 1: Delete dependent data
                            if (!this.deleteDependentData((int)this._ID, connection, transaction))
                            {
                                transaction.Rollback();
                                System.Windows.Forms.MessageBox.Show("Failed to delete dependent data.");
                                return false;
                            }

                            // Step 2: Update the table (DELETE operation)
                            // Ensure the SqlDataAdapter has a valid SelectCommand
                            if (this._SqlDataAdapter.SelectCommand == null)
                            {
                                throw new InvalidOperationException("The SelectCommand of the SqlDataAdapter must be set before updating the table.");
                            }
                            // Use SqlCommandBuilder to generate UpdateCommand and DeleteCommand
                            using (SqlCommandBuilder commandBuilder = new SqlCommandBuilder(this._SqlDataAdapter))
                            {
                                // Associate the transaction with the commands
                                // Generate the commands
                                this._SqlDataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                                this._SqlDataAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();
                                // Explicitly associate the transaction with the commands
                                this._SqlDataAdapter.UpdateCommand.Connection = connection;
                                this._SqlDataAdapter.UpdateCommand.Transaction = transaction;
                                this._SqlDataAdapter.DeleteCommand.Connection = connection;
                                this._SqlDataAdapter.DeleteCommand.Transaction = transaction;
                                // Remove the current item from the BindingSource
                                this._BindingSource.RemoveCurrent();
                                // Call updateTable
                                if (this.FormFunctions.updateTable(this._DataSet, this._DataTable.TableName, this._SqlDataAdapter, this._BindingSource, ref Message))
                                {
                                    // Update CollectionClosure table with new parent-child relationships
                                    // HandleCollectionClosure(connection, transaction, "DELETE", this._ID, null);
                                    // Commit the transaction
                                    transaction.Commit();
                                    return true;
                                }
                                else
                                {
                                    // Rollback the transaction if updateTable fails
                                    transaction.Rollback();
                                    //if (Message.IndexOf("FK_CollectionManager_Collection") > -1)
                                    //{
                                    //    Message =
                                    //        "Please remove this collection from all collection managers first\r\n(Transaction - Collection manager)\r\n\r\n" +
                                    //        Message;
                                    //}
                                    Message = "The dataset could not be deleted. This can happen if the dataset is used in transactions or other relations. \r\n\r\n" +
                                              "Detailed error message from database:" + Message;
                                }

                                System.Windows.Forms.MessageBox.Show(Message);
                                return false;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            transaction.Rollback();
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            DiversityWorkbench.ExceptionHandling.ShowErrorMessage(
                                "The dataset could not be deleted. An error occurred: \r\n" + ex.Message);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                DiversityWorkbench.ExceptionHandling.ShowErrorMessage(
                    "The dataset could not be deleted. An error occurred: \r\n" + ex.Message);
                return false;
            }
        }


        private bool deleteDependentData(int ID, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                // Images
                string SQL = "DELETE I FROM CollectionImage I WHERE I.CollectionID = @ID";
                using (SqlCommand cmd = new SqlCommand(SQL, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.ExecuteNonQuery();
                }
                // Tasks
                SQL = "DELETE T FROM CollectionTask T WHERE T.CollectionID = @ID";
                using (SqlCommand cmd = new SqlCommand(SQL, connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return false;
            }
            return true;
        }

        private void updateParentInDB(SqlConnection connection, SqlTransaction transaction, int CollectionID, int ParentCollectionID, bool ForLocation = false)
        {
            string Target = "CollectionParentID";
            if (ForLocation) Target = "LocationParentID";
            string Superior = "CollectionHierarchySuperior";
            if (ForLocation) Superior = "CollectionLocationSuperior";
            string SQL = @"
                DECLARE @ID INT;
                DECLARE @Parent INT;
                SET @ID = @CollectionID;
                SET @Parent = @ParentCollectionID;
                -- Update the target column
                UPDATE C 
                SET C." + Target + @" = @Parent 
                FROM Collection C 
                WHERE CollectionID = @ID;
                -- Check the count
                DECLARE @Count INT;
                SET @Count = (
                    SELECT COUNT(*) 
                    FROM [dbo].[" + Superior + @"](@Parent) 
                    WHERE CollectionID = @ID
                );
                -- Rollback or raise an error if the count is 1
                IF @Count = 1
                    THROW 50000, 'Circular reference detected.', 1;
            ";
            using (SqlCommand cmd = new SqlCommand(SQL, connection, transaction))
            {
                // Add parameters to the command
                cmd.Parameters.AddWithValue("@CollectionID", CollectionID.ToString());
                cmd.Parameters.AddWithValue("@ParentCollectionID", ParentCollectionID.ToString());
                cmd.ExecuteNonQuery();
            }
        }

        internal bool SetParentCollectionID(int CollectionID, int ParentCollectionID, bool ForLocation = false)
        {
            bool OK = true;
            if (this.NoHierarchyLoop(CollectionID, ParentCollectionID, ForLocation))
            {
                //string Target = "CollectionParentID";
                //if (ForLocation) Target = "LocationParentID";
                //string Superior = "CollectionHierarchySuperior";
                //if (ForLocation) Superior = "CollectionLocationSuperior";
                //string SQL = "begin tran " +
                //    "declare @ID int; " +
                //    "declare @Parent int; " +
                //    "set @ID = " + CollectionID.ToString() + "; " +
                //    "set @Parent = " + ParentCollectionID.ToString() + "; " +
                //    "UPDATE C SET C." + Target + " = @Parent FROM Collection C WHERE CollectionID = @ID; " +
                //    "declare @Count int; " +
                //    "set @Count = (SELECT COUNT(*) FROM[dbo].[" + Superior + "](@Parent) WHERE CollectionID = @ID); " +
                //    "if @Count = 1 " +
                //    "rollback tran " +
                //    "else " +
                //    "commit tran ";
                //OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                // OK = true;
                // Update the parent-child relationship in the database #215
                if (ForLocation)
                    OK = updateParentLocation(CollectionID, ParentCollectionID, false);
                else
                    OK = updateParent(CollectionID, ParentCollectionID, false);
            }
            else
                OK = false;
            return OK;
        }

        // TODO Ariane change back to private after updating toolStripButtonSetParentLocation_Click in FormCollection!
        internal bool NoHierarchyLoop(int CollectionID, int ParentCollectionID, bool ForLocation = false)
        {
            string Children = "CollectionChildNodes";
            if (ForLocation) Children = "CollectionLocationChildNodes";
            string Superior = "CollectionHierarchySuperior";
            if (ForLocation) Superior = "CollectionLocationSuperior";

            // Can not be parent of self
            if (CollectionID == ParentCollectionID)
                return false;
            bool OK = true;
            try
            {
                // Children should not contain parent
                string SQL = "SELECT COUNT(*) FROM [dbo].[" + Children + "] (" + CollectionID.ToString() + ") WHERE CollectionID = " + ParentCollectionID.ToString();
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
                if (Result != "0")
                    return false;
                // Parents should not contain child
                SQL = "SELECT COUNT(*) FROM [dbo].[" + Superior + "] (" + ParentCollectionID.ToString() + ") WHERE CollectionID = " + CollectionID.ToString();
                Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
                if (Result != "0")
                    return false;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        private bool saveDependentTables(SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                // Ensure the SqlDataAdapter has a valid SelectCommand
                if (this._SqlDataAdapterCollectionImages.SelectCommand == null)
                {
                    throw new InvalidOperationException("The SelectCommand of the SqlDataAdapter for dependent tables must be set before updating.");
                }
                // Use SqlCommandBuilder to generate UpdateCommand and DeleteCommand
                using (SqlCommandBuilder commandBuilder = new SqlCommandBuilder(this._SqlDataAdapterCollectionImages))
                {
                    // Generate the commands
                    this._SqlDataAdapterCollectionImages.UpdateCommand = commandBuilder.GetUpdateCommand();
                    this._SqlDataAdapterCollectionImages.DeleteCommand = commandBuilder.GetDeleteCommand();
                    // Associate the transaction with the commands
                    this._SqlDataAdapterCollectionImages.UpdateCommand.Connection = connection;
                    this._SqlDataAdapterCollectionImages.UpdateCommand.Transaction = transaction;
                    this._SqlDataAdapterCollectionImages.DeleteCommand.Connection = connection;
                    this._SqlDataAdapterCollectionImages.DeleteCommand.Transaction = transaction;
                    // Update the dependent table
                    return this.FormFunctions.updateTable(this._DataSet, "CollectionImage", this._SqlDataAdapterCollectionImages, this._BindingSource);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                // System.Windows.Forms.MessageBox.Show("An error occurred while saving dependent tables: " + ex.Message);
                return false;
            }
        }
        // TODO Ariane delte this after refactoring all other HierachicalEntity childs saveDependentTables to use connection and transaction, e.g. Analysis
        public override void saveDependentTables()
        {
            // only for compatibility reasons.. should not be called!
            this.FormFunctions.updateTable(this._DataSet, "CollectionImage", this._SqlDataAdapterCollectionImages, this._BindingSource);
        }

        public override void saveTables()
        {
            string Message = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(DiversityWorkbench.Settings.ConnectionString))
                {
                    connection.Open();
                    // Start a transaction
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Step 1: Update the main table
                            // Ensure the SqlDataAdapter has a valid SelectCommand
                            if (this._SqlDataAdapter.SelectCommand == null)
                            {
                                throw new InvalidOperationException("The SelectCommand of the SqlDataAdapter must be set before updating the table.");
                            }
                            // Use SqlCommandBuilder to generate UpdateCommand and DeleteCommand
                            using (SqlCommandBuilder commandBuilder = new SqlCommandBuilder(this._SqlDataAdapter))
                            {
                                // Associate the transaction with the commands
                                // Generate the commands
                                this._SqlDataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                                this._SqlDataAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();
                                // Explicitly associate the transaction with the commands
                                this._SqlDataAdapter.UpdateCommand.Connection = connection;
                                this._SqlDataAdapter.UpdateCommand.Transaction = transaction;
                                this._SqlDataAdapter.DeleteCommand.Connection = connection;
                                this._SqlDataAdapter.DeleteCommand.Transaction = transaction;
                                
                                //// update the main table
                                //this._DataTable.Rows[0].BeginEdit();
                                //this._DataTable.Rows[0].EndEdit();

                                
                                if (this.FormFunctions.updateTable(this._DataSet, this._DataTable.TableName,
                                        this._SqlDataAdapter, this._BindingSource))
                                {
                                    
                                    // Step 2:Save dependent data
                                    if (!this.saveDependentTables(connection, transaction))
                                    {
                                        throw new Exception("Failed to save dependent tables.");
                                    }

                                    
                                    // Commit the transaction
                                    transaction.Commit();
                                }
                                else
                                {
                                    // Rollback the transaction if updateTable fails
                                    transaction.Rollback();
                                    Message = "The dataset could not be updated. \r\n\r\n" +
                                              "Detailed error message from database:" + Message;
                                    System.Windows.Forms.MessageBox.Show(Message);
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            transaction.Rollback();
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            DiversityWorkbench.ExceptionHandling.ShowErrorMessage(
                                "The dataset could not be updated. \r\n\r\n\" + " +
                                "An error occurred: \r\n" + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                DiversityWorkbench.ExceptionHandling.ShowErrorMessage(
                    "The dataset could not be updated. An error occurred: \r\n" + ex.Message);
            }

            //// zu #205 - drawing hierarchy when saving
            //if (this._ID != null)
            //    this._formCollection.CollectionLocation.BuildHierarchy((int)this._ID, true);
        }


        public override bool deleteDependentData(int ID)
        {
            try
            {
                // Images
                string SQL = "DELETE I FROM CollectionImage I " +
                             "WHERE I.CollectionID = " + ID.ToString();
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);

                // Tasks
                SQL = "DELETE T FROM CollectionTask T " +
                      "WHERE T.CollectionID = " + ID.ToString();
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public enum HeaderDisplay { None, Image, Plan, Label }
        public HeaderDisplay headerDisplay
        {
            get
            {
                return this._headerDisplay;
            }
            set
            {
                if (this._headerDisplay == value)
                    this._headerDisplay = HeaderDisplay.None;
                else
                    this._headerDisplay = value;
            }
        }
        private HeaderDisplay _headerDisplay = HeaderDisplay.None;
        public override void setFormControls()
        {
            if (this._ListBoxImages != null
                && this._imageListCollectionImages != null
                && this._userControlImageCollectionImage != null)
            {
                this._ListBoxImages.Items.Clear();
                if (this._ListBoxImages.Visible)
                {
                    this.FormFunctions.FillImageList(this._ListBoxImages, this._imageListCollectionImages,
                        this._DataSet.Tables["CollectionImage"], "URI", this._userControlImageCollectionImage);
                }
            }
            try
            {
                if (this._buttonHeaderShowCollectionImage != null)
                {
                    switch (_headerDisplay)
                    {
                        case HeaderDisplay.None:
                            this._buttonHeaderShowCollectionImage.Image = DiversityCollection.Resource.IconesGrey;
                            this._buttonHeaderShowCollectionImage.BackColor = System.Drawing.SystemColors.Control;
                            this._buttonHeaderShowCollectionPlan.BackColor = System.Drawing.SystemColors.Control;
                            break;
                        case HeaderDisplay.Image:
                            this._buttonHeaderShowCollectionImage.Image = DiversityCollection.Resource.Icones;
                            this._buttonHeaderShowCollectionImage.BackColor = System.Drawing.Color.Red;
                            this._buttonHeaderShowCollectionPlan.BackColor = System.Drawing.SystemColors.Control;
                            break;
                        case HeaderDisplay.Plan:
                            this._buttonHeaderShowCollectionImage.Image = DiversityCollection.Resource.IconesGrey;
                            this._buttonHeaderShowCollectionImage.BackColor = System.Drawing.SystemColors.Control;
                            this._buttonHeaderShowCollectionPlan.BackColor = System.Drawing.Color.Red;
                            break;
                        case HeaderDisplay.Label:
                            this._buttonHeaderShowCollectionImage.BackColor = System.Drawing.SystemColors.Control;
                            this._buttonHeaderShowCollectionPlan.BackColor = System.Drawing.SystemColors.Control;
                            break;
                    }
                }
                if (_headerDisplay != HeaderDisplay.Image && this._buttonHeaderShowCollectionImage != null)
                {
                    if (this._DataSet.Tables["CollectionImage"].Rows.Count > 0)
                        this._buttonHeaderShowCollectionImage.BackColor = System.Drawing.Color.Yellow;
                    //else
                    //    this._buttonHeaderShowCollectionImage.BackColor = System.Drawing.SystemColors.Control;
                }
                if (_headerDisplay != HeaderDisplay.Plan && this._ID != null)
                {
                    System.Data.DataRow[] rr = this._DataSet.Tables["Collection"].Select("CollectionID = " + this._ID.ToString());
                    if (rr.Length == 1)
                    {
                        bool HasGeometryOrPlan = false;
                        if (rr[0]["LocationPlan"].ToString().Length > 0)// || !rr[0]["LocationGeometry"].Equals(System.DBNull.Value))
                            HasGeometryOrPlan = true;
                        if (this._ID != null)
                        {
                            string SQL = "SELECT COUNT(*) FROM Collection WHERE NOT LocationGeometry IS NULL AND CollectionID = " + this._ID.ToString();
                            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
                            if (Result == "1")
                                HasGeometryOrPlan = true;
                        }
                        if (HasGeometryOrPlan && this._buttonHeaderShowCollectionPlan != null)
                            this._buttonHeaderShowCollectionPlan.BackColor = System.Drawing.Color.Yellow;

                    }
                    //if (this._DataSet.Tables["Collection"].Rows[0]["LocationPlan"].ToString().Length > 0)
                    //    this._buttonHeaderShowCollectionPlan.BackColor = System.Drawing.Color.Yellow;
                    //else
                    //    this._buttonHeaderShowCollectionImage.BackColor = System.Drawing.SystemColors.Control;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            try
            {
                if (this._DataTable.Rows.Count > 0)
                {
                    string CollectionName = this._DataTable.Rows[0]["CollectionName"].ToString();
                    System.Data.DataRow[] RR = this._DataTable.Select("CollectionID = " + this.ID.ToString());
                    if (RR.Length > 0)
                        CollectionName = RR[0]["CollectionName"].ToString();
                    this.LabelHeader.Text = CollectionName;
                }
                else
                {
                    this.LabelHeader.Text = "";
                }
            }
            catch { }
            this.ItemChanged();
        }
        
        private System.Collections.Generic.Dictionary<string, string> _ChildrenTypes;

        public string ChildType(string ParentType)
        {
            string Type = "";
            if (_ChildrenTypes == null)
            {
                _ChildrenTypes = new Dictionary<string, string>();
                _ChildrenTypes.Add("area", "area");
                _ChildrenTypes.Add("box", "area");
                _ChildrenTypes.Add("container", "area");
                _ChildrenTypes.Add("cupboard", "drawer");
                _ChildrenTypes.Add("department", "room");
                _ChildrenTypes.Add("drawer", "area");
                _ChildrenTypes.Add("freezer", "container");
                _ChildrenTypes.Add("fridge", "container");
                _ChildrenTypes.Add("institution", "department");
                _ChildrenTypes.Add("location", "room");
                _ChildrenTypes.Add("radioactive", "container");
                _ChildrenTypes.Add("room", "cupboard");
                _ChildrenTypes.Add("steel locker", "container");
                _ChildrenTypes.Add("subdivided container", "area");
            }
            if (_ChildrenTypes.ContainsKey(ParentType))
                Type = _ChildrenTypes[ParentType];
            return Type;
        }

        #endregion

        #region Functions and properties

        protected override string SqlSpecimenCount(int ID)
        {
            return "SELECT COUNT(*) FROM CollectionSpecimenPart WHERE CollectionID = " + ID.ToString();
        }

        #endregion

        #region Interface

        #region Query
        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions 
        { 
            get 
            {
                System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

                #region Collection
                
                string Description = this.FormFunctions.ColumnDescription("Collection", "CollectionName");
                DiversityWorkbench.QueryCondition q0 = new DiversityWorkbench.QueryCondition(true, "Collection", "CollectionID", "CollectionName", "Collection", "Name", "Name", Description);
                QueryConditions.Add(q0);

                Description = DiversityWorkbench.Functions.ColumnDescription("Collection", "CollectionID");
                DiversityWorkbench.QueryCondition qCollectionID = new DiversityWorkbench.QueryCondition(false, "Collection", "CollectionID", "CollectionID", "Collection", "ID", "CollectionID", Description, false, false, true, false);
                QueryConditions.Add(qCollectionID);

                System.Data.DataTable dtCollection = new System.Data.DataTable();
                string SQL = "SELECT NULL AS [Value], NULL AS [ParentValue], NULL AS Display, NULL AS DisplayOrder " +
                    "UNION " +
                    "SELECT CollectionID AS [Value], CollectionParentID, CollectionName, DisplayOrder " +
                    "FROM Collection " +
                    "ORDER BY Display ";
                Microsoft.Data.SqlClient.SqlDataAdapter aColl = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aColl.Fill(dtCollection); }
                    catch { }
                }
                if (dtCollection.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn ParentValue = new System.Data.DataColumn("ParentValue");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    System.Data.DataColumn DisplayOrder = new System.Data.DataColumn("DisplayOrder");
                    dtCollection.Columns.Add(Value);
                    dtCollection.Columns.Add(ParentValue);
                    dtCollection.Columns.Add(Display);
                    dtCollection.Columns.Add(DisplayOrder);
                }
                System.Collections.Generic.List<DiversityWorkbench.QueryField> FFCollectionParentID = new List<DiversityWorkbench.QueryField>();
                DiversityWorkbench.QueryField CPC_CollectionParentID = new DiversityWorkbench.QueryField("Collection", "CollectionParentID", "CollectionID");
                FFCollectionParentID.Add(CPC_CollectionParentID);
                Description = DiversityWorkbench.Functions.ColumnDescription("Collection", "CollectionParentID");
                DiversityWorkbench.QueryCondition qCollectionParentID = new DiversityWorkbench.QueryCondition(true, FFCollectionParentID, "Collection", "Collection", "Collection", Description, dtCollection, true, "DisplayOrder", "ParentValue", "Display", "Value");
                QueryConditions.Add(qCollectionParentID);


                //Description = this.FormFunctions.ColumnDescription("Collection", "CollectionAcronym");
                //DiversityWorkbench.QueryCondition qHierarchy = new DiversityWorkbench.QueryCondition(true, "Collection", "CollectionID", "CollectionAcronym", "Collection", "Acronym", "Acronym", Description);
                //QueryConditions.Add(qHierarchy);

                Description = this.FormFunctions.ColumnDescription("Collection", "CollectionAcronym");
                DiversityWorkbench.QueryCondition q1 = new DiversityWorkbench.QueryCondition(true, "Collection", "CollectionID", "CollectionAcronym", "Collection", "Acronym", "Acronym", Description);
                QueryConditions.Add(q1);

                Description = this.FormFunctions.ColumnDescription("Collection", "Description");
                DiversityWorkbench.QueryCondition q2 = new DiversityWorkbench.QueryCondition(true, "Collection", "CollectionID", "Description", "Collection", "Description", "Description", Description);
                QueryConditions.Add(q2);

                Description = this.FormFunctions.ColumnDescription("Collection", "Location");
                DiversityWorkbench.QueryCondition q3 = new DiversityWorkbench.QueryCondition(true, "Collection", "CollectionID", "Location", "Collection", "Location", "Location", Description);
                QueryConditions.Add(q3);

                Description = this.FormFunctions.ColumnDescription("Collection", "AdministrativeContactName");
                DiversityWorkbench.QueryCondition q4 = new DiversityWorkbench.QueryCondition(true, "Collection", "CollectionID", "AdministrativeContactName", "Collection", "Contact", "Contact", Description);
                QueryConditions.Add(q4);

                //LocationPlan
                Description = this.FormFunctions.ColumnDescription("Collection", "LocationPlan");
                DiversityWorkbench.QueryCondition q5 = new DiversityWorkbench.QueryCondition(true, "Collection", "CollectionID", "LocationPlan", "Collection", "Plan", "Plan", Description);
                QueryConditions.Add(q5);

                //Type
                Description = DiversityWorkbench.Functions.ColumnDescription("Collection", "Type");
                DiversityWorkbench.QueryCondition qType = new DiversityWorkbench.QueryCondition(false, "Collection", "CollectionID", "Type", "Collection", "Type", "Type", Description, "CollCollectionType_Enum");
                QueryConditions.Add(qType);

                System.Data.DataTable dtUser = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT LoginName, CombinedNameCache " +
                    "FROM " + DiversityWorkbench.Settings.ServerConnection.Prefix() + "UserProxy " +
                    "ORDER BY Display";
                Microsoft.Data.SqlClient.SqlDataAdapter aUser = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aUser.Fill(dtUser); }
                    catch { }
                }

                Description = DiversityWorkbench.Functions.ColumnDescription("Collection", "LogCreatedBy");
                DiversityWorkbench.QueryCondition qLogCreatedBy = new DiversityWorkbench.QueryCondition(false, "Collection", "CollectionID", "LogCreatedBy", "Collection", "Creat. by", "The user that created the dataset", Description, dtUser, false);
                QueryConditions.Add(qLogCreatedBy);

                Description = DiversityWorkbench.Functions.ColumnDescription("Collection", "LogCreatedWhen");
                DiversityWorkbench.QueryCondition qLogCreatedWhen = new DiversityWorkbench.QueryCondition(false, "Collection", "CollectionID", "LogCreatedWhen", "Collection", "Creat. date", "The date when the dataset was created", Description, true);
                QueryConditions.Add(qLogCreatedWhen);

                Description = DiversityWorkbench.Functions.ColumnDescription("Collection", "LogUpdatedBy");
                DiversityWorkbench.QueryCondition qLogUpdatedBy = new DiversityWorkbench.QueryCondition(false, "Collection", "CollectionID", "LogUpdatedBy", "Collection", "Changed by", "The last user that changed the dataset", Description, dtUser, false);
                QueryConditions.Add(qLogUpdatedBy);

                Description = DiversityWorkbench.Functions.ColumnDescription("Collection", "LogUpdatedWhen");
                DiversityWorkbench.QueryCondition qLogUpdatedWhen = new DiversityWorkbench.QueryCondition(false, "Collection", "CollectionID", "LogUpdatedWhen", "Collection", "Changed at", "The last date when the dataset was changed", Description, true);
                QueryConditions.Add(qLogUpdatedWhen);


                #endregion

                #region Image

                Description = "If any image is present";
                DiversityWorkbench.QueryCondition qImagePresent = new DiversityWorkbench.QueryCondition(false, "CollectionImage", "CollectionID", "Image", "Presence", "Image present", Description, DiversityWorkbench.QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qImagePresent);

                Description = this.FormFunctions.ColumnDescription("CollectionImage", "URI");
                DiversityWorkbench.QueryCondition qURI = new DiversityWorkbench.QueryCondition(false, "CollectionImage", "CollectionID", "URI", "Image", "URI", "URI", Description);
                QueryConditions.Add(qURI);

                Description = this.FormFunctions.ColumnDescription("CollectionImage", "ImageType");
                DiversityWorkbench.QueryCondition qImageType = new DiversityWorkbench.QueryCondition(false, "CollectionImage", "CollectionID", "ImageType", "Image", "Type", "Image type", Description);
                QueryConditions.Add(qImageType);

                Description = this.FormFunctions.ColumnDescription("CollectionImage", "Notes");
                DiversityWorkbench.QueryCondition qImageNotes = new DiversityWorkbench.QueryCondition(false, "CollectionImage", "CollectionID", "Notes", "Image", "Notes", "Notes", Description);
                QueryConditions.Add(qImageNotes);
                
                #endregion

                return QueryConditions;
            } 
        }

        public override DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns 
        { 
            get 
            { 
                string TableName = "CollectionHierarchyAll()";
                //#if CollectionLocactionIDAvailable
                //                if (HierarchyAccordingToLocation)
                //                    TableName = "CollectionLocationHierarchyAll()";
                //#endif
                DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[5];
                QueryDisplayColumns[0].DisplayText = "Collection";
                QueryDisplayColumns[0].DisplayColumn = "CollectionName";
                QueryDisplayColumns[0].OrderColumn = "CollectionName";
                QueryDisplayColumns[0].IdentityColumn = "CollectionID";
                QueryDisplayColumns[0].TableName = TableName;

                QueryDisplayColumns[1].DisplayText = "Hierarchy";
                QueryDisplayColumns[1].DisplayColumn = "DisplayText";
                QueryDisplayColumns[1].OrderColumn = "DisplayText";
                QueryDisplayColumns[1].IdentityColumn = "CollectionID";
                QueryDisplayColumns[1].TableName = TableName;

                QueryDisplayColumns[2].DisplayText = "Acronym";
                QueryDisplayColumns[2].DisplayColumn = "CollectionAcronym";
                QueryDisplayColumns[2].OrderColumn = "CollectionAcronym";
                QueryDisplayColumns[2].IdentityColumn = "CollectionID";
                QueryDisplayColumns[2].TableName = TableName;

                QueryDisplayColumns[3].DisplayText = "Type";
                QueryDisplayColumns[3].DisplayColumn = "Type";
                QueryDisplayColumns[3].OrderColumn = "Type";
                QueryDisplayColumns[3].IdentityColumn = "CollectionID";
                QueryDisplayColumns[3].TableName = TableName;

                QueryDisplayColumns[4].DisplayText = "Description";
                QueryDisplayColumns[4].DisplayColumn = "Description";
                QueryDisplayColumns[4].OrderColumn = "Description";
                QueryDisplayColumns[4].IdentityColumn = "CollectionID";
                QueryDisplayColumns[4].TableName = TableName;

                return QueryDisplayColumns;
            } 
        }


        public override void ItemChanged()
        {
            try
            {
                if (this._formCollection != null && this._ID != null)
                {
                    this._formCollection.listTasks((int)this._ID);

                    this._formCollection.SetLocationGeometry((int)this._ID);
                    this._formCollection.SetLocationScale((int)this._ID);
                    // #205
                    this._formCollection.CollectionLocation.BuildHierarchy((int)_ID, true);
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Hierarchy

        private bool _HierarchyAccordingToLocation = false;

        public bool HierarchyAccordingToLocation 
        { 
            get => _HierarchyAccordingToLocation; 
            set 
            { 
                _HierarchyAccordingToLocation = value;
                // #205
                this._TreeView.Nodes.Clear();
                this.buildHierarchy(); 
            } 
        }

        private bool _HierarchyIncludingParts;
        public bool HierarchyIncludingParts { get => _HierarchyIncludingParts;  set { _HierarchyIncludingParts = value; this.buildHierarchy(); } }

        protected override void addHierarchyNodes(int ParentID, System.Windows.Forms.TreeNode pNode, bool IncludeAllChildren = true)
        {
            this._TreeView.LineColor = System.Drawing.Color.DarkOrange;
            //if (HierarchyIncludingParts)
            //{
            //    this.addHierarchyPartNodes(ParentID, pNode);
            //}
            string Restriction = this.ColumnParentID + " = " + ParentID.ToString();
            if (HierarchyAccordingToLocation)
            {
                //Restriction = ParentID.ToString() + " = case when LocationParentID is null then " + this.ColumnParentID + " else LocationParentID end ";
                Restriction =  "LocationParentID is not null and LocationParentID = " + ParentID.ToString() + " or LocationParentID is null and " + this.ColumnParentID + " = " + ParentID.ToString();
            }
            string ParentColumn = this.ColumnParentID;
            System.Data.DataRow[] rr = this._DataTable.Select(Restriction);
            foreach (System.Data.DataRow r in rr)
            {
                int CollectionID = System.Int32.Parse(r[this.ColumnID].ToString());
                string NodeText = this.HierarchyNodeText(r); // r[this.ColumnDisplayText].ToString();

                DiversityCollection.HierarchyNode Node = new HierarchyNode(r, !IncludeAllChildren);
#if DEBUG
#else
                //System.Windows.Forms.TreeNode Node = new System.Windows.Forms.TreeNode(NodeText);
                //System.Drawing.Font F = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                //if (r.Table.Columns.Contains("LocationParentID") && !r["LocationParentID"].Equals(System.DBNull.Value) && DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation && this.HierarchyAccordingToLocation)
                //{
                //    //Node.BackColor = System.Drawing.Color.SandyBrown;
                //    Node.ForeColor = System.Drawing.Color.Brown;
                //    F = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                //}
                //Node.NodeFont = F;
#endif
                Node.Tag = r;
                pNode.Nodes.Add(Node);
                if(HierarchyIncludingParts)
                {
                    this.addHierarchyPartNodes(CollectionID, Node);
                }
                if (CollectionID == this.ID)
                    this._CurrentNode = Node;

                if (this._ShowTasks)
                {
                    this.addHierarchyTaskNodes(CollectionID, Node, r["Type"].ToString());
                }
                if (IncludeAllChildren)
                    this.addHierarchyNodes(CollectionID, Node, IncludeAllChildren);
            }
        }

        protected override string HierarchyRestriction()
        {
            string Restriction = "CollectionID = " + this.ID.ToString();
            //if (this.HierarchyAccordingToLocation && this.ID != null)
            //{
            //    System.Data.DataRow[] rrCurrent = this._DataTable.Select("CollectionID = " + ID.ToString());
            //    if (rrCurrent.Length > 0)
            //    {
            //        System.Data.DataRow R = rrCurrent[0];
            //        if (!R["LocationParentID"].Equals(System.DBNull.Value))
            //        {
            //            Restriction = "CollectionID = " + R["LocationParentID"].ToString();
            //        }
            //    }
            //}
            return Restriction;
        }

        private System.DateTime _TaskStart;
        private System.DateTime _TaskEnd;

        public System.DateTime TaskStart { get => _TaskStart; set => _TaskStart = value; }
        public System.DateTime TaskEnd { get => _TaskEnd; set => _TaskEnd = value; }

        private void addHierarchyTaskNodes(int CollectionID, System.Windows.Forms.TreeNode pNode, string Type)
        {
            if (this._taskDisplayStyle == TaskDisplayStyle.RestrictToSameType)
            {
                //string Type = r["Type"].ToString();
                string SQL = "SELECT CASE WHEN C.DisplayText IS NULL OR C.DisplayText = '' THEN " +
                        "CASE WHEN C.MetricDescription <> '' AND C.MetricUnit <> '' THEN C.MetricDescription + ' ' + C.MetricUnit " +
                        "ELSE CASE WHEN T.DisplayText IS NULL THEN T.Type ELSE T.DisplayText END END " +
                        "ELSE " +
                        "CASE WHEN T.DisplayText<> C.DisplayText THEN T.DisplayText + ': ' ELSE '' END " +
                        "+ C.DisplayText END " +
                        "+ CASE WHEN C.TaskStart IS NULL THEN '' ELSE ' (' + CONVERT(varchar(10), C.TaskStart, 120) " +
                        "+ CASE WHEN C.TaskEnd IS NULL THEN '' ELSE ' - ' + CONVERT(varchar(10), C.TaskEnd, 120)  END " +
                        "+ ')' END " +
                        "AS DisplayText, " +
                        "C.CollectionTaskID, T.Type " +
                        ", CASE WHEN C.TaskEnd < GetDate() THEN 1 ELSE 0 END AS TrapEnded, CASE WHEN C.BoolValue = 1 THEN 1 ELSE 0 END AS Trapped, C.MetricUnit, T.Type " +
                        "FROM [dbo].[CollectionTask] C INNER JOIN Task T ON C.TaskID = T.TaskID " +
                        "AND C.CollectionID = " + CollectionID.ToString() +
                        " AND T.Type IN ('" + Type + "'";
                foreach (string T in DiversityCollection.LookupTable.TaskTypes(Type))
                {
                    if (T.ToLower() != Type.ToLower())
                        SQL += ", '" + T + "'";
                }
                SQL += ") ";
                if (this.TaskStart.Year > 1700)
                {
                    if(this.TaskEnd.Year > 1700)
                    {
                        SQL += " AND (C.TaskEnd IS NULL OR C.TaskEnd between convert(datetime, '" + this.TaskStart.ToString("yyyy-MM-dd") + "', 120) and convert(datetime, '" + this.TaskEnd.ToString("yyyy-MM-dd") + "', 120)) ";
                    }
                    else
                        SQL += " AND (C.TaskEnd IS NULL OR (convert(datetime, '" + this.TaskStart.ToString("yyyy-MM-dd") + "', 120) between C.TaskStart AND case when C.TaskEnd is null or C.TaskEnd < C.TaskStart then getdate() else C.TaskEnd end)) ";
                }
                SQL += "ORDER BY DisplayText";

                this._dtTask = new System.Data.DataTable();
                string Message = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._dtTask, ref Message);
                if (this._dtTask.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow R in this._dtTask.Rows)
                    {
                        string Task = R[0].ToString(); // r[this.ColumnDisplayText].ToString();
                        if (Task.Length == 0)
                        { }
                        System.Windows.Forms.TreeNode NodeTask = new System.Windows.Forms.TreeNode(Task);
                        string TypeOfTask = R["Type"].ToString().ToLower();
                        bool SetDefaultColor = true;
                        switch (TypeOfTask)
                        {
                            case "trap":
                                int IsTrapped = 0;
                                SetDefaultColor = false;
                                NodeTask.ForeColor = System.Drawing.Color.DarkBlue;
                                if (int.TryParse(R["TrapEnded"].ToString(), out IsTrapped) && IsTrapped == 1)
                                {
                                    NodeTask.ImageIndex = DiversityCollection.Specimen.CollectionTypeImage("Trapped", false);
                                    NodeTask.SelectedImageIndex = DiversityCollection.Specimen.CollectionTypeImage("Trapped", false);
                                    NodeTask.ForeColor = System.Drawing.Color.DarkRed;
                                }
                                else
                                    goto default;
                                break;
                            case "battery":
                                NodeTask.ImageIndex = DiversityCollection.Specimen.CollectionTypeImage("Battery", false);
                                NodeTask.SelectedImageIndex = DiversityCollection.Specimen.CollectionTypeImage("Battery", false);
                                //NodeTask.ForeColor = System.Drawing.Color.Blue;
                                break;
                            case "humidity":
                                NodeTask.ImageIndex = DiversityCollection.Specimen.CollectionTypeImage("SensorHum", false);
                                NodeTask.SelectedImageIndex = DiversityCollection.Specimen.CollectionTypeImage("SensorHum", false);
                                //NodeTask.ForeColor = System.Drawing.Color.Blue;
                                break;
                            case "temperature":
                                NodeTask.ImageIndex = DiversityCollection.Specimen.CollectionTypeImage("SensorTemp", false);
                                NodeTask.SelectedImageIndex = DiversityCollection.Specimen.CollectionTypeImage("SensorTemp", false);
                                //NodeTask.ForeColor = System.Drawing.Color.DarkViolet;
                                break;
                            default:
                                NodeTask.ImageIndex = DiversityCollection.Specimen.CollectionTypeImage(Type, false);
                                NodeTask.SelectedImageIndex = DiversityCollection.Specimen.CollectionTypeImage(Type, false);
                                //NodeTask.ForeColor = System.Drawing.Color.Gray;
                                break;
                        }
                        if (SetDefaultColor)
                            NodeTask.ForeColor = System.Drawing.Color.Green;
                        NodeTask.Tag = R[1].ToString();
                        pNode.Nodes.Add(NodeTask);
                    }
                }

            }
            else
            {
                if (this.TaskNodes().ContainsKey(CollectionID))
                {
                    foreach (System.Windows.Forms.TreeNode node in this.TaskNodes()[CollectionID])
                    {
                        pNode.Nodes.Add(node);
                    }
                }
            }
        }


        public void addHierarchyPartNodes(int CollectionID, System.Windows.Forms.TreeNode pNode)
        {
            string SQL = "SELECT P.CollectionSpecimenID, P.SpecimenPartID, P.MaterialCategory, P.StorageLocation, P.PartSublabel, " +
                "case when P.AccessionNumber <> '' then P.AccessionNumber else case when S.AccessionNumber <> '' then S.AccessionNumber else '' end end AS AccessionNumber, ";
            SQL += SubcollectionContentDisplayText();
                //"case when not P.Stock is null then cast(P.Stock as varchar) + case when P.StockUnit <> '' then ' ' + P.StockUnit else '' end + ' '  else '' end " +
                //"+ case when P.StorageLocation <> '' then P.StorageLocation + ' ' else '' end " +
                //"+ case when P.StorageLocation <> '' and(P.AccessionNumber <> '' or S.AccessionNumber <> '') then '- ' else '' end " +
                //"+ case when P.AccessionNumber <> '' then P.AccessionNumber + ' ' else case when S.AccessionNumber <> '' then S.AccessionNumber + ' ' else '' end end " +
                //"+ case when P.PartSublabel <> '' then P.PartSublabel else '' end " +
                //"+ case when E.CollectionYear is null and E.CollectionMonth is null and E.CollectionDay is null then '' " +
                //"else " +
                //"   +' - ' " +
                //"   + case when E.CollectionYear is null then '' else cast(E.CollectionYear as varchar) end + '/' " + 
                //"   + case when E.CollectionMonth is null then '' else cast(E.CollectionMonth as varchar) end + '/' " +
                //"   + case when E.CollectionDay is null then '' else cast(E.CollectionDay as varchar) end + ' ' " +
                //"   + case when E.LocalityDescription <> '' then ': ' + E.LocalityDescription else '' end " +
                //"end "  +
                //"AS DisplayText " +
                SQL += "FROM CollectionSpecimenPart AS P INNER JOIN " +
                "CollectionSpecimen AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID " +
                "LEFT OUTER JOIN CollectionEvent E ON E.CollectionEventID = S.CollectionEventID " +
                "LEFT OUTER JOIN IdentificationUnit AS U ON S.CollectionSpecimenID = U.CollectionSpecimenID " +
                "WHERE P.CollectionID = " + CollectionID.ToString() +
                "group by P.CollectionSpecimenID, P.SpecimenPartID, P.MaterialCategory, P.StorageLocation, P.PartSublabel, P.AccessionNumber, S.AccessionNumber, P.StorageLocation, E.LocalityDescription, E.CollectionDate " +
                "order by DisplayText" ;
            System.Data.DataTable dataTable = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL, DiversityWorkbench.Settings.ConnectionString, "CollectionSpecimenPart");
            foreach (System.Data.DataRow R in dataTable.Rows)
            {
                System.Drawing.Font F = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 8);
                DiversityCollection.HierarchyNode N = new HierarchyNode(R, true, F);
                N.ContextMenuStrip = ContextMenuStripPartNodes;
                pNode.Nodes.Add(N);
            }
        }

        public static string SubcollectionContentDisplayText(string AliasCollectionSpecimen = "S", string AliasCollectionSpecimenPart = "P", string AliasIdentificationUnit = "U", string AliasCollectionEvent = "E")
        {
            string SQL = "";
            if (AliasCollectionSpecimen.Length > 0 && DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Contains(DiversityCollection.Forms.FormCustomizeDisplay.SubcollectionContentDisplayText.AccessionNumber.ToString()))
                SQL += "case when " + AliasCollectionSpecimen + ".AccessionNumber is null then '' else " + AliasCollectionSpecimen + ".AccessionNumber + '; ' end + ";
            if (AliasCollectionSpecimenPart.Length > 0 && DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Contains(DiversityCollection.Forms.FormCustomizeDisplay.SubcollectionContentDisplayText.PartNumber.ToString()))
                SQL += "case when " + AliasCollectionSpecimenPart + ".AccessionNumber is null then '' else " + AliasCollectionSpecimenPart + ".AccessionNumber + '; ' end + ";
            if (AliasCollectionSpecimenPart.Length > 0 && DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Contains(DiversityCollection.Forms.FormCustomizeDisplay.SubcollectionContentDisplayText.PartSublabel.ToString()))
                SQL += "case when " + AliasCollectionSpecimenPart + ".PartSublabel is null then '' else " + AliasCollectionSpecimenPart + ".PartSublabel + '; ' end + ";
            if (AliasIdentificationUnit.Length > 0 && DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Contains(DiversityCollection.Forms.FormCustomizeDisplay.SubcollectionContentDisplayText.Identification.ToString()))
                SQL += "case when MIN(" + AliasIdentificationUnit + ".LastIdentificationCache) is null then '' else MIN(" + AliasIdentificationUnit + ".LastIdentificationCache) + '; ' end + ";
            if (AliasCollectionSpecimenPart.Length > 0 && DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Contains(DiversityCollection.Forms.FormCustomizeDisplay.SubcollectionContentDisplayText.StorageLocation.ToString()))
                SQL += "case when " + AliasCollectionSpecimenPart + ".StorageLocation is null then '' else " + AliasCollectionSpecimenPart + ".StorageLocation + '; ' end + ";
            if (AliasCollectionEvent.Length > 0 && DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Contains(DiversityCollection.Forms.FormCustomizeDisplay.SubcollectionContentDisplayText.Locality.ToString()))
                SQL += "CASE WHEN " + AliasCollectionEvent + ".LocalityDescription IS NULL  THEN '' ELSE " + AliasCollectionEvent + ".LocalityDescription + '; ' END + ";
            if (AliasCollectionEvent.Length > 0 && DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Contains(DiversityCollection.Forms.FormCustomizeDisplay.SubcollectionContentDisplayText.CollectionDate.ToString()))
                SQL += "CASE WHEN " + AliasCollectionEvent + ".CollectionDate IS NULL THEN '' ELSE CONVERT(nvarchar(50), " + AliasCollectionEvent + ".CollectionDate, 111) + '; ' END +  ";
            SQL += " '' AS DisplayText ";
            return SQL;
        }

        private System.Windows.Forms.ContextMenuStrip _contextMenuStripPartNodes;
        private System.Windows.Forms.ContextMenuStrip ContextMenuStripPartNodes
        {
            get
            {
                if (_contextMenuStripPartNodes == null)
                {
                    _contextMenuStripPartNodes = new System.Windows.Forms.ContextMenuStrip();
                    _contextMenuStripPartNodes.Items.Add("Copy", DiversityCollection.Resource.Copy1, ContextMenuStripPartNodes_onClick);
                }
                return _contextMenuStripPartNodes;
            }
        }

        private void ContextMenuStripPartNodes_onClick(object sender, EventArgs e)
        {
            if (this._TreeView.SelectedNode != null)
                System.Windows.Clipboard.SetText(this._TreeView.SelectedNode.Text.Trim());
        }

        private System.Data.DataTable _dtTask;


        public override void markHierarchyNodes()
        {
            try
            {
                //base.markHierarchyNodes();
                if (this._TreeView.ImageList == null)
                    this._TreeView.ImageList = DiversityCollection.Specimen.ImageList;

                foreach (System.Windows.Forms.TreeNode N in this._TreeView.Nodes)
                {
                    this.markHierarchyNode(N);
                    this.markHierachyChildNodes(N);
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void markHierachyChildNodes(System.Windows.Forms.TreeNode N)
        {
            try
            { 
                this.markHierarchyNode(N);
                foreach (System.Windows.Forms.TreeNode NC in N.Nodes)
                    this.markHierachyChildNodes(NC);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void markHierarchyNode(System.Windows.Forms.TreeNode N)
        {
            try
            {
                if (N.Tag != null && (N.Tag.GetType().BaseType == typeof(System.Data.DataRow) || N.Tag.GetType() == typeof(System.Data.DataRow)))
                {
                    System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                    if (R.Table.Columns.Contains("Type"))
                    {
                        string Type = R["Type"].ToString();
                        N.ImageIndex = DiversityCollection.Specimen.CollectionTypeImage(Type, false);
                        N.SelectedImageIndex = DiversityCollection.Specimen.CollectionTypeImage(Type, false);
                        int ID;
                        if (this.ID != null && int.TryParse(R["CollectionID"].ToString(), out ID) && ID == (int)this.ID)
                        {
                            N.BackColor = System.Drawing.Color.Yellow;
                            N.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

        }

        private bool _ShowTasks = false;
        public void SetTaskVisibility(bool ShowTasks, TaskDisplayStyle taskDisplay = TaskDisplayStyle.Default)
        {
            this._ShowTasks = ShowTasks;
            this._taskDisplayStyle = taskDisplay;
            this._TaskNodes = null;
        }

        private System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<System.Windows.Forms.TreeNode>> _TaskNodes;
        private System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<System.Windows.Forms.TreeNode>> TaskNodes()
        {
            if (_TaskNodes == null)
            {
                _TaskNodes = new Dictionary<int, List<System.Windows.Forms.TreeNode>>();
                string SQL = "SELECT case when H.CollectionHierarchyDisplayText like '%:%' then ltrim(rtrim(substring(H.CollectionHierarchyDisplayText, charindex(':', H.CollectionHierarchyDisplayText) + 1, 255))) " +
                    "else ltrim(rtrim(substring(H.CollectionHierarchyDisplayText, charindex('—', H.CollectionHierarchyDisplayText) + 1, 255))) " +
                    "end AS DisplayText, H.CollectionID, H.CollectionTaskID, T.Type FROM [dbo].[CollectionTaskHierarchyAll] () H  INNER JOIN Task T ON H.TaskID = T.TaskID " +
                    " ORDER BY H.CollectionHierarchyDisplayText";
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                foreach(System.Data.DataRow R in dt.Rows)
                {
                    string TaskType = R["Type"].ToString();
                    System.Windows.Forms.TreeNode NodeTask = new System.Windows.Forms.TreeNode(R["DisplayText"].ToString());
                    NodeTask.ImageIndex = DiversityCollection.Specimen.TaskTypeImage(TaskType, true);
                    NodeTask.SelectedImageIndex = DiversityCollection.Specimen.TaskTypeImage(TaskType, true);
                    NodeTask.ForeColor = System.Drawing.Color.Gray;
                    NodeTask.Tag = R["CollectionTaskID"].ToString();

                    int CollectionID = int.Parse(R["CollectionID"].ToString());
                    if (_TaskNodes.ContainsKey(CollectionID))
                    {
                        _TaskNodes[CollectionID].Add(NodeTask);
                    }
                    else
                    {
                        System.Collections.Generic.List<System.Windows.Forms.TreeNode> nodes = new List<System.Windows.Forms.TreeNode>();
                        nodes.Add(NodeTask);
                        _TaskNodes.Add(CollectionID, nodes);
                    }
                }
            }
            return _TaskNodes;
        }

        public enum TaskDisplayStyle { Default, RestrictToSameType }
        private TaskDisplayStyle _taskDisplayStyle = TaskDisplayStyle.Default;

        #endregion

        #endregion

    }
}
