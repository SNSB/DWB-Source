using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Spreadsheet
{
    public class Setting
    {

        #region Properties

        private Sheet _Sheet;
        
        #endregion

        #region Construction

        public Setting(ref Sheet Sheet)
        {
            this._Sheet = Sheet;
        }
        
        #endregion

        #region File and directory

        public static string SpreadsheetDirectory()
        {
            System.IO.DirectoryInfo SpreadsheetDirectory = new System.IO.DirectoryInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Spreadsheet));// DiversityWorkbench.Settings.ResourcesDirectoryModule() + "\\Spreadsheet");
            if (!SpreadsheetDirectory.Exists)
            {
                SpreadsheetDirectory.Create();
                SpreadsheetDirectory.Attributes = System.IO.FileAttributes.Directory | System.IO.FileAttributes.Hidden;
            }
            return SpreadsheetDirectory.FullName;
        }

        private string SettingsDirectory()
        {
            System.IO.DirectoryInfo SpreadsheetSettingsDirectory = new System.IO.DirectoryInfo(SpreadsheetDirectory() + "\\Settings");
            if (!SpreadsheetSettingsDirectory.Exists)
            {
                SpreadsheetSettingsDirectory.Create();
            }
            return SpreadsheetSettingsDirectory.FullName;
        }

        private string SettingsTargetDirectory()
        {
            System.IO.DirectoryInfo SettingsTargetDirectory = new System.IO.DirectoryInfo(SettingsDirectory() + "\\" + this._Sheet.Target());
            if (!SettingsTargetDirectory.Exists)
            {
                SettingsTargetDirectory.Create();
            }
            return SettingsTargetDirectory.FullName;
        }

        public System.Collections.Generic.Dictionary<string, string> TargetSettings()
        {
            System.Collections.Generic.Dictionary<string, string> _Settings = new Dictionary<string, string>();
            System.IO.DirectoryInfo _TargetDirectory = new System.IO.DirectoryInfo(SettingsTargetDirectory());
            foreach (System.IO.FileInfo FI in _TargetDirectory.GetFiles())
            {
                if (FI.Extension == ".xml")
                    _Settings.Add(FI.FullName, FI.Name.Substring(0, FI.Name.Length - 4));
            }
            return _Settings;
        }

        private string SettingsFile()
        {
            string FileName = "";
            try
            {
                FileName = SettingsDirectory() + "\\" + this._Sheet.Target() + ".xml";
                //System.IO.FileInfo FI = new System.IO.FileInfo(FileName);
            }
            catch (System.Exception ex)
            {
            }
            return FileName;
        }

        /// <summary>
        /// An additional setting stored in the target directory
        /// </summary>
        /// <param name="Name">Name of the file without extension</param>
        /// <returns>The directory and file name</returns>
        private string SettingsFile(string Name)
        {
            string FileName = "";
            try
            {
                FileName = SettingsTargetDirectory() + "\\" + Name + ".xml";
                //System.IO.FileInfo FI = new System.IO.FileInfo(FileName);
            }
            catch (System.Exception ex)
            {
            }
            return FileName;
        }

        #endregion

        #region Reading

        public void ReadSettings(string FileName)
        {
            System.Xml.XmlReaderSettings xSettings = new System.Xml.XmlReaderSettings();
            System.Xml.Linq.XElement SettingsDocument = null;
            System.IO.FileInfo _SettingsFile = null;
            bool TargetIsFitting = false;
            try
            {
                xSettings.CheckCharacters = false;
                _SettingsFile = new System.IO.FileInfo(FileName);
                if (_SettingsFile.Exists)
                {
                    SettingsDocument = System.Xml.Linq.XElement.Load(System.Xml.XmlReader.Create(_SettingsFile.FullName, xSettings));
                    // Check for correct target
                    try
                    {
                        if (SettingsDocument.HasAttributes)
                        {
                            if (SettingsDocument.Attribute("Target").Value == this._Sheet.Target())
                                TargetIsFitting = true;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                    if (TargetIsFitting)
                    {
                        // clear all map settings
                        this._Sheet.MapColors().Clear();
                        this._Sheet.MapSymbols().Clear();
                        this._Sheet.MapFilterList.Clear();

                        IEnumerable<System.Xml.Linq.XElement> Sets = SettingsDocument.Elements();
                        // Read the entire XML
                        foreach (var Set in Sets)
                        {
                            if (Set.Name == "ProjectID")
                            {
                                this._Sheet.setProjectID(int.Parse(Set.FirstAttribute.Value));
                            }
                            else if (Set.Name == "MaxResults")
                                this._Sheet.setMaxResult(int.Parse(Set.FirstAttribute.Value));
                            else if (Set.Name == "Map")
                            {
                                try
                                {
                                    foreach (System.Xml.Linq.XAttribute A in Set.Attributes())
                                    {
                                        this.ReadSettingsSetValue(A);
                                    }
                                }
                                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "foreach (System.Xml.Linq.XAttribute A in Set.Attributes()"); }
                                foreach (var T in Set.Elements())
                                {
                                    if (T.Name.ToString() == "Colors")
                                    {
                                        this._Sheet.MapColors().Clear();
                                        foreach (var CC in T.Elements())
                                        {
                                            string Brush = CC.Attribute("Brush").Value.Replace("\"", "");
                                            string Operator = CC.Attribute("Operator").Value.Replace("\"", "").Replace("&lt;", "<");
                                            string LowerValue = CC.Attribute("LowerValue").Value.Replace("\"", "");
                                            string UpperValue = CC.Attribute("UpperValue").Value.Replace("\"", "");
                                            MapColor MC = new MapColor(Brush, Operator, LowerValue, UpperValue);
                                            try
                                            {
                                                string SortingValue = CC.Attribute("SortingValue").Value.Replace("\"", "");
                                                int i;
                                                if (int.TryParse(SortingValue, out i))
                                                    MC.SetSortingValue(i, true);
                                            }
                                            catch (System.Exception ex)
                                            {
                                                // SortingValue has been introduced in a later stage and may not be present in older Settings
                                                //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                            }
                                            this._Sheet.MapColors().Add(MC);
                                        }
                                    }
                                    else if (T.Name.ToString() == "Symbols")
                                    {
                                        this._Sheet.MapSymbols().Clear();
                                        foreach (var CC in T.Elements())
                                        {
                                            try
                                            {
                                                string Value = CC.Attribute("Value").Value.Replace("\"", "");
                                                string SymbolTitle = CC.Attribute("SymbolTitle").Value.Replace("\"", "");
                                                double SymbolSize = 1;
                                                double.TryParse(CC.Attribute("SymbolSize").Value.Replace("\"", ""), out SymbolSize);
                                                MapSymbol MS = new MapSymbol(Value, SymbolSize, SymbolTitle);
                                                try
                                                {
                                                    bool IsExcluded = false;
                                                    if (bool.TryParse(CC.Attribute("IsExcluded").Value.Replace("\"", ""), out IsExcluded))
                                                        MS.IsExcluded = IsExcluded;
                                                }
                                                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                                                if (!this._Sheet.MapSymbols().ContainsKey(Value))
                                                    this._Sheet.MapSymbols().Add(Value, MS);
                                                if (Value.Length == 0)
                                                    this._Sheet.MapSymbolForMissing = MS;
                                            }
                                            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "else if (T.Name.ToString() == \"Symbols\")"); }
                                        }
                                    }
                                    else if (T.Name.ToString() == "Filter")
                                    {
                                        foreach (var CC in T.Elements())
                                        {
                                            try
                                            {
                                                string Type = CC.Attribute("FilterType").Value.Replace("\"", "");
                                                MapFilter.FilterTypes FT = MapFilter.FilterTypes.Color;
                                                if (Type == MapFilter.FilterTypes.Geography.ToString())
                                                    FT = MapFilter.FilterTypes.Geography;
                                                else if (Type == MapFilter.FilterTypes.Symbol.ToString())
                                                    FT = MapFilter.FilterTypes.Symbol;
                                                MapFilter MF = new MapFilter(FT, this._Sheet);

                                                string ForwardType = CC.Attribute("ForwardType").Value.Replace("\"", "");
                                                MapFilter.ForwardTypes FoTy = MapFilter.ForwardTypes.Allways;
                                                if (ForwardType == MapFilter.ForwardTypes.RestrictToSuccess.ToString())
                                                    FoTy = MapFilter.ForwardTypes.RestrictToSuccess;
                                                MF.ForwardType = FoTy;

                                                switch (FT)
                                                {
                                                    case MapFilter.FilterTypes.Color:
                                                        break;
                                                    case MapFilter.FilterTypes.Geography:
                                                        foreach (var Sy in CC.Elements())
                                                        {
                                                            if (Sy.Name.ToString() == "GeographyGazetteer")
                                                            {
                                                                MF.setGeographyGazetteer(Sy.Value.ToString());
                                                            }
                                                            else if (Sy.Name.ToString() == "GeographyFilterType")
                                                            {
                                                                switch (Sy.Value.ToString())
                                                                {
                                                                    case "CenterOfQuadrant":
                                                                        MF.setGeographyFilterType(MapFilter.GeographyFilterTypes.CenterOfQuadrant);
                                                                        break;
                                                                    case "Quadrant":
                                                                        MF.setGeographyFilterType(MapFilter.GeographyFilterTypes.Quadrant);
                                                                        break;
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    case MapFilter.FilterTypes.Symbol:
                                                        foreach (var Sy in CC.Elements())
                                                        {
                                                            if (Sy.Name.ToString() == "Symbols")
                                                            {
                                                                foreach (var sy in Sy.Elements())
                                                                {
                                                                    string SymbolValue = sy.Attribute("Value").Value.Replace("\"", "");
                                                                    MF.AddSymbol(SymbolValue);
                                                                }
                                                            }
                                                        }
                                                        break;
                                                }

                                                int Position = int.Parse(CC.Attribute("Position").Value.Replace("\"", ""));
                                                MF.setPosition(Position);
                                                this._Sheet.AddMapFilter(MF);
                                            }
                                            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "else if (T.Name.ToString() == \"Filter\")"); }
                                        }
                                    }
                                    else if (T.Name.ToString() == "Evaluation")
                                    {
                                        foreach (var CC in T.Elements())
                                        {
                                            try
                                            {
                                                if (CC.Name == "EvaluationGazetteer")
                                                {
                                                    this._Sheet.EvaluationSetGazetteer(CC.Value.ToString());
                                                }
                                                else if (CC.Name == "Symbols")
                                                {
                                                    this._Sheet.EvaluationSymbolValueSequence().Clear();
                                                    foreach (var Sy in CC.Elements())
                                                    {
                                                        if (Sy.Name.ToString() == "Symbol")
                                                        {
                                                            string Symbol = Sy.Attribute("Value").Value.ToString();
                                                            int Position;
                                                            if (int.TryParse(Sy.Attribute("Position").Value.ToString(), out Position) && !this._Sheet.EvaluationSymbolValueSequence().ContainsKey(Symbol))
                                                                this._Sheet.EvaluationSymbolValueSequence().Add(Symbol, Position);
                                                        }
                                                    }
                                                }
                                            }
                                            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "else if (T.Name.ToString() == \"Evaluation\")"); }
                                        }
                                    }
                                }
                            }
                            else if (Set.Name == "Tables")
                            {
                                foreach (var T in Set.Elements())
                                {
                                    string TableAlias = T.Attribute("TableAlias").Value;
                                    try
                                    {
                                        if (!this._Sheet.DataTables().ContainsKey(TableAlias))
                                        {
                                            // Check if an alias exists for the table
                                            string TemplateAlias = "";
                                            foreach (var CC in T.Elements())
                                            {
                                                if (CC.Name == "TemplateAlias")
                                                {
                                                    TemplateAlias = CC.Value.ToString();
                                                    if (this._Sheet.DataTables().ContainsKey(CC.Value.ToString()))
                                                    {
                                                        TemplateAlias = CC.Value.ToString();
                                                    }
                                                }
                                            }

                                            DataTable DT = null;
                                            if (TemplateAlias.Length > 0 && this._Sheet.DataTables().ContainsKey(TemplateAlias))
                                            {
                                                string Alias = this._Sheet.DataTables()[TemplateAlias].GetDataTableParallel(this._Sheet.DataTables()[TemplateAlias]);
                                                DT = this._Sheet.DataTables()[Alias];
                                            }
                                            else
                                            {
                                                DT = new DataTable(T.Attribute("TableAlias").Value, T.Attribute("TableName").Value, T.Attribute("TableAlias").Value, DataTable.TableType.Parallel, ref this._Sheet);
                                            }
                                            this._Sheet.AddDataTable(DT);
                                       }
                                        foreach (var CC in T.Elements())
                                        {
                                            if (CC.Name == "DisplayText")
                                            {
                                                this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DisplayText = CC.Value;
                                            }
                                            else if (CC.Name == "ParentTableAlias")
                                            {
                                                if (this._Sheet.DataTables()[T.Attribute("TableAlias").Value].ParentTable() == null)
                                                    this._Sheet.DataTables()[T.Attribute("TableAlias").Value].setParentTable(CC.Value.ToString());
                                            }
                                            else if (CC.Name == "FilterOperator")
                                            {
                                                this._Sheet.DataTables()[T.Attribute("TableAlias").Value].FilterOperator = CC.Value.ToString();
                                            }
                                            else if (CC.Name == "TemplateAlias")
                                            {
                                                if (!this._Sheet.DataTables().ContainsKey(CC.Value.ToString()))
                                                {
                                                    this._Sheet.DataTables().Remove(T.Attribute("TableAlias").Value);
                                                    continue;
                                                }
                                                else
                                                    this._Sheet.DataTables()[T.Attribute("TableAlias").Value].TemplateAlias = CC.Value.ToString();
                                            }
                                            else if (CC.Name == "Columns")
                                            {
                                                try
                                                {
                                                    System.Collections.Generic.List<string> HandledColumns = new List<string>();
                                                    foreach (var C in CC.Elements())
                                                    {
                                                        if (C.HasAttributes)
                                                        {
                                                            try 
                                                            {
                                                                string aliasTable = T.Attribute("TableAlias").Value;
                                                                string aliasColumn = C.Attribute("Name").Value;
                                                                if (this._Sheet.DataTables().ContainsKey(aliasTable))
                                                                {
                                                                    if (this._Sheet.DataTables()[aliasTable].DataColumns().ContainsKey(aliasColumn))
                                                                    {
                                                                        this._Sheet.DataTables()[aliasTable].DataColumns()[aliasColumn].IsVisible = bool.Parse(C.Attribute("IsVisible").Value.ToString());
                                                                        HandledColumns.Add(C.Attribute("Name").Value.ToString());
                                                                    }
                                                                }
                                                            }
                                                            catch (System.Exception ex)
                                                            {
                                                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "C.HasAttributes");
                                                            }
                                                        }
                                                        if (C.HasElements)
                                                        {
                                                            IEnumerable<System.Xml.Linq.XElement> ColumnInfos = C.Elements();
                                                            bool IsHiddenIsSet = false;
                                                            string aliasTable = T.Attribute("TableAlias").Value;
                                                            string aliasColumn = C.Attribute("Name").Value;
                                                            if (this._Sheet.DataTables().ContainsKey(aliasTable) && this._Sheet.DataTables()[aliasTable].DataColumns().ContainsKey(aliasColumn))
                                                            {
                                                                foreach (var CI in ColumnInfos)
                                                                {
                                                                    try
                                                                    {
                                                                        switch (CI.Name.ToString())
                                                                        {
                                                                            case "Width":
                                                                                this._Sheet.DataTables()[aliasTable].DataColumns()[aliasColumn].Width = int.Parse(CI.Value.ToString());
                                                                                break;
                                                                            case "IsHidden":
                                                                                this._Sheet.DataTables()[aliasTable].DataColumns()[aliasColumn].IsHidden = bool.Parse(CI.Value.ToString());
                                                                                IsHiddenIsSet = true;
                                                                                break;
                                                                            case "RestrictionValue":
                                                                                this._Sheet.DataTables()[aliasTable].DataColumns()[aliasColumn].RestrictionValue = CI.Value.ToString();
                                                                                this._Sheet.DataTables()[aliasTable].DataColumns()[aliasColumn].IsRestrictionColumn = true;
                                                                                break;
                                                                            case "DisplayText":
                                                                                this._Sheet.DataTables()[aliasTable].DataColumns()[aliasColumn].DisplayText = CI.Value;
                                                                                break;
                                                                            case "DisplayTextOriginal":
                                                                                this._Sheet.DataTables()[aliasTable].DataColumns()[aliasColumn].DisplayTextOriginal = CI.Value;
                                                                                break;
                                                                            case "DefaultForInsert":
                                                                                this._Sheet.DataTables()[aliasTable].DataColumns()[aliasColumn].DefaultForAdding = CI.Value;
                                                                                break;
                                                                            case "FilterOperator":
                                                                                this._Sheet.DataTables()[aliasTable].DataColumns()[aliasColumn].FilterOperator = CI.Value.ToString();
                                                                                break;
                                                                            case "FilterValue":
                                                                                if ((this._Sheet.DataTables()[aliasTable].DataColumns()[aliasColumn].FilterOperator == "|"
                                                                                    || this._Sheet.DataTables()[aliasTable].DataColumns()[aliasColumn].FilterOperator == "∉") &&
                                                                                    CI.Value.IndexOf("\n") > -1 &&
                                                                                    CI.Value.IndexOf("\r\n") == -1)
                                                                                    this._Sheet.DataTables()[aliasTable].DataColumns()[aliasColumn].FilterValue = CI.Value.ToString().Replace("\n", "\r\n");
                                                                                else
                                                                                    this._Sheet.DataTables()[aliasTable].DataColumns()[aliasColumn].FilterValue = CI.Value.ToString();
                                                                                break;
                                                                            case "FilterModuleLinkRoot":
                                                                                this._Sheet.DataTables()[aliasTable].DataColumns()[aliasColumn].FilterModuleLinkRoot = CI.Value.ToString();
                                                                                break;
                                                                            case "OrderDirection":
                                                                                {
                                                                                    switch (CI.Value.ToString())
                                                                                    {
                                                                                        case "↑":
                                                                                            this._Sheet.DataTables()[aliasTable].DataColumns()[aliasColumn].OrderDirection = DataColumn.OrderByDirection.ascending;
                                                                                            break;
                                                                                        case "↓":
                                                                                            this._Sheet.DataTables()[aliasTable].DataColumns()[aliasColumn].OrderDirection = DataColumn.OrderByDirection.descending;
                                                                                            break;
                                                                                        default:
                                                                                            this._Sheet.DataTables()[aliasTable].DataColumns()[aliasColumn].OrderDirection = DataColumn.OrderByDirection.none;
                                                                                            break;
                                                                                    }
                                                                                }
                                                                                break;
                                                                            case "OrderSequence":
                                                                                int i;
                                                                                if (int.TryParse(CI.Value.ToString(), out i))
                                                                                    this._Sheet.DataTables()[aliasTable].DataColumns()[aliasColumn].OrderSequence = i;
                                                                                break;
                                                                        }
                                                                    }
                                                                    catch (System.Exception ex)
                                                                    {
                                                                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "foreach (var CI in ColumnInfos)");
                                                                    }
                                                                }
                                                                /// Hidden is only saved to setting file if true, therefore this must be corrected here
                                                                /// otherwise the changes of a user of an originaly hidden field will not be effective
                                                                if (!IsHiddenIsSet && this._Sheet.DataTables()[aliasTable].DataColumns()[aliasColumn].IsHidden)
                                                                    this._Sheet.DataTables()[aliasTable].DataColumns()[aliasColumn].IsHidden = false;
                                                            }
                                                        }
                                                    }
                                                    foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DataColumns())
                                                    {
                                                        try
                                                        {
                                                            if (!HandledColumns.Contains(DC.Key) && DC.Value.IsVisible)
                                                                DC.Value.IsVisible = false;
                                                            if (this._Sheet.DataTables()[T.Attribute("TableAlias").Value].TemplateAlias != null &&
                                                                this._Sheet.DataTables()[T.Attribute("TableAlias").Value].TemplateAlias.Length > 0)
                                                            {
                                                                string TemplateAlias = this._Sheet.DataTables()[T.Attribute("TableAlias").Value].TemplateAlias;
                                                                if (this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].TypeOfLink == DataColumn.LinkType.OptionalLinkToDiversityWorkbenchModule &&
                                                                    this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].IsLinkColumn == null)
                                                                {
                                                                    string DecisionColmn = this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].RemoteLinkDecisionColmn;
                                                                    string DecisionValue = this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DataColumns()[DecisionColmn].RestrictionValue;
                                                                    if (this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].RemoteLinks != null)
                                                                    {
                                                                        DC.Value.setRemoteLinks(this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].RemoteLinks);
                                                                        if (DC.Value.RemoteLinks != null)
                                                                        {
                                                                            foreach (DiversityWorkbench.Spreadsheet.RemoteLink RL in DC.Value.RemoteLinks)
                                                                            {
                                                                                if (RL.DecisionColumnValues.Contains(DecisionValue))
                                                                                {
                                                                                    DC.Value.TypeOfLink = this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].TypeOfLink;
                                                                                    DC.Value.SetIsLinkColumn(true);
                                                                                    DC.Value.LinkedModule = RL.LinkedToModule;
                                                                                    DC.Value.setRemoteLinks(
                                                                                        this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].RemoteLinkIsOptional,
                                                                                        this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].RemoteLinkDecisionColmn,
                                                                                        this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].RemoteLinkDisplayColumn,
                                                                                        this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].RemoteLinks,
                                                                                        this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].TypeOfLink);
                                                                                    break;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        catch (System.Exception ex)
                                                        {
                                                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                                        }
                                                    }
                                                }
                                                catch (System.Exception ex)
                                                {
                                                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "else if (CC.Name == \"Columns\")");
                                                }
                                            }
                                        }
                                    }
                                    catch (System.Exception ex)
                                    {
                                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                    }
                                }
                            }
                        }
                        this._Sheet.ResetFilter();
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Settings file does not correspond to " + this._Sheet.Target());
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                xSettings = null;
                SettingsDocument = null;
                _SettingsFile = null;
            }
        }

        private void ReadSettingsSetValue(System.Xml.Linq.XAttribute A)
        {
            try
            {
                switch (A.Name.ToString())
                {
                    case "GeographyTableAlias":
                        this._Sheet.GeographyTableAlias = A.Value;
                        break;
                    case "GeographyColumn":
                        this._Sheet.GeographyColumn = A.Value;
                        break;
                    case "GeographyKeyTableAlias":
                        this._Sheet.GeographyKeyTableAlias = A.Value;
                        break;
                    case "GeographyKeyColum":
                        this._Sheet.GeographyKeyColumn = A.Value;
                        break;
                    case "GeographyUseKeyFilter":
                        this._Sheet.GeographyUseKeyFilter = bool.Parse(A.Value);
                        break;
                    case "GeographyColorTableAlias":
                        this._Sheet.GeographyColorTableAlias = A.Value;
                        break;
                    case "GeographyColorColumn":
                        this._Sheet.GeographyColorColumn = A.Value;
                        break;
                    case "GeographySymbolTableAlias":
                        this._Sheet.GeographySymbolTableAlias = A.Value;
                        break;
                    case "GeographySymbolColumn":
                        this._Sheet.GeographySymbolColumn = A.Value;
                        break;
                    case "GeographySymbolSize":
                        double d = 0;
                        if (double.TryParse(A.Value, out d))
                            this._Sheet.GeographySymbolSize = d;
                        break;
                    case "GeographySymbolSizeTableAlias":
                        this._Sheet.GeographySymbolSizeTableAlias = A.Value;
                        break;
                    case "GeographySymbolSizeColumn":
                        this._Sheet.GeographySymbolSizeColumn = A.Value;
                        break;
                    case "GeographySymbolSizeLinkedToColumn":
                        this._Sheet.GeographySymbolSizeLinkedToColumn = bool.Parse(A.Value);
                        break;
                    case "GeographySymbolSourceTable":
                        this._Sheet.GeographySymbolSourceTable = A.Value;
                        break;
                    case "GeographySymbolSourceColumn":
                        this._Sheet.GeographySymbolSourceColumn = A.Value;
                        break;
                    case "GeographySymbolSourceRestriction":
                        this._Sheet.GeographySymbolSourceRestriction = A.Value;
                        break;
                    case "GeographyTransparency":
                        byte b = 0;
                        if (byte.TryParse(A.Value, out b))
                            this._Sheet.GeographyTransparency = b;
                        break;
                    case "GeographyMap":
                        this._Sheet.GeographyMap = A.Value;
                        break;
                    case "GeographyWGS84TableAlias":
                        this._Sheet.GeographyWGS84TableAlias = A.Value;
                        break;
                    case "GeographyWGS84Column":
                        this._Sheet.GeographyWGS84Column = A.Value;
                        break;
                    case "GeographyUnitGeoTableAlias":
                        this._Sheet.GeographyUnitGeoTableAlias = A.Value;
                        break;
                    case "GeographyUnitGeoColumn":
                        this._Sheet.GeographyUnitGeoColumn = A.Value;
                        break;
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }


        public bool ReadSettings()
        {
            bool OK = true;
            try
            {
                System.IO.FileInfo F = new System.IO.FileInfo(SettingsFile());
                if (F.Exists)
                    this.ReadSettings(SettingsFile());
                else OK = false;
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }
        
        #endregion

        #region Reset

        public bool ResetSettings()
        {
            bool OK = true;
            try
            {
                System.IO.FileInfo F = new System.IO.FileInfo(SettingsFile());
                if (F.Exists)
                    F.Delete();
                //else OK = false;
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }
        
        #endregion

        #region Write settings

        public string WriteSettings()
        {
            string FileName = SettingsFile();
            this.WriteSheetSettings(FileName);
            return FileName;
        }

        /// <summary>
        /// Save an additional setting for the target
        /// </summary>
        /// <param name="Name">Name of the additional setting in the target directory</param>
        /// <returns>The full path and file name of the setting</returns>
        public string WriteSettings(string Name)
        {
            string FileName = this.SettingsFile(Name);
            //if (!FileName.EndsWith(".xml"))
            //    FileName += ".xml";
            this.WriteSheetSettings(FileName);
            return FileName;
        }

        private void WriteSheetSettings(string FileName)
        {
            System.Xml.XmlWriter W = null;
            try
            {
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                settings.Encoding = System.Text.Encoding.UTF8;
                W = System.Xml.XmlWriter.Create(FileName, settings);
                W.WriteStartDocument();
                W.WriteStartElement("SpreadsheetSettings");
                W.WriteAttributeString("Target", this._Sheet.Target());

                this.WriteSheetSettings(ref W);

                W.WriteStartElement("Tables");
                foreach (System.Collections.Generic.KeyValuePair<string, DataTable> KV in this._Sheet.DataTables())
                {
                    this.WriteTableSettings(ref W, KV.Value);
                }
                W.WriteEndElement();//Tables

                this.WriteMapSettings(ref W);//SpreadsheetSettings

                W.WriteEndElement();//SpreadsheetSettings
                W.WriteEndDocument();
                W.Flush();
                W.Close();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
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

        private void WriteSheetSettings(ref System.Xml.XmlWriter W)
        {
            try
            {
                W.WriteStartElement("ProjectID");
                W.WriteAttributeString("ID", this._Sheet.ProjectID().ToString());
                W.WriteEndElement();//ProjectID
                W.WriteStartElement("MaxResults");
                W.WriteAttributeString("Max", this._Sheet.MaxResult().ToString());
                W.WriteEndElement();//ProjectID
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void WriteTableSettings(ref System.Xml.XmlWriter W, DataTable Table)
        {
            try
            {
                W.WriteStartElement("Table");
                W.WriteAttributeString("TableAlias", Table.Alias());
                W.WriteAttributeString("TableName", Table.Name);
                if (Table.DisplayText != null && Table.DisplayText.Length > 0)
                    W.WriteElementString("DisplayText", Table.DisplayText);
                if (Table.ParentTable() != null)
                    W.WriteElementString("ParentTableAlias", Table.ParentTable().Alias());
                if (Table.FilterOperator != null)
                    W.WriteElementString("FilterOperator", Table.FilterOperator);
                if (Table.TemplateAlias != null)
                    W.WriteElementString("TemplateAlias", Table.TemplateAlias);
                W.WriteStartElement("Columns");
                foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in Table.DataColumns())
                {
                    this.WriteColumnSettings(ref W, DC.Value);
                }
                W.WriteEndElement();//Columns
                W.WriteEndElement();//Table
            }
            catch (System.Exception ex)
            {
            }
        }

        private void WriteColumnSettings(ref System.Xml.XmlWriter W, DataColumn Column)
        {
            try
            {
                if (Column.IsVisible || Column.RestrictionValue != null)
                {
                    W.WriteStartElement("Column");
                    W.WriteAttributeString("Name", Column.Name);
                    W.WriteAttributeString("IsVisible", Column.IsVisible.ToString());

                    if (Column.Width > 0)
                        W.WriteElementString("Width", Column.Width.ToString());

                    if (Column.IsHidden)
                        W.WriteElementString("IsHidden", Column.IsHidden.ToString());

                    if (Column.DisplayTextOriginal != null && Column.DisplayTextOriginal.Length > 0)
                        W.WriteElementString("DisplayTextOriginal", Column.DisplayTextOriginal);

                    if (Column.DisplayText != null && Column.DisplayText.Length > 0)
                        W.WriteElementString("DisplayText", Column.DisplayText);

                    if (Column.RestrictionValue != null && Column.RestrictionValue.Length > 0)
                        W.WriteElementString("RestrictionValue", Column.RestrictionValue.ToString());

                    if (Column.FilterOperator != null && Column.FilterOperator.Length > 0)
                        W.WriteElementString("FilterOperator", Column.FilterOperator.ToString());

                    if (Column.FilterModuleLinkRoot != null && Column.FilterModuleLinkRoot.Length > 0)
                        W.WriteElementString("FilterModuleLinkRoot", Column.FilterModuleLinkRoot.ToString());

                    if (Column.FilterValue != null && Column.FilterValue.Length > 0)
                        W.WriteElementString("FilterValue", Column.FilterValue.ToString());

                    if (Column.OrderDirection != DataColumn.OrderByDirection.none)
                    {
                        if (Column.OrderDirection == DataColumn.OrderByDirection.ascending)
                            W.WriteElementString("OrderDirection", "↑");
                        else
                            W.WriteElementString("OrderDirection", "↓");
                    }

                    if (Column.OrderSequence != null)
                        W.WriteElementString("OrderSequence", Column.OrderSequence.ToString());

                    if (Column.DefaultForAdding != null && Column.DefaultForAdding.Length > 0)
                        W.WriteElementString("DefaultForInsert", Column.DefaultForAdding.ToString());

                    W.WriteEndElement();//Column
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void WriteMapSettings(ref System.Xml.XmlWriter W)
        {
            try
            {
                W.WriteStartElement("Map");
                W.WriteAttributeString("GeographyTableAlias", this._Sheet.GeographyTableAlias);
                W.WriteAttributeString("GeographyColumn", this._Sheet.GeographyColumn);
                W.WriteAttributeString("GeographyKeyTableAlias", this._Sheet.GeographyKeyTableAlias);
                W.WriteAttributeString("GeographyKeyColum", this._Sheet.GeographyKeyColumn);
                W.WriteAttributeString("GeographyUseKeyFilter", this._Sheet.GeographyUseKeyFilter.ToString());
                W.WriteAttributeString("GeographyColorTableAlias", this._Sheet.GeographyColorTableAlias);
                W.WriteAttributeString("GeographyColorColumn", this._Sheet.GeographyColorColumn);
                W.WriteAttributeString("GeographySymbolTableAlias", this._Sheet.GeographySymbolTableAlias);
                W.WriteAttributeString("GeographySymbolColumn", this._Sheet.GeographySymbolColumn);
                W.WriteAttributeString("GeographySymbolSize", this._Sheet.GeographySymbolSize.ToString());
                W.WriteAttributeString("GeographySymbolSizeTableAlias", this._Sheet.GeographySymbolSizeTableAlias);
                W.WriteAttributeString("GeographySymbolSizeColumn", this._Sheet.GeographySymbolSizeColumn);
                W.WriteAttributeString("GeographySymbolSizeLinkedToColumn", this._Sheet.GeographySymbolSizeLinkedToColumn.ToString());
                W.WriteAttributeString("GeographySymbolSourceTable", this._Sheet.GeographySymbolSourceTable);
                W.WriteAttributeString("GeographySymbolSourceColumn", this._Sheet.GeographySymbolSourceColumn);
                W.WriteAttributeString("GeographySymbolSourceRestriction", this._Sheet.GeographySymbolSourceRestriction);
                W.WriteAttributeString("GeographyTransparency", this._Sheet.GeographyTransparency.ToString());
                W.WriteAttributeString("GeographyMap", this._Sheet.GeographyMap .ToString());
                W.WriteAttributeString("GeographyWGS84TableAlias", this._Sheet.GeographyWGS84TableAlias.ToString());
                W.WriteAttributeString("GeographyWGS84Column", this._Sheet.GeographyWGS84Column.ToString());
                W.WriteAttributeString("GeographyUnitGeoTableAlias", this._Sheet.GeographyUnitGeoTableAlias.ToString());
                W.WriteAttributeString("GeographyUnitGeoColumn", this._Sheet.GeographyUnitGeoColumn.ToString());

                W.WriteStartElement("Colors");
                foreach (MapColor MC in this._Sheet.MapColors())
                    this.WriteMapColor(ref W, MC);
                W.WriteEndElement();//Colors

                W.WriteStartElement("Symbols");
                foreach (System.Collections.Generic.KeyValuePair<string, MapSymbol> MS in this._Sheet.MapSymbols())
                    this.WriteMapSymbol(ref W, MS.Value);
                W.WriteEndElement();//Symbols

                W.WriteStartElement("Evaluation");
                W.WriteElementString("EvaluationGazetteer", this._Sheet.EvaluationGazetteer());
                W.WriteStartElement("Symbols");
                foreach (System.Collections.Generic.KeyValuePair<string, int> KV in this._Sheet.EvaluationSymbolValueSequence())
                {
                    W.WriteStartElement("Symbol");
                    W.WriteAttributeString("Value", KV.Key.ToString());
                    W.WriteAttributeString("Position", KV.Value.ToString());
                    W.WriteEndElement();//Symbol
                }
                W.WriteEndElement();//Symbols
                W.WriteEndElement();//Evaluation

                W.WriteStartElement("Filter");
                W.WriteAttributeString("UseFilter", this._Sheet.GeographyUseKeyFilter.ToString());
                foreach (System.Collections.Generic.KeyValuePair<int, MapFilter> MF in this._Sheet.MapFilterList)
                    this.WriteMapFilter(ref W, MF.Key, MF.Value);
                W.WriteEndElement();//Filter

                W.WriteEndElement();//Map
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void WriteMapColor(ref System.Xml.XmlWriter W, MapColor MapColor)
        {
            try
            {
                W.WriteStartElement("MapColor");
                W.WriteAttributeString("Brush", MapColor.Brush.ToString());
                W.WriteAttributeString("Operator", MapColor.Operator);
                W.WriteAttributeString("LowerValue", MapColor.LowerValue);
                W.WriteAttributeString("UpperValue", MapColor.UpperValue);
                if (MapColor.SortingValueFixed())
                    W.WriteAttributeString("SortingValue", MapColor.SortingValue().ToString());
                W.WriteEndElement();//MapColor
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void WriteMapSymbol(ref System.Xml.XmlWriter W, MapSymbol MapSymbol)
        {
            try
            {
                W.WriteStartElement("MapSymbol");
                W.WriteAttributeString("SymbolTitle", MapSymbol.SymbolTitle.ToString());
                W.WriteAttributeString("SymbolSize", MapSymbol.SymbolSize.ToString());
                W.WriteAttributeString("Value", MapSymbol.Value);
                W.WriteAttributeString("IsExcluded", MapSymbol.IsExcluded.ToString());
                W.WriteEndElement();//MapSymbol
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void WriteMapFilter(ref System.Xml.XmlWriter W, int Position, MapFilter MapFilter)
        {
            try
            {
                W.WriteStartElement("MapFilter");
                W.WriteAttributeString("Position", Position.ToString());
                W.WriteAttributeString("FilterType", MapFilter.FilterType().ToString());
                W.WriteAttributeString("ForwardType", MapFilter.ForwardType.ToString());
                switch (MapFilter.FilterType())
                {
                    case Spreadsheet.MapFilter.FilterTypes.Color:
                        W.WriteElementString("ColorFilterType", MapFilter.ColorFilterType().ToString());
                        break;
                    case Spreadsheet.MapFilter.FilterTypes.Geography:
                        W.WriteElementString("GeographyFilterType", MapFilter.GeographyFilterType().ToString());
                        W.WriteElementString("GeographyGazetteer", MapFilter.GeographyGazetteer());
                        break;
                    case Spreadsheet.MapFilter.FilterTypes.Symbol:
                        W.WriteStartElement("Symbols");
                        foreach (System.Collections.Generic.KeyValuePair<string, int> KV in MapFilter.SymbolValueSequence())
                        {
                            W.WriteStartElement("Symbol");
                            W.WriteAttributeString("Value", KV.Key.ToString());
                            W.WriteAttributeString("Position", KV.Value.ToString());
                            W.WriteEndElement();//Symbol
                        }
                        W.WriteEndElement();//Symbols
                        break;
                }
                W.WriteEndElement();//MapFilter
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

    }
}
