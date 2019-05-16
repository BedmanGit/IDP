using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class UserForRegisterDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(8, MinimumLength=4, ErrorMessage="Length must be between 4 and 8")]
        public string Password { get; set; }
    }
}