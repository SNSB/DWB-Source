using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.UserControls
{
    public partial class UserControlImportStep : UserControl
    {
        #region Parameter
        
        private int _Step;
        private DiversityCollection.Import_Step _ImportStep;
        //public enum ImportStatus { Done, Error, Warning };
        //private ImportStatus _ImportStatus;
        private bool _IsCurrent = false;
        
        #endregion
        
        #region Construction
        
        public UserControlImportStep(DiversityCollection.Import_Step ImportStep)
        {
            InitializeComponent();
            this._ImportStep = ImportStep;
            this.pictureBoxStep.Image = this._ImportStep.Image;
            this.labelStep.Text = this._ImportStep.Title;
            switch (this._ImportStep.Level)
            {
                case 0:
                    this.pictureBoxLevel.Width = 0;
                    break;
                case 1:
                    this.pictureBoxLevel.Width = 16;
                    break;
                case 2:
                    this.pictureBoxLevel.Width = 32;
                    break;
                default:
                    this.pictureBoxLevel.Width = 48;
                    break;
            }
            if (this._ImportStep.Image == null && this.pictureBoxLevel.Width > 0)
                this.pictureBoxLevel.Width = this.pictureBoxLevel.Width - 16;
            //if (this._ImportStep.IsGroupHaeder)
            //this.buttonMustImport.Visible = false;
            //string Table = this._ImportStep.TableName();
        }

        #endregion

        #region Interface
        
        public bool IsCurrent
        {
            get { return this._IsCurrent; }
            set
            {
                this.pictureBoxCurrent.Visible = value;
                if (value)
                {
                    this.labelStep.ForeColor = System.Drawing.Color.Black;
                    this.labelError.ForeColor = System.Drawing.Color.Black;
                    this._IsCurrent = true;
                }
                else
                {
                    this.labelStep.ForeColor = System.Drawing.Color.Gray;
                    this.labelError.ForeColor = System.Drawing.Color.Gray;
                    this._IsCurrent = false;
                }
                if (value)
                    this._ImportStep.ShowControls();//.setTabPageVisibility(value);
            }
        }

        public void setStatus()
        {
            try
            {
                switch (this._ImportStep.getImportStepStatus())
                {
                    case Import_Step.ImportStepStatus.Unhandled:
                        this.labelError.BackColor = System.Drawing.SystemColors.Control;
                        this.labelError.Visible = false;
                        this.pictureBoxStatus.Visible = false;
                        break;
                    case Import_Step.ImportStepStatus.Error:
                        this.labelError.Visible = true;
                        this.pictureBoxStatus.Visible = true;
                        if (DiversityCollection.Import.CurrentImportColumn != null
                            && DiversityCollection.Import.CurrentImportColumn.StepKey == this._ImportStep.StepKey()
                            && DiversityCollection.Import.CurrentImportColumn.IsSelected
                            && DiversityCollection.Import.CurrentImportColumn.ColumnInSourceFile == null
                            && DiversityCollection.Import.CurrentImportColumn.TypeOfSource == Import_Column.SourceType.File)
                            this.labelError.BackColor = System.Drawing.Color.Yellow;
                        else                        
                            this.labelError.BackColor = System.Drawing.Color.Pink;
                        this.labelError.Text = this._ImportStep.getStepError();
                        this.pictureBoxStatus.Image = this.imageListStatus.Images[2];
                        break;
                    case Import_Step.ImportStepStatus.OK:
                        this.labelError.Visible = false;
                        this.pictureBoxStatus.Visible = true;
                        this.labelError.BackColor = System.Drawing.SystemColors.Control;
                        this.pictureBoxStatus.Image = this.imageListStatus.Images[0];
                        break;
                }
                if (!this.buttonMustImport.Visible
                    && (this._ImportStep.TableName().Length > 0
                    || this._ImportStep.TableAlias().Length > 0))
                {
                    this.buttonMustImport.Visible = true;
                    if (this._ImportStep.MustImport)
                        this.buttonMustImport.Image = DiversityCollection.Resource.Import;
                    else this.buttonMustImport.Image = null;
                }
            }
            catch (System.Exception ex)
            { }
        }

        public void setStatus(System.Drawing.Color Color)
        {
            try
            {
                this.setStatus();
                if (Error.Length > 0)
                    this.labelError.BackColor = Color;
            }
            catch (System.Exception ex)
            { }
        }

        public string Error
        {
            get
            {
                return this._ImportStep.getStepError();
            }
        }

        public System.Drawing.Image StepImage
        {
            get
            {
                return this.pictureBoxStep.Image;
            }
        }

        /// <summary>
        /// if it can be changed if data are imported independent from data in the file. This is e.g. not the case for the project
        /// </summary>
        public bool MustImportCanBeChanged
        {
            set
            {
                this.buttonMustImport.Enabled = value;
            }
        }
        
        #endregion

        #region Moving

        private void tableLayoutPanel_Click(object sender, EventArgs e)
        {
            DiversityCollection.Import.MoveToStep(this._ImportStep.StepKey());
        }

        private void pictureBoxStep_Click(object sender, EventArgs e)
        {
            DiversityCollection.Import.MoveToStep(this._ImportStep.StepKey());
        }

        private void labelStep_Click(object sender, EventArgs e)
        {
            DiversityCollection.Import.MoveToStep(this._ImportStep.StepKey());
        }
 
        private void labelError_Click(object sender, EventArgs e)
        {
            DiversityCollection.Import.MoveToStep(this._ImportStep.StepKey());
        }

        private void buttonMustImport_Click(object sender, EventArgs e)
        {
            if (this._ImportStep.MustImport)
            {
                this._ImportStep.MustImport = false;
                this.buttonMustImport.Image = null;
            }
            else
            {
                this._ImportStep.MustImport = true;
                this.buttonMustImport.Image = DiversityCollection.Resource.Import;
            }
        }

	    #endregion    

    }
}
