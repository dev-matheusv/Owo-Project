using System.Security.Claims;
using Owo.Api.Common.Api;
using Owo.Core.Handlers;
using Owo.Core.Models;
using Owo.Core.Requests;
using Owo.Core.Requests.Categories;
using Owo.Core.Responses;

namespace Owo.Api.Endpoints.Categories;

public class CreateCategoryEndpoint : IEndpoint
{ 
    public static void Map(IEndpointRouteBuilder app)
         => app.MapPost("/", HandleAsync)
             .WithName("Categories: Create")
             .WithSummary("Cria uma nova categoria")
             .WithDescription("Cria uma nova categoria")
             .WithOrder(1)
             .Produces<Response<Category?>>();
 
     private static async Task<IResult> HandleAsync(
         ClaimsPrincipal user,
         ICategoryHandler handler,
         CreateCategoryRequest request)
     {
         request.UserId = user.Identity?.Name ?? string.Empty;
         var result  = await handler.CreateAsync(request);
         return result.IsSuccess 
             ? TypedResults.Created($"/{result.Data?.Id}", result.Data) 
             : TypedResults.BadRequest(result.Data);
     }
   
}