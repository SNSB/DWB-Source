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
    public partial class FormExportToCacheDB : Form
    {
        /*
        270972 identification units imported
        Procedure started at Feb 19 2013 11:43AM
        IdentificationUnitPartCache cleared: 9 sec. 
        IdentificationUnitPartCache filled: 9 sec.
        First entry correction: 7 sec.
        Last entry correction: 3 sec.
        Identification correction: 8 sec.
        TaxonNames correction: 10 sec.
        relation correction: 7 sec.
        Typification correction: 1 sec.
        MaterialCategory correction: 4 sec.
         * 
         * Problem hat sich erledigt - Timeout hochsetzen
         * 
         * Hinweise fuer evtl. Aufteilung der Prozedur (Fehler wegen Timeout)
         * 3 Stufen: 
         * 1. Loeschen
         * 2. Fuellen
         * 3. Korrigieren - evtl. auch in Teilschritten
         * */

        private bool _SpecimenExported = false;
        private bool _UnitsExported = false;
        private bool _PartsExported = false;

        private string _CacheDatabase;

        public FormExportToCacheDB()
        {
            InitializeComponent();
            this.initForm();
        }

        private void initForm()
        {
            if (this.CacheDatabase.Length == 0)
            {
                this.splitContainerMain.Panel2Collapsed = true;
            }
            else
            {
                this.splitContainerMain.Panel1Collapsed = true;
                this.labelHeader.Text = "Export data to database " + this.CacheDatabase;
                this.setCounts();
            }
        }

        private string CacheDatabase
        {
            get
            {
                if (this._CacheDatabase != null)
                    return this._CacheDatabase;
                string SQL = "SELECT [dbo].[DiversityCollectionCacheDatabaseName] ()";
                this._CacheDatabase = DiversityWorkbench.FormFunctions.SqlExecuteScalar(SQL);
                return this._CacheDatabase;
            }
        }

        private void setCounts()
        {
            string SQL = "USE " + this.CacheDatabase +  "; SELECT COUNT(*) FROM TaxonSynonymy";
            this.textBoxTaxa.Text = DiversityWorkbench.FormFunctions.SqlExecuteScalar(SQL);
            SQL = "USE " + this.CacheDatabase + "; SELECT COUNT(*) FROM CollectionSpecimenCache";
            this.textBoxSpecimen.Text = DiversityWorkbench.FormFunctions.SqlExecuteScalar(SQL);
            SQL = "USE " + this.CacheDatabase + "; SELECT COUNT(*) FROM IdentificationUnitCache";
            this.textBoxUnit.Text = DiversityWorkbench.FormFunctions.SqlExecuteScalar(SQL);
            SQL = "USE " + this.CacheDatabase + "; SELECT COUNT(*) FROM IdentificationUnitPartCache";
            this.textBoxPart.Text = DiversityWorkbench.FormFunctions.SqlExecuteScalar(SQL);
        }

        private void buttonTaxa_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string SQL = "USE " + this.CacheDatabase + "; EXECUTE [dbo].[procRefreshTaxonSynonyms]";
            this.pictureBoxTaxa.Visible = true;
            if (!DiversityWorkbench.FormFunctions.SqlExecuteNonQuery(SQL))
            {
                //System.Windows.Forms.MessageBox.Show("Export failed");
                this.pictureBoxTaxa.Image = this.imageList.Images[1];
            }
            else 
                this.pictureBoxTaxa.Image = this.imageList.Images[0];
            this.setCounts();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonUpdateSpecimen_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            if (this.RunExport("procRefreshCollectionSpecimenCache", this.pictureBoxSpecimen))
            {
                this.RunExport("procRefreshIdentificationUnitCache", this.pictureBoxUnit);
                this.RunExport("procRefreshIdentificationUnitPartCache", this.pictureBoxPart);
            }
            this.setCounts();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private bool RunExport(string Procedure, System.Windows.Forms.PictureBox PictureBox)
        {
            bool OK = true;
            string SQL = "USE " + this.CacheDatabase + "; EXECUTE [dbo].[" + Procedure + "]";
            PictureBox.Visible = true;
            string ExceptionMessage = "";
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout(900000));
            System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                C.CommandTimeout = 100000;
                C.ExecuteNonQuery();
                PictureBox.Image = this.imageList.Images[0];
            }
            catch (System.Data.SqlClient.SqlException exS)
            {
                ExceptionMessage = exS.Message + "\r\nSQL-Statement:\r\n" + SQL + "\r\n\r\n";
                PictureBox.Image = this.imageList.Images[1];
                System.Windows.Forms.MessageBox.Show(ExceptionMessage);
                OK = false;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return OK;
        }

        private void buttonViewTaxa_Click(object sender, EventArgs e)
        {
            this.ViewTableContent("TaxonSynonymy");
        }

        private void buttonViewSpecimen_Click(object sender, EventArgs e)
        {
            this.ViewTableContent("CollectionSpecimenCache");
        }

        private void buttonViewUnits_Click(object sender, EventArgs e)
        {
            this.ViewTableContent("IdentificationUnitCache");
        }

        private void buttonViewParts_Click(object sender, EventArgs e)
        {
            this.ViewTableContent("IdentificationUnitPartCache");
        }

        private void ViewTableContent(string Table)
        {
            string SQL = "USE " + this.CacheDatabase + "; SELECT * FROM " + Table;
            string TitleForm = "Content of " + Table;
            string TitleHeader = "The content of the table " + Table + " in the cache database " + this.CacheDatabase;
            System.Data.SqlClient.SqlDataAdapter Adapter = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            DiversityWorkbench.FormEditTable f = new DiversityWorkbench.FormEditTable(Adapter, TitleForm, TitleHeader, false);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.ShowDialog();
        }

    }
}
