using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Covid19.WebApp.Models;
using Covid19.WebApp.Services;
using Covid19.WebApp.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.WebUtilities;
using System.Web.Providers.Entities;

namespace Covid19.WebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMailService _mailService;
        private readonly IHtmlLocalizer<AccountController> _T;


        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IMailService mailService,
            IHtmlLocalizer<AccountController> T)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
            _T = T;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return View(loginViewModel);

            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginViewModel.ReturnUrl))
                        return RedirectToAction("Index", "Home");

                    return Redirect(loginViewModel.ReturnUrl);
                }
            }

            ModelState.AddModelError("", "Username/password not found");
            return View(loginViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            ViewBag.want = new List<string>() {"Yes i would like", "No i would not like", "I am not sure yet"};
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.want = new List<string>() { "Yes i would like", "No i would not like", "I am not sure yet" };

                var user = new ApplicationUser() { UserName = model.Email,Email = model.Email, wantVaccine = model.wantVaccine};

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)

                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var link = Url.Action(nameof(VerifyMail), "Account", new {userId = user.Id, token = token}, Request.Scheme, Request.Host.ToString());

                    await _mailService.SendEmailAsync(model.Email, "Verify your Account", $"<a href=\"{link}\">Verify email</a>");

                    return View("EmailVerification");
                }
                return View("Error");
            }
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> VerifyMail(string userId, string token)
        {

            var user = await _userManager.FindByIdAsync(userId);

            if (user==null || token==null) 
            {
                return BadRequest();
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return View();
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this._userManager.FindByEmailAsync(viewModel.Input.Email)
                    .ConfigureAwait(false);
                if (user == null)
                {
                    return this.RedirectToAction("NoUser");
                }

                var tokenGenerated = await this._userManager.GeneratePasswordResetTokenAsync(user)
                    .ConfigureAwait(false);
                var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(tokenGenerated));

                var callbackUrl = this.Url.Action("ResetPassword",
                    "Account",
                    new
                    {
                        encodedCode
                    },
                    this.Request.Scheme);

                await _mailService.SendEmailAsync(viewModel.Input.Email,
                        "Reset Password",
                        $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.")
                    .ConfigureAwait(false);

                return this.RedirectToAction("ForgotPasswordConfirmation");
            }

            return this.View();
        }

        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return this.View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult NoUser()
        {
            return this.View();
        }

        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string encodedCode = null)
        {
            if (encodedCode == null)
            {
                return this.BadRequest("A code must be supplied for password reset.");
            }

            var codeDecodedBytes = WebEncoders.Base64UrlDecode(encodedCode);
            var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);

            var viewModel = new ResetPasswordViewModel
            {
                Input = new ResetPasswordInputViewModel(),
                Code = codeDecoded
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var user = await this._userManager.FindByEmailAsync(viewModel.Input.Email)
                .ConfigureAwait(false);
            if (user == null)
            {
                return this.RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await this._userManager.ResetPasswordAsync(user, viewModel.Code, viewModel.Input.Password)
                .ConfigureAwait(false);

            if (result.Succeeded)
            {
                return this.RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }

            return this.View(viewModel);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return this.View();
        }

    }
}
