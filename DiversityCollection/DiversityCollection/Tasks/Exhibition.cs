using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.Tasks
{
    public class Exhibition
    {

        #region Parameter

        private System.Data.DataTable _DtExhibition;
        private System.Data.DataTable _DtCollection;
        private System.Data.DataTable _DtPart;
        private System.Data.DataTable _DtPartsInCollection;
        private int _ID;
        private int? _SelectedCollectionID;

        #endregion

        #region Construction
        public Exhibition(int ID)
        {
            this.ID = ID;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The CollectionTaskID of the Exhibition
        /// </summary>
        public int ID 
        { 
            get { return _ID; }
            set 
            {
                this.initExhibition(value);
            }
        }

        /// <summary>
        /// The CollectionID of the exhibition if available
        /// </summary>
        public int? CollectionID
        {
            get
            {
                if (this._DtExhibition != null && this._DtExhibition.Rows.Count == 1)
                {
                    int id;
                    if (int.TryParse(this._DtExhibition.Rows[0]["CollectionID"].ToString(), out id))
                        return id;
                }
                return null;
            }
        }

        public int SelectedCollectionID
        {
            set { this._SelectedCollectionID = value; }
        }

        /// <summary>
        /// The display text of the exhibition
        /// </summary>
        public string DisplayText
        {
            get
            {
                if (this._DtExhibition != null && this._DtExhibition.Rows.Count == 1)
                {
                    return this._DtExhibition.Rows[0]["DisplayText"].ToString();
                }
                return "";
            }
        }

        /// <summary>
        /// The task ID of the exhibition
        /// </summary>
        public int? TaskID
        {
            get
            {
                if (this._DtExhibition != null && this._DtExhibition.Rows.Count == 1)
                {
                    int id;
                    if (int.TryParse(this._DtExhibition.Rows[0]["TaskID"].ToString(), out id))
                        return id;
                }
                return null;
            }
        }

        /// <summary>
        /// The task ID of parts within the exhibition
        /// </summary>
        public int? PartTaskID
        {
            get
            {
                if (this.TaskID != null)
                {
                    string SQL = "SELECT P.TaskID " +
                        "FROM Task AS E INNER JOIN " +
                        "Task AS P ON E.TaskID = P.TaskParentID " +
                        "AND (P.Type = N'part') AND E.TaskID = " + this.TaskID.ToString() +
                        " ORDER BY P.TaskID DESC ";
                    int id;
                    if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out id))
                        return id;
                }
                return null;
            }
        }

        #endregion

        #region Datahandling

        #region Exhibition

        /// <summary>
        /// Init the exhibition and all depending dataobjects
        /// </summary>
        /// <param name="ID">The ID of the exhibition</param>
        private void initExhibition(int ID)
        {
            this._ID = ID;
            // init tables
            if (this._DtExhibition == null) this._DtExhibition = new System.Data.DataTable();
            if (this._DtCollection == null) this._DtCollection = new System.Data.DataTable();
            if (this._DtPart == null) this._DtPart = new System.Data.DataTable();
            this._DtExhibition.Clear();
            this._DtCollection.Clear();
            this._DtPart.Clear();
            // Fill Exhibition table
            string SQL = "SELECT CASE WHEN CT.TaskStart IS NULL THEN '?' ELSE CONVERT(varchar(10), CT.TaskStart, 120) END " +
                " + CASE WHEN CT.TaskEnd IS NULL THEN '' ELSE ' - ' + CONVERT(varchar(10), CT.TaskEnd, 120) END " +
                " + CASE WHEN CT.DisplayText IS NULL THEN '' ELSE  ': ' + CT.DisplayText END AS DisplayText, CT.CollectionTaskID, CT.CollectionID, CT.TaskID " +
                "FROM CollectionTask AS CT " +
                "WHERE CT.CollectionTaskID = " + ID.ToString();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref _DtExhibition);
        }

        #endregion

        #region Parts

        #region insert

        /// <summary>
        /// Inserting parts into an exhibition
        /// </summary>
        /// <returns>If insert was successful</returns>
        public bool InsertParts()//int CollectionID, System.Collections.Generic.List<int> PartIDs)
        {
            bool OK = false;
            string Header = "If available\r\nplease select start and end date\r\nof the transfer to the exhibition";
            DiversityWorkbench.Forms.FormGetDate formGetDate = new DiversityWorkbench.Forms.FormGetDate(false, true, true, null, null, "Transfer period", Header);
            formGetDate.ShowDialog();
            if (formGetDate.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                bool startSelected = formGetDate.DateSelected;
                bool endSelected = formGetDate.DateEndSelected;
                System.DateTime start = formGetDate.Date;
                System.DateTime end = formGetDate.EndDate;
                if (end < start && startSelected && endSelected)
                {
                    System.Windows.Forms.MessageBox.Show("The transfer to the exhibition can not end\r\n(" + end.ToString("yyyy-MM-dd") + ")\r\nbefore it started\r\n(" + start.ToString("yyyy-MM-dd") + ")");
                }
                else
                {
                    int? ColID = this.CollectionIDForNewParts();
                    if (ColID != null)
                    {
                        DiversityCollection.Forms.FormCollectionSpecimen f = new Forms.FormCollectionSpecimen(true, "Please select the parts for the exhibition");
                        f.ShowDialog();
                        if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            bool TakeAll = true;
                            System.Collections.Generic.List<int> IDs = f.IDs;
                            if (IDs.Count > 1)
                            {
                                switch (System.Windows.Forms.MessageBox.Show("The selection contains " + IDs.Count.ToString() + ".\r\nDo you want to insert only the selected part?\r\nIf you want to insert all " + IDs.Count.ToString() + " specimen please choose [no]\r\nTo cancel the insert, choose [cancel]", "Restrict to selected specimen?", System.Windows.Forms.MessageBoxButtons.YesNoCancel))
                                {
                                    case System.Windows.Forms.DialogResult.Yes:
                                        TakeAll = false;
                                        break;
                                    case System.Windows.Forms.DialogResult.No:
                                        TakeAll = true;
                                        break;
                                    case System.Windows.Forms.DialogResult.Cancel:
                                        System.Windows.Forms.MessageBox.Show("Insert canceled");
                                        return false;
                                }
                            }
                            else
                            {
                                if (System.Windows.Forms.MessageBox.Show("Insert " + IDs.Count.ToString() + " specimen and their parts?") != System.Windows.Forms.DialogResult.OK)
                                    return false;
                            }
                            int? TaskID = this.PartTaskID;
                            if (TaskID != null)
                            {
                                System.Collections.Generic.Dictionary<int, string> NotTransferredParts = new System.Collections.Generic.Dictionary<int, string>();
                                string ids = "";
                                if (TakeAll)
                                {
                                    foreach (int i in IDs)
                                    {
                                        if (ids.Length > 0) ids += ", ";
                                        ids += i.ToString();
                                        this.CheckIfPartIsInCollection(ref NotTransferredParts, (int)ColID, i);
                                    }
                                }
                                else
                                {
                                    ids = f.selectedIDs[0].ToString();
                                    this.CheckIfPartIsInCollection(ref NotTransferredParts, (int)ColID, f.selectedIDs[0]);
                                }
                                string SQL = "INSERT INTO CollectionTask " +
                                    "(CollectionTaskParentID, CollectionID, TaskID, DisplayText, CollectionSpecimenID, SpecimenPartID";
                                if (startSelected) SQL += ", TaskStart";
                                if (endSelected) SQL += ", TaskEnd";
                                SQL += ", ResponsibleAgent, ResponsibleAgentURI) " +
                                    "SELECT " + this._ID.ToString() + ", " + ColID.ToString() + ", " + TaskID.ToString() + ", " +
                                    "CASE WHEN P.AccessionNumber <> '' THEN P.AccessionNumber ELSE CASE WHEN S.AccessionNumber <> '' THEN S.AccessionNumber ELSE '' END END " +
                                    "+ CASE WHEN P.PartSublabel <> '' THEN ' ' + P.PartSublabel ELSE '' END " +
                                    "+ ': ' + P.StorageLocation + ' - ' " +
                                    "+ P.MaterialCategory AS DisplayText, P.CollectionSpecimenID, P.SpecimenPartID ";
                                if (startSelected) SQL += ", CONVERT(DATETIME, '" + start.ToString("yyyy-MM-dd") + " 00:00:00', 102)";
                                if (endSelected) SQL += ", CONVERT(DATETIME, '" + end.ToString("yyyy-MM-dd") + " 00:00:00', 102)";
                                if (Specimen.DefaultUseTaskResponsible)
                                    SQL += ", '" + Specimen.DefaultResponsibleName + "', '" + Specimen.DefaultResponsibleURI + "' ";
                                else
                                    SQL += ", '', '' ";
                                SQL += "FROM CollectionSpecimenPart AS P INNER JOIN " +
                                    "CollectionSpecimen AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID " +
                                    "WHERE S.CollectionSpecimenID IN (" + ids + ")";
                                if (NotTransferredParts.Count > 0)
                                {
                                    SQL += " AND P.SpecimenPartID NOT IN (";
                                    string Message = "The following parts where not transferred as they are already placed in the selected collection:\r\n";
                                    foreach(System.Collections.Generic.KeyValuePair<int, string> kvp in NotTransferredParts)
                                    {
                                        Message += "\r\n" + kvp.Value;
                                        if (!SQL.EndsWith("(")) 
                                            SQL += ", ";
                                        SQL += kvp.Key.ToString();
                                    }
                                    SQL += ") ";
                                    System.Windows.Forms.MessageBox.Show(Message);
                                }
                                OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                            }
                        }
                    }
                }
            }
            return OK;
        }

        private void CheckIfPartIsInCollection(ref System.Collections.Generic.Dictionary<int, string> NotTransferredParts, int ExhibitionCollectionID, int CollectionSpecimenID)
        {
            try
            {
                string SQL = "SELECT CASE WHEN P.AccessionNumber <> '' THEN P.AccessionNumber ELSE CASE WHEN S.AccessionNumber <> '' THEN S.AccessionNumber ELSE '' END END " +
                "+ CASE WHEN P.PartSublabel <> '' THEN ' ' + P.PartSublabel ELSE '' END " +
                "+ ': ' + P.StorageLocation + ' - ' " +
                "+ P.MaterialCategory AS DisplayText, P.SpecimenPartID " +
                "FROM CollectionSpecimenPart AS P INNER JOIN " +
                "CollectionSpecimen AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID " +
                "WHERE S.CollectionSpecimenID = " + CollectionSpecimenID.ToString() + " AND P.CollectionID = " + ExhibitionCollectionID.ToString();
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                foreach (System.Data.DataRow dataRow in dt.Rows)
                {
                    int PartID;
                    if (int.TryParse(dataRow["SpecimenPartID"].ToString(), out PartID))
                    {
                        if (!NotTransferredParts.ContainsKey(PartID))
                        {
                            NotTransferredParts.Add(PartID, dataRow["DisplayText"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// The collection ID for the parts
        /// </summary>
        /// <returns>The search for ID if successfull otherwise NULL</returns>
        public int? CollectionIDForNewParts()
        {
            int ID = 0;
            if (this.CollectionID != null)
            {
                System.Data.DataTable dt = this.CollectionListForNewParts();
                if (dt.Rows.Count == 1)
                    return int.Parse(dt.Rows[0]["CollectionID"].ToString());
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "DisplayText", "CollectionID", "Collection", "Please select the collection of the exhibition", "", false, true, false, DiversityCollection.Resource.Collection);
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    if (int.TryParse(f.SelectedValue, out ID))
                        return ID;
                }
            }
            return null;
        }

        /// <summary>
        /// Collection list for new parts for the current exhibition where the collections are children of the collection of the exhibition
        /// </summary>
        /// <returns>A datatable containing the potential collections</returns>
        private System.Data.DataTable CollectionListForNewParts()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            if (this.CollectionID != null)
            {
                string SQL = "SELECT C.DisplayText, C.CollectionID FROM [dbo].[CollectionHierarchyAll] () C WHERE C.CollectionID = " + CollectionID.ToString() +
                    " UNION SELECT H.DisplayText, H.CollectionID FROM [dbo].[CollectionHierarchyAll] () H  INNER JOIN [dbo].[CollectionChildNodes] (" + CollectionID.ToString() + ") C ON H.CollectionID = C.CollectionID " +
                    " WHERE C.Type not in ('trap', 'freezer', 'fridge', 'hardware', 'radioactive', 'steel locker') and  C.Type not like 'box%' " +
                    " ORDER BY DisplayText ";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            }
            return dt;
        }

        #endregion

        #region delete

        /// <summary>
        /// Removing a part from an exhibition
        /// </summary>
        /// <param name="CollectionTaskID">The ID of the part</param>
        /// <param name="Error">The messeage if anything goes wrong</param>
        /// <returns>If the removal was successful</returns>
        public bool DeletePart(int CollectionTaskID, ref string Error)
        {
            bool OK = false;
            string SQL = "DELETE FROM CollectionTaskID WHERE CollectionTaskID = " + CollectionTaskID.ToString() + " AND CollectionTaskParentID = " + this.ID.ToString();
            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
            return OK;
        }

        #endregion

        #region Display

        public System.Data.DataTable Parts()
        {
            if (_DtPart == null)
                _DtPart = new System.Data.DataTable();
            if (_DtPart.Rows.Count == 0)
            {
                string SQL = "SELECT EP.CollectionTaskID, P.CollectionSpecimenID, P.SpecimenPartID, " +
                    "CASE WHEN P.AccessionNumber <> '' THEN P.AccessionNumber ELSE CASE WHEN S.AccessionNumber <> '' THEN S.AccessionNumber ELSE '' END END " +
                    "+ CASE WHEN P.PartSublabel <> '' THEN ' ' + P.PartSublabel ELSE '' END " +
                    "+ ': ' + P.StorageLocation + ' - ' " +
                    "+ P.MaterialCategory AS DisplayText, " +
                    "CASE WHEN P.AccessionNumber <> '' THEN P.AccessionNumber ELSE CASE WHEN S.AccessionNumber <> '' THEN S.AccessionNumber ELSE '' END END + CASE WHEN P.PartSublabel <> '' THEN ' - ' + P.PartSublabel ELSE '' END AS AccessionNumber, " +
                    " P.MaterialCategory, P.StorageLocation, EP.CollectionID " +
                    "FROM CollectionTask AS EP INNER JOIN " +
                    "CollectionTask AS E ON EP.CollectionTaskParentID = E.CollectionTaskID INNER JOIN " +
                    "CollectionSpecimenPart AS P ON P.SpecimenPartID = EP.SpecimenPartID AND P.CollectionSpecimenID = EP.CollectionSpecimenID INNER JOIN " +
                    "CollectionSpecimen AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID " +
                    "WHERE E.CollectionTaskID = " + ID.ToString();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref _DtPart);
            }
            return _DtPart;
        }

        public System.Data.DataTable Parts(int CollectionID)
        {
            if (_DtPart == null)
                _DtPart = new System.Data.DataTable();
            if (_DtPart != null && _DtPart.Columns.Count > 0)
            {
                //System.Data.DataTable dataTable = this._DtPart.Copy();
                //foreach(System.Data.DataRow R in dataTable.Rows)
                //{
                //    if (R["CollectionID"].ToString() != CollectionID.ToString())
                //        R.Delete();
                //}
                //dataTable.AcceptChanges();

                System.Data.DataView dataView = new System.Data.DataView(_DtPart, "CollectionID = " + CollectionID.ToString(), "DisplayText", System.Data.DataViewRowState.OriginalRows);
                System.Data.DataTable dt = dataView.ToTable();
                return dt;
            }
            else
                return null;
        }

        public string OriginalCollection(int PartID)
        {
            string Collection = "";
            string SQL = "SELECT P.CollectionID " +
                "FROM CollectionSpecimenPart AS P " +
                "WHERE P.SpecimenPartID = " + PartID.ToString();
            string sCollID = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
            int iCollID;
            if (sCollID.Length > 0 && int.TryParse(sCollID, out iCollID))
                Collection = LookupTable.CollectionNameHierarchy(iCollID);
            return Collection;
        }

        #region Units
        public System.Data.DataTable Units(int PartID)
        {
            System.Data.DataTable dtUnit = new System.Data.DataTable();
                string SQL = "SELECT U.LastIdentificationCache " +
                    "FROM CollectionTask AS EP INNER JOIN " +
                    "CollectionTask AS E ON EP.CollectionTaskParentID = E.CollectionTaskID INNER JOIN " +
                    "CollectionSpecimenPart AS P INNER JOIN " +
                    "CollectionSpecimen AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID INNER JOIN " +
                    "IdentificationUnit AS U ON S.CollectionSpecimenID = U.CollectionSpecimenID INNER JOIN " +
                    "IdentificationUnitInPart AS I ON P.CollectionSpecimenID = I.CollectionSpecimenID AND P.SpecimenPartID = I.SpecimenPartID AND U.CollectionSpecimenID = I.CollectionSpecimenID AND " +
                    "U.IdentificationUnitID = I.IdentificationUnitID ON EP.CollectionSpecimenID = P.CollectionSpecimenID AND EP.SpecimenPartID = P.SpecimenPartID " +
                    "WHERE E.CollectionTaskID = " + ID.ToString() + " AND P.SpecimenPartID = " + PartID.ToString();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtUnit);
            return dtUnit;
        }

        #endregion

        #endregion

        #endregion

        #region Collection 

        #region Display

        public System.Data.DataTable Collections()
        {
            string SQL = "SELECT DISTINCT CH.CollectionID, CH.DisplayText, CH.LocationPlan, CH.LocationPlanWidth, CH.Type " +
                "FROM CollectionTask AS E INNER JOIN " +
                "CollectionTask AS P ON E.CollectionTaskID = P.CollectionTaskParentID INNER JOIN " +
                "Task AS TE ON E.TaskID = TE.TaskID INNER JOIN " +
                "Task AS TP ON P.TaskID = TP.TaskID AND TE.TaskID = TP.TaskParentID INNER JOIN " +
                "dbo.CollectionHierarchyAll() AS CH ON P.CollectionID = CH.CollectionID " +
                "WHERE (TP.Type = N'Part') AND (TE.Type = N'Exhibition') AND E.CollectionTaskID = " + ID.ToString() +
                " UNION SELECT NULL, '' AS DisplayText, '', NULL, '' ORDER BY DisplayText";
            this._DtCollection = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._DtCollection);
            return this._DtCollection;
        }

        #endregion

        #region Parts in collection

        public System.Data.DataTable PartsInCollection(int CollectionID)
        {
            if (this._DtPartsInCollection == null)
                _DtPartsInCollection = new System.Data.DataTable();
            this._DtPartsInCollection.Clear();
                string SQL = "SELECT P.CollectionSpecimenID, P.SpecimenPartID, " +
                    "CASE WHEN P.AccessionNumber <> '' THEN P.AccessionNumber ELSE CASE WHEN S.AccessionNumber <> '' THEN S.AccessionNumber ELSE '' END END " +
                    "+ CASE WHEN P.PartSublabel <> '' THEN ' ' + P.PartSublabel ELSE '' END " +
                    "+ ': ' + P.StorageLocation + ' - ' " +
                    "+ P.MaterialCategory AS DisplayText, " +
                    "CASE WHEN P.AccessionNumber <> '' THEN P.AccessionNumber ELSE CASE WHEN S.AccessionNumber <> '' THEN S.AccessionNumber ELSE '' END END + CASE WHEN P.PartSublabel <> '' THEN ' - ' + P.PartSublabel ELSE '' END AS AccessionNumber, " +
                    " P.MaterialCategory, P.StorageLocation " +
                    "FROM CollectionSpecimenPart AS P INNER JOIN " +
                    "CollectionSpecimen AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID " +
                    "WHERE P.CollectionID = " + CollectionID.ToString();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref _DtPartsInCollection);
            return _DtPartsInCollection;
        }


        #endregion

        #endregion


        #endregion

        #region static functions

        #region Display and lookup
        /// <summary>
        /// The list for all exhibitions in the database
        /// </summary>
        /// <returns>Datatable containing the exhibitions</returns>
        public static System.Data.DataTable ExhibitionList()
        {
            string SQL = "SELECT CASE WHEN CT.TaskStart IS NULL THEN '?' ELSE CONVERT(varchar(10), CT.TaskStart, 120) END " +
                " + CASE WHEN CT.TaskEnd IS NULL THEN '' ELSE ' - ' + CONVERT(varchar(10), CT.TaskEnd, 120) END " +
                " + CASE WHEN CT.DisplayText IS NULL THEN '' ELSE  ': ' + CT.DisplayText END AS DisplayText, CT.CollectionTaskID, CT.CollectionID " +
                "FROM CollectionTask AS CT INNER JOIN " +
                "Task AS T ON CT.TaskID = T.TaskID " +
                "WHERE (T.Type = N'exhibition') " +
                "ORDER BY DisplayText";
            System.Data.DataTable dtExhibition = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtExhibition);
            return dtExhibition;
        }

        /// <summary>
        /// The list of collections where exhibitions may be placed
        /// </summary>
        /// <returns>A datatable containg the potential collections</returns>
        private static System.Data.DataTable CollectionListForExhibition()
        {
            string SQL = "SELECT C.DisplayText, C.CollectionID FROM [dbo].[CollectionHierarchyAll] () C " +
                "WHERE C.Type in ('institution', 'department', 'room', 'location', 'collection') " +
                "ORDER BY C.DisplayText ";
            System.Data.DataTable dt = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            return dt;
        }

        #endregion

        #region Datahandling for exhibition

        /// <summary>
        /// Insert a new exhibtion
        /// </summary>
        /// <returns>If successful the ID of the exhibition, otherwise null</returns>
        public static int? InsertExhibition()
        {
            int CollectionTaskID = 0;

            System.DateTime start;
            System.DateTime end;
            string Title;
            int CollectionID;
            DiversityWorkbench.Forms.FormGetDate formGetDate = new DiversityWorkbench.Forms.FormGetDate(false, true, true, null, null, "Please select start and end date of the exhibition");
            formGetDate.ShowDialog();
            if (formGetDate.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                bool startSelected = formGetDate.DateSelected;
                bool endSelected = formGetDate.DateEndSelected;
                start = formGetDate.Date;
                end = formGetDate.EndDate;
                if (end < start && startSelected && endSelected)
                {
                    System.Windows.Forms.MessageBox.Show("The exhibition can not end\r\n(" + end.ToString("yyyy-MM-dd") + ")\r\nbefore it started\r\n(" + start.ToString("yyyy-MM-dd") + ")");
                }
                else
                {
                    DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Exhibition title", "Please enter the title of the exhibition", "Exhibition", DiversityCollection.Resource.Exhibition);
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        Title = f.String;
                        System.Data.DataTable dt = CollectionListForExhibition();
                        DiversityWorkbench.Forms.FormGetStringFromList form = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "DisplayText", "CollectionID", "Collection", "Please select the collection where the exhibition takes place", "", false, true, true, DiversityCollection.Resource.Collection);
                        form.ShowDialog();
                        if (form.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            if (int.TryParse(form.SelectedValue, out CollectionID))
                            {
                                int ExhibitionTaskID = 0;
                                int PartTaskID = 0;
                                // Check if a valid task exists and create one if missing
                                if (GetTaskIDs(ref ExhibitionTaskID, ref PartTaskID))
                                {
                                    // insert the collection task
                                    string SQL = "INSERT INTO CollectionTask " +
                                        "(CollectionID, TaskID, DisplayText";
                                    if (startSelected) SQL += ", TaskStart";
                                    if (endSelected) SQL += ", TaskEnd";
                                    SQL += ") " +
                                        "VALUES(" + CollectionID.ToString() + ", " + ExhibitionTaskID.ToString() + ", '" + Title + "'";
                                    if (startSelected) SQL += ", CONVERT(DATETIME, '" + start.ToString("yyyy-MM-dd") + " 00:00:00', 102)";
                                    if (endSelected) SQL += ", CONVERT(DATETIME, '" + end.ToString("yyyy-MM-dd") + " 00:00:00', 102)";
                                    SQL += "); " +
                                        "SELECT MAX(CollectionTaskID) FROM CollectionTask;";
                                    if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out CollectionTaskID))
                                        return CollectionTaskID;
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Delete an exhibition form the database
        /// </summary>
        /// <param name="ID">The ID of the exhibition that should be deleted</param>
        /// <param name="Exception">The error message if anything goes wrong</param>
        /// <returns>If the exhibition was removed</returns>
        public static bool DeleteExhibition(int ID, ref string Exception)
        {
            bool OK = false;

            string SQL = "DELETE FROM [dbo].[CollectionTask] WHERE CollectionTaskParentID = " + ID.ToString();
            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Exception))
            {
                SQL = "DELETE FROM [dbo].[CollectionTask] WHERE CollectionTaskID = " + ID.ToString();
                OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Exception);
            }
            return OK;
        }

        /// <summary>
        /// Receiving the basic ID - or if missing, create these
        /// </summary>
        /// <param name="ExhibitionTaskID">The task ID for the type exhibition</param>
        /// <param name="PartTaskID">The TaskID for the part depending on the exhibition</param>
        /// <returns></returns>
        public static bool GetTaskIDs(ref int ExhibitionTaskID, ref int PartTaskID)
        {
            bool OK = false;
            string SQL = "SELECT  TOP 1  E.TaskID, P.TaskID AS PartTaskID " +
                "FROM Task AS E INNER JOIN " +
                "Task AS P ON E.TaskID = P.TaskParentID " +
                "AND (E.Type = N'exhibition') AND (P.Type = N'part') " +
                "ORDER BY E.TaskID DESC ";
            System.Data.DataTable dtTask = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTask);
            if (dtTask.Rows.Count == 0)
            {
                SQL = "INSERT INTO Task (DisplayText, Type, DateType, DateBeginType, DateEndType) " +
                    "VALUES('Exhibition', 'Exhibition', 'Date from to', 'from', 'until')";
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                {
                    SQL = "SELECT  TOP 1  E.TaskID " +
                        "FROM Task AS E " +
                        "WHERE (E.Type = N'exhibition') " +
                        "ORDER BY E.TaskID DESC ";
                    if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out ExhibitionTaskID))
                    {
                        SQL = "INSERT INTO Task (TaskParentID, DisplayText, Type, SpecimenPartType, DateType, DateBeginType, DateEndType) " +
                            "VALUES(" + ExhibitionTaskID.ToString() + ", 'Part', 'Part', 'Part', 'Date from to', 'from', 'until')";
                        if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                        {
                            SQL = "SELECT TOP 1  E.TaskID " +
                                "FROM Task AS E " +
                                "WHERE (E.Type = N'Part') AND TaskParentID = " + ExhibitionTaskID.ToString() +
                                " ORDER BY E.TaskID DESC ";
                            if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out PartTaskID))
                            {
                                OK = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (!int.TryParse(dtTask.Rows[0][0].ToString(), out ExhibitionTaskID) || !int.TryParse(dtTask.Rows[0][1].ToString(), out PartTaskID))
                    OK = false;
                else
                    OK = true;
            }
            return OK;
        }

        #endregion

        #region obsolete
        public static System.Data.DataTable GetSpecimen()
        {
            System.Data.DataTable dtSpecimen = new System.Data.DataTable();
            DiversityCollection.Forms.FormCollectionSpecimen f = new Forms.FormCollectionSpecimen(true);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                string SQL = "";
                foreach (int ID in f.IDs)
                {
                    if (SQL.Length > 0) SQL += ", ";
                    SQL += ID.ToString();
                }
                SQL = "SELECT P.CollectionSpecimenID, P.SpecimenPartID, S.AccessionNumber, P.AccessionNumber AS AccessionNumberPart, P.PartSublabel, P.MaterialCategory, P.StorageLocation, U.LastIdentificationCache " +
                    "FROM CollectionSpecimenPart AS P INNER JOIN " +
                    "CollectionSpecimen AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID INNER JOIN " +
                    "IdentificationUnit AS U ON S.CollectionSpecimenID = U.CollectionSpecimenID " +
                    "WHERE P.CollectionSpecimenID IN (" + SQL + ")";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtSpecimen);
            }
            return dtSpecimen;
        }

        public static System.Collections.Generic.List<int> GetSpecimenIDs()
        {
            System.Collections.Generic.List<int> list = new List<int>();
            DiversityCollection.Forms.FormCollectionSpecimen f = new Forms.FormCollectionSpecimen(true);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                list = f.IDs;
            }
            return list;
        }

        #endregion

        #endregion

    }
}
