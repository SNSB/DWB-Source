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
    public partial class FormSynchronisation : Form
    {
        #region Paramter

        private System.Collections.Generic.List<int> _CollectionSpecimenIDList;
        public enum SynchronisationTransfer { Data, Data_and_related_informations, Management, Definitions, Entity }

        private System.Collections.Generic.List<string> _DataTargets;
        public enum SynchronisationTargets { Server, Local_database, Mobile_device, Undefined }

        private System.Data.SqlServerCe.SqlCeConnection _SqlCeConnectionSource;
        private System.Data.SqlServerCe.SqlCeConnection _SqlCeConnectionDestination;
        private System.Data.SqlClient.SqlConnection _SqlConnectionSource;
        private System.Data.SqlClient.SqlConnection _SqlConnectionDestination;

        //public System.Data.SqlServerCe.SqlCeConnection SqlCeConnectionSource
        //{
        //    get { return _SqlCeConnectionSource; }
        //    set { _SqlCeConnectionSource = value; }
        //}

        //public System.Data.SqlServerCe.SqlCeConnection SqlCeConnectionDestination
        //{
        //    get { return _SqlCeConnectionDestination; }+
        //    set { _SqlCeConnectionDestination = value; }
        //}

        //public System.Data.SqlClient.SqlConnection SqlConnectionSource
        //{
        //    get { return _SqlConnectionSource; }
        //    set { _SqlConnectionSource = value; }
        //}

        //public System.Data.SqlClient.SqlConnection SqlConnectionDestination
        //{
        //    get { return _SqlConnectionDestination; }
        //    set { _SqlConnectionDestination = value; }
        //}

        private readonly string _ConnectToSource = "connect to source";
        private readonly string _ConnectToDestination = "connect to destination";
        private readonly string _SpecifyTransferedData = "specify transfered data";

        System.Data.DataTable _DtData;
        System.Data.DataTable _DtManagement;
        System.Data.DataTable _DtDefinitions;
        System.Data.DataTable _DtEntity;

        #endregion

        #region Properties

        public System.Data.SqlServerCe.SqlCeConnection SqlCeConnectionSource
        {
            get 
            {
                if (DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType != SynchronisationTargets.Mobile_device.ToString())
                    return null;
                if (DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString.Length > 0
                    && DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType == SynchronisationTargets.Mobile_device.ToString())
                {
                    if (this._SqlCeConnectionSource == null)
                        this._SqlCeConnectionSource = new System.Data.SqlServerCe.SqlCeConnection(DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString);
                    else
                        if (this._SqlCeConnectionSource.ConnectionString != DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString)
                            this._SqlCeConnectionSource = new System.Data.SqlServerCe.SqlCeConnection(DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString);
                }
                return _SqlCeConnectionSource; 
            }
        }

        public System.Data.SqlServerCe.SqlCeConnection SqlCeConnectionDestination
        {
            get
            {
                if (DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType != SynchronisationTargets.Mobile_device.ToString())
                    return null;
                if (DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString.Length > 0
                    && DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType == SynchronisationTargets.Mobile_device.ToString())
                {
                    if (this._SqlCeConnectionDestination == null)
                        this._SqlCeConnectionDestination = new System.Data.SqlServerCe.SqlCeConnection(DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString);
                    else
                        if (this._SqlCeConnectionDestination.ConnectionString != DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString)
                            this._SqlCeConnectionDestination = new System.Data.SqlServerCe.SqlCeConnection(DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString);
                }
                return _SqlCeConnectionDestination;
            }
        }

        public System.Data.SqlClient.SqlConnection SqlConnectionSource
        {
            get
            {
                if (DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType == SynchronisationTargets.Mobile_device.ToString())
                    return null;
                if (DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString.Length > 0
                    && DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType != SynchronisationTargets.Undefined.ToString())
                {
                    if (this._SqlConnectionSource == null)
                        this._SqlConnectionSource = new System.Data.SqlClient.SqlConnection(DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString);
                    else
                        if (this._SqlConnectionSource.ConnectionString != DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString)
                            this._SqlConnectionSource = new System.Data.SqlClient.SqlConnection(DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString);
                }
                return _SqlConnectionSource;
            }
        }

        public System.Data.SqlClient.SqlConnection SqlConnectionDestination
        {
            get
            {
                if (DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType == SynchronisationTargets.Mobile_device.ToString())
                    return null;
                if (DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString.Length > 0
                    && DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType != SynchronisationTargets.Undefined.ToString())
                {
                    if (this._SqlConnectionDestination == null)
                        this._SqlConnectionDestination = new System.Data.SqlClient.SqlConnection(DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString);
                    else
                        if (this._SqlConnectionDestination.ConnectionString != DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString)
                            this._SqlConnectionDestination = new System.Data.SqlClient.SqlConnection(DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString);
                }
                return _SqlConnectionDestination;
            }
        }

        public System.Collections.Generic.List<string> TransferTypes
        {
            get
            {
                System.Collections.Generic.List<string> L = new List<string>();
                L.Add(SynchronisationTransfer.Data.ToString());
                L.Add(SynchronisationTransfer.Definitions.ToString());
                L.Add(SynchronisationTransfer.Entity.ToString());
                L.Add(SynchronisationTransfer.Management.ToString());
                return L;
            }
        }

        public System.Collections.Generic.List<string> DataTargets
        {
            get 
            {
                if (this._DataTargets == null)
                {
                    this._DataTargets = new List<string>();
                    this._DataTargets.Add(SynchronisationTargets.Server.ToString());
                    this._DataTargets.Add(SynchronisationTargets.Local_database.ToString());
                    this._DataTargets.Add(SynchronisationTargets.Mobile_device.ToString());
                }
                return _DataTargets; 
            }
            //set { _DataTargets = value; }
        }

        public SynchronisationTransfer TransferType
        {
            get
            {
                if (this.comboBoxTransfer.Text == SynchronisationTransfer.Data.ToString())
                    return SynchronisationTransfer.Data;
                else if (this.comboBoxTransfer.Text == SynchronisationTransfer.Definitions.ToString())
                    return SynchronisationTransfer.Definitions;
                else if (this.comboBoxTransfer.Text == SynchronisationTransfer.Entity.ToString())
                    return SynchronisationTransfer.Entity;
                else if (this.comboBoxTransfer.Text == SynchronisationTransfer.Management.ToString())
                    return SynchronisationTransfer.Management;
                else
                    return SynchronisationTransfer.Data;
            }
            set
            {
                for (int i = 0; i < this.comboBoxTransfer.Items.Count; i++)
                {
                    if (this.comboBoxTransfer.Items[i].ToString() == value.ToString())
                    {
                        this.comboBoxTransfer.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private SynchronisationTargets SourceType
        {
            get
            {
                if (DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType.Length > 0)
                {
                    if (DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType == SynchronisationTargets.Server.ToString())
                        return SynchronisationTargets.Server;
                    else if (DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType == SynchronisationTargets.Local_database.ToString())
                        return SynchronisationTargets.Local_database;
                    else if (DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType == SynchronisationTargets.Mobile_device.ToString())
                        return SynchronisationTargets.Mobile_device;
                    else return SynchronisationTargets.Undefined;
                }
                else
                {
                    DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType = this.comboBoxSource.Text;
                    if (this.comboBoxSource.Text == SynchronisationTargets.Server.ToString())
                        return SynchronisationTargets.Server;
                    else if (this.comboBoxSource.Text == SynchronisationTargets.Local_database.ToString())
                        return SynchronisationTargets.Local_database;
                    else if (this.comboBoxSource.Text == SynchronisationTargets.Mobile_device.ToString())
                        return SynchronisationTargets.Mobile_device;
                    else return SynchronisationTargets.Undefined;
                }
            }
        }

        private SynchronisationTargets DestinationType
        {
            get
            {
                if (DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType.Length > 0)
                {
                    if (DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType == SynchronisationTargets.Server.ToString())
                        return SynchronisationTargets.Server;
                    else if (DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType == SynchronisationTargets.Local_database.ToString())
                        return SynchronisationTargets.Local_database;
                    else if (DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType == SynchronisationTargets.Mobile_device.ToString())
                        return SynchronisationTargets.Mobile_device;
                    else return SynchronisationTargets.Undefined;
                }
                else
                {
                    DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType = this.comboBoxDestination.Text;
                    if (this.comboBoxDestination.Text == SynchronisationTargets.Server.ToString())
                        return SynchronisationTargets.Server;
                    else if (this.comboBoxDestination.Text == SynchronisationTargets.Local_database.ToString())
                        return SynchronisationTargets.Local_database;
                    else if (this.comboBoxDestination.Text == SynchronisationTargets.Mobile_device.ToString())
                        return SynchronisationTargets.Mobile_device;
                    else return SynchronisationTargets.Undefined;
                }
            }
        }

        private string ConnectionStringSource
        {
            get
            {
                if (this.SourceType == SynchronisationTargets.Mobile_device)
                {
                    if (this.SqlCeConnectionSource != null)
                        return this.SqlCeConnectionSource.ConnectionString;
                }
                else
                {
                    if (this.SqlConnectionSource != null)
                        return this.SqlConnectionSource.ConnectionString;
                }
                return "";
            }
        }

        private string ConnectionStringDestination
        {
            get
            {
                if (this.DestinationType == SynchronisationTargets.Mobile_device)
                {
                    if (this.SqlCeConnectionDestination != null)
                        return this.SqlCeConnectionDestination.ConnectionString;
                }
                else
                {
                    if (this.SqlConnectionDestination != null)
                    return this.SqlConnectionDestination.ConnectionString;
                }
                return "";
            }
        }

        private string SqlCollectionSpecimenIDRestriction
        {
            get
            {
                string SQL = "";
                for (int i = 0; i < this._CollectionSpecimenIDList.Count; i++)
                {
                    if (i > 0) SQL += ",";
                    SQL += this._CollectionSpecimenIDList[i].ToString();
                }
                return SQL;
            }
        }

        private bool RestrictToRelatedData
        {
            get
            {
                return false;
                if (this.checkBoxRestrictToRelatedData.Visible && this.checkBoxRestrictToRelatedData.Checked)
                    return true;
                else return false;
            }
        }

        private bool RestrictToProject
        {
            get
            {
                if (this.checkBoxRestrictToProject.Visible && this.checkBoxRestrictToProject.Checked)
                    return true;
                else return false;
            }
        }

        private int? RestrictionProject
        {
            get
            {
                if (this.comboBoxProject.Visible == false
                    || this.comboBoxProject.SelectedIndex == -1
                    || this.comboBoxProject.Text.Length == 0)
                    return null;
                else
                {
                    int i = 0;
                    if (int.TryParse(this.comboBoxProject.SelectedValue.ToString(), out i))
                        return i;
                    else return null;
                }
            }
        }

        #endregion

        #region Construction and form

        public FormSynchronisation(System.Collections.Generic.List<int> CollectionSpecimenIDList)
        {
            InitializeComponent();
            this._CollectionSpecimenIDList = CollectionSpecimenIDList;
            this.initForm();
        }

        public FormSynchronisation(
            System.Collections.Generic.List<int> CollectionSpecimenIDList,
            SynchronisationTargets SynchronisationSource,
            SynchronisationTargets SynchronisationDestination,
            SynchronisationTransfer TransferedDataType) : this(CollectionSpecimenIDList)
        {
            this.TransferType = TransferedDataType;
            DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType = SynchronisationSource.ToString();
            DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType = SynchronisationDestination.ToString();
            this.labelDestination.Visible = false;
            this.labelSource.Visible = false;
            this.comboBoxDestination.Visible = false;
            this.comboBoxSource.Visible = false;
            this.comboBoxTransfer.Visible = false;
            this.buttonReverseDirection.Visible = false;
            this.labelWhatIsToBeDone.Visible = false;
            this.labelTransferedData.Visible = false;
            this.buttonTransfer_Click(null, null);
            if (this._CollectionSpecimenIDList.Count == 0 &&
                this.TransferType != SynchronisationTransfer.Entity &&
                this.TransferType != SynchronisationTransfer.Definitions &&
                this.TransferType != SynchronisationTransfer.Management)
            {
                System.Windows.Forms.MessageBox.Show("No data selected");
                this.Close();
            }
        }

        private void initForm()
        {
            foreach (string s in this.DataTargets)
            {
                this.comboBoxDestination.Items.Add(s);
                this.comboBoxSource.Items.Add(s);
            }
            foreach (string s in this.TransferTypes)
                this.comboBoxTransfer.Items.Add(s);

            if (this._CollectionSpecimenIDList.Count > 0)
            {
                foreach (int i in this._CollectionSpecimenIDList)
                    this.listBoxSpecimen.Items.Add(i.ToString());
            }
            this.initSpecimenList();
            try
            {
                string ConnectionString = "";
                // Source
                if (DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType.Length > 0
                    && DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType != SynchronisationTargets.Undefined.ToString())
                {
                    string Source = DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType;
                    ConnectionString = DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString;
                    for (int i = 0; i < this.comboBoxSource.Items.Count; i++)
                    {
                        if (this.comboBoxSource.Items[i].ToString() == DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType)
                        {
                            this.comboBoxSource.SelectedIndex = i;
                            break;
                        }
                    }
                    // Connectionstring may have been deleted after change of the index in die columBox
                    if (DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString != ConnectionString)
                        DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString = ConnectionString;

                    //if (DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType != Source)
                    //    DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType = Source;
                    //if (DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType.Length > 0
                    //    && DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType != _Targets.Undefined.ToString()
                    //    && DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString.Length > 0)
                    //{
                    //    if (DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType == DiversityCollection.Forms.FormSynchronisation._Targets.Mobile_device.ToString())
                    //        this._SqlCeConnectionSource = new System.Data.SqlServerCe.SqlCeConnection(DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString);
                    //    else
                    //        this._SqlConnectionSource = new System.Data.SqlClient.SqlConnection(DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString);
                    //}
                }

                // Destination
                if (DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType.Length > 0
                    && DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType != SynchronisationTargets.Undefined.ToString())
                {
                    string Destination = DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType;
                    ConnectionString = DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString;
                    for (int i = 0; i < this.comboBoxDestination.Items.Count; i++)
                    {
                        if (this.comboBoxDestination.Items[i].ToString() == DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType)
                        {
                            this.comboBoxDestination.SelectedIndex = i;
                            break;
                        }
                    }
                    //if (DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType != Destination)
                    //    DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType = Destination;

                    // Connectionstring may have been deleted after change of the index in die columBox
                    if (DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString != ConnectionString)
                        DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString = ConnectionString;
                    
                    //if (DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType.Length > 0
                    //    && DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType != _Targets.Undefined.ToString()
                    //    && DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString.Length > 0)
                    //{
                    //    if (DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType == DiversityCollection.Forms.FormSynchronisation._Targets.Mobile_device.ToString())
                    //        this._SqlCeConnectionDestination = new System.Data.SqlServerCe.SqlCeConnection(DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString);
                    //    else this._SqlConnectionDestination = new System.Data.SqlClient.SqlConnection(DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString);
                    //}
                }
                this.setControls();
            }
            catch (System.Exception ex) { }
        }

        private void initSpecimenList()
        {
            if (this.SqlCollectionSpecimenIDRestriction.Length > 0)
            {
                string SQL = "SELECT CASE WHEN AccessionNumber IS NULL OR " +
                   "AccessionNumber = '' THEN ' [ID: ' + CAST(CollectionSpecimenID AS varchar) + ']' ELSE AccessionNumber END AS DisplayText " +
                   "FROM  CollectionSpecimen WHERE CollectionSpecimenID IN (" + this.SqlCollectionSpecimenIDRestriction + ") " +
                   "ORDER BY DisplayText";
                System.Data.DataTable dt = new DataTable();
                System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                this.listBoxSpecimen.DataSource = dt;
                this.listBoxSpecimen.DisplayMember = "DisplayText";
            }
        }

        private void FormSynchronisation_FormClosing(object sender, FormClosingEventArgs e)
        {
            DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType = this.DestinationType.ToString();
            if (this.DestinationType == SynchronisationTargets.Mobile_device)
            {
                if (this._SqlCeConnectionDestination != null)
                    DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString = this._SqlCeConnectionDestination.ConnectionString;
            }
            else
            {
                if(this._SqlConnectionDestination != null)
                    DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString = this._SqlConnectionDestination.ConnectionString;
            }

            DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType = this.SourceType.ToString();
            if (this.SourceType == SynchronisationTargets.Mobile_device)
            {
                if (this._SqlCeConnectionSource != null)
                    DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString = this._SqlCeConnectionSource.ConnectionString;
            }
            else
            {
                if (this._SqlConnectionSource != null)
                    DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString = this._SqlConnectionSource.ConnectionString;
            }
            DiversityCollection.Forms.FormSynchronisationSettings.Default.Save();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            this.buttonTransfer.Enabled = true;
            foreach (System.Windows.Forms.Control C in this.panelTableList.Controls)
            {
                DiversityWorkbench.UserControls.UserControlReplicateTable U = (DiversityWorkbench.UserControls.UserControlReplicateTable)C;
                //U.setState(DiversityWorkbench.UserControls.UserControlReplicateTable.StateOfReplication.Start);
            }
        }

        #endregion

        #region Controls

        private void setControls()
        {
            try
            {
                this.setSourceControls();
                this.setDestinationControls();
                this.setTransferContols();
                this.setInformationControls();

                //// Source controls
                //if (this.comboBoxSource.SelectedIndex != -1
                //    && DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType.Length > 0
                //    && DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType != SynchronisationTargets.Undefined.ToString())
                //{
                //    this.comboBoxSource.BackColor = System.Drawing.SystemColors.Window;
                //    this.labelSourceConnection.Visible = true;
                //    this.textBoxSourceConnection.Visible = true;
                //    if (this.ConnectionStringSource.Length > 0)
                //    {
                //        if (DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType == SynchronisationTargets.Mobile_device.ToString())
                //        {
                //            if (this.SqlCeConnectionSource != null)
                //            {
                //                this.textBoxSourceConnection.BackColor = System.Drawing.SystemColors.Control;
                //                this.textBoxSourceConnection.Text = this.SqlCeConnectionSource.DataSource;
                //            }
                //        }
                //        else
                //        {
                //            if (this.SqlConnectionSource != null)
                //            {
                //                this.textBoxSourceConnection.BackColor = System.Drawing.SystemColors.Control;
                //                this.textBoxSourceConnection.Text = "Server: " + this.SqlConnectionSource.DataSource + "\r\n";
                //                this.textBoxSourceConnection.Text += "Database: " + this.SqlConnectionSource.Database;
                //            }
                //        }
                //    }
                //    this.buttonSource.Visible = true;
                //    this.buttonSource.ImageIndex = this.comboBoxSource.SelectedIndex;
                //}
                //else
                //{
                //    this.textBoxSourceConnection.Visible = false;
                //    this.labelSourceConnection.Visible = false;
                //    this.buttonSource.Visible = false;
                //    this.comboBoxSource.BackColor = System.Drawing.Color.Pink;
                //}


                //// Specimen list
                //if (this._CollectionSpecimenIDList.Count > 0 &&
                //    this.comboBoxTransfer.Text == SynchronisationTransfer.Data.ToString() &&
                //    this.SqlConnectionSource != null &&
                //    this.SqlConnectionSource.ConnectionString == DiversityWorkbench.Settings.ConnectionString)
                //    this.listBoxSpecimen.Visible = true;
                //else
                //    this.listBoxSpecimen.Visible = false;


                //if (this.comboBoxDestination.SelectedIndex != -1
                //    && DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType.Length > 0
                //    && DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType != SynchronisationTargets.Undefined.ToString())
                //{
                //    this.comboBoxDestination.BackColor = System.Drawing.SystemColors.Window;
                //    this.labelDestinationConnection.Visible = true;
                //    this.textBoxDestinationConnection.Visible = true;
                //    if (this.ConnectionStringDestination.Length > 0)
                //    {
                //        if (DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType == SynchronisationTargets.Mobile_device.ToString())
                //        {
                //            if (this.SqlCeConnectionDestination != null)
                //            {
                //                this.textBoxDestinationConnection.BackColor = System.Drawing.SystemColors.Control;
                //                this.textBoxDestinationConnection.Text = this.SqlCeConnectionDestination.DataSource;
                //            }
                //        }
                //        else
                //        {
                //            if (this.SqlConnectionDestination != null)
                //            {
                //                this.textBoxDestinationConnection.BackColor = System.Drawing.SystemColors.Control;
                //                this.textBoxDestinationConnection.Text = "Server: " + this.SqlConnectionDestination.DataSource + "\r\n";
                //                this.textBoxDestinationConnection.Text += "Database: " + this.SqlConnectionDestination.Database;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        this.textBoxDestinationConnection.BackColor = System.Drawing.Color.Pink;
                //        this.textBoxDestinationConnection.Text = "Please " + this._ConnectToDestination;
                //    }
                //    this.buttonDestination.Visible = true;
                //    this.buttonDestination.ImageIndex = this.comboBoxDestination.SelectedIndex;
                //}
                //else
                //{
                //    this.textBoxDestinationConnection.Visible = false;
                //    this.labelDestinationConnection.Visible = false;
                //    this.buttonDestination.Visible = false;
                //    this.comboBoxDestination.BackColor = System.Drawing.Color.Pink;
                //}





                //// Transfer controls
                //if (this.ConnectionStringSource.Length > 0
                //    && this.ConnectionStringDestination.Length > 0
                //    && this.comboBoxTransfer.SelectedIndex > -1)
                //    this.buttonTransfer.Visible = true;
                //else
                //    this.buttonTransfer.Visible = false;



                //this.buttonTransfer.Tag = "List";
                //this.buttonTransfer.Text = "List tables";
                //this.buttonTransfer.ImageIndex = 4;
                //this.panelTableList.Controls.Clear();

                //// Setting the header label
                //string WhatIsToBeDone = "";
                //if (this.textBoxSourceConnection.Tag == null)
                //    WhatIsToBeDone += this._ConnectToSource;
                //if (this.comboBoxTransfer.SelectedIndex == -1)
                //{
                //    if (WhatIsToBeDone.Length > 0) WhatIsToBeDone += ", please ";
                //    WhatIsToBeDone += this._SpecifyTransferedData;
                //}
                //if (this.textBoxDestinationConnection.Tag == null)
                //{
                //    if (WhatIsToBeDone.Length > 0) WhatIsToBeDone += ", please ";
                //    WhatIsToBeDone += this._ConnectToDestination;
                //}
                //if (WhatIsToBeDone.Length > 0)
                //{
                //    WhatIsToBeDone = "Please " + WhatIsToBeDone;
                //    this.labelWhatIsToBeDone.Text = WhatIsToBeDone;
                //    this.labelWhatIsToBeDone.Visible = true;
                //}
                //else
                //    this.labelWhatIsToBeDone.Visible = false;
            }
            catch (System.Exception ex) { }
        }

        private void setSourceControls()
        {
            if (this.comboBoxSource.SelectedIndex != -1
                && DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType.Length > 0
                && DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType != SynchronisationTargets.Undefined.ToString())
            {
                this.comboBoxSource.BackColor = System.Drawing.SystemColors.Window;
                this.labelSourceConnection.Visible = true;
                this.textBoxSourceConnection.Visible = true;
                if (this.ConnectionStringSource.Length > 0)
                {
                    if (DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType == SynchronisationTargets.Mobile_device.ToString())
                    {
                        if (this.SqlCeConnectionSource != null)
                        {
                            this.textBoxSourceConnection.BackColor = System.Drawing.SystemColors.Control;
                            this.textBoxSourceConnection.Text = this.SqlCeConnectionSource.DataSource;
                        }
                    }
                    else
                    {
                        if (this.SqlConnectionSource != null)
                        {
                            this.textBoxSourceConnection.BackColor = System.Drawing.SystemColors.Control;
                            this.textBoxSourceConnection.Text = "Server: " + this.SqlConnectionSource.DataSource + "\r\n";
                            this.textBoxSourceConnection.Text += "Database: " + this.SqlConnectionSource.Database;
                        }
                    }
                }
                this.buttonSource.Visible = true;
                this.buttonSource.ImageIndex = this.comboBoxSource.SelectedIndex;
            }
            else
            {
                this.textBoxSourceConnection.Visible = false;
                this.labelSourceConnection.Visible = false;
                this.buttonSource.Visible = false;
                this.comboBoxSource.BackColor = System.Drawing.Color.Pink;
            }
            if (this._CollectionSpecimenIDList.Count > 0 &&
                this.comboBoxTransfer.Text == SynchronisationTransfer.Data.ToString() &&
                this.SqlConnectionSource != null &&
                this.SqlConnectionSource.ConnectionString == DiversityWorkbench.Settings.ConnectionString)
                this.listBoxSpecimen.Visible = true;
            else
                this.listBoxSpecimen.Visible = false;

        
        }
        
        private void setDestinationControls()
        {
            if (this.comboBoxDestination.SelectedIndex != -1
                && DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType.Length > 0
                && DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType != SynchronisationTargets.Undefined.ToString())
            {
                this.comboBoxDestination.BackColor = System.Drawing.SystemColors.Window;
                this.labelDestinationConnection.Visible = true;
                this.textBoxDestinationConnection.Visible = true;
                if (this.ConnectionStringDestination.Length > 0)
                {
                    if (DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType == SynchronisationTargets.Mobile_device.ToString())
                    {
                        if (this.SqlCeConnectionDestination != null)
                        {
                            this.textBoxDestinationConnection.BackColor = System.Drawing.SystemColors.Control;
                            this.textBoxDestinationConnection.Text = this.SqlCeConnectionDestination.DataSource;
                        }
                    }
                    else
                    {
                        if (this.SqlConnectionDestination != null)
                        {
                            this.textBoxDestinationConnection.BackColor = System.Drawing.SystemColors.Control;
                            this.textBoxDestinationConnection.Text = "Server: " + this.SqlConnectionDestination.DataSource + "\r\n";
                            this.textBoxDestinationConnection.Text += "Database: " + this.SqlConnectionDestination.Database;
                        }
                    }
                }
                else
                {
                    this.textBoxDestinationConnection.BackColor = System.Drawing.Color.Pink;
                    this.textBoxDestinationConnection.Text = "Please " + this._ConnectToDestination;
                }
                this.buttonDestination.Visible = true;
                this.buttonDestination.ImageIndex = this.comboBoxDestination.SelectedIndex;
            }
            else
            {
                this.textBoxDestinationConnection.Visible = false;
                this.labelDestinationConnection.Visible = false;
                this.buttonDestination.Visible = false;
                this.comboBoxDestination.BackColor = System.Drawing.Color.Pink;
            }
        
        }

        private void setTransferContols()
        {
            if (this.ConnectionStringSource.Length > 0
            && this.ConnectionStringDestination.Length > 0
            && this.comboBoxTransfer.SelectedIndex > -1)
                this.buttonTransfer.Visible = true;
            else
                this.buttonTransfer.Visible = false;

            if (this.buttonTransfer.Tag == null)
            {
                this.buttonTransfer.Tag = "List";
                this.buttonTransfer.Text = "List tables";
                this.buttonTransfer.ImageIndex = 4;
                this.panelTableList.Controls.Clear();
            }
        }

        private void setInformationControls()
        {
            string WhatIsToBeDone = "";
            if (DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString.Length  == 0)
                WhatIsToBeDone += this._ConnectToSource;
            if (this.comboBoxTransfer.SelectedIndex == -1)
            {
                if (WhatIsToBeDone.Length > 0) WhatIsToBeDone += ", please ";
                WhatIsToBeDone += this._SpecifyTransferedData;
            }
            if (DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString.Length == 0)
            {
                if (WhatIsToBeDone.Length > 0) WhatIsToBeDone += ", please ";
                WhatIsToBeDone += this._ConnectToDestination;
            }
            if (WhatIsToBeDone.Length > 0)
            {
                WhatIsToBeDone = "Please " + WhatIsToBeDone;
                this.labelWhatIsToBeDone.Text = WhatIsToBeDone;
                this.labelWhatIsToBeDone.Visible = true;
            }
            else
                this.labelWhatIsToBeDone.Visible = false;
        }

        private void buttonReverseDirection_Click(object sender, EventArgs e)
        {
            string Source = DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString;
            string Destination = DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString;
            try
            {
                if (Source.Length > 0 &&
                    Destination.Length > 0 &&
                    this.comboBoxSource.SelectedIndex > -1 &&
                    this.comboBoxDestination.SelectedIndex > -1)
                {
                    int SourceIndex = this.comboBoxSource.SelectedIndex;
                    int DestinationIndex = this.comboBoxDestination.SelectedIndex;
                    this.comboBoxDestination.SelectedIndex = SourceIndex;
                    this.comboBoxSource.SelectedIndex = DestinationIndex;
                    DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString = Destination;
                    DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString = Source;
                    this.setControls();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Connections not complete");
                }
            }
            catch { }
        }

        private string GetConnectionString(string TargetType)
        {
            string ConnectionString = "";
            if (TargetType == SynchronisationTargets.Mobile_device.ToString())
            {
                this.openFileDialogCompactDB = new OpenFileDialog();
                this.openFileDialogCompactDB.RestoreDirectory = true;
                this.openFileDialogCompactDB.Multiselect = false;
                this.openFileDialogCompactDB.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                this.openFileDialogCompactDB.Filter = "SDF|*.sdf";
                this.openFileDialogCompactDB.ShowDialog();
                if (this.openFileDialogCompactDB.FileName.Length > 0)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialogCompactDB.FileName);
                    string File = f.FullName;
                    ConnectionString = "Data Source=" + File + ";Persist Security Info=False;";
                }
            }
            else
            {
                DiversityWorkbench.ServerConnection S = new DiversityWorkbench.ServerConnection();
                if (TargetType == SynchronisationTargets.Local_database.ToString())
                    S.DatabaseServer = "127.0.0.1";
                else
                    S.DatabaseServer = DiversityWorkbench.Settings.DatabaseServer;
                S.DatabaseServerPort = DiversityWorkbench.Settings.DatabasePort;
                S.DatabaseName = DiversityWorkbench.Settings.DatabaseName;
                S.DatabaseUser = DiversityWorkbench.Settings.DatabaseUser;
                S.DatabasePassword = DiversityWorkbench.Settings.Password;
                S.IsTrustedConnection = DiversityWorkbench.Settings.IsTrustedConnection;
                S.ModuleName = DiversityWorkbench.Settings.ModuleName;
                DiversityWorkbench.FormDatabaseConnection f = new DiversityWorkbench.FormDatabaseConnection(S);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK
                    && f.ServerConnection.ConnectionString.Length > 0)
                {
                    ConnectionString = f.ServerConnection.ConnectionString;
                }
            }
            return ConnectionString;
            //this.setControls();
        }

        //private void SetConnection(System.Windows.Forms.TextBox TextBox,
        //    System.Windows.Forms.ComboBox ComboBox,
        //    ref System.Data.SqlServerCe.SqlCeConnection SqlCeConnection,
        //    ref System.Data.SqlClient.SqlConnection SqlConnection)
        //{
        //    TextBox.BackColor = System.Drawing.SystemColors.Control;
        //    if (ComboBox.Text == _Targets.Mobile_device.ToString().Replace('_', ' '))
        //    {
        //        if (SqlCeConnection == null || SqlCeConnection.ConnectionString.Length == 0)
        //        {
        //            this.openFileDialogCompactDB = new OpenFileDialog();
        //            this.openFileDialogCompactDB.RestoreDirectory = true;
        //            this.openFileDialogCompactDB.Multiselect = false;
        //            this.openFileDialogCompactDB.InitialDirectory = System.Windows.Forms.Application.StartupPath;
        //            this.openFileDialogCompactDB.Filter = "SDF|*.sdf";
        //            this.openFileDialogCompactDB.ShowDialog();
        //            if (this.openFileDialogCompactDB.FileName.Length > 0)
        //            {
        //                System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialogCompactDB.FileName);
        //                string File = f.FullName;
        //                string ConnectionString = "Data Source=" + File + ";Persist Security Info=False;";
        //                SqlCeConnection = new System.Data.SqlServerCe.SqlCeConnection(ConnectionString);
        //            }
        //        }
        //        TextBox.Tag = SqlCeConnection.ConnectionString;
        //        TextBox.Text = SqlCeConnection.DataSource;
        //    }
        //    else
        //    {
        //        if (SqlConnection == null || SqlConnection.ConnectionString.Length == 0)
        //        {
        //            DiversityWorkbench.ServerConnection S = new DiversityWorkbench.ServerConnection();
        //            if (ComboBox.Text == _Targets.Local_database.ToString().Replace('_', ' '))
        //                S.DatabaseServer = "127.0.0.1";
        //            else
        //                S.DatabaseServer = DiversityWorkbench.Settings.DatabaseServer;
        //            S.DatabaseServerPort = DiversityWorkbench.Settings.DatabasePort;
        //            S.DatabaseName = DiversityWorkbench.Settings.DatabaseName;
        //            S.DatabaseUser = DiversityWorkbench.Settings.DatabaseUser;
        //            S.DatabasePassword = DiversityWorkbench.Settings.Password;
        //            S.IsTrustedConnection = DiversityWorkbench.Settings.IsTrustedConnection;
        //            S.ModuleName = DiversityWorkbench.Settings.ModuleName;
        //            DiversityWorkbench.FormDatabaseConnection f = new DiversityWorkbench.FormDatabaseConnection(S);
        //            f.ShowDialog();
        //            if (f.DialogResult == DialogResult.OK
        //                && f.ServerConnection.ConnectionString.Length > 0)
        //            {
        //                //TextBox.Tag = f.ServerConnection.ConnectionString;
        //                //TextBox.Text = "Server: " + f.ServerConnection.DatabaseServer + "\r\n"
        //                //    + "Database: " + f.ServerConnection.DatabaseName + "\r\n";
        //                //if (!f.ServerConnection.IsTrustedConnection)
        //                //    TextBox.Text += "User: " + f.ServerConnection.DatabaseUser;
        //                SqlConnection = new System.Data.SqlClient.SqlConnection(f.ServerConnection.ConnectionString);
        //            }
        //        }
        //        TextBox.Tag = SqlConnection.ConnectionString;
        //        TextBox.Text = "Server: " + SqlConnection.DataSource + "\r\n";
        //        TextBox.Text += "Database: " + SqlConnection.Database;
        //    }
        //    this.setControls();
        //}

        //private void ResetConnetion(System.Windows.Forms.TextBox TextBox,
        //    System.Windows.Forms.ComboBox ComboBox,
        //    System.Windows.Forms.Button Button,
        //    ref System.Data.SqlServerCe.SqlCeConnection SqlCeConnection,
        //    ref System.Data.SqlClient.SqlConnection SqlConnection,
        //    string Message)
        //{
        //    ComboBox.BackColor = System.Drawing.SystemColors.Window;
        //    if (ComboBox.SelectedIndex > -1)
        //    {
        //        Button.ImageIndex = ComboBox.SelectedIndex;
        //        Button.Visible = true;
        //    }
        //    TextBox.Text = "Please " + Message;
        //    TextBox.BackColor = System.Drawing.Color.Pink;
        //    TextBox.Tag = null;
        //    SqlConnection = null;
        //    SqlCeConnection = null;

        //    this.setControls();
        //}

        #region Source

        private void comboBoxSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.ResetConnetion(this.textBoxSourceConnection, this.comboBoxSource, this.buttonSource, ref this._SqlCeConnectionSource, ref this._SqlConnectionSource, this._ConnectToSource);
            DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString = "";
            DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType = this.comboBoxSource.SelectedItem.ToString();
            this.setSourceControls();
            this.setInformationControls();
            this.setTransferContols();
            //this.setControls();
        }

        private void buttonSource_Click(object sender, EventArgs e)
        {
            if (DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType.Length > 0 &&
                DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType != SynchronisationTargets.Undefined.ToString())
            {
                DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString = this.GetConnectionString(DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceType);
            }
            this.setSourceControls();
            this.setTransferContols();
            this.setInformationControls();
            //this.setControls();
            //this.SetConnection(this.textBoxSourceConnection, this.comboBoxSource, ref this._SqlCeConnectionSource, ref this._SqlConnectionSource);
            //DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString = this._SqlConnectionSource.ConnectionString;
        }

        #endregion   
     
        #region Transfer

        private void comboBoxTransfer_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBoxTransfer.BackColor = System.Drawing.SystemColors.Window;
            this.checkBoxRestrictToProject.Visible = false;
            this.checkBoxRestrictToRelatedData.Visible = false;
            this.comboBoxProject.Visible = false;
            if (this.comboBoxTransfer.Text == SynchronisationTransfer.Data.ToString())
            {
            }
            else if (this.comboBoxTransfer.Text == SynchronisationTransfer.Management.ToString())
            {
                string SQL = "SELECT Project, ProjectID FROM ProjectList ORDER BY Project";
                System.Data.DataTable dt = new DataTable();
                if (this.SourceType == SynchronisationTargets.Mobile_device)
                {
                    System.Data.SqlServerCe.SqlCeDataAdapter adCE = new System.Data.SqlServerCe.SqlCeDataAdapter(SQL, DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString);
                    adCE.Fill(dt);
                }
                else
                {
                    System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.Forms.FormSynchronisationSettings.Default.SourceConnectionString);
                    ad.Fill(dt);
                }
                this.comboBoxProject.DataSource = dt;
                this.comboBoxProject.DisplayMember = "Project";
                this.comboBoxProject.ValueMember = "ProjectID";
                this.checkBoxRestrictToProject.Visible = true;
                //this.checkBoxRestrictToRelatedData.Visible = true;
                this.comboBoxProject.Visible = true;
            }
            this.setTransferContols();
            this.setInformationControls();
            //this.setControls();
        }

        private void buttonTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.buttonTransfer.Tag.ToString() == "List")
                {
                    this.setReplicationTables();
                    this.buttonTransfer.Tag = "Transfer";
                    this.buttonTransfer.Text = "Transfer data ";
                    this.buttonTransfer.ImageIndex = 3;
                }
                else
                {
                    for (int i = this.panelTableList.Controls.Count - 1; i >= 0; i--)
                    {
                        DiversityWorkbench.UserControls.UserControlReplicateTable U = (DiversityWorkbench.UserControls.UserControlReplicateTable)this.panelTableList.Controls[i];
                        //U.setState(DiversityWorkbench.UserControls.UserControlReplicateTable._State.Waiting);
                        //U.Refresh();
                        U.ReplicateTable();
                        //U.Refresh();
                        //U.setState(DiversityWorkbench.UserControls.UserControlReplicateTable._State.Finished);
                    }
                    this.buttonTransfer.Enabled = false;
                    //foreach (System.Windows.Forms.Control C in this.panelTableList.Controls)
                    //{
                    //    DiversityWorkbench.UserControls.UserControlReplicateTable U = (DiversityWorkbench.UserControls.UserControlReplicateTable)C;
                    //    U.ReplicateTable();
                    //}
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setReplicationTables()
        {
            this.panelTableList.Controls.Clear();
            try
            {
                this.TableListForReplication.AcceptChanges();
                System.Data.DataRow[] RR = this.TableListForReplication.Select("", "ORDINAL_POSITION");
                foreach (System.Data.DataRow R in RR)// this.TableListForReplication.Rows)
                {
                    bool SourceIsCE = false;
                    if (this.SourceType == SynchronisationTargets.Mobile_device) SourceIsCE = true;
                    bool DestinationIsCE = false;
                    if (this.DestinationType == SynchronisationTargets.Mobile_device) DestinationIsCE = true;
                    string SqlRestrictionColumn = "CollectionSpecimenID";
                    string SqlRestrictionIDs = this.SqlCollectionSpecimenIDRestriction;
                    if (R.RowState != DataRowState.Deleted)
                    {
                        if (this.SourceType != SynchronisationTargets.Mobile_device)
                            this.setRestriction(R[0].ToString(), ref SqlRestrictionColumn, ref SqlRestrictionIDs);
                        //DiversityWorkbench.UserControls.UserControlReplicateTable U
                        //    = new DiversityWorkbench.UserControls.UserControlReplicateTable(
                        //        R[0].ToString(),
                        //        SourceIsCE,
                        //        this.ConnectionStringSource,
                        //        DestinationIsCE,
                        //        this.ConnectionStringDestination,
                        //        SqlRestrictionColumn,
                        //        SqlRestrictionIDs);
                        //this.panelTableList.Controls.Add(U);
                        //U.Dock = DockStyle.Top;
                        //U.BringToFront();
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setRestriction(string TableName, ref string ColumnName, ref string SqlRestrictionIDs)
        {
            int i;
            string SQL = "select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C " +
                "where C.TABLE_NAME = '" + TableName + "'" +
                "and C.COLUMN_NAME = '" + ColumnName + "'";
            if (int.TryParse(DiversityWorkbench.FormFunctions.SqlExecuteScalar(SQL), out i))
            {
                if (this.TransferType == SynchronisationTransfer.Management ||
                    this.TransferType == SynchronisationTransfer.Definitions ||
                    this.TransferType == SynchronisationTransfer.Entity)
                {
                    ColumnName = "";
                    SqlRestrictionIDs = "";
                }
                if (i == 1)
                    return;
                else if (i == 0)
                {
                    switch (TableName)
                    {
                        case "CollectionEventImage":
                        case "CollectionEvent":
                        case "CollectionEventLocalisation":
                        case "CollectionEventProperty":
                            ColumnName = "CollectionEventID";
                            SqlRestrictionIDs = "SELECT CollectionEventID FROM CollectionSpecimen WHERE CollectionSpecimenID IN (" + SqlRestrictionIDs + ")";
                            break;
                        case "CollectionEventSeries":
                        case "CollectionEventSeriesImage":
                            ColumnName = "SeriesID";
                            SqlRestrictionIDs = "SELECT SeriesID FROM CollectionEvent WHERE CollectionEventID IN (SELECT CollectionEventID FROM CollectionSpecimen WHERE CollectionSpecimenID IN (" + SqlRestrictionIDs + "))";
                            break;
                        case "Collection":
                        case "CollectionImage":
                            //if (this.RestrictToProject)
                            //{
                            //    ColumnName = "CollectionID";
                            //    SqlRestrictionIDs = "SELECT DISTINCT CollectionID FROM CollectionSpecimenPart WHERE CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionProject WHERE ProjectID ";
                            //    if (this.RestrictionProject == null)
                            //        SqlRestrictionIDs += " IN (SELECT  FROM )";
                            //    else
                            //        SqlRestrictionIDs += " = " + this.RestrictionProject.ToString();
                            //    SqlRestrictionIDs += ")";
                            //}
                            //{
                            //    ColumnName = "CollectionID";
                            //    SqlRestrictionIDs = "SELECT DISTINCT CollectionID FROM CollectionSpecimenPart WHERE CollectionSpecimenID IN (" + SqlRestrictionIDs + ")";
                            //}
                            break;
                        case "CollectionManager":
                            //ColumnName = "LoginName";
                            //SqlRestrictionIDs = "SELECT USER_NAME()";
                            break;
                        case "Processing":
                        case "ProjectProcessing":
                        case "ProcessingMaterialCategory":
                            if (this.RestrictToProject)
                            {
                                ColumnName = "ProcessingID";
                                SqlRestrictionIDs = "SELECT ProcessingID FROM ProcessingProjectList(" + this.RestrictionProject.ToString() + ")";
                            }
                            break;
                        case "Transaction":
                        case "TransactionDocument":
                            //ColumnName = "TransactionID";
                            //SqlRestrictionIDs = "SELECT DISTINCT TransactionID FROM CollectionSpecimenTransaction WHERE CollectionSpecimenID IN (" + SqlRestrictionIDs + ")";
                            //break;
                        case "Analysis":
                        case "AnalysisResult":
                        case "ProjectAnalysis":
                        case "AnalysisTaxonomicGroup":
                            if (this.RestrictToProject && this.RestrictionProject != null)
                            {
                                ColumnName = "AnalysisID";
                                SqlRestrictionIDs = "SELECT AnalysisID FROM AnalysisProjectList(" + this.RestrictionProject.ToString() + ")";
                            }
                            break;
                        case "ProjectProxy":
                        case "ProjectUser":
                            if (this.RestrictToProject)
                            {
                                ColumnName = "ProjectID";
                                SqlRestrictionIDs = this.RestrictionProject.ToString();
                            }
                            break;
                        default:
                            ColumnName = "";
                            SqlRestrictionIDs = "";
                            break;
                    }
                }
                else
                {
                    switch (TableName)
                    {
                        case "CollectionEvent":
                        case "CollectionEventProperty":
                        case "CollectionEventLocalisation":
                        case "CollectionEventImage":
                            ColumnName = "CollectionEventID";
                            SqlRestrictionIDs = "SELECT CollectionEventID FROM CollectionSpecimen WHERE CollectionSpecimenID IN (" + SqlRestrictionIDs + ")";
                            break;
                        case "CollectionEventSeries":
                        case "CollectionEventSeriesImage":
                            ColumnName = "SeriesID";
                            SqlRestrictionIDs = SqlRestrictionIDs.Replace(",", ", ");
                            SqlRestrictionIDs = "SELECT SeriesID FROM  dbo.FirstLinesSeries('" + SqlRestrictionIDs + "')";
                            break;
                        default:
                            ColumnName = "";
                            SqlRestrictionIDs = "";
                            break;
                    }
                }
            }
        }
        
        #endregion

        #region Destination

        private void buttonDestination_Click(object sender, EventArgs e)
        {
            if (DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType.Length > 0 &&
                DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType != SynchronisationTargets.Undefined.ToString())
            {
                DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString = this.GetConnectionString(DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType);
            }
            this.setDestinationControls();
            this.setInformationControls();
            this.setTransferContols();
            //this.setControls();
            //this.SetConnection(this.textBoxDestinationConnection, this.comboBoxDestination, ref this._SqlCeConnectionDestination, ref this._SqlConnectionDestination);
            //this.buttonCleanDestination.Visible = false;
            //this.textBoxDestinationConnection.BackColor = System.Drawing.SystemColors.Control;
            //if (this.comboBoxDestination.Text == _Targets.Mobile_device.ToString().Replace('_', ' '))
            //{
            //    this.openFileDialogCompactDB = new OpenFileDialog();
            //    this.openFileDialogCompactDB.RestoreDirectory = true;
            //    this.openFileDialogCompactDB.Multiselect = false;
            //    this.openFileDialogCompactDB.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            //    this.openFileDialogCompactDB.Filter = "SDF|*.sdf";
            //    this.openFileDialogCompactDB.ShowDialog();
            //    if (this.openFileDialogCompactDB.FileName.Length > 0)
            //    {
            //        System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialogCompactDB.FileName);
            //        string File = f.FullName;
            //        string ConnectionString = "Data Source=" + File + ";Persist Security Info=False;";
            //        this._SqlCeConnectionDestination = new System.Data.SqlServerCe.SqlCeConnection(ConnectionString);
            //        this.textBoxDestinationConnection.Tag = ConnectionString;
            //        this.textBoxDestinationConnection.Text = this._SqlCeConnectionDestination.Database;
            //        this.buttonCleanDestination.Visible = true;
            //    }
            //}
            //else
            //{
            //    DiversityWorkbench.ServerConnection S = new DiversityWorkbench.ServerConnection();
            //    if (this.comboBoxSource.Text == _Targets.Local_database.ToString().Replace('_', ' '))
            //    {
            //        S.DatabaseServer = "127.0.0.1";
            //        this.buttonCleanDestination.Visible = true;
            //    }
            //    else
            //        S.DatabaseServer = DiversityWorkbench.Settings.DatabaseServer;
            //    S.DatabaseServerPort = DiversityWorkbench.Settings.DatabasePort;
            //    S.DatabaseName = DiversityWorkbench.Settings.DatabaseName;
            //    S.DatabaseUser = DiversityWorkbench.Settings.DatabaseUser;
            //    S.DatabasePassword = DiversityWorkbench.Settings.Password;
            //    S.IsTrustedConnection = DiversityWorkbench.Settings.IsTrustedConnection;
            //    S.ModuleName = DiversityWorkbench.Settings.ModuleName;
            //    DiversityWorkbench.FormDatabaseConnection f = new DiversityWorkbench.FormDatabaseConnection(S);
            //    f.ShowDialog();
            //    if (f.DialogResult == DialogResult.OK
            //        && f.ServerConnection.ConnectionString.Length > 0)
            //    {
            //        this.textBoxDestinationConnection.Tag = f.ServerConnection.ConnectionString;
            //        this.textBoxDestinationConnection.Text = "Server: " + f.ServerConnection.DatabaseServer + "\r\n"
            //           + "Database: " + f.ServerConnection.DatabaseName + "\r\n";
            //        if (!f.ServerConnection.IsTrustedConnection)
            //            this.textBoxDestinationConnection.Text += "User: " + f.ServerConnection.DatabaseUser;
            //        this._SqlConnectionDestination = new System.Data.SqlClient.SqlConnection(f.ServerConnection.ConnectionString);
            //    }
            //}
            //this.setControls();
        }

        private void buttonCleanDestination_Click(object sender, EventArgs e)
        {
            this.setControls();
        }

        private void comboBoxDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.ResetConnetion(this.textBoxDestinationConnection, this.comboBoxDestination, this.buttonDestination, ref this._SqlCeConnectionDestination, ref this._SqlConnectionDestination, this._ConnectToDestination);
            DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationConnectionString = "";
            DiversityCollection.Forms.FormSynchronisationSettings.Default.DestinationType = this.comboBoxDestination.SelectedItem.ToString();
            this.setDestinationControls();
            this.setInformationControls();
            //this.setControls();
        }
        
        #endregion

        #endregion

        #region Table lists

        private System.Data.DataTable TableListForReplication
        {
            get
            {
            //TODO - sortieren nach PK
                System.Data.DataTable dtTableSource;
                System.Data.DataTable dtTableDestination;
                System.Data.DataTable dtTables = new DataTable();
                try
                {
                    bool SourceIsCE = false;
                    if (this.SourceType == SynchronisationTargets.Mobile_device) SourceIsCE = true;
                    bool DestinationIsCE = false;
                    if (this.DestinationType == SynchronisationTargets.Mobile_device) DestinationIsCE = true;
                    switch (this.TransferType)
                    {
                        case SynchronisationTransfer.Data:
                            if (SourceIsCE) dtTableSource = this.TableListDataMobile(this.ConnectionStringSource);
                            else dtTableSource = this.TablesData(this.ConnectionStringSource);

                            if (DestinationIsCE) dtTableDestination = this.TableListDataMobile(this.ConnectionStringDestination);
                            else dtTableDestination = this.TablesData(this.ConnectionStringDestination);
                            break;
                        case SynchronisationTransfer.Definitions:

                            if (SourceIsCE) dtTableSource = this.TableListDefinitionsMobile(this.ConnectionStringSource);
                            else dtTableSource = this.TablesDefinitions(this.ConnectionStringSource);

                            if (DestinationIsCE) dtTableDestination = this.TableListDefinitionsMobile(this.ConnectionStringDestination);
                            else dtTableDestination = this.TablesDefinitions(this.ConnectionStringDestination);


                            dtTableSource = this.TablesDefinitions(this.ConnectionStringSource);
                            dtTableDestination = this.TablesDefinitions(this.ConnectionStringDestination);
                            break;
                        case SynchronisationTransfer.Entity:
                            if (SourceIsCE) dtTableSource = this.TableListEntityMobile(this.ConnectionStringSource);
                            else dtTableSource = this.TablesEntity(this.ConnectionStringSource);

                            if (DestinationIsCE) dtTableDestination = this.TableListEntityMobile(this.ConnectionStringDestination);
                            else dtTableDestination = this.TablesEntity(this.ConnectionStringDestination);
                            break;
                        case SynchronisationTransfer.Management:

                            if (SourceIsCE) dtTableSource = this.TableListManagementMobile(this.ConnectionStringSource);
                            else dtTableSource = this.TablesManagement(this.ConnectionStringSource);

                            if (DestinationIsCE) dtTableDestination = this.TableListManagementMobile(this.ConnectionStringDestination);
                            else dtTableDestination = this.TablesManagement(this.ConnectionStringDestination);
                            break;
                        default:
                            dtTableSource = new DataTable();
                            dtTableDestination = new DataTable();
                            break;
                    }
                    dtTables = dtTableDestination.Copy();
                    System.Collections.Generic.List<System.Data.DataRow> RowsToDelete = new List<DataRow>();
                    for (int i = 0; i < dtTables.Rows.Count; i++)
                    {
                        bool SourceContainsTable = false;
                        foreach (System.Data.DataRow R in dtTableSource.Rows)
                        {
                            if (R[0].ToString() == dtTables.Rows[i][0].ToString())
                                SourceContainsTable = true;
                        }
                        if (!SourceContainsTable)
                            RowsToDelete.Add(dtTables.Rows[i]);
                            //dtTables.Rows[i].Delete();
                    }
                    if (RowsToDelete.Count > 0)
                    {
                        foreach (System.Data.DataRow R in RowsToDelete)
                            R.Delete();
                    }
                    dtTables.AcceptChanges();
                    foreach (System.Data.DataRow R in dtTables.Rows)
                    {
                        if (R.ItemArray.Length > 1)
                        {
                            if (R[0].ToString() == "CollectionEventSeries")
                                R[1] = 0;
                            else if (R[0].ToString() == "CollectionSpecimenImage")
                                R[1] = 3;
                            else if (R[0].ToString() == "CollectionAgent")
                                R[1] = 2;
                            else if (R[0].ToString() == "Identification")
                                R[1] = 3;
                        }
                    }
                }
                catch (System.Exception ex) { }
                dtTables.AcceptChanges();
                return dtTables;
            }
        }
        
        private System.Collections.Generic.List<string> TablesDataList(string ConnectionString)
        {
            System.Collections.Generic.List<string> L = new List<string>();
            foreach (System.Data.DataRow R in this.TablesData(ConnectionString).Rows)
                L.Add(R[0].ToString());
            return L;
        }

        private System.Collections.Generic.List<string> TablesDefinitionsList(string ConnectionString)
        {
            System.Collections.Generic.List<string> L = new List<string>();
            foreach (System.Data.DataRow R in this.TablesDefinitions(ConnectionString).Rows)
                L.Add(R[0].ToString());
            return L;
        }

        private System.Collections.Generic.List<string> TablesEntityList(string ConnectionString)
        {
            System.Collections.Generic.List<string> L = new List<string>();
            foreach (System.Data.DataRow R in this.TablesEntity(ConnectionString).Rows)
                L.Add(R[0].ToString());
            return L;
        }

        private System.Collections.Generic.List<string> TablesManagementList(string ConnectionString)
        {
            System.Collections.Generic.List<string> L = new List<string>();
            foreach(System.Data.DataRow R in this.TablesManagement(ConnectionString).Rows)
                L.Add(R[0].ToString());
            return L;
        }

        private System.Data.DataTable TablesData(string ConnectionString)
        {
            if (this._DtData != null
                && this._DtData.Rows.Count > 0) 
                return this._DtData;
            this._DtData = new DataTable();
            string SQL = "SELECT T.TABLE_NAME, MAX(K.ORDINAL_POSITION) AS ORDINAL_POSITION " +
                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                "INFORMATION_SCHEMA.TABLES AS T ON K.TABLE_NAME = T.TABLE_NAME " +
                "WHERE (T.TABLE_TYPE = 'BASE TABLE') " +
                "AND (T.TABLE_SCHEMA = 'dbo') " +
                "AND (T.TABLE_NAME NOT LIKE '%_log') " +
                "AND (T.TABLE_NAME NOT LIKE '%_log_%') " +
                "AND (T.TABLE_NAME NOT LIKE 'xx_%') ";
            if (this.comboBoxTransfer.Text == SynchronisationTransfer.Data.ToString())
            {
                SQL += "AND ((T.TABLE_NAME + '_log') IN " +
                "   (SELECT TABLE_NAME " +
                "   FROM INFORMATION_SCHEMA.TABLES AS T " +
                "   WHERE (TABLE_TYPE = 'BASE TABLE') " +
                "   AND (TABLE_SCHEMA = 'dbo') " +
                "   AND (TABLE_NAME LIKE '%_log'))) ";
                if (this.TablesDefinitions(ConnectionString).Rows.Count > 0)
                {
                    SQL += "AND T.TABLE_NAME NOT IN (";
                    foreach (System.Data.DataRow R in this.TablesDefinitions(ConnectionString).Rows)
                        SQL += "'" + R[0].ToString() + "',";
                    SQL = SQL.Substring(0, SQL.Length - 1) + ")";
                }
                if (this.TablesManagement(ConnectionString).Rows.Count > 0)
                {
                    SQL += "AND T.TABLE_NAME NOT IN (";
                    foreach (System.Data.DataRow R in this.TablesManagement(ConnectionString).Rows)
                        SQL += "'" + R[0].ToString() + "',";
                    SQL = SQL.Substring(0, SQL.Length - 1) + ")";
                }
                if (this.TablesEntity(ConnectionString).Rows.Count > 0)
                {
                    SQL += "AND T.TABLE_NAME NOT IN (";
                    foreach (System.Data.DataRow R in this.TablesEntity(ConnectionString).Rows)
                        SQL += "'" + R[0].ToString() + "',";
                    SQL = SQL.Substring(0, SQL.Length - 1) + ")";
                }
            }
            else if (this.comboBoxTransfer.Text == SynchronisationTransfer.Entity.ToString())
                SQL += "AND (T.TABLE_NAME LIKE 'Entity%') ";
            SQL += "GROUP BY T.TABLE_NAME " +
            "ORDER BY MAX(K.ORDINAL_POSITION), T.TABLE_NAME ";
            try
            {
                System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
                ad.Fill(this._DtData);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return this._DtData;
        }

        private System.Data.DataTable TablesDefinitions(string ConnectionString)
        {
            if (this._DtDefinitions != null) return this._DtDefinitions;
            this._DtDefinitions = new DataTable();
            string SQL = "SELECT T.TABLE_NAME, MAX(K.ORDINAL_POSITION) AS ORDINAL_POSITION " +
                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                "INFORMATION_SCHEMA.TABLES AS T ON K.TABLE_NAME = T.TABLE_NAME " +
                "WHERE (K.CONSTRAINT_NAME LIKE 'PK_%') " +
                "AND (T.TABLE_TYPE = 'BASE TABLE') " +
                "AND (T.TABLE_SCHEMA = 'dbo') " +
                "AND (T.TABLE_NAME NOT LIKE '%_log') " +
                "AND (T.TABLE_NAME NOT LIKE '%_log_%')  " +
                "AND (T.TABLE_NAME NOT LIKE 'xx_%') " +
                "AND (((T.TABLE_NAME LIKE '%_Enum') " +
                "AND NOT (T.TABLE_NAME LIKE 'Entity%')) " +
                "OR T.TABLE_NAME IN ('Property', 'PropertyValueList', 'LocalisationSystem')) " +
                "GROUP BY T.TABLE_NAME " +
                "ORDER BY MAX(K.ORDINAL_POSITION), T.TABLE_NAME ";
            try
            {
                System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
                ad.Fill(this._DtDefinitions);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return this._DtDefinitions;
        }

        private System.Data.DataTable TablesEntity(string ConnectionString)
        {
            if (this._DtEntity != null) return this._DtEntity;
            this._DtEntity = new DataTable();
            string SQL = "SELECT T.TABLE_NAME, MAX(K.ORDINAL_POSITION) AS ORDINAL_POSITION " +
                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                "INFORMATION_SCHEMA.TABLES AS T ON K.TABLE_NAME = T.TABLE_NAME " +
                "WHERE (T.TABLE_TYPE = 'BASE TABLE') " +
                "AND (T.TABLE_SCHEMA = 'dbo') " +
                "AND (T.TABLE_NAME NOT LIKE '%_log') " +
                "AND (T.TABLE_NAME NOT LIKE '%_log_%') " +
                "AND (T.TABLE_NAME NOT LIKE 'xx_%') " +
                "AND (T.TABLE_NAME LIKE 'Entity%') " +
                "GROUP BY T.TABLE_NAME " +
                "ORDER BY MAX(K.ORDINAL_POSITION), T.TABLE_NAME ";
            try
            {
                System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
                ad.Fill(this._DtEntity);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return this._DtEntity;
        }

        private System.Data.DataTable TablesManagement(string ConnectionString)
        {
            if (this._DtManagement != null) return this._DtManagement;
            this._DtManagement = new DataTable();
            string SQL = "SELECT T.TABLE_NAME, MAX(K.ORDINAL_POSITION) AS ORDINAL_POSITION " +
                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                "INFORMATION_SCHEMA.TABLES AS T ON K.TABLE_NAME = T.TABLE_NAME " +
                "WHERE T.TABLE_NAME IN ('Analysis', 'AnalysisResult', 'AnalysisTaxonomicGroup', 'ProjectAnalysis',  " +
                "'Collection', 'CollectionManager', 'CollectionImage', 'Processing', 'ProcessingMaterialCategory', 'ProjectProcessing', 'Transaction', 'TransactionDocument', " +
                "'UserProxy', 'ProjectProxy', 'ProjectUser') " +
                "GROUP BY T.TABLE_NAME " +
                "ORDER BY MAX(K.ORDINAL_POSITION), T.TABLE_NAME ";
            try
            {
                System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
                ad.Fill(this._DtManagement);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return this._DtManagement;
        }

        private System.Data.DataTable TableListMobile(string ConnectionString)
        {
            System.Data.DataTable dtTables = new DataTable();
            System.Data.DataTable dtList = new DataTable();
            string SQL = "SELECT T.TABLE_NAME, K.ORDINAL_POSITION AS ORDINAL_POSITION " +
                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                "INFORMATION_SCHEMA.TABLES AS T ON K.TABLE_NAME = T.TABLE_NAME " +
                "WHERE (K.CONSTRAINT_NAME LIKE 'PK_%') " +
                "ORDER BY K.ORDINAL_POSITION DESC, T.TABLE_NAME";
            try
            {
                System.Data.SqlServerCe.SqlCeDataAdapter ad = new System.Data.SqlServerCe.SqlCeDataAdapter(SQL, ConnectionString);
                ad.Fill(dtList);
                System.Collections.Generic.List<string> TableList = new List<string>();
                foreach (System.Data.DataRow R in dtList.Rows)
                {
                    if (!TableList.Contains(R[0].ToString()))
                        TableList.Add(R[0].ToString());
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return dtTables;
        }

        private System.Data.DataTable TableListDataMobile(string ConnectionString)
        {
            System.Data.DataTable dtList = new DataTable();

            System.Data.DataTable dtTables = new DataTable();
            string SQL = "SELECT TABLE_NAME, 1 AS ORDINAL_POSITION " +
                "FROM INFORMATION_SCHEMA.TABLES AS T " +
                "WHERE (TABLE_TYPE = 'TABLE') ";
            if (this.comboBoxTransfer.Text == SynchronisationTransfer.Data.ToString())
            {
                SQL += "AND TABLE_NAME NOT IN (";
                foreach (System.Data.DataRow R in this.TableListDefinitionsMobile(ConnectionString).Rows)
                    SQL += "'" + R[0].ToString() + "',";
                SQL = SQL.Substring(0, SQL.Length - 1) + ") ";
            }
            SQL += "ORDER BY TABLE_NAME";
            try
            {
                System.Data.SqlServerCe.SqlCeDataAdapter ad = new System.Data.SqlServerCe.SqlCeDataAdapter(SQL, ConnectionString);
                ad.Fill(dtTables);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return dtTables;
        }

        private System.Data.DataTable TableListDefinitionsMobile(string ConnectionString)
        {
            System.Data.DataTable dtTables = new DataTable();
            string SQL = "SELECT TABLE_NAME " +
                "FROM INFORMATION_SCHEMA.TABLES AS T " +
                "WHERE (TABLE_TYPE = 'TABLE') " +
                "AND (TABLE_NAME NOT LIKE '%_log') " +
                "AND (TABLE_NAME NOT LIKE '%_log_%') " +
                "AND (TABLE_NAME NOT LIKE 'xx_%') " +
                "AND (TABLE_NAME NOT LIKE 'Entity%') " +
                "AND (((T.TABLE_NAME LIKE '%_Enum') " +
                "AND NOT (T.TABLE_NAME LIKE 'Entity%')) " +
                "OR TABLE_NAME IN ('Analysis', 'AnalysisResult', 'AnalysisTaxonomicGroup', " +
                "'Collection', 'Processing', 'LocalisationSystem')) " +
                "ORDER BY TABLE_NAME";
            try
            {
                System.Data.SqlServerCe.SqlCeDataAdapter ad = new System.Data.SqlServerCe.SqlCeDataAdapter(SQL, ConnectionString);
                ad.Fill(dtTables);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return dtTables;
        }

        private System.Data.DataTable TableListEntityMobile(string ConnectionString)
        {
            System.Data.DataTable dtTables = new DataTable();
            string SQL = "SELECT TABLE_NAME " +
                "FROM INFORMATION_SCHEMA.TABLES AS T " +
                "WHERE (TABLE_TYPE = 'TABLE') " +
                "AND (TABLE_NAME NOT LIKE '%_log') " +
                "AND (TABLE_NAME NOT LIKE '%_log_%') " +
                "AND (TABLE_NAME NOT LIKE 'xx_%') " +
                "AND (TABLE_NAME LIKE 'Entity%') " +
                "ORDER BY TABLE_NAME";
            try
            {
                System.Data.SqlServerCe.SqlCeDataAdapter ad = new System.Data.SqlServerCe.SqlCeDataAdapter(SQL, ConnectionString);
                ad.Fill(dtTables);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return dtTables;
        }

        private System.Data.DataTable TableListManagementMobile(string ConnectionString)
        {
            System.Data.DataTable dtTables = new DataTable();
            string SQL = "SELECT T.TABLE_NAME, MAX(K.ORDINAL_POSITION) AS ORDINAL_POSITION " +
                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                "INFORMATION_SCHEMA.TABLES AS T ON K.TABLE_NAME = T.TABLE_NAME " +
                "WHERE T.TABLE_NAME IN ('Analysis', 'AnalysisResult', 'AnalysisTaxonomicGroup', 'ProjectAnalysis',  " +
                "'Collection', 'CollectionManager', 'CollectionImage', 'Processing', 'ProcessingMaterialCategory', 'ProjectProcessing', 'Transaction', 'TransactionDocument', " +
                "'UserProxy', 'ProjectProxy', 'ProjectUser') " +
                "GROUP BY T.TABLE_NAME " +
                "ORDER BY MAX(K.ORDINAL_POSITION), T.TABLE_NAME ";
            try
            {
                System.Data.SqlServerCe.SqlCeDataAdapter ad = new System.Data.SqlServerCe.SqlCeDataAdapter(SQL, ConnectionString);
                ad.Fill(dtTables);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return dtTables;
        }

        private void buttonSelectAllTables_Click(object sender, EventArgs e)
        {
            foreach (DiversityWorkbench.UserControls.UserControlReplicateTable R in this.panelTableList.Controls)
            {
                R.TabelIsSelected = true;
            }
        }

        private void buttonSelectNoTable_Click(object sender, EventArgs e)
        {
            foreach (DiversityWorkbench.UserControls.UserControlReplicateTable R in this.panelTableList.Controls)
            {
                R.TabelIsSelected = false;
            }
        }

        #endregion 

    }
}
