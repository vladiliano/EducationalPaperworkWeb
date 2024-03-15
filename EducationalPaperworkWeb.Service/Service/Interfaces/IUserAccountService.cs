using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.UserEntities;
using System.Security.Claims;

namespace EducationalPaperworkWeb.Service.Service.Interfaces
{
    public interface IUserAccountService
    {
        public Task<IBaseResponse<User>> SignUpAsync(UserSignUp user);
        public Task<IBaseResponse<ClaimsIdentity>> SignInAsync(UserSignIn user);
        public Task<IBaseResponse<bool>> ChangePasswordAsync(UserRestorePassword user);
    }
}
