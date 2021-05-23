using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UserManagement.Pages
{
    public class ActivateModel : PageModel
    {
        private readonly UserManagement.Data.UserManagementContext _context;

        public ActivateModel(UserManagement.Data.UserManagementContext context)
        {
            _context = context;
        }


        [BindProperty(SupportsGet = true)]
        public string AccessURL { get; set; }

        [BindProperty]
        public int TokenID { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // no token provided
            if (string.IsNullOrEmpty(AccessURL))
            {
                return RedirectToPage("./Error");
            }

            // find activation token with url
            Models.ServiceToken t_token = _context.ServiceToken.Where(t => t.URL == AccessURL && t.Action == "activate").FirstOrDefault();

            // no such token
            if(t_token == null)
            {
                return RedirectToPage("./Error");
            }

            // expiration
            if (DateTime.UtcNow >= t_token.Expiration)
            {
                ViewData["warn"] = "This token is expired.";
                return Page();
            }

            // already done
            if (t_token.Resolved)
            {
                ViewData["warn"] = "This account is already activated.";
                return Page();
            }


            Models.User t_user = _context.User.Where(u => u.UserID == t_token.UserID).FirstOrDefault();
            ViewData["Username"] = t_user.Username;
            //ViewData["token_id"] = TokenID = t_token.ID;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            foreach(var t in _context.ServiceToken.Where(t => t.URL == AccessURL))
            {
                t.Resolved = true;
                t.URL = "";
            }
            await _context.SaveChangesAsync();

            ViewData.Remove("warn");
            ViewData.Remove("Username");
            ViewData["message"] = "Successfully activated account. You may now log in.";
            return Page();
        }
    }
}
