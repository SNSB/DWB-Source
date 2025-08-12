using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.CacheDatabase
{
    public partial class UserControlExchangeCongruence : UserControl
    {

        #region Parameter

        private string _ExchangedDatabase = "";
        private string _ReplacementDatabase = "";
        private string _SourceView;
        private DiversityCollection.CacheDatabase.UserControlLookupSource.TypeOfSource _SourceType;
        private int _ProjectID;
        private string _Schema;
        private string _Project;
        private bool _IsCongruent = false;
        //public enum CongruenceStatus { Congruent, Added, DataCountDifferent, DataAreMissing, PackagesDiffering, UpdateNeeded, Missing }
        private ReplaceDatabase.CongruenceStatus _CongruenceStatus;
        private System.Collections.Generic.Dictionary<string, ReplaceDatabase.CongruenceStatus> _PackageCongruence;
        private System.Collections.Generic.Dictionary<string, CongruenceObject> _CongruenceObjects;
        private CongruenceObject _CongruenceObject;

        #endregion

        #region Construction

        public UserControlExchangeCongruence(int ProjectID, string Project, ReplaceDatabase.CongruenceStatus Status, System.Collections.Generic.Dictionary<string, CongruenceObject> CongruenceObjects, string OldDatabase, string NewDatabase)
        {
            InitializeComponent();
            try
            {
                this.groupBoxPackages.Visible = true;
                this._ExchangedDatabase = OldDatabase;
                this._ReplacementDatabase = NewDatabase;
                this._Project = Project;
                this._ProjectID = ProjectID;
                this._Schema = "Project_" + Project;
                this.groupBox.Text = this._Project;
                this.SetCongruenceStatus(Status);
                this._CongruenceObjects = CongruenceObjects;
                this.initPackages();//.initProject();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public UserControlExchangeCongruence(int ProjectID, string Project, ReplaceDatabase.CongruenceStatus Status, System.Collections.Generic.Dictionary<string, ReplaceDatabase.CongruenceStatus> PackageCongruence, string ExchangedDatabase, string ReplacementDatabase)
        {
            InitializeComponent();
            try
            {
                this.groupBoxPackages.Visible = true;
                this._ExchangedDatabase = ExchangedDatabase;
                this._ReplacementDatabase = ReplacementDatabase;
                this._Project = Project;
                this._ProjectID = ProjectID;
                this._Schema = "Project_" + Project;
                this.groupBox.Text = this._Project;
                this.SetCongruenceStatus(Status);
                this._PackageCongruence = PackageCongruence;
                this.initPackages();//.initProject();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public UserControlExchangeCongruence(DiversityCollection.CacheDatabase.UserControlLookupSource.TypeOfSource SourceType, string SourceView, ReplaceDatabase.CongruenceStatus Status)
        {
            InitializeComponent();
            this.groupBoxPackages.Visible = false;
            this._SourceType = SourceType;
            this._SourceView = SourceView;
            this.SetCongruenceStatus(Status);
            this.groupBox.Text = this._SourceView;
        }

        public UserControlExchangeCongruence(CongruenceObject congruenceObject)
        {
            InitializeComponent();
            this.initControl(congruenceObject);
        }

        #endregion

        #region Interface

        public int CongruenceLevel()
        {
            switch(this._CongruenceStatus)
            {
                case ReplaceDatabase.CongruenceStatus.Added:
                    return 0; // = Added
                case ReplaceDatabase.CongruenceStatus.Congruent:
                    return 1; // = OK
                case ReplaceDatabase.CongruenceStatus.DataCountDifferent:
                    return 2; // = conflict
                case ReplaceDatabase.CongruenceStatus.UpdateNeeded:
                    return 3; // = Update
                default:
                    return 4; // = Error
            }
        }

        public bool IsCongruent()
        {
            if (CongruenceLevel() < 3)
                return true;
            else
                return false;
        }

        #endregion

        #region Congruence

        public string IncongruenceIssue()
        {
            if (this._CongruenceStatus == ReplaceDatabase.CongruenceStatus.Congruent)
                return "";
            string Congruence = "";
            switch (this._CongruenceStatus)
            {
                case ReplaceDatabase.CongruenceStatus.DataAreMissing:
                    Congruence = "No data";
                    break;
                case ReplaceDatabase.CongruenceStatus.UpdateNeeded:
                    Congruence = "Needs update";
                    break;
                case ReplaceDatabase.CongruenceStatus.Missing:
                    Congruence = "Is missing";
                    this.BackColor = System.Drawing.Color.Pink;
                    break;
                //case ReplaceDatabase.CongruenceStatus.PackagesDiffering:
                //    Congruence = "Packages differing";
                //    break;
                default:
                    Congruence = "Packages differing";
                    break;
            }
            return Congruence;
        }

        private void SetCongruenceStatus(ReplaceDatabase.CongruenceStatus Status)
        {
            this._CongruenceStatus = Status;
            switch (Status)
            {
                case ReplaceDatabase.CongruenceStatus.Added:
                    //this.pictureBoxCongruenceStatus.Image = this.imageListStatus.Images[0];
                    this.labelMessage.Text = "Will be added";
                    this._IsCongruent = true;
                    break;
                case ReplaceDatabase.CongruenceStatus.Congruent:
                    //this.pictureBoxCongruenceStatus.Image = this.imageListStatus.Images[1];
                    this._IsCongruent = true;
                    break;
                case ReplaceDatabase.CongruenceStatus.DataAreMissing:
                    //this.pictureBoxCongruenceStatus.Image = this.imageListStatus.Images[2];
                    this.labelMessage.Text = "No data";
                    this._IsCongruent = false;
                    break;
                case ReplaceDatabase.CongruenceStatus.UpdateNeeded:
                    //this.pictureBoxCongruenceStatus.Image = this.imageListStatus.Images[3];
                    this.labelMessage.Text = "Needs update";
                    this._IsCongruent = false;
                    break;
                case ReplaceDatabase.CongruenceStatus.Missing:
                    //this.pictureBoxCongruenceStatus.Image = this.imageListStatus.Images[4];
                    this.labelMessage.Text = "Is missing";
                    this.BackColor = System.Drawing.Color.Pink;
                    this._IsCongruent = false;
                    break;
                //case ReplaceDatabase.CongruenceStatus.PackagesDiffering:
                //    this.pictureBoxCongruenceStatus.Image = this.imageListStatus.Images[5];
                //    this.labelMessage.Text = "Packages differing";
                //    this._IsCongruent = false;
                //    break;
                default:
                    //this.pictureBoxCongruenceStatus.Image = this.imageListStatus.Images[5];
                    this.labelMessage.Text = "Packages differing";
                    this._IsCongruent = false;
                    break;
            }
            ReplaceDatabase rd = new ReplaceDatabase();
            this.pictureBoxCongruenceStatus.Image = rd.ImageCongruenceStatus(Status); ;
        }

        private void initControl(CongruenceObject CO)
        {
            this._CongruenceObject = CO;
            this.groupBoxPackages.Visible = false;
            this._SourceView = CO.ObjectName;
            this.SetCongruenceStatus(CO.Status);
            this.groupBox.Text = this._SourceView;
            if (_CongruenceObject.CongruenceCounts.Count > 0)
            {
                this.InitCongruenceCount(CO, this.panelContent);
            }
            if (this._CongruenceObject.ContainedObjects != null && this._CongruenceObject.ContainedObjects.Count > 0)
            {
                foreach(System.Collections.Generic.KeyValuePair<string, CongruenceObject> co in this._CongruenceObject.ContainedObjects)
                {
                    DiversityCollection.CacheDatabase.UserControlExchangeCongruence U = new UserControlExchangeCongruence(co.Value);
                    this.panelPackages.Controls.Add(U);
                    U.Dock = DockStyle.Top;
                    U.BringToFront();
                }
                this.groupBoxPackages.Visible = true;
            }
        }


        private void InitCongruenceCount(CongruenceObject CO, System.Windows.Forms.Panel Panel)
        {
            try
            {
                int iCount = 0;
                Panel.Controls.Clear();
                Panel.Visible = true;
                foreach (CongruenceCount CC in CO.CongruenceCounts)
                {
                    if ((int)CC.Status > 1)
                    {
                        this.AddCongruenceCount(CC, Panel);
                        iCount++;
                    }
                }
                if (iCount == 0)
                    groupBox.Visible = false;
                else
                {
                    groupBox.Visible = true;
                    if (iCount > 1 && this.Height < 60)
                        this.Height = 36 + (18 * (iCount - 1));
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }



        private void InitCongruenceCount(CongruenceObject CO, System.Windows.Forms.GroupBox groupBox)
        {
            try
            {
                int iCount = 0;
                groupBox.Controls.Clear();
                groupBox.Visible = true;
                foreach (CongruenceCount CC in CO.CongruenceCounts)
                {
                    if ((int)CC.Status > 1)
                    {
                        this.AddCongruenceCount(CC, groupBox);
                        iCount++;
                    }
                }
                if (iCount == 0)
                    groupBox.Visible = false;
                else
                {
                    groupBox.Visible = true;
                    if (iCount > 1)
                        this.Height = 36 + (18 * (iCount - 1));
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void AddCongruenceCount(CongruenceCount CC, System.Windows.Forms.Panel Panel)
        {
            System.Windows.Forms.Panel P = new Panel();
            P.Height = 18;
            System.Windows.Forms.PictureBox Pi = new PictureBox();
            ReplaceDatabase rd = new ReplaceDatabase();
            Pi.Image = rd.ImageCongruenceStatus(CC.Status);
            Pi.Height = 16;
            Pi.Width = 16;
            P.Controls.Add(Pi);
            Pi.Dock = DockStyle.Right;
            System.Windows.Forms.Label L = new Label();
            L.Text = CC.SourceName;
            P.Controls.Add(L);
            L.Dock = DockStyle.Fill;
            L.TextAlign = ContentAlignment.MiddleRight;
            L.BringToFront();
            Panel.Controls.Add(P);
            P.Dock = DockStyle.Top;
            P.BringToFront();
            this.buttonInfo.Click += Pp_Click;
        }


        private void AddCongruenceCount(CongruenceCount CC, System.Windows.Forms.GroupBox groupBox)
        {
            System.Windows.Forms.Panel P = new Panel();
            P.Height = 18;
            if (groupBox.Controls.Count == 0)
            {
                System.Windows.Forms.PictureBox Pp = new PictureBox();
                Pp.Dock = DockStyle.Left;
                Pp.Image = DiversityCollection.Resource.Manual;
                Pp.Height = 16;
                Pp.Width = 16;
                Pp.Click += Pp_Click;
                P.Controls.Add(Pp);
            }
            System.Windows.Forms.PictureBox Pi = new PictureBox();
            ReplaceDatabase rd = new ReplaceDatabase();
            Pi.Image = rd.ImageCongruenceStatus(CC.Status);
            //switch (CC.Status)
            //{
            //    case ReplaceDatabase.CongruenceStatus.Added:
            //        Pi.Image = DiversityCollection.Resource.Add1;
            //        break;
            //    case ReplaceDatabase.CongruenceStatus.Congruent:
            //        Pi.Image = DiversityCollection.Resource.OK;
            //        break;
            //    case ReplaceDatabase.CongruenceStatus.DataAreMissing:
            //    case ReplaceDatabase.CongruenceStatus.Missing:
            //        Pi.Image = DiversityCollection.Resource.Error;
            //        P.BackColor = System.Drawing.Color.Pink;
            //        break;
            //    case ReplaceDatabase.CongruenceStatus.UpdateNeeded:
            //        Pi.Image = DiversityCollection.Resource.Update;
            //        break;
            //    case ReplaceDatabase.CongruenceStatus.DataCountDifferent:
            //        Pi.Image = DiversityCollection.Resource.Conflict;
            //        break;
            //}
            Pi.Height = 16;
            Pi.Width = 16;
            P.Controls.Add(Pi);
            Pi.Dock = DockStyle.Right;
            System.Windows.Forms.Label L = new Label();
            L.Text = CC.SourceName;
            P.Controls.Add(L);
            L.Dock = DockStyle.Fill;
            L.TextAlign = ContentAlignment.MiddleRight;
            L.BringToFront();
            groupBox.Controls.Add(P);
            P.Dock = DockStyle.Top;
            P.BringToFront();
        }

        private void Pp_Click(object sender, EventArgs e)
        {
            string Message = "Old\t->\tNew\tName of source\r\n";
            foreach(CongruenceCount CC in this._CongruenceObject.CongruenceCounts)
            {
                Message += "\r\n" + CC.CountOld.ToString() + "\t->\t" + CC.CountNew.ToString() + "\t" + CC.SourceName;
            }
            System.Windows.Forms.MessageBox.Show(Message, "Content of " + this._CongruenceObject.ObjectName);
        }

        #endregion

        #region Project

        //private void initProject()
        //{
        //    this.initPackages();
        //    return;


        //switch (this._CongruenceStatus)
        //{
        //    case CongruenceStatus.Added:
        //        break;
        //}
        //if (this._CongruenceStatus == CongruenceStatus.Added)
        //{
        //    this.initPackages();
        //    return;
        //}
        //try
        //{
        //    string SQL = "SELECT \"" + this._Schema + "\".projectid();";
        //    string ConnectionString = DiversityWorkbench.PostgreSQL.Connection.ConnectionString(this._ExchangedDatabase);
        //    Npgsql.NpgsqlConnection con = new Npgsql.NpgsqlConnection(ConnectionString);
        //    con.Open();
        //    Npgsql.NpgsqlCommand C = new Npgsql.NpgsqlCommand("", con);
        //    C.CommandText = "SELECT \"" + this._Schema + "\".projectid();";
        //    int ExProjectID;
        //    if (!int.TryParse(C.ExecuteScalar().ToString(), out ExProjectID))
        //    {
        //    }
        //    con.Close();
        //    con.Dispose();
        //    // Check if Replacement contains Project
        //    //if (this._CongruenceStatus != CongruenceStatus.Missing)
        //    //{
        //    //    ConnectionString = DiversityWorkbench.PostgreSQL.Connection.ConnectionString(this._ReplacementDatabase);
        //    //    Npgsql.NpgsqlConnection conRe = new Npgsql.NpgsqlConnection(ConnectionString);
        //    //    conRe.Open();
        //    //    Npgsql.NpgsqlCommand CRe = new Npgsql.NpgsqlCommand("", con);
        //    //    CRe.CommandText = "SELECT \"" + this._Schema + "\".projectid();";
        //    //    int ProjectIDRe = int.Parse(CRe.ExecuteScalar().ToString());
        //    //    conRe.Close();
        //    //    conRe.Dispose();
        //    //}
        //    this.initPackages();
        //}
        //catch (Exception ex)
        //{
        //    this.SetCongruenceStatus(CongruenceStatus.Missing);
        //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //}
        //}

        private void initPackages()
        {
            try
            {
                int iPackage = 0;
                this.panelPackages.Controls.Clear();
                this.groupBoxPackages.Visible = true;
                foreach (System.Collections.Generic.KeyValuePair<string, ReplaceDatabase.CongruenceStatus> KV in this._PackageCongruence)
                {
                    this.AddPackage(KV.Key, KV.Value);
                    iPackage++;
                }

                if (iPackage == 0)
                    this.groupBoxPackages.Visible = false;
                else
                {
                    this.groupBoxPackages.Visible = true;
                    if (iPackage > 1)
                        this.Height = 36 + (18 * (iPackage - 1));
                }
            }
            catch (System.Exception ex)
            { }
        }

        private void AddPackage(string Package, ReplaceDatabase.CongruenceStatus Status)
        {
            System.Windows.Forms.Panel P = new Panel();
            P.Height = 18;
            if (this.panelPackages.Controls.Count == 0)
            {
                System.Windows.Forms.PictureBox Pp = new PictureBox();
                Pp.Dock = DockStyle.Left;
                Pp.Image = DiversityCollection.Resource.Package;
                Pp.Height = 16;
                Pp.Width = 16;
                P.Controls.Add(Pp);
            }
            System.Windows.Forms.PictureBox Pi = new PictureBox();
            switch (Status)
            {
                case ReplaceDatabase.CongruenceStatus.Added:
                    Pi.Image = DiversityCollection.Resource.Add1;
                    break;
                case ReplaceDatabase.CongruenceStatus.Congruent:
                    Pi.Image = DiversityCollection.Resource.OK;
                    break;
                case ReplaceDatabase.CongruenceStatus.Missing:
                    Pi.Image = DiversityCollection.Resource.Delete;
                    P.BackColor = System.Drawing.Color.Pink;
                    break;
                case ReplaceDatabase.CongruenceStatus.UpdateNeeded:
                    Pi.Image = DiversityCollection.Resource.Update;
                    break;
            }
            Pi.Height = 16;
            Pi.Width = 16;
            P.Controls.Add(Pi);
            Pi.Dock = DockStyle.Right;
            System.Windows.Forms.Label L = new Label();
            L.Text = Package;
            P.Controls.Add(L);
            L.Dock = DockStyle.Fill;
            L.TextAlign = ContentAlignment.MiddleRight;
            L.BringToFront();
            this.groupBoxPackages.Controls.Add(P);
            P.Dock = DockStyle.Top;
            P.BringToFront();
        }

        
        #endregion

    }
}
