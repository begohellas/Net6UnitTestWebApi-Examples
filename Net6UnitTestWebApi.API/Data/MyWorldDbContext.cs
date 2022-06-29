using Microsoft.EntityFrameworkCore;
using Net6UnitTestWebApi.API.Models;

namespace Net6UnitTestWebApi.API.Data;

public class MyWorldDbContext : DbContext
{
    public MyWorldDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Todo> Todos { get; set; } = null!;
}