using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormDisplaySettings : Form
    {
        private static bool _AdaptResolution = false;
        public FormDisplaySettings()
        {
            InitializeComponent();
            this.initForm();
        }

        private void initForm()
        {
            try
            {
                if (_AdaptResolution)
                {
                    this.toolStripButtonAdaptResolution.ImageScaling = ToolStripItemImageScaling.None;
                    System.Drawing.Image I = DiversityWorkbench.Properties.Resources.Anzeige_16;
                    int Size = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(16);
#if DEBUG
                    Size = 32; // only for Test - Remove after Test
#endif
                    System.Drawing.Image Inew = DiversityWorkbench.Forms.FormFunctions.ResizeImage(I, Size, Size);
                    this.toolStripButtonAdaptResolution.Image = Inew;

                    //this.toolStripButtonAdaptResolution.Image = DiversityWorkbench.Properties.Resources.Anzeige;
                    this.toolStripButtonAdaptResolution.ForeColor = System.Drawing.Color.Blue;
                    this.toolStripButtonAdaptResolution.Text = "Adapt resolution: YES";
                }
                else
                {
                    this.toolStripButtonAdaptResolution.ImageScaling = ToolStripItemImageScaling.SizeToFit;
                    this.toolStripButtonAdaptResolution.ForeColor = System.Drawing.Color.Black;
                    this.toolStripButtonAdaptResolution.Image = DiversityWorkbench.Properties.Resources.Anzeige_16;
                    this.toolStripButtonAdaptResolution.Text = "Adapt resolution: NO";
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public bool AdaptResolution()
        {
            return _AdaptResolution;
        }

        private void toolStripButtonAdaptResolution_Click(object sender, EventArgs e)
        {
            _AdaptResolution = !_AdaptResolution;
            this.initForm();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
