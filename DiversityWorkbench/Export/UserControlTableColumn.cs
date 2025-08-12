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
    public partial class UserControlTableColumn : UserControl
    {

        #region Parameter

        private Export.TableColumn _TableColumn;
        private Export.iExporter _iExporter;

        #endregion

        #region Construction

        public UserControlTableColumn(DiversityWorkbench.Export.TableColumn C, Export.iExporter iExporter)
        {
            try
            {
                InitializeComponent();
                this._TableColumn = C;
                this._iExporter = iExporter;
                this.labelColumName.Text = C.DisplayText;
                this.setControls();
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Control

        private void setControls()
        {
            try
            {
                this.labelColumName.Enabled = true;
                this.comboBoxGrouping.Visible = true;
                this.pictureBoxGrouping.Visible = true;
                this.numericUpDownSorting.Visible = true;
                this.buttonSorting.Visible = true;

                // Filter
                this.buttonFilter.Visible = true;
                if (this._TableColumn.Filter == null || this._TableColumn.Filter.Length == 0)
                    this.buttonFilter.BackColor = System.Drawing.Color.Transparent;
                else this.buttonFilter.BackColor = System.Drawing.Color.Red;

                if (this._TableColumn.RowFilter == null || this._TableColumn.RowFilter.Length == 0)
                    this.buttonFilterRow.BackColor = System.Drawing.Color.Transparent;
                else this.buttonFilterRow.BackColor = System.Drawing.Color.Red;

                // vorerst abgeschaltet
                // Grouping
                if (1 == 0 && this._TableColumn.Table.IsGrouped)
                {
                    this.comboBoxGrouping.Visible = true;
                    if (this._TableColumn.ColumnGrouping == TableColumn.Grouping.None)
                        this._TableColumn.ColumnGrouping = TableColumn.Grouping.GroupBy;
                    switch (this._TableColumn.ColumnGrouping)
                    {
                        case TableColumn.Grouping.None:
                            this.pictureBoxGrouping.Image = null;
                            break;
                        case TableColumn.Grouping.GroupBy:
                            this.pictureBoxGrouping.Image = this.imageListGrouping.Images[0];
                            break;
                        case TableColumn.Grouping.Maximum:
                            this.pictureBoxGrouping.Image = this.imageListGrouping.Images[1];
                            break;
                        case TableColumn.Grouping.Minimum:
                            this.pictureBoxGrouping.Image = this.imageListGrouping.Images[2];
                            break;
                    }
                }
                else
                {
                    this.pictureBoxGrouping.Image = null;
                    this.comboBoxGrouping.Visible = false;
                }

                // Sorting
                bool showSorting = false;
                if (this._TableColumn.Table.TypeOfParallelity == Table.Parallelity.referencing
                    && this._TableColumn.Table.ReferencedParallelPosition == 1)
                {
                    showSorting = true;
                }
                else if (this._TableColumn.Table.ParallelPosition > 1
                    || !this._TableColumn.AllowOrderBy
                    || this._TableColumn.Table.TypeOfParallelity != Table.Parallelity.parallel)
                {
                    showSorting = false;
                }
                else showSorting = true;

                if (showSorting)
                {
                    this.numericUpDownSorting.Visible = true;
                    this.numericUpDownSorting.Value = this._TableColumn.SortingSequence;

                    if (this._TableColumn.SortingSequence > 0)
                    {
                        this.buttonSorting.Visible = true;
                        switch (this._TableColumn.SortingType)
                        {
                            case TableColumn.Sorting.ascending:
                                this.buttonSorting.Image = this.imageListSorting.Images[0];
                                this.buttonSorting.Enabled = true;
                                break;
                            case TableColumn.Sorting.descending:
                                this.buttonSorting.Image = this.imageListSorting.Images[1];
                                this.buttonSorting.Enabled = true;
                                break;
                            case TableColumn.Sorting.notsorted:
                                this.buttonSorting.Image = null;
                                this.buttonSorting.Enabled = false;
                                break;
                        }
                        if (this._TableColumn.SortingType != TableColumn.Sorting.notsorted)
                        {
                            bool SameSortingSequencePresent = false;
                            foreach (System.Collections.Generic.KeyValuePair<string, Export.TableColumn> KV in this._TableColumn.Table.TableColumns)
                            {
                                if (KV.Key != this._TableColumn.ColumnName && KV.Value.SortingSequence == this._TableColumn.SortingSequence)
                                {
                                    SameSortingSequencePresent = true;
                                    break;
                                }
                            }
                            if (SameSortingSequencePresent)
                                this.numericUpDownSorting.BackColor = System.Drawing.Color.Pink;
                            else
                                this.numericUpDownSorting.BackColor = System.Drawing.SystemColors.Window;
                        }
                    }
                    else
                    {
                        this.buttonSorting.Visible = false;
                        this.numericUpDownSorting.BackColor = System.Drawing.SystemColors.Window;
                    }
                }
                else
                {
                    this.numericUpDownSorting.Visible = false;
                    this.buttonSorting.Visible = false;
                }

                // Module connections or relations
                if (this._TableColumn.DiversityWorkbenchModule != null && this._TableColumn.DiversityWorkbenchModule.Length > 0)
                {
                    if (this._TableColumn.DiversityWorkbenchModuleSelectedUnitValues == null || this._TableColumn.DiversityWorkbenchModuleSelectedUnitValues.Count == 0)
                    {
                        this.Height = (int)(46 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                        System.Windows.Forms.Label L = new Label();
                        L.Text = "Values from " + this._TableColumn.DiversityWorkbenchModule + " available";
                        L.Dock = DockStyle.Fill;
                        L.TextAlign = ContentAlignment.MiddleLeft;
                        L.AutoSize = false;
                        L.ForeColor = System.Drawing.Color.Gray;
                        L.Enabled = false;
                        this.pictureBoxUnitValues.Image = this.imageListUnitValueRelation.Images[1];
                        this.panelUnitValues.Controls.Add(L);
                    }
                    else
                    {

                    }
                }
                else if (this._TableColumn.ForeignRelations.Count > 0
                    && this._TableColumn.Table.ParentTable != null
                    && !this._TableColumn.ForeignRelations.ContainsKey(this._TableColumn.Table.ParentTable.TableName)
                    && !this._TableColumn.ForeignRelations.ContainsKey(this._TableColumn.Table.TableName))
                {
                    this.Height = (int)(46 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                    System.Windows.Forms.Label L = new Label();
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._TableColumn.ForeignRelations)
                    {
                        if (L.Text.Length > 0) L.Text += " and ";
                        L.Text += KV.Key;
                    }
                    L.Text = "Values from " + L.Text + " available";
                    L.Dock = DockStyle.Fill;
                    L.TextAlign = ContentAlignment.MiddleLeft;
                    L.AutoSize = false;
                    L.ForeColor = System.Drawing.Color.Gray;
                    L.Enabled = false;
                    this.pictureBoxUnitValues.Image = this.imageListUnitValueRelation.Images[0];
                    this.panelUnitValues.Controls.Add(L);
                }
                else if (this._TableColumn.ForeignRelations.Count > 0
                    && this._TableColumn.Table.ParentTable == null
                    && !this._TableColumn.ForeignRelations.ContainsKey(this._TableColumn.Table.TableName))
                {
                    this.Height = (int)(46 + DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                    System.Windows.Forms.Label L = new Label();
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._TableColumn.ForeignRelations)
                    {
                        if (L.Text.Length > 0) L.Text += " and ";
                        L.Text += KV.Key;
                    }
                    L.Text = "Values from " + L.Text + " available";
                    L.Dock = DockStyle.Fill;
                    L.TextAlign = ContentAlignment.MiddleLeft;
                    L.AutoSize = false;
                    L.ForeColor = System.Drawing.Color.Gray;
                    L.Enabled = false;
                    this.pictureBoxUnitValues.Image = this.imageListUnitValueRelation.Images[0];
                    this.panelUnitValues.Controls.Add(L);
                }
                else
                {
                    this.Height = (int)(23 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                    this.buttonAddUnitValue.Visible = false;
                }
                if (this._TableColumn.UnitValues.Count > 0)
                {
                    this.panelUnitValues.Controls.Clear();
                    for (int i = 0; i < this._TableColumn.UnitValues.Count; i++)
                    {
                        Export.UserControlTableColumnUnitValue u = new UserControlTableColumnUnitValue(this._TableColumn, this._TableColumn.UnitValues[i], this._iExporter);
                        u.Dock = DockStyle.Top;
                        this.panelUnitValues.Controls.Add(u);
                        u.BringToFront();
                        if (i > 0)
                            this.Height += 23;
                    }
                    this.Height = (int)(this.Height * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void comboBoxGrouping_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string Grouping = comboBoxGrouping.SelectedItem.ToString().Replace(" ", "").ToLower();
            if (Grouping == Export.TableColumn.Grouping.GroupBy.ToString().ToLower())
                this._TableColumn.ColumnGrouping = TableColumn.Grouping.GroupBy;
            else if (Grouping == Export.TableColumn.Grouping.Maximum.ToString().ToLower())
                this._TableColumn.ColumnGrouping = TableColumn.Grouping.Maximum;
            else if (Grouping == Export.TableColumn.Grouping.Minimum.ToString().ToLower())
                this._TableColumn.ColumnGrouping = TableColumn.Grouping.Minimum;
            else if (Grouping == Export.TableColumn.Grouping.None.ToString().ToLower())
                this._TableColumn.ColumnGrouping = TableColumn.Grouping.None;
            this.setControls();
        }

        private void numericUpDownSorting_Click(object sender, EventArgs e)
        {
            this._TableColumn.SortingSequence = (int)this.numericUpDownSorting.Value;
            if (this._TableColumn.SortingSequence == 0)
                this._TableColumn.SortingType = TableColumn.Sorting.notsorted;
            else if (this._TableColumn.SortingType == TableColumn.Sorting.notsorted)
                this._TableColumn.SortingType = TableColumn.Sorting.ascending;
            this.setControls();
        }

        private void buttonSorting_Click(object sender, EventArgs e)
        {
            if (this._TableColumn.SortingType == TableColumn.Sorting.descending)
                this._TableColumn.SortingType = TableColumn.Sorting.ascending;
            else if (this._TableColumn.SortingType == TableColumn.Sorting.ascending)
                this._TableColumn.SortingType = TableColumn.Sorting.descending;
            this.setControls();
        }

        private void buttonAddUnitValue_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._TableColumn.DiversityWorkbenchModule != null && this._TableColumn.DiversityWorkbenchModule.Length > 0)
                {
                    // Removing services with no additional information
                    System.Collections.Generic.Dictionary<string, string> Services = this._TableColumn.DiversityWorkbenchModuleSources;

                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(Services, "Select Service", "Please select the service where the data should be taken from", true);
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
                                    DiversityWorkbench.Forms.FormGetStringFromList fValue = new DiversityWorkbench.Forms.FormGetStringFromList(L, "Value", "Please select the value that should be used", true);
                                    fValue.ShowDialog();
                                    if (fValue.DialogResult == DialogResult.OK)
                                    {
                                        if (fValue.String.StartsWith("Link to "))
                                        {
                                            string DiversityWorkbenchModule = fValue.String.Substring(fValue.String.IndexOf("Diversity"));
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
                                                                string key = "";
                                                                if (KV.Value.DatabaseAndServiceURIs().TryGetValue(KVservice.Key, out key))
                                                                    DiversityWorkbenchModuleSources.Add(key, KVservice.Key);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            // Getting the secondary module
                                            if (DiversityWorkbenchModuleSources.Count > 0)
                                            {
                                                DiversityWorkbench.Forms.FormGetStringFromList fsource = new DiversityWorkbench.Forms.FormGetStringFromList(DiversityWorkbenchModuleSources, "Select Service", "Please select the service where the data should be taken from", true);
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

                                                    DiversityWorkbench.ServerConnection S = null;
                                                    if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[DiversityWorkbenchModule].ServerConnectionList().ContainsKey(fsource.SelectedString))
                                                    {
                                                        S = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[DiversityWorkbenchModule].ServerConnectionList()[fsource.SelectedString];
                                                        IWorkbenchUnit.setServerConnection(S);
                                                    }

                                                    if (S != null)
                                                    {
                                                        System.Collections.Generic.List<string> DiversityWorkbenchModulePossibleUnitValues = new List<string>();
                                                        System.Collections.Generic.Dictionary<string, string> Values = IWorkbenchUnit.UnitValues(-1);
                                                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Values)
                                                        {
                                                            if (!DiversityWorkbenchModulePossibleUnitValues.Contains(KV.Key))
                                                                DiversityWorkbenchModulePossibleUnitValues.Add(KV.Key);
                                                        }
                                                        // Getting the unit value column for the secondary module
                                                        DiversityWorkbench.Forms.FormGetStringFromList flink = new DiversityWorkbench.Forms.FormGetStringFromList(DiversityWorkbenchModulePossibleUnitValues, "Select column", "Please select the column", true);
                                                        flink.ShowDialog();
                                                        if (flink.DialogResult == DialogResult.OK)
                                                        {
                                                            Export.TableColumnUnitValue U = new TableColumnUnitValue(this._TableColumn, f.SelectedValue, fValue.String, f.SelectedString);
                                                            U.LinkedUnitValue = flink.String;
                                                            U.LinkedDiversityWorkbenchModuleBaseUri = S.BaseURL;
                                                            this._TableColumn.UnitValues.Add(U);
                                                            this._iExporter.ShowCurrentSourceTableColumns(this._TableColumn.Table);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Export.TableColumnUnitValue U = new TableColumnUnitValue(this._TableColumn, f.SelectedValue, fValue.String, f.SelectedString);
                                            this._TableColumn.UnitValues.Add(U);
                                            this._iExporter.ShowCurrentSourceTableColumns(this._TableColumn.Table);
                                        }
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
                                            DiversityWorkbench.Forms.FormGetStringFromList fValue = new DiversityWorkbench.Forms.FormGetStringFromList(L, "Value", "Please select the value that should be used", true);
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
                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(this._TableColumn.getForeignRelationTableColumns(), "Select column");
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

        private void buttonAdd_Click(object sender, EventArgs e)
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

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Filter", "Please enter the filter", this._TableColumn.Filter);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                this._TableColumn.Filter = f.String;
                this.setControls();
            }
        }

        private void buttonFilterRow_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Filter", "Please enter the row filter", this._TableColumn.RowFilter);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                this._TableColumn.RowFilter = f.String;
                this.setControls();
            }
        }

        #endregion

        #region Drag & Drop

        private void labelColumName_MouseDown(object sender, MouseEventArgs e)
        {

        }

        #endregion

    }
}
