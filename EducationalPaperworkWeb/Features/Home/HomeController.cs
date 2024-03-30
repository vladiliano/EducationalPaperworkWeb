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

        public HomeController(ILogger<HomeController> logger, IChatService chatService, IUserService userService)
        {
            _logger = logger;
            _chatService = chatService;
            _userService = userService;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message)
        {
            return View("Error", new ErrorViewModel
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

            var userId = _userService.GetUserId(HttpContext);

            if (userId.StatusCode == OperationStatusCode.OK)
            {
                var chats = await _chatService.GetUserChatsAsync(userId.Data);

                if (chats.StatusCode != OperationStatusCode.InternalServerError)
                {
                    return View(new UserViewModel
                    {
                        UserId = userId.Data,
                        Chats = chats.Data
                    });
                }
            }

            return Error(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> LoadChat(long senderId, long chatId)
        {
            var chats = await _chatService.GetUserChatsAsync(senderId);

            if (chats.StatusCode != OperationStatusCode.OK)
                return Error(nameof(LoadChat) + chats.Description);

            var chat = chats.Data.FirstOrDefault(x => x.Id == chatId);

            if (chat != null && !chat.IsTaken) return NoContent();

            var messages = await _chatService.GetChatMessagesAsync(chatId);

            if (messages.StatusCode == OperationStatusCode.InternalServerError)
                return Error(nameof(LoadChat) + messages.Description);

            var recepient = await _chatService.GetCompanionAsync(senderId, chatId);

            if (recepient.StatusCode == OperationStatusCode.InternalServerError)
                return Error(nameof(LoadChat) + recepient.Description);

            var dataToSend = new
            {
                Messages = messages.Data,
                Companion = $"{recepient.Data.Surname} {recepient.Data.Name} {recepient.Data.Patronymic}"
            };

            return Ok(dataToSend);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(long senderId, long chatId, string mess)
        {
            var messages = await _chatService.CreateMessageAsync(senderId, chatId, mess);

            switch (messages.StatusCode)
            {
                case OperationStatusCode.NoContent: return NoContent();
                case OperationStatusCode.InternalServerError: return Error(nameof(SendMessage) + messages.Description);
                default: return Ok(messages.Data["prevMessage"]);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat(long senderId, string chatName)
        {
            var chat = await _chatService.CreateChatAsync(new Chat
            {
                StudentId = senderId,
                IsTaken = false,
                Name = chatName
            });

            if(chat.StatusCode != OperationStatusCode.OK)
                return Error(nameof(CreateChat) + chat.Description);

            return Ok(chat.Data);
        }
    }
}