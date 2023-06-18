using Impact.Core.Contracts;
using Impact.Core.ViewModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators.OAuth2;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;


namespace Impact.Connectores
{
    public class CodeChallengeConnector : ICodeChallengeConnector
    {
        private readonly IIdentityProvider _identityProvider;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CodeChallengeConnectorOptions _options;


        public CodeChallengeConnector(IIdentityProvider identityProvider, IHttpClientFactory httpClientFactory, IOptions<CodeChallengeConnectorOptions> options)
        {
            _options = options.Value;

            _identityProvider = identityProvider;
            _httpClientFactory = httpClientFactory;
        }

        private async Task<string?> GetBearerToken()
        {
            string apiUrl = _options.ApiUrl + "Login";

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var content = new StringContent("{\"email\":\"" + _identityProvider.Email + "\"}", Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(apiUrl, content).ConfigureAwait(false);


                //// Create the request URL with the email as a query parameter
                //var requestUrl = $"{apiUrl}?email={HttpUtility.UrlEncode(_identityProvider.Email)}";

                //// Send the POST request to the API endpoint
                //var response = await httpClient.PostAsync(requestUrl, null);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserialize the response into an object
                    var loginResponse = System.Text.Json.JsonSerializer.Deserialize<LoginResponse>(jsonResponse);

                    // Retrieve the bearer token from the response
                    string bearerToken = loginResponse.token;

                    return bearerToken;
                }
            }

            return null;
        }

        private async Task<RestClient> GetRestClient(string routeUrl)
        {
            var bearerToken = await GetBearerToken().ConfigureAwait(false);

            var urlComplete = _options.ApiUrl + routeUrl;

            var restClientOptions = new RestClientOptions()
            {
                MaxTimeout = 1000000, //milliseconds == 500 seconds
                BaseUrl = new Uri(urlComplete),
            };

            var restClient = new RestClient(restClientOptions)
            {
                Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(bearerToken, "Bearer"),
            };

            return restClient;
        }

        public async Task<IList<Product>> GetAllProducts()
        {
            var urlComplete = _options.ApiUrl + "GetAllProducts";

            var restClient = await GetRestClient("GetAllProducts").ConfigureAwait(false);

            var restRequest = new RestRequest(urlComplete, Method.Get) { RequestFormat = DataFormat.Json };

            var restResponse = await restClient.ExecuteGetAsync<IList<Product>>(restRequest)
                .ConfigureAwait(false);

            if (restResponse.StatusCode == HttpStatusCode.OK)
                return restResponse.Data;

            if (restResponse.StatusCode == HttpStatusCode.NotFound || restResponse.StatusCode == HttpStatusCode.NoContent)
                return default;

            return null;
        }

        public async Task<Order> CreateOrder(Order order)
        {
            var urlComplete = _options.ApiUrl + "CreateOrder";

            var restClient = await GetRestClient("CreateOrder").ConfigureAwait(false);

            var restRequest = new RestRequest(urlComplete, Method.Post) { RequestFormat = DataFormat.Json };

            restRequest.AddBody(JsonConvert.SerializeObject(order), "application/json");

            //restRequest.AddJsonBody(order);

            var restResponse = await restClient.ExecuteGetAsync<Order>(restRequest)
              .ConfigureAwait(false);

            if (restResponse.StatusCode == HttpStatusCode.OK)
                return restResponse.Data;

            if (restResponse.StatusCode == HttpStatusCode.NotFound || restResponse.StatusCode == HttpStatusCode.NoContent)
                return default;

            return null;
        }

        public async Task<Order> GetOrder(string orderId)
        {
            var urlComplete = _options.ApiUrl + "GetOrder/" + orderId;

            var restClient = await GetRestClient("GetOrder").ConfigureAwait(false);

            var restRequest = new RestRequest(urlComplete, Method.Get) { RequestFormat = DataFormat.Json };

            var restResponse = await restClient.ExecuteGetAsync<Order>(restRequest)
                .ConfigureAwait(false);

            if (restResponse.StatusCode == HttpStatusCode.OK)
                return restResponse.Data;

            if (restResponse.StatusCode == HttpStatusCode.NotFound || restResponse.StatusCode == HttpStatusCode.NoContent)
                return default;

            return null;
        }
    }

    internal class LoginResponse
    {
        public string token { get; set; }
    }
}