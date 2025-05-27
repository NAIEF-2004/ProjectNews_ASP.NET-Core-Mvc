using Microsoft.AspNetCore.Mvc;
using ProjectNews_ASP.NET_Core_Mvc.Models;
using System.Diagnostics;

namespace ProjectNews_ASP.NET_Core_Mvc.Controllers
{
	public class HomeController : Controller
	{
	
        NewsContext db;
        private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger,NewsContext context)
		{
            db = context;
           _logger = logger;
		}

		public IActionResult Index()
		{
            try
            {
                var result = db.Categorys.ToList();
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ");
                return View("Error");
            }
            //جلب البيانات  من القاعدة كقائمة
           
		}
        public IActionResult News(int id)
        {
            Category c = db.Categorys.Find(id); //جلب التصنيف من القاعدة حسب المعرف
            ViewData["CategoryName"] = c.Name; //إرسال اسم التصنيف إلى العرض

            var result= db.News.Where(x => x.CategoryId == id).OrderByDescending(x=>x.Data).ToList(); //جلب البيانات من القاعدة حسب التصنيف المحدد
            return View(result);
        }

        public IActionResult Massages() //اضفت هذه الدالة لعرض الرسائل
        {

          var massages= db.ContactUs.ToList();
            return View(massages);
       
        }
        public IActionResult DeleteNews(int id)
        {
            var News = db.News.Find(id); //جلب الخبر من القاعدة حسب المعرف
            db.News.Remove(News); //حذف الخبر من القاعدة
            db.SaveChanges(); //حفظ التغييرات في القاعدة
            return RedirectToAction("Index"); //إعادة التوجيه إلى الصفحة الرئيسية
        }


        public IActionResult Contact()
        {
        
            return View();
        }
		[HttpPost]
        public IActionResult SaveContact(ContactUs model)
		{
			db.ContactUs.Add(model); //إضافة البيانات إلى القاعدة
            db.SaveChanges(); //حفظ التغييرات في القاعدة
            return RedirectToAction("Index"); //إعادة التوجيه إلى الصفحة الرئيسية
        }
        public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
