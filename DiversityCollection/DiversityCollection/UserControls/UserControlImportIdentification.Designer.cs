namespace DiversityCollection.UserControls
{
    partial class UserControlImportIdentification
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlImportIdentification));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.buttonRecover = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.tabControlImportSteps = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControlIdentification = new System.Windows.Forms.TabControl();
            this.tabPageName = new System.Windows.Forms.TabPage();
            this.tabPageDate = new System.Windows.Forms.TabPage();
            this.userControlImportDate = new DiversityCollection.UserControls.UserControlImportDate();
            this.tabPageResponsible = new System.Windows.Forms.TabPage();
            this.tabPageReference = new System.Windows.Forms.TabPage();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tabControlImportSteps.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControlIdentification.SuspendLayout();
            this.tabPageDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Identification.ico");
            this.imageList.Images.SetKeyName(1, "Agent.ico");
            this.imageList.Images.SetKeyName(2, "References.ico");
            this.imageList.Images.SetKeyName(3, "Time.ico");
            this.imageList.Images.SetKeyName(4, "Note.ico");
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
            this.splitContainer.Panel1.Controls.Add(this.buttonRecover);
            this.splitContainer.Panel1.Controls.Add(this.buttonRemove);
            this.splitContainer.Panel1.Controls.Add(this.buttonAdd);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tabControlImportSteps);
            this.splitContainer.Size = new System.Drawing.Size(556, 181);
            this.splitContainer.SplitterDistance = 45;
            this.splitContainer.TabIndex = 2;
            // 
            // buttonRecover
            // 
            this.buttonRecover.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonRecover.Image = global::DiversityCollection.Resource.Transfrom;
            this.buttonRecover.Location = new System.Drawing.Point(0, 48);
            this.buttonRecover.Name = "buttonRecover";
            this.buttonRecover.Size = new System.Drawing.Size(45, 24);
            this.buttonRecover.TabIndex = 2;
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
            this.buttonRemove.Size = new System.Drawing.Size(45, 24);
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
            this.buttonAdd.Size = new System.Drawing.Size(45, 24);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // tabControlImportSteps
            // 
            this.tabControlImportSteps.Controls.Add(this.tabPage1);
            this.tabControlImportSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlImportSteps.Location = new System.Drawing.Point(0, 0);
            this.tabControlImportSteps.Name = "tabControlImportSteps";
            this.tabControlImportSteps.SelectedIndex = 0;
            this.tabControlImportSteps.Size = new System.Drawing.Size(507, 181);
            this.tabControlImportSteps.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tabControlIdentification);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(499, 155);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Identification 1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabControlIdentification
            // 
            this.tabControlIdentification.Controls.Add(this.tabPageName);
            this.tabControlIdentification.Controls.Add(this.tabPageDate);
            this.tabControlIdentification.Controls.Add(this.tabPageResponsible);
            this.tabControlIdentification.Controls.Add(this.tabPageReference);
            this.tabControlIdentification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlIdentification.ImageList = this.imageList;
            this.tabControlIdentification.Location = new System.Drawing.Point(3, 3);
            this.tabControlIdentification.Name = "tabControlIdentification";
            this.tabControlIdentification.SelectedIndex = 0;
            this.tabControlIdentification.Size = new System.Drawing.Size(493, 149);
            this.tabControlIdentification.TabIndex = 1;
            // 
            // tabPageName
            // 
            this.tabPageName.BackColor = System.Drawing.Color.White;
            this.tabPageName.ImageIndex = 0;
            this.tabPageName.Location = new System.Drawing.Point(4, 23);
            this.tabPageName.Name = "tabPageName";
            this.tabPageName.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageName.Size = new System.Drawing.Size(485, 122);
            this.tabPageName.TabIndex = 0;
            this.tabPageName.Text = "Taxonomic name";
            // 
            // tabPageDate
            // 
            this.tabPageDate.BackColor = System.Drawing.Color.White;
            this.tabPageDate.Controls.Add(this.userControlImportDate);
            this.tabPageDate.ImageIndex = 3;
            this.tabPageDate.Location = new System.Drawing.Point(4, 23);
            this.tabPageDate.Name = "tabPageDate";
            this.tabPageDate.Size = new System.Drawing.Size(485, 122);
            this.tabPageDate.TabIndex = 4;
            this.tabPageDate.Text = "Date";
            // 
            // userControlImportDate
            // 
            this.userControlImportDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlImportDate.Location = new System.Drawing.Point(0, 0);
            this.userControlImportDate.Name = "userControlImportDate";
            this.userControlImportDate.Size = new System.Drawing.Size(485, 98);
            this.userControlImportDate.TabIndex = 0;
            // 
            // tabPageResponsible
            // 
            this.tabPageResponsible.BackColor = System.Drawing.Color.White;
            this.tabPageResponsible.ImageIndex = 1;
            this.tabPageResponsible.Location = new System.Drawing.Point(4, 23);
            this.tabPageResponsible.Name = "tabPageResponsible";
            this.tabPageResponsible.Size = new System.Drawing.Size(485, 122);
            this.tabPageResponsible.TabIndex = 2;
            this.tabPageResponsible.Text = "Responsible, date";
            // 
            // tabPageReference
            // 
            this.tabPageReference.BackColor = System.Drawing.Color.White;
            this.tabPageReference.ImageIndex = 2;
            this.tabPageReference.Location = new System.Drawing.Point(4, 23);
            this.tabPageReference.Name = "tabPageReference";
            this.tabPageReference.Size = new System.Drawing.Size(485, 122);
            this.tabPageReference.TabIndex = 3;
            this.tabPageReference.Text = "Reference, type, notes";
            // 
            // UserControlImportIdentification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Name = "UserControlImportIdentification";
            this.Size = new System.Drawing.Size(556, 181);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.tabControlImportSteps.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControlIdentification.ResumeLayout(false);
            this.tabPageDate.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlIdentification;
        private System.Windows.Forms.TabPage tabPageName;
        private System.Windows.Forms.TabPage tabPageResponsible;
        private System.Windows.Forms.TabPage tabPageReference;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.TabControl tabControlImportSteps;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPageDate;
        private System.Windows.Forms.Button buttonRecover;
        private System.Windows.Forms.ToolTip toolTip;
        private UserControlImportDate userControlImportDate;
    }
}
