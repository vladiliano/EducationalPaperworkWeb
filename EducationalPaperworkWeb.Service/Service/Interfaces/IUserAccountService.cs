using EducationalPaperworkWeb.Domain.Domain.Models.Response;
using EducationalPaperworkWeb.Domain.Domain.Models.User;
using System.Security.Claims;

namespace EducationalPaperworkWeb.Service.Service.Interfaces
{
    public interface IUserAccountService
    {
        public Task<IBaseResponse<User>> Register(UserSignUp user);
        public Task<IBaseResponse<ClaimsIdentity>> SignIn(UserSignIn user);
        public Task<IBaseResponse<bool>> ChangePassword(UserRestorePassword user);
    }
}
