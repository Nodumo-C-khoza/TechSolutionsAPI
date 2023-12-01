using Microsoft.AspNetCore.Identity;
using System;
using TechSolutionsAPI.Constants;
using TechSolutionsAPI.Data;
using TechSolutionsAPI.Models;  // Import the Employee model

namespace TechSolutionsAPI.Areas.Identity.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, TSContext tSContext)
        {
            // Assign the context
            var _tSContext = tSContext;

            // Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
    
            var defaultUser = new ApplicationUser
            {
                UserName = "Admin@gmail.com",
                Email = "Admin@gmail.com",
                FirstName = "Nodumo",
                LastName = "Khoza",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.User.ToString());
                }

            }
            if (!_tSContext.Customer.Any())
            {
                // Seed data for 3 employees
                string[] firstNames = { "John", "Jane", "Michael", "Emily", "David", "Olivia", "Daniel", "Sophia", "Christopher", "Emma" };
                string[] lastNames = { "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor" };
                string[] addresses = { "123 Main Street, Cityville, State 12345", "456 Elm Avenue, Townsville, State 56789", "789 Oak Lane, Villageton, State 98765", "321 Pine Road, Hamletville, State 54321",
                                    "876 Cedar Boulevard, Boroughburg, State 13579","543 Maple Drive, Villageburg, State 24680","210 Birch Street, Citytown, State 86420","987 Spruce Lane, Townberg, State 65432",
                                     "654 Willow Avenue, Villagetown, State 10987", "321 Redwood Road, Metropolis, State 56701"
};

                Random rand = new Random();

                for (int i = 1; i < 10 ; i++)
                {
                    var employee = new Customer
                    {
                        FirstName = firstNames[rand.Next(firstNames.Length)],
                        LastName = lastNames[rand.Next(lastNames.Length)],
                        Address = addresses[rand.Next(addresses.Length)],
                    };

                    _tSContext.Customer.Add(employee);
                }

                // Save changes to the database
                await _tSContext.SaveChangesAsync();
            }


        }

    }
}
