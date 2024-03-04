using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;
using EducationalPaperworkWeb.Domain.Domain.Models.User;
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
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(UserLogIn user)
        {
            if(ModelState.IsValid)
            {
                var result = await _service.LogIn(user);

                if(result.StatusCode == OperationStatusCode.OK)
                    return RedirectToAction(actionName: "Index", controllerName: "Home");
            }
            return View();
        }
    }
}
