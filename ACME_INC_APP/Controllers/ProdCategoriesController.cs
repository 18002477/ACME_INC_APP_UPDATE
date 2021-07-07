using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ACME_INC_APP.Models;
using Microsoft.AspNetCore.Http;

namespace ACME_INC_APP.Controllers
{
    public class ProdCategoriesController : Controller
    {
        private readonly ACMEContext _context;

        public ProdCategoriesController(ACMEContext context)
        {
            _context = context;
        }

        // GET: ProdCategories
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                return View(await _context.ProdCategories.ToListAsync());
            }
            else
            {
                TempData["LoginFirst"] = "You need to login first";
                return RedirectToAction("Login", "Login");
            }            
        }

        // GET: ProdCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prodCategory = await _context.ProdCategories
                .FirstOrDefaultAsync(m => m.ProdCatId == id);
            if (prodCategory == null)
            {
                return NotFound();
            }

            return View(prodCategory);
        }

        // GET: ProdCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProdCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProdCatId,ProdCatName")] ProdCategory prodCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prodCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(prodCategory);
        }

        // GET: ProdCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prodCategory = await _context.ProdCategories.FindAsync(id);
            if (prodCategory == null)
            {
                return NotFound();
            }
            return View(prodCategory);
        }

        // POST: ProdCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProdCatId,ProdCatName")] ProdCategory prodCategory)
        {
            if (id != prodCategory.ProdCatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prodCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdCategoryExists(prodCategory.ProdCatId))
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
            return View(prodCategory);
        }

        // GET: ProdCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prodCategory = await _context.ProdCategories
                .FirstOrDefaultAsync(m => m.ProdCatId == id);
            if (prodCategory == null)
            {
                return NotFound();
            }

            return View(prodCategory);
        }

        // POST: ProdCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prodCategory = await _context.ProdCategories.FindAsync(id);
            _context.ProdCategories.Remove(prodCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdCategoryExists(int id)
        {
            return _context.ProdCategories.Any(e => e.ProdCatId == id);
        }
    }
}
