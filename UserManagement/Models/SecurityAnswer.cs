using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models
{
    public class SecurityAnswer
    {
        [Key]
        public int ID { get; set; }
        
        [ForeignKey("User")]
        [Required]
        public int UserID { get; set; }

        [ForeignKey("SecurityQuestion")]
        [Required]
        public int QuestionID { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 4)]
        public string Answer { get; set; }
    }
}
