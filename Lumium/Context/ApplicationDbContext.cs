using Lumium.Models;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; }
}