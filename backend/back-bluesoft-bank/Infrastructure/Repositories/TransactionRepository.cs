using back_bluesoft_bank.Domain.Entities;
using back_bluesoft_bank.Domain.Interfaces;
using back_bluesoft_bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace back_bluesoft_bank.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly BlueSoftBankDbContext _context;

    public TransactionRepository(BlueSoftBankDbContext context)
    {
        _context = context;
    }

    public async Task<Transaction> AddAsync(Transaction transaction)
    {
        var entry = await _context.Transactions.AddAsync(transaction);
        return entry.Entity;
    }

    public async Task<List<Transaction>> GetByAccountIdAsync(Guid accountId, int limit = 20)
    {
        return await _context.Transactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.CreatedAt)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<Transaction>> GetMonthlyByAccountIdAsync(Guid accountId, int year, int month)
    {
        return await _context.Transactions
            .Where(t => t.AccountId == accountId
                && t.CreatedAt.Year == year
                && t.CreatedAt.Month == month)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Transaction>> GetAllForMonthAsync(int year, int month)
    {
        return await _context.Transactions
            .Include(t => t.Account)
                .ThenInclude(a => a.Client)
            .Where(t => t.CreatedAt.Year == year && t.CreatedAt.Month == month)
            .ToListAsync();
    }
}
