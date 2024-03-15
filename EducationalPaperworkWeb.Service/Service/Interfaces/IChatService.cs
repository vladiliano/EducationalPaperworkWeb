using EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;

namespace EducationalPaperworkWeb.Service.Service.Interfaces
{
    public interface IChatService
    {
        public Task<IBaseResponse<Message>> SendMessage(Message message);
        public Task<IBaseResponse<bool>> LoadChat(long chatId);
    }
}
