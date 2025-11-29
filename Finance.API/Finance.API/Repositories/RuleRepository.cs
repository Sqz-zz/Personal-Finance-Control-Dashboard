using Finance.API.Data;
using Finance.API.Interfaces;
using Finance.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance.API.Repositories
{
    public class RuleRepository : IRuleRepository
    {
        private readonly ApplicationDbContext _context;

        public RuleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FinancialRule>> GetRulesByUserIdAsync(string userId)
        {
            return await _context.FinancialRules
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<FinancialRule?> GetRuleByIdAsync(int id)
        {
            return await _context.FinancialRules.FindAsync(id);
        }

        public async Task<FinancialRule> AddRuleAsync(FinancialRule rule)
        {
            _context.FinancialRules.Add(rule);
            await _context.SaveChangesAsync();
            return rule;
        }

        public async Task DeleteRuleAsync(FinancialRule rule)
        {
            _context.FinancialRules.Remove(rule);
            await _context.SaveChangesAsync();
        }
    }
}