using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;


namespace DiversityCollection.Forms
{
    public struct AnalysisEntry
    {
        public int AnalysisID;
        public string AnalysisType;
    }

    public struct ProcessingEntry
    {
        public int ProcessingID;
        public string ProcessingType;
    }

    public class FormGridFunctions
    {

        #region Parameter

        private GridType _gridType;
        private System.Windows.Forms.TreeView _treeView;
        private System.Windows.Forms.DataGridView _dataGridView;

        #endregion

        #region Construction

        public FormGridFunctions(GridType gridType, System.Windows.Forms.TreeView treeView, System.Windows.Forms.DataGridView dataGridView)
        {
            this._gridType = gridType;
            this._treeView = treeView;
            this._dataGridView = dataGridView;
        }

        #endregion

        #region STATIC

        #region CellStyles

        public enum ReplaceOptionState { Replace, Insert, Append, Clear };
        public enum FormState { Loading, Editing };
        
        private static System.Windows.Forms.DataGridViewCellStyle _StyleBlocked;
        private static System.Windows.Forms.DataGridViewCellStyle _StyleUnblocked;
        private static System.Windows.Forms.DataGridViewCellStyle _StyleReadOnly;
        private static System.Windows.Forms.DataGridViewCellStyle _StyleRemove;

        public static System.Windows.Forms.DataGridViewCellStyle StyleBlocked
        {
            get
            {
                if (FormGridFunctions._StyleBlocked == null)
                {
                    FormGridFunctions._StyleBlocked = new DataGridViewCellStyle();
                    FormGridFunctions._StyleBlocked.BackColor = System.Drawing.Color.Yellow;
                    FormGridFunctions._StyleBlocked.SelectionBackColor = System.Drawing.Color.Yellow;
                    FormGridFunctions._StyleBlocked.ForeColor = System.Drawing.Color.Blue;
                    FormGridFunctions._StyleBlocked.SelectionForeColor = System.Drawing.Color.Blue;
                    FormGridFunctions._StyleBlocked.Tag = "Blocked";
                }
                return FormGridFunctions._StyleBlocked;
            }
        }

        public static System.Windows.Forms.DataGridViewCellStyle StyleUnblocked
        {
            get
            {
                if (FormGridFunctions._StyleUnblocked == null)
                {
                    FormGridFunctions._StyleUnblocked = new DataGridViewCellStyle();
                    FormGridFunctions._StyleUnblocked.BackColor = System.Drawing.SystemColors.Window;
                    FormGridFunctions._StyleUnblocked.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                    FormGridFunctions._StyleUnblocked.ForeColor = System.Drawing.Color.Black;
                    FormGridFunctions._StyleUnblocked.SelectionForeColor = System.Drawing.SystemColors.Window;
                    FormGridFunctions._StyleUnblocked.Tag = "";
                    // Allow AutoComplete
                    FormGridFunctions._StyleUnblocked.WrapMode = DataGridViewTriState.False;
                }
                return FormGridFunctions._StyleUnblocked;
            }
        }

        public static System.Windows.Forms.DataGridViewCellStyle StyleReadOnly
        {
            get
            {
                if (FormGridFunctions._StyleReadOnly == null)
                {
                    FormGridFunctions._StyleReadOnly = new DataGridViewCellStyle();
                    FormGridFunctions._StyleReadOnly.BackColor = System.Drawing.Color.LightGray;
                    FormGridFunctions._StyleReadOnly.SelectionBackColor = System.Drawing.Color.LightGray;
                    FormGridFunctions._StyleReadOnly.ForeColor = System.Drawing.Color.Black;
                    FormGridFunctions._StyleReadOnly.SelectionForeColor = System.Drawing.Color.Black;
                    FormGridFunctions._StyleReadOnly.Tag = "ReadOnly";
                }
                return FormGridFunctions._StyleReadOnly;
            }
        }

        public static System.Windows.Forms.DataGridViewCellStyle StyleRemove
        {
            get
            {
                if (FormGridFunctions._StyleRemove == null)
                {
                    FormGridFunctions._StyleRemove = new DataGridViewCellStyle();
                    FormGridFunctions._StyleRemove.BackColor = System.Drawing.Color.Red;
                    FormGridFunctions._StyleRemove.SelectionBackColor = System.Drawing.Color.Red;
                    FormGridFunctions._StyleRemove.ForeColor = System.Drawing.Color.Red;
                    FormGridFunctions._StyleRemove.SelectionForeColor = System.Drawing.Color.Red;
                    FormGridFunctions._StyleRemove.Tag = "Remove";
                }
                return FormGridFunctions._StyleRemove;
            }
        }

        #endregion

        #region GridFields

        public enum GridType { Event, Series, Specimen, Images, Unit, Part }

        private static System.Collections.Generic.Dictionary<GridType, System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField>> _GridModeQueryFieldDict;

        public static System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> GridModeQueryFields(System.Windows.Forms.TreeView treeView, GridType gridType)
        {
            if (_GridModeQueryFieldDict == null)
                _GridModeQueryFieldDict = new Dictionary<GridType, List<GridModeQueryField>>();
            if (_GridModeQueryFieldDict.ContainsKey(gridType))
                return _GridModeQueryFieldDict[gridType];

            #region obsolet
            //{
            //    foreach (System.Windows.Forms.TreeNode N in treeView.Nodes)
            //    {
            //        if (N.Tag != null && N.Tag.ToString().Contains(";"))
            //        {
            //            try
            //            {
            //                string[] Parameter = this.GridModeFieldTagArray(N.Tag.ToString());
            //                if (Parameter.Length > 1)
            //                {
            //                    DiversityCollection.Forms.GridModeQueryField Q = new Forms.GridModeQueryField();
            //                    if (Parameter.Length < 4) continue;

            //                    Q.Table = Parameter[0];

            //                    Q.AliasForTable = Parameter[1];
            //                    if (Q.AliasForTable.Length == 0) Q.AliasForTable = Q.Table;

            //                    Q.Restriction = Parameter[2];

            //                    Q.Column = Parameter[3];

            //                    if (Parameter.Length > 4)
            //                        Q.AliasForColumn = Parameter[4];
            //                    else Q.AliasForColumn = "";
            //                    if (Q.AliasForColumn.Length == 0)
            //                        Q.AliasForColumn = Q.Column;

            //                    if (Parameter.Length > 5)
            //                        Q.DatasourceTable = Parameter[5];
            //                    else Q.DatasourceTable = "";

            //                    if (N.Checked)
            //                        Q.IsVisible = true;
            //                    else Q.IsVisible = false;
            //                    Q.IsHidden = false;
            //                    if (Parameter.Length > 7)
            //                        Q.RemoveLinkColumn = Parameter[6];
            //                    else Q.RemoveLinkColumn = "";
            //                    _GridModeQueryFields.Add(Q);
            //                }

            //            }
            //            catch { }
            //        }
            //        else if (N.Tag != null)
            //        {
            //        }
            //        this.GridModeQueryFieldsAddChildNodes(N);
            //    }
            //    foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeHiddenQueryFields)
            //        this._GridModeQueryFields.Add(Q);
            //}
            //string Visibility = "";
            //try
            //{
            //    if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeUnitVisibility.Length == 0)
            //    {
            //        foreach (DiversityCollection.Forms.GridModeQueryField Q in this._GridModeQueryFields)
            //        {
            //            if (!Q.IsHidden)
            //            {
            //                if (Q.IsVisible) Visibility += "1";
            //                else Visibility += "0";
            //            }
            //        }
            //        DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeUnitVisibility = Visibility;
            //        DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
            //    }
            //    else
            //    {
            //        Visibility = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.GridModeUnitVisibility;
            //        for (int i = 0; i < this._GridModeQueryFields.Count; i++)
            //        {
            //            bool IsVisible = false;
            //            if (Visibility.Substring(0, 1) == "1") IsVisible = true;
            //            this.GridModeSetIsVisibleForQueryField(i, IsVisible);
            //            if (Visibility.Length > 1)
            //                Visibility = Visibility.Substring(1);
            //            else break;
            //        }
            //    }

            //}
            //catch { }
            #endregion
            if (!_GridModeQueryFieldDict.ContainsKey(gridType))
            {
                System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> _GridModeQueryFields = new List<GridModeQueryField>();
                foreach (System.Windows.Forms.TreeNode N in treeView.Nodes)
                    GridModeQueryFields(N, ref _GridModeQueryFields);
                _GridModeQueryFieldDict.Add(gridType, _GridModeQueryFields);
            }
            if (_GridModeQueryFieldDict.ContainsKey(gridType))
                return _GridModeQueryFieldDict[gridType];
            return null;
        }

