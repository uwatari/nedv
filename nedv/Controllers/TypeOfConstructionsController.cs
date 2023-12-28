using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using nedv.Models;
using nedv.Models.Data;
using nedv.ViewModel.TypeOfConstructions;

namespace nedv.Controllers
{
    [Authorize(Roles = "admin")]
    public class TypeOfConstructionsController : Controller
    {
        private readonly AppCtx _context;

        public TypeOfConstructionsController(AppCtx context)
        {
            _context = context;
        }

        // GET: TypeOfConstructions
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["TypeOfConstructionSortParm"] = String.IsNullOrEmpty(sortOrder) ? "roomtypename_desc" : "";

            var typeofconstrucrion = from t in _context.TypeOfConstructions
                           select t;
            if (!String.IsNullOrEmpty(searchString))
            {
                typeofconstrucrion = typeofconstrucrion.Where(t => t.TypeOfConstructionName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    typeofconstrucrion = typeofconstrucrion.OrderByDescending(t => t.TypeOfConstructionName);
                    break;
                default:
                    typeofconstrucrion = typeofconstrucrion.OrderBy(t => t.TypeOfConstructionName);
                    break;
            }
            return View(await typeofconstrucrion.AsNoTracking().ToListAsync());
        }

        // GET: TypeOfConstructions/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null || _context.TypeOfConstructions == null)
            {
                return NotFound();
            }

            var typeOfConstruction = await _context.TypeOfConstructions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeOfConstruction == null)
            {
                return NotFound();
            }

            return View(typeOfConstruction);
        }

        // GET: TypeOfConstructions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypeOfConstructions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTypeOfConstructionViewModel model)
        {
            if (_context.TypeOfConstructions
                .Where(f => f.TypeOfConstructionName == model.TypeOfConstructionName)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный тип постройки уже существует");
            }

            if (ModelState.IsValid)
            {
                TypeOfConstruction typeOfConstruction = new()
                {
                    TypeOfConstructionName = model.TypeOfConstructionName
                };

                _context.Add(typeOfConstruction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: TypeOfConstructions/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null || _context.TypeOfConstructions == null)
            {
                return NotFound();
            }

            var typeOfConstruction = await _context.TypeOfConstructions.FindAsync(id);
            if (typeOfConstruction == null)
            {
                return NotFound();
            }

            EditTypeOfConstructionViewModel model = new()
            {
                Id = typeOfConstruction.Id,
                TypeOfConstructionName = typeOfConstruction.TypeOfConstructionName
            };
            return View(model);
        }

        // POST: TypeOfConstructions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditTypeOfConstructionViewModel model)
        {
            TypeOfConstruction typeOfConstruction = await _context.TypeOfConstructions.FindAsync(id);

            if (_context.TypeOfConstructions
               .Where(f => f.TypeOfConstructionName == model.TypeOfConstructionName)
               .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный тип постройки уже существует");
            }

            if (id != typeOfConstruction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    typeOfConstruction.TypeOfConstructionName = model.TypeOfConstructionName;
                    _context.Update(typeOfConstruction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeOfConstructionExists(typeOfConstruction.Id))
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
            return View(model);
        }

        // GET: TypeOfConstructions/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null || _context.TypeOfConstructions == null)
            {
                return NotFound();
            }

            var typeOfConstruction = await _context.TypeOfConstructions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeOfConstruction == null)
            {
                return NotFound();
            }

            return View(typeOfConstruction);
        }

        // POST: TypeOfConstructions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            if (_context.TypeOfConstructions == null)
            {
                return Problem("Entity set 'AppCtx.TypeOfConstruction'  is null.");
            }
            var typeOfConstruction = await _context.TypeOfConstructions.FindAsync(id);
            if (typeOfConstruction != null)
            {
                _context.TypeOfConstructions.Remove(typeOfConstruction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeOfConstructionExists(short id)
        {
          return (_context.TypeOfConstructions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
