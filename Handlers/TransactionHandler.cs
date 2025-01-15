using Microsoft.EntityFrameworkCore;
using Owo.Api.Data;
using Owo.Core.Handlers;
using Owo.Core.Models;
using Owo.Core.Requests.Transactions;
using Owo.Core.Responses;

namespace Owo.Api.Handlers;

public class TransactionHandler(AppDbContext context) : ITransactionHandler
{
    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        try
        {
            var transaction = new Transaction
            {
                UserId = request.UserId,
                Title = request.Title,
                PaidOrReceivedAt = request.PaidOrReceivedAt,
                Type = request.Type,
                Amount = request.Amount,
                CategoryId = request.CategoryId
            };
            
            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();
            
            return new Response<Transaction?>(transaction, 201, "Transação efetuada com sucesso!");
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Não foi possível efetuar a transação");
        }
    }
    
    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        try
        {
            var transaction = await context
                .Transactions
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId && x.CategoryId == request.CategoryId);

            if (transaction is null)
                return new Response<Transaction?>(null, 404, "Transação não encontrada");

            transaction.Title = request.Title;
            transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;
            transaction.Type = request.Type;
            transaction.Amount = request.Amount;
            transaction.CategoryId = request.CategoryId;

            context.Transactions.Update(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, message: "Transação atualizada com sucesso!");
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Não foi possível alterar a transação");
        }
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        try
        {
            var transaction = await context
                .Transactions
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (transaction == null)
                return new Response<Transaction?>(null, 404, "Transação não encontrada");
            
            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync();
            
            return new Response<Transaction?>(transaction, message:"Transação deletada com sucesso!");
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Não foi possível excluir a transação");
        }
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        try
        {
            var transaction = await context
                .Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            return transaction is null
                ? new Response<Transaction?>(null, 404, "Transação não encontrada")
                : new Response<Transaction?>(transaction);
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Não foi possível recuperar a transação");
        }
    }

    public async Task<PagedResponse<List<Transaction>>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
    {
        try
        {
            var query = context
                .Transactions
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId && x.CreatedAt >= request.StartDate && x.CreatedAt <= request.EndDate)
                .OrderBy(x => x.CreatedAt);
            
            var transactions = await query
                .Skip((request.PageNumber - 1 ) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();
            
            var count = await query.CountAsync();
            
            return new PagedResponse<List<Transaction>>(transactions, count, request.PageNumber, request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Transaction>>(null, 500, "Não foi possível consultar as transações");
        }
    }
}