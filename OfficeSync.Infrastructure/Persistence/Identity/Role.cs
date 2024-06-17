using Microsoft.AspNetCore.Identity;
using OfficeSync.Domain.Interfaces;

namespace OfficeSync.Infrastructure.Persistence.Identity
{
    public class Role : IdentityRole<int>, IRole
    {
        public virtual Role ParentRef { get; set; }
    }
}
