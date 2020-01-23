using DataSummitHelper;
using DataSummitModels;
using DataSummitWeb.Models;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using DataSummitUser = DataSummitHelper.DataSummitUser;

namespace DataSummitWeb.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        //Connection string determined by Startup.IEnvironment and used privately in dbContext
        Users usersService = new Users(new DataSummitDbContext());

        private DataSummitDbContext db = new DataSummitDbContext();
        [HttpGet("{id}")]
        public string Get(int id)
        {
            List<AspNetUsers> users = usersService.GetAllCompanyUsers(id);
            return JsonConvert.SerializeObject(users.ToArray());
        }

        //[HttpPost, RequireHttps, Route("register")]
        [HttpPost, Route("register")]
        public async void Post([FromBody]RegisterDetails regUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Extract country from body
                    // What is the point in this ToString cast when Phonecode could be the same type...why have string vs int?!
                    var country = (db.Countries.First(c => c.Phonecode.ToString() == regUser.TelCode.Phonecode));
                    var countryCode = $"00{country.Phonecode}";

                    var phoneNumber = (regUser.Phone.Substring(1, 1) == "0") 
                        ? countryCode + regUser.Phone.Substring(1, regUser.Phone.Length - 1)
                        : countryCode + regUser.Phone;

                    //Extract address from body
                    var address = new HashSet<DataSummitModels.Addresses>()
                    {
                        new DataSummitModels.Addresses
                        {
                            Street = regUser.Street,
                            Street2 = regUser.Street2,
                            TownCity = regUser.TownCity,
                            County = regUser.County,
                            PostCode = regUser.PostCode,
                            CompanyId = 1,
                            CreatedDate = DateTime.Now,
                            CountryId = 1
                        }
                    };

                    //Extract DataSummitUser from body
                    var dataSummitUser = new DataSummitUser
                    {
                        UserName = regUser.Email,
                        Email = regUser.Email,
                        PhoneNumber = phoneNumber,
                        FirstName = regUser.FirstName,
                        Surname = regUser.LastName,
                        PositionTitle = regUser.JobRole,
                        IsTrial = regUser.IsTrial,
                        GenderId = regUser.GenderId,
                        CreatedDate = DateTime.Now,
                        DateOfBirth = DateTime.TryParse(regUser.DoB, out DateTime dateOfBirth) ? dateOfBirth : new DateTime()
                    };

                    //Extract user from body
                    AspNetUsers user = new AspNetUsers
                    {
                        FirstName = regUser.FirstName,
                        Surname = regUser.LastName,
                        PositionTitle = regUser.JobRole,
                        IsTrial = regUser.IsTrial,
                        GenderId = regUser.GenderId,
                        CreatedDate = DateTime.Now,
                        //Address = address,
                        DateOfBirth = DateTime.TryParse(regUser.DoB, out dateOfBirth) ? dateOfBirth : new DateTime()
                    };
                    
                    await usersService.CreateUsers(user, dataSummitUser, regUser.Password);
                }
                catch(Exception ae)
                {
                    string strMessage = ae.Message.ToString();
                    string strInner = ae.InnerException?.ToString();
                }
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]AspNetUsers user)
        {
            //Update
            usersService.UpdateUsers(id, user);
        }
    }
}
