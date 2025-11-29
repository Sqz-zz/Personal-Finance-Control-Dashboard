using System.ComponentModel.DataAnnotations;

namespace Finance.API.DTOs
{
    public class FinancialRuleDto
    {
        public int Id { get; set; } // Nullable при создании, заполнен при чтении

        [Required]
        public string RuleName { get; set; } = string.Empty;

        public string TargetCategory { get; set; } = "Any";

        [Required]
        [Range(0.01, 1000000.00)]
        public decimal ThresholdAmount { get; set; }

        [Required]
        // Допустимые значения: "MaxTransactionAmount", "MonthlyLimit"
        public string ConditionType { get; set; } = "MaxTransactionAmount";

        public bool IsActive { get; set; } = true;
    }
}