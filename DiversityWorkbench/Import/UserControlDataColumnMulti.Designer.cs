namespace DiversityWorkbench.Import
{
    partial class UserControlDataColumnMulti
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlDataColumnMulti));
            this.buttonTranslation = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxCopyPrevious = new System.Windows.Forms.PictureBox();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.labelPrefix = new System.Windows.Forms.Label();
            this.textBoxPrefix = new System.Windows.Forms.TextBox();
            this.buttonColumnInSourceFile = new System.Windows.Forms.Button();
            this.pictureBoxDecision = new System.Windows.Forms.PictureBox();
            this.labelSequence = new System.Windows.Forms.Label();
            this.labelPostfix = new System.Windows.Forms.Label();
            this.textBoxPostfix = new System.Windows.Forms.TextBox();
            this.imageListDecision = new System.Windows.Forms.ImageList(this.components);
            this.imageListCopyLine = new System.Windows.Forms.ImageList(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCopyPrevious)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDecision)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonTranslation
            // 
            this.buttonTranslation.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.buttonTranslation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTranslation.Image = global::DiversityWorkbench.Properties.Resources.Conflict;
            this.buttonTranslation.Location = new System.Drawing.Point(254, 0);
            this.buttonTranslation.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTranslation.Name = "buttonTranslation";
            this.buttonTranslation.Size = new System.Drawing.Size(24, 24);
            this.buttonTranslation.TabIndex = 9;
            this.toolTip.SetToolTip(this.buttonTranslation, "Define a transformation for the data in the file");
            this.buttonTranslation.UseVisualStyleBackColor = false;
            this.buttonTranslation.Click += new System.EventHandler(this.buttonTranslation_Click);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 14;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.buttonTranslation, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxCopyPrevious, 5, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonRemove, 11, 0);
            this.tableLayoutPanel.Controls.Add(this.labelPrefix, 6, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxPrefix, 7, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonColumnInSourceFile, 10, 0);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxDecision, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelSequence, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.labelPostfix, 8, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxPostfix, 9, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(701, 24);
            this.tableLayoutPanel.TabIndex = 3;
            // 
            // pictureBoxCopyPrevious
            // 
            this.pictureBoxCopyPrevious.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxCopyPrevious.Image = global::DiversityWorkbench.Properties.Resources.CopyLine;
            this.pictureBoxCopyPrevious.Location = new System.Drawing.Point(278, 2);
            this.pictureBoxCopyPrevious.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.pictureBoxCopyPrevious.Name = "pictureBoxCopyPrevious";
            this.pictureBoxCopyPrevious.Size = new System.Drawing.Size(18, 22);
            this.pictureBoxCopyPrevious.TabIndex = 12;
            this.pictureBoxCopyPrevious.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxCopyPrevious, "If the values found in the file should be copied into following empty lines");
            this.pictureBoxCopyPrevious.WaitOnLoad = true;
            this.pictureBoxCopyPrevious.Click += new System.EventHandler(this.pictureBoxCopyPrevious_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRemove.Image = global::DiversityWorkbench.Properties.Resources.Minus;
            this.buttonRemove.Location = new System.Drawing.Point(433, 0);
            this.buttonRemove.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(24, 24);
            this.buttonRemove.TabIndex = 13;
            this.toolTip.SetToolTip(this.buttonRemove, "Remove this additional column");
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // labelPrefix
            // 
            this.labelPrefix.AutoSize = true;
            this.labelPrefix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPrefix.Location = new System.Drawing.Point(296, 0);
            this.labelPrefix.Margin = new System.Windows.Forms.Padding(0);
            this.labelPrefix.Name = "labelPrefix";
            this.labelPrefix.Size = new System.Drawing.Size(30, 24);
            this.labelPrefix.TabIndex = 14;
            this.labelPrefix.Text = "Pre.:";
            this.labelPrefix.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPrefix
            // 
            this.textBoxPrefix.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxPrefix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPrefix.Location = new System.Drawing.Point(326, 3);
            this.textBoxPrefix.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.textBoxPrefix.Name = "textBoxPrefix";
            this.textBoxPrefix.Size = new System.Drawing.Size(17, 20);
            this.textBoxPrefix.TabIndex = 15;
            this.textBoxPrefix.Tag = "";
            this.toolTip.SetToolTip(this.textBoxPrefix, "A text inserted before the text from the file");
            this.textBoxPrefix.TextChanged += new System.EventHandler(this.textBoxPrefix_TextChanged);
            this.textBoxPrefix.DoubleClick += new System.EventHandler(this.textBoxPrefix_DoubleClick);
            // 
            // buttonColumnInSourceFile
            // 
            this.buttonColumnInSourceFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonColumnInSourceFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonColumnInSourceFile.Location = new System.Drawing.Point(400, 0);
            this.buttonColumnInSourceFile.Margin = new System.Windows.Forms.Padding(0);
            this.buttonColumnInSourceFile.Name = "buttonColumnInSourceFile";
            this.buttonColumnInSourceFile.Size = new System.Drawing.Size(33, 24);
            this.buttonColumnInSourceFile.TabIndex = 17;
            this.toolTip.SetToolTip(this.buttonColumnInSourceFile, "The position of the data in the file");
            this.buttonColumnInSourceFile.UseVisualStyleBackColor = true;
            this.buttonColumnInSourceFile.Click += new System.EventHandler(this.buttonColumnInSourceFile_Click);
            this.buttonColumnInSourceFile.MouseEnter += new System.EventHandler(this.buttonColumnInSourceFile_MouseEnter);
            // 
            // pictureBoxDecision
            // 
            this.pictureBoxDecision.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBoxDecision.Image = global::DiversityWorkbench.Properties.Resources.Decision1;
            this.pictureBoxDecision.Location = new System.Drawing.Point(0, 3);
            this.pictureBoxDecision.Margin = new System.Windows.Forms.Padding(0, 3, 1, 0);
            this.pictureBoxDecision.Name = "pictureBoxDecision";
            this.pictureBoxDecision.Size = new System.Drawing.Size(13, 21);
            this.pictureBoxDecision.TabIndex = 20;
            this.pictureBoxDecision.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxDecision, "If this column is a decisive column");
            this.pictureBoxDecision.Click += new System.EventHandler(this.pictureBoxDecision_Click);
            // 
            // labelSequence
            // 
            this.labelSequence.AutoSize = true;
            this.labelSequence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSequence.Location = new System.Drawing.Point(33, 0);
            this.labelSequence.Name = "labelSequence";
            this.labelSequence.Size = new System.Drawing.Size(154, 24);
            this.labelSequence.TabIndex = 22;
            this.labelSequence.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPostfix
            // 
            this.labelPostfix.AutoSize = true;
            this.labelPostfix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPostfix.Location = new System.Drawing.Point(346, 0);
            this.labelPostfix.Name = "labelPostfix";
            this.labelPostfix.Size = new System.Drawing.Size(34, 24);
            this.labelPostfix.TabIndex = 23;
            this.labelPostfix.Text = "Post.:";
            this.labelPostfix.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPostfix
            // 
            this.textBoxPostfix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPostfix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPostfix.Location = new System.Drawing.Point(383, 3);
            this.textBoxPostfix.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.textBoxPostfix.Name = "textBoxPostfix";
            this.textBoxPostfix.Size = new System.Drawing.Size(17, 20);
            this.textBoxPostfix.TabIndex = 24;
            this.toolTip.SetToolTip(this.textBoxPostfix, "A text inserted after the text from the file");
            this.textBoxPostfix.TextChanged += new System.EventHandler(this.textBoxPostfix_TextChanged);
            this.textBoxPostfix.DoubleClick += new System.EventHandler(this.textBoxPostfix_DoubleClick);
            // 
            // imageListDecision
            // 
            this.imageListDecision.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDecision.ImageStream")));
            this.imageListDecision.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListDecision.Images.SetKeyName(0, "Decision.ico");
            this.imageListDecision.Images.SetKeyName(1, "DecisionGrey.ico");
            // 
            // imageListCopyLine
            // 
            this.imageListCopyLine.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListCopyLine.ImageStream")));
            this.imageListCopyLine.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListCopyLine.Images.SetKeyName(0, "CopyLine.ico");
            this.imageListCopyLine.Images.SetKeyName(1, "CopyLineGray.ico");
            // 
            // UserControlDataColumnMulti
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlDataColumnMulti";
            this.Size = new System.Drawing.Size(701, 25);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCopyPrevious)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDecision)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonTranslation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.PictureBox pictureBoxCopyPrevious;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Label labelPrefix;
        private System.Windows.Forms.TextBox textBoxPrefix;
        private System.Windows.Forms.Button buttonColumnInSourceFile;
        private System.Windows.Forms.PictureBox pictureBoxDecision;
        private System.Windows.Forms.Label labelSequence;
        private System.Windows.Forms.ImageList imageListDecision;
        private System.Windows.Forms.ImageList imageListCopyLine;
        private System.Windows.Forms.Label labelPostfix;
        private System.Windows.Forms.TextBox textBoxPostfix;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
