using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data;

namespace UserManagement.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new UserManagementContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<UserManagementContext>>()))
            {
                if (!context.SecurityQuestion.Any())
                {
                    context.SecurityQuestion.AddRange(
                        new SecurityQuestion
                        {
                            Question = "What is your mother's maiden name?"
                        }
                        , new SecurityQuestion
                        {
                            Question = "What street did you grow up on?"
                        }
                        , new SecurityQuestion
                        {
                            Question = "What is the name of your childhood best friend?"
                        }
                        , new SecurityQuestion
                        {
                            Question = "What was your first car?"
                        }
                    );

                    context.SaveChanges();
                }
            }



        }
    }
}
