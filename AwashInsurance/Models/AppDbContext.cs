// Models/AppDbContext.cs
using AwashInsurance.Models;
using Microsoft.EntityFrameworkCore;

namespace AwashInsurance.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Employee> Employees { get; set; }

    }
}
