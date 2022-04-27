using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cognito.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public Item Index()
        {
            var user = HttpContext.User.Identity;
            return new Item { Id = 123, Name = "PolicyAdmin" };
        } 
    }
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
