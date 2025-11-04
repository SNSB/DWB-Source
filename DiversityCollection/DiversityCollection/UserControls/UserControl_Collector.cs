using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.UserControls
{
    public partial class UserControl_Collector : UserControl__Data
    {
        #region Constr
        public UserControl_Collector(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        #endregion

        #region Init
        private void initControl()
        {
            this.textBoxCollectorNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Notes", true));
            this.textBoxCollectorsNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "CollectorsNumber", true));
            
            this.comboBoxCollectorDataWithholdingReason.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "DataWithholdingReason", true));
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxCollectorDataWithholdingReason);
     
            DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
            this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryCollector, A, "CollectionAgent", "CollectorsName", "CollectorsAgentURI", this._Source);
            this.userControlModuleRelatedEntryCollector.textBoxValue.TextChanged += new System.EventHandler(this.updateHierarchyNodeByUserControlModuleRelatedEntry);

            //System.Collections.Generic.List<string> Settings = new List<string>();
            //Settings.Add("ModuleSource");
            //Settings.Add("CollectionAgent");
            //Settings.Add("CollectorsAgentURI");
            //this.setUserControlModuleRelatedEntrySources(Settings, ref this.userControlModuleRelatedEntryCollector);

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true, AutoCompleteMode.Suggest);

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();

            this.SetOptionResponsible();
        }

        private void SetOptionResponsible()
        {
            try
            {
                this.userControlModuleRelatedEntryCollector.ShowInfo = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames;
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames) // &&!_InfoTextAcceptedNameEventHandlerSet)
                {
                    this.userControlModuleRelatedEntryCollector.labelURI.TextChanged += new System.EventHandler(this.setInfoTextResponsible);
                }
                else
                {
                    this.userControlModuleRelatedEntryCollector.labelURI.TextChanged -= this.setInfoTextResponsible;
                    this.userControlModuleRelatedEntryCollector.SetInfoText(DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames, "", "", System.Drawing.SystemColors.WindowText);
                }
                this.userControlModuleRelatedEntryCollector.Height = this.userControlModuleRelatedEntryCollector.HeightOfControl();
            }
            catch (System.Exception ex)
            { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void setInfoTextResponsible(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Label L = (System.Windows.Forms.Label)sender;
                string URI = L.Text;
                string UrlResponsible = "";
                string Responsible = this.GetResponsibleName(URI, ref UrlResponsible);
                this.userControlModuleRelatedEntryCollector.SetInfoText(DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowValidAgentName, Responsible, UrlResponsible, System.Drawing.Color.Green);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string GetResponsibleName(string URI, ref string UrlResponsible)
        {
            string Responsible = "";
            if (URI.Length > 0)
                Responsible = DiversityWorkbench.Agent.AcceptedName(URI, ref UrlResponsible);
            return Responsible;
        }



        #endregion

        #region Interface

        public override void SetPosition(int Position)
        {
            base.SetPosition(Position);
            this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryCollector, "CollectorsAgentURI");
        }

        #endregion

        #region Events
        private void updateHierarchyNodeByUserControlModuleRelatedEntry(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)sender;
                if (T.DataBindings.Count == 0) return;
                System.Windows.Forms.Binding B = T.DataBindings[0];
                if (B.DataSource == null) return;
                System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)B.DataSource;
                if (BS.Current == null) return;
                System.Data.DataRowView RV = (System.Data.DataRowView)BS.Current;
                System.Data.DataRow R = RV.Row;
                System.Windows.Forms.TreeNode N = this._iMainForm.SelectedUnitHierarchyNode();
                if (N == null) return;
                DiversityCollection.HierarchyNode H = (DiversityCollection.HierarchyNode)N;
                if (H == null) return;
                H.setText();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void comboBoxCollectorDataWithholdingReason_TextChanged(object sender, EventArgs e)
        {
            this.SetDataWithholdingControl(this.comboBoxCollectorDataWithholdingReason, this.pictureBoxCollectorDataWithholdingReason);
        }

        private void comboBoxCollectorDataWithholdingReason_DropDown(object sender, EventArgs e)
        {
            string SQL = "SELECT DISTINCT DataWithholdingReason FROM CollectionAgent ORDER BY DataWithholdingReason";
            System.Data.DataTable dt = new DataTable();
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                this.comboBoxCollectorDataWithholdingReason.DataSource = dt;
                this.comboBoxCollectorDataWithholdingReason.DisplayMember = "DataWithholdingReason";
                this.comboBoxCollectorDataWithholdingReason.ValueMember = "DataWithholdingReason";
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonTemplateCollectorSet_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.TemplateForData T = new DiversityWorkbench.TemplateForData("CollectionAgent", TemplateCollectorSuppressedColumns);
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            T.CopyTemplateToRow(R.Row);
        }

        private System.Collections.Generic.List<string> TemplateCollectorSuppressedColumns
        {
            get
            {
                System.Collections.Generic.List<string> Suppress = new List<string>();
                Suppress.Add("CollectionSpecimenID");
                Suppress.Add("CollectorsSequence");
                Suppress.Add("LogCreatedWhen");
                Suppress.Add("LogCreatedBy");
                Suppress.Add("LogUpdatedWhen");
                Suppress.Add("LogUpdatedBy");
                Suppress.Add("RowGUID");
                return Suppress;
            }
        }

        private void buttonTemplateCollectorEdit_Click(object sender, EventArgs e)
        {
            System.Data.DataRow R = ((System.Data.DataRowView)this._Source.Current).Row;
            DiversityWorkbench.Forms.FormTemplateEditor f = new DiversityWorkbench.Forms.FormTemplateEditor("CollectionAgent", R, this.TemplateCollectorSuppressedColumns);
            f.setHelp("Template");
            f.ShowDialog();
        }

        #endregion

        #region Properties

        public System.Windows.Forms.TableLayoutPanel TableLayoutPanelCollector { get { return this.tableLayoutPanelCollector; } }

        #endregion
    }
}
