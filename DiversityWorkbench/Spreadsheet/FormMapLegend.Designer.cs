namespace DiversityWorkbench.Spreadsheet
{
    partial class FormMapLegend
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMapLegend));
            panelSymbol = new System.Windows.Forms.Panel();
            contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            moveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            fixPositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            splitContainer = new System.Windows.Forms.SplitContainer();
            groupBoxSymbol = new System.Windows.Forms.GroupBox();
            groupBoxColor = new System.Windows.Forms.GroupBox();
            panelColor = new System.Windows.Forms.Panel();
            helpProvider = new System.Windows.Forms.HelpProvider();
            contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            groupBoxSymbol.SuspendLayout();
            groupBoxColor.SuspendLayout();
            SuspendLayout();
            // 
            // panelSymbol
            // 
            panelSymbol.ContextMenuStrip = contextMenuStrip;
            panelSymbol.Dock = System.Windows.Forms.DockStyle.Fill;
            panelSymbol.Location = new System.Drawing.Point(4, 19);
            panelSymbol.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panelSymbol.Name = "panelSymbol";
            panelSymbol.Size = new System.Drawing.Size(90, 407);
            panelSymbol.TabIndex = 0;
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { closeToolStripMenuItem, moveToolStripMenuItem, fixPositionToolStripMenuItem });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new System.Drawing.Size(136, 70);
            // 
            // closeToolStripMenuItem
            // 
            closeToolStripMenuItem.Image = Properties.Resources.Delete;
            closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            closeToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            closeToolStripMenuItem.Text = "Close";
            closeToolStripMenuItem.Click += closeToolStripMenuItem_Click;
            // 
            // moveToolStripMenuItem
            // 
            moveToolStripMenuItem.Image = Properties.Resources.Pin_3Gray;
            moveToolStripMenuItem.Name = "moveToolStripMenuItem";
            moveToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            moveToolStripMenuItem.Text = "Move";
            moveToolStripMenuItem.Click += moveToolStripMenuItem_Click;
            // 
            // fixPositionToolStripMenuItem
            // 
            fixPositionToolStripMenuItem.Image = Properties.Resources.Pin_3;
            fixPositionToolStripMenuItem.Name = "fixPositionToolStripMenuItem";
            fixPositionToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            fixPositionToolStripMenuItem.Text = "Fix position";
            fixPositionToolStripMenuItem.Click += fixPositionToolStripMenuItem_Click;
            // 
            // splitContainer
            // 
            splitContainer.ContextMenuStrip = contextMenuStrip;
            splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer.Location = new System.Drawing.Point(4, 3);
            splitContainer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer.Name = "splitContainer";
            splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(groupBoxSymbol);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(groupBoxColor);
            splitContainer.Size = new System.Drawing.Size(98, 647);
            splitContainer.SplitterDistance = 429;
            splitContainer.SplitterWidth = 5;
            splitContainer.TabIndex = 1;
            // 
            // groupBoxSymbol
            // 
            groupBoxSymbol.ContextMenuStrip = contextMenuStrip;
            groupBoxSymbol.Controls.Add(panelSymbol);
            groupBoxSymbol.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxSymbol.Location = new System.Drawing.Point(0, 0);
            groupBoxSymbol.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxSymbol.Name = "groupBoxSymbol";
            groupBoxSymbol.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxSymbol.Size = new System.Drawing.Size(98, 429);
            groupBoxSymbol.TabIndex = 0;
            groupBoxSymbol.TabStop = false;
            groupBoxSymbol.Text = "Symbol";
            // 
            // groupBoxColor
            // 
            groupBoxColor.ContextMenuStrip = contextMenuStrip;
            groupBoxColor.Controls.Add(panelColor);
            groupBoxColor.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxColor.Location = new System.Drawing.Point(0, 0);
            groupBoxColor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxColor.Name = "groupBoxColor";
            groupBoxColor.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxColor.Size = new System.Drawing.Size(98, 213);
            groupBoxColor.TabIndex = 0;
            groupBoxColor.TabStop = false;
            groupBoxColor.Text = "Color";
            // 
            // panelColor
            // 
            panelColor.ContextMenuStrip = contextMenuStrip;
            panelColor.Dock = System.Windows.Forms.DockStyle.Fill;
            panelColor.Location = new System.Drawing.Point(4, 19);
            panelColor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panelColor.Name = "panelColor";
            panelColor.Size = new System.Drawing.Size(90, 191);
            panelColor.TabIndex = 0;
            // 
            // FormMapLegend
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(106, 653);
            ContextMenuStrip = contextMenuStrip;
            Controls.Add(splitContainer);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormMapLegend";
            Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Legend for map";
            TopMost = true;
            KeyDown += Form_KeyDown;
            contextMenuStrip.ResumeLayout(false);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            groupBoxSymbol.ResumeLayout(false);
            groupBoxColor.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSymbol;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.GroupBox groupBoxSymbol;
        private System.Windows.Forms.GroupBox groupBoxColor;
        private System.Windows.Forms.Panel panelColor;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fixPositionToolStripMenuItem;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}