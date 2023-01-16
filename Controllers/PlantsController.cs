using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROIECT.Data;
using PROIECT.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace PROIECT.Controllers
{
    [Authorize(Roles = "Employee")]
    public class PlantsController : Controller
    {
        private readonly PlantsContext _context;

        public PlantsController(PlantsContext context)
        {
            _context = context;
        }

        // GET: Plants
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder)? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var plants = from b in _context.Plants
                        join a in _context.Categories on b.CategoryID equals a.CategoryID
                        select new Plant
                        {
                            ID = b.ID,
                            Name = b.Name,
                            CategoryID = b.CategoryID,
                            Price = b.Price,
                            Description = b.Description,
                            Water = b.Water,
                            Temperature = b.Temperature,
                            Light = b.Light,
                            Image = b.Image,
                            Category = a
                        };

            if (!String.IsNullOrEmpty(searchString))
            {
                plants = plants.Where(s => s.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    plants = plants.OrderByDescending(b => b.Name);
                    break;
                case "Price":
                    plants = plants.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    plants = plants.OrderByDescending(b => b.Price);
                    break;
                default:
                    plants = plants.OrderBy(b => b.Name);
                    break;
            }

            int pageSize = 2;
            return View(await PaginatedList<Plant>.CreateAsync(plants.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Plants/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Plants == null)
            {
                return NotFound();
            }

            var plant = await _context.Plants
                .Include(p => p.Category)
                .Include(s => s.Orders)
                .ThenInclude(e => e.Customer)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (plant == null)
            {
                return NotFound();
            }

            return View(plant);
        }

        // GET: Plants/Create
        public IActionResult Create()
        {
            ViewData["Name"] = new SelectList(_context.Categories, "CategoryID", "Name");
            return View();
        }

        // POST: Plants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,CategoryID,Description,Light,Water,Temperature,Price,Image")] Plant plant)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(plant);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists ");
            }
            ViewData["Name"] = new SelectList(_context.Categories, "CategoryID", "Name", plant.CategoryID);
            return View(plant);
        }

        // GET: Plants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Plants == null)
            {
                return NotFound();
            }

            var plant = await _context.Plants.FindAsync(id);
            if (plant == null)
            {
                return NotFound();
            }
            ViewData["Name"] = new SelectList(_context.Categories, "CategoryID", "Name", plant.CategoryID);
            return View(plant);
        }

        // POST: Plants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantToUpdate = await _context.Plants.FirstOrDefaultAsync(s => s.ID == id);

            if (await TryUpdateModelAsync<Plant>(plantToUpdate,"",s => s.CategoryID, s => s.Name, s => s.Price, s => s.Description, s => s.Light, s => s.Water, s => s.Temperature, s => s.Image))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists");
                }
            }
            ViewData["Name"] = new SelectList(_context.Categories, "CategoryID", "Name", plantToUpdate.CategoryID);
            return View(plantToUpdate);
        }

        // GET: Plants/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plant = await _context.Plants
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (plant == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                "Delete failed. Try again";
            }

            return View(plant);
        }

        // POST: Plants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plant = await _context.Plants.FindAsync(id);

            if (plant == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Plants.Remove(plant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool PlantExists(int id)
        {
            return _context.Plants.Any(e => e.ID == id);
        }
    }
}
