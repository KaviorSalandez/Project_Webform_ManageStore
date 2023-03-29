using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using Newtonsoft.Json;
using System.Text.Json;

namespace WebApplication2.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Account account)
        {
            using (var context = new DbPrn211ProjectWebContext())
            {
                var ac = context.Accounts.FirstOrDefault(x => x.Email == account.Email && x.Pasword == account.Pasword);
                if(ac != null)
                {
                    if(ac.RoleId==1 || ac.RoleId == 2)
                    {
                        HttpContext.Session.SetString("userNow", System.Text.Json.JsonSerializer.Serialize(ac));
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.MessageLogin = "Bạn không đủ quyền để vào trang quản lí";
                        return View();
                    }
                }
                else
                {
                    ViewBag.MessageLogin= "Tên email hoặc password sai";
                    return View();
                }

            }
        }

        public IActionResult Signout()
        {
            if ((HttpContext.Session.GetString("userNow") != null) || string.IsNullOrEmpty(HttpContext.Session.GetString("userNow")))
            {
                HttpContext.Session.Remove("userNow");
            }
            return RedirectToAction("Login", "Account");
        }



    }
}
