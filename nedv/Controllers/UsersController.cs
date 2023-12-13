using nedv.Models;
using nedv.ViewModel.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using nedv.Models.Data;
using nedv.ViewModel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace nedv.Controllers
{
    public class UsersController : Controller
    {
        UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // отображение списка пользователей
        // действия для начальной страницы Index
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewData["EmailSortParm"] = String.IsNullOrEmpty(sortOrder) ? "email_asc" : "";
            ViewData["SurnameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "surname_asc" : "";
            ViewData["RegistrationDateSortParm"] = sortOrder == "Date" ? "registrationdate_asc" : "Date";
            ViewData["EmploymentDateSortParm"] = sortOrder == "Date" ? "employmentdate_asc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var users = from u in _userManager.Users
                           select u;

            switch (sortOrder)
            {
                case "name_asc":
                    users = users.OrderByDescending(s => s.Name);
                    break;
                case "surname_asc":
                    users = users.OrderByDescending(s => s.Surname);
                    break;
                case "registrationdate_asc":
                    users = users.OrderByDescending(s => s.RegistrationDate);
                    break;
                case "employmentdate_asc":
                    users = users.OrderByDescending(s => s.EmploymentDate);
                    break;
                default:
                    users = users.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 5;
            return View(await PaginatedList<User>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize));
        }


        // действия для создания пользователя Create
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Surname = model.Surname,
                    Name = model.Name,
                    RegistrationDate = DateTime.Now,
                    EmploymentDate = model.EmploymentDate
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }


        // действия для изменения пользователя Edit
        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Surname = user.Surname,
                Name = user.Name,
                EmploymentDate = user.EmploymentDate
            };
            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.Surname = model.Surname;
                    user.Name = model.Name;
                    user.EmploymentDate = model.EmploymentDate;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }


        // действия для удаления пользователя Delete с подтверждением
        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            IdentityResult result = await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }
}