using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finance.API.Models
{
    // Сущность правила для банковского Rule Engine
    public class FinancialRule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        // Название правила (например: "Лимит на фастфуд")
        [Required]
        [StringLength(100)]
        public string RuleName { get; set; } = string.Empty;

        // Категория, к которой применяется правило (или "Any" для всех)
        [StringLength(50)]
        public string TargetCategory { get; set; } = "Any";

        // Пороговая сумма (например: 500.00)
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ThresholdAmount { get; set; }

        // Тип условия: "MaxTransactionAmount", "MonthlyLimit", "BalanceLow"
        [Required]
        public string ConditionType { get; set; } = "MaxTransactionAmount";

        // Активно ли правило
        public bool IsActive { get; set; } = true;
    }
}