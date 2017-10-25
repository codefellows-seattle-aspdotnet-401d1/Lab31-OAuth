﻿using lab28_miya.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace lab28_miya.Controllers
{
    //[Authorize(Policy = "MinimumYearsInService")]
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
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel rvm)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = rvm.Email, Email = rvm.Email, FirstName = rvm.FirstName, LastName = rvm.LastName };

                var result = await _userManager.CreateAsync(user, rvm.Password);

                if(result.Succeeded)
                {
                    const string issuer = "www.miya.com";

                    //creating a list where I can add claims
                    List<Claim> myClaims = new List<Claim>();

                    //this is a claim for the user's name
                    Claim claim1 = new Claim(ClaimTypes.Name, rvm.FirstName + " " + rvm.LastName, ClaimValueTypes.String, issuer);
                    myClaims.Add(claim1);

                    //this is a claim for the user's role
                    Claim claim2 = new Claim(ClaimTypes.Role, "CPS Agent", ClaimValueTypes.String);
                    myClaims.Add(claim2);

                    //this is a claim that declares when the worker first started with the company
                    Claim StartDate = new Claim(ClaimTypes.UserData, rvm.StartDate.Date.ToString(), ClaimValueTypes.Date);
                    myClaims.Add(StartDate);

                    var addClaims = await _userManager.AddClaimsAsync(user, myClaims);

                    await _signInManager.SignInAsync(user, false);

                    return RedirectToAction("Index", "Home");

                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(lvm.Email);

                var result = await _signInManager.PasswordSignInAsync(lvm.Email, lvm.Password, lvm.RememberMe, false);
            }
            return View();
        }

        private IActionResult AccessDenied()
        {
            return View("Forbidden");
        }

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return View();
        }
    }
}
