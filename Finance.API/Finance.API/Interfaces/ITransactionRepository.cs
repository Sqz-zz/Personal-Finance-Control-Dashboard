using Finance.API.Models;

namespace Finance.API.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetUserTransactionsAsync(string userId);
        Task<Transaction?> GetTransactionByIdAsync(int id);
        Task<Transaction> CreateTransactionAsync(Transaction transaction);
        Task UpdateTransactionAsync(Transaction transaction);
        Task DeleteTransactionAsync(Transaction transaction);
    }
}