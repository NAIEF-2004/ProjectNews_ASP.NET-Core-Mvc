namespace ProjectNews_ASP.NET_Core_Mvc.Models
{
	public class Category
	{ 
        public int Id { get; set; }
		public string Name { get; set; } //اسم الخبر
		public string Description { get; set; }
		public List<News> Mylist { get; set; }
	}
}
