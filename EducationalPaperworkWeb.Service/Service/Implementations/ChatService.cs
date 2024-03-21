using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;
using EducationalPaperworkWeb.Domain.Domain.Models.ChatEntities;
using EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities;
using EducationalPaperworkWeb.Repository.Repository.Interfaces.UnitOfWork;
using EducationalPaperworkWeb.Service.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationalPaperworkWeb.Service.Service.Implementations
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _repository;

        public ChatService(IUnitOfWork repository)
        {
            _repository = repository;
        }

        public async Task<IBaseResponse<Chat>> CreateChatAsync(Chat chat)
        {
            try
            {
                if (chat == null) throw new Exception($"{nameof(CreateChatAsync)}: {nameof(chat)} == {chat}");

                chat.TimeStamp = DateTime.Now;

                await _repository.ChatRepository.CreateAsync(chat);

                var existingChat = _repository.ChatRepository.GetAll()
                    .OrderBy(x => x.TimeStamp)
                    .Last();

                if (existingChat == null) throw new Exception($"{nameof(CreateChatAsync)}: {nameof(existingChat)} == {existingChat}");

                return new BaseResponse<Chat>()
                {
                    Description = "Чат був успішно завантажений!",
                    StatusCode = OperationStatusCode.OK,
                    Data = existingChat
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Chat>()
                {
                    Description = ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<Message>> CreateMessageAsync(Message message)
        {
            try
            {
                if (message == null) throw new Exception($"{nameof(CreateChatAsync)}: {nameof(message)} == {message}");

                message.TimeStamp = DateTime.Now;

                await _repository.MessageRepository.CreateAsync(message);

                var existingMessage = _repository.MessageRepository.GetAll()
                    .OrderBy(x => x.TimeStamp).Last();

                if (existingMessage == null) throw new Exception($"{nameof(CreateChatAsync)}: {nameof(existingMessage)} == {existingMessage}");

                return new BaseResponse<Message>()
                {
                    Description = "Повідомлення було успішно завантажено!",
                    StatusCode = OperationStatusCode.OK,
                    Data = existingMessage
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Message>()
                {
                    Description = ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<List<Message>>> GetChatMessagesAsync(long chatId)
        {
            try
            {
                var messages = await _repository.MessageRepository.GetAll()
                    .Where(x => x.ChatId == chatId)
                    .OrderBy(x => x.TimeStamp)
                    .ToListAsync();

                if (messages == null || messages.Count == 0)
                {
                    return new BaseResponse<List<Message>>()
                    {
                        StatusCode = OperationStatusCode.NotFound,
                        Description = "Повідомлень в чаті не знайдено!",
                    };
                }

                return new BaseResponse<List<Message>>()
                {
                    Description = "Чат був успішно завантажений!",
                    StatusCode = OperationStatusCode.OK,
                    Data = messages
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Message>>()
                {
                    Description = ex.Message,
                    StatusCode = OperationStatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<List<Chat>>> GetUserChatsAsync(long userId)
        {
			try
			{
                var chats = await _repository.ChatRepository.GetAll()
                    .Where(x => x.StudentId == userId || x.AdminId == userId)
                    .OrderByDescending(x => x.TimeStamp)
                    .ToListAsync();

                if (chats == null || chats.Count == 0)
				{
					return new BaseResponse<List<Chat>>()
					{
						StatusCode = OperationStatusCode.NotFound,
						Description = "Повідомлень в чаті не знайдено!",
					};
				}

				return new BaseResponse<List<Chat>>()
				{
					Description = "Чат був успішно завантажений!",
					StatusCode = OperationStatusCode.OK,
					Data = chats
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<List<Chat>>()
				{
					Description = ex.Message,
					StatusCode = OperationStatusCode.InternalServerError,
				};
			}
		}
    }
}
