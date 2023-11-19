using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechSolutionsAPI.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<SelectListItem> Roles { get; set; }

        public List<string> UserRoles { get; set; }

        public List<string> SelectedRoles { get; set; }
    }
}

