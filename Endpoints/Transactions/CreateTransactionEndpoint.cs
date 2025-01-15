using Owo.Api.Common.Api;
using Owo.Core.Handlers;
using Owo.Core.Models;
using Owo.Core.Requests.Transactions;
using Owo.Core.Responses;

namespace Owo.Api.Endpoints.Transactions;

public class CreateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithName("Transactions: Create")
            .WithSummary("Cria uma nova transição")
            .WithDescription("Cria uma nova transição")
            .WithOrder(1)
            .Produces<Response<Transaction?>>();
 
    private static async Task<IResult> HandleAsync(
        ITransactionHandler handler,
        CreateTransactionRequest request)
    {
        var result  = await handler.CreateAsync(request);
        return result.IsSuccess 
            ? TypedResults.Created($"/{result.Data?.Id}", result.Data) 
            : TypedResults.BadRequest(result.Data);
    }
}