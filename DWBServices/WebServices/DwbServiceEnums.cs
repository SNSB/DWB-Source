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
            CatalogueOfLifeExtended,
            PoWo_WCVP,
            SpeciesFungorumPlus,
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
        
            public string SearchEndpoint { get; set; } // if the webservice has a specific search endpoint, e.g. for the CoLWebservice
        }
        // Important note:
        // for reasons of compatibility with earlier web service implementations.
        // Used in the diversityWorkbench.WorkbenchUnit class, _ServiceList, for example
        public static Dictionary<DwbService, DwbServiceInfo> TaxonomicServiceInfoDictionary()
        {
            var configuration = DwbServiceProviderAccessor.Instance.GetRequiredService<IConfiguration>();
            return new Dictionary<DwbService, DwbServiceInfo>
            {
                { DwbService.None, new DwbServiceInfo{ Type = DwbServiceType.None, Name = "None", Url = "None", DatasetKey = "", SearchEndpoint = ""}}, 
                { DwbService.CatalogueOfLife, new DwbServiceInfo{ Type = DwbServiceType.TaxonomicService, Name = "Catalogue of Life", Url = configuration["CoL:CoL_BaseAddress"], DatasetKey = configuration["CoL:CoL_DatasetKey"], SearchEndpoint = configuration["CoL:CoL_SearchEndpoint"]} },
                { DwbService.CatalogueOfLifeExtended, new DwbServiceInfo{ Type = DwbServiceType.TaxonomicService, Name = "Catalogue of Life - eXtended Release", Url = configuration["CoL:CoL_BaseAddress"], DatasetKey = configuration["CoL:CoLex_DatasetKey"], SearchEndpoint = configuration["CoL:CoL_SearchEndpoint"]} },
                { DwbService.PoWo_WCVP, new DwbServiceInfo{ Type = DwbServiceType.TaxonomicService, Name = "WCVP/PoWo - Catalogue of Life)", Url = configuration["CoL:CoL_BaseAddress"], DatasetKey = configuration["CoL:PoWo_DatasetKey"], SearchEndpoint = configuration["CoL:CoL_SearchEndpoint"]} },
                //{ DwbService.PaleoBioDB, new DwbServiceInfo{ Type = DwbServiceType.TaxonomicService, Name = "PaleoBioDB - Catalogue of Life)", Url = configuration["CoL:CoL_BaseAddress"], DatasetKey = "268676"} },
                { DwbService.SpeciesFungorumPlus, new DwbServiceInfo{ Type = DwbServiceType.TaxonomicService, Name = "SpeciesFungorumPlus - Catalogue of Life)", Url = configuration["CoL:CoL_BaseAddress"], DatasetKey = configuration["CoL:SpeciesFungorumPlus_DatasetKey"], SearchEndpoint = configuration["CoL:CoL_SearchEndpoint"]} },
                { DwbService.IndexFungorum, new DwbServiceInfo{ Type = DwbServiceType.TaxonomicService, Name = "Index Fungorum", Url = configuration["IndexFungorum:IndexFungorum_BaseAddress"], DatasetKey = "", SearchEndpoint = configuration["IndexFungorum:IndexFungorum_SearchEndpoint"]}},
                { DwbService.PESI, new DwbServiceInfo{ Type = DwbServiceType.TaxonomicService, Name = "Pan-European Species directories Infrastructure", Url = configuration["PESI:PESI_BaseAddress"], DatasetKey = "", SearchEndpoint = configuration["PESI:PESI_SearchEndpoint"]} },
                { DwbService.Mycobank, new DwbServiceInfo{ Type = DwbServiceType.TaxonomicService, Name = "Mycobank", Url = configuration["Mycobank:Mycobank_BaseAddress"], DatasetKey = "", SearchEndpoint = ""} },
                { DwbService.GBIFSpecies, new DwbServiceInfo{ Type = DwbServiceType.TaxonomicService, Name = "GBIF Species", Url = configuration["GbifSpecies:GbifSpecies_BaseAddress"], DatasetKey = configuration["GbifSpecies:GbifSpecies_DatasetKey"], SearchEndpoint = configuration["GbifSpecies:GbifSpecies_SearchEndpoint"]} },
                { DwbService.WoRMS, new DwbServiceInfo{ Type = DwbServiceType.TaxonomicService, Name = "WoRMS", Url = configuration["WoRMS:WoRMS_BaseAddress"], DatasetKey = "", SearchEndpoint = configuration["WoRMS:WoRMS_SearchEndpoint"]} },
                { DwbService.GFBioTerminologyService, new DwbServiceInfo{ Type = DwbServiceType.TaxonomicService, Name = "GFBio TerminologyService", Url = configuration["GfbioTerminology:GfbioTerminology_BaseAddress"], DatasetKey = "", SearchEndpoint = configuration["GfbioTerminology:GfbioTerminology_SearchEndpoint"]} }
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
