using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Models
{
    [Table("UserLogins")]
    public class UserLogin
    {
        [Key]
        [MaxLength(50)]
        public string LoginId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(250)]
        public string LoginProvider { get; set; }

        [Required]
        [MaxLength(250)]
        public string ProviderKey { get; set; }
    }
}
