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
    public class ModuleInitializer : BaseInitializer
    {
        public ModuleInitializer(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        public void SeedModules()
        {
            SeedModulesFromJSON().GetAwaiter().GetResult();
        }
        private async Task SeedModulesFromJSON()
        {
            List<Module> modules = await GetJsonModulesDataToSeed();
            if(modules is not null && modules.Count() > 0)
            {
                modules.ForEach(module =>
                {
                    _modelBuilder.Entity<Module>().HasData(module);
                });
            }
        }

        private async Task<List<Module>> GetJsonModulesDataToSeed()
        {
            string filepath = "Persistence\\Initializers\\SeedData";
            var modulesToSeed = await FileHelper.ReadJsonFile<List<Module>>("ModuleSeedData.json", filepath);

            return modulesToSeed;
        }
    }
}
