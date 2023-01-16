using Microsoft.AspNetCore.Mvc;
using PROIECT.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROIECT.Data;
using PROIECT.Models.PlantsViewModels;

namespace PROIECT.Controllers
{
    public class HomeController : Controller
    {
        private readonly PlantsContext _context;

        public HomeController(PlantsContext context)
        {
            _context = context;
        }
        public IActionResult Privacy()
        {
            return View();
        }
        // GET: Plants
        public async Task<IActionResult> Index(string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
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

        //public async Task<IActionResult> AddToCart (int ID)
        //{
        //    var order = await _context.Orders
        //        .Include(o => o.Customer)
        //        .Include(o => o.Plant)
        //        .FirstOrDefaultAsync(m => m.PlantID == ID);
        //    order.Number = 1;
        //    order.CustomerID = 1;
        //    order.PlantID = ID;
        //    order.OrderID = 2;
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(order);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(order);
        //}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<ActionResult> Statistics()
        {
            IQueryable<OrderGroup> data =
            from order in _context.Orders
            group order by order.OrderDate into dateGroup
            select new OrderGroup()
            {
                OrderDate = dateGroup.Key,
                PlantCount = dateGroup.Count()
            };
            return View(await data.AsNoTracking().ToListAsync());
        }

        public IActionResult Chat() 
        { 
            return View(); 
        }
    }
}