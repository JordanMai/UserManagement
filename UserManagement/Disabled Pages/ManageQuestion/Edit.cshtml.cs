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

namespace UserManagement.Pages.ManageQuestion
{
    public class EditModel : PageModel
    {
        private readonly UserManagement.Data.UserManagementContext _context;

        public EditModel(UserManagement.Data.UserManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SecurityQuestion SecurityQuestion { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SecurityQuestion = await _context.SecurityQuestion.FirstOrDefaultAsync(m => m.QuestionID == id);

            if (SecurityQuestion == null)
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

            _context.Attach(SecurityQuestion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SecurityQuestionExists(SecurityQuestion.QuestionID))
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

        private bool SecurityQuestionExists(int id)
        {
            return _context.SecurityQuestion.Any(e => e.QuestionID == id);
        }
    }
}
