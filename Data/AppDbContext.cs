using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Owo.Core.Models;

namespace Owo.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) 
    : DbContext(options)
{
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}