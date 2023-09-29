using System.Security.Claims;

namespace Travalers.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }
        public string UserId
        {
            get
            {
                if (_httpContextAccessor.HttpContext == null)
                    return null;

                return _httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }

    }
}
