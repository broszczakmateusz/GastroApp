using GastroApp.Data;
using GastroApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Data;
using System.Drawing;
using System.Linq;

namespace GastroApp.Controllers
{
    [Authorize(Roles = "Admin, RestaurantManager")]

    public class EditUserRoleController : Controller
    {
        private readonly GastroAppContext _context;
        private readonly UserManager<User> _userManager;
        public EditUserRoleController(GastroAppContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: EditUserRoleController
        public async Task<IActionResult> Index()
        {
            var adminRoleId = _context.Roles.First(r => r.Name == "Admin").Id;
            List<UserWithRole> gastroAppContext = await _context.UserRoles.Where(ur => ur.RoleId != adminRoleId)
            .Join(_context.Users, ur => ur.UserId, uu => uu.Id, (ur, u) => new { ur, u })
            .Join(_context.Roles, ur => ur.ur.RoleId, r => r.Id, (ur, r) => new UserWithRole { User = ur.u, Role = r.Name }).ToListAsync();
            
            return View(gastroAppContext);
        }
        // GET: EditUserRole/EditRole/5
        public async Task<IActionResult> EditRole(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var previousRole = _userManager.GetRolesAsync(user).Result.ToList().First();

            var roles = await _context.Roles.ToListAsync();
            roles.Remove(await _context.Roles.Where(r => r.Name == "Admin").FirstAsync());
            ViewData["RoleName"] = new SelectList(roles, "Name", "Name", previousRole);
            return View();
        }

        // POST: EditUserRole/EditRole/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(string? id, UserWithRole userWithRole)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var previousRole = _userManager.GetRolesAsync(user).Result.ToList().First();
            var removeResult = await _userManager.RemoveFromRoleAsync(user, previousRole);
            if (!removeResult.Succeeded)
            {
                return Problem("Error during removing previous role.");
            }
            var addResult = await _userManager.AddToRoleAsync(user, userWithRole.Role);
            if (!addResult.Succeeded)
            {
                var roles = await _context.Roles.ToListAsync();
                roles.Remove(await _context.Roles.Where(r => r.Name == "Admin").FirstAsync());
                ViewData["RoleName"] = new SelectList(roles, "Name", "Name", userWithRole.Role);
                return View(user);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
