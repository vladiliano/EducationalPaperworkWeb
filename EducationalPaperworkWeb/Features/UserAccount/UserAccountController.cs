using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;
using EducationalPaperworkWeb.Domain.Domain.Models.UserEntities;
using EducationalPaperworkWeb.Service.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EducationalPaperworkWeb.Features.UserAccount
{
    public class UserAccountController : Controller
    {
        private readonly ILogger<UserAccountController> _logger;
        private readonly IUserAccountService _service;

        public UserAccountController(ILogger<UserAccountController> logger, IUserAccountService userAccountService)
        {
            _logger = logger;
            _service = userAccountService;
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
            if(ModelState.IsValid)
            {
                var result = await _service.SignIn(user);

                switch (result.StatusCode)
                {
                    case OperationStatusCode.NotFound:
                        ModelState.AddModelError(nameof(user.Email), result.Description);
                        break;
                    case OperationStatusCode.Unauthorized:
                        ModelState.AddModelError(nameof(user.Password), result.Description);
                        break;
                    default:
                        return RedirectToAction(actionName: "Index", controllerName: "Home");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserSignUp user)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.SignUp(user);

                switch (result.StatusCode)
                {
                    case OperationStatusCode.Conflict:
                        ModelState.AddModelError(nameof(user.Email), result.Description);
                        break;
                    default:
                        return RedirectToAction(actionName: "Index", controllerName: "Home");
                }
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> RestorePassword(UserRestorePassword user)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.ChangePassword(user);

                if (result.StatusCode == OperationStatusCode.OK)
                    return RedirectToAction(actionName: "Index", controllerName: "Home");
            }
            return View();
        }
    }
}
