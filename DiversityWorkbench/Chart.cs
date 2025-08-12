using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench
{

    public struct ChartImage
    {
        public string Source;
        public string Alt;
        public int Height;
        public int Width;
    }

    public struct ChartValue
    {
        /// <summary>
        /// The unique key for the ChartValue
        /// </summary>
        public int ID;
        /// <summary>
        /// The Hierarchy ID of the child related the ChildID of superior data. 
        /// May correspond to ID but as for ScientificTerm, the ID corresponds to the RepresentationID and the ChildID to the TermID
        /// </summary>
        //public int ChildID;
        /// <summary>
        /// The text shown in a user interface
        /// </summary>
        public string DisplayText;
        /// <summary>
        /// Additional infos shown via tooltip
        /// </summary>
        public string Title;
        /// <summary>
        /// The background color used in a user interface
        /// </summary>
        public System.Drawing.Color ChartColor;
        /// <summary>
        /// If the color should be inherited to depending values
        /// </summary>
        public bool InheritColor;
        /// <summary>
        /// The Text color used in a user interface - only set if not standard black/white should be used
        /// will be adapted according to black/white change by transparency
        /// </summary>
        public System.Drawing.Color ForeColor;
        /// <summary>
        /// The IDs of the children
        /// </summary>
        public System.Collections.Generic.List<int> ChildrenIDs;
        /// <summary>
        /// IDs of all depending items
        /// </summary>
        public System.Collections.Generic.List<int> HierarchyIDs;
        /// <summary>
        /// The maximal depth of the hierarchy
        /// </summary>
        //public int HierarchyColumnCount;
        /// <summary>
        /// The summarized number of rows to display in a table including all children
        /// </summary>
        public int RowCount;
        /// <summary>
        /// The row within a display table where 0 is the most top positon within a group
        /// </summary>
        public int RowPosition;
        /// <summary>
        /// The column within a display table where 0 is the most left positon at the base
        /// </summary>
        public int ColumnPosition;
        /// <summary>
        /// The path or URL of an image as shown in the chart
        /// </summary>
        //public string ImagePath;
        //public int ImageWidth;
        //public int ImageHeigth;
        public System.Collections.Generic.List<ChartImage> Images;
        /// <summary>
        /// If the value is starting point of a new group, displayed in a new chart representated e.g. as a new html site
        /// e.g. when a terminology is to large to be displayed on a single page and should be separated into several sites
        /// </summary>
        //public bool IsGroup;
        /// <summary>
        /// If the value is contained in the section
        /// </summary>
        //public bool IsInSection;
    }

    public class Chart
    {

        #region Parameter

        private string _BaseURL;
        private string _ColumnID;
        private string _ColumnChildID;
        private string _ColumnParentID;
        private string _ColumnDisplayText;
        private System.Collections.Generic.List<string> _ColumnsForSorting;
        private System.Data.DataTable _dtChart;
        private System.Collections.Generic.Dictionary<int, ChartValue> _BaseChartValues;
        private System.Collections.Generic.Dictionary<int, int> _RepresentationIDs = null;
        private string _Title;
        private System.Collections.Generic.Dictionary<int, string> _Titles = null;
        private System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<ChartImage>> _ChartImages;
        private System.Collections.Generic.Dictionary<int, ChartValue> _ChartValues;
        private System.Collections.Generic.List<int> _ChartGroups;

        // Color
        private System.Collections.Generic.Dictionary<int, System.Drawing.Color> _ColorCode;
        private System.Collections.Generic.Dictionary<int, System.Drawing.Color> _ForeColorCode;
        private System.Collections.Generic.List<int> _ColorInheritance;
        //private bool _InheritColor = true;
        private int _MaxTransparency = 10;
        private enum TransparencyChange { up, down }
        //private TransparencyChange _TC = TransparencyChange.up;
        bool _UseMaxTransparency = true;

        // Section
        private int? _SectionID = null;
        private System.Collections.Generic.List<int> _IDsInSection = null;
        private System.Collections.Generic.Dictionary<int, string> _ChartSections = null;
        private int _SectionTransparency = 0;
        private int _SectionTransparencyReset = 0;
        private string _Section = "";
        private int _SectionTransparencyCycle = 9;
        //private float _SectionJitter = (float)0.15;

        // Size
        private int? _WindowWidth = null;
        private int? _WindowHeight = null;
        private int _DefaultColumnWidth = 200;

        // Image
        public static readonly int ImageMaxWidth = 100;
        public static readonly int ImageMaxHeight = 100;

        #endregion

        #region Construction

        /// <summary>
        /// Create a new Chart
        /// </summary>
        /// <param name="BaseURL">The connection to the source of the data</param>
        /// <param name="dtChart">The table containing the data</param>
        /// <param name="ColumnID">The ID column</param>
        /// <param name="ColumnParentID">The column pointing to the ID column for building a hierarchy</param>
        /// <param name="ColumnDisplayText">The column containing the display text</param>
        /// <param name="ColorCodes">The dictionary containing the codes for the colors</param>
        /// <param name="SortingColumns">The list of order columns</param>
        /// <param name="Images">The dictionary containing the images</param>
        /// <param name="RepresentationIDs">The dictionary for the translation of the ID into another value, optional</param>
        /// <param name="SectionID">The ID of the section, optional</param>
        /// <param name="IDsInSection">The dictionary containing the IDs of a section</param>
        public Chart(string BaseURL,
            System.Data.DataTable dtChart,
            string ColumnID,
            string ColumnParentID,
            string ColumnDisplayText,
            System.Collections.Generic.Dictionary<int, System.Drawing.Color> ColorCodes,
            System.Collections.Generic.List<string> SortingColumns,
            System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<ChartImage>> Images,
            System.Collections.Generic.Dictionary<int, int> RepresentationIDs = null,
            int? SectionID = null,
            System.Collections.Generic.List<int> IDsInSection = null,
            string Section = "",
            int? Width = null,
            int? Height = null,
            //int? ColumnWidth = null,
            string Title = "",
            System.Collections.Generic.Dictionary<int, System.Drawing.Color> ForeColorCodes = null,
            System.Collections.Generic.Dictionary<int, string> Titles = null,
            System.Collections.Generic.List<int> ColorInheritance = null)
        {
            try
            {
                this._BaseURL = BaseURL;
                this._ColumnID = ColumnID;
                this._ColumnParentID = ColumnParentID;
                this._ColumnDisplayText = ColumnDisplayText;
                if (SortingColumns.Count > 0)
                    this._ColumnsForSorting = SortingColumns;
                else
                {
                    this._ColumnsForSorting = new List<string>();
                    this._ColumnsForSorting.Add(this._ColumnID);
                }
                this._dtChart = dtChart;
                this._ColorCode = ColorCodes;
                this._ColorInheritance = ColorInheritance;
                this._ChartImages = Images;
                this._RepresentationIDs = RepresentationIDs;
                this._IDsInSection = IDsInSection;
                this._SectionID = SectionID;
                this._Section = Section;
                this._WindowHeight = Height;
                this._WindowWidth = Width;
                if (Title.Length > 0)
                    this._Title = Title;
                else
                    this._Title = dtChart.TableName;
                this._Titles = Titles;
                this._ForeColorCode = ForeColorCodes;
                //this._InheritColor = InheritColor;
                if (this._SectionID != null && this._SectionID > 0)
                    this.initSection();
                else
                    this.InitChartValues();
                //this.SetAutoGroups();
                //if (ColumnWidth != null)
                //    this._DefaultColumnWidth = (int)ColumnWidth;
                //if (ColumnWidth != null)
                //    this.SetGroupsAccordingToWidth((int)ColumnWidth);
                //else
                this.SetGroupsAccordingToWidth();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Init

        private void InitChartValues()
        {
            try
            {
                if (this._ChartValues == null)
                    this._ChartValues = new Dictionary<int, ChartValue>();
                // getting the basic values
                System.Data.DataRow[] RR = this._dtChart.Select(this._ColumnParentID + " IS NULL", Sorting());
                int iMax = RR.Length;
                if (_UseMaxTransparency && iMax > _MaxTransparency)
                {
                    iMax = _MaxTransparency;
                }
                int i = 0;
                TransparencyChange _TC = TransparencyChange.up;
                foreach (System.Data.DataRow R in RR)
                {
                    ChartValue C = new ChartValue();
                    // IDs
                    C.ID = int.Parse(R[this._ColumnID].ToString());

                    // Text
                    C.DisplayText = R[this._ColumnDisplayText].ToString();

                    // Title
                    if (this._Titles.ContainsKey(C.ID))
                        C.Title = this._Titles[C.ID];

                    // Image
                    if (this._ChartImages.ContainsKey(C.ID))
                    {
                        if (C.Images == null)
                            C.Images = new List<ChartImage>();
                        foreach (ChartImage CI in this._ChartImages[C.ID])
                        {
                            C.Images.Add(CI);
                        }
                        //C.ImagePath = this._ChartImages[C.ID].Source;
                        //C.ImageWidth = this._ChartImages[C.ID].Width;
                        //C.ImageHeigth = this._ChartImages[C.ID].Height;
                    }

                    // Position
                    C.ColumnPosition = 0;

                    // BackgroundColor
                    float Transparency = this.ColorTransparency(C, i);// (float)((float)((i - 1) / (float)(iMax + 2))) + (float)0.2;
                    C.ChartColor = this.ChartColor(C.ID);
                    C.InheritColor = this.InheritColor(C.ID);
                    if (C.ChartColor == System.Drawing.Color.Transparent)
                    {
                        //float Transparency = this.Transparency(i, iMax, C);// (float)((float)((i - 1) / (float)(iMax + 2))) + (float)0.2;
                        C.ChartColor = DiversityWorkbench.Forms.FormFunctions.paleColor(System.Drawing.Color.FromArgb(255, 50, 50, 50), Transparency); //System.Drawing.Color.DarkGray, Transparency);
                    }
                    i = this.TransparencyCounter(i, _MaxTransparency, ref _TC);

                    // Forecolor
                    C.ForeColor = this.ForeColor(C.ID, Transparency);

                    // Children
                    C.RowCount = this.GetChartValueChildren(ref C);//1;

                    if (this._BaseChartValues == null)
                        this._BaseChartValues = new Dictionary<int, ChartValue>();
                    this._BaseChartValues.Add(C.ID, C);
                    if (this._ChartValues == null)
                        this._ChartValues = new Dictionary<int, ChartValue>();
                    this._ChartValues.Add(C.ID, C);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        //        private float Transparency(int i, int Max, ChartValue CV)
        //        {
        //            float Trans = 0;
        //            if (this._UseMaxTransparency)
        //            {
        //                int p = CV.ColumnPosition % 2; // ensure differences between columns
        //                float part = (float)0.7;
        //                float threshold = (float)0.1;
        //                switch(p)
        //                {
        //                    case 0:
        //                        part = part / (float)(Max * 2);
        //                        Trans = (float)0.05 + threshold + part * (float)(i * 2);
        //                        break;
        //                    case 1:
        //                        part = part / (float)(Max + 1);
        //                        Trans = threshold + part * (float)(i);// Factor;
        //                        break;
        //                }
        //            }
        //            else
        //                Trans = (float)((float)((i - 1) / (float)(Max + 2))) + (float)0.2;
        //            // only for testing
        //#if DEBUG
        //            {
        //                if (this._Translist == null) this._Translist = new List<float>();
        //                this._Translist.Add(Trans);
        //            }
        //#endif
        //           return Trans;
        //        }

        //        private System.Collections.Generic.List<float> _Translist;

        private int TransparencyCounter(int i, int Max, ref TransparencyChange _TC)
        {
            int ii = i;
            if (_UseMaxTransparency)
            {
                if (_TC == TransparencyChange.up)
                {
                    ii++;
                    if (ii >= Max)
                        _TC = TransparencyChange.down;
                }
                else
                {
                    ii--;
                    if (ii < 1)
                        _TC = TransparencyChange.up;
                }
            }
            else
                ii++;

            return ii;
        }

        private string Sorting()
        {
            string S = "";
            foreach (string s in this._ColumnsForSorting)
            {
                if (S.Length > 0)
                    S += ", ";
                S += s;
            }
            return S;
        }


        /// <summary>
        /// Getting all children of a chart value
        /// </summary>
        /// <param name="CValue">The parent chart value</param>
        private int GetChartValueChildren(ref ChartValue CValue)
        {
            int RowCount = 1;
            try
            {
                System.Data.DataRow[] RR = this._dtChart.Select(this._ColumnParentID + " = " + CValue.ID.ToString(), this.Sorting());
                if (RR.Length > 0)
                {
                    int ColorCount = RR.Length;
                    int i = 1;
                    if (_UseMaxTransparency && ColorCount > _MaxTransparency)
                    {
                        ColorCount = _MaxTransparency;
                    }
                    TransparencyChange _TC = TransparencyChange.up;
                    foreach (System.Data.DataRow R in RR)
                    {
                        if (CValue.ChildrenIDs == null)
                            CValue.ChildrenIDs = new List<int>();
                        int ID = int.Parse(R[this._ColumnID].ToString());
                        CValue.ChildrenIDs.Add(ID);
                        if (CValue.HierarchyIDs == null)
                            CValue.HierarchyIDs = new List<int>();
                        CValue.HierarchyIDs.Add(ID);

                        // generate Child chart value
                        ChartValue C = new ChartValue();
                        C.ID = ID;
                        C.DisplayText = R[this._ColumnDisplayText].ToString();

                        // Title
                        if (this._Titles.ContainsKey(C.ID))
                            C.Title = this._Titles[C.ID];

                        // set the position
                        int Position = CValue.ColumnPosition + 1;
                        C.ColumnPosition = Position;

                        // set the color
                        bool ParentColorIsGreyScale = true;
                        if (CValue.ChartColor.R != CValue.ChartColor.G || CValue.ChartColor.R != CValue.ChartColor.B || CValue.ChartColor.G != CValue.ChartColor.B)
                            ParentColorIsGreyScale = false;
                        float Transparency = this.ColorTransparency(C, i);// (float)((float)((i - 1) / (float)(ColorCount + 2))) + (float)0.2;
                        C.ChartColor = this.ChartColor(C.ID);//.ChildID);
                        C.InheritColor = this.InheritColor(C.ID);
                        bool ColorIsGreyScale = true;
                        if (C.ChartColor.R != C.ChartColor.G || C.ChartColor.R != C.ChartColor.B || C.ChartColor.G != C.ChartColor.B)
                            ColorIsGreyScale = false;
                        if (!ParentColorIsGreyScale && ColorIsGreyScale && CValue.InheritColor) // && this.InheritColor(CValue.ID))
                        {
                            C.ChartColor = DiversityWorkbench.Forms.FormFunctions.paleColor(CValue.ChartColor, Transparency);
                            C.InheritColor = true;
                        }
                        else if (C.ChartColor == System.Drawing.Color.Transparent || C.ChartColor.Name == "0" || ColorIsGreyScale)
                        {
                            C.ChartColor = DiversityWorkbench.Forms.FormFunctions.paleColor(System.Drawing.Color.FromArgb(255, 50, 50, 50), Transparency);
                            C.InheritColor = false;
                        }
                        i = this.TransparencyCounter(i, _MaxTransparency, ref _TC);

                        // set forecolor
                        if (C.InheritColor)
                            C.ForeColor = this.ForeColor(C, Transparency);
                        else
                            C.ForeColor = this.ForeColor(C.ID, Transparency);

                        // Section
                        if (this._ChartSections == null)
                            this._ChartSections = new Dictionary<int, string>();

                        // set image
                        if (this._ChartImages.ContainsKey(C.ID))
                        {
                            if (C.Images == null)
                                C.Images = new List<ChartImage>();
                            foreach (ChartImage CI in this._ChartImages[C.ID])
                                C.Images.Add(CI);
                            //C.ImagePath = this._ChartImages[C.ID].Source;
                            //C.ImageWidth = this._ChartImages[C.ID].Width;
                            //C.ImageHeigth = this._ChartImages[C.ID].Height;
                        }

                        int RC = this.GetChartValueChildren(ref C);
                        C.RowCount = RC;

                        if (CValue.HierarchyIDs == null)
                            CValue.HierarchyIDs = new List<int>();
                        if (C.ChildrenIDs != null)
                        {
                            foreach (int id in C.ChildrenIDs)
                            {
                                if (!CValue.ChildrenIDs.Contains(id))
                                    CValue.HierarchyIDs.Add(id);
                            }
                        }
                        if (C.HierarchyIDs != null)
                        {
                            foreach (int id in C.HierarchyIDs)
                            {
                                if (!CValue.HierarchyIDs.Contains(id))
                                    CValue.HierarchyIDs.Add(id);
                            }
                        }
                        this._ChartValues.Add(C.ID, C);
                    }
                }
                CValue.RowCount = 1;
                //if (CValue.ChildrenIDs != null && CValue.ChildrenIDs.Count > 1)
                //{
                //    RowCount = CValue.ChildrenIDs.Count;
                //    CValue.RowCount = CValue.ChildrenIDs.Count;
                //}
                if (CValue.ChildrenIDs != null)
                {
                    foreach (int ID in CValue.ChildrenIDs)
                    {
                        ChartValue C = this.getChartValue(ID);
                        if (C.RowCount > 1)
                            RowCount += C.RowCount - 1;
                    }
                    CValue.RowCount = RowCount;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return RowCount;
        }

        #endregion

        #region Section

        public string Section() { return this._Section; }

        //private void initSection(int? ID = null, int Transparency = 0)
        //{
        //    try
        //    {
        //        if (this._ChartValues == null)
        //            this._ChartValues = new Dictionary<int, ChartValue>();

        //        if (this._FindTransparency)
        //        {
        //            this._SectionDictChildrenCount = new Dictionary<int, int>();
        //            this._SectionDictTransparency = new Dictionary<int, int>();
        //        }
        //        // getting the basic values
        //        if (ID == null)
        //        {
        //            System.Data.DataRow[] RR = this._dtChart.Select(this._ColumnParentID + " IS NULL", Sorting());

        //            int iMax = RR.Length;
        //            foreach (System.Data.DataRow R in RR)
        //            {
        //                int id = int.Parse(R[this._ColumnID].ToString());
        //                this.initSection(id, Transparency);
        //                this._SectionDictTransparency.Add(id, Transparency);
        //                Transparency++;
        //            }
        //        }
        //        else
        //        {
        //            if (this._IDsInSection.Contains((int)ID))
        //            {
        //                ChartValue C = new ChartValue();
        //                // IDs
        //                C.ID = (int)ID;

        //                // Text
        //                System.Data.DataRow[] rr = this._dtChart.Select(this._ColumnID + " = " + ID.ToString(), Sorting());
        //                C.DisplayText = rr[0][this._ColumnDisplayText].ToString();

        //                // Image
        //                if (this._ChartImages.ContainsKey(C.ID))
        //                    C.ImagePath = this._ChartImages[C.ID];

        //                // Position and Children
        //                C.ColumnPosition = 0;

        //                // Color
        //                C.ChartColor = this.ChartColor(C.ID);
        //                if (C.ChartColor == System.Drawing.Color.Transparent)
        //                {
        //                    _SectionTransparencyReset += 1;
        //                    _SectionTransparency = _SectionTransparencyReset;
        //                    _SectionTransparency += 1;
        //                    float ColorTransparency = (float)((float)((_SectionTransparency - 1) / (float)(this._IDsInSection.Count + 2))) + (float)0.2;
        //                    if (_UseMaxTransparency)
        //                        ColorTransparency = this.SectionTransparency(_SectionTransparency, this._IDsInSection.Count, C);
        //                    C.ChartColor = DiversityWorkbench.Forms.FormFunctions.paleColor(System.Drawing.Color.FromArgb(255, 50, 50, 50), ColorTransparency); //System.Drawing.Color.DarkGray, Transparency);
        //                }

        //                // Children
        //                C.RowCount = this.GetChartSectionChildren(ref C, (int)ID);//, Transparency);//1;

        //                if (this._BaseChartValues == null)
        //                    this._BaseChartValues = new Dictionary<int, ChartValue>();
        //                this._BaseChartValues.Add(C.ID, C);
        //                if (this._ChartValues == null)
        //                    this._ChartValues = new Dictionary<int, ChartValue>();
        //                if (!_ChartValues.ContainsKey(C.ID))
        //                    this._ChartValues.Add(C.ID, C);
        //            }
        //            else
        //            {
        //                System.Data.DataRow[] RR = this._dtChart.Select(this._ColumnParentID + " = " + ID.ToString(), Sorting());
        //                foreach (System.Data.DataRow R in RR)
        //                {
        //                    int id;
        //                    if (int.TryParse(R[this._ColumnID].ToString(), out id))
        //                        initSection(id);//, Transparency);
        //                }
        //            }

        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private int GetChartSectionChildren(ref ChartValue CValue, int ParentID, int Transparency)
        //{
        //    int RowCount = 1;
        //    try
        //    {
        //        System.Data.DataRow[] RR = this._dtChart.Select(this._ColumnParentID + " = " + ParentID.ToString(), this.Sorting());
        //        if (RR.Length > 0)
        //        {
        //            int ColorCount = RR.Length;
        //            int i = 1;
        //            foreach (System.Data.DataRow R in RR)
        //            {
        //                int ID = int.Parse(R[this._ColumnID].ToString());
        //                if (this._IDsInSection.Contains(ID))
        //                {
        //                    if (CValue.ChildrenIDs == null)
        //                        CValue.ChildrenIDs = new List<int>();
        //                    if (!CValue.ChildrenIDs.Contains(ID))
        //                        CValue.ChildrenIDs.Add(ID);
        //                    else { }
        //                    if (CValue.HierarchyIDs == null)
        //                        CValue.HierarchyIDs = new List<int>();
        //                    if (!CValue.HierarchyIDs.Contains(ID))
        //                        CValue.HierarchyIDs.Add(ID);
        //                    else { }

        //                    // generate Child chart value
        //                    ChartValue C = new ChartValue();
        //                    C.ID = ID;
        //                    C.DisplayText = R[this._ColumnDisplayText].ToString();

        //                    // set the position
        //                    int Position = CValue.ColumnPosition + 1;
        //                    C.ColumnPosition = Position;

        //                    // set the color
        //                    C.ChartColor = this.ChartColor(C.ID);//.ChildID);
        //                    if (C.ChartColor == System.Drawing.Color.Transparent || C.ChartColor.Name == "0")
        //                    {
        //                        _SectionTransparency += 1;
        //                        float ColorTransparency = (float)((float)((_SectionTransparency - 1) / (float)(this._IDsInSection.Count + 2))) + (float)0.2;
        //                        if (_UseMaxTransparency)
        //                            ColorTransparency = this.SectionTransparency(_SectionTransparency, this._IDsInSection.Count, C);
        //                        C.ChartColor = DiversityWorkbench.Forms.FormFunctions.paleColor(System.Drawing.Color.FromArgb(255, 50, 50, 50), ColorTransparency); //System.Drawing.Color.DarkGray, Transparency);
        //                    }

        //                    // set image
        //                    if (this._ChartImages.ContainsKey(C.ID))
        //                        C.ImagePath = this._ChartImages[C.ID];

        //                    int RC = this.GetChartSectionChildren(ref C, C.ID, Transparency);
        //                    C.RowCount = RC;

        //                    if (CValue.HierarchyIDs == null)
        //                        CValue.HierarchyIDs = new List<int>();
        //                    if (C.ChildrenIDs != null)
        //                    {
        //                        foreach (int id in C.ChildrenIDs)
        //                        {
        //                            if (!CValue.ChildrenIDs.Contains(id))
        //                                CValue.HierarchyIDs.Add(id);
        //                            else { }
        //                        }
        //                    }
        //                    if (C.HierarchyIDs != null)
        //                    {
        //                        foreach (int id in C.HierarchyIDs)
        //                        {
        //                            if (!CValue.HierarchyIDs.Contains(id))
        //                                CValue.HierarchyIDs.Add(id);
        //                            else { }
        //                        }
        //                    }
        //                    if (!this._ChartValues.ContainsKey(C.ID))
        //                        this._ChartValues.Add(C.ID, C);
        //                    this.GetChartSectionChildren(ref C, C.ID, Transparency);
        //                    i++;
        //                }
        //                else
        //                {
        //                    this.GetChartSectionChildren(ref CValue, ID, Transparency);
        //                }

        //            }
        //        }
        //        CValue.RowCount = 1;
        //        if (CValue.ChildrenIDs != null && CValue.ChildrenIDs.Count > 1)
        //        {
        //            RowCount = CValue.ChildrenIDs.Count;
        //            CValue.RowCount = CValue.ChildrenIDs.Count;
        //        }
        //        if (CValue.ChildrenIDs != null)
        //        {
        //            foreach (int ID in CValue.ChildrenIDs)
        //            {
        //                ChartValue C = this.getChartValue(ID);
        //                if (C.RowCount > 1)
        //                    RowCount += C.RowCount - 1;
        //            }
        //            CValue.RowCount = RowCount;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return RowCount;
        //}


        private void initSection(int? ID = null, int Row = 0)
        {
            try
            {
                if (this._ChartValues == null)
                    this._ChartValues = new Dictionary<int, ChartValue>();
                // getting the basic values
                if (ID == null)
                {
                    System.Data.DataRow[] RR = this._dtChart.Select(this._ColumnParentID + " IS NULL", Sorting());

                    int iMax = RR.Length;
                    foreach (System.Data.DataRow R in RR)
                    {
                        int id = int.Parse(R[this._ColumnID].ToString());
                        this.initSection(id, Row);
                        Row++;
                    }
                }
                else
                {
                    if (this._IDsInSection.Contains((int)ID))
                    {
                        ChartValue C = new ChartValue();
                        // IDs
                        C.ID = (int)ID;

                        // Row
                        C.RowPosition = Row;

                        // Text
                        System.Data.DataRow[] rr = this._dtChart.Select(this._ColumnID + " = " + ID.ToString(), Sorting());
                        C.DisplayText = rr[0][this._ColumnDisplayText].ToString();

                        // Title
                        if (this._Titles.ContainsKey(C.ID))
                            C.Title = this._Titles[C.ID];

                        // Image
                        if (this._ChartImages.ContainsKey(C.ID))
                        {
                            if (C.Images == null)
                                C.Images = new List<ChartImage>();
                            foreach (ChartImage CI in this._ChartImages[C.ID])
                            {
                                C.Images.Add(CI);
                            }
                            //C.ImagePath = this._ChartImages[C.ID].Source;
                            //C.ImageHeigth = this._ChartImages[C.ID].Height;
                            //C.ImageWidth = this._ChartImages[C.ID].Width;
                        }

                        // Position and Children
                        C.ColumnPosition = 0;

                        // Color
                        float ColorTransparency = (float)((float)((_SectionTransparency - 1) / (float)(this._IDsInSection.Count + 2))) + (float)0.2;
                        C.ChartColor = this.ChartColor(C.ID);
                        C.InheritColor = this.InheritColor(C.ID);
                        if (C.ChartColor == System.Drawing.Color.Transparent)
                        {
                            _SectionTransparencyReset += 1;
                            _SectionTransparency = _SectionTransparencyReset;
                            _SectionTransparency += 1;
                            if (_UseMaxTransparency)
                            {
                                //ColorTransparency = this.SectionTransparency(_SectionTransparency, this._IDsInSection.Count, C);
                                ColorTransparency = this.ColorTransparency(C, Row);
                            }
                            C.ChartColor = DiversityWorkbench.Forms.FormFunctions.paleColor(System.Drawing.Color.FromArgb(255, 50, 50, 50), ColorTransparency); //System.Drawing.Color.DarkGray, Transparency);
                        }

                        C.ForeColor = this.ForeColor(C.ID, ColorTransparency);

                        // Children
                        int iRow = 0;
                        C.RowCount = this.GetChartSectionChildren(ref C, (int)ID, ref iRow);//, Transparency);//1;

                        if (this._BaseChartValues == null)
                            this._BaseChartValues = new Dictionary<int, ChartValue>();
                        this._BaseChartValues.Add(C.ID, C);
                        if (this._ChartValues == null)
                            this._ChartValues = new Dictionary<int, ChartValue>();
                        if (!_ChartValues.ContainsKey(C.ID))
                            this._ChartValues.Add(C.ID, C);
                    }
                    else
                    {
                        System.Data.DataRow[] RR = this._dtChart.Select(this._ColumnParentID + " = " + ID.ToString(), Sorting());
                        foreach (System.Data.DataRow R in RR)
                        {
                            int id;
                            if (int.TryParse(R[this._ColumnID].ToString(), out id))
                                initSection(id);//, Transparency);
                        }
                    }

                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private int GetChartSectionChildren(ref ChartValue CValue, int ParentID, ref int Row)//, int Transparency)
        {
            //int Row = 0;
            int RowCount = 1;
            try
            {
                if (CValue.ChildrenIDs == null || CValue.ID != ParentID)
                {
                    System.Data.DataRow[] RR = this._dtChart.Select(this._ColumnParentID + " = " + ParentID.ToString(), this.Sorting());
                    if (RR.Length > 0)
                    {
                        int ColorCount = RR.Length;
                        int i = 1;
                        foreach (System.Data.DataRow R in RR)
                        {
                            int ID = int.Parse(R[this._ColumnID].ToString());
                            if (this._IDsInSection.Contains(ID))
                            {
                                if (CValue.ChildrenIDs == null)
                                    CValue.ChildrenIDs = new List<int>();
                                if (!CValue.ChildrenIDs.Contains(ID))
                                    CValue.ChildrenIDs.Add(ID);
                                else { }
                                if (CValue.HierarchyIDs == null)
                                    CValue.HierarchyIDs = new List<int>();
                                if (!CValue.HierarchyIDs.Contains(ID))
                                    CValue.HierarchyIDs.Add(ID);
                                else { }

                                // generate Child chart value
                                ChartValue C = new ChartValue();
                                C.ID = ID;
                                C.DisplayText = R[this._ColumnDisplayText].ToString();

                                // Title
                                if (this._Titles.ContainsKey(C.ID))
                                    C.Title = this._Titles[C.ID];

                                // set the position
                                int Position = CValue.ColumnPosition + 1;
                                C.ColumnPosition = Position;

                                // set the color
                                bool ParentColorIsGreyScale = true;
                                if (CValue.ChartColor.R != CValue.ChartColor.G || CValue.ChartColor.R != CValue.ChartColor.B || CValue.ChartColor.G != CValue.ChartColor.B)
                                    ParentColorIsGreyScale = false;
                                float ColorTransparency = (float)((float)((_SectionTransparency - 1) / (float)(this._IDsInSection.Count + 2))) + (float)0.2;
                                C.ChartColor = this.ChartColor(C.ID);//.ChildID);
                                C.InheritColor = this.InheritColor(C.ID);
                                bool ColorIsGreyScale = true;
                                if (C.ChartColor.R != C.ChartColor.G || C.ChartColor.R != C.ChartColor.B || C.ChartColor.G != C.ChartColor.B)
                                    ColorIsGreyScale = false;
                                if (ColorIsGreyScale)
                                {
                                    _SectionTransparency += 1;
                                    if (_UseMaxTransparency)
                                    {
                                        Row++;
                                        ColorTransparency = this.ColorTransparency(C, Row);
                                    }
                                    if (!ParentColorIsGreyScale && (this.InheritColor(CValue.ID) || CValue.InheritColor))
                                    {
                                        C.ChartColor = DiversityWorkbench.Forms.FormFunctions.paleColor(CValue.ChartColor, ColorTransparency);
                                        C.InheritColor = true;
                                    }
                                    else if (C.ChartColor == System.Drawing.Color.Transparent || C.ChartColor.Name == "0")
                                    {
                                        C.ChartColor = DiversityWorkbench.Forms.FormFunctions.paleColor(System.Drawing.Color.FromArgb(255, 50, 50, 50), ColorTransparency); //System.Drawing.Color.DarkGray, Transparency);
                                    }
                                }

                                C.ForeColor = this.ForeColor(C.ID, ColorTransparency);

                                // set image
                                if (this._ChartImages.ContainsKey(C.ID))
                                {
                                    if (C.Images == null)
                                        C.Images = new List<ChartImage>();
                                    foreach (ChartImage CI in this._ChartImages[C.ID])
                                        C.Images.Add(CI);
                                    //C.ImagePath = this._ChartImages[C.ID].Source;
                                    //C.ImageHeigth = this._ChartImages[C.ID].Height;
                                    //C.ImageWidth = this._ChartImages[C.ID].Width;
                                }

                                int iRow = 0;
                                int RC = this.GetChartSectionChildren(ref C, C.ID, ref iRow);//, Transparency);
                                C.RowCount = RC;

                                if (CValue.HierarchyIDs == null)
                                    CValue.HierarchyIDs = new List<int>();
                                if (C.ChildrenIDs != null)
                                {
                                    foreach (int id in C.ChildrenIDs)
                                    {
                                        if (!CValue.ChildrenIDs.Contains(id))
                                            CValue.HierarchyIDs.Add(id);
                                        else { }
                                    }
                                }
                                if (C.HierarchyIDs != null)
                                {
                                    foreach (int id in C.HierarchyIDs)
                                    {
                                        if (!CValue.HierarchyIDs.Contains(id))
                                            CValue.HierarchyIDs.Add(id);
                                        else { }
                                    }
                                }
                                if (!this._ChartValues.ContainsKey(C.ID))
                                    this._ChartValues.Add(C.ID, C);
                                this.GetChartSectionChildren(ref C, C.ID, ref Row);//, Transparency);
                                i++;
                            }
                            else
                            {
                                this.GetChartSectionChildren(ref CValue, ID, ref Row);//, Transparency);
                            }

                        }
                    }
                    else
                    {
                        if (CValue.ChildrenIDs == null)
                            CValue.ChildrenIDs = new List<int>();
                    }
                }

                CValue.RowCount = 1;
                if (CValue.ChildrenIDs != null && CValue.ChildrenIDs.Count > 1)
                {
                    RowCount = CValue.ChildrenIDs.Count;
                    CValue.RowCount = CValue.ChildrenIDs.Count;
                }
                if (CValue.ChildrenIDs != null)
                {
                    foreach (int ID in CValue.ChildrenIDs)
                    {
                        ChartValue C = this.getChartValue(ID);
                        if (C.RowCount > 1)
                            RowCount += C.RowCount - 1;
                    }
                    CValue.RowCount = RowCount;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return RowCount;
        }

        //        private float SectionTransparency(int i, int Max, ChartValue CV)
        //        {
        //            float Trans = 0;
        //            try
        //            {
        //                if (this._UseMaxTransparency)
        //                {
        //                    int p = CV.ColumnPosition % this.MaxColumnCount; // ensure differences between columns
        //                    int FormCount = (CV.ColumnPosition - p) / MaxColumnCount;
        //                    int iLocal = i;
        //                    if (p > 0 && CV.ColumnPosition > MaxColumnCount)
        //                        iLocal = i - (FormCount * MaxColumnCount);
        //                    int iMax = Max - MaxColumnCount;
        //                    float part = (float)0.7;
        //                    float threshold = (float)0.1;
        //                    Trans = threshold + (float)(iLocal / iMax) * part;
        //                }
        //                else
        //                    Trans = (float)((float)((i - 1) / (float)(Max + 2))) + (float)0.2;
        //            }
        //            catch(System.Exception ex)
        //            {
        //                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //            }
        //            // only for testing
        //#if DEBUG
        //            {
        //                if (this._SectionTranslist == null) this._SectionTranslist = new List<string>();
        //                this._SectionTranslist.Add(Trans.ToString());
        //            }
        //#endif
        //            return Trans;
        //        }

        //private System.Collections.Generic.List<string> _SectionTranslist;

        //private int SectionTransparencyCounter(int i, int Max, ref TransparencyChange _TC)
        //{
        //    int ii = i;
        //    if (_UseMaxTransparency)
        //    {
        //        if (_TC == TransparencyChange.up)
        //        {
        //            ii++;
        //            if (ii >= Max)
        //                _TC = TransparencyChange.down;
        //        }
        //        else
        //        {
        //            ii--;
        //            if (ii < 1)
        //                _TC = TransparencyChange.up;
        //        }
        //    }
        //    else
        //        ii++;

        //    return ii;
        //}

        #endregion

        #region Groups

        #region Setting Groups on account of Width
        private void SetGroupsAccordingToWidth()
        {
            int Max = MaxColumnsPerPage();
            if (Max == 0)
                Max = 1;
            foreach (System.Collections.Generic.KeyValuePair<int, ChartValue> CV in this._ChartValues)
            {
                if (CV.Value.ChildrenIDs != null && CV.Value.ChildrenIDs.Count > 0 &&
                    (((CV.Value.ColumnPosition + 1) % Max == 0 && CV.Value.ColumnPosition > 0) || (CV.Value.ColumnPosition == 0 && Max == 1)))
                {
                    if (this._ChartGroups == null)
                        this._ChartGroups = new List<int>();
                    this._ChartGroups.Add(CV.Key);
                }
            }
        }

        public int MaxColumnsPerPage()
        {
            int Max = 1;
            double Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            if (this._WindowWidth != null)
                Width = (double)this._WindowWidth;
            Width = Width / DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor;
            Max = (int)((int)Width / this._DefaultColumnWidth);
            return Max;
        }

        #endregion

        #region Setting groups an account of Hierarchy
        //private void SetAutoGroups()
        //{
        //    try
        //    {
        //        int GroupBorder = this.GroupBorder();
        //        this._ChartGroups = new List<int>();
        //        System.Collections.Generic.SortedDictionary<int, System.Collections.Generic.List<int>> ColumnCounts = this.ColumnCounts();
        //        foreach (System.Collections.Generic.KeyValuePair<int, System.Collections.Generic.List<int>> KV in ColumnCounts)
        //        {
        //            foreach (int ID in KV.Value)
        //            {
        //                ChartValue CV = this.getChartValue(ID);
        //                foreach (int H in CV.HierarchyIDs)
        //                {
        //                    bool Ignore = false;
        //                    if (this._ChartGroups.Contains(H))
        //                    {
        //                        Ignore = true;
        //                        break;
        //                    }
        //                    if (Ignore)
        //                        continue;
        //                }
        //                if (CV.ColumnPosition < this.MaxColumnCount)
        //                {
        //                    if (CV.RowCount > GroupBorder / 3)
        //                    {
        //                        this._ChartGroups.Add(CV.ID);
        //                    }
        //                    //else
        //                    //    return;
        //                }
        //            }
        //        }

        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }

        //}

        //private int GroupBorder(int Factor = 10)
        //{
        //    double Height = System.Windows.SystemParameters.PrimaryScreenHeight;
        //    double Width = System.Windows.SystemParameters.PrimaryScreenWidth;
        //    double FormFactor = (Height * Width) / DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor;
        //    int GroupBorder = (int)FormFactor / (this._ChartValues.Count * Factor);// * 1000);
        //    return GroupBorder;
        //}

        //private System.Collections.Generic.SortedDictionary<int, System.Collections.Generic.List<int>> ColumnCounts()
        //{
        //    System.Collections.Generic.Dictionary<int, int> ChartCounts = new Dictionary<int, int>();
        //    foreach (System.Collections.Generic.KeyValuePair<int, ChartValue> CV in this._ChartValues)
        //    {
        //        if (CV.Value.ColumnPosition > 0 && CV.Value.ChildrenIDs != null)
        //            ChartCounts.Add(CV.Key, CV.Value.ChildrenIDs.Count);
        //    }
        //    System.Collections.Generic.SortedDictionary<int, System.Collections.Generic.List<int>> Counts = new SortedDictionary<int, List<int>>();
        //    foreach (System.Collections.Generic.KeyValuePair<int, int> KV in ChartCounts)
        //    {
        //        if (!Counts.ContainsKey(-KV.Value))
        //        {
        //            System.Collections.Generic.List<int> L = new List<int>();
        //            L.Add(KV.Key);
        //            Counts.Add(-KV.Value, L);
        //        }
        //        else
        //        {
        //            Counts[-KV.Value].Add(KV.Key);
        //        }
        //    }
        //    return Counts;
        //}

        #endregion

        #endregion

        #region public interface

        public string Title() { return this._Title; }

        public string BaseURL { get { return this._BaseURL; } }

        public int WindowWidth()
        {
            int Width = 0;
            if (this._WindowWidth != null)
                Width = (int)this._WindowWidth;
            else
                Width = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            return Width;
        }

        public int WindowHeight()
        {
            int Heigth = 0;
            if (this._WindowHeight != null)
                Heigth = (int)this._WindowHeight;
            else
                Heigth = (int)System.Windows.SystemParameters.PrimaryScreenHeight;
            return Heigth;

        }

        public System.Collections.Generic.Dictionary<int, ChartValue> BaseChartValues()
        {
            if (this._BaseChartValues == null)
                this._BaseChartValues = new Dictionary<int, ChartValue>();
            return this._BaseChartValues;
        }

        public ChartValue getChartValue(int ID)
        {
            if (this._ChartValues == null)
                this._ChartValues = new Dictionary<int, ChartValue>();

            if (this._ChartValues.ContainsKey(ID))
                return this._ChartValues[ID];
            else
            {
                ChartValue C = new ChartValue();
                return C;
            }
        }

        public ChartValue getChartParent(int ID)
        {
            foreach (System.Collections.Generic.KeyValuePair<int, ChartValue> CV in this._ChartValues)
            {
                if (CV.Value.ChildrenIDs != null && CV.Value.ChildrenIDs.Contains(ID))
                    return CV.Value;
            }
            ChartValue cv = new ChartValue();
            cv.ID = -1;
            return cv;
        }

        public int RepresentationID(int ID)
        {
            if (this._RepresentationIDs == null)
                return ID;
            else
            {
                if (this._RepresentationIDs.ContainsKey(ID))
                    return this._RepresentationIDs[ID];
                else
                    return -1;
            }
        }

        public int MaxRowCount(int ID)
        {
            int Rows = 1;
            try
            {
                ChartValue CV = this.getChartValue(ID);
                if (this._ChartGroups == null)
                    this._ChartGroups = new List<int>();
                if (this._ChartGroups.Contains(ID))
                    return 1;
                if (CV.ChildrenIDs != null)
                {
                    int ChildrenRows = 0;
                    foreach (int id in CV.ChildrenIDs)
                    {
                        ChildrenRows += this.MaxRowCount(id);
                    }
                    if (ChildrenRows > 1)
                        Rows += ChildrenRows - 1;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Rows;
        }

        #region Displaytext format and length

        /// <summary>
        /// Dictionary keeping the max length of words for every column
        /// </summary>
        private System.Collections.Generic.Dictionary<int, int> _MaxColumnWordLengths;

        public int MaxDisplayTextWordLength(int ID, int Position)
        {
            int Max = 1;
            try
            {
                ChartValue CV = this.getChartValue(ID);
                return this.MaxDisplayTextWordLength(CV.ColumnPosition);

                if (this._MaxColumnWordLengths == null)
                    this._MaxColumnWordLengths = new Dictionary<int, int>();
                if (this._MaxColumnWordLengths.ContainsKey(CV.ColumnPosition))
                    return this._MaxColumnWordLengths[CV.ColumnPosition];

                // nothing found - test all data within this column
                int iMax = 0;
                foreach (System.Collections.Generic.KeyValuePair<int, ChartValue> cv in this._ChartValues)
                {
                    if (this.IsSection && !this._IDsInSection.Contains(cv.Key))
                        continue;
                    if (cv.Value.ColumnPosition == CV.ColumnPosition)
                    {
                        System.Collections.Generic.List<string> Words = this.DisplayTextSplitted(cv.Value.DisplayText);
                        foreach (string word in Words)
                        {
                            if (word.Length > iMax)
                                iMax = word.Length;
                        }
                    }
                }
                this._MaxColumnWordLengths.Add(CV.ColumnPosition, iMax);
                return iMax;


                string[] Display = CV.DisplayText.Split(new char[] { ' ' });
                for (int d = 0; d < Display.Length; d++)
                {
                    if (Display[d].Length > Max)
                        Max = Display[d].Length;
                }
                if (Position == null)
                {
                    foreach (System.Collections.Generic.KeyValuePair<int, ChartValue> cv in this._ChartValues)
                    {
                        if (cv.Value.ColumnPosition == CV.ColumnPosition)
                        {
                            int max = this.MaxDisplayTextWordLength(cv.Key, CV.ColumnPosition);
                            if (max > Max)
                                Max = max;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Max;
        }

        public int MaxDisplayTextWordLength(int Position)
        {
            int Max = 1;
            try
            {
                if (this._MaxColumnWordLengths == null)
                    this._MaxColumnWordLengths = new Dictionary<int, int>();
                if (this._MaxColumnWordLengths.ContainsKey(Position))
                    return this._MaxColumnWordLengths[Position];

                // nothing found - test all data within this column
                int iMax = 0;
                foreach (System.Collections.Generic.KeyValuePair<int, ChartValue> cv in this._ChartValues)
                {
                    if (this.IsSection && !this._IDsInSection.Contains(cv.Key))
                        continue;
                    if (cv.Value.ColumnPosition == Position)
                    {
                        System.Collections.Generic.List<string> Words = this.DisplayTextSplitted(cv.Value.DisplayText);
                        foreach (string word in Words)
                        {
                            if (word.Length > iMax)
                                iMax = word.Length;
                        }
                    }
                }
                this._MaxColumnWordLengths.Add(Position, iMax);
                return iMax;



                foreach (System.Collections.Generic.KeyValuePair<int, ChartValue> CV in this._ChartValues)
                {
                    if (CV.Value.ColumnPosition == Position)
                    {
                        System.Collections.Generic.List<string> Words = this.DisplayTextSplitted(CV.Key);
                        foreach (string W in Words)
                        {
                            if (W.Length > Max)
                                Max = W.Length;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Max;
        }

        public System.Collections.Generic.List<string> DisplayTextSplitted(int ID)
        {
            ChartValue CV = this.getChartValue(ID);
            System.Collections.Generic.List<string> DisplayList = this.DisplayTextSplitted(CV.DisplayText);
            return DisplayList;
        }

        public System.Collections.Generic.List<string> DisplayTextSplitted(string DisplayText)
        {
            System.Collections.Generic.List<string> DisplayList = new List<string>();
            string Word = "";
            bool WordContainsNonSplitterChar = false;
            for (int i = 0; i < DisplayText.Length; i++)
            {
                if (this.DisplaySplitter().Contains(DisplayText[i]) && WordContainsNonSplitterChar)
                {
                    DisplayList.Add(Word);
                    Word = DisplayText[i].ToString().Trim();
                    WordContainsNonSplitterChar = false;
                }
                else
                {
                    Word += DisplayText[i];
                    WordContainsNonSplitterChar = true;
                }
            }
            if (Word.Length > 0)
                DisplayList.Add(Word);
            return DisplayList;
        }


        private System.Collections.Generic.List<char> _DisplaySplitter;
        private System.Collections.Generic.List<char> DisplaySplitter()
        {
            if (this._DisplaySplitter == null)
            {
                this._DisplaySplitter = new List<char>();
                this._DisplaySplitter.Add(' ');
                this._DisplaySplitter.Add('-');
                this._DisplaySplitter.Add('_');
                this._DisplaySplitter.Add('/');
                this._DisplaySplitter.Add('&');
                this._DisplaySplitter.Add('+');
            }
            return this._DisplaySplitter;
        }

        #endregion

        public System.Collections.Generic.List<int> Groups
        {
            get
            {
                if (this._ChartGroups == null)
                    this._ChartGroups = new List<int>();
                return this._ChartGroups;
            }
        }

        #region Counting columns
        public int MaxColumnCount
        {
            get
            {
                return this.MaxColumnsPerPage();
                //return this._MaxColumnCount;
            }
        }


        /// <summary>
        /// Getting the next parent dataset that is a group or if none is found the topmost dataset of the hierarchy
        /// </summary>
        /// <param name="ID">The ID of the current ChartValue</param>
        /// <returns>The ChartValue representing the root</returns>
        //public ChartValue getChartGroupBase(int ID)
        //{
        //    if (this._ChartValues == null)
        //        this._ChartValues = new Dictionary<int, ChartValue>();

        //    if (this._ChartValues.ContainsKey(ID))
        //    {
        //        if (this._ChartValues[ID].ColumnPosition == 0)
        //            return this._ChartValues[ID];
        //        else
        //        {
        //            // get parent
        //            foreach (System.Collections.Generic.KeyValuePair<int, ChartValue> cv in this._ChartValues)
        //            {
        //                if (cv.Value.ChildrenIDs != null && cv.Value.ChildrenIDs.Contains(ID))
        //                {
        //                    //if (AutoGroups)
        //                    //{
        //                        //if (Groups.Contains(cv.Key))
        //                        //    return cv.Value;
        //                    //}
        //                    if (Groups.Contains(cv.Key))
        //                        return cv.Value;
        //                    //else if (cv.Value.IsGroup)
        //                    //    return cv.Value;
        //                    else
        //                        return getChartGroupBase(cv.Key);
        //                }
        //                else
        //                    continue;
        //            }
        //            ChartValue C = new ChartValue();
        //            return C;
        //        }
        //    }
        //    else
        //    {
        //        ChartValue C = new ChartValue();
        //        return C;
        //    }
        //}

        //public int MaxColumnCountGroup(int ID)
        //{
        //    int Count = 1;
        //    try
        //    {
        //        ChartValue Group = this.getChartValue(ID);
        //        if (Group.ChildrenIDs != null)
        //        {
        //            foreach (int c in Group.ChildrenIDs)
        //            {
        //                ChartValue CV = this.getChartValue(c);
        //                int cC = this.GroupColumnCount(CV.ID, Count);
        //                if (cC > Count)
        //                    Count = cC;
        //            }
        //        }
        //    }
        //    catch(System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return Count;
        //}

        /// <summary>
        /// Get the number of columns of a group where groups within the groups will not be followed for their depending values
        /// </summary>
        /// <param name="ID">The ID of the value starting the group</param>
        /// <param name="CurrentCount">The maximal number of columns found so far</param>
        /// <returns>The number of columns found for the group</returns>
        //private int GroupColumnCount(int ID, int CurrentCount)
        //{
        //    ChartValue Group = this.getChartValue(ID);
        //    System.Collections.Generic.Dictionary<int, int> ColumnCounts = new Dictionary<int, int>();
        //    int GroupCount = 0;
        //    try
        //    {
        //        if (Group.ChildrenIDs != null)
        //        {
        //            foreach (int c in Group.ChildrenIDs)
        //            {
        //                ChartValue cCV = this.getChartValue(c);
        //                bool IsGroup = this.Groups.Contains(cCV.ID); // cCV.IsGroup;
        //                //if (AutoGroups)
        //                //{
        //                //    if (this.Groups.Contains(cCV.ID))
        //                //        IsGroup = true;
        //                //    else
        //                //        IsGroup = false;
        //                //}
        //                if (IsGroup && GroupCount == 1)
        //                    GroupCount++;
        //                else
        //                {
        //                    int GCC = GroupColumnCount(cCV.ID, GroupCount);
        //                    if (GCC > GroupCount)
        //                        GroupCount = GCC;
        //                }
        //            }
        //        }
        //    }
        //    catch(System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return CurrentCount + GroupCount;
        //}

        #endregion

        #region Section

        /// <summary>
        /// If the chart is a section from the whole data
        /// </summary>
        public bool IsSection
        {
            get
            {
                return (this._SectionID != null && this._SectionID > 0);
            }
        }

        /// <summary>
        /// Search the hierarchy if any values are included in the current Section
        /// </summary>
        /// <param name="ID">The ID of the current value</param>
        /// <returns>If a value or a depending value is included in the current Section</returns>
        public bool ContainsSectionData(int ID)
        {
            bool Contained = false;
            if (this._ChartValues != null)
            {
                if (this._ChartSections != null && this._IDsInSection.Contains(ID))
                    return true;
                //if (this.getChartValue(ID).IsInSection)
                //    return true;
                if (this.getChartValue(ID).ChildrenIDs != null && getChartValue(ID).ChildrenIDs.Count > 0)
                {
                    foreach (int C in getChartValue(ID).ChildrenIDs)
                    {
                        if (this.ContainsSectionData(C))
                            return true;
                    }
                }
            }
            return Contained;
        }


        public int SectionStartColumn()
        {
            return 1;
        }

        #endregion

        #region Images

        public static System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<ChartImage>> getImages(System.Data.DataTable dtImages)
        {
            System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<ChartImage>> Images = new Dictionary<int, System.Collections.Generic.List<ChartImage>>();
            try
            {
                int Height = Chart.ImageMaxHeight;
                int Width = Chart.ImageMaxWidth;
                foreach (System.Data.DataRow R in dtImages.Rows)
                {
                    string Alt = "";
                    if (R.Table.Columns.Count > 2 && !R[2].Equals(System.DBNull.Value) && R[2].ToString().Length > 0)
                        Alt = R[2].ToString();
                    int ID;
                    if (!int.TryParse(R[0].ToString(), out ID))
                        continue;
                    if (R[1].Equals(System.DBNull.Value) || R[1].ToString().Length == 0)
                        continue;
                    //if (Images.ContainsKey(ID))
                    //    continue;
                    bool ImageAvailable = false;
                    string Path = R[1].ToString();
                    System.Drawing.Bitmap Bitmap = null;
                    if (Path.StartsWith("http"))
                    {
                        System.Uri U = new Uri(Path);
                        System.Net.WebRequest webrq = System.Net.WebRequest.Create(U);//ImageName);
                        webrq.Timeout = DiversityWorkbench.Settings.TimeoutWeb;
                        webrq.UseDefaultCredentials = true;
                        System.Net.WebResponse webResponse = webrq.GetResponse();
                        string ContentType = webResponse.Headers["Content-Type"].ToString();
                        string ContentTypeBase = ContentType.Substring(0, ContentType.IndexOf("/"));
                        long LengthOfUri = webResponse.ContentLength;
                        int SizeOfImage = 0;
                        SizeOfImage = (int)LengthOfUri / 1000;
                        if (SizeOfImage > DiversityWorkbench.Settings.MaximalImageSizeInKb)
                        {
                            Alt = DiversityWorkbench.Forms.FormFunctions.ImageTooBigMessage(SizeOfImage);
                        }
                        else
                        {
                            if (R.Table.Columns.Count > 2 && !R[2].Equals(System.DBNull.Value) && R[2].ToString().Length > 0)
                                Alt = R[2].ToString();
                            switch (ContentTypeBase.ToLower())
                            {
                                case "image":
                                    try
                                    {
                                        Bitmap = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromStream(webResponse.GetResponseStream());
                                        ImageAvailable = true;
                                    }
                                    catch { }
                                    break;
                            }
                        }
                        webResponse.Close();
                        webrq.Abort();
                    }
                    else
                    {
                        try
                        {
                            Bitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(Path);
                            ImageAvailable = true;
                        }
                        catch { }
                    }
                    if (ImageAvailable)//int.TryParse(R[0].ToString(), out ID) && )
                    {
                        ChartImage CI = new ChartImage();
                        CI.Source = Path;
                        if (Bitmap != null)
                        {
                            if (Bitmap.Width > Width)
                            {
                                double Faktor = (double)Chart.ImageMaxWidth / (double)Bitmap.Width;
                                Height = (int)((double)Bitmap.Height * Faktor);
                                Width = (int)((double)Bitmap.Width * Faktor);
                            }
                            else
                            {
                                Height = Bitmap.Height;
                                Width = Bitmap.Width;
                            }
                            CI.Height = Height;
                            CI.Width = Width;
                        }
                        else
                        {

                        }
                        if (Alt.Length > 0)
                            CI.Alt = Alt;
                        if (Images.ContainsKey(ID))
                            Images[ID].Add(CI);
                        else
                        {
                            System.Collections.Generic.List<ChartImage> L = new List<ChartImage>();
                            L.Add(CI);
                            Images[ID] = L;
                        }
                        //Images.Add(ID, CI);
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Images;
        }

        public ChartImage setImage(ref ChartValue CV, string Path, System.Drawing.Bitmap Bitmap = null)
        {
            ChartImage CI = new ChartImage();
            CI.Source = Path;
            int Height = Chart.ImageMaxHeight;
            int Width = Chart.ImageMaxWidth;
            if (Bitmap != null)
            {
                if (Bitmap.Width > Width)
                {
                    double Faktor = (double)Chart.ImageMaxWidth / (double)Bitmap.Width;
                    Height = (int)((double)Bitmap.Height * Faktor);
                    Width = (int)((double)Bitmap.Width * Faktor);
                }
                else
                {
                    Height = Bitmap.Height;
                    Width = Bitmap.Width;
                }
                CI.Height = Height;
                CI.Width = Width;
            }
            return CI;
        }


        #endregion

        #endregion

        #region Color

        public string ColorCode(System.Drawing.Color C, float Transparency = 0)
        {
            if (Transparency > 0)
            {
                C = DiversityWorkbench.Forms.FormFunctions.paleColor(C, Transparency);
            }
            string Code = System.Drawing.ColorTranslator.ToHtml(C);
            return Code;
        }

        public System.Drawing.Color ForeColor(int ID, float Transparency)
        {
            System.Drawing.Color C = System.Drawing.Color.Black;
            try
            {
                if (this._ForeColorCode != null && this._ForeColorCode.ContainsKey(ID))
                {
                    C = this._ForeColorCode[ID];
                    if (Transparency < (float)0.4)
                        C = DiversityWorkbench.Forms.FormFunctions.paleColor(C, (float)0.7);
                }
                else
                {
                    if (Transparency < (float)0.5)
                        C = System.Drawing.Color.White;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return C;
        }

        public System.Drawing.Color ForeColor(ChartValue CV, float Transparency)
        {
            System.Drawing.Color C = System.Drawing.Color.Black;
            try
            {
                if (this._ForeColorCode != null && this._ForeColorCode.ContainsKey(CV.ID))
                {
                    C = this._ForeColorCode[CV.ID];
                    if (Transparency < (float)0.4)
                        C = DiversityWorkbench.Forms.FormFunctions.paleColor(C, (float)0.7);
                }
                else
                {
                    float red = (255 - CV.ChartColor.R);
                    float green = (255 - CV.ChartColor.G);
                    float blue = (255 - CV.ChartColor.B);
                    float GreyScale = red + green + blue;
                    float threshold = (255 * 3) / 2;

                    if (GreyScale > threshold)
                        C = System.Drawing.Color.White;
                    else
                        C = System.Drawing.Color.Black;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return C;
        }

        private bool InheritColor(int ID)
        {
            return (this._ColorInheritance != null && this._ColorInheritance.Contains(ID));
        }

        private System.Drawing.Color ChartColor(int ID)
        {
            if (this._ColorCode.ContainsKey(ID))
                return this._ColorCode[ID];
            return System.Drawing.Color.Transparent;
        }

        private float ColorTransparency(ChartValue CV, int iRow)
        {
            float Transparency = 0;
            try
            {
                // get Transparency factor
                int iTrans = (int)System.Math.Abs(((iRow % _SectionTransparencyCycle) * 2) - (iRow % (_SectionTransparencyCycle * 2)));

                // get Transpareny part
                float TransPart = (float)1 / (float)(_SectionTransparencyCycle + MaxColumnCount + 1);

                // Change direction of Jitter depending on row
                int JitterDirection = 1;
                if (iRow % 2 == 1)
                    JitterDirection = -1;

                // Change size of Jitter depending on Column
                float Jitter = (TransPart / 8) * ((CV.ColumnPosition % 2) + 1);

                // get base for transparency according to column
                float TransBase = TransPart * (float)(CV.ColumnPosition % MaxColumnCount + 1);

                // get Transparency according to Row
                Transparency = TransBase + TransPart * (float)iTrans;

                // Apply Jitter
                Transparency = Transparency + (Jitter * JitterDirection);
                // only for testing
#if DEBUG
                //{
                //    if (this._SectionTranslist == null) this._SectionTranslist = new List<string>();
                //    this._SectionTranslist.Add(iRow.ToString() + " | " + CV.ColumnPosition.ToString() + " | " + Transparency.ToString() + " | " + TransBase.ToString() + " | " + Jitter.ToString() + " | " + CV.DisplayText);
                //}
#endif
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Transparency;
        }

        //public System.Drawing.Color ForeColor(System.Drawing.Color Backcolor)
        //{
        //    float red = Backcolor.R;
        //    float green = Backcolor.G;
        //    float blue = Backcolor.B;
        //    if (red + green + blue < 500)
        //        return System.Drawing.Color.White;
        //    else
        //        return System.Drawing.Color.Black;
        //}

        //public System.Drawing.Color ForeColor(int ID, System.Drawing.Color Backcolor)
        //{
        //    System.Drawing.Color C = System.Drawing.Color.Black;
        //    if (this._ForeColorCode.ContainsKey(ID))
        //        C = this._ForeColorCode[ID];
        //    float red = Backcolor.R;
        //    float green = Backcolor.G;
        //    float blue = Backcolor.B;
        //    if (red + green + blue < 500 && C == System.Drawing.Color.Black)
        //        return System.Drawing.Color.White;
        //    else
        //        C = DiversityWorkbench.Forms.FormFunctions.paleColor(C, (float)0.5);
        //    return C;
        //}

        //public bool InheritColor()
        //{
        //    return this._InheritColor;
        //}

        //public bool ColorDefined(int ID)
        //{
        //    return this._ColorCode.ContainsKey(ID);
        //}

        //public System.Drawing.Color ForeColor(int ID)
        //{
        //    if (this._ForeColorCode.ContainsKey(ID))
        //        return this._ForeColorCode[ID];
        //    return System.Drawing.Color.Transparent;
        //}

        #endregion

    }
}
