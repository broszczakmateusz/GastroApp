using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GastroApp.Data;
using GastroApp.Models;

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
        public async Task<IActionResult> Index(int? categoryId, int? orderId)
        {
            if (categoryId == null)
            {
                return Problem("Selected category is null");
            }
            ViewData["OrderId"] = orderId;
            var gastroAppContext = _context.Meals.Include(m => m.Category).Where(m => m.CategoryId == categoryId);
            return View(await gastroAppContext.ToListAsync());
        }

        // GET: Meals/Select/5
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
            return RedirectToAction("AddOrderedMealToOrder", "Orders", new { id = orderId, mealId = id });
        }
    }
}
