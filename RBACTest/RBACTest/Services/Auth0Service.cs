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

        private readonly HttpClient _httpClient;

        public Auth0Service(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            this.adress = config["Auth0:Adress"];
            this.clientId = config["Auth0:ClientId"];
            this.clietnSecret = config["Auth0:ClientSecret"];
            this.audience = config["Auth0:Audience"];
            _httpClient = httpClientFactory.CreateClient();
            GetToken();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token?.Access_token}");
        }

        private void GetToken()
        {
            var data = new Dictionary<string, string>
            {
                            { "grant_type", "client_credentials" },
                            { "client_id", $"{clientId}" },
                            { "client_secret", $"{clietnSecret}" },
                            { "audience", $"{audience}" }
            };
            var res = _httpClient.PostAsync(adress, new FormUrlEncodedContent(data)).Result;
            token = JsonConvert.DeserializeObject<AccessToken>(res.Content.ReadAsStringAsync().Result);
        }

        public async Task<IEnumerable<User>> GetUsers(CancellationToken ct)
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{audience}users");
            var users = await (await _httpClient.SendAsync(requestMessage, ct)).Content.ReadAsStringAsync(ct);
            return JsonConvert.DeserializeObject<IEnumerable<User>>(users);
        }

        public async Task<IEnumerable<Role>> GetRoles(CancellationToken ct)
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{audience}roles");
            var roles = await (await _httpClient.SendAsync(requestMessage, ct)).Content.ReadAsStringAsync(ct);
            return JsonConvert.DeserializeObject<IEnumerable<Role>>(roles);
        }

        public async Task CreateRole(string name, CancellationToken ct, string description = "")
        {
            var data = new Dictionary<string, string>
            {
                            { "name", $"{name}" },
                            { "description", $"{description}" }
            };
            await _httpClient.PostAsync($"{audience}roles", new FormUrlEncodedContent(data), ct);
        }

        public async Task AssignToRole(string roleId, string[] usersIds, CancellationToken ct)
        {
            var data = new Dictionary<string, string[]>
            {
                            { "users", usersIds }
            };
            await _httpClient.PostAsync($"{audience}roles/{roleId}/users", JsonContent.Create(data), ct);
        }
    }
}
