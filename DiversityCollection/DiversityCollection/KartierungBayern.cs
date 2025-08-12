using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace DiversityCollection
{
    public partial class KartierungBayern : Component
    {
        public KartierungBayern()
        {
            InitializeComponent();
        }

        public KartierungBayern(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
