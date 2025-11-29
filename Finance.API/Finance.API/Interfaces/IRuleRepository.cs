using Finance.API.Models;

namespace Finance.API.Interfaces
{
    public interface IRuleRepository
    {
        Task<List<FinancialRule>> GetRulesByUserIdAsync(string userId);
        Task<FinancialRule?> GetRuleByIdAsync(int id);
        Task<FinancialRule> AddRuleAsync(FinancialRule rule);
        Task DeleteRuleAsync(FinancialRule rule);
    }
}