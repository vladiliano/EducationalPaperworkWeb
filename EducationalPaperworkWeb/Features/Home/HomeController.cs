using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;
using EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities;
using EducationalPaperworkWeb.Domain.Domain.ViewModels;
using EducationalPaperworkWeb.Features.Error;
using EducationalPaperworkWeb.Service.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Security.Claims;

namespace EducationalPaperworkWeb.Views.Home
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IChatService _chatService;

        private static UserHomePageViewModel _userHomePageState;

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

            if (!long.TryParse(HttpContext.User.FindFirstValue("UserId"), out long userId))
                return View("Error");

            var chats = await _chatService.GetUserChatsAsync(userId);
            if (chats.StatusCode != OperationStatusCode.OK)
                return View("Error");

            List<UserChat> userChats = new List<UserChat>(chats.Data.Count);

            foreach (var chat in chats.Data)
            {
                //var messagesResponse = await _chatService.GetChatMessagesAsync(chat.Id);

                //if (messagesResponse.StatusCode == OperationStatusCode.InternalServerError)
                //    return View("Error");

                userChats.Add(new UserChat
                {
                    Chat = chat,
                    //Messages = messagesResponse.Data
                });
            }

            _userHomePageState = new UserHomePageViewModel
            {
                Id = userId,
                UserChats = userChats.OrderBy(x => x.Chat.TimeStamp).ToList()
            };

            return View(_userHomePageState);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string message)
        {
            return View(_userHomePageState);
		}

        [HttpPost]
        public async Task<IActionResult> LoadChat(long chatId)
        {
            var messages = await _chatService.GetChatMessagesAsync(chatId);

            if (messages.StatusCode != OperationStatusCode.InternalServerError)
            {
                if (_userHomePageState != null)
                {
                    var chat = _userHomePageState.UserChats.FirstOrDefault(x => x.Chat.Id == chatId);

                    if (chat != null)
                    {
                        chat.Messages = messages.Data;
                        _userHomePageState.SelectedChatId = chatId;
                    }
                }
            }

            return View("Index", _userHomePageState);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
