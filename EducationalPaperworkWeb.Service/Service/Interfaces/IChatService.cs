using EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;

namespace EducationalPaperworkWeb.Service.Service.Interfaces
{
    public interface IChatService
    {
		public Task<IBaseResponse<Chat>> CreateChatAsync(Chat chat);
		public Task<IBaseResponse<Message>> SendMessageAsync(Message message);
        public Task<IBaseResponse<List<Message>>> GetChatMessagesAsync(long id);
        public Task<IBaseResponse<List<Chat>>> GetUserChatsAsync(long userId);
    }
}
