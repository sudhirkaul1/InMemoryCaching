using Microsoft.EntityFrameworkCore;
using WebApiCachingApp.Models;

namespace WebApiCachingApp.Data;
public class ApiDbContext : DbContext
{
    public DbSet<Driver> Drivers {get; set;}
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {      
    }
}