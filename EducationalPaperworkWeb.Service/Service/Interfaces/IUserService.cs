using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.UserEntities;

namespace EducationalPaperworkWeb.Service.Service.Interfaces
{
    public interface IUserService
    {
        public IBaseResponse<User> GetUserById(long id);
    }
}
