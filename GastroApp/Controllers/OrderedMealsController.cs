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

        // POST: Orders/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create([Bind("Id,OrderId,Order,MealId,Meal,Annotation,CreatedDateTime")] OrderedMeal orderedMeal)
        {
            if (orderedMeal == null)
            {
                return Problem("OrderedMeal is null.");
            }
            //ModelState.Clear();
            //TryValidateModel(orderedMeal);

            if (ModelState.IsValid)
            {
                _context.Add(orderedMeal);
                _context.SaveChanges();
                return RedirectToAction("AddOrderedMealToOrder", "SelectedOrder", new { orderedMealId = orderedMeal.Id });
            }

            return Problem("Create OrderedMeal was failed.");
        }
    }
}
