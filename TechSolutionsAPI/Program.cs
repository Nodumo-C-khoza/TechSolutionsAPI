using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TechSolutionsAPI.Areas.Identity.Data;
using TechSolutionsAPI.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TSContextConnection") ?? throw new InvalidOperationException("Connection string 'TSContextConnection' not found.");

builder.Services.AddDbContext<TSContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
     .AddEntityFrameworkStores<TSContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
    options.AddPolicy("SuperAdmin", policy => policy.RequireRole("SuperAdmin"));
});


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "");
    });

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); ;

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Users}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "editEmployee",
        pattern: "Admin/EditEmployee/{id:guid}",
        defaults: new { controller = "Admin", action = "EditEmployee" });

});
app.MapRazorPages();
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<TSContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        await DbSeeder.SeedRolesAndAdminAsync(userManager, roleManager, context);
    }
    catch (Exception ex)
    {
        throw;
    }

}

app.Run();