
namespace DiversityCollection.UserControls
{
    partial class UserControl_EventSeriesDescriptor
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_EventSeriesDescriptor));
            this.groupBoxDescirptor = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelDescriptor = new System.Windows.Forms.TableLayoutPanel();
            this.treeViewDescriptor = new System.Windows.Forms.TreeView();
            this.imageListDescriptor = new System.Windows.Forms.ImageList(this.components);
            this.panelDescriptorModules = new System.Windows.Forms.Panel();
            this.userControlModuleRelatedEntryDescriptorPlots = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.userControlModuleRelatedEntryDescriptorTerms = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.userControlModuleRelatedEntryDescriptorReference = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.userControlModuleRelatedEntryDescriptorTaxa = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.userControlModuleRelatedEntryDescriptorProject = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.userControlModuleRelatedEntryDescriptorGazetteer = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.userControlModuleRelatedEntryDescriptorCollection = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.userControlModuleRelatedEntryDescriptorAgent = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.textBoxDescriptor = new System.Windows.Forms.TextBox();
            this.buttonDescriptorURL = new System.Windows.Forms.Button();
            this.textBoxDescriptorURL = new System.Windows.Forms.TextBox();
            this.labelDescriptorURL = new System.Windows.Forms.Label();
            this.toolStripDescriptor = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonDescriptorAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDescriptorDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDescriptorEditTypes = new System.Windows.Forms.ToolStripButton();
            this.groupBoxDescirptor.SuspendLayout();
            this.tableLayoutPanelDescriptor.SuspendLayout();
            this.panelDescriptorModules.SuspendLayout();
            this.toolStripDescriptor.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxDescirptor
            // 
            this.groupBoxDescirptor.Controls.Add(this.tableLayoutPanelDescriptor);
            this.groupBoxDescirptor.Controls.Add(this.toolStripDescriptor);
            this.groupBoxDescirptor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDescirptor.Location = new System.Drawing.Point(0, 0);
            this.groupBoxDescirptor.Name = "groupBoxDescirptor";
            this.groupBoxDescirptor.Size = new System.Drawing.Size(484, 150);
            this.groupBoxDescirptor.TabIndex = 22;
            this.groupBoxDescirptor.TabStop = false;
            this.groupBoxDescirptor.Text = "Descriptors";
            // 
            // tableLayoutPanelDescriptor
            // 
            this.tableLayoutPanelDescriptor.ColumnCount = 4;
            this.tableLayoutPanelDescriptor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDescriptor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDescriptor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDescriptor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDescriptor.Controls.Add(this.treeViewDescriptor, 0, 0);
            this.tableLayoutPanelDescriptor.Controls.Add(this.panelDescriptorModules, 0, 2);
            this.tableLayoutPanelDescriptor.Controls.Add(this.textBoxDescriptor, 0, 1);
            this.tableLayoutPanelDescriptor.Controls.Add(this.buttonDescriptorURL, 3, 1);
            this.tableLayoutPanelDescriptor.Controls.Add(this.textBoxDescriptorURL, 2, 1);
            this.tableLayoutPanelDescriptor.Controls.Add(this.labelDescriptorURL, 1, 1);
            this.tableLayoutPanelDescriptor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDescriptor.Location = new System.Drawing.Point(27, 16);
            this.tableLayoutPanelDescriptor.Name = "tableLayoutPanelDescriptor";
            this.tableLayoutPanelDescriptor.RowCount = 3;
            this.tableLayoutPanelDescriptor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDescriptor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDescriptor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDescriptor.Size = new System.Drawing.Size(454, 131);
            this.tableLayoutPanelDescriptor.TabIndex = 4;
            // 
            // treeViewDescriptor
            // 
            this.tableLayoutPanelDescriptor.SetColumnSpan(this.treeViewDescriptor, 4);
            this.treeViewDescriptor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewDescriptor.ImageIndex = 0;
            this.treeViewDescriptor.ImageList = this.imageListDescriptor;
            this.treeViewDescriptor.Location = new System.Drawing.Point(3, 3);
            this.treeViewDescriptor.Name = "treeViewDescriptor";
            this.treeViewDescriptor.SelectedImageIndex = 0;
            this.treeViewDescriptor.Size = new System.Drawing.Size(448, 66);
            this.treeViewDescriptor.TabIndex = 0;
            this.treeViewDescriptor.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewDescriptor_AfterSelect);
            // 
            // imageListDescriptor
            // 
            this.imageListDescriptor.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDescriptor.ImageStream")));
            this.imageListDescriptor.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListDescriptor.Images.SetKeyName(0, "NULL.ico");
            this.imageListDescriptor.Images.SetKeyName(1, "KeyBlue.ico");
            this.imageListDescriptor.Images.SetKeyName(2, "Agent.ico");
            this.imageListDescriptor.Images.SetKeyName(3, "Collection.ico");
            this.imageListDescriptor.Images.SetKeyName(4, "Country.ico");
            this.imageListDescriptor.Images.SetKeyName(5, "Project.ico");
            this.imageListDescriptor.Images.SetKeyName(6, "References.ico");
            this.imageListDescriptor.Images.SetKeyName(7, "SamplingPlot.ico");
            this.imageListDescriptor.Images.SetKeyName(8, "Kristall.ico");
            this.imageListDescriptor.Images.SetKeyName(9, "Plant.ico");
            // 
            // panelDescriptorModules
            // 
            this.tableLayoutPanelDescriptor.SetColumnSpan(this.panelDescriptorModules, 4);
            this.panelDescriptorModules.Controls.Add(this.userControlModuleRelatedEntryDescriptorPlots);
            this.panelDescriptorModules.Controls.Add(this.userControlModuleRelatedEntryDescriptorTerms);
            this.panelDescriptorModules.Controls.Add(this.userControlModuleRelatedEntryDescriptorReference);
            this.panelDescriptorModules.Controls.Add(this.userControlModuleRelatedEntryDescriptorTaxa);
            this.panelDescriptorModules.Controls.Add(this.userControlModuleRelatedEntryDescriptorProject);
            this.panelDescriptorModules.Controls.Add(this.userControlModuleRelatedEntryDescriptorGazetteer);
            this.panelDescriptorModules.Controls.Add(this.userControlModuleRelatedEntryDescriptorCollection);
            this.panelDescriptorModules.Controls.Add(this.userControlModuleRelatedEntryDescriptorAgent);
            this.panelDescriptorModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDescriptorModules.Location = new System.Drawing.Point(3, 104);
            this.panelDescriptorModules.Name = "panelDescriptorModules";
            this.panelDescriptorModules.Size = new System.Drawing.Size(448, 24);
            this.panelDescriptorModules.TabIndex = 0;
            this.panelDescriptorModules.Visible = false;
            // 
            // userControlModuleRelatedEntryDescriptorPlots
            // 
            this.userControlModuleRelatedEntryDescriptorPlots.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryDescriptorPlots.DependsOnUri = "";
            this.userControlModuleRelatedEntryDescriptorPlots.Domain = "";
            this.userControlModuleRelatedEntryDescriptorPlots.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryDescriptorPlots.Location = new System.Drawing.Point(545, 3);
            this.userControlModuleRelatedEntryDescriptorPlots.Module = null;
            this.userControlModuleRelatedEntryDescriptorPlots.Name = "userControlModuleRelatedEntryDescriptorPlots";
            this.userControlModuleRelatedEntryDescriptorPlots.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryDescriptorPlots.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryDescriptorPlots.ShowInfo = false;
            this.userControlModuleRelatedEntryDescriptorPlots.Size = new System.Drawing.Size(60, 22);
            this.userControlModuleRelatedEntryDescriptorPlots.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryDescriptorPlots.TabIndex = 7;
            this.userControlModuleRelatedEntryDescriptorPlots.Visible = false;
            // 
            // userControlModuleRelatedEntryDescriptorTerms
            // 
            this.userControlModuleRelatedEntryDescriptorTerms.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryDescriptorTerms.DependsOnUri = "";
            this.userControlModuleRelatedEntryDescriptorTerms.Domain = "";
            this.userControlModuleRelatedEntryDescriptorTerms.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryDescriptorTerms.Location = new System.Drawing.Point(619, 4);
            this.userControlModuleRelatedEntryDescriptorTerms.Module = null;
            this.userControlModuleRelatedEntryDescriptorTerms.Name = "userControlModuleRelatedEntryDescriptorTerms";
            this.userControlModuleRelatedEntryDescriptorTerms.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryDescriptorTerms.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryDescriptorTerms.ShowInfo = false;
            this.userControlModuleRelatedEntryDescriptorTerms.Size = new System.Drawing.Size(80, 22);
            this.userControlModuleRelatedEntryDescriptorTerms.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryDescriptorTerms.TabIndex = 6;
            this.userControlModuleRelatedEntryDescriptorTerms.Visible = false;
            // 
            // userControlModuleRelatedEntryDescriptorReference
            // 
            this.userControlModuleRelatedEntryDescriptorReference.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryDescriptorReference.DependsOnUri = "";
            this.userControlModuleRelatedEntryDescriptorReference.Domain = "";
            this.userControlModuleRelatedEntryDescriptorReference.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryDescriptorReference.Location = new System.Drawing.Point(436, 2);
            this.userControlModuleRelatedEntryDescriptorReference.Module = null;
            this.userControlModuleRelatedEntryDescriptorReference.Name = "userControlModuleRelatedEntryDescriptorReference";
            this.userControlModuleRelatedEntryDescriptorReference.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryDescriptorReference.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryDescriptorReference.ShowInfo = false;
            this.userControlModuleRelatedEntryDescriptorReference.Size = new System.Drawing.Size(40, 22);
            this.userControlModuleRelatedEntryDescriptorReference.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryDescriptorReference.TabIndex = 5;
            this.userControlModuleRelatedEntryDescriptorReference.Visible = false;
            // 
            // userControlModuleRelatedEntryDescriptorTaxa
            // 
            this.userControlModuleRelatedEntryDescriptorTaxa.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryDescriptorTaxa.DependsOnUri = "";
            this.userControlModuleRelatedEntryDescriptorTaxa.Domain = "";
            this.userControlModuleRelatedEntryDescriptorTaxa.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryDescriptorTaxa.Location = new System.Drawing.Point(736, 4);
            this.userControlModuleRelatedEntryDescriptorTaxa.Module = null;
            this.userControlModuleRelatedEntryDescriptorTaxa.Name = "userControlModuleRelatedEntryDescriptorTaxa";
            this.userControlModuleRelatedEntryDescriptorTaxa.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryDescriptorTaxa.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryDescriptorTaxa.ShowInfo = false;
            this.userControlModuleRelatedEntryDescriptorTaxa.Size = new System.Drawing.Size(40, 22);
            this.userControlModuleRelatedEntryDescriptorTaxa.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryDescriptorTaxa.TabIndex = 4;
            this.userControlModuleRelatedEntryDescriptorTaxa.Visible = false;
            // 
            // userControlModuleRelatedEntryDescriptorProject
            // 
            this.userControlModuleRelatedEntryDescriptorProject.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryDescriptorProject.DependsOnUri = "";
            this.userControlModuleRelatedEntryDescriptorProject.Domain = "";
            this.userControlModuleRelatedEntryDescriptorProject.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryDescriptorProject.Location = new System.Drawing.Point(344, 3);
            this.userControlModuleRelatedEntryDescriptorProject.Module = null;
            this.userControlModuleRelatedEntryDescriptorProject.Name = "userControlModuleRelatedEntryDescriptorProject";
            this.userControlModuleRelatedEntryDescriptorProject.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryDescriptorProject.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryDescriptorProject.ShowInfo = false;
            this.userControlModuleRelatedEntryDescriptorProject.Size = new System.Drawing.Size(51, 22);
            this.userControlModuleRelatedEntryDescriptorProject.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryDescriptorProject.TabIndex = 3;
            this.userControlModuleRelatedEntryDescriptorProject.Visible = false;
            // 
            // userControlModuleRelatedEntryDescriptorGazetteer
            // 
            this.userControlModuleRelatedEntryDescriptorGazetteer.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryDescriptorGazetteer.DependsOnUri = "";
            this.userControlModuleRelatedEntryDescriptorGazetteer.Domain = "";
            this.userControlModuleRelatedEntryDescriptorGazetteer.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryDescriptorGazetteer.Location = new System.Drawing.Point(231, 4);
            this.userControlModuleRelatedEntryDescriptorGazetteer.Module = null;
            this.userControlModuleRelatedEntryDescriptorGazetteer.Name = "userControlModuleRelatedEntryDescriptorGazetteer";
            this.userControlModuleRelatedEntryDescriptorGazetteer.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryDescriptorGazetteer.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryDescriptorGazetteer.ShowInfo = false;
            this.userControlModuleRelatedEntryDescriptorGazetteer.Size = new System.Drawing.Size(88, 22);
            this.userControlModuleRelatedEntryDescriptorGazetteer.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryDescriptorGazetteer.TabIndex = 2;
            this.userControlModuleRelatedEntryDescriptorGazetteer.Visible = false;
            // 
            // userControlModuleRelatedEntryDescriptorCollection
            // 
            this.userControlModuleRelatedEntryDescriptorCollection.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryDescriptorCollection.DependsOnUri = "";
            this.userControlModuleRelatedEntryDescriptorCollection.Domain = "";
            this.userControlModuleRelatedEntryDescriptorCollection.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryDescriptorCollection.Location = new System.Drawing.Point(118, 4);
            this.userControlModuleRelatedEntryDescriptorCollection.Module = null;
            this.userControlModuleRelatedEntryDescriptorCollection.Name = "userControlModuleRelatedEntryDescriptorCollection";
            this.userControlModuleRelatedEntryDescriptorCollection.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryDescriptorCollection.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryDescriptorCollection.ShowInfo = false;
            this.userControlModuleRelatedEntryDescriptorCollection.Size = new System.Drawing.Size(80, 22);
            this.userControlModuleRelatedEntryDescriptorCollection.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryDescriptorCollection.TabIndex = 1;
            this.userControlModuleRelatedEntryDescriptorCollection.Visible = false;
            // 
            // userControlModuleRelatedEntryDescriptorAgent
            // 
            this.userControlModuleRelatedEntryDescriptorAgent.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryDescriptorAgent.DependsOnUri = "";
            this.userControlModuleRelatedEntryDescriptorAgent.Domain = "";
            this.userControlModuleRelatedEntryDescriptorAgent.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryDescriptorAgent.Location = new System.Drawing.Point(14, 4);
            this.userControlModuleRelatedEntryDescriptorAgent.Module = null;
            this.userControlModuleRelatedEntryDescriptorAgent.Name = "userControlModuleRelatedEntryDescriptorAgent";
            this.userControlModuleRelatedEntryDescriptorAgent.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryDescriptorAgent.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryDescriptorAgent.ShowInfo = false;
            this.userControlModuleRelatedEntryDescriptorAgent.Size = new System.Drawing.Size(93, 22);
            this.userControlModuleRelatedEntryDescriptorAgent.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryDescriptorAgent.TabIndex = 0;
            this.userControlModuleRelatedEntryDescriptorAgent.Visible = false;
            // 
            // textBoxDescriptor
            // 
            this.textBoxDescriptor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescriptor.Location = new System.Drawing.Point(3, 75);
            this.textBoxDescriptor.Name = "textBoxDescriptor";
            this.textBoxDescriptor.Size = new System.Drawing.Size(187, 20);
            this.textBoxDescriptor.TabIndex = 1;
            this.textBoxDescriptor.Visible = false;
            // 
            // buttonDescriptorURL
            // 
            this.buttonDescriptorURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDescriptorURL.FlatAppearance.BorderSize = 0;
            this.buttonDescriptorURL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDescriptorURL.Image = global::DiversityCollection.Resource.Browse;
            this.buttonDescriptorURL.Location = new System.Drawing.Point(427, 75);
            this.buttonDescriptorURL.Name = "buttonDescriptorURL";
            this.buttonDescriptorURL.Size = new System.Drawing.Size(24, 23);
            this.buttonDescriptorURL.TabIndex = 2;
            this.buttonDescriptorURL.UseVisualStyleBackColor = true;
            this.buttonDescriptorURL.Visible = false;
            this.buttonDescriptorURL.Click += new System.EventHandler(this.buttonDescriptorURL_Click);
            // 
            // textBoxDescriptorURL
            // 
            this.textBoxDescriptorURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescriptorURL.Location = new System.Drawing.Point(234, 75);
            this.textBoxDescriptorURL.Name = "textBoxDescriptorURL";
            this.textBoxDescriptorURL.Size = new System.Drawing.Size(187, 20);
            this.textBoxDescriptorURL.TabIndex = 3;
            this.textBoxDescriptorURL.Visible = false;
            // 
            // labelDescriptorURL
            // 
            this.labelDescriptorURL.AutoSize = true;
            this.labelDescriptorURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDescriptorURL.Location = new System.Drawing.Point(196, 72);
            this.labelDescriptorURL.Name = "labelDescriptorURL";
            this.labelDescriptorURL.Size = new System.Drawing.Size(32, 29);
            this.labelDescriptorURL.TabIndex = 4;
            this.labelDescriptorURL.Text = "URL:";
            this.labelDescriptorURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelDescriptorURL.Visible = false;
            // 
            // toolStripDescriptor
            // 
            this.toolStripDescriptor.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStripDescriptor.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripDescriptor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonDescriptorAdd,
            this.toolStripButtonDescriptorDelete,
            this.toolStripButtonDescriptorEditTypes});
            this.toolStripDescriptor.Location = new System.Drawing.Point(3, 16);
            this.toolStripDescriptor.Name = "toolStripDescriptor";
            this.toolStripDescriptor.Size = new System.Drawing.Size(24, 131);
            this.toolStripDescriptor.TabIndex = 3;
            this.toolStripDescriptor.Text = "toolStrip1";
            // 
            // toolStripButtonDescriptorAdd
            // 
            this.toolStripButtonDescriptorAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDescriptorAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonDescriptorAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDescriptorAdd.Name = "toolStripButtonDescriptorAdd";
            this.toolStripButtonDescriptorAdd.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonDescriptorAdd.Text = "Add a new descriptor";
            this.toolStripButtonDescriptorAdd.Click += new System.EventHandler(this.toolStripButtonDescriptorAdd_Click);
            // 
            // toolStripButtonDescriptorDelete
            // 
            this.toolStripButtonDescriptorDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDescriptorDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonDescriptorDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDescriptorDelete.Name = "toolStripButtonDescriptorDelete";
            this.toolStripButtonDescriptorDelete.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonDescriptorDelete.Text = "Remove the selected descriptor";
            this.toolStripButtonDescriptorDelete.Click += new System.EventHandler(this.toolStripButtonDescriptorDelete_Click);
            // 
            // toolStripButtonDescriptorEditTypes
            // 
            this.toolStripButtonDescriptorEditTypes.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonDescriptorEditTypes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDescriptorEditTypes.Image = global::DiversityCollection.Resource.KeyBlue;
            this.toolStripButtonDescriptorEditTypes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDescriptorEditTypes.Name = "toolStripButtonDescriptorEditTypes";
            this.toolStripButtonDescriptorEditTypes.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonDescriptorEditTypes.Text = "Edit the descriptor types";
            this.toolStripButtonDescriptorEditTypes.Click += new System.EventHandler(this.toolStripButtonDescriptorEditTypes_Click);
            // 
            // UserControl_EventSeriesDescriptor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxDescirptor);
            this.Name = "UserControl_EventSeriesDescriptor";
            this.Size = new System.Drawing.Size(484, 150);
            this.groupBoxDescirptor.ResumeLayout(false);
            this.groupBoxDescirptor.PerformLayout();
            this.tableLayoutPanelDescriptor.ResumeLayout(false);
            this.tableLayoutPanelDescriptor.PerformLayout();
            this.panelDescriptorModules.ResumeLayout(false);
            this.toolStripDescriptor.ResumeLayout(false);
            this.toolStripDescriptor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxDescirptor;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDescriptor;
        private System.Windows.Forms.TreeView treeViewDescriptor;
        private System.Windows.Forms.Panel panelDescriptorModules;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryDescriptorPlots;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryDescriptorTerms;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryDescriptorReference;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryDescriptorTaxa;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryDescriptorProject;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryDescriptorGazetteer;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryDescriptorCollection;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryDescriptorAgent;
        private System.Windows.Forms.TextBox textBoxDescriptor;
        private System.Windows.Forms.Button buttonDescriptorURL;
        private System.Windows.Forms.TextBox textBoxDescriptorURL;
        private System.Windows.Forms.Label labelDescriptorURL;
        private System.Windows.Forms.ToolStrip toolStripDescriptor;
        private System.Windows.Forms.ToolStripButton toolStripButtonDescriptorAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonDescriptorDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonDescriptorEditTypes;
        private System.Windows.Forms.ImageList imageListDescriptor;
    }
}
