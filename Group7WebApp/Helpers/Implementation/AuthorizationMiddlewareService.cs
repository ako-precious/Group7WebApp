using Group7WebApp.Areas.Identity.Data;
using Group7WebApp.Helpers.Interface;
using Microsoft.AspNetCore.Identity;
using System.Configuration;

namespace Group7WebApp.Helpers.Implementation
{
    public class AuthorizationMiddlewareService : IAuthorizationMiddlewareService
    {
        private static IHttpContextAccessor _httpContextAccessor;
        private static UserManager<WebAppUser> _userManager;
        public AuthorizationMiddlewareService(IHttpContextAccessor httpContextAccessor, UserManager<WebAppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;

        }

        public async Task<bool> Authorize(string action, WebAppUser user = null)
        {
            var loggedInUser = _httpContextAccessor.HttpContext.User;
            if (loggedInUser == null)
                return false;

            switch (action)
            {
                case Priviledge.Create:
                    if (loggedInUser.IsInRole(Roles.ManagerRole))
                        return false;
                    break;
                case Priviledge.Approve:
                    if (loggedInUser.IsInRole(Roles.UserRole))
                        return false;
                    break;
                case Priviledge.Edit:
                case Priviledge.Delete:
                    if (loggedInUser.IsInRole(Roles.ManagerRole) || (loggedInUser.IsInRole(Roles.UserRole) && (user == null || user.ContactId != _userManager.GetUserId(loggedInUser))))
                        return false;
                    break;

                default:
                    break;
            }

            return true;
        }
    }
}
