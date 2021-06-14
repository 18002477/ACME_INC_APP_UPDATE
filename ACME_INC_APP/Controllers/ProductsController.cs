using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ACME_INC_APP.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ACME_INC_APP.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ACMEContext _context;

        public ProductsController(ACMEContext context)
        {
            _context = context;
        }

        // GET: Products
        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {

            var product = from p in _context.Products select p;
            product = product.Include(s => s.ProdCat);
            if (!String.IsNullOrEmpty(searchString))
            {
                product = product.Include(s => s.ProdCat);
                product = product.Where(s => s.ProductName.Contains(searchString));
            }
            /*else if (String.IsNullOrEmpty(searchString))
            {
                ViewBag.Error = "No result found !";
            }*/
            return View(await product.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> AdminIndex(string searchString)
        {

            var product = from p in _context.Products select p;
            product = product.Include(s => s.ProdCat);
            if (!String.IsNullOrEmpty(searchString))
            {
                product = product.Include(s => s.ProdCat);
                product = product.Where(s => s.ProductName.Contains(searchString));
            }
            /*else if (String.IsNullOrEmpty(searchString))
            {
                ViewBag.Error = "No result found !";
            }*/
            return View(await product.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProdCat)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ProdCatId"] = new SelectList(_context.ProdCategories, "ProdCatId", "ProdCatName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int ProductID, string ProductName, string Description, int ProdCatId, decimal Price, IFormFile formFile)
        {
            Product product = new Product()
            {
                ProductId = ProductID,
                ProductName = ProductName,
                Description = Description,
                ProdCatId = ProdCatId,
                Price = Price
            };
            MemoryStream memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);
            product.ProductImage = memoryStream.ToArray();
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProdCatId"] = new SelectList(_context.ProdCategories, "ProdCatId", "ProdCatName", product.ProdCatId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int ProductId, string ProductName, string Description, int ProdCatId, decimal Price, IFormFile formFile)
        {
            Product product = new Product()
            {
                ProductId = ProductId,
                ProductName = ProductName,
                Description = Description,
                ProdCatId = ProdCatId,
                Price = Price
            };
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    MemoryStream memoryStream = new MemoryStream();
                    formFile.CopyTo(memoryStream);
                    product.ProductImage = memoryStream.ToArray();
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["ProdCatId"] = new SelectList(_context.ProdCategories, "ProdCatId", "ProdCatName", product.ProdCatId);
            return View(product);
        }


        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProdCat)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
