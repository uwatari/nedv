﻿using nedv.Models.Data;
using nedv.ViewModel.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using nedv.Models.Data;
using nedv.ViewModel.Account;

namespace nedv.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        // метод срабатывает при открытии страницы регистрации, никакие значения пока передавать не нужно
        // вы просто открыли страницу с регистрацией и не успели еще ничего ввести
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        // теперь вы ввели значения и нажали кнопку "Зарегистрироваться", например
        // методом Post данные передаются через модель для представления RegisterViewModel
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // создание экземпляра user класса User и установка его свойствам значениям из модели
                User user = new User
                {
                    Surname = model.Surname,
                    Name = model.Name,
                    Email = model.Email,
                    UserName = model.Email
                };

                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    await _userManager.AddToRoleAsync(user, "registeredUser");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);   // возвращение модели в представление
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            /*if (ModelState.IsValid)
            {*/
            var result =
                await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {

                return RedirectToAction("Index", "Home");
            }
            // проверяем, принадлежит ли URL приложению
            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}