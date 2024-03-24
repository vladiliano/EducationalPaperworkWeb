using EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;
using EducationalPaperworkWeb.Domain.Domain.ViewModels;
using Microsoft.AspNetCore.Http;

namespace EducationalPaperworkWeb.Service.Service.Interfaces
{
    public interface IChatService
    {
        public Task<IBaseResponse<Chat>> CreateChatAsync(Chat chat);
        public Task<IBaseResponse<Message>> CreateMessageAsync(long userId, long chatId, string message);
        public Task<IBaseResponse<List<Message>>> GetChatMessagesAsync(long id);
        public Task<IBaseResponse<List<Chat>>> GetUserChatsAsync(long userId);
        public IBaseResponse<long> GetUserId(HttpContext context);
    }
}