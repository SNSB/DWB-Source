using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;
using static DWBServices.WebServices.TaxonomicServices.IndexFungorum.IndexFungorumEntity;

namespace DWBServices.WebServices.TaxonomicServices.IndexFungorum
{
    public class IndexFungorumWebservice : TaxonomicWebservice, IDwbWebservice<TaxonomicSearchResult, TaxonomicSearchResultItem, TaxonomicEntity>
    {
        public IndexFungorumWebservice(HttpClient httpClient) : base(httpClient)
        {
            string baseAddress = this.GetBaseAddress();
            httpClient.BaseAddress = new Uri(baseAddress);
        }

        public override async Task<T> CallWebServiceAsync<T>(
            string url,
            DwbServiceEnums.HttpAction action = DwbServiceEnums.HttpAction.GET,
            HttpContent? content = null)
        {
            HttpResponseMessage? response;

            response = await HttpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // The IndexFungorum Webservice does not return a valid xml file (23.12.2024) so we have to parse it with XDocument,
                // instead of using XMLSerializer to map automtically
                // Parse the XML using XDocument
                // Read the XML content as a string
                string xml = await response.Content.ReadAsStringAsync();
                // If T is string, return the raw XML
                if (typeof(T) == typeof(object))
                {
                    return (T)(object)xml;
                }
                throw new InvalidOperationException($"Unsupported type {typeof(T).Name} for raw data.");
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorContent}");
            }
        }

        public override string DwbApiQueryUrlString(DwbServiceEnums.DwbService currentService, string queryRestrictions, int offset, int maxPerPage)
        {
            IndexFungorumTaxonomicSearchCriterias criteria = new IndexFungorumTaxonomicSearchCriterias();
            queryRestrictions = Uri.EscapeDataString(queryRestrictions);
            if (criteria.ValidateQueryRestrictions(queryRestrictions, offset, maxPerPage))
            {
                criteria.query = queryRestrictions;
                // criteria.offset = offset.ToString();
                criteria.maxPerPage = maxPerPage.ToString();
            }
            else
            {
                throw new ArgumentException($"{queryRestrictions} is not a valid query restriction",
                    nameof(queryRestrictions));
            }

            return criteria.QueryParamString;
        }

        public override string GetBaseAddress()
        {
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("DwbServiceProviderAccessor.Instance is not initialized.");
            string settingValue = configuration["IndexFungorum:IndexFungorum_BaseAddress"];
            return settingValue;
        }

        public override DwbServiceEnums.DwbService GetServiceName()
        {
            return DwbServiceEnums.DwbService.IndexFungorum;
        }

        public override string GetServiceUri(DwbServiceEnums.DwbService currentService)
        {
            var configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("DwbServiceProviderAccessor.Instance is not initialized.");
            string settingValue = configuration["IndexFungorum:IndexFungorum_ServiceUri"];
            return settingValue;
        }

        // not in use so far, needs to be tested
        public override IndexFungorumSearchResultItem GetDwbApiSearchModel<T>(T tt)
        {
            try
            {
                if (tt == null)
                {
                    return null;
                }

                string xml = tt as string;
                if (string.IsNullOrEmpty(xml))
                {
                    return null;
                }

                XDocument xDocument = XDocument.Parse(xml);
                IndexFungorumSearchResultItem result = new IndexFungorumSearchResultItem();
                var rootElement = xDocument.Root;
                if (rootElement == null)
                {
                    throw new InvalidOperationException("The XML document has no root element.");
                }

                foreach (var property in typeof(IndexFungorumSearchResultItem).GetProperties())
                {
                    var element = rootElement.Element(property.Name);
                    if (element != null)
                    {
                        if (property.PropertyType == typeof(int) && int.TryParse(element.Value, out int intValue))
                        {
                            property.SetValue(result, intValue);
                        }
                        else if (property.PropertyType == typeof(string))
                        {
                            property.SetValue(result, element.Value);
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
            }
        }

        public override IndexFungorumSearchResult GetDwbApiSearchResultModel<T>(T tt)
        {
            try
            {
                if (tt == null)
                {
                    return null;
                }

                string xml = tt as string;
                if (string.IsNullOrEmpty(xml))
                {
                    return null;
                }

                XDocument xDocument = XDocument.Parse(xml);
                IndexFungorumSearchResult result = new IndexFungorumSearchResult();
                var rootElement = xDocument.Root;
                if (rootElement == null)
                {
                    throw new InvalidOperationException("The XML document has no root element.");
                }

                foreach (var property in typeof(IndexFungorumSearchResult).GetProperties())
                {
                    var element = rootElement.Element(property.Name);
                    if (element != null)
                    {
                        if (property.PropertyType == typeof(int) && int.TryParse(element.Value, out int intValue))
                        {
                            property.SetValue(result, intValue);
                        }
                        else if (property.PropertyType == typeof(string))
                        {
                            property.SetValue(result, element.Value);
                        }
                        else if (property.PropertyType == typeof(IndexFungorumSearchResultItem[]))
                        {
                            var items = ParseSearchResultItems(rootElement);
                            property.SetValue(result, items);
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
            }
        }
        private IndexFungorumSearchResultItem[] ParseSearchResultItems(XElement parentElement)
        {
            var items = new List<IndexFungorumSearchResultItem>();
            // Iterate through child elements that represent individual items
            foreach (var itemElement in parentElement.Elements("IndexFungorum"))
            {
                IndexFungorumSearchResultItem item = new IndexFungorumSearchResultItem();
                // Map properties of IndexFungorumSearchResultItem
                foreach (var property in typeof(IndexFungorumSearchResultItem).GetProperties())
                {
                    var element = itemElement.Element(property.Name);
                    if (element != null)
                    {
                        if (property.PropertyType == typeof(int) && int.TryParse(element.Value, out int intValue))
                        {
                            property.SetValue(item, intValue);
                        }
                        else if (property.PropertyType == typeof(string))
                        {
                            property.SetValue(item, element.Value);
                        }
                    }
                }
                items.Add(item);
            }
            return items.ToArray();
        }


        public override IndexFungorumEntity GetDwbApiDetailModel<T>(T tt)
        {
            try
            {
                if (tt == null)
                {
                    return null;
                }

                string xml = tt as string;
                if (string.IsNullOrEmpty(xml))
                {
                    return null;
                }

                XDocument xDocument = XDocument.Parse(xml);
                //// Define namespaces
                XNamespace rdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
                XNamespace taxonNameNs = "http://rs.tdwg.org/ontology/voc/TaxonName#";
                XNamespace ns = "http://purl.org/dc/elements/1.1/";
                XNamespace owl = "http://www.w3.org/2002/07/owl#";
                XNamespace publicationCitationNs = "http://rs.tdwg.org/ontology/voc/PublicationCitation#";

                XNamespace commonNs = "http://rs.tdwg.org/ontology/voc/Common#";
                // Extract TaxonName data
                var taxonNameElement = xDocument.Descendants(taxonNameNs + "TaxonName").FirstOrDefault();
                var taxonName = new TaxonName
                {
                    rdf_about = taxonNameElement?.Attribute(rdf + "about")?.Value ?? string.Empty,
                    ns_Title = taxonNameElement?.Element(ns + "Title")?.Value ?? string.Empty,
                    owl_versionInfo = taxonNameElement?.Element(owl + "versionInfo")?.Value ?? string.Empty,
                    nameComplete = taxonNameElement?.Element(taxonNameNs + "nameComplete")?.Value ?? string.Empty,
                    genusPart = taxonNameElement?.Element(taxonNameNs + "genusPart")?.Value ?? string.Empty,
                    specificEpithet = taxonNameElement?.Element(taxonNameNs + "specificEpithet")?.Value ?? string.Empty,
                    authorship = taxonNameElement?.Element(taxonNameNs + "authorship")?.Value ?? string.Empty,
                    basionymAuthorship = taxonNameElement?.Element(taxonNameNs + "basionymAuthorship")?.Value ??
                                         string.Empty,
                    combinationAuthorship = taxonNameElement?.Element(taxonNameNs + "combinationAuthorship")?.Value ??
                                            string.Empty,
                    year = taxonNameElement?.Element(taxonNameNs + "year")?.Value ?? string.Empty,
                    microReference = taxonNameElement?.Element(taxonNameNs + "microReference")?.Value ?? string.Empty,
                    rankString = taxonNameElement?.Element(taxonNameNs + "rankString")?.Value ?? string.Empty,
                    //NomenclaturalCode = taxonNameElement?.Element(taxonNameNs + "nomenclaturalCode")?.Attribute(rdf + "resource")?.Value,
                    //HasBasionym = taxonNameElement?.Element(taxonNameNs + "hasBasionym")?.Attribute(rdf + "resource")?.Value
                };
                // Extract PublicationCitation data
                var publicationCitationElement = xDocument.Descendants(publicationCitationNs + "PublicationCitation")
                    .FirstOrDefault();
                var publicationCitation = new PublicationCitation
                {
                    //Year = int.TryParse(publicationCitationElement?.Element(publicationCitationNs + "year")?.Value, out int pubYear) ? pubYear : 0,
                    //Title = publicationCitationElement?.Element(publicationCitationNs + "title")?.Value,
                    //Volume = publicationCitationElement?.Element(publicationCitationNs + "volume")?.Value,
                    //Number = publicationCitationElement?.Element(publicationCitationNs + "number")?.Value,
                    //Pages = publicationCitationElement?.Element(publicationCitationNs + "pages")?.Value
                };
                IndexFungorumEntity result = new IndexFungorumEntity
                {
                    taxonName = taxonName,
                    publicationCitation = publicationCitation
                };
                return result;
            }
            catch (Exception ex)
            {
                throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
            }
        }
        public override IndexFungorumEntity GetEmptyDwbApiDetailModel()
        {
            var iofModel = new IndexFungorumEntity();
            return iofModel;
        }
    }
}
