﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DnDManager.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace DnDManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel rvm, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = rvm.Email, Email = rvm.Email, Birthday = rvm.Birthday };
                var result = await _userManager.CreateAsync(user, rvm.Password);

                if (result.Succeeded)
                {
                    await _signInManager.PasswordSignInAsync(rvm.Email, rvm.Password, true, true);
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult AdminRegister(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminRegister(RegisterViewModel rvm, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = rvm.Email, Email = rvm.Email, Birthday = rvm.Birthday };
                var result = await _userManager.CreateAsync(user, rvm.Password);

                if (result.Succeeded)
                {
                    List<Claim> myClaims = new List<Claim>();

                    Claim admin = new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String);
                    myClaims.Add(admin);

                    var addClaims = await _userManager.AddClaimsAsync(user, myClaims);

                    if (addClaims.Succeeded)
                    {
                        await _signInManager.PasswordSignInAsync(rvm.Email, rvm.Password, true, true);
                        return RedirectToAction("Index", "Home"); 
                    }
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(lvm.Email, lvm.Password, lvm.RememberMe, true);
                if (result.Succeeded)
                {
                    List<Claim> myClaims = new List<Claim>();

                    Claim dungeonMaster = new Claim(ClaimTypes.Role, "Dungeon Master", ClaimValueTypes.String);
                    myClaims.Add(dungeonMaster);

                    Claim player = new Claim(ClaimTypes.Role, "Player", ClaimValueTypes.String);
                    myClaims.Add(player);

                    var userIdentity = new ClaimsIdentity("Registration");
                    userIdentity.AddClaims(myClaims);

                    var userPrincipal = new ClaimsPrincipal(userIdentity);

                    User.AddIdentity(userIdentity);

                    await HttpContext.SignInAsync("BrandonCookie", userPrincipal,
                        new AuthenticationProperties
                        {
                            ExpiresUtc = DateTime.UtcNow.AddHours(6),
                            IsPersistent = lvm.RememberMe,
                            AllowRefresh = false
                        });
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        public IActionResult ExternalLogin(string provider, string returnURL = null)
        {
            var redirectURL = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnURL });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectURL);
            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnURL = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                return RedirectToAction(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                return RedirectToAction("Index", "Home");

            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginModel { Email = email });
            }
        }

        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginModel elm)
        {
            if (ModelState.IsValid)
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();



                if (info == null)
                {
                    return RedirectToAction(nameof(Login));
                }

                var user = new ApplicationUser { UserName = elm.Email, Email = elm.Email };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View(nameof(ExternalLogin), elm);
        }

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
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
    }
}