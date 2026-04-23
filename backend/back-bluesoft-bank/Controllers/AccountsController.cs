using back_bluesoft_bank.Application.DTOs;
using back_bluesoft_bank.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_bluesoft_bank.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAccountRequest request)
    {
        var account = await _accountService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = account.Id }, account);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var account = await _accountService.GetByIdAsync(id);
        return Ok(account);
    }

    [HttpGet("{id:guid}/balance")]
    [ProducesResponseType(typeof(BalanceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBalance(Guid id)
    {
        var balance = await _accountService.GetBalanceAsync(id);
        return Ok(balance);
    }

    [HttpGet("{id:guid}/transactions")]
    [ProducesResponseType(typeof(List<TransactionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRecentTransactions(Guid id, [FromQuery] int limit = 20)
    {
        var transactions = await _accountService.GetRecentTransactionsAsync(id, limit);
        return Ok(transactions);
    }

    [HttpGet("{id:guid}/statement")]
    [ProducesResponseType(typeof(MonthlyStatementResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMonthlyStatement(Guid id, [FromQuery] int year, [FromQuery] int month)
    {
        var statement = await _accountService.GetMonthlyStatementAsync(id, year, month);
        return Ok(statement);
    }

    [HttpGet("by-client/{clientId:guid}")]
    [ProducesResponseType(typeof(List<AccountResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByClientId(Guid clientId)
    {
        var accounts = await _accountService.GetByClientIdAsync(clientId);
        return Ok(accounts);
    }
}
