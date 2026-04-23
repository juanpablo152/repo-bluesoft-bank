using back_bluesoft_bank.Domain.Enums;

namespace back_bluesoft_bank.Application.DTOs;

public record CreateAccountRequest(
    Guid ClientId,
    AccountType AccountType,
    decimal InitialBalance = 0
);

public record AccountResponse(
    Guid Id,
    string AccountNumber,
    AccountType AccountType,
    decimal Balance,
    Guid ClientId,
    string ClientName,
    string OriginCity,
    bool IsActive,
    DateTime CreatedAt
);

public record BalanceResponse(
    string AccountNumber,
    string ClientName,
    decimal Balance,
    DateTime ConsultedAt
);

public record MonthlyStatementResponse(
    string AccountNumber,
    string ClientName,
    AccountType AccountType,
    int Year,
    int Month,
    decimal InitialBalance,
    decimal TotalDeposits,
    decimal TotalWithdrawals,
    decimal FinalBalance,
    List<TransactionResponse> Transactions
);
