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
    public partial class UserControl_Label : UserControl__Data
    {

        #region Construction

        public UserControl_Label(
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

        #region Public

        public void setTitle(string Title)
        {
            this.groupBoxLabel.Text = Title;
        }

        public System.Windows.Forms.TableLayoutPanel TableLayoutPanelLabel { get { return this.tableLayoutPanelLabelDetails; } }

        #endregion

        #region Control

        private void initControl()
        {
            this.textBoxLabelTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "LabelTitle", true));
            this.textBoxLabelNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "LabelTranscriptionNotes", true));
            this.comboBoxLabelType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "LabelType", true));
            this.comboBoxLabelTranscriptionState.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "LabelTranscriptionState", true));
            this.comboBoxLabelTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "LabelTitle", true));

            //this.initEnumCombobox(this.comboBoxLabelType, "CollLabelType_Enum");
            //this.initEnumCombobox(this.comboBoxLabelTranscriptionState, "CollLabelTranscriptionState_Enum");
            this._EnumComboBoxes = new Dictionary<ComboBox, string>();
            this._EnumComboBoxes.Add(this.comboBoxLabelType, "CollLabelType_Enum");
            this._EnumComboBoxes.Add(this.comboBoxLabelTranscriptionState, "CollLabelTranscriptionState_Enum");
            this.InitLookupSources();

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxLabelTitle);
            //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxLabelTitle);
            //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxLabelNotes);

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();
        }

        public override void InitLookupSources() { this.InitEnums(); }
        
        private void comboBoxLabelTitle_DropDown(object sender, EventArgs e)
        {
            string SQL = "SELECT NULL AS LabelTitle UNION SELECT DISTINCT LabelTitle FROM CollectionSpecimen " +
                " WHERE (NOT (LabelTitle IS NULL) AND LEN(LabelTitle) > 0) ";
            if (this.comboBoxLabelTitle.Text.Length > 0) SQL += " AND LabelTitle LIKE '" + this.comboBoxLabelTitle.Text + "%'";
            SQL += " ORDER BY LabelTitle";
            System.Data.DataTable dtLabelTitle = new DataTable();
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtLabelTitle);
                this.comboBoxLabelTitle.DataSource = dtLabelTitle;
                this.comboBoxLabelTitle.DisplayMember = "LabelTitle";
                this.comboBoxLabelTitle.ValueMember = "LabelTitle";

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private bool _ShowLabelTitleTextBox = false;
        private void buttonLabelTitle_Click(object sender, EventArgs e)
        {
            this.SwitchLabelTitleDisplay();
        }

        private void SwitchLabelTitleDisplay()
        {
            _ShowLabelTitleTextBox = !_ShowLabelTitleTextBox;
            this.textBoxLabelTitle.Visible = _ShowLabelTitleTextBox;
            this.comboBoxLabelTitle.Visible = !_ShowLabelTitleTextBox;
            if(_ShowLabelTitleTextBox)
            {
                this.comboBoxLabelTitle.Dock = DockStyle.None;
                this.textBoxLabelTitle.Dock = DockStyle.Fill;
                this.buttonLabelTitle.Image = DiversityCollection.Resource.ArrowDown;
                this.toolTip.SetToolTip(this.buttonLabelTitle, "Switch to combobox");
            }
            else
            {
                this.comboBoxLabelTitle.Dock = DockStyle.Fill;
                this.textBoxLabelTitle.Dock = DockStyle.None;
                this.buttonLabelTitle.Image = DiversityCollection.Resource.List;
                this.toolTip.SetToolTip(this.buttonLabelTitle, "Switch to textbox");
                //if (this.comboBoxLabelTitle.Text.Length == 0 && this.textBoxLabelTitle.Text.Length > 0)
                    this.comboBoxLabelTitle.Text = this.textBoxLabelTitle.Text;
            }
            this.adaptHeigt();
        }

        private void adaptHeigt()
        {
            if (_ShowLabelTitleTextBox)
                this.panelLabelTitle.Height = (int)((this.Height - 26) / 2);
            else
                this.panelLabelTitle.Height = 24;
        }

        private void UserControl_Label_Resize(object sender, EventArgs e)
        {
            this.adaptHeigt();
        }
        #endregion

    }
}
