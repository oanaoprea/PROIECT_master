using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROIECT.Data;
using PROIECT.Models;
using PROIECT.Models.PlantsViewModels;

namespace PROIECT.Controllers
{
    [Authorize(Policy = "OnlySales")]
    public class ShopsController : Controller
    {
        private readonly PlantsContext _context;

        public ShopsController(PlantsContext context)
        {
            _context = context;
        }

        // GET: Shops
        public async Task<IActionResult> Index(int? id, int? plantID)
        {
            var viewModel = new ShopIndexData();
            viewModel.Shops = await _context.Shops
            .Include(i => i.AvailablePlants)
            .ThenInclude(i => i.Plant)
            .ThenInclude(i => i.Orders)
            .ThenInclude(i => i.Customer)
            .Include(i => i.AvailablePlants)
            .ThenInclude(i => i.Plant)
            .ThenInclude(i => i.Category)
            .AsNoTracking()
            .OrderBy(i => i.ShopName)
            .ToListAsync();
            if (id != null)
            {
                ViewData["ShopID"] = id.Value;
                Shop shop = viewModel.Shops.Where(
                i => i.ID == id.Value).Single();
                viewModel.Plants = shop.AvailablePlants.Select(s => s.Plant);
            }
            if (plantID != null)
            {
                ViewData["PlantID"] = plantID.Value;
                viewModel.Orders = viewModel.Plants.Where(
                x => x.ID == plantID).Single().Orders;
            }
            return View(viewModel);
        }

        // GET: Shops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Shops == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops
                .FirstOrDefaultAsync(m => m.ID == id);
            if (shop == null)
            {
                return NotFound();
            }

            return View(shop);
        }

        // GET: Shops/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ShopName,Adress")] Shop shop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shop);
        }

        // GET: Shops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops
                .Include(i => i.AvailablePlants)
                .ThenInclude(i => i.Plant)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            
            if (shop == null)
            {
                return NotFound();
            }
            PopulateAvailablePlantData(shop);
            return View(shop);
        }

        private void PopulateAvailablePlantData(Shop shop)
        {
            var allPlants = _context.Plants;
            var shopPlants = new HashSet<int>(shop.AvailablePlants.Select(c => c.PlantID));
            var viewModel = new List<AvailablePlantData>();
            foreach (var plant in allPlants)
            {
                viewModel.Add(new AvailablePlantData
                {
                    PlantID = plant.ID,
                    Name = plant.Name,
                    IsAvailable = shopPlants.Contains(plant.ID)
                });
            }
            ViewData["Plants"] = viewModel;
        }

        // POST: Shops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedPlants)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopToUpdate = await _context.Shops
                .Include(i => i.AvailablePlants)
                .ThenInclude(i => i.Plant)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (await TryUpdateModelAsync<Shop>(
            shopToUpdate,
            "",
            i => i.ShopName, i => i.Adress))
            {
                UpdateAvailablePlants(selectedPlants, shopToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateAvailablePlants(selectedPlants, shopToUpdate);
            PopulateAvailablePlantData(shopToUpdate);
            return View(shopToUpdate);
        }

        private void UpdateAvailablePlants(string[] selectedPlants, Shop shopToUpdate)
        {
            if (selectedPlants == null)
            {
                shopToUpdate.AvailablePlants = new List<AvailablePlant>();
                return;
            }
            var selectedPlantsHS = new HashSet<string>(selectedPlants);
            var availablePlants = new HashSet<int>
            (shopToUpdate.AvailablePlants.Select(c => c.Plant.ID));
            foreach (var plant in _context.Plants)
            {
                if (selectedPlantsHS.Contains(plant.ID.ToString()))
                {
                    if (!availablePlants.Contains(plant.ID))
                    {
                        shopToUpdate.AvailablePlants.Add(new AvailablePlant { ShopID = shopToUpdate.ID, PlantID = plant.ID });
                    }
                }
                else
                {
                    if (availablePlants.Contains(plant.ID))
                    {
                        AvailablePlant plantToRemove = shopToUpdate.AvailablePlants.FirstOrDefault(i => i.PlantID == plant.ID);
                        _context.Remove(plantToRemove);
                    }
                }
            }
        }

        // GET: Shops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Shops == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops
                .FirstOrDefaultAsync(m => m.ID == id);
            if (shop == null)
            {
                return NotFound();
            }

            return View(shop);
        }

        // POST: Shops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Shops == null)
            {
                return Problem("Entity set 'PlantsContext.Shops'  is null.");
            }
            var shop = await _context.Shops.FindAsync(id);
            if (shop != null)
            {
                _context.Shops.Remove(shop);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopExists(int id)
        {
          return _context.Shops.Any(e => e.ID == id);
        }
    }
}
