using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nedv.Models;
using nedv.Models.Data;
using nedv.ViewModel.AdTypes;

namespace nedv.Controllers
{
    public class AdTypesController : Controller
    {
        private readonly AppCtx _context;

        public AdTypesController(AppCtx context)
        {
            _context = context;
        }

        // GET: AdTypes
        public async Task<IActionResult> Index()
        {
            // через контекст данных получаем доступ к таблице базы данных FormsOfStudy
            var appCtx = _context.AdTypes
                .OrderBy(f => f.AdTypeName);          // сортируем все записи по имени форм обучения

            // возвращаем в представление полученный список записей
            return View(await appCtx.ToListAsync());
        }

        // GET: AdTypes/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null || _context.AdTypes == null)
            {
                return NotFound();
            }

            var adType = await _context.AdTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adType == null)
            {
                return NotFound();
            }

            return View(adType);
        }

        // GET: AdTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAdTypeViewModel model)
        {
            if (_context.AdTypes
                .Where(f => f.AdTypeName == model.AdTypeName)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный тип объявления уже существует");
            }

            if (ModelState.IsValid)
            {
                AdType adtype = new()
                {
                    AdTypeName = model.AdTypeName
                };

                _context.Add(adtype);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: AdTypes/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null || _context.AdTypes == null)
            {
                return NotFound();
            }

            var adType = await _context.AdTypes.FindAsync(id);
            if (adType == null)
            {
                return NotFound();
            }

            EditAdTypeViewModel model = new()
            {
                Id = adType.Id,
                AdTypeName = adType.AdTypeName
            };
            return View(model);
        }

        // POST: AdTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditAdTypeViewModel model)
        {
            AdType adType = await _context.AdTypes.FindAsync(id);

            if (_context.AdTypes
               .Where(f => f.AdTypeName == model.AdTypeName)
               .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный тип объявления уже существует");
            }

            if (id != adType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    adType.AdTypeName = model.AdTypeName;
                    _context.Update(adType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdTypeExists(adType.Id))
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

        // GET: AdTypes/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null || _context.AdTypes == null)
            {
                return NotFound();
            }

            var adType = await _context.AdTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adType == null)
            {
                return NotFound();
            }

            return View(adType);
        }

        // POST: AdTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            if (_context.AdTypes == null)
            {
                return Problem("Entity set 'AppCtx.AdTypes'  is null.");
            }
            var adType = await _context.AdTypes.FindAsync(id);
            if (adType != null)
            {
                _context.AdTypes.Remove(adType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdTypeExists(short id)
        {
          return (_context.AdTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
