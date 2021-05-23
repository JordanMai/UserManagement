using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UserManagement.Pages.Service
{
    public class UsernameModel : PageModel
    {
        private readonly UserManagement.Data.UserManagementContext _context;

        public UsernameModel(UserManagement.Data.UserManagementContext context)
        {
            _context = context;
        }


        [StringLength(255, MinimumLength = 6)]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$")]
        [BindProperty]
        public string Email { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["warn"] = "Invalid Username.";
                return Page();
            }

            if (!string.IsNullOrEmpty(Email))
            {
                if(_context.User.Any(u => u.Email == Email.ToLower()))
                {
                    ViewData["message"] = "Your username is \"" + _context.User.Where(u => u.Email == Email.ToLower()).FirstOrDefault().Username + "\" "
                        + "\nIn the future, we will send your username to you via email.";
                }
                else
                {
                    ViewData["message"] = "There are no accounts with that email address. "
                        + "\nIn the future, we will send your username to you via email.";
                }
            }

            return Page();
        }
    }
}
