using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models
{
    public class SecurityQuestion
    {
        [Key]
        [Required]
        public int QuestionID { get; set; }

        [Required]
        public string Question { get; set; }
    }
}
