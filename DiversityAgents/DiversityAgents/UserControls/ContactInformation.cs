using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityAgents.UserControls
{

    public partial class ContactInformation : UserControl
    {

        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterAgentContact;
        public ContactInformation(int AgentID, int DisplayOrder)
        {
            InitializeComponent();
            this.initControl(AgentID, DisplayOrder);
        }

        public void initControl(int AgentID, int DisplayOrder)
        {
            string SQL = "SELECT TOP 1 AgentID, DisplayOrder, ParentName, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, ValidFrom, ValidUntil, LogUpdatedWhen " +
                "FROM dbo.AgentContactInformation_Public() " +
                "WHERE (AgentID = " + AgentID.ToString() + ") ORDER BY DisplayOrder ";// AND (DisplayOrder = " + DisplayOrder.ToString() + ")";
            DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterAgentContact, this.dataSetAgent.AgentContactInformation, SQL, DiversityWorkbench.Settings.ConnectionString);
            this.tableLayoutPanelContact.Enabled = true;
        }

        public bool ContainsData()
        {
            if (this.dataSetAgent.AgentContactInformation.Rows.Count > 0)
                return true;
            else
                return false;
        }
    }
}
