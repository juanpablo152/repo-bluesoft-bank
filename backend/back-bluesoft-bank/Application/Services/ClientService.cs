using back_bluesoft_bank.Application.DTOs;
using back_bluesoft_bank.Application.Interfaces;
using back_bluesoft_bank.Domain.Entities;
using back_bluesoft_bank.Domain.Exceptions;
using back_bluesoft_bank.Domain.Interfaces;

namespace back_bluesoft_bank.Application.Services;

public class ClientService : IClientService
{
    private readonly IUnitOfWork _unitOfWork;

    public ClientService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ClientResponse> CreateAsync(CreateClientRequest request)
    {
        var existing = await _unitOfWork.Clients.GetByIdentificationAsync(request.IdentificationNumber);
        if (existing != null)
            throw new DomainException($"Ya existe un cliente con el número de identificación {request.IdentificationNumber}.");

        var client = new Client
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            IdentificationType = request.IdentificationType,
            IdentificationNumber = request.IdentificationNumber,
            Email = request.Email,
            Phone = request.Phone,
            City = request.City
        };

        await _unitOfWork.Clients.AddAsync(client);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(client);
    }

    public async Task<ClientResponse> GetByIdAsync(Guid id)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(id)
            ?? throw new NotFoundException($"No se encontró el cliente con Id {id}.");
        return MapToResponse(client);
    }

    public async Task<List<ClientResponse>> GetAllAsync()
    {
        var clients = await _unitOfWork.Clients.GetAllAsync();
        return clients.Select(MapToResponse).ToList();
    }

    private static ClientResponse MapToResponse(Client client) => new(
        client.Id,
        client.FirstName,
        client.LastName,
        client.FullName,
        client.IdentificationType,
        client.IdentificationNumber,
        client.Email,
        client.Phone,
        client.City,
        client.CreatedAt
    );
}
