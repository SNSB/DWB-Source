using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Windows.Forms;

namespace DiversityWorkbench.DwbManual
{
    public class Hugo
    {
        // #35

        #region ApacheService

        /// <summary>
        /// The lin to the file as provided by the settings
        /// </summary>
        private static string KeywordsDwbFile { get { return Properties.Settings.Default.Keywords; } }

        /// <summary>
        /// The prefix for the keywords as provided by the settings
        /// </summary>
        private static string KeywordsDwbPrefix { get { return Properties.Settings.Default.KeywordPrefix; } }

        private static System.Collections.Generic.Dictionary<string, string> _Keywords_Dwb;

        /// <summary>
        /// Creating the dict for the keyword links based on the file provided by the apache service
        /// </summary>
        /// <returns></returns>
        private static async Task<System.Collections.Generic.Dictionary<string, string>> Keywords_Dwb()
        {
            if (_Keywords_Dwb == null || (_Keywords_Dwb != null && _Keywords_Dwb.Count == 0))
            {
                try
                {
                    _Keywords_Dwb = new Dictionary<string, string>();
                    using (HttpClient client = new HttpClient())
                    {
                        string content = await client.GetStringAsync(KeywordsDwbFile);
                        string[] lines = content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string line in lines)
                        {
                            string[] parts = line.Split(new[] { ' ' }, 2);
                            if (parts.Length == 2 && !_Keywords_Dwb.ContainsKey(parts[0]) && !parts[0].StartsWith("#"))
                            {
                                _Keywords_Dwb.Add(parts[0], parts[1]);
                            }
                        }
                    }
                }
                catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("Error opening URL: " + ex.Message); }
            }
            return _Keywords_Dwb;
        }

        /// <summary>
        /// Providing a link to the manual for a keyword stored in the client
        /// </summary>
        /// <param name="keyword">The keyword taken from the client</param>
        /// <returns>nothing resp. Task - needed for async operation</returns>
        public async Task OpenDwbManual(string keyword)
        {
            if (Keywords_Dwb() == null) { return; }
            Dictionary<string, string> keys = await Keywords_Dwb();
            if (keys.ContainsKey(keyword.ToLower()))
            {
                string Link = KeywordsDwbPrefix + keys[keyword.ToLower()];
                try
                {
                    if (Link.Length > 0)
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = Link,
                            UseShellExecute = true
                        });
                    }
                }
                catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("Error opening URL: " + ex.Message); }
            }
        }


        #endregion

        #region Adding functions

        /// <summary>
        /// If the control contains a keyword related to the helpprovider of the form
        /// </summary>
        /// <param name="control"></param>
        /// <param name="helpProvider"></param>
        /// <returns></returns>
        private bool IsControlLinkedToHelpKeyword(Control control, HelpProvider helpProvider)
        {
            string helpKeyword = helpProvider.GetHelpKeyword(control);
            return !string.IsNullOrEmpty(helpKeyword);
        }

        /// <summary>
        /// If the form contains a keyword related to the helpprovider
        /// </summary>
        /// <param name="form"></param>
        /// <param name="helpProvider"></param>
        /// <returns></returns>
        private bool IsFormLinkedToHelpKeyword(Form form, HelpProvider helpProvider)
        {
            string helpKeyword = helpProvider.GetHelpKeyword(form);
            return !string.IsNullOrEmpty(helpKeyword);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="helpProvider">The helpprovider of the form</param>
        /// <param name="form">The form where the event handlers should be added</param>
        public Hugo(HelpProvider helpProvider, Form form)
        {
            _helpProvider = helpProvider;
            _form = form;
        }

        /// <summary>
        /// HelpProvider of the form
        /// </summary>
        private HelpProvider _helpProvider;

        /// <summary>
        /// the form containing the HelpProvider
        /// </summary>
        private System.Windows.Forms.Form _form;

        /// <summary>
        /// adding the event delegates to form and controls
        /// </summary>
        /// <returns></returns>
        public async Task addKeyDownF1ToForm()
        {
            try
            {
                if (_form != null && _helpProvider != null)
                {
                    if (IsFormLinkedToHelpKeyword((Form)_form, _helpProvider))
                    {
                        _form.KeyUp += new KeyEventHandler(form_KeyDown);
                    }
                    foreach (System.Windows.Forms.Control C in _form.Controls)
                    {
                        await addKeyDownF1ToControls(C);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// Adding Event delegates to the controls
        /// </summary>
        /// <param name="Cont">the control to which the delegate should be added it a keyword is present</param>
        /// <returns></returns>
        private async Task addKeyDownF1ToControls(System.Windows.Forms.Control Cont)
        {
            try
            {
                foreach (System.Windows.Forms.Control C in Cont.Controls)
                {
                    if (IsControlLinkedToHelpKeyword(C, _helpProvider))
                    {
                        //C.KeyDown += new KeyEventHandler(control_KeyDown);
                        C.HelpRequested += new HelpEventHandler(control_HelpRequested);
                    }
                    await addKeyDownF1ToControls(C);
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// Opening the manual if F1 is pressed within the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void form_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!e.Handled && e.KeyCode == Keys.F1)
                {
                    string Keyword = _helpProvider.GetHelpKeyword(_form);
                    await OpenDwbManual(Keyword);
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }


        /// <summary>
        /// Opening the manual if F1 is pressed within the control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void control_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    string Keyword = _helpProvider.GetHelpKeyword((System.Windows.Forms.Control)sender);
                    await OpenDwbManual(Keyword);
                    e.Handled = true;
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// Opening the manual if F1 is pressed within the control #35
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void control_HelpRequested(object sender, HelpEventArgs e)
        {
            try
            {
                if (true)
                {
                    string Keyword = _helpProvider.GetHelpKeyword((System.Windows.Forms.Control)sender);
                    await OpenDwbManual(Keyword);
                    e.Handled = true;
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }


        #endregion

        #region Static methods
        /// <summary>
        /// #35 - providing the keyword postfix for the module
        /// </summary>
        public static string KeywordPostfix
        {
            get
            {
                string Postfix = "";
                switch(DiversityWorkbench.Settings.ModuleName)
                {
                    case "DiversityAgents":
                        Postfix = "_DA";
                        break;
                    case "DiversityCollection":
                        Postfix = "_DC";
                        break;
                    case "DiversityDescriptions":
                        Postfix = "_DD";
                        break;
                    case "DiversityExsiccatae":
                        Postfix = "_DE";
                        break;
                    case "DiversityGazetteer":
                        Postfix = "_DG";
                        break;
                    case "DiversityProjects":
                        Postfix = "_DP";
                        break;
                    case "DiversityReferences":
                        Postfix = "_DR";
                        break;
                    case "DiversitySamplingPlots":
                        Postfix = "_DSP";
                        break;
                    case "DiversityScientificTerms":
                        Postfix = "_DST";
                        break;
                    case "DiversityTaxonNames":
                        Postfix = "_DTN";
                        break;
                    default:
                        Postfix = "_DWB";
                        break;
                }
                return Postfix;
            }
        }
        #endregion
    }
}
