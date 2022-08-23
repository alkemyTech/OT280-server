using Microsoft.Extensions.DependencyInjection;
using OngProject.Core.Mapper;
using OngProject.Repositories;
using OngProject.Repositories.Interfaces;


namespace OngProject.Middleware
{
    public static class IoC
    {
        public static IServiceCollection AddDependency(this IServiceCollection services)
        {
            // Inject generic repository services
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddAutoMapper(typeof(EntityMapper));

            return services;
        }
    }
}
