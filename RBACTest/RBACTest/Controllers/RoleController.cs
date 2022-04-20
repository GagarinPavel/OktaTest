using Auth0.ManagementApi.Models;
using Microsoft.AspNetCore.Mvc;
using RBACTest.Services;

namespace RBACTest.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class RoleController : Controller
    {
        private Auth0Service _auth0 { get; set; }
        public RoleController(Auth0Service auth0)
        {
            _auth0 = auth0;
        }

        [HttpPost]
        public async Task CreateRole(string name, CancellationToken ct, string description = "")
        {
           await _auth0.CreateRole(name, ct, description);
        }

        [HttpGet]
        public async Task<IEnumerable<Role>> GetRoles(CancellationToken ct)
        {
            return await _auth0.GetRoles(ct);
        }
    }
}
