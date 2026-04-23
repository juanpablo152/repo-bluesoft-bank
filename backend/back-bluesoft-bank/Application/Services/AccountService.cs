using back_bluesoft_bank.Application.DTOs;
using back_bluesoft_bank.Application.Interfaces;
using back_bluesoft_bank.Domain.Entities;
using back_bluesoft_bank.Domain.Enums;
using back_bluesoft_bank.Domain.Exceptions;
using back_bluesoft_bank.Domain.Interfaces;

namespace back_bluesoft_bank.Application.Services;

public class AccountService : IAccountService
{
    private readonly IUnitOfWork _unitOfWork;

    public AccountService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AccountResponse> CreateAsync(CreateAccountRequest request)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(request.ClientId)
            ?? throw new NotFoundException($"No se encontró el cliente con Id {request.ClientId}.");

        ValidateAccountTypeForClient(request.AccountType, client);

        var account = new Account
        {
            Id = Guid.NewGuid(),
            AccountNumber = GenerateAccountNumber(request.AccountType),
            AccountType = request.AccountType,
            ClientId = client.Id,
            OriginCity = client.City,
            IsActive = true
        };

        account.SetInitialBalance(request.InitialBalance);

        await _unitOfWork.Accounts.AddAsync(account);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(account, client.FullName);
    }

    public async Task<AccountResponse> GetByIdAsync(Guid id)
    {
        var account = await _unitOfWork.Accounts.GetByIdAsync(id)
            ?? throw new NotFoundException($"No se encontró la cuenta con Id {id}.");
        return MapToResponse(account, account.Client.FullName);
    }

    public async Task<BalanceResponse> GetBalanceAsync(Guid accountId)
    {
        var account = await _unitOfWork.Accounts.GetByIdAsync(accountId)
            ?? throw new NotFoundException($"No se encontró la cuenta con Id {accountId}.");

        return new BalanceResponse(
            account.AccountNumber,
            account.Client.FullName,
            account.Balance,
            DateTime.UtcNow
        );
    }

    public async Task<List<TransactionResponse>> GetRecentTransactionsAsync(Guid accountId, int limit = 20)
    {
        _ = await _unitOfWork.Accounts.GetByIdAsync(accountId)
            ?? throw new NotFoundException($"No se encontró la cuenta con Id {accountId}.");

        var transactions = await _unitOfWork.Transactions.GetByAccountIdAsync(accountId, limit);
        return transactions.Select(MapTransactionToResponse).ToList();
    }

    public async Task<MonthlyStatementResponse> GetMonthlyStatementAsync(Guid accountId, int year, int month)
    {
        var account = await _unitOfWork.Accounts.GetByIdAsync(accountId)
            ?? throw new NotFoundException($"No se encontró la cuenta con Id {accountId}.");

        var transactions = await _unitOfWork.Transactions.GetMonthlyByAccountIdAsync(accountId, year, month);

        var totalDeposits = transactions
            .Where(t => t.TransactionType == TransactionType.Consignacion)
            .Sum(t => t.Amount);

        var totalWithdrawals = transactions
            .Where(t => t.TransactionType == TransactionType.Retiro)
            .Sum(t => t.Amount);

        var finalBalance = transactions.Count > 0
            ? transactions.Last().BalanceAfter
            : account.Balance;

        var initialBalance = finalBalance - totalDeposits + totalWithdrawals;

        return new MonthlyStatementResponse(
            account.AccountNumber,
            account.Client.FullName,
            account.AccountType,
            year,
            month,
            initialBalance,
            totalDeposits,
            totalWithdrawals,
            finalBalance,
            transactions.Select(MapTransactionToResponse).ToList()
        );
    }

    public async Task<List<AccountResponse>> GetByClientIdAsync(Guid clientId)
    {
        _ = await _unitOfWork.Clients.GetByIdAsync(clientId)
            ?? throw new NotFoundException($"No se encontró el cliente con Id {clientId}.");

        var accounts = await _unitOfWork.Accounts.GetByClientIdAsync(clientId);
        return accounts.Select(a => MapToResponse(a, a.Client.FullName)).ToList();
    }

    private static void ValidateAccountTypeForClient(AccountType accountType, Client client)
    {
        if (accountType == AccountType.Ahorros && client.IdentificationType != IdentificationType.CC)
            throw new DomainException("Las cuentas de ahorros solo pueden ser creadas para personas naturales (CC).");

        if (accountType == AccountType.Corriente && client.IdentificationType != IdentificationType.NIT)
            throw new DomainException("Las cuentas corrientes solo pueden ser creadas para empresas (NIT).");
    }

    private static string GenerateAccountNumber(AccountType type)
    {
        var prefix = type == AccountType.Ahorros ? "AHO" : "COR";
        return $"{prefix}-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }

    private static AccountResponse MapToResponse(Account account, string clientName) => new(
        account.Id,
        account.AccountNumber,
        account.AccountType,
        account.Balance,
        account.ClientId,
        clientName,
        account.OriginCity,
        account.IsActive,
        account.CreatedAt
    );

    private static TransactionResponse MapTransactionToResponse(Transaction t) => new(
        t.Id,
        t.AccountId,
        t.TransactionType,
        t.Amount,
        t.BalanceAfter,
        t.Description,
        t.City,
        t.CreatedAt
    );
}
