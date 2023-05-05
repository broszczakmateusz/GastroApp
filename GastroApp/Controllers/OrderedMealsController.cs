using GastroApp.Data;
using GastroApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GastroApp.Controllers
{
    public class OrderedMealsController : Controller
    {
        private readonly GastroAppContext _context;

        public OrderedMealsController(GastroAppContext context)
        {
            _context = context;
        }
        // GET: OrderedMeals/Create
        public async Task<IActionResult> Create(int? orderId, int? mealId)
        {
            if (orderId == null || _context.Orders == null)
            {
                return NotFound();
            }
            if (mealId == null || _context.Meals == null)
            {
                return NotFound();
            }

            OrderedMeal orderedMeal = new();
            orderedMeal.OrderId = orderId.Value;

            orderedMeal.MealId = mealId.Value;
            var meal = await _context.Meals
             .Include(m => m.Category)
             .FirstOrDefaultAsync(m => m.Id == orderedMeal.MealId);
            if (meal == null)
            {
                return NotFound();
            }
            orderedMeal.Meal = meal;

            return View(orderedMeal);
        }
        // POST: OrderedMeals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, RestaurantManager, WaiterManager, Waiter")]
        public async Task<IActionResult> Create([Bind("Id,MealId,OrderId,Annotation")] OrderedMeal orderedMeal)
        {
            if (orderedMeal == null)
            {
                return Problem("OrderedMeal is null.");
            }
            var order = await _context.Orders
            .Include(o => o.Table)
                .ThenInclude(t => t.Room)
            .Include(o => o.Meals)
                .ThenInclude(m => m.Category)
            .Include(o => o.OrderedMeals)
                .ThenInclude(m => m.Meal.Category)
            .Include(o => o.User)
            .FirstOrDefaultAsync(m => m.Id == orderedMeal.OrderId);
            if (order == null)
            {
                return NotFound();
            }
            var meal = await _context.Meals
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == orderedMeal.MealId);
            if (meal == null)
            {
                return NotFound();
            }
            orderedMeal.Order = order;
            orderedMeal.Meal = meal;

            ModelState.Clear();
            TryValidateModel(orderedMeal);

            if (ModelState.IsValid)
            {
                _context.Add(orderedMeal);
                _context.SaveChanges();
                return RedirectToAction("UpdateOrderAfterOrderedMealsChange", "SelectedOrder", new { id = orderedMeal.OrderId });
            }

            return Problem("Create OrderedMeal was failed.");
        }
        [Authorize(Roles = "Admin, RestaurantManager, WaiterManager")]
        public async Task<IActionResult> RemoveFromOrder(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }
            var orderedMeal = await _context.OrderedMeals
            .Include(om => om.Meal.Category)
            .Include(om => om.Order)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (orderedMeal == null)
            {
                return Problem("OrderedMeal is null.");
            }
            return View(orderedMeal);
        }

        [HttpPost, ActionName("RemoveFromOrder")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, RestaurantManager, WaiterManager")]
        public async Task<IActionResult> RemoveFromOrderConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var orderedMeal = await _context.OrderedMeals
            .Include(om => om.Meal.Category)
            .Include(om => om.Order)
            .FirstAsync(m => m.Id == id);
            if (orderedMeal == null)
            {
                return Problem("OrderedMeal is null.");
            }

            var orderId = orderedMeal.OrderId;

            if (ModelState.IsValid)
            {
                _context.Remove(orderedMeal);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("UpdateOrderAfterOrderedMealsChange", "SelectedOrder", new { id = orderId });
        }
    }
}
