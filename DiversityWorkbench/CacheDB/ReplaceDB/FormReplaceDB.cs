using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityWorkbench.CacheDB.ReplaceDB
{
    public partial class FormReplaceDB : Form
    {
        #region Parameter
        //private string _OldReplacedDatabase = "";
        //private string _NewReplacementDatabase = "";
        private Replacement _Replacement;
        private string _DatabaseOwner = "CacheAdmin";
        private int? _StepsMax = null;
        private bool _DatabaseHasBeenReplaced = false;

        #endregion
        public bool DatabaseHasBeenReplaced() { return _DatabaseHasBeenReplaced; }

        #region Construction
        public FormReplaceDB(ref Replacement replacement, string DatabaseOwner)
        {
            InitializeComponent();
            this._Replacement = replacement;
            this._DatabaseOwner = DatabaseOwner;
            this.initForm();
            this.labelMessage.Visible = false;
            this.progressBar.Visible = false;
//#if !DEBUG
//            this.checkBoxKeepCurrentDatabase.Checked = false;
//            this.checkBoxKeepCurrentDatabase.Enabled = false;
//#endif
        }

        #endregion

        #region Interface

        public void setStepsMax(int Max)
        {
            this._StepsMax = Max;
            this.progressBar.Maximum = Max;
        }

        private void setStep(string Message)
        {
            if (Message.Length == 0)
            {
                this.progressBar.Visible = false;
                this.progressBar.Value = 0;
                this.labelMessage.Visible = false;
                this.labelMessage.Text = Message;
            }
            else
            {
                this.progressBar.Visible = true;
                if (this.progressBar.Value < this.progressBar.Maximum)
                    this.progressBar.Value++;
                this.labelMessage.Visible = true;
                this.labelMessage.Text = Message;

            }
            System.Windows.Forms.Application.DoEvents();
        }


        #endregion

        #region Form

        private void initForm()
        {
            try
            {
                // Header
                this.labelHeader.Text = this._Replacement.NewDatabase;
                this.checkBoxKeepCurrentDatabase.Text += this._Replacement.NewDatabase;

                // source for databases
                this.comboBoxReplacedDatabase.DataSource = this._Replacement.DtDatabases();
                this.comboBoxReplacedDatabase.DisplayMember = "datname";
                this.comboBoxReplacedDatabase.ValueMember = "datname";

                // imagelists for tabcontrols
                this.tabControlProjects.ImageList = this._Replacement.ImageListStates();
                this.tabControlCongruence.ImageList = this._Replacement.ImageListStates();
                this.tabControlSources.ImageList = this._Replacement.ImageListStates();
                this.tabControlWebservices.ImageList = this._Replacement.ImageListStates();

                // removing not used tabs
                if (!this._Replacement.Sources().ContainsKey(Replacement.Type.Agent))
                    this.tabControlSources.TabPages.Remove(this.tabPageAgents);
                if (!this._Replacement.Sources().ContainsKey(Replacement.Type.Collection))
                    this.tabControlSources.TabPages.Remove(this.tabPageCollection);
                if (!this._Replacement.Sources().ContainsKey(Replacement.Type.Gazetteer))
                    this.tabControlSources.TabPages.Remove(this.tabPageGazetteer);
                if (!this._Replacement.Sources().ContainsKey(Replacement.Type.SamplingPlot))
                    this.tabControlSources.TabPages.Remove(this.tabPagePlots);
                if (!this._Replacement.Sources().ContainsKey(Replacement.Type.Reference))
                    this.tabControlSources.TabPages.Remove(this.tabPageReferences);
                if (!this._Replacement.Sources().ContainsKey(Replacement.Type.Taxon))
                    this.tabControlSources.TabPages.Remove(this.tabPageTaxa);

                if (!this._Replacement.Sources().ContainsKey(Replacement.Type.Reference_Webservice))
                    this.tabControlWebservices.TabPages.Remove(this.tabPageReferenceWebservice);
                if (!this._Replacement.Sources().ContainsKey(Replacement.Type.Taxon_Webservice))
                    this.tabControlWebservices.TabPages.Remove(this.tabPageTaxaWebservice);

                // Projects
                this.tabControlProjects.TabPages.Clear();

                this.labelMessage.Visible = false;
                this.progressBar.Visible = false;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        private void comboBoxExchangedDatabase_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                this._Replacement.SetOldReplacedDatabase(this.comboBoxReplacedDatabase.SelectedValue.ToString());
                this.buttonReplace.Text = "Rename current database\r\n   \t" + this._Replacement.NewDatabase +
                    "\t   to   \t" + _Replacement.OldDatabase +
                    "\r\nand delete old version of\r\n   \t" + _Replacement.OldDatabase;

                System.Windows.Forms.Application.DoEvents();
                setStep("");
                bool CanReplace = true;
                // Sources
                bool OK = this.InitSources();
                if (!OK)
                    CanReplace = OK;
                // Projects
                OK = this.InitProjects();
                if (!OK)
                    CanReplace = OK;
                this.buttonReplace.Enabled = CanReplace;
                setStep("");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void buttonEnableIncongruentExchange_Click(object sender, EventArgs e)
        {
            this.buttonReplace.Enabled = true;
        }

        private void buttonReplace_Click(object sender, EventArgs e)
        {
            string Message = "";
            bool OK = this._Replacement.ReplaceDatabase(ref Message);//, this._DatabaseOwner);
            if (OK)
            {
                this.buttonReplace.Enabled = false;
                this.comboBoxReplacedDatabase.Enabled = false;
                _DatabaseHasBeenReplaced = true;
                System.Windows.Forms.MessageBox.Show("Replacement has been successful");
                OK = true;
                this.Close();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Replacement failed: " + Message);
            }
        }


#endregion

        private bool InitProjects()
        {
            bool OK = true;
            // getting rid of old controls
            this.tabControlProjects.TabPages.Clear();
            Replacement.Status statusPojects = Replacement.Status._None;

            try
            {
                foreach(System.Collections.Generic.KeyValuePair<string, ReplacedPart> KV in this._Replacement.ProjectReplacedParts())
                {
                    // creating the tab page
                    System.Windows.Forms.TabPage TP = new TabPage(KV.Value.Name);
                    TP.ImageIndex = this._Replacement.ImageIndexStatus(KV.Value.Status());
                    this.tabControlProjects.TabPages.Add(TP);
                    TP.AutoScroll = true;

                    // adding the user control
                    UserControlReplacement U = new UserControlReplacement(KV.Value, ref this._Replacement);
                    U.Dock = DockStyle.Fill;
                    U.BringToFront();
                    //P.Controls.Add(U);
                    TP.Controls.Add(U);

                    statusPojects = this._Replacement.StatusWorst(statusPojects, KV.Value.Status());

                    this.setStep("Checking Project: " + KV.Value.Name);
                }
                if (statusPojects != Replacement.Status._None)
                {
                    this.tabPageProjects.ImageIndex = this._Replacement.ImageIndexStatus(statusPojects);
                    OK = this._Replacement.CanReplaceDB(statusPojects);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        private bool InitSources()
        {
            bool OK = true;
            bool CanReplaceDB = true;
            try
            {
                Replacement.Status statusSources = Replacement.Status._None;
                Replacement.Status statusSourcesWeb = Replacement.Status._None;
                foreach (System.Collections.Generic.KeyValuePair<Replacement.Type, ReplacedPart> KV in this._Replacement.Sources())
                {
                    System.Windows.Forms.Panel P = null;
                    System.Windows.Forms.TabPage TP = null;
                    switch(KV.Key)
                    {
                        case Replacement.Type.Agent:
                            P = this.panelAgents;
                            TP = this.tabPageAgents;
                            break;
                        case Replacement.Type.Collection:
                            P = this.panelCollection;
                            TP = this.tabPageCollection;
                            break;
                        case Replacement.Type.Gazetteer:
                            P = this.panelGazetteer;
                            TP = this.tabPageGazetteer;
                            break;
                        case Replacement.Type.Reference:
                            P = this.panelReferences;
                            TP = this.tabPageReferences;
                            break;
                        case Replacement.Type.Reference_Webservice:
                            P = this.panelReferenceWebservice;
                            TP = this.tabPageReferenceWebservice;
                            break;
                        case Replacement.Type.SamplingPlot:
                            P = this.panelPlots;
                            TP = this.tabPagePlots;
                            break;
                        case Replacement.Type.ScientificTerm:
                            P = this.panelTerms;
                            TP = this.tabPageTerms;
                            break;
                        case Replacement.Type.Taxon:
                            P = this.panelTaxa;
                            TP = this.tabPageTaxa;
                            break;
                        case Replacement.Type.Taxon_Webservice:
                            P = this.panelTaxaWebservice;
                            TP = this.tabPageTaxaWebservice;
                            break;
                    }
                    // filling the panel
                    if (P != null)
                    {
                        // getting rid of old controls
                        Replacement.Status status = Replacement.Status._None;
                        P.Controls.Clear();
                        foreach(System.Collections.Generic.KeyValuePair<string, ReplacedPart> RP in this._Replacement.SourceReplacedParts(KV.Key))
                        {
                            status = this._Replacement.StatusWorst(status, RP.Value.Status());
                            switch (KV.Key)
                            {
                                case Replacement.Type.Reference_Webservice:
                                case Replacement.Type.Taxon_Webservice:
                                    statusSourcesWeb = this._Replacement.StatusWorst(statusSourcesWeb, RP.Value.Status());
                                    break;
                                default:
                                    statusSources = this._Replacement.StatusWorst(statusSources, RP.Value.Status());
                                    break;
                            }
                            // adding the user control
                            UserControlReplacement U = new UserControlReplacement(RP.Value, ref this._Replacement);
                            U.Dock = DockStyle.Top;
                            U.BringToFront();
                            P.Controls.Add(U);
                            this.setStep("Checking " + KV.Key.ToString() + " source: " + RP.Value.Name);
                        }
                        if (status != Replacement.Status._None)
                        {
                            TP.ImageIndex = this._Replacement.ImageIndexStatus(status);
                            OK = this._Replacement.CanReplaceDB(status);
                            if (!OK)
                                CanReplaceDB = OK;
                        }
                    }
                }
                if (statusSources != Replacement.Status._None)
                    this.tabPageSources.ImageIndex = this._Replacement.ImageIndexStatus(statusSources);
                if (statusSourcesWeb != Replacement.Status._None)
                    this.tabPageWebservices.ImageIndex = this._Replacement.ImageIndexStatus(statusSourcesWeb);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                CanReplaceDB = false;
            }
            return CanReplaceDB;
        }

        private void checkBoxKeepCurrentDatabase_CheckedChanged(object sender, EventArgs e)
        {
            this._Replacement.KeepCopyOfDB = this.checkBoxKeepCurrentDatabase.Checked;
        }

        private void checkBoxRestrictToCriticalCounts_CheckedChanged(object sender, EventArgs e)
        {
            this._Replacement.RestrictToCritical = this.checkBoxRestrictToCriticalCounts.Checked;
        }


    }
}
