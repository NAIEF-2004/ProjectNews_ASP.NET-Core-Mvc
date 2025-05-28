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


			 

			// Add services to the container.

			builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("AlternativeConnection")));

			builder.Services.AddDefaultIdentity<IdentityUser>()
			.AddEntityFrameworkStores<ApplicationDbContext>();

			//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

			//builder.Services.AddControllersWithViews();

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
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
            app.Run();
		}
	}
}
