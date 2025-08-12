using DWBServices.WebServices;
using DWBServices.WebServices.GeoServices.Geonames;
using DWBServices.WebServices.GeoServices.GFBioTermGeonames;
using DWBServices.WebServices.GeoServices.IHOWorldSeas;
using DWBServices.WebServices.GeoServices.ISOCountries;
using DWBServices.WebServices.TaxonomicServices.CatalogueOfLife;
using DWBServices.WebServices.TaxonomicServices.GbifSpecies;
using DWBServices.WebServices.TaxonomicServices.GfbioTerminology;
using DWBServices.WebServices.TaxonomicServices.IndexFungorum;
using DWBServices.WebServices.TaxonomicServices.Mycobank;
using DWBServices.WebServices.TaxonomicServices.PESI;
using DWBServices.WebServices.TaxonomicServices.WoRMS;
using Microsoft.Extensions.DependencyInjection;

namespace DWBServices
{
    /// <summary>
    /// Provides access to the service provider and facilitates the retrieval of specific web services
    /// for various DWB services.
    /// </summary>
    /// <remarks>
    /// This static class acts as a central point for accessing service instances and retrieving
    /// implementations of <see cref="IDwbWebservice{TSearchResult, TSearchResultItem, TEntity}"/> 
    /// based on the specified <see cref="DwbServiceEnums.DwbService"/> enumeration.
    /// </remarks>
    public static class DwbServiceProviderAccessor
    {
        public static IServiceProvider? Instance { get; set; }

        /// <summary>
        /// Retrieves an instance of a specific DWB web service based on the provided service type.
        /// </summary>
        /// <param name="service">
        /// The type of the DWB service to retrieve. This must be one of the values defined in 
        /// <see cref="DWBServices.WebServices.DwbServiceEnums.DwbService"/>.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IDwbWebservice{TDwbSearchResult, TDwbSearchResultItem, TDwbEntity}"/> 
        /// corresponding to the specified service type, or <c>null</c> if the service provider instance is not initialized.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Thrown if the specified service type is not supported.
        /// </exception>
        public static IDwbWebservice<DwbSearchResult, DwbSearchResultItem, DwbEntity>? GetDwbWebservice(DwbServiceEnums.DwbService service)
        {
            if (Instance == null)
                return null;
            return service switch
            {
                DwbServiceEnums.DwbService.CatalogueOfLife =>
                    Instance.GetRequiredService<CoLWebservice>(),
                DwbServiceEnums.DwbService.PoWo_WCVP =>
                    Instance.GetRequiredService<CoLWebservice>(),
                DwbServiceEnums.DwbService.IndexFungorum =>
                Instance.GetRequiredService<IndexFungorumWebservice>(),
                DwbServiceEnums.DwbService.PESI =>
                    Instance.GetRequiredService<PESIWebservice>(),
                DwbServiceEnums.DwbService.Mycobank =>
                    Instance.GetRequiredService<MycobankWebservice>(),
                DwbServiceEnums.DwbService.GBIFSpecies =>
                    Instance.GetRequiredService<GbifSpeciesWebservice>(),
                DwbServiceEnums.DwbService.WoRMS =>
                    Instance.GetRequiredService<WoRMSWebservice>(),
                DwbServiceEnums.DwbService.GFBioTerminologyService =>
                    Instance.GetRequiredService<GfbioTerminologyWebservice>(),
                DwbServiceEnums.DwbService.Geonames =>
                    Instance.GetRequiredService<GeonamesWebservice>(),
                DwbServiceEnums.DwbService.GFBioTermGeonames =>
                    Instance.GetRequiredService<GFBioTermGeonamesWebservice>(),
                DwbServiceEnums.DwbService.IsoCountries =>
                    Instance.GetRequiredService<IsoCountriesWebservice>(),
                DwbServiceEnums.DwbService.IHOWorldSeas =>
                    Instance.GetRequiredService<IHOWorldSeasWebservice>(),
                _ => throw new NotSupportedException($"Service {service} is not supported.")
            };
        }
    }
}
