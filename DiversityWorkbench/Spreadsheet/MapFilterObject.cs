using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Spreadsheet
{
    public class MapFilterObject
    {

        #region Parameter

        private MapColor _MapColor;
        private MapSymbol _MapSymbol;
        private int _iGeographyPosition;
        private int _iGeographySymbolPosition;
        private int _iGeographyColorPosition;
        private string _SybolValue;
        private string _ColorValue;
        private string _GeographyValue;
        private string _Filter;
        private Sheet _Sheet;
        
        #endregion

        #region Construction

        //public MapFilterObject(string Filter, MapColor Color, MapSymbol Symbol)
        //{
        //    this._Filter = Filter;
        //    this._MapColor = Color;
        //    this._MapSymbol = Symbol;
        //}

        //public MapFilterObject(string Filter, string ColorValue, string SymbolValue, string Geography)
        //{
        //    this._Filter = Filter;
        //    this._ColorValue = ColorValue;
        //    this._SybolValue = SymbolValue;
        //    this._GeographyValue = Geography;
        //}

        public MapFilterObject(string Filter, Sheet Sheet, System.Data.DataRow R, int iGeographyPosition, int iGeographySymbolPosition, int iGeographyColorPosition)
        {
            // setting the filter
            this._Filter = Filter;

            this._Sheet = Sheet;

            // setting the positions
            this._iGeographyColorPosition = iGeographyColorPosition;
            this._iGeographyPosition = iGeographyPosition;
            this._iGeographySymbolPosition = iGeographySymbolPosition;

            // setting the values
            try
            {
                if (!R[this._iGeographyColorPosition].Equals(System.DBNull.Value))
                    this._ColorValue = R[this._iGeographyColorPosition].ToString();
                if (!R[this._iGeographyPosition].Equals(System.DBNull.Value))
                    this._GeographyValue = R[this._iGeographyPosition].ToString();
                if (!R[this._iGeographySymbolPosition].Equals(System.DBNull.Value))
                    this._SybolValue = R[this._iGeographySymbolPosition].ToString();
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        public bool UpdateContent(System.Data.DataRow R)
        {
            bool OK = true;
            foreach (System.Collections.Generic.KeyValuePair<int, MapFilter> MF in this._Sheet.MapFilterList)
            {
                switch (MF.Value.FilterType())
                {
                    case MapFilter.FilterTypes.Geography:
                        if (this._GeographyValue == null)
                        {
                        }
                        break;
                    case MapFilter.FilterTypes.Color:
                        if (this._ColorValue == null)
                        {
                            this._ColorValue = R[this._iGeographyColorPosition].ToString();
                        }
                        else
                        {
                        }
                        break;
                    case MapFilter.FilterTypes.Symbol:
                        if (this._SybolValue == null)
                        {
                            this._SybolValue = R[this._iGeographySymbolPosition].ToString();
                        }
                        else
                        {
                        }
                        break;
                }
            }
            return OK;
        }

        //public bool UpdateContent(System.Data.DataRow R, int iGeographyPosition, int iGeographySymbolPosition, int iGeographyColorPosition)
        //{
        //    bool OK = true;

        //    return OK;
        //}

    }
}
