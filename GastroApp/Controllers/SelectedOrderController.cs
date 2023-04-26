using GastroApp.Data;
using GastroApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GastroApp.Controllers
{
    public class SelectedOrderController : Controller
    {
        private readonly GastroAppContext _context;

        public SelectedOrderController(GastroAppContext context)
        {
            _context = context;
        }

        // GET: SelectedOrder/SelectedOrder/5
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
                .Include(o => o.OrderedMeals)
                .Include(o => o.Meals)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [Authorize]
        public IActionResult RedirectToOrdersIndex()
        {
            return RedirectToAction("Index", "Orders");
        }

        // GET: Categories/5
        [Authorize]
        public IActionResult SelectCategory(int? id)
        {
            if (id == null)
            {
                return Problem("Selected orderId is null");
            }
            return RedirectToAction("Index", "Categories", new { orderId = id });
        }

        [Authorize]
        public async Task<IActionResult> AddOrderedMealToOrder(int? id, int? mealId)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Table)
                    .ThenInclude(t => t.Room)
                .Include(o => o.User)
                .Include(o => o.OrderedMeals)
                .Include(o => o.Meals)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            if (mealId == null)
            {
                return Problem("MealId is null.");
            }
            var meal = await _context.Meals
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == mealId);
            if (meal == null)
            {
                return Problem("Meal is null.");
            }

            OrderedMeal orderedMeal = new(id.Value, order, mealId.Value, meal);
            _context.Add(orderedMeal);
            await _context.SaveChangesAsync();

            order.UpdatedDateTime = orderedMeal.CreatedDateTime;
            order.TotalPrice = order.Meals.Sum(m => m.Price);

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
                return RedirectToAction("SelectedOrder", "SelectedOrder", new { id = id });
            }
            return RedirectToAction("Index", "Orders");
        }

        // GET: SelectedOrder/CloseOrder/5
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
            .Include(o => o.Meals)
            .Include(o => o.OrderedMeals)
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
                return RedirectToAction("Index", "Orders");
            }
            ViewData["PaymentMethod"] = new SelectList(_context.PaymentMethods, "Id", "Name", order.PaymentMethodId);
            return View(order);
        }


        // GET: SelectedOrder/ChangeTable/5
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
