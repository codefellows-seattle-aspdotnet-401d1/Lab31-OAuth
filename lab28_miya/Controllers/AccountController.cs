using lab28_miya.Models;
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

        public IActionResult ExternalLogin(string provider, string returnURL = null) 
        {
            var redirectURL = Url.Action(nameof(ExternalLoginCallback), "Account", new {returnURL});
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectURL);
            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnURL = null, string remoteError = null) 
        {
            if(remoteError != null) 
            {
                return RedirectToAction(nameof(LogIn));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if(info == null) 
            {
                return RedirectToAction(nameof(LogIn));       
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if(result.Succeeded) 
            {
                return RedirectToAction("Index", "Home");
            }

            if(result.IsLockedOut) 
            {
                return RedirectToAction("Index", "Home");
            }
            else 
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                return View("ExternalLogin", new ExternalLoginModel {Email = email});
            }
        }

        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginModel elm) 
        {
            if(ModelState.IsValid) 
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();

                if(info == null) 
                {
                    return RedirectToAction(nameof(LogIn));
                }
                
                var user = new ApplicationUser {UserName = elm.Email, Email = elm.Email};

                var result = await _userManager.CreateAsync(user);

                if(result.Succeeded) 
                {
                    result = await _userManager.AddLoginAsync(user, info);

                    if(result.Succeeded) 
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(nameof(ExternalLogin), elm);
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
