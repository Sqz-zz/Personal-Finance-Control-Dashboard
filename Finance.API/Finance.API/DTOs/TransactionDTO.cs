using System.ComponentModel.DataAnnotations;

namespace Finance.API.DTOs
{
    // DTO для создания и обновления транзакций
    public class TransactionDto
    {
        // Сумма транзакции
        [Required]
        [Range(0.01, 10000000.00)] // Ограничение суммы
        public decimal Amount { get; set; }

        // Тип: "Доход" или "Расход"
        [Required]
        [StringLength(10)]
        public string Type { get; set; } = string.Empty;

        // Описание
        [StringLength(250)]
        public string Description { get; set; } = string.Empty;

        // Категория
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        // Дата (не обязательна, если не передана, будет использоваться DateTime.Now)
        public DateTime? Date { get; set; }
    }
}