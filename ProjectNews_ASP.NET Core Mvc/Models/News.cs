using System.ComponentModel;

namespace ProjectNews_ASP.NET_Core_Mvc.Models
{
	public class News//مكونات كلاس الخبر 
	{
		public int Id { get; set; }
		[DisplayName("Title of news")]
		public string Title { get; set; }
		public string? Image { get; set; }
		public string Topic { get; set; }
		public DateTime Data { get; set; }
		public int CategoryId { get; set; }
		public Category Category { get; set; }
	}
}
