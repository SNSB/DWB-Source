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
    public partial class UserControl_ExternalIdentifier : UserControl__Data
    {

        #region Parameter

        private System.Data.DataTable _DtType;
        //private string _IDcolumn;
        
        #endregion

        #region Construction

        public UserControl_ExternalIdentifier(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            //string IDcolumn,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        #endregion

        #region Control

        private void initControl()
        {
            this.comboBoxType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "Type", true));
            this.setTypeSource();

            this.textBoxNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Notes", true));
            this.textBoxURL.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "URL", true));
            this.textBoxID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Identifier", true));

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();
        }

        private void buttonSetURL_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxURL.Text);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                this.textBoxURL.Text = f.URL;
        }

        public void setTypeSource(bool Reset = false)
        {
            if (DiversityCollection.LookupTable.DtCollectionWithHierarchy != null && (this.comboBoxType.DataSource == null || Reset))
            {
                this._DtType = new DataTable();
                string SQL = "SELECT Type FROM ExternalIdentifierType ORDER BY Type";
                string Message = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._DtType, ref Message);
                this.comboBoxType.DataSource = this._DtType;
                this.comboBoxType.DisplayMember = "Type";
                this.comboBoxType.ValueMember = "Type";
                this.comboBoxType.DropDownWidth = 360;

            }
        }

        public override void InitLookupSources() { this.setTypeSource(); }

        #endregion

        #region Help

        public System.Windows.Forms.TableLayoutPanel TableLayoutPanel { get { return this.tableLayoutPanel; } }

        #endregion

    }
}
