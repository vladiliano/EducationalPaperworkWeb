using EducationalPaperworkWeb.Domain.Domain.Enums.UserAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Security.Claims;

namespace EducationalPaperworkWeb.Service.Service.Implementations.ChatHub
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        private string? _userId => Context.User.Identity?.Name?.ToString(CultureInfo.InvariantCulture);
        private string? _userRole => Context?.User?.FindFirst(ClaimsIdentity.DefaultRoleClaimType).Value;
        private string _connectionId => Context.ConnectionId;
        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(_connectionId, _userId);

            _logger.LogInformation($"[{DateTime.Now.ToString("HH:mm:ss")}] " +
                $"Клієнт з Id: {_userId} та з Id підключення: {_connectionId} під'єднується до серверу!");

            if (_userRole == Role.Admin.ToString())
            {
                await Groups.AddToGroupAsync(_connectionId, "admins");
                _logger.LogInformation($"[{DateTime.Now.ToString("HH:mm:ss")}] " +
                    $"Клієнт з Id: {_userId} та з Id підключення: {_connectionId} під'єднується до групи [admins]!");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (_userRole == Role.Admin.ToString())
            {
                await Groups.RemoveFromGroupAsync(_connectionId, "admins");
                _logger.LogInformation($"[{DateTime.Now.ToString("HH:mm:ss")}] " +
                    $"Клієнт з Id: {_userId} та з Id підключення: {_connectionId} від'єднується від групи [admins]!");
            }

            await Groups.RemoveFromGroupAsync(_connectionId, _userId);

            if (exception != null)
            {
                var logMessagePart = exception.Message == "UserLoggedOut" ? "від'єднується від серверу!" : "був від'єднаний сервером!";
                _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss")}] Клієнт з Id: {_userId} та з Id підключення: {_connectionId} {logMessagePart}");
            }

            await base.OnDisconnectedAsync(exception);
        }

        public Task SendMessageToChat(long userId, object data) =>
            Clients.Group(userId.ToString()).SendAsync("SendMessageToChat", data);

        public Task AddRequestToTable(object data) =>
            Clients.Group("admins").SendAsync("AddRequestToTable", data);

        public Task RemoveRequestFromTable(object data) =>
            Clients.Group("admins").SendAsync("RemoveRequestFromTable", data);

        public Task UpdateStudentRequest(long userId, object chat) =>
            Clients.Group(userId.ToString()).SendAsync("UpdateStudentRequest", chat);

        public Task SetChatAsReadOnly(long userId) =>
            Clients.Group(userId.ToString()).SendAsync("SetChatAsReadOnly");
    }
}
