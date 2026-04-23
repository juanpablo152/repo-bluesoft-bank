using back_bluesoft_bank.Application.DTOs;
using back_bluesoft_bank.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace back_bluesoft_bank.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost("deposit")]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deposit([FromBody] DepositRequest request)
    {
        var transaction = await _transactionService.DepositAsync(request);
        return Created(string.Empty, transaction);
    }

    [HttpPost("withdrawal")]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Withdraw([FromBody] WithdrawalRequest request)
    {
        var transaction = await _transactionService.WithdrawAsync(request);
        return Created(string.Empty, transaction);
    }
}
