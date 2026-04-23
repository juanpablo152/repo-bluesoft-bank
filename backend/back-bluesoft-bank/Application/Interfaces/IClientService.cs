using back_bluesoft_bank.Application.DTOs;

namespace back_bluesoft_bank.Application.Interfaces;

public interface IClientService
{
    Task<ClientResponse> CreateAsync(CreateClientRequest request);
    Task<ClientResponse> GetByIdAsync(Guid id);
    Task<List<ClientResponse>> GetAllAsync();
}
