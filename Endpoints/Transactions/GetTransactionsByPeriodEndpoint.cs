using Microsoft.AspNetCore.Mvc;
using Owo.Api.Common.Api;
using Owo.Core;
using Owo.Core.Handlers;
using Owo.Core.Models;
using Owo.Core.Requests.Transactions;
using Owo.Core.Responses;

namespace Owo.Api.Endpoints.Transactions;

public class GetTransactionsByPeriodEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .WithName("Transactions: Get By Period Id")
            .WithSummary("Recupera as transações dentro de um determinado período de tempo")
            .WithDescription("Recupera as transações dentro de um determinado período de tempo")
            .WithOrder(5)
            .Produces<PagedResponse<List<Transaction>?>>();

    private static async Task<IResult> HandleAsync(
        ITransactionHandler handler,
        DateTime startDate,
        DateTime endDate,
        [FromQuery]int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery]int pageSize = Configuration.DefaultPageSize)
    {
        var request = new GetTransactionsByPeriodRequest
        {
            UserId = "teste@devmatheus",
            StartDate = startDate,
            EndDate = endDate,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        
        var result  = await handler.GetByPeriodAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok(result.Data) 
            : TypedResults.BadRequest(result.Data);
    }
}