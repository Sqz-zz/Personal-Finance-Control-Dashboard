using Finance.API.DTOs;
using Finance.API.Models;

namespace Finance.API.Interfaces
{
    public interface IRuleService
    {
        Task<List<FinancialRule>> GetUserRulesAsync(string userId);
        Task<FinancialRule> CreateRuleAsync(FinancialRuleDto dto, string userId);
        Task<bool> DeleteRuleAsync(int id, string userId);

        // Метод для проверки транзакции движком правил
        Task EvaluateTransactionAsync(Transaction transaction);
    }
}