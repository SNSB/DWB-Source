using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Regulation
{
    public class Regulation
    {
        #region Parameter

        #region Data

        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterRegulation;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterRegulationIdentifier;
        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterRegulationResource;

        private string _SelectCommandRegulation = "SELECT Regulation, ParentRegulation, Description, Type, Status, ValidFrom, ValidUntil, " +
            "ContractGiver, ContractGiverAgentURI, ContractAcceptor, ContractAcceptorAgentURI, " +
            "ResponsiblePrincipleInvestigator, ResponsiblePrincipleInvestigatorAgentURI, IssuingDate, CountryOrPlace, TaxonomicGroups, Notes, HierarchyOnly " +
            "FROM Regulation";
        private string _SelectCommandRegulationIdentifier = "SELECT Regulation, Identifier FROM RegulationIdentifier";
        private string _SelectCommandRegulationResource = "SELECT Regulation, ResourceID, Resource, URI, ResourceType FROM RegulationResource";

        DiversityWorkbench.ServerConnection _ServerConnection;

        private DataSetRegulation _datasetRegulation;


        #endregion
        
        private System.Collections.Generic.List<string> _Regulations;
        private string _SelectedRegulation;

        private string _ProjectURI = "";

        #endregion

        #region Construction

        public Regulation(string Regulation)
        {
            this._Regulations = new List<string>();
            this._Regulations.Add(Regulation);
        }

        public Regulation(System.Uri ProjectURI, string Regulation = "")
        {
            this._ProjectURI = ProjectURI.AbsoluteUri;
            if (!WorkbenchUnit.GlobalWorkbenchUnitList().ContainsKey("DiversityProjects"))
            {
                DiversityWorkbench.Project P = new Project(DiversityWorkbench.Settings.ServerConnection);
            }
            this._ServerConnection = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(this._ProjectURI);
            string ProjectID = DiversityWorkbench.WorkbenchUnit.getIDFromURI(this._ProjectURI);
            string SQL = "SELECT Regulation FROM ProjectRegulation WHERE ProjectID = " + ProjectID;
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
            System.Data.DataTable dt = new System.Data.DataTable();
            ad.Fill(dt);
            this._Regulations = new List<string>();
            foreach (System.Data.DataRow R in dt.Rows)
            {
                this._Regulations.Add(R[0].ToString());
            }
            if (Regulation.Length > 0 && this._Regulations.Contains(Regulation))
                this._SelectedRegulation = Regulation;
        }
        
        #endregion

        #region Interface
        
        public System.Collections.Generic.List<string> RegulationList()
        {
            if (this._Regulations == null)
                this._Regulations = new List<string>();
            return this._Regulations;
        }

        public System.Data.DataSet DataSetRegulation()
        {
            if (this._datasetRegulation == null)
                this._datasetRegulation = new DataSetRegulation();
            return this._datasetRegulation;
        }

        public System.Data.DataRow RegulationRow(string Regulation)
        {
            this._SelectedRegulation = Regulation;
            if (this.DataSetRegulation().Tables["Regulation"].Rows.Count == 0)
                this.GetData(Regulation, this._datasetRegulation);
            System.Data.DataRow[] RR = this.DataSetRegulation().Tables["Regulation"].Select("Regulation = '" + Regulation + "'");
            if (RR.Length > 0)
            {
                return RR[0];
            }
            else
            {
                this.GetData(Regulation, this._datasetRegulation);
                System.Data.DataRow[] RRnext = this.DataSetRegulation().Tables["Regulation"].Select("Regulation = '" + Regulation + "'");
                if (RRnext.Length > 0)
                    return RRnext[0];
                else
                    return null;
            }
        }

        public UserControlRegulation UserControlRegulation(UserControlRegulation.DesignVersion Design = DiversityWorkbench.Regulation.UserControlRegulation.DesignVersion.Default, bool ReadOnly = false)
        {
            UserControlRegulation UC = new UserControlRegulation(this, Design, ReadOnly);
            if (this._SelectedRegulation == null)
                this._SelectedRegulation = this.RegulationList()[0];
            this.GetData(this._SelectedRegulation, UC.DataSet());
            if (this._ProjectURI != null && this._ProjectURI.Length > 0)
                UC.SetProjectURI(this._ProjectURI);
            return UC;
        }
        
        #endregion

        #region Auxillary

        private void GetData(string Regulation, System.Data.DataSet DS)
        {
            if (this._ServerConnection == null)
                this._ServerConnection = DiversityWorkbench.Settings.ServerConnection;
            if (this._ServerConnection != null)
            {
                try
                {
                    if (this._datasetRegulation == null)
                        this._datasetRegulation = new DataSetRegulation();
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._ServerConnection.ConnectionString);

                    string WhereClause = " WHERE Regulation = '" + Regulation + "'";

                    if (this._sqlDataAdapterRegulation == null)
                        this._sqlDataAdapterRegulation = new Microsoft.Data.SqlClient.SqlDataAdapter(this._SelectCommandRegulation + WhereClause, this._ServerConnection.ConnectionString);
                    else
                    {
                        this._sqlDataAdapterRegulation.SelectCommand.Connection = con;
                        this._sqlDataAdapterRegulation.SelectCommand.CommandText = this._SelectCommandRegulation + WhereClause;
                    }
                    this._sqlDataAdapterRegulation.Fill(DS.Tables["Regulation"]);// this._datasetRegulation.Regulation);
                    if (this._ProjectURI.Length > 0)
                    {
                        foreach (System.Data.DataRow R in DS.Tables["Regulation"].Rows)
                        {
                            R["ProjectURI"] = this._ProjectURI;
                        }
                    }

                    if (this._sqlDataAdapterRegulationIdentifier == null)
                        this._sqlDataAdapterRegulationIdentifier = new Microsoft.Data.SqlClient.SqlDataAdapter(this._SelectCommandRegulationIdentifier + WhereClause, this._ServerConnection.ConnectionString);
                    else
                    {
                        this._sqlDataAdapterRegulationIdentifier.SelectCommand.Connection = con;
                        this._sqlDataAdapterRegulationIdentifier.SelectCommand.CommandText = this._SelectCommandRegulationIdentifier + WhereClause;
                    }
                    this._sqlDataAdapterRegulationIdentifier.Fill(DS.Tables["RegulationIdentifier"]);// this._datasetRegulation.RegulationIdentifier);

                    if (this._sqlDataAdapterRegulationResource == null)
                        this._sqlDataAdapterRegulationResource = new Microsoft.Data.SqlClient.SqlDataAdapter(this._SelectCommandRegulationResource + WhereClause, this._ServerConnection.ConnectionString);
                    else
                    {
                        this._sqlDataAdapterRegulationResource.SelectCommand.Connection = con;
                        this._sqlDataAdapterRegulationResource.SelectCommand.CommandText = this._SelectCommandRegulationResource + WhereClause;
                    }
                    this._sqlDataAdapterRegulationResource.Fill(DS.Tables["RegulationResource"]);// this._datasetRegulation.RegulationResource);
                }
                catch (System.Exception ex)
                {
                }
            }
        }
        
        #endregion

    }
}
