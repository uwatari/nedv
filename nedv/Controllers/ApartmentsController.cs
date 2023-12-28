using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using nedv.Models;
using nedv.Models.Data;
using nedv.ViewModel.Apartment;

namespace nedv.Controllers
{
    [Authorize(Roles = "admin, registeredUser")]
    public class ApartmentsController : Controller
    {
        private readonly AppCtx _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ApartmentsController(AppCtx context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Apartments
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber, int? adtypeId)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DescriptionSortParam"] = String.IsNullOrEmpty(sortOrder) ? "description_desc" : "";
            ViewData["CountRoomSortParam"] = sortOrder == "countroom" ? "countroom_desc" : "countroom";
            ViewData["NumberOfSquareMetersSortParam"] = sortOrder == "numberofsquaremeters" ? "numberofsquaremeters_desc" : "numberofsquaremeters";
            ViewData["AdTypeSortParam"] = sortOrder == "adtype" ? "adtype_desc" : "adtype";
            ViewData["TypeOfConstructionSortParam"] = sortOrder == "typeofconstruction" ? "typeofconstruction_desc" : "typeofconstruction";
            ViewData["FloorSortParam"] = sortOrder == "floor" ? "floor_desc" : "floor";
            ViewData["CitySortParam"] = sortOrder == "city" ? "city_desc" : "city";
            ViewData["UserSortParam"] = String.IsNullOrEmpty(sortOrder) ? "user_desc" : "";

            ViewData["IdAdType"] =  adtypeId;
            ViewBag.AdTypes = _context.AdTypes.ToList();
           /* List<SelectListItem> selectList = new List<SelectListItem>
            {

            };*/

            var announcement = from a in _context.Apartments.Include(i => i.AdType).Include(i => i.TypeOfConstruction).Include(i => i.City).Include(i => i.User)
                               select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                announcement = announcement.Where(a => a.Description.Contains(searchString));
            }

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            if (adtypeId.HasValue)
            {
                announcement = announcement.Where(w => w.IdAdType == adtypeId.Value);
            }

            /*switch (sortOrder)
            {
                case "countroom":
                    announcement = announcement.OrderBy(a => a.CountRoom);
                    break;
                case "countroom_desc":
                    announcement = announcement.OrderByDescending(a => a.CountRoom);
                    break;
                case "numberofsquaremeters":
                    announcement = announcement.OrderBy(a => a.NumberOfSquareMeters);
                    break;
                case "numberofsquaremeters_desc":
                    announcement = announcement.OrderByDescending(a => a.NumberOfSquareMeters);
                    break;
                case "adtype":
                    announcement = announcement.OrderBy(a => a.AdType);
                    break;
                case "adtype_desc":
                    announcement = announcement.OrderByDescending(a => a.AdType);
                    break;
                case "typeofconstruction":
                    announcement = announcement.OrderBy(a => a.TypeOfConstruction);
                    break;
                case "typeofconstruction_desc":
                    announcement = announcement.OrderByDescending(a => a.TypeOfConstruction);
                    break;
                case "floor":
                    announcement = announcement.OrderBy(a => a.Floor);
                    break;
                case "floor_desc":
                    announcement = announcement.OrderByDescending(a => a.Floor);
                    break;
                case "city":
                    announcement = announcement.OrderBy(a => a.City);
                    break;
                case "city_desc":
                    announcement = announcement.OrderByDescending(a => a.City);
                    break;
                case "user_desc":
                    announcement = announcement.OrderByDescending(a => a.User);
                    break;
                default:
                    announcement = announcement.OrderBy(a => a.Description);
                    break;
            }*/

            int pageSize = 5;
            return View(await PaginatedList<Apartment>.CreateAsync(announcement.AsNoTracking(), pageNumber ?? 1, pageSize));
            /*var appCtx = _context.Apartments
                .Include(a => a.City)
                .Include(a => a.TypeOfConstruction)
                .Include(a => a.AdType)
                .Include(a => a.User);
            return View(await appCtx.ToListAsync()); */

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
                if  (model.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.ImgUrl = "/images/" + uniqueFileName;

                    using(var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                }
                Apartment apartment = new()
                {
                    Description = model.Description,
                    CountRoom = model.CountRoom,
                    NumberOfSquareMeters = model.NumberOfSquareMeters,
                    ImgUrl = model.ImgUrl,
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
            else
            {
                return NotFound(ModelState);
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
                Description = apartment.Description,
                CountRoom = apartment.CountRoom,
                NumberOfSquareMeters = apartment.NumberOfSquareMeters,
                ImgUrl = apartment.ImgUrl,
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
                    f.ImgUrl == model.ImgUrl &&
                    f.Description == model.Description &&
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
                if (model.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.ImgUrl = "/images/" + uniqueFileName;

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                }
                try
                {
                    apartment.Description = model.Description;
                    apartment.IdAdType = model.IdAdType;
                    apartment.CountRoom = model.CountRoom;
                    apartment.Floor = model.Floor;
                    apartment.ImgUrl = model.ImgUrl;
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
