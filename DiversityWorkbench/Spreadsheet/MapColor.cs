using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Spreadsheet
{
    public class MapColor
    {

        #region Parameter & Properties

        private System.Windows.Media.Brush _Brush;
        private string _Operator;
        private string _LowerValue;
        private string _UpperValue;
        private double? _SortingValue = null;
        private bool _SortingValueFixed = false;

        public System.Windows.Media.Brush Brush
        {
            get { return _Brush; }
            set { _Brush = value; }
        }

        public System.Drawing.Color Color
        {
            get
            {
                return BrushColors()[this.Brush];
            }
        }

        public string Operator
        {
            get { return _Operator; }
            set 
            { 
                _Operator = value;
                if (this._Operator != "-")
                    this._UpperValue = "";
            }
        }

        public string LowerValue
        {
            get { return _LowerValue; }
            set { _LowerValue = value; }
        }

        public string UpperValue
        {
            get { return _UpperValue; }
            set { _UpperValue = value; }
        }

        public string SQL()
        {
            string SQL = "";
            switch (this.Operator)
            {
                case "-":
                    SQL += " BETWEEN '" + this.LowerValue + "' AND '" + this.UpperValue + "' ";
                    break;
                case "~":
                    SQL += " LIKE '" + this.LowerValue + "' ";
                    break;
                default:
                    SQL += " " + this.Operator + " '" + this.LowerValue + "' ";
                    break;
            }
            return SQL;
        }

        public void SetSortingValue(double Value, bool FixValue = false)
        {
            this._SortingValue = Value;
            this._SortingValueFixed = FixValue;
        }

        public bool SortingValueFixed() { return this._SortingValueFixed; }

        public void ResetSortingValue() 
        {
            this._SortingValue = null;
            this._SortingValueFixed = false; 
        }

        public double SortingValue()
        {
            if (this._SortingValue != null)
                return (double)this._SortingValue;
            double iVal = 0;
            double iLower = 0;
            double iUpper = 0;
            if (double.TryParse(this.LowerValue, out iLower))
            {
                if (this.UpperValue.Length > 0 && double.TryParse(this.UpperValue, out iUpper))
                {
                    iVal = (double)((iLower + iUpper) / 2);
                }
                else
                {
                    switch (this.Operator)
                    {
                        case "-":
                            iVal = (double)((iLower + iUpper) / 2);
                            break;
                        case "<":
                            iVal = iLower - 1;
                            break;
                        case ">":
                            iVal = iLower + 1;
                            break;
                        case "<=":
                            iVal = iLower;
                            break;
                        case ">=":
                            iVal = iLower;
                            break;
                        default:
                            iVal = iLower;
                            break;
                    }
                }
                //else 
                //    iVal = iLower;
            }
            return iVal;
        }

        public bool ValueIsInRange(string Value)
        {
            bool InRange = false;
            int iVal;
            if (int.TryParse(Value, out iVal))
            {
                int iLower = 0;
                int iUpper = 0;
                int.TryParse(this.LowerValue, out iLower);
                int.TryParse(this.UpperValue, out iUpper);
                switch (this.Operator)
                {
                    case "-":
                        if (iVal <= iUpper && iVal >= iLower)
                            InRange = true;
                        break;
                    case "<":
                        if (iVal < iLower)
                            InRange = true;
                        break;
                    case ">":
                        if (iVal > iLower)
                            InRange = true;
                        break;
                    case "<=":
                        if (iVal <= iLower)
                            InRange = true;
                        break;
                    case ">=":
                        if (iVal >= iLower)
                            InRange = true;
                        break;
                    default:
                        if (iVal == iLower)
                            InRange = true;
                        break;
                }
            }
            return InRange;
        }
        
        #endregion

        #region Construction

        public MapColor(System.Windows.Media.Brush Brush, string Operator, string LowerValue, string UpperValue)
        {
            this._Brush = Brush;
            this._Operator = Operator;
            this._LowerValue = LowerValue;
            this._UpperValue = UpperValue;
        }

        public MapColor(string Brush, string Operator, string LowerValue, string UpperValue)
        {
            foreach (System.Collections.Generic.KeyValuePair<System.Windows.Media.Brush, System.Drawing.Color> KV in BrushColors())
            {
                if (Brush == KV.Key.ToString())
                {
                    this._Brush = KV.Key;
                    break;
                }
                if (Brush == KV.Value.Name)
                {
                    this._Brush = KV.Key;
                    break;
                }
            }
            this._Operator = Operator;
            this._LowerValue = LowerValue;
            this._UpperValue = UpperValue;
        }

        #endregion

        #region static lists
        
        static private System.Collections.Generic.Dictionary<System.Windows.Media.Brush, System.Drawing.Color> _BrushColors;
        static public System.Collections.Generic.Dictionary<System.Windows.Media.Brush, System.Drawing.Color> BrushColors()
        {
            if (_BrushColors == null)
            {
                _BrushColors = new Dictionary<System.Windows.Media.Brush, System.Drawing.Color>();
                _BrushColors.Add(System.Windows.Media.Brushes.Red, System.Drawing.Color.Red);
                _BrushColors.Add(System.Windows.Media.Brushes.Orange, System.Drawing.Color.Orange);
                _BrushColors.Add(System.Windows.Media.Brushes.Yellow, System.Drawing.Color.Yellow);
                _BrushColors.Add(System.Windows.Media.Brushes.Green, System.Drawing.Color.Green);
                _BrushColors.Add(System.Windows.Media.Brushes.Blue, System.Drawing.Color.Blue);
                _BrushColors.Add(System.Windows.Media.Brushes.Violet, System.Drawing.Color.Violet);
                _BrushColors.Add(System.Windows.Media.Brushes.Brown, System.Drawing.Color.Brown);
                _BrushColors.Add(System.Windows.Media.Brushes.Black, System.Drawing.Color.Black);
                _BrushColors.Add(System.Windows.Media.Brushes.Gray, System.Drawing.Color.Gray);
                _BrushColors.Add(System.Windows.Media.Brushes.White, System.Drawing.Color.White);
            }
            return _BrushColors;
        }

        private static System.Collections.Generic.Dictionary<string, string> _Operators;
        public static System.Collections.Generic.Dictionary<string, string> Operators()
        {
            if (_Operators == null)
            {
                _Operators = new Dictionary<string, string>();
                _Operators.Add("=", "Is exactly");
                //_Operators.Add("~", "Is like the given value (use wildcards %, _)");
                _Operators.Add("<", "Is smaller than");
                _Operators.Add("<=", "Is smaller than or equal to");
                _Operators.Add(">", "Is bigger than");
                _Operators.Add(">=", "Is bigger than or equal to");
                _Operators.Add("<>", "Is not equal to");
                _Operators.Add("-", "Is within the range of the given values");
                _Operators.Add("∅", "Is missing");
            }
            return _Operators;
        }
        
        #endregion

    }
}
