using EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities;
using EducationalPaperworkWeb.Domain.Domain.ViewModels;
using EducationalPaperworkWeb.Features.Error;
using EducationalPaperworkWeb.Service.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace EducationalPaperworkWeb.Views.Home
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IChatService _chatService;

        public HomeController(ILogger<HomeController> logger, IChatService service)
        {
            _logger = logger;
            _chatService = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("SignIn", "UserAccount");

            if (!long.TryParse(HttpContext.User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType), out long userId))
                return View("Error");

            var chats = await _chatService.GetUserChatsAsync(userId);
            if (chats == null)
                return View("Error");

            var userChats = chats.Data.Select(async chat =>
            {
                var messagesResponse = await _chatService.GetChatMessagesAsync(chat.Id);
                return new UserChat
                {
                    Chat = chat,
                    Messages = messagesResponse?.Data
                };
            });

            var resolvedUserChats = await Task.WhenAll(userChats);

            return View(new UserHomePageViewModel
            {
                Id = userId,
                UserChats = resolvedUserChats.ToList()
            });
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
