using back_bluesoft_bank.Application.DTOs;
using back_bluesoft_bank.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_bluesoft_bank.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("client-transactions")]
    [ProducesResponseType(typeof(List<ClientTransactionReportItem>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClientTransactionReport(
        [FromQuery] int year, [FromQuery] int month)
    {
        var report = await _reportService.GetClientTransactionReportAsync(year, month);
        return Ok(report);
    }

    [HttpGet("out-of-city-withdrawals")]
    [ProducesResponseType(typeof(List<OutOfCityWithdrawalReportItem>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOutOfCityWithdrawalReport()
    {
        var report = await _reportService.GetOutOfCityWithdrawalReportAsync();
        return Ok(report);
    }
}
