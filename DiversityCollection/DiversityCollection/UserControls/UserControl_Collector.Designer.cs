namespace DiversityCollection.UserControls
{
    partial class UserControl_Collector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_Collector));
            this.groupBoxCollector = new System.Windows.Forms.GroupBox();
            this.pictureBoxCollectionAgent = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanelCollector = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxCollectorNotes = new System.Windows.Forms.TextBox();
            this.labelCollectorNotes = new System.Windows.Forms.Label();
            this.textBoxCollectorsNumber = new System.Windows.Forms.TextBox();
            this.userControlModuleRelatedEntryCollector = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.labelCollectorsNumber = new System.Windows.Forms.Label();
            this.labelCollectorDataWithholdingReason = new System.Windows.Forms.Label();
            this.comboBoxCollectorDataWithholdingReason = new System.Windows.Forms.ComboBox();
            this.pictureBoxCollectorDataWithholdingReason = new System.Windows.Forms.PictureBox();
            this.buttonTemplateCollectorSet = new System.Windows.Forms.Button();
            this.buttonTemplateCollectorEdit = new System.Windows.Forms.Button();
            this.groupBoxCollector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCollectionAgent)).BeginInit();
            this.tableLayoutPanelCollector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCollectorDataWithholdingReason)).BeginInit();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxCollector
            // 
            this.groupBoxCollector.AccessibleName = "CollectionAgent";
            this.groupBoxCollector.Controls.Add(this.pictureBoxCollectionAgent);
            this.groupBoxCollector.Controls.Add(this.tableLayoutPanelCollector);
            this.groupBoxCollector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxCollector.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxCollector.ForeColor = System.Drawing.Color.Blue;
            this.groupBoxCollector.Location = new System.Drawing.Point(0, 0);
            this.groupBoxCollector.MinimumSize = new System.Drawing.Size(0, 95);
            this.groupBoxCollector.Name = "groupBoxCollector";
            this.groupBoxCollector.Size = new System.Drawing.Size(518, 150);
            this.groupBoxCollector.TabIndex = 2;
            this.groupBoxCollector.TabStop = false;
            this.groupBoxCollector.Text = "Collector";
            // 
            // pictureBoxCollectionAgent
            // 
            this.pictureBoxCollectionAgent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxCollectionAgent.Image = global::DiversityCollection.Resource.Agent;
            this.pictureBoxCollectionAgent.Location = new System.Drawing.Point(499, 3);
            this.pictureBoxCollectionAgent.Name = "pictureBoxCollectionAgent";
            this.pictureBoxCollectionAgent.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxCollectionAgent.TabIndex = 1;
            this.pictureBoxCollectionAgent.TabStop = false;
            // 
            // tableLayoutPanelCollector
            // 
            this.tableLayoutPanelCollector.ColumnCount = 6;
            this.tableLayoutPanelCollector.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanelCollector.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelCollector.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCollector.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelCollector.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCollector.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanelCollector.Controls.Add(this.textBoxCollectorNotes, 2, 2);
            this.tableLayoutPanelCollector.Controls.Add(this.labelCollectorNotes, 1, 2);
            this.tableLayoutPanelCollector.Controls.Add(this.textBoxCollectorsNumber, 2, 1);
            this.tableLayoutPanelCollector.Controls.Add(this.userControlModuleRelatedEntryCollector, 1, 0);
            this.tableLayoutPanelCollector.Controls.Add(this.labelCollectorsNumber, 1, 1);
            this.tableLayoutPanelCollector.Controls.Add(this.labelCollectorDataWithholdingReason, 3, 1);
            this.tableLayoutPanelCollector.Controls.Add(this.comboBoxCollectorDataWithholdingReason, 4, 1);
            this.tableLayoutPanelCollector.Controls.Add(this.pictureBoxCollectorDataWithholdingReason, 5, 1);
            this.tableLayoutPanelCollector.Controls.Add(this.buttonTemplateCollectorSet, 0, 0);
            this.tableLayoutPanelCollector.Controls.Add(this.buttonTemplateCollectorEdit, 0, 1);
            this.tableLayoutPanelCollector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelCollector.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelCollector.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelCollector.Name = "tableLayoutPanelCollector";
            this.tableLayoutPanelCollector.RowCount = 3;
            this.tableLayoutPanelCollector.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollector.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollector.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelCollector.Size = new System.Drawing.Size(512, 131);
            this.tableLayoutPanelCollector.TabIndex = 0;
            // 
            // textBoxCollectorNotes
            // 
            this.tableLayoutPanelCollector.SetColumnSpan(this.textBoxCollectorNotes, 4);
            this.textBoxCollectorNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCollectorNotes.Location = new System.Drawing.Point(71, 58);
            this.textBoxCollectorNotes.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.textBoxCollectorNotes.Multiline = true;
            this.textBoxCollectorNotes.Name = "textBoxCollectorNotes";
            this.textBoxCollectorNotes.Size = new System.Drawing.Size(438, 73);
            this.textBoxCollectorNotes.TabIndex = 11;
            // 
            // labelCollectorNotes
            // 
            this.labelCollectorNotes.AccessibleName = "CollectionAgent.Notes";
            this.labelCollectorNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectorNotes.Location = new System.Drawing.Point(18, 61);
            this.labelCollectorNotes.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelCollectorNotes.Name = "labelCollectorNotes";
            this.labelCollectorNotes.Size = new System.Drawing.Size(53, 70);
            this.labelCollectorNotes.TabIndex = 6;
            this.labelCollectorNotes.Text = "Notes:";
            this.labelCollectorNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxCollectorsNumber
            // 
            this.textBoxCollectorsNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCollectorsNumber.Location = new System.Drawing.Point(71, 28);
            this.textBoxCollectorsNumber.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxCollectorsNumber.Name = "textBoxCollectorsNumber";
            this.textBoxCollectorsNumber.Size = new System.Drawing.Size(168, 20);
            this.textBoxCollectorsNumber.TabIndex = 10;
            // 
            // userControlModuleRelatedEntryCollector
            // 
            this.userControlModuleRelatedEntryCollector.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelCollector.SetColumnSpan(this.userControlModuleRelatedEntryCollector, 5);
            this.userControlModuleRelatedEntryCollector.DependsOnUri = "";
            this.userControlModuleRelatedEntryCollector.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlModuleRelatedEntryCollector.Domain = "";
            this.userControlModuleRelatedEntryCollector.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryCollector.Location = new System.Drawing.Point(18, 3);
            this.userControlModuleRelatedEntryCollector.Module = null;
            this.userControlModuleRelatedEntryCollector.Name = "userControlModuleRelatedEntryCollector";
            this.userControlModuleRelatedEntryCollector.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryCollector.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryCollector.ShowInfo = false;
            this.userControlModuleRelatedEntryCollector.Size = new System.Drawing.Size(491, 22);
            this.userControlModuleRelatedEntryCollector.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryCollector.TabIndex = 9;
            // 
            // labelCollectorsNumber
            // 
            this.labelCollectorsNumber.AccessibleName = "CollectionAgent.CollectorsNumber";
            this.labelCollectorsNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectorsNumber.Location = new System.Drawing.Point(18, 31);
            this.labelCollectorsNumber.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelCollectorsNumber.Name = "labelCollectorsNumber";
            this.labelCollectorsNumber.Size = new System.Drawing.Size(53, 27);
            this.labelCollectorsNumber.TabIndex = 4;
            this.labelCollectorsNumber.Text = "Col.No.:";
            this.labelCollectorsNumber.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelCollectorDataWithholdingReason
            // 
            this.labelCollectorDataWithholdingReason.AccessibleName = "CollectionAgent.DataWithholdingReason";
            this.labelCollectorDataWithholdingReason.AutoSize = true;
            this.labelCollectorDataWithholdingReason.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectorDataWithholdingReason.Location = new System.Drawing.Point(245, 31);
            this.labelCollectorDataWithholdingReason.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelCollectorDataWithholdingReason.Name = "labelCollectorDataWithholdingReason";
            this.labelCollectorDataWithholdingReason.Size = new System.Drawing.Size(73, 27);
            this.labelCollectorDataWithholdingReason.TabIndex = 12;
            this.labelCollectorDataWithholdingReason.Text = "Withh.reason:";
            this.labelCollectorDataWithholdingReason.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBoxCollectorDataWithholdingReason
            // 
            this.comboBoxCollectorDataWithholdingReason.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxCollectorDataWithholdingReason.DropDownWidth = 200;
            this.comboBoxCollectorDataWithholdingReason.FormattingEnabled = true;
            this.comboBoxCollectorDataWithholdingReason.Location = new System.Drawing.Point(318, 28);
            this.comboBoxCollectorDataWithholdingReason.Margin = new System.Windows.Forms.Padding(0, 0, 1, 3);
            this.comboBoxCollectorDataWithholdingReason.Name = "comboBoxCollectorDataWithholdingReason";
            this.comboBoxCollectorDataWithholdingReason.Size = new System.Drawing.Size(170, 21);
            this.comboBoxCollectorDataWithholdingReason.TabIndex = 13;
            this.comboBoxCollectorDataWithholdingReason.DropDown += new System.EventHandler(this.comboBoxCollectorDataWithholdingReason_DropDown);
            this.comboBoxCollectorDataWithholdingReason.TextChanged += new System.EventHandler(this.comboBoxCollectorDataWithholdingReason_TextChanged);
            // 
            // pictureBoxCollectorDataWithholdingReason
            // 
            this.pictureBoxCollectorDataWithholdingReason.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxCollectorDataWithholdingReason.Image = global::DiversityCollection.Resource.Stop3;
            this.pictureBoxCollectorDataWithholdingReason.Location = new System.Drawing.Point(489, 31);
            this.pictureBoxCollectorDataWithholdingReason.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.pictureBoxCollectorDataWithholdingReason.Name = "pictureBoxCollectorDataWithholdingReason";
            this.pictureBoxCollectorDataWithholdingReason.Size = new System.Drawing.Size(23, 27);
            this.pictureBoxCollectorDataWithholdingReason.TabIndex = 14;
            this.pictureBoxCollectorDataWithholdingReason.TabStop = false;
            // 
            // buttonTemplateCollectorSet
            // 
            this.buttonTemplateCollectorSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTemplateCollectorSet.FlatAppearance.BorderSize = 0;
            this.buttonTemplateCollectorSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTemplateCollectorSet.Image = global::DiversityCollection.Resource.Template;
            this.buttonTemplateCollectorSet.Location = new System.Drawing.Point(0, 0);
            this.buttonTemplateCollectorSet.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTemplateCollectorSet.Name = "buttonTemplateCollectorSet";
            this.buttonTemplateCollectorSet.Size = new System.Drawing.Size(15, 28);
            this.buttonTemplateCollectorSet.TabIndex = 15;
            this.buttonTemplateCollectorSet.UseVisualStyleBackColor = true;
            this.buttonTemplateCollectorSet.Click += new System.EventHandler(this.buttonTemplateCollectorSet_Click);
            // 
            // buttonTemplateCollectorEdit
            // 
            this.buttonTemplateCollectorEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTemplateCollectorEdit.FlatAppearance.BorderSize = 0;
            this.buttonTemplateCollectorEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTemplateCollectorEdit.Image = global::DiversityCollection.Resource.TemplateEditor;
            this.buttonTemplateCollectorEdit.Location = new System.Drawing.Point(0, 28);
            this.buttonTemplateCollectorEdit.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTemplateCollectorEdit.Name = "buttonTemplateCollectorEdit";
            this.buttonTemplateCollectorEdit.Size = new System.Drawing.Size(15, 30);
            this.buttonTemplateCollectorEdit.TabIndex = 16;
            this.buttonTemplateCollectorEdit.UseVisualStyleBackColor = true;
            this.buttonTemplateCollectorEdit.Click += new System.EventHandler(this.buttonTemplateCollectorEdit_Click);
            // 
            // UserControl_Collector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxCollector);
            this.Name = "UserControl_Collector";
            this.Size = new System.Drawing.Size(518, 150);
            this.groupBoxCollector.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCollectionAgent)).EndInit();
            this.tableLayoutPanelCollector.ResumeLayout(false);
            this.tableLayoutPanelCollector.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCollectorDataWithholdingReason)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxCollector;
        private System.Windows.Forms.PictureBox pictureBoxCollectionAgent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCollector;
        private System.Windows.Forms.TextBox textBoxCollectorNotes;
        private System.Windows.Forms.Label labelCollectorNotes;
        private System.Windows.Forms.TextBox textBoxCollectorsNumber;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryCollector;
        private System.Windows.Forms.Label labelCollectorsNumber;
        private System.Windows.Forms.Label labelCollectorDataWithholdingReason;
        private System.Windows.Forms.ComboBox comboBoxCollectorDataWithholdingReason;
        private System.Windows.Forms.PictureBox pictureBoxCollectorDataWithholdingReason;
        private System.Windows.Forms.Button buttonTemplateCollectorSet;
        private System.Windows.Forms.Button buttonTemplateCollectorEdit;
    }
}
