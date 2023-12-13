using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using nedv.Models;
using nedv.Models.Data;
using nedv.ViewModel.Apartment;

namespace nedv.Controllers
{
    public class ApartmentsController : Controller
    {
        private readonly AppCtx _context;

        public ApartmentsController(AppCtx context)
        {
            _context = context;
        }

        // GET: Apartments
        public async Task<IActionResult> Index()
        {
            var appCtx = _context.Apartments
                .Include(a => a.City)
                .Include(a => a.TypeOfConstruction)
                .Include(a => a.AdType)
                .Include(a => a.User);
            return View(await appCtx.ToListAsync());
        }

        // GET: Apartments/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null || _context.Apartments == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartments
                .Include(a => a.City)
                .Include(a => a.TypeOfConstruction)
                .Include(a => a.User)
                .Include(a => a.AdType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (apartment == null)
            {
                return NotFound();
            }

            return View(apartment);
        }

        // GET: Apartments/Create
        public IActionResult CreateAsync()
        {
            ViewData["IdCity"] = new SelectList(_context.Cities, "Id", "CityName");
            ViewData["IdTypeOfConstruction"] = new SelectList(_context.TypeOfConstructions, "Id", "TypeOfConstructionName");
            ViewData["IdUser"] = new SelectList(_context.Users, "Id", "Name", "Surname");
            ViewData["IdAdType"] = new SelectList(_context.AdTypes, "Id", "AdTypeName");
            return View();
        }

        // POST: Apartments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateApartmentViewModel model)
        {
            if (_context.Apartments
                .Where(
                    f => f.IdCity == model.IdCity &&
                    f.IdUser == model.IdUser &&
                    f.IdAdType == model.IdAdType &&
                    f.IdTypeOfConstruction == model.IdTypeOfConstruction &&
                    f.Floor == model.Floor &&
                    f.NumberOfSquareMeters == model.NumberOfSquareMeters)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введенное объявление уже существует");
            }

            if (ModelState.IsValid)
            {
            Apartment apartment = new()
            {
                Description = model.Description,
                CountRoom = model.CountRoom,
                NumberOfSquareMeters = model.NumberOfSquareMeters,
                IdCity = model.IdCity,
                IdUser = model.IdUser,
                IdAdType = model.IdAdType,
                IdTypeOfConstruction = model.IdTypeOfConstruction,
                Floor = model.Floor,
            };

            _context.Add(apartment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Apartments/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null || _context.Apartments == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartments.FindAsync(id);
            if (apartment == null)
            {
                return NotFound();
            }

            EditApartmentViewModel model = new()
            {
                Id = apartment.Id,
                IdCity = apartment.IdCity,
                CountRoom = apartment.CountRoom,
                NumberOfSquareMeters = apartment.NumberOfSquareMeters,
                Floor = apartment.Floor,
                IdAdType = apartment.IdAdType,
                IdTypeOfConstruction = apartment.IdTypeOfConstruction,
                IdUser = apartment.IdUser
            };

            ViewData["IdCity"] = new SelectList(_context.Cities, "Id", "CityName", apartment.IdCity);
            ViewData["IdTypeOfConstruction"] = new SelectList(_context.TypeOfConstructions, "Id", "TypeOfConstructionName", apartment.IdTypeOfConstruction);
            ViewData["IdUser"] = new SelectList(_context.Users, "Id", "Name", apartment.IdUser);
            ViewData["IdAdType"] = new SelectList(_context.AdTypes, "Id", "AdTypeName", apartment.IdUser);
            return View(model);
        }

        // POST: Apartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditApartmentViewModel model)
        {
            Apartment apartment = await _context.Apartments.FindAsync(id);

            if (_context.Apartments
                .Where(
                    f => f.IdCity == model.IdCity &&
                    f.IdUser == model.IdUser &&
                    f.IdAdType == model.IdAdType &&
                    f.IdTypeOfConstruction == model.IdTypeOfConstruction &&
                    f.Floor == model.Floor &&
                    f.NumberOfSquareMeters == model.NumberOfSquareMeters)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введенное объявление уже существует");
            }

            if (id != apartment.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    apartment.Description = model.Description;
                    apartment.IdAdType = model.IdAdType;
                    apartment.CountRoom = model.CountRoom;
                    apartment.Floor = model.Floor;
                    apartment.NumberOfSquareMeters = model.NumberOfSquareMeters;
                    apartment.IdTypeOfConstruction = model.IdTypeOfConstruction;
                    apartment.IdCity = model.IdCity;
                    apartment.IdUser = model.IdUser;
                    _context.Update(apartment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApartmentExists(apartment.Id))
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
        
            ViewData["IdCity"] = new SelectList(_context.Cities, "Id", "CityName", apartment.IdCity);
            ViewData["IdTypeOfConstruction"] = new SelectList(_context.TypeOfConstructions, "Id", "TypeOfConstructionName", apartment.IdTypeOfConstruction);
            ViewData["IdUser"] = new SelectList(_context.Users, "Id", "Name", apartment.IdUser);
            ViewData["IdAdType"] = new SelectList(_context.AdTypes, "Id", "AdTypeName", apartment.IdAdType);
            return View(model);
        }

        // GET: Apartments/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null || _context.Apartments == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartments
                .Include(a => a.City)
                .Include(a => a.TypeOfConstruction)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartment == null)
            {
                return NotFound();
            }

            return View(apartment);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            if (_context.Apartments == null)
            {
                return Problem("Entity set 'AppCtx.Apartments'  is null.");
            }
            var apartment = await _context.Apartments.FindAsync(id);
            if (apartment != null)
            {
                _context.Apartments.Remove(apartment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApartmentExists(short id)
        {
          return (_context.Apartments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
