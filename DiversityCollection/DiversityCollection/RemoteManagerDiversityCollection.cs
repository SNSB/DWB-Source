using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityCollection
{
    class RemoteManagerDiversityCollection
    {
        private string _SqlShortDescription = "SELECT     CollectionSpecimen.CollectionSpecimenID, CollectionSpecimen.AccessionNumber, IdentificationUnit.LastIdentificationCache,  " +
            " IdentificationUnit.TaxonomicGroup, CollectionAgent.CollectorsName, CollectionEvent.CollectionDay, CollectionEvent.CollectionMonth,  " +
            " CollectionEvent.CollectionYear, CollectionEvent.LocalityDescription, CollectionEvent.HabitatDescription " +
            " FROM         CollectionAgent CollectionAgent_1 INNER JOIN " +
            " CollectionAgent ON CollectionAgent_1.CollectionSpecimenID = CollectionAgent.CollectionSpecimenID AND  " +
            " CollectionAgent_1.CollectorsSequence = CollectionAgent.CollectorsSequence RIGHT OUTER JOIN " +
            " CollectionEvent RIGHT OUTER JOIN " +
            " CollectionSpecimen LEFT OUTER JOIN " +
            " IdentificationUnit INNER JOIN " +
            " IdentificationUnit IdentificationUnit_1 ON IdentificationUnit.CollectionSpecimenID = IdentificationUnit_1.CollectionSpecimenID ON  " +
            " CollectionSpecimen.CollectionSpecimenID = IdentificationUnit.CollectionSpecimenID ON  " +
            " CollectionEvent.CollectionEventID = CollectionSpecimen.CollectionEventID ON  " +
            " CollectionAgent.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID " +
            " WHERE     (IdentificationUnit_1.DisplayOrder = 1) " +
            " GROUP BY CollectionSpecimen.CollectionSpecimenID, CollectionSpecimen.AccessionNumber, IdentificationUnit.LastIdentificationCache,  " +
            " IdentificationUnit.TaxonomicGroup, CollectionAgent.CollectorsName, CollectionEvent.CollectionDay, CollectionEvent.CollectionMonth,  " +
            " CollectionEvent.CollectionYear, CollectionEvent.LocalityDescription, CollectionEvent.HabitatDescription,  " +
            " CollectionAgent_1.CollectionSpecimenID " +
            " ORDER BY CollectionSpecimen.AccessionNumber";
            /*
                    SELECT CollectionSpecimen.CollectionSpecimenID, 
            'Acc.Nr.: ' + CASE WHEN CollectionSpecimen.AccessionNumber IS NULL THEN '' ELSE CollectionSpecimen.AccessionNumber END +
            ', Taxon: ' + case when IdentificationUnit.LastIdentificationCache is null then '' else IdentificationUnit.LastIdentificationCache end +
            ', Group: ' + case when IdentificationUnit.TaxonomicGroup is null then '' else  IdentificationUnit.TaxonomicGroup end + 
            ', Collector: ' + case when CollectionAgent.CollectorsName is null then '' else CollectionAgent.CollectorsName end +
            ', Coll.date: ' + case when CollectionEvent.CollectionYear is null then '' else cast(CollectionEvent.CollectionYear as varchar) end + '/' +
            case when CollectionEvent.CollectionMonth is null then '' else cast(CollectionEvent.CollectionMonth as varchar) end + '/' +
            case when CollectionEvent.CollectionDay is null then '' else cast (CollectionEvent.CollectionDay as varchar) end +
            ', Locality: ' + case when CollectionEvent.LocalityDescription is null then '' else CollectionEvent.LocalityDescription end +
            ', Habitat: ' + case when CollectionEvent.HabitatDescription is null then '' else CollectionEvent.HabitatDescription end as Description
            FROM CollectionAgent CollectionAgent_1 INNER JOIN
            CollectionAgent ON CollectionAgent_1.CollectionSpecimenID = CollectionAgent.CollectionSpecimenID AND 
            CollectionAgent_1.CollectorsSequence = CollectionAgent.CollectorsSequence RIGHT OUTER JOIN
            CollectionEvent RIGHT OUTER JOIN
            CollectionSpecimen LEFT OUTER JOIN
            IdentificationUnit INNER JOIN
            IdentificationUnit IdentificationUnit_1 ON IdentificationUnit.CollectionSpecimenID = IdentificationUnit_1.CollectionSpecimenID ON 
            CollectionSpecimen.CollectionSpecimenID = IdentificationUnit.CollectionSpecimenID ON 
            CollectionEvent.CollectionEventID = CollectionSpecimen.CollectionEventID ON 
            CollectionAgent.CollectionSpecimenID = CollectionSpecimen.CollectionSpecimenID
            WHERE     (IdentificationUnit_1.DisplayOrder = 1)
            GROUP BY CollectionSpecimen.CollectionSpecimenID, CollectionSpecimen.AccessionNumber, IdentificationUnit.LastIdentificationCache, 
            IdentificationUnit.TaxonomicGroup, CollectionAgent.CollectorsName, CollectionEvent.CollectionDay, CollectionEvent.CollectionMonth, 
            CollectionEvent.CollectionYear, CollectionEvent.LocalityDescription, CollectionEvent.HabitatDescription, 
            CollectionAgent_1.CollectionSpecimenID
            ORDER BY CollectionSpecimen.AccessionNumber
             * */
    }
}
