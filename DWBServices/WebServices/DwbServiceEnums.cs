using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DWBServices.WebServices
{
    /// <summary>
    /// 
    /// </summary>
    public class DwbServiceEnums
    {
        public enum HttpAction
        {
            GET
        }
        
        public enum DwbServiceType
        {
            None,
            TaxonomicService,
            GeoService
        }

        public enum DwbService
        {
            None,
            CatalogueOfLife,
            PoWo_WCVP,
            IndexFungorum,
            PESI,
            Mycobank,
            GBIFSpecies,
            WoRMS,
            GFBioTerminologyService,
            Geonames,
            GFBioTermGeonames,
            IsoCountries,
            IHOWorldSeas
        }

        public class DwbServiceInfo
        {
            public DwbServiceType Type { get; set; }
            public string Name { get; set; }
            public string Url {get; set; }
            public string DatasetKey { get; set; } // if the webservice itself includes different datasets/webservices, see for examle the CoLWebservice
        }
        // Important note:
        // for reasons of compatibility with earlier web service implementations.
        // Used in the diversityWorkbench.WorkbenchUnit class, _ServiceList, for example
        public static Dictionary<DwbService, DwbServiceInfo> TaxonomicServiceInfoDictionary()
        {
            var configuration = DwbServiceProviderAccessor.Instance.GetRequiredService<IConfiguration>();
            return new Dictionary<DwbService, DwbServiceInfo>
            {
                { DwbService.None, new DwbServiceInfo{ Type = DwbServiceType.None, Name = "None", Url = "None", DatasetKey = ""}}, 
                { DwbService.CatalogueOfLife, new DwbServiceInfo{ Type = DwbServiceType.TaxonomicService, Name = "Catalogue of Life", Url = configuration["CoL:CoL_BaseAddress"], DatasetKey = "3LR"} },
                { DwbService.PoWo_WCVP, new DwbServiceInfo{ Type = DwbServiceType.TaxonomicService, Name = "WCVP/PoWo - Catalogue of Life)", Url = configuration["CoL:CoL_BaseAddress"], DatasetKey = "2232"} },
                { DwbService.IndexFungorum, new DwbServiceInfo{ Type = DwbServiceType.TaxonomicService, Name = "Index Fungorum", Url = configuration["IndexFungorum:IndexFungorum_BaseAddress"], DatasetKey = ""}},
                { DwbService.PESI, new DwbServiceInfo{ Type = DwbServiceType.TaxonomicService, Name = "Pan-European Species directories Infrastructure", Url = configuration["PESI:PESI_BaseAddress"], DatasetKey = ""} },
                { DwbService.Mycobank, new DwbServiceInfo{ Type = DwbServiceType.TaxonomicService, Name = "Mycobank", Url = configuration["Mycobank:Mycobank_BaseAddress"], DatasetKey = ""} },
                { DwbService.GBIFSpecies, new DwbServiceInfo{ Type = DwbServiceType.TaxonomicService, Name = "GBIF Species", Url = configuration["GbifSpecies:GbifSpecies_BaseAddress"], DatasetKey = ""} },
                { DwbService.WoRMS, new DwbServiceInfo{ Type = DwbServiceType.TaxonomicService, Name = "WoRMS", Url = configuration["WoRMS:WoRMS_BaseAddress"], DatasetKey = ""} },
                { DwbService.GFBioTerminologyService, new DwbServiceInfo{ Type = DwbServiceType.TaxonomicService, Name = "GFBio TerminologyService", Url = configuration["GfbioTerminology:GfbioTerminology_BaseAddress"], DatasetKey = ""} }
        };
        }

        public static Dictionary<DwbService, DwbServiceInfo> GeoServiceInfoDictionary()
        {
            var configuration = DwbServiceProviderAccessor.Instance.GetRequiredService<IConfiguration>();
            // { DwbService.Geonames, new DwbServiceInfo{ Type = DwbServiceType.GeoService, Name = "GeoNames", Url = configuration["GeoNames:GeoNames_ServiceUri"] } },
            return new Dictionary<DwbService, DwbServiceInfo>
            {
                { DwbService.None, new DwbServiceInfo{ Type = DwbServiceType.None, Name = "None", Url = "None"}},
                { DwbService.GFBioTermGeonames, new DwbServiceInfo{ Type = DwbServiceType.GeoService, Name = "GFBioTerminology GeoNames", Url = configuration["GFBioTermGeonames:GeoNames_ServiceUri"] } },
                { DwbService.IsoCountries, new DwbServiceInfo{ Type = DwbServiceType.GeoService, Name = "ISOCountries", Url = configuration["IsoCountries:IsoCountries_ServiceUri"] } },
                { DwbService.IHOWorldSeas, new DwbServiceInfo{ Type = DwbServiceType.GeoService, Name = "IHO World Seas", Url = configuration["IHOWorldSeas:IHOWorldSeas_ServiceUri"] } }
            };
        }
    }
}
