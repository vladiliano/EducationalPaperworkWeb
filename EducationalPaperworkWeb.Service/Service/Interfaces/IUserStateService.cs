using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;
using EducationalPaperworkWeb.Domain.Domain.ViewModels;

namespace EducationalPaperworkWeb.Service.Service.Interfaces
{
    public interface IUserStateService
    {
        public IBaseResponse<UserViewModel> TryAdd(long userId, List<UserChat> chats);
    }
}
