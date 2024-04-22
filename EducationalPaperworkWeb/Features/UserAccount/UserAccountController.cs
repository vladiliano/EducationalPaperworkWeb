using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;
using EducationalPaperworkWeb.Domain.Domain.Models.UserEntities;
using EducationalPaperworkWeb.Service.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EducationalPaperworkWeb.Service.Service.Implementations.ChatHub;

namespace EducationalPaperworkWeb.Features.UserAccount
{
    public class UserAccountController : Controller
    {
        private readonly ILogger<UserAccountController> _logger;
        private readonly IUserAccountService _service;
        private readonly ChatHub _hubManager;

        public UserAccountController(
            ILogger<UserAccountController> logger, 
            IUserAccountService userAccountService,
            ChatHub hubManager)
        {
            _logger = logger;
            _service = userAccountService;
            _hubManager = hubManager;
        }

        [HttpGet]
        public IActionResult SignIn() => View();

        [HttpGet]
        public IActionResult SignUp() => View();

        [HttpGet]
        public IActionResult RestorePassword() => View();

        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignIn user)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.SignInAsync(user);

                switch (result.StatusCode)
                {
                    case OperationStatusCode.NotFound:
                        ModelState.AddModelError(nameof(user.Email), result.Description);
                        break;
                    case OperationStatusCode.Unauthorized:
                        ModelState.AddModelError(nameof(user.Password), result.Description);
                        break;
                    default:
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(result.Data));

                        return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserSignUp user)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.SignUpAsync(user);

                switch (result.StatusCode)
                {
                    case OperationStatusCode.Conflict:
                        ModelState.AddModelError(nameof(user.Email), result.Description);
                        break;
                    default:
                        return RedirectToAction(actionName: "SignIn", controllerName: "UserAccount");
                }
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> RestorePassword(UserRestorePassword user)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.ChangePasswordAsync(user);

                if (result.StatusCode == OperationStatusCode.OK)
                    return RedirectToAction(actionName: "SignIn", controllerName: "UserAccount");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _hubManager.OnDisconnectedAsync(new Exception("UserLoggedOut"));
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("SignIn");
        }
    }
}
