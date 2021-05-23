using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace UserManagement.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {

            /*byte[] userBytes;
            HttpContext.Session.TryGetValue("login_user", out userBytes;
            Models.User user = Models.UMSerializer.DeserializeUser(userBytes);*/

            if (HttpContext.Session.TryGetValue("login_user", out _))
            {
                return RedirectToPage("./UserSystem/Index");
            }

            return Page();
        }
    }
}
