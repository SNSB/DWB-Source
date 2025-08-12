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
    public partial class UserControlImportState : UserControl
    {
        #region Parameter
        
        public static readonly string Working = "Working";

        private DiversityCollection.Import.ImportStep _ImportStep;
        private DiversityCollection.Import.ImportStepEvent _ImportStepEvent;
        private DiversityCollection.Import.ImportStepSpecimen _ImportStepSpecimen;
        private int _Step;
        public enum ImportStatus { Done, Working, Error, Warning };
        private ImportStatus _ImportStatus;
        private bool _IsCurrent = false;
        
        #endregion
        
        #region Construction

        public UserControlImportState(System.Drawing.Image StateImage, string State, DiversityCollection.Import.ImportStep ImportState, string Description)
        {
            InitializeComponent();
            this.pictureBoxStep.Image = StateImage;
            this.labelStep.Text = State;
            this._ImportStep = ImportState;
            if (Description.Length > 0)
                this.textBoxMessage.Text = Description;
            else
            {
                this.textBoxMessage.Visible = false;
                this.Height = 18;
            }
        }

        public UserControlImportState(System.Drawing.Image StateImage, string State, DiversityCollection.Import.ImportStepEvent ImportState, string Description)
        {
            InitializeComponent();
            this.pictureBoxStep.Image = StateImage;
            this.labelStep.Text = State;
            this._ImportStepEvent = ImportState;
            if (Description.Length > 0)
                this.textBoxMessage.Text = Description;
            else
            {
                this.textBoxMessage.Visible = false;
                this.Height = 18;
            }
        }

        public UserControlImportState(System.Drawing.Image StateImage, string State, DiversityCollection.Import.ImportStepSpecimen ImportState, string Description)
        {
            InitializeComponent();
            this.pictureBoxStep.Image = StateImage;
            this.labelStep.Text = State;
            this._ImportStepSpecimen = ImportState;
            if (Description.Length > 0)
                this.textBoxMessage.Text = Description;
            else
            {
                this.textBoxMessage.Visible = false;
                this.Height = 18;
            }
        }

        public UserControlImportState(System.Drawing.Image StateImage, string State, int Step, string Description)
        {
            InitializeComponent();
            this.pictureBoxStep.Image = StateImage;
            this.labelStep.Text = State;
            this._Step = Step;
            if (Description.Length > 0)
                this.textBoxMessage.Text = Description;
            else
            {
                this.textBoxMessage.Visible = false;
                this.Height = 18;
            }
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
                    this.textBoxMessage.ForeColor = System.Drawing.Color.Black;
                    this._IsCurrent = true;
                }
                else
                {
                    this.labelStep.ForeColor = System.Drawing.Color.Gray;
                    this.textBoxMessage.ForeColor = System.Drawing.Color.Gray;
                    this._IsCurrent = false;
                }
            }
        }

        public void setStatus(string Message)
        {
            this.textBoxMessage.Visible = false;
            this.textBoxMessage.Text = Message;
            this.Height = 20;
            this.pictureBoxWait.Visible = false;
            this.pictureBoxStatus.Image = this.imageListStatus.Images[0];
            this.pictureBoxStatus.Visible = true;

            if (Message.Length > 0)
            {
                if (Message == DiversityCollection.UserControls.UserControlImportState.Working)
                {
                    this.pictureBoxWait.Visible = true;
                    this.pictureBoxStatus.Visible = false;
                    this._ImportStatus = ImportStatus.Working;
                }
                else
                {
                    this.textBoxMessage.BackColor = System.Drawing.Color.Pink;
                    this.pictureBoxStatus.Image = this.imageListStatus.Images[2];
                    this.textBoxMessage.Visible = true;
                    this.Height = 55;
                    this._ImportStatus = ImportStatus.Error;
                }
            }
        }

        public void setStatus(string Error, string Warning, bool isFinished)
        {
            if (Error.Length > 0)
            {
                this.pictureBoxStatus.Image = this.imageListStatus.Images[2];
                this.textBoxMessage.Text = Error;
                this.textBoxMessage.Visible = true;
                this.Height = 52;
                this.textBoxMessage.BackColor = System.Drawing.Color.Pink;
                this._ImportStatus = ImportStatus.Error;
            }
            else if (Warning.Length > 0)
            {
                this.pictureBoxStatus.Image = this.imageListStatus.Images[1];
                this.textBoxMessage.Text = Warning;
                this.textBoxMessage.Visible = true;
                this.Height = 52;
                this.textBoxMessage.BackColor = System.Drawing.Color.Yellow;
                this._ImportStatus = ImportStatus.Warning;
            }
            else if (isFinished)
            {
                this.pictureBoxWait.Visible = false;
                this.pictureBoxStatus.Image = this.imageListStatus.Images[0];
                this.textBoxMessage.Visible = false;
                this.Height = 20;
                this._ImportStatus = ImportStatus.Done;
            }
            else
            {
                this.pictureBoxWait.Visible = true;
                this.pictureBoxStatus.Visible = false;
                //this.pictureBoxStatus.Image = this.imageListStatus.Images[1];
                this.textBoxMessage.Visible = false;
                this.Height = 20;
                this._ImportStatus = ImportStatus.Done;
            }
        }

        public ImportStatus Status
        {
            get
            {
                return this._ImportStatus;
            }
        }

        public string Error
        {
            get
            {
                return this.textBoxMessage.Text;
            }
        }
        
        #endregion

        //private DiversityCollection.UserControls.UserControlImportState.ImportStatus Status
        //{
        //    set
        //    {
        //        switch (value)
        //        {
        //            case ImportStatus.Done:
        //                this.pictureBoxStatus.Image = this.imageListStatus.Images[0];
        //                break;
        //            case ImportStatus.Working:
        //                this.pictureBoxStatus.Image = this.imageListStatus.Images[1];
        //                break;
        //            case ImportStatus.Error:
        //                this.pictureBoxStatus.Image = this.imageListStatus.Images[2];
        //                this.textBoxMessage.BackColor = System.Drawing.Color.Pink;
        //                break;
        //            case ImportStatus.Warning:
        //                this.pictureBoxStatus.Image = this.imageListStatus.Images[4];
        //                this.textBoxMessage.BackColor = System.Drawing.Color.Yellow;
        //                break;
        //        }
        //    }
        //}

        //public void setStatus(DiversityCollection.UserControls.UserControlImportState.ImportStatus Status, string Message)
        //{
        //    this.textBoxMessage.Visible = false;
        //    this.textBoxMessage.Text = Message;
        //    this.Height = 20;
        //    switch (Status)
        //    {
        //        case ImportStatus.Done:
        //            this.pictureBoxStatus.Image = this.imageListStatus.Images[0];
        //            break;
        //        case ImportStatus.Working:
        //            this.pictureBoxStatus.Image = this.imageListStatus.Images[1];
        //            break;
        //        case ImportStatus.Error:
        //            if (Message == "Working")
        //            {
        //                this.pictureBoxStatus.Image = this.imageListStatus.Images[1];
        //            }
        //            else
        //            {
        //                this.pictureBoxStatus.Image = this.imageListStatus.Images[2];
        //                if (Message.Length > 0)
        //                {
        //                    this.textBoxMessage.Visible = true;
        //                    this.Height = 50;
        //                    this.textBoxMessage.BackColor = System.Drawing.Color.Pink;
        //                }
        //            }
        //            break;
        //        case ImportStatus.Warning:
        //            this.pictureBoxStatus.Image = this.imageListStatus.Images[4];
        //            if (Message.Length > 0)
        //            {
        //                this.textBoxMessage.Visible = true;
        //                this.Height = 50;
        //                this.textBoxMessage.BackColor = System.Drawing.Color.Yellow;
        //            }
        //            break;
        //    }
        //}

        //public void setStatus(string Message, bool IsFinished)
        //{
        //    this.textBoxMessage.Visible = false;
        //    this.textBoxMessage.Text = Message;
        //    this.Height = 20;
        //    if (IsFinished)
        //    {
        //        this.pictureBoxWait.Visible = false;
        //        this.pictureBoxStatus.Image = this.imageListStatus.Images[0];
        //        this.pictureBoxStatus.Visible = true;
        //    }
        //    else
        //    {
        //        if (Message.Length > 0)
        //        {
        //            this.pictureBoxWait.Visible = false;
        //            this.pictureBoxStatus.Image = this.imageListStatus.Images[2];
        //            this.pictureBoxStatus.Visible = true;
        //            this.textBoxMessage.Visible = true;
        //            this.Height = 52;
        //            this.textBoxMessage.BackColor = System.Drawing.Color.Pink;
        //        }
        //        else
        //        {
        //            this.pictureBoxStatus.Visible = false;
        //            this.pictureBoxWait.Visible = true;
        //        }
        //    }
        //}


    }
}
