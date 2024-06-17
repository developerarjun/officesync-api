using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OfficeSync.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected int CurrentUserId
        {
            get
            {
                var userId = HttpContext.User?.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier)?.Value;
                return string.IsNullOrEmpty(userId) ? 0 : int.Parse(userId);
            }
        }
        protected string CurrentUserEmail => HttpContext.User?.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Email)?.Value;
        protected string MfaProvider => HttpContext.User?.Claims.FirstOrDefault(f => f.Type == ClaimTypes.AuthorizationDecision)?.Value;

        protected string ClientUrl => HttpContext.Request.Headers["client-url"];
    }
}
