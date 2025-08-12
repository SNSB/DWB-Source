using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DWBServices.WebServices.TaxonomicServices.Mycobank
{
    public class MycobankWebservice : TaxonomicWebservice, IDwbWebservice<TaxonomicSearchResult, TaxonomicSearchResultItem, TaxonomicEntity>
    {
        public string? _cachedToken;
        private DateTime _tokenExpiration;
        private IConfiguration? _configuration;
        private string ne;
        private string of;
        public MycobankWebservice(HttpClient httpClient) : base(httpClient)
        {
            // Access IConfiguration from the static DwbServiceProviderAccessor
            SetConfiguration();
            httpClient.DefaultRequestHeaders.Add(_configuration["Mycobank:Custom_Header"], _configuration["Mycobank:Custom_Id"]);
            string BaseAddress = this.GetBaseAddress();
            httpClient.BaseAddress = new Uri(BaseAddress);
        }

        private void SetConfiguration()
        {
            if (_configuration == null)
            {
                _configuration = DwbServiceProviderAccessor.Instance?.GetRequiredService<IConfiguration>()
                                ?? throw new InvalidOperationException("IConfiguration service is not available.");
            }
        }
        private async Task<string> GetCachedTokenAsync()
        {
            if (_cachedToken != null && _tokenExpiration > DateTime.Now)
            {
                return _cachedToken;
            }
            var tokenResponse = await GetBearerTokenAsync();
            _cachedToken = tokenResponse.AccessToken;
            _tokenExpiration = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn - 60);
            return _cachedToken;
        }
        
        public async Task<TokenResponse> GetBearerTokenAsync()
        {
            var authServerTokenEndpoint = getTokenEndpoint();
            var request = new HttpRequestMessage(HttpMethod.Post, authServerTokenEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var formData = getFormData();
            request.Content = formData;
            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            // Parse the token from the response
            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);
            return tokenResponse ?? throw new InvalidOperationException("Token retrieval failed.");
        }
        public class TokenResponse
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; }
            [JsonPropertyName("token_type")]
            public string TokenType { get; set; }
            [JsonPropertyName("expires_in")]
            public int ExpiresIn { get; set; }
        }

        public override async Task<T> CallWebServiceAsync<T>(string url,
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
        public override async Task<T> CallWebServiceAsync<T>(
            string queryContent,
            DwbServiceEnums.HttpAction action = DwbServiceEnums.HttpAction.GET,
            HttpContent? content = null)
        {
            string token = await GetCachedTokenAsync();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage? response;

            // For the mycobank get request, the url (containing the queryparams) have to be added as content to a post request.
            // We need to you a POST request for getting the data from Mycobank via  an expression
            string url = ""; // there are not aprameters in the url for this webservice
            if (queryContent.StartsWith("{\"Query\""))
            {
                content = new StringContent(
                    queryContent,
                    System.Text.Encoding.UTF8,
                    "application/json"
                );
                url = "search/Mycobank%20WS";
                response = await HttpClient.PostAsync(url, content);
            }
            else
            {
                url = queryContent;
                response = await HttpClient.GetAsync(url);
            }
            

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new InvalidOperationException("The json response was empty");
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

        // For the Mycobank Webservice POST is used to get the search data. So this urlstring is not added to the url, but to the content. (see CallWebServiceAsync)
        public override string DwbApiQueryUrlString(DwbServiceEnums.DwbService currentService, string queryRestrictions, int offset, int maxPerPage)
        {
            MycobankTaxonomicSearchCriterias criterias = new MycobankTaxonomicSearchCriterias();
            if (criterias.ValidateQueryRestrictions(queryRestrictions, offset, maxPerPage))
            {
                criterias.query = queryRestrictions;
                // no offset and maxPerPage fot this webservice
                //criterias.offset = offset.ToString();
                //criterias.maxPerPage = maxPerPage.ToString();
            }
            else
            {
                throw new ArgumentException($"{queryRestrictions} is not a valid query restriction",
                    nameof(queryRestrictions));
            }

            return criterias.QueryParamString;
        }

        private HttpContent? getFormData()
        {
            if (string.IsNullOrEmpty(ne) || (string.IsNullOrEmpty(of)))
            {
                setNE();
            }
            if (string.IsNullOrEmpty(ne) || (string.IsNullOrEmpty(of)))
                throw new InvalidOperationException("Token retrieval failed.");

            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", _configuration["Mycobank:grant_type"]),
                new KeyValuePair<string, string>("client_id", ne),
                new KeyValuePair<string, string>("client_secret", of)
            });
            return formData;
        }
        private void setNE()
        {
            ne = HelpMethod(_configuration["Mycobank:id"], _configuration["Mycobank:version"]);
            of = HelpMethod(_configuration["Mycobank:type"], _configuration["Mycobank:version"]);
        }
        private string getTokenEndpoint()
        {
            string settingValue = _configuration["Mycobank:Mycobank_TokenAddress"];
            return settingValue;
        }
        public override string GetBaseAddress()
        {
            string settingValue = _configuration["Mycobank:Mycobank_BaseAddress"];
            return settingValue;
        }

        public override DwbServiceEnums.DwbService GetServiceName()
        {
            return DwbServiceEnums.DwbService.CatalogueOfLife;
        }

        public override string GetServiceUri(DwbServiceEnums.DwbService currentService)
        {
            string settingValue = _configuration["Mycobank:Mycobank_ServiceUri"];
            return settingValue;
        }

        public override MycobankSearchResultItem GetDwbApiSearchModel<T>(T tt)
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(tt);
                var mycobankModel = JsonSerializer.Deserialize<MycobankSearchResultItem>(jsonString);

                var test = mycobankModel?.GetMappedApiClientModel();
                return mycobankModel;
            }
            catch (Exception ex)
            {
                throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
            }
        }

        public override MycobankSearchResult GetDwbApiSearchResultModel<T>(T tt)
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(tt);
                var mycobankModelResponse = JsonSerializer.Deserialize<MycobankSearchResult>(jsonString);
                if (mycobankModelResponse != null)
                {
                    mycobankModelResponse.ApiJsonResponse = jsonString;
                }

                return mycobankModelResponse;
            }
            catch (Exception ex)
            {
                throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
            }
        }

        public override MycobankEntity GetDwbApiDetailModel<T>(T tt)
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(tt);
                var mycobankModel = JsonSerializer.Deserialize<MycobankEntity>(jsonString);
                if (mycobankModel != null)
                {
                    mycobankModel.ApiJsonResponse = jsonString;
                }

                return mycobankModel;
            }
            catch (Exception ex)
            {
                throw new DataMappingException("An error occurred while mapping data in GetDwbApiDetailModel.", ex);
            }
        }

        public override MycobankEntity GetEmptyDwbApiDetailModel()
        {
            var mycobankModel = new MycobankEntity();
            return mycobankModel;
        }
        public string HelpMethod(string text, string help)
        {
            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(help.PadRight(8).Substring(0, 8));
                byte[] cipherBytes = Convert.FromBase64String(text);
                using (var des = DES.Create())
                {
                    des.Key = keyBytes;
                    des.Mode = CipherMode.ECB;
                    des.Padding = PaddingMode.PKCS7;
                    using (var decryptor = des.CreateDecryptor())
                    using (var ms = new MemoryStream(cipherBytes))
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var reader = new StreamReader(cs))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MycobankWebservice: HelpMethod failed: {ex}");
                return "";
            }
        }
    }
}
