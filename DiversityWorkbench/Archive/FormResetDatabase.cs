using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Archive
{
    public partial class FormResetDatabase : Form
    {

        #region Parameter

        private System.Collections.Generic.Dictionary<Archive.Table, Archive.UserControlTable> _Tables;
        
        #endregion

        #region Construction

        public FormResetDatabase(System.Collections.Generic.List<string> Tables)
        {
            InitializeComponent();
            this._Tables = new Dictionary<Archive.Table, Archive.UserControlTable>();
            foreach (string s in Tables)
            {
                Archive.Table T = new Table(s);
                Archive.UserControlTable U = new UserControlTable(T);
                U.CountData();
                U.setClearTableInfo("");
                this.panelTables.Controls.Add(U);
                U.Dock = DockStyle.Top;
                U.BringToFront();
                this._Tables.Add(T, U);
            }
        }
        
        #endregion

        #region Form

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Are you sure that you want to remove all data from the database?", "Clean database", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                return;
            bool OK = true;
            string Message = "";
            this.labelState.Text = "Removing keys";
            this.progressBar.Value = 0;
            this.progressBar.Maximum = this._Tables.Count;
            this.progressBar.Visible = true;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            foreach (System.Collections.Generic.KeyValuePair<Archive.Table, Archive.UserControlTable> KV in this._Tables)
            {
                Message += KV.Key.RemoveForeignKeys();
                KV.Value.setResetState(UserControlTable.ResetState.KeysRemove);
                if (this.progressBar.Value < this.progressBar.Maximum)
                    this.progressBar.Value++;
                Application.DoEvents();
            }
            this.progressBar.Value = 0;
            this.labelState.Text = "Removing data";
            this.labelState.BackColor = System.Drawing.Color.Yellow;
            foreach (System.Collections.Generic.KeyValuePair<Archive.Table, Archive.UserControlTable> KV in this._Tables)
            {
                string Error = KV.Key.ClearTable();
                Message += Error;
                KV.Value.setClearTableInfo(Error);
                if (Message.Length > 0) OK = false;
                KV.Value.CountData();
                KV.Value.setResetState(UserControlTable.ResetState.DataRemoved);
                if (this.progressBar.Value < this.progressBar.Maximum)
                    this.progressBar.Value++;
                Application.DoEvents();
            }
            this.progressBar.Visible = false;
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelState.BackColor = System.Drawing.Color.LightGreen;
            Message = "Reset finished";
            if (!OK) Message += " with errors";
            System.Windows.Forms.MessageBox.Show(Message);
            this.labelState.Text = Message;
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        #endregion

    }
}
