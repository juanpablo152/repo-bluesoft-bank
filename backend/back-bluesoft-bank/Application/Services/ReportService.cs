using back_bluesoft_bank.Application.DTOs;
using back_bluesoft_bank.Application.Interfaces;
using back_bluesoft_bank.Domain.Enums;
using back_bluesoft_bank.Domain.Interfaces;

namespace back_bluesoft_bank.Application.Services;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ClientTransactionReportItem>> GetClientTransactionReportAsync(int year, int month)
    {
        var transactions = await _unitOfWork.Transactions.GetAllForMonthAsync(year, month);

        var report = transactions
            .GroupBy(t => t.Account.Client)
            .Select(g => new ClientTransactionReportItem(
                g.Key.Id,
                g.Key.FullName,
                g.Key.IdentificationNumber,
                g.Count()
            ))
            .OrderByDescending(r => r.TransactionCount)
            .ToList();

        return report;
    }

    public async Task<List<OutOfCityWithdrawalReportItem>> GetOutOfCityWithdrawalReportAsync()
    {
        var clients = await _unitOfWork.Clients.GetAllAsync();
        var result = new List<OutOfCityWithdrawalReportItem>();

        foreach (var client in clients)
        {
            var accounts = await _unitOfWork.Accounts.GetByClientIdAsync(client.Id);

            foreach (var account in accounts)
            {
                var allTransactions = await _unitOfWork.Transactions.GetByAccountIdAsync(account.Id, int.MaxValue);

                var outOfCityWithdrawals = allTransactions
                    .Where(t => t.TransactionType == TransactionType.Retiro
                        && !string.Equals(t.City, account.OriginCity, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                var totalWithdrawals = outOfCityWithdrawals.Sum(t => t.Amount);

                if (totalWithdrawals > 1_000_000)
                {
                    result.Add(new OutOfCityWithdrawalReportItem(
                        client.Id,
                        client.FullName,
                        client.IdentificationNumber,
                        account.OriginCity,
                        totalWithdrawals,
                        outOfCityWithdrawals.Count
                    ));
                }
            }
        }

        return result.OrderByDescending(r => r.TotalWithdrawals).ToList();
    }
}
