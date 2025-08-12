using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityCollection.Tasks
{
    public partial class FormIPMobile : Form
    {
        public FormIPMobile()
        {
            InitializeComponent();
            this.textBoxScanner.Text = "25";
        }

        private void initGridViewResults(int CollectionTaskID)
        {
            try
            {
                this.dataGridViewResults.RowHeadersVisible = false;
                this.dataGridViewResults.AllowUserToAddRows = false;
                this.dataGridViewResults.AllowUserToDeleteRows = false;

                string SQL = "SELECT C.DisplayText FROM [dbo].[CollectionHierarchyAll]() C INNER JOIN CollectionTask T ON T.CollectionID = C.CollectionID AND T.CollectionTaskID = " + CollectionTaskID.ToString();
                this.labelCollectionTask.Text = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);

                this.labelDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");

                this.dataGridViewResults.Rows.Clear();
                //IPM iPM = new IPM();
                DataGridViewCellStyle s = this.dataGridViewResults.DefaultCellStyle;
                s.WrapMode = DataGridViewTriState.True;
                this.dataGridViewResults.Columns[0].DefaultCellStyle = s;
                //this.dataGridViewResults.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                // filling taxa if missing
                //if (Settings.Default.PestNameUris != null && Settings.Default.PestNameUris.Count == 0)
                //{
                //    foreach (System.Collections.Generic.KeyValuePair<string, Taxon> keyValue in iPM.GetPestTaxa())
                //        Settings.Default.PestNameUris.Add(keyValue.Key);
                //    Settings.Default.Save();
                //}

                // filling the grid results
                int i = 0;
                //foreach (System.Collections.Generic.KeyValuePair<string, Taxon> keyValue in iPM.GetPestTaxa())
                //{
                //    if (Settings.Default.PestNameUris != null && !Settings.Default.PestNameUris.Contains(keyValue.Key) && keyValue.Value.NameID > -1)
                //        continue;
                //    this.dataGridViewResults.Rows.Add(1);
                //    if (keyValue.Value.NameID > -1)
                //        this.dataGridViewResults.Rows[i].Height = 50;
                //    else
                //    {
                //        this.dataGridViewResults.Rows[i].Height = 20;
                //        this.dataGridViewResults.Rows[i].ReadOnly = true;
                //        this.dataGridViewResults.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.DarkGray;
                //    }
                //    this.dataGridViewResults.Rows[i].Cells[0].Value = keyValue.Value.Group;
                //    this.dataGridViewResults.Rows[i].Cells[1].Value = keyValue.Value.DisplayText();
                //    if (keyValue.Value.Icones != null && keyValue.Value.Icones.Count > 0)
                //    {
                //        this.dataGridViewResults.Rows[i].Cells[2].Value = keyValue.Value.Icones[0].Icon;
                //    }
                //    else
                //    {
                //        this.dataGridViewResults.Rows[i].Cells[2].Value = DiversityCollection.Resource.CheckYes;
                //    }
                //    this.dataGridViewResults.Rows[i].Tag = keyValue.Key;
                //    i++;
                //}
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void textBoxScanner_TextChanged(object sender, EventArgs e)
        {
            int ID;
            if (int.TryParse(this.textBoxScanner.Text, out ID))
                this.initGridViewResults(ID);
        }
    }
}
