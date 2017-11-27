using Lab31Erik.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lab31Erik.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel rvm, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = rvm.Email, Email = rvm.Email };
                var result = await _userManager.CreateAsync(user, rvm.Password);


                if (result.Succeeded)
                {
                    Claim User = new Claim(ClaimTypes.Role, "RegisteredUser", ClaimValueTypes.String);
                    await _userManager.AddClaimAsync(user, User);

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }
                //ModelState.AddModelError("Password", result.Errors.ToList()[0]);

            }

            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(lvm.Email, lvm.Password, lvm.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            string error = "you are wrong";
            ModelState.AddModelError("", error);
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult AdminRegister(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AdminRegister(AdminRegisterViewModel rvm, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = rvm.Email, Email = rvm.Email };
                var result = await _userManager.CreateAsync(user, rvm.Password);

                if (result.Succeeded)
                {
                    Claim admin = new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String);
                    var AddClaim = await _userManager.AddClaimAsync(user, admin);

                    if (AddClaim.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        return RedirectToAction("Index", "Home");
                    }
                }
                //ModelState.AddModelError("Password", result.Errors.ToList()[0]);

            }

            return View();
        }
        //This loads the admin log in page.
        [AllowAnonymous]
        [HttpGet]
        public IActionResult AdminLogIn()
        {
            return View();
        }

        //This will Post the user Admin credentials.
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AdminLogIn(AdminLogInViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(lvm.Email, lvm.Password, lvm.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            string error = "you are wrong";
            ModelState.AddModelError("", error);
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
