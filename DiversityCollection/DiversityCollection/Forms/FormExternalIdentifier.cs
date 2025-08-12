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
    public partial class FormExternalIdentifier : Form
    {

        #region Parameter

        System.Data.DataTable _DtType;
        System.Data.DataTable _DtIdentifier;
        System.Data.DataView _ViewIdentifier;
        DiversityWorkbench.ReferencingTable.IdentifierType _TypeOfIdentifier;
        string _WorkbenchUnit = "";
        
        #endregion

        #region Construction

        public FormExternalIdentifier(DiversityWorkbench.ReferencingTable.IdentifierType TypeOfIdentifier)
        {
            InitializeComponent();
            this._TypeOfIdentifier = TypeOfIdentifier;
            //this.initForm(TypeOfIdentifier);
            this.initFormForSingleIdentifier(TypeOfIdentifier);
        }

        public FormExternalIdentifier(string Identifier, string Type, string URL, string Notes, string WorkbenchUnit, DiversityWorkbench.ReferencingTable.IdentifierType TypeOfIdentifier)
        {
            InitializeComponent();
            this._WorkbenchUnit = WorkbenchUnit;
            this._TypeOfIdentifier = TypeOfIdentifier;
            this.Text = Type;
            //this.initForm(TypeOfIdentifier);
            switch (this._TypeOfIdentifier)
            {
                case DiversityWorkbench.ReferencingTable.IdentifierType.ID:
                    this.textBoxID.Text = Identifier;
                    this.textBoxURL.Text = URL;
                    break;
            }
            this.textBoxNotes.Text = Notes;
            this.initFormForSingleIdentifier(TypeOfIdentifier);
            if (this._DtType != null)
            {
                for (int i = 0; i < this._DtType.Rows.Count; i++)
                {
                    if (this._DtType.Rows[i][0].ToString() == Type)
                    {
                        this.comboBoxType.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                for (int ii = 0; ii < this.comboBoxType.Items.Count; ii++)
                {
                    if (this.comboBoxType.Items[ii].ToString() == Type)
                    {
                        this.comboBoxType.SelectedIndex = ii;
                        break;
                    }
                }
            }
        }

        public FormExternalIdentifier(System.Data.DataTable DtIdentifier, DiversityWorkbench.ReferencingTable.IdentifierType TypeOfIdentifier)
        {
            InitializeComponent();
            this._DtIdentifier = DtIdentifier;
            this._TypeOfIdentifier = TypeOfIdentifier;
            this.initFormForIdentifierOverview();
        }

        #endregion

        #region Form

        //private void initForm(DiversityWorkbench.ReferencingTable.IdentifierType TypeOfIdentifier)
        //{
        //}

        //private void initFormForSingleIdentifier()
        //{
        //    this.splitContainerMain.Panel2Collapsed = true;
        //    this.Height = 180;
        //    this.tableLayoutPanel.BringToFront();
        //    this.splitContainerMain.BringToFront();
        //    //string SQL = "SELECT T.Type FROM ExternalIdentifierType T ";
        //    //string WhereClause = "";
        //    //foreach (string T in DiversityWorkbench.ReferencingTable.Regulations())
        //    //{
        //    //    if (WhereClause.Length > 0) WhereClause += ", ";
        //    //    WhereClause += "'" + T + "'";
        //    //}
        //    //SQL += "WHERE T.Type NOT IN (" + WhereClause + ") ORDER BY Type";
        //    //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //    //this._DtType = new DataTable();
        //    //ad.Fill(this._DtType);
        //    //this.comboBoxType.DataSource = this._DtType;
        //    //this.comboBoxType.DisplayMember = "Type";
        //    //this.comboBoxType.ValueMember = "Type";
        //    System.Collections.Generic.List<string> Types = new List<string>();
        //    switch (this._TypeOfIdentifier)
        //    {
        //        case DiversityWorkbench.ReferencingTable.IdentifierType.ID:
        //            Types = DiversityWorkbench.ReferencingTable.Regulations();
        //            break;
        //        case DiversityWorkbench.ReferencingTable.IdentifierType.Regulation:
        //            Types = DiversityWorkbench.ReferencingTable.IDs();
        //            break;
        //    }
        //    foreach (string T in Types)
        //        this.comboBoxType.Items.Add(T);
        //    this.comboBoxType.SelectedIndex = 0;
        //    DiversityWorkbench.Forms.FormFunctions.addEditOnDoubleClickToTextboxes(this.textBoxID);
        //}

        private void initFormForSingleIdentifier(DiversityWorkbench.ReferencingTable.IdentifierType TypeOfIdentifier)
        {
            this.splitContainerMain.Panel2Collapsed = true;
            this.Height = 180;
            this.Text = TypeOfIdentifier.ToString();
            this.tableLayoutPanel.BringToFront();
            this.splitContainerMain.BringToFront();
            this.comboBoxType.Items.Clear();
            // visibility etc.
            switch (this._TypeOfIdentifier)
            {
                case DiversityWorkbench.ReferencingTable.IdentifierType.ID:

                    this.pictureBoxID.Visible = true;
                    this.textBoxID.Visible = true;
                    this.labelURL.Visible = true;
                    this.textBoxURL.Visible = true;
                    this.buttonSetURL.Visible = true;

                    this.userControlModuleRelatedEntryRestriction.Visible = false;
                    break;
                //case DiversityWorkbench.ReferencingTable.IdentifierType.Regulation:
                //    this.pictureBoxID.Visible = false;
                //    this.textBoxID.Visible = false;
                //    this.labelURL.Visible = false;
                //    this.textBoxURL.Visible = false;
                //    this.buttonSetURL.Visible = false;

                //    this.userControlModuleRelatedEntryRestriction.Visible = true;

                //    this.ForeColor = System.Drawing.Color.Red;
                //    this.pictureBoxID.Image = this.imageListTree.Images["Paragraph.ico"];
                //    Bitmap theBitmap = new Bitmap(this.imageListTree.Images["Paragraph.ico"], new Size(16, 16));
                //    IntPtr Hicon = theBitmap.GetHicon();// Get an Hicon for myBitmap.
                //    Icon newIcon = Icon.FromHandle(Hicon);// Create a new icon from the handle.
                //    this.Icon = newIcon;
                //    break;
            }
            // Types
            switch (TypeOfIdentifier)
            {
                case DiversityWorkbench.ReferencingTable.IdentifierType.ID:
                    foreach (string T in DiversityWorkbench.ReferencingTable.IDs())
                        this.comboBoxType.Items.Add(T);
                    break;
                //case DiversityWorkbench.ReferencingTable.IdentifierType.Regulation:
                //    foreach (string T in DiversityWorkbench.ReferencingTable.Regulations())
                //        this.comboBoxType.Items.Add(T);
                //    break;
            }
            // Databinding
            switch (this._TypeOfIdentifier)
            {
                case DiversityWorkbench.ReferencingTable.IdentifierType.ID:
                    break;
                //case DiversityWorkbench.ReferencingTable.IdentifierType.Regulation:
                //    if (this._WorkbenchUnit.Length > 0)
                //    {
                //        if (this._WorkbenchUnit == "Transaction")
                //        {
                //            DiversityWorkbench.Transaction T = new DiversityWorkbench.Transaction(DiversityWorkbench.Settings.ServerConnection);
                //            this.userControlModuleRelatedEntryRestriction.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)T;
                //        }
                //        else if (this._WorkbenchUnit == "DiversityProjects")
                //        {
                //            DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
                //            this.userControlModuleRelatedEntryRestriction.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)P;
                //        }
                //    }
                //    else
                //    {
                //        DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
                //        this.userControlModuleRelatedEntryRestriction.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)P;
                //    }
                //    break;
            }

            DiversityWorkbench.Forms.FormFunctions.addEditOnDoubleClickToTextboxes(this.textBoxID);
        }

        private void initFormForIdentifierOverview()
        {
            this.splitContainerMain.Panel1Collapsed = true;
            this.userControlDialogPanel.Visible = false;
            this.Height = 300;
            this.buildTree();
        }

        private void buildTree()
        {
            this.treeViewIdentifier.Nodes.Clear();
            // Event
            this.AddIdentifierRange("CollectionEvent", "Collection event", 1);
            // Specimen
            this.AddIdentifierRange("CollectionSpecimen", "Collection specimen", 2);
            // Unit
            this.AddIdentifierRange("IdentificationUnit", "Organisms", 3);
            // Part
            this.AddIdentifierRange("CollectionSpecimenPart", "Specimen part", 4);

            this.treeViewIdentifier.ExpandAll();
        }

        private void AddIdentifierRange(string Table, string DisplayTextRootNode, int ImageIndex)
        {
            try
            {
                string Restriction = "";
                System.Collections.Generic.List<string> Types = new List<string>();
                switch (this._TypeOfIdentifier)
                {
                    case DiversityWorkbench.ReferencingTable.IdentifierType.ID:
                        Types = DiversityWorkbench.ReferencingTable.IDs();
                        break;
                    //case DiversityWorkbench.ReferencingTable.IdentifierType.Regulation:
                    //    Types = DiversityWorkbench.ReferencingTable.Regulations();
                    //    break;
                }
                foreach (string T in Types)
                {
                    if (Restriction.Length > 0) Restriction += ", ";
                    Restriction += "'" + T.Replace("'", "''") + "'";
                }
                Restriction = "Type IN(" + Restriction + ") AND ReferencedTable = '" + Table + "'";
                System.Data.DataRow[] RR = this._DtIdentifier.Select(Restriction, "ID");
                if (RR.Length > 0)
                {
                    System.Windows.Forms.TreeNode NE = new TreeNode(DisplayTextRootNode, ImageIndex, ImageIndex);
                    this.treeViewIdentifier.Nodes.Add(NE);
                    foreach (System.Data.DataRow R in RR)
                    {
                        System.Windows.Forms.TreeNode NR = new TreeNode(R["Type"].ToString() + ": " + R["Identifier"].ToString());// + " [" + R["Type"].ToString() + "]");
                        NR.ImageIndex = 0;
                        NR.SelectedImageIndex = 0;
                        NE.Nodes.Add(NR);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        public void setHelpProvider(string HelpNamespace, string Keyword)
        {
            this.helpProvider.HelpNamespace = HelpNamespace;
            this.helpProvider.SetHelpNavigator(this, HelpNavigator.KeywordIndex);
            this.helpProvider.SetHelpKeyword(this, Keyword);
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        #endregion

        #region Interface

        public string IdentifierType()
        {
            if (this.comboBoxType.SelectedItem == null)
                return "";
            else
                return this.comboBoxType.SelectedItem.ToString();
        }

        public string Identifier()
        {
            if (this._WorkbenchUnit.Length > 0)// || this._TypeOfIdentifier == DiversityWorkbench.ReferencingTable.IdentifierType.Regulation)
                return this.userControlModuleRelatedEntryRestriction.textBoxValue.Text;
            else
                return this.textBoxID.Text;
        }

        public string URL()
        {
            if (this._WorkbenchUnit.Length > 0)
                return this.userControlModuleRelatedEntryRestriction.labelURI.Text;
            //else if (this._TypeOfIdentifier == DiversityWorkbench.ReferencingTable.IdentifierType.Regulation && this.userControlModuleRelatedEntryRestriction.labelURI.Text.Length > 0)
            //    return this.userControlModuleRelatedEntryRestriction.labelURI.Text;
            else
                return this.textBoxURL.Text;
        }

        public string Notes()
        {
            return this.textBoxNotes.Text;
        }

        private bool _IsDeleted = false;
        public bool IsDeleted()
        {
            return this._IsDeleted;
        }
        
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            this._IsDeleted = true;
            this.textBoxID.Text = "";
            this.Close();
        }

        private void buttonSetURL_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxURL.Text);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                this.textBoxURL.Text = f.URL;
        }

        #endregion

    }
}
