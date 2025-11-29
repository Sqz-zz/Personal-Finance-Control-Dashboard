using Microsoft.AspNetCore.Identity;

namespace Finance.API.Models
{
    // Класс, представляющий пользователя в нашей системе (расширяем IdentityUser)
    public class ApplicationUser : IdentityUser
    {
        // Здесь можно добавить любые нужные поля, например:
        public string? FullName { get; set; }
        public DateTime DateJoined { get; set; }
    }
}