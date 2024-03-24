using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;
using EducationalPaperworkWeb.Domain.Domain.Models.UserEntities;
using EducationalPaperworkWeb.Service.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EducationalPaperworkWeb.Features.UserAccount
{
    public class UserAccountController : Controller
    {
        private readonly ILogger<UserAccountController> _logger;
        private readonly IUserAccountService _service;
        private readonly IChatService _chatService;

        public UserAccountController(ILogger<UserAccountController> logger, IUserAccountService userAccountService, IChatService chatService)
        {
            _logger = logger;
            _service = userAccountService;
            _chatService = chatService;
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
    }
}
