using ProjectNews_ASP.NET_Core_Mvc.Models;

public class HomeViewModel
{
    public List<News> News { get; set; }
    public string SearchTerm { get; set; }
    public int? CategoryId { get; set; }
    public List<Category> Categories { get; set; }
}