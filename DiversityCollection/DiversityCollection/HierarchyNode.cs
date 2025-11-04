using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityCollection
{
    class HierarchyNode : System.Windows.Forms.TreeNode
    {
        #region Parameter

        private System.Data.DataRow _DataRow;
        private bool _GreyImage;

        private static System.Collections.Generic.Dictionary<string, System.Drawing.Color> _TableColors;
        private static System.Collections.Generic.Dictionary<string, int> _TableImageIndex;

        #endregion

        #region Construction

        public HierarchyNode(System.Data.DataRow Row)
        {
            try
            {
                this.Tag = Row;
                this._DataRow = Row;
                this.setText();
                this.setColor();
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public HierarchyNode(System.Data.DataRow Row, bool GreyImage)
            : this(Row)
        {
            try
            {
                this.ImageIndex = DiversityCollection.Specimen.ImageIndex(Row, GreyImage);
                this._GreyImage = GreyImage;
                if (this._GreyImage) this.setColor();
                this.SelectedImageIndex = this.ImageIndex;
                if (GreyImage && this.ForeColor == System.Drawing.Color.Black)
                    this.ForeColor = System.Drawing.Color.Gray;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public HierarchyNode(bool DisplayOnly, System.Data.DataRow Row)
            : this(Row)
        {
            try
            {
                this.ImageIndex = DiversityCollection.Specimen.ImageIndex(Row, DisplayOnly);
                this.SelectedImageIndex = this.ImageIndex;
                this.ForeColor = System.Drawing.Color.Gray;
                this.setToolTip();
                this._DataRow = null;
                this.Tag = null;
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public HierarchyNode(bool DisplayOnly, System.Data.DataRow Row, int ImageIndex, bool GreyImage)
            : this(Row)
        {
            try
            {
                if (GreyImage) ImageIndex++;
                this._GreyImage = GreyImage;
                this.ImageIndex = ImageIndex;
                this.SelectedImageIndex = this.ImageIndex;
                if (GreyImage)
                    this.ForeColor = System.Drawing.Color.Gray;
                this.setToolTip();
                this._DataRow = null;
                this.Tag = null;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public HierarchyNode(System.Data.DataRow Row, bool GreyImage, System.Drawing.Font Font)
            : this(Row, GreyImage)
        {
            this.NodeFont = Font;
            this._GreyImage = GreyImage;
        }

        public HierarchyNode(System.Data.DataRow Row, int ImageIndex, bool GreyImage, System.Drawing.Font Font)
            : this(Row)
        {
            try
            {
                if (Font != null) this.NodeFont = Font;
                if (GreyImage) ImageIndex++;
                this._GreyImage = GreyImage;
                this.ImageIndex = ImageIndex;
                this.SelectedImageIndex = this.ImageIndex;
                if (GreyImage && this.ForeColor == System.Drawing.Color.Black)
                    this.ForeColor = System.Drawing.Color.Gray;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Color
        
        private void setColor()
        {
            try
            {
                string Table = this._DataRow.Table.TableName;
                switch (Table)
                {
                    case "CollectionEventList":
                        this.ForeColor = System.Drawing.Color.Black;
                        break;
                    case "CollectionSpecimen":
                        this.ForeColor = System.Drawing.Color.White;
                        this.BackColor = System.Drawing.Color.Black;
                        break;
                    case "Collection":
                        this.ForeColor = System.Drawing.Color.White;
                        if (this._DataRow.Table.Columns.Contains("LocationParentID") && !this._DataRow["LocationParentID"].Equals(System.DBNull.Value))
                        {
                            this.BackColor = System.Drawing.Color.SandyBrown;
                            //this.BackColor = System.Drawing.Color.RosyBrown;
                        }
                        else
                        {
                            //this.BackColor = System.Drawing.Color.Brown;
                            this.BackColor = System.Drawing.Color.SaddleBrown;
                        }
                        break;
                    case "SamplingPlot":
                        this.ForeColor = System.Drawing.Color.DarkRed;
                        break;
                    case "CollectionSpecimenList":
                        this.ForeColor = System.Drawing.Color.Gray;
                        break;
                    case "CollectionSpecimenRelationInternal":
                        this.ForeColor = System.Drawing.Color.Gray;
                        break;
                    case "CollectionSpecimenRelationInvers":
                        this.ForeColor = System.Drawing.Color.Gray;
                        break;
                    case "CollectionSpecimenTransaction":
                        this.ForeColor = System.Drawing.Color.Brown;
                        if (this._DataRow.Table.Columns.Contains("TransactionReturnID") && !this._DataRow["TransactionReturnID"].Equals(System.DBNull.Value) || this._GreyImage)
                        {
                            this.ForeColor = System.Drawing.Color.Gray;
                        }
                        int TransactionID;
                        if (int.TryParse(this._DataRow["TransactionID"].ToString(), out TransactionID))
                        {
                            if (!this._DataRow["TransactionTitle"].Equals(System.DBNull.Value))
                            {
                                {
                                    if (DiversityCollection.LookupTable.TransactionType(TransactionID) == "regulation")
                                    {
                                        this.ForeColor = System.Drawing.Color.White;
                                        System.Drawing.Color RegColor = System.Drawing.Color.FromArgb(System.Drawing.Color.Red.A, 255, 100, 130);
                                        this.BackColor = RegColor;
                                    }
                                }
                            }
                            if (DiversityCollection.LookupTable.TransactionType(TransactionID) == "warning")
                            {
                                this.ForeColor = System.Drawing.Color.Yellow;
                                this.BackColor = System.Drawing.Color.Red;
                            }
                        }
                        break;
                    case "Identification":
                        this.ForeColor = System.Drawing.Color.Black;
                        if (this._DataRow["TypeStatus"].ToString().Length > 0
                            && !this._DataRow["TypeStatus"].ToString().StartsWith("not "))
                            this.ForeColor = System.Drawing.Color.Red;
                        break;
                    case "IdentificationUnitList":
                        this.ForeColor = System.Drawing.Color.Gray;
                        break;
                    case "Annotation":
                        if (this._DataRow["AnnotationType"].ToString().Length > 0
                            && this._DataRow["AnnotationType"].ToString() == "Problem")
                            this.ForeColor = System.Drawing.Color.Red;
                        break;
                    case "ExternalIdentifier":
                        if (this._DataRow["Type"].ToString().Length > 0
                            //&& DiversityWorkbench.ReferencingTable.Regulations() != null
                            //&& DiversityWorkbench.ReferencingTable.Regulations().Contains(this._DataRow["Type"].ToString())
                            )
                        {
                            //if (this._DataRow["URL"].ToString().Length > 0)
                            //    this.ForeColor = System.Drawing.Color.Green;
                            //else
                            //    this.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            //System.Windows.Forms.MessageBox.Show("Regulation type is missing");
                        }
                        break;
                    case "SubcollectionContent":
                        this.ForeColor = System.Drawing.Color.Gray;
                        break;
                    case "Regulation":
                    case "CollectionSpecimenPartRegulation":
                    //this.ForeColor = System.Drawing.Color.Red;
                    //break;
                    case "CollectionEventRegulation":
                        //if (this._DataRow["Regulation"].Equals(System.DBNull.Value) || 
                        //    this._DataRow["Regulation"].ToString() == this._DataRow["RegulationID"].ToString() || 
                        //    this._DataRow["Regulation"].ToString().Length == 0)
                        this.ForeColor = System.Drawing.Color.White;
                        System.Drawing.Color RegulationColor = System.Drawing.Color.FromArgb(System.Drawing.Color.Red.A, 255, 100, 130);
                        //                    float red = (255 - color.R) * correctionFactor + color.R;
                        //float green = (255 - color.G) * correctionFactor + color.G;
                        //float blue = (255 - color.B) * correctionFactor + color.B;
                        //Color lighterColor = Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
                        //return lighterColor;

                        this.BackColor = RegulationColor;
                        //else
                        //    this.ForeColor = System.Drawing.Color.Green;
                        break;
                    case "CollectionEventParameterValueList":
                    case "CollectionEventMethodList":
                        this.ForeColor = System.Drawing.Color.Black;
                        break;
                    default:
                        // Markus 5.7.23: Bugfix for missing tables
                        if (DiversityCollection.HierarchyNode.TableColors.ContainsKey(Table))
                            this.ForeColor = DiversityCollection.HierarchyNode.TableColors[Table];
                        else
                        {
                            this.ForeColor = System.Drawing.Color.Black;
                        }
                        break;
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public static System.Collections.Generic.Dictionary<string, System.Drawing.Color> TableColors
        {
            get
            {
                // Markus: Adding missing table Analysis
                if (DiversityCollection.HierarchyNode._TableColors == null)
                {
                    DiversityCollection.HierarchyNode._TableColors = new Dictionary<string, System.Drawing.Color>();
                    DiversityCollection.HierarchyNode._TableColors.Add("Collection", System.Drawing.Color.Brown);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionAgent", System.Drawing.Color.Blue);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionAgent_Core", System.Drawing.Color.Blue);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionEvent", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionEventLocalisation", System.Drawing.Color.DarkBlue);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionEventMethod", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionEventParameterValue", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("Samplingplot", System.Drawing.Color.DarkBlue);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionEventProperty", System.Drawing.Color.DarkGreen);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionEventImage", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionEventRegulation", System.Drawing.Color.Red);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionEventSeries", System.Drawing.Color.Blue);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionEventSeriesDescriptor", System.Drawing.Color.Blue);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionEventSeriesImage", System.Drawing.Color.Blue);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionProject", System.Drawing.Color.Red);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionSpecimen", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionSpecimen_Core", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionSpecimen_Core2", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("Label", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionSpecimenImage", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionSpecimenPart", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionSpecimenPart_Core", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionSpecimenPartRegulation", System.Drawing.Color.Red);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionSpecimenPartDescription", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionSpecimenProcessing", System.Drawing.Color.Blue);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionSpecimenProcessingMethod", System.Drawing.Color.Blue);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionSpecimenProcessingMethodParameter", System.Drawing.Color.Blue);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionSpecimenReference", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionSpecimenRelation", System.Drawing.Color.DarkMagenta);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionSpecimenRelationInternal", System.Drawing.Color.Magenta);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionSpecimenRelationInvers", System.Drawing.Color.Gray);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionSpecimenRelationExternal", System.Drawing.Color.DarkMagenta);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionSpecimenTransaction", System.Drawing.Color.Brown);
                    DiversityCollection.HierarchyNode._TableColors.Add("Transaction", System.Drawing.Color.Brown);
                    DiversityCollection.HierarchyNode._TableColors.Add("Identification", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("Identification_Core", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("Identification_Core2", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("IdentificationUnit", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("IdentificationUnit_Core", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("IdentificationUnit_Core2", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("Analysis", System.Drawing.Color.Blue);
                    DiversityCollection.HierarchyNode._TableColors.Add("IdentificationUnitAnalysis", System.Drawing.Color.Blue);
                    DiversityCollection.HierarchyNode._TableColors.Add("IdentificationUnitAnalysisMethod", System.Drawing.Color.Blue);
                    DiversityCollection.HierarchyNode._TableColors.Add("IdentificationUnitAnalysisMethodParameter", System.Drawing.Color.Blue);
                    DiversityCollection.HierarchyNode._TableColors.Add("IdentificationUnitGeoAnalysis", System.Drawing.Color.Blue);
                    DiversityCollection.HierarchyNode._TableColors.Add("IdentificationUnitInPart", System.Drawing.Color.Gray);
                    DiversityCollection.HierarchyNode._TableColors.Add("ExternalIdentifier", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("Annotation", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("Method", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("Parameter", System.Drawing.Color.Black);
                    DiversityCollection.HierarchyNode._TableColors.Add("Task", System.Drawing.Color.Gray);
                    DiversityCollection.HierarchyNode._TableColors.Add("CollectionTask", System.Drawing.Color.Gray);
                }
                return DiversityCollection.HierarchyNode._TableColors;
            }
        }
        
        #endregion

        #region Image
        
        public static System.Collections.Generic.Dictionary<string, int> TableImageIndex
        {
            get
            {
                if (DiversityCollection.HierarchyNode._TableImageIndex == null)
                {
                    DiversityCollection.HierarchyNode._TableImageIndex = new Dictionary<string, int>();
                    foreach (System.Collections.Generic.KeyValuePair<string, System.Drawing.Color> KV in DiversityCollection.HierarchyNode.TableColors)
                    {
                        DiversityCollection.HierarchyNode._TableImageIndex.Add(KV.Key, DiversityCollection.Specimen.TableImage(KV.Key, false));
                    }
                }
                return DiversityCollection.HierarchyNode._TableImageIndex;
            }
        }

        private static System.Collections.Generic.Dictionary<string, int> _TableAndGroupImageIndex;
        public static System.Collections.Generic.Dictionary<string, int> TableAndGroupImageIndex
        {
            get
            {
                if (DiversityCollection.HierarchyNode._TableAndGroupImageIndex == null)
                {
                    DiversityCollection.HierarchyNode._TableAndGroupImageIndex = new Dictionary<string, int>();// DiversityCollection.HierarchyNode.TableImageIndex;
                    foreach (System.Collections.Generic.KeyValuePair<string, System.Drawing.Color> KV in DiversityCollection.HierarchyNode.TableColors)
                    {
                        DiversityCollection.HierarchyNode._TableAndGroupImageIndex.Add(KV.Key, DiversityCollection.Specimen.TableImage(KV.Key, false));
                    }
                    foreach(string s in DiversityCollection.Specimen.DisplayGroups)
                        DiversityCollection.HierarchyNode._TableAndGroupImageIndex.Add(s, DiversityCollection.Specimen.TableOrGroupImage(s, false));
                }
                return DiversityCollection.HierarchyNode._TableAndGroupImageIndex;
            }
        }

        #endregion

        #region Text
        
        public string setText()
        {
            if (this._DataRow.RowState == System.Data.DataRowState.Deleted || this._DataRow.RowState == System.Data.DataRowState.Detached) return "";
            string Table = this._DataRow.Table.TableName;
            string Title = "";
            string Date = "";
            string Method = "";
            int m = 0;
            bool AddEmptySpaceForBoldCharacters = false;
            try
            {
                switch (Table)
                {
                    case "Collection":
                        Title = this._DataRow["CollectionName"].ToString();
                        if (Title.Length == 0) Title = this._DataRow["CollectionAcronym"].ToString();
                        if (this._DataRow.Table.Columns.Contains("CollectionParentID"))
                        {
                            if (this._DataRow["CollectionParentID"].Equals(System.DBNull.Value) &&
                                !this._DataRow["CollectionOwner"].Equals(System.DBNull.Value) &&
                                this._DataRow["CollectionOwner"].ToString().Length > 0)
                                Title = this._DataRow["CollectionOwner"].ToString() + " (" + this._DataRow["CollectionName"].ToString() + ")";
                        }
                        else if (this._DataRow.Table.Columns.Contains("LocationParentID"))
                        {
                            if (this._DataRow["LocationParentID"].Equals(System.DBNull.Value) &&
                                !this._DataRow["CollectionOwner"].Equals(System.DBNull.Value) &&
                                this._DataRow["CollectionOwner"].ToString().Length > 0)
                                Title = this._DataRow["CollectionOwner"].ToString() + " (" + this._DataRow["CollectionName"].ToString() + ")";
                        }
                        break;
                    case "CollectionAgent":
                        Title = this._DataRow["CollectorsName"].ToString();
                        if (this._DataRow["CollectorsNumber"].ToString().Length > 0)
                            Title += " (No.: " + this._DataRow["CollectorsNumber"].ToString() + ")";
                        break;

                    #region Event
                    case "CollectionEvent":
                        Title = this._DataRow["LocalityDescription"].ToString();
                        if (!this._DataRow["CollectionYear"].Equals(System.DBNull.Value) ||
                           !this._DataRow["CollectionMonth"].Equals(System.DBNull.Value) ||
                           !this._DataRow["CollectionDay"].Equals(System.DBNull.Value))
                        {
                            // Markus 5.7.2023: IsoDate if possible
                            if (!this._DataRow["CollectionDate"].Equals(System.DBNull.Value))
                            {
                                Date = DiversityWorkbench.Forms.FormFunctions.IsoDate(this._DataRow["CollectionDate"].ToString());
                            }
                            else
                            {
                                if (!this._DataRow["CollectionYear"].Equals(System.DBNull.Value))
                                    Date = this._DataRow["CollectionYear"].ToString();
                                else Date = "-";
                                if (!this._DataRow["CollectionMonth"].Equals(System.DBNull.Value))
                                    Date += "/" + this._DataRow["CollectionMonth"].ToString();
                                else Date += "/-";
                                if (!this._DataRow["CollectionDay"].Equals(System.DBNull.Value))
                                    Date += "/" + this._DataRow["CollectionDay"].ToString();
                                else Date += "/-";
                            }
                        }
                        if (!this._DataRow["CollectionEndYear"].Equals(System.DBNull.Value) ||
                           !this._DataRow["CollectionEndMonth"].Equals(System.DBNull.Value) ||
                           !this._DataRow["CollectionEndDay"].Equals(System.DBNull.Value))
                        {
                            if (Date.Length > 0) Date += " - ";
                            if (!this._DataRow["CollectionEndYear"].Equals(System.DBNull.Value))
                                Date += this._DataRow["CollectionEndYear"].ToString();
                            else Date += "-";
                            if (!this._DataRow["CollectionEndMonth"].Equals(System.DBNull.Value))
                                Date += "/" + this._DataRow["CollectionEndMonth"].ToString();
                            else Date += "/-";
                            if (!this._DataRow["CollectionEndDay"].Equals(System.DBNull.Value))
                                Date += "/" + this._DataRow["CollectionEndDay"].ToString();
                            else Date += "/-";
                        }
                        else if (!this._DataRow["CollectionDateSupplement"].Equals(System.DBNull.Value) &&
                            this._DataRow["CollectionDateSupplement"].ToString().Length > 0)
                        {
                            if (Date.Length > 0) Date += " ";
                            Date += this._DataRow["CollectionDateSupplement"].ToString();
                        }
                        if (Date.Length > 0) Title = Date + " " + Title;
                        if (Title.Length == 0) Title = "[ID: " + this._DataRow["CollectionEventID"].ToString() + "]";
                        break;
                    case "CollectionEventList":
                        Title = this._DataRow["DisplayText"].ToString();
                        break;
                    case "CollectionEventImage":
                        Title = this._DataRow["URI"].ToString();
                        break;
                    case "CollectionEventLocalisation":
                        string Loc = "";
                        string Loc1 = "";
                        string Loc2 = "";
                        foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtLocalisationSystem.Rows)
                        {
                            if (this._DataRow["LocalisationSystemID"].ToString() == R["LocalisationSystemID"].ToString())
                            {
                                Method = R["ParsingMethodName"].ToString();
                                Loc = R["DisplayText"].ToString();
                                Loc1 = R["DisplayTextLocation1"].ToString();
                                Loc2 = R["DisplayTextLocation2"].ToString();
                                break;
                            }
                        }
                        switch (Method)
                        {
                            case "Exposition":
                            case "Slope":
                            case "Height":
                            case "Depth":
                            case "Altitude":
                                string Content = this._DataRow["Location1"].ToString();
                                Title = Loc;
                                if (Content != "New " + Loc)
                                    Title += ": " + Content;
                                float fTest = 0;
                                if (float.TryParse(this._DataRow["Location1"].ToString(), out fTest))
                                    Title += " " + DiversityCollection.LookupTable.LocalisationMeasurementUnit(int.Parse(this._DataRow["LocalisationSystemID"].ToString()));
                                if (!this._DataRow["Location2"].Equals(System.DBNull.Value) && this._DataRow["Location2"].ToString().Trim().Length > 0)
                                {
                                    Title += " - " + this._DataRow["Location2"].ToString();
                                    if (float.TryParse(this._DataRow["Location2"].ToString(), out fTest))
                                        Title += " " + DiversityCollection.LookupTable.LocalisationMeasurementUnit(int.Parse(this._DataRow["LocalisationSystemID"].ToString()));
                                }
                                break;
                            case "Coordinates":
                                Title = Loc;
                                if (!this._DataRow["Location1"].Equals(System.DBNull.Value)
                                    && this._DataRow["Location1"].ToString().Trim().Length > 0
                                    && !this._DataRow["Location1"].ToString().StartsWith("New "))
                                    Title += " " + Loc1 + ": " + this._DataRow["Location1"];
                                if (!this._DataRow["Location2"].Equals(System.DBNull.Value) && this._DataRow["Location2"].ToString().Trim().Length > 0)
                                    Title += " " + Loc2 + ": " + this._DataRow["Location2"];
                                Title = Title.Trim();
                                break;
                            case "SamplingPlot":
                            case "Gazetteer":
                                if (!this._DataRow["DistanceToLocation"].Equals(System.DBNull.Value) && this._DataRow["DistanceToLocation"].ToString().Trim().Length > 0)
                                    Title += " " + this._DataRow["DistanceToLocation"].ToString();
                                if (!this._DataRow["DirectionToLocation"].Equals(System.DBNull.Value) && this._DataRow["DirectionToLocation"].ToString().Trim().Length > 0)
                                    Title += " " + this._DataRow["DirectionToLocation"].ToString() + " of ";
                                if (Title.Length > 0) Title += " ";
                                Title += this._DataRow["Location1"];
                                break;
                            case "MTB":
                                Title = Loc;
                                if (!this._DataRow["Location1"].Equals(System.DBNull.Value) && this._DataRow["Location1"].ToString().Trim().Length > 0)
                                {
                                    Title = " " + Loc1 + ": " + this._DataRow["Location1"];
                                    if (!this._DataRow["Location2"].Equals(System.DBNull.Value) && this._DataRow["Location2"].ToString().Trim().Length > 0)
                                        Title += ", " + Loc2 + ": " + this._DataRow["Location2"];
                                }
                                Title = Title.Trim();
                                break;
                            default:
                                Title = Loc;
                                if (!this._DataRow["Location1"].Equals(System.DBNull.Value) && this._DataRow["Location1"].ToString().Trim().Length > 0)
                                {
                                    if (Title.Length > 0) Title += " ";
                                    if (Loc1.Length > 0) Title += Loc1 + ": ";
                                    Title += this._DataRow["Location1"];
                                }
                                if (!this._DataRow["Location2"].Equals(System.DBNull.Value) && this._DataRow["Location2"].ToString().Trim().Length > 0)
                                {
                                    if (Title.Length > 0) Title += " ";
                                    if (Loc2.Length > 0) Title += Loc2 + ": ";
                                    Title += this._DataRow["Location2"];
                                }
                                Title = Title.Trim();
                                break;
                        }
                        break;
                    case "CollectionEventProperty":
                        int PropertyID = 0;
                        if (int.TryParse(this._DataRow["PropertyID"].ToString(), out PropertyID))
                        {
                            Title = DiversityCollection.LookupTable.PropertyName(PropertyID);
                        }
                        if (!this._DataRow["DisplayText"].Equals(System.DBNull.Value)) Title = this._DataRow["DisplayText"].ToString() + " [" + Title + "]";
                        break;
                    case "CollectionEventSeries":
                        if (!this._DataRow["DateStart"].Equals(System.DBNull.Value))
                        {
                            // Markus 5.7.2023: Isodate if possibile
                            string SeriesStart = DiversityWorkbench.Forms.FormFunctions.IsoDate(this._DataRow["DateStart"].ToString(), true, true);
                            if (SeriesStart.Length > 0 && Title.Length > 0) Title += " ";
                            Title += SeriesStart;
                            //System.DateTime D;
                            //if (System.DateTime.TryParse(this._DataRow["DateStart"].ToString(), out D))
                            //    Title += D.Year.ToString() + "/" + D.Month.ToString() + "/" + D.Day.ToString() + ": ";
                        }
                        if (Title.Length > 0 && this._DataRow["Description"].ToString().Length > 0) Title += " ";
                        Title += this._DataRow["Description"].ToString();
                        if (Title.Length == 0) Title += this._DataRow["SeriesCode"].ToString();
                        break;
                    case "SamplingPlot":
                        Title += this._DataRow["DisplayText"].ToString();
                        if (Title.Length == 0) Title += this._DataRow["PlotID"].ToString();
                        break;
                    case "CollectionEventRegulation":
                        // Markus 4.12.2023: Umbau Regulation
                        Title = this._DataRow["Regulation"].ToString();
                        /*
                        Title = "ABS"; // DiversityCollection.LookupTable.RegulationType(this._DataRow["Regulation"].ToString());
                        bool OK = DiversityWorkbench.Forms.FormFunctions.Permissions("Regulation", "UPDATE");
                        if (OK)
                        {
                            if (Title.Length > 0)
                                Title += ": ";
                            Title += this._DataRow["Regulation"].ToString();
                        }
                        */

                        break;

                    #endregion

                    #region Specimen & Part

                    case "CollectionSpecimen":
                    case "CollectionSpecimenList":
                        Title = this._DataRow["AccessionNumber"].ToString();
                        if (Title.Length == 0) Title = "[ID: " + this._DataRow["CollectionSpecimenID"].ToString() + "]";
                        //m = Title.Length;
                        ////AddEmptySpaceForBoldCharacters = true;
                        //for (int i = 0; i < m; i++) Title += " ";
                        break;
                    case "CollectionSpecimenPart":
                        if (this._DataRow.Table.Columns.Contains("DisplayText") && !this._DataRow["DisplayText"].Equals(System.DBNull.Value) && this._DataRow["DisplayText"].ToString().Length > 0) // #294
                            Title = this._DataRow["DisplayText"].ToString();
                        else
                        { 
                            if (!this._DataRow["AccessionNumber"].Equals(System.DBNull.Value)) Title = this._DataRow["AccessionNumber"].ToString();
                            if (!this._DataRow["PartSublabel"].Equals(System.DBNull.Value))
                            {
                                if (this._DataRow["PartSublabel"].ToString().Length > 0)
                                {
                                    if (Title.Length > 0) Title += " - ";
                                    Title += this._DataRow["PartSublabel"].ToString();
                                }
                            }
                            if(!this._DataRow["StorageLocation"].Equals(System.DBNull.Value) &&
                                this._DataRow["StorageLocation"].ToString().Length > 0)
                            {
                                if (Title.Length > 0 &&
                                    this._DataRow["StorageLocation"].ToString().Length > 0)
                                    Title += " - ";
                                Title += this._DataRow["StorageLocation"].ToString();
                            }
                            // Markus 2024-05-06: MaterialCategory if other fields are empty
                            if (Title.Length == 0)
                            {
                                Title = this._DataRow["MaterialCategory"].ToString();
                            }
                            AddEmptySpaceForBoldCharacters = true;
                            //int l = (int)(Title.Length / 2);
                            //string Spacer = "";
                            //for (int i = 0; i < l; i++) Spacer += " ";
                            //Title += Spacer;
                        }
                        break;
                    case "CollectionSpecimenPartRegulation":
                        Title = this._DataRow["Regulation"].ToString();
                        break;
                    case "CollectionSpecimenProcessing":
                        foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtProcessing.Rows)
                        {
                            if (R["ProcessingID"].ToString() == this._DataRow["ProcessingID"].ToString())
                            {
                                Title = R["DisplayText"].ToString();
                                // Markus 5.7.23: IsoDate if possible
                                string Datum = "";
                                string Dauer = "";
                                if (!this._DataRow["ProcessingDate"].Equals(System.DBNull.Value))
                                    Datum = DiversityWorkbench.Forms.FormFunctions.IsoDate(this._DataRow["ProcessingDate"].ToString());
                                if (!this._DataRow["ProcessingDuration"].Equals(System.DBNull.Value))
                                    Dauer = DiversityWorkbench.Forms.FormFunctions.IsoDate(this._DataRow["ProcessingDuration"].ToString());
                                if (Datum.Length > 0)
                                {
                                    Title += ";  " + Datum;
                                }
                                if (Dauer.Length > 0)
                                {
                                    if (Datum.Length == 0)
                                        Title += "; ";
                                    if (Dauer.Length > 0)
                                    {
                                        if (Datum.Length > 0)
                                            Title += " - ";
                                        Title += Dauer;
                                    }
                                    else
                                    {
                                        if (Dauer.Length > 0)
                                        {
                                            Title += " ";
                                            if (Datum.Length > 0)
                                                Title += "(";
                                            Title += Dauer;
                                            if (Datum.Length > 0)
                                                Title += ")";
                                        }
                                        else
                                        {
                                            if (Datum.Length > 0)
                                                Title += " - ";
                                            Title += " - " + Dauer;
                                        }
                                    }
                                }


                                //if (!this._DataRow["ProcessingDate"].Equals(System.DBNull.Value))
                                //{
                                //    System.DateTime d;
                                //    if (System.DateTime.TryParse(this._DataRow["ProcessingDate"].ToString(), out d))
                                //    {
                                //        Title += ";  " + d.ToString("yyyy-MM-dd HH:mm:ss");
                                //    }
                                //}
                                //if (!this._DataRow["ProcessingDuration"].Equals(System.DBNull.Value))
                                //{
                                //    if (this._DataRow["ProcessingDate"].Equals(System.DBNull.Value))
                                //        Title += "; ";
                                //    System.DateTime d;
                                //    if (System.DateTime.TryParse(this._DataRow["ProcessingDuration"].ToString(), out d))
                                //    {
                                //        if (!this._DataRow["ProcessingDate"].Equals(System.DBNull.Value))
                                //            Title += " - ";
                                //        Title += d.ToString("yyyy-MM-dd HH:mm:ss"); ;
                                //    }
                                //    else
                                //    {
                                //        string Duration = this._DataRow["ProcessingDuration"].ToString();
                                //        if (DiversityWorkbench.Forms.FormFunctions.IsIsoFormatPeriod(Duration))
                                //        {
                                //            Title += " ";
                                //            if (!this._DataRow["ProcessingDate"].Equals(System.DBNull.Value))
                                //                Title += "(";
                                //Title += DiversityWorkbench.Forms.FormFunctions.IsoFormatPeriodDisplayText(Duration);
                                //            if (!this._DataRow["ProcessingDate"].Equals(System.DBNull.Value))
                                //                Title += ")";
                                //        }
                                //        else
                                //        {
                                //            if (!this._DataRow["ProcessingDate"].Equals(System.DBNull.Value))
                                //                Title += " - ";
                                //            Title += " - " + Duration;
                                //        }
                                //    }
                                //}
                                break;
                            }
                        }
                        break;
                    case "CollectionSpecimenRelation":
                        Title = this._DataRow["RelatedSpecimenDisplayText"].ToString();
                        if (!this._DataRow["RelationType"].Equals(System.DBNull.Value))
                            Title += " (" + this._DataRow["RelationType"].ToString() + ")";
                        if (!this._DataRow["RelatedSpecimenDescription"].Equals(System.DBNull.Value))
                        {
                            if (this._DataRow["RelatedSpecimenDescription"].ToString().Length > 20)
                                Title += ". " + this._DataRow["RelatedSpecimenDescription"].ToString().Substring(0, 20) + "...";
                            else
                                Title += ". " + this._DataRow["RelatedSpecimenDescription"].ToString();
                        }
                        if (!this._DataRow["RelatedSpecimenCollectionID"].Equals(System.DBNull.Value))
                            Title += " in " + DiversityCollection.LookupTable.CollectionName(int.Parse(this._DataRow["RelatedSpecimenCollectionID"].ToString()));
                        if (Title.Length == 0)
                        {
                            Title = this._DataRow["RelatedSpecimenURI"].ToString();
                        }
                        break;
                    case "CollectionSpecimenRelationInvers":
                        Title = this._DataRow["RelatedSpecimenDisplayText"].ToString();
                        if (Title.Length == 0)
                            Title = "[ID " + this._DataRow["CollectionSpecimenID"].ToString() + "]";
                        if (!this._DataRow["RelationType"].Equals(System.DBNull.Value))
                            Title += ". " + this._DataRow["RelationType"].ToString();
                        if (!this._DataRow["RelatedSpecimenCollectionID"].Equals(System.DBNull.Value))
                            Title += " in " + DiversityCollection.LookupTable.CollectionName(int.Parse(this._DataRow["RelatedSpecimenCollectionID"].ToString()));
                        break;
                    case "CollectionSpecimenReference":
                        Title = this._DataRow["ReferenceTitle"].ToString();
                        break;
                    case "CollectionSpecimenTransaction":
                        int TransactionID = 0;
                        if (int.TryParse(this._DataRow["TransactionID"].ToString(), out TransactionID))
                        {
                            Title = DiversityCollection.LookupTable.TransactionTitle(TransactionID);
                            string Type = DiversityCollection.LookupTable.TransactionType(TransactionID);
                            if (Type.ToLower() != "regulation")
                            { 
                                DiversityCollection.LookupTable.TransactionDisplaySorter Sorter = LookupTable.TransactionDisplaySorter.BeginDate;
                                if (DiversityCollection.LookupTable.TransactionDisplaySorting().ContainsKey(Type))
                                    Sorter = DiversityCollection.LookupTable.TransactionDisplaySorting()[Type];
                                else
                                    Sorter = DiversityCollection.LookupTable.TransactionDisplaySorterParent(TransactionID);
                                if (this._DataRow.Table.Columns.Contains("AccessionNumber") && !this._DataRow["AccessionNumber"].Equals(System.DBNull.Value))
                                {
                                    if (Sorter == LookupTable.TransactionDisplaySorter.AccessionNumber)
                                        Title = this._DataRow["AccessionNumber"].ToString() + "; " + Title;
                                    else
                                        Title += "; " + this._DataRow["AccessionNumber"].ToString();
                                }
                                if (Type == "warning")
                                {
                                    AddEmptySpaceForBoldCharacters = true;
                                }
                            }
                        }
                        break;

                    #endregion

                    #region Unit
                    case "Identification":
                        Title = this._DataRow["TaxonomicName"].ToString();
                        if (!this._DataRow["IdentificationQualifier"].Equals(System.DBNull.Value)
                            && this._DataRow["IdentificationQualifier"].ToString().Length > 0)
                        {
                            string Qualifier = this._DataRow["IdentificationQualifier"].ToString();
                            if (Title.Length > 0 && Qualifier.Length > 0 && !Title.EndsWith(Qualifier))
                            {
                                Title = this.Taxon(Title, Qualifier);
                            }
                        }
                        if (Title.Length == 0) Title = this._DataRow["VernacularTerm"].ToString();
                        if (this._DataRow["TypeStatus"].ToString().Length > 0
                            && !this._DataRow["TypeStatus"].ToString().StartsWith("not "))
                            Title += " - " + this._DataRow["TypeStatus"].ToString();
                        string Ident = "";
                        if (!this._DataRow["ResponsibleName"].Equals(System.DBNull.Value))
                        {
                            string User = this._DataRow["ResponsibleName"].ToString();
                            if (User.Length > 0)
                            {
                                string Prefix = "respons.:";
                                if (!this._DataRow["IdentificationCategory"].Equals(System.DBNull.Value))
                                {
                                    string Category = this._DataRow["IdentificationCategory"].ToString();
                                    switch (Category)
                                    {
                                        case "absence":
                                            Prefix = "absent, " + User;
                                            break;
                                        case "confirmation":
                                            Prefix = "conf. by " + User;
                                            break;
                                        case "correction":
                                            Prefix = "corr. by " + User;
                                            break;
                                        case "determination":
                                            Prefix = "det. by " + User;
                                            break;
                                        case "dubious":
                                            Prefix = "dubious, " + User;
                                            break;
                                        case "implicit":
                                            Prefix = "impl., " + User;
                                            break;
                                        case "negative":
                                            Prefix = "neg., " + User;
                                            break;
                                        case "preference":
                                            Prefix = "pref., " + User;
                                            break;
                                        case "renaming":
                                            Prefix = "renamed by " + User;
                                            break;
                                        case "revision":
                                            Prefix = "rev. by " + User;
                                            break;
                                        case "expert assignment":
                                            Prefix = "exp. assgn. by " + User;
                                            break;
                                        default:
                                            Ident += Prefix + " " + User;
                                            break;
                                    }
                                }
                                else
                                    Prefix += " " + User;
                                Ident += Prefix;
                            }
                        }
                        string IdentDate = "";
                        // Markus 5.7.2023: Isodate if possible
                        if(!(this._DataRow["IdentificationDate"].Equals(System.DBNull.Value)))
                        {
                            IdentDate = DiversityWorkbench.Forms.FormFunctions.IsoDate(this._DataRow["IdentificationDate"].ToString());
                        }
                        else if (!(this._DataRow["IdentificationYear"].Equals(System.DBNull.Value) &&
                            this._DataRow["IdentificationYear"].ToString().Length > 0 &&
                            this._DataRow["IdentificationYear"].ToString() != "0")
                            ||
                            (!this._DataRow["IdentificationMonth"].Equals(System.DBNull.Value) &&
                            this._DataRow["IdentificationMonth"].ToString().Length > 0 &&
                            this._DataRow["IdentificationMonth"].ToString() != "0")
                            ||
                            (!this._DataRow["IdentificationDay"].Equals(System.DBNull.Value) &&
                            this._DataRow["IdentificationDay"].ToString().Length > 0 &&
                            this._DataRow["IdentificationDay"].ToString() != "0"))
                        {
                            IdentDate = this._DataRow["IdentificationYear"].ToString() + "-" +
                                this._DataRow["IdentificationMonth"].ToString() + "-" +
                                this._DataRow["IdentificationDay"].ToString();
                        }
                        if (IdentDate.Replace("-", "").Length > 0 || Ident.Length > 0)
                        {
                            Title += "   [" + Ident;
                            if (IdentDate.Length > 0 && IdentDate != "0-0-0" && IdentDate != "--")
                                Title += "   Date: " + IdentDate;
                            Title += "]";
                        }
                        break;
                    case "IdentificationUnit":
                    case "IdentificationUnitList":
                        if (!this._DataRow["UnitIdentifier"].Equals(System.DBNull.Value))
                            Title = this._DataRow["UnitIdentifier"].ToString().Trim();
                        if (this._DataRow["LastIdentificationCache"].ToString() != this._DataRow["TaxonomicGroup"].ToString())
                        {
                            if (Title.Length > 0) Title += ": ";
                            Title += this._DataRow["LastIdentificationCache"].ToString();
                        }
                        if (Title.Length == 0)
                        {
                            if (this._DataRow["UnitDescription"].Equals(System.DBNull.Value))
                                Title = this._DataRow["TaxonomicGroup"].ToString();
                            else
                                Title = this._DataRow["UnitDescription"].ToString();
                            if (Title.Length == 0) Title = this._DataRow["TaxonomicGroup"].ToString();
                        }
                        if (this._DataRow.Table.Columns.Contains("RelationType"))
                        {
                            if (this._DataRow["RelationType"].ToString() == "Part of")
                                this.BackColor = System.Drawing.SystemColors.Control;
                        }
                        if (this._DataRow["Gender"].ToString().Length > 0)
                            Title += " [" + this._DataRow["Gender"].ToString() + "]";
                        //AddEmptySpaceForBoldCharacters = true;
                        m = Title.Length;
                        for (int i = 0; i < m; i++) Title += " ";
                        break;
                    //case "IdentificationUnitList":
                    //    Title = this._DataRow["LastIdentificationCache"].ToString();
                    //    if (Title.Length == 0)
                    //        Title = this._DataRow["TaxonomicGroup"].ToString();
                    //    m = Title.Length;
                    //    for (int i = 0; i < m; i++) Title += " ";
                    //    break;
                    case "IdentificationUnitAnalysis":
                        string MeasurementUnit = "";
                        foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtAnalysis.Rows)
                        {
                            if (R["AnalysisID"].ToString() == this._DataRow["AnalysisID"].ToString())
                            {
                                Title = R["DisplayText"].ToString();
                                MeasurementUnit = R["MeasurementUnit"].ToString();
                                break;
                            }
                        }
                        if (this._DataRow["AnalysisDate"].ToString().Length > 0)
                        {
                            // Markus 5.7.2023: No Time if 0
                            string DateAnalysis = DiversityWorkbench.Forms.FormFunctions.IsoDate(this._DataRow["AnalysisDate"].ToString());
                            if (Title.Length > 0)
                                Title += " (" + DateAnalysis + ") ";
                            else
                                Title = DateAnalysis + " ";
                        }
                        if (this._DataRow["AnalysisNumber"].ToString().Length > 0)
                        {
                            string Nr = this._DataRow["AnalysisNumber"].ToString();
                            if (!Title.EndsWith(" ")) Title += " ";
                            Title += Nr;
                            Title = Title.Trim();
                        }
                        bool IsSequence = false;
                        if (!this._DataRow["AnalysisResult"].Equals(System.DBNull.Value) && this._DataRow["AnalysisResult"].ToString().Length > 0)
                        {
                            string Result = this._DataRow["AnalysisResult"].ToString();
                            // Check if the analysis is a DNA-Sequence
                            if (Result.Length > 20)
                            {
                                Result = Result.Replace("A", "").Replace("T", "").Replace("C", "").Replace("G", "").Replace("U", "").Replace("-", "");
                                if (Result.Trim().Length == 0)
                                    IsSequence = true;
                            }
                        }
                        if (!IsSequence && Title.Length > 0 && !this._DataRow["AnalysisResult"].Equals(System.DBNull.Value) && this._DataRow["AnalysisResult"].ToString().Length > 0)
                            Title += ": ";
                        if (!IsSequence && !this._DataRow["AnalysisResult"].Equals(System.DBNull.Value) && this._DataRow["AnalysisResult"].ToString().Length > 0)
                        {
                            string Result = this._DataRow["AnalysisResult"].ToString();
                            if (DiversityCollection.LookupTable.AnalysisResultsAreRestrictedToList(int.Parse(this._DataRow["AnalysisID"].ToString())))
                                Result = DiversityCollection.LookupTable.AnalysisResultsDisplayText(int.Parse(this._DataRow["AnalysisID"].ToString()), Result);
                            if (Result.Length > 30) Result = Result.Substring(0, 28) + "...";
                            if (Analysis.TypeOfSequence(MeasurementUnit) == Analysis.SequenceType.None)
                                Title += Result + " " + MeasurementUnit;
                            else
                                Title += Result;

                        }
                        else if (MeasurementUnit.Length > 0)
                        {
                            if (Analysis.TypeOfSequence(MeasurementUnit) == Analysis.SequenceType.None)
                                Title += " [" + MeasurementUnit + "]";
                        }
                        else
                        {
                            string Exc = "";
                            string SQL = "SELECT M.DisplayText " +
                                "FROM  IdentificationUnitAnalysisMethod AS AM INNER JOIN " +
                                "Method AS M ON AM.MethodID = M.MethodID " +
                                "WHERE AM.CollectionSpecimenID = " + this._DataRow["CollectionSpecimenID"].ToString() +
                                " AND AM.IdentificationUnitID = " + this._DataRow["IdentificationUnitID"].ToString() +
                                " AND AM.AnalysisID = " + this._DataRow["AnalysisID"].ToString() +
                                " AND AM.AnalysisNumber = '" + this._DataRow["AnalysisNumber"].ToString() + "' " +
                                "GROUP BY AM.CollectionSpecimenID, AM.IdentificationUnitID, AM.AnalysisID, M.DisplayText " +
                                "HAVING (COUNT(*) = 1)";
                            string MethodParameter = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Exc);
                            if (MethodParameter.Length > 0)
                            {
                                SQL = "SELECT COUNT(*) " +
                                    "FROM  IdentificationUnitAnalysisMethod AS AM INNER JOIN " +
                                    "Method AS M ON AM.MethodID = M.MethodID " +
                                    "WHERE AM.CollectionSpecimenID = " + this._DataRow["CollectionSpecimenID"].ToString() +
                                    " AND AM.IdentificationUnitID = " + this._DataRow["IdentificationUnitID"].ToString() +
                                    " AND AM.AnalysisID = " + this._DataRow["AnalysisID"].ToString() +
                                    " AND AM.AnalysisNumber = '" + this._DataRow["AnalysisNumber"].ToString() + "'";
                                string MethodCount = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                                if (int.Parse(MethodCount) > 1)
                                {
                                    MethodParameter = MethodCount.ToString() + " meth. (" + MethodParameter + ", ...)";
                                }
                                else
                                {
                                    SQL = "SELECT ' ' + max(P.DisplayText) + ' ' + max(AP.Value) " +
                                        "FROM IdentificationUnitAnalysisMethod AS AM INNER JOIN " +
                                        "Method AS M ON AM.MethodID = M.MethodID INNER JOIN " +
                                        "IdentificationUnitAnalysisMethodParameter AS AP ON AM.CollectionSpecimenID = AP.CollectionSpecimenID AND AM.IdentificationUnitID = AP.IdentificationUnitID AND AM.MethodID = AP.MethodID AND " +
                                        "AM.AnalysisID = AP.AnalysisID AND AM.AnalysisNumber = AP.AnalysisNumber AND AM.MethodMarker = AP.MethodMarker INNER JOIN " +
                                        "Parameter AS P ON M.MethodID = P.MethodID AND AP.ParameterID = P.ParameterID AND AP.MethodID = P.MethodID " +
                                        "WHERE AM.CollectionSpecimenID = " + this._DataRow["CollectionSpecimenID"].ToString() +
                                        " AND AM.IdentificationUnitID = " + this._DataRow["IdentificationUnitID"].ToString() +
                                        " AND AM.AnalysisID = " + this._DataRow["AnalysisID"].ToString() +
                                        " AND AM.AnalysisNumber = '" + this._DataRow["AnalysisNumber"].ToString() + "' " +
                                        " GROUP BY AM.CollectionSpecimenID, AM.IdentificationUnitID, AM.AnalysisID " +
                                        " HAVING COUNT(*) = 1";
                                    string Parameter = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
                                    if (Parameter.Length > 0)
                                        MethodParameter += " = " + Parameter;
                                }
                            }
                            if (MethodParameter.Length > 0)
                            {
                                if (Title.Length > 0 && !Title.EndsWith(" "))
                                {
                                    if (Title.IndexOf(":") == -1) Title += ": ";
                                    else Title += ". ";
                                }
                                Title += MethodParameter.Trim();
                            }
                        }
                        if (Title.Length == 0 && this._DataRow["AnalysisID"].ToString() != "-1")
                        {
                            Title = "Analysis " + this._DataRow["AnalysisNumber"].ToString();
                            if (MeasurementUnit.Length > 0) Title += "[" + MeasurementUnit + "]";
                        }
                        break;
                    case "IdentificationUnitGeoAnalysis":
                        string AnalysisDate = this._DataRow["AnalysisDate"].ToString();
                        System.DateTime AD;
                        if (System.DateTime.TryParse(AnalysisDate, out AD))
                        {
                            if (AD.Year >= 9000 && !this._DataRow["Notes"].Equals(System.DBNull.Value) && this._DataRow["Notes"].ToString().Length > 0)
                            {
                                Title = this._DataRow["Notes"].ToString();
                            }
                            else
                            {
                                // Markus 5.7.2023: Isodate if possible
                                Title = DiversityWorkbench.Forms.FormFunctions.IsoDate(AD.ToString());
                                //.Year.ToString() + "-";
                                //if (AD.Month < 10)
                                //    Title += "0";
                                //Title += AD.Month.ToString() + "-";
                                //if (AD.Day < 10)
                                //    Title += "0";
                                //Title += AD.Day.ToString() + " ";
                                //if (AD.Hour < 10)
                                //    Title += "0";
                                //Title += AD.Hour.ToString() + ":";
                                //if (AD.Minute < 10)
                                //    Title += "0";
                                //Title += AD.Minute.ToString() + ":" + AD.Second.ToString();
                            }
                        }
                        else
                            Title = DiversityWorkbench.Forms.FormFunctions.IsoDate( this._DataRow["AnalysisDate"].ToString());                                 // Markus 5.7.2023: Isodate if possible
                        break;
                    case "IdentificationUnitInPart":
                        if (!this._DataRow["Description"].Equals(System.DBNull.Value) &&
                            this._DataRow["Description"].ToString().Length > 0)
                        {
                            Title = this._DataRow["Description"].ToString();
                        }
                        foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtIdentificationUnit.Rows)
                        {
                            if (R["IdentificationUnitID"].ToString() == this._DataRow["IdentificationUnitID"].ToString())
                            {
                                if (!R["UnitIdentifier"].Equals(System.DBNull.Value))
                                {
                                    if (Title.Length > 0) Title += " of";
                                    Title += R["UnitIdentifier"].ToString().Trim();
                                }
                                if (R["LastIdentificationCache"].ToString() != R["TaxonomicGroup"].ToString())
                                {
                                    if (Title.Length > 0) Title += ": ";
                                    Title += R["LastIdentificationCache"].ToString();
                                }
                                if (Title.Length == 0)
                                {
                                    if (R["UnitDescription"].Equals(System.DBNull.Value))
                                        Title = R["TaxonomicGroup"].ToString();
                                    else
                                        Title = R["UnitDescription"].ToString();
                                    if (Title.Length == 0) Title = R["TaxonomicGroup"].ToString();
                                }
                                if (R["Gender"].ToString().Length > 0)
                                    Title += " [" + R["Gender"].ToString() + "]";
                                if (R["RelationType"].ToString() == "Part of")
                                    this.BackColor = System.Drawing.SystemColors.Control;
                                //Title = R["LastIdentificationCache"].ToString();
                                break;
                            }
                        }
                        //AddEmptySpaceForBoldCharacters = true;
                        m = Title.Length;
                        for (int i = 0; i < m; i++) Title += " ";
                        break;
                    #endregion

                    case "Annotation":
                        string AnnotationType = this._DataRow["AnnotationType"].ToString();
                        Title = "";
                        switch (AnnotationType)
                        {
                            case "Annotation":
                            case "Problem":
                                if (!this._DataRow["Title"].Equals(System.DBNull.Value) && this._DataRow["Title"].ToString().Length > 0)
                                    Title = this._DataRow["Title"].ToString();
                                if (!this._DataRow["Annotation"].Equals(System.DBNull.Value) && this._DataRow["Annotation"].ToString().Length > 0)
                                    Title = this._DataRow["Annotation"].ToString();
                                break;
                            case "Reference":
                                if (!this._DataRow["ReferenceDisplayText"].Equals(System.DBNull.Value) && this._DataRow["ReferenceDisplayText"].ToString().Length > 0)
                                    Title = this._DataRow["ReferenceDisplayText"].ToString();
                                else if (!this._DataRow["Title"].Equals(System.DBNull.Value) && this._DataRow["Title"].ToString().Length > 0)
                                    Title = this._DataRow["ReferenceDisplayText"].ToString();
                                else if (!this._DataRow["Annotation"].Equals(System.DBNull.Value) && this._DataRow["Annotation"].ToString().Length > 0)
                                    Title = this._DataRow["Annotation"].ToString();
                                break;
                            default:
                                if (!this._DataRow["Annotation"].Equals(System.DBNull.Value) && this._DataRow["Annotation"].ToString().Length > 0)
                                {
                                    Title = this._DataRow["Annotation"].ToString();
                                    if (!Title.ToLower().StartsWith(AnnotationType.ToLower() + ":"))
                                    Title = AnnotationType + ": " + Title;
                                }
                                break;
                        }
                        if (Title.Length > 200)
                            Title = Title.Substring(0, 200) + "...";
                        else if (Title.Length == 0)
                            Title = AnnotationType;
                        break;
                    case "ExternalIdentifier":
                        Title = this._DataRow["Type"].ToString() + ": " + this._DataRow["Identifier"].ToString();;
                        break;
                    case "SubcollectionContent":
                        Title = this._DataRow["DisplayText"].ToString();
                        break;
                    case "Regulation":
                        Title = this._DataRow["Regulation"].ToString();
                        break;
                    case "CollectionEventMethodList":
                        Title = this._DataRow["DisplayText"].ToString();
                        break;
                    case "CollectionEventParameterValueList":
                        Title = this._DataRow["DisplayText"].ToString() + ": " + this._DataRow["Value"].ToString();
                        break;
                    case "CollectionTask":
                        Title = DiversityCollection.CollectionTaskDisplay.NodeText(this._DataRow);
                        break;
                    case "Transaction":
                        int _TransactionID;
                        if (int.TryParse(this._DataRow["TransactionID"].ToString(), out _TransactionID))
                        {
                            Title = DiversityCollection.LookupTable.TransactionTitle(_TransactionID);
                        }
                        break;
                    default:
                        break;
                }
                if (AddEmptySpaceForBoldCharacters)
                {
                    for (int i = 0; i < Title.Length; i++)
                    {
                        if (i % 3 == 0) Title += " ";
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.Text = Title;
            return Title;
        }

        //private string IsoDate(string DateAndTime)
        //{
        //    System.DateTime DT;
        //    if (System.DateTime.TryParse(DateAndTime, out DT))
        //    {
        //        if (DT.Hour == 0 && DT.Minute == 0 && DT.Second == 0 && DT.Millisecond == 0)
        //            DateAndTime = DT.ToString("yyyy-MM-dd");
        //    }
        //    return DateAndTime;
        //}

        private string Taxon(string Taxon, string Qualifier)
        {
            if (Qualifier.Length == 0) return Taxon;

            Taxon = Taxon.Trim();
            string _Taxon = Taxon;
            string Rank = "";
            string Genus = "";
            string Authors = "";
            string IntraSpecificEpithet = "";
            string SpeciesEpithet = "";
            string QualifierText = "";
            string QualifierRank = "";
            /*
            1. ? = vor dem Namen, ob Gattung oder intragenerische Namen

            2. aff. forma (bzw.für cf. forma das gleiche):
            Cribraria vulgaris aff. f. (cf. f.) similis (L.) Fr.

            aff. gen (cf. gen.):
            aff. (cf.) Cribraria (L.) Fr.

            aff. sp. (cf. sp.)
            Cribraria aff. (cf.) vulgaris (L.) Fr.

            aff. ssp. (cf. ssp.)
            Cribraria vulgaris aff. ssp. (cf. ssp.) similis (L.) Fr.

            aff. var. (cf. var.)
            Cribraria vulgaris aff. var. (cf. var.) similis (L.) Fr.

            3. agg. (Autoren weglassen)
            Cribraria splendens agg.
            Cribraria spendens ssp. vulgaris agg.

            4. s.l. und s.str. (mit Autoren, ansonsten wie unter 3.)
            Cribraria splendens (L.) Fr. s.str.
            Cribraria splendens (L.) Fr. s.l.
            Cribraria spendens ssp. vulgaris (L.) Fr. s.l..
            Cribraria spendens f. vulgaris (L.) Fr. s.str.

            5. sp. nur bei Gattungen (Autoren weglassen)
            Cribraria sp.

            6. sp. nov. hinten an Namen (mit oder ohne Autoren) oder besser weglassen, würde ich eigentlich meinen
            Cribraria splendens Mayr sp. nov.
            Cribraria spendens ssp. vulgaris sp. nov.
            Cribraria spendens f. vulgaris Mayr & Müller sp. nov. 
            */
            try
            {
                if (Taxon.Length > 0)
                {
                    if (Taxon.IndexOf(" ") > -1)
                    {
                        Rank = "sp.";
                        //W.WriteStartElement("Taxon");
                        Genus = Taxon.Substring(0, Taxon.IndexOf(" "));
                        //W.WriteElementString("Genus", Genus);
                        Taxon = Taxon.Substring(Taxon.IndexOf(" ")).Trim();
                        if (Taxon.StartsWith("(")
                            || Taxon.Substring(0, 1).ToUpper() == Taxon.Substring(0, 1))
                        {
                            Authors = Taxon;
                            Taxon = "";
                            if (Qualifier == "sp.")
                                Rank = Qualifier;
                            else
                                Rank = "gen.";
                        }
                        else
                        {
                            if (Taxon.IndexOf(" ") > -1)
                            {
                                SpeciesEpithet = Taxon.Substring(0, Taxon.IndexOf(" "));
                                //W.WriteElementString("SpeciesEpithet", SpeciesEpithet);
                                Taxon = Taxon.Substring(Taxon.IndexOf(" ")).Trim();
                                if (Taxon.IndexOf(" var. ") > 0 ||
                                    Taxon.IndexOf(" ssp. ") > 0 ||
                                    Taxon.IndexOf(" fm. ") > 0 ||
                                    Taxon.IndexOf(" subvar. ") > 0 ||
                                    Taxon.IndexOf(" f. ") > 0)
                                {
                                    if (Taxon.IndexOf(" var. ") > 0)
                                        Rank = "var.";
                                    else if (Taxon.IndexOf(" ssp. ") > 0)
                                        Rank = "ssp.";
                                    else if (Taxon.IndexOf(" fm. ") > 0)
                                        Rank = "fm.";
                                    else if (Taxon.IndexOf(" subvar. ") > 0)
                                        Rank = "subvar.";
                                    else if (Taxon.IndexOf(" f. ") > 0)
                                        Rank = "f.";
                                    Authors = Taxon.Substring(0, Taxon.IndexOf(" " + Rank + " ")).Trim();
                                    //W.WriteElementString("Authors", Authors);
                                    Taxon = Taxon.Substring(Taxon.IndexOf(" " + Rank + " ") + Rank.Length + 2).Trim();
                                    IntraSpecificEpithet = Taxon;
                                    //W.WriteElementString("IntraSpecificEpithet", Taxon);
                                }
                                else
                                {
                                    if (Taxon.IndexOf("var.") == 0 ||
                                        Taxon.IndexOf("ssp.") == 0 ||
                                        Taxon.IndexOf("fm.") == 0 ||
                                        Taxon.IndexOf("subvar.") == 0 ||
                                        Taxon.IndexOf("f.") == 0)
                                    {
                                        Rank = Taxon.Substring(0, Taxon.IndexOf(".") + 1);
                                        Taxon = Taxon.Substring(Taxon.IndexOf(".") + 1).Trim();
                                        if (Taxon.IndexOf(" ") > -1)
                                        {
                                            IntraSpecificEpithet = Taxon.Substring(0, Taxon.IndexOf(" "));
                                            Taxon = Taxon.Substring(Taxon.IndexOf(" ")).Trim();
                                        }
                                        else
                                        {
                                            IntraSpecificEpithet = Taxon;
                                            Taxon = "";
                                        }
                                        //W.WriteElementString("IntraSpecificEpithet", IntraSpecificEpithet);
                                    }
                                    if (Taxon.Length > 0 && Taxon.Substring(0, 1) == Taxon.Substring(0, 1).ToUpper())
                                        Authors = Taxon;
                                    //W.WriteElementString("Authors", Taxon);
                                    else if (Taxon.Length > 0)
                                    {
                                        IntraSpecificEpithet = Taxon;
                                        //W.WriteElementString("IntraSpecificEpithet", Taxon);
                                        Rank = "ssp.";
                                    }
                                    if (Authors.Length > 0 &&
                                        (Authors.EndsWith(";")
                                        || Authors.EndsWith(",")))
                                    {
                                        Authors = Authors.Substring(0, Authors.Length - 1);
                                    }
                                }
                            }
                            else
                                SpeciesEpithet = Taxon;
                            //W.WriteElementString("SpeciesEpithet", Taxon);
                            //W.WriteElementString("Rank", Rank);
                            //W.WriteEndElement();
                        }
                    }
                    else
                    {
                        Rank = "gen.";
                        Genus = Taxon;
                        //W.WriteStartElement("Taxon");
                        //W.WriteElementString("Genus", Taxon);
                        //W.WriteElementString("Rank", Rank);
                        //W.WriteEndElement();
                    }
                }

                switch (Qualifier)
                {
                    case "aff. forma":
                    case "aff. gen.":
                    case "aff. sp.":
                    case "aff. ssp.":
                    case "aff. var.":
                        QualifierRank = Qualifier.Substring(5);
                        QualifierText = "aff.";
                        //W.WriteElementString("QualifierText", "aff.");
                        //W.WriteElementString("QualifierRank", Qualifier.Substring(5));
                        break;
                    case "cf. forma":
                    case "cf. gen.":
                    case "cf. sp.":
                    case "cf. ssp.":
                    case "cf. var.":
                        QualifierRank = Qualifier.Substring(4);
                        QualifierText = "cf.";
                        //W.WriteElementString("QualifierText", "cf.");
                        //W.WriteElementString("QualifierRank", Qualifier.Substring(4));
                        break;
                    case "cf. hybrid":
                        QualifierRank = "hybrid";
                        QualifierText = "cf.";
                        break;
                    case "?":
                    case "agg.":
                    case "s. l.":
                    case "s. str.":
                    case "sp.":
                    case "sp. nov.":
                    case "spp.":
                        QualifierRank = "";
                        QualifierText = Qualifier;
                        //W.WriteElementString("QualifierText", Qualifier);
                        //W.WriteElementString("QualifierRank", "");
                        break;
                }
                Taxon = "";
                if (QualifierText == "?") Taxon = "? ";
                if ((QualifierRank == "gen." || QualifierRank == "hybrid") && (QualifierText == "aff." || QualifierText == "cf.")) Taxon = QualifierText + " ";
                Taxon += Genus + " ";
                if (QualifierRank == "sp." && QualifierText.Length > 0)
                    Taxon += QualifierText + " ";
                if (SpeciesEpithet.Length > 0)
                    Taxon += SpeciesEpithet + " ";
                if (QualifierRank != "sp." &&
                    QualifierRank != "gen." &&
                    QualifierRank.Length > 0 &&
                    QualifierText.Length > 0 &&
                    QualifierText != "?" &&
                    IntraSpecificEpithet != SpeciesEpithet)
                    Taxon += QualifierText + " ";
                if (IntraSpecificEpithet.Length == 0)
                {
                    if (Authors.Length > 0 && QualifierText != "agg." && QualifierText != "sp.")
                        Taxon += Authors + " ";
                    //else if (Authors.Length > 0 && Rank == "gen.")
                    //    Taxon += Authors;
                    //if (QualifierText.Length > 0 && QualifierText != "?")
                    //    Taxon += QualifierText;
                }
                else
                {
                    if (SpeciesEpithet == IntraSpecificEpithet && Authors.Length > 0 && QualifierText != "agg." && QualifierText != "sp.")
                        Taxon += Authors + " ";
                    if (QualifierRank != "sp." &&
                       QualifierRank != "gen." &&
                       QualifierRank.Length > 0 &&
                       QualifierText.Length > 0 &&
                       QualifierText != "?" &&
                       IntraSpecificEpithet == SpeciesEpithet)
                        Taxon += QualifierText + " ";
                    Taxon += IntraSpecificEpithet + " ";
                    if (SpeciesEpithet != IntraSpecificEpithet && Authors.Length > 0 && QualifierText != "agg." && QualifierText != "sp.")
                        Taxon += Authors + " ";
                }
                if (QualifierText.Length > 0 && QualifierRank.Length == 0 && QualifierText != "?")
                    Taxon += QualifierText;
            }
            catch (System.Exception ex) { }

            if (Taxon.Length == 0 || (Taxon.Length < _Taxon.Length && QualifierText != "agg." && QualifierText != "sp." && QualifierText != "?"))
                return _Taxon;
            else
                return Taxon;
        }

        public static string DisplayText(System.Data.DataRow R)
        {
            string Title = "";
            DiversityCollection.HierarchyNode N = new HierarchyNode(R);
            Title = N.Text;
            return Title;
        }
        
        #endregion

        #region ToolTip
        
        private void setToolTip()
        {
            try
            {
                string ToolTipText = "";
                foreach (System.Data.DataColumn C in this._DataRow.Table.Columns)
                {

                    if (!this._DataRow[C.ColumnName].Equals(System.DBNull.Value) &&
                        this._DataRow[C.ColumnName].ToString().Length > 0 &&
                        !C.ColumnName.EndsWith("ID") &&
                        !C.ColumnName.StartsWith("Location") &&
                        C.ColumnName != "Version" &&
                        C.ColumnName != "CollectorsSequence" &&
                        C.ColumnName != "CollectorsName" &&
                        C.ColumnName != "DisplayOrder" &&
                        C.ColumnName != "AccessionNumber" &&
                        C.ColumnName != "IsInternalRelationCache" &&
                        !C.ColumnName.StartsWith("Average"))
                    {
                        string Entity = this._DataRow.Table + "." + C.ColumnName;
                        string DisplayText = DiversityWorkbench.Entity.EntityInformation(Entity)[DiversityWorkbench.Entity.EntityInformationField.DisplayText.ToString()];
                        ToolTipText += DisplayText + ": " + this._DataRow[C.ColumnName].ToString() + "\r\n";
                    }
                }
                this.ToolTipText = ToolTipText;
            }
            catch (System.Exception ex) { }
        }
        
        #endregion
    }
}
