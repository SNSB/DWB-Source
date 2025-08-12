using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Export
{
    public partial class UserControlTableColumnLink : UserControl
    {

        #region Parameter

        private Export.TableColumn _TableColumn;
        private Export.iExporter _iExporter;
        private string _UnitValue;

        #endregion

        #region Construction

        public UserControlTableColumnLink(DiversityWorkbench.Export.TableColumn C, string UnitValue, Export.iExporter iExporter)
        {
            InitializeComponent();
            this._TableColumn = C;
            this._UnitValue = UnitValue;
            this.labelValue.Text = this._UnitValue;
            this.labelSource.Text = this._TableColumn.UnitValues[0].SourceDisplayText;
            this._iExporter = iExporter;
        }
        
        #endregion

        #region Adding content

        private void buttonAddLink_Click(object sender, EventArgs e)
        {
            Export.FileColumn F = new FileColumn(this._TableColumn);
            if (Exporter.FileColumns.Count == 0)
                F.Position = 0;
            else
                F.Position = Exporter.FileColumnList.Last().Value.Position + 1;
            Exporter.FileColumns.Add(F);
            Exporter.ResetFileColumnList();
            this._iExporter.ShowFileColumns();
        }

        private void buttonAddUnitValue_Click(object sender, EventArgs e)
        {
            try
            {
                string DiversityWorkbenchModule = this._UnitValue.Substring(this._UnitValue.IndexOf("Diversity"));
                System.Collections.Generic.Dictionary<string, string> DiversityWorkbenchModuleSources = new Dictionary<string, string>();
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.WorkbenchUnit> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList())
                {
                    if (KV.Key == DiversityWorkbenchModule)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KVconn in KV.Value.ServerConnectionList())
                        {
                            if (KVconn.Key.Length > 0 && KVconn.Value.BaseURL != null && !DiversityWorkbenchModuleSources.ContainsKey(KVconn.Value.BaseURL))
                            {
                                DiversityWorkbenchModuleSources.Add(KVconn.Value.BaseURL, KVconn.Key);
                            }
                        }
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KVservice in KV.Value.AccessibleDatabasesAndServicesOfModule())
                        {
                            if (KV.Key != DiversityWorkbench.Settings.ServerConnection.ModuleName)
                            {
                                if (!KV.Value.ServerConnectionList().ContainsKey(KVservice.Key))
                                {
                                    DiversityWorkbenchModuleSources.Add(KV.Value.DatabaseAndServiceURIs()[KVservice.Key], KVservice.Key);
                                }
                            }
                        }
                    }
                }
                DiversityWorkbench.FormGetStringFromList fsource = new FormGetStringFromList(DiversityWorkbenchModuleSources, "Select Service", "Please select the service where the data should be taken from", true);
                fsource.ShowDialog();
                if (fsource.DialogResult == DialogResult.OK)
                {
                    DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit = null;
                    switch (DiversityWorkbenchModule)
                    {
                        case "DiversityAgents":
                            Agent A = new Agent(DiversityWorkbench.Settings.ServerConnection);
                            IWorkbenchUnit = A;
                            break;
                        case "DiversityCollection":
                            CollectionSpecimen C = new CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
                            IWorkbenchUnit = C;
                            break;
                        case "DiversityDescriptions":
                            Description D = new Description(DiversityWorkbench.Settings.ServerConnection);
                            IWorkbenchUnit = D;
                            break;
                        case "DiversityExsiccatae":
                            Exsiccate E = new Exsiccate(DiversityWorkbench.Settings.ServerConnection);
                            IWorkbenchUnit = E;
                            break;
                        case "DiversityGazetteer":
                            DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].getServerConnection();
                            Gazetteer G = new Gazetteer(SC);
                            IWorkbenchUnit = G;
                            break;
                        case "DiversityProjects":
                            Project P = new Project(DiversityWorkbench.Settings.ServerConnection);
                            IWorkbenchUnit = P;
                            break;
                        case "DiversityReferences":
                            Reference R = new Reference(DiversityWorkbench.Settings.ServerConnection);
                            IWorkbenchUnit = R;
                            break;
                        case "DiversitySamplingPlots":
                            SamplingPlot SP = new SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                            IWorkbenchUnit = SP;
                            break;
                        case "DiversityScientificTerms":
                            ScientificTerm ST = new ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                            IWorkbenchUnit = ST;
                            break;
                        case "DiversityTaxonNames":
                            TaxonName T = new TaxonName(DiversityWorkbench.Settings.ServerConnection);
                            IWorkbenchUnit = T;
                            break;
                    }

                    DiversityWorkbench.ServerConnection S;
                    if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[DiversityWorkbenchModule].ServerConnectionList().ContainsKey(fsource.SelectedString))
                    {
                        S = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[DiversityWorkbenchModule].ServerConnectionList()[fsource.SelectedString];
                        IWorkbenchUnit.setServerConnection(S);
                    }
                    System.Collections.Generic.List<string> DiversityWorkbenchModulePossibleUnitValues = new List<string>();
                    System.Collections.Generic.Dictionary<string, string> Values = IWorkbenchUnit.UnitValues(-1);
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Values)
                    {
                        if (!DiversityWorkbenchModulePossibleUnitValues.Contains(KV.Key))
                            DiversityWorkbenchModulePossibleUnitValues.Add(KV.Key);
                    }
                    DiversityWorkbench.FormGetStringFromList f = new FormGetStringFromList(DiversityWorkbenchModulePossibleUnitValues, "Select column", "Please select the column", true);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        Export.TableColumnUnitValue U = new TableColumnUnitValue(this._TableColumn, this._UnitValue + "»" + f.String);
                        this._TableColumn.UnitValues.Add(U);
                        this._iExporter.ShowCurrentSourceTableColumns(this._TableColumn.Table);
                    }

                }


                return;

                if (this._TableColumn.DiversityWorkbenchModule != null && this._TableColumn.DiversityWorkbenchModule.Length > 0)
                {
                    // Removing services with no additional information
                    System.Collections.Generic.Dictionary<string, string> Services = this._TableColumn.DiversityWorkbenchModuleSources;
                    //System.Collections.Generic.List<string> ServiceToRemove = new List<string>();
                    //foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Services)
                    //{
                    //    if (KV.Value == "MycoBank")
                    //        ServiceToRemove.Add(KV.Key);
                    //}
                    //foreach(string R in ServiceToRemove)
                    //    Services.Remove(R);

                    DiversityWorkbench.FormGetStringFromList f = new FormGetStringFromList(Services, "Select Service", "Please select the service where the data should be taken from", true);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK && f.SelectedString != null)
                    {
                        this._TableColumn.setWorkbenchUnitConnection(f.SelectedString);
                        bool ServiceFound = false;
                        foreach (DiversityWorkbench.DatabaseService d in this._TableColumn.IWorkbenchUnit.DatabaseServices())
                        {
                            if (d.IsListInDatabase && false)
                            {
                            }
                            else
                            {
                                if (d.DisplayText == f.SelectedString || d.DatabaseOnServer == f.SelectedString)
                                {
                                    System.Collections.Generic.List<string> L = this._TableColumn.getDiversityWorkbenchModulePossibleUnitValues(d.DisplayText);
                                    DiversityWorkbench.FormGetStringFromList fValue = new FormGetStringFromList(L, "Value", "Please select the value that should be used", true);
                                    fValue.ShowDialog();
                                    if (fValue.DialogResult == DialogResult.OK)
                                    {
                                        Export.TableColumnUnitValue U = new TableColumnUnitValue(this._TableColumn, f.SelectedValue, fValue.String, f.SelectedString);
                                        this._TableColumn.UnitValues.Add(U);
                                        this._iExporter.ShowCurrentSourceTableColumns(this._TableColumn.Table);
                                    }
                                    ServiceFound = true;
                                    break;
                                }
                            }
                        }
                        if (!ServiceFound && DiversityWorkbench.LinkedServer.LinkedServers().Count > 0)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.LinkedServer> KV in DiversityWorkbench.LinkedServer.LinkedServers())
                            {
                                if (f.SelectedString.StartsWith("[" + KV.Key + "]."))
                                {
                                    foreach (System.Collections.Generic.KeyValuePair<string, LinkedServerDatabase> KVDB in KV.Value.Databases())
                                    {
                                        if (f.SelectedString.EndsWith(KVDB.Key))
                                        {
                                            System.Collections.Generic.List<string> L = this._TableColumn.getDiversityWorkbenchModulePossibleUnitValues(f.SelectedString);
                                            DiversityWorkbench.FormGetStringFromList fValue = new FormGetStringFromList(L, "Value", "Please select the value that should be used", true);
                                            fValue.ShowDialog();
                                            if (fValue.DialogResult == DialogResult.OK)
                                            {
                                                Export.TableColumnUnitValue U = new TableColumnUnitValue(this._TableColumn, f.SelectedValue, fValue.String, f.SelectedString);
                                                this._TableColumn.UnitValues.Add(U);
                                                this._iExporter.ShowCurrentSourceTableColumns(this._TableColumn.Table);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
                else
                {
                    DiversityWorkbench.FormGetStringFromList f = new FormGetStringFromList(this._TableColumn.getForeignRelationTableColumns(), "Select column");
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        Export.TableColumnUnitValue U = new TableColumnUnitValue(this._TableColumn, f.String);
                        this._TableColumn.UnitValues.Add(U);
                        this._iExporter.ShowCurrentSourceTableColumns(this._TableColumn.Table);
                    }
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

    }
}
