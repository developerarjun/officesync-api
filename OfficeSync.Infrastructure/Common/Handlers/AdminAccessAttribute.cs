using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using OfficeSync.Application.Common.Interfaces;
using OfficeSync.Domain.Enumerations;
using System.Security.Claims;

namespace OfficeSync.Infrastructure.Common.Handlers
{
    public class AdminAccessAttribute : TypeFilterAttribute
    {
        public AdminAccessAttribute() : base(typeof(AdminAccessFilter)) { }
    }

    public class AdminAccessFilter : IAsyncAuthorizationFilter
    {
        private readonly IOfficeSyncDbContext _dbContext;
        public AdminAccessFilter(IOfficeSyncDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //if (context.HttpContext.User.Identity.IsAuthenticated)
            //{
            //    var descriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            //    var module = GetModule(descriptor.ControllerName);
            //    if (module == 0)
            //    {
            //        context.Result = new NotFoundResult();
            //        return;
            //    }
            //    var action = GetAction(descriptor.ActionName);
            //    if (action == 0)
            //    {
            //        context.Result = new NotFoundResult();
            //        return;
            //    }

            //    var roles = context.HttpContext.User.Claims.Where(w => w.Type == ClaimTypes.Role)
            //                                               .Select(s => int.Parse(s.Value))
            //                                               .ToArray();

            //    var hasPermission = await _dbContext.RolePermissions.AnyAsync(a => roles.Any(ra => ra == a.RoleId) &&
            //                                                                       a.Module == module &&
            //                                                                       a.Action == action);

            //    if (!hasPermission)
            //    {
            //        context.Result = new ForbidResult();
            //        return;
            //    }
            //}
        }

        //private static Module GetModule(string controllerName)
        //{
        //    switch (controllerName)
        //    {
        //        case "Users":
        //            return Module.Users;
        //        case "Companies":
        //            return Module.Companies;
        //        case "Customers":
        //            return Module.Customers;
        //    }
        //    return 0;
        //}

        //private static Domain.Enumerations.Action GetAction(string actionName)
        //{
        //    switch (actionName)
        //    {
        //        case "List":
        //        case "Get":
        //        case "SelectOptions":
        //            return Domain.Enumerations.Action.Read;
        //        case "Create":
        //            return Domain.Enumerations.Action.Create;
        //        case "Update":
        //            return Domain.Enumerations.Action.Update;
        //        case "Delete":
        //            return Domain.Enumerations.Action.Delete;
        //        case "Invite":
        //            return Domain.Enumerations.Action.Invite;
        //        case "RegisterBoir":
        //            return Domain.Enumerations.Action.Update;
        //    }
        //    return 0;
        //}
    }
}
