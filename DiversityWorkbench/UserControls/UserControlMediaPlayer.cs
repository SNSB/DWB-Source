using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WpfControls;

namespace DiversityWorkbench.UserControls
{
    public partial class UserControlMediaPlayer : UserControl
    {
        private enum MediumState
        {
            none,
            stopped,
            playing,
            paused
        }

        #region Parameter
        private string _File;
        private WpfControls.Media.UserControlMediaPlayer userControlPlayer;
        #endregion

        #region Construction

        public UserControlMediaPlayer()
        {
            InitializeComponent();

            // Child-Property cannot be set in form editor for SDK-like projekts, perform basic init here
            this.userControlPlayer = new WpfControls.Media.UserControlMediaPlayer();
            this.elementHostMedia.Child = userControlPlayer;

            // Set event handler
            this.userControlPlayer.MediaPlayer.MediaEnded += userControlPlayer_MediaEnded;
            this.labelMediumState.Text = "";
        }

        #endregion

        #region Properties

        public string File
        {
            get { return _File; }
            set
            {
                _File = value;
                this.textBoxFile.Text = value;
                this.buttonExternalBrowser.Visible = this.textBoxFile.Text != null;
            }
        }

        public string HelpNameSpace
        {
            set
            {
                this.helpProvider.HelpNamespace = value;
            }
        }

        public System.Drawing.Bitmap MediaImage
        {
            set
            {
                this.pictureBox.Image = value;
            }
        }

        #endregion

        #region Form and functions

        private void PlayMedia()
        {
            if (this.File != null)
            {
                try
                {
                    this.userControlPlayer.MediaPlayer.Source = new Uri(this.textBoxFile.Text);
                    this.userControlPlayer.MediaPlayer.Play();
                    this.labelMediumState.Text = MediumState.playing.ToString();
                    //System.Media.SoundPlayer S = new System.Media.SoundPlayer();
                    //S.SoundLocation = this.File;
                    //S.Play();
                    //if (this.File.StartsWith("http://"))
                    //{
                    //    System.Net.WebRequest webrq = System.Net.WebRequest.Create(this.File);
                    //    System.Net.WebResponse webResponse = webrq.GetResponse();
                    //    System.Media.SoundPlayer S = new System.Media.SoundPlayer();
                    //    S.SoundLocation = this.File;
                    //    S.Play();
                    //    long LengthOfUri = webResponse.ContentLength;
                    //    webResponse.Close();
                    //    webrq.Abort();
                    //}
                    //else
                    //{
                    //    DiversityWorkbench.FormMediaPlayer f = new FormMediaPlayer(this.File);
                    //    f.ShowDialog();
                    //}
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    //    {
                    //        try
                    //        {
                    //            if (this.File.StartsWith("http://"))
                    //            {
                    //                System.Net.WebRequest webrq = System.Net.WebRequest.Create(this.File);
                    //                System.Net.WebResponse webResponse = webrq.GetResponse();
                    //                long LengthOfUri = webResponse.ContentLength;
                    //                webResponse.Close();
                    //                webrq.Abort();
                    //            }
                    //        }
                    //        //catch (System.OutOfMemoryException oomex)
                    //        //{
                    //        //    DiversityWorkbench.UserControls.UserControlImage uci = new UserControlImage();
                    //        //    this.pictureBox.Image = (Bitmap)uci.DefaultIconTooBig;
                    //        //}
                    //        catch (System.Exception x)
                    //        {
                    //            //System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)this.imageList.Images[0];
                    //            //this.pictureBox.Image = bmp;
                    //            ////if (MaxSizeOfImage > 0 && (bmp.PhysicalDimension.Height > MaxSizeOfImage
                    //            ////  || bmp.PhysicalDimension.Width > MaxSizeOfImage))
                    //            ////{
                    //            ////    System.Drawing.Bitmap bmpToBig = (System.Drawing.Bitmap)this.DefaultIconTooBig;
                    //            ////    this.pictureBox.Image = bmpToBig;
                    //            ////}
                    //            ////else
                    //            ////    this.pictureBox.Image = bmp;
                    //            //OK = false;
                    //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(x);
                    //            //System.Windows.Forms.MessageBox.Show(x.Message);
                    //        }
                    //    }
                    //    //finally
                    //    //{
                    //    //    //this.AdaptZoom();
                    //    //}
                }
            }
        }
        private void StopPlayback()
        {
            this.userControlPlayer.MediaPlayer.Stop();
            this.labelMediumState.Text = MediumState.stopped.ToString();
        }

        private void PausePlayback()
        {
            this.userControlPlayer.MediaPlayer.Pause();
            this.labelMediumState.Text = MediumState.paused.ToString();
        }

        void userControlPlayer_MediaEnded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.StopPlayback();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (this.labelMediumState.Text == MediumState.playing.ToString())
                this.PausePlayback();
            else if (this.labelMediumState.Text == MediumState.paused.ToString())
            {
                this.userControlPlayer.MediaPlayer.Play();
                this.labelMediumState.Text = MediumState.playing.ToString();
            }
            else
                this.PlayMedia();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.StopPlayback();
        }


        private void buttonExternalBrowser_Click(object sender, EventArgs e)
        {
            if (this.textBoxFile.Text != null)
            {
                this.StopPlayback();
                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(this.textBoxFile.Text);
                info.UseShellExecute = true;
                System.Diagnostics.Process.Start(info);
            }
        }
        #endregion
    }
}
