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
        private readonly string _uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

        public HomeController(ILogger<HomeController> logger, IChatService chatService, IUserService userService)
        {
            _logger = logger;
            _chatService = chatService;
            _userService = userService;
            Directory.CreateDirectory(_uploadDirectory);
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
        public IActionResult AdminDashboard() => View();

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("SignIn", "UserAccount");

            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType)?.Value;

            if (userIdClaim != null)
            {
                if (long.TryParse(userIdClaim, out var userId))
                {
                    var chats = await _chatService.GetUserChatsAsync(userId);

                    if (chats.StatusCode != OperationStatusCode.InternalServerError)
                    {
                        return View(new UserViewModel
                        {
                            UserId = userId,
                            Chats = chats.Data
                        });
                    }
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

            var result = new
            {
                Messages = messages.Data,
                Companion = $"{recepient.Data.Name} {recepient.Data.Patronymic} {recepient.Data.Surname}"
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(long senderId, long chatId, string mess)
        {
            var messages = await _chatService.CreateMessageAsync(senderId, chatId, mess);

            if (messages.StatusCode == OperationStatusCode.OK)
            {
                messages.Data.Dequeue();

                if (messages.Data.TryDequeue(out var result)) 
                    return Ok(result);

                return NoContent();
            }
            return Error(nameof(SendMessage) + messages.Description);
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

        [HttpPost]
        public async Task<IActionResult> SendFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Error(nameof(SendFile) + "Не вдалося отримати файл");

            try
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                var filePath = Path.Combine(_uploadDirectory, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return Error(nameof(SendFile) + ex.Message);
            }
        }
    }
}