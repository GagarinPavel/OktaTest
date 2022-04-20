using Auth0.ManagementApi.Models;
using Newtonsoft.Json;
using RBACTest.Models;

namespace RBACTest.Services
{

    public class Auth0Service
    {
        public string adress { get; set; }
        private AccessToken? token { get; set; }

        public string clientId { get; set; }

        public string clietnSecret { get; set; }

        public string audience { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;
        public Auth0Service(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            this.adress = config["Auth0:Adress"];
            this.clientId = config["Auth0:ClientId"];
            this.clietnSecret = config["Auth0:ClientSecret"];
            this.audience = config["Auth0:Audience"];
            _httpClientFactory = httpClientFactory;
            GetToken();
        }

        private void GetToken()
        {
            HttpClient client = _httpClientFactory.CreateClient();

            var data = new Dictionary<string, string>
            {
                            { "grant_type", "client_credentials" },
                            { "client_id", $"{clientId}" },
                            { "client_secret", $"{clietnSecret}" },
                            { "audience", $"{audience}" }
            };
            var res = client.PostAsync(adress, new FormUrlEncodedContent(data)).Result;
            token = JsonConvert.DeserializeObject<AccessToken>(res.Content.ReadAsStringAsync().Result);
        }

        public async Task<IEnumerable<User>> GetUsers(CancellationToken ct)
        {
            var _httpClient = _httpClientFactory.CreateClient();
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{audience}users");

            requestMessage.Headers.Add("Authorization", $"Bearer {token?.Access_token}");
            var users = await (await _httpClient.SendAsync(requestMessage, ct)).Content.ReadAsStringAsync(ct);
            return JsonConvert.DeserializeObject<IEnumerable<User>>(users);
        }

        public async Task<IEnumerable<Role>> GetRoles(CancellationToken ct)
        {
            var _httpClient = _httpClientFactory.CreateClient();
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{audience}roles");

            requestMessage.Headers.Add("Authorization", $"Bearer {token?.Access_token}");
            var roles = await (await _httpClient.SendAsync(requestMessage, ct)).Content.ReadAsStringAsync(ct);
            return JsonConvert.DeserializeObject<IEnumerable<Role>>(roles);
        }

        public async Task CreateRole(string name, CancellationToken ct, string description = "")
        {
            HttpClient client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token?.Access_token}");
            var data = new Dictionary<string, string>
            {
                            { "name", $"{name}" },
                            { "description", $"{description}" }
            };
            await client.PostAsync($"{audience}roles", new FormUrlEncodedContent(data), ct);
        }

        public async Task AssignToRole(string roleId, string[] usersIds, CancellationToken ct)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token?.Access_token}");
            var data = new Dictionary<string, string[]>
            {
                            { "users", usersIds }
            };
            await client.PostAsync($"{audience}roles/{roleId}/users", JsonContent.Create(data), ct);
        }
    }
}
