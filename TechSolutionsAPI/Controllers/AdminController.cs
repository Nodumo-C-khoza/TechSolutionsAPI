using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TechSolutionsAPI.Areas.Identity.Data;
using TechSolutionsAPI.Constants;
using TechSolutionsAPI.Data;
using TechSolutionsAPI.Models;

namespace TechSolutionsAPI.Controllers
{

    [Authorize(Policy = "AdminPolicy")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TSContext _tSContext;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, TSContext tSContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tSContext = tSContext;
        }

        // GET: AdminController
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        public IActionResult Create()
        {
            var roles = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
            var model = new CreateUserViewModel
            {
                Roles = roles
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,

                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var defaultRole = "User";
                    await _userManager.AddToRoleAsync(user, defaultRole);

                    return RedirectToAction("Index", "Admin");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            var roles = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();

            return View(model);
        }

        public IActionResult CreateUserWithAdminRole()
        {
            var roles = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
            var model = new CreateUserViewModel
            {
                Roles = roles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserWithAdminRole(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,

                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Add user to the selected role
                    var defaultRole = "Admin";
                    await _userManager.AddToRoleAsync(user, defaultRole);

                    return RedirectToAction("Index", "Admin");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            var roles = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
            model.Roles = roles;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _userManager.FindByIdAsync(id);

                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.Email = user.Email;

                    await _userManager.UpdateAsync(existingUser);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return View(user);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return View(user);
        }


        public IActionResult FilterAdmin()
        {
            var adminRole = "Admin";
            var usersWithAdminRole = _userManager.GetUsersInRoleAsync(adminRole).Result;

            var viewModel = usersWithAdminRole.Select(user => new UserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            }).ToList();

            return View(viewModel);
        }

        public IActionResult FilterUsers()
        {
            // Get users with the "Admin" role
            var adminRole = "User";
            var usersWithAdminRole = _userManager.GetUsersInRoleAsync(adminRole).Result;

            var viewModel = usersWithAdminRole.Select(user => new UserViewModel
            {
                // Map the properties you want to display
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            }).ToList();

            return View(viewModel);
        }

        public IActionResult ShowCustomer()
        {
            List<Customer> customers = _tSContext.Customer.ToList();

            List<CustomerViewModel> customerViewModel = customers
                .Select(e => new CustomerViewModel
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Address = e.Address,
                   
                })
                .ToList();

            return View( customerViewModel);
        }


        public IActionResult EditCustomer(int id)
        {
            var Customer = _tSContext.Customer.Find(id);

            if (Customer == null)
            {
                return NotFound(); // Customer not found
            }

            // Map the Customer model to the CustomerViewModel
            var CustomerViewModel = new CustomerViewModel
            {
                Id = Customer.Id,
                FirstName = Customer.FirstName,
                LastName = Customer.LastName,
                Address = Customer.Address
            };

            return View(CustomerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCustomer(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingCustomer = _tSContext.Customer.FirstOrDefault(e => e.Id == model.Id);

                if (existingCustomer == null)
                {
                    return NotFound(); // Customer not found
                }

                existingCustomer.FirstName = model.FirstName;
                existingCustomer.LastName = model.LastName;
                existingCustomer.Address = model.Address;

                _tSContext.SaveChanges();

                return RedirectToAction("ShowCustomer");
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult CreateCustomer()
        {
            // This action is for displaying the create Customer form
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomer(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newCustomer = new Customer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,

                };

                _tSContext.Customer.Add(newCustomer);

                await _tSContext.SaveChangesAsync();

                return RedirectToAction("ShowCustomer");
            }
            return View(model);
        }

        public IActionResult DeleteCustomer(int id)
        {
            var Customer = _tSContext.Customer.Find(id);

            if (Customer == null)
            {
                return NotFound(); // Customer not found
            }

            var CustomerViewModel = new CustomerViewModel
            {
                Id = Customer.Id,
                FirstName = Customer.FirstName,
                LastName = Customer.LastName,
                Address = Customer.Address
            };

            return View(CustomerViewModel);
        }

        [HttpPost, ActionName("DeleteCustomer")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDeleteCustomer(int id)
        {
            var existingCustomer = _tSContext.Customer.FirstOrDefault(e => e.Id == id);

            if (existingCustomer == null)
            {
                return NotFound(); // Customer not found
            }

            _tSContext.Customer.Remove(existingCustomer);
            _tSContext.SaveChanges();

            return RedirectToAction("ShowCustomer");
        }


        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id, ApplicationUser user)
        {

            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing user from the database
                    var existingUser = await _userManager.FindByIdAsync(id);

                    // Check if the user exists
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    // Update user properties
                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.Email = user.Email;

                    // Save changes
                    await _userManager.UpdateAsync(existingUser);

                    return RedirectToAction("FilterUsers");
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                // Redirect to the user list or another appropriate action
                return RedirectToAction("Index");
            }

            return View(user);
        }


    }


}


