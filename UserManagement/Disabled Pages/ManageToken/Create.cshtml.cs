using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Pages.ManageToken
{
    public class CreateModel : PageModel
    {
        private readonly UserManagement.Data.UserManagementContext _context;

        public CreateModel(UserManagement.Data.UserManagementContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ServiceToken ServiceToken { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ServiceToken.Add(ServiceToken);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
