using DataSummitModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitWeb.Models
{
    public class RegisteredUser
    {
        public AspNetUsers AspNetUser { get; set; }
        public User User { get; set; }

        public RegisteredUser()
        { }

        //public AspNetUser GetAspNetUser()
        //{
        //    AspNetUser netUser = new AspNetUser();
        //    try
        //    {
        //        netUser.Id = this.AspNetUser.Id;
        //        netUser.AccessFailedCount = 0;
        //        netUser.Email = this.AspNetUser.Email;
        //        netUser.EmailConfirmed = false;
        //        netUser.LockoutEnabled = false;
        //        netUser.PhoneNumber = this.AspNetUser.PhoneNumber;
        //        netUser.PhoneNumberConfirmed = false;
        //        netUser.UserName = this.AspNetUser.UserName;

        //        //Dual factor authentication process to be implemented
        //        //Needs to be set to true once octo system is ready
        //        //***---(Snoop to advise)---***//
        //        netUser.TwoFactorEnabled = false;
        //    }
        //    catch (Exception ae)
        //    { string strError = ae.Message.ToString(); }
        //    return netUser;
        //}
        //public User GetDBUser()
        //{
        //    User user = new User();
        //    try
        //    {
        //        user.PositionTitle = this.User.PositionTitle;
        //        user.FirstName = this.User.FirstName;
        //        user.MiddleNames = this.User.MiddleNames;
        //        user.Surname = this.User.Surname;
        //        user.Title = this.User.Title;
        //        user.GenderId = this.User.GenderId;
        //        user.CreatedDate = DateTime.Now;
        //        user.Id = this.AspNetUser.Id;

        //        //UserTypeId to be set as a customer until
        //        //additional functionality is required to differentiate
        //        //the different user types
        //        user.UserTypeId = 2;    //2 = Customer

        //        //Photo attributes can only be added from a user's
        //        //settings page. Funtionality currently does not exist

        //    }
        //    catch (Exception ae)
        //    { string strError = ae.Message.ToString(); }
        //    return user;
        //}
    }
}
