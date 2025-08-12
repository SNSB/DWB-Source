using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;


namespace DiversityWorkbench.Forms
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
            this.labelProduct.Text = Application.ProductName.ToString();
            this.labelVersionText.Text = Application.ProductVersion.ToString();
            try
            {
                string SQL = "SELECT dbo.Version()";
                string VersionDB = "";
                VersionDB = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (VersionDB.Length > 0)
                    this.labelVersionText.Text += "      Database:   " + VersionDB;
            }
            catch (System.Exception ex)
            {
            }
            this.labelCopyrightText.Text = "© 1999 - " + System.DateTime.Now.Year.ToString() + ", Diversity Workbench";

            this.labelDotNetOnMachine.Text = DiversityWorkbench.Forms.FormFunctions.Get45or451FromRegistry();
            //this.labelCompanyText.Text = "Staatliche Naturwissenschaftliche Sammlungen Bayerns,\r\nIT Centre";
            //this.linkLabelLicence.Text = "http://www.gnu.org/licenses/gpl.html";


            //this.labelDescription.Text = Assembly.GetExecutingAssembly().GetCustomAttributes(at
            //this.labelCompanyText.Text = Application.CompanyName.ToString();
            //this.labelCopyrightText.Text = "© Diversity Workbench 2006 - 2008";
            //this.labelAuthorsText.Text = "Markus Weiss";
            //this.labelLicenseText.Text = "GNU General Public License (GPL)";
            //this.labelLicenseText.Text = "This program is free software: you can redistribute it and/or modify it " +
            //    "under the terms of the GNU General Public License as published by the Free Software Foundation. " +
            //    "\r\nThis program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; " +
            //    "without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. " +
            //    "\r\nSee the GNU General Public License for more details.";

            //Type clsType = typeof(FormAbout);
            //// Get the assembly object.
            //Assembly assy = clsType.Assembly;
            //// Store the assembly's name.
            //String assyName = assy.GetName().Name;
            //// See if the Assembly Description is defined.
            //bool isdef = Attribute.IsDefined(assy, typeof(AssemblyDescriptionAttribute));
            //if (isdef)
            //{
            //    // Affirm that the attribute is defined.
            //    Console.WriteLine("The AssemblyDescription attribute " +
            //        "is defined for assembly {0}.", assyName);
            //    // Get the description attribute itself.
            //    AssemblyDescriptionAttribute adAttr =
            //        (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(
            //        assy, typeof(AssemblyDescriptionAttribute));
            //    // Display the description.
            //    if (adAttr != null)
            //        this.labelDescription.Text = adAttr.Description;
            //}
        }

        public FormAbout(string Product, string Version, string Company)
        {
            InitializeComponent();
            this.labelProduct.Text = Product;
            this.labelVersionText.Text = Version;
            //if (Company.Length > 0)
            //    this.labelCompanyText.Text = Company;
            //this.labelCopyrightText.Text = "© Diversity Workbench 2006 - 2008";
            //this.labelAuthorsText.Text = "Markus Weiss";
            //this.labelLicenseText.Text = "This program is free software: you can redistribute it and/or modify it" +
            //    "under the terms of the GNU General Public License as published by the Free Software Foundation. " +
            //    "\r\nThis program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; " +
            //    "without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. " +
            //    "\r\nSee the GNU General Public License for more details.";
            ////this.linkLabelLicence.Text = "http://www.gnu.org/";
            //this.labelCompanyText.Text = "The IT Centre of the\r\nStaatliche Naturwissenschaftliche Sammlungen Bayerns";
        }

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //public void setModule(string Module)
        //{
        //    switch (Module)
        //    {
        //        case "DiversityTaxonNames":
        //            break;
        //        default:
        //            break;
        //    }
        //}

        private void linkLabelCompany_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string target = this.linkLabelCompany.Text;
            try
            {
                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(target);
                info.UseShellExecute = true;
                System.Diagnostics.Process.Start(info);
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void linkLabelLicence_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string target = this.linkLabelLicence.Text;
            try
            {
                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(target);
                info.UseShellExecute = true;
                System.Diagnostics.Process.Start(info);
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        protected void setSeize()
        {
            this.Width = (int)(355.0 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
            int Height = (int)(110.0 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
            Height += this.labelDescription.Height;
            Height += this.labelAuthorsText.Height;
            Height += this.labelCompanyText.Height;
            Height += this.labelCopyrightText.Height;
            Height += this.labelLicenseText.Height;
            Height += this.labelVersionText.Height;
            Height += this.linkLabelCompany.Height;
            Height += this.linkLabelLicence.Height;
            Height += this.labelProduct.Height;
            Height += this.labelDisclaimerText.Height;
            Height += this.buttonOK.Height;
            this.Height = Height;
        }

        private void linkLabelDotNet_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string URL = "https://dotnet.microsoft.com/download/dotnet-framework/net48";
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(URL);
            info.UseShellExecute = true;
            System.Diagnostics.Process.Start(info);
        }

        //protected virtual void setDescription() 
        //{
        //    Type clsType = typeof(FormAbout);
        //    // Get the assembly object.
        //    Assembly assy = clsType.Assembly;
        //    // Store the assembly's name.
        //    String assyName = assy.GetName().Name;
        //    // See if the Assembly Description is defined.
        //    bool isdef = Attribute.IsDefined(assy, typeof(AssemblyDescriptionAttribute));
        //    if (isdef)
        //    {
        //        // Get the description attribute itself.
        //        AssemblyDescriptionAttribute adAttr =
        //            (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(
        //            assy, typeof(AssemblyDescriptionAttribute));
        //        // Display the description.
        //        if (adAttr != null)
        //            this.labelDescription.Text = adAttr.Description;
        //    }
        //}

        //protected void setDescription(System.Windows.Forms.Form Form)
        //{
        //    Type clsType = typeof(Form);
        //    // Get the assembly object.
        //    Assembly assy = clsType.Assembly;
        //    // Store the assembly's name.
        //    String assyName = assy.GetName().Name;
        //    // See if the Assembly Description is defined.
        //    bool isdef = Attribute.IsDefined(assy, typeof(AssemblyDescriptionAttribute));
        //    if (isdef)
        //    {
        //        // Get the description attribute itself.
        //        AssemblyDescriptionAttribute adAttr =
        //            (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(
        //            assy, typeof(AssemblyDescriptionAttribute));
        //        // Display the description.
        //        if (adAttr != null)
        //            this.labelDescription.Text = adAttr.Description;
        //    }
        //}
    }
}