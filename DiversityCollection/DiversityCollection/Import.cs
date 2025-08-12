using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection
{

    public interface iImportInterface
    {
        /// <summary>
        /// display the import column definitions that where set for a column in the datagrid
        /// </summary>
        /// <param name="Position">The position in the datagrid</param>
        void setColumnDisplays(int Position);
        void setColumnHeaeder(int Position);
        /// <summary>
        /// The key (Table.Column) of the current import column resp. user control
        /// </summary>
        /// <returns></returns>
        string getCurrentImportKey();
        void setCurrentImportColumn();
        void setCurrentImportColumn(DiversityCollection.Import_Column ImportColumn);
        System.Windows.Forms.Button ButtonForward();
        System.Windows.Forms.Button ButtonBack();
        /// <summary>
        /// The panel where the import steps are listed
        /// </summary>
        /// <returns></returns>
        System.Windows.Forms.Panel PanelForImportSteps();
        /// <summary>
        /// The datagrid where the data of the file are inserted
        /// </summary>
        /// <returns></returns>
        System.Windows.Forms.DataGridView Grid();
        void setDataGridLineColor(int iLine, System.Drawing.Color Color);
        /// <summary>
        /// The first line that should be imported from the file
        /// </summary>
        /// <returns>Number of first line</returns>
        int FirstLineForImport();
        /// <summary>
        /// The last line that should be imported from the file
        /// </summary>
        /// <returns>Number of last line</returns>
        int LastLineForImport();
        void AddImportStep(string StepKey);
        void AddImportColumn(DiversityCollection.Import_Column ImportColumn);
        DiversityCollection.Import getImport();
        DiversityCollection.UserControls.UserControlImport_Column UserControlImportAttachmentColumn();
    }

    public class Import
    {
        #region static functions and properties

        #region File handling

        public static bool readFileInDataGridView(System.IO.FileInfo File, System.Windows.Forms.DataGridView Grid, System.Text.Encoding Encoding, int? EndLine)
        {
            Grid.Columns.Clear();
            try
            {
                System.IO.StreamReader sr = DiversityCollection.Import.StreamReader(File.FullName, Encoding);
                using (sr)
                {
                    String line;
                    int iLine = 0;
                    int iColumn = 0;
                    bool HeaderPassed = false;
                    bool EmptyLinePassed = false;
                    int iHeader = 0;
                    // reading the first for lines into the datagrid
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (EndLine != null && iLine > EndLine)
                            break;
                        // reading the columns
                        if (iLine == 0)
                        {
                            string Columns = line;
                            while (Columns.Length > 0)
                            {
                                string Column = "";
                                if (Columns.IndexOf("\t") > -1)
                                    Column = Columns.Substring(0, Columns.IndexOf("\t")).ToString().Trim();
                                else
                                {
                                    Column = Columns.Trim();
                                    Columns = "";
                                }
                                iColumn++;
                                Grid.Columns.Add("Column_" + iColumn.ToString(), "");
                                Columns = Columns.Substring(Columns.IndexOf("\t") + 1);
                            }
                        }
                        // reading the lines
                        Grid.Rows.Add(1);
                        iColumn = 0;
                        if (!HeaderPassed || line.Replace("\t", "").Trim().Length == 0)
                        {
                            // skip the header lines
                            if (line.Replace("\t", "").Trim().Length == 0 && iHeader > 1)
                                EmptyLinePassed = true;
                            iHeader++;
                            if (iHeader > 3 && EmptyLinePassed)
                                HeaderPassed = true;
                            Grid.Rows[iLine].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
                        }
                        while (line.Length > 0)
                        {
                            if (line.Replace("\t", "").Trim().Length == 0)
                                line = "";
                            else if (line.IndexOf("\t") > -1)
                                Grid.Rows[iLine].Cells[iColumn].Value = line.Substring(0, line.IndexOf("\t")).ToString().Trim();
                            else
                            {
                                Grid.Rows[iLine].Cells[iColumn].Value = line.Trim();
                                line = "";
                            }
                            line = line.Substring(line.IndexOf("\t") + 1);
                            iColumn++;
                        }
                        iLine++;
                    }
                }
                foreach (System.Windows.Forms.DataGridViewColumn C in Grid.Columns)
                    C.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            }
            catch (System.IO.IOException IOex)
            {
                System.Windows.Forms.MessageBox.Show(IOex.Message);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(IOex);
                return false;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return false;
            }
            return true;
        }

        private static System.IO.StreamReader StreamReader(string File, System.Text.Encoding Encoding)
        {
            System.IO.StreamReader sr;
            sr = new System.IO.StreamReader(File, Encoding);
            return sr;
        }

        private static System.IO.StreamWriter StreamWriter(string File, System.Text.Encoding Encoding)
        {
            System.IO.StreamWriter sr;
            sr = new System.IO.StreamWriter(File, false, Encoding);
            return sr;
        }

        public static string CopyFileAccordingToDataGridView(
            System.IO.FileInfo SourceFile, 
            string CopyFilePostfix, 
            System.Text.Encoding Encoding, 
            int StartLine, 
            int EndLine,
            bool FirstLineContainsColumnDefinition)
        {
            string FileNameCopy = SourceFile.FullName.Substring(0, SourceFile.FullName.Length - (SourceFile.Extension.Length)) + CopyFilePostfix + SourceFile.Extension;
            try
            {
                System.IO.StreamReader sr = DiversityCollection.Import.StreamReader(SourceFile.FullName, Encoding);
                System.IO.StreamWriter sw = DiversityCollection.Import.StreamWriter(FileNameCopy, Encoding);
                using (sr)
                {
                    String line;
                    int iLine = 0;
                    //int i = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        iLine++;
                        if (iLine > EndLine)
                            break;
                        //if (iLine > 0 
                        //    && 
                        //    (iLine < StartLine ||
                        //    !DiversityCollection.Import._NumberOfFailedLines.Contains(iLine - 1))
                        //    &&
                        //    (!FirstLineContainsColumnDefinition && iLine == 1))
                        //    continue;
                        if (FirstLineContainsColumnDefinition && iLine == 1)
                            sw.WriteLine(line);
                        else if (DiversityCollection.Import._NumberOfFailedLines.Contains(iLine - 1))
                            sw.WriteLine(line);
                         
                    }
                    sw.Close();
                }
            }
            catch (System.IO.IOException IOex)
            {
                //System.Windows.Forms.MessageBox.Show(IOex.Message);
                //return false;
            }
            catch (System.Exception ex)
            {
                //return false;
            }
            return FileNameCopy;
        }

        private static System.Collections.Generic.List<int> _NumberOfFailedLines = new List<int>();
        
        #endregion

        #region Tabcontrols

        private static System.Collections.Generic.Dictionary<System.Windows.Forms.TabPage, System.Windows.Forms.TabControl> _TabPagesControl;

        public static System.Collections.Generic.Dictionary<System.Windows.Forms.TabPage, System.Windows.Forms.TabControl> TabPagesControl
        {
            get
            {
                if (DiversityCollection.Import._TabPagesControl == null)
                    DiversityCollection.Import._TabPagesControl = new Dictionary<System.Windows.Forms.TabPage, System.Windows.Forms.TabControl>();
                return _TabPagesControl;
            }
            //set { _TabPagesControl = value; }
        }

        public static void initTabPagesControl(System.Windows.Forms.TabControl TabControl)
        {
            foreach (System.Windows.Forms.TabPage T in TabControl.TabPages)
            {
                if (!DiversityCollection.Import.TabPagesControl.ContainsKey(T))
                    DiversityCollection.Import.TabPagesControl.Add(T, TabControl);
            }
        }

        public static void resetTabPagesControl(System.Windows.Forms.TabControl TabControl)
        {
            foreach (System.Windows.Forms.TabPage T in TabControl.TabPages)
            {
                if (DiversityCollection.Import.TabPagesControl.ContainsKey(T))
                    DiversityCollection.Import.TabPagesControl.Remove(T);
            }
        }

        #endregion

        #region Selection

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, DiversityCollection.UserControls.UserControlImportSelector>> _ImportSelectors;

        public static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, DiversityCollection.UserControls.UserControlImportSelector>> ImportSelectors
        {
            get
            {
                if (Import._ImportSelectors == null)
                    Import._ImportSelectors = new Dictionary<string, Dictionary<string, DiversityCollection.UserControls.UserControlImportSelector>>();
                return Import._ImportSelectors;
            }
            //set { Import._ImportSelectors = value; }
        }

        public static void AddImportSelector(string Group, string Selector, DiversityCollection.Import_Step ImportStep, System.Windows.Forms.Panel SelectionPanel)
        {
            bool AddToPanel = false;
            DiversityCollection.UserControls.UserControlImportSelector U = new DiversityCollection.UserControls.UserControlImportSelector(ImportStep, Selector);
            if (!Import.ImportSelectors.ContainsKey(Group))
            {
                System.Collections.Generic.Dictionary<string, DiversityCollection.UserControls.UserControlImportSelector> Dict = new Dictionary<string, DiversityCollection.UserControls.UserControlImportSelector>();
                Dict.Add(Selector, U);
                Import.ImportSelectors.Add(Group, Dict);
                AddToPanel = true;
            }
            else
            {
                if (!Import.ImportSelectors[Group].ContainsKey(Selector))
                {
                    Import.ImportSelectors[Group].Add(Selector, U);
                    AddToPanel = true;
                }
                else
                {
                    Import.ImportSelectors[Group][Selector].addImportStep(ImportStep);
                }
            }
            if (AddToPanel)// || !SelectionPanel.Controls.Contains(UserControlImportSelector))
            {
                SelectionPanel.Controls.Add(U);
                U.Dock = System.Windows.Forms.DockStyle.Top;
                U.BringToFront();
            }
        }

        #endregion

        #region Steps

        private static System.Collections.Generic.SortedList<string, Import_Step> _ImportSteps;

        public static System.Collections.Generic.SortedList<string, Import_Step> ImportSteps
        {
            get
            {
                if (DiversityCollection.Import._ImportSteps == null)
                    DiversityCollection.Import._ImportSteps = new SortedList<string, Import_Step>();
                return DiversityCollection.Import._ImportSteps;
            }
        }

        public static void MoveToNextStep()
        {
            try
            {
                if (DiversityCollection.Import.ImportSteps.Count > 0)
                {
                    System.Collections.Generic.SortedList<int, string> ListOfSteps = new SortedList<int, string>();
                    int i = 0;
                    int iCurrent = 0;
                    int iNext = 0;
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DiversityCollection.Import.ImportSteps)
                    {
                        ListOfSteps.Add(i, KV.Key);
                        if (KV.Value.UserControlImportStep.IsCurrent)
                        {
                            iCurrent = i;
                            iNext = i + 1;
                        }
                        i++;
                    }
                    if (iNext > ListOfSteps.Count - 1)
                        iNext--;
                    bool IsVisible = DiversityCollection.Import.ImportSteps[ListOfSteps[iNext]].IsVisible();//.UserControlImportStep.Visible;
                    while (!IsVisible)
                    {
                        iNext++;
                        IsVisible = DiversityCollection.Import.ImportSteps[ListOfSteps[iNext]].IsVisible();//.UserControlImportStep.Visible;
                    }
                    if (DiversityCollection.Import.ImportSteps[ListOfSteps[iNext]].IsGroupHaeder)
                        iNext++;
                    if (ListOfSteps.ContainsKey(iNext) && DiversityCollection.Import.ImportSteps.ContainsKey(ListOfSteps[iNext]))
                    {
                        DiversityCollection.Import.ImportSteps[ListOfSteps[iCurrent]].UserControlImportStep.IsCurrent = false;
                        DiversityCollection.Import.ImportSteps[ListOfSteps[iNext]].UserControlImportStep.IsCurrent = true;
                    }



                    DiversityCollection.Import.setCurrentImportColumn();
                    //System.Collections.Generic.IEnumerator<KeyValuePair<string, DiversityCollection.Import_Step>> e = (System.Collections.Generic.IEnumerator<KeyValuePair<string, DiversityCollection.Import_Step>>)this.ImportSteps.GetEnumerator();
                    //e.MoveNext();
                    //e.MoveNext();
                    //DiversityCollection.Import_Step I = this.ImportSteps[e.Current.Key];
                    //I.UserControlImportStep.IsCurrent = true;
                }
            }
            catch (System.Exception ex) { }
        }

        public static void MoveToPreviousStep()
        {
            if (DiversityCollection.Import.ImportSteps.Count > 0)
            {
                System.Collections.Generic.SortedList<int, string> ListOfSteps = new SortedList<int, string>();
                int i = 0;
                int iCurrent = 0;
                int iPrevious = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DiversityCollection.Import.ImportSteps)
                {
                    ListOfSteps.Add(i, KV.Key);
                    if (KV.Value.UserControlImportStep.IsCurrent && i > 0)
                    {
                        iCurrent = i;
                        iPrevious = i - 1;
                        if (iPrevious < 0)
                            iPrevious = 0;
                    }
                    i++;
                }
                bool IsVisible = DiversityCollection.Import.ImportSteps[ListOfSteps[iPrevious]].IsVisible();//.UserControlImportStep.Visible;
                while (!IsVisible)
                {
                    iPrevious--;
                    IsVisible = DiversityCollection.Import.ImportSteps[ListOfSteps[iPrevious]].IsVisible();//.UserControlImportStep.Visible;
                }
                if (DiversityCollection.Import.ImportSteps[ListOfSteps[iPrevious]].IsGroupHaeder)
                    iPrevious--;
                if (ListOfSteps.ContainsKey(iPrevious) && DiversityCollection.Import.ImportSteps.ContainsKey(ListOfSteps[iPrevious]))
                {
                    DiversityCollection.Import.ImportSteps[ListOfSteps[iCurrent]].UserControlImportStep.IsCurrent = false;
                    DiversityCollection.Import.ImportSteps[ListOfSteps[iPrevious]].UserControlImportStep.IsCurrent = true;
                }
            }
        }

        public static void MoveToStep(string Step)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DiversityCollection.Import.ImportSteps)
            {
                DiversityCollection.Import.ImportSteps[KV.Key].UserControlImportStep.IsCurrent = false;
            }
            if (DiversityCollection.Import.ImportSteps.ContainsKey(Step))
                DiversityCollection.Import.ImportSteps[Step].UserControlImportStep.IsCurrent = true;
        }

        public void HideCurrentImportStep()
        {
            string CurrentPosition = this.CurrentPosition;
            DiversityCollection.Import.ImportSteps[CurrentPosition].IsVisible(false);
            if (DiversityCollection.Import.ImportSteps[CurrentPosition].UserControlImportStep.IsCurrent)
                DiversityCollection.Import.MoveToNextStep();

            DiversityCollection.Import.ImportSteps[CurrentPosition].IsVisible(false);
            if (this.PanelImportSteps.Controls.Contains(DiversityCollection.Import.ImportSteps[CurrentPosition].UserControlImportStep))
                this.PanelImportSteps.Controls.Remove(DiversityCollection.Import.ImportSteps[CurrentPosition].UserControlImportStep);
        }

        private System.Collections.Generic.List<int> DataGridColumnsRelatedToImportStep(DiversityCollection.Import_Step ImportStep)
        {
            System.Collections.Generic.List<DiversityCollection.Import_Column> Columns = new List<Import_Column>();
            System.Collections.Generic.List<int> L = new List<int>();
            return L;
        }

        #endregion

        #region Import column

        //public static bool ColumnSelectionPending;

        private static System.Collections.Generic.Dictionary<string, DiversityCollection.Import_Column> _ImportColumns;

        public static System.Collections.Generic.Dictionary<string, Import_Column> ImportColumns
        {
            get
            {
                if (DiversityCollection.Import._ImportColumns == null)
                    DiversityCollection.Import._ImportColumns = new Dictionary<string, Import_Column>();
                return _ImportColumns;
            }
            //set { _ImportColumns = value; }
        }

        private static DiversityCollection.Import_Column _CurrentImportColumn;

        public static DiversityCollection.Import_Column CurrentImportColumn
        {
            get 
            {
                if (DiversityCollection.Import._CurrentImportColumn == null)
                    DiversityCollection.Import._CurrentImportColumn = new Import_Column();
                return DiversityCollection.Import._CurrentImportColumn; 
            }
            set { DiversityCollection.Import._CurrentImportColumn = value; }
        }

        private static DiversityCollection.Import_Column _AttachmentKeyImportColumn;
        /// <summary>
        /// The import column that is used for attaching new data to existing data in the database
        /// </summary>
        public static DiversityCollection.Import_Column AttachmentKeyImportColumn
        {
            get 
            {
                if (Import._AttachmentKeyImportColumn == null)
                    Import._AttachmentKeyImportColumn = new Import_Column();
                return Import._AttachmentKeyImportColumn; 
            }
            set
            {
                if (value == null)
                {
                    if (Import._AttachmentKeyImportColumn != null && ImportColumnControls.ContainsKey(Import._AttachmentKeyImportColumn.Key))
                    {
                        ImportColumnControls[Import._AttachmentKeyImportColumn.Key].ImportColumn().MustSelect = false;
                        //DiversityCollection.Import.ImportSteps[Import._AttachmentKeyImportColumn.ImportStep].setStepError();
                    }
                }
                else
                {
                    if (!ImportColumns.ContainsKey(value.Key))
                        ImportColumns.Add(value.Key, value);
                    if (Import.ImportColumnControls.ContainsKey(value.Key))
                    {
                        ImportColumnControls[value.Key].ImportColumn().MustSelect = true;
                        //DiversityCollection.Import.ImportSteps[Import._AttachmentKeyImportColumn.ImportStep].setStepError();
                    }
                }
                Import._AttachmentKeyImportColumn.IsDecisionColumn = false;
                Import._AttachmentKeyImportColumn.CanSetDecisionColumn = false;
                Import._AttachmentKeyImportColumn = value;
            }
        }

        /// <summary>
        /// set the current import column to null, e.g. if no column should be selected
        /// </summary>
        public static void setCurrentImportColumn()
        {
            DiversityCollection.Import.CurrentImportColumn = null;
        }

        /// <summary>
        /// set the current import column to the given value
        /// </summary>
        /// <param name="ImportColumn">The import column</param>
        /// <returns>A message containing the instructions for selecting the column in the file</returns>
        public static string setCurrentImportColumn(DiversityCollection.Import_Column ImportColumn)
        {
            DiversityCollection.Import.CurrentImportColumn = ImportColumn;
            string Message = "Please select a column in the table with data for the import into the database ";
            if (DiversityCollection.Import.CurrentImportColumn.TableAlias != null &&
                DiversityCollection.Import.CurrentImportColumn.TableAlias.Length > 0 &&
                DiversityCollection.Import.CurrentImportColumn.TableAlias != DiversityCollection.Import.CurrentImportColumn.Table)
                Message += "for the " + DiversityCollection.Import.CurrentImportColumn.TableAlias + " ";
            Message += "in the table " + DiversityCollection.Import.CurrentImportColumn.Table + " " +
                "and the column " + DiversityCollection.Import.CurrentImportColumn.Column;
            return Message;
        }

        private static System.Collections.Generic.Dictionary<string, iImportColumnControl> _ImportColumnControls;
        public static System.Collections.Generic.Dictionary<string, iImportColumnControl> ImportColumnControls
        {
            get
            {
                if (_ImportColumnControls == null)
                    Import._ImportColumnControls = new Dictionary<string, iImportColumnControl>();
                return Import._ImportColumnControls;
            }
            //set { Import._ImportColumnControls = value; }
        }

        //public static void setCurrentImportColumn(string StepPosition, string ImportColumn)
        //{
        //    DiversityCollection.Import_Column C = new Import_Column(StepPosition);
        //    //DiversityCollection.Import.ColumnSelectionPending = true;
        //    try
        //    {
        //        string[] IC = ImportColumn.Split(new char[] { '.' });
        //        C.Table = IC[0];
        //        C.Column = IC[1];
        //        DiversityCollection.Import.setCurrentImportColumn(C);
        //    }
        //    catch { }
        //}

        //public static void setCurrentImportColumn(string StepPosition, System.Windows.Forms.Control C)
        //{
        //    DiversityCollection.Import_Column IC = new Import_Column(StepPosition);
        //    //DiversityCollection.Import.ColumnSelectionPending = true;
        //    try
        //    {
        //        string[] CC = C.AccessibleName.Split(new char[] { '.' });
        //        IC.Table = CC[0];
        //        IC.Column = CC[1];
        //        DiversityCollection.Import.setCurrentImportColumn(IC);
        //    }
        //    catch { }
        //}

        #endregion

        #region Tables and Import

        public static System.Collections.Generic.Dictionary<string, DiversityCollection.Import_Table> ImportTablesDataTreatment;

        private static System.Collections.Generic.Dictionary<string, DiversityCollection.Import_Table> _ImportTables;

        public static System.Collections.Generic.Dictionary<string, Import_Table> ImportTables
        {
            get
            {
                if (DiversityCollection.Import._ImportTables == null)
                     DiversityCollection.Import._ImportTables = new Dictionary<string,Import_Table>();
                return DiversityCollection.Import._ImportTables;
            }
        }

        public void initImportTablesAccordingToSettings()
        {
            if (DiversityCollection.Import._ImportTables != null)
                return;

            DiversityCollection.Import._ImportTables = new Dictionary<string, Import_Table>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> IS in DiversityCollection.Import.ImportSteps)
            {
                if (!IS.Value.IsVisible())
                    continue;
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> IC in DiversityCollection.Import._ImportColumns)
                {
                    if (IC.Value.StepKey == IS.Key && !IC.Value.IsSelected && IC.Value.MustSelect)
                    {
                        IC.Value.IsSelected = true;
                        IC.Value.ImportColumnControl.setInterface();
                    }

                    if (IC.Value.StepKey == IS.Key && IC.Value.IsSelected)
                    {
                        if (!DiversityCollection.Import._ImportTables.ContainsKey(IC.Value.TableAlias))
                        {
                            DiversityCollection.Import_Table T = new Import_Table(IC.Value.TableAlias, IC.Value.Table, DiversityCollection.Import.DataGridView);
                            DiversityCollection.Import._ImportTables.Add(IC.Value.TableAlias, T);
                            // set the data treatment for attached tables
                            if (DiversityCollection.Import.ImportTablesDataTreatment != null &&
                                DiversityCollection.Import.ImportTablesDataTreatment.ContainsKey(IC.Value.TableAlias))
                            {
                                DiversityCollection.Import._ImportTables[IC.Value.TableAlias].TreatmentOfData = DiversityCollection.Import.ImportTablesDataTreatment[IC.Value.TableAlias].TreatmentOfData;
                                DiversityCollection.Import._ImportTables[IC.Value.TableAlias].TreatmentOfErrors = DiversityCollection.Import.ImportTablesDataTreatment[IC.Value.TableAlias].TreatmentOfErrors;
                            }
                        }
                    }
                }
            }
        }

        public void ResetImportTables()
        {
            DiversityCollection.Import._ImportTables = null;
        }

        public static System.Windows.Forms.DataGridView DataGridView;

        private static System.Collections.Generic.List<string> _DecisionStepsWithNoValues;
        public static System.Collections.Generic.List<string> DecisionStepsWithNoValues
        {
            get
            {
                if (DiversityCollection.Import._DecisionStepsWithNoValues == null)
                    DiversityCollection.Import._DecisionStepsWithNoValues = new List<string>();
                return DiversityCollection.Import._DecisionStepsWithNoValues;
            }
        }

        #endregion

        #region Settings

        private static System.Collections.Generic.Dictionary<DiversityCollection.Import.Setting, string> _Settings;

        public static void AddSetting(DiversityCollection.Import.Setting Setting, string Value)
        {
            if (DiversityCollection.Import._Settings == null)
                DiversityCollection.Import._Settings = new Dictionary<DiversityCollection.Import.Setting, string>();
            if (DiversityCollection.Import._Settings.ContainsKey(Setting))
                DiversityCollection.Import._Settings[Setting] = Value;
            else
                DiversityCollection.Import._Settings.Add(Setting, Value);
        }

        public static void AddSetting(string Setting, string Value)
        {
            if (DiversityCollection.Import._Settings == null)
                DiversityCollection.Import._Settings = new Dictionary<DiversityCollection.Import.Setting, string>();
            if (Setting == DiversityCollection.Import.Setting.Attachment.ToString())
                DiversityCollection.Import.AddSetting(DiversityCollection.Import.Setting.Attachment, Value);
            else if (Setting == DiversityCollection.Import.Setting.Encoding.ToString())
                DiversityCollection.Import.AddSetting(DiversityCollection.Import.Setting.Encoding, Value);
            else if (Setting == DiversityCollection.Import.Setting.EndLine.ToString())
                DiversityCollection.Import.AddSetting(DiversityCollection.Import.Setting.EndLine, Value);
            else if (Setting == DiversityCollection.Import.Setting.StartLine.ToString())
                DiversityCollection.Import.AddSetting(DiversityCollection.Import.Setting.StartLine, Value);
            else if (Setting == DiversityCollection.Import.Setting.TrimValues.ToString())
                DiversityCollection.Import.AddSetting(DiversityCollection.Import.Setting.TrimValues, Value);
        }

        public static string GetSetting(DiversityCollection.Import.Setting Setting)
        {
            if (DiversityCollection.Import._Settings.ContainsKey(Setting))
                return DiversityCollection.Import._Settings[Setting];
            else return "";
        }

        private static System.Collections.Generic.Dictionary<string, System.Object> _SettingObjects;
        public static System.Collections.Generic.Dictionary<string, System.Object> SettingObjects
        {
            get
            {
                if (DiversityCollection.Import._SettingObjects == null)
                    DiversityCollection.Import._SettingObjects = new Dictionary<string, System.Object>();
                return _SettingObjects;
            }
            set { _SettingObjects = value; }
        }

        #endregion

        #region Special list for Series and Event IDs
        
        private static System.Collections.Generic.List<int> _SeriesIDList;

        public static System.Collections.Generic.List<int> SeriesIDList
        {
            get
            {
                if (DiversityCollection.Import._SeriesIDList == null)
                    DiversityCollection.Import._SeriesIDList = new List<int>();
                return DiversityCollection.Import._SeriesIDList;
            }
            set { DiversityCollection.Import._SeriesIDList = value; }
        }

        private static System.Collections.Generic.List<int> _EventIDList;

        public static System.Collections.Generic.List<int> EventIDList
        {
            get
            {
                if (DiversityCollection.Import._EventIDList == null)
                    DiversityCollection.Import._EventIDList = new List<int>();
                return DiversityCollection.Import._EventIDList;
            }
            set { DiversityCollection.Import._EventIDList = value; }
        }
        
        #endregion

        #endregion

        #region Parameter

        private System.Windows.Forms.Panel _PanelImportSteps;

        //private System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<Import_Column>> _ImportColumns;

        private System.Collections.Generic.List<Import_Column> _PresetColumns;

        private iImportInterface _ImportInterface;

        public iImportInterface ImportInterface
        {
            get { return _ImportInterface; }
            set { _ImportInterface = value; }
        }

        private System.Windows.Forms.DataGridView _Grid;
        /// <summary>
        /// the list of controls that are directly handeld
        /// </summary>
        private System.Collections.Generic.Dictionary<string, System.Windows.Forms.Control> _SettingControls;

        #region Enums

        public enum Setting { Encoding, StartLine, EndLine, TrimValues, Attachment, AttachmentColumnInSourceFile }; //, Project, EventHierarchySeparate, EventHierarchyGroups };

        public enum ImportStep { File, Series, Event, Specimen, Project, Relation, Collector, Organism, Storage, UnitInPart, Images, Summary };
        public enum ImportStepNew { File, Series, Event, Specimen, Project, Relation, Collector, Storage, Organism, UnitInPart, Images, Summary };
        public enum ImportStepEvent { Date, Locality, Altitude, Coordinates, Place, MTB, GaussKrueger, Depth, Height, Exposition, Slope, Plot, Chronostratigraphy, Lithostratigraphy, Method };
        public enum ImportStepEventMethod { Parameter };
        public enum ImportStepSpecimen { Accession, Depositor, Notes, Reference, Label, Relations };
        public enum ImportStepUnit { TaxonomicGroup, Identification, Analysis };
        //public enum ImportStepUnitAnalysisMethod { Method };
        //public enum ImportStepUnitAnalysisMethodParameter { Parameter };
        public enum ImportStepIdentification { TaxonomicName, ResponsibleDate, NotesReferenceType };
        public enum ImportStepStorage { Storage, Processing, Transaction };

        public static string getImportStepKey(DiversityCollection.Import_Step Import_Step)
        {
            string Key = Import_Step.StepKey();
            if (Import_Step.SuperiorImportStep != null)
                Key = Import_Step.SuperiorImportStep.StepKey() + Key;
            return Key;
        }

        public static string getImportStepKey(DiversityCollection.Import_Step Import_Step, int ParallelNumber)
        {
            string Key = Import_Step.StepKey();
            if (Import_Step.SuperiorImportStep != null)
                Key = Import_Step.SuperiorImportStep.StepKey() + Key;
            return Key;
        }

        public static string getImportStepKey(DiversityCollection.Import.ImportStep ImportStep)
        {
            string Key = ((int)ImportStep).ToString();
            if (Key.Length == 1) Key = "0" + Key;
            Key = Import_Step.StepKeySeparator + Key;
            return Key;
        }

        public static string getImportStepKey(DiversityCollection.Import.ImportStepEvent ImportStep)
        {
            string Key = ((int)ImportStep).ToString();
            if (Key.Length == 1) Key = "0" + Key;// ((int)DiversityCollection.Import.ImportStep.Event).ToString()
            Key = DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Event) + Import_Step.StepKeySeparator + Key;
            return Key;
        }

        public static string getImportStepKey(DiversityCollection.Import.ImportStepSpecimen ImportStep)
        {
            string Key = ((int)ImportStep).ToString();
            if (Key.Length == 1) Key = "0" + Key;
            Key = DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Specimen) + Import_Step.StepKeySeparator + Key;
            return Key;
        }

        public static string getImportStepKey(DiversityCollection.Import.ImportStep ImportStep, int ParallelImportNumber)
        {
            string Key = DiversityCollection.Import.getImportStepKey(ImportStep);
            string P = ParallelImportNumber.ToString();
            if (P.Length == 1) P = "0" + P;
            else if (P.Length > 2) P = P.Substring(0, 2);
            Key += Import_Step.StepKeyParallelNumberSeparator + P;
            return Key;
        }

        //public static string getImportStepKey(DiversityCollection.Import_Step Import_Step, DiversityCollection.Import.ImportStepUnitAnalysisMethod ImportStep, int ParallelImportNumber)
        //{
        //    string Key = Import_Step.StepKey();
        //    Key += "_0" + ((int)ImportStep).ToString();
        //    string P = ParallelImportNumber.ToString();
        //    if (P.Length == 1) P = "0" + P;
        //    else if (P.Length > 2) P = P.Substring(0, 2);
        //    Key += Import_Step.StepKeyParallelNumberSeparator + P;
        //    return Key;
        //}

        //public static string getImportStepKey(DiversityCollection.Import_Step Import_Step, DiversityCollection.Import.ImportStepUnitAnalysisMethod ImportStep)
        //{
        //    string Key = Import_Step.StepKey();
        //    Key += "_";
        //    if ((int)ImportStep < 10)
        //        Key += "0";
        //    Key += ((int)ImportStep).ToString();
        //    return Key;
        //}

        public static string getImportStepKeyFirstChild(DiversityCollection.Import_Step ImportStep)
        {
            string Key = DiversityCollection.Import.getImportStepKey(ImportStep);
            string KeyChild = Key + ":01";
            if (DiversityCollection.Import.ImportSteps.ContainsKey(KeyChild))
                Key = KeyChild;
            return Key;
        }

        #endregion

        #endregion

        #region Properties

        //public System.Collections.Generic.Dictionary<System.Windows.Forms.TabPage, System.Windows.Forms.TabControl> TabPagesControl
        //{
        //    get 
        //    {
        //        if (this._TabPagesControl == null)
        //            this._TabPagesControl = new Dictionary<System.Windows.Forms.TabPage, System.Windows.Forms.TabControl>();
        //        return _TabPagesControl; 
        //    }
        //    //set { _TabPagesControl = value; }
        //}

        //public void initTabPagesControl(System.Windows.Forms.TabControl TabControl)
        //{
        //    foreach (System.Windows.Forms.TabPage T in TabControl.TabPages)
        //    {
        //        if (!this.TabPagesControl.ContainsKey(T))
        //            this.TabPagesControl.Add(T, TabControl);
        //    }
        //}

        private static bool? _MustSelectProject;
        private static bool MustSelectProject()
        {
            if (DiversityCollection.Import._MustSelectProject == null)
            {
                DiversityCollection.Import._MustSelectProject = false;
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> KV in DiversityCollection.Import.ImportColumns)
                {
                    if (KV.Value.IsSelected 
                        && KV.Value.Table != "CollectionEvent"
                        && KV.Value.Table != "CollectionEventSeries"
                        && KV.Value.Table != "CollectionProject")
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> IS in DiversityCollection.Import.ImportSteps)
                        {
                            if (IS.Value.TableName() == KV.Value.Table)
                            {
                                DiversityCollection.UserControls.UserControlImportStep U = IS.Value.UserControlImportStep;
                                DiversityCollection.Import._MustSelectProject = true;
                                break;
                            }
                        }
                    }
                }
            }
            return (bool)DiversityCollection.Import._MustSelectProject;
        }
        private static void MustSelectProjectReset()
        {
            DiversityCollection.Import._MustSelectProject = null;
        }


        #endregion

        #region Construction

        public Import(iImportInterface ImportInterface)
        {
            this._ImportInterface = ImportInterface;
            this.PanelImportSteps = ImportInterface.PanelForImportSteps();
            this._Grid = ImportInterface.Grid();
            DiversityCollection.Import.DataGridView = this._Grid;
        }

        #endregion

        #region Schema file

        #region Save
        
        public void SaveSchemaFile(string FileName, string Message)
        {
            System.Xml.XmlWriter W;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            W = System.Xml.XmlWriter.Create(FileName, settings);
            try
            {
                W.WriteStartDocument();
                W.WriteStartElement("ImportSchedule");
                W.WriteAttributeString("version", "2");

                this.SaveImportHeader(ref W, Message);

                if (Message != null)
                    this.SaveImportErrors(ref W);

                this.SaveImportSettings(ref W);

                this.SaveImportColumns(ref W, Message);

                this.SaveImportTableAttachmentSettings(ref W);

                this.SaveImportSteps(ref W);

                W.WriteEndElement();//ImportSchema
                W.WriteEndDocument();
                W.Flush();
                W.Close();
            }
            catch (Exception ex)
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

        }

        private void SaveImportHeader(ref System.Xml.XmlWriter W, string Message)
        {
            W.WriteStartElement("Header");
            W.WriteElementString("Responsible", System.Environment.UserName);
            W.WriteElementString("Date", System.DateTime.Now.ToLongDateString());
            if (Message != null)
            {
                W.WriteElementString("Time", System.DateTime.Now.ToLongTimeString());
                W.WriteElementString("Server", DiversityWorkbench.Settings.DatabaseServer);
                W.WriteElementString("Database", DiversityWorkbench.Settings.DatabaseName);
                if (!DiversityWorkbench.Settings.IsTrustedConnection)
                    W.WriteElementString("DatabaseUser", DiversityWorkbench.Settings.DatabaseUser);
                W.WriteElementString("ImportedLines", this._NumberOfImportedLines.ToString());
                W.WriteElementString("FailedLines", DiversityCollection.Import._NumberOfFailedLines.Count.ToString());
                W.WriteElementString("Report", Message);
            }
            W.WriteEndElement(); //Header
        }

        private void SaveImportErrors(ref System.Xml.XmlWriter W)
        {
            // Settings
            W.WriteStartElement("ImportErrors");
            for (int i = this._ImportInterface.FirstLineForImport() - 1; i < this._ImportInterface.LastLineForImport(); i++)
            {
                bool ErrorFound = false;
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Table> TT in DiversityCollection.Import._ImportTables)
                {
                    if (TT.Value.getImportErrors().ContainsKey(i))
                        ErrorFound = true;
                }
                if (ErrorFound)
                {
                    W.WriteStartElement("ImportError");
                    W.WriteElementString("Line", (i + 1).ToString());
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Table> TT in DiversityCollection.Import._ImportTables)
                    {
                        if (TT.Value.getImportErrors().ContainsKey(i))
                        {
                            W.WriteStartElement("Table");
                            W.WriteElementString("TableName", TT.Key);
                            W.WriteElementString("Error", TT.Value.getImportErrors()[i]);
                            W.WriteEndElement(); //Table
                        }
                    }
                    W.WriteEndElement(); //ImportError
                }
            }
            W.WriteEndElement(); //ImportErrors
        }

        private void SaveImportSettings(ref System.Xml.XmlWriter W)
        {
            // Settings
            W.WriteStartElement("ImportSettings");
            foreach (System.Collections.Generic.KeyValuePair<DiversityCollection.Import.Setting, string> KV in DiversityCollection.Import._Settings)
            {
                W.WriteStartElement("ImportSetting");
                W.WriteElementString("Key", KV.Key.ToString());
                W.WriteElementString("Value", KV.Value);
                W.WriteEndElement(); //Setting
            }
            W.WriteEndElement(); //Settings
        }

        private void SaveImportColumns(ref System.Xml.XmlWriter W, string Message)
        {
            // Columns
            W.WriteStartElement("ImportColumns");
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> IS in DiversityCollection.Import.ImportSteps)
            {
                System.Collections.Generic.SortedDictionary<string, DiversityCollection.Import_Column> CC = new SortedDictionary<string, Import_Column>();
                foreach (System.Collections.Generic.KeyValuePair<string, Import_Column> KV in DiversityCollection.Import.ImportColumns)
                {
                    if (KV.Value.StepKey == IS.Key)
                        CC.Add(KV.Key, KV.Value);
                }
                foreach (System.Collections.Generic.KeyValuePair<string, Import_Column> KV in CC)
                {
                    // for the protocoll save only the selected columns
                    if (Message != null && !KV.Value.IsSelected)
                        continue;
                    // Values that are entered via the interface and can not be fixed
                    if (KV.Value.TypeOfSource == Import_Column.SourceType.Interface
                        && KV.Value.TypeOfFixing != Import_Column.FixingType.Schema)
                        continue;
                    // Values that can be fixed in the schema but are missing
                    if (KV.Value.TypeOfSource == Import_Column.SourceType.Interface
                        && KV.Value.TypeOfFixing == Import_Column.FixingType.Schema
                        && !KV.Value.ValueIsFixed
                        && (KV.Value.Value == null || KV.Value.Value.Length == 0))
                        continue;
                    // Columns with no details
                    if (!KV.Value.ValueIsFixed
                        && KV.Value.TranslationDictionary == null
                        && KV.Value.Format == null
                        && !KV.Value.IsSelected
                        && !KV.Value.MultiColumn
                        && KV.Value.RegularExpressionPattern == null
                        && KV.Value.RegularExpressionReplacement == null
                        && KV.Value.Separator == " "
                        && KV.Value.Sequence() == 1
                        && KV.Value.SplitPosition == 1
                        && KV.Value.Splitters.Count == 0
                        && KV.Value.Value == null)
                        continue;
                    if (KV.Value.Key == "."
                        || KV.Value.Key.EndsWith(".")
                        || KV.Value.Key.StartsWith("."))
                        continue;
                    if (KV.Value.Table.StartsWith("CollectionAgent"))
                    { } // for testing
                    W.WriteStartElement("ImportColumn");
                    W.WriteElementString("ImportColumnKey", KV.Value.Key);
                    if (KV.Value.IsSelected && KV.Value.TypeOfSource == Import_Column.SourceType.File)
                    { }
                    W.WriteElementString("IsSelected", KV.Value.IsSelected.ToString());
                    //if (KV.Value.AlternativeColumn != null && KV.Value.AlternativeColumn.Length > 0)
                    //    W.WriteElementString("AlternativeColumn", KV.Value.AlternativeColumn);
                    W.WriteElementString("Column", KV.Value.Column);
                    if (KV.Value.StepKey != null)
                        W.WriteElementString("ImportStep", KV.Value.StepKey.ToString());
                    if (KV.Value.StepKey != null
                        && DiversityCollection.Import.ImportSteps.ContainsKey(KV.Value.StepKey)
                        && DiversityCollection.Import.ImportSteps[KV.Value.StepKey].SuperiorStepKey().Length > 0)
                        W.WriteElementString("SuperiorImportStep", DiversityCollection.Import.ImportSteps[KV.Value.StepKey].SuperiorStepKey());
                    if (KV.Value.ColumnInSourceFile != null)
                        W.WriteElementString("ColumnInSourceFile", KV.Value.ColumnInSourceFile.ToString());
                    if (KV.Value.Format != null && KV.Value.Format.Length > 0)
                        W.WriteElementString("Format", KV.Value.Format);
                    if (KV.Value.RegularExpressionPattern != null && KV.Value.RegularExpressionPattern.Length > 0)
                        W.WriteElementString("RegularExpressionPattern", KV.Value.RegularExpressionPattern);
                    if (KV.Value.RegularExpressionReplacement != null && KV.Value.RegularExpressionReplacement.Length > 0)
                        W.WriteElementString("RegularExpressionReplacement", KV.Value.RegularExpressionReplacement);
                    if (KV.Value.Separator != null && KV.Value.Separator.Length > 0 && KV.Value.Separator != " ")
                        W.WriteElementString("Separator", KV.Value.Separator);
                    if (KV.Value.IsDecisionColumn)
                        W.WriteElementString("IsDecisionColumn", KV.Value.IsDecisionColumn.ToString());
                    if (KV.Value.Sequence() > 1)
                        W.WriteElementString("Sequence", KV.Value.Sequence().ToString());
                    if (KV.Value.SplitPosition > 1)
                        W.WriteElementString("SplitPosition", KV.Value.SplitPosition.ToString());
                    if (KV.Value.Splitters != null && KV.Value.Splitters.Count > 0)
                    {
                        W.WriteStartElement("Splitters");
                        foreach (string s in KV.Value.Splitters)
                            W.WriteElementString("Splitter", s);
                        W.WriteEndElement(); // Splitters
                    }
                    W.WriteElementString("Table", KV.Value.Table);
                    W.WriteElementString("TypeOfSource", KV.Value.TypeOfSource.ToString());
                    if (KV.Value.TableAlias != null && KV.Value.TableAlias.Length > 0)
                        W.WriteElementString("TableAlias", KV.Value.TableAlias.ToString());
                    if (KV.Value.TranslationDictionary != null && KV.Value.TranslationDictionary.Count > 0)
                    {
                        W.WriteStartElement("TranslationDictionary");
                        foreach (System.Collections.Generic.KeyValuePair<string, string> tt in KV.Value.TranslationDictionary)
                        {
                            W.WriteStartElement("Translation");
                            W.WriteElementString("Key", tt.Key);
                            W.WriteElementString("Value", tt.Value);
                            W.WriteEndElement(); // Translation
                        }
                        W.WriteEndElement(); // TranslationDictionary
                    }
                    //if (KV.Value.TypeOfSource == Import_Column.SourceType.Interface && KV.Value.TypeOfFixing == Import_Column.FixingType.Schema && KV.Value.Value != null)
                    if (KV.Value.ValueIsFixed && KV.Value.Value != null && KV.Value.Value.Length > 0)
                        W.WriteElementString("Value", KV.Value.Value.ToString());
                    if (KV.Value.TypeOfSource == Import_Column.SourceType.Database)
                    {
                    }
                    W.WriteEndElement(); // ImportColumm
                }
            }
            W.WriteEndElement();//ImportColumns
        }

        private void SaveImportTableAttachmentSettings(ref System.Xml.XmlWriter W)
        {
            try
            {
                // TableAttachmentSettings
                if (DiversityCollection.Import.ImportTablesDataTreatment != null)
                {
                    W.WriteStartElement("ImportTableAttachmentSettings");
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Table> TT in DiversityCollection.Import.ImportTablesDataTreatment)
                    {
                        W.WriteStartElement("ImportTableAttachmentSetting");
                        W.WriteElementString("TableAlias", TT.Value.TableAlias);
                        W.WriteElementString("TableName", TT.Value.TableName);
                        W.WriteElementString("DataTreatment", TT.Value.TreatmentOfData.ToString());
                        W.WriteElementString("ErrorTreatment", TT.Value.TreatmentOfErrors.ToString());
                        W.WriteEndElement(); //TableAttachmentSetting
                    }
                    W.WriteEndElement(); //TableAttachmentSettings
                }
            }
            catch (System.Exception ex) { }
        }

        private void SaveImportSteps(ref System.Xml.XmlWriter W)
        {
            // Steps
            W.WriteStartElement("ImportSteps");
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> IS in DiversityCollection.Import.ImportSteps)
            {
                W.WriteStartElement("ImportStep");
                W.WriteElementString("Key", IS.Key);
                W.WriteElementString("Title", IS.Value.Title.ToString());
                W.WriteElementString("IsVisible", IS.Value.IsVisible().ToString());
                W.WriteEndElement(); //Step
            }
            W.WriteEndElement(); //Steps
        }
        
        #endregion

        #region Load

        public void LoadSchemaFile(string FileName)
        {
            this.ImportInterface.setCurrentImportColumn();
            System.Collections.Generic.List<DiversityCollection.ImportColumnGroup> IClist = new List<ImportColumnGroup>();
            //string Node = "";
            System.Xml.XmlTextReader R = new System.Xml.XmlTextReader(FileName);
            try
            {
                int iVersion = 0;
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.Name == "xml")
                        continue;
                    if (R.Name == "ImportSchedule")
                    {
                        try
                        {
                            if (R.NodeType == System.Xml.XmlNodeType.Element)
                            {
                                string Vers = R.GetAttribute("version");
                                int.TryParse(Vers, out iVersion);
                            }
                        }
                        catch(System.Exception ex)
                        {
                            this.SchemaVersion = 0;
                        }
                        continue;
                    }
                    if (R.IsStartElement() && R.Name == "ImportSettings")
                        this.LoadSettings(ref R);
                    else if (R.IsStartElement() && R.Name == "ImportColumns")
                        this.LoadColumns(ref R);
                    else if (R.IsStartElement() && R.Name == "ImportTableAttachmentSettings" && !R.IsEmptyElement)
                        this.LoadTableAttachmentSettings(ref R);
                    else if (R.IsStartElement() && R.Name == "ImportSteps")
                        this.LoadSteps(ref R);
                    else
                    {
                    }
                }
                this.SchemaVersion = iVersion;
                this.ImportStepsShow();
                DiversityCollection.Import.MoveToStep("_00");
            }
            catch (System.Exception ex)
            {
            }
            finally
            {
                R.ResetState();
                R.Close();
            }
        }

        /// <summary>
        /// The version of the schema file, necessary to handle different versions of the schema 
        /// e.g. later versions containing additional columns that are missing in previous versions
        /// </summary>
        private int _SchemaVersion = 0;
        
        private int SchemaVersion
        {
            get { return this._SchemaVersion; }
            set
            {
                this._SchemaVersion = value;
                switch (this._SchemaVersion)
                {
                    case 0:
                        try
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DiversityCollection.Import.ImportSteps)
                            {
                                if (KV.Value.TableName() == "CollectionEventMethod")// || KV.Value.TableName() == "CollectionEventMethodParameterValue")
                                {
                                    System.Windows.Forms.Panel P = KV.Value.SelectionPanel;
                                    foreach (System.Windows.Forms.Control C in P.Controls)
                                    {
                                        DiversityCollection.UserControls.UserControlImportSelector U = (DiversityCollection.UserControls.UserControlImportSelector)C;
                                        if (U.isSelected)
                                        {
                                            System.Collections.Generic.List<DiversityCollection.Import_Step> ImportSteps = U.ImportSteps();
                                            foreach (DiversityCollection.Import_Step IS in ImportSteps)
                                            {
                                                if (IS.TableName() == "CollectionEventMethod")
                                                {
                                                    U.setSelection(false);
                                                    IS.IsVisible(false);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (System.Exception ex) { }
                        goto case 1;
                    case 1:
                        goto case 2;
                    case 2:
                        break;
                }
            }
        }

        #region Settings
        
        private void LoadSettings(ref System.Xml.XmlTextReader R)
        {
            try
            {
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.Name == "ImportSettings" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                        return;
                    if (R.Name == "ImportSetting")
                        this.LoadSetting(ref R);

                }
            }
            catch (System.Exception ex) { }
        }

        private void LoadSetting(ref System.Xml.XmlTextReader R)
        {
            try
            {
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.NodeType == System.Xml.XmlNodeType.EndElement
                        && R.Name == "ImportSetting")
                        return;

                    if (R.NodeType == System.Xml.XmlNodeType.Element && R.Name == "Key")
                    {
                        while (R.NodeType != System.Xml.XmlNodeType.Text)
                        {
                            R.Read();
                        }
                    }
                    if (R.NodeType == System.Xml.XmlNodeType.Text && R.Value.Length > 0)
                    {
                        string Setting = R.Value;
                        if (DiversityCollection.Import.SettingObjects.ContainsKey(Setting))
                        {
                            System.Object C;
                            if (Setting == "Attachment")//this.SettingObjects[Setting] == null && )
                            {
                                R.Read();
                                if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                    R.Read();
                                R.Read();
                                if (R.NodeType == System.Xml.XmlNodeType.Whitespace) 
                                    R.Read();
                                R.Read();
                                if (R.NodeType == System.Xml.XmlNodeType.Whitespace) 
                                    R.Read();
                                if (R.NodeType == System.Xml.XmlNodeType.EndElement
                                    && R.Name == "ImportSetting")
                                    return;
                                string ColumnKey = R.Value;
                                if (ColumnKey.Length > 0)
                                {
                                    if (!DiversityCollection.Import.ImportColumns.ContainsKey(ColumnKey))
                                    {
                                        string AttachmentImportColumnTemplateKey = ColumnKey.Substring(0, ColumnKey.Length - 1) + "1";
                                        if (DiversityCollection.Import.ImportColumns.ContainsKey(AttachmentImportColumnTemplateKey))
                                        {
                                            DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.ImportColumns[AttachmentImportColumnTemplateKey].StepKey,
                                                DiversityCollection.Import.ImportColumns[AttachmentImportColumnTemplateKey].Table,
                                                DiversityCollection.Import.ImportColumns[AttachmentImportColumnTemplateKey].TableAlias,
                                                DiversityCollection.Import.ImportColumns[AttachmentImportColumnTemplateKey].Column,
                                                0,
                                                this._ImportInterface.UserControlImportAttachmentColumn(),
                                                DiversityCollection.Import.ImportColumns[AttachmentImportColumnTemplateKey].TypeOfSource,
                                                DiversityCollection.Import.ImportColumns[AttachmentImportColumnTemplateKey].TypeOfFixing,
                                                DiversityCollection.Import.ImportColumns[AttachmentImportColumnTemplateKey].TypeOfEntry);
                                        }
                                        else if (ColumnKey == "CollectionSpecimen.CollectionSpecimenID.0")
                                        {
                                            string[] ColumnKeyParts = ColumnKey.Split(new char[] { '.' });
                                            string Table = ColumnKeyParts[0];
                                            string Column = ColumnKeyParts[1];
                                            DiversityCollection.Import_Column.GetImportColumn("_03_00",
                                               Table,
                                               Table,
                                               Column,
                                               0,
                                               this._ImportInterface.UserControlImportAttachmentColumn(),
                                               DiversityCollection.Import_Column.SourceType.Database,
                                               DiversityCollection.Import_Column.FixingType.None,
                                               DiversityCollection.Import_Column.EntryType.Database);
                                        }
                                    }
                                    if (DiversityCollection.Import.ImportColumns.ContainsKey(ColumnKey))
                                    {
                                        DiversityCollection.Import.AttachmentKeyImportColumn = DiversityCollection.Import.ImportColumns[ColumnKey];
                                        DiversityCollection.Import.SettingObjects[Setting] = DiversityCollection.Import.AttachmentKeyImportColumn;
                                        C = DiversityCollection.Import.AttachmentKeyImportColumn;
                                        DiversityCollection.FormImportWizard f = (DiversityCollection.FormImportWizard)this._ImportInterface;
                                        f.setAttachmentKey(DiversityCollection.Import.AttachmentKeyImportColumn.Table, DiversityCollection.Import.AttachmentKeyImportColumn.Column);
                                        if (!DiversityCollection.Import._Settings.ContainsKey(DiversityCollection.Import.Setting.Attachment))
                                            DiversityCollection.Import._Settings.Add(DiversityCollection.Import.Setting.Attachment, ColumnKey);
                                        if (ImportColumns[ColumnKey].ImportColumnControl != null)
                                        {
                                            ImportColumns[ColumnKey].ImportColumnControl.setInterface();
                                        }
                                        this.setColumnInterface(DiversityCollection.Import.AttachmentKeyImportColumn);
                                    }
                                    else
                                        C = new object();
                                }
                                else 
                                    C = new object();
                            }
                            else if (Setting == "AttachmentColumnInSourceFile")
                            {
                                R.Read();
                                if (R.NodeType == System.Xml.XmlNodeType.Whitespace) R.Read();
                                R.Read();
                                if (R.NodeType == System.Xml.XmlNodeType.Whitespace) R.Read();
                                R.Read();
                                if (R.NodeType == System.Xml.XmlNodeType.Whitespace) R.Read();
                                string ColumnInSourceFile = R.Value;
                                if (DiversityCollection.Import.AttachmentKeyImportColumn != null)
                                {
                                    DiversityCollection.Import.AttachmentKeyImportColumn.ColumnInSourceFile = int.Parse(ColumnInSourceFile);
                                    DiversityCollection.FormImportWizard f = (DiversityCollection.FormImportWizard)this._ImportInterface;
                                    f.UserControlImportAttachmentColumn().setInterface();
                                    f.setDataGridColorRange();
                                }
                                C = new object();
                            }
                            else
                            {
                                C = DiversityCollection.Import.SettingObjects[Setting];
                                R.Read();
                                if (R.NodeType == System.Xml.XmlNodeType.Whitespace) R.Read();
                                while (R.NodeType != System.Xml.XmlNodeType.Text)
                                {
                                    R.Read();
                                }
                            }
                            if (R.NodeType == System.Xml.XmlNodeType.Text && R.HasValue && R.Value.Length > 0)
                            {
                                if (C.GetType() == typeof(System.Windows.Forms.ComboBox))
                                {
                                    bool ValueIsSet = false;
                                    System.Windows.Forms.ComboBox Combo = (System.Windows.Forms.ComboBox)C;
                                    for (int i = 0; i < Combo.Items.Count; i++)
                                    {
                                        if (Combo.Items[i].ToString() == R.Value)
                                        {
                                            Combo.SelectedIndex = i;
                                            ValueIsSet = true;
                                            break;
                                        }
                                    }
                                    if (Combo.DataSource != null && !ValueIsSet)
                                    {
                                        System.Data.DataTable dt = (System.Data.DataTable)Combo.DataSource;
                                        System.Windows.Forms.BindingSource B = new System.Windows.Forms.BindingSource(Combo.DataSource, Combo.ValueMember);
                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                            if (dt.Rows[i][Combo.ValueMember].ToString() == R.Value)
                                            {
                                                try
                                                {
                                                    B.Position = i;
                                                    //Combo.
                                                    Combo.SelectedIndex = i;
                                                    break;
                                                }
                                                catch (System.Exception ex)
                                                { }
                                                //break;
                                            }
                                            //Combo.SelectedIndex++;
                                        }
                                    }
                                }
                                else if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                                {
                                    System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)C;
                                    T.Text = R.Value;
                                    DiversityCollection.Import.AddSetting(Setting, R.Value.ToString());
                                }
                                else if (C.GetType() == typeof(System.Windows.Forms.RadioButton))
                                {
                                    System.Windows.Forms.RadioButton RB = (System.Windows.Forms.RadioButton)C;
                                    bool Checked = bool.Parse(R.Value);
                                    RB.Checked = Checked;
                                    DiversityCollection.Import.AddSetting(Setting, Checked.ToString());
                                }
                                else if (C.GetType() == typeof(System.Windows.Forms.NumericUpDown))
                                {
                                    System.Windows.Forms.NumericUpDown N = (System.Windows.Forms.NumericUpDown)C;
                                    decimal V = decimal.Parse(R.Value);
                                    if (N.Maximum >= V && N.Minimum <= V)
                                        N.Value = V;
                                    else
                                        V = N.Maximum;
                                    DiversityCollection.Import.AddSetting(Setting, V.ToString());
                                }
                                else if (C.GetType() == typeof(System.Windows.Forms.CheckBox))
                                {
                                    System.Windows.Forms.CheckBox CB = (System.Windows.Forms.CheckBox)C;
                                    bool Checked = bool.Parse(R.Value);
                                    CB.Checked = Checked;
                                    DiversityCollection.Import.AddSetting(Setting, Checked.ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex) { }
        }
        
        #endregion

        #region Columns
       
        private void LoadColumns(ref System.Xml.XmlTextReader R)
        {
            try
            {
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.Name == "ImportColumns" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                        return;
                    if (R.IsStartElement() && R.Name == "ImportColumn")
                    {
                        DiversityCollection.Import_Column IC = new Import_Column();
                        this.LoadColumn(ref R, ref IC);
                        try
                        {
                            if (IC.StepKey.StartsWith("_07"))
                            { } // Test
                            if (IC.StepKey.StartsWith("_07") && IC.Column.StartsWith("Rela"))
                            {} // Test
                            if (IC.Table.StartsWith("CollectionAgent"))
                            {} // Test
                            if (IC.StepKey != null && !DiversityCollection.Import.ImportSteps.ContainsKey(IC.StepKey))
                            {
                                this._ImportInterface.AddImportStep(IC.StepKey);
                            }
                            if (!Import.ImportColumns.ContainsKey(IC.Key))
                            {
                                DiversityCollection.Import_Column.GetImportColumn(IC.StepKey, IC.Table, IC.TableAlias, IC.Column, IC.Sequence(), IC.ImportColumnControl
                                    ,IC.TypeOfSource,IC.TypeOfFixing,IC.TypeOfEntry);
                            }
                            DiversityCollection.Import.ImportColumns[IC.Key].ColumnInSourceFile = IC.ColumnInSourceFile;
                            DiversityCollection.Import.ImportColumns[IC.Key].Format = IC.Format;
                            DiversityCollection.Import.ImportColumns[IC.Key].IsSelected = IC.IsSelected;
                            DiversityCollection.Import.ImportColumns[IC.Key].RegularExpressionPattern = IC.RegularExpressionPattern;
                            DiversityCollection.Import.ImportColumns[IC.Key].RegularExpressionReplacement = IC.RegularExpressionReplacement;
                            DiversityCollection.Import.ImportColumns[IC.Key].Separator = IC.Separator;
                            DiversityCollection.Import.ImportColumns[IC.Key].setSequence(IC.Sequence());
                            DiversityCollection.Import.ImportColumns[IC.Key].SplitPosition = IC.SplitPosition;
                            DiversityCollection.Import.ImportColumns[IC.Key].Splitters = IC.Splitters;
                            DiversityCollection.Import.ImportColumns[IC.Key].TranslationDictionary = IC.TranslationDictionary;
                            DiversityCollection.Import.ImportColumns[IC.Key].Value = IC.Value;
                            DiversityCollection.Import.ImportColumns[IC.Key].ValueIsFixed = IC.ValueIsFixed;
                            DiversityCollection.Import.ImportColumns[IC.Key].StepKey = IC.StepKey;
                            DiversityCollection.Import.ImportColumns[IC.Key].IsDecisionColumn = IC.IsDecisionColumn;

                            if (IC.IsSelected &&
                                DiversityCollection.Import.ImportColumns[IC.Key].IsInternalRelation &&
                                DiversityCollection.Import.ImportColumns[IC.Key].TypeOfSource == Import_Column.SourceType.Database &&
                                DiversityCollection.Import.ImportColumns[IC.Key].TypeOfEntry == Import_Column.EntryType.Database &&
                                DiversityCollection.Import.ImportColumns[IC.Key].IsSelected == true &&
                                DiversityCollection.Import.ImportColumns[IC.Key].ValueIsFixed == false)
                            {
                                DiversityCollection.Import.ImportColumns[IC.Key].ValueIsFixed = true;
                                //DiversityCollection.UserControls.UserControlImport_Column U = (DiversityCollection.UserControls.UserControlImport_Column)DiversityCollection.Import.ImportColumns[IC.Key].ImportColumnControl; ;
                            }

                            if (ImportColumns[IC.Key].ImportColumnControl != null)
                            {
                                if (IC.Sequence() == 1)
                                {
                                    ImportColumns[IC.Key].ImportColumnControl.setInterface();
                                    //if (IC.Value != null && IC.Value.Length > 0)
                                    //    ImportColumns[IC.Key].ImportColumnControl.setValue(IC.Value);
                                }
                                else
                                {
                                    //string KeyBasicColumn = IC.Key.Substring(0, IC.Key.Length - IC.Sequence.ToString().Length) + "1";
                                    //if (DiversityCollection.Import.ImportColumns.ContainsKey(KeyBasicColumn))
                                    //{
                                    //    DiversityCollection.Import.ImportColumns[KeyBasicColumn].ImportColumnControl.AddMultiColumn(IC);
                                    //}
                                }
                            }
                            else
                            {
                                if (IC.Sequence() > 1)
                                {
                                    string KeyBasicColumn = IC.Key.Substring(0, IC.Key.Length - IC.Sequence().ToString().Length) + "1";
                                    if (DiversityCollection.Import.ImportColumns.ContainsKey(KeyBasicColumn)
                                        && DiversityCollection.Import.ImportColumns[KeyBasicColumn].ImportColumnControl != null)
                                    {
                                        DiversityCollection.Import.ImportColumns[KeyBasicColumn].ImportColumnControl.AddMultiColumn(IC);
                                    }
                                }
                                else if (IC.Sequence() == 1 && ImportColumns[IC.Key].ImportColumnControl == null)
                                {
                                    //IC = DiversityCollection.Import_Column.GetImportColumn(IC.StepKey, IC.Table, IC.TableAlias, IC.Column, IC.Sequence(), IC.ImportColumnControl
                                    //    , IC.TypeOfSource, IC.TypeOfFixing, IC.TypeOfEntry);
                                }
                            }
                        }
                        catch (System.Exception ex) { }
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        private void LoadColumn(ref System.Xml.XmlTextReader R, ref DiversityCollection.Import_Column IC)
        {
            string _CurrentNode = "";

            try
            {
                while (R.Read())
                {
                    // Checking if the end of the step information is found
                    if (R.NodeType == System.Xml.XmlNodeType.EndElement
                        && (R.Name == "ImportColumn" || R.Name == "ImportColumns"))
                    {
                        return;
                    }

                    // handling the data
                    switch (R.NodeType)
                    {
                        case System.Xml.XmlNodeType.Whitespace:
                            if (_CurrentNode == "Splitter")
                            {
                                this.LoadColumnSplitters(ref R, ref IC);
                                _CurrentNode = "";
                            }
                            continue;
                        case System.Xml.XmlNodeType.Text:
                            switch (_CurrentNode)
                            {
                                case "ImportColumnKey":
                                    IC.TableAlias = R.Value.Substring(0, R.Value.IndexOf('.'));
                                    IC.Table = IC.TableAlias;
                                    IC.Column = R.Value.Substring(R.Value.IndexOf('.') + 1);
                                    if (IC.Column.IndexOf('.') > -1)
                                        IC.Column = IC.Column.Substring(0, IC.Column.IndexOf('.'));
                                    break;
                                case "IsSelected":
                                    try
                                    {
                                        if (R.Value.Length > 0)
                                        {
                                            bool IS;
                                            if (bool.TryParse(R.Value, out IS))
                                                IC.IsSelected = IS;
                                            if (IS) { }// nur zur Kontrolle fuer Haltepunkt
                                        }
                                    }
                                    catch (System.Exception ex) { }
                                    break;
                                case "IsDecisionColumn":
                                    try
                                    {
                                        if (R.Value.Length > 0)
                                        {
                                            bool IS;
                                            if (bool.TryParse(R.Value, out IS))
                                                IC.IsDecisionColumn = IS;
                                            if (IS) { }// nur zur Kontrolle fuer Haltepunkt
                                        }
                                    }
                                    catch (System.Exception ex) { }
                                    break;
                                case "Column":
                                    IC.Column = R.Value;
                                    break;
                                case "ImportStep":
                                    try
                                    {
                                        if (R.Value.Length > 0)
                                        {
                                            IC.StepKey = R.Value;
                                        }
                                    }
                                    catch (System.Exception ex) { }
                                    break;
                                case "SuperiorImportStep":
                                    //IC. = R.Value;
                                    break;
                                case "TypeOfSource":
                                    try
                                    {
                                        if (R.Value.Length > 0)
                                        {
                                            if (R.Value == DiversityCollection.Import_Column.SourceType.File.ToString())
                                                IC.TypeOfSource = Import_Column.SourceType.File;
                                            else if (R.Value == DiversityCollection.Import_Column.SourceType.Interface.ToString())
                                                IC.TypeOfSource = Import_Column.SourceType.Interface;
                                            else if (R.Value == DiversityCollection.Import_Column.SourceType.Database.ToString())
                                                IC.TypeOfSource = Import_Column.SourceType.Database;
                                            else
                                                IC.TypeOfSource = Import_Column.SourceType.Any;
                                        }
                                    }
                                    catch (System.Exception ex) { }
                                    break;
                                case "TableAlias":
                                    IC.TableAlias = R.Value;
                                    break;
                                case "ColumnInSourceFile":
                                    try
                                    {
                                        if (R.Value.Length > 0)
                                        {
                                            int CISF;
                                            if (int.TryParse(R.Value, out CISF))
                                                IC.ColumnInSourceFile = CISF;
                                        }
                                    }
                                    catch (System.Exception ex) { }
                                    break;
                                case "SplitPosition":
                                    try
                                    {
                                        if (R.Value.Length > 0)
                                        {
                                            int SP;
                                            if (int.TryParse(R.Value, out SP))
                                                IC.SplitPosition = SP;
                                        }
                                    }
                                    catch (System.Exception ex) { }
                                    break;
                                case "Splitters":
                                    this.LoadColumnSplitters(ref R, ref IC);
                                    break;
                                case "Splitter":
                                    this.LoadColumnSplitters(ref R, ref IC);
                                    break;
                                case "Separator":
                                    try
                                    {
                                        if (R.Value.Length > 0)
                                        {
                                            IC.Separator = R.Value;
                                        }
                                    }
                                    catch (System.Exception ex) { }
                                    break;
                                case "Sequence":
                                    try
                                    {
                                        if (R.Value.Length > 0)
                                        {
                                            int Seq;
                                            if (int.TryParse(R.Value, out Seq))
                                                IC.setSequence(Seq);
                                        }
                                    }
                                    catch (System.Exception ex) { }
                                    break;
                                case "Value":
                                    try
                                    {
                                        if (R.Value.Length > 0)
                                        {
                                            IC.Value = R.Value;
                                            IC.ValueIsFixed = true;
                                        }
                                    }
                                    catch (System.Exception ex) { }
                                    break;

                                case "Table":
                                    IC.Table = R.Value;
                                    continue;
                                case "TranslationDictionary":
                                    this.LoadColumnTranslationDictionary(ref R, ref IC);
                                    break;
                                case "RegularExpressionPattern":
                                    try
                                    {
                                        if (R.Value.Length > 0)
                                        {
                                            IC.RegularExpressionPattern = R.Value;
                                        }
                                    }
                                    catch (System.Exception ex) { }
                                    break;
                                case "RegularExpressionReplacement":
                                    try
                                    {
                                        if (R.Value.Length > 0)
                                        {
                                            IC.RegularExpressionReplacement = R.Value;
                                        }
                                    }
                                    catch (System.Exception ex) { }
                                    break;

                                
                                default:
                                    break;
                            }
                            _CurrentNode = "";
                            break;
                        case System.Xml.XmlNodeType.Element:
                            _CurrentNode = R.Name;
                            break;
                        case System.Xml.XmlNodeType.EndElement:
                            break;
                        default:
                            break;
                    }

                }
            }
            catch (System.Exception ex) { }



            return;
        }

        private void LoadColumnTranslationDictionary(ref System.Xml.XmlTextReader R, ref DiversityCollection.Import_Column IC)
        {
            try
            {
                //string Column = "";
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.Name == "TranslationDictionary" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                        return;
                    if (R.IsStartElement() && R.Name == "Translation")
                    {
                        this.LoadColumnTranslationDictionaryTranslation(ref R, ref IC);
                    }
                }
            }
            catch (System.Exception ex) { }
            return;
        }

        private void LoadColumnTranslationDictionaryTranslation(ref System.Xml.XmlTextReader R, ref DiversityCollection.Import_Column IC)
        {
            try
            {
                //string Column = "";
                string Property = "";
                string Source = "";
                string Translation = "";
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.Name == "Translation" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                        return;
                    if (R.IsStartElement() && R.Name != "Translation")
                    {
                        if (R.Name.Length > 0)
                            Property = R.Name;
                        R.Read();
                        if (R.NodeType == System.Xml.XmlNodeType.Whitespace) 
                            R.Read();
                        if (R.Name == "Translation" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                            return;
                        if (R.NodeType != System.Xml.XmlNodeType.Whitespace)
                        {
                            switch (Property)
                            {
                                case "Key":
                                    Source = R.Value;
                                    break;
                                case "Value":
                                    Translation = R.Value;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    if (Source.Length > 0 && Translation.Length > 0 && !IC.TranslationDictionary.ContainsKey(Source))
                    {
                        IC.TranslationDictionary.Add(Source, Translation);
                        Source = "";
                        Translation = "";
                        return;
                    }
                }
            }
            catch (System.Exception ex) { }
            return;
        }

        private void LoadColumnSplitters(ref System.Xml.XmlTextReader R, ref DiversityCollection.Import_Column IC)
        {
            try
            {
                if ((R.NodeType == System.Xml.XmlNodeType.Text 
                    || R.NodeType == System.Xml.XmlNodeType.Whitespace)
                    && R.Name == ""
                    && R.Value.Length > 0
                    && !IC.Splitters.Contains(R.Value))
                {
                    IC.Splitters.Add(R.Value);
                }
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                    {
                        if ((R.NodeType == System.Xml.XmlNodeType.Text
                            || R.NodeType == System.Xml.XmlNodeType.Whitespace)
                            && R.Name == ""
                            && R.Value.Length > 0
                            && !IC.Splitters.Contains(R.Value))
                        {
                            IC.Splitters.Add(R.Value);
                        }
                        continue;
                    }
                    if (R.Name == "Splitters" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                        return;
                    if (!R.IsStartElement()
                        && R.Name != "Splitter"
                        && !IC.Splitters.Contains(R.Value)
                        && R.NodeType == System.Xml.XmlNodeType.Text)
                    {
                        IC.Splitters.Add(R.Value);
                    }
                    else if (R.Name == "Splitter" && R.NodeType == System.Xml.XmlNodeType.Element && !R.IsEmptyElement)
                    {
                        R.MoveToContent();
                        string x = R.ReadElementContentAsString();
                        if (!IC.Splitters.Contains(x))
                            IC.Splitters.Add(x);
                        R.MoveToElement();
                        if (R.Name == "Splitters" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                            return;
                    }
                }
            }
            catch (System.Exception ex) { }
            return;
        }
        
        #endregion   
     
        #region TableAttachmentSettings

        private void LoadTableAttachmentSettings(ref System.Xml.XmlTextReader R)
        {
            try
            {
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.Name == "ImportSteps")
                        return;
                    if (R.Name == "ImportTableAttachmentSettings" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                        return;
                    if (R.Name == "ImportTableAttachmentSetting" && R.NodeType == System.Xml.XmlNodeType.Element)
                        this.LoadTableAttachmentSetting(ref R);
                }
            }
            catch (System.Exception ex) { }
        }

        private void LoadTableAttachmentSetting(ref System.Xml.XmlTextReader R)
        {
            try
            {
                string TableAlias = "";
                string TableName = "";
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.NodeType == System.Xml.XmlNodeType.EndElement
                        && (R.Name == "ImportTableAttachmentSetting" || R.Name == "ErrorTreatment"))
                        return;

                    if (R.NodeType == System.Xml.XmlNodeType.Element && R.Name == "TableAlias")
                    {
                        R.Read();
                        if (R.NodeType == System.Xml.XmlNodeType.Whitespace) R.Read();
                        TableAlias = R.Value;
                    }
                    if (R.NodeType == System.Xml.XmlNodeType.Element && R.Name == "TableName")
                    {
                        R.Read();
                        if (R.NodeType == System.Xml.XmlNodeType.Whitespace) R.Read();
                        TableName = R.Value;
                    }
                    if (TableAlias.Length > 0 && TableName.Length > 0 && R.NodeType == System.Xml.XmlNodeType.Element && R.Name == "DataTreatment")
                    {
                        R.Read();
                        if (R.NodeType == System.Xml.XmlNodeType.Whitespace) R.Read();
                        string DataTreatment = R.Value;
                        DiversityCollection.Import_Table T = new Import_Table(TableAlias, TableName, this._Grid);
                        if (DataTreatment == DiversityCollection.Import_Table.DataTreatment.Insert.ToString())
                            T.TreatmentOfData = DiversityCollection.Import_Table.DataTreatment.Insert;
                        else if (DataTreatment == DiversityCollection.Import_Table.DataTreatment.Merge.ToString())
                            T.TreatmentOfData = DiversityCollection.Import_Table.DataTreatment.Merge;
                        else if (DataTreatment == DiversityCollection.Import_Table.DataTreatment.Update.ToString())
                            T.TreatmentOfData = DiversityCollection.Import_Table.DataTreatment.Update;
                        if (DiversityCollection.Import.ImportTablesDataTreatment == null)
                            DiversityCollection.Import.ImportTablesDataTreatment = new Dictionary<string, Import_Table>();
                        if (!DiversityCollection.Import.ImportTablesDataTreatment.ContainsKey(TableAlias))
                            DiversityCollection.Import.ImportTablesDataTreatment.Add(T.TableAlias, T);
                        else
                            DiversityCollection.Import.ImportTablesDataTreatment[TableAlias].TreatmentOfData = T.TreatmentOfData;
                    }
                    if (TableAlias.Length > 0 && TableName.Length > 0 && R.NodeType == System.Xml.XmlNodeType.Element && R.Name == "ErrorTreatment")
                    {
                        R.Read();
                        if (R.NodeType == System.Xml.XmlNodeType.Whitespace) R.Read();
                        string ErrorTreatment = R.Value;// DiversityCollection.Import_Table.ErrorTreatment.AutoNone.ToString();
                        if (ErrorTreatment == DiversityCollection.Import_Table.ErrorTreatment.AutoFirst.ToString())
                            DiversityCollection.Import.ImportTablesDataTreatment[TableAlias].TreatmentOfErrors = Import_Table.ErrorTreatment.AutoFirst;
                        else if (ErrorTreatment == DiversityCollection.Import_Table.ErrorTreatment.AutoLast.ToString())
                            DiversityCollection.Import.ImportTablesDataTreatment[TableAlias].TreatmentOfErrors = Import_Table.ErrorTreatment.AutoLast;
                        else if (ErrorTreatment == DiversityCollection.Import_Table.ErrorTreatment.Manual.ToString())
                            DiversityCollection.Import.ImportTablesDataTreatment[TableAlias].TreatmentOfErrors = Import_Table.ErrorTreatment.Manual;
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        #endregion

        #region Steps

        private void LoadSteps(ref System.Xml.XmlTextReader R)
        {
            try
            {
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.Name == "ImportSteps" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                        return;
                    if (R.Name == "ImportStep" && R.NodeType == System.Xml.XmlNodeType.Element)
                        this.LoadStep(ref R);
                }
            }
            catch (System.Exception ex) { }
        }

        private void LoadStep(ref System.Xml.XmlTextReader R)
        {
            bool? _StepIsVisible = null;
            string _CurrentNode = "";
            string _StepKey = "";
            string _Title = "";
            try
            {
                while (R.Read())
                {
                    // Checking if the end of the step information is found
                    if (R.NodeType == System.Xml.XmlNodeType.EndElement
                        && (R.Name == "ImportStep" || R.Name == "IsVisible"))
                        return;

                    // handling the data
                    switch (R.NodeType)
                    {
                        case System.Xml.XmlNodeType.Whitespace:
                            continue;
                        case System.Xml.XmlNodeType.Text:
                            switch (_CurrentNode)
                            {
                                case "IsVisible":
                                    bool V;
                                    if (bool.TryParse(R.Value, out V))
                                        _StepIsVisible = V;
                                    break;
                                case "Key":
                                    _StepKey = R.Value;
                                    break;
                                case "Title":
                                    _Title = R.Value;
                                    break;
                            }
                            _CurrentNode = "";
                            break;
                        case System.Xml.XmlNodeType.Element:
                            _CurrentNode = R.Name;
                            break;
                        default:
                            break;
                    }

                    // writing the infos for the step and return
                    if (_StepKey.Length > 0 && _StepIsVisible != null)
                    {
                        if (DiversityCollection.Import.ImportSteps.ContainsKey(_StepKey))
                            DiversityCollection.Import.ImportSteps[_StepKey].setImportStepVisibility((bool)_StepIsVisible);
                        return;
                    }
                }
            }
            catch (System.Exception ex) { }



        }
        
        #endregion

        #endregion

        #endregion

        #region Columns

        public System.Collections.Generic.List<Import_Column> getImportColumns(int i)
        {
            System.Collections.Generic.List<Import_Column> L = new List<Import_Column>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> IC in DiversityCollection.Import.ImportColumns)
            {
                if (IC.Value.ColumnInSourceFile == i)
                    L.Add(IC.Value);
            }
            return L;
        }

        public void setImportColumnInSourceFile(int? i, DiversityCollection.Import_Column IC)
        {
            IC.ColumnInSourceFile = i;

            if (DiversityCollection.Import.ImportColumns.ContainsKey(IC.Key))
                DiversityCollection.Import.ImportColumns[IC.Key].ColumnInSourceFile = i;
            else
                DiversityCollection.Import.ImportColumns.Add(IC.Key, IC);
        }

        public bool ImportContainsColumn(string Table, string Column)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, Import_Column> C in DiversityCollection.Import.ImportColumns)
            {
                if (C.Value.Table == Table && C.Value.Column == Column)
                    return true;
            }
            return false;
        }

        public bool ImportContainsColumn(Import_Column Column)
        {
            if (DiversityCollection.Import.ImportColumns.ContainsKey(Column.TableAlias + "." + Column.Column))
                return true;
            else return false;
            //foreach (System.Collections.Generic.KeyValuePair<int, System.Collections.Generic.List<Import_Column>> C in this.ImportColumns)
            //{
            //    foreach (Import_Column IC in C.Value)
            //    {
            //        if (IC.Table == Column.Table &&
            //            IC.Column == Column.Column)
            //            if (Column.PresetValue != null &&
            //                Column.PresetValueColumn != null)
            //            {
            //                if (IC.PresetValue == Column.PresetValue &&
            //                    IC.PresetValueColumn == Column.PresetValueColumn)
            //                    return true;
            //            }
            //            else
            //                return true;
            //    }
            //}
            //return false;
        }

        public void setPresetColumn(Import_Column IC)
        {
            if (this._PresetColumns == null)
                this._PresetColumns = new List<Import_Column>();
            DiversityCollection.Import_Column ICfound = DiversityCollection.Import_Column.GetImportColumn(IC.StepKey, IC.Table, IC.Column, IC.ImportColumnControl);
            foreach (Import_Column C in this._PresetColumns)
            {
                if (IC.Table == C.Table &&
                    IC.TableAlias == C.TableAlias &&
                    IC.Column == C.Column)
                {
                    ICfound = C;
                    break;
                }
            }
            if (ICfound.Table == null)
            {
                ICfound = IC;
                this._PresetColumns.Add(ICfound);
            }
            else
            {
                ICfound.AlternativeColumn = IC.AlternativeColumn;
                //ICfound.AnalysisMethod = IC.AnalysisMethod;
                ICfound.PresetValue = IC.PresetValue;
                ICfound.PresetValueColumn = IC.PresetValueColumn;
                ICfound.Value = IC.Value;
                ICfound.MultiColumn = IC.MultiColumn;
                ICfound.Separator = IC.Separator;
                ICfound.setSequence(IC.Sequence());
            }
        }

        #region Removing a column

        public void RemoveColumn(Import_Column IC, int Position)
        {
            if (DiversityCollection.Import.ImportColumns.ContainsKey(IC.Key) &&
                DiversityCollection.Import.ImportColumns[IC.Key].ColumnInSourceFile == Position)
            {
                DiversityCollection.Import.ImportColumns[IC.Key].IsSelected = false;
                //DiversityCollection.Import.ImportColumns.Remove(IC.Key);
            }
        }

        public void RemoveColumn(string Key)
        {
            if (DiversityCollection.Import._ImportColumns.ContainsKey(Key))
            {
                if (DiversityCollection.Import._ImportColumns[Key].ColumnInSourceFile != null)
                {
                    int ColumnInSourceFile = (int)DiversityCollection.Import._ImportColumns[Key].ColumnInSourceFile;
                    //DiversityCollection.Import.ImportColumns[Key].IsSelected = false;
                    DiversityCollection.Import.ImportColumns[Key].ColumnInSourceFile = null;
                    DiversityCollection.Import.ImportColumns[Key].ImportColumnControl.setInterface();
                    this._ImportInterface.setColumnDisplays(ColumnInSourceFile);
                    this._ImportInterface.Grid().Columns[ColumnInSourceFile].HeaderText = this.DataGridColumnHeaeder(ColumnInSourceFile);
                    //DiversityCollection.Import_Column IC = DiversityCollection.Import._ImportColumns[Key];
                    //if (IC.ColumnInSourceFile != null && IC.ColumnInSourceFile > -1)
                    //{
                    //    this._ImportInterface.setColumnDisplays((int)IC.ColumnInSourceFile);
                    //    this._ImportInterface.Grid().Columns[(int)IC.ColumnInSourceFile].HeaderText = this.DataGridColumnHeaeder((int)IC.ColumnInSourceFile);
                    //    //this._ImportInterface.Grid().Columns[(int)IC.ColumnInSourceFile].HeaderCell.Style.BackColor = System.Drawing.Color.Yellow;
                    //    //DiversityCollection.Import._ImportColumns.Remove(Key);
                    //    DiversityCollection.Import.ImportColumns[IC.Key].IsSelected = false;
                    //}
                }
            }
        }

        public void RemoveColumn(string Table, string Alias, string Column)
        {
            //int i = 0;
            string Key = Alias + "." + Column;
            if (Alias.Length == 0)
                Key = Table + Key;
            if (DiversityCollection.Import.ImportColumns.ContainsKey(Alias + "." + Column))
            {
                DiversityCollection.Import.ImportColumns[Key].IsSelected = false;
                //DiversityCollection.Import.ImportColumns.Remove(Alias + "." + Column);
            }
        }

        #endregion

        public string DataGridColumnHeaeder(int Position)
        {
            string Header = "";
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> KV in DiversityCollection.Import.ImportColumns)
            {
                if (KV.Value.ColumnInSourceFile == Position)
                {
                    Header += KV.Value.Table + " " + KV.Value.Column + "\r\n";
                }
            }
            //if (this.ImportColumns.ContainsKey(Position))
            //{
            //    foreach (Import_Column C in this.ImportColumns[Position])
            //    {
            //        Header += C.Table + " " + C.Column + "\r\n";
            //    }
            //}
            return Header;
        }

        public void setDataGridColumnHeader(int Position)
        {
            this._Grid.Columns[Position].HeaderText = this.DataGridColumnHeaeder(Position);
        }

        public void setColumnInterface(DiversityCollection.Import_Column IC)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> KV in DiversityCollection.Import.ImportColumns)
            {
                if (KV.Value.StepKey == IC.StepKey && IC != KV.Value && KV.Value.ImportColumnControl != null)
                    KV.Value.ImportColumnControl.setInterface();
            }
            if (IC.ImportColumnControl != null)
                IC.ImportColumnControl.setInterface();
        }

        #endregion

        #region Steps


        /// <summary>
        /// Adds an import step depending on a key
        /// </summary>
        /// <param name="SortKey">The Key by which the import steps are sorted</param>
        /// <param name="ImportStep">The import step</param>
        public void ImportStepAdd(DiversityCollection.Import_Step ImportStep)
        {
            if (!DiversityCollection.Import.ImportSteps.ContainsKey(ImportStep.StepKey()))
                DiversityCollection.Import.ImportSteps.Add(ImportStep.StepKey(), ImportStep);
        }

        public void ImportStepsShow()
        {
            // Getting the current position
            //this.PanelImportSteps.SuspendLayout();
            this.PanelImportSteps.Controls.Clear();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DiversityCollection.Import.ImportSteps)
            {
                try
                {
                    this.PanelImportSteps.Controls.Add(KV.Value.UserControlImportStep);
                    KV.Value.UserControlImportStep.Dock = System.Windows.Forms.DockStyle.Top;
                    KV.Value.UserControlImportStep.BringToFront();
                    KV.Value.UserControlImportStep.Visible = KV.Value.IsVisible();
                }
                catch (System.Exception ex) { }
            }
        }

        public string CurrentPosition
        {
            get
            {
                string KeyCurrent = "";
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DiversityCollection.Import.ImportSteps)
                {
                    if (KV.Value.UserControlImportStep.IsCurrent)
                    {
                        KeyCurrent = KV.Key;
                        break;
                    }
                }
                return KeyCurrent;
            }
            set
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DiversityCollection.Import.ImportSteps)
                {
                    KV.Value.UserControlImportStep.IsCurrent = false;
                    //KV.Value.ShowControls();//.setTabPageVisibility(false);
                }
                if (value.Length > 0 && DiversityCollection.Import.ImportSteps.ContainsKey(value))
                {
                    DiversityCollection.Import.ImportSteps[value].UserControlImportStep.IsCurrent = true;
                    DiversityCollection.Import.ImportSteps[value].ShowControls();//.setTabPageVisibility(true);
                }
            }
        }

        public System.Windows.Forms.Panel PanelImportSteps
        {
            get { return _PanelImportSteps; }
            set { _PanelImportSteps = value; }
        }

        public System.Collections.Generic.SortedList<int, string> ImportStepKeys
        {
            get
            {
                System.Collections.Generic.SortedList<int, string> ListOfSteps = new SortedList<int, string>();
                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DiversityCollection.Import.ImportSteps)
                {
                    ListOfSteps.Add(i, KV.Key);
                    i++;
                }
                return ListOfSteps;
            }
        }

        #endregion

        #region Interface

        /// <summary>
        /// display the import column definitions that where set for a column in the datagrid
        /// </summary>
        /// <param name="Position">The position in the datagrid</param>
        public void setColumnDisplays(int Position)
        {
            this._ImportInterface.setColumnDisplays(Position);
        }

        #endregion

        #region Import

        private int _NumberOfImportedLines = 0;
        //private System.Collections.Generic.List<int> _NumberOfFailedLines = new List<int>();

        public void ImportReset()
        {
            try
            {
                this._TablesForImport = new Dictionary<string, string>();
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DiversityCollection.Import._ImportSteps)
                    KV.Value.Reset();
                DiversityCollection.Import._ImportSteps = new SortedList<string, Import_Step>();
                DiversityCollection.Import.AttachmentKeyImportColumn = null;
                DiversityCollection.Import._AttachmentKeyImportColumn = null;
                DiversityCollection.Import._CurrentImportColumn = null;
                DiversityCollection.Import._ImportColumnControls = null;
                DiversityCollection.Import._ImportColumns = null;
                DiversityCollection.Import._ImportSelectors = null;
                DiversityCollection.Import._ImportSteps = null;
                DiversityCollection.Import._ImportTables = null;
                DiversityCollection.Import._SettingObjects = null;
                DiversityCollection.Import._Settings = null;
                DiversityCollection.Import._TabPagesControl = null;
                DiversityCollection.Import.ImportTablesDataTreatment = null;
            }
            catch (System.Exception ex) { }
        }

        /// <summary>
        /// Reset the import tables and recreate the structure from the settings
        /// </summary>
        /// <returns>the number of the imported lines</returns>
        public string ImportData()
        {
            if (!this.AllStepsOK())
                return "Please solve the pending errors before starting the import";

            if (!this.ImportPrepared())
                return "Import failed at preparing objects";

            string Message = "";
            DiversityCollection.Import._NumberOfFailedLines.Clear();
            this._NumberOfImportedLines = 0;

            string MessageForLines = "";
            try
            {
                DiversityCollection.FormImportWizard Wiz = (DiversityCollection.FormImportWizard)this.ImportInterface;
                for (int i = this._ImportInterface.FirstLineForImport() - 1; i < this._ImportInterface.LastLineForImport(); i++)
                {
                    this.CurrentImportRow = i;
                    Wiz.setProgressBarValue(i);

                    this.ClearValues();
                    DiversityCollection.Import.DecisionStepsWithNoValues.Clear();

                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Table> KV in DiversityCollection.Import._ImportTables)
                    {
                        KV.Value.FillTableWithDataFromFile(i);
                    }
                    bool ImportOfLineSuccessful = true;
                    System.Data.SqlClient.SqlConnection ImportConnection = new System.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    ImportConnection.Open();
                    System.Data.SqlClient.SqlTransaction ImportTransaction = ImportConnection.BeginTransaction("ImportLine");
                    if (DiversityCollection.Import.AttachmentKeyImportColumn != null && DiversityCollection.Import.AttachmentKeyImportColumn.Column != null)
                    {

                        if (!this.FillTablesWithDataFromDatabaseViaAttachmentKey(i, ImportConnection, ImportTransaction))
                        {
                            System.Windows.Forms.MessageBox.Show("Attachement failed");
                            return "";
                        }
                        else if (DiversityCollection.Import._ImportTables.ContainsKey(DiversityCollection.Import.AttachmentKeyImportColumn.TableAlias))
                            DiversityCollection.Import._ImportTables[DiversityCollection.Import.AttachmentKeyImportColumn.Table].TreatmentOfData = Import_Table.DataTreatment.Update;
                    }
                    System.Collections.Generic.List<DiversityCollection.Import_Table> TablesForImport = new List<Import_Table>();
                    foreach (string T in this.PotentialImportTableAliases)
                    {
                        if (DiversityCollection.Import._ImportTables.ContainsKey(T))
                        {
                            DiversityCollection.Import._ImportTables[T].FillColumnsFromSuperiorTables();
                            DiversityCollection.Import._ImportTables[T].ImportTableData(i, ImportConnection, ImportTransaction);
                            if (DiversityCollection.Import._ImportTables[T].getImportErrors().ContainsKey(i))
                            {
                                ImportOfLineSuccessful = false;
                                break;
                            }
                        }
                    }
                    if (ImportOfLineSuccessful)
                    {
                        ImportTransaction.Commit();
                        this._NumberOfImportedLines++;
                        this.ImportInterface.setDataGridLineColor(i, FormImportWizard.ColorForSuccess);
                    }
                    else
                    {
                        try
                        {
                            ImportTransaction.Rollback("ImportLine");
                        }
                        catch (System.Exception ex)
                        {
                        }
                        DiversityCollection.Import._NumberOfFailedLines.Add(i);
                        this.ImportInterface.setDataGridLineColor(i, FormImportWizard.ColorForError);
                        //DiversityCollection.FormImportWizard IW = (DiversityCollection.FormImportWizard)this.ImportInterface;
                    }
                    ImportConnection.Close();
                    ImportConnection.Dispose();
                }
                if (this._NumberOfImportedLines > 0)
                    Message = "Number of imported lines: " + this._NumberOfImportedLines.ToString() + "\r\n";
                if (DiversityCollection.Import._NumberOfFailedLines.Count > 0)
                {
                    Message += "Number of failed lines: " + DiversityCollection.Import._NumberOfFailedLines.Count.ToString() + "\r\n" + MessageForLines;
                    if (Wiz.SaveErrorLinesInFile)
                    {
                        System.IO.FileInfo Source = new System.IO.FileInfo(Wiz.SourceFile);
                        string ErrorFile = DiversityCollection.Import.CopyFileAccordingToDataGridView(Source, "_Error", Wiz.Encoding, this._ImportInterface.FirstLineForImport(), this._ImportInterface.LastLineForImport(), Wiz.FirstLineContainsColumnDefinition);
                        Wiz.SetImportErrorFile(ErrorFile);
                    }
                }
                else Wiz.SetImportErrorFile("");
            }
            catch (System.Exception ex) { }
            return Message;
        }

        private System.Collections.Generic.List<string> _PotentialImportTableAliases;
        private System.Collections.Generic.List<string> PotentialImportTableAliases
        {
            get
            {
                if (this._PotentialImportTableAliases == null)
                {
                    this._PotentialImportTableAliases = new List<string>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> IS in DiversityCollection.Import.ImportSteps)
                    {
                        if (!this._PotentialImportTableAliases.Contains(IS.Value.TableAlias()) && IS.Value.TableAlias().Length > 0)
                            this._PotentialImportTableAliases.Add(IS.Value.TableAlias());
                    }
                }
                return this._PotentialImportTableAliases;
            }
        }

        public bool ImportPrepared()
        {
            try
            {
                // clear the table construction.
                this.ResetImportTables();

                // rebuild the tables
                this.initImportTablesAccordingToSettings();
                this.completeTablePKs();
                this.getExistingColumnsFromSuperiorTables();
                this.initColumnDictionaries();
                this.createMissingSuperiorTables();
                this.createMissingConnectingTables();
            }
            catch (System.Exception ex)
            { return false; }
            return true;
        }

        private bool FillTablesWithDataFromDatabaseViaAttachmentKey(int iLine, System.Data.SqlClient.SqlConnection ImportConnection, System.Data.SqlClient.SqlTransaction ImportTransaction)
        {
            bool OK = true;
            try
            {
                if (!DiversityCollection.Import._ImportTables.ContainsKey(DiversityCollection.Import.AttachmentKeyImportColumn.TableAlias))
                {
                    DiversityCollection.Import_Table T = new Import_Table(DiversityCollection.Import.AttachmentKeyImportColumn.TableAlias, DiversityCollection.Import.AttachmentKeyImportColumn.Table, DiversityCollection.Import.DataGridView);
                    DiversityCollection.Import._ImportTables.Add(DiversityCollection.Import.AttachmentKeyImportColumn.TableAlias, T);
                }
                DiversityCollection.Import._ImportTables[DiversityCollection.Import.AttachmentKeyImportColumn.TableAlias].CompareAndInsertValuesViaAttachmentKey(iLine, ImportConnection, ImportTransaction);
                DiversityCollection.Import._ImportTables[DiversityCollection.Import.AttachmentKeyImportColumn.TableAlias].FillSuperiorKeyColumns(iLine);
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        private void createMissingConnectingTables()
        {
            try
            {
                System.Collections.Generic.List<string> UnitTables = new List<string>();
                System.Collections.Generic.List<string> PartTables = new List<string>();
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> IS in DiversityCollection.Import.ImportSteps)
                {
                    if (IS.Value.IsVisible())
                    {
                        if (IS.Value.TableName() == "IdentificationUnit" && !UnitTables.Contains(IS.Value.TableAlias()) && !IS.Value.IsGroupHaeder)
                            UnitTables.Add(IS.Value.TableAlias());
                        if (IS.Value.TableName() == "CollectionSpecimenPart" && !PartTables.Contains(IS.Value.TableAlias()) && !IS.Value.IsGroupHaeder)
                            PartTables.Add(IS.Value.TableAlias());
                    }
                }
                if (UnitTables.Count > 0 && PartTables.Count > 0)
                {
                    int i = 1;
                    foreach (string UnitTableAlias in UnitTables)
                    {
                        foreach (string PartTableAlias in PartTables)
                        {
                            this.createMissingIdentificationUnitInPart(PartTableAlias, UnitTableAlias, i);
                            i++;
                        }
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        private void createMissingIdentificationUnitInPart(string PartTableAlias, string UnitTableAlias, int DisplayOrder)
        {
            string StepKey = DiversityCollection.Import.getImportStepKey(ImportStep.UnitInPart, DisplayOrder);
            DiversityCollection.Import_Step IS = DiversityCollection.Import_Step.GetImportStep(
                "IdentificationUnitInPart " + DisplayOrder.ToString(),
                "IdentificationUnitInPart " + DisplayOrder.ToString(),
                StepKey,
                "IdentificationUnitInPart",
                DisplayOrder,
                null,
                2,
                null,null, null
                );
            IS.setImportStepVisibility(true);

            DiversityCollection.Import_Column ICIdentificationUnitID = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnitInPart", "IdentificationUnitInPart" + DisplayOrder.ToString(), "IdentificationUnitID", 1, null
                ,Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database); // new Import_Column();
            ICIdentificationUnitID.IsSelected = true;
            ICIdentificationUnitID.CanBeTransformed = false;
            ICIdentificationUnitID.TypeOfSource = Import_Column.SourceType.Database;
            ICIdentificationUnitID.TypeOfEntry = Import_Column.EntryType.Database;
            ICIdentificationUnitID.ParentTableAlias(UnitTableAlias);

            DiversityCollection.Import_Column ICCollectionSpecimenID = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnitInPart", "IdentificationUnitInPart" + DisplayOrder.ToString(), "CollectionSpecimenID", 1, null
                ,Import_Column.SourceType.Database,Import_Column.FixingType.None,Import_Column.EntryType.Database); // new Import_Column();
            ICCollectionSpecimenID.IsSelected = true;
            ICCollectionSpecimenID.CanBeTransformed = false;
            ICCollectionSpecimenID.TypeOfSource = Import_Column.SourceType.Database;
            ICCollectionSpecimenID.TypeOfEntry = Import_Column.EntryType.Database;
            ICCollectionSpecimenID.ParentTableAlias(UnitTableAlias);

            DiversityCollection.Import_Column ICSpecimenPartID = DiversityCollection.Import_Column.GetImportColumn(StepKey, "IdentificationUnitInPart", "IdentificationUnitInPart" + DisplayOrder.ToString(), "SpecimenPartID", 1, null
                ,Import_Column.SourceType.Database,Import_Column.FixingType.None,Import_Column.EntryType.Database); // new Import_Column();
            ICSpecimenPartID.IsSelected = true;
            ICSpecimenPartID.CanBeTransformed = false;
            ICSpecimenPartID.TypeOfSource = Import_Column.SourceType.Database;
            ICSpecimenPartID.TypeOfEntry = Import_Column.EntryType.Database;
            ICSpecimenPartID.ParentTableAlias(PartTableAlias);
        }

        public bool AllStepsOK()
        {
            bool AllStepsOK = true;
            try
            {
                // test the steps
                string Message = "";
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DiversityCollection.Import.ImportSteps)
                {
                    if (KV.Value.IsGroupHaeder || !KV.Value.IsVisible())
                        continue;
                    if (!DiversityCollection.Import.ImportTables.ContainsKey(KV.Value.TableAlias()))
                        continue;
                    KV.Value.setStepError();
                    string Error = KV.Value.getStepError();
                    if (Error.Length > 0)
                    {
                        AllStepsOK = false;
                        Message += Error + "\r\n";
                    }
                }
                if (Message.Length > 0)
                    System.Windows.Forms.MessageBox.Show(Message);
            }
            catch (System.Exception ex) { return false; }
            return AllStepsOK;
        }

        /// <summary>
        /// Creates missing columns that have a foreign key relation to a superior table
        /// </summary>
        private void getExistingColumnsFromSuperiorTables()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Table> KV in DiversityCollection.Import._ImportTables)
            {
                System.Collections.Generic.Dictionary<string, DiversityCollection.Import_Column> ICC = KV.Value.FindColumnsWithForeignRelations();
                if (ICC.Count > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> CC in ICC)
                    {
                        if (!DiversityCollection.Import.ImportColumns.ContainsKey(CC.Value.Key))
                        {
                            DiversityCollection.Import.ImportColumns.Add(CC.Value.Key, CC.Value);
                        }
                    }
                }
            }
        }

        private void initColumnDictionaries()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Table> KV in DiversityCollection.Import._ImportTables)
                KV.Value.initImportColumnDictionary();
        }

        /// <summary>
        /// create missing superior tables that have relations via a foreign key
        /// </summary>
        private void createMissingSuperiorTables()
        {
            try
            {
                System.Collections.Generic.List<DiversityCollection.Import_Table> SuperiorTables = new List<Import_Table>();
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Table> IT in DiversityCollection.Import._ImportTables)
                {
                    System.Collections.Generic.Dictionary<string, DiversityCollection.Import_Column> PK = IT.Value.FindPKColumnsWithMissingForeignRelations();
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> kvPK in PK)
                    {
                        if (!this.PotentialUpperTables.Contains(kvPK.Value.ParentTable()))
                            continue;
                        DiversityCollection.Import_Column IC = DiversityCollection.Import_Column.GetImportColumn("", kvPK.Value.ParentTable(), kvPK.Value.ParentTableAlias(), kvPK.Value.ParentColumn(), 1, null
                            ,Import_Column.SourceType.Database,Import_Column.FixingType.None,Import_Column.EntryType.Database);// new Import_Column();
                        IC.Table = kvPK.Value.ParentTable();
                        IC.TableAlias = kvPK.Value.ParentTableAlias();
                        IC.Column = kvPK.Value.ParentColumn();
                        IC.IsSelected = true;
                        if (!DiversityCollection.Import.ImportColumns.ContainsKey(IC.Key))
                        {
                            DiversityCollection.Import.ImportColumns.Add(IC.Key, IC);
                            if (!DiversityCollection.Import.ImportTables.ContainsKey(kvPK.Value.TableAlias))
                            {
                                DiversityCollection.Import_Table T = new Import_Table(kvPK.Value.TableAlias, kvPK.Value.Table, this._Grid);
                                SuperiorTables.Add(T);
                                T.CreateMissingPKColumns();
                            }
                        }
                        if (!DiversityCollection.Import.ImportTables.ContainsKey(kvPK.Value.ParentTableAlias()))
                        {
                            DiversityCollection.Import_Table T = new Import_Table(kvPK.Value.ParentTableAlias(), kvPK.Value.ParentTable(), this._Grid);
                            T.AddedForImportAsSuperiorTable = true;
                            SuperiorTables.Add(T);
                            T.CreateMissingPKColumns();
                        }
                    }
                }
                if (SuperiorTables.Count > 0)
                {
                    foreach (DiversityCollection.Import_Table T in SuperiorTables)
                    {
                        if(!DiversityCollection.Import.ImportTables.ContainsKey(T.TableAlias))
                            DiversityCollection.Import.ImportTables.Add(T.TableAlias, T);
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private System.Collections.Generic.List<string> _PotentialUpperTables;
        /// <summary>
        /// The list of the tables that can be an upper table that may be created for the import, e.g. a missing CollectionEvent for coordinates
        /// </summary>
        private System.Collections.Generic.List<string> PotentialUpperTables
        {
            get
            {
                if (this._PotentialUpperTables == null)
                {
                    this._PotentialUpperTables = new List<string>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> IS in DiversityCollection.Import.ImportSteps)
                    {
                        if (!this._PotentialUpperTables.Contains(IS.Value.TableName()))
                            this._PotentialUpperTables.Add(IS.Value.TableName());
                    }
                }
                return this._PotentialUpperTables;
            }
        }

        private void completeTablePKs()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Table> KV in DiversityCollection.Import._ImportTables)
            {
                if (KV.Key.StartsWith("CollectionAgent"))
                { } // for testing
                KV.Value.CreateMissingPKColumns();
            }
        }

        private void ClearValues()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Table> KV in DiversityCollection.Import._ImportTables)
                KV.Value.ClearValues();
        }

        private int _CurrentImportRow;
        private int CurrentImportRow
        {
            get
            {
                return this._CurrentImportRow;
            }
            set
            {
                //foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Table> KV in DiversityCollection.Import.ImportTables)
                //    KV.Value.ClearValues();
                _CurrentImportRow = value;
                //foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Table> KV in DiversityCollection.Import.ImportTables)
                //{
                //    KV.Value.FillValues(this._Grid, this._CurrentImportRow);
                //}
            }
        }

        private System.Collections.Generic.Dictionary<string, string> _TablesForImport;
        private System.Collections.Generic.Dictionary<string, string> TablesForImport
        {
            get
            {
                if (this._TablesForImport == null)
                {
                    this._TablesForImport = new Dictionary<string, string>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DiversityCollection.Import.ImportSteps)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> CC in DiversityCollection.Import.ImportColumns)
                        {
                            if (CC.Value.IsSelected)
                            {
                                if (!this._TablesForImport.ContainsKey(CC.Value.TableAlias))
                                    this._TablesForImport.Add(CC.Value.TableAlias, CC.Value.Table);
                            }
                        }
                    }
                }
                return this._TablesForImport;
            }
        }

        #endregion

    }
}
