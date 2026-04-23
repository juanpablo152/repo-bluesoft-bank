using back_bluesoft_bank.Domain.Enums;

namespace back_bluesoft_bank.Domain.Entities;

public class Client
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public IdentificationType IdentificationType { get; set; }
    public string IdentificationNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Account> Accounts { get; set; } = new List<Account>();

    public string FullName => $"{FirstName} {LastName}";
}
