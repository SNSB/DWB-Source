using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WpfSamplingPlotPage;


namespace DiversityCollection.Forms
{
    public partial class FormCollectionEventSeriesGridMode : Form
    {
        #region Paramter

        private System.Collections.Generic.List<int> _IDs;
        private string _sIDs;
        //private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeries;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeriesImage;
        //private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeriesEvent;
        //private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeriesSpecimen;
        //private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterEventSeriesUnit;
        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        //private int? _CollectionSpecimenID;
        private int _ProjectID;
        /// <summary>
        /// the WPF control for the GIS operations
        /// </summary>
        private WpfSamplingPlotPage.WpfControl _wpfControl;
        private GeoObject _GeoObject;
        private List<GeoObject> _GeoObjectsEditor;

        private DiversityCollection.Forms.FormGridFunctions.FormState _FormState = Forms.FormGridFunctions.FormState.Loading;

        #endregion

        #region Construction

        public FormCollectionEventSeriesGridMode(System.Collections.Generic.List<int> IDs, int ProjectID)
        {
            InitializeComponent();
            this._FormState = FormGridFunctions.FormState.Loading;
            this._IDs = IDs;
            this._ProjectID = ProjectID;
            for (int i = 0; i < _IDs.Count; i++)
            {
                if (i > 0) _sIDs += ", ";
                this._sIDs += _IDs[i].ToString();
            }
            this.initForm();
            if (this.userControlEventSeriesTree.DtEventSeries.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("The data contain no collection event series");
                this.Close();
            }
            this._FormState = FormGridFunctions.FormState.Editing;
        }
        
        #endregion

        #region Form

        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void initForm()
        {
            string TableName = "CollectionEventSeries";
            bool OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
            if (!OK)
            {
                this.dataGridView.ReadOnly = true;
                this.dataGridView.AllowUserToAddRows = false;
                this.dataGridView.AllowUserToDeleteRows = false;
            }

            this.userControlEventSeriesTree.initControl(this._IDs, "CollectionEventSeries", "SeriesID", this.dataGridView, this.dataSetCollectionEventSeries.CollectionEventSeries, this.toolStripButtonSearchSpecimen_Click);
            this.dataGridView.DataSource = this.userControlEventSeriesTree.DtEventSeries;
            this.userControlEventSeriesTree.toolStripButtonSearchSpecimen.Visible = false;
            this.userControlEventSeriesTree.toolStripButtonShowUnit.Visible = false;
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();

            // Create WPF control instance
            _wpfControl = new WpfSamplingPlotPage.WpfControl(DiversityWorkbench.Settings.Language);
            this._wpfControl.WpfShowRefMapDelButton(false);
            // hide the delete button of the backgroud map
            //_wpfControl.WpfShowRefMapDelButton(false);
            // Add to our Form
            elementHost.Child = _wpfControl;
        }

