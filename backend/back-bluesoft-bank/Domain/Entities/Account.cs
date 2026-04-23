using back_bluesoft_bank.Domain.Enums;
using back_bluesoft_bank.Domain.Exceptions;

namespace back_bluesoft_bank.Domain.Entities;

public class Account
{
    public Guid Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public AccountType AccountType { get; set; }
    public decimal Balance { get; private set; }
    public Guid ClientId { get; set; }
    public string OriginCity { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public uint RowVersion { get; set; }

    public Client Client { get; set; } = null!;
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("El monto de consignación debe ser mayor a cero.");
        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("El monto de retiro debe ser mayor a cero.");
        if (Balance - amount < 0)
            throw new InsufficientFundsException(
                $"Fondos insuficientes. Saldo actual: {Balance:C}, monto solicitado: {amount:C}");
        Balance -= amount;
    }

    public void SetInitialBalance(decimal balance)
    {
        if (balance < 0)
            throw new DomainException("El saldo inicial no puede ser negativo.");
        Balance = balance;
    }
}
