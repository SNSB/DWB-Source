using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Spreadsheet
{
    public class MapKeyObject
    {

        #region Parameter

        private MapColor _MapColor;
        private MapSymbol _MapSymbol;
        private Sheet _Sheet;

        // Positions of data in row
        private int _iGeographyPosition;
        private int _iGeographySymbolPosition;
        private int _iGeographyColorPosition;

        // Filter key
        private string _Key;
        public string Key() { return this._Key; }

        // Geography
        private string _GeographyValue;
        public string GeographyValue() { return this._GeographyValue; }

        // Color parameters
        private string _ColorValue;
        public string ColorValue() { return this._ColorValue; }
        private double _dColorSortingValue = -1;

        // Symbol parametes
        private string _SybolValue = "";
        public string SybolValue()
        {
            //if (this._SybolValue == null)
            //    return "";
            return this._SybolValue;
        }
        private int _iSymbolSortingPosition = -1;
        private double _dColorSortingValueAssociatedWithSymbol = -1;

        // Infos shown in map
        private System.Collections.Generic.List<string> _Infos;
        public System.Collections.Generic.List<string> Infos(System.Collections.Generic.Dictionary<string, string> DisplayedColumns = null)
        {
            if (this._Infos == null)
            {
                this._Infos = new System.Collections.Generic.List<string>();
                string Header = "";
                if (DisplayedColumns == null)
                {
                    foreach (System.Collections.Generic.KeyValuePair<int, DiversityWorkbench.Spreadsheet.DataColumn> DC in this._Sheet.SelectedColumns())
                    {
                        if (DC.Value.Type() == DataColumn.ColumnType.Data
                            && DC.Value.IsVisible
                            && !DC.Value.IsHidden)
                        {
                            string ColumnName = DC.Value.DisplayText;
                            int Width = DC.Value.Width / _InfoWidthConversionFaktor;
                            if (ColumnName.Length > Width)
                                ColumnName = ColumnName.Substring(0, Width);
                            if (ColumnName.Length < Width)
                            {
                                int l = Width - ColumnName.Length;
                                for (int i = 0; i < l; i++)
                                {
                                    ColumnName += " ";
                                }
                            }
                            ColumnName += "|";
                            Header += ColumnName;
                        }
                    }
                    if (Header.EndsWith("|"))
                        Header = Header.Substring(0, Header.Length - 1);
                }
                else
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DisplayedColumns)
                    {
                        if (Header.Length > 0)
                            Header += "|";
                        Header += KV.Value;
                    }
                }
                this._Infos.Add(Header);
            }
            return this._Infos;
        }
        private readonly int _InfoWidthConversionFaktor = 6;

        #endregion

        #region Construction

        public MapKeyObject(string Key, Sheet Sheet, System.Data.DataRow R, int iGeographyPosition, int iGeographySymbolPosition, int iGeographyColorPosition)
        {
            // setting the key filter
            this._Key = Key;

            // setting the sheet
            this._Sheet = Sheet;

            // setting the positions for the data in the Row
            this._iGeographyColorPosition = iGeographyColorPosition;
            this._iGeographyPosition = iGeographyPosition;
            this._iGeographySymbolPosition = iGeographySymbolPosition;

            // setting the initial values
            try
            {
                if (this._iGeographyColorPosition == -1)
                    this._ColorValue = "";
                else if (!R[this._iGeographyColorPosition].Equals(System.DBNull.Value))
                    this._ColorValue = R[this._iGeographyColorPosition].ToString();
                if (this._iGeographyPosition > -1 && !R[this._iGeographyPosition].Equals(System.DBNull.Value))
                    this._GeographyValue = R[this._iGeographyPosition].ToString();
                if (!R[this._iGeographySymbolPosition].Equals(System.DBNull.Value))
                {
                    this._SybolValue = R[this._iGeographySymbolPosition].ToString();
                }
                else
                    this._SybolValue = "";
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Handling content

        public bool UpdateContent(System.Data.DataRow R)
        {
            bool OK = true;
            foreach (System.Collections.Generic.KeyValuePair<int, MapFilter> MF in this._Sheet.MapFilterList)
            {
                switch (MF.Value.FilterType())
                {
                    case MapFilter.FilterTypes.Geography:
                        if (this._GeographyValue == null && MF.Value.GeographyFilterType() == MapFilter.GeographyFilterTypes.CenterOfQuadrant)
                        {
                            string TK25 = this._Key.Substring(0, 4);
                            string Quadrant = this._Key.Substring(5, 1);
                            string Geography = "";
                            float Latitude = 0;
                            float Longitude = 0;
                            if (DiversityWorkbench.Forms.FormTK25.GetGeographyOfTK25(TK25, Quadrant, MF.Value.GazetteerConnection(), ref Geography, ref Latitude, ref Longitude))
                            {
                                if (Latitude != 0 && Longitude != 0)
                                {
                                    string QuadrantCenter = "POINT (" + Longitude.ToString() + " " + Latitude.ToString() + ")";
                                    this._GeographyValue = QuadrantCenter;
                                }
                            }
                            else if (MF.Value.ForwardType == MapFilter.ForwardTypes.RestrictToSuccess)
                            {
                                return OK;
                            }
                        }
                        break;
                    case MapFilter.FilterTypes.Color:
                        if (this._ColorValue == null)
                        {
                            if (this._iGeographyColorPosition == -1)
                            {
                                //MapColor M = this._Sheet.getMapColor("");
                                this._ColorValue = "";
                            }
                            else
                                this._ColorValue = R[this._iGeographyColorPosition].ToString();
                        }
                        else
                        {
                            if (this._iGeographyColorPosition > -1)
                            {
                                if (!R[this._iGeographyColorPosition].Equals(System.DBNull.Value) && R[this._iGeographyColorPosition].ToString().Length > 0)
                                {
                                    string NewColorValue = R[this._iGeographyColorPosition].ToString();
                                    if (MF.Value.ColorFilterType() == MapFilter.ColorFilterTypes.Maximum)
                                    {
                                        bool DoneWithNumeric = true;
                                        try
                                        {
                                            double dPresent = double.Parse(this._ColorValue);
                                            double dNew = double.Parse(NewColorValue);
                                            if (dPresent < dNew)
                                            {
                                                // if the symbol decision is set after the color the symbol must be reset to get the symbol from the current row
                                                // but this should only happen when a new color range is reached
                                                int iCurrentColorRange = -1;
                                                int iNewColorRange = -1;
                                                for (int i = 0; i < this._Sheet.MapColors().Count; i++)// DiversityWorkbench.Spreadsheet.MapColor MD in this._Sheet.MapColors())
                                                {
                                                    DiversityWorkbench.Spreadsheet.MapColor MD = this._Sheet.MapColors()[i];
                                                    float fLower = 0;
                                                    float fUpper = 0;
                                                    float fColorCurrent = 0;
                                                    float fColorNew = 0;
                                                    if (MD.Operator == "-")
                                                        float.TryParse(MD.UpperValue, out fUpper);
                                                    float.TryParse(MD.LowerValue, out fLower);
                                                    float.TryParse(this._ColorValue, out fColorCurrent);
                                                    float.TryParse(NewColorValue, out fColorNew);
                                                    switch (MD.Operator)
                                                    {
                                                        case "-":
                                                            if (fColorCurrent >= fLower && fColorCurrent <= fUpper)
                                                                iCurrentColorRange = i;
                                                            if (fColorNew >= fLower && fColorNew <= fUpper)
                                                                iNewColorRange = i;
                                                            break;
                                                        case "<":
                                                            if (fColorCurrent < fLower)
                                                                iCurrentColorRange = i;
                                                            if (fColorNew < fLower)
                                                                iNewColorRange = i;
                                                            break;
                                                        case ">":
                                                            if (fColorCurrent > fLower)
                                                                iCurrentColorRange = i;
                                                            if (fColorNew > fLower)
                                                                iNewColorRange = i;
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                }
                                                // setting the new value for the color
                                                this._ColorValue = NewColorValue;
                                                if (!R[this._iGeographySymbolPosition].Equals(System.DBNull.Value) && R[this._iGeographySymbolPosition].ToString().Length > 0)
                                                {
                                                    // only remove symbol value if a new color range is reached and new color range is lower (more imporant) than old
                                                    if (iNewColorRange < iCurrentColorRange)
                                                    {
                                                        foreach (System.Collections.Generic.KeyValuePair<int, MapFilter> MFcheck in this._Sheet.MapFilterList)
                                                        {
                                                            if (MFcheck.Key > MF.Key
                                                                && MFcheck.Value.FilterType() == MapFilter.FilterTypes.Symbol)
                                                            {
                                                                this._SybolValue = null;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else if (dPresent > dNew)
                                            {
                                                if (this._SybolValue == "")
                                                {
                                                    // if the value for the symbol has not been set so far and a line with a value within the same range of the current value appears
                                                    bool CheckSymbol = false;
                                                    for (int i = 0; i < this._Sheet.MapColors().Count; i++)// DiversityWorkbench.Spreadsheet.MapColor MD in this._Sheet.MapColors())
                                                    {
                                                        DiversityWorkbench.Spreadsheet.MapColor MD = this._Sheet.MapColors()[i];
                                                        if (MD.Operator == "-")
                                                        {
                                                            try
                                                            {
                                                                if (dPresent >= double.Parse(MD.LowerValue) &&
                                                                    dPresent <= double.Parse(MD.UpperValue) &&
                                                                    dNew >= double.Parse(MD.LowerValue) &&
                                                                    dNew <= double.Parse(MD.UpperValue))
                                                                {
                                                                    CheckSymbol = true;
                                                                    break;
                                                                }
                                                            }
                                                            catch { }
                                                        }
                                                    }
                                                    if (!CheckSymbol && MF.Value.ForwardType == MapFilter.ForwardTypes.RestrictToSuccess)
                                                        return OK;
                                                }
                                                else
                                                {
                                                    if (MF.Value.ForwardType == MapFilter.ForwardTypes.RestrictToSuccess)
                                                    {
                                                        return OK;
                                                    }
                                                }
                                            }
                                        }
                                        catch (System.Exception ex)
                                        {
                                            DoneWithNumeric = false;
                                        }
                                        if (!DoneWithNumeric)
                                        {

                                        }
                                    }
                                }
                                else if (MF.Value.ForwardType == MapFilter.ForwardTypes.RestrictToSuccess)
                                {
                                    return OK;
                                }
                            }
                        }
                        break;
                    case MapFilter.FilterTypes.Symbol:
                        if (!R[this._iGeographySymbolPosition].Equals(System.DBNull.Value) && R[this._iGeographySymbolPosition].ToString().Length > 0)
                        {
                            if (this._SybolValue == "")
                            {
                                this._SybolValue = R[this._iGeographySymbolPosition].ToString();
                            }
                            else
                            {
                                if (!R[this._iGeographySymbolPosition].Equals(System.DBNull.Value) && R[this._iGeographySymbolPosition].ToString().Length > 0)
                                {
                                    string NewSymbolValue = R[this._iGeographySymbolPosition].ToString();
                                    if (MF.Value.SymbolValueSequence()[NewSymbolValue] < MF.Value.SymbolValueSequence()[this._SybolValue])
                                        this._SybolValue = NewSymbolValue;
                                }
                                else if (MF.Value.ForwardType == MapFilter.ForwardTypes.RestrictToSuccess)
                                {
                                    return OK;
                                }
                            }
                        }
                        break;
                }
            }
            return OK;
        }

        private enum SymbolState { NeuErmitteln, Vergleichen, KeineAenderung }

        /// <summary>
        /// evaluation of the content for e.g. TK25 maps
        /// </summary>
        /// <param name="R">The Row containing the data that should be evaluated</param>
        /// <returns>if the evaluation was successful</returns>
        public bool EvaluateContent(System.Data.DataRow R, System.Collections.Generic.Dictionary<string, string> DisplayedColumns = null)
        {
            bool OK = true;

            // Setting the infos as shown as tooltip in the map
            this.SetInfos(R, DisplayedColumns);

            // set Geography if missing
            this.EvaluteGeography(R);

            // COLOR
            // getting the value for the color
            double RowColorSortingValue = this.RowValueForColorSortingPosition(R);

            // if the color value in the row is higher than the current value, take the color value from the row
            if (RowColorSortingValue > this._dColorSortingValue)
            {
                this._dColorSortingValue = RowColorSortingValue;
                this._ColorValue = RowColorSortingValue.ToString();
            }

            // SYMBOL
            // getting the value for the symbol from the row
            string RowSymbolValue = this.RowValueForSymbol(R);

            // getting the sorting position for the value as defined by the user
            int iRowSymbolSortingValue = this.RowValueForSymbolSortingPosition(RowSymbolValue);

            if (RowColorSortingValue > this._dColorSortingValueAssociatedWithSymbol) // if the sorting value is higher set the symbol in any case
            {
                bool SetSymbol = true;
                if (RowSymbolValue.Length == 0 && this._Sheet.KeepLastSymbol) // there is no value in the row and the last value should be kept
                    SetSymbol = false;
                if (SetSymbol)
                {
                    this._SybolValue = RowSymbolValue;
                    this._dColorSortingValueAssociatedWithSymbol = RowColorSortingValue;
                    this._iSymbolSortingPosition = iRowSymbolSortingValue;
                }
            }
            else if (RowColorSortingValue == this._dColorSortingValueAssociatedWithSymbol) // same sorting value - compare sequence of symbol
            {
                if (iRowSymbolSortingValue < this._iSymbolSortingPosition && iRowSymbolSortingValue > -1)
                {
                    this._SybolValue = RowSymbolValue;
                    this._iSymbolSortingPosition = iRowSymbolSortingValue;
                }
            }

            return OK;




            // if the _SybolValue has not been set so far and the row contains a value
            if (this._SybolValue == "" && RowSymbolValue.Length > 0)
                this._SybolValue = RowSymbolValue;

            // getting infos about the symbol
            SymbolState _SymbolState = SymbolState.KeineAenderung; // set the State to neutral as a start

            string ColorValueOfRow = "";
            // Check if the color must be changed
            try
            {
                // The colors are not set
                if (this._iGeographyColorPosition == -1)
                {
                    ColorValueOfRow = "";
                    if (this._Sheet.KeepLastSymbol)
                    {
                        if (RowSymbolValue.Length > 0)
                        {
                            if (this._SybolValue.Length > 0)
                                _SymbolState = SymbolState.Vergleichen;
                            else
                                _SymbolState = SymbolState.NeuErmitteln;
                        }
                    }
                    else
                    {
                        _SymbolState = SymbolState.NeuErmitteln;
                    }
                }
                else // for every color in the list, check if the color corresponding to the row is below the range of the current color and if so no changes are needed
                {
                    ColorValueOfRow = R[this._iGeographyColorPosition].ToString();
                    double iSortingValueCurrent = -1;
                    double iSortingValueRow = -1;

                    // finding the sorting positions of the current color and the color correspondig to the row
                    foreach (MapColor MC in this._Sheet.MapColors())
                    {
                        string CurrentColorValue = this.ColorValue();
                        if (MC.ValueIsInRange(CurrentColorValue))
                            iSortingValueCurrent = MC.SortingValue();
                        if (MC.ValueIsInRange(ColorValueOfRow))
                            iSortingValueRow = MC.SortingValue();
                        if (MC.LowerValue == ColorValueOfRow && iSortingValueRow == -1)// && MC.SortingValueFixed())
                        {
                            iSortingValueRow = MC.SortingValue();
                        }
                    }

                    // Check if symbol and color must be changed
                    if (iSortingValueRow > iSortingValueCurrent) // the new sorting value is higher, so the symbol must be checked
                    {
                        // set the color
                        if (RowSymbolValue.Length > 0 || this._Sheet.KeepLastSymbol)
                        {
                            this._ColorValue = ColorValueOfRow;
                            _SymbolState = SymbolState.NeuErmitteln;
                        }
                        else if (this._Sheet.GeographySymbolColumn.Length == 0)
                        {
                            int i;
                            if (int.TryParse(ColorValueOfRow, out i))
                                this._ColorValue = ColorValueOfRow;
                        }
                    }
                    else if (iSortingValueRow == iSortingValueCurrent)
                    {
                        if (this._dColorSortingValueAssociatedWithSymbol < iSortingValueRow && RowSymbolValue.Length > 0 && this._SybolValue.Length == 0)
                            _SymbolState = SymbolState.NeuErmitteln;
                        else
                            _SymbolState = SymbolState.Vergleichen;
                    }
                    else if (iSortingValueRow < iSortingValueCurrent
                        && this._SybolValue.Length == 0
                        && RowSymbolValue.Length > 0
                        && this._Sheet.KeepLastSymbol)
                    {
                        _SymbolState = SymbolState.NeuErmitteln;
                    }
                    else if (this._dColorSortingValueAssociatedWithSymbol > -1
                        && this._dColorSortingValueAssociatedWithSymbol < iSortingValueRow)
                    {
                        _SymbolState = SymbolState.NeuErmitteln;
                    }
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
            }

            // Check if the symbol must be changed
            if (_SymbolState != SymbolState.KeineAenderung)
            {
                try
                {
                    // if the new symbol is in the list, while the old is not, take the new symbol
                    if (this._Sheet.EvaluationSymbolValueSequence().ContainsKey(RowSymbolValue)
                        && !this._Sheet.EvaluationSymbolValueSequence().ContainsKey(this._SybolValue)
                        && RowSymbolValue.Length > 0)
                    {
                        this._SybolValue = RowSymbolValue;
                        double.TryParse(ColorValueOfRow, out this._dColorSortingValueAssociatedWithSymbol);
                    }
                    else
                    {
                        if (_SymbolState == SymbolState.NeuErmitteln)
                        {
                            if (RowSymbolValue.Length > 0
                                || !this._Sheet.KeepLastSymbol)
                            {
                                this._SybolValue = RowSymbolValue;
                                double.TryParse(ColorValueOfRow, out this._dColorSortingValueAssociatedWithSymbol);
                            }
                            else
                            {
                            }
                        }
                        else if (_SymbolState == SymbolState.Vergleichen)
                        {
                            if (this._Sheet.EvaluationSymbolValueSequence().Count > 0)
                            {
                                if (RowSymbolValue.Length > 0
                                    || !this._Sheet.KeepLastSymbol)
                                {
                                    if (this._Sheet.EvaluationSymbolValueSequence().ContainsKey(RowSymbolValue) &&
                                        this._Sheet.EvaluationSymbolValueSequence().ContainsKey(this._SybolValue))
                                    {
                                        if (this._Sheet.EvaluationSymbolValueSequence()[RowSymbolValue] < this._Sheet.EvaluationSymbolValueSequence()[this._SybolValue])
                                        {
                                            this._SybolValue = RowSymbolValue;
                                            double.TryParse(ColorValueOfRow, out this._dColorSortingValueAssociatedWithSymbol);
                                        }
                                    }
                                    else if (this._Sheet.EvaluationSymbolValueSequence().ContainsKey(RowSymbolValue) &&
                                        !this._Sheet.EvaluationSymbolValueSequence().ContainsKey(this._SybolValue))
                                    {
                                        this._SybolValue = RowSymbolValue;
                                        double.TryParse(ColorValueOfRow, out this._dColorSortingValueAssociatedWithSymbol);
                                    }
                                    else if (!this._Sheet.EvaluationSymbolValueSequence().ContainsKey(RowSymbolValue) &&
                                        !this._Sheet.EvaluationSymbolValueSequence().ContainsKey(this._SybolValue) &&
                                        this._Sheet.GeographyColorColumnPosition() == null)
                                    {
                                        this._SybolValue = RowSymbolValue;
                                    }
                                }
                            }
                            else
                            {
                                if (this._Sheet.KeepLastSymbol)
                                {
                                    if (RowSymbolValue.Length > 0)
                                        this._SybolValue = RowSymbolValue;
                                }
                                else
                                {
                                    this._SybolValue = RowSymbolValue;
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                }
            }
            return OK;
        }

        private string RowValueForSymbol(System.Data.DataRow R)
        {
            string NewValueForSymbol = "";
            if (this._iGeographySymbolPosition > -1)
            {
                if (!R[this._iGeographySymbolPosition].Equals(System.DBNull.Value))
                    NewValueForSymbol = R[this._iGeographySymbolPosition].ToString().Trim();
            }
            if (!this._Sheet.EvaluationSymbolValueSequence().ContainsKey(NewValueForSymbol)
                && this._Sheet.EvaluationSymbolValueSequence().ContainsKey(NewValueForSymbol.ToLower()))
                NewValueForSymbol = NewValueForSymbol.ToLower();
            else if (!this._Sheet.EvaluationSymbolValueSequence().ContainsKey(NewValueForSymbol)
                && this._Sheet.EvaluationSymbolValueSequence().ContainsKey(NewValueForSymbol.ToUpper()))
                NewValueForSymbol = NewValueForSymbol.ToUpper();
            return NewValueForSymbol;
        }

        private double RowValueForColorSortingPosition(System.Data.DataRow R)
        {
            double Position = -1;
            double MissingCollorPosition = -1;
            // The colors are  set
            if (this._iGeographyColorPosition > -1)
            {
                string ColorValueOfRow = R[this._iGeographyColorPosition].ToString();

                // finding the sorting positions of the color correspondig to the row
                foreach (MapColor MC in this._Sheet.MapColors())
                {
                    if (MC.ValueIsInRange(ColorValueOfRow))
                    {
                        Position = MC.SortingValue();
                        break; // Markus 17.9.2019 - wenn Zeitraum gefunden ist Rest irrelavant
                    }
                    if (MC.LowerValue == ColorValueOfRow && Position == -1)
                    {
                        Position = MC.SortingValue();
                    }
                    if (MissingCollorPosition == -1)
                        MissingCollorPosition = MC.SortingValue();
                    if (MissingCollorPosition > MC.SortingValue())
                        MissingCollorPosition = MC.SortingValue();
                }
            }
            if (Position == -1)
                Position = MissingCollorPosition;
            return Position;
        }

        private int RowValueForSymbolSortingPosition(string Value)
        {
            int Position = -1;
            try
            {
                if (this._Sheet.EvaluationSymbolValueSequence().ContainsKey(Value))
                    Position = this._Sheet.EvaluationSymbolValueSequence()[Value];
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Position;
        }

        private void EvaluteGeography(System.Data.DataRow R)
        {
            // set Geography if missing
            if (this._GeographyValue == null)
            {
                try
                {
                    DiversityWorkbench.ServerConnection C = null;
                    foreach (System.Collections.Generic.KeyValuePair<string, ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnections())
                    {
                        if (this._Sheet.EvaluationGazetteer() == KV.Value.DisplayText)
                        {
                            C = KV.Value;
                            break;
                        }
                        C = KV.Value;
                    }
                    string TK25 = this._Key.Substring(0, 4);
                    string Quadrant = this._Key.Substring(5, 1);
                    string Geography = "";
                    float Latitude = 0;
                    float Longitude = 0;
                    if (DiversityWorkbench.Forms.FormTK25.GetGeographyOfTK25(TK25, Quadrant, C, ref Geography, ref Latitude, ref Longitude))
                    {
                        if (Latitude != 0 && Longitude != 0)
                        {
                            string QuadrantCenter = "POINT (" + Longitude.ToString() + " " + Latitude.ToString() + ")";
                            this._GeographyValue = QuadrantCenter.Replace(",", ".");
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        #endregion

        #region Infos

        private void SetInfos(System.Data.DataRow R, System.Collections.Generic.Dictionary<string, string> DisplayedColumns = null)
        {
            if (this._Sheet.ShowDetailsInMap)
            {
                try
                {
                    string Content = "";
                    if (DisplayedColumns == null)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<int, DiversityWorkbench.Spreadsheet.DataColumn> DC in this._Sheet.SelectedColumns())
                        {
                            if (R.Table.Columns.Count <= DC.Key)
                                continue;
                            if (DC.Value.Type() == DataColumn.ColumnType.Data
                                && DC.Value.IsVisible
                                && !DC.Value.IsHidden)
                            {
                                string ColumnContent = "";
                                if (!R[DC.Key].Equals(System.DBNull.Value))
                                    ColumnContent = R[DC.Key].ToString();
                                int Width = DC.Value.Width / _InfoWidthConversionFaktor;
                                if (ColumnContent.Length > Width)
                                    ColumnContent = ColumnContent.Substring(0, Width);
                                if (ColumnContent.Length < Width)
                                {
                                    int l = Width - ColumnContent.Length;
                                    for (int i = 0; i < l; i++)
                                    {
                                        ColumnContent += " ";
                                    }
                                }
                                ColumnContent += "|";
                                Content += ColumnContent;
                            }
                        }
                        if (Content.EndsWith("|"))
                            Content = Content.Substring(0, Content.Length - 1);
                        if (!this.Infos(DisplayedColumns).Contains(Content) || this._Sheet.ShowAllDetailsInMap)
                            this.Infos(DisplayedColumns).Add(Content);
                    }
                    else
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DisplayedColumns)
                        {
                            //if (Content.Length > 0)
                            //    Content += "|";
                            //Content += KV.Value;
                            string Value = R[KV.Key].ToString();
                            if (Value.Length > KV.Value.Length)
                                Value = Value.Substring(0, KV.Value.Length - 1);
                            while (Value.Length < KV.Value.Length)
                                Value += " ";
                            if (Content.Length > 0)
                                Content += "|";
                            Content += Value;
                        }
                        if (!this.Infos(DisplayedColumns).Contains(Content) || this._Sheet.ShowAllDetailsInMap)
                            this.Infos(DisplayedColumns).Add(Content);
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        public string InfoDisplayText(System.Collections.Generic.Dictionary<string, string> DisplayedColumns = null)
        {
            string Infos = "";
            if (this._Sheet.ShowDetailsInMap)
            {
                if (DisplayedColumns == null)
                {
                    foreach (string D in this.Infos())
                    {
                        if (Infos.Length > 0)
                            Infos += "\r\n";
                        Infos += D;
                    }
                }
                else
                {
                    foreach (string D in this.Infos(DisplayedColumns))
                    {
                        if (Infos.Length > 0)
                            Infos += "\r\n";
                        Infos += D;
                    }
                }
            }
            return Infos;
        }

        #endregion

    }
}
