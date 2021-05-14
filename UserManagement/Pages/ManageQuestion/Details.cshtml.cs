using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Pages.ManageQuestion
{
    public class DetailsModel : PageModel
    {
        private readonly UserManagement.Data.UserManagementContext _context;

        public DetailsModel(UserManagement.Data.UserManagementContext context)
        {
            _context = context;
        }

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
    }
}
