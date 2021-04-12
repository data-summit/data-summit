using DataSummitWeb.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DataSummitWeb.Interfaces
{
    interface IAuthentication
    {
        Task<IActionResult> Register(DataSummitUser authString);
        string Login(string loginString);
    }
}
