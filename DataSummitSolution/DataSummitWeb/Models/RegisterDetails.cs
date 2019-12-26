using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitWeb.Models
{
    public class RegisterDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string TownCity { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }
        public string DoB { get; set; }
        public string Company { get; set; }
        public string JobRole { get; set; }
        public PhoneCode TelCode { get; set; }
        public bool IsTrial = true;
        public byte GenderId = 1;

        public RegisterDetails()
        {

        }
    }
}
