using Finance.API.DTOs;
using Finance.API.Interfaces;
using Finance.API.Models;

namespace Finance.API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repository;
        // Внедряем RuleService, чтобы запускать проверки при создании
        private readonly IRuleService _ruleService;

        public TransactionService(ITransactionRepository repository, IRuleService ruleService)
        {
            _repository = repository;
            _ruleService = ruleService;
        }

        public async Task<List<Transaction>> GetUserTransactionsAsync(string userId)
        {
            return await _repository.GetUserTransactionsAsync(userId);
        }

        public async Task<Transaction?> GetUserTransactionByIdAsync(int id, string userId)
        {
            var transaction = await _repository.GetTransactionByIdAsync(id);
            return (transaction != null && transaction.UserId == userId) ? transaction : null;
        }

        public async Task<Transaction> CreateTransactionAsync(TransactionDto dto, string userId)
        {
            var transaction = new Transaction
            {
                UserId = userId,
                Amount = dto.Amount,
                Type = dto.Type,
                Description = dto.Description,
                Category = dto.Category,
                Date = dto.Date ?? DateTime.UtcNow
            };

            var createdTransaction = await _repository.CreateTransactionAsync(transaction);

            // --- ЗАПУСК RULE ENGINE ---
            // Проверяем правила после создания транзакции
            await _ruleService.EvaluateTransactionAsync(createdTransaction);

            return createdTransaction;
        }

        public async Task<bool> UpdateTransactionAsync(int id, TransactionDto dto, string userId)
        {
            var transaction = await _repository.GetTransactionByIdAsync(id);
            if (transaction == null || transaction.UserId != userId) return false;

            transaction.Amount = dto.Amount;
            transaction.Type = dto.Type;
            transaction.Description = dto.Description;
            transaction.Category = dto.Category;
            transaction.Date = dto.Date ?? transaction.Date;

            await _repository.UpdateTransactionAsync(transaction);
            return true;
        }

        public async Task<bool> DeleteTransactionAsync(int id, string userId)
        {
            var transaction = await _repository.GetTransactionByIdAsync(id);
            if (transaction == null || transaction.UserId != userId) return false;

            await _repository.DeleteTransactionAsync(transaction);
            return true;
        }
    }
}