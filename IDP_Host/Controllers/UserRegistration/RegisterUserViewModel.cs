﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDP.Controllers
{
    public class RegisterUserViewModel
    {
        // credentials       
        [MaxLength(100)]
        public string Username { get; set; }

        [MaxLength(100)]
        public string Password { get; set; }

        // claims 
        [Required]
        [MaxLength(100)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(100)]
        public string Lastname { get; set; }

        [Required]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "MM/dd/yyyy")]
        public DateTime Dob { get; set; }

        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        [Required]
        [MaxLength(30)]
        public string Country { get; set; }
        [Required]
        public string Role { get; set;}

        public string ReturnUrl { get; set; }

        public string Provider { get; set; }

        public string ProviderUserId { get; set; }

        public bool IsProvisioningFromExternal
        {
            get
            {
                return (Provider != null);
            }
        }


    }
}
