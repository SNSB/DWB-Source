using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.UserControls
{
    public interface iIdentification
    {
        void setIdentificationTermControls(bool IsTaxonomyRelatedTaxonomicGroup);
        void setTaxonomicGroup(string TaxonomicGroup);
    }
}
