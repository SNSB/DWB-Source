using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Net;


namespace DiversityWorkbench
{
    public class QRCode
    {

        public static readonly string QRcodeServiceDefaultTemplate =
            global::DiversityWorkbench.Properties.Settings.Default.SNSBQRCodeServicew;

        public enum QRcodeService { SNSB, Quickchart, UserDefined }
        public static string QRCodeImage(string StringToCode, int Size, string ImageDirectory)
        {
            return DiversityWorkbench.QRCode.QRCodeImage(StringToCode, Size, ImageDirectory, "QR");
        }

        public static string QRCodeImage(string StringToCode, int Size, string ImageDirectory, string FileName, QRcodeService qRCodeService = QRcodeService.SNSB)
        {
            try
            {
                if (qRCodeService == QRcodeService.SNSB && global::DiversityWorkbench.Properties.Settings.Default.SNSBQRCodeServicew!= QRcodeServiceDefaultTemplate) { qRCodeService = QRcodeService.UserDefined; }

                //var url = string.Format("http://chart.apis.google.com/chart?cht=qr&chs={1}x{2}&chl={0}", StringToCode, Size.ToString(), Size.ToString());
                //var url = string.Format("https://chart.googleapis.com/chart?cht=qr&chs={1}x{2}&chl={0}", StringToCode, Size.ToString(), Size.ToString());
                var url = "";
                switch (qRCodeService)
                {
                    case QRcodeService.SNSB:
                        url = string.Format(QRcodeServiceDefaultTemplate, StringToCode, Size.ToString());
                        break;
                    case QRcodeService.Quickchart:
                        url = string.Format("https://quickchart.io/qr?text=" + StringToCode);
                        break;
                    case QRcodeService.UserDefined:
                        url = string.Format(DiversityWorkbench.Settings.QRcodeService, StringToCode, Size.ToString());
                        break;
                }
                WebResponse response = default(WebResponse);
                Stream remoteStream = default(Stream);
                StreamReader readStream = default(StreamReader);
                WebRequest request = WebRequest.Create(url);
                response = request.GetResponse();
                remoteStream = response.GetResponseStream();
                readStream = new StreamReader(remoteStream);
                System.Drawing.Image img = System.Drawing.Image.FromStream(remoteStream);
                System.IO.DirectoryInfo D = new DirectoryInfo(ImageDirectory);
                if (!D.Exists)
                    D.Create();
                if (!ImageDirectory.EndsWith("\\"))
                    ImageDirectory += "\\";
                img.Save(ImageDirectory + FileName + ".png");
                response.Close();
                remoteStream.Close();
                readStream.Close();
                StringToCode = string.Empty;
                return ImageDirectory + FileName + ".png";
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return "";
        }
    
    }
}
