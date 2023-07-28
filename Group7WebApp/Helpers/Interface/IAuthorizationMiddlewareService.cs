using Group7WebApp.Areas.Identity.Data;
using Group7WebApp.Data;

namespace Group7WebApp.Helpers.Interface
{
    public interface IAuthorizationMiddlewareService
    {
        
        Task<bool> Authorize(string action, WebAppUser user = null);
    }
}
