namespace DiversityCollection.UserControls
{
    partial class UserControlImportAnalysis
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlImportAnalysis));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.buttonRecover = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.tabControlImportSteps = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControlAnalysis = new System.Windows.Forms.TabControl();
            this.tabPageAnalysis = new System.Windows.Forms.TabPage();
            this.tabPageResponsible = new System.Windows.Forms.TabPage();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tabControlImportSteps.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControlAnalysis.SuspendLayout();
            this.SuspendLayout();
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
            this.splitContainer.Size = new System.Drawing.Size(401, 150);
            this.splitContainer.SplitterDistance = 32;
            this.splitContainer.TabIndex = 3;
            // 
            // buttonRecover
            // 
            this.buttonRecover.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonRecover.Image = global::DiversityCollection.Resource.Transfrom;
            this.buttonRecover.Location = new System.Drawing.Point(0, 48);
            this.buttonRecover.Name = "buttonRecover";
            this.buttonRecover.Size = new System.Drawing.Size(32, 24);
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
            this.buttonRemove.Size = new System.Drawing.Size(32, 24);
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
            this.buttonAdd.Size = new System.Drawing.Size(32, 24);
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
            this.tabControlImportSteps.Size = new System.Drawing.Size(365, 150);
            this.tabControlImportSteps.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tabControlAnalysis);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(357, 124);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Analysis 1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabControlAnalysis
            // 
            this.tabControlAnalysis.Controls.Add(this.tabPageAnalysis);
            this.tabControlAnalysis.Controls.Add(this.tabPageResponsible);
            this.tabControlAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlAnalysis.ImageList = this.imageList;
            this.tabControlAnalysis.Location = new System.Drawing.Point(3, 3);
            this.tabControlAnalysis.Name = "tabControlAnalysis";
            this.tabControlAnalysis.SelectedIndex = 0;
            this.tabControlAnalysis.Size = new System.Drawing.Size(351, 118);
            this.tabControlAnalysis.TabIndex = 1;
            // 
            // tabPageAnalysis
            // 
            this.tabPageAnalysis.ImageIndex = 1;
            this.tabPageAnalysis.Location = new System.Drawing.Point(4, 23);
            this.tabPageAnalysis.Name = "tabPageAnalysis";
            this.tabPageAnalysis.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAnalysis.Size = new System.Drawing.Size(343, 91);
            this.tabPageAnalysis.TabIndex = 0;
            this.tabPageAnalysis.Text = "Analysis";
            this.tabPageAnalysis.UseVisualStyleBackColor = true;
            // 
            // tabPageResponsible
            // 
            this.tabPageResponsible.ImageIndex = 0;
            this.tabPageResponsible.Location = new System.Drawing.Point(4, 23);
            this.tabPageResponsible.Name = "tabPageResponsible";
            this.tabPageResponsible.Size = new System.Drawing.Size(343, 91);
            this.tabPageResponsible.TabIndex = 2;
            this.tabPageResponsible.Text = "Responsible, date, notes";
            this.tabPageResponsible.UseVisualStyleBackColor = true;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Agent.ico");
            this.imageList.Images.SetKeyName(1, "Analysis.ico");
            // 
            // UserControlImportAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Name = "UserControlImportAnalysis";
            this.Size = new System.Drawing.Size(401, 150);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.tabControlImportSteps.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControlAnalysis.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.TabControl tabControlImportSteps;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControlAnalysis;
        private System.Windows.Forms.TabPage tabPageAnalysis;
        private System.Windows.Forms.TabPage tabPageResponsible;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonRecover;

    }
}
