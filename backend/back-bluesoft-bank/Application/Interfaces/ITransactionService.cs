using back_bluesoft_bank.Application.DTOs;

namespace back_bluesoft_bank.Application.Interfaces;

public interface ITransactionService
{
    Task<TransactionResponse> DepositAsync(DepositRequest request);
    Task<TransactionResponse> WithdrawAsync(WithdrawalRequest request);
}
