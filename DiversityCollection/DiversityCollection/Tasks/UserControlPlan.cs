using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityCollection.Tasks
{
    public partial class UserControlPlan : UserControl
    {

        #region Parameter

        private int _CollectionID;
        private WpfControls.Geometry.UserControlGeometry _UserControlGeometry;

        private int _CollectionTaskID;

        #endregion

        #region Construction and init

        public UserControlPlan()
        {
            InitializeComponent();
            this.InitFlorplanControls();
        }

        private void InitFlorplanControls()
        {
            _UserControlGeometry = new WpfControls.Geometry.UserControlGeometry();
            elementHost.Child = _UserControlGeometry;
            _UserControlGeometry.SetAddButtonsEnabled(false);
        }

        #endregion

        #region Interface

        public void SetCollectionID(int ID)
        {
            this._CollectionID = ID;
            this.SetFloorPlan();
        }

        public void setEditState(WpfControls.Geometry.UserControlGeometry.State state)
        {
            this._UserControlGeometry.setState(state);
        }

        #region CollectionTask

        public void setCollectionTaskImage(int ID, string URI)
        {
            try
            {
                this._CollectionTaskID = ID;
                this._UserControlGeometry.SetImage(URI);
                this._UserControlGeometry.SetAddButtonsEnabled();
                //string SQL = "SELECT ObjectGeometry.ToString() FROM dbo.CollectionTaskImage WHERE CollectionTaskID = " + ID.ToString() + " AND URI = '" + URI + "'";
                //string Geometry = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                //this._UserControlGeometry.SetRectangleAndPolygonGeometry(Geometry, "");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setCollectionTaskImageGeometry(int ID, string URI)
        {
            try
            {
                //this._CollectionTaskID = ID;
                //this._UserControlGeometry.SetImage(URI);
                string SQL = "SELECT ObjectGeometry.ToString() FROM dbo.CollectionTaskImage WHERE CollectionTaskID = " + ID.ToString() + " AND URI = '" + URI + "'";
                string Geometry = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                this._UserControlGeometry.SetRectangleAndPolygonGeometry(Geometry, "");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setCollectionTaskImageGeometry()
        {
            try
            {
                this._UserControlGeometry.SetRectangleAndPolygonGeometry("");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public string getRectangleGeometry()
        {
            System.Windows.Media.RectangleGeometry rectangle = this._UserControlGeometry.GetRectangle();
            bool HasArea = false;
            if (rectangle.Bounds.Height > 0 || rectangle.Bounds.Width > 0)
                HasArea = true;
            string geo = "";
            if (HasArea)
            {
                geo = "geometry::STGeomFromText('POLYGON ((";
                geo += rectangle.Bounds.BottomLeft.X.ToString() + " " + rectangle.Bounds.BottomLeft.Y.ToString() + ", " +
                    rectangle.Bounds.TopLeft.X.ToString() + " " + rectangle.Bounds.TopLeft.Y.ToString() + ", " +
                    rectangle.Bounds.TopRight.X.ToString() + " " + rectangle.Bounds.TopRight.Y.ToString() + ", " +
                    rectangle.Bounds.BottomRight.X.ToString() + " " + rectangle.Bounds.BottomRight.Y.ToString() + ", " +
                    rectangle.Bounds.BottomLeft.X.ToString() + " " + rectangle.Bounds.BottomLeft.Y.ToString();
                geo += "))', 0) ";
            }
            else
            {
                geo = "";
            }
            return geo;
        }

        #endregion

        #endregion

        #region setting the plan

        private void SetFloorPlan()
        {
            try
            {
                string SQL = "SELECT LocationPlan FROM dbo.CollectionHierarchySuperior(" + _CollectionID.ToString() + ") WHERE CollectionID = " + _CollectionID.ToString();
                string FloorPlan = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (FloorPlan.Length > 0)
                {
                    this._UserControlGeometry.SetImage(FloorPlan);
                    SQL = "SELECT LocationGeometry.ToString() FROM dbo.CollectionHierarchySuperior(" + _CollectionID.ToString() + ") WHERE CollectionID = " + _CollectionID.ToString();
                    string Geometry = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    string ParentGeometry = "";
                    if (Geometry.Length == 0)
                    {
                        SQL = "SELECT [LocationGeometry].ToString() FROM dbo.CollectionHierarchySuperior(" + _CollectionID.ToString() + ") WHERE CollectionID = " + _CollectionID.ToString();
                    }
                    else
                        SQL = "SELECT P.[LocationGeometry].ToString() FROM [dbo].[Collection] C INNER JOIN dbo.CollectionHierarchySuperior(" + _CollectionID.ToString() + ") P ON C.CollectionParentID = P.CollectionID AND C.CollectionID = " + _CollectionID.ToString();
                    ParentGeometry = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
                    this._UserControlGeometry.SetRectangleAndPolygonGeometry(Geometry, ParentGeometry);
                    SQL = "SELECT LocationPlanWidth FROM dbo.CollectionHierarchySuperior(" + _CollectionID.ToString() + ") WHERE CollectionID = " + _CollectionID.ToString();
                    string Scale = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    this._UserControlGeometry.SetScaleLine(Scale);
                }
                else
                {
                    this._UserControlGeometry.SetImage("");
                    this._UserControlGeometry.SetRectangleAndPolygonGeometry("");
                    this._UserControlGeometry.SetScaleLine("");
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

    }
}
