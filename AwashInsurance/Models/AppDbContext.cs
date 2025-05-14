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

        public DbSet<OrganizationType> OrganizationTypes { get; set; }
        public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }

        // user account rule DBMS mapping
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserAccount>()
                .HasIndex(u => u.LoginId)
                .IsUnique(); // No duplicate login IDs allowed

            modelBuilder.Entity<UserAccount>()
                .HasIndex(u => new { u.EmployeeId, u.RoleId })
                .IsUnique(); // Same employee can't get the same role twice

            modelBuilder.Entity<UserAccount>()
                .HasOne(u => u.Employee)
                .WithMany(e => e.UserAccounts)
                .HasForeignKey(u => u.EmployeeId); // Link UserAccount → Employee

            modelBuilder.Entity<UserAccount>()
                .HasOne(u => u.Role)
                .WithMany(r => r.UserAccounts)
                .HasForeignKey(u => u.RoleId); // Link UserAccount → Role
        }
    }
}


