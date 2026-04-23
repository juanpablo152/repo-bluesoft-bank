using back_bluesoft_bank.Domain.Entities;
using back_bluesoft_bank.Domain.Interfaces;
using back_bluesoft_bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace back_bluesoft_bank.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly BlueSoftBankDbContext _context;

    public AccountRepository(BlueSoftBankDbContext context)
    {
        _context = context;
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        return await _context.Accounts
            .Include(a => a.Client)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Account?> GetByAccountNumberAsync(string accountNumber)
    {
        return await _context.Accounts
            .Include(a => a.Client)
            .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
    }

    public async Task<List<Account>> GetByClientIdAsync(Guid clientId)
    {
        return await _context.Accounts
            .Include(a => a.Client)
            .Where(a => a.ClientId == clientId)
            .ToListAsync();
    }

    public async Task<Account> AddAsync(Account account)
    {
        var entry = await _context.Accounts.AddAsync(account);
        return entry.Entity;
    }

    public Task UpdateAsync(Account account)
    {
        _context.Accounts.Update(account);
        return Task.CompletedTask;
    }
}
