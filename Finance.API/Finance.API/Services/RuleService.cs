using Finance.API.DTOs;
using Finance.API.Interfaces;
using Finance.API.Models;

namespace Finance.API.Services
{
    public class RuleService : IRuleService
    {
        private readonly IRuleRepository _ruleRepository;
        // Сервис правил должен уметь создавать уведомления!
        private readonly INotificationRepository _notificationRepository;

        public RuleService(IRuleRepository ruleRepository, INotificationRepository notificationRepository)
        {
            _ruleRepository = ruleRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task<List<FinancialRule>> GetUserRulesAsync(string userId)
        {
            return await _ruleRepository.GetRulesByUserIdAsync(userId);
        }

        public async Task<FinancialRule> CreateRuleAsync(FinancialRuleDto dto, string userId)
        {
            var rule = new FinancialRule
            {
                UserId = userId,
                RuleName = dto.RuleName,
                TargetCategory = dto.TargetCategory,
                ThresholdAmount = dto.ThresholdAmount,
                ConditionType = dto.ConditionType,
                IsActive = dto.IsActive
            };
            return await _ruleRepository.AddRuleAsync(rule);
        }

        public async Task<bool> DeleteRuleAsync(int id, string userId)
        {
            var rule = await _ruleRepository.GetRuleByIdAsync(id);
            if (rule == null || rule.UserId != userId) return false;

            await _ruleRepository.DeleteRuleAsync(rule);
            return true;
        }

        // --- CORE LOGIC: BANKING RULE ENGINE ---
        public async Task EvaluateTransactionAsync(Transaction transaction)
        {
            var rules = await _ruleRepository.GetRulesByUserIdAsync(transaction.UserId);

            foreach (var rule in rules.Where(r => r.IsActive))
            {
                bool triggered = false;

                // Пример: Превышение лимита одной транзакции
                if (rule.ConditionType == "MaxTransactionAmount")
                {
                    if ((rule.TargetCategory == "Any" || rule.TargetCategory == transaction.Category) &&
                        transaction.Amount > rule.ThresholdAmount)
                    {
                        triggered = true;
                    }
                }

                if (triggered)
                {
                    // Создаем уведомление
                    var notif = new Notification
                    {
                        UserId = transaction.UserId,
                        Type = "Warning",
                        Message = $"Rule Alert: '{rule.RuleName}' triggered by transaction of {transaction.Amount}!",
                        CreatedAt = DateTime.UtcNow,
                        IsRead = false
                    };
                    await _notificationRepository.AddNotificationAsync(notif);
                }
            }
        }
    }
}