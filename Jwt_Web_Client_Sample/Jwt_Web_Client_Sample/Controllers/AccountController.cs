using Jwt_Web_Client_Sample.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Jwt_Web_Client_Sample.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Login()
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewBag.data = @"<form action='' method='post'><button type='submit'>login</button></form>";
            
            return View("_view");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string val=null)
        {
            var user = new UserModel() { Email = "tariq.information@gmail.com", Password = "admin" /*, Dob = DateTime.Parse("2010/01/01")*/ };
            var tokenObj = await PostAsync<TokenModel, UserModel>("account/login", user);
            if (string.IsNullOrEmpty(tokenObj.Token) == false)
            {
                Response.Cookies.Append("token", tokenObj.Token, new CookieOptions { Secure=true, Expires = DateTime.UtcNow.AddMinutes(tokenObj.ExpiresIn) });
                HttpContext.Session.SetString(AppData.TokenName, tokenObj.Token);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.GivenName, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.DateOfBirth, user.Dob.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, "dsf", "sdf");

                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. Required when setting the 
                    // ExpireTimeSpan option of CookieAuthenticationOptions 
                    // set with AddCookie. Also required when setting 
                    // ExpiresUtc.

                    IssuedUtc = DateTimeOffset.Now,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
            }

            ViewBag.data = "logged-in";

            return View("_view");
        }

        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete(AppData.TokenName);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // await HttpContext.SignOutAsync("Identity.External");
            // await HttpContext.SignOutAsync();

            ViewBag.data = "logged-out";

            return View("_view");
        }

        public IActionResult LoginStatus()
        {
            ViewBag.data = HttpContext.User.Identity.IsAuthenticated.ToString();

            return View("_view");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError("RemoteError", $"Error from external provider: {remoteError}");
                return RedirectToAction(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info != null)
            {
                return RedirectToAction("login");
            }

            // Sign in the user with this external login provider if the user already has a login.
            //var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            //if (result.Succeeded)
            //{
            //    //_logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
            //    return RedirectToLocal(returnUrl);
            //}
            //if (result.IsLockedOut)
            //{
            //    return Content("locked out");
            //}
            //else
            //{
            //    // If the user does not have an account, then ask the user to create an account.
            //    ViewData["ReturnUrl"] = returnUrl;
            //    ViewData["LoginProvider"] = info.LoginProvider;
            //    var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            //    return Content($"ExternalLoginCallBack: {email}");
            //}

            ViewBag.data = "external login failed/cancelled/...";
            return View("_view");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        // -------NOT IN USE-------
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }

                var userModel = new UserModel();
                userModel.Email = model.Email;
                userModel.Username = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userModel == null)
                {
                    ViewData["LoginProvider"] = info.LoginProvider;
                    var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                    return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
                }

                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        //await AddClaims(userModel.Email, userModel.Dob);
                        await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

                        return RedirectToLocal(returnUrl);
                    }
                }
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        // Event Handler
        public static Task OnExternalLoginDenial(RemoteFailureContext context)
        {
            context.Response.Redirect("/account/login");
            context.HandleResponse();
            return System.Threading.Tasks.Task.FromResult(0);
        }
}
}
