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
    public partial class UserControlDatawithholding : UserControl
    {
        private int _NumberToPublish;

        private string _DataWithholdingReasonColumn = "DataWithholdingReason";

        public string DataWithholdingReasonColumn
        {
            get { return _DataWithholdingReasonColumn; }
            set { _DataWithholdingReasonColumn = value; }
        }

        public DiversityCollection.UserControls.UserControlDatawithholdingSummary UCSummary;

        public int NumberToPublish
        {
            get { return _NumberToPublish; }
            set { _NumberToPublish = value; }
        }
        private int _NumberBlocked;

        public int NumberBlocked
        {
            get { return _NumberBlocked; }
            set { _NumberBlocked = value; }
        }
        private string ListOfIDs;
        string Table;

        private Microsoft.Data.SqlClient.SqlConnection _SqlConnection;
        private bool _UseConnection = false;

        public UserControlDatawithholding()
        {
            InitializeComponent();
        }

        public void initUserControl(string Table, string IDs, Microsoft.Data.SqlClient.SqlConnection SqlConnection = null)
        {
            this.ListOfIDs = IDs;
            this.Table = Table;
            if (SqlConnection != null)
            {
                this._SqlConnection = SqlConnection;
                this._UseConnection = true;
            }
            this.buttonSetBlocked.Text += this.DisplayText;
            this.buttonSetToPublish.Text += this.DisplayText;
            this.initUserControl(this._UseConnection);
        }

        public void initUserControl(bool UseConnection = false)
        {
            try
            {
                string SQL = this.SqlBlocked;
                System.Data.DataTable dt = new DataTable();
                this.SqlFillTable(ref dt, SQL);
                //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                //ad.Fill(dt);
                this.listBoxBlocked.DataSource = dt;
                this.listBoxBlocked.DisplayMember = "Counter";
                this.listBoxBlocked.ValueMember = this.DataWithholdingReasonColumn;// "DataWithholdingReason";

                SQL = this.SqlToPublish;
                System.Data.DataTable dtFree = new DataTable();

                this.SqlFillTable(ref dtFree, SQL);
                //Microsoft.Data.SqlClient.SqlDataAdapter adFree;
                //if (this._UseConnection) adFree = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._SqlConnection);
                //else adFree = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                //adFree.Fill(dtFree);
                this.listBoxToPublish.DataSource = dtFree;
                string DataColumn = "";
                switch (this.Table)
                {
                    case "CollectionEvent":
                        switch(this._DataWithholdingReasonColumn)
                        {
                            case "DataWithholdingReason":
                                DataColumn = "LocalityDescription";
                                break;
                            case "DataWithholdingReasonDate":
                                DataColumn = "CollectionDate";
                                break;
                        }
                        break;
                    case "CollectionSpecimen":
                        DataColumn = "AccessionNumber";
                        break;
                    case "CollectionAgent":
                        DataColumn = "CollectorsName";
                        break;
                    case "CollectionSpecimenPart":
                        DataColumn = "StorageLocation";
                        break;
                    case "IdentificationUnit":
                        DataColumn = "LastIdentificationCache";
                        break;
                    case "CollectionEventImage":
                    case "CollectionEventSeriesImage":
                    case "CollectionSpecimenImage":
                    case "CollectionImage":
                        DataColumn = "URI";
                        break;
                }
                this.listBoxToPublish.DisplayMember = DataColumn;
                this.listBoxToPublish.ValueMember = DataColumn;

                SQL = "SELECT COUNT(*) " + this.SqlFromClause + " AND (RTRIM(T." + this.DataWithholdingReasonColumn + ") = '' OR T." + this.DataWithholdingReasonColumn + " IS NULL) ";
                string NumberToPublish = this.SqlExecuteScalar(SQL);// DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                int.TryParse(NumberToPublish, out this._NumberToPublish);
                this.labelToPublish.Text = "Number of " + this.DisplayText.Trim() + " without a withholding reason: " + NumberToPublish;
                SQL = "SELECT COUNT(*) " + this.SqlFromClause + " AND (RTRIM(T." + this.DataWithholdingReasonColumn + ") <> '') ";
                string NumberBlocked = this.SqlExecuteScalar(SQL);// DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                int.TryParse(NumberBlocked, out this._NumberBlocked);
                if (this.UCSummary != null)
                {
                    this.UCSummary.initUserControl(this.NumberToPublish, this.NumberBlocked, this.DisplayText.Trim());
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void SqlFillTable(ref System.Data.DataTable dt, string SQL)
        {
            Microsoft.Data.SqlClient.SqlDataAdapter ad;
            if (this._UseConnection) ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._SqlConnection);
            else ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
            try
            {
                ad.Fill(dt);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private string SqlExecuteScalar(string SQL)
        {
            string Result = "";
            try
            {
                if (this._UseConnection) Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, _SqlConnection);
                else Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return Result;
        }

        private bool SqlExecuteNonQuery(string SQL)
        {
            bool OK = true;
            try
            {
                if (this._UseConnection) OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, _SqlConnection);
                else OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
            catch (System.Exception ex)
            {
                OK = false;
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return OK;
        }


        public string DisplayText
        {
            get
            {
                string DisplayText = "";
                switch (Table)
                {
                    case "CollectionEvent":
                        switch (_DataWithholdingReasonColumn)
                        {
                            case "DataWithholdingReason":
                                DisplayText = " events";
                                break;
                            case "DataWithholdingReasonDate":
                                DisplayText = " collection dates";
                                break;
                        }
                        break;
                    case "CollectionSpecimen":
                        DisplayText = " specimens";
                        break;
                    case "CollectionAgent":
                        DisplayText = " agents";
                        break;
                    case "CollectionSpecimenPart":
                        DisplayText = " parts";
                        break;
                    case "IdentificationUnit":
                        DisplayText = " organisms";
                        break;
                    case "CollectionEventImage":
                        DisplayText = " event images";
                        break;
                    case "CollectionEventSeriesImage":
                        DisplayText = " series images";
                        break;
                    case "CollectionSpecimenImage":
                        DisplayText = " specimen images";
                        break;
                    case "CollectionImage":
                        DisplayText = " collection images";
                        break;
                }
                return DisplayText;
            }
        }

        private string SqlBlocked
        {
            get
            {
                string SQL = "SELECT T." + this.DataWithholdingReasonColumn + " +  '   [' + CAST(COUNT(*) as varchar) + ' x]' AS Counter,  T." + this.DataWithholdingReasonColumn + " ";
                SQL += this.SqlFromClause;
                SQL += " AND RTRIM(T." + this.DataWithholdingReasonColumn + ") <> '' " +
                    "GROUP BY T." + this.DataWithholdingReasonColumn + " " +
                    "ORDER BY T." + this.DataWithholdingReasonColumn + "";
                return SQL;
            }
        }

        private string SqlToPublish
        {
            get
            {
                string SQL = "SELECT ";
                switch (this.Table)
                {
                    case "CollectionEvent":
                        switch (this._DataWithholdingReasonColumn)
                        {
                            case "DataWithholdingReason":
                                SQL += "T.LocalityDescription +  '   [' + CAST(COUNT(*) as varchar) + ' x]' AS LocalityDescription, T.CollectionEventID  ";
                                break;
                            case "DataWithholdingReasonDate":
                                SQL += "case when CollectionYear is null then '' else cast(CollectionYear as varchar) end + '-' + " +
                                    "case when CollectionMonth is null then '' else cast(CollectionMonth as varchar) end + '-' + " +
                                    "case when CollectionDay is null then '' else cast(CollectionDay as varchar) end " +
                                    "+  '   [' + CAST(COUNT(*) as varchar) + ' x]' AS CollectionDate, T.CollectionEventID  ";
                                break;
                        }
                        break;
                    case "CollectionSpecimen":
                        SQL += "CASE WHEN T.AccessionNumber IS NULL THEN '[ID: ' + CAST(T.CollectionSpecimenID AS varchar) + ']' ELSE T.AccessionNumber END AS AccessionNumber, T.CollectionSpecimenID ";
                        break;
                    case "CollectionSpecimenPart":
                        SQL += "CASE WHEN S.AccessionNumber IS NULL THEN '[ID: ' + CAST(S.CollectionSpecimenID AS varchar) + ']' ELSE S.AccessionNumber END + ' | ' + T.MaterialCategory + CASE WHEN T.StorageLocation IS NULL THEN '' ELSE ' | ' + T.StorageLocation END AS StorageLocation, T.SpecimenPartID ";
                        break;
                    case "CollectionAgent":
                        SQL += "T.CollectorsName, T.CollectionSpecimenID ";
                        break;
                    case "IdentificationUnit":
                        SQL += "T.LastIdentificationCache, T.CollectionSpecimenID, T.IdentificationUnitID ";
                        break;
                    case "CollectionEventImage":
                        SQL += "T.URI, T.CollectionEventID ";
                        break;
                    case "CollectionEventSeriesImage":
                        SQL += "T.URI, T.SeriesID ";
                        break;
                    case "CollectionSpecimenImage":
                        SQL += "T.URI, T.CollectionSpecimenID ";
                        break;
                    case "CollectionImage":
                        SQL += "T.URI, T.CollectionID ";
                        break;
                }
                SQL += this.SqlFromClause;
                SQL += " AND (RTRIM(T." + this.DataWithholdingReasonColumn + ") = '' OR T." + this.DataWithholdingReasonColumn + " IS NULL) " +
                    "GROUP BY ";
                switch (this.Table)
                {
                    case "CollectionEvent":
                        switch (this._DataWithholdingReasonColumn)
                        {
                            case "DataWithholdingReason":
                                SQL += "T.LocalityDescription, T.CollectionEventID ORDER BY T.LocalityDescription  ";
                                break;
                            case "DataWithholdingReasonDate":
                                SQL += "CollectionYear, CollectionMonth, CollectionDay, T.CollectionEventID  ORDER BY CollectionYear, CollectionMonth, CollectionDay";
                                break;
                        }
                        break;
                    case "CollectionSpecimen":
                        SQL += "T.AccessionNumber, T.CollectionSpecimenID ORDER BY T.AccessionNumber ";
                        break;
                    case "CollectionSpecimenPart":
                        SQL += "T.MaterialCategory, T.StorageLocation, T.AccessionNumber, T.CollectionSpecimenID, T.SpecimenPartID, S.AccessionNumber, S.CollectionSpecimenID ORDER BY StorageLocation ";
                        break;
                    case "CollectionAgent":
                        SQL += "T.CollectorsName, T.CollectionSpecimenID ORDER BY T.CollectorsName ";
                        break;
                    case "IdentificationUnit":
                        SQL += "T.LastIdentificationCache, T.CollectionSpecimenID, T.IdentificationUnitID ORDER BY T.LastIdentificationCache ";
                        break;
                    case "CollectionEventImage":
                        SQL += "T.URI, T.CollectionEventID ORDER BY T.URI ";
                        break;
                    case "CollectionEventSeriesImage":
                        SQL += "T.URI, T.SeriesID ORDER BY T.URI ";
                        break;
                    case "CollectionSpecimenImage":
                        SQL += "T.URI, T.CollectionSpecimenID ORDER BY T.URI ";
                        break;
                    case "CollectionImage":
                        SQL += "T.URI, T.CollectionID ORDER BY T.URI ";
                        break;
                }
                return SQL;
            }
        }

        private string SqlFromClause
        {
            get
            {
                string SQL = "";
                switch (this.Table)
                {
                    case "CollectionEvent":
                        SQL += "FROM CollectionEvent AS T, CollectionSpecimen AS S ";
                        if (_UseConnection)
                            SQL += " INNER JOIN #CollectionSpecimenIDS IDs ON IDs.CollectionSpecimenID = S.CollectionSpecimenID WHERE T.CollectionEventID = S.CollectionEventID ";
                        else
                            SQL += " WHERE (S.CollectionSpecimenID IN (" + this.ListOfIDs + ")) AND T.CollectionEventID = S.CollectionEventID ";
                        break;
                    case "CollectionSpecimen":
                        SQL += "FROM CollectionSpecimen AS T ";
                        if (_UseConnection)
                            SQL += " INNER JOIN #CollectionSpecimenIDS IDs ON IDs.CollectionSpecimenID = T.CollectionSpecimenID WHERE 1=1 ";
                        else
                            SQL += "WHERE (T.CollectionSpecimenID IN (" + this.ListOfIDs + ")) ";
                        break;
                    case "CollectionSpecimenPart":
                        SQL += "FROM CollectionSpecimenPart AS T ";
                        if (_UseConnection)
                            SQL += "INNER JOIN CollectionSpecimen AS S ON S.CollectionSpecimenID = T.CollectionSpecimenID INNER JOIN #CollectionSpecimenIDS IDs ON IDs.CollectionSpecimenID = S.CollectionSpecimenID WHERE 1=1 ";
                        else
                            SQL += ", CollectionSpecimen AS S WHERE T.CollectionSpecimenID = S.CollectionSpecimenID AND (T.CollectionSpecimenID IN (" + this.ListOfIDs + ")) ";
                        break;
                    case "IdentificationUnit":
                        SQL += "FROM IdentificationUnit AS T";
                        if (_UseConnection)
                            SQL += " INNER JOIN #CollectionSpecimenIDS IDs ON IDs.CollectionSpecimenID = T.CollectionSpecimenID WHERE 1=1 ";
                        else
                            SQL += "WHERE T.CollectionSpecimenID = S.CollectionSpecimenID AND (T.CollectionSpecimenID IN (" + this.ListOfIDs + ")) ";
                        break;
                    case "CollectionAgent":
                        SQL += "FROM CollectionAgent AS T ";
                        if (_UseConnection)
                            SQL += " INNER JOIN #CollectionSpecimenIDS IDs ON IDs.CollectionSpecimenID = T.CollectionSpecimenID WHERE 1=1 ";
                        else
                            SQL += "WHERE (T.CollectionSpecimenID IN (" + this.ListOfIDs + ")) ";
                        break;
                    case "CollectionEventImage":
                        SQL += "FROM CollectionEvent AS E  ";
                        if (_UseConnection)
                        {
                            SQL += "INNER JOIN CollectionEventImage AS T ON T.CollectionEventID = E.CollectionEventID " +
                                " INNER JOIN CollectionSpecimen AS S ON T.CollectionEventID = S.CollectionEventID " +
                                " INNER JOIN #CollectionSpecimenIDS IDs ON IDs.CollectionSpecimenID = S.CollectionSpecimenID " +
                                " AND T.CollectionEventID = E.CollectionEventID " +
                                " AND E.CollectionEventID = S.CollectionEventID WHERE 1=1 ";
                        }
                        else
                        {
                            SQL += ", CollectionEventImage AS T, CollectionSpecimen AS S " +
                                " WHERE (S.CollectionSpecimenID IN (" + this.ListOfIDs + ")) " +
                                " AND T.CollectionEventID = E.CollectionEventID " +
                                " AND E.CollectionEventID = S.CollectionEventID ";
                        }
                        break;
                    case "CollectionEventSeriesImage":
                        SQL += "FROM CollectionEvent AS E, CollectionEventSeriesImage AS T, " +
                        "CollectionSpecimen AS S  ";
                        if (_UseConnection)
                        {
                            SQL += " INNER JOIN #CollectionSpecimenIDS IDs ON IDs.CollectionSpecimenID = S.CollectionSpecimenID " +
                                " WHERE T.SeriesID = E.SeriesID AND E.CollectionEventID = S.CollectionEventID ";
                        }
                        else
                        {
                            SQL += " WHERE (S.CollectionSpecimenID IN (" + this.ListOfIDs + ")) " +
                                " AND T.SeriesID = E.SeriesID AND E.CollectionEventID = S.CollectionEventID ";
                        }
                        break;
                    case "CollectionSpecimenImage":
                        SQL += "FROM CollectionSpecimenImage AS T ";
                        if (_UseConnection)
                            SQL += " INNER JOIN #CollectionSpecimenIDS IDs ON IDs.CollectionSpecimenID = T.CollectionSpecimenID WHERE 1=1";
                        else
                            SQL += "WHERE (T.CollectionSpecimenID IN (" + this.ListOfIDs + ")) ";
                        break;
                    case "CollectionImage":
                        SQL += "FROM CollectionImage AS T " +
                        "WHERE T.CollectionID IN (select P.CollectionID from CollectionSpecimenPart P ";
                        if (_UseConnection)
                            SQL += " INNER JOIN #CollectionSpecimenIDS IDs ON IDs.CollectionSpecimenID = P.CollectionSpecimenID) ";
                        else
                            SQL += "WHERE P.CollectionSpecimenID IN (" + this.ListOfIDs + ")) ";
                        break;
                }
                return SQL;
            }
        }

        private string SqlFromClauseSingle(System.Data.DataRowView R, bool Block)
        {
            string SQL = "";
            if (Block)
            {
                switch (this.Table)
                {
                    case "CollectionEvent":
                        SQL = "FROM CollectionEvent AS T WHERE CollectionEventID = " + R["CollectionEventID"].ToString();
                        break;
                    case "CollectionSpecimen":
                        SQL = "FROM CollectionSpecimen AS T WHERE CollectionSpecimenID = " + R["CollectionSpecimenID"].ToString();
                        break;
                    case "CollectionSpecimenPart":
                        SQL = "FROM CollectionSpecimenPart AS T WHERE SpecimenPartID = " + R["SpecimenPartID"].ToString();
                        break;
                    case "CollectionAgent":
                        SQL = "FROM CollectionAgent AS T WHERE CollectionSpecimenID = " + R["CollectionSpecimenID"].ToString() + " AND CollectorsName = '" + R["CollectorsName"].ToString() + "'";
                        break;
                    case "IdentificationUnit":
                        SQL = "FROM IdentificationUnit AS T WHERE CollectionSpecimenID = " + R["CollectionSpecimenID"].ToString() + " AND IdentificationUnitID = " + R["IdentificationUnitID"].ToString();
                        break;
                    case "CollectionEventImage":
                        SQL = "FROM CollectionEventImage AS T WHERE CollectionEventID = " + R["CollectionEventID"].ToString() + " AND URI = '" + R["URI"].ToString() + "'";
                        break;
                    case "CollectionEventSeriesImage":
                        SQL = "FROM CollectionEventSeriesImage AS T WHERE SeriesID = " + R["SeriesID"].ToString() + " AND URI = '" + R["URI"].ToString() + "'";
                        break;
                    case "CollectionSpecimenImage":
                        SQL = "FROM CollectionSpecimenImage AS T WHERE CollectionSpecimenID = " + R["CollectionSpecimenID"].ToString() + " AND URI = '" + R["URI"].ToString() + "'";
                        break;
                    case "CollectionImage":
                        SQL = "FROM CollectionImage AS T WHERE CollectionID = " + R["CollectionID"].ToString() + " AND URI = '" + R["URI"].ToString() + "'";
                        break;
                }
            }
            else
            {
                switch (this.Table)
                {
                    case "CollectionEvent":
                        SQL += "FROM CollectionEvent AS T, " +
                        "CollectionSpecimen AS S ";
                        if (_UseConnection)
                            SQL += " INNER JOIN #CollectionSpecimenIDS IDs ON IDs.CollectionSpecimenID = S.CollectionSpecimenID WHERE T.CollectionEventID = S.CollectionEventID";
                        else
                            SQL += "WHERE (S.CollectionSpecimenID IN (" + this.ListOfIDs + ")) AND T.CollectionEventID = S.CollectionEventID ";
                        SQL += "AND T." + this.DataWithholdingReasonColumn + " = '" + R[this.DataWithholdingReasonColumn].ToString() + "'";
                        break;
                    case "CollectionSpecimen":
                        SQL += "FROM CollectionSpecimen AS T ";
                        if (_UseConnection)
                            SQL += " INNER JOIN #CollectionSpecimenIDS IDs ON IDs.CollectionSpecimenID = T.CollectionSpecimenID WHERE 1=1 ";
                        else
                            SQL += "WHERE (T.CollectionSpecimenID IN (" + this.ListOfIDs + ")) ";
                        SQL += "AND T.DataWithholdingReason = '" + R[this.DataWithholdingReasonColumn].ToString() + "'";
                        break;
                    case "CollectionSpecimenPart":
                        SQL += "FROM CollectionSpecimenPart AS T";
                        if (_UseConnection)
                            SQL += "INNER JOIN CollectionSpecimen AS S ON S.CollectionSpecimenID = T.CollectionSpecimenID INNER JOIN #CollectionSpecimenIDS IDs ON IDs.CollectionSpecimenID = S.CollectionSpecimenID WHERE 1=1 ";
                        else
                            SQL += ", CollectionSpecimen AS S " +
                        "WHERE T.CollectionSpecimenID = S.CollectionSpecimenID AND (T.CollectionSpecimenID IN (" + this.ListOfIDs + ")) ";
                        SQL += "AND T.DataWithholdingReason = '" + R[this.DataWithholdingReasonColumn].ToString() + "'";
                        break;
                    case "CollectionAgent":
                        SQL += "FROM CollectionAgent AS T ";
                        if (_UseConnection)
                            SQL += " INNER JOIN #CollectionSpecimenIDS IDs ON IDs.CollectionSpecimenID = T.CollectionSpecimenID WHERE 1=1 ";
                        else
                            SQL += "WHERE (T.CollectionSpecimenID IN (" + this.ListOfIDs + ")) ";
                        SQL += "AND T.DataWithholdingReason = '" + R[this.DataWithholdingReasonColumn].ToString() + "'";
                        break;
                    case "CollectionEventImage":
                        SQL += "FROM CollectionEvent AS E";
                        if (_UseConnection)
                            SQL += "INNER JOIN CollectionEventImage AS T ON T.CollectionEventID = E.CollectionEventID INNER JOIN CollectionSpecimen AS S ON T.CollectionEventID = S.CollectionEventID " +
                                "INNER JOIN #CollectionSpecimenIDS IDs ON IDs.CollectionSpecimenID = S.CollectionSpecimenID WHERE 1=1 ";
                        else
                            SQL += ", CollectionEventImage AS T, " +
                            "CollectionSpecimen AS S " +
                            "WHERE (S.CollectionSpecimenID IN (" + this.ListOfIDs + ")) " +
                            "AND T.CollectionEventID = E.CollectionEventID " +
                            "AND E.CollectionEventID = S.CollectionEventID ";
                        SQL += "AND T.DataWithholdingReason = '" + R[this.DataWithholdingReasonColumn].ToString() + "'";
                        break;
                    case "CollectionEventSeriesImage":
                        SQL += "FROM CollectionEvent AS E, CollectionEventSeriesImage AS T, " +
                        "CollectionSpecimen AS S  ";
                        if (_UseConnection)
                            SQL += " INNER JOIN #CollectionSpecimenIDS IDs ON IDs.CollectionSpecimenID = S.CollectionSpecimenID WHERE 1=1 ";
                        else
                            SQL += "WHERE (S.CollectionSpecimenID IN (" + this.ListOfIDs + ")) ";
                        SQL +=  "AND T.SeriesID = E.SeriesID AND E.CollectionEventID = S.CollectionEventID " + 
                            "AND T.DataWithholdingReason = '" + R[this.DataWithholdingReasonColumn].ToString() + "'";
                        break;
                    case "CollectionSpecimenImage":
                        SQL += "FROM CollectionSpecimenImage AS T ";
                        if (_UseConnection)
                            SQL += " INNER JOIN #CollectionSpecimenIDS IDs ON IDs.CollectionSpecimenID = T.CollectionSpecimenID WHERE 1=1";
                        else
                            SQL += "WHERE (T.CollectionSpecimenID IN (" + this.ListOfIDs + ")) ";
                        SQL += "AND T.DataWithholdingReason = '" + R[this.DataWithholdingReasonColumn].ToString() + "'";
                        break;
                    case "CollectionImage":
                        SQL += "FROM CollectionImage AS T " +
                        "WHERE T.CollectionID IN (select P.CollectionID from CollectionSpecimenPart P ";
                        if (_UseConnection)
                            SQL += " INNER JOIN #CollectionSpecimenIDS IDs ON IDs.CollectionSpecimenID = P.CollectionSpecimenID) WHERE ";
                        else
                            SQL += "WHERE P.CollectionSpecimenID IN (" + this.ListOfIDs + ")) AND ";
                        SQL += " T.DataWithholdingReason = '" + R[this.DataWithholdingReasonColumn].ToString() + "'";
                        break;
                }
            }
            return SQL;
        }

        private void buttonSetBlocked_Click(object sender, EventArgs e)
        {
            if (this.textBoxBlockedReason.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please enter a reason for withholding the data");
                return;
            }
            string SQL = "UPDATE T SET T." + this.DataWithholdingReasonColumn + " = CASE WHEN T." + this.DataWithholdingReasonColumn + " IS NULL OR RTRIM(T." + this.DataWithholdingReasonColumn + ") = '' THEN '' ELSE T." + this.DataWithholdingReasonColumn + " + ' | ' END + '" + this.textBoxBlockedReason.Text + "' " +
                this.SqlFromClause;
            if (this.SqlExecuteNonQuery(SQL))// DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                this.initUserControl();
            else System.Windows.Forms.MessageBox.Show("Update failed");
        }

        private void buttonSetToPublish_Click(object sender, EventArgs e)
        {
            string SQL = "UPDATE T SET T." + this.DataWithholdingReasonColumn + " = '' " +
                this.SqlFromClause;
            if (this.SqlExecuteNonQuery(SQL))// DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                this.initUserControl();
            else System.Windows.Forms.MessageBox.Show("Update failed");
        }

        private void buttonSetBlockedSingle_Click(object sender, EventArgs e)
        {
            if (this.textBoxBlockedReason.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please enter a reason for withholding the data");
                return;
            }
            if (this.listBoxToPublish.SelectedItems.Count > 0)
            {
                foreach (System.Object O in this.listBoxToPublish.SelectedItems)
                {
                    string SQL = "UPDATE T SET T." + this.DataWithholdingReasonColumn + " = CASE WHEN T." + this.DataWithholdingReasonColumn + " IS NULL OR RTRIM(T." + this.DataWithholdingReasonColumn + ") = '' THEN '' ELSE T." + this.DataWithholdingReasonColumn + " + ' | ' END + '" + this.textBoxBlockedReason.Text + "' " +
                    this.SqlFromClauseSingle((System.Data.DataRowView)O, true);
                    this.SqlExecuteNonQuery(SQL); // DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                }
            }
            this.initUserControl();
        }

        private void buttonSetToPublishSingle_Click(object sender, EventArgs e)
        {
            string SQL = "UPDATE T SET T." + this.DataWithholdingReasonColumn + " = '' " +
                this.SqlFromClauseSingle((System.Data.DataRowView)this.listBoxBlocked.SelectedItem, false);
            if (this.SqlExecuteNonQuery(SQL))// DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                this.initUserControl();
            else System.Windows.Forms.MessageBox.Show("Update failed");
        }
    }
}
