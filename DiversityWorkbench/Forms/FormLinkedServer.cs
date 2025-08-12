using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormLinkedServer : Form
    {

        #region Parameter

        private string _CurrentServer = "";
        private string _CurrentDatabase = "";

        #endregion

        #region Construction and form

        public FormLinkedServer()
        {
            InitializeComponent();
            this.initForm();
        }

        private void initForm()
        {
            this.Text += " on " + DiversityWorkbench.Settings.DatabaseServer;
            this.initTree();
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        #endregion

        #region Public functions

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        #endregion

        #region Tree

        private void initTree()
        {
            this.treeViewServer.Nodes.Clear();
            if (DiversityWorkbench.LinkedServer.LinkedServers().Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.LinkedServer> KV in DiversityWorkbench.LinkedServer.LinkedServers())
                {
                    System.Windows.Forms.TreeNode NLink = new TreeNode(KV.Key);
                    NLink.SelectedImageIndex = 0;
                    NLink.ImageIndex = 0;
                    NLink.ForeColor = System.Drawing.Color.Gray;
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.LinkedServerDatabase> KVdb in KV.Value.Databases())
                    {
                        System.Windows.Forms.TreeNode Ndb = new TreeNode(KVdb.Key);
                        Ndb.ImageIndex = 1;
                        Ndb.SelectedImageIndex = 1;
                        Ndb.ForeColor = System.Drawing.Color.Gray;
                        Ndb.Tag = KVdb.Key;
                        NLink.Nodes.Add(Ndb);
                    }
                    //foreach (string DB in KV.Value.DatabaseList())
                    //{
                    //    System.Windows.Forms.TreeNode Ndb = new TreeNode(DB);
                    //    Ndb.ImageIndex = 1;
                    //    Ndb.SelectedImageIndex = 1;
                    //    Ndb.ForeColor = System.Drawing.Color.Gray;
                    //    Ndb.Tag = DB;
                    //    NLink.Nodes.Add(Ndb);
                    //}
                    this.treeViewServer.Nodes.Add(NLink);
                }
            }
            this.treeViewServer.ExpandAll();
        }

        private void treeViewServer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (treeViewServer.SelectedNode.Tag != null)
                {
                    this._CurrentDatabase = treeViewServer.SelectedNode.Tag.ToString();
                    this.labelDatabase.Text = this._CurrentDatabase;
                    this._CurrentServer = this.treeViewServer.SelectedNode.Parent.Text;
                    if (this.listBoxTables.DataSource != null)
                        this.listBoxTables.DataSource = null;//.Items.Clear();
                    if (this.listBoxViews.DataSource != null)
                        this.listBoxViews.DataSource = null; //.Items.Clear();

                    if (this._CurrentServer.Length > 0 && this._CurrentDatabase.Length > 0)
                    {
                        string SQL = "select Table_name " +
                            "from [" + this._CurrentServer + "].[" + this._CurrentDatabase + "].Information_Schema.Tables T " +
                            "where T.Table_Type = 'BASE TABLE' " +
                            "order by Table_name";
                        System.Data.DataTable dtTables = new DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dtTables);
                        this.listBoxTables.DataSource = dtTables;
                        this.listBoxTables.DisplayMember = "Table_name";
                        this.listBoxTables.ValueMember = "Table_name";

                        // Views
                        System.Data.DataTable dtViews = new DataTable();
                        SQL = "select Table_name " +
                            "from [" + this._CurrentServer + "].[" + this._CurrentDatabase + "].Information_Schema.Tables T " +
                            "where T.Table_Type = 'VIEW' " +
                            "order by Table_name";
                        ad.SelectCommand.CommandText = SQL;
                        ad.Fill(dtViews);
                        // testing the views - to slow
                        //System.Collections.Generic.List<System.Data.DataRow> ViewsToRemove = new List<DataRow>();
                        //foreach (System.Data.DataRow R in dtViews.Rows)
                        //{
                        //    SQL = "SELECT TOP 1 * FROM [" + this._CurrentServer + "].[" + this._CurrentDatabase + "].dbo." + R[0].ToString();
                        //    int i;
                        //    try
                        //    {
                        //        if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out i))
                        //            ViewsToRemove.Add(R);
                        //    }
                        //    catch (System.Exception ex)
                        //    {
                        //        ViewsToRemove.Add(R);
                        //    }
                        //}
                        //foreach (System.Data.DataRow R in ViewsToRemove)
                        //    R.Delete();
                        this.listBoxViews.DataSource = dtViews;
                        this.listBoxViews.DisplayMember = "Table_name";
                        this.listBoxViews.ValueMember = "Table_name";
                    }
                    this.toolStripButtonDeleteServer.Enabled = false;
                }
                else
                {
                    this._CurrentServer = this.treeViewServer.SelectedNode.Text;
                    this.toolStripButtonDeleteServer.Enabled = true;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Data inspection

        private string TopClause()
        {
            string Top = "";
            if (this.checkBoxRestrictToNumber.Checked)
                Top = " TOP " + this.numericUpDownRestriction.Value.ToString() + " ";
            return Top;
        }

        private void listBoxViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxViews.SelectedItem == null)
                return;
            try
            {
                this.labelViewError.Visible = false;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxViews.SelectedItem;
                string View = R[0].ToString();
                string SQL = "SELECT " + this.TopClause() + " * FROM [" + this._CurrentServer + "].[" + this._CurrentDatabase + "].dbo." + View;
                System.Data.DataTable dtView = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtView);
                this.dataGridViewView.DataSource = dtView;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "FormLinkedServer.listBoxViews_SelectedIndexChanged");
                this.dataGridViewView.DataSource = null;
                this.labelViewError.Visible = true;
                this.labelViewError.Text = "Error in view. Please turn to the administrator";
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void listBoxTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxTables.SelectedItem == null)
                return;
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxTables.SelectedItem;
                string Table = R[0].ToString();
                string SQL = "SELECT " + this.TopClause() + " * FROM [" + this._CurrentServer + "].[" + this._CurrentDatabase + "].dbo." + Table;
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                this.dataGridViewTable.DataSource = dt;
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "FormLinkedServer.listBoxTables_SelectedIndexChanged");
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        #endregion

        #region Adding and dropping server

        private void toolStripButtonAddServer_Click(object sender, EventArgs e)
        {
            string tntServer = global::DiversityWorkbench.Properties.Settings.Default.TNTServer;
            DiversityWorkbench.Forms.FormGetString f = new FormGetString("Server name", "Please enter the name of the linked server, e.g. " + tntServer, "TNT.DIVERSITYWORKBENCH.DE,5432");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.String.Length > 0)
            {
                DiversityWorkbench.Forms.FormGetString fLogin = new FormGetString("Login", "Please enter the name of the remote login", "TNT");
                fLogin.ShowDialog();
                if (fLogin.DialogResult == System.Windows.Forms.DialogResult.OK && fLogin.String.Length > 0)
                {
                    DiversityWorkbench.Forms.FormGetPassword fP = new FormGetPassword(fLogin.String);
                    fP.ShowDialog();
                    if (fP.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        if (DiversityWorkbench.LinkedServer.AddLinkedServer(f.String, fLogin.String, fP.Password()))
                        {
                            this.initForm();
                        }
                    }
                }
            }
        }

        private void toolStripButtonDeleteServer_Click(object sender, EventArgs e)
        {
            if (this.treeViewServer.SelectedNode == null)
                return;
            if (this.treeViewServer.SelectedNode.Tag != null)
                return;
            string message = $"Do you really want to delete the linked server '{_CurrentServer}'?";
            if (MessageBox.Show(message, "Delete linked server", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                LinkedServer.ResetLinkedServer();
                if (DiversityWorkbench.LinkedServer.DropLinkedServer(this._CurrentServer))
                    this.initForm();
            }
        }

        #endregion

    }
}
