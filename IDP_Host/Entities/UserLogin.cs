using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IDP.Entities
{
    [Table("UserLogins")]
    public class UserLogin
    {
        [Key]
        [MaxLength(50)]
        public string LoginId { get; set; }

        [MaxLength(50)]
        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string LoginProvider { get; set; }

        [Required]
        [MaxLength(250)]
        public string ProviderKey { get; set; }
    }
}
