using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;
using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.UserEntities;
using EducationalPaperworkWeb.Domain.Domain.ViewModels;
using EducationalPaperworkWeb.Service.Service.Interfaces;

namespace EducationalPaperworkWeb.Service.Service.Implementations
{
    public class UserStateService : IUserStateService
    {
        private static Dictionary<long, UserViewModel> _usersState = new();

        public IBaseResponse<UserViewModel> TryAdd(long userId, List<UserChat> chats)
        {
            try
            {
                if (chats == null) throw new Exception("Чатів не існує!");

                var user = new UserViewModel
                {
                    Id = userId,
                    UserChats = chats
                };

                if(!_usersState.TryAdd(userId, user))
                {
                    return new BaseResponse<UserViewModel>()
                    {
                        Description = "Користувача не було додано!",
                        StatusCode = OperationStatusCode.OK,
                    };
                }    

                return new BaseResponse<UserViewModel>()
                {
                    Description = "Користувача додано успішно!",
                    StatusCode = OperationStatusCode.OK,
                    Data = user
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserViewModel>()
                {
                    Description = nameof(TryAdd) + ": " + ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError
                };
            }
        }
    }
}
