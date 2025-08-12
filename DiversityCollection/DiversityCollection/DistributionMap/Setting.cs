using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.DistributionMap
{
    public class Setting
    {
        #region Parameter and properties

        public enum SymbolColor { Red, Orange, Yellow, Green, Blue, Violet, Brown, Black, Gray, White }
        private SymbolColor _SymbolColor = SymbolColor.Black;

        public void setColorForSymbol(string ColorName)
        {
            switch (ColorName)
            {
                case "Black":
                    this._SymbolColor = SymbolColor.Black;
                    break;
                case "Blue":
                    this._SymbolColor = SymbolColor.Blue;
                    break;
                case "Brown":
                    this._SymbolColor = SymbolColor.Brown;
                    break;
                case "Gray":
                    this._SymbolColor = SymbolColor.Gray;
                    break;
                case "Green":
                    this._SymbolColor = SymbolColor.Green;
                    break;
                case "Orange":
                    this._SymbolColor = SymbolColor.Orange;
                    break;
                case "Red":
                    this._SymbolColor = SymbolColor.Red;
                    break;
                case "Violett":
                    this._SymbolColor = SymbolColor.Violet;
                    break;
                case "White":
                    this._SymbolColor = SymbolColor.White;
                    break;
                case "Yellow":
                    this._SymbolColor = SymbolColor.Yellow;
                    break;
                default:
                    this._SymbolColor = SymbolColor.Black;
                    break;
            }
        }
        //private System.Drawing.Color _SettingColor = System.Drawing.Color.Black;
        public System.Drawing.Color SettingColor
        {
            get
            {
                switch (this._SymbolColor)
                {
                    case SymbolColor.Black:
                        return System.Drawing.Color.Black;
                    case SymbolColor.Blue:
                        return System.Drawing.Color.Blue;
                    case SymbolColor.Brown:
                        return System.Drawing.Color.Brown;
                    case SymbolColor.Gray:
                        return System.Drawing.Color.Gray;
                    case SymbolColor.Green:
                        return System.Drawing.Color.Green;
                    case SymbolColor.Orange:
                        return System.Drawing.Color.Orange;
                    case SymbolColor.Red:
                        return System.Drawing.Color.Red;
                    case SymbolColor.Violet:
                        return System.Drawing.Color.Violet;
                    case SymbolColor.White:
                        return System.Drawing.Color.White;
                    case SymbolColor.Yellow:
                        return System.Drawing.Color.Yellow;
                    default:
                        return System.Drawing.Color.Black;
                }
                //if (this._SettingColor == null)
                //    this._SettingColor = System.Drawing.Color.Black;
                //return _SettingColor;
            }
            //set { _SettingColor = value; }
        }

        public System.Windows.Media.Brush Brush
        {
            get
            {
                switch (this._SymbolColor)
                {
                    case SymbolColor.Black:
                        return System.Windows.Media.Brushes.Black;
                    case SymbolColor.Blue:
                        return System.Windows.Media.Brushes.Blue;
                    case SymbolColor.Brown:
                        return System.Windows.Media.Brushes.Brown;
                    case SymbolColor.Gray:
                        return System.Windows.Media.Brushes.Gray;
                    case SymbolColor.Green:
                        return System.Windows.Media.Brushes.Green;
                    case SymbolColor.Orange:
                        return System.Windows.Media.Brushes.Orange;
                    case SymbolColor.Red:
                        return System.Windows.Media.Brushes.Red;
                    case SymbolColor.Violet:
                        return System.Windows.Media.Brushes.Violet;
                    case SymbolColor.White:
                        return System.Windows.Media.Brushes.White;
                    case SymbolColor.Yellow:
                        return System.Windows.Media.Brushes.Yellow;
                    default:
                        return System.Windows.Media.Brushes.Black;
                }
            }
        }

        private string _Title;
        public string Title
        {
            get
            {
                if (this._Title == null)
                    this._Title = "";
                return _Title;
            }
            set { _Title = value; }
        }

        private string _FromWhereClause;
        public string FromWhereClause
        {
            get { return _FromWhereClause; }
            set { _FromWhereClause = value; }
        }

        public enum Symbol { Circle, CircleFilled, Square, SquareFilled, Diamond, DiamondFilled/*, TriangleUp, TriangleUpFilled, TriangleDown, TriangleDownFilled*/, Cross, X/*, Minus, Questionmark*/ }
        private Symbol _Symbol = Symbol.CircleFilled;
        public void setSymbol(Symbol Symbol)
        {
            this._Symbol = Symbol;
            switch (this._Symbol)
            {
                case Setting.Symbol.Circle:
                    this._PointSymbol = WpfSamplingPlotPage.PointSymbol.Circle;
                    this._SymbolTransparency = 100;
                    break;
                case Setting.Symbol.CircleFilled:
                    this._PointSymbol = WpfSamplingPlotPage.PointSymbol.Circle;
                    this._SymbolTransparency = 0;
                    break;
                case Setting.Symbol.Cross:
                    this._PointSymbol = WpfSamplingPlotPage.PointSymbol.Cross;
                    this._SymbolTransparency = 0;
                    break;
                case Setting.Symbol.Diamond:
                    this._PointSymbol = WpfSamplingPlotPage.PointSymbol.Diamond;
                    this._SymbolTransparency = 100;
                    break;
                case Setting.Symbol.DiamondFilled:
                    this._PointSymbol = WpfSamplingPlotPage.PointSymbol.Diamond;
                    this._SymbolTransparency = 0;
                    break;
                case Setting.Symbol.Square:
                    this._PointSymbol = WpfSamplingPlotPage.PointSymbol.Square;
                    this._SymbolTransparency = 100;
                    break;
                case Setting.Symbol.SquareFilled:
                    this._PointSymbol = WpfSamplingPlotPage.PointSymbol.Square;
                    this._SymbolTransparency = 0;
                    break;
                case Setting.Symbol.X:
                    this._PointSymbol = WpfSamplingPlotPage.PointSymbol.X;
                    this._SymbolTransparency = 0;
                    break;
            }
        }

        private WpfSamplingPlotPage.PointSymbol _PointSymbol = WpfSamplingPlotPage.PointSymbol.Circle;
        public WpfSamplingPlotPage.PointSymbol PointSymbol
        {
            get
            {
                return _PointSymbol;
            }
            set { _PointSymbol = value; }
        }

        private bool _SymbolIsFilled = true;
        /// <summary>
        /// Extending the WpfSamplingPlotPage.PointSymbol to filled symbols
        /// </summary>
        public bool SymbolIsFilled
        {
            get { return _SymbolIsFilled; }
            set { _SymbolIsFilled = value; }
        }

        private byte _SymbolTransparency = 0;
        /// <summary>
        /// Transparency for line and filling
        /// </summary>
        public byte SymbolTransparency
        {
            get { return _SymbolTransparency; }
            set { _SymbolTransparency = value; }
        }

        //public string GetPointSymbolCharacter()
        //{
        //    string SymbolCharater = "●";
        //    switch (this._PointSymbol)
        //    {
        //        case WpfSamplingPlotPage.PointSymbol.Circle:
        //            if (this.SymbolIsFilled)
        //                SymbolCharater = "●";
        //            else
        //                SymbolCharater = "○";
        //            break;
        //        case WpfSamplingPlotPage.PointSymbol.Cross:
        //            SymbolCharater = "+";
        //            break;
        //        case WpfSamplingPlotPage.PointSymbol.Diamond:
        //            if (this.SymbolIsFilled)
        //                SymbolCharater = "";
        //            else
        //                SymbolCharater = "";
        //            break;
        //        case WpfSamplingPlotPage.PointSymbol.Square:
        //            if (this.SymbolIsFilled)
        //                SymbolCharater = "";
        //            else
        //                SymbolCharater = "□";
        //            break;
        //        case WpfSamplingPlotPage.PointSymbol.X:
        //            SymbolCharater = "x";
        //            break;
        //    }
        //    return SymbolCharater;
        //}

        private int _SymbolSize = 10;
        public int SymbolSize
        {
            get
            {
                if (this._SymbolSize == 0)
                    this._SymbolSize = 10;
                return _SymbolSize;
            }
            set { _SymbolSize = value; }
        }

        //private int _Transparency;
        //private string _RestrictionTable;
        //private string _RestrictionColumn;
        //private string _RestrictionValue;
        
        #endregion

        #region Construction
        
        public Setting()
        {

        }
        
        #endregion

        #region File and directory

        public static string DistributionMapDirectory()
        {
            System.IO.DirectoryInfo DistributionMapDirectory = new System.IO.DirectoryInfo(DiversityWorkbench.Settings.ResourcesDirectoryModule() + "\\DistributionMap");
            if (!DistributionMapDirectory.Exists)
            {
                DistributionMapDirectory.Create();
            }
            return DistributionMapDirectory.FullName;
        }

        public static string SettingsDirectory()
        {
            System.IO.DirectoryInfo DistributionMapSettingsDirectory = new System.IO.DirectoryInfo(DistributionMapDirectory() + "\\Settings");
            if (!DistributionMapSettingsDirectory.Exists)
            {
                DistributionMapSettingsDirectory.Create();
            }
            return DistributionMapSettingsDirectory.FullName;
        }

        //private string SettingsFile()
        //{
        //    string FileName = "";
        //    try
        //    {
        //        FileName = SettingsDirectory() + "\\DistributionMap.xml";
        //        //System.IO.FileInfo FI = new System.IO.FileInfo(FileName);
        //    }
        //    catch (System.Exception ex)
        //    {
        //    }
        //    return FileName;
        //}

        /// <summary>
        /// An additional setting stored in the directory
        /// </summary>
        /// <param name="Name">Name of the file without extension</param>
        /// <returns>The directory and file name</returns>
        private static string SettingsFile(string Name)
        {
            string FileName = "";
            try
            {
                FileName = SettingsDirectory() + "\\" + Name + ".xml";
                //System.IO.FileInfo FI = new System.IO.FileInfo(FileName);
            }
            catch (System.Exception ex)
            {
            }
            return FileName;
        }

        #endregion

        #region Write settings

        //public string WriteSettings()
        //{
        //    string FileName = SettingsFile();
        //    this.WriteSheetSettings(FileName);
        //    return FileName;
        //}

        /// <summary>
        /// Save an additional setting for the target
        /// </summary>
        /// <param name="Name">Name of the additional setting in the target directory</param>
        /// <returns>The full path and file name of the setting</returns>
        //public string WriteSettings(string Name)
        //{
        //    string FileName = Setting.SettingsFile(Name);
        //    //if (!FileName.EndsWith(".xml"))
        //    //    FileName += ".xml";
        //    this.WriteSheetSettings(FileName);
        //    return FileName;
        //}

        public static void WriteDistributionMapSettings(string FileName, System.Collections.Generic.List<Setting> Settings)
        {
            System.Xml.XmlWriter W = null;
            try
            {
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                settings.Encoding = System.Text.Encoding.UTF8;
                W = System.Xml.XmlWriter.Create(SettingsFile(FileName), settings);
                W.WriteStartDocument();
                W.WriteStartElement("DistributionMap");

                W.WriteStartElement("Settings");
                foreach (Setting S in Settings)
                {
                    Setting.WriteSetting(ref W, S);
                }
                W.WriteEndElement();//Settings

                W.WriteEndElement();//DistributionMap
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

        //private void WriteSheetSettings(ref System.Xml.XmlWriter W)
        //{
        //    try
        //    {
        //        W.WriteStartElement("ProjectID");
        //        W.WriteAttributeString("ID", this._Sheet.ProjectID().ToString());
        //        W.WriteEndElement();//ProjectID
        //        W.WriteStartElement("MaxResults");
        //        W.WriteAttributeString("Max", this._Sheet.MaxResult().ToString());
        //        W.WriteEndElement();//ProjectID
        //    }
        //    catch (System.Exception ex)
        //    { }
        //}

        private static void WriteSetting(ref System.Xml.XmlWriter W, Setting Setting)
        {
            try
            {
                W.WriteStartElement("Setting");
                W.WriteAttributeString("Title", Setting.Title);
                W.WriteAttributeString("Symbol", Setting._Symbol.ToString());
                if (Setting._FromWhereClause != null && Setting._FromWhereClause.Length > 0)
                    W.WriteElementString("FromWhereClause", Setting._FromWhereClause);
                if (Setting._SymbolColor != SymbolColor.Black)
                    W.WriteElementString("SymbolColor", Setting._SymbolColor.ToString());
                //if (Setting._SettingColor != System.Drawing.Color.Black)
                //{
                //    W.WriteElementString("SettingColor", Setting._SettingColor.Name);
                //}

                //if (Table.FilterOperator != null)
                //    W.WriteElementString("FilterOperator", Table.FilterOperator);
                //if (Table.TemplateAlias != null)
                //    W.WriteElementString("TemplateAlias", Table.TemplateAlias);
                //W.WriteStartElement("Columns");
                //foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in Table.DataColumns())
                //{
                //    this.WriteColumnSettings(ref W, DC.Value);
                //}
                W.WriteEndElement();//Setting
                //W.WriteEndElement();//Table
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Reading

        public static System.Collections.Generic.List<Setting> ReadSettings(string FileName)
        {
            System.Xml.XmlReaderSettings xmlSettings = new System.Xml.XmlReaderSettings();
            System.Xml.Linq.XElement SettingsDocument = null;
            System.IO.FileInfo _SettingsFile = null;
            System.Collections.Generic.List<Setting> SS = new List<Setting>();
            try
            {
                xmlSettings.CheckCharacters = false;
                _SettingsFile = new System.IO.FileInfo(FileName);
                if (_SettingsFile.Exists)
                {
                    SettingsDocument = System.Xml.Linq.XElement.Load(System.Xml.XmlReader.Create(_SettingsFile.FullName, xmlSettings));
                    //if (TargetIsFitting)
                    {
                        IEnumerable<System.Xml.Linq.XElement> Sets = SettingsDocument.Elements();
                        // Read the entire XML
                        foreach (var Set in Sets)
                        {
                            if (Set.Name == "Settings")
                            {
                                foreach (var T in Set.Elements())
                                {
                                    try
                                    {
                                        Setting S = new Setting();
                                        S._Title = T.Attribute("Title").Value;
                                        string Symbol = T.Attribute("Symbol").Value;
                                        switch (Symbol)
                                        {
                                            case "Circle":
                                                S._Symbol = Setting.Symbol.Circle;
                                                break;
                                            case "CircleFilled":
                                                S._Symbol = Setting.Symbol.CircleFilled;
                                                break;
                                            case "Cross":
                                                S._Symbol = Setting.Symbol.Cross;
                                                break;
                                            case "Diamond":
                                                S._Symbol = Setting.Symbol.Diamond;
                                                break;
                                            case "DiamondFilled":
                                                S._Symbol = Setting.Symbol.DiamondFilled;
                                                break;
                                            case "Square":
                                                S._Symbol = Setting.Symbol.Square;
                                                break;
                                            case "SquareFilled":
                                                S._Symbol = Setting.Symbol.SquareFilled;
                                                break;
                                            case "X":
                                                S._Symbol = Setting.Symbol.X;
                                                break;
                                        }
                                        foreach (var CC in T.Elements())
                                        {
                                            if (CC.Name == "FromWhereClause")
                                            {
                                                S._FromWhereClause = CC.Value;
                                            }
                                        }
                                        SS.Add(S);

                                        //System.Drawing.Color C = new System.Drawing.c
                                        //C.
                    //                    if (!this._Sheet.DataTables().ContainsKey(T.Attribute("TableAlias").Value))
                    //                    {
                    //                        DataTable DT = new DataTable(T.Attribute("TableAlias").Value, T.Attribute("TableName").Value, T.Attribute("TableAlias").Value, DataTable.TableType.Parallel, ref this._Sheet);
                    //                        this._Sheet.AddDataTable(DT);
                    //                    }
                    //                    foreach (v ar CC in T.Elements())
                    //                    {
                    //                        if (CC.Name == "DisplayText")
                    //                        {
                    //                            this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DisplayText = CC.Value;
                    //                        }
                    //                        else if (CC.Name == "ParentTableAlias")
                    //                        {
                    //                            if (this._Sheet.DataTables()[T.Attribute("TableAlias").Value].ParentTable() == null)
                    //                                this._Sheet.DataTables()[T.Attribute("TableAlias").Value].setParentTable(CC.Value.ToString());
                    //                        }
                    //                        else if (CC.Name == "FilterOperator")
                    //                        {
                    //                            this._Sheet.DataTables()[T.Attribute("TableAlias").Value].FilterOperator = CC.Value.ToString();
                    //                        }
                    //                        else if (CC.Name == "TemplateAlias")
                    //                        {
                    //                            if (!this._Sheet.DataTables().ContainsKey(CC.Value.ToString()))
                    //                            {
                    //                                this._Sheet.DataTables().Remove(T.Attribute("TableAlias").Value);
                    //                                continue;
                    //                            }
                    //                            else
                    //                                this._Sheet.DataTables()[T.Attribute("TableAlias").Value].TemplateAlias = CC.Value.ToString();
                    //                        }
                    //                        else if (CC.Name == "Columns")
                    //                        {
                    //                            System.Collections.Generic.List<string> HandledColumns = new List<string>();
                    //                            foreach (var C in CC.Elements())
                    //                            {
                    //                                if (C.HasAttributes)
                    //                                {
                    //                                    this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DataColumns()[C.Attribute("Name").Value].IsVisible = bool.Parse(C.Attribute("IsVisible").Value.ToString());
                    //                                    HandledColumns.Add(C.Attribute("Name").Value.ToString());
                    //                                }
                    //                                if (C.HasElements)
                    //                                {
                    //                                    IEnumerable<System.Xml.Linq.XElement> ColumnInfos = C.Elements();
                    //                                    foreach (var CI in ColumnInfos)
                    //                                    {
                    //                                        switch (CI.Name.ToString())
                    //                                        {
                    //                                            case "Width":
                    //                                                this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DataColumns()[C.Attribute("Name").Value].Width = int.Parse(CI.Value.ToString());
                    //                                                break;
                    //                                            case "RestrictionValue":
                    //                                                this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DataColumns()[C.Attribute("Name").Value].RestrictionValue = CI.Value.ToString();
                    //                                                this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DataColumns()[C.Attribute("Name").Value].IsRestrictionColumn = true;
                    //                                                break;
                    //                                            case "DisplayText":
                    //                                                this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DataColumns()[C.Attribute("Name").Value].DisplayText = CI.Value;
                    //                                                break;
                    //                                            case "DefaultForInsert":
                    //                                                this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DataColumns()[C.Attribute("Name").Value].DefaultForAdding = CI.Value;
                    //                                                break;
                    //                                            case "FilterOperator":
                    //                                                this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DataColumns()[C.Attribute("Name").Value].FilterOperator = CI.Value.ToString();
                    //                                                break;
                    //                                            case "FilterValue":
                    //                                                if ((this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DataColumns()[C.Attribute("Name").Value].FilterOperator == "|"
                    //                                                    || this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DataColumns()[C.Attribute("Name").Value].FilterOperator == "∉") &&
                    //                                                    CI.Value.IndexOf("\n") > -1 &&
                    //                                                    CI.Value.IndexOf("\r\n") == -1)
                    //                                                    this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DataColumns()[C.Attribute("Name").Value].FilterValue = CI.Value.ToString().Replace("\n", "\r\n");
                    //                                                else
                    //                                                    this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DataColumns()[C.Attribute("Name").Value].FilterValue = CI.Value.ToString();
                    //                                                break;
                    //                                            case "OrderDirection":
                    //                                                {
                    //                                                    switch (CI.Value.ToString())
                    //                                                    {
                    //                                                        case "↑":
                    //                                                            this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DataColumns()[C.Attribute("Name").Value].OrderDirection = DataColumn.OrderByDirection.ascending;
                    //                                                            break;
                    //                                                        case "↓":
                    //                                                            this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DataColumns()[C.Attribute("Name").Value].OrderDirection = DataColumn.OrderByDirection.descending;
                    //                                                            break;
                    //                                                        default:
                    //                                                            this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DataColumns()[C.Attribute("Name").Value].OrderDirection = DataColumn.OrderByDirection.none;
                    //                                                            break;
                    //                                                    }
                    //                                                }
                    //                                                break;
                    //                                            case "OrderSequence":
                    //                                                int i;
                    //                                                if (int.TryParse(CI.Value.ToString(), out i))
                    //                                                    this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DataColumns()[C.Attribute("Name").Value].OrderSequence = i;
                    //                                                break;
                    //                                        }
                    //                                    }
                    //                                }
                    //                            }
                    //                            foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DataColumns())
                    //                            {
                    //                                try
                    //                                {
                    //                                    if (!HandledColumns.Contains(DC.Key) && DC.Value.IsVisible)
                    //                                        DC.Value.IsVisible = false;
                    //                                    if (this._Sheet.DataTables()[T.Attribute("TableAlias").Value].TemplateAlias != null &&
                    //                                        this._Sheet.DataTables()[T.Attribute("TableAlias").Value].TemplateAlias.Length > 0)
                    //                                    {
                    //                                        string TemplateAlias = this._Sheet.DataTables()[T.Attribute("TableAlias").Value].TemplateAlias;
                    //                                        if (this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].TypeOfLink == DataColumn.LinkType.OptionalLinkToDiversityWorkbenchModule &&
                    //                                            this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].IsLinkColumn == null)
                    //                                        {
                    //                                            string DecisionColmn = this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].RemoteLinkDecisionColmn;
                    //                                            string DecisionValue = this._Sheet.DataTables()[T.Attribute("TableAlias").Value].DataColumns()[DecisionColmn].RestrictionValue;
                    //                                            if (this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].RemoteLinks != null)
                    //                                            {
                    //                                                DC.Value.setRemoteLinks(this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].RemoteLinks);
                    //                                                if (DC.Value.RemoteLinks != null)
                    //                                                {
                    //                                                    foreach (DiversityWorkbench.Spreadsheet.RemoteLink RL in DC.Value.RemoteLinks)
                    //                                                    {
                    //                                                        if (RL.DecisionColumnValues.Contains(DecisionValue))
                    //                                                        {
                    //                                                            DC.Value.TypeOfLink = this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].TypeOfLink;
                    //                                                            DC.Value.SetIsLinkColumn(true);
                    //                                                            DC.Value.LinkedModule = RL.LinkedToModule;
                    //                                                            DC.Value.setRemoteLinks(
                    //                                                                this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].RemoteLinkIsOptional,
                    //                                                                this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].RemoteLinkDecisionColmn,
                    //                                                                this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].RemoteLinkDisplayColumn,
                    //                                                                this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].RemoteLinks,
                    //                                                                this._Sheet.DataTables()[TemplateAlias].DataColumns()[DC.Key].TypeOfLink);
                    //                                                            break;
                    //                                                        }
                    //                                                    }
                    //                                                }
                    //                                            }
                    //                                        }
                    //                                    }
                    //                                }
                    //                                catch (System.Exception ex)
                    //                                {
                    //                                }
                    //                            }
                    //                        }
                    //                    }
                                    }
                                    catch (System.Exception ex)
                                    {
                                    }
                                }
                            }
                        }
                        //this._Sheet.ResetFilter();
                    }
                    //else
                    //{
                    //    System.Windows.Forms.MessageBox.Show("Settings file does not correspond to " + this._Sheet.Target());
                    //}
                }
            }
            catch (System.Exception ex)
            { }
            finally
            {
                xmlSettings = null;
                SettingsDocument = null;
                _SettingsFile = null;
            }
            return SS;
        }

        #endregion

    }
}
