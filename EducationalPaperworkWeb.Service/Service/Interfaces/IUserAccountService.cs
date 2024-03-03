using EducationalPaperworkWeb.Domain.Domain.Models.Response;
using EducationalPaperworkWeb.Domain.Domain.Models.User;

namespace EducationalPaperworkWeb.Service.Service.Interfaces
{
    public interface IUserAccountService
    {
        public Task<IBaseResponse<User>> Register(User user);
        public Task<IBaseResponse<User>> LogIn(UserLogIn user);
        public Task<IBaseResponse<User>> ForgotPassword(User user);
    }
}
