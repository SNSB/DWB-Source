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
    public partial class FormTool : Form
    {

        /*
         * 
         * 
 update t
 set [ToolUsageTemplate] = 
 '<Tools xmlns="http://diversityworkbench.net/Schema/tools">
	<Tool Name="Photometer" ToolID="1">
      <Usage Name="Anregungswellenlaenge" Value="">
        <ValueEnum>504 nm</ValueEnum>
        <ValueEnum>356 nm</ValueEnum>
        <ValueEnum>289 nm</ValueEnum>
      </Usage>
      <Usage Name="Filter" Value="">
      </Usage>
    </Tool>
</Tools>'
  FROM [DiversityCollection_Test].[dbo].[Tool]
t where t.ToolID  = 1

 update t
 set [ToolUsageTemplate] = 
 '<Tools xmlns="http://diversityworkbench.net/Schema/tools">
	<Tool Name="LCO1490" ToolID="2">
      <Usage Name="Direction" Value="">
        <ValueEnum>Forward</ValueEnum>
        <ValueEnum>Reverse</ValueEnum>
      </Usage>
      <Usage Name="Topology" Value=""/>
      <Usage Name="Gene" Value=""/>
    </Tool>
</Tools>'
  FROM [DiversityCollection_Test].[dbo].[Tool]
t where t.ToolID  = 2

SELECT XML_SCHEMA_NAMESPACE (N'dbo', N'Tools')

         * */


        #region Parameter

        private DiversityCollection.Tool _Tool ;

        #endregion

        #region Construction

        public FormTool()
        {
            InitializeComponent();
            this.splitContainerData.Panel2Collapsed = true;
            this.splitContainerMain.Panel2.Visible = false;
            this.initForm();
            // online manual
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
        }

        public FormTool(int? ItemID)
            : this()
        {
            if (ItemID != null)
            {
                this._Tool.setItem((int)ItemID);
                this.labelHeader.Visible = false;
                this.menuStripMain.Visible = false;
                this.splitContainerMain.Panel1Collapsed = true;
                this.splitContainerMain.Panel2.Enabled = false;
                //this.userControlDialogPanel.Visible = false;
            }
            else
            {
                this.userControlDialogPanel.Visible = true;
                this.labelHeader.Visible = true;
            }
        }

        public FormTool(int? ItemID, System.Collections.Generic.List<DiversityWorkbench.UserControls.QueryRestrictionItem> Restrictions)
            : this(ItemID)
        {
            this.userControlQueryList.setQueryRestrictionList(Restrictions);
            this.userControlQueryList.StartQuery();
        }

        #endregion

        #region Form

        private void initForm()
        {
            System.Data.DataSet Dataset = this.dataSetTool;
            if (this._Tool == null)
                this._Tool = new Tool(ref Dataset, this.dataSetTool.Tool,
                    ref this.treeViewTool, this, this.userControlQueryList, this.splitContainerMain,
                    this.splitContainerData, null,// null,
                    null, this.helpProvider, this.toolTip, ref this.toolBindingSource);
            this._Tool.initForm();
            this._Tool.setToolStripButtonDeleteEvent(this.toolStripButtonDelete);
            this._Tool.setToolStripButtonNewEvent(this.toolStripButtonNew);
            this._Tool.setToolStripButtonIncludeIDEvent(this.toolStripButtonIncludeID);
            this._Tool.setToolStripButtonSetParentWithHierarchyEvent(this.toolStripButtonSetParent);
            this._Tool.setToolStripButtonRemoveParentEvent(this.toolStripButtonRemoveParent);
            this.setPermissions();
            //this.userControlSpecimenList.toolStripButtonDelete.Visible = false;
            //this.toolStripButtonSpecimenList.Visible = false;
        }

        private void FormTool_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._Tool.saveItem();
        }

        private void setPermissions()
        {
            DiversityWorkbench.Forms.FormFunctions FF = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
            this._Tool.setToolbarPermission(ref this.toolStripButtonDelete, "Tool", "Delete");
            this._Tool.setToolbarPermission(ref this.toolStripButtonNew, "Tool", "Insert");

            System.Collections.Generic.List<System.Windows.Forms.Control> ControlList = new List<Control>();
            System.Collections.Generic.List<System.Windows.Forms.ToolStripItem> TSIlist = new List<ToolStripItem>();
            TSIlist.Add(this.toolStripButtonAnalysisAdd);
            FF.setObjectsAccordingToPermission(ControlList, TSIlist, "ToolForAnalysis", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Insert);
            TSIlist.Clear();
            TSIlist.Add(this.toolStripButtonAnalysisDelete);
            FF.setObjectsAccordingToPermission(ControlList, TSIlist, "ToolForAnalysis", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Delete);

            TSIlist.Clear();
            TSIlist.Add(this.toolStripButtonProcessingAdd);
            FF.setObjectsAccordingToPermission(ControlList, TSIlist, "ToolForProcessing", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Insert);
            TSIlist.Clear();
            TSIlist.Add(this.toolStripButtonProcessingDelete);
            FF.setObjectsAccordingToPermission(ControlList, TSIlist, "ToolForProcessing", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Delete);
        }


        private void FormTool_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetTool.ToolForProcessingList". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.toolForProcessingListTableAdapter.Fill(this.dataSetTool.ToolForProcessingList);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetTool.ToolForAnalysisList". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.toolForAnalysisListTableAdapter.Fill(this.dataSetTool.ToolForAnalysisList);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetTool.Tool". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.toolTableAdapter.Fill(this.dataSetTool.Tool);

        }

        #endregion

        #region Menue
        
        private void toolStripMenuItemHistory_Click(object sender, EventArgs e)
        {
            string Title = "History of " + this.dataSetTool.Tool.Rows[0]["Name"].ToString() + " (ToolID: " + this.dataSetTool.Tool.Rows[0]["ToolID"].ToString() + ")";
            try
            {
                bool HistoryPresent = false;
                System.Collections.Generic.List<System.Data.DataTable> LogTables = new List<DataTable>();

                if (this.dataSetTool.Tool.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "ToolID", this.dataSetTool.Tool.TableName, ""));
                    HistoryPresent = true;
                }
                if (HistoryPresent)
                {
                    DiversityWorkbench.Forms.FormHistory f = new DiversityWorkbench.Forms.FormHistory(Title, LogTables, DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace());
                    f.setHelpProviderNameSpace(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace(), "History");
                    f.ShowDialog();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("No history data found");
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void feedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                this.userControlQueryList.QueryString(),
                this.ID.ToString());
        }

        private void showURIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.showURIToolStripMenuItem.Checked)
                {
                    this.showURIToolStripMenuItem.Checked = false;
                    this.splitContainerURI.Panel2Collapsed = true;
                }
                else
                {
                    this.showURIToolStripMenuItem.Checked = true;
                    this.splitContainerURI.Panel2Collapsed = false;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        #endregion

        #region URI

        private void buttonUriOpen_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxURI.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                int i = this.toolBindingSource.Position;
                this.dataSetTool.Tool.Rows[i]["ToolURI"] = f.URL;
                this.textBoxURI.Text = f.URL;
            }
        }

        private void textBoxURI_TextChanged(object sender, EventArgs e)
        {
            if (this.textBoxURI.Text.Length > 0)
            {
                this.webBrowserURI.Visible = true;
                try
                {
                    System.Uri URI = new Uri(this.textBoxURI.Text);
                    this.webBrowserURI.Url = URI;
                }
                catch { }
            }
            else
                this.webBrowserURI.Visible = false;
        }

        #endregion

        #region Tool

        private void textBoxName_Leave(object sender, EventArgs e)
        {
            if (this.treeViewTool.Nodes.Count > 0)
            {
                try
                {
                    if (this.treeViewToolUsageTemplate.Nodes[0].Nodes[0].Tag != null)
                    {
                        DiversityWorkbench.UserControls.XMLNode XN = (DiversityWorkbench.UserControls.XMLNode)this.treeViewToolUsageTemplate.Nodes[0].Nodes[0].Tag;
                        if (XN.Name == "Tool")
                        {
                            DiversityWorkbench.UserControls.XMLAttribute XAnew = new DiversityWorkbench.UserControls.XMLAttribute();
                            int i = 0;
                            foreach (DiversityWorkbench.UserControls.XMLAttribute XA in XN.Attributes)
                            {
                                if (XA.Name == "Name")
                                {
                                    XAnew.Name = XA.Name;
                                    XAnew.Value = this.textBoxName.Text;
                                    break;
                                }
                                i++;
                            }
                            XN.Attributes.RemoveAt(i);
                            XN.Attributes.Add(XAnew);
                            this.treeViewToolUsageTemplate.Nodes[0].Nodes[0].Text = XAnew.Value;
                            this.toolStripButtonToolsSave.Enabled = true;
                            this.toolStripButtonToolsSave.BackColor = System.Drawing.Color.Red;
                            this.toolStripButtonToolsSave_Click(null, null);
                        }
                    }
                }
                catch(System.Exception ex){}
            }
            //System.Data.DataRowView R = (System.Data.DataRowView)this.toolBindingSource.Current;
            //R.BeginEdit();
            //this.writeToolTreeToXml();
            //R.EndEdit();
            //this.toolStripButtonToolsSave.Enabled = false;
        }

        #endregion

        #region Processing
        
        private void toolStripButtonProcessingAdd_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT P.HierarchyDisplayText AS DisplayText, P.ProcessingID " +
                "FROM dbo.ProcessingHierarchyAll() P " +
                "WHERE P.ProcessingID NOT IN (SELECT ProcessingID FROM ToolForProcessing WHERE ToolID = " + this.ID.ToString() + ") " +
                "ORDER BY P.HierarchyDisplayText";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            System.Collections.Generic.Dictionary<string, string> Entity = DiversityWorkbench.Entity.EntityInformation("ToolForProcessing.ProcessingID");
            string Header = "Please select the processing that should be added to the tool";
            //try { Header = DiversityCollection.FormAnalysisText.Please_select_the_project_that; }
            //catch { }
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "DisplayText", "ProcessingID", DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, Entity), Header);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                System.Data.DataRow R = this.dataSetTool.ToolForProcessing.NewRow();
                System.Data.DataRow Rlist = this.dataSetTool.ToolForProcessingList.NewToolForProcessingListRow();
                R["ToolID"] = this.ID;
                R["ProcessingID"] = int.Parse(f.SelectedValue.ToString());
                Rlist["ToolID"] = this.ID;
                Rlist["DisplayText"] = f.SelectedString;
                Rlist["ProcessingID"] = int.Parse(f.SelectedValue.ToString());
                try
                {
                    this.dataSetTool.ToolForProcessing.Rows.Add(R);
                    this.dataSetTool.ToolForProcessingList.Rows.Add(Rlist);
                }
                catch { }
            }
        }

        private void toolStripButtonProcessingDelete_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxProcessing.SelectedItem;
                int ProcessingID = int.Parse(RV["ProcessingID"].ToString());
                System.Data.DataRow[] RR = this.dataSetTool.ToolForProcessing.Select("ProcessingID = " + ProcessingID.ToString());
                if (RR.Length > 0)
                {
                    RR[0].Delete();
                    this.toolForProcessingListBindingSource.RemoveCurrent();
                }
            }
            catch { }
        }
        
        #endregion

        #region Analysis
        
        private void toolStripButtonAnalysisAdd_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT P.HierarchyDisplayText AS DisplayText, P.AnalysisID " +
                "FROM dbo.AnalysisHierarchyAll() P " +
                "WHERE P.AnalysisID NOT IN (SELECT AnalysisID FROM ToolForAnalysis WHERE ToolID = " + this.ID.ToString() + ") " +
                "ORDER BY P.HierarchyDisplayText";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            System.Collections.Generic.Dictionary<string, string> Entity = DiversityWorkbench.Entity.EntityInformation("ToolForAnalysis.AnalysisID");
            string Header = "Please select the Analysis that should be added to the tool";
            //try { Header = DiversityCollection.FormAnalysisText.Please_select_the_project_that; }
            //catch { }
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "DisplayText", "AnalysisID", DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, Entity), Header);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                System.Data.DataRow R = this.dataSetTool.ToolForAnalysis.NewRow();
                System.Data.DataRow Rlist = this.dataSetTool.ToolForAnalysisList.NewToolForAnalysisListRow();
                R["ToolID"] = this.ID;
                R["AnalysisID"] = int.Parse(f.SelectedValue.ToString());
                Rlist["ToolID"] = this.ID;
                Rlist["DisplayText"] = f.SelectedString;
                Rlist["AnalysisID"] = int.Parse(f.SelectedValue.ToString());
                try
                {
                    this.dataSetTool.ToolForAnalysis.Rows.Add(R);
                    this.dataSetTool.ToolForAnalysisList.Rows.Add(Rlist);
                }
                catch { }
            }
        }

        private void toolStripButtonAnalysisDelete_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxAnalysis.SelectedItem;
                int AnalysisID = int.Parse(RV["AnalysisID"].ToString());
                System.Data.DataRow[] RR = this.dataSetTool.ToolForAnalysis.Select("AnalysisID = " + AnalysisID.ToString());
                if (RR.Length > 0)
                {
                    RR[0].Delete();
                    this.toolForAnalysisListBindingSource.RemoveCurrent();
                }
            }
            catch (System.Exception ex) { }
        }
        
        #endregion

        #region ToolUsageTemplate
        
        private void toolStripButtonToolsAdd_Click(object sender, EventArgs e)
        {
            if (this.treeViewToolUsageTemplate.Nodes.Count == 0)
            {
                DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Enter usage", "Enter a new usage for the current tool", "");
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.String.Length > 0)
                {
                    System.Windows.Forms.TreeNode NTools = new TreeNode("Tools");
                    this.treeViewToolUsageTemplate.Nodes.Add(NTools);

                    System.Windows.Forms.TreeNode NTool = new TreeNode(this.textBoxName.Text);
                    DiversityWorkbench.UserControls.XMLNode XTool = new DiversityWorkbench.UserControls.XMLNode();
                    XTool.Name = "Tool";
                    XTool.Attributes = new List<DiversityWorkbench.UserControls.XMLAttribute>();
                    DiversityWorkbench.UserControls.XMLAttribute AName = new DiversityWorkbench.UserControls.XMLAttribute();
                    DiversityWorkbench.UserControls.XMLAttribute AToolID = new DiversityWorkbench.UserControls.XMLAttribute();

                    AName.Name = "Name";
                    AName.Value = this.textBoxName.Text;
                    XTool.Attributes.Add(AName);

                    AToolID.Name = "ToolID";
                    AToolID.Value = this.ID.ToString();
                    XTool.Attributes.Add(AToolID);

                    NTool.Tag = XTool;
                    NTools.Nodes.Add(NTool);

                    System.Windows.Forms.TreeNode NUsage = new TreeNode(f.String);
                    DiversityWorkbench.UserControls.XMLNode XUsage = new DiversityWorkbench.UserControls.XMLNode();
                    XUsage.Name = "Usage";
                    XUsage.Attributes = new List<DiversityWorkbench.UserControls.XMLAttribute>();
                    DiversityWorkbench.UserControls.XMLAttribute AUsage = new DiversityWorkbench.UserControls.XMLAttribute();
                    DiversityWorkbench.UserControls.XMLAttribute AValue = new DiversityWorkbench.UserControls.XMLAttribute();

                    AUsage.Name = "Name";
                    AUsage.Value = f.String;
                    XUsage.Attributes.Add(AUsage);

                    AValue.Name = "Value";
                    XUsage.Attributes.Add(AValue);

                    NUsage.Tag = XUsage;
                    NTool.Nodes.Add(NUsage);
                }
            }
            else if (this.treeViewToolUsageTemplate.SelectedNode != null && this.treeViewToolUsageTemplate.SelectedNode.Tag.GetType() == typeof(DiversityWorkbench.UserControls.XMLNode))
            {
                DiversityWorkbench.UserControls.XMLNode X = (DiversityWorkbench.UserControls.XMLNode)this.treeViewToolUsageTemplate.SelectedNode.Tag;
                if (X.Name == "Usage")
                {
                    string Title = "";
                    foreach (DiversityWorkbench.UserControls.XMLAttribute A in X.Attributes)
                    {
                        if (A.Name == "Name")
                        {
                            Title = A.Value;
                            break;
                        }
                    }
                    string ValueEnum = "";
                    DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Enter value", "Enter a new value for " + Title, "");
                    f.StartPosition = FormStartPosition.CenterParent;
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.String.Length > 0)
                    {
                        DiversityWorkbench.UserControls.XMLNode XNew = new DiversityWorkbench.UserControls.XMLNode();
                        XNew.Name = "ValueEnum";
                        XNew.Value = f.String;
                        System.Windows.Forms.TreeNode N = new TreeNode(XNew.Value);
                        N.Tag = XNew;
                        this.treeViewToolUsageTemplate.SelectedNode.Nodes.Add(N);
                    }
                }
                else if (X.Name == "Tool")
                {
                    string Title = "";
                    foreach (DiversityWorkbench.UserControls.XMLAttribute A in X.Attributes)
                    {
                        if (A.Name == "Name")
                        {
                            Title = A.Value;
                            break;
                        }
                    }
                    DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Enter usage", "Enter a new usage for " + Title, "");
                    f.StartPosition = FormStartPosition.CenterParent;
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.String.Length > 0)
                    {
                        DiversityWorkbench.UserControls.XMLNode XNew = new DiversityWorkbench.UserControls.XMLNode();
                        XNew.Name = "Usage";
                        DiversityWorkbench.UserControls.XMLAttribute AName = new DiversityWorkbench.UserControls.XMLAttribute();
                        AName.Name = "Name";
                        AName.Value = f.String;
                        XNew.Attributes = new List<DiversityWorkbench.UserControls.XMLAttribute>();
                        XNew.Attributes.Add(AName);
                        DiversityWorkbench.UserControls.XMLAttribute AValue = new DiversityWorkbench.UserControls.XMLAttribute();
                        AValue.Name = "Value";
                        XNew.Attributes.Add(AValue);
                        System.Windows.Forms.TreeNode N = new TreeNode(f.String);
                        N.Tag = XNew;
                        this.treeViewToolUsageTemplate.SelectedNode.Nodes.Add(N);
                    }
                }
            }
            this.treeViewToolUsageTemplate.ExpandAll();
            this.toolStripButtonToolsSave.Enabled = true;
            this.toolStripButtonToolsSave.BackColor = System.Drawing.Color.Red;
            this.toolStripButtonToolsSave_Click(null, null);
        }

        private void toolStripButtonToolsRemove_Click(object sender, EventArgs e)
        {
            if (this.treeViewToolUsageTemplate.SelectedNode.Tag.GetType() == typeof(DiversityWorkbench.UserControls.XMLNode))
            {
                DiversityWorkbench.UserControls.XMLNode X = (DiversityWorkbench.UserControls.XMLNode)this.treeViewToolUsageTemplate.SelectedNode.Tag;
                if (X.Name == "Usage")
                {
                    System.Windows.Forms.TreeNode P = this.treeViewToolUsageTemplate.SelectedNode.Parent;
                    P.Nodes.Remove(this.treeViewToolUsageTemplate.SelectedNode);
                }
                else if (X.Name == "ValueEnum")
                {
                    System.Windows.Forms.TreeNode P = this.treeViewToolUsageTemplate.SelectedNode.Parent;
                    P.Nodes.Remove(this.treeViewToolUsageTemplate.SelectedNode);
                }
                this.toolStripButtonToolsSave.Enabled = true;
                this.toolStripButtonToolsSave.BackColor = System.Drawing.Color.Red;
                this.toolStripButtonToolsSave_Click(null, null);
            }
        }

        private void toolStripButtonToolsEdit_Click(object sender, EventArgs e)
        {
            if (this.treeViewToolUsageTemplate.SelectedNode.Tag.GetType() == typeof(DiversityWorkbench.UserControls.XMLNode))
            {
                DiversityWorkbench.UserControls.XMLNode X = (DiversityWorkbench.UserControls.XMLNode)this.treeViewToolUsageTemplate.SelectedNode.Tag;
                DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Edit value", "Edit the value for the " + X.Name, X.Value);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.String.Length > 0)
                {
                    X.Value = f.String;
                    this.treeViewToolUsageTemplate.SelectedNode.Tag = X;
                    this.treeViewToolUsageTemplate.SelectedNode.Text = X.Value;
                }
                this.toolStripButtonToolsSave.Enabled = true;
                this.toolStripButtonToolsSave.BackColor = System.Drawing.Color.Red;
                this.toolStripButtonToolsSave_Click(null, null);
            }
        }

        private void toolStripButtonToolsSearch_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new DataTable();
            string SQL = "SELECT Name, ToolUsageTemplate " +
                "FROM Tool " +
                "WHERE (NOT (ToolUsageTemplate IS NULL)) " +
                "ORDER BY Name";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            DiversityWorkbench.Forms.FormSelectXmlTemplate f = new DiversityWorkbench.Forms.FormSelectXmlTemplate(
                dt, "Name", "ToolUsageTemplate");
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK && f.Template.Length > 0)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.toolBindingSource.Current;
                R.BeginEdit();
                string Template = f.Template;
                string TemplateStart = Template.Substring(0, Template.IndexOf("<Tool "));
                string TemplateEnd = Template.Substring(Template.IndexOf("<Usage "));
                string TemplateNew =  TemplateStart + "<Tool Name=\"" + R["Name"].ToString() +"\" ToolID=\"" + R["ToolID"].ToString() + "\">" + TemplateEnd;
                R["ToolUsageTemplate"] = TemplateNew;
                R.EndEdit();
                this.buildToolTree();
            }
        }

        private void toolStripButtonToolsSave_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.toolBindingSource.Current;
                R.BeginEdit();
                this.writeToolTreeToXml();
                R.EndEdit();
                this.toolStripButtonToolsSave.Enabled = false;
                this.toolStripButtonToolsSave.BackColor = System.Drawing.SystemColors.Control;
            }
            catch (System.Exception ex) { }
        }

        private void toolStripButtonToolUsageSchemaInfo_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT XML_SCHEMA_NAMESPACE (N'dbo', N'Tools')";
            string Schema = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            //DiversityWorkbench.Forms.FormWebBrowser W = new DiversityWorkbench.Forms.FormWebBrowser(Schema, false);
            //W.ShowDialog();
            Schema = Schema.Replace("><xsd:", ">\r\n<xsd:").Replace("></xsd:", ">\r\n</xsd:");
            DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Schema definition for the tools", Schema, true);
            f.ShowDialog();
        }

        private void treeViewToolUsageTemplate_AfterSelect(object sender, TreeViewEventArgs e)
        {
            System.Windows.Forms.TreeNode N = this.treeViewToolUsageTemplate.SelectedNode;
            this.toolStripButtonToolsSearch.Enabled = false;
            if (N.Tag == null)
            {
                this.toolStripButtonToolsAdd.Enabled = false;
                this.toolStripButtonToolsEdit.Enabled = false;
                this.toolStripButtonToolsRemove.Enabled = false;
            }
            else
            {
                if (N.Tag.GetType() == typeof(DiversityWorkbench.UserControls.XMLNode))
                {
                    DiversityWorkbench.UserControls.XMLNode X = (DiversityWorkbench.UserControls.XMLNode)N.Tag;
                    if (X.Name == "Tool")
                    {
                        this.toolStripButtonToolsAdd.Enabled = true;
                        this.toolStripButtonToolsAdd.ToolTipText = "Add a usage to the selected tool";
                        this.toolStripButtonToolsEdit.Enabled = false;
                        this.toolStripButtonToolsRemove.Enabled = false;
                    }
                    else if (X.Name == "Usage")
                    {
                        this.toolStripButtonToolsAdd.Enabled = true;
                        this.toolStripButtonToolsAdd.ToolTipText = "Add a value to the selected usage";
                        this.toolStripButtonToolsEdit.Enabled = false;
                        this.toolStripButtonToolsRemove.Enabled = true;
                        this.toolStripButtonToolsRemove.ToolTipText = "Remove the selected usage";
                    }
                    else if (X.Name == "ValueEnum")
                    {
                        this.toolStripButtonToolsAdd.Enabled = false;
                        this.toolStripButtonToolsEdit.Enabled = true;
                        this.toolStripButtonToolsEdit.ToolTipText = "Edit the selected value";
                        this.toolStripButtonToolsRemove.Enabled = true;
                        this.toolStripButtonToolsRemove.ToolTipText = "Remove the selected value";
                    }
                }
            }
        }

        public void buildToolTree()
        {
            //int i = this.ID;
            //System.Data.DataRowView R = (System.Data.DataRowView)this.toolBindingSource.Current;
            string ToolUsage = "";
            if (this.treeViewTool.SelectedNode != null && this.treeViewTool.SelectedNode.Tag != null)
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewTool.SelectedNode.Tag;
                ToolUsage = R["ToolUsageTemplate"].ToString();
            }
            else
            {
                System.Data.DataRow[] rr = this.dataSetTool.Tool.Select("ToolID = " + this.ID.ToString());
                ToolUsage = rr[0]["ToolUsageTemplate"].ToString();
            }
            if (ToolUsage.Length > 0)
            {
                DiversityCollection.Tool.BuildToolTreeFromXmlContent(this.treeViewToolUsageTemplate, ToolUsage);
                this.toolStripButtonToolsAdd.Enabled = false;
                this.toolStripButtonToolsEdit.Enabled = false;
                this.toolStripButtonToolsRemove.Enabled = false;
                this.toolStripButtonToolsSearch.Enabled = false;
            }
            else
            {
                this.treeViewToolUsageTemplate.Nodes.Clear();
                this.toolStripButtonToolsAdd.Enabled = true;
                this.toolStripButtonToolsAdd.ToolTipText = "Add a usage to the selected tool";
                this.toolStripButtonToolsSearch.Enabled = true;
                this.toolStripButtonToolsEdit.Enabled = false;
                this.toolStripButtonToolsRemove.Enabled = false;
            }
        }

        public void writeToolTreeToXml()
        {
            string XML = DiversityCollection.Tool.XmlFromToolTree(this.treeViewToolUsageTemplate);
            System.Data.DataRowView R = (System.Data.DataRowView)this.toolBindingSource.Current;
            R["ToolUsageTemplate"] = XML;
        }

        #endregion

        #region Properties

        public int ID { get { return int.Parse(this.dataSetTool.Tool.Rows[this.toolBindingSource.Position][0].ToString()); } }
        public string DisplayText { get { return this.dataSetTool.Tool.Rows[this.toolBindingSource.Position][2].ToString(); } }
        public string ToolUsageTemplate { get { return this.dataSetTool.Tool.Rows[this.toolBindingSource.Position]["ToolUsageTemplate"].ToString(); } }

        //public bool ChangeToSpecimen { get { return this.userControlSpecimenList.ChangeToSpecimen; } }
        //public int CollectionSpecimenID { get { return this.userControlSpecimenList.CollectionSpecimenID; } }

        #endregion


    }
}
