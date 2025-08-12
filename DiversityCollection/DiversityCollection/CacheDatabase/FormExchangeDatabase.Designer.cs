namespace DiversityCollection.CacheDatabase
{
    partial class FormExchangeDatabase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExchangeDatabase));
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            labelHeader = new System.Windows.Forms.Label();
            buttonExchange = new System.Windows.Forms.Button();
            labelExchangedDatabase = new System.Windows.Forms.Label();
            comboBoxExchangedDatabase = new System.Windows.Forms.ComboBox();
            buttonCancel = new System.Windows.Forms.Button();
            buttonFeedback = new System.Windows.Forms.Button();
            tabControlCongruence = new System.Windows.Forms.TabControl();
            tabPageSources = new System.Windows.Forms.TabPage();
            tabControlSources = new System.Windows.Forms.TabControl();
            tabPageAgents = new System.Windows.Forms.TabPage();
            tabPageGazetteer = new System.Windows.Forms.TabPage();
            tabPagePlots = new System.Windows.Forms.TabPage();
            tabPageReferences = new System.Windows.Forms.TabPage();
            tabPageTaxa = new System.Windows.Forms.TabPage();
            tabPageTerms = new System.Windows.Forms.TabPage();
            imageList = new System.Windows.Forms.ImageList(components);
            tabPageWebservices = new System.Windows.Forms.TabPage();
            tabControlWebservices = new System.Windows.Forms.TabControl();
            tabPageTaxaWebservice = new System.Windows.Forms.TabPage();
            tabPageProjects = new System.Windows.Forms.TabPage();
            tabControlProjects = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            tabPage2 = new System.Windows.Forms.TabPage();
            buttonEnableIncongruentExchange = new System.Windows.Forms.Button();
            helpProvider = new System.Windows.Forms.HelpProvider();
            tableLayoutPanel.SuspendLayout();
            tabControlCongruence.SuspendLayout();
            tabPageSources.SuspendLayout();
            tabControlSources.SuspendLayout();
            tabPageWebservices.SuspendLayout();
            tabControlWebservices.SuspendLayout();
            tabPageProjects.SuspendLayout();
            tabControlProjects.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 4;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.Controls.Add(labelHeader, 0, 0);
            tableLayoutPanel.Controls.Add(buttonExchange, 1, 3);
            tableLayoutPanel.Controls.Add(labelExchangedDatabase, 0, 1);
            tableLayoutPanel.Controls.Add(comboBoxExchangedDatabase, 2, 1);
            tableLayoutPanel.Controls.Add(buttonCancel, 0, 4);
            tableLayoutPanel.Controls.Add(buttonFeedback, 3, 0);
            tableLayoutPanel.Controls.Add(tabControlCongruence, 0, 2);
            tableLayoutPanel.Controls.Add(buttonEnableIncongruentExchange, 0, 3);
            tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 5;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            tableLayoutPanel.Size = new System.Drawing.Size(528, 380);
            tableLayoutPanel.TabIndex = 0;
            // 
            // labelHeader
            // 
            labelHeader.AutoSize = true;
            tableLayoutPanel.SetColumnSpan(labelHeader, 3);
            labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHeader.Location = new System.Drawing.Point(7, 7);
            labelHeader.Margin = new System.Windows.Forms.Padding(7);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new System.Drawing.Size(478, 20);
            labelHeader.TabIndex = 1;
            labelHeader.Text = "label1";
            labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonExchange
            // 
            tableLayoutPanel.SetColumnSpan(buttonExchange, 3);
            buttonExchange.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonExchange.Enabled = false;
            buttonExchange.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            buttonExchange.Location = new System.Drawing.Point(100, 295);
            buttonExchange.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonExchange.Name = "buttonExchange";
            tableLayoutPanel.SetRowSpan(buttonExchange, 2);
            buttonExchange.Size = new System.Drawing.Size(424, 82);
            buttonExchange.TabIndex = 0;
            buttonExchange.Text = "Replace database";
            buttonExchange.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            buttonExchange.UseVisualStyleBackColor = true;
            buttonExchange.Click += buttonExchange_Click;
            // 
            // labelExchangedDatabase
            // 
            labelExchangedDatabase.AutoSize = true;
            tableLayoutPanel.SetColumnSpan(labelExchangedDatabase, 2);
            labelExchangedDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            labelExchangedDatabase.Location = new System.Drawing.Point(4, 34);
            labelExchangedDatabase.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelExchangedDatabase.Name = "labelExchangedDatabase";
            labelExchangedDatabase.Size = new System.Drawing.Size(185, 29);
            labelExchangedDatabase.TabIndex = 3;
            labelExchangedDatabase.Text = "Database that should be replaced:";
            labelExchangedDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxExchangedDatabase
            // 
            tableLayoutPanel.SetColumnSpan(comboBoxExchangedDatabase, 2);
            comboBoxExchangedDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxExchangedDatabase.FormattingEnabled = true;
            comboBoxExchangedDatabase.Location = new System.Drawing.Point(189, 37);
            comboBoxExchangedDatabase.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            comboBoxExchangedDatabase.Name = "comboBoxExchangedDatabase";
            comboBoxExchangedDatabase.Size = new System.Drawing.Size(335, 23);
            comboBoxExchangedDatabase.TabIndex = 2;
            comboBoxExchangedDatabase.SelectionChangeCommitted += comboBoxExchangedDatabase_SelectionChangeCommitted;
            // 
            // buttonCancel
            // 
            buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonCancel.Location = new System.Drawing.Point(4, 352);
            buttonCancel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 3);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new System.Drawing.Size(88, 25);
            buttonCancel.TabIndex = 4;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonFeedback
            // 
            buttonFeedback.Image = Resource.Feedback;
            buttonFeedback.Location = new System.Drawing.Point(496, 3);
            buttonFeedback.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonFeedback.Name = "buttonFeedback";
            buttonFeedback.Size = new System.Drawing.Size(28, 28);
            buttonFeedback.TabIndex = 5;
            buttonFeedback.UseVisualStyleBackColor = true;
            buttonFeedback.Click += buttonFeedback_Click;
            // 
            // tabControlCongruence
            // 
            tableLayoutPanel.SetColumnSpan(tabControlCongruence, 4);
            tabControlCongruence.Controls.Add(tabPageSources);
            tabControlCongruence.Controls.Add(tabPageWebservices);
            tabControlCongruence.Controls.Add(tabPageProjects);
            tabControlCongruence.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlCongruence.ImageList = imageList;
            tabControlCongruence.Location = new System.Drawing.Point(4, 66);
            tabControlCongruence.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabControlCongruence.Name = "tabControlCongruence";
            tabControlCongruence.SelectedIndex = 0;
            tabControlCongruence.Size = new System.Drawing.Size(520, 223);
            tabControlCongruence.TabIndex = 6;
            // 
            // tabPageSources
            // 
            tabPageSources.Controls.Add(tabControlSources);
            tabPageSources.Location = new System.Drawing.Point(4, 24);
            tabPageSources.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageSources.Name = "tabPageSources";
            tabPageSources.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageSources.Size = new System.Drawing.Size(512, 195);
            tabPageSources.TabIndex = 0;
            tabPageSources.Text = "Sources";
            tabPageSources.UseVisualStyleBackColor = true;
            // 
            // tabControlSources
            // 
            tabControlSources.Controls.Add(tabPageAgents);
            tabControlSources.Controls.Add(tabPageGazetteer);
            tabControlSources.Controls.Add(tabPagePlots);
            tabControlSources.Controls.Add(tabPageReferences);
            tabControlSources.Controls.Add(tabPageTaxa);
            tabControlSources.Controls.Add(tabPageTerms);
            tabControlSources.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlSources.ImageList = imageList;
            tabControlSources.Location = new System.Drawing.Point(4, 3);
            tabControlSources.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabControlSources.Name = "tabControlSources";
            tabControlSources.SelectedIndex = 0;
            tabControlSources.Size = new System.Drawing.Size(504, 189);
            tabControlSources.TabIndex = 0;
            // 
            // tabPageAgents
            // 
            tabPageAgents.AutoScroll = true;
            tabPageAgents.Location = new System.Drawing.Point(4, 24);
            tabPageAgents.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageAgents.Name = "tabPageAgents";
            tabPageAgents.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageAgents.Size = new System.Drawing.Size(496, 161);
            tabPageAgents.TabIndex = 1;
            tabPageAgents.Text = "Agents";
            tabPageAgents.UseVisualStyleBackColor = true;
            // 
            // tabPageGazetteer
            // 
            tabPageGazetteer.AutoScroll = true;
            tabPageGazetteer.Location = new System.Drawing.Point(4, 24);
            tabPageGazetteer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageGazetteer.Name = "tabPageGazetteer";
            tabPageGazetteer.Size = new System.Drawing.Size(496, 161);
            tabPageGazetteer.TabIndex = 3;
            tabPageGazetteer.Text = "Gazetteer";
            tabPageGazetteer.UseVisualStyleBackColor = true;
            // 
            // tabPagePlots
            // 
            tabPagePlots.Location = new System.Drawing.Point(4, 24);
            tabPagePlots.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPagePlots.Name = "tabPagePlots";
            tabPagePlots.Size = new System.Drawing.Size(496, 161);
            tabPagePlots.TabIndex = 5;
            tabPagePlots.Text = "Plots";
            tabPagePlots.UseVisualStyleBackColor = true;
            // 
            // tabPageReferences
            // 
            tabPageReferences.AutoScroll = true;
            tabPageReferences.Location = new System.Drawing.Point(4, 24);
            tabPageReferences.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageReferences.Name = "tabPageReferences";
            tabPageReferences.Size = new System.Drawing.Size(496, 161);
            tabPageReferences.TabIndex = 4;
            tabPageReferences.Text = "References";
            tabPageReferences.UseVisualStyleBackColor = true;
            // 
            // tabPageTaxa
            // 
            tabPageTaxa.AutoScroll = true;
            tabPageTaxa.Location = new System.Drawing.Point(4, 24);
            tabPageTaxa.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageTaxa.Name = "tabPageTaxa";
            tabPageTaxa.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageTaxa.Size = new System.Drawing.Size(496, 161);
            tabPageTaxa.TabIndex = 0;
            tabPageTaxa.Text = "Taxa";
            tabPageTaxa.UseVisualStyleBackColor = true;
            // 
            // tabPageTerms
            // 
            tabPageTerms.AutoScroll = true;
            tabPageTerms.Location = new System.Drawing.Point(4, 24);
            tabPageTerms.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageTerms.Name = "tabPageTerms";
            tabPageTerms.Size = new System.Drawing.Size(496, 161);
            tabPageTerms.TabIndex = 2;
            tabPageTerms.Text = "Terms";
            tabPageTerms.UseVisualStyleBackColor = true;
            // 
            // imageList
            // 
            imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList.ImageStream");
            imageList.TransparentColor = System.Drawing.Color.Transparent;
            imageList.Images.SetKeyName(0, "Add.ico");
            imageList.Images.SetKeyName(1, "OK.ico");
            imageList.Images.SetKeyName(2, "Conflict3.ico");
            imageList.Images.SetKeyName(3, "Update.ico");
            imageList.Images.SetKeyName(4, "Error.ico");
            imageList.Images.SetKeyName(5, "Project.ico");
            imageList.Images.SetKeyName(6, "Agent.ico");
            imageList.Images.SetKeyName(7, "Country.ico");
            imageList.Images.SetKeyName(8, "References.ico");
            // 
            // tabPageWebservices
            // 
            tabPageWebservices.Controls.Add(tabControlWebservices);
            tabPageWebservices.Location = new System.Drawing.Point(4, 24);
            tabPageWebservices.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageWebservices.Name = "tabPageWebservices";
            tabPageWebservices.Size = new System.Drawing.Size(512, 195);
            tabPageWebservices.TabIndex = 2;
            tabPageWebservices.Text = "Webservices";
            tabPageWebservices.UseVisualStyleBackColor = true;
            // 
            // tabControlWebservices
            // 
            tabControlWebservices.Controls.Add(tabPageTaxaWebservice);
            tabControlWebservices.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlWebservices.Location = new System.Drawing.Point(0, 0);
            tabControlWebservices.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabControlWebservices.Name = "tabControlWebservices";
            tabControlWebservices.SelectedIndex = 0;
            tabControlWebservices.Size = new System.Drawing.Size(512, 195);
            tabControlWebservices.TabIndex = 0;
            // 
            // tabPageTaxaWebservice
            // 
            tabPageTaxaWebservice.Location = new System.Drawing.Point(4, 24);
            tabPageTaxaWebservice.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageTaxaWebservice.Name = "tabPageTaxaWebservice";
            tabPageTaxaWebservice.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageTaxaWebservice.Size = new System.Drawing.Size(504, 167);
            tabPageTaxaWebservice.TabIndex = 0;
            tabPageTaxaWebservice.Text = "Taxa";
            tabPageTaxaWebservice.UseVisualStyleBackColor = true;
            // 
            // tabPageProjects
            // 
            tabPageProjects.AutoScroll = true;
            tabPageProjects.Controls.Add(tabControlProjects);
            tabPageProjects.Location = new System.Drawing.Point(4, 24);
            tabPageProjects.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageProjects.Name = "tabPageProjects";
            tabPageProjects.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageProjects.Size = new System.Drawing.Size(512, 195);
            tabPageProjects.TabIndex = 1;
            tabPageProjects.Text = "Projects";
            tabPageProjects.UseVisualStyleBackColor = true;
            // 
            // tabControlProjects
            // 
            tabControlProjects.Controls.Add(tabPage1);
            tabControlProjects.Controls.Add(tabPage2);
            tabControlProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlProjects.Location = new System.Drawing.Point(4, 3);
            tabControlProjects.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabControlProjects.Name = "tabControlProjects";
            tabControlProjects.SelectedIndex = 0;
            tabControlProjects.Size = new System.Drawing.Size(504, 189);
            tabControlProjects.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Location = new System.Drawing.Point(4, 24);
            tabPage1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage1.Size = new System.Drawing.Size(496, 161);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new System.Drawing.Point(4, 24);
            tabPage2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPage2.Size = new System.Drawing.Size(498, 155);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // buttonEnableIncongruentExchange
            // 
            buttonEnableIncongruentExchange.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonEnableIncongruentExchange.Location = new System.Drawing.Point(4, 295);
            buttonEnableIncongruentExchange.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            buttonEnableIncongruentExchange.Name = "buttonEnableIncongruentExchange";
            buttonEnableIncongruentExchange.Size = new System.Drawing.Size(88, 57);
            buttonEnableIncongruentExchange.TabIndex = 7;
            buttonEnableIncongruentExchange.Text = "Enable incongruent replacement";
            buttonEnableIncongruentExchange.UseVisualStyleBackColor = true;
            buttonEnableIncongruentExchange.Click += buttonEnableIncongruentExchange_Click;
            // 
            // FormExchangeDatabase
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(528, 380);
            Controls.Add(tableLayoutPanel);
            helpProvider.SetHelpKeyword(this, "cachedatabase_postgres_replacedb_dc");
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormExchangeDatabase";
            helpProvider.SetShowHelp(this, true);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Replace Database";
            KeyDown += Form_KeyDown;
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            tabControlCongruence.ResumeLayout(false);
            tabPageSources.ResumeLayout(false);
            tabControlSources.ResumeLayout(false);
            tabPageWebservices.ResumeLayout(false);
            tabControlWebservices.ResumeLayout(false);
            tabPageProjects.ResumeLayout(false);
            tabControlProjects.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonExchange;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Label labelExchangedDatabase;
        private System.Windows.Forms.ComboBox comboBoxExchangedDatabase;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.TabControl tabControlCongruence;
        private System.Windows.Forms.TabPage tabPageSources;
        private System.Windows.Forms.TabControl tabControlSources;
        private System.Windows.Forms.TabPage tabPageTaxa;
        private System.Windows.Forms.TabPage tabPageAgents;
        private System.Windows.Forms.TabPage tabPageTerms;
        private System.Windows.Forms.TabPage tabPageGazetteer;
        private System.Windows.Forms.TabPage tabPageProjects;
        private System.Windows.Forms.TabPage tabPageReferences;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Button buttonEnableIncongruentExchange;
        private System.Windows.Forms.TabPage tabPagePlots;
        private System.Windows.Forms.TabPage tabPageWebservices;
        private System.Windows.Forms.TabControl tabControlWebservices;
        private System.Windows.Forms.TabPage tabPageTaxaWebservice;
        private System.Windows.Forms.TabControl tabControlProjects;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}