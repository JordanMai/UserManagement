using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models
{
    public class ServiceToken
    {
        [Key]
        public int ID { get; set; }
        
        [Required]
        public string URL { get; set; }

        [ForeignKey("User")]
        [Required]
        public int UserID { get; set; }

        [Required]
        public string Action { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime Expiration { get; set; }

        [Required]
        public bool Resolved { get; set; }
    }
}
