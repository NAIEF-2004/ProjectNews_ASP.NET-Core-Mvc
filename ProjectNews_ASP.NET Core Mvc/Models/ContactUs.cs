namespace ProjectNews_ASP.NET_Core_Mvc.Models
{
	public class ContactUs
	{
		public int Id { get; set; }
		public string Name { get; set; } //اسم العضو
		public string Email { get; set; } //البريد الالكتروني
		public string Subject { get; set; } //عنوان الرسالة
		public string Message { get; set; } //محتوى الرسالة

	}
}
