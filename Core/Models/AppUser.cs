﻿using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;


namespace Core.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + SecondName;
            }
        }


        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        [Display(AutoGenerateField = false)]
        public Guid UserSettingsId { get; set; }

        public override string ToString()
        {
            return UserName;
        }
    }
}