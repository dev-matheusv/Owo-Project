using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Owo.Api.Data;
using Owo.Api.Endpoints;
using Owo.Api.Handlers;
using Owo.Api.Models;
using Owo.Core.Handlers;
using Owo.Core.Models;
using Owo.Core.Requests.Categories;
using Owo.Core.Responses;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

var cnnStr = builder.Configuration.GetConnectionString("DefaultConnection") ?? String.Empty;

builder
    .Services
    .AddDbContext<AppDbContext>(
        x => { x.UseSqlServer(cnnStr); });
builder.Services
    .AddIdentityCore<User>()
    .AddRoles<IdentityRole<long>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => { x.CustomSchemaIds(type => type.FullName); });

builder.Services
    .AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();
builder.Services.AddAuthorization();

builder
    .Services
    .AddTransient<ICategoryHandler, CategoryHandler>();
builder
    .Services
    .AddTransient<ITransactionHandler, TransactionHandler>();

// Injeção de dependência

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => new { message = "OK"});
app.MapEndpoints();

app.Run();