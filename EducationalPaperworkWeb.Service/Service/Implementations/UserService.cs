using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;
using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.UserEntities;
using EducationalPaperworkWeb.Repository.Repository.Interfaces.UnitOfWork;
using EducationalPaperworkWeb.Service.Service.Helpers.Hashing;
using EducationalPaperworkWeb.Service.Service.Interfaces;
using System.Security.Claims;

namespace EducationalPaperworkWeb.Service.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _repository;

        public UserService(IUnitOfWork repository)
        {
            _repository = repository;
        }
        public IBaseResponse<User> GetUserById(long id)
        {
            try
            {
                var user = _repository.UserRepository.GetAll().FirstOrDefault(x => x.Id == id);

                if (user == null)
                {
                    return new BaseResponse<User>()
                    {
                        Description = "Користувача з таким id не знайдено!",
                        StatusCode = OperationStatusCode.NotFound
                    };
                }

                return new BaseResponse<User>()
                {
                    Description = "Користувача знайдено!",
                    StatusCode = OperationStatusCode.OK,
                    Data = user
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = nameof(GetUserById) + ": " + ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError
                };
            }
        }
    }
}
