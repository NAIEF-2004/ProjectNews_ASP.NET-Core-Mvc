using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ProjectNews_ASP.NET_Core_Mvc.Models
{
	public class NewsContext : DbContext
	{
		public NewsContext(DbContextOptions<NewsContext> options) : base(options) { }
		public DbSet<Category> Categorys { get; set; }
		public DbSet<News> News { get; set; }
		public DbSet<Teammembers> Teammembers { get; set; }
		public DbSet<ContactUs> ContactUs { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectModels;Initial Catalog=ProjectNews_Mvc;Integrated Security=True;");

		}


	}
}