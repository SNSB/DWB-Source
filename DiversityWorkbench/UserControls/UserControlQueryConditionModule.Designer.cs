namespace DiversityWorkbench.UserControls
{
    partial class UserControlQueryConditionModule
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlQueryConditionModule));
            buttonQueryConditionOperator = new System.Windows.Forms.Button();
            comboBoxQueryConditionOperator = new System.Windows.Forms.ComboBox();
            buttonCondition = new System.Windows.Forms.Button();
            toolTipQueryCondition = new System.Windows.Forms.ToolTip(components);
            buttonModuleConnection = new System.Windows.Forms.Button();
            buttonViewItems = new System.Windows.Forms.Button();
            labelItemCount = new System.Windows.Forms.Label();
            labelTextOrUri = new System.Windows.Forms.Label();
            pictureBoxTextOrUri = new System.Windows.Forms.PictureBox();
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            pictureBoxQueryOperator = new System.Windows.Forms.PictureBox();
            buttonBaseURL = new System.Windows.Forms.Button();
            labelModule = new System.Windows.Forms.Label();
            helpProvider = new System.Windows.Forms.HelpProvider();
            imageListOperator = new System.Windows.Forms.ImageList(components);
            ((System.ComponentModel.ISupportInitialize)pictureBoxTextOrUri).BeginInit();
            tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxQueryOperator).BeginInit();
            SuspendLayout();
            // 
            // buttonQueryConditionOperator
            // 
            buttonQueryConditionOperator.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonQueryConditionOperator.FlatAppearance.BorderSize = 0;
            buttonQueryConditionOperator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonQueryConditionOperator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            buttonQueryConditionOperator.Location = new System.Drawing.Point(102, 0);
            buttonQueryConditionOperator.Margin = new System.Windows.Forms.Padding(0);
            buttonQueryConditionOperator.Name = "buttonQueryConditionOperator";
            buttonQueryConditionOperator.Size = new System.Drawing.Size(16, 22);
            buttonQueryConditionOperator.TabIndex = 14;
            buttonQueryConditionOperator.TabStop = false;
            buttonQueryConditionOperator.Text = "∈";
            buttonQueryConditionOperator.UseVisualStyleBackColor = true;
            // 
            // comboBoxQueryConditionOperator
            // 
            comboBoxQueryConditionOperator.BackColor = System.Drawing.SystemColors.Control;
            comboBoxQueryConditionOperator.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxQueryConditionOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxQueryConditionOperator.DropDownWidth = 30;
            comboBoxQueryConditionOperator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            comboBoxQueryConditionOperator.FormattingEnabled = true;
            comboBoxQueryConditionOperator.Items.AddRange(new object[] { "~", "=", "≠", "Ø", "•", "—", "<", ">", "|" });
            comboBoxQueryConditionOperator.Location = new System.Drawing.Point(82, 0);
            comboBoxQueryConditionOperator.Margin = new System.Windows.Forms.Padding(0);
            comboBoxQueryConditionOperator.MaxDropDownItems = 20;
            comboBoxQueryConditionOperator.Name = "comboBoxQueryConditionOperator";
            comboBoxQueryConditionOperator.Size = new System.Drawing.Size(20, 23);
            comboBoxQueryConditionOperator.TabIndex = 13;
            comboBoxQueryConditionOperator.TabStop = false;
            comboBoxQueryConditionOperator.SelectedIndexChanged += comboBoxQueryConditionOperator_SelectedIndexChanged;
            comboBoxQueryConditionOperator.SelectionChangeCommitted += comboBoxQueryConditionOperator_SelectionChangeCommitted;
            // 
            // buttonCondition
            // 
            buttonCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonCondition.FlatAppearance.BorderSize = 0;
            buttonCondition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonCondition.Location = new System.Drawing.Point(0, 0);
            buttonCondition.Margin = new System.Windows.Forms.Padding(0);
            buttonCondition.Name = "buttonCondition";
            buttonCondition.Size = new System.Drawing.Size(82, 22);
            buttonCondition.TabIndex = 12;
            buttonCondition.TabStop = false;
            buttonCondition.Text = "Condition";
            buttonCondition.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            buttonCondition.UseVisualStyleBackColor = true;
            // 
            // buttonModuleConnection
            // 
            buttonModuleConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonModuleConnection.FlatAppearance.BorderSize = 0;
            buttonModuleConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonModuleConnection.Image = Properties.Resources.DiversityWorkbench16;
            buttonModuleConnection.Location = new System.Drawing.Point(365, 0);
            buttonModuleConnection.Margin = new System.Windows.Forms.Padding(0);
            buttonModuleConnection.Name = "buttonModuleConnection";
            tableLayoutPanel.SetRowSpan(buttonModuleConnection, 2);
            buttonModuleConnection.Size = new System.Drawing.Size(19, 37);
            buttonModuleConnection.TabIndex = 15;
            toolTipQueryCondition.SetToolTip(buttonModuleConnection, "Connect to database");
            buttonModuleConnection.UseVisualStyleBackColor = true;
            buttonModuleConnection.Click += buttonModuleConnection_Click;
            // 
            // buttonViewItems
            // 
            buttonViewItems.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonViewItems.FlatAppearance.BorderSize = 0;
            buttonViewItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonViewItems.Image = Properties.Resources.Lupe;
            buttonViewItems.Location = new System.Drawing.Point(134, 0);
            buttonViewItems.Margin = new System.Windows.Forms.Padding(0);
            buttonViewItems.Name = "buttonViewItems";
            buttonViewItems.Size = new System.Drawing.Size(21, 22);
            buttonViewItems.TabIndex = 16;
            toolTipQueryCondition.SetToolTip(buttonViewItems, "Show the selected items");
            buttonViewItems.UseVisualStyleBackColor = true;
            buttonViewItems.Click += buttonViewItems_Click;
            // 
            // labelItemCount
            // 
            labelItemCount.AutoSize = true;
            labelItemCount.Dock = System.Windows.Forms.DockStyle.Fill;
            labelItemCount.Location = new System.Drawing.Point(155, 0);
            labelItemCount.Margin = new System.Windows.Forms.Padding(0);
            labelItemCount.Name = "labelItemCount";
            labelItemCount.Size = new System.Drawing.Size(13, 22);
            labelItemCount.TabIndex = 18;
            labelItemCount.Text = "0";
            labelItemCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            toolTipQueryCondition.SetToolTip(labelItemCount, "The number of selected Items");
            // 
            // labelTextOrUri
            // 
            labelTextOrUri.AutoSize = true;
            tableLayoutPanel.SetColumnSpan(labelTextOrUri, 6);
            labelTextOrUri.Dock = System.Windows.Forms.DockStyle.Left;
            labelTextOrUri.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, 0);
            labelTextOrUri.ForeColor = System.Drawing.Color.Blue;
            labelTextOrUri.Location = new System.Drawing.Point(102, 22);
            labelTextOrUri.Margin = new System.Windows.Forms.Padding(0);
            labelTextOrUri.MaximumSize = new System.Drawing.Size(0, 15);
            labelTextOrUri.Name = "labelTextOrUri";
            labelTextOrUri.Size = new System.Drawing.Size(66, 15);
            labelTextOrUri.TabIndex = 22;
            labelTextOrUri.Text = "Filter for URI";
            toolTipQueryCondition.SetToolTip(labelTextOrUri, "Change filter mode");
            labelTextOrUri.Click += labelTextOrUri_Click;
            // 
            // pictureBoxTextOrUri
            // 
            pictureBoxTextOrUri.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxTextOrUri.Image = Properties.Resources.Filter;
            pictureBoxTextOrUri.Location = new System.Drawing.Point(82, 22);
            pictureBoxTextOrUri.Margin = new System.Windows.Forms.Padding(0);
            pictureBoxTextOrUri.Name = "pictureBoxTextOrUri";
            pictureBoxTextOrUri.Size = new System.Drawing.Size(20, 15);
            pictureBoxTextOrUri.TabIndex = 23;
            pictureBoxTextOrUri.TabStop = false;
            toolTipQueryCondition.SetToolTip(pictureBoxTextOrUri, "Change filter mode");
            pictureBoxTextOrUri.Click += pictureBoxTextOrUri_Click;
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 9;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.Controls.Add(labelTextOrUri, 2, 1);
            tableLayoutPanel.Controls.Add(buttonCondition, 0, 0);
            tableLayoutPanel.Controls.Add(pictureBoxTextOrUri, 1, 1);
            tableLayoutPanel.Controls.Add(buttonViewItems, 5, 0);
            tableLayoutPanel.Controls.Add(buttonModuleConnection, 8, 0);
            tableLayoutPanel.Controls.Add(labelItemCount, 6, 0);
            tableLayoutPanel.Controls.Add(buttonQueryConditionOperator, 2, 0);
            tableLayoutPanel.Controls.Add(comboBoxQueryConditionOperator, 1, 0);
            tableLayoutPanel.Controls.Add(pictureBoxQueryOperator, 4, 0);
            tableLayoutPanel.Controls.Add(buttonBaseURL, 7, 0);
            tableLayoutPanel.Controls.Add(labelModule, 0, 1);
            tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 2;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanel.Size = new System.Drawing.Size(384, 37);
            tableLayoutPanel.TabIndex = 19;
            // 
            // pictureBoxQueryOperator
            // 
            pictureBoxQueryOperator.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxQueryOperator.Image = Properties.Resources.Hierarchy;
            pictureBoxQueryOperator.Location = new System.Drawing.Point(118, 2);
            pictureBoxQueryOperator.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            pictureBoxQueryOperator.Name = "pictureBoxQueryOperator";
            pictureBoxQueryOperator.Size = new System.Drawing.Size(16, 20);
            pictureBoxQueryOperator.TabIndex = 19;
            pictureBoxQueryOperator.TabStop = false;
            pictureBoxQueryOperator.Visible = false;
            // 
            // buttonBaseURL
            // 
            buttonBaseURL.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonBaseURL.FlatAppearance.BorderSize = 0;
            buttonBaseURL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonBaseURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            buttonBaseURL.ForeColor = System.Drawing.Color.Blue;
            buttonBaseURL.Location = new System.Drawing.Point(168, 0);
            buttonBaseURL.Margin = new System.Windows.Forms.Padding(0);
            buttonBaseURL.Name = "buttonBaseURL";
            buttonBaseURL.Size = new System.Drawing.Size(197, 22);
            buttonBaseURL.TabIndex = 20;
            buttonBaseURL.UseVisualStyleBackColor = true;
            buttonBaseURL.Click += buttonBaseURL_Click;
            // 
            // labelModule
            // 
            labelModule.AutoSize = true;
            labelModule.Dock = System.Windows.Forms.DockStyle.Fill;
            labelModule.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            labelModule.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            labelModule.Location = new System.Drawing.Point(0, 22);
            labelModule.Margin = new System.Windows.Forms.Padding(0);
            labelModule.Name = "labelModule";
            labelModule.Size = new System.Drawing.Size(82, 15);
            labelModule.TabIndex = 24;
            labelModule.Text = "TaxonNames";
            labelModule.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imageListOperator
            // 
            imageListOperator.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListOperator.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListOperator.ImageStream");
            imageListOperator.TransparentColor = System.Drawing.Color.Transparent;
            imageListOperator.Images.SetKeyName(0, "Hierarchy.ico");
            imageListOperator.Images.SetKeyName(1, "Synonyms.ico");
            imageListOperator.Images.SetKeyName(2, "HierarchyAndSynonyms.ico");
            imageListOperator.Images.SetKeyName(3, "SynonymsOfAll.ico");
            // 
            // UserControlQueryConditionModule
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "UserControlQueryConditionModule";
            Size = new System.Drawing.Size(384, 37);
            ((System.ComponentModel.ISupportInitialize)pictureBoxTextOrUri).EndInit();
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxQueryOperator).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button buttonQueryConditionOperator;
        private System.Windows.Forms.ComboBox comboBoxQueryConditionOperator;
        private System.Windows.Forms.Button buttonCondition;
        private System.Windows.Forms.ToolTip toolTipQueryCondition;
        private System.Windows.Forms.Button buttonModuleConnection;
        private System.Windows.Forms.Button buttonViewItems;
        private System.Windows.Forms.Label labelItemCount;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.ImageList imageListOperator;
        private System.Windows.Forms.PictureBox pictureBoxQueryOperator;
        private System.Windows.Forms.Button buttonBaseURL;
        private System.Windows.Forms.Label labelTextOrUri;
        private System.Windows.Forms.PictureBox pictureBoxTextOrUri;
        private System.Windows.Forms.Label labelModule;
    }
}
