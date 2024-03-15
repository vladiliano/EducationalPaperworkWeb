using EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;

namespace EducationalPaperworkWeb.Service.Service.Interfaces
{
    public interface IChatService
    {
        public Task<IBaseResponse<Message>> SendMessageAsync(Message message);
        public Task<IBaseResponse<List<Message>>> LoadChatAsync(Chat chat);
        public Task<IBaseResponse<Chat>> CreateChatAsync(Chat chat);
    }
}
