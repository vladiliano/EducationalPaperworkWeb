using EducationalPaperworkWeb.Domain.Domain.Models.Response;
using EducationalPaperworkWeb.Domain.Domain.Models.User;
using System.Security.Claims;

namespace EducationalPaperworkWeb.Service.Service.Interfaces
{
    public interface IUserAccountService
    {
        public Task<IBaseResponse<User>> Register(User user);
        public Task<IBaseResponse<ClaimsIdentity>> LogIn(UserLogIn user);
        public Task<IBaseResponse<bool>> ChangePassword(UserChangePassword user);
    }
}
