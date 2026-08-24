//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web;
//using System.IO;
//using System.Net;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


namespace DiversityWorkbench
{
    //public class QRCode
    //{

    //    public static readonly string QRcodeServiceDefaultTemplate =
    //        global::DiversityWorkbench.Properties.Settings.Default.SNSBQRCodeServicew;

    //    public enum QRcodeService { SNSB, Quickchart, UserDefined }
    //    public static string QRCodeImage(string StringToCode, int Size, string ImageDirectory)
    //    {
    //        return DiversityWorkbench.QRCode.QRCodeImage(StringToCode, Size, ImageDirectory, "QR");
    //    }

    //    public static string QRCodeImage(string StringToCode, int Size, string ImageDirectory, string FileName, QRcodeService qRCodeService = QRcodeService.SNSB)
    //    {
    //        try
    //        {
    //            if (qRCodeService == QRcodeService.SNSB && global::DiversityWorkbench.Properties.Settings.Default.SNSBQRCodeServicew!= QRcodeServiceDefaultTemplate) { qRCodeService = QRcodeService.UserDefined; }

    //            //var url = string.Format("http://chart.apis.google.com/chart?cht=qr&chs={1}x{2}&chl={0}", StringToCode, Size.ToString(), Size.ToString());
    //            //var url = string.Format("https://chart.googleapis.com/chart?cht=qr&chs={1}x{2}&chl={0}", StringToCode, Size.ToString(), Size.ToString());
    //            var url = "";
    //            switch (qRCodeService)
    //            {
    //                case QRcodeService.SNSB:
    //                    url = string.Format(QRcodeServiceDefaultTemplate, StringToCode, Size.ToString());
    //                    break;
    //                case QRcodeService.Quickchart:
    //                    url = string.Format("https://quickchart.io/qr?text=" + StringToCode);
    //                    break;
    //                case QRcodeService.UserDefined:
    //                    url = string.Format(DiversityWorkbench.Settings.QRcodeService, StringToCode, Size.ToString());
    //                    break;
    //            }
    //            WebResponse response = default(WebResponse);
    //            Stream remoteStream = default(Stream);
    //            StreamReader readStream = default(StreamReader);
    //            WebRequest request = WebRequest.Create(url);
    //            response = request.GetResponse();
    //            remoteStream = response.GetResponseStream();
    //            readStream = new StreamReader(remoteStream);
    //            System.Drawing.Image img = System.Drawing.Image.FromStream(remoteStream);
    //            System.IO.DirectoryInfo D = new DirectoryInfo(ImageDirectory);
    //            if (!D.Exists)
    //                D.Create();
    //            if (!ImageDirectory.EndsWith("\\"))
    //                ImageDirectory += "\\";
    //            img.Save(ImageDirectory + FileName + ".png");
    //            response.Close();
    //            remoteStream.Close();
    //            readStream.Close();
    //            StringToCode = string.Empty;
    //            return ImageDirectory + FileName + ".png";
    //        }
    //        catch(System.Exception ex)
    //        {
    //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
    //        }
    //        return "";
    //    }

    //}

    // Überarbeitete Version mit asynchroner HTTP-Anfrage und verbesserter Ressourcenverwaltung - #issue-406
    public class QRCode
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static readonly string QRcodeServiceDefaultTemplate =
            global::DiversityWorkbench.Properties.Settings.Default.SNSBQRCodeServicew;

        public enum QRcodeService { SNSB, Quickchart, UserDefined }

        // vorhandene synchrone Signatur bleibt erhalten und ruft die async-Implementierung blockierend auf
        public static string QRCodeImage(string StringToCode, int Size, string ImageDirectory)
        {
            return QRCodeImage(StringToCode, Size, ImageDirectory, "QR");
        }

        public static string QRCodeImage(string StringToCode, int Size, string ImageDirectory, string FileName, QRcodeService qRCodeService = QRcodeService.SNSB)
        {
            // synchroner Wrapper - bevorzugt: QRCodeImageAsync aufrufen
            return QRCodeImageAsync(StringToCode, Size, ImageDirectory, FileName, qRCodeService).GetAwaiter().GetResult();
        }

        // neue asynchrone Implementierung
        public static async Task<string> QRCodeImageAsync(string StringToCode, int Size, string ImageDirectory, string FileName = "QR", QRcodeService qRCodeService = QRcodeService.SNSB)
        {
            try
            {
                if (qRCodeService == QRcodeService.SNSB && global::DiversityWorkbench.Properties.Settings.Default.SNSBQRCodeServicew != QRcodeServiceDefaultTemplate)
                {
                    qRCodeService = QRcodeService.UserDefined;
                }

                string url = qRCodeService switch
                {
                    QRcodeService.SNSB => string.Format(QRcodeServiceDefaultTemplate, Uri.EscapeDataString(StringToCode), Size.ToString()),
                    QRcodeService.Quickchart => "https://quickchart.io/qr?text=" + Uri.EscapeDataString(StringToCode),
                    QRcodeService.UserDefined => string.Format(DiversityWorkbench.Settings.QRcodeService, Uri.EscapeDataString(StringToCode), Size.ToString()),
                    _ => throw new InvalidOperationException("Unbekannter QRcodeService")
                };

                using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                using var ms = new MemoryStream(bytes);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);

                var D = new DirectoryInfo(ImageDirectory);
                if (!D.Exists)
                    D.Create();
                if (!ImageDirectory.EndsWith("\\"))
                    ImageDirectory += "\\";

                var outPath = Path.Combine(ImageDirectory, FileName + ".png");
                img.Save(outPath);

                // Ressourcen freigeben
                img.Dispose();

                StringToCode = string.Empty;
                return outPath;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return string.Empty;
        }
    }
}

