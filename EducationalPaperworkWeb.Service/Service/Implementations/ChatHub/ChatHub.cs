using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace EducationalPaperworkWeb.Service.Service.Implementations.ChatHub
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        private string? _userId => Context.User.Identity?.Name?.ToString(CultureInfo.InvariantCulture);
        private string _connectionId => Context.ConnectionId;
        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(_connectionId, _userId);
            await base.OnConnectedAsync();
            _logger.LogInformation($"[{DateTime.Now.ToString("HH:mm:ss")}] Клієнт з Id: {_userId} та з Id підключення: {_connectionId} під'єднується до серверу!");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(_connectionId, _userId);
            await base.OnDisconnectedAsync(exception);

            if (exception != null)
            {
                var logMessagePart = exception.Message == "UserLoggedOut" ? "від'єднується від серверу!" : "був від'єднаний сервером!";
                _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss")}] Клієнт з Id: {_userId} та з Id підключення: {_connectionId} {logMessagePart}");
            }
        }

        public Task SendMessageToChat(long userId, object data) =>
            Clients.Group(userId.ToString()).SendAsync("SendMessageToChat", data);
    }
}
