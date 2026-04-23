namespace back_bluesoft_bank.Application.DTOs;

public record ClientTransactionReportItem(
    Guid ClientId,
    string ClientName,
    string IdentificationNumber,
    int TransactionCount
);

public record OutOfCityWithdrawalReportItem(
    Guid ClientId,
    string ClientName,
    string IdentificationNumber,
    string AccountOriginCity,
    decimal TotalWithdrawals,
    int WithdrawalCount
);
