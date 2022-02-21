using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eshop.Areas.Admin.Models;
using Eshop.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly EshopContext _context;
        
        public HomeController(EshopContext context)
        {
            _context = context;          
        }
        public IActionResult Index()
        {
            int sl = _context.Carts.Include(prop => prop.Product).Where(prop => prop.Account.Username == HttpContext.Session.GetString("Username")).Count();
            ViewBag.soluong = sl;
            if (HttpContext.Session.Keys.Contains("Id"))
            {
                ViewBag.id = HttpContext.Session.GetInt32("Id");
            }
            else
            {
                ViewBag.ErrorMessage = "Sai tên đăng nhập hoặc mật khẩu";
            }
            if (HttpContext.Session.Keys.Contains("Username"))
            {
                ViewBag.username = HttpContext.Session.GetString("Username");
            }
            else
            {
                ViewBag.ErrorMessage = "Sai tên đăng nhập hoặc mật khẩu";
            }
            var product = _context.Products.ToList();
            return View(product);
        }
        public IActionResult Deatail()
        {
            
            return View();
        }

        public IActionResult Login(string username, string password)
        {
            ViewBag.isLogin = true;
            Account user = _context.Accounts.Where(i => i.Username == username && i.Password == password).FirstOrDefault();
            if (user != null)
            {
                //////// tạo cookie
                //HttpContext.Response.Cookies.Append("Id", emp.Id.ToString());
                //HttpContext.Response.Cookies.Append("Username", emp.Username.ToString());
                //return RedirectToAction("Index", "Admin");


                //tạo session
                HttpContext.Session.SetInt32("Id", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                ViewBag.success_Login_Message = "Đăng nhập thành công";
                return RedirectToAction("Index", "Home");
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
            return RedirectToAction("Index", "Home");
        }
    }
}
