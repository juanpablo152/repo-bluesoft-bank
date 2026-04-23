using back_bluesoft_bank.Application.DTOs;

namespace back_bluesoft_bank.Application.Interfaces;

public interface IReportService
{
    Task<List<ClientTransactionReportItem>> GetClientTransactionReportAsync(int year, int month);
    Task<List<OutOfCityWithdrawalReportItem>> GetOutOfCityWithdrawalReportAsync();
}
