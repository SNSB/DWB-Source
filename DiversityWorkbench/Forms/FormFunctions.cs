using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DiversityWorkbench.Forms
{
    public struct WebImage
    {
        public System.Uri URI;
        public System.Drawing.Bitmap Bitmap;
        public long Size;
        public string Extension;
        public string ContentType;
        public string ContentTypeBase;
    }

    public struct OriginalImage
    {
        public string Name;
        public string Type;
        public System.Drawing.Image Image;
        public int? WidthControl;
        public int? HeightControl;
    }

    public struct CacheDescription
    {
        public string DisplayText;
        public string Abbreviation;
        public string Description;
    }

    public class FormFunctions
    {

        #region Parameter

        public enum DatabaseGrant { Delete, Insert, Select, Update }
        public enum Medium { Unknown, Image, Audio, Video, Ignore, VectorGraphic } // new MediaTye Unknown Toni 3.10.12 Toni 20210429: New medium type Ignore // Markus 20220323: New media type VectorGraphic

        protected Microsoft.Data.SqlClient.SqlConnection SqlConnection;
        private System.Windows.Forms.ToolTip ToolTip;

        private System.Windows.Forms.Form Form;
        private System.Windows.Forms.UserControl _UserControl;
        private System.Windows.Forms.Control _Control;

        #endregion

        #region Construction

        public FormFunctions(System.Windows.Forms.Form Form,
            string ConnectionString,
            ref System.Windows.Forms.ToolTip ToolTip)
        {
            this.Form = Form;
            this.SqlConnection = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            this.ToolTip = ToolTip;
        }

        public FormFunctions(System.Windows.Forms.Form Form,
            Microsoft.Data.SqlClient.SqlConnection Connection,
            ref System.Windows.Forms.ToolTip ToolTip)
        {
            this.Form = Form;
            this.SqlConnection = Connection;
            this.ToolTip = ToolTip;
        }

        public FormFunctions(System.Windows.Forms.Control Control,
            string ConnectionString,
            ref System.Windows.Forms.ToolTip ToolTip)
        {
            this._Control = Control;
            this.SqlConnection = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            this.ToolTip = ToolTip;
        }

        public FormFunctions(System.Windows.Forms.UserControl Control,
            string ConnectionString,
            ref System.Windows.Forms.ToolTip ToolTip)
        {
            this._UserControl = Control;
            this.SqlConnection = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            this.ToolTip = ToolTip;
        }

        #endregion

        #region Form

        /// <summary>
        /// Setting the title of the form
        /// </summary>
        public void setTitle()
        {
            try
            {
                string Project = DiversityWorkbench.Settings.ModuleName;
                // SERVER - should not be displayed in the form
                string Server = "";
                if (DiversityWorkbench.Settings.IsLocalExpressDatabase)
                    Server = DiversityWorkbench.Settings.SqlExpressDbFileName;
                else
                {
                    if (DiversityWorkbench.Settings.DatabaseServer != "") Server = DiversityWorkbench.Settings.DatabaseServer;
                    if (DiversityWorkbench.Settings.DatabasePort != 1433) Server = Server + "    Port: " + DiversityWorkbench.Settings.DatabasePort.ToString();
                }
                // VERSION
                string[] strArr = System.Windows.Forms.Application.ProductVersion.ToString().Split(new Char[] { '.' });
                char Null = System.Convert.ToChar("0");
                string Version = strArr[0] + "." + strArr[1] + "." + strArr[2];
                if (strArr.Length > 3)
                    Version += "." + strArr[3];
                if (this.SqlConnection == null || DiversityWorkbench.Settings.DatabaseServer == "" || DiversityWorkbench.Settings.DatabaseName == "" || DiversityWorkbench.Settings.DatabaseServer == null || DiversityWorkbench.Settings.DatabaseName == null || !this.DatabaseAccessible)
                    this.Form.Text = Project + "    v. " + Version + "    not connected";
                else
                {
                    if (this.SqlConnection != null && DiversityWorkbench.Settings.DatabaseServer != "" && DiversityWorkbench.Settings.DatabaseName == "")
                        this.Form.Text = Project + "    v. " + Version + "   Server: " + Server + "    not connected";
                    else if (this.SqlConnection != null && DiversityWorkbench.Settings.DatabaseServer != "" && DiversityWorkbench.Settings.DatabaseName != "")
                    {
                        if (DiversityWorkbench.Settings.DatabaseName != Project)
                            this.Form.Text = Project + ",  Database: " + DiversityWorkbench.Settings.DatabaseName;
                        else
                            this.Form.Text = DiversityWorkbench.Settings.DatabaseName;
                        this.Form.Text += "    v. " + Version;
                        // For security reasons IP and Port of server should not be displayed in the form
                        // if (!DiversityWorkbench.Settings.IsLocalExpressDatabase) this.Form.Text += "   Server: " + Server;
                        // if (!DiversityWorkbench.Settings.IsLocalExpressDatabase) this.Form.Text += "   Server: remote" + Server;
                        string DatabaseVersion = "";
                    }
                    else
                        this.Form.Text = Project + "    v. " + Version + "    not connected";
                    //if (DiversityWorkbench.Settings.IsTrustedConnection)
                    //{
                    //    this.Form.Text = this.Form.Text + "    User: " + System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    //    if (DiversityWorkbench.Settings.IsLocalExpressDatabase) this.Form.Text += "   Local DB: " + Server;
                    //}
                    //else
                    //{
                    //    if (DiversityWorkbench.Settings.DatabaseUser != "")
                    //        this.Form.Text = this.Form.Text + "    User: " + DiversityWorkbench.Settings.DatabaseUser;
                    //}
                }
            }
            catch (System.Exception ex) { }
        }

        public string ConnectionInfo()
        {
            string Connection = "";
            string Server = "";
            if (DiversityWorkbench.Settings.IsLocalExpressDatabase)
                Server = DiversityWorkbench.Settings.SqlExpressDbFileName;
            else
            {
                if (DiversityWorkbench.Settings.DatabaseServer != "") Server = DiversityWorkbench.Settings.DatabaseServer;
                if (DiversityWorkbench.Settings.DatabasePort != 1433) Server = Server + "    Port: " + DiversityWorkbench.Settings.DatabasePort.ToString();
            }
            if (this.SqlConnection == null || DiversityWorkbench.Settings.DatabaseServer == "" || DiversityWorkbench.Settings.DatabaseName == "" || DiversityWorkbench.Settings.DatabaseServer == null || DiversityWorkbench.Settings.DatabaseName == null || !this.DatabaseAccessible)
                Connection = "    not connected";
            else
            {
                if (this.SqlConnection != null && DiversityWorkbench.Settings.DatabaseServer != "" && DiversityWorkbench.Settings.DatabaseName == "")
                    Connection = "   Server: " + Server + "    not connected";
                else if (this.SqlConnection != null && DiversityWorkbench.Settings.DatabaseServer != "" && DiversityWorkbench.Settings.DatabaseName != "")
                {
                    if (!DiversityWorkbench.Settings.IsLocalExpressDatabase) Connection += "   Server: " + Server;
                }
                else
                    Connection = "    not connected";
                if (DiversityWorkbench.Settings.IsTrustedConnection)
                {
                    Connection = Connection + "    User: " + System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    if (DiversityWorkbench.Settings.IsLocalExpressDatabase) Connection += "   Local DB: " + Server;
                }
                else
                {
                    if (DiversityWorkbench.Settings.DatabaseUser != "")
                        Connection = Connection + "    User: " + DiversityWorkbench.Settings.DatabaseUser;
                }
            }
            Connection = Connection.Trim();
            return Connection;
        }

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="HelpNamespace"></param>
        /// <param name="KeyWord"></param>


        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="HelpProvider">The helpprovider for which the namespace and keyword should be set</param>
        /// <param name="Control">The control for which the namespace and keyword should be set</param>
        /// <param name="HelpNamespace">The namespace for the helpprovider</param>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public static void SetHelp(System.Windows.Forms.HelpProvider HelpProvider, System.Windows.Forms.Control Control, string HelpNamespace, string KeyWord)
        {
            try
            {
                if (HelpNamespace.Length > 0)
                    HelpProvider.HelpNamespace = HelpNamespace;
                if (HelpProvider.HelpNamespace == null || HelpProvider.HelpNamespace.Length == 0)
                    HelpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();// WorkbenchDirectoryModule() + "\\" + DiversityWorkbench.Settings.ModuleName + ".chm";
                HelpProvider.SetHelpKeyword(Control, KeyWord);
                HelpProvider.SetHelpNavigator(Control, System.Windows.Forms.HelpNavigator.KeywordIndex);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="HelpProvider">The helpprovider for which the namespace and keyword should be set</param>
        /// <param name="Form">The form in which for all controls the namespace and keyword should be set</param>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public static void SetHelp(System.Windows.Forms.HelpProvider HelpProvider, System.Windows.Forms.Form Form, string KeyWord)
        {
            try
            {
                if (HelpProvider.HelpNamespace == null || HelpProvider.HelpNamespace.Length == 0)
                    HelpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();// WorkbenchDirectoryModule() + "\\" + DiversityWorkbench.Settings.ModuleName + ".chm";
                foreach (System.Windows.Forms.Control C in Form.Controls)
                {
                    HelpProvider.SetHelpKeyword(C, KeyWord);
                    HelpProvider.SetHelpNavigator(C, System.Windows.Forms.HelpNavigator.KeywordIndex);
                    HelpProvider.SetShowHelp(C, true);
                    //HelpProvider.SetHelpString(C, KeyWord);
                    //if (C.Controls.Count > 0)
                    //{
                    //    foreach (System.Windows.Forms.Control CC in C.Controls)
                    //    {
                    //        HelpProvider.SetHelpKeyword(CC, KeyWord);
                    //        HelpProvider.SetHelpNavigator(CC, System.Windows.Forms.HelpNavigator.KeywordIndex);
                    //        HelpProvider.SetShowHelp(CC, true);
                    //        HelpProvider.SetHelpString(CC, KeyWord);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="HelpProvider">The helpprovider for which the namespace and keyword should be set</param>
        /// <param name="Form">The form in which for all controls the namespace and keyword should be set</param>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public static void SetHelp(System.Windows.Forms.HelpProvider HelpProvider, System.Windows.Forms.UserControl Control, string KeyWord)
        {
            try
            {
                if (HelpProvider.HelpNamespace == null || HelpProvider.HelpNamespace.Length == 0)
                    HelpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();// WorkbenchDirectoryModule() + "\\" + DiversityWorkbench.Settings.ModuleName + ".chm";
                foreach (System.Windows.Forms.Control C in Control.Controls)
                {
                    HelpProvider.SetHelpKeyword(C, KeyWord);
                    HelpProvider.SetHelpNavigator(C, System.Windows.Forms.HelpNavigator.KeywordIndex);
                    HelpProvider.SetShowHelp(C, true);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        #endregion

        #region Image scaling in tool strips

        private static System.Collections.Generic.Dictionary<string, OriginalImage> _OriginalImages = new Dictionary<string, OriginalImage>();

        private static OriginalImage getOriginalImage(string Name)
        {
            if (_OriginalImages.ContainsKey(Name))
            {
                return _OriginalImages[Name];
            }
            else
            {
                OriginalImage O = new OriginalImage();
                O.Name = "";
                return O;
            }
        }

        //// TODO check Ariane uncommented since this is no longer needed in .net8 ?
        //public static void SetImageScalingForm(System.Windows.Forms.Form Form, bool SizeToFit, System.Windows.Forms.ToolStrip MainMenu = null)
        //{
        //    try
        //    {
        //        if (Form.MainMenuStrip != null)
        //        {
        //            foreach (System.Windows.Forms.ToolStripMenuItem M in Form.MainMenuStrip.Items)
        //            {
        //                if (M.GetType() == typeof(System.Windows.Forms.ToolStripMenuItem))
        //                {
        //                    if (SizeToFit)
        //                        M.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.SizeToFit;
        //                    else
        //                        M.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
        //                    SetImageScalingToolStripItem(M, SizeToFit);
        //                }
        //            }
        //        }
        //        else if (MainMenu != null)
        //        {
        //            foreach (System.Windows.Forms.ToolStripMenuItem M in MainMenu.Items)
        //            {
        //                if (M.GetType() == typeof(System.Windows.Forms.ToolStripMenuItem))
        //                {
        //                    if (SizeToFit)
        //                        M.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.SizeToFit;
        //                    else
        //                        M.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
        //                    SetImageScalingToolStripItem(M, SizeToFit);
        //                }
        //            }
        //        }
        //        foreach (System.Windows.Forms.Control C in Form.Controls)
        //        {
        //            if (C.GetType() == typeof(System.Windows.Forms.GroupBox)
        //                || C.GetType() == typeof(System.Windows.Forms.Panel)
        //                || C.GetType() == typeof(System.Windows.Forms.TableLayoutPanel)
        //                || C.GetType() == typeof(System.Windows.Forms.SplitContainer)
        //                || C.GetType() == typeof(System.Windows.Forms.SplitterPanel)
        //                || C.GetType() == typeof(System.Windows.Forms.UserControl)
        //                || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)
        //                || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryList)
        //                || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlLocalList)
        //                || C.GetType() == typeof(System.Windows.Forms.ToolStrip)
        //                || C.GetType() == typeof(System.Windows.Forms.TabPage)
        //                || C.GetType() == typeof(System.Windows.Forms.TabControl))
        //                SetImageScalingControl(C, SizeToFit);
        //            if (C.GetType() == typeof(System.Windows.Forms.ToolStrip))
        //            {
        //                SetImageScalingToolStrip((System.Windows.Forms.ToolStrip)C, SizeToFit);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //// TODO Ariane check if this is in use in DD, DP usw.
        //public static void SetImageScalingControl(System.Windows.Forms.Control Control, bool SizeToFit)
        //{
        //    try
        //    {
        //        foreach (System.Windows.Forms.Control C in Control.Controls)
        //        {
        //            if (C.GetType() == typeof(System.Windows.Forms.GroupBox)
        //                || C.GetType() == typeof(System.Windows.Forms.Panel)
        //                || C.GetType() == typeof(System.Windows.Forms.TableLayoutPanel)
        //                || C.GetType() == typeof(System.Windows.Forms.SplitContainer)
        //                || C.GetType() == typeof(System.Windows.Forms.SplitterPanel)
        //                || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)
        //                || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryList)
        //                || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlLocalList)
        //                || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryCondition)
        //                || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryConditionSet)
        //                //|| C.GetType() == typeof(System.Windows.Forms.ToolStrip)
        //                || C.GetType() == typeof(System.Windows.Forms.TabPage)
        //                || C.GetType() == typeof(System.Windows.Forms.TabControl))
        //                SetImageScalingControl(C, SizeToFit);
        //            else if (C.GetType() == typeof(System.Windows.Forms.ToolStrip))
        //            {
        //                SetImageScalingToolStrip((System.Windows.Forms.ToolStrip)C, SizeToFit);
        //            }
        //            else if (C.GetType() == typeof(System.Windows.Forms.Button))
        //            {
        //                SetImageScalingButton((System.Windows.Forms.Button)C, SizeToFit);
        //            }
        //            else if (C.GetType() == typeof(System.Windows.Forms.PictureBox))
        //            {
        //                SetImageScalingPicture((System.Windows.Forms.PictureBox)C, SizeToFit);
        //            }
        //            else if (C.GetType() == typeof(System.Windows.Forms.CheckBox))
        //            {
        //                SetImageScalingCheckBox((System.Windows.Forms.CheckBox)C, SizeToFit);
        //            }
        //            else if (C.GetType() == typeof(System.Windows.Forms.TreeView))
        //            {
        //                SetImageScalingTreeView((System.Windows.Forms.TreeView)C, SizeToFit);
        //            }
        //            else if (C.GetType() == typeof(System.Windows.Forms.ListBox)
        //                || C.GetType() == typeof(System.Windows.Forms.TextBox)
        //                || C.GetType() == typeof(System.Windows.Forms.ComboBox)
        //                || C.GetType() == typeof(System.Windows.Forms.CheckedListBox)
        //                || C.GetType() == typeof(System.Windows.Forms.DateTimePicker)
        //                || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlDatePanel)
        //                || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlHierarchySelector)
        //                || C.GetType() == typeof(System.Windows.Forms.MaskedTextBox)
        //                || C.GetType() == typeof(System.Windows.Forms.Label))
        //            {
        //            }
        //            else
        //            {
        //                if (C.Controls != null && C.Controls.Count > 0)
        //                {
        //                    SetImageScalingControl(C, SizeToFit);
        //                }
        //            }
        //        }
        //        try
        //        {
        //            if (false)
        //            {
        //                if (Control.GetType() == typeof(System.Windows.Forms.ToolStrip))
        //                {
        //                    System.Windows.Forms.ToolStrip TS = (System.Windows.Forms.ToolStrip)Control;
        //                    if (TS.Items != null && TS.Items.Count > 0)
        //                    {
        //                        foreach (System.Object O in TS.Items)
        //                        {
        //                            if (O.GetType() == typeof(System.Windows.Forms.ToolStripItem))
        //                            {
        //                                System.Windows.Forms.ToolStripItem TSI = (System.Windows.Forms.ToolStripItem)O;
        //                                SetImageScalingToolStripItem(TSI, SizeToFit);
        //                            }
        //                            else if (O.GetType() == typeof(System.Windows.Forms.ToolStripButton))
        //                            {
        //                                System.Windows.Forms.ToolStripButton TB = (System.Windows.Forms.ToolStripButton)O;
        //                                if (SizeToFit)
        //                                    TB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.SizeToFit;
        //                                else
        //                                    TB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
        //                            }
        //                            else if (O.GetType() == typeof(System.Windows.Forms.ToolStripDropDownButton))
        //                            {
        //                                System.Windows.Forms.ToolStripDropDownButton TB = (System.Windows.Forms.ToolStripDropDownButton)O;
        //                                if (SizeToFit)
        //                                    TB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.SizeToFit;
        //                                else
        //                                    TB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
        //                                if (TB.DropDownItems.Count > 0)
        //                                {
        //                                    foreach (System.Object oo in TB.DropDownItems)
        //                                    {
        //                                        if (oo.GetType() == typeof(System.Windows.Forms.ToolStripMenuItem))
        //                                        {
        //                                            System.Windows.Forms.ToolStripMenuItem TM = (System.Windows.Forms.ToolStripMenuItem)oo;
        //                                            if (SizeToFit)
        //                                                TM.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.SizeToFit;
        //                                            else
        //                                                TM.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
        //                                            SetImageScalingToolStripMenuItem(TM, SizeToFit);
        //                                        }
        //                                        else
        //                                        {

        //                                        }
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {

        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        catch (System.Exception ex)
        //        {
        //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //// TODO Ariane chieck if this is in use in DD, DP, ...
        //public static void SetImageScalingToolStripMenuItem(System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem, bool SizeToFit)
        //{
        //    try
        //    {
        //        foreach (System.Windows.Forms.ToolStripItem M in ToolStripMenuItem.DropDownItems)
        //        {
        //            if (M.GetType() == typeof(System.Windows.Forms.ToolStripMenuItem))
        //            {
        //                if (SizeToFit)
        //                    M.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.SizeToFit;
        //                else
        //                    M.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
        //                SetImageScalingToolStripMenuItem((System.Windows.Forms.ToolStripMenuItem)M, SizeToFit);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

//        // TODO Ariane chieck if this is in use in DD, DP, ...
//        private static void SetImageScalingToolStrip(System.Windows.Forms.ToolStrip Strip, bool SizeToFit)
//        {
//            try
//            {
//                foreach (System.Windows.Forms.ToolStripItem M in Strip.Items)
//                {
//                    if (M.GetType() == typeof(System.Windows.Forms.ToolStripMenuItem))
//                    {
//                        if (SizeToFit)
//                            M.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.SizeToFit;
//                        else
//                            M.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
//                        SetImageScalingToolStripMenuItem((System.Windows.Forms.ToolStripMenuItem)M, SizeToFit);
//                    }
//                    else if (M.GetType() == typeof(System.Windows.Forms.ToolStripButton))
//                    {
//                        System.Drawing.Bitmap Inew = null;
//                        if (SizeToFit)
//                        {
//                            if (M.Image != null)
//                            {
//                                int Width = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(M.Image.Width);
//                                int Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(M.Image.Height);
//                                Inew = ResizeImage(M.Image, Width, Height);
//#if DEBUG
//                                int Size = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(16);
//                                Size = 32; // only for Test - Remove after Test
//                                Inew = ResizeImage(M.Image, Size, Size);
//#endif
//                                M.Image = Inew;
//                            }
//                            //M.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.SizeToFit;
//                            M.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
//                        }
//                        else
//                        {
//                            if (M.Image != null)
//                            {
//                                Inew = ResizeImage(M.Image, 16, 16);
//                                M.Image = Inew;
//                            }
//                            M.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
//                        }
//                    }
//                    else if (M.GetType() == typeof(System.Windows.Forms.ToolStripDropDownButton))
//                    {
//                        System.Drawing.Bitmap Inew = null;
//                        System.Windows.Forms.ToolStripDropDownButton TB = (System.Windows.Forms.ToolStripDropDownButton)M;
//                        if (SizeToFit)
//                        {
//                            if (TB.Image != null)
//                            {
//                                int Width = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(TB.Image.Width);
//                                int Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(TB.Image.Height);
//                                Inew = ResizeImage(M.Image, Width, Height);
//#if DEBUG
//                                int Size = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(16);
//                                Size = 32; // only for Test - Remove after Test
//                                Inew = ResizeImage(M.Image, Size, Size);
//#endif
//                                TB.Image = Inew;
//                            }
//                            //M.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.SizeToFit;
//                            TB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
//                        }
//                        else
//                        {
//                            if (TB.Image != null)
//                            {
//                                Inew = ResizeImage(M.Image, 16, 16);
//                                TB.Image = Inew;
//                            }
//                            TB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
//                            //TB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
//                        }
//                        if (TB.DropDownItems.Count > 0)
//                        {
//                            foreach (System.Object oo in TB.DropDownItems)
//                            {
//                                if (oo.GetType() == typeof(System.Windows.Forms.ToolStripMenuItem))
//                                {
//                                    System.Windows.Forms.ToolStripMenuItem TM = (System.Windows.Forms.ToolStripMenuItem)oo;
//                                    if (SizeToFit)
//                                        TM.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.SizeToFit;
//                                    else
//                                        TM.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
//                                    SetImageScalingToolStripMenuItem(TM, SizeToFit);
//                                }
//                                else
//                                {

//                                }
//                            }
//                        }
//                    }

//                }
//            }
//            catch (Exception ex)
//            {
//                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
//            }
//        }

        //// TODO Ariane check if this is in use in DP DD, ..
        //public static void SetImageScalingToolStripItem(System.Windows.Forms.ToolStripItem ToolStripItem, bool SizeToFit)
        //{
        //    try
        //    {
        //        if (ToolStripItem.GetType() == typeof(System.Windows.Forms.ToolStripMenuItem) ||
        //            ToolStripItem.GetType() == typeof(System.Windows.Forms.ToolStripButton) ||
        //            ToolStripItem.GetType() == typeof(System.Windows.Forms.ToolStripDropDownButton))
        //        {
        //            if (SizeToFit)
        //                ToolStripItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.SizeToFit;
        //            else
        //                ToolStripItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
        //            if (ToolStripItem.GetType() == typeof(System.Windows.Forms.ToolStripDropDownButton))
        //            {
        //                System.Windows.Forms.ToolStripDropDownButton ToolStripMenuItem = (System.Windows.Forms.ToolStripDropDownButton)ToolStripItem;
        //                foreach (System.Windows.Forms.ToolStripItem M in ToolStripMenuItem.DropDownItems)
        //                {
        //                    if (M.GetType() == typeof(System.Windows.Forms.ToolStripMenuItem))
        //                    {
        //                        if (SizeToFit)
        //                            M.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.SizeToFit;
        //                        else
        //                            M.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
        //                        SetImageScalingToolStripItem((System.Windows.Forms.ToolStripMenuItem)M, SizeToFit);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem = (System.Windows.Forms.ToolStripMenuItem)ToolStripItem;
        //                foreach (System.Windows.Forms.ToolStripItem M in ToolStripMenuItem.DropDownItems)
        //                {
        //                    if (M.GetType() == typeof(System.Windows.Forms.ToolStripMenuItem))
        //                    {
        //                        if (SizeToFit)
        //                            M.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.SizeToFit;
        //                        else
        //                            M.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
        //                        SetImageScalingToolStripItem((System.Windows.Forms.ToolStripMenuItem)M, SizeToFit);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

//        // TODO ARiane check if this is in use in DP, DD, ..
//        private static void SetImageScalingButton(System.Windows.Forms.Button Button, bool SizeToFit)
//        {
//            try
//            {
//                System.Drawing.Bitmap Inew = null;
//                if (Button.Image != null)
//                {
//                    System.Drawing.Image I = Button.Image;
//                    if (SizeToFit)
//                    {
//                        int Width = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(I.Width);
//                        int Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(I.Height);
//                        Inew = ResizeImage(I, Width, Height);
//#if DEBUG
//                        int Size = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(16);
//                        Size = 32; // only for Test - Remove after Test
//                        Inew = ResizeImage(I, Size, Size);
//#endif
//                    }
//                    else
//                    {
//                        if (I.Height < 16 || I.Width < 16)
//                            Inew = ResizeImage(I, I.Width, I.Height);
//                        else
//                            Inew = ResizeImage(I, 16, 16);
//                    }
//                }
//                if (Inew != null)
//                    Button.Image = Inew;

//            }
//            catch (Exception ex)
//            {
//                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
//            }
//        }

//        // TODO Ariane check if this is in use in DP, DD, ..
//        private static void SetImageScalingPicture(System.Windows.Forms.PictureBox pictureBox, bool SizeToFit)
//        {
//            try
//            {
//                if (pictureBox.Image != null)
//                {
//                    pictureBox.Image = getResizedImage(SizeToFit, pictureBox.Name, pictureBox.GetType().ToString(), pictureBox.Image, pictureBox.Width, pictureBox.Height);
//                }
//            }
//            catch (Exception ex)
//            {
//                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
//            }
//        }

//        // TODO Ariane check if this is in use in DP, DD, ..
//        private static void SetImageScalingCheckBox(System.Windows.Forms.CheckBox checkBox, bool SizeToFit)
//        {
//            try
//            {
//                System.Drawing.Bitmap Inew = null;
//                if (checkBox.Image != null)
//                {
//                    System.Drawing.Image I = checkBox.Image;
//                    if (SizeToFit)
//                    {
//                        int Width = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(I.Width);
//                        int Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(I.Height);
//                        Inew = ResizeImage(I, Width, Height);
//#if DEBUG
//                        int Size = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(16);
//                        Size = 32; // only for Test - Remove after Test
//                        Inew = ResizeImage(I, Size, Size);
//#endif
//                    }
//                    else
//                    {
//                        if (I.Height < 16 || I.Width < 16)
//                            Inew = ResizeImage(I, I.Width, I.Height);
//                        else
//                            Inew = ResizeImage(I, 16, 16);
//                    }
//                }
//                if (Inew != null)
//                    checkBox.Image = Inew;

//            }
//            catch (Exception ex)
//            {
//                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
//            }
//        }

//        private static void SetImageScalingTreeView(System.Windows.Forms.TreeView treeView, bool SizeToFit)
//        {
//            return;

//            // Klappt leider nicht - Bilder werden zwar groesser, sind aber leer
//            try
//            {
//                System.Windows.Forms.ImageList II = new System.Windows.Forms.ImageList();
//                if (treeView.ImageList != null)
//                {
//                    int iSize = treeView.ImageList.ImageSize.Width;
//                    if (SizeToFit)
//                    {
//                        iSize = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(iSize);
//#if DEBUG
//                        iSize = 32;
//#endif
//                    }
//                    else
//                        iSize = 16;
//                    treeView.ImageList.ImageSize = new Size(iSize, iSize);
//                    //II.ImageSize = new Size(iSize, iSize);
//                    for (int i = 0; i < treeView.ImageList.Images.Count; i++)
//                    {
//                        System.Drawing.Bitmap Inew = null;
//                        if (SizeToFit)
//                        {
//                            int Width = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(treeView.ImageList.Images[i].Width);
//                            int Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(treeView.ImageList.Images[i].Height);
//                            Inew = ResizeImage(treeView.ImageList.Images[i], Width, Height);
//#if DEBUG
//                            int Size = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(16);
//                            Size = 32; // only for Test - Remove after Test
//                            Inew = ResizeImage(treeView.ImageList.Images[i], Size, Size);
//#endif
//                        }
//                        else
//                        {
//                            if (treeView.ImageList.Images[i].Height < 16 || treeView.ImageList.Images[i].Width < 16)
//                                Inew = ResizeImage(treeView.ImageList.Images[i], treeView.ImageList.Images[i].Width, treeView.ImageList.Images[i].Height);
//                            else
//                                Inew = ResizeImage(treeView.ImageList.Images[i], 16, 16);
//                        }
//                        treeView.ImageList.Images[i] = Inew;
//                        //II.Images.Add(Inew);
//                    }
//                }
//            }
//            catch (System.Exception ex)
//            {
//                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
//            }
//        }


        public static Bitmap getResizedImage(bool SizeToFit, string Name, string Type, Image image, int? WidthControl = null, int? HeigthControl = null)
        {
            if (!_OriginalImages.ContainsKey(Name) && (image.Width > 16 || image.Height > 16))
            {
                OriginalImage O = new OriginalImage();
                O.Name = Name;
                O.Type = Type;
                O.Image = image;
                O.WidthControl = WidthControl;
                O.HeightControl = HeigthControl;
                _OriginalImages.Add(Name, O);
            }

            if (_OriginalImages.ContainsKey(Name))
                image = _OriginalImages[Name].Image;

            int Width = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(image.Width);
            int Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(image.Height);
            if (SizeToFit)
            {
#if DEBUG
                Width = 32; // only for Test - Remove after Test
                Height = 32;
#endif
            }
            else
            {
                Width = image.Width;
                if (Width > 16)
                    Width = 16;
                Height = image.Height;
                if (Height > 16)
                    Height = 16;
            }
            var destImage = new Bitmap(Width, Height);
            destImage = ResizeImage(image, Width, Height);
            return destImage;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        /// source: https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                using (var wrapMode = new System.Drawing.Imaging.ImageAttributes())
                {
                    wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        /// <summary>
        /// Get the rotation info of an image as documented in the EXIF data
        /// </summary>
        /// <param name="XML">EXIF data</param>
        /// <returns>The rotation</returns>
        public static System.Drawing.RotateFlipType ExifRotationInfo(string XML)
        {
            System.Drawing.RotateFlipType Rotate = RotateFlipType.RotateNoneFlipNone;
            try
            {
                System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
                xml.LoadXml(XML);
                bool done = false;
                foreach (System.Xml.XmlNode node in xml.ChildNodes)
                {
                    foreach (System.Xml.XmlNode Cnode in node.ChildNodes)
                    {

                        foreach (System.Xml.XmlNode CCnode in Cnode.ChildNodes)
                        {
                            if (CCnode.Name == "IFD0:Orientation")
                            {
                                foreach (System.Xml.XmlNode CCCnode in CCnode.ChildNodes)
                                {
                                    switch (CCCnode.Value)
                                    {
                                        case "Rotate 90 CW":
                                            Rotate = RotateFlipType.Rotate90FlipNone;
                                            break;
                                        case "Rotate 270 CW":
                                            Rotate = RotateFlipType.Rotate270FlipNone;
                                            break;
                                        case "Rotate 180":
                                            Rotate = RotateFlipType.Rotate180FlipNone;
                                            break;
                                    }
                                }
                                done = true;
                                break;
                            }
                        }
                        if (done) break;
                    }
                    if (done) break;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Rotate;
        }

        #endregion

        #region Connection
        public bool ChooseServer() //DiversityWorkbench.ApplicationSettings applicationSettings)
        {
            bool OK = false;
            return OK;
        }

        private static bool _ShowServerActiviyForPostgres = false;
        private static readonly int _PostgresProcessActivityAge = 30;

        public static void ShowServerActiviy(int? Width = null, int? Height = null, bool ForPostgres = false)
        {
            // Markus 3.5.23: Auch fuer Postgres verwendet, Anpassung daran und Oeffnen nur als Dialog
            _ShowServerActiviyForPostgres = ForPostgres;
            try
            {
                //if (FormServerActivity == null || FormServerActivity.IsDisposed || ForPostgres)
                {
                    using (FormServerActivity = new System.Windows.Forms.Form())
                    {
                        Bitmap bmp = (Bitmap)DiversityWorkbench.Properties.Resources.ServerIO;
                        Bitmap newBmp = new Bitmap(bmp);
                        Bitmap targetBmp = newBmp.Clone(new Rectangle(0, 0, newBmp.Width, newBmp.Height), System.Drawing.Imaging.PixelFormat.Format64bppArgb);
                        IntPtr Hicon = targetBmp.GetHicon();
                        Icon myIcon = Icon.FromHandle(Hicon);

                        FormServerActivity.Icon = myIcon;
                        System.Windows.Forms.Button B = new System.Windows.Forms.Button();
                        B.Text = "Refresh";
                        System.Drawing.Font F = new Font(B.Font, FontStyle.Bold);
                        B.Font = F;
                        B.Image = DiversityWorkbench.Properties.Resources.Transfrom;
                        B.TextAlign = ContentAlignment.MiddleCenter;
                        B.ImageAlign = ContentAlignment.MiddleLeft;
                        B.Click += DiversityWorkbench.Forms.FormFunctions.getServerActivity_Click;
                        B.Dock = System.Windows.Forms.DockStyle.Bottom;
                        B.Height = 26;
                        FormServerActivity.Controls.Add(B);

                        System.Windows.Forms.Button Bkill = new System.Windows.Forms.Button();
                        Bkill.Text = "Kill selected process";
                        Bkill.Font = F;
                        Bkill.Image = DiversityWorkbench.Properties.Resources.Stop3;
                        Bkill.TextAlign = ContentAlignment.MiddleCenter;
                        Bkill.ImageAlign = ContentAlignment.MiddleLeft;
                        Bkill.Click += DiversityWorkbench.Forms.FormFunctions.killButton_Click;
                        Bkill.Dock = System.Windows.Forms.DockStyle.Bottom;
                        Bkill.Height = 26;
                        Bkill.ForeColor = System.Drawing.Color.Red;
                        FormServerActivity.Controls.Add(Bkill);

                        DataGridServerActivity = new System.Windows.Forms.DataGridView();
                        GetServerActivity();
                        DataGridServerActivity.DataSource = DtServerActivity;
                        DataGridServerActivity.ReadOnly = true;
                        DataGridServerActivity.AllowUserToAddRows = false;
                        DataGridServerActivity.RowHeadersVisible = false;
                        DataGridServerActivity.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;

                        int MaxHeight = (int)System.Windows.SystemParameters.PrimaryScreenHeight - 100;
                        int MaxWidth = (int)System.Windows.SystemParameters.PrimaryScreenWidth - 100;

                        FormServerActivity.Controls.Add(DataGridServerActivity);

                        FormServerActivity.Text = "Current activity on server";
                        if (DtServerActivity != null)
                        {
                            FormServerActivity.Width = 100 * DtServerActivity.Columns.Count + 50;
                            if (FormServerActivity.Width > MaxWidth)
                                FormServerActivity.Width = MaxWidth;
                            FormServerActivity.Height = 24 * DtServerActivity.Rows.Count + 90;
                            if (FormServerActivity.Height > MaxHeight)
                                FormServerActivity.Height = MaxHeight;
                        }
                        else if (Height != null && Width != null)
                        {
                            FormServerActivity.Height = (int)Height;
                            FormServerActivity.Width = (int)Width;
                        }
                        FormServerActivity.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                        if (_ShowServerActiviyForPostgres)
                            FormServerActivity.Text = "Activ processes on postgres server " + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + " that are older then " + _PostgresProcessActivityAge.ToString() + " min.";
                        else
                            FormServerActivity.Text += " " + DiversityWorkbench.Settings.DatabaseServer;

                        DataGridServerActivity.Dock = System.Windows.Forms.DockStyle.Fill;
                        DataGridServerActivity.AutoResizeColumns(System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
                        GetServerActivity();
                        FormServerActivity.ShowDialog();
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private static System.Data.DataTable DtServerActivity;

        private static System.Windows.Forms.Form FormServerActivity;

        private static System.Windows.Forms.DataGridView DataGridServerActivity;

        private static System.Data.DataTable GetServerActivity()
        {
            if (_ShowServerActiviyForPostgres)
                return GetServerActivityForPostgres();

            string SQL = "sp_who2";
            if (DtServerActivity == null)
                DtServerActivity = new System.Data.DataTable();
            DtServerActivity.Clear();
            //System.Data.DataTable dt = new System.Data.DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(DtServerActivity);
            foreach (System.Data.DataRow R in DtServerActivity.Rows)
            {
                if (R["Hostname"].ToString().Replace(".", "").Trim().Length == 0
                    || R["DBName"].ToString() == "msdb")
                {
                    R.Delete();
                }
            }
            DtServerActivity.AcceptChanges();
            for (int iLine = 0; iLine < DtServerActivity.Rows.Count; iLine++)
            {
                if (DtServerActivity.Rows[iLine]["BlkBy"].ToString().Replace(".", "").Trim().Length > 0)
                {
                    for (int iCol = 0; iCol < DataGridServerActivity.Columns.Count; iCol++)
                    {
                        DataGridServerActivity.Rows[iLine].Cells[iCol].Style.BackColor = System.Drawing.Color.Pink;
                    }
                }
                else if (DtServerActivity.Rows[iLine]["Login"].ToString() == DiversityWorkbench.Settings.DatabaseUser)
                {
                    for (int iCol = 0; iCol < DataGridServerActivity.Columns.Count; iCol++)
                    {
                        DataGridServerActivity.Rows[iLine].Cells[iCol].Style.BackColor = System.Drawing.Color.LightYellow;
                    }
                }
                if (DtServerActivity.Rows[iLine]["Status"].ToString().Replace(".", "").Trim().ToLower() == "sleeping")
                {
                    for (int iCol = 0; iCol < DataGridServerActivity.Columns.Count; iCol++)
                    {
                        DataGridServerActivity.Rows[iLine].Cells[iCol].Style.ForeColor = System.Drawing.Color.Gray;
                    }
                }

            }
            return DtServerActivity;
        }

        private static void getServerActivity_Click(object sender, EventArgs e)
        {
            GetServerActivity();
        }

        private static void killButton_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormRestricted FR = new DiversityWorkbench.Forms.FormRestricted();
            if (FR.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                System.Windows.Forms.MessageBox.Show("Obviously not qualified ...");
                return;
            }

            System.Data.DataRow R = DtServerActivity.Rows[DataGridServerActivity.SelectedCells[0].RowIndex];
            if (_ShowServerActiviyForPostgres)
            {
                int pid;
                if (int.TryParse(R["pid"].ToString(), out pid))
                {
                    string Error = "";
                    if (!StopPostgresProcess(pid, ref Error))
                        System.Windows.Forms.MessageBox.Show(Error, "Failed", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                return;
            }

            string SQL = "kill " + R["SPID"].ToString();
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
            if (Message.Length == 0)
                GetServerActivity();
            else
                System.Windows.Forms.MessageBox.Show(Message);
        }

        #region Postgres

        /// <summary>
        /// listing zombie processes that are older then 30 min
        /// </summary>
        /// <returns>table with processes</returns>
        private static System.Data.DataTable GetServerActivityForPostgres()
        {
            string SQL = "select pid, age(query_start, clock_timestamp()), usename, client_addr, query " +
                "FROM pg_stat_activity " +
                "WHERE query != '<IDLE>' AND query NOT ILIKE '%pg_stat_activity%' " +
                "AND query_start<NOW() -INTERVAL '" + _PostgresProcessActivityAge.ToString() + " minute'";
            DtServerActivity = new System.Data.DataTable();
            string Message = "";
            DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref DtServerActivity, ref Message);
            if (Message.Length > 0)
                System.Windows.Forms.MessageBox.Show(Message, "Failed", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            return DtServerActivity;
        }

        /// <summary>
        /// Stopping a process
        /// </summary>
        /// <param name="pid">ID of the process that should be stopped</param>
        /// <returns>If the stop was successful</returns>
        private static bool StopPostgresProcess(int pid, ref string Message)
        {
            string SQL = "select pg_terminate_backend(" + pid.ToString() + ")";
            bool OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
            return OK;
        }


        #endregion

        #endregion

        #region Login

        public static bool CheckPasswordMatch(string Pw1, string Pw2)
        {
            bool OK = false;
            if (Pw1 == Pw2 && Pw2.Length > 0)
            {
                OK = true;
            }
            return OK;
        }

        public static bool CheckIfEntryOK(string Entry, bool IsPW = true)
        {
            bool OK = true;
            string Forbidden = "";
            string EntryType = "password";
            if (!IsPW) EntryType = "login";
            if (!NoForbiddenCharacter(Entry, ref Forbidden))
            {
                string Message = "Invalid " + EntryType + ":\r\nThe character\r\n   \"" + Forbidden + "\"\r\nis not allowed";
                System.Windows.Forms.MessageBox.Show(Message, "Illegal " + EntryType, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                OK = false;
            }
            if (IsPW && Entry.Length < 8)
                OK = false;
            return OK;
        }

        private static bool NoForbiddenCharacter(string Text, ref string Forbidden)
        {
            bool OK = true;
            foreach (char c in Text)
            {
                if (ForbiddenCharacters.Contains(c))
                {
                    Forbidden = c.ToString();
                    OK = false;
                    break;
                }
            }
            return OK;
        }

        //[] {}() , ; ? * ! @
        private static System.Collections.Generic.List<char> _ForbiddenCharacters;
        private static System.Collections.Generic.List<char> ForbiddenCharacters
        {
            get
            {
                if (_ForbiddenCharacters == null)
                {
                    _ForbiddenCharacters = new List<char>();
                    _ForbiddenCharacters.Add(' ');
                    _ForbiddenCharacters.Add('\'');
                    _ForbiddenCharacters.Add('"');
                    _ForbiddenCharacters.Add('=');
                    _ForbiddenCharacters.Add('[');
                    _ForbiddenCharacters.Add(']');
                    _ForbiddenCharacters.Add('{');
                    _ForbiddenCharacters.Add('}');
                    _ForbiddenCharacters.Add('(');
                    _ForbiddenCharacters.Add(')');
                    _ForbiddenCharacters.Add(',');
                    _ForbiddenCharacters.Add(';');
                    _ForbiddenCharacters.Add('?');
                    _ForbiddenCharacters.Add('*');
                    _ForbiddenCharacters.Add('!');
                    _ForbiddenCharacters.Add('@');
                    _ForbiddenCharacters.Add('$');
                }
                return _ForbiddenCharacters;
            }
        }


        #endregion

        #region Permissions

        public void setControlsAccordingToPermissions(System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<DatabaseGrant, System.Collections.Generic.List<System.Windows.Forms.Control>>> DataControlDictionary)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<DatabaseGrant, System.Collections.Generic.List<System.Windows.Forms.Control>>>
                KV_Table in DataControlDictionary)
            {
                bool OK;
                string TableName = KV_Table.Key;
                foreach (System.Collections.Generic.KeyValuePair<DatabaseGrant, System.Collections.Generic.List<System.Windows.Forms.Control>>
                    KV_Controls in KV_Table.Value)
                {
                    OK = this.getObjectPermissions(TableName, KV_Controls.Key.ToString());
                    foreach (System.Windows.Forms.Control C in KV_Controls.Value)
                    {
                        this.setDataControlEnabled(C, OK);
                    }
                }
            }
        }

        public void setObjectsAccordingToPermission(
            System.Collections.Generic.List<System.Windows.Forms.Control> ControlList,
            System.Collections.Generic.List<System.Windows.Forms.ToolStripItem> ToolstripItemList,
            string Table,
            DiversityWorkbench.Forms.FormFunctions.DatabaseGrant Grant)
        {
            bool OK = this.getObjectPermissions(Table, Grant.ToString());
            foreach (System.Windows.Forms.ToolStripItem T in ToolstripItemList)
                T.Visible = OK;
            foreach (System.Windows.Forms.Control C in ControlList)
            {
                if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                {
                    System.Windows.Forms.TextBox tb = (System.Windows.Forms.TextBox)C;
                    tb.ReadOnly = !OK;
                }
                else
                {
                    C.Enabled = OK;
                }
            }
        }

        public void setObjectsAccordingToOldClient(
            System.Collections.Generic.List<System.Windows.Forms.Control> ControlList,
            System.Collections.Generic.List<System.Windows.Forms.ToolStripItem> ToolstripItemList)
        {
            bool OK = false;
            foreach (System.Windows.Forms.ToolStripItem T in ToolstripItemList)
                T.Visible = OK;
            foreach (System.Windows.Forms.Control C in ControlList)
            {
                if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                {
                    System.Windows.Forms.TextBox tb = (System.Windows.Forms.TextBox)C;
                    tb.ReadOnly = !OK;
                }
                else
                {
                    C.Enabled = OK;
                }
            }
        }

        public void setObjectsAccordingToPermission(
            System.Collections.Generic.List<System.Object> ObjectList,
            string Table,
            DiversityWorkbench.Forms.FormFunctions.DatabaseGrant Grant)
        {

            try
            {
                bool OK = this.getObjectPermissions(Table, Grant.ToString());
                foreach (System.Object O in ObjectList)
                {
                    if (O.GetType() == typeof(System.Windows.Forms.TextBox))
                    {
                        System.Windows.Forms.TextBox tb = (System.Windows.Forms.TextBox)O;
                        tb.ReadOnly = !OK;
                    }
                    else if (O.GetType() == typeof(System.Windows.Forms.ToolStripItem)
                        || O.GetType() == typeof(System.Windows.Forms.ToolStripButton))
                    {
                        System.Windows.Forms.ToolStripItem I = (System.Windows.Forms.ToolStripItem)O;
                        I.Visible = OK;
                        I.Enabled = OK;
                    }
                    else if (O.GetType() == typeof(System.Windows.Forms.Control))
                    {
                        System.Windows.Forms.Control C = (System.Windows.Forms.Control)O;
                        C.Enabled = OK;
                    }
                    else if (O.GetType() == typeof(System.Windows.Forms.TableLayoutPanel))
                    {
                        System.Windows.Forms.TableLayoutPanel C = (System.Windows.Forms.TableLayoutPanel)O;
                        C.Enabled = OK;
                    }
                    else
                    {
                        try
                        {
                            System.Windows.Forms.Control C = (System.Windows.Forms.Control)O;
                            C.Enabled = OK;
                        }
                        catch { }
                    }
                }

            }
            catch { throw; }
        }

        public void setDataControlEnabled(System.Windows.Forms.Control Cont, bool Enabled)
        {
            foreach (System.Windows.Forms.Control C in Cont.Controls)
            {
                if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                {
                    try
                    {
                        System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)C;
                        T.ReadOnly = !Enabled;
                    }
                    catch { }
                }
                if (C.GetType() == typeof(System.Windows.Forms.MaskedTextBox))
                {
                    try
                    {
                        System.Windows.Forms.MaskedTextBox T = (System.Windows.Forms.MaskedTextBox)C;
                        T.ReadOnly = !Enabled;
                    }
                    catch { }
                }
                else if (C.GetType() == typeof(System.Windows.Forms.CheckBox)
                    || C.GetType() == typeof(System.Windows.Forms.DateTimePicker)
                    || C.GetType() == typeof(System.Windows.Forms.NumericUpDown)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlDatePanel)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)
                    || C.GetType() == typeof(System.Windows.Forms.ToolStrip)
                    || C.GetType() == typeof(System.Windows.Forms.ListBox)
                    || C.GetType() == typeof(System.Windows.Forms.ComboBox)
                    || C.GetType() == typeof(System.Windows.Forms.Button))
                {
                    try
                    {
                        C.Enabled = Enabled;
                    }
                    catch { }
                }
                else if (C.GetType() == typeof(System.Windows.Forms.GroupBox)
                    || C.GetType() == typeof(System.Windows.Forms.Panel)
                    || C.GetType() == typeof(System.Windows.Forms.TabControl)
                    || C.GetType() == typeof(System.Windows.Forms.TableLayoutPanel)
                    || C.GetType() == typeof(System.Windows.Forms.SplitContainer)
                    || C.GetType() == typeof(System.Windows.Forms.SplitterPanel)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlLocalList)
                    //|| C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)
                    //|| C.GetType() == typeof(DiversityWorkbench.UserControlDatePanel)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlURI)
                    || C.GetType() == typeof(System.Windows.Forms.TabPage))
                    this.setDataControlEnabled(C, Enabled);
            }
        }


        public bool getObjectPermissions(string ObjectName, DiversityWorkbench.Forms.FormFunctions.DatabaseGrant DatabaseGrant)
        {
            return (this.getObjectPermissions(ObjectName, DatabaseGrant.ToString()));
        }

        public bool getObjectPermissions(string ObjectName, string Permission)
        {
            /*
            1 SELECT ALL
            2 UPDATE ALL
            4 REFERENCES ALL
            8 INSERT
            16 DELETE
            32 EXECUTE (nur Prozeduren)
            4096 SELECT ANY (mindestens eine Spalte)
            8192 UPDATE ANY
            */

            bool Permit = false;
            Microsoft.Data.SqlClient.SqlConnection con = null;
            try
            {
                string SQL = "";
                if (ObjectName.Contains(".dbo."))
                {
                    SQL = "USE " + ObjectName.Split(new char[] { '.' })[0] + ";";
                    ObjectName = ObjectName.Split(new char[] { '.' })[2];
                }
                SQL += "IF PERMISSIONS(OBJECT_ID('" + ObjectName + "'))&";
                Permission = Permission.ToUpper();
                switch (Permission)
                {
                    case "SELECT":
                        SQL += "1=1";
                        break;
                    case "INSERT":
                        SQL += "8=8";
                        break;
                    case "UPDATE":
                        SQL += "2=2";
                        break;
                    case "DELETE":
                        SQL += "16=16";
                        break;
                    case "EXECUTE":
                        SQL += "32=32";
                        break;
                }
                SQL += " SELECT 'True' ELSE SELECT 'False'";

                con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                C.CommandText = SQL;
                con.Open(); 
                string testF = C.ExecuteScalar()?.ToString() ?? string.Empty;
                if (testF == string.Empty)
                {
                    con.Close();
                    return false;
                }

                Permit = System.Boolean.Parse(testF);
            }
            catch (System.Exception ex)
            {
                Permit = false;
            }
            finally
            {
                con.Close();
            }
            return Permit;
        }

        public bool getObjectPermissions(string ObjectName, string ColumnName, string Permission)
        {
            /*
            1 SELECT 
            2 UPDATE 
            4 REFERENCES 
            */

            bool Permit = false;
            string SQL = "";
            if (ObjectName.Contains(".dbo."))
            {
                SQL = "USE " + ObjectName.Split(new char[] { '.' })[0] + ";";
                ObjectName = ObjectName.Split(new char[] { '.' })[2];
            }
            SQL += "IF PERMISSIONS(OBJECT_ID('" + ObjectName + "'), '" + ColumnName + "')&";
            Permission = Permission.ToUpper();
            switch (Permission)
            {
                case "SELECT":
                    SQL += "1=1";
                    break;
                case "UPDATE":
                    SQL += "2=2";
                    break;
            }
            SQL += " SELECT 'True' ELSE SELECT 'False'";

            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            C.CommandText = SQL;
            try
            {
                con.Open();
                
				string testF = C.ExecuteScalar()?.ToString() ?? string.Empty;
                if (testF == string.Empty)
                {
                    con.Close();
                    return false;
                }
				Permit = System.Boolean.Parse(testF);
            }
            finally
            {
                con.Close();
            }
            return Permit;
        }

        public static bool Permissions(string ObjectName, string Permission)
        {
            /*
            1 SELECT ALL
            2 UPDATE ALL
            4 REFERENCES ALL
            8 INSERT
            16 DELETE
            32 EXECUTE (nur Prozeduren)
            4096 SELECT ANY (mindestens eine Spalte)
            8192 UPDATE ANY
            */

            bool Permit = false;
            string SQL = "";
            if (ObjectName.Contains(".dbo."))
            {
                SQL = "USE " + ObjectName.Split(new char[] { '.' })[0] + ";";
                ObjectName = ObjectName.Split(new char[] { '.' })[2];
            }
            SQL += "IF PERMISSIONS(OBJECT_ID('" + ObjectName + "'))&";
            Permission = Permission.ToUpper();
            switch (Permission)
            {
                case "SELECT":
                    SQL += "1=1";
                    break;
                case "INSERT":
                    SQL += "8=8";
                    break;
                case "UPDATE":
                    SQL += "2=2";
                    break;
                case "DELETE":
                    SQL += "16=16";
                    break;
                case "EXECUTE":
                    SQL += "32=32";
                    break;
            }
            SQL += " SELECT 'True' ELSE SELECT 'False'";

            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            C.CommandText = SQL;
            try
            {
                con.Open();
                string testF = C.ExecuteScalar()?.ToString() ?? string.Empty;
                if (testF == string.Empty)
                {
                    con.Close();
                    return false;
                }
				Permit = System.Boolean.Parse(testF);
            }
            finally
            {
                con.Close();
            }
            return Permit;
        }

        public static bool Permissions(string ObjectName, string Permission, Microsoft.Data.SqlClient.SqlConnection Connection)
        {
            /*
            1 SELECT ALL
            2 UPDATE ALL
            4 REFERENCES ALL
            8 INSERT
            16 DELETE
            32 EXECUTE (nur Prozeduren)
            4096 SELECT ANY (mindestens eine Spalte)
            8192 UPDATE ANY
            */

            bool Permit = false;
            string SQL = "";
            if (ObjectName.Contains(".dbo."))
            {
                SQL = "USE " + ObjectName.Split(new char[] { '.' })[0] + ";";
                ObjectName = ObjectName.Split(new char[] { '.' })[2];
            }
            SQL += "IF PERMISSIONS(OBJECT_ID('" + ObjectName + "'))&";
            Permission = Permission.ToUpper();
            switch (Permission)
            {
                case "SELECT":
                    SQL += "1=1";
                    break;
                case "INSERT":
                    SQL += "8=8";
                    break;
                case "UPDATE":
                    SQL += "2=2";
                    break;
                case "DELETE":
                    SQL += "16=16";
                    break;
                case "EXECUTE":
                    SQL += "32=32";
                    break;
            }
            SQL += " SELECT 'True' ELSE SELECT 'False'";

            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, Connection);
            C.CommandText = SQL;
            try
            {
                if (Connection.State.ToString() == "Closed")
                    Connection.Open();
                string testF = C.ExecuteScalar()?.ToString() ?? string.Empty;
                if (testF == string.Empty)
                {
                    Connection.Close();
                    return false;
                }
				Permit = System.Boolean.Parse(testF);
            }
            catch (System.Exception ex)
            {
            }
            finally
            {
            }
            return Permit;
        }

        public enum Permission { SELECT, INSERT, UPDATE, DELETE }
        public static bool Permissions(string ObjectName, Permission Permission)
        {
            bool OK = Permissions(ObjectName, Permission.ToString());
            return OK;
        }

        public static System.Collections.Generic.Dictionary<Permission, bool> TablePermissions(string TableName)
        {
            System.Collections.Generic.Dictionary<Permission, bool> Permissions = new Dictionary<Permission, bool>();
            Permissions.Add(Permission.SELECT, DiversityWorkbench.Forms.FormFunctions.Permissions(TableName, "SELECT"));
            Permissions.Add(Permission.INSERT, DiversityWorkbench.Forms.FormFunctions.Permissions(TableName, "INSERT"));
            Permissions.Add(Permission.UPDATE, DiversityWorkbench.Forms.FormFunctions.Permissions(TableName, "UPDATE"));
            Permissions.Add(Permission.DELETE, DiversityWorkbench.Forms.FormFunctions.Permissions(TableName, "DELETE"));
            return Permissions;
        }

        public static System.Collections.Generic.Dictionary<Permission, bool> TablePermissionsReadOnly()
        {
            System.Collections.Generic.Dictionary<Permission, bool> Permissions = new Dictionary<Permission, bool>();
            Permissions.Add(Permission.SELECT, true);
            Permissions.Add(Permission.INSERT, false);
            Permissions.Add(Permission.UPDATE, false);
            Permissions.Add(Permission.DELETE, false);
            return Permissions;
        }


        #endregion

        #region Description

        public virtual void setDescriptions()
        {
            foreach (System.Windows.Forms.Control C in this.Form.Controls)
            {
                if (C.GetType() == typeof(System.Windows.Forms.GroupBox)
                    || C.GetType() == typeof(System.Windows.Forms.Panel)
                    || C.GetType() == typeof(System.Windows.Forms.TabControl)
                    || C.GetType() == typeof(System.Windows.Forms.TabPage)
                    || C.GetType() == typeof(System.Windows.Forms.TableLayoutPanel)
                    || C.GetType() == typeof(System.Windows.Forms.SplitterPanel)
                    || C.GetType() == typeof(System.Windows.Forms.SplitContainer))
                    this.setDescriptions(C);
            }
        }

        public void setDescriptions(System.Windows.Forms.Control Cont)
        {
            foreach (System.Windows.Forms.Control C in Cont.Controls)
            {
                object[] ar = new Object[9] { "", "", "", "", "", "", "", "", "" };
                string Table = "";
                string Column = "";
                string Database = "";
                if (C.GetType() == typeof(System.Windows.Forms.CheckBox)
                    || C.GetType() == typeof(System.Windows.Forms.TextBox)
                    || C.GetType() == typeof(System.Windows.Forms.MaskedTextBox)
                    || C.GetType() == typeof(System.Windows.Forms.ComboBox)
                    || C.GetType() == typeof(System.Windows.Forms.DateTimePicker))
                {
                    try
                    {
                        C.DataBindings.CopyTo(ar, 0);
                        if (ar[0].ToString() != "")
                        {
                            System.Windows.Forms.BindingMemberInfo bmi = C.DataBindings[0].BindingMemberInfo;
                            Column = bmi.BindingField;
                            Table = bmi.BindingPath;
                            if (Table.Length == 0)
                            {
                                System.Windows.Forms.Binding B = (System.Windows.Forms.Binding)ar[0];
                                if (B.DataSource.GetType() == typeof(System.Windows.Forms.BindingSource))
                                {
                                    System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)B.DataSource;
                                    Table = BS.DataMember.ToString();
                                    Column = B.BindingMemberInfo.BindingField;
                                    Database = DiversityWorkbench.Settings.DatabaseName;
                                }
                                else if (B.DataSource.GetType().BaseType == typeof(System.Data.DataTable))
                                {
                                    Table = B.DataSource.ToString();
                                    Column = B.BindingMemberInfo.BindingField;
                                    Database = DiversityWorkbench.Settings.DatabaseName;
                                }
                            }
                            string ToolTipText = this.ColumnDescription(Table, Column, Database);
                            this.ToolTip.SetToolTip(C, ToolTipText);
                        }
                    }
                    catch { }
                }
                else if ((C.GetType() == typeof(System.Windows.Forms.Label) ||
                    C.GetType() == typeof(System.Windows.Forms.GroupBox) ||
                    C.GetType() == typeof(System.Windows.Forms.TabPage))
                    &&
                    C.AccessibleName != null)
                {
                    try
                    {
                        if (C.AccessibleName.IndexOf('.') > -1)
                        {
                            string[] Entity = C.AccessibleName.Split(new char[] { '.' });
                            Table = Entity[0];
                            Column = Entity[1];
                            Database = DiversityWorkbench.Settings.DatabaseName;
                            string ToolTipText = this.ColumnDescription(Table, Column, Database);
                            this.ToolTip.SetToolTip(C, ToolTipText);
                        }
                        else
                        {
                            string ToolTipText = this.TableDescription(C.AccessibleName);
                            this.ToolTip.SetToolTip(C, ToolTipText);
                        }
                    }
                    catch { }
                    if (C.GetType() == typeof(System.Windows.Forms.GroupBox)
                        || C.GetType() == typeof(System.Windows.Forms.TabPage))
                        this.setDescriptions(C);
                }
                else if (C.GetType() == typeof(System.Windows.Forms.Label) ||
                    C.GetType() == typeof(System.Windows.Forms.GroupBox) ||
                    C.GetType() == typeof(System.Windows.Forms.TabPage))
                {
                    this.setDescriptions(C);
                }
                else if (C.GetType() == typeof(System.Windows.Forms.Panel)
                    || C.GetType() == typeof(System.Windows.Forms.TabControl)
                    || C.GetType() == typeof(System.Windows.Forms.TableLayoutPanel)
                    || C.GetType() == typeof(System.Windows.Forms.SplitContainer)
                    || C.GetType() == typeof(System.Windows.Forms.SplitterPanel)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlLocalList)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlDatePanel)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlURI))
                    this.setDescriptions(C);
            }
        }

        /// <summary>
        /// the description of a table column
        /// </summary>
        /// <param name="Table">the database table</param>
        /// <param name="Column">the column</param>
        /// <param name="Database">the database</param>
        /// <returns>the description of the column</returns>
        public string ColumnDescription(string Table, string Column, string Database, string ConnectionString = "")
        {
            string Description = "";
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0) return "";
            if (Table.IndexOf("_Main20") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Main20"));
            if (Table.IndexOf("_Core") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Core"));
            if (Table.IndexOf("_IsAccepted") > 0)
                Table = Table.Substring(0, Table.IndexOf("_IsAccepted"));
            Description = getDescriptionCache(Table, Column);
            if (Description == null || Description.Length == 0)
            {
                if (ConnectionString.Length == 0)
                    ConnectionString = DiversityWorkbench.Settings.ConnectionString;
                Microsoft.Data.SqlClient.SqlConnection conn = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
                string SQL = "SELECT max(CONVERT(varchar(300), [value])) " +
                    " FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', '" + Table +
                    "', 'column', '" + Column + "') WHERE name =  'MS_Description'--[::fn_listextendedproperty_1]";
                Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, conn);
                try
                {
                    conn.Open();
                    Description = Com.ExecuteScalar().ToString();
                }
                catch
                {
                }
                finally
                {
                    conn.Close();
                }
            }
            return Description;
        }

        public string ColumnDescription(string Table, string Column)
        {
            string Description = "";
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0) return "";
            if (Table.IndexOf("_Main20") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Main20"));
            if (Table.IndexOf("_Core") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Core"));
            if (Table.IndexOf("_IsAccepted") > 0)
                Table = Table.Substring(0, Table.IndexOf("_IsAccepted"));
            Description = getDescriptionCache(Table, Column);
            if (Description == null || Description.Length == 0)
            {
                Microsoft.Data.SqlClient.SqlConnection conn = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                string SQL = "SELECT max(CONVERT(nvarchar(MAX), [value])) " +
                    " FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', '" + Table +
                    "', 'column', '" + Column + "') WHERE name =  'MS_Description'--[::fn_listextendedproperty_1]";
                Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, conn);
                try
                {
                    conn.Open();
                    Description = Com.ExecuteScalar().ToString();
                }
                catch
                {
                }
                finally
                {
                    conn.Close();
                }
            }
            return Description;
        }

        public string TableDescription(string Table, string ConnectionString = "")
        {
            string Description = "";
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0) return "";
            if (Table.IndexOf("_Main20") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Main20"));
            if (Table.IndexOf("_Core") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Core"));
            if (Table.IndexOf("_IsAccepted") > 0)
                Table = Table.Substring(0, Table.IndexOf("_IsAccepted"));
            Description = getDescriptionCache(Table, "");
            if (Description == null || Description.Length == 0)
            {
                if (ConnectionString.Length == 0)
                    ConnectionString = DiversityWorkbench.Settings.ConnectionString;
                Microsoft.Data.SqlClient.SqlConnection conn = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
                string SQL = "SELECT max(CONVERT(nvarchar(MAX), [value])) " +
                    " FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', '" + Table +
                    "', default, NULL) WHERE name =  'MS_Description'--[::fn_listextendedproperty_1]";
                Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, conn);
                try
                {
                    conn.Open();
                    Description = Com.ExecuteScalar().ToString();
                }
                catch
                {
                }
                finally
                {
                    conn.Close();
                }
            }
            return Description;
        }

        public static string getColumnDescription(string Table, string Column)
        {
            string Description = "";
            if (DiversityWorkbench.Settings.Language != "en"
                && DiversityWorkbench.Settings.Context != "General")
                Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(Table, Column, DiversityWorkbench.Settings.Context, DiversityWorkbench.Settings.Language);
            if (Description.Length > 0) return Description;
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0) return "";
            if (Table.IndexOf("_Main20") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Main20"));
            if (Table.IndexOf("_Core") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Core"));
            if (Table.IndexOf("_IsAccepted") > 0)
                Table = Table.Substring(0, Table.IndexOf("_IsAccepted"));
            Description = getDescriptionCache(Table, Column);
            if (Description == null || Description.Length == 0)
            {
                Microsoft.Data.SqlClient.SqlConnection conn = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                string SQL = "SELECT max(CONVERT(nvarchar(MAX), [value])) " +
                    " FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', '" + Table +
                    "', 'column', '" + Column + "') WHERE name =  'MS_Description'--[::fn_listextendedproperty_1]";
                Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, conn);
                try
                {
                    conn.Open();
                    Description = Com.ExecuteScalar().ToString();
                }
                catch
                {
                }
                finally
                {
                    conn.Close();
                }
            }
            return Description;
        }

        public static string getColumnDescription(string Table, bool IsView, string Column, bool IncludeBasicTable = false, string ConnectionString = "", string schema = "dbo")
        {
            string Description = "";
            if (IsView && IncludeBasicTable)
            {
                Description = getColumnDescriptionOfBase(Table, Column);
                if (Description.Length > 0)
                    return Description;
                else if (ConnectionString.Length > 0) // Markus 10.1.2023 - ConnectionString to get values from CacheDB
                {
                    Description = getColumnDescriptionOfBase(Table, Column, 3, ConnectionString);
                    if (Description.Length > 0)
                        return Description;
                }
            }
            if (DiversityWorkbench.Settings.Language != "en"
                && DiversityWorkbench.Settings.Context != "General")
                Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(Table, Column, DiversityWorkbench.Settings.Context, DiversityWorkbench.Settings.Language);
            if (Description.Length > 0)
                return Description;

            string SQL = "SELECT max(CONVERT(nvarchar(MAX), [value])) " +
                string.Format(" FROM ::fn_listextendedproperty(NULL, 'SCHEMA', '{0}', '", schema);
            if (IsView)
                SQL += "VIEW";
            else
                SQL += "TABLE";
            SQL += "', '" + Table +
                "', 'COLUMN', '" + Column + "') WHERE name =  'MS_Description'--[::fn_listextendedproperty_1]";
            Description = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (Description.Length > 0)
                return Description;

            if (DiversityWorkbench.Settings.ConnectionString.Length == 0) return "";
            if (Table.IndexOf("_Main20") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Main20"));
            if (Table.IndexOf("_Core") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Core"));
            if (Table.IndexOf("_IsAccepted") > 0)
                Table = Table.Substring(0, Table.IndexOf("_IsAccepted"));
            if (Description == null || Description.Length == 0)
            {
                if (ConnectionString.Length == 0)
                    ConnectionString = DiversityWorkbench.Settings.ConnectionString;
                Microsoft.Data.SqlClient.SqlConnection conn = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
                SQL = "SELECT max(CONVERT(nvarchar(MAX), [value])) " +
                    string.Format(" FROM ::fn_listextendedproperty(NULL, 'SCHEMA', '{0}', '", schema);
                if (IsView)
                    SQL += "VIEW";
                else
                    SQL += "TABLE";
                SQL += "', '" + Table +
                    "', 'COLUMN', '" + Column + "') WHERE name =  'MS_Description'--[::fn_listextendedproperty_1]";
                Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, conn);
                try
                {
                    conn.Open();
                    Description = Com.ExecuteScalar().ToString();
                }
                catch
                {
                }
                finally
                {
                    conn.Close();
                }
            }
            if (Description.Length == 0 && IncludeBasicTable)
                Description = getColumnDescriptionOfBase(Table, Column, 0, ConnectionString);
            return Description;
        }

        public static string getColumnDescription(string Table, bool IsView, string Column, Microsoft.Data.SqlClient.SqlConnection Connection, bool IncludeBasicTable = false, string schema = "dbo")
        {
            string Description = "";
            string SQL = "";
            if (IsView)
            {
                SQL = "SELECT max(CONVERT(nvarchar(MAX), [value])) " + string.Format(" FROM ::fn_listextendedproperty(NULL, 'SCHEMA', '{0}', '", schema);
                SQL += "VIEW', '" + Table + "', 'COLUMN', '" + Column + "') WHERE name =  'MS_Description'";
                Description = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            }
            if (Description.Length == 0 && IsView && IncludeBasicTable)
            {
                Description = getColumnDescriptionOfBase(Table, Column, Connection);// Markus 10.1.2023 - Reducing number of connections
                if (Description.Length > 0)
                    return Description;
            }
            if (DiversityWorkbench.Settings.Language != "en"
                && DiversityWorkbench.Settings.Context != "General")
                Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(Table, Column, DiversityWorkbench.Settings.Context, DiversityWorkbench.Settings.Language);
            if (Description.Length > 0)
                return Description;

            SQL = "SELECT max(CONVERT(nvarchar(MAX), [value])) " +
                string.Format(" FROM ::fn_listextendedproperty(NULL, 'SCHEMA', '{0}', '", schema);
            if (IsView)
                SQL += "VIEW";
            else
                SQL += "TABLE";
            SQL += "', '" + Table +
                "', 'COLUMN', '" + Column + "') WHERE name =  'MS_Description'--[::fn_listextendedproperty_1]";
            Description = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (Description.Length > 0)
                return Description;

            if (DiversityWorkbench.Settings.ConnectionString.Length == 0) return "";
            if (Table.IndexOf("_Main20") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Main20"));
            if (Table.IndexOf("_Core") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Core"));
            if (Table.IndexOf("_IsAccepted") > 0)
                Table = Table.Substring(0, Table.IndexOf("_IsAccepted"));
            if (Description == null || Description.Length == 0)
            {
                SQL = "SELECT max(CONVERT(nvarchar(MAX), [value])) " +
                    string.Format(" FROM ::fn_listextendedproperty(NULL, 'SCHEMA', '{0}', '", schema);
                if (IsView)
                    SQL += "VIEW";
                else
                    SQL += "TABLE";
                SQL += "', '" + Table +
                    "', 'COLUMN', '" + Column + "') WHERE name =  'MS_Description'--[::fn_listextendedproperty_1]";
                Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, Connection);
                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed)
                        Connection.Open();
                    Description = Com.ExecuteScalar().ToString();
                }
                catch
                {
                }
                finally
                {
                }
            }
            if (Description.Length == 0 && IncludeBasicTable)
                Description = getColumnDescriptionOfBase(Table, Column, Connection, 0);
            return Description;
        }

        public static string getColumnDescriptionOfBase(string Object, string Column, Microsoft.Data.SqlClient.SqlConnection Connection, int Depth = 0, string Schema = "dbo")
        {
            // search for only up to 3 levels
            if (Depth > 3)
                return "";
            Depth++;

            string Description = "";
            // Getting the basic objects the object depends upon
            string SQL = "SELECT DISTINCT d.referenced_entity_name " +
                "FROM sys.sql_expression_dependencies d, INFORMATION_SCHEMA.TABLES T " +
                "WHERE referencing_id = OBJECT_ID(N'" + Object + "') " +
                "AND d.referenced_entity_name = T.TABLE_NAME;  ";

            System.Data.DataTable dt = new System.Data.DataTable();
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, Connection);
            // Try to get a description of the column in the basic table
            foreach (System.Data.DataRow R in dt.Rows)
            {
                SQL = "SELECT max(CONVERT(nvarchar(MAX), [value])) " +
                    " FROM ::fn_listextendedproperty(NULL, 'user', '" + Schema + "', 'table', '" + R[0].ToString() +
                    "', 'column', '" + Column + "') WHERE name =  'MS_Description'--[::fn_listextendedproperty_1]";
                Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, Connection);
                try
                {
                    if (Connection.State == System.Data.ConnectionState.Closed)
                        Connection.Open();
                    Description = Com.ExecuteScalar().ToString();
                    if (Description.Length == 0 && Column.IndexOf("_") > -1)
                    {
                        Description = getColumnDescriptionOfBase(Object, Column.Replace("_", ""), Connection, Depth);
                    }
                }
                catch
                {
                }
                finally
                {
                }
                if (Description.Length > 0)
                    break;
            }
            if (Description.Length == 0 && dt.Rows.Count > 0)
            {
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescriptionOfBase(R[0].ToString(), Column, Connection, Depth);
                    if (Description.Length > 0)
                        break;
                }
            }
            // try to get values from basic functions
            if (Description.Length == 0)
            {
                if (dt.Rows.Count == 0)
                {
                    SQL = "SELECT d.referenced_entity_name, R.ROUTINE_TYPE " +
                        "FROM sys.sql_expression_dependencies d, INFORMATION_SCHEMA.ROUTINES R " +
                        "WHERE referencing_id = OBJECT_ID(N'" + Object + "')  " +
                        "AND d.referenced_entity_name = r.ROUTINE_NAME;";
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            DatabaseObjectType Type = DatabaseObjectType.FUNCTION;
                            if (R[1].ToString().ToLower() != "function")
                                Type = DatabaseObjectType.PROCEDURE;
                            Description = DiversityWorkbench.Forms.FormFunctions.getDescription(Type, R[0].ToString(), Column);
                            if (Description.Length > 0)
                                break;
                        }
                        if (Description.Length == 0)
                        {
                            foreach (System.Data.DataRow R in dt.Rows)
                            {
                                try
                                {
                                    Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescriptionOfBase(R[0].ToString(), Column, Depth);
                                    if (Description.Length > 0)
                                        break;
                                }
                                catch (System.Exception ex)
                                {
                                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                }
                            }
                        }
                    }
                }
            }
            return Description;
        }


        /// <summary>
        /// Get a description of a basic object, e.g. if a column in a view has no description and the description of the table should be shown instead
        /// </summary>
        /// <param name="Object">The name of the object, e.g. a view</param>
        /// <param name="Column">The name of the column</param>
        /// <returns></returns>
        public static string getColumnDescriptionOfBase(string Object, string Column, int Depth = 0, string ConnectionString = "", string Schema = "dbo")
        {
            // search for only up to 3 levels
            if (Depth > 3)
                return "";
            Depth++;

            string Description = "";
            // Getting the basic objects the object depends upon
            string SQL = "SELECT d.referenced_entity_name " +
                "FROM sys.sql_expression_dependencies d, INFORMATION_SCHEMA.TABLES T " +
                "WHERE referencing_id = OBJECT_ID(N'" + Object + "') " +
                "AND d.referenced_entity_name = T.TABLE_NAME;  ";

            /// Markus 23.1.2023: Reducing number of connections
            System.Data.DataTable dt = DataTable(SQL, ConnectionString); // new System.Data.DataTable(); Markus
            //string Message = "";
            //DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
            // Try to get a description of the column in the basic table
            if (ConnectionString.Length == 0)
                ConnectionString = DiversityWorkbench.Settings.ConnectionString;
            else if (dt.Rows.Count == 0)
            {
                /// Markus 23.1.2023: Reducing number of connections
                //Microsoft.Data.SqlClient.SqlConnection sqlConnection = Connection(ConnectionString);// new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ConnectionString);
            }
            /// Markus 23.1.2023: Reducing number of connections
            using (Microsoft.Data.SqlClient.SqlConnection conn = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString))// new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            {
                conn.Open();
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    SQL = "SELECT max(CONVERT(nvarchar(MAX), [value])) " +
                        " FROM ::fn_listextendedproperty(NULL, 'user', '" + Schema + "', 'table', '" + R[0].ToString() +
                        "', 'column', '" + Column + "') WHERE name =  'MS_Description'--[::fn_listextendedproperty_1]";
                    Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, conn);
                    try
                    {
                        Description = Com.ExecuteScalar().ToString();
                    }
                    catch
                    {
                    }
                    finally
                    {
                    }
                    if (Description.Length > 0)
                        break;
                }
                conn.Close();
            }
            if (Description.Length == 0 && dt.Rows.Count > 0)
            {
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescriptionOfBase(R[0].ToString(), Column, Depth, ConnectionString);
                    if (Description.Length > 0)
                        break;
                }
            }
            // try to get values from basic functions
            if (Description.Length == 0)
            {
                if (dt.Rows.Count == 0)
                {
                    SQL = "SELECT DISTINCT d.referenced_entity_name, R.ROUTINE_TYPE " +
                        "FROM sys.sql_expression_dependencies d, INFORMATION_SCHEMA.ROUTINES R " +
                        "WHERE referencing_id = OBJECT_ID(N'" + Object + "')  " +
                        "AND d.referenced_entity_name = r.ROUTINE_NAME;";
                    /// Markus 23.1.2023: Reducing number of connections
                    dt = DataTable(SQL);
                    //DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            DatabaseObjectType Type = DatabaseObjectType.FUNCTION;
                            if (R[1].ToString().ToLower() != "function")
                                Type = DatabaseObjectType.PROCEDURE;
                            Description = DiversityWorkbench.Forms.FormFunctions.getDescription(Type, R[0].ToString(), Column);
                            if (Description.Length > 0)
                                break;
                        }
                        if (Description.Length == 0)
                        {
                            foreach (System.Data.DataRow R in dt.Rows)
                            {
                                try
                                {
                                    Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescriptionOfBase(R[0].ToString(), Column, Depth);
                                    if (Description.Length > 0)
                                        break;
                                }
                                catch (System.Exception ex)
                                {
                                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                }
                            }
                        }
                    }
                }
            }
            return Description;
        }

        public enum DatabaseObjectType { TABLE, VIEW, FUNCTION, PROCEDURE, USER }

        public static string getDescription(DatabaseObjectType Type, string Object, string Column, int? Depth = null, int MaxDepth = 3)
        {
            string Description = "";
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0) return "";
            if (Object.IndexOf("_Main20") > 0)
                Object = Object.Substring(0, Object.IndexOf("_Main20"));
            if (Object.IndexOf("_Core") > 0)
                Object = Object.Substring(0, Object.IndexOf("_Core"));
            if (Object.IndexOf("_IsAccepted") > 0)
                Object = Object.Substring(0, Object.IndexOf("_IsAccepted"));
            if (Description == null || Description.Length == 0)
            {
                /// Markus 23.1.2023: Reducing number of connections
                //Microsoft.Data.SqlClient.SqlConnection conn = Connection(); // new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                string SQL = "SELECT max(CONVERT(nvarchar(MAX), [value])) " +
                    " FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', '";
                SQL += Type.ToString();
                SQL += "', '" + Object + "', ";
                if (Column.StartsWith("@"))
                    SQL += "'PARAMETER'";
                else if (Column.Length == 0)
                    SQL += "default";
                else
                    SQL += "'COLUMN'";
                SQL += ", ";
                if (Column.Length == 0)
                    SQL += "NULL";
                else
                    SQL += "'" + Column + "'";
                SQL += ") WHERE name =  'MS_Description'";
                if (Type == DatabaseObjectType.USER)
                {
                    SQL = "SELECT max(CONVERT(nvarchar(MAX), [value]))  " +
                        "FROM ::fn_listextendedproperty(NULL, 'USER', '" + Object + "', NULL, NULL, NULL, NULL)  " +
                        "WHERE name =  'MS_Description'";
                }
                try
                {
                    // Markus 30.1.2023: Bugfix opening open connection
                    //if (conn.State == System.Data.ConnectionState.Closed)
                    //conn.Open();
                    using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString))
                    {
                        con.Open();
                        Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, con); // conn);
                        Description = Com.ExecuteScalar().ToString();
                        if (Description.Length == 0 && Type == DatabaseObjectType.FUNCTION)
                        {
                            System.Collections.Generic.Dictionary<string, string> BaseObject = DiversityWorkbench.Data.Routine.DependentOn(Object);
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in BaseObject)
                            {
                                DatabaseObjectType t = DatabaseObjectType.TABLE;
                                switch (KV.Value)
                                {
                                    case "PRIMARY_KEY_CONSTRAINT": break;
                                    case "INTERNAL_TABLE": break;
                                    case "DEFAULT_CONSTRAINT": break;
                                    case "SQL_TRIGGER": break;
                                    case "VIEW":
                                        t = DatabaseObjectType.VIEW;
                                        break;
                                    case "SERVICE_QUEUE": break;
                                    case "SYSTEM_TABLE": break;
                                    case "SQL_TABLE_VALUED_FUNCTION":
                                        t = DatabaseObjectType.FUNCTION;
                                        break;
                                    case "USER_TABLE": break;
                                    case "SQL_STORED_PROCEDURE":
                                        t = DatabaseObjectType.PROCEDURE;
                                        break;
                                    case "RULE": break;
                                    case "CHECK_CONSTRAINT": break;
                                    case "FOREIGN_KEY_CONSTRAINT": break;
                                    case "SQL_SCALAR_FUNCTION":
                                        t = DatabaseObjectType.FUNCTION;
                                        break;
                                }
                                if (Depth != null)
                                {
                                    int depth = (int)Depth + 1;
                                    if (depth < MaxDepth)
                                        Description = getDescription(t, KV.Key, Column.Replace("@", ""), depth, MaxDepth);
                                    if (Description.Length == 0 && Column.IndexOf("_") > -1)
                                    {
                                        Description = getDescription(t, KV.Key, Column.Replace("_", ""), depth, MaxDepth);
                                    }
                                }
                                else
                                    Description = getDescription(t, KV.Key, Column.Replace("@", ""), 1);
                                if (Description.Length > 0)
                                    break;
                            }
                        }
                        con.Close();
                    }
                }
                catch
                {
                }
                finally
                {
                    //conn.Close();
                }
            }
            return Description;
        }

        public static string getColumnDescription(string Table, string Column, bool forSQL)
        {
            string Description = "";
            if (DiversityWorkbench.Settings.Language != "en"
                && DiversityWorkbench.Settings.Context != "General")
                Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(Table, Column, DiversityWorkbench.Settings.Context, DiversityWorkbench.Settings.Language);
            if (Description.Length > 0) return Description;
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0) return "";
            if (Table.IndexOf("_Main20") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Main20"));
            if (Table.IndexOf("_Core") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Core"));
            if (Table.IndexOf("_IsAccepted") > 0)
                Table = Table.Substring(0, Table.IndexOf("_IsAccepted"));
            if (Description == null || Description.Length == 0)
            {
                Microsoft.Data.SqlClient.SqlConnection conn = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                string SQL = "SELECT max(CONVERT(nvarchar(MAX), [value])) " +
                    " FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', '" + Table +
                    "', 'column', '" + Column + "') WHERE name =  'MS_Description'--[::fn_listextendedproperty_1]";
                Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, conn);
                try
                {
                    conn.Open();
                    Description = Com.ExecuteScalar().ToString();
                }
                catch
                {
                }
                finally
                {
                    conn.Close();
                }
            }
            if (forSQL)
                Description = Description.Replace("'", "' + CHAR(39) + '");
            return Description;
        }

        public static string getColumnDescription(string Table, string Column, string Context, string Language)
        {
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0) return "";
            string Description = "";
            if (Description == null || Description.Length == 0)
            {
                Microsoft.Data.SqlClient.SqlConnection conn = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                string SQL = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES T WHERE T.TABLE_NAME = 'Entity'";
                Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, conn);
                try
                {
                    conn.Open();
                    int i = 0;
                    if (int.TryParse(Com.ExecuteScalar().ToString(), out i))
                    {
                        if (i == 1)
                        {
                            Com.CommandText = "select Description from [dbo].[EntityInformation_2] ('" + Table + "." + Column + "', '" + Language + "', '" + Context + "')";
                            Description = Com.ExecuteScalar().ToString();
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    conn.Close();
                }
            }
            return Description;
        }

        public static string getTableDescription(string Table)
        {
            string Description = "";
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0) return "";
            if (Table.IndexOf("_Main20") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Main20"));
            if (Table.IndexOf("_Core") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Core"));
            if (Table.IndexOf("_IsAccepted") > 0)
                Table = Table.Substring(0, Table.IndexOf("_IsAccepted"));
            if (Description == null || Description.Length == 0)
            {
                Microsoft.Data.SqlClient.SqlConnection conn = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                string SQL = "SELECT max(CONVERT(nvarchar(MAX), [value])) " +
                    " FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', '" + Table +
                    "', default, NULL) WHERE name =  'MS_Description'--[::fn_listextendedproperty_1]";
                Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, conn);
                try
                {
                    conn.Open();
                    Description = Com.ExecuteScalar().ToString();
                }
                catch
                {
                }
                finally
                {
                    conn.Close();
                }
            }
            return Description;
        }

        public static string getDescription(string DatabaseObject, string ObjectType, string Column, string ConnectionString = "", string Schema = "dbo")
        {
            string Description = "";
            string SQL = "SELECT max(CONVERT(nvarchar(MAX), [value])) " +
                " FROM ::fn_listextendedproperty(NULL, 'SCHEMA', '" + Schema + "', '" + ObjectType + "', '" + DatabaseObject + "', ";
            if (Column.Length > 0)
                SQL += "'COLUMN', '" + Column + "'";
            else
                SQL += " default, NULL";
            SQL += ") WHERE name =  'MS_Description'--[::fn_listextendedproperty_1]";
            if (ConnectionString.Length == 0)
                ConnectionString = DiversityWorkbench.Settings.ConnectionString;
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                Description = C.ExecuteScalar()?.ToString();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                return null;
            }
            finally
            {
                con.Close();
            }
            return Description;
        }

        public static string getDescription(string DatabaseObject, string ObjectType, string Column, Microsoft.Data.SqlClient.SqlConnection Connection, string Schema = "dbo")
        {
            string Description = "";
            string SQL = "SELECT max(CONVERT(nvarchar(MAX), [value])) " +
                " FROM ::fn_listextendedproperty(NULL, 'SCHEMA', '" + Schema + "', '";
            if (ObjectType.ToUpper() == "TRIGGER")
                SQL += "TABLE";
            else
                SQL += ObjectType;
            SQL += "', '" + DatabaseObject + "', ";
            if (Column.Length > 0)
            {
                if (ObjectType.ToUpper() == "TRIGGER")
                    SQL += "'" + ObjectType + "', '" + Column + "'";
                else
                    SQL += "'COLUMN', '" + Column + "'";
            }
            else
                SQL += " default, NULL";
            SQL += ") WHERE name =  'MS_Description'--[::fn_listextendedproperty_1]";
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, Connection);
            try
            {
                if (Connection.State == System.Data.ConnectionState.Closed)
                    Connection.Open();
                Description = C.ExecuteScalar()?.ToString();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                return null;
            }
            finally
            {
            }
            return Description;
        }



        public static string getTableDescription(string Table, string Context, string Language)
        {
            string Description = "";
            if (Description == null || Description.Length == 0)
            {
                Microsoft.Data.SqlClient.SqlConnection conn = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                //string SQL = "select Description from [DiversityWorkbench].[dbo].[EntityInformation_2] ('" + DiversityWorkbench.Settings.ModuleName + "." + Table + "', '" + Language + "', '" + Context + "')";
                string SQL = "select Description from [dbo].[EntityInformation_2] ('" + Table + "', '" + Language + "', '" + Context + "')";
                Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, conn);
                try
                {
                    conn.Open();
                    Description = Com.ExecuteScalar().ToString();
                }
                catch
                {
                }
                finally
                {
                    conn.Close();
                }
            }
            return Description;
        }

        #region Cached descriptions
        public enum CachedInfo { DisplayText, Abbreviation, Description }
        public static string getDescriptionCache(string Table, string Colum, string Language = "en-US", string Context = "General", CachedInfo info = CachedInfo.Description)
        {
            string Description = "";

            try
            {
                if (_DescriptionCacheExists != null && !(bool)_DescriptionCacheExists)
                {
                    return "";
                }
                else if (_DescriptionCacheExists == null)
                {
                    string SQL = "SELECT COUNT(*) FROM CacheDescription";
                    int Count;
                    if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true), out Count))
                    {
                        _DescriptionCacheExists = false;
                    }
                    else
                    {
                        if (Count == 0)
                        {
                            SQL = "exec dbo.procFillCacheDescription";
                            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                        }
                        _DescriptionCacheExists = true;
                        _CachedDescriptions = new Dictionary<string, CacheDescription>();
                        System.Data.DataTable dt = new System.Data.DataTable();
                        SQL = "SELECT TableName + '|' + ColumnName + '|' + LanguageCode + '|' + Context AS DescriptionKey, DisplayText, Abbreviation, Description FROM CacheDescription";
                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            if (!_CachedDescriptions.ContainsKey(R[0].ToString()))
                            {
                                CacheDescription CD = new CacheDescription();
                                CD.DisplayText = R[1].ToString();
                                CD.Abbreviation = R[2].ToString();
                                CD.Description = R[3].ToString();
                                _CachedDescriptions.Add(R[0].ToString(), CD);
                            }
                        }
                        string Key = Table + "|" + Colum + "|" + Language + "|" + Context;
                        if (_CachedDescriptions.ContainsKey(Key))
                        {
                            switch (info)
                            {
                                case CachedInfo.Description:
                                    Description = _CachedDescriptions[Key].Description;
                                    break;
                                case CachedInfo.DisplayText:
                                    Description = _CachedDescriptions[Key].DisplayText;
                                    break;
                                case CachedInfo.Abbreviation:
                                    Description = _CachedDescriptions[Key].Abbreviation;
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    //Key = "CollectionEventSeries|SeriesCode|en-US|General"
                    string Key = Table + "|" + Colum + "|" + Language + "|" + Context;
                    if (_CachedDescriptions.ContainsKey(Key))
                    {
                        switch (info)
                        {
                            case CachedInfo.Description:
                                Description = _CachedDescriptions[Key].Description;
                                break;
                            case CachedInfo.DisplayText:
                                Description = _CachedDescriptions[Key].DisplayText;
                                break;
                            case CachedInfo.Abbreviation:
                                Description = _CachedDescriptions[Key].Abbreviation;
                                break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Description;
        }

        private static bool? _DescriptionCacheExists;
        //private static System.Collections.Generic.Dictionary<string, string> _ColumnDescriptions;
        private static System.Collections.Generic.Dictionary<string, CacheDescription> _CachedDescriptions;

        #endregion

        #endregion

        #region Text editing

        public void addEditOnDoubleClickToTextboxes()
        {
            try
            {
                if (this.Form != null)
                {
                    foreach (System.Windows.Forms.Control C in this.Form.Controls)
                    {
                        if (C.GetType() == typeof(System.Windows.Forms.GroupBox)
                            || C.GetType() == typeof(System.Windows.Forms.Panel)
                            || C.GetType() == typeof(System.Windows.Forms.TableLayoutPanel)
                            || C.GetType() == typeof(System.Windows.Forms.SplitContainer)
                            || C.GetType() == typeof(System.Windows.Forms.SplitterPanel)
                            || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)
                            || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlLocalList)
                            || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlURI)
                            || C.GetType() == typeof(System.Windows.Forms.TabControl))
                            this.addEditOnDoubleClickToTextboxes(C);
                    }
                }
                else if (this._UserControl != null)
                {
                    foreach (System.Windows.Forms.Control C in this._UserControl.Controls)
                    {
                        if (C.GetType() == typeof(System.Windows.Forms.GroupBox)
                            || C.GetType() == typeof(System.Windows.Forms.Panel)
                            || C.GetType() == typeof(System.Windows.Forms.TableLayoutPanel)
                            || C.GetType() == typeof(System.Windows.Forms.SplitContainer)
                            || C.GetType() == typeof(System.Windows.Forms.SplitterPanel)
                            || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)
                            || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlLocalList)
                            || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlURI)
                            || C.GetType() == typeof(System.Windows.Forms.TabControl))
                            this.addEditOnDoubleClickToTextboxes(C);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void addEditOnDoubleClickToTextboxes(System.Windows.Forms.Control Cont)
        {
            object[] ar = new Object[9] { "", "", "", "", "", "", "", "", "" };
            foreach (System.Windows.Forms.Control C in Cont.Controls)
            {
                if (C.GetType() == typeof(System.Windows.Forms.TextBox)
                    || C.GetType() == typeof(System.Windows.Forms.ComboBox))
                {
                    C.DataBindings.CopyTo(ar, 0);
                    if (ar[0].ToString() != "")
                    {
                        if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                            C.DoubleClick += new System.EventHandler(this.openEditTextBoxOnDoubleClick);
                        else
                            C.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.openEditTextBoxOnMouseDoubleClick);
                    }
                }
                else if (C.GetType() == typeof(System.Windows.Forms.GroupBox)
                    || C.GetType() == typeof(System.Windows.Forms.Panel)
                    || C.GetType() == typeof(System.Windows.Forms.TableLayoutPanel)
                    || C.GetType() == typeof(System.Windows.Forms.SplitContainer)
                    || C.GetType() == typeof(System.Windows.Forms.SplitterPanel)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlLocalList)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlURI)
                    || C.GetType() == typeof(System.Windows.Forms.TabControl)
                    || C.GetType() == typeof(System.Windows.Forms.TabPage))
                    this.addEditOnDoubleClickToTextboxes(C);
            }
        }

        private void openEditTextBoxOnDoubleClick(object sender, System.EventArgs e)
        {
            object[] ar = new Object[9] { "", "", "", "", "", "", "", "", "" };
            System.Windows.Forms.Control C = (System.Windows.Forms.Control)sender;
            string Title = "Edit ";
            int? MaxLength = null;
            if (C.DataBindings != null)
            {
                try
                {
                    C.DataBindings.CopyTo(ar, 0);
                    if (ar[0].ToString() != "")
                    {
                        System.Windows.Forms.Binding B = (System.Windows.Forms.Binding)ar[0];
                        Title = Title + B.BindingMemberInfo.BindingField;
                        try
                        {
                            if (B.BindingManagerBase.Position > -1 && B.BindingManagerBase.Current != null)
                            {
                                System.Data.DataRowView R = (System.Data.DataRowView)B.BindingManagerBase.Current;
                                int iLength = R.Row.Table.Columns[B.BindingMemberInfo.BindingField].MaxLength;
                                if (iLength > 0 && iLength < 32767)
                                {
                                    System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)C;
                                    T.MaxLength = iLength;
                                    MaxLength = iLength;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                        }
                    }
                }
                catch { }
            }
            bool ReadOnly = true;
            try
            {
                System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)C;
                ReadOnly = T.ReadOnly;
            }
            catch { }
            DiversityWorkbench.Forms.FormEditText fet = new DiversityWorkbench.Forms.FormEditText(Title, C.Text, ReadOnly);
            if (MaxLength != null)
            {
                fet.MaxLength = (int)MaxLength;
            }
            fet.ShowDialog();
            if (fet.DialogResult == System.Windows.Forms.DialogResult.OK)
                C.Text = fet.EditedText;
        }

        private void openEditTextBoxOnMouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            object[] ar = new Object[9] { "", "", "", "", "", "", "", "", "" };
            System.Windows.Forms.Control C = (System.Windows.Forms.Control)sender;
            string Title = "Edit ";
            if (C.DataBindings != null)
            {
                try
                {
                    C.DataBindings.CopyTo(ar, 0);
                    if (ar[0].ToString() != "")
                    {
                        System.Windows.Forms.Binding B = (System.Windows.Forms.Binding)ar[0];
                        Title = Title + B.BindingMemberInfo.BindingField;
                    }
                }
                catch { }
            }
            bool R = true;
            try
            {
                System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)C;
                R = T.ReadOnly;
            }
            catch { }
            DiversityWorkbench.Forms.FormEditText fet = new DiversityWorkbench.Forms.FormEditText(Title, C.Text, R);
            fet.ShowDialog();
            if (fet.DialogResult == System.Windows.Forms.DialogResult.OK)
                C.Text = fet.EditedText;
        }

        public static void addEditOnDoubleClickToTextboxes(System.Windows.Forms.TextBox TextBox)
        {
            TextBox.DoubleClick += new System.EventHandler(openTextBoxEditorOnDoubleClick);
        }

        private static void openTextBoxEditorOnDoubleClick(object sender, System.EventArgs e)
        {
            object[] ar = new Object[9] { "", "", "", "", "", "", "", "", "" };
            System.Windows.Forms.Control C = (System.Windows.Forms.Control)sender;
            string Title = "Edit ";
            if (C.DataBindings != null)
            {
                try
                {
                    C.DataBindings.CopyTo(ar, 0);
                    if (ar[0].ToString() != "")
                    {
                        System.Windows.Forms.Binding B = (System.Windows.Forms.Binding)ar[0];
                        Title = Title + B.BindingMemberInfo.BindingField;
                    }
                    else
                    {
                        if (C.Parent != null)
                        {
                            if (C.Parent.AccessibleName != null)
                                Title = C.Parent.AccessibleName;
                            else if (C.Parent.Parent.AccessibleName != null)
                                Title = C.Parent.Parent.AccessibleName;
                        }
                    }
                }
                catch { }
            }
            bool R = true;
            try
            {
                System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)C;
                R = T.ReadOnly;
            }
            catch { }
            DiversityWorkbench.Forms.FormEditText fet = new DiversityWorkbench.Forms.FormEditText(Title, C.Text, R);
            fet.ShowDialog();
            if (fet.DialogResult == System.Windows.Forms.DialogResult.OK)
                C.Text = fet.EditedText;
        }

        #region Restriction of Length

        #region Event for textboxes creating message and cutting text to maximal length

        public void addRestrictLengthToTextboxes()
        {
            try
            {
                if (this.Form != null)
                {
                    foreach (System.Windows.Forms.Control C in this.Form.Controls)
                    {
                        if (C.GetType() == typeof(System.Windows.Forms.GroupBox)
                            || C.GetType() == typeof(System.Windows.Forms.Panel)
                            || C.GetType() == typeof(System.Windows.Forms.TableLayoutPanel)
                            || C.GetType() == typeof(System.Windows.Forms.SplitContainer)
                            || C.GetType() == typeof(System.Windows.Forms.SplitterPanel)
                            || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)
                            || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlLocalList)
                            || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlURI)
                            || C.GetType() == typeof(System.Windows.Forms.TabControl))
                            this.addRestrictLengthToTextboxes(C);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void addRestrictLengthToTextboxes(System.Windows.Forms.Control Cont)
        {
            object[] ar = new Object[9] { "", "", "", "", "", "", "", "", "" };
            foreach (System.Windows.Forms.Control C in Cont.Controls)
            {
                if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                {
                    C.DataBindings.CopyTo(ar, 0);
                    if (ar[0].ToString() != "")
                    {
                        if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                        {
                            C.TextChanged += new System.EventHandler(this.restrictTextBoxLengthOnTextChanged);
                        }
                    }
                }
                else if (C.GetType() == typeof(System.Windows.Forms.GroupBox)
                    || C.GetType() == typeof(System.Windows.Forms.Panel)
                    || C.GetType() == typeof(System.Windows.Forms.TableLayoutPanel)
                    || C.GetType() == typeof(System.Windows.Forms.SplitContainer)
                    || C.GetType() == typeof(System.Windows.Forms.SplitterPanel)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlLocalList)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlURI)
                    || C.GetType() == typeof(System.Windows.Forms.TabControl)
                    || C.GetType() == typeof(System.Windows.Forms.TabPage))
                    this.addRestrictLengthToTextboxes(C);
            }
        }

        private void restrictTextBoxLengthOnTextChanged(object sender, System.EventArgs e)
        {
            object[] ar = new Object[9] { "", "", "", "", "", "", "", "", "" };
            System.Windows.Forms.Control C = (System.Windows.Forms.Control)sender;
            if (C.DataBindings != null)
            {
                try
                {
                    C.DataBindings.CopyTo(ar, 0);
                    if (ar[0].ToString() != "")
                    {
                        System.Windows.Forms.Binding B = (System.Windows.Forms.Binding)ar[0];
                        string Column = B.BindingMemberInfo.BindingField;
                        if (B.BindingManagerBase.Position > -1 && B.BindingManagerBase.Current != null)
                        {
                            System.Data.DataRowView R = (System.Data.DataRowView)B.BindingManagerBase.Current;
                            int iLength = R.Row.Table.Columns[Column].MaxLength;
                            if (iLength > 0)
                            {
                                int CurrentLength = C.Text.Length;
                                if (CurrentLength > iLength)
                                {
                                    System.Windows.Forms.MessageBox.Show("Max. length: " + iLength.ToString());
                                    C.Text = C.Text.Substring(0, iLength - 1);
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                }
            }
        }

        #endregion

        #region setting max length for textboxes - klappt nur mit Daten, daher obige Version ueber Ereignis

        public void setMaxLengthForTextboxes()
        {
            foreach (System.Windows.Forms.Control C in this.Form.Controls)
            {
                if (C.GetType() == typeof(System.Windows.Forms.GroupBox)
                    || C.GetType() == typeof(System.Windows.Forms.Panel)
                    || C.GetType() == typeof(System.Windows.Forms.TableLayoutPanel)
                    || C.GetType() == typeof(System.Windows.Forms.SplitContainer)
                    || C.GetType() == typeof(System.Windows.Forms.SplitterPanel)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlLocalList)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlURI)
                    || C.GetType() == typeof(System.Windows.Forms.TabControl))
                    this.setMaxLengthForTextboxes(C);
            }
        }

        private void setMaxLengthForTextboxes(System.Windows.Forms.Control Cont)
        {
            object[] ar = new Object[9] { "", "", "", "", "", "", "", "", "" };
            foreach (System.Windows.Forms.Control C in Cont.Controls)
            {
                if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                {
                    C.DataBindings.CopyTo(ar, 0);
                    if (ar[0].ToString() != "")
                    {
                        System.Windows.Forms.Binding B = (System.Windows.Forms.Binding)ar[0];
                        string Column = B.BindingMemberInfo.BindingField;
                        if (B.BindingManagerBase.Position > -1 && B.BindingManagerBase.Current != null)
                        {
                            System.Data.DataRowView R = (System.Data.DataRowView)B.BindingManagerBase.Current;
                            int iLength = R.Row.Table.Columns[Column].MaxLength;
                            if (iLength > 0)
                            {
                                System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)Cont;
                                T.MaxLength = iLength;
                            }
                        }
                    }
                }
                else if (C.GetType() == typeof(System.Windows.Forms.GroupBox)
                    || C.GetType() == typeof(System.Windows.Forms.Panel)
                    || C.GetType() == typeof(System.Windows.Forms.TableLayoutPanel)
                    || C.GetType() == typeof(System.Windows.Forms.SplitContainer)
                    || C.GetType() == typeof(System.Windows.Forms.SplitterPanel)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlLocalList)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlURI)
                    || C.GetType() == typeof(System.Windows.Forms.TabControl)
                    || C.GetType() == typeof(System.Windows.Forms.TabPage))
                    this.setMaxLengthForTextboxes(C);
            }
        }

        #endregion

        #endregion

        #endregion

        #region Auxilliary functions
        /// <summary>
        /// setting the visiblity for a comination of fields displaying user defined resp. cached values
        /// the group should contain a label, a combobox, a textbox and a button
        /// the label contains the cached value and is bound to it with no editing possilbe
        /// the combobox needs a data binding of the tag to the ID of the cached value
        /// the textbox contains the typed value and is bound to the same field as the label and enables editing
        /// the button opens the dataset of the cached value in the related module
        /// </summary>
        /// <param name="CC">The control containing the fields</param>
        public void setCacheFieldVisibility(System.Windows.Forms.Control CC)
        {
            bool IDset = false;
            foreach (System.Windows.Forms.Control C in CC.Controls)
            {
                if (C.GetType() == typeof(System.Windows.Forms.ComboBox))
                {
                    try
                    {
                        if (C.DataBindings.Count > 0)
                        {
                            if (C.Tag.Equals(System.DBNull.Value)) IDset = false;
                            else IDset = true;
                        }
                    }
                    catch { }
                }
            }
            foreach (System.Windows.Forms.Control C in CC.Controls)
            {
                if (C.GetType() == typeof(System.Windows.Forms.Label))
                {
                    try
                    {
                        if (C.DataBindings.Count > 0)
                        {
                            C.Visible = IDset;
                            if (IDset) C.Dock = System.Windows.Forms.DockStyle.Fill;
                            else C.Dock = System.Windows.Forms.DockStyle.Left;
                        }
                    }
                    catch { }
                }
            }
            foreach (System.Windows.Forms.Control C in CC.Controls)
            {
                if (C.GetType() == typeof(System.Windows.Forms.TextBox) ||
                    C.GetType() == typeof(System.Windows.Forms.ComboBox))
                    C.Visible = !IDset;
            }
        }

        /// <summary>
        /// setting the visiblity for a comination of fields displaying user defined resp. cached values
        /// the group should contain a label, a combobox, a textbox and a button
        /// the label contains the cached value and is bound to it with no editing possilbe
        /// the textbox contains the typed value and is bound to the same field as the label and enables editing
        /// the button opens the dataset of the cached value in the related module
        /// </summary>
        /// <param name="Control">The control containing the fields</param>
        /// <param name="IdIsSet">If the ID connecting the entry to an external dataset is set</param>
        public void setCacheFieldVisibility(System.Windows.Forms.Control Control, bool IdIsSet)
        {
            foreach (System.Windows.Forms.Control C in Control.Controls)
            {
                if (C.GetType() == typeof(System.Windows.Forms.Label))
                {
                    try
                    {
                        if (C.DataBindings.Count > 0)
                        {
                            C.Visible = IdIsSet;
                            if (IdIsSet) C.Dock = System.Windows.Forms.DockStyle.Fill;
                            else C.Dock = System.Windows.Forms.DockStyle.Left;
                        }
                    }
                    catch { }
                }
            }
            foreach (System.Windows.Forms.Control C in Control.Controls)
            {
                if (C.GetType() == typeof(System.Windows.Forms.TextBox) ||
                    C.GetType() == typeof(System.Windows.Forms.ComboBox))
                    C.Visible = !IdIsSet;
            }
        }


        public void setCacheFieldVisibility(bool IdIsSet, System.Windows.Forms.Control Control)
        {
            foreach (System.Windows.Forms.Control C in Control.Controls)
            {
                if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                {
                    try
                    {
                        if (C.DataBindings.Count > 0)
                        {
                            C.Visible = IdIsSet;
                            C.BackColor = System.Drawing.SystemColors.Info;
                            //							C.BackColor = System.Drawing.Color.LightBlue;
                            //							C.BackColor = System.Drawing.SystemColors.Window;
                            if (IdIsSet) C.Dock = System.Windows.Forms.DockStyle.Fill;
                            else C.Dock = System.Windows.Forms.DockStyle.Left;
                        }
                    }
                    catch { }
                }
                if (C.GetType() == typeof(System.Windows.Forms.ComboBox))
                    C.Visible = !IdIsSet;
                if (C.GetType() == typeof(System.Windows.Forms.Button) && C.Tag != null)
                    C.Visible = IdIsSet;
                if (C.GetType() == typeof(System.Windows.Forms.Button) && C.Tag == null)
                {
                    System.Windows.Forms.Button B = (System.Windows.Forms.Button)C;
                    if (B.ImageList != null)
                    {
                        if (IdIsSet) B.ImageIndex = 1;
                        else B.ImageIndex = 0;
                    }
                }
            }
        }

        //		public void setCacheFieldVisibilitySettingID
        //			(System.Windows.Forms.Control Control)
        //		{
        //			foreach(System.Windows.Forms.Control C in Control.Controls)
        //			{
        //				if (C.GetType() == typeof(System.Windows.Forms.TextBox))
        //				{
        //					try
        //					{
        //						if(C.DataBindings.Count > 0) 
        //						{
        //							C.Visible = true;
        //							C.Dock = System.Windows.Forms.DockStyle.Fill;
        //						}
        //					}
        //					catch{}
        //				}
        //				if (C.GetType() == typeof(System.Windows.Forms.ComboBox) && C.DataBindings.Count > 0)
        //					C.Visible = false;
        //				if (C.GetType() == typeof(System.Windows.Forms.Button) && C.Tag != null)
        //					C.Visible = true;
        //			}
        //		}
        #endregion

        #region Data

        public bool updateTable(System.Data.DataSet DataSet, string TableName, Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapter, System.Windows.Forms.BindingSource BindingSource)
        {
            bool OK = true;
            try
            {

                BindingSource.EndEdit();
                if (DataSet.HasChanges() && sqlDataAdapter != null)
                {
                    sqlDataAdapter.Update(DataSet, TableName);
                }
            }
            catch (Exception ex)
            {
                // Markus 3.5.2022: Avoiding nonsense operation
                if (sqlDataAdapter.SelectCommand.CommandText.Trim().ToLower().EndsWith(" where 2 = 1"))
                    return OK;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, sqlDataAdapter.SelectCommand.CommandText);
                OK = false;
            }
            return OK;
        }



        public bool updateTable(System.Data.DataSet DataSet, string TableName, Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapter, System.Windows.Forms.BindingSource BindingSource, ref string Message)
        {
            bool OK = true;
            try
            {
                BindingSource.EndEdit();
                if (DataSet.HasChanges() && sqlDataAdapter != null)
                {
                    sqlDataAdapter.Update(DataSet, TableName);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                Message = ex.Message;
                OK = false;
            }
            return OK;
        }

        public bool updateTable(System.Data.DataSet DataSet, string TableName, Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapter, System.Windows.Forms.BindingContext BindingContext)
        {
            bool HasChanges = false;
            try
            {
                System.Windows.Forms.BindingManagerBase BMB = BindingContext[DataSet, TableName];
                BMB.EndCurrentEdit();
                if ((DataSet.HasChanges()) && sqlDataAdapter != null)
                {
                    sqlDataAdapter.Update(DataSet, TableName);
                    HasChanges = true;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, TableName);
            }
            return HasChanges;
        }


        public bool updateTable(System.Data.DataSet DataSet, string TableName, Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapter, System.Windows.Forms.BindingContext BindingContext, ref bool OK)
        {
            bool HasChanges = false;
            try
            {
                System.Windows.Forms.BindingManagerBase BMB = BindingContext[DataSet, TableName];
                BMB.EndCurrentEdit();
                if ((DataSet.HasChanges()) && sqlDataAdapter != null)
                {
                    sqlDataAdapter.Update(DataSet, TableName);
                    HasChanges = true;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, TableName);
                OK = false;
                HasChanges = true;
            }
            return HasChanges;
        }

        public bool updateTable(System.Data.DataSet DataSet, string TableName, Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapter, System.Windows.Forms.BindingContext BindingContext, ref System.Collections.Generic.Dictionary<string, string> Messages)
        {
            bool HasChanges = false;
            try
            {
                System.Windows.Forms.BindingManagerBase BMB = BindingContext[DataSet, TableName];
                BMB.EndCurrentEdit();
                if (DataSet.HasChanges() && sqlDataAdapter != null)
                {
                    sqlDataAdapter.Update(DataSet, TableName);
                    HasChanges = true;
                }
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                Messages.Add("ErrorCode", ex.ErrorCode.ToString());
                Messages.Add("Error", ex.Message);
                Messages.Add("Class", ex.Class.ToString());
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, TableName);
                Messages.Add("Error", ex.Message);
            }
            return HasChanges;
        }

        public bool updateTable(System.Data.DataSet DataSet, string TableName, Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapter, System.Windows.Forms.BindingContext BindingContext, ref string Message)
        {
            bool HasChanges = false;
            try
            {
                System.Windows.Forms.BindingManagerBase BMB = BindingContext[DataSet, TableName];
                BMB.EndCurrentEdit();
                if (DataSet.HasChanges() && sqlDataAdapter != null)
                {
                    sqlDataAdapter.Update(DataSet, TableName);
                    HasChanges = true;
                }
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, TableName);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, TableName);
                Message = ex.Message;
            }
            return HasChanges;
        }

        public bool initSqlAdapter(ref Microsoft.Data.SqlClient.SqlDataAdapter Adapter, string SQL, System.Data.DataTable Table)
        {
            try
            {
                if (SQL.Length > 0)
                {
                    if (Adapter == null)
                        Adapter = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    else
                    {
                        // Markus 3.8.2023 - Bugfix missing command
                        if (Adapter.SelectCommand == null)
                        {
                            Adapter = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        }
                        Adapter.SelectCommand.CommandText = SQL;
                        Adapter.SelectCommand.Connection.ConnectionString = DiversityWorkbench.Settings.ConnectionString;
                    }
                    if (Adapter.SelectCommand.Connection.ConnectionString.Length > 0)
                    {
                        Adapter.Fill(Table);
                        Microsoft.Data.SqlClient.SqlCommandBuilder cb = new Microsoft.Data.SqlClient.SqlCommandBuilder(Adapter);
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, Table.TableName + "; " + SQL);
                return false;
            }
            return true;
        }

        /// <summary>
        /// init a sql adapter esp. for errors like ... Concurrency violation: the UpdateCommand affected 0 of the expected 1 records ... DBConcurrencyException
        /// </summary>
        /// <param name="Adapter">The adapter</param>
        /// <param name="SQL">The selection string</param>
        /// <param name="Table">The table containing the data</param>
        /// <param name="Option">The option, regulary ConflictOption.OverwriteChanges</param>
        /// <returns></returns>
        public bool initSqlAdapter(ref Microsoft.Data.SqlClient.SqlDataAdapter Adapter, string SQL, System.Data.DataTable Table, System.Data.ConflictOption Option)
        {
            try
            {
                if (SQL.Length > 0)
                {
                    if (Adapter == null)
                        Adapter = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    else
                    {
                        Adapter.SelectCommand.CommandText = SQL;
                        Adapter.SelectCommand.Connection.ConnectionString = DiversityWorkbench.Settings.ConnectionString;
                    }
                    if (Adapter.SelectCommand.Connection.ConnectionString.Length > 0)
                    {
                        Adapter.Fill(Table);
                        Microsoft.Data.SqlClient.SqlCommandBuilder cb = new Microsoft.Data.SqlClient.SqlCommandBuilder(Adapter);
                        cb.ConflictOption = Option;
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, Table.TableName + "; " + SQL);
                return false;
            }
            return true;
        }

        public static bool initSqlAdapter(ref Microsoft.Data.SqlClient.SqlDataAdapter Adapter, System.Data.DataTable Table, string SQL, string ConnectionString)
        {
            try
            {
                if (Adapter == null)
                    Adapter = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
                else
                {
                    if (Adapter.SelectCommand == null)
                    {
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
                        Adapter.SelectCommand = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    }
                    else
                        Adapter.SelectCommand.CommandText = SQL;
                }
                Adapter.Fill(Table);
                Microsoft.Data.SqlClient.SqlCommandBuilder cb = new Microsoft.Data.SqlClient.SqlCommandBuilder(Adapter);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
                return false;
            }
            return true;
        }

        public bool DatabaseAccessible
        {
            get
            {
                bool OK = true;
                try
                {
                    if (DiversityWorkbench.Settings.ConnectionString.Length == 0)
                        return false;
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand("SELECT 1", con);
                    con.Open();
                    c.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    OK = false;
                }
                return OK;
            }
        }

        #endregion

        #region Image handling

        public static System.Drawing.Icon IconFromImage(System.Drawing.Image Image)
        {
            Bitmap bmp = (Bitmap)Image;
            Bitmap newBmp = new Bitmap(bmp);
            Bitmap targetBmp = newBmp.Clone(new Rectangle(0, 0, newBmp.Width, newBmp.Height), System.Drawing.Imaging.PixelFormat.Format64bppArgb);
            IntPtr Hicon = targetBmp.GetHicon();
            Icon myIcon = Icon.FromHandle(Hicon);
            return myIcon;

        }

        // wiederholter Aufruf des gleichen Bilds vom Web fuehrt zu sehr langen Antwortzeiten. 
        // Benutzer vermutet, dass Programm abgeschiert ist
        // Abhilfe durch Schliessen der Webanfrage
        //private static System.Collections.Generic.Dictionary<string, DiversityWorkbench.WebImage> _WebImages;
        //public static System.Collections.Generic.Dictionary<string, DiversityWorkbench.WebImage> WebImages
        //{
        //    get
        //    {
        //        if (DiversityWorkbench.Forms.FormFunctions._WebImages == null)
        //            DiversityWorkbench.Forms.FormFunctions._WebImages = new Dictionary<string, WebImage>();
        //        return DiversityWorkbench.Forms.FormFunctions._WebImages;
        //    }
        //}

        //public static DiversityWorkbench.WebImage GetWebImage(string URI)
        //{
        //    if (DiversityWorkbench.Forms.FormFunctions.WebImages.ContainsKey(URI))
        //        return DiversityWorkbench.Forms.FormFunctions.WebImages[URI];

        //    DiversityWorkbench.WebImage W = new WebImage();

        //    try
        //    {
        //        if (URI.ToLower().StartsWith("http://") || URI.ToLower().StartsWith("https://"))
        //        {
        //            System.Net.WebRequest webrq = System.Net.WebRequest.Create(URI);
        //            int startIdx = webrq.RequestUri.AbsolutePath.LastIndexOf(".");
        //            if (startIdx < 0)
        //                W.Extension = webrq.RequestUri.AbsolutePath.Substring(startIdx);
        //            W.Bitmap = (Bitmap)Bitmap.FromStream(webrq.GetResponse().GetResponseStream());
        //            System.Net.WebResponse webResponse = webrq.GetResponse();
        //            W.Size = webResponse.ContentLength;
        //            System.Uri U = new Uri(URI);
        //            W.URI = U;
        //            W.ContentType = webResponse.Headers["Content-Type"].ToString();
        //            W.ContentTypeBase = W.ContentType.Substring(0, W.ContentType.IndexOf("/"));
        //            webResponse.Close();
        //            webrq.Abort();
        //            return DiversityWorkbench.Forms.FormFunctions.WebImages[URI];
        //        }
        //    }
        //    catch (System.Exception ex) { }

        //    return W;
        //}

        public static string UrlText(string url)
        {
            return url.Trim().TrimEnd(';', ',', ':');
        }

        public static string MimeType(string Path)
        {
            string M = "application/octet-stream";
            string Extension = "";
            try
            {
                Path = UrlText(Path);
                if (Path.Length > 0)
                {
                    if (Path.ToLower().StartsWith("http://") || Path.ToLower().StartsWith("https://"))
                    {
                        //DiversityWorkbench.WebImage W = DiversityWorkbench.Forms.FormFunctions.GetWebImage(Path);
                        //Extension = W.Extension;

                        System.Net.WebRequest webrq = System.Net.WebRequest.Create(Path);
                        int startIdx = webrq.RequestUri.AbsolutePath.LastIndexOf(".");
                        if (startIdx < 0)
                            return M;
                        Extension = webrq.RequestUri.AbsolutePath.Substring(startIdx);
                        webrq.Abort();
                    }
                    else if (Path.ToLower().StartsWith("color://#"))
                    {
                        return "color/rgbhex";
                    }
                    else
                    {
                        if (Path.ToLower().StartsWith("file:///"))
                            Path = Path.Substring("file:///".Length);
                        System.IO.FileInfo F = new System.IO.FileInfo(Path);
                        Extension = F.Extension;
                    }

                    if (Extension.LastIndexOf('.') > -1)
                        Extension = Extension.Substring(Extension.LastIndexOf('.') + 1);
                    if (Extension == string.Empty)
                        Extension = "octet-stream";

                    switch (MediaType(Path))
                    {
                        case Medium.Image:
                            M = "image/" + (Extension == "jpg" ? "jpeg" : Extension);
                            break;
                        case Medium.Audio:
                            M = "audio/" + Extension;
                            break;
                        case Medium.Video:
                            M = "video/" + Extension;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch { }
            return M;
        }

        public static DiversityWorkbench.Forms.FormFunctions.Medium MediaType(string Path)
        {
            DiversityWorkbench.Forms.FormFunctions.Medium M = Medium.Unknown; // new MediaTye Unknown Toni 3.10.12
            string Extension = "";
            try
            {
                Path = UrlText(Path);
                if (Path.Length > 0)
                {
                    if (Path.ToLower().StartsWith("http://") || Path.ToLower().StartsWith("https:"))
                    {
                        System.Net.WebRequest webrq = System.Net.WebRequest.Create(Path);
                        webrq.Timeout = DiversityWorkbench.Settings.TimeoutWeb;
                        int startIdx = webrq.RequestUri.AbsolutePath.LastIndexOf(".");
                        if (webrq.RequestUri.AbsolutePath.Length <= 1)
                            return M;

                        if (startIdx < 0)
                        {
                            try
                            {
                                // Markus 23.7.2018 - try to get an image, e.g. for an URL like https://biocase.zfmk.de/media?guid=46953E91-1F63-4F4C-B599-0EA3B2DD4986
                                System.IO.Stream stream = webrq.GetResponse().GetResponseStream();
                                System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                                if (image.Height > 0 && image.Width > 0)
                                    M = Medium.Image;
                            }
                            catch (System.Exception ex)
                            {
                            }
                            return M;
                        }
                        Extension = webrq.RequestUri.AbsolutePath.Substring(startIdx);
                        webrq.Abort();
                    }
                    else
                    {
                        System.IO.FileInfo F = new System.IO.FileInfo(Path.ToLower().StartsWith("file:///") ? Path.Substring("file:///".Length) : Path);
                        Extension = F.Extension;
                    }

                    switch (Extension.ToLower())
                    {
                        case ".jpeg":
                        case ".jpg":
                        case ".gif":
                        case ".png":
                        case ".tif":
                            M = DiversityWorkbench.Forms.FormFunctions.Medium.Image;
                            break;
                        case ".svg":
                            M = DiversityWorkbench.Forms.FormFunctions.Medium.VectorGraphic;
                            break;
                        case ".au":
                        case ".snd":
                        case ".aif":
                        case ".aifc":
                        case ".aiff":
                        case ".wma":
                        case ".mp3":
                        case ".wav":
                        case ".mid":
                        case ".ogg":
                            M = DiversityWorkbench.Forms.FormFunctions.Medium.Audio;
                            break;
                        case ".mpeg":
                        case ".mpg":
                        case ".m1v":
                        case ".mp2":
                        case ".mpa":
                        case ".mpe":
                        case ".avi":
                        case ".wmv":
                            M = DiversityWorkbench.Forms.FormFunctions.Medium.Video;
                            break;
                        case ".exe":
                        case ".com":
                        case ".msi":
                        case ".bat":
                        case ".zip":
                        case ".ps1":
                            M = DiversityWorkbench.Forms.FormFunctions.Medium.Ignore;
                            break;
                        default:
                            M = DiversityWorkbench.Forms.FormFunctions.Medium.Unknown; // new MediaTye Unknown Toni 3.10.12
                            break;
                    }
                }
            }
            catch { }
            return M;
        }

        public static System.Drawing.Bitmap BitmapFromWeb(
            string ImageName,
            System.Drawing.Bitmap IconTooBig,
            System.Drawing.Bitmap IconFileNotFound)
        {
            System.Drawing.Bitmap Bitmap = IconFileNotFound;
            if (ImageName.Length == 0)
            {
                return IconFileNotFound;
            }
            else
            {
                DiversityWorkbench.Forms.FormFunctions.Medium Medium = MediaType(ImageName);
                if (ImageName.Trim().IndexOf("http:") == 0 || ImageName.Trim().IndexOf("https:") == 0)
                {
                    switch (Medium)
                    {
                        case Medium.Image:
                            try
                            {
                                ImageName = ImageName.Replace("\\", "/");
                                if (ImageName.IndexOf("@") > -1 && ImageName.IndexOf(":", 6) > -1)
                                {
                                    //throw;
                                }

                                System.Uri U = new Uri(ImageName);
                                if (DiversityWorkbench.WorkbenchSettings.Default.ForceCanonicalPathAndQuery)
                                    DiversityWorkbench.Forms.FormFunctions.ForceCanonicalPathAndQuery(U);
                                System.Net.WebRequest webrq = System.Net.WebRequest.Create(U);//ImageName);
                                webrq.Timeout = DiversityWorkbench.Settings.TimeoutWeb;
                                if (DiversityWorkbench.Settings.TimeoutWeb == 0)
                                    throw new System.Net.WebException();
                                System.Net.WebResponse webResponse = webrq.GetResponse();
                                string ContentType = webResponse.Headers["Content-Type"].ToString();
                                string ContentTypeBase = ContentType.Substring(0, ContentType.IndexOf("/"));
                                long LengthOfUri = webResponse.ContentLength;
                                int SizeOfImage = (int)LengthOfUri / 1000;
                                if (SizeOfImage > DiversityWorkbench.Settings.MaximalImageSizeInKb)
                                {
                                    System.Drawing.Bitmap bmpToBig = IconTooBig;
                                }
                                else
                                {
                                    switch (ContentTypeBase.ToLower())
                                    {
                                        case "image":
                                            Bitmap = (Bitmap)Bitmap.FromStream(webResponse.GetResponseStream());//W.Bitmap;
                                            break;
                                        case "audio":
                                            Bitmap = DiversityWorkbench.Forms.FormFunctions.MediaBitMap(Medium.Audio);
                                            break;
                                        case "video":
                                            Bitmap = DiversityWorkbench.Forms.FormFunctions.MediaBitMap(Medium.Video);
                                            break;
                                        default:
                                            Bitmap = DiversityWorkbench.Forms.FormFunctions.MediaBitMap(Medium.Image);
                                            break;
                                    }
                                }
                                webResponse.Close();
                                webrq.Abort();
                                return Bitmap;
                            }
                            catch (System.OutOfMemoryException oomex)
                            {
                                return IconTooBig;
                            }
                            catch (System.Net.WebException wx)
                            {
                                if (wx.Response != null &&
                                    ((System.Net.HttpWebResponse)wx.Response).StatusCode.ToString() == "Unauthorized")
                                {
                                    try
                                    {
                                        System.Net.HttpWebResponse HttpWResponse;
                                        string User = "";
                                        string PW = "";
                                        string Domain = "";
                                        System.Net.NetworkCredential Cre = new System.Net.NetworkCredential();
                                        if (ImageName.IndexOf("@") > -1 && ImageName.IndexOf(":", 6) > -1)
                                        {
                                            User = ImageName.Substring(7);
                                            PW = User.Substring(User.IndexOf(":") + 1);
                                            PW = PW.Substring(0, PW.IndexOf("@"));
                                            User = User.Substring(0, User.IndexOf(":"));
                                            //ImageName = ImageName.Substring(0, ImageName.IndexOf("//")) + "//" + ImageName.Substring(ImageName.IndexOf("@") + 1);
                                        }
                                        System.Net.WebRequest webrq = System.Net.WebRequest.Create(ImageName);
                                        //webrq.Timeout = 1000;
                                        System.Net.HttpWebRequest HttpWebRequest = (System.Net.HttpWebRequest)webrq;
                                        System.Uri U = new Uri(ImageName);
                                        System.Net.NetworkCredential myCred = new System.Net.NetworkCredential(User, PW);
                                        System.Net.CredentialCache MyCrendentialCache = new System.Net.CredentialCache();
                                        MyCrendentialCache.Add(U, "Basic", myCred);
                                        HttpWebRequest.Credentials = MyCrendentialCache;
                                        HttpWebRequest.Method = "PROPFIND";
                                        HttpWebRequest.Proxy = null;
                                        HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.1; Windows XP)";
                                        HttpWebRequest.KeepAlive = true; //this is the default              
                                        HttpWebRequest.Headers.Set("Pragma", "no-cache");
                                        HttpWebRequest.Timeout = DiversityWorkbench.Settings.TimeoutWeb;// 10000;
                                        HttpWebRequest.Method = "Get";
                                        //if (null != HttpWResponse)
                                        //{
                                        //    HttpWResponse.Close(); // close any previous connection
                                        //    HttpWResponse = null; // clear the object.
                                        //}
                                        HttpWResponse = (System.Net.HttpWebResponse)HttpWebRequest.GetResponse();
                                        long LengthOfUri = HttpWResponse.ContentLength;
                                        int SizeOfImage = (int)LengthOfUri / 1000;
                                        if (SizeOfImage > DiversityWorkbench.Settings.MaximalImageSizeInKb)
                                        {
                                            return IconTooBig;
                                        }
                                        else
                                        {
                                            Bitmap = (Bitmap)Bitmap.FromStream(HttpWResponse.GetResponseStream());
                                        }
                                        HttpWResponse.Close();
                                        HttpWResponse = null;
                                        webrq.Abort();
                                        return Bitmap;
                                    }
                                    catch
                                    {
                                        return IconFileNotFound;
                                    }
                                }
                                else
                                {
                                    return IconFileNotFound;
                                }
                            }
                            catch (System.Exception x)
                            {
                                return IconFileNotFound;
                            }
                            break;
                        case Medium.Audio:
                            DiversityWorkbench.UserControls.UserControlImage Ua = new DiversityWorkbench.UserControls.UserControlImage();
                            return (Bitmap)Ua.DefaultIconAudio;
                            break;
                        case Medium.Video:
                            DiversityWorkbench.UserControls.UserControlImage Uv = new DiversityWorkbench.UserControls.UserControlImage();
                            return (Bitmap)Uv.DefaultIconVideo;
                            break;
                        default:
                            DiversityWorkbench.UserControls.UserControlImage Uu = new DiversityWorkbench.UserControls.UserControlImage();
                            return (Bitmap)Uu.DefaultIconUnknown;
                            break;
                    }
                }
                else
                {
                    return IconFileNotFound;
                }
            }
            return Bitmap;
        }

        public static System.Drawing.Bitmap BitmapFromWeb(
            string ImageName,
            ref string Message,
            bool IgnoreSizeLimit = false)
        {
            System.Drawing.Bitmap Bitmap = null;
            if (ImageName.Length == 0)
            {
                Message = ImageNotFoundMessage();
                return null;
            }
            else
            {
                int SizeOfImage = 0;
                if (ImageName.IndexOf("http:") == 0 || ImageName.IndexOf("https:") == 0)
                {
                    try
                    {
                        if (DiversityWorkbench.Settings.TimeoutWeb == 0)
                        {
                            Message = "Webrequests blocked. Timeout set to 0";
                            return null;
                        }
                        ImageName = ImageName.Replace("\\", "/");
                        if (ImageName.IndexOf("@") > -1 && ImageName.IndexOf(":", 6) > -1)
                        {
                            //throw;
                        }
                        System.Uri U = new Uri(ImageName);
                        if (DiversityWorkbench.WorkbenchSettings.Default.ForceCanonicalPathAndQuery)
                            DiversityWorkbench.Forms.FormFunctions.ForceCanonicalPathAndQuery(U);
                        System.Net.WebRequest webrq = System.Net.WebRequest.Create(U);//ImageName);
                        webrq.Timeout = DiversityWorkbench.Settings.TimeoutWeb;
                        webrq.UseDefaultCredentials = true;
                        System.Net.WebResponse webResponse = webrq.GetResponse();
                        string ContentType = webResponse.Headers["Content-Type"].ToString();
                        string ContentTypeBase = ContentType.Substring(0, ContentType.IndexOf("/"));
                        long LengthOfUri = webResponse.ContentLength;
                        SizeOfImage = (int)LengthOfUri / 1000;
                        if (!IgnoreSizeLimit && SizeOfImage > DiversityWorkbench.Settings.MaximalImageSizeInKb)
                        {
                            Message = ImageTooBigMessage(SizeOfImage);
                        }
                        else
                        {
                            switch (ContentTypeBase.ToLower())
                            {
                                case "image":
                                    Bitmap = (Bitmap)Bitmap.FromStream(webResponse.GetResponseStream());
                                    break;
                                case "audio":
                                    Bitmap = DiversityWorkbench.Forms.FormFunctions.MediaBitMap(Medium.Audio);
                                    break;
                                case "video":
                                    Bitmap = DiversityWorkbench.Forms.FormFunctions.MediaBitMap(Medium.Video);
                                    break;
                                default:
                                    Message = "Content type unknown";
                                    Bitmap = DiversityWorkbench.Forms.FormFunctions.MediaBitMap(Medium.Ignore);
                                    break;
                            }
                        }
                        webResponse.Close();
                        webrq.Abort();
                        return Bitmap;
                    }
                    catch (System.OutOfMemoryException oomex)
                    {
                        Message = oomex.Message;// 
                        if (DiversityWorkbench.Settings.TimeoutWeb < (int)SizeOfImage * 2) // beliebigen Wert genommen - muesste man austesten
                            Message += "\r\nTimeout for web request is 1 sec.\r\nA higher value may help";
                        if (SizeOfImage > DiversityWorkbench.Settings.MaximalImageSizeInKb)
                            Message = ImageTooBigMessage(SizeOfImage);
                        return Bitmap;
                    }
                    catch (System.Net.WebException wx)
                    {
                        if (wx.Response != null &&
                            ((System.Net.HttpWebResponse)wx.Response).StatusCode.ToString() == "Unauthorized")
                        {
                            try
                            {
                                System.Net.HttpWebResponse HttpWResponse;
                                string User = "";
                                string PW = "";
                                string Domain = "";
                                System.Net.NetworkCredential Cre = new System.Net.NetworkCredential();
                                if (ImageName.IndexOf("@") > -1 && ImageName.IndexOf(":", 6) > -1)
                                {
                                    User = ImageName.Substring(7);
                                    PW = User.Substring(User.IndexOf(":") + 1);
                                    PW = PW.Substring(0, PW.IndexOf("@"));
                                    User = User.Substring(0, User.IndexOf(":"));
                                    //ImageName = ImageName.Substring(0, ImageName.IndexOf("//")) + "//" + ImageName.Substring(ImageName.IndexOf("@") + 1);
                                }
                                System.Net.WebRequest webrq = System.Net.WebRequest.Create(ImageName);
                                System.Net.HttpWebRequest HttpWebRequest = (System.Net.HttpWebRequest)webrq;
                                System.Uri U = new Uri(ImageName);
                                System.Net.NetworkCredential myCred = new System.Net.NetworkCredential(User, PW);
                                System.Net.CredentialCache MyCrendentialCache = new System.Net.CredentialCache();
                                MyCrendentialCache.Add(U, "Basic", myCred);
                                HttpWebRequest.Credentials = MyCrendentialCache;
                                HttpWebRequest.Method = "PROPFIND";
                                HttpWebRequest.Proxy = null;
                                HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.1; Windows XP)";
                                HttpWebRequest.KeepAlive = true; //this is the default              
                                HttpWebRequest.Headers.Set("Pragma", "no-cache");
                                HttpWebRequest.Timeout = 10000;
                                HttpWebRequest.Method = "Get";
                                //if (null != HttpWResponse)
                                //{
                                //    HttpWResponse.Close(); // close any previous connection
                                //    HttpWResponse = null; // clear the object.
                                //}
                                HttpWResponse = (System.Net.HttpWebResponse)HttpWebRequest.GetResponse();
                                long LengthOfUri = HttpWResponse.ContentLength;
                                SizeOfImage = (int)LengthOfUri / 1000;
                                if (SizeOfImage > DiversityWorkbench.Settings.MaximalImageSizeInKb)
                                {
                                    Message = ImageTooBigMessage(SizeOfImage);
                                    return Bitmap;
                                }
                                else
                                {
                                    Bitmap = (Bitmap)Bitmap.FromStream(HttpWResponse.GetResponseStream());
                                }
                                HttpWResponse.Close();
                                HttpWResponse = null;
                                webrq.Abort();
                                return Bitmap;
                            }
                            catch
                            {
                                Message = ImageNotFoundMessage();
                                return Bitmap;
                            }
                        }
                        else if (wx.Status == System.Net.WebExceptionStatus.SecureChannelFailure)
                        {
                            // Toni 20200715: Since some time Wiki links like
                            // https://upload.wikimedia.org/wikipedia/commons/thumb/d/d7/Eisenhut_blau.JPG/500px-Eisenhut_blau.JPG
                            // return an error concerning the ssl connection. Anyway, the browser can show the picture..
                            Message = System.Net.WebExceptionStatus.SecureChannelFailure.ToString();
                        }
                        else if (wx.Status == System.Net.WebExceptionStatus.ProtocolError)
                        {
                            // Markus 3.2.2022 - gleicher Fehler wie oben
                            Message = System.Net.WebExceptionStatus.SecureChannelFailure.ToString();
                        }
                        else
                        {
                            Message = ImageNotFoundMessage();
                            return Bitmap;
                        }
                    }
                    catch (System.Exception x)
                    {
                        Message = ImageNotFoundMessage();
                        return Bitmap;
                    }
                }
                else
                {
                    try
                    {
                        System.IO.FileInfo File = new System.IO.FileInfo(ImageName.StartsWith("file:\\\\\\") ? ImageName.Substring("file:\\\\\\".Length) : ImageName);
                        if (File.Exists)
                        {
                            System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(File.FullName);
                            return bmp;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                    Message = ImageNotFoundMessage();
                    return Bitmap;
                }
            }
            return Bitmap;
        }

        /// <summary>
        /// from https://stackoverflow.com/questions/781205/getting-a-url-with-an-url-encoded-slash
        /// </summary>
        /// <param name="uri">the URI</param>
        public static void ForceCanonicalPathAndQuery(Uri uri)
        {
            string paq = uri.PathAndQuery; // need to access PathAndQuery
            System.Reflection.FieldInfo flagsFieldInfo = typeof(Uri).GetField("m_Flags", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            ulong flags = (ulong)flagsFieldInfo.GetValue(uri);
            flags &= ~((ulong)0x30); // Flags.PathNotCanonical|Flags.QueryNotCanonical
            flagsFieldInfo.SetValue(uri, flags);
        }

        public static void setImageDescription(System.Windows.Forms.BindingSource BindingSource)
        {
            if (BindingSource.Current == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select an image");
                return;
            }
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)BindingSource.Current;
                string XML = R["Description"].ToString();
                DiversityWorkbench.Forms.FormXml f = new DiversityWorkbench.Forms.FormXml("EXIF for image", XML, true);
                f.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }



        public static string ImageTooBigMessage(int Size)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkbenchMessages));
            string Message = resources.GetString("ImageSize").Substring(0, 1).ToUpper()
                + resources.GetString("ImageSize").Substring(1)
                + ": " + Size.ToString() + " KB\r\n"
                + resources.GetString("ImageSize") + " " + resources.GetString("restricted_to") + " " + DiversityWorkbench.Settings.MaximalImageSizeInKb.ToString() + " KB";
            return Message;
        }

        public static string ImageNotFoundMessage()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkbenchMessages));
            string Message = resources.GetString("Image").Substring(0, 1).ToUpper()
                            + resources.GetString("Image").Substring(1)
                            + " " + resources.GetString("NotFound");
            return Message;
        }

        private void FillImageList(
            string ImageName,
            ref System.Windows.Forms.ImageList ImageList,
            DiversityWorkbench.UserControls.UserControlImage UserControlImage)
        {
            this.FillImageList(ImageName, ref ImageList, (System.Drawing.Bitmap)UserControlImage.DefaultIconTooBig, (System.Drawing.Bitmap)UserControlImage.DefaultIconWrongPath);
        }

        private void FillImageList(
            string ImageName,
            ref System.Windows.Forms.ImageList ImageList,
            System.Drawing.Bitmap IconTooBig,
            System.Drawing.Bitmap IconFileNotFound)
        {
            if (ImageName.Length == 0)
            {
                System.Drawing.Bitmap bmp = IconFileNotFound;
                ImageList.Images.Add(bmp);
            }
            else
            {
                if (ImageName.Trim().IndexOf("http:") == 0 || ImageName.Trim().IndexOf("https:") == 0)
                {
                    try
                    {
                        ImageList.Images.Add(DiversityWorkbench.Forms.FormFunctions.BitmapFromWeb(ImageName, IconTooBig, IconFileNotFound));
                    }
                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                }
                else
                {
                    try
                    {
                        Medium M = MediaType(ImageName);
                        DiversityWorkbench.UserControls.UserControlImage U = new DiversityWorkbench.UserControls.UserControlImage();
                        switch (M)
                        {
                            case Medium.Image:
                                System.Drawing.Bitmap Picture = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(ImageName);
                                ImageList.Images.Add(Picture);
                                break;
                            case Medium.Audio:
                                ImageList.Images.Add(U.DefaultIconAudio);
                                break;
                            case Medium.Video:
                                ImageList.Images.Add(U.DefaultIconVideo);
                                break;
                            default:
                                ImageList.Images.Add(U.DefaultIconUnknown);
                                break;
                        }
                    }
                    catch (System.ArgumentException ex)
                    {
                        try
                        {
                            System.Net.WebRequest webrq = System.Net.WebRequest.Create(ImageName);
                            System.Net.WebResponse webResponse = webrq.GetResponse();
                            long LengthOfUri = webResponse.ContentLength;
                            int SizeOfImage = (int)LengthOfUri / 1000;
                            if (SizeOfImage > DiversityWorkbench.Settings.MaximalImageSizeInKb)
                            {
                                System.Drawing.Bitmap bmpToBig = IconTooBig;
                                ImageList.Images.Add(bmpToBig);
                            }
                            else
                            {
                                System.Drawing.Bitmap bmp = (Bitmap)Bitmap.FromStream(webResponse.GetResponseStream());
                                ImageList.Images.Add(bmp);
                            }
                            webResponse.Close();
                            webrq.Abort();
                        }
                        catch (System.OutOfMemoryException oomex)
                        {
                            DiversityWorkbench.UserControls.UserControlImage uci = new DiversityWorkbench.UserControls.UserControlImage();
                            ImageList.Images.Add((Bitmap)uci.DefaultIconTooBig);
                        }
                        catch (System.Net.WebException wx)
                        {
                            System.Drawing.Bitmap bmp = IconFileNotFound;
                            ImageList.Images.Add(bmp);
                        }
                        catch (System.Exception x)
                        {
                            System.Drawing.Bitmap bmp = IconFileNotFound;
                            ImageList.Images.Add(bmp);
                        }
                    }
                    catch (System.Exception exc)
                    {
                        System.Drawing.Bitmap bmp = IconFileNotFound;
                        ImageList.Images.Add(bmp);
                    }
                }
            }
        }

        public void FillImageList(System.Windows.Forms.ListBox ListBox,
            System.Windows.Forms.ImageList ImageList,
            System.Windows.Forms.ImageList DefaultImages,
            System.Data.DataTable DataTable,
            string ImagepathDataColumn,
            DiversityWorkbench.UserControls.UserControlImage UserControlImage)
        {
            try
            {
                ImageList.Images.Clear();
                ListBox.Items.Clear();
                string s = "";
                if (DataTable.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow r in DataTable.Rows)
                    {
                        if (r.RowState != System.Data.DataRowState.Deleted
                            && !r[ImagepathDataColumn].Equals(System.DBNull.Value)
                            && r[ImagepathDataColumn].ToString().Length > 0)
                        {
                            s = r[ImagepathDataColumn].ToString();
                            this.FillImageList(s, ref ImageList, UserControlImage);
                        }
                    }
                    foreach (System.Drawing.Image i in ImageList.Images)
                    {
                        ListBox.Items.Add(i);
                    }
                    if (DataTable.Rows.Count > 0)
                    {
                        try
                        {
                            if (DataTable.Rows[0].RowState != System.Data.DataRowState.Deleted)
                            {
                                // only for test
                                string strResourceID = DataTable.Rows[0]["ResourceURI"].ToString();

                                if (ListBox.Items.Count > 0)
                                    ListBox.SelectedIndex = 0;
                            }
                        }
                        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                    }
                }
                else
                {
                    UserControlImage.ImagePath = "";
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void FillImageList(System.Windows.Forms.ListBox ListBox,
            System.Windows.Forms.ImageList ImageList,
            System.Windows.Forms.ImageList DefaultImages,
            System.Data.DataRow[] DataRows,
            string ImagepathDataColumn,
            DiversityWorkbench.UserControls.UserControlImage UserControlImage)
        {
            try
            {
                ImageList.Images.Clear();
                ListBox.Items.Clear();
                string s = "";
                if (DataRows != null && DataRows.Length > 0)
                {
                    for (int i = 0; i < DataRows.Length; i++)//System.Data.DataRow r in DataTable.Rows)
                    {
                        if (DataRows[i].RowState != System.Data.DataRowState.Deleted)
                        {
                            s = DataRows[i][ImagepathDataColumn].ToString();
                            this.FillImageList(s, ref ImageList, UserControlImage);
                        }
                    }
                    foreach (System.Drawing.Image i in ImageList.Images)
                    {
                        ListBox.Items.Add(i);
                    }
                    if (DataRows.Length > 0)
                    {
                        try
                        {
                            string strResourceID = DataRows[0]["ResourceURI"].ToString();
                            if (ListBox.Items.Count > 0)
                                ListBox.SelectedIndex = 0;
                        }
                        catch { }
                    }
                }
                else
                {
                    UserControlImage.ImagePath = "";
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void FillImageList(System.Windows.Forms.ListBox ListBox,
            System.Windows.Forms.ImageList ImageList,
            System.Data.DataTable DataTable,
            string ImagepathDataColumn,
            DiversityWorkbench.UserControls.UserControlImage UserControlImage)
        {
            try
            {
                ImageList.Images.Clear();
                ListBox.Items.Clear();
                string s = "";
                if (DataTable.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow r in DataTable.Rows)
                    {
                        if (r.RowState != System.Data.DataRowState.Deleted)
                        {
                            s = r[ImagepathDataColumn].ToString();
                            this.FillImageList(s, ref ImageList, UserControlImage);
                        }
                    }
                    foreach (System.Drawing.Image i in ImageList.Images)
                    {
                        ListBox.Items.Add(i);
                    }
                    if (DataTable.Rows.Count > 0)
                    {
                        try
                        {
                            string strResourceID = DataTable.Rows[0]["ResourceURI"].ToString();
                            if (ListBox.Items.Count > 0)
                                ListBox.SelectedIndex = 0;
                        }
                        catch { }
                    }
                }
                else
                {
                    UserControlImage.ImagePath = "";
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        public void FillImageList(System.Windows.Forms.ListBox ListBox,
            System.Windows.Forms.ImageList ImageList,
            System.Windows.Forms.ImageList DefaultImages,
            System.Data.DataTable DataTable,
            string ImagepathDataColumn,
            DiversityWorkbench.UserControls.UserControlImage UserControlImage,
            string ImageFolder)
        {
            try
            {
                ImageList.Images.Clear();
                ListBox.Items.Clear();
                string s = "";
                //System.Data.DataView V = new System.Data.DataView(DataTable, "", ImagepathDataColumn + " DESC", System.Data.DataViewRowState.CurrentRows);
                if (DataTable.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow r in DataTable.Rows)
                    {
                        if (r.RowState != System.Data.DataRowState.Deleted)
                        {
                            if (r[ImagepathDataColumn].ToString().Length == 0)
                            {
                                System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)DefaultImages.Images[0];
                                ImageList.Images.Add(bmp);
                            }
                            else
                            {
                                string ImagePath = r[ImagepathDataColumn].ToString();
                                if (ImageFolder.Length == 0 && ImagePath.IndexOf("/") == -1 && ImagePath.IndexOf("\\") == -1)
                                {
                                    System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)DefaultImages.Images[0];
                                    ImageList.Images.Add(bmp);
                                }
                                else
                                {
                                    if (!ImagePath.StartsWith("http://") && !ImagePath.StartsWith("https://"))
                                        ImagePath = ImageFolder + ImagePath;
                                    this.FillImageList(ImagePath, ref ImageList, UserControlImage);
                                }
                            }
                        }
                    }
                    foreach (System.Drawing.Image i in ImageList.Images)
                    {
                        ListBox.Items.Add(i);
                    }
                    if (DataTable.Rows.Count > 0)
                    {
                        try
                        {
                            string strResourceID = DataTable.Rows[0]["ResourceURI"].ToString();
                            if (ListBox.Items.Count > 0)
                                ListBox.SelectedIndex = 0;
                        }
                        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                    }
                }
                else
                {
                    UserControlImage.ImagePath = "";
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        public void FillImageList(System.Windows.Forms.ListBox ListBox,
            System.Windows.Forms.ImageList ImageList,
            System.Windows.Forms.ImageList DefaultImages,
            System.Data.DataTable DataTable,
            string ImagepathDataColumn,
            System.Windows.Forms.PictureBox PictureBox)
        {
            try
            {
                ImageList.Images.Clear();
                ListBox.Items.Clear();
                string s = "";
                if (DataTable.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow r in DataTable.Rows)
                    {
                        if (r.RowState != System.Data.DataRowState.Deleted)
                        {
                            s = r[ImagepathDataColumn].ToString();
                            System.Drawing.Bitmap BM0 = null;
                            System.Drawing.Bitmap BM1 = null;
                            if (DefaultImages.Images.Count > 0)
                                BM0 = (System.Drawing.Bitmap)DefaultImages.Images[0];
                            else
                            {
                                BM0 = (System.Drawing.Bitmap)DiversityWorkbench.Properties.Resources.NotFound;
                                BM1 = (System.Drawing.Bitmap)DiversityWorkbench.Properties.Resources.TooBig;
                            }
                            if (DefaultImages.Images.Count > 1)
                            {
                                BM1 = (System.Drawing.Bitmap)DefaultImages.Images[1];
                            }
                            else
                            {
                                BM1 = (System.Drawing.Bitmap)DiversityWorkbench.Properties.Resources.TooBig;
                            }
                            this.FillImageList(s, ref ImageList, BM1, BM0);
                            //this.FillImageList(s, ref ImageList, (System.Drawing.Bitmap)DefaultImages.Images[1], (System.Drawing.Bitmap)DefaultImages.Images[0]);
                        }
                    }
                    foreach (System.Drawing.Image i in ImageList.Images)
                    {
                        ListBox.Items.Add(i);
                    }
                    if (DataTable.Rows.Count > 0)
                    {
                        try
                        {
                            string strResourceID = DataTable.Rows[0][ImagepathDataColumn].ToString();
                            if (ListBox.Items.Count > 0)
                                ListBox.SelectedIndex = 0;
                        }
                        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                    }
                }
                else
                {
                    PictureBox.Image = null;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void FillImageList(System.Windows.Forms.ListBox ListBox,
            System.Windows.Forms.ImageList ImageList,
            System.Data.DataTable DataTable,
            string ImagepathDataColumn,
            System.Windows.Forms.PictureBox PictureBox)
        {
            try
            {
                ImageList.Images.Clear();
                ListBox.Items.Clear();
                string s = "";
                if (DataTable.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow r in DataTable.Rows)
                    {
                        if (r.RowState != System.Data.DataRowState.Deleted)
                        {
                            s = r[ImagepathDataColumn].ToString();
                            System.Drawing.Bitmap BM0 = (System.Drawing.Bitmap)DiversityWorkbench.Properties.Resources.NotFound;
                            System.Drawing.Bitmap BM1 = (System.Drawing.Bitmap)DiversityWorkbench.Properties.Resources.TooBig;
                            this.FillImageList(s, ref ImageList, BM1, BM0);
                        }
                    }
                    foreach (System.Drawing.Image i in ImageList.Images)
                    {
                        ListBox.Items.Add(i);
                    }
                    if (DataTable.Rows.Count > 0)
                    {
                        try
                        {
                            string strResourceID = DataTable.Rows[0][ImagepathDataColumn].ToString();
                            if (ListBox.Items.Count > 0)
                                ListBox.SelectedIndex = 0;
                        }
                        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                    }
                }
                else
                {
                    PictureBox.Image = null;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public static void SetPictureBoxImage(string Path, System.Windows.Forms.PictureBox PictureBox, System.Windows.Forms.ImageList ImageList)
        {
            if (Path == "")
            {
                PictureBox.Image = null;
            }
            else
            {
                try
                {
                    DiversityWorkbench.Forms.FormFunctions.Medium MediumType = DiversityWorkbench.Forms.FormFunctions.MediaType(Path);
                    Path = Path.Replace("/", "\\");
                    if (Path.IndexOf("http:") == 0 || Path.IndexOf("https:") == 0)
                    {
                        try
                        {
                            switch (MediumType)
                            {
                                case Medium.Image:
                                    string Message = "";
                                    PictureBox.Image = DiversityWorkbench.Forms.FormFunctions.BitmapFromWeb(Path, ref Message);
                                    break;
                                default:
                                    System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)ImageList.Images[0];
                                    PictureBox.Image = bmp;
                                    break;
                            }
                            //Path = Path.Replace("\\", "/");
                            //System.Net.WebRequest webrq = System.Net.WebRequest.Create(Path);
                            //webrq.Timeout = 1;
                            //System.Drawing.Bitmap bmp = (Bitmap)Bitmap.FromStream(webrq.GetResponse().GetResponseStream());
                            //PictureBox.Image = bmp;
                            //webrq.Abort();
                        }
                        catch (System.Exception x)
                        {
                            System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)ImageList.Images[0];
                            PictureBox.Image = bmp;
                        }
                    }
                    else
                    {
                        System.IO.FileInfo File = new System.IO.FileInfo(Path);
                        if (File.Exists)
                        {
                            try
                            {
                                System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(Path);
                                PictureBox.Image = bmp;
                            }
                            catch (System.Exception ex)
                            {
                            }
                        }
                    }
                }
                catch (System.ArgumentException ex)
                {
                    try
                    {
                        //DiversityWorkbench.WebImage W = DiversityWorkbench.Forms.FormFunctions.GetWebImage(Path);
                        //PictureBox.Image = W.Bitmap;

                        System.Net.WebRequest webrq = System.Net.WebRequest.Create(Path);
                        System.Drawing.Bitmap bmp = (Bitmap)Bitmap.FromStream(webrq.GetResponse().GetResponseStream());
                        PictureBox.Image = bmp;
                        webrq.Abort();
                    }
                    catch (System.Exception x)
                    {
                        System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)ImageList.Images[0];
                        PictureBox.Image = bmp;
                    }
                }
            }
        }

        private static System.Drawing.Bitmap MediaBitMap(FormFunctions.Medium Medium)
        {
            DiversityWorkbench.UserControls.UserControlImage uci = new DiversityWorkbench.UserControls.UserControlImage();
            try
            {
                switch (Medium)
                {
                    case Medium.Audio:
                        return (Bitmap)uci.DefaultIconAudio;
                        break;
                    case Medium.Video:
                        return (Bitmap)uci.DefaultIconVideo;
                        break;
                    default:
                        return (Bitmap)uci.DefaultIconImage;
                        break;
                }
            }
            catch { }
            return (Bitmap)uci.DefaultIconImage;
        }

        public static long SizeOfFile(string Path)
        {
            long Size = -1;
            try
            {
                if (Path.IndexOf("http:") == 0)
                {
                    //DiversityWorkbench.WebImage W = DiversityWorkbench.Forms.FormFunctions.GetWebImage(Path);
                    //Size = W.Size;

                    System.Net.WebRequest webrq = System.Net.WebRequest.Create(Path);
                    //webrq.Timeout = 1000;
                    System.Net.WebResponse webResponse = webrq.GetResponse();
                    Size = webResponse.ContentLength;
                    webResponse.Close();
                    webrq.Abort();
                }
                else
                {
                    System.IO.FileInfo File = new System.IO.FileInfo(Path);
                    if (File.Exists)
                    {
                        Size = File.Length;
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return Size;
        }

        /// <summary>
        /// Converting a bitmap into a grayscale image
        /// Source: http://stackoverflow.com/questions/2265910/convert-an-image-to-grayscale
        /// </summary>
        /// <param name="original">the original bitmap</param>
        /// <returns>the grayscale bitmap</returns>
        public static System.Drawing.Bitmap MakeGrayscale3(System.Drawing.Bitmap original)
        {
            //create a blank bitmap the same size as original
            System.Drawing.Bitmap newBitmap = new System.Drawing.Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(
               new float[][]
              {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
              });

            //create some image attributes
            System.Drawing.Imaging.ImageAttributes attributes = new System.Drawing.Imaging.ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new System.Drawing.Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, System.Drawing.GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        #endregion

        #region Menu functions
        /// <summary>
        /// building the menu for editing the data. For predefined entries menuItems are added
        /// </summary>
        public void buildSearchMenu(ref System.Windows.Forms.ToolStripMenuItem M, ref System.Data.DataTable dtApplicationSearchSelectionStrings, System.EventHandler menuItemSearchOnClick)
        {
            try
            {
                if (M.DropDownItems.Count > 0)
                    M.DropDownItems.Clear();
                if (dtApplicationSearchSelectionStrings == null) dtApplicationSearchSelectionStrings = new System.Data.DataTable();
                else dtApplicationSearchSelectionStrings.Clear();
                string SQL = "SELECT SQLStringIdentifier, ItemTable, SQLString, Description FROM ApplicationSearchSelectionStrings_Core ORDER BY SQLStringIdentifier";
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    a.Fill(dtApplicationSearchSelectionStrings);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            if (dtApplicationSearchSelectionStrings.Rows.Count > 0)
            {
                M.Visible = true;
                M.Visible = true;
                try
                {
                    for (int i = 0; i < dtApplicationSearchSelectionStrings.Rows.Count; i++)
                    {
                        string Caption = (i + 1).ToString() + " - " + dtApplicationSearchSelectionStrings.Rows[i]["SQLStringIdentifier"].ToString();
                        System.Windows.Forms.ToolStripMenuItem mi = new System.Windows.Forms.ToolStripMenuItem(Caption, null, menuItemSearchOnClick);
                        mi.ToolTipText = dtApplicationSearchSelectionStrings.Rows[i]["Description"].ToString();
                        M.DropDownItems.Add(mi);
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else
            {
                M.Visible = false;
            }
        }

        #endregion

        #region Autocompletion

        #region Setting autocompletion on demand

        private static System.Windows.Forms.AutoCompleteMode _DefaultAutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
        public static void SetDefaultAutoCompleteMode(System.Windows.Forms.AutoCompleteMode Mode) { _DefaultAutoCompleteMode = Mode; }
        public static System.Windows.Forms.AutoCompleteMode DefaultAutoCompleteMode() { return _DefaultAutoCompleteMode; }

        #region Exclusions

        //private static System.Collections.Generic.List<string> _AutocompletionExcludes;
        //public static void setAutocompletionExcludes(System.Collections.Specialized.StringCollection Excludes)
        //{
        //    if (_AutocompletionExcludes == null)
        //    {
        //        _AutocompletionExcludes = new List<string>();
        //    }
        //    foreach (string E in Excludes)
        //    {
        //        if (!_AutocompletionExcludes.Contains(E))
        //            _AutocompletionExcludes.Add(E);
        //    }
        //}
        //private static bool AutoCompletionExcluded(string Table, string Column)
        //{
        //    if (_AutocompletionExcludes == null)
        //        return false;
        //    else if (_AutocompletionExcludes.Contains(Table + "." + Column))
        //        return true;
        //    return false;
        //}

        private static System.Collections.Generic.List<string> _AutoCompleteStringCollectionExclusions;

        private static bool AutoCompleteExcluded(string Table, string Column)
        {
            if (_AutoCompleteStringCollectionExclusions == null)
                return false;
            else if (_AutoCompleteStringCollectionExclusions.Contains(Table + "." + Column))
                return true;
            return false;
        }


        public static System.Collections.Generic.List<string> AutoCompleteStringCollectionExclusions
        { get { if (_AutoCompleteStringCollectionExclusions == null) _AutoCompleteStringCollectionExclusions = new List<string>(); return _AutoCompleteStringCollectionExclusions; } }

        public static void AutoCompleteStringCollectionExclusionAdd(string Table, string Column)
        {
            if (!AutoCompleteStringCollectionExclusions.Contains(Table + "." + Column))
                AutoCompleteStringCollectionExclusions.Add(Table + "." + Column);
        }

        public static void AutoCompleteStringCollectionExclusionRemove(string Table, string Column)
        {
            if (Table.Length > 0)
                AutoCompleteStringCollectionExclusionRemove(Table + "." + Column);
            else
            {
                System.Collections.Generic.List<string> ToRemove = new List<string>();
                foreach (string Ex in AutoCompleteStringCollectionExclusions)
                {
                    if (Ex.EndsWith("." + Column))
                        ToRemove.Add(Ex);
                }
                foreach (string TR in ToRemove)
                    AutoCompleteStringCollectionExclusions.Remove(TR);
            }
        }

        public static void AutoCompleteStringCollectionExclusionRemove(string Key)
        {
            if (AutoCompleteStringCollectionExclusions.Contains(Key))
                AutoCompleteStringCollectionExclusions.Remove(Key);
        }

        public static void AutoCompleteStringCollectionExclusionClear()
        {
            _AutoCompleteStringCollectionExclusions.Clear();
        }

        #endregion

        #region Setting

        public static void setAutoCompletion(System.Windows.Forms.Form form, bool OnDemand = false)
        {
            foreach (System.Windows.Forms.Control C in form.Controls)
            {
                if (C.GetType() == typeof(System.Windows.Forms.GroupBox)
                    || C.GetType() == typeof(System.Windows.Forms.Panel)
                    || C.GetType() == typeof(System.Windows.Forms.TabControl)
                    || C.GetType() == typeof(System.Windows.Forms.TabPage)
                    || C.GetType() == typeof(System.Windows.Forms.TableLayoutPanel)
                    || C.GetType() == typeof(System.Windows.Forms.SplitterPanel)
                    || C.GetType() == typeof(System.Windows.Forms.SplitContainer))
                    setAutoCompletion(C, OnDemand);
            }
        }

        public static void setAutoCompletion(System.Windows.Forms.Control Cont, bool OnDemand = false, System.Windows.Forms.AutoCompleteMode Mode = System.Windows.Forms.AutoCompleteMode.Suggest)
        {
            foreach (System.Windows.Forms.Control C in Cont.Controls)
            {
                object[] ar = new Object[9] { "", "", "", "", "", "", "", "", "" };
                string Table = "";
                string Column = "";
                string Database = "";
                if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                {
                    try
                    {
                        System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)C;
                        // Markus 22.7.22: for read only fields no autocompletion
                        if (textBox.ReadOnly)
                            continue;
                        C.DataBindings.CopyTo(ar, 0);
                        if (ar[0].ToString() != "")
                        {
                            System.Windows.Forms.BindingMemberInfo bmi = C.DataBindings[0].BindingMemberInfo;
                            Column = bmi.BindingField;
                            Table = bmi.BindingPath;
                            if (Table.Length == 0)
                            {
                                System.Windows.Forms.Binding B = (System.Windows.Forms.Binding)ar[0];
                                if (B.DataSource.GetType() == typeof(System.Windows.Forms.BindingSource))
                                {
                                    System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)B.DataSource;
                                    Table = BS.DataMember.ToString();
                                    Column = B.BindingMemberInfo.BindingField;
                                    Database = DiversityWorkbench.Settings.DatabaseName;
                                }
                                else if (B.DataSource.GetType().BaseType == typeof(System.Data.DataTable))
                                {
                                    Table = B.DataSource.ToString();
                                    Column = B.BindingMemberInfo.BindingField;
                                    Database = DiversityWorkbench.Settings.DatabaseName;
                                }
                            }
                            if (AutoCompleteExcluded(Table, Column))
                                continue;
                            // Markus 19.08.2022 - dauert zu lang
                            //if (textBox.Multiline)
                            //{
                            //    System.Windows.Forms.AutoCompleteStringCollection autoCompleteStringCollection = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionOnDemand(Table, Column);
                            //    addAutocompleteToMultiLineTextbox(ref Cont, ref textBox, autoCompleteStringCollection);
                            //}
                            //else
                            //{
                            if (OnDemand)
                            {
                                _autoCompleteMode = Mode;
                                if (Cont.GetType() == typeof(System.Windows.Forms.Panel) && Cont.Parent.GetType() == typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry))
                                {
                                    DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntry = (DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)Cont;
                                    userControlModuleRelatedEntry.setAutocompleteForTextbox(Mode);
                                }
                                else
                                    textBox.KeyUp += textBoxOnDemandAutocomplete_KeyUp;
#if DEBUG
#else
                                //textBox.KeyUp += textBoxOnDemandAutocomplete_KeyUp;
#endif
                            }
                            else
                            {
                                System.Windows.Forms.AutoCompleteStringCollection autoCompleteStringCollection = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionOnDemand(Table, Column);
                                textBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                                textBox.AutoCompleteMode = Mode;
                                textBox.AutoCompleteCustomSource = autoCompleteStringCollection;
                            }
                            //}
                        }
                    }
                    catch { }
                }
                else if (C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry) && OnDemand)
                {
                    _autoCompleteMode = Mode;
                    DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntry = (DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)C;
                    userControlModuleRelatedEntry.setAutocompleteForTextbox(Mode);
                }
#if DEBUG
#endif
                else if (C.GetType() == typeof(System.Windows.Forms.Panel)
                    || C.GetType() == typeof(System.Windows.Forms.TabControl)
                    || C.GetType() == typeof(System.Windows.Forms.TableLayoutPanel)
                    || C.GetType() == typeof(System.Windows.Forms.SplitContainer)
                    || C.GetType() == typeof(System.Windows.Forms.SplitterPanel)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlLocalList)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlDatePanel)
                    || C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlURI)
                    || C.GetType() == typeof(System.Windows.Forms.GroupBox)
                    || C.GetType() == typeof(System.Windows.Forms.TabPage))
                    setAutoCompletion(C, OnDemand, Mode);
            }
        }

        private static System.Windows.Forms.AutoCompleteMode _autoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;

        private static void textBoxOnDemandAutocomplete_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (sender.GetType() == typeof(System.Windows.Forms.TextBox))
            {
                System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)sender;
                if (textBox.ReadOnly || (textBox.AutoCompleteCustomSource != null && textBox.AutoCompleteCustomSource.Count > 0))
                    return;
                object[] ar = new Object[9] { "", "", "", "", "", "", "", "", "" };
                string Table = "";
                string Column = "";
                string Database = "";
                textBox.DataBindings.CopyTo(ar, 0);
                if (ar[0].ToString() != "")
                {
                    System.Windows.Forms.BindingMemberInfo bmi = textBox.DataBindings[0].BindingMemberInfo;
                    Column = bmi.BindingField;
                    Table = bmi.BindingPath;
                    if (Table.Length == 0)
                    {
                        System.Windows.Forms.Binding B = (System.Windows.Forms.Binding)ar[0];
                        if (B.DataSource.GetType() == typeof(System.Windows.Forms.BindingSource))
                        {
                            System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)B.DataSource;
                            Table = BS.DataMember.ToString();
                            Column = B.BindingMemberInfo.BindingField;
                            Database = DiversityWorkbench.Settings.DatabaseName;
                        }
                        else if (B.DataSource.GetType().BaseType == typeof(System.Data.DataTable))
                        {
                            Table = B.DataSource.ToString();
                            Column = B.BindingMemberInfo.BindingField;
                            Database = DiversityWorkbench.Settings.DatabaseName;
                        }
                    }
                    if (!AutoCompleteStringCollectionExclusions.Contains(Table + "." + Column))
                    {
                        System.Windows.Forms.AutoCompleteStringCollection autoCompleteStringCollection = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionOnDemand(Table, Column);
                        textBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                        textBox.AutoCompleteMode = _autoCompleteMode;
                        textBox.AutoCompleteCustomSource = autoCompleteStringCollection;
                    }
                }
            }
        }

        private static void addAutocompleteToMultiLineTextbox(ref System.Windows.Forms.Control ParentControl, ref System.Windows.Forms.TextBox textBox, System.Windows.Forms.AutoCompleteStringCollection autoCompleteStringCollection)
        {
#if DEBUG
            Forms.TextBoxMultilineAutocomplete textBoxMultilineAutocomplete = new Forms.TextBoxMultilineAutocomplete(ref ParentControl, ref textBox, autoCompleteStringCollection);
#endif
        }

        //private static void textBox_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        //{
        //    System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)sender;
        //    var x = textBox.Left;
        //    var y = textBox.Top + textBox.Height;
        //    var width = textBox.Width + 20;
        //    const int height = 40;

        //    System.Windows.Forms.ListBox listBox = new System.Windows.Forms.ListBox();
        //    listBox.SetBounds(x, y, width, height);
        //    listBox.KeyDown += listBox_SelectedIndexChanged;

        //    //List<string> localList = list.Where(z => z.StartsWith(textBox.Text)).ToList();
        //    //if (localList.Any() && !string.IsNullOrEmpty(textBox.Text))
        //    //{
        //    //    listBox.DataSource = localList;
        //    //    listBox.Show();
        //    //    listBox.Focus();

        //    //}
        //}

        //private static void listBox_SelectedIndexChanged(object sender, System.Windows.Forms.KeyEventArgs e)
        //{
        //    if (e.KeyValue == (decimal)System.Windows.Forms.Keys.Enter)
        //    {
        //        //textBox2.Text = ((System.Windows.Forms.ListBox)sender).SelectedItem.ToString();
        //        //listBox.Hide();
        //    }
        //}

        #endregion

        #region Reset

        public static void resetAutoCompleteStringCollectionOnDemand(string Table = "", string Column = "")
        {
            if (Table.Length > 0 && Column.Length > 0)
            {
                string Key = Table + "." + Column;
                if (_AutoCompleteStringCollections.ContainsKey(Key))
                    _AutoCompleteStringCollections.Remove(Key);
            }
            else
            {
                _AutoCompleteStringCollections.Clear();
            }
        }

        #endregion

        private static System.Collections.Generic.Dictionary<string, System.Windows.Forms.AutoCompleteStringCollection> _AutoCompleteStringCollections;

        public static System.Windows.Forms.AutoCompleteStringCollection AutoCompleteStringCollectionOnDemand(string Table, string Column)
        {
            if (_AutoCompleteStringCollections == null)
                _AutoCompleteStringCollections = new Dictionary<string, System.Windows.Forms.AutoCompleteStringCollection>();
            string Key = Table + "." + Column;
            if (_AutoCompleteStringCollections.ContainsKey(Key))
                return _AutoCompleteStringCollections[Key];
            else if (AutoCompleteStringCollectionExclusions.Contains(Key))
                return null;
            else
            {
                _AutoCompleteStringCollections.Add(Key, AutoCompleteStringCollectionOnDemand(Table, Column, DiversityWorkbench.Settings.ConnectionString));
                return _AutoCompleteStringCollections[Key];
            }
        }

        public static void AutoCompleteStringCollectionOnDemand_Remove(string Key)
        {
            if (_AutoCompleteStringCollections != null && _AutoCompleteStringCollections.ContainsKey(Key))
                _AutoCompleteStringCollections.Remove(Key);
        }

        public static System.Collections.Generic.Dictionary<string, System.Windows.Forms.AutoCompleteStringCollection> AutoCompleteStringCollectionsOnDemand()
        {
            if (_AutoCompleteStringCollections == null)
                _AutoCompleteStringCollections = new Dictionary<string, System.Windows.Forms.AutoCompleteStringCollection>();
            return _AutoCompleteStringCollections;
        }

        private static System.Windows.Forms.AutoCompleteStringCollection AutoCompleteStringCollectionOnDemand(string Table, string Column, string ConnectionString)
        {
            System.Windows.Forms.AutoCompleteStringCollection StringCollection = new System.Windows.Forms.AutoCompleteStringCollection();
            if (ConnectionString.Length == 0)
                return StringCollection;

            if (Column.Length > 0 && Table.Length > 0 && ConnectionString.Length > 0)
            {
                try
                {
                    string TableName = "";
                    if (Table.IndexOf("[") > -1)
                        TableName = Table;
                    else if (Table.IndexOf(".dbo.") > -1)
                    {
                        string[] ssTab = Table.Split(new char[] { '.' });
                        for (int i = 0; i < ssTab.Length; i++)
                            TableName += "[" + ssTab[i] + "].";
                        TableName = TableName.Substring(0, TableName.Length - 1) + " ";
                    }
                    else TableName = " [" + Table + "] ";

                    if (Column.IndexOf(".") > -1) Column = Column.Replace(".", "].[");
                    // Markus 17.7.23: Bugfix missing alias
                    string SQL = "SELECT DISTINCT T.[" + Column + "] FROM " + TableName + " AS T ";
                    if (_AutoCompleteRestrictions != null && _AutoCompleteRestrictions.Count > 0)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in _AutoCompleteRestrictions)
                        {
                            string SqlTest = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = '" + Table + "' AND C.COLUMN_NAME = '" + KV.Key + "' ";
                            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlTest);
                            if (Result == "1")
                            {
                                if (ViewDoesExist(KV.Value))
                                {
                                    SQL += " INNER JOIN [" + KV.Value + "] AS R ON R.[" + KV.Key + "] = T.[" + KV.Key + "]";
                                    break;
                                }
                            }
                            //else if (KV.Value.StartsWith("INNER JOIN"))
                            //{
                            //    SQL += " " + KV.Value;
                            //}
                        }
                    }
                    // Markus 17.7.23: Bugfix missing alias
                    SQL += " ORDER BY T.[" + Column + "]";

                    if (SQL.Length > 0)
                    {
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
                        System.Data.DataTable dt = new System.Data.DataTable();
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            StringCollection.Add(R[0].ToString());
                        }
                    }

                }
                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            }
            return StringCollection;
        }

        private static bool ViewDoesExist(string View)
        {
            string SQL = "select count(*) from information_schema.Views v where v.TABLE_NAME = '" + View + "'";
            return (DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL) == "1");
        }

        #region Restrictions

        private static System.Collections.Generic.Dictionary<string, string> _AutoCompleteRestrictions;
        public static void setAutoCompleteRestriction(string Column, string Restriction)
        {
            if (_AutoCompleteRestrictions == null)
                _AutoCompleteRestrictions = new Dictionary<string, string>();
            if (!_AutoCompleteRestrictions.ContainsKey(Column))
                _AutoCompleteRestrictions.Add(Column, Restriction);
        }

        public static System.Collections.Generic.Dictionary<string, string> getAutoCompleteRestrictions()
        {
            return _AutoCompleteRestrictions;
        }

        #endregion

        #region For UserControlModuleRelatedEntry depending on ServerConnection

        #region ServerConnection

        private static System.Collections.Generic.Dictionary<string, System.Windows.Forms.AutoCompleteStringCollection> _AutoCompleteStringCollections_ForServerConnection;
        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>> _AutoCompleteDict_ForServerConnection;

        public static System.Windows.Forms.AutoCompleteStringCollection AutoCompleteStringCollectionOnDemand(DiversityWorkbench.ServerConnection serverConnection)
        {
            try
            {
                string ConnectionKey = serverConnection.Key();
                if (_AutoCompleteStringCollections_ForServerConnection == null)
                    _AutoCompleteStringCollections_ForServerConnection = new Dictionary<string, System.Windows.Forms.AutoCompleteStringCollection>();
                if (_AutoCompleteStringCollections_ForServerConnection.ContainsKey(ConnectionKey) && _AutoCompleteStringCollections_ForServerConnection[ConnectionKey].Count > 0)
                    return _AutoCompleteStringCollections_ForServerConnection[ConnectionKey];
                else
                {
                    if (_AutoCompleteStringCollections_ForServerConnection.ContainsKey(ConnectionKey))
                        _AutoCompleteStringCollections_ForServerConnection[ConnectionKey] = AutoCompleteStringCollectionOnDemand(ConnectionKey, serverConnection);
                    else
                    {
                        if (!_AutoCompleteStringCollections_ForServerConnection.ContainsKey(ConnectionKey))
                        {
                            _AutoCompleteStringCollections_ForServerConnection.Add(ConnectionKey, AutoCompleteStringCollectionOnDemand(ConnectionKey, serverConnection));
                        }
                    }
                    if (_AutoCompleteStringCollections_ForServerConnection.ContainsKey(ConnectionKey))
                        return _AutoCompleteStringCollections_ForServerConnection[ConnectionKey];
                    else return null;
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return null;
        }

        private static System.Windows.Forms.AutoCompleteStringCollection AutoCompleteStringCollectionOnDemand(string Key, DiversityWorkbench.ServerConnection serverConnection)
        {
            try
            {
                if (_AutoCompleteStringCollections_ForServerConnection != null && _AutoCompleteStringCollections_ForServerConnection.ContainsKey(Key) && _AutoCompleteStringCollections_ForServerConnection[Key].Count > 0)
                    return _AutoCompleteStringCollections_ForServerConnection[Key];
#if DEBUG
                if (_AutoCompleteStringCollections_ForServerConnection == null) // Markus 13.3.2023 - only if null
                    _AutoCompleteStringCollections_ForServerConnection = new Dictionary<string, System.Windows.Forms.AutoCompleteStringCollection>();
                if (_AutoCompleteDict_ForServerConnection == null) // Markus 13.3.2023 - only if null
                    _AutoCompleteDict_ForServerConnection = new Dictionary<string, Dictionary<string, string>>();
#else
            _AutoCompleteStringCollections_ForServerConnection = new Dictionary<string, System.Windows.Forms.AutoCompleteStringCollection>();
            _AutoCompleteDict_ForServerConnection = new Dictionary<string, Dictionary<string, string>>();
#endif
                if (serverConnection != null)
                {
                    if (serverConnection.ConnectionIsValid)
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();

                        string SQL = AutoCompleteStringCollections_SQL(serverConnection);// "";
                        string conStr = serverConnection.ConnectionString;
#if DEBUG
                        if (serverConnection.ConnectionStringModule.Length > 0 && serverConnection.ModuleName != DiversityWorkbench.Settings.ModuleName)
                        {
                            conStr = serverConnection.ConnectionStringModule;
                            SQL = AutoCompleteStringCollections_SQL(serverConnection);
                        }
#endif
                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, conStr);
                        if (!_AutoCompleteStringCollections_ForServerConnection.ContainsKey(Key))
                        {
                            System.Windows.Forms.AutoCompleteStringCollection collection = new System.Windows.Forms.AutoCompleteStringCollection();
                            _AutoCompleteStringCollections_ForServerConnection.Add(Key, collection);
                        }
                        if (!_AutoCompleteDict_ForServerConnection.ContainsKey(Key))
                        {
                            System.Collections.Generic.Dictionary<string, string> dict = new Dictionary<string, string>();
                            _AutoCompleteDict_ForServerConnection.Add(Key, dict);
                        }
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            if (!_AutoCompleteStringCollections_ForServerConnection[Key].Contains(R["DisplayText"].ToString()))
                            {
                                _AutoCompleteStringCollections_ForServerConnection[Key].Add(R["DisplayText"].ToString());
                                _AutoCompleteDict_ForServerConnection[Key].Add(R["DisplayText"].ToString(), R["URI"].ToString());
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            if (_AutoCompleteStringCollections_ForServerConnection.ContainsKey(Key))
                return _AutoCompleteStringCollections_ForServerConnection[Key];
            else return null;
        }

        public static System.Collections.Generic.Dictionary<string, string> AutoCompleteDict_ForServerConnection(string Key)
        {
            if (_AutoCompleteDict_ForServerConnection != null && _AutoCompleteDict_ForServerConnection.ContainsKey(Key))
                return _AutoCompleteDict_ForServerConnection[Key];
            System.Collections.Generic.Dictionary<string, string> dict = new Dictionary<string, string>();
            return dict;
        }

        #endregion

        #region CacheDB

        private static System.Collections.Generic.Dictionary<string, System.Windows.Forms.AutoCompleteStringCollection> _AutoCompleteStringCollections_ForCacheDB;
        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>> _AutoCompleteDict_ForForCacheDB;

        public static System.Windows.Forms.AutoCompleteStringCollection AutoCompleteStringCollectionOnDemand_ForForCacheDB(string CacheDB, string Module, string SourceView)
        {
            DiversityWorkbench.ServerConnection serverConnection = DiversityWorkbench.Settings.ServerConnection;
            serverConnection.CacheDB = CacheDB;
            serverConnection.CacheDBSourceView = SourceView;
            serverConnection.ModuleName = Module;
            string Key = serverConnection.KeyCacheDB();
            if (_AutoCompleteStringCollections_ForCacheDB == null)
                _AutoCompleteStringCollections_ForCacheDB = new Dictionary<string, System.Windows.Forms.AutoCompleteStringCollection>();
            if (_AutoCompleteStringCollections_ForCacheDB.ContainsKey(Key) && _AutoCompleteStringCollections_ForCacheDB[Key].Count > 0)
                return _AutoCompleteStringCollections_ForCacheDB[Key];
            else
            {
                if (_AutoCompleteStringCollections_ForCacheDB.ContainsKey(Key))
                    _AutoCompleteStringCollections_ForCacheDB[Key] = AutoCompleteStringCollectionOnDemand_ForForCacheDB(Module, SourceView, serverConnection);
                else
                {
                    _AutoCompleteStringCollections_ForCacheDB.Add(Key, AutoCompleteStringCollectionOnDemand_ForForCacheDB(Module, SourceView, serverConnection));
                }
                return _AutoCompleteStringCollections_ForCacheDB[Key];
            }
        }

        private static System.Windows.Forms.AutoCompleteStringCollection AutoCompleteStringCollectionOnDemand_ForForCacheDB(string Module, string SourceView, DiversityWorkbench.ServerConnection serverConnection)
        {
            string Key = serverConnection.KeyCacheDB();
            if (_AutoCompleteStringCollections_ForCacheDB != null && _AutoCompleteStringCollections_ForCacheDB.ContainsKey(Key) && _AutoCompleteStringCollections_ForCacheDB[Key].Count > 0)
                return _AutoCompleteStringCollections_ForCacheDB[Key];
            _AutoCompleteStringCollections_ForCacheDB = new Dictionary<string, System.Windows.Forms.AutoCompleteStringCollection>();
            _AutoCompleteDict_ForForCacheDB = new Dictionary<string, Dictionary<string, string>>();
            if (serverConnection != null)
            {
                if (serverConnection.ConnectionIsValid)
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    string SQL = AutoCompleteStringCollections_SQL(serverConnection);// "";
                    string conStr = serverConnection.ConnectionStringCacheDB;
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, conStr);
                    if (!_AutoCompleteStringCollections_ForCacheDB.ContainsKey(Key))
                    {
                        System.Windows.Forms.AutoCompleteStringCollection collection = new System.Windows.Forms.AutoCompleteStringCollection();
                        _AutoCompleteStringCollections_ForCacheDB.Add(Key, collection);
                    }
                    if (!_AutoCompleteDict_ForForCacheDB.ContainsKey(Key))
                    {
                        System.Collections.Generic.Dictionary<string, string> dict = new Dictionary<string, string>();
                        _AutoCompleteDict_ForForCacheDB.Add(Key, dict);
                    }
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        if (!_AutoCompleteStringCollections_ForCacheDB[Key].Contains(R["DisplayText"].ToString()))
                        {
                            _AutoCompleteStringCollections_ForCacheDB[Key].Add(R["DisplayText"].ToString());
                            _AutoCompleteDict_ForForCacheDB[Key].Add(R["DisplayText"].ToString(), R["URI"].ToString());
                        }
                    }
                }
            }
            return _AutoCompleteStringCollections_ForCacheDB[Key];
        }

        public static System.Collections.Generic.Dictionary<string, string> AutoCompleteDict_ForForCacheDB(string Key)
        {
            if (_AutoCompleteDict_ForForCacheDB != null && _AutoCompleteDict_ForForCacheDB.ContainsKey(Key))
                return _AutoCompleteDict_ForForCacheDB[Key];
            System.Collections.Generic.Dictionary<string, string> dict = new Dictionary<string, string>();
            return dict;
        }

        private static string AutoCompleteStringCollections_SQL(DiversityWorkbench.ServerConnection serverConnection)
        {
            string SQL = "";
            bool ForCacheDB = serverConnection.CacheDB.Length > 0 && serverConnection.CacheDBSourceView.Length > 0 && serverConnection.ConnectionStringCacheDB.Length > 0;
            switch (serverConnection.ModuleName)
            {
                case "DiversityAgents":
                    if (ForCacheDB)
                        SQL = "SELECT T.BaseURL + CAST(T.AgentID AS varchar) AS URI, T.AgentName AS DisplayText " +
                            "FROM Agent T " +
                            "WHERE (T.SourceView = '" + serverConnection.CacheDBSourceView + "')";
                    else
                        SQL = "SELECT U.BaseURL + CAST(A.AgentID AS varchar) AS URI, A.AgentName AS DisplayText " +
                            "FROM " + serverConnection.Prefix() + "ViewAgentNames AS A INNER JOIN " +
                            serverConnection.Prefix() + "AgentProject AS P ON A.AgentID = P.AgentID CROSS JOIN " +
                            serverConnection.Prefix() + "ViewBaseURL AS U " +
                            "WHERE(P.ProjectID = " + serverConnection.ProjectID.ToString() + ")";
                    break;
                case "DiversityCollection":
                    SQL = "SELECT U.BaseURL + CAST(C.CollectionSpecimenID AS varchar) AS URI, " +
                        "CASE WHEN C.[AccessionNumber] IS NULL OR  RTRIM(C.[AccessionNumber]) = '' THEN 'ID: ' + CAST(C.[CollectionSpecimenID] AS varchar) ELSE C.[AccessionNumber] END AS DisplayText " +
                        "FROM " + serverConnection.Prefix() + "CollectionSpecimen AS C INNER JOIN " +
                        serverConnection.Prefix() + "CollectionProject AS P ON C.CollectionSpecimenID = P.CollectionSpecimenID CROSS JOIN " +
                        serverConnection.Prefix() + "ViewBaseURL AS U " +
                        "WHERE(P.ProjectID = " + serverConnection.ProjectID.ToString() + ")";
                    break;
                case "DiversityDescriptions":
                    break;
                case "DiversityExsiccatae":
                    SQL = "SELECT U.BaseURL + CAST(T.[ExsiccataID] AS varchar) AS URI, T.ExsAbbreviation AS DisplayText " +
                        "FROM " + serverConnection.Prefix() + "Exsiccata AS T CROSS JOIN " +
                        serverConnection.Prefix() + "ViewBaseURL AS U ";
                    break;
                case "DiversityGazetteer":
                    if (ForCacheDB)
                        SQL = "SELECT T.BaseURL + CAST(T.NameID AS varchar) AS URI, T.Name AS DisplayText " +
                        "FROM Gazetteer T " +
                        "WHERE (T.SourceView = '" + serverConnection.CacheDBSourceView + "')";
                    else
                        SQL = "SELECT U.BaseURL + CAST(G.PreferredNameID AS varchar) AS URI, REPLACE(C.HierarchyCountryToPlace, '|', ', ') AS DisplayText " +
                        "FROM " + serverConnection.Prefix() + "GeoCache AS C INNER JOIN " +
                        serverConnection.Prefix() + "ViewGeoPlace G ON C.PlaceID = G.PlaceID INNER JOIN " +
                        serverConnection.Prefix() + "GeoProject AS P ON G.PreferredNameID = P.NameID CROSS JOIN " +
                        serverConnection.Prefix() + "ViewBaseURL AS U " +
                        "WHERE(P.ProjectID = ";
                    if (serverConnection.ProjectID != null)
                        SQL += serverConnection.ProjectID.ToString();
                    else if (serverConnection.CurrentProjectID != null)
                        SQL += serverConnection.CurrentProjectID.ToString();
                    SQL += ") AND C.HierarchyCountryToPlace <> '' AND C.HierarchyCountryToPlace NOT LIKE '|%'";
                    break;
                case "DiversityProjects":
                    SQL = "SELECT U.BaseURL + CAST(T.ProjectID AS varchar) AS URI, T.Project AS DisplayText " +
                        "FROM " + serverConnection.Prefix() + "Project AS T CROSS JOIN " +
                        serverConnection.Prefix() + "ViewBaseURL AS U ";
                    break;
                case "DiversityReferences":
                    if (ForCacheDB)
                        SQL = "SELECT T.BaseURL + CAST(T.RefID AS varchar) AS URI, T.RefDescription_Cache AS DisplayText " +
                        "FROM ReferenceTitle T " +
                        "WHERE (T.SourceView = '" + serverConnection.CacheDBSourceView + "')";
                    else
                        SQL = "SELECT U.BaseURL + CAST(T.RefID AS varchar) AS URI, T.RefDescription_Cache AS DisplayText " +
                        "FROM " + serverConnection.Prefix() + "ReferenceTitle AS T INNER JOIN " +
                        serverConnection.Prefix() + "ReferenceProject AS P ON T.RefID = P.RefID CROSS JOIN " +
                        serverConnection.Prefix() + "ViewBaseURL AS U " +
                        "WHERE(P.ProjectID = " + serverConnection.ProjectID.ToString() + ")";
                    break;
                case "DiversitySamplingPlots":
                    if (ForCacheDB)
                        SQL = "SELECT T.BaseURL + CAST(T.PlotID AS varchar) AS URI, T.PlotIdentifier AS DisplayText " +
                        "FROM SamplingPlot T " +
                        "WHERE (T.SourceView = '" + serverConnection.CacheDBSourceView + "')";
                    else
                        SQL = "SELECT U.BaseURL + CAST(T.PlotID AS varchar) AS URI, T.PlotIdentifier AS DisplayText " +
                        "FROM " + serverConnection.Prefix() + "ViewSamplingPlot AS T INNER JOIN " +
                        serverConnection.Prefix() + "SamplingProject AS P ON T.PlotID = P.PlotID CROSS JOIN " +
                        serverConnection.Prefix() + "ViewBaseURL AS U " +
                        "WHERE(P.ProjectID = " + serverConnection.ProjectID.ToString() + ")";
                    break;
                case "DiversityScientificTerms":
                    if (ForCacheDB)
                        SQL = "SELECT T.BaseURL + CAST(T.RepresentationID AS varchar) AS URI, T.DisplayText AS DisplayText " +
                        "FROM ScientificTerm T " +
                        "WHERE (T.SourceView = '" + serverConnection.CacheDBSourceView + "')";
                    else
                        SQL = "SELECT U.BaseURL + CAST(T.TermRepresentationID AS varchar) AS URI, T.DisplayText " +
                        "FROM " + serverConnection.Prefix() + "TermRepresentation AS T INNER JOIN " +
                        serverConnection.Prefix() + "Terminology AS P ON T.TerminologyID = P.TerminologyID CROSS JOIN " +
                        serverConnection.Prefix() + "ViewBaseURL AS U " +
                        "WHERE(P.TerminologyID = " + serverConnection.ProjectID.ToString() + ")";
                    break;
                case "DiversityTaxonNames":
                    if (ForCacheDB)
                        SQL = "SELECT T.BaseURL + CAST(T.NameID AS varchar) AS URI, T.TaxonName AS DisplayText " +
                        "FROM TaxonSynonymy T " +
                        "WHERE (T.SourceView = '" + serverConnection.CacheDBSourceView + "')";
                    else
                    {
                        SQL = "SELECT V.BaseURL + CAST(T.NameID AS varchar) AS URI, T.TaxonNameCache AS DisplayText " +
                        "FROM " + serverConnection.Prefix() + "TaxonName AS T  ";
                        if (serverConnection.ProjectID != null)
                        {
                            SQL += " INNER JOIN " +
                            serverConnection.Prefix() + "TaxonNameProject AS P ON T.NameID = P.NameID AND P.ProjectID = " + serverConnection.ProjectID.ToString() + "";
                        }
                        if (serverConnection.SectionID != null)
                        {
                            SQL += " INNER JOIN " +
                            serverConnection.Prefix() + "TaxonNameList AS L ON T.NameID = L.NameID AND L.ProjectID = " + serverConnection.SectionID.ToString() + "";
                        }
                        SQL += " CROSS JOIN " +
                            serverConnection.Prefix() + "ViewBaseURL AS V WHERE T.IgnoreButKeepForReference = 0";
                    }
                    break;
                default:
                    break;
            }

            return SQL;
        }
        #endregion

        #endregion

        #endregion

        #region Not on demand

        public static void setAutoCompletion(System.Windows.Forms.ComboBox ComboBox, System.Windows.Forms.AutoCompleteMode Mode)
        {
            try
            {
                if (ComboBox.DataSource != null)
                {
                    System.Data.DataTable DT = (System.Data.DataTable)ComboBox.DataSource;
                    System.Windows.Forms.AutoCompleteStringCollection StringCollection = new System.Windows.Forms.AutoCompleteStringCollection();
                    foreach (System.Data.DataRow R in DT.Rows)
                        StringCollection.Add(R[ComboBox.DisplayMember].ToString());

                    ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                    ComboBox.AutoCompleteCustomSource = StringCollection;
                    ComboBox.AutoCompleteMode = Mode;
                }
            }
            catch { }
        }

        /// <summary>
        /// Setting the auto completion for combobox and textbox controls. 
        /// Comboboxes with a datasource use these as source.
        /// Textboxes with multiline will not use autocompletion
        /// </summary>
        /// <param name="Control">the control for which the autocompletion should be set</param>
        /// <param name="SearchString">The select statement for the retrieval of the list, if empty a list will be created based on the databindings of the control</param>
        public static void setAutoCompletion(System.Windows.Forms.Control Control, string SearchString)
        {
            try
            {
                if (Control.GetType() == typeof(System.Windows.Forms.ComboBox))
                {
                    System.Windows.Forms.ComboBox C = (System.Windows.Forms.ComboBox)Control;
                    if (C.AutoCompleteCustomSource != null) return;
                    if (C.DataSource != null)
                    {
                        System.Data.DataTable DT = (System.Data.DataTable)C.DataSource;
                        System.Windows.Forms.AutoCompleteStringCollection StringCollection = new System.Windows.Forms.AutoCompleteStringCollection();
                        foreach (System.Data.DataRow R in DT.Rows)
                            StringCollection.Add(R[C.DisplayMember].ToString());

                        C.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                        C.AutoCompleteCustomSource = StringCollection;
                        C.AutoCompleteMode = _DefaultAutoCompleteMode;
                    }
                    else
                    {
                        if (DiversityWorkbench.Settings.QueryLimitDropdownList == 0)
                            return;
                        string SQL = SearchString;
                        if (SQL.Length == 0)
                        {
                            if (C.BindingContext != null)
                            {
                                if (C.DataBindings.Count > 0)
                                {
                                    string Column = C.DataBindings[0].BindingMemberInfo.BindingField;
                                    string Table = "";
                                    if (C.DataBindings[0].BindingManagerBase.Count > 0)
                                    {
                                        System.Data.DataRowView RV = (System.Data.DataRowView)C.DataBindings[0].BindingManagerBase.Current;
                                        Table = RV.Row.Table.TableName;
                                    }
                                    else
                                    {
                                        System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)C.DataBindings[0].DataSource;
                                        Table = BS.DataMember;
                                    }
                                    SQL = "SELECT COUNT(DISTINCT [" + Column + "]) FROM [" + Table + "]";
                                    int i = 0;
                                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                                    Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                                    con.Open();
                                    if (int.TryParse(Com.ExecuteScalar().ToString(), out i))
                                    {
                                        if (i > 0 && i < DiversityWorkbench.Settings.QueryLimitDropdownList)
                                        {
                                            SQL = "SELECT DISTINCT [" + Column + "] FROM [" + Table + "] ORDER BY " + Column;
                                        }
                                    }
                                    con.Close();
                                }
                            }
                        }
                        if (SQL.Length == 0) return;
                        System.Windows.Forms.AutoCompleteStringCollection StringCollection = new System.Windows.Forms.AutoCompleteStringCollection();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        System.Data.DataTable dt = new System.Data.DataTable();
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            StringCollection.Add(R[0].ToString());
                        }
                        C.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                        C.AutoCompleteCustomSource = StringCollection;
                        C.AutoCompleteMode = _DefaultAutoCompleteMode;
                    }
                }
                else if (Control.GetType() == typeof(System.Windows.Forms.TextBox))
                {
                    if (DiversityWorkbench.Settings.QueryLimitDropdownList == 0)
                        return;
                    System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)Control;
                    if (T.AutoCompleteCustomSource != null && T.AutoCompleteCustomSource.Count > 0) return;
                    if (T.Multiline) return;
                    string SQL = SearchString;
                    if (SQL.Length == 0)
                    {
                        if (T.BindingContext != null)
                        {
                            if (T.DataBindings.Count > 0)
                            {
                                string Table = "";
                                string Column = T.DataBindings[0].BindingMemberInfo.BindingField;
                                if (T.DataBindings[0].BindingManagerBase.Count > 0)
                                {
                                    System.Data.DataRowView RV = (System.Data.DataRowView)T.DataBindings[0].BindingManagerBase.Current;
                                    Table = RV.Row.Table.TableName;
                                }
                                else
                                {
                                    System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)T.DataBindings[0].DataSource;
                                    Table = BS.DataMember;
                                }
                                SQL = "SELECT COUNT(DISTINCT [" + Column + "]) FROM [" + Table + "]";
                                int i = 0;
                                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                                con.Open();
                                if (int.TryParse(C.ExecuteScalar()?.ToString(), out i))
                                {
                                    if (i > 0 && i < DiversityWorkbench.Settings.QueryLimitDropdownList)
                                    {
                                        SQL = "SELECT DISTINCT [" + Column + "] FROM [" + Table + "] ORDER BY " + Column;
                                    }
                                }
                                con.Close();
                            }
                        }
                    }
                    if (SQL.Length == 0) return;
                    System.Windows.Forms.AutoCompleteStringCollection StringCollection = new System.Windows.Forms.AutoCompleteStringCollection();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    ad.Fill(dt);
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        StringCollection.Add(R[0].ToString());
                    }
                    T.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                    T.AutoCompleteCustomSource = StringCollection;
                    T.AutoCompleteMode = _DefaultAutoCompleteMode;
                }
            }
            catch { }
        }

        /// <summary>
        /// Setting the auto completion for comboboxes. 
        /// Comboboxes with a datasource use these as source.
        /// Autocompletion is set to SuggestAppend
        /// </summary>
        /// <param name="ComboBox">the ComboBox for which the autocompletion should be set</param>
        public static void setAutoCompletion(System.Windows.Forms.ComboBox ComboBox)
        {
            try
            {
                if (ComboBox.AutoCompleteCustomSource != null && ComboBox.AutoCompleteCustomSource.Count > 0) return;
                if (ComboBox.DataSource != null)
                {
                    System.Windows.Forms.AutoCompleteStringCollection StringCollection = new System.Windows.Forms.AutoCompleteStringCollection();
                    if (typeof(System.Data.DataTable) == ComboBox.DataSource.GetType())
                    {
                        System.Data.DataTable DT = (System.Data.DataTable)ComboBox.DataSource;
                        foreach (System.Data.DataRow R in DT.Rows)
                            StringCollection.Add(R[ComboBox.DisplayMember].ToString());
                    }
                    else if (typeof(System.Windows.Forms.BindingSource) == ComboBox.DataSource.GetType())
                    {
                        System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)ComboBox.DataSource;
                        if (typeof(System.Data.DataSet) == BS.DataSource.GetType().BaseType)
                        {
                            System.Data.DataSet DS = (System.Data.DataSet)BS.DataSource;
                            System.Data.DataTable DT = DS.Tables[BS.DataMember];
                            foreach (System.Data.DataRow R in DT.Rows)
                                StringCollection.Add(R[ComboBox.DisplayMember].ToString());

                        }
                    }
                    if (StringCollection.Count > 0)
                    {
                        ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                        ComboBox.AutoCompleteCustomSource = StringCollection;
                        ComboBox.AutoCompleteMode = _DefaultAutoCompleteMode;
                    }
                }
                else
                {
                    if (ComboBox.BindingContext != null)
                    {
                        if (ComboBox.DataBindings.Count > 0)
                        {
                            string Column = ComboBox.DataBindings[0].BindingMemberInfo.BindingField;
                            string Table = "";
                            if (ComboBox.DataBindings[0].BindingManagerBase.Count > 0)
                            {
                                System.Data.DataRowView RV = (System.Data.DataRowView)ComboBox.DataBindings[0].BindingManagerBase.Current;
                                Table = RV.Row.Table.TableName;
                            }
                            else
                            {
                                System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)ComboBox.DataBindings[0].DataSource;
                                Table = BS.DataMember;
                            }
                            if (Column.Length > 0 && Table.Length > 0)
                            {
                                ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                                ComboBox.AutoCompleteCustomSource = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollection(Table, Column);
                                ComboBox.AutoCompleteMode = _DefaultAutoCompleteMode;
                            }
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Setting the auto completion for comboboxes. 
        /// Comboboxes with a datasource use these as source.
        /// Autocompletion is set to SuggestAppend
        /// </summary>
        /// <param name="ComboBox">the ComboBox for which the autocompletion should be set</param>
        /// <param name="Datatable">the Datatable used as source for the autocompletion</param>
        public static void setAutoCompletion(System.Windows.Forms.ComboBox ComboBox, System.Data.DataTable Datatable)
        {
            try
            {
                if (ComboBox.AutoCompleteCustomSource != null && ComboBox.AutoCompleteCustomSource.Count > 0) return;
                if (ComboBox.DataSource != null)
                {
                    System.Windows.Forms.AutoCompleteStringCollection StringCollection = new System.Windows.Forms.AutoCompleteStringCollection();
                    if (typeof(System.Data.DataTable) == ComboBox.DataSource.GetType())
                    {
                        System.Data.DataTable DT = (System.Data.DataTable)ComboBox.DataSource;
                        foreach (System.Data.DataRow R in DT.Rows)
                            StringCollection.Add(R[ComboBox.DisplayMember].ToString());
                    }
                    else if (typeof(System.Windows.Forms.BindingSource) == ComboBox.DataSource.GetType())
                    {
                        System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)ComboBox.DataSource;
                        if (typeof(System.Data.DataSet) == BS.DataSource.GetType().BaseType)
                        {
                            System.Data.DataSet DS = (System.Data.DataSet)BS.DataSource;
                            System.Data.DataTable DT = DS.Tables[BS.DataMember];
                            foreach (System.Data.DataRow R in DT.Rows)
                                StringCollection.Add(R[ComboBox.DisplayMember].ToString());

                        }
                    }
                    if (StringCollection.Count > 0)
                    {
                        ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                        ComboBox.AutoCompleteCustomSource = StringCollection;
                        ComboBox.AutoCompleteMode = _DefaultAutoCompleteMode;
                    }
                }
                else
                {
                    if (ComboBox.BindingContext != null)
                    {
                        if (ComboBox.DataBindings.Count > 0)
                        {
                            string Column = ComboBox.DataBindings[0].BindingMemberInfo.BindingField;
                            string Table = "";
                            if (ComboBox.DataBindings[0].BindingManagerBase.Count > 0)
                            {
                                System.Data.DataRowView RV = (System.Data.DataRowView)ComboBox.DataBindings[0].BindingManagerBase.Current;
                                Table = RV.Row.Table.TableName;
                            }
                            else
                            {
                                System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)ComboBox.DataBindings[0].DataSource;
                                Table = BS.DataMember;
                            }
                            if (Column.Length > 0 && Table.Length > 0)
                            {
                                ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                                ComboBox.AutoCompleteCustomSource = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollection(Table, Column);
                                ComboBox.AutoCompleteMode = _DefaultAutoCompleteMode;
                            }
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Setting the auto completion for comboboxes. 
        /// Comboboxes with a datasource use these as source.
        /// Autocompletion is set to SuggestAppend
        /// </summary>
        /// <param name="ComboBox">the ComboBox for which the autocompletion should be set</param>
        /// <param name="ConnectionString">the string for connecting to the database</param>
        public static void setAutoCompletion(System.Windows.Forms.ComboBox ComboBox, string ConnectionString)
        {
            try
            {
                if (ComboBox.AutoCompleteCustomSource != null && ComboBox.AutoCompleteCustomSource.Count > 0) return;
                if (ComboBox.DataSource != null)
                {
                    System.Windows.Forms.AutoCompleteStringCollection StringCollection = new System.Windows.Forms.AutoCompleteStringCollection();
                    if (typeof(System.Data.DataTable) == ComboBox.DataSource.GetType())
                    {
                        System.Data.DataTable DT = (System.Data.DataTable)ComboBox.DataSource;
                        foreach (System.Data.DataRow R in DT.Rows)
                            StringCollection.Add(R[ComboBox.DisplayMember].ToString());
                    }
                    else if (typeof(System.Windows.Forms.BindingSource) == ComboBox.DataSource.GetType())
                    {
                        System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)ComboBox.DataSource;
                        if (typeof(System.Data.DataSet) == BS.DataSource.GetType().BaseType)
                        {
                            System.Data.DataSet DS = (System.Data.DataSet)BS.DataSource;
                            System.Data.DataTable DT = DS.Tables[BS.DataMember];
                            foreach (System.Data.DataRow R in DT.Rows)
                                StringCollection.Add(R[ComboBox.DisplayMember].ToString());

                        }
                    }
                    if (StringCollection.Count > 0)
                    {
                        ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                        ComboBox.AutoCompleteCustomSource = StringCollection;
                        ComboBox.AutoCompleteMode = _DefaultAutoCompleteMode;
                    }
                }
                else
                {
                    if (ComboBox.BindingContext != null)
                    {
                        if (ComboBox.DataBindings.Count > 0)
                        {
                            string Column = ComboBox.DataBindings[0].BindingMemberInfo.BindingField;
                            string Table = "";
                            if (ComboBox.DataBindings[0].BindingManagerBase.Count > 0)
                            {
                                System.Data.DataRowView RV = (System.Data.DataRowView)ComboBox.DataBindings[0].BindingManagerBase.Current;
                                Table = RV.Row.Table.TableName;
                            }
                            else
                            {
                                System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)ComboBox.DataBindings[0].DataSource;
                                Table = BS.DataMember;
                            }
                            if (Column.Length > 0 && Table.Length > 0)
                            {
                                ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                                ComboBox.AutoCompleteCustomSource = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollection(Table, Column, ConnectionString);
                                ComboBox.AutoCompleteMode = _DefaultAutoCompleteMode;
                            }
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Setting the auto completion for combobox and textbox controls. 
        /// Textboxes with multiline will not use autocompletion
        /// </summary>
        /// <param name="TextBox">the TextBox for which the autocompletion should be set</param>
        public static void setAutoCompletion(System.Windows.Forms.TextBox TextBox)
        {
            try
            {
                if (TextBox.AutoCompleteCustomSource != null && TextBox.AutoCompleteCustomSource.Count > 0) return;
                if (TextBox.Multiline) return;
                if (TextBox.BindingContext != null)
                {
                    if (TextBox.DataBindings.Count > 0)
                    {
                        string Table = "";
                        string Column = TextBox.DataBindings[0].BindingMemberInfo.BindingField;
                        if (TextBox.DataBindings[0].BindingManagerBase != null && TextBox.DataBindings[0].BindingManagerBase.Count > 0)
                        {
                            System.Data.DataRowView RV = (System.Data.DataRowView)TextBox.DataBindings[0].BindingManagerBase.Current;
                            Table = RV.Row.Table.TableName;
                        }
                        else
                        {
                            System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)TextBox.DataBindings[0].DataSource;
                            Table = BS.DataMember;
                        }
                        if (Column.Length > 0 && Table.Length > 0)
                        {
                            TextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                            TextBox.AutoCompleteCustomSource = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollection(Table, Column);
                            TextBox.AutoCompleteMode = _DefaultAutoCompleteMode;
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Setting the auto completion for combobox and textbox controls. 
        /// </summary>
        /// <param name="TextBox">the TextBox for which the autocompletion should be set</param>
        /// <param name="Table">the name of the table</param>
        /// <param name="Column">the name of the column</param>
        public static void setAutoCompletion(System.Windows.Forms.TextBox TextBox, string Table, string Column)
        {
            try
            {
                if (TextBox.AutoCompleteCustomSource != null &&
                    TextBox.AutoCompleteCustomSource.Count > 0)
                    return;
                if (TextBox.Multiline) return;
                if (Column.Length > 0 && Table.Length > 0)
                {
                    TextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                    TextBox.AutoCompleteCustomSource = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollection(Table, Column);
                    TextBox.AutoCompleteMode = _DefaultAutoCompleteMode;
                }
            }
            catch { }
        }

        /// <summary>
        /// Setting the auto completion for textbox controls. 
        /// </summary>
        /// <param name="MinCharacterCount">The minimal number of charaters necessary to create the list</param>
        /// <param name="TextBox">the TextBox for which the autocompletion should be set</param>
        /// <param name="Table">the name of the table</param>
        /// <param name="Column">the name of the column</param>
        /// <param name="ConnectionString">the string for connecting to the database</param>
        public static void setAutoCompletion(int MinCharacterCount, System.Windows.Forms.TextBox TextBox, string Table, string Column, string ConnectionString)
        {
            try
            {
                if (TextBox.Text.Length < MinCharacterCount)
                    return;
                if (TextBox.Multiline) return;
                if (Column.Length > 0 && Table.Length > 0)
                {
                    TextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                    TextBox.AutoCompleteCustomSource = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollection(Table, Column, ConnectionString);
                    //TextBox.AutoCompleteCustomSource = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollection(TextBox.Text, Table, Column, ConnectionString);
                    TextBox.AutoCompleteMode = _DefaultAutoCompleteMode;
                }
            }
            catch { }
        }
        /// <summary>
        /// Setting the auto completion for textbox 
        /// </summary>
        /// <param name="TextBox">the TextBox for which the autocompletion should be set</param>
        /// <param name="Table">the name of the table</param>
        /// <param name="Column">the name of the column</param>
        /// <param name="ConnectionString">the string for connecting to the database</param>
        public static void setAutoCompletion(System.Windows.Forms.TextBox TextBox, string Table, string Column, string ConnectionString)
        {
            try
            {
                if (TextBox.AutoCompleteCustomSource != null && TextBox.AutoCompleteCustomSource.Count > 0) return;
                if (TextBox.Multiline) return;
                if (Column.Length > 0 && Table.Length > 0)
                {
                    TextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                    TextBox.AutoCompleteCustomSource = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollection(Table, Column, ConnectionString);
                    TextBox.AutoCompleteMode = _DefaultAutoCompleteMode;
                }
            }
            catch { }
        }

        /// <summary>
        /// Setting the auto completion for textbox 
        /// </summary>
        /// <param name="TextBox">the TextBox for which the autocompletion should be set</param>
        /// <param name="Table">the name of the table</param>
        /// <param name="Column">the name of the column</param>
        /// <param name="Restriction">the restriction of the query</param>
        /// <param name="ConnectionString">the string for connecting to the database</param>
        public static void setAutoCompletion(System.Windows.Forms.TextBox TextBox, string Table, string Column, string ConnectionString, string Restriction)
        {
            try
            {
                if (TextBox.AutoCompleteCustomSource != null && TextBox.AutoCompleteCustomSource.Count > 0) return;
                if (TextBox.Multiline) return;
                if (Column.Length > 0 && Table.Length > 0)
                {
                    TextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                    TextBox.AutoCompleteCustomSource = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollection(Table, Column, ConnectionString, Restriction);
                    TextBox.AutoCompleteMode = _DefaultAutoCompleteMode;
                }
            }
            catch { }
        }

        /// <summary>
        /// Getting an AutoCompleteStringCollection for a control if the number of entries does not exeed the limit (DiversityWorkbench.Settings.QueryLimitDropdownList)
        /// </summary>
        /// <param name="Table">The table in the database</param>
        /// <param name="Column">the column in the table</param>
        /// <returns>An AutoCompleteStringCollection</returns>
        private static System.Windows.Forms.AutoCompleteStringCollection AutoCompleteStringCollection(string Table, string Column)
        {
            return DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollection(Table, Column, DiversityWorkbench.Settings.ConnectionString);
        }

        /// <summary>
        /// Getting an AutoCompleteStringCollection for a control if the number of entries does not exeed 1000
        /// </summary>
        /// <param name="Table">The table in the database</param>
        /// <param name="Column">the column in the table</param>
        /// <param name="ConnectionString">the string for connecting to the database</param>
        /// <returns>An AutoCompleteStringCollection</returns>
        private static System.Windows.Forms.AutoCompleteStringCollection AutoCompleteStringCollection(string Table, string Column, string ConnectionString)
        {
            System.Windows.Forms.AutoCompleteStringCollection StringCollection = new System.Windows.Forms.AutoCompleteStringCollection();
            if (DiversityWorkbench.Settings.QueryLimitDropdownList == 0)
                return StringCollection;
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0) return StringCollection;
            if (Column.IndexOf(".") > -1) Column = Column.Replace(".", "].[");
            string SQL = "SELECT COUNT(DISTINCT [" + Column + "]) FROM ";
            if (Table.IndexOf("[") > -1) SQL += Table;
            else if (Table.IndexOf(".dbo.") > -1)
            {
                string[] ssTab = Table.Split(new char[] { '.' });
                for (int i = 0; i < ssTab.Length; i++)
                    SQL += "[" + ssTab[i] + "].";
                SQL = SQL.Substring(0, SQL.Length - 1) + " ";
            }
            else SQL += " [" + Table + "]";
            if (Column.Length > 0 && Table.Length > 0 && ConnectionString.Length > 0)
            {
                try
                {
                    int i = 0;
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    con.Open();
                    if (int.TryParse(C.ExecuteScalar()?.ToString(), out i))
                    {
                        if (i > 0 && i < DiversityWorkbench.Settings.QueryLimitDropdownList)
                        {
                            SQL = "SELECT DISTINCT [" + Column + "] FROM ";
                            if (Table.IndexOf("[") > -1) SQL += Table;
                            else if (Table.IndexOf(".dbo.") > -1)
                            {
                                string[] ssTab = Table.Split(new char[] { '.' });
                                for (int ii = 0; ii < ssTab.Length; ii++)
                                    SQL += "[" + ssTab[ii] + "].";
                                SQL = SQL.Substring(0, SQL.Length - 1) + " ";
                            }
                            else SQL += " [" + Table + "]";
                            SQL += " ORDER BY " + Column;
                        }
                        else SQL = "";
                    }
                    con.Close();
                    if (SQL.Length > 0)
                    {
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
                        System.Data.DataTable dt = new System.Data.DataTable();
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            StringCollection.Add(R[0].ToString());
                        }
                    }

                }
                catch { }
            }
            return StringCollection;
        }

        /// <summary>
        /// Getting an AutoCompleteStringCollection for a control if the number of entries does not exeed 1000
        /// </summary>
        /// <param name="Table">The table in the database</param>
        /// <param name="Column">the column in the table</param>
        /// <param name="ConnectionString">the string for connecting to the database</param>
        /// <returns>An AutoCompleteStringCollection</returns>
        //private static System.Windows.Forms.AutoCompleteStringCollection AutoCompleteStringCollection(string StartString, string Table, string Column, string ConnectionString)
        //{
        //    System.Windows.Forms.AutoCompleteStringCollection StringCollection = new System.Windows.Forms.AutoCompleteStringCollection();
        //    if (DiversityWorkbench.Settings.QueryLimitDropdownList == 0)
        //        return StringCollection;
        //    if (DiversityWorkbench.Settings.ConnectionString.Length == 0) return StringCollection;
        //    if (Column.IndexOf(".") > -1) Column = Column.Replace(".", "].[");
        //    string SQL = "SELECT COUNT(DISTINCT [" + Column + "]) FROM ";
        //    if (Table.IndexOf("[") > -1) SQL += Table;
        //    else if (Table.IndexOf(".dbo.") > -1)
        //    {
        //        string[] ssTab = Table.Split(new char[] { '.' });
        //        for (int i = 0; i < ssTab.Length; i++)
        //            SQL += "[" + ssTab[i] + "].";
        //        SQL = SQL.Substring(0, SQL.Length - 1) + " ";
        //    }
        //    else SQL += " [" + Table + "]";
        //    if (Column.Length > 0 && Table.Length > 0 && ConnectionString.Length > 0)
        //    {
        //        try
        //        {
        //            int i = 0;
        //            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
        //            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //            con.Open();
        //            if (int.TryParse(C.ExecuteScalar()?.ToString(), out i))
        //            {
        //                if (i > 0 && i < DiversityWorkbench.Settings.QueryLimitDropdownList)
        //                {
        //                    SQL = "SELECT DISTINCT [" + Column + "] FROM ";
        //                    if (Table.IndexOf("[") > -1) SQL += Table;
        //                    else if (Table.IndexOf(".dbo.") > -1)
        //                    {
        //                        string[] ssTab = Table.Split(new char[] { '.' });
        //                        for (int ii = 0; ii < ssTab.Length; ii++)
        //                            SQL += "[" + ssTab[ii] + "].";
        //                        SQL = SQL.Substring(0, SQL.Length - 1) + " ";
        //                    }
        //                    else SQL += " [" + Table + "]";
        //                    SQL += " ORDER BY " + Column;
        //                }
        //                else SQL = "";
        //            }
        //            con.Close();
        //            if (SQL.Length > 0)
        //            {
        //                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
        //                System.Data.DataTable dt = new System.Data.DataTable();
        //                ad.Fill(dt);
        //                foreach (System.Data.DataRow R in dt.Rows)
        //                {
        //                    StringCollection.Add(R[0].ToString());
        //                }
        //            }

        //        }
        //        catch { }
        //    }
        //    return StringCollection;
        //}

        /// <summary>
        /// Getting an AutoCompleteStringCollection for a control if the number of entries does not exeed 1000
        /// </summary>
        /// <param name="Table">The table in the database</param>
        /// <param name="Column">the column in the table</param>
        /// <param name="ConnectionString">the string for connecting to the database</param>
        /// <param name="Restriction">The restriction of the query</param>
        /// <returns>An AutoCompleteStringCollection</returns>
        private static System.Windows.Forms.AutoCompleteStringCollection AutoCompleteStringCollection(string Table, string Column, string ConnectionString, string Restriction)
        {
            System.Windows.Forms.AutoCompleteStringCollection StringCollection = new System.Windows.Forms.AutoCompleteStringCollection();
            if (DiversityWorkbench.Settings.QueryLimitDropdownList == 0)
                return StringCollection;
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0) return StringCollection;
            if (Column.IndexOf(".") > -1) Column = Column.Replace(".", "].[");
            string SQL = "SELECT COUNT(DISTINCT [" + Column + "]) FROM ";
            if (Table.IndexOf("[") > -1) SQL += Table;
            else if (Table.IndexOf(".dbo.") > -1)
            {
                string[] ssTab = Table.Split(new char[] { '.' });
                for (int i = 0; i < ssTab.Length; i++)
                    SQL += "[" + ssTab[i] + "].";
                SQL = SQL.Substring(0, SQL.Length - 1) + " ";
            }
            else SQL += " [" + Table + "]";
            if (Restriction.Length > 0)
                SQL += " WHERE " + Restriction;
            if (Column.Length > 0 && Table.Length > 0 && ConnectionString.Length > 0)
            {
                try
                {
                    int i = 0;
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    con.Open();
                    if (int.TryParse(C.ExecuteScalar()?.ToString(), out i))
                    {
                        if (i > 0 && i < DiversityWorkbench.Settings.QueryLimitDropdownList)
                        {
                            SQL = "SELECT DISTINCT [" + Column + "] FROM ";
                            if (Table.IndexOf("[") > -1) SQL += Table;
                            else if (Table.IndexOf(".dbo.") > -1)
                            {
                                string[] ssTab = Table.Split(new char[] { '.' });
                                for (int ii = 0; ii < ssTab.Length; ii++)
                                    SQL += "[" + ssTab[ii] + "].";
                                SQL = SQL.Substring(0, SQL.Length - 1) + " ";
                            }
                            else SQL += " [" + Table + "]";
                            if (Restriction.Length > 0)
                                SQL += " WHERE " + Restriction;
                            SQL += " ORDER BY " + Column;
                        }
                        else SQL = "";
                    }
                    con.Close();
                    if (SQL.Length > 0)
                    {
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
                        System.Data.DataTable dt = new System.Data.DataTable();
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            StringCollection.Add(R[0].ToString());
                        }
                    }

                }
                catch { }
            }
            return StringCollection;
        }

        #endregion

        #endregion

        #region SQL

        public static int SqlServerVersion()
        {
            Microsoft.Data.SqlClient.SqlConnection sqlConnection = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.SqlServer.Management.Smo.Server server = new Microsoft.SqlServer.Management.Smo.Server(new Microsoft.SqlServer.Management.Common.ServerConnection(sqlConnection));
            return server.VersionMajor;
        }

        public static int? AgentID
        {
            get
            {
                try
                {
                    string SQL = "SELECT AgentURI FROM UserProxy WHERE LoginName = User_Name()";
                    string AgentURI = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    int AgentID = -1;
                    if (AgentURI.Length > 0)
                    {
                        string BaseURIAgents = DiversityWorkbench.WorkbenchUnit.getIDFromURI(AgentURI);
                        if (!int.TryParse(BaseURIAgents, out AgentID))
                            AgentID = int.Parse(AgentURI.Substring(BaseURIAgents.Length));
                        return AgentID;
                    }
                    else return null;
                }
                catch (System.Exception ex) { }
                return null;
            }
        }

        public static bool SqlExecuteNonQuery(string SqlCommand, bool UseDefaultConnection = false)
        {
            bool OK = true;
            try
            {
                using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString))
                {
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlCommand, con);
                    C.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                    con.Open();
                    C.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SqlCommand);
                OK = false;
            }
            finally
            {
                //if (con != null && !UseDefaultConnection)
                //{
                //    con.Close();
                //    con.Dispose();
                //}
            }
            if (!OK && SqlCommand.IndexOf("\\") > -1)
            {
                //string text = SqlCommand.Replace("\\K","\K");

            }
            return OK;
        }


        public static async Task<bool> SqlExecuteNonQueryAsync(string sqlCommand, bool useDefaultConnection = false) 
        { 
            bool ok = true; 
            try 
            { 
                using (SqlConnection con = new SqlConnection(DiversityWorkbench.Settings.ConnectionString)) 
                { 
                    SqlCommand cmd = new SqlCommand(sqlCommand, con); 
                    cmd.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase; 
                    await con.OpenAsync(); 
                    await cmd.ExecuteNonQueryAsync(); 
                    await con.CloseAsync(); 
                } 
            } 
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, sqlCommand); ok = false; }
            return ok;
        }


        public static bool SqlExecuteNonQuery(string SqlCommand, bool? IgnoreException)
        {
            bool OK = true;
            try
            {
                using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString))
                {
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlCommand, con);
                    C.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                    con.Open();
                    C.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (System.Exception ex)
            {
                if (IgnoreException == null || !(bool)IgnoreException)
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SqlCommand);
                OK = false;
            }
            finally
            {
                //if (con != null && !UseDefaultConnection)
                //{
                //    con.Close();
                //    con.Dispose();
                //}
            }
            if (!OK && SqlCommand.IndexOf("\\") > -1)
            {
                //string text = SqlCommand.Replace("\\K","\K");

            }
            return OK;
        }

        public static bool SqlExecuteNonQuery(string SqlCommand, string ConnectionString)
        {
            bool OK = true;
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            try
            {
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlCommand, con);
                C.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                con.Open();
                C.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SqlCommand);
                OK = false;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            if (!OK && SqlCommand.IndexOf("\\") > -1)
            {
                //string text = SqlCommand.Replace("\\K","\K");

            }
            return OK;
        }

        public static bool SqlExecuteNonQuery(string SqlCommand, Microsoft.Data.SqlClient.SqlConnection con)
        {
            bool OK = true;
            if (con == null)
                return false;
            try
            {
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlCommand, con);
                C.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                C.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SqlCommand);
                OK = false;
            }
            finally
            {
                //con.Close();
                //con.Dispose();
            }
            if (!OK && SqlCommand.IndexOf("\\") > -1)
            {
                //string text = SqlCommand.Replace("\\K","\K");

            }
            return OK;
        }

        public static bool SqlExecuteNonQuery(string SqlCommand, string ConnectionString, ref string Message)
        {
            bool OK = true;
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            try
            {
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlCommand, con);
                C.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                con.Open();
                C.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SqlCommand);
                Message = ex.Message;
                OK = false;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            if (!OK && SqlCommand.IndexOf("\\") > -1)
            {
            }
            return OK;
        }

        public static bool SqlExecuteNonQuery(string SqlCommand, ref string Message, ref int ErrorCode)
        {
            bool OK = true;
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            try
            {
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlCommand, con);
                C.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                con.Open();
                C.ExecuteNonQuery();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                OK = false;
                Message = ex.Message;
                ErrorCode = ex.Number;
            }
            catch (System.Exception ex) { OK = false; }
            finally
            {
                con.Close();
                con.Dispose();
            }
            if (!OK && SqlCommand.IndexOf("\\") > -1)
            {
                //string text = SqlCommand.Replace("\\K","\K");

            }
            return OK;
        }

        public static bool SqlExecuteNonQuery(string SqlCommand, ref string ExceptionMessage)
        {
            bool OK = true;
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            try
            {
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlCommand, con);
                con.Open();
                C.ExecuteNonQuery();
            }
            catch (Microsoft.Data.SqlClient.SqlException exS)
            {
                if (exS.Procedure != "sp_addextendedproperty")
                {
                    if (ExceptionMessage.Length > 0)
                        ExceptionMessage += "\r\n";
                    ExceptionMessage += exS.Message + "\r\nSQL-Statement:\r\n" + SqlCommand + "\r\n\r\n";
                }
                OK = false;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return OK;
        }

        public static bool SqlExecuteNonQuery(string SqlCommand, ref string ExceptionMessage, int Timeout)
        {
            bool OK = true;
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout(Timeout));
            try
            {
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlCommand, con);
                C.CommandTimeout = Timeout;
                con.Open();
                C.ExecuteNonQuery();
            }
            catch (Microsoft.Data.SqlClient.SqlException exS)
            {
                if (exS.Procedure != "sp_addextendedproperty")
                    ExceptionMessage = exS.Message + "\r\nSQL-Statement:\r\n" + SqlCommand + "\r\n\r\n";
                OK = false;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return OK;
        }

        public static bool SqlExecuteNonQuery(string SqlCommand, Microsoft.Data.SqlClient.SqlConnection Connection, ref string Message, ref int ErrorCode)
        {
            bool OK = true;
            try
            {
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlCommand, Connection);
                C.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                if (Connection.State == System.Data.ConnectionState.Closed) Connection.Open();
                C.ExecuteNonQuery();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                OK = false;
                Message = ex.Message;
                ErrorCode = ex.Number;
            }
            catch (System.Exception ex) { OK = false; }
            return OK;
        }

        public static bool SqlExecuteNonQuery(string SqlCommand, Microsoft.Data.SqlClient.SqlConnection Connection, ref string Message)
        {
            bool OK = true;
            try
            {
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlCommand, Connection);
                C.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                if (Connection.State == System.Data.ConnectionState.Closed) Connection.Open();
                C.ExecuteNonQuery();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                OK = false;
                Message = ex.Message;
            }
            catch (System.Exception ex) { Message = ex.Message; OK = false; }
            return OK;
        }


        public static string SqlExecuteScalar(string SqlCommand, bool IgnoreException = false)
        {
            string Result = "";
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0)
                return Result;
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            try
            {
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlCommand, con);
                C.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                con.Open();
                Result = C.ExecuteScalar()?.ToString() ?? "";
            }
            catch (System.Exception ex)
            {
                if (!IgnoreException)
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SqlCommand);
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Result;
        }

        public static string SqlExecuteScalar(string SqlCommand, string ConnectionString)
        {
            string Result = "";
            if (ConnectionString.Length == 0)
                return Result;
            // MW 27.2.23: using statement to ensure correct disposal
            try
            {
                using (Microsoft.Data.SqlClient.SqlConnection c = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString))
                {
                    using (Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlCommand, c))
                    {
                        c.Open();
                        C.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                        Result = C.ExecuteScalar()?.ToString() ?? "";
                        c.Close();
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            finally
            {
            }
            return Result;
        }

        public static string SqlExecuteScalar(string SqlCommand, Microsoft.Data.SqlClient.SqlConnection Connection)
        {
            string Result = "";
            try
            {
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlCommand, Connection);
                C.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                if (Connection.State == System.Data.ConnectionState.Closed)
                    Connection.Open();
                Result = C.ExecuteScalar()?.ToString() ?? "";
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            finally
            {
            }
            return Result;
        }




        public static string SqlExecuteScalar(string SqlCommand, string ConnectionString, ref string Message)
        {
            string Result = "";
            if (ConnectionString.Length == 0)
                return Result;
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            try
            {
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlCommand, con);
                C.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                con.Open();
                Result = C.ExecuteScalar()?.ToString() ?? "";
            }
            catch (System.Exception ex) { Message = ex.Message; }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Result;
        }

        public static string SqlExecuteScalar(string SqlCommand, ref string ExceptionMessage, bool IncludeNullReferenceException = false, bool UseConnectionTimeout = false)
        {
            string Result = "";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlCommand, con);
            if (UseConnectionTimeout)
                C.CommandTimeout = DiversityWorkbench.Settings.TimeoutConnection;
            else
                C.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
            try
            {
                con.Open();
                Result = C.ExecuteScalar()?.ToString() ?? "";
            }
            catch (Microsoft.Data.SqlClient.SqlException exS)
            {
                if (exS.Procedure != "sp_addextendedproperty")
                    ExceptionMessage = exS.Message + "\r\nSQL-Statement:\r\n" + SqlCommand + "\r\n\r\n";
            }
            catch (System.NullReferenceException ex)
            {
                // Markus 17.6.2021 - wird für einige Faelle notwendig
                if (IncludeNullReferenceException)
                    ExceptionMessage = ex.Message + "\r\nSQL-Statement:\r\n" + SqlCommand + "\r\n\r\n";
            }
            catch (System.Exception ex)
            {
                ExceptionMessage += ex.Message + "\r\nSQL-Statement:\r\n" + SqlCommand + "\r\n\r\n";
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Result;
        }

        public static string SqlExecuteScalar(string SqlCommand, ref string ExceptionMessage, System.Data.IsolationLevel Level)
        {
            string Result = "";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
            try
            {
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlCommand, con);
                C.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                con.Open();
                Microsoft.Data.SqlClient.SqlTransaction T = con.BeginTransaction(Level);
                C.Transaction = T;
                Result = C.ExecuteScalar()?.ToString() ?? "";
                T.Commit();
            }
            catch (Microsoft.Data.SqlClient.SqlException exS)
            {
                if (exS.Procedure != "sp_addextendedproperty")
                    ExceptionMessage = exS.Message + "\r\nSQL-Statement:\r\n" + SqlCommand + "\r\n\r\n";
            }
            catch (System.NullReferenceException)
            { }
            catch (System.Exception ex)
            {
                ExceptionMessage = ex.Message + "\r\nSQL-Statement:\r\n" + SqlCommand + "\r\n\r\n";
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Result;
        }


        /// <summary>
        /// Filling a table with data
        /// </summary>
        /// <param name="SqlCommand">The command for filling the table</param>
        /// <param name="DT">The table that should be filled</param>
        /// <param name="ExceptionMessage">The message if any exception should happen</param>
        /// <param name="Timeout">an optional timeout, if null the application settings will be used</param>
        /// <returns></returns>
        public static bool SqlFillTable(string SqlCommand, ref System.Data.DataTable DT, ref string ExceptionMessage, int? Timeout = null)
        {
            bool OK = false;
            if (DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase).Length > 0)
            {
                Microsoft.Data.SqlClient.SqlConnection con;
                if (Timeout != null)
                    con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout((int)Timeout));
                else
                    con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                try
                {
                    if (con.State == System.Data.ConnectionState.Closed && con.State != System.Data.ConnectionState.Connecting)
                        con.Open();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCommand, con);
                    if (Timeout != null)
                        ad.SelectCommand.CommandTimeout = (int)Timeout;
                    else
                        ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                    ad.Fill(DT);
                    OK = true;
                }
                catch (Microsoft.Data.SqlClient.SqlException exS)
                {
                    if (exS.Procedure != "sp_addextendedproperty")
                        ExceptionMessage = exS.Message + "\r\nSQL-Statement:\r\n" + SqlCommand + "\r\n\r\n";
                }
                catch (System.NullReferenceException)
                { }
                catch (System.Exception ex)
                {
                    ExceptionMessage = ex.Message + "\r\nSQL-Statement:\r\n" + SqlCommand + "\r\n\r\n";
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return OK;
        }

        public static bool SqlFillTable(string SqlCommand, ref System.Data.DataTable DT, string ConnectionString = "")
        {
            bool OK = false;
            if (ConnectionString.Length == 0)
                ConnectionString = DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase);
            if (ConnectionString.Length > 0)
            {
                try
                {
                    using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString))
                    {
                        con.Open();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCommand, con);
                        ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                        ad.Fill(DT);
                        OK = true;
                        con.Close();
                    }
                }
                catch (Microsoft.Data.SqlClient.SqlException exS)
                {
                    if (exS.Procedure != "sp_addextendedproperty")
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(exS, SqlCommand);
                }
                catch (System.NullReferenceException)
                { }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SqlCommand);
                }
                finally
                {
                    //con.Close();
                    //con.Dispose();
                }
            }
            return OK;
        }

        public static bool SqlFillTable(string SqlCommand, ref System.Data.DataTable DT, Microsoft.Data.SqlClient.SqlConnection con, bool CloseConnection = false)
        {
            bool OK = false;
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCommand, con);
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(DT);
                OK = true;
            }
            catch (Microsoft.Data.SqlClient.SqlException exS)
            {
                if (exS.Procedure != "sp_addextendedproperty")
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(exS, SqlCommand);
            }
            catch (System.NullReferenceException)
            { }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SqlCommand);
            }
            finally
            {
                if (CloseConnection)
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return OK;
        }

        public static string SqlRemoveHyphens(string SqlContent)
        {
            string S = SqlContent.Replace("'", "' + CHAR(39) + '");
            return S;
        }

        public static bool SqlDatabaseReadOnly()
        {
            bool Result = true;
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
            try
            {
                string SqlCommand = "SELECT is_read_only FROM sys.databases WHERE name = '" + DiversityWorkbench.Settings.DatabaseName + "'";
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlCommand, con);
                con.Open();
                Result = (bool)C.ExecuteScalar();
            }
            catch (System.Exception ex)
            {
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Result;
        }


        #region Connections: Using only one connection for every connection string to reduce number of open connections

        public static int ProcessID()
        {
            System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
            int id = p.Id;
            return id;
        }


        private static System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, Microsoft.Data.SqlClient.SqlConnection>> _Connections;

        //public static Microsoft.Data.SqlClient.SqlConnection Connection(string ConnectionString = "")
        //{
        //    try
        //    {
        //        if (ConnectionString.Length == 0)
        //            ConnectionString = Settings.ConnectionString;
        //        if (_Connections == null)
        //            _Connections = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<string, Microsoft.Data.SqlClient.SqlConnection>>();
        //        if (ConnectionString.Length > 0)
        //        {
        //            if (!_Connections.ContainsKey(ProcessID()))
        //            {
        //                System.Collections.Generic.Dictionary<string, Microsoft.Data.SqlClient.SqlConnection> dict = new Dictionary<string, Microsoft.Data.SqlClient.SqlConnection>();
        //                _Connections.Add(ProcessID(), dict);
        //            }
        //            if (!_Connections[ProcessID()].ContainsKey(ConnectionString))
        //            {
        //                Microsoft.Data.SqlClient.SqlConnection sqlConnection = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
        //                _Connections[ProcessID()].Add(ConnectionString, sqlConnection);
        //            }
        //            if (_Connections[ProcessID()].ContainsKey(ConnectionString))
        //            {
        //                // Bugfix for disposed connections
        //                if(_Connections[ProcessID()][ConnectionString].ConnectionString.Length == 0)
        //                {
        //                    _Connections[ProcessID()][ConnectionString].ConnectionString = ConnectionString;
        //                }
        //                if (_Connections[ProcessID()][ConnectionString].State == System.Data.ConnectionState.Closed)
        //                    _Connections[ProcessID()][ConnectionString].Open();
        //                return _Connections[ProcessID()][ConnectionString];
        //            }
        //        }
        //        else return null;
        //    }
        //    catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //    return null;
        //}

        public static System.Data.DataTable DataTable(string SQL, string ConnectionString = "", string TableName = "")
        {
            System.Data.DataTable dt = new System.Data.DataTable(TableName);
            try
            {
                if (ConnectionString.Length == 0)
                    ConnectionString = Settings.ConnectionString;
                using (Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString))
                {
                    ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                    ad.Fill(dt);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return dt;
        }

        public static System.Data.DataTable DataTable(string SQL, Microsoft.Data.SqlClient.SqlConnection sqlConnection, string TableName)
        {
            System.Data.DataTable dt = new System.Data.DataTable(TableName);
            try
            {
                using (Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, sqlConnection))
                {
                    ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                    ad.Fill(dt);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return dt;
        }

        #endregion

        #endregion

        #region Conversions

        public static string NumberToRoman(int number)
        {
            if (number < 0 || number > 3999)
            {
                throw new ArgumentException("Value must be in the range 0 – 3,999.");
            }
            if (number == 0) return "N";

            int[] values = new int[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
            string[] numerals = new string[] { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };

            StringBuilder result = new StringBuilder();

            // Loop through each of the values to diminish the number
            for (int i = 0; i < 13; i++)
            {

                // If the number being converted is less than the test value, append
                // the corresponding numeral or numeral pair to the resultant string
                while (number >= values[i])
                {
                    number -= values[i];
                    result.Append(numerals[i]);
                }

            }
            return result.ToString();
        }

        #endregion

        #region TreeView

        #region XML

        public static void BuildTreeFromXmlContent(System.Windows.Forms.TreeView TreeView, string XmlContent)
        {
            try
            {
                TreeView.Nodes.Clear();
                System.Xml.XmlTextReader tr = new System.Xml.XmlTextReader(XmlContent, System.Xml.XmlNodeType.Element, null);
                System.Xml.XmlDocument dom = new System.Xml.XmlDocument();
                dom.Load(tr);
                if (dom.DocumentElement == null) return;
                if (dom.DocumentElement.ChildNodes.Count >= 0)
                {
                    // SECTION 2. Initialize the TreeView control.
                    TreeView.Nodes.Clear();
                    //if (dom.DocumentElement.Name.Length > 0)
                    TreeView.Nodes.Add(new System.Windows.Forms.TreeNode(dom.DocumentElement.Name));
                    System.Windows.Forms.TreeNode tNode = new System.Windows.Forms.TreeNode();
                    tNode = TreeView.Nodes[0];
                    // SECTION 3. Populate the TreeView with the DOM nodes.
                    DiversityWorkbench.Forms.FormFunctions.AddNodeFromXmlContent(dom.DocumentElement, tNode);
                    TreeView.ExpandAll();
                }
                else
                {
                    if (dom.DocumentElement.Attributes["error_message"].InnerText.Length > 0)
                    {
                        string Message = dom.DocumentElement.Attributes["error_message"].InnerText;
                        System.Windows.Forms.MessageBox.Show(Message);
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        private static void AddNodeFromXmlContent(System.Xml.XmlNode inXmlNode, System.Windows.Forms.TreeNode inTreeNode)
        {
            System.Xml.XmlNode xNode;
            System.Windows.Forms.TreeNode tNode;
            System.Xml.XmlNodeList nodeList;
            int i;

            // Loop through the XML nodes until the leaf is reached.
            // Add the nodes to the TreeView during the looping process.
            try
            {
                if (inXmlNode.HasChildNodes)
                {
                    nodeList = inXmlNode.ChildNodes;
                    for (i = 0; i <= nodeList.Count - 1; i++)
                    {
                        xNode = inXmlNode.ChildNodes[i];
                        string TreeNodeName = xNode.Name;
                        string TreeNodeValue = xNode.Value;
                        if (xNode.Attributes != null && xNode.Attributes.Count > 0)
                        {
                            System.Xml.XmlAttribute attrName = (System.Xml.XmlAttribute)(xNode.Attributes.GetNamedItem("Name"));
                            if (attrName != null)
                                TreeNodeName = attrName.Value;
                            System.Xml.XmlAttribute attrValue = (System.Xml.XmlAttribute)(xNode.Attributes.GetNamedItem("Value"));
                            if (attrValue != null)
                            {
                                TreeNodeValue = attrValue.Value;
                                TreeNodeName += ": " + TreeNodeValue;
                            }
                        }
                        inTreeNode.Nodes.Add(new System.Windows.Forms.TreeNode(TreeNodeName));
                        if (i >= inTreeNode.Nodes.Count)
                            continue;
                        tNode = inTreeNode.Nodes[i];
                        AddNodeFromXmlContent(xNode, tNode);
                    }
                }
                else
                {
                    if (inXmlNode.Value != null)
                    {
                        DiversityWorkbench.UserControls.XMLNode N = new DiversityWorkbench.UserControls.XMLNode();
                        if (inXmlNode.Name == "#text")
                            N.Name = inXmlNode.ParentNode.Name;
                        else
                            N.Name = inXmlNode.Name;
                        string TreeNodeValue = inXmlNode.Value;
                        if (inXmlNode.Attributes != null && inXmlNode.Attributes.Count > 0)
                        {
                            System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute)(inXmlNode.Attributes.GetNamedItem("Value"));
                            if (attr != null)
                                TreeNodeValue = attr.Value;
                        }
                        N.Value = inXmlNode.Value;
                        inTreeNode.Parent.Tag = N;
                        inTreeNode.Parent.Text = inTreeNode.Parent.Text + ": " + inXmlNode.Value;
                        inTreeNode.Remove();
                    }
                }
            }
            catch (System.Exception ex) { }

        }

        public static string XmlFromTree(System.Windows.Forms.TreeView TreeView)
        {
            System.IO.FileInfo XmlFile = new System.IO.FileInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Temp.xml");

            string XML = "";
            if (TreeView.Nodes.Count == 0) return "";
            System.Xml.XmlDocument Doc = new System.Xml.XmlDocument();
            System.Xml.XmlWriter W;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            W = System.Xml.XmlWriter.Create(XmlFile.FullName, settings);
            W.WriteStartElement(TreeView.Nodes[0].Text);
            foreach (System.Windows.Forms.TreeNode N in TreeView.Nodes[0].Nodes)
            {
                XmlFromTreeAddChild(N, ref W);
            }
            W.WriteEndElement();
            W.Flush();
            W.Close();
            System.IO.StreamReader R = new System.IO.StreamReader(XmlFile.FullName);
            XML = R.ReadToEnd();
            R.Close();
            R.Dispose();
            XmlFile.Delete();
            return XML;
        }

        private static bool XmlFromTreeAddChild(System.Windows.Forms.TreeNode TreeNode, ref System.Xml.XmlWriter W)
        {
            try
            {
                if (TreeNode.Tag != null)
                {
                    try
                    {
                        UserControls.XMLNode N = (UserControls.XMLNode)TreeNode.Tag;
                        W.WriteElementString(N.Name, N.Value);
                    }
                    catch { }
                }
                else
                {
                    string Node = TreeNode.Text.Replace(" ", "_");
                    if (Node.IndexOf(":") > -1)
                        Node = Node.Substring(0, Node.IndexOf(":"));
                    W.WriteStartElement(Node);
                    foreach (System.Windows.Forms.TreeNode NChild in TreeNode.Nodes)
                    {
                        if (!XmlFromTreeAddChild(NChild, ref W))
                            return false;
                    }
                    W.WriteEndElement();
                }
                return true;
            }
            catch (System.Exception ex) { return false; }
        }

        #endregion

        #region JSON

        public static void JsonToTreeView(System.Windows.Forms.TreeView TreeView, string JSON)
        {
            try
            {
                System.Collections.Generic.Dictionary<string, object> D = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(JSON);
                foreach (System.Collections.Generic.KeyValuePair<string, object> KVroot in D)
                {
                    System.Windows.Forms.TreeNode Nroot = new System.Windows.Forms.TreeNode(KVroot.Key);
                    TreeView.Nodes.Add(Nroot);
                    if (KVroot.Value.GetType() == typeof(object[]))
                    {
                        object[] ooChild = (object[])KVroot.Value;
                        for (int i = 0; i < ooChild.Length; i++)
                        {
                            DiversityWorkbench.Forms.FormFunctions.JsonToTreeChildNodes(Nroot, ooChild[i]);
                        }
                    }
                    else
                    {
                        System.Windows.Forms.TreeNode NC = new System.Windows.Forms.TreeNode(KVroot.Value.ToString());
                        Nroot.Nodes.Add(NC);
                    }
                }

                TreeView.ExpandAll();
            }
            catch (System.Exception ex)
            {
            }
        }

        private static void JsonToTreeChildNodes(System.Windows.Forms.TreeNode ParentNode, object O)
        {
            try
            {
                if (O.GetType() == typeof(System.Collections.Generic.Dictionary<string, object>))
                {
                    System.Collections.Generic.Dictionary<string, object> D = (System.Collections.Generic.Dictionary<string, object>)O;
                    foreach (System.Collections.Generic.KeyValuePair<string, object> KVroot in D)
                    {
                        System.Windows.Forms.TreeNode Nroot = new System.Windows.Forms.TreeNode(KVroot.Key);
                        ParentNode.Nodes.Add(Nroot);
                        if (KVroot.Value.GetType() == typeof(object[]))
                        {
                            object[] ooChild = (object[])KVroot.Value;
                            for (int i = 0; i < ooChild.Length; i++)
                            {
                                DiversityWorkbench.Forms.FormFunctions.JsonToTreeChildNodes(Nroot, ooChild[i]);
                            }
                        }
                        else
                        {
                            System.Windows.Forms.TreeNode NC = new System.Windows.Forms.TreeNode(KVroot.Value.ToString());
                            Nroot.Nodes.Add(NC);
                        }
                    }
                }
                else
                {
                    System.Windows.Forms.TreeNode Nchild = new System.Windows.Forms.TreeNode(O.ToString());
                    ParentNode.Nodes.Add(Nchild);
                }
            }
            catch (System.Exception ex)
            {
            }
        }
        
        public static void JsonToTable(ref System.Data.DataTable dtValues, string JSON, System.Collections.Generic.Dictionary<string, string> ColumnMapping)
        {
            try
            {
                System.Collections.Generic.Dictionary<string, object> D = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(JSON);
                    foreach (System.Collections.Generic.KeyValuePair<string, object> KVroot in D)
                    {
                        if (KVroot.Value.GetType() == typeof(object[]))
                        {
                            object[] ooChild = (object[])KVroot.Value;
                            for (int i = 0; i < ooChild.Length; i++)
                            {
                                if (ooChild[i].GetType() == typeof(System.Collections.Generic.Dictionary<string, object>))
                                {
                                    System.Collections.Generic.Dictionary<string, object> Dchild = (System.Collections.Generic.Dictionary<string, object>)ooChild[i];
                                }

                            }
                        }
                        else
                        {
                        }
                    }
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Loops

        public static System.Collections.Generic.List<int> LoopsInHierarchy(System.Collections.Generic.Dictionary<int, int?> ChildParentRelation)
        {
            System.Collections.Generic.Dictionary<int, int?> dict = ChildParentRelation;
            System.Collections.Generic.List<int> list = new List<int>();
            int Count = 0;
            LoopingNodes(ref list, ref Count, dict);
            System.Collections.Generic.List<int> loops = new List<int>();
            if (dict.Count > list.Count)
            {
                foreach (System.Collections.Generic.KeyValuePair<int, int?> KV in dict)
                {
                    if (!list.Contains(KV.Key))
                        loops.Add(KV.Key);
                }
            }
            return loops;
        }

        private static void LoopingNodes(ref System.Collections.Generic.List<int> list, ref int Count, System.Collections.Generic.Dictionary<int, int?> dict)
        {
            int Start = Count;
            foreach (System.Collections.Generic.KeyValuePair<int, int?> KV in dict)
            {
                if (KV.Value == null && !list.Contains(KV.Key))
                {
                    list.Add(KV.Key);
                    Count++;
                }
            }
            foreach (System.Collections.Generic.KeyValuePair<int, int?> KV in dict)
            {
                if (KV.Value != null && !list.Contains(KV.Key) && list.Contains((int)KV.Value))
                {
                    list.Add(KV.Key);
                    Count++;
                }
            }
            if (list.Count == dict.Count)
                return;
            if (Start == Count)
                return;
            LoopingNodes(ref list, ref Count, dict);
        }


        #endregion

        #endregion

        #region System settings

        private static float? _DisplayZoomFactor;
        public static float DisplayZoomFactor
        {
            get
            {
                if (_DisplayZoomFactor == null)
                {
                    Graphics graphics;
                    System.Windows.Forms.UserControl U = new System.Windows.Forms.UserControl();
                    graphics = U.CreateGraphics();
                    var dpi = graphics.DpiX;
                    _DisplayZoomFactor = dpi / 96.0f;
                    if (dpi == 96)
                        _DisplayZoomFactor = 1;
                    else if (dpi == 120)
                        _DisplayZoomFactor = (float)1.25;
                    else if (dpi == 144)
                        _DisplayZoomFactor = (float)1.5;
                }
                return (float)_DisplayZoomFactor;
            }
        }

        public static int DiplayZoomCorrected(int Value)
        {
            return (int)((float)Value * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
        }

        private static string CheckFor45PlusVersion(int releaseKey)
        {
            var release = new SortedDictionary<int, string>()
            {
                { 378389, "4.5" },
                { 378675, "4.5.1" }, { 379893, "4.5.2" },
                { 393295, "4.6" },
                { 394254, "4.6.1" }, { 394802, "4.6.2" },
                { 460798, "4.7" },
                { 461308, "4.7.1" }, { 461808, "4.7.2" },
                { 528040, "4.8 or later" }
            };
            int result = -1;
            foreach (var k in release)
            {
                if (k.Key <= releaseKey) result = k.Key; else break;
            };
            return (result > 0) ? release[result] : "No 4.5 or later version detected";
        }

        public static string Get45or451FromRegistry()
        {
            string Version = "";
            using (Microsoft.Win32.RegistryKey ndpKey = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
            {
                int releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));
                if (true)
                {
                    Version = CheckFor45PlusVersion(releaseKey);
                }
            }
            return Version;
        }


        #endregion

        #region Update

        public static string WikiDownloadPath(string Module)
        {
            System.Security.Cryptography.MD5.Create();
            string URI = "http://diversityworkbench.net/Portal/" + Module;
            try
            {
                URI = "http://diversityworkbench.net/w/media/";
                string FileName = DiversityWorkbench.Forms.FormFunctions.LatestClientVersion;
                if (FileName.StartsWith("0")) FileName = FileName.Substring(1);
                FileName = FileName.Replace(".0", ".");
                FileName = FileName.Replace(".", "_");
                FileName = Module + "_" + FileName + ".zip";
                string Hash = CreateMD5Hash(FileName);
                URI += Hash.Substring(0, 1).ToLower() + "/" + Hash.Substring(0, 2).ToLower() + "/" + FileName;
            }
            catch (System.Exception ex) { }
            return URI;
        }


        private static string _LatestClientVersion = "";
        public static string LatestClientVersion
        {
            get
            {
                if (_LatestClientVersion.Length == 0)
                {
                    string SQL = "select dbo.VersionClient()";
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    try
                    {
                        con.Open();
                        _LatestClientVersion = C.ExecuteScalar()?.ToString() ?? string.Empty;
                        con.Close();
                    }
                    catch { }
                }
                return _LatestClientVersion;
            }
        }

        public static string CreateMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2")); 
            }
            return sb.ToString();
        }



        #endregion

        #region Grid

        public static void DrawRowNumber(System.Windows.Forms.DataGridView dataGridView, System.Drawing.Font Font, System.Windows.Forms.DataGridViewRowPostPaintEventArgs e)
        {
            DrawRowNumber(dataGridView, Font, e, false);
            //try
            //{
            //    string rowNumber =
            //            (e.RowIndex + 1).ToString()
            //            .PadLeft(dataGridView.RowCount.ToString().Length);

            //    // Schriftgröße:
            //    System.Drawing.SizeF size = e.Graphics.MeasureString(rowNumber, Font);

            //    // Breite des ZeilenHeaders anpassen:
            //    if (dataGridView.RowHeadersWidth < (int)(size.Width + 20))
            //        dataGridView.RowHeadersWidth = (int)(size.Width + 20);

            //    // ZeilenNr zeichnen:
            //    e.Graphics.DrawString(
            //        rowNumber,
            //        Font,
            //        System.Drawing.SystemBrushes.ControlText,
            //        e.RowBounds.Location.X + 15,
            //        e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
            //}
            //catch (Exception ex)
            //{
            //}
            return;
        }

        public static void DrawRowNumber(System.Windows.Forms.DataGridView dataGridView, System.Drawing.Font Font, System.Windows.Forms.DataGridViewRowPostPaintEventArgs e, bool RowheaderWidthFixed)
        {
            try
            {
                string rowNumber =
                        (e.RowIndex + 1).ToString()
                        .PadLeft(dataGridView.RowCount.ToString().Length);

                // Schriftgröße:
                System.Drawing.SizeF size = e.Graphics.MeasureString(rowNumber, Font);

                // Breite des ZeilenHeaders anpassen:
                if (!RowheaderWidthFixed)
                {
                    if (dataGridView.RowHeadersWidth < (int)(size.Width + 20))
                        dataGridView.RowHeadersWidth = (int)(size.Width + 20);
                }

                // ZeilenNr zeichnen:
                e.Graphics.DrawString(
                    rowNumber,
                    Font,
                    System.Drawing.SystemBrushes.ControlText,
                    e.RowBounds.Location.X + 15,
                    e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
            }
            catch (Exception ex)
            {
            }
            return;
        }

        #endregion

        #region Clipboard and grid

        public static bool CanCopyClipboardInDataGrid(int IndexTopRow, System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues
            , System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums
            , System.Windows.Forms.DataGridView dataGridView)
        {
            bool OK = true;
            try
            {
                string Message = CheckNumberOfRowsToCopyClipboard(IndexTopRow, ClipBoardValues, dataGridView);
                Message += CheckNumberOfColumnsToCopyClipboard(ClipBoardValues, GridColums);
                Message += CheckTypeOfColumnsToCopyClipboard(GridColums);
                Message = Message.Trim();
                if (Message.Length > 0)
                {
                    OK = false;
                    System.Windows.Forms.MessageBox.Show(Message);
                }
            }
            catch { OK = false; }
            return OK;
        }

        private static string CheckNumberOfRowsToCopyClipboard(int IndexTopRow, System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues, System.Windows.Forms.DataGridView dataGridView)
        {
            string Message = "";
            int iAllowAdding = 0;
            if (!dataGridView.AllowUserToAddRows)
                iAllowAdding = 1;
            if (dataGridView.Rows.Count + iAllowAdding <= IndexTopRow + ClipBoardValues.Count)
                Message = "You try to copy " + ClipBoardValues.Count.ToString() + " rows into " + (dataGridView.Rows.Count - IndexTopRow - 1 + iAllowAdding).ToString() + " available row(s).\r\n" +
                    "Please reduce you selection.\r\n\r\n";
            return Message;
        }

        private static string CheckNumberOfColumnsToCopyClipboard(System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues, System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums)
        {
            string Message = "";
            if (GridColums.Count < ClipBoardValues[0].Count)
                Message = "You try to copy " + ClipBoardValues[0].Count.ToString() + " columns into " + (GridColums.Count).ToString() + " available column(s).\r\n" +
                    "Please reduce your selection.\r\n\r\n";
            return Message;
        }

        private static string CheckTypeOfColumnsToCopyClipboard(System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums)
        {
            string Message = "";
            foreach (System.Windows.Forms.DataGridViewColumn GridColum in GridColums)
            {
                if (GridColum.CellType == typeof(System.Windows.Forms.DataGridViewButtonCell)
                    && (GridColum.HeaderText.StartsWith("Link to ")
                    || GridColum.HeaderText.StartsWith("Remove link to ")))
                    Message += GridColum.HeaderText + "\r\n";
            }
            if (Message.Length > 0)
                Message = "The following columns can not be changed via the clipboard:\r\n" + Message + "Please hide the columns from the grid or reduce your selection\r\n\r\n";
            return Message;
        }

        public static System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues
        {
            get
            {
                // parsing the content of the clipboard
                System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues = new List<List<string>>();
                string[] stringSeparators = new string[] { "\r\n" };
                string[] lineSeparators = new string[] { "\t" };
                string ClipBoardText = System.Windows.Forms.Clipboard.GetText();
                string[] ClipBoardList = ClipBoardText.Split(stringSeparators, StringSplitOptions.None);
                for (int i = 0; i < ClipBoardList.Length; i++)
                {
                    System.Collections.Generic.List<string> ClipBoardValueStrings = new List<string>();
                    string[] ClipBoardListStrings = ClipBoardList[i].Split(lineSeparators, StringSplitOptions.None);
                    for (int ii = 0; ii < ClipBoardListStrings.Length; ii++)
                        ClipBoardValueStrings.Add(ClipBoardListStrings[ii]);
                    ClipBoardValues.Add(ClipBoardValueStrings);
                }
                if ((ClipBoardValues[0].Count > ClipBoardValues[ClipBoardList.Length - 1].Count ||
                    ClipBoardValues[0].Count == 1) &&
                    ClipBoardValues[ClipBoardList.Length - 1][0].Length == 0)
                    ClipBoardValues.Remove(ClipBoardValues[ClipBoardList.Length - 1]);
                return ClipBoardValues;
            }
        }

        public static void WriteToClipboard(System.Windows.Forms.DataGridView Grid)
        {
            System.Windows.Forms.Clipboard.Clear();
            string Text = "";
            try
            {
                if (Grid.SelectedCells.Count > 0)
                {
                    // Check if there are more than 1 Column
                    int InitialColumn = Grid.SelectedCells[0].ColumnIndex;
                    foreach (System.Windows.Forms.DataGridViewCell C in Grid.SelectedCells)
                    {
                        if (C.ColumnIndex != InitialColumn)
                        {
                            System.Windows.Forms.MessageBox.Show("Only one column can be selected");
                            return;
                        }
                    }

                    // Check if there are gaps in the selection
                    int InitialRow = Grid.SelectedCells[0].RowIndex;
                    foreach (System.Windows.Forms.DataGridViewCell C in Grid.SelectedCells)
                    {
                        if (System.Math.Sign(C.RowIndex - InitialRow) * (C.RowIndex - InitialRow) > 1)
                        {
                            System.Windows.Forms.MessageBox.Show("Only blocks can be selected");
                            return;
                        }
                        InitialRow = C.RowIndex;
                    }

                    int rStart;
                    int rEnd;
                    if (Grid.SelectedCells[0].RowIndex < Grid.SelectedCells[Grid.SelectedCells.Count - 1].RowIndex)
                    {
                        rStart = Grid.SelectedCells[0].RowIndex;
                        rEnd = Grid.SelectedCells[Grid.SelectedCells.Count - 1].RowIndex;
                    }
                    else
                    {
                        rEnd = Grid.SelectedCells[0].RowIndex;
                        rStart = Grid.SelectedCells[Grid.SelectedCells.Count - 1].RowIndex;
                    }
                    int cStart;
                    int cEnd;
                    if (Grid.SelectedCells[0].ColumnIndex < Grid.SelectedCells[Grid.SelectedCells.Count - 1].ColumnIndex)
                    {
                        cStart = Grid.SelectedCells[0].ColumnIndex;
                        cEnd = Grid.SelectedCells[Grid.SelectedCells.Count - 1].ColumnIndex;
                    }
                    else
                    {
                        cEnd = Grid.SelectedCells[0].ColumnIndex;
                        cStart = Grid.SelectedCells[Grid.SelectedCells.Count - 1].ColumnIndex;
                    }
                    for (int r = rStart; r <= rEnd; r++)
                    {
                        if (Text.Length > 0)
                            Text += "\r\n";
                        for (int c = cStart; c <= cEnd; c++)
                        {
                            if (Text.Length > 0 && !Text.EndsWith("\r\n"))
                                Text += "\t";
                            if (Grid.Rows[r].Cells[c].Value != null)
                            {
                                string Value = Grid.Rows[r].Cells[c].Value.ToString();
                                Value = Value.Replace("\r\n", "");
                                Value = Value.Replace("\t", "");
                                Text += Value;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            System.Windows.Forms.Clipboard.SetText(Text, System.Windows.Forms.TextDataFormat.UnicodeText);
        }

        //public static void WriteToClipboardWithIndex(System.Windows.Forms.DataGridView Grid)
        //{
        //    System.Windows.Forms.Clipboard.Clear();
        //    string Text = "";
        //    try
        //    {
        //        if (Grid.SelectedCells.Count > 0)
        //        {
        //            // Check if there are more than 1 Column
        //            int InitialColumn = Grid.SelectedCells[0].ColumnIndex;
        //            foreach(System.Windows.Forms.DataGridViewCell C in Grid.SelectedCells)
        //            {
        //                if (C.ColumnIndex != InitialColumn)
        //                {
        //                    System.Windows.Forms.MessageBox.Show("Only one column can be selected");
        //                    return;
        //                }
        //            }
        //            System.Collections.Generic.SortedDictionary<int, string> Values = new SortedDictionary<int, string>();
        //            foreach (System.Windows.Forms.DataGridViewCell C in Grid.SelectedCells)
        //            {
        //                Values.Add(C.ColumnIndex, C.Value.ToString());
        //            }
        //            foreach (System.Collections.Generic.KeyValuePair<int, string> KV in Values)
        //            {
        //                if (Text.Length > 0)
        //                    Text += "\r\n";
        //                string Value = KV.Value;
        //                Value = Value.Replace("\r\n", "");
        //                Value = Value.Replace("\t", "");
        //                Text += KV.Key + "\t" + Value;
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //    }
        //    System.Windows.Forms.Clipboard.SetText(Text, System.Windows.Forms.TextDataFormat.UnicodeText);
        //}

        public static System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums(System.Windows.Forms.DataGridView dataGridView)
        {
            System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums = new List<System.Windows.Forms.DataGridViewColumn>();
            int CurrentDisplayIndex = dataGridView.Columns[dataGridView.SelectedCells[0].ColumnIndex].DisplayIndex;
            if (ClipBoardValues.Count > 0)
            {
                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    foreach (System.Windows.Forms.DataGridViewColumn C in dataGridView.Columns)
                    {
                        if (C.Visible && C.DisplayIndex == CurrentDisplayIndex + i)
                        {
                            GridColums.Add(C);
                            break;
                        }
                    }
                    if (GridColums.Count >= ClipBoardValues[0].Count) break;
                }
            }
            return GridColums;
        }

        public static bool ValueIsValid(
            System.Windows.Forms.DataGridView dataGridView,
            int ColumnIndex,
            string Value)
        {
            System.Globalization.CultureInfo InvC = new System.Globalization.CultureInfo("");

            if (Value.Length == 0) return true;
            System.DateTime Date;
            System.Byte Byte;
            System.Int16 Int16;
            string TypeOfColumn = dataGridView.Columns[ColumnIndex].ValueType.ToString();
            string ColumnName = dataGridView.Columns[dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
            //string ColumnName = this.dataSetCollectionSpecimenGridMode.FirstLinesCollectionSpecimen.Columns[ColumnIndex].ColumnName;
            int Int;
            bool ValidValue = true;
            try
            {
                // check if the values are valible
                switch (TypeOfColumn)
                {
                    case "System.DateTime":
                        if (!System.DateTime.TryParse(Value, out Date))
                        {
                            if (!System.DateTime.TryParse(Value, InvC, System.Globalization.DateTimeStyles.None, out Date))
                                ValidValue = false;
                        }
                        break;
                    case "int":
                        if (!int.TryParse(Value, out Int))
                            ValidValue = false;

                        break;
                    case "System.Byte":
                        if (!System.Byte.TryParse(Value, out Byte))
                            ValidValue = false;
                        break;
                    case "System.Int16":
                        if (!System.Int16.TryParse(Value, out Int16))
                            ValidValue = false;
                        break;
                }

                // check if the values fit the column definition
                if (Value.Length > 0 && ValidValue)
                {
                    switch (ColumnName)
                    {
                        case "Collection_day":
                        case "Accession_day":
                        case "Identification_day":
                            int Day = int.Parse(Value);
                            if (Day < 1 || Day > 31)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Collection_month":
                        case "Accession_month":
                        case "Identification_month":
                            int Month = int.Parse(Value);
                            if (Month < 1 || Month > 12)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Collection_year":
                        case "Accession_year":
                        case "Identification_year":
                            int Year = int.Parse(Value);
                            if (Year < 1000 || Year > System.DateTime.Now.Year + 1)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Preparation_date":
                            break;
                        case "Longitude":
                            bool IsValid = true;
                            float Lon;
                            if (!float.TryParse(Value, System.Globalization.NumberStyles.Float, InvC, out Lon)) IsValid = false;
                            if (!IsValid || Lon > 180 || Lon < -180)
                            {
                                int x = (int)Lon;
                                float f = Lon * 2;
                                ValidValue = false;
                            }
                            break;
                        case "Latitude":
                            IsValid = true;
                            float Lat;
                            if (!float.TryParse(Value, System.Globalization.NumberStyles.Float, InvC, out Lat)) IsValid = false;
                            if (!IsValid || Lat > 90 || Lat < -90)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Altitude_from":
                        case "Altitude_to":
                            IsValid = true;
                            float Alt;
                            if (!float.TryParse(Value, System.Globalization.NumberStyles.Float, InvC, out Alt)) IsValid = false;
                            if (!IsValid || Alt > 9000 || Alt < -11000)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "MTB":
                            bool OK = true;
                            if (Value.Length != 4) OK = false;
                            if (!int.TryParse(Value, out Int)) OK = false;
                            if (!OK)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Quadrant":
                            OK = true;
                            if (!int.TryParse(Value, out Int)) OK = false;
                            else
                            {
                                for (int i = 0; i < Value.Length; i++)
                                {
                                    if (Value.Substring(i, 1) != "1" &&
                                        Value.Substring(i, 1) != "2" &&
                                        Value.Substring(i, 1) != "3" &&
                                        Value.Substring(i, 1) != "4")
                                        OK = false;
                                }
                            }
                            if (!OK)
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Number_of_units":
                            if (!int.TryParse(Value, out Int))
                            {
                                ValidValue = false;
                            }
                            break;
                        case "Stock":
                            IsValid = true;
                            float Stock;
                            if (!float.TryParse(Value, System.Globalization.NumberStyles.Float, InvC, out Stock)) IsValid = false;
                            if (!IsValid || Stock < 0)
                            {
                                ValidValue = false;
                            }
                            break;
                    }
                }
            }
            catch { }
            return ValidValue;
        }

        #endregion

        #region TableEditor

        public static void TableEditing(string Table, System.Drawing.Image Icon, System.Collections.Generic.List<string> BlockedColumns = null)
        {

        }

        private static void TableEditingNotOptimized(string Table, System.Drawing.Image Icon, System.Collections.Generic.List<string> BlockedColumns = null)
        {
            //try
            //{
            //    if (this.userControlQueryList.OptimizingIsUsed())
            //    {
            //        this.TableEditingForOptimizing(Table, Icon, BlockedColumns);
            //    }
            //    else
            //    {
            //        this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            //        if (this.userControlQueryList.ListOfIDs.Count == 0)
            //        {
            //            System.Windows.Forms.MessageBox.Show("Nothing selected");
            //            this.Cursor = System.Windows.Forms.Cursors.Default;
            //            return;
            //        }
            //        System.Collections.Generic.List<string> ReadOnlyColumns = new List<string>();
            //        DiversityWorkbench.Data.Table T = new DiversityWorkbench.Data.Table(Table);
            //        foreach (string PK in T.PrimaryKeyColumnList)
            //            ReadOnlyColumns.Add(PK);
            //        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Data.Column> DC in T.Columns)
            //        {
            //            if (DC.Key.StartsWith("Log") && !ReadOnlyColumns.Contains(DC.Key))
            //                ReadOnlyColumns.Add(DC.Key);
            //            else if (DC.Value.IsIdentity && !ReadOnlyColumns.Contains(DC.Key))
            //                ReadOnlyColumns.Add(DC.Key);
            //            else if (DC.Key.EndsWith("ID") && !ReadOnlyColumns.Contains(DC.Key))
            //                ReadOnlyColumns.Add(DC.Key);
            //        }
            //        if (BlockedColumns != null)
            //        {
            //            foreach (string C in BlockedColumns)
            //            {
            //                if (!ReadOnlyColumns.Contains(C))
            //                    ReadOnlyColumns.Add(C);
            //            }
            //        }

            //        string IDs = "";
            //        foreach (int i in this.userControlQueryList.ListOfIDs)
            //        {
            //            if (IDs.Length > 0) IDs += ",";
            //            IDs += i.ToString();
            //        }
            //        string SQL = "SELECT * FROM " + Table + " T WHERE T.CollectionSpecimenID IN (" + IDs + ")";
            //        if (Table.StartsWith("CollectionEventSeries"))
            //        {
            //            string SqlIDs = "SELECT DISTINCT E.SeriesID FROM CollectionEvent E, CollectionSpecimen T WHERE E.CollectionEventID = T.CollectionEventID AND T.CollectionSpecimenID IN (" + IDs + ") ";
            //            System.Data.DataTable _DtIDs = new DataTable();
            //            Microsoft.Data.SqlClient.SqlDataAdapter adIDs = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlIDs, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout * 1000));
            //            adIDs.SelectCommand.CommandTimeout = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout * 1000;
            //            adIDs.Fill(_DtIDs);
            //            IDs = "";
            //            foreach (System.Data.DataRow R in _DtIDs.Rows)
            //            {
            //                if (IDs.Length > 0) IDs += ",";
            //                IDs += R[0].ToString();
            //            }
            //            if (IDs.Length == 0)
            //            {
            //                System.Windows.Forms.MessageBox.Show("Nothing selected");
            //                this.Cursor = System.Windows.Forms.Cursors.Default;
            //                return;
            //            }
            //            SQL = "SELECT * FROM " + Table + " T WHERE T.SeriesID IN (" + IDs + ")";
            //        }
            //        else if (Table.StartsWith("CollectionEvent"))
            //        {
            //            string SqlIDs = "SELECT DISTINCT CollectionEventID FROM CollectionSpecimen T WHERE T.CollectionSpecimenID IN (" + IDs + ") AND NOT CollectionEventID IS NULL ";
            //            System.Data.DataTable _DtIDs = new DataTable();
            //            //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout * 1000));
            //            Microsoft.Data.SqlClient.SqlDataAdapter adIDs = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlIDs, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout * 1000));
            //            adIDs.SelectCommand.CommandTimeout = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout * 1000;
            //            //con.Open();
            //            adIDs.Fill(_DtIDs);
            //            //con.Close();
            //            IDs = "";
            //            foreach (System.Data.DataRow R in _DtIDs.Rows)
            //            {
            //                if (IDs.Length > 0) IDs += ",";
            //                IDs += R[0].ToString();
            //            }
            //            if (IDs.Length == 0)
            //            {
            //                System.Windows.Forms.MessageBox.Show("Nothing selected");
            //                this.Cursor = System.Windows.Forms.Cursors.Default;
            //                return;
            //            }
            //            SQL = "SELECT * FROM " + Table + " T WHERE T.CollectionEventID IN (" + IDs + ")";
            //        }
            //        else if (Table == "Transaction")
            //        {
            //            string SqlIDs = "SELECT DISTINCT TransactionID FROM CollectionSpecimenTransaction T WHERE T.CollectionSpecimenID IN (" + IDs + ")";
            //            System.Data.DataTable _DtIDs = new DataTable();
            //            Microsoft.Data.SqlClient.SqlDataAdapter adIDs = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlIDs, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout * 1000));
            //            adIDs.SelectCommand.CommandTimeout = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout * 1000;
            //            adIDs.Fill(_DtIDs);
            //            IDs = "";
            //            foreach (System.Data.DataRow R in _DtIDs.Rows)
            //            {
            //                if (IDs.Length > 0) IDs += ",";
            //                IDs += R[0].ToString();
            //            }
            //            if (IDs.Length == 0)
            //            {
            //                System.Windows.Forms.MessageBox.Show("Nothing selected");
            //                this.Cursor = System.Windows.Forms.Cursors.Default;
            //                return;
            //            }
            //            SQL = "SELECT * FROM [" + Table + "] T WHERE T.TransactionID IN (" + IDs + ")";
            //        }
            //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout * 1000));
            //        ad.SelectCommand.CommandTimeout = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout * 1000;
            //        DiversityWorkbench.Forms.FormTableEditor f = new DiversityWorkbench.Forms.FormTableEditor(Icon, ad, ReadOnlyColumns, DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout, null, Table);
            //        f.StartPosition = FormStartPosition.CenterParent;
            //        f.Width = this.Width - 10;
            //        f.Height = this.Height - 10;
            //        f.setHelpProvider(this.HelpProvider.HelpNamespace, "Table editor");
            //        bool SetTimeout = false;
            //        try
            //        {
            //            f.ShowDialog();
            //            this.setSpecimen(this.ID);
            //        }
            //        catch (System.Exception ex)
            //        {
            //            SetTimeout = true;
            //        }
            //        if (SetTimeout)
            //        {
            //            int? Timeout = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout;
            //            DiversityWorkbench.FormGetInteger ftimeout = new DiversityWorkbench.Forms.FormGetIntegerTimeout, "Set timeout", "A timeout occured. Please set the seconds you are prepared to wait");
            //            ftimeout.ShowDialog();
            //            if (ftimeout.DialogResult == System.Windows.Forms.DialogResult.OK && ftimeout.Integer != null)
            //            {
            //                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout = (int)ftimeout.Integer;
            //                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
            //            }
            //        }
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "TableEditing(string Table, System.Drawing.Image Icon)");
            //}
            //this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private static void TableEditingForOptimizing(string Table, System.Drawing.Image Icon, System.Collections.Generic.List<string> BlockedColumns = null)
        {
            //if (this.userControlQueryList.OptimizingIsUsed())
            //{
            //    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            //    if (this.userControlQueryList.ListOfIDs.Count == 0)
            //    {
            //        System.Windows.Forms.MessageBox.Show("Nothing selected");
            //        this.Cursor = System.Windows.Forms.Cursors.Default;
            //        return;
            //    }
            //    System.Collections.Generic.List<string> ReadOnlyColumns = this.TableEditingReadOnlyColumns(Table);
            //    if (BlockedColumns != null)
            //    {
            //        foreach (string C in BlockedColumns)
            //            ReadOnlyColumns.Add(C);
            //    }

            //    string SQL = "SELECT T.* FROM [" + Table + "] T WHERE T.CollectionSpecimenID IN (SELECT ID FROM #ID)";
            //    if (this.userControlQueryList.ListOfBlockedIDs.Count > 0)
            //    {
            //        string IDs = "";
            //        foreach (int i in this.userControlQueryList.ListOfBlockedIDs)
            //        {
            //            if (IDs.Length > 0)
            //                IDs += ", ";
            //            IDs += i.ToString();
            //        }
            //        SQL += " AND T.CollectionSpecimenID NOT IN (" + IDs + ")";
            //    }
            //    if (Table.StartsWith("CollectionEventSeries"))
            //    {
            //        SQL = "SELECT T.* FROM [" + Table + "] T WHERE T.SeriesID IN (SELECT E.SeriesID FROM CollectionEvent E, CollectionSpecimen S, #ID I " +
            //            "WHERE E.CollectionEventID = S.CollectionEventID AND S.CollectionSpecimenID = I.ID )";
            //    }
            //    else if (Table.StartsWith("CollectionEvent"))
            //    {
            //        SQL = "SELECT T.* FROM [" + Table + "] T WHERE T.CollectionEventID IN (SELECT S.CollectionEventID FROM CollectionSpecimen S, #ID I " +
            //            "WHERE S.CollectionSpecimenID = I.ID) ";
            //    }
            //    else if (Table.StartsWith("Transaction"))
            //    {
            //        SQL = "SELECT T.* FROM [" + Table + "] T WHERE T.TransactionID IN (SELECT S.TransactionID FROM CollectionSpecimenTransaction S, #ID I " +
            //            "WHERE S.CollectionSpecimenID = I.ID )";
            //    }
            //    // Check if Table #ID is filled
            //    bool TempTableIDfilled = true;
            //    string SqlCheck = "SELECT COUNT(*) FROM #ID ";
            //    Microsoft.Data.SqlClient.SqlDataAdapter adCheck = this.userControlQueryList.DataAdapterForTempIDs(SqlCheck);
            //    System.Data.DataTable dtCheck = new DataTable();
            //    adCheck.Fill(dtCheck);
            //    if (dtCheck.Rows.Count == 0)
            //    {

            //    }
            //    else if (dtCheck.Rows.Count == 1)
            //    {
            //        string Result = dtCheck.Rows[0][0].ToString();
            //        if (Result == "0")
            //        {
            //            if (this.userControlQueryList.ListOfIDs.Count > 0)
            //            {

            //            }
            //        }
            //    }

            //    Microsoft.Data.SqlClient.SqlDataAdapter ad = this.userControlQueryList.DataAdapterForTempIDs(SQL);
            //    DiversityWorkbench.Forms.FormTableEditor f = new DiversityWorkbench.Forms.FormTableEditor(Icon, ad, Table, "T", ReadOnlyColumns, this.TableEditingLookupValues(Table));
            //    f.StartPosition = FormStartPosition.CenterParent;
            //    f.Width = this.Width - 10;
            //    f.Height = this.Height - 10;
            //    f.setHelpProvider(this.HelpProvider.HelpNamespace, "Table editor");
            //    bool SetTimeout = false;
            //    try
            //    {
            //        f.ShowDialog();
            //        this.setSpecimen(this.ID);
            //    }
            //    catch (System.Exception ex)
            //    {
            //        SetTimeout = true;
            //    }
            //    if (SetTimeout)
            //    {
            //        int? Timeout = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout;
            //        DiversityWorkbench.FormGetInteger ftimeout = new DiversityWorkbench.Forms.FormGetIntegerTimeout, "Set timeout", "A timeout occured. Please set the seconds you are prepared to wait");
            //        ftimeout.ShowDialog();
            //        if (ftimeout.DialogResult == System.Windows.Forms.DialogResult.OK && ftimeout.Integer != null)
            //        {
            //            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Timeout = (int)ftimeout.Integer;
            //            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
            //        }
            //    }
            //    this.Cursor = System.Windows.Forms.Cursors.Default;
            //}
        }


        #endregion

        #region Color

        /// <summary>
        /// Returning a lighter hue of the original color
        /// </summary>
        /// <param name="color">The original color</param>
        /// <param name="correctionFactor">The transparency factor: 0 = transparent - 1 = no transparency</param>
        /// <returns></returns>
        public static System.Drawing.Color paleColor(System.Drawing.Color color, float correctionFactor)
        {
            if (correctionFactor > 1) correctionFactor = 1;
            if (correctionFactor < 0) correctionFactor = 0;
            float red = (255 - color.R) * correctionFactor + color.R;
            float green = (255 - color.G) * correctionFactor + color.G;
            float blue = (255 - color.B) * correctionFactor + color.B;
            if (color.R == 255 || color.G == 255 || color.B == 255)
            {
                float Min = red;
                if (green < red)
                    Min = green;
                if (blue < Min)
                    Min = blue;
                red = (Min - color.R) * correctionFactor + color.R;
                green = (Min - color.G) * correctionFactor + color.G;
                blue = (Min - color.B) * correctionFactor + color.B;
            }
            Color lighterColor = Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
            return lighterColor;
        }

        #endregion

        #region Performance

        private static System.Collections.Generic.Dictionary<string, DateTime> _TimeSteps;

        public static void AddStep(string Step, DateTime Time)
        {
            if (_TimeSteps == null) _TimeSteps = new Dictionary<string, DateTime>();
            _TimeSteps.Add((_TimeSteps.Count + 1).ToString() + ": " + Step, System.DateTime.Now);
        }

        public static void ShowSteps()
        {
            string Message = "";
            DateTime? previousDate = null;
            string PreviousStep = null;
            foreach (System.Collections.Generic.KeyValuePair<string, DateTime> KV in _TimeSteps)
            {
                if (PreviousStep != null && previousDate != null)
                {

                }
                else
                {

                }
                PreviousStep = KV.Key;
                previousDate = KV.Value;
            }
        }

        #endregion

        #region Availability

        public enum AvailabilityState { Unknown, NotAvailable, ReadOnly, FullAccess, Locked };

        /// <summary>
        /// Returns the availability of a dataset. 
        /// Top priority are locked data. 
        /// Next are available data that are not contained in a read only list
        /// Next are read only data
        /// Final rest is not available
        /// </summary>
        /// <param name="ID">PK of the dataset</param>
        /// <param name="IdentityColumn">ColumnName of the PK</param>
        /// <param name="FullAccessView">View containing data where the user has full access</param>
        /// <param name="ReadOnlyView">View containing read only data</param>
        /// <param name="LockedView">View containing locked data</param>
        /// <returns></returns>
        public static AvailabilityState Availability(int ID, string IdentityColumn, string FullAccessView, string ReadOnlyView, string LockedView)
        {
            AvailabilityState A = AvailabilityState.Unknown;
            try
            {
                string SQL = "";
                string Test = "";
                // Testing locked if provided
                if (LockedView.Length > 0)
                {
                    SQL = "SELECT COUNT(*) " +
                        "FROM " + LockedView + " AS A " +
                        "WHERE " + IdentityColumn + " = " + ID.ToString();
                    Test = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Test == "1")
                        A = AvailabilityState.Locked;
                }

                // Dataset is not locked
                if (A != AvailabilityState.Locked)
                {
                    // Check for full access
                    if (FullAccessView.Length > 0)
                    {
                        SQL = "SELECT COUNT(*) " +
                            "FROM " + FullAccessView + " AS A " +
                            "WHERE " + IdentityColumn + " = " + ID.ToString();
                        Test = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        if (Test != "0")
                            A = AvailabilityState.FullAccess;
                    }
                    // View for Read only
                    if (A != AvailabilityState.FullAccess)
                    {
                        SQL = "SELECT COUNT(*) " +
                        "FROM " + ReadOnlyView + " AS A " +
                        "WHERE " + IdentityColumn + " = " + ID.ToString();
                        Test = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        if (Test == "1")
                            A = AvailabilityState.ReadOnly;
                        else
                            A = AvailabilityState.NotAvailable;
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return A;
        }


        #endregion

        #region Date

        public static string IsoFormatPeriodDisplayText(string Text, bool Short = true)
        {
            string Display = "";
            int Years = 0;
            int Months = 0;
            int Days = 0;
            int hours = 0;
            int minutes = 0;
            int seconds = 0;
            if (IsIsoFormatPeriod(Text, ref Years, ref Months, ref Days, ref hours, ref minutes, ref seconds))
            {
                System.Collections.Generic.Dictionary<DurationPart, int> Parts = new Dictionary<DurationPart, int>();
                Parts.Add(DurationPart.Y, Years);
                Parts.Add(DurationPart.M, Months);
                Parts.Add(DurationPart.D, Days);
                Parts.Add(DurationPart.h, hours);
                Parts.Add(DurationPart.m, minutes);
                Parts.Add(DurationPart.s, seconds);
                System.Collections.Generic.Dictionary<DurationPart, string> PartTexts = new Dictionary<DurationPart, string>();
                PartTexts.Add(DurationPart.Y, "year");
                PartTexts.Add(DurationPart.M, "month");
                PartTexts.Add(DurationPart.D, "day");
                PartTexts.Add(DurationPart.h, "hour");
                if (Short)
                {
                    PartTexts.Add(DurationPart.m, "min.");
                    PartTexts.Add(DurationPart.s, "sec.");
                }
                else
                {
                    PartTexts.Add(DurationPart.m, "minute");
                    PartTexts.Add(DurationPart.s, "second");
                }
                int s = 0;
                foreach (System.Collections.Generic.KeyValuePair<DurationPart, int> pair in Parts)
                {
                    if (pair.Value > 0)
                    {
                        if (Display.Length > 0) Display += " ";
                        Display += pair.Value.ToString() + " " + PartTexts[pair.Key];
                        if (pair.Value > 1 && !Display.EndsWith(".")) Display += "s";
                        s++;
                    }
                    if (Short && s > 2) break;
                }
            }
            else
                Display = Text;
            return Display;
        }

        /// <summary>
        /// Returns a Iso date if valid date is provided
        /// Markus 5.7.23
        /// </summary>
        /// <param name="Text">The date and time as a string</param>
        /// <param name="SpaceAsSeparator">If a space instead of a T as separator between Date and Time should be used</param>
        /// <param name="RestrictToDate">If a space instead should be restricted to the date</param>
        /// <returns>The isodate in case of a valid date, otherwise the original string</returns>
        public static string IsoDate(string Text, bool SpaceAsSeparator = true, bool RestrictToDate = false)
        {
            System.DateTime DT;
            if (System.DateTime.TryParse(Text, out DT))
            {
                if ((DT.Hour == 0 && DT.Minute == 0 && DT.Second == 0 && DT.Millisecond == 0) || RestrictToDate)
                    Text = DT.ToString("yyyy-MM-dd");
                else
                {
                    if (!SpaceAsSeparator)
                        Text = DT.ToString("yyyy-MM-ddTHH:mm:ss");
                    else
                        Text = DT.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            return Text;
        }

        public static string IsoFormatPeriod(int Year, int Month, int Day, int hour, int minute, int second)
        {
            string IsoFormat = "P";
            if (Year > 0) IsoFormat += Year.ToString() + "Y";
            if (Month > 0) IsoFormat += Month.ToString() + "M";
            if (Day > 0) IsoFormat += Day.ToString() + "D";
            if (hour > 0 || minute > 0 || second > 0) IsoFormat += "T";
            if (hour > 0) IsoFormat += hour.ToString() + "H";
            if (minute > 0) IsoFormat += minute.ToString() + "M";
            if (second > 0) IsoFormat += second.ToString() + "S";
            return IsoFormat;
        }

        public static bool IsIsoFormatPeriod(string Text)
        {
            int Y = 0;
            int M = 0;
            int D = 0;
            int h = 0;
            int m = 0;
            int s = 0;
            return IsIsoFormatPeriod(Text, ref Y, ref M, ref D, ref h, ref m, ref s);
        }


        public static bool IsIsoFormatPeriod(string Text, ref int Year, ref int Month, ref int Day, ref int hour, ref int minute, ref int second)
        {
            for (int i = 0; i < Text.Length; i++)
            {
                char Check = Text.ToUpper()[i];
                int iCheck;
                if (!int.TryParse(Check.ToString(), out iCheck))
                {
                    if (Check != 'P' && Check != 'Y' && Check != 'M' && Check != 'D' && Check != 'T' && Check != 'H' && Check != 'S')
                        return false;
                }
            }
            bool IsIso = true;
            IsIso = Text.ToUpper().StartsWith("P") || Text.Length == 0;
            if (Text.ToUpper().StartsWith("P")) Text = Text.Substring(1);
            string Time = "";
            string Date = "";
            if (IsIso)
            {
                // Splitting Date and Time
                if (Text.IndexOf("T") > -1) // Time is given
                {
                    string[] tt = Text.Split(new char[] { 'T' });
                    if (tt.Length == 0)
                        return false;
                    else if (tt.Length == 1)
                    {
                        Time = tt[0];
                    }
                    else if (tt.Length == 2)
                    {
                        Date = tt[0];
                        Time = tt[1];
                    }
                    else return false;
                }
                else
                {
                    if (Text.ToUpper().StartsWith("P"))
                        Date = Text.Substring(1);
                    else Date = Text;
                }
                // Parsing Date
                if (Date.Length > 0)
                {
                    IsIso = GetIsoDatePart(DurationPart.Y, ref Year, ref Date);
                    if (!IsIso) return false;
                    IsIso = GetIsoDatePart(DurationPart.M, ref Month, ref Date);
                    if (!IsIso) return false;
                    IsIso = GetIsoDatePart(DurationPart.D, ref Day, ref Date);
                    //if (Date.Length > 0) IsIso = false;
                }
                // Parsing Time
                if (Time.Length > 0)
                {
                    IsIso = GetIsoDatePart(DurationPart.h, ref hour, ref Time);
                    if (!IsIso) return false;
                    IsIso = GetIsoDatePart(DurationPart.m, ref minute, ref Time);
                    if (!IsIso) return false;
                    IsIso = GetIsoDatePart(DurationPart.s, ref second, ref Time);
                    //if (Time.Length > 0) IsIso = false;
                }
            }
#if !DEBUG
            return false;
#endif
            return IsIso;
        }

        private enum DurationPart { Y, M, D, h, m, s }

        private static bool GetIsoDatePart(DurationPart Part, ref int Value, ref string Text)
        {
            bool OK = true;
            try
            {
                if (Text.IndexOf(Part.ToString().ToUpper()) > -1)
                {
                    string[] tt = Text.Split(new char[] { Part.ToString().ToUpper()[0] });
                    OK = int.TryParse(tt[0], out Value);
                    if (OK && tt.Length > 1)
                        Text = tt[1];
                    else Text = "";
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); OK = false; }
            return OK;
        }


        #endregion

    }
}
