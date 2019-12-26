using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataSummitOAuthServer
{
    public class DataSummitUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string PositionTitle { get; set; }
        [Required]
        public bool IsTrial { get; set; }
        [Required]
        public int GenderId { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
