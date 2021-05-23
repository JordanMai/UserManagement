using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UserManagement.Pages.Service
{
    public class PasswordModel : PageModel
    {
        private readonly UserManagement.Data.UserManagementContext _context;

        public PasswordModel(UserManagement.Data.UserManagementContext context)
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
                if (_context.User.Any(u => u.Email == Email.ToLower()))
                {
                    // get user ID
                    int id = _context.User.Where(u => u.Email == Email).FirstOrDefault().UserID;


                    // remove pending resets
                    foreach(var t in _context.ServiceToken.Where(t => t.UserID == id && t.Action == "password" && t.Resolved == false))
                    {
                        _context.ServiceToken.Remove(t);
                    }


                    // misuse password hasher to make an activation URL
                    Models.Hasher hasher = new Models.Hasher();
                    string Salt = hasher.GenerateSalt(8);
                    string url;
                    do
                    {
                        int i = 0;
                        url = hasher.HashPassword("p" + id + DateTime.UtcNow, "", 10 + i, 8 + (i / 8));
                    } while (_context.ServiceToken.Any(t => t.URL == url));


                    // make reset token
                    Models.ServiceToken newToken;
                    try
                    {
                        newToken = new Models.ServiceToken
                        {
                            UserID = id,
                            Action = "password",
                            URL = url,
                            Creation = DateTime.UtcNow,
                            Expiration = DateTime.UtcNow.AddHours(12),
                            Resolved = false
                        };
                    }
                    catch (Exception ex)
                    {
                        ViewData["message"] = "There was an error creating your password reset token.";
                        return Page();
                    }

                    await _context.ServiceToken.AddAsync(newToken);
                    await _context.SaveChangesAsync();
                    
                    Models.ServiceToken token = _context.ServiceToken.Where(t => t.UserID == id && t.Action == "password" && t.Resolved == false).FirstOrDefault();

                    // TODO: replace with email
                    //return RedirectToPage("./ResetPassword/" + token.URL);
                    ViewData["reset_link"] = "./ResetPassword/" + token.URL;
                    return Page();
                }
                else
                {
                    ViewData["message"] = "There are no accounts with that email address. "
                        + "\nIn the future, we will send password resets to you via email.";
                    return Page();
                }
            }

            return Page();
        }
    }
}
