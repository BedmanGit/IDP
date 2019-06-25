using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDP.Entities
{
    [Table("Claims")]
    public class UserClaim
    {         
        [Key]
        [MaxLength(50)]
        public string ClaimId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(250)]
        public string ClaimType { get; set; }

        [Required]
        [MaxLength(250)]
        public string ClaimValue { get; set; }

        public UserClaim(string claimType, string claimValue)
        {
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

        public UserClaim()
        {

        }
    }
}
