using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Pages.ManageAnswer
{
    public class DeleteModel : PageModel
    {
        private readonly UserManagement.Data.UserManagementContext _context;

        public DeleteModel(UserManagement.Data.UserManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SecurityAnswer SecurityAnswer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SecurityAnswer = await _context.SecurityAnswer.FirstOrDefaultAsync(m => m.ID == id);

            if (SecurityAnswer == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SecurityAnswer = await _context.SecurityAnswer.FindAsync(id);

            if (SecurityAnswer != null)
            {
                _context.SecurityAnswer.Remove(SecurityAnswer);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
