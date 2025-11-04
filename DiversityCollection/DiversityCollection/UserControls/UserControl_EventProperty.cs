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
    public partial class UserControl_EventProperty : UserControl__Data
    {

        #region Parameter

        DiversityWorkbench.UserControls.RemoteValueBinding _RvbHierarchie;

        #endregion


        #region Construction

        public UserControl_EventProperty(
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

        #region Public

        public override void SetPosition(int Position)
        {
            base.SetPosition(Position);
            this.setValueControlVisibility();
            this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryEventPropertyResponsible, "ResponsibleAgentURI");
            this.setUserControlModuleRelatedEntrySourceProperty();
            //int PropertyID = this.setEventPropertyControls();
            //this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryScientificTerm, "PropertyID_" + PropertyID.ToString());
        }

        #endregion

        #region Private

		private void initControl()
        {
            // ScientificTerms
            DiversityWorkbench.ScientificTerm T = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
            this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryScientificTerm, T, "CollectionEventProperty", "DisplayText", "PropertyURI", this._Source);
            //von PartDescription übernommen - noch testen
            this.userControlModuleRelatedEntryScientificTerm.IsListInDatabase = true;
            this.userControlModuleRelatedEntryScientificTerm.EnableChart(true);

            System.Collections.Generic.List<DiversityWorkbench.UserControls.RemoteValueBinding> RvbProperty = new List<DiversityWorkbench.UserControls.RemoteValueBinding>();
            _RvbHierarchie = new DiversityWorkbench.UserControls.RemoteValueBinding();
            _RvbHierarchie.BindingSource = this._Source;
            _RvbHierarchie.Column = "PropertyHierarchyCache";
            _RvbHierarchie.RemoteParameter = "Hierarchy";
            RvbProperty.Add(_RvbHierarchie);
            this.userControlModuleRelatedEntryScientificTerm.setRemoteValueBindings(RvbProperty);

            // Agents
            DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
            this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryEventPropertyResponsible, A, "CollectionEventProperty", "ResponsibleName", "ResponsibleAgentURI", this._Source);

            this.textBoxEventPropertyValue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "PropertyValue", true));
            this.textBoxEventPropertyNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Notes", true));
            this.textBoxEventPropertyHierarchyCache.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "PropertyHierarchyCache", true));
            this.textBoxEventPropertyAverageValueCache.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "AverageValueCache", true));
            this.textBoxEventPropertyHierarchyCache.ReadOnly = true;

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();
        }

        private void buttonEventPropertyHierarchyCacheEdit_Click(object sender, EventArgs e)
        {
            string Hierarchy = "";
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            if (!R["PropertyHierarchyCache"].Equals(System.DBNull.Value))
                Hierarchy = R["PropertyHierarchyCache"].ToString();
            DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Hierachy", Hierarchy);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                R["PropertyHierarchyCache"] = f.EditedText;
                this.textBoxEventPropertyHierarchyCache.Text = f.EditedText;
            }
        }

        private void setValueControlVisibility()
        {
            bool ShowValue = false;
            bool ShowAverage = false;
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            int PropertyID;
            if (R != null && int.TryParse(R["PropertyID"].ToString(), out PropertyID))
            {
                string ParsingMethodName = DiversityCollection.LookupTable.PropertyParsingMethod(PropertyID);
                switch (ParsingMethodName)
                {
                    case "Vegetation":
                        goto default;
                    case "Geographic region":
                        goto default;
                    case "Chronostratigraphy":
                    case "Lithostratigraphy":
                    case "Stratigraphy":
                        ShowValue = true;
                        ShowAverage = false;
                        break;
                    default:
                        ShowValue = false;
                        ShowAverage = false;
                        break;
                }

                this.labelEventPropertyValue.Visible = ShowValue;
                this.textBoxEventPropertyValue.Visible = ShowValue;
                this.labelEventPropertyAverageValueCache.Visible = ShowAverage;
                this.textBoxEventPropertyAverageValueCache.Visible = ShowAverage;
                if (!ShowAverage)
                {
                    this.labelEventPropertyAverageValueCache.Text = "";
                    this.textBoxEventPropertyAverageValueCache.Width = 0;
                }
                else
                {
                    //this.labelEventPropertyAverageValueCache.Text = "Average:";
                    //this.textBoxEventPropertyAverageValueCache.Width = (int)(125 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                }

                //this.userControlModuleRelatedEntryScientificTerm.setChart(false);
                string PropertyURI = DiversityCollection.LookupTable.PropertyURI(PropertyID);
                if (PropertyURI.Length > 0)
                {
                    DiversityWorkbench.Chart C = DiversityWorkbench.Terminology.GetChart(PropertyURI);
                    if (C != null)
                    {
                        string Property = DiversityCollection.LookupTable.PropertyName(PropertyID);
                        string BaseURL = DiversityWorkbench.WorkbenchUnit.getBaseURIfromURI(PropertyURI);
                        //this.userControlModuleRelatedEntryScientificTerm.EnableChart(C, BaseURL, "Please select an item from " + Property);
                    }
                    //else
                    //{
                    //    this.userControlModuleRelatedEntryScientificTerm.setChart(false, true);
                    //}
                }
                else
                {
                    //this.userControlModuleRelatedEntryScientificTerm.setChart(false, true);
                }
            }
        }

        private int setEventPropertyControls()
        {
            int PropertyID = 0;
            string ParsingMethodName;
            if (this._Source.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                if (int.TryParse(R["PropertyID"].ToString(), out PropertyID))
                {
                    ParsingMethodName = DiversityCollection.LookupTable.ParsingMethodNameProperty(PropertyID);
                    string PropertyName = DiversityCollection.LookupTable.PropertyName(PropertyID);
                    this.userControlModuleRelatedEntryScientificTerm.ListInDatabase = PropertyName;
                    try
                    {
                        System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtProperty.Select("PropertyID = " + PropertyID.ToString());
                        if (rr.Length > 0)
                        {
                            this.groupBoxEventProperty.Text = PropertyName;
                            this.pictureBoxEventProperty.Image = DiversityCollection.Specimen.ImageForCollectionEventProperty(PropertyID);
                            switch (ParsingMethodName)
                            {
                                case "Vegetation":
                                    goto default;
                                case "Geographic region":
                                    goto default;
                                case "Chronostratigraphy":
                                case "Lithostratigraphy":
                                case "Stratigraphy":
                                    this.labelEventPropertyValue.Visible = true;
                                    this.textBoxEventPropertyValue.Visible = true;
                                    this.labelEventPropertyAverageValueCache.Visible = false;
                                    this.labelEventPropertyAverageValueCache.Text = "";
                                    this.textBoxEventPropertyAverageValueCache.Visible = false;
                                    this.textBoxEventPropertyAverageValueCache.Width = 0;
                                    break;
                                case "TODO":
                                    this.labelEventPropertyAverageValueCache.Visible = true;
                                    this.labelEventPropertyAverageValueCache.Text = "Average:";
                                    this.textBoxEventPropertyAverageValueCache.Visible = true;
                                    this.textBoxEventPropertyAverageValueCache.Width = (int)(125 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                                    this.labelEventPropertyValue.Visible = true;
                                    this.textBoxEventPropertyValue.Visible = true;
                                    break;
                                default:
                                    this.labelEventPropertyAverageValueCache.Visible = false;
                                    this.labelEventPropertyValue.Visible = false;
                                    this.textBoxEventPropertyAverageValueCache.Visible = false;
                                    this.textBoxEventPropertyValue.Visible = false;
                                    break;
                            }
                        }
                        else
                        {
                            //this.splitContainerOverviewEventLocProp.Panel2.Visible = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
            return PropertyID;
        }

        private void setUserControlModuleRelatedEntrySourceProperty()
        {
            try
            {
                DiversityWorkbench.UserSettings U = new DiversityWorkbench.UserSettings();
                string Source = "";
                string ProjectID = "";
                string Project = "";
                // Markus Bugfix 17.7.24 - Test ob null
                System.Data.DataSet data = (System.Data.DataSet)this._Source.DataSource;
                System.Data.DataTable dataTable = data.Tables["CollectionEventProperty"];

                if (this._Source.Current != null) // || this._ParentSource != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                    string URI = "";

                    System.Collections.Generic.List<string> Settings = new List<string>();
                    Settings.Add("ModuleSource");
                    Settings.Add("CollectionEventProperty");
                    Settings.Add("PropertyID_" + R["PropertyID"].ToString());
                    //int? Width = null;
                    //int? Height = null;
                    //if (this.ParentForm != null)
                    //{
                    //    Width = this.ParentForm.Width;
                    //    Height = this.ParentForm.Height;
                    //}
                    //else if (this._iMainForm != null)
                    //{
                    //    Width = this._iMainForm.FormWidth();
                    //    Height = this._iMainForm.FormHeight();
                    //}
                    this.setUserControlModuleRelatedEntrySources(Settings,
                        ref userControlModuleRelatedEntryScientificTerm);//, Width, Height);

                    if (false)
                    {

                    Source = U.GetSetting(Settings);
                    //if (Source.Length > 0)
                    //{
                    //    this.userControlModuleRelatedEntryScientificTerm.FixSource(Source);
                    //    System.Collections.Generic.Dictionary<string, string> Options = U.GetSettingOptions(Settings, this.userControlModuleRelatedEntryScientificTerm.SourceWebSerice());
                    //    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Options)
                    //    {
                    //        this.userControlModuleRelatedEntryScientificTerm.SetSourceWebserviceOptions(KV.Key, KV.Value);
                    //    }
                    //}
                    // TODO Ariane ask Markus : added .lenght == 0
                    if (Source.Length == 0)
                    {
                        Source = U.GetSetting(Settings, "Database");
                        if (Source.Length == 0)
                        {
                            Source = U.GetSetting(Settings, "Webservice");
                            if (Source.Length == 0)
                            {
                                this.userControlModuleRelatedEntryScientificTerm.ReleaseSource();
                            }
                            else
                            {
                                this.userControlModuleRelatedEntryScientificTerm.FixSource(Source);
                            }
                        }
                        else
                        {
                            ProjectID = U.GetSetting(Settings, "ProjectID");
                            Project = U.GetSetting(Settings, "Project");
                            string SectionID = U.GetSetting(Settings, "SectionID");
                            string Section = U.GetSetting(Settings, "Section");
                            int iSectionID = -1;
                            int? iiSectionID = null;
                            if (int.TryParse(SectionID, out iSectionID))
                                iiSectionID = iSectionID;
                            iiSectionID = iSectionID;
                            //int? Width = null;
                            //int? Height = null;
                            if (this.ParentForm != null)
                            {
                                Width = this.ParentForm.Width;
                                Height = this.ParentForm.Height;
                            }
                            else if (this._iMainForm != null)
                            {
                                Width = this._iMainForm.FormWidth();
                                Height = this._iMainForm.FormHeight();
                            }

                            this.userControlModuleRelatedEntryScientificTerm.FixSource(Source, ProjectID, Project, iSectionID, Section);

                            }
                    }
                    }

                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Help

        public System.Windows.Forms.TableLayoutPanel TableLayoutPanelEventProperty
        {
            get { return this.tableLayoutPanelEventProperty; }
        }

        #endregion


    }
}
