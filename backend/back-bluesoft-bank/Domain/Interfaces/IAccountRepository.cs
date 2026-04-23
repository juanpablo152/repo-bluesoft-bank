using back_bluesoft_bank.Domain.Entities;

namespace back_bluesoft_bank.Domain.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id);
    Task<Account?> GetByAccountNumberAsync(string accountNumber);
    Task<List<Account>> GetByClientIdAsync(Guid clientId);
    Task<Account> AddAsync(Account account);
    Task UpdateAsync(Account account);
}
