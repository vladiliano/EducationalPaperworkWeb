using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;
using EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.UserEntities;
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
        private readonly IUserService _userService;
        private static Dictionary<long, UserViewModel> _usersState = new();

        public HomeController(ILogger<HomeController> logger, IChatService chatService, IUserService userService)
        {
            _logger = logger;
            _chatService = chatService;
            _userService = userService;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message)
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = message
            });
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("SignIn", "UserAccount");

            var userId = _chatService.GetUserId(HttpContext);

            if (userId.StatusCode == OperationStatusCode.OK)
            {
                var chats = await _chatService.GetUserChatsAsync(userId.Data);

                if (chats.StatusCode != OperationStatusCode.InternalServerError)
                {
                    _usersState.TryAdd(userId.Data, new UserViewModel
                    {
                        Id = userId.Data,
                        UserChats = chats.Data.Select(chat => new UserChat
                        {
                            Chat = chat
                        })
                          .OrderBy(x => x.Chat.TimeStamp)
                          .ToList()
                    });

                    return View(_usersState[userId.Data]);
                }
            }

            return Error(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> LoadChat(long chatId)
        {
            var messages = await _chatService.GetChatMessagesAsync(chatId);

            switch (messages.StatusCode)
            {
                case OperationStatusCode.NoContent: return NoContent();
                case OperationStatusCode.InternalServerError: return Error(nameof(LoadChat) + messages.Description);
                default: return Ok(messages.Data);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(long senderId, long chatId, string mess)
        {
            var message = await _chatService.CreateMessageAsync(senderId, chatId, mess);

            switch (message.StatusCode)
            {
                case OperationStatusCode.NoContent: return NoContent();
                case OperationStatusCode.InternalServerError: return Error(nameof(SendMessage) + message.Description);
                default: return Ok();
            }
        }
    }
}