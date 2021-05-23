using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UserManagement.Pages.Register
{
    public class SuccessModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            byte[] tokenBytes;
            if(HttpContext.Session.TryGetValue("activation_token", out tokenBytes))
            {
                var token = Models.UMSerializer.DeserializeToken(tokenBytes);

                if(token == null)
                {
                    ViewData.Remove("token");
                }
                else
                {
                    ViewData["token"] = token;
                }

                return Page();
            }

            return RedirectToPage("./Error");
        }
    }
}
