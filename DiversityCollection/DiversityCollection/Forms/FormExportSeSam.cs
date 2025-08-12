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
    public partial class FormExportSeSam : Form
    {
        #region Parameter

        private System.Collections.Generic.List<int> _IDs;
        private string _WhereClause;
        private System.Collections.Generic.Dictionary<string, string> _ExportValues;
        
        #endregion

        #region Construction
        
        public FormExportSeSam(System.Collections.Generic.List<int> IDs)
        {
            InitializeComponent();
            this._IDs = IDs;
            this.initForm();
        }
        
        #endregion

        #region Form
        
        private void initForm()
        {
            this.initList();
        }
        
        private void buttonStart_Click(object sender, EventArgs e)
        {
            this.ExportData();
        }

        private void initList()
        {
            System.Data.DataTable dtSex = new DataTable();
            string SQL = "SELECT DISTINCT Gender " +
                " FROM IdentificationUnit " +
                " WHERE (Gender <> N'') AND " + this.WhereClause +
                " ORDER BY Gender";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtSex);
            if (dtSex.Rows.Count == 0)
            {
                this.labelIsFemale.Visible = false;
                this.labelIsMale.Visible = false;
                this.labelIsWorker.Visible = false;
                this.checkedListBoxFemale.Visible = false;
                this.checkedListBoxMale.Visible = false;
                this.checkedListBoxWorker.Visible = false;
            }
            else
            {
                foreach (System.Data.DataRow R in dtSex.Rows)
                {
                    this.checkedListBoxFemale.Items.Add(R[0].ToString(), false);
                    this.checkedListBoxMale.Items.Add(R[0].ToString(), false);
                    this.checkedListBoxWorker.Items.Add(R[0].ToString(), false);
                }
            }
            SQL = "select c.TABLE_NAME + '.' + C.COLUMN_NAME "+
                "from INFORMATION_SCHEMA.COLUMNS " +
                "c, INFORMATION_SCHEMA.TABLES T where c.COLUMN_NAME like '%Notes%' " +
                "and C.TABLE_NAME NOT LIKE '%_log%' " +
                "and C.TABLE_NAME NOT LIKE 'Analysis%' " +
                "and C.TABLE_NAME = T.TABLE_NAME " +
                "and T.TABLE_TYPE = 'BASE TABLE' " +
                "and  (T.TABLE_NAME Like 'CollectionEvent%' " +
                "OR T.TABLE_NAME Like 'CollectionSpecimen%' " +
                "OR T.TABLE_NAME Like 'CollectionAgent%' " +
                "OR T.TABLE_NAME Like 'Identification%') " +
                "ORDER BY c.TABLE_NAME + '.' + C.COLUMN_NAME";
            System.Data.DataTable dtNotes = new DataTable();
            ad.SelectCommand.CommandText = SQL;
            ad.Fill(dtNotes);
            foreach (System.Data.DataRow R in dtNotes.Rows)
                this.checkedListBoxNote.Items.Add(R[0].ToString(), false);
        }

        #endregion

        #region Export
        
        private void ExportData()
        {
            foreach (int i in this._IDs)
                this.ExportSpecimen(i);
        }

        private void ExportSpecimen(int CollectionSpecimenID)
        {

        }
        
        #endregion

        #region Auxillary
        
        private System.Collections.Generic.Dictionary<string, string> ExportValues
        {
            get
            {
                if (this._ExportValues == null)
                {
                    this._ExportValues = new Dictionary<string, string>();
                    ExportValues.Add("Katalognummer1", "");
                    ExportValues.Add("Katalognummer2", "");
                    ExportValues.Add("Katalognummer3", "");
                    ExportValues.Add("Ordnung", "");
                    ExportValues.Add("Unterordnung", "");
                    ExportValues.Add("Ueberfamilie", "");
                    ExportValues.Add("Familie", "");
                    ExportValues.Add("Unterfamilie", "");
                    ExportValues.Add("Tribus", "");
                    ExportValues.Add("TypusNEU", "");
                    ExportValues.Add("Gattung", "");
                    ExportValues.Add("Untergattung", "");
                    ExportValues.Add("Art", "");
                    ExportValues.Add("Unterart", "");
                    ExportValues.Add("Varietaet", "");
                    ExportValues.Add("Form", "");
                    ExportValues.Add("AutorJahr_Art", "");
                    ExportValues.Add("AutorJahr_Unterart", "");
                    ExportValues.Add("AutorJahr_Varietaet", "");
                    ExportValues.Add("AutorJahr_Form", "");
                    ExportValues.Add("Bestimmungsunsicherheit", "");
                    ExportValues.Add("Sammler", "");
                    ExportValues.Add("Sammeldatum", "");
                    ExportValues.Add("Sammeldatum_Freitext", "");
                    ExportValues.Add("Erwerbsart", "");
                    ExportValues.Add("erworben_von", "");
                    ExportValues.Add("ex_Sammlung", "");
                    ExportValues.Add("Land", "");
                    ExportValues.Add("Provinz", "");
                    ExportValues.Add("Fundortbeschreibung", "");
                    ExportValues.Add("Bearbeitungszustand", "");
                    ExportValues.Add("Anzahl_weiblich", "");
                    ExportValues.Add("Anzahl_maennlich", "");
                    ExportValues.Add("Anzahl_Exemplare", "");
                    ExportValues.Add("Anzahl_Arbeiterinnen", "");
                    ExportValues.Add("Standort", "");
                    ExportValues.Add("Bestimmer", "");
                    ExportValues.Add("Bemerkung_Bestimmung", "");
                    ExportValues.Add("Bestimmungsdatum_Freitext", "");
                    ExportValues.Add("Bemerkungen", "");
                    ExportValues.Add("Erfassungsdatum", "");
                }
                return this._ExportValues;
            }
        }

        private void ClearExportValues()
        {
            this.ExportValues["Katalognummer1"] = "";
            this.ExportValues["Katalognummer2"] = "";
            this.ExportValues["Katalognummer3"] = "";
            this.ExportValues["Ordnung"] = "";
            this.ExportValues["Unterordnung"] = "";
            this.ExportValues["Ueberfamilie"] = "";
            this.ExportValues["Familie"] = "";
            this.ExportValues["Unterfamilie"] = "";
            this.ExportValues["Tribus"] = "";
            this.ExportValues["TypusNEU"] = "";
            this.ExportValues["Gattung"] = "";
            this.ExportValues["Untergattung"] = "";
            this.ExportValues["Art"] = "";
            this.ExportValues["Unterart"] = "";
            this.ExportValues["Varietaet"] = "";
            this.ExportValues["Form"] = "";
            this.ExportValues["AutorJahr_Art"] = "";
            this.ExportValues["AutorJahr_Unterart"] = "";
            this.ExportValues["AutorJahr_Varietaet"] = "";
            this.ExportValues["AutorJahr_Form"] = "";
            this.ExportValues["Bestimmungsunsicherheit"] = "";
            this.ExportValues["Sammler"] = "";
            this.ExportValues["Sammeldatum"] = "";
            this.ExportValues["Sammeldatum_Freitext"] = "";
            this.ExportValues["Erwerbsart"] = "";
            this.ExportValues["erworben_von"] = "";
            this.ExportValues["ex_Sammlung"] = "";
            this.ExportValues["Land"] = "";
            this.ExportValues["Provinz"] = "";
            this.ExportValues["Fundortbeschreibung"] = "";
            this.ExportValues["Bearbeitungszustand"] = "";
            this.ExportValues["Anzahl_weiblich"] = "";
            this.ExportValues["Anzahl_maennlich"] = "";
            this.ExportValues["Anzahl_Exemplare"] = "";
            this.ExportValues["Anzahl_Arbeiterinnen"] = "";
            this.ExportValues["Standort"] = "";
            this.ExportValues["Bestimmer"] = "";
            this.ExportValues["Bemerkung_Bestimmung"] = "";
            this.ExportValues["Bestimmungsdatum_Freitext"] = "";
            this.ExportValues["Bemerkungen"] = "";
            this.ExportValues["Erfassungsdatum"] = "";

        }

        private string WhereClause
        {
            get
            {
                if (this._WhereClause == null)
                {
                    this._WhereClause = "";
                    foreach (int i in this._IDs)
                    {
                        if (this._WhereClause.Length > 0)
                            this._WhereClause += ", ";
                        this._WhereClause += i.ToString();
                    }
                    this._WhereClause = " CollectionSpecimenID IN (" + this._WhereClause + ")";
                }
                return this._WhereClause;
            }
        }
        
        #endregion
    }
}
