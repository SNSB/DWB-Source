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
    public partial class FormMethod : Form
    {
        #region Parameter

        private DiversityCollection.Method _Method ;

        #endregion

        #region Construction

        public FormMethod()
        {
            InitializeComponent();
            this.splitContainerData.Panel2Collapsed = true;
            this.splitContainerMain.Panel2.Visible = false;
            this.initForm();
            // online manual
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
        }

        public FormMethod(int? ItemID)
            : this()
        {
            if (ItemID != null)
            {
                this._Method.setItem((int)ItemID);
                this.labelHeader.Visible = false;
                this.menuStripMain.Visible = false;
                this.splitContainerMain.Panel1Collapsed = true;
                this.splitContainerMain.Panel2.Enabled = false;
            }
            else
            {
                this.userControlDialogPanel.Visible = true;
                this.labelHeader.Visible = true;
            }
        }

        public FormMethod(int? ItemID, System.Collections.Generic.List<DiversityWorkbench.UserControls.QueryRestrictionItem> Restrictions)
            : this(ItemID)
        {
            this.userControlQueryList.setQueryRestrictionList(Restrictions);
            this.userControlQueryList.StartQuery();
        }

        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        #endregion

        #region Form

        private void initForm()
        {
            System.Data.DataSet Dataset = this.dataSetMethod;
            if (this._Method == null)
                this._Method = new Method(ref Dataset, this.dataSetMethod.Method,
                    ref this.treeViewMethod, this, this.userControlQueryList, this.splitContainerMain,
                    this.splitContainerData, null, //null,
                    null, this.helpProvider, this.toolTip, ref this.methodBindingSource);
            this._Method.initForm();
            this._Method.setToolStripButtonDeleteEvent(this.toolStripButtonDelete);
            this._Method.setToolStripButtonNewEvent(this.toolStripButtonNew);
            this._Method.setToolStripButtonIncludeIDEvent(this.toolStripButtonIncludeID);
            this._Method.setToolStripButtonSetParentWithHierarchyEvent(this.toolStripButtonSetParent);
            this._Method.setToolStripButtonRemoveParentEvent(this.toolStripButtonRemoveParent);
            this._Method.setFormMethod(this);
            this.groupBoxParameterValues.Enabled = false;
            this.setPermissions();

            this.userControlQueryList.RememberSettingIsAvailable(true);
            this.userControlQueryList.RememberQuerySettingsIdentifier = "Method";
            this.userControlQueryList.RememberQueryConditionSettings_ReadFromFile();
            if (this.userControlQueryList.RememberQuerySettings() && this.userControlQueryList.ListOfIDs.Count > 0)
            {
                this.userControlQueryList.listBoxQueryResult.SelectedIndex = -1;
                this.userControlQueryList.listBoxQueryResult.SelectedIndex = this.userControlQueryList.RememberedIndex();
            }
        }

        private void FormMethod_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._Method.saveItem();

            if (this.userControlQueryList.RememberQuerySettings())
                this.userControlQueryList.RememberQueryConditionSettings_SaveToFile();
            else
                this.userControlQueryList.RememberQueryConditionSettings_RemoveFile();
        }

        private void setPermissions()
        {
            DiversityWorkbench.Forms.FormFunctions FF = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
            this._Method.setToolbarPermission(ref this.toolStripButtonDelete, "Method", "Delete");
            this._Method.setToolbarPermission(ref this.toolStripButtonNew, "Method", "Insert");
        }

        private void FormMethod_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetMethod.ParameterValue_Enum". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.parameterValue_EnumTableAdapter.Fill(this.dataSetMethod.ParameterValue_Enum);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetMethod.Parameter". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.parameterTableAdapter.Fill(this.dataSetMethod.Parameter);
        }

        public void SetFormAccordingToItem()
        {
            if (this._Method.ID != null)
            {
                string SQL = "SELECT OnlyHierarchy FROM Method WHERE MethodID = " + this._Method.ID.ToString();
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                bool EnableDetails = true;
                if (Result == "True")
                {
                    EnableDetails = false;
                }
                this.groupBoxAnalysis.Enabled = EnableDetails;
                this.groupBoxParameter.Enabled = EnableDetails;
                this.groupBoxProcessing.Enabled = EnableDetails;
                this.buttonUriOpen.Enabled = EnableDetails;
                this.textBoxURI.Enabled = EnableDetails;
            }
        }

        #endregion

        #region Menue
        
        private void toolStripMenuItemHistory_Click(object sender, EventArgs e)
        {
            if (this.dataSetMethod.Method.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
                return;
            }

            System.Data.DataRowView R = (System.Data.DataRowView)methodBindingSource.Current;
            string Title = "History of " + R["DisplayText"].ToString() + " (MethodID: " + R["MethodID"].ToString() + ")";
            try
            {
                bool HistoryPresent = false;
                System.Collections.Generic.List<System.Data.DataTable> LogTables = new List<DataTable>();

                if (this.dataSetMethod.Method.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "MethodID", this.dataSetMethod.Method.TableName, ""));
                    HistoryPresent = true;
                }
                if (this.dataSetMethod.Parameter.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "MethodID", this.dataSetMethod.Parameter.TableName, ""));
                    HistoryPresent = true;
                }
                if (this.dataSetMethod.ParameterValue_Enum.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "MethodID", this.dataSetMethod.ParameterValue_Enum.TableName, ""));
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

        private void feedbackMethodStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                this.userControlQueryList.QueryString(),
                this.ID.ToString());
        }

        private void showURIMethodStripMenuItem_Click(object sender, EventArgs e)
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
        
        private void feedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(GetType()).GetName().Version.ToString(), "", "");
        }

        private void tableEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                if (this.userControlQueryList.ListOfIDs.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Nothing selected");
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    return;
                }
                DiversityWorkbench.Forms.FormTableEditor f = new DiversityWorkbench.Forms.FormTableEditor(DiversityCollection.Resource.Tools, "Method", "MethodID");
                f.initTable(this.userControlQueryList.ListOfIDs);
                f.StartPosition = FormStartPosition.CenterParent;
                f.Width = this.Width - 10;
                f.Height = this.Height - 10;
                try
                {
                    f.ShowDialog();
                }
                catch (System.Exception ex)
                {
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "tableEditorToolStripMenuItem_Click");
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        #endregion

        #region URI

        private void buttonUriOpen_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxURI.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                int i = this.methodBindingSource.Position;
                this.dataSetMethod.Method.Rows[i]["MethodURI"] = f.URL;
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

        #region Method

        //private void textBoxDisplayText_Leave(object sender, EventArgs e)
        //{
        //    if (this.treeViewMethod.Nodes.Count > 0)
        //    {
        //        try
        //        {
        //            if (this.treeViewMethodUsageTemplate.Nodes[0].Nodes[0].Tag != null)
        //            {
        //                DiversityWorkbench.UserControls.XMLNode XN = (DiversityWorkbench.UserControls.XMLNode)this.treeViewMethodUsageTemplate.Nodes[0].Nodes[0].Tag;
        //                if (XN.Name == "Method")
        //                {
        //                    DiversityWorkbench.UserControls.XMLAttribute XAnew = new DiversityWorkbench.UserControls.XMLAttribute();
        //                    int i = 0;
        //                    foreach (DiversityWorkbench.UserControls.XMLAttribute XA in XN.Attributes)
        //                    {
        //                        if (XA.Name == "Name")
        //                        {
        //                            XAnew.Name = XA.Name;
        //                            XAnew.Value = this.textBoxDisplayText.Text;
        //                            break;
        //                        }
        //                        i++;
        //                    }
        //                    XN.Attributes.RemoveAt(i);
        //                    XN.Attributes.Add(XAnew);
        //                    this.treeViewMethodUsageTemplate.Nodes[0].Nodes[0].Text = XAnew.Value;
        //                }
        //            }
        //        }
        //        catch(System.Exception ex){}
        //    }
        //}

        #endregion

        #region MethodUsageTemplate
        
        private void toolStripButtonMethodAdd_Click(object sender, EventArgs e)
        {
            //if (this.treeViewMethodUsageTemplate.Nodes.Count == 0)
            //{
            //    DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Enter usage", "Enter a new usage for the current tool", "");
            //    f.StartPosition = FormStartPosition.CenterParent;
            //    f.ShowDialog();
            //    if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.String.Length > 0)
            //    {
            //        System.Windows.Forms.TreeNode NMethods = new TreeNode("Methods");
            //        //this.treeViewMethodUsageTemplate.Nodes.Add(NMethods);

            //        System.Windows.Forms.TreeNode NMethod = new TreeNode(this.textBoxDisplayText.Text);
            //        DiversityWorkbench.UserControls.XMLNode XMethod = new DiversityWorkbench.UserControls.XMLNode();
            //        XMethod.Name = "Method";
            //        XMethod.Attributes = new List<DiversityWorkbench.UserControls.XMLAttribute>();
            //        DiversityWorkbench.UserControls.XMLAttribute AName = new DiversityWorkbench.UserControls.XMLAttribute();
            //        DiversityWorkbench.UserControls.XMLAttribute AMethodID = new DiversityWorkbench.UserControls.XMLAttribute();

            //        AName.Name = "DisplayText";
            //        AName.Value = this.textBoxDisplayText.Text;
            //        XMethod.Attributes.Add(AName);

            //        AMethodID.Name = "MethodID";
            //        AMethodID.Value = this.ID.ToString();
            //        XMethod.Attributes.Add(AMethodID);

            //        NMethod.Tag = XMethod;
            //        NMethods.Nodes.Add(NMethod);

            //        System.Windows.Forms.TreeNode NUsage = new TreeNode(f.String);
            //        DiversityWorkbench.UserControls.XMLNode XUsage = new DiversityWorkbench.UserControls.XMLNode();
            //        XUsage.Name = "Usage";
            //        XUsage.Attributes = new List<DiversityWorkbench.UserControls.XMLAttribute>();
            //        DiversityWorkbench.UserControls.XMLAttribute AUsage = new DiversityWorkbench.UserControls.XMLAttribute();
            //        DiversityWorkbench.UserControls.XMLAttribute AValue = new DiversityWorkbench.UserControls.XMLAttribute();

            //        AUsage.Name = "DisplayText";
            //        AUsage.Value = f.String;
            //        XUsage.Attributes.Add(AUsage);

            //        AValue.Name = "Value";
            //        XUsage.Attributes.Add(AValue);

            //        NUsage.Tag = XUsage;
            //        NMethod.Nodes.Add(NUsage);
            //    }
            //}
            //else if (this.treeViewMethodUsageTemplate.SelectedNode != null && this.treeViewMethodUsageTemplate.SelectedNode.Tag.GetType() == typeof(DiversityWorkbench.UserControls.XMLNode))
            //{
            //    DiversityWorkbench.UserControls.XMLNode X = (DiversityWorkbench.UserControls.XMLNode)this.treeViewMethodUsageTemplate.SelectedNode.Tag;
            //    if (X.Name == "Usage")
            //    {
            //        string Title = "";
            //        foreach (DiversityWorkbench.UserControls.XMLAttribute A in X.Attributes)
            //        {
            //            if (A.Name == "Name")
            //            {
            //                Title = A.Value;
            //                break;
            //            }
            //        }
            //        string ValueEnum = "";
            //        DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Enter value", "Enter a new value for " + Title, "");
            //        f.StartPosition = FormStartPosition.CenterParent;
            //        f.ShowDialog();
            //        if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.String.Length > 0)
            //        {
            //            DiversityWorkbench.UserControls.XMLNode XNew = new DiversityWorkbench.UserControls.XMLNode();
            //            XNew.Name = "ValueEnum";
            //            XNew.Value = f.String;
            //            System.Windows.Forms.TreeNode N = new TreeNode(XNew.Value);
            //            N.Tag = XNew;
            //            this.treeViewMethodUsageTemplate.SelectedNode.Nodes.Add(N);
            //        }
            //    }
            //    else if (X.Name == "Method")
            //    {
            //        string Title = "";
            //        foreach (DiversityWorkbench.UserControls.XMLAttribute A in X.Attributes)
            //        {
            //            if (A.Name == "Name")
            //            {
            //                Title = A.Value;
            //                break;
            //            }
            //        }
            //        DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Enter usage", "Enter a new usage for " + Title, "");
            //        f.StartPosition = FormStartPosition.CenterParent;
            //        f.ShowDialog();
            //        if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.String.Length > 0)
            //        {
            //            DiversityWorkbench.UserControls.XMLNode XNew = new DiversityWorkbench.UserControls.XMLNode();
            //            XNew.Name = "Usage";
            //            DiversityWorkbench.UserControls.XMLAttribute AName = new DiversityWorkbench.UserControls.XMLAttribute();
            //            AName.Name = "Name";
            //            AName.Value = f.String;
            //            XNew.Attributes = new List<DiversityWorkbench.UserControls.XMLAttribute>();
            //            XNew.Attributes.Add(AName);
            //            DiversityWorkbench.UserControls.XMLAttribute AValue = new DiversityWorkbench.UserControls.XMLAttribute();
            //            AValue.Name = "Value";
            //            XNew.Attributes.Add(AValue);
            //            System.Windows.Forms.TreeNode N = new TreeNode(f.String);
            //            N.Tag = XNew;
            //            this.treeViewMethodUsageTemplate.SelectedNode.Nodes.Add(N);
            //        }
            //    }
            //}
            //this.treeViewMethodUsageTemplate.ExpandAll();
        }

        private void toolStripButtonMethodRemove_Click(object sender, EventArgs e)
        {
            //if (this.treeViewMethodUsageTemplate.SelectedNode.Tag.GetType() == typeof(DiversityWorkbench.UserControls.XMLNode))
            //{
            //    DiversityWorkbench.UserControls.XMLNode X = (DiversityWorkbench.UserControls.XMLNode)this.treeViewMethodUsageTemplate.SelectedNode.Tag;
            //    if (X.Name == "Usage")
            //    {
            //        System.Windows.Forms.TreeNode P = this.treeViewMethodUsageTemplate.SelectedNode.Parent;
            //        P.Nodes.Remove(this.treeViewMethodUsageTemplate.SelectedNode);
            //    }
            //    else if (X.Name == "ValueEnum")
            //    {
            //        System.Windows.Forms.TreeNode P = this.treeViewMethodUsageTemplate.SelectedNode.Parent;
            //        P.Nodes.Remove(this.treeViewMethodUsageTemplate.SelectedNode);
            //    }
            //}
        }

        private void toolStripButtonMethodUsageSchemaInfo_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT XML_SCHEMA_NAMESPACE (N'dbo', N'Methods')";
            string Schema = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            Schema = Schema.Replace("><xsd:", ">\r\n<xsd:").Replace("></xsd:", ">\r\n</xsd:");
            DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Schema definition for the methods", Schema, true);
            f.ShowDialog();
        }

        #endregion

        #region Properties

        public int ID { get { return int.Parse(this.dataSetMethod.Method.Rows[this.methodBindingSource.Position][0].ToString()); } }
        public string DisplayText { get { return this.dataSetMethod.Method.Rows[this.methodBindingSource.Position][2].ToString(); } }
        //public string MethodUsageTemplate { get { return this.dataSetMethod.Method.Rows[this.methodBindingSource.Position]["MethodUsageTemplate"].ToString(); } }

        #endregion

        #region MethodParameter

        private int ParameterID()
        {
            int ParameterID = -1;
            if (this.parameterBindingSource != null && this.parameterBindingSource.Current != null)
            {
                int P = 0;
                System.Data.DataRowView R = (System.Data.DataRowView)this.parameterBindingSource.Current;
                if (int.TryParse(R["ParameterID"].ToString(), out P))
                    ParameterID = P;
            }
            return ParameterID;
        }

        private System.Data.DataView _ParameterValues;
        private System.Data.DataView ParameterValues
        {
            get
            {
                if (this._ParameterValues == null)
                    this._ParameterValues = new DataView(this.dataSetMethod.ParameterValue_Enum, "ParameterID = " + this.ParameterID().ToString(), "Value", DataViewRowState.CurrentRows);
                else
                    this._ParameterValues.RowFilter = "ParameterID = " + this.ParameterID().ToString();
                return this._ParameterValues;
            }
        }
        
        private void checkBoxUseDedicatedParameterValues_Click(object sender, EventArgs e)
        {
            if (this.checkBoxUseDedicatedParameterValues.Checked)
                this.toolStripButtonParameterValueAdd_Click(null, null);
            else
            {
                if (System.Windows.Forms.MessageBox.Show("Do you want to remove all values from the list", "Remove values", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    while (this.ParameterValues.Count > 0)
                        this.ParameterValues.Delete(0);
                }
                else
                    this.checkBoxUseDedicatedParameterValues.Checked = true;
            }
            this.listBoxParameter_SelectedIndexChanged(null, null);
        }

        private void checkBoxUseDedicatedParameterValues_CheckedChanged(object sender, EventArgs e)
        {
        }
        
        private void toolStripButtonParameterAdd_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New parameter", "Please enter the name of the new parameter", "");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                DiversityCollection.Datasets.DataSetMethod.ParameterRow R = this.dataSetMethod.Parameter.NewParameterRow();
                R.DisplayText = f.String;
                R.MethodID = this.ID;
                this.dataSetMethod.Parameter.Rows.Add(R);
                this._Method.saveDependentTables();
                this._Method.setItem(this.ID);
            }
        }

        private void toolStripButtonParameterDelete_Click(object sender, EventArgs e)
        {
            int ParameterID;
            System.Data.DataRowView R = (System.Data.DataRowView)this.parameterBindingSource.Current;
            if (int.TryParse(R["ParameterID"].ToString(), out ParameterID))
            {
                string SQL = "SELECT COUNT(*) AS Expr1 " +
                    "FROM  CollectionEventParameterValue " +
                    "WHERE     Value <> '' AND     ParameterID =   " + ParameterID.ToString();
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Result != "0")
                {
                    string Message = "The parameter is " + Result + " times used for methods in collection events.\r\nDo you really want to delete it?";
                    if (System.Windows.Forms.MessageBox.Show(Message, "Delete Parameter", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                        return;
                }
                SQL = "SELECT COUNT(*) AS Expr1 " +
                    "FROM  IdentificationUnitAnalysisMethodParameter " +
                    "WHERE     Value <> '' AND     ParameterID =   " + ParameterID.ToString();
                Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Result != "0")
                {
                    string Message = "The parameter is " + Result + " times used for methods in the analysis or organisms.\r\nDo you really want to delete it?";
                    if (System.Windows.Forms.MessageBox.Show(Message, "Delete Parameter?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                        return;
                }
                SQL = "DELETE " +
                    "FROM  CollectionEventParameterValue " +
                    "WHERE ParameterID = " + ParameterID.ToString();
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                SQL = "DELETE " +
                    "FROM  IdentificationUnitAnalysisMethodParameter " +
                    "WHERE ParameterID = " + ParameterID.ToString();
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                this.parameterBindingSource.RemoveCurrent();
                this._Method.saveDependentTables();
            }
        }

        private void buttonParameterURI_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxParameterURI.Text);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                this.textBoxParameterURI.Text = f.URL;
            }
        }

        private void toolStripButtonParameterValueAdd_Click(object sender, EventArgs e)
        {
            if (this.parameterBindingSource.Current == null)
                return;
            string Parameter = "";
            System.Data.DataRowView RP = (System.Data.DataRowView)this.parameterBindingSource.Current;
            Parameter = RP["DisplayText"].ToString();
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New value", "Please enter the new value for the parameter " + Parameter, "");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    DiversityCollection.Datasets.DataSetMethod.ParameterValue_EnumRow R = this.dataSetMethod.ParameterValue_Enum.NewParameterValue_EnumRow();
                    R.DisplayText = f.String;
                    R.Value = f.String;
                    R.MethodID = this.ID;
                    R.ParameterID = int.Parse(((System.Data.DataRowView)this.parameterBindingSource.Current)["ParameterID"].ToString());
                    this.dataSetMethod.ParameterValue_Enum.Rows.Add(R);
                    this._Method.saveDependentTables();
                }
                catch (System.Exception ex) { }
            }
        }

        private void toolStripButtonParameterValueRemove_Click(object sender, EventArgs e)
        {
            this.parameterValueEnumBindingSource.RemoveCurrent();
            this._Method.saveDependentTables();
        }

        private void listBoxParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.listBoxParameterValue.DataSource = this.ParameterValues;
            this.listBoxParameterValue.DisplayMember = "Value";
            this.listBoxParameterValue.ValueMember = "Value";
            this.listBoxParameterValue_SelectedIndexChanged(null, null);
            if (this.ParameterValues.Count > 0)
            {
                this.checkBoxUseDedicatedParameterValues.Checked = true;
                this.groupBoxParameterValues.Enabled = true;
                this.comboBoxParameterDefaultValue.DropDownStyle = ComboBoxStyle.DropDownList;
                //this.comboBoxParameterDefaultValue.Items.Clear();
                System.Data.DataTable dt = this.ParameterValues.ToTable();
                System.Data.DataRow R = dt.NewRow();
                R["Value"] = "";
                R["ParameterID"] = -1;
                R["MethodID"] = -1;
                dt.Rows.Add(R);
                this.comboBoxParameterDefaultValue.DataSource = dt;
                this.comboBoxParameterDefaultValue.DisplayMember = "Value";
                this.comboBoxParameterDefaultValue.ValueMember = "Value";
                if (this.parameterBindingSource.Current != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        System.Data.DataRowView rP = (System.Data.DataRowView)this.parameterBindingSource.Current;
                        if (rP["DefaultValue"].ToString() == dt.Rows[i]["Value"].ToString())
                        {
                            this.comboBoxParameterDefaultValue.SelectedIndex = i;
                            break;
                        }
                    }
                }
                this.textBoxParameterDefaultValue.Visible = false;
                this.textBoxParameterDefaultValue.Dock = DockStyle.Left;
                this.comboBoxParameterDefaultValue.Visible = true;
                this.comboBoxParameterDefaultValue.Dock = DockStyle.Fill;
            }
            else
            {
                this.comboBoxParameterDefaultValue.DataSource = null;
                this.checkBoxUseDedicatedParameterValues.Checked = false;
                this.groupBoxParameterValues.Enabled = false;
                //this.comboBoxParameterDefaultValue.DropDownStyle = ComboBoxStyle.Simple;
                //System.Data.DataRowView rP = (System.Data.DataRowView)this.parameterBindingSource.Current;
                //this.comboBoxParameterDefaultValue.Text = rP["DefaultValue"].ToString();

                this.comboBoxParameterDefaultValue.Visible = false;
                this.comboBoxParameterDefaultValue.Dock = DockStyle.Right;
                this.textBoxParameterDefaultValue.Visible = true;
                this.textBoxParameterDefaultValue.Dock = DockStyle.Fill;
            }
        }

        private void listBoxParameterValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxParameterValue.SelectedItem == null)
                return;
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxParameterValue.SelectedItem;
            for (int i = 0; i < this.dataSetMethod.ParameterValue_Enum.Rows.Count; i++)
            {
                if (R.Row == this.dataSetMethod.ParameterValue_Enum.Rows[i])
                {
                    this.parameterValueEnumBindingSource.Position = i;
                    return;
                }
            }
        }

        private void listBoxParameter_DataSourceChanged(object sender, EventArgs e)
        {
            this.listBoxParameter_SelectedIndexChanged(null, null);
        }

        private void comboBoxParameterDefaultValue_KeyUp(object sender, KeyEventArgs e)
        {
            //System.Data.DataRowView R = (System.Data.DataRowView)this.parameterBindingSource.Current;
            //if (R["DefaultValue"].ToString() != this.comboBoxParameterDefaultValue.Text)
            //{
            //    R.BeginEdit();
            //    R["DefaultValue"] = this.comboBoxParameterDefaultValue.Text;
            //    R.EndEdit();
            //}
        }

        private void buttonParameterValueUri_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxParameterValueUri.Text);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                this.textBoxParameterValueUri.Text = f.URL;
                System.Data.DataRowView R = (System.Data.DataRowView)this.parameterValueEnumBindingSource.Current;
                R["URI"] = f.URL;
            }
        }

        private void toolStripButtonParameterSave_Click(object sender, EventArgs e)
        {
            if (this.parameterBindingSource.Current == null)
                return;
            this.labelDescription.Focus();
            System.Data.DataRowView RP = (System.Data.DataRowView)this.parameterBindingSource.Current;
            RP.BeginEdit();
            RP.EndEdit();
            this._Method.saveDependentTables();
        }

        #endregion

        #region Analysis

        private void toolStripButtonAnalysisAdd_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT DISTINCT A.AnalysisID, A.HierarchyDisplayText " +
                "FROM dbo.AnalysisHierarchyAll() A, ProjectAnalysis PA, ProjectList P " +
                "WHERE A.AnalysisID NOT IN (SELECT AnalysisID FROM MethodForAnalysis WHERE MethodID = " + this.ID.ToString() + ") " +
                "AND PA.AnalysisID = A.AnalysisID AND P.ProjectID = PA.ProjectID " +
                "ORDER BY A.HierarchyDisplayText";
            System.Data.DataTable dtAnalysis = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtAnalysis);
            System.Collections.Generic.Dictionary<string, string> Entity = DiversityWorkbench.Entity.EntityInformation("Analysis.DisplayText");
            string Header = "Please select the analysis that should be added to the method";
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtAnalysis, "HierarchyDisplayText", "AnalysisID", DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, Entity), Header);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                System.Data.DataRow R = this.dataSetMethod.MethodForAnalysis.NewRow();
                System.Data.DataRow Rlist = this.dataSetMethod.MethodForAnalysisList.NewMethodForAnalysisListRow();
                R["AnalysisID"] = int.Parse(f.SelectedValue.ToString());
                R["MethodID"] = this.ID;
                Rlist["AnalysisID"] = int.Parse(f.SelectedValue.ToString());
                Rlist["DisplayText"] = f.SelectedString;
                Rlist["MethodID"] = this.ID;
                try
                {
                    this.dataSetMethod.MethodForAnalysis.Rows.Add(R);
                    this.dataSetMethod.MethodForAnalysisList.Rows.Add(Rlist);
                }
                catch { }
            }
        }

        private void toolStripButtonAnalysisRemove_Click(object sender, EventArgs e)
        {
            try
            {
                // check IdentificationUnitAnalysisMethodParameter
                string SQL = "SELECT COUNT(*) FROM IdentificationUnitAnalysisMethodParameter AS P WHERE MethodID = " + ID.ToString();
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Result != "0")
                {
                    if (System.Windows.Forms.MessageBox.Show(Result + " datasets in table IdentificationUnitAnalysisMethodParameter are linked to the parameters of this method. Do you want to remove these data?", "Remove parameter", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        SQL = "DELETE FROM IdentificationUnitAnalysisMethodParameter WHERE MethodID = " + ID.ToString();
                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                    }
                    else
                        return;
                }

                // check IdentificationUnitAnalysisMethod
                SQL = "SELECT COUNT(*) FROM IdentificationUnitAnalysisMethod AS M WHERE MethodID = " + ID.ToString();
                Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Result != "0")
                {
                    if (System.Windows.Forms.MessageBox.Show(Result + " datasets in table IdentificationUnitAnalysisMethod are linked to this method. Do you want to remove these data?", "Remove method?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        SQL = "DELETE FROM IdentificationUnitAnalysisMethod WHERE MethodID = " + ID.ToString();
                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                    }
                    else
                        return;
                }

                int AnalysisID = int.Parse(this.listBoxAnalysis.SelectedValue.ToString());
                System.Data.DataRow[] RR = this.dataSetMethod.MethodForAnalysis.Select("AnalysisID = " + AnalysisID.ToString());
                System.Data.DataRow[] RRlist = this.dataSetMethod.MethodForAnalysisList.Select("AnalysisID = " + AnalysisID.ToString());
                if (RR.Length > 0)
                {
                    RR[0].Delete();
                    RRlist[0].Delete();
                }
            }
            catch { }
        }
        
        #endregion

        #region Processing

        private void toolStripButtonProcessingAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL = "SELECT DISTINCT P.ProcessingID, P.HierarchyDisplayText " +
                    "FROM dbo.ProcessingHierarchyAll() P, ProjectProcessing PP, ProjectList L " +
                    "WHERE P.ProcessingID NOT IN (SELECT ProcessingID FROM MethodForProcessing WHERE MethodID = " + this.ID.ToString() + ") " +
                    "AND PP.ProcessingID = P.ProcessingID " +
                    "AND L.ProjectID = PP.ProjectID " +
                    "ORDER BY P.HierarchyDisplayText";
                System.Data.DataTable dtProcessing = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtProcessing);
                if (dtProcessing.Rows.Count > 0)
                {
                    System.Collections.Generic.Dictionary<string, string> Entity = DiversityWorkbench.Entity.EntityInformation("Processing.DisplayText");
                    string Header = "Please select the processing that should be added to the method";
                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtProcessing, "HierarchyDisplayText", "ProcessingID", DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, Entity), Header);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        System.Data.DataRow R = this.dataSetMethod.MethodForProcessing.NewRow();
                        System.Data.DataRow Rlist = this.dataSetMethod.MethodForProcessingList.NewMethodForProcessingListRow();
                        R["ProcessingID"] = int.Parse(f.SelectedValue.ToString());
                        R["MethodID"] = this.ID;
                        Rlist["ProcessingID"] = int.Parse(f.SelectedValue.ToString());
                        Rlist["DisplayText"] = f.SelectedString;
                        Rlist["MethodID"] = this.ID;
                        try
                        {
                            this.dataSetMethod.MethodForProcessing.Rows.Add(R);
                            this.dataSetMethod.MethodForProcessingList.Rows.Add(Rlist);
                        }
                        catch { }
                    }
                }
                else
                    System.Windows.Forms.MessageBox.Show("No processings available");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonProcessingRemove_Click(object sender, EventArgs e)
        {
            try
            {
                // check CollectionSpecimenProcessingMethodParameter
                string SQL = "SELECT COUNT(*) FROM CollectionSpecimenProcessingMethodParameter AS P WHERE MethodID = " + ID.ToString();
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Result != "0")
                {
                    if (System.Windows.Forms.MessageBox.Show(Result + " datasets in table CollectionSpecimenProcessingMethodParameter are linked to the parameters of this method. Do you want to remove these data?", "Remove parameter", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        SQL = "DELETE FROM CollectionSpecimenProcessingMethodParameter WHERE MethodID = " + ID.ToString();
                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                    }
                    else
                        return;
                }

                // check CollectionSpecimenProcessingMethod
                SQL = "SELECT COUNT(*) FROM CollectionSpecimenProcessingMethod AS M WHERE MethodID = " + ID.ToString();
                Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Result != "0")
                {
                    if (System.Windows.Forms.MessageBox.Show(Result + " datasets in table CollectionSpecimenProcessingMethod are linked to this method. Do you want to remove these data?", "Remove method?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        SQL = "DELETE FROM CollectionSpecimenProcessingMethod WHERE MethodID = " + ID.ToString();
                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                    }
                    else
                        return;
                }

                // check MethodForProcessing
                SQL = "SELECT COUNT(*) FROM MethodForProcessing AS M WHERE MethodID = " + ID.ToString();
                Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Result != "0")
                {
                    if (System.Windows.Forms.MessageBox.Show(Result + " datasets in table MethodForProcessing are linked to this method. Do you want to remove these data?", "Remove method?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        SQL = "DELETE FROM MethodForProcessing WHERE MethodID = " + ID.ToString();
                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                    }
                    else
                        return;
                }

                int ProcessingID = int.Parse(this.listBoxProcessing.SelectedValue.ToString());
                System.Data.DataRow[] RR = this.dataSetMethod.MethodForProcessing.Select("ProcessingID = " + ProcessingID.ToString());
                System.Data.DataRow[] RRlist = this.dataSetMethod.MethodForProcessingList.Select("ProcessingID = " + ProcessingID.ToString());
                if (RR.Length > 0)
                {
                    RR[0].Delete();
                    RRlist[0].Delete();
                }
            }
            catch (System.Exception ex) 
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        
        #endregion

    }
}
