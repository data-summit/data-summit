using DataSummitWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSummitWeb.Interfaces
{
    interface IAuthentication
    {
        Task<IActionResult> Register(DataSummitUser authString);
        string Login(string loginString);
    }
}
