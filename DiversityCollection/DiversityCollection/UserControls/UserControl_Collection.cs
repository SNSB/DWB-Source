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
    public partial class UserControl_Collection : UserControl__Data
    {

        #region Construction

        public UserControl_Collection(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this._Source = Source;
            this._HelpNamespace = HelpNamespace;
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        #endregion

        #region Control

        private void initControl()
        {
            this.textBoxCollectionName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "CollectionName", true));
            this.textBoxCollectionAcronym.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "CollectionAcronym", true));
            this.textBoxCollectionDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Description", true));
            this.textBoxCollectionLocation.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Location", true));
            this.textBoxCollectionOwner.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "CollectionOwner", true));

            DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
            this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryCollectionContact, A, "Collection", "AdministrativeContactName", "AdministrativeContactAgentURI", this._Source);

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);
            //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxCollectionAcronym);
            //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxCollectionLocation);
            //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxCollectionName);
            //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxCollectionOwner);

            this.CheckIfClientIsUpToDate();
        }

        #endregion

        #region Interface
        public override void SetPosition(int Position)
        {
            base.SetPosition(Position);
            this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryCollectionContact, "AdministrativeContactAgentURI");
        }


        public void setAvailability(bool IsAvailable)
        {
            this.tableLayoutPanelCollection.Enabled = IsAvailable;
        }

        public void EnableEditDefaultCollection(bool Enabled)
        {
            this.buttonEditDefaultCollection.Enabled = Enabled;
        }

        private int? _DefaultCollection = -1;

        public int DefaultCollection
        {
            get
            {
                if (this._DefaultCollection == null)
                {
                    if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultCollection != -1)
                    {
                        this._DefaultCollection = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultCollection;
                        return (int)this._DefaultCollection;
                    }
                    if (this._DefaultCollection == null)
                        return -1;
                    else
                        return (int)this._DefaultCollection;
                }
                else
                {
                    return (int)this._DefaultCollection;
                }
            }

            set
            {
                this._DefaultCollection = value;
                this.textBoxDefaultCollection.Text = DiversityCollection.LookupTable.CollectionNameHierarchy((int)this._DefaultCollection);
                string Owner = DiversityCollection.LookupTable.CollectionOwner((int)this._DefaultCollection);
                string ToolTip = this.textBoxDefaultCollection.Text;
                if (Owner.Length > 0) ToolTip = Owner + ": " + ToolTip;
                this.toolTip.SetToolTip(this.textBoxDefaultCollection, ToolTip);
                if ((int)this._DefaultCollection != DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultCollection)
                {
                    DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultCollection = (int)this._DefaultCollection;
                    DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
                }
            }
        }

        #endregion

        #region Events

        private void buttonOpenFormCollection_Click(object sender, EventArgs e)
        {
            int CollectionID;
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            if (int.TryParse(R["CollectionID"].ToString(), out CollectionID))
            {
                DiversityCollection.Forms.FormCollection f = new Forms.FormCollection(CollectionID, this._iMainForm);
                f.ShowDialog();
            }
        }

        private void buttonEditDefaultCollection_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Auosuggest

        private void textBoxCollectionName_KeyUp(object sender, KeyEventArgs e)
        {

        }

        #endregion

    }
}
