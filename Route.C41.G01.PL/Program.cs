using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Route.C41.G01.DAL.Data;
using Route.C41.G01.DAL.Models;
using Route.C41.G01.PL.Extentions;
using Route.C41.G01.PL.Helpers;
using System;

namespace Route.C41.G01.PL
{
	public class Program
	{
		// Entry Point
		public static void Main(string[] args)
		{
			#region Configur Services
			var webApplicationBuilder = WebApplication.CreateBuilder(args);

			webApplicationBuilder.Services.AddControllersWithViews(); // Register Built-In webApplicationBuilder.Services Required by MVC

			//webApplicationBuilder.Services.AddTransient<ApplicationDbContext>();
			//services.AddSingleton<ApplicationDbContext>();

			//services.AddScoped<ApplicationDbContext>();
			//services.AddScoped<DbContextOptions<ApplicationDbContext>>();

			webApplicationBuilder.Services.AddDbContext<ApplicationDbContext>(options =>
			{
				object value = options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			}/*,ServiceLifetime.Scoped*/);

			//ApplicationwebApplicationBuilder.ServicesExtintions.ApplicationwebApplicationBuilder.Services(webApplicationBuilder.Services); // Static Method

			webApplicationBuilder.Services.ApplicationServices(); // Extention Method

			webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));

			webApplicationBuilder.Services.AddIdentity<ApplicationUsers, IdentityRole>(options =>
			{
				options.Password.RequiredUniqueChars = 2;
				options.Password.RequireDigit = true;
				options.Password.RequireNonAlphanumeric = true; // @#$
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = true;
				options.Password.RequiredLength = 6;

				//options.User.AllowedUserNameCharacters = "asdfg12345@"
				options.User.RequireUniqueEmail = true;

				options.Lockout.AllowedForNewUsers = true;
				options.Lockout.MaxFailedAccessAttempts = 3;
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(5);


			})
				.AddEntityFrameworkStores<ApplicationDbContext>();

			webApplicationBuilder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Account/SignIn";
				options.ExpireTimeSpan = TimeSpan.FromDays(1);
				options.AccessDeniedPath = "/Home/Error";

			});

			//webApplicationBuilder.Services.AddAuthentication("Hamada");

			webApplicationBuilder.Services.AddAuthentication(options =>
			{
				//options.DefaultAuthenticateScheme = "Identity.Application";
			})
				.AddCookie("Hamada", options =>
				{
					options.LoginPath = "Account/SignIn";
					options.ExpireTimeSpan = TimeSpan.FromDays(1);
					options.AccessDeniedPath = "/Home/Error";

				});

			#endregion


			var app = webApplicationBuilder.Build();

			#region Configure Kestrel MiddleWare

			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});

			#endregion

			app.Run();
		}


	}
}
