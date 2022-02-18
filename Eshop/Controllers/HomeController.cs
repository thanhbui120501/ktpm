using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eshop.Areas.Admin.Models;
using Eshop.Data;
using Microsoft.AspNetCore.Hosting;

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
            var product = _context.Products.ToList();
            return View(product);
        }
    }
}
