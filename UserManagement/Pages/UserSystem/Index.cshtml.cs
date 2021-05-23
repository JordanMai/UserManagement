using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace UserManagement.Pages.UserSystem
{
    public class IndexModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            byte[] userBytes;
            HttpContext.Session.TryGetValue("login_user", out userBytes);
            ViewData["login_user"] = Models.UMSerializer.DeserializeUser(userBytes);

            if (ViewData["login_user"] == null)
            {
                return RedirectToPage("../Index");
            }

            return Page();
        }
    }
}
