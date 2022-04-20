using Auth0.ManagementApi.Models;
using Microsoft.AspNetCore.Mvc;
using RBACTest.Services;

namespace RBACTest.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class UsersController : Controller
    {
        private Auth0Service _auth0 { get; set; }
        public UsersController(Auth0Service auth0)
        {
            _auth0 = auth0;
        }

        [HttpPost]
        public async Task AssignToRole(string roleId, string[] userIds, CancellationToken ct)
        {
            await _auth0.AssignToRole(roleId, userIds, ct);
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetUsers(CancellationToken ct)
        {
            return await _auth0.GetUsers(ct);
        }
    }
}
