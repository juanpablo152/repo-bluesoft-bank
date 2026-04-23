using back_bluesoft_bank.Domain.Enums;

namespace back_bluesoft_bank.Application.DTOs;

public record DepositRequest(
    Guid AccountId,
    decimal Amount,
    string Description,
    string City
);

public record WithdrawalRequest(
    Guid AccountId,
    decimal Amount,
    string Description,
    string City
);

public record TransactionResponse(
    Guid Id,
    Guid AccountId,
    TransactionType TransactionType,
    decimal Amount,
    decimal BalanceAfter,
    string Description,
    string City,
    DateTime CreatedAt
);
