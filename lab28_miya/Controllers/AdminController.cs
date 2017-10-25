using lab28_miya.Models;
using Microsoft.AspNetCore.Authentication;
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
    [Authorize(Policy="Admin Only")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdminController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = rvm.Email, Email = rvm.Email, FirstName = rvm.FirstName, LastName = rvm.LastName };

                var result = await _userManager.CreateAsync(user, rvm.Password);

                if (result.Succeeded)
                {
                    //const string issuer = "www.miya.com";

                    //creating a list where I can add claims
                    List<Claim> myClaims = new List<Claim>();

                    //this is a claim for the user's name
                    Claim claim1 = new Claim(ClaimTypes.Name, rvm.FirstName + " " + rvm.LastName, ClaimValueTypes.String);
                    myClaims.Add(claim1);

                    //this is a claim for the user's role
                    Claim claim2 = new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String);
                    myClaims.Add(claim2);

                    //this is a claim for the user's date of birth
                    Claim StartDate = new Claim(ClaimTypes.UserData, rvm.StartDate.Date.ToString(), ClaimValueTypes.Date);
                    myClaims.Add(StartDate);

                    var addClaims = await _userManager.AddClaimsAsync(user, myClaims);

                    return RedirectToAction("Index", "Home");

                }
            }
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
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

                if (result.Succeeded)
                {
                    const string issuer = "www.miya.com";

                    //this will create a list for me to add my claims
                    List<Claim> myClaims = new List<Claim>();

                    Claim claim1 = new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName, ClaimValueTypes.String, issuer);
                    myClaims.Add(claim1);

                    Claim claim2 = new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String);
                    myClaims.Add(claim2);

                    Claim StartDate = new Claim(ClaimTypes.UserData, user.StartDate.Date.ToString(), ClaimValueTypes.Date);
                    myClaims.Add(StartDate);

                    var userIdentity = new ClaimsIdentity("Registration");
                    userIdentity.AddClaims(myClaims);

                    var userPrincipal = new ClaimsPrincipal(userIdentity);

                    //user.AddIdentity(userIdentity);

                    await HttpContext.SignInAsync(
                        "MyCookieLogin", userPrincipal, new AuthenticationProperties
                        {
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                            IsPersistent = false,
                            AllowRefresh = false
                        });
                    return RedirectToAction("Index", "Home");
                }
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
