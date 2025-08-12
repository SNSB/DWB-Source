namespace DiversityWorkbench.Forms
{
    partial class FormCompareData
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCompareData));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.tabControlTables = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.labelHeader = new System.Windows.Forms.Label();
            this.buttonRequery = new System.Windows.Forms.Button();
            this.checkBoxCompareKey = new System.Windows.Forms.CheckBox();
            this.checkBoxRestrictToDifferences = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanelHelp = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxHelpKey = new System.Windows.Forms.TextBox();
            this.textBoxHelpDifference = new System.Windows.Forms.TextBox();
            this.textBoxHelpNoDifference = new System.Windows.Forms.TextBox();
            this.textBoxHelpInfo = new System.Windows.Forms.TextBox();
            this.textBoxHelpKeyDifference = new System.Windows.Forms.TextBox();
            this.buttonSetColumnWith = new System.Windows.Forms.Button();
            this.tableLayoutPanelMain.SuspendLayout();
            this.tabControlTables.SuspendLayout();
            this.tableLayoutPanelHelp.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 5;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.tabControlTables, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonRequery, 4, 0);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxCompareKey, 2, 0);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxRestrictToDifferences, 3, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonSetColumnWith, 0, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(1137, 190);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // tabControlTables
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.tabControlTables, 5);
            this.tabControlTables.Controls.Add(this.tabPage1);
            this.tabControlTables.Controls.Add(this.tabPage2);
            this.tabControlTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlTables.Location = new System.Drawing.Point(3, 33);
            this.tabControlTables.Name = "tabControlTables";
            this.tabControlTables.SelectedIndex = 0;
            this.tabControlTables.Size = new System.Drawing.Size(1131, 154);
            this.tabControlTables.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1123, 128);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1123, 130);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(33, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(780, 30);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "Compare data";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonRequery
            // 
            this.buttonRequery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRequery.Image = global::DiversityWorkbench.Properties.Resources.Transfrom;
            this.buttonRequery.Location = new System.Drawing.Point(1110, 3);
            this.buttonRequery.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.buttonRequery.Name = "buttonRequery";
            this.buttonRequery.Size = new System.Drawing.Size(24, 26);
            this.buttonRequery.TabIndex = 3;
            this.toolTip.SetToolTip(this.buttonRequery, "Requery data");
            this.buttonRequery.UseVisualStyleBackColor = true;
            this.buttonRequery.Click += new System.EventHandler(this.buttonRequery_Click);
            // 
            // checkBoxCompareKey
            // 
            this.checkBoxCompareKey.AutoSize = true;
            this.checkBoxCompareKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxCompareKey.Location = new System.Drawing.Point(819, 3);
            this.checkBoxCompareKey.Name = "checkBoxCompareKey";
            this.checkBoxCompareKey.Size = new System.Drawing.Size(150, 24);
            this.checkBoxCompareKey.TabIndex = 4;
            this.checkBoxCompareKey.Text = "Compare text key columns";
            this.checkBoxCompareKey.UseVisualStyleBackColor = true;
            this.checkBoxCompareKey.CheckedChanged += new System.EventHandler(this.checkBoxCompareKey_CheckedChanged);
            // 
            // checkBoxRestrictToDifferences
            // 
            this.checkBoxRestrictToDifferences.AutoSize = true;
            this.checkBoxRestrictToDifferences.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxRestrictToDifferences.Location = new System.Drawing.Point(975, 3);
            this.checkBoxRestrictToDifferences.Name = "checkBoxRestrictToDifferences";
            this.checkBoxRestrictToDifferences.Size = new System.Drawing.Size(129, 24);
            this.checkBoxRestrictToDifferences.TabIndex = 5;
            this.checkBoxRestrictToDifferences.Text = "Restrict to differences";
            this.toolTip.SetToolTip(this.checkBoxRestrictToDifferences, "Restrict the displayed tables and columns to those with differences in the data");
            this.checkBoxRestrictToDifferences.UseVisualStyleBackColor = true;
            this.checkBoxRestrictToDifferences.CheckedChanged += new System.EventHandler(this.checkBoxRestrictToDifferences_CheckedChanged);
            // 
            // tableLayoutPanelHelp
            // 
            this.tableLayoutPanelHelp.ColumnCount = 5;
            this.tableLayoutPanelHelp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelHelp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelHelp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelHelp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelHelp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelHelp.Controls.Add(this.textBoxHelpKey, 0, 0);
            this.tableLayoutPanelHelp.Controls.Add(this.textBoxHelpDifference, 2, 0);
            this.tableLayoutPanelHelp.Controls.Add(this.textBoxHelpNoDifference, 3, 0);
            this.tableLayoutPanelHelp.Controls.Add(this.textBoxHelpInfo, 4, 0);
            this.tableLayoutPanelHelp.Controls.Add(this.textBoxHelpKeyDifference, 1, 0);
            this.tableLayoutPanelHelp.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanelHelp.Location = new System.Drawing.Point(0, 190);
            this.tableLayoutPanelHelp.Name = "tableLayoutPanelHelp";
            this.tableLayoutPanelHelp.RowCount = 1;
            this.tableLayoutPanelHelp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelHelp.Size = new System.Drawing.Size(1137, 26);
            this.tableLayoutPanelHelp.TabIndex = 1;
            // 
            // textBoxHelpKey
            // 
            this.textBoxHelpKey.BackColor = System.Drawing.Color.Yellow;
            this.textBoxHelpKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxHelpKey.Location = new System.Drawing.Point(3, 3);
            this.textBoxHelpKey.Name = "textBoxHelpKey";
            this.textBoxHelpKey.ReadOnly = true;
            this.textBoxHelpKey.Size = new System.Drawing.Size(221, 20);
            this.textBoxHelpKey.TabIndex = 0;
            this.textBoxHelpKey.Text = "Key columns of the table";
            this.textBoxHelpKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxHelpDifference
            // 
            this.textBoxHelpDifference.BackColor = System.Drawing.Color.Pink;
            this.textBoxHelpDifference.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxHelpDifference.Location = new System.Drawing.Point(457, 3);
            this.textBoxHelpDifference.Name = "textBoxHelpDifference";
            this.textBoxHelpDifference.ReadOnly = true;
            this.textBoxHelpDifference.Size = new System.Drawing.Size(221, 20);
            this.textBoxHelpDifference.TabIndex = 1;
            this.textBoxHelpDifference.Text = "Differences between the data";
            this.textBoxHelpDifference.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxHelpNoDifference
            // 
            this.textBoxHelpNoDifference.BackColor = System.Drawing.Color.White;
            this.textBoxHelpNoDifference.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxHelpNoDifference.Location = new System.Drawing.Point(684, 3);
            this.textBoxHelpNoDifference.Name = "textBoxHelpNoDifference";
            this.textBoxHelpNoDifference.ReadOnly = true;
            this.textBoxHelpNoDifference.Size = new System.Drawing.Size(221, 20);
            this.textBoxHelpNoDifference.TabIndex = 2;
            this.textBoxHelpNoDifference.Text = "No differences found";
            this.textBoxHelpNoDifference.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxHelpInfo
            // 
            this.textBoxHelpInfo.BackColor = System.Drawing.Color.LightGray;
            this.textBoxHelpInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxHelpInfo.Location = new System.Drawing.Point(911, 3);
            this.textBoxHelpInfo.Name = "textBoxHelpInfo";
            this.textBoxHelpInfo.ReadOnly = true;
            this.textBoxHelpInfo.Size = new System.Drawing.Size(223, 20);
            this.textBoxHelpInfo.TabIndex = 3;
            this.textBoxHelpInfo.Text = "Logging information";
            this.textBoxHelpInfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxHelpKeyDifference
            // 
            this.textBoxHelpKeyDifference.BackColor = System.Drawing.Color.PeachPuff;
            this.textBoxHelpKeyDifference.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxHelpKeyDifference.Location = new System.Drawing.Point(230, 3);
            this.textBoxHelpKeyDifference.Name = "textBoxHelpKeyDifference";
            this.textBoxHelpKeyDifference.ReadOnly = true;
            this.textBoxHelpKeyDifference.Size = new System.Drawing.Size(221, 20);
            this.textBoxHelpKeyDifference.TabIndex = 4;
            this.textBoxHelpKeyDifference.Text = "Text key with difference";
            this.textBoxHelpKeyDifference.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonSetColumnWith
            // 
            this.buttonSetColumnWith.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSetColumnWith.Image = global::DiversityWorkbench.Properties.Resources.OptColumnWidth;
            this.buttonSetColumnWith.Location = new System.Drawing.Point(3, 3);
            this.buttonSetColumnWith.Name = "buttonSetColumnWith";
            this.buttonSetColumnWith.Size = new System.Drawing.Size(24, 24);
            this.buttonSetColumnWith.TabIndex = 6;
            this.toolTip.SetToolTip(this.buttonSetColumnWith, "Set with of columns according to content");
            this.buttonSetColumnWith.UseVisualStyleBackColor = true;
            this.buttonSetColumnWith.Click += new System.EventHandler(this.buttonSetColumnWith_Click);
            // 
            // FormCompareData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1137, 216);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Controls.Add(this.tableLayoutPanelHelp);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCompareData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Compare data";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.tabControlTables.ResumeLayout(false);
            this.tableLayoutPanelHelp.ResumeLayout(false);
            this.tableLayoutPanelHelp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TabControl tabControlTables;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Button buttonRequery;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelHelp;
        private System.Windows.Forms.TextBox textBoxHelpKey;
        private System.Windows.Forms.TextBox textBoxHelpDifference;
        private System.Windows.Forms.TextBox textBoxHelpNoDifference;
        private System.Windows.Forms.TextBox textBoxHelpInfo;
        private System.Windows.Forms.CheckBox checkBoxCompareKey;
        private System.Windows.Forms.TextBox textBoxHelpKeyDifference;
        private System.Windows.Forms.CheckBox checkBoxRestrictToDifferences;
        private System.Windows.Forms.Button buttonSetColumnWith;
    }
}