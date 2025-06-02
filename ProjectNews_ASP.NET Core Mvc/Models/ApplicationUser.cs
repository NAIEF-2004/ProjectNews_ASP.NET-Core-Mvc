using Microsoft.AspNetCore.Identity;

namespace ProjectNews_ASP.NET_Core_Mvc.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ProfilePicture { get; set; }
        public string? FullName { get; set; }
        public string? Bio { get; set; }
    }
}