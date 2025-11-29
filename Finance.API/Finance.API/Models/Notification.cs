using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finance.API.Models
{
    // Сущность уведомления (результат срабатывания правил или AI)
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;

        // Тип: "Warning", "Info", "Advice"
        [StringLength(20)]
        public string Type { get; set; } = "Info";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;
    }
}