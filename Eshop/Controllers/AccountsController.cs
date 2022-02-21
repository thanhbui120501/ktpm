using Eshop.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eshop.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Eshop.Controllers
{
    public class AccountsController : Controller
    {
        private readonly EshopContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AccountsController(EshopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (HttpContext.Session.Keys.Contains("Id"))
            {
                ViewBag.id = HttpContext.Session.GetInt32("Id");
            }
            else
            {
                ViewBag.ErrorMessage = "Sai tên đăng nhập hoặc mật khẩu";
            }
            if (id == null)
            {
                return NotFound();
            }

            if (HttpContext.Session.Keys.Contains("Username"))
            {
                ViewBag.username = HttpContext.Session.GetString("Username");
            }
            else
            {
                ViewBag.ErrorMessage = "Sai tên đăng nhập hoặc mật khẩu";
            }
            var account = await _context.Accounts.FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        public async Task<IActionResult> Edit(int? id)
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


            if (id == null)
            {
                return NotFound();
            }



            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Admin/Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,Email,Phone,Address,FullName,IsAdmin,Avatar,Status")] Account account)
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


            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }
        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }
    }
}
