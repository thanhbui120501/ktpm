using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Eshop.Data;
using Eshop.Areas.Admin.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Controllers
{
    public class CartController : Controller
    {
        private readonly EshopContext _context;
        public CartController(EshopContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Cart()
        {
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
            int sl = _context.Carts.Include(prop => prop.Product).Where(prop => prop.Account.Username == HttpContext.Session.GetString("Username")).Count();
            ViewBag.soluong = sl;

            var cart = _context.Carts.Include(prop=>prop.Product).Where(prop=>prop.Account.Username == HttpContext.Session.GetString("Username"));          
            return View(await cart.ToListAsync());
        }      
        public IActionResult AddToCart(int id)
        {
            return AddToCart(id, 1);
        }
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            string username = HttpContext.Session.GetString("Username");
            int accountId = _context.Accounts.FirstOrDefault(a => a.Username == username).Id;
            Cart cart = _context.Carts.FirstOrDefault(c => c.AccountId == accountId && c.ProductId == productId);
                if (cart == null)
                {
                    cart = new Cart();
                    cart.AccountId = accountId;
                    cart.ProductId = productId;
                    cart.Quantity = quantity;

                    _context.Carts.Add(cart);
                }
                else
                {
                    cart.Quantity += quantity;

                }
                _context.SaveChanges();
                return RedirectToAction("Cart", "Cart");
        }
        
    }
}
