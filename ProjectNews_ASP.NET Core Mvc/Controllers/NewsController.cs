using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectNews_ASP.NET_Core_Mvc.Models;

namespace ProjectNews_ASP.NET_Core_Mvc.Controllers
{
    public class NewsController : Controller
    {
        private readonly NewsContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NewsController(NewsContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: News
        public async Task<IActionResult> Index()
        {
            var newsContext = await _context.News
                .Include(n => n.Category)
                .OrderByDescending(n => n.Data)
                .ToListAsync();
            return View(newsContext);
        }

        // GET: News/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "Id", "Name");
            return View();
        }

        // POST: News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,ImageFile,Topic,Data,CategoryId")] News news)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // معالجة الصورة إذا تم رفعها
                    if (news.ImageFile != null)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "news");

                        // إنشاء مجلد الصور إذا لم يكن موجوداً
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // إنشاء اسم فريد للصورة
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + news.ImageFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // حفظ الصورة
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await news.ImageFile.CopyToAsync(fileStream);
                        }

                        // حفظ مسار الصورة في قاعدة البيانات
                        news.Image = "/images/news/" + uniqueFileName;
                    }

                    // إضافة الخبر إلى قاعدة البيانات
                    _context.Add(news);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "تم إضافة الخبر بنجاح";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "حدث خطأ أثناء حفظ الخبر: " + ex.Message);
                }
            }

            // في حالة وجود أخطاء، إعادة تحميل القائمة المنسدلة للتصنيفات
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "Id", "Name", news.CategoryId);
            return View(news);
        }
        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "Id", "Name", news.CategoryId);
            return View(news);
        }

        // POST: News/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Topic,Data,CategoryId")] News news, IFormFile? newImage)
        {
            if (id != news.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // الحصول على الخبر القديم من قاعدة البيانات
                    var existingNews = await _context.News.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
                    if (existingNews == null)
                    {
                        return NotFound();
                    }

                    // الاحتفاظ بالصورة القديمة إذا لم يتم تحميل صورة جديدة
                    news.Image = existingNews.Image;

                    // معالجة الصورة الجديدة إذا تم رفعها
                    if (newImage != null)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "news");

                        // إنشاء مجلد الصور إذا لم يكن موجوداً
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // حذف الصورة القديمة إذا كانت موجودة
                        if (!string.IsNullOrEmpty(existingNews.Image))
                        {
                            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingNews.Image.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // إنشاء اسم فريد للصورة الجديدة
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + newImage.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // حفظ الصورة الجديدة
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await newImage.CopyToAsync(fileStream);
                        }

                        // تحديث مسار الصورة في قاعدة البيانات
                        news.Image = "/images/news/" + uniqueFileName;
                    }

                    _context.Update(news);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "تم تعديل الخبر بنجاح";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["CategoryId"] = new SelectList(_context.Categorys, "Id", "Name", news.CategoryId);
            return View(news);
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.Id == id);
        }
        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                // حذف الصورة من المجلد إذا كانت موجودة
                if (!string.IsNullOrEmpty(news.Image))
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, news.Image.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.News.Remove(news);
                await _context.SaveChangesAsync();
                TempData["Success"] = "تم حذف الخبر بنجاح";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}