using Microsoft.EntityFrameworkCore;
using OfficeSync.Application.Common.Helpers;
using OfficeSync.Domain.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeSync.Infrastructure.Persistence.Initializers
{
    public class RolePermissionModuleInitializer : BaseInitializer
    {
        public RolePermissionModuleInitializer(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        public void SeedRolePermissionModules()
        {
            SeedrolePermissionsModulesFromJSON().GetAwaiter().GetResult();
        }

        private async Task SeedrolePermissionsModulesFromJSON()
        {
            List<RolePermissionsModule> modules = await GetJsonModulesRolePermissionDataToSeed();
            if (modules is not null && modules.Count() > 0)
            {
                modules.ForEach(module =>
                {
                    _modelBuilder.Entity<RolePermissionsModule>().HasData(module);
                });
            }
        }

        private async Task<List<RolePermissionsModule>> GetJsonModulesRolePermissionDataToSeed()
        {
            string filepath = "Persistence\\Initializers\\SeedData";
            var rolePermissionsModuleToSeed = await FileHelper.ReadJsonFile<List<RolePermissionsModule>>("RolePermissionsModuleSeedData.json", filepath);

            return rolePermissionsModuleToSeed;
        }
    }
}
