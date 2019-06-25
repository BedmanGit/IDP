using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDP.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]  
        public int UserId { get; set; }
    
        [MaxLength(100)]
        [Required]
        public string UserName { get; set; }

        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        public bool IsActive { get; set; }


        public ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();

        public ICollection<UserLogin> Logins { get; set; } = new List<UserLogin>();


    }
}
