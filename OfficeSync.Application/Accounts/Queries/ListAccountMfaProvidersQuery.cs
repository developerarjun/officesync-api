using MediatR;
using OfficeSync.Application.Common.Interfaces;
using OfficeSync.Domain.Enumerations;

namespace OfficeSync.Application.Accounts.Queries
{
    public class ListAccountMfaProvidersHandler : IRequestHandler<ListAccountMfaProvidersQuery, MfaProvider[]>
    {
        private readonly IIdentityService _identityService;
        public ListAccountMfaProvidersHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<MfaProvider[]> Handle(ListAccountMfaProvidersQuery request, CancellationToken cancellationToken)
        {
            var providers = await _identityService.ListUserMfaProvidersAsync(request.CurrentUserEmail, cancellationToken);
            return providers;
        }
    }

    public class ListAccountMfaProvidersQuery : IRequest<MfaProvider[]>
    {
        public string CurrentUserEmail { get; set; }
    }
}
