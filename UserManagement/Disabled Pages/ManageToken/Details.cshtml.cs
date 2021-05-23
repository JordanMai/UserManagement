using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;

namespace UserManagement.Pages.ManageToken
{
    public class DetailsModel : PageModel
    {
        private readonly UserManagement.Data.UserManagementContext _context;

        public DetailsModel(UserManagement.Data.UserManagementContext context)
        {
            _context = context;
        }

        public ServiceToken ServiceToken { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ServiceToken = await _context.ServiceToken.FirstOrDefaultAsync(m => m.ID == id);

            if (ServiceToken == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
