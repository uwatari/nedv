using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using nedv.Models;
using nedv.Models.Data;
using nedv.ViewModel.Cities;

namespace nedv.Controllers
{
    public class CitiesController : Controller
    {
        private readonly AppCtx _context;

        public CitiesController(AppCtx context)
        {
            _context = context;
        }

        // GET: Cities
        public async Task<IActionResult> Index()
        {
            // через контекст данных получаем доступ к таблице базы данных FormsOfStudy
            var appCtx = _context.Cities
                .OrderBy(f => f.CityName);          // сортируем все записи по имени форм обучения

            // возвращаем в представление полученный список записей
            return View(await appCtx.ToListAsync());
        }

        // GET: Cities/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null || _context.Cities == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // GET: Cities/Create
        public IActionResult Create()
        {
            return View();
        }   

        // POST: Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCityViewModel model)
        {
            if (_context.Cities
                .Where(f => f.CityName == model.CityName)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный город уже существует");
            }

            if (ModelState.IsValid)
            {
                City genre = new()
                {
                    CityName = model.CityName
                };

                _context.Add(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Cities/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null || _context.Cities == null)
            {
                return NotFound();
            }

            var genre = await _context.Cities.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            EditCityViewModel model = new()
            {
                Id = genre.Id,
                CityName = genre.CityName
            };
            return View(model);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditCityViewModel model)
        {
            City city = await _context.Cities.FindAsync(id);

            if (_context.Cities
                .Where(f => f.CityName == model.CityName)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный город уже существует");
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
            return View(model);
        }

        // GET: Cities/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null || _context.Cities == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            if (_context.Cities == null)
            {
                return Problem("Entity set 'AppCtx.Cities'  is null.");
            }
            var city = await _context.Cities.FindAsync(id);
            if (city != null)
            {
                _context.Cities.Remove(city);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(short id)
        {
          return (_context.Cities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
