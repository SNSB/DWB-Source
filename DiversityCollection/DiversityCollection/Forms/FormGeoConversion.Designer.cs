namespace DiversityCollection.Forms
{
    partial class FormGeoConversion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGeoConversion));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelMain = new System.Windows.Forms.Label();
            this.labelCoordinatesOri = new System.Windows.Forms.Label();
            this.labelLatitude = new System.Windows.Forms.Label();
            this.labelLongitude = new System.Windows.Forms.Label();
            this.textBoxLatitudeOri = new System.Windows.Forms.TextBox();
            this.textBoxLongitudeOri = new System.Windows.Forms.TextBox();
            this.comboBoxCoordinatesResult = new System.Windows.Forms.ComboBox();
            this.textBoxLatitudeResult = new System.Windows.Forms.TextBox();
            this.textBoxLongitudeResult = new System.Windows.Forms.TextBox();
            this.labelTo = new System.Windows.Forms.Label();
            this.buttonConvert = new System.Windows.Forms.Button();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.labelMain, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelCoordinatesOri, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelLatitude, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelLongitude, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.textBoxLatitudeOri, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxLongitudeOri, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.comboBoxCoordinatesResult, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxLatitudeResult, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxLongitudeResult, 3, 2);
            this.tableLayoutPanel.Controls.Add(this.labelTo, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonConvert, 2, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(358, 81);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelMain
            // 
            this.labelMain.AutoSize = true;
            this.labelMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelMain.Location = new System.Drawing.Point(3, 10);
            this.labelMain.Margin = new System.Windows.Forms.Padding(3, 0, 3, 6);
            this.labelMain.Name = "labelMain";
            this.labelMain.Size = new System.Drawing.Size(57, 13);
            this.labelMain.TabIndex = 0;
            this.labelMain.Text = "Convert";
            this.labelMain.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCoordinatesOri
            // 
            this.labelCoordinatesOri.AutoSize = true;
            this.labelCoordinatesOri.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelCoordinatesOri.Location = new System.Drawing.Point(66, 10);
            this.labelCoordinatesOri.Margin = new System.Windows.Forms.Padding(3, 0, 3, 6);
            this.labelCoordinatesOri.Name = "labelCoordinatesOri";
            this.labelCoordinatesOri.Size = new System.Drawing.Size(100, 13);
            this.labelCoordinatesOri.TabIndex = 1;
            this.labelCoordinatesOri.Text = "Gauss Krüger";
            this.labelCoordinatesOri.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLatitude
            // 
            this.labelLatitude.AutoSize = true;
            this.labelLatitude.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLatitude.Location = new System.Drawing.Point(3, 29);
            this.labelLatitude.Name = "labelLatitude";
            this.labelLatitude.Size = new System.Drawing.Size(57, 26);
            this.labelLatitude.TabIndex = 3;
            this.labelLatitude.Text = "Latitude:";
            this.labelLatitude.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelLongitude
            // 
            this.labelLongitude.AutoSize = true;
            this.labelLongitude.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLongitude.Location = new System.Drawing.Point(3, 55);
            this.labelLongitude.Name = "labelLongitude";
            this.labelLongitude.Size = new System.Drawing.Size(57, 26);
            this.labelLongitude.TabIndex = 4;
            this.labelLongitude.Text = "Longitude:";
            this.labelLongitude.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxLatitudeOri
            // 
            this.textBoxLatitudeOri.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLatitudeOri.Location = new System.Drawing.Point(66, 32);
            this.textBoxLatitudeOri.Name = "textBoxLatitudeOri";
            this.textBoxLatitudeOri.ReadOnly = true;
            this.textBoxLatitudeOri.Size = new System.Drawing.Size(100, 20);
            this.textBoxLatitudeOri.TabIndex = 5;
            // 
            // textBoxLongitudeOri
            // 
            this.textBoxLongitudeOri.Location = new System.Drawing.Point(66, 58);
            this.textBoxLongitudeOri.Name = "textBoxLongitudeOri";
            this.textBoxLongitudeOri.ReadOnly = true;
            this.textBoxLongitudeOri.Size = new System.Drawing.Size(100, 20);
            this.textBoxLongitudeOri.TabIndex = 6;
            // 
            // comboBoxCoordinatesResult
            // 
            this.comboBoxCoordinatesResult.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.comboBoxCoordinatesResult.FormattingEnabled = true;
            this.comboBoxCoordinatesResult.Location = new System.Drawing.Point(203, 5);
            this.comboBoxCoordinatesResult.Name = "comboBoxCoordinatesResult";
            this.comboBoxCoordinatesResult.Size = new System.Drawing.Size(152, 21);
            this.comboBoxCoordinatesResult.TabIndex = 2;
            this.comboBoxCoordinatesResult.SelectedIndexChanged += new System.EventHandler(this.comboBoxCoordinatesResult_SelectedIndexChanged);
            // 
            // textBoxLatitudeResult
            // 
            this.textBoxLatitudeResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLatitudeResult.Location = new System.Drawing.Point(203, 32);
            this.textBoxLatitudeResult.Name = "textBoxLatitudeResult";
            this.textBoxLatitudeResult.ReadOnly = true;
            this.textBoxLatitudeResult.Size = new System.Drawing.Size(152, 20);
            this.textBoxLatitudeResult.TabIndex = 7;
            // 
            // textBoxLongitudeResult
            // 
            this.textBoxLongitudeResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLongitudeResult.Location = new System.Drawing.Point(203, 58);
            this.textBoxLongitudeResult.Name = "textBoxLongitudeResult";
            this.textBoxLongitudeResult.ReadOnly = true;
            this.textBoxLongitudeResult.Size = new System.Drawing.Size(152, 20);
            this.textBoxLongitudeResult.TabIndex = 8;
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelTo.Location = new System.Drawing.Point(172, 10);
            this.labelTo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 6);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(25, 13);
            this.labelTo.TabIndex = 9;
            this.labelTo.Text = "to";
            this.labelTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonConvert
            // 
            this.buttonConvert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonConvert.Image = global::DiversityCollection.Resource.ArrowNext1;
            this.buttonConvert.Location = new System.Drawing.Point(172, 32);
            this.buttonConvert.Name = "buttonConvert";
            this.tableLayoutPanel.SetRowSpan(this.buttonConvert, 2);
            this.buttonConvert.Size = new System.Drawing.Size(25, 46);
            this.buttonConvert.TabIndex = 10;
            this.buttonConvert.UseVisualStyleBackColor = true;
            this.buttonConvert.Click += new System.EventHandler(this.buttonConvert_Click);
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 81);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(358, 27);
            this.userControlDialogPanel.TabIndex = 1;
            // 
            // FormGeoConversion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 108);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormGeoConversion";
            this.Text = "Conversion of coordinates";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGeoConversion_FormClosing);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.Label labelMain;
        private System.Windows.Forms.Label labelCoordinatesOri;
        private System.Windows.Forms.ComboBox comboBoxCoordinatesResult;
        private System.Windows.Forms.Label labelLatitude;
        private System.Windows.Forms.Label labelLongitude;
        private System.Windows.Forms.TextBox textBoxLatitudeOri;
        private System.Windows.Forms.TextBox textBoxLongitudeOri;
        private System.Windows.Forms.TextBox textBoxLatitudeResult;
        private System.Windows.Forms.TextBox textBoxLongitudeResult;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.Button buttonConvert;
    }
}