        private static void GridModeQueryFields(System.Windows.Forms.TreeNode treeNode, ref System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> gridModeQueryFields)
        {
            foreach(System.Windows.Forms.TreeNode childNode in treeNode.Nodes)
            {
                GridModeQueryField gridModeQueryField = GridModeQueryFieldOfNode(childNode);
                gridModeQueryFields.Add(gridModeQueryField);
                if (childNode.Nodes.Count > 0)
                {
                    foreach (System.Windows.Forms.TreeNode node in childNode.Nodes)
                        GridModeQueryFields(node, ref gridModeQueryFields);
                }
            }
        }

        public static void getDataColumn(System.Windows.Forms.TreeView treeView, GridType gridType, string ColumnAlias, ref string TableName, ref string ColumnName)
        {
            try
            {
                System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> gridModeQueryFields = GridModeQueryFields(treeView, gridType);
                if (gridModeQueryFields != null)
                {
                    foreach (DiversityCollection.Forms.GridModeQueryField field in gridModeQueryFields)
                    {
                        if (field.AliasForColumn == ColumnAlias)
                        {
                            TableName = field.Table;
                            ColumnName = field.Column;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex) { System.Windows.Forms.MessageBox.Show(ex.Message); }
        }

        #endregion

        #region Visibility

        /// <summary>
        /// the grid mode field as specified in the treeViewData
        /// </summary>
        public static System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> GridModeQueryFields(
            ref System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> _GridModeQueryFields,
            System.Windows.Forms.TreeView treeViewGridModeFieldSelector,
            System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> GridModeHiddenQueryFields,
            ref string ColumnVisibility)
        {
            if (_GridModeQueryFields == null)
            {
                _GridModeQueryFields = new List<GridModeQueryField>();
                foreach (System.Windows.Forms.TreeNode N in treeViewGridModeFieldSelector.Nodes)
                {
                    if (N.Tag != null && N.Tag.ToString().Contains(";"))
                    {
                        try
                        {
                            string[] Parameter = GridModeFieldTagArray(N.Tag.ToString());
                            if (Parameter.Length > 1)
                            {
                                DiversityCollection.Forms.GridModeQueryField Q = new Forms.GridModeQueryField();
                                if (Parameter.Length < 4) continue;

                                Q.Table = Parameter[0];

                                Q.AliasForTable = Parameter[1];
                                if (Q.AliasForTable.Length == 0) Q.AliasForTable = Q.Table;

                                Q.Restriction = Parameter[2];

                                Q.Column = Parameter[3];

                                if (Parameter.Length > 4)
                                    Q.AliasForColumn = Parameter[4];
                                else Q.AliasForColumn = "";
                                if (Q.AliasForColumn.Length == 0)
                                    Q.AliasForColumn = Q.Column;

                                if (Parameter.Length > 5)
                                    Q.DatasourceTable = Parameter[5];
                                else Q.DatasourceTable = "";

                                if (N.Checked)
                                    Q.IsVisible = true;
                                else Q.IsVisible = false;
                                Q.IsHidden = false;
                                if (Parameter.Length > 7)
                                    Q.RemoveLinkColumn = Parameter[6];
                                else Q.RemoveLinkColumn = "";
                                _GridModeQueryFields.Add(Q);
                            }

                        }
                        catch { }
                    }
                    else if (N.Tag != null)
                    {
                    }
                    GridModeQueryFieldsAddChildNodes(N, ref _GridModeQueryFields);
                }
                foreach (DiversityCollection.Forms.GridModeQueryField Q in GridModeHiddenQueryFields)
                    _GridModeQueryFields.Add(Q);
            }
            string Visibility = "";
            try
            {
                if (ColumnVisibility.Length == 0)
                {
                    foreach (DiversityCollection.Forms.GridModeQueryField Q in _GridModeQueryFields)
                    {
                        if (!Q.IsHidden)
                        {
                            if (Q.IsVisible) Visibility += "1";
                            else Visibility += "0";
                        }
                    }
                    ColumnVisibility = Visibility;
                }
                else
                {
                    Visibility = ColumnVisibility;
                    for (int i = 0; i < _GridModeQueryFields.Count; i++)
                    {
                        bool IsVisible = false;
                        if (Visibility.Substring(0, 1) == "1") IsVisible = true;
                        GridModeSetIsVisibleForQueryField(i, IsVisible, ref _GridModeQueryFields);
                        if (Visibility.Length > 1)
                            Visibility = Visibility.Substring(1);
                        else break;
                    }
                }

            }
            catch { }
            return _GridModeQueryFields;
        }

        /// <summary>
        /// a parameter array based on a string
        /// </summary>
        /// <param name="Tag">the string separated with ";"</param>
        /// <returns>a string array</returns>
        public static string[] GridModeFieldTagArray(string Tag)
        {
            char[] charSeparators = new char[] { ';' };
            string[] Parameter = Tag.Split(charSeparators);
            return Parameter;
        }

        /// <summary>
        /// Getting the parameters of the node tags:
        /// string[] separated by ";"
        /// 0 - Table
        /// 1 - AliasForTable (may be empty if table name is unique)
        /// 2 - The restriction used for the view; necessary for an update in the dataset
        /// 3 - Column
        /// 4 - The alias for the column - unique in view
        /// 5 - The datasource table of the column
        /// 7 - RemoveLinkColumn - a dummy column for the buttons for removing a link
        /// </summary>
        /// <param name="Node">the node with the parameters encoded in the tag</param>
        private static void GridModeQueryFieldsAddChildNodes(System.Windows.Forms.TreeNode Node,
            ref System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> _GridModeQueryFields)
        {
            char[] charSeparators = new char[] { ';' };
            foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
            {
                try
                {
                    if (N.Tag != null && N.Tag.ToString().Contains(";"))
                    {
                        string[] Parameter = GridModeFieldTagArray(N.Tag.ToString());
                        if (Parameter.Length > 1)
                        {
                            DiversityCollection.Forms.GridModeQueryField Q = new Forms.GridModeQueryField();
                            if (Parameter.Length < 4) continue;

                            Q.Table = Parameter[0];

                            Q.AliasForTable = Parameter[1];
                            if (Q.AliasForTable.Length == 0) Q.AliasForTable = Q.Table;

                            Q.Restriction = Parameter[2];

                            Q.Column = Parameter[3];

                            if (Parameter.Length > 4)
                                Q.AliasForColumn = Parameter[4];
                            else Q.AliasForColumn = "";
                            if (Q.AliasForColumn.Length == 0)
                                Q.AliasForColumn = Q.Column;

                            if (Parameter.Length > 5)
                                Q.DatasourceTable = Parameter[5];
                            else Q.DatasourceTable = "";

                            if (N.Checked)
                                Q.IsVisible = true;
                            else Q.IsVisible = false;
                            Q.IsHidden = false;
                            if (Parameter.Length > 7)
                                Q.RemoveLinkColumn = Parameter[7];
                            else Q.RemoveLinkColumn = "";
                            if (Q.Table.Length > 0)
                                Q.Entity = Q.Table;
                            if (Q.Column.Length > 0)
                                Q.Entity += "." + Q.Column;
                            _GridModeQueryFields.Add(Q);
                        }
                    }
                    else if (N.Tag != null)
                    {
                    }
                    GridModeQueryFieldsAddChildNodes(N, ref _GridModeQueryFields);
                }
                catch { }
            }
        }


        /// <summary>
        /// Set the visibilty of a QueryField base on the index
        /// </summary>
        /// <param name="Index">the index of the query field</param>
        /// <param name="IsVisible">The visibility</param>
        private static void GridModeSetIsVisibleForQueryField(int Index,
            bool IsVisible,
            ref System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> _GridModeQueryFields)
        {
            try
            {
                DiversityCollection.Forms.GridModeQueryField Q = new Forms.GridModeQueryField();
                Q.AliasForColumn = _GridModeQueryFields[Index].AliasForColumn;
                Q.AliasForTable = _GridModeQueryFields[Index].AliasForTable;
                Q.Column = _GridModeQueryFields[Index].Column;
                Q.DatasourceTable = _GridModeQueryFields[Index].DatasourceTable;
                Q.IsVisible = IsVisible;
                Q.Restriction = _GridModeQueryFields[Index].Restriction;
                Q.Table = _GridModeQueryFields[Index].Table;
                Q.AliasForTable = _GridModeQueryFields[Index].AliasForTable;
                Q.RemoveLinkColumn = _GridModeQueryFields[Index].RemoveLinkColumn;
                _GridModeQueryFields[Index] = Q;
            }
            catch { }
        }

        public static void FindUsedDataColumns(System.Data.DataTable DT, System.Windows.Forms.TreeView TreeView)
        {
            System.Collections.Generic.List<string> UsedColumnList = new List<string>();
            foreach (System.Data.DataColumn C in DT.Columns)
            {
                string Restricton = C.ColumnName + " ";
                if (C.DataType.Name == "Int32" ||
                    C.DataType.Name == "Int16")
                    Restricton += " NOT IS NULL ";
                else if (C.DataType.Name == "String")
                    Restricton += " <> ''";
                else if (C.DataType.Name == "Boolean")
                    Restricton += " <> 0";
                else
                    Restricton += " NOT IS NULL ";
                System.Data.DataRow[] RR = DT.Select(Restricton);
                if (RR.Length > 0)
                    UsedColumnList.Add(C.ColumnName);
            }
            if (UsedColumnList.Count > 0)
            {
                foreach (System.Windows.Forms.TreeNode N in TreeView.Nodes)
                    DiversityCollection.Forms.FormGridFunctions.FindUsedDataColumns(UsedColumnList, N);
            }
        }

        private static void FindUsedDataColumns(System.Collections.Generic.List<string> UsedColumnList, System.Windows.Forms.TreeNode N)
        {
            if (N.Tag != null)
            {
                try
                {
                    string[] Tag = N.Tag.ToString().Split(new char[] { ';' });
                    if (Tag.Length > 4)
                    {
                        if (UsedColumnList.Contains(Tag[4]))
                            N.Checked = true;
                        else N.Checked = false;
                    }
                }
                catch (System.Exception ex) { }
            }
            if (N.Nodes.Count > 0)
            {
                foreach (System.Windows.Forms.TreeNode T in N.Nodes)
                    DiversityCollection.Forms.FormGridFunctions.FindUsedDataColumns(UsedColumnList, T);
            }
        }

        public static void FindEditedColumns(System.Data.DataTable DT
            , System.Windows.Forms.TreeView TreeView
            , System.Collections.Generic.List<string> ReadOnlyColumns)
        {
            System.Collections.Generic.List<string> UsedColumnList = new List<string>();
            foreach (System.Data.DataColumn C in DT.Columns)
            {
                string Restricton = C.ColumnName + " ";
                if (C.DataType.Name == "Int32" ||
                    C.DataType.Name == "Int16")
                    Restricton += " NOT IS NULL ";
                else if (C.DataType.Name == "String")
                    Restricton += " <> ''";
                else if (C.DataType.Name == "Boolean")
                    Restricton += " <> 0";
                else
                    Restricton += " NOT IS NULL ";
                System.Data.DataRow[] RR = DT.Select(Restricton);
                if (RR.Length > 0 && !ReadOnlyColumns.Contains(C.ColumnName))
                    UsedColumnList.Add(C.ColumnName);
            }
            if (UsedColumnList.Count > 0)
            {
                foreach (System.Windows.Forms.TreeNode N in TreeView.Nodes)
                    DiversityCollection.Forms.FormGridFunctions.FindEditedColumns(UsedColumnList, N);
            }
        }

        private static void FindEditedColumns(System.Collections.Generic.List<string> UsedColumnList, System.Windows.Forms.TreeNode N)
        {
            if (N.Tag != null)
            {
                try
                {
                    string[] Tag = N.Tag.ToString().Split(new char[] { ';' });
                    if (Tag.Length > 4)
                    {
                        if (UsedColumnList.Contains(Tag[4]))
                            N.Checked = true;
                        else N.Checked = false;
                    }
                }
                catch (System.Exception ex) { }
            }
            if (N.Nodes.Count > 0)
            {
                foreach (System.Windows.Forms.TreeNode T in N.Nodes)
                    DiversityCollection.Forms.FormGridFunctions.FindEditedColumns(UsedColumnList, T);
            }
        }

        #endregion

        #region Clipboard
        
        public static bool CanCopyClipboardInDataGrid(int IndexTopRow, System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues
            , System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums
            , System.Windows.Forms.DataGridView dataGridView)
        {
            bool OK = true;
            try
            {
                string Message = CheckNumberOfRowsToCopyClipboard(IndexTopRow, ClipBoardValues, dataGridView);
                Message += CheckNumberOfColumnsToCopyClipboard(ClipBoardValues, GridColums);
                Message += CheckTypeOfColumnsToCopyClipboard(GridColums);
                Message = Message.Trim();
                if (Message.Length > 0)
                {
                    OK = false;
                    System.Windows.Forms.MessageBox.Show(Message);
                }
            }
            catch { OK = false; }
            return OK;
        }

        private static string CheckNumberOfRowsToCopyClipboard(int IndexTopRow, System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues, System.Windows.Forms.DataGridView dataGridView)
        {
            string Message = "";
            if (dataGridView.Rows.Count <= IndexTopRow + ClipBoardValues.Count)
                Message = "You try to copy " + ClipBoardValues.Count.ToString() + " rows into " + (dataGridView.Rows.Count - IndexTopRow - 1).ToString() + " available row(s).\r\n" +
                    "Please reduce you selection.\r\n\r\n";
            return Message;
        }

        private static string CheckNumberOfColumnsToCopyClipboard(System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues, System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums)
        {
            string Message = "";
            if (GridColums.Count < ClipBoardValues[0].Count)
                Message = "You try to copy " + ClipBoardValues[0].Count.ToString() + " columns into " + (GridColums.Count).ToString() + " available column(s).\r\n" +
                    "Please reduce your selection.\r\n\r\n";
            return Message;
        }

        private static string CheckTypeOfColumnsToCopyClipboard(System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums)
        {
            string Message = "";
            foreach (System.Windows.Forms.DataGridViewColumn GridColum in GridColums)
            {
                if (GridColum.CellType == typeof(System.Windows.Forms.DataGridViewButtonCell)
                    && (GridColum.HeaderText.StartsWith("Link to ")
                    || GridColum.HeaderText.StartsWith("Remove link to ")))
                    Message += GridColum.HeaderText + "\r\n";
            }
            if (Message.Length > 0)
                Message = "The following columns can not be changed via the clipboard:\r\n" + Message + "Please hide the columns from the grid or reduce your selection\r\n\r\n";
            return Message;
        }

        public static System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues
        {
            get
            {
                // parsing the content of the clipboard
                System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues = new List<List<string>>();
                string[] stringSeparators = new string[] { "\r\n" };
                string[] lineSeparators = new string[] { "\t" };
                string ClipBoardText = System.Windows.Forms.Clipboard.GetText();
                string[] ClipBoardList = ClipBoardText.Split(stringSeparators, StringSplitOptions.None);
                for (int i = 0; i < ClipBoardList.Length; i++)
                {
                    System.Collections.Generic.List<string> ClipBoardValueStrings = new List<string>();
                    string[] ClipBoardListStrings = ClipBoardList[i].Split(lineSeparators, StringSplitOptions.None);
                    for (int ii = 0; ii < ClipBoardListStrings.Length; ii++)
                        ClipBoardValueStrings.Add(ClipBoardListStrings[ii]);
                    ClipBoardValues.Add(ClipBoardValueStrings);
                }
                if ((ClipBoardValues[0].Count > ClipBoardValues[ClipBoardList.Length - 1].Count ||
                    ClipBoardValues[0].Count == 1) &&
                    ClipBoardValues[ClipBoardList.Length - 1][0].Length == 0)
                    ClipBoardValues.Remove(ClipBoardValues[ClipBoardList.Length - 1]);
                return ClipBoardValues;
            }
        }

        public static System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums (System.Windows.Forms.DataGridView dataGridView)
        {
            System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums = new List<DataGridViewColumn>();
            int CurrentDisplayIndex = dataGridView.Columns[dataGridView.SelectedCells[0].ColumnIndex].DisplayIndex;
            if (ClipBoardValues.Count > 0)
            {
                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    foreach (System.Windows.Forms.DataGridViewColumn C in dataGridView.Columns)
                    {
                        if (C.Visible && C.DisplayIndex == CurrentDisplayIndex + i)
                        {
                            GridColums.Add(C);
                            break;
                        }
                    }
                    if (GridColums.Count >= ClipBoardValues[0].Count) break;
                }
            }
            return GridColums;
        }



        #endregion

        #region Marking column and selection of cells

        public static void MarkWholeColumn(int ColumnIndex, System.Windows.Forms.DataGridView dataGridView)
        {
            try
            {
                //UnselectGridView(dataGridView);
                foreach (System.Windows.Forms.DataGridViewCell Cell in dataGridView.SelectedCells)
                    Cell.Selected = false;

                System.Windows.Forms.DataGridViewColumn C = dataGridView.Columns[ColumnIndex];
                int iLines = dataGridView.Rows.Count;
                if (dataGridView.AllowUserToAddRows)
                    iLines--;
                for (int i = 0; i < iLines; i++)
                {
                    dataGridView.Rows[i].Cells[ColumnIndex].Selected = true;
                }
            }
            catch { }
        }

        //public static void UnselectGridView(System.Windows.Forms.DataGridView dataGridView)
        //{
        //    try
        //    {
        //        foreach (System.Windows.Forms.DataGridViewCell Cell in dataGridView.SelectedCells)
        //            Cell.Selected = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Windows.Forms.MessageBox.Show(ex.Message);
        //    }
        //}

        public static System.Collections.Generic.List<System.Drawing.Point> SelectedGridPositions(System.Windows.Forms.DataGridView dataGridView)
        {
            System.Collections.Generic.List<System.Drawing.Point> SelectedCells = new List<System.Drawing.Point>();
            try
            {
                if (dataGridView.SelectedCells != null && dataGridView.SelectedCells.Count > 0)
                {
                    foreach (System.Windows.Forms.DataGridViewCell cell in dataGridView.SelectedCells)
                    {
                        System.Drawing.Point point = new System.Drawing.Point(cell.ColumnIndex, cell.RowIndex);
                        SelectedCells.Add(point);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return SelectedCells;
        }

        public static void SetSelectionRange(System.Windows.Forms.DataGridView dataGridView, System.Collections.Generic.List<System.Drawing.Point> Positions)
        {
            try
            {
                foreach (System.Windows.Forms.DataGridViewCell Cell in dataGridView.SelectedCells)
                    Cell.Selected = false;
                if (Positions != null)
                {
                    foreach (System.Drawing.Point position in Positions)
                    {
                        dataGridView.Rows[position.Y].Cells[position.X].Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        #endregion

        //public static void PasteFromClipboard(System.Windows.Forms.DataGridView dataGridView)
        //{
        //    if (System.Windows.Forms.Clipboard.ContainsData(System.Windows.Forms.DataFormats.CommaSeparatedValue)
        //        || System.Windows.Forms.Clipboard.ContainsData(System.Windows.Forms.DataFormats.Text))
        //    {
        //        // finding the top row
        //        int IndexTopRow = dataGridView.Rows.Count;
        //        if (dataGridView.SelectedCells.Count > 0)
        //        {
        //            foreach (System.Windows.Forms.DataGridViewCell C in dataGridView.SelectedCells)
        //                if (IndexTopRow > C.RowIndex) IndexTopRow = C.RowIndex;
        //        }

        //        // parsing the content of the clipboard
        //        System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues = DiversityCollection.Forms.FormGridFunctions.ClipBoardValues;// this.ClipBoardValues;
        //        System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums = DiversityCollection.Forms.FormGridFunctions.GridColums(dataGridView);
        //        if (!DiversityCollection.Forms.FormGridFunctions.CanCopyClipboardInDataGrid(IndexTopRow, ClipBoardValues, GridColums, dataGridView))
        //            return;
        //        //if (!this.CanCopyClipboardInDataGrid(IndexTopRow, ClipBoardValues, GridColums))
        //        //    return;

        //        try
        //        {
        //            for (int ii = 0; ii < GridColums.Count; ii++) // the columns
        //            {
        //                for (int i = 0; i < ClipBoardValues.Count; i++) // the rows
        //                {
        //                    if (dataGridView.Rows[IndexTopRow + i].Cells[GridColums[ii].Index].ReadOnly)
        //                        continue;
        //                    if (DiversityCollection.Forms.FormGridFunctions.ValueIsValid(dataGridView, GridColums[ii].Index, ClipBoardValues[i][ii]))
        //                    {
        //                        dataGridView.Rows[IndexTopRow + i].Cells[GridColums[ii].Index].Value = ClipBoardValues[i][ii];
        //                        this.checkForMissingAndDefaultValues(dataGridView.Rows[IndexTopRow + i].Cells[GridColums[ii].Index], true);
        //                    }
        //                    else
        //                    {
        //                        string Message = ClipBoardValues[i][ii] + " is not a valid value for "
        //                            + dataGridView.Columns[GridColums[ii].Index].DataPropertyName
        //                            + "\r\n\r\nDo you want to try to insert the other values?";
        //                        if (System.Windows.Forms.MessageBox.Show(Message, "Invalid value", MessageBoxButtons.YesNo) == DialogResult.No)
        //                            break;
        //                    }
        //                    if (i + IndexTopRow + 3 > dataGridView.Rows.Count)
        //                        continue;
        //                }
        //            }
        //        }
        //        catch { }
        //    }
        //    else
        //    {
        //        System.Windows.Forms.MessageBox.Show("Only text and spreadsheet values can be copied");
        //        return;
        //    }
        //}

        #region Remove and clear

        private static void ClearGridviewCells(System.Windows.Forms.DataGridView dataGridView)
    {
        foreach (System.Windows.Forms.DataGridViewCell C in dataGridView.SelectedCells)
        {
            try
            {
                C.Value = null;
                if (C.Value != null)
                {
                    C.Value = "";
                    if (C.Value != "")
                        C.Value = System.DBNull.Value;
                }
            }
            catch (System.Exception ex) { }
        }
    }

    public static void RemoveRowFromGrid(System.Windows.Forms.DataGridView dataGridView, System.Data.DataSet DataSet)
    {
        if (dataGridView.SelectedRows.Count > 0)
        {
            foreach (System.Windows.Forms.DataGridViewRow R in dataGridView.SelectedRows)
            {
                try
                {
                    System.Data.DataRowView Rdata = (System.Data.DataRowView)R.DataBoundItem;
                    Rdata.Delete();
                    Rdata.Row.AcceptChanges();
                }
                catch (System.Exception ex) { }
            }
            DataSet.AcceptChanges();
        }
        else
            System.Windows.Forms.MessageBox.Show("No rows selected");

    }

    #endregion

        #region Filtering

        public static void FilterGrid(System.Windows.Forms.DataGridView dataGridView)
        {
            try
            {
                if (dataGridView.SelectedCells != null)
                {
                    if (dataGridView.SelectedCells[0].Value != null)
                    {
                        string Filter = dataGridView.SelectedCells[0].Value.ToString();
                        System.Collections.Generic.List<System.Data.DataRow> RowsToRemove = new List<System.Data.DataRow>();
                        foreach (System.Windows.Forms.DataGridViewRow R in dataGridView.Rows)
                        {
                            if ((R.Cells[dataGridView.SelectedCells[0].ColumnIndex].Value == null && Filter.Length > 0)
                                || (R.Cells[dataGridView.SelectedCells[0].ColumnIndex].Value != null && R.Cells[dataGridView.SelectedCells[0].ColumnIndex].Value.ToString() != Filter))
                            {
                                if (R.DataBoundItem != null)
                                {
                                    System.Data.DataRowView Rdata = (System.Data.DataRowView)R.DataBoundItem;
                                    RowsToRemove.Add(Rdata.Row);
                                }
                            }
                        }
                        foreach (System.Data.DataRow R in RowsToRemove)
                            R.Delete();
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Replacing values

        public static bool ValueIsValid(
            System.Windows.Forms.DataGridView dataGridView,
            int ColumnIndex,
            string Value)
        {
            CultureInfo InvC = new CultureInfo("");

            if (Value.Length == 0) return true;
            System.DateTime Date;
            System.Byte Byte;
            System.Int16 Int16;
            string TypeOfColumn = dataGridView.Columns[ColumnIndex].ValueType.ToString();
            string ColumnName = dataGridView.Columns[ColumnIndex].DataPropertyName; //Ariane changed this, since the first selected column might not be the correct one, e.g. for the _levae event of the datagridviewautosuggest. dataGridView.Columns[dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
            //string ColumnName = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Columns[ColumnIndex].ColumnName;
            int Int;
            bool ValidValue = true;
            try
            {
                // check if the values are valible
                switch (TypeOfColumn)
                {
                    case "System.DateTime":
                        if (!System.DateTime.TryParse(Value, out Date))
                        {
                            if (!System.DateTime.TryParse(Value, InvC, DateTimeStyles.None, out Date))
                                ValidValue = false;
                        }
                        break;
                    case "int":
                        if (!int.TryParse(Value, out Int))
                            ValidValue = false;

                        break;
                    case "System.Byte":
                        if (!System.Byte.TryParse(Value, out Byte))
                            ValidValue = false;
                        break;
                    case "System.Int16":
                        if (!System.Int16.TryParse(Value, out Int16))
                            ValidValue = false;
                        break;
                }

                // check if the values fit the column definition
                if (Value.Length > 0 && ValidValue)
                {
                    switch (ColumnName)
                    {
                        case "Collection_day":
                        case "Accession_day":
                        case "Identification_day":
                            int Day = int.Parse(Value);
                            if (Day < 1 || Day > 31)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Collection_month":
                        case "Accession_month":
                        case "Identification_month":
                            int Month = int.Parse(Value);
                            if (Month < 1 || Month > 12)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Collection_year":
                        case "Accession_year":
                        case "Identification_year":
                            int Year = int.Parse(Value);
                            if (Year < 1000 || Year > System.DateTime.Now.Year + 1)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Preparation_date":
                            break;
                        case "Longitude":
                            bool IsValid = true;
                            float Lon;
                            if (!float.TryParse(Value, NumberStyles.Float, InvC, out Lon)) IsValid = false;
                            if (!IsValid || Lon > 180 || Lon < -180)
                            {
                                int x = (int)Lon;
                                float f = Lon * 2;
                                ValidValue = false;
                            }
                            break;
                        case "Latitude":
                            IsValid = true;
                            float Lat;
                            if (!float.TryParse(Value, NumberStyles.Float, InvC, out Lat)) IsValid = false;
                            if (!IsValid || Lat > 90 || Lat < -90)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Altitude_from":
                        case "Altitude_to":
                            IsValid = true;
                            float Alt;
                            if (!float.TryParse(Value, NumberStyles.Float, InvC, out Alt)) IsValid = false;
                            if (!IsValid || Alt > 9000 || Alt < -11000)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "MTB":
                            bool OK = true;
                            if (Value.Length != 4) OK = false;
                            if (!int.TryParse(Value, out Int)) OK = false;
                            if (!OK)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Quadrant":
                            OK = true;
                            if (!int.TryParse(Value, out Int)) OK = false;
                            else
                            {
                                for (int i = 0; i < Value.Length; i++)
                                {
                                    if (Value.Substring(i, 1) != "1" &&
                                        Value.Substring(i, 1) != "2" &&
                                        Value.Substring(i, 1) != "3" &&
                                        Value.Substring(i, 1) != "4")
                                        OK = false;
                                }
                            }
                            if (!OK)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Number_of_units":
                            if (!int.TryParse(Value, out Int))
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Stock":
                            IsValid = true;
                            float Stock;
                            if (!float.TryParse(Value, NumberStyles.Float, InvC, out Stock)) IsValid = false;
                            if (!IsValid || Stock < 0)
                            {
                                ValidValue = false;
                            }
                            break;
                    }
                }
            }
            catch { }
            return ValidValue;
        }

        public static void AskIfReplacementShouldBeStopped(
            string ColumnName,
            string Value,
            ReplaceOptionState ReplaceOptionStatus)
        {
            string Message = "";
            if (Value.Length > 0)
                Message = Value + " is not a valid value for " + ColumnName.Replace("_", " ") + "\r\n\r\nDo you want to " + ReplaceOptionStatus.ToString() + " further values?";
            else
                Message = "You insert an empty value for " + ColumnName.Replace("_", " ") + "\r\n\r\nDo you want to " + ReplaceOptionStatus.ToString() + " further values?";
            if (System.Windows.Forms.MessageBox.Show(Message, "Invalid value", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                DiversityCollection.Forms.FormCollectionSpecimenGridMode.StopReplacing = true;
            else DiversityCollection.Forms.FormCollectionSpecimenGridMode.StopReplacing = false;
        }

        public static void ColumnValues_ReplaceInsertClear(
            System.Windows.Forms.DataGridView dataGridView,
            System.Windows.Forms.ComboBox comboBoxReplace,
            System.Windows.Forms.ComboBox comboBoxReplaceWith,
            System.Windows.Forms.TextBox textBoxGridModeReplace,
            System.Windows.Forms.TextBox textBoxGridModeReplaceWith,
            System.Data.DataTable dtSource,
            int ColumnIndexPK,
            ReplaceOptionState ReplaceOptionStatus)
        {
            System.Collections.Generic.List<int> PK = new List<int>();
            if (dataGridView.SelectedCells.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nothing has been selected");
                return;
            }
            if (dataGridView.SelectedCells.Count > 0)
            {
                DiversityCollection.Forms.FormCollectionSpecimenGridMode.StopReplacing = false;
                DiversityCollection.Forms.FormCollectionSpecimenGridMode.StopReplacing = false;
                int Index = dataGridView.SelectedCells[0].ColumnIndex;
                if (dataGridView.SelectedCells.Count > 1)
                {
                    foreach (System.Windows.Forms.DataGridViewCell C in dataGridView.SelectedCells)
                    {
                        int iPK = 0;
                        if (dataGridView.Rows[C.RowIndex].Cells[0].Value != null)
                        {
                            if (int.TryParse(dataGridView.Rows[C.RowIndex].Cells[0].Value.ToString(), out iPK))
                                PK.Add(iPK);
                        }
                    }
                }
                else if (dataGridView.SelectedCells.Count == 1)
                {
                    if (System.Windows.Forms.MessageBox.Show("Only one field has been selected.\r\n Do you want to apply the changes to the whole column?",
                        "Apply to whole column?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        foreach (System.Windows.Forms.DataGridViewRow R in dataGridView.Rows)
                        {
                            int iPK = 0;
                            if (dataGridView.Rows[R.Index].Cells[0].Value != null)
                            {
                                if (int.TryParse(R.Cells[0].Value.ToString(), out iPK))
                                    PK.Add(iPK);
                            }
                        }
                    }
                    else
                    {
                        int iPK = 0;
                        if (dataGridView.Rows[dataGridView.SelectedCells[0].RowIndex].Cells[0].Value != null)
                        {
                            if (int.TryParse(dataGridView.Rows[dataGridView.SelectedCells[0].RowIndex].Cells[0].Value.ToString(), out iPK))
                                PK.Add(iPK);
                        }
                    }
                }

                foreach (int ID in PK)
                {
                    try
                    {
                        System.Windows.Forms.DataGridViewRow Row = dataGridView.Rows[0];
                        foreach (System.Windows.Forms.DataGridViewRow RR in dataGridView.Rows)
                        {
                            if (RR.Cells[0].Value.ToString() == ID.ToString())
                            {
                                Row = RR;
                                break;
                            }
                        }
                        if (Row.Cells[Index].Style.Tag != null && Row.Cells[Index].Style.Tag.ToString() == "Blocked")
                            continue;
                        if (Row.Index > dtSource.Rows.Count - 1)
                            break;
                        System.Type Type = Row.Cells[Index].ValueType;
                        string Value = "";
                        if (Row.Cells[Index].Value != null && !Row.Cells[Index].Value.Equals(System.DBNull.Value))
                            Value = Row.Cells[Index].Value.ToString();
                        string OriginalText = textBoxGridModeReplace.Text;
                        if (comboBoxReplace.Visible)
                        {
                            if (comboBoxReplace.SelectedIndex > -1)
                                OriginalText = comboBoxReplace.SelectedValue.ToString();
                            else OriginalText = "";
                        }
                        string NewText = textBoxGridModeReplaceWith.Text;
                        if (comboBoxReplaceWith.Visible)
                        {
                            if (comboBoxReplaceWith.SelectedIndex > -1)
                                NewText = comboBoxReplaceWith.SelectedValue.ToString();
                            else NewText = "";
                        }
                        bool IsValid = true;
                        string CorrectedValue = DiversityCollection.Forms.FormGridFunctions.CheckReplaceValue(
                            dataGridView,
                            dtSource,
                            Index,
                            Value,
                            OriginalText,
                            NewText,
                            ref IsValid,
                            ReplaceOptionStatus);
                        if (DiversityCollection.Forms.FormCollectionSpecimenGridMode.StopReplacing) return;
                        if (!IsValid) continue;
                        if (Type == typeof(string))
                        {
                            Row.Cells[Index].Value = CorrectedValue;
                            if (CorrectedValue.Length == 0 && dataGridView.Columns[Index].CellType.Name == "DataGridViewComboBoxCell") // && comboBoxReplaceWith.Visible && comboBoxReplaceWith.SelectedIndex == 0)
                            {
                                Row.Cells[Index].Value = null;
                            }
                        }
                        else if (Type == typeof(int)
                            || Type == typeof(System.Byte)
                            || Type == typeof(System.DateTime)
                            || Type == typeof(System.Int16))
                        {
                            if (CorrectedValue.Length == 0)
                                Row.Cells[Index].Value = System.DBNull.Value;
                            else
                            {
                                if (Type == typeof(int))
                                    Row.Cells[Index].Value = int.Parse(CorrectedValue);
                                else if (Type == typeof(System.Byte))
                                    Row.Cells[Index].Value = System.Byte.Parse(CorrectedValue);
                                else if (Type == typeof(System.Int16))
                                    Row.Cells[Index].Value = System.Int16.Parse(CorrectedValue);
                                else if (Type == typeof(System.DateTime))
                                    Row.Cells[Index].Value = System.DateTime.Parse(CorrectedValue);
                            }
                        }
                    }
                    catch { }
                    if (DiversityCollection.Forms.FormCollectionSpecimenGridMode.StopReplacing)
                        break;
                }
            }
        }

        public static void ColumnValues_ReplaceInsertClear(
            System.Windows.Forms.DataGridView dataGridView,
            System.Windows.Forms.ComboBox comboBoxReplace,
            System.Windows.Forms.ComboBox comboBoxReplaceWith,
            System.Windows.Forms.TextBox textBoxGridModeReplace,
            System.Windows.Forms.TextBox textBoxGridModeReplaceWith,
            System.Data.DataTable dtSource,
            System.Collections.Generic.List<int> ColumnIndexPKs,
            ReplaceOptionState ReplaceOptionStatus)
        {
            System.Collections.Generic.List<System.Collections.Generic.List<string>> PKs = new List<List<string>>();
            if (dataGridView.SelectedCells.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nothing has been selected");
                return;
            }
            if (dataGridView.SelectedCells.Count > 0)
            {
                DiversityCollection.Forms.FormCollectionSpecimenGridMode.StopReplacing = false;
                DiversityCollection.Forms.FormCollectionSpecimenGridMode.StopReplacing = false;
                int Index = dataGridView.SelectedCells[0].ColumnIndex;
                if (dataGridView.SelectedCells.Count > 1)
                {
                    foreach (System.Windows.Forms.DataGridViewCell C in dataGridView.SelectedCells)
                    {
                        System.Collections.Generic.List<string> PK = new List<string>();
                        //int iPK = 0;
                        foreach (int i in ColumnIndexPKs)
                        {
                            if (dataGridView.Rows[C.RowIndex].Cells[i].Value != null)
                            {
                                PK.Add(dataGridView.Rows[C.RowIndex].Cells[i].Value.ToString());
                            }
                        }
                        PKs.Add(PK);
                        //if (dataGridView.Rows[C.RowIndex].Cells[0].Value != null)
                        //{
                        //    if (int.TryParse(dataGridView.Rows[C.RowIndex].Cells[0].Value.ToString(), out iPK))
                        //        PK.Add(iPK);
                        //}
                    }
                }
                else if (dataGridView.SelectedCells.Count == 1)
                {
                    if (System.Windows.Forms.MessageBox.Show("Only one field has been selected.\r\n Do you want to apply the changes to the whole column?",
                        "Apply to whole column?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        foreach (System.Windows.Forms.DataGridViewRow R in dataGridView.Rows)
                        {
                            System.Collections.Generic.List<string> PK = new List<string>();
                            foreach (int i in ColumnIndexPKs)
                            {
                                if (dataGridView.Rows[R.Index].Cells[i].Value != null)
                                {
                                    PK.Add(dataGridView.Rows[R.Index].Cells[i].Value.ToString());
                                }
                            }
                            PKs.Add(PK);
                            //int iPK = 0;
                            //if (dataGridView.Rows[R.Index].Cells[0].Value != null)
                            //{
                            //    if (int.TryParse(R.Cells[0].Value.ToString(), out iPK))
                            //        PK.Add(iPK);
                            //}
                        }
                    }
                    else
                    {
                        System.Collections.Generic.List<string> PK = new List<string>();
                        foreach (int i in ColumnIndexPKs)
                        {
                            if (dataGridView.Rows[dataGridView.SelectedCells[0].RowIndex].Cells[i].Value != null)
                            {
                                PK.Add(dataGridView.Rows[dataGridView.SelectedCells[0].RowIndex].Cells[i].Value.ToString());
                            }
                        }
                        PKs.Add(PK);
                        //int iPK = 0;
                        //if (dataGridView.Rows[dataGridView.SelectedCells[0].RowIndex].Cells[0].Value != null)
                        //{
                        //    if (int.TryParse(dataGridView.Rows[dataGridView.SelectedCells[0].RowIndex].Cells[0].Value.ToString(), out iPK))
                        //        PK.Add(iPK);
                        //}
                    }
                }

                foreach (System.Collections.Generic.List<string> L in PKs)
                {
                    try
                    {
                        System.Windows.Forms.DataGridViewRow Row = dataGridView.Rows[0];
                        foreach (System.Windows.Forms.DataGridViewRow RR in dataGridView.Rows)
                        {
                            bool RowFound = true;
                            for (int i = 0; i < ColumnIndexPKs.Count; i++ )
                            {
                                if (RR.Cells[ColumnIndexPKs[i]].Value.ToString() != L[i])
                                {
                                    RowFound = false;
                                    break;
                                }
                            }
                            if (RowFound)
                            {
                                Row = RR;
                                break;
                            }
                            //if (RR.Cells[0].Value.ToString() == ID.ToString())
                            //{
                            //    Row = RR;
                            //    break;
                            //}
                        }
                        if (Row.Cells[Index].Style.Tag != null && Row.Cells[Index].Style.Tag.ToString() == "Blocked")
                            continue;
                        if (Row.Index > dtSource.Rows.Count - 1)
                            break;
                        System.Type Type = Row.Cells[Index].ValueType;
                        string Value = "";
                        if (Row.Cells[Index].Value != null && !Row.Cells[Index].Value.Equals(System.DBNull.Value))
                            Value = Row.Cells[Index].Value.ToString();
                        string OriginalText = textBoxGridModeReplace.Text;
                        if (comboBoxReplace.Visible)
                        {
                            if (comboBoxReplace.SelectedIndex > -1)
                                OriginalText = comboBoxReplace.SelectedValue.ToString();
                            else OriginalText = "";
                        }
                        string NewText = textBoxGridModeReplaceWith.Text;
                        if (comboBoxReplaceWith.Visible)
                        {
                            if (comboBoxReplaceWith.SelectedIndex > -1)
                                NewText = comboBoxReplaceWith.SelectedValue.ToString();
                            else NewText = "";
                        }
                        bool IsValid = true;
                        string CorrectedValue = DiversityCollection.Forms.FormGridFunctions.CheckReplaceValue(
                            dataGridView,
                            dtSource,
                            Index,
                            Value,
                            OriginalText,
                            NewText,
                            ref IsValid,
                            ReplaceOptionStatus);
                        if (DiversityCollection.Forms.FormCollectionSpecimenGridMode.StopReplacing) return;
                        if (!IsValid) continue;
                        if (Type == typeof(string))
                        {
                            Row.Cells[Index].Value = CorrectedValue;
                            if (CorrectedValue.Length == 0 && dataGridView.Columns[Index].CellType.Name == "DataGridViewComboBoxCell") // && comboBoxReplaceWith.Visible && comboBoxReplaceWith.SelectedIndex == 0)
                            {
                                Row.Cells[Index].Value = null;
                            }
                        }
                        else if (Type == typeof(int)
                            || Type == typeof(System.Byte)
                            || Type == typeof(System.DateTime)
                            || Type == typeof(System.Int16))
                        {
                            if (CorrectedValue.Length == 0)
                                Row.Cells[Index].Value = System.DBNull.Value;
                            else
                            {
                                if (Type == typeof(int))
                                    Row.Cells[Index].Value = int.Parse(CorrectedValue);
                                else if (Type == typeof(System.Byte))
                                    Row.Cells[Index].Value = System.Byte.Parse(CorrectedValue);
                                else if (Type == typeof(System.Int16))
                                    Row.Cells[Index].Value = System.Int16.Parse(CorrectedValue);
                                else if (Type == typeof(System.DateTime))
                                    Row.Cells[Index].Value = System.DateTime.Parse(CorrectedValue);
                            }
                        }
                    }
                    catch { }
                    if (DiversityCollection.Forms.FormCollectionSpecimenGridMode.StopReplacing)
                        break;
                }
            }
        }

        public static void ColumnValues_ReplaceInsertClear(
            System.Windows.Forms.DataGridView dataGridView,
            System.Windows.Forms.LinkLabel LinkLabelLink,
            int DisplayColumnIndex,
            string DisplayText,
            System.Data.DataTable dtSource,
            System.Collections.Generic.List<int> ColumnIndexPKs,
            ReplaceOptionState ReplaceOptionStatus)
        {
            System.Collections.Generic.List<System.Collections.Generic.List<string>> PKs = new List<List<string>>();
            if (dataGridView.SelectedCells.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nothing has been selected");
                return;
            }
            if (dataGridView.SelectedCells.Count > 0)
            {
                DiversityCollection.Forms.FormCollectionSpecimenGridMode.StopReplacing = false;
                DiversityCollection.Forms.FormCollectionSpecimenGridMode.StopReplacing = false;
                int Index = dataGridView.SelectedCells[0].ColumnIndex;
                if (dataGridView.SelectedCells.Count > 1)
                {
                    foreach (System.Windows.Forms.DataGridViewCell C in dataGridView.SelectedCells)
                    {
                        System.Collections.Generic.List<string> PK = new List<string>();
                        foreach (int i in ColumnIndexPKs)
                        {
                            if (dataGridView.Rows[C.RowIndex].Cells[i].Value != null)
                            {
                                PK.Add(dataGridView.Rows[C.RowIndex].Cells[i].Value.ToString());
                            }
                        }
                        PKs.Add(PK);
                    }
                }
                else if (dataGridView.SelectedCells.Count == 1)
                {
                    if (System.Windows.Forms.MessageBox.Show("Only one field has been selected.\r\n Do you want to apply the changes to the whole column?",
                        "Apply to whole column?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        foreach (System.Windows.Forms.DataGridViewRow R in dataGridView.Rows)
                        {
                            System.Collections.Generic.List<string> PK = new List<string>();
                            foreach (int i in ColumnIndexPKs)
                            {
                                if (dataGridView.Rows[R.Index].Cells[i].Value != null)
                                {
                                    PK.Add(dataGridView.Rows[R.Index].Cells[i].Value.ToString());
                                }
                            }
                            PKs.Add(PK);
                        }
                    }
                    else
                    {
                        System.Collections.Generic.List<string> PK = new List<string>();
                        foreach (int i in ColumnIndexPKs)
                        {
                            if (dataGridView.Rows[dataGridView.SelectedCells[0].RowIndex].Cells[i].Value != null)
                            {
                                PK.Add(dataGridView.Rows[dataGridView.SelectedCells[0].RowIndex].Cells[i].Value.ToString());
                            }
                        }
                        PKs.Add(PK);
                    }
                }

                foreach (System.Collections.Generic.List<string> L in PKs)
                {
                    try
                    {
                        System.Windows.Forms.DataGridViewRow Row = dataGridView.Rows[0];
                        foreach (System.Windows.Forms.DataGridViewRow RR in dataGridView.Rows)
                        {
                            bool RowFound = true;
                            for (int i = 0; i < ColumnIndexPKs.Count; i++)
                            {
                                if (RR.Cells[ColumnIndexPKs[i]].Value.ToString() != L[i])
                                {
                                    RowFound = false;
                                    break;
                                }
                            }
                            if (RowFound)
                            {
                                Row = RR;
                                break;
                            }
                        }
                        if (Row.Cells[Index].Style.Tag != null && Row.Cells[Index].Style.Tag.ToString() == "Blocked")
                            continue;
                        if (Row.Index > dtSource.Rows.Count - 1)
                            break;
                        System.Type Type = Row.Cells[Index].ValueType;
                        string Value = "";
                        if (Row.Cells[Index].Value != null && !Row.Cells[Index].Value.Equals(System.DBNull.Value))
                            Value = Row.Cells[Index].Value.ToString();
                        string NewLink = LinkLabelLink.Text;
                        bool IsValid = true;
                        string CorrectedValue = DiversityCollection.Forms.FormGridFunctions.CheckReplaceValue(
                            dataGridView,
                            dtSource,
                            Index,
                            Value,
                            "",
                            NewLink,
                            ref IsValid,
                            ReplaceOptionStatus);
                        if (DiversityCollection.Forms.FormCollectionSpecimenGridMode.StopReplacing) return;
                        if (!IsValid) continue;
                        if (Type == typeof(string))
                        {
                            Row.Cells[Index].Value = CorrectedValue;
                            Row.Cells[DisplayColumnIndex].Value = DisplayText;
                            if (CorrectedValue.Length == 0 && dataGridView.Columns[Index].CellType.Name == "DataGridViewComboBoxCell") // && comboBoxReplaceWith.Visible && comboBoxReplaceWith.SelectedIndex == 0)
                            {
                                Row.Cells[Index].Value = null;
                            }
                        }
                        else if (Type == typeof(int)
                            || Type == typeof(System.Byte)
                            || Type == typeof(System.DateTime)
                            || Type == typeof(System.Int16))
                        {
                            if (CorrectedValue.Length == 0)
                                Row.Cells[Index].Value = System.DBNull.Value;
                            else
                            {
                                if (Type == typeof(int))
                                    Row.Cells[Index].Value = int.Parse(CorrectedValue);
                                else if (Type == typeof(System.Byte))
                                    Row.Cells[Index].Value = System.Byte.Parse(CorrectedValue);
                                else if (Type == typeof(System.Int16))
                                    Row.Cells[Index].Value = System.Int16.Parse(CorrectedValue);
                                else if (Type == typeof(System.DateTime))
                                    Row.Cells[Index].Value = System.DateTime.Parse(CorrectedValue);
                            }
                        }
                    }
                    catch { }
                    if (DiversityCollection.Forms.FormCollectionSpecimenGridMode.StopReplacing)
                        break;
                }
            }
        }

        public static string CheckReplaceValue(
            System.Windows.Forms.DataGridView dataGridView,
            System.Data.DataTable dtSource,
            int ColumnIndex,
            string OriginalValue,
            string ReplacedValue,
            string Replacement,
            ref bool IsValid,
            ReplaceOptionState ReplaceOptionStatus)
        {
            string TypeOfColumn = dataGridView.Columns[ColumnIndex].ValueType.ToString();
            System.Type Type = dataGridView.Columns[ColumnIndex].ValueType;
            string ColumnName = dtSource.Columns[ColumnIndex].ColumnName;
            string Value = "";
            IsValid = true;
            try
            {
                // constructing the new value
                if (typeof(System.Windows.Forms.DataGridViewComboBoxCell)
                    == dataGridView.Columns[dataGridView.SelectedCells[0].ColumnIndex].CellType)
                {
                    switch (ReplaceOptionStatus)
                    {
                        case ReplaceOptionState.Insert:
                            Value = Replacement;
                            break;
                        case ReplaceOptionState.Append:
                            Value = Replacement;
                            break;
                        case ReplaceOptionState.Replace:
                            if (OriginalValue == ReplacedValue)
                                Value = Replacement;
                            else Value = OriginalValue;
                            break;
                    }
                    //Value = Replacement;
                }
                else
                {
                    switch (ReplaceOptionStatus)
                    {
                        case ReplaceOptionState.Insert:
                            if (Replacement.StartsWith("http://") && OriginalValue.StartsWith("http://"))
                                Value = Replacement;
                            else
                                Value = Replacement + OriginalValue;
                            break;
                        case ReplaceOptionState.Append:
                            Value = OriginalValue + Replacement;
                            break;
                        case ReplaceOptionState.Replace:
                            if (Type == typeof(int)
                            || Type == typeof(System.Byte)
                            || Type == typeof(System.DateTime)
                            || Type == typeof(System.Int16))
                            {
                                if (OriginalValue == ReplacedValue)
                                    Value = Replacement;
                                else
                                    Value = OriginalValue;
                            }
                            else
                            {
                                if (Replacement.Length < OriginalValue.Length && ReplacedValue.Length > 0)
                                    Value = OriginalValue.Replace(ReplacedValue, Replacement);
                                else if (OriginalValue.Length == 0 && ReplacedValue.Length == 0)
                                    Value = Replacement;
                                else if (OriginalValue == ReplacedValue && ReplacedValue.Length > 0)
                                    Value = Replacement;
                                else
                                    IsValid = false;
                            }
                            break;
                    }
                }

                if (!DiversityCollection.Forms.FormGridFunctions.ValueIsValid(dataGridView, ColumnIndex, Value))
                {
                    IsValid = false;
                    DiversityCollection.Forms.FormGridFunctions.AskIfReplacementShouldBeStopped(ColumnName, Value, ReplaceOptionStatus);
                }
            }
            catch { }
            return Value;
        }

        #endregion

        #region Treeview

        public static void setTitleInTreeView(System.Windows.Forms.TreeNode TreeNode)
        {
            try
            {
                GridModeQueryField Q = DiversityCollection.Forms.FormGridFunctions.GridModeQueryFieldOfNode(TreeNode);
                System.Collections.Generic.Dictionary<string, string> EntityInfo = new Dictionary<string, string>();
                if (Q.Entity != null && Q.Entity.Length > 0
                    && (Q.Restriction == null || Q.Restriction.Length == 0)
                    && !Q.AliasForColumn.StartsWith("Link_to")
                    && !Q.AliasForColumn.StartsWith("Remove_link"))
                {
                    if (TreeNode.Text == "Link to Transaction")
                    {
                        TreeNode.Text = DiversityCollection.Forms.FormCollectionSpecimenGridModeText.Link_to_Transaction;
                    }
                    else
                    {
                        EntityInfo = DiversityWorkbench.Entity.EntityInformation(Q.Entity);
                        if (EntityInfo["DisplayTextOK"] == "True")
                            TreeNode.Text = EntityInfo["DisplayText"];
                        else
                        {
                        }
                    }
                }
                else if (TreeNode.Tag != null)
                {
                    if (Q.Entity != null && Q.Entity.Length > 0 && Q.Restriction != null
                        && (Q.Entity.EndsWith(".DistanceToLocation")
                        || Q.Entity.EndsWith(".DirectionToLocation")
                        || Q.Entity.EndsWith(".LocationAccuracy")))
                    {
                        EntityInfo = DiversityWorkbench.Entity.EntityInformation(Q.Entity);
                        if (EntityInfo["DisplayTextOK"] == "True")
                            TreeNode.Text = EntityInfo["DisplayText"];
                        else
                        {

                        }
                    }
                    else if (!TreeNode.Tag.ToString().Contains(";"))
                    {
                        EntityInfo = DiversityWorkbench.Entity.EntityInformation(TreeNode.Tag.ToString());
                        if (EntityInfo["DisplayTextOK"] == "True")
                            TreeNode.Text = EntityInfo["DisplayText"];
                        else
                        {
                        }
                    }
                    else
                    {
                        string Title = TreeNode.Text;
                        if (Title.EndsWith("SamplingPlots"))
                        {
                            EntityInfo = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID.13");
                            Title = Title.Replace("SamplingPlots", EntityInfo["DisplayText"]);
                        }
                        if (Title.StartsWith("Link to"))
                        {
                            Title = Title.Replace("Link to", DiversityCollection.Forms.FormCollectionSpecimenGridModeText.Link_to);
                        }
                        else if (Title.StartsWith("Remove link to"))
                        {
                            switch (DiversityWorkbench.Settings.Language)
                            {
                                case "de-DE":
                                    Title = Title.Replace("Remove link to", "Lösche Verbindung zu");
                                    break;
                            }
                        }
                        else if (Title.StartsWith("Remove link for"))
                        {
                            Title = Title.Replace("Remove link for", DiversityCollection.Forms.FormCollectionSpecimenGridModeText.Remove_link_for);
                        }
                        else
                        {
                            switch (Title)
                            {
                                case "Named area":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID.7");
                                    break;
                                case "MTB":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID.3");
                                    break;
                                case "Quadrant":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID.3.Quadrant");
                                    break;
                                case "Notes for MTB":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("CollectionEventLocalisation.LocationNotes");
                                    break;
                                case "Longitude":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("CollectionEventLocalisation.AverageLongitudeCache");
                                    break;
                                case "Latitude":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("CollectionEventLocalisation.AverageLatitudeCache");
                                    break;
                                case "Altitude to":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID.4.AltitudeTo");
                                    break;
                                case "Altitude from":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("CollectionEventLocalisation.AverageAltitudeCache");
                                    break;
                                case "Sampling plot":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID.13");
                                    break;
                                case "Geographic region":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("Property.PropertyID.10");
                                    break;
                                case "Lithostratigraphy":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("Property.PropertyID.30");
                                    break;
                                case "Chronostratigraphy":
                                    EntityInfo = DiversityWorkbench.Entity.EntityInformation("Property.PropertyID.20");
                                    break;
                                default:
                                    EntityInfo = new Dictionary<string, string>();
                                    break;
                            }
                            if (EntityInfo.ContainsKey("DisplayTextOK")
                                && EntityInfo["DisplayTextOK"] == "True")
                                Title = EntityInfo["DisplayText"];
                            else
                            {
                            }
                        }
                        TreeNode.Text = Title;
                    }
                }
                else
                {
                }
                foreach (System.Windows.Forms.TreeNode childN in TreeNode.Nodes)
                    DiversityCollection.Forms.FormGridFunctions.setTitleInTreeView(childN);
            }
            catch { }
        }

        private static GridModeQueryField GridModeQueryFieldOfNode(System.Windows.Forms.TreeNode N)
        {
            DiversityCollection.Forms.GridModeQueryField Q = new Forms.GridModeQueryField();
            try
            {
                if (N.Tag != null)
                {
                    string[] Parameter = DiversityCollection.Forms.FormGridFunctions.GridModeFieldTagArray(N.Tag.ToString());
                    if (Parameter.Length > 1)
                    {
                        Q.Table = Parameter[0];

                        Q.AliasForTable = Parameter[1];
                        if (Q.AliasForTable.Length == 0) Q.AliasForTable = Q.Table;

                        Q.Restriction = Parameter[2];

                        Q.Column = Parameter[3];

                        if (Parameter.Length > 4)
                            Q.AliasForColumn = Parameter[4];
                        else Q.AliasForColumn = "";
                        if (Q.AliasForColumn.Length == 0)
                            Q.AliasForColumn = Q.Column;

                        if (Parameter.Length > 5)
                            Q.DatasourceTable = Parameter[5];
                        else Q.DatasourceTable = "";

                        if (N.Checked)
                            Q.IsVisible = true;
                        else Q.IsVisible = false;
                        Q.IsHidden = false;
                        if (Parameter.Length > 7)
                            Q.RemoveLinkColumn = Parameter[7];
                        else Q.RemoveLinkColumn = "";
                        if (Q.Table.Length > 0)
                            Q.Entity = Q.Table;
                        if (Q.Column.Length > 0)
                            Q.Entity += "." + Q.Column;
                    }
                }

            }
            catch { }
            return Q;
        }

        #endregion

        #region Row numbers for Grid & Counting

        public static string GridCounter(System.Windows.Forms.DataGridView dataGridView)
        {
            string Counter = "";
            int LinesInGrid = dataGridView.Rows.Count;
            if (dataGridView.AllowUserToAddRows) LinesInGrid--;
            if (dataGridView.SelectedCells.Count == 1)
                Counter = "line " + (dataGridView.SelectedCells[0].RowIndex + 1).ToString() + " of " + (LinesInGrid).ToString();
            else if (dataGridView.SelectedCells.Count > 1)
            {
                int StartLine = dataGridView.SelectedCells[0].RowIndex + 1;
                if (dataGridView.SelectedCells[dataGridView.SelectedCells.Count - 1].RowIndex + 1 < StartLine)
                    StartLine = dataGridView.SelectedCells[dataGridView.SelectedCells.Count - 1].RowIndex + 1;
                Counter = dataGridView.SelectedCells.Count.ToString() + " lines (" + StartLine.ToString() + " - " +
                    (dataGridView.SelectedCells.Count + StartLine - 1).ToString() + ") of " +
                    (LinesInGrid).ToString();
            }
            else
                Counter = "line 1 of " + (LinesInGrid).ToString();
            return Counter;
        }

        public static void DrawRowNumber(System.Windows.Forms.DataGridView dataGridView, System.Drawing.Font Font, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                string rowNumber =
                        (e.RowIndex + 1).ToString()
                        .PadLeft(dataGridView.RowCount.ToString().Length);

                // Schriftgröße:
                System.Drawing.SizeF size = e.Graphics.MeasureString(rowNumber, Font);

                // Breite des ZeilenHeaders anpassen:
                if (dataGridView.RowHeadersWidth < (int)(size.Width + 20))
                    dataGridView.RowHeadersWidth = (int)(size.Width + 20);

                // ZeilenNr zeichnen:
                e.Graphics.DrawString(
                    rowNumber,
                    Font,
                    System.Drawing.SystemBrushes.ControlText,
                    e.RowBounds.Location.X + 15,
                    e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
            }
            catch (Exception ex)
            {
            }
            return;
        }

        #endregion

        #region Autosuggest


        #endregion

        #endregion

    }
}
