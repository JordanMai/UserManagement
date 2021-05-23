using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UserManagement.Pages.Service
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManagement.Data.UserManagementContext _context;

        public ResetPasswordModel(UserManagement.Data.UserManagementContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string AccessURL { get; set; }

        [StringLength(255)]
        [Required]
        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // no token provided
            if (string.IsNullOrEmpty(AccessURL))
            {
                return RedirectToPage("./Error");
            }

            // find activation token with url
            Models.ServiceToken t_token = _context.ServiceToken.Where(t => t.URL == AccessURL && t.Action == "password").FirstOrDefault();

            // no such token
            if (t_token == null)
            {
                return RedirectToPage("./Error");
            }

            // expiration
            if(DateTime.UtcNow >= t_token.Expiration)
            {
                ViewData["warn"] = "This token is expired.";
                return Page();
            }

            // already done
            if (t_token.Resolved)
            {
                ViewData["warn"] = "This token is expired.";
                return Page();
            }

            Models.User t_user = _context.User.Where(u => u.UserID == t_token.UserID).FirstOrDefault();
            ViewData["Username"] = t_user.Username;
            ViewData["Email"] = t_user.Email;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // generate salt and password
            Models.Hasher hasher = new Models.Hasher();
            string Salt = hasher.GenerateSalt(32);
            this.Password = hasher.HashPassword(this.Password, Salt, 100, 32);

            Models.ServiceToken t_token = _context.ServiceToken.Where(t => t.URL == AccessURL && t.Action == "password" && t.Resolved == false).FirstOrDefault();
            var U = _context.User.Where(u => u.UserID == t_token.UserID).FirstOrDefault();
            U.Salt = Salt;
            U.Password = Password;

            t_token.Resolved = true;
            t_token.URL = "";

            await _context.SaveChangesAsync();

            ViewData["message"] = "Password reset successfully. You may now log in.";
            return Page();
        }
    }
}
