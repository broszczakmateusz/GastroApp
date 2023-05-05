using GastroApp.Data;
using GastroApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GastroApp.Controllers
{
    [Authorize(Roles = "Admin, RestaurantManager, WaiterManager, Waiter")]
    public class SelectedOrderController : Controller
    {
        private readonly GastroAppContext _context;

        public SelectedOrderController(GastroAppContext context)
        {
            _context = context;
        }

        // GET: SelectedOrder/SelectedOrder/5
        public async Task<IActionResult> SelectedOrder(int? id)
        {
            var order = await _context.Orders
            .Include(o => o.Table)
                .ThenInclude(t => t.Room)
            .Include(o => o.Meals)
                .ThenInclude(m => m.Category)
            .Include(o => o.OrderedMeals)
                .ThenInclude(m => m.Meal.Category)
            .Include(o => o.User)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        public IActionResult RedirectToOrdersIndex()
        {
            return RedirectToAction("Index", "Orders");
        }

        // GET: Categories/5
        public IActionResult SelectCategory(int? id)
        {
            if (id == null)
            {
                return Problem("Selected orderId is null");
            }
            return RedirectToAction("SelectIndex", "Categories", new { orderId = id });
        }

        public async Task<IActionResult> UpdateOrderAfterOrderedMealsChange(int? id)
        {
            var order = await _context.Orders
            .Include(o => o.Table)
                .ThenInclude(t => t.Room)
            .Include(o => o.Meals)
                .ThenInclude(m => m.Category)
            .Include(o => o.OrderedMeals)
                .ThenInclude(m => m.Meal.Category)
            .Include(o => o.User)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            order.UpdateTotalPrice();

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
                return RedirectToAction("SelectedOrder", "SelectedOrder", new { id = order.Id });
            }
            return RedirectToAction("Index", "Orders");
        }

        // GET: SelectedOrder/CloseOrder/5
        public async Task<IActionResult> CloseOrder(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
            .Include(o => o.Table)
                .ThenInclude(t => t.Room)
            .Include(o => o.Meals)
                .ThenInclude(m => m.Category)
            .Include(o => o.OrderedMeals)
                .ThenInclude(m => m.Meal.Category)
            .Include(o => o.User)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "Id", "Name", order.PaymentMethodId);
            return View(order);
        }

        // POST: SelectedOrder/CloseOrder/5
        [HttpPost, ActionName("CloseOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseOrderConfirmed(int? id, int? paymentMethodId)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }
            if (paymentMethodId == null || _context.PaymentMethods == null)
            {
                return NotFound();
            }
            var order = await _context.Orders
            .Include(o => o.Table)
                .ThenInclude(t => t.Room)
            .Include(o => o.Meals)
                .ThenInclude(m => m.Category)
            .Include(o => o.OrderedMeals)
                .ThenInclude(m => m.Meal.Category)
            .Include(o => o.User)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            PaymentMethod? paymentMethod = await _context.PaymentMethods.FindAsync(paymentMethodId);
            if (paymentMethod == null)
            {
                return Problem("Payment method is null.");
            }
            order.SetAsPaid(paymentMethod);

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
                return RedirectToAction("Index", "Orders");
            }
            return View(order);
        }

        // GET: SelectedOrder/ChangeTable/5
        public async Task<IActionResult> ChangeTable(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
            .Include(o => o.Table)
                .ThenInclude(t => t.Room)
            .Include(o => o.Meals)
                .ThenInclude(m => m.Category)
            .Include(o => o.OrderedMeals)
                .ThenInclude(m => m.Meal.Category)
            .Include(o => o.User)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }
            var Tables = await _context.Tables.Include(t => t.Room).ToListAsync();
            ViewData["TableId"] = new SelectList(Tables, "Id", "RoomAndTable", order.TableId);
            return View(order);
        }

        // POST: SelectedOrder/ChangeTable/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeTable(int? id, int? tableId)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }
            if (tableId == null || _context.Tables == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
            .Include(o => o.Table)
                .ThenInclude(t => t.Room)
            .Include(o => o.Meals)
                .ThenInclude(m => m.Category)
            .Include(o => o.OrderedMeals)
                .ThenInclude(m => m.Meal.Category)
            .Include(o => o.User)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }
            order.TableId = tableId.Value;
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
                return RedirectToAction("Index", "Orders");
            }
            var Tables = await _context.Tables.Include(t => t.Room).ToListAsync();
            ViewData["TableId"] = new SelectList(Tables, "Id", "RoomAndTable", order.TableId);
            return View(order);
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
