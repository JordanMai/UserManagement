using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        [BindProperty]
        public Models.Login Credentials {get; set;}



        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.TryGetValue("login_user", out _))
            {
                return RedirectToPage("./UserSystem/Index");
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["warn"] = "Incomplete login info.";
                return Page();
            }

            // store successful login
            User success = await Credentials.TryLogin(_context);

            // try again if failed
            if (success == null)
            {
                ViewData["warn"] = "Invalid login info.";
                return Page();
            }

            // check if account is activated
            if (_context.ServiceToken.Any(t => t.UserID == success.UserID && t.Action == "activate" && t.Resolved == false))
            {
                ViewData["warn"] = "Account is not activated.";
                return Page();
            }

            // store login user in session
            HttpContext.Session.Set("login_user", UMSerializer.SerializeUser(success));

            return RedirectToPage("./UserSystem/Index");
        }


        /*public async Task OnGetAsync()
        {

        }*/

    }
}
