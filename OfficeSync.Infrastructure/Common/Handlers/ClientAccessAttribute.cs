using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OfficeSync.Infrastructure.Persistence.Initializers;
using System.Security.Claims;

namespace OfficeSync.Infrastructure.Common.Handlers
{
    public class ClientAccessAttribute : TypeFilterAttribute
    {
        public ClientAccessAttribute() : base(typeof(ClientAccessFilter)) { }
    }

    public class ClientAccessFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var roles = context.HttpContext.User.Claims.Where(w => w.Type == ClaimTypes.Role)
                                                           .Select(s => int.Parse(s.Value))
                                                           .ToArray();

                //if (!roles.Any(r => r == RoleInitializer.CUSTOMER))
                //{
                //    context.Result = new ForbidResult();
                //    return;
                //}
            }
        }
    }
}
