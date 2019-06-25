using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.API.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }

        [MaxLength(100)]
        public string Password { get; set; }
        //public byte[] PasswordHash { get; set; }
        //public byte[] PasswordSalt { get; set; }
        //public string Gender { get; set; }
        //public DateTime DateOfBirth { get; set; }
        //public string KnownAs { get; set; }
        //public DateTime Created { get; set; }
        //public DateTime LastActive { get; set; }
        //public string Introduction { get; set; }
        //public string LookingFor { get; set; }
        //public string Interests { get; set; }
        //public string City { get; set; }
        //public string Country { get; set; }
        public bool IsActive { get; set; }
        public ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();
        public ICollection<UserLogin> Logins { get; set; } = new List<UserLogin>();
        public ICollection<Photo> Photos { get; set; }

    }


}