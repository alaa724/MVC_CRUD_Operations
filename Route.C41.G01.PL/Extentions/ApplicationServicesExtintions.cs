using Microsoft.Extensions.DependencyInjection;
using Route.C41.G01.BL.Interfaces;
using Route.C41.G01.BL.Repositories;

namespace Route.C41.G01.PL.Extentions
{
    public static class ApplicationServicesExtintions
    {
        public static void ApplicationServices(this IServiceCollection services)
        {
            //services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //services.AddSingleton<IDepartmentRepository, DepartmentRepository>();

            services.AddScoped<IEmployeeInterface, EmployeeRepository>();
        }
    }
}
