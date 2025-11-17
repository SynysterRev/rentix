using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Rentix.Application.Common.Behaviors;
using Rentix.Application.RealEstate.Mappers;
using System.Reflection;

namespace Rentix.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);

                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(assembly);

            services.AddScoped<IPropertyMapper, PropertyMapper>();

            return services;
        }
    }
}
