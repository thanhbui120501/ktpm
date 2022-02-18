using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eshop.Areas.Admin.Models;
using Eshop.Data;
using Microsoft.AspNetCore.Http;

namespace Eshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly EshopContext _context;
        public AdminController(EshopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains("Username"))
            {
                ViewBag.EmployeeUsername = HttpContext.Session.GetString("Username");
            }
            else
            {
                ViewBag.ErrorMessage = "Sai tên đăng nhập hoặc mật khẩu";
            }
            return View();
        }
        public IActionResult Dashboard()
        {          
            return View();
        }
        public IActionResult Login(string username, string password)
        {
            ViewBag.isLogin = true;
            Account emp = _context.Accounts.Where(i => i.Username == username && i.Password == password).FirstOrDefault();
            if (emp != null)
            {
                //////// tạo cookie
                //HttpContext.Response.Cookies.Append("Id", emp.Id.ToString());
                //HttpContext.Response.Cookies.Append("Username", emp.Username.ToString());
                //return RedirectToAction("Index", "Admin");


                //tạo session
                HttpContext.Session.SetInt32("Id", emp.Id);
                HttpContext.Session.SetString("Username", emp.Username);
                ViewBag.success_Login_Message = "Đăng nhập thành công";
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.failed_Login_Message = "Đăng nhập thất bại";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Admin");
        }  
       
    }
}
