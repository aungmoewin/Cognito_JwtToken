using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cognito.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Admin2Controller : ControllerBase
    {
        //private readonly IHttpContextAccessor _context;
        //public Admin2Controller(IHttpContextAccessor context)
        //{
        //    _context = context;
        //}

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public Item Index()
        {
            var user = (ClaimsIdentity)HttpContext.User.Identity;
            //System.Diagnostics.Debug.WriteLine(name);
            return new Item { Id = 123, Name = "RoleAdmin" };
        }
    }
}
