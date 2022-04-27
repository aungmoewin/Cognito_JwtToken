using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cognito.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        [Authorize(Roles = "CVX/agency")]
        [HttpGet]
        public Item Index()
        {
            var user = HttpContext.User.Identity;
            return new Item { Id = 123, Name = "PolicyAdmin" };
        }

        //[Authorize(Roles = "super_admin, agency_admin")]
        //public void Post(CreateUserInfo createUserInfo) {
        //    //this.User.IsSuperAdmin();
        //    authorizationSvc.ValidateCreateUser(this.User, createUserInfo);
        //}

        //[Authorize(Roles = "super_admin, agency_admin")]
        //public void Post(CreateUserInfo cameraAccessRequest)
        //{
        //    authorizationSvc.ValidateCameraAccess(this.User, camera, CameraActions.Control);
        //}
    }

    static class UserExtensions {
        public static bool IsSuperAdmin(this ClaimsPrincipal user) {
            return user.IsInRole("super_admin");
        }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
