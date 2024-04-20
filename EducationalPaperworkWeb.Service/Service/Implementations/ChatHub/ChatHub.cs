using EducationalPaperworkWeb.Domain.Domain.Models.UserEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Globalization;

namespace EducationalPaperworkWeb.Service.Service.Implementations.ChatHub
{
    [Authorize]
    public class ChatHub : Hub
    {

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity?.Name?.ToString(CultureInfo.InvariantCulture));
            await base.OnConnectedAsync();
        }

        public Task SendMessageToChat(long userId, object data)
        {
            return Clients.Group(userId.ToString()).SendAsync("SendMessageToChat", data);
        }
    }
}
