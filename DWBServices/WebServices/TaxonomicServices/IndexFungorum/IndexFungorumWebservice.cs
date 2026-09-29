using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
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

        public override async Task<TaxonomicEntity> GetEntityHierarchyAsync<T>(string url, TaxonomicEntity dwbEntity, CancellationToken cancellationToken)
        {
            var hierarchy = new List<T>();
            if (string.IsNullOrEmpty(url))
            {
                return dwbEntity;
            }
            IndexFungorumEntity indexEntity = (dwbEntity as IndexFungorumEntity);
            url = url.TrimEnd('/');
            int lastIndex = url.LastIndexOf('=');
            string baseUrl = "https://www.indexfungorum.org/IXFWebService/Fungus.asmx/NameByKey?NameKey="; // to get the hierarchy we need to call another endpoint for indexfungorum
            string currentId = url.Substring(lastIndex + 1);    // Everything after the '='

            if (!string.IsNullOrEmpty(currentId))
            {
                // Perform web request to fetch the entity by ID
                var response = await CallWebServiceAsync<T>($"{baseUrl}{currentId}", cancellationToken, DwbServiceEnums.HttpAction.GET);
                var hierarchyentity = GetDwbApiHierarchyModel<T>(response);
                if (hierarchyentity == null)
                {
                    return dwbEntity;
                }
                dynamic mappedClientModel = (hierarchyentity as dynamic).GetMappedApiEntityModel();
                // Add the current entity to the hierarchy
                if (mappedClientModel != null)
                {
                    hierarchy.Add((T)(object)mappedClientModel);
                    string newHierarchy = mappedClientModel.Hierarchy;
                    indexEntity.taxonName.hierarchy = newHierarchy.Trim(' ', '|');
                    indexEntity.taxonName.family = mappedClientModel.Family;
                    indexEntity.taxonName.order = mappedClientModel.Order;
                    indexEntity.taxonName.genus = mappedClientModel.Genus;
                    indexEntity.Hierarchy = newHierarchy.Trim(' ', '|');
                    indexEntity.Family = mappedClientModel.Family;
                    indexEntity.Order = mappedClientModel.Order;
                    indexEntity.Genus = mappedClientModel.Genus;
                }
            }
            return dwbEntity;
        }

        public string TransformUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;

            // Replace old endpoint with new endpoint
            if (url.Contains("NameByKeyRDF?NameLsid="))
            {
                url = url.Replace("NameByKeyRDF?NameLsid=", "NameByKey?NameKey=");
            }

            return url;
        }

        public override async Task<T> CallWebServiceAsync<T>(
            string url, CancellationToken cancellationToken,
            DwbServiceEnums.HttpAction action = DwbServiceEnums.HttpAction.GET,
            HttpContent? content = null)
        {
            HttpResponseMessage? response;
            
            try {
                string transformedUrl = TransformUrl(url);
                // Set a timeout of 1 minute
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromMinutes(1));
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

                response = await HttpClient.GetAsync(transformedUrl, linkedCts.Token);

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
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override string DwbApiQueryUrlString(DwbServiceEnums.DwbService currentService, string queryRestrictions, int offset, int maxPerPage)
        {
            // get datasetkey from current service
            var serviceInfoDictionary = DwbServiceEnums.TaxonomicServiceInfoDictionary();

            IndexFungorumTaxonomicSearchCriterias criteria = new IndexFungorumTaxonomicSearchCriterias();
            queryRestrictions = Uri.EscapeDataString(queryRestrictions);
            if (criteria.ValidateQueryRestrictions(queryRestrictions, offset, maxPerPage))
            {
                if (serviceInfoDictionary.TryGetValue(currentService, out var serviceInfo))
                {
                    criteria.endpoint = serviceInfo.SearchEndpoint;
                }
                
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

                var indexFungorumElement = xDocument.Descendants("IndexFungorum").FirstOrDefault();

                if (indexFungorumElement == null)
                {
                    return null;
                }

                var taxonName = new TaxonName
                {
                    nameComplete = indexFungorumElement?.Element("NAME_x0020_OF_x0020_FUNGUS")?.Value ?? string.Empty,
                    authorship = indexFungorumElement?.Element("AUTHORS")?.Value ?? string.Empty,
                    specificEpithet = indexFungorumElement?.Element("SPECIFIC_x0020_EPITHET")?.Value ?? string.Empty,
                    rankString = indexFungorumElement?.Element("INFRASPECIFIC_x0020_RANK")?.Value ?? string.Empty,
                    infraspecificEpithet = indexFungorumElement?.Element("INFRASPECIFIC_x0020_EPITHET")?.Value ?? string.Empty,
                    year = indexFungorumElement?.Element("YEAR_x0020_OF_x0020_PUBLICATION")?.Value ?? string.Empty,
                    family = indexFungorumElement?.Element("Family_x0020_name")?.Value ?? string.Empty,
                    order = indexFungorumElement?.Element("Order_x0020_name")?.Value ?? string.Empty,
                    genus = indexFungorumElement?.Element("Genus_x0020_name")?.Value ?? string.Empty,
                    record_number = indexFungorumElement?.Element("RECORD_x0020_NUMBER")?.Value ?? string.Empty,
                    current_name = indexFungorumElement?.Element("CURRENT_x0020_NAME")?.Value ?? string.Empty
                };

                // Build hierarchy from taxonomy fields
                var hierarchyParts = new List<string>();
                var kingdom = indexFungorumElement?.Element("Kingdom_x0020_name")?.Value;
                var phylum = indexFungorumElement?.Element("Phylum_x0020_name")?.Value;
                var subphylum = indexFungorumElement?.Element("Subphylum_x0020_name")?.Value;
                var classElement = indexFungorumElement?.Element("Class_x0020_name")?.Value;
                var subclass = indexFungorumElement?.Element("Subclass_x0020_name")?.Value;
                var order = indexFungorumElement?.Element("Order_x0020_name")?.Value;
                var family = indexFungorumElement?.Element("Family_x0020_name")?.Value;

                if (!string.IsNullOrEmpty(kingdom)) hierarchyParts.Add(kingdom);
                if (!string.IsNullOrEmpty(phylum)) hierarchyParts.Add(phylum);
                if (!string.IsNullOrEmpty(subphylum)) hierarchyParts.Add(subphylum);
                if (!string.IsNullOrEmpty(classElement)) hierarchyParts.Add(classElement);
                if (!string.IsNullOrEmpty(subclass)) hierarchyParts.Add(subclass);
                if (!string.IsNullOrEmpty(order)) hierarchyParts.Add(order);
                if (!string.IsNullOrEmpty(family)) hierarchyParts.Add(family);

                taxonName.hierarchy = string.Join(" > ", hierarchyParts);

                var publicationCitation = new PublicationCitation
                {
                    year = indexFungorumElement?.Element("YEAR_x0020_OF_x0020_PUBLICATION")?.Value ?? string.Empty,
                    title = indexFungorumElement?.Element("pubAcceptedTitle")?.Value ?? string.Empty,
                    volume = indexFungorumElement?.Element("VOLUME")?.Value ?? string.Empty,
                    pages = indexFungorumElement?.Element("PAGE")?.Value ?? string.Empty
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

        public IndexFungorumEntity GetDwbApiHierarchyModel<T>(T tt)
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
                // Extract TaxonName classification data
                var taxonNameElement = xDocument.Descendants("IndexFungorum").FirstOrDefault();
                var taxonName = new TaxonName
                {
                    family = taxonNameElement?.Element("Family_x0020_name")?.Value ?? string.Empty,
                    order = taxonNameElement?.Element("Order_x0020_name")?.Value ?? string.Empty,
                    genus = taxonNameElement?.Element("Genus_x0020_name")?.Value ?? string.Empty,
                    hierarchy = ((taxonNameElement?.Element("Kingdom_x0020_name")?.Value ?? string.Empty) + " | " +
                    (taxonNameElement?.Element("Phylum_x0020_name")?.Value ?? string.Empty) + " | " +
                    (taxonNameElement?.Element("Subphylum_x0020_name")?.Value ?? string.Empty) + " | " +
                    (taxonNameElement?.Element("Class_x0020_name")?.Value ?? string.Empty) + " | " +
                    (taxonNameElement?.Element("Subclass_x0020_name")?.Value ?? string.Empty) + " | " +
                    (taxonNameElement?.Element("Order_x0020_name")?.Value ?? string.Empty) + " | " +
                    (taxonNameElement?.Element("Family_x0020_name")?.Value ?? string.Empty) + " | " +
                    (taxonNameElement?.Element("Genus_x0020_name")?.Value ?? string.Empty) + " | " +
                    (taxonNameElement?.Element("NAME_x0020_OF_x0020_FUNGUS")?.Value ?? string.Empty))
                };

                IndexFungorumEntity result = new IndexFungorumEntity
                {
                    taxonName = taxonName
                };
                return result;
            }
            catch (Exception ex)
            {
                throw new DataMappingException("An error occurred while mapping data in GetDwbApiHierarchyModel.", ex);
            }
        }

        public override IndexFungorumEntity GetEmptyDwbApiDetailModel()
        {
            var iofModel = new IndexFungorumEntity();
            return iofModel;
        }
    }
}
