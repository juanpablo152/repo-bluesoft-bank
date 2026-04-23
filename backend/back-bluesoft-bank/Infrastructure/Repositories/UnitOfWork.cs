using System.Data;
using back_bluesoft_bank.Domain.Interfaces;
using back_bluesoft_bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace back_bluesoft_bank.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly BlueSoftBankDbContext _context;
    private IDbContextTransaction? _transaction;

    public IClientRepository Clients { get; }
    public IAccountRepository Accounts { get; }
    public ITransactionRepository Transactions { get; }

    public UnitOfWork(BlueSoftBankDbContext context)
    {
        _context = context;
        Clients = new ClientRepository(context);
        Accounts = new AccountRepository(context);
        Transactions = new TransactionRepository(context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync(
            System.Data.IsolationLevel.RepeatableRead);
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
