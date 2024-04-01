using EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.UserEntities;

namespace EducationalPaperworkWeb.Service.Service.Interfaces
{
    public interface IChatService
    {
        public Task<IBaseResponse<Chat>> CreateChatAsync(Chat chat);
        public Task<IBaseResponse<Queue<Message>>> CreateMessageAsync(long userId, long chatId, string message);
        public Task<IBaseResponse<List<Message>>> GetChatMessagesAsync(long id);
        public Task<IBaseResponse<List<Chat>>> GetUserChatsAsync(long userId);
        public Task<IBaseResponse<User>> GetCompanionAsync(long userId, long chatId);
    }
}