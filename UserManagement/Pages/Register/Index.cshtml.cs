using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Pages.Register
{
    public class IndexModel : PageModel
    {
        private readonly UserManagement.Data.UserManagementContext _context;

        public IndexModel(UserManagement.Data.UserManagementContext context)
        {
            _context = context;
        }



        [BindProperty(SupportsGet = true)]
        public Models.SignUp Credentials { get; set; }

        public SelectList QuestionList { get; set; }

        /*
        [BindProperty(SupportsGet = true)]
        public List<Models.SecurityQuestion> Questions { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<Models.SecurityAnswer> Answers { get; set; }
        */

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.TryGetValue("login_user", out _))
            {
                return RedirectToPage("./UserSystem/Index");
            }
            
            // get Questions
            IQueryable<string> questionQuery = from q in _context.SecurityQuestion
                                               orderby q.Question
                                               select q.Question;
            QuestionList = new SelectList(await questionQuery.Distinct().ToListAsync());

            return Page();
        }

        public async Task OnPostAsync()
        {
            if (string.IsNullOrEmpty(Credentials.Email) || string.IsNullOrEmpty(Credentials.Username) || string.IsNullOrEmpty(Credentials.Password)
                || string.IsNullOrEmpty(Credentials.FirstName) || string.IsNullOrEmpty(Credentials.LastName))
            {
                await OnGetAsync();
                return;
            }

            // verify questions
            foreach (var q in Credentials.SecurityQuestions)
            {
                if (string.IsNullOrEmpty(q.Question))
                {
                    await OnGetAsync();
                    return;
                }
            }
            // verify answers
            for (int i = 0; i < Credentials.SecurityAnswers.Count(); i++)
            {
                var a = Credentials.SecurityAnswers[i];
                a.QuestionID = Credentials.SecurityQuestions[i].QuestionID;
                if (string.IsNullOrEmpty(a.Answer))
                {
                    await OnGetAsync();
                    return;
                }
            }

            if(_context.User.Count() > 0)
            {
                if (_context.User.Any(u => u.Username == Credentials.Username))
                {
                    ViewData["warn"] = "Username \"" + Credentials.Username + "\" is taken.";
                    await OnGetAsync();
                    return;
                }
                if (_context.User.Any(u => u.Email == Credentials.Email))
                {
                    ViewData["warn"] = "Email \"" + Credentials.Email + "\" is already in use.";
                    await OnGetAsync();
                    return;
                }
            }

            if (await Credentials.TrySignup(_context))
            {
                // provide activation token directly until email is enabled
                // TODO: replace with email
                Models.User user = _context.User.Where(u => u.Username == Credentials.Username && u.Password == Credentials.Password).FirstOrDefault();
                Models.ServiceToken token = _context.ServiceToken.Where(t => t.UserID == user.UserID && t.Action == "activate").FirstOrDefault();
                HttpContext.Session.Set("activation_token", Models.UMSerializer.SerializeToken(token));

                Response.Redirect("./Register/Success");
                return;
            }
            else
            {
                ViewData["warn"] = "An error has occurred while creating your account (Username:" + Credentials.Username + "). Please contact an administrator.";
                await OnGetAsync();
                return;
            }

            //Response.Redirect("../Index");
        }

    }
}
