using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Common;
using TechSolutionsAPI.Data;

namespace Test
{
    internal class ApiFactory : WebApplicationFactory<Program>//, IAsyncLifetime
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
             builder.ConfigureServices(services =>
                {
                    var options = new DbContextOptionsBuilder<TSContext>()
                               .UseSqlServer("TSContextConnection");
                    var dbContext = new TSContext(options.Options);
                    dbContext.Database.Migrate();
                    //// Remove existing DbContextOptions and DbConnection services
                    //var dbContextDescriptor = services.SingleOrDefault(
                    //    d => d.ServiceType == typeof(DbContextOptions<TSContext>));
                    //services.Remove(dbContextDescriptor);

                    //var dbConnectionDescriptor = services.SingleOrDefault(
                    //    d => d.ServiceType == typeof(DbConnection));
                    //services.Remove(dbConnectionDescriptor);
                    //var options = new DbContextOptionsBuilder<TSContext>()
                    //            .UseSqlServer("Server=localhost,14331;Database=AspNetCoreTesting;User Id=sa;Password=P@ssword123");
                    //var dbContext = new TSContext(options.Options);
                    //dbContext.Database.Migrate();

                    // Use SQL Server
                    //services.AddDbContext<TSContext>((container, options) =>
                    //{
                    //    var connectionString = builder.Configuration.GetConnectionString("TSContextConnection") ?? throw new InvalidOperationException("Connection string 'TSContextConnection' not found.");
                    //});
                });

                builder.UseEnvironment("Development");
            }
    }
}
