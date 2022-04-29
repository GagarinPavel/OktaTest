using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0Test.Models;
using Newtonsoft.Json;

namespace Auth0Test.Services
{
    public class Auth0Service
    {
        public string adress = "https://dev---aps9vw.us.auth0.com/oauth/token";
        private AccessToken token { get; set; }

        public string clientId = "y6t9LmN8ejqRn2h0b5A2lI6ezkmY5R3O";

        public string clietnSecret = "NBX_u1K4d8P3tsPHxUzj562Frmahkfb3DFHNaSOy3OcsEpTE1sNrCgKZIJ5LC3mG";

        public string audience = "https://dev---aps9vw.us.auth0.com/api/v2/";

        public async Task GetToken()
        {
            HttpClient client = new HttpClient();

            var data = new Dictionary<string, string>
            {
                            { "grant_type", "client_credentials" },
                            { "client_id", $"{clientId}" },
                            { "client_secret", $"{clietnSecret}" },
                            { "audience", $"{audience}" }
            };
            var res = await client.PostAsync(adress, new FormUrlEncodedContent(data));
            token = JsonConvert.DeserializeObject<AccessToken>(await res.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var _httpClient = new HttpClient();
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{audience}users");

            requestMessage.Headers.Add("Authorization", $"Bearer {token.Access_token}");
            var users = await (await _httpClient.SendAsync(requestMessage)).Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<User>>(users);
        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            var _httpClient = new HttpClient();
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{audience}roles");

            requestMessage.Headers.Add("Authorization", $"Bearer {token.Access_token}");
            var roles = await (await _httpClient.SendAsync(requestMessage)).Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Role>>(roles);
        }

        public async Task CreateRole(string name, string description = "")
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Access_token}");
            var data = new Dictionary<string, string>
            {
                            { "name", $"{name}" },
                            { "description", $"{description}" }
            };
            var res = await client.PostAsync($"{audience}roles", new FormUrlEncodedContent(data));
            var i = 10;
        }

        public async Task AssignToRole(string roleId, string[] usersIds)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Access_token}");
            var data = new Dictionary<string, string[]>
            {
                            { "users", usersIds }
            };
            var res = await client.PostAsync($"{audience}roles/{roleId}/users", JsonContent.Create(data));
            var resCont = await res.Content.ReadAsStringAsync();
        }
    }
}
