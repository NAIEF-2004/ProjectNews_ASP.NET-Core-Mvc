using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectNews_ASP.NET_Core_Mvc.Models;
using System.Diagnostics;
namespace ProjectNews_ASP.NET_Core_Mvc.Controllers
{
	public class HomeController : Controller
	{
	
        private readonly NewsContext _context;
        private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger,NewsContext context)
		{
            _context = context;
           _logger = logger;
		}

        public IActionResult Index(string searchTerm, int? categoryId)
        {
            // تحميل التصنيفات للقائمة المنسدلة وللعرض
            var categories = _context.Categorys.ToList();
            ViewBag.Categories = categories;

            // تحضير نموذج العرض
            var viewModel = new HomeViewModel
            {
                Categories = categories,
                News = new List<News>() // لا نحتاج لعرض الأخبار في الصفحة الرئيسية
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Search(string searchTerm, int? categoryId)
        {
            var query = _context.News.Include(n => n.Category).AsQueryable();

            // تطبيق الفلترة حسب النص المدخل
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(n => n.Title.Contains(searchTerm) ||
                                       n.Topic.Contains(searchTerm));
            }

            // فلترة حسب التصنيف
            if (categoryId.HasValue)
            {
                query = query.Where(n => n.CategoryId == categoryId.Value);
            }

            var news = query.OrderByDescending(n => n.Data)
                           .Select(n => new
                           {
                               id = n.Id,
                               title = n.Title,
                               topic = n.Topic,
                               image = n.Image,
                               data = n.Data,
                               category = new { id = n.Category.Id, name = n.Category.Name }
                           })
                           .ToList();

            return Json(new { success = true, news = news });
        }

        public IActionResult News(int? id, string searchTerm)
        {
            // تحميل التصنيفات للقائمة المنسدلة
            ViewBag.Categories = _context.Categorys.ToList();

            // تحضير استعلام الأخبار
            var query = _context.News.Include(n => n.Category).AsQueryable();

            // فلترة حسب التصنيف
            if (id.HasValue)
            {
                query = query.Where(x => x.CategoryId == id.Value);
            }

            // فلترة حسب نص البحث
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(n => n.Title.Contains(searchTerm) ||
                                       n.Topic.Contains(searchTerm));
            }

            // تنفيذ الاستعلام
            var result = query.OrderByDescending(x => x.Data).ToList();

            // إضافة رسائل للعرض
            if (!string.IsNullOrEmpty(searchTerm))
            {
                ViewBag.SearchTerm = searchTerm;
            }

            if (id.HasValue)
            {
                var category = _context.Categorys.Find(id.Value);
                if (category != null)
                {
                    ViewBag.CategoryName = category.Name;
                }
            }

            // إضافة رسالة إذا لم يتم العثور على نتائج
            if (!result.Any())
            {
                ViewBag.NoResults = true;
                ViewBag.Message = "لا توجد نتائج تطابق بحثك";
            }

            return View(result);
        }

        public IActionResult Massages() //اضفت هذه الدالة لعرض الرسائل
        {
            var massages = _context.ContactUs.ToList();
            return View(massages);
        } 
        public IActionResult About() //اضفت هذه الدالة لعرض الرسائل
        {
            return View();
        }
        public IActionResult DeleteNews(int id)
        {
            var News = _context.News.Find(id); //جلب الخبر من القاعدة حسب المعرف
            _context.News.Remove(News); //حذف الخبر من القاعدة
            _context.SaveChanges(); //حفظ التغييرات في القاعدة
            return RedirectToAction("Index"); //إعادة التوجيه إلى الصفحة الرئيسية
        }


        public IActionResult Contact()
        {
        
            return View();
        }
		[HttpPost]
        public IActionResult SaveContact(ContactUs model)
		{
			_context.ContactUs.Add(model); //إضافة البيانات إلى القاعدة
            _context.SaveChanges(); //حفظ التغييرات في القاعدة
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
