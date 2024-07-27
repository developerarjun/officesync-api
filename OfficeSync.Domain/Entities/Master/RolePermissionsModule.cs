using OfficeSync.Domain.Enumerations;
using OfficeSync.Domain.Interfaces;

namespace OfficeSync.Domain.Entities.Master
{
    public class RolePermissionsModule : BaseEntity<int>
    {
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
        public Permission Action { get; set; }
        public virtual Module ModuleRef { get; set; }
        public virtual IRole RoleRef { get; set; }
    }
}
