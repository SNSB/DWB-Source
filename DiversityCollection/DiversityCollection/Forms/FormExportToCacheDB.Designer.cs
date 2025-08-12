namespace DiversityCollection.Forms
{
    partial class FormExportToCacheDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExportToCacheDB));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.labelNoDB = new System.Windows.Forms.Label();
            this.tableLayoutPanelCacheDB = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxPart = new System.Windows.Forms.PictureBox();
            this.pictureBoxUnit = new System.Windows.Forms.PictureBox();
            this.pictureBoxSpecimen = new System.Windows.Forms.PictureBox();
            this.buttonViewParts = new System.Windows.Forms.Button();
            this.buttonViewUnits = new System.Windows.Forms.Button();
            this.buttonViewSpecimen = new System.Windows.Forms.Button();
            this.labelTaxa = new System.Windows.Forms.Label();
            this.labelHeader = new System.Windows.Forms.Label();
            this.textBoxTaxa = new System.Windows.Forms.TextBox();
            this.buttonTaxa = new System.Windows.Forms.Button();
            this.labelSpecimen = new System.Windows.Forms.Label();
            this.textBoxSpecimen = new System.Windows.Forms.TextBox();
            this.labelUnit = new System.Windows.Forms.Label();
            this.textBoxUnit = new System.Windows.Forms.TextBox();
            this.labelPart = new System.Windows.Forms.Label();
            this.textBoxPart = new System.Windows.Forms.TextBox();
            this.buttonUpdateSpecimen = new System.Windows.Forms.Button();
            this.buttonViewTaxa = new System.Windows.Forms.Button();
            this.pictureBoxTaxa = new System.Windows.Forms.PictureBox();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tableLayoutPanelCacheDB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpecimen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTaxa)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.labelNoDB);
            this.splitContainerMain.Panel1Collapsed = true;
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tableLayoutPanelCacheDB);
            this.splitContainerMain.Size = new System.Drawing.Size(782, 335);
            this.splitContainerMain.SplitterDistance = 114;
            this.splitContainerMain.TabIndex = 0;
            // 
            // labelNoDB
            // 
            this.labelNoDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNoDB.Location = new System.Drawing.Point(0, 0);
            this.labelNoDB.Name = "labelNoDB";
            this.labelNoDB.Size = new System.Drawing.Size(114, 100);
            this.labelNoDB.TabIndex = 0;
            this.labelNoDB.Text = "No cache database has been defined for the currrent database. Please turn to you " +
    "administrator to edit the function DiversityCollectionCacheDatabase to enable th" +
    "e export";
            this.labelNoDB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanelCacheDB
            // 
            this.tableLayoutPanelCacheDB.ColumnCount = 5;
            this.tableLayoutPanelCacheDB.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCacheDB.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanelCacheDB.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelCacheDB.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelCacheDB.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCacheDB.Controls.Add(this.pictureBoxPart, 3, 4);
            this.tableLayoutPanelCacheDB.Controls.Add(this.pictureBoxUnit, 3, 3);
            this.tableLayoutPanelCacheDB.Controls.Add(this.pictureBoxSpecimen, 3, 2);
            this.tableLayoutPanelCacheDB.Controls.Add(this.buttonViewParts, 2, 4);
            this.tableLayoutPanelCacheDB.Controls.Add(this.buttonViewUnits, 2, 3);
            this.tableLayoutPanelCacheDB.Controls.Add(this.buttonViewSpecimen, 2, 2);
            this.tableLayoutPanelCacheDB.Controls.Add(this.labelTaxa, 0, 1);
            this.tableLayoutPanelCacheDB.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelCacheDB.Controls.Add(this.textBoxTaxa, 1, 1);
            this.tableLayoutPanelCacheDB.Controls.Add(this.buttonTaxa, 4, 1);
            this.tableLayoutPanelCacheDB.Controls.Add(this.labelSpecimen, 0, 2);
            this.tableLayoutPanelCacheDB.Controls.Add(this.textBoxSpecimen, 1, 2);
            this.tableLayoutPanelCacheDB.Controls.Add(this.labelUnit, 0, 3);
            this.tableLayoutPanelCacheDB.Controls.Add(this.textBoxUnit, 1, 3);
            this.tableLayoutPanelCacheDB.Controls.Add(this.labelPart, 0, 4);
            this.tableLayoutPanelCacheDB.Controls.Add(this.textBoxPart, 1, 4);
            this.tableLayoutPanelCacheDB.Controls.Add(this.buttonUpdateSpecimen, 4, 2);
            this.tableLayoutPanelCacheDB.Controls.Add(this.buttonViewTaxa, 2, 1);
            this.tableLayoutPanelCacheDB.Controls.Add(this.pictureBoxTaxa, 3, 1);
            this.tableLayoutPanelCacheDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelCacheDB.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelCacheDB.Name = "tableLayoutPanelCacheDB";
            this.tableLayoutPanelCacheDB.RowCount = 6;
            this.tableLayoutPanelCacheDB.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCacheDB.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCacheDB.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCacheDB.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCacheDB.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCacheDB.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCacheDB.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelCacheDB.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelCacheDB.Size = new System.Drawing.Size(782, 335);
            this.tableLayoutPanelCacheDB.TabIndex = 0;
            // 
            // pictureBoxPart
            // 
            this.pictureBoxPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxPart.Image = global::DiversityCollection.Resource.wait_animation;
            this.pictureBoxPart.Location = new System.Drawing.Point(427, 202);
            this.pictureBoxPart.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.pictureBoxPart.Name = "pictureBoxPart";
            this.pictureBoxPart.Size = new System.Drawing.Size(20, 23);
            this.pictureBoxPart.TabIndex = 18;
            this.pictureBoxPart.TabStop = false;
            this.pictureBoxPart.Visible = false;
            // 
            // pictureBoxUnit
            // 
            this.pictureBoxUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxUnit.Image = global::DiversityCollection.Resource.wait_animation;
            this.pictureBoxUnit.Location = new System.Drawing.Point(427, 173);
            this.pictureBoxUnit.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.pictureBoxUnit.Name = "pictureBoxUnit";
            this.pictureBoxUnit.Size = new System.Drawing.Size(20, 23);
            this.pictureBoxUnit.TabIndex = 17;
            this.pictureBoxUnit.TabStop = false;
            this.pictureBoxUnit.Visible = false;
            // 
            // pictureBoxSpecimen
            // 
            this.pictureBoxSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxSpecimen.Image = global::DiversityCollection.Resource.wait_animation;
            this.pictureBoxSpecimen.Location = new System.Drawing.Point(427, 144);
            this.pictureBoxSpecimen.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.pictureBoxSpecimen.Name = "pictureBoxSpecimen";
            this.pictureBoxSpecimen.Size = new System.Drawing.Size(20, 23);
            this.pictureBoxSpecimen.TabIndex = 16;
            this.pictureBoxSpecimen.TabStop = false;
            this.pictureBoxSpecimen.Visible = false;
            // 
            // buttonViewParts
            // 
            this.buttonViewParts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonViewParts.Image = global::DiversityCollection.Resource.Lupe;
            this.buttonViewParts.Location = new System.Drawing.Point(398, 198);
            this.buttonViewParts.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
            this.buttonViewParts.Name = "buttonViewParts";
            this.buttonViewParts.Size = new System.Drawing.Size(26, 24);
            this.buttonViewParts.TabIndex = 14;
            this.buttonViewParts.UseVisualStyleBackColor = true;
            this.buttonViewParts.Click += new System.EventHandler(this.buttonViewParts_Click);
            // 
            // buttonViewUnits
            // 
            this.buttonViewUnits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonViewUnits.Image = global::DiversityCollection.Resource.Lupe;
            this.buttonViewUnits.Location = new System.Drawing.Point(398, 169);
            this.buttonViewUnits.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
            this.buttonViewUnits.Name = "buttonViewUnits";
            this.buttonViewUnits.Size = new System.Drawing.Size(26, 24);
            this.buttonViewUnits.TabIndex = 13;
            this.buttonViewUnits.UseVisualStyleBackColor = true;
            this.buttonViewUnits.Click += new System.EventHandler(this.buttonViewUnits_Click);
            // 
            // buttonViewSpecimen
            // 
            this.buttonViewSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonViewSpecimen.Image = global::DiversityCollection.Resource.Lupe;
            this.buttonViewSpecimen.Location = new System.Drawing.Point(398, 140);
            this.buttonViewSpecimen.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
            this.buttonViewSpecimen.Name = "buttonViewSpecimen";
            this.buttonViewSpecimen.Size = new System.Drawing.Size(26, 24);
            this.buttonViewSpecimen.TabIndex = 12;
            this.buttonViewSpecimen.UseVisualStyleBackColor = true;
            this.buttonViewSpecimen.Click += new System.EventHandler(this.buttonViewSpecimen_Click);
            // 
            // labelTaxa
            // 
            this.labelTaxa.AutoSize = true;
            this.labelTaxa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTaxa.Location = new System.Drawing.Point(3, 109);
            this.labelTaxa.Name = "labelTaxa";
            this.labelTaxa.Size = new System.Drawing.Size(329, 29);
            this.labelTaxa.TabIndex = 0;
            this.labelTaxa.Text = "Number of taxa:";
            this.labelTaxa.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanelCacheDB.SetColumnSpan(this.labelHeader, 5);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(3, 0, 3, 6);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(776, 103);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "Export to cache database";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // textBoxTaxa
            // 
            this.textBoxTaxa.Location = new System.Drawing.Point(338, 112);
            this.textBoxTaxa.Name = "textBoxTaxa";
            this.textBoxTaxa.ReadOnly = true;
            this.textBoxTaxa.Size = new System.Drawing.Size(54, 20);
            this.textBoxTaxa.TabIndex = 2;
            // 
            // buttonTaxa
            // 
            this.buttonTaxa.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonTaxa.Location = new System.Drawing.Point(450, 112);
            this.buttonTaxa.Name = "buttonTaxa";
            this.buttonTaxa.Size = new System.Drawing.Size(73, 23);
            this.buttonTaxa.TabIndex = 3;
            this.buttonTaxa.Text = "Update taxa";
            this.buttonTaxa.UseVisualStyleBackColor = true;
            this.buttonTaxa.Click += new System.EventHandler(this.buttonTaxa_Click);
            // 
            // labelSpecimen
            // 
            this.labelSpecimen.AutoSize = true;
            this.labelSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSpecimen.Location = new System.Drawing.Point(3, 138);
            this.labelSpecimen.Name = "labelSpecimen";
            this.labelSpecimen.Size = new System.Drawing.Size(329, 29);
            this.labelSpecimen.TabIndex = 4;
            this.labelSpecimen.Text = "Number of specimen:";
            this.labelSpecimen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxSpecimen
            // 
            this.textBoxSpecimen.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxSpecimen.Location = new System.Drawing.Point(338, 144);
            this.textBoxSpecimen.Name = "textBoxSpecimen";
            this.textBoxSpecimen.ReadOnly = true;
            this.textBoxSpecimen.Size = new System.Drawing.Size(54, 20);
            this.textBoxSpecimen.TabIndex = 5;
            // 
            // labelUnit
            // 
            this.labelUnit.AutoSize = true;
            this.labelUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUnit.Location = new System.Drawing.Point(3, 167);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(329, 29);
            this.labelUnit.TabIndex = 6;
            this.labelUnit.Text = "Number of organisms:";
            this.labelUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxUnit
            // 
            this.textBoxUnit.Location = new System.Drawing.Point(338, 170);
            this.textBoxUnit.Name = "textBoxUnit";
            this.textBoxUnit.ReadOnly = true;
            this.textBoxUnit.Size = new System.Drawing.Size(54, 20);
            this.textBoxUnit.TabIndex = 7;
            // 
            // labelPart
            // 
            this.labelPart.AutoSize = true;
            this.labelPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPart.Location = new System.Drawing.Point(3, 196);
            this.labelPart.Name = "labelPart";
            this.labelPart.Size = new System.Drawing.Size(329, 29);
            this.labelPart.TabIndex = 8;
            this.labelPart.Text = "Number of parts:";
            this.labelPart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPart
            // 
            this.textBoxPart.Location = new System.Drawing.Point(338, 199);
            this.textBoxPart.Name = "textBoxPart";
            this.textBoxPart.ReadOnly = true;
            this.textBoxPart.Size = new System.Drawing.Size(54, 20);
            this.textBoxPart.TabIndex = 9;
            // 
            // buttonUpdateSpecimen
            // 
            this.buttonUpdateSpecimen.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonUpdateSpecimen.Location = new System.Drawing.Point(450, 141);
            this.buttonUpdateSpecimen.Name = "buttonUpdateSpecimen";
            this.tableLayoutPanelCacheDB.SetRowSpan(this.buttonUpdateSpecimen, 3);
            this.buttonUpdateSpecimen.Size = new System.Drawing.Size(73, 81);
            this.buttonUpdateSpecimen.TabIndex = 10;
            this.buttonUpdateSpecimen.Text = "Export specimen, organisms and parts";
            this.buttonUpdateSpecimen.UseVisualStyleBackColor = true;
            this.buttonUpdateSpecimen.Click += new System.EventHandler(this.buttonUpdateSpecimen_Click);
            // 
            // buttonViewTaxa
            // 
            this.buttonViewTaxa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonViewTaxa.Image = global::DiversityCollection.Resource.Lupe;
            this.buttonViewTaxa.Location = new System.Drawing.Point(398, 111);
            this.buttonViewTaxa.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
            this.buttonViewTaxa.Name = "buttonViewTaxa";
            this.buttonViewTaxa.Size = new System.Drawing.Size(26, 24);
            this.buttonViewTaxa.TabIndex = 11;
            this.buttonViewTaxa.UseVisualStyleBackColor = true;
            this.buttonViewTaxa.Click += new System.EventHandler(this.buttonViewTaxa_Click);
            // 
            // pictureBoxTaxa
            // 
            this.pictureBoxTaxa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxTaxa.Image = global::DiversityCollection.Resource.wait_animation;
            this.pictureBoxTaxa.Location = new System.Drawing.Point(427, 115);
            this.pictureBoxTaxa.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.pictureBoxTaxa.Name = "pictureBoxTaxa";
            this.pictureBoxTaxa.Size = new System.Drawing.Size(20, 23);
            this.pictureBoxTaxa.TabIndex = 15;
            this.pictureBoxTaxa.TabStop = false;
            this.pictureBoxTaxa.Visible = false;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "OK.ico");
            this.imageList.Images.SetKeyName(1, "Error.ico");
            // 
            // FormExportToCacheDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 335);
            this.Controls.Add(this.splitContainerMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormExportToCacheDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export data to cache database";
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.tableLayoutPanelCacheDB.ResumeLayout(false);
            this.tableLayoutPanelCacheDB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpecimen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTaxa)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Label labelNoDB;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCacheDB;
        private System.Windows.Forms.Label labelTaxa;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.TextBox textBoxTaxa;
        private System.Windows.Forms.Button buttonTaxa;
        private System.Windows.Forms.Label labelSpecimen;
        private System.Windows.Forms.TextBox textBoxSpecimen;
        private System.Windows.Forms.Label labelUnit;
        private System.Windows.Forms.TextBox textBoxUnit;
        private System.Windows.Forms.Label labelPart;
        private System.Windows.Forms.TextBox textBoxPart;
        private System.Windows.Forms.Button buttonUpdateSpecimen;
        private System.Windows.Forms.Button buttonViewTaxa;
        private System.Windows.Forms.Button buttonViewParts;
        private System.Windows.Forms.Button buttonViewUnits;
        private System.Windows.Forms.Button buttonViewSpecimen;
        private System.Windows.Forms.PictureBox pictureBoxPart;
        private System.Windows.Forms.PictureBox pictureBoxUnit;
        private System.Windows.Forms.PictureBox pictureBoxSpecimen;
        private System.Windows.Forms.PictureBox pictureBoxTaxa;
        private System.Windows.Forms.ImageList imageList;
    }
}