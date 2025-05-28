using Microsoft.EntityFrameworkCore;
using ProjectNews_ASP.NET_Core_Mvc.Models;
using Microsoft.AspNetCore.Identity;
using ProjectNews_ASP.NET_Core_Mvc.Data;

namespace ProjectNews_ASP.NET_Core_Mvc
{
	public class Program
	{
		public static void Main(string[] args)
		{

			var builder = WebApplication.CreateBuilder(args);
   var connectionString = builder.Configuration.GetConnectionString("ProjectNews_ASPNET_Core_MvcContextConnection") ?? throw new InvalidOperationException("Connection string 'ProjectNews_ASPNET_Core_MvcContextConnection' not found.");

			 

			// Add services to the container.

			builder.Services.AddControllersWithViews();
            
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ProjectNews_ASPNET_Core_MvcContext>();

            
            builder.Services.AddDbContext<NewsContext>(options =>
				options.UseSqlServer("Data Source=(localdb)\\ProjectModels;Initial Catalog=ProjectNews_Mvc;Integrated Security=True;"));

			builder.Services.AddControllersWithViews();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
