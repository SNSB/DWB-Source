using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench
{
    public partial class Language : Component
    {
        public Language()
        {
            InitializeComponent();
        }

        public Language(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public static System.Drawing.Image Image(string LanguageOrCountry)
        {
            if (LanguageOrCountry.Length == 2)
            {
                switch (LanguageOrCountry.ToLower())
                {
                    case "de":
                        return ImageList.Images["Deutsch.ico"];
                    case "en":
                        return ImageList.Images["English.ico"];
                    case "fr":
                        return ImageList.Images["Frankreich.ico"];
                    case "es":
                        return ImageList.Images["Spanien.ico"];
                }
            }
            return ImageList.Images[0];
        }

        private static System.Windows.Forms.ImageList _ImageList;
        public static System.Windows.Forms.ImageList ImageList
        {
            get
            {
                if (DiversityWorkbench.Language._ImageList == null)
                {
                    DiversityWorkbench.Language L = new Language();
                    DiversityWorkbench.Language._ImageList = L.imageList;
                }
                return DiversityWorkbench.Language._ImageList;
            }
        }


    }
}
