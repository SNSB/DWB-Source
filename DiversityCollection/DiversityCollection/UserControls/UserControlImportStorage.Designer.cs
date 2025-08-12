namespace DiversityCollection.UserControls
{
    partial class UserControlImportStorage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlImportStorage));
            this.imageListStorage = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.panelSelectionStorage = new System.Windows.Forms.Panel();
            this.buttonRecover = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.tabControlImportSteps = new System.Windows.Forms.TabControl();
            this.tabPageStorage1 = new System.Windows.Forms.TabPage();
            this.tabControlStorage = new System.Windows.Forms.TabControl();
            this.tabPageStorage = new System.Windows.Forms.TabPage();
            this.tabControlStorageDetails = new System.Windows.Forms.TabControl();
            this.tabPageIdentifier = new System.Windows.Forms.TabPage();
            this.tabPageCollection = new System.Windows.Forms.TabPage();
            this.tabPagePreparation = new System.Windows.Forms.TabPage();
            this.tabPageTransaction = new System.Windows.Forms.TabPage();
            this.tabPageProcessing = new System.Windows.Forms.TabPage();
            this.userControlImportTransaction = new DiversityCollection.UserControls.UserControlImportTransaction();
            this.userControlImportProcessing = new DiversityCollection.UserControls.UserControlImportProcessing();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panelSelectionStorage.SuspendLayout();
            this.tabControlImportSteps.SuspendLayout();
            this.tabPageStorage1.SuspendLayout();
            this.tabControlStorage.SuspendLayout();
            this.tabPageStorage.SuspendLayout();
            this.tabControlStorageDetails.SuspendLayout();
            this.tabPageTransaction.SuspendLayout();
            this.tabPageProcessing.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListStorage
            // 
            this.imageListStorage.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListStorage.ImageStream")));
            this.imageListStorage.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListStorage.Images.SetKeyName(0, "Collection.ico");
            this.imageListStorage.Images.SetKeyName(1, "Processing.ico");
            this.imageListStorage.Images.SetKeyName(2, "Transaction.ico");
            // 
            // splitContainer
            // 
            this.splitContainer.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.panelSelectionStorage);
            this.splitContainer.Panel1.Controls.Add(this.buttonRemove);
            this.splitContainer.Panel1.Controls.Add(this.buttonAdd);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tabControlImportSteps);
            this.splitContainer.Size = new System.Drawing.Size(646, 216);
            this.splitContainer.SplitterDistance = 40;
            this.splitContainer.TabIndex = 2;
            // 
            // panelSelectionStorage
            // 
            this.panelSelectionStorage.Controls.Add(this.buttonRecover);
            this.panelSelectionStorage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSelectionStorage.Location = new System.Drawing.Point(0, 48);
            this.panelSelectionStorage.Name = "panelSelectionStorage";
            this.panelSelectionStorage.Size = new System.Drawing.Size(40, 168);
            this.panelSelectionStorage.TabIndex = 3;
            // 
            // buttonRecover
            // 
            this.buttonRecover.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonRecover.Image = global::DiversityCollection.Resource.Transfrom;
            this.buttonRecover.Location = new System.Drawing.Point(0, 0);
            this.buttonRecover.Name = "buttonRecover";
            this.buttonRecover.Size = new System.Drawing.Size(40, 24);
            this.buttonRecover.TabIndex = 3;
            this.buttonRecover.UseVisualStyleBackColor = true;
            this.buttonRecover.Visible = false;
            this.buttonRecover.Click += new System.EventHandler(this.buttonRecover_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonRemove.Image = global::DiversityCollection.Resource.Remove;
            this.buttonRemove.Location = new System.Drawing.Point(0, 24);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(40, 24);
            this.buttonRemove.TabIndex = 2;
            this.buttonRemove.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Visible = false;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonAdd.Image = global::DiversityCollection.Resource.Add1;
            this.buttonAdd.Location = new System.Drawing.Point(0, 0);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(40, 24);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // tabControlImportSteps
            // 
            this.tabControlImportSteps.Controls.Add(this.tabPageStorage1);
            this.tabControlImportSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlImportSteps.Location = new System.Drawing.Point(0, 0);
            this.tabControlImportSteps.Name = "tabControlImportSteps";
            this.tabControlImportSteps.SelectedIndex = 0;
            this.tabControlImportSteps.Size = new System.Drawing.Size(602, 216);
            this.tabControlImportSteps.TabIndex = 2;
            // 
            // tabPageStorage1
            // 
            this.tabPageStorage1.Controls.Add(this.tabControlStorage);
            this.tabPageStorage1.Location = new System.Drawing.Point(4, 22);
            this.tabPageStorage1.Name = "tabPageStorage1";
            this.tabPageStorage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStorage1.Size = new System.Drawing.Size(594, 190);
            this.tabPageStorage1.TabIndex = 0;
            this.tabPageStorage1.Text = "Storage 1";
            this.tabPageStorage1.UseVisualStyleBackColor = true;
            // 
            // tabControlStorage
            // 
            this.tabControlStorage.Controls.Add(this.tabPageStorage);
            this.tabControlStorage.Controls.Add(this.tabPageTransaction);
            this.tabControlStorage.Controls.Add(this.tabPageProcessing);
            this.tabControlStorage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlStorage.ImageList = this.imageListStorage;
            this.tabControlStorage.Location = new System.Drawing.Point(3, 3);
            this.tabControlStorage.Name = "tabControlStorage";
            this.tabControlStorage.SelectedIndex = 0;
            this.tabControlStorage.Size = new System.Drawing.Size(588, 184);
            this.tabControlStorage.TabIndex = 1;
            // 
            // tabPageStorage
            // 
            this.tabPageStorage.Controls.Add(this.tabControlStorageDetails);
            this.tabPageStorage.ImageIndex = 0;
            this.tabPageStorage.Location = new System.Drawing.Point(4, 23);
            this.tabPageStorage.Name = "tabPageStorage";
            this.tabPageStorage.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStorage.Size = new System.Drawing.Size(580, 157);
            this.tabPageStorage.TabIndex = 0;
            this.tabPageStorage.Text = "Storage";
            this.tabPageStorage.UseVisualStyleBackColor = true;
            // 
            // tabControlStorageDetails
            // 
            this.tabControlStorageDetails.Controls.Add(this.tabPageIdentifier);
            this.tabControlStorageDetails.Controls.Add(this.tabPageCollection);
            this.tabControlStorageDetails.Controls.Add(this.tabPagePreparation);
            this.tabControlStorageDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlStorageDetails.Location = new System.Drawing.Point(3, 3);
            this.tabControlStorageDetails.Name = "tabControlStorageDetails";
            this.tabControlStorageDetails.SelectedIndex = 0;
            this.tabControlStorageDetails.Size = new System.Drawing.Size(574, 151);
            this.tabControlStorageDetails.TabIndex = 0;
            // 
            // tabPageIdentifier
            // 
            this.tabPageIdentifier.Location = new System.Drawing.Point(4, 22);
            this.tabPageIdentifier.Name = "tabPageIdentifier";
            this.tabPageIdentifier.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageIdentifier.Size = new System.Drawing.Size(566, 125);
            this.tabPageIdentifier.TabIndex = 0;
            this.tabPageIdentifier.Text = "Identifier and numbers";
            this.tabPageIdentifier.UseVisualStyleBackColor = true;
            // 
            // tabPageCollection
            // 
            this.tabPageCollection.Location = new System.Drawing.Point(4, 22);
            this.tabPageCollection.Name = "tabPageCollection";
            this.tabPageCollection.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCollection.Size = new System.Drawing.Size(566, 125);
            this.tabPageCollection.TabIndex = 1;
            this.tabPageCollection.Text = "Collection etc.";
            this.tabPageCollection.UseVisualStyleBackColor = true;
            // 
            // tabPagePreparation
            // 
            this.tabPagePreparation.Location = new System.Drawing.Point(4, 22);
            this.tabPagePreparation.Name = "tabPagePreparation";
            this.tabPagePreparation.Size = new System.Drawing.Size(566, 125);
            this.tabPagePreparation.TabIndex = 2;
            this.tabPagePreparation.Text = "Preparation";
            this.tabPagePreparation.UseVisualStyleBackColor = true;
            // 
            // tabPageTransaction
            // 
            this.tabPageTransaction.Controls.Add(this.userControlImportTransaction);
            this.tabPageTransaction.ImageIndex = 2;
            this.tabPageTransaction.Location = new System.Drawing.Point(4, 23);
            this.tabPageTransaction.Name = "tabPageTransaction";
            this.tabPageTransaction.Size = new System.Drawing.Size(580, 157);
            this.tabPageTransaction.TabIndex = 2;
            this.tabPageTransaction.Text = "Transaction";
            this.tabPageTransaction.UseVisualStyleBackColor = true;
            // 
            // tabPageProcessing
            // 
            this.tabPageProcessing.Controls.Add(this.userControlImportProcessing);
            this.tabPageProcessing.ImageIndex = 1;
            this.tabPageProcessing.Location = new System.Drawing.Point(4, 23);
            this.tabPageProcessing.Name = "tabPageProcessing";
            this.tabPageProcessing.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProcessing.Size = new System.Drawing.Size(580, 157);
            this.tabPageProcessing.TabIndex = 1;
            this.tabPageProcessing.Text = "Processing";
            this.tabPageProcessing.UseVisualStyleBackColor = true;
            // 
            // userControlImportTransaction
            // 
            this.userControlImportTransaction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlImportTransaction.Location = new System.Drawing.Point(0, 0);
            this.userControlImportTransaction.Name = "userControlImportTransaction";
            this.userControlImportTransaction.Size = new System.Drawing.Size(580, 157);
            this.userControlImportTransaction.TabIndex = 0;
            // 
            // userControlImportProcessing
            // 
            this.userControlImportProcessing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlImportProcessing.Location = new System.Drawing.Point(3, 3);
            this.userControlImportProcessing.Name = "userControlImportProcessing";
            this.userControlImportProcessing.Size = new System.Drawing.Size(574, 151);
            this.userControlImportProcessing.TabIndex = 0;
            // 
            // UserControlImportStorage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Name = "UserControlImportStorage";
            this.Size = new System.Drawing.Size(646, 216);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.panelSelectionStorage.ResumeLayout(false);
            this.tabControlImportSteps.ResumeLayout(false);
            this.tabPageStorage1.ResumeLayout(false);
            this.tabControlStorage.ResumeLayout(false);
            this.tabPageStorage.ResumeLayout(false);
            this.tabControlStorageDetails.ResumeLayout(false);
            this.tabPageTransaction.ResumeLayout(false);
            this.tabPageProcessing.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TabControl tabControlStorage;
        public System.Windows.Forms.ImageList imageListStorage;
        public System.Windows.Forms.TabPage tabPageStorage;
        public System.Windows.Forms.TabPage tabPageProcessing;
        public System.Windows.Forms.TabPage tabPageTransaction;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TabControl tabControlImportSteps;
        private System.Windows.Forms.TabPage tabPageStorage1;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Panel panelSelectionStorage;
        private UserControlImportProcessing userControlImportProcessing;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonRecover;
        private UserControlImportTransaction userControlImportTransaction;
        private System.Windows.Forms.TabControl tabControlStorageDetails;
        private System.Windows.Forms.TabPage tabPageIdentifier;
        private System.Windows.Forms.TabPage tabPageCollection;
        private System.Windows.Forms.TabPage tabPagePreparation;
    }
}
