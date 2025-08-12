using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormProperty : Form
    {

        #region Parameter
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterProperty;
        
        #endregion

        #region Construction
        public FormProperty()
        {
            InitializeComponent();
            this.initForm();
        }

        #endregion

        #region Form
        private void initForm()
        {
            this.RequeryData();

            DiversityWorkbench.Terminology T = new DiversityWorkbench.Terminology(DiversityWorkbench.Settings.ServerConnection);
            this.userControlModuleRelatedEntryProperty.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)T;
            this.userControlModuleRelatedEntryProperty.Domain = "Terminology";
            this.userControlModuleRelatedEntryProperty.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;
            this.userControlModuleRelatedEntryProperty.bindToData("Property", "PropertyName", "PropertyURI", this.propertyBindingSource);

            System.Collections.Generic.List<DiversityWorkbench.UserControls.RemoteValueBinding> RvbProperty = new List<DiversityWorkbench.UserControls.RemoteValueBinding>();

            DiversityWorkbench.UserControls.RemoteValueBinding RvbPropertyID = new DiversityWorkbench.UserControls.RemoteValueBinding();
            RvbPropertyID.BindingSource = this.propertyBindingSource;
            RvbPropertyID.Column = "PropertyID";
            RvbPropertyID.RemoteParameter = "TerminologyID";
            RvbProperty.Add(RvbPropertyID);

            DiversityWorkbench.UserControls.RemoteValueBinding RvbDisplayText = new DiversityWorkbench.UserControls.RemoteValueBinding();
            RvbDisplayText.BindingSource = this.propertyBindingSource;
            RvbDisplayText.Column = "DisplayText";
            RvbDisplayText.RemoteParameter = "DisplayText";
            RvbProperty.Add(RvbDisplayText);

            DiversityWorkbench.UserControls.RemoteValueBinding RvbDescription = new DiversityWorkbench.UserControls.RemoteValueBinding();
            RvbDescription.BindingSource = this.propertyBindingSource;
            RvbDescription.Column = "Description";
            RvbDescription.RemoteParameter = "Description";
            RvbProperty.Add(RvbDescription);

            this.userControlModuleRelatedEntryProperty.setRemoteValueBindings(RvbProperty);

            this.toolTip.AutoPopDelay = 30000;

            this.FormFunctions.setDescriptions();

        }

        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        private DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                return this._FormFunctions;
            }
        }

        private void RequeryData()
        {
            if (this.dataSetProperty.Property.Rows.Count > 0)
            {
                try
                {
                    this._sqlDataAdapterProperty.Update(this.dataSetProperty.Property);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }

            }
            this.dataSetProperty.Clear();

            string SQL = "SELECT PropertyID, PropertyParentID, PropertyName, DefaultAccuracyOfProperty, DefaultMeasurementUnit, ParsingMethodName, DisplayText, DisplayEnabled, " +
                "DisplayOrder, Description, PropertyURI " +
                "FROM Property " +
                "ORDER BY PropertyName";
            DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterProperty, this.dataSetProperty.Property, SQL, DiversityWorkbench.Settings.ConnectionString);
        }

        private void FormProperty_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetProperty.Property". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.propertyTableAdapter.Fill(this.dataSetProperty.Property);

        }

        private void FormProperty_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._sqlDataAdapterProperty.Update(this.dataSetProperty.Property);
        }

        #endregion

        #region private events

        private void comboBoxParsingMethodName_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboBoxParsingMethodName.Text)
            {
                case "Vegetation":
                    this.pictureBoxParsingMethodName.Image = this.imageListParsingMethodName.Images[0];
                    break;
                default:
                    this.pictureBoxParsingMethodName.Image = this.imageListParsingMethodName.Images[1];
                    break;
            }
        }

        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRow R = this.dataSetProperty.Property.NewRow();
                R["DisplayText"] = "New property";
                R["PropertyName"] = "New property";
                R["PropertyID"] = -1;
                R["DisplayEnabled"] = 1;
                R["ParsingMethodName"] = "Stratigraphy";
                this.dataSetProperty.Property.Rows.Add(R);
            }
            catch (Exception ex)
            {
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.propertyBindingSource.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.propertyBindingSource.Current;
                    R.Delete();
                    this.RequeryData();
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButtonImport_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Terminology T = new DiversityWorkbench.Terminology(DiversityWorkbench.Settings.ServerConnection);
                DiversityWorkbench.Forms.FormRemoteQuery f = new DiversityWorkbench.Forms.FormRemoteQuery(T, "Terminology", true);
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    System.Collections.Generic.Dictionary<string, string> UnitValues = T.UnitValues();
                    System.Data.DataRow R = this.dataSetProperty.Property.NewRow();
                    R["DisplayText"] = UnitValues["DisplayText"];
                    R["PropertyName"] = UnitValues["DisplayText"];
                    R["PropertyID"] = int.Parse(UnitValues["TerminologyID"]);
                    R["DisplayEnabled"] = 1;
                    R["ParsingMethodName"] = "Vegetation";
                    R["PropertyURI"] = UnitValues["_URI"];
                    R["Description"] = UnitValues["Description"];
                    this.dataSetProperty.Property.Rows.Add(R);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void toolStripButtonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        #endregion

    }
}
