using back_bluesoft_bank.Domain.Entities;
using back_bluesoft_bank.Domain.Interfaces;
using back_bluesoft_bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace back_bluesoft_bank.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly BlueSoftBankDbContext _context;

    public ClientRepository(BlueSoftBankDbContext context)
    {
        _context = context;
    }

    public async Task<Client?> GetByIdAsync(Guid id)
    {
        return await _context.Clients
            .Include(c => c.Accounts)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Client?> GetByIdentificationAsync(string identificationNumber)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(c => c.IdentificationNumber == identificationNumber);
    }

    public async Task<List<Client>> GetAllAsync()
    {
        return await _context.Clients
            .Include(c => c.Accounts)
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .ToListAsync();
    }

    public async Task<Client> AddAsync(Client client)
    {
        var entry = await _context.Clients.AddAsync(client);
        return entry.Entity;
    }

    public Task UpdateAsync(Client client)
    {
        _context.Clients.Update(client);
        return Task.CompletedTask;
    }
}
