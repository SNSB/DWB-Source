using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Export
{
    public partial class UserControlTable : UserControl
    {

        #region Parameter

        private Export.Table _Table;

        public Export.Table Table
        {
            get { return _Table; }
            //set { _Table = value; }
        }

        private Export.iExporter _iExporter;
        
        #endregion

        #region Construction

        public UserControlTable(DiversityWorkbench.Export.Table Table, DiversityWorkbench.Export.iExporter Exporter)
        {
            InitializeComponent();
            try
            {
                this._Table = Table;
                this._Table.UserControlTable = this;
                this._iExporter = Exporter;
                this.pictureBoxTable.Image = this._Table.Image;
                this.labelTable.Text = this._Table.DisplayText;
                this.panelSpacer.Width = 0;
                this.panelSpacer.Width = 20 * this._Table.Indent;
                this.SetControls();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void SetControls()
        {
            try
            {

                #region Orientation, Grouping, Join - im Moment nicht verwendet
                // Orientiation
                // vorerst abgeschaltet
                //if (this._Table.TypeOfParallelity == Export.Table.Parallelity.parallel)
                //{
                //    this.buttonColumnsOrLines.Visible = true;
                //    if (this._Table.ParallelOrientation == Table.OrientationOfParallel.InColumns)
                //    {
                //        this.buttonColumnsOrLines.Image = this.imageListOrientation.Images[0];
                //        this.toolTip.SetToolTip(this.buttonColumnsOrLines, "Export parallel data in columns");
                //    }
                //    else
                //    {
                //        this.buttonColumnsOrLines.Image = this.imageListOrientation.Images[1];
                //        this.toolTip.SetToolTip(this.buttonColumnsOrLines, "Export parallel data in lines");
                //    }
                //}
                //else
                //    this.buttonColumnsOrLines.Visible = false;
                //// vorerst abgeschaltet
                //this.buttonColumnsOrLines.Visible = false;


                // Grouping
                // vorerst abgeschaltet
                //if (this._Table.IsGrouped)
                //{
                //    this.buttonGroupBy.Image = this.imageListGroupBy.Images[0];
                //    this.toolTip.SetToolTip(this.buttonGroupBy, "Group data");
                //}
                //else
                //{
                //    this.buttonGroupBy.Image = this.imageListGroupBy.Images[1];
                //    this.toolTip.SetToolTip(this.buttonGroupBy, "Export every line in the data");
                //}
                //// vorerst abgeschaltet
                //this.buttonGroupBy.Visible = false;

                // Join
                // vorerst abgeschaltet
                //switch (this._Table.TypeOfJoin)
                //{
                //    case Export.Table.JoinType.Inner:
                //        this.buttonJoin.Image = this.imageListJoin.Images[0];
                //        this.toolTip.SetToolTip(this.buttonJoin, "Export only if data in this table do exist");
                //        break;
                //    case Export.Table.JoinType.Left:
                //        this.buttonJoin.Image = this.imageListJoin.Images[1];
                //        this.toolTip.SetToolTip(this.buttonJoin, "Export independed of the existence of data in the parent table");
                //        break;
                //    case Export.Table.JoinType.Outer:
                //        this.buttonJoin.Image = this.imageListJoin.Images[2];
                //        this.toolTip.SetToolTip(this.buttonJoin, "Export independed of the existence of data in this and the parent table");
                //        break;
                //    case Export.Table.JoinType.Right:
                //        this.buttonJoin.Image = this.imageListJoin.Images[3];
                //        this.toolTip.SetToolTip(this.buttonJoin, "Export independed of the existence of data in this table");
                //        break;
                //}
                //// vorerst abgeschaltet
                //this.buttonJoin.Visible = false;
                
                #endregion
                
                // Parallelity
                if (this._Table.TypeOfParallelity == Export.Table.Parallelity.parallel 
                    && this._Table.ParallelPosition == 1)
                {
                    this.buttonAddParallel.Visible = true;
                    if (this._Table.CanDetectParallelData() && this._Table.TypeOfParallelity == Export.Table.Parallelity.parallel)
                        this.buttonAddParallelMulti.Visible = true;
                    else this.buttonAddParallelMulti.Visible = false;
                }
                else if (this._Table.TypeOfParallelity == Export.Table.Parallelity.referencing)
                {
                    this.buttonAddParallelMulti.Visible = false;
                    if (this._Table.ReferencedParallelPosition == 1)
                        this.buttonAddParallel.Visible = true;
                    else this.buttonAddParallel.Visible = false;
                    //foreach (Export.Table T in DiversityWorkbench.Export.Exporter.SourceTableList())
                    //{
                    //    if (T.TableName == this._Table.TableName && T.ParentTable.TableAlias == this._Table.ParentTable.TableAlias)
                    //    {
                    //        if (T.ParallelPosition < this._Table.ParallelPosition)
                    //        {
                    //            this.buttonAddParallel.Visible = false;
                    //            break;
                    //        }
                    //    }
                    //}
                }
                else
                {
                    this.buttonAddParallel.Visible = false;
                    this.buttonAddParallelMulti.Visible = false;
                }

                // Dependent tables
                if (this._Table.HasInternalRelations && this._Table.TableAlias != Export.Exporter.StartTable.TableAlias)
                {
                    if (this._Table.ParentTable != null
                        && this._Table.ParentTable.TableName == this._Table.TableName
                        && this._Table.TypeOfParallelity == Export.Table.Parallelity.unique)
                        this.buttonAddDependent.Visible = false;
                    else
                        this.buttonAddDependent.Visible = true;
                }
                else this.buttonAddDependent.Visible = false;

                // Can be deleted
                if (this._Table.TypeOfParallelity == Export.Table.Parallelity.referencing)
                {
                    if (this._Table.ReferencedParallelPosition > 1)
                        this.buttonDelete.Visible = true;
                    else this.buttonDelete.Visible = false;
                    //int Position = 0;
                    //this.buttonDelete.Visible = false;
                    //foreach (Export.Table T in DiversityWorkbench.Export.Exporter.SourceTableList())
                    //{
                    //    if (T.TableName == this._Table.TableName && T.ParentTable.TableAlias == this._Table.ParentTable.TableAlias)
                    //    {
                    //        if (Position > 0)
                    //        {
                    //            this.buttonDelete.Visible = true;
                    //            break;
                    //        }
                    //        Position++;
                    //    }
                    //}
                }
                else if ((this._Table.ParallelPosition > 1 
                    && this._Table.TypeOfParallelity != Export.Table.Parallelity.restricted 
                    && this._Table.TypeOfParallelity != Export.Table.Parallelity.referencing)
                    || (this._Table.ParentTable != null && this._Table.ParentTable.TableName == this._Table.TableName && this._Table.TableAlias != this._Table.TableName))
                    this.buttonDelete.Visible = true;
                else this.buttonDelete.Visible = false;

                //this._iExporter.MarkCurrentSourceTable(this._Table);
                //this._iExporter.ShowCurrentSourceTableColumns(this._Table);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Button events
        
        private void buttonColumnsOrLines_Click(object sender, EventArgs e)
        {
            if (this._Table.ParallelOrientation == Table.OrientationOfParallel.InLines)
                this._Table.ParallelOrientation = Table.OrientationOfParallel.InColumns;
            else
                this._Table.ParallelOrientation = Table.OrientationOfParallel.InLines;
            this.SetControls();
        }
        
        private void buttonJoin_Click(object sender, EventArgs e)
        {
            switch (this._Table.TypeOfJoin)
            {
                case Export.Table.JoinType.Inner:
                    this._Table.TypeOfJoin = Table.JoinType.Left;
                    break;
                case Export.Table.JoinType.Left:
                    this._Table.TypeOfJoin = Table.JoinType.Outer;
                    break;
                case Export.Table.JoinType.Outer:
                    this._Table.TypeOfJoin = Table.JoinType.Right;
                    break;
                case Export.Table.JoinType.Right:
                    this._Table.TypeOfJoin = Table.JoinType.Inner;
                    break;

            }
            this.SetControls();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            this._iExporter.RemoveSourceTable(this._Table);
            Export.Exporter.ResetSourceTableList();
        }

        private void buttonGroupBy_Click(object sender, EventArgs e)
        {
            this._Table.IsGrouped = !this._Table.IsGrouped;
            this.SetControls();
            this._iExporter.ShowCurrentSourceTableColumns(this._Table);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            //DiversityWorkbench.Export.Table Child = new Table(
        }

        private void pictureBoxTable_Click(object sender, EventArgs e)
        {
            try
            {
                this._iExporter.MarkCurrentSourceTable(this._Table);
            }
            catch (System.Exception ex)
            {
            }
        }

        private void labelTable_Click(object sender, EventArgs e)
        {
            try
            {
                //this._iExporter.ShowCurrentSourceTableColumns(this._Table);
                this._iExporter.MarkCurrentSourceTable(this._Table);
            }
            catch (System.Exception ex)
            {
            }
        }

        public void MarkCurrent(Export.Table CurrentTable)
        {
            try
            {
                if (CurrentTable == this._Table)
                    this.BackColor = System.Drawing.Color.Yellow;
                else this.BackColor = System.Drawing.SystemColors.Control;
            }
            catch (System.Exception ex)
            {
            }
        }

        #region Adding tables
        
        private void buttonAddParallel_Click(object sender, EventArgs e)
        {
            this._iExporter.AddSourceTable(Exporter.TemplateTableDictionary[this.Table.TableName], this.Table.ParentTable);
        }

        private void buttonAddDependent_Click(object sender, EventArgs e)
        {
            try
            {
                this._iExporter.AddSourceTable(Exporter.TemplateTableDictionary[this.Table.TableName], this.Table);
            }
            catch (System.Exception ex) { }
        }

        private void buttonAddParallelMulti_Click(object sender, EventArgs e)
        {
            this._iExporter.AddMultiSourceTables(Exporter.TemplateTableDictionary[this.Table.TableName], this.Table.ParentTable);
        }
        
        #endregion

        #endregion

        #region Drag & Drop
        
        private void UserControlTable_DragDrop(object sender, DragEventArgs e)
        {
            //DiversityWorkbench.Export.Table T = (DiversityWorkbench.Export.Table)e.Data.GetData("DiversityWorkbench.Export.Table");
            //if (this._Table.ParentTable == T)
            //{
            //    this._Table.Exporter.SourceTables.Add(T.TableName, T);
            //}
            //else if (T.ParentTable == this._Table)
            //{
            //    this._Table.Exporter.SourceTables.Add(T.TableName, T);
            //}
        }

        private void UserControlTable_DragOver(object sender, DragEventArgs e)
        {

        }

        private void UserControlTable_DragEnter(object sender, DragEventArgs e)
        {
            //DiversityWorkbench.Export.Table T = (DiversityWorkbench.Export.Table)e.Data.GetData("DiversityWorkbench.Export.Table");
            //if (this._Table.ParentTable == T || T.ParentTable == this._Table)
            //    this.AllowDrop = true;
            //else
            //    this.AllowDrop = false;
        }
        
        private void labelTable_DragDrop(object sender, DragEventArgs e)
        {
            //DiversityWorkbench.Export.Table T = (DiversityWorkbench.Export.Table)e.Data.GetData("DiversityWorkbench.Export.Table");
            //this._iExporter.AddSourceTable(T, this._Table);


            ////if (this._Table.ParentTable == T)
            ////{
            ////    this._Table.Exporter.SourceTables.Add(T.TableName, T);
            ////}
            ////else if (T.ParentTable == this._Table)
            ////{
            ////    //this._Table.Exporter.SourceTables.Add(T.TableName, T);
            ////}
        }

        private void labelTable_DragEnter(object sender, DragEventArgs e)
        {
            //DiversityWorkbench.Export.Table T = (DiversityWorkbench.Export.Table)e.Data.GetData("DiversityWorkbench.Export.Table");
            //if (T.ParentTable.TableName == this._Table.TableName)
            //{
            //    e.Effect = DragDropEffects.Copy;
            //}
            //else
            //    e.Effect = DragDropEffects.None;
        }

        private void labelTable_DragOver(object sender, DragEventArgs e)
        {

        }

        #endregion

    }
}
