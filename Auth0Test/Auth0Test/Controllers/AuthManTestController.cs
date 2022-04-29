using Auth0.ManagementApi;
using Auth0Test.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth0Test.Controllers
{
    public class AuthManTestController : Controller
    {
        public async Task<IActionResult> Index()
        {
            /*var client = new RestClient("https://dev---aps9vw.us.auth0.com/oauth/token");
            var request = new RestRequest("", method: Method.Post);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "{\"client_id\":\"y6t9LmN8ejqRn2h0b5A2lI6ezkmY5R3O\",\"client_secret\":\"NBX_u1K4d8P3tsPHxUzj562Frmahkfb3DFHNaSOy3OcsEpTE1sNrCgKZIJ5LC3mG\",\"audience\":\"https://localhost:5001\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
            RestResponse response = await client.ExecutePostAsync(request);*/

            var Auth0Service = new Auth0Service();
            await Auth0Service.GetToken();
            var users = await Auth0Service.GetUsers();
            var roles = await Auth0Service.GetRoles();
            await Auth0Service.CreateRole("new role from api", null);
            await Auth0Service.AssignToRole("rol_LaqLuXcau33GxWwk", new string[] { "auth0|625fb1b81109db006a6e9046" });
            return View();
        }
    }
}
