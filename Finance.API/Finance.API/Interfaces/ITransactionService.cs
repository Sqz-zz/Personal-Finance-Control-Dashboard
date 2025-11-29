using Finance.API.DTOs;
using Finance.API.Models;

namespace Finance.API.Interfaces
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetUserTransactionsAsync(string userId);
        Task<Transaction?> GetUserTransactionByIdAsync(int id, string userId);
        Task<Transaction> CreateTransactionAsync(TransactionDto dto, string userId);
        Task<bool> UpdateTransactionAsync(int id, TransactionDto dto, string userId);
        Task<bool> DeleteTransactionAsync(int id, string userId);
    }
}