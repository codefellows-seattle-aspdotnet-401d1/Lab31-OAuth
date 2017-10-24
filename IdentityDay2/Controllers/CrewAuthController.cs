using IdentityDay2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityDay2.Controllers
{
    //Handles Crew login and registration as well as admin creation
    public class CrewAuthController : Controller
    {
        private readonly UserManager<CrewMember> _userManager;
        private readonly SignInManager<CrewMember> _signInManager;

        public CrewAuthController(UserManager<CrewMember> usermanager, SignInManager<CrewMember> signInManager)
        {
            _userManager = usermanager;
            _signInManager = signInManager;
        }

        //Regular user register form
        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //Register regular user
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel rvm, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new CrewMember { UserName = rvm.Email, Email = rvm.Email, Rank = rvm.Rank, Department = rvm.Department };
                var result = await _userManager.CreateAsync(user, rvm.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    List<Claim> memberClaims = new List<Claim>();

                    Claim memberDept = new Claim(ClaimTypes.Role, rvm.Department.ToString(), ClaimValueTypes.String);
                    memberClaims.Add(memberDept);

                    var addClaims = await _userManager.AddClaimsAsync(user, memberClaims);

                    if (addClaims.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View();
        }

        //Admin Registration form
        [HttpGet]
        public IActionResult RegisterAdmin(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //Register Admin user
        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(RegisterViewModel avm, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new CrewMember { UserName = avm.Email, Email = avm.Email, Rank = avm.Rank, Department = avm.Department };
                var result = await _userManager.CreateAsync(user, avm.Password);

                if (result.Succeeded)
                {
                    List<Claim> memberClaims = new List<Claim>();

                    Claim admin = new Claim(ClaimTypes.Role, "Admin", ClaimValueTypes.String);
                    memberClaims.Add(admin);

                    var addClaims = await _userManager.AddClaimsAsync(user, memberClaims);


                    if (addClaims.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View();
        }

        //Login for all users
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(lvm.Email, lvm.Password, lvm.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            string error = "Unable to log you in.  Please try again or contact your system admin.";
            ModelState.AddModelError("", error);
            return View();
        }

        public IActionResult ExternalLogin(string provider, string returnURL = null)
        {
            var redirectURL = Url.Action(nameof(ExternalLoginCallback), "CrewAuth", new { returnURL });
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

                var user = new CrewMember { UserName = elm.Email, Email = elm.Email };

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
