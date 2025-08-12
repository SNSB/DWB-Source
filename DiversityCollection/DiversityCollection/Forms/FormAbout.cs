using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace DiversityCollection.Forms
{
    public partial class FormAbout : DiversityWorkbench.Forms.FormAbout
    {
        public FormAbout()
        {
            InitializeComponent();
            this.setSeize();
            this.setDescription();
            this.labelAuthorsText.Text = "Markus Weiss, Ariane Grunz, Wolfgang Reichert (GIS editor)";
        }

        protected void setDescription()
        {
            Type clsType = typeof(FormAbout);
            // Get the assembly object.
            Assembly assy = clsType.Assembly;
            // Store the assembly's name.
            String assyName = assy.GetName().Name;
            // See if the Assembly Description is defined.
            bool isdef = Attribute.IsDefined(assy, typeof(AssemblyDescriptionAttribute));
            if (isdef)
            {
                // Get the description attribute itself.
                AssemblyDescriptionAttribute adAttr =
                    (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(
                    assy, typeof(AssemblyDescriptionAttribute));
                // Display the description.
                if (adAttr != null)
                    this.labelDescription.Text = adAttr.Description;
            }
        }

    }
}

