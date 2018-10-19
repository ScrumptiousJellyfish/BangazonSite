using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bangazon.Data;
using Bangazon.Models;
using Microsoft.AspNetCore.Identity;
using Bangazon.Models.OrderViewModels;

namespace Bangazon.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Order.Include(o => o.PaymentType).Include(o => o.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.PaymentType)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Cart
        public async Task<IActionResult> Cart()
        {

            var user = await GetCurrentUserAsync();

            var order = await _context.Order
                .Where(o => o.UserId == user.Id)
                .Where(o => o.PaymentTypeId == null)
                .Include(o => o.User)
                .FirstOrDefaultAsync();

           
            var ODVM = new OrderDetailViewModel();

            if (order == null)
            {
                return View(ODVM);
            }

            var orderProducts = _context.OrderProduct
                .Where(op => op.OrderId == order.OrderId)
                .Include(op => op.Product);
            var productCounts = orderProducts.Select(op => op.Product.Title)
                                                    .GroupBy(op => op)
                                                    .ToDictionary(x => x.Key, y => y.Count());
            ODVM.Order = order;
            ODVM.LineItems = orderProducts.Select(op => new OrderLineItem() { Product = op.Product, OrderProductId = op.OrderProductId, Cost = op.Product.Price * productCounts[op.Product.Title], Units = productCounts[op.Product.Title] });
            ODVM.LineItems = ODVM.LineItems.GroupBy(p => p.Product.Title).Select(g => g.First()).ToList();
            double total = 0.0;
            foreach (var item in ODVM.LineItems)
            {
                total += item.Product.Price * item.Units;
            }
            ODVM.Total = total;
            return View(ODVM);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentType, "PaymentTypeId", "AccountNumber");
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,DateCreated,DateCompleted,UserId,PaymentTypeId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentType, "PaymentTypeId", "AccountNumber", order.PaymentTypeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentType, "PaymentTypeId", "AccountNumber", order.PaymentTypeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,DateCreated,DateCompleted,UserId,PaymentTypeId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentType, "PaymentTypeId", "AccountNumber", order.PaymentTypeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var order = await _context.Order
                .Include(o => o.PaymentType)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);

           
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            var orderProducts = _context.OrderProduct
               .Where(op => op.OrderId == order.OrderId);
            foreach (var OP in orderProducts)
            {
                _context.OrderProduct.Remove(OP);
            }
            await _context.SaveChangesAsync();
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Cart));
        }

       
        
        public async Task<IActionResult> DeleteItem(int id)
        {
            var orderProduct = await _context.OrderProduct.FindAsync(id);
            _context.OrderProduct.Remove(orderProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Cart));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }


        // GET: Orders/Complete/5
        public async Task<IActionResult> Complete(int? id, double? Totes)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await GetCurrentUserAsync();
            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.OrderId == id);
            var paymentTypes = _context.PaymentType
                .Where(pt => user.Id == pt.UserId)
                .Select(pt => new SelectListItem() { Value = pt.PaymentTypeId.ToString(), Text = pt.Description}).ToList();

            if (order == null)
            {
                return NotFound();
            }
            var OCVM = new OrderCompleteViewModel
            {
                Order = order,
                PaymentTypes = paymentTypes

            };
            OCVM.Total = Totes;
            return View(OCVM);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id, [Bind("OrderId,DateCreated,DateCompleted,UserId,PaymentTypeId")] Order order)
        {
            order.DateCompleted = DateTime.Now;
            _context.Update(order);
            await _context.SaveChangesAsync();
                
            return RedirectToAction(nameof(Cart));
            
        }


    }
}
