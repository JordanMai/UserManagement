using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Models
{
    public class Login
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public async Task<User> TryLogin(UserManagement.Data.UserManagementContext _context)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                return null;
            }

            // set up LINQ for search
            IQueryable<User> users = from u in _context.User select u;

            // narrow down by username
            var User = users.AsEnumerable().Where(
                u => u.Username.Equals(this.Username)
            ).ToList();
            if (!User.Any())
            {
                return null;
            }
            if(User.Count > 1)
            {
                return null;
            }

            var Account = User.Single();
            Hasher hasher = new Hasher();
            string tryPass = hasher.HashPassword(this.Password, Account.Salt, 100, 32);

            // check if password matches
            if(Account.Password == tryPass)
            {
                return Account;
            }
            else
            {
                return null;
            }
        }
    }
}
