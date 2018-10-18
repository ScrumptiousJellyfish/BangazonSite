using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bangazon.Data;
using Bangazon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Bangazon.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context,
                                  UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Stores private reference to Identity Framework user manager
        private readonly UserManager<ApplicationUser> _userManager;

        // This task retrieves the currently authenticated user
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Product.Include(p => p.ProductType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // queries by ProductId and returns all fields for the Product that was clicked
            var product = await _context.Product
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            // selects the OrderProducts table and matches the current ProductId with the ProductId in OrderProducts. Gets a distinc count of a ProductId
            int ordersPlaced = _context.OrderProduct.Where(op => op.ProductId == product.ProductId).Count();

            // Subtracts the original Product Quantity from the total ordersPlaced.
            product.Quantity -= ordersPlaced;
           
            return View(product);
        }

        // GET: Products/Create

        [Authorize]
        public async Task<IActionResult> Create()
        {
            // Get the current user
            var user = await GetCurrentUserAsync();

            ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "ProductTypeId", "Label");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,DateCreated,Description,Title,Price,Quantity,ProductTypeId")] Product product)
        {
            // Remove the user from the model validation because it is
            // not information posted in the form
            ModelState.Remove("User");
            ModelState.Remove("UserId");


            if (ModelState.IsValid)
            {
                // Get the current user
                var user = await GetCurrentUserAsync();

                product.UserId = user.Id;

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = product.ProductId});
            }

            ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "ProductTypeId", "Label", product.ProductTypeId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "ProductTypeId", "Label", product.ProductTypeId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,DateCreated,Description,Title,Price,Quantity,ProductTypeId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "ProductTypeId", "Label", product.ProductTypeId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProductType)
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
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
