using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityWorkbench.Archive
{
    public partial class FormCreateSchema : Form
    {

        #region Parameter

        private string _DirectoryForArchive = "";
        private DiversityWorkbench.Archive.DataArchive _DataArchive;
        System.Collections.Generic.List<string> _Excludes;

        #endregion

        #region Construction
        public FormCreateSchema(System.Collections.Generic.List<string> Excludes)
        {
            InitializeComponent();
            this.labelHeader.Text = DiversityWorkbench.Settings.DatabaseName + " Vers.: " + this.Version();
            this._Excludes = Excludes;
            this.ListTables();
            this._DirectoryForArchive = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Archive);
            this.textBoxArchiveFolder.Text = this._DirectoryForArchive;
        }

        #endregion

        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private string _Version;
        private string Version()
        {
            if (this._Version == null)
            {
                string SQL = "select dbo.Version()";
                this._Version = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            }
            return this._Version;
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        private void buttonListTables_Click(object sender, EventArgs e)
        {
            this.ListTables();
            //this._DataArchive = new DataArchive(this.Tables());
            //foreach (System.Collections.Generic.KeyValuePair<string, Archive.Table> KV in this._DataArchive.getTables())
            //{
            //    this.checkedListBoxTables.Items.Add(KV.Key, true);
            //}
        }

        private void ListTables()
        {
            this.checkedListBoxTables.Items.Clear();
            foreach(System.Collections.Generic.KeyValuePair<string, string> KV in this.Tables())
            {
                this.checkedListBoxTables.Items.Add(KV.Key, true);
            }
            foreach(string Ex in this._Excludes)
            {
                this.setTableListChecked(false, Ex);
            }
        }

        private System.Collections.Generic.Dictionary<string, string> Tables()
        {
            System.Collections.Generic.Dictionary<string, string> TT = new Dictionary<string, string>();
            string SQL = "SELECT T.TABLE_NAME FROM INFORMATION_SCHEMA.TABLES T " +
                "WHERE T.TABLE_TYPE = 'BASE TABLE' " +
                "ORDER BY T.TABLE_NAME";
            System.Data.DataTable dt = new DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            foreach(System.Data.DataRow R in dt.Rows)
            {
                TT.Add(R[0].ToString(), "");
            }
            return TT;
        }

        private void buttonCreateSchema_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            foreach(System.Object o in this.checkedListBoxTables.CheckedItems)
            {
                DiversityWorkbench.Archive.Table T = new Table(o.ToString());
                T.WriteSchema(this.ArchiveFolder(), this.Version());
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
            System.Windows.Forms.MessageBox.Show("Schemata created");
        }

        private void buttonDirectory_Click(object sender, EventArgs e)
        {
            if (System.IO.Directory.Exists(this.ArchiveFolder()))
                System.Diagnostics.Process.Start("explorer.exe", this.ArchiveFolder());
        }

        private void buttonAddToSelection_Click(object sender, EventArgs e)
        {
            if (this.textBoxAddToSelection.Text.Length > 0)
                this.setTableListChecked(true, this.textBoxAddToSelection.Text);
        }

        private void buttonRemoveFromSelection_Click(object sender, EventArgs e)
        {
            if (this.textBoxRemoveFromSelection.Text.Length > 0)
                this.setTableListChecked(false, this.textBoxRemoveFromSelection.Text);
        }

        private void setTableListChecked(bool Add, string Pattern)
        {
            string PatternCore = Pattern.Replace("*", "");
            for (int i = 0; i < this.checkedListBoxTables.Items.Count; i++)
            {
                string Item = this.checkedListBoxTables.Items[i].ToString();
                if (Item == Pattern
                    || (Pattern.StartsWith("*") && Item.EndsWith(PatternCore))
                    || (Pattern.EndsWith("*") && Item.StartsWith(PatternCore))
                    || (Pattern.StartsWith("*") && Pattern.EndsWith("*") && Item.IndexOf(PatternCore) > -1))
                    this.checkedListBoxTables.SetItemChecked(i, Add);
            }
        }

        private void checkBoxTableListAll_Click(object sender, EventArgs e)
        {
            this.setTableListChecked(true);
        }

        private void checkBoxTableListNone_Click(object sender, EventArgs e)
        {
            this.setTableListChecked(false);
        }

        private void setTableListChecked(bool Checked)
        {
            for (int i = 0; i < this.checkedListBoxTables.Items.Count; i++)
                this.checkedListBoxTables.SetItemChecked(i, Checked);
        }

        private string ArchiveFolder()
        {
            string Folder = "";
            Folder = this.textBoxArchiveFolder.Text;
            Folder += DiversityWorkbench.Settings.ModuleName + "_";
            Folder += this.Version().Replace(".", "") + "_";
            Folder += System.DateTime.Now.ToString("yyyy-MM-dd");//.Year.ToString();
            //if (System.DateTime.Now.Month < 10)
            //    Folder += "0";
            //Folder += System.DateTime.Now.Month.ToString();
            //if (System.DateTime.Now.Day < 10)
            //    Folder += "0";
            //Folder += System.DateTime.Now.Day.ToString();
            System.IO.DirectoryInfo D = new System.IO.DirectoryInfo(Folder);
            if (!D.Exists)
                D.Create();
            return Folder;
        }


    }
}
