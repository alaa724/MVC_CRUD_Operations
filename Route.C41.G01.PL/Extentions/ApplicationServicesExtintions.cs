﻿using Microsoft.Extensions.DependencyInjection;
using Route.C41.G01.BL;
using Route.C41.G01.BL.Interfaces;
using Route.C41.G01.BL.Repositories;

namespace Route.C41.G01.PL.Extentions
{
    public static class ApplicationServicesExtintions
    {
        public static IServiceCollection ApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            //services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //services.AddSingleton<IDepartmentRepository, DepartmentRepository>();
            //services.AddScoped<IEmployeeInterface, EmployeeRepository>();

            return services;
        }
    }
}