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
    public partial class UserControl_Print : UserControl__Data
    {

        #region Construction

        public UserControl_Print(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this._HelpNamespace = HelpNamespace;
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        #endregion

        #region Control

        private void initControl()
        {
            this.webBrowserLabel.Url = new Uri("about:blank ");
            //this.userControlWebViewLabel.Url = null;
            //this.userControlWebViewLabel.Navigate(new Uri("about:blank "));
            if (this.comboBoxLabelConversion.Items.Count == 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<DiversityCollection.Transaction.ConversionType, string> KV in DiversityCollection.Transaction.ConversionDictionary)
                    this.comboBoxLabelConversion.Items.Add(KV.Key.ToString().Replace("_", " "));
            }

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();
        }

        private void toolStripButtonOpenSchemaFile_Click(object sender, EventArgs e)
        {
            string Path = Folder.LabelPrinting(Folder.LabelPrintingFolder.Schemas);
            if (this.textBoxSchemaFile.Text.Length > 0)
            {
                try
                {
                    System.IO.FileInfo FI = new System.IO.FileInfo(this.textBoxSchemaFile.Text);
                    if (FI.Exists)
                        Path = FI.DirectoryName;
                }
                catch { }
            }
            this.openFileDialogLabelSchema = new OpenFileDialog
            {
                RestoreDirectory = true,
                Multiselect = false,
                InitialDirectory = Path,
                Filter = "XSLT Files|*.xslt"
            };
            try
            {
                this.openFileDialogLabelSchema.ShowDialog();
                if (this.openFileDialogLabelSchema.FileName.Length > 0)
                {
                    this.textBoxSchemaFile.Tag = this.openFileDialogLabelSchema.FileName;
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialogLabelSchema.FileName);
                    this.textBoxSchemaFile.Text = f.FullName;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonLabelPreview_Click(object sender, EventArgs e)
        {
            string File = this.createXmlFromDataset(this._iMainForm.DataSetCollectionSpecimen(), this._iMainForm.DataSetCollectionEventSeries(), false);
            if (File.Length > 0)
            {
                try
                {
                    System.Uri URI = new Uri(File);
                    this.webBrowserLabel.Url = URI;
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private string createXmlFromDataset(DiversityCollection.Datasets.DataSetCollectionSpecimen dsSpecimen,
            DiversityCollection.Datasets.DataSetCollectionEventSeries dsEventSeries, bool MultiLabelPrint)
        {
            string XmlFile = Folder.LabelPrinting() + "Label.XML";
            try
            {
                System.Data.DataRow RR;
                DiversityCollection.XmlExport Export;
                int Duplicates;
                bool LabelForSpecimen = true;
                System.Data.DataRow RowToPrint;
                int? PartID = null;
                if (this._iMainForm.SelectedPartHierarchyNode() == null && this._iMainForm.SelectedUnitHierarchyNode() != null)
                {
                    RR = (System.Data.DataRow)this._iMainForm.SelectedUnitHierarchyNode().Tag;
                }
                else if (this._iMainForm.SelectedPartHierarchyNode() != null)
                    RR = (System.Data.DataRow)this._iMainForm.SelectedPartHierarchyNode().Tag;
                else if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPart.Rows.Count == 1)
                    RR = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPart.Rows[0];
                else if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPart.Rows.Count == 0)
                    RR = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0];
                else
                {
                    RR = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0];
                    System.Windows.Forms.MessageBox.Show("Please select the PART that should be printed in a label.\r\nOtherwise the information of the specimen will be displayed", "No part selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //return "";
                }
                string Table = RR.Table.TableName;
                switch (Table)
                {
                    case "CollectionSpecimen":
                        if (this._iMainForm.SelectedPartHierarchyNode() == null && this._iMainForm.SelectedUnitHierarchyNode() != null)
                            RowToPrint = (System.Data.DataRow)this._iMainForm.SelectedUnitHierarchyNode().Tag;
                        else if (this._iMainForm.SelectedPartHierarchyNode() != null)
                            RowToPrint = (System.Data.DataRow)this._iMainForm.SelectedPartHierarchyNode().Tag;
                        else if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPart.Rows.Count == 1)
                            RowToPrint = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPart.Rows[0];
                        else if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows.Count > 0)
                            RowToPrint = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0];
                        else
                            return "";
                        break;
                    case "CollectionSpecimenPart":
                    case "IdentificationUnitInPart":
                        if (this._iMainForm.SelectedPartHierarchyNode() != null)
                        {
                            RowToPrint = (System.Data.DataRow)this._iMainForm.SelectedPartHierarchyNode().Tag;
                            if (RowToPrint.Table.TableName != "CollectionSpecimenPart")
                            {
                                if (RowToPrint.Table.Columns.Contains("SpecimenPartID"))
                                {
                                    string sPartID = RowToPrint["SpecimenPartID"].ToString();
                                    System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPart.Select("SpecimenPartID = " + sPartID);
                                    if (rr.Length > 0)
                                        RowToPrint = rr[0];
                                }
                            }
                        }
                        else if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPart.Rows.Count == 1)
                        {
                            RowToPrint = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPart.Rows[0];
                        }
                        else return "";
                        PartID = int.Parse(RowToPrint["SpecimenPartID"].ToString());
                        LabelForSpecimen = false;
                        break;
                    default:
                        if (this._iMainForm.SelectedPartHierarchyNode() != null)
                            RowToPrint = (System.Data.DataRow)this._iMainForm.SelectedPartHierarchyNode().Tag;
                        else if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPart.Rows.Count > 0)
                            RowToPrint = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPart[0];
                        else if(this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows.Count > 0)
                            RowToPrint = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0];
                        else
                            return "";
                        if (RowToPrint.RowState == DataRowState.Deleted || RowToPrint.RowState == DataRowState.Detached)
                            RowToPrint = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0];
                        break;
                }
                if (this.textBoxSchemaFile.Tag == null) this.textBoxSchemaFile.Tag = "";
                if (this.textBoxSchemaFile.Text.Length == 0) this.textBoxSchemaFile.Tag = "";
                Export = new XmlExport(this.textBoxSchemaFile.Tag.ToString(), XmlFile, dsSpecimen, dsEventSeries);
                Duplicates = 1;
                int.TryParse(this.toolStripTextBoxPrintDuplicates.Text, out Duplicates);
                if (RowToPrint != null)
                {
                    bool RestrictToCollection = true;
                    if (!this.checkBoxPrintRestrictToCollection.Visible) RestrictToCollection = false;
                    if (!this.checkBoxPrintRestrictToCollection.Checked) RestrictToCollection = false;

                    bool RestrictToMaterial = true;
                    if (!this.checkBoxPrintRestrictToMaterial.Visible) RestrictToMaterial = false;
                    if (!this.checkBoxPrintRestrictToMaterial.Checked) RestrictToMaterial = false;

                    XmlExport.QRcodeSource QRsource = XmlExport.QRcodeSource.None;
                    if (this.comboBoxLabelQRcode.SelectedItem != null)
                    {
                        //if (this.comboBoxLabelQRcode.SelectedItem.ToString() == "AccessionNumber")
                        //    QRsource = XmlExport.QRcodeSource.AccessionNumber;
                        //else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == "CollectorsEventNumber")
                        //    QRsource = XmlExport.QRcodeSource.CollectorsEventNumber;
                        //else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == "DepositorsAccessionNumber")
                        //    QRsource = XmlExport.QRcodeSource.DepositorsAccessionNumber;
                        //else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == "ExternalIdentifier")
                        //    QRsource = XmlExport.QRcodeSource.ExternalIdentifier;
                        //else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == "PartAccessionNumber")
                        //    QRsource = XmlExport.QRcodeSource.PartAccessionNumber;
                        //else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == "StableIdentifier")
                        //    QRsource = XmlExport.QRcodeSource.StableIdentifier;
                        //else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == "StorageLocation")
                        //    QRsource = XmlExport.QRcodeSource.StorageLocation;
                        //else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == "GUID")
                        //    QRsource = XmlExport.QRcodeSource.GUID;
                        //else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == "")
                        //    QRsource = XmlExport.QRcodeSource.None;

                        switch (this.comboBoxLabelQRcode.SelectedItem.ToString())
                        {
                            case "AccessionNumber":
                                QRsource = XmlExport.QRcodeSource.AccessionNumber;
                                break;
                            case "CollectorsEventNumber":
                                QRsource = XmlExport.QRcodeSource.CollectorsEventNumber;
                                break;
                            case "DepositorsAccessionNumber":
                                QRsource = XmlExport.QRcodeSource.DepositorsAccessionNumber;
                                break;
                            case "ExternalIdentifier":
                                QRsource = XmlExport.QRcodeSource.ExternalIdentifier;
                                break;
                            case "PartAccessionNumber":
                                QRsource = XmlExport.QRcodeSource.PartAccessionNumber;
                                break;
                            case "StableIdentifier":
                                QRsource = XmlExport.QRcodeSource.StableIdentifier;
                                break;
                            case "StorageLocation":
                                QRsource = XmlExport.QRcodeSource.StorageLocation;
                                break;
                            case "GUID":
                                QRsource = XmlExport.QRcodeSource.GUID;
                                break;
                            default:
                                QRsource = XmlExport.QRcodeSource.None;
                                break;
                        }
                    }


                    string QRtype = "";
                    if (this.comboBoxLabelQRcodeType.SelectedItem != null)
                        QRtype = this.comboBoxLabelQRcodeType.SelectedItem.ToString();
                    if (this._iMainForm.ProjectID() == null)
                    {
                        System.Windows.Forms.MessageBox.Show("Please select a project (in the query conditions)");
                        return "";
                    }
                    int ProjectID = (int)this._iMainForm.ProjectID();
                    return Export.createXmlFromDatasets(this.textBoxReportTitle.Text,
                        QRsource,
                        QRtype,
                        RowToPrint,
                        ProjectID,
                        Duplicates,
                        MultiLabelPrint,
                        LabelForSpecimen,
                        RestrictToCollection,
                        RestrictToMaterial,
                        this.checkBoxUseStockForLabelDuplicates.Checked,
                        DiversityCollection.Transaction.ConversionTypeFormString(this.comboBoxLabelConversion.Text));
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return XmlFile;
        }

        private void toolStripButtonLabelMulti_Click(object sender, EventArgs e)
        {
            if (this._iMainForm.SelectedIDs().Count == 1 && System.Windows.Forms.MessageBox.Show("Only one item has been selected. Do you want to print labels for all items", "All items?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this._iMainForm.SelectAll();
            }
            if (this._iMainForm.SelectedIDs().Count > 50)// this._iMainForm.SelectedIDs().Count > 50)
            {
                if (System.Windows.Forms.MessageBox.Show("Do you really want to generate the labels for\r\n\t\t"
                    + this._iMainForm.SelectedIDs().Count.ToString()
                    + "\r\n\tspecimen selected in the list", "Label print", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            DiversityCollection.CollectionSpecimen Specimen = new CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
            DiversityCollection.Datasets.DataSetCollectionSpecimen dsSpecimen = new DiversityCollection.Datasets.DataSetCollectionSpecimen();
            DiversityCollection.Datasets.DataSetCollectionEventSeries dsEventSeries = new DiversityCollection.Datasets.DataSetCollectionEventSeries();
            try
            {
                for (int i = 0; i < this._iMainForm.SelectedIDs().Count; i++)
                {
                    //System.Data.DataRowView rv = (System.Data.DataRowView)this._iMainForm.SelectedIDs()[i];
                    //int SpecimenID = int.Parse(rv["ID"].ToString());
                    Specimen.fillSpecimen(this._iMainForm.SelectedIDs()[i], ref dsSpecimen, ref dsEventSeries);
                }
                string File = this.createXmlFromDataset(dsSpecimen, dsEventSeries, true);
                if (File.Length > 0)
                {
                    System.Uri URI = new Uri(File);
                    this.webBrowserLabel.Url = URI;
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void toolStripTextBoxPrintDuplicates_TextChanged(object sender, EventArgs e)
        {
            int D;
            if (int.TryParse(this.toolStripTextBoxPrintDuplicates.Text, out D))
            {
                if (D > 99 || D < 1)
                {
                    System.Windows.Forms.MessageBox.Show(DiversityCollection.Forms.FormCollectionSpecimenText.Values_are_restricted_to + ": 1 - 99");//"Only values from 1 - 99 are allowed here");
                    this.toolStripTextBoxPrintDuplicates.Text = "1";
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(DiversityCollection.Forms.FormCollectionSpecimenText.Values_are_restricted_to + ": 1 - 99");//"Only values from 1 - 99 are allowed here");
                this.toolStripTextBoxPrintDuplicates.Text = "1";
            }
        }

        private void toolStripButtonPageSetup_Click(object sender, EventArgs e)
        {
            this.webBrowserLabel.ShowPageSetupDialog();
            this.webBrowserLabel.Refresh();

            //this.userControlWebViewLabel.ShowPageSetupDialog();
            //this.webBrowserLabel.Refresh();
        }

        private void toolStripButtonPrint_Click(object sender, EventArgs e)
        {
            this.webBrowserLabel.ShowPrintPreviewDialog();
        }

        private void toolStripButtonLabelExport_Click(object sender, EventArgs e)
        {
            this.saveFileDialog.RestoreDirectory = true;
            this.saveFileDialog.Filter = "html files (*.htm)|*.htm";
            this.saveFileDialog.AddExtension = true;
            this.saveFileDialog.DefaultExt = "htm";
            this.saveFileDialog.FileName = "Label.htm";
            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string Text = this.webBrowserLabel.DocumentText;
                System.IO.StreamWriter w = new System.IO.StreamWriter(this.saveFileDialog.FileName, false, System.Text.Encoding.UTF8);
                w.Write(Text);
                w.Close();
            }
        }

        private void sNSBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // System.Diagnostics.Process.Start("https://github.com/SNSB/DWB-Contrib/tree/master/DiversityCollection/LabelPrinting/Schemas");
            string link = "https://github.com/SNSB/DWB-Contrib/tree/master/DiversityCollection/LabelPrinting/Schemas";
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(link)
            {
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(info);
        }

        private void zFMKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // System.Diagnostics.Process.Start("https://github.com/ZFMK/Labels-and-Imports-for-DiversityWorkbench/tree/master/Labels");
            string link = "https://github.com/ZFMK/Labels-and-Imports-for-DiversityWorkbench/tree/master/Labels";
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(link)
            {
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(info);
        }

        private void comboBoxLabelConversion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxLabelConversion.SelectedIndex > -1)
            {
                this.toolTip.SetToolTip(this.comboBoxLabelConversion, DiversityCollection.Transaction.ConversionDescription(this.comboBoxLabelConversion.Text));
            }
        }
        
        #endregion

        #region Interface
        
        public void setMultiPrintRestrictionText()
        {
            try
            {
                bool PartSelected = this._iMainForm.SelectedPartHierarchyNode() != null;
                this.checkBoxPrintRestrictToCollection.Enabled = PartSelected;
                this.checkBoxPrintRestrictToMaterial.Enabled = PartSelected;
                this.checkBoxUseStockForLabelDuplicates.Enabled = PartSelected;
                this.buttonLabelQRcode.Enabled = PartSelected;
                this.checkBoxLabelQRcode.Enabled = PartSelected;
                this.comboBoxLabelQRcode.Enabled = PartSelected;
                this.comboBoxLabelQRcodeType.Enabled = PartSelected;
                //if (this._iMainForm.SelectedPartHierarchyNode() == null)
                //{
                //    this.checkBoxPrintRestrictToCollection.Enabled = false;
                //    this.checkBoxPrintRestrictToMaterial.Enabled = false;
                //    this.checkBoxUseStockForLabelDuplicates.Enabled = false;
                //}
                //else
                if (PartSelected)
                {
                    //this.checkBoxPrintRestrictToCollection.Enabled = true;
                    //this.checkBoxPrintRestrictToMaterial.Enabled = true;
                    //this.checkBoxUseStockForLabelDuplicates.Enabled = true;
                    System.Data.DataRow R = (System.Data.DataRow)this._iMainForm.SelectedPartHierarchyNode().Tag;
                    if (R.Table.TableName == "IdentificationUnitInPart")
                    {
                        System.Windows.Forms.TreeNode N = this._iMainForm.SelectedPartHierarchyNode().Parent;
                        R = (System.Data.DataRow)N.Tag;
                    }
                    else if (R.Table.TableName == "CollectionSpecimen")
                    {
                        System.Windows.Forms.TreeNode N = this._iMainForm.SelectedPartHierarchyNode();
                        if (N.Nodes.Count > 0)
                        {
                            System.Windows.Forms.TreeNode NN = N.Nodes[0];
                            R = (System.Data.DataRow)NN.Tag;
                        }
                    }
                    if (R.Table.TableName == "CollectionSpecimenPart" && R.RowState != DataRowState.Deleted && R.RowState != DataRowState.Detached)
                    {
                        if (R["CollectionID"].Equals(System.DBNull.Value))
                        {
                            this.checkBoxPrintRestrictToCollection.Checked = false;
                            this.checkBoxPrintRestrictToCollection.Visible = false;
                        }
                        else
                        {
                            string Collection = DiversityCollection.LookupTable.CollectionName(int.Parse(R["CollectionID"].ToString()));
                            this.checkBoxPrintRestrictToCollection.Text = DiversityCollection.Forms.FormCollectionSpecimenText.Restrict_to_collection + ":  " + Collection;
                        }
                        if (R["MaterialCategory"].Equals(System.DBNull.Value))
                        {
                            this.checkBoxPrintRestrictToMaterial.Checked = false;
                            this.checkBoxPrintRestrictToMaterial.Visible = false;
                            this.pictureBoxPrintRestrictToMaterial.Visible = false;
                            this.tableLayoutPanelLabel.Height = 50;
                        }
                        else
                        {
                            string Abbreviation = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, DiversityWorkbench.Entity.EntityInformation("CollMaterialCategory_Enum.Code." + R["MaterialCategory"].ToString()));
                            this.checkBoxPrintRestrictToMaterial.Text = DiversityCollection.Forms.FormCollectionSpecimenText.Restrict_to_material + ":  " + Abbreviation;
                            this.pictureBoxPrintRestrictToMaterial.Image = DiversityCollection.Specimen.MaterialCategoryImage(false, R["MaterialCategory"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        public void ResetLabel()
        {
            this.webBrowserLabel.Url = new Uri("about:blank ");
        }

        public System.Windows.Forms.TableLayoutPanel TableLayoutPanel { get { return this.tableLayoutPanelLabel; } }

        #endregion

        #region QR code

        private bool _PrintQRcode = false;

        private void buttonLabelQRcode_Click(object sender, EventArgs e)
        {
            this.SetQrCodeControls();
        }

        private void checkBoxLabelQRcode_Click(object sender, EventArgs e)
        {
            this.SetQrCodeControls();
        }

        private void SetQrCodeControls()
        {
            this._PrintQRcode = !this._PrintQRcode;
            this.checkBoxLabelQRcode.Checked = this._PrintQRcode;
            this.comboBoxLabelQRcode.Enabled = this._PrintQRcode;
            this.comboBoxLabelQRcodeType.Enabled = this._PrintQRcode;
            this.pictureBoxLabelQRcodeSource.Enabled = this._PrintQRcode;

            this.buttonLabelQRcode.Image = DiversityCollection.Resource.QRcode;

        }

        private bool StableIdentifierSettingsComplete()
        {
            bool SpecificationsComplete = true;
            if (this._iMainForm.ProjectID() == null)// !this.userControlQueryList.ProjectIsSelected)// || this._iMainForm.ProjectID() == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a project");
                SpecificationsComplete = false;
            }
            else
            {
                string Test = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT dbo.StableIdentifier(" + this._iMainForm.ProjectID().ToString() + ", " + this._iMainForm.ID_Specimen().ToString() + ", NULL, NULL)");
                if (Test.Length == 0)
                {
                    string SQL = "SELECT Project, StableIdentifierBase, StableIdentifierTypeID " +
                        "FROM ProjectProxy AS p " +
                        "WHERE ProjectID = " + this._iMainForm.ProjectID().ToString();
                    System.Data.DataTable dt = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                        if (dt.Rows[0]["StableIdentifierBase"].Equals(System.DBNull.Value)
                            || dt.Rows[0]["StableIdentifierBase"].ToString().Length == 0
                            || dt.Rows[0]["StableIdentifierTypeID"].Equals(System.DBNull.Value))
                        {
                            string Message = "Missing specifications:\r\n";
                            if (dt.Rows[0]["StableIdentifierBase"].Equals(System.DBNull.Value) || dt.Rows[0]["StableIdentifierBase"].ToString().Length == 0)
                                Message += "No base for the stable identifier specified\r\n";
                            if (dt.Rows[0]["StableIdentifierTypeID"].Equals(System.DBNull.Value))
                                Message += "No type for the stable identifier selected\r\n";
                            Message += "Do you want to insert the missing specifications?";
                            if (System.Windows.Forms.MessageBox.Show(Message, "Missing specifications", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                DiversityWorkbench.Forms.FormStableIdentifier f = new DiversityWorkbench.Forms.FormStableIdentifier((int)this._iMainForm.ProjectID());//dtProject);
                                f.setHelp("Stable identifier");
                                f.ShowDialog();
                                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                                {
                                    dt.Clear();
                                    ad.Fill(dt);
                                    if (dt.Rows.Count == 0
                                        || dt.Rows[0]["StableIdentifierBase"].Equals(System.DBNull.Value)
                                        || dt.Rows[0]["StableIdentifierBase"].ToString().Length == 0
                                        || dt.Rows[0]["StableIdentifierTypeID"].Equals(System.DBNull.Value))
                                        SpecificationsComplete = false;
                                }
                                else
                                    SpecificationsComplete = false;
                            }
                            else
                                SpecificationsComplete = false;
                        }
                    }
                    else
                    {
                        SpecificationsComplete = false;
                    }
                }
            }
            if (!SpecificationsComplete)
            {
                this.buttonLabelQRcode.Image = DiversityCollection.Resource.QRcodeGray;
                this.comboBoxLabelQRcode.Items.Clear();
            }
            return SpecificationsComplete;
        }

        private void comboBoxLabelQRcode_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxLabelQRcode.Items.Count == 0)
            {
                this.comboBoxLabelQRcode.Items.Add("");
                this.comboBoxLabelQRcode.Items.Add(DiversityCollection.XmlExport.QRcodeSource.AccessionNumber);
                this.comboBoxLabelQRcode.Items.Add(DiversityCollection.XmlExport.QRcodeSource.CollectorsEventNumber);
                this.comboBoxLabelQRcode.Items.Add(DiversityCollection.XmlExport.QRcodeSource.DepositorsAccessionNumber);
                this.comboBoxLabelQRcode.Items.Add(DiversityCollection.XmlExport.QRcodeSource.ExternalIdentifier);
                this.comboBoxLabelQRcode.Items.Add(DiversityCollection.XmlExport.QRcodeSource.PartAccessionNumber);
                this.comboBoxLabelQRcode.Items.Add(DiversityCollection.XmlExport.QRcodeSource.StableIdentifier);
                this.comboBoxLabelQRcode.Items.Add(DiversityCollection.XmlExport.QRcodeSource.StorageLocation);
                this.comboBoxLabelQRcode.Items.Add(DiversityCollection.XmlExport.QRcodeSource.GUID);
            }
        }

        private void comboBoxLabelQRcode_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBoxLabelQRcodeType.Items.Clear();
            this.comboBoxLabelQRcodeType.Text = "";
            this.comboBoxLabelQRcodeType.Enabled = false;
            if (this.comboBoxLabelQRcode.SelectedItem.ToString() == DiversityCollection.XmlExport.QRcodeSource.AccessionNumber.ToString())
            {
                this.pictureBoxLabelQRcodeSource.Image = DiversityCollection.Resource.CollectionSpecimen;
            }
            else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == DiversityCollection.XmlExport.QRcodeSource.CollectorsEventNumber.ToString())
            {
                this.pictureBoxLabelQRcodeSource.Image = DiversityCollection.Resource.Event;
            }
            else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == DiversityCollection.XmlExport.QRcodeSource.DepositorsAccessionNumber.ToString())
            {
                this.pictureBoxLabelQRcodeSource.Image = DiversityCollection.Resource.CollectionSpecimen;
            }
            else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == DiversityCollection.XmlExport.QRcodeSource.ExternalIdentifier.ToString())
            {
                this.pictureBoxLabelQRcodeSource.Image = DiversityCollection.Resource.Identifier;
                this.comboBoxLabelQRcodeType.Items.Add("");
                System.Data.DataTable dt = new DataTable();
                string SQL = "SELECT Type FROM ExternalIdentifierType ORDER BY Type";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow R in dt.Rows)
                        this.comboBoxLabelQRcodeType.Items.Add(R[0].ToString());
                    this.comboBoxLabelQRcodeType.Enabled = true;
                }
            }
            else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == DiversityCollection.XmlExport.QRcodeSource.PartAccessionNumber.ToString())
            {
                this.pictureBoxLabelQRcodeSource.Image = DiversityCollection.Resource.Specimen;
            }
            else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == DiversityCollection.XmlExport.QRcodeSource.StableIdentifier.ToString())
            {
                this.pictureBoxLabelQRcodeSource.Image = DiversityCollection.Resource.QRcode;
                if (this.StableIdentifierSettingsComplete())
                {
                    this.comboBoxLabelQRcodeType.Items.Add("");
                    this.comboBoxLabelQRcodeType.Items.Add(DiversityCollection.XmlExport.QRcodeStableIdentifierType.AccessionNumber.ToString());
                    this.comboBoxLabelQRcodeType.Items.Add(DiversityCollection.XmlExport.QRcodeStableIdentifierType.SpecimenID.ToString());
                    this.comboBoxLabelQRcodeType.Items.Add(DiversityCollection.XmlExport.QRcodeStableIdentifierType.Units.ToString());
                    this.comboBoxLabelQRcodeType.Items.Add(DiversityCollection.XmlExport.QRcodeStableIdentifierType.UnitsInPart.ToString());
                    this.comboBoxLabelQRcodeType.Enabled = true;
                }
                else
                {
                    //this.checkBoxLabelQRcode_Click(null, null);
                    this.checkBoxLabelQRcode.Checked = false;
                    this.SetQrCodeControls();
                }
            }
            else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == DiversityCollection.XmlExport.QRcodeSource.StorageLocation.ToString())
            {
                this.pictureBoxLabelQRcodeSource.Image = DiversityCollection.Resource.Specimen;
            }
            else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == DiversityCollection.XmlExport.QRcodeSource.GUID.ToString())
            {
                this.pictureBoxLabelQRcodeSource.Image = DiversityCollection.Resource.QRcode;
            }

        }

        private string StableIdentifierSpecimen(int SpecimenID, int UnitID, int? PartID)
        {
            if (this._iMainForm.ProjectID() != null)// !this.userControlQueryList.ProjectIsSelected)
            {
                System.Windows.Forms.MessageBox.Show("Please select a project");
                return "";
            }
            string SIS = "";
            string SQL = "SELECT [dbo].[StableIdentifier] (" + this._iMainForm.ProjectID().ToString() +
                ", " + SpecimenID.ToString() + ", " + UnitID.ToString() + ", ";
            if (PartID == null) SQL += "NULL)";
            else SQL += PartID.ToString() + ")";
            SIS = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            return SIS;
        }


        #region Context menu
        private void setServiceTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("QR service template", "Please enter the text for the QR code generator as a template", DiversityWorkbench.Settings.QRcodeService, DiversityCollection.Resource.QRcode);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK) { DiversityWorkbench.Settings.QRcodeService = f.String; }
        }
        private void setSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetInteger f = new DiversityWorkbench.Forms.FormGetInteger(DiversityWorkbench.Settings.QRcodeSize, "QR code size", "Please enter the size for the QR code");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK && f.Integer != null) { DiversityWorkbench.Settings.QRcodeSize = (int)f.Integer; }
        }

        #endregion

        #endregion

    }
}
