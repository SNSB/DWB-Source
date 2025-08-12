namespace DiversityCollection.UserControls
{
    partial class UserControlImportUnit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlImportUnit));
            this.imageListUnit = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.panelSelectionUnit = new System.Windows.Forms.Panel();
            this.buttonRecover = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.tabControlImportSteps = new System.Windows.Forms.TabControl();
            this.tabPageUnit = new System.Windows.Forms.TabPage();
            this.tabControlIdentificationUnit = new System.Windows.Forms.TabControl();
            this.tabPageIdentifications = new System.Windows.Forms.TabPage();
            this.tabPageAnalysis = new System.Windows.Forms.TabPage();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.userControlImportIdentification = new DiversityCollection.UserControls.UserControlImportIdentification();
            this.userControlImportAnalysis = new DiversityCollection.UserControls.UserControlImportAnalysis();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tabControlImportSteps.SuspendLayout();
            this.tabPageUnit.SuspendLayout();
            this.tabControlIdentificationUnit.SuspendLayout();
            this.tabPageIdentifications.SuspendLayout();
            this.tabPageAnalysis.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListUnit
            // 
            this.imageListUnit.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListUnit.ImageStream")));
            this.imageListUnit.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListUnit.Images.SetKeyName(0, "Agent.ico");
            this.imageListUnit.Images.SetKeyName(1, "Analysis.ico");
            this.imageListUnit.Images.SetKeyName(2, "Identification.ico");
            this.imageListUnit.Images.SetKeyName(3, "Note.ico");
            this.imageListUnit.Images.SetKeyName(4, "Plant.ico");
            this.imageListUnit.Images.SetKeyName(5, "References.ico");
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
            this.splitContainer.Panel1.Controls.Add(this.panelSelectionUnit);
            this.splitContainer.Panel1.Controls.Add(this.buttonRecover);
            this.splitContainer.Panel1.Controls.Add(this.buttonRemove);
            this.splitContainer.Panel1.Controls.Add(this.buttonAdd);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tabControlImportSteps);
            this.splitContainer.Size = new System.Drawing.Size(697, 234);
            this.splitContainer.SplitterDistance = 39;
            this.splitContainer.TabIndex = 1;
            // 
            // panelSelectionUnit
            // 
            this.panelSelectionUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSelectionUnit.Location = new System.Drawing.Point(0, 72);
            this.panelSelectionUnit.Name = "panelSelectionUnit";
            this.panelSelectionUnit.Size = new System.Drawing.Size(39, 162);
            this.panelSelectionUnit.TabIndex = 2;
            // 
            // buttonRecover
            // 
            this.buttonRecover.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonRecover.Image = global::DiversityCollection.Resource.Transfrom;
            this.buttonRecover.Location = new System.Drawing.Point(0, 48);
            this.buttonRecover.Name = "buttonRecover";
            this.buttonRecover.Size = new System.Drawing.Size(39, 24);
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
            this.buttonRemove.Size = new System.Drawing.Size(39, 24);
            this.buttonRemove.TabIndex = 1;
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
            this.buttonAdd.Size = new System.Drawing.Size(39, 24);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // tabControlImportSteps
            // 
            this.tabControlImportSteps.Controls.Add(this.tabPageUnit);
            this.tabControlImportSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlImportSteps.ImageList = this.imageListUnit;
            this.tabControlImportSteps.Location = new System.Drawing.Point(0, 0);
            this.tabControlImportSteps.Name = "tabControlImportSteps";
            this.tabControlImportSteps.SelectedIndex = 0;
            this.tabControlImportSteps.Size = new System.Drawing.Size(654, 234);
            this.tabControlImportSteps.TabIndex = 0;
            // 
            // tabPageUnit
            // 
            this.tabPageUnit.Controls.Add(this.tabControlIdentificationUnit);
            this.tabPageUnit.ImageIndex = 4;
            this.tabPageUnit.Location = new System.Drawing.Point(4, 23);
            this.tabPageUnit.Name = "tabPageUnit";
            this.tabPageUnit.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUnit.Size = new System.Drawing.Size(646, 207);
            this.tabPageUnit.TabIndex = 0;
            this.tabPageUnit.Text = "Organism 1";
            this.tabPageUnit.UseVisualStyleBackColor = true;
            // 
            // tabControlIdentificationUnit
            // 
            this.tabControlIdentificationUnit.Controls.Add(this.tabPageIdentifications);
            this.tabControlIdentificationUnit.Controls.Add(this.tabPageAnalysis);
            this.tabControlIdentificationUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlIdentificationUnit.ImageList = this.imageListUnit;
            this.tabControlIdentificationUnit.Location = new System.Drawing.Point(3, 3);
            this.tabControlIdentificationUnit.Name = "tabControlIdentificationUnit";
            this.tabControlIdentificationUnit.SelectedIndex = 0;
            this.tabControlIdentificationUnit.Size = new System.Drawing.Size(640, 201);
            this.tabControlIdentificationUnit.TabIndex = 1;
            // 
            // tabPageIdentifications
            // 
            this.tabPageIdentifications.Controls.Add(this.userControlImportIdentification);
            this.tabPageIdentifications.ImageIndex = 2;
            this.tabPageIdentifications.Location = new System.Drawing.Point(4, 23);
            this.tabPageIdentifications.Name = "tabPageIdentifications";
            this.tabPageIdentifications.Size = new System.Drawing.Size(632, 174);
            this.tabPageIdentifications.TabIndex = 1;
            this.tabPageIdentifications.Text = "Identifications";
            this.tabPageIdentifications.UseVisualStyleBackColor = true;
            // 
            // tabPageAnalysis
            // 
            this.tabPageAnalysis.Controls.Add(this.userControlImportAnalysis);
            this.tabPageAnalysis.ImageIndex = 1;
            this.tabPageAnalysis.Location = new System.Drawing.Point(4, 23);
            this.tabPageAnalysis.Name = "tabPageAnalysis";
            this.tabPageAnalysis.Size = new System.Drawing.Size(632, 174);
            this.tabPageAnalysis.TabIndex = 2;
            this.tabPageAnalysis.Text = "Analysis";
            this.tabPageAnalysis.UseVisualStyleBackColor = true;
            // 
            // userControlImportIdentification
            // 
            this.userControlImportIdentification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlImportIdentification.Location = new System.Drawing.Point(0, 0);
            this.userControlImportIdentification.Name = "userControlImportIdentification";
            this.userControlImportIdentification.Size = new System.Drawing.Size(632, 174);
            this.userControlImportIdentification.TabIndex = 0;
            // 
            // userControlImportAnalysis
            // 
            this.userControlImportAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlImportAnalysis.Location = new System.Drawing.Point(0, 0);
            this.userControlImportAnalysis.Name = "userControlImportAnalysis";
            this.userControlImportAnalysis.Size = new System.Drawing.Size(632, 174);
            this.userControlImportAnalysis.TabIndex = 0;
            // 
            // UserControlImportUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Name = "UserControlImportUnit";
            this.Size = new System.Drawing.Size(697, 234);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.tabControlImportSteps.ResumeLayout(false);
            this.tabPageUnit.ResumeLayout(false);
            this.tabControlIdentificationUnit.ResumeLayout(false);
            this.tabPageIdentifications.ResumeLayout(false);
            this.tabPageAnalysis.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageListUnit;
        public System.Windows.Forms.TabControl tabControlImportSteps;
        public System.Windows.Forms.TabPage tabPageUnit;
        private System.Windows.Forms.TabControl tabControlIdentificationUnit;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.TabPage tabPageIdentifications;
        private UserControlImportIdentification userControlImportIdentification;
        private System.Windows.Forms.TabPage tabPageAnalysis;
        private System.Windows.Forms.Panel panelSelectionUnit;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonRecover;
        private UserControlImportAnalysis userControlImportAnalysis;
    }
}
