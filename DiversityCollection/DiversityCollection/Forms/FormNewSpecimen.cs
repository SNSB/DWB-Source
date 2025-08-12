using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    /// <summary>
    /// Formular zum Anlegen neuer Specimen
    /// 3 Szenarien:
    /// 0) Es soll keine AccessionNumber vergeben werden -> OK
    /// 1) keine AccessionNumber -> (3) / (0)
    /// 2) vorgegebene AccessionNumber -> (3) / (0)
    /// 3) Suche nach AccessionNumber
    ///     naechste freie suchen
    ///         freie wird gefunden
    ///             diese wird akzeptiert -> OK
    ///             gefundene wird nicht akzeptiert
    ///                 neuer Suchlauf
    ///                     es wird eine neue gefunden und akzeptiert -> OK
    ///                     es wird keine passende gefunden ->
    /// 4) Manuelle Eingabe
    /// </summary>
    public partial class FormNewSpecimen : Form
    {
        #region Parameter
        private int? _CollectionSpecimenID;
        private string _AccessionNumber = "";
        private bool _CreateNewSpecimen = true;
        #endregion

        #region Construction
        private FormNewSpecimen()
        {
            InitializeComponent();
        }

        public FormNewSpecimen(string AccessionNumber)
            : this()
        {
            this._AccessionNumber = AccessionNumber;
        }
        
        #endregion  
  
        #region AccessionNumber
        private void textBoxAccessionNumber_TextChanged(object sender, EventArgs e)
        {
            this._AccessionNumber = this.textBoxAccessionNumber.Text;
            if (this.AccessionNumberIsPresent)
            {
                this.buttonCreateNewSpecimen.Text = "Open dataset with accession number\r\n" + this._AccessionNumber;
                this.textBoxAccessionNumber.BackColor = System.Drawing.Color.Pink;
                this._CreateNewSpecimen = false;
            }
            else
            {
                string Message = "Create a new dataset";
                if (this._AccessionNumber.Length > 0) Message += " with accession number\r\n" + this._AccessionNumber;
                else Message += " without an accession number";
                this.buttonCreateNewSpecimen.Text = Message;
                this.textBoxAccessionNumber.BackColor = System.Drawing.SystemColors.Window;
                this._CreateNewSpecimen = true;
            }
        }

        private bool AccessionNumberIsPresent
        {
            get
            {
                bool IsPresent = false;
                if (this._AccessionNumber.Length == 0)
                    return true;
                string SQL = "SELECT COUNT(*) FROM CollectionSpecimen WHERE AccessionNumber = '" + this.textBoxAccessionNumber.Text + "'";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                try
                {
                    int i = 0;
                    if (!int.TryParse(C.ExecuteScalar()?.ToString(), out i))
                        IsPresent = false;
                    if (i > 0) IsPresent = false;
                }
                catch
                {
                    IsPresent = false;
                }
                con.Close();
                //if (IsPresent)
                //{
                //    this.buttonCreateNewSpecimen.Text = "Open dataset with accession number\r\n" + this._AccessionNumber;
                //    this.textBoxAccessionNumber.BackColor = System.Drawing.Color.Pink;
                //    this._CreateNewSpecimen = false;
                //}
                //else
                //{
                //    this.buttonCreateNewSpecimen.Text = "Create new dataset with accession number\r\n" + this._AccessionNumber;
                //    this.textBoxAccessionNumber.BackColor = System.Drawing.SystemColors.Window;
                //    this._CreateNewSpecimen = true;
                //}
                return IsPresent;
            }
        }
        
        #endregion

        #region Form and Buttons
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        
        private void buttonCreateNewSpecimen_Click(object sender, EventArgs e)
        {

        }

        private void buttonSearchForNewAccessionNumber_Click(object sender, EventArgs e)
        {

        }

        #endregion


        #region Public Properties
        public int? CollectionSpecimenID { get { return this._CollectionSpecimenID; } }
        public string AccessionNumber { get { return this._AccessionNumber; } }

        #endregion


    }
}
