namespace DiversityWorkbench.Import
{
    partial class FormColumnInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormColumnInfo));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelColumnDescription = new System.Windows.Forms.Label();
            this.splitContainerGeography = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelGeography = new System.Windows.Forms.TableLayoutPanel();
            this.labelGeographyHeader = new System.Windows.Forms.Label();
            this.radioButtonGeographyNoFixes = new System.Windows.Forms.RadioButton();
            this.radioButtonGeographyPoint = new System.Windows.Forms.RadioButton();
            this.radioButtonGeographyLine = new System.Windows.Forms.RadioButton();
            this.radioButtonGeographyArea = new System.Windows.Forms.RadioButton();
            this.labelGeographyPrefix = new System.Windows.Forms.Label();
            this.labelGeographyData = new System.Windows.Forms.Label();
            this.labelGeographyPostfix = new System.Windows.Forms.Label();
            this.labelGeoraphyPrefixPOINT = new System.Windows.Forms.Label();
            this.labelGeoraphyPrefixLINE = new System.Windows.Forms.Label();
            this.labelGeoraphyPrefixAREA = new System.Windows.Forms.Label();
            this.labelGeoraphyPostfixPOINT = new System.Windows.Forms.Label();
            this.labelGeoraphyPostfixLINE = new System.Windows.Forms.Label();
            this.labelGeoraphyPostfixAREA = new System.Windows.Forms.Label();
            this.labelGeoraphyDataPOINT = new System.Windows.Forms.Label();
            this.labelGeoraphyDataLINE = new System.Windows.Forms.Label();
            this.labelGeoraphyDataAREA = new System.Windows.Forms.Label();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.tableLayoutPanelMain.SuspendLayout();
            this.splitContainerGeography.Panel1.SuspendLayout();
            this.splitContainerGeography.SuspendLayout();
            this.tableLayoutPanelGeography.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.labelColumnDescription, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.splitContainerGeography, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(531, 234);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // labelColumnDescription
            // 
            this.labelColumnDescription.AutoSize = true;
            this.labelColumnDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelColumnDescription.Location = new System.Drawing.Point(9, 9);
            this.labelColumnDescription.Margin = new System.Windows.Forms.Padding(9);
            this.labelColumnDescription.Name = "labelColumnDescription";
            this.labelColumnDescription.Size = new System.Drawing.Size(35, 13);
            this.labelColumnDescription.TabIndex = 0;
            this.labelColumnDescription.Text = "label1";
            // 
            // splitContainerGeography
            // 
            this.splitContainerGeography.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerGeography.Location = new System.Drawing.Point(3, 34);
            this.splitContainerGeography.Name = "splitContainerGeography";
            // 
            // splitContainerGeography.Panel1
            // 
            this.splitContainerGeography.Panel1.Controls.Add(this.tableLayoutPanelGeography);
            this.splitContainerGeography.Panel2Collapsed = true;
            this.splitContainerGeography.Size = new System.Drawing.Size(525, 197);
            this.splitContainerGeography.SplitterDistance = 214;
            this.splitContainerGeography.TabIndex = 1;
            // 
            // tableLayoutPanelGeography
            // 
            this.tableLayoutPanelGeography.ColumnCount = 4;
            this.tableLayoutPanelGeography.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelGeography.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeography.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeography.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeography.Controls.Add(this.labelGeographyHeader, 0, 0);
            this.tableLayoutPanelGeography.Controls.Add(this.radioButtonGeographyNoFixes, 0, 2);
            this.tableLayoutPanelGeography.Controls.Add(this.radioButtonGeographyPoint, 0, 3);
            this.tableLayoutPanelGeography.Controls.Add(this.radioButtonGeographyLine, 0, 4);
            this.tableLayoutPanelGeography.Controls.Add(this.radioButtonGeographyArea, 0, 5);
            this.tableLayoutPanelGeography.Controls.Add(this.labelGeographyPrefix, 1, 1);
            this.tableLayoutPanelGeography.Controls.Add(this.labelGeographyData, 2, 1);
            this.tableLayoutPanelGeography.Controls.Add(this.labelGeographyPostfix, 3, 1);
            this.tableLayoutPanelGeography.Controls.Add(this.labelGeoraphyPrefixPOINT, 1, 3);
            this.tableLayoutPanelGeography.Controls.Add(this.labelGeoraphyPrefixLINE, 1, 4);
            this.tableLayoutPanelGeography.Controls.Add(this.labelGeoraphyPrefixAREA, 1, 5);
            this.tableLayoutPanelGeography.Controls.Add(this.labelGeoraphyPostfixPOINT, 3, 3);
            this.tableLayoutPanelGeography.Controls.Add(this.labelGeoraphyPostfixLINE, 3, 4);
            this.tableLayoutPanelGeography.Controls.Add(this.labelGeoraphyPostfixAREA, 3, 5);
            this.tableLayoutPanelGeography.Controls.Add(this.labelGeoraphyDataPOINT, 2, 3);
            this.tableLayoutPanelGeography.Controls.Add(this.labelGeoraphyDataLINE, 2, 4);
            this.tableLayoutPanelGeography.Controls.Add(this.labelGeoraphyDataAREA, 2, 5);
            this.tableLayoutPanelGeography.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelGeography.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelGeography.Name = "tableLayoutPanelGeography";
            this.tableLayoutPanelGeography.RowCount = 7;
            this.tableLayoutPanelGeography.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeography.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeography.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeography.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeography.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeography.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeography.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelGeography.Size = new System.Drawing.Size(525, 197);
            this.tableLayoutPanelGeography.TabIndex = 0;
            // 
            // labelGeographyHeader
            // 
            this.labelGeographyHeader.AutoSize = true;
            this.tableLayoutPanelGeography.SetColumnSpan(this.labelGeographyHeader, 4);
            this.labelGeographyHeader.Location = new System.Drawing.Point(3, 6);
            this.labelGeographyHeader.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.labelGeographyHeader.Name = "labelGeographyHeader";
            this.labelGeographyHeader.Size = new System.Drawing.Size(492, 13);
            this.labelGeographyHeader.TabIndex = 0;
            this.labelGeographyHeader.Text = "To import geographical data, the values have to be formated according to the defi" +
    "nitions of SQL-Server";
            // 
            // radioButtonGeographyNoFixes
            // 
            this.radioButtonGeographyNoFixes.AutoSize = true;
            this.tableLayoutPanelGeography.SetColumnSpan(this.radioButtonGeographyNoFixes, 2);
            this.radioButtonGeographyNoFixes.Location = new System.Drawing.Point(3, 41);
            this.radioButtonGeographyNoFixes.Name = "radioButtonGeographyNoFixes";
            this.radioButtonGeographyNoFixes.Size = new System.Drawing.Size(152, 17);
            this.radioButtonGeographyNoFixes.TabIndex = 1;
            this.radioButtonGeographyNoFixes.TabStop = true;
            this.radioButtonGeographyNoFixes.Text = "Do not use pre- and postfix";
            this.radioButtonGeographyNoFixes.UseVisualStyleBackColor = true;
            // 
            // radioButtonGeographyPoint
            // 
            this.radioButtonGeographyPoint.AutoSize = true;
            this.radioButtonGeographyPoint.Location = new System.Drawing.Point(3, 64);
            this.radioButtonGeographyPoint.Name = "radioButtonGeographyPoint";
            this.radioButtonGeographyPoint.Size = new System.Drawing.Size(179, 17);
            this.radioButtonGeographyPoint.TabIndex = 2;
            this.radioButtonGeographyPoint.TabStop = true;
            this.radioButtonGeographyPoint.Text = "Use pre- and postfix for a POINT";
            this.radioButtonGeographyPoint.UseVisualStyleBackColor = true;
            // 
            // radioButtonGeographyLine
            // 
            this.radioButtonGeographyLine.AutoSize = true;
            this.radioButtonGeographyLine.Location = new System.Drawing.Point(3, 87);
            this.radioButtonGeographyLine.Name = "radioButtonGeographyLine";
            this.radioButtonGeographyLine.Size = new System.Drawing.Size(170, 17);
            this.radioButtonGeographyLine.TabIndex = 3;
            this.radioButtonGeographyLine.TabStop = true;
            this.radioButtonGeographyLine.Text = "Use pre- and postfix for a LINE";
            this.radioButtonGeographyLine.UseVisualStyleBackColor = true;
            // 
            // radioButtonGeographyArea
            // 
            this.radioButtonGeographyArea.AutoSize = true;
            this.radioButtonGeographyArea.Location = new System.Drawing.Point(3, 110);
            this.radioButtonGeographyArea.Name = "radioButtonGeographyArea";
            this.radioButtonGeographyArea.Size = new System.Drawing.Size(181, 17);
            this.radioButtonGeographyArea.TabIndex = 4;
            this.radioButtonGeographyArea.TabStop = true;
            this.radioButtonGeographyArea.Text = "Use pre- and postfix for an AREA";
            this.radioButtonGeographyArea.UseVisualStyleBackColor = true;
            // 
            // labelGeographyPrefix
            // 
            this.labelGeographyPrefix.AutoSize = true;
            this.labelGeographyPrefix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGeographyPrefix.Location = new System.Drawing.Point(193, 25);
            this.labelGeographyPrefix.Name = "labelGeographyPrefix";
            this.labelGeographyPrefix.Size = new System.Drawing.Size(211, 13);
            this.labelGeographyPrefix.TabIndex = 5;
            this.labelGeographyPrefix.Text = "Prefix";
            this.labelGeographyPrefix.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelGeographyData
            // 
            this.labelGeographyData.AutoSize = true;
            this.labelGeographyData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGeographyData.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelGeographyData.Location = new System.Drawing.Point(410, 25);
            this.labelGeographyData.Name = "labelGeographyData";
            this.labelGeographyData.Size = new System.Drawing.Size(61, 13);
            this.labelGeographyData.TabIndex = 6;
            this.labelGeographyData.Text = "Data";
            this.labelGeographyData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGeographyPostfix
            // 
            this.labelGeographyPostfix.AutoSize = true;
            this.labelGeographyPostfix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGeographyPostfix.Location = new System.Drawing.Point(477, 25);
            this.labelGeographyPostfix.Name = "labelGeographyPostfix";
            this.labelGeographyPostfix.Size = new System.Drawing.Size(45, 13);
            this.labelGeographyPostfix.TabIndex = 7;
            this.labelGeographyPostfix.Text = "Postfix";
            this.labelGeographyPostfix.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelGeoraphyPrefixPOINT
            // 
            this.labelGeoraphyPrefixPOINT.AutoSize = true;
            this.labelGeoraphyPrefixPOINT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGeoraphyPrefixPOINT.Location = new System.Drawing.Point(193, 61);
            this.labelGeoraphyPrefixPOINT.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelGeoraphyPrefixPOINT.Name = "labelGeoraphyPrefixPOINT";
            this.labelGeoraphyPrefixPOINT.Size = new System.Drawing.Size(214, 23);
            this.labelGeoraphyPrefixPOINT.TabIndex = 8;
            this.labelGeoraphyPrefixPOINT.Text = "geography::STPointFromText(\'POINT(";
            this.labelGeoraphyPrefixPOINT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelGeoraphyPrefixLINE
            // 
            this.labelGeoraphyPrefixLINE.AutoSize = true;
            this.labelGeoraphyPrefixLINE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGeoraphyPrefixLINE.Location = new System.Drawing.Point(193, 84);
            this.labelGeoraphyPrefixLINE.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelGeoraphyPrefixLINE.Name = "labelGeoraphyPrefixLINE";
            this.labelGeoraphyPrefixLINE.Size = new System.Drawing.Size(214, 23);
            this.labelGeoraphyPrefixLINE.TabIndex = 9;
            this.labelGeoraphyPrefixLINE.Text = "geography::STLineFromText(\'LINESTRING(";
            this.labelGeoraphyPrefixLINE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelGeoraphyPrefixAREA
            // 
            this.labelGeoraphyPrefixAREA.AutoSize = true;
            this.labelGeoraphyPrefixAREA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGeoraphyPrefixAREA.Location = new System.Drawing.Point(193, 107);
            this.labelGeoraphyPrefixAREA.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelGeoraphyPrefixAREA.Name = "labelGeoraphyPrefixAREA";
            this.labelGeoraphyPrefixAREA.Size = new System.Drawing.Size(214, 23);
            this.labelGeoraphyPrefixAREA.TabIndex = 10;
            this.labelGeoraphyPrefixAREA.Text = "geography::STGeomFromText(\'POLYGON((";
            this.labelGeoraphyPrefixAREA.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelGeoraphyPostfixPOINT
            // 
            this.labelGeoraphyPostfixPOINT.AutoSize = true;
            this.labelGeoraphyPostfixPOINT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGeoraphyPostfixPOINT.Location = new System.Drawing.Point(474, 61);
            this.labelGeoraphyPostfixPOINT.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelGeoraphyPostfixPOINT.Name = "labelGeoraphyPostfixPOINT";
            this.labelGeoraphyPostfixPOINT.Size = new System.Drawing.Size(48, 23);
            this.labelGeoraphyPostfixPOINT.TabIndex = 11;
            this.labelGeoraphyPostfixPOINT.Text = ")\', 4326)";
            this.labelGeoraphyPostfixPOINT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelGeoraphyPostfixLINE
            // 
            this.labelGeoraphyPostfixLINE.AutoSize = true;
            this.labelGeoraphyPostfixLINE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGeoraphyPostfixLINE.Location = new System.Drawing.Point(474, 84);
            this.labelGeoraphyPostfixLINE.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelGeoraphyPostfixLINE.Name = "labelGeoraphyPostfixLINE";
            this.labelGeoraphyPostfixLINE.Size = new System.Drawing.Size(48, 23);
            this.labelGeoraphyPostfixLINE.TabIndex = 12;
            this.labelGeoraphyPostfixLINE.Text = ")\', 4326)";
            this.labelGeoraphyPostfixLINE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelGeoraphyPostfixAREA
            // 
            this.labelGeoraphyPostfixAREA.AutoSize = true;
            this.labelGeoraphyPostfixAREA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGeoraphyPostfixAREA.Location = new System.Drawing.Point(474, 107);
            this.labelGeoraphyPostfixAREA.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelGeoraphyPostfixAREA.Name = "labelGeoraphyPostfixAREA";
            this.labelGeoraphyPostfixAREA.Size = new System.Drawing.Size(48, 23);
            this.labelGeoraphyPostfixAREA.TabIndex = 13;
            this.labelGeoraphyPostfixAREA.Text = "))\', 4326)";
            this.labelGeoraphyPostfixAREA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelGeoraphyDataPOINT
            // 
            this.labelGeoraphyDataPOINT.AutoSize = true;
            this.labelGeoraphyDataPOINT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGeoraphyDataPOINT.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelGeoraphyDataPOINT.Location = new System.Drawing.Point(407, 61);
            this.labelGeoraphyDataPOINT.Margin = new System.Windows.Forms.Padding(0);
            this.labelGeoraphyDataPOINT.Name = "labelGeoraphyDataPOINT";
            this.labelGeoraphyDataPOINT.Size = new System.Drawing.Size(67, 23);
            this.labelGeoraphyDataPOINT.TabIndex = 14;
            this.labelGeoraphyDataPOINT.Text = "42.3 12.5";
            this.labelGeoraphyDataPOINT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGeoraphyDataLINE
            // 
            this.labelGeoraphyDataLINE.AutoSize = true;
            this.labelGeoraphyDataLINE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGeoraphyDataLINE.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelGeoraphyDataLINE.Location = new System.Drawing.Point(407, 84);
            this.labelGeoraphyDataLINE.Margin = new System.Windows.Forms.Padding(0);
            this.labelGeoraphyDataLINE.Name = "labelGeoraphyDataLINE";
            this.labelGeoraphyDataLINE.Size = new System.Drawing.Size(67, 23);
            this.labelGeoraphyDataLINE.TabIndex = 15;
            this.labelGeoraphyDataLINE.Text = "42.3 12.5, ...";
            this.labelGeoraphyDataLINE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGeoraphyDataAREA
            // 
            this.labelGeoraphyDataAREA.AutoSize = true;
            this.labelGeoraphyDataAREA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGeoraphyDataAREA.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelGeoraphyDataAREA.Location = new System.Drawing.Point(407, 107);
            this.labelGeoraphyDataAREA.Margin = new System.Windows.Forms.Padding(0);
            this.labelGeoraphyDataAREA.Name = "labelGeoraphyDataAREA";
            this.labelGeoraphyDataAREA.Size = new System.Drawing.Size(67, 23);
            this.labelGeoraphyDataAREA.TabIndex = 16;
            this.labelGeoraphyDataAREA.Text = "42.3 12.5, ...";
            this.labelGeoraphyDataAREA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 234);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(531, 27);
            this.userControlDialogPanel.TabIndex = 1;
            // 
            // FormColumnInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 261);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormColumnInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormColumnInfo";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.splitContainerGeography.Panel1.ResumeLayout(false);
            this.splitContainerGeography.ResumeLayout(false);
            this.tableLayoutPanelGeography.ResumeLayout(false);
            this.tableLayoutPanelGeography.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelColumnDescription;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.SplitContainer splitContainerGeography;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelGeography;
        private System.Windows.Forms.Label labelGeographyHeader;
        private System.Windows.Forms.RadioButton radioButtonGeographyNoFixes;
        private System.Windows.Forms.RadioButton radioButtonGeographyPoint;
        private System.Windows.Forms.RadioButton radioButtonGeographyLine;
        private System.Windows.Forms.RadioButton radioButtonGeographyArea;
        private System.Windows.Forms.Label labelGeographyPrefix;
        private System.Windows.Forms.Label labelGeographyData;
        private System.Windows.Forms.Label labelGeographyPostfix;
        private System.Windows.Forms.Label labelGeoraphyPrefixPOINT;
        private System.Windows.Forms.Label labelGeoraphyPrefixLINE;
        private System.Windows.Forms.Label labelGeoraphyPrefixAREA;
        private System.Windows.Forms.Label labelGeoraphyPostfixPOINT;
        private System.Windows.Forms.Label labelGeoraphyPostfixLINE;
        private System.Windows.Forms.Label labelGeoraphyPostfixAREA;
        private System.Windows.Forms.Label labelGeoraphyDataPOINT;
        private System.Windows.Forms.Label labelGeoraphyDataLINE;
        private System.Windows.Forms.Label labelGeoraphyDataAREA;
    }
}