using System.Text;
using System.Text.Json;

namespace DWBServices.WebServices
{
    /// <summary>
    /// Represents the base class for search results returned by DWB web services.
    /// </summary>
    /// <remarks>
    /// This abstract class serves as a foundation for specific search result implementations
    /// in various domains, such as taxonomic and geographic services. It provides a common
    /// structure for handling API responses and search result items.
    /// </remarks>
    public abstract class DwbSearchResult
    {
        public string ApiJsonResponse { get; set; }
        public abstract DwbSearchResultItem[] DwbApiSearchResponse { get; }
    }
    /// <summary>
    /// Represents an abstract base class for items in the search result of the DWB web services.
    /// </summary>
    /// <remarks>
    /// This class serves as a foundation for specific types of search result items, 
    /// such as those used in taxonomic or geographic services. It provides common properties 
    /// like a URL and display text, which can be extended by derived classes.
    /// </remarks>
    public abstract class DwbSearchResultItem
    {
        public string _URL { get; set; }

        public string _DisplayText { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public abstract class DwbEntity
    {
        public string ApiJsonResponse { get; set; }

        public string _URL { get; set; }

        public string _DisplayText { get; set; }

        public abstract DwbEntity? GetMappedApiEntityModel();

        public virtual string GetDisplayText()
        {
            return _DisplayText;
        }
    }
    /// <summary>
    /// Represents an abstract base class for web services in the DWBServices namespace.
    /// Provides functionality for interacting with web services, including making HTTP requests
    /// and handling responses, as well as defining service-specific behaviors.
    /// </summary>
    /// <typeparam name="TDwbSearchResult">
    /// The type representing the search result model returned by the web service.
    /// </typeparam>
    /// <typeparam name="TDwbSearchResultItem">
    /// The type representing individual search result items within the search result model.
    /// </typeparam>
    /// <typeparam name="TDwbEntity">
    /// The type representing the detailed entity model returned by the web service.
    /// </typeparam>
    public abstract class DwbWebservice<TDwbSearchResult, TDwbSearchResultItem, TDwbEntity>: IDwbWebservice<TDwbSearchResult, TDwbSearchResultItem, TDwbEntity>
        where TDwbSearchResult : class
        where TDwbSearchResultItem : class
        where TDwbEntity : class
    {
        protected readonly HttpClient HttpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="DwbWebservice{TDwbSearchResult, TDwbSearchResultItem, TDwbEntity}"/> class.
        /// </summary>
        /// <param name="httpClient">
        /// An instance of <see cref="HttpClient"/> used to send HTTP requests to the web service.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the <paramref name="httpClient"/> parameter is <c>null</c>.
        /// </exception>
        protected DwbWebservice(HttpClient httpClient)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Asynchronously calls a web service with the specified URL, content, and HTTP action.
        /// </summary>
        /// <typeparam name="T">The type of the response expected from the web service.</typeparam>
        /// <param name="url">The URL of the web service to call.</param>
        /// <param name="content">The content to send with the request, formatted as a JSON string.</param>
        /// <param name="action">
        /// The HTTP action to perform (e.g., GET). Defaults to <see cref="DwbServiceEnums.HttpAction.GET"/>.
        /// </param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response of type <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the provided URL is not valid.</exception>
        public virtual async Task<T> CallWebServiceAsync<T>(string url,
            string content,
            DwbServiceEnums.HttpAction action = DwbServiceEnums.HttpAction.GET)
        {
            if (IsValidUrl(url) == false)
            {
                throw new ArgumentException("The provided URL is not valid.");
            }
            StringContent contentString = new(content, Encoding.UTF8, "application/json");
            return await CallWebServiceAsync<T>(url, action, contentString);
        }
        /// <summary>
        /// Sends an HTTP request to the specified URL and processes the response asynchronously.
        /// </summary>
        /// <typeparam name="T">The type to which the response content will be deserialized.</typeparam>
        /// <param name="url">The URL of the web service to call.</param>
        /// <param name="action">The HTTP action to perform (e.g., GET). Defaults to <see cref="DwbServiceEnums.HttpAction.GET"/>.</param>
        /// <param name="content">The HTTP content to send with the request, if applicable. Defaults to <c>null</c>.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the deserialized response of type <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the URL is invalid or other preconditions are not met.</exception>
        /// <exception cref="HttpRequestException">Thrown if the HTTP request fails or the response indicates an error.</exception>
        public virtual async Task<T> CallWebServiceAsync<T>(
            string url,
            DwbServiceEnums.HttpAction action = DwbServiceEnums.HttpAction.GET,
            HttpContent? content = null)
        {
            HttpResponseMessage? response;

            //TODO if we want to implement POST etc. we have to distinguish here
            // change this to above if we also want post, put, update etc.
            response = await HttpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new InvalidOperationException("No match - response from the web service was empty");
                }
                var result = JsonSerializer.Deserialize<T>(json);
                if (result == null)
                {
                    throw new InvalidOperationException("Deserialization resulted in null.");
                }
                return result;
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorContent}");
            }
        }

        public abstract string GetBaseAddress();

        public abstract string GetServiceUri(DwbServiceEnums.DwbService currentService);

        public abstract DwbServiceEnums.DwbService GetServiceName();
        
        public virtual bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;

            var result = Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
                         (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return result;
        }

        public abstract string DwbApiQueryUrlString(DwbServiceEnums.DwbService currentService, string queryRestrictions, int offset, int maxPerPage);
        public abstract TDwbSearchResultItem GetDwbApiSearchModel<T>(T tt);

        public abstract TDwbSearchResult GetDwbApiSearchResultModel<T>(T tt);

        public abstract TDwbEntity GetDwbApiDetailModel<T>(T tt);

        public abstract TDwbEntity GetEmptyDwbApiDetailModel();

    }
}
