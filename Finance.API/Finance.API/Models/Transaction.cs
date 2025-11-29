using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finance.API.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        // Сумма транзакции
        [Required]
        [Column(TypeName = "decimal(18, 2)")] // Для точного хранения денежных значений
        public decimal Amount { get; set; }

        // Тип транзакции: "Доход" или "Расход"
        [Required]
        [StringLength(10)]
        public string Type { get; set; } = string.Empty;

        // Описание или заметки
        [StringLength(250)]
        public string Description { get; set; } = string.Empty;

        // Дата, когда транзакция произошла
        public DateTime Date { get; set; } = DateTime.Now;

        // Категория (например, "Зарплата", "Еда", "Аренда")
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        // -------------------------------------------------------------------
        // СВЯЗЬ С ПОЛЬЗОВАТЕЛЕМ (ОБЯЗАТЕЛЬНОЕ ПОЛЕ)
        // -------------------------------------------------------------------
        // Внешний ключ, связывающий транзакцию с ApplicationUser, который ее создал
        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}