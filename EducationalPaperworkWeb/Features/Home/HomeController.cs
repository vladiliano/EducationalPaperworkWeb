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

        public async Task<IActionResult> Index()
        {
            var id = HttpContext.User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);

            if (!long.TryParse(id, out long userId))
                throw new Exception("Failed to parse user id.");

            var chats = await _chatService.GetUserChatsAsync(userId);

            var userChats = new List<UserChat>(chats.Data.Count);

            foreach (var chat in chats.Data)
            {
                var messages = await _chatService.GetChatMessagesAsync(chat.Id);
                userChats.Add(new UserChat
                {
                    Chat = chat,
                    Messages = messages.Data
                });
            }

            return View(new UserHomePageViewModel
            {
                Id = userId,
                UserChats = userChats
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
