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
    public class CategoriesController : Controller
    {
        private readonly GastroAppContext _context;

        public CategoriesController(GastroAppContext context)
        {
            _context = context;
        }

        // GET: Categories
        [Authorize(Roles = "Admin, RestaurantManager, WaiterManager, Waiter, Chief")]
        public async Task<IActionResult> SelectIndex(int? orderId)
        {
            if (orderId == null)
            {
                Problem("Entity set 'GastroAppContext.Categories'  is null.");
            }
            
            ViewData["OrderId"] = orderId;
            return View(await _context.Categories.ToListAsync());
        }

        [Authorize(Roles = "Admin, RestaurantManager, WaiterManager, Waiter")]
        public IActionResult Select(int? id, int? orderId)
        {   
            if (id == null)
            {
                return Problem("Selected categoryId is null");
            }
            if (orderId == null)
            {
                return Problem("Selected orderId is null");
            }
            return RedirectToAction("Index", "Meals", new { categoryId = id, orderId = orderId});
        }

        // GET: Categories
        [Authorize(Roles = "Admin, RestaurantManager, Chief")]
        public async Task<IActionResult> Index()
        {
            return _context.Categories.Include(c => c.Meals) != null ?
                        View(await _context.Categories.ToListAsync()) :
                        Problem("Entity set 'GastroAppContext.Categories'  is null.");
        }
        // GET: Categories/Details/5
        [Authorize(Roles = "Admin, RestaurantManager, Chief")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories.Include(c => c.Meals) == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Meals)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        [Authorize(Roles = "Admin, RestaurantManager, Chief")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, RestaurantManager, Chief")]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "Admin, RestaurantManager, Chief")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories.Include(c => c.Meals) == null)
            {
                return NotFound();
            }

            //var category = await _context.Categories.FindAsync(id);
            var category = await _context.Categories
            .Include(c => c.Meals)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, RestaurantManager, Chief")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "Admin, RestaurantManager, Chief")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Meals)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, RestaurantManager, Chief")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'GastroAppContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
