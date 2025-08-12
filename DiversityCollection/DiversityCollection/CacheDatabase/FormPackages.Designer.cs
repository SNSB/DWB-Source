namespace DiversityCollection.CacheDatabase
{
    partial class FormPackages
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPackages));
            tableLayoutPanelPostgresPackages = new System.Windows.Forms.TableLayoutPanel();
            panelPostgresProjectPackages = new System.Windows.Forms.Panel();
            labelPostgresPackagesAvailable = new System.Windows.Forms.Label();
            labelPostgresPackagesEstablished = new System.Windows.Forms.Label();
            listBoxPostgresPackagesAvailable = new System.Windows.Forms.ListBox();
            buttonPostgresPackageEstablish = new System.Windows.Forms.Button();
            buttonFeedback = new System.Windows.Forms.Button();
            helpProvider = new System.Windows.Forms.HelpProvider();
            toolTip = new System.Windows.Forms.ToolTip(components);
            tableLayoutPanelPostgresPackages.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanelPostgresPackages
            // 
            tableLayoutPanelPostgresPackages.ColumnCount = 4;
            tableLayoutPanelPostgresPackages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelPostgresPackages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelPostgresPackages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelPostgresPackages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelPostgresPackages.Controls.Add(panelPostgresProjectPackages, 2, 1);
            tableLayoutPanelPostgresPackages.Controls.Add(labelPostgresPackagesAvailable, 0, 0);
            tableLayoutPanelPostgresPackages.Controls.Add(labelPostgresPackagesEstablished, 2, 0);
            tableLayoutPanelPostgresPackages.Controls.Add(listBoxPostgresPackagesAvailable, 0, 1);
            tableLayoutPanelPostgresPackages.Controls.Add(buttonPostgresPackageEstablish, 1, 1);
            tableLayoutPanelPostgresPackages.Controls.Add(buttonFeedback, 3, 0);
            tableLayoutPanelPostgresPackages.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(tableLayoutPanelPostgresPackages, "cachedatabase_packages_dc");
            tableLayoutPanelPostgresPackages.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelPostgresPackages.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelPostgresPackages.Name = "tableLayoutPanelPostgresPackages";
            tableLayoutPanelPostgresPackages.RowCount = 2;
            tableLayoutPanelPostgresPackages.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelPostgresPackages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            helpProvider.SetShowHelp(tableLayoutPanelPostgresPackages, true);
            tableLayoutPanelPostgresPackages.Size = new System.Drawing.Size(590, 302);
            tableLayoutPanelPostgresPackages.TabIndex = 2;
            // 
            // panelPostgresProjectPackages
            // 
            panelPostgresProjectPackages.AutoScroll = true;
            panelPostgresProjectPackages.BackColor = System.Drawing.Color.LightGreen;
            tableLayoutPanelPostgresPackages.SetColumnSpan(panelPostgresProjectPackages, 2);
            panelPostgresProjectPackages.Dock = System.Windows.Forms.DockStyle.Fill;
            panelPostgresProjectPackages.Location = new System.Drawing.Point(155, 21);
            panelPostgresProjectPackages.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panelPostgresProjectPackages.Name = "panelPostgresProjectPackages";
            panelPostgresProjectPackages.Size = new System.Drawing.Size(431, 278);
            panelPostgresProjectPackages.TabIndex = 0;
            // 
            // labelPostgresPackagesAvailable
            // 
            labelPostgresPackagesAvailable.AutoSize = true;
            labelPostgresPackagesAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            labelPostgresPackagesAvailable.Location = new System.Drawing.Point(4, 0);
            labelPostgresPackagesAvailable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelPostgresPackagesAvailable.Name = "labelPostgresPackagesAvailable";
            labelPostgresPackagesAvailable.Size = new System.Drawing.Size(116, 18);
            labelPostgresPackagesAvailable.TabIndex = 1;
            labelPostgresPackagesAvailable.Text = "Available";
            labelPostgresPackagesAvailable.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelPostgresPackagesEstablished
            // 
            labelPostgresPackagesEstablished.AutoSize = true;
            labelPostgresPackagesEstablished.Dock = System.Windows.Forms.DockStyle.Left;
            labelPostgresPackagesEstablished.Location = new System.Drawing.Point(155, 0);
            labelPostgresPackagesEstablished.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelPostgresPackagesEstablished.Name = "labelPostgresPackagesEstablished";
            labelPostgresPackagesEstablished.Size = new System.Drawing.Size(118, 18);
            labelPostgresPackagesEstablished.TabIndex = 2;
            labelPostgresPackagesEstablished.Text = "Established packages";
            labelPostgresPackagesEstablished.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // listBoxPostgresPackagesAvailable
            // 
            listBoxPostgresPackagesAvailable.BackColor = System.Drawing.Color.Pink;
            listBoxPostgresPackagesAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxPostgresPackagesAvailable.FormattingEnabled = true;
            listBoxPostgresPackagesAvailable.IntegralHeight = false;
            listBoxPostgresPackagesAvailable.ItemHeight = 15;
            listBoxPostgresPackagesAvailable.Location = new System.Drawing.Point(4, 21);
            listBoxPostgresPackagesAvailable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxPostgresPackagesAvailable.Name = "listBoxPostgresPackagesAvailable";
            listBoxPostgresPackagesAvailable.Size = new System.Drawing.Size(116, 278);
            listBoxPostgresPackagesAvailable.TabIndex = 3;
            toolTip.SetToolTip(listBoxPostgresPackagesAvailable, "Available packages");
            // 
            // buttonPostgresPackageEstablish
            // 
            buttonPostgresPackageEstablish.Dock = System.Windows.Forms.DockStyle.Top;
            buttonPostgresPackageEstablish.Image = Resource.ArrowNext1;
            buttonPostgresPackageEstablish.Location = new System.Drawing.Point(124, 21);
            buttonPostgresPackageEstablish.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            buttonPostgresPackageEstablish.Name = "buttonPostgresPackageEstablish";
            buttonPostgresPackageEstablish.Size = new System.Drawing.Size(27, 27);
            buttonPostgresPackageEstablish.TabIndex = 4;
            toolTip.SetToolTip(buttonPostgresPackageEstablish, "Establish the selected package");
            buttonPostgresPackageEstablish.UseVisualStyleBackColor = true;
            buttonPostgresPackageEstablish.Click += buttonPostgresPackageEstablish_Click;
            // 
            // buttonFeedback
            // 
            buttonFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonFeedback.FlatAppearance.BorderSize = 0;
            buttonFeedback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonFeedback.Image = Resource.Feedback;
            buttonFeedback.Location = new System.Drawing.Point(567, 0);
            buttonFeedback.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            buttonFeedback.Name = "buttonFeedback";
            buttonFeedback.Size = new System.Drawing.Size(19, 18);
            buttonFeedback.TabIndex = 5;
            toolTip.SetToolTip(buttonFeedback, "Send a feedback to the administrator");
            buttonFeedback.UseVisualStyleBackColor = true;
            buttonFeedback.Click += buttonFeedback_Click;
            // 
            // FormPackages
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(590, 302);
            Controls.Add(tableLayoutPanelPostgresPackages);
            helpProvider.SetHelpKeyword(this, "cachedatabase_packages_dc");
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormPackages";
            helpProvider.SetShowHelp(this, true);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "FormPackages";
            KeyDown += Form_KeyDown;
            tableLayoutPanelPostgresPackages.ResumeLayout(false);
            tableLayoutPanelPostgresPackages.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPostgresPackages;
        private System.Windows.Forms.Panel panelPostgresProjectPackages;
        private System.Windows.Forms.Label labelPostgresPackagesAvailable;
        private System.Windows.Forms.Label labelPostgresPackagesEstablished;
        private System.Windows.Forms.ListBox listBoxPostgresPackagesAvailable;
        private System.Windows.Forms.Button buttonPostgresPackageEstablish;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.ToolTip toolTip;
    }
}