        private DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                return this._FormFunctions;
            }
        }

        private void toolStripButtonSearchSpecimen_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormCollectionEventSeriesGridMode_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCollectionEventSeries.CollectionEventSeriesImage". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collectionEventSeriesImageTableAdapter.Fill(this.dataSetCollectionEventSeries.CollectionEventSeriesImage);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCollectionEventSeries.CollectionEventSeries". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collectionEventSeriesTableAdapter.Fill(this.dataSetCollectionEventSeries.CollectionEventSeries);

        }

        private void FormCollectionEventSeriesGridMode_SizeChanged(object sender, EventArgs e)
        {
            this.setColumnWidth();
        }

        private void FormCollectionEventSeriesGridMode_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
                this.userControlEventSeriesTree.saveDataEventSeries();
        }

        private void buttonHistory_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.SeriesID == null)
                {
                    System.Windows.Forms.MessageBox.Show("No data selected");
                    return;
                }
                System.Data.DataRow[] rr = this.userControlEventSeriesTree.DtEventSeries.Select("SeriesID = " + this.SeriesID.ToString());
                if (rr.Length > 0)
                {
                    string Title = "History of the collection event series with the SeriesID " + rr[0]["SeriesID"].ToString();
                    this.dataSetCollectionEventSeries.CollectionEventSeries.Clear();
                    DiversityCollection.Datasets.DataSetCollectionEventSeries.CollectionEventSeriesRow R = this.dataSetCollectionEventSeries.CollectionEventSeries.NewCollectionEventSeriesRow();
                    foreach (System.Data.DataColumn C in this.dataSetCollectionEventSeries.CollectionEventSeries.Columns)
                    {
                        R[C.ColumnName] = rr[0][C.ColumnName];
                    }
                    this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Add(R);
                    System.Collections.Generic.List<System.Data.DataTable> LogTables = new List<DataTable>();
                    if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 0)
                        LogTables.Add(DiversityWorkbench.Database.DtHistory((int)this.SeriesID, "SeriesID", this.dataSetCollectionEventSeries.CollectionEventSeries.TableName, ""));
                    if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 0)
                        LogTables.Add(DiversityWorkbench.Database.DtHistory((int)this.SeriesID, "SeriesID", this.dataSetCollectionEventSeries.CollectionEventSeriesImage.TableName, ""));
                    DiversityWorkbench.Forms.FormHistory f = new DiversityWorkbench.Forms.FormHistory(Title, LogTables, DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace());
                    f.setHelpProviderNameSpace(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace(), "History");
                    f.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Feedback.SendFeedback(
                    System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,
                    System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                    "",
                    this.SeriesID.ToString());
            }
            catch { }
        }

        private void setHeaeder()
        {
            try
            {
                this.textBoxHeaderSeriesID.Text = this.dataGridView.Rows[this.dataGridView.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
                this.textBoxHeaderCode.Text = this.dataGridView.Rows[this.dataGridView.SelectedCells[0].RowIndex].Cells[3].Value.ToString();
                this.textBoxHeaderDescription.Text = this.dataGridView.Rows[this.dataGridView.SelectedCells[0].RowIndex].Cells[2].Value.ToString();
            }
            catch (System.Exception ex) { }
        }

        #endregion

        #region Data

        //private void saveData()
        //{
        //    try
        //    {
        //        foreach (System.Data.DataRow R in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
        //        {
        //            R.EndEdit();
        //        }
        //        if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 0
        //            && this.dataSetCollectionEventSeries.CollectionEventSeries.DataSet.HasChanges())
        //        {
        //            this._sqlDataAdapterEventSeries.Update(this.dataSetCollectionEventSeries.CollectionEventSeries);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void fillDataSet()
        //{
        //    this.saveData();
        //    // init the sql adapter
        //    try
        //    {
        //        string WhereClause = " WHERE SeriesID = 0";
        //        this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeries, DiversityCollection.CollectionSpecimen.SqlEventSeries + WhereClause, this.dataSetCollectionEventSeries.CollectionEventSeries);
        //        // clear the table
        //        this.dataSetCollectionEventSeries.CollectionEventSeries.Clear();
        //        // fill tables
        //        // EventSeries
        //        string SQL = "SELECT SeriesID, SeriesParentID, DateStart, DateEnd, SeriesCode, Description, Notes FROM CollectionEventSeries WHERE SeriesID IN(SELECT SeriesID FROM dbo.FirstLinesSeries('" + this._sIDs + "'))";
        //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //        ad.Fill(this.dataSetCollectionEventSeries.CollectionEventSeries);
        //        this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeries, SQL, this.dataSetCollectionEventSeries.CollectionEventSeries);

        //        // the collection events
        //        if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 0)
        //        {
        //            SQL = DiversityCollection.CollectionSpecimen.SqlEventSeriesEvent + "WHERE SeriesID IN (";
        //            foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
        //            {
        //                SQL += r["SeriesID"].ToString() + ", ";
        //            }
        //        }
        //        if (SQL.EndsWith(", "))
        //            SQL = SQL.Substring(0, SQL.Length - 2) + ") ORDER BY CollectionDate";
        //        else
        //            SQL = SQL.Substring(0, SQL.Length - 1) + ") ORDER BY CollectionDate";
        //        this.dataSetCollectionEventSeries.CollectionEventList.Clear();
        //        this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeriesEvent, SQL, this.dataSetCollectionEventSeries.CollectionEventList);

        //        if (this.dataSetCollectionEventSeries.CollectionEventList.Rows.Count > 0)
        //        {
        //            // Specimen
        //            SQL = DiversityCollection.CollectionSpecimen.SqlEventSeriesSpecimen + " WHERE CollectionEventID IN (";
        //            foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventList.Rows)
        //            {
        //                SQL += r["CollectionEventID"].ToString() + ", ";
        //            }
        //            SQL = SQL.Substring(0, SQL.Length - 2) + ") ORDER BY AccessionNumber";
        //            this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeriesSpecimen, SQL, this.dataSetCollectionEventSeries.CollectionSpecimenList);
        //            // Unit
        //            if (this.dataSetCollectionEventSeries.CollectionSpecimenList.Rows.Count > 0)
        //            {
        //                SQL = DiversityCollection.CollectionSpecimen.SqlEventSeriesUnit + " WHERE CollectionSpecimenID IN (";
        //                foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionSpecimenList.Rows)
        //                {
        //                    SQL += r["CollectionSpecimenID"].ToString() + ", ";
        //                }
        //                SQL = SQL.Substring(0, SQL.Length - 2) + ")";
        //                this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeriesUnit, SQL, this.dataSetCollectionEventSeries.IdentificationUnitList);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    //this.fillEventSeriesImages();
        //}

        public int? SeriesID
        {
            get
            {
                if (this.userControlEventSeriesTree.DtEventSeries.Rows.Count > 0)
                {
                    int ID = -1;
                    try
                    {
                        if (this.dataGridView.SelectedCells.Count == 0)
                        {
                            if (int.TryParse(this.dataGridView.Rows[0].Cells[0].Value.ToString(), out ID))
                                return ID;
                        }
                        else if (int.TryParse(this.dataGridView.Rows[this.dataGridView.SelectedCells[0].RowIndex].Cells[0].Value.ToString(), out ID))
                        {
                            return ID;
                        }
                        System.Windows.Forms.BindingManagerBase BMB = this.BindingContext[this.userControlEventSeriesTree.DtEventSeries, "CollectionEventSeries"];
                        int p = BMB.Position;
                        if (this.userControlEventSeriesTree.DtEventSeries.Rows.Count > 0)
                            ID = System.Int32.Parse(this.userControlEventSeriesTree.DtEventSeries.Rows[p]["SeriesID"].ToString());
                        return (int?)ID;
                    }
                    catch 
                    {
                        return ID;
                    }
                }
                else
                    return null;
            }
        }

        #endregion

        #region GIS

        private void SetGeoObject()
        {
            this._GeoObject = new GeoObject();
            try
            {
                string DisplayText = "";
                System.Data.DataRow[] RRseries = this.userControlEventSeriesTree.DtEventSeries.Select("SeriesID = " + this.SeriesID.ToString());
                if (RRseries.Length > 0)
                {
                    System.Data.DataRow R = RRseries[0];
                    System.Data.DataRow[] RR = this.userControlEventSeriesTree.DtEventSeriesGeography.Select("SeriesID = " + this.SeriesID.ToString());
                    if (RR.Length > 0)
                    {
                        DisplayText = R["SeriesCode"].ToString();
                        if (!RR[0]["Geography"].Equals(System.DBNull.Value) &&
                            RR[0]["Geography"].ToString().Length > 0)
                        {
                            if (DisplayText.Length > 20) 
                                DisplayText = DisplayText.Substring(0, 20) + "...";
                            this._GeoObject = this.setupGeoObject(R["SeriesID"].ToString()
                                , DisplayText
                                , RR[0]["Geography"].ToString()
                                , this.Brush);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }

            if (this._GeoObject.GeometryData != null && this._GeoObject.GeometryData.Length > 0)
            {
                this._GeoObjectsEditor = new List<GeoObject>();
                this._GeoObjectsEditor.Add(this._GeoObject);

                this._wpfControl.WpfClearAllSamples(true);
                this._wpfControl.WpfSetMapAndGeoObjects(this._GeoObjectsEditor);
            }
        }

        private GeoObject setupGeoObject(string Identifier, string DisplayText, string data, System.Windows.Media.Brush brush)
        {
            GeoObject geoObject = new GeoObject();
            geoObject.Identifier = Identifier;
            geoObject.DisplayText = DisplayText;
            geoObject.StrokeBrush = brush;
            geoObject.FillBrush = brush;
            geoObject.StrokeTransparency = this.StrokeTransparency;
            geoObject.FillTransparency = this.FillTransparency;
            geoObject.StrokeThickness = this.StrokeThickness;
            geoObject.PointType = this.Symbol;
            geoObject.PointSymbolSize = this.SymbolSize;
            geoObject.GeometryData = data;
            return geoObject;
        }

        private byte StrokeTransparency { get { return 255; } }

        private double StrokeThickness
        {
            get
            {
                try
                {
                    double T = 1.0;
                    //if (!double.TryParse(this.toolStripTextBoxThickness.Text, out T))
                    //    T = 1.0;
                    return T;
                }
                catch
                {
                    return 2.0;
                }
            }
        }

        private double SymbolSize
        {
            get
            {
                try
                {
                    return 10.0;
                    //return double.Parse(this.toolStripTextBoxSize.Text);
                }
                catch
                {
                    return 10.0;
                }
            }
        }

        private byte FillTransparency
        {
            get
            {
                try
                {
                    byte Trans = 0;
                    //if (this.toolStripDropDownButtonSymbol.Image.Tag.ToString() == this.imageListSymbol.Images.Keys[1].ToString()
                    //    || this.toolStripDropDownButtonSymbol.Image.Tag.ToString() == this.imageListSymbol.Images.Keys[4].ToString()
                    //    || this.toolStripDropDownButtonSymbol.Image.Tag.ToString() == this.imageListSymbol.Images.Keys[7].ToString())
                    //    Trans = 255;
                    return Trans;
                }
                catch
                {
                    return 32;
                }
            }
        }

        private WpfSamplingPlotPage.PointSymbol Symbol
        {
            get
            {
                try
                {
                    return WpfSamplingPlotPage.PointSymbol.Pin;
                    //return (WpfSamplingPlotPage.PointSymbol)this.toolStripDropDownButtonSymbol.Tag;
                }
                catch
                {
                    return WpfSamplingPlotPage.PointSymbol.Pin;
                }
            }
        }

        private System.Windows.Media.Brush Brush
        {
            get
            {
                System.Windows.Media.Brush b = System.Windows.Media.Brushes.Red;
                try
                {
                    //if (this.toolStripDropDownButtonColor.Tag != null)
                    //    b = (System.Windows.Media.Brush)this.toolStripDropDownButtonColor.Tag;
                }
                catch { }
                return b;
            }
        }

        private void toolStripButtonGeographySave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._wpfControl.WpfGetGeoObjects(out this._GeoObjectsEditor)
                    && this._GeoObjectsEditor.Count == 1
                    && this._GeoObjectsEditor[0].GeometryData.Length > 0)
                {
                    //if (this.userControlEventSeriesTree.DtEventSeriesGeography.Rows.Count > 0)
                    //{
                    //    System.Data.DataRow[] RR = this.userControlEventSeriesTree.DtEventSeriesGeography.Select("SeriesID = " + this.SeriesID.ToString());
                    //    if (RR.Length > 0)
                    //    {

                    //    }
                    //}
                    string Geography = this._GeoObjectsEditor[0].GeometryData;
                    string SQL = "UPDATE CollectionEventSeries SET Geography = geography::STGeomFromText('" + Geography + "', 4326) " +
                        "WHERE SeriesID = " + this.SeriesID.ToString();
                    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);

                    System.Data.DataRow[] rr = this.userControlEventSeriesTree.DtEventSeriesGeography.Select("SeriesID = " + this.SeriesID.ToString());
                    if (rr.Length > 0)
                    {
                        rr[0][1] = Geography;
                    }
                    else
                    {
                        System.Data.DataRow R = this.userControlEventSeriesTree.DtEventSeriesGeography.NewRow();
                        R[0] = this.SeriesID;
                        R[1] = Geography;
                        this.userControlEventSeriesTree.DtEventSeriesGeography.Rows.Add(R);
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        #endregion

        #region Event series images

        private void fillEventSeriesImages()
        {
            try
            {
                this.FormFunctions.updateTable(this.dataSetCollectionEventSeries, "CollectionEventSeriesImage", this._sqlDataAdapterEventSeriesImage, this.BindingContext);
                this.listBoxImage.Items.Clear();
                this.dataSetCollectionEventSeries.CollectionEventSeriesImage.Rows.Clear();
                // Images
                if (this.userControlEventSeriesTree.DtEventSeries.Rows.Count > 0)
                {
                    if (this.SeriesID != null)
                    {
                        string SQL = DiversityCollection.CollectionSpecimen.SqlEventSeriesImage + " WHERE SeriesID = " + this.SeriesID.ToString();
                        this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeriesImage, SQL, this.dataSetCollectionEventSeries.CollectionEventSeriesImage);
                        this.FormFunctions.FillImageList(this.listBoxImage, this.imageListDataset, this.imageListForm, this.dataSetCollectionEventSeries.CollectionEventSeriesImage, "URI", this.userControlImage);
                    }
                }
                //if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 0)
                //{
                //    if (this.SeriesID != null)
                //    {
                //        string SQL = DiversityCollection.CollectionSpecimen.SqlEventSeriesImage + " WHERE SeriesID = " + this.SeriesID.ToString();
                //        this.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterEventSeriesImage, SQL, this.dataSetCollectionEventSeries.CollectionEventSeriesImage);
                //        this.FormFunctions.FillImageList(this.listBoxImage, this.imageListDataset, this.imageListForm, this.dataSetCollectionEventSeries.CollectionEventSeriesImage, "URI", this.userControlImage);
                //    }
                //}

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonImageNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.SeriesID == null) return;

                int AgentID = 0;
                if (DiversityWorkbench.Forms.FormFunctions.AgentID != null)
                    AgentID = (int)DiversityWorkbench.Forms.FormFunctions.AgentID;
                string RowGUID = System.Guid.NewGuid().ToString();
                DiversityWorkbench.Forms.FormGetImage f = new DiversityWorkbench.Forms.FormGetImage(_ProjectID, "");
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (f.ImagePath.Length > 0)
                    {
                        DiversityCollection.Datasets.DataSetCollectionEventSeries.CollectionEventSeriesImageRow R = this.dataSetCollectionEventSeries.CollectionEventSeriesImage.NewCollectionEventSeriesImageRow();
                        R.SeriesID = (int)this.SeriesID;
                        R.URI = f.URIImage;
                        this.dataSetCollectionEventSeries.CollectionEventSeriesImage.Rows.Add(R);
                        this.FormFunctions.FillImageList(this.listBoxImage, this.imageListDataset, this.imageListForm, this.dataSetCollectionEventSeries.CollectionEventSeriesImage, "URI", this.userControlImage);

                        //DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionEventImageRow R = this.dataSetCollectionSpecimen.CollectionEventImage.NewCollectionEventImageRow();
                        //int EventID = int.Parse(this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].ToString());
                        //R.CollectionEventID = EventID;
                        //R.URI = f.URIImage;
                        //this.dataSetCollectionSpecimen.CollectionEventImage.Rows.Add(R);
                        //this.FormFunctions.FillImageList(this.listBoxEventImages, this.imageListEventImages, this.imageListForm, this.dataSetCollectionSpecimen.CollectionEventImage, "URI", this.userControlImageEventImage);
                    }
                }



                //DiversityWorkbench.Forms.FormGetImage f = new DiversityWorkbench.Forms.FormGetImage();
                //if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{
                //    if (f.ImagePath.Length > 0)
                //    {
                //        DiversityCollection.Datasets.DataSetCollectionEventSeries.CollectionEventSeriesImageRow R = this.dataSetCollectionEventSeries.CollectionEventSeriesImage.NewCollectionEventSeriesImageRow();
                //        R.SeriesID = (int)this.SeriesID;
                //        R.URI = f.URIImage;
                //        this.dataSetCollectionEventSeries.CollectionEventSeriesImage.Rows.Add(R);
                //        this.FormFunctions.FillImageList(this.listBoxImage, this.imageListDataset, this.imageListForm, this.dataSetCollectionEventSeries.CollectionEventSeriesImage, "URI", this.userControlImageCollectionEventSeries);
                //    }
                //}
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButtonImageDelete_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.BindingManagerBase BMB = this.BindingContext[this.dataSetCollectionEventSeries, "CollectionEventSeriesImage"];
                int p = BMB.Position;
                System.Data.DataRow r = this.dataSetCollectionEventSeries.CollectionEventSeriesImage.Rows[p];
                if (r.RowState != System.Data.DataRowState.Deleted)
                {
                    this.dataSetCollectionEventSeries.CollectionEventSeriesImage.Rows[p].Delete();
                    this.FormFunctions.FillImageList(this.listBoxImage, this.imageListDataset, this.imageListForm, this.dataSetCollectionEventSeries.CollectionEventSeriesImage, "URI", this.userControlImage);
                    if (this.listBoxImage.Items.Count > 0) this.listBoxImage.SelectedIndex = 0;
                    else
                    {
                        this.listBoxImage.SelectedIndex = -1;
                        this.userControlImage.ImagePath = "";
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void listBoxImage_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                if (e.Index > -1)
                    this.imageListDataset.Draw(e.Graphics, e.Bounds.X, e.Bounds.Y, 50, 50, e.Index);
            }
            catch { }
        }

        private void listBoxImage_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = this.imageListDataset.ImageSize.Height;
            e.ItemWidth = this.imageListDataset.ImageSize.Width;
        }

        private void listBoxImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int i = this.listBoxImage.SelectedIndex;
                if (this.dataSetCollectionEventSeries.CollectionEventSeriesImage.Rows.Count == 0) return;
                for (int p = 0; p <= i; p++)
                {
                    if (this.dataSetCollectionEventSeries.CollectionEventSeriesImage.Rows[p].RowState == System.Data.DataRowState.Deleted) i++;
                }
                if (this.dataSetCollectionEventSeries.CollectionEventSeriesImage.Rows.Count > i)
                {
                    this.setEventSeriesImageControlsEnabled(true);
                    System.Data.DataRow r = this.dataSetCollectionEventSeries.CollectionEventSeriesImage.Rows[i];
                    this.userControlImage.ImagePath = r["URI"].ToString();
                    System.Windows.Forms.BindingManagerBase BMB = this.BindingContext[this.dataSetCollectionEventSeries, "CollectionEventSeriesImage"];
                    BMB.Position = i;

                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    string Restriction = "(Code LIKE 'audio%' OR Code LIKE 'video%')";
                    if (this.userControlImage.MediumType == DiversityWorkbench.Forms.FormFunctions.Medium.Image)
                        Restriction = "NOT " + Restriction;
                    DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxImageType, "CollEventSeriesImageType_Enum", con, true, true, true, Restriction);
                }
                else
                    this.setEventSeriesImageControlsEnabled(false);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void setEventSeriesImageControlsEnabled(bool IsEnabled)
        {
            this.comboBoxImageType.Enabled = IsEnabled;
            this.textBoxImageNotes.Enabled = IsEnabled;
        }

        #endregion

        #region Grid and Datahandling

        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DiversityCollection.Forms.FormGridFunctions.DrawRowNumber(this.dataGridView, this.dataGridView.Font, e);
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void setColumnWidth()
        {
            if (this.dataGridView.Columns.Count > 0)
            {
                int i = this.dataGridView.Columns.Count;
                int W = this.dataGridView.Width - this.dataGridView.RowHeadersWidth - this.dataGridView.Columns[i - 1].Width - this.dataGridView.Columns[i - 2].Width - this.dataGridView.Columns[i - 4].Width;
                this.dataGridView.Columns[i - 3].Width = W / 2;
                this.dataGridView.Columns[i - 5].Width = (W / 2) - 20;
            }
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //this.buttonGridModeDelete.Enabled = false;
                string ColumnName = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
                //this.labelGridViewReplaceColumnName.Text = ColumnName;
                //if (this.buttonGridModeFind.Enabled == false)
                //    this.buttonGridModeFind.Enabled = true;

                //if (this.dataGridView.SelectedCells.Count > 1)
                //    this.enableReplaceButtons(true);
                //else this.enableReplaceButtons(false);

                if (e.ColumnIndex == this.dataGridView.SelectedCells[0].ColumnIndex)
                {
                    switch (ColumnName)
                    {
                        case "DateStart":
                        case "DateEnd":
                            DiversityWorkbench.Forms.FormGetDate f = new DiversityWorkbench.Forms.FormGetDate(false);
                            f.ShowDialog();
                            if (f.DialogResult == DialogResult.OK)
                                this.dataGridView.SelectedCells[0].Value = f.Date;
                            break;
                        //case "Link_to_DiversityTaxonNames_of_second_organism":
                        //case "Link_to_DiversityAgents":
                        //case "Depositors_link_to_DiversityAgents":
                        //case "Link_to_DiversityExsiccatae":
                        //case "Link_to_DiversityReferences":
                        ////case "Link_to_DiversityReferences_of_second_organism":
                        //case "Link_to_DiversityAgents_for_responsible":
                        //    //case "Link_to_DiversityAgents_for_responsible_of_second_organism":
                        //    //this.GetRemoteValues(this.dataGridView.SelectedCells[0]);
                        //    break;
                        //case "On_loan":
                        //case "_TransactionID":
                        //    int TransactionID;
                        //    if (int.TryParse(this.dataGridView.SelectedCells[0].Value.ToString(), out TransactionID))
                        //    {
                        //        DiversityCollection.Forms.FormTransaction f = new FormTransaction(TransactionID);
                        //        f.ShowDialog();
                        //    }
                        //    else
                        //    {
                        //        string Message = "";
                        //        if (ColumnName == "On_loan") Message = "This dataset is not on loan";
                        //        if (ColumnName == "_TransactionID") Message = "This dataset is not involved in a transaction";
                        //        if (Message.Length > 0) System.Windows.Forms.MessageBox.Show(Message);
                        //    }
                        //    break;
                        //case "Remove_link_to_gazetteer":
                        //case "Remove_link_to_SamplingPlots":
                        //case "Remove_link_for_collector":
                        //case "Remove_link_for_Depositor":
                        //case "Remove_link_to_exsiccatae":
                        //case "Remove_link_for_identification":
                        //case "Remove_link_for_reference":
                        ////case "Remove_link_for_reference_of_second_organism":
                        ////case "Remove_link_for_determiner":
                        ////    //case "Remove_link_for_responsible_of_second_organism":
                        ////    //case "Remove_link_for_second_organism":
                        ////    this.RemoveLink(this.dataGridView.SelectedCells[0]);
                        ////    break;
                        ////case "Analysis":
                        ////    this.setAnalysis();
                        ////    break;
                        ////case "Analysis_result":
                        ////    if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[this.DatasetIndexOfCurrentLine]["AnalysisID"].Equals(System.DBNull.Value))
                        ////        this.setAnalysis();
                        //    break;
                        ////case "New_identification":
                        //case "New_identification_of_second_organism":
                        //    this.insertNewIdentification(this.dataGridView.SelectedCells[0]);
                        //    break;
                    }
                }
                //if (this.textBoxHeaderID.Text.Length == 0)
                //    this.setSpecimen(this.SpecimenID, this._UnitID);

            }
            catch { }
        }

        //private void dataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    try
        //    {
        //        string ColumnName = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
        //        this.labelGridViewReplaceColumnName.Text = ColumnName;
        //        if (this.buttonGridModeFind.Enabled == false)
        //            this.buttonGridModeFind.Enabled = true;
        //        if ((this.dataGridView.SelectedCells.Count > 1 && ColumnName != "CollectionSpecimenID")
        //            && (typeof(System.Windows.Forms.DataGridViewTextBoxCell) == this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].CellType
        //            && ColumnName != "CollectionSpecimenID")
        //            //|| typeof(System.Windows.Forms.DataGridViewComboBoxCell) == this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].CellType
        //            )
        //            this.enableReplaceButtons(true);
        //        else this.enableReplaceButtons(false);
        //        if (this.dataGridView.SelectedCells.Count == 1)
        //            this.labelGridCounter.Text = "line " + (this.dataGridView.SelectedCells[0].RowIndex + 1).ToString() + " of " + (this.dataGridView.Rows.Count - 1).ToString();
        //        else if (this.dataGridView.SelectedCells.Count > 1)
        //        {
        //            int StartLine = this.dataGridView.SelectedCells[0].RowIndex + 1;
        //            if (this.dataGridView.SelectedCells[this.dataGridView.SelectedCells.Count - 1].RowIndex + 1 < StartLine)
        //                StartLine = this.dataGridView.SelectedCells[this.dataGridView.SelectedCells.Count - 1].RowIndex + 1;
        //            this.labelGridCounter.Text = "line " + StartLine.ToString() + " to " +
        //                (this.dataGridView.SelectedCells.Count + StartLine - 1).ToString() + " of " +
        //                (this.dataGridView.Rows.Count - 1).ToString();
        //        }
        //        else
        //            this.labelGridCounter.Text = "line 1 of " + (this.dataGridView.Rows.Count - 1).ToString();

        //    }
        //    catch { }
        //}

        //private void dataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (this.dataGridView.SelectedCells.Count > 0 &&
        //             this.dataGridView.SelectedCells[0].EditedFormattedValue.ToString().Length > 0 &&
        //             this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count > 0)
        //        this.checkForMissingAndDefaultValues(this.dataGridView.SelectedCells[0], false);

        //}

        private void dataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //this.fillEventSeriesImages();
                //this.setHeaeder();
                //this.SetGeoObject();
                //int SpecimenID = 0;
                //int UnitID = 0;
                //int iCurrentLine = DatasetIndexOfCurrentLine - 1;
                //if (iCurrentLine == -1) iCurrentLine = 0;
                //if (this.dataGridView.SelectedCells.Count > 0 &&
                //    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count > 0 &&
                //    this.dataGridView.SelectedCells[0].RowIndex < this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count &&
                //    int.TryParse(this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[iCurrentLine]["CollectionSpecimenID"].ToString(), out SpecimenID) &&
                //    int.TryParse(this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[iCurrentLine]["IdentificationUnitID"].ToString(), out UnitID))
                //{
                //    if (SpecimenID != this._SpecimenID ||
                //        UnitID != this._UnitID)
                //    {
                //        this.setSpecimen(SpecimenID, UnitID);
                //        this._SpecimenID = SpecimenID;
                //        this._UnitID = UnitID;
                //    }
                //}
                //else if (this.dataGridView.SelectedCells.Count > 0 &&
                //    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count > 0 &&
                //    this.dataGridView.SelectedCells[0].RowIndex >= this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count)
                //{
                //    this.insertNewDataset(this.dataGridView.SelectedCells[0].RowIndex);
                //}
                //else if (this.dataGridView.SelectedCells.Count == 0
                //    && this.dataGridView.Rows.Count > 0
                //    && this.dataGridView.Rows[0].Cells[1].Value != null)
                //{
                //    if (int.TryParse(this.dataGridView.Rows[0].Cells[1].Value.ToString(), out SpecimenID) &&
                //        int.TryParse(this.dataGridView.Rows[0].Cells[0].Value.ToString(), out UnitID))
                //    {
                //        this.setSpecimen(SpecimenID, UnitID);
                //        this._SpecimenID = SpecimenID;
                //        this._UnitID = UnitID;
                //    }
                //}
                //else if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count == 0)
                //{
                //    this.insertNewDataset(0);
                //}
                //this.setCellBlockings();
                //this.setRemoveCellStyle();

            }
            catch { }
        }

        //private string GridModeFillCommand()
        //{
        //    string SQL = "SELECT " + this._SqlSpecimenFields + " FROM dbo.FirstLinesUnit_2 ";

        //    try
        //    {
        //        string WhereClause = "";
        //        foreach (int i in this._IDs)
        //        {
        //            WhereClause += i.ToString() + ", ";
        //        }
        //        if (WhereClause.Length == 0) WhereClause = " ('') ";
        //        else
        //            WhereClause = " ('" + WhereClause.Substring(0, WhereClause.Length - 2) + "'";
        //        WhereClause += ", ";
        //        if (DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.UseAnalysisIDs)
        //        {
        //            WhereClause += "'";
        //            foreach (string s in DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisIDs)
        //                WhereClause += s + ",";
        //            WhereClause = WhereClause.Substring(0, WhereClause.Length - 1) + "'";
        //        }
        //        else
        //            WhereClause += "null";
        //        WhereClause += ", ";
        //        if (DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.UseAnalysisEndDate)
        //        {
        //            WhereClause += "'" + DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisEndDate.Year.ToString() +
        //                "/" + DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisEndDate.Month.ToString() +
        //                "/" + DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisEndDate.Day.ToString() + "'";
        //        }
        //        else
        //            WhereClause += "null";
        //        WhereClause += ", ";
        //        if (DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.UseAnalysisStartDate)
        //        {
        //            WhereClause += "'" + DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisStartDate.Year.ToString() +
        //                "/" + DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisStartDate.Month.ToString() +
        //                "/" + DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisStartDate.Day.ToString() + "'";
        //        }
        //        else
        //            WhereClause += "null";
        //        WhereClause += ")";
        //        SQL += WhereClause + " ORDER BY Accession_number, CollectionSpecimenID, IdentificationUnitID ";
        //    }
        //    catch { }
        //    return SQL;
        //}

        //private void updateSpecimenFromGrid(System.Windows.Forms.DataGridViewRow Row)
        //{
        //    try
        //    {
        //        for (int i = 0; i < this.dataGridView.Columns.Count; i++)
        //        {
        //            string DataColumn = this.dataGridView.Columns[i].DataPropertyName;
        //            DiversityCollection.Forms.GridModeQueryField Q = this.GridModeGetQueryField(DataColumn);
        //            System.Data.DataTable DT = this.dataSetCollectionSpecimen.Tables[Q.Table];
        //            if (Q.Restriction.Length > 0)
        //            {
        //                System.Data.DataRow[] RR = DT.Select(Q.Restriction);
        //            }
        //            else
        //            {
        //                System.Data.DataRow R = DT.Rows[0];
        //            }
        //        }

        //    }
        //    catch { }
        //}

        //private void GridModeUpdate(int Index)
        //{
        //    try
        //    {
        //        if (this.dataSetIdentifictionUnitGridMode.HasChanges())
        //        {
        //            System.Data.DataRow RDataset = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index];
        //            if (RDataset.RowState == DataRowState.Modified || RDataset.RowState == DataRowState.Added)
        //            {
        //                // setting the dataset
        //                // the dataset is filled with the original data from the database as a basis for comparision with the data in the grid
        //                int CollectionSpecimenID = int.Parse(this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["CollectionSpecimenID"].ToString());
        //                int IdentificationUnitID = int.Parse(this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["IdentificationUnitID"].ToString());
        //                this._SpecimenID = CollectionSpecimenID;
        //                this._UnitID = IdentificationUnitID;
        //                this.fillSpecimen(this._SpecimenID, this._UnitID);

        //                // if no data 
        //                if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count == 0)
        //                    this.setSpecimen(this._SpecimenID, this.UnitID);

        //                // getting a list of all tables (Alias + TableName) from the grid
        //                System.Collections.Generic.Dictionary<string, string> Tables = new Dictionary<string, string>();
        //                int iii = 0;
        //                foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeQueryFields)
        //                {
        //                    if (Q.AliasForTable != null)
        //                    {
        //                        if (!Tables.ContainsKey(Q.AliasForTable))
        //                            Tables.Add(Q.AliasForTable, Q.Table);
        //                    }
        //                    else
        //                    {
        //                        if (!Tables.ContainsKey(Q.Table))
        //                            Tables.Add(Q.Table, Q.Table);
        //                    }
        //                    iii++;
        //                }

        //                // for every table (Alias) perform update of the entries as compared with the original data
        //                // or insert if the data are missing
        //                // the only exception is CollectionAgent as there the name is part of the PK
        //                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Tables)
        //                {
        //                    System.Collections.Generic.Dictionary<string, string> TableColumns = new Dictionary<string, string>();
        //                    System.Collections.Generic.Dictionary<string, string> ColumnValues = new Dictionary<string, string>();

        //                    // get all columns of the current table
        //                    iii = 0;
        //                    foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeQueryFields)
        //                    {
        //                        if (!TableColumns.ContainsKey(Q.AliasForColumn)
        //                            && Q.AliasForTable == KV.Key
        //                            && Q.Column.Length > 0
        //                            && !Q.AliasForColumn.StartsWith("Remove"))
        //                        {
        //                            TableColumns.Add(Q.AliasForColumn, Q.Column);
        //                            ColumnValues.Add(Q.Column, "");
        //                        }
        //                        iii++;
        //                    }

        //                    // no rows in the table - add new entry
        //                    // check if there are any values
        //                    bool AnyValuePresent = false;
        //                    foreach (System.Collections.Generic.KeyValuePair<string, string> KVc in TableColumns)
        //                    {
        //                        if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Columns.Contains(KVc.Key))
        //                        {
        //                            ColumnValues[KVc.Value] = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index][KVc.Key].ToString();
        //                            if (ColumnValues[KVc.Value].Length > 0)
        //                                AnyValuePresent = true;
        //                        }
        //                    }

        //                    if (this.dataSetCollectionSpecimen.Tables[KV.Value].Rows.Count == 0 &&
        //                        AnyValuePresent)
        //                        this.GridModeInsertNewData(KV.Value, KV.Key, Index);
        //                    else if (KV.Value == "CollectionEvent" &&
        //                        this.dataSetCollectionSpecimen.Tables[KV.Value].Rows.Count == 0 &&
        //                        !AnyValuePresent)
        //                    {
        //                        bool EventDependentDataPresent = false;
        //                        foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeQueryFields)
        //                        {
        //                            if (Q.Table == "CollectionEventLocalisation" || Q.Table == "CollectionEventProperty")
        //                            {
        //                                if (!RDataset[Q.AliasForColumn].Equals(System.DBNull.Value))
        //                                {
        //                                    EventDependentDataPresent = true;
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                        if (EventDependentDataPresent)
        //                            this.GridModeInsertNewData(KV.Value, KV.Key, Index);
        //                    }
        //                    else if (AnyValuePresent)
        //                    {
        //                        string WhereClause = "";
        //                        string Value = "";
        //                        bool containsHyphen = false;

        //                        // get the primary key columns of the table
        //                        System.Data.DataColumn[] PK = this.dataSetCollectionSpecimen.Tables[KV.Value].PrimaryKey;
        //                        System.Collections.Generic.List<string> PKColumns = new List<string>();
        //                        for (int i = 0; i < PK.Length; i++)
        //                            PKColumns.Add(PK[i].ColumnName);

        //                        // get the primary key with its original values
        //                        System.Collections.Generic.Dictionary<string, string> PKvalues = new Dictionary<string, string>();
        //                        foreach (System.Data.DataColumn Col in PK)
        //                        {
        //                            // get the column name in the view
        //                            string ColumnAlias = "";
        //                            foreach (System.Collections.Generic.KeyValuePair<string, string> KvPK in TableColumns)
        //                            {
        //                                if (KvPK.Value == Col.ColumnName)
        //                                {
        //                                    ColumnAlias = KvPK.Key;
        //                                    PKvalues.Add(KvPK.Key, "");
        //                                    break;
        //                                }
        //                            }

        //                            // get the value of the column
        //                            // TODO: Notlösung - bei ' wird mit LIKE und _ gesucht
        //                            if (ColumnAlias.Length > 0 && !RDataset[ColumnAlias].Equals(System.DBNull.Value))
        //                            {
        //                                PKvalues[ColumnAlias] = RDataset[ColumnAlias].ToString();
        //                                Value = RDataset[ColumnAlias].ToString();
        //                                if (Value.IndexOf("'") > -1)
        //                                {
        //                                    containsHyphen = true;
        //                                }
        //                                if (WhereClause.Length > 0) WhereClause += " AND ";
        //                                if (containsHyphen)
        //                                {
        //                                    string[] WW = Value.Split(new char[] { '\'' });
        //                                    for (int i = 0; i < WW.Length; i++)
        //                                    {
        //                                        if (WW[i].Length > 0)
        //                                        {
        //                                            if (i > 0 && WhereClause.Length > 0) WhereClause += " AND ";
        //                                            WhereClause += " " + Col.ColumnName;
        //                                            WhereClause += " LIKE '";
        //                                            if (i > 0) WhereClause += "%";
        //                                            WhereClause += WW[i];
        //                                            if (i < WW.Length) WhereClause += "%";
        //                                            WhereClause += "' ";
        //                                        }
        //                                    }
        //                                }
        //                                else WhereClause += " " + Col.ColumnName + " = '" + Value + "' ";
        //                            }
        //                            else
        //                            {
        //                                if (Col.ColumnName != "CollectionSpecimenID")
        //                                {
        //                                    switch (Col.ColumnName)
        //                                    {
        //                                        case "IdentificationUnitID":
        //                                            switch (KV.Key)
        //                                            {
        //                                                case "Identification":
        //                                                    if (WhereClause.Length > 0) WhereClause += " AND ";
        //                                                    WhereClause += " IdentificationUnitID = " + this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["IdentificationUnitID"].ToString();
        //                                                    break;
        //                                                case "IdentificationUnitAnalysis":
        //                                                    break;
        //                                            }
        //                                            break;
        //                                        case "AnalysisID":
        //                                            switch (KV.Key)
        //                                            {
        //                                                case "Analysis_0":
        //                                                    if (WhereClause.Length > 0) WhereClause += " AND ";
        //                                                    WhereClause += " AnalysisID = " + this._AnalysisList[0].AnalysisID;
        //                                                    break;
        //                                                case "Analysis_1":
        //                                                    if (WhereClause.Length > 0) WhereClause += " AND ";
        //                                                    WhereClause += " AnalysisID = " + this._AnalysisList[1].AnalysisID;
        //                                                    break;
        //                                                case "Analysis_2":
        //                                                    if (WhereClause.Length > 0) WhereClause += " AND ";
        //                                                    WhereClause += " AnalysisID = " + this._AnalysisList[2].AnalysisID;
        //                                                    break;
        //                                                case "Analysis_3":
        //                                                    if (WhereClause.Length > 0) WhereClause += " AND ";
        //                                                    WhereClause += " AnalysisID = " + this._AnalysisList[3].AnalysisID;
        //                                                    break;
        //                                                case "Analysis_4":
        //                                                    if (WhereClause.Length > 0) WhereClause += " AND ";
        //                                                    WhereClause += " AnalysisID = " + this._AnalysisList[4].AnalysisID;
        //                                                    break;
        //                                                case "Analysis_5":
        //                                                    if (WhereClause.Length > 0) WhereClause += " AND ";
        //                                                    WhereClause += " AnalysisID = " + this._AnalysisList[5].AnalysisID;
        //                                                    break;
        //                                                case "Analysis_6":
        //                                                    if (WhereClause.Length > 0) WhereClause += " AND ";
        //                                                    WhereClause += " AnalysisID = " + this._AnalysisList[6].AnalysisID;
        //                                                    break;
        //                                                case "Analysis_7":
        //                                                    if (WhereClause.Length > 0) WhereClause += " AND ";
        //                                                    WhereClause += " AnalysisID = " + this._AnalysisList[7].AnalysisID;
        //                                                    break;
        //                                                case "Analysis_8":
        //                                                    if (WhereClause.Length > 0) WhereClause += " AND ";
        //                                                    WhereClause += " AnalysisID = " + this._AnalysisList[8].AnalysisID;
        //                                                    break;
        //                                                case "Analysis_9":
        //                                                    if (WhereClause.Length > 0) WhereClause += " AND ";
        //                                                    WhereClause += " AnalysisID = " + this._AnalysisList[9].AnalysisID;
        //                                                    break;
        //                                            }
        //                                            break;
        //                                        case "IdentificationSequence":
        //                                            switch (KV.Key)
        //                                            {
        //                                                case "Identification":
        //                                                    if (!this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_IdentificationSequence"].Equals(System.DBNull.Value))
        //                                                    {
        //                                                        if (WhereClause.Length > 0) WhereClause += " AND ";
        //                                                        WhereClause += " IdentificationSequence = " + this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_IdentificationSequence"].ToString();
        //                                                    }
        //                                                    break;
        //                                                case "SecondUnitIdentification":
        //                                                    if (!this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_SecondSequence"].Equals(System.DBNull.Value))
        //                                                    {
        //                                                        if (WhereClause.Length > 0) WhereClause += " AND ";
        //                                                        WhereClause += " IdentificationSequence = " + this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_SecondSequence"].ToString();
        //                                                    }
        //                                                    break;
        //                                            }
        //                                            break;
        //                                    }
        //                                }
        //                            }
        //                        }

        //                        // get a possible restriction
        //                        foreach (DiversityCollection.Forms.GridModeQueryField Qrest in this.GridModeQueryFields)
        //                        {
        //                            if (Qrest.AliasForTable == KV.Key && Qrest.Restriction != null && Qrest.Restriction.Length > 0)
        //                            {
        //                                if (WhereClause.Length > 0)
        //                                    WhereClause += " AND " + Qrest.Restriction;
        //                                else
        //                                    WhereClause = Qrest.Restriction;
        //                                break;
        //                            }
        //                        }
        //                        try
        //                        {
        //                            int DisplayOrder = 1;
        //                            System.Data.DataRow[] RR = this.dataSetCollectionSpecimen.Tables[KV.Value].Select(WhereClause);

        //                            // special treatment for Agent as CollectorsName is contained in the PK
        //                            if (RR.Length == 0 && KV.Value == "CollectionAgent" && WhereClause.StartsWith(" CollectorsName "))
        //                            {
        //                                string NewCollectorsName = "";
        //                                if (containsHyphen)
        //                                    NewCollectorsName = Value.Trim();
        //                                else
        //                                    NewCollectorsName = WhereClause.Substring(18).Replace("'", "").Trim();
        //                                if (NewCollectorsName.Length > 0)
        //                                {
        //                                    RR = this.dataSetCollectionSpecimen.Tables[KV.Value].Select("", "CollectorsSequence");
        //                                    RR[0]["CollectorsName"] = NewCollectorsName;
        //                                }
        //                                if (RR.Length > 1)
        //                                {
        //                                    RR = this.dataSetCollectionSpecimen.Tables[KV.Value].Select("CollectorsName <> ''", "CollectorsSequence");
        //                                    if (RR.Length > 1)
        //                                        RR = this.dataSetCollectionSpecimen.Tables[KV.Value].Select("CollectorsName = '" + RR[0].ToString() + "'", "CollectorsSequence");
        //                                }
        //                            }

        //                            if (RR.Length == 1)// && WhereClause.Length > 0)
        //                            {
        //                                // Check all columns in the table if the value has changed and write changes in the dataset
        //                                foreach (System.Collections.Generic.KeyValuePair<string, string> KvTableColumn in TableColumns)
        //                                {
        //                                    if (!PKColumns.Contains(KvTableColumn.Value) &&
        //                                        RR[0].Table.Columns.Contains(KvTableColumn.Value) &&
        //                                        RR[0][KvTableColumn.Value].ToString() != RDataset[KvTableColumn.Key].ToString())
        //                                    {
        //                                        RR[0][KvTableColumn.Value] = RDataset[KvTableColumn.Key];
        //                                    }
        //                                }

        //                                if (KV.Key == "SecondUnit")
        //                                {
        //                                    System.Data.DataRow[] RRDisplayOrder = this.dataSetCollectionSpecimen.IdentificationUnit.Select("DisplayOrder > 0", "DisplayOrder");
        //                                    if (RRDisplayOrder.Length > 0)
        //                                    {
        //                                        if (int.TryParse(RRDisplayOrder[0]["DisplayOrder"].ToString(), out DisplayOrder))
        //                                        {
        //                                            System.Data.DataRow[] RRsecond = this.dataSetCollectionSpecimen.IdentificationUnit.Select("DisplayOrder > " + DisplayOrder.ToString(), "DisplayOrder");
        //                                            if (RRsecond.Length > 0)
        //                                            {
        //                                                if (int.TryParse(RRsecond[0]["DisplayOrder"].ToString(), out DisplayOrder))
        //                                                {
        //                                                    RR[0]["DisplayOrder"] = DisplayOrder;
        //                                                }
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                if (KV.Key == "CollectionSpecimen")
        //                                {
        //                                    if (!this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_CollectionEventID"].Equals(System.DBNull.Value) &&
        //                                        this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].Equals(System.DBNull.Value))
        //                                        this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"] = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_CollectionEventID"];
        //                                }
        //                            }
        //                            else if (containsHyphen && RR.Length > 1)
        //                            {
        //                                // if the PK contains values that can not be separated on the basis of like statements
        //                                // according to the parts split by the hyphens, e.g. "a'b'c'd" and "a'c'b'd"
        //                                System.Data.DataRow Rhyphen = RR[0];
        //                                for (int i = 0; i < RR.Length; i++)
        //                                {
        //                                    foreach (System.Data.DataColumn C in RR[0].Table.Columns)
        //                                    {
        //                                        foreach (System.Collections.Generic.KeyValuePair<string, string> KVpk in PKvalues)
        //                                        {
        //                                            string Column = this.ColumnNameForAlias(KVpk.Key);
        //                                            if (Column == C.ColumnName &&
        //                                                KVpk.Value == RR[i][Column].ToString())
        //                                            {
        //                                                Rhyphen = RR[i];
        //                                                goto Found;
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            Found:
        //                                // Check all columns in the table if the value has changed and write changes in the dataset
        //                                foreach (System.Collections.Generic.KeyValuePair<string, string> KvTableColumn in TableColumns)
        //                                {
        //                                    if (!PKColumns.Contains(KvTableColumn.Value) &&
        //                                        Rhyphen.Table.Columns.Contains(KvTableColumn.Value) &&
        //                                        Rhyphen[KvTableColumn.Value].ToString() != RDataset[KvTableColumn.Key].ToString())
        //                                    {
        //                                        Rhyphen[KvTableColumn.Value] = RDataset[KvTableColumn.Key];
        //                                    }
        //                                }

        //                            }
        //                            else
        //                            {
        //                                this.GridModeInsertNewData(KV.Value, KV.Key, Index);
        //                            }
        //                        }
        //                        catch { }
        //                    }
        //                }
        //                this.updateSpecimen();
        //                RDataset.AcceptChanges();
        //            }
        //        }
        //    }
        //    catch { }
        //}

        //private void GridModeInsertNewData(string Table, string AliasForTable, int Index)
        //{
        //    bool PKcontainsIdentity = false;
        //    // security checks for dependent tables
        //    // Second Identification
        //    try
        //    {
        //        if (Table == "Identification" && AliasForTable == "SecondUnitIdentification")
        //        {
        //            if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_group_of_second_organism"].Equals(System.DBNull.Value) ||
        //                this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_group_of_second_organism"].ToString().Length == 0)
        //            {
        //                string Message = "Please enter the taxonomic group for the second organism ";
        //                if (!this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name_of_second_organism"].Equals(System.DBNull.Value))
        //                    Message += this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name_of_second_organism"].ToString() + " ";
        //                if (!this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Accession_number"].Equals(System.DBNull.Value))
        //                    Message += this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Accession_number"].ToString();
        //                System.Windows.Forms.MessageBox.Show(Message);
        //            }
        //        }
        //        // get all columns of the table
        //        System.Collections.Generic.Dictionary<string, string> TableColumns = new Dictionary<string, string>();
        //        System.Collections.Generic.Dictionary<string, string> ColumnValues = new Dictionary<string, string>();
        //        foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeQueryFields)
        //        {
        //            if (!TableColumns.ContainsKey(Q.AliasForColumn)
        //                && Q.AliasForTable == AliasForTable
        //                && !Q.AliasForColumn.StartsWith("Remove"))
        //            {
        //                TableColumns.Add(Q.AliasForColumn, Q.Column);
        //                ColumnValues.Add(Q.Column, "");
        //            }
        //        }

        //        foreach (System.Collections.Generic.KeyValuePair<string, string> KVc in TableColumns)
        //        {
        //            ColumnValues[KVc.Value] = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index][KVc.Key].ToString();
        //        }

        //        // get the primary key columns of the table
        //        System.Data.DataColumn[] PK = this.dataSetCollectionSpecimen.Tables[Table].PrimaryKey;
        //        System.Collections.Generic.Dictionary<string, string> PKColumns = new Dictionary<string, string>();
        //        for (int i = 0; i < PK.Length; i++)
        //        {
        //            PKColumns.Add(PK[i].ColumnName, "");
        //            if (PK[i].AutoIncrement)
        //                PKcontainsIdentity = true;
        //        }
        //        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in PKColumns)
        //        {
        //            if (!TableColumns.ContainsValue(KV.Key))
        //            {
        //                string AliasForColumn = "";
        //                foreach (DiversityCollection.Forms.GridModeQueryField Q in this.GridModeQueryFields)
        //                {
        //                    if (Q.Column == KV.Key
        //                        && (Q.AliasForTable == AliasForTable
        //                        || AliasForTable.StartsWith(Q.AliasForTable)))
        //                    {
        //                        AliasForColumn = Q.AliasForColumn;
        //                        break;
        //                    }
        //                }
        //                if (AliasForColumn.Length > 0)
        //                {
        //                    TableColumns.Add(AliasForColumn, KV.Key);
        //                    ColumnValues.Add(KV.Key, "");
        //                }
        //                else
        //                {
        //                    TableColumns.Add(KV.Key, KV.Key);
        //                    ColumnValues.Add(KV.Key, "");
        //                }
        //            }
        //        }

        //        // if there is a restriction, get the column and the value
        //        string ResColumn = "";
        //        string ResValue = "";
        //        char[] charSeparators = new char[] { '=' };
        //        string[] Parameter = null;
        //        foreach (DiversityCollection.Forms.GridModeQueryField Qrest in this.GridModeQueryFields)
        //        {
        //            if (Qrest.AliasForTable == AliasForTable && Qrest.Restriction != null && Qrest.Restriction.Length > 0)
        //            {
        //                Parameter = Qrest.Restriction.Split(charSeparators);
        //                break;
        //            }
        //        }
        //        if (Parameter != null && Parameter.Length > 1)
        //        {
        //            ResColumn = Parameter[0];
        //            ResValue = Parameter[1];
        //        }
        //        if (ColumnValues.ContainsKey(ResColumn))
        //            ColumnValues[ResColumn] = ResValue;

        //        foreach (System.Collections.Generic.KeyValuePair<string, string> KVpk in PKColumns)
        //        {
        //            if (ColumnValues[KVpk.Key] == "")
        //            {
        //                // try to get the value from the table
        //                string AliasForColumn = "";
        //                foreach (System.Collections.Generic.KeyValuePair<string, string> KVcol in TableColumns)
        //                {
        //                    if (KVcol.Value == KVpk.Key)
        //                    {
        //                        AliasForColumn = KVcol.Key;
        //                        break;
        //                    }
        //                }
        //                if (!this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Columns.Contains(AliasForColumn))
        //                {
        //                    foreach (DiversityCollection.Forms.GridModeQueryField Qrest in this.GridModeQueryFields)
        //                    {
        //                        if (Qrest.Column == KVpk.Key)
        //                        {
        //                            AliasForColumn = Qrest.AliasForColumn;
        //                            break;
        //                        }
        //                    }
        //                }
        //                ColumnValues[KVpk.Key] = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index][AliasForColumn].ToString();
        //            }
        //        }

        //        // test if all PrimaryKey values are set
        //        string TaxonomicGroup = "";
        //        int DisplayOrder = 1;
        //        string LastIdentification = "";
        //        foreach (System.Collections.Generic.KeyValuePair<string, string> KVpk in PKColumns)
        //        {
        //            if (ColumnValues[KVpk.Key].Length == 0)
        //            {
        //                // the value for a primary key column is missing
        //                switch (KVpk.Key)
        //                {
        //                    case "CollectionSpecimenID":
        //                        // if a new line is entered in the datagrid
        //                        if (Table == "CollectionSpecimen")
        //                        {
        //                            int CollectionSpecimenID = 0;
        //                            if (int.TryParse(this.InsertNewSpecimen(Index).ToString(), out CollectionSpecimenID))
        //                            {
        //                                if (CollectionSpecimenID < 0)
        //                                    return;
        //                                else
        //                                    ColumnValues["CollectionSpecimenID"] = CollectionSpecimenID.ToString();
        //                            }
        //                        }
        //                        break;
        //                    case "CollectionEventID":
        //                        // if a previously empty part related to the event is filled
        //                        string Locality = "";
        //                        if (!this.dataSetIdentifictionUnitGridMode.FirstLinesUnit[Index]["Locality_description"].Equals(System.DBNull.Value))
        //                            this.dataSetIdentifictionUnitGridMode.FirstLinesUnit[Index]["Locality_description"].ToString();
        //                        Locality = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit[Index]["Locality_description"].ToString();
        //                        int EventID = this.createEvent(Index);
        //                        this.dataSetIdentifictionUnitGridMode.FirstLinesUnit[Index]["_CollectionEventID"] = EventID;
        //                        ColumnValues["CollectionEventID"] = EventID.ToString();
        //                        if (Locality.Length > 0
        //                            && ColumnValues.ContainsKey("LocalityDescription"))
        //                            ColumnValues["LocalityDescription"] = Locality;
        //                        break;
        //                    case "IdentificationUnitID":
        //                        // if a previously empty part related to the unit or second unit is filled
        //                        switch (AliasForTable)
        //                        {
        //                            case "IdentificationUnit":
        //                                TaxonomicGroup = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_group"].ToString();
        //                                if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name"].Equals(System.DBNull.Value) ||
        //                                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name"].ToString().Length == 0)
        //                                    LastIdentification = TaxonomicGroup;
        //                                else
        //                                    LastIdentification = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name"].ToString();
        //                                DisplayOrder = 1;
        //                                //this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Last_identification"] = LastIdentification;
        //                                break;
        //                            case "SecondUnit":
        //                                TaxonomicGroup = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_group_of_second_organism"].ToString();
        //                                if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name_of_second_organism"].Equals(System.DBNull.Value) ||
        //                                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name_of_second_organism"].ToString().Length == 0)
        //                                    LastIdentification = TaxonomicGroup;
        //                                else
        //                                    LastIdentification = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name_of_second_organism"].ToString();
        //                                DisplayOrder = 2;
        //                                break;
        //                        }
        //                        if (TaxonomicGroup.Length > 0 && LastIdentification.Length > 0)
        //                        {
        //                            int UnitID = this.createIdentificationUnit(Index, AliasForTable);
        //                            ColumnValues[KVpk.Key] = UnitID.ToString();
        //                            switch (AliasForTable)
        //                            {
        //                                case "IdentificationUnit":
        //                                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_IdentificationUnitID"] = UnitID;
        //                                    break;
        //                                case "SecondUnit":
        //                                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_SecondUnitID"] = UnitID;
        //                                    break;
        //                            }
        //                        }
        //                        break;
        //                    case "IdentificationSequence":
        //                        // if a previously empty part related to the identification of the unit or second unit is filled
        //                        System.Data.DataRow[] RRIdent = this.dataSetCollectionSpecimen.Identification.Select("", "IdentificationSequence DESC");
        //                        if (RRIdent.Length > 0)
        //                        {
        //                            int Sequence = int.Parse(RRIdent[0]["IdentificationSequence"].ToString()) + 1;
        //                            ColumnValues[KVpk.Key] = Sequence.ToString();
        //                            switch (AliasForTable)
        //                            {
        //                                case "Identification":
        //                                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_IdentificationSequence"] = Sequence;
        //                                    break;
        //                                case "IdentificationSecondUnit":
        //                                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_SecondSequence"] = Sequence;
        //                                    break;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ColumnValues[KVpk.Key] = "1";
        //                            switch (AliasForTable)
        //                            {
        //                                case "Identification":
        //                                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_IdentificationSequence"] = 1;
        //                                    break;
        //                                case "IdentificationSecondUnit":
        //                                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_SecondSequence"] = 1;
        //                                    break;
        //                            }
        //                        }
        //                        break;
        //                    case "SpecimenPartID":
        //                        System.Data.DataRow[] RRPart = this.dataSetCollectionSpecimen.CollectionSpecimenPart.Select("", "SpecimenPartID DESC");
        //                        if (RRPart.Length > 0)
        //                        {
        //                            int PartID = int.Parse(RRPart[0]["SpecimenPartID"].ToString()) + 1;
        //                            ColumnValues[KVpk.Key] = PartID.ToString();
        //                            this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["_SpecimenPartID"] = PartID;
        //                        }
        //                        break;
        //                }
        //            }
        //        }

        //        System.Data.DataRow Rnew = this.dataSetCollectionSpecimen.Tables[Table].NewRow();
        //        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in ColumnValues)
        //        {
        //            if (Rnew.Table.Columns.Contains(KV.Key))
        //            {
        //                if (KV.Value.Length > 0)
        //                    Rnew[KV.Key] = KV.Value;
        //                else
        //                {
        //                    if (Rnew.Table.Columns[KV.Key].DataType == typeof(int) ||
        //                        Rnew.Table.Columns[KV.Key].DataType == typeof(System.Int16) ||
        //                        Rnew.Table.Columns[KV.Key].DataType == typeof(System.DateTime))
        //                        Rnew[KV.Key] = System.DBNull.Value;
        //                    else if (Rnew[KV.Key].GetType() == typeof(string))
        //                        Rnew[KV.Key] = "";
        //                }
        //            }
        //        }
        //        if (Table == "IdentificationUnit" &&
        //            (Rnew["LastIdentificationCache"].Equals(System.DBNull.Value) ||
        //            Rnew["LastIdentificationCache"].ToString().Length == 0)
        //            && !Rnew["TaxonomicGroup"].Equals(System.DBNull.Value))
        //        {
        //            if (LastIdentification.Length > 0)
        //                Rnew["LastIdentificationCache"] = LastIdentification;
        //            else Rnew["LastIdentificationCache"] = Rnew["TaxonomicGroup"];
        //        }

        //        if (Table == "IdentificationUnit" &&
        //            Rnew["DisplayOrder"].Equals(System.DBNull.Value))
        //            Rnew["DisplayOrder"] = DisplayOrder;

        //        if (Table == "CollectionEvent" &&
        //            Rnew["Version"].Equals(System.DBNull.Value))
        //            Rnew["Version"] = 1;

        //        if (Table == "CollectionSpecimen" &&
        //            Rnew["Version"].Equals(System.DBNull.Value))
        //            Rnew["Version"] = 1;

        //        if (Table == "CollectionSpecimen" &&
        //            Rnew["CollectionEventID"].Equals(System.DBNull.Value) &&
        //            !this.dataSetIdentifictionUnitGridMode.FirstLinesUnit[Index]["_CollectionEventID"].Equals(System.DBNull.Value))
        //            Rnew["CollectionEventID"] = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit[Index]["_CollectionEventID"];

        //        if (Table == "CollectionSpecimenPart" &&
        //            Rnew["SpecimenPartID"].Equals(System.DBNull.Value))
        //            Rnew["SpecimenPartID"] = 1;

        //        if (Table == "IdentificationUnitAnalysis" &&
        //            Rnew["AnalysisNumber"].Equals(System.DBNull.Value))
        //            Rnew["AnalysisNumber"] = "1";

        //        if (Table == "IdentificationUnitAnalysis" &&
        //            Rnew["AnalysisID"].Equals(System.DBNull.Value))
        //        {
        //            switch (AliasForTable)
        //            {
        //                case "Analysis_0":
        //                    Rnew["AnalysisID"] = this._AnalysisList[0].AnalysisID;
        //                    break;
        //                case "Analysis_1":
        //                    Rnew["AnalysisID"] = this._AnalysisList[1].AnalysisID;
        //                    break;
        //                case "Analysis_2":
        //                    Rnew["AnalysisID"] = this._AnalysisList[2].AnalysisID;
        //                    break;
        //                case "Analysis_3":
        //                    Rnew["AnalysisID"] = this._AnalysisList[3].AnalysisID;
        //                    break;
        //                case "Analysis_4":
        //                    Rnew["AnalysisID"] = this._AnalysisList[4].AnalysisID;
        //                    break;
        //                case "Analysis_5":
        //                    Rnew["AnalysisID"] = this._AnalysisList[5].AnalysisID;
        //                    break;
        //                case "Analysis_6":
        //                    Rnew["AnalysisID"] = this._AnalysisList[6].AnalysisID;
        //                    break;
        //                case "Analysis_7":
        //                    Rnew["AnalysisID"] = this._AnalysisList[7].AnalysisID;
        //                    break;
        //                case "Analysis_8":
        //                    Rnew["AnalysisID"] = this._AnalysisList[8].AnalysisID;
        //                    break;
        //                case "Analysis_9":
        //                    Rnew["AnalysisID"] = this._AnalysisList[9].AnalysisID;
        //                    break;
        //            }
        //        }

        //        if (Table == "IdentificationUnitAnalysis" &&
        //            Rnew["AnalysisDate"].Equals(System.DBNull.Value))
        //        {
        //            Rnew["AnalysisDate"] = System.DateTime.Now.Year.ToString() + "/" + System.DateTime.Now.Month.ToString() + "/" + System.DateTime.Now.Day.ToString();
        //        }


        //        // setting default for responsible
        //        if (Table == "Identification" &&
        //            Rnew["ResponsibleName"].Equals(System.DBNull.Value) &&
        //            Specimen.DefaultUseIdentificationResponsible)
        //        {
        //            Rnew["ResponsibleName"] = Specimen.DefaultResponsibleName;
        //            Rnew["ResponsibleAgentURI"] = Specimen.DefaultResponsibleURI;
        //        }

        //        if (Table == "CollectionAgent" &&
        //            Rnew["CollectorsName"].Equals(System.DBNull.Value) &&
        //            Specimen.DefaultUseCollector)
        //        {
        //            Rnew["CollectorsName"] = Specimen.DefaultResponsibleName;
        //            Rnew["CollectorsAgentURI"] = Specimen.DefaultResponsibleURI;
        //        }

        //        if (Table == "CollectionEventLocalisation" &&
        //            Rnew["ResponsibleName"].Equals(System.DBNull.Value) &&
        //            Specimen.DefaultUseLocalisationResponsible)
        //        {
        //            Rnew["ResponsibleName"] = Specimen.DefaultResponsibleName;
        //            Rnew["ResponsibleAgentURI"] = Specimen.DefaultResponsibleURI;
        //        }

        //        if (Table == "CollectionEventProperty" &&
        //            Rnew["ResponsibleName"].Equals(System.DBNull.Value) &&
        //            Specimen.DefaultUseEventPropertiyResponsible)
        //        {
        //            Rnew["ResponsibleName"] = Specimen.DefaultResponsibleName;
        //            Rnew["ResponsibleAgentURI"] = Specimen.DefaultResponsibleURI;
        //        }

        //        if (Table == "IdentificationUnitAnalysis" &&
        //            Rnew["ResponsibleName"].Equals(System.DBNull.Value) &&
        //            Specimen.DefaultUseAnalyisisResponsible)
        //        {
        //            Rnew["ResponsibleName"] = Specimen.DefaultResponsibleName;
        //            Rnew["ResponsibleAgentURI"] = Specimen.DefaultResponsibleURI;
        //        }

        //        if (Table == "CollectionSpecimenProcessing" &&
        //            Rnew["ResponsibleName"].Equals(System.DBNull.Value) &&
        //            Specimen.DefaultUseProcessingResponsible)
        //        {
        //            Rnew["ResponsibleName"] = Specimen.DefaultResponsibleName;
        //            Rnew["ResponsibleAgentURI"] = Specimen.DefaultResponsibleURI;
        //        }

        //        this.dataSetCollectionSpecimen.Tables[Table].Rows.Add(Rnew);
        //        if (PKcontainsIdentity)
        //            Rnew.AcceptChanges();
        //    }
        //    catch { }
        //}

        //#region Remote services

        //private void GetCoordinatesFromGoogleMaps()
        //{
        //    string Latitude = "";
        //    string Longitude = "";
        //    try
        //    {
        //        if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[DatasetIndexOfCurrentLine]["Latitude"].Equals(System.DBNull.Value))
        //        {
        //            Latitude = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[DatasetIndexOfCurrentLine]["Latitude"].ToString();
        //            Longitude = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[DatasetIndexOfCurrentLine]["Longitude"].ToString();
        //        }
        //        if (Latitude.Length == 0)
        //        {
        //            System.Windows.Forms.DataGridViewRow Row = this.dataGridView.Rows[this.dataGridView.SelectedCells[0].RowIndex];
        //            foreach (System.Windows.Forms.DataGridViewCell C in Row.Cells)
        //            {
        //                if (this.dataGridView.Columns[C.ColumnIndex].DataPropertyName == "Latitude"
        //                    && C.Value != null)
        //                    Latitude = C.Value.ToString();
        //                if (this.dataGridView.Columns[C.ColumnIndex].DataPropertyName == "Longitude"
        //                    && C.Value != null)
        //                    Longitude = C.Value.ToString();
        //                if (Longitude.Length > 0 && Latitude.Length > 0) break;
        //            }
        //        }
        //        bool OK = false;
        //        DiversityWorkbench.Forms.FormGoogleMapsCoordinates f;

        //        System.Globalization.CultureInfo InvC = new System.Globalization.CultureInfo("");

        //        double la = 0.0;
        //        double lo = 0.0;
        //        string Lat = Latitude.ToString(InvC).Replace(",", ".");
        //        string Long = Longitude.ToString(InvC).Replace(",", ".");
        //        if (Lat.Length > 0 && Long.Length > 0)
        //        {
        //            OK = true;
        //            if (!double.TryParse(Lat, NumberStyles.Float, InvC, out la)) OK = false;
        //            if (!double.TryParse(Long, NumberStyles.Float, InvC, out lo)) OK = false;
        //        }
        //        if (!OK)
        //        {
        //            // try to find existing coordinates
        //            System.Data.DataRow[] rrC = this.dataSetCollectionSpecimen.CollectionEventLocalisation.Select("NOT AverageLatitudeCache IS NULL AND NOT AverageLongitudeCache IS NULL");
        //            if (rrC.Length > 0)
        //            {
        //                OK = true;
        //                if (!double.TryParse(rrC[0]["AverageLatitudeCache"].ToString(), NumberStyles.Float, InvC, out la)) OK = false;
        //                if (!double.TryParse(rrC[0]["AverageLongitudeCache"].ToString(), NumberStyles.Float, InvC, out lo)) OK = false;
        //            }
        //        }

        //        if (!OK)
        //        {
        //            try
        //            {
        //                System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.CollectionEventLocalisation.Select("AverageLatitudeCache  <> 0 AND AverageLongitudeCache <> 0", "AverageLatitudeCache DESC");
        //                if (rr.Length > 0)
        //                {
        //                    OK = true;
        //                    if (!double.TryParse(rr[0]["AverageLatitudeCache"].ToString(), NumberStyles.Float, InvC, out la)) OK = false;
        //                    if (!double.TryParse(rr[0]["AverageLongitudeCache"].ToString(), NumberStyles.Float, InvC, out lo)) OK = false;
        //                }
        //            }
        //            catch { OK = false; }
        //        }
        //        if (OK) f = new DiversityWorkbench.Forms.FormGoogleMapsCoordinates(la, lo);
        //        else f = new DiversityWorkbench.Forms.FormGoogleMapsCoordinates(0.0, 0.0);
        //        f.ShowDialog();
        //        if (f.DialogResult == DialogResult.OK)
        //        {
        //            try
        //            {
        //                if (f.Longitude < 180 && f.Longitude > -180 && f.Latitude < 180 && f.Latitude > -180 && f.LatitudeAccuracy != 0.0 && f.LongitudeAccuracy != 0.0)
        //                {
        //                    System.Data.DataRowView R = (System.Data.DataRowView)this.firstLinesUnitBindingSource.Current;
        //                    R["Longitude"] = f.Longitude.ToString("F09", InvC);
        //                    R["Latitude"] = f.Latitude.ToString("F09", InvC);
        //                    R["Coordinates_accuracy"] = f.Accuracy.ToString("F00", InvC) + " m";
        //                    R["_CoordinatesLocationAccuracy"] = f.Accuracy.ToString("F00", InvC) + " m";
        //                    R["_CoordinatesAverageLongitudeCache"] = f.Longitude;
        //                    R["_CoordinatesAverageLatitudeCache"] = f.Latitude;
        //                    string Notes = "";
        //                    if (!R["_CoordinatesLocationNotes"].Equals(System.DBNull.Value)) Notes = R["_CoordinatesLocationNotes"].ToString();
        //                    if (Notes.Length > 0)
        //                    {
        //                        if (Notes.IndexOf("Derived from Google Maps") == -1)
        //                            Notes += ". Derived from Google Maps";
        //                    }
        //                    else Notes = "Derived from Google Maps";
        //                    R["_CoordinatesLocationNotes"] = Notes;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //            }
        //        }

        //    }
        //    catch { }
        //}

        //private void GetRemoteValues(System.Windows.Forms.DataGridViewCell Cell)
        //{
        //    DiversityWorkbench.Forms.FormRemoteQuery f;
        //    try
        //    {
        //        string ValueColumn = this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName;
        //        string DisplayColumn = ValueColumn;
        //        bool IsListInDatabase = false;
        //        string ListInDatabase = "";
        //        DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit;
        //        System.Collections.Generic.List<DiversityWorkbench.UserControls.RemoteValueBinding> RemoteValueBindings = new List<DiversityWorkbench.UserControls.RemoteValueBinding>();
        //        switch (ValueColumn)
        //        {
        //            case "NamedAreaLocation2":
        //                if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityGazetteer")) return;
        //                ValueColumn = "NamedAreaLocation2";
        //                DisplayColumn = "Named_area";
        //                DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
        //                IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)G;

        //                DiversityWorkbench.UserControls.RemoteValueBinding RvbCountry = new DiversityWorkbench.UserControls.RemoteValueBinding();
        //                RvbCountry.BindingSource = this.firstLinesUnitBindingSource;
        //                RvbCountry.Column = "Country";
        //                RvbCountry.RemoteParameter = "Country";
        //                RemoteValueBindings.Add(RvbCountry);

        //                DiversityWorkbench.UserControls.RemoteValueBinding RvbLatitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
        //                RvbLatitude.BindingSource = this.firstLinesUnitBindingSource;
        //                RvbLatitude.Column = "_NamedAverageLatitudeCache";
        //                RvbLatitude.RemoteParameter = "Latitude";
        //                RemoteValueBindings.Add(RvbLatitude);

        //                DiversityWorkbench.UserControls.RemoteValueBinding RvbLongitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
        //                RvbLongitude.BindingSource = this.firstLinesUnitBindingSource;
        //                RvbLongitude.Column = "_NamedAverageLongitudeCache";
        //                RvbLongitude.RemoteParameter = "Longitude";
        //                RemoteValueBindings.Add(RvbLongitude);
        //                break;

        //            case "Link_to_SamplingPlots":
        //                if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversitySamplingPlots")) return;
        //                ValueColumn = "Link_to_SamplingPlots";
        //                DisplayColumn = "Sampling_plot";
        //                DiversityWorkbench.SamplingPlot S = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
        //                IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)S;

        //                DiversityWorkbench.UserControls.RemoteValueBinding RvbSamplingPlotLatitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
        //                RvbSamplingPlotLatitude.BindingSource = this.firstLinesUnitBindingSource;
        //                RvbSamplingPlotLatitude.Column = "Latitude_of_sampling_plot";
        //                RvbSamplingPlotLatitude.RemoteParameter = "Latitude";
        //                RemoteValueBindings.Add(RvbSamplingPlotLatitude);

        //                DiversityWorkbench.UserControls.RemoteValueBinding RvbSamplingPlotLongitude = new DiversityWorkbench.UserControls.RemoteValueBinding();
        //                RvbSamplingPlotLongitude.BindingSource = this.firstLinesUnitBindingSource;
        //                RvbSamplingPlotLongitude.Column = "Longitude_of_sampling_plot";
        //                RvbSamplingPlotLongitude.RemoteParameter = "Longitude";
        //                RemoteValueBindings.Add(RvbSamplingPlotLongitude);

        //                DiversityWorkbench.UserControls.RemoteValueBinding RvbSamplingPlotAccuracy = new DiversityWorkbench.UserControls.RemoteValueBinding();
        //                RvbSamplingPlotAccuracy.BindingSource = this.firstLinesUnitBindingSource;
        //                RvbSamplingPlotAccuracy.Column = "Accuracy_of_sampling_plot";
        //                RvbSamplingPlotAccuracy.RemoteParameter = "Accuracy";
        //                RemoteValueBindings.Add(RvbSamplingPlotAccuracy);
        //                break;

        //            case "Geographic_region":
        //                if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityScientificTerms")) return;
        //                ValueColumn = "_GeographicRegionPropertyURI";
        //                IsListInDatabase = true;
        //                ListInDatabase = "Geographic regions";
        //                DiversityWorkbench.ScientificTerm S_Geo = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
        //                IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)S_Geo;
        //                break;
        //            case "Lithostratigraphy":
        //                if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityScientificTerms")) return;
        //                ValueColumn = "_LithostratigraphyPropertyURI";
        //                IsListInDatabase = true;
        //                ListInDatabase = "Lithostratigraphy";
        //                DiversityWorkbench.ScientificTerm S_Litho = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
        //                IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)S_Litho;

        //                DiversityWorkbench.UserControls.RemoteValueBinding RvbHierarchyLitho = new DiversityWorkbench.UserControls.RemoteValueBinding();
        //                RvbHierarchyLitho.BindingSource = this.firstLinesUnitBindingSource;
        //                RvbHierarchyLitho.Column = "_LithostratigraphyPropertyHierarchyCache";
        //                RvbHierarchyLitho.RemoteParameter = "HierarchyCache";
        //                RemoteValueBindings.Add(RvbHierarchyLitho);
        //                break;

        //            case "Chronostratigraphy":
        //                if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityScientificTerms")) return;
        //                ValueColumn = "_ChronostratigraphyPropertyURI";
        //                IsListInDatabase = true;
        //                ListInDatabase = "Chronostratigraphy";
        //                DiversityWorkbench.ScientificTerm S_Chrono = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
        //                IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)S_Chrono;

        //                DiversityWorkbench.UserControls.RemoteValueBinding RvbHierarchyChrono = new DiversityWorkbench.UserControls.RemoteValueBinding();
        //                RvbHierarchyChrono.BindingSource = this.firstLinesUnitBindingSource;
        //                RvbHierarchyChrono.Column = "_ChronostratigraphyPropertyHierarchyCache";
        //                RvbHierarchyChrono.RemoteParameter = "HierarchyCache";
        //                RemoteValueBindings.Add(RvbHierarchyChrono);
        //                break;

        //            case "Link_to_DiversityTaxonNames":
        //                if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityTaxonNames")) return;
        //                ValueColumn = "Link_to_DiversityTaxonNames";
        //                DisplayColumn = "Taxonomic_name";
        //                DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
        //                IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)T;

        //                DiversityWorkbench.UserControls.RemoteValueBinding RvbFamily = new DiversityWorkbench.UserControls.RemoteValueBinding();
        //                RvbFamily.BindingSource = this.firstLinesUnitBindingSource;
        //                RvbFamily.Column = "Family_of_taxon";
        //                RvbFamily.RemoteParameter = "Family";
        //                RemoteValueBindings.Add(RvbFamily);

        //                DiversityWorkbench.UserControls.RemoteValueBinding RvbOrder = new DiversityWorkbench.UserControls.RemoteValueBinding();
        //                RvbOrder.BindingSource = this.firstLinesUnitBindingSource;
        //                RvbOrder.Column = "Order_of_taxon";
        //                RvbOrder.RemoteParameter = "Order";
        //                RemoteValueBindings.Add(RvbOrder);
        //                break;

        //            case "Link_to_DiversityTaxonNames_of_second_organism":
        //                if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityTaxonNames")) return;
        //                ValueColumn = "Link_to_DiversityTaxonNames_of_second_organism";
        //                DisplayColumn = "Taxonomic_name_of_second_organism";
        //                DiversityWorkbench.TaxonName Tsecond = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
        //                IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Tsecond;

        //                DiversityWorkbench.UserControls.RemoteValueBinding RvbSecondFamily = new DiversityWorkbench.UserControls.RemoteValueBinding();
        //                RvbSecondFamily.BindingSource = this.firstLinesUnitBindingSource;
        //                RvbSecondFamily.Column = "_SecondUnitFamilyCache";
        //                RvbSecondFamily.RemoteParameter = "Family";
        //                RemoteValueBindings.Add(RvbSecondFamily);

        //                DiversityWorkbench.UserControls.RemoteValueBinding RvbSecondOrder = new DiversityWorkbench.UserControls.RemoteValueBinding();
        //                RvbSecondOrder.BindingSource = this.firstLinesUnitBindingSource;
        //                RvbSecondOrder.Column = "Order_of_taxon";
        //                RvbSecondOrder.RemoteParameter = "_SecondUnitOrderCache";
        //                RemoteValueBindings.Add(RvbSecondOrder);
        //                break;

        //            case "Link_to_DiversityAgents":
        //                if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents")) return;
        //                ValueColumn = "Link_to_DiversityAgents";
        //                DisplayColumn = "Collectors_name";
        //                DiversityWorkbench.Agent Agent = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
        //                IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Agent;
        //                break;

        //            case "Depositors_link_to_DiversityAgents":
        //                if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents")) return;
        //                ValueColumn = "Depositors_link_to_DiversityAgents";
        //                DisplayColumn = "Depositors_name";
        //                DiversityWorkbench.Agent Depositor = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
        //                IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Depositor;
        //                break;

        //            case "Link_to_DiversityAgents_for_responsible":
        //                if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents")) return;
        //                ValueColumn = "Link_to_DiversityAgents_for_responsible";
        //                DisplayColumn = "ResponsibleName";
        //                DiversityWorkbench.Agent Identifier = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
        //                IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Identifier;
        //                break;

        //            case "Link_to_DiversityAgents_for_responsible_of_second_organism":
        //                if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents")) return;
        //                ValueColumn = "Link_to_DiversityAgents_for_responsible_of_second_organism";
        //                DisplayColumn = "Responsible_of_second_organism";
        //                DiversityWorkbench.Agent Identifier2 = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
        //                IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Identifier2;
        //                break;

        //            case "Link_to_DiversityExsiccatae":
        //                if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityExsiccatae")) return;
        //                ValueColumn = "Link_to_DiversityExsiccatae";
        //                DisplayColumn = "Exsiccata_abbreviation";
        //                DiversityWorkbench.Exsiccate Exs = new DiversityWorkbench.Exsiccate(DiversityWorkbench.Settings.ServerConnection);
        //                IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Exs;
        //                break;

        //            case "Link_to_DiversityReferences":
        //                if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityReferences")) return;
        //                ValueColumn = "Link_to_DiversityReferences";
        //                DisplayColumn = "Reference_title";
        //                DiversityWorkbench.Reference Ref = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
        //                IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Ref;
        //                break;

        //            case "Link_to_DiversityReferences_of_second_organism":
        //                if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityReferences")) return;
        //                ValueColumn = "Link_to_DiversityReferences_of_second_organism";
        //                DisplayColumn = "Reference_title_of_second_organism";
        //                DiversityWorkbench.Reference Ref2 = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
        //                IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Ref2;
        //                break;

        //            default:
        //                DiversityWorkbench.TaxonName Default = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
        //                IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Default;
        //                break;
        //        }

        //        if (this.firstLinesUnitBindingSource != null && IWorkbenchUnit != null)
        //        {
        //            System.Data.DataRowView RU = (System.Data.DataRowView)this.firstLinesUnitBindingSource.Current;
        //            string URI = "";
        //            if (RU != null)
        //                if (!RU[ValueColumn].Equals(System.DBNull.Value)) URI = RU[ValueColumn].ToString();
        //            if (URI.Length == 0)
        //            {
        //                if (IsListInDatabase)
        //                {
        //                    f = new DiversityWorkbench.Forms.FormRemoteQuery(IWorkbenchUnit, ListInDatabase);
        //                }
        //                else
        //                {
        //                    f = new DiversityWorkbench.Forms.FormRemoteQuery(IWorkbenchUnit);
        //                }
        //            }
        //            else
        //            {
        //                f = new DiversityWorkbench.Forms.FormRemoteQuery(URI, IWorkbenchUnit);
        //            }
        //            try { f.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace; }
        //            catch { }
        //            f.TopMost = true;
        //            f.ShowDialog();
        //            if (f.DialogResult == DialogResult.OK && f.DisplayText != null)
        //            {
        //                System.Data.DataRowView R = (System.Data.DataRowView)this.firstLinesUnitBindingSource.Current;
        //                R.BeginEdit();
        //                R[ValueColumn] = f.URI;
        //                R[DisplayColumn] = f.DisplayText;
        //                R.EndEdit();
        //                //this.labelURI.Text = f.URI;
        //                if (RemoteValueBindings != null && RemoteValueBindings.Count > 0)
        //                {
        //                    foreach (DiversityWorkbench.UserControls.RemoteValueBinding RVB in RemoteValueBindings)
        //                    {
        //                        foreach (System.Collections.Generic.KeyValuePair<string, string> P in IWorkbenchUnit.UnitValues())
        //                        {
        //                            if (RVB.RemoteParameter == P.Key)
        //                            {
        //                                System.Data.DataRowView RV = (System.Data.DataRowView)RVB.BindingSource.Current;
        //                                RV.BeginEdit();
        //                                RV[RVB.Column] = P.Value;
        //                                RV.EndEdit();
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //#region Removing links and Columns for removing links to external services

        //private void RemoveLink(System.Windows.Forms.DataGridViewCell Cell)
        //{
        //    string DisplayColumn = this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName;
        //    string ValueColumn = "";
        //    string Table = "";
        //    string LinkColumn = "";
        //    try
        //    {
        //        foreach (DiversityCollection.Forms.GridModeQueryField Q in this._GridModeQueryFields)
        //        {
        //            if (Q.AliasForColumn == DisplayColumn)
        //            {
        //                ValueColumn = Q.Column;
        //                Table = Q.AliasForTable;
        //                break;
        //            }
        //        }

        //        foreach (DiversityCollection.Forms.GridModeQueryField Q in this._GridModeQueryFields)
        //        {
        //            if (Q.AliasForTable == Table && Q.Column == ValueColumn)
        //            {
        //                LinkColumn = Q.AliasForColumn;
        //                break;
        //            }
        //        }

        //        if (this.firstLinesUnitBindingSource != null)
        //        {
        //            System.Data.DataRowView RU = (System.Data.DataRowView)this.firstLinesUnitBindingSource.Current;
        //            RU[LinkColumn] = System.DBNull.Value;
        //        }

        //    }
        //    catch { }
        //}

        ///// <summary>
        ///// Dictionary of columns that remove links of related modules etc.
        ///// </summary>
        //private System.Collections.Generic.Dictionary<string, string> RemoveColumns
        //{
        //    get
        //    {
        //        if (this._RemoveColumns == null)
        //        {
        //            this._RemoveColumns = new Dictionary<string, string>();
        //            this._RemoveColumns.Add("Remove_link_to_SamplingPlots", "");
        //            this._RemoveColumns.Add("Remove_link_to_gazetteer", "");
        //            this._RemoveColumns.Add("Remove_link_for_collector", "");
        //            this._RemoveColumns.Add("Remove_link_for_Depositor", "");
        //            this._RemoveColumns.Add("Remove_link_to_exsiccatae", "");
        //            this._RemoveColumns.Add("Remove_link_for_identification", "");
        //            this._RemoveColumns.Add("Remove_link_for_reference", "");
        //            this._RemoveColumns.Add("Remove_link_for_determiner", "");
        //            this._RemoveColumns.Add("Remove_link_for_second_organism", "");
        //            this._RemoveColumns.Add("Remove_link_for_reference_of_second_organism", "");
        //            this._RemoveColumns.Add("Remove_link_for_responsible_of_second_organism", "");
        //        }
        //        return this._RemoveColumns;
        //    }
        //}

        //private void setRemoveCellStyle()
        //{
        //    for (int i = 0; i < this.dataGridView.Rows.Count - 1; i++)
        //        this.setRemoveCellStyle(i);
        //}

        //private void setRemoveCellStyle(int RowIndex)
        //{
        //    if (this._StyleRemove == null)
        //    {
        //        this._StyleRemove = new DataGridViewCellStyle();
        //        this._StyleRemove.BackColor = System.Drawing.Color.Red;
        //        this._StyleRemove.SelectionBackColor = System.Drawing.Color.Red;
        //        this._StyleRemove.ForeColor = System.Drawing.Color.Red;
        //        this._StyleRemove.SelectionForeColor = System.Drawing.Color.Red;
        //        this._StyleRemove.Tag = "Remove";
        //    }
        //    try
        //    {
        //        foreach (System.Windows.Forms.DataGridViewCell Cell in this.dataGridView.Rows[RowIndex].Cells)
        //        {
        //            if (this.RemoveColumns.ContainsKey(this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName))
        //            {
        //                foreach (System.Windows.Forms.DataGridViewCell RemoveCell in this.dataGridView.Rows[RowIndex].Cells)
        //                {
        //                    if (this.dataGridView.Columns[RemoveCell.ColumnIndex].DataPropertyName ==
        //                        this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName)
        //                    {
        //                        RemoveCell.Style = this._StyleRemove;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch { }
        //}

        //#endregion

        //#endregion

        //#region Columns for new identifications

        ////private void insertNewIdentification(System.Windows.Forms.DataGridViewCell Cell)
        ////{
        ////    string DisplayColumn = this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName;
        ////    string Table = "SecondUnitIdentification";
        ////    if (this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName == "New_identification")
        ////        Table = "Identification";

        ////    System.Data.DataRowView RU = (System.Data.DataRowView)this.firstLinesUnitBindingSource.Current;
        ////    DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationRow R = this.dataSetCollectionSpecimen.Identification.NewIdentificationRow();
        ////    R.CollectionSpecimenID = int.Parse(RU["CollectionSpecimenID"].ToString());
        ////    if (Table == "Identification")
        ////    {
        ////        if (RU["_IdentificationUnitID"].Equals(System.DBNull.Value))
        ////            return;
        ////        R.IdentificationUnitID = int.Parse(RU["_IdentificationUnitID"].ToString());
        ////    }
        ////    else
        ////    {
        ////        if (RU["_SecondUnitID"].Equals(System.DBNull.Value))
        ////            return;
        ////        R.IdentificationUnitID = int.Parse(RU["_SecondUnitID"].ToString());
        ////    }
        ////    System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.Identification.Select("", "IdentificationSequence DESC");
        ////    if (rr.Length == 0)
        ////        return;
        ////    short Sequence = 0;
        ////    if (!short.TryParse(rr[0]["IdentificationSequence"].ToString(), out Sequence))
        ////        return;
        ////    Sequence++;
        ////    R.IdentificationSequence = Sequence;
        ////    R.TaxonomicName = "New identification";
        ////    this.dataSetCollectionSpecimen.Identification.Rows.Add(R);

        ////    try
        ////    {
        ////        foreach (DiversityCollection.Forms.GridModeQueryField Q in this._GridModeQueryFields)
        ////        {
        ////            if (Q.AliasForTable == Table)
        ////            {
        ////                if (this.firstLinesUnitBindingSource != null)
        ////                {
        ////                    foreach (System.Data.DataColumn C in R.Table.Columns)
        ////                    {
        ////                        foreach (DiversityCollection.Forms.GridModeQueryField QQ in this.GridModeQueryFields)
        ////                        {
        ////                            if (QQ.AliasForTable == R.Table.TableName
        ////                                && QQ.Column == C.ColumnName
        ////                                && !QQ.AliasForColumn.StartsWith("Remove"))
        ////                            {
        ////                                try { RU[QQ.AliasForColumn] = R[QQ.Column]; }
        ////                                catch { }
        ////                            }
        ////                            //foreach (System.Data.DataColumn Ccurrent in RU.Row.Table.Columns)
        ////                            //{
        ////                            //    if (C.ColumnName == Ccurrent.ColumnName) ;
        ////                            //}
        ////                        }
        ////                    }
        ////                    //System.Data.DataRowView RUx = (System.Data.DataRowView)this.firstLinesUnitBindingSource.Current;
        ////                    //string Tee = "";
        ////                    //if (this.dataGridView.Columns.Contains(Q.AliasForColumn))
        ////                    //{ string y = ""; }
        ////                    ////if (this.dataGridView.Columns[Q.AliasForColumn].ReadOnly == false)
        ////                    ////{
        ////                    //    try 
        ////                    //    { 
        ////                    //        RUx[Q.AliasForColumn] = System.DBNull.Value; 
        ////                    //    }
        ////                    //    catch { }
        ////                    ////}
        ////                }
        ////            }
        ////        }

        ////        //foreach (DiversityCollection.Forms.GridModeQueryField Q in this._GridModeQueryFields)
        ////        //{
        ////        //    //if (Q.AliasForTable == Table && Q.Column == ValueColumn)
        ////        //    //{
        ////        //    //    LinkColumn = Q.AliasForColumn;
        ////        //    //    break;
        ////        //    //}
        ////        //}


        ////    }
        ////    catch { }
        ////}

        /////// <summary>
        /////// Dictionary of columns that insert new identifications
        /////// </summary>
        ////private System.Collections.Generic.Dictionary<string, string> NewIdentificationColumns
        ////{
        ////    get
        ////    {
        ////        if (this._NewIdentificationColumns == null)
        ////        {
        ////            this._NewIdentificationColumns = new Dictionary<string, string>();
        ////            this._NewIdentificationColumns.Add("New_identification", "");
        ////            this._NewIdentificationColumns.Add("New_identification_of_second_organism", "");
        ////        }
        ////        return this._NewIdentificationColumns;
        ////    }
        ////}

        ////private void setNewIdentificationCellStyle()
        ////{
        ////    for (int i = 0; i < this.dataGridView.Rows.Count - 1; i++)
        ////        this.setNewIdentificationCellStyle(i);
        ////}

        ////private void setNewIdentificationCellStyle(int RowIndex)
        ////{
        ////    if (this._StyleNewIdentification == null)
        ////    {
        ////        this._StyleNewIdentification = new DataGridViewCellStyle();
        ////        this._StyleNewIdentification.BackColor = System.Drawing.Color.Green;
        ////        this._StyleNewIdentification.SelectionBackColor = System.Drawing.Color.Green;
        ////        this._StyleNewIdentification.ForeColor = System.Drawing.Color.Green;
        ////        this._StyleNewIdentification.SelectionForeColor = System.Drawing.Color.Green;
        ////        this._StyleNewIdentification.Tag = "New identification";
        ////    }
        ////    try
        ////    {
        ////        foreach (System.Windows.Forms.DataGridViewCell Cell in this.dataGridView.Rows[RowIndex].Cells)
        ////        {
        ////            if (this.NewIdentificationColumns.ContainsKey(this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName))
        ////            {
        ////                foreach (System.Windows.Forms.DataGridViewCell NewIdentificationCell in this.dataGridView.Rows[RowIndex].Cells)
        ////                {
        ////                    if (this.dataGridView.Columns[NewIdentificationCell.ColumnIndex].DataPropertyName ==
        ////                        this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName)
        ////                    {
        ////                        NewIdentificationCell.Style = this._StyleNewIdentification;
        ////                        break;
        ////                    }
        ////                }
        ////            }
        ////        }
        ////    }
        ////    catch { }
        ////}

        //#endregion

        //#region Blocking of Cells that are linked to external services

        //private void setCellBlockings()
        //{
        //    for (int i = 0; i < this.dataGridView.Rows.Count - 1; i++)
        //        this.setCellBlockings(i);
        //}

        //private void setCellBlockings(int RowIndex)
        //{
        //    if (this._StyleBlocked == null)
        //    {
        //        this._StyleBlocked = new DataGridViewCellStyle();
        //        this._StyleBlocked.BackColor = System.Drawing.Color.Yellow;
        //        this._StyleBlocked.SelectionBackColor = System.Drawing.Color.Yellow;
        //        this._StyleBlocked.ForeColor = System.Drawing.Color.Blue;
        //        this._StyleBlocked.SelectionForeColor = System.Drawing.Color.Blue;
        //        this._StyleBlocked.Tag = "Blocked";
        //    }
        //    if (this._StyleUnblocked == null)
        //    {
        //        this._StyleUnblocked = new DataGridViewCellStyle();
        //        this._StyleUnblocked.BackColor = System.Drawing.SystemColors.Window;
        //        this._StyleUnblocked.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        //        this._StyleUnblocked.ForeColor = System.Drawing.Color.Black;
        //        this._StyleUnblocked.SelectionForeColor = System.Drawing.SystemColors.Window;
        //        this._StyleUnblocked.Tag = "";
        //    }
        //    if (this._StyleReadOnly == null)
        //    {
        //        this._StyleReadOnly = new DataGridViewCellStyle();
        //        this._StyleReadOnly.BackColor = System.Drawing.Color.LightGray;
        //        this._StyleReadOnly.SelectionBackColor = System.Drawing.Color.LightGray;
        //        this._StyleReadOnly.ForeColor = System.Drawing.Color.Black;
        //        this._StyleReadOnly.SelectionForeColor = System.Drawing.Color.Black;
        //        this._StyleReadOnly.Tag = "ReadOnly";
        //    }
        //    try
        //    {
        //        foreach (System.Windows.Forms.DataGridViewCell Cell in this.dataGridView.Rows[RowIndex].Cells)
        //        {
        //            if (this.BlockedColumns.ContainsKey(this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName))
        //            {
        //                foreach (System.Windows.Forms.DataGridViewCell CellToBlock in this.dataGridView.Rows[RowIndex].Cells)
        //                {
        //                    if (this.dataGridView.Columns[CellToBlock.ColumnIndex].DataPropertyName ==
        //                        this.BlockedColumns[this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName])
        //                    {
        //                        if (Cell.EditedFormattedValue.ToString().Length > 0)
        //                        {
        //                            CellToBlock.Style = this._StyleBlocked;
        //                            CellToBlock.ReadOnly = true;
        //                        }
        //                        else
        //                        {
        //                            CellToBlock.Style = this._StyleUnblocked;
        //                            CellToBlock.ReadOnly = false;
        //                        }
        //                        break;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                foreach (System.Windows.Forms.DataGridViewCell CellToBlock in this.dataGridView.Rows[RowIndex].Cells)
        //                {
        //                    if (this.ReadOnlyColumns.Contains(this.dataGridView.Columns[CellToBlock.ColumnIndex].DataPropertyName))
        //                    {
        //                        CellToBlock.Style = this._StyleReadOnly;
        //                        CellToBlock.ReadOnly = true;
        //                    }
        //                }
        //            }
        //        }

        //        //System.Windows.Forms.DataGridViewCellStyle StyleReadOnly = new DataGridViewCellStyle();
        //        //StyleReadOnly.BackColor = System.Drawing.Color.LightGray;
        //        //StyleReadOnly.SelectionBackColor = System.Drawing.Color.LightGray;
        //        //StyleReadOnly.ForeColor = System.Drawing.Color.Gray;
        //        //StyleReadOnly.SelectionForeColor = System.Drawing.Color.Gray;
        //        //StyleReadOnly.Tag = "ReadOnly";

        //        //foreach (string R in this.ReadOnlyColumns)
        //        //{
        //        //    foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
        //        //    {
        //        //        if (C.DataPropertyName == R)
        //        //            C.HeaderCell.Style = StyleReadOnly;
        //        //    }
        //        //}
        //    }
        //    catch (System.Exception ex) { }
        //}

        //private System.Collections.Generic.Dictionary<string, string> BlockedColumns
        //{
        //    get
        //    {
        //        if (this._BlockedColumns == null)
        //        {
        //            this._BlockedColumns = new Dictionary<string, string>();
        //            this._BlockedColumns.Add("Link_to_DiversityAgents", "Collectors_name");
        //            this._BlockedColumns.Add("Depositors_link_to_DiversityAgents", "Depositors_name");
        //            this._BlockedColumns.Add("Link_to_DiversityExsiccatae", "Exsiccata_abbreviation");
        //            this._BlockedColumns.Add("Link_to_DiversityTaxonNames", "Taxonomic_name");
        //            this._BlockedColumns.Add("Link_to_DiversityReferences", "Reference_title");
        //            this._BlockedColumns.Add("Link_to_DiversityAgents_for_responsible", "Responsible");

        //            this._BlockedColumns.Add("Link_to_DiversityTaxonNames_of_second_organism", "Taxonomic_name_of_second_organism");
        //            this._BlockedColumns.Add("Link_to_DiversityReferences_of_second_organism", "Reference_title_of_second_organism");
        //            this._BlockedColumns.Add("Link_to_DiversityAgents_for_responsible_of_second_organism", "Responsible_of_second_organism");

        //            this._BlockedColumns.Add("NamedAreaLocation2", "Named_area");
        //            this._BlockedColumns.Add("Link_to_SamplingPlots", "Sampling_plot");
        //        }
        //        //foreach (DiversityCollection.Forms.GridModeQueryField Q in this._GridModeQueryFields)
        //        //{
        //        //    if (Q.Table != "IdentificationUnit" &&
        //        //        Q.Table != "Identification" &&
        //        //        Q.Table != "IdentificationUnitAnalysis")
        //        //        if (!this._BlockedColumns.ContainsKey(Q.AliasForColumn))
        //        //            this._BlockedColumns.Add(Q.AliasForColumn, Q.AliasForColumn);
        //        //}
        //        return this._BlockedColumns;
        //    }
        //}

        //private System.Collections.Generic.List<string> ReadOnlyColumns
        //{
        //    get
        //    {
        //        if (this._ReadOnlyColumns == null)
        //        {
        //            this._ReadOnlyColumns = new List<string>();
        //            foreach (DiversityCollection.Forms.GridModeQueryField Q in this._GridModeQueryFields)
        //            {
        //                if (Q.Table != "IdentificationUnit" &&
        //                    Q.Table != "Identification" &&
        //                    Q.Table != "IdentificationUnitAnalysis")
        //                    if (!this._ReadOnlyColumns.Contains(Q.AliasForColumn))
        //                        this._ReadOnlyColumns.Add(Q.AliasForColumn);
        //            }
        //            this._ReadOnlyColumns.Add("Related_organism");
        //            this._ReadOnlyColumns.Add("Analysis");
        //            this._ReadOnlyColumns.Add("AnalysisID");
        //        }
        //        return this._ReadOnlyColumns;
        //    }
        //}

        //#endregion

        //#region Copy

        ///// <summary>
        ///// create a copy of a collection specimen
        ///// </summary>
        ///// <param name="OriginalID">The CollectionSpecimenID of the original dataset</param>
        ///// <param name="AccessionNumber">The new AccessionNumber</param>
        ///// <param name="EventCopyMode">The mode for the copy of the collection event</param>
        ///// <param name="CopyUnits">If the identification units should be copied</param>
        ///// <returns></returns>
        //private int CopySpecimen(int OriginalID, string AccessionNumber, DiversityCollection.FormCopyDataset.EventCopyMode EventCopyMode, bool CopyUnits)
        //{
        //    string SQL = "execute dbo.procCopyCollectionSpecimen NULL , " + OriginalID.ToString() + ", '" + AccessionNumber + "'";
        //    switch (EventCopyMode)
        //    {
        //        case FormCopyDataset.EventCopyMode.NewEvent:
        //            SQL += ", 1";
        //            break;
        //        case FormCopyDataset.EventCopyMode.SameEvent:
        //            SQL += ", 0";
        //            break;
        //        case FormCopyDataset.EventCopyMode.NoEvent:
        //            SQL += ", -1";
        //            break;
        //    }
        //    if (CopyUnits) SQL += ", 1";
        //    else SQL += ", 0";
        //    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //    Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //    con.Open();
        //    int ID = 0;
        //    try
        //    {
        //        ID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    con.Close();
        //    return ID;
        //}

        //private int? NewIdentificationUnitID()
        //{
        //    int? UnitID = null;
        //    try
        //    {
        //        if (this.dataSetCollectionSpecimen.IdentificationUnit.Rows.Count > 0)
        //        {
        //            System.Data.DataRow[] RR = this.dataSetCollectionSpecimen.IdentificationUnit.Select("DisplayOrder > 0", "DisplayOrder ASC");
        //            if (RR.Length > 0)
        //            {
        //                int ID = 0;
        //                if (int.TryParse(RR[0]["IdentificationUnitID"].ToString(), out ID))
        //                    UnitID = ID;
        //            }
        //        }

        //    }
        //    catch { }
        //    return UnitID;
        //}

        //private int? NewEventID()
        //{
        //    int? EventID = null;
        //    try
        //    {
        //        if (this.dataSetCollectionSpecimen.CollectionEvent.Rows.Count > 0)
        //        {
        //            int ID = 0;
        //            if (int.TryParse(this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionEventID"].ToString(), out ID))
        //                EventID = ID;
        //        }

        //    }
        //    catch { }
        //    return EventID;
        //}

        //#endregion

        //#region Events for new Entries

        //private void insertNewDataset(int Index)
        //{
        //    if ((this.dataGridView.SelectedCells.Count > 0 &&
        //        this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count > 0 &&
        //        this.dataGridView.SelectedCells[0].RowIndex >= this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count)
        //        || this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count == 0)
        //    {
        //        string TaxonomicGroup = "";
        //        if (this.ProjectSettings.Count > 0)
        //        {
        //            if (this.ProjectSettings.ContainsKey("IdentificationUnit.TaxonomicGroup") &&
        //                this.ProjectSettings["IdentificationUnit.TaxonomicGroup"].Length > 0)
        //            {
        //                TaxonomicGroup = this.ProjectSettings["IdentificationUnit.TaxonomicGroup"];
        //            }

        //        }
        //        DiversityCollection.Forms.FormIdentificationUnitGridNewEntry f = new DiversityCollection.Forms.FormIdentificationUnitGridNewEntry(this.dataSetCollectionSpecimen, TaxonomicGroup);
        //        f.TopMost = true;
        //        f.ShowDialog();
        //        if (f.DialogResult == DialogResult.OK)
        //        {
        //            int ID = this._SpecimenID;
        //            if (f.IdentificationUnitCopyMode == DiversityCollection.Forms.FormIdentificationUnitGridNewEntry.UnitCopyMode.NewSpecimen)
        //            {
        //                try
        //                {
        //                    if (f.AccessionNumber.Length > 0)
        //                        ID = this.InsertNewSpecimen(f.AccessionNumber);
        //                    else
        //                        ID = this.InsertNewSpecimen(Index);
        //                    this._IDs.Add(ID);
        //                }
        //                catch (Exception ex)
        //                {
        //                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //                    return;
        //                }
        //            }
        //            try
        //            {
        //                int UnitID = this.InsertNewUnit(ID);
        //                System.Data.DataTable dt = new DataTable();
        //                string SQL = "SELECT " + this._SqlSpecimenFields +
        //                    " FROM dbo.FirstLinesUnit_2 ('" + ID.ToString() + "', null, null, null) WHERE IdentificationUnitID = " + UnitID.ToString();
        //                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //                ad.Fill(dt);
        //                DiversityCollection.Datasets.DataSetIdentifictionUnitGridMode.FirstLinesUnitRow Rnew = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.NewFirstLinesUnitRow();
        //                if (dt.Rows.Count > 0)
        //                {
        //                    foreach (System.Data.DataColumn C in Rnew.Table.Columns)
        //                    {
        //                        Rnew[C.ColumnName] = dt.Rows[0][C.ColumnName];
        //                    }
        //                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Add(Rnew);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //            }
        //        }
        //        //if (System.Windows.Forms.MessageBox.Show("Do you want to create a new dataset?", "New dataset?", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //        //{
        //        //    try
        //        //    {
        //        //        string AccessionNumber = "";
        //        //        if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count > 0)
        //        //        {
        //        //            System.Data.DataRow[] RR = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Select("Accession_number <> ''", "Accession_number DESC");
        //        //            if (RR.Length > 0)
        //        //                AccessionNumber = RR[0]["Accession_number"].ToString();
        //        //            else if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count - 1]["Accession_number"].Equals(System.DBNull.Value) &&
        //        //                this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count - 1]["Accession_number"].ToString().Length > 0)
        //        //                AccessionNumber = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count - 1]["Accession_number"].ToString();
        //        //        }
        //        //        DiversityCollection.FormAccessionNumber f = new FormAccessionNumber(AccessionNumber, true);
        //        //        f.ShowDialog();
        //        //        if (f.DialogResult == DialogResult.OK)
        //        //            AccessionNumber = f.AccessionNumber;
        //        //        if (AccessionNumber.Length == 0 &&
        //        //            System.Windows.Forms.MessageBox.Show("You did not give an accession number.\r\nDo you want to insert a dataset without an accession number?", "Insert without accession number", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //        //            return;
        //        //        int ID = this.InsertNewSpecimen(AccessionNumber);

        //        //        System.Data.DataTable dt = new DataTable();
        //        //        string SQL = "SELECT " + this._SqlSpecimenFields +
        //        //            " FROM dbo.FirstLines ('" + ID.ToString() + "')";
        //        //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //        //        ad.Fill(dt);
        //        //        DiversityCollection.Datasets.DataSetIdentifictionUnitGridMode.FirstLinesUnitRow Rnew = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.NewFirstLinesUnitRow();
        //        //        if (dt.Rows.Count > 0)
        //        //        {
        //        //            foreach (System.Data.DataColumn C in Rnew.Table.Columns)
        //        //            {
        //        //                Rnew[C.ColumnName] = dt.Rows[0][C.ColumnName];
        //        //            }
        //        //            this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Add(Rnew);
        //        //        }
        //        //    }
        //        //    catch (Exception ex)
        //        //    {
        //        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        //    }
        //        //}
        //    }
        //}

        ///// <summary>
        ///// inserting a new specimen into the table CollectionSpecimen
        ///// </summary>
        ///// <param name="Index">Index of the row in the grid view</param>
        ///// <returns>the CollectionSpecimenID of the new specimen</returns>
        //private int InsertNewSpecimen(int Index)
        //{
        //    try
        //    {
        //        int ID = -1;
        //        int ProjectID = -1;
        //        System.Data.DataTable dt = new DataTable();
        //        string SQL = "SELECT Project, ProjectID FROM ProjectList ORDER BY Project";
        //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //        ad.Fill(dt);
        //        DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Project", "ProjectID", "Select a project", "Please select a project from the list, where the new dataset should belong to");
        //        f.ShowDialog();
        //        if (f.DialogResult == DialogResult.OK)
        //        {
        //            if (!int.TryParse(f.SelectedValue.ToString(), out ProjectID))
        //                return ID;
        //            SQL = this.GridModeInsertCommandForNewData(Index, "CollectionSpecimen");
        //            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //            Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //            con.Open();
        //            ID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
        //            SQL = "INSERT INTO CollectionProject (CollectionSpecimenID, ProjectID) VALUES (" + ID.ToString() + ", " + ProjectID.ToString() + ")";
        //            cmd.CommandText = SQL;
        //            cmd.ExecuteNonQuery();
        //            con.Close();
        //        }
        //        return ID;
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        return -1;
        //    }
        //}

        //private int InsertNewSpecimen(string AccessionNumber)
        //{
        //    try
        //    {
        //        int ID = -1;
        //        int ProjectID = -1;
        //        System.Data.DataTable dt = new DataTable();
        //        string SQL = "SELECT Project, ProjectID FROM ProjectList ORDER BY Project";
        //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //        ad.Fill(dt);
        //        DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Project", "ProjectID", "Select a project", "Please select a project from the list, where the new dataset should belong to", this._ProjectID.ToString());
        //        f.ShowDialog();
        //        if (f.DialogResult == DialogResult.OK)
        //        {
        //            if (!int.TryParse(f.SelectedValue.ToString(), out ProjectID))
        //                return ID;
        //            SQL = "INSERT INTO CollectionSpecimen (Version, AccessionNumber) VALUES (1, '" + AccessionNumber + "');  (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])";
        //            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //            Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //            con.Open();
        //            ID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
        //            SQL = "INSERT INTO CollectionProject (CollectionSpecimenID, ProjectID) VALUES (" + ID.ToString() + ", " + ProjectID.ToString() + ")";
        //            cmd.CommandText = SQL;
        //            cmd.ExecuteNonQuery();
        //            //string TaxonomicGroup = "";
        //            //if (this.ProjectSettings.ContainsKey("IdentificationUnit.TaxonomicGroup") &&
        //            //    this.ProjectSettings["IdentificationUnit.TaxonomicGroup"].Length > 0)
        //            //{
        //            //    TaxonomicGroup = this.ProjectSettings["IdentificationUnit.TaxonomicGroup"];
        //            //}
        //            //else
        //            //    TaxonomicGroup = this.dataSetCollectionSpecimen.IdentificationUnit.Rows[0]["TaxonomicGroup"].ToString();
        //            //SQL = "INSERT INTO IdentificationUnit " +
        //            //    "(CollectionSpecimenID, LastIdentificationCache, TaxonomicGroup, DisplayOrder) " +
        //            //    "VALUES (" + ID.ToString() + ", '" + TaxonomicGroup
        //            //    + "', '" + TaxonomicGroup + "', 1)";
        //            //cmd.CommandText = SQL;
        //            //cmd.ExecuteNonQuery();
        //            if (this.ProjectSettings.Count > 0)
        //            {
        //                if ((this.ProjectSettings.ContainsKey("CollectionSpecimenPart.CollectionID")
        //                    && this.ProjectSettings["CollectionSpecimenPart.CollectionID"].Length > 0)
        //                    || (this.ProjectSettings.ContainsKey("CollectionSpecimenPart.MaterialCategory")
        //                    && this.ProjectSettings["CollectionSpecimenPart.MaterialCategory"].Length > 0))
        //                {
        //                    SQL = "INSERT INTO CollectionSpecimenPart " +
        //                        "(CollectionSpecimenID, SpecimenPartID, CollectionID, MaterialCategory) " +
        //                        "VALUES (" + ID.ToString() + ", 1, ";
        //                    if (this.ProjectSettings.ContainsKey("CollectionSpecimenPart.CollectionID")
        //                    && this.ProjectSettings["CollectionSpecimenPart.CollectionID"].Length > 0)
        //                        SQL += this.ProjectSettings["CollectionSpecimenPart.CollectionID"];
        //                    else
        //                        SQL += " NULL ";
        //                    SQL += ", ";
        //                    if (this.ProjectSettings.ContainsKey("CollectionSpecimenPart.MaterialCategory")
        //                    && this.ProjectSettings["CollectionSpecimenPart.MaterialCategory"].Length > 0)
        //                        SQL += " '" + this.ProjectSettings["CollectionSpecimenPart.MaterialCategory"] + "'";
        //                    else
        //                        SQL += " NULL ";
        //                    SQL += ")";
        //                    cmd.CommandText = SQL;
        //                    cmd.ExecuteNonQuery();
        //                }
        //            }
        //            con.Close();
        //        }
        //        return ID;
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        return -1;
        //    }
        //}

        //private int InsertNewUnit(int SpecimenID)
        //{
        //    try
        //    {
        //        int UnitID;
        //        string TaxonomicGroup = "";
        //        if (this.ProjectSettings.ContainsKey("IdentificationUnit.TaxonomicGroup") &&
        //            this.ProjectSettings["IdentificationUnit.TaxonomicGroup"].Length > 0)
        //            TaxonomicGroup = this.ProjectSettings["IdentificationUnit.TaxonomicGroup"];
        //        else
        //            TaxonomicGroup = this.dataSetCollectionSpecimen.IdentificationUnit.Rows[0]["TaxonomicGroup"].ToString();
        //        string SQL = "INSERT INTO IdentificationUnit " +
        //            "(CollectionSpecimenID, LastIdentificationCache, TaxonomicGroup, DisplayOrder) " +
        //            "VALUES (" + SpecimenID.ToString() + ", '" + TaxonomicGroup
        //            + "', '" + TaxonomicGroup + "', 1) (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])";
        //        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //        Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //        cmd.CommandText = SQL;
        //        con.Open();
        //        UnitID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
        //        con.Close();
        //        return UnitID;
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        return -1;
        //    }
        //}

        //private int createEvent(int Index)
        //{
        //    int EventID = -1;
        //    try
        //    {
        //        string SQL = this.GridModeInsertCommandForNewData(Index, "CollectionEvent");//"INSERT INTO CollectionEvent (LocalityDescription)VALUES ('" + LocalityDescription + "') (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])";
        //        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //        Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //        con.Open();
        //        EventID = System.Int32.Parse(cmd.ExecuteScalar().ToString());
        //        con.Close();
        //        con.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return EventID;
        //}

        //private int createIdentificationUnit(int Index, string AliasForTable)
        //{
        //    int UnitID = -1;
        //    try
        //    {
        //        string SQL = this.GridModeInsertCommandForNewData(Index, AliasForTable);
        //        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //        Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //        con.Open();
        //        UnitID = System.Int32.Parse(cmd.ExecuteScalar().ToString());
        //        con.Close();
        //        con.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return UnitID;
        //}

        ///// <summary>
        ///// Returns the SQL command for the new data
        ///// </summary>
        ///// <param name="Index">Index of the row in the grid view</param>
        ///// <param name="AliasForTable">The alias for the table</param>
        ///// <returns>SQL</returns>
        //private string GridModeInsertCommandForNewData(int Index, string AliasForTable)
        //{
        //    string SQL = "";
        //    string SqlColumns = "INSERT INTO " + this.GridModeTableName(AliasForTable) + " (";
        //    string SqlValues = "VALUES ( ";
        //    if (this.GridModeTableName(AliasForTable) == "CollectionSpecimen" ||
        //        this.GridModeTableName(AliasForTable) == "CollectionEvent")
        //    {
        //        SqlColumns += " Version, ";
        //        SqlValues += " 1, ";
        //    }
        //    else if (this.GridModeTableName(AliasForTable) == "IdentificationUnit")
        //    {
        //        string TaxonomicGroup = "";
        //        int DisplayOrder = 1;
        //        string LastIdentification = "";
        //        SqlColumns += " CollectionSpecimenID, ";
        //        SqlValues += this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["CollectionSpecimenID"].ToString() + ", ";
        //        switch (AliasForTable)
        //        {
        //            case "IdentificationUnit":
        //                TaxonomicGroup = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_group"].ToString();
        //                if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name"].Equals(System.DBNull.Value) ||
        //                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name"].ToString().Length == 0)
        //                    LastIdentification = TaxonomicGroup;
        //                else
        //                    LastIdentification = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name"].ToString();
        //                DisplayOrder = 1;
        //                break;
        //            case "SecondUnit":
        //                TaxonomicGroup = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_group_of_second_organism"].ToString();
        //                if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name_of_second_organism"].Equals(System.DBNull.Value) ||
        //                    this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name_of_second_organism"].ToString().Length == 0)
        //                    LastIdentification = TaxonomicGroup;
        //                else
        //                    LastIdentification = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index]["Taxonomic_name_of_second_organism"].ToString();
        //                DisplayOrder = 2;
        //                break;
        //        }
        //        SqlColumns += " LastIdentificationCache, ";
        //        SqlValues += " '" + LastIdentification + "', ";
        //        SqlColumns += " DisplayOrder, ";
        //        SqlValues += " " + DisplayOrder.ToString() + ", ";
        //    }
        //    if (this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows.Count > Index)
        //    {
        //        System.Data.DataRow R = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index];
        //        foreach (System.Data.DataColumn C in this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Columns)
        //        {
        //            DiversityCollection.Forms.GridModeQueryField GMQF = this.GridModeGetQueryField(C.ColumnName);
        //            if (GMQF.AliasForTable == AliasForTable &&
        //                !this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index][GMQF.AliasForColumn].Equals(System.DBNull.Value))
        //            {
        //                SqlColumns += GMQF.Column + ", ";
        //                SqlValues += "'" + this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[Index][GMQF.AliasForColumn].ToString() + "', ";
        //            }
        //        }
        //    }
        //    if (SqlValues.EndsWith(", ")) SqlValues = SqlValues.Substring(0, SqlValues.Length - 2);
        //    if (SqlColumns.EndsWith(", ")) SqlColumns = SqlColumns.Substring(0, SqlColumns.Length - 2);
        //    SqlValues += ") ";
        //    SqlColumns += ") ";
        //    SQL = SqlColumns + SqlValues + " (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])";
        //    return SQL;
        //}

        //#endregion

        #endregion

        //#region Tree

        //private void initTree()
        //{
        //    this.treeView.ImageList = DiversityCollection.Specimen.ImageList;
        //    this.treeView.Visible = false;
        //    this.treeView.Nodes.Clear();
        //    try
        //    {
        //        try
        //        {
        //            if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 0)
        //            {
        //                this.addEventSeries();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        }
        //        this.treeView.ExpandAll();
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    finally
        //    {
        //        this.treeView.Visible = true;
        //    }
        //}

        //private System.Windows.Forms.TreeNode addEventSeriesSuperiorList()
        //{
        //    System.Windows.Forms.TreeNode ParentNode = new TreeNode();
        //    try
        //    {
        //        if (this.NoLoopInEventSeries())
        //        {
        //            foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
        //            {
        //                if (r["SeriesParentID"].Equals(System.DBNull.Value)/* || r["SeriesParentID"].ToString() == r["SeriesID"].ToString()*/)
        //                {
        //                    DiversityCollection.HierarchyNode N = new HierarchyNode(this.CollectionEventSeriesDataRowFromEventDataset(r), false);
        //                    ParentNode = N;
        //                    this.treeView.Nodes.Add(N);
        //                    this.getEventSeriesSuperiorChilds(N, ref ParentNode);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            string SeriesID = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[0]["SeriesID"].ToString();
        //            System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesID = " + SeriesID);
        //            ParentNode = new HierarchyNode(this.CollectionEventSeriesDataRowFromEventDataset(rr[0]), false);
        //            System.Windows.Forms.MessageBox.Show("The event series contains a loop. Please set the series for the collection event");
        //            this.treeView.Nodes.Add(ParentNode);
        //        }
        //    }
        //    catch { }

        //    return ParentNode;
        //}

        //private bool NoLoopInEventSeries()
        //{
        //    bool NoLoop = true;
        //    System.Data.DataRow RParent;
        //    System.Data.DataRow RChild;
        //    if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 0)
        //    {
        //        System.Data.DataRow[] RR = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesParentID IS NULL");
        //        if (RR.Length > 0)
        //        {
        //            RParent = RR[0];
        //            return true;
        //        }
        //        else
        //        {
        //            RParent = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[0];
        //        }
        //        if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 1)
        //        {
        //            if (RR.Length > 0)
        //            {
        //                System.Data.DataRow[] RRChild = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("NOT SeriesParentID IS NULL");
        //                if (RRChild.Length > 0)
        //                    RChild = RRChild[0];
        //                else
        //                    RChild = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[0];
        //            }
        //            else
        //                RChild = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[1];
        //        }
        //        else
        //            RChild = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[0];
        //        if (RChild != null && RParent != null)
        //            NoLoop = this.NoLoopInEventSeries(RChild, RParent);
        //    }
        //    return NoLoop;
        //}

        ////private bool NoLoopInEventSeries(System.Data.DataRow rChild, System.Data.DataRow rParent)
        ////{
        ////    bool NoLoop = true;
        ////    try
        ////    {
        ////        int ChildID = int.Parse(rChild["SeriesID"].ToString());
        ////        int ParentID = int.Parse(rParent["SeriesID"].ToString());
        ////        if (ChildID == ParentID)
        ////            return false;
        ////        int? ParentOfParentID = null;
        ////        int iPP = 0;
        ////        if (int.TryParse(rParent["SeriesParentID"].ToString(), out iPP))
        ////            ParentOfParentID = iPP;
        ////        System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesID = " + ParentID);
        ////        if (rr.Length > 0)
        ////        {
        ////            if (!rr[0]["SeriesParentID"].Equals(System.DBNull.Value))
        ////            {
        ////                while (ParentOfParentID != null)
        ////                {
        ////                    if (ParentOfParentID == ChildID)
        ////                    {
        ////                        NoLoop = false;
        ////                        break;
        ////                    }
        ////                    System.Data.DataRow[] RR = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesID = " + ParentOfParentID);
        ////                    if (RR.Length > 0)
        ////                    {
        ////                        if (RR[0]["SeriesParentID"].Equals(System.DBNull.Value))
        ////                            break;
        ////                        else
        ////                        {
        ////                            ParentOfParentID = int.Parse(RR[0]["SeriesParentID"].ToString());
        ////                        }
        ////                    }
        ////                    else break;
        ////                }
        ////            }
        ////        }
        ////    }
        ////    catch { }
        ////    return NoLoop;
        ////}

        //private System.Data.DataRow CollectionEventSeriesDataRowFromEventDataset(System.Data.DataRow DataRowFromSpecimenDataset)
        //{
        //    System.Data.DataRow Rseries = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[0];
        //    foreach (System.Data.DataRow R in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
        //    {
        //        if (R["SeriesID"].ToString() == DataRowFromSpecimenDataset["SeriesID"].ToString())
        //        {
        //            Rseries = R;
        //            break;
        //        }
        //    }
        //    return Rseries;
        //}

        //private void getEventSeriesSuperiorChilds(System.Windows.Forms.TreeNode Node, ref System.Windows.Forms.TreeNode ParentNode)
        //{
        //    try
        //    {
        //        System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
        //        string SeriesParentID = rParent["SeriesID"].ToString();
        //        System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesParentID = " + rParent["SeriesID"].ToString(), "DateStart");
        //        foreach (System.Data.DataRow rO in rr)
        //        {
        //            foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
        //            {
        //                if (rO["SeriesID"].ToString() == r["SeriesID"].ToString())
        //                {
        //                    DiversityCollection.HierarchyNode N = new HierarchyNode(this.CollectionEventSeriesDataRowFromEventDataset(r), false);
        //                    Node.Nodes.Add(N);
        //                    this.getEventSeriesSuperiorChilds(N, ref ParentNode);
        //                    ParentNode = N;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void addEventSeries()
        //{
        //    foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
        //    {
        //        if (r["SeriesParentID"].Equals(System.DBNull.Value))
        //        {
        //            DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
        //            this.treeView.Nodes.Add(N);
        //            this.getEventSeriesChilds(N);
        //            this.getEventSeriesEvents(N);
        //        }
        //    }
        //    this.addHierarchyUnits();
        //    // if a mistake occurs, i.e. a loop the system will take one eventseries and add the rest of the tree
        //    //if (this.treeView.Nodes.Count == 0)
        //    //{
        //    //    this.addEventSeriesSuperiorList();
        //    //    System.Windows.Forms.TreeNode EventNode = this.OverviewHierarchyEventNode;
        //    //    this.treeView.Nodes[0].Nodes.Add(EventNode);
        //    //    System.Windows.Forms.TreeNode SpecimenNode = this.OverviewHierarchySpecimenNode;
        //    //    EventNode.Nodes.Add(SpecimenNode);
        //    //}
        //    //this.replaceEventListNode();
        //}

        //private void getEventSeriesChilds(System.Windows.Forms.TreeNode Node)
        //{
        //    try
        //    {
        //        System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
        //        string SeriesParentID = rParent["SeriesID"].ToString();
        //        System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesParentID = " + rParent["SeriesID"].ToString(), "DateStart");
        //        foreach (System.Data.DataRow rO in rr)
        //        {
        //            foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventSeries.Rows)
        //            {
        //                if (rO["SeriesID"].ToString() == r["SeriesID"].ToString())
        //                {
        //                    DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
        //                    Node.Nodes.Add(N);
        //                    this.getEventSeriesChilds(N);
        //                    this.getEventSeriesEvents(N);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void getEventSeriesEvents(System.Windows.Forms.TreeNode Node)
        //{
        //    try
        //    {
        //        if (Node != null)
        //        {
        //            System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
        //            string SeriesID = rParent["SeriesID"].ToString();
        //            foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionEventList.Rows)
        //            {
        //                if (r["SeriesID"].ToString() == SeriesID)
        //                {
        //                    DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
        //                    Node.Nodes.Add(N);
        //                    this.getEventSeriesEventSpecimen(N);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            System.Data.DataRow rEvent = this.dataSetCollectionEventSeries.CollectionEventList.Rows[0];
        //            DiversityCollection.HierarchyNode N = new HierarchyNode(rEvent, false);
        //            this.treeView.Nodes.Add(N);
        //            this.getEventSeriesEventSpecimen(N);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void getEventSeriesEventSpecimen(System.Windows.Forms.TreeNode Node)
        //{
        //    try
        //    {
        //        System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
        //        string CollectionEventID = rParent["CollectionEventID"].ToString();
        //        foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionSpecimenList.Rows)
        //        {
        //            if (r["CollectionEventID"].ToString() == CollectionEventID)
        //            {
        //                DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
        //                Node.Nodes.Add(N);
        //            }
        //        }
        //        //this.addHierarchyUnits();
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void getEventSeriesEventSpecimen(System.Windows.Forms.TreeNode Node, int MainSpecimenID)
        //{
        //    try
        //    {
        //        System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
        //        string CollectionEventID = rParent["CollectionEventID"].ToString();
        //        foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.CollectionSpecimenList.Rows)
        //        {
        //            if (r["CollectionEventID"].ToString() == CollectionEventID && r["CollectionSpecimenID"].ToString() != MainSpecimenID.ToString())
        //            {
        //                DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
        //                Node.Nodes.Add(N);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void getEventSeriesEventSpecimenUnits(System.Windows.Forms.TreeNode Node)
        //{
        //    if (!this.ShowUnit) return;
        //    try
        //    {
        //        if (Node.Nodes.Count > 0)
        //        {
        //            foreach (System.Windows.Forms.TreeNode NChild in Node.Nodes)
        //            {
        //                System.Data.DataRow RChildNode = (System.Data.DataRow)NChild.Tag;
        //                if (RChildNode.Table.TableName == "IdentificationUnitList")
        //                {
        //                    this.getEventSeriesEventSpecimenUnitChilds(NChild);
        //                    return;
        //                }
        //            }
        //        }

        //        System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
        //        string CollectionSpecimenID = rParent["CollectionSpecimenID"].ToString();
        //        foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.IdentificationUnitList.Rows)
        //        {
        //            if (r["CollectionSpecimenID"].ToString() == CollectionSpecimenID
        //                && r["RelatedUnitID"].Equals(System.DBNull.Value))
        //            {
        //                DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
        //                Node.Nodes.Add(N);
        //                this.getEventSeriesEventSpecimenUnitChilds(N);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void getEventSeriesEventSpecimenUnitChilds(System.Windows.Forms.TreeNode Node)
        //{
        //    if (!this.ShowUnit) return;
        //    try
        //    {
        //        System.Data.DataRow rParent = (System.Data.DataRow)Node.Tag;
        //        string CollectionSpecimenID = rParent["CollectionSpecimenID"].ToString();
        //        string SubstrateID = rParent["IdentificationUnitID"].ToString();
        //        foreach (System.Data.DataRow r in this.dataSetCollectionEventSeries.IdentificationUnitList.Rows)
        //        {
        //            if (r["CollectionSpecimenID"].ToString() == CollectionSpecimenID && r["RelatedUnitID"].ToString() == SubstrateID && SubstrateID != null)
        //            {
        //                DiversityCollection.HierarchyNode N = new HierarchyNode(r, false);
        //                Node.Nodes.Add(N);
        //                this.getEventSeriesEventSpecimenUnitChilds(N);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private bool ShowUnit
        //{
        //    get
        //    {
        //        if (this.toolStripButtonShowUnit.Tag == null) return true;
        //        else
        //        {
        //            if (this.toolStripButtonShowUnit.Tag.ToString() == "Show") return true;
        //            else return false;
        //        }
        //    }
        //}



        ////private void replaceEventListNode()
        ////{
        ////    System.Collections.Generic.List<System.Windows.Forms.TreeNode> EventNodes = new List<TreeNode>();
        ////    this.getHierarchyNodes(null, "CollectionEventList", this.treeView, ref EventNodes);
        ////    this.treeView.CollapseAll();
        ////    foreach (System.Windows.Forms.TreeNode N in EventNodes)
        ////    {
        ////        System.Data.DataRow R = (System.Data.DataRow)N.Tag;
        ////        if (R["CollectionEventID"].ToString() == this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].ToString())
        ////        {
        ////            System.Windows.Forms.TreeNode ParentNode = N.Parent;
        ////            N.Remove();
        ////            System.Windows.Forms.TreeNode EventNode = this.OverviewHierarchyEventNode;
        ////            ParentNode.Nodes.Add(EventNode);
        ////            System.Windows.Forms.TreeNode SpecimenNode = this.OverviewHierarchySpecimenNode;
        ////            int CollectionSpecimenID = int.Parse(this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionSpecimenID"].ToString());
        ////            this.getEventSeriesEventSpecimen(EventNode, CollectionSpecimenID);
        ////            EventNode.Nodes.Add(SpecimenNode);
        ////            SpecimenNode.Expand();
        ////            this.treeView.SelectedNode = SpecimenNode;
        ////            this.treeView.SelectedNode = null;
        ////            EventNode.ExpandAll();
        ////            //ParentNode.ExpandAll();
        ////            return;
        ////        }
        ////    }
        ////}

        ////private void addOverviewHierarchyEventSeriesHierarchy()
        ////{
        ////    if (this.ShowEventSeries)
        ////    {
        ////        //System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
        ////        //this.getOverviewHierarchyNodes(null, "CollectionEvent", this.treeView, ref Nodes);
        ////        //foreach (System.Data.DataRow R in this.dataSetCollectionSpecimen.CollectionEventProperty.Rows)
        ////        //{
        ////        //    DiversityCollection.HierarchyNode NA = new HierarchyNode(R);
        ////        //    NA.ImageIndex = (int)DiversityCollection.Specimen.OverviewImage.EventProperty;
        ////        //    NA.SelectedImageIndex = (int)DiversityCollection.Specimen.OverviewImage.EventProperty;
        ////        //    NA.ForeColor = System.Drawing.Color.Green;
        ////        //    Nodes[0].Nodes.Add(NA);
        ////        //}
        ////    }
        ////}

        ////private void addOverviewHierarchyEventSeries()
        ////{
        ////    if (this.ShowEventSeries)
        ////    {
        ////        //System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
        ////        //this.getOverviewHierarchyNodes(null, "CollectionEvent", this.treeView, ref Nodes);
        ////        //foreach (System.Data.DataRow R in this.dataSetCollectionSpecimen.CollectionEventProperty.Rows)
        ////        //{
        ////        //    DiversityCollection.HierarchyNode NA = new HierarchyNode(R);
        ////        //    NA.ImageIndex = (int)DiversityCollection.Specimen.OverviewImage.EventProperty;
        ////        //    NA.SelectedImageIndex = (int)DiversityCollection.Specimen.OverviewImage.EventProperty;
        ////        //    NA.ForeColor = System.Drawing.Color.Green;
        ////        //    Nodes[0].Nodes.Add(NA);
        ////        //}
        ////    }
        ////}

        //#region Hiding
        ////private void hideOverviewHierarchyEventSeriesHierarchy()
        ////{
        ////    this.hideOverviewHierarchyNodes("CollectionEventSeries");
        ////    this.hideOverviewHierarchyNodes("CollectionEventSeries");
        ////    this.hideOverviewHierarchyNodes("CollectionEventSeries");
        ////    this.hideOverviewHierarchyNodes("CollectionEventSeries");
        ////}

        ////private void hideOverviewHierarchyEventSeries()
        ////{
        ////    this.hideOverviewHierarchyNodes("CollectionEventSeries");
        ////}

        //#endregion

        //#region Drag & Drop

        //#region Unit tree

        //private void treeView_DragDrop(object sender, DragEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Data.GetDataPresent("DiversityCollection.HierarchyNode", false))
        //        {
        //            Point pt = this.treeView.PointToClient(new Point(e.X, e.Y));
        //            TreeNode ParentNode = this.treeView.GetNodeAt(pt);
        //            TreeNode ChildNode = (TreeNode)e.Data.GetData("DiversityCollection.HierarchyNode");
        //            System.Data.DataRow rChild = (System.Data.DataRow)ChildNode.Tag;
        //            System.Data.DataRow rParent = (System.Data.DataRow)ParentNode.Tag;
        //            string ChildTable = rChild.Table.TableName;
        //            string ParentTable = rParent.Table.TableName;
        //            string SQL = "";
        //            Microsoft.Data.SqlClient.SqlConnection c = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //            Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, c);
        //            switch (ChildTable)
        //            {
        //                case "CollectionEventSeries":
        //                    if (ParentTable == "CollectionEventSeries")
        //                    {
        //                        if (this.NoLoopInEventSeries(rChild, rParent))
        //                        {
        //                            rChild["SeriesParentID"] = rParent["SeriesID"];
        //                            this.initTree();
        //                        }
        //                        else
        //                            System.Windows.Forms.MessageBox.Show("This would create a loop in the event series", "Loop", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    }
        //                    else
        //                    {
        //                        System.Windows.Forms.MessageBox.Show("Event series can only be placed within other event series");
        //                        return;
        //                    }
        //                    break;
        //                case "CollectionSpecimenList":
        //                    if (ParentTable == "CollectionEvent")
        //                    {
        //                        SQL = "UPDATE CollectionSpecimen SET CollectionEventID  = " + rParent["CollectionEventID"].ToString() + " WHERE CollectionSpecimenID = " + rChild["CollectionSpecimenID"].ToString();
        //                        cmd.CommandText = SQL;
        //                        c.Open();
        //                        cmd.ExecuteNonQuery();
        //                        c.Close();
        //                        //this.setSpecimen(this.ID);
        //                    }
        //                    else
        //                    {
        //                        System.Windows.Forms.MessageBox.Show("Specimens can only be placed within collection events");
        //                        return;
        //                    }
        //                    break;
        //                case "CollectionEventList":
        //                    if (ParentTable == "CollectionEventSeries")
        //                    {
        //                        rChild["SeriesID"] = rParent["SeriesID"];
        //                        SQL = "UPDATE CollectionEvent SET SeriesID  = " + rParent["SeriesID"].ToString() + " WHERE CollectionEventID = " + rChild["CollectionEventID"].ToString();
        //                        cmd.CommandText = SQL;
        //                        c.Open();
        //                        cmd.ExecuteNonQuery();
        //                        c.Close();
        //                        //this.setSpecimen(this.ID);
        //                    }
        //                    else
        //                    {
        //                        System.Windows.Forms.MessageBox.Show("Collection events can only be placed within event series");
        //                        //this.buildEventSeriesHierarchy();
        //                        //this.setSpecimen(this.ID);
        //                        return;
        //                    }
        //                    break;
        //                //case "CollectionEventSeries":
        //                //    if (ParentTable == "CollectionEventSeries")
        //                //    {
        //                //        SQL = "UPDATE CollectionEventSeries SET EventSeriesParentID  = " + rParent["SeriesID"].ToString() + " WHERE EventSeriesID = " + rChild["SeriesID"].ToString();
        //                //        cmd.CommandText = SQL;
        //                //        c.Open();
        //                //        cmd.ExecuteNonQuery();
        //                //        c.Close();
        //                //        //this.buildEventSeriesHierarchy();
        //                //        this.setSpecimen(this.ID);
        //                //    }
        //                //    else
        //                //    {
        //                //        System.Windows.Forms.MessageBox.Show("EventSeriess / collection event groups can only be placed within EventSeriess / collection event groups");
        //                //        //this.buildEventSeriesHierarchy();
        //                //        return;
        //                //    }
        //                //    break;
        //                case "IdentificationUnit":
        //                    int ChildID;
        //                    int oldChildSubstrateID;
        //                    int ParentID;
        //                    //try
        //                    //{
        //                    //    if (e.Data.GetDataPresent("DiversityCollection.HierarchyNode", false))
        //                    //    {
        //                    //        pt = this.treeViewUnitHierarchy.PointToClient(new Point(e.X, e.Y));
        //                    //        ParentNode = this.treeViewUnitHierarchy.GetNodeAt(pt);
        //                    //        ChildNode = (TreeNode)e.Data.GetData("DiversityCollection.HierarchyNode");
        //                    if (ParentTable == "IdentificationUnitList")
        //                    {
        //                        if (!ParentNode.Equals(ChildNode))
        //                        {
        //                            rChild = (System.Data.DataRow)ChildNode.Tag;
        //                            ChildID = System.Int32.Parse(rChild["IdentificationUnitID"].ToString());
        //                            if (rChild["RelatedUnitID"].Equals(System.DBNull.Value)) oldChildSubstrateID = -1;
        //                            else oldChildSubstrateID = System.Int32.Parse(rChild["RelatedUnitID"].ToString());
        //                            if (ParentNode.Tag != null)
        //                            {
        //                                rParent = (System.Data.DataRow)ParentNode.Tag;
        //                                ParentID = System.Int32.Parse(rParent["IdentificationUnitID"].ToString());
        //                                rChild["RelatedUnitID"] = ParentID;
        //                            }
        //                            else rChild["RelatedUnitID"] = System.DBNull.Value;
        //                            System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.Tables["IdentificationUnit"].Select("RelatedUnitID = " + ChildID.ToString());
        //                            foreach (System.Data.DataRow r in rr)
        //                            {
        //                                if (oldChildSubstrateID > -1) r["RelatedUnitID"] = oldChildSubstrateID;
        //                                else r["RelatedUnitID"] = System.DBNull.Value;
        //                            }
        //                            //this.buildUnitHierarchy();
        //                        }
        //                    }
        //                    else if (ParentTable == "CollectionSpecimen")
        //                    {
        //                        rChild = (System.Data.DataRow)ChildNode.Tag;
        //                        rChild["RelatedUnitID"] = System.DBNull.Value;
        //                    }
        //                    //    }
        //                    //}
        //                    //catch (Exception ex)
        //                    //{
        //                    //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //                    //}
        //                    break;
        //                default:
        //                    System.Windows.Forms.MessageBox.Show("Only event series, collection events, collection speciman and identification units can moved here");
        //                    //this.buildEventSeriesHierarchy();
        //                    //return;
        //                    break;
        //            }
        //            this.fillDataSet();
        //            this.initTree();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}


        ////private bool NoLoopInEventSeries()
        ////{
        ////    bool NoLoop = true;
        ////    System.Data.DataRow RParent;
        ////    System.Data.DataRow RChild;
        ////    if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 0)
        ////    {
        ////        System.Data.DataRow[] RR = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesParentID IS NULL");
        ////        if (RR.Length > 0)
        ////        {
        ////            RParent = RR[0];
        ////            return true;
        ////        }
        ////        else
        ////        {
        ////            RParent = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[0];
        ////        }
        ////        if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count > 1)
        ////        {
        ////            if (RR.Length > 0)
        ////            {
        ////                System.Data.DataRow[] RRChild = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("NOT SeriesParentID IS NULL");
        ////                if (RRChild.Length > 0)
        ////                    RChild = RRChild[0];
        ////                else
        ////                    RChild = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[0];
        ////            }
        ////            else
        ////                RChild = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[1];
        ////        }
        ////        else
        ////            RChild = this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[0];
        ////        if (RChild != null && RParent != null)
        ////            NoLoop = this.NoLoopInEventSeries(RChild, RParent);
        ////    }
        ////    return NoLoop;
        ////}


        //private bool NoLoopInEventSeries(System.Data.DataRow rChild, System.Data.DataRow rParent)
        //{
        //    bool NoLoop = true;
        //    try
        //    {
        //        int ChildID = int.Parse(rChild["SeriesID"].ToString());
        //        int ParentID = int.Parse(rParent["SeriesID"].ToString());
        //        if (ChildID == ParentID)
        //            return false;
        //        int? ParentOfParentID = null;
        //        int iPP = 0;
        //        if (int.TryParse(rParent["SeriesParentID"].ToString(), out iPP))
        //            ParentOfParentID = iPP;
        //        System.Data.DataRow[] rr = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesID = " + ParentID);
        //        if (rr.Length > 0)
        //        {
        //            if (!rr[0]["SeriesParentID"].Equals(System.DBNull.Value))
        //            {
        //                while (ParentOfParentID != null)
        //                {
        //                    if (ParentOfParentID == ChildID)
        //                    {
        //                        NoLoop = false;
        //                        break;
        //                    }
        //                    System.Data.DataRow[] RR = this.dataSetCollectionEventSeries.CollectionEventSeries.Select("SeriesID = " + ParentOfParentID);
        //                    if (RR.Length > 0)
        //                    {
        //                        if (RR[0]["SeriesParentID"].Equals(System.DBNull.Value))
        //                            break;
        //                        else
        //                        {
        //                            ParentOfParentID = int.Parse(RR[0]["SeriesParentID"].ToString());
        //                        }
        //                    }
        //                    else break;
        //                }
        //            }
        //        }
        //    }
        //    catch { }
        //    return NoLoop;
        //}

        //private void treeView_DragOver(object sender, DragEventArgs e)
        //{
        //    try
        //    {
        //        System.Windows.Forms.TreeNode tn;
        //        e.Effect = DragDropEffects.Move;
        //        TreeView tv = sender as TreeView;
        //        Point pt = tv.PointToClient(new Point(e.X, e.Y));
        //        int delta = tv.Height - pt.Y;
        //        if ((delta < tv.Height / 2) && (delta > 0))
        //        {
        //            tn = tv.GetNodeAt(pt.X, pt.Y);
        //            if (tn != null)
        //            {
        //                if (tn.NextVisibleNode != null)
        //                    tn.NextVisibleNode.EnsureVisible();
        //            }
        //        }
        //        if ((delta > tv.Height / 2) && (delta < tv.Height))
        //        {
        //            tn = tv.GetNodeAt(pt.X, pt.Y);
        //            if (tn.PrevVisibleNode != null)
        //                tn.PrevVisibleNode.EnsureVisible();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }

        //    //System.Windows.Forms.TreeNode N = (System.Windows.Forms.TreeNode)sender;
        //    //System.Data.DataRow R = (System.Data.DataRow)N.Tag;
        //    //string Table = R.Table.TableName;
        //    //switch (Table)
        //    //{
        //    //    case "CollectionEventSeries":
        //    //        try
        //    //        {
        //    //            e.Effect = DragDropEffects.Move;
        //    //            TreeView tv = sender as TreeView;
        //    //            Point pt = tv.PointToClient(new Point(e.X, e.Y));
        //    //            int delta = tv.Height - pt.Y;
        //    //            if ((delta < tv.Height / 2) && (delta > 0))
        //    //            {
        //    //                TreeNode tn = tv.GetNodeAt(pt.X, pt.Y);
        //    //                if (tn.NextVisibleNode != null)
        //    //                    tn.NextVisibleNode.EnsureVisible();
        //    //            }
        //    //            if ((delta > tv.Height / 2) && (delta < tv.Height))
        //    //            {
        //    //                TreeNode tn = tv.GetNodeAt(pt.X, pt.Y);
        //    //                if (tn.PrevVisibleNode != null)
        //    //                    tn.PrevVisibleNode.EnsureVisible();
        //    //            }

        //    //        }
        //    //        catch (Exception ex)
        //    //        {
        //    //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    //        }
        //    //        break;
        //    //    case "IdentificationUnit":
        //    //        try
        //    //        {
        //    //            e.Effect = DragDropEffects.Move;
        //    //            TreeView tv = sender as TreeView;
        //    //            Point pt = tv.PointToClient(new Point(e.X, e.Y));
        //    //            int delta = tv.Height - pt.Y;
        //    //            if ((delta < tv.Height / 2) && (delta > 0))
        //    //            {
        //    //                TreeNode tn = tv.GetNodeAt(pt.X, pt.Y);
        //    //                if (tn.NextVisibleNode != null)
        //    //                    tn.NextVisibleNode.EnsureVisible();
        //    //            }
        //    //            if ((delta > tv.Height / 2) && (delta < tv.Height))
        //    //            {
        //    //                TreeNode tn = tv.GetNodeAt(pt.X, pt.Y);
        //    //                if (tn.PrevVisibleNode != null)
        //    //                    tn.PrevVisibleNode.EnsureVisible();
        //    //            }

        //    //        }
        //    //        catch (Exception ex)
        //    //        {
        //    //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    //        }
        //    //        break;
        //    //}
        //}

        //private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        //{
        //    System.Windows.Forms.TreeNode N = (System.Windows.Forms.TreeNode)e.Item;
        //    System.Data.DataRow R = (System.Data.DataRow)N.Tag;
        //    string Table = R.Table.TableName;
        //    switch (Table)
        //    {
        //        case "CollectionEventSeries":
        //        case "CollectionEventList":
        //        case "CollectionSpecimenList":
        //        //case "IdentificationUnit":
        //            this.treeView.DoDragDrop(e.Item, System.Windows.Forms.DragDropEffects.Move);
        //            break;
        //    }
        //}

        //#endregion

        //#endregion





        //private void toolStripButtonShowUnit_Click(object sender, EventArgs e)
        //{
        //    this.setToolStripButtonOverviewHierarchyState(
        //        this.toolStripButtonShowUnit,
        //        DiversityCollection.Specimen.ImageList.Images[(int)DiversityCollection.Specimen.OverviewImageTaxon.Plant],
        //        DiversityCollection.Specimen.ImageList.Images[(int)DiversityCollection.Specimen.OverviewImageTaxon.PlantGrey]);
        //    if (!this.ShowUnit) this.hideHierarchyUnits();
        //    else
        //        this.addHierarchyUnits();
        //}

        //private void addHierarchyUnits()
        //{
        //    if (this.ShowUnit)
        //    {
        //        System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
        //        this.getHierarchyNodes(null, "CollectionSpecimenList", this.treeView, ref Nodes);
        //        if (Nodes.Count > 0)
        //        {
        //            foreach (System.Windows.Forms.TreeNode N in Nodes)
        //            {
        //                this.getEventSeriesEventSpecimenUnits(N);
        //            }
        //        }
        //    }
        //}

        //private void hideHierarchyUnits()
        //{
        //    this.hideHierarchyNodes("IdentificationUnit");
        //    this.hideHierarchyNodes("IdentificationUnitList");
        //}

        //private void hideHierarchyNodes(string Table)
        //{
        //    System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
        //    this.getHierarchyNodes(null, Table, this.treeView, ref Nodes);
        //    foreach (System.Windows.Forms.TreeNode N in Nodes)
        //        N.Remove();
        //}

        //private void getHierarchyNodes(System.Windows.Forms.TreeNode Node, string Table,
        //    System.Windows.Forms.TreeView Treeview,
        //    ref System.Collections.Generic.List<System.Windows.Forms.TreeNode> TreeNodes)
        //{
        //    if (TreeNodes == null) TreeNodes = new List<TreeNode>();
        //    if (Node == null)
        //    {
        //        foreach (System.Windows.Forms.TreeNode N in Treeview.Nodes)
        //        {
        //            if (N.Tag != null)
        //            {
        //                try
        //                {
        //                    System.Data.DataRow R = (System.Data.DataRow)N.Tag;
        //                    if (R.Table.TableName == Table)
        //                        TreeNodes.Add(N);
        //                    this.getHierarchyNodes(N, Table, Treeview, ref TreeNodes);
        //                }
        //                catch { }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
        //        {
        //            if (N.Tag != null)
        //            {
        //                try
        //                {
        //                    System.Data.DataRow R = (System.Data.DataRow)N.Tag;
        //                    if (R.Table.TableName == Table) TreeNodes.Add(N);
        //                    this.getHierarchyNodes(N, Table, Treeview, ref TreeNodes);
        //                }
        //                catch { }
        //            }
        //        }
        //    }
        //}

        //private void setToolStripButtonOverviewHierarchyState(
        //    System.Windows.Forms.ToolStripButton Button,
        //    System.Drawing.Image ImageShow,
        //    System.Drawing.Image ImageHide)
        //{
        //    if (Button.Tag == null)
        //        Button.Tag = "Hide";
        //    else
        //    {
        //        if (Button.Tag.ToString() == "Hide")
        //            Button.Tag = "Show";
        //        else
        //            Button.Tag = "Hide";
        //    }
        //    if (Button.Tag.ToString() == "Hide")
        //    {
        //        Button.Image = ImageHide;
        //        Button.BackColor = System.Drawing.Color.Yellow;
        //    }
        //    else
        //    {
        //        Button.Image = ImageShow;
        //        Button.BackColor = System.Drawing.SystemColors.Control;
        //    }
        //}

        //private void toolStripButtonTreeSearch_Click(object sender, EventArgs e)
        //{
        //    if (this.treeView.SelectedNode != null)
        //    {
        //        if (this.treeView.SelectedNode.Tag != null)
        //        {
        //            try
        //            {
        //                System.Data.DataRow R = (System.Data.DataRow)treeView.SelectedNode.Tag;
        //                switch (R.Table.TableName)
        //                {
        //                    case "IdentificationUnitList":
        //                    case "CollectionSpecimenList":
        //                        int ID;
        //                        if (int.TryParse(R["CollectionSpecimenID"].ToString(), out ID))
        //                        {
        //                            this._CollectionSpecimenID = ID;
        //                            this.DialogResult = DialogResult.OK;
        //                            this.Close();
        //                        }
        //                        break;
        //                    default:
        //                        System.Windows.Forms.MessageBox.Show("Please select a specimen");
        //                        break;
        //                }
        //            }
        //            catch { }
        //        }
        //    }
        //}

        //#endregion

        #region Properties

        public int? CollectionSpecimenID
        {
            get
            {
                return this.userControlEventSeriesTree.CollectionSpecimenID;
            }
        }

        #endregion

        #region Visibility of images and column selection tree

        //private void setImageVisibility(string Visibility)
        //{
        //    try
        //    {

        //        if (this.ShowImages || this.ShowDataTree)
        //        {
        //            this.splitContainerMain.Panel1Collapsed = false;

        //            if (this.ShowImages) this.splitContainerTreeAndImage.Panel2Collapsed = false;
        //            else this.splitContainerTreeAndImage.Panel2Collapsed = true;

        //            if (this.ShowDataTree)
        //                this.splitContainerTreeAndImage.Panel1Collapsed = false;
        //            else this.splitContainerTreeAndImage.Panel1Collapsed = true;

        //        }
        //        else
        //            this.splitContainerMain.Panel1Collapsed = true;

        //    }
        //    catch { }

        //    this.setVisibility();
        //}

        private void buttonHeaderShowImage_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.buttonHeaderShowImage.BackColor == System.Drawing.Color.Red)
                {
                    if (this.dataSetCollectionEventSeries.CollectionEventSeriesImage.Rows.Count > 0)
                        this.buttonHeaderShowImage.BackColor = System.Drawing.Color.Yellow;
                    else
                        this.buttonHeaderShowImage.BackColor = System.Drawing.SystemColors.Control;
                }
                else
                {
                    this.buttonHeaderShowImage.BackColor = System.Drawing.Color.Red;
                    if (this.dataSetCollectionEventSeries.CollectionEventSeriesImage.Rows.Count > 0
                        && this.listBoxImage.Items.Count == 0)
                        this.FormFunctions.FillImageList(this.listBoxImage, this.imageListDataset,
                            this.dataSetCollectionEventSeries.CollectionEventSeriesImage, "URI", this.userControlImage);
                }

            }
            catch { }
            this.setVisibility();
        }

        private void buttonHeaderShowTree_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.buttonHeaderShowTree.BackColor == System.Drawing.SystemColors.Control)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    this.buttonHeaderShowTree.BackColor = System.Drawing.Color.Red;
                    //this.userControlEventSeriesTree.initControl(this._IDs, "CollectionEventSeries", "SeriesID", this.dataGridView, this.dataSetCollectionEventSeries.CollectionEventSeries, toolStripButtonSearchSpecimen_Click);
                    //this.setColumnEventSeriesWidth();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
                else this.buttonHeaderShowTree.BackColor = System.Drawing.SystemColors.Control;

            }
            catch { }
            this.setVisibility();
        }

        private void buttonHeaderShowGeography_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.buttonHeaderShowGeography.BackColor == System.Drawing.Color.Red)
                {
                    System.Data.DataRow[] RR = this.userControlEventSeriesTree.DtEventSeriesGeography.Select("SeriesID = " + this.SeriesID.ToString());
                    if (RR.Length > 0)
                        this.buttonHeaderShowGeography.BackColor = System.Drawing.Color.Yellow;
                    else
                        this.buttonHeaderShowGeography.BackColor = System.Drawing.SystemColors.Control;
                }
                else
                {
                    this.buttonHeaderShowGeography.BackColor = System.Drawing.Color.Red;
                    //if (this.dataSetCollectionEventSeries.CollectionEventSeriesGeography.Rows.Count > 0
                    //    && this.listBoxImage.Items.Count == 0)
                    //    this.FormFunctions.FillImageList(this.listBoxImage, this.imageListDataset,
                    //        this.dataSetCollectionEventSeries.CollectionEventSeriesGeography, "URI", this.userControlImage);
                }

            }
            catch { }
            this.setVisibility();
        }

        private void setVisibility()
        {
            try
            {
                if (this.ShowImages || this.ShowDataTree || this.ShowGeography)
                {
                    this.splitContainerMain.Panel1Collapsed = false;

                    if (this.ShowImages || this.ShowGeography)
                    {
                        this.splitContainerTreeAndImage.Panel2Collapsed = false;
                        if (this.ShowGeography) this.splitContainerImageAndGeography.Panel2Collapsed = false;
                        else this.splitContainerImageAndGeography.Panel2Collapsed = true;
                        if (this.ShowImages) this.splitContainerImageAndGeography.Panel1Collapsed = false;
                        else this.splitContainerImageAndGeography.Panel1Collapsed = true;
                    }
                    else this.splitContainerTreeAndImage.Panel2Collapsed = true;

                    if (this.ShowDataTree)
                        this.splitContainerTreeAndImage.Panel1Collapsed = false;
                    else this.splitContainerTreeAndImage.Panel1Collapsed = true;
                }
                else
                    this.splitContainerMain.Panel1Collapsed = true;
            }
            catch { }
        }

        private bool ShowDataTree
        {
            get
            {
                if (this.buttonHeaderShowTree.BackColor == System.Drawing.SystemColors.Control)
                    return false;
                else return true;
            }
        }

        private bool ShowImages
        {
            get
            {
                if (this.buttonHeaderShowImage.BackColor == System.Drawing.Color.Red)
                    return true;
                else
                    return false;
            }
        }

        private bool ShowGeography
        {
            get
            {
                if (this.buttonHeaderShowGeography.BackColor == System.Drawing.Color.Red)
                    return true;
                else
                    return false;
            }
        }

        #region Geography

        private void toolStripGIS_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        
        #endregion

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            this.fillEventSeriesImages();
            this.setHeaeder();
            this.SetGeoObject();
            //if (this.dataSetCollectionSpecimen.CollectionEventSeriesGeo.Rows.Count > 0)
            //    this.userControlGIS.DataRowCollectionEventSeriesGeo = this.dataSetCollectionSpecimen.CollectionEventSeriesGeo.Rows[0];
            //else
            //    this.userControlGIS.DataRowCollectionEventSeriesGeo = null;

            bool HasGeography = false;
            this._wpfControl.WpfClearAllSamples(true);
            if (this.userControlEventSeriesTree.DtEventSeriesGeography.Rows.Count > 0 &&
                this.ShowGeography)
            {
                System.Data.DataRow[] RR = this.userControlEventSeriesTree.DtEventSeriesGeography.Select("SeriesID = " + this.SeriesID.ToString());
                if (RR.Length > 0)
                {
                    this._GeoObjectsEditor = new List<GeoObject>();
                    this._GeoObjectsEditor.Add(this._GeoObject);
                    this._wpfControl.WpfSetMapAndGeoObjects(this._GeoObjectsEditor);
                    HasGeography = true;
                }
            }
            if (!HasGeography)
            {
                this._wpfControl.WpfClearAllSamples(true);
            }
            //this.userControlEventSeriesTree.seth
        }



        //private string CurrentHeaderDisplaySettings
        //{
        //    get
        //    {
        //        string Setting = "11";

        //        try
        //        {
        //            if (this.ShowColumnSelectionTree) Setting = "1";
        //            else Setting = "0";
        //            if (this.ShowImages) Setting += "1";
        //            else Setting += "0";

        //        }
        //        catch
        //        {
        //            Setting = "11";
        //        }
        //        return Setting;
        //    }
        //}

        //private void toolStripButtonSearchSpecimen_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        #endregion

        #region Autocomplete

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    // getting Table and ColumnName
                    string Column = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
                    string Table = this.dataGridView.DataSource.ToString(); // binding.DataMember;
                    System.Windows.Forms.AutoCompleteStringCollection autoCompleteStringCollection = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionOnDemand(Table, Column);
                    textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
                    textBox.AutoCompleteCustomSource = autoCompleteStringCollection;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Manual

        /// <summary>
        /// Adding event deletates to form and controls
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task InitManual()
        {
            try
            {

                DiversityWorkbench.DwbManual.Hugo manual = new Hugo(this.helpProvider, this);
                if (manual != null)
                {
                    await manual.addKeyDownF1ToForm();
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// ensure that init is only done once
        /// </summary>
        private bool _InitManualDone = false;


        /// <summary>
        /// KeyDown of the form adding event deletates to form and controls within the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!_InitManualDone)
                {
                    await this.InitManual();
                    _InitManualDone = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion
    }
}
