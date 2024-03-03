using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;
using EducationalPaperworkWeb.Domain.Domain.Models.Response;
using EducationalPaperworkWeb.Domain.Domain.Models.User;
using EducationalPaperworkWeb.Repository.Repositories.Interfaces;
using EducationalPaperworkWeb.Service.Service.Interfaces;

namespace EducationalPaperworkWeb.Service.Service.Implementations
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IBaseRepository<User> _repository;

        public UserAccountService(IBaseRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<IBaseResponse<User>> ForgotPassword(User user)
        {
            try
            {
                await _repository.Create(user);
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<User>> LogIn(UserLogIn user)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<User>> Register(User user)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
