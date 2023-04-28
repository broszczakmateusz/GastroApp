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
    [Authorize(Roles = "Admin, RestaurantManager, WaiterManager, Waiter")]
    public class OrdersController : Controller
    {
        private readonly GastroAppContext _context;

        public OrdersController(GastroAppContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var gastroAppContext = _context.Orders.Where(o => o.IsPaid == false)
                .Include(o => o.Table)
                    .ThenInclude(t => t.Room)
                .Include(o => o.User)
            .Include(o => o.OrderedMeals)
            .Include(o => o.Meals);
            return View(await gastroAppContext.ToListAsync());
        }
        // GET: SelectedOrder/5
        public IActionResult SelectOrder(int? id)
        {
            if (id == null)
            {
                return Problem("Selected order is null");
            }
            return RedirectToAction("SelectedOrder", "SelectedOrder", new { id = id });
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            var Tables = _context.Tables.Include(t => t.Room).ToList();
            ViewData["TableId"] = new SelectList(Tables, "Id", "RoomAndTable");

            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TableId")] Order order)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Problem("User is null.");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return Problem("User is null.");
            }

            var table = await _context.Tables.Include(t => t.Room).FirstOrDefaultAsync(t => t.Id == order.TableId);
            if (table == null)
            {
                return Problem("Table is null.");
            }
            order.SetUserAndTableForNew(user, table);

            ModelState.Clear();
            TryValidateModel(order);

            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("SelectedOrder", "SelectedOrder", new { id = order.Id });
            }
            var Tables = await _context.Tables.Include(t => t.Room).ToListAsync();
            ViewData["TableId"] = new SelectList(Tables, "Id", "RoomAndTable", order.TableId);
            return View(order);
        }
    }
}
