﻿using System;
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
    public class CategoriesController : Controller
    {
        private readonly GastroAppContext _context;

        public CategoriesController(GastroAppContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index(int? orderId)
        {

            if (orderId == null)
            {
                Problem("Entity set 'GastroAppContext.Categories'  is null.");
            }
            
            ViewData["OrderId"] = orderId;
            return View(await _context.Categories.ToListAsync());
        }
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
    }
}