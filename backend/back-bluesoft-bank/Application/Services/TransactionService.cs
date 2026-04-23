using back_bluesoft_bank.Application.DTOs;
using back_bluesoft_bank.Application.Interfaces;
using back_bluesoft_bank.Domain.Entities;
using back_bluesoft_bank.Domain.Enums;
using back_bluesoft_bank.Domain.Exceptions;
using back_bluesoft_bank.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back_bluesoft_bank.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionService> _logger;
    private const int MaxRetries = 3;

    public TransactionService(IUnitOfWork unitOfWork, ILogger<TransactionService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TransactionResponse> DepositAsync(DepositRequest request)
    {
        return await ExecuteWithRetryAsync(async () =>
        {
            await _unitOfWork.BeginTransactionAsync();

            var account = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId)
                ?? throw new NotFoundException($"No se encontró la cuenta con Id {request.AccountId}.");

            account.Deposit(request.Amount);

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                TransactionType = TransactionType.Consignacion,
                Amount = request.Amount,
                BalanceAfter = account.Balance,
                Description = request.Description,
                City = request.City
            };

            await _unitOfWork.Accounts.UpdateAsync(account);
            await _unitOfWork.Transactions.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return MapToResponse(transaction);
        });
    }

    public async Task<TransactionResponse> WithdrawAsync(WithdrawalRequest request)
    {
        return await ExecuteWithRetryAsync(async () =>
        {
            await _unitOfWork.BeginTransactionAsync();

            var account = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId)
                ?? throw new NotFoundException($"No se encontró la cuenta con Id {request.AccountId}.");

            account.Withdraw(request.Amount);

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                TransactionType = TransactionType.Retiro,
                Amount = request.Amount,
                BalanceAfter = account.Balance,
                Description = request.Description,
                City = request.City
            };

            await _unitOfWork.Accounts.UpdateAsync(account);
            await _unitOfWork.Transactions.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return MapToResponse(transaction);
        });
    }


    
    private async Task<TransactionResponse> ExecuteWithRetryAsync(Func<Task<TransactionResponse>> operation)
    {
        for (int attempt = 1; attempt <= MaxRetries; attempt++)
        {
            try
            {
                return await operation();
            }
            catch (DbUpdateConcurrencyException)
            {
                await _unitOfWork.RollbackTransactionAsync();

                if (attempt == MaxRetries)
                {
                    _logger.LogError("Concurrency conflict after {MaxRetries} retries", MaxRetries);
                    throw new ConcurrencyException(
                        "No se pudo completar la operación debido a un conflicto de concurrencia. Intente nuevamente.");
                }

                _logger.LogWarning("Concurrency conflict on attempt {Attempt}, retrying...", attempt);
                await Task.Delay(attempt * 50);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        throw new ConcurrencyException("No se pudo completar la operación.");
    }

    private static TransactionResponse MapToResponse(Transaction t) => new(
        t.Id,
        t.AccountId,
        t.TransactionType,
        t.Amount,
        t.BalanceAfter,
        t.Description,
        t.City,
        t.CreatedAt
    );
}
