using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models
{
    [Serializable]
    public class User
    {
        [Key]
        [Required]
        public int UserID { get; set; }

        [StringLength(20)]
        [Required]
        [Display(Name = "First")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Last")]
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

        [StringLength(255)]
        [Required]
        public string Salt { get; set; }
    }
}
