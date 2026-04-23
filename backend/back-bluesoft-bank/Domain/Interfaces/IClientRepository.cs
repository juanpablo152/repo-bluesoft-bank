using back_bluesoft_bank.Domain.Entities;

namespace back_bluesoft_bank.Domain.Interfaces;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(Guid id);
    Task<Client?> GetByIdentificationAsync(string identificationNumber);
    Task<List<Client>> GetAllAsync();
    Task<Client> AddAsync(Client client);
    Task UpdateAsync(Client client);
}
