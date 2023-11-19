using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using TechSolutionsAPI.Areas.Identity.Data;

using Moq;
using TechSolutionsAPI.Areas.Identity.Pages.Account;
using TechSolutionsAPI.Models;

namespace Test.IntergrationTests
{
    public class LoginControllerTest 
    {
        [Fact]
        public async Task CreateEmployee_ValidModel_RedirectsToShowEmployees()
        {
            var factory =new ApiFactory();
            var client = factory.CreateClient();
            // Arrange
            var model = new EmployeeViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                JobTitle = "Software Engineer"
                // Add other properties as needed
            };

            // Act
            var response = await client.PostAsJsonAsync("/Employee/CreateEmployee", model);

            // Assert
            response.EnsureSuccessStatusCode(); 


        }

        // Add more test methods for different scenarios
    }


}

