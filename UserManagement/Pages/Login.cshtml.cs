using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Pages
{
    //[BindProperties]
    public class LoginModel : PageModel
    {
        private readonly UserManagement.Data.UserManagementContext _context;

        public LoginModel(UserManagement.Data.UserManagementContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Models.Login Credentials {get; set;}

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData.Remove("login");
                return Page();
            }

            ViewData["login"] = ("Username: " + Credentials.Username + "\nPassword: " + Credentials.Password + "\nValid:" + await Credentials.TryLogin(_context));

            return Page();

            //return RedirectToPage("./Index");
        }

        /*public async Task OnGetAsync()
        {

        }*/

    }
}
