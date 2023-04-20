using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GastroApp.Data;
using GastroApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using GastroApp.Migrations;

namespace GastroApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly GastroAppContext _context;

        public OrdersController(GastroAppContext context)
        {
            _context = context;
        }

        // GET: Orders
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var gastroAppContext = _context.Orders.Where(o => o.IsPaid == false)
                .Include(o => o.Table)
                    .ThenInclude(t => t.Room)
                .Include(o => o.User);
            return View(await gastroAppContext.ToListAsync());
        }

        // GET: Orders/SelectedOrder/5
        [Authorize]
        public async Task<IActionResult> SelectedOrder(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Table)
                    .ThenInclude(t => t.Room)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        [Authorize]
        private Order? GetDataForNewOrder(Order order)
        {


            return order;
        }

        // GET: Orders/Create
        [Authorize]
        public IActionResult Create()
        {
            var Tables = _context.Tables.Include(t => t.Room).ToList();
            ViewData["TableId"] = new SelectList(Tables, "Id", "RoomAndTable");

            return View();
        }


        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,TableId")] Order order)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Problem("User is null.");
            }
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return Problem("User is null.");
            }

            var table = _context.Tables.Include(t => t.Room).FirstOrDefault(t => t.Id == order.TableId);
            if (table == null)
            {
                return Problem("Table is null.");
            }
            order.SetUserANdTableForNew(user, table);

            ModelState.Clear();
            TryValidateModel(order);

            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(SelectedOrder), new { Id = order.Id });
            }
            var Tables = _context.Tables.Include(t => t.Room).ToList();
            ViewData["TableId"] = new SelectList(Tables, "Id", "RoomAndTable", order.TableId);
            return View(order);
        }

        // GET: Orders/ChangeTable/5
        [Authorize]
        public async Task<IActionResult> ChangeTable(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
            .Include(o => o.Table)
                .ThenInclude(t => t.Room)
            .Include(o => o.User)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }
            var Tables = _context.Tables.Include(t => t.Room).ToList();
            ViewData["TableId"] = new SelectList(Tables, "Id", "RoomAndTable", order.TableId);
            return View(order);
        }

        // POST: Orders/ChangeTable/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangeTable(int id, Order updatedOrder)
        {
            if (id != updatedOrder.Id)
            {
                return NotFound();
            }

            var order = await _context.Orders
            .Include(o => o.Table)
                .ThenInclude(t => t.Room)
            .Include(o => o.User)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }
            order.TableId = updatedOrder.TableId;
            ModelState.Clear();
            TryValidateModel(order);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            var Tables = _context.Tables.Include(t => t.Room).ToList();
            ViewData["TableId"] = new SelectList(Tables, "Id", "RoomAndTable", order.TableId);
            return View(order);
        }

        // GET: Orders/CloseOrder/5
        [Authorize]
        public async Task<IActionResult> CloseOrder(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
            .Include(o => o.Table)
                .ThenInclude(t => t.Room)
            .Include(o => o.User)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "Id", "Name", order.PaymentMethodId);
            return View(order);
        }


        // POST: Orders/CloseOrder/5
        [HttpPost, ActionName("CloseOrder")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CloseOrderConfirmed(int id, Order selectedOrder)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'GastroAppContext.Orders'  is null.");
            }

            var paymentMethodId = selectedOrder.PaymentMethodId;
            if (paymentMethodId == null)
            {
                return Problem("Payment method is null.");
            }
            PaymentMethod? paymentMethod = _context.PaymentMethods.Find(paymentMethodId);
            if (paymentMethod == null)
            {
                return Problem("Payment method is null."); 
            }

            var order = await _context.Orders
            .Include(o => o.Table)
                .ThenInclude(t => t.Room)
            .Include(o => o.User)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (order != null)
            {
                try
                {
                    order.SetAsPaid(paymentMethod);

                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["PaymentMethod"] = new SelectList(_context.PaymentMethods, "Id", "Name", order.PaymentMethodId);
            return View(order);
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
