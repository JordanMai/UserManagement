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
    public class IndexModel : PageModel
    {
        private readonly UserManagement.Data.UserManagementContext _context;

        public IndexModel(UserManagement.Data.UserManagementContext context)
        {
            _context = context;
        }

        public IList<SecurityQuestion> SecurityQuestion { get;set; }

        public async Task OnGetAsync()
        {
            SecurityQuestion = await _context.SecurityQuestion.ToListAsync();
        }
    }
}
