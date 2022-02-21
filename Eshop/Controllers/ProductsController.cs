using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Eshop.Areas.Admin.Models;
using Eshop.Data;


namespace Eshop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly EshopContext _context;

        public ProductsController(EshopContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Detail(int? id)
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
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }
    }
}
