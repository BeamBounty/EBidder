using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace EBidderWeb.Server.Models
{
    public partial class ApplicationUser : IdentityUser
    {
        [JsonIgnore, IgnoreDataMember]
        public override string PasswordHash { get; set; }

        public string Password { get; set; }

        public string SSN { get; set; }

        public string Address { get; set; }

        public string ConfirmPassword { get; set; }

        [JsonIgnore, IgnoreDataMember, NotMapped]
        public string Name
        {
            get
            {
                return UserName;
            }
            set
            {
                UserName = value;
            }
        }

        public ICollection<ApplicationRole> Roles { get; set; }
    }
}