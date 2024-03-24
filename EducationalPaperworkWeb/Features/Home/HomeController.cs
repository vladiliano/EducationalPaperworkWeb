using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;
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

            if (messages.StatusCode != OperationStatusCode.InternalServerError)
            {
                var userId = _chatService.GetUserId(HttpContext);

                var chat = _usersState[userId.Data].UserChats.FirstOrDefault(x => x.Chat.Id == chatId);

                if (chat != null)
                {
                    chat.Messages = messages.Data;
                    _usersState[userId.Data].SelectedChatId = chatId;

                    return View("Index", _usersState[userId.Data]);
                }
            }

            return Error(nameof(LoadChat));
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string message)
        {
            var userId = _chatService.GetUserId(HttpContext);

            if (userId.StatusCode == OperationStatusCode.OK)
            {
                var result = await _chatService.CreateMessageAsync(userId.Data, message, _usersState[userId.Data]);

                if(result.StatusCode != OperationStatusCode.InternalServerError)
                {
                    return View("Index", _usersState[userId.Data]);
                }
            }
            return Error(nameof(LoadChat));
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateChat()
        //{

        //}
    }
}
//await _chatService.CreateMessageAsync(new Message
//{
//    ChatId = 2,
//    SenderId = 3,
//    RecipientId = 1,
//    TimeStamp = new DateTime(2024, 3, 19, 9,25,0),
//    Content = "Доброго дня!"
//});
//await _chatService.CreateMessageAsync(new Message
//{
//    ChatId = 2,
//    SenderId = 1,
//    RecipientId = 3,
//    TimeStamp = new DateTime(2024, 3, 20, 12, 38, 0),
//    Content = "Доброго!"
//});
//await _chatService.CreateMessageAsync(new Message
//{
//    ChatId = 2,
//    SenderId = 3,
//    RecipientId = 1,
//    TimeStamp = new DateTime(2024, 3, 20, 13, 21, 0),
//    Content = "Вам треба оформити документ про те що Ви являєтесь студентом який навчається в нашому університеті, чи якесь інше питання вирішити?"
//});
//await _chatService.CreateMessageAsync(new Message
//{
//    ChatId = 2,
//    SenderId = 1,
//    RecipientId = 3,
//    TimeStamp = new DateTime(2024, 3, 20, 13, 34, 0),
//    Content = "Взагалі то я по іншому питаню."
//});
//await _chatService.CreateMessageAsync(new Message
//{
//    ChatId = 2,
//    SenderId = 3,
//    RecipientId = 1,
//    TimeStamp = new DateTime(2024, 3, 20, 13, 57, 0),
//    Content = "По якому тоді? Бо в темі чату Ви нічого не вказали."
//});
//await _chatService.CreateMessageAsync(new Message
//{
//    ChatId = 2,
//    SenderId = 1,
//    RecipientId = 3,
//    TimeStamp = new DateTime(2024, 3, 20, 15, 37, 0),
//    Content = "Мені треба дистанційно отримати диплом."
//});
//await _chatService.CreateMessageAsync(new Message
//{
//    ChatId = 2,
//    SenderId = 3,
//    RecipientId = 1,
//    TimeStamp = new DateTime(2024, 3, 20, 15, 59, 0),
//    Content = "Це не до нас Вам звертатись треба, пишіть на пошту НТУ ХПІ Вашої кафедри."
//});
//await _chatService.CreateMessageAsync(new Message
//{
//    ChatId = 2,
//    SenderId = 1,
//    RecipientId = 3,
//    TimeStamp = new DateTime(2024, 3, 20, 16, 7, 0),
//    Content = "Добре, дякую за відповідь."
//});