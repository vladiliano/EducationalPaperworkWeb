using EducationalPaperworkWeb.Domain.Domain.Enums.Chat;
using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;
using EducationalPaperworkWeb.Domain.Domain.Enums.UserAccount;
using EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.UserEntities;
using EducationalPaperworkWeb.Domain.Domain.ViewModels;
using EducationalPaperworkWeb.Features.Error;
using EducationalPaperworkWeb.Infrastructure.Infrastructure.DataStorage.Interface;
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
        private readonly IUserService _userService;
        private readonly IDataStorage _dataStorage;

        public HomeController(
            ILogger<HomeController> logger, 
            IChatService chatService, 
            IUserService userService, 
            IDataStorage dataStorage)
        {
            _logger = logger;
            _chatService = chatService;
            _userService = userService;
            _dataStorage = dataStorage;
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

        private long? GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
            if (!string.IsNullOrEmpty(userIdClaim) && long.TryParse(userIdClaim, out long userId))
                return userId;
            return null;
        }

        private Role? GetUserRole()
        {
            var userRoleClaim = User.FindFirstValue(ClaimsIdentity.DefaultRoleClaimType);
            if (!string.IsNullOrEmpty(userRoleClaim) && Enum.TryParse(userRoleClaim, out Role userRole))
                return userRole;
            return null;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
                return RedirectToAction("SignIn", "UserAccount");

            var userId = GetUserId();
            var userRole = GetUserRole();

            if (userId != null && userRole != null)
            {
                var chats = await _chatService.GetUserChatsAsync(userId.Value);

                if (chats.StatusCode != OperationStatusCode.InternalServerError)
                {
                    return View(new UserViewModel
                    {
                        UserId = userId.Value,
                        UserRole = userRole.Value,
                        Chats = chats.Data
                    });
                }
            }

            return Error(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> LoadChat(long userId, long chatId)
        {
            var chats = await _chatService.GetUserChatsAsync(userId);

            if (chats.StatusCode != OperationStatusCode.OK)
                return Error(nameof(LoadChat) + chats.Description);

            var chat = chats.Data.FirstOrDefault(x => x.Id == chatId);

            if (chat != null && !chat.IsTaken) return NoContent();

            var messages = await _chatService.GetChatMessagesAsync(chatId);

            if (messages.StatusCode == OperationStatusCode.InternalServerError)
                return Error(nameof(LoadChat) + messages.Description);

            var recepient = await _chatService.GetCompanionAsync(userId, chatId);

            if (recepient.StatusCode == OperationStatusCode.InternalServerError)
                return Error(nameof(LoadChat) + recepient.Description);

            var result = new
            {
                Messages = messages.Data,
                Companion = $"{recepient.Data.Name} {recepient.Data.Patronymic} {recepient.Data.Surname}"
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat(long userId, string chatName)
        {
            var chat = await _chatService.CreateChatAsync(new Chat
            {
                StudentId = userId,
                IsTaken = false,
                Name = chatName
            });

            if(chat.StatusCode != OperationStatusCode.OK)
                return Error(nameof(CreateChat) + chat.Description);

            return Ok(chat.Data);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(long userId, long chatId, string mess)
        {
            var message = await _chatService.CreateMessageAsync(userId, chatId, mess, MessageContentType.Text);

            if (message.StatusCode == OperationStatusCode.OK)
            {
                var previousMessage = _chatService.GetPreviousMessage(chatId, message.Data);

                if (previousMessage.StatusCode != OperationStatusCode.InternalServerError)
                {
                    var result = new
                    {
                        Content = message.Data.Content,
                        TimeStamp = message.Data.TimeStamp,
                        PreviousMessageTimeStamp = previousMessage.StatusCode == OperationStatusCode.NoContent
                            ? DateTime.UtcNow.AddDays(-2)
                            : previousMessage.Data.TimeStamp
                    };
                    return Ok(result);
                }
            }
            return Error(nameof(SendMessage) + message.Description);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(long userId, long chatId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Error(nameof(UploadFile) + "Файл не було передано або його розмір дорівнює нулю.");

            var fileResult = await _dataStorage.UploadFileAsync(file);

            if(fileResult.StatusCode != OperationStatusCode.OK)
                return Error(nameof(UploadFile) + fileResult.Description);

            var message = await _chatService.CreateMessageAsync(userId, chatId, fileResult.Data, MessageContentType.File);

            if (message.StatusCode != OperationStatusCode.OK)
                return Error(nameof(UploadFile) + message.Description);

            var previousMessage = _chatService.GetPreviousMessage(chatId, message.Data);

            if (previousMessage.StatusCode != OperationStatusCode.InternalServerError)
            {
                var result = new
                {
                    Content = message.Data.Content,
                    TimeStamp = message.Data.TimeStamp,
                    PreviousMessageTimeStamp = previousMessage.StatusCode == OperationStatusCode.NoContent
                        ? DateTime.UtcNow.AddDays(-2)
                        : previousMessage.Data.TimeStamp
                };
                return Ok(result);
            }

            return Error(nameof(UploadFile) + previousMessage.Description);
        }

        [HttpPost]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var file = await _dataStorage.GetFileAsync(fileName);

            if (file.StatusCode != OperationStatusCode.OK)
                return Error(nameof(DownloadFile) + file.Description);

            return File(file.Data.Content, file.Data.Mime, file.Data.Name);
        }

        [HttpGet]
        public async Task<IActionResult> GetUnacceptedRequests()
        {
            var chats = await _chatService.GetUnselectedChats();

            if (chats.StatusCode == OperationStatusCode.InternalServerError)
                return Error(nameof(GetUnacceptedRequests) + chats.Description);

            if (chats.StatusCode == OperationStatusCode.NoContent)
                return NoContent();

            var usersWithChats = new List<Tuple<User,Chat>>(chats.Data.Count);

            var tasks = chats.Data
                .OrderBy(chat => chat.TimeStamp)
                .Select(async chat =>
                {
                    var user = await _userService.GetUserAsync(chat.StudentId);
                    return (user.StatusCode == OperationStatusCode.OK) 
                    ? Tuple.Create(user.Data, chat) : null;
                });

            foreach (var task in tasks)
            {
                var result = await task;
                if (result != null) usersWithChats.Add(result);
            }


            return Ok(usersWithChats);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptRequest(long chatId, long adminId)
        {
            var chat = await _chatService.AcceptRequest(chatId, adminId);

            if (chat.StatusCode == OperationStatusCode.InternalServerError)
                return Error(nameof(AcceptRequest) + chat.Description);

            if (chat.StatusCode == OperationStatusCode.NoContent)
                return NoContent();

            return Ok(chat.Data);
        }
    }
}