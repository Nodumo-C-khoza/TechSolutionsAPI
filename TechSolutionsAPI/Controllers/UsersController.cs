using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TechSolutionsAPI.Areas.Identity.Data;
using TechSolutionsAPI.Models;

namespace TechSolutionsAPI.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> LoginAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (roles.Contains("User"))
                {
                    return RedirectToAction("Index", "Users");
                }

                // Handle other roles or scenarios if needed
            }

            return View("~/Areas/Identity/Pages/_AuthLayout.cshtml");
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            // Retrieve the user from the database using the provided id
            var user = await _userManager.FindByIdAsync(id);

            // Check if the user exists
            if (user == null)
            {
                return NotFound();
            }

            // You might want to pass the user data to the view
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser user)
        {
            // Ensure the id from the route matches the id in the model
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

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    // Handle exceptions, log, etc.
                    throw;
                }
            }

            // If the model is not valid, redisplay the form with validation errors
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
                // Redirect to the user list or another appropriate action
                return RedirectToAction("Index");
            }

            // If deletion fails, handle accordingly (e.g., show an error message)
            // You can inspect the errors using result.Errors

            return View(user); // Return to the delete confirmation view with the user details
        }
    }



    // ... (other actions remain unchanged)

    // Additional actions can be added as needed
}

