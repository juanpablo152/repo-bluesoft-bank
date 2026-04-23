using back_bluesoft_bank.Domain.Entities;

namespace back_bluesoft_bank.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<Transaction> AddAsync(Transaction transaction);
    Task<List<Transaction>> GetByAccountIdAsync(Guid accountId, int limit = 20);
    Task<List<Transaction>> GetMonthlyByAccountIdAsync(Guid accountId, int year, int month);
    Task<List<Transaction>> GetAllForMonthAsync(int year, int month);
}
