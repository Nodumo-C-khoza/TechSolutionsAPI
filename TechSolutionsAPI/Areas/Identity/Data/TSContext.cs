using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechSolutionsAPI.Areas.Identity.Data;

namespace TechSolutionsAPI.Data;

public class TSContext : IdentityDbContext<ApplicationUser>
{
    public TSContext(DbContextOptions<TSContext> options)
        : base(options)
    {
    }
    public DbSet<Customer> Customer { get; set; }
    //public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
       
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
