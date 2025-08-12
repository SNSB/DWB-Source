using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Import
{
    public partial class UserControlTableMessage : UserControl//, iDisposableControl
    {
        public UserControlTableMessage(DiversityWorkbench.Import.DataTable.TableMessageType MessageType, string Message)
        {
            InitializeComponent();
            this.labelMessage.Text = Message;
            switch (MessageType)
            {
                case DataTable.TableMessageType.OK:
                    this.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    this.pictureBox.Image = this.imageList.Images[0];
                    break;
                case DataTable.TableMessageType.ColumnNotSeleced:
                    this.pictureBox.Image = this.imageList.Images[1];
                    break;
                case DataTable.TableMessageType.DecisiveColumnNotSelected:
                    this.pictureBox.Image = this.imageList.Images[2];
                    break;
                case DataTable.TableMessageType.FileColumnNotSelected:
                    this.pictureBox.Image = this.imageList.Images[6];
                    break;
                case DataTable.TableMessageType.TypeOfSouceNotSelected:
                    this.pictureBox.Image = this.imageList.Images[3];
                    break;
                case DataTable.TableMessageType.ValueMissing:
                    this.pictureBox.Image = this.imageList.Images[5];
                    break;
                case DataTable.TableMessageType.ValueNotSelected:
                    this.pictureBox.Image = this.imageList.Images[4];
                    break;
                case DataTable.TableMessageType.CompareKeyNotSelected:
                    this.pictureBox.Image = this.imageList.Images[7];
                    break;
            }
        }

        //public void DisposeComponents()
        //{
        //    this.imageList.Dispose();
        //}
    }
}
