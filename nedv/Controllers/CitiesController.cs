using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using nedv.Models.Data;
using nedv.Models;
using nedv.Models;
using nedv.Models.Data;
using nedv.ViewModel.Cities;

namespace nedv.Controllers
{
    /*    [Authorize(Roles = "admin, registeredUser")]*/
    public class CitiesController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public CitiesController(AppCtx context, UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: Specialties
        public async Task<IActionResult> Index()
        {
            // IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            var appCtx = _context.Cities
                .Include(s => s.Region)                    // связываем специальности с формами обучения
                .OrderBy(f => f.CityName);                          // сортировка по коду специальности
            return View(await appCtx.ToListAsync());            // полученный результат передаем в представление списком
        }

        // GET: Specialties/Create
        public async Task<IActionResult> CreateAsync()
        {
            // IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            // при отображении страницы заполняем элемент "выпадающий список" формами обучения
            // при этом указываем, что в качестве идентификатора используется поле "Id"
            // а отображать пользователю нужно поле "FormOfEdu" - название формы обучения
            ViewData["IdRegion"] = new SelectList(_context.Regions, "Id", "RegionName");
            return View();
        }

        // POST: Specialties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCityViewModel model)
        {
            // IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.Cities
                .Where(f => f.IdRegion == model.IdRegion &&
                    f.CityName == model.CityName)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Такое название отеля уже существует");
            }

            if (ModelState.IsValid)
            {
                // если введены корректные данные,
                // то создается экземпляр класса модели Specialty, т.е. формируется запись в таблицу Specialties
                City city = new()
                {
                    CityName = model.CityName,
                    // с помощью свойства модели получим идентификатор выбранной формы обучения пользователем
                    IdRegion = model.IdRegion
                };

                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdRegion"] = new SelectList(
                _context.Regions,
                "Id", "RegionName", model.IdRegion);
            return View(model);
        }

        // GET: Specialties/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            EditCityViewModel model = new()
            {
                Id = city.Id,
                CityName = city.CityName,
                IdRegion = city.IdRegion
            };

            // IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            // в списке в качестве текущего элемента устанавливаем значение из базы данных,
            // указываем параметр specialty.IdFormOfStudy
            ViewData["IdRegion"] = new SelectList(
                _context.Regions,
                "Id", "RegionName", city.IdRegion);
            return View(model);
        }

        // POST: Specialties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditCityViewModel model)
        {
            City city = await _context.Cities.FindAsync(id);

            if (_context.Cities
                .Where(f => f.IdRegion == model.IdRegion &&
                    f.CityName == model.CityName)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Вы не изменили название города");
            }
            if (id != city.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    city.CityName = model.CityName;
                    city.IdRegion = model.IdRegion;
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.Id))
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
            // IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            ViewData["IdRegion"] = new SelectList(
                _context.Regions,
                "Id", "RegionName", city.IdRegion);
            return View(model);
        }

        // GET: Specialties/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .Include(s => s.Region)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: Specialties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var city = await _context.Cities.FindAsync(id);
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Specialties/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .Include(s => s.Region)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        private bool CityExists(short id)
        {
            return _context.Cities.Any(e => e.Id == id);
        }
    }
}