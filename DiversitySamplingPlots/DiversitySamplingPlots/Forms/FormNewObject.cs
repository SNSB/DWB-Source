using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversitySamplingPlots
{
    public partial class FormNewObject : Form
    {
        private int _PlotID;
        private System.Data.DataTable _dtObjects;


        public FormNewObject(int PlotID)
        {
            InitializeComponent();
            this._PlotID = PlotID;
            this.initForm();
        }

        private void initForm()
        {
            DiversityWorkbench.CollectionSpecimen C = new DiversityWorkbench.CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
            this.userControlModuleRelatedEntryCollectionSpecimen.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)C;

            string SQL = "select O.DisplayText, O.SamplingObjectID " +
                "from dbo.SamplingObject O, dbo.SamplingObjectInPlot OP, dbo.SamplingPlotHierarchy(" + this._PlotID + ") H " +
                "where H.PlotID = OP.PlotID AND O.SamplingObjectID = OP.SamplingObjectID";

            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            this._dtObjects = new DataTable();
            ad.Fill(this._dtObjects);
            this.listBoxObjects.DataSource = this._dtObjects;
            this.listBoxObjects.DisplayMember = "DisplayText";
            this.listBoxObjects.ValueMember = "SamplingObjectID";

            if (this._dtObjects.Rows.Count > 0)
            {
                this.radioButtonFromList.Checked = true;
            }
            else
            {
                this.tableLayoutPanelSelector.Visible = false;
                this.splitContainerMain.Panel1Collapsed = true;
                this.splitContainerMain.Panel2Collapsed = false;
            }
        }

        private void radioButtonFromList_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonFromList.Checked)
            {
                this.splitContainerMain.Panel1Collapsed = false;
                this.splitContainerMain.Panel2Collapsed = true;
            }
            else
            {
                this.splitContainerMain.Panel1Collapsed = true;
                this.splitContainerMain.Panel2Collapsed = false;
            }
        }

        #region Properties
        public bool CreateNewObject 
        { 
            get 
            {
                if (!this.tableLayoutPanelSelector.Visible) return true;
                else return this.radioButtonNew.Checked; 
            } 
        }

        public int SamplingObjectID
        {
            get
            {
                int ID = 0;
                int.TryParse(this.listBoxObjects.SelectedValue.ToString(), out ID);
                return ID;
            }
        }

        public string DisplayText { get { return this.textBoxObjectDisplayText.Text; } }
        public string Description { get { return this.textBoxObjectsDescription.Text; } }
        public string SamplingObjectURI { get { return this.userControlModuleRelatedEntryCollectionSpecimen.labelURI.Text; } }
        
        #endregion
    }
}
