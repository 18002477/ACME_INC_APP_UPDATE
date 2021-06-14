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
    public class CartsController : Controller
    {
        private readonly ACMEContext _context;

        public CartsController(ACMEContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var aCMEContext = _context.Carts.Include(c => c.Product).Include(c => c.User);
            return View(await aCMEContext.ToListAsync());
        }

        /* public IActionResult Index()
         {
             if (HttpContext.Session.GetString("LoggedInUser") != null)
             {
                 string username = HttpContext.Session.GetString("LoggedInUser");
                 return View(_context.Carts.Where(b => b.User.Equals(username)).ToList());
             }
             else
             {
                 TempData["LoginFirst"] = "You need to login first";
                 return RedirectToAction("Login", "Login");
             }
         }*/


        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Product)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id)
        {
            int? currentUser = HttpContext.Session.GetInt32("UserID");
            //var productID = HttpContext.Session.GetInt32("ProductID");

            if (HttpContext.Session.GetString("LoggedInUser") != null)
            {
                ViewBag.User = HttpContext.Session.GetString("LoggedInUser");

                Cart cart = new Cart()
                {
                    UserId = (int)currentUser,
                    ProductId = (int)id
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["LoginFirst"] = "Please login to add the product to your Cart";
                return RedirectToAction("Login", "Login");
            }
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*       [HttpPost]
               [ValidateAntiForgeryToken]
               public async Task<IActionResult> Create([Bind("CartId,UserId,ProductId")] Cart cart)
               {
                   if (ModelState.IsValid)
                   {
                       _context.Add(cart);
                       await _context.SaveChangesAsync();
                       return RedirectToAction(nameof(Index));
                   }
                   ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Description", cart.ProductId);
                   ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", cart.UserId);
                   return View(cart);
               }*/

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Description", cart.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", cart.UserId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartId,UserId,ProductId")] Cart cart)
        {
            if (id != cart.CartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.CartId))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Description", cart.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", cart.UserId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Product)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.CartId == id);
        }

    }
}
