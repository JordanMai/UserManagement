using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Models
{

    public class SignUp
    {
        [StringLength(20)]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(255, MinimumLength = 6)]
        //https://www.regular-expressions.info/email.html
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$")]
        [Required]
        public string Email { get; set; }

        [StringLength(16)]
        [Required]
        public string Username { get; set; }

        [StringLength(255)]
        [Required]
        public string Password { get; set; }

        public List<SecurityQuestion> SecurityQuestions { get; set; }
        public List<SecurityAnswer> SecurityAnswers { get; set; }


        public SignUp()
        {
            SecurityQuestions = new List<SecurityQuestion>();
            SecurityAnswers = new List<SecurityAnswer>();

            SecurityQuestions.Add(new SecurityQuestion());
            SecurityAnswers.Add(new SecurityAnswer());
        }

        public async Task<bool> TrySignup(UserManagement.Data.UserManagementContext _context)
        {
            // verify all fields filled
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password)
                || string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
            {
                return false;
            }

            Username = Username.ToLower();
            Email = Email.ToLower();
            
            // make sure not already in use
            if(_context.User.Count() > 0 && (_context.User.Any(u => u.Username == this.Username || u.Email == this.Email)))
            {
                return false;
            }

            // verify questions
            foreach(var q in SecurityQuestions)
            {
                if (string.IsNullOrEmpty(q.Question))
                {
                    return false;
                }
                q.QuestionID = _context.SecurityQuestion.Where(x => x.Question == q.Question).FirstOrDefault().QuestionID;
            }
            // verify answers
            for(int i = 0; i < SecurityAnswers.Count(); i++)
            {
                var a = SecurityAnswers[i];
                a.QuestionID = SecurityQuestions[i].QuestionID;
                if (string.IsNullOrEmpty(a.Answer))
                {
                    return false;
                }
            }

            // generate salt and password
            Hasher hasher = new Hasher();
            string Salt = hasher.GenerateSalt(32);
            this.Password = hasher.HashPassword(this.Password, Salt, 100, 32);

            User NewUser;
            try
            {
                NewUser = new User
                {
                    Username = this.Username
                    ,
                    FirstName = this.FirstName
                    ,
                    LastName = this.LastName
                    ,
                    Email = this.Email
                    ,
                    Salt = Salt
                    ,
                    Password = this.Password
                };
            }
            catch(Exception ex)
            {
                return false;
            }

            // add user
            await _context.User.AddAsync(
                NewUser
                );
            await _context.SaveChangesAsync();

            // get id to assign answer
            int id = _context.User.FirstOrDefault(u => u.Username == NewUser.Username).UserID;

            if(id < 1)
            {
                return false;
            }

            // assign answers
            foreach(SecurityAnswer a in SecurityAnswers)
            {
                await _context.SecurityAnswer.AddAsync(
                    new SecurityAnswer
                    {
                        UserID = id
                        //make matching easier
                        , Answer = a.Answer.ToLower().Replace(" ","").Replace("\t","").Replace("\n","")
                        , QuestionID = a.QuestionID
                    }
                );
            }

            await _context.SaveChangesAsync();

            // misuse password hasher to make an activation URL
            Salt = hasher.GenerateSalt(8);
            string url;
            do
            {
                int i = 0;
                url = hasher.HashPassword("a" + id + DateTime.UtcNow, "", 10 + i, 8 + (i/8));
            } while (_context.ServiceToken.Any(t => t.URL == url));

            // make activation token
            ServiceToken newToken;
            try
            {
                newToken = new ServiceToken {
                    UserID = id,
                    Action = "activate",
                    URL = url,
                    Creation = DateTime.UtcNow,
                    Expiration = DateTime.UtcNow.AddDays(365),
                    Resolved = false
                };
            }
            catch(Exception ex)
            {
                return false;
            }

            await _context.ServiceToken.AddAsync(newToken);
            await _context.SaveChangesAsync();


            return true;
        }
    }
}
