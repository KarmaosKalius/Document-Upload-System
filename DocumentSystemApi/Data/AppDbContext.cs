using Microsoft.EntityFrameworkCore;
using DocumentSystemApi.Models;
namespace DocumentSystemApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public DbSet<Document> Documents{get;set;}
}