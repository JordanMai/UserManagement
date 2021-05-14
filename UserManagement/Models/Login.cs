using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Models
{
    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public async Task<int> TryLogin(UserManagement.Data.UserManagementContext _context)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                return -1;
            }

            // set up LINQ for search
            IQueryable<User> users = from u in _context.User select u;

            // narrow down by username
            var User = users.AsEnumerable().Where(
                u => u.Username.Equals(this.Username)
            ).ToList();
            if (!User.Any())
            {
                return 1;
            }
            if(User.Count > 1)
            {
                return 3;
            }

            var Account = User.Single();
            Hasher hasher = new Hasher();
            string tryPass = hasher.HashPassword(this.Password, Account.Salt, 100, 32);

            // check if password matches
            if(Account.Password == tryPass)
            {
                return 0;
            }
            else
            {
                return 2;
            }

            //return -2;
        }
    }
}
