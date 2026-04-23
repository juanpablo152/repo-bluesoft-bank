using back_bluesoft_bank.Domain.Enums;

namespace back_bluesoft_bank.Application.DTOs;

public record CreateClientRequest(
    string FirstName,
    string LastName,
    IdentificationType IdentificationType,
    string IdentificationNumber,
    string Email,
    string Phone,
    string City
);

public record ClientResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string FullName,
    IdentificationType IdentificationType,
    string IdentificationNumber,
    string Email,
    string Phone,
    string City,
    DateTime CreatedAt
);
