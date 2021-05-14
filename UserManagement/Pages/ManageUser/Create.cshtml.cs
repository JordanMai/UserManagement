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

namespace UserManagement.Pages.ManageUser
{
    public class CreateModel : PageModel
    {
        private readonly UserManagement.Data.UserManagementContext _context;

        public CreateModel(UserManagement.Data.UserManagementContext context)
        {
            _context = context;
        }

        /*public IActionResult OnGet()
        {
            return Page();
        }*/

        [BindProperty]
        public User User { get; set; }

        public SelectList Questions { get; set; }
        [BindProperty(SupportsGet = true)]
        public SecurityQuestion Question { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.User.Add(User);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public async Task OnGetAsync()
        {
            // get Questions
            IQueryable<string> questionQuery = from q in _context.SecurityQuestion
                                               orderby q.Question
                                               select q.Question;
            Questions = new SelectList(await questionQuery.Distinct().ToListAsync());

        }
    }
}
