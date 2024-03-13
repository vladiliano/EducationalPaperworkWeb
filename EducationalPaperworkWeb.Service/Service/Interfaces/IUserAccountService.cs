using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.UserEntities;
using System.Security.Claims;

namespace EducationalPaperworkWeb.Service.Service.Interfaces
{
    public interface IUserAccountService
    {
        public Task<IBaseResponse<User>> SignUp(UserSignUp user);
        public Task<IBaseResponse<ClaimsIdentity>> SignIn(UserSignIn user);
        public Task<IBaseResponse<bool>> ChangePassword(UserRestorePassword user);
    }
}
