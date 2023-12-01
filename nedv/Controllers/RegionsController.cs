using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nedv.Models;
using nedv.Models.Data;
using nedv.ViewModel.Cities;
using nedv.ViewModel.Regions;

namespace nedv.Controllers
{
    public class RegionsController : Controller
    {
        private readonly AppCtx _context;

        public RegionsController(AppCtx context)
        {
            _context = context;
        }

        // GET: Regions
        public async Task<IActionResult> Index()
        {
            // через контекст данных получаем доступ к таблице базы данных FormsOfStudy
            var appCtx = _context.Regions
                .OrderBy(f => f.RegionName);          // сортируем все записи по имени форм обучения

            // возвращаем в представление полученный список записей
            return View(await appCtx.ToListAsync());
        }

        // GET: Regions/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null || _context.Regions == null)
            {
                return NotFound();
            }

            var region = await _context.Regions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (region == null)
            {
                return NotFound();
            }

            return View(region);
        }

        // GET: Regions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Regions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRegionViewModel model)
        {
            if (_context.Regions
                .Where(f => f.RegionName == model.RegionName)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введенная область уже существует");
            }

            if (ModelState.IsValid)
            {
                Region region = new()
                {
                    RegionName = model.RegionName
                };

                _context.Add(region);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Regions/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null || _context.Regions == null)
            {
                return NotFound();
            }

            var region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            EditRegionViewModel model = new()
            {
                Id = region.Id,
                RegionName = region.RegionName
            };
            return View(model);
        }

        // POST: Regions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditRegionViewModel model)
        {
            Region region = await _context.Regions.FindAsync(id);

            if (_context.Regions
                .Where(f => f.RegionName == model.RegionName)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введенная область уже существует");
            }

            if (id != region.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    region.RegionName = model.RegionName;
                    _context.Update(region);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegionExists(region.Id))
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

        // GET: Regions/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null || _context.Regions == null)
            {
                return NotFound();
            }

            var region = await _context.Regions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (region == null)
            {
                return NotFound();
            }

            return View(region);
        }

        // POST: Regions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            if (_context.Regions == null)
            {
                return Problem("Entity set 'AppCtx.Regions'  is null.");
            }
            var region = await _context.Regions.FindAsync(id);
            if (region != null)
            {
                _context.Regions.Remove(region);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegionExists(short id)
        {
          return (_context.Regions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
