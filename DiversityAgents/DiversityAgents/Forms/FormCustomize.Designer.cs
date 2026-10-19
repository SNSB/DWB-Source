namespace DiversityAgents.Forms
{
    partial class FormCustomize
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCustomize));
            tabControlMain = new System.Windows.Forms.TabControl();
            tabPageDefaults = new System.Windows.Forms.TabPage();
            tableLayoutPanelDefaults = new System.Windows.Forms.TableLayoutPanel();
            groupBoxQuery = new System.Windows.Forms.GroupBox();
            tableLayoutPanelQuery = new System.Windows.Forms.TableLayoutPanel();
            pictureBoxQueryOptimized = new System.Windows.Forms.PictureBox();
            pictureBoxQueryRemember = new System.Windows.Forms.PictureBox();
            checkBoxQueryRemember = new System.Windows.Forms.CheckBox();
            checkBoxQueryOptimized = new System.Windows.Forms.CheckBox();
            groupBoxStart = new System.Windows.Forms.GroupBox();
            checkBoxLoadConnections = new System.Windows.Forms.CheckBox();
            toolTip = new System.Windows.Forms.ToolTip(components);
            helpProvider = new System.Windows.Forms.HelpProvider();
            tabControlMain.SuspendLayout();
            tabPageDefaults.SuspendLayout();
            tableLayoutPanelDefaults.SuspendLayout();
            groupBoxQuery.SuspendLayout();
            tableLayoutPanelQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxQueryOptimized).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxQueryRemember).BeginInit();
            groupBoxStart.SuspendLayout();
            SuspendLayout();
            // 
            // tabControlMain
            // 
            tabControlMain.Controls.Add(tabPageDefaults);
            tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlMain.Location = new System.Drawing.Point(0, 0);
            tabControlMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.SelectedIndex = 0;
            tabControlMain.Size = new System.Drawing.Size(266, 158);
            tabControlMain.TabIndex = 0;
            // 
            // tabPageDefaults
            // 
            tabPageDefaults.Controls.Add(tableLayoutPanelDefaults);
            tabPageDefaults.Location = new System.Drawing.Point(4, 24);
            tabPageDefaults.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageDefaults.Name = "tabPageDefaults";
            tabPageDefaults.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageDefaults.Size = new System.Drawing.Size(258, 130);
            tabPageDefaults.TabIndex = 0;
            tabPageDefaults.Text = "Defaults";
            tabPageDefaults.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelDefaults
            // 
            tableLayoutPanelDefaults.ColumnCount = 1;
            tableLayoutPanelDefaults.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelDefaults.Controls.Add(groupBoxQuery, 0, 0);
            tableLayoutPanelDefaults.Controls.Add(groupBoxStart, 0, 1);
            tableLayoutPanelDefaults.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelDefaults.Location = new System.Drawing.Point(4, 3);
            tableLayoutPanelDefaults.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelDefaults.Name = "tableLayoutPanelDefaults";
            tableLayoutPanelDefaults.RowCount = 3;
            tableLayoutPanelDefaults.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelDefaults.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelDefaults.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelDefaults.Size = new System.Drawing.Size(250, 124);
            tableLayoutPanelDefaults.TabIndex = 0;
            // 
            // groupBoxQuery
            // 
            groupBoxQuery.Controls.Add(tableLayoutPanelQuery);
            groupBoxQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxQuery.Location = new System.Drawing.Point(4, 3);
            groupBoxQuery.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxQuery.Name = "groupBoxQuery";
            groupBoxQuery.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            groupBoxQuery.Size = new System.Drawing.Size(242, 46);
            groupBoxQuery.TabIndex = 33;
            groupBoxQuery.TabStop = false;
            groupBoxQuery.Text = "Query";
            // 
            // tableLayoutPanelQuery
            // 
            tableLayoutPanelQuery.ColumnCount = 5;
            tableLayoutPanelQuery.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            tableLayoutPanelQuery.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelQuery.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            tableLayoutPanelQuery.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelQuery.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelQuery.Controls.Add(pictureBoxQueryOptimized, 0, 0);
            tableLayoutPanelQuery.Controls.Add(pictureBoxQueryRemember, 2, 0);
            tableLayoutPanelQuery.Controls.Add(checkBoxQueryRemember, 3, 0);
            tableLayoutPanelQuery.Controls.Add(checkBoxQueryOptimized, 1, 0);
            tableLayoutPanelQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelQuery.Location = new System.Drawing.Point(4, 16);
            tableLayoutPanelQuery.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanelQuery.Name = "tableLayoutPanelQuery";
            tableLayoutPanelQuery.RowCount = 1;
            tableLayoutPanelQuery.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelQuery.Size = new System.Drawing.Size(238, 30);
            tableLayoutPanelQuery.TabIndex = 4;
            // 
            // pictureBoxQueryOptimized
            // 
            pictureBoxQueryOptimized.BackColor = System.Drawing.Color.Yellow;
            pictureBoxQueryOptimized.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxQueryOptimized.Image = Properties.Resources.SpeedUp;
            pictureBoxQueryOptimized.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            pictureBoxQueryOptimized.Location = new System.Drawing.Point(2, 2);
            pictureBoxQueryOptimized.Margin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            pictureBoxQueryOptimized.Name = "pictureBoxQueryOptimized";
            pictureBoxQueryOptimized.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            pictureBoxQueryOptimized.Size = new System.Drawing.Size(19, 26);
            pictureBoxQueryOptimized.TabIndex = 2;
            pictureBoxQueryOptimized.TabStop = false;
            // 
            // pictureBoxQueryRemember
            // 
            pictureBoxQueryRemember.BackColor = System.Drawing.Color.Yellow;
            pictureBoxQueryRemember.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBoxQueryRemember.Image = Properties.Resources.Pin_3;
            pictureBoxQueryRemember.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            pictureBoxQueryRemember.Location = new System.Drawing.Point(110, 2);
            pictureBoxQueryRemember.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            pictureBoxQueryRemember.Name = "pictureBoxQueryRemember";
            pictureBoxQueryRemember.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            pictureBoxQueryRemember.Size = new System.Drawing.Size(16, 26);
            pictureBoxQueryRemember.TabIndex = 3;
            pictureBoxQueryRemember.TabStop = false;
            // 
            // checkBoxQueryRemember
            // 
            checkBoxQueryRemember.AutoSize = true;
            checkBoxQueryRemember.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxQueryRemember.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            checkBoxQueryRemember.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            checkBoxQueryRemember.Location = new System.Drawing.Point(128, 0);
            checkBoxQueryRemember.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            checkBoxQueryRemember.Name = "checkBoxQueryRemember";
            checkBoxQueryRemember.Size = new System.Drawing.Size(84, 30);
            checkBoxQueryRemember.TabIndex = 1;
            checkBoxQueryRemember.Text = "Remember";
            toolTip.SetToolTip(checkBoxQueryRemember, "Set remembering of query settings as default");
            checkBoxQueryRemember.UseVisualStyleBackColor = true;
            checkBoxQueryRemember.Click += checkBoxQueryRemember_Click;
            // 
            // checkBoxQueryOptimized
            // 
            checkBoxQueryOptimized.AutoSize = true;
            checkBoxQueryOptimized.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxQueryOptimized.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            checkBoxQueryOptimized.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            checkBoxQueryOptimized.Location = new System.Drawing.Point(23, 0);
            checkBoxQueryOptimized.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            checkBoxQueryOptimized.Name = "checkBoxQueryOptimized";
            checkBoxQueryOptimized.Size = new System.Drawing.Size(87, 30);
            checkBoxQueryOptimized.TabIndex = 0;
            checkBoxQueryOptimized.Text = "Optimized  ";
            toolTip.SetToolTip(checkBoxQueryOptimized, "Set optimizing the query as default");
            checkBoxQueryOptimized.UseVisualStyleBackColor = true;
            checkBoxQueryOptimized.Click += checkBoxQueryOptimized_Click;
            // 
            // groupBoxStart
            // 
            groupBoxStart.Controls.Add(checkBoxLoadConnections);
            groupBoxStart.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxStart.Location = new System.Drawing.Point(4, 55);
            groupBoxStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxStart.Name = "groupBoxStart";
            groupBoxStart.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxStart.Size = new System.Drawing.Size(242, 51);
            groupBoxStart.TabIndex = 34;
            groupBoxStart.TabStop = false;
            groupBoxStart.Text = "Start";
            // 
            // checkBoxLoadConnections
            // 
            checkBoxLoadConnections.BackColor = System.Drawing.Color.PeachPuff;
            checkBoxLoadConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxLoadConnections.Image = Properties.Resources.DataSource;
            checkBoxLoadConnections.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            checkBoxLoadConnections.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            checkBoxLoadConnections.Location = new System.Drawing.Point(4, 19);
            checkBoxLoadConnections.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            checkBoxLoadConnections.Name = "checkBoxLoadConnections";
            checkBoxLoadConnections.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            checkBoxLoadConnections.Size = new System.Drawing.Size(234, 29);
            checkBoxLoadConnections.TabIndex = 19;
            checkBoxLoadConnections.Text = "Load linked datasources at start";
            checkBoxLoadConnections.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            toolTip.SetToolTip(checkBoxLoadConnections, "Load datasources from linked server when starting the program");
            checkBoxLoadConnections.UseVisualStyleBackColor = false;
            checkBoxLoadConnections.Click += checkBoxLoadConnections_Click;
            // 
            // FormCustomize
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(266, 158);
            Controls.Add(tabControlMain);
            helpProvider.SetHelpKeyword(this, "customize_da");
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormCustomize";
            helpProvider.SetShowHelp(this, true);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Customize form";
            FormClosing += FormCustomize_FormClosing;
            KeyDown += Form_KeyDown;
            tabControlMain.ResumeLayout(false);
            tabPageDefaults.ResumeLayout(false);
            tableLayoutPanelDefaults.ResumeLayout(false);
            groupBoxQuery.ResumeLayout(false);
            tableLayoutPanelQuery.ResumeLayout(false);
            tableLayoutPanelQuery.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxQueryOptimized).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxQueryRemember).EndInit();
            groupBoxStart.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageDefaults;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDefaults;
        private System.Windows.Forms.GroupBox groupBoxQuery;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelQuery;
        private System.Windows.Forms.PictureBox pictureBoxQueryOptimized;
        private System.Windows.Forms.PictureBox pictureBoxQueryRemember;
        private System.Windows.Forms.CheckBox checkBoxQueryRemember;
        private System.Windows.Forms.CheckBox checkBoxQueryOptimized;
        private System.Windows.Forms.GroupBox groupBoxStart;
        private System.Windows.Forms.CheckBox checkBoxLoadConnections;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}