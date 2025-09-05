using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Import
{

    public interface iWizardInterface
    {
        void RefreshStepSelectionPanel();
        void RefreshStepPanel();
        bool FileIsOK();
        void SetStartStep();
        bool ShowLoggingColumns();
        System.Windows.Forms.Panel DataColumnPanel();
        System.Windows.Forms.DataGridView DataGridView();
        void setDataGridColorRange();
        void setDataGridLineColor(int iLine, System.Drawing.Color Color);
        void SetGridHeaders();
        void setActiveDataGridRow(int RowNumber);
        void setDataHeader(DiversityWorkbench.Import.Step Step);
        void setDataHeader(DiversityWorkbench.Import.Step Step, DiversityWorkbench.Import.StepColumnGroup Group);
        void setTableMessage(DiversityWorkbench.Import.DataTable DataTable);
        void FreezeHaederline();
        void setProgressBarPosition(int Position);
        void setProgressBarMaximum(int Maximum);
        void setProgressBarVisibility(bool IsVisible);
        void SetLockState(bool locked);

        System.Collections.Generic.Dictionary<Import.SchemaFileSource, string> SourcesForSchemaFiles();

        void SetSourcesForSchemaFiles(System.Collections.Generic.Dictionary<Import.SchemaFileSource, string> Sources);

    }

    public class Import
    {

        #region Static properties and parameter

        #region Colors

        public static readonly System.Drawing.Color ColorAttachment = System.Drawing.Color.LightBlue;
        public static readonly System.Drawing.Color ColorKeyColumn = System.Drawing.Color.LightGoldenrodYellow;
        public static readonly System.Drawing.Color ColorDecisive = System.Drawing.Color.Green;
        public static readonly System.Drawing.Color ColorUpdate = System.Drawing.Color.Yellow;
        public static readonly System.Drawing.Color ColorNoDifference = System.Drawing.Color.LightYellow;
        public static readonly System.Drawing.Color ColorNoData = System.Drawing.Color.LightGray;
        public static readonly System.Drawing.Color ColorError = System.Drawing.Color.Pink;
        public static readonly System.Drawing.Color ColorImport = System.Drawing.Color.LightGreen;

        #endregion

        #region Templates for steps and tables

        private static System.Collections.Generic.SortedDictionary<string, DiversityWorkbench.Import.Step> _TemplateSteps;
        /// <summary>
        /// The template for the steps as generated in the initial construction
        /// </summary>
        public static System.Collections.Generic.SortedDictionary<string, DiversityWorkbench.Import.Step> TemplateSteps
        {
            get
            {
                if (Import._TemplateSteps == null)
                    Import._TemplateSteps = new SortedDictionary<string, Step>();
                return Import._TemplateSteps;
            }
            set { Import._TemplateSteps = value; }
        }

        private static System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.DataTable> _TemplateTables;
        /// <summary>
        /// the template of the tables as generated in the initial construction
        /// </summary>
        public static System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.DataTable> TemplateTables
        {
            get
            {
                if (Import._TemplateTables == null)
                {
                    Import._TemplateTables = new Dictionary<string, DataTable>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.Step> S in DiversityWorkbench.Import.Import.TemplateSteps)
                    {
                        DiversityWorkbench.Import.Import.TemplateTables.Add(S.Value.DataTableTemplate.TableAlias, S.Value.DataTableTemplate);
                    }
                }
                return Import._TemplateTables;
            }
            set { Import._TemplateTables = value; }
        }

        #endregion

        #region Reset

        public static void Reset()
        {
            DiversityWorkbench.Import.Import.AttachmentColumn = null;
            //DiversityWorkbench.Import.Import.SelectedTableAliases = null;
            DiversityWorkbench.Import.Import._TableListForImport = null;
            DiversityWorkbench.Import.Import._Steps = null;
            DiversityWorkbench.Import.Import._Tables = null;
            DiversityWorkbench.Import.Import._Encoding = null;
            DiversityWorkbench.Import.Import._EndLine = -1;
            DiversityWorkbench.Import.Import._StartLine = -1;
            DiversityWorkbench.Import.Import._SchemaName = "";
        }

        public static void ResetTemplate()
        {
            DiversityWorkbench.Import.Import.AttachmentColumn = null;
            DiversityWorkbench.Import.Import._TemplateSteps = null;
            DiversityWorkbench.Import.Import._TemplateTables = null;
        }

        public static void ResetTableMessages()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataTable> DT in DiversityWorkbench.Import.Import.Tables)
            {
                DT.Value.Messages.Clear();
            }
        }

        #endregion

        #region Dictionaries of steps and tables

        private static System.Collections.Generic.SortedDictionary<string, DiversityWorkbench.Import.Step> _Steps;
        /// <summary>
        /// the steps of the import
        /// </summary>
        public static System.Collections.Generic.SortedDictionary<string, DiversityWorkbench.Import.Step> Steps
        {
            get
            {
                if (Import._Steps == null)// || Import._Steps.Count == 0)
                {
                    // Creating all step according to the the templates
                    Import._Steps = new SortedDictionary<string, Step>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.Step> KV in DiversityWorkbench.Import.Import.TemplateSteps)
                    {
                        if (KV.Value != null)
                        {
                            // Creating the table of the step
                            DiversityWorkbench.Import.DataTable DT = DiversityWorkbench.Import.DataTable.GetTableParallel(KV.Value.DataTableTemplate, false, "");
                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KVcolumn in KV.Value.DataTableTemplate.DataColumns)
                            {
                                if (KVcolumn.Value.IsSelected)
                                    DT.DataColumns[KVcolumn.Key].IsSelected = true;
                                if (KVcolumn.Value.TypeOfSource != DataColumn.SourceType.NotDecided
                                    && DT.DataColumns[KVcolumn.Key].TypeOfSource != KVcolumn.Value.TypeOfSource)
                                    DT.DataColumns[KVcolumn.Key].TypeOfSource = KVcolumn.Value.TypeOfSource;
                                if (KVcolumn.Value.ValueIsPreset && KVcolumn.Value.Value != null && KVcolumn.Value.Value.Length > 0)
                                    DT.DataColumns[KVcolumn.Key].Value = KVcolumn.Value.Value;
                                if (!KVcolumn.Value.AllowInsert)
                                    DT.DataColumns[KVcolumn.Key].AllowInsert = KVcolumn.Value.AllowInsert;
                            }
                            System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.StepColumnGroup> ColumnGroups = new Dictionary<string, StepColumnGroup>();
                            if (KV.Value.StepColumnGroups != null)
                            {
                                System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.StepColumnGroup> StepColumnGroups = new Dictionary<string, StepColumnGroup>();
                                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.StepColumnGroup> KVgroup in KV.Value.StepColumnGroups)
                                {
                                    System.Collections.Generic.List<string> L = new List<string>();
                                    foreach (string s in KVgroup.Value.Columns)
                                        L.Add(s);
                                    DiversityWorkbench.Import.StepColumnGroup G = new StepColumnGroup(KVgroup.Value.Image, KVgroup.Value.DisplayText, L);
                                    ColumnGroups.Add(KVgroup.Key, G);
                                }
                            }

                            DiversityWorkbench.Import.Step S = DiversityWorkbench.Import.Step.GetStep(KV.Value.PositionKey, DT, KV.Value.Image, KV.Value.IndentPosition);

                            if (ColumnGroups.Count > 0)
                                S.StepColumnGroups = ColumnGroups;
                            S.IsSelected = KV.Value.IsSelected;
                            S.MustSelect = KV.Value.MustSelect;
                            if (!Import._Steps.ContainsKey(KV.Key))
                                Import._Steps.Add(KV.Key, KV.Value);
                        }
                    }
                }
                return Import._Steps;
            }
            //set { Import._Steps = value; }
        }

        private static System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.DataTable> _Tables;
        /// <summary>
        /// all tables available for the import
        /// </summary>
        public static System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.DataTable> Tables
        {
            get
            {
                if (Import._Tables == null)
                {
                    Import._Tables = new Dictionary<string, DataTable>();
                }
                return Import._Tables;
            }
            //set { Import._Tables = value; }
        }

        #endregion

        #region Settings for the import

        #region File and schema

        private static string _FileName;
        /// <summary>
        /// The file from where the data should be imported, tab separated text file
        /// </summary>
        public static string FileName
        {
            get
            {
                if (_FileName == null)
                    _FileName = "";
                return _FileName;
            }
            set
            {
                if (_FileName != value)
                {
                    _FileName = value;
                    _LineValuesFromFile = null;
                }
            }
        }

        private static string _ErrorFileName;
        /// <summary>
        /// The name of the file containing the data that produced errors during the import
        /// </summary>
        public static string ErrorFileName
        {
            get { return Import._ErrorFileName; }
            set { Import._ErrorFileName = value; }
        }

        public static bool SaveFailedLinesInErrorFile = true;

        private static string _SchemaName;
        /// <summary>
        /// The XML schema containing the settings for the import
        /// </summary>
        public static string SchemaName
        {
            get
            {
                if (_SchemaName == null)
                    _SchemaName = "";
                return _SchemaName;
            }
            set { _SchemaName = value; }
        }

        /// <summary>
        /// if the selected schema has the correct module, target and version
        /// </summary>
        public static bool? SelectedImportSchemaIsValid;

        private static int _SchemaVersion = 1;
        /// <summary>
        /// The version of the schema file, necessary to handle different versions of the schema 
        /// e.g. later versions containing additional columns that are missing in previous versions
        /// </summary>
        private static int SchemaVersion
        {
            get { return _SchemaVersion; }
            set
            {
                _SchemaVersion = value;
                switch (_SchemaVersion)
                {
                    case 0:
                        goto case 1;
                    case 1:
                        goto case 2;
                    case 2:
                        break;
                }
            }
        }

        #endregion

        #region Separator

        public enum separator { TAB, SEMICOLON }
        public enum fileExtension { csv, txt }

        private static separator _Separator = separator.TAB;
        private static fileExtension _FileExtension = fileExtension.txt;

        public static separator Separator
        {
            get
            {
                return _Separator;
            }
            set
            {
                _Separator = value;
            }
        }

        public static fileExtension FileExtension
        {
            get
            {
                return _FileExtension;
            }
            set { _FileExtension = value; }
        }

        #endregion

        #region Encoding

        private static System.Collections.Generic.Dictionary<string, System.Text.Encoding> _Encodings = new Dictionary<string, Encoding>();

        /// <summary>
        /// The encodings allowed for the import file
        /// </summary>
        public static System.Collections.Generic.Dictionary<string, System.Text.Encoding> Encodings
        {
            get
            {
                try
                {
                    if (Import._Encodings == null || Import._Encodings.Count == 0)
                    {
                        Import._Encodings = new Dictionary<string, System.Text.Encoding>();
                        // #253
                        //Import._Encodings.Add("", null);
                        Import._Encodings.Add("ASCII", System.Text.Encoding.ASCII);
                        //Import._Encodings.Add("ANSI", System.Text.Encoding.Default);
                        Import._Encodings.Add("UTF8", System.Text.Encoding.UTF8);
                        Import._Encodings.Add("UTF32", System.Text.Encoding.UTF32);
                        Import._Encodings.Add("Unicode", System.Text.Encoding.Unicode);
                        Import._Encodings.Add("windows-1252", System.Text.Encoding.GetEncoding(1252));
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                return Import._Encodings;
            }
            //set { Import._Encodings = value; }
        }

        private static System.Text.Encoding _Encoding;
        /// <summary>
        /// The encoding of the imported file
        /// #253 Default is UTF8
        /// </summary>
        public static System.Text.Encoding Encoding
        {
            get
            {
                if (_Encoding == null)
                    _Encoding = System.Text.Encoding.UTF8;
                return _Encoding;
            }
            set { _Encoding = value; }
        }
        /// <summary>
        /// The name of the encoding as shown in an interface
        /// </summary>
        /// <param name="Encoding">the encoding</param>
        /// <returns>the name of the encoding</returns>
        public static string EncodingDisplayText(System.Text.Encoding Encoding)
        {
            if (Import.Encodings.ContainsValue(Encoding))
            {
                foreach (System.Collections.Generic.KeyValuePair<string, System.Text.Encoding> KV in Import.Encodings)
                {
                    if (KV.Value == Encoding)
                        return KV.Key;
                }
            }
            return "";
        }

        #endregion

        #region Lines

        /// <summary>
        /// if the first line in the imported file contains the column definition and should not be imported
        /// </summary>
        public static bool FirstLineContainsColumnDefinition = true;

        private static int _StartLine;
        /// <summary>
        /// The line where the import starts
        /// </summary>
        public static int StartLine
        {
            get { return _StartLine; }
            set
            {
                if (value <= LastLine)
                    _StartLine = value;
                //else _StartLine = 1;
            }
        }
        private static int _EndLine;
        /// <summary>
        /// the line where the import stops
        /// </summary>
        public static int EndLine
        {
            get { return _EndLine; }
            set
            {
                if (value <= LastLine)
                    _EndLine = value;
                else _EndLine = _LastLine;
            }
        }

        private static int _LastLine;
        public static int LastLine
        {
            get { return _LastLine; }
            set { _LastLine = value; }
        }
        /// <summary>
        /// Option to translate the string \r\n into a line break in the database
        /// </summary>
        public static bool TranslateReturn { get; set; } = false;

        #endregion

        #region Language

        public enum Langage { US, de, sp, fr, it, en, IVL }

        private static Langage _CurrentLanguage;
        /// <summary>
        /// The language used for the current import
        /// </summary>
        public static Langage CurrentLanguage
        {
            get
            {
                if (_CurrentLanguage == null)
                    _CurrentLanguage = Langage.de;
                return _CurrentLanguage;
            }
            set
            {
                Langage oldLanguage = _CurrentLanguage;
                _CurrentLanguage = value;
                try
                {
                    System.Globalization.CultureInfo CI = LanguageCode == "iv" ? System.Globalization.CultureInfo.InvariantCulture : new System.Globalization.CultureInfo(LanguageCode);
                }
                catch (Exception)
                {
                    _CurrentLanguage = oldLanguage;
                }
            }
        }

        private static string _LanguageCode;
        /// <summary>
        /// The language used for the data of this import e.g. for converting date values
        /// </summary>
        public static string LanguageCode
        {
            get
            {
                switch (CurrentLanguage)
                {
                    case Langage.US:
                        _LanguageCode = "en-US";
                        break;
                    case Langage.en:
                        _LanguageCode = "en-EN";
                        break;
                    case Langage.de:
                        _LanguageCode = "de-DE";
                        break;
                    case Langage.fr:
                        _LanguageCode = "fr-FR";
                        break;
                    case Langage.it:
                        _LanguageCode = "it-IT";
                        break;
                    case Langage.sp:
                        _LanguageCode = "sp-SP";
                        break;
                    case Langage.IVL:
                        _LanguageCode = "iv";
                        break;
                }
                return Import._LanguageCode;
            }
            //set { Import._LanguageCode = value; }
        }

        #endregion

        #endregion

        #region File related informations

        private static int _CurrentLine;
        /// <summary>
        /// the current line of the import when importing
        /// </summary>
        public static int CurrentLine
        {
            get { return Import._CurrentLine; }
            //set { Import._CurrentLine = value; }
        }

        private static System.Collections.Generic.Dictionary<int, string> _LineValuesFromFile;
        /// <summary>
        /// the lines as found in the file
        /// </summary>
        /// <returns></returns>
        public static System.Collections.Generic.Dictionary<int, string> LineValuesFromFile()
        {
            if (DiversityWorkbench.Import.Import._LineValuesFromFile == null)
                DiversityWorkbench.Import.Import._LineValuesFromFile = new Dictionary<int, string>();
            return DiversityWorkbench.Import.Import._LineValuesFromFile;
        }

        public static System.Collections.Generic.Dictionary<int, string> LineValuesFromFile(System.Windows.Forms.DataGridView Grid, int Line)
        {
            if (DiversityWorkbench.Import.Import._LineValuesFromFile == null)
                DiversityWorkbench.Import.Import._LineValuesFromFile = new Dictionary<int, string>();
            try
            {
                if (Line - 1 != DiversityWorkbench.Import.Import.CurrentLine ||
                    DiversityWorkbench.Import.Import._LineValuesFromFile.Count == 0)
                {
                    DiversityWorkbench.Import.Import._CurrentLine = Line - 1;
                    DiversityWorkbench.Import.Import._LineValuesFromFile.Clear();
                    for (int c = 0; c < Grid.Columns.Count; c++)
                    {
                        string Value = "";
                        if (Grid.Rows.Count > DiversityWorkbench.Import.Import._CurrentLine &&
                            Grid.Rows[DiversityWorkbench.Import.Import._CurrentLine].Cells[c].Value != null)
                            Value = Grid.Rows[DiversityWorkbench.Import.Import._CurrentLine].Cells[c].Value.ToString();
                        else
                        {
                            // Value is missing
                        }
                        DiversityWorkbench.Import.Import._LineValuesFromFile.Add(c, Value);
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return DiversityWorkbench.Import.Import._LineValuesFromFile;
        }

        private static int _NumberOfFailedImportLines;
        /// <summary>
        ///  the number of lines where errors occured during the import
        /// </summary>
        public static int NumberOfFailedImportLines
        {
            get { return Import._NumberOfFailedImportLines; }
            set { Import._NumberOfFailedImportLines = value; }
        }

        public static int NumberOfLinesImported = 0;
        public static int NumberOfLinesUpdated = 0;
        public static int NumberOfLinesEmpty = 0;
        public static int NumberOfLinesNoDifference = 0;
        public static int NumberOfLinesDupliate = 0;
        public static System.Collections.Generic.List<string> Duplicates = new List<string>();

        #endregion

        #region File handling

        /// <summary>
        /// Split tab separated input line into columns
        /// Removes quotation marks from the columns
        /// </summary>
        /// <param name="line">Tab separated input line</param>
        /// <param name="columns">Resulting column strings</param>
        /// <returns>'true' if line was detected as empty</returns>
        public static bool SplitLine(string line, List<string> columns)
        {
            bool result = true;
            string ColumnSeparator = "\t";
            if (Separator == separator.SEMICOLON)
                ColumnSeparator = ";";
            columns.Clear();

            while (line.Length > 0)
            {
                string column = "";
                int tabIdx = line.IndexOf(ColumnSeparator);

                // Check if value is included in quotation marks
                if (line[0] == '"')
                {
                    // Find end of quoted string
                    int startIdx = 1;
                    int endIdx = 0;
                    while (endIdx > -1 && line.Length > endIdx + 1)
                    {
                        // Find next quotation mark
                        endIdx = line.IndexOf('"', endIdx + 1);
                        if (endIdx > -1)
                        {
                            // Find next tab if actual one is included in quoted string
                            if (tabIdx > -1 && endIdx > tabIdx)
                                tabIdx = line.IndexOf(ColumnSeparator, endIdx + 1);

                            if (line.Length > endIdx + 1 && line[endIdx + 1] == '"')
                            {
                                // Ignore ""
                                endIdx++;

                                if (line.Length <= endIdx + 1)
                                {
                                    // Copy text in quotation marks and replace "" by "
                                    column = line.Substring(1, endIdx).Replace("\"\"", "\"").Replace(ColumnSeparator, "");
                                    startIdx = endIdx + 1; // No closing quotation mark
                                    break;
                                }
                            }
                            else
                            {
                                // Copy text in quotation marks and replace "" by "
                                column = line.Substring(1, endIdx - 1).Replace("\"\"", "\"").Replace(ColumnSeparator, "");
                                startIdx = endIdx + 1; // Ingnore closing quotation mark
                                break;
                            }
                        }
                        else
                        {
                            // Copy text in quotation marks and replace "" by "
                            if (tabIdx > -1)
                                endIdx = tabIdx;
                            else
                                endIdx = line.Length;
                            column = line.Substring(1, endIdx - 1).Replace("\"\"", "\"").Replace(ColumnSeparator, "");
                            startIdx = endIdx; // No closing quotation mark
                            break;
                        }
                    }

                    // Append rest of string to column
                    if (tabIdx > -1)
                        endIdx = tabIdx;
                    else
                        endIdx = line.Length;
                    if (endIdx > startIdx)
                        column += line.Substring(startIdx, endIdx - startIdx).Replace(ColumnSeparator, "").TrimEnd();
                    if (tabIdx < 0)
                        line = "";
                }
                else
                {
                    // No starting quotation mark -> no replacements of quotation marks
                    if (tabIdx > -1)
                        column = line.Substring(0, tabIdx).Replace(ColumnSeparator, "").Trim();
                    else
                    {
                        column = line.Trim();
                        line = "";
                    }
                }
                columns.Add(column);
                if (column != "")
                    result = false;
                line = line.Substring(tabIdx + 1);
            }
            return result;
        }

        /// <summary>
        /// Reading the file and insert the values into the datagrid
        /// </summary>
        /// <param name="File">The file that should be read</param>
        /// <param name="Grid">The datagrid in which the data should be inserted</param>
        /// <param name="Encoding">The encoding of the file</param>
        /// <param name="EndLine">If the reading should be stopped at a certain line, the number of the line</param>
        /// <returns></returns>
        public static bool readFileInDataGridView(
            System.IO.FileInfo File,
            System.Windows.Forms.DataGridView Grid,
            System.Text.Encoding Encoding,
            int? EndLine)
        {
            Grid.Columns.Clear();
            try
            {
                // Count the file lines and insert grid columns and rows
                System.IO.StreamReader sr = DiversityWorkbench.Import.Import.StreamReader(File.FullName, Encoding);
                using (sr)
                {
                    String line;
                    bool emptyLine;
                    List<string> columns = new List<string>();
                    int iLine = 0;
                    int iColumn = 0;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (EndLine != null && iLine > EndLine)
                            break;

                        // Insert the columns
                        emptyLine = SplitLine(line, columns);

                        // Ignore leading empty lines
                        if (iLine == 0 && emptyLine)
                            continue;

                        if (iLine == 0)
                        {
                            for (iColumn = 0; iColumn < columns.Count; iColumn++)
                                Grid.Columns.Add("Column_" + iColumn.ToString(), "");
                        }
                        iLine++;
                    }
                    Grid.Rows.Add(iLine);
                }

                // Read the file into the data grid view
                sr = DiversityWorkbench.Import.Import.StreamReader(File.FullName, Encoding);
                using (sr)
                {
                    String line;
                    bool emptyLine;
                    List<string> columns = new List<string>();
                    int iColumn = 0;
                    int iLine = 0;
                    bool HeaderPassed = false;
                    bool EmptyLinePassed = false;
                    int iHeader = 0;
                    // reading the first for lines into the datagrid
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (EndLine != null && iLine > EndLine)
                            break;

                        // reading the columns
                        emptyLine = SplitLine(line, columns);

                        // Ignore leading empty lines
                        if (iLine == 0 && emptyLine)
                            continue;

                        //if (iLine == 0)
                        //{
                        //    for (iColumn = 0; iColumn < columns.Count; iColumn++)
                        //        Grid.Columns.Add("Column_" + iColumn.ToString(), "");
                        //}

                        // reading the lines
                        if (Grid.RowCount <= iLine)
                            Grid.Rows.Add(1);
                        iColumn = 0;
                        if (!HeaderPassed || emptyLine)
                        {
                            // skip the header lines
                            if (emptyLine && iHeader > 1)
                                EmptyLinePassed = true;
                            iHeader++;
                            if (iHeader > 3 && EmptyLinePassed)
                                HeaderPassed = true;
                            Grid.Rows[iLine].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
                        }
                        for (iColumn = 0; iColumn < columns.Count; iColumn++)
                        {
                            // Check if column index is exceeded
                            if (Grid.ColumnCount <= iColumn)
                            {
                                if (FirstLineContainsColumnDefinition)
                                {
                                    // Ask if reading shall be aborted
                                    if (System.Windows.Forms.MessageBox.Show("In line " + iLine.ToString() + " the value " + line + " was outside the range of columns defined in the header line.\r\n\r\nStop reading the file?", "Value outside header", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                                        return false;
                                    else
                                        break;
                                }
                                else
                                {
                                    //Insert missing column
                                    string columnName = columns[iColumn];
                                    if (columnName.IndexOf(' ') > -1)
                                        columnName = columnName.Substring(0, columnName.IndexOf(' '));
                                    Grid.Columns.Add("Column_" + iColumn.ToString(), columnName);
                                }
                            }
                            // Insert column
                            Grid[iColumn, iLine].Value = columns[iColumn];
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

        /// <summary>
        /// Copy the lines of the file that failed during the import into a new file
        /// </summary>
        /// <param name="SourceFile">The source file that was imported</param>
        /// <param name="CopyFilePostfix">The postfix marking the new file</param>
        /// <param name="Encoding">The encoding of the source file</param>
        /// <param name="StartLine">The line where the transfer should start</param>
        /// <param name="EndLine">The line where the transfer should end</param>
        /// <param name="FirstLineContainsColumnDefinition">If the first line contains the column definition</param>
        /// <returns>The name of the file in which the data where transferred</returns>
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
                bool CopyNameIsNew = false;
                System.IO.FileInfo Copy = new System.IO.FileInfo(FileNameCopy);
                while (!CopyNameIsNew)
                {
                    if (!Copy.Exists)
                        CopyNameIsNew = true;
                    else
                    {
                        DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Failed lines", "Please change the name of the file where the failed lines should be stored", Copy.Name.Substring(0, Copy.Name.IndexOf(".")));
                        f.ShowDialog();
                        if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            FileNameCopy = SourceFile.FullName.Substring(0, SourceFile.FullName.Length - (SourceFile.Name.Length)) + f.String + SourceFile.Extension;
                            Copy = new System.IO.FileInfo(FileNameCopy);
                            if (!Copy.Exists)
                                CopyNameIsNew = true;
                        }
                        else
                        {
                            if (System.Windows.Forms.MessageBox.Show("The file " + Copy + " exists. This file will be deleted to save the errors. Rename or copy this file if you want to keep it", "Delete existing file?", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                Copy.Delete();
                                CopyNameIsNew = true;
                            }
                        }
                    }
                }

                System.IO.StreamReader sr = DiversityWorkbench.Import.Import.StreamReader(SourceFile.FullName, Encoding);
                System.IO.StreamWriter sw = DiversityWorkbench.Import.Import.StreamWriter(FileNameCopy, Encoding);
                using (sr)
                {
                    String line;
                    int iLine = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        iLine++;
                        if (iLine > EndLine)
                            break;
                        if (FirstLineContainsColumnDefinition && iLine == 1)
                            sw.WriteLine(line);
                        else if (DiversityWorkbench.Import.Import._FailedLines.Contains(iLine))
                            sw.WriteLine(line);

                    }
                    sw.Close();
                }
            }
            catch (System.IO.IOException IOex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(IOex);
                //System.Windows.Forms.MessageBox.Show(IOex.Message);
                //return false;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                //return false;
            }
            return FileNameCopy;
        }

        private static System.Collections.Generic.List<int> _FailedLines = new List<int>();

        #endregion

        #region Attachment

        /// <summary>
        /// The type of the attachment
        /// ChildParentInSameTable: when the data should be attached to data in the same table via a child-parent relation 
        ///     e.g. CollectionEventSeries
        /// TableOnlyForAttachment: when the whole table has only be inserted for attachment and only the attachment column is shown 
        ///     e.g. CollectionEventSeries for the import of CollectionEvent
        /// AttachToTable: Standard - attach to a table where the table contains the identity
        ///     e.g. CollectionSpecimen
        /// AttachToChild: when the attachment information is located in a child table of the main table
        ///     e.g. CollectionAgent
        /// </summary>
        public enum TypeOfAttachment { UnDecided, NoAttachment, TableOnlyForAttachment, ChildParentInSameTable, AttachToTable, AttachToChild }

        private static TypeOfAttachment _AttachmentType;

        public static TypeOfAttachment AttachmentType
        {
            get
            {
                if (_AttachmentType != TypeOfAttachment.UnDecided)
                    return _AttachmentType;

                // No AttachmentColumn selected
                if (DiversityWorkbench.Import.Import.AttachmentColumn == null)
                    _AttachmentType = TypeOfAttachment.NoAttachment;

                // table added only for attachment
                else if (DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias].IsForAttachment)
                    _AttachmentType = TypeOfAttachment.TableOnlyForAttachment;

                // internal child parent relation
                else if (DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias].AttachViaParentChildRelation())
                    _AttachmentType = TypeOfAttachment.ChildParentInSameTable;


                else
                {
                    bool IsChild = false;
                    string TableAlias = DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias;
                    foreach (string PK in DiversityWorkbench.Import.Import.Tables[TableAlias].PrimaryKeyColumnList)
                    {
                        if (DiversityWorkbench.Import.Import.Tables[TableAlias].DataColumns[PK].ForeignRelationTable == null)
                            continue;
                        else
                        {
                            IsChild = true;
                        }
                    }
                    if (IsChild)
                        _AttachmentType = TypeOfAttachment.AttachToChild;
                    else
                        _AttachmentType = TypeOfAttachment.AttachToTable;
                }


                //else _AttachmentType = TypeOfAttachment.AttachToTable;

                return _AttachmentType;
            }
            set { _AttachmentType = value; }
        }

        private static DiversityWorkbench.Import.DataColumn _AttachmentColumn;
        /// <summary>
        /// The column selected for attachment of the data
        /// </summary>
        public static DiversityWorkbench.Import.DataColumn AttachmentColumn
        {
            get { return Import._AttachmentColumn; }
            set
            {
                _AttachmentType = TypeOfAttachment.UnDecided;
                Import._AttachmentColumn = value;
            }
        }

        /// <summary>
        /// getting the attachment informations from the database
        /// </summary>
        /// <returns>false in case of an error</returns>
        private static bool GetAttachmentData()
        {
            bool OK = false;
            if (DiversityWorkbench.Import.Import.AttachmentColumn != null)
                OK = DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias].GetAttachmentData();
            else
                OK = true;
            return OK;
        }

        #endregion

        #region Default duplicate check

        private static DiversityWorkbench.Import.DataColumn _DefaultDuplicateCheckColumn;

        public static DiversityWorkbench.Import.DataColumn DefaultDuplicateCheckColumn
        {
            get
            {
                if (_DefaultDuplicateCheckColumn != null && _DefaultDuplicateCheckColumn != AttachmentColumn)
                    return _DefaultDuplicateCheckColumn;
                else
                    return null;
            }
            set { _DefaultDuplicateCheckColumn = value; }
        }

        private static bool _UseDefaultDuplicateCheck = true;

        public static bool UseDefaultDuplicateCheck
        {
            get { return Import._UseDefaultDuplicateCheck; }
            set { Import._UseDefaultDuplicateCheck = value; }
        }

        #endregion

        #region AttachmentUseTransformation

        private static bool _AttachmentUseTransformation;

        public static bool AttachmentUseTransformation
        {
            get { return Import._AttachmentUseTransformation; }
            set { Import._AttachmentUseTransformation = value; }
        }

        #endregion

        #region SQL

        //private static bool _RecordSQL = false;
        //public static bool RecordSQL { set => _RecordSQL = value; get => _RecordSQL; }

        //public static string SQLfile()
        //{
        //    return "SQL_" + DefaultFileName() + ".txt";
        //}

        //public static void AddSQLStatement(string SQL, bool failed = false)
        //{

        //}

        #endregion

        #region Import

        private static string _CurrentStepKey;

        public static string CurrentStepKey
        {
            get { return Import._CurrentStepKey; }
            set
            {
                Import._CurrentStepKey = value;
            }
        }

        // TODO: Evtl. gleiche funktion wie unten

        //private static System.Collections.Generic.List<string> _SelectedTableAliases;
        ///// <summary>
        ///// The list of tables that where selected for the import
        ///// </summary>
        //public static System.Collections.Generic.List<string> SelectedTableAliases
        //{
        //    get 
        //    {
        //        if (DiversityWorkbench.Import.Import._SelectedTableAliases == null)
        //        {
        //            DiversityWorkbench.Import.Import._SelectedTableAliases = new List<string>();
        //            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.Step> KV in DiversityWorkbench.Import.Import.Steps)
        //            {
        //                if (KV.Value.IsSelected)
        //                {
        //                    if (!DiversityWorkbench.Import.Import._SelectedTableAliases.Contains(KV.Value.DataTableTemplate.TableAlias))
        //                        DiversityWorkbench.Import.Import._SelectedTableAliases.Add(KV.Value.DataTableTemplate.TableAlias);
        //                }
        //            }
        //        }
        //        return Import._SelectedTableAliases; 
        //    }
        //    set { Import._SelectedTableAliases = value; }
        //}

        // TODO: Evtl. gleiche funktion wie oben

        private static System.Collections.Generic.List<string> _TableListForImport;
        /// <summary>
        /// The list of tables selected for the import according to the sequence defined by the steps
        /// </summary>
        public static System.Collections.Generic.List<string> TableListForImport
        {
            get
            {
                if (DiversityWorkbench.Import.Import._TableListForImport == null || DiversityWorkbench.Import.Import._TableListForImport.Count == 0)
                {
                    DiversityWorkbench.Import.Import._TableListForImport = new List<string>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.Step> KV in DiversityWorkbench.Import.Import.Steps)
                    {
                        if (KV.Value.IsSelected && !DiversityWorkbench.Import.Import._TableListForImport.Contains(KV.Value.DataTableTemplate.TableAlias))
                            DiversityWorkbench.Import.Import._TableListForImport.Add(KV.Value.DataTableTemplate.TableAlias);
                    }
                }
                return DiversityWorkbench.Import.Import._TableListForImport;
            }
            set { DiversityWorkbench.Import.Import._TableListForImport = value; }
        }

        /// <summary>
        /// Import the data into the database
        /// </summary>
        /// <param name="WizardInterface">The interface for the import</param>
        public static string ImportData(DiversityWorkbench.Import.iWizardInterface WizardInterface, bool IncludeDescription)
        {
            if (!DiversityWorkbench.Import.Import.ImportPreconditionsOK())
                return "";
            // reset the list of tables
            DiversityWorkbench.Import.Import._TableListForImport = null;
            // reset the line numbers
            DiversityWorkbench.Import.Import.NumberOfLinesImported = 0;
            DiversityWorkbench.Import.Import.NumberOfLinesUpdated = 0;
            DiversityWorkbench.Import.Import.NumberOfLinesEmpty = 0;
            DiversityWorkbench.Import.Import.NumberOfLinesNoDifference = 0;
            DiversityWorkbench.Import.Import.NumberOfFailedImportLines = 0;
            DiversityWorkbench.Import.Import.NumberOfLinesDupliate = 0;

            // reset Description
            DiversityWorkbench.Import.Import.ResetSchemaDescription();

            // reset Failed lines
            DiversityWorkbench.Import.Import._FailedLines = new List<int>();

            // reset Duplicates
            DiversityWorkbench.Import.Import.Duplicates = new List<string>();

            // reset the error file
            DiversityWorkbench.Import.Import.ErrorFileName = "";
            // Walk through the lines of the file
            WizardInterface.setProgressBarVisibility(true);
            WizardInterface.setProgressBarPosition(0);
            WizardInterface.setProgressBarMaximum(DiversityWorkbench.Import.Import.EndLine - DiversityWorkbench.Import.Import.StartLine + 1);
            for (int l = DiversityWorkbench.Import.Import.StartLine; l <= DiversityWorkbench.Import.Import.EndLine; l++)
            {
                DiversityWorkbench.Import.Import.ImportData(WizardInterface, l, false);
                WizardInterface.setProgressBarPosition(l - DiversityWorkbench.Import.Import.StartLine + 1);
                if (WizardInterface.DataGridView().Rows.Count < l)
                {
                    System.Windows.Forms.MessageBox.Show("Import stopped at end of file in line " + WizardInterface.DataGridView().Rows.Count.ToString() + ".");
                    break;
                }
            }
            if (DiversityWorkbench.Import.Import.NumberOfFailedImportLines > 0
                && DiversityWorkbench.Import.Import.SaveFailedLinesInErrorFile)
            {
                System.IO.FileInfo FI = new System.IO.FileInfo(DiversityWorkbench.Import.Import.FileName);
                DiversityWorkbench.Import.Import.ErrorFileName = DiversityWorkbench.Import.Import.CopyFileAccordingToDataGridView(
                    FI,
                    "Error",
                    DiversityWorkbench.Import.Import.Encoding,
                    DiversityWorkbench.Import.Import.StartLine,
                    DiversityWorkbench.Import.Import.EndLine,
                    DiversityWorkbench.Import.Import.FirstLineContainsColumnDefinition);
            }
            WizardInterface.setProgressBarPosition(0);
            return DiversityWorkbench.Import.Import.SaveSchemaFile(true, IncludeDescription, true, "");
        }

        /// <summary>
        /// Check if the preconditions of the import are fulfilled
        /// </summary>
        /// <returns></returns>
        public static bool ImportPreconditionsOK()
        {
            try
            {
                // Check if tables were selected
                DiversityWorkbench.Import.Import.TableListForImport = null;
                if (DiversityWorkbench.Import.Import.TableListForImport.Count == 0)
                {
                    string Message = "No tables where selected";
                    System.Windows.Forms.MessageBox.Show(Message);
                    return false;
                }

                System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.UserControlTableMessage> Messages = new Dictionary<string, UserControlTableMessage>();
                foreach (string T in DiversityWorkbench.Import.Import.TableListForImport)
                {
                    string Message = "";
                    DiversityWorkbench.Import.DataTable.TableMessageType MessageType = DiversityWorkbench.Import.Import.Tables[T].GetTableMessage(ref Message);
                    if (MessageType != DataTable.TableMessageType.OK)
                    {
                        DiversityWorkbench.Import.UserControlTableMessage U = new UserControlTableMessage(MessageType, Message);
                        Messages.Add(T, U);
                    }
                }
                if (Messages.Count > 0)
                {
                    System.Windows.Forms.Form F = new System.Windows.Forms.Form();
                    F.Width = 300;
                    F.Height = 100 + (Messages.Count * 40);
                    F.ShowIcon = false;
                    F.Text = "TODO:";
                    F.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                    F.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
                    System.Collections.Generic.Stack<string> Stack = new Stack<string>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.UserControlTableMessage> KV in Messages)
                    {
                        Stack.Push(KV.Key);
                    }
                    while (Stack.Count > 0)
                    {
                        string Table = Stack.Pop();
                        F.Controls.Add(Messages[Table]);
                        Messages[Table].Dock = System.Windows.Forms.DockStyle.Top;
                        System.Windows.Forms.Label L = new System.Windows.Forms.Label();
                        L.Text = Table;
                        L.Dock = System.Windows.Forms.DockStyle.Top;
                        L.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
                        F.Controls.Add(L);
                    }
                    F.ShowDialog();
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            return true;
        }

        #region Status of import

        public static bool ImportActive(ref string Message)
        {
            bool ImportIsActive = false;
            string SQL = "SELECT ID, Target, StartedBy, StartedAt, EndedBy, EndedAt " +
                "FROM ImportStatus " +
                "WHERE (ID = (SELECT MAX(ID) FROM ImportStatus)) AND NOT Ended IS NULL";
            System.Data.DataTable dt = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
            if (Message.Length == 0 && dt.Rows.Count == 1)
            {
                Message = "Active import stated by " + dt.Rows[0]["StartedBy"].ToString() + " at " + dt.Rows[0]["StartedAt"] + " is not finished";
            }
            return ImportIsActive;
        }

        public static bool ForceImportEnd()
        {
            string SQL = "update I set EndedAt = getdate(), EndedBy = suser_sname() from ImportStatus I where I.EndedBy IS NULL and I.ID = (select max(ID) from ImportStatus)";
            bool OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            return OK;
        }

        public static System.Data.DataTable ImportStatusOverview()
        {
            string SQL = "SELECT ID, Target, StartedBy, StartedAt, EndedBy, EndedAt " +
                "FROM ImportStatus " +
                "ORDER BY ID DESC";
            System.Data.DataTable dt = new System.Data.DataTable();
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
            return dt;
        }

        public static bool SetImportActive(string Target)
        {
            bool OK = true;
            string SQL = "if (select count(*) from ImportStatus I where I.EndedBy IS NULL and I.ID = (select max(ID) from ImportStatus)) = 0 " +
                "begin " +
                "INSERT INTO ImportStatus (Target)  " +
                "VALUES ('" + Target + "');  " +
                "SELECT MAX(ID) FROM ImportStatus; " +
                "end " +
                "else " +
                "begin " +
                "select 0; " +
                "end";
            string Message = "";
            OK = int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message), out _ImportID);
            return OK;
        }

        private static int _ImportID;

        public static bool SetImportEnd()
        {
            bool OK = true;
            string SQL = "if (select count(*) from ImportStatus I where I.EndedBy IS NULL and I.ID = (select max(ID) from ImportStatus)) = 1 " +
                "begin " +
                "update I set EndedAt = getdate(), EndedBy = suser_sname() from ImportStatus I where I.EndedBy IS NULL and I.ID = (select max(ID) from ImportStatus); " +
                "select 1; " +
                "end " +
                "else " +
                "begin " +
                "select 0; " +
                "end";
            string Message = "";
            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message) == "1")
                OK = true;
            else
                OK = false;
            return OK;
        }

        #endregion

        /// <summary>
        /// Import the data into the database
        /// </summary>
        /// <param name="WizardInterface">The interface for the import</param>
        /// <param name="Line">The line that should be imported</param>
        /// <param name="ForTesting">If the import should only be tested, i.e. Transaction-Rollback after finishing all steps</param>
        public static bool ImportData(DiversityWorkbench.Import.iWizardInterface WizardInterface, int Line, bool ForTesting)
        {
            // reset all values derived either from the file or from the database (e.g. Identity)
            try
            {
                foreach (string T in DiversityWorkbench.Import.Import.TableListForImport)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> DC in DiversityWorkbench.Import.Import.Tables[T].DataColumns)
                    {
                        if (DC.Value.Value != null && DC.Value.Value.Length > 0)
                        {
                            if (DC.Value.TypeOfSource == DataColumn.SourceType.Database && DC.Value.IsIdentity)
                                DC.Value.Value = "";
                            if (DC.Value.TypeOfSource == DataColumn.SourceType.File)
                                DC.Value.Value = "";
                            if (DC.Value.TypeOfSource == DataColumn.SourceType.Interface && (DC.Value.PrepareInsertDefined || DC.Value.PrepareUpdateDefined)) // Toni: Re-initialize prepared values
                                DC.Value.Value = DC.Value.OriginalValue;
                            if (DC.Value.TypeOfSource == DataColumn.SourceType.Interface && DC.Value.DataRetrievalType == DataColumn.RetrievalType.FunctionInDatabase) // Markus: Function corresponds to identity
                                DC.Value.Value = "";
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }


            //// reset Failed lines
            //DiversityWorkbench.Import.Import._FailedLines = new List<int>();

            // reset SchemaDescription
            DiversityWorkbench.Import.Import.ResetSchemaDescription();

            if (ForTesting) // this function is called directly by the testing option, so the reset of the tables must happen here
            {
                DiversityWorkbench.Import.Import._TableListForImport = null;
                if (!DiversityWorkbench.Import.Import.ImportPreconditionsOK())
                    return false;
            }

            // start import
            try
            {
                DiversityWorkbench.Import.Import.LineValuesFromFile(WizardInterface.DataGridView(), Line);
                //if (DiversityWorkbench.Import.Import.GetAttachmentData())
                {
                    Microsoft.Data.SqlClient.SqlConnection ImportConnection = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    ImportConnection.Open();
                    // Markus 3.12.2019: Umgestellt auf Serializable um gleichzeitige Importe zu trennen
                    Microsoft.Data.SqlClient.SqlTransaction ImportTransaction = ImportConnection.BeginTransaction(System.Data.IsolationLevel.Serializable, "ImportLine");
                    bool ErrorOccurred = false;

                    DiversityWorkbench.Import.DataTable.NeededAction SummarizedAction = DataTable.NeededAction.Undefined;
                    // Reading from file must happen before import of single tables to allow writing key values in dependent tables that should not be deleted
                    // when a dependet table is 
                    foreach (string T in DiversityWorkbench.Import.Import.TableListForImport)
                    {
                        DiversityWorkbench.Import.Import.Tables[T].ReadDataFromFile();
                        DiversityWorkbench.Import.Import.Tables[T].SetActionNeeded(DataTable.NeededAction.Undefined);
                    }

                    // Reading the Attachment data after the data from the file where retrieved
                    if (!DiversityWorkbench.Import.Import.GetAttachmentData())
                        return false;

                    // Check the Default duplicate check
                    bool IsDuplicate = false;
                    if (DiversityWorkbench.Import.Import.UseDefaultDuplicateCheck
                        &&
                        (DiversityWorkbench.Import.Import.AttachmentColumn == null ||
                        (DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName != DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.ColumnName) ||
                        DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableName != DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.DataTable.TableName)
                        &&
                        DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.DataTable.TableAlias].MergeHandling == DataTable.Merging.Insert
                        )
                    {
                        string CurrentCheckValue = "";
                        // Markus 18.1.24: Bugfix falls CurrentCheckValue nicht ermittelt werden kann
                        try
                        {
                            DiversityWorkbench.Import.DataTable dataTable = DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.DataTable.TableName];
                            DiversityWorkbench.Import.DataColumn dataColumn = dataTable.DataColumns[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.ColumnName];
                            if (dataColumn != null && dataColumn.Value != null)
                            {
                                CurrentCheckValue = dataColumn.Value.Replace("'", "''");
                            }
                            //CurrentCheckValue = DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.DataTable.TableName].DataColumns[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.ColumnName].Value.Replace("'", "''");
                        }
                        catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                        if (DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.DataTable.TableName].DataColumns[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.ColumnName].IsMultiColumn)
                        {
                            CurrentCheckValue = "";
                            if (DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.DataTable.TableName].DataColumns[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.ColumnName].Prefix != null)
                                CurrentCheckValue = DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.DataTable.TableName].DataColumns[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.ColumnName].Prefix.Replace("'", "''");
                            try
                            {
                                DiversityWorkbench.Import.DataTable dataTable = DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.DataTable.TableName];
                                DiversityWorkbench.Import.DataColumn dataColumn = dataTable.DataColumns[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.ColumnName];
                                if (dataColumn != null && dataColumn.Value != null)
                                {
                                    CurrentCheckValue += dataColumn.Value.Replace("'", "''");
                                }
                                //CurrentCheckValue = DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.DataTable.TableName].DataColumns[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.ColumnName].Value.Replace("'", "''");
                            }
                            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                            //CurrentCheckValue += DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.DataTable.TableName].DataColumns[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.ColumnName].Value.Replace("'", "''");
                            if (DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.DataTable.TableName].DataColumns[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.ColumnName].Postfix != null)
                                CurrentCheckValue += DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.DataTable.TableName].DataColumns[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.ColumnName].Postfix.Replace("'", "''");
                            foreach (DiversityWorkbench.Import.ColumnMulti CM in DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.DataTable.TableName].DataColumns[DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.ColumnName].MultiColumns)
                            {
                                if (CM.Prefix != null)
                                    CurrentCheckValue += CM.Prefix.Replace("'", "''");
                                CurrentCheckValue += DiversityWorkbench.Import.Import.LineValuesFromFile()[CM.ColumnInFile];
                                if (CM.Postfix != null)
                                    CurrentCheckValue += CM.Postfix.Replace("'", "''");
                            }
                        }
                        if (CurrentCheckValue != "")
                        {
                            string SQL = "SELECT COUNT(*) FROM " + DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.DataTable.TableName + " WHERE " + DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.ColumnName + " = '" + CurrentCheckValue + "' AND RTRIM(" + DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.ColumnName + ") <> ''";
                            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL).ToString();
                            if (Result != "0")
                            {
                                IsDuplicate = true;
                                {
                                    if (!ForTesting)
                                    {
                                        DiversityWorkbench.Import.Import._FailedLines.Add(Line);
                                        DiversityWorkbench.Import.Import.NumberOfFailedImportLines++;
                                        DiversityWorkbench.Import.Import.NumberOfLinesDupliate++;
                                        DiversityWorkbench.Import.Import.Duplicates.Add(CurrentCheckValue);
                                    }
                                    else
                                        System.Windows.Forms.MessageBox.Show("The dataset with the " + DiversityWorkbench.Import.Import.DefaultDuplicateCheckColumn.ColumnName + " " + CurrentCheckValue + "\r\nis already " + Result + " time(s) present in the database.\r\nIt will not be imported into the database");
                                }
                                foreach (string T in DiversityWorkbench.Import.Import.TableListForImport)
                                {
                                    DiversityWorkbench.Import.Import.Tables[T].SetActionNeeded(DataTable.NeededAction.Duplicate);
                                }
                            }
                        }
                    }

                    if (!IsDuplicate)
                    {
                        foreach (string T in DiversityWorkbench.Import.Import.TableListForImport)
                        {
                            try
                            {
                                DiversityWorkbench.Import.Import.Tables[T].ImportData(ForTesting, ImportConnection, ImportTransaction);
                            }
                            catch (System.Exception ex)
                            {
                                ErrorOccurred = true;
                                DiversityWorkbench.Import.Import.NumberOfFailedImportLines++;
                            }
                            if (DiversityWorkbench.Import.Import.Tables[T].Messages.ContainsKey(DiversityWorkbench.Import.Import.CurrentLine))
                            {
                                if (!ErrorOccurred)
                                    DiversityWorkbench.Import.Import.NumberOfFailedImportLines++;
                                ErrorOccurred = true;
                            }
                            switch (DiversityWorkbench.Import.Import.Tables[T].ActionNeeded())
                            {
                                case DataTable.NeededAction.Correction:
                                    if (SummarizedAction != DataTable.NeededAction.Error)
                                        SummarizedAction = DataTable.NeededAction.Correction;
                                    break;
                                case DataTable.NeededAction.Error:
                                    SummarizedAction = DataTable.NeededAction.Error;
                                    break;
                                case DataTable.NeededAction.Insert:
                                    if (SummarizedAction != DataTable.NeededAction.Error
                                        && SummarizedAction != DataTable.NeededAction.Correction)
                                        SummarizedAction = DataTable.NeededAction.Insert;
                                    break;
                                case DataTable.NeededAction.NoData:
                                    if (SummarizedAction == DataTable.NeededAction.Undefined)
                                        SummarizedAction = DataTable.NeededAction.NoData;
                                    break;
                                case DataTable.NeededAction.NoDifferences:
                                    if (SummarizedAction != DataTable.NeededAction.Error
                                        && SummarizedAction != DataTable.NeededAction.Correction
                                        && SummarizedAction != DataTable.NeededAction.Insert
                                        && SummarizedAction != DataTable.NeededAction.Update)
                                        SummarizedAction = DataTable.NeededAction.NoDifferences;
                                    break;
                                case DataTable.NeededAction.Update:
                                    if (SummarizedAction != DataTable.NeededAction.Error
                                        && SummarizedAction != DataTable.NeededAction.Correction
                                        && SummarizedAction != DataTable.NeededAction.Insert)
                                        SummarizedAction = DataTable.NeededAction.Update;
                                    break;
                            }
                        }
                        try
                        {
                            if (ForTesting)
                                ImportTransaction.Rollback("ImportLine");
                            else
                            {
                                if (ErrorOccurred)
                                {
                                    ImportTransaction.Rollback("ImportLine");
                                    DiversityWorkbench.Import.Import._FailedLines.Add(Line);
                                }
                                else
                                    ImportTransaction.Commit();
                            }
                        }
                        catch (System.Exception ex)
                        {
                            ErrorOccurred = true;
                            SummarizedAction = DataTable.NeededAction.Error;
                            if (!ForTesting)
                            {
                                //DiversityWorkbench.Import.Import.NumberOfFailedImportLines++;
                                DiversityWorkbench.Import.Import._FailedLines.Add(Line);
                            }
                        }

                        if (!ForTesting)
                        {
                            if (ErrorOccurred)
                            {
                                WizardInterface.setDataGridLineColor(Line, System.Drawing.Color.Pink);
                            }
                            else
                            {
                                WizardInterface.setDataGridLineColor(Line, DiversityWorkbench.Import.DataTable.ActionColor(SummarizedAction));
                                switch (SummarizedAction)
                                {
                                    case DataTable.NeededAction.Insert:
                                        DiversityWorkbench.Import.Import.NumberOfLinesImported++;
                                        break;
                                    case DataTable.NeededAction.NoData:
                                        DiversityWorkbench.Import.Import.NumberOfLinesEmpty++;
                                        break;
                                    case DataTable.NeededAction.NoDifferences:
                                        DiversityWorkbench.Import.Import.NumberOfLinesNoDifference++;
                                        break;
                                    case DataTable.NeededAction.Update:
                                        DiversityWorkbench.Import.Import.NumberOfLinesUpdated++;
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                    }
                    ImportConnection.Close();
                    ImportConnection.Dispose();
                    if (RecordSQL.Record)
                        RecordSQL.Finish();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return false;
            }
            return true;
        }

        #endregion

        #region Schema file

        public enum SchemaFileSource { SNSB, ZFMK }

        #region Save

        private static string _Target;

        public static string Target
        {
            get { return Import._Target; }
            set { Import._Target = value; }
        }

        public static string SaveSchemaFile(bool IncludeProtocol, bool IncludeDescription, bool IncludeTabes, string FileName)
        {
            System.Xml.XmlWriter W;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();

            // creating the file name
            System.IO.FileInfo FI = new System.IO.FileInfo(DiversityWorkbench.Import.Import.FileName);
            string SchemaFileName = "";
            if (IncludeProtocol || FileName.Length == 0)
            {
                SchemaFileName = DefaultFileName();
                //SchemaFileName = FI.FullName.Substring(0, FI.FullName.Length - FI.Extension.Length);
                //SchemaFileName += "_" + System.DateTime.Now.Year.ToString();
                //if (System.DateTime.Now.Month < 10) SchemaFileName += "0";
                //SchemaFileName += System.DateTime.Now.Month.ToString();
                //if (System.DateTime.Now.Day < 10) SchemaFileName += "0";
                //SchemaFileName += System.DateTime.Now.Day.ToString() + "_";
                //if (System.DateTime.Now.Hour < 10) SchemaFileName += "0";
                //SchemaFileName += System.DateTime.Now.Hour.ToString();
                //if (System.DateTime.Now.Minute < 10) SchemaFileName += "0";
                //SchemaFileName += System.DateTime.Now.Minute.ToString();
                //if (System.DateTime.Now.Second < 10) SchemaFileName += "0";
                //SchemaFileName += System.DateTime.Now.Second.ToString();
                SchemaFileName += ".xml";
            }
            else
                SchemaFileName = FileName;

            settings.Encoding = System.Text.Encoding.UTF8;
            W = System.Xml.XmlWriter.Create(SchemaFileName, settings);
            try
            {
                W.WriteStartDocument();
                W.WriteStartElement("ImportSchedule");
                W.WriteAttributeString("version", "1");
                W.WriteAttributeString("Module", DiversityWorkbench.Settings.ModuleName);
                W.WriteAttributeString("Target", DiversityWorkbench.Import.Import.Target);
                W.WriteAttributeString("DBversion", DiversityWorkbench.Import.Import.DBversion);
                string Version = new Version(System.Windows.Forms.Application.ProductVersion).ToString();
                W.WriteAttributeString("ClientVersion", Version);
                W.WriteElementString("Encoding", DiversityWorkbench.Import.Import.EncodingDisplayText(DiversityWorkbench.Import.Import.Encoding));
                W.WriteElementString("StartLine", DiversityWorkbench.Import.Import.StartLine.ToString());
                W.WriteElementString("EndLine", DiversityWorkbench.Import.Import.EndLine.ToString());
                W.WriteElementString("FirstLineContainsColumnDefinition", DiversityWorkbench.Import.Import.FirstLineContainsColumnDefinition.ToString());
                W.WriteElementString("UseDefaultDuplicateCheck", DiversityWorkbench.Import.Import.UseDefaultDuplicateCheck.ToString());
                W.WriteElementString("AttachmentUseTransformation", DiversityWorkbench.Import.Import.AttachmentUseTransformation.ToString());
                W.WriteElementString("Language", DiversityWorkbench.Import.Import.CurrentLanguage.ToString());
                W.WriteElementString("Separator", DiversityWorkbench.Import.Import.Separator.ToString());
                W.WriteElementString("TranslateReturn", DiversityWorkbench.Import.Import.TranslateReturn.ToString());

                if (DiversityWorkbench.Import.Import.AttachmentColumn != null)
                {
                    W.WriteElementString("AttachmentTableAlias", DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias);
                    W.WriteElementString("AttachmentColumn", DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName);
                }

                if (DiversityWorkbench.Import.Import.SchemaDescription.Length > 0)
                    W.WriteElementString("Description", DiversityWorkbench.Import.Import.SchemaDescription);

                if (IncludeProtocol)
                {
                    W.WriteElementString("Responsible", System.Environment.UserName);
                    W.WriteElementString("Date", System.DateTime.Now.ToLongDateString());
                    W.WriteElementString("Time", System.DateTime.Now.ToLongTimeString());
                    W.WriteElementString("Server", DiversityWorkbench.Settings.DatabaseServer);
                    W.WriteElementString("Database", DiversityWorkbench.Settings.DatabaseName);
                    if (!DiversityWorkbench.Settings.IsTrustedConnection)
                        W.WriteElementString("DatabaseUser", DiversityWorkbench.Settings.DatabaseUser);
                }

                if (IncludeTabes)
                {
                    W.WriteStartElement("Tables");
                    foreach (string T in DiversityWorkbench.Import.Import.TableListForImport)
                    {
                        W.WriteStartElement("Table");
                        W.WriteAttributeString("TableAlias", T);
                        DiversityWorkbench.Import.Import.SaveTableSettings(ref W, T);
                        W.WriteEndElement();//Table
                    }
                    W.WriteEndElement();//Tables
                }

                if (IncludeProtocol)
                {
                    W.WriteStartElement("Protocol");
                    int NumberOfLines = 1 + DiversityWorkbench.Import.Import.EndLine - DiversityWorkbench.Import.Import.StartLine;
                    //int NumberOfImportedLines = NumberOfLines - NumberOfFailedImportLines;
                    W.WriteElementString("NumberOfLines", NumberOfLines.ToString());
                    W.WriteElementString("NumberOfLinesImported", DiversityWorkbench.Import.Import.NumberOfLinesImported.ToString());
                    W.WriteElementString("NumberOfLinesEmpty", DiversityWorkbench.Import.Import.NumberOfLinesEmpty.ToString());
                    W.WriteElementString("NumberOfLinesNoDifference", DiversityWorkbench.Import.Import.NumberOfLinesNoDifference.ToString());
                    W.WriteElementString("NumberOfLinesUpdated", DiversityWorkbench.Import.Import.NumberOfLinesUpdated.ToString());
                    W.WriteElementString("NumberOfFailedImportLines", DiversityWorkbench.Import.Import.NumberOfFailedImportLines.ToString());
                    W.WriteElementString("NumberOfDuplicates", DiversityWorkbench.Import.Import.NumberOfLinesDupliate.ToString());
                    if (DiversityWorkbench.Import.Import.Duplicates.Count > 0)
                    {
                        string Duplicates = "";
                        foreach (string D in DiversityWorkbench.Import.Import.Duplicates)
                        {
                            if (Duplicates.Length > 0) Duplicates += ", ";
                            Duplicates += D;
                        }
                        W.WriteElementString("Duplicates", Duplicates);
                    }
                    //W.WriteElementString("NumberOfImportedLines", NumberOfImportedLines.ToString());
                    //W.WriteElementString("NumberOfFailedLines", NumberOfFailedImportLines.ToString());
                    Import.SaveImportErrors(ref W);
                    W.WriteEndElement();//Protocol
                }

                if (IncludeDescription)
                {
                    W.WriteStartElement("Description");
                    if (DiversityWorkbench.Import.Import.SchemaDescription.Length > 0)
                        W.WriteAttributeString("Text", DiversityWorkbench.Import.Import.SchemaDescription);
                    Import.SaveFileColumns(ref W);
                    Import.SaveInterfaceSettings(ref W);
                    W.WriteEndElement();//Description
                }

                W.WriteEndElement();//ImportSchedule
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
            return SchemaFileName;
        }

        private static void SaveSchemaHaeder(ref System.Xml.XmlWriter W)
        {
        }

        public static string DefaultFileName()
        {
            System.IO.FileInfo FI = new System.IO.FileInfo(DiversityWorkbench.Import.Import.FileName);
            string DefaultFileName = "";
            {
                DefaultFileName = FI.FullName.Substring(0, FI.FullName.Length - FI.Extension.Length);
                DefaultFileName += "_" + System.DateTime.Now.Year.ToString();
                if (System.DateTime.Now.Month < 10) DefaultFileName += "0";
                DefaultFileName += System.DateTime.Now.Month.ToString();
                if (System.DateTime.Now.Day < 10) DefaultFileName += "0";
                DefaultFileName += System.DateTime.Now.Day.ToString() + "_";
                if (System.DateTime.Now.Hour < 10) DefaultFileName += "0";
                DefaultFileName += System.DateTime.Now.Hour.ToString();
                if (System.DateTime.Now.Minute < 10) DefaultFileName += "0";
                DefaultFileName += System.DateTime.Now.Minute.ToString();
                if (System.DateTime.Now.Second < 10) DefaultFileName += "0";
                DefaultFileName += System.DateTime.Now.Second.ToString();
            }
            return DefaultFileName;
        }

        private static void SaveTableSettings(ref System.Xml.XmlWriter W, string TableAlias)
        {
            //W.WriteElementString("TableAlias", TableAlias);
            W.WriteElementString("TableName", DiversityWorkbench.Import.Import.Tables[TableAlias].TableName);
            if (DiversityWorkbench.Import.Import.Tables[TableAlias].ParentTableAlias != null &&
                DiversityWorkbench.Import.Import.Tables[TableAlias].ParentTableAlias.Length > 0)
                W.WriteElementString("ParentTableAlias", DiversityWorkbench.Import.Import.Tables[TableAlias].ParentTableAlias);
            W.WriteElementString("MergeHandling", DiversityWorkbench.Import.Import.Tables[TableAlias].MergeHandling.ToString());
            W.WriteStartElement("Columns");
            if (DiversityWorkbench.Import.Import.Tables[TableAlias].IsForAttachment)
            {
                foreach (string AC in DiversityWorkbench.Import.Import.Tables[TableAlias].AttachmentColumns)
                {
                    if (DiversityWorkbench.Import.Import.Tables[TableAlias].DataColumns[AC].IsSelected)
                    {
                        W.WriteStartElement("Column");
                        W.WriteAttributeString("ColumnName", DiversityWorkbench.Import.Import.Tables[TableAlias].DataColumns[AC].ColumnName);
                        DiversityWorkbench.Import.Import.SaveColumnSettings(ref W, DiversityWorkbench.Import.Import.Tables[TableAlias].DataColumns[AC]);
                        W.WriteEndElement();//Column
                    }
                }
            }
            else
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> DC in DiversityWorkbench.Import.Import.Tables[TableAlias].DataColumns)
                {
                    if (DC.Value.IsSelected)
                    {
                        W.WriteStartElement("Column");
                        W.WriteAttributeString("ColumnName", DC.Key);
                        DiversityWorkbench.Import.Import.SaveColumnSettings(ref W, DC.Value);
                        W.WriteEndElement();//Column
                    }
                }
            }
            W.WriteEndElement();//Columns
        }

        private static void SaveColumnSettings(ref System.Xml.XmlWriter W, DiversityWorkbench.Import.DataColumn DataColumn)
        {
            try
            {
                W.WriteElementString("CompareKey", DataColumn.CompareKey.ToString());
                W.WriteElementString("CopyPrevious", DataColumn.CopyPrevious.ToString());
                if (DataColumn.FileColumn != null)
                    W.WriteElementString("FileColumn", DataColumn.FileColumn.ToString());
                W.WriteElementString("IsDecisive", DataColumn.IsDecisive.ToString());
                if (DataColumn.Prefix != null)
                    W.WriteElementString("Prefix", DataColumn.Prefix.ToString());
                if (DataColumn.Postfix != null)
                    W.WriteElementString("Postfix", DataColumn.Postfix.ToString());
                W.WriteElementString("TypeOfSource", DataColumn.TypeOfSource.ToString());
                if (DataColumn.TypeOfSource == DiversityWorkbench.Import.DataColumn.SourceType.Interface
                    && DataColumn.OriginalValue != null && DataColumn.OriginalValue.Length > 0) // Toni: Save original value supplied in form - adaption to preprocessing
                    W.WriteElementString("Value", DataColumn.OriginalValue.ToString());         // Toni: Save original value supplied in form - adaption to preprocessing 
                if (DataColumn.SelectParallelForeignRelationTableAlias)
                    W.WriteElementString("ForeignRelationTableAlias", DataColumn.ForeignRelationTableAlias);
                if (DataColumn.IsMultiColumn && DataColumn.MultiColumns.Count > 0)
                {
                    W.WriteStartElement("MultiColumns");
                    foreach (DiversityWorkbench.Import.ColumnMulti MC in DataColumn.MultiColumns)
                    {
                        W.WriteStartElement("MultiColumn");
                        DiversityWorkbench.Import.Import.SaveColumnMultiColumnSettings(ref W, MC);
                        W.WriteEndElement();//MultiColumn
                    }
                    W.WriteEndElement();//MultiColumns
                }

                if (DataColumn.Transformations.Count > 0)
                {
                    W.WriteStartElement("Transformations");
                    foreach (DiversityWorkbench.Import.Transformation T in DataColumn.Transformations)
                    {
                        W.WriteStartElement("Transformation");
                        DiversityWorkbench.Import.Import.SaveColumnTransformationSettings(ref W, T);
                        W.WriteEndElement();//Transformation
                    }
                    W.WriteEndElement();//Transformations
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private static void SaveColumnMultiColumnSettings(ref System.Xml.XmlWriter W, DiversityWorkbench.Import.ColumnMulti MultiColumn)
        {
            W.WriteElementString("CopyPrevious", MultiColumn.CopyPrevious.ToString());
            W.WriteElementString("ColumnInFile", MultiColumn.ColumnInFile.ToString());
            W.WriteElementString("IsDecisive", MultiColumn.IsDecisive.ToString());
            W.WriteElementString("Prefix", MultiColumn.Prefix.ToString());
            W.WriteElementString("Postfix", MultiColumn.Postfix.ToString());

            if (MultiColumn.Transformations.Count > 0)
            {
                W.WriteStartElement("Transformations");
                foreach (DiversityWorkbench.Import.Transformation T in MultiColumn.Transformations)
                {
                    W.WriteStartElement("Transformation");
                    DiversityWorkbench.Import.Import.SaveColumnTransformationSettings(ref W, T);
                    W.WriteEndElement();//Transformation
                }
                W.WriteEndElement();//Transformations
            }
        }

        private static void SaveColumnTransformationSettings(ref System.Xml.XmlWriter W, DiversityWorkbench.Import.Transformation T)
        {
            try
            {
                W.WriteAttributeString("TypeOfTransformation", T.TypeOfTransformation.ToString());

                switch (T.TypeOfTransformation)
                {
                    case Transformation.TransformationType.Calculation:
                        if (T.CalulationOperator != null && T.CalulationOperator.Length > 0 &&
                            T.CalculationFactor != null && T.CalculationFactor.Length > 0)
                        {
                            W.WriteElementString("CalulationOperator", T.CalulationOperator.ToString());
                            double CalculationFactor = double.Parse(T.CalculationFactor.Replace(" ", "").ToString());
                            W.WriteElementString("CalculationFactor", CalculationFactor.ToString());
                        }
                        if (T.CalculationConditionOperator != null && T.CalculationConditionValue != null &&
                            T.CalculationConditionOperator.Length > 0 && T.CalculationConditionValue.Length > 0)
                        {
                            W.WriteElementString("CalulationConditionOperator", T.CalculationConditionOperator);
                            double CalculationConditionValue = double.Parse(T.CalculationConditionValue.Replace(" ", ""));
                            W.WriteElementString("CalculationConditionValue", CalculationConditionValue.ToString());
                        }
                        if (T.CalculationApplyOnData && T.CalculationApplyOnDataOperator != null && T.CalculationApplyOnDataOperator.Length > 0)
                        {
                            W.WriteElementString("CalculationApplyOnDataOperator", T.CalculationApplyOnDataOperator.ToString());
                        }
                        break;
                    case Transformation.TransformationType.RegularExpression:
                        W.WriteElementString("RegularExpression", T.RegularExpression.ToString());
                        W.WriteElementString("RegularExpressionReplacement", T.RegularExpressionReplacement.ToString());
                        break;
                    case Transformation.TransformationType.Replacement:
                        W.WriteElementString("Replace", T.Replace.ToString());
                        string ReplaceWith = "";
                        if (T.ReplaceWith != null)
                            ReplaceWith = T.ReplaceWith.ToString();
                        W.WriteElementString("ReplaceWith", ReplaceWith);
                        break;
                    case Transformation.TransformationType.Split:
                        W.WriteElementString("SplitterPosition", T.SplitterPosition.ToString());
                        W.WriteElementString("ReverseSequence", T.ReverseSequence.ToString());
                        W.WriteStartElement("Splitters");
                        foreach (string S in T.SplitterList)
                            W.WriteElementString("Splitter", S);
                        W.WriteEndElement();//Splitters
                        break;
                    case Transformation.TransformationType.Translation:
                        W.WriteStartElement("Translations");
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in T.TranslationDictionary)
                        {
                            W.WriteStartElement("Translation");
                            W.WriteAttributeString("From", KV.Key);
                            W.WriteAttributeString("To", KV.Value);
                            W.WriteEndElement();
                            //W.WriteElementString("Translation", KV.Key);
                        }
                        W.WriteElementString("SourceTable", T.TranslationSourceTable);
                        W.WriteElementString("FromColumn", T.TranslationFromColumn);
                        W.WriteElementString("IntoColumn", T.TranslationIntoColumn);
                        W.WriteEndElement();//Translations
                        break;
                    case Transformation.TransformationType.Filter:
                        W.WriteElementString("FilterUseFixedValue", T.FilterUseFixedValue.ToString());
                        W.WriteElementString("FilterFixedValue", T.FilterFixedValue.ToString());
                        W.WriteElementString("FilterConditionsOperator", T.FilterConditionsOperator.ToString());
                        W.WriteStartElement("FilterConditions");
                        foreach (DiversityWorkbench.Import.TransformationFilter F in T.FilterConditions)
                        {
                            W.WriteStartElement("FilterCondition");
                            W.WriteElementString("Filter", F.Filter);
                            W.WriteElementString("FilterColumn", F.FilterColumn.ToString());
                            W.WriteElementString("FilterOperator", F.FilterOperator.ToString());
                            W.WriteEndElement();//FiterCondition
                        }
                        W.WriteEndElement();//FiterConditions
                        break;
                    case Transformation.TransformationType.Color:
                        W.WriteElementString("ColorFrom", T.ColorFrom.ToString());
                        W.WriteElementString("ColorInto", T.ColorInto.ToString());
                        break;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        public static string ShowConvertedFile(string FileName)
        {
            System.IO.FileInfo xmlFile = new System.IO.FileInfo(FileName);
            System.Xml.Xsl.XslCompiledTransform xslt = new System.Xml.Xsl.XslCompiledTransform();
            // load default style sheet from resources
            System.IO.StringReader xsltReader = new System.IO.StringReader(DiversityWorkbench.Properties.Resources.TransformationSchema);
            System.Xml.XmlReader xml = System.Xml.XmlReader.Create(xsltReader);
            xslt.Load(xml);

            System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(xmlFile.FullName);

            // The output file:
            string outputFile = xmlFile.FullName.Substring(0, xmlFile.FullName.Length
                - xmlFile.Extension.Length) + ".htm";

            // Create the writer.             
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(outputFile, xslt.OutputSettings);

            // Transform the file
            xslt.Transform(doc, writer);
            writer.Close();

            return outputFile;
        }

        private static void SaveImportErrors(ref System.Xml.XmlWriter W)
        {
            // Settings
            W.WriteStartElement("ImportErrors");
            for (int i = DiversityWorkbench.Import.Import.StartLine - 1 /* this._ImportInterface.FirstLineForImport() - 1*/; i < DiversityWorkbench.Import.Import.EndLine /* this._ImportInterface.LastLineForImport()*/; i++)
            {
                bool ErrorFound = false;
                foreach (string s in DiversityWorkbench.Import.Import.TableListForImport)
                {
                    if (DiversityWorkbench.Import.Import.Tables[s].Messages.ContainsKey(i))
                    {
                        ErrorFound = true;
                        break;
                    }
                }
                if (ErrorFound)
                {
                    W.WriteStartElement("ImportError");
                    W.WriteElementString("Line", (i + 1).ToString());
                    foreach (string T in DiversityWorkbench.Import.Import.TableListForImport)
                    {
                        if (DiversityWorkbench.Import.Import.Tables[T].Messages.ContainsKey(i))
                        {
                            W.WriteStartElement("Table");
                            W.WriteElementString("TableAlias", T);
                            W.WriteElementString("TableDisplayText", DiversityWorkbench.Import.Import.Tables[T].DisplayText);
                            W.WriteElementString("Error", DiversityWorkbench.Import.Import.Tables[T].Messages[i]);
                            W.WriteEndElement(); //Table
                        }
                    }
                    W.WriteEndElement(); //ImportError
                }
            }
            W.WriteEndElement(); //ImportErrors
        }

        private void SaveImportSteps(ref System.Xml.XmlWriter W)
        {
            // Steps
            W.WriteStartElement("Steps");
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.Step> IS in DiversityWorkbench.Import.Import.Steps)
            {
                if (IS.Value.IsSelected)
                {
                    W.WriteStartElement("ImportStep");
                    W.WriteElementString("Key", IS.Key);
                    W.WriteElementString("Title", IS.Value.DisplayText.ToString());
                    W.WriteEndElement(); //Step
                }
            }
            W.WriteEndElement(); //Steps
        }

        #endregion

        #region Description

        private static string _SchemaDescription;

        public static string SchemaDescription
        {
            get
            {
                if (_SchemaDescription == null)
                    _SchemaDescription = "";
                return Import._SchemaDescription;
            }
            set { Import._SchemaDescription = value; }
        }

        public static void SaveFileColumns(ref System.Xml.XmlWriter W)
        {
            System.Collections.Generic.SortedDictionary<int, System.Collections.Generic.List<DiversityWorkbench.Import.iDataColumn>> FileColumns = new SortedDictionary<int, List<iDataColumn>>();
            foreach (string T in DiversityWorkbench.Import.Import.TableListForImport)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in DiversityWorkbench.Import.Import.Tables[T].DataColumns)
                {
                    if (KV.Value.PositionInFile() != null)
                    {
                        int i = (int)KV.Value.PositionInFile();
                        if (FileColumns.ContainsKey(i))
                            FileColumns[i].Add(KV.Value);
                        else
                        {
                            System.Collections.Generic.List<DiversityWorkbench.Import.iDataColumn> Columns = new List<iDataColumn>();
                            Columns.Add(KV.Value);
                            FileColumns.Add(i, Columns);
                        }
                    }
                    if (KV.Value.MultiColumns.Count > 0)
                    {
                        foreach (DiversityWorkbench.Import.ColumnMulti MC in KV.Value.MultiColumns)
                        {
                            if (FileColumns.ContainsKey(MC.ColumnInFile))
                                FileColumns[MC.ColumnInFile].Add(MC);
                            else
                            {
                                System.Collections.Generic.List<DiversityWorkbench.Import.iDataColumn> Columns = new List<iDataColumn>();
                                Columns.Add(MC);
                                FileColumns.Add(MC.ColumnInFile, Columns);
                            }
                        }
                    }
                }
            }
            W.WriteStartElement("FileColumns");
            int iMax = FileExample.Keys.Last();// FileColumns.Keys.Last();
            for (int i = 0; i <= iMax; i++)
            {
                if (!FileColumns.ContainsKey(i))
                {
                    System.Collections.Generic.List<DiversityWorkbench.Import.iDataColumn> Columns = new List<iDataColumn>();
                    FileColumns.Add(i, Columns);
                }
            }
            //string Space = "˽";

            foreach (System.Collections.Generic.KeyValuePair<int, System.Collections.Generic.List<DiversityWorkbench.Import.iDataColumn>> KV in FileColumns)
            {
                W.WriteStartElement("FileColumn");
                W.WriteAttributeString("Position", KV.Key.ToString());
                if (FileColumnHeader.ContainsKey(KV.Key))
                    W.WriteAttributeString("ColumnHeader", FileColumnHeader[KV.Key]);
                if (KV.Value.Count == 0)
                    W.WriteAttributeString("IsImported", "false");
                else
                {
                    W.WriteAttributeString("IsImported", "true");
                    W.WriteStartElement("Columns");
                    foreach (DiversityWorkbench.Import.iDataColumn iDC in KV.Value)
                    {
                        if (iDC.GetType() == typeof(DiversityWorkbench.Import.ColumnMulti))
                        {
                            DiversityWorkbench.Import.ColumnMulti DC = (DiversityWorkbench.Import.ColumnMulti)iDC;
                            W.WriteStartElement("Column");
                            W.WriteAttributeString("Table", DC.DataColumn.DataTable.TableName);
                            W.WriteAttributeString("TableAlias", DC.DataColumn.DataTable.TableAlias);
                            W.WriteAttributeString("MergeHandling", DC.DataColumn.DataTable.MergeHandling.ToString());
                            W.WriteAttributeString("Name", DC.DataColumn.ColumnName);
                            W.WriteAttributeString("IsMultiColumn", "True");
                            if (DC.Prefix != null && DC.Prefix.Length > 0)
                                W.WriteElementString("Prefix", Import.ReplaceSpaceWithBox(DC.Prefix));
                            if (DC.Postfix != null && DC.Postfix.Length > 0)
                                W.WriteElementString("Postfix", Import.ReplaceSpaceWithBox(DC.Postfix));
                            if (DC.IsDecisive)
                                W.WriteElementString("IsDecisive", "");
                            if (DC.DataColumn.CompareKey)
                                W.WriteElementString("CompareKey", "");
                            if (DC.Transformations.Count > 0)
                            {
                                W.WriteStartElement("Transformations");
                                foreach (DiversityWorkbench.Import.Transformation T in DC.Transformations)
                                {
                                    W.WriteStartElement("Transformation");
                                    DiversityWorkbench.Import.Import.SaveColumnTransformationSettings(ref W, T);
                                    W.WriteEndElement();//Transformation
                                }
                                W.WriteEndElement();//Transformations
                            }
                            if (FileExample.ContainsKey(KV.Key))
                            {
                                string OriginalValue = FileExample[KV.Key];
                                W.WriteElementString("Value", FileExample[KV.Key]);
                                DC.DataColumn.Value = FileExample[KV.Key];
                                string TransformedValue = DiversityWorkbench.Import.DataColumn.TransformedValue(OriginalValue, DC.Transformations, FileExample);// DC.Value;
                                if (TransformedValue.Length > 0)
                                {
                                    if (DC.Prefix != null && DC.Prefix.Length > 0)
                                        TransformedValue = DC.Prefix + TransformedValue;
                                    if (DC.Postfix != null && DC.Postfix.Length > 0)
                                        TransformedValue += DC.Postfix;
                                }
                                if (OriginalValue != TransformedValue)
                                    W.WriteElementString("TransformedValue", Import.ReplaceSpaceWithBox(TransformedValue));
                            }
                        }
                        else if (iDC.GetType() == typeof(DiversityWorkbench.Import.DataColumn))
                        {
                            DiversityWorkbench.Import.DataColumn DC = (DiversityWorkbench.Import.DataColumn)iDC;
                            if (!DC.IsSelected)
                                continue;
                            W.WriteStartElement("Column");
                            W.WriteAttributeString("Table", DC.DataTable.TableName);
                            W.WriteAttributeString("TableAlias", DC.DataTable.TableAlias);
                            W.WriteAttributeString("MergeHandling", DC.DataTable.MergeHandling.ToString());
                            W.WriteAttributeString("Name", DC.ColumnName);
                            W.WriteAttributeString("IsMultiColumn", "False");
                            if (DC.Prefix != null && DC.Prefix.Length > 0)
                                W.WriteElementString("Prefix", Import.ReplaceSpaceWithBox(DC.Prefix));
                            if (DC.Postfix != null && DC.Postfix.Length > 0)
                                W.WriteElementString("Postfix", Import.ReplaceSpaceWithBox(DC.Postfix));
                            if (DC.IsDecisive)
                                W.WriteElementString("IsDecisive", "");
                            if (DC.CompareKey)
                                W.WriteElementString("CompareKey", "");
                            if (DC.Transformations.Count > 0)
                            {
                                W.WriteStartElement("Transformations");
                                foreach (DiversityWorkbench.Import.Transformation T in DC.Transformations)
                                {
                                    W.WriteStartElement("Transformation");
                                    DiversityWorkbench.Import.Import.SaveColumnTransformationSettings(ref W, T);
                                    W.WriteEndElement();//Transformation
                                }
                                W.WriteEndElement();//Transformations
                            }
                            if (FileExample.ContainsKey(KV.Key))
                            {
                                string OriginalValue = FileExample[KV.Key];
                                W.WriteElementString("Value", Import.ReplaceSpaceWithBox(FileExample[KV.Key]));
                                if (DC.Transformations.Count > 0)
                                {
                                    string TransformedValue = DiversityWorkbench.Import.DataColumn.TransformedValue(OriginalValue, DC.Transformations, FileExample);
                                    if (TransformedValue.Length > 0)
                                    {
                                        if (DC.Prefix != null && DC.Prefix.Length > 0)
                                            TransformedValue = DC.Prefix + TransformedValue;
                                        if (DC.Postfix != null && DC.Postfix.Length > 0)
                                            TransformedValue += DC.Postfix;
                                    }
                                    if (OriginalValue != TransformedValue)
                                        W.WriteElementString("TransformedValue", Import.ReplaceSpaceWithBox(TransformedValue));
                                }
                            }
                        }
                        W.WriteEndElement();//Column
                    }
                    W.WriteEndElement();//Columns
                }
                W.WriteEndElement();//Filecolumn
            }
            W.WriteEndElement();//Filecolumns
        }

        private static string ReplaceSpaceWithBox(string Value)
        {
            string Space = "˽";
            if (Value.StartsWith(" "))
                Value = Space + Value.Substring(1);
            if (Value.EndsWith(" "))
                Value = Value.Substring(0, Value.Length - 1) + Space;
            return Value;
        }

        public static void SaveInterfaceSettings(ref System.Xml.XmlWriter W)
        {
            System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<DiversityWorkbench.Import.DataColumn>> InterfaceSettings = new SortedDictionary<string, System.Collections.Generic.List<DiversityWorkbench.Import.DataColumn>>();
            foreach (string T in DiversityWorkbench.Import.Import.TableListForImport)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in DiversityWorkbench.Import.Import.Tables[T].DataColumns)
                {
                    if (KV.Value.TypeOfSource == DataColumn.SourceType.Interface)
                    {
                        if (InterfaceSettings.ContainsKey(KV.Value.DataTable.TableName))
                        {
                            InterfaceSettings[KV.Value.DataTable.TableName].Add(KV.Value);
                        }
                        else
                        {
                            System.Collections.Generic.List<DiversityWorkbench.Import.DataColumn> CC = new List<DataColumn>();
                            CC.Add(KV.Value);
                            InterfaceSettings.Add(KV.Value.DataTable.TableName, CC);
                        }
                    }
                }
            }
            if (InterfaceSettings.Count > 0)
            {
                W.WriteStartElement("InterfaceSettings");
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<DiversityWorkbench.Import.DataColumn>> KV in InterfaceSettings)
                {
                    W.WriteStartElement("Table");
                    foreach (DiversityWorkbench.Import.DataColumn DC in KV.Value)
                    {
                        W.WriteStartElement("Column");
                        W.WriteAttributeString("Table", DC.DataTable.TableName);
                        W.WriteAttributeString("TableAlias", DC.DataTable.TableAlias);
                        W.WriteAttributeString("Name", DC.ColumnName);
                        if (DC.TransformedValue().Length > 0)// KV.Value.TransformedValue().Length > 0)
                        {
                            W.WriteElementString("Value", DC.TransformedValue());// KV.Value.TransformedValue());
                            if (DC.SqlDataSource != null && DC.SqlDataSource.Length > 0)
                            {
                                try
                                {
                                    string SQL = DC.SqlDataSource;
                                    if (SQL.ToUpper().IndexOf(" ORDER BY ") > -1)
                                    {
                                        SQL = SQL.Substring(0, SQL.ToUpper().IndexOf(" ORDER BY "));
                                    }
                                    //SQL += " WHERE " + DC.MandatoryListValueColumn + " = '" + DC.Value + "'";
                                    System.Data.DataTable dt = new System.Data.DataTable();
                                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                                    ad.Fill(dt);
                                    System.Data.DataRow[] rr = dt.Select(DC.MandatoryListValueColumn + " = '" + DC.Value + "'");
                                    if (rr.Length > 0)
                                    {
                                        string Result = rr[0][DC.MandatoryListDisplayColumn].ToString();
                                        if (Result.Length > 0)
                                            W.WriteElementString("Display", Result);
                                    }
                                    //string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                                }
                                catch (System.Exception ex)
                                {
                                }
                            }
                        }
                        W.WriteEndElement();//Column
                    }
                    W.WriteEndElement();//Table
                }
                W.WriteEndElement();//InterfaceSettings
            }
        }

        private static System.Collections.Generic.Dictionary<int, string> _FileColumnHeader;

        public static System.Collections.Generic.Dictionary<int, string> FileColumnHeader
        {
            get
            {
                if (Import._FileColumnHeader == null)
                {
                    Import._FileColumnHeader = new Dictionary<int, string>();
                    if (Import.FirstLineContainsColumnDefinition)
                    {
                        try
                        {
                            string Header = "";
                            System.IO.StreamReader sr = DiversityWorkbench.Import.Import.StreamReader(Import._FileName, Encoding);
                            Header = sr.ReadLine();
                            sr.Close();
                            sr.Dispose();
                            string[] Headers = Header.Split(new char[] { '\t' });
                            for (int i = 0; i < Headers.Length; i++)
                            {
                                Import._FileColumnHeader.Add(i, Headers[i]);
                            }
                        }
                        catch (System.Exception ex)
                        {
                        }
                    }
                }
                return Import._FileColumnHeader;
            }
            set { Import._FileColumnHeader = value; }
        }

        private static System.Collections.Generic.Dictionary<int, string> _FileExample;

        public static System.Collections.Generic.Dictionary<int, string> FileExample
        {
            get
            {
                if (Import._FileExample == null)
                {
                    Import._FileExample = new Dictionary<int, string>();
                    try
                    {
                        string Example = "";
                        System.IO.StreamReader sr = DiversityWorkbench.Import.Import.StreamReader(Import._FileName, Encoding);
                        for (int i = 0; i < Import._StartLine; i++)
                            Example = sr.ReadLine();
                        //if (Import.FirstLineContainsColumnDefinition)
                        //    Example = sr.ReadLine();
                        sr.Close();
                        sr.Dispose();
                        string[] Values = Example.Split(new char[] { '\t' });
                        for (int i = 0; i < Values.Length; i++)
                        {
                            Import._FileExample.Add(i, Values[i]);
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
                return Import._FileExample;
            }
            set { Import._FileExample = value; }
        }

        public static string ShowConvertedDescriptionFile(string FileName)
        {
            System.IO.FileInfo xmlFile = new System.IO.FileInfo(FileName);
            System.Xml.Xsl.XslCompiledTransform xslt = new System.Xml.Xsl.XslCompiledTransform();
            // load default style sheet from resources
            System.IO.StringReader xsltReader = new System.IO.StringReader(DiversityWorkbench.Properties.Resources.TransformationSchema);
            System.Xml.XmlReader xml = System.Xml.XmlReader.Create(xsltReader);
            xslt.Load(xml);

            System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(xmlFile.FullName);

            // The output file:
            string outputFile = xmlFile.FullName.Substring(0, xmlFile.FullName.Length
                - xmlFile.Extension.Length) + ".htm";

            // Create the writer.             
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(outputFile, xslt.OutputSettings);

            // Transform the file
            xslt.Transform(doc, writer);
            writer.Close();

            return outputFile;
        }

        public static void ResetSchemaDescription()
        {
            _FileExample = null;
            _FileColumnHeader = null;
        }

        #endregion

        #region Load

        private static string _SchemaModule;

        public static string SchemaModule
        {
            get { return Import._SchemaModule; }
            //set { Import._SchemaModule = value; }
        }
        private static string _SchemaTarget;

        public static string SchemaTarget
        {
            get { return Import._SchemaTarget; }
            //set { Import._SchemaTarget = value; }
        }

        private static string _DBversion;

        public static string DBversion
        {
            get { return Import._DBversion; }
            set { Import._DBversion = value; }
        }

        /// <summary>
        /// Loading a Schema for the import
        /// </summary>
        /// <param name="FileName">The name of the XML file</param>
        /// <returns></returns>
        public static bool LoadSchemaFile(string FileName)
        {
            DiversityWorkbench.Import.Import.Reset();

            // regenerate the steps and tables by a call of the steps
            int i = DiversityWorkbench.Import.Import.Steps.Count;

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
                    if (R.Name == "ImportScheduleDescription")
                    {
                        System.Windows.Forms.MessageBox.Show("This file is for description only, not for import");
                        return false;
                    }
                    if (R.Name == "ImportSchedule")
                    {
                        try
                        {
                            if (R.NodeType == System.Xml.XmlNodeType.Element)
                            {
                                string Vers = R.GetAttribute("version");
                                int.TryParse(Vers, out iVersion);
                                DiversityWorkbench.Import.Import._SchemaModule = R.GetAttribute("Module");
                                DiversityWorkbench.Import.Import._SchemaTarget = R.GetAttribute("Target");
                                DiversityWorkbench.Import.Import._DBversion = R.GetAttribute("DBversion");
                                if (DiversityWorkbench.Import.Import._SchemaModule != DiversityWorkbench.Settings.ModuleName ||
                                    DiversityWorkbench.Import.Import._SchemaTarget != DiversityWorkbench.Import.Import.Target)
                                {
                                    System.Windows.Forms.MessageBox.Show("The selected schema does not fit the current import");
                                    return false;
                                }
                                else
                                {
                                    SchemaVersion = iVersion;
                                    DiversityWorkbench.Import.Import.SchemaName = FileName;
                                    break;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            SchemaVersion = 0;
                            return false;
                        }
                        continue;
                    }
                }
                if (!LoadSettings(ref R))
                {
                    //System.Windows.Forms.MessageBox.Show("File does not contain valid definitions or is no valid XML document");
                    return false;
                }
                if (!LoadTables(ref R))
                {
                    System.Windows.Forms.MessageBox.Show("The selected schema contains no valid table definition");
                }
                if (!DiversityWorkbench.Import.Import.LoadFailedAttachmentSettings())
                {
                    return false;
                }
                LoadDescription(ref R);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                R.ResetState();
                R.Close();
            }
            return true;
        }

        #region Settings

        private static bool LoadSettings(ref System.Xml.XmlTextReader R)
        {
            bool OK = true;
            try
            {
                string AttachmentTableAlias = "";
                DiversityWorkbench.Import.Import._FailedAttachementColumns = null;
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.Name.Length > 0 &&
                        R.Name != "Encoding" &&
                        R.Name != "StartLine" &&
                        R.Name != "EndLine" &&
                        R.Name != "FirstLineContainsColumnDefinition" &&
                        R.Name != "UseDefaultDuplicateCheck" &&
                        R.Name != "AttachmentUseTransformation" &&
                        R.Name != "Language" &&
                        R.Name != "AttachmentTableAlias" &&
                        R.Name != "AttachmentColumn" &&
                        R.Name != "TranslateReturn" &&
                        R.Name != "Separator")
                        return OK;
                    if (R.IsStartElement())
                    {
                        switch (R.Name)
                        {
                            case "Encoding":
                                R.Read();
                                DiversityWorkbench.Import.Import.Encoding = DiversityWorkbench.Import.Import.Encodings[R.Value];
                                break;
                            case "StartLine":
                                R.Read();
                                DiversityWorkbench.Import.Import.StartLine = int.Parse(R.Value);
                                break;
                            case "EndLine":
                                R.Read();
                                DiversityWorkbench.Import.Import.EndLine = int.Parse(R.Value);
                                break;
                            case "FirstLineContainsColumnDefinition":
                                R.Read();
                                DiversityWorkbench.Import.Import.FirstLineContainsColumnDefinition = bool.Parse(R.Value);
                                break;
                            case "UseDefaultDuplicateCheck":
                                R.Read();
                                DiversityWorkbench.Import.Import.UseDefaultDuplicateCheck = bool.Parse(R.Value);
                                break;
                            case "AttachmentUseTransformation":
                                R.Read();
                                DiversityWorkbench.Import.Import.AttachmentUseTransformation = bool.Parse(R.Value);
                                break;
                            case "Language":
                                R.Read();
                                string Language = R.Value;
                                if (Language == DiversityWorkbench.Import.Import.Langage.de.ToString())
                                    DiversityWorkbench.Import.Import.CurrentLanguage = Langage.de;
                                else if (Language == DiversityWorkbench.Import.Import.Langage.US.ToString())
                                    DiversityWorkbench.Import.Import.CurrentLanguage = Langage.US;
                                else if (Language == DiversityWorkbench.Import.Import.Langage.sp.ToString())
                                    DiversityWorkbench.Import.Import.CurrentLanguage = Langage.sp;
                                else if (Language == DiversityWorkbench.Import.Import.Langage.it.ToString())
                                    DiversityWorkbench.Import.Import.CurrentLanguage = Langage.it;
                                else if (Language == DiversityWorkbench.Import.Import.Langage.fr.ToString())
                                    DiversityWorkbench.Import.Import.CurrentLanguage = Langage.fr;
                                else if (Language == DiversityWorkbench.Import.Import.Langage.IVL.ToString())
                                    DiversityWorkbench.Import.Import.CurrentLanguage = Langage.IVL;
                                break;
                            case "AttachmentTableAlias":
                                R.Read();
                                AttachmentTableAlias = R.Value;
                                break;
                            case "AttachmentColumn":
                                R.Read();
                                if (AttachmentTableAlias.Length > 0)
                                {
                                    if (DiversityWorkbench.Import.Import.Tables.ContainsKey(AttachmentTableAlias))
                                        DiversityWorkbench.Import.Import.AttachmentColumn = DiversityWorkbench.Import.Import.Tables[AttachmentTableAlias].DataColumns[R.Value];
                                    else
                                    {
                                        DiversityWorkbench.Import.Import.AddFailedAttachmentColumn(AttachmentTableAlias, R.Value);
                                    }
                                }
                                break;
                            case "Separator":
                                R.Read();
                                switch (R.Value)
                                {
                                    case "SEMICOLON":
                                        DiversityWorkbench.Import.Import.Separator = separator.SEMICOLON;
                                        break;
                                    default:
                                        DiversityWorkbench.Import.Import.Separator = separator.TAB;
                                        break;
                                }
                                break;
                            case "TranslateReturn":
                                R.Read();
                                TranslateReturn = bool.Parse(R.Value.ToString());
                                break;
                        }
                    }
                }
            }
            catch (System.Exception ex) { OK = false; }
            return OK;
        }

        private static System.Collections.Generic.Dictionary<string, string> _FailedAttachementColumns;
        private static void AddFailedAttachmentColumn(string TableAlias, string ColumnName)
        {
            if (DiversityWorkbench.Import.Import._FailedAttachementColumns == null)
                DiversityWorkbench.Import.Import._FailedAttachementColumns = new Dictionary<string, string>();
            DiversityWorkbench.Import.Import._FailedAttachementColumns.Add(TableAlias, ColumnName);
        }

        private static bool LoadFailedAttachmentSettings()
        {
            bool OK = true;
            try
            {
                if (DiversityWorkbench.Import.Import.AttachmentColumn != null && DiversityWorkbench.Import.Import._FailedAttachementColumns != null)
                    return false;
                if (DiversityWorkbench.Import.Import._FailedAttachementColumns != null)
                {
                    if (DiversityWorkbench.Import.Import._FailedAttachementColumns.Count > 1)
                        return false;
                    if (DiversityWorkbench.Import.Import._FailedAttachementColumns.Count == 1)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DiversityWorkbench.Import.Import._FailedAttachementColumns)
                        {
                            DiversityWorkbench.Import.Import.AttachmentColumn = DiversityWorkbench.Import.Import.Tables[KV.Key].DataColumns[KV.Value];
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        #endregion

        #region Tables

        private static bool LoadTables(ref System.Xml.XmlTextReader R)
        {
            bool OK = false;
            try
            {
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.Name == "Table" && R.IsStartElement())
                    {
                        OK = LoadTable(ref R);
                    }
                    // Toni 23.02.2016: Terminate loading at end of tables
                    if (R.Name == "Tables" && !R.IsStartElement())// R.Name == "Description" && R.AttributeCount > 0 && R.NodeType ==
                    {
                        return OK;
                    }
                }
            }
            catch (System.Exception ex) { OK = false; }
            return OK;
        }

        private static bool LoadTable(ref System.Xml.XmlTextReader R)
        {
            bool OK = false;
            try
            {
                string TableAlias = "";
                if (R.AttributeCount > 0)
                    TableAlias = R.GetAttribute("TableAlias");
                if (!DiversityWorkbench.Import.Import.Tables.ContainsKey(TableAlias))
                {
                    string TemplateTableAlias = "";
                    string[] NameParts = TableAlias.Split(new char[] { '_' });
                    int ParallelPosition = int.Parse(NameParts.Last());
                    for (int i = 0; i < NameParts.Length - 1; i++)
                    {
                        TemplateTableAlias += NameParts[i] + "_";
                    }
                    TemplateTableAlias += "1";
                    DiversityWorkbench.Import.Import.Steps[DiversityWorkbench.Import.Import.Tables[TemplateTableAlias].PositionKey].CopyStep();
                }

                if (DiversityWorkbench.Import.Import.Steps.ContainsKey(DiversityWorkbench.Import.Import.Tables[TableAlias].PositionKey))
                {
                    DiversityWorkbench.Import.Import.Steps[DiversityWorkbench.Import.Import.Tables[TableAlias].PositionKey].IsSelected = true;
                    if (!DiversityWorkbench.Import.Import.TableListForImport.Contains(TableAlias))
                        DiversityWorkbench.Import.Import.TableListForImport.Add(TableAlias);
                }
                else
                    System.Windows.Forms.MessageBox.Show("The table " + TableAlias + " is missing in the list of tables");
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Element && R.Name == "MergeHandling")
                    {
                        R.Read();
                        string MergeHandling = R.Value;
                        switch (MergeHandling)
                        {
                            case "Update":
                                DiversityWorkbench.Import.Import.Tables[TableAlias].MergeHandling = DataTable.Merging.Update;
                                break;
                            case "Insert":
                                DiversityWorkbench.Import.Import.Tables[TableAlias].MergeHandling = DataTable.Merging.Insert;
                                break;
                            case "Merge":
                                DiversityWorkbench.Import.Import.Tables[TableAlias].MergeHandling = DataTable.Merging.Merge;
                                break;
                            case "Attach":
                                DiversityWorkbench.Import.Import.Tables[TableAlias].MergeHandling = DataTable.Merging.Attach;
                                break;
                        }
                    }
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.NodeType == System.Xml.XmlNodeType.EndElement && R.Name == "Table")
                        return OK;
                    if (R.IsStartElement() && R.Name == "Columns")
                    {
                        System.Collections.Generic.List<string> Columns = LoadColumns(ref R, DiversityWorkbench.Import.Import.Tables[TableAlias]);
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in DiversityWorkbench.Import.Import.Tables[TableAlias].DataColumns)
                        {
                            if (!Columns.Contains(KV.Key))
                            {
                                if (KV.Value.IsSelected)
                                    KV.Value.IsSelected = false;
                            }
                        }
                    }
                    OK = true;
                }
            }
            catch (System.Exception ex) { OK = false; }
            return OK;
        }

        #endregion

        #region Columns

        private static System.Collections.Generic.List<string> LoadColumns(ref System.Xml.XmlTextReader R, DiversityWorkbench.Import.DataTable DT)
        {
            System.Collections.Generic.List<string> ColumnList = new List<string>();
            try
            {
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.Name == "Columns" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                        return ColumnList;
                    if (R.IsStartElement() && R.Name == "Column")
                    {
                        try
                        {
                            string ColumnName = R.GetAttribute("ColumnName");
                            if (DT.DataColumns.ContainsKey(ColumnName))
                            {
                                LoadColumn(ref R, DT.DataColumns[ColumnName]);
                                ColumnList.Add(ColumnName);
                            }
                            else
                            {
                                string Message = "In the table " + DT.TableAlias + " (= " + DT.TableName + ") the column " + ColumnName + " could not be found.\r\n"
                                    + "The schema may have been created depending on a different version of the database";
                                System.Windows.Forms.MessageBox.Show(Message);
                            }
                        }
                        catch (System.Exception ex) { }
                    }
                }
            }
            catch (System.Exception ex) { }
            return ColumnList;
        }

        private static void LoadColumn(ref System.Xml.XmlTextReader R, DiversityWorkbench.Import.DataColumn DC)
        {
            string _CurrentNode = "";
            DC.IsSelected = true;
            try
            {
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.Name == "Column" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                        return;
                    if (R.IsStartElement() && R.Name.Length > 0)
                    {
                        string Node = R.Name;
                        if (!R.IsEmptyElement)
                            R.Read();
                        switch (Node)
                        {
                            case "CompareKey":
                                DC.CompareKey = bool.Parse(R.Value);
                                break;
                            case "CopyPrevious":
                                DC.CopyPrevious = bool.Parse(R.Value);
                                break;
                            case "IsDecisive":
                                DC.IsDecisive = bool.Parse(R.Value);
                                break;
                            case "TypeOfSource":
                                switch (R.Value)
                                {
                                    case "Database":
                                        DC.TypeOfSource = DataColumn.SourceType.Database;
                                        break;
                                    case "File":
                                        DC.TypeOfSource = DataColumn.SourceType.File;
                                        break;
                                    case "NotDecided":
                                        DC.TypeOfSource = DataColumn.SourceType.NotDecided;
                                        break;
                                    case "Interface":
                                        DC.TypeOfSource = DataColumn.SourceType.Interface;
                                        break;
                                    case "Preset":
                                        DC.TypeOfSource = DataColumn.SourceType.Preset;
                                        break;
                                    case "ParentTable":
                                        DC.TypeOfSource = DataColumn.SourceType.ParentTable;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case "FileColumn":
                                DC.FileColumn = int.Parse(R.Value);
                                DC.TypeOfSource = DataColumn.SourceType.File;
                                break;
                            case "Prefix":
                                if (R.NodeType != System.Xml.XmlNodeType.Whitespace)
                                    DC.Prefix = R.Value;
                                else
                                {
                                    string P = R.Value;
                                    if (P.Replace(" ", "").Length == 0)
                                        DC.Prefix = P;
                                }
                                break;
                            case "Postfix":
                                if (R.NodeType != System.Xml.XmlNodeType.Whitespace)
                                    DC.Postfix = R.Value;
                                else
                                {
                                    string P = R.Value;
                                    if (P.Replace(" ", "").Length == 0)
                                        DC.Postfix = P;
                                }
                                break;
                            case "ForeignRelationTableAlias":
                                DC.ForeignRelationTableAlias = R.Value;
                                break;
                            case "MultiColumns":
                                DiversityWorkbench.Import.Import.LoadMultiColumns(ref R, ref DC);
                                break;
                            case "Transformations":
                                DiversityWorkbench.Import.Import.LoadColumnTransformations(ref R, DC);
                                break;
                            case "Value":
                                DC.Value = R.Value;
                                break;
                        }
                    }
                }
            }
            catch (System.Exception ex) { }
            return;
        }

        private static void LoadMultiColumns(ref System.Xml.XmlTextReader R, ref DiversityWorkbench.Import.DataColumn IC)
        {
            try
            {
                if (R.IsStartElement() && R.Name == "MultiColumn")
                {
                    try
                    {
                        DiversityWorkbench.Import.Import.LoadMultiColumn(ref R, ref IC);
                    }
                    catch (System.Exception ex) { }
                }
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.Name == "MultiColumns" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                        return;
                    if (R.IsStartElement() && R.Name == "MultiColumn")
                    {
                        try
                        {
                            DiversityWorkbench.Import.Import.LoadMultiColumn(ref R, ref IC);
                        }
                        catch (System.Exception ex) { }
                    }
                }
            }
            catch (System.Exception ex) { }
            return;
        }

        private static void LoadMultiColumn(ref System.Xml.XmlTextReader R, ref DiversityWorkbench.Import.DataColumn DC)
        {
            try
            {
                DiversityWorkbench.Import.ColumnMulti MC = new ColumnMulti(DiversityWorkbench.Import.Import.Tables[DC.DataTable.TableAlias].DataColumns[DC.ColumnName], DiversityWorkbench.Import.Import.Tables[DC.DataTable.TableAlias].DataColumns[DC.ColumnName].iDataColumnInterface, -1);
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.IsEmptyElement)
                        continue;
                    if (R.Name == "MultiColumn" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                        return;
                    if (R.NodeType == System.Xml.XmlNodeType.Element && R.Name.Length > 0)
                    {
                        if (R.Name == "MultiColumn" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                            return;
                        string Node = R.Name;
                        R.Read();
                        switch (Node)
                        {
                            case "CopyPrevious":
                                MC.CopyPrevious = bool.Parse(R.Value);
                                break;
                            case "IsDecisive":
                                MC.IsDecisive = bool.Parse(R.Value);
                                break;
                            case "ColumnInFile":
                                MC.ColumnInFile = int.Parse(R.Value);
                                break;
                            case "Prefix":
                                MC.Prefix = R.Value;
                                break;
                            case "Postfix":
                                MC.Postfix = R.Value;
                                break;
                            case "Transformations":
                                DiversityWorkbench.Import.Import.LoadColumnTransformations(ref R, MC);
                                break;
                        }
                    }
                }
            }
            catch (System.Exception ex) { }
            return;
        }

        private static void LoadColumnTransformations(ref System.Xml.XmlTextReader R, DiversityWorkbench.Import.iDataColumn iDC)
        {
            try
            {
                if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                    R.Read();
                string TypeOfTransformation = R.GetAttribute(0);
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.EndElement && R.Name == "Transformations")
                        return;
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.NodeType == System.Xml.XmlNodeType.EndElement)
                        continue;
                    if (R.IsStartElement())
                    {
                        if (R.Name == "Transformation")
                        {
                            //R.Read();
                            if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                R.Read();
                            TypeOfTransformation = R.GetAttribute(0);
                        }
                        switch (TypeOfTransformation)
                        {
                            case "Replacement":
                                if (R.Name == "Transformation")
                                    R.Read();
                                if (R.IsStartElement() && R.Name == "Replace")
                                    R.Read();
                                string Replace = R.Value;
                                while (!R.IsStartElement() && R.Name != "ReplaceWith")
                                    R.Read();
                                R.Read();
                                string ReplaceWith = R.Value;
                                DiversityWorkbench.Import.Transformation Trepl = new Transformation(iDC, Transformation.TransformationType.Replacement);
                                Trepl.Replace = Replace;
                                Trepl.ReplaceWith = ReplaceWith;
                                break;
                            case "Calculation":
                                if (R.Name == "Transformation")
                                    R.Read();
                                if (R.IsStartElement() && R.Name == "Calculation")
                                    R.Read();
                                if (R.IsStartElement() && R.Name == "CalulationOperator")
                                    R.Read();
                                string CalulationOperator = R.Value;
                                string CalculationFactor = "";
                                string CalulationConditionOperator = "";
                                string CalculationConditionValue = "";
                                string CalculationApplyOnDataOperator = "";
                                while (!R.IsStartElement() && (R.Name != "CalculationFactor" || R.Name != "CalculationApplyOnDataOperator"))
                                    R.Read();
                                if (R.Name == "CalculationFactor")
                                {
                                    R.Read();
                                    CalculationFactor = R.Value;
                                    R.Read();
                                    R.Read();
                                    if (R.Name == "CalulationConditionOperator")
                                    {
                                        R.Read();
                                        CalulationConditionOperator = R.Value;
                                        R.Read();
                                        R.Read();
                                        R.Read();
                                        CalculationConditionValue = R.Value;
                                    }
                                }
                                if (R.Name == "CalculationApplyOnDataOperator")
                                {
                                    R.Read();
                                    CalculationApplyOnDataOperator = R.Value;
                                }
                                //if (R.NodeType == System.Xml.XmlNodeType.EndElement && R.Name == "Transformation")
                                //{
                                DiversityWorkbench.Import.Transformation Tcalc = new Transformation(iDC, Transformation.TransformationType.Calculation);
                                Tcalc.CalulationOperator = CalulationOperator;
                                Tcalc.CalculationFactor = CalculationFactor;
                                if (CalulationConditionOperator.Length > 0)
                                {
                                    Tcalc.CalculationConditionOperator = CalulationConditionOperator;
                                    Tcalc.CalculationConditionValue = CalculationConditionValue;
                                }
                                if (CalculationApplyOnDataOperator.Length > 0)
                                {
                                    Tcalc.CalculationApplyOnData = true;
                                    Tcalc.CalculationApplyOnDataOperator = CalculationApplyOnDataOperator;
                                }
                                //}

                                break;
                            case "Translation":
                                DiversityWorkbench.Import.Transformation Ttrans = new Transformation(iDC, Transformation.TransformationType.Translation);
                                //R.Read();
                                if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                    R.Read();
                                if (R.IsStartElement() && R.Name == "Translations" || R.Name == "Translation" || R.Name == "Transformation")
                                {
                                    R.Read();
                                    if (R.Name == "Transformation" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                                        break;
                                    if (R.Name == "Translations" && R.NodeType == System.Xml.XmlNodeType.Element)
                                        R.Read();
                                    while (R.NodeType != System.Xml.XmlNodeType.EndElement && R.Name != "Translations")
                                    {
                                        if (R.IsStartElement() && R.Name == "Translations")
                                            R.Read();
                                        if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                            R.Read();
                                        if (R.IsStartElement() && R.Name == "Translation")
                                        {
                                            string Translate = R.GetAttribute(0);
                                            string Into = R.GetAttribute(1);
                                            Ttrans.TranslationDictionary.Add(Translate, Into);
                                        }
                                        if (R.IsStartElement() && R.Name == "SourceTable")
                                        {
                                            R.Read();
                                            Ttrans.TranslationSourceTable = R.Value;
                                            // Markus 1.8.22 - bei leeren Feldern wurden zu viele Reads abgesetzt
                                            while (!R.IsStartElement() && R.Name != "FromColumn")
                                                R.Read();
                                            if (R.IsStartElement() && R.Name == "FromColumn")
                                            {
                                                R.Read();
                                                Ttrans.TranslationFromColumn = R.Value;
                                            }
                                            while (!R.IsStartElement() && R.Name != "IntoColumn")
                                                R.Read();
                                            if (R.IsStartElement() && R.Name == "IntoColumn")
                                            {
                                                R.Read();
                                                Ttrans.TranslationIntoColumn = R.Value;
                                            }
                                            if (Ttrans.TranslationSourceTable.Length > 0)
                                                Ttrans.TranslationDictionarySourceTableReadData();
                                        }

                                        if (R.Name == "Translations" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                                            break;
                                        R.Read();
                                        if (R.Name == "Translations" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                                            break;
                                    }
                                }
                                break;
                            case "Split":
                                DiversityWorkbench.Import.Transformation Tsplit = new Transformation(iDC, Transformation.TransformationType.Split);
                                int Position;
                                if (R.Name == "SplitterPosition" && R.NodeType == System.Xml.XmlNodeType.Element)
                                {
                                    R.Read();
                                    if (R.NodeType == System.Xml.XmlNodeType.Text && R.Value.ToString().Length > 0)
                                    {
                                        if (int.TryParse(R.Value, out Position))
                                            Tsplit.SplitterPosition = Position;
                                    }
                                }
                                R.Read();
                                if (R.NodeType == System.Xml.XmlNodeType.EndElement)
                                    R.Read();
                                if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                    R.Read();
                                if (R.IsStartElement() && R.Name == "SplitterPosition")
                                {
                                    R.Read();
                                    if (int.TryParse(R.Value, out Position))
                                        Tsplit.SplitterPosition = Position;
                                    R.Read();
                                }
                                if (R.NodeType == System.Xml.XmlNodeType.EndElement && R.Name == "SplitterPosition")
                                    R.Read();
                                if (R.IsStartElement() && R.Name == "ReverseSequence")
                                {
                                    bool ReverseSequence;
                                    R.Read();
                                    if (bool.TryParse(R.Value, out ReverseSequence))
                                        Tsplit.ReverseSequence = ReverseSequence;
                                    R.Read();
                                }
                                while (R.Name != "Transformation")
                                {
                                    R.Read();
                                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                        R.Read();
                                    if (R.IsEmptyElement)
                                    {
                                        System.Windows.Forms.MessageBox.Show("Split transformation missing splitters detected");
                                        break;
                                    }
                                    if (R.IsStartElement() && R.Name == "Splitters")
                                    {
                                        R.Read();
                                        if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                            R.Read();
                                        while (R.Name != "Splitters")
                                        {
                                            if (R.Name == "Splitter" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                                                R.Read();
                                            if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                                R.Read();
                                            if (R.Name == "Splitter" && R.IsStartElement())
                                            {
                                                R.Read();
                                                Tsplit.SplitterList.Add(R.Value);
                                            }
                                            if (R.Name != "Splitters")
                                                R.Read();
                                        }
                                    }
                                }
                                break;
                            case "RegularExpression":
                                if (R.Name == "Transformation")
                                    R.Read();
                                if (R.IsStartElement() && R.Name == "RegularExpression")
                                    R.Read();
                                string RegularExpression = R.Value;
                                while (!R.IsStartElement() && R.Name != "RegularExpressionReplacement")
                                    R.Read();
                                R.Read();
                                string RegularExpressionReplacement = R.Value;
                                DiversityWorkbench.Import.Transformation Treg = new Transformation(iDC, Transformation.TransformationType.RegularExpression);
                                Treg.RegularExpression = RegularExpression;
                                Treg.RegularExpressionReplacement = RegularExpressionReplacement;
                                break;
                            case "Filter":
                                // Alte version - fuehrt zu endlos schleife
                                if (R.Name == "Filter")
                                {
                                    return;
                                }

                                string FilterUseFixedValue = "";
                                if (R.Name == "Transformation")
                                    R.Read();

                                while (!R.IsStartElement() && R.Name != "FilterUseFixedValue")
                                    R.Read();
                                R.Read();
                                FilterUseFixedValue = R.Value;

                                while (R.NodeType != System.Xml.XmlNodeType.Element && R.Name != "FilterFixedValue")
                                    R.Read();
                                if (!R.IsEmptyElement)
                                    R.Read();
                                string FilterFixedValue = R.Value;

                                if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                    R.Read();

                                while (!R.IsStartElement() || R.Name != "FilterConditionsOperator")
                                    R.Read();

                                R.Read();
                                string FilterOperator = R.Value;
                                Transformation.FilterConditionsOperators FCO = Transformation.FilterConditionsOperators.And;
                                if (FilterOperator != FCO.ToString())
                                    FCO = Transformation.FilterConditionsOperators.Or;

                                DiversityWorkbench.Import.Transformation Tfilter = new Transformation(iDC, Transformation.TransformationType.Filter);
                                Tfilter.FilterUseFixedValue = bool.Parse(FilterUseFixedValue);
                                Tfilter.FilterFixedValue = FilterFixedValue;
                                Tfilter.FilterConditionsOperator = FCO;

                                while (R.Name != "FilterConditions")
                                    R.Read();
                                R.Read();
                                while (R.Name != "FilterConditions") // looking for the end
                                {
                                    while (R.Name != "Filter")
                                    {
                                        //if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                        //    R.Read();
                                        if (R.Name == "FilterConditions")// && R.NodeType == System.Xml.XmlNodeType.EndElement)
                                            break;
                                        R.Read();
                                    }
                                    if (R.Name == "FilterConditions")// && R.NodeType == System.Xml.XmlNodeType.EndElement)
                                        break;

                                    if (!R.IsEmptyElement)
                                        R.Read();
                                    while (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                        R.Read();
                                    DiversityWorkbench.Import.TransformationFilter TF = new TransformationFilter(Tfilter);
                                    Tfilter.FilterConditions.Add(TF);
                                    TF.Filter = R.Value;

                                    //while (!R.IsStartElement() && R.Name != "FilterColumn")
                                    //    R.Read();
                                    while (R.Name != "FilterColumn")
                                        R.Read();
                                    R.Read();
                                    TF.FilterColumn = int.Parse(R.Value);

                                    //while (!R.IsStartElement() && R.Name != "FilterOperator")
                                    //    R.Read();
                                    while (R.Name != "FilterOperator")
                                        R.Read();
                                    R.Read();
                                    TF.FilterOperator = R.Value;
                                }
                                break;
                            case "Color":
                                if (R.IsStartElement() && R.Name == "ColorFrom")
                                    R.Read();
                                string ColorFrom = R.Value;
                                while (!R.IsStartElement() && R.Name != "ColorInto")
                                    R.Read();
                                R.Read();
                                string ColorInto = R.Value;
                                DiversityWorkbench.Import.Transformation Tcolor = new Transformation(iDC, Transformation.TransformationType.Color);
                                switch (ColorFrom)
                                {
                                    case "ARGBint":
                                        Tcolor.ColorFrom = Transformation.ColorFormat.ARGBint;
                                        break;
                                    case "RGBdec":
                                        Tcolor.ColorFrom = Transformation.ColorFormat.RGBdec;
                                        break;
                                    case "RGBhex":
                                        Tcolor.ColorFrom = Transformation.ColorFormat.RGBhex;
                                        break;
                                }
                                switch (ColorInto)
                                {
                                    case "ARGBint":
                                        Tcolor.ColorInto = Transformation.ColorFormat.ARGBint;
                                        break;
                                    case "RGBdec":
                                        Tcolor.ColorInto = Transformation.ColorFormat.RGBdec;
                                        break;
                                    case "RGBhex":
                                        Tcolor.ColorInto = Transformation.ColorFormat.RGBhex;
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
            catch (System.Exception ex) { }
            return;
        }

        private static void LoadColumnTranslation(ref System.Xml.XmlTextReader R, ref DiversityWorkbench.Import.DataColumn IC)
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
                }
            }
            catch (System.Exception ex) { }
            return;
        }

        private static void LoadColumnSplitters(ref System.Xml.XmlTextReader R, ref DiversityWorkbench.Import.DataColumn IC)
        {
            try
            {
                //if ((R.NodeType == System.Xml.XmlNodeType.Text
                //    || R.NodeType == System.Xml.XmlNodeType.Whitespace)
                //    && R.Name == ""
                //    && R.Value.Length > 0
                //    && !IC.Splitters.Contains(R.Value))
                //{
                //    IC.Splitters.Add(R.Value);
                //}
                //while (R.Read())
                //{
                //    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                //    {
                //        if ((R.NodeType == System.Xml.XmlNodeType.Text
                //            || R.NodeType == System.Xml.XmlNodeType.Whitespace)
                //            && R.Name == ""
                //            && R.Value.Length > 0
                //            && !IC.Splitters.Contains(R.Value))
                //        {
                //            IC.Splitters.Add(R.Value);
                //        }
                //        continue;
                //    }
                //    if (R.Name == "Splitters" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                //        return;
                //    if (!R.IsStartElement()
                //        && R.Name != "Splitter"
                //        && !IC.Splitters.Contains(R.Value)
                //        && R.NodeType == System.Xml.XmlNodeType.Text)
                //    {
                //        IC.Splitters.Add(R.Value);
                //    }
                //    else if (R.Name == "Splitter" && R.NodeType == System.Xml.XmlNodeType.Element && !R.IsEmptyElement)
                //    {
                //        R.MoveToContent();
                //        string x = R.ReadElementContentAsString();
                //        if (!IC.Splitters.Contains(x))
                //            IC.Splitters.Add(x);
                //        R.MoveToElement();
                //        if (R.Name == "Splitters" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                //            return;
                //    }
                //}
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
                    //if (R.Name == "ImportTableAttachmentSetting" && R.NodeType == System.Xml.XmlNodeType.Element)
                    //    this.LoadTableAttachmentSetting(ref R);
                }
            }
            catch (System.Exception ex) { }
        }

        //private void LoadTableAttachmentSetting(ref System.Xml.XmlTextReader R)
        //{
        //    try
        //    {
        //        string TableAlias = "";
        //        string TableName = "";
        //        while (R.Read())
        //        {
        //            if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
        //                continue;
        //            if (R.NodeType == System.Xml.XmlNodeType.EndElement
        //                && (R.Name == "ImportTableAttachmentSetting" || R.Name == "ErrorTreatment"))
        //                return;

        //            if (R.NodeType == System.Xml.XmlNodeType.Element && R.Name == "TableAlias")
        //            {
        //                R.Read();
        //                if (R.NodeType == System.Xml.XmlNodeType.Whitespace) R.Read();
        //                TableAlias = R.Value;
        //            }
        //            if (R.NodeType == System.Xml.XmlNodeType.Element && R.Name == "TableName")
        //            {
        //                R.Read();
        //                if (R.NodeType == System.Xml.XmlNodeType.Whitespace) R.Read();
        //                TableName = R.Value;
        //            }
        //            if (TableAlias.Length > 0 && TableName.Length > 0 && R.NodeType == System.Xml.XmlNodeType.Element && R.Name == "DataTreatment")
        //            {
        //                R.Read();
        //                if (R.NodeType == System.Xml.XmlNodeType.Whitespace) R.Read();
        //                string DataTreatment = R.Value;
        //                DiversityCollection.Import_Table T = new Import_Table(TableAlias, TableName, this._Grid);
        //                if (DataTreatment == DiversityCollection.Import_Table.DataTreatment.Insert.ToString())
        //                    T.TreatmentOfData = DiversityCollection.Import_Table.DataTreatment.Insert;
        //                else if (DataTreatment == DiversityCollection.Import_Table.DataTreatment.Merge.ToString())
        //                    T.TreatmentOfData = DiversityCollection.Import_Table.DataTreatment.Merge;
        //                else if (DataTreatment == DiversityCollection.Import_Table.DataTreatment.Update.ToString())
        //                    T.TreatmentOfData = DiversityCollection.Import_Table.DataTreatment.Update;
        //                if (DiversityCollection.Import.ImportTablesDataTreatment == null)
        //                    DiversityCollection.Import.ImportTablesDataTreatment = new Dictionary<string, Import_Table>();
        //                if (!DiversityCollection.Import.ImportTablesDataTreatment.ContainsKey(TableAlias))
        //                    DiversityCollection.Import.ImportTablesDataTreatment.Add(T.TableAlias, T);
        //                else
        //                    DiversityCollection.Import.ImportTablesDataTreatment[TableAlias].TreatmentOfData = T.TreatmentOfData;
        //            }
        //            if (TableAlias.Length > 0 && TableName.Length > 0 && R.NodeType == System.Xml.XmlNodeType.Element && R.Name == "ErrorTreatment")
        //            {
        //                R.Read();
        //                if (R.NodeType == System.Xml.XmlNodeType.Whitespace) R.Read();
        //                string ErrorTreatment = R.Value;// DiversityCollection.Import_Table.ErrorTreatment.AutoNone.ToString();
        //                if (ErrorTreatment == DiversityCollection.Import_Table.ErrorTreatment.AutoFirst.ToString())
        //                    DiversityCollection.Import.ImportTablesDataTreatment[TableAlias].TreatmentOfErrors = Import_Table.ErrorTreatment.AutoFirst;
        //                else if (ErrorTreatment == DiversityCollection.Import_Table.ErrorTreatment.AutoLast.ToString())
        //                    DiversityCollection.Import.ImportTablesDataTreatment[TableAlias].TreatmentOfErrors = Import_Table.ErrorTreatment.AutoLast;
        //                else if (ErrorTreatment == DiversityCollection.Import_Table.ErrorTreatment.Manual.ToString())
        //                    DiversityCollection.Import.ImportTablesDataTreatment[TableAlias].TreatmentOfErrors = Import_Table.ErrorTreatment.Manual;
        //            }
        //        }
        //    }
        //    catch (System.Exception ex) { }
        //}

        #endregion

        #region Steps

        //private void LoadSteps(ref System.Xml.XmlTextReader R)
        //{
        //    try
        //    {
        //        while (R.Read())
        //        {
        //            if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
        //                continue;
        //            if (R.Name == "ImportSteps" && R.NodeType == System.Xml.XmlNodeType.EndElement)
        //                return;
        //            if (R.Name == "ImportStep" && R.NodeType == System.Xml.XmlNodeType.Element)
        //                this.LoadStep(ref R);
        //        }
        //    }
        //    catch (System.Exception ex) { }
        //}

        //private void LoadStep(ref System.Xml.XmlTextReader R)
        //{
        //    bool? _StepIsVisible = null;
        //    string _CurrentNode = "";
        //    string _StepKey = "";
        //    string _Title = "";
        //    try
        //    {
        //        while (R.Read())
        //        {
        //            // Checking if the end of the step information is found
        //            if (R.NodeType == System.Xml.XmlNodeType.EndElement
        //                && (R.Name == "ImportStep" || R.Name == "IsVisible"))
        //                return;

        //            // handling the data
        //            switch (R.NodeType)
        //            {
        //                case System.Xml.XmlNodeType.Whitespace:
        //                    continue;
        //                case System.Xml.XmlNodeType.Text:
        //                    switch (_CurrentNode)
        //                    {
        //                        case "IsVisible":
        //                            bool V;
        //                            if (bool.TryParse(R.Value, out V))
        //                                _StepIsVisible = V;
        //                            break;
        //                        case "Key":
        //                            _StepKey = R.Value;
        //                            break;
        //                        case "Title":
        //                            _Title = R.Value;
        //                            break;
        //                    }
        //                    _CurrentNode = "";
        //                    break;
        //                case System.Xml.XmlNodeType.Element:
        //                    _CurrentNode = R.Name;
        //                    break;
        //                default:
        //                    break;
        //            }

        //            // writing the infos for the step and return
        //            if (_StepKey.Length > 0 && _StepIsVisible != null)
        //            {
        //                //if (DiversityCollection.Import.ImportSteps.ContainsKey(_StepKey))
        //                //    DiversityCollection.Import.ImportSteps[_StepKey].setImportStepVisibility((bool)_StepIsVisible);
        //                return;
        //            }
        //        }
        //    }
        //    catch (System.Exception ex) { }



        //}

        #endregion

        #region Description

        private static void LoadDescription(ref System.Xml.XmlTextReader R)
        {
            try
            {
                if (R.Name == "Description" && R.IsStartElement())
                {
                    if (R.AttributeCount > 0)
                        Import.SchemaDescription = R.GetAttribute("Text");
                }
                //while (R.Read())
                //{
                //    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                //        continue;
                //    if (R.Name == "Description" && R.IsStartElement())
                //    {
                //        if (R.AttributeCount > 0)
                //            Import.SchemaDescription = R.GetAttribute("Text");
                //    }
                //}
            }
            catch (System.Exception ex) { }
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region Construction

        public Import()
        {
        }

        #endregion

    }
}
