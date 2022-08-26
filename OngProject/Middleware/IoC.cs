using Microsoft.Extensions.DependencyInjection;
using OngProject.Core.Mapper;
using OngProject.Repositories;
using OngProject.Repositories.Interfaces;
using OngProject.Services;
using OngProject.Services.Interfaces;

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
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            
            services.AddAutoMapper(typeof(EntityMapper));

            return services;
        }
    }
}
