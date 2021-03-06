using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Pages.ManageAnswer
{
    public class EditModel : PageModel
    {
        private readonly UserManagement.Data.UserManagementContext _context;

        public EditModel(UserManagement.Data.UserManagementContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SecurityAnswer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SecurityAnswerExists(SecurityAnswer.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SecurityAnswerExists(int id)
        {
            return _context.SecurityAnswer.Any(e => e.ID == id);
        }
    }
}
