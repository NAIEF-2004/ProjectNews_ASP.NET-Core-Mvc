using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ProjectNews_ASP.NET_Core_Mvc.Models
{
    public class News
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "عنوان الخبر مطلوب")]
        [DisplayName("عنوان الخبر")]
        public string Title { get; set; }

        [DisplayName("صورة الخبر")]
        public string? Image { get; set; }

        [NotMapped] // هذا يعني أن هذا الحقل لن يتم تخزينه في قاعدة البيانات
        [DisplayName("ملف الصورة")]
        public IFormFile? ImageFile { get; set; } // إضافة خاصية جديدة للتعامل مع ملف الصورة

        [Required(ErrorMessage = "موضوع الخبر مطلوب")]
        [DisplayName("موضوع الخبر")]
        public string Topic { get; set; }

        [Required(ErrorMessage = "تاريخ الخبر مطلوب")]
        [DisplayName("تاريخ الخبر")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "التصنيف مطلوب")]
        [DisplayName("التصنيف")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}