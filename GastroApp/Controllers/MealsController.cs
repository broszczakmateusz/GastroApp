using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GastroApp.Data;
using GastroApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace GastroApp.Controllers
{
    public class MealsController : Controller
    {
        private readonly GastroAppContext _context;

        public MealsController(GastroAppContext context)
        {
            _context = context;
        }

        // GET: Meals
        [Authorize(Roles = "Admin, RestaurantManager, WaiterManager, Waiter, Chief")]
        public async Task<IActionResult> SelectIndex(int? categoryId, int? orderId)
        {
            if (categoryId == null)
            {
                return Problem("Selected category is null");
            }
            if (categoryId == null)
            {
                return Problem("Selected order is null");
            }
            ViewData["OrderId"] = orderId;
            var gastroAppContext = _context.Meals.Include(m => m.Category).Where(m => m.CategoryId == categoryId);
            return View(await gastroAppContext.ToListAsync());
        }

        // GET: Meals/Select/5
        [Authorize(Roles = "Admin, RestaurantManager, WaiterManager, Waiter, Chief")]
        public async Task<IActionResult> Select(int? id, int? orderId)
        {
            if (id == null || _context.Meals == null)
            {
                return NotFound();
            }

            var meal = await _context.Meals
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meal == null)
            {
                return NotFound();
            }
            return RedirectToAction("Create", "OrderedMeals", new { orderId = orderId, mealId = id });

            //return RedirectToAction("CreateOrderedMeal", "SelectedOrder", new { id = orderId, mealId = id });
        }
        // GET: Meals
        public async Task<IActionResult> Index()
        {
            var gastroAppContext = _context.Meals.Include(m => m.Category);
            return View(await gastroAppContext.ToListAsync());
        }

        // GET: Meals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Meals == null)
            {
                return NotFound();
            }

            var meal = await _context.Meals
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meal == null)
            {
                return NotFound();
            }

            return View(meal);
        }

        // GET: Meals/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Meals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,VATRate,CategoryId")] Meal meal)
        {
            var category = await _context.Categories.FindAsync(meal.CategoryId);
            if (category == null)
            {
                return Problem("Category is null.");
            }
            meal.Category = category;
            ModelState.Clear();
            TryValidateModel(meal);

            if (ModelState.IsValid)
            {
                _context.Add(meal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", meal.CategoryId);
            return View(meal);
        }

        // GET: Meals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Meals == null)
            {
                return NotFound();
            }

            var meal = await _context.Meals.FindAsync(id);
            if (meal == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", meal.CategoryId);
            return View(meal);
        }

        // POST: Meals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,VATRate,CategoryId")] Meal meal)
        {
            if (id != meal.Id)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(meal.CategoryId);
            if (category == null)
            {
                return Problem("Category is null.");
            }
            meal.Category = category;
            ModelState.Clear();
            TryValidateModel(meal);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MealExists(meal.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", meal.CategoryId);
            return View(meal);
        }

        // GET: Meals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Meals == null)
            {
                return NotFound();
            }

            var meal = await _context.Meals
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meal == null)
            {
                return NotFound();
            }

            return View(meal);
        }

        // POST: Meals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Meals == null)
            {
                return Problem("Entity set 'GastroAppContext.Meals'  is null.");
            }
            var meal = await _context.Meals.FindAsync(id);
            if (meal != null)
            {
                _context.Meals.Remove(meal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MealExists(int id)
        {
            return (_context.Meals?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
