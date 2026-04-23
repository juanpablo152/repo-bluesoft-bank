using back_bluesoft_bank.Application.DTOs;

namespace back_bluesoft_bank.Application.Interfaces;

public interface IAccountService
{
    Task<AccountResponse> CreateAsync(CreateAccountRequest request);
    Task<AccountResponse> GetByIdAsync(Guid id);
    Task<BalanceResponse> GetBalanceAsync(Guid accountId);
    Task<List<TransactionResponse>> GetRecentTransactionsAsync(Guid accountId, int limit = 20);
    Task<MonthlyStatementResponse> GetMonthlyStatementAsync(Guid accountId, int year, int month);
    Task<List<AccountResponse>> GetByClientIdAsync(Guid clientId);
}
