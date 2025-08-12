using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityWorkbench.Geography
{
    public partial class FormEditGeography : Form
    {

        #region Parameter

        private DiversityWorkbench.Geography.Geography _Geography;
        private System.Data.DataTable _DtGeography;
        private string _DataTable;
        private string _DataColumn;
        private string _GeographyAsString;
        private System.Collections.Generic.List<GeoConversion.GeodeticPosition> _geoPoints;

        #endregion

        #region Construction

        public FormEditGeography(string Header, string DataTable, string DataColumn, string Geography = "")
        {
            InitializeComponent();
            this.labelHeader.Text = Header;
            this._DataColumn = DataColumn;
            this._DataTable = DataTable;
            this.initGeography(Geography);
            this.CancelButton = this.buttonCancel;
            this.AcceptButton = this.buttonOK;
        }

        #endregion

        #region Interface

        public string Geography()
        {
            return this._Geography.GeographyAsString();
        }

        public bool SaveGeography(string Table, string Column, string WhereClause)
        {
            this.saveGeography();
            return this._Geography.SaveGeography(Table, Column, WhereClause);
        }

        #endregion

        #region Events

        private void initGeography(string Geography)
        {
            try
            {
                this._GeographyAsString = Geography;
                this._Geography = new Geography(Geography);
                //this.fillDataset();
                this.setControlsAccordingToDisplayType();
                this.SetControlsAccordingToType();
                this.dataGridViewGeography.Columns.Clear();
                this.setDatagridSource();
                //this.dataGridViewGeography.DataSource = null;
                //this.dataGridViewGeography.DataSource = this.dataSet.Tables["TableNumeric"];
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void saveGeography()
        {
            try
            {
                System.Collections.Generic.List<GeoConversion.GeodeticPosition> Points = new List<GeoConversion.GeodeticPosition>();
                switch (this._Geography.TypeOfDisplay)
                {
                    case DiversityWorkbench.Geography.Geography.DisplayType.degree:
                        foreach (System.Data.DataRow R in this.dataSet.Tables["TableDegree"].Rows)
                        {
                            int LatDeg;
                            int LatMin;
                            double LatSec;
                            int LongDeg;
                            int LongMin;
                            double LongSec;
                            double Alt = 0;
                            if (int.TryParse(R["LatitudeDeg"].ToString(), out LatDeg) &&
                                int.TryParse(R["MinLat"].ToString(), out LatMin) &&
                                double.TryParse(R["SecLat"].ToString(), out LatSec) &&
                                int.TryParse(R["Longitude"].ToString(), out LongDeg) &&
                                int.TryParse(R["MinLong"].ToString(), out LongMin) &&
                                double.TryParse(R["SecLong"].ToString(), out LongSec))
                            {
                                double Latitude = 0;
                                double Longitude = 0;
                                double alt;
                                if (double.TryParse(R["Altitude"].ToString(), out alt))
                                    Alt = alt;
                                if (GeoConversion.GeoCon.ConvertCoordinateToDecimal(LatDeg, LatMin, LatSec, ref Latitude) &&
                                    GeoConversion.GeoCon.ConvertCoordinateToDecimal(LongDeg, LongMin, LongSec, ref Longitude))
                                {
                                    GeoConversion.GeodeticPosition position = new GeoConversion.GeodeticPosition(Longitude, Latitude, Alt);
                                    Points.Add(position);
                                }
                            }
                        }
                        break;
                    case DiversityWorkbench.Geography.Geography.DisplayType.numeric:
                        foreach (System.Data.DataRow R in this.dataSet.Tables["TableNumeric"].Rows)
                        {
                            double Lat;
                            double Long;
                            double Alt = 0;
                            if (double.TryParse(R["Latitude"].ToString(), out Lat) &&
                                double.TryParse(R["Longitude"].ToString(), out Long))
                            {
                                double alt;
                                if (double.TryParse(R["Altitude"].ToString(), out alt))
                                    Alt = alt;
                                GeoConversion.GeodeticPosition position = new GeoConversion.GeodeticPosition(Long, Lat, Alt);
                                Points.Add(position);
                            }
                        }
                        break;
                }
                this._Geography.WriteGeoPoints(Points);
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void fillDataset(bool ChangeDisplay = false)
        {
            switch (this._Geography.TypeOfDisplay)
            {
                case DiversityWorkbench.Geography.Geography.DisplayType.degree:
                    this.dataSet.Tables["TableDegree"].Clear();
                    foreach(GeoConversion.GeodeticPosition position in this._Geography.getGeoPoints())
                    {
                        System.Data.DataRow dataRow = this.dataSet.Tables["TableDegree"].NewRow();
                        dataRow["Longitude"] = position.lonDeg;
                        dataRow["MinLong"] = position.lonMin;
                        dataRow["SecLong"] = position.lonSec;
                        dataRow["LatitudeDeg"] = position.latDeg;
                        dataRow["MinLat"] = position.latMin;
                        dataRow["SecLat"] = position.latSec;
                        dataRow["Altitude"] = position.h;
                        this.dataSet.Tables["TableDegree"].Rows.Add(dataRow);
                    }
                    break;
                case DiversityWorkbench.Geography.Geography.DisplayType.numeric:
                    this.dataSet.Tables["TableNumeric"].Clear();
                    foreach (GeoConversion.GeodeticPosition position in this._Geography.getGeoPoints())
                    {
                        System.Data.DataRow dataRow = this.dataSet.Tables["TableNumeric"].NewRow();
                        dataRow["Latitude"] = position.lat;
                        dataRow["Longitude"] = position.lon;
                        dataRow["Altitude"] = position.h;
                        this.dataSet.Tables["TableNumeric"].Rows.Add(dataRow);
                    }
                    break;
            }
        }

        private void initDataGrid()
        {
            this.dataGridViewGeography.Columns.Clear();
            switch(this._Geography.TypeOfDisplay)
            {
                case DiversityWorkbench.Geography.Geography.DisplayType.degree:
                    this.dataGridViewGeography.DataSource = this.dataSet.Tables["TableDegree"];
                    break;
                case DiversityWorkbench.Geography.Geography.DisplayType.numeric:
                    this.dataGridViewGeography.DataSource = this.dataSet.Tables["TableNumeric"];
                    break;

            }
        }

        private void comboBoxNumericOrDegMinSec_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this._Geography.setTypeOfDisplay(comboBoxNumericOrDegMinSec.SelectedItem.ToString());
                this.setDatagridSource();

            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setDatagridSource()
        {
            try
            {
                //this.saveGeography();
                this.fillDataset();
                this.dataGridViewGeography.DataSource = null;
                switch (this._Geography.TypeOfDisplay)
                {
                    case DiversityWorkbench.Geography.Geography.DisplayType.degree:
                        this.dataGridViewGeography.DataSource = this.dataSet.Tables["TableDegree"];
                        this.dataGridViewGeography.Columns[0].HeaderText = "↔°";
                        this.dataGridViewGeography.Columns[0].Width = 30;
                        this.dataGridViewGeography.Columns[1].HeaderText = "'";
                        this.dataGridViewGeography.Columns[1].Width = 20;
                        this.dataGridViewGeography.Columns[2].HeaderText = "''";
                        this.dataGridViewGeography.Columns[2].Width = 60;
                        this.dataGridViewGeography.Columns[3].HeaderText = "↕°";
                        this.dataGridViewGeography.Columns[3].Width = 30;
                        this.dataGridViewGeography.Columns[4].HeaderText = "'";
                        this.dataGridViewGeography.Columns[4].Width = 20;
                        this.dataGridViewGeography.Columns[5].HeaderText = "''";
                        this.dataGridViewGeography.Columns[5].Width = 60;
                        this.dataGridViewGeography.Columns[6].HeaderText = "Altitude";
                        this.dataGridViewGeography.Columns[6].Width = 80;
                        this.dataGridViewGeography.Columns[6].Visible = this._Geography.IncludeAltitude;
                        break;
                    default:
                        this.dataGridViewGeography.DataSource = this.dataSet.Tables["TableNumeric"];
                        this.dataGridViewGeography.Columns[0].Width = 100;
                        this.dataGridViewGeography.Columns[1].Width = 100;
                        this.dataGridViewGeography.Columns[2].HeaderText = "Altitude [m]";
                        this.dataGridViewGeography.Columns[2].Width = 100;
                        this.dataGridViewGeography.Columns[2].Visible = this._Geography.IncludeAltitude;
                        break;
                }
                //this.initDataGrid();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void dataGridViewGeography_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //bool OK = this.CheckCellContent(e.RowIndex, e.ColumnIndex);
            //if (OK)
            //    this.userControlDialogPanel.Enabled = this.CheckContent();
            //string Content = this.dataGridViewGeography.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            //double Value;
            //if (double.TryParse(Content, out Value))
            //{
            //    switch(this._Geography.TypeOfDisplay)
            //    {
            //        case DiversityWorkbench.Geography.Geography.DisplayType.degree:
            //            switch(e.ColumnIndex)
            //            {
            //                case 0:
            //                    OK = (Value <= 180 && Value >= -180);
            //                    break;
            //                case 3:
            //                    OK = (Value <= 90 && Value >= -90);
            //                    break;
            //                case 1:
            //                case 4:
            //                    OK = (Value < 60 && Value >= 0);
            //                    break;
            //                case 2:
            //                case 5:
            //                    OK = (Value < 60 && Value >= 0);
            //                    break;
            //            }
            //            break;
            //        case DiversityWorkbench.Geography.Geography.DisplayType.numeric:
            //            switch (e.ColumnIndex)
            //            {
            //                case 0:
            //                    OK = (Value <= 180 && Value >= -180);
            //                    break;
            //                case 1:
            //                    OK = (Value <= 90 && Value >= -90);
            //                    break;
            //            }
            //            break;
            //    }
            //}
            //else { OK = false; }
            //if (OK)
            //    this.dataGridViewGeography.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = System.Drawing.SystemColors.Window;
            //else
            //    this.dataGridViewGeography.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = System.Drawing.Color.Pink;
            //this.userControlDialogPanel.Enabled = OK;
        }

        private bool _ContentOK = true;

        private bool CheckContent(ref string Message)
        {
            _ContentOK = true;
            int MaxCount = this.dataGridViewGeography.Rows.Count - 1;
            if (this._Geography.TypeOfGeometry == DiversityWorkbench.Geography.Geography.GeometryType.POINT)
                MaxCount++;
            for (int r = 0; r < MaxCount; r++)
            {
                for (int c = 0; c < this.dataGridViewGeography.ColumnCount; c++)
                {
                    if (!this.CheckCellContent(r, c, ref Message))
                    {
                        _ContentOK = false;
                        //break;
                    }
                }
                //if (!_ContentOK)
                //    break;
            }
            return _ContentOK;
        }

        private bool CheckCellContent(int RowIndex, int ColumnIndex, ref string Message)
        {
            bool OK = true;
            try
            {
                string Content = this.dataGridViewGeography.Rows[RowIndex].Cells[ColumnIndex].Value.ToString();
                double Value;
                if (Content.Length > 0)
                {
                    if (double.TryParse(Content, out Value))
                    {
                        switch (this._Geography.TypeOfDisplay)
                        {
                            case DiversityWorkbench.Geography.Geography.DisplayType.degree:
                                switch (ColumnIndex)
                                {
                                    case 0:
                                        OK = this.CheckCellContent("longitude", Value, -180, 180, ref Message);// (Value <= 180 && Value >= -180);
                                                                                                               //if (!OK) Message += "The value " + Value.ToString() + "for longitude is not between 180 and -180\r\n";
                                        break;
                                    case 3:
                                        OK = this.CheckCellContent("latitude", Value, -90, 90, ref Message);// (Value <= 180 && Value >= -180);
                                                                                                            //OK = (Value <= 90 && Value >= -90);
                                                                                                            //if (!OK) Message += "The value " + Value.ToString() + "for latitude is not between 90 and -90\r\n";
                                        break;
                                    case 1:
                                    case 4:
                                        OK = this.CheckCellContent("minutes", Value, 0, 60, ref Message);// (Value <= 180 && Value >= -180);
                                                                                                         //OK = (Value < 60 && Value >= 0);
                                                                                                         //if (!OK) Message += "The value " + Value.ToString() + "for minutes is not between 0 and 60\r\n";
                                        break;
                                    case 2:
                                    case 5:
                                        OK = this.CheckCellContent("seconds", Value, 0, 60, ref Message);// (Value <= 180 && Value >= -180);
                                                                                                         //OK = (Value < 60 && Value >= 0);
                                                                                                         //if (!OK) Message += "The value " + Value.ToString() + "for seconds is not between 0 and 60\r\n";
                                        break;
                                }
                                break;
                            case DiversityWorkbench.Geography.Geography.DisplayType.numeric:
                                switch (ColumnIndex)
                                {
                                    case 0:
                                        OK = this.CheckCellContent("longitude", Value, -180, 180, ref Message);// (Value <= 180 && Value >= -180);
                                                                                                               //OK = (Value <= 180 && Value >= -180);
                                                                                                               //if (!OK) Message += "The value " + Value.ToString() + "for longitude is not between 180 and -180\r\n";
                                        break;
                                    case 1:
                                        OK = this.CheckCellContent("latitude", Value, -90, 90, ref Message);// (Value <= 180 && Value >= -180);
                                                                                                            //OK = (Value <= 90 && Value >= -90);
                                                                                                            //if (!OK) Message += "The value " + Value.ToString() + "for latitude is not between 90 and -90\r\n";
                                        break;
                                }
                                break;
                        }
                    }
                    else { OK = false; }
                }
                if (OK)
                    this.dataGridViewGeography.Rows[RowIndex].Cells[ColumnIndex].Style.BackColor = System.Drawing.SystemColors.Window;
                else if (Content.Length > 0)
                {
                    this.dataGridViewGeography.Rows[RowIndex].Cells[ColumnIndex].Style.BackColor = System.Drawing.Color.Pink;
                    _ContentOK = false;
                    //this.buttonOK.Enabled = OK;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        private bool CheckCellContent(string CheckedEntry, double Value, double LowerBorder, double UpperBorder, ref string Message)
        {
            bool OK = Value >= LowerBorder && Value <= UpperBorder;
            if (OK && UpperBorder == 60)
                OK = Value < UpperBorder;
            if (!OK)
                Message += "The value " + Value.ToString() + " for " + CheckedEntry + " is not between " + LowerBorder.ToString() + " and " + UpperBorder.ToString() + "\r\n";
            return OK;
        }

        private void dataGridViewGeography_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    bool OK = this.CheckCellContent(e.RowIndex, e.ColumnIndex);
            //}
            //catch (System.Exception ex)
            //{
            //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //}
        }

        private void dataGridViewGeography_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            //this.CheckRow(e.RowIndex);
        }

        private void dataGridViewGeography_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            //this.CheckRow(e.RowIndex);
        }

        private string CheckRow(int RowIndex)
        {
            _ContentOK = true;
            string Message = "";
            for (int c = 0; c < this.dataGridViewGeography.ColumnCount; c++)
            {
                if (!this.CheckCellContent(RowIndex, c, ref Message))
                {
                    _ContentOK = false;
                }
            }
            if (!_ContentOK)
                this.buttonOK.Enabled = false;
            return Message;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                switch (this._Geography.TypeOfDisplay)
                {
                    case DiversityWorkbench.Geography.Geography.DisplayType.degree:
                        this.dataSet.Tables["TableDegree"].AcceptChanges();
                        break;
                    default:
                        this.dataSet.Tables["TableNumeric"].AcceptChanges();
                        break;
                }
                string Message = "";
                if (this.CheckContent(ref Message))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(Message, "Wrong entries", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Type

        private void SetControlsAccordingToType()
        {
            switch (this._Geography.TypeOfGeometry)
            {
                case DiversityWorkbench.Geography.Geography.GeometryType.POINT:
                    this.radioButtonTypePoint.Checked = true;
                    break;
                case DiversityWorkbench.Geography.Geography.GeometryType.MULTIPOINT:
                    this.radioButtonTypeMultipoint.Checked = true;
                    break;
                case DiversityWorkbench.Geography.Geography.GeometryType.LINESTRING:
                    this.radioButtonTypeLine.Checked = true;
                    break;
                case DiversityWorkbench.Geography.Geography.GeometryType.POLYGON:
                    this.radioButtonTypePolygon.Checked = true;
                    break;
            }
            this.dataGridViewGeography.AllowUserToAddRows = (this._Geography.TypeOfGeometry != DiversityWorkbench.Geography.Geography.GeometryType.POINT);
        }

        private void radioButtonTypePoint_Click(object sender, EventArgs e)
        {
            this._Geography.TypeOfGeometry = DiversityWorkbench.Geography.Geography.GeometryType.POINT;
            this.SetControlsAccordingToType();
        }

        private void radioButtonTypeMultipoint_Click(object sender, EventArgs e)
        {
            this._Geography.TypeOfGeometry = DiversityWorkbench.Geography.Geography.GeometryType.MULTIPOINT;
            this.SetControlsAccordingToType();
        }

        private void radioButtonTypeLine_Click(object sender, EventArgs e)
        {
            this._Geography.TypeOfGeometry = DiversityWorkbench.Geography.Geography.GeometryType.LINESTRING;
            this.SetControlsAccordingToType();
        }

        private void radioButtonTypePolygon_Click(object sender, EventArgs e)
        {
            this._Geography.TypeOfGeometry = DiversityWorkbench.Geography.Geography.GeometryType.POLYGON;
            this.SetControlsAccordingToType();
        }

        #endregion

        #region Display

        private void setControlsAccordingToDisplayType()
        {
            switch(this._Geography.TypeOfDisplay)
            {
                case DiversityWorkbench.Geography.Geography.DisplayType.degree:
                    this.radioButtonDisplayDegree.Checked = true;
                    break;
                case DiversityWorkbench.Geography.Geography.DisplayType.numeric:
                    this.radioButtonDisplayNumeric.Checked = true;
                    break;
            }
        }

        private void ChangeDisplayType(DiversityWorkbench.Geography.Geography.DisplayType type)
        {
            this.saveGeography();
            this._Geography.TypeOfDisplay = type;
            this.setControlsAccordingToDisplayType();
            this.setDatagridSource();
        }

        private void radioButtonDisplayNumeric_Click(object sender, EventArgs e)
        {
            this.ChangeDisplayType(DiversityWorkbench.Geography.Geography.DisplayType.numeric);
        }

        private void radioButtonDisplayDegree_Click(object sender, EventArgs e)
        {
            this.ChangeDisplayType(DiversityWorkbench.Geography.Geography.DisplayType.degree);
        }

        private void checkBoxIncludeAltitude_Click(object sender, EventArgs e)
        {
            this._Geography.IncludeAltitude = !this._Geography.IncludeAltitude;
            this.checkBoxIncludeAltitude.Checked = this._Geography.IncludeAltitude;
            this.saveGeography();
            this.setDatagridSource();
        }

        #endregion

    }
}
