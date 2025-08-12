namespace DiversityWorkbench.UserControls    
{
    partial class UserControlModuleRelatedEntry
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlModuleRelatedEntry));
            textBoxValue = new System.Windows.Forms.TextBox();
            comboBoxLocalValues = new System.Windows.Forms.ComboBox();
            toolTip = new System.Windows.Forms.ToolTip(components);
            buttonDeleteURI = new System.Windows.Forms.Button();
            buttonOpenModule = new System.Windows.Forms.Button();
            buttonConnectToDatabase = new System.Windows.Forms.Button();
            buttonFixSource = new System.Windows.Forms.Button();
            buttonHtml = new System.Windows.Forms.Button();
            panelTextBox = new System.Windows.Forms.Panel();
            labelInfo = new System.Windows.Forms.Label();
            labelURI = new System.Windows.Forms.Label();
            buttonChart = new System.Windows.Forms.Button();
            helpProvider = new System.Windows.Forms.HelpProvider();
            buttonSave = new System.Windows.Forms.Button();
            imageList = new System.Windows.Forms.ImageList(components);
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            panelTextBox.SuspendLayout();
            tableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // textBoxValue
            // 
            textBoxValue.BackColor = System.Drawing.SystemColors.Control;
            textBoxValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(textBoxValue, "textBoxValue");
            textBoxValue.Name = "textBoxValue";
            helpProvider.SetShowHelp(textBoxValue, (bool)resources.GetObject("textBoxValue.ShowHelp"));
            textBoxValue.EnabledChanged += textBoxValue_EnabledChanged;
            textBoxValue.DoubleClick += textBoxValue_DoubleClick;
            // 
            // comboBoxLocalValues
            // 
            resources.ApplyResources(comboBoxLocalValues, "comboBoxLocalValues");
            comboBoxLocalValues.DropDownWidth = 400;
            comboBoxLocalValues.FormattingEnabled = true;
            comboBoxLocalValues.Name = "comboBoxLocalValues";
            helpProvider.SetShowHelp(comboBoxLocalValues, (bool)resources.GetObject("comboBoxLocalValues.ShowHelp"));
            toolTip.SetToolTip(comboBoxLocalValues, resources.GetString("comboBoxLocalValues.ToolTip"));
            comboBoxLocalValues.SelectionChangeCommitted += comboBoxLocalValues_SelectionChangeCommitted;
            comboBoxLocalValues.KeyDown += comboBoxLocalValues_KeyDown;
            comboBoxLocalValues.MouseClick += comboBoxLocalValues_MouseClick;
            // 
            // buttonDeleteURI
            // 
            resources.ApplyResources(buttonDeleteURI, "buttonDeleteURI");
            buttonDeleteURI.FlatAppearance.BorderSize = 0;
            buttonDeleteURI.Image = ResourceWorkbench.Delete;
            buttonDeleteURI.Name = "buttonDeleteURI";
            helpProvider.SetShowHelp(buttonDeleteURI, (bool)resources.GetObject("buttonDeleteURI.ShowHelp"));
            toolTip.SetToolTip(buttonDeleteURI, resources.GetString("buttonDeleteURI.ToolTip"));
            buttonDeleteURI.UseVisualStyleBackColor = true;
            buttonDeleteURI.Click += buttonDeleteURI_Click;
            // 
            // buttonOpenModule
            // 
            resources.ApplyResources(buttonOpenModule, "buttonOpenModule");
            buttonOpenModule.FlatAppearance.BorderSize = 0;
            buttonOpenModule.Image = Properties.Resources.DiversityWorkbench16;
            buttonOpenModule.Name = "buttonOpenModule";
            helpProvider.SetShowHelp(buttonOpenModule, (bool)resources.GetObject("buttonOpenModule.ShowHelp"));
            toolTip.SetToolTip(buttonOpenModule, resources.GetString("buttonOpenModule.ToolTip"));
            buttonOpenModule.UseVisualStyleBackColor = true;
            buttonOpenModule.Click += buttonOpenModule_Click;
            // 
            // buttonConnectToDatabase
            // 
            resources.ApplyResources(buttonConnectToDatabase, "buttonConnectToDatabase");
            buttonConnectToDatabase.FlatAppearance.BorderSize = 0;
            buttonConnectToDatabase.Image = Properties.Resources.NoDatabase;
            buttonConnectToDatabase.Name = "buttonConnectToDatabase";
            helpProvider.SetShowHelp(buttonConnectToDatabase, (bool)resources.GetObject("buttonConnectToDatabase.ShowHelp"));
            toolTip.SetToolTip(buttonConnectToDatabase, resources.GetString("buttonConnectToDatabase.ToolTip"));
            buttonConnectToDatabase.UseVisualStyleBackColor = true;
            buttonConnectToDatabase.Click += buttonConnectToDatabase_Click;
            // 
            // buttonFixSource
            // 
            resources.ApplyResources(buttonFixSource, "buttonFixSource");
            buttonFixSource.FlatAppearance.BorderSize = 0;
            buttonFixSource.Name = "buttonFixSource";
            toolTip.SetToolTip(buttonFixSource, resources.GetString("buttonFixSource.ToolTip"));
            buttonFixSource.UseVisualStyleBackColor = true;
            buttonFixSource.Click += buttonFixSource_Click;
            // 
            // buttonHtml
            // 
            resources.ApplyResources(buttonHtml, "buttonHtml");
            buttonHtml.FlatAppearance.BorderSize = 0;
            buttonHtml.Image = Properties.Resources.Browse;
            buttonHtml.Name = "buttonHtml";
            helpProvider.SetShowHelp(buttonHtml, (bool)resources.GetObject("buttonHtml.ShowHelp"));
            toolTip.SetToolTip(buttonHtml, resources.GetString("buttonHtml.ToolTip"));
            buttonHtml.UseVisualStyleBackColor = true;
            buttonHtml.Click += buttonHtml_Click;
            // 
            // panelTextBox
            // 
            panelTextBox.Controls.Add(labelInfo);
            panelTextBox.Controls.Add(textBoxValue);
            resources.ApplyResources(panelTextBox, "panelTextBox");
            panelTextBox.Name = "panelTextBox";
            helpProvider.SetShowHelp(panelTextBox, (bool)resources.GetObject("panelTextBox.ShowHelp"));
            // 
            // labelInfo
            // 
            labelInfo.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(labelInfo, "labelInfo");
            labelInfo.Name = "labelInfo";
            // 
            // labelURI
            // 
            resources.ApplyResources(labelURI, "labelURI");
            labelURI.ForeColor = System.Drawing.SystemColors.HotTrack;
            labelURI.Name = "labelURI";
            helpProvider.SetShowHelp(labelURI, (bool)resources.GetObject("labelURI.ShowHelp"));
            labelURI.TextChanged += labelURI_TextChanged;
            labelURI.Click += labelURI_DoubleClick;
            labelURI.DoubleClick += labelURI_DoubleClick;
            // 
            // buttonChart
            // 
            resources.ApplyResources(buttonChart, "buttonChart");
            buttonChart.FlatAppearance.BorderSize = 0;
            buttonChart.Image = Properties.Resources.Chart_16;
            buttonChart.Name = "buttonChart";
            buttonChart.UseVisualStyleBackColor = true;
            buttonChart.Click += buttonChart_Click;
            // 
            // buttonSave
            // 
            resources.ApplyResources(buttonSave, "buttonSave");
            buttonSave.FlatAppearance.BorderSize = 0;
            buttonSave.Image = Properties.Resources.SaveSmall;
            buttonSave.Name = "buttonSave";
            helpProvider.SetShowHelp(buttonSave, (bool)resources.GetObject("buttonSave.ShowHelp"));
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // imageList
            // 
            imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList.ImageStream");
            imageList.TransparentColor = System.Drawing.Color.Transparent;
            imageList.Images.SetKeyName(0, "Pin_3.ico");
            imageList.Images.SetKeyName(1, "Pin_3Gray.ico");
            imageList.Images.SetKeyName(2, "Pin_3Orange.ico");
            imageList.Images.SetKeyName(3, "Pin_3Lila.ico");
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(tableLayoutPanel, "tableLayoutPanel");
            tableLayoutPanel.Controls.Add(buttonChart, 9, 0);
            tableLayoutPanel.Controls.Add(buttonHtml, 8, 0);
            tableLayoutPanel.Controls.Add(buttonConnectToDatabase, 7, 0);
            tableLayoutPanel.Controls.Add(buttonOpenModule, 6, 0);
            tableLayoutPanel.Controls.Add(buttonDeleteURI, 5, 0);
            tableLayoutPanel.Controls.Add(labelURI, 4, 0);
            tableLayoutPanel.Controls.Add(buttonSave, 2, 0);
            tableLayoutPanel.Controls.Add(panelTextBox, 3, 0);
            tableLayoutPanel.Controls.Add(buttonFixSource, 0, 0);
            tableLayoutPanel.Controls.Add(comboBoxLocalValues, 1, 0);
            tableLayoutPanel.Name = "tableLayoutPanel";
            // 
            // UserControlModuleRelatedEntry
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel);
            Name = "UserControlModuleRelatedEntry";
            helpProvider.SetShowHelp(this, (bool)resources.GetObject("$this.ShowHelp"));
            panelTextBox.ResumeLayout(false);
            panelTextBox.PerformLayout();
            tableLayoutPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        public System.Windows.Forms.Button buttonOpenModule;
        public System.Windows.Forms.TextBox textBoxValue;
        public System.Windows.Forms.ComboBox comboBoxLocalValues;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonConnectToDatabase;
        public System.Windows.Forms.Label labelURI;
        public System.Windows.Forms.HelpProvider helpProvider;
        public System.Windows.Forms.Button buttonDeleteURI;
        private System.Windows.Forms.ImageList imageList;
        public System.Windows.Forms.Button buttonFixSource;
        private System.Windows.Forms.Panel panelTextBox;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Button buttonChart;
        private System.Windows.Forms.Button buttonHtml;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonSave;
    }
}
