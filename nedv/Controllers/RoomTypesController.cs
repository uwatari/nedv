using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nedv.Models;
using nedv.Models.Data;
using nedv.ViewModel.RoomTypes;

namespace nedv.Controllers
{
    [Authorize(Roles = "admin")]
    public class RoomTypesController : Controller
    {
        private readonly AppCtx _context;

        public RoomTypesController(AppCtx context)
        {
            _context = context;
        }

        // GET: AdTypes
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["RoomTypeNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "roomtypename_desc" : "";

            var roomtype = from s in _context.RoomTypes
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                roomtype = roomtype.Where(s => s.RoomTypeName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    roomtype = roomtype.OrderByDescending(s => s.RoomTypeName);
                    break;
                default:
                    roomtype = roomtype.OrderBy(s => s.RoomTypeName);
                    break;
            }
            return View(await roomtype.AsNoTracking().ToListAsync());
        }

        // GET: AdTypes/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null || _context.RoomTypes == null)
            {
                return NotFound();
            }

            var roomType = await _context.RoomTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomType == null)
            {
                return NotFound();
            }

            return View(roomType);
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
        public async Task<IActionResult> Create(CreateRoomTypeViewModel model)
        {
            if (_context.RoomTypes
                .Where(f => f.RoomTypeName == model.RoomTypeName)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный тип помещения уже существует");
            }

            if (ModelState.IsValid)
            {
                RoomType roomType = new()
                {
                    RoomTypeName = model.RoomTypeName
                };

                _context.Add(roomType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: AdTypes/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null || _context.RoomTypes == null)
            {
                return NotFound();
            }

            var roomType = await _context.RoomTypes.FindAsync(id);
            if (roomType == null)
            {
                return NotFound();
            }

            EditRoomTypeViewModel model = new()
            {
                Id = roomType.Id,
                RoomTypeName = roomType.RoomTypeName
            };
            return View(model);
        }

        // POST: AdTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditRoomTypeViewModel model)
        {
            RoomType roomType = await _context.RoomTypes.FindAsync(id);

            if (_context.RoomTypes
               .Where(f => f.RoomTypeName == model.RoomTypeName)
               .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный тип помещения уже существует");
            }

            if (id != roomType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    roomType.RoomTypeName = model.RoomTypeName;
                    _context.Update(roomType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdTypeExists(roomType.Id))
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
            if (id == null || _context.RoomTypes == null)
            {
                return NotFound();
            }

            var roomType = await _context.RoomTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomType == null)
            {
                return NotFound();
            }

            return View(roomType);
        }

        // POST: AdTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            if (_context.RoomTypes == null)
            {
                return Problem("Entity set 'AppCtx.AdTypes'  is null.");
            }
            var roomType = await _context.RoomTypes.FindAsync(id);
            if (roomType != null)
            {
                _context.RoomTypes.Remove(roomType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdTypeExists(short id)
        {
            return (_context.RoomTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
