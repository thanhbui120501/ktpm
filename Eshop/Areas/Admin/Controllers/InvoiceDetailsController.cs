﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshop.Areas.Admin.Models;
using Eshop.Data;

namespace Eshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InvoiceDetailsController : Controller
    {
        private readonly EshopContext _context;

        public InvoiceDetailsController(EshopContext context)
        {
            _context = context;
        }

        // GET: Admin/InvoiceDetails
        public async Task<IActionResult> Index()
        {
            var eshopContext = _context.InvoiceDetails.Include(i => i.Invoice).Include(i => i.Product);
            return View(await eshopContext.ToListAsync());
        }

        // GET: Admin/InvoiceDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceDetail = await _context.InvoiceDetails
                .Include(i => i.Invoice)
                .Include(i => i.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoiceDetail == null)
            {
                return NotFound();
            }

            return View(invoiceDetail);
        }

        // GET: Admin/InvoiceDetails/Create
        public IActionResult Create()
        {
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // POST: Admin/InvoiceDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InvoiceId,ProductId,Quantity,UnitPrice")] InvoiceDetail invoiceDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoiceDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "Id", invoiceDetail.InvoiceId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", invoiceDetail.ProductId);
            return View(invoiceDetail);
        }

        // GET: Admin/InvoiceDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceDetail = await _context.InvoiceDetails.FindAsync(id);
            if (invoiceDetail == null)
            {
                return NotFound();
            }
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "Id", invoiceDetail.InvoiceId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", invoiceDetail.ProductId);
            return View(invoiceDetail);
        }

        // POST: Admin/InvoiceDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InvoiceId,ProductId,Quantity,UnitPrice")] InvoiceDetail invoiceDetail)
        {
            if (id != invoiceDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoiceDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceDetailExists(invoiceDetail.Id))
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
            ViewData["InvoiceId"] = new SelectList(_context.Invoices, "Id", "Id", invoiceDetail.InvoiceId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", invoiceDetail.ProductId);
            return View(invoiceDetail);
        }

        // GET: Admin/InvoiceDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceDetail = await _context.InvoiceDetails
                .Include(i => i.Invoice)
                .Include(i => i.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoiceDetail == null)
            {
                return NotFound();
            }

            return View(invoiceDetail);
        }

        // POST: Admin/InvoiceDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoiceDetail = await _context.InvoiceDetails.FindAsync(id);
            _context.InvoiceDetails.Remove(invoiceDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceDetailExists(int id)
        {
            return _context.InvoiceDetails.Any(e => e.Id == id);
        }
    }
}
