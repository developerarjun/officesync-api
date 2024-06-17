using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OfficeSync.Application.Common.Behaviours;
using System.Reflection;

namespace OfficeSync.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            // Ref. https://code-maze.com/cqrs-mediatr-fluentvalidation/
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
